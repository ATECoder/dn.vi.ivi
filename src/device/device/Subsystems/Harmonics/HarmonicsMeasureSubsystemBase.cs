using cc.isr.Std.EscapeSequencesExtensions;

namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a Harmonics Measure Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class HarmonicsMeasureSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="HarmonicsMeasureSubsystemBase" /> class.
    /// </summary>
    /// <remarks>   David, 2020-07-28. </remarks>
    /// <param name="statusSubsystem">  A reference to a <see cref="StatusSubsystemBase">status
    ///                                 subsystem</see>. </param>
    /// <param name="readingAmounts">   The reading amounts. </param>
    protected HarmonicsMeasureSubsystemBase( StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem )
    {
        this.ReadingAmounts = readingAmounts;
        this.DefaultMeasurementUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionRange = Ranges.NonnegativeFullRange;
        this.DefaultMeasureModeDecimalPlaces = 3;
        this.GeneratorOutputLevelRange = new Std.Primitives.RangeR( double.MinValue, double.MaxValue );
        this.GeneratorTimerRange = new Std.Primitives.RangeI( int.MinValue, int.MaxValue );
        this.VoltmeterHighLimitRange = new Std.Primitives.RangeR( double.MinValue, double.MaxValue );
        this.VoltmeterLowLimitRange = new Std.Primitives.RangeR( double.MinValue, double.MaxValue );

        this.FunctionUnit = this.DefaultFunctionUnit;
        this.FunctionRange = this.DefaultFunctionRange;
        this.FunctionRangeDecimalPlaces = this.DefaultMeasureModeDecimalPlaces;
        this.MeasureModeDecimalPlaces = [];
        this.DefineMeasureModeDecimalPlaces();
        this.MeasureModeReadWrites = [];
        this.DefineMeasureModeReadWrites();
        this.MeasureModeRanges = [];
        this.DefineMeasureModeRanges();
        this.MeasureModeUnits = [];
        this.DefineMeasureModeUnits();

        this._failureLongDescription = string.Empty;
        this._failureShortDescription = string.Empty;
        this._failureCode = string.Empty;
        this.FetchCommand = string.Empty;
        this.NewBufferReadingsQueue = [];

    }

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the clear execution state (CLS) by setting system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    public override void DefineClearExecutionState()
    {
        base.DefineClearExecutionState();
        this.DefineFunctionClearKnownState();
        this.NotifyPropertyChanged( nameof( this.ReadingAmounts ) );
    }

    /// <summary>   Sets the known initial post reset state. </summary>
    /// <remarks>   Customizes the reset state. </remarks>
    public override void InitKnownState()
    {
        base.InitKnownState();
        _ = this.ParsePrimaryReading( string.Empty );
    }

    #endregion

    #region " query management "

    /// <summary>   Gets or sets the ready to query timeout. </summary>
    /// <value> The ready to query timeout. </value>
    public TimeSpan ReadyToQueryTimeout { get; set; } = TimeSpan.FromMilliseconds( 100 );

    /// <summary>   Gets or sets the ready to read timeout. </summary>
    /// <value> The ready to read timeout. </value>
    public TimeSpan ReadyToReadTimeout { get; set; } = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the ready to write timeout. </summary>
    /// <value> The ready to write timeout. </value>
    public TimeSpan ReadyToWriteTimeout { get; set; } = TimeSpan.FromMilliseconds( 20 );

    #endregion

    #region " clear, trigger, fetch, fetch, measure "

    /// <summary>   Gets or sets the 'clear known state' command. </summary>
    /// <value> The 'clear known state' command. </value>
    protected virtual string ClearKnownStateCommand { get; set; } = string.Empty;

    /// <summary>   Gets or sets the clear known state refractory time span. </summary>
    /// <value> The clear known state refractory time span. </value>
    public abstract TimeSpan ClearKnownStateRefractoryTimeSpan { get; set; }

    /// <summary>   Clears the known state. </summary>
    /// <remarks>   David, 2021-03-30. </remarks>
    /// <returns>   <see cref="ExecuteInfo"/>. </returns>
    public virtual ExecuteInfo ClearKnownState()
    {
        this.DefineClearExecutionState();
        if ( string.IsNullOrWhiteSpace( this.ClearKnownStateCommand ) )
            return ExecuteInfo.Empty;

        ExecuteInfo executeInfo = new( this.Session.WriteLineElapsedIfDefined( this.ClearKnownStateCommand ) );
        _ = Pith.SessionBase.AsyncDelay( this.ClearKnownStateRefractoryTimeSpan );
        return executeInfo;
    }

    /// <summary>   Gets or sets the Trigger command. </summary>
    /// <remarks>   SCPI: '*TRG'. </remarks>
    /// <value> The Trigger command. </value>
    protected virtual string TriggerCommand { get; set; } = string.Empty;

    /// <summary>   Triggers a single short. </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   <see cref="ExecuteInfo"/>. </returns>
    public virtual ExecuteInfo AssertTrigger( bool checkStatus = false )
    {
        return checkStatus
            ? this.Session.ExecuteStatusReady( this.ReadyToQueryTimeout, this.TriggerCommand )
            : new( this.Session.WriteLineElapsedIfDefined( this.TriggerCommand ) );
    }

    /// <summary>   Gets or sets the fetch command. </summary>
    /// <remarks>   SCPI: 'FETCh?'. </remarks>
    /// <value> The fetch command. </value>
    public virtual string FetchCommand { get; set; } = string.Empty;

    /// <summary>   Converts a readInfo to a query information. </summary>
    /// <remarks>   David, 2021-04-16. </remarks>
    /// <param name="result">   The result. </param>
    /// <returns>   Result as a Tuple. </returns>
    private QueryInfo ToQueryInfo( ReadInfo result )
    {
        return new QueryInfo( result );
    }

    /// <summary>   Fetches the reading. </summary>
    /// <remarks>
    /// Issues the 'FETCH?' query, which reads data stored in the Sample Buffer. If, for example,
    /// there are 20 data arrays stored in the Sample Buffer, then all 20 data arrays will be sent to
    /// the computer when 'FETCh?' is executed. Note that FETCh? does not affect data in the Sample
    /// Buffer. Thus, subsequent executions of FETCh? acquire the same data.
    /// </remarks>
    /// <returns>   QueryParseInfo{double} </returns>
    public virtual QueryParseInfo<double> FetchReading( bool checkStatus = false )
    {
        if ( this.ReadingAmounts.PrimaryReading is not null )
            this.Session.MakeEmulatedReplyIfEmpty( this.ReadingAmounts.PrimaryReading!.Generator.Value.ToString() );
        QueryInfo qi =
            checkStatus
            ? string.IsNullOrWhiteSpace( this.FetchCommand )
                ? this.ToQueryInfo( this.Session.ReadIfStatusDataReady( this.ReadyToReadTimeout ) )
                : this.Session.QueryIfStatusDataReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.FetchCommand )
            : string.IsNullOrWhiteSpace( this.FetchCommand )
                ? this.ToQueryInfo( this.Session.ReadElapsed() )
                : this.Session.QueryElapsed( this.FetchCommand );
        if ( string.IsNullOrWhiteSpace( qi.ReceivedMessage ) )
        {
            return new QueryParseInfo<double>( false, default, this.PrimaryReading?.ReadingCaption ?? string.Empty, qi );
        }
        else
        {
            double? parsedValue = this.ParsePrimaryReading( qi.ReceivedMessage );
            return new QueryParseInfo<double>( parsedValue.HasValue, parsedValue.GetValueOrDefault( default ),
                        this.PrimaryReading?.ReadingCaption ?? string.Empty, qi );
        }
    }

    /// <summary>   Gets or sets the read command. </summary>
    /// <remarks>   SCPI: 'READ'. </remarks>
    /// <value> The read command. </value>
    protected virtual string ReadCommand { get; set; } = string.Empty;

    /// <summary>   Initiates an operation and then fetches the data with status awareness. </summary>
    /// <remarks>
    /// Issues the 'READ?' query, which performs a trigger initiation and then a
    /// <see cref="FetchReading(bool)">Fetch Reading</see>
    /// The initiate triggers a new measurement cycle which puts new data in the Sample Buffer. Fetch
    /// reads that new data. The
    /// <see cref="VI.HarmonicsMeasureSubsystemBase.QueryParseReadingAmounts(string)">Measure</see>
    /// command places the instrument in a “one-shot” mode and then performs a read.
    /// </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// (ExecuteInfo ExecuteInfo, QueryParseInfo{double} queryParseInfo)
    /// </returns>
    public virtual (ExecuteInfo ExecuteInfo, QueryParseInfo<double> queryParseInfo) Read( bool checkStatus = false )
    {
        if ( this.ReadingAmounts.PrimaryReading is not null )
            this.Session.MakeEmulatedReplyIfEmpty( this.ReadingAmounts.PrimaryReading!.Generator.Value.ToString() );
        ExecuteInfo executeInfo = this.AssertTrigger( checkStatus );
        QueryParseInfo<double> queryParseInfo = this.FetchReading( checkStatus );
        return (executeInfo, queryParseInfo);
    }

    /// <summary>   Trigger fetch with status awareness. </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// (double? ParsedValue, ExecuteInfo ExecuteInfo, QueryParseInfo{double} queryParseInfo)
    /// </returns>
    public virtual (ExecuteInfo ExecuteInfo, QueryParseInfo<double> queryParseInfo) TriggerFetch( bool checkStatus = false )
    {
        return this.Read( checkStatus );
    }

    /// <summary>   Gets or sets The Measure query command. </summary>
    /// <value> The Measure query command. </value>
    protected virtual string MeasureQueryCommand { get; set; } = string.Empty;

    /// <summary>   Queries readings into the reading amounts with status awareness. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <returns>   The reading or none if unknown. </returns>
    public virtual double? QueryParseReadingAmounts()
    {
        return this.QueryParseReadingAmounts( this.MeasureQueryCommand );
    }

    /// <summary>
    /// QueryEnum a measured value from the instrument with status awareness. Does not use
    /// <see cref="HarmonicsMeasureSubsystemBase.ReadingAmounts"/>.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <returns>   The reading or none if unknown. </returns>
    public virtual double? QueryParsePrimaryReading()
    {
        return this.QueryParsePrimaryReading( this.MeasureQueryCommand );
    }

    /// <summary>   Estimates the lower bound on measurement time. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <returns>   A TimeSpan. </returns>
    public virtual TimeSpan EstimateMeasurementTime()
    {
        return TimeSpan.FromMilliseconds( this.GeneratorTimer.GetValueOrDefault( 6 ) );
    }

    #endregion

    #region " streaming "

    /// <summary>   Adds a buffer reading. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>   An int. </returns>
    public bool AddBufferReading( string receivedMessage )
    {
        (bool success, double? _, string _) = this.ParsePrimaryReadingFull( receivedMessage );
        if ( success )
        {
            MeasuredAmount newReading = new( this.PrimaryReading );
            this.BufferReadingsBindingList.Add( newReading );
            this.EnqueueRange( [newReading] );
            this.NotifyPropertyChanged( nameof( this.BufferReadingsCount ) );
            this.NotifyPropertyChanged( nameof( this.NewReadingsCount ) );
        }
        return success;
    }

    #endregion

    #region " buffer readings "

    /// <summary>   Gets or sets a list of buffer readings bindings. </summary>
    /// <value> A list of buffer readings bindings. </value>
    public VI.Primitives.BindingLists.InvokingBindingList<MeasuredAmount> BufferReadingsBindingList { get; private set; } = [];

    /// <summary>   Gets the number of buffer readings. </summary>
    /// <value> The number of buffer readings. </value>
    public int BufferReadingsCount => this.BufferReadingsBindingList.Count;

    /// <summary>   Gets or sets the items locker. </summary>
    /// <value> The items locker. </value>
    protected ReaderWriterLockSlim ItemsLocker { get; private set; } = new ReaderWriterLockSlim();

    /// <summary>   Gets or sets a queue of new buffer readings. </summary>
    /// <value> A thread safe Queue of buffer readings. </value>
    public System.Collections.Concurrent.ConcurrentQueue<MeasuredAmount> NewBufferReadingsQueue { get; private set; }

    /// <summary>   Clears the buffer readings queue. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    public void ClearBufferReadingsQueue()
    {
        this.NewBufferReadingsQueue = new System.Collections.Concurrent.ConcurrentQueue<MeasuredAmount>();
    }

    /// <summary>   Gets the number of new readings. </summary>
    /// <value> The number of new readings. </value>
    public int NewReadingsCount
    {
        get
        {
            try
            {
                this.ItemsLocker.EnterReadLock();
                return this.NewBufferReadingsQueue.Count;
            }
            finally
            {
                this.ItemsLocker.ExitReadLock();
            }
        }
    }

    /// <summary>   Enqueue range. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="items">    The items. </param>
    public void EnqueueRange( IList<MeasuredAmount> items )
    {
        if ( items is null || !items.Any() )
            return;
        this.ItemsLocker.EnterWriteLock();
        try
        {
            foreach ( MeasuredAmount item in items )
                this.NewBufferReadingsQueue.Enqueue( item );
        }
        finally
        {
            this.ItemsLocker.ExitWriteLock();
        }
    }

    /// <summary>   Dequeues the specified number of new values. </summary>
    /// <remarks>   David, 2020-08-01. </remarks>
    /// <param name="count">    Number of values to dequeue. </param>
    /// <returns>
    /// An enumerator that allows for each to be used to process dequeue range in this collection.
    /// </returns>
    public IList<MeasuredAmount> DequeueRange( int count )
    {
        List<MeasuredAmount> result = [];
        try
        {
            this.ItemsLocker.EnterReadLock();
            while ( this.NewBufferReadingsQueue.Any() & count > 0 )
            {
                if ( this.NewBufferReadingsQueue.TryDequeue( out MeasuredAmount value ) )
                {
                    result.Add( value );
                    count -= 1;
                }
            }
        }
        finally
        {
            this.ItemsLocker.ExitReadLock();
        }

        return result;
    }

    #endregion

    #region " auto range enabled "

    /// <summary>   Auto Range enabled. </summary>
    private bool? _autoRangeEnabled;

    /// <summary>   Gets or sets the cached Auto Range Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Range Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public virtual bool? AutoRangeEnabled
    {
        get => this._autoRangeEnabled;
        set
        {
            if ( !Equals( this.AutoRangeEnabled, value ) )
            {
                this._autoRangeEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Writes and reads back the Auto Range Enabled sentinel. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">    if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns>  A Tuple: (bool? ParsedValue, WriteBooleanInfo WriteBooleanInfo, QueryParseBooleanInfo QueryParseBooleanInfo). </returns>
    public (WriteBooleanInfo WriteBooleanInfo, QueryParseBooleanInfo QueryParseBooleanInfo) ApplyAutoRangeEnabled( bool value )
    {
        (_, WriteBooleanInfo writeBooleanInfo) = this.WriteAutoRangeEnabled( value );
        QueryParseBooleanInfo queryParseBooleanInfo = this.QueryAutoRangeEnabled();
        return (writeBooleanInfo, queryParseBooleanInfo);
    }

    /// <summary>   Gets or sets the automatic Range enabled query command. </summary>
    /// <remarks>   SCPI: ":RANG:AUTO?". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string AutoRangeEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the automatic range enabled query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteAutoRangeEnabledQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.AutoRangeEnabledQueryCommand );
    }

    /// <summary>
    /// Queries the Auto Range Enabled sentinel. Also sets the
    /// <see cref="AutoRangeEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <returns> A Tuple: (bool? ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo) . </returns>
    public virtual QueryParseBooleanInfo QueryAutoRangeEnabled()
    {
        QueryParseBooleanInfo queryParseBooleanInfo = this.Session.QueryElapsed( this.AutoRangeEnabled, this.AutoRangeEnabledQueryCommand );
        this.AutoRangeEnabled = queryParseBooleanInfo.ParsedValue;
        return queryParseBooleanInfo;
    }

    /// <summary>   Gets or sets the automatic Range enabled command Format. </summary>
    /// <remarks>   SCPI: ":RANGE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string AutoRangeEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Range Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        if set to <c>true</c> is enabled. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   (bool SentValue, WriteBooleanInfo WriteBooleanInfo) </returns>
    public virtual (bool SentValue, WriteBooleanInfo WriteBooleanInfo) WriteAutoRangeEnabled( bool value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.AutoRangeEnabledCommandFormat, value ) );
            this.AutoRangeEnabled = value;
            return (value, new WriteBooleanInfo( value, executeInfo ));
        }
        else
        {
            (bool sentValue, WriteBooleanInfo writeBooleanInfo) = this.Session.WriteLineElapsed( value, this.AutoRangeEnabledCommandFormat );
            this.AutoRangeEnabled = sentValue;
            return (sentValue, writeBooleanInfo);
        }
    }

    #endregion

    #region " access rights mode "

    /// <summary>   The access rights mode. </summary>

    /// <summary>   Gets or sets the Access Rights Mode. </summary>
    /// <value> The Access Rights Mode. </value>
    public int? AccessRightsMode
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.AccessRightsMode, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Writes and reads back the Access Rights Mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        The Access Rights Mode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple:  (int ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (int ParsedValue, WriteInfo<int> WriteInfo, QueryParseInfo<int> QueryParseInfo) ApplyAccessRightsMode( int value, bool checkStatus = false )
    {
        (int _, WriteInfo<int> writeInfo) = this.WriteAccessRightsMode( value, checkStatus );
        (int parsedValue, QueryParseInfo<int> queryParseInfo) = this.QueryAccessRightsMode( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets The Access Rights Mode query command. </summary>
    /// <value> The Access Rights Mode query command. </value>
    protected virtual string AccessRightsModeQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the access rights mode query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteAccessRightsModeQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.AccessRightsModeQueryCommand );
    }

    /// <summary>   Queries parse access rights mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo) </returns>
    protected (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryParseAccessRightsMode( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.AccessRightsModeQueryCommand )
            : this.Session.QueryElapsed( this.AccessRightsModeQueryCommand );
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseAccessRightsMode( queryInfo.ReceivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<int>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.AccessRightsModeQueryCommand}'" );
    }

    /// <summary>   Parse access rights mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns> A Tuple: (int ParsedValue, <see cref="ParseInfo{T}"/>) </returns>
    public (int ParsedValue, ParseInfo<int> ParseInfo) ParseAccessRightsMode( string receivedMessage )
    {
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseAccessRightsMode( receivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.AccessRightsModeQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse the access rights mode from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A Tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<int> ParseInfo) TryParseAccessRightsMode( string receivedMessage, out int parsedValue );

    /// <summary>   Queries The Access Rights Mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo) </returns>
    public (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryAccessRightsMode( bool checkStatus = false )
    {
        (int ParsedValue, QueryParseInfo<int> QueryParseInfo) reply = this.QueryParseAccessRightsMode( checkStatus );
        this.AccessRightsMode = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets The Access Rights Mode command format. </summary>
    /// <value> The Access Rights Mode command format. </value>
    protected virtual string AccessRightsModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Access Rights Mode without reading back the value from the device.
    /// </summary>
    /// <remarks>   This command sets The Access Rights Mode. </remarks>
    /// <param name="value">        The AccessRightsMode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int SentValue, WriteInfo{int} WriteInfo) </returns>
    public (int SentValue, WriteInfo<int> WriteInfo) WriteAccessRightsMode( int value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.AccessRightsModeCommandFormat, value ) );
            this.AccessRightsMode = value;
            return (value, new WriteInfo<int>( value, executeInfo ));
        }
        else
        {
            (int sentValue, WriteInfo<int> writeInfo) = this.Session.WriteLineElapsed( value, this.AccessRightsModeCommandFormat );
            this.AccessRightsMode = sentValue;
            return (sentValue, writeInfo);
        }
    }

    #endregion

    #region " bandwidth limiting enabled "

    /// <summary>   The bandwidth limiting enabled. </summary>

    /// <summary>   Gets or sets the bandwidth limiting enabled. </summary>
    /// <value> The bandwidth limiting enabled. </value>
    public bool? BandwidthLimitingEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.BandwidthLimitingEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Writes and reads back the bandwidth limiting enabled sentinel. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        if set to <c>true</c> if enabling; False if disabling. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple: (bool ParsedValue, WriteBooleanInfo WriteBooleanInfo, QueryParseBooleanInfo
    /// QueryParseBooleanInfo)
    /// </returns>
    public (bool ParsedValue, WriteBooleanInfo WriteBooleanInfo, QueryParseBooleanInfo QueryParseBooleanInfo) ApplyBandwidthLimitingEnabled( bool value, bool checkStatus = false )
    {
        (bool _, WriteBooleanInfo writeBooleanInfo) = this.WriteBandwidthLimitingEnabled( value, checkStatus );
        (bool parsedValue, QueryParseBooleanInfo queryParseBooleanInfo) = this.QueryBandwidthLimitingEnabled( checkStatus );
        return (parsedValue, writeBooleanInfo, queryParseBooleanInfo);
    }

    /// <summary>   Gets or sets the bandwidth limiting enabled query command. </summary>
    /// <value> The bandwidth limiting enabled query command. </value>
    protected virtual string BandwidthLimitingEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the bandwidth limiting enabled query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteBandwidthLimitingEnabledQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.BandwidthLimitingEnabledQueryCommand );
    }

    /// <summary>   Queries parse bandwidth limiting enabled. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <returns>
    /// A tuple: (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo ParseBooleanInfo)
    /// </returns>
    protected (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo) QueryParseBandwidthLimitingEnabled( bool checkStatus = false )
    {
        QueryInfo qi = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.BandwidthLimitingEnabledQueryCommand )
            : this.Session.QueryElapsed( this.BandwidthLimitingEnabledQueryCommand );
        (bool hasValue, ParseBooleanInfo parseBooleanInfo) = this.TryParseBandwidthLimitingEnabled( qi.ReceivedMessage, out bool parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseBooleanInfo( qi, parseBooleanInfo ))
            : throw new InvalidCastException( $"Failed parsing '{qi.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseBooleanInfo.ParsedMessage}' for query '{this.BandwidthLimitingEnabledQueryCommand}'" );
    }

    /// <summary>   Parse bandwidth limiting enabled. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>   A tuple: (bool ParsedValue, ParseBooleanInfo ParseBooleanInfo)  </returns>
    public (bool ParsedValue, ParseBooleanInfo ParseBooleanInfo) ParseBandwidthLimitingEnabled( string receivedMessage )
    {
        (bool hasValue, ParseBooleanInfo parseBooleanInfo) = this.TryParseBandwidthLimitingEnabled( receivedMessage, out bool parsedValue );
        return hasValue
            ? (parsedValue, parseBooleanInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseBooleanInfo.ParsedMessage}' for query '{this.BandwidthLimitingEnabledQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse a bandwidth limiting enabled from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A tuple: (bool Success, ParseBooleanInfo ParseBooleanInfo) </returns>
    public abstract (bool HasValue, ParseBooleanInfo ParseBooleanInfo) TryParseBandwidthLimitingEnabled( string receivedMessage, out bool parsedValue );

    /// <summary>
    /// Queries the bandwidth limiting enabled sentinel. Also sets the
    /// <see cref="BandwidthLimitingEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo) </returns>
    public (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo) QueryBandwidthLimitingEnabled( bool checkStatus = false )
    {
        (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo) reply = this.QueryParseBandwidthLimitingEnabled( checkStatus );
        this.BandwidthLimitingEnabled = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets the bandwidth limiting enabled command Format. </summary>
    /// <remarks>   SCPI: ":SENSE:Zero:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The bandwidth limiting enabled query command. </value>
    protected virtual string BandwidthLimitingEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the bandwidth limiting enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        if set to <c>true</c> is enabled. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (bool SentValue, string SentMessage, ElapsedTimeSpan[] ElapsedTimes)
    /// </returns>
    public (bool SentValue, WriteBooleanInfo) WriteBandwidthLimitingEnabled( bool value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ),
                                                             string.Format( this.BandwidthLimitingEnabledCommandFormat, value.GetHashCode() ) );
            this.BandwidthLimitingEnabled = value;
            return (value, new WriteBooleanInfo( value, executeInfo ));
        }
        else
        {
            (bool sentValue, WriteBooleanInfo writeBooleanInfo) = this.Session.WriteLineElapsed( value, this.BandwidthLimitingEnabledCommandFormat );
            this.BandwidthLimitingEnabled = sentValue;
            return (sentValue, writeBooleanInfo);
        }
    }

    #endregion

    #region " generator output level "

    /// <summary>   The Generator Output Level range. </summary>

    /// <summary>   The Generator Output Level range in seconds. </summary>
    /// <value> The Generator Output Level range. </value>
    public Std.Primitives.RangeR? GeneratorOutputLevelRange
    {
        get;
        set
        {
            if ( this.GeneratorOutputLevelRange != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Gets or sets the generator output level. </summary>
    /// <value> The generator output level. </value>
    public double? GeneratorOutputLevel
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.GeneratorOutputLevel, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   QueryEnum if 'value' is generator output level out of range. </summary>
    /// <remarks>   David, 2021-06-17. </remarks>
    /// <param name="value">    The Generator Output Level. </param>
    /// <returns>   True if generator output level out of range, false if not. </returns>
    public bool IsGeneratorOutputLevelOutOfRange( double value )
    {
        return !this.GeneratorOutputLevelRange!.Contains( value );
    }

    /// <summary>   Writes and reads back the Generator Output Level. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        The Generator Output Level. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (double ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (double ParsedValue, WriteInfo<double> WriteInfo, QueryParseInfo<double> QueryParseInfo) ApplyGeneratorOutputLevel( double value, bool checkStatus = false )
    {
        (double _, WriteInfo<double> writeInfo) = this.WriteGeneratorOutputLevel( value, checkStatus );
        (double parsedValue, QueryParseInfo<double> queryParseInfo) = this.QueryGeneratorOutputLevel( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets The Generator Output Level query command. </summary>
    /// <value> The Generator Output Level query command. </value>
    protected virtual string GeneratorOutputLevelQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the generator output level query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteGeneratorOutputLevelQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.GeneratorOutputLevelQueryCommand );
    }

    /// <summary>   Queries parse generator output level. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo).
    /// </returns>
    protected (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryParseGeneratorOutputLevel( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.GeneratorOutputLevelQueryCommand )
            : this.Session.QueryElapsed( this.GeneratorOutputLevelQueryCommand );
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseGeneratorOutputLevel( queryInfo.ReceivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<double>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.GeneratorOutputLevelQueryCommand}'" );
    }

    /// <summary>   Parse generator output level. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>(double ParsedValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public (double ParsedValue, ParseInfo<double> ParseInfo) ParseGeneratorOutputLevel( string receivedMessage )
    {
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseGeneratorOutputLevel( receivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.GeneratorOutputLevelQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse a generator output level from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<double> ParseInfo) TryParseGeneratorOutputLevel( string receivedMessage, out double parsedValue );

    /// <summary>   Queries The Generator Output Level. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryGeneratorOutputLevel( bool checkStatus = false )
    {
        (double ParsedValue, QueryParseInfo<double> QueryParseInfo) reply = this.QueryParseGeneratorOutputLevel( checkStatus );
        this.GeneratorOutputLevel = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets The Generator Output Level command format. </summary>
    /// <value> The Generator Output Level command format. </value>
    protected virtual string GeneratorOutputLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Generator Output Level without reading back the value from the device.
    /// </summary>
    /// <remarks>   This command sets The Generator Output Level. </remarks>
    /// <param name="value">        The GeneratorOutputLevel. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (double sentValue, <see cref="WriteInfo{T}"/> writeInfo) </returns>
    public (double sentValue, WriteInfo<double> writeInfo) WriteGeneratorOutputLevel( double value, bool checkStatus = false )
    {
        if ( this.IsGeneratorOutputLevelOutOfRange( value ) )
            throw new ArgumentException( $"Requested Generator Output Level of {value} is out of range {this.GeneratorOutputLevelRange}", nameof( value ) );
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.GeneratorOutputLevelCommandFormat, value ) );
            this.GeneratorOutputLevel = value;
            return (value, new WriteInfo<double>( value, executeInfo ));
        }
        else
        {
            (double sentValue, WriteInfo<double> writeInfo) = this.Session.WriteLineElapsed( value, this.GeneratorOutputLevelCommandFormat );
            this.GeneratorOutputLevel = value;
            return (sentValue, writeInfo);
        }
    }

    #endregion

    #region " generator timer "

    /// <summary>   The Generator Timer range. </summary>

    /// <summary>   The Generator Timer range in seconds. </summary>
    /// <value> The Generator Timer range. </value>
    public Std.Primitives.RangeI? GeneratorTimerRange
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   Gets or sets the Generator Timer. </summary>
    /// <value> The Generator Timer. </value>
    public int? GeneratorTimer
    {
        get;

        protected set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   Writes and reads back the Generator Timer. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        The Generator Timer. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (int ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (int ParsedValue, WriteInfo<int> WriteInfo, QueryParseInfo<int> QueryParseInfo) ApplyGeneratorTimer( int value, bool checkStatus = false )
    {
        (int _, WriteInfo<int> writeInfo) = this.WriteGeneratorTimer( value, checkStatus );
        (int parsedValue, QueryParseInfo<int> queryParseInfo) = this.QueryGeneratorTimer( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets The Generator Timer query command. </summary>
    /// <value> The Generator Timer query command. </value>
    protected virtual string GeneratorTimerQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the generator timer query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteGeneratorTimerQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.GeneratorTimerQueryCommand );
    }

    /// <summary>   Queries parse Generator Timer. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo).
    /// </returns>
    protected (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryParseGeneratorTimer( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.GeneratorTimerQueryCommand )
            : this.Session.QueryElapsed( this.GeneratorTimerQueryCommand );
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseGeneratorTimer( queryInfo.ReceivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<int>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.GeneratorTimerQueryCommand}'" );
    }

    /// <summary>   Parse Generator Timer. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>(int ParsedValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public (int ParsedValue, ParseInfo<int> ParseInfo) ParseGeneratorTimer( string receivedMessage )
    {
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseGeneratorTimer( receivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.GeneratorTimerQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse a Generator Timer from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<int> ParseInfo) TryParseGeneratorTimer( string receivedMessage, out int parsedValue );

    /// <summary>   Queries The Generator Timer. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A Tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo) </returns>
    public (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryGeneratorTimer( bool checkStatus = false )
    {
        (int ParsedValue, QueryParseInfo<int> QueryParseInfo) reply = this.QueryParseGeneratorTimer( checkStatus );
        this.GeneratorTimer = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets The Generator Timer command format. </summary>
    /// <value> The Generator Timer command format. </value>
    protected virtual string GeneratorTimerCommandFormat { get; set; } = string.Empty;

    /// <summary>   Writes The Generator Timer without reading back the value from the device. </summary>
    /// <remarks>   This command sets The Generator Timer. </remarks>
    /// <param name="value">        The GeneratorTimer. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int sentValue, <see cref="WriteInfo{T}"/> writeInfo) </returns>
    public (int sentValue, WriteInfo<int> writeInfo) WriteGeneratorTimer( int value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.GeneratorTimerCommandFormat, value ) );
            this.GeneratorTimer = value;
            return (value, new WriteInfo<int>( value, executeInfo ));
        }
        else
        {
            (int sentValue, WriteInfo<int> writeInfo) = this.Session.WriteLineElapsed( value, this.GeneratorTimerCommandFormat );
            this.GeneratorTimer = value;
            return (sentValue, writeInfo);
        }
    }

    #endregion

    #region " impedance range mode "

    /// <summary>   The Impedance Range mode. </summary>
    private int? _impedanceRangeMode;

    /// <summary>   Gets or sets the Impedance Range Mode. </summary>
    /// <value> The Impedance Range Mode. </value>
    public virtual int? ImpedanceRangeMode
    {
        get => this._impedanceRangeMode;

        protected set
        {
            if ( !Nullable.Equals( this.ImpedanceRangeMode, value ) )
            {
                this._impedanceRangeMode = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Writes and reads back the Impedance Range Mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        The Impedance Range Mode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple:  (int ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (int ParsedValue, WriteInfo<int> WriteInfo, QueryParseInfo<int> QueryParseInfo) ApplyImpedanceRangeMode( int value, bool checkStatus = false )
    {
        (int _, WriteInfo<int> writeInfo) = this.WriteImpedanceRangeMode( value, checkStatus );
        (int parsedValue, QueryParseInfo<int> queryParseInfo) = this.QueryImpedanceRangeMode( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets The Impedance Range Mode query command. </summary>
    /// <value> The Impedance Range Mode query command. </value>
    protected virtual string ImpedanceRangeModeQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the impedance range mode query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteImpedanceRangeModeQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.ImpedanceRangeCommandFormat );
    }

    /// <summary>   Queries parse Impedance Range mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo) </returns>
    protected (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryParseImpedanceRangeMode( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.ImpedanceRangeModeQueryCommand )
            : this.Session.QueryElapsed( this.ImpedanceRangeModeQueryCommand );
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseImpedanceRangeMode( queryInfo.ReceivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<int>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.ImpedanceRangeModeQueryCommand}'" );
    }

    /// <summary>   Parse Impedance Range mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns> A Tuple: (int ParsedValue, <see cref="ParseInfo{T}"/>) </returns>
    public (int ParsedValue, ParseInfo<int> ParseInfo) ParseImpedanceRangeMode( string receivedMessage )
    {
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseImpedanceRangeMode( receivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.ImpedanceRangeModeQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse the Impedance Range mode from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A Tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<int> ParseInfo) TryParseImpedanceRangeMode( string receivedMessage, out int parsedValue );

    /// <summary>   Queries The Impedance Range Mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo) </returns>
    public (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryImpedanceRangeMode( bool checkStatus = false )
    {
        (int ParsedValue, QueryParseInfo<int> QueryParseInfo) reply = this.QueryParseImpedanceRangeMode( checkStatus );
        this.ImpedanceRangeMode = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets The Impedance Range Mode command format. </summary>
    /// <value> The Impedance Range Mode command format. </value>
    protected virtual string ImpedanceRangeModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Impedance Range Mode without reading back the value from the device.
    /// </summary>
    /// <remarks>   This command sets The Impedance Range Mode. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="value">        The ImpedanceRangeMode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int SentValue, WriteInfo{int} WriteInfo) </returns>
    public (int SentValue, WriteInfo<int> WriteInfo) WriteImpedanceRangeMode( int value, bool checkStatus = false )
    {
        if ( this.StatusSubsystem.Session is null )
            throw new InvalidOperationException( "Status subsystem session is null." );

        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.ImpedanceRangeModeCommandFormat, value ) );
            (_, int statusByte1, _) = this.Session.AwaitStatus( this.ImpedanceRangeRefractoryTimeSpan );
            (bool hasError, string details, int statusByte) = this.StatusSubsystem.Session.IsStatusError( statusByte1 );
            if ( hasError )
                throw new InvalidOperationException( $"Status error 0x{statusByte:X2}:'{details}' after writing '{executeInfo.SentMessage}'" );
            this.ImpedanceRangeMode = value;
            return (value, new WriteInfo<int>( value, executeInfo ));
        }
        else
        {
            (int sentValue, WriteInfo<int> writeInfo) = this.Session.WriteLineElapsed( value, this.ImpedanceRangeModeCommandFormat );
            (_, int statusByte1, _) = this.Session.AwaitStatus( this.ImpedanceRangeRefractoryTimeSpan );
            (bool hasError, string details, int statusByte) = this.StatusSubsystem.Session.IsStatusError( statusByte1 );
            if ( hasError )
                throw new InvalidOperationException( $"Status error 0x{statusByte:X2}:'{details}' after writing '{writeInfo.SentMessage}'" );
            this.ImpedanceRangeMode = sentValue;
            return (sentValue, writeInfo);
        }
    }

    /// <summary>   Gets or sets the impedance range change execution refractory time span. </summary>
    /// <value> The impedance range change execution refractory time span. </value>
    public virtual TimeSpan ImpedanceRangeRefractoryTimeSpan { get; set; }

    #endregion

    #region " impedance range "

    /// <summary>   The impedance range. </summary>

    /// <summary>   Gets or sets the range of Impedance Range. </summary>
    /// <value> The range Impedance Range. </value>
    public Std.Primitives.RangeR? ImpedanceRangeRange
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   The impedance range. </summary>
    private double? _impedanceRange;

    /// <summary>   Gets or sets the Impedance Range. </summary>
    /// <value> The Impedance Range. </value>
    public virtual double? ImpedanceRange
    {
        get => this._impedanceRange;

        protected set => _ = base.SetProperty( ref this._impedanceRange, value );
    }

    /// <summary>   Applies the Impedance Range described by value. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <param name="value">        The impedance value for setting the range. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (double ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="WriteInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (double ParsedValue, WriteInfo<double> WriteInfo, QueryParseInfo<double> QueryParseInfo) ApplyImpedanceRange( double value, bool checkStatus = false )
    {
        (double _, WriteInfo<double> writeInfo) = this.WriteImpedanceRange( value, checkStatus );
        (double parsedValue, QueryParseInfo<double> queryParseInfo) = this.QueryImpedanceRange( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets the 'Impedance Range query' command. </summary>
    /// <value> The 'Impedance Range query' command. </value>
    protected virtual string ImpedanceRangeQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the impedance range query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteImpedanceRangeQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.ImpedanceRangeQueryCommand );
    }

    /// <summary>   Queries parse impedance range. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <returns>
    /// A tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    protected (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryParseImpedanceRange( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.ImpedanceRangeQueryCommand )
            : this.Session.QueryElapsed( this.ImpedanceRangeQueryCommand );
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseImpedanceRange( queryInfo.ReceivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<double>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.ImpedanceRangeQueryCommand}'" );
    }

    /// <summary>   Parse impedance range. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>   A tuple: (double ParsedValue, <see cref="ParseInfo{T}"/> ParseInfo) . </returns>
    public (double ParsedValue, ParseInfo<double> ParseInfo) ParseImpedanceRange( string receivedMessage )
    {
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseImpedanceRange( receivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.ImpedanceRangeQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse an impedance range from the given data, returning a default value rather
    /// than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<double> ParseInfo) TryParseImpedanceRange( string receivedMessage, out double parsedValue );

    /// <summary>   Queries Impedance Range. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public virtual (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryImpedanceRange( bool checkStatus = false )
    {
        (double ParsedValue, QueryParseInfo<double> QueryParseInfo) reply = this.QueryParseImpedanceRange( checkStatus );
        this.ImpedanceRange = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets the Impedance Range command format. </summary>
    /// <value> The Impedance Range command format. </value>
    protected virtual string ImpedanceRangeCommandFormat { get; set; } = string.Empty;

    /// <summary>   Writes a scaled Impedance Range. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <param name="value">        The impedance value for setting the range. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (double sentValue, <see cref="WriteInfo{T}"/> writeInfo) </returns>
    protected virtual (double sentValue, WriteInfo<double> writeInfo) WriteScaledImpedanceRange( double value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.ImpedanceRangeCommandFormat, value ) );
            return (value, new WriteInfo<double>( value, executeInfo ));
        }
        else
        {
            return this.Session.WriteLineElapsed( value, this.ImpedanceRangeCommandFormat );
        }
    }

    /// <summary>   Writes a Impedance Range. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <param name="value">        The impedance value for setting the range. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (double SentValue, <see cref="WriteInfo{T}"/> WriteInfo) </returns>
    public virtual (double SentValue, WriteInfo<double> WriteInfo) WriteImpedanceRange( double value, bool checkStatus = false )
    {
        (double sentValue, WriteInfo<double> writeInfo) reply = this.WriteScaledImpedanceRange( value, checkStatus );
        this.ImpedanceRange = value;
        return reply;
    }

    #endregion

    #region " measurement start mode "

    /// <summary>   The Measurement Start mode. </summary>

    /// <summary>   Gets or sets the Measurement Start Mode. </summary>
    /// <value> The Measurement Start Mode. </value>
    public int? MeasurementStartMode
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.MeasurementStartMode, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Writes and reads back the Measurement Start Mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        The Measurement Start Mode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple:  (int ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (int ParsedValue, WriteInfo<int> WriteInfo, QueryParseInfo<int> QueryParseInfo) ApplyMeasurementStartMode( int value, bool checkStatus = false )
    {
        (int _, WriteInfo<int> writeInfo) = this.WriteMeasurementStartMode( value, checkStatus );
        (int parsedValue, QueryParseInfo<int> queryParseInfo) = this.QueryMeasurementStartMode( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets The Measurement Start Mode query command. </summary>
    /// <value> The Measurement Start Mode query command. </value>
    protected virtual string MeasurementStartModeQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the measurement start mode query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteMeasurementStartModeQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.MeasurementStartModeQueryCommand );
    }

    /// <summary>   Queries parse Measurement Start mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo) </returns>
    protected (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryParseMeasurementStartMode( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.MeasurementStartModeQueryCommand )
            : this.Session.QueryElapsed( this.MeasurementStartModeQueryCommand );
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseMeasurementStartMode( queryInfo.ReceivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<int>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.MeasurementStartModeQueryCommand}'" );
    }

    /// <summary>   Parse Measurement Start mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns> A Tuple: (int ParsedValue, <see cref="ParseInfo{T}"/>) </returns>
    public (int ParsedValue, ParseInfo<int> ParseInfo) ParseMeasurementStartMode( string receivedMessage )
    {
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseMeasurementStartMode( receivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.MeasurementStartModeQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse the Measurement Start mode from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A Tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<int> ParseInfo) TryParseMeasurementStartMode( string receivedMessage, out int parsedValue );

    /// <summary>   Queries The Measurement Start Mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo) </returns>
    public (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryMeasurementStartMode( bool checkStatus = false )
    {
        (int ParsedValue, QueryParseInfo<int> QueryParseInfo) reply = this.QueryParseMeasurementStartMode( checkStatus );
        this.MeasurementStartMode = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets The Measurement Start Mode command format. </summary>
    /// <value> The Measurement Start Mode command format. </value>
    protected virtual string MeasurementStartModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Measurement Start Mode without reading back the value from the device.
    /// </summary>
    /// <remarks>   This command sets The Measurement Start Mode. </remarks>
    /// <param name="value">        The MeasurementStartMode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int SentValue, WriteInfo{int} WriteInfo) </returns>
    public (int SentValue, WriteInfo<int> WriteInfo) WriteMeasurementStartMode( int value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.MeasurementStartModeCommandFormat, value ) );
            this.MeasurementStartMode = value;
            return (value, new WriteInfo<int>( value, executeInfo ));
        }
        else
        {
            (int sentValue, WriteInfo<int> writeInfo) = this.Session.WriteLineElapsed( value, this.MeasurementStartModeCommandFormat );
            this.MeasurementStartMode = sentValue;
            return (sentValue, writeInfo);
        }
    }

    #endregion

    #region " voltmeter output enabled "

    /// <summary>   The Voltmeter Output enabled. </summary>

    /// <summary>   Gets or sets the Voltmeter Output enabled. </summary>
    /// <value> The Voltmeter Output enabled. </value>
    public bool? VoltmeterOutputEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.VoltmeterOutputEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Writes and reads back the Voltmeter Output enabled sentinel. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        if set to <c>true</c> if enabling; False if disabling. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple: (bool ParsedValue, WriteBooleanInfo WriteBooleanInfo, QueryParseBooleanInfo
    /// QueryParseBooleanInfo)
    /// </returns>
    public (bool ParsedValue, WriteBooleanInfo WriteBooleanInfo, QueryParseBooleanInfo QueryParseBooleanInfo) ApplyVoltmeterOutputEnabled( bool value, bool checkStatus = false )
    {
        (bool _, WriteBooleanInfo writeBooleanInfo) = this.WriteVoltmeterOutputEnabled( value, checkStatus );
        (bool parsedValue, QueryParseBooleanInfo queryParseBooleanInfo) = this.QueryVoltmeterOutputEnabled( checkStatus );
        return (parsedValue, writeBooleanInfo, queryParseBooleanInfo);
    }

    /// <summary>   Gets or sets the Voltmeter Output enabled query command. </summary>
    /// <value> The Voltmeter Output enabled query command. </value>
    protected virtual string VoltmeterOutputEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the Voltmeter Output enabled query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteVoltmeterOutputEnabledQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.VoltmeterOutputEnabledQueryCommand );
    }

    /// <summary>   Queries parse Voltmeter Output enabled. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <returns>
    /// A tuple: (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo ParseBooleanInfo)
    /// </returns>
    protected (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo) QueryParseVoltmeterOutputEnabled( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.VoltmeterOutputEnabledQueryCommand )
            : this.Session.QueryElapsed( this.VoltmeterOutputEnabledQueryCommand );
        (bool hasValue, ParseBooleanInfo parseBooleanInfo) = this.TryParseVoltmeterOutputEnabled( queryInfo.ReceivedMessage, out bool parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseBooleanInfo( queryInfo, parseBooleanInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseBooleanInfo.ParsedMessage}' for query '{this.VoltmeterOutputEnabledQueryCommand}'" );
    }

    /// <summary>   Parse Voltmeter Output enabled. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>   A tuple: (bool ParsedValue, ParseBooleanInfo ParseBooleanInfo)  </returns>
    public (bool ParsedValue, ParseBooleanInfo ParseBooleanInfo) ParseVoltmeterOutputEnabled( string receivedMessage )
    {
        (bool hasValue, ParseBooleanInfo parseBooleanInfo) = this.TryParseVoltmeterOutputEnabled( receivedMessage, out bool parsedValue );
        return hasValue
            ? (parsedValue, parseBooleanInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseBooleanInfo.ParsedMessage}' for query '{this.VoltmeterOutputEnabledQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse a Voltmeter Output enabled from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A tuple: (bool Success, ParseBooleanInfo ParseBooleanInfo) </returns>
    public abstract (bool HasValue, ParseBooleanInfo ParseBooleanInfo) TryParseVoltmeterOutputEnabled( string receivedMessage, out bool parsedValue );

    /// <summary>
    /// Queries the Voltmeter Output enabled sentinel. Also sets the
    /// <see cref="VoltmeterOutputEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo) </returns>
    public (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo) QueryVoltmeterOutputEnabled( bool checkStatus = false )
    {
        (bool ParsedValue, QueryParseBooleanInfo QueryParseBooleanInfo) reply = this.QueryParseVoltmeterOutputEnabled( checkStatus );
        this.VoltmeterOutputEnabled = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets the Voltmeter Output enabled command Format. </summary>
    /// <remarks>   SCPI: ":SENSE:Zero:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Voltmeter Output enabled query command. </value>
    protected virtual string VoltmeterOutputEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Voltmeter Output enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        if set to <c>true</c> is enabled. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (bool SentValue, string SentMessage, ElapsedTimeSpan[] ElapsedTimes)
    /// </returns>
    public (bool SentValue, WriteBooleanInfo) WriteVoltmeterOutputEnabled( bool value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ),
                                                             string.Format( this.VoltmeterOutputEnabledCommandFormat, value.GetHashCode() ) );
            this.VoltmeterOutputEnabled = value;
            return (value, new WriteBooleanInfo( value, executeInfo ));
        }
        else
        {
            (bool sentValue, WriteBooleanInfo writeBooleanInfo) = this.Session.WriteLineElapsed( value, this.VoltmeterOutputEnabledCommandFormat );
            this.VoltmeterOutputEnabled = sentValue;
            return (sentValue, writeBooleanInfo);
        }
    }

    #endregion

    #region " voltmeter range mode "

    /// <summary>   The Voltmeter Range mode. </summary>
    private int? _voltmeterRangeMode;

    /// <summary>   Gets or sets the Voltmeter Range Mode. </summary>
    /// <value> The Voltmeter Range Mode. </value>
    public virtual int? VoltmeterRangeMode
    {
        get => this._voltmeterRangeMode;

        protected set
        {
            if ( !Nullable.Equals( this.VoltmeterRangeMode, value ) )
            {
                this._voltmeterRangeMode = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Writes and reads back the Voltmeter Range Mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        The Voltmeter Range Mode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple:  (int ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (int ParsedValue, WriteInfo<int> WriteInfo, QueryParseInfo<int> QueryParseInfo) ApplyVoltmeterRangeMode( int value, bool checkStatus = false )
    {
        (int _, WriteInfo<int> writeInfo) = this.WriteVoltmeterRangeMode( value, checkStatus );
        (int parsedValue, QueryParseInfo<int> queryParseInfo) = this.QueryVoltmeterRangeMode( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets The Voltmeter Range Mode query command. </summary>
    /// <value> The Voltmeter Range Mode query command. </value>
    protected virtual string VoltmeterRangeModeQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the Voltmeter Range mode query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteVoltmeterRangeModeQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.VoltmeterRangeModeQueryCommand );
    }

    /// <summary>   Queries parse Voltmeter Range mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo) </returns>
    protected (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryParseVoltmeterRangeMode( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.VoltmeterRangeModeQueryCommand )
            : this.Session.QueryElapsed( this.VoltmeterRangeModeQueryCommand );
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseVoltmeterRangeMode( queryInfo.ReceivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<int>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.VoltmeterRangeModeQueryCommand}'" );
    }

    /// <summary>   Parse Voltmeter Range mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>   A Tuple: (int ParsedValue, <see cref="ParseInfo{T}"/>) </returns>
    public (int ParsedValue, ParseInfo<int> ParseInfo) ParseVoltmeterRangeMode( string receivedMessage )
    {
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseVoltmeterRangeMode( receivedMessage, out int parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.VoltmeterRangeModeQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse the Voltmeter Range mode from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A Tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<int> ParseInfo) TryParseVoltmeterRangeMode( string receivedMessage, out int parsedValue );

    /// <summary>   Queries The Voltmeter Range Mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo) </returns>
    public (int ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryVoltmeterRangeMode( bool checkStatus = false )
    {
        (int ParsedValue, QueryParseInfo<int> QueryParseInfo) reply = this.QueryParseVoltmeterRangeMode( checkStatus );
        this.VoltmeterRangeMode = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets The Voltmeter Range Mode command format. </summary>
    /// <value> The Voltmeter Range Mode command format. </value>
    protected virtual string VoltmeterRangeModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Voltmeter Range Mode without reading back the value from the device.
    /// </summary>
    /// <remarks>   This command sets The Voltmeter Range Mode. </remarks>
    /// <param name="value">        The MeasureRangeMode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (int SentValue, WriteInfo{int} WriteInfo) </returns>
    public (int SentValue, WriteInfo<int> WriteInfo) WriteVoltmeterRangeMode( int value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.VoltmeterRangeModeCommandFormat, value ) );
            this.VoltmeterRangeMode = value;
            return (value, new WriteInfo<int>( value, executeInfo ));
        }
        else
        {
            (int sentValue, WriteInfo<int> writeInfo) = this.Session.WriteLineElapsed( value, this.VoltmeterRangeModeCommandFormat );
            this.VoltmeterRangeMode = sentValue;
            return (sentValue, writeInfo);
        }
    }

    #endregion

    #region " voltmeter range "

    /// <summary>   The range of the Voltmeter. </summary>

    /// <summary>   Gets or sets the range of Voltmeter Range. </summary>
    /// <value> The range Voltmeter Range. </value>
    public Std.Primitives.RangeR? VoltmeterRangeRange
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   The Voltmeter Range. </summary>
    private double? _voltmeterRange;

    /// <summary>   Gets or sets the Voltmeter Range. </summary>
    /// <value> The Voltmeter Range. </value>
    public virtual double? VoltmeterRange
    {
        get => this._voltmeterRange;

        protected set => _ = base.SetProperty( ref this._voltmeterRange, value );
    }

    /// <summary>   Applies the Voltmeter Range described by value. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <param name="value">        The VoltageMeasure value for setting the range. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (double ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="WriteInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (double ParsedValue, WriteInfo<double> WriteInfo, QueryParseInfo<double> QueryParseInfo) ApplyVoltmeterRange( double value, bool checkStatus = false )
    {
        (double _, WriteInfo<double> writeInfo) = this.WriteVoltmeterRange( value, checkStatus );
        (double parsedValue, QueryParseInfo<double> queryParseInfo) = this.QueryVoltmeterRange( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets the 'Voltmeter Range query' command. </summary>
    /// <value> The 'Voltmeter Range query' command. </value>
    protected virtual string VoltmeterRangeQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the Voltmeter Range query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteVoltmeterRangeQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.VoltmeterRangeQueryCommand );
    }

    /// <summary>   Queries parse Voltmeter Range. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    protected (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryParseVoltmeterRange( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.VoltmeterRangeQueryCommand )
            : this.Session.QueryElapsed( this.VoltmeterRangeQueryCommand );
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseVoltmeterRange( queryInfo.ReceivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<double>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.VoltmeterRangeQueryCommand}'" );
    }

    /// <summary>   Parse Voltmeter Range. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>   A tuple: (double ParsedValue, <see cref="ParseInfo{T}"/> ParseInfo) . </returns>
    public (double ParsedValue, ParseInfo<double> ParseInfo) ParseVoltmeterRange( string receivedMessage )
    {
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseVoltmeterRange( receivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.VoltmeterRangeQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse an Voltmeter Range from the given data, returning a default value rather
    /// than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns>
    /// A tuple: (bool hasValue, <see cref="ParseInfo{T}"/> ParseInfo)
    /// </returns>
    public abstract (bool hasValue, ParseInfo<double> ParseInfo) TryParseVoltmeterRange( string receivedMessage, out double parsedValue );

    /// <summary>   Queries Voltmeter Range. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public virtual (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryVoltmeterRange( bool checkStatus = false )
    {
        (double ParsedValue, QueryParseInfo<double> QueryParseInfo) reply = this.QueryParseVoltmeterRange( checkStatus );
        this.VoltmeterRange = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets the Voltmeter Range command format. </summary>
    /// <value> The Voltmeter Range command format. </value>
    protected virtual string VoltmeterRangeCommandFormat { get; set; } = string.Empty;

    /// <summary>   Writes a scaled Voltmeter Range. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <param name="value">        The VoltageMeasure value for setting the range. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (double sentValue, <see cref="WriteInfo{T}"/> writeInfo) </returns>
    protected virtual (double sentValue, WriteInfo<double> writeInfo) WriteScaledVoltmeterRange( double value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.VoltmeterRangeCommandFormat, value ) );
            return (value, new WriteInfo<double>( value, executeInfo ));
        }
        else
        {
            return this.Session.WriteLineElapsed( value, this.VoltmeterRangeCommandFormat );
        }
    }

    /// <summary>   Writes a Voltmeter Range. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="value">        The VoltageMeasure value for setting the range. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (double sentValue, <see cref="WriteInfo{T}"/> writeInfo) </returns>
    public virtual (double sentValue, WriteInfo<double> writeInfo) WriteVoltmeterRange( double value, bool checkStatus = false )
    {
        if ( this.StatusSubsystem.Session is null )
            throw new InvalidOperationException( "Status subsystem session is null." );

        if ( !this.StatusSubsystem.Session.IsDeviceOpen )
            throw new InvalidOperationException( "Status subsystem session is not open." );

        (double sentValue, WriteInfo<double> writeInfo) reply = this.WriteScaledVoltmeterRange( value, checkStatus );
        (_, int statusByte1, _) = this.Session.AwaitStatus( this.VoltmeterRangeRefractoryTimeSpan );
        (bool hasError, string details, int statusByte) = this.StatusSubsystem.Session.IsStatusError( statusByte1 );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{statusByte:X2}:'{details}' after writing '{reply.writeInfo.SentMessage}'" );
        this.VoltmeterRange = value;
        return reply;
    }

    /// <summary>   Gets or sets the Voltmeter Range change execution refractory time span. </summary>
    /// <value> The Voltmeter Range change execution refractory time span. </value>
    public virtual TimeSpan VoltmeterRangeRefractoryTimeSpan { get; set; }

    #endregion

    #region " voltmeter high limit "

    /// <summary>   The Voltmeter High Limit range. </summary>

    /// <summary>   The Voltmeter High Limit range in seconds. </summary>
    /// <value> The Voltmeter High Limit range. </value>
    public Std.Primitives.RangeR? VoltmeterHighLimitRange
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   QueryEnum if 'value' is out of voltmeter high limit range. </summary>
    /// <remarks>   David, 2021-06-17. </remarks>
    /// <param name="value">    The Generator Output Level. </param>
    /// <returns>   True if value is out of voltmeter high limit range, false if not. </returns>
    public bool IsVoltmeterHighLimitOutOfRange( double value )
    {
        return !this.VoltmeterHighLimitRange?.Contains( value ) ?? false;
    }

    /// <summary>   Gets or sets the Voltmeter High Limit. </summary>
    /// <value> The Voltmeter High Limit. </value>
    public double? VoltmeterHighLimit
    {
        get;

        protected set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   Writes and reads back the Voltmeter High Limit. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        The Voltmeter High Limit. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (double ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (double ParsedValue, WriteInfo<double> WriteInfo, QueryParseInfo<double> QueryParseInfo) ApplyVoltmeterHighLimit( double value, bool checkStatus = false )
    {
        (double _, WriteInfo<double> writeInfo) = this.WriteVoltmeterHighLimit( value, checkStatus );
        (double parsedValue, QueryParseInfo<double> queryParseInfo) = this.QueryVoltmeterHighLimit( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets The Voltmeter High Limit query command. </summary>
    /// <value> The Voltmeter High Limit query command. </value>
    protected virtual string VoltmeterHighLimitQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the voltmeter high limit query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteVoltmeterHighLimitQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.VoltmeterHighLimitQueryCommand );
    }

    /// <summary>   Queries parse Voltmeter High Limit. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo).
    /// </returns>
    protected (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryParseVoltmeterHighLimit( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.VoltmeterHighLimitQueryCommand )
            : this.Session.QueryElapsed( this.VoltmeterHighLimitQueryCommand );
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseVoltmeterHighLimit( queryInfo.ReceivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<double>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.VoltmeterHighLimitQueryCommand}'" );
    }

    /// <summary>   Parse Voltmeter High Limit. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>(double ParsedValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public (double ParsedValue, ParseInfo<double> ParseInfo) ParseVoltmeterHighLimit( string receivedMessage )
    {
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseVoltmeterHighLimit( receivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.VoltmeterHighLimitQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse a Voltmeter High Limit from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<double> ParseInfo) TryParseVoltmeterHighLimit( string receivedMessage, out double parsedValue );

    /// <summary>   Queries The Voltmeter High Limit. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryVoltmeterHighLimit( bool checkStatus = false )
    {
        (double ParsedValue, QueryParseInfo<double> QueryParseInfo) reply = this.QueryParseVoltmeterHighLimit( checkStatus );
        this.VoltmeterHighLimit = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets The Voltmeter High Limit command format. </summary>
    /// <value> The Voltmeter High Limit command format. </value>
    protected virtual string VoltmeterHighLimitCommandFormat { get; set; } = string.Empty;

    /// <summary>   Writes a scaled Voltmeter high limit. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <param name="value">        The Voltmeter high limit. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (double sentValue, <see cref="WriteInfo{T}"/> writeInfo) </returns>
    protected virtual (double sentValue, WriteInfo<double> writeInfo) WriteScaledVoltmeterHighLimit( double value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.VoltmeterHighLimitCommandFormat, value ) );
            return (value, new WriteInfo<double>( value, executeInfo ));
        }
        else
        {
            return this.Session.WriteLineElapsed( value, this.VoltmeterHighLimitCommandFormat );
        }
    }

    /// <summary>
    /// Writes The Voltmeter High Limit without reading back the value from the device.
    /// </summary>
    /// <remarks>   This command sets The Voltmeter High Limit. </remarks>
    /// <param name="value">        The VoltmeterHighLimit. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (double sentValue, <see cref="WriteInfo{T}"/> writeInfo) </returns>
    public (double sentValue, WriteInfo<double> writeInfo) WriteVoltmeterHighLimit( double value, bool checkStatus = false )
    {
        if ( this.IsVoltmeterHighLimitOutOfRange( value ) )
            throw new ArgumentException( $"Requested low limit of {value} is out of range {this.VoltmeterHighLimitRange}", nameof( value ) );
        (double sentValue, WriteInfo<double> writeInfo) = this.WriteScaledVoltmeterHighLimit( value, checkStatus );
        this.VoltmeterHighLimit = value;
        return (sentValue, writeInfo);
    }

    #endregion

    #region " voltmeter low limit "

    /// <summary>   The Voltmeter Low Limit range. </summary>

    /// <summary>   The Voltmeter Low Limit range in seconds. </summary>
    /// <value> The Voltmeter Low Limit range. </value>
    public Std.Primitives.RangeR? VoltmeterLowLimitRange
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   QueryEnum if 'value' is out of voltmeter low limit range. </summary>
    /// <remarks>   David, 2021-06-17. </remarks>
    /// <param name="value">    The Generator Output Level. </param>
    /// <returns>   True if value is out of voltmeter low limit range, false if not. </returns>
    public bool IsVoltmeterLowLimitOutOfRange( double value )
    {
        return !this.VoltmeterLowLimitRange?.Contains( value ) ?? false;
    }

    /// <summary>   Gets or sets the Voltmeter Low Limit. </summary>
    /// <value> The Voltmeter Low Limit. </value>
    public double? VoltmeterLowLimit
    {
        get;

        protected set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   Writes and reads back the Voltmeter Low Limit. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">        The Voltmeter Low Limit. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (double ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (double ParsedValue, WriteInfo<double> WriteInfo, QueryParseInfo<double> QueryParseInfo) ApplyVoltmeterLowLimit( double value, bool checkStatus = false )
    {
        (double _, WriteInfo<double> writeInfo) = this.WriteVoltmeterLowLimit( value, checkStatus );
        (double parsedValue, QueryParseInfo<double> queryParseInfo) = this.QueryVoltmeterLowLimit( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary>   Gets or sets The Voltmeter Low Limit query command. </summary>
    /// <value> The Voltmeter Low Limit query command. </value>
    protected virtual string VoltmeterLowLimitQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the voltmeter low limit query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteVoltmeterLowLimitQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.VoltmeterLowLimitQueryCommand );
    }

    /// <summary>   Queries parse Voltmeter Low Limit. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo).
    /// </returns>
    protected (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryParseVoltmeterLowLimit( bool checkStatus = false )
    {
        QueryInfo queryInfo = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.VoltmeterLowLimitQueryCommand )
            : this.Session.QueryElapsed( this.VoltmeterLowLimitQueryCommand );
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseVoltmeterLowLimit( queryInfo.ReceivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<double>( queryInfo, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{queryInfo.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.VoltmeterLowLimitQueryCommand}'" );
    }

    /// <summary>   Parse Voltmeter Low Limit. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>(double ParsedValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public (double ParsedValue, ParseInfo<double> ParseInfo) ParseVoltmeterLowLimit( string receivedMessage )
    {
        (bool hasValue, ParseInfo<double> parseInfo) = this.TryParseVoltmeterLowLimit( receivedMessage, out double parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.VoltmeterLowLimitQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse a Voltmeter Low Limit from the given data, returning a default value
    /// rather than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<double> ParseInfo) TryParseVoltmeterLowLimit( string receivedMessage, out double parsedValue );

    /// <summary>   Queries The Voltmeter Low Limit. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A Tuple: (double ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (double ParsedValue, QueryParseInfo<double> QueryParseInfo) QueryVoltmeterLowLimit( bool checkStatus = false )
    {
        (double ParsedValue, QueryParseInfo<double> QueryParseInfo) reply = this.QueryParseVoltmeterLowLimit( checkStatus );
        this.VoltmeterLowLimit = reply.ParsedValue;
        return reply;
    }

    /// <summary>   Gets or sets The Voltmeter Low Limit command format. </summary>
    /// <value> The Voltmeter Low Limit command format. </value>
    protected virtual string VoltmeterLowLimitCommandFormat { get; set; } = string.Empty;

    /// <summary>   Writes a scaled Voltmeter Low limit. </summary>
    /// <remarks>   David, 2021-03-27. </remarks>
    /// <param name="value">        The Voltmeter Low limit. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (double sentValue, <see cref="WriteInfo{T}"/> writeInfo) </returns>
    protected virtual (double sentValue, WriteInfo<double> writeInfo) WriteScaledVoltmeterLowLimit( double value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ), string.Format( this.VoltmeterLowLimitCommandFormat, value ) );
            return (value, new WriteInfo<double>( value, executeInfo ));
        }
        else
        {
            return this.Session.WriteLineElapsed( value, this.VoltmeterLowLimitCommandFormat );
        }
    }

    /// <summary>
    /// Writes The Voltmeter Low Limit without reading back the value from the device.
    /// </summary>
    /// <remarks>   This command sets The Voltmeter Low Limit. </remarks>
    /// <param name="value">        The VoltmeterLowLimit. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>   A tuple: (double sentValue, <see cref="WriteInfo{T}"/> writeInfo) </returns>
    public (double sentValue, WriteInfo<double> writeInfo) WriteVoltmeterLowLimit( double value, bool checkStatus = false )
    {
        if ( this.IsVoltmeterLowLimitOutOfRange( value ) )
            throw new ArgumentException( $"Requested low limit of {value} is out of range {this.VoltmeterLowLimitRange}", nameof( value ) );
        (double sentValue, WriteInfo<double> writeInfo) = this.WriteScaledVoltmeterLowLimit( value, checkStatus );
        this.VoltmeterLowLimit = value;
        return (sentValue, writeInfo);
    }

    #endregion

    #region " measure unit "

    /// <summary>   Gets or sets the default measurement unit. </summary>
    /// <value> The default measure unit. </value>
    public cc.isr.UnitsAmounts.Unit DefaultMeasurementUnit { get; set; }

    /// <summary>   Gets or sets the function unit. </summary>
    /// <value> The function unit. </value>
    public cc.isr.UnitsAmounts.Unit MeasurementUnit
    {
        get => this.PrimaryReading!.Amount.Unit;
        set
        {
            if ( this.MeasurementUnit != value )
            {
                this.PrimaryReading!.ApplyUnit( value );
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " front / read terminal support "

    /// <summary>   Gets true if the subsystem supports front terminals selection query. </summary>
    /// <value>
    /// The value indicating if the subsystem supports front terminals selection query.
    /// </value>
    public bool SupportsFrontTerminalsSelectionQuery => false;

    /// <summary>   Gets or sets the terminals caption. </summary>
    /// <value> The terminals caption. </value>
    public string TerminalsCaption { get; set; } = "N/A";

    #endregion
}
