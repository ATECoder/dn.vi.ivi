using System;
using cc.isr.VI.Pith.Settings;
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

        AppSettingsScribe scribe = new( [AllSettings.TestSiteSettings, AllSettings.IOSettings,
            AllSettings.CommandsSettings, AllSettings.ResourceSettings,
            AllSettings.DeviceErrorsSettings, AllSettings.DigitalIOSettings, AllSettings.SystemSubsystemSettings,
            AllSettings.SenseResistanceSettings, AllSettings.SourceResistanceSettings, AllSettings.ResistanceSettings,
            AllSettings.SenseCurrentSettings, AllSettings.SourceCurrentSettings, AllSettings.CurrentSourceMeasureSettings],
            settingsFileInfo.AppContextAssemblyFilePath!, settingsFileInfo.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = settingsFileInfo.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = settingsFileInfo.ThisUserAssemblyFilePath
        };

        scribe.ReadSettings();

        if ( !AllSettings.SettingsExist( out string details ) )
            throw new InvalidOperationException( details );

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
    public static bool SettingsFileExists()
    {
        return System.IO.File.Exists( FilePath );
    }

    /// <summary>   Checks if all settings exist. </summary>
    /// <remarks>   2025-01-18. </remarks>
    /// <returns>   A Tuple. </returns>
    public static bool SettingsExist( out string details )
    {
        details = string.Empty;
        if ( !AllSettings.SettingsFileExists() )
            details = $"{AllSettings.Scribe.UserSettingsPath} not found.";
        else if ( AllSettings.TestSiteSettings is null || !AllSettings.TestSiteSettings.Exists )
            details = $"{nameof( AllSettings.TestSiteSettings )} not found.";
        else if ( AllSettings.IOSettings is null || !AllSettings.IOSettings.Exists )
            details = $"{nameof( AllSettings.IOSettings )} not found.";
        else if ( AllSettings.ResourceSettings is null || !AllSettings.ResourceSettings.Exists )
            details = $"{nameof( AllSettings.ResourceSettings )} not found.";
        else if ( AllSettings.CommandsSettings is null || !AllSettings.CommandsSettings.Exists )
            details = $"{nameof( AllSettings.CommandsSettings )} not found.";
        else if ( AllSettings.DigitalIOSettings is null || !AllSettings.DigitalIOSettings.Exists )
            details = $"{nameof( AllSettings.DigitalIOSettings )} not found.";
        else if ( AllSettings.SystemSubsystemSettings is null || !AllSettings.SystemSubsystemSettings.Exists )
            details = $"{nameof( AllSettings.SystemSubsystemSettings )} not found.";
        else if ( AllSettings.SenseResistanceSettings is null || !AllSettings.SenseResistanceSettings.Exists )
            details = $"{nameof( AllSettings.SenseResistanceSettings )} not found.";
        else if ( AllSettings.SourceResistanceSettings is null || !AllSettings.SourceResistanceSettings.Exists )
            details = $"{nameof( AllSettings.SourceResistanceSettings )} not found.";
        else if ( AllSettings.ResistanceSettings is null || !AllSettings.ResistanceSettings.Exists )
            details = $"{nameof( AllSettings.ResistanceSettings )} not found.";
        else if ( AllSettings.SenseCurrentSettings is null || !AllSettings.SenseCurrentSettings.Exists )
            details = $"{nameof( AllSettings.SenseCurrentSettings )} not found.";
        else if ( AllSettings.SourceCurrentSettings is null || !AllSettings.SourceCurrentSettings.Exists )
            details = $"{nameof( AllSettings.SourceCurrentSettings )} not found.";
        else if ( AllSettings.CurrentSourceMeasureSettings is null || !AllSettings.CurrentSourceMeasureSettings.Exists )
            details = $"{nameof( AllSettings.CurrentSourceMeasureSettings )} not found.";

        return details.Length == 0;
    }

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    internal static TestSiteSettings TestSiteSettings { get; private set; } = new();

    /// <summary>   Gets or sets the i/o settings. </summary>
    /// <value> The i/o settings. </value>
    internal static IOSettings IOSettings { get; private set; } = new();

    /// <summary>   Gets or sets the commands settings. </summary>
    /// <value> The commands settings. </value>
    internal static CommandsSettings CommandsSettings { get; private set; } = new();

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    internal static Pith.Settings.ResourceSettings ResourceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the digital i/o settings. </summary>
    /// <value> The digital i/o settings. </value>
    internal static DigitalIOSettings DigitalIOSettings { get; private set; } = new();

    /// <summary>   Gets or sets the device errors settings. </summary>
    /// <value> The device errors settings. </value>
    internal static DeviceErrorsSettings DeviceErrorsSettings { get; private set; } = new();

    /// <summary>   Gets or sets the system subsystem settings. </summary>
    /// <value> The system subsystem settings. </value>
    internal static VI.Settings.SystemSubsystemSettings SystemSubsystemSettings { get; private set; } = new();

    /// <summary>   Gets or sets the sense subsystem settings. </summary>
    /// <value> The sense subsystem settings. </value>
    internal static VI.Settings.SenseSubsystemSettings SenseSubsystemSettings { get; private set; } = new();

    /// <summary>   Gets or sets the sense resistance settings. </summary>
    /// <value> The sense resistance settings. </value>
    internal static SenseResistanceSettings SenseResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets source resistance settings. </summary>
    /// <value> The source resistance settings. </value>
    internal static SourceResistanceSettings SourceResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the resistance settings. </summary>
    /// <value> The resistance settings. </value>
    internal static ResistanceSettings ResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the sense current settings. </summary>
    /// <value> The sense current settings. </value>
    internal static SenseCurrentSettings SenseCurrentSettings { get; private set; } = new();

    /// <summary>   Gets or sets source current settings. </summary>
    /// <value> The source current settings. </value>
    internal static SourceCurrentSettings SourceCurrentSettings { get; private set; } = new();

    /// <summary>   Gets or sets the current source settings. </summary>
    /// <value> The current source settings. </value>
    internal static CurrentSourceMeasureSettings CurrentSourceMeasureSettings { get; private set; } = new();

    #endregion
}

