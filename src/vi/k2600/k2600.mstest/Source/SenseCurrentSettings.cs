namespace cc.isr.VI.Tsp.K2600.MSTest.Source;

/// <summary> Current Source Test Info. </summary>
/// <remarks> <para>
/// David, 2018-02-12 </para></remarks>
public class SenseCurrentSettings() : VI.Settings.SenseSubsystemSettings
{
    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( CurrentSourceMeasureSettings ), Settings.AllSettings.CurrentSourceMeasureSettings );
    }

    #endregion
}
