using System.Diagnostics;

namespace cc.isr.VI;

public partial class VisaSessionBase
{
    private readonly Std.Concurrent.ConcurrentToken<string> _serviceRequestFailureMessageToken = new();

    /// <summary> Gets or sets a message describing the service request failure. </summary>
    /// <value> A message describing the service request failure. </value>
    public string ServiceRequestFailureMessage
    {
        get => this._serviceRequestFailureMessageToken.Value!;
        set
        {
            value ??= string.Empty;
            if ( !string.Equals( value, this.ServiceRequestFailureMessage, StringComparison.Ordinal ) )
            {
                this._serviceRequestFailureMessageToken.Value = value;
                if ( !string.IsNullOrWhiteSpace( this.ServiceRequestFailureMessage ) )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogInformation( this.ServiceRequestFailureMessage );
                }
            }
            // send every time
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Processes the service request. </summary>
    protected abstract void ProcessServiceRequest();

    /// <summary> Reads the event registers after receiving a service request. </summary>
    /// <returns> True if it succeeds; otherwise, false. </returns>
    protected bool TryProcessServiceRequest()
    {
        bool result = true;
        try
        {
            this.ProcessServiceRequest();
        }
        catch ( Exception ex )
        {
            ex.Data.Add( $"{ex.Data.Count}-{nameof( VisaSessionBase.ServiceRequestStatus )}", $"{this.Session?.BuildServiceRequestValueDescription()}" );
            ex.Data.Add( $"{ex.Data.Count}-{nameof( Pith.SessionBase.LastAction )}", this.Session?.LastAction );
            ex.Data.Add( $"{ex.Data.Count}-{nameof( Pith.SessionBase.LastMessageSent )}", this.Session?.LastMessageSent );
            ex.Data.Add( $"{ex.Data.Count}-{nameof( Pith.SessionBase.LastMessageReceived )}", this.Session?.LastMessageReceived );
            this.ServiceRequestFailureMessage = cc.isr.VI.SessionLogger.Instance.LogException( ex, "processing service request" );
            result = false;
        }

        return result;
    }

    /// <summary> The service request Status concurrent token. </summary>
    private readonly Std.Concurrent.ConcurrentToken<int> _serviceRequestStatusToken = new();

    /// <summary> Gets or sets the Service Request Status. </summary>
    /// <value> The Service Request Status. </value>
    public int ServiceRequestStatus
    {
        get => this._serviceRequestStatusToken.Value;
        set
        {
            if ( !int.Equals( this.ServiceRequestStatus, value ) )
            {
                this._serviceRequestStatusToken.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The service request reading concurrent token. </summary>
    private readonly Std.Concurrent.ConcurrentToken<string> _serviceRequestReadingToken = new();

    /// <summary> Gets or sets the Service Request reading. </summary>
    /// <value> The Service Request reading. </value>
    public string? ServiceRequestReading
    {
        get => this._serviceRequestReadingToken.Value;
        set
        {
            if ( !string.Equals( this.ServiceRequestReading, value, StringComparison.Ordinal ) )
            {
                this._serviceRequestReadingToken.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   (Immutable) the lazy service request automatic read token. </summary>
    private readonly Lazy<Std.Concurrent.ConcurrentToken<bool>> _lazyServiceRequestAutoReadToken = new( () => new Std.Concurrent.ConcurrentToken<bool>() );

    /// <summary> Gets or sets the automatic read Service Requesting option . </summary>
    /// <value> The automatic read service request option. </value>
    public bool ServiceRequestAutoRead
    {
        get => this._lazyServiceRequestAutoReadToken.Value.Value;
        set
        {
            if ( this.ServiceRequestAutoRead != value )
            {
                this._lazyServiceRequestAutoReadToken.Value.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

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
    /// Synchronously invokes the <see cref="ServiceRequested">ServiceRequested Event</see>.
    /// </summary>
    /// <param name="e"> The <see cref="EventArgs" /> instance containing the event data. </param>
    protected void NotifyServiceRequested( EventArgs e )
    {
        this.ServiceRequested?.Invoke( this, e );
    }

    #endregion

    /// <summary> Session base service requested. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void SessionBaseServiceRequested( object? sender, EventArgs e )
    {
        if ( this.IsDisposed || sender is null || e is null )
            return;
        string activity = $"handling {nameof( Pith.SessionBase )} service request";
        try
        {
            _ = this.TryProcessServiceRequest();
            this.NotifyServiceRequested( e );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Starts awaiting service request reading task. </summary>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A Threading.Tasks.Task. </returns>
    public async System.Threading.Tasks.Task StartAwaitingServiceRequestReadingTask( TimeSpan timeout )
    {
        await System.Threading.Tasks.Task.Factory.StartNew( () =>
        {
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            while ( this.ServiceRequestEventEnabled && string.IsNullOrWhiteSpace( this.ServiceRequestReading )
                        && string.IsNullOrWhiteSpace( this.ServiceRequestFailureMessage )
                        && sw.ElapsedTicks < ticks )
            {
                System.Threading.Thread.SpinWait( 1 );
                Pith.SessionBase.DoEventsAction?.Invoke();
            }
        } ).ConfigureAwait( false );
    }
}
