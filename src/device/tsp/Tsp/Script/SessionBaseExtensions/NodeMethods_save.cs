using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{
    /// <summary>   A <see cref="SessionBase"/> extension method that fetches saved scripts names. </summary>
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

    /// <summary>
    /// A <see cref="SessionBase"/> extension method that query if 'session' is saved script.
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
}
