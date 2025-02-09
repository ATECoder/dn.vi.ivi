using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Meter Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class TtmMeterSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmMeterSettings() { }

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

    #region " meter "

    private double _lineFrequency = 60;

    /// <summary>   Gets or sets the line frequency. </summary>
    /// <value> The line frequency. </value>
	[Description( "The Instrument Line Frequency (60)." )]
    public double LineFrequency
    {
        get => this._lineFrequency;
        set => this.SetProperty( ref this._lineFrequency, value );
    }

    private int _legacyDriver = 1;

    /// <summary>   Gets or sets the legacy driver flag. </summary>
    /// <value> The legacy driver flag. </value>
	[Description( "The Post Transient Delay Time in Seconds (0.5)." )]
    public int LegacyDriver
    {
        get => this._legacyDriver;
        set => this.SetProperty( ref this._legacyDriver, value );
    }

    private int _legacyDriverDefault = 1;

    /// <summary>   Gets or sets the default legacy driver flag. </summary>
    /// <value> The default legacy driver flag. </value>
	[Description( "The Default Legacy Driver Flag (1)." )]
    public int LegacyDriverDefault
    {
        get => this._legacyDriverDefault;
        set => this.SetProperty( ref this._legacyDriverDefault, value );
    }

    #endregion

    #region " contact check "

    private cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions _contactCheckOptions = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Start;

    /// <summary>   Gets or sets options for controlling the contact check. </summary>
    /// <value> Options that control the contact check. </value>
	[Description( "The Contact Check Options (1)." )]
    public cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions ContactCheckOptions
    {
        get => this._contactCheckOptions;
        set => this.SetProperty( ref this._contactCheckOptions, value );
    }

    private cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions _contactCheckOptionsDefault = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Start;

    /// <summary>   Gets or sets options for controlling the contact check. </summary>
    /// <value> Options that control the contact check. </value>
	[Description( "The Default Contact Check Options (1)." )]
    public cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions ContactCheckOptionsDefault
    {
        get => this._contactCheckOptionsDefault;
        set => this.SetProperty( ref this._contactCheckOptionsDefault, value );
    }

    private bool _contactCheckEnabled = false;

    /// <summary>   Gets or sets a value indicating whether the contact check is enabled. </summary>
    /// <value> True if contact check enabled, false if not. </value>
	[Description( "The Contact Check Enabled State (false)." )]
    public bool ContactCheckEnabled
    {
        get => this._contactCheckEnabled;
        set => this.SetProperty( ref this._contactCheckEnabled, value );
    }

    private bool _contactCheckEnabledDefault = false;

    /// <summary>
    /// Gets or sets a value indicating whether the contact check enabled default.
    /// </summary>
    /// <value> True if contact check enabled default, false if not. </value>
	[Description( "The Default Contact Check Enabled State (false)." )]
    public bool ContactCheckEnabledDefault
    {
        get => this._contactCheckEnabledDefault;
        set => this.SetProperty( ref this._contactCheckEnabledDefault, value );
    }

    private int _contactCheckThreshold = 10;

    /// <summary>   Gets or sets the contact check threshold. </summary>
    /// <value> The contact check threshold. </value>
	[Description( "The Contact Check Threshold (100)." )]
    public int ContactCheckThreshold
    {
        get => this._contactCheckThreshold;
        set => this.SetProperty( ref this._contactCheckThreshold, value );
    }

    private int _contactCheckThresholdDefault = 100;

    /// <summary>   Gets or sets the contact check threshold default. </summary>
    /// <value> The contact check threshold default. </value>
	[Description( "The Default Contact Check Threshold (100)." )]
    public int ContactCheckThresholdDefault
    {
        get => this._contactCheckThresholdDefault;
        set => this.SetProperty( ref this._contactCheckThresholdDefault, value );
    }

    #endregion

    #region " post transient "

    private double _postTransientDelay = 0.01;

    /// <summary>   Gets or sets the post transient delay. </summary>
    /// <value> The post transient delay. </value>
	[Description( "The Post Transient Delay Time in Seconds (0.01)." )]
    public double PostTransientDelay
    {
        get => this._postTransientDelay;
        set => this.SetProperty( ref this._postTransientDelay, value );
    }

    private double _postTransientDelayDefault = 0.01;
    /// <summary>   Gets or sets the post transient delay default. </summary>    /// <value> The post transient delay default. </value>	[Description( "The Default Post Transient Delay Time in Seconds (0.01)." )]    public double PostTransientDelayDefault    {        get => this._postTransientDelayDefault;        set => this.SetProperty( ref this._postTransientDelayDefault, value );    }    private double _postTransientDelayMaximum = 10;    /// <summary>   Gets or sets the post transient delay maximum. </summary>    /// <value> The post transient delay maximum. </value>	[Description( "The Maximum Post Transient Delay Time in Seconds (10)." )]    public double PostTransientDelayMaximum    {        get => this._postTransientDelayMaximum;        set => this.SetProperty( ref this._postTransientDelayMaximum, value );    }    private double _postTransientDelayMinimum = 0.001;    /// <summary>   Gets or sets the post transient delay minimum. </summary>    /// <value> The post transient delay minimum. </value>
	[Description( "The Minimum Post Transient Delay Time in Seconds (0.001)." )]
    public double PostTransientDelayMinimum
    {
        get => this._postTransientDelayMinimum;
        set => this.SetProperty( ref this._postTransientDelayMinimum, value );
    }

    #endregion

    #region " smu "

    private string _sourceMeasureUnit = Syntax.ThermalTransient.DefaultSourceMeterName;

    /// <summary>   Gets or sets source measure unit. </summary>
    /// <value> The source measure unit. </value>
	[Description( "The Source Measure Unit Name (smua)." )]
    public string SourceMeasureUnit
    {
        get => this._sourceMeasureUnit;
        set => this.SetProperty( ref this._sourceMeasureUnit, value );
    }

    private string _sourceMeasureUnitDefault = Syntax.ThermalTransient.DefaultSourceMeterName;

    /// <summary>   Gets or sets source measure unit default. </summary>
    /// <value> The source measure unit default. </value>
	[Description( "The Default Source Measure Unit Name (smua)." )]
    public string SourceMeasureUnitDefault
    {
        get => this._sourceMeasureUnitDefault;
        set => this.SetProperty( ref this._sourceMeasureUnitDefault, value );
    }

    #endregion
}
