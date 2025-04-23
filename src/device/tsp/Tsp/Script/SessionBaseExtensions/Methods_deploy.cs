using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that queries if the script is set to run when
    /// the instrument starts.
    /// </summary>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <returns>   True if the script is set to auto run, false if not. </returns>
    public static bool IsAutoRun( this SessionBase session, string scriptName )
    {
        string tspCommand = $"print( {scriptName}.autorun )";
        string yesNo = session.QueryTrimEnd( tspCommand );
        bool isAutoRun = string.Equals( yesNo, "yes", StringComparison.OrdinalIgnoreCase );
        return isAutoRun;
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that disable the script from running when
    /// the instrument starts.
    /// </summary>
    /// <remarks>   2025-04-22. <para>
    /// The script must be saved to make this so. </para> </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Name of the script. </param>
    public static void TurnOffAutoRan( this SessionBase session, string scriptName )
    {
        if ( session.IsAutoRun( scriptName ) )
            _ = session.WriteLine( $"script.user.scripts.{scriptName}.autorun = \"no\" {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} " );
        //  _ = session.WriteLine( $"{scriptName}.autorun = \"no\" {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand}";
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that sets the script to run when
    /// the instrument starts.
    /// </summary>
    /// <remarks>
    /// 2025-04-22. <para>
    /// The script must be saved to make this so. </para>
    /// </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Name of the script. </param>
    public static void TurnOnAutoRan( this SessionBase session, string scriptName )
    {
        if ( !session.IsAutoRun( scriptName ) )
            _ = session.WriteLine( $"script.user.scripts.{scriptName}.autorun = \"yes\" {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} " );
        //  _ = session.WriteLine( $"{scriptName}.autorun = \"yes\" {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand}";
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads the script from the deploy file
    /// and saves it to non-volatile memory.
    /// </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="folderPath">   Full pathname of the folder file. </param>
    /// <param name="lineDelay">    The line delay. </param>
    public static void ImportSaveToNvm( this Pith.SessionBase session, ScriptInfoBase scriptInfo, string folderPath, TimeSpan lineDelay )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        // set the deploy file path.
        string deployFilePath = Path.Combine( folderPath, scriptInfo.DeployFileName );

        // delete the script if it exists.
        session.DeleteScript( scriptInfo.Title );

        SessionBaseExtensionMethods.TraceLastAction( $"Importing script from {scriptInfo.DeployFileFormat} '{deployFilePath}' file" );
        session.ImportScript( scriptInfo.Title, deployFilePath, lineDelay );

        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        session.SaveScript( scriptInfo.Title );

        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that queries if a load menu item exists.
    /// </summary>
    /// <remarks>   2025-04-21. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="itemName"> Name of the item. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool LoadMenuItemExists( this SessionBase session, string itemName )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( itemName ) ) throw new ArgumentNullException( nameof( itemName ) );

        string query = string.Format( Syntax.Tsp.Script.FindLoadMenuItemQueryFormat, itemName );
        return SessionBase.EqualsTrue( session.QueryTrimEnd( query ) );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that deletes the load menu item. </summary>
    /// <remarks>   2025-04-16. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="itemName"> Name of the item. </param>
    public static void DeleteLoadMenuItem( this SessionBase session, string itemName )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( itemName ) ) throw new ArgumentNullException( nameof( itemName ) );

        if ( session.LoadMenuItemExists( itemName ) )
        {
            session.TraceLastAction( $"deleting the '{itemName}' load menu item;. " );
            string command = string.Format( Syntax.Tsp.Script.DeleteExistingLoadMenuItemCommandFormat, itemName );
            command += " " + Syntax.Tsp.Lua.OperationCompletedQueryCommand;
            _ = session.WriteLine( command );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();

            if ( session.LoadMenuItemExists( itemName ) )
                throw new InvalidOperationException( $"The load menu item {itemName} was not deleted." );
        }
    }
}
