using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that removes the script from the list of user scripts. </summary>
    /// <remarks>   2025-04-11. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static void RemoveUserScript( this Pith.SessionBase session, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;
        session.SetLastAction( $"checking if the {scriptName} script is listed as user script;. " );
        if ( !session.IsNil( $"script.user.scripts.{scriptName}" ) )
        {
            session.TraceLastAction( $"\r\n\tremoving '{scriptName} script from the user scripts." );
            _ = session.WriteLine( $"script.user.scripts.{scriptName}.name = '' {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();
        }

        if ( !session.IsNil( $"script.user.scripts.{scriptName}" ) )
            throw new InvalidOperationException( $"The script {scriptName} was not removed from the user scripts." );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that deletes the script. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptName">       Name of the script. </param>
    public static void DeleteScript( this SessionBase session, string scriptName )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        // turn off auto run
        session.TraceLastAction( $"Checking auto run on {scriptName}" );
        if ( session.IsAutoRun( scriptName ) )
        {
            session.TraceLastAction( $"disabling auto run on {scriptName}" );
            session.TurnOffAutoRun( scriptName );
        }

        // removes the saved script from the catalog of saved scripts. 
        session.RemoveSavedScript( scriptName );

        // remove the script from the list of user scripts.
        session.RemoveUserScript( scriptName );

        // nill the script if is is not nil.
        session.NillObject( scriptName );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that deletes the saved scripts described by
    /// session.
    /// </summary>
    /// <remarks>   2025-05-13. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="prefixFilter"> (Optional) A filter specifying the prefix. </param>
    /// <returns>   A tuple: TTM embedded script count, Deleted Scripts count). </returns>
    public static (int TtmScriptCount, int deletedScriptCount) DeleteSavedScripts( this SessionBase session, string prefixFilter = "isr_" )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        session.TraceLastAction( "enabling service request on operation completion" );
        session.EnableServiceRequestOnOperationCompletion();

        string savedScripts = session.FetchSavedScriptsNames();

        int removedCount = 0;
        string[] scriptNames = savedScripts.Split( ',' );
        int scriptCount = scriptNames.Length;

        session.DisplayLine( "Deleting scripts", 1 );

        session.LastNodeNumber = default;
        foreach ( string scriptName in scriptNames )
        {
            string scriptTitle = scriptName.Trim();

            if ( string.IsNullOrWhiteSpace( scriptTitle ) )
                continue;

            if ( scriptTitle.StartsWith( prefixFilter ) )
            {
                removedCount += 1;
                session.DisplayLine( $"Removing {scriptTitle}", 2 );
                session.DeleteScript( scriptTitle );
            }
            else
            {
                // if the script is non isr script, decrement the script count and skip it.
                session.DisplayLine( $"Skipping {scriptTitle} script", 2 );
                scriptCount -= 1;
            }
        }

        string message = $"Removed {removedCount} of {scriptCount} TTM embedded scripts";
        session.DisplayLine( message, 2 );
        TraceLastAction( $"\r\n\t{message}" );
        return (scriptCount, removedCount);
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
    /// <param name="scripts">          The scripts. </param>
    public static void DeleteScripts( this SessionBase session, string frameworkName, ScriptInfoCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        session.TraceLastAction( "enabling service request on operation completion" );
        session.EnableServiceRequestOnOperationCompletion();

        int removedCount = 0;
        session.LastNodeNumber = default;
        foreach ( ScriptInfo script in scripts )
        {
            if ( !session.IsNil( script.Title ) )
            {
                session.DisplayLine( $"Deleting {frameworkName}", 1 );
                session.DisplayLine( $"Removing {script.Title}", 2 );
                removedCount += 1;
                session.DeleteScript( script.Title );
            }
        }

        if ( removedCount == 0 )
            TraceLastAction( $"\r\n\tNo scripts to remove for {frameworkName}." );
        else
            TraceLastAction( $"\r\n\t{removedCount} scripts were removed for {frameworkName}." );
    }

}
