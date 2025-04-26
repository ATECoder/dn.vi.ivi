using cc.isr.VI.Pith;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that queries firmware version. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="script">       The script. </param>
    /// <returns>   The firmware version. </returns>
    public static string QueryFirmwareVersion( this Pith.SessionBase session, int nodeNumber, ScriptInfo script )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        return session.IsNil( nodeNumber, script.VersionGetterElement )
            ?  Syntax.Tsp.Lua.NilValue
            : session.QueryPrintTrimEnd( nodeNumber, script.VersionGetter ) ?? Syntax.Tsp.Lua.NilValue;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that reads script state. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="script">       The script. </param>
    /// <returns>   The script state. </returns>
    public static ScriptInfo ReadScriptState( this Pith.SessionBase session, int nodeNumber, ScriptInfo script )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        ScriptInfo embeddedScript = new( script )
        {
            // clear the script state
            ScriptStatus = ScriptStatuses.Unknown,
            Version = string.Empty
        };

        if ( !session.IsNil( nodeNumber, embeddedScript.Title ) )
        {
            embeddedScript.ScriptStatus = ScriptStatuses.Loaded;
            if ( session.IsNil( nodeNumber, embeddedScript.VersionGetterElement ) )
            {
                embeddedScript.ScriptStatus = ScriptStatuses.Loaded;
            }
            else
            {
                embeddedScript.ScriptStatus |= ScriptStatuses.Activated;
                embeddedScript.ActualVersion = session.QueryFirmwareVersion( nodeNumber, script );
                if ( session.IsBinaryScript( nodeNumber, script.Title ) )
                    embeddedScript.ScriptStatus |= ScriptStatuses.BinaryByteCode;
            }

            if ( session.IsSavedScript( script.Title, nodeNumber ) )
            {
                embeddedScript.ScriptStatus |= ScriptStatuses.Saved;
            }
        }
        embeddedScript.VersionStatus = SessionBaseExtensionMethods.ValidateFirmware( embeddedScript, script.Version );
        return embeddedScript;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that reads script state. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="scripts">      The scripts. </param>
    /// <returns>   The script state. </returns>
    public static ScriptInfoCollection ReadScriptState( this SessionBase session, int nodeNumber, ScriptInfoCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        ScriptInfoCollection embeddedScripts = [];

        session.LastNodeNumber = nodeNumber;
        foreach ( ScriptInfo script in scripts )
        {
            ScriptInfo embeddedScript = session.ReadScriptState( nodeNumber, script );
            embeddedScripts.Add( embeddedScript );
        }
        return embeddedScripts;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that reads script state. </summary>
    /// <remarks>   2025-04-26. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeScripts">  The node scripts. </param>
    /// <returns>   The script state. </returns>
    public static NodesScriptsCollection ReadScriptState( this SessionBase session, NodesScriptsCollection nodeScripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( nodeScripts is null ) throw new ArgumentNullException( nameof( nodeScripts ) );

        NodesScriptsCollection embeddedNodeScripts = [];
        foreach ( ScriptInfoCollection scriptInfoCollection in nodeScripts.Values )
        {
            if ( scriptInfoCollection is not null )
            {
                ScriptInfoCollection embeddedScriptInfoCollection = (scriptInfoCollection.NodeNumber == 0) || (scriptInfoCollection.NodeNumber == session.QueryControllerNodeNumber())
                    ? session.ReadScriptState( scriptInfoCollection )
                    : session.ReadScriptState( scriptInfoCollection.NodeNumber, scriptInfoCollection );
                embeddedNodeScripts.Add( scriptInfoCollection.NodeNumber, embeddedScriptInfoCollection );
            }
        }
        return embeddedNodeScripts;
    }
}
