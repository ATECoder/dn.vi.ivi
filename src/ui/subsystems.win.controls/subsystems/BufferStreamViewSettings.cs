using System;
using System.ComponentModel;
using cc.isr.Json.AppSettings.Models;
using System.Text.Json.Serialization;
using cc.isr.Json.AppSettings.ViewModels;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class BufferStreamViewSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2025-01-16. </remarks>
    public BufferStreamViewSettings() { }

    #endregion

    #region " singleton "

    /// <summary>
    /// Creates an instance of the <see cref="BufferStreamViewSettings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static BufferStreamViewSettings CreateInstance()
    {
        BufferStreamViewSettings ti = new();
        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static BufferStreamViewSettings Instance => _instance.Value;

    private static readonly Lazy<BufferStreamViewSettings> _instance = new( CreateInstance, true );

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
            throw new InvalidOperationException( $"{nameof( BufferStreamViewSettings )} was not found." );
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
    /// The settings <see cref="BufferStreamViewSettings.Initialize(Type, string, bool, bool)"/></para>
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
