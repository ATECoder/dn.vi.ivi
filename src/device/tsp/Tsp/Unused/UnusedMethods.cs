using cc.isr.VI.Tsp.Script.SessionBaseExtensions;
using cc.isr.VI.Tsp.Script;
using cc.isr.VI.Tsp.Script.LineEndingExtensions;

namespace cc.isr.VI.Tsp.Unused;
internal static class UnusedMethods
{

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads the script from the deploy file
    /// and saves it to non-volatile memory.
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
    public static void ImportSaveToNvm( this Pith.SessionBase session, ScriptInfo scriptInfo, string folderPath, TimeSpan lineDelay )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        // set the deploy file path.
        string deployFilePath = Path.Combine( folderPath, scriptInfo.DeployFileName );

        // delete the script if it exists.
        session.DeleteScript( scriptInfo.Title );

        SessionBaseExtensionMethods.TraceLastAction( $"\r\n\tImporting script from {scriptInfo.DeployFileFormat} '{deployFilePath}' file" );
        session.ImportScript( scriptInfo.Title, deployFilePath, lineDelay );

        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        session.SaveScript( scriptInfo.Title );

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
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="fromFilePath"> The source file path. </param>
    /// <param name="toFilePath">   the destination file path. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void CompressScriptFile( this string fromFilePath, string toFilePath, bool overWrite = false, bool validate = true )
    {
        if ( string.IsNullOrWhiteSpace( fromFilePath ) )
            throw new ArgumentNullException( nameof( fromFilePath ) );
        if ( string.IsNullOrWhiteSpace( toFilePath ) )
            throw new ArgumentNullException( nameof( toFilePath ) );

        if ( !overWrite && System.IO.File.Exists( toFilePath ) )
            throw new InvalidOperationException( $"The script source cannot be exported because the file '{toFilePath}' exists." );

        string scriptSource = LineEndingExtensionMethods.TerminateFileLines( fromFilePath, validate );

        // compress and export the source to the file as is.
        System.IO.File.WriteAllText( toFilePath, ScriptCompressor.Compress( scriptSource ), System.Text.Encoding.UTF8 );
    }

}
