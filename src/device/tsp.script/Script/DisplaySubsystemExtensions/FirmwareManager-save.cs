using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.DisplaySubsystemExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Embeds the user script in non-volatile memory. </summary>
    /// <remarks>
    /// 2024-09-05. The embed needs to be validated once all scripts are embedded to embed time.
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="scriptName">       Specifies the script to embed. </param>
    /// <param name="node">             Specifies the node. </param>
    /// <param name="embedAsByteCode">     Specifies the condition requesting saving the user scripts
    ///                                 source as byte code. </param>
    /// <param name="autoRun">          Specifies the condition indicating if this script is to automatically run upon instrument start. </param>
    public static void EmbedUserScript( this DisplaySubsystemBase? displaySubsystem, string scriptName, NodeEntityBase node,
        bool embedAsByteCode, bool autoRun )
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
            throw new InvalidOperationException( $"{session.ResourceNameNodeCaption} custom firmware script '{scriptName}' not embedded on node {node.Number};. " );

        if ( !autoRun )

            // if a script is a boot script, a boot embed is required.
            node.BootScriptEmbedRequired = true;

        // embed validation is done after all scripts are embedded.
        // this throws an error on device errors

        if ( embedAsByteCode )
        {
            displaySubsystem.DisplayLine( 2, $"{node.Number}:{scriptName} 2 binary" );
            if ( node.IsController )
                session.ConvertToByteCode( scriptName );
            else
                // displaySubsystem.ConvertBinaryScript( binaryScriptName, node, timeoutInfo );
                throw new InvalidOperationException( "loading byte code scripts to a remote node is not supported at this time." );
        }

        // embed the script.
        displaySubsystem.DisplayLine( 2, $"{node.Number}:{scriptName} saving" );
        session.EmbedScript( scriptName, node, autoRun );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{session.ResourceNameNodeCaption} embedded script '{scriptName}' on node {node.Number};. " );
    }

    /// <summary>   Embeds the user script in non-volatile memory. </summary>
    /// <remarks>   2024-09-05.
    /// Embed validation is done after all scripts are embedded. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="script">           Specifies the <see cref="ScriptEntityBase">script</see>to
    ///                                 delete. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static void EmbedUserScript( this DisplaySubsystemBase? displaySubsystem, ScriptEntityBase script )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );

        displaySubsystem.EmbedUserScript( script.Name, script.Node, script.FirmwareScript.ConvertToByteCode, script.FirmwareScript.IsAutoexecScript );

    }

    /// <summary>   Embeds the user scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="scripts">          Specifies the list of scripts to embed. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool EmbedUserScripts( this DisplaySubsystemBase? displaySubsystem, ScriptEntityCollection scripts )
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

            if ( script.FirmwareScript.IsAutoexecScript )
                bootScript = script;

            if ( script.Loaded && !script.Embedded )
                displaySubsystem.EmbedUserScript( script );
#if false
            if ( script.Loaded )
            {
            session.SetLastAction( $"checking if embed is required for '{script.Name}' on node {script.Node.Number}" );
                // TODO: Replace scripts.IsEmbedRequired( session, script )  with script.IsEmbeddedRequired()

                bool isEmbedRequired = scripts.IsEmbedRequired( session, script );
                resetRequired = resetRequired || isEmbedRequired;

                if ( isEmbedRequired )
                    displaySubsystem.EmbedUserScript( script );
            }
#endif
        }

        session.SetLastAction( "reading the scripts state after saving" );
        scripts.ReadScriptsState( session );
        bool success = scripts.AllEmbedded();
        if ( success )
            scripts.Node.BootScriptEmbedRequired = false;

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
                displaySubsystem.DisplayLine( 2, $"All scripts embedded on node {scripts.Node.Number}" );
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

    /// <summary>   Embeds all users scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem">     A reference to a
    ///                                     <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                     subsystem</see>. </param>
    /// <param name="scriptsCollection">    Collection of <see cref="ScriptEntityCollection"/>. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool EmbedUserScripts( this DisplaySubsystemBase? displaySubsystem, ICollection<ScriptEntityCollection> scriptsCollection )
    {
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );
        bool success = true;
        foreach ( ScriptEntityCollection scripts in scriptsCollection )
            success &= displaySubsystem.EmbedUserScripts( scripts );

        return success;
    }
}
