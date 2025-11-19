// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP Status syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class Status
{
    #region " measurement events "

    /// <summary> The  measurement event enable command format. </summary>
    public const string MeasurementEventEnableCommandFormat = "_G.status.measurement.enable={0}";

    /// <summary> The  measurement event enable query (print) command. </summary>
    public const string MeasurementEventEnableQueryCommand = "_G.print(_G.tostring(_G.status.measurement.enable))";

    /// <summary> The  measurement event status query (print) command. </summary>
    public const string MeasurementEventQueryCommand = "_G.print(_G.tostring(_G.status.measurement.event))";

    /// <summary> The  measurement event condition query command. </summary>
    public const string MeasurementEventConditionQueryCommand = "_G.print(_G.tostring(_G.status.measurement.condition))";

    /// <summary> The measurement event condition query (print) command. </summary>
    public const string MeasurementEventConditionFormattedQueryCommand = "_G.print(_G.string.format('%d',_G.status.measurement.condition))";

    #endregion

    #region " operation events "

    /// <summary> The  operation event enable command format. </summary>
    public const string OperationEventEnableCommandFormat = "_G.status.operation.enable={0}";

    /// <summary> The  operation event enable query (print) command. </summary>
    public const string OperationEventEnableQueryCommand = "_G.print(_G.tostring(_G.status.operation.enable))";

    /// <summary> The  operation event status query (print) command. </summary>
    public const string OperationEventQueryCommand = "_G.print(_G.tostring(_G.status.operation.event))";

    /// <summary> The  operation event condition query (print) command. </summary>
    public const string OperationEventConditionQueryCommand = "_G.print(_G.tostring(_G.status.operation.condition))";

    #endregion

    #region " questionable events "

    /// <summary> The  questionable event enable command format. </summary>
    public const string QuestionableEventEnableCommandFormat = "_G.status.questionable.enable={0}";

    /// <summary> The  questionable event enable query (print) command. </summary>
    public const string QuestionableEventEnableQueryCommand = "_G.print(_G.tostring(_G.status.questionable.enable))";

    /// <summary> The  questionable event status query (print) command. </summary>
    public const string QuestionableEventQueryCommand = "_G.print(_G.tostring(_G.status.questionable.event))";

    /// <summary> The  questionable event condition query (print) command. </summary>
    public const string QuestionableEventConditionQueryCommand = "_G.print(_G.tostring(_G.status.questionable.condition))";

    #endregion

    #region " service request "

    /// <summary> The Service Request Enable *SRE {0:D}' Enable command. </summary>
    /// <remarks> Same as '*SRE {0:D}'</remarks>
    public const string ServiceRequestEnableCommandFormat = "_G.status.request_enable={0}";

    /// <summary> The  service request enable query (print) command. </summary>
    /// <remarks> Same as '*SRE?'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    public const string ServiceRequestEnableQueryCommand = "_G.print(_G.tostring(_G.status.request_enable)) ";

    /// <summary> The  service request enable query (print) command builder. </summary>
    /// <remarks> Same as ''.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    public const string ServiceRequestEventQueryCommand = "_G.print(_G.tostring(_G.status.condition))";

    #endregion

    #region " standard events "

    /// <summary> The Standard Event Enable command. </summary>
    /// <remarks> Same as '*ESE {0:D}'</remarks>
    public const string StandardEventEnableCommandFormat = "_G.status.standard.enable={0}";

    /// <summary>   The  standard event enable query (print) command. </summary>
    /// <remarks>
    /// Same as '*ESE?'. The formatting command (To String) serves to return an whole number string
    /// rather than an exponential formatted string.
    /// </remarks>
    public const string StandardEventEnableQueryCommand = "_G.print(_G.tostring(_G.status.standard.enable)) ";

    /// <summary> The Standard Event Enable (*ESR?) query (print) command. </summary>
    /// <remarks> Same as '*ESR?'</remarks>
    public const string StandardEventStatusQueryCommand = "_G.print(_G.tostring(_G.status.standard.event))";

    #endregion

    #region " service request + standard events "

    /// <summary>
    /// (Immutable) The Standard Event and Service Request Enable command format.
    /// <code>
    /// Lua: _G.status.standard.enable={0} _G.status.request_enable={1}
    /// SCPI: '*ESE {0:D}; *SRE {1:D}'
    /// </code>
    /// <para>When preceded with the Clear Execution State command <see cref="Lua.ClearExecutionStateCommand"/>,
    /// ending with line feed delimiter causes the 2600 instrument to fail when initializing.</para><para>
    /// The command works fine with line feeds if issued on its own.</para>
    /// </summary>
    public const string StandardServiceEnableCommandFormat = "_G.status.standard.enable={0} _G.status.request_enable={1}";

    /// <summary> The  Standard Event and Service Request Enable wait complete command format preceded by clear execution state.
    /// <code>
    /// Lua: _G.status.reset() _G.waitcomplete() _G.status.standard.enable={0} _G.status.request_enable={1} _G.opc()
    /// SCPI: *CLS; *WAI; *ESE {0:D}; *SRE {1:D}; *OPC
    /// <para>Using line feed delimiter causes the 2600 instrument to fail when initializing.</para>
    /// </code>
    /// </summary>
    ///
    /// <remarks>
    /// </remarks>
    public const string StandardServiceEnableOperationCompleteCommandFormat = "_G.status.reset() _G.waitcomplete() _G.status.standard.enable={0} _G.status.request_enable={1} _G.opc()";

    /// <summary>   (Immutable) the standard service enable query complete command format.
    /// <code>
    /// Lua: _G.status.reset() _G.waitcomplete() _G.status.standard.enable={0} _G.status.request_enable={1} _G.waitcomplete() print('1')
    /// SCPI: *CLS; *WAI; *ESE {0:D}; *SRE {1:D}; *OPC?
    /// <para>Using line feed delimiter causes the 2600 instrument to fail when initializing.</para>
    /// </code>
    /// </summary>
    public const string StandardServiceEnableQueryCompleteCommandFormat = "_G.status.reset() _G.waitcomplete() _G.status.standard.enable={0} _G.status.request_enable={1} _G.waitcomplete() print('1') ";

    #endregion
}
