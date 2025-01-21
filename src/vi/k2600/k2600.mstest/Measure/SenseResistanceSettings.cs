namespace cc.isr.VI.Tsp.K2600.MSTest.Measure;

/// <summary> A Resistance measurement Test settings. </summary>
/// <remarks> <para>
/// David, 2018-02-12 </para></remarks>
public class SenseResistanceSettings() : VI.Settings.SenseSubsystemSettings
{
    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.Instance.Scribe!.AllUsersSettingsPath!, nameof( SenseResistanceSettings ), Settings.AllSettings.Instance.SenseResistanceSettings );
    }

    #endregion
}

