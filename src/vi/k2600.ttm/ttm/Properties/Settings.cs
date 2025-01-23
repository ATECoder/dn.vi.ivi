using System.Text.Json.Serialization;
using cc.isr.Json.AppSettings.Models;
using cc.isr.Logging.TraceLog;

namespace cc.isr.VI.Tsp.K2600.Ttm.Properties;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    private Settings() { }

    #endregion

    #region " singleton "

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static Settings Instance => _instance.Value;

    private static readonly Lazy<Settings> _instance = new( () => new Settings(), true );

    #endregion

    #region " scribe "

    /// <summary>   Gets or sets the scribe. </summary>
    /// <value> The scribe. </value>
    public AppSettingsScribe? Scribe { get; set; }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    /// <param name="settingsAssembly">     The settings assembly. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.driver' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void CreateScribe( System.Reflection.Assembly settingsAssembly, string settingsFileSuffix = ".driver" )
    {
        AssemblyFileInfo ai = new( settingsAssembly, null, settingsFileSuffix, ".json" );

        // must copy application context settings here to clear any bad settings files.

        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! );
        AppSettingsScribe.CopySettings( ai.AppContextAssemblyFilePath!, ai.ThisUserAssemblyFilePath! );

        this.Scribe = new( [this.TraceLogSettings, this.TtmEstimatorSettings, this.TtmMeterSettings, this.TtmResistanceSettings,
            this.TtmShuntSettings, this.TtmTraceSettings],
            ai.AppContextAssemblyFilePath!, ai.AllUsersAssemblyFilePath! )
        {
            AllUsersSettingsPath = ai.AllUsersAssemblyFilePath,
            ThisUserSettingsPath = ai.ThisUserAssemblyFilePath
        };
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-05. </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.driver' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void CreateScribe( System.Type callingEntity, string settingsFileSuffix = ".driver" )
    {
        this.CreateScribe( callingEntity.Assembly, settingsFileSuffix );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-17. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="settingsAssembly">     The assembly where the settings fille is located.. </param>
    /// <param name="settingsFileSuffix">   (Optional) The suffix of the assembly settings file, e.g.,
    ///                                     '.driver' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void ReadSettings( System.Reflection.Assembly settingsAssembly, string settingsFileSuffix = ".driver" )
    {
        if ( this.Scribe is null )
            this.CreateScribe( settingsAssembly, settingsFileSuffix );

        this.Scribe!.ReadSettings();

        if ( !System.IO.File.Exists( this.Scribe.UserSettingsPath ) )
            throw new InvalidOperationException( $"{nameof( Settings )} settings file {this.Scribe.UserSettingsPath} not found." );
        else if ( !this.SettingsExist( out string details ) )
            throw new InvalidOperationException( details );
    }

    /// <summary>   Reads the settings. </summary>
    /// <remarks> 2024-08-05. <para>
    /// Creates the scribe if null.</para>
    /// </remarks>
    /// <param name="callingEntity">        The calling entity. </param>
    /// <param name="settingsFileSuffix">   The suffix of the assembly settings file, e.g.,
    ///                                     '.driver' in
    ///                                     'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.JSon' where
    ///                                     cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name. </param>
    public void ReadSettings( System.Type callingEntity, string settingsFileSuffix = ".driver" )
    {
        this.ReadSettings( callingEntity.Assembly, settingsFileSuffix );
    }

    /// <summary>   Gets the full pathname of the settings file. </summary>
    /// <value> The full pathname of the settings file. </value>
    [JsonIgnore]
    public string? FilePath => this.Scribe?.UserSettingsPath;

    /// <summary>   Check if the settings file exists. </summary>
    /// <remarks>   2024-07-06. </remarks>
    /// <returns>   True if it the settings file exists; otherwise false. </returns>
    public bool SettingsFileExists()
    {
        return this.FilePath is not null && System.IO.File.Exists( this.FilePath );
    }

    /// <summary>   Checks if all settings exist. </summary>
    /// <remarks>   2025-01-18. </remarks>
    /// <returns>   A Tuple. </returns>
    public bool SettingsExist( out string details )
    {
        if ( this.TraceLogSettings is null || !this.TraceLogSettings.Exists )
            details = $"{nameof( this.TraceLogSettings )} not found.";
        else if ( this.TtmEstimatorSettings is null || !this.TtmEstimatorSettings.Exists )
            details = $"{nameof( this.TtmEstimatorSettings )} not found.";
        else if ( this.TtmMeterSettings is null || !this.TtmMeterSettings.Exists )
            details = $"{nameof( this.TtmMeterSettings )} not found.";
        else if ( this.TtmResistanceSettings is null || !this.TtmResistanceSettings.Exists )
            details = $"{nameof( this.TtmResistanceSettings )} not found.";
        else if ( this.TtmShuntSettings is null || !this.TtmShuntSettings.Exists )
            details = $"{nameof( this.TtmShuntSettings )} not found.";
        else if ( this.TtmTraceSettings is null || !this.TtmTraceSettings.Exists )
            details = $"{nameof( this.TtmTraceSettings )} not found.";
        else
            details = string.Empty;

        return details.Length == 0;
    }

    #endregion

    #region " setting instances "

    /// <summary>   Gets or sets the trace log settings. </summary>
    /// <value> The trace log settings. </value>
    public TraceLogSettings TraceLogSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Estimator settings. </summary>
    /// <value> The TTM Estimator settings. </value>
    public TtmEstimatorSettings TtmEstimatorSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Meter settings. </summary>
    /// <value> The TTM Meter settings. </value>
    public TtmMeterSettings TtmMeterSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM MeasuredValue settings. </summary>
    /// <value> The TTM MeasuredValue settings. </value>
    public TtmResistanceSettings TtmResistanceSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Shunt settings. </summary>
    /// <value> The TTM Shunt settings. </value>
    public TtmShuntSettings TtmShuntSettings { get; private set; } = new();

    /// <summary>   Gets or sets the TTM Trace settings. </summary>
    /// <value> The TTM Trace settings. </value>
    public TtmTraceSettings TtmTraceSettings { get; private set; } = new();

    #endregion
}
