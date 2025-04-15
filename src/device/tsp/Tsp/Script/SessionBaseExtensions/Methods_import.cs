using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

/// <summary>   A session base methods. </summary>
/// <remarks>   2025-04-10. </remarks>
public static partial class SessionBaseExtensionMethods
{
    /// <summary>   A <see cref="string"/> extension method that import script. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="fromFilePath">     The file path. </param>
    /// <param name="toFilePath">       True to delete an existing script if it exists. </param>
    /// <param name="retainOutline">    True to retain outline. </param>
    public static void TrimScript( this string fromFilePath, string toFilePath, bool retainOutline )
    {
        if ( fromFilePath is null || string.IsNullOrWhiteSpace( fromFilePath ) ) throw new ArgumentNullException( nameof( fromFilePath ) );
        if ( toFilePath is null || string.IsNullOrWhiteSpace( toFilePath ) ) throw new ArgumentNullException( nameof( toFilePath ) );
        cc.isr.VI.Syntax.Tsp.TspScriptParser.TrimTspSourceCode( fromFilePath, toFilePath, retainOutline );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that import script. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="filePath">                 The file path. </param>
    /// <param name="runScriptAfterLoading">    (Optional) [false] True to run script after loading. </param>
    /// <param name="deleteExisting">           (Optional) [false] True to delete an existing script
    ///                                         if it exists. </param>
    /// <param name="ignoreExisting">           (Optional) [false] True to ignore existing script. </param>
    public static void ImportScript( this SessionBase session, string? scriptName, string? filePath, bool runScriptAfterLoading = false, bool deleteExisting = false, bool ignoreExisting = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );

        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( filePath is null || string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        TextReader? reader1;

        // check if the script is compressed.
        if ( ScriptCompressor.IsCompressedFile( filePath ) )
        {
            // decompress the script.
            reader1 = new StringReader( ScriptCompressor.Decompress( System.IO.File.ReadAllText( filePath ) ) );
        }
        else
        {
            reader1 = (string.IsNullOrWhiteSpace( filePath ) || !System.IO.File.Exists( filePath )
                       ? null
                       : new System.IO.StreamReader( filePath )) ?? throw new System.IO.FileNotFoundException( "Failed opening script file", filePath );
        }
        using TextReader? reader = reader1;

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        bool scriptExists = !session.IsNil( scriptName );
        if ( scriptExists && deleteExisting )
            session.DeleteScript( scriptName );
        else if ( scriptExists && !ignoreExisting )
            throw new InvalidOperationException( $"The script {scriptName} cannot be imported over an existing script." );

        // use the text reader to load the script.
        session.LoadScript( reader, scriptName, runScriptAfterLoading, deleteExisting, ignoreExisting );

        reader1?.Dispose();
    }

}
