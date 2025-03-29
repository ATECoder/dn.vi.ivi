using cc.isr.Std.TimeSpanExtensions;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " bit masking "

    /// <summary> Query if 'statusValue' is fully bit-masked. </summary>
    /// <param name="statusValue"> The statusByte value. </param>
    /// <param name="bitmask">     The bitmask. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public static bool IsFullyMasked( int statusValue, int bitmask )
    {
        return bitmask != 0 && (statusValue & bitmask) == bitmask;
    }

    /// <summary> Checks if the service request statusByte if fully masked. </summary>
    /// <param name="bitmask"> The bitmask. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool IsFullyMasked( ServiceRequests bitmask )
    {
        return IsFullyMasked( ( int ) this.ServiceRequestStatus, ( int ) bitmask );
    }

    /// <summary> Query if 'bitmask' is partially masked. </summary>
    /// <param name="statusValue"> The statusByte value. </param>
    /// <param name="bitmask">     The bitmask. </param>
    /// <returns> <c>true</c> if partially masked; otherwise <c>false</c> </returns>
    public static bool IsPartiallyMasked( int statusValue, int bitmask )
    {
        return bitmask != 0 && (statusValue & bitmask) != 0;
    }

    /// <summary> Query if 'bitmask' is partially masked. </summary>
    /// <param name="bitmask"> The bitmask. </param>
    /// <returns> <c>true</c> if partially masked; otherwise <c>false</c> </returns>
    public bool IsPartiallyMasked( ServiceRequests bitmask )
    {
        return IsPartiallyMasked( ( int ) this.ServiceRequestStatus, ( int ) bitmask );
    }

    /// <summary> Count set bits. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> The total number of set bits. </returns>
    public static int CountSetBits( int value )
    {
        int count = 0;
        while ( value > 0 )
        {
            count += value & 1;
            value >>= 1;
        }

        return count;
    }

    #endregion

    #region " service request register events: measurement "

    private ServiceRequests _measurementEventBitmask;

    /// <summary>
    /// Gets or sets the bit that would be set when an enabled measurement event has occurred.
    /// </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The Measurement event bit value. </value>
    public ServiceRequests MeasurementEventBitmask
    {
        get => this._measurementEventBitmask;
        set
        {
            if ( !value.Equals( this.MeasurementEventBitmask ) )
            {
                if ( CountSetBits( ( int ) value ) > 1 )
                {
                    throw new InvalidOperationException( $"Measurement Event Status bit cannot be set to a value={value} having more than one bit" );
                }
                this._measurementEventBitmask = value;
                base.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.MeasurementEventBitmask ) ) );
            }
        }
    }

    /// <summary> Gets a value indicating whether an enabled measurement event has occurred. </summary>
    /// <value>
    /// <c>true</c> if an enabled measurement event has occurred; otherwise, <c>false</c>.
    /// </value>
    public bool HasMeasurementEvent => this.IsMeasurementEventBitSet( this.ServiceRequestStatus );

    /// <summary> Gets a value indicating whether the measurement event bit is set. </summary>
    /// <value>
    /// <c>true</c> if an enabled measurement event has occurred; otherwise, <c>false</c>.
    /// </value>
    public bool IsMeasurementEventBitSet( ServiceRequests statusByte )
    {
        return 0 != (statusByte & this.MeasurementEventBitmask);
    }

    #endregion

    #region " service request register events: system event "

    /// <summary> The system event bit. </summary>
    private ServiceRequests _systemEventBitmask;

    /// <summary>
    /// Gets or sets the bit that would be set for detecting if a System Event has occurred.
    /// </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The System Event bit value. </value>
    public ServiceRequests SystemEventBitmask
    {
        get => this._systemEventBitmask;
        set
        {
            if ( !value.Equals( this.SystemEventBitmask ) )
            {
                if ( CountSetBits( ( int ) value ) > 1 )
                {
                    throw new InvalidOperationException( $"System Event Status bit cannot be set to a value={value} having more than one bit" );
                }

                this._systemEventBitmask = value;
                base.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.SystemEventBitmask ) ) );
            }
        }
    }

    /// <summary> Gets a value indicating whether a System Event has occurred. </summary>
    /// <value> <c>true</c> if System Event bit is on; otherwise, <c>false</c>. </value>
    public bool HasSystemEvent => this.IsSystemEventBitSet( this.ServiceRequestStatus );

    /// <summary> Gets a value indicating whether the System event bit is set. </summary>
    /// <value>
    /// <c>true</c> if an enabled System event has occurred; otherwise, <c>false</c>.
    /// </value>
    public bool IsSystemEventBitSet( ServiceRequests statusByte )
    {
        return 0 != (statusByte & this.SystemEventBitmask);
    }

    #endregion

    #region " service request register events: error "

    private ServiceRequests _errorAvailableBitmask;

    /// <summary> Gets or sets the bit that would be set if an error event has occurred. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The error event bit. </value>
    public ServiceRequests ErrorAvailableBitmask
    {
        get => this._errorAvailableBitmask;
        set => _ = SessionBase.CountSetBits( ( int ) value ) > 1
                ? throw new InvalidOperationException( $"Error Available Status Bit cannot be set to {value} because it has {SessionBase.CountSetBits( ( int ) value )} bits" )
                : this.SetProperty( ref this._errorAvailableBitmask, value );
    }

    /// <summary>
    /// Gets a value indicating whether the error available bit is set in the last read statusByte byte.
    /// This does not affect the cached <see cref="ServiceRequestStatus"/> property.
    /// </summary>
    /// <value> <c>true</c> if [Error available]; otherwise, <c>false</c>. </value>
    public bool ErrorAvailable => this.IsErrorBitSet( this.ServiceRequestStatus );

    /// <summary>
    /// Reads the status byte and returns true if the error bit is set.
    /// </summary>
    /// <remarks>   David, 2021-04-01. </remarks>
    /// <param name="statusByte">   The statusByte byte. </param>
    /// <returns>   <c>true</c> if it error bit is on; otherwise <c>false</c> </returns>
    public bool IsErrorBitSet( ServiceRequests statusByte )
    {
        return (this.ErrorAvailableBitmask & statusByte) != 0;
    }

    /// <summary>
    /// Reads the status byte and returns true if the error bit is set.
    /// </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="statusByte">   The statusByte byte. </param>
    /// <returns>   <c>true</c> if it error bit is on; otherwise <c>false</c> </returns>
    public bool IsErrorBitSet( int statusByte )
    {
        return (( int ) this.ErrorAvailableBitmask & statusByte) != 0;
    }

    /// <summary>
    /// Reads the status byte and returns true if the error bit is set; Does not affect the
    /// cached <see cref="ErrorAvailable"/> property.
    /// </summary>
    /// <returns> <c>true</c> if it error bit is on; otherwise <c>false</c> </returns>
    [Obsolete( "Use <session>.IsErrorBitSet( <session>.ReadStatusByte() ) " )]
    public bool IsErrorBitSet()
    {
        // read statusByte byte without affecting the error available property.
        return this.IsErrorBitSet( this.ReadStatusByte() );
    }

    /// <summary>   Is error available. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <returns>   A Tuple: ( true if error, details, status byte) </returns>
    public virtual (bool HasError, string Details, int StatusByte) IsStatusError( int statusByte )
    {
        return this.IsErrorBitSet( statusByte ) ? (true, "Error", statusByte) : (false, string.Empty, statusByte);
    }

    /// <summary>   Is status error or warning. </summary>
    /// <remarks>   David, 2021-04-17. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <returns>   A Tuple. </returns>
    public virtual (bool HasError, string Details, int StatusByte) IsStatusErrorOrWarning( int statusByte )
    {
        return this.IsErrorBitSet( statusByte ) ? (true, "Error", statusByte) : (false, string.Empty, statusByte);
    }

    /// <summary>   Queries status error. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <returns>   A Tuple: ( true if error, details, status byte) </returns>
    public (bool hasError, string Details, int StatusByte) QueryStatusError()
    {
        return this.IsStatusError( ( int ) this.ReadStatusByte() );
    }

    #endregion

    #region " service request register events: questionable "

    /// <summary> The questionable event bit. </summary>
    private ServiceRequests _questionableEventBitmask;

    /// <summary>
    /// Gets or sets the bit that would be set if a Questionable event has occurred.
    /// </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The Questionable event bit. </value>
    public ServiceRequests QuestionableEventBitmask
    {
        get => this._questionableEventBitmask;
        set
        {
            if ( !value.Equals( this.QuestionableEventBitmask ) )
            {
                if ( CountSetBits( ( int ) value ) > 1 )
                {
                    throw new InvalidOperationException( $"Questionable Event Status bit cannot be set to a value={value} having more than one bit" );
                }

                this._questionableEventBitmask = value;
                base.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.QuestionableEventBitmask ) ) );
            }
        }
    }

    /// <summary> Gets a value indicating whether a Questionable event has occurred. </summary>
    /// <value> <c>true</c> if Questionable event has occurred; otherwise, <c>false</c>. </value>
    public bool HasQuestionableEvent => this.IsQuestionableEventBitSet( this.ServiceRequestStatus );

    /// <summary> Gets a value indicating whether the Questionable event bit is set. </summary>
    /// <value>
    /// <c>true</c> if an enabled Questionable event has occurred; otherwise, <c>false</c>.
    /// </value>
    public bool IsQuestionableEventBitSet( ServiceRequests statusByte )
    {
        return 0 != (statusByte & this.QuestionableEventBitmask);
    }

    #endregion

    #region " service request register events: message "

    /// <summary> The message available bit. </summary>
    private ServiceRequests _messageAvailableBitmask;

    /// <summary> Gets or sets the bit that would be set if a message is available. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The Message available bit. </value>
    public ServiceRequests MessageAvailableBitmask
    {
        get => this._messageAvailableBitmask;
        set
        {
            if ( !value.Equals( this.MessageAvailableBitmask ) )
            {
                if ( CountSetBits( ( int ) value ) > 1 )
                {
                    throw new InvalidOperationException( $"Message Available Status Bit cannot be set to a value={value} having more than one bit" );
                }

                this._messageAvailableBitmask = value;
                base.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.MessageAvailableBitmask ) ) );
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the message available bit <see cref="MessageAvailableBitmask"/>
    /// is set in the last read statusByte byte. This does not affect the cached <see cref="ServiceRequestStatus"/>
    /// property.
    /// </summary>
    /// <value> <c>true</c> if a message is available; otherwise, <c>false</c>. </value>
    public bool MessageAvailable => this.IsMessageAvailableBitSet( this.ServiceRequestStatus );

    /// <summary>
    /// Queries if the message bit <see cref="MessageAvailableBitmask"/> is set in the <paramref name="statusByte"/>.
    /// </summary>
    /// <remarks>   David, 2021-04-01. </remarks>
    /// <param name="statusByte">   The statusByte byte. </param>
    /// <returns>   True if the message is available, false if not. </returns>
    public bool IsMessageAvailableBitSet( ServiceRequests statusByte )
    {
        return this.MessageAvailableBitmask == (statusByte & this.MessageAvailableBitmask);
    }

    /// <summary>
    /// Queries if the message bit <see cref="MessageAvailableBitmask"/> is set in the <paramref name="statusByte"/>.
    /// </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <param name="statusByte">   The statusByte byte. </param>
    /// <returns>   True if the message is available, false if not. </returns>
    public bool IsMessageAvailableBitSet( int statusByte )
    {
        return ( int ) this.MessageAvailableBitmask == (statusByte & ( int ) this.MessageAvailableBitmask);
    }

    /// <summary>
    /// Wait until the status byte message available <see cref="MessageAvailableBitmask"/>
    /// or the <see cref="ErrorAvailableBitmask"/> bits are set or count out.
    /// </summary>
    /// <remarks>
    /// This was modified to read the status byte without invoking the statusByte byte event
    /// handlers. <para>
    /// Delays looking for the message statusByte by the statusByte latency to make sure the
    /// instrument had enough time to process the previous command.</para>
    /// </remarks>
    /// <param name="latency">  The latency. </param>
    /// <param name="countOut"> The number of time to check for the bit before counting out. </param>
    /// <returns>   <see cref="ServiceRequests"/> value that must be checked for the proper status.. </returns>
    public ServiceRequests AwaitErrorOrMessageAvailableBits( TimeSpan latency, int countOut )
    {
        ServiceRequests statusByte;
        bool done;
        do
        {
            if ( latency > TimeSpan.Zero )
                _ = latency.AsyncWait();
            statusByte = this.ReadStatusByte();
            countOut -= 1;
            done = this.IsMessageAvailableBitSet( statusByte ) || this.IsErrorBitSet( statusByte );
        }
        while ( countOut > 0 && !done );
        return statusByte;
    }

    /// <summary>
    /// Reads the status byte and returns true if the message available <see cref="MessageAvailableBitmask"/>
    /// bit is set.
    /// </summary>
    /// <remarks>
    /// This was modified to read the status byte without invoking the statusByte byte event handlers.
    /// </remarks>
    /// <returns>   <c>true</c> if the message available bit is on; otherwise, <c>false</c>. </returns>
    public bool QueryMessageAvailableBit()
    {
        ServiceRequests statusByte = this.ReadStatusByte();
        return this.IsMessageAvailableBitSet( statusByte );
    }

    /// <summary>
    /// Awaits for the status byte to have the <see cref="MessageAvailableBitmask"/>
    /// and or <see cref="ErrorAvailableBitmask"/> bits to set or to timeout out.
    /// </summary>
    /// <remarks>
    /// This was modified to read the status byte without invoking the statusByte byte event handlers.
    /// </remarks>
    /// <param name="timeout">  Specifies the time to wait for bit masks. </param>
    /// <returns>   <c>true</c> if the message available bit is on; otherwise, <c>false</c>. </returns>
    public (bool TimedOut, ServiceRequests StatusByte, TimeSpan Elapsed) AwaitErrorOrMessageAvailableBits( TimeSpan timeout )
    {
        return this.AwaitStatusBitmask( this.ErrorAvailableBitmask | this.MessageAvailableBitmask, timeout );
    }

    #endregion

    #region " service request register events: standard event "

    private ServiceRequests _standardEventSummaryBitmask;

    /// <summary>
    /// Gets or sets the bitmask that would be set if an enabled standard event has occurred.
    /// </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The Standard Event bit. </value>
    public ServiceRequests StandardEventSummaryBitmask
    {
        get => this._standardEventSummaryBitmask;
        set
        {
            if ( !value.Equals( this.StandardEventSummaryBitmask ) )
            {
                if ( CountSetBits( ( int ) value ) > 1 )
                {
                    throw new InvalidOperationException( $"Standard Event Status bit cannot be set to a value={value} having more than one bit" );
                }

                this._standardEventSummaryBitmask = value;
                base.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.StandardEventSummaryBitmask ) ) );
            }
        }
    }

    /// <summary> Gets a value indicating whether an enabled standard event has occurred. </summary>
    /// <value>
    /// <c>true</c> if an enabled standard event has occurred; otherwise, <c>false</c>.
    /// </value>
    public bool HasStandardEvent => this.IsStandardEventSummaryBitSet( this.ServiceRequestStatus );

    /// <summary> Gets a value indicating whether the Standard event summary bit is set. </summary>
    /// <value>
    /// <c>true</c> if an enabled Standard event has occurred; otherwise, <c>false</c>.
    /// </value>
    public bool IsStandardEventSummaryBitSet( ServiceRequests statusByte )
    {
        return 0 != (statusByte & this.StandardEventSummaryBitmask);
    }

    #endregion

    #region " service request register events: service event (srq) "

    private ServiceRequests _requestingServiceBitmask = ServiceRequests.RequestingService;

    /// <summary>
    /// Gets or sets bit that would be set if a requested service or Main Summary Bit (MSB) event has
    /// occurred.
    /// </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The requested service or Main Summary Status Byte Bit. </value>
    public ServiceRequests RequestingServiceBitmask
    {
        get => this._requestingServiceBitmask;
        set
        {
            if ( !value.Equals( this.RequestingServiceBitmask ) )
            {
                if ( CountSetBits( ( int ) value ) > 1 )
                {
                    throw new InvalidOperationException( $"Requesting Service Status Bit cannot be set to a value={value} having more than one bit" );
                }

                this._requestingServiceBitmask = value;
                base.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.RequestingServiceBitmask ) ) );
            }
        }
    }

    /// <summary> Gets a value indicating if the device has requested service. </summary>
    /// <value> <c>true</c> if the device has requested service; otherwise, <c>false</c>. </value>
    public bool RequestedService => this.IsRequestedServiceBitSet( this.ServiceRequestStatus );

    /// <summary> Gets a value indicating whether the Requested Service event bit is set. </summary>
    /// <value>
    /// <c>true</c> if an enabled Requested Service event has occurred; otherwise, <c>false</c>.
    /// </value>
    public bool IsRequestedServiceBitSet( ServiceRequests statusByte )
    {
        return 0 != (statusByte & this.RequestingServiceBitmask);
    }

    #endregion

    #region " service request register events: operation "

    private ServiceRequests _operationEventSummaryBitmask;

    /// <summary> Gets or sets the bit that would be set if an operation event has occurred. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <value> The Operation event bit. </value>
    public ServiceRequests OperationEventSummaryBitmask
    {
        get => this._operationEventSummaryBitmask;
        set
        {
            if ( !value.Equals( this.OperationEventSummaryBitmask ) )
            {
                if ( CountSetBits( ( int ) value ) > 1 )
                {
                    throw new InvalidOperationException( $"Operation Event Status bit cannot be set to a value={value} having more than one bit" );
                }

                this._operationEventSummaryBitmask = value;
                base.OnPropertyChanged( new System.ComponentModel.PropertyChangedEventArgs( nameof( this.OperationEventSummaryBitmask ) ) );
            }
        }
    }

    /// <summary> Gets a value indicating whether an operation event has occurred. </summary>
    /// <value> <c>true</c> if an operation event has occurred; otherwise, <c>false</c>. </value>
    public bool HasOperationEvent => this.IsOperationEventBitSet( this.ServiceRequestStatus );

    /// <summary> Gets a value indicating whether the Operation event bit is set. </summary>
    /// <value>
    /// <c>true</c> if an enabled Operation event has occurred; otherwise, <c>false</c>.
    /// </value>
    public bool IsOperationEventBitSet( ServiceRequests statusByte )
    {
        return 0 != (statusByte & this.OperationEventSummaryBitmask);
    }

    #endregion

    #region " events: device error occurred "

    /// <summary> Notifies of the DeviceErrorOccurred event. </summary>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected virtual void OnDeviceErrorOccurred( ServiceRequestEventArgs e )
    {
        this.NotifyDeviceErrorOccurred( e );
    }

    /// <summary> Removes the Service Requested event handlers. </summary>
    protected void RemoveDeviceErrorOccurredEventHandlers()
    {
        EventHandler<ServiceRequestEventArgs>? handler = this.DeviceErrorOccurred;
        if ( handler is not null )
        {
            foreach ( Delegate? item in handler.GetInvocationList() )
            {
                handler -= ( EventHandler<ServiceRequestEventArgs> ) item;
            }
        }
    }

    /// <summary> Event queue for all listeners interested in DeviceErrorOccurred events. </summary>
    /// <remarks> A custom Event is used here to allow us to synchronize with the event listeners.
    /// Using a custom Raise method lets you iterate through the delegate list.
    /// </remarks>
    public event EventHandler<ServiceRequestEventArgs>? DeviceErrorOccurred;

    /// <summary>
    /// synchronously invokes the <see cref="DeviceErrorOccurred">DeviceErrorOccurred Event</see>.
    /// Not thread safe.
    /// </summary>
    /// <remarks>   David, 2021-08-16. </remarks>
    /// <param name="e">    The <see cref="ServiceRequestEventArgs" /> instance containing the event data. </param>
    protected void NotifyDeviceErrorOccurred( ServiceRequestEventArgs e )
    {
        this.DeviceErrorOccurred?.Invoke( this, e );
    }

    #endregion

    #region " event handler error "

    /// <summary>   Event queue for all listeners interested in EventHandlerError events. </summary>
    public event EventHandler<System.Threading.ThreadExceptionEventArgs>? EventHandlerError;

    /// <summary>   Raises the cc.isr. core. error event. </summary>
    /// <remarks>   David, 2021-05-03. </remarks>
    /// <param name="e">    Event information to send to registered event handlers. </param>
    protected virtual void OnEventHandlerError( System.Threading.ThreadExceptionEventArgs e )
    {
        this.EventHandlerError?.Invoke( this, e );
    }

    /// <summary>   Raises the cc.isr. core. error event. </summary>
    /// <remarks>   David, 2021-05-03. </remarks>
    /// <param name="ex">   The exception. </param>
    protected virtual void OnEventHandlerError( Exception ex )
    {
        this.OnEventHandlerError( new System.Threading.ThreadExceptionEventArgs( ex ) );
    }

    #endregion

    #region " query data and query results bitmasks "

    private int _statusBusyBitmask;

    /// <summary>   Gets or sets the status busy bitmask. </summary>
    /// <value> The status busy bitmask. </value>
    public int StatusBusyBitmask
    {
        get => this._statusBusyBitmask;
        set => _ = this.SetProperty( ref this._statusBusyBitmask, value );
    }

    /// <summary>
    /// Query if the <paramref name="statusByte"/> is busy; that is, any status bit match the <see cref="StatusBusyBitmask"/>.
    /// </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <returns>   True if status busy, false if not. </returns>
    public virtual bool IsStatusBusy( int statusByte )
    {
        return 0 != (statusByte & this.StatusBusyBitmask);
    }

    /// <summary>   Queries status not busy. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <returns>   The status not busy. </returns>
    public virtual (bool done, int StatusByte) QueryStatusNotBusy()
    {
        int statusByte = ( int ) this.ReadStatusByte();
        return (this.IsStatusBusy( statusByte ), statusByte);
    }

    private int _statusDataReadyBitmask;

    /// <summary>   Gets or sets the status data ready bitmask. </summary>
    /// <value> The status data ready bitmask. </value>
    public int StatusDataReadyBitmask
    {
        get => this._statusDataReadyBitmask;
        set => _ = this.SetProperty( ref this._statusDataReadyBitmask, value );
    }

    /// <summary>   Query if the <paramref name="statusByte"/> status indicates that the instrument data is ready
    ///             by checking if the <see cref="StatusDataReadyBitmask"/> is set. </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <returns>   True if the instrument data is ready, false if not. </returns>
    public virtual bool IsStatusDataReady( int statusByte )
    {
        return this.StatusDataReadyBitmask == (statusByte & this.StatusDataReadyBitmask);
    }

    private int _statusQueryResultReadyBitmask;

    /// <summary>   Gets or sets the status query result ready. </summary>
    /// <value> The status query result ready. </value>
    public int StatusQueryResultReadyBitmask
    {
        get => this._statusQueryResultReadyBitmask;
        set => _ = this.SetProperty( ref this._statusQueryResultReadyBitmask, value );
    }

    /// <summary>   Query if the <paramref name="statusByte"/> status indicates that the query as a ready
    ///             by checking if the <see cref="StatusQueryResultReadyBitmask"/> is set. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <returns>   True if the instrument has a query result, false if not. </returns>
    public virtual bool IsStatusQueryResultReady( int statusByte )
    {
        return this.StatusQueryResultReadyBitmask == (statusByte & this.StatusQueryResultReadyBitmask);
    }

    #endregion

    #region " query data and query results bitmasks - extended "

    /// <summary>   Executes the command after waiting for status ready. </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <param name="readyToQueryTimeout">  The ready to query timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <returns>   <see cref="ExecuteInfo"/> </returns>
    public ExecuteInfo ExecuteStatusReady( TimeSpan readyToQueryTimeout, string dataToWrite )
    {
        return string.IsNullOrWhiteSpace( dataToWrite )
            ? ExecuteInfo.Empty
            : this.ExecuteStatusReady( readyToQueryTimeout, dataToWrite,
                ( statusByte ) => !this.IsStatusBusy( statusByte ), this.IsStatusError );
    }

    /// <summary>   Reads if status data ready. </summary>
    /// <remarks>   David, 2021-04-16. </remarks>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <returns>   if status data ready. </returns>
    public ReadInfo ReadIfStatusDataReady( TimeSpan readyToReadTimeout )
    {
        return this.ReadIfStatusReady( readyToReadTimeout, this.IsStatusDataReady, this.IsStatusError );
    }

    #endregion
}
