namespace cc.isr.VI.Tsp.K2600.MSTest.Measure;

/// <summary> A Resistance measurement Test settings. </summary>
/// <remarks> <para>
/// David, 2018-02-12 </para></remarks>
public class ResistanceSettings : VI.Device.MSTest.Settings.SubsystemsSettingsBase
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2023-05-09. </remarks>
    public ResistanceSettings()
    { }

    #endregion

    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( ResistanceSettings ), Settings.AllSettings.ResistanceSettings );
    }

    #endregion

    #region " measure subsystem information "

    private bool _autoZeroEnabled = true;

    /// <summary>   Gets or sets a value indicating whether the automatic zero is enabled. </summary>
    /// <value> True if automatic zero enabled, false if not. </value>
	public bool AutoZeroEnabled
    {
        get => this._autoZeroEnabled;
        set => this.SetProperty( ref this._autoZeroEnabled, value );
    }

    private bool _autoRangeEnabled = true;

    /// <summary>
    /// Gets or sets a value indicating whether the automatic range is enabled.
    /// </summary>
    /// <value> True if automatic range enabled, false if not. </value>
	public bool AutoRangeEnabled
    {
        get => this._autoRangeEnabled;
        set => this.SetProperty( ref this._autoRangeEnabled, value );
    }

    private SenseFunctionModes _senseFunction = SenseFunctionModes.ResistanceFourWire;

    /// <summary>   Gets or sets the sense function. </summary>
    /// <value> The sense function. </value>
	public SenseFunctionModes SenseFunction
    {
        get => this._senseFunction;
        set => this.SetProperty( ref this._senseFunction, value );
    }

    private double _powerLineCycles = 1;

    /// <summary>   Gets or sets the power line cycles. </summary>
    /// <value> The power line cycles. </value>
	public double PowerLineCycles
    {
        get => this._powerLineCycles;
        set => this.SetProperty( ref this._powerLineCycles, value );
    }


    private bool _remoteSenseSelected = true;

    /// <summary>   Gets or sets a value indicating whether the remote sense selected. </summary>
    /// <value> True if remote sense selected, false if not. </value>
	public bool RemoteSenseSelected
    {
        get => this._remoteSenseSelected;
        set => this.SetProperty( ref this._remoteSenseSelected, value );
    }

    #endregion

    #region " source subsystem information "

    private bool _sourceReadBackEnabled = true;

    /// <summary>
    /// Gets or sets a value indicating whether the source read back is enabled.
    /// </summary>
    /// <value> True if source read back enabled, false if not. </value>
	public bool SourceReadBackEnabled
    {
        get => this._sourceReadBackEnabled;
        set => this.SetProperty( ref this._sourceReadBackEnabled, value );
    }

    private bool _frontTerminalsSelected = true;

    /// <summary>   Gets or sets a value indicating whether the front terminals selected. </summary>
    /// <value> True if front terminals selected, false if not. </value>
	public bool FrontTerminalsSelected
    {
        get => this._frontTerminalsSelected;
        set => this.SetProperty( ref this._frontTerminalsSelected, value );
    }

    private SourceFunctionMode _sourceFunction = SourceFunctionMode.CurrentDC;

    /// <summary>   Gets or sets source function. </summary>
    /// <value> The source function. </value>
	public SourceFunctionMode SourceFunction
    {
        get => this._sourceFunction;
        set => this.SetProperty( ref this._sourceFunction, value );
    }

    private double _sourceLevel = 0.02;

    /// <summary>   Gets or sets source level. </summary>
    /// <value> The source level. </value>
	public double SourceLevel
    {
        get => this._sourceLevel;
        set => this.SetProperty( ref this._sourceLevel, value );
    }

    #endregion

    #region " resistance information "

    private double _expectedResistance = 100;

    /// <summary>   Gets or sets the expected resistance. </summary>
    /// <value> The expected resistance. </value>
	public double ExpectedResistance
    {
        get => this._expectedResistance;
        set => this.SetProperty( ref this._expectedResistance, value );
    }

    private double _resistanceTolerance = 0.01;

    /// <summary>   Gets or sets the resistance tolerance. </summary>
    /// <value> The resistance tolerance. </value>
	public double ResistanceTolerance
    {
        get => this._resistanceTolerance;
        set => this.SetProperty( ref this._resistanceTolerance, value );
    }

    #endregion
}

