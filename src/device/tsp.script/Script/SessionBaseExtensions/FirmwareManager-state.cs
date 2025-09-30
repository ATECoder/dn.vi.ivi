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
        script.Embedded = false;

        script.Loaded = !session.IsNil( script.Node.IsController, script.Node.Number, script.Name );

        if ( script.Loaded )
        {
            script.Activated = !session.IsNil( script.Node.IsController, script.Node.Number, script.FirmwareVersionGetter.TrimEnd( ['(', ')'] ) );

            string? embeddedScripts = session.FetchEmbeddedScriptsNames( script.Node ).EmbeddedScripts;
            script.Embedded = !string.IsNullOrWhiteSpace( embeddedScripts )
                && (embeddedScripts!.IndexOf( script.Name + ",", 0, StringComparison.OrdinalIgnoreCase ) >= 0);
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
    /// <param name="embeddedScripts"> The embedded scripts. </param>
    public static void ReadScriptState( this Pith.SessionBase? session, ScriptEntityBase script, string embeddedScripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( embeddedScripts is null ) throw new ArgumentNullException( nameof( embeddedScripts ) );

        // clear the script state
        script.Activated = false;
        script.EmbeddedFirmwareVersion = string.Empty;
        script.HasFirmwareVersionGetter = false;
        script.Loaded = false;
        script.Embedded = false;
        script.LoadedAsByteCode = false;

        script.Loaded = !session.IsNil( script.Node.IsController, script.Node.Number, script.Name );

        if ( script.Loaded )
        {
            // note that the scripts are loaded as byte code first on the local node and then copied to the remote node.
            script.Activated = !session.IsNil( script.Node.IsController, script.Node.Number, script.FirmwareVersionGetter.TrimEnd( ['(', ')'] ) );

            script.Embedded = !string.IsNullOrWhiteSpace( embeddedScripts )
                && (embeddedScripts!.IndexOf( script.Name + ",", 0, StringComparison.OrdinalIgnoreCase ) >= 0);
            script.LoadedAsByteCode = session.IsByteCodeScript( script.Name );
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
