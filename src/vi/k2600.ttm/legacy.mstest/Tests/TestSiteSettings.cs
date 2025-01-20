using cc.isr.Json.AppSettings.Models;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

/// <summary>   Provides settings for all tests. </summary>
/// <remarks>   2024-11-01. </remarks>
internal sealed class TestSiteSettings() : Std.Tests.TestSiteSettingsBase
{
    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( AllSettings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( TestSiteSettings ), AllSettings.TestSiteSettings );
    }

    #endregion
}
