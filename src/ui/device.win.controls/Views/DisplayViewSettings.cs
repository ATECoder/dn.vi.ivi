using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using cc.isr.Json.AppSettings.Models;
using cc.isr.Json.AppSettings.ViewModels;

namespace cc.isr.VI.DeviceWinControls.Views;
/// <summary>   A display view settings. </summary>
/// <remarks>   2025-01-14. </remarks>
public class DisplayViewSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2025-01-16. </remarks>
    public DisplayViewSettings() { }

    #endregion

    #region " singleton "

    /// <summary>
    /// Creates an instance of the <see cref="DisplayViewSettings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static DisplayViewSettings CreateInstance()
    {
        DisplayViewSettings ti = new();
        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static DisplayViewSettings Instance => _instance.Value;

    private static readonly Lazy<DisplayViewSettings> _instance = new( CreateInstance, true );

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

    private System.Drawing.Color _charcoalColor = System.Drawing.Color.FromArgb( 30, 38, 44 );

    /// <summary>   Gets or sets the color of the charcoal. </summary>
    /// <value> The color of the charcoal. </value>
	public System.Drawing.Color CharcoalColor
    {
        get => this._charcoalColor;
        set => _ = this.SetProperty( ref this._charcoalColor, value );
    }

    private System.Drawing.Color _backgroundColor = System.Drawing.Color.FromArgb( 30, 38, 44 );

    /// <summary>   Gets or sets the color of the Background. </summary>
    /// <value> The color of the Background. </value>
	public System.Drawing.Color BackgroundColor
    {
        get => this._backgroundColor;
        set => _ = this.SetProperty( ref this._backgroundColor, value );
    }

    private bool _displayStandardServiceRequests = true;

    /// <summary>
    /// Gets or sets a value indicating whether the display standard service requests.
    /// </summary>
    /// <value> True if display standard service requests, false if not. </value>
	public bool DisplayStandardServiceRequests
    {
        get => this._displayStandardServiceRequests;
        set => _ = this.SetProperty( ref this._displayStandardServiceRequests, value );
    }

    #endregion

    #region " Settings editor "

    /// <summary>   Opens the settings editor. </summary>
    /// <remarks>   David, 2021-12-08. <para>
    /// The settings <see cref="DisplayViewSettings.Initialize(Type, string)"/></para> must be called before attempting to edit the settings. </remarks>
    /// <returns>   A System.Windows.Forms.DialogResult. </returns>
    public DialogResult OpenSettingsEditor()
    {
        Form form = new Json.AppSettings.WinForms.JsonSettingsEditorForm( "Display View Settings Editor",
            new AppSettingsEditorViewModel( this.Scribe!, Json.AppSettings.Services.SimpleServiceProvider.GetInstance() ) );
        return form.ShowDialog();
    }

    #endregion
}
