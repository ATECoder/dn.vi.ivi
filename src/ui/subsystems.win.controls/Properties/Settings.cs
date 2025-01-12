using System;
using System.ComponentModel;
using System.Diagnostics;
using cc.isr.Json.AppSettings.Models;
using System.Text.Json.Serialization;

namespace cc.isr.VI.SubsystemsWinControls.Properties;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    /// <remarks>   2023-04-24. </remarks>
    public Settings() { }

    #endregion

    #region " singleton "

    /// <summary>
    /// Creates an instance of the <see cref="Settings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static Settings CreateInstance()
    {
        Settings ti = new();
        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static Settings Instance => _instance.Value;

    private static readonly Lazy<Settings> _instance = new( CreateInstance, true );

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
    public void Initialize( System.Type callingEntity, string settingsFileSuffix )
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

    #region " configuration information "

    private TraceLevel _messageLevel = TraceLevel.Off;

    /// <summary>   Gets or sets the trace level. </summary>
    /// <remarks>
    /// This property name is different from the <see cref="System.Text.Json"/> property name in
    /// order to ensure that the class is correctly serialized. It's value is initialized as <see cref="TraceLevel.Off"/>
    /// in order to test the reading from the settings file.
    /// </remarks>
    /// <value> The message <see cref="TraceLevel"/>. </value>
    [System.ComponentModel.Description( "Sets the message level" )]
    [JsonPropertyName( "TraceLevel" )]
    public TraceLevel MessageLevel
    {
        get => this._messageLevel;
        set => _ = this.SetProperty( ref this._messageLevel, value );
    }

    #endregion

    #region " values "

    private TraceEventType _messageDisplayLevel = TraceEventType.Verbose;

    /// <summary>   Gets or sets the display level for log and trace messages. </summary>
    /// <remarks>
    /// The maximum trace event type for displaying logged and trace events. Only messages with a
    /// message <see cref="System.Diagnostics.TraceEventType"/> level that is same or higher than
    /// this level are displayed.
    /// </remarks>
    /// <value> The message display level. </value>
	[Description( "The maximum trace event type for displaying logged and trace events. Only messages with a message a level that is same or higher than this level are displayed." )]
    public TraceEventType MessageDisplayLevel
    {
        get => this._messageDisplayLevel;
        set => this.SetProperty( ref this._messageDisplayLevel, value );
    }

    private int _statusReadDelay = 10;

    /// <summary>   Gets or sets the status read delay. </summary>
    /// <value> The status read delay. </value>
	public int StatusReadDelay
    {
        get => this._statusReadDelay;
        set => this.SetProperty( ref this._statusReadDelay, value );
    }

    private int _readAfterWriteDelay = 10;

    /// <summary>   Gets or sets the read delay. </summary>
    /// <value> The read delay. </value>
	public int ReadAfterWriteDelay
    {
        get => this._readAfterWriteDelay;
        set => this.SetProperty( ref this._readAfterWriteDelay, value );
    }

    private int _maximumDelay = 1000;

    /// <summary>   Gets or sets the read delay. </summary>
    /// <value> The read delay. </value>
	public int MaximumDelay
    {
        get => this._maximumDelay;
        set => this.SetProperty( ref this._maximumDelay, value );
    }

    private string _termination = "\\n";

    /// <summary>   Gets or sets the termination. </summary>
    /// <value> The termination. </value>
	public string Termination
    {
        get => this._termination;
        set => this.SetProperty( ref this._termination, value );
    }

    private System.Drawing.Color _charcoalColor = System.Drawing.Color.FromArgb( 30, 38, 44 );

    /// <summary>   Gets or sets the color of the charcoal. </summary>
    /// <value> The color of the charcoal. </value>
	public System.Drawing.Color CharcoalColor
    {
        get => this._charcoalColor;
        set => this.SetProperty( ref this._charcoalColor, value );
    }

    private string _resourceModel = "Facade";

    /// <summary>   Gets or sets the resource model. </summary>
    /// <value> The resource model. </value>
	public string ResourceModel
    {
        get => this._resourceModel;
        set => this.SetProperty( ref this._resourceModel, value );
    }

    private int _resourceNameSelectionTimeout = 10000;

    /// <summary>   Gets or sets the resource name selection timeout. </summary>
    /// <value> The resource name selection timeout. </value>
	public int ResourceNameSelectionTimeout
    {
        get => this._resourceNameSelectionTimeout;
        set => this.SetProperty( ref this._resourceNameSelectionTimeout, value );
    }

    private double _nominalResistance = 100;

    /// <summary>   Gets or sets the nominal resistance. </summary>
    /// <value> The nominal resistance. </value>
	public double NominalResistance
    {
        get => this._nominalResistance;
        set => this.SetProperty( ref this._nominalResistance, value );
    }

    private double _resistanceTolerance = 0.01;

    /// <summary>   Gets or sets the resistance tolerance. </summary>
    /// <value> The resistance tolerance. </value>
	public double ResistanceTolerance
    {
        get => this._resistanceTolerance;
        set => this.SetProperty( ref this._resistanceTolerance, value );
    }

    private double _openLimit = 1000;

    /// <summary>   Gets or sets the open limit. </summary>
    /// <value> The open limit. </value>
	public double OpenLimit
    {
        get => this._openLimit;
        set => this.SetProperty( ref this._openLimit, value );
    }

    private int _passBitmask = 1;

    /// <summary>   Gets or sets the pass bitmask. </summary>
    /// <value> The pass bitmask. </value>
	public int PassBitmask
    {
        get => this._passBitmask;
        set => this.SetProperty( ref this._passBitmask, value );
    }


    private int _failBitmask = 2;

    /// <summary>   Gets or sets the fail bitmask. </summary>
    /// <value> The fail bitmask. </value>
	public int FailBitmask
    {
        get => this._failBitmask;
        set => this.SetProperty( ref this._failBitmask, value );
    }

    private int _overflowBitmask = 4;

    /// <summary>   Gets or sets the overflow bitmask. </summary>
    /// <value> The overflow bitmask. </value>
	public int OverflowBitmask
    {
        get => this._overflowBitmask;
        set => this.SetProperty( ref this._overflowBitmask, value );
    }

    private int _binningStrobeDuration = 10;

    /// <summary>   Gets or sets the duration of the binning strobe. </summary>
    /// <value> The binning strobe duration. </value>
	public int BinningStrobeDuration
    {
        get => this._binningStrobeDuration;
        set => this.SetProperty( ref this._binningStrobeDuration, value );
    }

    private int _bufferStreamPollInterval = 50;

    /// <summary>   Gets or sets the buffer stream poll interval. </summary>
    /// <value> The buffer stream poll interval. </value>
	public int BufferStreamPollInterval
    {
        get => this._bufferStreamPollInterval;
        set => this.SetProperty( ref this._bufferStreamPollInterval, value );
    }

    private bool _displayStandardServiceRequests = true;

    /// <summary>
    /// Gets or sets a value indicating whether the display standard service requests.
    /// </summary>
    /// <value> True if display standard service requests, false if not. </value>
	public bool DisplayStandardServiceRequests
    {
        get => this._displayStandardServiceRequests;
        set => this.SetProperty( ref this._displayStandardServiceRequests, value );
    }

    private cc.isr.VI.SenseFunctionModes _streamBufferSenseFunctionMode = cc.isr.VI.SenseFunctionModes.ResistanceFourWire;

    /// <summary>   Gets or sets the stream buffer sense function mode. </summary>
    /// <value> The stream buffer sense function mode. </value>
	public cc.isr.VI.SenseFunctionModes StreamBufferSenseFunctionMode
    {
        get => this._streamBufferSenseFunctionMode;
        set => this.SetProperty( ref this._streamBufferSenseFunctionMode, value );
    }

    private string _scanCardScanList = "(@1:3)";

    /// <summary>   Gets or sets a list of scan card scans. </summary>
    /// <value> A list of scan card scans. </value>
	public string ScanCardScanList
    {
        get => this._scanCardScanList;
        set => this.SetProperty( ref this._scanCardScanList, value );
    }

    private int _scanCardSampleCount = 3;

    /// <summary>   Gets or sets the number of scan card samples. </summary>
    /// <value> The number of scan card samples. </value>
	public int ScanCardSampleCount
    {
        get => this._scanCardSampleCount;
        set => this.SetProperty( ref this._scanCardSampleCount, value );
    }

    private int _streamTriggerCount = 999;

    /// <summary>   Gets or sets the number of stream triggers. </summary>
    /// <value> The number of stream triggers. </value>
	public int StreamTriggerCount
    {
        get => this._streamTriggerCount;
        set => this.SetProperty( ref this._streamTriggerCount, value );
    }

    private cc.isr.VI.TriggerSources _streamBufferTriggerSource = cc.isr.VI.TriggerSources.Bus;

    /// <summary>   Gets or sets the stream buffer trigger source. </summary>
    /// <value> The stream buffer trigger source. </value>
	public cc.isr.VI.TriggerSources StreamBufferTriggerSource
    {
        get => this._streamBufferTriggerSource;
        set => this.SetProperty( ref this._streamBufferTriggerSource, value );
    }


    private cc.isr.VI.ArmSources _streamBufferArmSource = cc.isr.VI.ArmSources.Bus;

    /// <summary>   Gets or sets the stream buffer arm source. </summary>
    /// <value> The stream buffer arm source. </value>
	public cc.isr.VI.ArmSources StreamBufferArmSource
    {
        get => this._streamBufferArmSource;
        set => this.SetProperty( ref this._streamBufferArmSource, value );
    }

    private int _strobeLineNumber = 4;

    /// <summary>   Gets or sets the strobe line number. </summary>
    /// <value> The strobe line number. </value>
	public int StrobeLineNumber
    {
        get => this._strobeLineNumber;
        set => this.SetProperty( ref this._strobeLineNumber, value );
    }

    private int _strobeDuration = 10;

    /// <summary>   Gets or sets the duration of the strobe. </summary>
    /// <value> The strobe duration. </value>
	public int StrobeDuration
    {
        get => this._strobeDuration;
        set => this.SetProperty( ref this._strobeDuration, value );
    }

    private int _binLineNumber = 1;

    /// <summary>   Gets or sets the bin line number. </summary>
    /// <value> The bin line number. </value>
	public int BinLineNumber
    {
        get => this._binLineNumber;
        set => this.SetProperty( ref this._binLineNumber, value );
    }

    private int _binDuration = 20;

    /// <summary>   Gets or sets the duration of the bin. </summary>
    /// <value> The bin duration. </value>
	public int BinDuration
    {
        get => this._binDuration;
        set => this.SetProperty( ref this._binDuration, value );
    }

    #endregion
}

