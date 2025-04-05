using System.Text.Json.Serialization;
using cc.isr.Json.AppSettings.Models;
using cc.isr.Logging.TraceLog;

namespace cc.isr.VI.Tsp.K2600.Ttm.Properties;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings
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

    /// <summary>   Gets or sets the scribe. </summary>
    /// <value> The scribe. </value>
    public AppSettingsScribe? Scribe { get; set; }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    /// <param name="settingsAssembly">     The settings assembly. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.driver' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public void CreateScribe( System.Reflection.Assembly settingsAssembly, string settingsFileSuffix = ".driver", bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        AssemblyFileInfo ai = new( settingsAssembly, null, settingsFileSuffix, ".json" );

        // copy application context settings if these do not exist; use restore if the settings are bad.

        AppSettingsScribe.InitializeSettingsFiles( ai, overrideAllUsersFile, overrideThisUserFile );

        this.Scribe = new( [this.TraceLogSettings, this.TtmEstimatorSettings, this.TtmMeterSettings, this.TtmResistanceSettings,
            this.TtmShuntSettings, this.TtmTraceSettings],
            ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = ai.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = ai.ThisUserAssemblyFilePath
        };
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.driver' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public void CreateScribe( System.Type callingEntity, string settingsFileSuffix = ".driver", bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        this.CreateScribe( callingEntity.Assembly, settingsFileSuffix, overrideAllUsersFile, overrideThisUserFile );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-17. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="settingsAssembly">     The assembly where the settings fille is located.. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.driver' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public void ReadSettings( System.Reflection.Assembly settingsAssembly, string settingsFileSuffix = ".driver", bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        if ( this.Scribe is null )
            this.CreateScribe( settingsAssembly, settingsFileSuffix, overrideAllUsersFile, overrideThisUserFile );
        else
            this.Scribe.InitializeSettingsFiles( overrideAllUsersFile, overrideThisUserFile );

        this.Scribe!.ReadSettings();

        if ( !this.SettingsExist( out string settingsClassName ) )
            throw new InvalidOperationException( $"{settingsClassName} not found or failed to read from {this.Scribe.UserSettingsPath}." );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks> 2024-08-05. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   The suffix of the assembly settings file, e.g.,
    ///                                     '.driver' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public void ReadSettings( System.Type callingEntity, string settingsFileSuffix = ".driver", bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        this.ReadSettings( callingEntity.Assembly, settingsFileSuffix, overrideAllUsersFile, overrideThisUserFile );
    }

    /// <summary>   Gets the full path name of the settings file. </summary>
    /// <value> The full path name of the settings file. </value>
    [JsonIgnore]
    public string? FilePath => this.Scribe?.UserSettingsPath;

    /// <summary>   Check if the settings file exists. </summary>
    /// <remarks>   2024-07-06. </remarks>
    /// <returns>   True if it the settings file exists; otherwise false. </returns>
    public bool SettingsFileExists()
    {
        return this.FilePath is not null && System.IO.File.Exists( this.FilePath );
    }

    /// <summary>   Checks if all settings exist. </summary>
    /// <remarks>   2025-01-18. </remarks>
    /// <param name="settingsClassName"> The name of the settings class that failed to read. </param>
    /// <returns>   True if all settings exit; otherwise false. </returns>
    public bool SettingsExist( out string settingsClassName )
    {
        if ( this.TraceLogSettings is null || !this.TraceLogSettings.Exists )
            settingsClassName = $"{nameof( Settings.TraceLogSettings )}";
        else if ( this.TtmEstimatorSettings is null || !this.TtmEstimatorSettings.Exists )
            settingsClassName = $"{nameof( Settings.TtmEstimatorSettings )}";
        else if ( this.TtmMeterSettings is null || !this.TtmMeterSettings.Exists )
            settingsClassName = $"{nameof( Settings.TtmMeterSettings )}";
        else if ( this.TtmResistanceSettings is null || !this.TtmResistanceSettings.Exists )
            settingsClassName = $"{nameof( Settings.TtmResistanceSettings )}";
        else if ( this.TtmShuntSettings is null || !this.TtmShuntSettings.Exists )
            settingsClassName = $"{nameof( Settings.TtmShuntSettings )}";
        else if ( this.TtmTraceSettings is null || !this.TtmTraceSettings.Exists )
            settingsClassName = $"{nameof( Settings.TtmTraceSettings )}";
        else
            settingsClassName = string.Empty;

        return settingsClassName.Length == 0;
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
