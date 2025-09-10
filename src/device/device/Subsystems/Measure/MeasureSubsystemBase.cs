namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by a Measure Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class MeasureSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasureSubsystemBase" /> class.
    /// </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    /// <param name="readingAmounts">  The reading amounts. </param>
    protected MeasureSubsystemBase( StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem )
    {
        this.ReadingAmounts = readingAmounts;
        this.DefaultMeasurementUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionRange = Ranges.NonnegativeFullRange;
        this.DefaultFunctionModeDecimalPlaces = 3;
        this._apertureRange = Ranges.StandardApertureRange;
        this._filterCountRange = Ranges.StandardFilterCountRange;
        this._filterWindowRange = Ranges.StandardFilterWindowRange;
        this._powerLineCyclesRange = Ranges.StandardPowerLineCyclesRange;
        this.FunctionUnit = this.DefaultFunctionUnit;
        this._functionRange = this.DefaultFunctionRange;
        this.FunctionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
        this.FunctionModeDecimalPlaces = [];
        this.DefineFunctionModeDecimalPlaces();
        this.FunctionModeReadWrites = [];
        this.DefineFunctionModeReadWrites();
        this.FunctionModeRanges = [];
        this.DefineFunctionModeRanges();
        this.FunctionModeUnits = [];
        this.DefineFunctionModeUnits();

        this._failureShortDescription = string.Empty;
        this._failureLongDescription = string.Empty;
        this._failureCode = string.Empty;
        this.OpenDetectorKnownStates = [];
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
        this.NotifyPropertyChanged( nameof( this.ReadingAmounts ) );
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

    #region " init, read, fetch, measure "

    /// <summary> Gets or sets the fetch command. </summary>
    /// <remarks> SCPI: 'FETCh?'. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
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
    public virtual string FetchReading()
    {
        return this.FetchReading( this.FetchCommand );
    }

    /// <summary> Fetches the data. </summary>
    /// <remarks>
    /// Issues the 'FETCH?' query, which reads data stored in the Sample Buffer. If, for example,
    /// there are 20 data arrays stored in the Sample Buffer, then all 20 data arrays will be sent to
    /// the computer when 'FETCh?' is executed. Note that FETCh? does not affect data in the Sample
    /// Buffer. Thus, subsequent executions of FETCh? acquire the same data.
    /// </remarks>
    /// <returns> A Double? </returns>
    public virtual double? Fetch()
    {
        this.Session.MakeEmulatedReplyIfEmpty( this.ReadingAmounts.PrimaryReading!.Generator.Value.ToString() );
        return this.MeasureReadingAmounts( this.FetchCommand );
    }

    /// <summary> Gets or sets the read command. </summary>
    /// <remarks> SCPI: 'READ'. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The read command. </value>
    protected virtual string ReadCommand { get; set; } = string.Empty;

    /// <summary> Initiates an operation and then fetches the data. </summary>
    /// <remarks>
    /// Issues the 'READ?' query, which performs a trigger initiation and then a
    /// <see cref="Fetch">FETCh? </see>
    /// The initiate triggers a new measurement cycle which puts new data in the Sample Buffer. Fetch
    /// reads that new data. The
    /// <see cref="VI.MeasureSubsystemBase.MeasureReadingAmounts(string)">Measure</see> command places
    /// the instrument in a “one-shot” mode and then performs a read.
    /// </remarks>
    /// <returns> A Double? </returns>
    public virtual double? Read()
    {
        this.Session.MakeEmulatedReplyIfEmpty( this.ReadingAmounts.PrimaryReading!.Generator.Value.ToString() );
        return this.MeasureReadingAmounts( this.ReadCommand );
    }

    /// <summary> Gets or sets The Measure query command. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The Measure query command. </value>
    protected virtual string MeasureQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries readings into the reading amounts. </summary>
    /// <returns> The reading or none if unknown. </returns>
    public virtual double? MeasureReadingAmounts()
    {
        return this.MeasureReadingAmounts( this.MeasureQueryCommand );
    }

    /// <summary>
    /// QueryEnum a measured value from the instrument. Does not use
    /// <see cref="MeasureSubsystemBase.ReadingAmounts"/>.
    /// </summary>
    /// <returns> The reading or none if unknown. </returns>
    public virtual double? MeasurePrimaryReading()
    {
        return this.MeasurePrimaryReading( this.MeasureQueryCommand );
    }

    /// <summary> Estimates the lower bound on measurement time. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <returns> A TimeSpan. </returns>
    public virtual TimeSpan EstimateMeasurementTime()
    {
        if ( !this.PowerLineCycles.HasValue )
            throw new InvalidOperationException( $"{nameof( MultimeterSubsystemBase.PowerLineCycles )} value not set" );
        if ( !this.FilterEnabled.HasValue )
            throw new InvalidOperationException( $"{nameof( MultimeterSubsystemBase.FilterEnabled )} value not set" );
        double aperture = this.PowerLineCycles.Value / this.StatusSubsystem.LineFrequency.GetValueOrDefault( 60d );
        double timeSeconds = 0d;
        if ( this.FilterEnabled.Value )
        {
            if ( !this.FilterCount.HasValue )
                throw new InvalidOperationException( $"{nameof( MultimeterSubsystemBase.FilterCount )} value not set" );
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

        return TimeSpan.FromTicks( ( long ) (TimeSpan.TicksPerSecond * timeSeconds) );
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
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Aperture, value ) )
            {
                field = value;
                this.PowerLineCycles = value.HasValue ? this.Aperture!.Value * this.StatusSubsystem.LineFrequency : new double?();
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
    public virtual bool? AutoRangeEnabled
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
    /// <remarks> SCPI: ":RANG:AUTO?". </remarks>
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
    /// <remarks> SCPI: ":RANGE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
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

    /// <summary> Gets or sets the cached Auto Zero Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Zero Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoZeroEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.AutoZeroEnabled, value ) )
            {
                field = value;
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

    /// <summary> Gets or sets the 'automatic zero once' command. </summary>
    /// <value> The 'automatic zero once' command. </value>
    protected virtual string AutoZeroOnceCommand { get; set; } = string.Empty;

    /// <summary> Request a single auto zero. </summary>
    public void AutoZeroOnce()
    {
        if ( !string.IsNullOrWhiteSpace( this.AutoZeroOnceCommand ) )
            _ = this.Session.WriteLine( this.AutoZeroOnceCommand );
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

    /// <summary>
    /// Gets or sets the cached sense Filter Count. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? FilterCount
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.FilterCount, value ) )
            {
                field = value;
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

    /// <summary> Gets or sets the cached Filter Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Filter Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? FilterEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.FilterEnabled, value ) )
            {
                field = value;
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

    /// <summary> Gets or sets the cached Moving Average Filter Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Moving Average Filter Enabled is not known; <c>true</c> if output is on;
    /// otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? MovingAverageFilterEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.MovingAverageFilterEnabled, value ) )
            {
                field = value;
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

    /// <summary>
    /// Gets or sets the cached sense Filter Window. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? FilterWindow
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.FilterWindow, value ) )
            {
                field = value;
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

    /// <summary> Gets The Filter Window query command. </summary>
    /// <value> The FilterWindow query command. </value>
    protected virtual string FilterWindowQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Filter Window. </summary>
    /// <returns> The Filter Window or none if unknown. </returns>
    public double? QueryFilterWindow()
    {
        double? value = this.Session.Query( this.FilterWindow, this.FilterWindowQueryCommand );
        this.FilterWindow = value.HasValue ? 100d * value.Value : new double?();
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
        this.FilterWindow = this.Session.WriteLine( 100d * value, this.FilterWindowCommandFormat );
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

    /// <summary> Gets or sets the cached Front Terminals Selected sentinel. </summary>
    /// <value>
    /// <c>null</c> if Front Terminals Selected is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? FrontTerminalsSelected
    {
        get;

        protected set
        {
            if ( !Equals( this.FrontTerminalsSelected, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( MultimeterSubsystemBase.TerminalsCaption ) );
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
        get;

        protected set
        {
            if ( !Equals( this.Limit1AutoClear, value ) )
            {
                field = value;
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
    /// <remarks> TSP: "_G.dmm.measure.limit1.autoclear={0:'dmm.ON';'dmm.ON';'dmm.OFF'}". </remarks>
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
        get;

        protected set
        {
            if ( !Equals( this.Limit1Enabled, value ) )
            {
                field = value;
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
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Limit1LowerLevel, value ) )
            {
                field = value;
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
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Limit1UpperLevel, value ) )
            {
                field = value;
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
        get;

        protected set
        {
            if ( !Equals( this.Limit2AutoClear, value ) )
            {
                field = value;
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
        get;

        protected set
        {
            if ( !Equals( this.Limit2Enabled, value ) )
            {
                field = value;
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
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Limit2LowerLevel, value ) )
            {
                field = value;
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
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Limit2UpperLevel, value ) )
            {
                field = value;
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

    #region " measure unit "

    /// <summary> Gets or sets the default measurement unit. </summary>
    /// <value> The default measure unit. </value>
    public cc.isr.UnitsAmounts.Unit DefaultMeasurementUnit { get; set; }

    /// <summary> Gets or sets the function unit. </summary>
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

    #region " open detector enabled "

    /// <summary> Gets or sets a list of states of the open detector knowns. </summary>
    /// <value> The open detector known states. </value>
    public BooleanDictionary OpenDetectorKnownStates { get; private set; }

    /// <summary> Gets or sets the cached Open Detector Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Open Detector Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? OpenDetectorEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.OpenDetectorEnabled, value ) )
            {
                field = value;
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

    /// <summary>
    /// Gets or sets the cached sense PowerLineCycles. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? PowerLineCycles
    {
        get;

        protected set
        {
            if ( value is not null && !Nullable.Equals( this.PowerLineCycles, value ) )
            {
                field = value;
                this.Aperture = StatusSubsystemBase.FromPowerLineCycles( field.Value ).TotalSeconds;
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

    /// <summary>
    /// Gets or sets the cached sense Range. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Range
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Range, value ) )
            {
                field = value;
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

    #region " remote sense selected "

    /// <summary> Remote Sense Selected. </summary>

    /// <summary> Gets or sets the cached Remote Sense Selected sentinel. </summary>
    /// <value>
    /// <c>null</c> if Remote Sense Selected is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? RemoteSenseSelected
    {
        get;

        protected set
        {
            if ( !Equals( this.RemoteSenseSelected, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Remote Sense Selected sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyRemoteSenseSelected( bool value )
    {
        _ = this.WriteRemoteSenseSelected( value );
        return this.QueryRemoteSenseSelected();
    }

    /// <summary> Gets or sets the remote sense selected query command. </summary>
    /// <value> The remote sense selected query command. </value>
    protected virtual string RemoteSenseSelectedQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Remote Sense Selected sentinel. Also sets the
    /// <see cref="RemoteSenseSelected">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryRemoteSenseSelected()
    {
        this.RemoteSenseSelected = this.Session.Query( this.RemoteSenseSelected, this.RemoteSenseSelectedQueryCommand );
        return this.RemoteSenseSelected;
    }

    /// <summary> Gets or sets the remote sense selected command format. </summary>
    /// <value> The remote sense selected command format. </value>
    protected virtual string RemoteSenseSelectedCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Remote Sense Selected sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteRemoteSenseSelected( bool value )
    {
        this.RemoteSenseSelected = this.Session.WriteLine( value, this.RemoteSenseSelectedCommandFormat );
        return this.RemoteSenseSelected;
    }

    #endregion
}
