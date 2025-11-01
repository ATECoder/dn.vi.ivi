using System.Text.Json.Serialization;

namespace cc.isr.VI.Pith.Settings;

/// <summary>   A settings container class for all settings. </summary>
/// <remarks>   2024-10-16. </remarks>
public class AllSettings : Json.AppSettings.Settings.SettingsContainerBase
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    /// <remarks>   2023-04-24. </remarks>
    public AllSettings() { }

    #endregion

    #region " singleton "

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static AllSettings Instance => _instance.Value;

    private static readonly Lazy<AllSettings> _instance = new( () => new AllSettings(), true );

    #endregion

    #region " create scribe "

    /// <summary>   Creates a scribe. </summary>
    /// <remarks>   2025-01-13. </remarks>
    public override void CreateScribe()
    {
        this.TimingSettings = new();
        this.CommandsSettings = new();
        this.IOSettings = new();
        this.RegistersBitmasksSettings = new();
        this.ScpiExceptionsSettings = new();
        this.ResourceSettings = new();

        this.Scribe = new( [this.TimingSettings, this.CommandsSettings, this.IOSettings,
            this.RegistersBitmasksSettings, this.ScpiExceptionsSettings, this.ResourceSettings] );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>
    /// 2024-08-17. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="declaringType">        The Type of the declaring object. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.json' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings
    ///                                     file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings
    ///                                     file. </param>
    public override void ReadSettings( Type declaringType, string settingsFileSuffix = ".session",
        bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        base.ReadSettings( declaringType, settingsFileSuffix, overrideAllUsersFile, overrideThisUserFile );
    }

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the timing settings. </summary>
    /// <value> The timing settings. </value>
    [JsonIgnore, CLSCompliant( false )]
    public Settings.TimingSettings TimingSettings { get; private set; } = new Settings.TimingSettings();

    /// <summary>   Gets or sets the commands settings. </summary>
    /// <value> The commands settings. </value>
    [JsonIgnore, CLSCompliant( false )]
    public Settings.CommandsSettings CommandsSettings { get; private set; } = new Settings.CommandsSettings();

    /// <summary>   Gets or sets the i/o settings. </summary>
    /// <value> The i/o settings. </value>
    [JsonIgnore, CLSCompliant( false )]
    public Settings.IOSettings IOSettings { get; private set; } = new Settings.IOSettings();

    /// <summary>   Gets or sets the registers bitmasks settings. </summary>
    /// <value> The register settings. </value>
    [JsonIgnore, CLSCompliant( false )]
    public Settings.RegistersBitmasksSettings RegistersBitmasksSettings { get; private set; } = new Settings.RegistersBitmasksSettings();

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    [JsonIgnore, CLSCompliant( false )]
    public Settings.ResourceSettings ResourceSettings { get; private set; } = new Settings.ResourceSettings();

    /// <summary>   Gets or sets the SCPI exceptions settings. </summary>
    /// <value> The SCPI exceptions settings. </value>
    [CLSCompliant( false )]
    public Settings.ScpiExceptionsSettings ScpiExceptionsSettings { get; private set; } = new Settings.ScpiExceptionsSettings();

    #endregion
}

