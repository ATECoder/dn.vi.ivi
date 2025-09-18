using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.DisplaySubsystemExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Saves the user script in non-volatile memory. </summary>
    /// <remarks>
    /// 2024-09-05. The save needs to be validated once all scripts are saved to save time.
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="scriptName">       Specifies the script to save. </param>
    /// <param name="node">             Specifies the node. </param>
    /// <param name="saveAsByteCode">     Specifies the condition requesting saving the user scripts
    ///                                 source as byte code. </param>
    /// <param name="autoRun">          Specifies the condition indicating if this script is to automatically run upon instrument start. </param>
    public static void SaveUserScript( this DisplaySubsystemBase? displaySubsystem, string scriptName, NodeEntityBase node,
        bool saveAsByteCode, bool autoRun )
    {
        if ( scriptName is null ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );

        Pith.SessionBase session = displaySubsystem.Session;

        session.LastNodeNumber = node.Number;

        session.SetLastAction( $"looking for '{scriptName}' on node {node.Number}" );
        if ( session.IsNil( node.IsController, node.Number, scriptName ) )
            throw new InvalidOperationException( $"{session.ResourceNameNodeCaption} custom firmware script '{scriptName}' not saved on node {node.Number};. " );

        if ( !autoRun )

            // if a script is a boot script, a boot save is required.
            node.BootScriptSaveRequired = true;

        // save validation is done after all scripts are saved.
        // this throws an error on device errors

        if ( saveAsByteCode )
        {
            displaySubsystem.DisplayLine( 2, $"{node.Number}:{scriptName} 2 binary" );
            session.ConvertToByteCode( scriptName, node, displaySubsystem.StatusSubsystem.VersionInfoBase );
        }

        // save the script.
        displaySubsystem.DisplayLine( 2, $"{node.Number}:{scriptName} saving" );
        session.SaveScript( scriptName, node, autoRun );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{session.ResourceNameNodeCaption} saved script '{scriptName}' on node {node.Number};. " );
    }

    /// <summary>   Saves the user script in non-volatile memory. </summary>
    /// <remarks>   2024-09-05.
    /// Save validation is done after all scripts are saved. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="script">           Specifies the <see cref="ScriptEntityBase">script</see>to
    ///                                 delete. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static void SaveUserScript( this DisplaySubsystemBase? displaySubsystem, ScriptEntityBase script )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );

        Pith.SessionBase session = displaySubsystem.Session;

        if ( script.FirmwareScript.ConvertToByteCode )
        {
            if ( script.Node.InstrumentModelFamily is InstrumentModelFamily.K2600 or InstrumentModelFamily.K2600A or InstrumentModelFamily.K3700 )
            {
                // these instruments convert script to byte code by setting the script to nil.
            }
            else
            {
                string functionName = SessionBaseExtensions.FirmwareManager.BinaryScriptsFunctionName;
                ResourceManager.BinaryScriptEntity ??= ResourceManager.DefineBinaryScript(
                    displaySubsystem.StatusSubsystem.VersionInfoBase.FirmwareVersion.ToString(),
                    displaySubsystem.StatusSubsystem.VersionInfoBase,
                    script.Node );

                // check if we need to load the byte code scripts
                session.SetLastAction( $"checking if {functionName} is nil" );

                if ( session.IsNil( script.Node.Number, functionName ) )
                {
                    Stream? stream = ResourceManager.GetResourceStream( ResourceManager.BinaryScriptEntity!.FirmwareScript.DeployFileName );
                    if ( stream is not null )
                    {
                        session.SetLastAction( $"loading {functionName} from embedded resource" );
                        session.LoadEmbeddedResource( ResourceManager.BinaryScriptEntity!.FirmwareScript.DeployFileName );
                    }
                    else
                    {
                        FileInfo? fileInfo = ResourceManager.GetResourceFileInfo( ResourceManager.BinaryScriptEntity!.FirmwareScript.DeployFileName );
                        if ( fileInfo is not null )
                        {
                            session.SetLastAction( $"loading {functionName} from file" );
                            session.LoadScriptFile( ResourceManager.BinaryScriptEntity.Name, fileInfo.FullName );
                        }
                        else
                        {
                            throw new InvalidOperationException(
                                $"{session.ResourceNameNodeCaption} failed loading byte code script conversion function. The function resource {ResourceManager.BinaryScriptEntity!.FirmwareScript.DeployFileName} was not found;. " );
                        }
                    }
                }
            }
        }

        displaySubsystem.SaveUserScript( script.Name, script.Node, script.FirmwareScript.ConvertToByteCode, script.FirmwareScript.IsBootScript );

    }

    /// <summary>   Saves the user scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="scripts">          Specifies the list of scripts to save. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool SaveUserScripts( this DisplaySubsystemBase? displaySubsystem, ScriptEntityCollection scripts )
    {
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );
        if ( scripts.Node is null ) throw new ArgumentNullException( nameof( scripts.Node ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );

        Pith.SessionBase session = displaySubsystem.Session;

        bool resetRequired = false;

        session.LastNodeNumber = scripts.Node.Number;

        session.SetLastAction( "reading the scripts state before saving" );
        scripts.ReadScriptsState( session );

        ScriptEntityBase? bootScript = null;

        foreach ( ScriptEntityBase script in scripts )
        {

            if ( script.FirmwareScript.IsBootScript )
                bootScript = script;

            if ( script.Loaded && !script.Saved )
                displaySubsystem.SaveUserScript( script );
#if false
            if ( script.Loaded )
            {
            session.SetLastAction( $"checking if save is required for '{script.Name}' on node {script.Node.Number}" );
                // TODO: Replace scripts.IsSaveRequired( session, script )  with script.IsSavedRequired()

                bool isSaveRequired = scripts.IsSaveRequired( session, script );
                resetRequired = resetRequired || isSaveRequired;

                if ( isSaveRequired )
                    displaySubsystem.SaveUserScript( script );
            }
#endif
        }

        session.SetLastAction( "reading the scripts state after saving" );
        scripts.ReadScriptsState( session );
        bool success = scripts.AllSaved();
        if ( success )
            scripts.Node.BootScriptSaveRequired = false;

        if ( resetRequired )
        {
            // reset to refresh the instrument display.
            try
            {
                session.SetLastAction( "resetting local TSP node" );
                _ = session.WriteLine( $"localnode.reset() {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} " );

                // query operation completion throw if reply is not 1.
                session.QueryAndThrowIfOperationIncomplete();
            }
            catch ( Pith.NativeException ex )
            {
                _ = session.TraceException( ex );
                success = false;
            }
            catch ( Exception ex )
            {
                _ = session.TraceException( ex );
                success = false;
            }
        }

        if ( success )
        {
            if ( bootScript is null )
                displaySubsystem.DisplayLine( 2, $"All scripts saved on node {scripts.Node.Number}" );
            else
            {
                // query operation completion throw if reply is not 1.
                // this was added because the script did not display the expected screen
                session.QueryAndThrowIfOperationIncomplete();

                session.RunScript( bootScript );

                // allow the boot script time to run.
                _ = SessionBase.AsyncDelay( TimeSpan.FromSeconds( 2 ) );
            }
        }

        else
            displaySubsystem.DisplayLine( 2, $"Failed saving scripts on node {scripts.Node.Number}" );

        return success;
    }

    /// <summary>   Saves all users scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem">     A reference to a
    ///                                     <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                     subsystem</see>. </param>
    /// <param name="scriptsCollection">    Collection of <see cref="ScriptEntityCollection"/>. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool SaveUserScripts( this DisplaySubsystemBase? displaySubsystem, ICollection<ScriptEntityCollection> scriptsCollection )
    {
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );
        bool success = true;
        foreach ( ScriptEntityCollection scripts in scriptsCollection )
            success &= displaySubsystem.SaveUserScripts( scripts );

        return success;
    }
}
