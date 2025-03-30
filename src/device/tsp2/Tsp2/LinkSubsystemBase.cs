using System.Diagnostics;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.VI.ExceptionExtensions;
namespace cc.isr.VI.Tsp2;

/// <summary> Defines a subsystem for handing TSP Link and multiple nodes. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-11-01. Based on legacy status subsystem. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class LinkSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
    /// Subsystem</see>. </param>
    protected LinkSubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.TspLinkResetTimeout = TimeSpan.FromMilliseconds( 5000d );
    }

    #endregion

    #region " i presettable "

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Customizes the reset state. </remarks>
    public override void InitKnownState()
    {
        base.InitKnownState();
        // 2016-01-07: Moved from the Master Device.
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
        this._nodeEntities = new NodeEntityCollection();
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

    #region " error queue: node "

    /// <summary> Queries error count on a remote node. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node"> . </param>
    /// <returns> The error count. </returns>
    public int QueryErrorQueueCount( NodeEntityBase node )
    {
        int? count;
        if ( node is null )
            throw new ArgumentNullException( nameof( node ) );
        count = node.IsController ? this.Session.QueryPrint( 0, 1, Syntax.Tsp.EventLog.ErrorCount ) : this.Session.QueryPrint( 0, 1, Syntax.Tsp.Node.NodeErrorCountBuilder, ( object ) node.Number );
        return count.GetValueOrDefault( 0 );
    }

    /// <summary> Clears the error cache. </summary>
    public void ClearErrorCache()
    {
        this.StatusSubsystem.ClearErrorCache();
        this.DeviceErrorQueue = new Queue<TspDeviceError>();
    }

    /// <summary> Clears the error queue for the specified node. </summary>
    /// <param name="nodeNumber"> The node number. </param>
    public void ClearErrorQueue( int nodeNumber )
    {
        if ( !this.NodeExists( nodeNumber ) )
        {
            _ = this.Session.WriteLine( Syntax.Tsp.EventLog.NodeClearEventLogCommand, ( object ) nodeNumber );
        }
    }

    /// <summary> Clears the error queue. </summary>
    public void ClearErrorQueue()
    {
        if ( this.NodeEntities() is null )
        {
            this.StatusSubsystem.ClearErrorQueue();
        }
        else
        {
            this.ClearErrorCache();
            foreach ( NodeEntityBase node in this.NodeEntities() )
                this.ClearErrorQueue( node.Number );
        }
    }

    /// <summary> Gets the error queue. </summary>
    /// <value> A Queue of device errors. </value>
    protected Queue<Syntax.Tsp.TspDeviceError> DeviceErrorQueue { get; private set; }

    /// <summary> Returns the queued error. </summary>
    /// <remarks> Sends the error print format query and reads back and parses the error. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node"> . </param>
    /// <returns> The queued error. </returns>
    public Syntax.Tsp.TspDeviceError QueryQueuedError( NodeEntityBase node )
    {
        Syntax.Tsp.TspDeviceError err = new();
        if ( this.QueryErrorQueueCount( node ) > 0 )
        {
            if ( node is null )
                throw new ArgumentNullException( nameof( node ) );
            this.Session.SetLastAction( "Querying queued device errors" );
            string message;
            if ( node.IsController )
            {
                this.Session.LastNodeNumber = node.ControllerNodeNumber;
                message = this.Session.QueryPrintStringFormatTrimEnd( $"%d,%s,%d,node{node.Number}", "_G.eventlog.next(eventlog.SEV_ERROR)" );
            }
            else
            {
                this.Session.LastNodeNumber = node.Number;
                message = this.Session.QueryPrintStringFormatTrimEnd( $"%d,%s,%d,node{node.Number}", $"node[{node.Number}].eventlog.next(eventlog.SEV_ERROR)" );
            }

            this.CheckThrowDeviceException( false, this.Session.StatusReadDelay, "getting queued error;. using {0}.", this.Session.LastMessageSent );
            err = new();
            err.Parse( message );
        }

        return err;
    }

    /// <summary> Reads the device errors. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node"> . </param>
    /// <returns> <c>true</c> if device has errors, <c>false</c> otherwise. </returns>
    public string QueryDeviceErrors( NodeEntityBase node )
    {
        if ( node is null )
            throw new ArgumentNullException( nameof( node ) );
        TspDeviceError deviceError;
        do
        {
            deviceError = this.QueryQueuedError( node );
            if ( deviceError.IsError )
            {
                this.DeviceErrorQueue.Enqueue( deviceError );
            }
        }
        while ( this.StatusSubsystem.ErrorAvailable );
        System.Text.StringBuilder message = new System.Text.StringBuilder();
        if ( this.DeviceErrorQueue is object && this.DeviceErrorQueue.Count > 0 )
        {
            _ = message.AppendFormat( "Instrument {0} Node {1} Errors:", this.ResourceNameCaption, node.Number );
            _ = message.AppendLine();
            foreach ( TspDeviceError e in this.DeviceErrorQueue )
                _ = message.AppendLine( e.ErrorMessage );
        }

        this.StatusSubsystem.AppendDeviceErrorMessage( message.ToString() );
        return this.StatusSubsystem.Session.DeviceErrorReport;
    }

    /// <summary> Reads the device errors. </summary>
    /// <returns> <c>true</c> if device has errors, <c>false</c> otherwise. </returns>
    public string QueryDeviceErrors()
    {
        this.ClearErrorCache();
        foreach ( NodeEntityBase node in this.NodeEntities() )
            _ = this.QueryDeviceErrors( node );
        return this.StatusSubsystem.Session.DeviceErrorReport;
    }

    #endregion

    #region " opc "

    /// <summary> Enables group wait complete. </summary>
    /// <param name="groupNumber"> Specifies the group number. That would be the same as the TSP
    /// Link group number for the node. </param>
    public void EnableWaitComplete( int groupNumber )
    {
        this.Session.EnableServiceRequestOnOperationCompletion();
        _ = this.Session.WriteLine( Syntax.Tsp.Lua.WaitGroupCommandFormat, ( object ) groupNumber );
    }

    /// <summary> Waits completion after command. </summary>
    /// <param name="nodeNumber"> Specifies the node number. </param>
    /// <param name="timeout">    The timeout. </param>
    /// <param name="isQuery">    Specifies the condition indicating if the command that preceded the
    /// wait is a query, which determines how errors are fetched. </param>
    public void WaitComplete( int nodeNumber, TimeSpan timeout, bool isQuery )
    {
        this.Session.SetLastAction( "enabling wait complete" );
        this.Session.LastNodeNumber = nodeNumber;
        this.EnableWaitComplete( 0 );
        this.CheckThrowDeviceException( !isQuery, "enabled wait complete group '{0}';. ", ( object ) 0 );
        this.Session.SetLastAction( "waiting completion" );
        _ = this.Session.ApplyStatusByte( this.Session.AwaitOperationCompletion( timeout ).Status );
        this.CheckThrowDeviceException( !isQuery, "waiting completion" );
        this.Session.LastNodeNumber = new int?();
    }

    #endregion

    #region " collect garbage "

    /// <summary> Collect garbage wait complete. </summary>
    /// <param name="node">    Specifies the remote node number to validate. </param>
    /// <param name="timeout"> Specifies the time to wait for the instrument to return operation
    /// completed. </param>
    /// <param name="format">  Describes the format to use. </param>
    /// <param name="args">    A variable-length parameters list containing arguments. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool CollectGarbageWaitComplete( NodeEntityBase node, TimeSpan timeout, string format, params object[] args )
    {
        return this.CollectGarbageWaitCompleteThis( node, timeout, format, args );
    }

    /// <summary> Does garbage collection. Reports operations synopsis. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node">    Specifies the remote node number to validate. </param>
    /// <param name="timeout"> Specifies the time to wait for the instrument to return operation
    /// completed. </param>
    /// <param name="format">  Describes the format to use. </param>
    /// <param name="args">    A variable-length parameters list containing arguments. </param>
    /// <returns> <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    private bool CollectGarbageWaitCompleteThis( NodeEntityBase node, TimeSpan timeout, string format, params object[] args )
    {
        if ( node is null )
            throw new ArgumentNullException( nameof( node ) );
        if ( node.IsController )
        {
            return this.StatusSubsystem.CollectGarbageWaitComplete( timeout, format, args );
        }

        bool affirmative;
        // do a garbage collection
        try
        {
            this.Session.SetLastAction( "collectinve garbage" );
            _ = this.Session.WriteLine( Syntax.Tsp.Node.CollectNodeGarbageFormat, ( object ) node.Number );
            this.Session.TraceInformation();
            affirmative = !this.StatusSubsystem.TraceDeviceOperationIfError( this.ControllerNode.Number, this.Session.StatusReadDelay,  "collecting garbage after {0};. ", string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
        }
        catch ( Pith.NativeException ex )
        {
            this.Session.TraceException( ex, "collecting garbage after {0};. ", string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
            affirmative = false;
        }
        catch ( Exception ex )
        {
            this.Session.TraceException( ex, "collecting garbage after {0};. ", string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
            affirmative = false;
        }

        try
        {
            if ( affirmative )
            {
                // to_do: fix
                this.EnableWaitComplete( 0 );
                _ = this.Session.ApplyStatusByte( this.Session.AwaitOperationCompletion( timeout ).Status );
                affirmative = this.StatusSubsystem.TraceVisaDeviceOperationOkay( true, "awaiting completion after collecting garbage after {0};. ", string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
            }
        }
        catch ( Pith.NativeException ex )
        {
            this.Session.TraceException( ex, "awaiting completion after collecting garbage after {0};. ", string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
            affirmative = false;
        }
        catch ( Exception ex )
        {
            this.StatusSubsystem.TraceOperation( ex, "awaiting completion after collecting garbage after {0};. ", string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
            affirmative = false;
        }

        return affirmative;
    }

    #endregion

    #region " data queue "

    /// <summary> clears the data queue for the specified node. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node">                Specifies the node. </param>
    /// <param name="reportQueueNotEmpty"> true to report queue not empty. </param>
    public void ClearDataQueue( NodeEntityBase node, bool reportQueueNotEmpty )
    {
        if ( node is null )
            throw new ArgumentNullException( nameof( node ) );
        if ( this.NodeExists( node.Number ) )
        {
            if ( reportQueueNotEmpty )
            {
                if ( this.QueryDataQueueCount( node ) > 0 )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Data queue not empty on node {node.Number}" );
                }
            }

            _ = this.Session.WriteLine( "node[{0}].dataqueue.clear() waitcomplete({0})", ( object ) node.Number );
        }
    }

    /// <summary> clears the data queue for the specified node. </summary>
    /// <param name="nodeNumber"> The node number. </param>
    public void ClearDataQueue( int nodeNumber )
    {
        if ( this.NodeExists( nodeNumber ) )
        {
            _ = this.Session.WriteLine( "node[{0}].dataqueue.clear() waitcomplete({0})", ( object ) nodeNumber );
        }
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
        return this.NodeExists( nodeNumber ) ? this.Session.QueryPrint( 0, 1, $"node[{nodeNumber}].dataqueue.CAPACITY" ) : 0;
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
        bool affirmative;
        try
        {
            this.Session.SetLastAction( "resetting local TSP node" );
            _ = this.Session.WriteLine( "localnode.reset()" );
            this.Session.TraceInformation();
            affirmative = !this.StatusSubsystem.TraceDeviceExceptionIfError();
        }
        catch ( Pith.NativeException ex )
        {
            this.Session.TraceException( ex, "resetting local TSP node" );
            affirmative = false;
        }
        catch ( Exception ex )
        {
            this.StatusSubsystem.TraceOperation( ex, "resetting local TSP node" );
            affirmative = false;
        }

        return affirmative;
    }

    /// <summary> Resets a node. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node"> The node. </param>
    /// <returns> <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public bool ResetNode( NodeEntityBase node )
    {
        if ( node is null )
            throw new ArgumentNullException( nameof( node ) );
        bool affirmative;
        if ( node.IsController )
        {
            affirmative = this.ResetNode();
        }
        else
        {
            try
            {
               this.Session.SetLastAction( "resetting local TSP node" );
                _ = this.Session.WriteLine( "node[{0}].reset()", ( object ) node.Number );
                this.Session.TraceInformation();
                affirmative = !this.StatusSubsystem.TraceDeviceExceptionIfError( this.ControllerNode.Number, this.Session.StatusReadDelay, "resetting TSP node {0};. ", ( object ) node.Number );
            }
            catch ( Pith.NativeException ex )
            {
                this.Session.TraceException( ex, node.Number, "resetting TSP node {0};. ", ( object ) node.Number );
                affirmative = false;
            }
            catch ( Exception ex )
            {
                this.Session.TraceException( ex, "resetting TSP node {0};. ", ( object ) node.Number );
                affirmative = false;
            }
        }

        return affirmative;
    }

    /// <summary> Sets the connect rule on the specified node. </summary>
    /// <param name="nodeNumber"> Specifies the remote node number. </param>
    /// <param name="value">      true to value. </param>
    public void ConnectRuleSetter( int nodeNumber, int value )
    {
        _ = this.Session.WriteLine( Syntax.Tsp.Node.ConnectRuleSetterCommandFormat, nodeNumber, value );
    }

    #endregion

    #region " reset nodes "

    /// <summary> Gets the reset nodes command. </summary>
    /// <value> The reset nodes command. </value>
    public string ResetNodesCommand { get; set; }

    /// <summary> Resets the TSP nodes. </summary>
    /// <param name="timeout"> Specifies the time to wait for the instrument to return operation
    /// completed. </param>
    public void ResetNodes( TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( this.ResetNodesCommand ) )
        {
            this.Session.EnableServiceRequestOnOperationCompletion();
            _ = this.Session.WriteLine( "cc.isr.node.reset() waitcomplete(0)" );
            _ = this.Session.ApplyStatusByte( this.Session.AwaitOperationCompletion( timeout ).Status );
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
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{this.Session.ResourceNameCaption} timed out resetting nodes; Ignored {ex.BuildMessage()}" );
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
    private NodeEntityBase _controllerNode;

    /// <summary> Gets or sets reference to the controller node. </summary>
    /// <value> The controller node. </value>
    public NodeEntityBase ControllerNode
    {
        get => this._controllerNode;
        set
        {
            if ( value is null )
            {
                if ( this.ControllerNode is not null )
                {
                    this._controllerNode = value;
                    this.NotifyPropertyChanged();
                }
            }
            else if ( this.ControllerNode is null || !value.Equals( this.ControllerNode ) )
            {
                this._controllerNode = value;
                this.NotifyPropertyChanged();
            }
        }
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
            this.ControllerNode = new NodeEntity( this.ControllerNodeNumber.Value, this.ControllerNodeNumber.Value );
            this.ControllerNode.InitializeKnownState( this.Session );
            this.NotifyPropertyChanged( nameof( this.ControllerNodeModel ) );
            _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Initiated controller node #{this.ControllerNode.Number};. Instrument model {this.ControllerNode.ModelNumber} S/N={this.ControllerNode.SerialNumber} Firmware={this.ControllerNode.FirmwareVersion} enumerated on node." );
        }

        this._nodeEntities = new NodeEntityCollection();
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
        set
        {
            if ( !Nullable.Equals( value, this.ControllerNodeNumber ) )
            {
                this._controllerNodeNumber = value;
                this.NotifyPropertyChanged();
            }
        }
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
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        catch ( InvalidCastException ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }

        return this.ControllerNodeNumber;
    }

    #endregion

    #endregion

    #region " tsp: node entities "

    /// <summary> Gets or sets the node entities. </summary>
    /// <remarks> Required for reading the system errors. </remarks>
    private NodeEntityCollection _nodeEntities;

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
        if ( nodeNumber == this.ControllerNodeNumber == true )
        {
            this._nodeEntities.Add( this.ControllerNode );
        }
        else
        {
            NodeEntity node = new NodeEntity( nodeNumber, this.ControllerNodeNumber.Value );
            node.InitializeKnownState( this.Session );
            this._nodeEntities.Add( node );
            _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Added node #{node.Number};. Instrument model {node.ModelNumber} S/N={node.SerialNumber} Firmware={node.FirmwareVersion} enumerated on node." );
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
        set
        {
            if ( value != this.UsingTspLink )
            {
                this._usingTspLink = value;
                this.NotifyPropertyChanged();
            }
        }
    }

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
        set
        {
            if ( !Equals( value, this.IsTspLinkOnline ) )
            {
                this._isTspLinkOnline = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the tsp link on-line state query command. </summary>
    /// <value> The tsp link on line state query command. </value>
    public string TspLinkOnlineStateQueryCommand { get; set; }

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
        set
        {
            if ( !Equals( value, this.IsTspLinkOffline ) )
            {
                this._isTspLinkOffline = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the tsp link Off line state query command. </summary>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <value> The tsp link Off line state query command. </value>
    public string TspLinkOfflineStateQueryCommand { get; set; }

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
    public string TspLinkResetCommand { get; set; }

    /// <summary> Resets the TSP link and the ISR support framework. </summary>
    /// <remarks> Requires loading the 'isr.tsplink' scripts. </remarks>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="maximumNodeCount"> Number of maximum nodes. </param>
    public void ResetTspLinkWaitComplete( TimeSpan timeout, int maximumNodeCount )
    {
        try
        {
            this.Session.StoreCommunicationTimeout( timeout );
            if ( !string.IsNullOrWhiteSpace( this.TspLinkResetCommand ) )
            {
                this.Session.SetLastAction( "resetting TSP Link" );
                this.Session.LastNodeNumber = new int?();
                // do not condition the reset upon a previous reset.
                this.Session.EnableServiceRequestOnOperationCompletion();
                _ = this.Session.WriteLine( this.TspLinkResetCommand );
                _ = this.Session.ApplyStatusByte( this.Session.AwaitOperationCompletion( timeout ).Status );
                this.CheckThrowDeviceException( false, this.Session.StatusReadDelay, "resetting TSP Link" );
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
    /// <param name="timeout">          The timeout. </param>
    /// <param name="maximumNodeCount"> Number of maximum nodes. </param>
    /// <param name="displaySubsystem"> The display subsystem. </param>
    /// <param name="frameworkName">    Name of the framework. </param>
    public void ResetTspLink( TimeSpan timeout, int maximumNodeCount, DisplaySubsystemBase displaySubsystem, string frameworkName )
    {
        if ( displaySubsystem is null )
            throw new ArgumentNullException( nameof( displaySubsystem ) );
        displaySubsystem.DisplayLine( 1, "Resetting  {0}", frameworkName );
        displaySubsystem.DisplayLine( 2, "Resetting TSP Link" );
        this.ResetTspLinkWaitComplete( timeout, maximumNodeCount );
        if ( this.NodeCount <= 0 )
        {
            if ( this.UsingTspLink )
            {
                throw new InvalidOperationException( $"{this.Session.ResourceNameCaption} failed resetting TSP Link--no nodes" );
            }
            else
            {
                throw new InvalidOperationException( $"{this.Session.ResourceNameCaption} failed setting master node;. " );
            }
        }
        else if ( (this.UsingTspLink && !this.IsTspLinkOnline.GetValueOrDefault( false )) == true )
        {
            throw new InvalidOperationException( $"{this.Session.ResourceNameCaption} failed resetting TSP Link;. TSP Link is not on line." );
        }
    }

    #endregion

    #region " tsp link state "

    /// <summary> State of the online. </summary>
    private const string _onlineState = "online";

    /// <summary> State of the tsp link. </summary>
    private string _tspLinkState;

    /// <summary> Gets or sets the state of the tsp link. </summary>
    /// <value> The tsp state. </value>
    public string TspLinkState
    {
        get => this._tspLinkState;
        set
        {
            if ( string.IsNullOrWhiteSpace( value ) )
                value = string.Empty;
            if ( !value.Equals( this.TspLinkState ) )
            {
                this._tspLinkState = value;
                this.NotifyPropertyChanged();
                this.IsTspLinkOnline = this.TspLinkState.Equals( _onlineState, StringComparison.OrdinalIgnoreCase );
                this.IsTspLinkOffline = !this.IsTspLinkOnline.Value;
            }
        }
    }

    /// <summary> Reads tsp link state. </summary>
    /// <returns> The tsp link state. </returns>
    public string QueryTspLinkState()
    {
        this.Session.SetLastAction( "reading TSP Link state" );
        this.Session.LastNodeNumber = new int?();
        this.TspLinkState = this.Session.QueryPrintStringFormatTrimEnd( "tsplink.state" );
        this.CheckThrowDeviceException( false, this.Session.StatusReadDelay, "getting tsp link state;. using {0}.", this.Session.LastMessageSent );
        return this.TspLinkState;
    }

    #endregion

    #region " tsp link group numbers "

    /// <summary>
    /// Assigns group numbers to the nodes. A unique group number is required for executing
    /// concurrent code on all nodes.
    /// </summary>
    /// <remarks>
    /// David, 2009-09-08, 3.0.3538. Allows setting groups even if TSP Link is not on line.
    /// </remarks>
    /// <returns> <c>true</c> of okay; otherwise, <c>false</c>. </returns>
    public bool AssignNodeGroupNumbers()
    {
        bool affirmative = true;
        if ( this.NodeEntities() is not null )
        {
            foreach ( NodeEntityBase node in this._nodeEntities )
            {
                try
                {
                    this.Session.SetLastAction ( $"assigning group to node number {node.Number} " );
                    _ = this.IsTspLinkOnline == true
                        ? this.Session.WriteLine( "node[{0}].tsplink.group = {0}", node.Number )
                        : this.Session.WriteLine( "localnode.tsplink.group = {0}", node.Number );

                   this.Session.TraceInformation();
                    affirmative = !this.StatusSubsystem.TraceDeviceExceptionIfError();
                }
                catch ( Pith.NativeException ex )
                {
                    this.Session.TraceException( ex, "assigning group to node number {0};. ", ( object ) node.Number );
                    affirmative = false;
                }
                catch ( Exception ex )
                {
                    this.StatusSubsystem.TraceOperation( ex, "assigning group to node number {0};. ", ( object ) node.Number );
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

    /// <summary> Gets or sets the tsp link reset timeout. </summary>
    /// <value> The tsp link reset timeout. </value>
    public TimeSpan TspLinkResetTimeout { get; set; }

    /// <summary> Reset TSP Link with error reporting. </summary>
    /// <remarks>
    /// David, 2009-09-22, 3.0.3552.x"> The procedure caused error 1220 - TSP link failure on the
    /// remote instrument. The error occurred only if the program was stopped and restarted without
    /// toggling power on the instruments. Waiting completion of the previous task helped even though
    /// that task did not access the remote node!
    /// </remarks>
    private void ResetTspLinkIgnoreError()
    {
        this.IsTspLinkOnline = new bool?();
        this.EnableWaitComplete( 0 );
        _ = this.Session.ApplyStatusByte( this.Session.AwaitOperationCompletion( this.TspLinkResetTimeout ).Status );
        this.Session.EnableWaitComplete();
        _ = this.Session.WriteLine( "tsplink.reset() waitcomplete(0) errorqueue.clear() waitcomplete()" );
        _ = this.Session.ApplyStatusByte( this.Session.AwaitOperationCompletion( this.TspLinkResetTimeout ).Status );
    }

    /// <summary> Reset TSP Link with error reporting. </summary>
    /// <returns> <c>true</c> of okay; otherwise, <c>false</c>. </returns>
    private bool TryResetTspLinkReportError()
    {
        this.IsTspLinkOnline = new bool?();
        bool affirmative;
        try
        {
            this.Session.EnableServiceRequestOnOperationCompletion();
            _ = this.Session.WriteLine( "tsplink.reset() waitcomplete(0)" );
            affirmative = !this.StatusSubsystem.TraceDeviceExceptionIfError( "resetting TSP Link;. " );
        }
        catch ( Pith.NativeException ex )
        {
            this.Session.TraceException( ex, "resetting TSP Link;. " );
            affirmative = false;
        }
        catch ( Exception ex )
        {
            this.StatusSubsystem.TraceOperation( ex, "resetting TSP Link;. " );
            affirmative = false;
        }

        if ( affirmative )
        {
            try
            {
                the.Session.SetLastAction( "awaiting completion after resetting TSP Link" );
                (bool TimedOut, Pith.ServiceRequests Status, TimeSpan Elapsed) = this.Session.AwaitStatusBitmask( Pith.ServiceRequests.RequestingService,
                                                                             TimeSpan.FromMilliseconds( 1000d ), TimeSpan.Zero, TimeSpan.FromMilliseconds( 10d ) );
                // to_do: fix
                _ = this.Session.ApplyStatusByte( Status );
                affirmative = !this.StatusSubsystem.TraceDeviceExceptionIfError( "resetting TSP Link;. " );
            }
            catch ( Pith.NativeException ex )
            {
                this.Session.TraceException( ex, "awaiting completion after resetting TSP Link;. " );
                affirmative = false;
            }
            catch ( Exception ex )
            {
                this.Session.TraceException( ex );
                affirmative = false;
            }
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Instrument '{this.ResourceNameCaption}' failed resetting TSP Link;. {Environment.NewLine}{new StackFrame( true ).UserCallStack()}" );
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
        this.ClearErrorQueue();

        // clear data queues.
        this.ClearDataQueue( this.NodeEntities() );
    }

    #endregion

    #endregion

    #region " check and report "

    /// <summary>
    /// Check and reports visa or device error occurred. Can only be used after receiving a full
    /// reply from the instrument.
    /// </summary>
    /// <param name="nodeNumber"> Specifies the remote node number to validate. </param>
    /// <param name="format">     Specifies the report format. </param>
    /// <param name="args">       Specifies the report arguments. </param>
    /// <returns> <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public override bool TraceVisaDeviceOperationOkay( int nodeNumber, string format, params object[] args )
    {
        this.Session.TraceInformation();
        bool success = !this.StatusSubsystem.TraceDeviceDeviceExceptionIfError( this.ControllerNode.Number, this.Session.StatusReadDelay,  format, args );
        if ( success && nodeNumber != this.ControllerNode.ControllerNodeNumber )
        {
            success = this.QueryErrorQueueCount( this.ControllerNode ) == 0;
            string details = string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args );
            _ = success
                ? cc.isr.VI.SessionLogger.Instance.LogInformation( $"Instrument {this.ResourceNameCaption} node {nodeNumber} done {details}" )
                : cc.isr.VI.SessionLogger.Instance.LogWarning( $"Instrument {this.ResourceNameCaption} node {nodeNumber} encountered errors {this.StatusSubsystem.Session.DeviceErrorReport}Details: {details}\n{new StackFrame( true ).UserCallStack() }" );
        }

        return success;
    }

    #endregion

}
