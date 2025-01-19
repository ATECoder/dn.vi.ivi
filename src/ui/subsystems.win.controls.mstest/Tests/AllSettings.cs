using System;

namespace cc.isr.VI.SubsystemsWinControls.Tests;

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

    private static readonly Lazy<AllSettings> _instance = new( AllSettings.CreateInstance, true );

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

    private static readonly Lazy<AssemblyFileInfo> _settingsFileInfo = new( AllSettings.CreateSettingsFileInfo, true );

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

        AppSettingsScribe scribe = new( [TestSiteSettings, BufferStreamViewSettings, DigitalOutputViewSettings],
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

    private static readonly Lazy<AppSettingsScribe> _scribe = new( AllSettings.CreateScribe, true );

    /// <summary>   Gets the full path name of the settings file. </summary>
    /// <value> The full path name of the settings file. </value>
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
        else if ( AllSettings.BufferStreamViewSettings is null || !AllSettings.BufferStreamViewSettings.Exists )
            details = $"{nameof( AllSettings.BufferStreamViewSettings )} not found.";
        else if ( AllSettings.DigitalOutputViewSettings is null || !AllSettings.DigitalOutputViewSettings.Exists )
            details = $"{nameof( AllSettings.DigitalOutputViewSettings )} not found.";

        return details.Length == 0;
    }

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    internal static TestSiteSettings TestSiteSettings { get; private set; } = new();

    /// <summary>   Gets or sets the buffer stream view settings. </summary>
    /// <value> The buffer stream view settings. </value>
    internal static BufferStreamViewSettings BufferStreamViewSettings { get; private set; } = new();

    /// <summary>   Gets or sets the digital output view settings. </summary>
    /// <value> The digital output view settings. </value>
    internal static DigitalOutputViewSettings DigitalOutputViewSettings { get; private set; } = new();

    #endregion
}

