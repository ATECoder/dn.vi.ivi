using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Trace Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class TtmTraceSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmTraceSettings() { }

    #endregion

    #region " exists "

    private bool _exists;

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get => this._exists;
        set => _ = this.SetProperty( ref this._exists, value );
    }

    #endregion

    #region " thermal transient properties "

    private double _aperture = 0.004;

    /// <summary>   Gets or sets the Thermal Transient aperture. </summary>
    /// <value> The Thermal Transient aperture. </value>
    [Description( "Thermal Transient Aperture {0.004)." )]
    public double Aperture
    {
        get => this._aperture;
        set => this.SetProperty( ref this._aperture, value );
    }

    private double _apertureDefault = 0.004;

    /// <summary>   Gets or sets the Thermal Transient aperture default. </summary>
    /// <value> The Thermal Transient aperture default. </value>
    [Description( "Thermal Transient Default Aperture {0.004)." )]
    public double ApertureDefault
    {
        get => this._apertureDefault;
        set => this.SetProperty( ref this._apertureDefault, value );
    }

    private double _apertureMaximum = 0.01;

    /// <summary>   Gets or sets the Thermal Transient aperture Maximum. </summary>
    /// <value> The Thermal Transient aperture Maximum. </value>
    [Description( "Thermal Transient Maximum Aperture {0.01)." )]
    public double ApertureMaximum
    {
        get => this._apertureMaximum;
        set => this.SetProperty( ref this._apertureMaximum, value );
    }

    private double _apertureMinimum = 0.001;

    /// <summary>   Gets or sets the Thermal Transient aperture minimum. </summary>
    /// <value> The Thermal Transient aperture minimum. </value>
    [Description( "Thermal Transient Minimum Aperture {0.001)." )]
    public double ApertureMinimum
    {
        get => this._apertureMinimum;
        set => this.SetProperty( ref this._apertureMinimum, value );
    }

    private double _currentLevel = 0.27;

    /// <summary>   Gets or sets the Thermal Transient current level. </summary>
    /// <value> The Thermal Transient current level. </value>
    [Description( "Thermal Transient Current Level {0.27)." )]
    public double CurrentLevel
    {
        get => this._currentLevel;
        set => this.SetProperty( ref this._currentLevel, value );
    }

    private double _currentLevelDefault = 0.27;

    /// <summary>   Gets or sets the Thermal Transient current level default. </summary>
    /// <value> The Thermal Transient current level default. </value>
    [Description( "Thermal Transient Default Current Level {0.27)." )]
    public double CurrentLevelDefault
    {
        get => this._currentLevelDefault;
        set => this.SetProperty( ref this._currentLevelDefault, value );
    }

    private double _currentMaximum = 0.999;

    /// <summary>   Gets or sets the Thermal Transient current Maximum. </summary>
    /// <value> The Thermal Transient current Maximum. </value>
    [Description( "Thermal Transient Maximum Current {0.999)." )]
    public double CurrentMaximum
    {
        get => this._currentMaximum;
        set => this.SetProperty( ref this._currentMaximum, value );
    }

    private double _currentMinimum = 0.01;

    /// <summary>   Gets or sets the Thermal Transient current minimum. </summary>
    /// <value> The Thermal Transient current minimum. </value>
    [Description( "Thermal Transient Minimum Current {0.01)." )]
    public double CurrentMinimum
    {
        get => this._currentMinimum;
        set => this.SetProperty( ref this._currentMinimum, value );
    }

    private double _duration = 0.01;

    /// <summary>   Gets or sets the Thermal Transient Duration. </summary>
    /// <value> The Thermal Transient Duration. </value>
    [Description( "Thermal Transient Duration {0.01)." )]
    public double Duration
    {
        get => this._duration;
        set => this.SetProperty( ref this._duration, value );
    }

    private double _durationDefault = 0.01;

    /// <summary>   Gets or sets the Thermal Transient Duration default. </summary>
    /// <value> The Thermal Transient Duration default. </value>
    [Description( "Thermal Transient Default Duration {0.01)." )]
    public double DurationDefault
    {
        get => this._durationDefault;
        set => this.SetProperty( ref this._durationDefault, value );
    }

    private double _durationMaximum = 0.1;

    /// <summary>   Gets or sets the Thermal Transient Duration Maximum. </summary>
    /// <value> The Thermal Transient Duration Maximum. </value>
    [Description( "Thermal Transient Duration Maximum Value {0.01)." )]
    public double DurationMaximum
    {
        get => this._durationMaximum;
        set => this.SetProperty( ref this._durationMaximum, value );
    }

    private double _durationMinimum = 0.01;

    /// <summary>   Gets or sets the Thermal Transient Duration minimum. </summary>
    /// <value> The Thermal Transient Duration minimum. </value>
    [Description( "Thermal Transient Duration Minimum Value {0.01)." )]
    public double DurationMinimum
    {
        get => this._durationMinimum;
        set => this.SetProperty( ref this._durationMinimum, value );
    }

    private double _highLimit = 0.017;

    /// <summary>   Gets or sets the Thermal Transient voltage change high limit . </summary>
    /// <value> The Thermal Transient voltage change high limit . </value>
    [Description( "Thermal Transient Voltage Change High Limit {0.017)." )]
    public double HighLimit
    {
        get => this._highLimit;
        set => this.SetProperty( ref this._highLimit, value );
    }

    private double _highLimitDefault = 0.017;

    /// <summary>   Gets or sets the Thermal Transient High limit default. </summary>
    /// <value> The Thermal Transient High limit default. </value>
    [Description( "Thermal Transient Default Voltage Change High Limit {0.017)." )]
    public double HighLimitDefault
    {
        get => this._highLimitDefault;
        set => this.SetProperty( ref this._highLimitDefault, value );
    }

    private double _latency = 0.00035;

    /// <summary>   Gets or sets the Thermal Transient Latency. </summary>
    /// <remarks> This is the estimated time from the onset of the current pulse to the onset of data acquisition. </remarks>
    /// <value> The Thermal Transient Latency. </value>
    [Description( "Thermal Transient Latency {0.00035)." )]
    public double Latency
    {
        get => this._latency;
        set => this.SetProperty( ref this._latency, value );
    }

    private double _latencyDefault = 0.00035;

    /// <summary>   Gets or sets the Thermal Transient Latency default. </summary>
    /// <value> The Thermal Transient Latency default. </value>
    [Description( "Thermal Transient Default Latency {0.00035)." )]
    public double LatencyDefault
    {
        get => this._latencyDefault;
        set => this.SetProperty( ref this._latencyDefault, value );
    }

    private double _lowLimit = 0.006;

    /// <summary>   Gets or sets the Thermal Transient low limit . </summary>
    /// <value> The Thermal Transient low limit . </value>
    [Description( "Thermal Voltage Change Default Low Limit {0.006)." )]
    public double LowLimit
    {
        get => this._lowLimit;
        set => this.SetProperty( ref this._lowLimit, value );
    }

    private double _lowLimitDefault = 0.006;

    /// <summary>   Gets or sets the Thermal Transient low limit default. </summary>
    /// <value> The Thermal Transient low limit default. </value>
    [Description( "Thermal Transient Voltage Change Default Low Limit {0.006)." )]
    public double LowLimitDefault
    {
        get => this._lowLimitDefault;
        set => this.SetProperty( ref this._lowLimitDefault, value );
    }

    private int _medianFilterLength = 3;

    /// <summary>   Gets or sets the Thermal Transient Median Filter Length. </summary>
    /// <value> The Thermal Transient Median Filter Length. </value>
    [Description( "Thermal Transient Median Filter Length {3)." )]
    public int MedianFilterLength
    {
        get => this._medianFilterLength;
        set => this.SetProperty( ref this._medianFilterLength, value );
    }

    private int _medianFilterLengthDefault = 3;

    /// <summary>   Gets or sets the Thermal Transient Trace Median Filter Length default. </summary>
    /// <value> The Thermal Transient Median Filter Length default. </value>
    [Description( "Thermal Transient Default Median Filter Length {3)." )]
    public int MedianFilterLengthDefault
    {
        get => this._medianFilterLengthDefault;
        set => this.SetProperty( ref this._medianFilterLengthDefault, value );
    }

    private int _medianFilterLengthMaximum = 9;

    /// <summary>   Gets or sets the Thermal Transient Median Filter Length Maximum. </summary>
    /// <value> The Thermal Transient Median Filter Length Maximum. </value>
    [Description( "Thermal Transient Maximum Median Filter Length {9)." )]
    public int MedianFilterLengthMaximum
    {
        get => this._medianFilterLengthMaximum;
        set => this.SetProperty( ref this._medianFilterLengthMaximum, value );
    }

    private int _medianFilterLengthMinimum = 3;

    /// <summary>   Gets or sets the Thermal Transient Median Filter Length minimum. </summary>
    /// <value> The Thermal Transient Median Filter Length minimum. </value>
    [Description( "Thermal Transient Minimum Median Filter Length {3)." )]
    public int MedianFilterLengthMinimum
    {
        get => this._medianFilterLengthMinimum;
        set => this.SetProperty( ref this._medianFilterLengthMinimum, value );
    }

    private double _samplingInterval = 0.0001;

    /// <summary>   Gets or sets the Thermal Transient Sampling Interval. </summary>
    /// <value> The Thermal Transient Sampling Interval. </value>
    [Description( "Thermal Transient Maximum Sampling Interval {0.0001)." )]
    public double SamplingInterval
    {
        get => this._samplingInterval;
        set => this.SetProperty( ref this._samplingInterval, value );
    }

    private double _samplingIntervalDefault = 0.0001;

    /// <summary>   Gets or sets the Thermal Transient Sampling Interval default. </summary>
    /// <value> The Thermal Transient Sampling Interval default. </value>
    [Description( "Thermal Transient Default Sampling Interval {0.0001)." )]
    public double SamplingIntervalDefault
    {
        get => this._samplingIntervalDefault;
        set => this.SetProperty( ref this._samplingIntervalDefault, value );
    }

    private double _samplingIntervalMaximum = 0.001;

    /// <summary>   Gets or sets the Thermal Transient Sampling Interval Maximum. </summary>
    /// <value> The Thermal Transient Sampling Interval Maximum. </value>
    [Description( "Thermal Transient Maximum Sampling Interval {0.001)." )]
    public double SamplingIntervalMaximum
    {
        get => this._samplingIntervalMaximum;
        set => this.SetProperty( ref this._samplingIntervalMaximum, value );
    }

    private double _samplingIntervalMinimum = 0.00008;

    /// <summary>   Gets or sets the Thermal Transient SamplingInterval minimum. </summary>
    /// <value> The Thermal Transient SamplingInterval minimum. </value>
    [Description( "Thermal Transient Minimum Sampling Interval {0.00008)." )]
    public double SamplingIntervalMinimum
    {
        get => this._samplingIntervalMinimum;
        set => this.SetProperty( ref this._samplingIntervalMinimum, value );
    }

    private int _tracePoints = 100;

    /// <summary>   Gets or sets the Thermal Transient Trace Points. </summary>
    /// <value> The Thermal Transient Trace Points. </value>
    [Description( "Thermal Transient Trace Points {100)." )]
    public int TracePoints
    {
        get => this._tracePoints;
        set => this.SetProperty( ref this._tracePoints, value );
    }

    private int _tracePointsDefault = 100;

    /// <summary>   Gets or sets the Thermal Transient Trace Points default. </summary>
    /// <value> The Thermal Transient Trace Points default. </value>
    [Description( "Thermal Transient Default Trace Points {100)." )]
    public int TracePointsDefault
    {
        get => this._tracePointsDefault;
        set => this.SetProperty( ref this._tracePointsDefault, value );
    }

    private int _tracePointsMaximum = 10000;

    /// <summary>   Gets or sets the Thermal Transient Median Filter Length Maximum. </summary>
    /// <value> The Thermal Transient Trace Points Maximum. </value>
    [Description( "Thermal Transient Maximum Trace Points {10000)." )]
    public int TracePointsMaximum
    {
        get => this._tracePointsMaximum;
        set => this.SetProperty( ref this._tracePointsMaximum, value );
    }

    private int _tracePointsMinimum = 10;

    /// <summary>   Gets or sets the Thermal Transient Trace Points minimum. </summary>
    /// <value> The Thermal Transient Trace Points minimum. </value>
    [Description( "Thermal Transient Minimum Trace Points {10)." )]
    public int TracePointsMinimum
    {
        get => this._tracePointsMinimum;
        set => this.SetProperty( ref this._tracePointsMinimum, value );
    }

    private double _voltageChange = 0.099;

    /// <summary>   Gets or sets the Thermal Transient voltage Change . </summary>
    /// <value> The Thermal Transient voltage Change . </value>
    [Description( "Thermal Transient Voltage Change {0.099)." )]
    public double VoltageChange
    {
        get => this._voltageChange;
        set => this.SetProperty( ref this._voltageChange, value );
    }

    private double _voltageChangeDefault = 0.099;

    /// <summary>   Gets or sets the Thermal Transient voltage Change default. </summary>
    /// <value> The Thermal Transient voltage Change default. </value>
    [Description( "Thermal Transient Default Voltage Change {0.099)." )]
    public double VoltageChangeDefault
    {
        get => this._voltageChangeDefault;
        set => this.SetProperty( ref this._voltageChangeDefault, value );
    }

    private double _voltageChangeMaximum = 0.099;

    /// <summary>   Gets or sets the Thermal Transient voltage Change Maximum. </summary>
    /// <value> The Thermal Transient voltage Change Maximum. </value>
    [Description( "Thermal Transient Maximum Voltage Change {0.099)." )]
    public double VoltageChangeMaximum
    {
        get => this._voltageChangeMaximum;
        set => this.SetProperty( ref this._voltageChangeMaximum, value );
    }

    private double _voltageChangeMinimum = 0.001;

    /// <summary>   Gets or sets the Thermal Transient voltage Change Minimum. </summary>
    /// <value> The Thermal Transient voltage Change Minimum. </value>
    [Description( "Thermal Transient Minimum Voltage Change {0.001)." )]
    public double VoltageChangeMinimum
    {
        get => this._voltageChangeMinimum;
        set => this.SetProperty( ref this._voltageChangeMinimum, value );
    }

    private double _voltageLimit = 0.99;

    /// <summary>   Gets or sets the Thermal Transient voltage limit . </summary>
    /// <value> The Thermal Transient voltage limit . </value>
    [Description( "Thermal Transient Voltage Limit {0.99)." )]
    public double VoltageLimit
    {
        get => this._voltageLimit;
        set => this.SetProperty( ref this._voltageLimit, value );
    }

    private double _voltageLimitDefault = 0.99;

    /// <summary>   Gets or sets the Thermal Transient voltage limit default. </summary>
    /// <value> The Thermal Transient voltage limit default. </value>
    [Description( "Thermal Transient Default Voltage Limit {0.99)." )]
    public double VoltageLimitDefault
    {
        get => this._voltageLimitDefault;
        set => this.SetProperty( ref this._voltageLimitDefault, value );
    }

    private double _voltageMaximum = 9.99;

    /// <summary>   Gets or sets the Thermal Transient voltage maximum. </summary>
    /// <value> The Thermal Transient voltage maximum. </value>
    [Description( "Thermal Transient Maximum Voltage {9.99)." )]
    public double VoltageMaximum
    {
        get => this._voltageMaximum;
        set => this.SetProperty( ref this._voltageMaximum, value );
    }

    private double _voltageMinimum = 0.01;

    /// <summary>   Gets or sets the Thermal Transient voltage Minimum. </summary>
    /// <value> The Thermal Transient voltage Minimum. </value>
    [Description( "Thermal Transient Minimum Voltage {0.01)." )]
    public double VoltageMinimum
    {
        get => this._voltageMinimum;
        set => this.SetProperty( ref this._voltageMinimum, value );
    }

    #endregion

}
