using System.Text;
using cc.isr.Std.EscapeSequencesExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp;

/// <summary> A link status subsystem. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-03-28 </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="LinkStatusSubsystem" /> class.
/// </remarks>
/// <param name="session"> The session. </param>
public class LinkStatusSubsystem( Pith.SessionBase session ) : Tsp.StatusSubsystemBase( Pith.SessionBase.Validated( session ) )
{
    #region " construction and cleanup "

    /// <summary> Creates a new StatusSubsystem. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session"> The session. </param>
    /// <returns> A StatusSubsystem. </returns>
    public static LinkStatusSubsystem Create( Pith.SessionBase session )
    {
        LinkStatusSubsystem? subsystem;
        try
        {
            subsystem = new LinkStatusSubsystem( session );
        }
        catch
        {
            throw;
        }

        return subsystem;
    }

    #endregion

    #region " error queue: node "

    /// <summary> Gets or sets the node entities. </summary>
    /// <remarks> Required for reading the system errors. </remarks>
    /// <value> The node entities. </value>
    public NodeEntityCollection NodeEntities { get; private set; } = [];

    private const string V = "_G.print(_G.string.format('%d',_G.errorqueue.count))";

    /// <summary> Gets or sets The ErrorQueueCount query command. </summary>
    /// <value> The ErrorQueueCount query command. </value>
    public string ErrorQueueCountQueryCommand { get; set; } = V;

    /// <summary> Queries error count on a remote node. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node"> . </param>
    /// <returns> The error count. </returns>
    public int QueryErrorQueueCount( NodeEntityBase node )
    {
        int? count;
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        this.ErrorQueueCountQueryCommand = node.IsController
            ? "_G.print(_G.string.format('%d',_G.errorqueue.count))"
            : $"_G.print(_G.string.format('%d',node[{node.Number}].errorqueue.count))";
        count = this.Session.Query( 0, this.ErrorQueueCountQueryCommand );
        return count.GetValueOrDefault( 0 );
    }

    /// <summary> Clears the error cache. </summary>
    public void ClearErrorCache()
    {
        this.DeviceErrorQueue = new Queue<cc.isr.VI.Syntax.Tsp.TspDeviceError>();
    }

    /// <summary> Queries if a given node exists. </summary>
    /// <param name="nodeNumber"> The node number. </param>
    /// <returns> True if it succeeds; otherwise, false. </returns>
    public bool NodeExists( int nodeNumber )
    {
        bool affirmative = true;
        return this.NodeEntities.Count > 0 && this.NodeEntities.Contains( NodeEntityBase.BuildKey( nodeNumber ) ) ? affirmative : affirmative;
    }

    /// <summary> Clears the error queue for the specified node. </summary>
    /// <param name="nodeNumber"> The node number. </param>
    private void ClearErrorQueue( int nodeNumber )
    {
        if ( !this.NodeExists( nodeNumber ) )
        {
            _ = this.Session.WriteLine( "node[{0}].errorqueue.clear() waitcomplete({0})", nodeNumber );
        }
    }

    /// <summary> Gets or sets the cached error queue count. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? ErrorQueueCount
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Clears the error queue. </summary>
    public void ClearErrorQueue()
    {
        if ( this.NodeEntities is null )
        {
            this.ClearErrorCache();
            if ( !string.IsNullOrWhiteSpace( this.Session.ClearErrorQueueCommand ) )
            {
                this.Session.SetLastAction( "clearing the error queue" );
                _ = this.Session.WriteLine( this.Session.ClearErrorQueueCommand );
                _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay );
            }
            this.ErrorQueueCount = new int?();
        }
        else
        {
            this.ClearErrorCache();
            foreach ( NodeEntityBase node in this.NodeEntities )
                this.ClearErrorQueue( node.Number );
        }
    }

    /// <summary> Gets the error queue. </summary>
    /// <value> A Queue of device errors. </value>
    protected Queue<cc.isr.VI.Syntax.Tsp.TspDeviceError> DeviceErrorQueue { get; private set; } = new Queue<cc.isr.VI.Syntax.Tsp.TspDeviceError>();

    /// <summary> Returns the queued error. </summary>
    /// <remarks> Sends the error print format query and reads back and parses the error. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="node"> . </param>
    /// <returns> The queued error. </returns>
    private cc.isr.VI.Syntax.Tsp.TspDeviceError QueryQueuedError( NodeEntityBase node )
    {
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );

        cc.isr.VI.Syntax.Tsp.TspDeviceError err = new( this.Session.NoErrorCompoundMessage );
        this.Session.LastNodeNumber = node.Number;
        this.Session.SetLastAction( "querying queued device errors" );
        if ( this.QueryErrorQueueCount( node ) > 0 )
        {
            string? message = node.IsController
                ? this.Session.QueryPrintStringFormatTrimEnd( $"%d,%s,%d,node{node.Number}", "_G.errorqueue.next()" )
                : this.Session.QueryPrintStringFormatTrimEnd( $"%d,%s,%d,node{node.Number}", $"node[{node.Number}].errorqueue.next()" );
            if ( !string.IsNullOrWhiteSpace( message ) )
                err.Parse( message! );
        }

        return err;
    }

    /// <summary>   Reads the device errors. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="node">         . </param>
    /// <param name="statusByte">   The status byte. </param>
    private void QueryDeviceErrors( NodeEntityBase node, ServiceRequests statusByte )
    {
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        cc.isr.VI.Syntax.Tsp.TspDeviceError deviceError;
        do
        {
            if ( this.Session.IsErrorBitSet( statusByte ) )
            {
                deviceError = this.QueryQueuedError( node );
                if ( deviceError.IsError )
                    this.DeviceErrorQueue.Enqueue( deviceError );
            }
            statusByte = this.Session.ReadStatusByte();
        }
        while ( this.Session.IsErrorBitSet( statusByte ) );
    }

    /// <summary> The device errors. </summary>
    /// <value> The device error builder. </value>
    protected System.Text.StringBuilder DeviceErrorBuilder { get; set; } = new();

    /// <summary>   Reads the device errors. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <param name="indent">       (Optional) The indent. </param>
    /// <returns>   <c>true</c> if device has errors, <c>false</c> otherwise. </returns>
    protected string QueryDeviceErrorsThis( ServiceRequests statusByte, string indent = "\t" )
    {
        this.ClearErrorCache();
        StringBuilder builder = new();
        string value;
        int errorCount = 0;
        foreach ( NodeEntityBase node in this.NodeEntities )
            this.QueryDeviceErrors( node, statusByte );

        int lastNodeNumber = -1;
        if ( this.DeviceErrorQueue is not null && this.DeviceErrorQueue.Count > 0 )
        {
            foreach ( cc.isr.VI.Syntax.Tsp.TspDeviceError e in this.DeviceErrorQueue )
            {
                if ( e.NodeNumber != lastNodeNumber )
                    _ = builder.AppendLine( $"{indent}Instrument {this.ResourceNameCaption} Node {e.NodeNumber} Errors:" );
                errorCount += 1;
                _ = builder.AppendLine( $"{indent}{indent}{e.CompoundErrorMessage}" );
            }
        }

        // todo: add class to hold the status register and last received and sent messages for each node.

        value = builder.ToString().TrimEnd( Environment.NewLine.ToCharArray() );
        if ( value != null && value.Length > 10 )
            _ = this.DeviceErrorBuilder.Append( builder.ToString().TrimEnd( Environment.NewLine.ToCharArray() ) );

        DeviceError lastError = this.DeviceErrorQueue is not null && this.DeviceErrorQueue.Count > 0
            ? this.DeviceErrorQueue.ElementAtOrDefault( this.DeviceErrorQueue.Count - 1 )
            : new( this.Session.NoErrorCompoundMessage );
        this.Session.LastErrorCompoundErrorMessage = lastError.CompoundErrorMessage;
        this.Session.HasDeviceError = lastError.IsError;
        this.Session.DeviceErrorReport = this.DeviceErrorBuilder.ToString();
        this.Session.DeviceErrorReportCount = errorCount;

        this.NotifyPropertyChanged( nameof( this.Session.DeviceErrorReport ) );
        return this.Session.DeviceErrorReport;
    }

    #endregion

    #region " identity "

    /// <summary> Gets or sets the identity query command. </summary>
    /// <value> The identity query command. </value>
    protected override string IdentificationQueryCommand { get; set; } = VI.Syntax.Tsp.LocalNode.IdentificationQueryCommand;

    /// <summary> Gets or sets the serial number query command. </summary>
    /// <value> The serial number query command. </value>
    protected override string SerialNumberQueryCommand { get; set; } = cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand;

    // was: "_G.print(string.format('%d',_G.localnode.serialno))";

    /// <summary> Queries the Identity. </summary>
    /// <remarks> Sends the <see cref="IdentificationQueryCommand">identity query</see>. </remarks>
    /// <returns>   A <see cref="string" />. </returns>
    public override string QueryIdentity()
    {
        if ( !string.IsNullOrWhiteSpace( this.IdentificationQueryCommand ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Requesting identity;. " );
            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
            this.WriteIdentificationQueryCommand();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Trying to read identity;. " );
            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
            string value = this.Session.ReadLineTrimEnd();
            value = value.ReplaceCommonEscapeSequences().Trim();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting identity to {value};. " );
            this.VersionInfo.Parse( value );
            this.VersionInfoBase = this.VersionInfo;
            this.Identity = this.VersionInfo.Identity;
        }

        return this.Identity;
    }

    #endregion

    #region " device error handling "

    /// <summary>   Handles device error from the <see cref="Pith.SessionBase"/>. </summary>
    /// <remarks>   2024-09-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Service request event information. </param>
    protected override void HandleDeviceErrorOccurred( object? sender, ServiceRequestEventArgs e )
    {
    }

    #endregion
}
