using cc.isr.Enums;
using cc.isr.Std.TimeSpanExtensions;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " service request register events: report "

    /// <summary>
    /// Returns the value of the service request status register (SRQ) byte plus the description if its
    /// constituent bit.
    /// </summary>
    /// <remarks>   2024-09-03. </remarks>
    /// <param name="delimiter">    (Optional) The delimiter. </param>
    /// <returns>   The structured report. </returns>
    public string BuildServiceRequestValueDescription( string delimiter = "; " )
    {
        if ( string.IsNullOrWhiteSpace( delimiter ) ) delimiter = "; ";

        System.Text.StringBuilder builder = new();

        foreach ( ServiceRequests element in Enum.GetValues( typeof( ServiceRequests ) ) )
        {
            if ( element != ServiceRequests.None && element != ServiceRequests.All && (element & this.ServiceRequestStatus) != 0 )
            {
                if ( builder.Length > 0 )
                    _ = builder.Append( delimiter );

                _ = builder.Append( element.Description() );
            }
        }
        if ( builder.Length > 0 )
        {
            _ = builder.Append( ")." );
            _ = builder.Insert( 0, $"0x{( int ) this.ServiceRequestStatus:X2} (" );
        }

        return builder.ToString();
    }

    /// <summary>
    /// Returns the value of the service request status register (SRQ) byte plus the description if its
    /// constituent bit.
    /// </summary>
    /// <remarks>   2024-09-03. </remarks>
    /// <param name="value">        Specifies the statusByte that was read from the service request
    ///                             register. </param>
    /// <param name="delimiter">    (Optional) The delimiter. </param>
    /// <returns>   The structured report. </returns>
    public string BuildValueDescription( ServiceRequests value, string delimiter = "; " )
    {
        if ( string.IsNullOrWhiteSpace( delimiter ) ) delimiter = "; ";
        this.ServiceRequestStatus = value;
        return this.BuildServiceRequestValueDescription( delimiter );
    }

    #endregion

    #region " suspension "

    /// <summary> Number of suspended service requested. </summary>

    /// <summary> Gets or sets the number of services requested while suspended. </summary>
    /// <statusByte> The number of services requested. </statusByte>
    public int SuspendedServiceRequestedCount
    {
        get;
        protected set => _ = base.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the type of the service request. </summary>
    /// <statusByte> The type of the service request. </statusByte>
    public string? ServiceRequestType { get; set; }

    /// <summary> Gets or sets the service request handling suspended. </summary>
    /// <statusByte> The service request handling suspended. </statusByte>
    public bool ServiceRequestHandlingSuspended { get; set; }

    /// <summary> Resume service request handing. </summary>
    public virtual void ResumeServiceRequestHanding()
    {
        this.SetLastAction( "resuming service request handing" );
        if ( this.ServiceRequestEventEnabled )
        {
            this.ClearLastError();
            this.SuspendedServiceRequestedCount = 0;
            this.ServiceRequestType = string.Empty;
            this.DiscardServiceRequests();
        }

        this.ServiceRequestHandlingSuspended = false;
    }

    /// <summary> Suspends the service request handling. </summary>
    public virtual void SuspendServiceRequestHanding()
    {
        this.SetLastAction( "suspending service request handing" );
        if ( this.ServiceRequestEventEnabled )
        {
            this.ClearLastError();
            this.SuspendedServiceRequestedCount = 0;
            this.ServiceRequestType = string.Empty;
            this.DiscardServiceRequests();
        }

        this.ServiceRequestHandlingSuspended = true;
    }

    #endregion

    #region " service request handler "

    /// <summary> Notifies of the ServiceRequested event. </summary>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected virtual void OnServiceRequested( EventArgs e )
    {
        this.NotifyServiceRequested( e );
    }

    /// <summary> Removes the Service Requested event handlers. </summary>
    protected void RemoveServiceRequestedEventHandlers()
    {
        EventHandler<EventArgs>? handler = this.ServiceRequested;
        if ( handler is not null )
        {
            foreach ( Delegate? item in handler.GetInvocationList() )
            {
                handler -= ( EventHandler<EventArgs> ) item;
            }
        }
    }

    /// <summary> Event queue for all listeners interested in ServiceRequested events. </summary>
    /// <remarks> A custom Event is used here to allow us to synchronize with the event listeners.
    /// Using a custom Raise method lets you iterate through the delegate list.
    /// </remarks>
    public event EventHandler<EventArgs>? ServiceRequested;

    /// <summary>
    /// Synchronously invokes the <see cref="ServiceRequested">ServiceRequested Event</see>. Not thread safe.
    /// </summary>
    /// <param name="e"> The <see cref="EventArgs" /> instance containing the event data. </param>
    protected void NotifyServiceRequested( EventArgs e )
    {
        this.ServiceRequested?.Invoke( this, e );
    }

    #endregion

    #region " srq enable "

    /// <summary> Gets or sets the emulated status byte. </summary>
    /// <statusByte> The emulated status byte. </statusByte>
    public ServiceRequests EmulatedStatusByte { get; set; }

    /// <summary> Makes emulated status byte. </summary>
    /// <param name="value"> The emulated statusByte. </param>
    public void MakeEmulatedReply( ServiceRequests value )
    {
        this.EmulatedStatusByte = value;
    }

    /// <summary> Makes emulated status byte if none. </summary>
    /// <param name="value"> The emulated statusByte. </param>
    public void MakeEmulatedReplyIfEmpty( ServiceRequests value )
    {
        if ( this.EmulatedStatusByte == ServiceRequests.None )
        {
            this.MakeEmulatedReply( value );
        }
    }

    /// <summary> Queries if a service request is enabled. </summary>
    /// <param name="bitmask"> The bit mask. </param>
    /// <returns> <c>true</c> if a service request is enabled; otherwise <c>false</c> </returns>
    public bool IsServiceRequestEnabled( ServiceRequests bitmask )
    {
        return this.ServiceRequestEventEnabled && (this.ServiceRequestEnableBitmask & bitmask) != 0;
    }

    /// <summary>   Gets or sets the service request events enable bitmask. </summary>
    /// <statusByte> The service request events enable bitmask. </statusByte>
    public ServiceRequests ServiceRequestEnableEventsBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = ServiceRequests.All & ~ServiceRequests.RequestingService;

    /// <summary>   Gets or sets the cached service request enable bit mask. </summary>
    /// <statusByte>
    /// <c>null</c> if statusByte is not known; otherwise <see cref="VI.Pith.ServiceRequests.All"/> and
    /// not <see cref="VI.Pith.ServiceRequests.RequestingService"/>
    /// </statusByte>
    public ServiceRequests? ServiceRequestEnableBitmask
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the service request enable query command. </summary>
    /// <remarks>
    /// <code>
    /// SCPI: "*SRE?".
    /// TSP: _G.status.request_enable={0}
    /// </code>
    /// <see cref="VI.Syntax.Ieee488Syntax.ServiceRequestEnableQueryCommand"> </see>
    /// </remarks>
    /// <statusByte> The service request enable command format. </statusByte>
    public string ServiceRequestEnableQueryCommand
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    } = Syntax.Ieee488Syntax.ServiceRequestEnableQueryCommand;

    /// <summary> Gets or sets the service request enable query command. </summary>
    /// <remarks>
    /// <code>
    /// TSP: _G.status.request_enable={0}
    /// </code>
    /// <see cref="VI.Syntax.Tsp.Node.ServiceRequestEnableCommandFormat"> </see>
    /// </remarks>
    /// <statusByte> The service request enable command format. </statusByte>
    public string ServiceRequestEnableQueryNodeCommandFormat
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    } = string.Empty;

    /// <summary> Gets or sets the service request enable command format. </summary>
    /// <remarks>
    /// SCPI: "*SRE {0:D}".
    /// <see cref="VI.Syntax.Ieee488Syntax.ServiceRequestEnableCommandFormat"> </see>
    /// </remarks>
    /// <statusByte> The service request enable command format. </statusByte>
    public string ServiceRequestEnableCommandFormat
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    } = Syntax.Ieee488Syntax.ServiceRequestEnableCommandFormat;

    /// <summary> Queries the service request enable bit mask. </summary>
    /// <returns>
    /// <c>null</c> if statusByte is not known; otherwise <see cref="int">Service Requests</see>.
    /// </returns>
    public int QueryServiceRequestEnableBitmask()
    {
        int value = default;
        if ( !string.IsNullOrWhiteSpace( this.ServiceRequestEnableQueryCommand ) )
        {
            this.SetLastAction( "querying status register enable bitmask" );
            value = this.Query( 0, this.ServiceRequestEnableQueryCommand! );
            this.ServiceRequestEnableBitmask = ( ServiceRequests ) value;
        }
        return value;
    }

    /// <summary> Gets the supports service request enable query. </summary>
    /// <statusByte> The supports service request enable query. </statusByte>
    public bool SupportsServiceRequestEnableQuery => !string.IsNullOrWhiteSpace( this.ServiceRequestEnableQueryCommand );

    /// <summary>
    /// Apply (WriteEnum and then read) bitmask setting the device to turn on the Requesting Service
    /// (RQS) / Main Summary Status (MSS) bit and issue an SRQ upon any of the SCPI events unmasked
    /// by the bitmask. Uses *SRE to select (unmask) the events that will issue an SRQ.
    /// </summary>
    /// <param name="bitmask"> The bitmask; zero to disable all events. </param>
    public void ApplyStatusByteEnableBitmask( int bitmask )
    {
        this.WriteServiceRequestEnableBitmask( bitmask );
        _ = this.QueryServiceRequestEnableBitmask();
    }

    /// <summary>
    /// Apply (WriteEnum and then read) bitmask setting the device to turn on the Requesting Service
    /// (RQS) / Main Summary Status (MSS) bit and issue an SRQ upon any of the SCPI events unmasked
    /// by the bitmask. Uses *SRE to select (unmask) the events that will issue an SRQ.
    /// </summary>
    /// <param name="commandFormat"> The service request enable command format. </param>
    /// <param name="bitmask">       The bitmask; zero to disable all events. </param>
    public void ApplyStatusByteEnableBitmask( string commandFormat, int bitmask )
    {
        this.ServiceRequestEnableCommandFormat = commandFormat;
        this.ApplyStatusByteEnableBitmask( bitmask );
    }

    /// <summary> Writes a service request enable bitmask. </summary>
    /// <param name="bitmask"> The bitmask; zero to disable all events. </param>
    public void WriteServiceRequestEnableBitmask( int bitmask )
    {
        if ( string.IsNullOrWhiteSpace( this.ServiceRequestEnableCommandFormat ) )
        {
            this.ServiceRequestEnableBitmask = ServiceRequests.None;
        }
        else
        {
            _ = this.WriteLine( this.ServiceRequestEnableCommandFormat!, bitmask );
            this.ServiceRequestEnableBitmask = ( ServiceRequests ) bitmask;
        }
    }

    /// <summary> Returns <c>true</c> is this instrument supports service request enable. </summary>
    /// <statusByte> The supports service request enable. </statusByte>
    public bool SupportsServiceRequestEnable => !string.IsNullOrWhiteSpace( this.ServiceRequestEnableCommandFormat );

    /// <summary> Emulate service request. </summary>
    /// <param name="statusByte"> The status byte. </param>
    public void EmulateServiceRequest( ServiceRequests statusByte )
    {
        this.EmulatedStatusByte = statusByte;
        if ( this.ServiceRequestEventEnabled )
            this.OnServiceRequested( EventArgs.Empty );
    }

    /// <summary>
    /// Gets or sets or set the sentinel indication if a service request event handler was enabled
    /// and registered.
    /// </summary>
    /// <statusByte>
    /// <c>true</c> if service request event is enabled and registered; otherwise, <c>false</c>.
    /// </statusByte>
    public abstract bool ServiceRequestEventEnabled { get; set; }

    /// <summary> Discard pending service requests. </summary>
    public abstract void DiscardServiceRequests();

    /// <summary> Enables and adds the service request event handler. </summary>
    public abstract void EnableServiceRequestEventHandler();

    /// <summary> Disables and removes the service request event handler. </summary>
    public abstract void DisableServiceRequestEventHandler();

    #endregion

    #region " service request status "

    /// <summary> Bitmask statusByte changed. </summary>
    /// <param name="bitmask">  The bit mask. </param>
    /// <param name="oldValue"> The old statusByte. </param>
    /// <param name="newValue"> The statusByte. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    private static bool BitmaskValueChanged( int bitmask, int oldValue, int newValue )
    {
        return (bitmask & oldValue) != (bitmask & newValue);
    }

    /// <summary> Applies the service request described by statusByte. </summary>
    /// <remarks>
    /// This method is called only upon reading a new status byte. Therefore, the property change event is
    /// invoked for all status byte events.
    /// </remarks>
    /// <param name="statusByte"> Specifies the status byte that was read from the service request register. </param>
    public void ApplyStatusByte( ServiceRequests statusByte )
    {
        // save the new status statusByte
        this.ServiceRequestStatus = statusByte;

        // note that the status byte reflects that actual relevant statusByte. This statusByte might be the same as the
        // last statusByte that was fetched from the instrument. Therefore, all status events must elicit a property change
        // event.

        this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.ErrorAvailable ) ) );

        this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.MessageAvailable ) ) );

        this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasMeasurementEvent ) ) );

        this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasSystemEvent ) ) );

        this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasQuestionableEvent ) ) );

        this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasStandardEvent ) ) );

        this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.RequestedService ) ) );

        this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasOperationEvent ) ) );

        // this must be the last property change notification
        this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.ServiceRequestStatus ) ) );

        // ignore device error in the presence of a message in order to prevent query interrupted errors.
        // error notification does not cause fetching device error if a message is available.
        if ( this.IsErrorBitSet( statusByte ) && !this.IsMessageAvailableBitSet( statusByte ) )
        {
            this.QueryAndReportDeviceErrors( statusByte );
            this.NotifyDeviceErrorOccurred( new ServiceRequestEventArgs( statusByte ) );
        }

        // update the status caption
        this.StatusRegisterCaption = string.Format( RegisterValueFormat, ( int ) statusByte );
    }

    /// <summary> Applies the service request described by statusByte. </summary>
    /// <param name="oldValue"> The old statusByte. </param>
    /// <param name="newValue"> The statusByte. </param>
    [Obsolete( "We must assume that every status byte is new" )]
    protected void ApplyStatusByte( ServiceRequests oldValue, ServiceRequests newValue )
    {
        // save the new status statusByte
        this.ServiceRequestStatus = newValue;

        // error notification does not cause fetching device error is a message is available.
        if ( BitmaskValueChanged( ( int ) this.ErrorAvailableBitmask, ( int ) oldValue, ( int ) newValue ) )
            this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.ErrorAvailable ) ) );

        if ( BitmaskValueChanged( ( int ) this.MessageAvailableBitmask, ( int ) oldValue, ( int ) newValue ) )
            this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.MessageAvailable ) ) );

        if ( BitmaskValueChanged( ( int ) this.MeasurementEventBitmask, ( int ) oldValue, ( int ) newValue ) )
            this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasMeasurementEvent ) ) );

        if ( BitmaskValueChanged( ( int ) this.SystemEventBitmask, ( int ) oldValue, ( int ) newValue ) )
            this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasSystemEvent ) ) );

        if ( BitmaskValueChanged( ( int ) this.QuestionableEventBitmask, ( int ) oldValue, ( int ) newValue ) )
            this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasQuestionableEvent ) ) );

        if ( BitmaskValueChanged( ( int ) this.StandardEventSummaryBitmask, ( int ) oldValue, ( int ) newValue ) )
            this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasStandardEvent ) ) );

        if ( BitmaskValueChanged( ( int ) this.RequestingServiceBitmask, ( int ) oldValue, ( int ) newValue ) )
            this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.RequestedService ) ) );

        if ( BitmaskValueChanged( ( int ) this.OperationEventSummaryBitmask, ( int ) oldValue, ( int ) newValue ) )
            this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.HasOperationEvent ) ) );

        // Notify is status changed
        if ( !newValue.Equals( oldValue ) )
            this.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.ServiceRequestStatus ) ) );

        // ignore device error in the presence of a message in order to prevent query interrupted errors.
        if ( !this.MessageAvailable && this.ErrorAvailable )
            this.NotifyDeviceErrorOccurred( new ServiceRequestEventArgs( newValue ) );

        // update the status caption
        this.StatusRegisterCaption = string.Format( RegisterValueFormat, ( int ) newValue );
    }

    /// <summary> Gets or sets the cached service request Status. </summary>
    /// <remarks>
    /// The service request status is posted to be parsed by the status subsystem that is specific to
    /// the instrument at hand. This is critical to the proper workings of the status subsystem. The
    /// service request status is posted asynchronously. This may not be processed fast enough to
    /// determine the next action. Not sure how to address this at this time.
    /// </remarks>
    /// <statusByte>
    /// <c>null</c> if statusByte is not known; otherwise <see cref="VI.Pith.ServiceRequests">Service
    /// Requests</see>.
    /// </statusByte>
    public ServiceRequests ServiceRequestStatus { get; private set; }

    /// <summary>
    /// Reads the status register, applies the value to elicit the property change events on the
    /// status bits and handle the device error as necessary.
    /// </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <returns>
    /// <c>null</c> if statusByte is not known; otherwise <see cref="VI.Pith.ServiceRequests">Service
    /// Requests</see>.
    /// </returns>
    [Obsolete( "Use Session.ApplyStatusByte( Session.ReadStatusByte() )" )]
    public ServiceRequests ReadStatusRegister()
    {
        ServiceRequests statusByte = this.ReadStatusByte();
        this.ApplyStatusByte( statusByte );
        return statusByte;
    }

    /// <summary> The unknown register statusByte caption. </summary>
    /// <statusByte> The unknown register statusByte caption. </statusByte>
    protected static string? UnknownRegisterValueCaption { get; set; } = "0x..";

    /// <summary> The register statusByte format. </summary>
    /// <statusByte> The register statusByte format. </statusByte>
    protected static string? RegisterValueFormat { get; set; } = "0x{0:X2}";

    /// <summary> Gets or sets the status register caption. </summary>
    /// <statusByte> The status register caption. </statusByte>
    public string? StatusRegisterCaption
    {
        get;
        set => _ = base.SetProperty( ref field, value ?? string.Empty );
    } = "0x..";

    /// <summary>   Gets or sets the 'status byte query' command. </summary>
    /// <statusByte> The 'status byte query' command. </statusByte>
    public string StatusByteQueryCommand
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = Syntax.Ieee488Syntax.StatusByteQueryCommand;


    #endregion

    #region " srq wait for "

    /// <summary> The status read turnaround time. </summary>

    /// <summary> Gets or sets the status read turnaround time. </summary>
    /// <statusByte> The status read turnaround time. </statusByte>
    public TimeSpan StatusReadTurnaroundTime
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   Await service request. </summary>
    /// <remarks>
    /// Delay defaults to <see cref="StatusReadDelay"/> and poll delay default to
    /// <see cref="StatusReadTurnaroundTime"/>
    /// </remarks>
    /// <param name="timeout">          The timeout. </param>
    /// <returns>
    /// A ( bool, ServiceRequests, TimeSpan )) (true if timed out, status byte, elapsed time)
    /// </returns>
    public virtual (bool TimedOut, ServiceRequests StatusByte, TimeSpan Elapsed) AwaitServiceRequest( TimeSpan timeout )
    {
        return this.AwaitStatusBitmask( this.RequestingServiceBitmask, timeout );
    }

    /// <summary>   Await status register bitmask. </summary>
    /// <remarks>
    /// Delay defaults to <see cref="StatusReadDelay"/> and poll delay default to
    /// <see cref="StatusReadTurnaroundTime"/>
    /// </remarks>
    /// <param name="bitmask">          Specifies the anticipated status bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    /// <returns>
    /// A ( bool, ServiceRequests, TimeSpan )) (true if timed out, status byte, elapsed time)
    /// </returns>
    public virtual (bool TimedOut, ServiceRequests StatusByte, TimeSpan Elapsed) AwaitStatusBitmask( ServiceRequests bitmask, TimeSpan timeout )
    {
        // emulate the reply for disconnected operations.
        this.MakeEmulatedReplyIfEmpty( bitmask );
        CancellationTokenSource cts = new();
        Task<(bool TimedOut, ServiceRequests StatusByte, TimeSpan Elapsed)> t = StartAwaitingBitmaskTask( bitmask, timeout, this.StatusReadDelay,
                                                                    this.StatusReadTurnaroundTime, this.ReadStatusByte );
        t.Wait();
        return t.Result;
    }

    /// <summary>   Await status register bitmask. </summary>
    /// <remarks>
    /// Delay defaults to <see cref="StatusReadDelay"/> and poll delay default to
    /// <see cref="StatusReadTurnaroundTime"/>
    /// </remarks>
    /// <param name="bitmask">  Specifies the anticipated status bitmask. </param>
    /// <param name="timeout">  The timeout. </param>
    /// <returns>   A ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed time) </returns>
    public virtual (bool TimedOut, int StatusByte, TimeSpan Elapsed) AwaitStatusBitmask( int bitmask, TimeSpan timeout )
    {
        // emulate the reply for disconnected operations.
        this.MakeEmulatedReplyIfEmpty( bitmask );
        CancellationTokenSource cts = new();
        Task<(bool TimedOut, int StatusByte, TimeSpan Elapsed)> t = StartAwaitingBitmaskTask( bitmask, timeout, this.StatusReadDelay,
                                                            this.StatusReadTurnaroundTime, () => ( int ) this.ReadStatusByte() );
        t.Wait();
        return t.Result;
    }

    /// <summary>   Await status register bitmask. </summary>
    /// <remarks>
    /// Delay defaults to <see cref="StatusReadDelay"/> and poll delay default to
    /// <see cref="StatusReadTurnaroundTime"/>
    /// </remarks>
    /// <param name="bitmask">          Specifies the anticipated status bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    /// <returns>   A ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed time) </returns>
    public virtual (bool TimedOut, int StatusByte, TimeSpan Elapsed) AwaitStatusBitmask( byte bitmask, TimeSpan timeout )
    {
        // emulate the reply for disconnected operations.
        this.MakeEmulatedReplyIfEmpty( bitmask );
        CancellationTokenSource cts = new();
        Task<(bool TimedOut, int StatusByte, TimeSpan Elapsed)> t = StartAwaitingBitmaskTask( bitmask, timeout, this.StatusReadDelay, this.StatusReadTurnaroundTime, () => ( byte ) this.ReadStatusByte() );
        t.Wait();
        return t.Result;
    }

    /// <summary>   Await status register bitmask. </summary>
    /// <remarks>
    /// Delay defaults to <see cref="StatusReadDelay"/> and poll delay default to
    /// <see cref="StatusReadTurnaroundTime"/>
    /// </remarks>
    /// <param name="bitmask">          Specifies the anticipated status bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="queryStatusError"> The status read. </param>
    /// <returns>
    /// A Tuple: ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed time)
    /// </returns>
    public virtual (bool TimedOut, int StatusByte, TimeSpan Elapsed) AwaitStatusBitmask( byte bitmask, TimeSpan timeout, Func<(bool, string, int)> queryStatusError )
    {
        // emulate the reply for disconnected operations.
        this.MakeEmulatedReplyIfEmpty( bitmask );
        CancellationTokenSource cts = new();
        Task<(bool TimedOut, int StatusByte, TimeSpan Elapsed)> t = StartAwaitingBitmaskTask( bitmask, timeout, this.StatusReadDelay, this.StatusReadTurnaroundTime, queryStatusError );
        t.Wait();
        return t.Result;
    }

    /// <summary>   Await status. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="queryStatusDone">  The query status done. </param>
    /// <returns>
    /// A Tuple: ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed time)
    /// </returns>
    public virtual (bool TimedOut, int StatusByte, TimeSpan Elapsed) AwaitStatus( TimeSpan timeout, Func<(bool, int)> queryStatusDone )
    {
        // emulate the reply for disconnected operations.
        this.MakeEmulatedReplyIfEmpty( 0 );
        CancellationTokenSource cts = new();
        Task<(bool TimedOut, int StatusByte, TimeSpan Elapsed)> t = StartAwaitingStatusTask( timeout, this.StatusReadDelay, this.StatusReadTurnaroundTime, queryStatusDone );
        t.Wait();
        return t.Result;
    }

    /// <summary>   Await status register bitmask. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <param name="bitmask">      Specifies the anticipated status bitmask. </param>
    /// <param name="timeout">      The timeout. </param>
    /// <param name="onsetDelay">   The time to wait before starting the queries. </param>
    /// <param name="pollInterval"> The time to wait between queries. </param>
    /// <returns>   A ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed time) </returns>
    public (bool TimedOut, ServiceRequests StatusByte, TimeSpan Elapsed) AwaitStatusBitmask( ServiceRequests bitmask, TimeSpan timeout, TimeSpan onsetDelay,
                                                                                             TimeSpan pollInterval )
    {
        // emulate the reply for disconnected operations.
        this.MakeEmulatedReplyIfEmpty( bitmask );
        CancellationTokenSource cts = new();
        Task<(bool TimedOut, ServiceRequests StatusByte, TimeSpan Elapsed)> t = StartAwaitingBitmaskTask( bitmask, timeout, onsetDelay, pollInterval, this.ReadStatusByte );
        t.Wait();
        return t.Result;
    }

    #endregion

    #region " tasks starting methods "

    /// <summary>
    /// Starts a task waiting for a bitmask. The task complete after a timeout or if the action
    /// function statusByte matches the bitmask statusByte.
    /// </summary>
    /// <remarks>
    /// The task timeout is included in the task function. Otherwise, upon Wit(timeout), the task
    /// deadlocks attempting to get the task result. For more information see
    /// https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html. That document is short
    /// on examples for how to resolve this issue.
    /// </remarks>
    /// <param name="bitmask">          Specifies the anticipated bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="action">           The action. </param>
    /// <returns>
    /// A Threading.Tasks.Task(Of ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed
    /// time).
    /// </returns>
    public static Task<(bool TimedOut, int StatusByte, TimeSpan Elpased)> StartAwaitingBitmaskTask( int bitmask, TimeSpan timeout, Func<int> action )
    {
        return Task.Factory.StartNew<(bool, int, TimeSpan)>( () =>
        {
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            int reading = action.Invoke();
            if ( timeout <= TimeSpan.Zero )
                return (false, reading, TimeSpan.Zero);
            bool timedOut = sw.ElapsedTicks >= ticks;
            while ( (bitmask & reading) != bitmask && !timedOut )
            {
                System.Threading.Thread.SpinWait( 1 );
                SessionBase.DoEventsAction?.Invoke();
                reading = action.Invoke();
                timedOut = sw.ElapsedTicks >= ticks;
            }

            return (timedOut, reading, sw.Elapsed);
        } );
    }

    /// <summary>
    /// Starts a task waiting for a bitmask. The task complete after a timeout or if the action
    /// function statusByte matches the bitmask statusByte.
    /// </summary>
    /// <remarks>
    /// The task timeout is included in the task function. Otherwise, upon Wit(timeout), the task
    /// deadlocks attempting to get the task result. For more information see
    /// https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html. That document is short
    /// on examples for how to resolve this issue.
    /// </remarks>
    /// <param name="bitmask">          Specifies the anticipated bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="action">           The action. </param>
    /// <returns>
    /// A Threading.Tasks.Task(Of ( bool, int?, TimeSpan )) (true if timed out, status byte, elapsed
    /// time).
    /// </returns>
    public static Task<(bool TimedOut, int? StatusByte, TimeSpan Elapsed)> StartAwaitingBitmaskTask( int bitmask, TimeSpan timeout, Func<int?> action )
    {
        return Task.Factory.StartNew<(bool, int?, TimeSpan)>( () =>
        {
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            int? reading = action.Invoke();
            if ( timeout <= TimeSpan.Zero )
                return (false, reading, TimeSpan.Zero);
            bool timedOut = sw.ElapsedTicks >= ticks;
            while ( (bitmask & reading) != bitmask && !timedOut )
            {
                System.Threading.Thread.SpinWait( 1 );
                SessionBase.DoEventsAction?.Invoke();
                reading = action.Invoke();
                timedOut = sw.ElapsedTicks >= ticks;
            }

            return (timedOut, reading, sw.Elapsed);
        } );
    }

    /// <summary>
    /// Starts a task waiting for a bitmask. The task complete after a timeout or if the action
    /// function statusByte matches the bitmask statusByte.
    /// </summary>
    /// <remarks>
    /// The task timeout is included in the task function. Otherwise, upon Wit(timeout), the task
    /// deadlocks attempting to get the task result. For more information see
    /// https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html. That document is short
    /// on examples for how to resolve this issue.
    /// </remarks>
    /// <param name="bitmask">          Specifies the anticipated bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="action">           The action. </param>
    /// <returns>
    /// A Threading.Tasks.Task(Of ( bool, ServiceRequests, TimeSpan )) (true if timed out, status
    /// byte, elapsed time).
    /// </returns>
    public static Task<(bool TimedOut, ServiceRequests StatusByte, TimeSpan Elapsed)> StartAwaitingBitmaskTask( ServiceRequests bitmask, TimeSpan timeout,
                                                                                                                Func<ServiceRequests> action )
    {
        return Task.Factory.StartNew<(bool, ServiceRequests, TimeSpan)>( () =>
        {
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            ServiceRequests reading = action.Invoke();
            if ( timeout <= TimeSpan.Zero )
                return (false, reading, TimeSpan.Zero);
            bool timedOut = sw.ElapsedTicks >= ticks;
            while ( (bitmask & reading) != bitmask && !timedOut )
            {
                System.Threading.Thread.SpinWait( 1 );
                SessionBase.DoEventsAction?.Invoke();
                reading = action.Invoke();
                timedOut = sw.ElapsedTicks >= ticks;
            }

            return (timedOut, reading, sw.Elapsed);
        } );
    }

    /// <summary>
    /// Starts a task waiting for a bitmask. The task complete after a timeout or if the action
    /// function statusByte matches the bitmask statusByte.
    /// </summary>
    /// <remarks>
    /// The task timeout is included in the task function. Otherwise, upon Wit(timeout), the task
    /// deadlocks attempting to get the task result. For more information see
    /// https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html. That document is short
    /// on examples for how to resolve this issue.
    /// </remarks>
    /// <param name="bitmask">      Specifies the anticipated bitmask. </param>
    /// <param name="timeout">      The timeout. </param>
    /// <param name="onsetDelay">   The time to wait before starting the queries. </param>
    /// <param name="pollInterval"> Specifies time between serial polls. </param>
    /// <param name="action">       The action. </param>
    /// <returns>
    /// A Threading.Tasks.Task(Of ( bool, int?, TimeSpan )) (true if timed out, status byte, elapsed
    /// time).
    /// </returns>
    public static Task<(bool TimedOut, int? StatusByte, TimeSpan Elapsed)> StartAwaitingBitmaskTask( int bitmask, TimeSpan timeout,
                                                                                                     TimeSpan onsetDelay,
                                                                                                     TimeSpan pollInterval,
                                                                                                     Func<int?> action )
    {
        return Task.Factory.StartNew<(bool, int?, TimeSpan)>( () =>
        {
            _ = onsetDelay.SyncWait();
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            int? reading = action.Invoke();
            if ( timeout <= TimeSpan.Zero )
                return (false, reading, TimeSpan.Zero);
            bool timedOut = sw.ElapsedTicks >= ticks;
            while ( (bitmask & reading) != bitmask && !timedOut )
            {
                System.Threading.Thread.SpinWait( 1 );
                SessionBase.DoEventsAction?.Invoke();
                _ = pollInterval.SyncWait();
                reading = action.Invoke();
                timedOut = sw.ElapsedTicks >= ticks;
            }

            return (timedOut, reading, sw.Elapsed);
        } );
    }

    /// <summary>
    /// Starts a task waiting for a bitmask. The task complete after a timeout or if the action
    /// function statusByte matches the bitmask statusByte.
    /// </summary>
    /// <remarks>
    /// The task timeout is included in the task function. Otherwise, upon Wit(timeout), the task
    /// deadlocks attempting to get the task result. For more information see
    /// https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html. That document is short
    /// on examples for how to resolve this issue.
    /// </remarks>
    /// <param name="bitmask">          Specifies the anticipated bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="onsetDelay">       The time to wait before starting the queries. </param>
    /// <param name="pollInterval">     The interval between serial polls. </param>
    /// <param name="queryStatusError"> The status read. </param>
    /// <returns>
    /// A Threading.Tasks.Task(Of ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed
    /// time).
    /// </returns>
    public static Task<(bool TimedOut, int StatusByte, TimeSpan Elapsed)> StartAwaitingBitmaskTask( int bitmask, TimeSpan timeout,
                                                                                                    TimeSpan onsetDelay, TimeSpan pollInterval,
                                                                                                    Func<(bool hasError, string details, int statusByte)> queryStatusError )
    {
        return Task.Factory.StartNew<(bool, int, TimeSpan)>( () =>
        {
            _ = onsetDelay.SyncWait();
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            (bool hasError, string details, int statusByte) reading = queryStatusError.Invoke();
            if ( timeout <= TimeSpan.Zero || reading.hasError )
                return (false, reading.statusByte, TimeSpan.Zero);
            bool timedOut = sw.ElapsedTicks >= ticks;
            while ( !reading.hasError && (bitmask & reading.statusByte) != bitmask && !timedOut )
            {
                System.Threading.Thread.SpinWait( 1 );
                SessionBase.DoEventsAction?.Invoke();
                _ = pollInterval.SyncWait();
                reading = queryStatusError.Invoke();
                timedOut = sw.ElapsedTicks >= ticks;
            }

            return (timedOut, reading.statusByte, sw.Elapsed);
        } );
    }

    /// <summary>   Starts awaiting status. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="onsetDelay">       The time to wait before starting to query. </param>
    /// <param name="pollInterval">     Specifies time between serial polls. </param>
    /// <param name="queryStatusDone">  The query status done. </param>
    /// <returns>
    /// A Tuple: ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed time)
    /// </returns>
    public static Task<(bool TimedOut, int StatusByte, TimeSpan Elapsed)> StartAwaitingStatusTask( TimeSpan timeout,
                                                                                                   TimeSpan onsetDelay, TimeSpan pollInterval,
                                                                                                   Func<(bool done, int statusByte)> queryStatusDone )
    {
        return Task.Factory.StartNew<(bool, int, TimeSpan)>( () =>
        {
            _ = onsetDelay.SyncWait();
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            (bool done, int statusByte) reading = queryStatusDone.Invoke();
            if ( timeout <= TimeSpan.Zero || reading.done )
                return (false, reading.statusByte, TimeSpan.Zero);
            bool timedOut = sw.ElapsedTicks >= ticks;
            while ( !reading.done && !timedOut )
            {
                System.Threading.Thread.SpinWait( 1 );
                SessionBase.DoEventsAction?.Invoke();
                _ = pollInterval.SyncWait();
                reading = queryStatusDone.Invoke();
                timedOut = sw.ElapsedTicks >= ticks;
            }

            return (timedOut, reading.statusByte, sw.Elapsed);
        } );
    }

    /// <summary>   Await status. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="isStatusReady">    The is status ready. </param>
    /// <returns>
    /// A Tuple: ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed time)
    /// </returns>
    public virtual (bool TimedOut, int StatusByte, TimeSpan Elapsed) AwaitStatus( TimeSpan timeout, Func<int, bool> isStatusReady )
    {
        // emulate the reply for disconnected operations.
        this.MakeEmulatedReplyIfEmpty( 0 );
        CancellationTokenSource cts = new();
        Task<(bool TimedOut, int StatusByte, TimeSpan Elapsed)> t = this.StartAwaitingStatusTask( timeout, this.StatusReadDelay, this.StatusReadTurnaroundTime,
                                                                                                  isStatusReady );
        t.Wait();
        return t.Result;
    }

    /// <summary>   Starts awaiting status. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="timeout">          The timeout. </param>
    /// <param name="onsetDelay">       The time to wait before starting to query. </param>
    /// <param name="pollInterval">     Specifies time between serial polls. </param>
    /// <param name="isStatusReady">    The is status ready. </param>
    /// <returns>
    /// A Tuple: ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed time)
    /// Elapsed time includes the <paramref name="onsetDelay"/> and therefore might exceed the
    /// <paramref name="timeout"/>
    /// </returns>
    public Task<(bool TimedOut, int StatusByte, TimeSpan Elapsed)> StartAwaitingStatusTask( TimeSpan timeout,
                                                                                            TimeSpan onsetDelay, TimeSpan pollInterval,
                                                                                            Func<int, bool> isStatusReady )
    {
        return Task.Factory.StartNew<(bool, int, TimeSpan)>( () =>
        {
            TimeSpan elapsed = TimeSpan.Zero;
            _ = onsetDelay.SyncWait();
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            int statusByte = ( int ) this.ReadStatusByte();
            bool reading = isStatusReady.Invoke( statusByte );
            if ( timeout <= TimeSpan.Zero || reading )
            {
                elapsed = sw.Elapsed;
                elapsed = elapsed.Add( onsetDelay );
                return (false, statusByte, elapsed);
            }
            bool timedOut = sw.ElapsedTicks >= ticks;
            while ( !reading && !timedOut )
            {
                System.Threading.Thread.SpinWait( 1 );
                SessionBase.DoEventsAction?.Invoke();
                _ = pollInterval.SyncWait();
                statusByte = ( int ) this.ReadStatusByte();
                reading = isStatusReady.Invoke( statusByte );
                timedOut = sw.ElapsedTicks >= ticks;
            }
            elapsed = sw.Elapsed;
            elapsed = elapsed.Add( onsetDelay );
            return (timedOut, statusByte, sw.Elapsed.Add( onsetDelay ));
        } );
    }

    /// <summary>
    /// Starts a task waiting for a bitmask. The task complete after a timeout or if the action
    /// function statusByte matches the bitmask statusByte.
    /// </summary>
    /// <remarks>
    /// The task timeout is included in the task function. Otherwise, upon Wit(timeout), the task
    /// deadlocks attempting to get the task result. For more information see
    /// https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html. That document is short
    /// on examples for how to resolve this issue.
    /// </remarks>
    /// <param name="bitmask">      Specifies the anticipated bitmask. </param>
    /// <param name="timeout">      The timeout. </param>
    /// <param name="onsetDelay">   The time to wait before starting to query. </param>
    /// <param name="pollInterval"> Specifies time between serial polls. </param>
    /// <param name="action">       The action. </param>
    /// <returns>
    /// A Threading.Tasks.Task(Of ( bool, int, TimeSpan )) (true if timed out, status byte, elapsed
    /// time). Elapsed time includes the <paramref name="onsetDelay"/> and therefore might exceed the
    /// <paramref name="timeout"/>
    /// </returns>
    public static Task<(bool TimedOut, int StatusByte, TimeSpan Elapsed)> StartAwaitingBitmaskTask( int bitmask, TimeSpan timeout,
                                                                                                    TimeSpan onsetDelay, TimeSpan pollInterval,
                                                                                                    Func<int> action )
    {
        return Task.Factory.StartNew<(bool, int, TimeSpan)>( () =>
        {
            TimeSpan elapsed = TimeSpan.Zero;
            _ = onsetDelay.SyncWait();
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            int reading = action.Invoke();
            if ( timeout <= TimeSpan.Zero )
            {
                elapsed = sw.Elapsed;
                elapsed = elapsed.Add( onsetDelay );
                return (false, reading, elapsed);
            }
            bool timedOut = sw.ElapsedTicks >= ticks;
            while ( (bitmask & reading) != bitmask && !timedOut )
            {
                System.Threading.Thread.SpinWait( 1 );
                SessionBase.DoEventsAction?.Invoke();
                _ = pollInterval.SyncWait();
                reading = action.Invoke();
                timedOut = sw.ElapsedTicks >= ticks;
            }
            elapsed = sw.Elapsed;
            elapsed = elapsed.Add( onsetDelay );
            return (timedOut, reading, sw.Elapsed);
        } );
    }

    /// <summary>
    /// Starts a task waiting for a bitmask. The task complete after a timeout or if the action
    /// function statusByte matches the bitmask statusByte.
    /// </summary>
    /// <remarks>
    /// The task timeout is included in the task function. Otherwise, upon Wait(timeout), the task
    /// deadlocks attempting to get the task result. For more information see
    /// https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html. That document is short
    /// on examples for how to resolve this issue.
    /// </remarks>
    /// <param name="bitmask">      Specifies the anticipated bitmask. </param>
    /// <param name="timeout">      The timeout. </param>
    /// <param name="onsetDelay">   The time to wait before starting to query. </param>
    /// <param name="pollInterval"> Specifies time between serial polls. </param>
    /// <param name="action">       The action. </param>
    /// <returns>
    /// A Threading.Tasks.Task(Of ( bool, ServiceRequests, TimeSpan )) (true if timed out, status
    /// byte, elapsed time).
    /// </returns>
    public static Task<(bool TimedOut, ServiceRequests StatusByte, TimeSpan Elapsed)> StartAwaitingBitmaskTask( ServiceRequests bitmask,
                                                                                                                TimeSpan timeout, TimeSpan onsetDelay,
                                                                                                                TimeSpan pollInterval, Func<ServiceRequests> action )
    {
        return Task.Factory.StartNew<(bool, ServiceRequests, TimeSpan)>( () =>
        {
            _ = onsetDelay.SyncWait();
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            ServiceRequests reading = action.Invoke();
            if ( timeout <= TimeSpan.Zero )
                return (false, reading, TimeSpan.Zero);
            bool timedOut = sw.ElapsedTicks >= ticks;
            while ( (bitmask & reading) != bitmask && !timedOut )
            {
                System.Threading.Thread.SpinWait( 1 );
                SessionBase.DoEventsAction?.Invoke();
                _ = pollInterval.SyncWait();
                reading = action.Invoke();
                timedOut = sw.ElapsedTicks >= ticks;
            }

            return (timedOut, reading, sw.Elapsed);
        } );
    }

    #endregion
}
