using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that queries if a script is embedded and byte code. </summary>
    /// <remarks>   2025-10-04. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if script embedded, false if not. </returns>
    public static bool IsScriptEmbeddedAndByteCode( this Pith.SessionBase session, ScriptInfo scriptInfo, out string details )
    {
        if ( !session.IncludedInLoadedTable( scriptInfo.RequireChunkName, out details ) )
            details = $"Script '{scriptInfo.Title}' is not embedded; {details}.";
        else if ( !session.IsInLoaded( scriptInfo.RequiredChunkName, out details ) )
            details = $"Script '{scriptInfo.Title}' is not embedded; {details}.";
        else if ( !session.IsScriptEmbedded( scriptInfo.Title ) )
            details = $"Script '{scriptInfo.Title}' is not embedded.";
        else if ( !session.IsByteCodeScript( scriptInfo.Title ) )
            details = $"Script '{scriptInfo.Title}' is not in byte code.";
        else if ( scriptInfo.IsAutoexec && !session.IsAutoRun( scriptInfo.Title ) )
            details = $"Script '{scriptInfo.Title}' is not set to auto run.";
        else
            details = string.Empty;
        if ( string.IsNullOrEmpty( details ) )
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\tScript '{scriptInfo.Title}' is embedded;. " );
        return string.IsNullOrWhiteSpace( details );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that fetches the names of the embedded scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    /// <returns>   The embedded scripts. </returns>
    public static string FetchEmbeddedScriptsNames( this Pith.SessionBase session, bool consoleOut = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.LastNodeNumber = default;
        string scriptNames;
        string message = "fetching the names of the embedded scripts";
        if ( consoleOut )
            _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        else
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

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
    /// A <see cref="string"/> extension method that filters script names by prefix in this collection. </summary>
    /// <remarks>   2025-11-06. </remarks>
    /// <param name="scriptNames">  The scriptNames to act on. </param>
    /// <param name="prefixFilter"> (Optional) A filter specifying the prefix. </param>
    /// <returns>
    /// An enumerator that allows foreach to be used to process filter script names by prefix in this
    /// collection.
    /// </returns>
    public static IEnumerable<string> FilterScriptNamesByPrefix( this string scriptNames, string prefixFilter = "isr_" )
    {
        string[] filteredScriptNames = scriptNames.Split( ',' );
        return filteredScriptNames.Where( name => name.StartsWith( prefixFilter, StringComparison.OrdinalIgnoreCase ) );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that checks if a script is included in the
    /// catalog of embedded scripts (TSP: _G.script.user.catalog()).
    /// </summary>
    /// <remarks>   2025-04-11. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool IsScriptEmbedded( this SessionBase session, string scriptName )
    {
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );
        // string findCommand = $"local exists = false for name in script.user.catalog() do exists = (name=='{scriptName}') end print(exists)";
        string findCommand = string.Format( Syntax.Tsp.Script.FindEmbeddedScriptQueryFormat, scriptName );
        string reply = session.QueryTrimEnd( findCommand );
        return SessionBase.EqualsTrue( reply );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that queries if all the provided scripts are embedded. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scripts">      The scripts. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool AllEmbedded( this Pith.SessionBase session, ScriptInfoBaseCollection<ScriptInfo> scripts, bool consoleOut = false )
    {
        string scriptNames = session.FetchEmbeddedScriptsNames();
        bool affirmative = true;
        foreach ( ScriptInfo script in scripts )
        {
            affirmative = scriptNames.Contains( $"{script.Title}," );
            if ( !affirmative )
            {
                string message = $"script {script.Title} is not embedded;. ";
                if ( consoleOut )
                    _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
                else
                    _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );
                break;
            }
        }
        return affirmative;
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that removes a script from Non-Volatile-
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
    public static void RemoveEmbeddedScript( this SessionBase session, string scriptName, bool consoleOut = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;
        session.SetLastAction( $"checking if the '{scriptName}' script is embedded;. " );
        string message;
        if ( session.IsScriptEmbedded( scriptName ) )
        {
            message = $"removing the '{scriptName}' script from the embedded script catalog;. ";
            if ( consoleOut )
                _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            else
                _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

            _ = session.WriteLine( $"script.delete('{scriptName}')" );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            // query and throw if operation complete query failed
            session.QueryAndThrowIfOperationIncomplete();
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

            // throw if device error occurred
            session.ThrowDeviceExceptionIfError();

            message = $"checking if the '{scriptName}' script is no longer embedded;. ";
            if ( consoleOut )
                _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            else
                _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

            if ( !session.IsScriptEmbedded( scriptName ) )
                _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( "script is not longer embedded;. " );
            else
                throw new InvalidOperationException( $"Removing the '{scriptName}' script for the embedded catalog. The script still exists among the embedded scripts;. " );

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
    /// A <see cref="Pith.SessionBase"/> extension method that embeds a script in non-volatile-
    /// memory (NVM) as byte code.
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session to act on. </param>
    /// <param name="scriptName">       Name of the script. </param>
    /// <param name="embedByteCode">    True to embed the script as byte code. </param>
    /// <param name="consoleOut">       (Optional) True to console out. </param>
    public static void EmbedScript( this SessionBase session, string scriptName, bool embedByteCode, bool consoleOut = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

        string message = $"checking if the '{scriptName}' script was loaded;. ";
        if ( consoleOut )
            _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        else
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script '{scriptName}' cannot be embedded because it was not loaded." );

        message = $"checking if the '{scriptName}' script exists;. ";
        if ( consoleOut )
            _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        else
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );
        session.LastNodeNumber = default;

        if ( embedByteCode && !session.IsByteCodeScript( scriptName ) )
        {
            message = $"converting '{scriptName}' script to byte code;. ";
            if ( consoleOut )
                _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            else
                _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );
            _ = session.WriteLine( $"{scriptName}.source=nil" );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        }

        message = $"Embedding the '{scriptName}' script;. ";
        if ( consoleOut )
            _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        else
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

        _ = session.WriteLine( $"{scriptName}.save()" );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        if ( !session.IsScriptEmbedded( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} failed to be embedded." );
    }
}
