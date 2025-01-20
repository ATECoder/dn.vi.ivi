namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Settings;

/// <summary>   Provides settings for all tests. </summary>
/// <remarks>   2023-04-24. </remarks>
internal sealed class TestSiteSettings() : isr.Std.Tests.TestSiteSettingsBase
{
    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( Settings.TestSiteSettings ), Settings.AllSettings.TestSiteSettings );
    }

    #endregion
}
