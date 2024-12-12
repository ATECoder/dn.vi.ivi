namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by a Sense Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific ReSenses, Inc.<para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class SenseSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary> Initializes a new instance of the <see cref="SenseSubsystemBase" /> class. </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    /// <param name="readingAmounts">  The reading amounts. </param>
    protected SenseSubsystemBase( StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem )
    {
        this.DefaultFunctionRange = Ranges.NonnegativeFullRange;
        this.DefaultFunctionModeDecimalPlaces = 3;
        this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionRange = Ranges.NonnegativeFullRange;
        this.DefaultFunctionModeDecimalPlaces = 3;
        this.ReadingAmounts = readingAmounts;
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

    #region " concurrent sense enabled "

    /// <summary> Auto Range enabled. </summary>
    private bool? _concurrentSenseEnabled;

    /// <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Range Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? ConcurrentSenseEnabled
    {
        get => this._concurrentSenseEnabled;

        protected set
        {
            if ( !Equals( this.ConcurrentSenseEnabled, value ) )
            {
                this._concurrentSenseEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Auto Range Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyConcurrentSenseEnabled( bool value )
    {
        _ = this.WriteConcurrentSenseEnabled( value );
        return this.QueryConcurrentSenseEnabled();
    }

    /// <summary> Gets the automatic Range enabled query command. </summary>
    /// <remarks> SCPI: ":SENS:CONC:STAT?". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string ConcurrentSenseEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Range Enabled sentinel. Also sets the
    /// <see cref="ConcurrentSenseEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryConcurrentSenseEnabled()
    {
        this.ConcurrentSenseEnabled = this.Session.Query( this.ConcurrentSenseEnabled, this.ConcurrentSenseEnabledQueryCommand );
        return this.ConcurrentSenseEnabled;
    }

    /// <summary> Gets the automatic Range enabled command Format. </summary>
    /// <remarks> SCPI: ":SENSE:CONC:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string ConcurrentSenseEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Range Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteConcurrentSenseEnabled( bool value )
    {
        this.ConcurrentSenseEnabled = this.Session.WriteLine( value, this.ConcurrentSenseEnabledCommandFormat );
        return this.ConcurrentSenseEnabled;
    }

    #endregion

    #region " power line cycles (nplc) "

    /// <summary> The Power Line Cycles. </summary>
    private double? _powerLineCycles;

    /// <summary> Gets the integration period. </summary>
    /// <value> The integration period. </value>
    public TimeSpan? IntegrationPeriod => this.PowerLineCycles.HasValue ? StatusSubsystemBase.FromPowerLineCycles( this.PowerLineCycles.Value ) : new TimeSpan?();

    /// <summary> Gets or sets the cached sense PowerLineCycles. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? PowerLineCycles
    {
        get => this._powerLineCycles;

        protected set
        {
            if ( !Nullable.Equals( this.PowerLineCycles, value ) )
            {
                this._powerLineCycles = value;
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

    #region " protection level "

    /// <summary> The Current Limit. </summary>
    private double? _protectionLevel;

    /// <summary>
    /// Gets or sets the cached source current Limit for a voltage source. Set to
    /// <see cref="cc.isr.VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="cc.isr.VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
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

    #region " fetch; data; read "

    /// <summary> Gets or sets the latest data query command. </summary>
    /// <remarks> SCPI: ":SENSE:DATA:LAT?". </remarks>
    /// <value> The latest data query command. </value>
    protected virtual string LatestDataQueryCommand { get; set; } = string.Empty;

    /// <summary> Fetches the latest data and parses it. </summary>
    /// <remarks>
    /// Issues the ':SENSE:DATA:LAT?' query, which reads data stored in the Sample Buffer.
    /// </remarks>
    /// <returns> The latest data. </returns>
    public virtual double? FetchLatestData()
    {
        return this.MeasureReadingAmounts( this.LatestDataQueryCommand );
    }

    #endregion
}
