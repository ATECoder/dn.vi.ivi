using cc.isr.Std.EqualityExtensions;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   A script compressor. </summary>
/// <remarks>   2025-04-14. </remarks>
public static class ScriptCompressor
{
    /// <summary>   Returns the compressed code prefix. </summary>
    /// <value> The compressed prefix. </value>
    public static string CompressedPrefix => "<COMPRESSED>\r\n";

    /// <summary>   Returns the compressed code suffix. </summary>
    /// <value> The compressed suffix. </value>
    public static string CompressedSuffix => "\r\n</COMPRESSED>";

    /// <summary>   Decorates the compressed value decorated with the prefix and suffix. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="compressedContents">   The compressed contents. </param>
    /// <param name="prefix">               The prefix. </param>
    /// <param name="suffix">               The suffix. </param>
    /// <returns>   The decorated compressed contents. </returns>
    public static string Decorate( string compressedContents, string prefix, string suffix )
    {
        if ( !string.IsNullOrWhiteSpace( prefix ) || !string.IsNullOrWhiteSpace( prefix ) )
        {
            System.Text.StringBuilder builder = new( prefix );
            _ = builder.Append( compressedContents );
            _ = builder.Append( suffix );
            return builder.ToString();
        }
        else
            return compressedContents;
    }

    /// <summary>   Validates the compression described by contents. </summary>
    /// <remarks>   2025-05-03. </remarks>
    /// <param name="contents"> The string being compressed. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool ValidateCompression( string contents )
    {
        string decompressed = ScriptCompressor.Decompress( ScriptCompressor.Compress( contents ) );
        return string.Equals( contents, decompressed, StringComparison.Ordinal );
    }

    /// <summary>   Returns a compressed value. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="contents"> The string being compressed. </param>
    /// <param name="prefix">   The prefix. </param>
    /// <param name="suffix">   The suffix. </param>
    /// <returns>   The compressed contents. </returns>
    public static string Compress( string contents, string prefix, string suffix )
    {
        contents = cc.isr.Std.IO.Compression.StringCompressor.CompressToBase64( contents );
        if ( !string.IsNullOrWhiteSpace( prefix ) || !string.IsNullOrWhiteSpace( prefix ) )
        {
            System.Text.StringBuilder builder = new( prefix );
            _ = builder.Append( contents );
            _ = builder.Append( suffix );
            return builder.ToString();
        }
        else
            return contents;
    }

    /// <summary>
    /// Returns a compressed value decorated with the <see cref="ScriptCompressor.CompressedPrefix"/>
    /// and <see cref="ScriptCompressor.CompressedSuffix"/>.
    /// </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="contents"> The string being compressed. </param>
    /// <returns>   The compressed contents. </returns>
    public static string Compress( string contents )
    {
        return ScriptCompressor.Compress( contents, ScriptCompressor.CompressedPrefix, ScriptCompressor.CompressedSuffix );
    }

    /// <summary>   Strip decorations. </summary>
    /// <remarks>   2025-05-03. </remarks>
    /// <param name="contents"> The string being compressed. </param>
    /// <param name="prefix">   The prefix. </param>
    /// <param name="suffix">   The suffix. </param>
    /// <returns>   A string. </returns>
    public static string StripDecorations( string contents, string prefix, string suffix )
    {
        // this handles the case where the reader might have read the initial block.
        int fromIndex = contents.Contains( prefix, StringComparison.Ordinal )
            ? contents.IndexOf( prefix, StringComparison.OrdinalIgnoreCase ) + prefix.Length
            : 0;
        int count = contents.Contains( suffix, StringComparison.Ordinal )
            ? contents.IndexOf( suffix, StringComparison.OrdinalIgnoreCase ) - fromIndex
            : contents.Length - fromIndex;
        return contents.Substring( fromIndex, count + 1 );
    }

    /// <summary>   Strip decorations. </summary>
    /// <remarks>   2025-05-03. </remarks>
    /// <param name="contents"> The string being compressed. </param>
    /// <returns>   A string. </returns>
    public static string StripDecorations( string contents )
    {
        return ScriptCompressor.StripDecorations( contents, ScriptCompressor.CompressedPrefix, ScriptCompressor.CompressedSuffix );
    }

    /// <summary>   Returns the decompressed string of the value. </summary>
    /// <param name="contents">    The contents to decompress. </param>
    /// <param name="prefix">   The prefix. </param>
    /// <param name="suffix">   The suffix. </param>
    /// <returns>   Decompressed value. </returns>
    public static string Decompress( string contents, string prefix, string suffix )
    {
        string source = ScriptCompressor.StripDecorations( contents, prefix, suffix );
        source = cc.isr.Std.IO.Compression.StringCompressor.DecompressFromBase64( source );
        return source;
    }

    /// <summary>   Returns the decompressed string after removing the <see cref="ScriptCompressor.CompressedPrefix"/>
    /// and <see cref="ScriptCompressor.CompressedSuffix"/>. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="contents"> The contents to decompress. </param>
    /// <returns>   Decompressed value. </returns>
    public static string Decompress( string contents )
    {
        return ScriptCompressor.Decompress( contents, ScriptCompressor.CompressedPrefix, ScriptCompressor.CompressedSuffix );
    }

    /// <summary>   Compressed files equal. </summary>
    /// <remarks>   2025-05-03. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="filePath1">        The first file path. </param>
    /// <param name="filePath2">        The second file path. </param>
    /// <param name="useStringEquals">  (Optional) True to use string equals. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool CompressedFilesEqual( string filePath1, string filePath2, bool useStringEquals = true )
    {
        if ( filePath1 == null && filePath2 == null )
            return true;
        else if ( filePath1 == null || filePath2 == null )
            return false;
        else if ( filePath1.Equals( filePath2, StringComparison.OrdinalIgnoreCase ) )
            return true;
        else if ( System.IO.File.Equals( filePath1, filePath2 ) )
            return true;
        else if ( System.IO.File.Exists( filePath1 ) && System.IO.File.Exists( filePath2 ) )
        {
            return useStringEquals
                ? ScriptCompressor.Decompress( System.IO.File.ReadAllText( filePath1 ) ).Equals(
                      ScriptCompressor.Decompress( System.IO.File.ReadAllText( filePath2 ) ), StringComparison.InvariantCulture )
                : ScriptCompressor.Decompress( System.IO.File.ReadAllText( filePath1 ) ).LinesEqual(
                      ScriptCompressor.Decompress( System.IO.File.ReadAllText( filePath2 ) ) );
        }
        else
            throw new FileNotFoundException( "One or both of the files do not exist.", filePath1 + " or " + filePath2 );
    }


    /// <summary>   Query if 'contents' is compressed. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="contents"> The string being compressed. </param>
    /// <returns>   True if compressed, false if not. </returns>
    public static bool IsCompressed( string contents )
    {
        return contents.StartsWith( ScriptCompressor.CompressedPrefix, false, System.Globalization.CultureInfo.CurrentCulture );
    }

    /// <summary>   Query if 'filePath' is compressed file. </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="filePath"> Full pathname of the file. </param>
    /// <returns>   True if compressed file, false if not. </returns>
    public static bool IsCompressedFile( string filePath )
    {
        using StreamReader? reader = (string.IsNullOrWhiteSpace( filePath ) || !System.IO.File.Exists( filePath )
            ? null
            : new System.IO.StreamReader( filePath )) ?? throw new System.IO.FileNotFoundException( "Failed opening script file", filePath );
        return ScriptCompressor.IsCompressed( reader );
    }

    /// <summary>   Query if 'contents' is compressed. </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <param name="reader">   The reader. </param>
    /// <returns>   True if compressed, false if not. </returns>
    public static bool IsCompressed( TextReader reader )
    {
        int count = ScriptCompressor.CompressedPrefix.Length;
        char[] buffer = new char[count];
        _ = reader.ReadBlock( buffer, 0, count );
        string value = new( buffer );

        // rewind the stream.
        if ( reader is StreamReader streamReader )
        {
            long position = streamReader.BaseStream.Position = 0;
            streamReader.DiscardBufferedData();
            if ( position > 0 )
                throw new InvalidOperationException( $"Failed rewinding the stream reader to position 0. Current position is {position}." );
        }
        return ScriptCompressor.IsCompressed( value );
    }
}
