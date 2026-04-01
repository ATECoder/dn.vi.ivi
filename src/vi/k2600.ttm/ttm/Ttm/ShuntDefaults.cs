using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Driver Shunt Default Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class ShuntDefaults() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    #region " Shunt Resistance Measurement properties "

    /// <summary>   Gets or sets the Shunt Measurement aperture. </summary>
    /// <value> The Shunt Measurement aperture. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Aperture (0.1)." )]
    public partial double Aperture { get; set; } = 0.1;

    /// <summary>   Gets or sets the Shunt Measurement aperture Maximum. </summary>
    /// <value> The Shunt Measurement aperture Maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Maximum Aperture (5)." )]
    public partial double ApertureMaximum { get; set; } = 5;

    /// <summary>   Gets or sets the Shunt Measurement aperture minimum. </summary>
    /// <value> The Shunt Measurement aperture minimum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Minimum Aperture (0.01)." )]
    public partial double ApertureMinimum { get; set; } = 0.01;

    /// <summary>   Gets or sets the Shunt Measurement Measurement current source current level. </summary>
    /// <value> The Shunt Measurement current level. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Current Source Current Level (0.001)." )]
    public partial double CurrentLevel { get; set; } = 0.001;

    /// <summary>   Gets or sets the Shunt Measurement Minimum Current. </summary>
    /// <value> The Shunt Measurement Minimum Current. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Minimum Current (0.00001)." )]
    public partial double CurrentMinimum { get; set; } = 0.00001;

    /// <summary>   Gets the Shunt Measurement Maximum Current. </summary>
    /// <value> The Shunt Measurement Maximum Current. </value>
    [Description( "Specifies the Shunt Measurement Maximum Current (readonly)." )]
    public double CurrentMaximum => this.CurrentMinimum + this.CurrentRange;

    /// <summary>   Gets or sets the Shunt Measurement current Range. </summary>
    /// <value> The Shunt Measurement current Range. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Current Range (0.01)." )]
    public partial double CurrentRange { get; set; } = 0.01;

    /// <summary>   Gets or sets the Shunt Measurement High limit. </summary>
    /// <value> The Shunt Measurement High limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement High Limit (1250)." )]
    public partial double HighLimit { get; set; } = 1250;

    /// <summary>   Gets or sets the Shunt Measurement low limit. </summary>
    /// <value> The Shunt Measurement low limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Low Limit (1150)." )]
    public partial double LowLimit { get; set; } = 1150;

    /// <summary>   Gets or sets the Shunt Measurement Maximum. </summary>
    /// <value> The Shunt Measurement Maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Maximum (10000)." )]
    public partial double Maximum { get; set; } = 10000;

    /// <summary>   Gets or sets the Shunt Measurement minimum. </summary>
    /// <value> The Shunt Measurement minimum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Minimum (900)." )]
    public partial double Minimum { get; set; } = 900;

    /// <summary>   Gets or sets the Shunt Measurement Current Source Voltage Limit. </summary>
    /// <value> The Shunt Measurement Current Source Voltage Limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Current Source Current Source Voltage Limit (10)." )]
    public partial double VoltageLimit { get; set; } = 10;

    /// <summary>   Gets or sets the Shunt Measurement Maximum Voltage. </summary>
    /// <value> The Shunt Measurement voltage maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Maximum Voltage (100)." )]
    public partial double VoltageMaximum { get; set; } = 100;

    /// <summary>   Gets or sets the Shunt Measurement Minimum Voltage. </summary>
    /// <value> The Shunt Measurement Minimum Voltage. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Minimum Voltage (11)." )]
    public partial double VoltageMinimum { get; set; } = 1;

    #endregion
}
