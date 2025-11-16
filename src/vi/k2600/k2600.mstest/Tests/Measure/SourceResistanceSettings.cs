namespace cc.isr.VI.Tsp.K2600.Tests.Measure;

/// <summary> A Resistance measurement Test settings. </summary>
/// <remarks> <para>
/// David, 2018-02-12 </para></remarks>
public class SourceResistanceSettings() : VI.Settings.SourceSubsystemSettings
{
    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Tests.Settings.AllSettings.Instance.Scribe!.AllUsersSettingsPath!, nameof( SourceResistanceSettings ),
            Tests.Settings.AllSettings.Instance.SourceResistanceSettings,
            AppSettingsScribe.DefaultSerializerOptions, AppSettingsScribe.DefaultDocumentOptions );
    }

    #endregion

    #region " TSP Settings "

    /// <summary>   Gets or sets source function. </summary>
    /// <value> The source function. </value>
    public SourceFunctionMode SourceFunction
    {
        get;
        set => this.SetProperty( ref field, value );
    } = SourceFunctionMode.CurrentDC;

    #endregion
}

