namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a Source Function Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class SourceFunctionSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceFunctionSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected SourceFunctionSubsystemBase( StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionRange = Ranges.NonnegativeFullRange;
        this.DefaultFunctionModeDecimalPlaces = 3;
        this._amount = new cc.isr.UnitsAmounts.Amount( 0d, this.DefaultFunctionUnit );
        this.FunctionUnit = this.DefaultFunctionUnit;
        this._functionRange = this.DefaultFunctionRange;
        this._functionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
        this.FunctionModeDecimalPlaces = [];
        this.DefineFunctionModeDecimalPlaces();
        this.FunctionModeReadWrites = [];
        this.DefineFunctionModeReadWrites();
        this.FunctionModeRanges = [];
        this.DefineFunctionModeRanges();
        this.FunctionModeUnits = [];
        this.DefineFunctionModeUnits();
        this._levelCaption = string.Empty;
        this._functionMode = SourceFunctionModes.None;
        this.FunctionCode = string.Empty;
    }

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the clear execution state (CLS) by setting system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    public override void DefineClearExecutionState()
    {
        base.DefineClearExecutionState();
        this.Level = new double?();
        this.DefineFunctionClearKnownState();
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.Amount = new cc.isr.UnitsAmounts.Amount( 0d, this.DefaultFunctionUnit );
        this.FunctionUnit = this.DefaultFunctionUnit;
        this._functionRange = this.DefaultFunctionRange;
        this.FunctionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
        this.AutoClearEnabled = false;
        this.AutoDelayEnabled = true;
        this.Delay = TimeSpan.Zero;
        this.SweepPoints = 2500;
    }

    #endregion

    #region " auto clear enabled "

    /// <summary> The automatic clear enabled. </summary>
    private bool? _autoClearEnabled;

    /// <summary> Gets or sets the cached Auto Clear Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Clear Enabled is not known; <c>true</c> if output is on; otherwise,
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

    /// <summary> Writes and reads back the Auto Clear Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoClearEnabled( bool value )
    {
        _ = this.WriteAutoClearEnabled( value );
        return this.QueryAutoClearEnabled();
    }

    /// <summary> Gets or sets the automatic Clear enabled query command. </summary>
    /// <remarks> SCPI: ":SOUR&lt;ch&gt;:CLE:AUTO?". </remarks>
    /// <value> The automatic Clear enabled query command. </value>
    protected virtual string AutoClearEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Clear Enabled sentinel. Also sets the
    /// <see cref="AutoClearEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoClearEnabled()
    {
        this.AutoClearEnabled = this.Session.Query( this.AutoClearEnabled, this.AutoClearEnabledQueryCommand );
        return this.AutoClearEnabled;
    }

    /// <summary> Gets or sets the automatic Clear enabled command Format. </summary>
    /// <remarks> SCPI: ":SOUR&lt;ch&gt;:CLE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic Clear enabled query command. </value>
    protected virtual string AutoClearEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Clear Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoClearEnabled( bool value )
    {
        this.AutoClearEnabled = this.Session.WriteLine( value, this.AutoClearEnabledCommandFormat );
        return this.AutoClearEnabled;
    }

    #endregion

    #region " auto delay enabled "

    /// <summary> The automatic delay enabled. </summary>
    private bool? _autoDelayEnabled;

    /// <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Delay Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoDelayEnabled
    {
        get => this._autoDelayEnabled;

        protected set
        {
            if ( !Equals( this.AutoDelayEnabled, value ) )
            {
                this._autoDelayEnabled = value;
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
    /// <remarks> SCPI: ":SOUR&lt;ch&gt;:DEL:AUTO?". </remarks>
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
    /// <remarks> SCPI: ":SOUR&lt;ch&gt;:DEL:AUTO {0:'ON';'ON';'OFF'}". </remarks>
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

    #region " auto range enabled "

    /// <summary> Auto Range enabled. </summary>
    private bool? _autoRangeEnabled;

    /// <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Range Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoRangeEnabled
    {
        get => this._autoRangeEnabled;

        protected set
        {
            if ( !Equals( this.AutoRangeEnabled, value ) )
            {
                this._autoRangeEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Auto Range Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoRangeEnabled( bool value )
    {
        _ = this.WriteAutoRangeEnabled( value );
        return this.QueryAutoRangeEnabled();
    }

    /// <summary> Gets or sets the automatic Range enabled query command. </summary>
    /// <remarks> SCPI: "SOUR:RANG:AUTO?". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string AutoRangeEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Range Enabled sentinel. Also sets the
    /// <see cref="AutoRangeEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoRangeEnabled()
    {
        this.AutoRangeEnabled = this.Session.Query( this.AutoRangeEnabled, this.AutoRangeEnabledQueryCommand );
        return this.AutoRangeEnabled;
    }

    /// <summary> Gets or sets the automatic Range enabled command Format. </summary>
    /// <remarks> SCPI: "SOUR:RANGE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string AutoRangeEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Range Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoRangeEnabled( bool value )
    {
        this.AutoRangeEnabled = this.Session.WriteLine( value, this.AutoRangeEnabledCommandFormat );
        return this.AutoRangeEnabled;
    }

    #endregion

    #region " delay "

    /// <summary> The delay. </summary>
    private TimeSpan? _delay;

    /// <summary> Gets or sets the cached Source Delay. </summary>
    /// <remarks>
    /// The delay is used to delay operation in the Source layer. After the programmed Source event
    /// occurs, the instrument waits until the delay period expires before performing the Device
    /// Action.
    /// </remarks>
    /// <value> The Source Delay or none if not set or unknown. </value>
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

    /// <summary> Writes and reads back the Source Delay. </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The Source Delay or none if unknown. </returns>
    public TimeSpan? ApplyDelay( TimeSpan value )
    {
        _ = this.WriteDelay( value );
        _ = this.QueryDelay();
        return default;
    }

    /// <summary> Gets or sets the delay query command. </summary>
    /// <remarks> SCPI: ":SOUR&lt;ch&gt;:DEL?". </remarks>
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
    /// <remarks> SCPI: ":SOUR&lt;ch&gt;:DEL {0:s\.FFFFFFF}". </remarks>
    /// <value> The delay command format. </value>
    protected virtual string DelayCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Source Delay without reading back the value from the device. </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The Source Delay or none if unknown. </returns>
    public TimeSpan? WriteDelay( TimeSpan value )
    {
        _ = this.Session.WriteLine( value, this.DelayCommandFormat );
        this.Delay = value;
        return this.Delay;
    }

    #endregion

    #region " level "

    /// <summary> The amount. </summary>
    private cc.isr.UnitsAmounts.Amount _amount;

    /// <summary> Gets or sets the amount. </summary>
    /// <value> The amount. </value>
    public cc.isr.UnitsAmounts.Amount Amount
    {
        get => this._amount;
        set
        {
            this._amount = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Creates a new amount. </summary>
    /// <param name="unit"> The unit. </param>
    private void NewAmount( cc.isr.UnitsAmounts.Unit unit )
    {
        if ( this.Level.HasValue )
        {
            this._amount = new cc.isr.UnitsAmounts.Amount( this.Level.Value, unit );
            this.LevelCaption = $"{this.Amount} {this.Amount.Unit}";
        }
        else
        {
            this._amount = new cc.isr.UnitsAmounts.Amount( 0d, unit );
            this.LevelCaption = $"-.---- {this.Amount.Unit}";
        }

        this.NotifyPropertyChanged( nameof( SourceSubsystemBase.Amount ) );
        this.NotifyPropertyChanged( nameof( SourceSubsystemBase.FunctionUnit ) );
    }

    /// <summary> The level caption. </summary>
    private string _levelCaption;

    /// <summary> Gets or sets the Level caption. </summary>
    /// <value> The Level caption. </value>
    public string LevelCaption
    {
        get => this._levelCaption;
        set
        {
            if ( !string.Equals( value, this.LevelCaption, StringComparison.OrdinalIgnoreCase ) )
            {
                this._levelCaption = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The level. </summary>
    private double? _level;

    /// <summary> Gets or sets the cached Source Current Level. </summary>
    /// <value> The Source Current Level. Actual current depends on the power supply mode. </value>
    public double? Level
    {
        get => this._level;

        protected set
        {
            if ( !Nullable.Equals( this.Level, value ) )
            {
                this._level = value;
                this.NewAmount( this.FunctionUnit );
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the source current level. </summary>
    /// <remarks>
    /// This command set the immediate output current level. The value is in Amperes. The immediate
    /// level is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <param name="value"> The current level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? ApplyLevel( double value )
    {
        _ = this.WriteLevel( value );
        return this.QueryLevel();
    }

    /// <summary> Gets or sets The Level query command. </summary>
    /// <value> The Level query command. </value>
    protected virtual string LevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current level. </summary>
    /// <returns> The current level or none if unknown. </returns>
    public double? QueryLevel()
    {
        this.Level = this.Session.Query( this.Level.GetValueOrDefault( 0d ), this.LevelQueryCommand );
        return this.Level;
    }

    /// <summary> Gets or sets The Level command format. </summary>
    /// <value> The Level command format. </value>
    protected virtual string LevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the source current level without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output current level. The value is in Amperes. The immediate
    /// level is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <param name="value"> The current level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? WriteLevel( double value )
    {
        _ = this.Session.WriteLine( this.LevelCommandFormat, value );
        this.Level = value;
        return this.Level;
    }

    #endregion

    #region " protection level "

    /// <summary> The protection level. </summary>
    private double? _protectionLevel;

    /// <summary> Gets or sets the cached Over Voltage Protection Level. </summary>
    /// <remarks>
    /// This command sets the over-voltage protection (OVP) level of the output. The values are
    /// programmed in volts. If the output voltage exceeds the OVP level, the output is disabled and
    /// OVP is set in the Questionable Condition status register. The*RST value = Max.
    /// </remarks>
    /// <value> <c>null</c> if value is not known. </value>
    public double? ProtectionLevel
    {
        get => this._protectionLevel;

        protected set
        {
            if ( !Nullable.Equals( this.ProtectionLevel, value ) )
            {
                this._protectionLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the protection level. </summary>
    /// <param name="value"> the protection level. </param>
    /// <returns> the protection level. </returns>
    public double? ApplyProtectionLevel( double value )
    {
        _ = this.WriteProtectionLevel( value );
        return this.QueryProtectionLevel();
    }

    /// <summary> Gets or sets the protection level query command. </summary>
    /// <value> the protection level query command. </value>
    protected virtual string ProtectionLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the protection level. </summary>
    /// <returns> the protection level or none if unknown. </returns>
    public double? QueryProtectionLevel()
    {
        this.ProtectionLevel = this.Session.Query( this.ProtectionLevel, this.ProtectionLevelQueryCommand );
        return this.ProtectionLevel;
    }

    /// <summary> Gets or sets the protection level command format. </summary>
    /// <value> the protection level command format. </value>
    protected virtual string ProtectionLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the protection level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the protection level. </remarks>
    /// <param name="value"> the protection level. </param>
    /// <returns> the protection level. </returns>
    public double? WriteProtectionLevel( double value )
    {
        this.ProtectionLevel = this.Session.WriteLine( value, this.ProtectionLevelCommandFormat );
        return this.ProtectionLevel;
    }

    #endregion

    #region " range "

    /// <summary> The range. </summary>
    private double? _range;

    /// <summary>
    /// Gets or sets the cached range. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Range
    {
        get => this._range;

        protected set
        {
            if ( !Nullable.Equals( this.Range, value ) )
            {
                this._range = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the range. </summary>
    /// <param name="value"> The range. </param>
    /// <returns> The range. </returns>
    public double? ApplyRange( double value )
    {
        _ = this.WriteRange( value );
        return this.QueryRange();
    }

    /// <summary> Gets or sets the range query command. </summary>
    /// <value> The range query command. </value>
    protected virtual string RangeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the range. </summary>
    /// <returns> The range or none if unknown. </returns>
    public double? QueryRange()
    {
        this.Range = this.Session.Query( this.Range, this.RangeQueryCommand );
        return this.Range;
    }

    /// <summary> Gets or sets the range command format. </summary>
    /// <value> The range command format. </value>
    protected virtual string RangeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the range without reading back the value from the device. </summary>
    /// <remarks> This command sets the range. </remarks>
    /// <param name="value"> The range. </param>
    /// <returns> The range. </returns>
    public double? WriteRange( double value )
    {
        this.Range = this.Session.WriteLine( value, this.RangeCommandFormat );
        return this.Range;
    }

    #endregion

    #region " sweep start level "

    /// <summary> The Sweep Start Level. </summary>
    private double? _sweepStartLevel;

    /// <summary>
    /// Gets or sets the cached Sweep Start Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? SweepStartLevel
    {
        get => this._sweepStartLevel;

        protected set
        {
            if ( !Nullable.Equals( this.SweepStartLevel, value ) )
            {
                this._sweepStartLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sweep Start Level. </summary>
    /// <param name="value"> The Sweep Start Level. </param>
    /// <returns> The Sweep Start Level. </returns>
    public double? ApplySweepStartLevel( double value )
    {
        _ = this.WriteSweepStartLevel( value );
        return this.QuerySweepStartLevel();
    }

    /// <summary> Gets or sets the Sweep Start Level query command. </summary>
    /// <value> The Sweep Start Level query command. </value>
    protected virtual string SweepStartLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Sweep Start Level. </summary>
    /// <returns> The Sweep Start Level or none if unknown. </returns>
    public double? QuerySweepStartLevel()
    {
        this.SweepStartLevel = this.Session.Query( this.SweepStartLevel, this.SweepStartLevelQueryCommand );
        return this.SweepStartLevel;
    }

    /// <summary> Gets or sets the Sweep Start Level command format. </summary>
    /// <value> The Sweep Start Level command format. </value>
    protected virtual string SweepStartLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Sweep Start Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Sweep Start Level. </remarks>
    /// <param name="value"> The Sweep Start Level. </param>
    /// <returns> The Sweep Start Level. </returns>
    public double? WriteSweepStartLevel( double value )
    {
        this.SweepStartLevel = this.Session.WriteLine( value, this.SweepStartLevelCommandFormat );
        return this.SweepStartLevel;
    }

    #endregion

    #region " sweep stop level "

    /// <summary> The Sweep Stop Level. </summary>
    private double? _sweepStopLevel;

    /// <summary>
    /// Gets or sets the cached Sweep Stop Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? SweepStopLevel
    {
        get => this._sweepStopLevel;

        protected set
        {
            if ( !Nullable.Equals( this.SweepStopLevel, value ) )
            {
                this._sweepStopLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sweep Stop Level. </summary>
    /// <param name="value"> The Sweep Stop Level. </param>
    /// <returns> The Sweep Stop Level. </returns>
    public double? ApplySweepStopLevel( double value )
    {
        _ = this.WriteSweepStopLevel( value );
        return this.QuerySweepStopLevel();
    }

    /// <summary> Gets or sets the Sweep Stop Level query command. </summary>
    /// <value> The Sweep Stop Level query command. </value>
    protected virtual string SweepStopLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Sweep Stop Level. </summary>
    /// <returns> The Sweep Stop Level or none if unknown. </returns>
    public double? QuerySweepStopLevel()
    {
        this.SweepStopLevel = this.Session.Query( this.SweepStopLevel, this.SweepStopLevelQueryCommand );
        return this.SweepStopLevel;
    }

    /// <summary> Gets or sets the Sweep Stop Level command format. </summary>
    /// <value> The Sweep Stop Level command format. </value>
    protected virtual string SweepStopLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Sweep Stop Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the Sweep Stop Level. </remarks>
    /// <param name="value"> The Sweep Stop Level. </param>
    /// <returns> The Sweep Stop Level. </returns>
    public double? WriteSweepStopLevel( double value )
    {
        this.SweepStopLevel = this.Session.WriteLine( value, this.SweepStopLevelCommandFormat );
        return this.SweepStopLevel;
    }

    #endregion

    #region " sweep mode  "

    /// <summary> The Sweep Mode. </summary>
    private SweepMode _sweepMode;

    /// <summary> Gets or sets the cached Sweep Mode. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public SweepMode SweepMode
    {
        get => this._sweepMode;

        protected set
        {
            if ( !Nullable.Equals( this.SweepMode, value ) )
            {
                this._sweepMode = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sweep Mode. </summary>
    /// <param name="value"> The Sweep Mode. </param>
    /// <returns> The Sweep Mode. </returns>
    public SweepMode ApplySweepMode( SweepMode value )
    {
        _ = this.WriteSweepMode( value );
        return this.QuerySweepMode();
    }

    /// <summary> Gets or sets the Sweep Mode  query command. </summary>
    /// <value> The Sweep Mode  query command. </value>
    protected virtual string SweepModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Sweep Mode. </summary>
    /// <returns> The Sweep Mode  or none if unknown. </returns>
    public SweepMode QuerySweepMode()
    {
        this.SweepMode = this.Session.QueryEnum( this.SweepMode, this.SweepModeQueryCommand );
        return this.SweepMode;
    }

    /// <summary> Gets or sets the Sweep Mode  command format. </summary>
    /// <value> The Sweep Mode  command format. </value>
    protected virtual string SweepModeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Sweep Mode  without reading back the value from the device. </summary>
    /// <remarks> This command sets the Sweep Mode. </remarks>
    /// <param name="value"> The Sweep Mode. </param>
    /// <returns> The Sweep Mode. </returns>
    public SweepMode WriteSweepMode( SweepMode value )
    {
        this.SweepMode = this.Session.WriteEnum( value, this.SweepModeCommandFormat );
        return this.SweepMode;
    }

    #endregion

    #region " sweep points "

    /// <summary> The sweep point. </summary>
    private int? _sweepPoint;

    /// <summary> Gets or sets the cached Sweep Points. </summary>
    /// <value> The Sweep Points or none if not set or unknown. </value>
    public int? SweepPoints
    {
        get => this._sweepPoint;

        protected set
        {
            if ( !Nullable.Equals( this.SweepPoints, value ) )
            {
                this._sweepPoint = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sweep Points. </summary>
    /// <param name="value"> The current SweepPoints. </param>
    /// <returns> The SweepPoints or none if unknown. </returns>
    public int? ApplySweepPoints( int value )
    {
        _ = this.WriteSweepPoints( value );
        return this.QuerySweepPoints();
    }

    /// <summary> Gets or sets Sweep Points query command. </summary>
    /// <remarks> SCPI: ":SOUR&lt;ch&gt;:SWE:POIN?". </remarks>
    /// <value> The Sweep Points query command. </value>
    protected virtual string SweepPointsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Sweep Points. </summary>
    /// <returns> The Sweep Points or none if unknown. </returns>
    public int? QuerySweepPoints()
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepPointsQueryCommand ) )
        {
            this.SweepPoints = this.Session.Query( 0, this.SweepPointsQueryCommand );
        }

        return this.SweepPoints;
    }

    /// <summary> Gets or sets Sweep Points command format. </summary>
    /// <remarks> SCPI: "SOUR&lt;ch&gt;:SWE:POIN {0}". </remarks>
    /// <value> The Sweep Points command format. </value>
    protected virtual string SweepPointsCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Sweep Points without reading back the value from the device. </summary>
    /// <param name="value"> The current Sweep Points. </param>
    /// <returns> The Sweep Points or none if unknown. </returns>
    public int? WriteSweepPoints( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepPointsCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.SweepPointsCommandFormat, value );
        }

        this.SweepPoints = value;
        return this.SweepPoints;
    }

    #endregion
}
/// <summary> Specifies the source sweep modes. </summary>
public enum SweepMode
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None,

    /// <summary> An enum constant representing the fixed option. </summary>
    [System.ComponentModel.Description( "Fixed (FIX)" )]
    Fixed,

    /// <summary> An enum constant representing the sweep option. </summary>
    [System.ComponentModel.Description( "Sweep (SWE)" )]
    Sweep,

    /// <summary> An enum constant representing the list option. </summary>
    [System.ComponentModel.Description( "List (LIST)" )]
    List
}
