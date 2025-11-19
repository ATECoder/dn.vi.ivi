namespace cc.isr.VI.Tsp.Script;

/// <summary>   Interface for script compressor. </summary>
/// <remarks>   2025-09-19. </remarks>
public interface IScriptCompressor
{

    /// <summary>   Compress the plain text to base 64 format. </summary>
    /// <param name="plainText"> The plain text being compressed. </param>
    /// <returns>   The compressed plain text in base 64 format. </returns>
    public string CompressToBase64( string plainText );

    /// <summary>   Decompress the compressed text from base 64. </summary>
    /// <param name="compressedText"> The compressed text in base 64 format. </param>
    /// <returns>   The decompressed plain text. </returns>
    public string DecompressFromBase64( string compressedText );

    /// <summary>   Query if 'contents' is compressed. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="contents"> The content to be checked for compression. </param>
    /// <returns>   True if compressed, false if not. </returns>
    public bool IsCompressed( string contents );
}
