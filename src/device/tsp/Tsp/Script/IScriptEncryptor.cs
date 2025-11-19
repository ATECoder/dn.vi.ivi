// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.Script;
/// <summary>   Interface for script encryptor. </summary>
/// <remarks>   2025-09-19. </remarks>
public interface IScriptEncryptor
{
    /// <summary>   Decrypts the provided Base 64 cipher text. </summary>
    /// <remarks>   2025-05-24. </remarks>
    /// <param name="cipherText">   The cipher text. </param>
    /// <returns>   A decrypted text. </returns>
    public string DecryptFromBase64( string cipherText );

    /// <summary>   Encrypt to base 64. </summary>
    /// <param name="plainText">  The plain text to encrypt to Base 64 format. </param>
    /// <returns>   The cipher text in base 64 format. </returns>
    public string EncryptToBase64( string plainText );

    /// <summary>   Query if 'contents' is encrypted. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="contents"> The content to be checked for encryption. </param>
    /// <returns>   True if encrypted, false if not. </returns>
    public bool IsEncrypted( string contents );

}
