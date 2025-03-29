using cc.isr.Json.AppSettings.Models;

namespace cc.isr.VI.WinControls.Tests;

/// <summary>   Provides settings for all tests. </summary>
/// <remarks>   2023-04-24. </remarks>
internal sealed class TestSiteSettings : Std.Tests.TestSiteSettings
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2023-05-09. </remarks>
    public TestSiteSettings()
    { }

    #endregion

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
