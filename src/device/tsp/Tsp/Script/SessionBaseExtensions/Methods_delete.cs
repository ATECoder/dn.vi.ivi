using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that query if <paramref name="scriptName"/>
    /// exists, is a named script and is listed in the catalog of user scripts.
    /// </summary>
    /// <remarks>   2025-10-06. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   True if <paramref name="scriptName"/> is a user script, false if not. </returns>
    public static bool IsUserScript( this SessionBase session, string scriptName )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        return !(session.IsNil( scriptName ) || session.IsNil( $"script.user.scripts.{scriptName}" ));
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that query if <paramref name="scriptName"/>
    /// is a loaded script.
    /// </summary>
    /// <remarks>   2025-10-06. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   True if loaded script, false if not. </returns>
    public static bool IsLoadedScript( this SessionBase session, string scriptName )
    {
        return session.IsUserScript( scriptName );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that removes the script from the list of user scripts. </summary>
    /// <remarks>   2025-04-11. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
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

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that query if the <paramref name="scriptName"/> exists. The script exists if 
    /// it <see cref="IsScriptEmbedded(SessionBase, string)"/> or <see cref="IsUserScript(SessionBase, string)"/> of not <see cref="SessionBase.IsNil(string)"/>.
    /// </summary>
    /// <remarks>   2025-10-06. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <param name="details">      [out] The details indicating if the script is either embedded, or a user script or is not nil. </param>
    /// <returns>   True if script deleted, false if not. </returns>
    public static bool IsScriptExists( this SessionBase session, string scriptName, out string details )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        details = string.Empty;
        if ( session.IsScriptEmbedded( scriptName ) )
            details = $"script '{scriptName}' is embedded;. ";
        else if ( session.IsUserScript( scriptName ) )
            details = $"script '{scriptName}' is a user script;. ";
        else if ( !session.IsNil( scriptName ) )
            details = $"script '{scriptName}' is not nil;. ";

        return !string.IsNullOrWhiteSpace( details );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that deletes the script. </summary>
    /// <remarks>   2025-11-18. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="validate">     True to validate if the script was deleted. </param>
    public static void DeleteScript( this SessionBase session, string scriptName, bool validate )
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

        // removes the embedded script from the catalog of embedded scripts.
        session.RemoveEmbeddedScript( scriptName );

        // remove the script from the list of user scripts.
        session.RemoveUserScript( scriptName );

        // nill the script if is is not nil.
        session.NillObject( scriptName );

        if ( validate && session.IsScriptExists( scriptName, out string details ) )
            throw new InvalidOperationException( $"The script '{scriptName}' was not deleted;. {details}" );
    }


    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that attempts to delete script. </summary>
    /// <remarks>   2025-10-06. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <param name="validate">     True to validate if the script was deleted. </param>
    /// <param name="details">      [out] The details indicating if the script is either embedded, or
    ///                             a user script or is not nil. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool TryDeleteScript( this SessionBase session, string scriptName, bool validate, out string details )
    {
        details = string.Empty;
        try
        {
            session.DeleteScript( scriptName, validate );
        }
        catch ( InvalidOperationException ex )
        {
            details = ex.Message;
        }
        catch ( Exception ex )
        {
            details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that deletes the scripts.
    /// </summary>
    /// <remarks>   2025-11-06. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptNames">  List of names of the scripts. </param>
    /// <returns>   A Tuple. </returns>
    public static (int TtmScriptCount, int deletedScriptCount) DeleteScripts( this SessionBase session, IEnumerable<string> scriptNames )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptNames is null ) throw new ArgumentNullException( nameof( scriptNames ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        int removedCount = 0;
        int scriptCount = scriptNames.Count();

        if ( scriptCount == 0 )
        {
            session.DisplayLine( "No scripts to delete", 2 );
            _ = TraceLastAction( $"\r\n\tNo scripts to delete." );
            return (scriptCount, removedCount);
        }

        session.TraceLastAction( "enabling service request on operation completion" );
        session.EnableServiceRequestOnOperationCompletion();


        session.DisplayLine( "Deleting scripts", 1 );

        session.LastNodeNumber = default;
        foreach ( string scriptName in scriptNames )
        {
            string scriptTitle = scriptName.Trim();

            if ( string.IsNullOrWhiteSpace( scriptTitle ) )
                continue;

            removedCount += 1;
            session.DisplayLine( $"Removing {scriptTitle}", 2 );
            session.DeleteScript( scriptTitle, true );
        }

        string message = $"Removed {removedCount} of {scriptCount} TTM embedded scripts";
        session.DisplayLine( message, 2 );
        _ = TraceLastAction( $"\r\n\t{message}" );
        return (scriptCount, removedCount);
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that deletes the embedded scripts.
    /// </summary>
    /// <remarks>   2025-05-13. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="prefixFilter"> (Optional) A filter specifying the prefix. </param>
    /// <returns>   A tuple: TTM embedded script count, Deleted Scripts count). </returns>
    public static (int TtmScriptCount, int deletedScriptCount) DeleteEmbeddedScripts( this SessionBase session, string prefixFilter = "isr_" )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        session.TraceLastAction( "enabling service request on operation completion" );
        session.EnableServiceRequestOnOperationCompletion();

        string embeddedScripts = session.FetchEmbeddedScriptsNames();

        if ( string.IsNullOrWhiteSpace( embeddedScripts ) )
        {
            session.DisplayLine( "No embedded scripts to delete", 2 );
            _ = TraceLastAction( $"\r\n\tNo embedded scripts to delete." );
            return (0, 0);
        }

        IEnumerable<string> scriptNames = embeddedScripts.FilterScriptNamesByPrefix( prefixFilter );

        return session.DeleteScripts( scriptNames );
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
                session.DeleteScript( script.Title, true );
            }
        }

        if ( removedCount == 0 )
            _ = TraceLastAction( $"\r\n\tNo scripts to remove for {frameworkName}." );
        else
            _ = TraceLastAction( $"\r\n\t{removedCount} scripts were removed for {frameworkName}." );
    }

}
