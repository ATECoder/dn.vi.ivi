using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that removes the user script. </summary>
    /// <remarks>   2025-04-21. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the remote node number. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    public static void RemoveUserScript( this Pith.SessionBase session, int nodeNumber, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;
        session.SetLastAction( $"checking if the {scriptName} script is listed as user script on node {nodeNumber};. " );
        if ( !session.IsNil( nodeNumber, $"script.user.scripts.{scriptName}" ) )
        {
            session.TraceLastAction( $"\r\n\tRemoving '{scriptName} script from the user scripts on node {nodeNumber};. " );
            _ = session.ExecuteCommandQueryComplete( nodeNumber, "script.user.scripts.{0}.name = ''", scriptName );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();
        }

        if ( !session.IsNil( nodeNumber, $"script.user.scripts.{scriptName}" ) )
            throw new InvalidOperationException( $"The script {scriptName} was not removed from the user scripts on node {nodeNumber};. " );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that deletes the script. </summary>
    /// <remarks>   2025-04-21. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the remote node number. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    public static void DeleteScript( this Pith.SessionBase session, int nodeNumber, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.RemoveSavedScript( nodeNumber, scriptName );

        session.RemoveUserScript( nodeNumber, scriptName );

        session.NillObject( nodeNumber, scriptName );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that deletes the scripts.
    /// </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="frameworkName">    Name of the framework. </param>
    /// <param name="nodeNumber">       Specifies the remote node number. </param>
    /// <param name="scripts">          The scripts. </param>
    public static void DeleteScripts( this SessionBase session, string frameworkName, int nodeNumber, ScriptInfoCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        session.TraceLastAction( "enabling service request on operation completion" );
        session.EnableServiceRequestOnOperationCompletion();

        int removedCount = 0;
        session.LastNodeNumber = nodeNumber;
        foreach ( ScriptInfo script in scripts )
        {
            if ( !session.IsNil( nodeNumber, script.Title ) )
            {
                session.DisplayLine( $"Deleting {frameworkName}", 1 );
                session.DisplayLine( $"Removing {script.Title}", 2 );
                removedCount += 1;
                session.DeleteScript( nodeNumber, script.Title );
            }
        }

        if ( removedCount == 0 )
            Trace.WriteLine( $"No scripts to remove for {frameworkName} node number {nodeNumber}." );
        else
            Trace.WriteLine( $"{removedCount} scripts were removed for {frameworkName} node number {nodeNumber}." );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that deletes the scripts.
    /// </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="frameworkName">    Name of the framework. </param>
    /// <param name="nodeScripts">          The scripts. </param>
    public static void DeleteScripts( this SessionBase session, string frameworkName, NodesScriptsCollection nodeScripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( nodeScripts is null ) throw new ArgumentNullException( nameof( nodeScripts ) );

        foreach ( ScriptInfoCollection scriptInfoCollection in nodeScripts.Values )
        {
            if ( scriptInfoCollection is not null )
            {
                if ( (scriptInfoCollection.NodeNumber == 0) || (scriptInfoCollection.NodeNumber == session.QueryControllerNodeNumber()) )
                    session.DeleteScripts( frameworkName, scriptInfoCollection );
                else
                    session.DeleteScripts( frameworkName, scriptInfoCollection.NodeNumber, scriptInfoCollection );
            }
        }
    }
}
