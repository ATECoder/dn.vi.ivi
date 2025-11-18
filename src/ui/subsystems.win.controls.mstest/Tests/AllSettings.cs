using System;

namespace cc.isr.VI.SubsystemsWinControls.Tests;

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
		this.TraceLogSettings = new();
		this.BufferStreamViewSettings = new();
		this.DigitalOutputViewSettings = new();
		
        this.Scribe = new( [this.TraceLogSettings, this.BufferStreamViewSettings, this.DigitalOutputViewSettings] )
        {
            SerializerOptions = AppSettingsScribe.CsvRgbColorSerializerOptions
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

        bool overwrite = Json.AppSettings.Models.AppSettingsScribe.IsDebuggingOrTesting();

        ti.ReadSettings( declaringType, ".Settings", overwrite, overwrite );

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
    internal cc.isr.Json.AppSettings.Settings.LocationSettings LocationSettings { get; private set; } = new();

    /// <summary>   Gets or sets the buffer stream view settings. </summary>
    /// <value> The buffer stream view settings. </value>
    internal BufferStreamViewSettings BufferStreamViewSettings { get; private set; } = new();

    /// <summary>   Gets or sets the digital output view settings. </summary>
    /// <value> The digital output view settings. </value>
    internal DigitalOutputViewSettings DigitalOutputViewSettings { get; private set; } = new();

    #endregion
}

