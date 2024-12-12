using System;

namespace cc.isr.VI.DeviceWinControls.MSTest;

/// <summary>   Provides settings for all tests. </summary>
/// <remarks>   2023-04-24. </remarks>
internal sealed class TestSiteSettings : cc.isr.Std.Tests.TestSiteSettingsBase
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2023-05-09. </remarks>
    public TestSiteSettings()
    { }

    #endregion

    #region " singleton "

    /// <summary>   Creates an instance of the <see cref="TestSiteSettings"/> after restoring the 
    /// application context settings to both the user and all user files. </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static TestSiteSettings CreateInstance()
    {
        TestSiteSettings ti = new();
        AppSettingsScribe.ReadSettings( Settings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( TestSiteSettings ), ti );
        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static TestSiteSettings Instance => _instance.Value;

    private static readonly Lazy<TestSiteSettings> _instance = new( CreateInstance, true );

    #endregion
}
