namespace cc.isr.VI.Tsp.K2600.MSTest.Settings;

/// <summary>   Provides settings for all tests. </summary>
/// <remarks>   2023-04-24. </remarks>
internal sealed class TestSiteSettings : isr.Std.Tests.TestSiteSettings
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
        isr.Std.Tests.TestSiteSettings.SettingsPath = Settings.AllSettings.Instance.Scribe!.AllUsersSettingsPath!;
#if false
        AppSettingsScribe.ReadSettings( Settings.AllSettings.Instance.Scribe!.AllUsersSettingsPath!, nameof( Settings.TestSiteSettings ),
            Settings.AllSettings.Instance.TestSiteSettings,
            AppSettingsScribe.DefaultSerializerOptions, AppSettingsScribe.DefaultDocumentOptions );
#endif
        base.ReadSettings();
    }

    #endregion
}
