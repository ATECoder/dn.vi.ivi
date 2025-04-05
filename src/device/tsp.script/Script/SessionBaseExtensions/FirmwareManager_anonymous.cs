using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

/// <summary>   Manager for script firmware. </summary>
/// <remarks>   2024-09-06. </remarks>
public static partial class FirmwareManager
{

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads an anonymous script.
    /// </summary>
    /// <remarks>   2024-10-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptSource"> The script source. </param>
    public static void LoadAnonymousScript( this Pith.SessionBase? session, string? scriptSource )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptSource is null || string.IsNullOrWhiteSpace( scriptSource ) ) throw new ArgumentNullException( nameof( scriptSource ) );

        // decorate source with load script end script and load string as necessary. Must run an anonymous script after load
        scriptSource = FirmwareScriptBase.BuildLoadScriptSyntax( scriptSource, string.Empty, true );

        // write the script source to the instrument.
        session.SetLastAction( "loading anonymous script" );
        session.WriteLines( scriptSource, Environment.NewLine, FirmwareScriptBase.WriteLinesDelay );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.WaitCommand );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   A Pith.SessionBase? extension method that loads named script. </summary>
    /// <remarks>   2025-04-03. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="scriptName">           Name of the script. </param>
    /// <param name="scriptSource">         The script source. </param>
    /// <param name="runScriptAfterLoad">   True to run script after load. </param>
    public static void LoadNamedScript( this Pith.SessionBase? session, string scriptName, string? scriptSource, bool runScriptAfterLoad )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptSource is null || string.IsNullOrWhiteSpace( scriptSource ) ) throw new ArgumentNullException( nameof( scriptSource ) );

        // decorate source with load script end script and load string as necessary. Must run an anonymous script after load
        scriptSource = FirmwareScriptBase.BuildLoadScriptSyntax( scriptSource, scriptName, runScriptAfterLoad );

        // write the script source to the instrument.
        session.SetLastAction( $"loading script {scriptName}" );
        session.WriteLines( scriptSource, Environment.NewLine, FirmwareScriptBase.WriteLinesDelay );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.WaitCommand );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.ThrowDeviceExceptionIfError();
    }
}
