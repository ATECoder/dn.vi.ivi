using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using cc.isr.Json.AppSettings.Models;

namespace cc.isr.VI.WinControls.Properties;

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
    public bool Exists()
    {
        return this.Scribe is not null && System.IO.File.Exists( this.Scribe.UserSettingsPath );
    }

    #endregion

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

}

