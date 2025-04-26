namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP Node syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class Node
{
    #region " node info "

    /// <summary>   (Immutable) the controller node number. </summary>
    public const string ControllerNodeNumber = "_G.tsplink.node";

    #endregion


    #region " node identity "

    /// <summary>
    /// Gets or sets the IDN query (print) command returning an identity formatted for Keithley
    /// instruments matching the SCPI standard.
    /// </summary>
    /// <remarks>
    /// Same as '*IDN?'.<para>
    /// Requires setting the subsystem reference.
    /// </para><code>
    /// Value = string.Format(KeithleyInstrumentIdentificationQueryCommandBuilder,"node name")
    /// </code>
    /// </remarks>
    /// <value> The identity query (print) command format. </value>
    public static string KeithleyInstrumentIdentificationQueryCommandBuilder { get; set; } = "_G.print(\"Keithley Instruments Inc., Model \"..{0}.model..\", \"..{0}.serialno..\", \"..{0}.version)";

    #endregion

    #region " node commands "

    /// <summary> Gets the collect garbage wait complete command. Requires a node number argument. </summary>
    public const string CollectGarbageOperationCompleteCommandFormat = "_G.node[{0}].execute('collectgarbage()') _G.opc()";

    /// <summary> Gets the collect garbage query complete command. Requires a node number argument. </summary>
    public const string CollectGarbageQueryCompleteCommandFormat = "_G.node[{0}].execute('collectgarbage()') _G.waitcomplete() print('1') ";

    /// <summary> Gets the collect garbage wait command. Requires a node number argument. </summary>
    public const string CollectGarbageWaitCommandFormat = "_G.node[{0}].execute('collectgarbage()') _G.waitcomplete()";

    /// <summary> Gets the execute command operation complete. Requires node number and command arguments. </summary>
    public const string ExecuteCommandOperationCompleteCommandFormat = "_G.node[{0}].execute(\"{1}\") _G.opc()";

    /// <summary> Gets the execute wait command.  Requires node number and command arguments. </summary>
    public const string ExecuteCommandWaitCommandFormat = "_G.node[{0}].execute(\"{1}\") _G.waitcomplete()";

    /// <summary> Gets the value returned by executing a command on the node.
    /// Requires node number and value to get arguments. </summary>
    public const string ValueGetterWaitCommandFormat1 = "_G.node[{0}].execute('dataqueue.add({1})') _G.waitcomplete() _G.print(_G.node[{0}].dataqueue.next())";

    // 3517 "_G.node[{0}].execute('dataqueue.add({1})') _G.waitcomplete(0) _G.print(_G.node[{0}].dataqueue.next())"

    /// <summary> Gets the value returned by executing a command on the node.
    /// Requires node number, command, and value to get arguments. </summary>
    public const string ValueGetterWaitCommandFormat2 = "_G.node[{0}].execute(\"do {1} dataqueue.add({2}) end\") _G.waitcomplete() _G.print(_G.node[{0}].dataqueue.next())";

    // 3517 "_G.node[{0}].execute(""do {1} dataqueue.add({2}) end"") _G.waitcomplete(0) _G.print(_G.node[{0}].dataqueue.next())"

    /// <summary> Gets the connect rule command. Requires node number and value arguments. </summary>
    public const string ConnectRuleSetterWaitCommandFormat = "_G.node[{0}].channel.connectrule = {1} _G.waitcomplete()";

    /// <summary>   Saved script getter command. </summary>
    /// <remarks>   2025-04-12. </remarks>
    /// <param name="nodeNumber">   The node number. Must not be a controller node. </param>
    /// <returns>   A string. </returns>
    public static string SavedScriptGetterCommand( int nodeNumber )
    {
        return string.Format( Syntax.Tsp.Node.ValueGetterWaitCommandFormat2, nodeNumber, Syntax.Tsp.Script.ScriptCatalogGetterCommand, "names" );
    }

    /// <summary>   Returns a command for searching of the specified script on the node. </summary>
    /// <remarks>   2025-04-12. </remarks>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="nodeNumber">   The node number. Must not be a controller node. </param>
    /// <returns>   The find script getter command. </returns>
    public static string FindSavedScriptCommand( string scriptName, int nodeNumber )
    {
        string getterCommand = string.Format( Syntax.Tsp.Script.FindSavedScriptCommandFormat, scriptName );
        return string.Format( Syntax.Tsp.Node.ValueGetterWaitCommandFormat2, nodeNumber, getterCommand, "exists" );
    }

    #endregion

    #region " system command builders "

    /// <summary> Gets or sets the reset command Builder. </summary>
    /// <remarks>
    /// Same as '*RST'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The reset known state command builder. </value>
    public static string ResetKnownStateCommandBuilder { get; set; } = "{0}.reset()";

    /// <summary> Gets or sets the status clear (CLS) command Builder. </summary>
    /// <remarks>
    /// Same as '*CLS'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The clear execution state command builder. </value>
    public static string ClearExecutionStateCommandBuilder { get; set; } = "{0}.status.reset()";

    #endregion

    #region " error queue command builders "

    /// <summary> Gets or sets the error queue clear command builder. </summary>
    /// <remarks>
    /// Same as ':STAT:QUE:CLEAR'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The clear error queue command builder. </value>
    public static string ClearErrorQueueCommandBuilder { get; set; } = "{0}.eventlog.clear()";

    /// <summary> Gets or sets the error queue query (print) command builder. </summary>
    /// <remarks>
    /// Same as ':STAT:QUE?'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The error queue query (print) command builder. </value>
    public static string ErrorQueueQueryCommandBuilder { get; set; } = "_G.print(string.format('%d,%s,level=%d',{0}.eventlog.next(eventlog.SEV_ERROR)))";

    /// <summary> Gets or sets the error queue count query (print) command builder. </summary>
    /// <remarks> Requires setting the subsystem reference. </remarks>
    /// <value> The error queue count query (print) command builder. </value>
    public static string ErrorQueueCountQueryCommandBuilder { get; set; } = "_G.print({0}.eventlog.getcount(eventlog.SEV_ERROR))";

    /// <summary> The node error count value builder. </summary>
    public const string NodeErrorCountBuilder = "node[{0}].eventlog.getcount(eventlog.SEV_ERROR)";

    #endregion

    #region " operation events "

    /// <summary> Gets or sets the operation event enable command format builder. </summary>
    /// <remarks>
    /// Same as ''.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The operation event enable command format builder. </value>
    public static string OperationEventEnableCommandFormatBuilder { get; set; } = "{0}.status.operation.enable = {{0}}";

    /// <summary> Gets or sets the operation event enable query (print) command builder. </summary>
    /// <remarks>
    /// Same as ''.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The operation event enable query (print) command builder. </value>
    public static string OperationEventEnableQueryCommandBuilder { get; set; } = "_G.print(_G.tostring({0}.status.operation.enable))";

    /// <summary> Gets or sets the operation event status query (print) command builder. </summary>
    /// <remarks>
    /// Same as ''.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The operation event query (print) command builder. </value>
    public static string OperationEventQueryCommandBuilder { get; set; } = "_G.print(_G.tostring({0}.status.operation.event))";

    #endregion

    #region " service request "

    /// <summary> Gets or sets the service request enable command format builder. </summary>
    /// <remarks>
    /// Same as *SRE {0:D}'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The service request enable command format builder. </value>
    public static string ServiceRequestEnableCommandFormatBuilder { get; set; } = "{0}.status.request_enable = {{0}}";

    /// <summary>   Gets or sets the service request enable node command format. </summary>
    /// <value> The service request enable node command format. </value>
    public static string ServiceRequestEnableCommandFormat { get; set; } = "_G.Node[{0}].status.request_enable = {1}";

    /// <summary> Gets or sets the service request enable query (print) command builder. </summary>
    /// <remarks>
    /// Same as ''.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The service request enable query (print) command builder. </value>
    public static string ServiceRequestEnableQueryCommandBuilder { get; set; } = "_G.print(_G.tostring({0}.status.request_enable))";

    /// <summary> Gets or sets the service request enable query (print) command builder. </summary>
    /// <remarks>
    /// Same as '*ESR?'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The service request event query (print) command builder. </value>
    public static string ServiceRequestEventQueryCommandBuilder { get; set; } = "_G.print(_G.tostring({0}.status.condition))";

    /// <summary>   Gets or sets the service request event query node command format. </summary>
    /// <value> The service request event query node command format. </value>
    public static string ServiceRequestEventQueryCommandFormat { get; set; } = "_G.print(_G.tostring(_G.Node[{0}].status.condition))";

    #endregion

    #region " standard events "

    /// <summary> Gets or sets the standard event enable command format builder. </summary>
    /// <remarks>
    /// Same as *ESE {0:D}'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The standard event enable command format builder. </value>
    public static string StandardEventEnableCommandFormatBuilder { get; set; } = "{0}.status.standard.enable = {{0}}";

    /// <summary>   Gets or sets the standard event enable node command format. </summary>
    /// <value> The standard event enable node command format. </value>
    public static string StandardEventEnableCommandFormat { get; set; } = "_G.Node[{0}].status.standard.enable = {1}";

    /// <summary> Gets or sets the standard event enable query (print) command builder. </summary>
    /// <remarks>
    /// Same as ''.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The standard event enable query (print) command builder. </value>
    public static string StandardEventEnableQueryCommandBuilder { get; set; } = "_G.print(_G.tostring({0}.status.standard.enable))";

    /// <summary>   Gets or sets the standard event enable query node command format. </summary>
    /// <value> The standard event enable query node command format. </value>
    public static string StandardEventEnableQueryCommandFormat { get; set; } = "_G.print(_G.tostring(_G.Node[{0}].status.standard.enable))";

    /// <summary> Gets or sets the standard event status query (print) command builder. </summary>
    /// <remarks>
    /// Same as *ESR?'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    /// <value> The standard event query (print) command builder. </value>
    public static string StandardEventQueryCommandBuilder { get; set; } = "_G.waitcomplete() _G.print(_G.tostring({0}.status.standard.event))";

    /// <summary>   Gets or sets the standard event query node command format. </summary>
    /// <value> The standard event query node command format. </value>
    public static string StandardEventQueryCommandFormat { get; set; } = "_G.waitcomplete() _G.print(_G.tostring(_G.Node[{0}].status.standard.event))";

    #endregion

    #region " service request + standard events "

    /// <summary>
    /// (Immutable) The Standard Event and Service Request Enable command format.
    /// <code>
    /// Lua: _G.Node[{0}].status.standard.enable = {1} _G.Node[{0}].status.request_enable = {2}
    /// SCPI: '*ESE {0:D}; *SRE {1:D}'
    /// </code>
    /// <para>When preceded with the Clear Execution State command <see cref="Lua.ClearExecutionStateCommand"/>,
    /// ending with line feed delimiter causes the 2600 instrument to fail when initializing.</para><para>
    /// The command works fine with line feeds if issued on its own.</para>
    /// </summary>
    public const string StandardServiceEnableCommandFormat = "_G.Node[{0}].status.standard.enable = {1} _G.Node[{0}].status.request_enable = {2}";

    /// <summary> The  Standard Event and Service Request Enable operation complete command format preceded by clear execution state.
    /// <code>
    /// Lua: _G.Node[{0}].status.reset() _G.Node[{0}].status.standard.enable = {1} _G.Node[{0}].status.request_enable = {2} _G.Node[{0}].opc()
    /// SCPI: *CLS; *ESE {0:D}; *SRE {1:D}; *OPC
    /// <para>Using line feed delimiter causes the 2600 instrument to fail when initializing.</para>
    /// </code>
    /// </summary>
    ///
    /// <remarks>
    /// </remarks>
    public const string StandardServiceEnableOperationCompleteCommandFormat = "_G.Node[{0}].status.reset() _G.Node[{0}].status.standard.enable = {1} _G.Node[{0}].status.request_enable = {2} _G.Node[{0}].opc()";

    #endregion
}
