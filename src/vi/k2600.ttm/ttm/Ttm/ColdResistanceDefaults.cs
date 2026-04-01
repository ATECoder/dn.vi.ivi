using System.ComponentModel;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Driver Cold Resistance Default Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class ColdResistanceDefaults() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    #region " settings "

    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Current Source Source Measure Unit (smua)." )]
    public partial string CurrentSourceSmu { get; set; } = "smua";

    /// <summary>   Gets or sets the bridge-wire resistance measurement aperture. </summary>
    /// <value> The bridge-wire resistance measurement aperture. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Aperture (1)." )]
    public partial double Aperture { get; set; } = 1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement aperture Maximum. </summary>
    /// <value> The bridge-wire resistance measurement aperture Maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Maximum Bridge-Wire Measurement Aperture (1)." )]
    public partial double ApertureMaximum { get; set; } = 10;

    /// <summary>   Gets or sets the bridge-wire resistance measurement aperture minimum. </summary>
    /// <value> The bridge-wire resistance measurement aperture minimum. </value>
    [ObservableProperty]
    [Description( "Specifies the Minimum Bridge-Wire Measurement Aperture (1)." )]
    public partial double ApertureMinimum { get; set; } = 0.01;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current source current level. </summary>
    /// <value> The bridge-wire resistance measurement current source current level. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Current Source Current Level (0.01)." )]
    public partial double CurrentLevel { get; set; } = 0.01;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage source current Limit. </summary>
    /// <value> The bridge-wire resistance measurement voltage source current Limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Voltage Source Current Limit (0.01)." )]
    public partial double CurrentLimit { get; set; } = 0.01;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current Maximum. </summary>
    /// <value> The bridge-wire resistance measurement current Maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Maximum Current (0.05)." )]
    public partial double CurrentMaximum { get; set; } = 0.05;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current minimum. </summary>
    /// <value> The bridge-wire resistance measurement current minimum. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Minimum Current (0.0001)." )]
    public partial double CurrentMinimum { get; set; } = 0.0001;

    /// <summary>   Gets or sets the bridge-wire resistance measurement fail status. </summary>
    /// <value> The bridge-wire resistance measurement fail status. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Fail StatusReading (2)." )]
    public partial int FailStatus { get; set; } = 2;

    /// <summary>   Gets or sets the bridge-wire resistance measurement High limit. </summary>
    /// <value> The bridge-wire resistance measurement High limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement High Limit (2.156)." )]
    public partial double HighLimit { get; set; } = 2.156;

    /// <summary>   Gets or sets the bridge-wire resistance measurement low limit. </summary>
    /// <value> The bridge-wire resistance measurement low limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Low Limit (1.85)." )]
    public partial double LowLimit { get; set; } = 1.85;

    /// <summary>   Gets or sets the bridge-wire resistance measurement minimum. </summary>
    /// <value> The bridge-wire resistance measurement minimum. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Minimum (0.1)." )]
    public partial double Minimum { get; set; } = 0.1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement Maximum. </summary>
    /// <value> The bridge-wire resistance measurement Maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Measurement Maximum (10)." )]
    public partial double Maximum { get; set; } = 10;

    /// <summary>   Specifies the Source Meter Output Option [Current; 0]. </summary>
    /// <value> The source output. </value>
    [ObservableProperty]
    [Description( "Specifies the Source Meter Output Option (Current; 0)" )]
    public partial SourceOutputOption SourceOutput { get; set; } = SourceOutputOption.Current;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage source voltage Level. </summary>
    /// <value> The bridge-wire resistance measurement voltage source voltage Level. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Current Source Voltage Level (0.1)." )]
    public partial double VoltageLevel { get; set; } = 0.1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current source voltage limit. </summary>
    /// <value> The bridge-wire resistance measurement current source voltage limit. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Current Source Voltage Limit (0.1)." )]
    public partial double VoltageLimit { get; set; } = 0.1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage maximum. </summary>
    /// <value> The bridge-wire resistance measurement voltage maximum. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Maximum Voltage Limit (0.1)." )]
    public partial double VoltageMaximum { get; set; } = 0.1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage Minimum. </summary>
    /// <value> The bridge-wire resistance measurement voltage Minimum. </value>
    [ObservableProperty]
    [Description( "Specifies the Bridge-Wire Minimum Voltage Limit (0.001)." )]
    public partial double VoltageMinimum { get; set; } = 0.001;

    #endregion
}
