namespace cc.isr.VI.Tsp.K2600.MSTest.Source;

/// <summary> Current Source Test Info. </summary>
/// <remarks> <para>
/// David, 2018-02-12 </para></remarks>
public class SourceCurrentSettings() : VI.Settings.SourceSubsystemSettings
{
    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.Instance.Scribe!.AllUsersSettingsPath!, nameof( SourceCurrentSettings ),
            Settings.AllSettings.Instance.SourceCurrentSettings,
            AppSettingsScribe.DefaultSerializerOptions, AppSettingsScribe.DefaultDocumentOptions );
    }

    #endregion

    #region " source subsystem information "

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
