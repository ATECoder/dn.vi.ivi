using cc.isr.VI.Pith.Settings;
using cc.isr.VI.Tsp.K2600.MSTest.Measure;
using cc.isr.VI.Tsp.K2600.MSTest.Source;
using cc.isr.VI.Tsp.K2600.MSTest.Subsystems;
using cc.isr.VI.Tsp.K2600.MSTest.Visa;

namespace cc.isr.VI.Tsp.K2600.MSTest.Settings;

/// <summary>   A settings container class for all settings. </summary>
/// <remarks>   2024-10-16. </remarks>
public class AllSettings : cc.isr.Json.AppSettings.Settings.SettingsContainerBase
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    /// <remarks>   2023-04-24. </remarks>
    private AllSettings() { }

    #endregion

    #region " singleton "

    /// <summary>   Creates a scribe. </summary>
    /// <remarks>   2025-01-13. </remarks>
    public override void CreateScribe()
    {
        this.LocationSettings = new();
        this.CommandsSettings = new();
        this.DeviceErrorsSettings = new();
        this.DigitalIOSettings = new();
        this.IOSettings = new();
        this.ResourceSettings = new();
        this.SystemSubsystemSettings = new();
        this.SenseResistanceSettings = new();
        this.SourceResistanceSettings = new();
        this.ResistanceSettings = new();
        this.SenseVoltageSettings = new();
        this.SourceCurrentSettings = new();
        this.CurrentSourceMeasureSettings = new();

        this.Scribe = new( [this.LocationSettings,
            this.CommandsSettings, this.DeviceErrorsSettings, this.DigitalIOSettings,
            this.IOSettings, this.ResourceSettings, this.SystemSubsystemSettings,
            this.SenseResistanceSettings, this.SourceResistanceSettings, this.ResistanceSettings,
            this.SenseVoltageSettings, this.SourceCurrentSettings, this.CurrentSourceMeasureSettings] );
    }

    /// <summary>
    /// Creates an instance of the <see cref="AllSettings"/> after restoring the application context
    /// settings to both the user and all user files if these do not exist.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static AllSettings CreateInstance()
    {
        // Get the type of the class that declares this method.
        Type declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!;

        AllSettings ti = new();

        ti.ReadSettings( declaringType, overrideAllUsersFile: System.Diagnostics.Debugger.IsAttached, overrideThisUserFile: System.Diagnostics.Debugger.IsAttached );

        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static AllSettings Instance => _instance.Value;

    private static readonly Lazy<AllSettings> _instance = new( AllSettings.CreateInstance, true );

    #endregion

    #region " IO "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-17. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="declaringType">        The Type of the declaring object. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.session' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.json' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    /// <param name="overrideAllUsersFile"> (Optional) [false] True to override all users settings file. </param>
    /// <param name="overrideThisUserFile"> (Optional) [false] True to override this user settings file. </param>
    public override void ReadSettings( Type declaringType, string settingsFileSuffix = ".settings",
        bool overrideAllUsersFile = false, bool overrideThisUserFile = false )
    {
        base.ReadSettings( declaringType, settingsFileSuffix, overrideAllUsersFile, overrideThisUserFile );
    }

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the location settings. </summary>
    /// <value> The location settings. </value>
    internal cc.isr.Json.AppSettings.Settings.LocationSettings LocationSettings { get; private set; } = new();

    /// <summary>   Gets or sets the i/o settings. </summary>
    /// <value> The i/o settings. </value>
    internal IOSettings IOSettings { get; private set; } = new();

    /// <summary>   Gets or sets the commands settings. </summary>
    /// <value> The commands settings. </value>
    internal CommandsSettings CommandsSettings { get; private set; } = new();

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    internal Pith.Settings.ResourceSettings ResourceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the digital i/o settings. </summary>
    /// <value> The digital i/o settings. </value>
    internal DigitalIOSettings DigitalIOSettings { get; private set; } = new();

    /// <summary>   Gets or sets the device errors settings. </summary>
    /// <value> The device errors settings. </value>
    internal DeviceErrorsSettings DeviceErrorsSettings { get; private set; } = new();

    /// <summary>   Gets or sets the system subsystem settings. </summary>
    /// <value> The system subsystem settings. </value>
    internal VI.Settings.SystemSubsystemSettings SystemSubsystemSettings { get; private set; } = new();

    /// <summary>   Gets or sets the sense subsystem settings. </summary>
    /// <value> The sense subsystem settings. </value>
    internal VI.Settings.SenseSubsystemSettings SenseSubsystemSettings { get; private set; } = new();

    /// <summary>   Gets or sets the sense resistance settings. </summary>
    /// <value> The sense resistance settings. </value>
    internal SenseResistanceSettings SenseResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets source resistance settings. </summary>
    /// <value> The source resistance settings. </value>
    internal SourceResistanceSettings SourceResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the resistance settings. </summary>
    /// <value> The resistance settings. </value>
    internal ResistanceSettings ResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the sense voltage settings. </summary>
    /// <value> The sense voltage settings. </value>
    internal SenseVoltageSettings SenseVoltageSettings { get; private set; } = new();

    /// <summary>   Gets or sets source current settings. </summary>
    /// <value> The source current settings. </value>
    internal SourceCurrentSettings SourceCurrentSettings { get; private set; } = new();

    /// <summary>   Gets or sets the current source settings. </summary>
    /// <value> The current source settings. </value>
    internal CurrentSourceMeasureSettings CurrentSourceMeasureSettings { get; private set; } = new();

    #endregion
}

