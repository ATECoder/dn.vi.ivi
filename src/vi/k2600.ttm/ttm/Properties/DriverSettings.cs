using cc.isr.Logging.TraceLog;

namespace cc.isr.VI.Tsp.K2600.Ttm.Properties;

/// <summary>   The TTM Driver Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class DriverSettings : Json.AppSettings.Settings.SettingsContainerBase
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    private DriverSettings() { }

    #endregion

    #region " singleton "

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static DriverSettings Instance => _instance.Value;

    private static readonly Lazy<DriverSettings> _instance = new( () => new DriverSettings(), true );

    #endregion

    #region " scribe "

    /// <summary>   Creates the Settings <see cref="cc.isr.Json.AppSettings.Settings.SettingsContainerBase.Scribe"/>. </summary>
    /// <remarks>   2024-08-05. </remarks>
    public override void CreateScribe()
    {
        this.TraceLogSettings = new();
        this.ColdResistanceDefaults = new();
        this.ColdResistanceSettings = new();
        this.EstimatorDefaults = new();
        this.EstimatorSettings = new();
        this.MeterDefaults = new();
        this.MeterSettings = new();
        this.ShuntDefaults = new();
        this.ShuntSettings = new();
        this.TraceDefaults = new();
        this.TraceSettings = new();

        this.Scribe = new( [this.TraceLogSettings,
            this.EstimatorDefaults, this.EstimatorSettings,
            this.ColdResistanceDefaults, this.ColdResistanceSettings,
            this.MeterDefaults, this.MeterSettings,
            this.ShuntSettings,this.ShuntSettings,
            this.TraceDefaults, this.TraceSettings] );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-17. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="declaringType">        The Type of the declaring object. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.driver' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.json' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    /// <param name="overwriteAllUsersFile"> (Optional) [false] True to over-write all users settings file. </param>
    /// <param name="overwriteThisUserFile"> (Optional) [false] True to over-write this user settings file. </param>
    public override void ReadSettings( Type declaringType, string settingsFileSuffix = ".driver",
        bool overwriteAllUsersFile = false, bool overwriteThisUserFile = false )
    {
        base.ReadSettings( declaringType, settingsFileSuffix, overwriteAllUsersFile, overwriteThisUserFile );
    }

    #endregion

    #region " setting instances "

    /// <summary>   Gets or sets the trace log settings. </summary>
    /// <value> The trace log settings. </value>
    public TraceLogSettings TraceLogSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Estimator settings. </summary>
    /// <value> The TTM Estimator settings. </value>
    public EstimatorSettings EstimatorSettings { get; private set; } = new();

    /// <summary>   Gets or sets the estimator defaults. </summary>
    /// <value> The estimator defaults. </value>
    public EstimatorDefaults EstimatorDefaults { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Meter settings. </summary>
    /// <value> The TTM Meter settings. </value>
    public MeterSettings MeterSettings { get; private set; } = new();

    /// <summary>   Gets or sets the meter defaults. </summary>
    /// <value> The meter defaults. </value>
    public MeterDefaults MeterDefaults { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Cold Resistance settings. </summary>
    /// <value> The TTM Cold Resistance settings. </value>
    public ColdResistanceSettings ColdResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the cold resistance defaults. </summary>
    /// <value> The cold resistance defaults. </value>
    public ColdResistanceDefaults ColdResistanceDefaults { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Shunt settings. </summary>
    /// <value> The TTM Shunt settings. </value>
    public ShuntSettings ShuntSettings { get; private set; } = new();

    /// <summary>   Gets or sets the shunt defaults. </summary>
    /// <value> The shunt defaults. </value>
    public ShuntDefaults ShuntDefaults { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Trace settings. </summary>
    /// <value> The TTM Trace settings. </value>
    public TraceSettings TraceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the trace defaults. </summary>
    /// <value> The trace defaults. </value>
    public TraceDefaults TraceDefaults { get; private set; } = new();

    #endregion
}
