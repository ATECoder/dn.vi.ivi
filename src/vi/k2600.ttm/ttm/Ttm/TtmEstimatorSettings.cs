using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Driver Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class TtmEstimatorSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmEstimatorSettings() { }

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

    #region " thermal coefficient "

    private double _thermalCoefficient = 0.0005;

    /// <summary>   Gets or sets the thermal coefficient in inverse degrees centigrade. </summary>
    /// <value> The thermal coefficient. </value>
	[Description( "The Bridge-Wire Thermal Coefficient in Inverse Degrees Centigrade (0.0005)." )]
    public double ThermalCoefficient
    {
        get => this._thermalCoefficient;
        set => this.SetProperty( ref this._thermalCoefficient, value );
    }

    private double _thermalCoefficientDefault = 0.0005;

    /// <summary>   Gets or sets the default thermal coefficient in inverse degrees centigrade. </summary>
    /// <value> The default thermal coefficient. </value>
	[Description( "The Default Bridge-Wire Thermal Coefficient (0.0005)." )]
    public double ThermalCoefficientDefault
    {
        get => this._thermalCoefficientDefault;
        set => this.SetProperty( ref this._thermalCoefficientDefault, value );
    }

    private double _thermalCoefficientMaximum = 0.001;

    /// <summary>   Gets or sets the thermal coefficient maximum. </summary>
    /// <value> The thermal coefficient maximum. </value>
	[Description( "The Maximum Bridge-Wire Thermal Coefficient (0.001)." )]
    public double ThermalCoefficientMaximum
    {
        get => this._thermalCoefficientMaximum;
        set => this.SetProperty( ref this._thermalCoefficientMaximum, value );
    }

    private double _thermalCoefficientMinimum = 0.00001;

    /// <summary>   Gets or sets the thermal coefficient minimum. </summary>
    /// <value> The thermal coefficient minimum. </value>
	[Description( "The Minimum Bridge-Wire Thermal Coefficient (0.0001)." )]
    public double ThermalCoefficientMinimum
    {
        get => this._thermalCoefficientMinimum;
        set => this.SetProperty( ref this._thermalCoefficientMinimum, value );
    }

    #endregion
}
