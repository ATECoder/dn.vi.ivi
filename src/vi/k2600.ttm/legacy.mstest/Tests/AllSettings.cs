// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

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

        this.Scribe = new( [this.LocationSettings] );
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

    #endregion
}

