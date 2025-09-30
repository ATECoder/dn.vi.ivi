using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.DisplaySubsystemExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Fetch each script source and save the script source to a file under the specified folder. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="folderPath">       Specifies the script file folder. </param>
    /// <param name="scripts">          Specifies the scripts. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool FetchScriptsSaveToFiles( this DisplaySubsystemBase? displaySubsystem, string folderPath, ScriptEntityCollection scripts )
    {
        if ( folderPath is null ) throw new ArgumentNullException( nameof( folderPath ) );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );

        bool success = true;
        foreach ( ScriptEntityBase script in scripts )
        {
            // do not save the script if is has no file scriptName. Future upgrade might suggest adding a file name to the boot script.
            if ( !(string.IsNullOrWhiteSpace( script.FirmwareScript.Name ) || script.FirmwareScript.ExportedToFile) )
            {
                displaySubsystem.DisplayLine( 2, "Writing {0}:{1}", script.Node.ModelNumber, script.Name );
                if ( displaySubsystem.Session.FetchScriptSaveToFile( folderPath, script, (script.FirmwareScript.DeployFileFormat & ScriptFileFormats.Compressed) != 0 ) )
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


    /// <summary>   Fetch each script source and save the script source to a file under the specified folder. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="folderPath">       Specifies the script file folder. </param>
    /// <param name="nodeScripts">      The node scripts. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool FetchScriptsSaveToFiles( this DisplaySubsystemBase? displaySubsystem, string folderPath, ICollection<ScriptEntityCollection> nodeScripts )
    {
        if ( folderPath is null ) throw new ArgumentNullException( nameof( folderPath ) );
        if ( nodeScripts is null ) throw new ArgumentNullException( nameof( nodeScripts ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );

        bool success = true;
        foreach ( ScriptEntityCollection scripts in nodeScripts )
            success &= FetchScriptsSaveToFiles( displaySubsystem, folderPath, scripts );

        return success;
    }
}
