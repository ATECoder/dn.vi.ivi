using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Driver Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class TtmEstimatorSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmEstimatorSettings() { }

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

    #region " thermal coefficient "

    /// <summary>   Gets or sets the thermal coefficient in inverse degrees centigrade. </summary>
    /// <value> The thermal coefficient. </value>
    [ObservableProperty]
    [Description( "The Bridge-Wire Thermal Coefficient in Inverse Degrees Centigrade (0.0005)." )]
    public partial double ThermalCoefficient { get; set; } = 0.0005;

    /// <summary>   Gets or sets the default thermal coefficient in inverse degrees centigrade. </summary>
    /// <value> The default thermal coefficient. </value>
    [ObservableProperty]
    [Description( "The Default Bridge-Wire Thermal Coefficient (0.0005)." )]
    public partial double ThermalCoefficientDefault { get; set; } = 0.0005;

    /// <summary>   Gets or sets the thermal coefficient maximum. </summary>
    /// <value> The thermal coefficient maximum. </value>
    [ObservableProperty]
    [Description( "The Maximum Bridge-Wire Thermal Coefficient (0.001)." )]
    public partial double ThermalCoefficientMaximum { get; set; } = 0.001;

    /// <summary>   Gets or sets the thermal coefficient minimum. </summary>
    /// <value> The thermal coefficient minimum. </value>
    [ObservableProperty]
    [Description( "The Minimum Bridge-Wire Thermal Coefficient (0.0001)." )]
    public partial double ThermalCoefficientMinimum { get; set; } = 0.00001;

    #endregion
}
