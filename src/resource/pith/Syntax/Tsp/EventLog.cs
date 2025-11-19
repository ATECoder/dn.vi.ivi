// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP Event (and error) Log syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class EventLog
{
    /// <summary> The global event log clear command. </summary>
    /// <remarks> Same as ':STAT:QUE:CLEAR'</remarks>
    public const string ClearEventLogWaitCommand = "_G.eventlog.clear() _G.waitcomplete()";

    /// <summary> The local node clear event log command. </summary>
    public const string LocalNodeClearEventLogWaitCommand = "localnode.eventlog.clear() _G.waitcomplete()";

    /// <summary> The node event log clear. </summary>
    public const string NodeClearEventLogWaitCommand = "node[{0}].eventlog.clear() waitcomplete()";

    /// <summary> The next error value. </summary>
    public const string NextError = "_G.eventlog.next(eventlog.SEV_ERROR)";

    /// <summary> The next error query (print) command. </summary>
    public const string NextErrorQueryCommand = "_G.print(_G.eventlog.next(eventlog.SEV_ERROR))";

    /// <summary> The next error formatted query command. </summary>
    public const string NextErrorFormattedQueryCommand = "_G.print(string.format('%d,%s,level=%d',_G.eventlog.next(eventlog.SEV_ERROR)))";

    /// <summary> The error count value. </summary>
    public const string ErrorCount = "_G.eventlog.getcount(eventlog.SEV_ERROR)";

    /// <summary> The error count query (print) command. </summary>
    public const string ErrorCountQueryCommand = "_G.print(_G.eventlog.getcount(eventlog.SEV_ERROR))";

    #region " default error messages "

    /// <summary> Gets the error message representing no error. </summary>
    public const string NoErrorMessage = "No Error";

    /// <summary> Gets the compound error message representing no error. </summary>
    public const string NoErrorCompoundMessage = "0,No Error,0,0";

    #endregion
}
