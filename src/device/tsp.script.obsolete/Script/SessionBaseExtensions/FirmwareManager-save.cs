using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>
    /// Saves the specifies script to non-volatile memory, fetch the save scripts and verify that the
    /// script was saved.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Gets or sets the script name. </param>
    /// <param name="autoRun">      True to set the script to automatically run. </param>
    /// <returns>   <c>true</c> is script exist; otherwise, <c>false</c>. </returns>
    public static (bool Saved, string SavedScriptNames, List<string> AuthorScriptNames) SaveScriptAndVerify( this Pith.SessionBase session, string scriptName, bool autoRun )
    {
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        // save
        session.SaveScript( scriptName, autoRun );

        // fetch names
        (string scriptNames, List<string> authorScripts) = session.FetchSavedScriptsNames();

        return (FirmwareScriptBase.ScriptNameExists( scriptNames, scriptName ), scriptNames, authorScripts);
    }

    /// <summary>   A Pith.SessionBase extension method that saves a script. </summary>
    /// <remarks>   2024-09-20. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Gets or sets the script name. </param>
    /// <param name="autoRun">      True to set the script to automatically run. </param>
    public static void SaveScript( this Pith.SessionBase session, string scriptName, bool autoRun )
    {
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        // saving just the script name does not work if script was created and script name assigned.
        session.LastNodeNumber = default;
        if ( autoRun )
        {
            session.SetLastAction( $"setting script '{scriptName}' to auto run." );
            // _ = session.WriteLine( $"{scriptName}.autorun = \"yes\" " );
            _ = session.WriteLine( $"script.user.scripts.{scriptName}.autorun = \"yes\" {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} " );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );
        }
        session.SetLastAction( $"saving script '{scriptName}'" );
        // _ = session.WriteLine( $"{scriptName}.save()" );
        _ = session.WriteLine( $"script.user.scripts.{scriptName}.save() {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} " );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

        // read query reply and throw if reply is not 1.
        session.QueryAndThrowIfOperationIncomplete();

        // throw if device errors
        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   A Pith.SessionBase extension method that saves a script. </summary>
    /// <remarks>   2024-11-23. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Gets or sets the script name. </param>
    /// <param name="node">         The node. </param>
    /// <param name="autoRun">      True to set the script to automatically run. </param>
    public static void SaveScript( this Pith.SessionBase session, string scriptName, NodeEntityBase node, bool autoRun )
    {
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );

        if ( node.IsController )
            session.SaveScript( scriptName, autoRun );
        else
        {
            session.SetLastAction( $"setting script '{scriptName}' on node {node.Number} to auto run" );
            _ = session.ExecuteCommandWaitComplete( node.Number, $"script.user.scripts.{scriptName}.autorun = \"yes\" " );

            session.SetLastAction( $"saving script '{scriptName}' on node {node.Number}" );
            _ = session.ExecuteCommandWaitComplete( node.Number, $"script.user.scripts.{scriptName}.save() {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} " );

            // read query reply and throw if reply is not 1.
            session.QueryAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();
        }
    }
}
