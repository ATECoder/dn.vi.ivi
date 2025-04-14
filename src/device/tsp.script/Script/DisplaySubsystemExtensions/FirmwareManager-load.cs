using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.DisplaySubsystemExtensions;

public static partial class FirmwareManager
{
    /// <summary>   Loads the specified TSP script from code. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="script">           Specifies reference to a valid <see cref="ScriptEntity">
    ///                                 script</see> </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool LoadUserScript( this DisplaySubsystemBase? displaySubsystem, ScriptEntityBase script )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( DisplaySubsystemBase.StatusSubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( DisplaySubsystemBase.Session ) );

        if ( displaySubsystem.Session.IsNil( script.Name ) )
        {
            script.Loaded = true;
            script.Saved = false;
            script.Activated = false;
            return displaySubsystem.LoadUserScriptThis( script );
        }
        else
        {
            script.Loaded = true;
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Instrument '{displaySubsystem.Session.ResourceNameCaption}' script '{script.Name}' already exists;. ",
                displaySubsystem.Session.ResourceNameCaption ?? "n/a resource scriptName", script.Name );
            return true;
        }
    }
    /// <summary>   Loads user script this. </summary>
    /// <remarks>   2024-09-07. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="VI.Tsp.DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="script">           Specifies the
    ///                                 <see cref="ScriptEntityBase">script</see>to delete. </param>
    /// <returns>   True if it the script was loaded, false if it fails. </returns>
    private static bool LoadUserScriptThis( this DisplaySubsystemBase? displaySubsystem, ScriptEntityBase script )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );
        if ( script.FirmwareScript is null ) throw new ArgumentNullException( nameof( script.FirmwareScript ) );
        if ( string.IsNullOrWhiteSpace( script.FirmwareVersionGetter ) ) throw new ArgumentNullException( nameof( script.FirmwareVersionGetter ) );
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( displaySubsystem.StatusSubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem.StatusSubsystem ) );
        if ( displaySubsystem.Session is null ) throw new ArgumentNullException( nameof( displaySubsystem.Session ) );

        Pith.SessionBase session = displaySubsystem.Session;
        session.LastNodeNumber = script.Node.Number;
        try
        {
            session.SetLastAction( $"loading {script.Name}" );
            displaySubsystem.DisplayLine( 2, $"Loading {script.Name}" );

            // todo: check for errors
            _ = session.TraceInformation();
            _ = session.TraceDeviceExceptionIfError( failureMessage: $"error(s) ignored before loading {script.Name}" );

            // load the script
            session.LoadScript( script.Name, script.FirmwareScript.Source );

            _ = session.TraceInformation();
            _ = session.TraceDeviceExceptionIfError( failureMessage: $"error(s) ignored loading {script.Name}" );

            script.Loaded = !session.IsNil( script.Name );

        }
        catch ( Pith.NativeException ex )
        {
            _ = session.TraceException( ex );
        }
        catch ( Exception ex )
        {
            _ = session.TraceException( ex );
        }

        if ( script.Loaded )
        {
            // do a garbage collection
            if ( !session.CollectGarbageQueryComplete( script.Node.Number ) )
                _ = session.TraceWarning( message: $"operation incomplete (reply not '1') after loading {script.Name} on node {script.Node.Number}" );

            _ = session.TraceDeviceExceptionIfError();

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{session.ResourceNameNodeCaption} {script.Name} script loaded;. " );
            displaySubsystem.DisplayLine( 2, "{0} Loaded", script.Name );
        }

        return script.Loaded;
    }
}
