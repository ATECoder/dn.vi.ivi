namespace cc.isr.VI;

/// <summary> Defines a SCPI arm layer base Subsystem. </summary>
/// <remarks>
/// (c) 2010 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-05, . based on SCPI 5.1 library. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class ArmLayerSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmLayerSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected ArmLayerSubsystemBase( StatusSubsystemBase statusSubsystem ) : this( 1, statusSubsystem )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArmLayerSubsystemBase" /> class.
    /// </summary>
    /// <param name="layerNumber">     The arm layer number. </param>
    /// <param name="statusSubsystem"> A reference to a
    /// <see cref="StatusSubsystemBase">status subsystem</see>. </param>
    protected ArmLayerSubsystemBase( int layerNumber, StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.LayerNumber = layerNumber;
        this.ArmLayerBypassModeReadWrites = [];
        this.DefineArmLayerBypassModeReadWrites();
        this.ArmSourceReadWrites = [];
        this.DefineArmSourceReadWrites();
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
        this.ArmCount = 1;
        this.Delay = TimeSpan.Zero;
        this.InputLineNumber = 2;
        this.OutputLineNumber = 1;
        this.TimerInterval = TimeSpan.FromSeconds( 1d );
        this.ArmSource = ArmSources.Immediate;
        this.ArmLayerBypassMode = TriggerLayerBypassModes.Acceptor;
        this.MaximumArmCount = 99999;
        this.MaximumDelay = TimeSpan.FromSeconds( 999999.999d );
    }

    #endregion

    #region " commands "

    /// <summary> Gets or sets the Immediate command. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:IMM". </remarks>
    /// <value> The Immediate command. </value>
    protected virtual string ImmediateCommand { get; set; } = string.Empty;

    /// <summary> Immediately move tot he next layer. </summary>
    public void Immediate()
    {
        if ( !string.IsNullOrWhiteSpace( this.ImmediateCommand ) )
            _ = this.Session.WriteLine( this.ImmediateCommand );
    }

    #endregion

    #region " layer number "

    /// <summary> Gets or sets the arm layer number. </summary>
    /// <value> The arm layer number. </value>
    public int LayerNumber { get; private set; }

    #endregion

    #region " arm count "

    /// <summary> Number of maximum arms. </summary>

    /// <summary> Gets or sets the cached Maximum Arm Count. </summary>
    /// <remarks>
    /// Specifies how many times an operation is performed in the specified layer of the Arm model.
    /// </remarks>
    /// <value> The Arm MaximumArmCount or none if not set or unknown. </value>
    public int MaximumArmCount
    {
        get;

        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Returns <c>true</c> if using an infinite Arm count. </summary>
    /// <value> The number of is infinite Arms. </value>
    public bool? IsArmCountInfinite => this.ArmCount.HasValue ? this.ArmCount >= int.MaxValue : new bool?();

    /// <summary> Gets or sets the cached Arm Count. </summary>
    /// <remarks>
    /// Specifies how many times an operation is performed in the specified layer of the Arm model.
    /// </remarks>
    /// <value> The Arm ArmCount or none if not set or unknown. </value>
    public int? ArmCount
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.ArmCount, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.IsArmCountInfinite ) );
            }
        }
    }

    /// <summary> Writes and reads back the Arm ArmCount. </summary>
    /// <param name="value"> The current ArmCount. </param>
    /// <returns> The ArmCount or none if unknown. </returns>
    public int? ApplyArmCount( int value )
    {
        _ = this.WriteArmCount( value );
        return this.QueryArmCount();
    }

    /// <summary> Gets or sets Arm ArmCount query command. </summary>
    /// <remarks> SCPI: ":ARM:COUN?". </remarks>
    /// <value> The Arm ArmCount query command. </value>
    protected virtual string ArmCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current PointsArmCount. </summary>
    /// <returns> The PointsArmCount or none if unknown. </returns>
    public int? QueryArmCount()
    {
        if ( !string.IsNullOrWhiteSpace( this.ArmCountQueryCommand ) )
        {
            this.ArmCount = this.Session.Query( 0, this.ArmCountQueryCommand );
        }

        return this.ArmCount;
    }

    /// <summary> Gets or sets Arm ArmCount command format. </summary>
    /// <remarks> SCPI: ":ARM:COUN {0}". </remarks>
    /// <value> The Arm ArmCount command format. </value>
    protected virtual string ArmCountCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the arm count without reading back the value from the device. </summary>
    /// <param name="value"> The current arm count. </param>
    /// <returns> The arm count or none if unknown. </returns>
    public int? WriteArmCount( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.ArmCountCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.ArmCountCommandFormat, value );
        }

        this.ArmCount = value;
        return this.ArmCount;
    }

    #endregion

    #region " delay "

    /// <summary> The maximum delay. </summary>
    private TimeSpan _maximumDelay;

    /// <summary> Gets or sets the maximum delay. </summary>
    /// <value> The maximum delay. </value>
    public TimeSpan MaximumDelay
    {
        get => this._maximumDelay;

        protected set => _ = this.SetProperty( ref this._maximumDelay, value );
    }

    /// <summary> The delay. </summary>
    private TimeSpan? _delay;

    /// <summary> Gets or sets the cached Arm Delay. </summary>
    /// <remarks>
    /// The delay is used to delay operation in the Arm layer. After the programmed Arm event occurs,
    /// the instrument waits until the delay period expires before performing the Device Action.
    /// </remarks>
    /// <value> The Arm Delay or none if not set or unknown. </value>
    public TimeSpan? Delay
    {
        get => this._delay;

        protected set => _ = this.SetProperty( ref this._delay, value );
    }

    /// <summary> Writes and reads back the Arm Delay. </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The Arm Delay or none if unknown. </returns>
    public TimeSpan? ApplyDelay( TimeSpan value )
    {
        _ = this.WriteDelay( value );
        return this.QueryDelay();
    }

    /// <summary> Gets or sets the delay query command. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:DEL?". </remarks>
    /// <value> The delay query command. </value>
    protected virtual string DelayQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets or sets the Delay format for converting the query to time span. </summary>
    /// <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    /// <value> The Delay query command. </value>
    protected virtual string DelayFormat { get; set; } = string.Empty;

    /// <summary>   Queries the Delay. </summary>
    /// <remarks>   2024-07-05. </remarks>
    /// <returns>   The Delay or none if unknown. </returns>
    public TimeSpan? QueryDelay()
    {
        this.Delay = this.Session.Query( this.Delay, this.DelayFormat, this.DelayQueryCommand );
        return this.Delay;
    }

    /// <summary> Gets or sets the delay command format. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:DEL {0:s\.FFFFFFF}". </remarks>
    /// <value> The delay command format. </value>
    protected virtual string DelayCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Arm Delay without reading back the value from the device. </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The Arm Delay or none if unknown. </returns>
    public TimeSpan? WriteDelay( TimeSpan value )
    {
        _ = this.Session.WriteLine( value, this.DelayCommandFormat );
        this.Delay = value;
        return this.Delay;
    }

    #endregion

    #region " input line number "

    /// <summary> The input line number. </summary>

    /// <summary> Gets or sets the cached Arm Input Line Number. </summary>
    /// <value> The Arm InputLineNumber or none if not set or unknown. </value>
    public int? InputLineNumber
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Writes and reads back the Input Line Number. </summary>
    /// <param name="value"> The current Input Line Number. </param>
    /// <returns> The Input Line Number or none if unknown. </returns>
    public int? ApplyInputLineNumber( int value )
    {
        _ = this.WriteInputLineNumber( value );
        return this.QueryInputLineNumber();
    }

    /// <summary> Gets or sets the Input Line Number query command. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:ILIN?". </remarks>
    /// <value> The Input Line Number query command. </value>
    protected virtual string InputLineNumberQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the InputLineNumber. </summary>
    /// <returns> The Input Line Number or none if unknown. </returns>
    public int? QueryInputLineNumber()
    {
        this.InputLineNumber = this.Session.Query( this.InputLineNumber, this.InputLineNumberQueryCommand );
        return this.InputLineNumber;
    }

    /// <summary> Gets or sets the Input Line Number command format. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:ILIN {1}". </remarks>
    /// <value> The Input Line Number command format. </value>
    protected virtual string InputLineNumberCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Arm Input Line Number without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current InputLineNumber. </param>
    /// <returns> The Arm Input Line Number or none if unknown. </returns>
    public int? WriteInputLineNumber( int value )
    {
        this.InputLineNumber = this.Session.WriteLine( value, this.InputLineNumberCommandFormat );
        return this.InputLineNumber;
    }

    #endregion

    #region " output line number "

    /// <summary> The output line number. </summary>

    /// <summary> Gets or sets the cached Output Line Number. </summary>
    /// <value> The Arm OutputLineNumber or none if not set or unknown. </value>
    public int? OutputLineNumber
    {
        get;

        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Writes and reads back the Arm Output Line Number. </summary>
    /// <param name="value"> The current Output Line Number. </param>
    /// <returns> The Output Line Number or none if unknown. </returns>
    public int? ApplyOutputLineNumber( int value )
    {
        _ = this.WriteOutputLineNumber( value );
        return this.QueryOutputLineNumber();
    }

    /// <summary> Gets or sets the Output Line Number query command. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:OLIN?". </remarks>
    /// <value> The Output Line Number query command. </value>
    protected virtual string OutputLineNumberQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the OutputLineNumber. </summary>
    /// <returns> The Output Line Number or none if unknown. </returns>
    public int? QueryOutputLineNumber()
    {
        this.OutputLineNumber = this.Session.Query( this.OutputLineNumber, this.OutputLineNumberQueryCommand );
        return this.OutputLineNumber;
    }

    /// <summary> Gets or sets the Output Line Number command format. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:OLIN {0}". </remarks>
    /// <value> The Output Line Number command format. </value>
    protected virtual string OutputLineNumberCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Arm Output Line Number without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current OutputLineNumber. </param>
    /// <returns> The Arm Output Line Number or none if unknown. </returns>
    public int? WriteOutputLineNumber( int value )
    {
        this.OutputLineNumber = this.Session.WriteLine( value, this.OutputLineNumberCommandFormat );
        return this.OutputLineNumber;
    }

    #endregion

    #region " timer time span "

    /// <summary> The timer interval. </summary>
    private TimeSpan? _timerInterval;

    /// <summary> Gets or sets the cached Arm Timer Interval. </summary>
    /// <remarks>
    /// The Timer Interval is used to Timer Interval operation in the Arm layer. After the programmed
    /// Arm event occurs, the instrument waits until the Timer Interval period expires before
    /// performing the Device Action.
    /// </remarks>
    /// <value> The Arm Timer Interval or none if not set or unknown. </value>
    public TimeSpan? TimerInterval
    {
        get => this._timerInterval;

        protected set => _ = this.SetProperty( ref this._timerInterval, value );
    }

    /// <summary> Writes and reads back the Arm Timer Interval. </summary>
    /// <param name="value"> The current TimerTimeSpan. </param>
    /// <returns> The Arm Timer Interval or none if unknown. </returns>
    public TimeSpan? ApplyTimerTimeSpan( TimeSpan value )
    {
        _ = this.WriteTimerTimeSpan( value );
        return this.QueryTimerTimeSpan();
    }

    /// <summary> Gets or sets the Timer Interval query command. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:TIM?". </remarks>
    /// <value> The Timer Interval query command. </value>
    protected virtual string TimerIntervalQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Timer Interval format for converting the query to time span.
    /// </summary>
    /// <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    /// <value> The Timer Interval query command. </value>
    protected virtual string TimerIntervalFormat { get; set; } = string.Empty;

    /// <summary> Queries the Timer Interval. </summary>
    /// <returns> The Timer Interval or none if unknown. </returns>
    public TimeSpan? QueryTimerTimeSpan()
    {
        this.TimerInterval = this.Session.Query( this.TimerInterval, this.TimerIntervalFormat, this.TimerIntervalQueryCommand );
        return this.TimerInterval;
    }

    /// <summary> Gets or sets the Timer Interval command format. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:TIM {0:s\.FFFFFFF}". </remarks>
    /// <value> The query command format. </value>
    protected virtual string TimerIntervalCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Arm Timer Interval without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current TimerTimeSpan. </param>
    /// <returns> The Arm Timer Interval or none if unknown. </returns>
    public TimeSpan? WriteTimerTimeSpan( TimeSpan value )
    {
        _ = this.Session.WriteLine( value, this.TimerIntervalCommandFormat );
        this.TimerInterval = value;
        return this.TimerInterval;
    }

    #endregion
}
