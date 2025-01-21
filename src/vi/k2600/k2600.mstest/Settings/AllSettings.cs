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
    /// <remarks>   2025-01-20. </remarks>
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
        AppSettingsScribe scribe = new( [this.TestSiteSettings,
            this.CommandsSettings, this.DeviceErrorsSettings, this.DigitalIOSettings,
            this.IOSettings, this.ResourceSettings, this.SystemSubsystemSettings,
            this.SenseResistanceSettings, this.SourceResistanceSettings, this.ResistanceSettings,
            this.SenseVoltageSettings, this.SourceCurrentSettings, this.CurrentSourceMeasureSettings],
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
        else if ( this.IOSettings is null || !this.IOSettings.Exists )
            details = $"{nameof( this.IOSettings )} not found.";
        else if ( this.ResourceSettings is null || !this.ResourceSettings.Exists )
            details = $"{nameof( this.ResourceSettings )} not found.";
        else if ( this.CommandsSettings is null || !this.CommandsSettings.Exists )
            details = $"{nameof( this.CommandsSettings )} not found.";
        else if ( this.DigitalIOSettings is null || !this.DigitalIOSettings.Exists )
            details = $"{nameof( this.DigitalIOSettings )} not found.";
        else if ( this.SystemSubsystemSettings is null || !this.SystemSubsystemSettings.Exists )
            details = $"{nameof( this.SystemSubsystemSettings )} not found.";
        else if ( this.SenseResistanceSettings is null || !this.SenseResistanceSettings.Exists )
            details = $"{nameof( this.SenseResistanceSettings )} not found.";
        else if ( this.SourceResistanceSettings is null || !this.SourceResistanceSettings.Exists )
            details = $"{nameof( this.SourceResistanceSettings )} not found.";
        else if ( this.ResistanceSettings is null || !this.ResistanceSettings.Exists )
            details = $"{nameof( this.ResistanceSettings )} not found.";
        else if ( this.SenseVoltageSettings is null || !this.SenseVoltageSettings.Exists )
            details = $"{nameof( this.SenseVoltageSettings )} not found.";
        else if ( this.SourceCurrentSettings is null || !this.SourceCurrentSettings.Exists )
            details = $"{nameof( this.SourceCurrentSettings )} not found.";
        else if ( this.CurrentSourceMeasureSettings is null || !this.CurrentSourceMeasureSettings.Exists )
            details = $"{nameof( this.CurrentSourceMeasureSettings )} not found.";
        else
            details = string.Empty;

        return details.Length == 0;
    }

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    internal TestSiteSettings TestSiteSettings { get; private set; } = new();

    /// <summary>   Gets or sets the i/o settings. </summary>
    /// <value> The i/o settings. </value>
    internal IOSettings IOSettings { get; private set; } = new();

    /// <summary>   Gets or sets the commands settings. </summary>
    /// <value> The commands settings. </value>
    internal CommandsSettings CommandsSettings { get; private set; } = new();

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    internal Pith.Settings.ResourceSettings ResourceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the digital i/o settings. </summary>
    /// <value> The digital i/o settings. </value>
    internal DigitalIOSettings DigitalIOSettings { get; private set; } = new();

    /// <summary>   Gets or sets the device errors settings. </summary>
    /// <value> The device errors settings. </value>
    internal DeviceErrorsSettings DeviceErrorsSettings { get; private set; } = new();

    /// <summary>   Gets or sets the system subsystem settings. </summary>
    /// <value> The system subsystem settings. </value>
    internal VI.Settings.SystemSubsystemSettings SystemSubsystemSettings { get; private set; } = new();

    /// <summary>   Gets or sets the sense subsystem settings. </summary>
    /// <value> The sense subsystem settings. </value>
    internal VI.Settings.SenseSubsystemSettings SenseSubsystemSettings { get; private set; } = new();

    /// <summary>   Gets or sets the sense resistance settings. </summary>
    /// <value> The sense resistance settings. </value>
    internal SenseResistanceSettings SenseResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets source resistance settings. </summary>
    /// <value> The source resistance settings. </value>
    internal SourceResistanceSettings SourceResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the resistance settings. </summary>
    /// <value> The resistance settings. </value>
    internal ResistanceSettings ResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the sense voltage settings. </summary>
    /// <value> The sense voltage settings. </value>
    internal SenseVoltageSettings SenseVoltageSettings { get; private set; } = new();

    /// <summary>   Gets or sets source current settings. </summary>
    /// <value> The source current settings. </value>
    internal SourceCurrentSettings SourceCurrentSettings { get; private set; } = new();

    /// <summary>   Gets or sets the current source settings. </summary>
    /// <value> The current source settings. </value>
    internal CurrentSourceMeasureSettings CurrentSourceMeasureSettings { get; private set; } = new();

    #endregion
}

