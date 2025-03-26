using System;
using System.ComponentModel;

namespace cc.isr.Visa.IO.Demo.Properties;

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
        AppSettingsScribe.ReadSettings( Settings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( Settings ), ti,
            AppSettingsScribe.DefaultSerializerOptions, AppSettingsScribe.DefaultDocumentOptions );
        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static Settings Instance => _instance.Value;

    private static readonly Lazy<Settings> _instance = new( CreateInstance, true );

    #endregion

    #region " assembly settings file information "

    /// <summary>
    /// Creates an instance of the settings <see cref="AssemblyFileInfo"/>. This restores the
    /// application context settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2024-07-18. </remarks>
    /// <returns>   The new settings file information. </returns>
    private static AssemblyFileInfo CreateSettingsFileInfo()
    {
        // get assembly files using the .Logging suffix.

        AssemblyFileInfo ai = new( typeof( Settings ).Assembly, null, ".Settings", ".json" );

        // must copy application context settings here to clear any bad settings files.

        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! );
        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.ThisUserAssemblyFilePath! );

        return ai;
    }

    [JsonIgnore]
    private static AssemblyFileInfo SettingsFileInfo => _settingsFileInfo.Value;

    private static readonly Lazy<AssemblyFileInfo> _settingsFileInfo = new( CreateSettingsFileInfo, true );

    #endregion

    #region " settings scribe "

    /// <summary>
    /// Creates an instance of the <see cref="Settings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static AppSettingsScribe CreateScribe()
    {
        // get an instance of the settings file info first.
        AssemblyFileInfo settingsFileInfo = Settings.SettingsFileInfo;

        AppSettingsScribe scribe = new( [Settings.Instance],
            settingsFileInfo.AppContextAssemblyFilePath!, settingsFileInfo.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = settingsFileInfo.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = settingsFileInfo.ThisUserAssemblyFilePath
        };
        scribe.ReadSettings();

        return scribe;
    }

    /// <summary>   Gets the <see cref="AppSettingsScribe">settings reader and writer</see>. </summary>
    /// <value> The scribe. </value>
    [JsonIgnore]
    public static AppSettingsScribe Scribe => _scribe.Value;

    private static readonly Lazy<AppSettingsScribe> _scribe = new( CreateScribe, true );

    /// <summary>   Gets the full pathname of the settings file. </summary>
    /// <value> The full pathname of the settings file. </value>
    [JsonIgnore]
    public static string FilePath => Settings.Scribe.UserSettingsPath;

    /// <summary>   Check if the settings file exits. </summary>
    /// <remarks>   2024-07-06. </remarks>
    /// <returns>   True if it the settings file exists; otherwise false. </returns>
    public static bool Exists()
    {
        return System.IO.File.Exists( Settings.FilePath );
    }

    #endregion

    #region " configuration information "

    private TraceLevel _traceLevel = TraceLevel.Verbose;

    /// <summary>   Gets or sets the trace level. </summary>
    /// <value> The trace level. </value>
    [System.ComponentModel.Description( "Sets the message level" )]
    public TraceLevel TraceLevel
    {
        get => this._traceLevel;
        set => _ = this.SetProperty( ref this._traceLevel, value );
    }

    private bool _enabled = true;

    /// <summary>   Gets or sets a value indicating whether this object is enabled. </summary>
    /// <value> True if enabled, false if not. </value>
    [System.ComponentModel.Description( "True if testing is enabled for this test class" )]
    public bool Enabled
    {
        get => this._enabled;
        set => this.SetProperty( ref this._enabled, value );
    }

    private bool _all = true;

    /// <summary> Gets or sets all. </summary>
    /// <value> all. </value>
    [System.ComponentModel.Description( "True if all testing is enabled for this test class" )]
    public bool All
    {
        get => this._all;
        set => this.SetProperty( ref this._all, value );
    }

    #endregion

    private Microsoft.Extensions.Logging.LogLevel _applicationLogLevel = Microsoft.Extensions.Logging.LogLevel.Trace;

    /// <summary>   Gets or sets the application log level. </summary>
    /// <remarks>
    /// The minimal level for logging events at the application level. The logger logs the message if
    /// the message level is the same or higher than this level.
    /// </remarks>
    /// <value> The application log level. </value>
	[Description( "The minimal level for logging events at the application level. The logger logs the message if the message level is the same or higher than this level" )]
    public Microsoft.Extensions.Logging.LogLevel ApplicationLogLevel
    {
        get => this._applicationLogLevel;
        set => this.SetProperty( ref this._applicationLogLevel, value );
    }

    private Microsoft.Extensions.Logging.LogLevel _assemblyLogLevel = Microsoft.Extensions.Logging.LogLevel.Trace;

    /// <summary>   Gets or sets the assembly log level. </summary>
    /// <remarks>
    /// The minimum log level for sending the message to the logger by this assembly. This
    /// filters the message before it is sent to the Logging program.
    /// </remarks>
    /// <value> The assembly log level. </value>
	[Description( "The minimum log level for sending the message to the logger by this assembly. This filters the message before it is sent to the Logging program." )]
    public Microsoft.Extensions.Logging.LogLevel AssemblyLogLevel
    {
        get => this._assemblyLogLevel;
        set => this.SetProperty( ref this._assemblyLogLevel, value );
    }

    private TraceEventType _messageDisplayLevel = TraceEventType.Verbose;

    /// <summary>   Gets or sets the display level for log and trace messages. </summary>
    /// <remarks>
    /// The maximum trace event type for displaying logged and trace events. Only messages with a
    /// message <see cref="Diagnostics.TraceEventType"/> level that is same or higher than
    /// this level are displayed.
    /// </remarks>
    /// <value> The message display level. </value>
	[Description( "The maximum trace event type for displaying logged and trace events. Only messages with a message a level that is same or higher than this level are displayed." )]
    public TraceEventType MessageDisplayLevel
    {
        get => this._messageDisplayLevel;
        set => this.SetProperty( ref this._messageDisplayLevel, value );
    }

    private bool _multiSessionEnabled;

    /// <summary>   Gets or sets a value indicating whether the multi session is enabled. </summary>
    /// <value> True if multi session enabled, false if not. </value>
	[Description( "Enabled selecting multiple sessions" )]
    public bool MultiSessionEnabled
    {
        get => this._multiSessionEnabled;
        set => this.SetProperty( ref this._multiSessionEnabled, value );
    }

    private bool _serviceRequestEnabled = true;

    /// <summary>
    /// Gets or sets a value indicating whether the service request is enabled.
    /// </summary>
    /// <value> True if service request enabled, false if not. </value>
	[Description( "Enables service request handling" )]
    public bool ServiceRequestEnabled
    {
        get => this._serviceRequestEnabled;
        set => this.SetProperty( ref this._serviceRequestEnabled, value );
    }

    private string _resourceName = "TCPIP0::192.168.0.144::inst0::INSTR";

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
	[Description( "The default VISA resource name" )]
    public string ResourceName
    {
        get => this._resourceName;
        set => this.SetProperty( ref this._resourceName, value );
    }

    private string _serviceRequestCommands = "*SRE 16|SS,0";

    /// <summary>   Gets or sets the service request commands. </summary>
    /// <value> The service request commands. </value>
	[Description( "The service request commands to display in the combo box" )]
    public string ServiceRequestCommands
    {
        get => this._serviceRequestCommands;
        set => this.SetProperty( ref this._serviceRequestCommands, value );
    }
}

