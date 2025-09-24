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
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        // ensure the script is loaded.
        if ( session.IsNil( scriptName ) )
            return false;

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
    public static void TurnOffAutoRun( this SessionBase session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        // ensure the script is loaded.
        if ( !session.IsNil( scriptName ) )
        {
            if ( session.IsAutoRun( scriptName ) )
                _ = session.WriteLine( $"script.user.scripts.{scriptName}.autorun = \"no\" {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} " );
            //  _ = session.WriteLine( $"{scriptName}.autorun = \"no\" {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand}";
        }
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
    public static void TurnOnAutoRun( this SessionBase session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        // ensure the script is loaded.
        if ( !session.IsNil( scriptName ) )
        {
            if ( !session.IsAutoRun( scriptName ) )
                _ = session.WriteLine( $"script.user.scripts.{scriptName}.autorun = \"yes\" {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} " );
            //  _ = session.WriteLine( $"{scriptName}.autorun = \"yes\" {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand}";
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that queries firmware version.
    /// </summary>
    /// <remarks>   2025-05-08. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="versionGetter">    The version getter. </param>
    /// <returns>   The firmware version or <see cref="Syntax.Tsp.Lua.NilValue"/>. </returns>
    public static string QueryFirmwareVersion( this Pith.SessionBase session, string versionGetter )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        return session.IsNil( versionGetter.TrimEnd( "()".ToCharArray() ) )
            ? Syntax.Tsp.Lua.NilValue
            : session.QueryTrimEnd( Syntax.Tsp.Lua.PrintCommand( versionGetter ) );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that queries firmware version. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="script">   The script. </param>
    /// <returns>   The firmware version. </returns>
    public static string QueryFirmwareVersion( this Pith.SessionBase session, ScriptInfo script )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        return session.IsNil( script.VersionGetterElement )
            ? Syntax.Tsp.Lua.NilValue
            : session.QueryTrimEnd( Syntax.Tsp.Lua.PrintCommand( script.VersionGetter ) );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that queries firmware version.
    /// </summary>
    /// <remarks>   2025-04-26. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="scripts">  The scripts. </param>
    public static void QueryFirmwareVersion( this SessionBase session, ScriptInfoCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        foreach ( ScriptInfo script in scripts )
            script.EmbeddedVersion = session.QueryFirmwareVersion( script );
    }

    /// <summary>
    /// A <see cref="ScriptInfo"/> extension method that parses the firmware version based on the
    /// script actually embedded version and next version.
    /// </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <param name="script">   The script. </param>
    /// <returns>   The <see cref="FirmwareVersionStatus"/>. </returns>
    public static FirmwareVersionStatus ParseFirmwareVersionStatis( this ScriptInfo script )
    {
        if ( string.IsNullOrWhiteSpace( script.NextVersion ) )
            return FirmwareVersionStatus.NextVersionNotSet;

        else if ( string.IsNullOrWhiteSpace( script.EmbeddedVersion ) )
            return FirmwareVersionStatus.Unknown;

        else if ( (script.EmbeddedVersion ?? "") == Syntax.Tsp.Lua.NilValue )
            return FirmwareVersionStatus.Missing;

        else
        {
            switch ( new Version( script.EmbeddedVersion ).CompareTo( new Version( script.NextVersion ) ) )
            {
                case var @case when @case > 0:
                    {
                        return FirmwareVersionStatus.Newer;
                    }

                case 0:
                    {
                        return FirmwareVersionStatus.Current;
                    }

                default:
                    {
                        return FirmwareVersionStatus.Older;
                    }
            }
        }
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that reads script state running the script if it was not run. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="script">       The script. </param>
    /// <returns>   The script state. </returns>
    public static ScriptInfo ReadScriptState( this Pith.SessionBase session, ScriptInfo script )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        ScriptInfo embeddedScript = new( script )
        {
            // clear the script state
            ScriptStatus = ScriptStatuses.Unknown,
            EmbeddedVersion = string.Empty
        };

        if ( !session.IsNil( embeddedScript.Title ) )
        {
            embeddedScript.ScriptStatus = ScriptStatuses.Loaded;

            // run if version getter element was not found.
            if ( session.IsNil( embeddedScript.VersionGetterElement ) )
                session.RunScript( embeddedScript.Title );

            if ( session.IsNil( embeddedScript.VersionGetterElement ) )
            {
                embeddedScript.ScriptStatus = ScriptStatuses.Loaded;
            }
            else
            {
                embeddedScript.ScriptStatus |= ScriptStatuses.Activated;
                embeddedScript.EmbeddedVersion = session.QueryFirmwareVersion( script );
                if ( session.IsByteCodeScript( script.Title ) )
                    embeddedScript.ScriptStatus |= ScriptStatuses.ByteCode;
            }

            if ( session.IsSavedScript( script.Title ) )
            {
                embeddedScript.ScriptStatus |= ScriptStatuses.Saved;
            }
        }
        embeddedScript.VersionStatus = SessionBaseExtensionMethods.ParseFirmwareVersionStatis( embeddedScript );
        return embeddedScript;
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that reads script state. </summary>
    /// <remarks>   2025-04-26. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="scripts">  The scripts. </param>
    /// <returns>   The script state. </returns>
    public static ScriptInfoCollection ReadScriptState( this SessionBase session, ScriptInfoCollection scripts )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        ScriptInfoCollection embeddedScriptInfoCollection = new()
        {
            SerialNumber = scripts.SerialNumber,
            ModelNumber = scripts.ModelNumber,
            NodeNumber = scripts.NodeNumber
        };

        foreach ( ScriptInfo script in scripts )
        {
            embeddedScriptInfoCollection.Add( session.ReadScriptState( script ) );
        }
        return embeddedScriptInfoCollection;
    }
}
