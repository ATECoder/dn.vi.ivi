using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that query if the specified script is
    /// activated.
    /// </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="session">              The session. </param>
    /// <param name="scriptName">           Specifies the script name. Empty for an anonymous script. </param>
    /// <param name="expectedScriptEntity"> The expected script entity. </param>
    /// <returns>   True if loaded, false if not. </returns>
    public static bool IsActivated( this Pith.SessionBase session, string scriptName, string expectedScriptEntity )
    {
        return !(session.IsNil( scriptName ) || session.IsNil( expectedScriptEntity ));
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that executes the 'script' operation. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The reference to the K2600 Device. </param>
    /// <param name="scriptName">           (Optional) [empty] Name of the script. Empty if running
    ///                                     an anonymous script. </param>
    /// <param name="scriptElementName">    (Optional) [empty] Name of the script element. </param>
    public static void RunScript( this SessionBase session, string scriptName = "", string scriptElementName = "" )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        if ( !string.IsNullOrWhiteSpace( scriptName ) )
        {
            session.SetLastAction( $"checking if the {scriptName} script exists;. " );
            if ( session.IsNil( scriptName ) )
                throw new InvalidOperationException( $"The script {scriptName} cannot be run because it was not found." );

            session.SetLastAction( $"running the {scriptName} script;. " );
            _ = session.WriteLine( $"{scriptName}.run()" );
        }
        else
        {
            session.SetLastAction( $"running the anonymous script;. " );
            _ = session.WriteLine( $"scrip.run()" );
        }
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        if ( !string.IsNullOrWhiteSpace( scriptElementName ) )
        {
            session.SetLastAction( $"looking for {scriptElementName};. " );
            if ( session.IsNil( scriptElementName ) )
                throw new InvalidOperationException( $"The script element {scriptElementName} was not found after running the {scriptName} script." );
        }
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that enumerate the loaded table. </summary>
    /// <remarks>   2024-10-14. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <returns>   The require table items. </returns>
    public static string EnumerateTheLoadedTable( this Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        string fetchQuery = "names = \"\" for name, value in _G.pairs( _G._LOADED ) do if name ~= nil then names = names .. name .. ',' end end print ( names ) ";
        _ = session.WriteLine( fetchQuery );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );
        return session.ReadLineTrimEnd();
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that queries if the LOADED table includes
    /// the specified item.
    /// </summary>
    /// <remarks>   2025-04-22. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="itemName"> The name of the expected item. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool IncludedInLoadedTable( this SessionBase session, string itemName, out string details )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        string loadedItems = session.EnumerateTheLoadedTable();

        if ( string.IsNullOrWhiteSpace( loadedItems ) )
        {
            details = $"The LOADED table is empty.";
            return false;
        }
        else if ( loadedItems.Contains( itemName, StringComparison.InvariantCulture ) )
        {
            details = $"The LOADED table includes '{itemName}'.";
            return true;
        }
        else
        {
            details = $"The LOADED table does not include '{itemName}'.";
            return false;
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that queries if the LOADED table includes
    /// the specified item using the TSP <c>_G.isInLoaded</c> method.
    /// </summary>
    /// <remarks>   2025-04-22. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="itemName"> The name of the expected item. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if in loaded, false if not. </returns>
    public static bool IsInLoaded( this SessionBase session, string itemName, out string details )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        string functionName = "_G.isInLoaded";
        string functionCommand = $"{functionName}('{itemName}')";

        if ( session.IsNil( functionName ) )
        {
            details = $"The function {functionName} was not found.";
            return false;
        }
        else
        {
            string reply = session.QueryTrimEnd( Syntax.Tsp.Lua.PrintCommand( functionCommand ) );
            if ( reply == cc.isr.VI.Syntax.Tsp.Lua.TrueValue )
            {
                details = $"The function {functionName} found the {itemName}.";
                return true;
            }
            else
            {
                details = $"The function {functionName} did not fine the {itemName}.";
                return false;
            }
        }
    }
}
