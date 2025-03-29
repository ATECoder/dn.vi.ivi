using System.Diagnostics;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.VI.ExceptionExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp;

/// <summary> Defines a subsystem for handing TSP Link and multiple nodes. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-11-01. Based on legacy status subsystem. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SystemSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.Tsp.LinkStatusSubsystem">TSP status
/// Subsystem</see>. </param>
public abstract class LinkSubsystemBase( LinkStatusSubsystem statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Customizes the reset state. </remarks>
    public override void InitKnownState()
    {
        base.InitKnownState();
        // 2016-01-07: Moved from the Main Device.
        this.UsingTspLink = false;
        if ( this.IsControllerNode )
        {
            // establish the current node as the controller node.
            this.InitiateControllerNode();
        }
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this._nodeEntities = [];
        this.NotifyPropertyChanged( nameof( LinkSubsystemBase.NodeEntities ) );
        this.ControllerNodeNumber = new int?();
        this.ControllerNode = null;

        // these values are set upon loading or initializing the framework.
        this.TspLinkOnlineStateQueryCommand = string.Empty;
        this.TspLinkOfflineStateQueryCommand = string.Empty;
        this.TspLinkResetCommand = string.Empty;
        this.ResetNodesCommand = string.Empty;
        this.IsTspLinkOnline = new bool?();
        this.IsTspLinkOffline = new bool?();
    }

    #endregion

    #region " collect garbage "

    /// <summary>   Collect garbage wait complete. </summary>
    /// <remarks>   2024-08-20. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="node">                         Specifies the remote node number to validate. </param>
    /// <param name="operationCompletionTimeout">   Specifies the time to wait for the instrument to
    ///                                             return operation completed. </param>
    /// <returns>   <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public (ServiceRequests StatusByte, string Details) CollectGarbageWaitComplete( NodeEntityBase node, TimeSpan operationCompletionTimeout )
    {
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );

        this.Session.LastNodeNumber = node.Number;

        // enable wait completion
        this.Session.SetLastAction( "service requests for wait completion" );
        this.Session.EnableServiceRequestOnOperationCompletion();
        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
        ServiceRequests statusByte = this.Session.ReadStatusByte();
        if ( this.Session.IsErrorBitSet( statusByte ) )
            return (statusByte, $"Status error after {this.Session.LastAction}.");

        this.Session.SetLastAction( "writing the wait complete command" );
        if ( node.IsController )
            _ = this.Session.WriteLine( Syntax.Tsp.Lua.CollectGarbageOperationCompleteCommand );
        else
            _ = this.Session.WriteLine( Syntax.Tsp.Node.CollectGarbageOperationCompleteCommandFormat, node.Number );

        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
        statusByte = this.Session.ReadStatusByte();
        if ( this.Session.IsErrorBitSet( statusByte ) )
            return (statusByte, $"Status error enabling the service request after {this.Session.LastAction}.");

        statusByte = this.Session.AwaitOperationCompletion( operationCompletionTimeout ).Status;
        if ( this.Session.IsErrorBitSet( statusByte ) )
            return (statusByte, $"Status error awaiting completion after {this.Session.LastAction}.");
        else
            return (statusByte, string.Empty);

    }

    #endregion

    #region " data queue "

    /// <summary> clears the data queue for the specified node. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node">                Specifies the node. </param>
    /// <param name="reportQueueNotEmpty"> true to report queue not empty. </param>
    public void ClearDataQueue( NodeEntityBase node, bool reportQueueNotEmpty )
    {
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        if ( this.NodeExists( node.Number ) )
        {
            if ( reportQueueNotEmpty )
            {
                if ( this.QueryDataQueueCount( node ) > 0 )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Data queue not empty on node {node.Number};. " );
                }
            }

            _ = this.Session.WriteLine( "node[{0}].dataqueue.clear() waitcomplete({0})", node.Number );
        }
    }

    /// <summary> clears the data queue for the specified node. </summary>
    /// <param name="nodeNumber"> The node number. </param>
    public void ClearDataQueue( int nodeNumber )
    {
        if ( this.NodeExists( nodeNumber ) )
        {
            _ = this.Session.WriteLine( "node[{0}].dataqueue.clear() waitcomplete({0})", nodeNumber );
        }
    }

    /// <summary>   clears the data queue for the specified node. </summary>
    /// <remarks>   2024-09-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    public static void ClearDataQueue( Pith.SessionBase? session, int nodeNumber )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( NodeEntityBase.NodeExists( session, nodeNumber ) )
            _ = session.WriteLine( "node[{0}].dataqueue.clear() waitcomplete({0})", nodeNumber );
    }

    /// <summary> Clears data queue on all nodes. </summary>
    /// <param name="nodeEntities">        The node entities. </param>
    /// <param name="reportQueueNotEmpty"> true to report queue not empty. </param>
    public void ClearDataQueue( NodeEntityCollection nodeEntities, bool reportQueueNotEmpty )
    {
        if ( nodeEntities is not null )
        {
            foreach ( NodeEntityBase node in nodeEntities )
                this.ClearDataQueue( node, reportQueueNotEmpty );
        }
    }

    /// <summary> Clears data queue on the nodes. </summary>
    /// <param name="nodes"> The nodes. </param>
    public void ClearDataQueue( NodeEntityCollection nodes )
    {
        if ( nodes is not null )
        {
            foreach ( NodeEntityBase node in nodes )
            {
                if ( node is not null )
                {
                    this.ClearDataQueue( node.Number );
                }
            }
        }
    }

    /// <summary> Clears data queue on all nodes. </summary>
    public void ClearDataQueue()
    {
        this.ClearDataQueue( this.NodeEntities() );
    }

    #region " capacity "

    /// <summary> Queries the capacity of the data queue. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node"> . </param>
    /// <returns> Capacity. </returns>
    public int QueryDataQueueCapacity( NodeEntityBase node )
    {
        return node is null ? throw new ArgumentNullException( nameof( node ) ) : this.QueryDataQueueCapacity( node.Number );
    }

    /// <summary> Queries the capacity of the data queue. </summary>
    /// <param name="nodeNumber"> The node number. </param>
    /// <returns> Capacity. </returns>
    public int QueryDataQueueCapacity( int nodeNumber )
    {
        return this.NodeExists( nodeNumber ) ? this.Session.QueryPrint( 0, 1, $"node[{nodeNumber}].dataqueue.capacity" ) : 0;
    }

    #endregion

    #region " count "

    /// <summary> Queries the data queue count. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node"> . </param>
    /// <returns> Count. </returns>
    public int QueryDataQueueCount( NodeEntityBase node )
    {
        return node is null ? throw new ArgumentNullException( nameof( node ) ) : this.QueryDataQueueCount( node.Number );
    }

    /// <summary> Queries the data queue count. </summary>
    /// <param name="nodeNumber"> The node number. </param>
    /// <returns> Count. </returns>
    public int QueryDataQueueCount( int nodeNumber )
    {
        return this.NodeExists( nodeNumber ) ? this.Session.QueryPrint( 0, 1, $"node[{nodeNumber}].dataqueue.count" ) : default;
    }

    #endregion

    #endregion

    #region " node "

    /// <summary> Resets the local TSP node. </summary>
    /// <returns> <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public bool ResetNode()
    {
        this.Session.LastNodeNumber = default;
        Pith.ServiceRequests statusByte = Pith.ServiceRequests.None;
        string details;
        try
        {
            this.Session.SetLastAction( "resetting local TSP node" );
            _ = this.Session.WriteLine( "localnode.reset()" );
            _ = this.Session.TraceInformation();
            (statusByte, details) = this.Session.TraceDeviceExceptionIfError();
        }
        catch ( Pith.NativeException ex )
        {
            _ = this.Session.TraceException( ex );
        }
        catch ( Exception ex )
        {
            _ = this.Session.TraceException( ex );
        }

        return !this.Session.IsErrorBitSet( statusByte );
    }

    /// <summary> Resets a node. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node"> The node. </param>
    /// <returns> <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public bool ResetNode( NodeEntityBase node )
    {
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        Pith.ServiceRequests statusByte = Pith.ServiceRequests.None;
        string details;
        this.Session.LastNodeNumber = node.Number;
        if ( node.IsController )
            return this.ResetNode();

        else
        {
            try
            {
                this.Session.SetLastAction( $"resetting TSP node {node.Number}" );
                _ = this.Session.WriteLine( "node[{0}].reset()", node.Number );
                (statusByte, details) = this.Session.TraceDeviceExceptionIfError( this.ControllerNode!.Number );
            }
            catch ( Pith.NativeException ex )
            {
                _ = this.Session.TraceException( ex );
            }
            catch ( Exception ex )
            {
                _ = this.Session.TraceException( ex );
            }
        }

        return !this.Session.IsErrorBitSet( statusByte );
    }

    /// <summary> Sets the connect rule on the specified node. </summary>
    /// <param name="nodeNumber"> Specifies the remote node number. </param>
    /// <param name="value">      true to value. </param>
    public void ConnectRuleSetter( int nodeNumber, int value )
    {
        _ = this.Session.WriteLine( Syntax.Tsp.Node.ConnectRuleSetterWaitCommandFormat, nodeNumber, value );
    }

    #endregion

    #region " reset nodes "

    /// <summary> Gets the reset nodes command. </summary>
    /// <value> The reset nodes command. </value>
    public string ResetNodesCommand { get; set; } = string.Empty;

    /// <summary> Resets the TSP nodes. </summary>
    /// <param name="timeout"> Specifies the time to wait for the instrument to return operation
    /// completed. </param>
    public void ResetNodes( TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( this.ResetNodesCommand ) )
        {
            this.Session.SetLastAction( "resetting the node" );
            _ = this.Session.WriteLine( "cc.isr.node.reset() waitcomplete(0)" );
            _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
            ServiceRequests statusByte = this.Session.AwaitOperationCompletion( timeout ).Status;
            this.Session.ThrowDeviceExceptionIfError( statusByte );
        }
    }

    /// <summary> Try reset nodes. </summary>
    /// <param name="timeout"> Specifies the time to wait for the instrument to return operation
    /// completed. </param>
    /// <returns> <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public bool TryResetNodes( TimeSpan timeout )
    {
        try
        {
            this.ResetNodes( timeout );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{this.Session.ResourceNameCaption} timed out resetting nodes; Ignored {ex.BuildMessage()};. " );
            return false;
        }

        return default;
    }

    #endregion

    #region " controller node "

    /// <summary> Gets the controller node model. </summary>
    /// <value> The controller node model. </value>
    public string ControllerNodeModel => this.ControllerNode is null ? string.Empty : this.ControllerNode.ModelNumber;

    /// <summary> The controller node. </summary>
    private NodeEntityBase? _controllerNode;

    /// <summary> Gets or sets reference to the controller node. </summary>
    /// <value> The controller node. </value>
    public NodeEntityBase? ControllerNode
    {
        get => this._controllerNode;
        set => _ = this.SetProperty( ref this._controllerNode, value );
    }

    /// <summary> Gets or sets the is controller node. </summary>
    /// <remarks> Required to allow this subsystem to initialize properly. </remarks>
    /// <value> The is controller node. </value>
    public bool IsControllerNode { get; set; }

    /// <summary> Initiates the controller node. </summary>
    /// <remarks> This also clears the <see cref="NodeEntities">collection of nodes</see>. </remarks>
    public void InitiateControllerNode()
    {
        if ( this.ControllerNode is null )
        {
            _ = this.QueryControllerNodeNumber();

            if ( this.ControllerNodeNumber is null ) throw new cc.isr.VI.Pith.NativeException( $"Failed fetching {nameof( this.ControllerNodeNumber )}" );

            this.ControllerNode = new NodeEntity( this.ControllerNodeNumber.Value, null );
            this.ControllerNode.InitializeKnownState( this.Session );
            this.NotifyPropertyChanged( nameof( this.ControllerNodeModel ) );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Initiated controller node #{this.ControllerNode.Number};. Instrument model {this.ControllerNode.ModelNumber} S/N={this.ControllerNode.SerialNumber} Firmware={this.ControllerNode.FirmwareVersion} enumerated on node." );
        }

        this._nodeEntities = [];
        this.NodeEntities().Add( this.ControllerNode );
    }

    #region " controller node number "

    /// <summary> The controller node number. </summary>
    private int? _controllerNodeNumber;

    /// <summary> Gets or sets the Controller (local) node number. </summary>
    /// <value> The Controller (local) node number. </value>
    public int? ControllerNodeNumber
    {
        get => this._controllerNodeNumber;
        set => _ = this.SetProperty( ref this._controllerNodeNumber, value );
    }

    /// <summary> Reads the Controller node number. </summary>
    /// <returns> An Integer or Null of failed. </returns>
    public int? QueryControllerNodeNumber()
    {
        this.ControllerNodeNumber = this.Session.QueryPrint( 0, 1, "_G.tsplink.node" );
        return this.ControllerNodeNumber;
    }

    /// <summary> Reads the Controller node number. </summary>
    /// <returns> An Integer or Null of failed. </returns>
    public int? TryQueryControllerNodeNumber()
    {
        string activity = "querying controller node number";
        try
        {
            _ = this.QueryControllerNodeNumber();
        }
        catch ( InvalidOperationException ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, $"Exception {activity}" );
        }
        catch ( InvalidCastException ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, $"Exception {activity}" );
        }

        return this.ControllerNodeNumber;
    }

    #endregion

    #endregion

    #region " tsp: node entities "

    /// <summary> Gets or sets the node entities. </summary>
    /// <remarks> Required for reading the system errors. </remarks>
    private NodeEntityCollection _nodeEntities = [];

    /// <summary> Returns the enumerated list of node entities. </summary>
    /// <returns> A list of. </returns>
    public NodeEntityCollection NodeEntities()
    {
        return this._nodeEntities;
    }

    /// <summary> Gets the number of nodes detected in the system. </summary>
    /// <value> The number of nodes. </value>
    public int NodeCount => this._nodeEntities is null ? 0 : this._nodeEntities.Count;

    /// <summary> Adds a node entity. </summary>
    /// <param name="nodeNumber"> The node number. </param>
    private void AddNodeEntity( int nodeNumber )
    {
        if ( this.ControllerNode is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ControllerNode )} is null." );
        if ( this.ControllerNodeNumber is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.ControllerNodeNumber )} is null." );
        if ( nodeNumber == this.ControllerNodeNumber )
        {
            this._nodeEntities.Add( this.ControllerNode );
        }
        else
        {
            NodeEntity node = new( nodeNumber, this.ControllerNode );
            node.InitializeKnownState( this.Session );
            this._nodeEntities.Add( node );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Added node #{node.Number};. Instrument model {node.ModelNumber} S/N={node.SerialNumber} Firmware={node.FirmwareVersion} enumerated on node." );
        }
    }

    /// <summary> Queries if a given node exists. </summary>
    /// <remarks>
    /// Uses the <see cref="NodeEntities">node entities</see> as a cache and add to the cache is not
    /// is not cached.
    /// </remarks>
    /// <param name="nodeNumber"> Specifies the node number. </param>
    /// <returns> <c>true</c> if node exists; otherwise, false. </returns>
    public bool NodeExists( int nodeNumber )
    {
        bool affirmative = true;
        if ( this._nodeEntities.Count > 0 && this._nodeEntities.Contains( NodeEntityBase.BuildKey( nodeNumber ) ) )
        {
            return affirmative;
        }
        else if ( NodeEntityBase.NodeExists( this.Session, nodeNumber ) )
        {
            this.AddNodeEntity( nodeNumber );
            affirmative = this._nodeEntities.Count > 0 && this._nodeEntities.Contains( NodeEntityBase.BuildKey( nodeNumber ) );
        }
        else
        {
            affirmative = false;
        }

        return affirmative;
    }

    /// <summary> Enumerates the collection of nodes on the TSP Link net. </summary>
    /// <param name="maximumCount"> Specifies the maximum expected node number. There could be up to
    /// 64 nodes on the TSP link. Specify 0 to use the maximum node
    /// count. </param>
    public void EnumerateNodes( int maximumCount )
    {
        this.InitiateControllerNode();
        if ( maximumCount > 1 )
        {
            for ( int i = 1, loopTo = maximumCount; i <= loopTo; i++ )
            {
                if ( !NodeEntityBase.NodeExists( this.Session, i ) )
                {
                    this.AddNodeEntity( i );
                }
            }
        }

        this.NotifyPropertyChanged( nameof( this.NodeCount ) );
    }

    #endregion

    #region " tsp link "

    /// <summary> True to using tsp link. </summary>
    private bool _usingTspLink;

    /// <summary>
    /// Gets or sets the condition for using TSP Link. Must be affirmative otherwise TSP link reset
    /// commands are ignored.
    /// </summary>
    /// <value> The using tsp link. </value>
    public bool UsingTspLink
    {
        get => this._usingTspLink;
        set => _ = this.SetProperty( ref this._usingTspLink, value );
    }

    #endregion

    #region " tsp link online state "

    /// <summary> The is tsp link online. </summary>
    private bool? _isTspLinkOnline;

    /// <summary> gets or sets the sentinel indicating if the TSP Link System is ready. </summary>
    /// <value>
    /// <c>null</c> if Not known; <c>true</c> if the tsp link is on line; otherwise <c>false</c>.
    /// </value>
    public bool? IsTspLinkOnline
    {
        get => this._isTspLinkOnline;
        set => _ = this.SetProperty( ref this._isTspLinkOnline, value );
    }

    /// <summary> Gets or sets the tsp link on-line state query command. </summary>
    /// <value> The tsp link on line state query command. </value>
    public string TspLinkOnlineStateQueryCommand { get; set; } = string.Empty;

    /// <summary> Reads tsp link on line state. </summary>
    /// <returns>
    /// <c>null</c> if Not known; <c>true</c> if the tsp link is on line; otherwise
    /// <c>false</c>.
    /// </returns>
    public bool? ReadTspLinkOnlineState()
    {
        if ( !string.IsNullOrWhiteSpace( this.TspLinkOnlineStateQueryCommand ) )
        {
            this.IsTspLinkOnline = this.Session.IsStatementTrue( this.TspLinkOnlineStateQueryCommand );
        }

        return this.IsTspLinkOnline;
    }

    #endregion

    #region " tsp link offline state "

    /// <summary> The is tsp link offline. </summary>
    private bool? _isTspLinkOffline;

    /// <summary> gets or sets the sentinel indicating if the TSP Link System is ready. </summary>
    /// <value>
    /// <c>null</c> if Not known; <c>true</c> if the tsp link is Off line; otherwise <c>false</c>.
    /// </value>
    public bool? IsTspLinkOffline
    {
        get => this._isTspLinkOffline;
        set => _ = this.SetProperty( ref this._isTspLinkOffline, value );
    }

    /// <summary> Gets or sets the tsp link Off line state query command. </summary>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <value> The tsp link Off line state query command. </value>
    public string TspLinkOfflineStateQueryCommand { get; set; } = string.Empty;

    /// <summary> Reads tsp link Off line state. </summary>
    /// <returns>
    /// <c>null</c> if Not known; <c>true</c> if the tsp link is Off line; otherwise
    /// <c>false</c>.
    /// </returns>
    public bool? ReadTspLinkOfflineState()
    {
        if ( !string.IsNullOrWhiteSpace( this.TspLinkOfflineStateQueryCommand ) )
        {
            this.IsTspLinkOnline = this.Session.IsStatementTrue( this.TspLinkOfflineStateQueryCommand );
        }

        return this.IsTspLinkOffline;
    }

    #endregion

    #region " reset "

    /// <summary> Gets or sets the tsp link reset command. </summary>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <value> The tsp link reset command. </value>
    public string TspLinkResetCommand { get; set; } = string.Empty;

    /// <summary> Resets the TSP link and the ISR support framework. </summary>
    /// <remarks> Requires loading the 'isr.TspLink' scripts. </remarks>
    /// <param name="timeout">          The operationCompletionTimeout. </param>
    /// <param name="maximumNodeCount"> Number of maximum nodes. </param>
    public void ResetTspLinkWaitComplete( TimeSpan timeout, int maximumNodeCount )
    {
        try
        {
            this.Session.StoreCommunicationTimeout( timeout );
            if ( !string.IsNullOrWhiteSpace( this.TspLinkResetCommand ) )
            {
                this.Session.SetLastAction( "resetting TSP Link" );
                this.Session.LastNodeNumber = default;

                // do not condition the reset upon a previous reset.
                this.Session.SetLastAction( "enabling completion events monitoring" );
                this.Session.EnableServiceRequestOnOperationCompletion();

                this.Session.SetLastAction( "resetting the tsp link with wait complete" );
                _ = this.Session.WriteLine( $"{this.TspLinkResetCommand} {this.Session.OperationCompleteCommand}" );

                this.Session.SetLastAction( "awaiting completion" );
                _ = this.Session.AwaitOperationCompletion( timeout );

                this.Session.ThrowDeviceExceptionIfError();
            }

            // clear the reset status
            this.IsTspLinkOffline = new bool?();
            this.IsTspLinkOnline = new bool?();
            _ = this.ReadTspLinkOnlineState();
            this.EnumerateNodes( maximumNodeCount );
        }
        catch
        {
            throw;
        }
        finally
        {
            this.Session.RestoreCommunicationTimeout();
        }
    }

    /// <summary> Reset the TSP Link or just the first node if TSP link not defined. </summary>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="timeout">          The operationCompletionTimeout. </param>
    /// <param name="maximumNodeCount"> Number of maximum nodes. </param>
    /// <param name="displaySubsystem"> The display subsystem. </param>
    /// <param name="frameworkName">    Name of the framework. </param>
    public void ResetTspLink( TimeSpan timeout, int maximumNodeCount, DisplaySubsystemBase displaySubsystem, string frameworkName )
    {
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );

        this.Session.LastNodeNumber = default;
        displaySubsystem.DisplayLine( 1, "Resetting  {0}", frameworkName );
        displaySubsystem.DisplayLine( 2, "Resetting TSP Link" );
        this.Session.SetLastAction( "resetting tsp link" );
        this.ResetTspLinkWaitComplete( timeout, maximumNodeCount );
        if ( this.NodeCount <= 0 )
        {
            if ( this.UsingTspLink )
                throw new InvalidOperationException( $"{this.Session.LastAction} failed--no nodes;. " );
            else
                throw new InvalidOperationException( $"{this.Session.LastAction} failed setting main node;. " );
        }
        else if ( this.UsingTspLink && !this.IsTspLinkOnline.GetValueOrDefault( false ) )
            throw new InvalidOperationException( $"{this.Session.LastAction} failed--TSP Link is not on line." );
    }

    #endregion

    #region " tsp link state "

    /// <summary> State of the online. </summary>
    private const string Online_State = "online";

    /// <summary> State of the tsp link. </summary>
    private string? _tspLinkState;

    /// <summary> Gets or sets the state of the tsp link. </summary>
    /// <value> The tsp state. </value>
    public string? TspLinkState
    {
        get => this._tspLinkState;
        set
        {
            if ( this.SetProperty( ref this._tspLinkState, value ) && this._tspLinkState is not null )
            {
                this.IsTspLinkOnline = this._tspLinkState.Equals( Online_State, StringComparison.OrdinalIgnoreCase );
                this.IsTspLinkOffline = !this.IsTspLinkOnline.Value;
            }
        }
    }

    /// <summary> Reads tsp link state. </summary>
    /// <returns> The tsp link state. </returns>
    public string? QueryTspLinkState()
    {
        this.Session.SetLastAction( "reading TSP Link state" );
        this.Session.LastNodeNumber = default;
        this.TspLinkState = this.Session.QueryPrintStringFormatTrimEnd( "tsplink.state" );
        this.Session.ThrowDeviceExceptionIfError();
        return this.TspLinkState;
    }

    #endregion

    #region " tsp link group numbers "

    /// <summary>
    /// Assigns group numbers to the nodes. A unique group number is required for executing
    /// concurrent code on all nodes.
    /// </summary>
    /// <remarks>
    /// David, 2009-09-08, 3.0.3538.x"> Allows setting groups even if TSP Link is not on line.
    /// </remarks>
    /// <returns> <c>true</c> of okay; otherwise, <c>false</c>. </returns>
    public bool AssignNodeGroupNumbers()
    {
        Pith.ServiceRequests statusByte;
        string details;
        bool affirmative = false;
        if ( this.NodeEntities() is not null )
        {
            foreach ( NodeEntityBase node in this._nodeEntities )
            {
                try
                {
                    this.Session.SetLastAction( $"assigning group to node number {node.Number}" );
                    _ = this.IsTspLinkOnline == true
                        ? this.Session.WriteLine( "node[{0}].tsplink.group = {0}", node.Number )
                        : this.Session.WriteLine( "localnode.tsplink.group = {0}", node.Number );
                    _ = this.Session.TraceInformation();
                    (statusByte, details) = this.Session.TraceDeviceExceptionIfError();
                    affirmative = !this.Session.IsErrorBitSet( statusByte );
                }
                catch ( Pith.NativeException ex )
                {
                    _ = this.Session.TraceException( ex );
                    affirmative = false;
                }
                catch ( Exception ex )
                {
                    _ = this.Session.TraceException( ex );
                    affirmative = false;
                }

                if ( !affirmative )
                    break;
            }
            // If Me.IsTspLinkOnline Then
            // End If
        }

        return affirmative;
    }

    #endregion

    #region " tsp link reset "

    /// <summary> Gets or sets the tsp link reset operationCompletionTimeout. </summary>
    /// <value> The tsp link reset operationCompletionTimeout. </value>
    public TimeSpan TspLinkResetTimeout { get; set; } = TimeSpan.FromMilliseconds( 5000d );

    /// <summary> Reset TSP Link with error reporting. </summary>
    /// <remarks>
    /// David, 2009-09-22, 3.0.3552.x"> The procedure caused error 1220 - TSP link failure on the
    /// remote instrument. The error occurred only if the program was stopped and restarted without
    /// toggling power on the instruments. Waiting completion of the previous task helped even though
    /// that task did not access the remote node!
    /// </remarks>
    private void ResetTspLinkIgnoreError()
    {
        this.Session.LastNodeNumber = default;
        this.IsTspLinkOnline = new bool?();
        this.Session.SetLastAction( "enable service request on completion" );
        this.Session.EnableServiceRequestOnOperationCompletion();
        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );

        // old code:
        // if ( this.TspLinkResetTimeout > TimeSpan.Zero )
        // this.Session.ThrowDeviceErrorIfError( this.Session.AwaitOperation Completion( this.TspLinkResetTimeout ).Status );

        this.Session.SetLastAction( "resetting the node" );
        _ = this.Session.WriteLine( "tsplink.reset() waitcomplete(0) errorqueue.clear() waitcomplete()" );
        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );

        ServiceRequests statusByte = this.TspLinkResetTimeout > TimeSpan.Zero
            ? this.Session.AwaitOperationCompletion( this.TspLinkResetTimeout ).Status
            : this.Session.ReadStatusByte();

        this.Session.ThrowDeviceExceptionIfError( statusByte );
    }

    /// <summary> Reset TSP Link with error reporting. </summary>
    /// <returns> <c>true</c> of okay; otherwise, <c>false</c>. </returns>
    private bool TryResetTspLinkReportError()
    {
        this.Session.LastNodeNumber = default;
        this.IsTspLinkOnline = new bool?();
        Pith.ServiceRequests statusByte;
        string details;
        bool affirmative;
        try
        {
            this.Session.SetLastAction( "enable service request on completion" );
            this.Session.EnableServiceRequestOnOperationCompletion();
            _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
            _ = this.Session.TraceInformation();
            (statusByte, details) = this.Session.TraceDeviceExceptionIfError();
            affirmative = !this.Session.IsErrorBitSet( statusByte );
        }
        catch ( Pith.NativeException ex )
        {
            _ = this.Session.TraceException( ex );
            affirmative = false;
        }
        catch ( Exception ex )
        {
            _ = this.Session.TraceException( ex );
            affirmative = false;
        }

        if ( affirmative )
        {
            try
            {
                this.Session.SetLastAction( "awaiting status bitmask" );
                (bool timedOut, Pith.ServiceRequests status, TimeSpan elapsed) = this.Session.AwaitStatusBitmask( Pith.ServiceRequests.RequestingService,
                    TimeSpan.FromMilliseconds( 1000d ), TimeSpan.Zero, TimeSpan.FromMilliseconds( 10d ) );
                _ = this.Session.TraceInformation();
                (statusByte, details) = this.Session.TraceDeviceExceptionIfError();
                affirmative = !this.Session.IsErrorBitSet( statusByte );
            }
            catch ( Pith.NativeException ex )
            {
                _ = this.Session.TraceException( ex );
                affirmative = false;
            }
            catch ( Exception ex )
            {
                _ = this.Session.TraceException( ex );
                affirmative = false;
            }
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning(
                $"Instrument '{this.ResourceNameCaption}' failed resetting TSP Link;. \n{new StackFrame( true ).UserCallStack()}" );
        }

        return affirmative;
    }

    /// <summary> Resets the TSP link if not on line. </summary>
    /// <remarks>
    /// David, 2009-09-08, 3.0.3538.x"> Allows to complete TSP Link reset even on failure in case we
    /// have a single node.
    /// </remarks>
    /// <param name="maximumNodeCount"> Number of maximum nodes. </param>
    public void ResetTspLink( int maximumNodeCount )
    {
        if ( this.UsingTspLink )
        {
            this.ResetTspLinkIgnoreError();
        }

        if ( this.IsTspLinkOnline == true )
        {
            // enumerate all nodes.
            this.EnumerateNodes( maximumNodeCount );
        }
        else
        {
            // enumerate the controller node.
            this.EnumerateNodes( 1 );
        }

        // assign node group numbers.
        _ = this.AssignNodeGroupNumbers();

        // clear the error queue on all nodes.
        this.Session.ClearErrorQueue();

        // clear data queues.
        this.ClearDataQueue( this.NodeEntities() );
    }

    #endregion
}
