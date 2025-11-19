using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Trace Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class TtmTraceSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmTraceSettings() { }

    #endregion

    #region " exists "

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #region " thermal transient properties "

    /// <summary>   Gets or sets the Thermal Transient aperture. </summary>
    /// <value> The Thermal Transient aperture. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Aperture {0.004)." )]
    public partial double Aperture { get; set; } = 0.004;

    /// <summary>   Gets or sets the Thermal Transient aperture default. </summary>
    /// <value> The Thermal Transient aperture default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Aperture {0.004)." )]
    public partial double ApertureDefault { get; set; } = 0.004;

    /// <summary>   Gets or sets the Thermal Transient aperture Maximum. </summary>
    /// <value> The Thermal Transient aperture Maximum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Maximum Aperture {0.01)." )]
    public partial double ApertureMaximum { get; set; } = 0.01;

    /// <summary>   Gets or sets the Thermal Transient aperture minimum. </summary>
    /// <value> The Thermal Transient aperture minimum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Minimum Aperture {0.001)." )]
    public partial double ApertureMinimum { get; set; } = 0.001;

    /// <summary>   Gets or sets the Thermal Transient current level. </summary>
    /// <value> The Thermal Transient current level. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Current Level {0.27)." )]
    public partial double CurrentLevel { get; set; } = 0.27;

    /// <summary>   Gets or sets the Thermal Transient current level default. </summary>
    /// <value> The Thermal Transient current level default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Current Level {0.27)." )]
    public partial double CurrentLevelDefault { get; set; } = 0.27;

    /// <summary>   Gets or sets the Thermal Transient current Maximum. </summary>
    /// <value> The Thermal Transient current Maximum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Maximum Current {0.999)." )]
    public partial double CurrentMaximum { get; set; } = 0.999;

    /// <summary>   Gets or sets the Thermal Transient current minimum. </summary>
    /// <value> The Thermal Transient current minimum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Minimum Current {0.01)." )]
    public partial double CurrentMinimum { get; set; } = 0.01;

    /// <summary>   Gets or sets the Thermal Transient Duration. </summary>
    /// <value> The Thermal Transient Duration. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Duration {0.01)." )]
    public partial double Duration { get; set; } = 0.01;

    /// <summary>   Gets or sets the Thermal Transient Duration default. </summary>
    /// <value> The Thermal Transient Duration default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Duration {0.01)." )]
    public partial double DurationDefault { get; set; } = 0.01;

    /// <summary>   Gets or sets the Thermal Transient Duration Maximum. </summary>
    /// <value> The Thermal Transient Duration Maximum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Duration Maximum Value {0.01)." )]
    public partial double DurationMaximum { get; set; } = 0.1;

    /// <summary>   Gets or sets the Thermal Transient Duration minimum. </summary>
    /// <value> The Thermal Transient Duration minimum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Duration Minimum Value {0.01)." )]
    public partial double DurationMinimum { get; set; } = 0.01;

    /// <summary>   Gets or sets the Thermal Transient voltage change high limit . </summary>
    /// <value> The Thermal Transient voltage change high limit . </value>
    [ObservableProperty]
    [Description( "Thermal Transient Voltage Change High Limit {0.017)." )]
    public partial double HighLimit { get; set; } = 0.017;

    /// <summary>   Gets or sets the Thermal Transient High limit default. </summary>
    /// <value> The Thermal Transient High limit default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Voltage Change High Limit {0.017)." )]
    public partial double HighLimitDefault { get; set; } = 0.017;

    /// <summary>   Gets or sets the Thermal Transient Latency. </summary>
    /// <remarks> This is the estimated time from the onset of the current pulse to the onset of data acquisition. </remarks>
    /// <value> The Thermal Transient Latency. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Latency {0.00035)." )]
    public partial double Latency { get; set; } = 0.00035;

    /// <summary>   Gets or sets the Thermal Transient Latency default. </summary>
    /// <value> The Thermal Transient Latency default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Latency {0.00035)." )]
    public partial double LatencyDefault { get; set; } = 0.00035;

    /// <summary>   Gets or sets the Thermal Transient low limit . </summary>
    /// <value> The Thermal Transient low limit . </value>
    [ObservableProperty]
    [Description( "Thermal Voltage Change Default Low Limit {0.006)." )]
    public partial double LowLimit { get; set; } = 0.006;

    /// <summary>   Gets or sets the Thermal Transient low limit default. </summary>
    /// <value> The Thermal Transient low limit default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Voltage Change Default Low Limit {0.006)." )]
    public partial double LowLimitDefault { get; set; } = 0.006;

    /// <summary>   Gets or sets the Thermal Transient Median Filter Length. </summary>
    /// <value> The Thermal Transient Median Filter Length. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Median Filter Length {3)." )]
    public partial int MedianFilterLength { get; set; } = 3;

    /// <summary>   Gets or sets the Thermal Transient Trace Median Filter Length default. </summary>
    /// <value> The Thermal Transient Median Filter Length default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Median Filter Length {3)." )]
    public partial int MedianFilterLengthDefault { get; set; } = 3;

    /// <summary>   Gets or sets the Thermal Transient Median Filter Length Maximum. </summary>
    /// <value> The Thermal Transient Median Filter Length Maximum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Maximum Median Filter Length {9)." )]
    public partial int MedianFilterLengthMaximum { get; set; } = 9;

    /// <summary>   Gets or sets the Thermal Transient Median Filter Length minimum. </summary>
    /// <value> The Thermal Transient Median Filter Length minimum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Minimum Median Filter Length {3)." )]
    public partial int MedianFilterLengthMinimum { get; set; } = 3;

    /// <summary>   Gets or sets the Thermal Transient Sampling Interval. </summary>
    /// <value> The Thermal Transient Sampling Interval. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Maximum Sampling Interval {0.0001)." )]
    public partial double SamplingInterval { get; set; } = 0.0001;

    /// <summary>   Gets or sets the Thermal Transient Sampling Interval default. </summary>
    /// <value> The Thermal Transient Sampling Interval default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Sampling Interval {0.0001)." )]
    public partial double SamplingIntervalDefault { get; set; } = 0.0001;

    /// <summary>   Gets or sets the Thermal Transient Sampling Interval Maximum. </summary>
    /// <value> The Thermal Transient Sampling Interval Maximum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Maximum Sampling Interval {0.001)." )]
    public partial double SamplingIntervalMaximum { get; set; } = 0.001;

    /// <summary>   Gets or sets the Thermal Transient SamplingInterval minimum. </summary>
    /// <value> The Thermal Transient SamplingInterval minimum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Minimum Sampling Interval {0.00008)." )]
    public partial double SamplingIntervalMinimum { get; set; } = 0.00008;

    /// <summary>   Gets or sets the Thermal Transient Trace Points. </summary>
    /// <value> The Thermal Transient Trace Points. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Trace Points {100)." )]
    public partial int TracePoints { get; set; } = 100;

    /// <summary>   Gets or sets the Thermal Transient Trace Points default. </summary>
    /// <value> The Thermal Transient Trace Points default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Trace Points {100)." )]
    public partial int TracePointsDefault { get; set; } = 100;

    /// <summary>   Gets or sets the Thermal Transient Median Filter Length Maximum. </summary>
    /// <value> The Thermal Transient Trace Points Maximum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Maximum Trace Points {10000)." )]
    public partial int TracePointsMaximum { get; set; } = 10000;

    /// <summary>   Gets or sets the Thermal Transient Trace Points minimum. </summary>
    /// <value> The Thermal Transient Trace Points minimum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Minimum Trace Points {10)." )]
    public partial int TracePointsMinimum { get; set; } = 10;

    /// <summary>   Gets or sets the Thermal Transient voltage Change . </summary>
    /// <value> The Thermal Transient voltage Change . </value>
    [ObservableProperty]
    [Description( "Thermal Transient Voltage Change {0.099)." )]
    public partial double VoltageChange { get; set; } = 0.099;

    /// <summary>   Gets or sets the Thermal Transient voltage Change default. </summary>
    /// <value> The Thermal Transient voltage Change default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Voltage Change {0.099)." )]
    public partial double VoltageChangeDefault { get; set; } = 0.099;

    /// <summary>   Gets or sets the Thermal Transient voltage Change Maximum. </summary>
    /// <value> The Thermal Transient voltage Change Maximum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Maximum Voltage Change {0.099)." )]
    public partial double VoltageChangeMaximum { get; set; } = 0.099;

    /// <summary>   Gets or sets the Thermal Transient voltage Change Minimum. </summary>
    /// <value> The Thermal Transient voltage Change Minimum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Minimum Voltage Change {0.001)." )]
    public partial double VoltageChangeMinimum { get; set; } = 0.001;

    /// <summary>   Gets or sets the Thermal Transient voltage limit . </summary>
    /// <value> The Thermal Transient voltage limit . </value>
    [ObservableProperty]
    [Description( "Thermal Transient Voltage Limit {0.99)." )]
    public partial double VoltageLimit { get; set; } = 0.99;

    /// <summary>   Gets or sets the Thermal Transient voltage limit default. </summary>
    /// <value> The Thermal Transient voltage limit default. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Default Voltage Limit {0.99)." )]
    public partial double VoltageLimitDefault { get; set; } = 0.99;

    /// <summary>   Gets or sets the Thermal Transient voltage maximum. </summary>
    /// <value> The Thermal Transient voltage maximum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Maximum Voltage {9.99)." )]
    public partial double VoltageMaximum { get; set; } = 9.99;

    /// <summary>   Gets or sets the Thermal Transient voltage Minimum. </summary>
    /// <value> The Thermal Transient voltage Minimum. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Minimum Voltage {0.01)." )]
    public partial double VoltageMinimum { get; set; } = 0.01;

    #endregion

}
