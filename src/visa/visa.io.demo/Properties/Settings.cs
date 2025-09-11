using System;
using System.ComponentModel;

namespace cc.isr.Visa.IO.Demo.Properties;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings : System.ComponentModel.INotifyPropertyChanged
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    /// <remarks>   2023-04-24. </remarks>
    public Settings() { }

    #endregion

    #region " notify property change implementation "

    /// <summary>   Occurs when a property value changes. </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>   Executes the 'property changed' action. </summary>
    /// <param name="propertyName"> Name of the property. </param>
    protected virtual void OnPropertyChanged( string? propertyName )
    {
        if ( !string.IsNullOrEmpty( propertyName ) )
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }

    /// <summary>   Executes the 'property changed' action. </summary>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="backingField"> [in,out] The backing field. </param>
    /// <param name="value">        The value. </param>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    /// <returns>   <see langword="true"/> if it succeeds; otherwise, <see langword="false"/>. </returns>
    protected virtual bool OnPropertyChanged<T>( ref T backingField, T value, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = "" )
    {
        if ( System.Collections.Generic.EqualityComparer<T>.Default.Equals( backingField, value ) )
            return false;

        backingField = value;
        this.OnPropertyChanged( propertyName );
        return true;
    }

    /// <summary>   Sets a property. </summary>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="prop">         [in,out] The property. </param>
    /// <param name="value">        The value. </param>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    /// <returns>   <see langword="true"/> if it succeeds; otherwise, <see langword="false"/>. </returns>
    protected bool SetProperty<T>( ref T prop, T value, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null )
    {
        if ( System.Collections.Generic.EqualityComparer<T>.Default.Equals( prop, value ) ) return false;
        prop = value;
        this.OnPropertyChanged( propertyName );
        return true;
    }

    /// <summary>   Sets a property. </summary>
    /// <remarks>   2023-03-24. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="oldValue">     The old value. </param>
    /// <param name="newValue">     The new value. </param>
    /// <param name="callback">     The callback. </param>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    /// <returns>   <see langword="true"/> if it succeeds; otherwise, <see langword="false"/>. </returns>
    protected bool SetProperty<T>( T oldValue, T newValue, Action callback, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null )
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( callback, nameof( callback ) );
#else
        if ( callback is null ) throw new ArgumentNullException( nameof( callback ) );
#endif

        if ( System.Collections.Generic.EqualityComparer<T>.Default.Equals( oldValue, newValue ) )
        {
            return false;
        }

        callback();

        this.OnPropertyChanged( propertyName );

        return true;
    }

    /// <summary>   Removes the property changed event handlers. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    protected void RemovePropertyChangedEventHandlers()
    {
        PropertyChangedEventHandler? handler = this.PropertyChanged;
        if ( handler is not null )
        {
            foreach ( Delegate? item in handler.GetInvocationList() )
            {
                handler -= ( PropertyChangedEventHandler ) item;
            }
        }
    }

    #endregion

    #region " singleton "

    /// <summary>
    /// Creates an instance of the <see cref="Settings"/>.
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

    #region " assembly settings file information "

    /// <summary>
    /// Creates an instance of the settings <see cref="AssemblyFileInfo"/>. This restores the
    /// application context settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2024-07-18. </remarks>
    /// <returns>   The new settings file information. </returns>
    private static AssemblyFileInfo CreateSettingsFileInfo()
    {
        // Get the method declaring type for the assembly file information and the settings section name.
        Type declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!;

        // get assembly files using the .Settings suffix.

        AssemblyFileInfo ai = new( declaringType.Assembly, null, ".Settings", ".json" );

        // Copy application context settings if these do not exist or if the debugger is attached as when running under the IDE.
        // A damaged settings file can be restored from the AppSettingsScribe.AppContextSettingsPath by AppSettingsScribe.Restore().

        AppSettingsScribe.InitializeSettingsFiles( ai, Debugger.IsAttached, Debugger.IsAttached );

        return ai;
    }

    [JsonIgnore]
    private static AssemblyFileInfo SettingsFileInfo => _settingsFileInfo.Value;

    private static readonly Lazy<AssemblyFileInfo> _settingsFileInfo = new( CreateSettingsFileInfo, true );

    #endregion

    #region " settings scribe "

    /// <summary>
    /// Creates an instance of the <see cref="AppSettingsScribe"/> using the constructed <see cref="AssemblyFileInfo"/>,
	/// reads the settings into a singleton instance of this class and validates that the settings was read.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new scribe. </returns>
    private static AppSettingsScribe CreateScribe()
    {
        // get an instance of the settings file info first.
        AssemblyFileInfo settingsFileInfo = Settings.SettingsFileInfo;

        AppSettingsScribe scribe = new( [Settings.Instance], settingsFileInfo );

        scribe.ReadSettings();

        if ( !Settings.Instance.Exists )
            throw new InvalidOperationException( $"{nameof( Settings )} not found or failed to read from {scribe.UserSettingsPath}." );

        return scribe;
    }

    /// <summary>   Gets the <see cref="AppSettingsScribe">settings reader and writer</see>. </summary>
    /// <value> The scribe. </value>
    [JsonIgnore]
    [CLSCompliant( false )]
    public static AppSettingsScribe Scribe => _scribe.Value;

    private static readonly Lazy<AppSettingsScribe> _scribe = new( CreateScribe, true );

    /// <summary>   Gets the full path name of the settings file. </summary>
    /// <value> The full path name of the settings file. </value>
    [JsonIgnore]
    public static string FilePath => Settings.Scribe.UserSettingsPath;

    /// <summary>   Check if the settings file exits. </summary>
    /// <remarks>   2024-07-06. </remarks>
    /// <returns>   True if it the settings file exists; otherwise false. </returns>
    public static bool FileExists()
    {
        return System.IO.File.Exists( Settings.FilePath );
    }

    #endregion

    #region " exists "

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

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

    /// <summary>   Gets or sets a value indicating whether this object is enabled. </summary>
    /// <value> True if enabled, false if not. </value>
    [System.ComponentModel.Description( "True if testing is enabled for this test class" )]
    public bool Enabled
    {
        get;
        set => this.SetProperty( ref field, value );
    } = true;

    /// <summary> Gets or sets all. </summary>
    /// <value> all. </value>
    [System.ComponentModel.Description( "True if all testing is enabled for this test class" )]
    public bool All
    {
        get;
        set => this.SetProperty( ref field, value );
    } = true;

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

