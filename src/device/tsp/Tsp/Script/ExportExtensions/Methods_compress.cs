using System.Text;

namespace cc.isr.VI.Tsp.Script.ExportExtensions;

/// <summary>   A script source extension methods. </summary>
/// <remarks>   2025-05-02. </remarks>
public static partial class ExportExtensionsMethods
{
    /// <summary>
    /// A <see cref="string"/> extension method that compress a script file to a destination file.
    /// </summary>
    /// <remarks>   2025-04-15. </remarks>
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
    public static void CompressScriptFile( this string fromFilePath, string toFilePath, bool overWrite = false, bool validate = true )
    {
        if ( string.IsNullOrWhiteSpace( fromFilePath ) )
            throw new ArgumentNullException( nameof( fromFilePath ) );
        if ( string.IsNullOrWhiteSpace( toFilePath ) )
            throw new ArgumentNullException( nameof( toFilePath ) );

        if ( !overWrite && System.IO.File.Exists( toFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{toFilePath}' exists." );

        string scriptSource = System.IO.File.ReadAllText( fromFilePath );

        // Note that the actual output is not validated against the input because the line endings might change.
        // Rather, what is validated is that the reader and writer are able to process the script source.
        if ( validate )
            scriptSource.ValidateStringReaderStringWriter();

        // if required, convert the line endings to windows format.
        using TextReader? reader = new System.IO.StringReader( scriptSource )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader from the script source." );

        StringBuilder sb = new();
        using TextWriter? writer = new System.IO.StringWriter( sb )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a writer for the string builder" );

        // export the script to the string build in Windows format.
        reader.ExportScript( writer );

        // compress and export the source to the file as is.
        System.IO.File.WriteAllText( toFilePath, ScriptCompressor.Compress( sb.ToString() ) );
    }

    /// <summary>
    /// A <see cref="string"/> extension method that converts the source to Windows format and then compress the source to file.
    /// </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="scriptSource"> The script source. </param>
    /// <param name="toFilePath">   the destination file path. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void CompressScript( this string scriptSource, string toFilePath, bool overWrite = false, bool validate = true )
    {
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new ArgumentNullException( nameof( scriptSource ) );
        if ( string.IsNullOrWhiteSpace( toFilePath ) )
            throw new ArgumentNullException( nameof( toFilePath ) );

        if ( !overWrite && File.Exists( toFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{toFilePath}' exists." );

        // Note that the actual output is not validated against the input because the line endings might change.
        // Rather, what is validated is that the reader and writer are able to process the script source.
        if ( validate )
            scriptSource.ValidateStringReaderStringWriter();

        // if required, convert the line endings to windows format.
        using TextReader? reader = new StringReader( scriptSource )
            ?? throw new FileNotFoundException( $"Failed creating a reader from the script source" );

        StringBuilder sb = new();
        using TextWriter? writer = new StringWriter( sb )
            ?? throw new FileNotFoundException( $"Failed creating a writer for the string builder" );

        reader.ExportScript( writer );

        // compress and export the source to the file as is.
        File.WriteAllText( toFilePath, ScriptCompressor.Compress( sb.ToString() ) );
    }

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

        // read the source file.
        string source = System.IO.File.ReadAllText( fromFilePath );

        if ( ScriptCompressor.IsCompressed( source ) )
            source = ScriptCompressor.Decompress( source );

        // Note that the actual output is not validated against the input because the line endings might change.
        // Rather, what is validated is that the reader and writer are able to process the script source.
        if ( validate )
            source.ValidateStringReaderStringWriter();

        // if required, convert the line endings to windows format.
        using TextReader? textReader = new System.IO.StringReader( source )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader from the '{fromFilePath}' file source." );

        StringBuilder sb = new();
        using TextWriter? writer = new System.IO.StringWriter( sb )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a writer for the string builder" );

        // export the scrip to the string builder using Windows end of file format.
        textReader.ExportScript( writer );

        // compress and export the source to the file as is.
        System.IO.File.WriteAllText( toFilePath, sb.ToString() );
    }
}
