using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Driver Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class TtmShuntSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmShuntSettings() { }

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

    #region " Shunt Resistance Measurement properties "

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement aperture. </summary>
    /// <value> The Shunt MeasuredValue Measurement aperture. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Aperture (0.1)." )]
    public partial double Aperture { get; set; } = 0.1;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement default aperture. </summary>
    /// <value> The Shunt MeasuredValue Measurement aperture default. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Default Aperture (0.1)." )]
    public partial double ApertureDefault { get; set; } = 0.1;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement aperture Maximum. </summary>
    /// <value> The Shunt MeasuredValue Measurement aperture Maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Maximum Aperture (5)." )]
    public partial double ApertureMaximum { get; set; } = 5;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement aperture minimum. </summary>
    /// <value> The Shunt MeasuredValue Measurement aperture minimum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Minimum Aperture (0.01)." )]
    public partial double ApertureMinimum { get; set; } = 0.01;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Measurement current source current level. </summary>
    /// <value> The Shunt MeasuredValue Measurement current level. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Source Current Level (0.001)." )]
    public partial double CurrentLevel { get; set; } = 0.001;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement current source default current level. </summary>
    /// <value> The Shunt MeasuredValue Measurement current level default. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Source Default Current Level (0.001)." )]
    public partial double CurrentLevelDefault { get; set; } = 0.001;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Minimum Current. </summary>
    /// <value> The Shunt MeasuredValue Measurement Minimum Current. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Minimum Current (0.00001)." )]
    public partial double CurrentMinimum { get; set; } = 0.00001;

    /// <summary>   Gets the Shunt MeasuredValue Measurement Maximum Current. </summary>
    /// <value> The Shunt MeasuredValue Measurement Maximum Current. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Maximum Current (readonly)." )]
    public double CurrentMaximum => this.CurrentMinimum + this.CurrentRange;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement current Range. </summary>
    /// <value> The Shunt MeasuredValue Measurement current Range. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Range (0.01)." )]
    public partial double CurrentRange { get; set; } = 0.01;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Default Current Range. </summary>
    /// <value> The Shunt MeasuredValue Measurement Default Current Range. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Default Current Range (0.01)." )]
    public partial double CurrentRangeDefault { get; set; } = 0.01;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement High limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement High limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement High Limit (1250)." )]
    public partial double HighLimit { get; set; } = 1250;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Default High limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement Default High limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Default High Limit (1250)." )]
    public partial double HighLimitDefault { get; set; } = 1250;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement low limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement low limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Low Limit (1150)." )]
    public partial double LowLimit { get; set; } = 1150;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Default Low Limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement Default Low Limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Default Low Limit (1150)." )]
    public partial double LowLimitDefault { get; set; } = 1150;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Maximum. </summary>
    /// <value> The Shunt MeasuredValue Measurement Maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Maximum (10000)." )]
    public partial double Maximum { get; set; } = 10000;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement minimum. </summary>
    /// <value> The Shunt MeasuredValue Measurement minimum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Minimum (900)." )]
    public partial double Minimum { get; set; } = 900;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Current Source Voltage Limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement Current Source Voltage Limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Source Current Source Voltage Limit (10)." )]
    public partial double VoltageLimit { get; set; } = 10;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Current Source Default Voltage Limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement Current Source Default Voltage Limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Source Current Source Default Voltage Limit (10)." )]
    public partial double VoltageLimitDefault { get; set; } = 10;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Maximum Voltage. </summary>
    /// <value> The Shunt MeasuredValue Measurement voltage maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Maximum Voltage (100)." )]
    public partial double VoltageMaximum { get; set; } = 100;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Minimum Voltage. </summary>
    /// <value> The Shunt MeasuredValue Measurement Minimum Voltage. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt MeasuredValue Measurement Minimum Voltage (11)." )]
    public partial double VoltageMinimum { get; set; } = 1;

    #endregion

}
