using System;
using System.Windows.Forms;
using cc.isr.Json.AppSettings.ViewModels;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary>   A buffer stream view settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class BufferStreamViewSettings : cc.isr.Json.AppSettings.Settings.SettingsBase
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2025-01-16. </remarks>
    public BufferStreamViewSettings() { }

    #endregion

    #region " singleton "

    /// <summary>   Creates the scribe. </summary>
    /// <remarks>   2025-10-30. </remarks>
    public override void CreateScribe()
    {
        this.Scribe = new( [this.SectionName], [this] );
    }

    /// <summary>
    /// Creates an instance of the <see cref="BufferStreamViewSettings"/> pointing to the current assembly without reading the resources.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static BufferStreamViewSettings CreateInstance()
    {
        // Get the type of the class that declares this method.
        Type declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!;

        BufferStreamViewSettings ti = new()
        {
            SectionName = declaringType.Name
        };

        ti.ReadSettings( declaringType, ".Settings", System.Diagnostics.Debugger.IsAttached, System.Diagnostics.Debugger.IsAttached );

        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static BufferStreamViewSettings Instance => _instance.Value;

    private static readonly Lazy<BufferStreamViewSettings> _instance = new( BufferStreamViewSettings.CreateInstance, true );

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the stream buffer sense function mode. </summary>
    /// <value> The stream buffer sense function mode. </value>
    public SenseFunctionModes StreamBufferSenseFunctionMode
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = SenseFunctionModes.ResistanceFourWire;

    /// <summary>   Gets or sets the nominal resistance. </summary>
    /// <value> The nominal resistance. </value>
    public double NominalResistance
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 100;

    /// <summary>   Gets or sets the resistance tolerance. </summary>
    /// <value> The resistance tolerance. </value>
    public double ResistanceTolerance
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 0.01;

    /// <summary>   Gets or sets the open limit. </summary>
    /// <value> The open limit. </value>
    public double OpenLimit
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 1000;

    /// <summary>   Gets or sets the pass bitmask. </summary>
    /// <value> The pass bitmask. </value>
    public int PassBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 1;

    /// <summary>   Gets or sets the fail bitmask. </summary>
    /// <value> The fail bitmask. </value>
    public int FailBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 2;

    /// <summary>   Gets or sets the overflow bitmask. </summary>
    /// <value> The overflow bitmask. </value>
    public int OverflowBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 4;

    /// <summary>   Gets or sets the duration of the binning strobe. </summary>
    /// <value> The binning strobe duration. </value>
    public int BinningStrobeDuration
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 10;

    /// <summary>   Gets or sets the number of stream triggers. </summary>
    /// <value> The number of stream triggers. </value>
    public int StreamTriggerCount
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 999;

    /// <summary>   Gets or sets the stream buffer arm source. </summary>
    /// <value> The stream buffer arm source. </value>
    public ArmSources StreamBufferArmSource
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = ArmSources.Bus;

    /// <summary>   Gets or sets the stream buffer trigger source. </summary>
    /// <value> The stream buffer trigger source. </value>
    public TriggerSources StreamBufferTriggerSource
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = TriggerSources.Bus;

    /// <summary>   Gets or sets the buffer stream poll interval. </summary>
    /// <value> The buffer stream poll interval. </value>
    public int BufferStreamPollInterval
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 50;

    /// <summary>   Gets or sets the number of scan card samples. </summary>
    /// <value> The number of scan card samples. </value>
    public int ScanCardSampleCount
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = 3;

    /// <summary>   Gets or sets a list of scan card scans. </summary>
    /// <value> A list of scan card scans. </value>
    public string ScanCardScanList
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "(@1:3)";

    #endregion

    #region " Settings editor "

    /// <summary>   Opens the settings editor. </summary>
    /// <remarks>
    /// David, 2021-12-08. <para>
    /// The settings <see cref="cc.isr.Json.AppSettings.Settings.SettingsBase.ReadSettings(Type, string, bool, bool)"/></para>
    /// must be called before attempting to edit the settings.
    /// </remarks>
    /// <returns>   A System.Windows.Forms.DialogResult. </returns>
    public DialogResult OpenSettingsEditor()
    {
        Form form = new Json.AppSettings.WinForms.JsonSettingsEditorForm( "Buffer Stream View Settings Editor",
            new AppSettingsEditorViewModel( this.Scribe!, Json.AppSettings.Services.SimpleServiceProvider.GetInstance() ) );
        return form.ShowDialog();
    }

    #endregion
}
