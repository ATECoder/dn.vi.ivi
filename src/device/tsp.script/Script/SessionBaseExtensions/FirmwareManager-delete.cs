using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Checks if delete is required on any node. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                      The session. </param>
    /// <param name="scriptsCollection">            Specifies the list of nodes on which scripts are
    ///                                             deleted. </param>
    /// <param name="allowDeletingNewerScripts">    true to allow, false to deny deleting newer
    ///                                             scripts. </param>
    /// <param name="refreshScriptsCatalog">        Refresh catalog before checking if script exists. </param>
    /// <returns>   <c>true</c> if delete is required on any node. </returns>
    public static bool IsDeleteUserScriptRequired( this Pith.SessionBase session, ICollection<ScriptEntityCollection> scriptsCollection, bool allowDeletingNewerScripts, bool refreshScriptsCatalog )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptsCollection is null ) throw new ArgumentNullException( nameof( scriptsCollection ) );

        bool deleteRequired = false;
        foreach ( ScriptEntityCollection scripts in scriptsCollection )
        {
            if ( refreshScriptsCatalog )
                scripts.ReadScriptsState( session );

            deleteRequired = !deleteRequired && scripts.IsDeleteUserScriptRequired( allowDeletingNewerScripts );
        }

        return deleteRequired;
    }

    /// <summary>   Removes the script from the device and collect garbage. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  A reference to a
    ///                                         <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                         subsystem</see>. </param>
    /// <param name="scriptName">               Name of the script. </param>
    /// <returns>   A Tuple. </returns>
    public static bool DeleteSavedScriptCollectGarbage( this Pith.SessionBase? session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        bool deleted = FirmwareManager.DeleteSavedScript( session, scriptName );

        if ( !session.CollectGarbageQueryComplete() )
            _ = session.TraceWarning( message: $"garbage collection incomplete (reply not '1') after deleting saved script {scriptName}" );

        _ = session.TraceDeviceExceptionIfError( failureMessage: "ignoring error after deleting scripts." );

        return deleted;
    }

    /// <summary>
    /// Deletes the <paramref scriptName="scriptName">specified</paramref> saved script. Also nulls
    /// the script if delete command worked. Uses <see cref="SessionBase.IsNil(string)"/> to
    /// determine if script was deleted.
    /// </summary>
    /// <remarks>   Assumes the script is known to exist. Waits for operation completion. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   A Tuple. </returns>
    public static bool DeleteSavedScript( this Pith.SessionBase? session, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;

        session.SetLastAction( $"deleting script '{scriptName}" );
        _ = session.WriteLine( $"script.delete('{scriptName}') {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );

        // read query reply and throw if reply is not 1.
        session.ReadAndThrowIfOperationIncomplete();

        // throw if device errors
        session.ThrowDeviceExceptionIfError();

        session.SetLastAction( $"nulling script '{scriptName}" );
        session.NillObject( scriptName );
        bool deleted = true;

        session.ThrowDeviceExceptionIfError();

        return deleted;
    }

    /// <summary>   Removes the script from the device. </summary>
    /// <remarks>   2024-09-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  A reference to a
    ///                                         <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                         subsystem</see>. </param>
    /// <param name="node">                     Specifies the node entity. </param>
    /// <param name="scriptName">               Name of the script. </param>
    /// <returns>   <c>true</c> if the scripts deleted; otherwise <c>false</c>. </returns>
    public static bool DeleteSavedScripts( this Pith.SessionBase? session, NodeEntityBase? node, string scriptName )
    {
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        bool deleted = session.DeleteSavedScript( node.Number, scriptName );

        if ( !session.CollectGarbageQueryComplete( node.Number ) )
            _ = session.TraceWarning( message: $"garbage collection incomplete (reply not '1') after deleting saved script {scriptName} on node {node.Number}" );

        _ = session.TraceDeviceExceptionIfError( failureMessage: $"ignoring error after deleting saved user script {scriptName} on node {node.Number}" );

        return deleted;
    }

    /// <summary>
    /// Deletes the <paramref scriptName="scriptName">specified</paramref> saved script. Also nulls
    /// the script if delete command worked. Then checks if the script was deleted and if so returns
    /// true. Otherwise, returns false.
    /// </summary>
    /// <remarks>   Presumes the saved script exists. Waits for operation completion. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="nodeNumber">           Specifies the remote node number. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <returns>   <c>true</c> if the script is nil; otherwise <c>false</c>. </returns>
    public static bool DeleteSavedScript( this Pith.SessionBase? session, int nodeNumber, string? scriptName )
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
        session.NillObject( nodeNumber, scriptName );

        session.ThrowDeviceExceptionIfError();

        return true;
    }

    /// <summary>   Deletes the script. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      A reference to a
    ///                             <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                             subsystem</see>. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   <c>true</c> if the script is nil; otherwise <c>false</c>. </returns>
    public static bool TryDeleteSavedScript( this Pith.SessionBase? session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        session.DeleteScript( scriptName );
        return !session.IsNil( scriptName );
    }

    /// <summary>   Deletes the user scripts. </summary>
    /// <remarks>   2024-09-24. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="scriptsCollection">    Collection of scripts. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0306:Simplify collection initialization", Justification = "<Pending>" )]
    public static void DeleteUserScripts( this Pith.SessionBase session, ICollection<ScriptEntityCollection> scriptsCollection )
    {
        if ( scriptsCollection is null ) throw new ArgumentNullException( nameof( scriptsCollection ) );
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        foreach ( ScriptEntityCollection scripts in scriptsCollection )
        {
            string savedScripts = session.FetchSavedScriptsNames( scripts.Node ).SavedScripts;
            session.LastNodeNumber = scripts.Node.Number;
            Queue<ScriptEntityBase> queue = new( scripts.Reverse() );
            while ( queue.Count > 0 )
            {
                ScriptEntityBase script = queue.Dequeue();
                if ( !session.IsNil( script.Name ) )
                {
                    if ( savedScripts.Contains( script.Name, StringComparison.OrdinalIgnoreCase ) )
                        session.DeleteScript( script.Name );
                    else
                        session.NillObject( script.Name );
                }
            }
        }
    }
}
