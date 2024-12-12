namespace cc.isr.VI;

/// <summary> Defines a Multimeter Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-01-15 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class MultimeterSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
    /// </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// Subsystem</see>. </param>
    /// <param name="readingAmounts">  The reading amounts. </param>
    protected MultimeterSubsystemBase( StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem )
    {
        this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionRange = Ranges.NonnegativeFullRange;
        this.DefaultFunctionModeDecimalPlaces = 3;
        this.ReadingAmounts = readingAmounts;
        this._apertureRange = Ranges.StandardApertureRange;
        this._filterCountRange = Ranges.StandardFilterCountRange;
        this._filterWindowRange = Ranges.StandardFilterWindowRange;
        this._powerLineCyclesRange = Ranges.StandardPowerLineCyclesRange;
        this.FunctionUnit = this.DefaultFunctionUnit;
        this._functionRange = this.DefaultFunctionRange;
        this._functionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
        this.FunctionModeReadWrites = [];
        this.DefineFunctionModeReadWrites();
        this.OpenDetectorCapabilities = [];
        this.DefineOpenDetectorCapabilities();
        this.FunctionModeDecimalPlaces = [];
        this.DefineFunctionModeDecimalPlaces();
        this.FunctionModeRanges = [];
        this.DefineFunctionModeRanges();
        this.FunctionModeUnits = [];
        this.DefineFunctionModeUnits();
        this.AutoDelayModeReadWrites = [];
        this.DefineAutoDelayModeReadWrites();
        this.MultimeterMeasurementUnitReadWrites = [];
        this.DefineMultimeterMeasurementUnitReadWrites();
        this.InputImpedanceModeReadWrites = [];
        this.DefineInputImpedanceModeReadWrites();
        this._failureCode = string.Empty;
        this._failureLongDescription = string.Empty;
        this._failureShortDescription = string.Empty;
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
        this.DefineFunctionClearKnownState();
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Customizes the reset state. </remarks>
    public override void InitKnownState()
    {
        base.InitKnownState();
        _ = this.ParsePrimaryReading( string.Empty );
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.ApertureRange = Ranges.StandardApertureRange;
        this.FilterCountRange = Ranges.StandardFilterCountRange;
        this.FilterWindowRange = Ranges.StandardFilterWindowRange;
        this.PowerLineCyclesRange = Ranges.StandardPowerLineCyclesRange;
        this.FunctionUnit = this.DefaultFunctionUnit;
        this.FunctionRange = this.DefaultFunctionRange;
        this.FunctionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
    }

    #endregion

    #region " aperture "

    /// <summary> The aperture range. </summary>
    private Std.Primitives.RangeR _apertureRange;

    /// <summary> The aperture range in seconds. </summary>
    /// <value> The aperture range. </value>
    public Std.Primitives.RangeR ApertureRange
    {
        get => this._apertureRange;
        set
        {
            if ( this.ApertureRange != value )
            {
                this._apertureRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The Aperture. </summary>
    private double? _aperture;

    /// <summary>
    /// Gets or sets the cached sense Aperture. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <remarks>
    /// The aperture sets the amount of time the ADC takes when making a measurement, which is the
    /// integration period For the selected measurement Function. The integration period Is specified
    /// In seconds. In general, a short integration period provides a fast reading rate, while a long
    /// integration period provides better accuracy. The selected integration period Is a compromise
    /// between speed And accuracy. During the integration period, If an external trigger With a
    /// count Of 1 Is sent, the trigger Is ignored. If the count Is Set To more than 1, the first
    /// reading Is initialized by this trigger. Subsequent readings occur as rapidly as the
    /// instrument can make them. If a trigger occurs during the group measurement, the trigger Is
    /// latched And another group Of measurements With the same count will be triggered after the
    /// current group completes. You can also Set the integration rate by setting the number Of power
    /// line cycles (NPLCs). Changing the NPLC value changes the aperture time And changing the
    /// aperture time changes the NPLC value.
    /// </remarks>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Aperture
    {
        get => this._aperture;

        protected set
        {
            if ( !Nullable.Equals( this.Aperture, value ) )
            {
                this._aperture = value;
                this.PowerLineCycles = value.HasValue
                    ? StatusSubsystemBase.ToPowerLineCycles( TimeSpan.FromTicks( ( long ) (TimeSpan.TicksPerSecond * this._aperture!) ) )
                    : new double?();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the sense Aperture. </summary>
    /// <param name="value"> The Aperture. </param>
    /// <returns> The Aperture. </returns>
    public double? ApplyAperture( double value )
    {
        _ = this.WriteAperture( value );
        return this.QueryAperture();
    }

    /// <summary> Gets or sets The Aperture query command. </summary>
    /// <value> The Aperture query command. </value>
    protected virtual string ApertureQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Aperture. </summary>
    /// <returns> The Aperture or none if unknown. </returns>
    public double? QueryAperture()
    {
        this.Aperture = this.Session.Query( this.Aperture, this.ApertureQueryCommand );
        return this.Aperture;
    }

    /// <summary> Gets or sets The Aperture command format. </summary>
    /// <value> The Aperture command format. </value>
    protected virtual string ApertureCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes The Aperture without reading back the value from the device. </summary>
    /// <remarks> This command sets The Aperture. </remarks>
    /// <param name="value"> The Aperture. </param>
    /// <returns> The Aperture. </returns>
    public double? WriteAperture( double value )
    {
        this.Aperture = this.Session.WriteLine( value, this.ApertureCommandFormat );
        return this.Aperture;
    }

    #endregion

    #region " auto delay enabled "

    /// <summary> Auto Delay enabled. </summary>
    private bool? _autoDelayEnabled;

    /// <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    /// <remarks> When this is enabled, a delay is added before each measurement. </remarks>
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

    /// <summary> Gets or sets the automatic Delay enabled query command. </summary>
    /// <value> The automatic Delay enabled query command. </value>
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

    /// <summary> Gets or sets the automatic Delay enabled command Format. </summary>
    /// <value> The automatic Delay enabled query command. </value>
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

    #region " auto delay mode "

    /// <summary> Define automatic delay mode read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineAutoDelayModeReadWrites()
    {
        this.AutoDelayModeReadWrites = new();
        foreach ( MultimeterAutoDelayModes enumValue in Enum.GetValues( typeof( MultimeterAutoDelayModes ) ) )
            this.AutoDelayModeReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of multimeter Auto Delay Mode parses. </summary>
    /// <value> A Dictionary of multimeter Auto Delay Mode parses. </value>
    public Pith.EnumReadWriteCollection AutoDelayModeReadWrites { get; private set; }

    /// <summary> The supported automatic delay modes. </summary>
    private MultimeterAutoDelayModes _supportedAutoDelayModes;

    /// <summary>
    /// Gets or sets the supported Auto Delay Modes. This is a subset of the AutoDelays supported by
    /// the instrument.
    /// </summary>
    /// <value> The supported multimeter Auto Delay Modes. </value>
    public MultimeterAutoDelayModes SupportedAutoDelayModes
    {
        get => this._supportedAutoDelayModes;
        set
        {
            if ( !this.SupportedAutoDelayModes.Equals( value ) )
            {
                this._supportedAutoDelayModes = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The automatic delay mode. </summary>
    private MultimeterAutoDelayModes? _autoDelayMode;

    /// <summary> Gets or sets the cached multimeter Auto Delay Mode. </summary>
    /// <value>
    /// The <see cref="AutoDelayMode">multimeter Auto Delay Mode</see> or none if not set or unknown.
    /// </value>
    public MultimeterAutoDelayModes? AutoDelayMode
    {
        get => this._autoDelayMode;

        protected set
        {
            if ( !this.AutoDelayMode.Equals( value ) )
            {
                this._autoDelayMode = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the multimeter Auto Delay Mode. </summary>
    /// <param name="value"> The  multimeter Auto Delay Mode. </param>
    /// <returns>
    /// The <see cref="AutoDelayMode">source multimeter Auto Delay Mode</see> or none if unknown.
    /// </returns>
    public virtual MultimeterAutoDelayModes? ApplyAutoDelayMode( MultimeterAutoDelayModes value )
    {
        _ = this.WriteAutoDelayMode( value );
        return this.QueryAutoDelayMode();
    }

    /// <summary> Gets or sets the multimeter Auto Delay Mode query command. </summary>
    /// <value> The multimeter Auto Delay Mode query command. </value>
    protected virtual string AutoDelayModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the multimeter Auto Delay Mode. </summary>
    /// <returns>
    /// The <see cref="AutoDelayMode">multimeter Auto Delay Mode</see> or none if unknown.
    /// </returns>
    public virtual MultimeterAutoDelayModes? QueryAutoDelayMode()
    {
        return this.QueryAutoDelayMode( this.AutoDelayModeQueryCommand );
    }

    /// <summary> Queries the multimeter Auto Delay Mode. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>
    /// The <see cref="AutoDelayMode">multimeter Auto Delay Mode</see> or none if unknown.
    /// </returns>
    public virtual MultimeterAutoDelayModes? QueryAutoDelayMode( string queryCommand )
    {
        this.AutoDelayMode = this.Session.Query( this.AutoDelayMode.GetValueOrDefault( MultimeterAutoDelayModes.Off ), this.AutoDelayModeReadWrites, queryCommand );
        return this.AutoDelayMode;
    }

    /// <summary> Gets or sets the multimeter Auto Delay Mode command format. </summary>
    /// <value> The multimeter Auto Delay Mode command format. </value>
    protected virtual string AutoDelayModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the multimeter Auto Delay Mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The multimeter Auto Delay Mode. </param>
    /// <returns>
    /// The <see cref="AutoDelayMode">multimeter Auto Delay Mode</see> or none if unknown.
    /// </returns>
    public virtual MultimeterAutoDelayModes? WriteAutoDelayMode( MultimeterAutoDelayModes value )
    {
        this.AutoDelayMode = this.Session.Write( value, this.AutoDelayModeCommandFormat, this.AutoDelayModeReadWrites );
        return this.AutoDelayMode;
    }

    #endregion

    #region " auto range enabled "

    /// <summary> Auto Range enabled. </summary>
    private bool? _autoRangeEnabled;

    /// <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    /// <remarks>
    /// When this command is set to off, you must set the range. If you do not set the range, the
    /// instrument remains at the range that was selected by auto range. When this command Is set to
    /// on, the instrument automatically goes to the most sensitive range to perform the measurement.
    /// If a range Is manually selected through the front panel Or a remote command, this command Is
    /// automatically set to off. Auto range selects the best range In which To measure the signal
    /// that Is applied To the input terminals of the instrument. When auto range Is enabled, the
    /// range increases at 120 percent of range And decreases occurs When the reading Is less than 10
    /// percent Of nominal range. For example, If you are On the 1 volt range And auto range Is
    /// enabled, the instrument auto ranges up To the 10 volt range When the measurement exceeds 1.2
    /// volts. It auto ranges down To the 100 mV range When the measurement falls below 1 volt.
    /// </remarks>
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
                if ( value.HasValue && value.Value )
                {
                    this.Range = new double?();
                }
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
    /// <remarks> SCPI: ":SENSE:RANG:AUTO?". </remarks>
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
    /// <remarks> SCPI: ":SENSE:RANGE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
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

    #region " auto zero enabled "

    /// <summary> Auto Zero enabled. </summary>
    private bool? _autoZeroEnabled;

    /// <summary> Gets or sets the cached Auto Zero Enabled sentinel. </summary>
    /// <remarks>
    /// To ensure the accuracy of readings, the instrument must periodically get new measurements of
    /// its internal ground And voltage reference. The time interval between updates To these
    /// reference measurements Is determined by the integration aperture that Is being used for
    /// measurements. The Model DMM7510 uses separate reference And zero measurements For Each
    /// aperture. By Default, the instrument automatically checks these reference measurements
    /// whenever a signal measurement Is made. The time To make the reference measurements Is In
    /// addition To the normal measurement time. If timing Is critical, you can disable auto zero to
    /// avoid this time penalty. When auto zero Is set to off, the instrument may gradually drift out
    /// of specification. To minimize the drift, you can send the once command to make a reference
    /// And zero measurement immediately before a test sequence. For AC voltage And AC current
    /// measurements where the detector bandwidth Is set To 3 Hz Or 30 Hz, auto zero Is set on And
    /// cannot be changed.
    /// </remarks>
    /// <value>
    /// <c>null</c> if Auto Zero Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoZeroEnabled
    {
        get => this._autoZeroEnabled;

        protected set
        {
            if ( !Equals( this.AutoZeroEnabled, value ) )
            {
                this._autoZeroEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Auto Zero Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoZeroEnabled( bool value )
    {
        _ = this.WriteAutoZeroEnabled( value );
        return this.QueryAutoZeroEnabled();
    }

    /// <summary> Gets or sets the automatic Zero enabled query command. </summary>
    /// <remarks> SCPI: ":SENSE:RANG:AUTO?". </remarks>
    /// <value> The automatic Zero enabled query command. </value>
    protected virtual string AutoZeroEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Zero Enabled sentinel. Also sets the
    /// <see cref="AutoZeroEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoZeroEnabled()
    {
        this.AutoZeroEnabled = this.Session.Query( this.AutoZeroEnabled, this.AutoZeroEnabledQueryCommand );
        return this.AutoZeroEnabled;
    }

    /// <summary> Gets or sets the automatic Zero enabled command Format. </summary>
    /// <remarks> SCPI: ":SENSE:Zero:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic Zero enabled query command. </value>
    protected virtual string AutoZeroEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Zero Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoZeroEnabled( bool value )
    {
        this.AutoZeroEnabled = this.Session.WriteLine( value, this.AutoZeroEnabledCommandFormat );
        return this.AutoZeroEnabled;
    }

    #endregion

    #region " auto zero once "

    /// <summary> Gets or sets the automatic zero once command. </summary>
    /// <value> The automatic zero once command. </value>
    protected virtual string AutoZeroOnceCommand { get; set; } = string.Empty;

    /// <summary> Request a single auto zero. </summary>
    public void AutoZeroOnce()
    {
        _ = this.Session.WriteLineIfDefined( this.AutoZeroOnceCommand );
    }

    #endregion

    #region " bias "

    #region " bias actual "

    /// <summary> The BiasActual. </summary>
    private double? _biasActual;

    /// <summary>
    /// Gets or sets the cached Multimeter Bias Actual. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? BiasActual
    {
        get => this._biasActual;

        protected set
        {
            if ( !Nullable.Equals( this.BiasActual, value ) )
            {
                this._biasActual = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets The Bias Actual query command. </summary>
    /// <value> The BiasActual query command. </value>
    protected virtual string BiasActualQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Bias Actual. </summary>
    /// <returns> The Bias Actual or none if unknown. </returns>
    public double? QueryBiasActual()
    {
        this.BiasActual = this.Session.Query( this.BiasActual, this.BiasActualQueryCommand );
        return this.BiasActual;
    }

    #endregion

    #region " bias level "

    /// <summary> The BiasLevel. </summary>
    private double? _biasLevel;

    /// <summary>
    /// Gets or sets the cached Multimeter Bias Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <remarks>
    /// Selects the amount of current that is sourced by the instrument to make measurements. Applies
    /// to diode measurements.
    /// </remarks>
    /// <value> <c>null</c> if value is not known. </value>
    public double? BiasLevel
    {
        get => this._biasLevel;

        protected set
        {
            if ( !Nullable.Equals( this.BiasLevel, value ) )
            {
                this._biasLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Multimeter Bias Level. </summary>
    /// <param name="value"> The Bias Level. </param>
    /// <returns> The Bias Level. </returns>
    public double? ApplyBiasLevel( double value )
    {
        _ = this.WriteBiasLevel( value );
        return this.QueryBiasLevel();
    }

    /// <summary> Gets or sets The Bias Level query command. </summary>
    /// <value> The BiasLevel query command. </value>
    protected virtual string BiasLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Bias Level. </summary>
    /// <returns> The Bias Level or none if unknown. </returns>
    public double? QueryBiasLevel()
    {
        double? value = this.Session.Query( this.BiasLevel, this.BiasLevelQueryCommand );
        this.BiasLevel = value ?? new double?();
        return this.BiasLevel;
    }

    /// <summary> Gets or sets The Bias Level command format. </summary>
    /// <value> The BiasLevel command format. </value>
    protected virtual string BiasLevelCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes The Bias Level without reading back the value from the device. </summary>
    /// <remarks> This command sets The Bias Level. </remarks>
    /// <param name="value"> The Bias Level. </param>
    /// <returns> The Bias Level. </returns>
    public double? WriteBiasLevel( double value )
    {
        this.BiasLevel = this.Session.WriteLine( value, this.BiasLevelCommandFormat );
        return this.BiasLevel;
    }

    #endregion

    #endregion

    #region " connect/disconnect "

    /// <summary> Gets or sets the 'connect' command. </summary>
    /// <value> The 'connect' command. </value>
    protected virtual string ConnectCommand { get; set; } = string.Empty;

    /// <summary> Connects this object. </summary>
    public void Connect()
    {
        if ( !string.IsNullOrWhiteSpace( this.ConnectCommand ) )
            _ = this.Session.WriteLine( this.ConnectCommand );
    }

    /// <summary> Gets or sets the 'disconnect' command. </summary>
    /// <value> The 'disconnect' command. </value>
    protected virtual string DisconnectCommand { get; set; } = string.Empty;

    /// <summary> Disconnects this object. </summary>
    public void Disconnect()
    {
        if ( !string.IsNullOrWhiteSpace( this.DisconnectCommand ) )
            _ = this.Session.WriteLine( this.DisconnectCommand );
    }

    #endregion

    #region " filter "

    #region " filter count "

    /// <summary> The filter count range. </summary>
    private Std.Primitives.RangeI _filterCountRange;

    /// <summary> The Filter Count range in seconds. </summary>
    /// <value> The filter count range. </value>
    public Std.Primitives.RangeI FilterCountRange
    {
        get => this._filterCountRange;
        set
        {
            if ( this.FilterCountRange != value )
            {
                this._filterCountRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The FilterCount. </summary>
    private int? _filterCount;

    /// <summary>
    /// Gets or sets the cached sense Filter Count. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? FilterCount
    {
        get => this._filterCount;

        protected set
        {
            if ( !Nullable.Equals( this.FilterCount, value ) )
            {
                this._filterCount = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the sense Filter Count. </summary>
    /// <param name="value"> The Filter Count. </param>
    /// <returns> The Filter Count. </returns>
    public int? ApplyFilterCount( int value )
    {
        _ = this.WriteFilterCount( value );
        return this.QueryFilterCount();
    }

    /// <summary> Gets or sets The Filter Count query command. </summary>
    /// <value> The FilterCount query command. </value>
    protected virtual string FilterCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Filter Count. </summary>
    /// <returns> The Filter Count or none if unknown. </returns>
    public int? QueryFilterCount()
    {
        this.FilterCount = this.Session.Query( this.FilterCount, this.FilterCountQueryCommand );
        return this.FilterCount;
    }

    /// <summary> Gets or sets The Filter Count command format. </summary>
    /// <value> The FilterCount command format. </value>
    protected virtual string FilterCountCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes The Filter Count without reading back the value from the device. </summary>
    /// <remarks> This command sets The Filter Count. </remarks>
    /// <param name="value"> The Filter Count. </param>
    /// <returns> The Filter Count. </returns>
    public int? WriteFilterCount( int value )
    {
        this.FilterCount = this.Session.WriteLine( value, this.FilterCountCommandFormat );
        return this.FilterCount;
    }

    #endregion

    #region " filter enabled "

    /// <summary> Filter enabled. </summary>
    private bool? _filterEnabled;

    /// <summary> Gets or sets the cached Filter Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Filter Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? FilterEnabled
    {
        get => this._filterEnabled;

        protected set
        {
            if ( !Equals( this.FilterEnabled, value ) )
            {
                this._filterEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Filter Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyFilterEnabled( bool value )
    {
        _ = this.WriteFilterEnabled( value );
        return this.QueryFilterEnabled();
    }

    /// <summary> Gets or sets the Filter enabled query command. </summary>
    /// <remarks> TSP: _G.print(dmm.filter.enable==1) </remarks>
    /// <value> The Filter enabled query command. </value>
    protected virtual string FilterEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Filter Enabled sentinel. Also sets the
    /// <see cref="FilterEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryFilterEnabled()
    {
        this.FilterEnabled = this.Session.Query( this.FilterEnabled, this.FilterEnabledQueryCommand );
        return this.FilterEnabled;
    }

    /// <summary> Gets or sets the Filter enabled command Format. </summary>
    /// <remarks> TSP "dmm.filter.enable={0:'1';'1';'0'}". </remarks>
    /// <value> The Filter enabled query command. </value>
    protected virtual string FilterEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Filter Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteFilterEnabled( bool value )
    {
        this.FilterEnabled = this.Session.WriteLine( value, this.FilterEnabledCommandFormat );
        return this.FilterEnabled;
    }

    #endregion

    #region " moving average filter enabled "

    /// <summary> Moving Average Filter enabled. </summary>
    private bool? _movingAverageFilterEnabled;

    /// <summary> Gets or sets the cached Moving Average Filter Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Moving Average Filter Enabled is not known; <c>true</c> if output is on;
    /// otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? MovingAverageFilterEnabled
    {
        get => this._movingAverageFilterEnabled;

        protected set
        {
            if ( !Equals( this.MovingAverageFilterEnabled, value ) )
            {
                this._movingAverageFilterEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Moving Average Filter Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyMovingAverageFilterEnabled( bool value )
    {
        _ = this.WriteMovingAverageFilterEnabled( value );
        return this.QueryMovingAverageFilterEnabled();
    }

    /// <summary> Gets or sets the Moving Average Filter enabled query command. </summary>
    /// <remarks> TSP: _G.print(dmm.filter.type=0) </remarks>
    /// <value> The Moving Average Filter enabled query command. </value>
    protected virtual string MovingAverageFilterEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Moving Average Filter Enabled sentinel. Also sets the
    /// <see cref="MovingAverageFilterEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryMovingAverageFilterEnabled()
    {
        this.MovingAverageFilterEnabled = this.Session.Query( this.MovingAverageFilterEnabled, this.MovingAverageFilterEnabledQueryCommand );
        return this.MovingAverageFilterEnabled;
    }

    /// <summary> Gets or sets the Moving Average Filter enabled command Format. </summary>
    /// <remarks> TSP: "dmm.filter.type={0:'0';'0';'1'}". </remarks>
    /// <value> The Moving Average Filter enabled query command. </value>
    protected virtual string MovingAverageFilterEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Moving Average Filter Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteMovingAverageFilterEnabled( bool value )
    {
        this.MovingAverageFilterEnabled = this.Session.WriteLine( value, this.MovingAverageFilterEnabledCommandFormat );
        return this.MovingAverageFilterEnabled;
    }

    #endregion

    #region " filter window "

    /// <summary> The filter window range. </summary>
    private Std.Primitives.RangeR _filterWindowRange;

    /// <summary> The Filter Window range. </summary>
    /// <value> The filter window range. </value>
    public Std.Primitives.RangeR FilterWindowRange
    {
        get => this._filterWindowRange;
        set
        {
            if ( this.FilterWindowRange != value )
            {
                this._filterWindowRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The FilterWindow. </summary>
    private double? _filterWindow;

    /// <summary>
    /// Gets or sets the cached sense Filter Window. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? FilterWindow
    {
        get => this._filterWindow;

        protected set
        {
            if ( !Nullable.Equals( this.FilterWindow, value ) )
            {
                this._filterWindow = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the sense Filter Window. </summary>
    /// <param name="value"> The Filter Window. </param>
    /// <returns> The Filter Window. </returns>
    public double? ApplyFilterWindow( double value )
    {
        _ = this.WriteFilterWindow( value );
        return this.QueryFilterWindow();
    }

    /// <summary> Gets or sets The Filter Window query command. </summary>
    /// <value> The FilterWindow query command. </value>
    protected virtual string FilterWindowQueryCommand { get; set; } = string.Empty;

    /// <summary> The filter window scale. </summary>
    private double _filterWindowScale = 100d;

    /// <summary> Gets or sets the filter window scale. </summary>
    /// <value> The filter window scale. </value>
    public double FilterWindowScale
    {
        get => this._filterWindowScale;
        set
        {
            if ( this.FilterWindowScale != value )
            {
                this._filterWindowScale = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Queries The Filter Window. </summary>
    /// <returns> The Filter Window or none if unknown. </returns>
    public double? QueryFilterWindow()
    {
        double? value = this.Session.Query( this.FilterWindow, this.FilterWindowQueryCommand );
        this.FilterWindow = value.HasValue ? value.Value / this.FilterWindowScale : new double?();
        return this.FilterWindow;
    }

    /// <summary> Gets The Filter Window command format. </summary>
    /// <value> The FilterWindow command format. </value>
    protected virtual string FilterWindowCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes The Filter Window without reading back the value from the device. </summary>
    /// <remarks> This command sets The Filter Window. </remarks>
    /// <param name="value"> The Filter Window. </param>
    /// <returns> The Filter Window. </returns>
    public double? WriteFilterWindow( double value )
    {
        this.FilterWindow = this.Session.WriteLine( this.FilterWindowScale * value, this.FilterWindowCommandFormat );
        return this.FilterWindow;
    }

    #endregion

    #endregion

    #region " front terminals selected "

    /// <summary> Gets the front terminal label. </summary>
    /// <value> The front terminal label. </value>
    public string FrontTerminalLabel { get; set; } = "F";

    /// <summary> Gets the rear terminal label. </summary>
    /// <value> The rear terminal label. </value>
    public string RearTerminalLabel { get; set; } = "R";

    /// <summary> Gets the unknown terminal label. </summary>
    /// <value> The unknown terminal label. </value>
    public string UnknownTerminalLabel { get; set; } = string.Empty;

    /// <summary> Gets the terminals caption. </summary>
    /// <value> The terminals caption. </value>
    public string TerminalsCaption => this.FrontTerminalsSelected.HasValue ? this.FrontTerminalsSelected.Value ? this.FrontTerminalLabel : this.RearTerminalLabel : this.UnknownTerminalLabel;

    /// <summary> Gets true if the subsystem supports front terminals selection query. </summary>
    /// <value>
    /// The value indicating if the subsystem supports front terminals selection query.
    /// </value>
    public bool SupportsFrontTerminalsSelectionQuery => !string.IsNullOrWhiteSpace( this.FrontTerminalsSelectedQueryCommand );

    /// <summary> Front Terminals Selected. </summary>
    private bool? _frontTerminalsSelected;

    /// <summary> Gets or sets the cached Front Terminals Selected sentinel. </summary>
    /// <value>
    /// <c>null</c> if Front Terminals Selected is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? FrontTerminalsSelected
    {
        get => this._frontTerminalsSelected;

        protected set
        {
            if ( !Equals( this.FrontTerminalsSelected, value ) )
            {
                this._frontTerminalsSelected = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.TerminalsCaption ) );
            }
        }
    }

    /// <summary> Writes and reads back the Front Terminals Selected sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyFrontTerminalsSelected( bool value )
    {
        _ = this.WriteFrontTerminalsSelected( value );
        return this.QueryFrontTerminalsSelected();
    }

    /// <summary> Gets or sets the front terminals selected query command. </summary>
    /// <value> The front terminals selected query command. </value>
    protected virtual string FrontTerminalsSelectedQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Front Terminals Selected sentinel. Also sets the
    /// <see cref="FrontTerminalsSelected">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryFrontTerminalsSelected()
    {
        this.FrontTerminalsSelected = this.Session.Query( this.FrontTerminalsSelected, this.FrontTerminalsSelectedQueryCommand );
        return this.FrontTerminalsSelected;
    }

    /// <summary> Gets or sets the front terminals selected command format. </summary>
    /// <value> The front terminals selected command format. </value>
    protected virtual string FrontTerminalsSelectedCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Front Terminals Selected sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteFrontTerminalsSelected( bool value )
    {
        this.FrontTerminalsSelected = this.Session.WriteLine( value, this.FrontTerminalsSelectedCommandFormat );
        return this.FrontTerminalsSelected;
    }

    #endregion

    #region " limit 1 "

    #region " limit1 auto clear "

    /// <summary> Limit1 Auto Clear. </summary>
    private bool? _limit1AutoClear;

    /// <summary> Gets or sets the cached Limit1 Auto Clear sentinel. </summary>
    /// <remarks>
    /// When auto clear is set to on for a measure function, limit conditions are cleared
    /// automatically after each measurement. If you are making a series of measurements, the
    /// instrument shows the limit test result of the last measurement for the pass Or fail
    /// indication for the limit. If you want To know If any Of a series Of measurements failed the
    /// limit, Set the auto clear setting To off. When this set to off, a failed indication Is Not
    /// cleared automatically. It remains set until it Is cleared With the clear command. The auto
    /// clear setting affects both the high And low limits.
    /// </remarks>
    /// <value>
    /// <c>null</c> if Limit1 Auto Clear is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit1AutoClear
    {
        get => this._limit1AutoClear;

        protected set
        {
            if ( !Equals( this.Limit1AutoClear, value ) )
            {
                this._limit1AutoClear = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Auto Clear sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? ApplyLimit1AutoClear( bool value )
    {
        _ = this.WriteLimit1AutoClear( value );
        return this.QueryLimit1AutoClear();
    }

    /// <summary> Gets or sets the Limit1 Auto Clear query command. </summary>
    /// <remarks> TSP: _G.print(_G.dmm.measure.limit1.autoclear==dmm.ON) </remarks>
    /// <value> The Limit1 Auto Clear query command. </value>
    protected virtual string Limit1AutoClearQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit1 Auto Clear sentinel. Also sets the
    /// <see cref="Limit1AutoClear">AutoClear</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? QueryLimit1AutoClear()
    {
        this.Limit1AutoClear = this.Session.Query( this.Limit1AutoClear, this.Limit1AutoClearQueryCommand );
        return this.Limit1AutoClear;
    }

    /// <summary> Gets or sets the Limit1 Auto Clear command Format. </summary>
    /// <remarks> TSP: _G.dmm.measure.limit1.autoclear={0:'dmm.ON';'dmm.ON';'dmm.OFF'}". </remarks>
    /// <value> The Limit1 Auto Clear query command. </value>
    protected virtual string Limit1AutoClearCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Limit1 Auto Clear sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is Auto Clear. </param>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? WriteLimit1AutoClear( bool value )
    {
        this.Limit1AutoClear = this.Session.WriteLine( value, this.Limit1AutoClearCommandFormat );
        return this.Limit1AutoClear;
    }

    #endregion

    #region " limit1 enabled "

    /// <summary> Limit1 enabled. </summary>
    private bool? _limit1Enabled;

    /// <summary> Gets or sets the cached Limit1 Enabled sentinel. </summary>
    /// <remarks>
    /// This command enables or disables a limit test for the selected measurement function. When
    /// this attribute Is enabled, the limit 1 testing occurs on each measurement made by the
    /// instrument. Limit 1 testing compares the measurements To the high And low limit values. If a
    /// measurement falls outside these limits, the test fails.
    /// </remarks>
    /// <value>
    /// <c>null</c> if Limit1 Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit1Enabled
    {
        get => this._limit1Enabled;

        protected set
        {
            if ( !Equals( this.Limit1Enabled, value ) )
            {
                this._limit1Enabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyLimit1Enabled( bool value )
    {
        _ = this.WriteLimit1Enabled( value );
        return this.QueryLimit1Enabled();
    }

    /// <summary> Gets or sets the Limit1 enabled query command. </summary>
    /// <remarks> TSP: _G.print(_G.dmm.measure.limit1.autoclear==dmm.ON) </remarks>
    /// <value> The Limit1 enabled query command. </value>
    protected virtual string Limit1EnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit1 Enabled sentinel. Also sets the
    /// <see cref="Limit1Enabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryLimit1Enabled()
    {
        this.Limit1Enabled = this.Session.Query( this.Limit1Enabled, this.Limit1EnabledQueryCommand );
        return this.Limit1Enabled;
    }

    /// <summary> Gets or sets the Limit1 enabled command Format. </summary>
    /// <remarks> TSP _G.dmm.measure.limit1.enable={0:'dmm.ON';'dmm.ON';'dmm.OFF'} </remarks>
    /// <value> The Limit1 enabled query command. </value>
    protected virtual string Limit1EnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Limit1 Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteLimit1Enabled( bool value )
    {
        this.Limit1Enabled = this.Session.WriteLine( value, this.Limit1EnabledCommandFormat );
        return this.Limit1Enabled;
    }

    #endregion

    #region " limit1 lower level "

    /// <summary> The Limit1 Lower Level. </summary>
    private double? _limit1LowerLevel;

    /// <summary>
    /// Gets or sets the cached Limit1 Lower Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <remarks>
    /// This command sets the lower limit for the limit 1 test for the selected measure function.
    /// When limit 1 testing Is enabled, this causes a fail indication to occur when the measurement
    /// value Is less than this value.  Default Is 0.3 For limit 1 When the diode Function Is
    /// selected. The Default For limit 2 For the diode Function is() –1.
    /// </remarks>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Limit1LowerLevel
    {
        get => this._limit1LowerLevel;

        protected set
        {
            if ( !Nullable.Equals( this.Limit1LowerLevel, value ) )
            {
                this._limit1LowerLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Lower Level. </summary>
    /// <param name="value"> The Limit1 Lower Level. </param>
    /// <returns> The Limit1 Lower Level. </returns>
    public double? ApplyLimit1LowerLevel( double value )
    {
        _ = this.WriteLimit1LowerLevel( value );
        return this.QueryLimit1LowerLevel();
    }

    /// <summary> Gets or sets The Limit1 Lower Level query command. </summary>
    /// <value> The Limit1 Lower Level query command. </value>
    protected virtual string Limit1LowerLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit1 Lower Level. </summary>
    /// <returns> The Limit1 Lower Level or none if unknown. </returns>
    public double? QueryLimit1LowerLevel()
    {
        this.Limit1LowerLevel = this.Session.Query( this.Limit1LowerLevel, this.Limit1LowerLevelQueryCommand );
        return this.Limit1LowerLevel;
    }

    /// <summary> Gets or sets The Limit1 Lower Level command format. </summary>
    /// <value> The Limit1 Lower Level command format. </value>
    protected virtual string Limit1LowerLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit1 Lower Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit1 Lower Level. </remarks>
    /// <param name="value"> The Limit1 Lower Level. </param>
    /// <returns> The Limit1 Lower Level. </returns>
    public double? WriteLimit1LowerLevel( double value )
    {
        this.Limit1LowerLevel = this.Session.WriteLine( value, this.Limit1LowerLevelCommandFormat );
        return this.Limit1LowerLevel;
    }

    #endregion

    #region " limit1 upper level "

    /// <summary> The Limit1 Upper Level. </summary>
    private double? _limit1UpperLevel;

    /// <summary>
    /// Gets or sets the cached Limit1 Upper Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <remarks>
    /// This command sets the high limit for the limit 2 test for the selected measurement function.
    /// When limit 2 testing Is enabled, the instrument generates a fail indication When the
    /// measurement value Is more than this value. Default Is 0.8 For limit 1 When the diode Function
    /// Is selected; 10 When the continuity Function Is selected. The default for limit 2 for the
    /// diode And continuity functions Is 1.
    /// </remarks>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Limit1UpperLevel
    {
        get => this._limit1UpperLevel;

        protected set
        {
            if ( !Nullable.Equals( this.Limit1UpperLevel, value ) )
            {
                this._limit1UpperLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Upper Level. </summary>
    /// <param name="value"> The Limit1 Upper Level. </param>
    /// <returns> The Limit1 Upper Level. </returns>
    public double? ApplyLimit1UpperLevel( double value )
    {
        _ = this.WriteLimit1UpperLevel( value );
        return this.QueryLimit1UpperLevel();
    }

    /// <summary> Gets or sets The Limit1 Upper Level query command. </summary>
    /// <value> The Limit1 Upper Level query command. </value>
    protected virtual string Limit1UpperLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit1 Upper Level. </summary>
    /// <returns> The Limit1 Upper Level or none if unknown. </returns>
    public double? QueryLimit1UpperLevel()
    {
        this.Limit1UpperLevel = this.Session.Query( this.Limit1UpperLevel, this.Limit1UpperLevelQueryCommand );
        return this.Limit1UpperLevel;
    }

    /// <summary> Gets or sets The Limit1 Upper Level command format. </summary>
    /// <value> The Limit1 Upper Level command format. </value>
    protected virtual string Limit1UpperLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit1 Upper Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit1 Upper Level. </remarks>
    /// <param name="value"> The Limit1 Upper Level. </param>
    /// <returns> The Limit1 Upper Level. </returns>
    public double? WriteLimit1UpperLevel( double value )
    {
        this.Limit1UpperLevel = this.Session.WriteLine( value, this.Limit1UpperLevelCommandFormat );
        return this.Limit1UpperLevel;
    }

    #endregion

    #endregion

    #region " limit 2 "

    #region " limit2 auto clear "

    /// <summary> Limit2 Auto Clear. </summary>
    private bool? _limit2AutoClear;

    /// <summary> Gets or sets the cached Limit2 Auto Clear sentinel. </summary>
    /// <remarks>
    /// When auto clear is set to on for a measure function, limit conditions are cleared
    /// automatically after each measurement. If you are making a series of measurements, the
    /// instrument shows the limit test result of the last measurement for the pass Or fail
    /// indication for the limit. If you want To know If any Of a series Of measurements failed the
    /// limit, Set the auto clear setting To off. When this set to off, a failed indication Is Not
    /// cleared automatically. It remains set until it Is cleared With the clear command. The auto
    /// clear setting affects both the high And low limits.
    /// </remarks>
    /// <value>
    /// <c>null</c> if Limit2 Auto Clear is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit2AutoClear
    {
        get => this._limit2AutoClear;

        protected set
        {
            if ( !Equals( this.Limit2AutoClear, value ) )
            {
                this._limit2AutoClear = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Auto Clear sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? ApplyLimit2AutoClear( bool value )
    {
        _ = this.WriteLimit2AutoClear( value );
        return this.QueryLimit2AutoClear();
    }

    /// <summary> Gets or sets the Limit2 Auto Clear query command. </summary>
    /// <remarks> TSP: _G.print(_G.dmm.measure.limit1.autoclear==dmm.ON) </remarks>
    /// <value> The Limit2 Auto Clear query command. </value>
    protected virtual string Limit2AutoClearQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit2 Auto Clear sentinel. Also sets the
    /// <see cref="Limit2AutoClear">AutoClear</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? QueryLimit2AutoClear()
    {
        this.Limit2AutoClear = this.Session.Query( this.Limit2AutoClear, this.Limit2AutoClearQueryCommand );
        return this.Limit2AutoClear;
    }

    /// <summary> Gets or sets the Limit2 Auto Clear command Format. </summary>
    /// <remarks> TSP: "_G.dmm.measure.limit1.autoclear={0:'dmm.ON';'dmm.ON';'dmm.OFF'}". </remarks>
    /// <value> The Limit2 Auto Clear query command. </value>
    protected virtual string Limit2AutoClearCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Limit2 Auto Clear sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is Auto Clear. </param>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? WriteLimit2AutoClear( bool value )
    {
        this.Limit2AutoClear = this.Session.WriteLine( value, this.Limit2AutoClearCommandFormat );
        return this.Limit2AutoClear;
    }

    #endregion

    #region " limit2 enabled "

    /// <summary> Limit2 enabled. </summary>
    private bool? _limit2Enabled;

    /// <summary> Gets or sets the cached Limit2 Enabled sentinel. </summary>
    /// <remarks>
    /// This command enables or disables a limit test for the selected measurement function. When
    /// this attribute Is enabled, the limit 2 testing occurs on each measurement made by the
    /// instrument. Limit 2 testing compares the measurements To the high And low limit values. If a
    /// measurement falls outside these limits, the test fails.
    /// </remarks>
    /// <value>
    /// <c>null</c> if Limit2 Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit2Enabled
    {
        get => this._limit2Enabled;

        protected set
        {
            if ( !Equals( this.Limit2Enabled, value ) )
            {
                this._limit2Enabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyLimit2Enabled( bool value )
    {
        _ = this.WriteLimit2Enabled( value );
        return this.QueryLimit2Enabled();
    }

    /// <summary> Gets or sets the Limit2 enabled query command. </summary>
    /// <remarks> TSP: _G.print(_G.dmm.measure.limit2.autoclear==dmm.ON) </remarks>
    /// <value> The Limit2 enabled query command. </value>
    protected virtual string Limit2EnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit2 Enabled sentinel. Also sets the
    /// <see cref="Limit2Enabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryLimit2Enabled()
    {
        this.Limit2Enabled = this.Session.Query( this.Limit2Enabled, this.Limit2EnabledQueryCommand );
        return this.Limit2Enabled;
    }

    /// <summary> Gets or sets the Limit2 enabled command Format. </summary>
    /// <remarks> TSP: _G.dmm.measure.limit2.enable={0:'dmm.ON';'dmm.ON';'dmm.OFF'} </remarks>
    /// <value> The Limit2 enabled query command. </value>
    protected virtual string Limit2EnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Limit2 Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteLimit2Enabled( bool value )
    {
        this.Limit2Enabled = this.Session.WriteLine( value, this.Limit2EnabledCommandFormat );
        return this.Limit2Enabled;
    }

    #endregion

    #region " limit2 lower level "

    /// <summary> The Limit2 Lower Level. </summary>
    private double? _limit2LowerLevel;

    /// <summary>
    /// Gets or sets the cached Limit2 Lower Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <remarks>
    /// This command sets the lower limit for the limit 1 test for the selected measure function.
    /// When limit 1 testing Is enabled, this causes a fail indication to occur when the measurement
    /// value Is less than this value.  Default Is 0.3 For limit 1 When the diode Function Is
    /// selected. The Default For limit 2 For the diode Function is() –1.
    /// </remarks>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Limit2LowerLevel
    {
        get => this._limit2LowerLevel;

        protected set
        {
            if ( !Nullable.Equals( this.Limit2LowerLevel, value ) )
            {
                this._limit2LowerLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Lower Level. </summary>
    /// <param name="value"> The Limit2 Lower Level. </param>
    /// <returns> The Limit2 Lower Level. </returns>
    public double? ApplyLimit2LowerLevel( double value )
    {
        _ = this.WriteLimit2LowerLevel( value );
        return this.QueryLimit2LowerLevel();
    }

    /// <summary> Gets or sets The Limit2 Lower Level query command. </summary>
    /// <value> The Limit2 Lower Level query command. </value>
    protected virtual string Limit2LowerLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit2 Lower Level. </summary>
    /// <returns> The Limit2 Lower Level or none if unknown. </returns>
    public double? QueryLimit2LowerLevel()
    {
        this.Limit2LowerLevel = this.Session.Query( this.Limit2LowerLevel, this.Limit2LowerLevelQueryCommand );
        return this.Limit2LowerLevel;
    }

    /// <summary> Gets or sets The Limit2 Lower Level command format. </summary>
    /// <value> The Limit2 Lower Level command format. </value>
    protected virtual string Limit2LowerLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit2 Lower Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit2 Lower Level. </remarks>
    /// <param name="value"> The Limit2 Lower Level. </param>
    /// <returns> The Limit2 Lower Level. </returns>
    public double? WriteLimit2LowerLevel( double value )
    {
        this.Limit2LowerLevel = this.Session.WriteLine( value, this.Limit2LowerLevelCommandFormat );
        return this.Limit2LowerLevel;
    }

    #endregion

    #region " limit2 upper level "

    /// <summary> The Limit2 Upper Level. </summary>
    private double? _limit2UpperLevel;

    /// <summary>
    /// Gets or sets the cached Limit2 Upper Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <remarks>
    /// This command sets the high limit for the limit 2 test for the selected measurement function.
    /// When limit
    /// 2 testing Is enabled, the instrument generates a fail indication When the measurement value
    /// Is more than this value. Default Is 0.8 For limit 1 When the diode Function Is selected; 10
    /// When the continuity Function Is selected. The default for limit 2 for the diode And
    /// continuity functions Is 1.
    /// </remarks>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Limit2UpperLevel
    {
        get => this._limit2UpperLevel;

        protected set
        {
            if ( !Nullable.Equals( this.Limit2UpperLevel, value ) )
            {
                this._limit2UpperLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Upper Level. </summary>
    /// <param name="value"> The Limit2 Upper Level. </param>
    /// <returns> The Limit2 Upper Level. </returns>
    public double? ApplyLimit2UpperLevel( double value )
    {
        _ = this.WriteLimit2UpperLevel( value );
        return this.QueryLimit2UpperLevel();
    }

    /// <summary> Gets or sets The Limit2 Upper Level query command. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The Limit2 Upper Level query command. </value>
    protected virtual string Limit2UpperLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit2 Upper Level. </summary>
    /// <returns> The Limit2 Upper Level or none if unknown. </returns>
    public double? QueryLimit2UpperLevel()
    {
        this.Limit2UpperLevel = this.Session.Query( this.Limit2UpperLevel, this.Limit2UpperLevelQueryCommand );
        return this.Limit2UpperLevel;
    }

    /// <summary> Gets or sets The Limit2 Upper Level command format. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The Limit2 Upper Level command format. </value>
    protected virtual string Limit2UpperLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit2 Upper Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit2 Upper Level. </remarks>
    /// <param name="value"> The Limit2 Upper Level. </param>
    /// <returns> The Limit2 Upper Level. </returns>
    public double? WriteLimit2UpperLevel( double value )
    {
        this.Limit2UpperLevel = this.Session.WriteLine( value, this.Limit2UpperLevelCommandFormat );
        return this.Limit2UpperLevel;
    }

    #endregion

    #endregion

    #region " measure "

    /// <summary> Estimates the lower bound on measurement time. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <returns> A TimeSpan. </returns>
    public virtual TimeSpan EstimateMeasurementTime()
    {
        if ( !this.PowerLineCycles.HasValue )
            throw new InvalidOperationException( $"{nameof( this.PowerLineCycles )} value not set" );
        if ( !this.FilterEnabled.HasValue )
            throw new InvalidOperationException( $"{nameof( this.FilterEnabled )} value not set" );
        double aperture = this.PowerLineCycles.Value / this.StatusSubsystem.LineFrequency.GetValueOrDefault( 60d );
        double timeSeconds = 0d;
        if ( this.FilterEnabled.Value )
        {
            if ( !this.FilterCount.HasValue )
                throw new InvalidOperationException( $"{nameof( this.FilterCount )} value not set" );
            if ( this.FilterCount.Value > 0 )
            {
                // if auto zero once is included the time maybe too long
                timeSeconds = aperture * this.FilterCount.Value;
            }
            else
            {
                // assumes auto zero
                timeSeconds = aperture * 2d;
            }
        }

        if ( ( int? ) this.FunctionMode is ( int? ) MultimeterFunctionModes.ResistanceFourWire
            or ( int? ) MultimeterFunctionModes.ResistanceTwoWire
            or ( int? ) MultimeterFunctionModes.ResistanceCommonSide )
        {
            // assumes that resistance measurements take twice the duration.
            timeSeconds += timeSeconds;
        }

        return TimeSpan.FromTicks( ( long ) (TimeSpan.TicksPerSecond * timeSeconds) );
    }

    /// <summary> Gets or sets The Measure query command. </summary>
    /// <value> The Measure query command. </value>
    protected virtual string MeasureQueryCommand { get; set; } = string.Empty;

    /// <summary> Reads a value in to the primary reading and converts it to Double. </summary>
    /// <returns> The measured value or none if unknown. </returns>
    public virtual double? MeasurePrimaryReading()
    {
        this.Session.MakeEmulatedReplyIfEmpty( this.ReadingAmounts.PrimaryReading!.Generator.Value.ToString() );
        return this.MeasurePrimaryReading( this.MeasureQueryCommand );
    }

    /// <summary> Queries the reading and parse the reading amounts. </summary>
    /// <returns> The reading or none if unknown. </returns>
    public virtual double? MeasureReadingAmounts()
    {
        this.Session.MakeEmulatedReplyIfEmpty( this.ReadingAmounts.PrimaryReading!.Generator.Value.ToString() );
        return this.MeasureReadingAmounts( this.MeasureQueryCommand );
    }

    #endregion

    #region " open detector enabled "

    /// <summary> Define open detector capabilities. </summary>
    private void DefineOpenDetectorCapabilities()
    {
        this.OpenDetectorCapabilities = [];
        foreach ( MultimeterFunctionModes functionMode in Enum.GetValues( typeof( MultimeterFunctionModes ) ) )
            this.OpenDetectorCapabilities.Add( ( int ) functionMode, false );
    }

    /// <summary>
    /// Gets or sets a list of open detector capability for each multimeter function mode.
    /// </summary>
    /// <value> The open detector capability for each multimeter function mode. </value>
    public BooleanDictionary OpenDetectorCapabilities { get; private set; }

    /// <summary> Gets or sets the default open detector capable. </summary>
    /// <value> The default open detector capable. </value>
    public bool DefaultOpenDetectorCapable { get; set; } = false;

    /// <summary> QueryEnum if 'functionMode' is open detector capable. </summary>
    /// <param name="functionMode"> The function mode. </param>
    /// <returns> <c>true</c> if open detector capable; otherwise <c>false</c> </returns>
    public virtual bool IsOpenDetectorCapable( int functionMode )
    {
        return this.OpenDetectorCapabilities[functionMode];
    }

    /// <summary> True if function open detector capable. </summary>
    private bool _functionOpenDetectorCapable;

    /// <summary> Gets or sets the function open detector capable. </summary>
    /// <value> The function open detector capable. </value>
    public bool FunctionOpenDetectorCapable
    {
        get => this._functionOpenDetectorCapable;

        protected set
        {
            if ( this.FunctionOpenDetectorCapable != value )
            {
                this._functionOpenDetectorCapable = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Open Detector enabled. </summary>
    private bool? _openDetectorEnabled;

    /// <summary> Gets or sets the cached Open Detector Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Open Detector Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? OpenDetectorEnabled
    {
        get => this._openDetectorEnabled;

        protected set
        {
            if ( !Equals( this.OpenDetectorEnabled, value ) )
            {
                this._openDetectorEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Open Detector Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyOpenDetectorEnabled( bool value )
    {
        _ = this.WriteOpenDetectorEnabled( value );
        return this.QueryOpenDetectorEnabled();
    }

    /// <summary> Gets the automatic Zero enabled query command. </summary>
    /// <remarks> TSP: _G.print(_G.dmm.opendetector==1) </remarks>
    /// <value> The automatic Zero enabled query command. </value>
    protected virtual string OpenDetectorEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Open Detector Enabled sentinel. Also sets the
    /// <see cref="OpenDetectorEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryOpenDetectorEnabled()
    {
        this.OpenDetectorEnabled = this.Session.Query( this.OpenDetectorEnabled, this.OpenDetectorEnabledQueryCommand );
        return this.OpenDetectorEnabled;
    }

    /// <summary> Gets the automatic Zero enabled command Format. </summary>
    /// <remarks> TSP: _G.opendetector={0:'1';'1';'0'}". </remarks>
    /// <value> The automatic Zero enabled query command. </value>
    protected virtual string OpenDetectorEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Open Detector Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteOpenDetectorEnabled( bool value )
    {
        this.OpenDetectorEnabled = this.Session.WriteLine( value, this.OpenDetectorEnabledCommandFormat );
        return this.OpenDetectorEnabled;
    }

    #endregion

    #region " power line cycles (nplc) "

    /// <summary> Gets the power line cycles decimal places. </summary>
    /// <value> The power line decimal places. </value>
    public int PowerLineCyclesDecimalPlaces => ( int ) Math.Max( 0d, 1d - Math.Log10( this.PowerLineCyclesRange.Min ) );

    /// <summary> The power line cycles range. </summary>
    private Std.Primitives.RangeR _powerLineCyclesRange;

    /// <summary> The power line cycles range in units. </summary>
    /// <value> The power line cycles range. </value>
    public Std.Primitives.RangeR PowerLineCyclesRange
    {
        get => this._powerLineCyclesRange;
        set
        {
            if ( this.PowerLineCyclesRange != value )
            {
                this._powerLineCyclesRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The Power Line Cycles. </summary>
    private double? _powerLineCycles;

    /// <summary>
    /// Gets or sets the cached sense PowerLineCycles. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? PowerLineCycles
    {
        get => this._powerLineCycles;

        protected set
        {
            if ( value.HasValue && !Nullable.Equals( this.PowerLineCycles, value ) )
            {
                this._powerLineCycles = value;
                this._aperture = StatusSubsystemBase.FromPowerLineCycles( this._powerLineCycles.Value ).TotalSeconds;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the sense PowerLineCycles. </summary>
    /// <param name="value"> The Power Line Cycles. </param>
    /// <returns> The Power Line Cycles. </returns>
    public double? ApplyPowerLineCycles( double value )
    {
        _ = this.WritePowerLineCycles( value );
        return this.QueryPowerLineCycles();
    }

    /// <summary> Gets or sets The Power Line Cycles query command. </summary>
    /// <value> The Power Line Cycles query command. </value>
    protected virtual string PowerLineCyclesQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Power Line Cycles. </summary>
    /// <returns> The Power Line Cycles or none if unknown. </returns>
    public double? QueryPowerLineCycles()
    {
        this.PowerLineCycles = this.Session.Query( this.PowerLineCycles, this.PowerLineCyclesQueryCommand );
        return this.PowerLineCycles;
    }

    /// <summary> Gets or sets The Power Line Cycles command format. </summary>
    /// <value> The Power Line Cycles command format. </value>
    protected virtual string PowerLineCyclesCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Power Line Cycles without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Power Line Cycles. </remarks>
    /// <param name="value"> The Power Line Cycles. </param>
    /// <returns> The Power Line Cycles. </returns>
    public double? WritePowerLineCycles( double value )
    {
        this.PowerLineCycles = this.Session.WriteLine( value, this.PowerLineCyclesCommandFormat );
        return this.PowerLineCycles;
    }

    #endregion

    #region " range "

    /// <summary> The Range. </summary>
    private double? _range;

    /// <summary>
    /// Gets or sets the cached sense Range. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <remarks>
    /// You can assign any real number using this command. The instrument selects the closest fixed
    /// range that Is large enough to measure the entered number. For example, for current
    /// measurements, if you expect a reading Of approximately 9 mA, Set the range To 9 mA To Select
    /// the 10 mA range. When you read this setting, you see the positive full-scale value Of the
    /// measurement range that the instrument Is presently using. This command Is primarily intended
    /// To eliminate the time that Is required by the instrument To automatically search For a range.
    /// When a range Is fixed, any signal greater than the entered range generates an overrange
    /// condition. When an over-range condition occurs, the front panel displays "Overflow" And the
    /// remote interface returns 9.9e+37.
    /// </remarks>
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

    /// <summary> Writes and reads back the sense Range. </summary>
    /// <param name="value"> The Range. </param>
    /// <returns> The Range. </returns>
    public double? ApplyRange( double value )
    {
        _ = this.WriteRange( value );
        return this.QueryRange();
    }

    /// <summary> Gets or sets The Range query command. </summary>
    /// <value> The Range query command. </value>
    protected virtual string RangeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Range. </summary>
    /// <returns> The Range or none if unknown. </returns>
    public double? QueryRange()
    {
        this.Range = this.Session.Query( this.Range, this.RangeQueryCommand );
        return this.Range;
    }

    /// <summary> Gets or sets The Range command format. </summary>
    /// <value> The Range command format. </value>
    protected virtual string RangeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes The Range without reading back the value from the device. </summary>
    /// <remarks> This command sets The Range. </remarks>
    /// <param name="value"> The Range. </param>
    /// <returns> The Range. </returns>
    public double? WriteRange( double value )
    {
        this.Range = this.Session.WriteLine( value, this.RangeCommandFormat );
        return this.Range;
    }

    #endregion

    #region " read "

    /// <summary> Gets or sets the read buffer query command format. </summary>
    /// <value> The read buffer query command format. </value>
    protected virtual string ReadBufferQueryCommandFormat { get; set; } = string.Empty;

    /// <summary> Queries buffer reading. </summary>
    /// <remarks>
    /// This command initiates measurements using the present function setting, stores the readings
    /// in a reading buffer, and returns the last reading. The dmm.measure.count attribute determines
    /// how many measurements are performed. When you use a reading buffer with a command Or action
    /// that makes multiple readings, all readings are available In the reading buffer. However, only
    /// the last reading Is returned As a reading With the command. If you define a specific reading
    /// buffer, the reading buffer must exist before you make the measurement.
    /// </remarks>
    /// <param name="bufferName"> Name of the buffer. </param>
    /// <returns> The reading or none if unknown. </returns>
    public double? MeasureBuffer( string bufferName )
    {
        string? value = this.Session.EmulatedReply;
        string? result = this.Session.QueryTrimEnd( value, string.Format( this.ReadBufferQueryCommandFormat, bufferName ) );
        // the emulator will set the last reading. 
        return this.ParsePrimaryReading( result! );
    }

    #endregion
}
/// <summary> Specifies the Auto Delay modes. </summary>
[Flags]
public enum MultimeterAutoDelayModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "No set ()" )]
    None = 0,

    /// <summary> An enum constant representing the off] option. </summary>
    [System.ComponentModel.Description( "Off (_G.dmm.OFF)" )]
    Off = 1,

    /// <summary> An enum constant representing the on] option. </summary>
    [System.ComponentModel.Description( "On (_G.dmm.ON)" )]
    On = 2,

    /// <summary> An enum constant representing the once] option. </summary>
    [System.ComponentModel.Description( "Once (_G.dmm.AUTODELAY_ONCE)" )]
    Once = 4
}
