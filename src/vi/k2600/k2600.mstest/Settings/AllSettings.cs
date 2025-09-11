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
        AssemblyFileInfo settingsFileInfo = AllSettings.CreateSettingsFileInfo();

        AllSettings ti = new();
        ti.Scribe = ti.CreateScribe( settingsFileInfo );
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
        // Get the method declaring type for the assembly file information and the settings section name.
        Type declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!;

        // get assembly files using the .Settings suffix.

        AssemblyFileInfo ai = new( declaringType.Assembly, null, ".Settings", ".json" );

        // must copy application context settings here to clear any bad settings files.

        // must copy application context settings here to clear any bad settings files.

        AppSettingsScribe.InitializeSettingsFiles( ai, true, true );

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
            this.SenseVoltageSettings, this.SourceCurrentSettings, this.CurrentSourceMeasureSettings], settingsFileInfo );

        scribe.ReadSettings();

        if ( !this.SettingsExist( out string settingsClassName ) )
            throw new InvalidOperationException( $"{settingsClassName} not found or failed to read from {scribe.UserSettingsPath}." );

        return scribe;
    }

    /// <summary>   Gets or sets the <see cref="AppSettingsScribe">settings reader and writer</see>. </summary>
    /// <value> The scribe. </value>
    [JsonIgnore]
    public AppSettingsScribe? Scribe { get; private set; }

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
        if ( this.TestSiteSettings is null || !this.TestSiteSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.TestSiteSettings )}";
        else if ( this.IOSettings is null || !this.IOSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.IOSettings )}";
        else if ( this.ResourceSettings is null || !this.ResourceSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.ResourceSettings )}";
        else if ( this.CommandsSettings is null || !this.CommandsSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.CommandsSettings )}";
        else if ( this.DigitalIOSettings is null || !this.DigitalIOSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.DigitalIOSettings )}";
        else if ( this.SystemSubsystemSettings is null || !this.SystemSubsystemSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.SystemSubsystemSettings )}";
        else if ( this.SenseResistanceSettings is null || !this.SenseResistanceSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.SenseResistanceSettings )}";
        else if ( this.SourceResistanceSettings is null || !this.SourceResistanceSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.SourceResistanceSettings )}";
        else if ( this.ResistanceSettings is null || !this.ResistanceSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.ResistanceSettings )}";
        else if ( this.SenseVoltageSettings is null || !this.SenseVoltageSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.SenseVoltageSettings )}";
        else if ( this.SourceCurrentSettings is null || !this.SourceCurrentSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.SourceCurrentSettings )}";
        else if ( this.CurrentSourceMeasureSettings is null || !this.CurrentSourceMeasureSettings.Exists )
            settingsClassName = $"{nameof( AllSettings.CurrentSourceMeasureSettings )}";
        else
            settingsClassName = string.Empty;

        return settingsClassName.Length == 0;
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

