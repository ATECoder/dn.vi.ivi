using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class Methods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that fetches and exports a script to file. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    public static void ExportScript( this SessionBase session, string scriptName, string filePath, bool overWrite = false )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( filePath ) )
            throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && System.IO.File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because the file '{filePath}' exists." );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it was not found." );

        string scriptSource = session.FetchScript( scriptName );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it is empty." );

        // append end of line to the script source.
        scriptSource += "\r\n";

        // write the source to file.
        scriptSource.ExportScript( filePath, overWrite: overWrite );
    }

    /// <summary>   A <see cref="TextReader"/> extension method that exports script to file preserving the Windows end-of-line format. </summary>
    /// <remarks>   2025-04-12. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="reader">       The reader. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="fromEol">      (Optional) end-of-line to change. </param>
    /// <param name="toEol">        (Optional) end-of-line to change to. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    public static void ExportScript( this TextReader reader, string filePath, string fromEol = "\n", string toEol = "\r\n", bool overWrite = false )
    {
        if ( reader == null )
            throw new ArgumentNullException( nameof( reader ) );
        if ( string.IsNullOrWhiteSpace( filePath ) )
            throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && System.IO.File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{filePath}' exists." );

        using StreamWriter? writer = new System.IO.StreamWriter( filePath )
            ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader from the script source" );

        string? line = "";
        bool? isExpectedLineFormat = null;
        char[] oldEol = string.IsNullOrEmpty( fromEol ) ? [] : fromEol.ToCharArray();
        while ( line is not null )
        {
            line = reader.ReadLine();

            if ( line is not null )
            {
                // check if the line ends with a windows EOL format.
                isExpectedLineFormat ??= string.IsNullOrEmpty( fromEol ) || string.IsNullOrEmpty( toEol ) || line.EndsWith( toEol );

                if ( true != isExpectedLineFormat )
                {
                    line = line.TrimEnd( oldEol );
                    line += toEol;
                }
                // writer.WriteLine( line );
                writer.Write( line );
            }
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
    /// <param name="fromEol">      (Optional) end-of-line to change. </param>
    /// <param name="toEol">        (Optional) end-of-line to change to. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    public static void ExportScript( this string scriptSource, string filePath, string fromEol = "\n", string toEol = "\r\n", bool overWrite = false )
    {
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new ArgumentNullException( nameof( scriptSource ) );
        if ( string.IsNullOrWhiteSpace( filePath ) )
            throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && System.IO.File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{filePath}' exists." );

        if ( string.IsNullOrEmpty( fromEol ) || string.IsNullOrEmpty( toEol ) )
        {
            // export the source to the file as is.
            System.IO.File.WriteAllText( filePath, scriptSource );
        }
        else
        {
            using TextReader? reader = new System.IO.StringReader( scriptSource )
                ?? throw new System.IO.FileNotFoundException( $"Failed creating a reader from the script source" );

            reader.ExportScript( filePath, fromEol, toEol, overWrite );
        }

        // check tat the file size matches the source size.
        FileInfo fileInfo = new( filePath );
        if ( fileInfo.Length < scriptSource.Length )
            throw new InvalidOperationException(
                $"The exported script source file '{filePath}' length {fileInfo.Length} is smaller that the script source length {scriptSource.Length}." );
    }

}
