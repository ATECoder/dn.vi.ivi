using cc.isr.Std.LineEndingExtensions;
namespace cc.isr.VI.Tsp.Script.ExportExtensions;

public static partial class ExportExtensionsMethods
{
    /// <summary>
    /// A <see cref="TextReader"/> extension method that exports a script to file in Windows end-of-
    /// line format.
    /// </summary>
    /// <remarks>
    /// 2025-04-20. <para>
    /// The stream reader fails reading a script that is embedded as the TSP byte code format because it
    /// may incorrectly interpret a backslash as an end of file. For example, the following line
    /// from the source file became the last line of the destination file.<c>
    /// "\91\201\2\185\92I\2\186\92\201\2\187\93I\2\188\93\201\2\189^", "\91\201\2\185\92I\2\186\92\20
    /// </c>
    /// </para><para>
    /// Note that the output file ends with new line characters. </para>
    /// 
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="reader">       The reader. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    internal static void ExportScript( this TextReader reader, string filePath, bool overWrite = false )
    {
        if ( reader == null ) throw new ArgumentNullException( nameof( reader ) );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );
        if ( reader is StreamReader )
            throw new InvalidOperationException( "A stream reader must not be used for reading TSP byte code." );

        if ( !overWrite && File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{filePath}' exists." );

        using StreamWriter? writer = new StreamWriter( filePath )
            ?? throw new FileNotFoundException( $"Failed creating a reader from the script source" );

        string? line = "";
        while ( line is not null )
        {
            line = reader.ReadLine();

            if ( line is not null )
                writer.WriteLine( line );
        }
    }

    /// <summary>
    /// A <see cref="TextReader"/> extension method that exports a script to windows end of line
    /// format using a <see cref="TextWriter"/>.
    /// </summary>
    /// <remarks>
    /// 2025-04-20. <para>
    /// The stream reader fails reading a script that is embedded as byte code format because it
    /// may incorrectly interpret a backslash as an end of file. For example, the following line
    /// from the source file became the last line of the destination file.<c>
    /// "\91\201\2\185\92I\2\186\92\201\2\187\93I\2\188\93\201\2\189^", "\91\201\2\185\92I\2\186\92\20
    /// </c>
    /// </para> <para>
    /// Note that the output ends with a line characters.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="reader">   The reader. </param>
    /// <param name="writer">   The writer. </param>
    internal static void ExportScript( this TextReader reader, TextWriter writer )
    {
        if ( reader is null ) throw new ArgumentNullException( nameof( reader ) );
        if ( writer is null ) throw new ArgumentNullException( nameof( writer ) );
        if ( reader is StreamReader )
            throw new InvalidOperationException( "A stream reader must not be used for reading TSP byte code." );

        string? line = "";
        while ( line is not null )
        {
            line = reader.ReadLine();

            if ( line is not null )
                writer.WriteLine( line );
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that exports a script to file.
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="scriptSource"> The script source. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void ExportScript( this string scriptSource, string filePath, bool overWrite = false, bool validate = true )
    {
        if ( string.IsNullOrWhiteSpace( scriptSource ) ) throw new ArgumentNullException( nameof( scriptSource ) );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{filePath}' exists." );

        // ensure that the source ends with a single line termination.
        scriptSource = scriptSource.TrimMultipleLineEndings();

        // Note that the actual output is not validated against the input because the line endings might change.
        // Rather, what is validated is that the reader and writer are able to process the script source.
        if ( validate )
            scriptSource.ValidateStringReaderStreamWriter();

        using TextReader? reader = new StringReader( scriptSource )
            ?? throw new FileNotFoundException( $"Failed creating a reader from the script source" );

        reader.ExportScript( filePath, overWrite );

        // check that the file size matches the source size.
        FileInfo fileInfo = new( filePath );
        if ( fileInfo.Length < scriptSource.Length )
            throw new InvalidOperationException(
                $"The exported script source file '{filePath}' length {fileInfo.Length} is smaller that the script source length {scriptSource.Length}." );
    }
}

