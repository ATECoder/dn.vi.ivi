using System;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings : Json.AppSettings.Settings.SettingsContainerBase
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

    /// <summary>   Creates a scribe. </summary>
    /// <remarks>   2025-01-13. </remarks>
    public override void CreateScribe()
    {
        this.LotSettings = new();
        this.Scribe = new( [this.LotSettings] );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    /// <param name="declaringType">        The type of the declaring entity. </param>
    /// <param name="settingsFileSuffix">   (Optional) [.settings] The suffix of the assembly settings file,
    ///                                     e.g., '.Settings' in 'cc.isr.VI.Tsp.K2600.Device.MSTest.Settings.json'
    ///                                     where cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name.</param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public override void ReadSettings( Type declaringType, string settingsFileSuffix = ".Settings",
        bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        base.ReadSettings( declaringType, settingsFileSuffix, overrideAllUsersFile, overrideThisUserFile );
    }

    #endregion

    #region " setting instances

    /// <summary>   Gets or sets the lot settings. </summary>
    /// <value> The lot settings. </value>
    public LotSettings LotSettings { get; private set; } = new();

    #endregion
}
