using System;
using cc.isr.Json.AppSettings.Models;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings
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

    /// <summary>   Gets or sets the scribe. </summary>
    /// <value> The scribe. </value>
    public AppSettingsScribe? Scribe { get; set; }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public void ReadSettings( Type callingEntity, string settingsFileSuffix, bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        AssemblyFileInfo ai = new( callingEntity.Assembly, null, settingsFileSuffix, ".json" );

        // copy application context settings if these do not exist; use restore if the settings are bad.

        AppSettingsScribe.InitializeSettingsFiles( ai, overrideAllUsersFile, overrideThisUserFile );

        this.Scribe = new( [this.LotSettings],
            ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = ai.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = ai.ThisUserAssemblyFilePath
        };
        this.Scribe.ReadSettings();
    }

    #endregion

    #region " setting instances

    /// <summary>   Gets or sets the lot settings. </summary>
    /// <value> The lot settings. </value>
    public LotSettings LotSettings { get; private set; } = new();

    #endregion
}
