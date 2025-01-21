namespace cc.isr.VI.Tsp.K2600.MSTest.Source;

/// <summary> Current Source sense voltage settings. </summary>
/// <remarks> <para>
/// David, 2018-02-12 </para></remarks>
public class SenseVoltageSettings() : VI.Settings.SenseSubsystemSettings
{
    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.Instance.Scribe!.AllUsersSettingsPath!, nameof( SenseVoltageSettings ), Settings.AllSettings.Instance.SenseVoltageSettings );
    }

    #endregion
}
