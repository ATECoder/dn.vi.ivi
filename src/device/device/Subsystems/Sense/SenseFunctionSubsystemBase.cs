namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a Sense function Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific ReSenses, Inc.<para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class SenseFunctionSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="SenseFunctionSubsystemBase" /> class.
    /// </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    /// <param name="readingAmounts">  The reading amounts. </param>
    protected SenseFunctionSubsystemBase( StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem )
    {
        this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionRange = Ranges.NonnegativeFullRange;
        this.DefaultFunctionModeDecimalPlaces = 3;
        this.ReadingAmounts = readingAmounts;
        this._apertureRange = Ranges.StandardApertureRange;
        this._powerLineCyclesRange = Ranges.StandardPowerLineCyclesRange;
        this.FunctionUnit = this.DefaultFunctionUnit;
        this.DefaultFunctionRange = Ranges.NonnegativeFullRange;
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
        this.AverageFilterTypesReadWrites = [];
        this.DefineAverageFilterTypesReadWrites();
        this.ConfigurationModeReadWrites = [];
        this.DefineConfigurationModeReadWrites();
        this._averageCountRange = new Std.Primitives.RangeI( 1, 100 );
        this._averagePercentWindowRange = new Std.Primitives.RangeR( 0.0d, 1d );
        this._resolutionDigitsRange = new Std.Primitives.RangeI( 4, 9 );
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

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.AutoZeroEnabled = true;
        this.AutoRangeEnabled = true;
        this.PowerLineCycles = 1;
        this.PowerLineCyclesRange = Ranges.StandardPowerLineCyclesRange;
        this.FunctionRange = this.DefaultFunctionRange;
        this.FunctionUnit = this.DefaultFunctionUnit;
        this.FunctionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
        this.AverageCount = 10;
        this.AveragePercentWindow = 1;
    }

    #endregion

    #region " aperture (nplc) "

    /// <summary> The aperture range. </summary>
    private Std.Primitives.RangeR _apertureRange;

    /// <summary> The Range of the Aperture. </summary>
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
    /// <value> <c>null</c> if value is not known. </value>
    public double? Aperture
    {
        get => this._aperture;

        protected set
        {
            if ( !Nullable.Equals( this.Aperture, value ) )
            {
                this._aperture = value;
                this.NotifyPropertyChanged();
                this.PowerLineCycles = value.HasValue ? StatusSubsystemBase.ToPowerLineCycles( TimeSpan.FromTicks( ( long ) (TimeSpan.TicksPerSecond * value.Value) ) ) : new double?();
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

    #region " auto delay "

    /// <summary> Gets or sets the supports automatic Delay. </summary>
    /// <value> The supports automatic Delay. </value>
    public bool SupportsAutoDelay { get; private set; } = false;

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
    /// <remarks> SCPI: "CURR:RANG:AUTO?". </remarks>
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
    /// <remarks> SCPI: "CURR:RANG:AUTO {0:'ON';'ON';'OFF'}". </remarks>
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

    /// <summary> Gets the automatic Zero enabled query command. </summary>
    /// <remarks> SCPI: "CURR:AZER?". </remarks>
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

    /// <summary> Gets the automatic Zero enabled command Format. </summary>
    /// <remarks> SCPI: "CURR:AZER {0:'ON';'ON';'OFF'}". </remarks>
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

    /// <summary> Gets the supports automatic zero. </summary>
    /// <value> The supports automatic zero. </value>
    public bool SupportsAutoZero => !string.IsNullOrWhiteSpace( this.AutoZeroEnabledCommandFormat );

    #endregion

    #region " configuration mode "

    /// <summary> Define configuration mode read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineConfigurationModeReadWrites()
    {
        this.ConfigurationModeReadWrites = new();
        foreach ( ConfigurationModes enumValue in Enum.GetValues( typeof( ConfigurationModes ) ) )
            this.ConfigurationModeReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of Configuration Mode parses. </summary>
    /// <value> A Dictionary of Configuration Mode parses. </value>
    public Pith.EnumReadWriteCollection ConfigurationModeReadWrites { get; private set; }

    /// <summary> The supported configuration modes. </summary>
    private ConfigurationModes _supportedConfigurationModes;

    /// <summary> Gets or sets the supported Configuration Modes. </summary>
    /// <value> The supported Configuration Modes. </value>
    public ConfigurationModes SupportedConfigurationModes
    {
        get => this._supportedConfigurationModes;
        set
        {
            if ( !this.SupportedConfigurationModes.Equals( value ) )
            {
                this._supportedConfigurationModes = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The configuration mode. </summary>
    private ConfigurationModes? _configurationMode;

    /// <summary> Gets or sets the cached source ConfigurationMode. </summary>
    /// <value>
    /// The <see cref="ConfigurationMode">source Configuration Mode</see> or none if not set or
    /// unknown.
    /// </value>
    public ConfigurationModes? ConfigurationMode
    {
        get => this._configurationMode;

        protected set
        {
            if ( !this.ConfigurationMode.Equals( value ) )
            {
                this._configurationMode = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the source Configuration Mode. </summary>
    /// <param name="value"> The  Source Configuration Mode. </param>
    /// <returns>
    /// The <see cref="ConfigurationMode">source Configuration Mode</see> or none if unknown.
    /// </returns>
    public ConfigurationModes? ApplyConfigurationMode( ConfigurationModes value )
    {
        _ = this.WriteConfigurationMode( value );
        return this.QueryConfigurationMode();
    }

    /// <summary> Gets or sets the Configuration Mode query command. </summary>
    /// <remarks> SCPI: :SENS:RES:MODE? </remarks>
    /// <value> The Configuration Mode query command. </value>
    protected virtual string ConfigurationModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Configuration Mode. </summary>
    /// <returns>
    /// The <see cref="ConfigurationMode">Configuration Mode</see> or none if unknown.
    /// </returns>
    public ConfigurationModes? QueryConfigurationMode()
    {
        this.ConfigurationMode = this.Session.Query( this.ConfigurationMode.GetValueOrDefault( ConfigurationModes.None ), this.ConfigurationModeReadWrites,
            this.ConfigurationModeQueryCommand );
        return this.ConfigurationMode;
    }

    /// <summary> Gets or sets the Configuration Mode command format. </summary>
    /// <remarks> SCPI: :SENS:RES:MODE {0}. </remarks>
    /// <value> The write Configuration Mode command format. </value>
    protected virtual string ConfigurationModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Configuration Mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The Configuration Mode. </param>
    /// <returns>
    /// The <see cref="ConfigurationMode">Configuration Mode</see> or none if unknown.
    /// </returns>
    public ConfigurationModes? WriteConfigurationMode( ConfigurationModes value )
    {
        this.ConfigurationMode = this.Session.Write( value, this.ConfigurationModeCommandFormat, this.ConfigurationModeReadWrites );
        return this.ConfigurationMode;
    }

    #endregion

    #region " delay "

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
    /// <remarks> SCPI: ":SENS:FRES:DEL?". </remarks>
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
    /// <remarks> SCPI: ":SENS:FRES:DEL {0:s\.FFFFFFF}". </remarks>
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

    #region " open lead detector enabled "

    /// <summary> Open Lead Detector enabled. </summary>
    private bool? _openLeadDetectorEnabled;

    /// <summary> Gets or sets the cached Open Lead Detector Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Open Lead Detector Enabled is not known; <c>true</c> if output is on;
    /// otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? OpenLeadDetectorEnabled
    {
        get => this._openLeadDetectorEnabled;

        protected set
        {
            if ( !Equals( this.OpenLeadDetectorEnabled, value ) )
            {
                this._openLeadDetectorEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Open Lead Detector Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyOpenLeadDetectorEnabled( bool value )
    {
        _ = this.WriteOpenLeadDetectorEnabled( value );
        return this.QueryOpenLeadDetectorEnabled();
    }

    /// <summary> Gets the Open Lead Detector enabled query command. </summary>
    /// <remarks> SCPI: ":FRES:ODET?". </remarks>
    /// <value> The Open Lead Detector enabled query command. </value>
    protected virtual string OpenLeadDetectorEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Open Lead Detector Enabled sentinel. Also sets the
    /// <see cref="OpenLeadDetectorEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryOpenLeadDetectorEnabled()
    {
        this.OpenLeadDetectorEnabled = this.Session.Query( this.OpenLeadDetectorEnabled, this.OpenLeadDetectorEnabledQueryCommand );
        return this.OpenLeadDetectorEnabled;
    }

    /// <summary> Gets the Open Lead Detector enabled command Format. </summary>
    /// <remarks> SCPI: ":FRES:ODET {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Open Lead Detector enabled query command. </value>
    protected virtual string OpenLeadDetectorEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Open Lead Detector Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteOpenLeadDetectorEnabled( bool value )
    {
        this.OpenLeadDetectorEnabled = this.Session.WriteLine( value, this.OpenLeadDetectorEnabledCommandFormat );
        return this.OpenLeadDetectorEnabled;
    }

    /// <summary> Gets the supports open lead detector. </summary>
    /// <value> The supports open lead detector. </value>
    public bool SupportsOpenLeadDetector => !string.IsNullOrWhiteSpace( this.OpenLeadDetectorEnabledCommandFormat );

    #endregion

    #region " power line cycles (nplc) "

    /// <summary> Gets the power line cycles decimal places. </summary>
    /// <value> The power line decimal places. </value>
    public int PowerLineCyclesDecimalPlaces => ( int ) Math.Max( 0d, 1d - Math.Log10( this.PowerLineCyclesRange.Min ) );

    /// <summary> The power line cycles range. </summary>
    private Std.Primitives.RangeR _powerLineCyclesRange;

    /// <summary> The Range of the power line cycles. </summary>
    /// <value> The power line cycles range. </value>
    public Std.Primitives.RangeR PowerLineCyclesRange
    {
        get => this._powerLineCyclesRange;
        set
        {
            // force a unit change as the value needs to be updated when the subsystem is switched.
            this._powerLineCyclesRange = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> The Power Line Cycles. </summary>
    private double? _powerLineCycles;

    /// <summary> Gets the integration period. </summary>
    /// <value> The integration period. </value>
    public TimeSpan? IntegrationPeriod => this.PowerLineCycles.HasValue ? StatusSubsystemBase.FromPowerLineCycles( this.PowerLineCycles.Value ) : new TimeSpan?();

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
            // force a unit change as the value needs to be updated when the subsystem is switched.
            this._powerLineCycles = value;
            this.Aperture = value.HasValue ? StatusSubsystemBase.FromPowerLineCycles( this.PowerLineCycles!.Value ).TotalSeconds : new double?();
            this.NotifyPropertyChanged();
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

    #region " protection level "

    /// <summary> The Current Limit. </summary>
    private double? _protectionLevel;

    /// <summary>
    /// Gets or sets the cached source current Limit for a voltage source. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
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

    #region " protection enabled "

    /// <summary> Protection enabled. </summary>
    private bool? _protectionEnabled;

    /// <summary>
    /// Gets or sets a cached value indicating whether Sense Voltage protection is enabled.
    /// </summary>
    /// <remarks>
    /// :SENSE:VOLT:PROT:STAT The setter enables or disables the over-Voltage protection (OCP)
    /// function. The enabled state is On (1); the disabled state is Off (0). If the over-Voltage
    /// protection function is enabled and the output goes into constant Voltage operation, the
    /// output is disabled and OCP is set in the Questionable Condition status register. The *RST
    /// value = Off.
    /// </remarks>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? ProtectionEnabled
    {
        get => this._protectionEnabled;

        protected set
        {
            if ( !Equals( this.ProtectionEnabled, value ) )
            {
                this._protectionEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Protection Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyProtectionEnabled( bool value )
    {
        _ = this.WriteProtectionEnabled( value );
        return this.QueryProtectionEnabled();
    }

    /// <summary> Gets or sets the Protection enabled query command. </summary>
    /// <remarks> SCPI: ":SENSE:PROT:STAT?". </remarks>
    /// <value> The Protection enabled query command. </value>
    protected virtual string ProtectionEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Protection Enabled sentinel. Also sets the
    /// <see cref="ProtectionEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryProtectionEnabled()
    {
        this.ProtectionEnabled = this.Session.Query( this.ProtectionEnabled, this.ProtectionEnabledQueryCommand );
        return this.ProtectionEnabled;
    }

    /// <summary> Gets or sets the Protection enabled command Format. </summary>
    /// <remarks> SCPI: ""SENSE:PROT:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Protection enabled query command. </value>
    protected virtual string ProtectionEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Protection Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteProtectionEnabled( bool value )
    {
        this.ProtectionEnabled = this.Session.WriteLine( value, this.ProtectionEnabledCommandFormat );
        return this.ProtectionEnabled;
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
            // force a unit change as the value needs to be updated when the subsystem is switched.
            this._range = value;
            this.NotifyPropertyChanged();
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

    #region " resolution digits "

    /// <summary> The resolution digits range. </summary>
    private Std.Primitives.RangeI _resolutionDigitsRange;

    /// <summary> The resolution digits range in seconds. </summary>
    /// <value> The resolution digits range. </value>
    public Std.Primitives.RangeI ResolutionDigitsRange
    {
        get => this._resolutionDigitsRange;
        set
        {
            if ( this.ResolutionDigitsRange != value )
            {
                this._resolutionDigitsRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The ResolutionDigits. </summary>
    private double? _resolutionDigits;

    /// <summary> Gets or sets the cached ResolutionDigits. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? ResolutionDigits
    {
        get => this._resolutionDigits;

        protected set
        {
            if ( !Nullable.Equals( this.ResolutionDigits, value ) )
            {
                this._resolutionDigits = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the ResolutionDigits. </summary>
    /// <param name="value"> The ResolutionDigits. </param>
    /// <returns> The ResolutionDigits. </returns>
    public double? ApplyResolutionDigits( double value )
    {
        _ = this.WriteResolutionDigits( value );
        return this.QueryResolutionDigits();
    }

    /// <summary> Gets or sets the ResolutionDigits query command. </summary>
    /// <value> The ResolutionDigits query command. </value>
    protected virtual string ResolutionDigitsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the ResolutionDigits. </summary>
    /// <returns> The ResolutionDigits or none if unknown. </returns>
    public double? QueryResolutionDigits()
    {
        this.ResolutionDigits = this.Session.Query( this.ResolutionDigits, this.ResolutionDigitsQueryCommand );
        return this.ResolutionDigits;
    }

    /// <summary> Gets or sets the ResolutionDigits command format. </summary>
    /// <value> The ResolutionDigits command format. </value>
    protected virtual string ResolutionDigitsCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the ResolutionDigits without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the ResolutionDigits. </remarks>
    /// <param name="value"> The ResolutionDigits. </param>
    /// <returns> The ResolutionDigits. </returns>
    public double? WriteResolutionDigits( double value )
    {
        this.ResolutionDigits = this.Session.WriteLine( value, this.ResolutionDigitsCommandFormat );
        return this.ResolutionDigits;
    }

    #endregion
}
/// <summary> Enumerates the configuration mode. </summary>
[Flags]
public enum ConfigurationModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 0,

    /// <summary> An enum constant representing the auto] option. </summary>
    [System.ComponentModel.Description( "Auto (AUTO)" )]
    Auto = 1,

    /// <summary> An enum constant representing the manual] option. </summary>
    [System.ComponentModel.Description( "Manual (MAN)" )]
    Manual = 2
}
