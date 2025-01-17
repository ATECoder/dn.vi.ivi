namespace cc.isr.VI.WinControls.Tests;

/// <summary>   Provides settings for all tests. </summary>
/// <remarks>   2023-04-24. </remarks>
internal sealed class TestSiteSettings : Std.Tests.TestSiteSettingsBase
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
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( AllSettings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( TestSiteSettings ), AllSettings.TestSiteSettings );
    }

    #endregion
}
