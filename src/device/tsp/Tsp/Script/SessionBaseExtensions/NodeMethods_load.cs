namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{
    /// <summary>   A Pith.SessionBase extension method that query if 'script' name is nil. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="script">       The script. </param>
    /// <returns>   True if nil, false if not. </returns>
    public static bool IsLoaded( this Pith.SessionBase session, int nodeNumber, ScriptInfo script )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        return session.IsControllerNode( nodeNumber )
            ? session.IsNil( script.Title )
            : session.IsNil( nodeNumber, script.Title );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that query if 'session' is loaded. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <returns>   True if loaded, false if not. </returns>
    public static bool IsLoaded( this Pith.SessionBase session, int nodeNumber, string scriptName )
    {
        return !session.IsNil( nodeNumber, scriptName );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that any loaded. </summary>
    /// <remarks>
    /// 2025-04-25. <para>
    /// NOTE: This method assumes that the scripts are already filtered for the specific instrument
    /// thus not requiring to match the script to the instrument based on the model mask as was
    /// previously done.
    /// </para>
    /// </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="scripts">      The scripts. </param>
    /// <returns>   True if any script is loaded. </returns>
    public static bool AnyLoaded( this Pith.SessionBase session, int nodeNumber, ScriptInfoBaseCollection<ScriptInfo> scripts )
    {
        bool affirmative = false;
        foreach ( ScriptInfo script in scripts )
        {
            affirmative = !session.IsNil( nodeNumber, script.Title );
            if ( affirmative ) break;
        }
        return affirmative;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that any loaded. </summary>
    /// <remarks>
    /// 2025-04-25. <para>
    /// NOTE: This method assumes that the scripts are already filtered for the specific instrument
    /// thus not requiring to match the script to the instrument based on the model mask as was
    /// previously done.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="scripts">  The scripts. </param>
    /// <returns>   True if any script is loaded. </returns>
    public static bool AnyLoaded( this Pith.SessionBase session, NodesScriptsCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        bool affirmative = false;
        foreach ( ScriptInfoCollection scriptInfoCollection in scripts.Values )
        {
            if ( scriptInfoCollection is not null )
            {
                affirmative = (scriptInfoCollection.NodeNumber == 0) || (scriptInfoCollection.NodeNumber == session.QueryControllerNodeNumber())
                    ? session.AnyLoaded( scriptInfoCollection )
                    : session.AnyLoaded( scriptInfoCollection.NodeNumber, scriptInfoCollection );
                if ( affirmative ) break;

            }
        }
        return affirmative;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that all loaded. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="scripts">      The scripts. </param>
    /// <returns>   True if all scripts loaded; otherwise, false. </returns>
    public static bool AllLoaded( this Pith.SessionBase session, int nodeNumber, ScriptInfoBaseCollection<ScriptInfo> scripts )
    {
        bool affirmative = true;
        foreach ( ScriptInfo script in scripts )
        {
            affirmative = !session.IsNil( nodeNumber, script.Title );
            if ( !affirmative ) break;
        }
        return affirmative;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that all loaded. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="scripts">  The scripts. </param>
    /// <returns>   True if all scripts loaded; otherwise, false. </returns>
    public static bool AllLoaded( this Pith.SessionBase session, NodesScriptsCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        bool affirmative = true;
        foreach ( ScriptInfoCollection scriptInfoCollection in scripts.Values )
        {
            if ( scriptInfoCollection is not null )
            {
                affirmative = (scriptInfoCollection.NodeNumber == 0) || (scriptInfoCollection.NodeNumber == session.QueryControllerNodeNumber())
                    ? session.AllLoaded( scriptInfoCollection )
                    : session.AllLoaded( scriptInfoCollection.NodeNumber, scriptInfoCollection );
                if ( !affirmative ) break;

            }
        }
        return affirmative;
    }


}
