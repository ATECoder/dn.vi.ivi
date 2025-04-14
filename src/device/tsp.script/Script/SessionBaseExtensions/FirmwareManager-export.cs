using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Fetch the script source and write it to file. </summary>
    /// <remarks>   2024-09-05. Requires the ISR support scripts. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="IOException">              Thrown when an IO failure occurred. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="folderPath">       Specifies the script file folder. </param>
    /// <param name="script">           Specifies the script. </param>
    /// <param name="compress">         Specifies the compression condition. True to compress the
    ///                                 source before saving. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0270:Use coalesce expression", Justification = "<Pending>" )]
    public static bool FetchScriptSaveToFile( this Pith.SessionBase? session, string folderPath, ScriptEntityBase script, bool compress )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( folderPath is null ) throw new ArgumentNullException( nameof( folderPath ) );
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );

        try
        {
            string scriptSource = session.FetchScriptSource( script );

            if ( string.IsNullOrWhiteSpace( scriptSource ) )
                throw new cc.isr.VI.Pith.NativeException( $"Failed fetching script source for {script.FirmwareScript.Name}" );

            bool isBinary = session.IsBinaryScript( scriptSource );
            ScriptFileFormats fileFormat = FirmwareScriptBase.BuildScriptFileFormat( isBinary, compress );
            string fileTitle = FirmwareScriptBase.BuildScriptFileTitle( script.FirmwareScript.Name, fileFormat,
                 script.FirmwareScript.FirmwareVersion, script.FirmwareScript.ModelMask, script.FirmwareScript.ModelMajorVersion );
            if ( !script.Node.IsController )
                fileTitle = $"{fileTitle}.node{script.Node.Number}";

            string fileExtension = FirmwareScriptBase.SelectScriptFileExtension( fileFormat );
            string fileName = $"{fileTitle}{fileExtension}";
            string filePath = Path.Combine( folderPath, $"{fileName}{fileExtension}" );

            using StreamWriter? scriptFile = new( filePath );

            if ( scriptFile is null )
                // now report the error to the calling module
                throw new System.IO.IOException( $"Failed opening file '{filePath}' for output';." );


            if ( compress )
                scriptFile.WriteLine( Tsp.Script.ScriptCompressor.Compress( scriptSource ) );
            else
                scriptFile.WriteLine( scriptSource );

            return true;
        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary>   Fetch the script source and write it to file. </summary>
    /// <remarks>   2024-09-05. Requires the ISR support scripts. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="IOException">              Thrown when an I/O failure occurred. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="filePath">     full path name of the file. </param>
    /// <param name="compress">     Specifies the compression condition. True to compress the source
    ///                             before saving to file. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool FetchScriptSaveToFile( this Pith.SessionBase? session, string scriptName, string filePath, bool compress )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( filePath is null ) throw new ArgumentNullException( nameof( filePath ) );

        try
        {
            using StreamWriter? scriptFile = new StreamWriter( filePath )
                ?? throw new System.IO.IOException( "Failed opening TSP Script File for output'" + filePath + "'." );

            string scriptSource = session.FetchScriptSource( scriptName, false, TimeSpan.FromMilliseconds( 500 ) );

            // replace LF with CRLF for Windows EOL format
            scriptSource = scriptSource.Replace( "\n", "\r\n" );

            if ( string.IsNullOrWhiteSpace( scriptSource ) )
                throw new cc.isr.VI.Pith.NativeException( $"Failed fetching script source for {scriptName}" );

            if ( compress )
                scriptFile.WriteLine( Tsp.Script.ScriptCompressor.Compress( scriptSource ) );
            else
                scriptFile.WriteLine( scriptSource );

            return true;
        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }


}
