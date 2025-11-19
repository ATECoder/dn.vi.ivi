// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;
using cc.isr.Std.TimeSpanExtensions;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by a Trace Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class TraceSubsystemBase : SubsystemBase, IDisposable
{
    #region " construction and cleanup "

    /// <summary> Initializes a new instance of the <see cref="TraceSubsystemBase" /> class. </summary>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    protected TraceSubsystemBase( StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this._orderedReadingElementTypes = [ReadingElementTypes.Reading, ReadingElementTypes.Timestamp, ReadingElementTypes.Status, ReadingElementTypes.Units];
        this.FeedControlReadWrites = [];
        this.DefineFeedControlReadWrites();
        this.FeedSourceReadWrites = [];
        this.DefineFeedSourceReadWrites();
        this.BufferReadingsBindingList = [];
        this.BusTriggerRequestedToken = new cc.isr.VI.Primitives.ConcurrentToken<bool>();
        this.BufferStreamingEnabledToken = new cc.isr.VI.Primitives.ConcurrentToken<bool>();
        this.BufferStreamingActiveToken = new cc.isr.VI.Primitives.ConcurrentToken<bool>();
        this.BufferStreamingAlertToken = new cc.isr.VI.Primitives.ConcurrentToken<bool>();
        this.BinningStrobeRequestedToken = new cc.isr.VI.Primitives.ConcurrentToken<bool>();
        this.NewBufferReadingsQueue = new System.Collections.Concurrent.ConcurrentQueue<BufferReading>();
        this.MeasuredElementType = ReadingElementTypes.Voltage;
        this.BufferStreamTasker = new();
        this._lastBufferReading = new();
        this._bufferReadingUnit = new();
    }

    /// <summary> Calls <see cref="Dispose(bool)" /> to cleanup. </summary>
    /// <remarks>
    /// Do not make this method Overridable (virtual) because a derived class should not be able to
    /// override this method.
    /// </remarks>
    public void Dispose()
    {
        this.Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// Gets or sets the dispose status sentinel of the base class.  This applies to the derived
    /// class provided proper implementation.
    /// </summary>
    /// <value> <c>true</c> if disposed; otherwise, <c>false</c>. </value>
    protected bool IsDisposed { get; set; }

    /// <summary> Cleans up unmanaged or managed and unmanaged resources. </summary>
    /// <remarks>
    /// Executes in two distinct scenarios as determined by its disposing parameter:<para>
    /// If True, the method has been called directly or indirectly by a user's code--managed and
    /// unmanaged resources can be disposed.</para><para>
    /// If False, the method has been called by the runtime from inside the finalizer and you should
    /// not reference other objects--only unmanaged resources can be disposed.</para>
    /// </remarks>
    /// <param name="disposing"> <c>true</c> if this method releases both managed and unmanaged
    /// resources;
    /// False if this method releases only unmanaged
    /// resources. </param>
    protected virtual void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;
        try
        {
            if ( disposing )
            {
                this.ItemsLocker?.Dispose();
                this.BufferStreamingEnabledToken?.Dispose();
                this.BufferStreamingActiveToken?.Dispose();
            }
        }
        finally
        {
            // set the sentinel indicating that the class was disposed.
            this.IsDisposed = true;
        }
    }

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.PointsCount = 0;
        this.FeedSource = FeedSources.None;
        this.FeedControl = FeedControls.Never;
        this.BufferReadingsBindingList = [];
        this.ReadingElementTypes = JoinReadingElementTypes( this.OrderedReadingElementTypes );
    }

    #endregion

    #region " commands "

    /// <summary> Gets or sets the Clear Buffer command. </summary>
    /// <remarks> SCPI: ":TRAC:CLE". </remarks>
    /// <value> The ClearBuffer command. </value>
    protected virtual string ClearBufferCommand { get; set; } = string.Empty;

    /// <summary> Clears the buffer. </summary>
    public void ClearBuffer()
    {
        _ = this.Session.WriteLine( this.ClearBufferCommand );
    }

    #endregion

    #region " auto points enabled "

    /// <summary> Auto Points enabled. </summary>
    private bool? _autoPointsEnabled;

    /// <summary> Gets or sets the cached Auto Points Enabled sentinel. </summary>
    /// <remarks>
    /// When setting to auto points, the buffer points default to the trigger plan trigger count.
    /// </remarks>
    /// <value>
    /// <c>null</c> if Auto Points Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public virtual bool? AutoPointsEnabled
    {
        get => this._autoPointsEnabled;

        protected set
        {
            if ( !Equals( this.AutoPointsEnabled, value ) )
            {
                this._autoPointsEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Auto Points Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoPointsEnabled( bool value )
    {
        _ = this.WriteAutoPointsEnabled( value );
        return this.QueryAutoPointsEnabled();
    }

    /// <summary> Gets or sets the automatic Points enabled query command. </summary>
    /// <remarks> SCPI: ":TRAC:POIN:AUTO?". </remarks>
    /// <value> The automatic Points enabled query command. </value>
    protected virtual string AutoPointsEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Points Enabled sentinel. Also sets the
    /// <see cref="AutoPointsEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoPointsEnabled()
    {
        this.AutoPointsEnabled = this.Session.Query( this.AutoPointsEnabled, this.AutoPointsEnabledQueryCommand );
        return this.AutoPointsEnabled;
    }

    /// <summary> Gets or sets the automatic Points enabled command Format. </summary>
    /// <remarks> SCPI: ":TRAC:POIN:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic Points enabled query command. </value>
    protected virtual string AutoPointsEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Points Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoPointsEnabled( bool value )
    {
        this.AutoPointsEnabled = this.Session.WriteLine( value, this.AutoPointsEnabledCommandFormat );
        return this.AutoPointsEnabled;
    }

    #endregion

    #region " buffer capacity "

    /// <summary> The maximum capacity. </summary>
    private int? _maximumCapacity;

    /// <summary> Gets or sets the maximum Buffer Capacity. </summary>
    /// <value> The Maximum Buffer Capacity or none if not set or unknown. </value>
    public virtual int? MaximumCapacity
    {
        get => this._maximumCapacity;

        protected set
        {
            if ( !Nullable.Equals( this.MaximumCapacity, value ) )
            {
                this._maximumCapacity = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " points count "

    /// <summary> QueryEnum if data trace is enabled. </summary>
    /// <returns> <c>true</c> if data trace enabled; otherwise <c>false</c> </returns>
    public bool IsDataTraceEnabled()
    {
        return this.PointsCount.GetValueOrDefault( 0 ) > 0 && this.FeedSource.GetValueOrDefault( FeedSources.None ) != FeedSources.None && this.FeedControl.GetValueOrDefault( FeedControls.None ) != FeedControls.None;
    }

    /// <summary> Gets or sets the cached Trace Points Count. </summary>
    /// <value> The Trace Points Count or none if not set or unknown. </value>
    public int? PointsCount
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.PointsCount, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.ActualPointCount = value;
            }
        }
    }

    /// <summary> Writes and reads back the Trace Points Count. </summary>
    /// <param name="value"> The current PointsCount. </param>
    /// <returns> The PointsCount or none if unknown. </returns>
    public int? ApplyPointsCount( int value )
    {
        _ = this.WritePointsCount( value );
        return this.QueryPointsCount();
    }

    /// <summary> Gets or sets the points count query command. </summary>
    /// <remarks> SCPI: ":TRAC:POIN:COUN?". </remarks>
    /// <value> The points count query command. </value>
    protected virtual string PointsCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current PointsCount. </summary>
    /// <returns> The PointsCount or none if unknown. </returns>
    public int? QueryPointsCount()
    {
        if ( !string.IsNullOrWhiteSpace( this.PointsCountQueryCommand ) )
        {
            this.PointsCount = this.Session.Query( 0, this.PointsCountQueryCommand );
        }

        return this.PointsCount;
    }

    /// <summary> Gets or sets the points count command format. </summary>
    /// <remarks> SCPI: ":TRAC:POIN:COUN {0}". </remarks>
    /// <value> The points count query command format. </value>
    protected virtual string PointsCountCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// WriteEnum the Trace Points Count without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current Points Count. </param>
    /// <returns> The Points Count or none if unknown. </returns>
    public int? WritePointsCount( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.PointsCountCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.PointsCountCommandFormat, value );
        }

        this.PointsCount = value;
        return this.PointsCount;
    }

    #endregion

    #region " actual point count "

    /// <summary> Number of ActualPoint. </summary>

    /// <summary> Gets or sets the cached Trace Actual Point Count. </summary>
    /// <value> The Trace Actual Point Count or none if not set or unknown. </value>
    public int? ActualPointCount
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.ActualPointCount, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.PointsCount = value;
            }
        }
    }

    /// <summary> Gets the Actual Points count query command. </summary>
    /// <remarks> SCPI: ":TRAC:ACT?". </remarks>
    /// <value> The Actual Points count query command. </value>
    protected virtual string ActualPointCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Actual Point Count. </summary>
    /// <returns> The Actual Point Count or none if unknown. </returns>
    public virtual int? QueryActualPointCount()
    {
        if ( !string.IsNullOrWhiteSpace( this.ActualPointCountQueryCommand ) )
        {
            this.ActualPointCount = this.Session.Query( 0, this.ActualPointCountQueryCommand );
        }

        return this.ActualPointCount;
    }

    /// <summary>
    /// Gets an indication if this subsystem supports reading buffer actual points.
    /// </summary>
    /// <value> <c>true</c> if this subsystem supports reading buffer actual points. </value>
    public bool SupportsActualPointCount => !string.IsNullOrWhiteSpace( this.ActualPointCountQueryCommand );

    #endregion

    #region " first point number "

    /// <summary> Number of First Point. </summary>

    /// <summary> Gets or sets the cached buffer First Point Number. </summary>
    /// <value> The buffer First Point Number or none if not set or unknown. </value>
    public int? FirstPointNumber
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.FirstPointNumber, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets The First Point Number query command. </summary>
    /// <remarks> SCPI: ":TRAC:ACT:STA?". </remarks>
    /// <value> The First Point Number query command. </value>
    protected virtual string FirstPointNumberQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current FirstPointNumber. </summary>
    /// <returns> The First Point Number or none if unknown. </returns>
    public virtual int? QueryFirstPointNumber()
    {
        if ( !string.IsNullOrWhiteSpace( this.FirstPointNumberQueryCommand ) )
        {
            this.FirstPointNumber = this.Session.Query( 0, this.FirstPointNumberQueryCommand );
        }

        return this.FirstPointNumber;
    }

    /// <summary>
    /// Gets an indication if this subsystem supports reading buffer first point number.
    /// </summary>
    /// <value> <c>true</c> if this subsystem supports reading buffer first point number. </value>
    public bool SupportsFirstPointNumber => !string.IsNullOrWhiteSpace( this.FirstPointNumberQueryCommand );

    #endregion

    #region " last point number "

    /// <summary> Number of Last Point. </summary>

    /// <summary> Gets or sets the cached buffer Last Point Number. </summary>
    /// <value> The buffer Last Point Number or none if not set or unknown. </value>
    public int? LastPointNumber
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.LastPointNumber, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets The Last Point Number query command. </summary>
    /// <remarks> SCPI: ":TRAC:ACT:END?". </remarks>
    /// <value> The Last Point Number query command. </value>
    protected virtual string LastPointNumberQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Last Point Number. </summary>
    /// <returns> The LastPointNumber or none if unknown. </returns>
    public virtual int? QueryLastPointNumber()
    {
        if ( !string.IsNullOrWhiteSpace( this.LastPointNumberQueryCommand ) )
        {
            this.LastPointNumber = this.Session.Query( 0, this.LastPointNumberQueryCommand );
        }

        return this.LastPointNumber;
    }

    /// <summary>
    /// Gets an indication if this subsystem supports reading buffer Last point number.
    /// </summary>
    /// <value> <c>true</c> if this subsystem supports reading buffer Last point number. </value>
    public bool SupportsLastPointNumber => !string.IsNullOrWhiteSpace( this.LastPointNumberQueryCommand );

    #endregion

    #region " available point count "

    /// <summary> Number of Available Points. </summary>

    /// <summary> Gets or sets the number of points still available to fill in the buffer. </summary>
    /// <value> The Available Points Count or none if not set or unknown. </value>
    public int? AvailablePointCount
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.AvailablePointCount, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the Available Points count query command. </summary>
    /// <remarks> SCPI: ":TRAC:FREE?". </remarks>
    /// <value> The Available Points count query command. </value>
    protected virtual string AvailablePointCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Available Point Count. </summary>
    /// <returns> The Available Point Count or none if unknown. </returns>
    public int? QueryAvailablePointCount()
    {
        if ( !string.IsNullOrWhiteSpace( this.AvailablePointCountQueryCommand ) )
        {
            this.AvailablePointCount = this.Session.Query( 0, this.AvailablePointCountQueryCommand );
        }

        return this.AvailablePointCount;
    }

    #endregion

    #region " buffer free "

    /// <summary> Gets or sets the Buffer Free query command. </summary>
    /// <remarks> SCPI: ":TRAC:FREE?". </remarks>
    /// <value> The buffer free query command. </value>
    protected virtual string BufferFreePointCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the buffer free count. </summary>
    /// <returns> The buffer free and actual point count. </returns>
    public (int BufferFree, int ActualPointCount) QueryBufferFreePointCount()
    {
        int bufferFree = 0;
        int actualPointCount = 0;
        if ( !string.IsNullOrWhiteSpace( this.BufferFreePointCountQueryCommand ) )
        {
            string reading = this.Session.QueryTrimTermination( this.BufferFreePointCountQueryCommand );
            if ( !string.IsNullOrWhiteSpace( reading ) )
            {
                Queue<string> values = new( reading.Split( ',' ) );
                if ( values.Any() && int.TryParse( values.Dequeue(), out bufferFree ) )
                {
                    if ( values.Any() && int.TryParse( values.Dequeue(), out actualPointCount ) )
                    {
                    }
                }
            }
        }

        this.ActualPointCount = actualPointCount;
        this.AvailablePointCount = bufferFree;
        return (bufferFree, actualPointCount);
    }

    #endregion

    #region " data "

    /// <summary> String. </summary>

    /// <summary> Gets or sets the cached Trace Data. </summary>
    /// <value> The data. </value>
    public string? Data
    {
        get;

        protected set
        {
            if ( !Equals( this.Data, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the data query command. </summary>
    /// <remarks> SCPI: ":TRAC:DATA?". </remarks>
    /// <value> The points count query command. </value>
    protected virtual string DataQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Data. </summary>
    /// <returns> The Data or none if unknown. </returns>
    public string? QueryData()
    {
        if ( !string.IsNullOrWhiteSpace( this.DataQueryCommand ) )
        {
            _ = this.Session.WriteLine( this.DataQueryCommand );
            // read the entire data.
            this.Data = this.Session.ReadFreeLineTrimEnd();
        }

        return this.Data;
    }

    /// <summary> Queries the current Data. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns> The Data or empty if none. </returns>
    public string? QueryData( string? queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
        {
            this.Data = string.Empty;
            _ = this.Session.WriteLine( queryCommand! );
            // read the entire data.
            this.Data = this.Session.ReadFreeLineTrimEnd();
        }

        return this.Data;
    }

    /// <summary> Queries the current Data. </summary>
    /// <remarks> David, 2020-09-04. </remarks>
    /// <param name="baseReading"> The base reading. </param>
    /// <returns> The Data or empty if none. </returns>
    public virtual IList<ReadingAmounts> QueryReadings( ReadingAmounts baseReading )
    {
        string? values = this.QueryData();
        return baseReading.Parse( baseReading, values );
    }

    #endregion

    #region " fetch "

    /// <summary> Gets or sets the fetch command. </summary>
    /// <remarks> SCPI: 'FETCh?'. </remarks>
    /// <value> The fetch command. </value>
    protected virtual string FetchCommand { get; set; } = string.Empty;

    /// <summary> Fetches the data. </summary>
    /// <remarks>
    /// Issues the 'FETCH?' query, which reads data stored in the Sample Buffer. If, for example,
    /// there are 20 data arrays stored in the Sample Buffer, then all 20 data arrays will be sent to
    /// the computer when 'FETCh?' is executed. Note that FETCh? does not affect data in the Sample
    /// Buffer. Thus, subsequent executions of FETCh? acquire the same data.
    /// </remarks>
    /// <returns> The reading. </returns>
    public virtual string? FetchReading()
    {
        return this.Session.QueryTrimEnd( this.Session.EmulatedReply, this.FetchCommand );
    }

    #endregion

    #region " buffer stream "

    /// <summary> Duration of the binning. </summary>
    private TimeSpan _binningDuration;

    /// <summary> Gets or sets the duration of the binning. </summary>
    /// <value> The binning duration. </value>
    public TimeSpan BinningDuration
    {
        get => this._binningDuration;
        set
        {
            if ( value != this.BinningDuration )
            {
                this._binningDuration = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Queries the current Data. </summary>
    /// <returns> The Data or empty if none. </returns>
    public virtual IList<BufferReading> QueryBufferReadings()
    {
        int count = this.QueryActualPointCount().GetValueOrDefault( 0 );
        if ( count > 0 )
        {
            int first = this.QueryFirstPointNumber().GetValueOrDefault( 0 );
            int last = this.QueryLastPointNumber().GetValueOrDefault( 0 );
            return this.QueryBufferReadings( first, last );
        }
        else
        {
            return [];
        }
    }

    /// <summary> Gets or sets the default buffer 1 read command format. </summary>
    /// <value> The default buffer 1 read command format. </value>
    public string DefaultBuffer1ReadCommandFormat { get; set; } = ":TRAC:DATA? {0},{1},'defbuffer1',READ,TST,STAT,UNIT";

    private IEnumerable<ReadingElementTypes> _orderedReadingElementTypes;

    /// <summary> Gets or sets a list of types of the reading Elements. </summary>
    /// <value> A list of types of the reading elements. </value>
    public IEnumerable<ReadingElementTypes> OrderedReadingElementTypes
    {
        get => this._orderedReadingElementTypes;
        set
        {
            if ( !Equals( value, this.OrderedReadingElementTypes ) )
            {
                this._orderedReadingElementTypes = value;
                this.NotifyPropertyChanged();
            }

            this.ReadingElementTypes = JoinReadingElementTypes( value );
        }
    }

    /// <summary> Join reading Element types. </summary>
    /// <param name="values"> The values. </param>
    /// <returns> The combined ReadingElementTypes. </returns>
    private static ReadingElementTypes JoinReadingElementTypes( IEnumerable<ReadingElementTypes> values )
    {
        ReadingElementTypes result = default;
        foreach ( ReadingElementTypes v in values )
            result |= v;
        return result;
    }

    /// <summary> Gets or sets the type of the measured element. </summary>
    /// <value> The type of the measured element. </value>
    public ReadingElementTypes MeasuredElementType
    {
        get;
        set
        {
            if ( this.MeasuredElementType != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets a list of types of the reading elements. </summary>
    /// <value> A list of types of the reading elements. </value>
    public ReadingElementTypes ReadingElementTypes
    {
        get;
        set
        {
            if ( this.ReadingElementTypes != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Enumerates the buffer readings in this collection. </summary>
    /// <param name="commaSeparatedValues"> The comma separated values. </param>
    /// <returns>
    /// An enumerator that allows for each to be used to process the buffer readings in this
    /// collection.
    /// </returns>
    public IList<BufferReading> EnumerateBufferReadings( string commaSeparatedValues )
    {
        List<BufferReading> l = [];
        if ( !string.IsNullOrWhiteSpace( commaSeparatedValues ) )
        {
            Queue<string> q = new( commaSeparatedValues.Split( ',' ) );
            while ( q.Any() )
                l.Add( new BufferReading( this.MeasuredElementType, q, this.OrderedReadingElementTypes ) );
        }

        return l;
    }

    /// <summary> Queries the current Data. </summary>
    /// <param name="firstIndex"> Zero-based index of the first. </param>
    /// <param name="lastIndex">  Zero-based index of the last. </param>
    /// <returns> The Data or empty if none. </returns>
    public virtual IList<BufferReading> QueryBufferReadings( int firstIndex, int lastIndex )
    {
        _ = this.QueryData( string.Format( this.DefaultBuffer1ReadCommandFormat, firstIndex, lastIndex ) );
        return this.EnumerateBufferReadings( this.Data! );
    }

    /// <summary> Gets the buffer reading binding list. </summary>
    /// <value> The buffer readings. </value>
    public BufferReadingBindingList BufferReadingsBindingList { get; private set; }

    /// <summary> Gets the number of buffer readings. </summary>
    /// <value> The number of buffer readings. </value>
    public int BufferReadingsCount => this.BufferReadingsBindingList.Count;

    /// <summary> The last buffer reading. </summary>
    private BufferReading _lastBufferReading;

    /// <summary> Gets or sets the last buffer reading. </summary>
    /// <value> The last buffer reading. </value>
    public BufferReading LastBufferReading
    {
        get => this._lastBufferReading;
        set
        {
            this._lastBufferReading = value;
            if ( value is null )
            {
                this.LastBufferReadingCaption = "-.---";
                this.LastRawReading = string.Empty;
                this.LastBufferReadingStatus = string.Empty;
            }
            else
            {
                this.LastBufferReadingCaption = this.LastBufferReading.ReadingCaption;
                this.LastRawReading = this.LastBufferReading.Reading;
                this.LastBufferReadingStatus = this.LastBufferReading.StatusReading;
            }

            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Gets or sets the last buffer reading readings. </summary>
    /// <value> The last buffer reading. </value>
    public string? LastBufferReadingStatus
    {
        get;
        set
        {
            if ( !string.Equals( value, this.LastBufferReadingStatus, StringComparison.Ordinal ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the last buffer reading readings. </summary>
    /// <value> The last buffer reading. </value>
    public string? LastRawReading
    {
        get;
        set
        {
            if ( !string.Equals( value, this.LastRawReading, StringComparison.Ordinal ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The buffer reading unit. </summary>
    private cc.isr.UnitsAmounts.Unit _bufferReadingUnit;

    /// <summary> Gets or sets the buffer reading unit. </summary>
    /// <value> The buffer reading unit. </value>
    public cc.isr.UnitsAmounts.Unit BufferReadingUnit
    {
        get => this._bufferReadingUnit;

        protected set
        {
            if ( !Equals( value, this.BufferReadingUnit ) )
            {
                this._bufferReadingUnit = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the last buffer reading. </summary>
    /// <value> The last buffer reading. </value>
    public string? LastBufferReadingCaption
    {
        get;
        set
        {
            if ( !string.Equals( value, this.LastBufferReadingCaption, StringComparison.Ordinal ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the items locker. </summary>
    /// <value> The items locker. </value>
    protected ReaderWriterLockSlim ItemsLocker { get; private set; } = new ReaderWriterLockSlim();

    /// <summary> Gets a queue of new buffer readings. </summary>
    /// <value> A thread safe Queue of buffer readings. </value>
    public System.Collections.Concurrent.ConcurrentQueue<BufferReading> NewBufferReadingsQueue { get; private set; }

    /// <summary> Clears the buffer readings queue. </summary>
    public void ClearBufferReadingsQueue()
    {
        this.NewBufferReadingsQueue = new System.Collections.Concurrent.ConcurrentQueue<BufferReading>();
    }

    /// <summary> Gets the number of new readings. </summary>
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

    /// <summary> Enqueue range. </summary>
    /// <param name="items"> The items. </param>
    public void EnqueueRange( IList<BufferReading> items )
    {
        if ( items is null || !items.Any() )
            return;
        this.ItemsLocker.EnterWriteLock();
        try
        {
            foreach ( BufferReading item in items )
                this.NewBufferReadingsQueue.Enqueue( item );
        }
        finally
        {
            this.ItemsLocker.ExitWriteLock();
        }
    }

    /// <summary> Dequeues the specified number of new values. </summary>
    /// <remarks> David, 2020-08-01. </remarks>
    /// <param name="count"> Number of values to dequeue. </param>
    /// <returns>
    /// An enumerator that allows for each to be used to process dequeue range in this collection.
    /// </returns>
    public IList<BufferReading> DequeueRange( int count )
    {
        List<BufferReading> result = [];
        try
        {
            this.ItemsLocker.EnterReadLock();
            while ( this.NewBufferReadingsQueue.Any() & count > 0 )
            {
                if ( this.NewBufferReadingsQueue.TryDequeue( out BufferReading value ) )
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

    /// <summary> Adds buffer readings. </summary>
    /// <param name="bufferInfo"> Information describing the buffer. </param>
    /// <returns> An Integer. </returns>
    private BufferInfo AddBufferReadings( BufferInfo bufferInfo )
    {
        BufferInfo nextBufferInfo = bufferInfo.IsEmpty ? new BufferInfo( this.QueryFirstPointNumber().GetValueOrDefault( BufferInfo.EmptyBufferFirstBufferNumber ), BufferInfo.EmptyBufferLastBufferNumber ) : new BufferInfo( bufferInfo );
        if ( !nextBufferInfo.IsEmpty )
        {
            // if has at least one reading, get the last reading; it might be the first reading.
            nextBufferInfo = new BufferInfo( nextBufferInfo.First, this.QueryLastPointNumber().GetValueOrDefault( BufferInfo.EmptyBufferLastBufferNumber ) );
            // check if we have new readings.
            if ( nextBufferInfo.Count > this.BufferReadingsCount )
            {
                IList<BufferReading> newReadings = this.QueryBufferReadings( this.BufferReadingsCount + 1, nextBufferInfo.Last );
                foreach ( BufferReading reading in newReadings )
                    reading.BuildAmount( this.BufferReadingUnit );
                this.BufferReadingsBindingList.Add( newReadings );
                this.EnqueueRange( newReadings );
                // fixes the missing last buffer reading.
                this.LastBufferReading = this.BufferReadingsBindingList.LastReading;
            }
        }

        return nextBufferInfo;
    }

    /// <summary> Adds all buffer readings. </summary>
    /// <returns> An Integer. </returns>
    public virtual int AddAllBufferReadings()
    {
        IList<BufferReading> newReadings = this.QueryBufferReadings();
        if ( newReadings.Any() )
        {
            this.BufferReadingsBindingList.Add( newReadings );
            this.EnqueueRange( newReadings );
            this.LastBufferReading = this.BufferReadingsBindingList.LastReading;
            this.NotifyPropertyChanged( nameof( this.BufferReadingsCount ) );
        }

        return newReadings.Count;
    }

    /// <summary> Gets or sets the binning strobe requested token. </summary>
    /// <value> The binning strobe requested token. </value>
    private cc.isr.VI.Primitives.ConcurrentToken<bool> BinningStrobeRequestedToken { get; set; }

    /// <summary> Gets or sets the binning strobe requested. </summary>
    /// <value> The binning strobe requested. </value>
    public bool BinningStrobeRequested
    {
        get => this.BinningStrobeRequestedToken.Value;
        set
        {
            if ( this.BinningStrobeRequested != value )
            {
                this.BinningStrobeRequestedToken.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the binning strobe action. </summary>
    /// <value> The binning strobe action. </value>
    public Action? BinningStrobeAction { get; set; }

    /// <summary> Gets or sets the bus trigger requested token. </summary>
    /// <value> The bus trigger requested token. </value>
    private cc.isr.VI.Primitives.ConcurrentToken<bool> BusTriggerRequestedToken { get; set; }

    /// <summary> Gets or sets BUS Trigger Requested. </summary>
    /// <value> The Bus Trigger Requested value. </value>
    public bool BusTriggerRequested
    {
        get => this.BusTriggerRequestedToken.Value;
        set
        {
            if ( this.BusTriggerRequested != value )
            {
                this.BusTriggerRequestedToken.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the buffer streaming enabled token. </summary>
    /// <value> The buffer streaming enabled token. </value>
    private cc.isr.VI.Primitives.ConcurrentToken<bool> BufferStreamingEnabledToken { get; set; }

    /// <summary> Gets or sets the buffer streaming enabled. </summary>
    /// <value> The buffer streaming enabled. </value>
    public bool BufferStreamingEnabled
    {
        get => this.BufferStreamingEnabledToken.Value;

        protected set
        {
            if ( this.BufferStreamingEnabled != value )
            {
                this.BufferStreamingEnabledToken.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the buffer streaming alert token. </summary>
    /// <value> The buffer streaming alert token. </value>
    private cc.isr.VI.Primitives.ConcurrentToken<bool> BufferStreamingAlertToken { get; set; }

    /// <summary> Gets or sets the buffer streaming Alert. </summary>
    /// <value> The buffer streaming Alert. </value>
    public bool BufferStreamingAlert
    {
        get => this.BufferStreamingAlertToken.Value;
        set
        {
            if ( this.BufferStreamingAlert != value )
            {
                this.BufferStreamingAlertToken.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the buffer streaming active token. </summary>
    /// <value> The buffer streaming active token. </value>
    private cc.isr.VI.Primitives.ConcurrentToken<bool> BufferStreamingActiveToken { get; set; }

    /// <summary> Gets or sets the buffer streaming Active. </summary>
    /// <value> The buffer streaming Active. </value>
    public bool BufferStreamingActive
    {
        get => this.BufferStreamingActiveToken.Value;

        protected set
        {
            if ( this.BufferStreamingActive != value )
            {
                this.BufferStreamingActiveToken.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Duration of the stream cycle. </summary>
    private TimeSpan _streamCycleDuration;

    /// <summary> Gets or sets the duration of the stream cycle. </summary>
    /// <value> The stream cycle duration. </value>
    public TimeSpan StreamCycleDuration
    {
        get => this._streamCycleDuration;
        set
        {
            if ( !TimeSpan.Equals( value, this.StreamCycleDuration ) )
            {
                this._streamCycleDuration = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Stream buffer. </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    /// <param name="triggerSubsystem"> The trigger subsystem. </param>
    /// <param name="pollPeriod">       The poll period. </param>
    private void StreamBufferThis( TriggerSubsystemBase triggerSubsystem, TimeSpan pollPeriod )
    {
        _ = BufferInfo.Empty();
        BufferInfo previousBufferInfo = BufferInfo.Empty();
        this.BufferStreamingEnabled = true;
        this.BufferReadingsBindingList.Clear();
        this.ClearBufferReadingsQueue();
        this.BufferStreamingActive = true;
        _ = triggerSubsystem.QueryTriggerState();
        this.StreamCycleDuration = TimeSpan.FromTicks( 10L * pollPeriod.Ticks );
        Stopwatch bufferCycleDurationStopwatch = Stopwatch.StartNew();
        Stopwatch pollStopwatch = Stopwatch.StartNew();
        while ( this.BufferStreamingEnabled && triggerSubsystem.IsTriggerStateActive() )
        {
            BufferInfo currentBufferInfo = this.AddBufferReadings( previousBufferInfo );
            if ( previousBufferInfo.Count < currentBufferInfo.Count )
            {
                // if having new readings, update cycle duration and notify
                this.StreamCycleDuration = bufferCycleDurationStopwatch.Elapsed;
                bufferCycleDurationStopwatch.Restart();
                this.NotifyPropertyChanged( nameof( this.BufferReadingsCount ) );
                previousBufferInfo = currentBufferInfo;
                Pith.SessionBase.DoEventsAction?.Invoke();
                pollStopwatch.Restart();
            }

            if ( this.BusTriggerRequested )
            {
                this.BusTriggerRequested = false;
                this.Session.AssertTrigger();
            }

            _ = pollPeriod.Subtract( pollStopwatch.Elapsed ).AsyncWait();
            pollStopwatch.Restart();
            Pith.SessionBase.DoEventsAction?.Invoke();
            _ = triggerSubsystem.QueryTriggerState();
        }

        this.BufferStreamingActive = false;
    }

    /// <summary> Stream buffer. </summary>
    /// <remarks> David, 2020-07-23. </remarks>
    /// <param name="triggerSubsystem"> The trigger subsystem. </param>
    /// <param name="pollPeriod">       The poll period. </param>
    /// <param name="readingUnit">      The reading unit. </param>
    public virtual void StreamBuffer( TriggerSubsystemBase triggerSubsystem, TimeSpan pollPeriod, cc.isr.UnitsAmounts.Unit readingUnit )
    {
        try
        {
            this.BufferReadingUnit = readingUnit;
            this.StreamBufferThis( triggerSubsystem, pollPeriod );
        }
        catch
        {
            // stop buffer streaming; the exception is handled in the Async Completed event handler
            this.BufferStreamingAlert = true;
            this.BufferStreamingEnabled = false;
            this.BufferStreamingActive = false;
            throw;
        }
    }

    /// <summary> Gets the buffer stream tasker. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The buffer stream tasker. </value>
    public cc.isr.Std.Async.Tasker BufferStreamTasker { get; private set; }

    /// <summary> Starts buffer stream. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="triggerSubsystem"> The trigger subsystem. </param>
    /// <param name="pollPeriod">       The poll period. </param>
    /// <param name="readingUnit">      The reading unit. </param>
    public void StartBufferStream( TriggerSubsystemBase triggerSubsystem, TimeSpan pollPeriod, cc.isr.UnitsAmounts.Unit readingUnit )
    {
        if ( triggerSubsystem is null ) throw new ArgumentNullException( nameof( triggerSubsystem ) );
        this.BufferStreamTasker.StartAction( () => this.StreamBuffer( triggerSubsystem, pollPeriod, readingUnit ) );
    }

    /// <summary>
    /// Estimates the stream stop timeout interval as scale margin times the cycle and poll intervals.
    /// </summary>
    /// <remarks> David, 2020-04-13. </remarks>
    /// <param name="streamCycleDuration"> Duration of the stream cycle. </param>
    /// <param name="pollInterval">        The poll interval. </param>
    /// <param name="scaleMargin">         The scale margin. </param>
    /// <returns> A TimeSpan. </returns>
    public static TimeSpan EstimateStreamStopTimeoutInterval( TimeSpan streamCycleDuration, TimeSpan pollInterval, double scaleMargin )
    {
        return TimeSpan.FromTicks( ( long ) (scaleMargin * (streamCycleDuration.Ticks + pollInterval.Ticks)) );
    }

    /// <summary> Stops buffer stream and wait for completion or timeout. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public (bool Success, string Details) StopBufferStream( TimeSpan timeout )
    {
        this.BufferStreamingEnabled = false;
        (bool success, TaskStatus status) = this.BufferStreamTasker.TryAwaitCompletion( timeout );
        return status switch
        {
            TaskStatus.Created => (true, $"buffer streaming task exited with a {nameof( System.Threading.Tasks.TaskStatus )} of {status}"),
            TaskStatus.WaitingForActivation => (true, $"buffer streaming task exited with a {nameof( System.Threading.Tasks.TaskStatus )} of {status}"),
            TaskStatus.WaitingToRun => (true, $"buffer streaming task exited with a {nameof( System.Threading.Tasks.TaskStatus )} of {status}"),
            TaskStatus.Running => (false, $"buffer streaming task exited with a {nameof( System.Threading.Tasks.TaskStatus )} of {status}"),
            TaskStatus.WaitingForChildrenToComplete => (true, $"buffer streaming task exited with a {nameof( System.Threading.Tasks.TaskStatus )} of {status}"),
            TaskStatus.RanToCompletion => this.BufferStreamingActive
                                                ? (false, $"Failed stopping buffer streaming; still active")
                                                : (true, "buffer streaming completed"),
            TaskStatus.Canceled => (true, $"buffer streaming task exited with a {nameof( System.Threading.Tasks.TaskStatus )} of {status}"),
            TaskStatus.Faulted => (true, $"buffer streaming task exited with a {nameof( System.Threading.Tasks.TaskStatus )} of {status}"),
            _ => (true, $"buffer streaming task exited with a {nameof( System.Threading.Tasks.TaskStatus )} of {status}"),
        };
    }

    #endregion
}
/// <summary> Information about the buffer. </summary>
/// <remarks> David, 2020-08-01. </remarks>
public struct BufferInfo
{
    /// <summary> The empty buffer first buffer number. </summary>
    public const int EmptyBufferFirstBufferNumber = 0;

    /// <summary> The empty buffer last buffer number. </summary>
    public const int EmptyBufferLastBufferNumber = 0;

    /// <summary> Constructor. </summary>
    /// <remarks> David, 2020-08-01. </remarks>
    /// <param name="first"> The first. </param>
    /// <param name="last">  The last. </param>
    public BufferInfo( int first, int last )
    {
        this.First = first;
        this.Last = last;
    }

    /// <summary> Constructor. </summary>
    /// <remarks> David, 2020-08-01. </remarks>
    /// <param name="bufferInfo"> Information describing the buffer. </param>
    public BufferInfo( BufferInfo bufferInfo )
    {
        this.First = bufferInfo.First;
        this.Last = bufferInfo.Last;
    }

    /// <summary> Gets the empty. </summary>
    /// <remarks> David, 2020-08-01. </remarks>
    /// <returns> A BufferInfo. </returns>
    public static BufferInfo Empty()
    {
        return new BufferInfo( EmptyBufferFirstBufferNumber, EmptyBufferLastBufferNumber );
    }

    /// <summary> Gets the first. </summary>
    /// <value> The first. </value>
    public int First { get; set; }

    /// <summary> Gets the last. </summary>
    /// <value> The last. </value>
    public int Last { get; set; }

    /// <summary> Gets the is empty. </summary>
    /// <value> The is empty. </value>
    public readonly bool IsEmpty => this.First <= 0;

    /// <summary> Gets the number of. </summary>
    /// <value> The count. </value>
    public readonly int Count => this.IsEmpty ? 0 : this.Last - this.First + 1;

    /// <summary> Indicates whether this instance and a specified object are equal. </summary>
    /// <param name="obj"> The object to compare with the current instance. </param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and
    /// represent the same value; otherwise, <see langword="false" />.
    /// </returns>
    public override readonly bool Equals( object obj )
    {
        return this.Equals( ( BufferInfo ) obj );
    }

    /// <summary> Tests if this BufferInfo is considered equal to another. </summary>
    /// <param name="other"> The buffer information to compare to this object. </param>
    /// <returns> True if the objects are considered equal, false if they are not. </returns>
    public readonly bool Equals( BufferInfo other )
    {
        return this.First == other.First && this.Last == other.Last;
    }

    /// <summary> Returns the hash code for this instance. </summary>
    /// <exception cref="NotImplementedException"> Thrown when the requested operation is
    /// unimplemented. </exception>
    /// <returns> A 32-bit signed integer that is the hash code for this instance. </returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine( this.First, this.Last );
    }

    /// <summary> Compares BufferInfo. </summary>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( BufferInfo left, BufferInfo right )
    {
        return left.Equals( right );
    }

    /// <summary> Compares BufferInfo. </summary>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( BufferInfo left, BufferInfo right )
    {
        return !(left == right);
    }
}
