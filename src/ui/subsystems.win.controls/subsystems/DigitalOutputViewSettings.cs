using System;
using System.ComponentModel;
using cc.isr.Json.AppSettings.Models;
using System.Text.Json.Serialization;
using cc.isr.Json.AppSettings.ViewModels;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class DigitalOutputViewSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    #region " values "

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

    #region " Settings editor "

    /// <summary>   Opens the settings editor. </summary>
    /// <remarks>   David, 2021-12-08. <para>
    /// The settings <see cref="DigitalOutputViewSettings.Initialize(Type, string)"/></para> must be called before attempting to edit the settings. </remarks>
    /// <returns>   A System.Windows.Forms.DialogResult. </returns>
    public DialogResult OpenSettingsEditor()
    {
        Form form = new Json.AppSettings.WinForms.JsonSettingsEditorForm( "Digital Output View Settings Editor",
            new AppSettingsEditorViewModel( this.Scribe!, Json.AppSettings.Services.SimpleServiceProvider.GetInstance() ) );
        return form.ShowDialog();
    }

    #endregion
}

