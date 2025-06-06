using cc.isr.Std.LineEndingExtensions;

namespace cc.isr.VI.Tsp.Script.ExportExtensions;

/// <summary>   A script source extension methods. </summary>
/// <remarks>   2025-05-02. </remarks>
public static partial class ExportExtensionsMethods
{
    /// <summary>   A string extension method that decompress the script file. </summary>
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
    public static void DecompressScriptFile( this string fromFilePath, string toFilePath, bool overWrite = false, bool validate = true )
    {
        if ( string.IsNullOrWhiteSpace( fromFilePath ) ) throw new ArgumentNullException( nameof( fromFilePath ) );
        if ( string.IsNullOrWhiteSpace( toFilePath ) ) throw new ArgumentNullException( nameof( toFilePath ) );

        if ( !overWrite && System.IO.File.Exists( toFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{toFilePath}' exists." );

        // read the compressed contents from the file.
        string contents = System.IO.File.ReadAllText( fromFilePath );

        if ( ScriptCompressor.IsCompressed( contents ) )
            contents = ScriptCompressor.Decompress( contents );

        // terminate the decompressed contents with a single line ending.
        contents = contents.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        contents = contents.TerminateLines( validate );

        // compress and export the source to the file as is.
        System.IO.File.WriteAllText( toFilePath, contents, System.Text.Encoding.Default );
    }
}
