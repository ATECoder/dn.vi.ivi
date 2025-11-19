using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that query if the script was activated.
    /// </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   . </param>
    /// <param name="script">       The script. </param>
    /// <returns>   True if script namespaces, false if not. </returns>
    public static bool IsActive( this Pith.SessionBase session, int nodeNumber, ScriptInfo script )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        return !string.IsNullOrWhiteSpace( script.VersionGetterElement )
            && session.IsControllerNode( nodeNumber )
                ? session.IsNil( script.VersionGetterElement )
                : session.IsNil( nodeNumber, script.VersionGetterElement );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that runs the script. </summary>
    /// <remarks>   2025-04-21. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="nodeNumber">           Specifies the remote node number. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <param name="scriptElementName">    (Optional) Name of the script element. </param>
    public static void RunScript( this Pith.SessionBase session, int nodeNumber, string? scriptName, string scriptElementName = "" )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        if ( !session.IsNil( scriptName ) )
        {
            session.SetLastAction( $"Running script '{scriptName}' on node {nodeNumber}" );
            _ = session.WriteLine( $"node[{nodeNumber}].execute( '{scriptName}.run()' ) print('{cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue}') " );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();

            if ( !string.IsNullOrWhiteSpace( scriptElementName ) )
            {
                session.SetLastAction( $"looking for {scriptElementName};. " );
                if ( session.IsNil( nodeNumber, scriptElementName ) )
                    throw new InvalidOperationException( $"The script element {scriptElementName} was not found after running the {scriptName} script on node {nodeNumber}." );
            }
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that runs the scripts.
    /// </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="nodeNumber">       Specifies the remote node number. </param>
    /// <param name="scripts">          The scripts. </param>
    public static void RunScripts( this SessionBase session, int nodeNumber, ScriptInfoCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        foreach ( ScriptInfo script in scripts )
        {
            if ( !session.IsNil( nodeNumber, script.Title ) )
            {
                session.RunScript( nodeNumber, script.Title, script.VersionGetterElement );
            }
        }
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that runs the scripts. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeScripts">  The scripts. </param>
    public static void RunScripts( this SessionBase session, NodesScriptsCollection nodeScripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( nodeScripts is null ) throw new ArgumentNullException( nameof( nodeScripts ) );

        foreach ( ScriptInfoCollection scriptInfoCollection in nodeScripts.Values )
        {
            if ( scriptInfoCollection is not null )
            {
                if ( (scriptInfoCollection.NodeNumber == 0) || (scriptInfoCollection.NodeNumber == session.QueryControllerNodeNumber()) )
                    session.RunScripts( scriptInfoCollection );
                else
                    session.RunScripts( scriptInfoCollection.NodeNumber, scriptInfoCollection );
            }
        }
    }

}
