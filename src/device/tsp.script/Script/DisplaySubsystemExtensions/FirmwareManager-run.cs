using System.Diagnostics;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.Std.TrimExtensions;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.DisplaySubsystemExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Executes the specified TSP script from file. </summary>
    /// <remarks>
    /// David, 2009-05-13. Modified to run irrespective of the existence of the script
    /// because a new script can be loaded on top of existing old code and the new version number
    /// will not materialize.
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="script">           Specifies reference to a valid <see cref="ScriptEntity">
    ///                                 script</see> </param>
    /// <param name="always">        (Optional) true to run always. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static (bool Activated, string Details) RunUserScript( this DisplaySubsystemBase? displaySubsystem, ScriptEntityBase script, bool always = true )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );

        Pith.SessionBase session = displaySubsystem.Session;

        session.LastNodeNumber = script.Node.Number;
        session.SetLastAction( $"checking if script {script} is nil" );
        if ( session.IsNil( script ) )
        {
            script.Loaded = false;
            script.Saved = false;
            script.Activated = false;
            return (false, $"failed running {script.Name} because it does not exist;. {Environment.NewLine}{new StackFrame( true ).UserCallStack()}");
        }

        // taken out to run always.
        session.SetLastAction( "checking if script was activated (run)" );
        if ( !always && session.HasScriptNamespaces( script ) )
        {
            script.Loaded = true;
            script.Activated = true;

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{session.ResourceNameNodeCaption} script '{script.Name}' already run. Nothing to do;. " );
            return (true, $"{session.ResourceNameNodeCaption} script '{script.Name}' already run. Nothing to do;. ");
        }

        System.Text.StringBuilder builder = new();

        displaySubsystem.DisplayLine( 2, $"Running {script.Name}{session.LastNodeNumberCaption}" );

        session.SetLastAction( $"running {script.Name}" );
        session.RunScript( script, always );

        if ( !string.IsNullOrWhiteSpace( script.LastScriptManagerActions ) ) _ = builder.AppendLine( $"{script.Name} actions:\n\t{script.LastScriptManagerActions}" );

        if ( script.Activated )
        {

            session.SetLastAction( $"checking activation of {script.Name}" );
            if ( script.FirmwareVersionGetter is not null && script.FirmwareVersionGetter.Length > 0
                && session.IsNil( script.Node.IsController, script.Node.Number, script.FirmwareVersionGetter.TrimEnd( ['(', ')'] ) ) )
            {
                script.Saved = false;
                script.Activated = false;
                _ = builder.AppendLine( $"{session.ResourceNameNodeCaption} namespace {script.FirmwareVersionGetter.TrimEnd( ['(', ')'] )} is nil;. " );
                displaySubsystem.DisplayLine( 2, $"{script.Name}{session.LastNodeNumberCaption} missing version getter" );
            }
            else
            {
                displaySubsystem.DisplayLine( 2, $"Done running {script.Name}{session.LastNodeNumberCaption}" );
                _ = builder.AppendLine( $"{session.ResourceNameNodeCaption} {script.Name} script run okay;. " );
            }
        }
        else
        {
            session.SetLastAction( $"looking for script {script.Name}" );
            if ( session.IsNil( script ) )
            {
                _ = builder.AppendLine( $"{session.ResourceNameNodeCaption} script '{script.Name}' not found;. " );
                return (script.Activated, builder.ToString().TrimEndNewLine());
            }
        }

        return (script.Activated, builder.ToString().TrimEndNewLine());
    }
}
