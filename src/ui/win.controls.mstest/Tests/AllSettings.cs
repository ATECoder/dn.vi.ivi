namespace cc.isr.VI.WinControls.Tests;

/// <summary>   A settings container class for all settings. </summary>
/// <remarks>   2024-10-16. </remarks>
public class AllSettings : cc.isr.Json.AppSettings.Settings.SettingsContainerBase
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
        this.LocationSettings = new();
        this.TraceLogSettings = new();
        this.Scribe = new( [this.LocationSettings, this.TraceLogSettings] );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-17. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="declaringType">        The Type of the declaring object. </param>
    /// <param name="settingsFileSuffix">   (Optional) [.settings] The suffix of the assembly settings file,
    ///                                     e.g., '.settings' in 'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.json'
    ///                                     where cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name.<,/param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public override void ReadSettings( Type declaringType, string settingsFileSuffix = ".settings",
        bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        base.ReadSettings( declaringType, settingsFileSuffix, overrideAllUsersFile, overrideThisUserFile );
    }

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the location settings. </summary>
    /// <value> The location settings. </value>
    internal cc.isr.Json.AppSettings.Settings.LocationSettings LocationSettings { get; private set; } = new();

    /// <summary>   Gets or sets the <see cref="Properties.Settings"/>. </summary>
    /// <value> The Win Controls settings. </value>
    internal cc.isr.Logging.TraceLog.TraceLogSettings TraceLogSettings { get; private set; } = new();

    #endregion
}

