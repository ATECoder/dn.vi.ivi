namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that trim compress load convert export.
    /// </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="folderPath">   Full pathname of the folder file. </param>
    public static void TrimCompressLoadConvertExport( this Pith.SessionBase session, IScriptInfo scriptInfo, string folderPath )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        string fromFilePath = Path.Combine( folderPath, scriptInfo.BuiltFileName );
        string trimmedFilePath = Path.Combine( folderPath, scriptInfo.TrimmedFileName );
        string compressedFilePath = Path.Combine( folderPath, $"{scriptInfo.TrimmedFileName}{ScriptInfoBase.SelectScriptFileExtension( ScriptFileFormats.Compressed )}" );
        if ( System.IO.File.Exists( fromFilePath ) )
        {
            SessionBaseExtensionMethods.TraceLastAction( $"Trimming script file '{fromFilePath}' to '{trimmedFilePath}'" );
            fromFilePath.TrimScript( trimmedFilePath, true );

            SessionBaseExtensionMethods.TraceLastAction( $"Compressing trimmed script file to '{compressedFilePath}'" );
            trimmedFilePath.CompressScriptFile( compressedFilePath, overWrite: true );
        }
        else
            throw new FileNotFoundException( fromFilePath );

        // delete the script if it exists.
        session.DeleteScript( scriptInfo.Title );

        SessionBaseExtensionMethods.TraceLastAction( $"Importing script from trimmed '{trimmedFilePath}' file" );
        session.ImportScript( scriptInfo.Title, trimmedFilePath );

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        // convert the script to binary.
        session.ConvertToBinary( scriptInfo.Title );

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        string deployFileName = $"{scriptInfo.DeployFileTitle}{ScriptInfoBase.SelectScriptFileExtension( ScriptFileFormats.Binary )}";
        string binaryFilePath = Path.Combine( folderPath, deployFileName );

        session.ExportScript( scriptInfo.Title, binaryFilePath, true );

        deployFileName = $"{scriptInfo.DeployFileTitle}{ScriptInfoBase.SelectScriptFileExtension( ScriptFileFormats.Compressed | ScriptFileFormats.Binary )}";
        compressedFilePath = Path.Combine( folderPath, deployFileName );

        SessionBaseExtensionMethods.TraceLastAction( $"Compressing binary script file to '{compressedFilePath}'" );
        binaryFilePath.CompressScriptFile( compressedFilePath, overWrite: true );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads the scripts and saves it to non-
    /// volatile memory.
    /// </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptInfo">       Information describing the script. </param>
    /// <param name="folderPath">       Full pathname of the folder file. </param>
    public static void LoadSaveToNvm( this Pith.SessionBase session, IScriptInfo scriptInfo, string folderPath )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        string fromFilePath = Path.Combine( folderPath, scriptInfo.DeployFileName );

        // delete the script if it exists.
        session.DeleteScript( scriptInfo.Title );

        SessionBaseExtensionMethods.TraceLastAction( $"Importing script from {scriptInfo.DeployFileFormat} '{fromFilePath}' file" );
        session.ImportScript( scriptInfo.Title, fromFilePath );

        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        session.SaveScript( scriptInfo.Title );

        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );
    }
}
