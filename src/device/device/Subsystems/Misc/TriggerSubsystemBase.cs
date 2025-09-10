using cc.isr.Std.TimeSpanExtensions;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by a Trigger Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class TriggerSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    protected TriggerSubsystemBase( StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.TriggerSourceReadWrites = [];
        this.DefineTriggerSourceReadWrites();
        this.TriggerLayerBypassModeReadWrites = [];
        this.DefineTriggerLayerBypassReadWrites();
        this.TriggerEventReadWrites = [];
        this.DefineTriggerEventReadWrites();
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
        this.AutoDelayEnabled = false;
        this.TriggerCount = 1;
        this.Delay = TimeSpan.Zero;
        this.InputLineNumber = 1;
        this.OutputLineNumber = 2;
        this.TimerInterval = TimeSpan.FromSeconds( 0.1d );
        this.ContinuousEnabled = false;
        this.TriggerState = VI.TriggerState.None;
        this.MaximumTriggerCount = 99999;
        this.MaximumDelay = TimeSpan.FromSeconds( 999999.999d );
        this.ResetScpiKnownState();
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    private void ResetScpiKnownState()
    {
        this.AutoDelayEnabled = false;
        this.TriggerCount = 1;
        this.Delay = TimeSpan.Zero;
        this.TriggerLayerBypassMode = TriggerLayerBypassModes.Acceptor;
        this.InputLineNumber = 1;
        this.OutputLineNumber = 2;
        this.TriggerSource = TriggerSources.Immediate;
        this.TimerInterval = TimeSpan.FromSeconds( 0.1d );
        this.SupportedTriggerSources = TriggerSources.Bus | TriggerSources.External | TriggerSources.Immediate;
        this.ContinuousEnabled = false;
        this.TriggerState = VI.TriggerState.None;
    }

    #endregion

    #region " commands "

    /// <summary> Gets or sets the Abort command. </summary>
    /// <remarks> SCPI: ":ABOR". </remarks>
    /// <value> The Abort command. </value>
    protected virtual string AbortCommand { get; set; } = string.Empty;

    /// <summary> Aborts operations. </summary>
    /// <remarks>
    /// When this action command is sent, the SourceMeter aborts operation and returns to the idle
    /// state. A faster way to return to idle is to use the DCL or SDC command. With auto output- off
    /// enabled (:SOURce1:CLEar:AUTO ON), the output will remain on if operation is terminated before
    /// the output has a chance to automatically turn off.
    /// </remarks>
    public void Abort()
    {
        if ( !string.IsNullOrWhiteSpace( this.AbortCommand ) )
        {
            _ = this.Session.WriteLine( this.AbortCommand );
        }
    }

    /// <summary> Gets or sets the clear command. </summary>
    /// <remarks> SCPI: ":TRIG:CLE". </remarks>
    /// <value> The clear command. </value>
    protected virtual string ClearCommand { get; set; } = string.Empty;

    /// <summary> Clears the triggers. </summary>
    public void ClearTriggers()
    {
        _ = this.Session.WriteLine( this.ClearCommand );
    }

    /// <summary> Gets or sets the clear  trigger model command. </summary>
    /// <remarks> SCPI: ":TRIG:LOAD 'EMPTY'". </remarks>
    /// <value> The clear command. </value>
    protected virtual string ClearTriggerModelCommand { get; set; } = string.Empty;

    /// <summary> Clears the trigger model. </summary>
    public void ClearTriggerModel()
    {
        _ = this.Session.WriteLine( this.ClearTriggerModelCommand );
    }

    /// <summary> Gets or sets the initiate command. </summary>
    /// <remarks> SCPI: ":INIT". </remarks>
    /// <value> The initiate command. </value>
    protected virtual string InitiateCommand { get; set; } = string.Empty;

    /// <summary> Initiates operations. </summary>
    /// <remarks>
    /// This command is used to initiate source-measure operation by taking the SourceMeter out of
    /// idle. The :READ? and :MEASure? commands also perform an initiation. Note that if auto output-
    /// off is disabled (SOURce1:CLEar:AUTO OFF), the source output must first be turned on before an
    /// initiation can be performed. The :MEASure? command automatically turns the output source on
    /// before performing the initiation.
    /// </remarks>
    public void Initiate()
    {
        _ = this.Session.WriteLine( this.InitiateCommand );
    }

    /// <summary> Gets or sets the Immediate command. </summary>
    /// <remarks> SCPI: ":TRIG:IMM". </remarks>
    /// <value> The Immediate command. </value>
    protected virtual string ImmediateCommand { get; set; } = string.Empty;

    /// <summary> Immediately move tot he next layer. </summary>
    public void Immediate()
    {
        if ( !string.IsNullOrWhiteSpace( this.ImmediateCommand ) )
            _ = this.Session.WriteLine( this.ImmediateCommand );
    }

    #endregion

    #region " auto delay enabled "

    /// <summary> The automatic delay enabled. </summary>

    /// <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Delay Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoDelayEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.AutoDelayEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Auto Delay Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoDelayEnabled( bool value )
    {
        _ = this.WriteAutoDelayEnabled( value );
        return this.QueryAutoDelayEnabled();
    }

    /// <summary> Gets or sets the automatic delay enabled query command. </summary>
    /// <remarks> SCPI: ":TRIG:DEL:AUTO?". </remarks>
    /// <value> The automatic delay enabled query command. </value>
    protected virtual string AutoDelayEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Delay Enabled sentinel. Also sets the
    /// <see cref="AutoDelayEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoDelayEnabled()
    {
        this.AutoDelayEnabled = this.Session.Query( this.AutoDelayEnabled, this.AutoDelayEnabledQueryCommand );
        return this.AutoDelayEnabled;
    }

    /// <summary> Gets or sets the automatic delay enabled command Format. </summary>
    /// <remarks> SCPI: ":TRIG:DEL:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic delay enabled query command. </value>
    protected virtual string AutoDelayEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Delay Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoDelayEnabled( bool value )
    {
        this.AutoDelayEnabled = this.Session.WriteLine( value, this.AutoDelayEnabledCommandFormat );
        return this.AutoDelayEnabled;
    }

    #endregion

    #region " averaging enabled "

    /// <summary> The averaging enabled. </summary>

    /// <summary> Gets or sets the cached Averaging Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Averaging Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AveragingEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.AveragingEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Averaging Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAveragingEnabled( bool value )
    {
        _ = this.WriteAveragingEnabled( value );
        return this.QueryAveragingEnabled();
    }

    /// <summary> Gets or sets the automatic delay enabled query command. </summary>
    /// <remarks> SCPI: ":TRIG:AVER?". </remarks>
    /// <value> The automatic delay enabled query command. </value>
    protected virtual string AveragingEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Averaging Enabled sentinel. Also sets the
    /// <see cref="AveragingEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAveragingEnabled()
    {
        this.AveragingEnabled = this.Session.Query( this.AveragingEnabled, this.AveragingEnabledQueryCommand );
        return this.AveragingEnabled;
    }

    /// <summary> Gets or sets the automatic delay enabled command Format. </summary>
    /// <remarks> SCPI: ":TRIG:AVER {0:1;1;0}". </remarks>
    /// <value> The automatic delay enabled query command. </value>
    protected virtual string AveragingEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Averaging Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAveragingEnabled( bool value )
    {
        this.AveragingEnabled = this.Session.WriteLine( value, this.AveragingEnabledCommandFormat );
        return this.AveragingEnabled;
    }

    #endregion

    #region " continuous enabled "

    /// <summary> The continuous enabled. </summary>

    /// <summary> Gets or sets the cached Continuous Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Continuous Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? ContinuousEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.ContinuousEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Continuous Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyContinuousEnabled( bool value )
    {
        _ = this.WriteContinuousEnabled( value );
        return this.QueryContinuousEnabled();
    }

    /// <summary> Gets or sets the Continuous enabled query command. </summary>
    /// <remarks> SCPI: ":INIT:CONT?". </remarks>
    /// <value> The Continuous enabled query command. </value>
    protected virtual string ContinuousEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Continuous Enabled sentinel. Also sets the
    /// <see cref="ContinuousEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryContinuousEnabled()
    {
        this.ContinuousEnabled = this.Session.Query( this.ContinuousEnabled, this.ContinuousEnabledQueryCommand );
        return this.ContinuousEnabled;
    }

    /// <summary> Gets or sets the Continuous enabled command Format. </summary>
    /// <remarks> SCPI: ":INIT:CONT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Continuous enabled query command. </value>
    protected virtual string ContinuousEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Continuous Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteContinuousEnabled( bool value )
    {
        this.ContinuousEnabled = this.Session.WriteLine( value, this.ContinuousEnabledCommandFormat );
        return this.ContinuousEnabled;
    }

    #endregion

    #region " trigger count "

    /// <summary> Number of maximum triggers. </summary>

    /// <summary> Gets or sets the cached Maximum Trigger Count. </summary>
    /// <remarks>
    /// Specifies how many times an operation is performed in the specified layer of the Trigger
    /// model.
    /// </remarks>
    /// <value> The Trigger MaximumTriggerCount or none if not set or unknown. </value>
    public int MaximumTriggerCount
    {
        get;

        protected set
        {
            if ( !Equals( this.MaximumTriggerCount, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Returns <c>true</c> if using an infinite trigger count. </summary>
    /// <value> The number of is infinite triggers. </value>
    public bool? IsTriggerCountInfinite => this.TriggerCount.HasValue ? this.TriggerCount >= int.MaxValue : new bool?();

    /// <summary> Gets or sets the cached Trigger Count. </summary>
    /// <remarks>
    /// Specifies how many times an operation is performed in the specified layer of the trigger
    /// model.
    /// </remarks>
    /// <value> The trigger count or none if not set or unknown. </value>
    public int? TriggerCount
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.TriggerCount, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.IsTriggerCountInfinite ) );
            }
        }
    }

    /// <summary> Writes and reads back the Trigger Count. </summary>
    /// <param name="value"> The current Trigger Count. </param>
    /// <returns> The TriggerCount or none if unknown. </returns>
    public int? ApplyTriggerCount( int value )
    {
        _ = this.WriteTriggerCount( value );
        return this.QueryTriggerCount();
    }

    /// <summary> Gets or sets trigger count query command. </summary>
    /// <remarks> SCPI: ":TRIG:COUN?". </remarks>
    /// <value> The trigger count query command. </value>
    protected virtual string TriggerCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Trigger Count. </summary>
    /// <returns> The Trigger Count or none if unknown. </returns>
    public int? QueryTriggerCount()
    {
        if ( !string.IsNullOrWhiteSpace( this.TriggerCountQueryCommand ) )
        {
            double candidateTriggerCount = this.Session.Query( 0d, this.TriggerCountQueryCommand );
            this.TriggerCount = candidateTriggerCount < int.MaxValue ? ( int ) candidateTriggerCount : int.MaxValue;
        }

        return this.TriggerCount;
    }

    /// <summary> Gets or sets trigger count command format. </summary>
    /// <remarks> SCPI: ":TRIG:COUN {0}". </remarks>
    /// <value> The trigger count command format. </value>
    protected virtual string TriggerCountCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Trigger Count without reading back the value from the device. </summary>
    /// <param name="value"> The current PointsTriggerCount. </param>
    /// <returns> The Trigger Count or none if unknown. </returns>
    public int? WriteTriggerCount( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.TriggerCountCommandFormat ) )
        {
            _ = value == int.MaxValue
                ? this.Session.WriteLine( this.TriggerCountCommandFormat, "INF" )
                : this.Session.WriteLine( this.TriggerCountCommandFormat, value );
        }

        this.TriggerCount = value;
        return this.TriggerCount;
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

        protected set
        {
            if ( !Equals( this.MaximumDelay, value ) )
            {
                this._maximumDelay = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The delay. </summary>
    private TimeSpan? _delay;

    /// <summary> Gets or sets the cached Trigger Delay. </summary>
    /// <remarks>
    /// The delay is used to delay operation in the trigger layer. After the programmed trigger event
    /// occurs, the instrument waits until the delay period expires before performing the Device
    /// Action.
    /// </remarks>
    /// <value> The Trigger Delay or none if not set or unknown. </value>
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

    /// <summary> Writes and reads back the Trigger Delay. </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The Trigger Delay or none if unknown. </returns>
    public TimeSpan? ApplyDelay( TimeSpan value )
    {
        _ = this.WriteDelay( value );
        return this.QueryDelay();
    }

    /// <summary> Gets or sets the delay query command. </summary>
    /// <remarks> SCPI: ":TRIG:DEL?". </remarks>
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
    /// <remarks> SCPI: ":TRIG:DEL {0:s\.FFFFFFF}". </remarks>
    /// <value> The delay command format. </value>
    protected virtual string DelayCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Trigger Delay without reading back the value from the device. </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The Trigger Delay or none if unknown. </returns>
    public TimeSpan? WriteDelay( TimeSpan value )
    {
        _ = this.Session.WriteLine( value, this.DelayCommandFormat );
        this.Delay = value;
        return this.Delay;
    }

    #endregion

    #region " input line number "

    /// <summary> The input line number. </summary>

    /// <summary> Gets or sets the cached Trigger Input Line Number. </summary>
    /// <value> The Trigger InputLineNumber or none if not set or unknown. </value>
    public int? InputLineNumber
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.InputLineNumber, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Trigger Input Line Number. </summary>
    /// <param name="value"> The current Input Line Number. </param>
    /// <returns> The Input Line Number or none if unknown. </returns>
    public int? ApplyInputLineNumber( int value )
    {
        _ = this.WriteInputLineNumber( value );
        return this.QueryInputLineNumber();
    }

    /// <summary> Gets or sets the Input Line Number query command. </summary>
    /// <remarks>
    /// SCPI: :TRIG:ILIN?
    /// 2002: :TRIG:TCON:ASYN:ILIN?
    /// </remarks>
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
    /// <remarks>
    /// SCPI: :TRIG:ILIN {0}
    /// 2002: :TRIG:TCON:ASYN:ILIN {0}
    /// </remarks>
    /// <value> The Input Line Number command format. </value>
    protected virtual string InputLineNumberCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Trigger Input Line Number without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current InputLineNumber. </param>
    /// <returns> The Trigger Input Line Number or none if unknown. </returns>
    public int? WriteInputLineNumber( int value )
    {
        this.InputLineNumber = this.Session.WriteLine( value, this.InputLineNumberCommandFormat );
        return this.InputLineNumber;
    }

    #endregion

    #region " output line number "

    /// <summary> The output line number. </summary>

    /// <summary> Gets or sets the cached Trigger Output Line Number. </summary>
    /// <value> The Trigger OutputLineNumber or none if not set or unknown. </value>
    public int? OutputLineNumber
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.OutputLineNumber, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Trigger Output Line Number. </summary>
    /// <param name="value"> The current Output Line Number. </param>
    /// <returns> The Output Line Number or none if unknown. </returns>
    public int? ApplyOutputLineNumber( int value )
    {
        _ = this.WriteOutputLineNumber( value );
        return this.QueryOutputLineNumber();
    }

    /// <summary> Gets or sets the Output Line Number query command. </summary>
    /// <remarks>
    /// SCPI: :TRIG:OLIN?
    /// 2002: :TRIG:TCON:ASYN:OLIN?
    /// </remarks>
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
    /// <remarks>
    /// SCPI: :TRIG:OLIN {0}
    /// 2002: :TRIG:TCON:ASYN:OLIN {0}
    /// </remarks>
    /// <value> The Output Line Number command format. </value>
    protected virtual string OutputLineNumberCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Trigger Output Line Number without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current OutputLineNumber. </param>
    /// <returns> The Trigger Output Line Number or none if unknown. </returns>
    public int? WriteOutputLineNumber( int value )
    {
        this.OutputLineNumber = this.Session.WriteLine( value, this.OutputLineNumberCommandFormat );
        return this.OutputLineNumber;
    }

    #endregion

    #region " timer time span "

    /// <summary> The timer interval. </summary>
    private TimeSpan? _timerInterval;

    /// <summary> Gets or sets the cached Trigger Timer Interval. </summary>
    /// <remarks>
    /// The Timer Interval is used to Timer Interval operation in the trigger layer. After the
    /// programmed trigger event occurs, the instrument waits until the Timer Interval period expires
    /// before performing the Device Action.
    /// </remarks>
    /// <value> The Trigger Timer Interval or none if not set or unknown. </value>
    public TimeSpan? TimerInterval
    {
        get => this._timerInterval;

        protected set
        {
            if ( !this.TimerInterval.Equals( value ) )
            {
                this._timerInterval = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Trigger Timer Interval. </summary>
    /// <param name="value"> The current TimerTimeSpan. </param>
    /// <returns> The Trigger Timer Interval or none if unknown. </returns>
    public TimeSpan? ApplyTimerTimeSpan( TimeSpan value )
    {
        _ = this.WriteTimerTimeSpan( value );
        return this.QueryTimerTimeSpan();
    }

    /// <summary> Gets the Timer Interval query command. </summary>
    /// <remarks> SCPI: ":TRIG:TIM?". </remarks>
    /// <value> The Timer Interval query command. </value>
    protected virtual string TimerIntervalQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets the Timer Interval format for converting the query to time span. </summary>
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

    /// <summary> Gets the Timer Interval command format. </summary>
    /// <remarks> SCPI: ":TRIG:TIM {0:s\.FFFFFFF}". </remarks>
    /// <value> The query command format. </value>
    protected virtual string TimerIntervalCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Trigger Timer Interval without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current TimerTimeSpan. </param>
    /// <returns> The Trigger Timer Interval or none if unknown. </returns>
    public TimeSpan? WriteTimerTimeSpan( TimeSpan value )
    {
        _ = this.Session.WriteLine( value, this.TimerIntervalCommandFormat );
        this.TimerInterval = value;
        return default;
    }

    #endregion

    #region " trigger state "

    /// <summary> Monitor active trigger state. </summary>
    /// <param name="pollPeriod"> The poll period. </param>
    public void MonitorActiveTriggerState( TimeSpan pollPeriod )
    {
        _ = this.QueryTriggerState();
        while ( this.IsTriggerStateActive() )
        {
            _ = pollPeriod.AsyncWait();
            Pith.SessionBase.DoEventsAction?.Invoke();
            _ = this.QueryTriggerState();
        }
    }

    /// <summary> Gets the asynchronous task. </summary>
    /// <value> The asynchronous task. </value>
    public System.Threading.Tasks.Task? AsyncTask { get; private set; }

    /// <summary> Gets the action task. </summary>
    /// <value> The action task. </value>
    public System.Threading.Tasks.Task? ActionTask { get; private set; }

    /// <summary> Asynchronous monitor trigger state. </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    /// <param name="pollPeriod"> The poll period. </param>
    /// <returns> A Threading.Tasks.Task. </returns>
    public System.Threading.Tasks.Task AsyncMonitorTriggerState( TimeSpan pollPeriod )
    {
        this.AsyncTask = this.AsyncAwaitMonitorTriggerState( pollPeriod );
        return this.AsyncTask;
    }

    /// <summary> Asynchronous await monitor trigger state. </summary>
    /// <param name="pollPeriod"> The poll period. </param>
    /// <returns> A Threading.Tasks.Task. </returns>
    private async System.Threading.Tasks.Task AsyncAwaitMonitorTriggerState( TimeSpan pollPeriod )
    {
        this.ActionTask = System.Threading.Tasks.Task.Run( () => this.MonitorActiveTriggerState( pollPeriod ) );
        await this.ActionTask;
    }

    /// <summary> Gets the trigger state caption. </summary>
    /// <value> The trigger state caption. </value>
    public string TriggerStateCaption => this.TriggerState.HasValue ? this.TriggerState.ToString() : string.Empty;

    /// <summary> Gets or sets the cached State TriggerState. </summary>
    /// <value>
    /// The <see cref="TriggerState">State Trigger State</see> or none if not set or unknown.
    /// </value>
    public TriggerState? TriggerState
    {
        get;

        protected set
        {
            if ( !this.TriggerState.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.TriggerStateCaption ) );
            }
        }
    }

    /// <summary> Gets or sets the cached trigger block <see cref="TriggerState"/>. </summary>
    /// <value>
    /// The <see cref="TriggerBlockState">State Trigger State</see> or none if not set or unknown.
    /// </value>
    public TriggerState? TriggerBlockState
    {
        get;

        protected set
        {
            if ( !this.TriggerBlockState.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached trigger state block number. </summary>
    /// <value> The block number of the trigger state. </value>
    public int? TriggerStateBlockNumber
    {
        get;

        protected set
        {
            if ( !this.TriggerStateBlockNumber.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> QueryEnum if this object is trigger state done. </summary>
    /// <returns> <c>true</c> if trigger state done; otherwise <c>false</c> </returns>
    public bool IsTriggerStateDone()
    {
        bool result;
        if ( this.TriggerState.HasValue )
        {
            TriggerState value = this.TriggerState.Value;
            result = value is VI.TriggerState.Aborted or VI.TriggerState.Failed or VI.TriggerState.Empty or VI.TriggerState.Idle or VI.TriggerState.None;
        }
        else
        {
            return false;
        }

        return result;
    }

    /// <summary> Queries if a trigger state is active. </summary>
    /// <returns> <c>true</c> if a trigger state is active; otherwise <c>false</c> </returns>
    public bool IsTriggerStateActive()
    {
        bool result;
        if ( this.TriggerState.HasValue )
        {
            TriggerState value = this.TriggerState.Value;
            result = value is VI.TriggerState.Running or VI.TriggerState.Waiting;
        }
        else
        {
            return false;
        }

        return result;
    }

    /// <summary> Queries if a trigger state is aborting. </summary>
    /// <returns> <c>true</c> if a trigger state is active; otherwise <c>false</c> </returns>
    public bool IsTriggerStateAborting()
    {
        bool result;
        if ( this.TriggerState.HasValue )
        {
            TriggerState value = this.TriggerState.Value;
            result = value == VI.TriggerState.Aborting;
        }
        else
        {
            return false;
        }

        return result;
    }

    /// <summary> Queries if a trigger state is Failed. </summary>
    /// <returns> <c>true</c> if a trigger state is active; otherwise <c>false</c> </returns>
    public bool IsTriggerStateFailed()
    {
        bool result;
        if ( this.TriggerState.HasValue )
        {
            TriggerState value = this.TriggerState.Value;
            result = value == VI.TriggerState.Failed;
        }
        else
        {
            return false;
        }

        return result;
    }

    /// <summary> Queries if a trigger state is Idle. </summary>
    /// <returns> <c>true</c> if a trigger state is active; otherwise <c>false</c> </returns>
    public bool IsTriggerStateIdle()
    {
        bool result;
        if ( this.TriggerState.HasValue )
        {
            TriggerState value = this.TriggerState.Value;
            result = value == VI.TriggerState.Idle;
        }
        else
        {
            return false;
        }

        return result;
    }

    /// <summary>   Clears the trigger state, setting it to <see cref="VI.TriggerState.None"/> </summary>
    /// <remarks>   David, 2021-07-10. </remarks>
    public void ClearTriggerState()
    {
        this.TriggerState = VI.TriggerState.None;
    }

    /// <summary>   Queries if the trigger plan is inactive. </summary>
    /// <remarks>   David, 2021-07-09. </remarks>
    /// <returns>   True if the trigger plan is inactive, false if not. </returns>
    public bool IsTriggerPlanInactive()
    {
        return !this.TriggerState.HasValue || this.TriggerState.Value == VI.TriggerState.None;
    }

    /// <summary> Gets the Trigger State query command. </summary>
    /// <remarks>
    /// <c>SCPI: :TRIG:STAT? <para>
    /// TSP2: trigger.model.state()
    /// </para></c>
    /// </remarks>
    /// <value> The Trigger State query command. </value>
    protected virtual string TriggerStateQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Trigger State. </summary>
    /// <returns> The <see cref="TriggerState">Trigger State</see> or none if unknown. </returns>
    public virtual TriggerState? QueryTriggerState()
    {
        string currentValue = this.TriggerState.ToString();
        if ( string.IsNullOrWhiteSpace( this.Session.EmulatedReply ) )
            this.Session.EmulatedReply = currentValue;
        if ( !string.IsNullOrWhiteSpace( this.TriggerStateQueryCommand ) )
        {
            currentValue = this.Session.QueryTrimEnd( this.TriggerStateQueryCommand );
        }

        if ( string.IsNullOrWhiteSpace( currentValue ) )
        {
            this.TriggerState = new TriggerState?();
        }
        else
        {
            Queue<string> values = new( currentValue.Split( ';' ) );
            if ( values.Any() )
            {
                this.TriggerState = ( TriggerState ) ( int ) Enum.Parse( typeof( TriggerState ), values.Dequeue(), true );
            }

            if ( values.Any() )
            {
                this.TriggerBlockState = ( TriggerState ) ( int ) Enum.Parse( typeof( TriggerState ), values.Dequeue(), true );
            }

            if ( values.Any() )
            {
                this.TriggerStateBlockNumber = int.Parse( values.Dequeue() );
            }
        }

        return this.TriggerState;
    }

    /// <summary> Gets the state of the supports trigger. </summary>
    /// <value> The supports trigger state. </value>
    public bool SupportsTriggerState => !string.IsNullOrWhiteSpace( this.TriggerStateQueryCommand );

    #endregion
}
/// <summary> Values that represent trigger status. </summary>
public enum TriggerState
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Idle (trigger.STATE_NONE)" )]
    None,

    /// <summary> An enum constant representing the idle option. </summary>
    [System.ComponentModel.Description( "Idle (trigger.STATE_IDLE)" )]
    Idle,

    /// <summary> An enum constant representing the running option. </summary>
    [System.ComponentModel.Description( "Running (trigger.STATE_RUNNING)" )]
    Running,

    /// <summary> An enum constant representing the waiting option. </summary>
    [System.ComponentModel.Description( "Waiting (trigger.STATE_WAITING)" )]
    Waiting,

    /// <summary> An enum constant representing the empty option. </summary>
    [System.ComponentModel.Description( "Empty (trigger.STATE_EMPTY)" )]
    Empty,

    /// <summary> An enum constant representing the building option. </summary>
    [System.ComponentModel.Description( "Building (trigger.STATE_BUILDING)" )]
    Building,

    /// <summary> An enum constant representing the failed option. </summary>
    [System.ComponentModel.Description( "Failed (trigger.STATE_FAILED)" )]
    Failed,

    /// <summary> An enum constant representing the aborting option. </summary>
    [System.ComponentModel.Description( "Aborting (trigger.STATE_ABORTING)" )]
    Aborting,

    /// <summary> An enum constant representing the aborted option. </summary>
    [System.ComponentModel.Description( "Aborted (trigger.STATE_ABORTED)" )]
    Aborted
}
