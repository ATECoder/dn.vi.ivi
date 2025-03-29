using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Driver Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class TtmShuntSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmShuntSettings() { }

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

    #region " Shunt Resistance Measurement properties "

    private double _aperture = 0.1;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement aperture. </summary>
    /// <value> The Shunt MeasuredValue Measurement aperture. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Aperture (0.1)." )]
    public double Aperture
    {
        get => this._aperture;
        set => this.SetProperty( ref this._aperture, value );
    }

    private double _apertureDefault = 0.1;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement default aperture. </summary>
    /// <value> The Shunt MeasuredValue Measurement aperture default. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Default Aperture (0.1)." )]
    public double ApertureDefault
    {
        get => this._apertureDefault;
        set => this.SetProperty( ref this._apertureDefault, value );
    }

    private double _apertureMaximum = 5;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement aperture Maximum. </summary>
    /// <value> The Shunt MeasuredValue Measurement aperture Maximum. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Maximum Aperture (5)." )]
    public double ApertureMaximum
    {
        get => this._apertureMaximum;
        set => this.SetProperty( ref this._apertureMaximum, value );
    }

    private double _apertureMinimum = 0.01;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement aperture minimum. </summary>
    /// <value> The Shunt MeasuredValue Measurement aperture minimum. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Minimum Aperture (0.01)." )]
    public double ApertureMinimum
    {
        get => this._apertureMinimum;
        set => this.SetProperty( ref this._apertureMinimum, value );
    }

    private double _currentLevel = 0.001;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Measurement current source current level. </summary>
    /// <value> The Shunt MeasuredValue Measurement current level. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Source Current Level (0.001)." )]
    public double CurrentLevel
    {
        get => this._currentLevel;
        set => this.SetProperty( ref this._currentLevel, value );
    }

    private double _currentLevelDefault = 0.001;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement current source default current level. </summary>
    /// <value> The Shunt MeasuredValue Measurement current level default. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Source Default Current Level (0.001)." )]
    public double CurrentLevelDefault
    {
        get => this._currentLevelDefault;
        set => this.SetProperty( ref this._currentLevelDefault, value );
    }

    private double _currentMinimum = 0.00001;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Minimum Current. </summary>
    /// <value> The Shunt MeasuredValue Measurement Minimum Current. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Minimum Current (0.00001)." )]
    public double CurrentMinimum
    {
        get => this._currentMinimum;
        set => this.SetProperty( ref this._currentMinimum, value );
    }

    /// <summary>   Gets the Shunt MeasuredValue Measurement Maximum Current. </summary>
    /// <value> The Shunt MeasuredValue Measurement Maximum Current. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Maximum Current (readonly)." )]
    public double CurrentMaximum => this.CurrentMinimum + this.CurrentRange;

    private double _currentRange = 0.01;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement current Range. </summary>
    /// <value> The Shunt MeasuredValue Measurement current Range. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Range (0.01)." )]
    public double CurrentRange
    {
        get => this._currentRange;
        set => this.SetProperty( ref this._currentRange, value );
    }

    private double _currentRangeDefault = 0.01;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Default Current Range. </summary>
    /// <value> The Shunt MeasuredValue Measurement Default Current Range. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Default Current Range (0.01)." )]
    public double CurrentRangeDefault
    {
        get => this._currentRangeDefault;
        set => this.SetProperty( ref this._currentRangeDefault, value );
    }

    private double _highLimit = 1250;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement High limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement High limit. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement High Limit (1250)." )]
    public double HighLimit
    {
        get => this._highLimit;
        set => this.SetProperty( ref this._highLimit, value );
    }

    private double _highLimitDefault = 1250;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Default High limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement Default High limit. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Default High Limit (1250)." )]
    public double HighLimitDefault
    {
        get => this._highLimitDefault;
        set => this.SetProperty( ref this._highLimitDefault, value );
    }

    private double _lowLimit = 1150;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement low limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement low limit. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Low Limit (1150)." )]
    public double LowLimit
    {
        get => this._lowLimit;
        set => this.SetProperty( ref this._lowLimit, value );
    }

    private double _lowLimitDefault = 1150;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Default Low Limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement Default Low Limit. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Default Low Limit (1150)." )]
    public double LowLimitDefault
    {
        get => this._lowLimitDefault;
        set => this.SetProperty( ref this._lowLimitDefault, value );
    }

    private double _maximum = 10000;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Maximum. </summary>
    /// <value> The Shunt MeasuredValue Measurement Maximum. </value>
    [Description( "Specifies the Shunt MeasuredValue Maximum (10000)." )]
    public double Maximum
    {
        get => this._maximum;
        set => this.SetProperty( ref this._maximum, value );
    }

    private double _minimum = 900;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement minimum. </summary>
    /// <value> The Shunt MeasuredValue Measurement minimum. </value>
    [Description( "Specifies the Shunt MeasuredValue Minimum (900)." )]
    public double Minimum
    {
        get => this._minimum;
        set => this.SetProperty( ref this._minimum, value );
    }

    private double _voltageLimit = 10;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Current Source Voltage Limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement Current Source Voltage Limit. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Source Current Source Voltage Limit (10)." )]
    public double VoltageLimit
    {
        get => this._voltageLimit;
        set => this.SetProperty( ref this._voltageLimit, value );
    }

    private double _voltageLimitDefault = 10;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Current Source Default Voltage Limit. </summary>
    /// <value> The Shunt MeasuredValue Measurement Current Source Default Voltage Limit. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Current Source Current Source Default Voltage Limit (10)." )]
    public double VoltageLimitDefault
    {
        get => this._voltageLimitDefault;
        set => this.SetProperty( ref this._voltageLimitDefault, value );
    }

    private double _voltageMaximum = 100;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Maximum Voltage. </summary>
    /// <value> The Shunt MeasuredValue Measurement voltage maximum. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Maximum Voltage (100)." )]
    public double VoltageMaximum
    {
        get => this._voltageMaximum;
        set => this.SetProperty( ref this._voltageMaximum, value );
    }

    private double _voltageMinimum = 1;

    /// <summary>   Gets or sets the Shunt MeasuredValue Measurement Minimum Voltage. </summary>
    /// <value> The Shunt MeasuredValue Measurement Minimum Voltage. </value>
    [Description( "Specifies the Shunt MeasuredValue Measurement Minimum Voltage (11)." )]
    public double VoltageMinimum
    {
        get => this._voltageMinimum;
        set => this.SetProperty( ref this._voltageMinimum, value );
    }

    #endregion

}
