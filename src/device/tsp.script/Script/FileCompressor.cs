using System.IO.Compression;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   A file compressor. </summary>
/// <remarks>   2025-04-08. </remarks>
public static class FileCompressor
{
    /// <summary>   Compress the file. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="originalFilePath">     Full pathname of the original file. </param>
    /// <param name="compressedFilePath">   Full pathname of the compressed file. </param>
    public static void Compress( string originalFilePath, string compressedFilePath )
    {
        using FileStream originalFileStream = File.Open( originalFilePath, FileMode.Open );
        using FileStream compressedFileStream = File.Create( compressedFilePath );
        using GZipStream compressor = new( compressedFileStream, CompressionMode.Compress );
        originalFileStream.CopyTo( compressor );
    }

    /// <summary>   Decompress the file. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="compressedFilePath">   Full pathname of the compressed file. </param>
    /// <param name="decompressedFilePath"> Full pathname of the decompressed file. </param>
    public static void Decompress( string compressedFilePath, string decompressedFilePath )
    {
        using FileStream compressedFileStream = File.Open( compressedFilePath, FileMode.Open );
        using FileStream outputFileStream = File.Create( decompressedFilePath );
        using GZipStream decompressor = new( compressedFileStream, CompressionMode.Decompress );
        decompressor.CopyTo( outputFileStream );
    }

    /// <summary>   Query if the file is compressed. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="filePath"> Full pathname of the file. </param>
    /// <returns>   True if compressed, false if not. </returns>
    public static bool IsCompressed( string filePath )
    {
        using FileStream compressedFileStream = File.Open( filePath, FileMode.Open );
        byte[] bytes = new byte[2];
        int count = compressedFileStream.Read( bytes, 0, 2 );
        if ( count == 2 )
            // Check the first two bytes for GZip magic number
            return bytes[0] == 0x1F && bytes[1] == 0x8B;
        else
            throw new InvalidOperationException( "Could not read the file." );
    }
}
