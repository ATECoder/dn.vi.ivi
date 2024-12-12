using System.Text;
using System.Security.Cryptography;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   An encryption engine. </summary>
/// <remarks>
/// (c) 2014 Matthew Givens <para>
/// <see href="https://www.codeproject.com/Articles/796587/Using-Encrypted-Files-Without-Decrypting-To-Disk">
/// Code project article</see>. </para><para>
/// <see href="http://www.codeproject.com/info/cpol10.aspx">license</see>. </para>
/// </remarks>
public static class EncryptionEngine
{
    private const int Start_Index = 65;
    private const int Key_Length = 8;
    private static string Key { get; } = cc.isr.VI.SolutionInfo.PublicKey.Substring( Start_Index, Key_Length );
    private static string Vector { get; } = cc.isr.VI.SolutionInfo.PublicKey.Substring( Start_Index + Key_Length, Key_Length );

    /// <summary>
    /// This procedure accepts two filenames, one for the raw input file and the other for the
    /// encrypted output file. It loads the raw data into an internal buffer, then writes that
    /// buffer to the output file using an encryption stream based upon an output stream.
    /// </summary>
    /// <remarks>   2024-12-10. </remarks>
    /// <param name="sourceFile">   Source file. </param>
    /// <param name="destination">  Destination file. </param>
    public static void EncryptBinaryFile( string sourceFile, string destination )
    {
        // Get file info, used to know the size of the file
        FileInfo info = new( sourceFile );

        // Open the file for input
        using FileStream input = new( sourceFile, FileMode.Open, FileAccess.Read );

        // Create a data buffer large enough to hold the file -- Could be modified to read the file in smaller chunks.
        byte[] dta = new byte[info.Length];

        // Read the raw file into the buffer and close it
        _ = input.Read( dta, 0, ( int ) info.Length );
        input.Close();

        // Open the file for output
        using FileStream output = new( destination, FileMode.Create, FileAccess.Write );

        // Setup the encryption object
        using DESCryptoServiceProvider cryptic = new()
        {
            Key = ASCIIEncoding.ASCII.GetBytes( EncryptionEngine.Key ),  // You can customize these values, but remember the maximum length
            IV = ASCIIEncoding.ASCII.GetBytes( EncryptionEngine.Vector )   // for this encryption method is 8 characters each.
        };

        // Create a cryptographic output stream for the already open file output stream
        using CryptoStream crStream = new( output, cryptic.CreateEncryptor(), CryptoStreamMode.Write );

        // Write the data buffer to the encryption stream, then close everything
        crStream.Write( dta, 0, dta.Length );
        crStream.Close();
        output.Close();
    }

    /// <summary>
    /// Accepts two filenames, one for the encrypted input file and the other for the output file. It
    /// loads the encrypted data into an internal buffer, then writes that buffer to the output file
    /// using an encryption stream based upon an output stream.
    /// </summary>
    /// <remarks>   2024-12-10. </remarks>
    /// <param name="sourceFile">   Source file. </param>
    /// <param name="destination">  Destination file. </param>
    public static void DecryptBinaryFile( string sourceFile, string destination )
    {
        FileInfo info = new( sourceFile );
        using FileStream input = new( sourceFile, FileMode.Open, FileAccess.Read );

        using DESCryptoServiceProvider cryptic = new()
        {
            Key = ASCIIEncoding.ASCII.GetBytes( EncryptionEngine.Key ),
            IV = ASCIIEncoding.ASCII.GetBytes( EncryptionEngine.Vector )
        };

        byte[] dta = new byte[info.Length];

        using CryptoStream crStream = new( input, cryptic.CreateDecryptor(), CryptoStreamMode.Read );
        using BinaryReader reader = new( crStream );
        _ = reader.Read( dta, 0, ( int ) info.Length );
        reader.Close();
        input.Close();

        using FileStream output = new( destination, FileMode.Create, FileAccess.Write );
        output.Write( dta, 0, dta.Length );
        output.Close();
    }

    /// <summary>
    /// Accepts one filename. It opens a StreamReader on a CryptoStream on a FileStream, and uses
    /// that to load a List of strings. Please note that reading (and for that matter, writing) is
    /// accomplished using the normal <see cref="StreamReader"/> methods you already know how to use... ReadLine
    /// and WriteLine work just fine.
    /// </summary>
    /// <remarks>   2024-12-10. </remarks>
    /// <param name="sourceFile">   Source file. </param>
    /// <returns>   The encrypted text file. </returns>
    public static List<string> LoadEncryptedTextFile( string sourceFile )
    {
        List<string> lines = [];

        using DESCryptoServiceProvider cryptic = new()
        {
            Key = ASCIIEncoding.ASCII.GetBytes( EncryptionEngine.Key ),
            IV = ASCIIEncoding.ASCII.GetBytes( EncryptionEngine.Vector )
        };

        // Open the file, decrypt the data stream, and send it to XmlTextReader
        using FileStream input = new( sourceFile, FileMode.Open );
        using CryptoStream cryptoStream = new( input, cryptic.CreateDecryptor(), CryptoStreamMode.Read );
        using StreamReader vStreamReader = new( cryptoStream, Encoding.ASCII );

        while ( !vStreamReader.EndOfStream )
        {
            string line = vStreamReader.ReadLine();
            lines.Add( line );
        }
        vStreamReader.Close();
        cryptoStream.Close();
        input.Close();

        return lines;
    }
}
