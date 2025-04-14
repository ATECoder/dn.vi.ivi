using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.DisplaySubsystemExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Loads and runs the user scripts on the controller instrument. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="displaySubsystem">     A reference to a
    ///                                     <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                     subsystem</see>. </param>
    /// <param name="scripts">              Specifies the collection of scripts. </param>
    /// <returns>   The run user scripts. </returns>
    public static bool LoadRunUserScripts( this DisplaySubsystemBase? displaySubsystem, ScriptEntityCollection scripts )
    {
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );
        if ( scripts.Node is null ) throw new ArgumentNullException( nameof( scripts.Node ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );

        Pith.SessionBase session = displaySubsystem.Session;

        // <c>true</c> if any load and run action was executed
        bool resetRequired = false;
        if ( scripts is not null && scripts.Count > 0 )
        {
            foreach ( ScriptEntityBase script in scripts )
            {
                // reset if a new script will be loaded.
                resetRequired = resetRequired || session.IsNil( script );
                displaySubsystem.LoadRunUserScript( script );
            }
            // update the list of loaded scripts.
            scripts.FetchLoadedScriptsNames( session );
        }

        bool success = false;
        if ( resetRequired )
        {
            // reset to refresh the instrument display.
            try
            {
                session.SetLastAction( "resetting local TSP node" );
                _ = session.WriteLine( "localnode.reset()" );
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

    /// <summary>   Loads and executes the specified TSP script from file. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when operation failed to execute. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="script">           Specifies reference to a valid <see cref="ScriptEntity">
    ///                                 script</see> </param>
    public static void LoadRunUserScript( this DisplaySubsystemBase? displaySubsystem, ScriptEntityBase script )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.FirmwareScript is null ) throw new ArgumentNullException( nameof( script.FirmwareScript ) );
        if ( string.IsNullOrWhiteSpace( script.FirmwareVersionGetter ) ) throw new ArgumentNullException( nameof( script.FirmwareVersionGetter ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );

        Pith.SessionBase session = displaySubsystem.Session;
        session.LastNodeNumber = script.Node.Number;

        session.SetLastAction( $"checking if script '{script.Name}' was loaded" );
        if ( session.IsNil( script.Node.IsController, script.Node.Number, [script.Name] ) )
        {
            if ( string.IsNullOrWhiteSpace( script.FirmwareScript.Source ) && script.FirmwareScript.Source.Length > 10 )
            {
                displaySubsystem.DisplayLine( 2, $"Attempted loading empty script {script.Node.Number}:{script.Name}" );
                throw new InvalidOperationException( $"Attempted loading empty script;. {script.Node.Number}:{script.Name}" );
            }

            session.SetLastAction( $"loading script {script.Name}" );
            if ( FirmwareManager.LoadUserScript( displaySubsystem, script ) )
            {
                session.SetLastAction( $"running script {script.Name}" );
                if ( !displaySubsystem.RunUserScript( script ).Activated )
                    throw new InvalidOperationException( $"Failed running script;. {script.Node.Number}:{script.Name}" );
            }
            else
                throw new InvalidOperationException( $"Failed loading script;. {script.Node.Number}:{script.Name}" );

        }
    }
}
