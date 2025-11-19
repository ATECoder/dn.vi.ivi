namespace cc.isr.VI.Syntax.Tsp;

/// <summary>   Error Queue TSP Syntax. </summary>
/// <remarks>   David, 2020-12-01. </remarks>
public static class ErrorQueue
{
    /// <summary>   (Immutable) queue of errors. </summary>
    private const string Error_Queue = "_G.errorqueue";

    /// <summary>   (Immutable) queue of local node errors. </summary>
    private const string Local_Node_Error_Queue = "_G.localnode.errorqueue";

    /// <summary> The default error queue clear command. </summary>
    /// <remarks> Same as ':STAT:QUE:CLEAR'</remarks>
    public const string ClearErrorQueueCommand = $"{ErrorQueue.Error_Queue}.clear()";

    /// <summary> The default error queue clear command. </summary>
    /// <remarks> Same as ':STAT:QUE:CLEAR'</remarks>
    public const string ClearErrorQueueWaitCommand = $"{ErrorQueue.ClearErrorQueueCommand} {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} ";

    /// <summary>   (Immutable) the clear error queue operation complete command. </summary>
    public const string ClearErrorQueueOperationCompleteCommand = $"{ErrorQueue.ClearErrorQueueCommand} {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} ";

    /// <summary> The clear error queue command. </summary>
    public const string ClearLocalNodeErrorQueueWaitCommand = $"{Local_Node_Error_Queue}.clear() {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} ";

    /// <summary> The default error queue (print) command. </summary>
    /// <remarks> Same as ':STAT:QUE?'</remarks>
    public const string ErrorQueueQueryCommand = $"_G.print(string.format('%d,%s,level=%d',{ErrorQueue.Error_Queue}.next()))";

    /// <summary>   The  error queue count query (print) command. </summary>
    /// <remarks>
    /// Same as '..'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    public const string ErrorQueueCountQueryCommand = $"_G.print({ErrorQueue.Error_Queue}.count)";
}
