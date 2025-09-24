using cc.isr.Std.LineEndingExtensions;

namespace cc.isr.VI.Tsp.Script.ExportExtensions;

/// <summary>   A script source extension methods. </summary>
/// <remarks>   2025-05-02. </remarks>
public static partial class ExportExtensionsMethods
{
    #region " support methods "

    /// <summary>   Gets temporary path. </summary>
    /// <remarks>   2025-06-03. </remarks>
    /// <param name="subFolderName">    (optional) [RegisterTests] Pathname of the sub folder. </param>
    /// <returns>   The temporary path. </returns>
    public static string GetTempPath( string subFolderName = "ExportExtensions" )
    {
        string tempPath = Path.Combine( Path.GetTempPath(), "~cc.isr", subFolderName );
        _ = System.IO.Directory.CreateDirectory( tempPath );
        return tempPath;
    }

    #endregion

    /// <summary>   Query if 'source' is byte code. </summary>
    /// <remarks>   2024-10-11. </remarks>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <returns>   True if byte code script, false if not. </returns>
    public static bool IsByteCodeScript( this string source )
    {
        return source.Contains( @"\27LuaP\0\4\4\4\", StringComparison.Ordinal );
    }

    /// A <see cref="string"/> extension method that encrypts and compresses a script into a string.
    /// <remarks>   2025-09-22. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="source">       specifies the source code for the script. </param>
    /// <param name="fileFormat">   The file format. </param>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="encryptor">    The encryptor. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    /// <returns>   A string. </returns>
    public static string CompressScript( this string source, ScriptFormats fileFormat, IScriptCompressor compressor,
        IScriptEncryptor encryptor, bool validate = true )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( string.IsNullOrWhiteSpace( source ) ) throw new ArgumentNullException( nameof( source ) );

        // terminate the contents with a single line ending.
        source = source.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        source = source.TerminateLines( validate );

        if ( encryptor.IsEncrypted( source ) )
            throw new InvalidOperationException( "The script source is already encrypted" );

        if ( compressor.IsCompressed( source ) )
            throw new InvalidOperationException( "The script source is already compressed" );

        string compressed = source;
        if ( fileFormat.HasFlag( ScriptFormats.Compressed ) )
            compressed = compressor.CompressToBase64( compressed );

        if ( fileFormat.HasFlag( ScriptFormats.Encrypted ) )
            compressed = encryptor.EncryptToBase64( compressed );

        if ( validate && (fileFormat.HasFlag( ScriptFormats.Compressed ) || fileFormat.HasFlag( ScriptFormats.Encrypted )) )
        {
            string decompressed = compressed;
            if ( fileFormat.HasFlag( ScriptFormats.Encrypted ) )
                decompressed = encryptor.DecryptFromBase64( decompressed );
            if ( fileFormat.HasFlag( ScriptFormats.Compressed ) )
                decompressed = compressor.DecompressFromBase64( decompressed );

            if ( source.Length != decompressed.Length )
                throw new InvalidOperationException( $"The compressed script source could not be validated; the de-compressed length {decompressed.Length} does not match the end-of-line trimmed source length {source.Length}." );

            if ( !source.Equals( decompressed, StringComparison.Ordinal ) )
                throw new InvalidOperationException( $"The compressed script source could not be validated; the de-compressed content does not match the original source." );
        }
        return compressed;
    }

    /// <summary>
    /// A <see cref="string"/> extension method that encrypts and compresses a script file into a string.
    /// </summary>
    /// <remarks>   2025-09-22. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="inputFilePath">    The full path name of the input file to act on. </param>
    /// <param name="fileFormat">       The file format. </param>
    /// <param name="compressor">       The compressor. </param>
    /// <param name="encryptor">        The encryptor. </param>
    /// <param name="validate">         (Optional) True to validate. </param>
    /// <returns>   A string. </returns>
    public static string CompressScriptFile( this string inputFilePath, ScriptFormats fileFormat, IScriptCompressor compressor,
        IScriptEncryptor encryptor, bool validate = true )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( string.IsNullOrWhiteSpace( inputFilePath ) ) throw new ArgumentNullException( nameof( inputFilePath ) );

        // read the contents from the file.
        string contents = System.IO.File.ReadAllText( inputFilePath );

        // terminate the contents with a single line ending.
        contents = contents.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        contents = contents.TerminateLines( validate );

        if ( encryptor.IsEncrypted( contents ) )
            throw new InvalidOperationException( $"The input file {inputFilePath} is already encrypted" );

        if ( compressor.IsCompressed( contents ) )
            throw new InvalidOperationException( $"The input file {inputFilePath} is already compressed" );

        try
        {
            return contents.CompressScript( fileFormat, compressor, encryptor, validate );
        }
        catch ( Exception ex )
        {
            throw new InvalidOperationException( $"The compressed script source file '{inputFilePath}' could not be validated; {ex.Message}." );
        }
    }

    /// <summary>
    /// A <see cref="string"/> extension method that encrypts and compresses a script file into
    /// another.
    /// </summary>
    /// <remarks>
    /// 2025-09-19. <para>
    /// Note that the source file is pre-processed to ensure that line endings are Windows style.
    /// Therefore, the compression cannot be validated simply by comparing a decompressed compressed
    /// file with the original plain text file.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="inputFilePath">    The full path name of the input file to act on. </param>
    /// <param name="outputFilePath">   The full pathname of the output file. </param>
    /// <param name="fileFormat">       The file format. </param>
    /// <param name="compressor">       The compressor. </param>
    /// <param name="encryptor">        The encryptor. </param>
    /// <param name="overWrite">        (Optional) [false] True to over write. </param>
    /// <param name="validate">         (Optional) True to validate. </param>
    public static void CompressScriptFile( this string inputFilePath, string outputFilePath, ScriptFormats fileFormat, IScriptCompressor compressor,
        IScriptEncryptor encryptor, bool overWrite = false, bool validate = true )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( string.IsNullOrWhiteSpace( inputFilePath ) ) throw new ArgumentNullException( nameof( inputFilePath ) );
        if ( string.IsNullOrWhiteSpace( outputFilePath ) ) throw new ArgumentNullException( nameof( outputFilePath ) );

        if ( !overWrite && System.IO.File.Exists( outputFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{outputFilePath}' exists." );

        // get the compressed string
        string compressed = inputFilePath.CompressScriptFile( fileFormat, compressor, encryptor, validate );

        // export to file
        System.IO.File.WriteAllText( outputFilePath, compressed, System.Text.Encoding.Default );
    }

    /// <summary>
    /// A <see cref="string"/> extension method that decrypts and decompresses the compressed input string.
    /// </summary>
    /// <remarks>   2025-09-22. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="compressed">   The compressed contents to act on. </param>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="encryptor">    The encryptor. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    /// <returns>   A string. </returns>
    public static string DecompressScript( this string compressed, IScriptCompressor compressor, IScriptEncryptor encryptor, bool validate = true )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( string.IsNullOrWhiteSpace( compressed ) ) throw new ArgumentNullException( nameof( compressed ) );

        ScriptFormats fileFormat = ScriptFormats.None;
        if ( compressed.IsByteCodeScript() )
            fileFormat |= ScriptFormats.ByteCode;

        string source = compressed;
        if ( encryptor.IsEncrypted( source ) )
        {
            fileFormat |= ScriptFormats.Encrypted;
            source = encryptor.DecryptFromBase64( source );
        }

        if ( compressor.IsCompressed( source ) )
        {
            fileFormat |= ScriptFormats.Compressed;
            source = compressor.DecompressFromBase64( source );
        }

        if ( validate && (fileFormat.HasFlag( ScriptFormats.Compressed ) || fileFormat.HasFlag( ScriptFormats.Encrypted )) )
        {
            string recompressed = source;
            if ( fileFormat.HasFlag( ScriptFormats.Compressed ) )
                recompressed = compressor.CompressToBase64( recompressed );

            if ( fileFormat.HasFlag( ScriptFormats.Encrypted ) )
                recompressed = encryptor.EncryptToBase64( recompressed );

            if ( compressed.Length != recompressed.Length )
                throw new InvalidOperationException( $"The compressed script source could not be validated; the re-compressed length {recompressed.Length} does not match the original length {compressed.Length}." );

            if ( !compressed.Equals( recompressed, StringComparison.Ordinal ) )
                throw new InvalidOperationException( $"The compressed script source could not be validated; the re-compressed content does not match the original." );
        }

        // terminate the decompressed contents with a single line ending.
        source = source.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        return source.TerminateLines( validate );
    }


    /// <summary>   A <see cref="string"/> extension method that decrypts and decompresses the script file. </summary>
    /// <remarks>   2025-09-22. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="inputFilePath">    The full path name of the input file to act on. </param>
    /// <param name="compressor">       The compressor. </param>
    /// <param name="encryptor">        The encryptor. </param>
    /// <param name="validate">         (Optional) True to validate. </param>
    /// <returns>   A string. </returns>
    public static string DecompressScriptFile( this string inputFilePath, IScriptCompressor compressor, IScriptEncryptor encryptor, bool validate = true )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( string.IsNullOrWhiteSpace( inputFilePath ) ) throw new ArgumentNullException( nameof( inputFilePath ) );

        // read the compressed contents from the file.
        string contents = System.IO.File.ReadAllText( inputFilePath );

        try
        {
            return contents.DecompressScript( compressor, encryptor, validate );
        }
        catch ( Exception ex )
        {
            throw new InvalidOperationException( $"The compressed script source file '{inputFilePath}' could not be validated; {ex.Message}." );
            throw;
        }
    }


    /// <summary>   A <see cref="string"/> extension method that decrypts and decompresses the script file. </summary>
    /// <remarks>   2025-05-01. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="inputFilePath">    The full path name of the input file to act on. </param>
    /// <param name="outputFilePath">   The full pathname of the output file. </param>
    /// <param name="compressor">       The compressor. </param>
    /// <param name="encryptor">        The encryptor. </param>
    /// <param name="overWrite">        (Optional) [false] True to over write. </param>
    /// <param name="validate">         (Optional) True to validate. </param>
    public static void DecompressScriptFile( this string inputFilePath, string outputFilePath, IScriptCompressor compressor, IScriptEncryptor encryptor,
        bool overWrite = false, bool validate = true )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( string.IsNullOrWhiteSpace( inputFilePath ) ) throw new ArgumentNullException( nameof( inputFilePath ) );
        if ( string.IsNullOrWhiteSpace( outputFilePath ) ) throw new ArgumentNullException( nameof( outputFilePath ) );

        if ( !overWrite && System.IO.File.Exists( outputFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{outputFilePath}' exists." );

        // get the decompressed string
        string source = inputFilePath.DecompressScriptFile( compressor, encryptor, validate );

        // export to file
        System.IO.File.WriteAllText( outputFilePath, source, System.Text.Encoding.Default );
    }

    /// <summary>   A <see cref="string"/> extension method that file equals to. </summary>
    /// <remarks>   2025-09-22. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="filePath1">    The filePath1 to act on. </param>
    /// <param name="filePath2">    The second file path. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool FileEqualsTo( this string filePath1, string filePath2, out string details )
    {
        if ( string.IsNullOrWhiteSpace( filePath1 ) ) throw new ArgumentNullException( nameof( filePath1 ) );
        if ( string.IsNullOrWhiteSpace( filePath2 ) ) throw new ArgumentNullException( nameof( filePath2 ) );
        FileInfo filePath1Info = new( filePath1 );
        FileInfo filePath2Info = new( filePath2 );
        if ( filePath1Info.Length != filePath2Info.Length )
        {
            details = $"The '{filePath1}' length {filePath1Info.Length} is not the same as the length of the '{filePath2}' ({filePath2Info.Length}).";
            return false;
        }
        if ( File.ReadAllBytes( filePath1 ).SequenceEqual( File.ReadAllBytes( filePath2 ) ) )
        {
            details = $"'{filePath1}' equals {filePath2}";
            return true;
        }
        else
        {
            details = $"The '{filePath1}' file is not equal to the '{filePath2}'.";
            return false;
        }
    }

    /// <summary>
    /// A <see cref="string"/> extension method to compare files ignoring any difference in the file
    /// ending because compression removes new lines from the end of the file.
    /// </summary>
    /// <remarks>   2025-09-24. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="filePath1">    The filePath1 to act on. </param>
    /// <param name="filePath2">    The second file path. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool CompareFilesIgnoreEnding( this string filePath1, string filePath2, out string details )
    {
        if ( string.IsNullOrWhiteSpace( filePath1 ) ) throw new ArgumentNullException( nameof( filePath1 ) );
        if ( string.IsNullOrWhiteSpace( filePath2 ) ) throw new ArgumentNullException( nameof( filePath2 ) );
        if ( !File.Exists( filePath1 ) )
        {
            details = $"The '{filePath1}' file does not exist.";
            return false;
        }
        if ( !File.Exists( filePath2 ) )
        {
            details = $"The '{filePath2}' file does not exist.";
            return false;
        }

        string contents1 = File.ReadAllText( filePath1 ).TrimEnd( Environment.NewLine.ToCharArray() );
        string contents2 = File.ReadAllText( filePath1 ).TrimEnd( Environment.NewLine.ToCharArray() );

        if ( contents1.Equals( contents2, StringComparison.Ordinal ) )
        {
            details = $"'{filePath1}' equals {filePath2}";
            return true;
        }
        else
        {
            details = $"The '{filePath1}' file is not equal to the '{filePath2}'.";
            return false;
        }
    }

    /// <summary>
    /// A <see cref="string"/> extension method that compressed files ignoring any differences in the
    /// file endings because the compression process removed excess new lines from the end of the file.
    /// </summary>
    /// <remarks>   2025-09-22. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="filePath1">    The flePath1 to act on. </param>
    /// <param name="filePath2">    The second file path. </param>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="encryptor">    The encryptor. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool CompressedFileEqualsIgnoreEnding( this string filePath1, string filePath2, IScriptCompressor compressor,
        IScriptEncryptor encryptor, out string details )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( string.IsNullOrWhiteSpace( filePath1 ) ) throw new ArgumentNullException( nameof( filePath1 ) );
        if ( string.IsNullOrWhiteSpace( filePath2 ) ) throw new ArgumentNullException( nameof( filePath2 ) );

        string outPath = ExportExtensionsMethods.GetTempPath();

        string decompressedFile1 = Path.Combine( outPath, Path.GetFileName( filePath1 ) );
        string decompressedFile2 = Path.Combine( outPath, Path.GetFileName( filePath2 ) );
        filePath1.DecompressScriptFile( decompressedFile1, compressor, encryptor, true, false );
        filePath2.DecompressScriptFile( decompressedFile2, compressor, encryptor, true, false );
        if ( decompressedFile1.CompareFilesIgnoreEnding( decompressedFile2, out details ) )
            return true;
        else
        {
            details = $"The contents of the compressed files are not equals; {details}";
            return false;
        }
    }
}
