namespace cc.isr.VI.Tsp.K2600.MSTest.Subsystems;

/// <summary>   The Subsystems Test Settings class. </summary>
/// <remarks>   David, 2021-06/30. </remarks>
public class DigitalIOSettings : VI.Settings.DigitalIOSubsystemSettings
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2023-05-09. </remarks>
    public DigitalIOSettings() : base()
    { }

    #endregion

    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( DigitalIOSettings ), Settings.AllSettings.DigitalIOSettings );
    }

    #endregion
}

