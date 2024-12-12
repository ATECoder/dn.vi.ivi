using System.Diagnostics;
using cc.isr.Std.TimeSpanExtensions;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by a Buffer Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="BufferSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class BufferSubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem ), IDisposable
{
    #region " construction and cleanup "

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
    /// False if this
    /// method releases
    /// only unmanaged
    /// resources. </param>
    protected virtual void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;
        try
        {
            if ( disposing )
            {
                this.ItemsLocker?.Dispose();
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
        this.Capacity = 0;
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

    #region " fill once enabled "

    /// <summary> Fill Once enabled. </summary>
    private bool? _fillOnceEnabled;

    /// <summary> Gets or sets the cached Fill Once Enabled sentinel. </summary>
    /// <remarks> When this is enabled, a delay is added before each measurement. </remarks>
    /// <value>
    /// <c>null</c> if Fill Once Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? FillOnceEnabled
    {
        get => this._fillOnceEnabled;

        protected set
        {
            if ( !Equals( this.FillOnceEnabled, value ) )
            {
                this._fillOnceEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Fill Once Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyFillOnceEnabled( bool value )
    {
        _ = this.WriteFillOnceEnabled( value );
        return this.QueryFillOnceEnabled();
    }

    /// <summary> Gets or sets the automatic Delay enabled query command. </summary>
    /// <value> The automatic Delay enabled query command. </value>
    protected virtual string FillOnceEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Fill Once Enabled sentinel. Also sets the
    /// <see cref="FillOnceEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryFillOnceEnabled()
    {
        this.FillOnceEnabled = this.Session.Query( this.FillOnceEnabled, this.FillOnceEnabledQueryCommand );
        return this.FillOnceEnabled;
    }

    /// <summary> Gets or sets the automatic Delay enabled command Format. </summary>
    /// <value> The automatic Delay enabled query command. </value>
    protected virtual string FillOnceEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Fill Once Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteFillOnceEnabled( bool value )
    {
        this.FillOnceEnabled = this.Session.WriteLine( value, this.FillOnceEnabledCommandFormat );
        return this.FillOnceEnabled;
    }

    #endregion

    #region " capacity "

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

    /// <summary> The capacity. </summary>
    private int? _capacity;

    /// <summary> Gets or sets the cached Buffer Capacity. </summary>
    /// <value> The Buffer Capacity or none if not set or unknown. </value>
    public virtual int? Capacity
    {
        get => this._capacity;

        protected set
        {
            if ( !Nullable.Equals( this.Capacity, value ) )
            {
                this._capacity = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Buffer Capacity. </summary>
    /// <param name="value"> The current Capacity. </param>
    /// <returns> The Capacity or none if unknown. </returns>
    public int? ApplyCapacity( int value )
    {
        _ = this.WriteCapacity( value );
        return this.QueryCapacity();
    }

    /// <summary> Gets or sets the points count query command. </summary>
    /// <remarks> SCPI: :TRAC:POIN:COUN? </remarks>
    /// <value> The points count query command. </value>
    protected virtual string CapacityQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Capacity. </summary>
    /// <returns> The Capacity or none if unknown. </returns>
    public int? QueryCapacity()
    {
        if ( !string.IsNullOrWhiteSpace( this.CapacityQueryCommand ) )
        {
            this.Capacity = this.Session.Query( 0, this.CapacityQueryCommand );
        }

        return this.Capacity;
    }

    /// <summary> Gets or sets the points count command format. </summary>
    /// <remarks> SCPI: :TRAC:POIN:COUN {0} </remarks>
    /// <value> The points count query command format. </value>
    protected virtual string CapacityCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Buffer Capacity without reading back the value from the device. </summary>
    /// <param name="value"> The current Capacity. </param>
    /// <returns> The Capacity or none if unknown. </returns>
    public int? WriteCapacity( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.CapacityCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.CapacityCommandFormat, value );
        }

        this.Capacity = value;
        return this.Capacity;
    }

    #endregion

    #region " actual point count "

    /// <summary> Number of ActualPoint. </summary>
    private int? _actualPointCount;

    /// <summary> Gets or sets the cached Buffer ActualPointCount. </summary>
    /// <value> The Buffer ActualPointCount or none if not set or unknown. </value>
    public int? ActualPointCount
    {
        get => this._actualPointCount;

        protected set
        {
            if ( !Nullable.Equals( this.ActualPointCount, value ) )
            {
                this._actualPointCount = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the ActualPoint count query command. </summary>
    /// <remarks> SCPI: :TRAC:ACT? </remarks>
    /// <value> The ActualPoint count query command. </value>
    protected virtual string ActualPointCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current ActualPointCount. </summary>
    /// <returns> The ActualPointCount or none if unknown. </returns>
    public int? QueryActualPointCount()
    {
        if ( !string.IsNullOrWhiteSpace( this.ActualPointCountQueryCommand ) )
        {
            this.ActualPointCount = this.Session.Query( 0, this.ActualPointCountQueryCommand );
        }

        return this.ActualPointCount;
    }

    #endregion

    #region " first point number "

    /// <summary> Number of First Point. </summary>
    private int? _firstPointNumber;

    /// <summary> Gets or sets the cached buffer First Point Number. </summary>
    /// <value> The buffer First Point Number or none if not set or unknown. </value>
    public int? FirstPointNumber
    {
        get => this._firstPointNumber;

        protected set
        {
            if ( !Nullable.Equals( this.FirstPointNumber, value ) )
            {
                this._firstPointNumber = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets The First Point Number query command. </summary>
    /// <remarks> SCPI: :TRAC:ACT:STA? </remarks>
    /// <value> The First Point Number query command. </value>
    protected virtual string FirstPointNumberQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current FirstPointNumber. </summary>
    /// <returns> The First Point Number or none if unknown. </returns>
    public int? QueryFirstPointNumber()
    {
        if ( !string.IsNullOrWhiteSpace( this.FirstPointNumberQueryCommand ) )
        {
            this.FirstPointNumber = this.Session.Query( 0, this.FirstPointNumberQueryCommand );
        }

        return this.FirstPointNumber;
    }

    #endregion

    #region " last point number "

    /// <summary> Number of Last Point. </summary>
    private int? _lastPointNumber;

    /// <summary> Gets or sets the cached buffer Last Point Number. </summary>
    /// <value> The buffer Last Point Number or none if not set or unknown. </value>
    public int? LastPointNumber
    {
        get => this._lastPointNumber;

        protected set
        {
            if ( !Nullable.Equals( this.LastPointNumber, value ) )
            {
                this._lastPointNumber = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets The Last Point Number query command. </summary>
    /// <remarks> SCPI: :TRAC:ACT:END? </remarks>
    /// <value> The Last Point Number query command. </value>
    protected virtual string LastPointNumberQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Last Point Number. </summary>
    /// <returns> The LastPointNumber or none if unknown. </returns>
    public int? QueryLastPointNumber()
    {
        if ( !string.IsNullOrWhiteSpace( this.LastPointNumberQueryCommand ) )
        {
            this.LastPointNumber = this.Session.Query( 0, this.LastPointNumberQueryCommand );
        }

        return this.LastPointNumber;
    }

    #endregion

    #region " available point count "

    /// <summary> Number of Available Points. </summary>
    private int? _availablePointCount;

    /// <summary> Gets or sets the number of points still available to fill in the buffer. </summary>
    /// <value> The Available Points Count or none if not set or unknown. </value>
    public int? AvailablePointCount
    {
        get => this._availablePointCount;

        protected set
        {
            if ( !Nullable.Equals( this.AvailablePointCount, value ) )
            {
                this._availablePointCount = value;
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
        if ( !string.IsNullOrWhiteSpace( this.ActualPointCountQueryCommand ) )
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
    private string? _data;

    /// <summary> Gets or sets the cached Buffer Data. </summary>
    /// <value> The data. </value>
    public string? Data
    {
        get => this._data;

        protected set
        {
            if ( !Equals( this.Data, value ) )
            {
                this._data = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the data query command. </summary>
    /// <remarks> SCPI: ":TRAC:DATA?". </remarks>
    /// <value> The data query command. </value>
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
    public string? QueryData( string queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
        {
            this.Data = string.Empty;
            _ = this.Session.WriteLine( queryCommand );

            // read the entire data.
            this.Data = this.Session.ReadFreeLineTrimEnd();
        }

        return this.Data;
    }

    #endregion

    #region " buffer stream "

    /// <summary> Queries the current Data. </summary>
    /// <returns> The Data or empty if none. </returns>
    public IList<BufferReading> QueryBufferReadings()
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

    /// <summary> Gets or sets the buffer read command format. </summary>
    /// <value> The buffer read command format. </value>
    public virtual string BufferReadCommandFormat { get; set; } = ":TRAC:DATA? {0},{1},'defbuffer1',READ,TST,STAT,UNIT";

    /// <summary> Queries the current Data. </summary>
    /// <param name="firstIndex"> Zero-based index of the first. </param>
    /// <param name="lastIndex">  Zero-based index of the last. </param>
    /// <returns> The Data or empty if none. </returns>
    public IList<BufferReading> QueryBufferReadings( int firstIndex, int lastIndex )
    {
        _ = this.QueryData( string.Format( this.BufferReadCommandFormat, firstIndex, lastIndex ) );
        return this.Data is null
            ? []
            : this.EnumerateBufferReadings( this.Data );
    }

    private IEnumerable<ReadingElementTypes> _orderedReadingElementTypes = [ReadingElementTypes.Reading, ReadingElementTypes.Timestamp, ReadingElementTypes.Status, ReadingElementTypes.Units];

    /// <summary> Gets or sets a list of types of the readings. </summary>
    /// <value> A list of types of the readings. </value>
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

    /// <summary> Type of the measured element. </summary>
    private ReadingElementTypes _measuredElementType;

    /// <summary> Gets or sets the type of the measured element. </summary>
    /// <value> The type of the measured element. </value>
    public ReadingElementTypes MeasuredElementType
    {
        get => this._measuredElementType;
        set
        {
            if ( this.MeasuredElementType != value )
            {
                this._measuredElementType = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> List of types of the reading elements. </summary>
    private ReadingElementTypes _readingElementTypes;

    /// <summary> Gets or sets a list of types of the reading elements. </summary>
    /// <value> A list of types of the reading elements. </value>
    public ReadingElementTypes ReadingElementTypes
    {
        get => this._readingElementTypes;
        set
        {
            if ( this.ReadingElementTypes != value )
            {
                this._readingElementTypes = value;
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
            {
                BufferReading reading = new( this.MeasuredElementType, q, this.OrderedReadingElementTypes );
                if ( !this.OrderedReadingElementTypes.Contains( ReadingElementTypes.Units ) )
                {
                    reading.BuildAmount( this.BufferReadingUnit );
                }

                l.Add( reading );
            }
        }

        return l;
    }

    /// <summary> Gets a queue of new buffer readings. </summary>
    /// <value> A thread safe Queue of buffer readings. </value>
    public System.Collections.Concurrent.ConcurrentQueue<BufferReading> NewBufferReadingsQueue { get; private set; } = [];

    /// <summary> Gets the buffer reading binding list. </summary>
    /// <value> The buffer readings. </value>
    public BufferReadingBindingList BufferReadingsBindingList { get; private set; } = [];

    /// <summary> Gets the number of buffer readings. </summary>
    /// <value> The number of buffer readings. </value>
    public int BufferReadingsCount => this.BufferReadingsBindingList.Count;

    /// <summary> The buffer reading unit. </summary>
    private cc.isr.UnitsAmounts.Unit _bufferReadingUnit = new();

    /// <summary> Gets or sets the buffer reading unit. </summary>
    /// <value> The buffer reading unit. </value>
    public cc.isr.UnitsAmounts.Unit BufferReadingUnit
    {
        get => this._bufferReadingUnit;
        set
        {
            if ( !Equals( value, this.BufferReadingUnit ) )
            {
                this._bufferReadingUnit = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The last reading. </summary>
    private BufferReading _lastReading = new();

    /// <summary> Gets or sets the last buffer reading. </summary>
    /// <value> The last buffer reading. </value>
    public BufferReading LastReading
    {
        get => this._lastReading;
        set
        {
            this._lastReading = value;
            if ( value is null )
            {
                this.LastReadingCaption = "-.---";
                this.LastRawReading = string.Empty;
                this.LastReadingStatus = string.Empty;
            }
            else
            {
                this.LastReadingCaption = this.LastReading.ReadingCaption;
                this.LastRawReading = this.LastReading.Reading;
                this.LastReadingStatus = this.LastReading.StatusReading;
            }
        }
    }

    /// <summary> The last reading status. </summary>
    private string? _lastReadingStatus;

    /// <summary> Gets or sets the last buffer reading readings. </summary>
    /// <value> The last buffer reading. </value>
    public string? LastReadingStatus
    {
        get => this._lastReadingStatus;
        set
        {
            if ( !string.Equals( value, this.LastReadingStatus, StringComparison.Ordinal ) )
            {
                this._lastReadingStatus = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The last raw reading. </summary>
    private string? _lastRawReading;

    /// <summary> Gets or sets the last buffer reading readings. </summary>
    /// <value> The last buffer reading. </value>
    public string? LastRawReading
    {
        get => this._lastRawReading;
        set
        {
            if ( !string.Equals( value, this.LastRawReading, StringComparison.Ordinal ) )
            {
                this._lastRawReading = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The last reading caption. </summary>
    private string? _lastReadingCaption;

    /// <summary> Gets or sets the last buffer reading. </summary>
    /// <value> The last buffer reading. </value>
    public string? LastReadingCaption
    {
        get => this._lastReadingCaption;
        set
        {
            if ( !string.Equals( value, this.LastReadingCaption, StringComparison.Ordinal ) )
            {
                this._lastReadingCaption = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the items locker. </summary>
    /// <value> The items locker. </value>
    protected ReaderWriterLockSlim ItemsLocker { get; private set; } = new ReaderWriterLockSlim();

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

    /// <summary> Enumerates dequeue range in this collection. </summary>
    /// <returns>
    /// An enumerator that allows for each to be used to process dequeue range in this collection.
    /// </returns>
    public IList<BufferReading> DequeueRange()
    {
        List<BufferReading> result = [];
        while ( this.NewBufferReadingsQueue.Any() )
        {
            if ( this.NewBufferReadingsQueue.TryDequeue( out BufferReading value ) )
            {
                result.Add( value );
            }
        }

        return result;
    }

    /// <summary> The buffer streaming enabled. </summary>
    private readonly Std.Concurrent.ConcurrentToken<bool> _bufferStreamingEnabled = new();

    /// <summary> Gets or sets the buffer streaming enabled. </summary>
    /// <value> The buffer streaming enabled. </value>
    public bool BufferStreamingEnabled
    {
        get => this._bufferStreamingEnabled.Value;
        set
        {
            if ( this.BufferStreamingEnabled != value )
            {
                this._bufferStreamingEnabled.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The buffer streaming active. </summary>
    private readonly Std.Concurrent.ConcurrentToken<bool> _bufferStreamingActive = new();

    /// <summary> Gets or sets the buffer streaming Active. </summary>
    /// <value> The buffer streaming Active. </value>
    public bool BufferStreamingActive
    {
        get => this._bufferStreamingActive.Value;
        set
        {
            if ( this.BufferStreamingActive != value )
            {
                this._bufferStreamingActive.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The buffer streaming alert. </summary>
    private readonly Std.Concurrent.ConcurrentToken<bool> _bufferStreamingAlert = new();

    /// <summary> Gets or sets the buffer streaming Alert. </summary>
    /// <value> The buffer streaming Alert. </value>
    public bool BufferStreamingAlert
    {
        get => this._bufferStreamingAlert.Value;
        set
        {
            if ( this.BufferStreamingAlert != value )
            {
                this._bufferStreamingAlert.Value = value;
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
        int first = 0;
        this.BufferStreamingEnabled = true;
        this.BufferReadingsBindingList.Clear();
        this.NewBufferReadingsQueue = new System.Collections.Concurrent.ConcurrentQueue<BufferReading>();
        _ = triggerSubsystem.QueryTriggerState();
        this.BufferStreamingActive = true;
        Stopwatch pollStopwatch = Stopwatch.StartNew();
        while ( triggerSubsystem.IsTriggerStateActive() && this.BufferStreamingEnabled )
        {
            if ( first == 0 )
                first = this.QueryFirstPointNumber().GetValueOrDefault( 0 );
            if ( first > 0 )
            {
                int last = this.QueryLastPointNumber().GetValueOrDefault( 0 );
                if ( last - first + 1 > this.BufferReadingsCount )
                {
                    IList<BufferReading> newReadings = this.QueryBufferReadings( this.BufferReadingsCount + 1, last );
                    this.BufferReadingsBindingList.Add( newReadings );
                    this.EnqueueRange( newReadings );
                    this.NotifyPropertyChanged( nameof( this.BufferReadingsCount ) );
                    Pith.SessionBase.DoEventsAction?.Invoke();
                    pollStopwatch.Restart();
                }
            }

            pollPeriod.Subtract( pollStopwatch.Elapsed ).AsyncWait();
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
    public virtual void StreamBuffer( TriggerSubsystemBase triggerSubsystem, TimeSpan pollPeriod )
    {
        try
        {
            this.StreamBufferThis( triggerSubsystem, pollPeriod );
        }
        catch
        {
            // stop buffer streaming; the exception is handled in the Async Completed event handler
            this.BufferStreamingActive = false;
            this.BufferStreamingAlert = true;
            this.BufferStreamingEnabled = false;
        }
    }

    /// <summary> Gets or sets the buffer stream tasker. </summary>
    /// <value> The buffer stream tasker. </value>
    public Std.Async.Tasker BufferStreamTasker { get; private set; } = new Std.Async.Tasker();

    /// <summary> Starts buffer stream. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="triggerSubsystem"> The trigger subsystem. </param>
    /// <param name="pollPeriod">       The poll period. </param>
    public void StartBufferStream( TriggerSubsystemBase triggerSubsystem, TimeSpan pollPeriod )
    {
        if ( triggerSubsystem is null ) throw new ArgumentNullException( nameof( triggerSubsystem ) );
        this.BufferStreamTasker.StartAction( () => this.StreamBuffer( triggerSubsystem, pollPeriod ) );
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
        (bool Success, string Details) result;
        this.BufferStreamingEnabled = false;
        TaskStatus status = this.BufferStreamTasker.AwaitCompletion( timeout );
        result = System.Threading.Tasks.TaskStatus.RanToCompletion == status ? this.BufferStreamingActive ? (false, $"Failed stopping buffer streaming; still active") : (true, "buffer streaming completed") : (false, $"buffer streaming task existed with a {status} {nameof( System.Threading.Tasks.TaskStatus )}");
        return result;
    }

    #endregion
}
