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
        AppSettingsScribe.ReadSettings( Settings.SettingsFileInfo.AllUsersAssemblyFilePath!, nameof( Settings ), ti );
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

