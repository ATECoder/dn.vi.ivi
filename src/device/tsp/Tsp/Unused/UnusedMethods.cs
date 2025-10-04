using cc.isr.VI.Tsp.Script.SessionBaseExtensions;
using cc.isr.VI.Tsp.Script;

namespace cc.isr.VI.Tsp.Unused;

internal static class UnusedMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads the script from the deploy file
    /// and embeds it to non-volatile memory.
    /// </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="folderPath">   Full pathname of the folder file. </param>
    /// <param name="lineDelay">    The line delay. </param>
    public static void ImportEmbed( this Pith.SessionBase session, ScriptInfo scriptInfo, string folderPath, TimeSpan lineDelay )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        // set the deploy file path.
        string deployFilePath = Path.Combine( folderPath, scriptInfo.DeployFileName );

        // delete the script if it exists.
        session.DeleteScript( scriptInfo.Title );

        SessionBaseExtensionMethods.TraceLastAction( $"\r\n\tImporting script from {scriptInfo.DeployFileFormat} '{deployFilePath}' file" );
        session.ImportScript( scriptInfo.Compressor, scriptInfo.Encryptor, scriptInfo.Title, deployFilePath, lineDelay );

        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        session.EmbedScript( scriptInfo.Title, scriptInfo.EmbedAsByteCode, false );

        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );
    }

    /// <summary>
    /// A <see cref="string"/> extension method that compress a script file to a destination file.
    /// </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="inputFilePath">    The full path name of the input file to act on. </param>
    /// <param name="outputFilePath">   Full pathname of the output file. </param>
    /// <param name="compressor">       The compressor. </param>
    /// <param name="overWrite">        (Optional) [false] True to over write. </param>
    public static void CompressScriptFile( this string inputFilePath, string outputFilePath, IScriptCompressor compressor, bool overWrite = false )
    {
        if ( string.IsNullOrWhiteSpace( inputFilePath ) )
            throw new ArgumentNullException( nameof( inputFilePath ) );
        if ( string.IsNullOrWhiteSpace( outputFilePath ) )
            throw new ArgumentNullException( nameof( outputFilePath ) );

        if ( !overWrite && System.IO.File.Exists( outputFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{outputFilePath}' exists." );

        // compress and export the source to the file as is.
        System.IO.File.WriteAllText( outputFilePath, compressor.CompressToBase64( System.IO.File.ReadAllText( inputFilePath ) ), System.Text.Encoding.Default );
    }
}
