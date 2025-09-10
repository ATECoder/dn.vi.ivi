using System;
using System.ComponentModel;
using cc.isr.Json.AppSettings.Models;
using System.Text.Json.Serialization;
using cc.isr.Json.AppSettings.ViewModels;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class DigitalOutputViewSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2025-01-16. </remarks>
    public DigitalOutputViewSettings() { }

    #endregion

    #region " singleton "

    /// <summary>
    /// Creates an instance of the <see cref="DigitalOutputViewSettings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static DigitalOutputViewSettings CreateInstance()
    {
        DigitalOutputViewSettings ti = new();
        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static DigitalOutputViewSettings Instance => _instance.Value;

    private static readonly Lazy<DigitalOutputViewSettings> _instance = new( CreateInstance, true );

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
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public void Initialize( Type callingEntity, string settingsFileSuffix, bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        AssemblyFileInfo ai = new( callingEntity.Assembly, null, settingsFileSuffix, ".json" );

        // copy application context settings if these do not exist; use restore if the settings are bad.

        AppSettingsScribe.InitializeSettingsFiles( ai, overrideAllUsersFile, overrideThisUserFile );

        this.Scribe = new( [_instance.Value], ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = ai.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = ai.ThisUserAssemblyFilePath
        };

        this.Scribe.ReadSettings();

        if ( _instance.Value is null || !_instance.Value.Exists )
            throw new InvalidOperationException( $"{nameof( DigitalOutputViewSettings )} was not found." );
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

    #region " values "


    /// <summary>   Gets or sets the strobe line number. </summary>
    /// <value> The strobe line number. </value>
    [ObservableProperty]
    public partial int StrobeLineNumber { get; set; } = 4;

    /// <summary>   Gets or sets the duration of the strobe. </summary>
    /// <value> The strobe duration. </value>
    [ObservableProperty]
    public partial int StrobeDuration { get; set; } = 10;

    /// <summary>   Gets or sets the bin line number. </summary>
    /// <value> The bin line number. </value>
    [ObservableProperty]
    public partial int BinLineNumber { get; set; } = 1;

    /// <summary>   Gets or sets the duration of the bin. </summary>
    /// <value> The bin duration. </value>
    [ObservableProperty]
    public partial int BinDuration { get; set; } = 20;

    #endregion

    #region " Settings editor "

    /// <summary>   Opens the settings editor. </summary>
    /// <remarks>   David, 2021-12-08. <para>
    /// The settings <see cref="DigitalOutputViewSettings.Initialize(Type, string, bool, bool )"/></para> must be called before attempting to edit the settings. </remarks>
    /// <returns>   A System.Windows.Forms.DialogResult. </returns>
    public DialogResult OpenSettingsEditor()
    {
        Form form = new Json.AppSettings.WinForms.JsonSettingsEditorForm( "Digital Output View Settings Editor",
            new AppSettingsEditorViewModel( this.Scribe!, Json.AppSettings.Services.SimpleServiceProvider.GetInstance() ) );
        return form.ShowDialog();
    }

    #endregion
}

