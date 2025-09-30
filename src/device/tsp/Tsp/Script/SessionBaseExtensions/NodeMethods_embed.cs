using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that fetches the names of the embedded scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="ArgumentOutOfRangeException">  Thrown when one or more arguments are outside
    ///                                                 the required range. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <returns>   The names of the embedded scripts. </returns>
    public static string FetchEmbeddedScriptsNames( this Pith.SessionBase session, int nodeNumber )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        if ( nodeNumber < 0 )
            throw new ArgumentOutOfRangeException( nameof( nodeNumber ), "Node number must be greater than or equal to zero." );

        int controllerNodeNumber = session.QueryControllerNodeNumber();

        if ( controllerNodeNumber == nodeNumber )
        {
            return session.FetchEmbeddedScriptsNames();
        }
        else
        {
            session.LastNodeNumber = nodeNumber;
            session.SetLastAction( $"fetching embedded scripts from node {nodeNumber}" );
            _ = session.WriteLine( Syntax.Tsp.Node.EmbeddedScriptGetterCommand( nodeNumber ) );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

            string embeddedNodeScriptNames = session.ReadLineTrimEnd();

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            // query and throw if operation complete query failed
            session.QueryAndThrowIfOperationIncomplete();
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            return embeddedNodeScriptNames;
        }
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that queries if all scripts are embedded. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <param name="scripts">      The scripts. </param>
    /// <returns>   True if all scripts are embedded, false if it fails. </returns>
    public static bool AllEmbedded( this Pith.SessionBase session, int nodeNumber, ScriptInfoBaseCollection<ScriptInfo> scripts )
    {
        string scriptNames = session.FetchEmbeddedScriptsNames( nodeNumber );
        bool affirmative = true;
        foreach ( ScriptInfo script in scripts )
        {
            affirmative = scriptNames.Contains( $"{script.Title}," );
            if ( !affirmative )
            {
                session.TraceLastAction( $"\r\n\tscript {script.Title} is not embedded;. " );
                break;
            }
        }
        return affirmative;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that checks if all scripts are embedded on the specified node. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="scripts">  The scripts. </param>
    /// <returns>   True if all scripts are embedded, false if it fails. </returns>
    public static bool AllEmbedded( this Pith.SessionBase session, NodesScriptsCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        bool affirmative = true;
        foreach ( ScriptInfoCollection scriptInfoCollection in scripts.Values )
        {
            if ( scriptInfoCollection is not null )
            {
                affirmative = (scriptInfoCollection.NodeNumber == 0) || (scriptInfoCollection.NodeNumber == session.QueryControllerNodeNumber())
                    ? session.AllEmbedded( scriptInfoCollection )
                    : session.AllEmbedded( scriptInfoCollection.NodeNumber, scriptInfoCollection );
                if ( !affirmative ) break;

            }
        }
        return affirmative;
    }


    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that query if the script is included in the
    /// list of embedded scripts on the node.
    /// </summary>
    /// <remarks>   2025-04-12. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <returns>   True if the script is embedded, false if not. </returns>
    public static bool IsScriptEmbedded( this SessionBase session, string scriptName, int nodeNumber )
    {
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

        int controllerNodeNumber = session.QueryControllerNodeNumber();

        if ( controllerNodeNumber == nodeNumber )
        {
            return session.IsScriptEmbedded( scriptName );
        }
        else
        {
            string findCommand = Syntax.Tsp.Node.FindEmbeddedScriptCommand( scriptName, nodeNumber );
            string reply = session.QueryTrimEnd( findCommand );
            return SessionBase.EqualsTrue( reply );
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that removes the script fo the catalog of
    /// embedded scripts on the node and null the script.
    /// </summary>
    /// <remarks>   2025-04-21. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <param name="scriptName">   Name of the script. </param>
    ///
    /// ### <returns>
    /// True if the script is no longer embedded on the node, false if it fails.
    /// </returns>
    public static void DeleteEmbeddedScript( this Pith.SessionBase session, int nodeNumber, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = nodeNumber;

        // TODO: Do we need to enable completion detection on the node?
        session.SetLastAction( $"enabling service request on operation completion" );
        session.EnableServiceRequestOnOperationCompletion();

        session.SetLastAction( $"removing script '{scriptName}'" );
        _ = session.ExecuteCommandQueryComplete( nodeNumber, $"script.delete('{scriptName}')" );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        string reply = session.ReadLineTrimEnd();
        if ( reply != cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue )
            throw new InvalidOperationException( $"{session.ResourceNameNodeCaption} operation complete query reply '{reply}' should be '1';. " );

        session.ThrowDeviceExceptionIfError();

        session.SetLastAction( $"nulling script '{scriptName}" );
        session.NillObject( nodeNumber, scriptName );

        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   A Pith.SessionBase extension method that removes the embedded script from thecatalog of embedded scripts on the node.. </summary>
    /// <remarks>   2025-09-29. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <param name="scriptName">   Name of the script. </param>
    public static void RemoveEmbeddedScript( this Pith.SessionBase session, int nodeNumber, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = nodeNumber;

        // TODO: Do we need to enable completion detection on the node?
        session.SetLastAction( $"enabling service request on operation completion" );
        session.EnableServiceRequestOnOperationCompletion();

        session.SetLastAction( $"removing script '{scriptName}'" );
        _ = session.ExecuteCommandQueryComplete( nodeNumber, $"script.delete('{scriptName}')" );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        string reply = session.ReadLineTrimEnd();
        if ( reply != cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue )
            throw new InvalidOperationException( $"{session.ResourceNameNodeCaption} operation complete query reply '{reply}' should be '1';. " );

        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that embeds a script on the node. </summary>
    /// <remarks>   2025-04-27. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <param name="scriptName">   Name of the script. </param>
    public static void EmbedScript( this Pith.SessionBase session, int nodeNumber, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        session.SetLastAction( $"embedding script '{scriptName}' on node {nodeNumber}" );
        _ = session.ExecuteCommandWaitComplete( nodeNumber, $"script.user.scripts.{scriptName}.save() {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} " );

        session.ThrowDeviceExceptionIfError();

        session.SetLastAction( $"nulling script '{scriptName}" );
        session.NillObject( nodeNumber, scriptName );

        session.ThrowDeviceExceptionIfError();

        if ( !session.IsScriptEmbedded( scriptName, nodeNumber ) )
            throw new InvalidOperationException( $"The script {scriptName} failed to embed on node {nodeNumber}." );

    }
}
