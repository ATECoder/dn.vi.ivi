namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   Opens a firmware file as a <see cref="System.IO.StreamReader"/>. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="filePath"> Specifies the firmware file path. </param>
    /// <returns>   A reference to an open <see cref="System.IO.StreamReader"/>. </returns>
    public static System.IO.StreamReader? OpenScriptFile( this string filePath )
    {
        return string.IsNullOrWhiteSpace( filePath ) || !System.IO.File.Exists( filePath )
            ? null
            : new System.IO.StreamReader( filePath );
    }

    /// <summary>   Reads the script from the script file and decompresses it if it is compressed. </summary>
    /// <remarks>   2024-07-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="filePath"> Specifies the script file path. </param>
    /// <returns>   The script. </returns>
    public static string ReadScript( this string filePath )
    {
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );
        using StreamReader? reader = SessionBaseExtensionMethods.OpenScriptFile( filePath! );
        string? source = reader?.ReadToEnd();

        if ( source is null || string.IsNullOrWhiteSpace( source ) )
            throw new InvalidOperationException( $"Failed reading script;. file '{filePath}' includes no source." );
        else if ( source.Length < 2 )
            throw new InvalidOperationException( $"Failed reading script;. file '{filePath}' includes no source." );
        else if ( ScriptCompressor.IsCompressed( source ) )
            source = Tsp.Script.ScriptCompressor.Decompress( source );

        return source;
    }

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
}
