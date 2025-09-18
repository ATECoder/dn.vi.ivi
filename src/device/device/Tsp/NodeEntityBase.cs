namespace cc.isr.VI;

/// <summary>   Provides an contract for a Node Entity. </summary>
/// <remarks>   (c) 2010 Integrated Scientific Resources, Inc. All rights reserved. <para>
///             Licensed under The MIT License. </para><para>
///             David, 2009-03-02, 3.0.3348.x. </para> </remarks>
/// <remarks>   Constructs the class. </remarks>
/// <param name="number">               Specifies the node number. </param>
/// <param name="controllerNode">       The controller node or null if the node is the controller node. </param>
[CLSCompliant( false )]
public abstract class NodeEntityBase( int number, NodeEntityBase? controllerNode ) : object()
{
    #region " construction "

    /// <summary> Builds a key. </summary>
    /// <param name="nodeNumber"> The node number. </param>
    /// <returns> The unique value for the node number. </returns>
    public static string BuildKey( int nodeNumber )
    {
        return nodeNumber.ToString();
    }

    #endregion

    #region " init "

    /// <summary> Initializes the node properties . </summary>
    /// <exception cref="FormatException"> Thrown when the format of the received message is
    /// incorrect. </exception>
    /// <param name="session"> The session. </param>
    public void InitializeKnownState( Pith.SessionBase session )
    {
        this.QueryModelNumber( session );
        if ( string.IsNullOrWhiteSpace( this.ModelFamily ) )
            throw new FormatException( $"Node #{this.Number} instrument model {this.ModelNumber} has no defined instrument family in {VersionInfoBase.ModelFamilies};. " );

        this.QueryFirmwareVersion( session );
        this.QuerySerialNumber( session );
    }

    #endregion

    #region " node info "

    /// <summary>   References to the construction controller node. </summary>
    private readonly NodeEntityBase? _controllerNode = controllerNode;

    /// <summary>
    /// Gets the reference to the assigned controller node which is this node if no controller node is assigned.
    /// </summary>
    /// <value> The controller node. </value>
    public NodeEntityBase ControllerNode => this._controllerNode is null ? this : this._controllerNode;

    /// <summary> Returns True if the <see cref="NodeEntityBase"/> is a controller node. </summary>
    /// <value> True if the <see cref="NodeEntityBase"/> is a controller node. </value>
    public bool IsController => this._controllerNode is null;

    /// <summary>   Gets or sets the controller node number. </summary>
    /// <value> The controller node number. </value>
    public int ControllerNodeNumber => this.ControllerNode.Number;

    /// <summary> Queries if a given node exists. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session">    The session. </param>
    /// <param name="nodeNumber"> The node number. </param>
    /// <returns> <c>true</c> is node exists; otherwise, <c>false</c>. </returns>
    public static bool NodeExists( Pith.SessionBase session, int nodeNumber )
    {
        return session is null ? throw new ArgumentNullException( nameof( session ) ) : !session.IsNil( $"node[{nodeNumber}]" );
    }

    /// <summary>
    /// Gets the condition to indicate that the boot script must be re-saved because a script
    /// reference changes as would happened if a new byte code script was created with a table reference
    /// that differs from the table reference of the previous script that was used in the previous
    /// boot script.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The boot script save required. </value>
    public bool BootScriptSaveRequired { get; set; }

    /// <summary> Gets the data queue capacity. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The data queue capacity. </value>
    public int? DataQueueCapacity { get; set; }

    /// <summary> Queries the data queue capacity. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session"> The session. </param>
    public void QueryDataQueueCapacity( Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.SetLastAction( "querying data queue capacity" );
        this.DataQueueCapacity = session.QueryPrint( 0, 1, $"node[{this.Number}].dataqueue.capacity" );
    }

    /// <summary> Gets the data queue Count. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The data queue Count. </value>
    public int? DataQueueCount { get; set; }

    /// <summary> Queries the data queue Count. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session"> The session. </param>
    public void QueryDataQueueCount( Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.SetLastAction( "querying data queue count" );
        this.DataQueueCount = session.QueryPrint( 0, 1, $"node[{this.Number}].dataqueue.count" );
    }

    /// <summary> Gets the firmware version. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The firmware version. </value>
    public string FirmwareVersion { get; set; } = string.Empty;

    /// <summary> Queries firmware version. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session"> The session. </param>
    public void QueryFirmwareVersion( Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.SetLastAction( "querying firmware revision" );
        this.FirmwareVersion = session.QueryTrimEnd( $"_G.print(node[{this.Number}].revision)" );
    }

    /// <summary> Gets the <see cref="InstrumentModelFamily">instrument model family.</see> </summary>
    /// <value> The instrument model family. </value>
    public InstrumentModelFamily InstrumentModelFamily { get; private set; } = InstrumentModelFamily.None;

    /// <summary>   Gets or sets the model family. </summary>
    /// <value> The model family. </value>
    public string ModelFamily { get; private set; } = string.Empty;

    /// <summary> Gets the model number. </summary>
    /// <value> The model number. </value>
    public string ModelNumber { get; private set; } = string.Empty;

    /// <summary> Queries the serial number. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session">    The session. </param>
    /// <param name="nodeNumber"> The node number. </param>
    /// <returns> The model number. </returns>
    public static string QueryModelNumber( Pith.SessionBase session, int nodeNumber )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.SetLastAction( "querying model number" );
        return session.QueryTrimEnd( $"_G.print(node[{nodeNumber}].model)" );
    }

    /// <summary> Queries controller node model. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session"> The session. </param>
    /// <returns> The controller node model. </returns>
    public static string QueryControllerNodeModel( Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.SetLastAction( "querying controller model number" );
        return session.QueryTrimEnd( "_G.print(_G.localnode.model)" );
    }

    /// <summary> Queries the serial number. </summary>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="session"> The session. </param>
    public void QueryModelNumber( Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        this.ModelNumber = this.IsController
            ? NodeEntityBase.QueryControllerNodeModel( session )
            : NodeEntityBase.QueryModelNumber( session, this.Number );
        if ( string.IsNullOrWhiteSpace( this.ModelNumber ) )
            throw new InvalidOperationException( "Failed reading node model--empty." );
        (this.ModelFamily, _) = VersionInfoBase.ParseModelFamily( this.ModelNumber, VersionInfoBase.ModelFamilyMasks, VersionInfoBase.ModelFamilies );
        this.InstrumentModelFamily = ParseModelNumber( this.ModelNumber );
    }

    /// <summary> Gets or sets the node number. </summary>
    /// <value> The node number. </value>
    public int Number { get; private set; } = number;

    /// <summary> Gets or sets the serial number. </summary>
    /// <value> The serial number. </value>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary> Queries the serial number. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session"> The session. </param>
    public void QuerySerialNumber( Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.SetLastAction( "querying serial number" );
        this.SerialNumber = session.QueryTrimEnd( $"_G.print(node[{this.Number}].serialno)" );
    }

    /// <summary> Gets or sets the unique key. </summary>
    /// <value> The unique key. </value>
    public string UniqueKey { get; private set; } = BuildKey( number );

    #endregion

    #region " instrument model "

    /// <summary> Parses the model number to get the model family. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> A <see cref="InstrumentModelFamily">model family</see>. </returns>
    public static InstrumentModelFamily ParseModelNumber( string value )
    {
        foreach ( InstrumentModelFamily item in Enum.GetValues( typeof( InstrumentModelFamily ) ) )
        {
            if ( item != 0 && VersionInfo.IsModelMatch( value, ModelFamilyMask( item ) ) )
            {
                return item;
            }
        }

        return InstrumentModelFamily.None;
    }

    /// <summary> Returns the mask for the model family. </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    /// the required range. </exception>
    /// <param name="value"> The value. </param>
    /// <returns> The mask for the model family. </returns>
    public static string ModelFamilyMask( InstrumentModelFamily value )
    {
        switch ( value )
        {
            case InstrumentModelFamily.K2600:
                {
                    return "26%%";
                }

            case InstrumentModelFamily.K2600A:
                {
                    return "26%%A";
                }

            case InstrumentModelFamily.K2600B:
                {
                    return "26%%B";
                }

            case InstrumentModelFamily.K3700:
                {
                    return "37*";
                }

            case InstrumentModelFamily.K2450:
                {
                    return "245%";
                }

            case InstrumentModelFamily.K6500:
                {
                    return "65%%";
                }

            case InstrumentModelFamily.K7500:
                {
                    return "75%%";
                }

            case InstrumentModelFamily.None:
                throw new ArgumentOutOfRangeException( nameof( value ), value, "Unhandled model family" );

            default:
                {
                    throw new ArgumentOutOfRangeException( nameof( value ), value, "Unhandled model family" );
                }
        }
    }

    /// <summary> Model family resource file suffix. </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    /// the required range. </exception>
    /// <param name="value"> The value. </param>
    /// <returns> The suffix for the model family resource file name. </returns>
    public static string ModelFamilyResourceFileSuffix( InstrumentModelFamily value )
    {
        switch ( value )
        {
            case InstrumentModelFamily.K2600:
                {
                    return ".2600";
                }

            case InstrumentModelFamily.K2600A:
                {
                    return ".2600A";
                }

            case InstrumentModelFamily.K2600B:
                {
                    return ".2600B";
                }

            case InstrumentModelFamily.K3700:
                {
                    return ".3700";
                }

            case InstrumentModelFamily.K2450:
                {
                    return ".2450";
                }

            case InstrumentModelFamily.K6500:
                {
                    return ".6500";
                }

            case InstrumentModelFamily.K7500:
                {
                    return ".7500";
                }

            case InstrumentModelFamily.None:
                throw new ArgumentOutOfRangeException( nameof( value ), value, "Unhandled model family" );

            default:
                {
                    throw new ArgumentOutOfRangeException( nameof( value ), value, "Unhandled model family" );
                }
        }
    }

    #endregion
}
/// <summary>
/// A <see cref="System.Collections.ObjectModel.KeyedCollection{TKey, TItem}">collection</see> of
/// <see cref="NodeEntityBase">Node entity</see>
/// items keyed by the <see cref="NodeEntityBase.UniqueKey">unique key.</see>
/// </summary>
[CLSCompliant( false )]
public class NodeEntityCollection : NodeEntityBaseCollection<NodeEntityBase>
{
    /// <summary> Gets key for item. </summary>
    /// <param name="item"> The item. </param>
    /// <returns> The key for item. </returns>
    protected override string GetKeyForItem( NodeEntityBase item )
    {
        return base.GetKeyForItem( item );
    }
}
/// <summary>
/// A <see cref="System.Collections.ObjectModel.KeyedCollection{TKey, TItem}">collection</see> of
/// <see cref="NodeEntityBase">Node entity</see>
/// items keyed by the <see cref="NodeEntityBase.UniqueKey">unique key.</see>
/// </summary>
[CLSCompliant( false )]
public class NodeEntityBaseCollection<TItem> : System.Collections.ObjectModel.KeyedCollection<string, TItem> where TItem : NodeEntityBase
{
    /// <summary> Gets key for item. </summary>
    /// <param name="item"> The item. </param>
    /// <returns> The key for item. </returns>
    protected override string GetKeyForItem( TItem item )
    {
        return item.UniqueKey;
    }
}
/// <summary> Enumerates the instrument model families. </summary>
public enum InstrumentModelFamily
{
    /// <summary>Not defined.</summary>
    [System.ComponentModel.Description( "Not defined" )]
    None = 0,

    /// <summary>26xx Source Meters.</summary>
    [System.ComponentModel.Description( "26xx Source Meters" )]
    K2600 = 1,

    /// <summary>26xxA Source Meters.</summary>
    [System.ComponentModel.Description( "26xxA Source Meters" )]
    K2600A = 2,

    /// <summary>26xxB Source Meters.</summary>
    [System.ComponentModel.Description( "26xxB Source Meters" )]
    K2600B = 3,

    /// <summary>37xx Switch Systems.</summary>
    [System.ComponentModel.Description( "37xx Switch Systems" )]
    K3700 = 4,

    /// <summary>24xx Source Meters.</summary>
    [System.ComponentModel.Description( "245x Source Meters" )]
    K2450 = 5,

    /// <summary>75xx meters.</summary>
    [System.ComponentModel.Description( "65xx Meters" )]
    K6500 = 6,

    /// <summary>75xx meters.</summary>
    [System.ComponentModel.Description( "75xx Meters" )]
    K7500 = 7
}
