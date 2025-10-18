using System.Collections.ObjectModel;
using System.Diagnostics;
using cc.isr.Std.TrimExtensions;
using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script;

/// <summary>
/// A <see cref="System.Collections.ObjectModel.KeyedCollection{TKey, TItem}">collection</see> of
/// <see cref="ScriptEntityBase">script entity</see>
/// items keyed by the <see cref="ScriptEntityBase.Name">name.</see>
/// </summary>
/// <remarks>   David, 2009-03-02, 3.0.3348.x <para>
///             (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.</para><para>
///             Licensed under The MIT License.</para> </remarks>
/// <remarks>   Constructor. </remarks>
/// <remarks>   2024-09-09. </remarks>
/// <param name="node">         Specifies the node. </param>
/// <typeparam name="TItem">    Type of the item. </typeparam>
[CLSCompliant( false )]
public class ScriptEntityBaseCollection<TItem>( NodeEntityBase node ) : System.Collections.ObjectModel.KeyedCollection<string, TItem> where TItem : ScriptEntityBase
{
    #region " construction and cleanup "

    /// <summary>   Constructor. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="scripts">  The scripts. </param>
    public ScriptEntityBaseCollection( ScriptEntityBaseCollection<ScriptEntityBase> scripts ) : this( scripts.Node )
    {
        foreach ( ScriptEntityBase script in scripts )
        {
            _ = this.AddScriptItem( new ScriptEntity( script ) );
        }
        this.ControllerNodeNumber = scripts.ControllerNodeNumber;
    }

    #endregion

    #region " select script "

    /// <summary>   Gets key for item. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="item"> The item. </param>
    /// <returns>   The key for item. </returns>
    protected override string GetKeyForItem( TItem item )
    {
        return item.Name + item.ModelMask;
    }

    /// <summary>
    /// Returns reference to the boot script for the specified node or nothing if a boot script does
    /// not exist.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="node"> Specifies the node. </param>
    /// <returns>
    /// Reference to the boot script for the specified node or nothing if a boot script does not
    /// exist.
    /// </returns>
    public TItem? SelectBootScript( NodeEntityBase? node )
    {
        if ( node is not null )
        {
            foreach ( TItem script in this.Items )
            {
                if ( script.IsModelMatch( node.ModelNumber ) && script.FirmwareScript.IsAutoexecScript )
                    return script;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns reference to the Serial Number script for the specified node or nothing if a serial
    /// number script does not exist.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="node"> Specifies the node. </param>
    /// <returns>
    /// Reference to the Serial Number script for the specified node or nothing if a serial number
    /// script does not exist.
    /// </returns>
    public TItem? SelectSerialNumberScript( NodeEntityBase? node )
    {
        if ( node is null ) return null;

        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( node.ModelNumber ) && script.FirmwareScript.IsAutoexecScript )
                return script;
        }

        return null;
    }

    /// <summary>
    /// Returns reference to the support script for the specified node or nothing if a support script
    /// does not exist.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="node"> The node. </param>
    /// <returns>
    /// Reference to the support script for the specified node or nothing if a support script does
    /// not exist.
    /// </returns>
    public TItem? SelectSupportScript( NodeEntityBase? node )
    {
        if ( node is null ) return null;

        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( node.ModelNumber ) && script.FirmwareScript.IsSupportScript )
                return script;
        }

        return null;
    }

    #endregion

    #region " Add script "

    /// <summary>
    /// Adds an object to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1"></see>.
    /// </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="script">   The object to be added to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1">
    ///                         </see>. The value can be null for reference types. </param>
    /// <returns>   A var. </returns>
    public Collection<TItem> AddScriptItem( ScriptEntityBase script )
    {
        return this.AddScriptItem( ( TItem ) script );
    }

    /// <summary>
    /// Adds an object to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1"></see>.
    /// </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <param name="script">   The object to be added to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1">
    ///                         </see>. The value can be null for reference types. </param>
    public Collection<TItem> AddScriptItem( TItem script )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.IsModelMatch( this.Node.ModelNumber ) )
        {
            script.Node = this.Node;
            this.Items.Add( script );
            return this;
        }
        else
            throw new NativeException(
                $"{script.Name} script model mask {script.ModelMask} does not match the collection node model number {this.Node.ModelNumber} " );
    }

    #endregion

    #region " firmware state "

    /// <summary>   Gets or sets the node. </summary>
    /// <value> The node. </value>
    public NodeEntityBase Node { get; private set; } = node;

    /// <summary>   Gets or sets the controller node number. </summary>
    /// <value> The controller node number. </value>
    public int ControllerNodeNumber { get; set; }

    /// <summary>   Gets the model number. </summary>
    /// <value> The model number. </value>
    public string ModelNumber => this.Node is null ? string.Empty : this.Node.ModelNumber;

    /// <summary> The identified script. </summary>

    /// <summary>   Gets any script identified using the test methods. </summary>
    /// <value> The identified script. </value>
    public TItem? IdentifiedScript { get; private set; }

    /// <summary>   Reads the loaded, activated, embedded and version of all scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  Specifies the TSP session. </param>
    public void ReadScriptsState( Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        this.FetchEmbeddedScriptsNames( session );
        foreach ( TItem script in this.Items )
            session.ReadScriptState( script, this.EmbeddedScriptNames );
    }

    /// <summary>
    /// Gets or sets the condition indicating if scripts that are newer than the scripts specified by
    /// the program can be isNull. This is required to allow the program install the scripts it
    /// considers current.
    /// </summary>
    /// <value> The allow deleting newer scripts. </value>
    public bool AllowDeletingNewerScripts { get; set; }

    #endregion

    #region " report firmware state "

    /// <summary>   Builds script state omissions. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <returns>   A string. </returns>
    public string BuildScriptStateOmissions()
    {
        System.Text.StringBuilder builder = new();
        _ = builder.AppendLine( $"Info for '{this.ModelNumber}' SN {this.Node.SerialNumber} node {this.Node.Number}:" );
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            if ( !string.IsNullOrWhiteSpace( script.Name ) && script.IsModelMatch( this.ModelNumber ) )
            {
                _ = builder.AppendLine( $"Info for script '{script.Name}':" );
                if ( script.Loaded )
                {
                    if ( script.Activated )
                    {
                        if ( script.HasFirmwareVersionGetter )
                        {
                            if ( string.IsNullOrWhiteSpace( script.EmbeddedFirmwareVersion ) )
                                _ = builder.AppendLine( $"\tno firmware version." );
                        }
                        else
                            _ = builder.AppendLine( $"\tno firmware version getter." );
                    }
                    else
                        _ = builder.AppendLine( $"\tnot activated." );

                    if ( !script.Embedded )
                        _ = builder.AppendLine( $"\tnot embedded." );
                }
                else
                    _ = builder.AppendLine( $"\tnot loaded." );

            }
        }
        return builder.ToString().TrimEndNewLine();
    }

    /// <summary>   Builds script state report. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <returns>   A string. </returns>
    public string BuildScriptStateReport()
    {
        System.Text.StringBuilder builder = new();
        _ = builder.AppendLine( $"Info for '{this.ModelNumber}' SN {this.Node.SerialNumber} node {this.Node.Number}:" );
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            if ( string.IsNullOrWhiteSpace( script.Name ) )
                _ = builder.AppendLine( "Script name is empty " );

            else if ( script.IsModelMatch( this.ModelNumber ) )
            {
                _ = builder.AppendLine( $"Info for script '{script.Name}':" );
                _ = builder.AppendLine( $"\tReleased version: {script.FirmwareScript.FirmwareVersion}." );
                if ( script.Loaded )
                {
                    if ( script.LoadedAsByteCode )
                        _ = builder.AppendLine( $"\tLoaded as byte code." );
                    else
                        _ = builder.AppendLine( $"\tLoaded." );

                    if ( script.Activated )
                    {
                        _ = builder.AppendLine( $"\tActivated." );
                        if ( script.HasFirmwareVersionGetter )
                        {
                            _ = builder.AppendLine( $"\tHas firmware version getter." );

                            if ( string.IsNullOrWhiteSpace( script.EmbeddedFirmwareVersion ) )
                                _ = builder.AppendLine( $"\tNo firmware version." );
                            else
                                _ = builder.AppendLine( $"\tEmbedded version: {script.EmbeddedFirmwareVersion}" );
                        }
                        else
                            _ = builder.AppendLine( $"\tNo firmware version getter." );
                    }
                    else
                        _ = builder.AppendLine( $"\tNot activated." );

                    if ( script.Embedded )
                        _ = builder.AppendLine( $"\tEmbedded." );
                    else
                        _ = builder.AppendLine( $"\tNot embedded." );
                }
                else
                    _ = builder.AppendLine( $"\tNot loaded." );

                switch ( script.VersionStatus )
                {
                    case FirmwareVersionStatus.Current:
                        {
                            _ = builder.AppendLine( $"\tThe embedded firmware is current." );
                            break;
                        }

                    case FirmwareVersionStatus.Missing:
                        {
                            _ = builder.AppendLine( $"\tThe version function not defined." );
                            break;
                        }

                    case FirmwareVersionStatus.Newer:
                        {
                            _ = builder.AppendLine( $"\tOutdated Program: The embedded firmware {script.EmbeddedFirmwareVersion} is newer than the released firmware {script.FirmwareScript.FirmwareVersion}. A newer version of this program is available." );
                            break;
                        }

                    case FirmwareVersionStatus.Older:
                        {
                            _ = builder.AppendLine( $"\tOutdated Firmware: The embedded firmware {script.EmbeddedFirmwareVersion} is older than the released firmware {script.FirmwareScript.FirmwareVersion}." );
                            break;
                        }

                    case FirmwareVersionStatus.NextVersionNotSet:
                        {
                            _ = builder.AppendLine( $"\tThe released firmware version is not specified." );
                            break;
                        }

                    case FirmwareVersionStatus.Unknown:
                        {
                            _ = builder.AppendLine( $"\tThe embedded firmware version is not known." );
                            break;
                        }

                    case FirmwareVersionStatus.None:
                        _ = builder.AppendLine( $"\tThe version status was not set." );
                        break;

                    default:
                        {
                            _ = builder.AppendLine( $"The case {script.VersionStatus} was unhandled when reporting the firmware version status." );
                            break;
                        }
                }

            }
        }
        return builder.ToString().TrimEndNewLine();
    }

    #endregion

    #region " determine firmware state "

    /// <summary>   Returns <c>true</c> if all script versions are current. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   <c>true</c> if all script versions are current. </returns>
    public bool AllVersionsCurrent()
    {
        bool affirmative = true;
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( this.ModelNumber ) && !script.FirmwareScript.IsAutoexecScript )
            {
                if ( script.VersionStatus != FirmwareVersionStatus.Current )
                {
                    this.IdentifiedScript = script;
                    affirmative = false;
                    break;
                }
            }
        }

        return affirmative;
    }

    /// <summary>   All embedded. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool AllEmbedded()
    {
        bool affirmative = true;
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( this.ModelNumber ) && !script.FirmwareScript.IsAutoexecScript )
            {
                if ( !script.Embedded )
                {
                    this.IdentifiedScript = script;
                    affirmative = false;
                    break;
                }
            }
        }

        return affirmative;
    }

    /// <summary>   All scripts exist (loaded). </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool AllExist()
    {
        bool affirmative = true;
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( this.ModelNumber ) && !script.FirmwareScript.IsAutoexecScript )
            {
                if ( !script.Loaded )
                {
                    this.IdentifiedScript = script;
                    affirmative = false;
                    break;
                }
            }
        }

        return affirmative;
    }

    /// <summary>   Any exist. </summary>
    /// <remarks>   2024-09-13. </remarks>
    /// <returns>   True if any script is loaded. </returns>
    public bool AnyExist()
    {
        bool affirmative = false;
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( this.ModelNumber ) && !script.FirmwareScript.IsAutoexecScript )
            {
                if ( script.Loaded )
                {
                    this.IdentifiedScript = script;
                    affirmative = true;
                    break;
                }
            }
        }

        return affirmative;
    }

    /// <summary>   Returns <c>true</c> if any script has an unspecified version. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   <c>true</c> if any script has an unspecified version. </returns>
    public bool VersionsUnspecified()
    {
        bool anyVersionUnspecified = false;
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( this.ModelNumber ) && !script.FirmwareScript.IsAutoexecScript )
            {
                if ( script.VersionStatus is FirmwareVersionStatus.Missing or FirmwareVersionStatus.Unknown )
                {
                    this.IdentifiedScript = script;
                    anyVersionUnspecified = true;
                    break;
                }
            }
        }

        return anyVersionUnspecified;
    }

    /// <summary>
    /// Returns <c>true</c> if any script version is newer than its released version.
    /// </summary>
    /// <remarks>   2024-07-09. </remarks>
    /// <returns>   <c>true</c> if any script version is newer than its released version. </returns>
    public bool IsProgramOutdated()
    {
        bool outdated = false;
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            if ( script.IsModelMatch( this.ModelNumber ) && !script.FirmwareScript.IsAutoexecScript && FirmwareVersionStatus.Newer == script.VersionStatus )
            {
                this.IdentifiedScript = script;
                outdated = true;
                break;
            }
        }
        return outdated;
    }

    /// <summary>   Returns <c>true</c> if any script requires update from file. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   <c>true</c> if any script requires update from file. </returns>
    public bool RequiresReadParseWrite()
    {
        bool requires = false;
        foreach ( TItem script in this.Items )
        {
            if ( script.FirmwareScript.RequiresReadParseWrite )
            {
                requires = true;
                break;
            }
        }

        return requires;
    }

    #endregion

    #region " ask about script state "

    /// <summary>   Checks if delete is required on any user script. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="allowDeletingNewerScripts">    True to allow, false to suppress the deleting
    ///                                             newer scripts. </param>
    /// <returns>   <c>true</c> if delete is required on any user script. </returns>
    public bool IsDeleteUserScriptRequired( bool allowDeletingNewerScripts )
    {
        bool required = false;
        foreach ( TItem script in this.Items )
        {
            if ( !script.FirmwareScript.IsAutoexecScript && (script.RequiresDeletion || allowDeletingNewerScripts) )

                if ( script.FirmwareScript.RequiresReadParseWrite )
                {
                    // stop in design time to make sure delete is not incorrect.
                    required = true;
                    Debug.Assert( !(required && Debugger.IsAttached), "ARE YOU SURE?" );
                    break;
                }
        }
        return required;
    }

    #endregion

    #region " firmware actions: delete "

    /// <summary>   Removes the script from the device. Updates the script list. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      Specifies the <see cref="VI.Pith.SessionBase">TSP session.</see> </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <returns>   <c>true</c> if the script is nil; otherwise <c>false</c>. </returns>
    public bool RemoveScript( Pith.SessionBase session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        bool isNull = this.DeleteScript( session, scriptName );
        if ( isNull )
        {
            if ( !session.CollectGarbageQueryComplete() )
                _ = session.TraceWarning( message: $"garbage collection incomplete (reply not '1') after removing script {scriptName}" );

            _ = session.TraceDeviceExceptionIfError( failureMessage: "ignoring error after removing script." );
        }
        return isNull;
    }

    /// <summary>
    /// Deletes the <paramref scriptName="scriptName">specified</paramref> script. Also nulls the
    /// script if delete command worked. Call <see cref="ScriptEntityBaseCollection{TItem}.FetchEmbeddedScriptsNames(SessionBase)"/>
    /// to refresh the list of names.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      Specifies the <see cref="VI.Pith.SessionBase">TSP session.</see> </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   <c>true</c> if the script is nil; otherwise <c>false</c>. </returns>
    public bool DeleteScript( Pith.SessionBase? session, string? scriptName )
    {
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        bool isNull = true;
        // check if the script name exists.
        if ( !session.IsNil( scriptName ) )
        {
            // check if embedded script exists
            this.FetchEmbeddedScriptsNames( session );
            if ( FirmwareScriptBase.ScriptNameExists( this.EmbeddedScriptNames, scriptName ) )
            {
                session.DeleteScript( scriptName, true );
                isNull = session.IsNil( scriptName );
            }
            else
            {
                session.NillObject( scriptName );
                isNull = true;
            }
        }
        return isNull;
    }

    #endregion

    #region " firmware actions: run "

    /// <summary>
    /// Runs existing scripts if they did not ran so their versions can be checked. Running exits
    /// after the first script that failed running assuming that scripts depend on previous scripts.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  Specifies the TSP session. </param>
    /// <param name="refresh">  True to refresh the state of the scripts. </param>
    /// <returns>   <c>true</c> if okay; <c>false</c> if any exception had occurred. </returns>
    public (bool Success, string Details) RunScripts( Pith.SessionBase? session, bool refresh )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        if ( refresh ) this.ReadScriptsState( session );

        string details = string.Empty;
        bool success = true;
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            try
            {
                session.RunScript( script );
            }
            catch ( Exception )
            {
                details = script.LastScriptManagerActions;
                success = false;
            }
            if ( !success )
                // exit the loop if any script failed to run.
                break;
        }

        return (success, details);
    }

    #endregion

    #region " firmware actions: query versions  "

    /// <summary>   Queries firmware versions. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  Specifies the <see cref="VI.Pith.SessionBase">TSP session.</see> </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool QueryFirmwareVersions( Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        bool success = false;
        this.IdentifiedScript = null;
        foreach ( TItem script in this.Items )
        {
            string value = script.QueryFirmwareVersion( session );

            success &= string.IsNullOrWhiteSpace( value );
            if ( !success )
                // exit the loop if any script failed to read the firmware version.
                break;
        }

        return success;
    }

    #endregion

    #region " Clear Known state "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    public void DefineKnownResetState()
    {
        this.LastFetchedScriptName = string.Empty;
        this.LastFetchedScriptSource = string.Empty;
        this.EmbeddedScriptNames = string.Empty;
        this._embeddedAuthorScriptNames = [];
    }

    #endregion

    #region " Last Fetched script source "

    /// <summary>   Gets the name of the last fetched script. </summary>
    /// <value> The name of the last fetched script. </value>
    public string LastFetchedScriptName { get; set; } = string.Empty;

    /// <summary>   Gets or sets the last fetched script source. </summary>
    /// <value> The last fetched script source. </value>
    public string LastFetchedScriptSource { get; set; } = string.Empty;

    #endregion

    #region " loaded and embedded script names "

    /// <summary>   Gets or sets a the names of the embedded scripts. </summary>
    /// <value> A list of names of the embedded scripts. </value>
    public string EmbeddedScriptNames { get; set; } = string.Empty;

    /// <summary> The last fetched names of the embedded author scripts. </summary>
    private List<string> _embeddedAuthorScriptNames = [];

    /// <summary>   Last fetched names of the author scripts. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   A list of strings. </returns>
    public System.Collections.ObjectModel.ReadOnlyCollection<string> EmbeddedAuthorScriptNames()
    {
        return this._embeddedAuthorScriptNames is null
            ? new System.Collections.ObjectModel.ReadOnlyCollection<string>( [] )
            : new System.Collections.ObjectModel.ReadOnlyCollection<string>( this._embeddedAuthorScriptNames );
    }

    /// <summary>   Fetches the list of embedded scripts. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <param name="session">  Specifies the TSP session. </param>
    public void FetchEmbeddedScriptsNames( Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        (this.EmbeddedScriptNames, this._embeddedAuthorScriptNames) = session.FetchEmbeddedScriptsNames( this.Node );
    }

    private string[] _loadedScriptNames = [];

    /// <summary>   Last fetched loaded script names. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   A list of strings. </returns>
    public System.Collections.ObjectModel.ReadOnlyCollection<string> LoadedScriptNames()
    {
        return this._loadedScriptNames is null
            ? new System.Collections.ObjectModel.ReadOnlyCollection<string>( [] )
            : new System.Collections.ObjectModel.ReadOnlyCollection<string>( this._loadedScriptNames );
    }

    /// <summary>   Fetches loaded scripts names. </summary>
    /// <remarks>   2024-09-19. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  Specifies the TSP session. </param>
    public void FetchLoadedScriptsNames( Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        this._loadedScriptNames = session.FetchUserScriptNames();
    }

    /// <summary>
    /// Checks if the specified script exists as a embedded script in the <see cref="ScriptEntityBaseCollection{ScriptEntityBase}.EmbeddedScriptNames"/>.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="session">              Specifies the <see cref="VI.Pith.SessionBase">TSP
    ///                                     session.</see> </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <param name="refreshScriptCatalog"> (Optional) True to refresh the list of embedded scripts. </param>
    /// <returns>
    /// <c>true</c> if the specified script exists as a embedded script; otherwise, <c>false</c>.
    /// </returns>
    public bool EmbeddedScriptExists( Pith.SessionBase session, string scriptName, bool refreshScriptCatalog = false )
    {
        if ( refreshScriptCatalog )
            this.FetchEmbeddedScriptsNames( session );

        return FirmwareScriptBase.ScriptNameExists( this.EmbeddedScriptNames, scriptName );
    }

    /// <summary>
    /// Checks if the script needs to be embedded to non-volatile memory. Presumes list of embedded scripts
    /// was retrieved.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Specifies the script to embed. </param>
    /// <param name="embedToNonVolatileMemory">  True to embed to non volatile memory. </param>
    /// <param name="convertToByteCode">          Specifies the condition requesting saving the source
    ///                                         as byte code. </param>
    /// <param name="isBootScript">             Specifies the condition indicating if this is a boot
    ///                                         script. </param>
    /// <returns>   <c>true</c> if embed is required; otherwise, <c>false</c>. </returns>
    public bool IsEmbedRequired( Pith.SessionBase session, string scriptName, bool embedToNonVolatileMemory, bool convertToByteCode, bool isBootScript )
    {
        return string.IsNullOrWhiteSpace( scriptName )
            ? throw new ArgumentNullException( nameof( scriptName ) )
            : this.Node is null
                ? throw new ArgumentNullException( nameof( this.Node ) )
                : embedToNonVolatileMemory &&
                    ((convertToByteCode && !session.isByteCodeScript( scriptName, this.Node ).GetValueOrDefault( false ))
                    || !this.EmbeddedScriptExists( session, scriptName, false )
                    || (isBootScript && this.Node.BootScriptEmbedRequired));
    }

    /// <summary>
    /// checks if embed is required for the specified script. Presumes list of embedded scripts was
    /// retrieved.
    /// </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="script">   The object to be added to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1">
    ///                         </see>. The value can be null for reference types. </param>
    /// <returns>   <c>true</c> if embed required; otherwise, <c>false</c>. </returns>
    public bool IsEmbedRequired( Pith.SessionBase session, ScriptEntityBase script )
    {
        return this.IsEmbedRequired( session, script.Name, script.FirmwareScript.EmbedToNonVolatileMemory, script.FirmwareScript.ConvertToByteCode, script.FirmwareScript.IsAutoexecScript );
    }

    #endregion
}

