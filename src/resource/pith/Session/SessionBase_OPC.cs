namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    /// <summary> Gets or sets the wait command. </summary>
    /// <value> The wait command. </value>
    public string WaitCommand { get; set; } = Syntax.Ieee488Syntax.WaitCommand;

    /// <summary> Issues the wait command. </summary>
    public void Wait()
    {
        _ = this.WriteLine( this.WaitCommand );
    }

    private string _operationCompletedReplyMessage = cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue;

    /// <summary>   Gets or sets a message describing the operation completed reply. </summary>
    /// <value> A message describing the operation completed reply. </value>
    public virtual string OperationCompletedReplyMessage
    {
        get => this._operationCompletedReplyMessage;
        set => _ = this.SetProperty( ref this._operationCompletedReplyMessage, value );
    }

    private bool? _operationCompleted;

    /// <summary>
    /// Gets or sets the cached value indicating whether the last operation completed.
    /// </summary>
    /// <value>
    /// <c>null</c> if operation completed contains no value, <c>true</c> if operation completed;
    /// otherwise, <c>false</c>.
    /// </value>
    public bool? OperationCompleted
    {
        get => this._operationCompleted;
        set => _ = base.SetProperty( ref this._operationCompleted, value );
    }

    /// <summary> Gets or sets the operation completed query command. </summary>
    /// <remarks>
    /// SCPI: "*OPC?".
    /// <see cref="VI.Syntax.Ieee488Syntax.OperationCompletedQueryCommand"> </see>
    /// </remarks>
    /// <value> The operation completed query command. </value>
    public string OperationCompletedQueryCommand { get; set; } = Syntax.Ieee488Syntax.OperationCompletedQueryCommand;

    /// <summary>
    /// Issues the operation completion query, reads the reply and returns true if expected reply is
    /// received.
    /// </summary>
    /// <remarks>   2024-09-17. </remarks>
    /// <param name="timeoutMilliseconds">  (Optional) (10000) The timeout in milliseconds
    ///                                     Set to zero for an infinite (120 seconds) communication
    ///                                     timeout. </param>
    /// <returns>
    /// <c>true</c> if the operation completed reply matches the expected <see cref="OperationCompletedReplyMessage"/>
    /// value; otherwise <c>false</c>
    /// </returns>
    public bool? QueryOperationCompleted( int timeoutMilliseconds = 10000 )
    {
        this.MakeEmulatedReplyIfEmpty( this.OperationCompletedReplyMessage );
        string reply;
        if ( string.IsNullOrWhiteSpace( this.OperationCompletedQueryCommand ) )
            reply = this.EmulatedReply;
        else
        {
            _ = this.WriteLine( this.OperationCompletedQueryCommand );
            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );
            reply = this.ReadOperationCompleteReply( timeoutMilliseconds );
        }
        this.OperationCompleted = string.Equals( this.OperationCompletedReplyMessage, reply, StringComparison.OrdinalIgnoreCase );
        return this.OperationCompleted;
    }

    /// <summary>   Reads operation complete reply. </summary>
    /// <remarks>   2024-09-16. </remarks>
    /// <param name="timeoutMilliseconds">  (Optional) The timeout in milliseconds. </param>
    /// <returns>   The operation complete reply. </returns>
    public string ReadOperationCompleteReply( int timeoutMilliseconds = 10000 )
    {
        string reply = string.Empty;
        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );
        try
        {
            this.StoreCommunicationTimeout( TimeSpan.FromMilliseconds( timeoutMilliseconds ) );
            reply = this.ReadLineTrimEnd();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RestoreCommunicationTimeout();
        }
        return reply;
    }

    private StandardEvents _standardEventEnableOperationCompleteBitmask = StandardEvents.All & ~StandardEvents.RequestControl;

    /// <summary> Gets or sets the bitmask to enable standard event operation complete events. </summary>
    /// <value> The bitmask to enable standard event operation complete events [253, 0xFD]. </value>
    public StandardEvents StandardEventEnableOperationCompleteBitmask
    {
        get => this._standardEventEnableOperationCompleteBitmask;
        set => _ = base.SetProperty( ref this._standardEventEnableOperationCompleteBitmask, value );
    }

    private ServiceRequests _serviceRequestEnableOperationCompleteBitmask = ServiceRequests.All & ~ServiceRequests.RequestingService & ~ServiceRequests.MessageAvailable;

    /// <summary>   Gets or sets the service request operation complete enable bitmask. </summary>
    /// <value> The service request operation complete enable bitmask. </value>
    public ServiceRequests ServiceRequestEnableOperationCompleteBitmask
    {
        get => this._serviceRequestEnableOperationCompleteBitmask;
        set => _ = this.SetProperty( ref this._serviceRequestEnableOperationCompleteBitmask, value );
    }

    /// <summary> Gets the operation completed command. </summary>
    /// <remarks>
    /// SCPI: "*OPC".
    /// <see cref="VI.Syntax.Ieee488Syntax.OperationCompleteCommand"> </see>
    /// </remarks>
    /// <value> The operation complete query command. </value>
    public string? OperationCompleteCommand { get; set; } = Syntax.Ieee488Syntax.OperationCompleteCommand;

    /// <summary> Gets the supports operation complete. </summary>
    /// <value> The supports operation complete. </value>
    public bool SupportsOperationComplete => !string.IsNullOrWhiteSpace( this.OperationCompleteCommand );

    /// <summary> Issue operation complete. </summary>
    /// <remarks>
    /// When the *OPC command is sent, the instrument exits from the Operation Complete Command Idle
    /// State (OCIS) And enters the Operation Complete Command Active State (OCAS). In OCAS, the
    /// instrument continuously monitors the No-Operation-Pending flag. After the last pending
    /// overlapped command Is complete (NoOperation-Pending flag set to true), the Operation Complete
    /// (OPC) bit in the Standard Event Status Register sets, And the instrument goes back into OCIS.
    /// </remarks>
    public void IssueOperationComplete()
    {
        if ( this.SupportsOperationComplete )
            _ = this.WriteLine( this.OperationCompleteCommand! );
    }

    /// <summary>
    /// Enables service requests upon detection of completion. <para>
    /// Writes the standard and service bitmasks to unmask events to let the device set the Event
    /// Summary bit (ESB) <see cref="Pith.ServiceRequests.StandardEventSummary"/> and Main Summary
    /// Status (MSS) upon any of the unmasked SCPI events. Uses *ESE to select (unmask) the events
    /// that set the ESB and *SRE to select (unmask) the events that will set the Main Summary
    /// Status (MSS) bit for requesting service. Also issues *OPC.</para><para>
    /// This sets the default bitmasks.</para>
    /// </summary>
    /// <remarks>   2024-09-16. </remarks>
    /// <param name="readAfterWrite">   (Optional) True to read after write. </param>
    public void EnableServiceRequestOnOperationCompletion( bool readAfterWrite = false )
    {
        this.EnableServiceRequestOnOperationCompletion( this.RegistersBitmasksSettings.StandardEventEnableOperationCompleteBitmask,
            this.RegistersBitmasksSettings.ServiceRequestEnableOperationCompleteBitmask, readAfterWrite );
    }

    /// <summary>
    /// Enables service requests upon detection of completion. <para>
    /// Writes the standard and service bitmasks to unmask events to let the device set the Event
    /// Summary bit (ESB) <see cref="Pith.ServiceRequests.StandardEventSummary"/> and Main Summary
    /// Status (MSS) upon any of the unmasked SCPI events. Uses *ESE to select (unmask) the events
    /// that set the ESB and *SRE to select (unmask) the events that will set the Main Summary
    /// Status (MSS) bit for requesting service. Also issues *OPC.</para><para>
    /// This sets the default bitmasks.</para>
    /// </summary>
    /// <remarks>   2024-09-16. </remarks>
    /// <param name="standardEventEnableBitmask">   The standard event enable bitmask. </param>
    /// <param name="serviceRequestEnableBitmask">  The service request enable bitmask. </param>
    /// <param name="readAfterWrite">               (Optional) True to read after write. </param>
    public void EnableServiceRequestOnOperationCompletion( int standardEventEnableBitmask, int serviceRequestEnableBitmask, bool readAfterWrite = false )
    {
        if ( readAfterWrite )
            this.ApplyStandardServiceRequestEnableBitmasks( standardEventEnableBitmask, serviceRequestEnableBitmask );
        else
            this.WriteStandardServiceRequestEnableBitmasks( standardEventEnableBitmask, serviceRequestEnableBitmask );

        // update the operation completion bitmasks.
        if ( this.StandardEventEnableBitmask.GetValueOrDefault( StandardEvents.None ) != StandardEvents.None )
            this.StandardEventEnableOperationCompleteBitmask = this.StandardEventEnableBitmask!.Value;

        if ( this.ServiceRequestEnableBitmask.GetValueOrDefault( ServiceRequests.None ) != ServiceRequests.None )
            this.ServiceRequestEnableOperationCompleteBitmask = this.ServiceRequestEnableBitmask!.Value;
    }

    /// <summary> Waits for completion of last operation. </summary>
    /// <remarks> Assumes requesting service event is registered with the instrument. </remarks>
    /// <param name="timeout"> Specifies the time to wait for the instrument to return operation
    /// completed. </param>
    /// <returns> A (TimedOut As Boolean, Status As VI.Pith.ServiceRequests) </returns>
    public (bool TimedOut, ServiceRequests Status, TimeSpan Elapsed) AwaitOperationCompletion( TimeSpan timeout )
    {
        return this.AwaitOperationCompletion( this.ServiceRequestEnableOperationCompleteBitmask, timeout );
    }

    /// <summary> Waits for completion of last operation. </summary>
    /// <param name="value">   The value. </param>
    /// <param name="timeout"> Specifies the time to wait for the instrument to return operation
    /// completed. </param>
    /// <returns> A (TimedOut As Boolean, Status As VI.Pith.ServiceRequests) </returns>
    public (bool TimedOut, ServiceRequests Status, TimeSpan Elapsed) AwaitOperationCompletion( ServiceRequests value, TimeSpan timeout )
    {
        return this.AwaitStatusBitmask( value, timeout, this.StatusReadDelay, this.StatusReadTurnaroundTime );
    }

}
