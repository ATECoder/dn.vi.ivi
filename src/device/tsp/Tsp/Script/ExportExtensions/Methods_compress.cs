using cc.isr.Std.LineEndingExtensions;

namespace cc.isr.VI.Tsp.Script.ExportExtensions;

/// <summary>   A script source extension methods. </summary>
/// <remarks>   2025-05-02. </remarks>
public static partial class ExportExtensionsMethods
{
    /// <summary>   Query if 'source' is byte code. </summary>
    /// <remarks>   2024-10-11. </remarks>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <returns>   True if byte code script, false if not. </returns>
    public static bool IsByteCodeScript( this string source )
    {
        return source.Contains( @"\27LuaP\0\4\4\4\", StringComparison.Ordinal );
    }

    /// <summary>   A string extension method that decrypts and decompresses the script file. </summary>
    /// <remarks>   2025-05-01. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="fromFilePath"> The source file path. </param>
    /// <param name="toFilePath">   the destination file path. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void DecompressScriptFile( this string fromFilePath, string toFilePath, IScriptCompressor compressor, IScriptEncryptor encryptor,
        bool overWrite = false, bool validate = true )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( string.IsNullOrWhiteSpace( fromFilePath ) ) throw new ArgumentNullException( nameof( fromFilePath ) );
        if ( string.IsNullOrWhiteSpace( toFilePath ) ) throw new ArgumentNullException( nameof( toFilePath ) );

        if ( !overWrite && System.IO.File.Exists( toFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{toFilePath}' exists." );

        // read the compressed contents from the file.
        string contents = System.IO.File.ReadAllText( fromFilePath );

        ScriptFileFormats fileFormat = ScriptFileFormats.None;
        if ( contents.IsByteCodeScript() )
            fileFormat |= ScriptFileFormats.ByteCode;

        if ( encryptor.IsEncrypted( contents ) )
        {
            fileFormat |= ScriptFileFormats.Encrypted;
            contents = encryptor.DecryptFromBase64( contents );
        }

        if ( compressor.IsCompressed( contents ) )
        {
            fileFormat |= ScriptFileFormats.Compressed;
            contents = compressor.DecompressFromBase64( contents );
        }

        // terminate the decompressed contents with a single line ending.
        contents = contents.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        contents = contents.TerminateLines( validate );

        // export to file
        System.IO.File.WriteAllText( toFilePath, contents, System.Text.Encoding.Default );

        if ( validate )
        {
            string subFolderName = "~isr";
            string tempPath = Path.Combine( Path.GetTempPath(), subFolderName );
            if ( !System.IO.Directory.Exists( tempPath ) )
                _ = System.IO.Directory.CreateDirectory( tempPath );
            string tempFilePath = Path.Combine( tempPath, Path.GetFileName( toFilePath ) );
            toFilePath.CompressScriptFile( tempFilePath, fileFormat, compressor, encryptor, true, false );
            FileInfo fromFilePathInfo = new( fromFilePath );
            FileInfo tempFilePathInfo = new( tempFilePath );
            if ( fromFilePathInfo.Length != tempFilePathInfo.Length )
                throw new InvalidOperationException( $"The processed script source file '{fromFilePath}' could not be validated; It length {fromFilePathInfo.Length} is not the same as the length of the validation file {tempFilePathInfo.Length}; " );
            if ( !File.ReadAllBytes( fromFilePath ).SequenceEqual( File.ReadAllBytes( tempFilePath ) ) )
                throw new InvalidOperationException( $"The processed script source file '{fromFilePath}' could not be validated; is not equal to the validation file {tempFilePathInfo.Length}; " );
        }
    }

    /// <summary>   A string extension method that encrypts and compresses script file. </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="fromFilePath"> The source file path. </param>
    /// <param name="toFilePath">   the destination file path. </param>
    /// <param name="fileFormat">   The file format. </param>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="encryptor">    The encryptor. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void CompressScriptFile( this string fromFilePath, string toFilePath, ScriptFileFormats fileFormat, IScriptCompressor compressor,
        IScriptEncryptor encryptor, bool overWrite = false, bool validate = true )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( string.IsNullOrWhiteSpace( fromFilePath ) ) throw new ArgumentNullException( nameof( fromFilePath ) );
        if ( string.IsNullOrWhiteSpace( toFilePath ) ) throw new ArgumentNullException( nameof( toFilePath ) );

        if ( !overWrite && System.IO.File.Exists( toFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{toFilePath}' exists." );

        // read the compressed contents from the file.
        string contents = System.IO.File.ReadAllText( fromFilePath );

        // terminate the decompressed contents with a single line ending.
        contents = contents.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        contents = contents.TerminateLines( validate );

        if ( encryptor.IsEncrypted( contents ) )
            throw new InvalidOperationException( $"The input file {fromFilePath} is already encrypted" );

        if ( compressor.IsCompressed( contents ) )
            throw new InvalidOperationException( $"The input file {fromFilePath} is already compressed" );

        if ( fileFormat.HasFlag( ScriptFileFormats.Compressed ) )
            contents = compressor.CompressToBase64( contents );

        if ( fileFormat.HasFlag( ScriptFileFormats.Encrypted ) )
            contents = encryptor.EncryptToBase64( contents );

        // export to file
        System.IO.File.WriteAllText( toFilePath, contents, System.Text.Encoding.Default );

        if ( validate )
        {
            string subFolderName = "~isr";
            string tempPath = Path.Combine( Path.GetTempPath(), subFolderName );
            if ( !System.IO.Directory.Exists( tempPath ) )
                _ = System.IO.Directory.CreateDirectory( tempPath );
            string tempFilePath = Path.Combine( tempPath, Path.GetFileName( toFilePath ) );
            toFilePath.DecompressScriptFile( tempFilePath, compressor, encryptor, true, false );
            FileInfo fromFilePathInfo = new( fromFilePath );
            FileInfo tempFilePathInfo = new( tempFilePath );
            if ( fromFilePathInfo.Length != tempFilePathInfo.Length )
                throw new InvalidOperationException( $"The script source file '{fromFilePath}' could not be validated; It length {fromFilePathInfo.Length} is not the same as the length of the validation file {tempFilePathInfo.Length}; " );
            if ( !File.ReadAllBytes( fromFilePath ).SequenceEqual( File.ReadAllBytes( tempFilePath ) ) )
                throw new InvalidOperationException( $"The script source file '{fromFilePath}' could not be validated; is not equal to the validation file {tempFilePathInfo.Length}; " );
        }

    }

}
