using System;
using System.Windows.Forms;
using cc.isr.Json.AppSettings.ViewModels;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary>   A digital output view settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class DigitalOutputViewSettings : cc.isr.Json.AppSettings.Settings.SettingsBase
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2025-01-16. </remarks>
    public DigitalOutputViewSettings() { }

    #endregion

    #region " singleton "

    /// <summary>   Creates the scribe. </summary>
    /// <remarks>   2025-10-30. </remarks>
    public override void CreateScribe()
    {
        this.Scribe = new( [this.SectionName], [this] );
    }

    /// <summary>
    /// Creates an instance of the <see cref="DigitalOutputViewSettings"/> pointing to the current assembly without reading the resources.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static DigitalOutputViewSettings CreateInstance()
    {
        // Get the type of the class that declares this method.
        Type declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!;

        DigitalOutputViewSettings ti = new()
        {
            SectionName = declaringType.Name
        };

        bool overwrite = Json.AppSettings.Models.AppSettingsScribe.IsDebuggingOrTesting();

        ti.ReadSettings( declaringType, ".Settings", overwrite, overwrite );

        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static DigitalOutputViewSettings Instance => _instance.Value;

    private static readonly Lazy<DigitalOutputViewSettings> _instance = new( DigitalOutputViewSettings.CreateInstance, true );

    #endregion

    #region " values "

    /// <summary>   Gets or sets the strobe line number. </summary>
    /// <value> The strobe line number. </value>
    public int StrobeLineNumber
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 4;

    /// <summary>   Gets or sets the duration of the strobe. </summary>
    /// <value> The strobe duration. </value>
    public int StrobeDuration
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 10;

    /// <summary>   Gets or sets the bin line number. </summary>
    /// <value> The bin line number. </value>
    public int BinLineNumber
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 1;

    /// <summary>   Gets or sets the duration of the bin. </summary>
    /// <value> The bin duration. </value>
    public int BinDuration
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 20;

    #endregion

    #region " Settings editor "

    /// <summary>   Opens the settings editor. </summary>
    /// <remarks>
    /// David, 2021-12-08. <para>
    /// The settings <see cref="cc.isr.Json.AppSettings.Settings.SettingsBase.ReadSettings(Type, string, bool, bool )"/>
    /// </para> must be called before attempting to edit the settings.
    /// </remarks>
    /// <returns>   A System.Windows.Forms.DialogResult. </returns>
    public DialogResult OpenSettingsEditor()
    {
        Form form = new Json.AppSettings.WinForms.JsonSettingsEditorForm( "Digital Output View Settings Editor",
            new AppSettingsEditorViewModel( this.Scribe!, Json.AppSettings.Services.SimpleServiceProvider.GetInstance() ) );
        return form.ShowDialog();
    }

    #endregion
}

