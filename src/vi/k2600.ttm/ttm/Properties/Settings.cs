using cc.isr.Json.AppSettings.Models;

namespace cc.isr.VI.Tsp.K2600.Ttm.Properties;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public Settings() { }

    #endregion

    #region " singleton "

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static Settings Instance => _instance.Value;

    private static readonly Lazy<Settings> _instance = new( () => new Settings(), true );

    #endregion

    #region " scribe "

    /// <summary>   Gets or sets the scribe. </summary>
    /// <value> The scribe. </value>
    public AppSettingsScribe? Scribe { get; set; }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void ReadSettings( System.Type callingEntity, string settingsFileSuffix )
    {
        AssemblyFileInfo ai = new( callingEntity.Assembly, null, settingsFileSuffix, ".json" );

        // must copy application context settings here to clear any bad settings files.

        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! );
        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.ThisUserAssemblyFilePath! );

        this.Scribe = new( [this.TtmEstimatorSettings, this.TtmMeterSettings, this.TtmResistanceSettings,
            this.TtmSessionSettings, this.TtmShuntSettings, this.TtmTimingSettings, this.TtmTraceSettings],
            ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = ai.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = ai.ThisUserAssemblyFilePath
        };
        this.Scribe.ReadSettings();
    }

    #endregion

    #region " setting instances 

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

    /// <summary>   Gets or sets the TTM Session settings. </summary>
    /// <value> The TTM Session settings. </value>
    public TtmSessionSettings TtmSessionSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Timing settings. </summary>
    /// <value> The TTM Timing settings. </value>
    public TtmTimingSettings TtmTimingSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Trace settings. </summary>
    /// <value> The TTM Trace settings. </value>
    public TtmTraceSettings TtmTraceSettings { get; private set; } = new();

    #endregion
}
