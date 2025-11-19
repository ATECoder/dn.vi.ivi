using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that enumerate the loaded table.
    /// </summary>
    /// <remarks>
    /// 2024-10-14. <para>
    /// Each time an script is run, its <c>ChunkName</c> is added to the <c>_G._LOADED</c> table.
    /// Thus, script can determine their dependencies by checking if an item exists in the <c>
    /// _G._LOADED</c> table. gets </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <returns>   The require table items. </returns>
    public static string EnumerateTheLoadedTable( this Pith.SessionBase session )
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
    /// <remarks>
    /// 2025-04-22. <para>
    /// Each time an script is run, its <c>ChunkName</c> is added to the <c>_G._LOADED</c> table.
    /// Thus, script can determine their dependencies by checking if an item exists in the <c>
    /// _G._LOADED</c> table. gets </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="codeChunkName">    Name of the code chunk. </param>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool IncludedInLoadedTable( this SessionBase session, string codeChunkName, out string details )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        string loadedItems = session.EnumerateTheLoadedTable();

        if ( string.IsNullOrWhiteSpace( loadedItems ) )
        {
            details = $"The LOADED table is empty.";
            return false;
        }
        else if ( loadedItems.Contains( codeChunkName, StringComparison.InvariantCulture ) )
        {
            details = $"The LOADED table includes '{codeChunkName}'.";
            return true;
        }
        else
        {
            details = $"The LOADED table does not include '{codeChunkName}'.";
            return false;
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that queries if the LOADED table includes
    /// the specified item using the TSP <c>_G.isInLoaded</c> method.
    /// </summary>
    /// <remarks>   2025-04-22. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="codeChunkName">    Name of the code chunk. </param>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   True if in loaded, false if not. </returns>
    public static bool IsInLoaded( this SessionBase session, string codeChunkName, out string details )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        string functionName = "_G.isInLoaded";
        string functionCommand = $"{functionName}('{codeChunkName}')";

        if ( session.IsNil( functionName ) )
        {
            string isInLoadedCode = "function isInLoaded( requiredName ) return nil ~= _G._LOADED[ requiredName or \"\" ] end";
            session.SetLastAction( $"loading the {functionName} function;. " );
            _ = session.WriteLine( isInLoadedCode );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        }

        if ( session.IsNil( functionName ) )
        {
            details = $"the function {functionName} was not found.";
            return false;
        }
        else
        {
            string reply = session.QueryTrimEnd( Syntax.Tsp.Lua.PrintCommand( functionCommand ) );
            if ( reply == cc.isr.VI.Syntax.Tsp.Lua.TrueValue )
            {
                details = $"The function {functionName} found the {codeChunkName}.";
                return true;
            }
            else
            {
                details = $"The function {functionName} did not fine the {codeChunkName}.";
                return false;
            }
        }
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

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that executes the 'scripts' operation. </summary>
    /// <remarks>   2025-04-26. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The reference to the K2600 Device. </param>
    /// <param name="scripts">  The scripts. </param>
    public static void RunScripts( this SessionBase session, ScriptInfoCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        session.TraceLastAction( "enabling service request on operation completion" );
        session.EnableServiceRequestOnOperationCompletion();

        session.LastNodeNumber = default;
        foreach ( ScriptInfo script in scripts )
        {
            if ( !session.IsNil( script.Title ) )
            {
                session.RunScript( script.Title );
            }
        }
    }

}
