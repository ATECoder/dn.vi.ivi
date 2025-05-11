using cc.isr.Std.NumericExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Thermal transient Measure Subsystem. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-23 </para>
/// </remarks>
public class ThermalTransientMeasure : MeasureSubsystemBase
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="statusSubsystem">  The status subsystem. </param>
    /// <param name="thermalTransient"> The thermal transient element. </param>
    public ThermalTransientMeasure( StatusSubsystemBase statusSubsystem, ThermalTransient thermalTransient ) : base( statusSubsystem )
    {
        this.MeterEntity = ThermalTransientMeterEntity.Transient;
        this.ThermalTransient = thermalTransient;
    }

    #endregion

    #region " i presettable "

    /// <summary> Clears known state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineClearExecutionState()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing {this.EntityName} thermal transient execution state;. " );
        base.DefineClearExecutionState();
        this.ThermalTransient.DefineClearExecutionState();
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineKnownResetState()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resetting {this.EntityName} thermal transient to known state;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        base.DefineKnownResetState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this.ThermalTransient.ResetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary>
    /// Sets the subsystem known preset state. Read and apply the instrument defaults and query the
    /// configurations.
    /// </summary>
    /// <remarks>   2024-11-15. </remarks>
    public override void PresetKnownState()
    {
        base.PresetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this.ThermalTransient.PresetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    #endregion

    #region " device under test: thermal transient "

    /// <summary> Gets the <see cref="ThermalTransient">part Thermal Transient</see>. </summary>
    /// <value> The cold resistance. </value>
    public ThermalTransient ThermalTransient { get; set; }

    /// <summary> Gets the <see cref="MeasureBase">part primary measurement base of the thermal transient</see>. </summary>
    /// <value> The primary measurement base of the thermal transient. </value>
    protected override MeasureBase PrimaryMeasurement => this.ThermalTransient;

    #endregion

    #region " current level "

    private double? _currentLevel;

    /// <summary> Gets or sets the cached Source Current Level. </summary>
    /// <value> The Source Current Level. </value>
    public virtual double? CurrentLevel
    {
        get => this._currentLevel;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmTraceSettings.CurrentLevel;
            this.ThermalTransient.CurrentLevel = value.Value;
            _ = this.SetProperty( ref this._currentLevel, value );
        }
    }

    /// <summary> Writes and reads back the source Current Level. </summary>
    /// <remarks>
    /// This command set the immediate output Current Level. The value is in Amperes. The immediate
    /// Current Level is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <param name="value"> The Current Level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? ApplyCurrentLevel( double value )
    {
        _ = this.WriteCurrentLevel( value );
        return this.QueryCurrentLevel();
    }

    /// <summary> Queries the Current Level. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Current Level or none if unknown. </returns>
    public double? QueryCurrentLevel()
    {
        const decimal printFormat = 9.6m;
        this.CurrentLevel = this.Session.QueryPrint( this.CurrentLevel.GetValueOrDefault( 0.27d ), printFormat, "{0}.level", this.EntityName );
        return this.CurrentLevel;
    }

    /// <summary>
    /// Writes the source Current Level without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Current Level. The value is in Amperes. The immediate
    /// CurrentLevel is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Current Level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? WriteCurrentLevel( double value )
    {
        if ( !ThermalTransient.ValidateCurrentLevel( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( "{0}:levelSetter({1})", this.EntityName, value );
        this.CurrentLevel = value;
        return this.CurrentLevel;
    }

    #endregion

    #region " voltage limit "

    /// <summary>   (Immutable) the voltage limit default. </summary>
    public const double VoltageLimitDefault = 0.999d;

    /// <summary> The Voltage Limit. </summary>
    private double? _voltageLimit;

    /// <summary> Gets or sets the cached Source Voltage Limit. </summary>
    /// <value> The Source Voltage Limit. </value>
    public virtual double? VoltageLimit
    {
        get => this._voltageLimit;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmTraceSettings.VoltageLimit;
            this.ThermalTransient.VoltageLimit = value.Value;
            _ = this.SetProperty( ref this._voltageLimit, value );
        }
    }

    /// <summary> Writes and reads back the source Voltage Limit. </summary>
    /// <remarks>
    /// This command set the immediate output Voltage Limit. The value is in Volts. The immediate
    /// Voltage Limit is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Voltage Limit. </param>
    /// <returns> The Source Voltage Limit. </returns>
    public double? ApplyVoltageLimit( double value )
    {
        _ = this.WriteVoltageLimit( value );
        return this.QueryVoltageLimit();
    }

    /// <summary> Queries the Voltage Limit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Voltage Limit or none if unknown. </returns>
    public double? QueryVoltageLimit()
    {
        const decimal printFormat = 9.6m;
        this.VoltageLimit = this.Session.QueryPrint( this.VoltageLimit.GetValueOrDefault( ThermalTransientMeasure.VoltageLimitDefault ), printFormat, "{0}.limit", this.EntityName );
        return this.VoltageLimit;
    }

    /// <summary>
    /// Writes the source Voltage Limit without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Voltage Limit. The value is in Volts. The immediate
    /// VoltageLimit is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Voltage Limit. </param>
    /// <returns> The Source Voltage Limit. </returns>
    public double? WriteVoltageLimit( double value )
    {
        if ( !ThermalTransient.ValidateVoltageLimit( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( "{0}:limitSetter({1})", this.EntityName, value );
        this.VoltageLimit = value;
        return this.VoltageLimit;
    }

    #endregion

    #region " pulse duration "

    /// <summary> Gets the duration of the pulse. </summary>
    /// <value> The pulse duration. </value>
    public TimeSpan PulseDuration => TimeSpan.FromTicks( ( long ) Math.Round( TimeSpan.TicksPerSecond * this.TracePoints.GetValueOrDefault( 0 ) * this.SamplingInterval.GetValueOrDefault( 0d ) ) );

    #endregion

    #region " aperture "

    /// <summary> Writes the Aperture without reading back the value from the device. </summary>
    /// <remarks>
    /// This command sets the immediate output Aperture. The value is in Volts. The immediate
    /// Aperture is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Aperture. </param>
    /// <returns> The Aperture. </returns>
    public override double? WriteAperture( double value )
    {
        _ = base.WriteAperture( value );
        // update the maximum sampling rage.
        _ = this.Session.WriteLine( $"{this.EntityName}:maxRateSetter()" );
        // and read the current period.
        _ = this.QuerySamplingInterval();
        return this.Aperture;
    }

    #endregion

    #region " median filter size "

    /// <summary> The Median Filter Size. </summary>
    private int? _medianFilterSize;

    /// <summary> Gets or sets the cached Median Filter Size. </summary>
    /// <value> The Median Filter Size. </value>
    public int? MedianFilterSize
    {
        get => this._medianFilterSize;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmTraceSettings.MedianFilterLength;
            this.ThermalTransient.MedianFilterSize = value.Value;
            _ = this.SetProperty( ref this._medianFilterSize, value );
        }
    }

    /// <summary> Writes and reads back the Median Filter Size. </summary>
    /// <remarks>
    /// This command set the immediate output Median Filter Size. The value is in Volts. The
    /// immediate Median Filter Size is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Median Filter Size. </param>
    /// <returns> The Median Filter Size. </returns>
    public int? ApplyMedianFilterSize( int value )
    {
        _ = this.WriteMedianFilterSize( value );
        return this.QueryMedianFilterSize();
    }

    /// <summary> Queries the Median Filter Size. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Median Filter Size or none if unknown. </returns>
    public int? QueryMedianFilterSize()
    {
        const int printFormat = 1;
        this.MedianFilterSize = this.Session.QueryPrint( this.MedianFilterSize.GetValueOrDefault( 5 ), printFormat,
            $"{this.EntityName}.medianFilterSize" );
        return this.MedianFilterSize;
    }

    /// <summary>
    /// Writes the Median Filter Size without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Median Filter Size. The value is in Volts. The
    /// immediate LegacyDriver is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Median Filter Size. </param>
    /// <returns> The Median Filter Size. </returns>
    public int? WriteMedianFilterSize( int value )
    {
        if ( !ThermalTransient.ValidateMedianFilterSize( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( $"{this.EntityName}:medianFilterSizeSetter({value})" );
        this.MedianFilterSize = value;
        return this.MedianFilterSize;
    }

    #endregion

    #region " sampling interval "

    /// <summary> The Sampling Interval. </summary>
    private double? _samplingInterval;

    /// <summary> Gets or sets the cached Sampling Interval. </summary>
    /// <value> The Sampling Interval. </value>
    public double? SamplingInterval
    {
        get => this._samplingInterval;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmTraceSettings.SamplingInterval;
            this.ThermalTransient.SamplingInterval = value.Value;
            if ( this.SamplingInterval.Differs( value, 0.000001d ) )
            {
                this._samplingInterval = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sampling Interval. </summary>
    /// <remarks>
    /// This command set the immediate output Sampling Interval. The value is in Ohms. The immediate
    /// Sampling Interval is the output Low setting. At *RST, the Low values = 0.
    /// </remarks>
    /// <param name="value"> The Sampling Interval. </param>
    /// <returns> The Sampling Interval. </returns>
    public double? ApplySamplingInterval( double value )
    {
        _ = this.WriteSamplingInterval( value );
        return this.QuerySamplingInterval();
    }

    /// <summary> Queries the Sampling Interval. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Sampling Interval or none if unknown. </returns>
    public double? QuerySamplingInterval()
    {
        const decimal printFormat = 9.6m;
        this.SamplingInterval = this.Session.QueryPrint( this.SamplingInterval.GetValueOrDefault( 0.0001d ), printFormat,
            $"{this.EntityName}.period" );
        return this.SamplingInterval;
    }

    /// <summary>
    /// Writes the Sampling Interval without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Sampling Interval. The value is in Ohms. The immediate
    /// SamplingInterval is the output Low setting. At *RST, the Low values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Sampling Interval. </param>
    /// <returns> The Sampling Interval. </returns>
    public double? WriteSamplingInterval( double value )
    {
        if ( !ThermalTransient.ValidateLimit( value, out string details )
            || (this.TracePoints.HasValue && !ThermalTransient.ValidatePulseWidth( value, this.TracePoints.Value, out details )) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( $"{this.EntityName}:periodSetter({value})" );
        this.SamplingInterval = value;
        return this.SamplingInterval;
    }

    #endregion

    #region " trace points "

    /// <summary> The Trace Points. </summary>
    private int? _tracePoints;

    /// <summary> Gets or sets the cached Trace Points. </summary>
    /// <value> The Trace Points. </value>
    public int? TracePoints
    {
        get => this._tracePoints;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmTraceSettings.TracePoints;
            this.ThermalTransient.TracePoints = value.Value;
            _ = this.SetProperty( ref this._tracePoints, value );
        }
    }

    /// <summary> Writes and reads back the Trace Points. </summary>
    /// <remarks>
    /// This command set the immediate output Trace Points. The value is in Volts. The immediate
    /// Trace Points is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Trace Points. </param>
    /// <returns> The Trace Points. </returns>
    public int? ApplyTracePoints( int value )
    {
        _ = this.WriteTracePoints( value );
        return this.QueryTracePoints();
    }

    /// <summary> Queries the Trace Points. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Trace Points or none if unknown. </returns>
    public int? QueryTracePoints()
    {
        const int printFormat = 1;
        this.TracePoints = this.Session.QueryPrint( this.TracePoints.GetValueOrDefault( 100 ), printFormat, $"{this.EntityName}.points" );
        return this.TracePoints;
    }

    /// <summary> Writes the Trace Points without reading back the value from the device. </summary>
    /// <remarks>
    /// This command sets the immediate output Trace Points. The value is in Volts. The immediate
    /// ContactLimit is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Trace Points. </param>
    /// <returns> The Trace Points. </returns>
    public int? WriteTracePoints( int value )
    {
        if ( !ThermalTransient.ValidateTracePoints( value, out string details )
            || (this.SamplingInterval.HasValue && !ThermalTransient.ValidatePulseWidth( this.SamplingInterval.Value, value, out details )) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( $"{this.EntityName}:pointsSetter({value})" );
        this.TracePoints = value;
        return this.TracePoints;
    }

    #endregion

    #region " time series "

    /// <summary> The last trace. </summary>
    private string? _lastTrace;

    /// <summary> Gets or sets (protected) the last trace. </summary>
    /// <value> The last trace. </value>
    public string? LastTrace
    {
        get => this._lastTrace;

        protected set
        {
            this._lastTrace = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Gets or sets the thermal transient time series. </summary>
    /// <value> The last time series. </value>
    public IList<cc.isr.Std.Cartesian.CartesianPoint<double>>? LastTimeSeries { get; private set; }

    #endregion

    #region " voltage change "

    /// <summary> The Voltage Change. </summary>
    private double? _voltageChange;

    /// <summary> Gets or sets the cached Voltage Change. </summary>
    /// <value> The Voltage Change. </value>
    public double? VoltageChange
    {
        get => this._voltageChange;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmTraceSettings.VoltageChange;
            this.ThermalTransient.AllowedVoltageChange = value.Value;
            if ( this.VoltageChange.Differs( value, 0.000001d ) )
            {
                this._voltageChange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Voltage Change. </summary>
    /// <remarks>
    /// This command set the immediate output Voltage Change. The value is in Volts. The immediate
    /// Voltage Change is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Voltage Change. </param>
    /// <returns> The Voltage Change. </returns>
    public double? ApplyVoltageChange( double value )
    {
        _ = this.WriteVoltageChange( value );
        return this.QueryVoltageChange();
    }

    /// <summary> Queries the Voltage Change. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Voltage Change or none if unknown. </returns>
    public double? QueryVoltageChangeObsolete()
    {
        const decimal printFormat = 9.6m;
        this.VoltageChange = this.Session.QueryPrint( this.VoltageChange.GetValueOrDefault( 0.099d ), printFormat,
            $"{this.EntityName}.maxVoltageChange" );
        return this.VoltageChange;
    }

    /// <summary> Writes the Voltage Change without reading back the value from the device. </summary>
    /// <remarks>
    /// This command sets the immediate output Voltage Change. The value is in Volts. The immediate
    /// VoltageChange is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Voltage Change. </param>
    /// <returns> The Voltage Change. </returns>
    public double? WriteVoltageChange( double value )
    {
        if ( !ThermalTransient.ValidateVoltageChange( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( $"{this.EntityName}:maxVoltageChangeSetter({value})" );
        this.VoltageChange = value;
        return this.VoltageChange;
    }

    #endregion

    #region " configure "

    /// <summary> Applies the instrument defaults. </summary>
    /// <remarks> This is required until the reset command gets implemented. </remarks>
    public override void ApplyInstrumentDefaults()
    {
        base.ApplyInstrumentDefaults();
        _ = this.Session.WriteLine( $"{this.EntityName}:maxRateSetter()" );
        _ = this.Session.QueryOperationCompleted();
        _ = this.Session.WriteLine( $"{this.EntityName}:maxVoltageChangeSetter({this.DefaultsName}.level)" );
        _ = this.Session.QueryOperationCompleted();
        _ = this.Session.WriteLine( $"{this.EntityName}:latencySetter({this.DefaultsName}.latency)" );
        _ = this.Session.QueryOperationCompleted();
        _ = this.Session.WriteLine( $"{this.EntityName}:medianFilterSizeSetter({this.DefaultsName}.medianFilterSize)" );
        _ = this.Session.QueryOperationCompleted();
        _ = this.Session.WriteLine( $"{this.EntityName}:pointsSetter({this.DefaultsName}.points)" );
        _ = this.Session.QueryOperationCompleted();
    }

    /// <summary> Reads instrument defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ReadInstrumentDefaults()
    {
        base.ReadInstrumentDefaults();
        bool localTryQueryPrint()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.ApertureDefault;
            bool ret = this.Session.TryQueryPrint( 7.4m, ref result, "{0}.aperture", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.ApertureDefault = result;
            return ret;
        }

        if ( !localTryQueryPrint() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient aperture;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint1()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.ApertureMinimum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.minAperture", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.ApertureMinimum = result;
            return ret;
        }

        if ( !localTryQueryPrint1() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient minimum aperture;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint2()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.ApertureMaximum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.maxAperture", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.ApertureMaximum = result;
            return ret;
        }

        if ( !localTryQueryPrint2() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient maximum aperture;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint3()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.CurrentLevelDefault;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.level", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.CurrentLevelDefault = result;
            return ret;
        }

        if ( !localTryQueryPrint3() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient current level;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint4()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.CurrentMinimum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.minCurrent", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.CurrentMinimum = result;
            return ret;
        }

        if ( !localTryQueryPrint4() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient minimum current level;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint5()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.CurrentMaximum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.maxCurrent", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.CurrentMaximum = result;
            return ret;
        }

        if ( !localTryQueryPrint5() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient maximum current level;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint6()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.VoltageChangeDefault;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.limit", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.VoltageChangeDefault = result;
            return ret;
        }

        if ( !localTryQueryPrint6() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient voltage limit;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint7()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.minVoltage", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum = result;
            return ret;
        }

        if ( !localTryQueryPrint7() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient minimum voltage;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint8()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.VoltageMaximum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.maxVoltage", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.VoltageMaximum = result;
            return ret;
        }

        if ( !localTryQueryPrint8() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient maximum voltage;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint9()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.HighLimitDefault;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.highLimit", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.HighLimitDefault = result;
            return ret;
        }

        if ( !localTryQueryPrint9() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient high limit;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint10()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.LowLimitDefault;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.lowLimit", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.LowLimitDefault = result;
            return ret;
        }

        if ( !localTryQueryPrint10() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient low limit;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint11()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.VoltageChangeDefault;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.maxVoltageChange", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.VoltageChangeDefault = result;
            return ret;
        }

        if ( !localTryQueryPrint11() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient maximum voltage change;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint12()
        {
            int result = Properties.Settings.Instance.TtmTraceSettings.MedianFilterLengthDefault;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.medianFilterSize", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.MedianFilterLengthDefault = result;
            return ret;
        }

        if ( !localTryQueryPrint12() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient median filter size;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint13()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalDefault;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.period", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalDefault = result;
            return ret;
        }

        if ( !localTryQueryPrint13() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient period;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint14()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalMinimum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.minPeriod", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalMinimum = result;
            return ret;
        }

        if ( !localTryQueryPrint14() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient minimum period;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint15()
        {
            double result = Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalMaximum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.maxPeriod", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalMaximum = result;
            return ret;
        }

        if ( !localTryQueryPrint15() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient maximum period;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint16()
        {
            int result = Properties.Settings.Instance.TtmTraceSettings.TracePointsDefault;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.points", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.TracePointsDefault = result;
            return ret;
        }

        if ( !localTryQueryPrint16() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient trace points;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint17()
        {
            int result = Properties.Settings.Instance.TtmTraceSettings.TracePointsMinimum;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.minPoints", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.TracePointsMinimum = result;
            return ret;
        }

        if ( !localTryQueryPrint17() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient minimum trace points;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        bool localTryQueryPrint18()
        {
            int result = Properties.Settings.Instance.TtmTraceSettings.TracePointsMaximum;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.maxPoints", this.DefaultsName );
            Properties.Settings.Instance.TtmTraceSettings.TracePointsMaximum = result;
            return ret;
        }

        if ( !localTryQueryPrint18() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient maximum trace points;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );
    }

    /// <summary> Configures the meter for making the thermal transient measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="thermalTransient"> The Thermal Transient element. </param>
    public void Configure( ThermalTransientBase thermalTransient )
    {
        if ( thermalTransient is null ) throw new ArgumentNullException( nameof( thermalTransient ) );
        this.Session.SetLastAction( "configuring Thermal Transient measurement" );
        base.Configure( thermalTransient );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} Allowed Voltage Change to {thermalTransient.AllowedVoltageChange};. " );
        _ = this.ApplyVoltageChange( thermalTransient.AllowedVoltageChange );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.EntityName} Allowed Voltage Change set to {thermalTransient.AllowedVoltageChange};. " );
        this.Session.ThrowDeviceExceptionIfError();
        this.ThermalTransient.CheckThrowUnequalConfiguration( thermalTransient );
    }

    /// <summary> Applies changed meter configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="thermalTransient"> The Thermal Transient element. </param>
    public void ConfigureChanged( ThermalTransientBase thermalTransient )
    {
        if ( thermalTransient is null ) throw new ArgumentNullException( nameof( thermalTransient ) );
        this.Session.SetLastAction( "configuring Thermal Transient measurement" );
        base.ConfigureChanged( thermalTransient );
        if ( !this.ThermalTransient.ConfigurationEquals( thermalTransient ) )
        {
            if ( !this.ThermalTransient.AllowedVoltageChange.Equals( thermalTransient.AllowedVoltageChange ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Allowed Voltage Change to {thermalTransient.AllowedVoltageChange};. " );
                _ = this.ApplyVoltageChange( thermalTransient.AllowedVoltageChange );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.EntityName} Allowed Voltage Change set to {thermalTransient.AllowedVoltageChange};. " );
            }

            if ( !this.ThermalTransient.CurrentLevel.Equals( thermalTransient.CurrentLevel ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Current Level to {thermalTransient.CurrentLevel};. " );
                _ = this.ApplyCurrentLevel( thermalTransient.CurrentLevel );
            }

            if ( !this.ThermalTransient.VoltageLimit.Equals( thermalTransient.VoltageLimit ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Voltage Limit to {thermalTransient.VoltageLimit};. " );
                _ = this.ApplyVoltageLimit( thermalTransient.VoltageLimit );
            }

        }

        this.Session.ThrowDeviceExceptionIfError();
        this.ThermalTransient.CheckThrowUnequalConfiguration( thermalTransient );
    }

    /// <summary> Queries the configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void QueryConfiguration()
    {
        this.Session.SetLastAction( $"reading {this.EntityName} configuration" );
        base.QueryConfiguration();
        _ = this.QueryMedianFilterSize();
        _ = this.QuerySamplingInterval();
        _ = this.QueryTracePoints();
        _ = this.QueryVoltageChange();
        this.Session.ThrowDeviceExceptionIfError();
    }

    #endregion

    #region " measure "

    /// <summary> Measures the Thermal Transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="thermalTransient"> The Thermal Transient element. </param>
    public new void Measure( MeasureBase thermalTransient )
    {
        if ( thermalTransient is null ) throw new ArgumentNullException( nameof( thermalTransient ) );
        if ( this.Session.IsSessionOpen )
        {
            base.Measure( thermalTransient );
            this.ReadThermalTransient( thermalTransient );
        }
        else
        {
            this.EmulateThermalTransient( thermalTransient );
        }
    }

    #endregion

    #region " read "

    /// <summary> Gets or sets the trace ready to read sentinel. </summary>
    /// <value> The trace ready to read. </value>
    public bool NewTraceReadyToRead { get; set; }

    /// <summary>
    /// Reads the thermal transient. Sets the last <see cref="MeasureSubsystemBase.FirmwareReading">reading</see>,
    /// <see cref="MeasureSubsystemBase.FirmwareOutcomeReading">outcome</see> <see cref="MeasureSubsystemBase.FirmwareStatusReading">status</see> and
    /// <see cref="MeasureSubsystemBase.MeasurementAvailable">Measurement available sentinel</see>.
    /// The outcome is left empty if measurements were not made.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    public void ReadThermalTransient()
    {
        this.NewTraceReadyToRead = false;
        this.Session.SetLastAction( "reading outcome" );
        if ( this.QueryFirmwareOutcome() is null )
        {
            this.FirmwareReading = VI.Syntax.Tsp.Lua.NilValue;
            this.FirmwareOutcomeReading = VI.Syntax.Tsp.Lua.NilValue;
            this.FirmwareStatusReading = VI.Syntax.Tsp.Lua.NilValue;
            this.FirmwareOkayReading = VI.Syntax.Tsp.Lua.NilValue;
            throw new InvalidOperationException( "Measurement Not made." );
        }
        else if ( this.QueryFirmwareOkay().GetValueOrDefault( false ) )
        {
            this.Session.SetLastAction( "reading voltage change" );
            _ = this.QueryVoltageChange();
            this.Session.SetLastAction( "reading status" );
            _ = this.QueryFirmwareStatus();
            // this.FirmwareReading = this.Session.QueryPrintStringFormatTrimEnd( 9.6m, "{0}.voltageChange", ThermalTransientEstimatorEntityName );
            this.Session.ThrowDeviceExceptionIfError();
            // this.FirmwareOutcomeReading = "0";
            // this.FirmwareStatusReading = string.Empty;
            this.NewTraceReadyToRead = true;
        }
        else
        {
            // if outcome failed, read and parse the outcome and status.
            this.Session.SetLastAction( "reading outcome" );
            // this.FirmwareOutcomeReading = this.Session.QueryPrintStringFormatTrimEnd( 1, "{0}.outcome", this.EntityName );
            this.Session.ThrowDeviceExceptionIfError();

            this.Session.SetLastAction( "reading status" );
            _ = this.QueryFirmwareStatus();
            // this.FirmwareStatusReading = this.Session.QueryPrintStringFormatTrimEnd( 1, "{0}.status", this.EntityName );
            this.Session.ThrowDeviceExceptionIfError();
            this.Session.SetLastAction( "reading voltage change" );
            _ = this.QueryVoltageChange();
            // this.FirmwareReading = string.Empty;
        }

        this.MeasurementAvailable = true;
    }

    /// <summary> Reads the Thermal Transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="thermalTransient"> The Thermal Transient element. </param>
    public void ReadThermalTransient( MeasureBase thermalTransient )
    {
        if ( thermalTransient is null ) throw new ArgumentNullException( nameof( thermalTransient ) );
        this.Session.SetLastAction( "reading thermal transient" );
        this.ReadThermalTransient();
        this.Session.ThrowDeviceExceptionIfError();
        MeasurementOutcomes measurementOutcome = MeasurementOutcomes.None;
        if ( this.FirmwareOutcomeReading != "0" )
        {
            string details = string.Empty;
            measurementOutcome = this.ParseOutcome( ref details );
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Measurement failed;. Details: {0}", details );
        }

        if ( this.FirmwareReading is null ) throw new InvalidOperationException( $"{nameof( this.FirmwareReading )} is null." );

        this.ThermalTransient.ParseReading( this.FirmwareReading, measurementOutcome );
        thermalTransient.ParseReading( this.FirmwareReading, measurementOutcome );
    }

    /// <summary> Emulates a Thermal Transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="thermalTransient"> The Thermal Transient element. </param>
    public void EmulateThermalTransient( MeasureBase thermalTransient )
    {
        if ( thermalTransient is null ) throw new ArgumentNullException( nameof( thermalTransient ) );
        this.ThermalTransient.EmulateReading();
        this.FirmwareReading = this.ThermalTransient.FirmwareReading;
        this.FirmwareOutcomeReading = this.ThermalTransient.FirmwareOutcomeReading;
        this.FirmwareStatusReading = this.ThermalTransient.FirmwareStatusReading;
        this.FirmwareOkayReading = this.ThermalTransient.FirmwareOkayReading;
        MeasurementOutcomes measurementOutcome = MeasurementOutcomes.None;

        if ( this.FirmwareReading is null ) throw new InvalidOperationException( $"{nameof( this.FirmwareReading )} is null." );
        this.ThermalTransient.ParseReading( this.FirmwareReading, measurementOutcome );
        thermalTransient.ParseReading( this.FirmwareReading, measurementOutcome );
    }

    /// <summary>
    /// Reads the thermal transient trace from the instrument into a
    /// <see cref="LastTrace">comma-separated array of times in milliseconds and values in
    /// milli volts.</see> and
    /// <see cref="LastTimeSeries">collection of times in milliseconds and values in millivolts.</see>
    /// Also updates the outcome and status.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    public void ReadThermalTransientTrace()
    {
        // clear the new trace sentinel
        this.NewTraceReadyToRead = false;
        string delimiter = ",";
        this.LastTimeSeries = [];
        if ( this.FirmwareOutcomeReading != "0" )
        {
            this.LastTrace = string.Empty;
            throw new InvalidOperationException( "Measurement failed." );
        }
        else
        {
            this.Session.SetLastAction( "reading thermal transient trace" );
            System.Text.StringBuilder builder = new();
            _ = this.Session.WriteLine( "{0}:printTimeSeries(4)", this.EntityName );

            _ = this.Session.TraceInformation( message: $"sent query '{this.Session.LastMessageSent}';. " );

            bool done = false;

            while ( !done )
            {
                // check if we have data in the output buffer.
                _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
                ServiceRequests statusByte = this.Session.AwaitErrorOrMessageAvailableBits( TimeSpan.FromMilliseconds( 1d ), 3 );
                this.Session.ThrowDeviceExceptionIfError( statusByte );

                if ( this.Session.IsMessageAvailableBitSet( statusByte ) )
                {
                    string reading = this.Session.ReadLine();
                    if ( builder.Length > 0 )
                    {
                        _ = builder.Append( delimiter );
                    }

                    _ = builder.Append( reading );
                    string[] xy = reading.Split( ',' );
                    if ( !float.TryParse( xy[0].Trim(), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float x ) )
                    {
                        x = -1;
                    }

                    if ( !float.TryParse( xy[1].Trim(), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out float y ) )
                    {
                        y = -1;
                    }

                    Std.Cartesian.CartesianPoint<double> point = new( x, y );
                    this.LastTimeSeries.Add( point );
                }
                else

                    // if we have no value we are done
                    done = true;
            }

            this.LastTrace = builder.ToString();
        }

        this.NotifyPropertyChanged();
    }

    #endregion

    #region " estimate "

    /// <summary> Gets or sets the model. </summary>
    /// <value> The model. </value>
    public Numerical.Optima.PulseResponseFunction? Model { get; private set; }

    /// <summary> The simplex. </summary>
    private Numerical.Optima.Simplex? _simplex;

    /// <summary> Model transient response. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    ///                                              null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="thermalTransient"> The thermal transient element. </param>
    public void ModelTransientResponse( ThermalTransient thermalTransient )
    {
        if ( thermalTransient is null )
        {
            throw new ArgumentNullException( nameof( thermalTransient ) );
        }
        else if ( this.LastTimeSeries is null )
        {
            throw new InvalidOperationException( "Last time series not read" );
        }
        else if ( this.LastTimeSeries.Count == 0 )
        {
            throw new InvalidOperationException( "Time series has no values" );
        }
        else if ( this.LastTimeSeries.Count <= 10 )
        {
            throw new InvalidOperationException( "Time series has less than 10 values" );
        }

        double voltageScale = 1000d; // scale to millivolts
        double timeScale = 1000d; // scale to milliseconds
        double flatnessPrecisionFactor = 0.0001d;
        double pulseDuration = timeScale * this.PulseDuration.TotalSeconds;
        double estimatedTau = timeScale * 0.5d * this.PulseDuration.TotalSeconds;
        double estimatedAsymptote = voltageScale * this.ThermalTransient.HighLimit;
        double[] voltageRange = [0.5d * estimatedAsymptote, 2d * estimatedAsymptote];
        double voltagePrecision = flatnessPrecisionFactor * estimatedAsymptote;
        double[] negativeInverseTauRange = [-1 / (0.5d * estimatedTau), -1 / (2d * estimatedTau)];
        double inverseTauPrecision = flatnessPrecisionFactor / estimatedTau;
        int dimension = 2;
        int maximumIterations = 100;
        double expectedDeviation = 0.05d * estimatedAsymptote;
        double expectedMaximumSSQ = this.ThermalTransient.TracePoints * expectedDeviation * expectedDeviation;
        double objectivePrecisionFactor = 0.0001d;
        double objectivePrecision = objectivePrecisionFactor * expectedMaximumSSQ;
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Instantiating Simplex" );
        this._simplex = new Numerical.Optima.Simplex( "Exponent", dimension, [voltageRange[0], negativeInverseTauRange[0]], [voltageRange[1], negativeInverseTauRange[1]], maximumIterations, [voltagePrecision, inverseTauPrecision], objectivePrecision );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Instantiating Thermal Transient model" );
        this.Model = new Numerical.Optima.PulseResponseFunction( this.LastTimeSeries );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Initializing simplex" );
        this._simplex.Initialize( this.Model );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Initial simplex is {this._simplex}" );
        _ = this._simplex.Solve();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Finished in {this._simplex.IterationNumber} Iterations" );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Final simplex is {this._simplex}" );
        this.Model.EvaluateFunctionValues( this._simplex.BestSolution.Values() );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Estimated Asymptote = {this._simplex.BestSolution.Values().ElementAtOrDefault( 0 ):G4} mV" );
        thermalTransient.Asymptote = 0.001d * this._simplex.BestSolution.Values().ElementAtOrDefault( 0 );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Estimated Time Constant = {-1 / this._simplex.BestSolution.Values().ElementAtOrDefault( 1 ):G4} ms" );
        thermalTransient.TimeConstant = -0.001d / this._simplex.BestSolution.Values().ElementAtOrDefault( 1 );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Estimated Thermal Transient Voltage = {this.Model.FunctionValue( this._simplex.BestSolution.Values(), [pulseDuration, 0d] ):G4} mV" );
        thermalTransient.EstimatedVoltage = 0.001d * this.Model.FunctionValue( this._simplex.BestSolution.Values(), [pulseDuration, 0d] );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Correlation Coefficient = {this.Model.EvaluateCorrelationCoefficient():G4}" );
        thermalTransient.CorrelationCoefficient = this.Model.EvaluateCorrelationCoefficient();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Standard Error = {this.Model.EvaluateStandardError( this._simplex.BestSolution.Objective ):G4} mV" );
        thermalTransient.StandardError = 0.001d * this.Model.EvaluateStandardError( this._simplex.BestSolution.Objective );
        thermalTransient.Iterations = this._simplex.IterationNumber;
        thermalTransient.OptimizationOutcome = this._simplex.Converged()
            ? OptimizationOutcome.Converged
            : this._simplex.Optimized()
                ? OptimizationOutcome.Optimized
                : this._simplex.IterationNumber >= maximumIterations ? OptimizationOutcome.Exhausted : ( OptimizationOutcome? ) OptimizationOutcome.None;

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Converged = {this._simplex.Converged()}" );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Optimized = {this._simplex.Optimized()}" );
    }

    #endregion
}
