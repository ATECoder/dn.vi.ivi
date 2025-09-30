using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.DisplaySubsystemExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Deletes the user script. </summary>
    /// <remarks>   2024-07-09. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="displaySubsystem">         A reference to a
    ///                                         <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                         subsystem</see>. </param>
    /// <param name="script">                   Specifies the <see cref="ScriptEntityBase">script</see>
    ///                                               to delete. </param>
    /// <param name="scriptNames">              List of names of the scripts. </param>
    /// <param name="refreshScriptsCatalog">    Refresh catalog before checking if script exists. </param>
    /// <returns>
    /// Returns <c>true</c> if script was deleted or did not exit. Returns <c>false</c> if deletion
    /// failed.
    /// </returns>
    public static bool DeleteEmbeddedUserScript( this DisplaySubsystemBase? displaySubsystem, ScriptEntityBase? script, string scriptNames, bool refreshScriptsCatalog )
    {
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );

        SessionBase session = displaySubsystem.Session;

        if ( refreshScriptsCatalog )
            scriptNames = session.FetchEmbeddedScriptsNames( script.Node ).EmbeddedScripts;

        if ( session.IsNil( script.Node.IsController, script.Node.Number, script.Name ) )
        {
            // todo:  check but ignore error if nil
            // _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Instrument '{this.Session.ResourceNameCaption}' had error(s) looking for script '{script.Name}' on node {script.Node.Number};. Nothing to do." );

            script.Loaded = false;
            script.Embedded = false;
            script.Activated = false;
            script.IsDeleted = true;
            return true;
        }

        displaySubsystem.DisplayLine( 2, $"Deleting {script.Node.Number}:{script.Name}" );

        if ( script.Node.IsController )
            session.DeleteEmbeddedScript( script.Name );
        else
            session.DeleteEmbeddedScript( script.Node.Number, script.Name );

        bool deleted = true;
#if false
        bool deleted = script.Node.IsController
                ? FirmwareScript.ScriptNameExists( scriptNames, script.Name )
                    ? session.DeleteEmbeddedScript( script.Name )
                    : session.NillObject( script.Name )
                : FirmwareScript.ScriptNameExists( scriptNames, script.Name )
                    ? session.DeleteEmbeddedScript( script.Node.Number, script.Name )
                    : session.NillObject( script.Node.Number, script.Name );

        // todo: check status byte for errors.

#endif
        if ( !deleted )
            throw new InvalidOperationException( $"{session.ResourceNameNodeCaption} script '{script.Name}' still exists after nil." );


        // todo: check status byte for errors.

        // do a garbage collection
        if ( !session.CollectGarbageQueryComplete( script.Node.Number ) )
            _ = session.TraceWarning( message: $"operation incomplete (reply not '1') after deleting embedded script {script.Name} on node {script.Node.Number}" );

        script.Loaded = false;
        script.Embedded = false;
        script.Activated = false;
        script.IsDeleted = true;
        return true;
    }

    /// <summary>
    /// Deletes user scripts that are outdated or where a deletion is set for the script.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem">         A reference to a
    ///                                         <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                         subsystem</see>. </param>
    /// <param name="scripts">                  Specifies the list of the scripts to be deleted. </param>
    /// <param name="embeddedScripts">             The embedded scripts. </param>
    /// <param name="refreshStateRequired">     True if refresh state required. </param>
    /// <param name="refreshScriptsCatalog">    Refresh catalog before checking if script exists. </param>
    /// <param name="deleteOutdatedOnly">       if set to <c>true</c> deletes only if scripts is out
    ///                                         of date. </param>
    /// <returns>   <c>true</c> if success, <c>false</c> otherwise. </returns>
    public static bool DeleteEmbeddedUserScripts( this DisplaySubsystemBase displaySubsystem, ScriptEntityCollection scripts,
        string embeddedScripts, bool refreshStateRequired, bool refreshScriptsCatalog, bool deleteOutdatedOnly )
    {
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );
        if ( scripts.Node is null ) throw new ArgumentNullException( nameof( scripts.Node ) );

        SessionBase session = displaySubsystem.Session;

        bool success = true;

        // <c>true</c> if any delete action was executed
        bool scriptsDeleted = false;
        if ( refreshScriptsCatalog )
            embeddedScripts = session.FetchEmbeddedScriptsNames( scripts.Node ).EmbeddedScripts;

        if ( refreshStateRequired )
            scripts.ReadScriptsState( session );

        // deletion of scripts must be done in reverse order.
        for ( int i = scripts.Count - 1; i >= 0; i -= 1 )
        {
            ScriptEntityBase script = scripts[i];
            try
            {
                if ( !script.IsDeleted && (script.RequiresDeletion || !deleteOutdatedOnly
                    || scripts.IsDeleteUserScriptRequired( !deleteOutdatedOnly )) )
                {
                    if ( displaySubsystem.DeleteEmbeddedUserScript( script, embeddedScripts, false ) )
                    {
                        // mark that scripts were deleted, i.e., that any script was deleted
                        // or if a script that existed no longer exists.
                        scriptsDeleted = true;
                    }
                    else
                    {
                        _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"failed deleting script '{script.Name}' from node {script.Node.Number};. " );
                        success = false;
                    }
                }
            }
            catch ( Exception ex )
            {
                try
                {
                    success = success && session.IsNil( script.Name );
                    _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, $"Exception occurred deleting firmware {script.Name} from node {script.Node.Number};. " );
                }
                catch
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogException( ex,
                        $"Exception occurred checking existence after attempting deletion of firmware {script.Name} from node {script.Node.Number};. " );
                    success = false;
                }
            }
        }

        if ( scriptsDeleted )
        {
            // reset to refresh the instrument display.
            try
            {
                session.SetLastAction( "resetting local node" );
                _ = session.WriteLine( Syntax.Tsp.LocalNode.ResetCommand );
                _ = session.TraceInformation();
                success = !session.IsErrorBitSet( session.TraceDeviceExceptionIfError().StatusByte );
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

        return success;
    }

    /// <summary>   Deletes user scripts from the remote instrument. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem">         A reference to a
    ///                                         <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                         subsystem</see>. </param>
    /// <param name="nodeScripts">              The node scripts. </param>
    /// <param name="refreshStateRequired">     True if refresh state required. </param>
    /// <param name="refreshScriptsCatalog">    Refresh catalog before checking if script exists. </param>
    /// <param name="deleteOutdatedOnly">       if set to <c>true</c> deletes only if scripts is out
    ///                                         of date. </param>
    /// <returns>   <c>true</c> if success, <c>false</c> otherwise. </returns>
    public static bool DeleteEmbeddedUserScripts( this DisplaySubsystemBase displaySubsystem, Dictionary<int, ScriptEntityCollection> nodeScripts,
        bool refreshStateRequired, bool refreshScriptsCatalog, bool deleteOutdatedOnly )
    {
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( nodeScripts is null ) throw new ArgumentNullException( nameof( nodeScripts ) );

        bool success = true;
        foreach ( ScriptEntityCollection scripts in nodeScripts.Values )
            success &= displaySubsystem.DeleteEmbeddedUserScripts( scripts, scripts.EmbeddedScriptNames, refreshStateRequired, refreshScriptsCatalog, deleteOutdatedOnly );
        return success;
    }
}
