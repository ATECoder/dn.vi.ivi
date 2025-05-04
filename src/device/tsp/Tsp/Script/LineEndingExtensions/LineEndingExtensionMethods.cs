using System.Text;
using cc.isr.VI.Tsp.Script.ExportExtensions;

namespace cc.isr.VI.Tsp.Script.LineEndingExtensions;

/// <summary>   A line ending extension methods. </summary>
/// <remarks>   2025-05-03. </remarks>
public static class LineEndingExtensionMethods
{

    /// <summary>   A string extension method that terminate lines. </summary>
    /// <remarks>   2025-05-03. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="contents"> The contents to act on. </param>
    /// <param name="validate"> (Optional) [true] True to validate the reader input against the
    ///                         writer output. </param>
    /// <returns>   A string. </returns>
    public static string TerminateLines( this string contents, bool validate = true )
    {
        if ( string.IsNullOrWhiteSpace( contents ) ) throw new ArgumentNullException( nameof( contents ) );

        // add the new line so that the reader input matches the writer output which ends with a new line
        contents = contents.TrimMultipleLineEndings();

        // Validate that the writer output equals the reader input.
        if ( validate )
            contents.ValidateStringReaderStringWriter();

        // if required, convert the line endings to windows format.
        using TextReader? reader = new System.IO.StringReader( contents )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader from the script source." );

        StringBuilder sb = new();
        using TextWriter? writer = new System.IO.StringWriter( sb )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a writer for the string builder" );

        // export the script to the string build in Windows format.
        reader.ExportScript( writer );

        // return the line terminate script.
        return sb.ToString();
    }

    /// <summary>   A <see cref="string"/> extension method that terminate lines read from the input file. </summary>
    /// <remarks>   2025-05-03. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="fromFilePath"> The source file path. </param>
    /// <param name="validate">     (Optional) [true] True to validate the reader input against the writer output. </param>
    public static string TerminateFileLines( this string fromFilePath, bool validate = true )
    {
        if ( string.IsNullOrWhiteSpace( fromFilePath ) )
            throw new ArgumentNullException( nameof( fromFilePath ) );

        return LineEndingExtensionMethods.TerminateFileLines( System.IO.File.ReadAllText( fromFilePath ), validate );
    }
}
