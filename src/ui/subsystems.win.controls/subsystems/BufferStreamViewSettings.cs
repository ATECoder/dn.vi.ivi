using System;
using System.ComponentModel;
using cc.isr.Json.AppSettings.Models;
using System.Text.Json.Serialization;
using cc.isr.Json.AppSettings.ViewModels;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class BufferStreamViewSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2025-01-16. </remarks>
    public BufferStreamViewSettings() { }

    #endregion

    #region " singleton "

    /// <summary>
    /// Creates an instance of the <see cref="BufferStreamViewSettings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static BufferStreamViewSettings CreateInstance()
    {
        BufferStreamViewSettings ti = new();
        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static BufferStreamViewSettings Instance => _instance.Value;

    private static readonly Lazy<BufferStreamViewSettings> _instance = new( CreateInstance, true );

    #endregion

    #region " scribe "

    /// <summary>   Gets or sets the scribe. </summary>
    /// <value> The scribe. </value>
    [JsonIgnore]
    public AppSettingsScribe? Scribe { get; set; }

    /// <summary>   Initializes and reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void Initialize( Type callingEntity, string settingsFileSuffix )
    {
        AssemblyFileInfo ai = new( callingEntity.Assembly, null, settingsFileSuffix, ".json" );

        // must copy application context settings here to clear any bad settings files.
        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! );
        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.ThisUserAssemblyFilePath! );

        this.Scribe = new( [_instance.Value], ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = ai.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = ai.ThisUserAssemblyFilePath
        };

        this.Scribe.ReadSettings();

        if ( _instance.Value is null || !_instance.Value.Exists )
            throw new InvalidOperationException( $"{nameof( BufferStreamViewSettings )} was not found." );
    }

    /// <summary>   Check if the settings file exits. </summary>
    /// <remarks>   2024-07-06. </remarks>
    /// <returns>   True if it the settings file exists; otherwise false. </returns>
    public bool SettingsFileExists()
    {
        return this.Scribe is not null && System.IO.File.Exists( this.Scribe.UserSettingsPath );
    }

    #endregion

    #region " exists "

    private bool _exists;

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
	[Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get => this._exists;
        set => _ = this.SetProperty( ref this._exists, value );
    }

    #endregion

    #region " settings "

    private SenseFunctionModes _streamBufferSenseFunctionMode = SenseFunctionModes.ResistanceFourWire;

    /// <summary>   Gets or sets the stream buffer sense function mode. </summary>
    /// <value> The stream buffer sense function mode. </value>
	public SenseFunctionModes StreamBufferSenseFunctionMode
    {
        get => this._streamBufferSenseFunctionMode;
        set => _ = this.SetProperty( ref this._streamBufferSenseFunctionMode, value );
    }

    private double _nominalResistance = 100;

    /// <summary>   Gets or sets the nominal resistance. </summary>
    /// <value> The nominal resistance. </value>
	public double NominalResistance
    {
        get => this._nominalResistance;
        set => _ = this.SetProperty( ref this._nominalResistance, value );
    }

    private double _resistanceTolerance = 0.01;

    /// <summary>   Gets or sets the resistance tolerance. </summary>
    /// <value> The resistance tolerance. </value>
	public double ResistanceTolerance
    {
        get => this._resistanceTolerance;
        set => _ = this.SetProperty( ref this._resistanceTolerance, value );
    }

    private double _openLimit = 1000;

    /// <summary>   Gets or sets the open limit. </summary>
    /// <value> The open limit. </value>
	public double OpenLimit
    {
        get => this._openLimit;
        set => _ = this.SetProperty( ref this._openLimit, value );
    }

    private int _passBitmask = 1;

    /// <summary>   Gets or sets the pass bitmask. </summary>
    /// <value> The pass bitmask. </value>
	public int PassBitmask
    {
        get => this._passBitmask;
        set => _ = this.SetProperty( ref this._passBitmask, value );
    }

    private int _failBitmask = 2;

    /// <summary>   Gets or sets the fail bitmask. </summary>
    /// <value> The fail bitmask. </value>
	public int FailBitmask
    {
        get => this._failBitmask;
        set => _ = this.SetProperty( ref this._failBitmask, value );
    }

    private int _overflowBitmask = 4;

    /// <summary>   Gets or sets the overflow bitmask. </summary>
    /// <value> The overflow bitmask. </value>
	public int OverflowBitmask
    {
        get => this._overflowBitmask;
        set => _ = this.SetProperty( ref this._overflowBitmask, value );
    }

    private int _binningStrobeDuration = 10;

    /// <summary>   Gets or sets the duration of the binning strobe. </summary>
    /// <value> The binning strobe duration. </value>
	public int BinningStrobeDuration
    {
        get => this._binningStrobeDuration;
        set => _ = this.SetProperty( ref this._binningStrobeDuration, value );
    }

    private int _streamTriggerCount = 999;

    /// <summary>   Gets or sets the number of stream triggers. </summary>
    /// <value> The number of stream triggers. </value>
	public int StreamTriggerCount
    {
        get => this._streamTriggerCount;
        set => _ = this.SetProperty( ref this._streamTriggerCount, value );
    }

    private ArmSources _streamBufferArmSource = ArmSources.Bus;

    /// <summary>   Gets or sets the stream buffer arm source. </summary>
    /// <value> The stream buffer arm source. </value>
	public ArmSources StreamBufferArmSource
    {
        get => this._streamBufferArmSource;
        set => _ = this.SetProperty( ref this._streamBufferArmSource, value );
    }

    private TriggerSources _streamBufferTriggerSource = TriggerSources.Bus;

    /// <summary>   Gets or sets the stream buffer trigger source. </summary>
    /// <value> The stream buffer trigger source. </value>
	public TriggerSources StreamBufferTriggerSource
    {
        get => this._streamBufferTriggerSource;
        set => _ = this.SetProperty( ref this._streamBufferTriggerSource, value );
    }

    private int _bufferStreamPollInterval = 50;

    /// <summary>   Gets or sets the buffer stream poll interval. </summary>
    /// <value> The buffer stream poll interval. </value>
	public int BufferStreamPollInterval
    {
        get => this._bufferStreamPollInterval;
        set => _ = this.SetProperty( ref this._bufferStreamPollInterval, value );
    }

    private int _scanCardSampleCount = 3;

    /// <summary>   Gets or sets the number of scan card samples. </summary>
    /// <value> The number of scan card samples. </value>
	public int ScanCardSampleCount
    {
        get => this._scanCardSampleCount;
        set => _ = this.SetProperty( ref this._scanCardSampleCount, value );
    }

    private string _scanCardScanList = "(@1:3)";

    /// <summary>   Gets or sets a list of scan card scans. </summary>
    /// <value> A list of scan card scans. </value>
	public string ScanCardScanList
    {
        get => this._scanCardScanList;
        set => _ = this.SetProperty( ref this._scanCardScanList, value );
    }

    #endregion

    #region " Settings editor "

    /// <summary>   Opens the settings editor. </summary>
    /// <remarks>   David, 2021-12-08. <para>
    /// The settings <see cref="BufferStreamViewSettings.Initialize(Type, string)"/></para> must be called before attempting to edit the settings. </remarks>
    /// <returns>   A System.Windows.Forms.DialogResult. </returns>
    public DialogResult OpenSettingsEditor()
    {
        Form form = new Json.AppSettings.WinForms.JsonSettingsEditorForm( "Buffer Stream View Settings Editor",
            new AppSettingsEditorViewModel( this.Scribe!, Json.AppSettings.Services.SimpleServiceProvider.GetInstance() ) );
        return form.ShowDialog();
    }

    #endregion
}
