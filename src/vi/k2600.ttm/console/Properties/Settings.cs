using cc.isr.VI.Tsp.K2600.Ttm.Controls;

namespace cc.isr.VI.Tsp.K2600.Ttm.Console.Properties;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class Settings : cc.isr.Json.AppSettings.Settings.SettingsContainerBase
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    /// <remarks>   2023-04-24. </remarks>
    public Settings() { }

    #endregion

    #region " singleton "

    /// <summary>   Creates the scribe. </summary>
    /// <remarks>   2025-10-30. </remarks>
    public override void CreateScribe()
    {
        Settings.ConsoleSettings = new();
        Settings.TtmSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance;
        Settings.LotSettingsContainer = Controls.Settings.Instance;

        this.Scribe = new( [Settings.ConsoleSettings] );
    }

    /// <summary>
    /// Creates an instance of the <see cref="Settings"/> after restoring the application context
    /// settings to both the user and all user files.
    /// </summary>
    /// <remarks>   2023-05-15. </remarks>
    /// <returns>   The new instance. </returns>
    private static Settings CreateInstance()
    {
        // Get the type of the class that declares this method.
        Type declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!;

        Settings.TtmSettings.ReadSettings( declaringType, ".Driver" );
        Settings.LotSettingsContainer.ReadSettings( declaringType, ".Lot" );

        Settings ti = new();

        ti.ReadSettings( declaringType, ".Settings", System.Diagnostics.Debugger.IsAttached, System.Diagnostics.Debugger.IsAttached );

        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static Settings Instance => _instance.Value;

    private static readonly Lazy<Settings> _instance = new( Settings.CreateInstance, true );

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the console settings. </summary>
    /// <value> The console settings. </value>
    public static UI.ConsoleSettings ConsoleSettings { get; private set; } = new();

    /// <summary>   Gets or sets the ttm settings. </summary>
    /// <value> The ttm settings. </value>
    public static cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings TtmSettings { get; set; } = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance;

    /// <summary>   Gets or sets the lot settings container. </summary>
    /// <value> The lot settings container. </value>
    public static Controls.Settings LotSettingsContainer { get; set; } = Controls.Settings.Instance;

    #endregion
}
