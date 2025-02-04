using System.ComponentModel;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Driver Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class TtmResistanceSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmResistanceSettings() { }

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

    #region " legacy settings "

    private string _currentSourceSmu = "smua";

    /// <summary>   Gets or sets the current source smu. </summary>
    /// <value> The current source smu. </value>
	[Description( "Specifies the Bridge-Wire Measurement Current Source Source Measure Unit (smua)." )]
    public string CurrentSourceSmu
    {
        get => this._currentSourceSmu;
        set => _ = this.SetProperty( ref this._currentSourceSmu, value );
    }

    #endregion

    #region " resistance "

    private double _aperture = 1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement aperture. </summary>
    /// <value> The bridge-wire resistance measurement aperture. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Aperture (1)." )]
    public double Aperture
    {
        get => this._aperture;
        set => this.SetProperty( ref this._aperture, value );
    }

    private double _apertureDefault = 1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement aperture default. </summary>
    /// <value> The bridge-wire resistance measurement aperture default. </value>
	[Description( "Specifies the Default Bridge-Wire MeasuredValue Measurement Aperture (1)." )]
    public double ApertureDefault
    {
        get => this._apertureDefault;
        set => this.SetProperty( ref this._apertureDefault, value );
    }

    private double _apertureMaximum = 10;

    /// <summary>   Gets or sets the bridge-wire resistance measurement aperture Maximum. </summary>
    /// <value> The bridge-wire resistance measurement aperture Maximum. </value>
	[Description( "Specifies the Maximum Bridge-Wire MeasuredValue Measurement Aperture (1)." )]
    public double ApertureMaximum
    {
        get => this._apertureMaximum;
        set => this.SetProperty( ref this._apertureMaximum, value );
    }

    private double _apertureMinimum = 0.01;

    /// <summary>   Gets or sets the bridge-wire resistance measurement aperture minimum. </summary>
    /// <value> The bridge-wire resistance measurement aperture minimum. </value>
	[Description( "Specifies the Minimum Bridge-Wire MeasuredValue Measurement Aperture (1)." )]
    public double ApertureMinimum
    {
        get => this._apertureMinimum;
        set => this.SetProperty( ref this._apertureMinimum, value );
    }

    private double _currentLevel = 0.01;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current source current level. </summary>
    /// <value> The bridge-wire resistance measurement current source current level. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Current Source Current Level (0.01)." )]
    public double CurrentLevel
    {
        get => this._currentLevel;
        set => this.SetProperty( ref this._currentLevel, value );
    }

    private double _currentLevelDefault = 0.01;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current source default current level. </summary>
    /// <value> The bridge-wire resistance measurement current source default current level. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Current Source Default Current Level (0.01)." )]
    public double CurrentLevelDefault
    {
        get => this._currentLevelDefault;
        set => this.SetProperty( ref this._currentLevelDefault, value );
    }

    private double _currentLimit = 0.01;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage source current Limit. </summary>
    /// <value> The bridge-wire resistance measurement voltage source current Limit. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Voltage Source Current Limit (0.01)." )]
    public double CurrentLimit
    {
        get => this._currentLimit;
        set => this.SetProperty( ref this._currentLimit, value );
    }

    private double _currentLimitDefault = 0.01;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage source default current Limit. </summary>
    /// <value> The bridge-wire resistance measurement voltage source default current Limit. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Voltage Source Default Current Limit (0.01)." )]
    public double CurrentLimitDefault
    {
        get => this._currentLimitDefault;
        set => this.SetProperty( ref this._currentLimitDefault, value );
    }

    private double _currentMaximum = 0.01;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current Maximum. </summary>
    /// <value> The bridge-wire resistance measurement current Maximum. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Maximum Current (0.05)." )]
    public double CurrentMaximum
    {
        get => this._currentMaximum;
        set => this.SetProperty( ref this._currentMaximum, value );
    }

    private double _currentMinimum = 0.0001;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current minimum. </summary>
    /// <value> The bridge-wire resistance measurement current minimum. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Minimum Current (0.0001)." )]
    public double CurrentMinimum
    {
        get => this._currentMinimum;
        set => this.SetProperty( ref this._currentMinimum, value );
    }

    private int _failStatus = 2;

    /// <summary>   Gets or sets the bridge-wire resistance measurement fail status. </summary>
    /// <value> The bridge-wire resistance measurement fail status. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Fail StatusReading (2)." )]
    public int FailStatus
    {
        get => this._failStatus;
        set => this.SetProperty( ref this._failStatus, value );
    }

    private int _failStatusDefault = 2;

    /// <summary>   Gets or sets the bridge-wire resistance measurement default fail status. </summary>
    /// <value> The bridge-wire resistance measurement default fail status. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Default Fail StatusReading (2)." )]
    public int FailStatusDefault
    {
        get => this._failStatusDefault;
        set => this.SetProperty( ref this._failStatusDefault, value );
    }


    private double _highLimit = 2.156;

    /// <summary>   Gets or sets the bridge-wire resistance measurement High limit. </summary>
    /// <value> The bridge-wire resistance measurement High limit. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement High Limit (2.156)." )]
    public double HighLimit
    {
        get => this._highLimit;
        set => this.SetProperty( ref this._highLimit, value );
    }

    private double _highLimitDefault = 2.156;

    /// <summary>   Gets or sets the bridge-wire resistance measurement High limit default. </summary>
    /// <value> The bridge-wire resistance measurement High limit default. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Default High Limit (2.156)." )]
    public double HighLimitDefault
    {
        get => this._highLimitDefault;
        set => this.SetProperty( ref this._highLimitDefault, value );
    }

    private double _lowLimit = 1.85;

    /// <summary>   Gets or sets the bridge-wire resistance measurement low limit. </summary>
    /// <value> The bridge-wire resistance measurement low limit. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Low Limit (1.85)." )]
    public double LowLimit
    {
        get => this._lowLimit;
        set => this.SetProperty( ref this._lowLimit, value );
    }

    private double _lowLimitDefault = 1.85;

    /// <summary>   Gets or sets the bridge-wire resistance measurement low limit default. </summary>
    /// <value> The bridge-wire resistance measurement low limit default. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Default Low Limit (1.85)." )]
    public double LowLimitDefault
    {
        get => this._lowLimitDefault;
        set => this.SetProperty( ref this._lowLimitDefault, value );
    }

    private double _minimum = 0.1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement minimum. </summary>
    /// <value> The bridge-wire resistance measurement minimum. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Minimum (0.1)." )]
    public double Minimum
    {
        get => this._minimum;
        set => this.SetProperty( ref this._minimum, value );
    }

    private double _maximum = 10;

    /// <summary>   Gets or sets the bridge-wire resistance measurement Maximum. </summary>
    /// <value> The bridge-wire resistance measurement Maximum. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Measurement Maximum (10)." )]
    public double Maximum
    {
        get => this._maximum;
        set => this.SetProperty( ref this._maximum, value );
    }

    private SourceOutputOption _sourceOutput = SourceOutputOption.Current;

    /// <summary>   Specifies the Source Meter Output Option [Current; 0]. </summary>
    /// <value> The source output. </value>
	[Description( "Specifies the Source Meter Output Option (Current; 0)" )]
    public SourceOutputOption SourceOutput
    {
        get => this._sourceOutput;
        set => this.SetProperty( ref this._sourceOutput, value );
    }

    private SourceOutputOption _sourceOutputDefault = SourceOutputOption.Current;

    /// <summary>   Specifies the Source Meter Default Source Output Option [Current; 0]. </summary>
    /// <value> The source output. </value>
	[Description( "Specifies the Source Meter Default Source Output Option (Current; 0)" )]
    public SourceOutputOption SourceOutputDefault
    {
        get => this._sourceOutput;
        set => this.SetProperty( ref this._sourceOutputDefault, value );
    }

    private double _voltageLevel = 0.1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage source voltage Level. </summary>
    /// <value> The bridge-wire resistance measurement voltage source voltage Level. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Current Source Voltage Level (0.1)." )]
    public double VoltageLevel
    {
        get => this._voltageLevel;
        set => this.SetProperty( ref this._voltageLevel, value );
    }

    private double _voltageLevelDefault = 0.1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage source default voltage Level. </summary>
    /// <value> The bridge-wire resistance measurement voltage source default voltage Level. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Current Source Default Voltage Level (0.1)." )]
    public double VoltageLevelDefault
    {
        get => this._voltageLevelDefault;
        set => this.SetProperty( ref this._voltageLevelDefault, value );
    }

    private double _voltageLimit = 0.1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current source voltage limit. </summary>
    /// <value> The bridge-wire resistance measurement current source voltage limit. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Current Source Voltage Limit (0.1)." )]
    public double VoltageLimit
    {
        get => this._voltageLimit;
        set => this.SetProperty( ref this._voltageLimit, value );
    }

    private double _voltageLimitDefault = 0.1;

    /// <summary>   Gets or sets the bridge-wire resistance measurement current source default voltage limit. </summary>
    /// <value> The bridge-wire resistance measurement current source default voltage limit. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Current Source Default Voltage Limit (0.1)." )]
    public double VoltageLimitDefault
    {
        get => this._voltageLimitDefault;
        set => this.SetProperty( ref this._voltageLimitDefault, value );
    }

    private double _voltageMaximum = 10;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage maximum. </summary>
    /// <value> The bridge-wire resistance measurement voltage maximum. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Maximum Voltage Limit (9.999)." )]
    public double VoltageMaximum
    {
        get => this._voltageMaximum;
        set => this.SetProperty( ref this._voltageMaximum, value );
    }

    private double _voltageMinimum = 0.001;

    /// <summary>   Gets or sets the bridge-wire resistance measurement voltage Minimum. </summary>
    /// <value> The bridge-wire resistance measurement voltage Minimum. </value>
	[Description( "Specifies the Bridge-Wire MeasuredValue Minimum Voltage Limit (0.001)." )]
    public double VoltageMinimum
    {
        get => this._voltageMinimum;
        set => this.SetProperty( ref this._voltageMinimum, value );
    }

    #endregion
}
