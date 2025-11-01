using System;
using System.ComponentModel;

namespace cc.isr.Visa.IO.Demo.Properties;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings : cc.isr.Json.AppSettings.Settings.SettingsBase
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
        // Get the type of the class that declares this method.
        Type declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!;

        // Get the AssemblyFileInfo for the assembly that contains the declaring type and
        // append '.Settings' to the assembly name to build the JSon settings file name.

        AssemblyFileInfo ai = new( declaringType.Assembly, null, ".Settings", ".json" );

        // A damaged settings file can be restored from the AppSettingsScribe.AppContextSettingsPath by AppSettingsScribe.Restore().

        // copy application context settings if files do not exist or to clear corrupted settings.

        AppSettingsScribe.InitializeSettingsFiles( ai, System.Diagnostics.Debugger.IsAttached, System.Diagnostics.Debugger.IsAttached );

        Settings ti = new()
        {
            SectionName = declaringType.Name
        };

        ti.ReadSettings( declaringType, ".Settings", System.Diagnostics.Debugger.IsAttached, System.Diagnostics.Debugger.IsAttached );

        return ti;
    }

    /// <summary>   Creates the scribe. </summary>
    /// <remarks>   2025-10-30. </remarks>
    public override void CreateScribe()
    {
        this.Scribe = new( [this.SectionName], [this] );
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static Settings Instance => _instance.Value;

    private static readonly Lazy<Settings> _instance = new( Settings.CreateInstance, true );

    #endregion

    #region " configuration information "

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
        get;
        set => _ = this.SetProperty( ref field, value );
    } = TraceLevel.Off;

    #endregion

    #region " log and message levels "

    /// <summary>   Gets or sets the application log level. </summary>
    /// <remarks>
    /// The minimal level for logging events at the application level. The logger logs the message if
    /// the message level is the same or higher than this level.
    /// </remarks>
    /// <value> The application log level. </value>
    [Description( "The minimal level for logging events at the application level. The logger logs the message if the message level is the same or higher than this level" )]
    public Microsoft.Extensions.Logging.LogLevel ApplicationLogLevel
    {
        get;
        set => this.SetProperty( ref field, value );
    } = Microsoft.Extensions.Logging.LogLevel.Trace;

    /// <summary>   Gets or sets the assembly log level. </summary>
    /// <remarks>
    /// The minimum log level for sending the message to the logger by this assembly. This
    /// filters the message before it is sent to the Logging program.
    /// </remarks>
    /// <value> The assembly log level. </value>
    [Description( "The minimum log level for sending the message to the logger by this assembly. This filters the message before it is sent to the Logging program." )]
    public Microsoft.Extensions.Logging.LogLevel AssemblyLogLevel
    {
        get;
        set => this.SetProperty( ref field, value );
    } = Microsoft.Extensions.Logging.LogLevel.Trace;

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
        get;
        set => this.SetProperty( ref field, value );
    } = TraceEventType.Verbose;

    #endregion

    #region " custom settings "

    /// <summary>   Gets or sets a value indicating whether the multi session is enabled. </summary>
    /// <value> True if multi session enabled, false if not. </value>
    [Description( "Enabled selecting multiple sessions" )]
    public bool MultiSessionEnabled
    {
        get;
        set => this.SetProperty( ref field, value );
    }

    /// <summary>
    /// Gets or sets a value indicating whether the service request is enabled.
    /// </summary>
    /// <value> True if service request enabled, false if not. </value>
    [Description( "Enables service request handling" )]
    public bool ServiceRequestEnabled
    {
        get;
        set => this.SetProperty( ref field, value );
    } = true;

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [Description( "The default VISA resource name" )]
    public string ResourceName
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "TCPIP0::192.168.0.144::inst0::INSTR";

    /// <summary>   Gets or sets the service request commands. </summary>
    /// <value> The service request commands. </value>
    [Description( "The service request commands to display in the combo box" )]
    public string ServiceRequestCommands
    {
        get;
        set => this.SetProperty( ref field, value );
    } = "*SRE 16|SS,0";

    #endregion
}

