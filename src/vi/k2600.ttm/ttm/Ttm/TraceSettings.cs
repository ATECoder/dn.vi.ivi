using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Trace Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class TraceSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
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

    /// <summary>   Gets or sets the Thermal Transient current level. </summary>
    /// <value> The Thermal Transient current level. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Current Level {0.27)." )]
    public partial double CurrentLevel { get; set; } = 0.27;

    /// <summary>   Gets or sets the Thermal Transient Duration. </summary>
    /// <value> The Thermal Transient Duration. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Duration {0.01)." )]
    public partial double Duration { get; set; } = 0.01;

    /// <summary>   Gets or sets the Thermal Transient voltage change high limit . </summary>
    /// <value> The Thermal Transient voltage change high limit . </value>
    [ObservableProperty]
    [Description( "Thermal Transient Voltage Change High Limit {0.017)." )]
    public partial double HighLimit { get; set; } = 0.017;

    /// <summary>   Gets or sets the Thermal Transient Latency. </summary>
    /// <remarks> This is the estimated time from the onset of the current pulse to the onset of data acquisition. </remarks>
    /// <value> The Thermal Transient Latency. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Latency {0.00035)." )]
    public partial double Latency { get; set; } = 0.00035;

    /// <summary>   Gets or sets the Thermal Transient low limit . </summary>
    /// <value> The Thermal Transient low limit . </value>
    [ObservableProperty]
    [Description( "Thermal Voltage Change Default Low Limit {0.006)." )]
    public partial double LowLimit { get; set; } = 0.006;

    /// <summary>   Gets or sets the Thermal Transient Median Filter Length. </summary>
    /// <value> The Thermal Transient Median Filter Length. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Median Filter Length {3)." )]
    public partial int MedianFilterLength { get; set; } = 3;

    /// <summary>   Gets or sets the Thermal Transient Sampling Interval. </summary>
    /// <value> The Thermal Transient Sampling Interval. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Maximum Sampling Interval {0.0001)." )]
    public partial double SamplingInterval { get; set; } = 0.0001;

    /// <summary>   Gets or sets the Thermal Transient Trace Points. </summary>
    /// <value> The Thermal Transient Trace Points. </value>
    [ObservableProperty]
    [Description( "Thermal Transient Trace Points {100)." )]
    public partial int TracePoints { get; set; } = 100;

    /// <summary>   Gets or sets the Thermal Transient voltage Change . </summary>
    /// <value> The Thermal Transient voltage Change . </value>
    [ObservableProperty]
    [Description( "Thermal Transient Voltage Change {0.099)." )]
    public partial double VoltageChange { get; set; } = 0.099;

    /// <summary>   Gets or sets the Thermal Transient voltage limit . </summary>
    /// <value> The Thermal Transient voltage limit . </value>
    [ObservableProperty]
    [Description( "Thermal Transient Voltage Limit {0.99)." )]
    public partial double VoltageLimit { get; set; } = 0.99;

    #endregion
}
