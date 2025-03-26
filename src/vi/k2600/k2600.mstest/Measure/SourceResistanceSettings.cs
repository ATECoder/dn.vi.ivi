namespace cc.isr.VI.Tsp.K2600.MSTest.Measure;

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
        AppSettingsScribe.ReadSettings( Settings.AllSettings.Instance.Scribe!.AllUsersSettingsPath!, nameof( SourceResistanceSettings ),
            Settings.AllSettings.Instance.SourceResistanceSettings,
            AppSettingsScribe.DefaultSerializerOptions, AppSettingsScribe.DefaultDocumentOptions );
    }

    #endregion

    #region " TSP Settings "

    private SourceFunctionMode _sourceFunction = SourceFunctionMode.CurrentDC;

    /// <summary>   Gets or sets source function. </summary>
    /// <value> The source function. </value>
	public SourceFunctionMode SourceFunction
    {
        get => this._sourceFunction;
        set => this.SetProperty( ref this._sourceFunction, value );
    }

    #endregion
}

