using System;
using System.Text.Json.Serialization;
using cc.isr.Json.AppSettings.Models;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

/// <summary>   Settings for testing the legacy driver. </summary>
/// <remarks>   David, 2024-11-01. </remarks>
public class AllSettings
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    /// <remarks>   2023-04-24. </remarks>
    private AllSettings() { }

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
        AssemblyFileInfo settingsFileInfo = AllSettings.CreateSettingsFileInfo();
        ti.Scribe = ti.CreateScribe( settingsFileInfo );
        // AppSettingsScribe.ReadSettings( SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( AllSettings ), ti );
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


    #endregion

    #region " settings scribe "

    /// <summary>
    /// Creates an instance of the <see cref="AllSettings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="settingsFileInfo"> Information describing the settings file. </param>
    /// <returns>   The new instance. </returns>
    private AppSettingsScribe CreateScribe( AssemblyFileInfo settingsFileInfo )
    {
        AppSettingsScribe scribe = new( [this.TestSiteSettings],
            settingsFileInfo.AppContextAssemblyFilePath!, settingsFileInfo.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = settingsFileInfo.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = settingsFileInfo.ThisUserAssemblyFilePath
        };

        scribe.ReadSettings();

        if ( !System.IO.File.Exists( scribe.UserSettingsPath ) )
            throw new InvalidOperationException( $"{nameof( AllSettings )} settings file {scribe.UserSettingsPath} not found." );
        else if ( !this.SettingsExist( out string details ) )
            throw new InvalidOperationException( details );

        return scribe;
    }

    /// <summary>   Gets or sets the <see cref="AppSettingsScribe">settings reader and writer</see>. </summary>
    /// <value> The scribe. </value>
    [JsonIgnore]
    public AppSettingsScribe? Scribe { get; private set; }

    /// <summary>   Gets the full pathname of the settings file. </summary>
    /// <value> The full pathname of the settings file. </value>
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
    /// <returns>   A Tuple. </returns>
    public bool SettingsExist( out string details )
    {
        if ( this.TestSiteSettings is null || !this.TestSiteSettings.Exists )
            details = $"{nameof( this.TestSiteSettings )} not found.";
        else
            details = string.Empty;

        return details.Length == 0;
    }

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    internal TestSiteSettings TestSiteSettings { get; private set; } = new();

    #endregion
}

