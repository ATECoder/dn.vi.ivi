using cc.isr.Logging.TraceLog;

namespace cc.isr.VI.Tsp.K2600.Ttm.Properties;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings : Json.AppSettings.Settings.SettingsContainerBase
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    private Settings() { }

    #endregion

    #region " singleton "

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static Settings Instance => _instance.Value;

    private static readonly Lazy<Settings> _instance = new( () => new Settings(), true );

    #endregion

    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    public override void CreateScribe()
    {
        this.TraceLogSettings = new();
        this.TtmEstimatorSettings = new();
        this.TtmMeterSettings = new();
        this.TtmResistanceSettings = new();
        this.TtmShuntSettings = new();
        this.TtmTraceSettings = new();

        this.Scribe = new( [this.TraceLogSettings, this.TtmEstimatorSettings, this.TtmMeterSettings, this.TtmResistanceSettings,
            this.TtmShuntSettings, this.TtmTraceSettings] );
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
    public TtmEstimatorSettings TtmEstimatorSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Meter settings. </summary>
    /// <value> The TTM Meter settings. </value>
    public TtmMeterSettings TtmMeterSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM MeasuredValue settings. </summary>
    /// <value> The TTM MeasuredValue settings. </value>
    public TtmResistanceSettings TtmResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Shunt settings. </summary>
    /// <value> The TTM Shunt settings. </value>
    public TtmShuntSettings TtmShuntSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Trace settings. </summary>
    /// <value> The TTM Trace settings. </value>
    public TtmTraceSettings TtmTraceSettings { get; private set; } = new();

    #endregion
}
