using cc.isr.Json.AppSettings.Models;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

/// <summary>   Provides settings for all tests. </summary>
/// <remarks>   2024-11-01. </remarks>
internal sealed class TestSiteSettings() : cc.isr.Std.Tests.TestSiteSettings
{
    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public override void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( AllSettings.Instance.Scribe!.AllUsersSettingsPath!, nameof( AllSettings.TestSiteSettings ), this,
            AppSettingsScribe.DefaultSerializerOptions, AppSettingsScribe.DefaultDocumentOptions );
    }

    /// <summary>   Saves the settings. </summary>
    /// <remarks>   2025-03-28. </remarks>
    public override void SaveSettings()
    {
        AppSettingsScribe.WriteSettings( AllSettings.Instance.Scribe!.AllUsersSettingsPath!, nameof( AllSettings.TestSiteSettings ), this,
            AppSettingsScribe.DefaultSerializerOptions );
    }

    #endregion
}
