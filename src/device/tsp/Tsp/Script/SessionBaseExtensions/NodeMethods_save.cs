using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that fetches saved scripts names. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="ArgumentOutOfRangeException">  Thrown when one or more arguments are outside
    ///                                                 the required range. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <returns>   The saved scripts. </returns>
    public static string FetchSavedScriptsNames( this Pith.SessionBase? session, int nodeNumber )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        if ( nodeNumber < 0 )
            throw new ArgumentOutOfRangeException( nameof( nodeNumber ), "Node number must be greater than or equal to zero." );

        int controllerNodeNumber = session.QueryControllerNodeNumber();

        if ( controllerNodeNumber == nodeNumber )
        {
            return session.FetchSavedScriptsNames();
        }
        else
        {
            session.LastNodeNumber = nodeNumber;
            session.SetLastAction( $"fetching saved scripts from node {nodeNumber}" );
            _ = session.WriteLine( Syntax.Tsp.Node.SavedScriptGetterCommand( nodeNumber ) );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

            string savedNodScriptNames = session.ReadLineTrimEnd();

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            // query and throw if operation complete query failed
            session.QueryAndThrowIfOperationIncomplete();
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            return savedNodScriptNames;
        }
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that all saved. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <param name="scripts">      The scripts. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool AllSaved( this Pith.SessionBase session, int nodeNumber, ScriptInfoBaseCollection<ScriptInfo> scripts )
    {
        string scriptNames = session.FetchSavedScriptsNames( nodeNumber );
        bool affirmative = true;
        foreach ( ScriptInfo script in scripts )
        {
            affirmative = scriptNames.Contains( $"{script.Title}," );
            if ( !affirmative )
            {
                session.TraceLastAction( $"script {script.Title} was not saved;. " );
                break;
            }
        }
        return affirmative;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that all saved. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="scripts">  The scripts. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool AllSaved( this Pith.SessionBase session, NodesScriptsCollection scripts )
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
                    ? session.AllSaved( scriptInfoCollection )
                    : session.AllSaved( scriptInfoCollection.NodeNumber, scriptInfoCollection );
                if ( !affirmative ) break;

            }
        }
        return affirmative;
    }


    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that query if 'session' is saved script.
    /// </summary>
    /// <remarks>   2025-04-12. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <returns>   True if saved script, false if not. </returns>
    public static bool IsSavedScript( this SessionBase session, string scriptName, int nodeNumber )
    {
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

        int controllerNodeNumber = session.QueryControllerNodeNumber();

        if ( controllerNodeNumber == nodeNumber )
        {
            return session.IsSavedScript( scriptName );
        }
        else
        {
            string findCommand = Syntax.Tsp.Node.FindSavedScriptCommand( scriptName, nodeNumber );
            string reply = session.QueryTrimEnd( findCommand );
            return SessionBase.EqualsTrue( reply );
        }
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that removes the saved script. </summary>
    /// <remarks>   2025-04-21. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static void RemoveSavedScript( this Pith.SessionBase? session, int nodeNumber, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = nodeNumber;

        // TODO: Do we need to enable completion detection on the node?
        session.SetLastAction( $"enabling service request on operation completion" );
        session.EnableServiceRequestOnOperationCompletion();

        session.SetLastAction( $"deleting script '{scriptName}'" );
        _ = session.ExecuteCommandQueryComplete( nodeNumber, $"script.delete('{scriptName}')" );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        string reply = session.ReadLineTrimEnd();
        if ( reply != cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue )
            throw new InvalidOperationException( $"{session.ResourceNameNodeCaption} operation complete query reply '{reply}' should be '1';. " );

        session.ThrowDeviceExceptionIfError();

        session.SetLastAction( $"nulling script '{scriptName}" );
        session.NillScript( nodeNumber, scriptName );

        session.ThrowDeviceExceptionIfError();
    }
}
