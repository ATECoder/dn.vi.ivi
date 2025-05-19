using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   Fetches the names of the saved scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    /// <returns>   The saved scripts. </returns>
    public static string FetchSavedScriptsNames( this Pith.SessionBase session, bool consoleOut = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.LastNodeNumber = default;
        string scriptNames;
        string message = "fetching saved scripts";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );

        _ = session.WriteLine( "do {0} print( names ) end ", Syntax.Tsp.Script.ScriptCatalogGetterCommand );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

        scriptNames = session.ReadLineTrimEnd();

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        return scriptNames;
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that check if a script is included in the catalog of saved scripts.
    /// </summary>
    /// <remarks>   2025-04-11. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool IsSavedScript( this SessionBase session, string scriptName )
    {
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );
        // string findCommand = $"local exists = false for name in script.user.catalog() do exists = (name=='{scriptName}') end print(exists)";
        string findCommand = string.Format( Syntax.Tsp.Script.FindSavedScriptQueryFormat, scriptName );
        string reply = session.QueryTrimEnd( findCommand );
        return SessionBase.EqualsTrue( reply );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that all saved. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scripts">      The scripts. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool AllSaved( this Pith.SessionBase session, ScriptInfoBaseCollection<ScriptInfo> scripts, bool consoleOut = false )
    {
        string scriptNames = session.FetchSavedScriptsNames();
        bool affirmative = true;
        foreach ( ScriptInfo script in scripts )
        {
            affirmative = scriptNames.Contains( $"{script.Title}," );
            if ( !affirmative )
            {
                string message = $"script {script.Title} was not saved;. ";
                if ( consoleOut )
                    SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                else
                    SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
                break;
            }
        }
        return affirmative;
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that removed a script from Non-Volatile-
    /// Memory (NVM).
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is v
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    public static void RemoveSavedScript( this SessionBase session, string scriptName, bool consoleOut = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;
        session.SetLastAction( $"checking if a '{scriptName}' saved script exists;. " );
        string message;
        if ( session.IsSavedScript( scriptName ) )
        {
            message = $"removing the '{scriptName}' script from the saved script catalog;. ";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );

            _ = session.WriteLine( $"script.delete('{scriptName}')" );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            // query and throw if operation complete query failed
            session.QueryAndThrowIfOperationIncomplete();
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            message = $"checking if the saved '{scriptName}' script was deleted;. ";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );

            if ( !session.IsSavedScript( scriptName ) )
                SessionBaseExtensionMethods.TraceLastAction( "script was deleted;. " );
            else
                throw new InvalidOperationException( $"Deletion of the {scriptName} saved script failed. The script still exists among the saved scripts;. " );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            // query and throw if operation complete query failed
            session.QueryAndThrowIfOperationIncomplete();
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that saves a script to non-volatile-memory
    /// (NVM).
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    public static void SaveScript( this SessionBase session, string scriptName, bool consoleOut = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

        string message = $"checking if the {scriptName} script was loaded;. ";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );

        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be saved because it was not loaded." );

        message = $"checking if the {scriptName} saved script exists;. ";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
        session.LastNodeNumber = default;

        message = $"saving the {scriptName} script;. ";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );

        _ = session.WriteLine( $"{scriptName}.save()" );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        if ( !session.IsSavedScript( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} failed to be saved." );
    }
}
