using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads a script. </summary>
    /// <remarks>   2025-04-20. <para>
    /// Notes:</para><para>
    /// 1. The script must not include the load script command or the end script command. </para><para>
    /// 2. A binary script must include the <see cref="Syntax.Tsp.Lua.LoadStringCommand"/>. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="source">                   specifies the source code for the script. </param>
    /// <param name="lineDelay">                The line delay. </param>
    /// <param name="runScriptAfterLoading">    (Optional) [false] True to run script after loading. </param>
    /// <param name="deleteExisting">           (Optional) [false] True to delete the existing. </param>
    /// <param name="ignoreExisting">           (Optional) [false] True to ignore existing. </param>
    public static void LoadScript( this Pith.SessionBase? session, string scriptName, string source, TimeSpan lineDelay,
        bool runScriptAfterLoading = false, bool deleteExisting = false, bool ignoreExisting = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( string.IsNullOrWhiteSpace( source ) ) throw new ArgumentNullException( nameof( source ) );
        string[] scriptLines = source.Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
        session.LoadScript( scriptName, scriptLines, lineDelay, runScriptAfterLoading, deleteExisting, ignoreExisting );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads a script. </summary>
    /// <remarks>   2025-04-20. <para>
    /// Notes:</para><para>
    /// 1. The script must not include the load script command or the end script command. </para><para>
    /// 2. A binary script must include the <see cref="Syntax.Tsp.Lua.LoadStringCommand"/>. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="scriptLines">              The script lines. </param>
    /// <param name="lineDelay">                The line delay. </param>
    /// <param name="runScriptAfterLoading">    (Optional) [false] True to run script after loading. </param>
    /// <param name="deleteExisting">           (Optional) [false] True to delete the existing. </param>
    /// <param name="ignoreExisting">           (Optional) [false] True to ignore existing. </param>
    public static void LoadScript( this Pith.SessionBase? session, string scriptName, string[] scriptLines, TimeSpan lineDelay,
        bool runScriptAfterLoading = false, bool deleteExisting = false, bool ignoreExisting = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( scriptLines is null ) throw new ArgumentNullException( nameof( scriptLines ) );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;

        bool scriptExists = !session.IsNil( scriptName );
        if ( scriptExists && deleteExisting )
            session.DeleteScript( scriptName );
        else if ( scriptExists && !ignoreExisting )
            throw new InvalidOperationException( $"The script {scriptName} cannot be imported over an existing script." );

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
                _ = session.WriteNonCompoundLine( line.Trim() );
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
            scriptExists = !session.IsNil( scriptName );
            if ( !scriptExists )
                throw new InvalidOperationException( $"The script {scriptName} was not found after loading the script." );
        }
    }
}
