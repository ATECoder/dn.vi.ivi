using cc.isr.Std;
using cc.isr.Std.CompressExtensions;
using cc.isr.Std.LineEndingExtensions;

namespace cc.isr.VI.Tsp.Script.ScriptInfoExtensions;

/// <summary>   A script information extensions methods. </summary>
/// <remarks>   2025-09-20. </remarks>
public static partial class ScriptInfoMethods
{
    /// <summary>   A <see cref="ScriptInfo"/>  extension method that compress script. </summary>
    /// <remarks>   2025-09-20. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="scriptInfo">   The scriptInfo to act on. </param>
    /// <param name="source">       specifies the source code for the script. </param>
    /// <param name="scriptFormat"> The script format. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    /// <returns>   A string. </returns>
    public static string CompressScript( this ScriptInfo scriptInfo, string source, FileFormats scriptFormat, bool validate = true )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );
        if ( string.IsNullOrWhiteSpace( source ) ) throw new ArgumentNullException( nameof( source ) );

        // terminate the decompressed source with a single line ending.
        source = source.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        source = source.TerminateLines( validate );

        if ( scriptInfo.Encryptor.IsEncrypted( source ) )
            throw new InvalidOperationException( "The script source is already encrypted" );

        if ( scriptInfo.Compressor.IsCompressed( source ) )
            throw new InvalidOperationException( $"The script source is already compressed" );

        string result = source;
        if ( scriptFormat.HasFlag( FileFormats.Compressed ) )
            result = scriptInfo.Compressor.CompressToBase64( result );

        if ( scriptFormat.HasFlag( FileFormats.Encrypted ) )
            result = scriptInfo.Encryptor.EncryptToBase64( result );

        if ( validate && (scriptFormat.HasFlag( FileFormats.Compressed ) || scriptFormat.HasFlag( FileFormats.Encrypted )) )
        {
            string validationSource = scriptInfo.DecompressScript( result, false );
            if ( source.Length != validationSource.Length )
                throw new InvalidOperationException( $"The script source could not be validated; it's length {source.Length} is not the same as the length of the validation source {validationSource.Length}." );
            if ( !string.Equals( source, validationSource, StringComparison.Ordinal ) )
                throw new InvalidOperationException( $"The script source file could not be validated; it is not equal to the validation source." );
        }
        return result;
    }

    /// <summary>   A <see cref="ScriptInfo"/> extension method that decompress the script. </summary>
    /// <remarks>   2025-09-20. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="scriptInfo">   The scriptInfo to act on. </param>
    /// <param name="source">       specifies the source code for the script. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    /// <returns>   A string. </returns>
    public static string DecompressScript( this ScriptInfo scriptInfo, string source, bool validate = true )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        FileFormats scriptFormat = FileFormats.None;
        if ( source.IsLuaByteCode() )
            scriptFormat |= FileFormats.ByteCode;

        string result = source;
        if ( scriptInfo.Encryptor.IsEncrypted( result ) )
        {
            scriptFormat |= FileFormats.Encrypted;
            result = scriptInfo.Encryptor.DecryptFromBase64( result );
        }

        if ( scriptInfo.Compressor.IsCompressed( result ) )
        {
            scriptFormat |= FileFormats.Compressed;
            result = scriptInfo.Compressor.DecompressFromBase64( result );
        }

        // terminate the decompressed source with a single line ending.
        result = result.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        result = result.TerminateLines( validate );

        if ( validate && (scriptFormat.HasFlag( FileFormats.Compressed ) || scriptFormat.HasFlag( FileFormats.Encrypted )) )
        {
            string validationSource = scriptInfo.DecompressScript( scriptInfo.CompressScript( result, scriptFormat, false ), false );
            if ( result.Length != validationSource.Length )
                throw new InvalidOperationException( $"The processed script source could not be validated; it's length {result.Length} is not the same as the length of the validation source {validationSource.Length}; " );
            if ( !string.Equals( result, validationSource, StringComparison.Ordinal ) )
                throw new InvalidOperationException( "The processed script source could not be validated; it is not equal to the validation source." );
        }

        return result;
    }

    /// <summary>  A <see cref="ScriptInfo"/> extension method that encrypts and compresses script file. </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="scriptInfo">       The scriptInfo to act on. </param>
    /// <param name="inputFilePath">    Full pathname of the input file. </param>
    /// <param name="outputFilePath">   Full pathname of the output file. </param>
    /// <param name="fileFormat">       The file format. </param>
    /// <param name="overwrite">        (Optional) [false] True to over write an existing file. </param>
    /// <param name="validate">         (Optional) True to validate. </param>
    public static void CompressScriptFile( this ScriptInfo scriptInfo, string inputFilePath, string outputFilePath,
        FileFormats fileFormat, bool overwrite = false, bool validate = true )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );
        if ( string.IsNullOrWhiteSpace( inputFilePath ) ) throw new ArgumentNullException( nameof( inputFilePath ) );
        if ( string.IsNullOrWhiteSpace( outputFilePath ) ) throw new ArgumentNullException( nameof( outputFilePath ) );

        if ( !overwrite && System.IO.File.Exists( outputFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{outputFilePath}' exists." );

        inputFilePath.CompressToFile( outputFilePath, fileFormat, scriptInfo.Compressor, scriptInfo.Encryptor, overwrite, validate );
    }

    /// <summary>
    /// A <see cref="ScriptInfo"/> extension method that decrypts and decompresses the script file.
    /// </summary>
    /// <remarks>   2025-05-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="scriptInfo">       The scriptInfo to act on. </param>
    /// <param name="inputFilePath">    Full pathname of the input file. </param>
    /// <param name="outputFilePath">   Full pathname of the output file. </param>
    /// <param name="overwrite">        (Optional) [false] True to over write an existing file. </param>
    /// <param name="validate">         (Optional) True to validate. </param>
    public static void DecompressScriptFile( this ScriptInfo scriptInfo, string inputFilePath, string outputFilePath, bool overwrite = false, bool validate = true )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );
        if ( string.IsNullOrWhiteSpace( inputFilePath ) ) throw new ArgumentNullException( nameof( inputFilePath ) );
        if ( string.IsNullOrWhiteSpace( outputFilePath ) ) throw new ArgumentNullException( nameof( outputFilePath ) );

        inputFilePath.DecompressToFile( outputFilePath, scriptInfo.Compressor, scriptInfo.Encryptor, overwrite, validate );
    }
}
