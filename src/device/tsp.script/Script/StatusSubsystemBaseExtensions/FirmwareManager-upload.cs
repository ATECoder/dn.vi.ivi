using System.Diagnostics;
using cc.isr.Std.ExceptionExtensions;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.StatusSubsystemBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Create a new script on the controller node and copy it tot he remote node. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="statusSubsystem">      A reference to a
    ///                                     <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                     subsystem</see>. </param>
    /// <param name="script">               . </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool UploadScript( this StatusSubsystemBase? statusSubsystem, ScriptEntityBase? script )
    {
        if ( statusSubsystem is null ) throw new ArgumentNullException( nameof( statusSubsystem ) );
        if ( statusSubsystem.Session is null ) throw new ArgumentNullException( nameof( statusSubsystem.Session ) );
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );
        if ( script.FirmwareScript is null ) throw new ArgumentNullException( nameof( script.FirmwareScript ) );
        if ( string.IsNullOrWhiteSpace( script.FirmwareScript.TopNamespace ) ) throw new ArgumentNullException( nameof( script.FirmwareScript.TopNamespace ) );

        Pith.SessionBase session = statusSubsystem.Session;

        session.LastNodeNumber = script.Node.Number;
        session.SetLastAction( $"loading script '{script.Name}'" );
        bool affirmative = true;
        if ( script.FirmwareScript.IsBinaryScript || (!session.IsNil( script.Name ) && session.IsBinaryScript( script ).GetValueOrDefault( false )) )
        {
            script.Loaded = false;
            script.Saved = false;
            script.Activated = false;
            string tempName = "isr_temp";
            try
            {
                session.SetLastAction( "enabling wait completion" );
                session.EnableServiceRequestOnOperationCompletion();

                // clear the data queue.
                session.SetLastAction( "clearing data queue" );
                session.ClearDataQueueWaitComplete( script.Node );
                string scriptLoaderScript = script.FirmwareScript.BuildScriptLoaderScript( tempName, script.Node.Number );

                // load and ran the temporary script.
                session.SetLastAction( "loading temporary script" );

                // replaced by the code below
                // session.LoadString( scriptLoaderScript );

                scriptLoaderScript = FirmwareScriptBase.BuildLoadStringSyntax( scriptLoaderScript );
                session.WriteLines( scriptLoaderScript, Environment.NewLine, FirmwareScriptBase.WriteLinesDelay );
                _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
                _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.OperationCompleteCommand );

                // await completion (the wait complete command is included in the loader script)
                session.SetLastAction( "awaiting completion" );
                (_, ServiceRequests statusByte, _) = session.AwaitOperationCompletion( TimeSpan.FromMilliseconds( 1000 ) );

                _ = session.TraceInformation();
                _ = session.TraceDeviceExceptionIfError( statusByte );

            }
            catch ( Pith.NativeException ex )
            {
                _ = session.TraceException( ex );
                affirmative = false;
            }
            catch ( Exception ex )
            {
                _ = session.TraceException( ex );
                affirmative = false;
            }

            if ( affirmative )
            {
                session.SetLastAction( "removing the temporary script" );
                try
                {
                    // remove the temporary script if there.
                    session.NillScript( tempName );

                    _ = session.TraceInformation();
                    _ = session.TraceDeviceExceptionIfError();
                }
                catch ( Pith.NativeException ex )
                {
                    _ = session.TraceException( ex );
                }
                catch ( Exception ex )
                {
                    _ = ex.AddExceptionData();
                    _ = session.TraceException( ex );
                }
            }
        }

        // if scripts is already stored in the controller node in non-binary format, just
        // copy it (upload) from the controller to the remote node.
        else if ( !UploadScriptCopy( statusSubsystem, script ) )
        {
            script.Saved = false;
            script.Activated = false;
            affirmative = false;
        }

        if ( !affirmative )
            return affirmative;

        // verify that the script was loaded.
        session.SetLastAction( $"verifying script '{script.Name}' loaded on node {script.Node.Number}" );
        session.LastNodeNumber = script.Node.Number;

        // check if the script short name exists.
        if ( session.WaitNotNil( script.Node.Number, script.Name, TimeSpan.FromMilliseconds( 1000 ) ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{session.ResourceNameNodeCaption} loaded script '{script.Name}' to node {script.Node.Number};. " );
            script.Loaded = true;
        }
        else
        {
            // if script short name not found, check to see if the script long name is found.
            string fullName = "script.user.scripts." + script.Name;
            session.SetLastAction( $"awaiting '{fullName}' is not nil" );
            script.Loaded = session.WaitNotNil( script.Node.Number, fullName, TimeSpan.FromMilliseconds( 1000 ) );

            if ( script.Loaded )
            {
                // in design mode, assert a problem.
                // 3783. this was asserted the first time after upgrading the 3706 to firmware 1.32.a
                Debug.Assert( !Debugger.IsAttached, "Failed setting script short scriptName" );

                // assign the new script name
                session.SetLastAction( $"uploading script '{script.Name}' on node {script.Node.Number}" );
                _ = session.ExecuteCommandQueryComplete( script.Node.Number, $"{script.Name} = {fullName} waitcomplete() " );

                // read query reply and throw if reply is not 1.
                session.ReadAndThrowIfOperationIncomplete();

                // throw if device errors
                session.ThrowDeviceExceptionIfError();

                session.SetLastAction( $"awaiting '{script.Name}' is not nil" );
                script.Loaded = session.WaitNotNil( script.Node.Number, script.Name, TimeSpan.FromMilliseconds( 1000 ) );
                if ( !script.Loaded )
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed referencing script '{script.Name}' using '{fullName}' on node {script.Node.Number};. --new script not found on the remote node.{Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );
            }
            else
                // if both long and short names not found, report failure.
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed uploading script '{fullName}' to node {script.Node.Number} from script '{script.Name}';. --new script not found on the remote node.{Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );
        }

        session.LastNodeNumber = default;
        return script.Loaded;
    }

    /// <summary>
    /// Copy a script from the controller node to a remote node using the same script name on the
    /// remote node. Does not require having the ISR Support script on the controller node.
    /// </summary>
    /// <remarks>
    /// For binary scripts, the controller and remote nodes must be binary compatible.
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="statusSubsystem">              A reference to a
    ///                                             <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                             subsystem</see>. </param>
    /// <param name="script">                       . </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool UploadScriptCopy( this Tsp.StatusSubsystemBase? statusSubsystem, ScriptEntityBase? script )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.FirmwareScript is null ) throw new ArgumentNullException( nameof( script.FirmwareScript ) );
        if ( string.IsNullOrWhiteSpace( script.FirmwareScript.TopNamespace ) ) throw new ArgumentNullException( nameof( script.FirmwareScript.TopNamespace ) );
        if ( statusSubsystem is null ) throw new ArgumentNullException( nameof( statusSubsystem ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );
        if ( statusSubsystem.Session is null ) throw new ArgumentNullException( nameof( statusSubsystem.Session ) );

        // string[] authorNamespaces = this.ScriptEntities.SelectSupportScript( this.LinkSubsystem.ControllerNode )!.FirmwareScript.Namespaces;
        Pith.SessionBase session = statusSubsystem.Session;

        session.LastNodeNumber = script.Node.Number;
        // NOTE: WAIT COMPLETE is required on the system before a wait complete is tested on the node
        // otherwise getting error 1251.

        session.SetLastAction( $"checking if script '{script.Name}' was activated" );
        System.Text.StringBuilder commands = new( 1024 );
        if ( session.IsNil( script.FirmwareScript.TopNamespace ) )
        {
            // loads and runs the specified script.
            _ = commands.Append( $"node[{script.Node.Number}].dataqueue.add({script.Name}.source) waitcomplete()" );
            _ = commands.AppendLine();
            _ = commands.Append( $"node[{script.Node.Number}].execute('waitcomplete() {script.Name}=script.new(dataqueue.next(),[[{script.Name}]])')" );
        }
        else
        {
            _ = commands.Append( $"isr.script.uploadScript(node[{script.Node.Number}],{script.Name})" );
        }

        _ = commands.AppendLine();
        _ = commands.AppendLine( "waitcomplete(0)" );
        bool affirmative = false;
        try
        {
            session.SetLastAction( $"enabling service request on completion" );
            session.EnableServiceRequestOnOperationCompletion();
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

            session.SetLastAction( $"loading script {script.Name}" );

            // replaced by the code blow.
            // session.LoadString( commands.ToString() );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

            session.WriteLines( commands.ToString(), Environment.NewLine, FirmwareScriptBase.WriteLinesDelay );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
            _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.OperationCompleteCommand );

            session.SetLastAction( $"awaiting completion" );
            ServiceRequests statusByte = session.AwaitOperationCompletion( TimeSpan.FromMilliseconds( 1000 ) ).Status;

            _ = session.TraceInformation();
            _ = session.TraceDeviceExceptionIfError( statusByte );
        }
        catch ( Pith.NativeException ex )
        {
            _ = session.TraceException( ex );
        }
        catch ( Exception ex )
        {
            _ = session.TraceException( ex );
        }

        return affirmative;
    }

    /// <summary>
    /// Updates users scripts. Deletes outdated scripts and loads and runs new scripts as required
    /// on all nodes.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="statusSubsystem">      A reference to a
    ///                                     <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                     subsystem</see>. </param>
    /// <param name="scripts">              Specifies the list of the scripts to be deleted. </param>
    /// <param name="isSecondPass">         Set true on the second pass through if first pass
    ///                                     requires loading new scripts. </param>
    /// <param name="interactiveEnabled">   True to enable, false to disable the interactive. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    [Obsolete( "THIS REQUIRES MAJOR EDITING" )]
    public static bool UpdateUserScripts( Tsp.StatusSubsystemBase statusSubsystem, ScriptEntityCollection scripts, bool isSecondPass, bool interactiveEnabled )
    {
        if ( statusSubsystem is null ) throw new ArgumentNullException( nameof( statusSubsystem ) );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );
        Pith.SessionBase session = statusSubsystem.Session;
        string prefix = $"{session.ResourceNameNodeCaption}";

        if ( interactiveEnabled )
        {
            // clear buffers before deleting.
            // !@#
            _ = SessionBase.AsyncDelay( session.StatusReadDelay );
            _ = session.ReadStatusByte();
            _ = SessionBase.AsyncDelay( session.StatusReadDelay );
            _ = session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
        }

        return false;
        // run scripts so that we can read their version numbers. ScriptEntities will run only if not ran, namely
        // if there authorNamespaces are not defined.

        // THIS REQUIRES MAJOR EDITING:
        // Use the script analyzer from the script collection to figure out what needs to be done and proceed from there.

#if false

        if ( !scripts.RunScripts( this.Session, node, this ) )
        {
            // report any failure.
            if ( isSecondPass )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{prefix} failed running some firmware scripts because {scripts.OutcomeDetails};. {Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );
                return false;
            }
            else
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"{prefix} failed running some firmware scripts because {scripts.OutcomeDetails}--problem ignored;. " );
            }
        }

        // read scripts versions.
        if ( !scripts.ReadFirmwareVersions( node, this.Session ) )
        {
            if ( isSecondPass )
            {
                // report any failure.
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{prefix} failed reading some firmware versions because {scripts.OutcomeDetails};. {Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );
                return false;
            }
            else
            {
                // report any failure.
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"{prefix} failed reading some firmware versions because {scripts.OutcomeDetails}. Problem ignored;. " );
            }
        }

        // make sure program is up to date.
        if ( !isSecondPass && scripts.IsProgramOutdated( node ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{prefix} program out of date because {scripts.OutcomeDetails}. System initialization aborted;. {Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );
            return false;
        }

        if ( scripts.VersionsUnspecified( node ) )
        {
            if ( isSecondPass )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{prefix} failed verifying firmware version because {scripts.OutcomeDetails};. {Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );
                return false;
            }
            else
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( "{0} failed verifying firmware version because {1}--problem ignored;. ", prefix, scripts.OutcomeDetails );
            }
        }

        if ( scripts.AllVersionsCurrent( node ) )
        {
            return true;
        }
        else if ( isSecondPass )
        {
            _ = string.IsNullOrWhiteSpace( scripts.OutcomeDetails )
                ? _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{prefix} failed updating scripts;. Check log for details.{Environment.NewLine}{new StackFrame( true ).UserCallStack()}" )
                : _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{prefix} failed updating scripts because {scripts.OutcomeDetails};. {Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );

            return false;
        }
        else
        {

            // delete scripts that are outdated or slated for deletion.
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "{0} deleting outdated scripts;. ", prefix );
            if ( !this.DeleteUserScripts( scripts, node, true, true ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"{prefix} failed deleting outdated scripts;. Check log for details. Problem ignored." );
            }

            if ( this.LoadRunUserScripts( scripts, node ) )
            {
                return this.UpdateUserScripts( scripts, node, true );
            }
            else
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{prefix} failed loading and/or running scripts;. Check log for details.{Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );
                return false;
            }
        }
#endif

    }

    /// <summary>   Updates users scripts deleting, as necessary, those that are out of date. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="statusSubsystem">      A reference to a
    ///                                     <see cref="VI.Tsp.StatusSubsystemBase">status
    ///                                     subsystem</see>. </param>
    /// <param name="scripts">              Specifies the list of the scripts to be deleted. </param>
    /// <param name="nodes">                Specifies the list of nodes on which scripts are deleted. </param>
    /// <param name="interactiveEnabled">   True to enable, false to disable the interactive. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    [Obsolete( "THIS REQUIRES MAJOR EDITING" )]
    public static bool UpdateUserScripts( Tsp.StatusSubsystemBase statusSubsystem, ScriptEntityCollection scripts, NodeEntityCollection nodes, bool interactiveEnabled )
    {
        if ( statusSubsystem is null ) throw new ArgumentNullException( nameof( statusSubsystem ) );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );
        if ( nodes is null ) throw new ArgumentNullException( nameof( nodes ) );
        if ( !UpdateUserScripts( statusSubsystem, scripts, false, interactiveEnabled ) )
            return false;

        return true;
    }

}
