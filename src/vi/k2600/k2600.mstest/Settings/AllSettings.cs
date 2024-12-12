using System;
using cc.isr.VI.Tsp.K2600.MSTest.Measure;
using cc.isr.VI.Tsp.K2600.MSTest.Source;
using cc.isr.VI.Tsp.K2600.MSTest.Subsystems;
using cc.isr.VI.Tsp.K2600.MSTest.Visa;

namespace cc.isr.VI.Tsp.K2600.MSTest.Settings;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class AllSettings
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    /// <remarks>   2023-04-24. </remarks>
    public AllSettings() { }

    #endregion

    #region " singleton "

    /// <summary>
    /// Creates an instance of the <see cref="AllSettings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static AllSettings CreateInstance()
    {
        AllSettings ti = new();
        AppSettingsScribe.ReadSettings( SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( AllSettings ), ti );
        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static AllSettings Instance => _instance.Value;

    private static readonly Lazy<AllSettings> _instance = new( CreateInstance, true );

    #endregion

    #region " assembly settings file information "

    /// <summary>
    /// Creates an instance of the settings <see cref="AssemblyFileInfo"/>. This restores the
    /// application context settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2024-07-18. </remarks>
    /// <returns>   The new settings file information. </returns>
    private static AssemblyFileInfo CreateSettingsFileInfo()
    {
        // get assembly files using the .Logging suffix.

        AssemblyFileInfo ai = new( typeof( AllSettings ).Assembly, null, ".Settings", ".json" );

        // must copy application context settings here to clear any bad settings files.

        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! );
        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.ThisUserAssemblyFilePath! );

        return ai;
    }

    /// <summary>   Gets information describing the settings file. </summary>
    /// <value> Information describing the settings file. </value>
    [JsonIgnore]
    internal static AssemblyFileInfo SettingsFileInfo => _settingsFileInfo.Value;

    private static readonly Lazy<AssemblyFileInfo> _settingsFileInfo = new( CreateSettingsFileInfo, true );

    #endregion

    #region " settings scribe "

    /// <summary>
    /// Creates an instance of the <see cref="AllSettings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static AppSettingsScribe CreateScribe()
    {
        // get an instance of the settings file info first.
        AssemblyFileInfo settingsFileInfo = SettingsFileInfo;

        AppSettingsScribe scribe = new( [AllSettings.DummySettings, AllSettings.TestSiteSettings, AllSettings.ResourceSettings,
            AllSettings.DeviceErrorsSettings, AllSettings.SubsystemsSettings, AllSettings.ResistanceSettings, AllSettings.CurrentSourceSettings],
            settingsFileInfo.AppContextAssemblyFilePath!, settingsFileInfo.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = settingsFileInfo.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = settingsFileInfo.ThisUserAssemblyFilePath
        };
        scribe.ReadSettings();

        return scribe;
    }

    /// <summary>   Gets the <see cref="AppSettingsScribe">settings reader and writer</see>. </summary>
    /// <value> The scribe. </value>
    [JsonIgnore]
    public static AppSettingsScribe Scribe => _scribe.Value;

    private static readonly Lazy<AppSettingsScribe> _scribe = new( CreateScribe, true );

    /// <summary>   Gets the full pathname of the settings file. </summary>
    /// <value> The full pathname of the settings file. </value>
    [JsonIgnore]
    public static string FilePath => Scribe.UserSettingsPath;

    /// <summary>   Check if the settings file exists. </summary>
    /// <remarks>   2024-07-06. </remarks>
    /// <returns>   True if it the settings file exists; otherwise false. </returns>
    public static bool Exists()
    {
        return System.IO.File.Exists( FilePath );
    }

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the dummy settings. </summary>
    /// <value> The dummy settings. </value>
    internal static DummySettings DummySettings { get; private set; } = new();

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    internal static TestSiteSettings TestSiteSettings { get; private set; } = new();

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    internal static ResourceSettings ResourceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the resistance settings. </summary>
    /// <value> The resistance settings. </value>
    internal static ResistanceSettings ResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the current source settings. </summary>
    /// <value> The current source settings. </value>
    internal static CurrentSourceSettings CurrentSourceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the subsystems settings. </summary>
    /// <value> The subsystems settings. </value>
    internal static SubsystemsSettings SubsystemsSettings { get; private set; } = new();

    /// <summary>   Gets or sets the device errors settings. </summary>
    /// <value> The device errors settings. </value>
    internal static DeviceErrorsSettings DeviceErrorsSettings { get; private set; } = new();

    #endregion
}

