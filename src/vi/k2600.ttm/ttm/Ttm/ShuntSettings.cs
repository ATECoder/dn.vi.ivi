using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Driver Shunt Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class ShuntSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    /// <summary>   Gets or sets the Shunt Measurement Measurement current source current level. </summary>
    /// <value> The Shunt Measurement current level. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Current Source Current Level (0.001)." )]
    public partial double CurrentLevel { get; set; } = 0.001;

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

    /// <summary>   Gets or sets the Shunt Measurement Current Source Voltage Limit. </summary>
    /// <value> The Shunt Measurement Current Source Voltage Limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Shunt Measurement Current Source Current Source Voltage Limit (10)." )]
    public partial double VoltageLimit { get; set; } = 10;

    #endregion
}
