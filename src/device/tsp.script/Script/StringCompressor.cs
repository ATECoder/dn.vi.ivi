using System.IO.Compression;
using System.Text;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   A string compressor. </summary>
/// <remarks>   2025-04-08.<para>
/// <see href="https://stackoverflow.com/questions/7343465/compression-decompression-string-with-c-sharp"/> </para>
/// </remarks>
public static class StringCompressor
{

    /// <summary>   Copies to. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="fromStream">   from stream. </param>
    /// <param name="toStream">     to stream. </param>
    public static void CopyTo( Stream fromStream, Stream toStream )
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ( (cnt = fromStream.Read( bytes, 0, bytes.Length )) != 0 )
        {
            toStream.Write( bytes, 0, cnt );
        }
    }

    /// <summary>   Compress. </summary>
    /// <remarks>   2025-04-08. <para>
    /// This returns a byte[], while Decompress returns a string. Base64 encode (for example by using Convert.ToBase64String(r1)) 
    /// to encode the decompressed outcome (the result of Compress is VERY binary! It isn't something you can print to the screen or write directly in an XML).
    /// </para></remarks>
    /// <param name="contents">  The contents to compress. </param>
    /// <returns>   A byte[]. </returns>
    public static byte[] Compress( string contents )
    {
        byte[] bytes = Encoding.UTF8.GetBytes( contents );

        using MemoryStream msi = new( bytes );
        using MemoryStream mso = new();
        using ( GZipStream gs = new( mso, CompressionMode.Compress ) )
            CopyTo( msi, gs );

        return mso.ToArray();
    }

    /// <summary>   Compress to base 64. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="contents">  The contents to compress. </param>
    /// <returns>   A string. </returns>
    public static string CompressToBase64( string contents )
    {
        return Convert.ToBase64String( Compress( contents ) );
    }

    /// <summary>   Decompress the given bytes. </summary>
    /// <remarks>   2025-04-08. <para>
    /// </para></remarks>
    /// <param name="compressed">    The compressed bytes. </param>
    /// <returns>   A string. </returns>
    public static string Decompress( byte[] compressed )
    {
        using MemoryStream msi = new( compressed );
        using MemoryStream mso = new();
        using ( GZipStream gs = new( msi, CompressionMode.Decompress ) )
            CopyTo( gs, mso );

        return Encoding.UTF8.GetString( mso.ToArray() );
    }

    /// <summary>   Decompress from base 64 string. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="compressed">   The compressed bytes. </param>
    /// <returns>   A string. </returns>
    public static string DecompressFromBase64( string compressed )
    {
        return Decompress( Convert.FromBase64String( compressed ) );
    }

    /// <summary>   Query if 'compressed' is a compressed stream. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="bytes">   The bytes to check for compression. </param>
    /// <returns>   True if compressed, false if not. </returns>
    public static bool IsCompressed( byte[] bytes )
    {
        if ( bytes.LongLength >= 2 )
            // Check the first two bytes for GZip magic number
            return bytes[0] == 0x1F && bytes[1] == 0x8B;
        else
            throw new InvalidOperationException( "Insufficient data; must be at least 2 bytes." );
    }

    /// <summary>   Query if 'contents' is base 64 compressed. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="contents"> The contents to compress. </param>
    /// <returns>   True if base 64 compressed, false if not. </returns>
    public static bool IsBase64Compressed( string contents )
    {
        if ( contents.Length >= 4 )
        {
            try
            {
                byte[] bytes = Convert.FromBase64String( contents[..4] );
                // Check the first two bytes for GZip magic number
                return bytes[0] == 0x1F && bytes[1] == 0x8B;
            }
            catch ( System.FormatException )
            {
                return false;
            }
        }
        else
            throw new InvalidOperationException( "Insufficient data; must be at least 4 characters." );
    }

}
