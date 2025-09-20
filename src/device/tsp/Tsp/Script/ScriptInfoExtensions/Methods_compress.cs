using cc.isr.Std.LineEndingExtensions;
using cc.isr.VI.Tsp.Script.ExportExtensions;

namespace cc.isr.VI.Tsp.Script.ScriptInfoExtensions;

/// <summary>   A script information extensions methods. </summary>
/// <remarks>   2025-09-20. </remarks>
public static partial class ScriptInfoExtensionsMethods
{
    /// <summary>   Query if 'source' is byte code. </summary>
    /// <remarks>   2024-10-11. </remarks>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <returns>   True if byte code script, false if not. </returns>
    public static bool IsByteCodeScript( this string source )
    {
        return source.Contains( @"\27LuaP\0\4\4\4\", StringComparison.Ordinal );
    }

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
    public static string CompressScript( this ScriptInfo scriptInfo, string source, ScriptFormats scriptFormat, bool validate = true )
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
        if ( scriptFormat.HasFlag( ScriptFormats.Compressed ) )
            result = scriptInfo.Compressor.CompressToBase64( result );

        if ( scriptFormat.HasFlag( ScriptFormats.Encrypted ) )
            result = scriptInfo.Encryptor.EncryptToBase64( result );

        if ( validate && (scriptFormat.HasFlag( ScriptFormats.Compressed ) || scriptFormat.HasFlag( ScriptFormats.Encrypted )) )
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

        ScriptFormats scriptFormat = ScriptFormats.None;
        if ( source.IsByteCodeScript() )
            scriptFormat |= ScriptFormats.ByteCode;

        string result = source;
        if ( scriptInfo.Encryptor.IsEncrypted( result ) )
        {
            scriptFormat |= ScriptFormats.Encrypted;
            result = scriptInfo.Encryptor.DecryptFromBase64( result );
        }

        if ( scriptInfo.Compressor.IsCompressed( result ) )
        {
            scriptFormat |= ScriptFormats.Compressed;
            result = scriptInfo.Compressor.DecompressFromBase64( result );
        }

        // terminate the decompressed source with a single line ending.
        result = result.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        result = result.TerminateLines( validate );

        if ( validate && (scriptFormat.HasFlag( ScriptFormats.Compressed ) || scriptFormat.HasFlag( ScriptFormats.Encrypted )) )
        {
            string validationSource = scriptInfo.CompressScript( result, scriptFormat, false );
            if ( source.Length != validationSource.Length )
                throw new InvalidOperationException( $"The processed script source could not be validated; it's length {source.Length} is not the same as the length of the validation source {validationSource.Length}; " );
            if ( !string.Equals( source, validationSource, StringComparison.Ordinal ) )
                throw new InvalidOperationException( "The processed script source could not be validated; it is not equal to the validation source." );
        }

        return result;
    }

    /// <summary>   A string extension method that encrypts and compresses script file. </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="scriptInfo">   The scriptInfo to act on. </param>
    /// <param name="fromFilePath"> The source file path. </param>
    /// <param name="toFilePath">   the destination file path. </param>
    /// <param name="fileFormat">   The file format. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void CompressScriptFile( this ScriptInfo scriptInfo, string fromFilePath, string toFilePath, ScriptFormats fileFormat, bool overWrite = false, bool validate = true )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );
        if ( string.IsNullOrWhiteSpace( fromFilePath ) ) throw new ArgumentNullException( nameof( fromFilePath ) );
        if ( string.IsNullOrWhiteSpace( toFilePath ) ) throw new ArgumentNullException( nameof( toFilePath ) );

        if ( !overWrite && System.IO.File.Exists( toFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{toFilePath}' exists." );

        fromFilePath.CompressScriptFile( toFilePath, fileFormat, scriptInfo.Compressor, scriptInfo.Encryptor, overWrite, validate );
    }

    /// <summary>   A string extension method that decrypts and decompresses the script file. </summary>
    /// <remarks>   2025-05-01. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="scriptInfo">   The scriptInfo to act on. </param>
    /// <param name="fromFilePath"> The source file path. </param>
    /// <param name="toFilePath">   the destination file path. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void DecompressScriptFile( this ScriptInfo scriptInfo, string fromFilePath, string toFilePath, bool overWrite = false, bool validate = true )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );
        if ( string.IsNullOrWhiteSpace( fromFilePath ) ) throw new ArgumentNullException( nameof( fromFilePath ) );
        if ( string.IsNullOrWhiteSpace( toFilePath ) ) throw new ArgumentNullException( nameof( toFilePath ) );

        fromFilePath.DecompressScriptFile( toFilePath, scriptInfo.Compressor, scriptInfo.Encryptor, overWrite, validate );
    }
}
