using System.Text;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   A script compressor. </summary>
/// <remarks>   2025-04-14. </remarks>
public static class ScriptCompressor
{
    /// <summary>   Returns the compressed code prefix. </summary>
    /// <value> The compressed prefix. </value>
    public static string CompressedPrefix => "<COMPRESSED>";

    /// <summary>   Returns the compressed code suffix. </summary>
    /// <value> The compressed suffix. </value>
    public static string CompressedSuffix => "</COMPRESSED>";

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
            StringBuilder builder = new( prefix );
            _ = builder.Append( compressedContents );
            _ = builder.Append( suffix );
            return builder.ToString();
        }
        else
            return compressedContents;
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
            StringBuilder builder = new( prefix );
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
        return Compress( contents, ScriptCompressor.CompressedPrefix, ScriptCompressor.CompressedSuffix );
    }

    /// <summary>   Returns the decompressed string of the value. </summary>
    /// <param name="contents">    The contents to decompress. </param>
    /// <param name="prefix">   The prefix. </param>
    /// <param name="suffix">   The suffix. </param>
    /// <returns>   Decompressed value. </returns>
    public static string Decompress( string contents, string prefix, string suffix )
    {
        string source = string.Empty;
        if ( contents.StartsWith( prefix, false, System.Globalization.CultureInfo.CurrentCulture ) )
        {
            int fromIndex = contents.IndexOf( prefix, StringComparison.OrdinalIgnoreCase ) + prefix.Length;
            int toIndex = contents.IndexOf( suffix, StringComparison.OrdinalIgnoreCase ) - 1;
            source = contents.Substring( fromIndex, toIndex - fromIndex + 1 );
            source = cc.isr.Std.IO.Compression.StringCompressor.DecompressFromBase64( source );
        }
        return source;
    }

    /// <summary>   Returns the decompressed string after removing the <see cref="ScriptCompressor.CompressedPrefix"/>
    /// and <see cref="ScriptCompressor.CompressedSuffix"/>. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="contents"> The contents to decompress. </param>
    /// <returns>   Decompressed value. </returns>
    public static string Decompress( string contents )
    {
        return Decompress( contents, ScriptCompressor.CompressedPrefix, ScriptCompressor.CompressedSuffix );
    }
}
