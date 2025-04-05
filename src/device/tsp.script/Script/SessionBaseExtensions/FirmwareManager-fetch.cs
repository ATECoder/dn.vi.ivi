using System.Diagnostics;
using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    #region " fetch names "

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

    #endregion

    #region " fetch saved scripts names "

    /// <summary>
    /// Loads the 'printUserScriptNames' function and return the function scriptName. The function is
    /// loaded only if it does not exists already.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <exception cref="DeviceException">  Thrown when a Device error condition occurs. </exception>
    /// <returns>   The function name. </returns>
    public static string LoadPrintUserScriptNamesFunction( this Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        session.LastNodeNumber = default;
        string functionName = "printUserScriptNames";
        session.SetLastAction( $"checking if function '{functionName}' exits" );
        if ( session.IsGlobalExists( functionName ) )
            return functionName;
        else
        {
            string functionCode =
                " function printUserScriptNames( delimiter ) local scripts = nil for i,v in _G.pairs(_G.script.user.scripts) do if scripts == nil then scripts = i else scripts = scripts .. delimiter .. i end end _G.print (scripts) _G.waitcomplete() end ";

            session.SetLastAction( $"loading function {functionName}" );
            if ( session.LoadFunction( functionName, functionCode ) )
            {
                _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );
                return functionName;
            }
            else
                throw new NativeException( $"Failed loading function '{functionName}' code." );
        }
    }

    /// <summary>   Fetches loaded user script names from the user scripts table. </summary>
    /// <remarks>
    /// 2024-09-06. <para>
    /// Relies on teh communication timeout for reading the result.  It takes longer than the fetch
    /// delay. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="TimeoutException">             Thrown when a Timeout error condition occurs. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="fetchTimeoutMilliseconds"> (Optional) The fetch timeout in milliseconds. </param>
    /// <returns>   The user script names this. </returns>
    public static string[] FetchUserScriptNames( this Pith.SessionBase? session, int fetchTimeoutMilliseconds = 2000 )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        string names;
        string[] userScriptNames = [];
        session.LastNodeNumber = default;

        // set the function argument
        char delimiter = ',';

        // load the TSP function that gets the script names.
        session.SetLastAction( "loading the script name fetching function" );
        string functionName = session.LoadPrintUserScriptNamesFunction();
        session.ThrowDeviceExceptionIfError();

        if ( string.IsNullOrWhiteSpace( functionName ) )
            throw new InvalidOperationException( $"{session.LastAction} failed" );

        try
        {
            session.StoreCommunicationTimeout( TimeSpan.FromMilliseconds( fetchTimeoutMilliseconds ) );

            session.SetLastAction( $"executing '{functionName}' for fetching user script names" );
            session.CallFunction( functionName, "'" + delimiter + "'" );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );
            session.ThrowDeviceExceptionIfError();

            session.SetLastAction( $"reading '{functionName}' output" );
            names = session.ReadLineTrimEnd();
        }
        catch ( Exception )
        {
            throw;
        }
        finally
        {
            session.RestoreCommunicationTimeout();
        }

        string expectedOne = cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue;
        string expectedTrue = Syntax.Tsp.Lua.TrueValue;

        // read the names
        if ( names is null || string.IsNullOrWhiteSpace( names ) )
        {
            // if nothing was read; no user scripts stored.
            // we are assuming that the the tsp prompt ("TSP>") is disabled.
            // return the empty array as initialized above.
        }

        // check if returned script names is a TSP null value. This will happened if the call is bad.
        else if ( names == Syntax.Tsp.Lua.NilValue )
        {
            // read the next scriptName, which should be true
            string value = session.ReadLineTrimEnd().Trim();
            if ( value is null || string.IsNullOrWhiteSpace( value )
                || !(value.StartsWith( expectedTrue, StringComparison.OrdinalIgnoreCase )
                    || value.StartsWith( expectedOne, StringComparison.OrdinalIgnoreCase )) )

                Debug.Assert( !Debugger.IsAttached, "Expected True", $"Expected '{expectedOne}' or '{expectedTrue}' but got {value}" );
        }
        else
        {
            // check if the return value is false.  This will happen if the function failed.
            if ( (names![..4] ?? "") == Syntax.Tsp.Lua.FalseValue )
                throw new InvalidOperationException( $"{session.LastAction} failed fetching valid script names" );
            else
            {
                // split the return values
                userScriptNames = names.Split( delimiter );

                // read the next value, which should be true
                string value = session.ReadLineTrimEnd().Trim();
                session.ThrowDeviceExceptionIfError();
                if ( value is null || string.IsNullOrWhiteSpace( value )
                    || !(value.StartsWith( expectedTrue, StringComparison.OrdinalIgnoreCase )
                        || value.StartsWith( expectedOne, StringComparison.OrdinalIgnoreCase )) )
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Expected {expectedOne} or {expectedTrue} but received {value}" );
            }
        }
        return userScriptNames;
    }

    #endregion

    #region " fetch saved scripts names "

    /// <summary>
    /// Fetches the names of the saved scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <returns>   The saved scripts. </returns>
    public static (string SavedScripts, List<string> SavedAuthorScripts) FetchSavedScriptsNames( this Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.LastNodeNumber = default;
        string scriptNames;
        List<string> authorScripts = [];
        session.SetLastAction( "fetching saved scripts" );
        _ = session.WriteLine( "do {0} print( names ) end ", Syntax.Tsp.Lua.ScriptCatalogGetterCommand );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

        scriptNames = session.ReadLineTrimEnd();

        if ( !string.IsNullOrWhiteSpace( scriptNames ) )
        {
            string[] scripts = scriptNames.Split( ',' );
            foreach ( string s in scripts )
            {
                if ( s.StartsWith( Author_Prefix, StringComparison.OrdinalIgnoreCase ) )
                    authorScripts.Add( s );
            }
        }

        return (scriptNames, authorScripts);
    }

    /// <summary>
    /// Fetches the names of the saved scripts.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the subsystem node. </param>
    /// <returns>   The saved scripts. </returns>
    public static string FetchSavedScriptsNames( this Pith.SessionBase? session, int nodeNumber )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.SetLastAction( "fetching catalog" );
        session.LastNodeNumber = nodeNumber;
        _ = session.WriteLine( Syntax.Tsp.Node.ValueGetterWaitCommandFormat2, nodeNumber, Syntax.Tsp.Lua.ScriptCatalogGetterCommand, "names" );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );
        session.LastNodeNumber = default;
        return session.ReadLineTrimEnd();
    }

    /// <summary>   Fetches the names of the saved scripts. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="node">     Specifies a node on which to operate; null if this is the controller node. </param>
    /// <returns>   The saved scripts. </returns>
    public static (string SavedScripts, List<string> AuthorScripts) FetchSavedScriptsNames( this Pith.SessionBase? session, NodeEntityBase? node )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        string scriptNames;
        List<string> authorScripts = [];
        session.SetLastAction( "fetching saved scripts" );
        session.LastNodeNumber = node.Number;
        if ( node is null || node.IsController )
            _ = session.WriteLine( "do {0} print( names ) end ", Syntax.Tsp.Lua.ScriptCatalogGetterCommand );
        else
            _ = session.WriteLine( Syntax.Tsp.Node.ValueGetterWaitCommandFormat2, node.Number, Syntax.Tsp.Lua.ScriptCatalogGetterCommand, "names" );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );
        scriptNames = session.ReadLineTrimEnd();
        if ( !string.IsNullOrWhiteSpace( scriptNames ) )
        {
            string[] scripts = scriptNames.Split( ',' );
            foreach ( string s in scripts )
            {
                if ( s.StartsWith( Author_Prefix, StringComparison.OrdinalIgnoreCase ) )
                    authorScripts.Add( s );
            }
        }
        return (scriptNames, authorScripts);
    }

    #endregion
}
