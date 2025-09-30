using System.Diagnostics;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.VI.Tsp.Script.StatusSubsystemBaseExtensions;

namespace cc.isr.VI.Tsp.Script.DisplaySubsystemExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Loads the specified script from the local to the remote node. </summary>
    /// <remarks>   Will not load a script list that includes the create script command. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="script">           Specifies reference to a valid
    ///                                 <see cref="ScriptEntity">script</see> </param>
    /// <param name="uploadTimeout">    The upload timeout. </param>
    public static void UploadUserScript( this DisplaySubsystemBase? displaySubsystem, ScriptEntityBase script, TimeSpan uploadTimeout )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );
        if ( script.FirmwareScript is null ) throw new ArgumentNullException( nameof( script.FirmwareScript ) );
        if ( string.IsNullOrWhiteSpace( script.FirmwareVersionGetter ) ) throw new ArgumentNullException( nameof( script.FirmwareVersionGetter ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );

        Pith.SessionBase session = displaySubsystem.Session;
        Tsp.StatusSubsystemBase statusSubsystem = ( Tsp.StatusSubsystemBase ) displaySubsystem.StatusSubsystem;

        session.LastNodeNumber = script.Node.Number;
        script.Loaded = !session.IsNil( script.Node.Number, script.Name );

        if ( script.Loaded )
        {
            displaySubsystem.DisplayLine( 2, "{0}_{1} exists", script.Node.Number, script.Name );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose(
                $"{session.ResourceNameNodeCaption} script '{script.Name}' already exists on node {script.Node.Number};. Nothing to do." );
            return;
        }
        else
        {
            script.Embedded = false;
            script.Activated = false;
        }

        session.SetLastAction( $"uploading {script.Node.Number}:{script.Name}" );
        displaySubsystem.DisplayLine( 2, $"Uploading {script.Node.Number}:{script.Name}" );
        script.Loaded = statusSubsystem.UploadScript( script );
        if ( !script.Loaded )
        {
            displaySubsystem.DisplayLine( 2, $"Failed uploading {script.Node.Number}:{script.Name}" );
            return;
        }

        displaySubsystem.DisplayLine( 2, $"Verifying {script.Node.Number}:{script.Name}" );
        if ( !session.WaitNotNil( script.Node.Number, script.Name, uploadTimeout ) )
        {
            script.Loaded = false;
            displaySubsystem.DisplayLine( 2, $"{script.Node.Number} {script.Name} not found after loading" );
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed loading script '{script.Name}' to node {script.Node.Number};. --new script not found on the remote node.{Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );
            return;
        }

        if ( !script.Node.IsController && script.Node.ControllerNode is not null )
        {
            // do a garbage collection
            displaySubsystem.DisplayLine( 2, "Cleaning local node" );

            session.SetLastAction( $"collecting garbage after uploading {script.Node.ControllerNode.Number}:{script.Name}" );

            if ( !session.CollectGarbageQueryComplete() )
                _ = session.TraceWarning( message: $"garbage collection incomplete (reply not '1') after uploading {script.Name} on node {script.Node.ControllerNode.Number}" );

            _ = session.TraceDeviceExceptionIfError();
        }

        // do a garbage collection on remote node
        displaySubsystem.DisplayLine( 2, $"Cleaning node {script.Node.Number}" );
        session.SetLastAction( $"collecting garbage after uploading {script.Node.Number}:{script.Name}" );

        if ( !session.CollectGarbageQueryComplete( script.Node.Number ) )
            _ = session.TraceWarning( message: $"garbage collection incomplete (reply not '1') after uploading {script.Name} on node {script.Node.Number}" );

        _ = session.TraceDeviceExceptionIfError( failureMessage: $"ignoring error after uploading user script {script.Name} on node {script.Node.Number}" );

        displaySubsystem.DisplayLine( 2, $"{script.Node.Number}:{script.Name} Loaded" );
    }
}
