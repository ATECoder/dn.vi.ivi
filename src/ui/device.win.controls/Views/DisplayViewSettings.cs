using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using cc.isr.Json.AppSettings.Models;
using cc.isr.Json.AppSettings.ViewModels;

namespace cc.isr.VI.DeviceWinControls.Views;
/// <summary>   A display view settings. </summary>
/// <remarks>   2025-01-14. </remarks>
public class DisplayViewSettings() : System.ComponentModel.INotifyPropertyChanged
{
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
        if ( EqualityComparer<T>.Default.Equals( backingField, value ) )
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
        if ( EqualityComparer<T>.Default.Equals( prop, value ) ) return false;
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

        if ( EqualityComparer<T>.Default.Equals( oldValue, newValue ) )
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
            throw new InvalidOperationException( $"{nameof( DisplayViewSettings )} was not found." );
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
    /// The settings <see cref="DisplayViewSettings.Initialize(Type, string, bool, bool)"/></para> must be called before attempting to edit the settings. </remarks>
    /// <returns>   A System.Windows.Forms.DialogResult. </returns>
    public DialogResult OpenSettingsEditor()
    {
        Form form = new Json.AppSettings.WinForms.JsonSettingsEditorForm( "Display View Settings Editor",
            new AppSettingsEditorViewModel( this.Scribe!, Json.AppSettings.Services.SimpleServiceProvider.GetInstance() ) );
        return form.ShowDialog();
    }

    #endregion
}
