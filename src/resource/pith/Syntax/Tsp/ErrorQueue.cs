namespace cc.isr.VI.Syntax.Tsp;

/// <summary>   Error Queue Ieee488Syntax. </summary>
/// <remarks>   David, 2020-12-01. </remarks>
public static class ErrorQueue
{
    /// <summary> The default error queue clear command. </summary>
    /// <remarks> Same as ':STAT:QUE:CLEAR'</remarks>
    public const string ClearErrorQueueWaitCommand = "_G.errorqueue.clear() _G.waitcomplete()";

    /// <summary> The clear error queue command. </summary>
    public const string ClearLocalNodeErrorQueueWaitCommand = "localnode.errorqueue.clear() waitcomplete()";

    /// <summary> The default error queue (print) command. </summary>
    /// <remarks> Same as ':STAT:QUE?'</remarks>
    public const string ErrorQueueQueryCommand = "_G.print(string.format('%d,%s,level=%d',_G.errorqueue.next()))";

    /// <summary>   The  error queue count query (print) command. </summary>
    /// <remarks>
    /// Same as '..'.<para>
    /// Requires setting the subsystem reference.
    /// </para>
    /// </remarks>
    public const string ErrorQueueCountQueryCommand = "_G.print(_G.errorqueue.count)";
}
