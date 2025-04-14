namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that query if 'session' is loaded. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <returns>   True if loaded, false if not. </returns>
    public static bool IsLoaded( this Pith.SessionBase session, int nodeNumber, string scriptName )
    {
        return !session.IsNil( nodeNumber, scriptName );
    }

}
