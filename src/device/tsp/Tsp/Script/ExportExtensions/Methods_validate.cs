using System.Text;

namespace cc.isr.VI.Tsp.Script.ExportExtensions;

public static partial class ExportExtensionsMethods
{
    /// <summary>
    /// A <see cref="string"/> extension method that validates that the output from a string writer
    /// is equal to the input from the stream reader.
    /// </summary>
    /// <remarks> 2025-05-02. <para>
    /// This became important after it was found that the stream miss-identified line
    /// endings when reading TSP byte code. </para><para>
    /// The stream reader fails reading a script that is saved in the TSP byte code format because it
    /// may incorrectly interpret an a backslash as an end of file. For example, the following line
    /// from the source file became the last line of the destination file.<c>
    /// "\91\201\2\185\92I\2\186\92\201\2\187\93I\2\188\93\201\2\189^", "\91\201\2\185\92I\2\186\92\20
    /// </c> </para> </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="filePath"> Full pathname of the file from which to read. </param>
    [Obsolete( "A stream reader must not be used to read scripts." )]
    public static void ValidateStreamReaderStringWriter( this string filePath )
    {
        if ( filePath is null || string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        string originalContents = System.IO.File.ReadAllText( filePath );

        // if required, convert the line endings to windows format.
        using TextReader? reader = new System.IO.StreamReader( filePath )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader from {filePath}" );

        StringBuilder sb = new();
        using TextWriter? writer = new System.IO.StringWriter( sb )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a writer for the string builder" );

        string? line = "";
        while ( line is not null )
        {
            line = reader.ReadLine();
            if ( line is not null )
                writer.WriteLine( line );
        }
        if ( !string.Equals( originalContents, sb.ToString(), StringComparison.Ordinal ) )
        {
            string tempPath = System.IO.Path.GetTempPath();
            string readerFileName = $"ReaderContents.{System.DateTime.Now:yyyyMMddHHmmss}.txt";
            string writerFileName = $"WriterContents.{System.DateTime.Now:yyyyMMddHHmmss}.txt";
            System.IO.File.WriteAllText( System.IO.Path.Combine( tempPath, readerFileName ), originalContents, System.Text.Encoding.UTF8 );
            System.IO.File.WriteAllText( System.IO.Path.Combine( tempPath, writerFileName ), sb.ToString(), System.Text.Encoding.UTF8 );
            throw new InvalidOperationException( $"String writer output {writerFileName} is not equal the string reader input {readerFileName}.\r\n\tin {tempPath}, " );
        }
    }


    /// <summary>
    /// A <see cref="string"/> extension method that validates that the output from a string writer
    /// is equal to the input from the string reader. The <paramref name="originalContents"/> is
    /// expected to be using Windows end of line characters.
    /// </summary>
    /// <remarks>
    /// 2025-05-02. <para>
    /// This became important after it was found that the stream miss-identified line endings when
    /// reading TSP byte code. </para> <para>
    /// Note that this assumes that the original contents ends with a new line. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="originalContents"> The original contents to read from and compare to. </param>
    public static void ValidateStringReaderStringWriter( this string originalContents )
    {
        if ( originalContents is null || string.IsNullOrWhiteSpace( originalContents ) ) throw new ArgumentNullException( nameof( originalContents ) );

        // if required, convert the line endings to windows format.
        using TextReader? reader = new StringReader( originalContents )
            ?? throw new FileNotFoundException( $"Failed creating a reader from the script source" );

        StringBuilder sb = new();
        using TextWriter? writer = new StringWriter( sb )
            ?? throw new FileNotFoundException( $"Failed creating a writer for the string builder" );

        string? line = "";
        while ( line is not null )
        {
            line = reader.ReadLine();
            if ( line is not null )
                writer.WriteLine( line );
        }
        if ( !string.Equals( originalContents, sb.ToString(), StringComparison.Ordinal ) )
        {
            string tempPath = Path.GetTempPath();
            string readerFileName = $"ReaderContents.{DateTime.Now:yyyyMMddHHmmss}.txt";
            string writerFileName = $"WriterContents.{DateTime.Now:yyyyMMddHHmmss}.txt";
            File.WriteAllText( Path.Combine( tempPath, readerFileName ), originalContents, System.Text.Encoding.UTF8 );
            File.WriteAllText( Path.Combine( tempPath, writerFileName ), sb.ToString(), System.Text.Encoding.UTF8 );
            throw new InvalidOperationException( $"String writer output {writerFileName} is not equal the string reader input {readerFileName}.\r\n\tin {tempPath}, " );
        }
    }

    /// <summary>
    /// A <see cref="string"/> extension method that validates that the output from a stream writer
    /// is equal to the input from the string reader. The <paramref name="originalContents"/> is
    /// expected to be using Windows end of line characters.
    /// </summary>
    /// <remarks>
    /// 2025-05-02. <para>
    /// This became important after it was found that the stream miss-identified line endings when
    /// reading TSP byte code. </para><para>
    /// Assumes that the original contents end with a new line. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="originalContents"> The originalContents to read from and to compare to. </param>
    public static void ValidateStringReaderStreamWriter( this string originalContents )
    {
        if ( originalContents is null || string.IsNullOrWhiteSpace( originalContents ) ) throw new ArgumentNullException( nameof( originalContents ) );

        string filePath = Path.GetTempFileName();

        // if required, convert the line endings to windows format.
        using TextReader? reader = new StringReader( originalContents )
            ?? throw new FileNotFoundException( $"Failed creating a reader from the script source" );

        using StreamWriter? writer = new StreamWriter( filePath )
            ?? throw new FileNotFoundException( $"Failed creating a writer for the temporary file path '{filePath}'" );

        string? line = "";
        while ( line is not null )
        {
            line = reader.ReadLine();
            if ( line is not null )
                writer.WriteLine( line );
        }

        // we got an exception trying to read back from the file path. 
        writer.Close();

        // read back the contents of the file.
        string actualContents = File.ReadAllText( filePath );
        if ( !string.Equals( originalContents, actualContents, StringComparison.Ordinal ) )
        {
            string tempPath = Path.GetTempPath();
            string readerFileName = $"ReaderContents.{DateTime.Now:yyyyMMddHHmmss}.txt";
            string writerFileName = $"WriterContents.{DateTime.Now:yyyyMMddHHmmss}.txt";
            File.WriteAllText( Path.Combine( tempPath, readerFileName ), originalContents, System.Text.Encoding.UTF8 );
            File.WriteAllText( Path.Combine( tempPath, writerFileName ), actualContents, System.Text.Encoding.UTF8 );
            throw new InvalidOperationException( $"Stream writer output {writerFileName} is not equal the string reader input {readerFileName}.\r\n\tin {tempPath}, " );
        }
    }
}
