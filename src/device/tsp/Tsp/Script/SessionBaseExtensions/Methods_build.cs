namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   A ScriptInfoBase extension method that trim compress. </summary>
    /// <remarks>   2025-04-16. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="folderPath">   Full pathname of the folder file. </param>
    public static void TrimCompress( this ScriptInfoBase scriptInfo, string folderPath )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        string fromFilePath = Path.Combine( folderPath, scriptInfo.BuiltFileName );
        string trimmedFilePath = Path.Combine( folderPath, scriptInfo.TrimmedFileName );
        string compressedFilePath = trimmedFilePath + "c";
        if ( System.IO.File.Exists( fromFilePath ) )
        {
            SessionBaseExtensionMethods.TraceLastAction( $"Trimming script file '{fromFilePath}' to '{trimmedFilePath}'" );
            fromFilePath.TrimScript( trimmedFilePath, true );

            SessionBaseExtensionMethods.TraceLastAction( $"Compressing trimmed script file to '{compressedFilePath}'" );
            trimmedFilePath.CompressScriptFile( compressedFilePath, overWrite: true );
        }
        else
            throw new FileNotFoundException( fromFilePath );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that trims, loads, optionally converts to
    /// binary, and compresses the script to the deploy file.
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
    public static void TrimCompressLoadConvertExport( this Pith.SessionBase session, ScriptInfoBase scriptInfo, string folderPath )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        string fromFilePath = Path.Combine( folderPath, scriptInfo.BuiltFileName );
        string trimmedFilePath = Path.Combine( folderPath, scriptInfo.TrimmedFileName );
        string deployFilePath = Path.Combine( folderPath, scriptInfo.DeployFileName );
        if ( System.IO.File.Exists( fromFilePath ) )
        {
            SessionBaseExtensionMethods.TraceLastAction( $"Trimming script file '{fromFilePath}' to '{trimmedFilePath}'" );
            fromFilePath.TrimScript( trimmedFilePath, true );

            if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFileFormats.Compressed )
                && !scriptInfo.DeployFileFormat.HasFlag( ScriptFileFormats.Binary ) )
            {
                SessionBaseExtensionMethods.TraceLastAction( $"Compressing trimmed script file to '{deployFilePath}'" );
                trimmedFilePath.CompressScriptFile( deployFilePath, overWrite: true );
            }
        }
        else
            throw new FileNotFoundException( fromFilePath );

        // delete the script if it exists.
        session.DeleteScript( scriptInfo.Title );

        SessionBaseExtensionMethods.TraceLastAction( $"Importing script from trimmed '{trimmedFilePath}' file" );
        session.ImportScript( scriptInfo.Title, trimmedFilePath, TimeSpan.Zero );

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        // convert the script to binary.
        session.ConvertToBinary( scriptInfo.Title );

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFileFormats.Binary ) )
        {
            if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFileFormats.Compressed ) )
            {
                // export and compress the script.
                SessionBaseExtensionMethods.TraceLastAction( $"Fetching and compressing binary script to '{deployFilePath}'" );
                session.CompressScript( scriptInfo.Title, deployFilePath, true );
            }
            else
            {
                // if naked binary, export tot he deploy file name.
                SessionBaseExtensionMethods.TraceLastAction( $"Fetching binary script to '{deployFilePath}'" );
                session.ExportScript( scriptInfo.Title, deployFilePath, true );
            }
        }
    }
}
