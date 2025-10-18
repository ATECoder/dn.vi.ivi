using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.DisplaySubsystemExtensions;

public static partial class FirmwareManager
{
    /// <summary>
    /// Fetch each script source and export the script source to a file under the specified folder.
    /// </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="folderPath">       Specifies the script file folder. </param>
    /// <param name="scripts">          Specifies the scripts. </param>
    /// <param name="compressor">       The compressor. </param>
    /// <param name="encryptor">        The encryptor. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool FetchScriptsExportToFiles( this DisplaySubsystemBase? displaySubsystem,
        string folderPath, ScriptEntityCollection scripts,
        IScriptCompressor compressor, IScriptEncryptor encryptor )
    {
        if ( folderPath is null ) throw new ArgumentNullException( nameof( folderPath ) );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );

        bool success = true;
        foreach ( ScriptEntityBase script in scripts )
        {
            // do not embed the script if is has no file scriptName. Future upgrade might suggest adding a file name to the boot script.
            if ( !(string.IsNullOrWhiteSpace( script.FirmwareScript.Name ) || script.FirmwareScript.ExportedToFile) )
            {
                displaySubsystem.DisplayLine( 2, "Writing {0}:{1}", script.Node.ModelNumber, script.Name );
                if ( displaySubsystem.Session.FetchScriptExportToFile( folderPath, script,
                    (script.FirmwareScript.DeployFileFormat & ScriptFormats.Compressed) != 0, compressor, encryptor ) )
                {
                    script.FirmwareScript.ExportedToFile = true;
                    success = success && true;
                }
                else
                    success = false;
            }
        }

        return success;
    }


    /// <summary>
    /// Fetch each script source and export the script source to a file under the specified folder.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="folderPath">       Specifies the script file folder. </param>
    /// <param name="nodeScripts">      The node scripts. </param>
    /// <param name="compressor">       The compressor. </param>
    /// <param name="encryptor">        The encryptor. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool FetchScriptsExportToFiles( this DisplaySubsystemBase? displaySubsystem, string folderPath,
        ICollection<ScriptEntityCollection> nodeScripts, IScriptCompressor compressor, IScriptEncryptor encryptor )
    {
        if ( compressor is null ) throw new ArgumentNullException( nameof( compressor ) );
        if ( encryptor is null ) throw new ArgumentNullException( nameof( encryptor ) );
        if ( folderPath is null ) throw new ArgumentNullException( nameof( folderPath ) );
        if ( nodeScripts is null ) throw new ArgumentNullException( nameof( nodeScripts ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );

        bool success = true;
        foreach ( ScriptEntityCollection scripts in nodeScripts )
            success &= FetchScriptsExportToFiles( displaySubsystem, folderPath, scripts, compressor, encryptor );

        return success;
    }
}
