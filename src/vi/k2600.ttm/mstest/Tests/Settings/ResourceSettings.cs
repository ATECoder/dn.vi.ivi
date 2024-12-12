namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Settings;

/// <summary>   The Resource Settings class. </summary>
/// <remarks>   David, 2021-06/30. </remarks>
public class ResourceSettings : Device.MSTest.Settings.ResourceSettingsBase
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2023-05-09. </remarks>
    public ResourceSettings() : base()
    { }

    #endregion

    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( AllSettings.SettingsFileInfo.AllUsersAssemblyFilePath!,
            nameof( ResourceSettings ), AllSettings.ResourceSettings );
    }

    #endregion
}

