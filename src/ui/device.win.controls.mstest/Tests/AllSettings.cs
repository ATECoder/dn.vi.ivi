using cc.isr.VI.Pith.Settings;

namespace cc.isr.VI.DeviceWinControls.Tests;

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

    /// <summary>   Creates the scribe. </summary>
    /// <remarks>   2025-10-30. </remarks>
    public override void CreateScribe()
    {
        this.LocationSettings = new();
        this.ResourceSettings = new();
        this.DisplayViewSettings = new();

        this.Scribe = new( [this.LocationSettings, this.ResourceSettings, this.DisplayViewSettings] )
        {
            SerializerOptions = cc.isr.Json.AppSettings.Models.AppSettingsScribe.CsvRgbColorSerializerOptions
        };
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

        ti.ReadSettings( declaringType, ".Settings", System.Diagnostics.Debugger.IsAttached, System.Diagnostics.Debugger.IsAttached );

        return ti;
    }

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static AllSettings Instance => _instance.Value;

    private static readonly Lazy<AllSettings> _instance = new( AllSettings.CreateInstance, true );

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the location settings. </summary>
    /// <value> The location settings. </value>
    internal Json.AppSettings.Settings.LocationSettings LocationSettings { get; private set; } = new();

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    internal ResourceSettings ResourceSettings { get; set; } = new();

    /// <summary>   Gets or sets the display view settings. </summary>
    /// <value> The display view settings. </value>
    internal Views.DisplayViewSettings DisplayViewSettings { get; private set; } = new();

    #endregion
}

