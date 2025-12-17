using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that any loaded. </summary>
    /// <remarks>
    /// 2025-04-25. <para>
    /// NOTE: This method assumes that the scripts are already filtered for the specific instrument
    /// thus not requiring to match the script to the instrument based on the model mask as was
    /// previously done.
    /// </para>
    /// </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="scripts">  The scripts. </param>
    /// <returns>   True if any script is loaded. </returns>
    public static bool AnyLoaded( this Pith.SessionBase session, ScriptInfoBaseCollection<ScriptInfo> scripts )
    {
        bool affirmative = false;
        foreach ( ScriptInfo script in scripts )
        {
            affirmative = !session.IsNil( script.Title );
            if ( affirmative )
            {
                session.TraceLastAction( $"\r\n\tfound loaded script {script.Title};. " );
                break;
            }
        }
        return affirmative;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that all loaded. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="scripts">  The scripts. </param>
    /// <returns>   True if all scripts loaded; otherwise, false. </returns>
    public static bool AllLoaded( this Pith.SessionBase session, ScriptInfoBaseCollection<ScriptInfo> scripts )
    {
        bool affirmative = true;
        foreach ( ScriptInfo script in scripts )
        {
            affirmative = !session.IsNil( script.Title );
            if ( !affirmative )
            {
                session.TraceLastAction( $"\r\n\tscript {script.Title} is not loaded;. " );
                break;
            }
        }
        return affirmative;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that fetches user scripts names. </summary>
    /// <remarks>   2025-12-16. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    /// <returns>   The user scripts names or empty if no user scripts are loaded. </returns>
    public static string FetchUserScriptsNames( this Pith.SessionBase session, bool consoleOut = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.LastNodeNumber = default;
        string scriptNames;
        string message = "fetching the names of the user scripts";
        if ( consoleOut )
            _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        else
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

        _ = session.WriteLine( "do {0} print( names ) end ", Syntax.Tsp.Script.UserScriptsGetterCommand );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

        scriptNames = session.ReadLineTrimEnd();

        scriptNames = string.Equals( scriptNames, cc.isr.VI.Syntax.Tsp.Lua.NilValue, StringComparison.OrdinalIgnoreCase )
            ? string.Empty
            : scriptNames;

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
    /// A <see cref="Pith.SessionBase"/> extension method that fetches the author user scripts names.
    /// </summary>
    /// <remarks>   2025-12-16. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="prefixFilter"> (Optional) A filter specifying the prefix. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    /// <returns>   The user scripts names filter by the author prefix or empty if no user scripts are loaded. </returns>
    public static string FetchAuthorUserScriptsNames( this Pith.SessionBase session, string prefixFilter = "isr_", bool consoleOut = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        string scriptNames = session.FetchUserScriptsNames( consoleOut );

        scriptNames = string.IsNullOrWhiteSpace( scriptNames )
            ? string.Empty
            : string.Join( ",", scriptNames.FilterScriptNamesByPrefix( prefixFilter ) );

        return scriptNames;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads a script. </summary>
    /// <remarks>   2025-04-20. <para>
    /// Notes:</para><para>
    /// 1. The script must not include the load script command or the end script command. </para><para>
    /// 2. A byte code script must include the <see cref="Syntax.Tsp.Lua.LoadStringCommand"/>. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="scriptLines">              The script lines. </param>
    /// <param name="lineDelay">                The line delay. </param>
    /// <param name="runScriptAfterLoading">    True to run script after loading. </param>
    /// <param name="deleteExisting">           True to delete the existing script before loading the new script. </param>
    public static void LoadScript( this Pith.SessionBase session, string scriptName, string[] scriptLines, TimeSpan lineDelay,
        bool runScriptAfterLoading, bool deleteExisting )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( scriptLines is null ) throw new ArgumentNullException( nameof( scriptLines ) );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;

        if ( deleteExisting && (session.IsScriptEmbedded( scriptName ) || session.IsUserScript( scriptName ) || !session.IsNil( scriptName )) )
            session.DeleteScript( scriptName, true );

        string loadCommand = runScriptAfterLoading
                    ? Syntax.Tsp.Script.LoadAndRunScriptCommand
                    : Syntax.Tsp.Script.LoadScriptCommand;

        string message = runScriptAfterLoading
            ? $"loading and running"
            : $"loading";

        if ( string.IsNullOrWhiteSpace( scriptName ) )
        {
            session.SetLastAction( $"{message} an anonymous script" );
            _ = session.WriteLine( loadCommand );
        }
        else
        {
            session.SetLastAction( $"{message} script {(string.IsNullOrWhiteSpace( scriptName ) ? """anonymous""" : scriptName)} script" );
            _ = session.WriteLine( $"{loadCommand} {scriptName}" );
        }
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.SetLastAction( $"loading lines for {(string.IsNullOrWhiteSpace( scriptName ) ? """anonymous""" : scriptName)} script" );

        foreach ( string line in scriptLines )
        {
            if ( line is not null
                && !string.IsNullOrWhiteSpace( line )
                && !line.Contains( Syntax.Tsp.Script.LoadScriptCommand )
                && !line.Contains( Syntax.Tsp.Script.LoadAndRunScriptCommand )
                && !line.Contains( Syntax.Tsp.Script.EndScriptCommand ) )
            {
                // The Keithley TSP byte code might include a compound command separator,
                // which might explain why TSP does not support SCPI compound commands 
                // consisting of semi-colon-separated commands. The write line method is capable 
                // of splitting compound commands and therefore it cannot be used to send the 
                // byte code to the instrument.
                _ = session.WriteByteCodeLine( line.Trim() );
                if ( lineDelay > TimeSpan.Zero )
                    _ = SessionBase.AsyncDelay( lineDelay );
            }
        }
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.SetLastAction( $"ending {(string.IsNullOrWhiteSpace( scriptName ) ? """anonymous""" : scriptName)} script" );
        _ = session.WriteLine( Syntax.Tsp.Script.EndScriptCommand );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        _ = session.WriteLine( Syntax.Tsp.Lua.WaitCommand );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        if ( !string.IsNullOrWhiteSpace( scriptName ) )
        {
            session.SetLastAction( $"checking if the {scriptName} script exists after load;. " );
            if ( session.IsNil( scriptName ) )
                throw new InvalidOperationException( $"The script {scriptName} was not found after loading the script." );
        }
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads a script. </summary>
    /// <remarks>   2025-04-20. <para>
    /// Notes:</para><para>
    /// 1. The script must not include the load script command or the end script command. </para><para>
    /// 2. A byte code script must include the <see cref="Syntax.Tsp.Lua.LoadStringCommand"/>. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="source">                   specifies the source code for the script. </param>
    /// <param name="lineDelay">                The line delay. </param>
    /// <param name="runScriptAfterLoading">    True to run script after loading. </param>
    /// <param name="deleteExisting">           True to delete the existing script before loading the new script. </param>
    public static void LoadScript( this Pith.SessionBase session, string scriptName, string source, TimeSpan lineDelay,
        bool runScriptAfterLoading, bool deleteExisting )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( string.IsNullOrWhiteSpace( source ) ) throw new ArgumentNullException( nameof( source ) );
        string[] scriptLines = source.Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
        session.LoadScript( scriptName, scriptLines, lineDelay, runScriptAfterLoading, deleteExisting );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads a script. </summary>
    /// <remarks>
    /// 2025-04-20. <para>
    /// Notes:</para><para>
    /// 1. The script must not include the load script command or the end script command. </para><para>
    /// 2. A byte code script must include the <see cref="Syntax.Tsp.Lua.LoadStringCommand"/>. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="source">                   specifies the source code for the script. </param>
    /// <param name="lineDelay">                The line delay. </param>
    /// <param name="runScriptAfterLoading">    True to run script after loading. </param>
    /// <param name="deleteExisting">           True to delete the existing script before loading the
    ///                                         new script. </param>
    /// <param name="loadAsByteCode">           True to load as byte code. </param>
    public static void LoadScript( this Pith.SessionBase session, string scriptName, string source, TimeSpan lineDelay,
        bool runScriptAfterLoading, bool deleteExisting, bool loadAsByteCode )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( string.IsNullOrWhiteSpace( source ) ) throw new ArgumentNullException( nameof( source ) );
        string[] scriptLines = source.Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
        session.LoadScript( scriptName, scriptLines, lineDelay, runScriptAfterLoading, deleteExisting );

        if ( loadAsByteCode && !session.IsByteCodeScript( scriptName ) )
        {
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
            _ = session.WriteLine( $"{scriptName}.source=nil" );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        }
    }

}
