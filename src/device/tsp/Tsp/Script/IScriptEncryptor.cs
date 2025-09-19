namespace cc.isr.VI.Tsp.Script;
/// <summary>   Interface for script encryptor. </summary>
/// <remarks>   2025-09-19. </remarks>
public interface IScriptEncryptor
{
    /// <summary>   Encrypts a binary file. </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <param name="sourceFile">   Source file. </param>
    /// <param name="destination">  Destination for the. </param>
    public void EncryptBinaryFile( string sourceFile, string destination );

    /// <summary>   Decrypt binary file. </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <param name="sourceFile">   Source file. </param>
    /// <param name="destination">  Destination for the. </param>
    public void DecryptBinaryFile( string sourceFile, string destination );

    /// <summary>   Loads encrypted text file. </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <param name="sourceFile">   Source file. </param>
    /// <returns>   The encrypted text file. </returns>
    public List<string> LoadEncryptedTextFile( string sourceFile );

    /// <summary>   Generates a stream from string. </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <param name="s">    The string. </param>
    /// <returns>   The stream from string. </returns>
    public Stream GenerateStreamFromString( string s );

    /// <summary>   Decrypts. </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <param name="encrypted">    The encrypted. </param>
    /// <returns>   A string. </returns>
    public string Decrypt( string encrypted );
}
