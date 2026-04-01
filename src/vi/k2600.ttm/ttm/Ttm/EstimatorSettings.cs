using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Driver Estimator Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class EstimatorSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    #region " thermal coefficient "

    /// <summary>   Gets or sets the thermal coefficient in inverse degrees centigrade. </summary>
    /// <value> The thermal coefficient. </value>
    [ObservableProperty]
    [Description( "The Bridge-Wire Thermal Coefficient in Inverse Degrees Centigrade (0.0005)." )]
    public partial double ThermalCoefficient { get; set; } = 0.0005;

    #endregion
}
