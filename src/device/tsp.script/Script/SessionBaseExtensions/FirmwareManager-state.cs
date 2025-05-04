namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that reads scripts state. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="script">   The script. </param>
    public static void ReadScriptState( this Pith.SessionBase? session, ScriptEntityBase script )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        // clear the script state
        script.Activated = false;
        script.EmbeddedFirmwareVersion = string.Empty;
        script.HasFirmwareVersionGetter = false;
        script.Loaded = false;
        script.Saved = false;

        script.Loaded = !session.IsNil( script.Node.IsController, script.Node.Number, script.Name );

        if ( script.Loaded )
        {
            script.Activated = !session.IsNil( script.Node.IsController, script.Node.Number, script.FirmwareVersionGetter.TrimEnd( ['(', ')'] ) );

            string? savedScripts = session.FetchSavedScriptsNames( script.Node ).SavedScripts;
            script.Saved = !string.IsNullOrWhiteSpace( savedScripts )
                && (savedScripts!.IndexOf( script.Name + ",", 0, StringComparison.OrdinalIgnoreCase ) >= 0);
        }

        if ( script.Activated )
        {
            script.HasFirmwareVersionGetter = script.FirmwareVersionGetterExists( session );

            if ( script.HasFirmwareVersionGetter )
                script.EmbeddedFirmwareVersion = script.QueryFirmwareVersion( session );
        }

        script.VersionStatus = script.ValidateFirmware();
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that reads scripts state. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="script">       The script. </param>
    /// <param name="savedScripts"> The saved scripts. </param>
    public static void ReadScriptState( this Pith.SessionBase? session, ScriptEntityBase script, string savedScripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( savedScripts is null ) throw new ArgumentNullException( nameof( savedScripts ) );

        // clear the script state
        script.Activated = false;
        script.EmbeddedFirmwareVersion = string.Empty;
        script.HasFirmwareVersionGetter = false;
        script.Loaded = false;
        script.Saved = false;
        script.LoadedAsBinary = false;

        script.Loaded = !session.IsNil( script.Node.IsController, script.Node.Number, script.Name );

        if ( script.Loaded )
        {
            // note that the scripts are loaded as binary first on the local node and then copied to the remote node.
            script.Activated = !session.IsNil( script.Node.IsController, script.Node.Number, script.FirmwareVersionGetter.TrimEnd( ['(', ')'] ) );

            script.Saved = !string.IsNullOrWhiteSpace( savedScripts )
                && (savedScripts!.IndexOf( script.Name + ",", 0, StringComparison.OrdinalIgnoreCase ) >= 0);
            script.LoadedAsBinary = session.IsByteCodeScript( script.Name );
        }

        if ( script.Activated )
        {
            script.HasFirmwareVersionGetter = script.FirmwareVersionGetterExists( session );

            if ( script.HasFirmwareVersionGetter )
                script.EmbeddedFirmwareVersion = script.QueryFirmwareVersion( session );
        }

        script.VersionStatus = script.ValidateFirmware();
    }

}
