using cc.isr.Std.Primitives;
using cc.isr.VI.Pith;

namespace cc.isr.VI;

/// <summary> Defines the SCPI Digital Output subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-05. Created based on SCPI 5.1 library.  </para><para>
/// David, 2008-03-25, 5.0.3004. Port to new SCPI library. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class DigitalOutputSubsystemBase : SubsystemBase, IDisposable
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="DigitalOutputSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected DigitalOutputSubsystemBase( StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.OutputModeReadWrites = [];
        this.DefineOutputModeReadWrites();
        this.DigitalActiveLevelReadWrites = [];
        this.DefineDigitalActiveLevelReadWrites();
        this.DigitalOutputLines = [];
        this.BinningStrobeTasker = new Std.Async.Tasker();
        // make sure the dispatcher is instantiated. This takes a bit of time the first time around
        VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> True to disposed value. </summary>
    private bool _disposedValue;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    /// resources.
    /// </summary>
    /// <remarks> David, 2020-07-17. </remarks>
    /// <param name="disposing"> True to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    protected virtual void Dispose( bool disposing )
    {
        if ( !this._disposedValue )
        {
            if ( disposing )
            {
                this.BinningStrobeTasker?.Dispose();
            }

            this._disposedValue = true;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    /// resources.
    /// </summary>
    /// <remarks> David, 2020-07-17. </remarks>
    public void Dispose()
    {
        this.Dispose( disposing: true );
        GC.SuppressFinalize( this );
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
        this.BitSize = 4;
        this.Level = 15;
        this.AutoClearEnabled = false;
        this._delayRange = new Range<TimeSpan>( TimeSpan.Zero, TimeSpan.FromSeconds( 60d ) );
        this.NotifyPropertyChanged( nameof( this.DelayRange ) );
        this.Delay = TimeSpan.FromSeconds( 0.0001d );
        this.OutputMode = OutputModes.EndTest;
        this.CurrentDigitalActiveLevel = DigitalActiveLevels.Low;
    }

    #endregion

    #region " commands "

    /// <summary> Gets or sets the Digital Outputs clear command. </summary>
    /// <remarks> SCPI: ":SOUR2:CLE". </remarks>
    /// <value> The Digital Outputs clear command. </value>
    protected virtual string ClearCommand { get; set; } = string.Empty;

    /// <summary> Clears Digital Outputs. </summary>
    public void ClearOutput()
    {
        if ( !string.IsNullOrWhiteSpace( this.ClearCommand ) )
            _ = this.Session.WriteLine( this.ClearCommand );
    }

    #endregion

    #region " auto clear enabled "

    /// <summary> The automatic clear enabled. </summary>
    private bool? _autoClearEnabled;

    /// <summary> Gets or sets the cached Digital Outputs Auto Clear enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Digital Outputs Auto Clear enabled is not known; <c>true</c> if output is on;
    /// otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoClearEnabled
    {
        get => this._autoClearEnabled;

        protected set
        {
            if ( !Equals( this.AutoClearEnabled, value ) )
            {
                this._autoClearEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Digital Outputs Auto Clear enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if Enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoClearEnabled( bool value )
    {
        _ = this.WriteAutoClearEnabled( value );
        return this.QueryAutoClearEnabled();
    }

    /// <summary> Gets or sets the Digital Outputs Auto Clear enabled query command. </summary>
    /// <remarks> SCPI: ":SOUR2:CLE:AUTO?". </remarks>
    /// <value> The Digital Outputs Auto Clear enabled query command. </value>
    protected virtual string AutoClearEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Delay Enabled sentinel. Also sets the
    /// <see cref="AutoClearEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoClearEnabled()
    {
        this.AutoClearEnabled = this.Session.Query( this.AutoClearEnabled, this.AutoClearEnabledQueryCommand );
        return this.AutoClearEnabled;
    }

    /// <summary> Gets or sets the Digital Outputs Auto Clear enabled command Format. </summary>
    /// <remarks> SCPI: ":SOUR2:CLE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Digital Outputs Auto Clear enabled query command. </value>
    protected virtual string AutoClearEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Delay Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoClearEnabled( bool value )
    {
        this.AutoClearEnabled = this.Session.WriteLine( value, this.AutoClearEnabledCommandFormat );
        return this.AutoClearEnabled;
    }

    #endregion

    #region " bit size "

    /// <summary> Size of the bit. </summary>
    private int? _bitSize;

    /// <summary> Gets or sets the cached Digital Outputs Bit Size. </summary>
    /// <value> The Digital Outputs Bit Size or none if not set or unknown. </value>
    public int? BitSize
    {
        get => this._bitSize;

        protected set
        {
            if ( !Nullable.Equals( this.BitSize, value ) )
            {
                this._bitSize = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Digital Outputs Bit Size. </summary>
    /// <param name="value"> The current Digital Outputs Bit Size. </param>
    /// <returns> The Digital Outputs Bit Size or none if unknown. </returns>
    public int? ApplyBitSize( int value )
    {
        _ = this.WriteBitSize( value );
        return this.QueryBitSize();
    }

    /// <summary> Gets or sets the Lower Limit Bit Size query command. </summary>
    /// <remarks> SCPI: ":SOUR2:BSIZ?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string BitSizeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Lower Limit Bit Size. </summary>
    /// <returns> The Lower Limit Bit Size or none if unknown. </returns>
    public int? QueryBitSize()
    {
        this.BitSize = this.Session.Query( this.BitSize, this.BitSizeQueryCommand );
        return this.BitSize;
    }

    /// <summary> Gets or sets the Lower Limit Bit Size query command. </summary>
    /// <remarks> SCPI: "::SOUR2:BSIZ {0}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string BitSizeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Sets back the Lower Limit Bit Size without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current Lower Limit Bit Size. </param>
    /// <returns> The Lower Limit Bit Size or none if unknown. </returns>
    public int? WriteBitSize( int value )
    {
        this.BitSize = this.Session.WriteLine( value, this.BitSizeCommandFormat );
        return this.BitSize;
    }

    #endregion

    #region " delay (pulse width) "

    /// <summary> The delay range. </summary>
    private Range<TimeSpan> _delayRange = new( TimeSpan.Zero, TimeSpan.Zero );

    /// <summary> Gets or sets the delay (pulse width) range. </summary>
    /// <value> The delay range. </value>
    public virtual Range<TimeSpan> DelayRange
    {
        get => this._delayRange;
        set
        {
            if ( this.DelayRange != value )
            {
                this._delayRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The delay. </summary>
    private TimeSpan? _delay;

    /// <summary> Gets or sets the cached output delay (pulse width). </summary>
    /// <remarks>
    /// The delay is used to delay operation in the Source layer. After the programmed Source event
    /// occurs, the instrument waits until the delay period expires before performing the Device
    /// Action.
    /// </remarks>
    /// <value> The output delay (pulse width) or none if not set or unknown. </value>
    public TimeSpan? Delay
    {
        get => this._delay;

        protected set
        {
            if ( !Nullable.Equals( this.Delay, value ) )
            {
                this._delay = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the output delay (pulse width). </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The output delay (pulse width) or none if unknown. </returns>
    public TimeSpan? ApplyDelay( TimeSpan value )
    {
        _ = this.WriteDelay( value );
        _ = this.QueryDelay();
        return default;
    }

    /// <summary> Gets or sets the delay query command. </summary>
    /// <remarks> SCPI: ":SOUR:DEL?". </remarks>
    /// <value> The delay query command. </value>
    protected virtual string DelayQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets or sets the Delay format for converting the query to time span. </summary>
    /// <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    /// <value> The Delay query command. </value>
    protected virtual string DelayFormat { get; set; } = string.Empty;

    /// <summary> Queries the Delay. </summary>
    /// <returns> The Delay or none if unknown. </returns>
    public TimeSpan? QueryDelay()
    {
        this.Delay = this.Session.Query( this.Delay, this.DelayFormat, this.DelayQueryCommand );
        return this.Delay;
    }

    /// <summary> Gets or sets the delay command format. </summary>
    /// <remarks> SCPI: ":SOUR:DEL {0:s\.FFFFFFF}". </remarks>
    /// <value> The delay command format. </value>
    protected virtual string DelayCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the output delay (pulse width) without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The output delay (pulse width) or none if unknown. </returns>
    public TimeSpan? WriteDelay( TimeSpan value )
    {
        _ = this.Session.WriteLine( value, this.DelayCommandFormat );
        this.Delay = value;
        return this.Delay;
    }

    #endregion

    #region " level "

    /// <summary> The level. </summary>
    private int? _level;

    /// <summary> Gets or sets the cached Digital Outputs Level. </summary>
    /// <value> The Digital Outputs Level or none if not set or unknown. </value>
    public int? Level
    {
        get => this._level;

        protected set
        {
            if ( !Nullable.Equals( this.Level, value ) )
            {
                this._level = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Digital Outputs Level. </summary>
    /// <param name="value"> The current Digital Outputs Level. </param>
    /// <returns> The Digital Outputs Level or none if unknown. </returns>
    public int? ApplyLevel( int value )
    {
        _ = this.WriteLevel( value );
        return this.QueryLevel();
    }

    /// <summary> Gets or sets the Level query command. </summary>
    /// <remarks> SCPI: ":SOUR2:TTL:ACT?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string LevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Lower Limit Level. </summary>
    /// <returns> The Level or none if unknown. </returns>
    public int? QueryLevel()
    {
        this.Level = this.Session.Query( this.Level, this.LevelQueryCommand );
        return this.Level;
    }

    /// <summary> Gets or sets the Level query command. </summary>
    /// <remarks> SCPI: "::SOUR2:TTL:LEVEL {0}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string LevelCommandFormat { get; set; } = string.Empty;

    /// <summary> Sets back the Level without reading back the value from the device. </summary>
    /// <param name="value"> The current Lower Limit Level. </param>
    /// <returns> The Level or none if unknown. </returns>
    public int? WriteLevel( int value )
    {
        this.Level = this.Session.WriteLine( value, this.LevelCommandFormat );
        return this.Level;
    }

    #endregion

    #region " line level "

    /// <summary> Writes and reads back the Digital Outputs LineLevel. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="value">      The current Digital Outputs LineLevel. </param>
    /// <returns> The Digital Outputs Line Level or none if unknown. </returns>
    public int? ApplyLineLevel( int lineNumber, int value )
    {
        _ = this.WriteLineLevel( lineNumber, value );
        return this.QueryLineLevel( lineNumber );
    }

    /// <summary> Gets or sets the Line Level query command. </summary>
    /// <remarks> SCPI: ":SOUR:TTL{0}:LEV?". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string LineLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Line Level. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <returns> The Line Level or none if unknown. </returns>
    public int? QueryLineLevel( int lineNumber )
    {
        int? value = this.Session.Query( lineNumber, string.Format( this.LineLevelQueryCommand, lineNumber ) );
        this.DigitalOutputLines.Update( lineNumber, value );
        this.NotifyPropertyChanged( nameof( this.DigitalOutputLines ) );
        return value;
    }

    /// <summary> Gets or sets the Line Level query command. </summary>
    /// <remarks> SCPI: "::SOUR2:TTL{0}:LEV {{0}}". </remarks>
    /// <value> The Limit enabled query command. </value>
    protected virtual string LineLevelCommandFormat { get; set; } = string.Empty;

    /// <summary> Sets back the Line Level without reading back the value from the device. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="value">      The current Line Level. </param>
    /// <returns> The Line Level or none if unknown. </returns>
    public int? WriteLineLevel( int lineNumber, int value )
    {
        int result = this.Session.WriteLine( value, string.Format( this.LineLevelCommandFormat, lineNumber ) );
        this.DigitalOutputLines.Update( lineNumber, result );
        this.NotifyPropertyChanged( nameof( this.DigitalOutputLines ) );
        return result;
    }

    /// <summary>   Outputs a pulse. </summary>
    /// <remarks>   David, 2020-07-17. </remarks>
    /// <param name="lineNumber">   The strobe line. </param>
    /// <param name="lineLevel">    The line level. </param>
    /// <param name="duration">     Duration of the strobe. </param>
    public void Pulse( int lineNumber, int lineLevel, TimeSpan duration )
    {
        _ = this.WriteLineLevel( lineNumber, lineLevel );
        _ = SessionBase.AsyncDelay( duration );
        _ = this.WriteLineLevel( lineNumber, lineLevel == 0 ? 1 : 0 );
    }

    /// <summary> Outputs to bin line and strobes the strobe line. </summary>
    /// <remarks> David, 2020-07-17. </remarks>
    /// <param name="strobeLine">     The strobe line. </param>
    /// <param name="strobeDuration"> Duration of the strobe. </param>
    /// <param name="binLine">        The bin line. </param>
    /// <param name="binDuration">    Duration of the bin. </param>
    public void Strobe( int strobeLine, TimeSpan strobeDuration, int binLine, TimeSpan binDuration )
    {
        if ( binDuration <= strobeDuration )
            binDuration = strobeDuration.Add( TimeSpan.FromMilliseconds( 1d ) );
        _ = this.WriteLineLevel( binLine, 1 );
        _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 0.5d * (binDuration - strobeDuration).TotalMilliseconds ) );
        _ = this.WriteLineLevel( strobeLine, 1 );
        _ = SessionBase.AsyncDelay( strobeDuration );
        _ = this.WriteLineLevel( strobeLine, 0 );
        _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 0.5d * (binDuration - strobeDuration).TotalMilliseconds ) ); ;
        _ = this.WriteLineLevel( binLine, 0 );
    }

    /// <summary> Gets or sets the tasker. </summary>
    /// <value> The tasker. </value>
    public Std.Async.Tasker BinningStrobeTasker { get; private set; }

    /// <summary> Starts strobe task. </summary>
    /// <remarks> David, 2020-07-17. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="strobeLine">     The strobe line. </param>
    /// <param name="strobeDuration"> Duration of the strobe. </param>
    /// <param name="binLine">        The bin line. </param>
    /// <param name="binDuration">    Duration of the bin. </param>
    public void StartStrobeTask( int strobeLine, TimeSpan strobeDuration, int binLine, TimeSpan binDuration )
    {
        if ( this.BinningStrobeTasker?.IsBusy() == true )
            throw new InvalidOperationException( $"{nameof( Std.Async.Tasker )} is busy" );
        this.BinningStrobeTasker?.StartAction( () => this.Strobe( strobeLine, strobeDuration, binLine, binDuration ) );
    }

    #endregion
}
