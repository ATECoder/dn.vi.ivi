using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>
    /// Fetches the names of the saved scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <returns>   The saved scripts. </returns>
    public static string FetchSavedScriptsNames( this Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.LastNodeNumber = default;
        string scriptNames;
        session.TraceLastAction( "fetching saved scripts" );
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
        string findCommand = string.Format( Syntax.Tsp.Script.FindSaveScriptQueryFormat, scriptName );
        string reply = session.QueryTrimEnd( findCommand );
        return SessionBase.EqualsTrue( reply );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that removed a script from Non-Volatile-Memory (NVM). </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                      v                          invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptName">       Name of the script. </param>
    public static void RemoveSavedScript( this SessionBase session, string scriptName )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;
        session.SetLastAction( $"checking if a '{scriptName}' saved script exists;. " );
        if ( session.IsSavedScript( scriptName ) )
        {
            session.TraceLastAction( $"removing the '{scriptName}' script from the saved script catalog;. " );
            _ = session.WriteLine( $"script.delete('{scriptName}')" );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            // query and throw if operation complete query failed
            session.QueryAndThrowIfOperationIncomplete();
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            session.SetLastAction( $"checking if the saved '{scriptName}' script was deleted;. " );
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

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that saves a script to non-volatile-memory (NVM). </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    public static void SaveScript( this SessionBase session, string scriptName )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be run because it was not found." );

        session.SetLastAction( $"checking if the {scriptName} saved script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsSavedScript( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} is already saved." );

        session.TraceLastAction( $"saving the {scriptName} script;. " );
        _ = session.WriteLine( $"{scriptName}.save()" );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();
    }


}
