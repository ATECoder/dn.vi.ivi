namespace cc.isr.VI.Tsp.K2600.MSTest.Subsystems;

/// <summary>   The Subsystems Test Settings class. </summary>
/// <remarks>   David, 2021-06/30. </remarks>
public class SubsystemsSettings : VI.Device.MSTest.Settings.SubsystemsSettingsBase
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2023-05-09. </remarks>
    public SubsystemsSettings() : base()
    { }

    #endregion

    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( SubsystemsSettings ), Settings.AllSettings.SubsystemsSettings );
    }

    #endregion

    #region " digital i/o subsystem settings "

    private int _digitalOutputLineNumber = 1;

    /// <summary>   Gets or sets the digital output line number. </summary>
    /// <value> The digital output line number. </value>
	public virtual int DigitalOutputLineNumber
    {
        get => this._digitalOutputLineNumber;
        set => this.SetProperty( ref this._digitalOutputLineNumber, value );
    }

    private int _digitalInputLineNumber = 2;

    /// <summary>   Gets or sets the digital input line number. </summary>
    /// <value> The digital input line number. </value>
	public virtual int DigitalInputLineNumber
    {
        get => this._digitalInputLineNumber;
        set => this.SetProperty( ref this._digitalInputLineNumber, value );
    }

    private int _digitalLineCount = 6;

    /// <summary>   Gets or sets the number of digital lines. </summary>
    /// <value> The number of digital lines. </value>
	public virtual int DigitalLineCount
    {
        get => this._digitalLineCount;
        set => this.SetProperty( ref this._digitalLineCount, value );
    }

    #endregion
}

