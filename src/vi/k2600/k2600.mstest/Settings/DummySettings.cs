namespace cc.isr.VI.Tsp.K2600.MSTest.Settings;

/// <summary>   A dummy settings. </summary>
/// <remarks>   2024-08-03. </remarks>
internal sealed class DummySettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " Settings "

    private bool _checkUnpublishedMessageLogFileSize;

    /// <summary>   Gets or sets a value indicating whether the option to check unpublished message log file size is enabled. </summary>
    /// <value> True if enabled, false if not. </value>
    [System.ComponentModel.Description( "True if the unpublished message log file size is to be checked." )]
    public bool CheckUnpublishedMessageLogFileSize
    {
        get => this._checkUnpublishedMessageLogFileSize;
        set => _ = this.SetProperty( ref this._checkUnpublishedMessageLogFileSize, value );
    }

    private string _dummy = "Dummy";

    /// <summary> Gets or sets the candidate time zones of this location. </summary>
    /// <value> The candidate time zones of the test site. </value>
    [System.ComponentModel.Description( "Specifies a dummy name." )]
    public string Dummy
    {
        get => this._dummy;
        set => _ = this.SetProperty( ref this._dummy, value );
    }

    #endregion

    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( DummySettings ), Settings.AllSettings.DummySettings );
    }

    #endregion
}
