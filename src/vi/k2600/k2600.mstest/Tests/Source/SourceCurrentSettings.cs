// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600.Tests.Source;

/// <summary> Current Source Test Info. </summary>
/// <remarks> <para>
/// David, 2018-02-12 </para></remarks>
public class SourceCurrentSettings() : cc.isr.VI.Settings.SourceSubsystemSettings
{
    #region " scribe "

    /// <summary>   Reads the settings. </summary>
    /// <remarks>   2024-08-03. </remarks>
    public void ReadSettings()
    {
        AppSettingsScribe.ReadSettings( Settings.AllSettings.Instance.Scribe!.AllUsersSettingsPath!, nameof( SourceCurrentSettings ),
            Settings.AllSettings.Instance.SourceCurrentSettings,
            AppSettingsScribe.DefaultSerializerOptions, AppSettingsScribe.DefaultDocumentOptions );
    }

    #endregion

    #region " source subsystem information "

    /// <summary>   Gets or sets source function. </summary>
    /// <value> The source function. </value>
    public SourceFunctionMode SourceFunction
    {
        get;
        set => this.SetProperty( ref field, value );
    } = SourceFunctionMode.CurrentDC;

    #endregion
}
