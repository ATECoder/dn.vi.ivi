// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP Local Node syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class LocalNode
{
    /// <summary> The line frequency property string. </summary>
    public const string Local_Node = "_G.localnode";

    /// <summary> The line frequency property string. </summary>
    public const string LineFrequency = "localnode.linefreq";

    /// <summary> The line frequency query (print) command. </summary>
    public const string LineFrequencyQueryCommand = "_G.print(localnode.linefreq)";

    /// <summary> Gets or sets the 'identity query' command. </summary>
    /// <value> The 'identity query' command. </value>
    public static string IdentificationQueryCommand { get; set; } = "*IDN?";

    /// <summary> The serial number query (print) command. </summary>
    public const string SerialNumberQueryCommand = "_G.print(G.localnode.serialno)";

    /// <summary> The serial number query (print) command. </summary>
    public const string SerialNumberFormattedQueryCommand = "_G.print(string.format('%d',_G.localnode.serialno))";

    /// <summary> The firmware version query (print) command. </summary>
    public const string FirmwareVersionQueryCommand = "_G.print(_G.localnode.version)";

    /// <summary> The model query (print) command. </summary>
    public const string ModelQueryCommand = "_G.print(_G.localnode.model)";

    /// <summary>   (Immutable) the reset known state command. </summary>
    public const string ResetCommand = "_G.localnode.reset()";

    /// <summary>
    /// Gets or sets the IDN query (print) command returning an identity formatted for Keithley
    /// instruments matching the SCPI standard.
    /// </summary>
    /// <remarks>
    /// Returns a message similar to '*IDN?' with Keithley Instruments as the manufacturer.
    /// </remarks>
    /// <value> The identity query (print) command format. </value>
    public static string KeithleyInstrumentIdentificationQueryCommand { get; set; } = "_G.print(\"Keithley Instruments Inc., Model \".._G.localnode.model..\", \".._G.localnode.serialno..\", \".._G.localnode.version)";

    /// <summary> The show errors property string. </summary>
    public static string ShowErrors = "_G.localnode.showerrors";

    /// <summary> The hide/show errors command. </summary>
    public static string ShowErrorsSetterCommand = "_G.localnode.showerrors={0:'1';'1';'0'}";

    /// <summary> The show prompts property. </summary>
    public static string ShowPrompts = "_G.localnode.prompts";

    /// <summary> The hide/show prompts setter command. </summary>
    public static string ShowPromptsSetterCommand = "_G.localnode.prompts={0:'1';'1';'0'}";

}
/// <summary> A bit-field of flags for specifying event log modes. </summary>
[Flags]
public enum EventLogModes
{
    /// <summary> No errors. </summary>
    [System.ComponentModel.Description( "No events" )]
    None = 0,

    /// <summary> Errors only. </summary>
    [System.ComponentModel.Description( "Errors only (eventlog.SEV_ERR)" )]
    Errors = 1,

    /// <summary> Warnings only. </summary>
    [System.ComponentModel.Description( "Warnings only (eventlog.SEV_WARN)" )]
    Warnings = 2,

    /// <summary> Information only. </summary>
    [System.ComponentModel.Description( "Information only (eventlog.SEV_INFO)" )]
    Information = 4
}
