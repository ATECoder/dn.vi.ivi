using System;
using System.Windows.Forms;
using cc.isr.Json.AppSettings.ViewModels;

namespace cc.isr.VI.DeviceWinControls.Views;

/// <summary>   A display view settings. </summary>
/// <remarks>   2025-01-14. </remarks>
public class DisplayViewSettings : cc.isr.Json.AppSettings.Settings.SettingsBase
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2023-05-09. </remarks>
    public DisplayViewSettings()
    { }

    #endregion

    #region " singleton "

    /// <summary>   Creates the scribe. </summary>
    /// <remarks>   2025-10-30. </remarks>
    public override void CreateScribe()
    {
        this.Scribe = new( [this.SectionName], [this] );
    }

    /// <summary>
    /// Creates an instance of the <see cref="DisplayViewSettings"/> pointing to the current assembly without reading the resources.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static DisplayViewSettings CreateInstance()
    {
        // Get the type of the class that declares this method.
        Type declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!;

        DisplayViewSettings ti = new()
        {
            SectionName = declaringType.Name
        };

        ti.ReadSettings( declaringType, ".Settings", System.Diagnostics.Debugger.IsAttached, System.Diagnostics.Debugger.IsAttached );

        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static DisplayViewSettings Instance => _instance.Value;

    private static readonly Lazy<DisplayViewSettings> _instance = new( DisplayViewSettings.CreateInstance, true );

    #endregion

    #region " values "

    /// <summary>   Gets or sets the color of the charcoal. </summary>
    /// <value> The color of the charcoal. </value>
    public System.Drawing.Color CharcoalColor
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = System.Drawing.Color.FromArgb( 30, 38, 44 );

    /// <summary>   Gets or sets the color of the Background. </summary>
    /// <value> The color of the Background. </value>
    public System.Drawing.Color BackgroundColor
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = System.Drawing.Color.FromArgb( 30, 38, 44 );

    /// <summary>
    /// Gets or sets a value indicating whether the display standard service requests.
    /// </summary>
    /// <value> True if display standard service requests, false if not. </value>
    public bool DisplayStandardServiceRequests
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = true;

    #endregion

    #region " Settings editor "

    /// <summary>   Opens the settings editor. </summary>
    /// <remarks>   David, 2021-12-08. <para>
    /// The settings <see cref="cc.isr.Json.AppSettings.Settings.SettingsBase.ReadSettings(Type, string, bool, bool)"/></para> must be called before attempting to edit the settings. </remarks>
    /// <returns>   A System.Windows.Forms.DialogResult. </returns>
    public DialogResult OpenSettingsEditor()
    {
        Form form = new Json.AppSettings.WinForms.JsonSettingsEditorForm( "Display View Settings Editor",
            new AppSettingsEditorViewModel( this.Scribe!, Json.AppSettings.Services.SimpleServiceProvider.GetInstance() ) );
        return form.ShowDialog();
    }

    #endregion
}
