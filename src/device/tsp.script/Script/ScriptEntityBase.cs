namespace cc.isr.VI.Tsp.Script;

/// <summary>   Provides the contract for a Script Entity. </summary>
/// <remarks>   David, 2009-03-02, 3.0.3348. <para>
///             (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. </para><para>
///             Licensed under The MIT License. </para> </remarks>
/// <remarks>   Specialized default constructor for use only by derived classes. </remarks>
/// <remarks>   2024-09-05. </remarks>
/// <param name="firmwareScript">   The firmware scripts associates with this script entity. </param>
/// <param name="node">             The node. </param>
[CLSCompliant( false )]
public abstract class ScriptEntityBase( FirmwareScriptBase firmwareScript, NodeEntityBase node ) : object()
{
    #region " firmware "

    /// <summary>   Gets or sets the node. </summary>
    /// <value> The node. </value>
    public NodeEntityBase Node { get; set; } = node;

    /// <summary>   Gets or sets the <see cref="FirmwareScriptBase"/> associated with this script entity. </summary>
    /// <value> The script firmware. </value>
    public FirmwareScriptBase FirmwareScript { get; set; } = firmwareScript;

    /// <summary>   Gets or sets the embedded firmware version. </summary>
    /// <value> The embedded firmware version. </value>
    public string EmbeddedFirmwareVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the firmware version getter function to be printed from the instrument.
    /// </summary>
    /// <value> The firmware version command. </value>
    public string FirmwareVersionGetter { get; set; } = string.Empty;

    /// <summary>   Gets or sets the script <see cref="FirmwareVersionStatus"/>. </summary>
    /// <value> The version status. </value>
    public FirmwareVersionStatus VersionStatus { get; set; }

    /// <summary>   Checks if the firmware version getter exists. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  Specifies reference to the Tsp Session. </param>
    /// <returns>
    /// <c>true</c> if the firmware version command exists; otherwise, <c>false</c>.
    /// </returns>
    private bool FirmwareVersionGetterExistsThis( Pith.SessionBase? session )
    {
        return session is null
            ? throw new ArgumentNullException( nameof( session ) )
            : !session.IsNil( this.FirmwareVersionGetter.TrimEnd( "()".ToCharArray() ) );
    }

    /// <summary>   Checks if the firmware version command exists. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="session">      Specifies reference to the Tsp Session. </param>
    /// <returns>
    /// <c>true</c> if the firmware version command exists; otherwise, <c>false</c>.
    /// </returns>
    private bool FirmwareVersionGetterExistsThis( int nodeNumber, Pith.SessionBase? session )
    {
        return session is null
            ? throw new ArgumentNullException( nameof( session ) )
            : !session.IsNil( nodeNumber, this.FirmwareVersionGetter.TrimEnd( "()".ToCharArray() ) );
    }

    /// <summary>   Checks if the firmware version command exists. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="node">     Specifies the node. </param>
    /// <param name="session">  Specifies reference to the Tsp Session. </param>
    /// <returns>
    /// <c>true</c> if the firmware version command exists; otherwise, <c>false</c>.
    /// </returns>
    private bool FirmwareVersionGetterExistsThis( NodeEntityBase? node, Pith.SessionBase? session )
    {
        return node is null
            ? throw new ArgumentNullException( nameof( node ) )
            : session is null
                ? throw new ArgumentNullException( nameof( session ) )
                : node.IsController
                    ? this.FirmwareVersionGetterExistsThis( session )
                    : this.FirmwareVersionGetterExistsThis( node.Number, session );
    }

    /// <summary>   Checks if the firmware version getter exists. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  Specifies reference to the Tsp Session. </param>
    /// <returns>
    /// <c>true</c> if the firmware version command exists; otherwise, <c>false</c>.
    /// </returns>
    public bool FirmwareVersionGetterExists( Pith.SessionBase? session )
    {
        return this.FirmwareVersionGetterExistsThis( this.Node, session );
    }

    /// <summary>
    /// Queries the embedded firmware version from a remote node and saves it to
    /// <see cref="EmbeddedFirmwareVersion">the firmware version cache.</see>
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  Specifies reference to the Tsp Session. </param>
    /// <returns>   The firmware version. </returns>
    private string QueryFirmwareVersionThis( Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        this.EmbeddedFirmwareVersion = this.FirmwareVersionGetterExistsThis( session )
            ? session.QueryTrimEnd( Syntax.Tsp.Lua.PrintCommand( this.FirmwareVersionGetter ) )
            : Syntax.Tsp.Lua.NilValue;
        return this.EmbeddedFirmwareVersion;
    }

    /// <summary>
    /// Queries the embedded firmware version from a remote node and saves it to
    /// <see cref="EmbeddedFirmwareVersion">the firmware version cache.</see>
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="nodeNumber">   The node number. </param>
    /// <param name="session">      Specifies reference to the Tsp Session. </param>
    /// <returns>   The firmware version. </returns>
    private string QueryFirmwareVersionThis( int nodeNumber, Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        this.EmbeddedFirmwareVersion = this.FirmwareVersionGetterExistsThis( nodeNumber, session )
            ? session.QueryPrintTrimEnd( nodeNumber, this.FirmwareVersionGetter ) ?? Syntax.Tsp.Lua.NilValue
            : Syntax.Tsp.Lua.NilValue;
        return this.EmbeddedFirmwareVersion;
    }

    /// <summary>
    /// Queries the embedded firmware version from a remote node and saves it to
    /// <see cref="EmbeddedFirmwareVersion">the firmware version cache.</see>
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="node">     Specifies the node. </param>
    /// <param name="session">  Specifies reference to the Tsp Session. </param>
    /// <returns>   The firmware version. </returns>
    private string QueryFirmwareVersionThis( NodeEntityBase? node, Pith.SessionBase? session )
    {
        return node is null
            ? throw new ArgumentNullException( nameof( node ) )
            : session is null
                ? throw new ArgumentNullException( nameof( session ) )
                : node.IsController
                    ? this.QueryFirmwareVersionThis( session )
                    : this.QueryFirmwareVersionThis( node.Number, session );
    }

    /// <summary>
    /// Queries the embedded firmware version from a remote node and saves it to
    /// <see cref="EmbeddedFirmwareVersion">the firmware version cache.</see>
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  Specifies reference to the Tsp Session. </param>
    /// <returns>   The embedded firmware version. </returns>
    public string QueryFirmwareVersion( Pith.SessionBase? session )
    {
        return this.QueryFirmwareVersionThis( this.Node, session );
    }

    /// <summary>   Validates the released against the embedded firmware. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   The <see cref="FirmwareVersionStatus">version status</see>. </returns>
    public FirmwareVersionStatus ValidateFirmware()
    {
        if ( string.IsNullOrWhiteSpace( this.FirmwareScript.FirmwareVersion ) )
            return FirmwareVersionStatus.ReleaseVersionNotSet;

        else if ( string.IsNullOrWhiteSpace( this.EmbeddedFirmwareVersion ) )
            return FirmwareVersionStatus.Unknown;

        else if ( (this.EmbeddedFirmwareVersion ?? "") == Syntax.Tsp.Lua.NilValue )
            return FirmwareVersionStatus.Missing;

        else
        {
            switch ( new Version( this.EmbeddedFirmwareVersion ).CompareTo( new Version( this.FirmwareScript.FirmwareVersion ) ) )
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

    #endregion

    #region " script management "

    /// <summary>   Gets or sets a value indicating whether the script is loaded. </summary>
    /// <remarks> The script is loaded if the script name is found in the instrument. </remarks>
    /// <value> True if the script is loaded, false if not. </value>
    public bool Loaded { get; set; }

    /// <summary>   Gets or sets a value indicating whether the script was loaded as a binary script. </summary>
    /// <value> True if the script is loaded as a binary script, false if not. </value>
    public bool LoadedAsBinary { get; set; }

    /// <summary>   Gets or sets a value indicating whether the script was activated by running this script. </summary>
    /// <value> True if the script was activated by running this script, false if not. </value>
    public bool Activated { get; set; }

    /// <summary>   Gets or sets a value indicating whether this script is saved in the instrument catalog. </summary>
    /// <value> True if this script is saved in the instrument catalog, false if not. </value>
    public bool Saved { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the script has a defined firmware version getter
    /// and the getter is not Nil.
    /// </summary>
    /// <value> True if the script has a firmware version getter, false if not. </value>
    public bool HasFirmwareVersionGetter { get; set; }

    /// <summary>   Gets or sets the condition indicating if this script was deleted. </summary>
    /// <value> The is deleted. </value>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the condition indicating if this scripts needs to be deleted on the instrument.
    /// At this time this is used in design mode. It might be used later for refreshing the stored
    /// scripts.
    /// </summary>
    /// <value> The requires deletion. </value>
    public bool RequiresDeletion { get; set; }

    /// <summary>   Checks if the script exits (loaded). </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <returns>   <c>true</c> is script exist; otherwise, <c>false</c>. </returns>
    public bool ScriptExists( Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        return this.Node.IsController
            ? !session.IsNil( this.Name )
            : !session.IsNil( this.Node.Number, this.Name );
    }

    /// <summary>   Gets or sets the last manager actions. </summary>
    /// <value> The last manager actions. </value>
    public string LastScriptManagerActions { get; set; } = string.Empty;

    /// <summary>   Query if this object is save required. </summary>
    /// <remarks>   2024-09-21. </remarks>
    /// <returns>   True if save required, false if not. </returns>
    public bool IsSaveRequired()
    {
        return !this.Saved || (this.FirmwareScript.SaveToNonVolatileMemory && this.LoadedAsBinary);
    }

    #endregion

    #region " model management "

    /// <summary>   Specifies the family of instrument models for this script. </summary>
    /// <value> The model mask. </value>
    public string ModelMask { get; private set; } = firmwareScript.ModelMask;

    /// <summary>
    /// Checks if the <paramref name="model">model</paramref> matches the
    /// <see cref="ModelMask">mask</see>.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="model">    The model. </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="model">model</paramref> matches the
    /// <see cref="ModelMask">mask</see>.
    /// </returns>
    public bool IsModelMatch( string model )
    {
        return FirmwareScriptBase.IsModelMatch( model, this.ModelMask );
    }

    #endregion

    #region " script specifications "

    /// <summary>   Gets the name of the script. </summary>
    /// <value> The name. </value>
    public string Name { get; private set; } = firmwareScript.Name;

    #endregion
}

/// <summary>
/// A <see cref="System.Collections.ObjectModel.KeyedCollection{TKey, TItem}">collection</see> of
/// <see cref="ScriptEntityBase">script entity</see>
/// items keyed by the <see cref="ScriptEntityBase.Name">name.</see>
/// </summary>
/// <remarks>
/// David, 2009-03-02, 3.0.3348. <para>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.</para><para>
/// Licensed under The MIT License.</para>
/// </remarks>
/// <param name="node">         The node. </param>
[CLSCompliant( false )]
public class ScriptEntityCollection( NodeEntityBase node ) : ScriptEntityBaseCollection<ScriptEntityBase>( node )
{
    /// <summary>   Clone constructor. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="scripts">  The scripts. </param>
    public ScriptEntityCollection( ScriptEntityCollection scripts ) : this( scripts.Node )
    {
        foreach ( ScriptEntityBase script in scripts )
        {
            _ = this.AddScriptItem( new ScriptEntity( script ) );
        }
        this.AllowDeletingNewerScripts = scripts.AllowDeletingNewerScripts;
    }

    /// <summary>   Gets key for item. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="item"> The item. </param>
    /// <returns>   The key for item. </returns>
    protected override string GetKeyForItem( ScriptEntityBase item )
    {
        return base.GetKeyForItem( item );
    }
}
