using cc.isr.Std;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseMethods
{

    /// <summary>
    /// Reads the script from the script file and decrypt and decompresses as necessary.
    /// </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="filePath"> Specifies the script file path. </param>
    /// <returns>   The script. </returns>
    public static string ReadScript( this string filePath )
    {
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        using StreamReader? reader = new System.IO.StreamReader( filePath ) ??
            throw new FileNotFoundException( $"Failed opening script file '{filePath}'" );

        string source = System.IO.File.Exists( filePath )
            ? System.IO.File.ReadAllText( filePath )
            : throw new FileNotFoundException( $"Script file '{filePath}' not found;. " );

        if ( source is null || string.IsNullOrWhiteSpace( source ) )
            throw new InvalidOperationException( $"Failed reading script;. file '{filePath}' includes no source;. " );
        else if ( source.Length < 2 )
            throw new InvalidOperationException( $"Failed reading script;. file '{filePath}' includes no source;. " );

        return source;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads script file. </summary>
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
    /// <param name="filePath">                 Full pathname of the file. </param>
    /// <param name="lineDelay">                The line delay. </param>
    /// <param name="runScriptAfterLoading">    True to run script after loading. </param>
    /// <param name="deleteExisting">           True to delete the existing existing script before loading the script. </param>
    public static void ImportScript( this Pith.SessionBase session, string scriptName, string filePath, TimeSpan lineDelay,
        bool runScriptAfterLoading, bool deleteExisting )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( filePath is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( filePath ) );

        string scriptNameOrAnonymous = string.IsNullOrWhiteSpace( scriptName ) ? "anonymous" : scriptName;

        // read the script source from file.
        string? scriptSource = filePath.ReadScript();
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new ArgumentNullException( $"Failed reading {scriptNameOrAnonymous} script source from {filePath}" );

        // This reads the entire source from the file and then loads the file line by line as source code or byte code
        session.LoadScript( scriptName, scriptSource, lineDelay, runScriptAfterLoading, deleteExisting );
    }

    /// <summary>
    /// Reads the script from the script file and decrypt and decompresses as necessary.
    /// </summary>
    /// <remarks>   2024-07-09. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="filePath">     Specifies the script file path. </param>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="encryptor">    The encryptor. </param>
    /// <returns>   The script. </returns>
    public static string ReadScript( this string filePath, IStringCompressor compressor, IStringEncryptor encryptor )
    {
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        using StreamReader? reader = new System.IO.StreamReader( filePath ) ??
            throw new FileNotFoundException( $"Failed opening script file '{filePath}'" );

        string source = SessionBaseMethods.ReadScript( filePath );

        if ( source is null || string.IsNullOrWhiteSpace( source ) )
            throw new InvalidOperationException( $"Failed reading script;. file '{filePath}' includes no source;. " );
        else if ( source.Length < 2 )
            throw new InvalidOperationException( $"Failed reading script;. file '{filePath}' includes no source;. " );

        if ( encryptor.IsEncrypted( source ) )
            source = encryptor.DecryptFromBase64( source );
        if ( compressor.IsCompressed( source ) )
            source = compressor.DecompressFromBase64( source );
        return source;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads script file. </summary>
    /// <remarks>
    /// 2025-04-20. <para>
    /// Notes:</para><para>
    /// 1. The script must not include the load script command or the end script command. </para><para>
    /// 2. A byte code script must include the <see cref="Syntax.Tsp.Lua.LoadStringCommand"/>. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="compressor">               The compressor. </param>
    /// <param name="encryptor">                The encryptor. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="filePath">                 Full pathname of the file. </param>
    /// <param name="lineDelay">                The line delay. </param>
    /// <param name="runScriptAfterLoading">    True to run script after loading. </param>
    /// <param name="deleteExisting">           True to delete the existing script prior to loading the new script. </param>
    public static void ImportScript( this Pith.SessionBase session, IStringCompressor compressor, IStringEncryptor encryptor,
        string scriptName, string filePath, TimeSpan lineDelay, bool runScriptAfterLoading, bool deleteExisting )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( filePath is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( filePath ) );

        string scriptNameOrAnonymous = string.IsNullOrWhiteSpace( scriptName ) ? "anonymous" : scriptName;

        // read the script source from file.
        string? scriptSource = filePath.ReadScript( compressor, encryptor );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new ArgumentNullException( $"Failed reading {scriptNameOrAnonymous} script source from {filePath}" );

        // This reads the entire source from the file and then loads the file line by line as source code or byte code
        session.LoadScript( scriptName, scriptSource, lineDelay, runScriptAfterLoading, deleteExisting );
    }
}
