using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class Methods
{
    /// <summary>   Query if 'source' is a compressed source. </summary>
    /// <remarks>   2024-10-11. </remarks>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <returns>   True if compressed source, false if not. </returns>
    public static bool IsCompressedSource( this string source )
    {
        return ScriptCompressor.IsCompressed( source );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that query if the specified script is loaded. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. Empty for an anonymous script. </param>
    /// <returns>   True if loaded, false if not. </returns>
    public static bool IsLoaded( this Pith.SessionBase session, string scriptName )
    {
        return !session.IsNil( scriptName );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads a script. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="reader">                   The reader. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="runScriptAfterLoading">    (Optional) [false] True to run the script after
    ///                                         loading using the <see cref="Syntax.Tsp.Script.LoadAndRunScriptCommand"/>.</param>
    /// <param name="deleteExisting">           (Optional) [false] True to delete an existing script
    ///                                         if it exists. </param>
    /// <param name="ignoreExisting">           (Optional) [false] True to ignore existing script. </param>
    public static void LoadScript( this SessionBase session, TextReader reader, string scriptName,
        bool runScriptAfterLoading = false, bool deleteExisting = false, bool ignoreExisting = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

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
            session.SetLastAction( $"{message} an anonymous script from {reader}" );
            _ = session.WriteLine( loadCommand );
        }
        else
        {
            session.SetLastAction( $"{message} script '{scriptName}' from {reader}" );
            _ = session.WriteLine( $"{loadCommand} {scriptName}" );
        }
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        string? line = "";
        while ( line is not null )
        {
            line = reader.ReadLine();
            if ( line is not null
                && !string.IsNullOrWhiteSpace( line )
                && !line.Contains( Syntax.Tsp.Script.LoadScriptCommand )
                && !line.Contains( Syntax.Tsp.Script.LoadAndRunScriptCommand )
                && !line.Contains( Syntax.Tsp.Script.EndScriptCommand ) )
                _ = session.WriteLine( line );
        }
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.SetLastAction( $"ending script '{scriptName}' from {reader}" );
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

        session.SetLastAction( $"checking if the {scriptName} script exists after load;. " );
        scriptExists = !session.IsNil( scriptName );
        if ( !scriptExists )
            throw new InvalidOperationException( $"The script {scriptName} was not found after loading the script {reader}." );

    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads a script. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="session">                  The reference to the K2600 Device. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="scriptSource">             The script source. </param>
    /// <param name="runScriptAfterLoading">    (Optional) [false] True to run the script after
    ///                                         loading using the <see cref="Syntax.Tsp.Script.LoadAndRunScriptCommand"/>.</param>
    /// <param name="deleteExisting">           (Optional) True to delete the existing. </param>
    /// <param name="ignoreExisting">           (Optional) True to ignore existing. </param>
    public static void LoadScript( this SessionBase session, string scriptName, string scriptSource, bool runScriptAfterLoading = false, bool deleteExisting = false, bool ignoreExisting = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new ArgumentNullException( nameof( scriptSource ) );

        // decompress the script if compressed.
        if ( ScriptCompressor.IsCompressed( scriptSource ) )
            scriptSource = ScriptCompressor.Decompress( scriptSource )
                ?? throw new InvalidOperationException( $"Failed decompressing the script {scriptName}." );

        using TextReader? reader = new System.IO.StringReader( scriptSource )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader from the {scriptName} script source" );

        session.LoadScript( reader, scriptName, runScriptAfterLoading, deleteExisting, ignoreExisting );
    }
}
