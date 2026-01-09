using System.Diagnostics;
using Ivi.Visa;

namespace cc.isr.VI.Foundation;

/// <summary> A Visa session implementing the IVI Foundation message based session. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-20 </para>
/// </remarks>
[CLSCompliant( false )]
public partial class Session : Pith.SessionBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the MessageBasedSession object from the specified resource name.
    /// </summary>
    public Session() : base() =>
        // flags service request as not enabled.
        this._enabledEventType = EventType.Custom;

    #region " disposable support"

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    protected override void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;
        try
        {
            if ( disposing )
            {
                try
                {
                    this.DisposeSession();
                }
                catch ( Exception ex )
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed discarding enabled events.", $"Failed discarding enabled events. {ex}" );
                }
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #endregion

    #region " session "

    /// <summary> Gets or sets the sentinel indicating whether this is a dummy session. </summary>
    /// <value> The dummy sentinel. </value>
    public override bool IsDummy { get; } = false;

    /// <summary> The visa session. </summary>
    /// <remarks>
    /// Must be defined without events; Otherwise, setting the timeout causes a memory exception.
    /// </remarks>
    /// <value> The visa session. </value>
    [CLSCompliant( false )]
    public IMessageBasedSession? VisaSession { get; set; }

    /// <summary> Gets the type of the hardware interface. </summary>
    /// <value> The type of the hardware interface. </value>
    [CLSCompliant( false )]
    public HardwareInterfaceType HardwareInterfaceType => this.VisaSession is null ? HardwareInterfaceType.Custom : this.VisaSession.HardwareInterfaceType;

    /// <summary>
    /// Gets the session open sentinel. When open, the session is capable of addressing the hardware.
    /// See also <see cref="P:VI.Pith.SessionBase.IsDeviceOpen" />.
    /// </summary>
    /// <value> The is session open. </value>
    public override bool IsSessionOpen => this.ResourceOpenState == Pith.ResourceOpenState.Success && this.VisaSession is object;

    /// <summary> Executes the session open action. </summary>
    /// <param name="resourceName">  Name of the resource. </param>
    /// <param name="resourceModel"> The resource model. </param>
    protected override void OnSessionOpen( string resourceName, string resourceModel )
    {
        // 6824: disable events if enabled. Apparently, the events are enabled,
        // which causes issues trying to read STB to detect message available as VISA might be
        // reading the STB byte already.
        this.VisaSession?.DisableEvent( EventType.ServiceRequest );
        // reads session defaults.
        base.OnSessionOpen( resourceName, resourceModel );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Ivi.Visa.IMessageBasedSession" /> class.
    /// </summary>
    /// <remarks>
    /// This method does not lock the resource. Rev 4.1 and 5.0 of VISA did not support this call and
    /// could not verify the resource.
    /// </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <exception cref="NativeVisaException">      Thrown when a Native Visa error condition occurs. </exception>
    /// <exception cref="cc.isr.VI.Pith.NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="timeout">      The timeout for opening a session. </param>
    protected override void CreateSession( string resourceName, TimeSpan timeout )
    {
        try
        {
            this.ClearLastError();
            ResourceOpenStatus result = ResourceOpenStatus.Success;
            if ( this.Enabled )
            {
                string activity = "creating visa session";
                result = ResourceOpenStatus.Unknown;
                this.VisaSession = ( IMessageBasedSession ) GlobalResourceManager.Open( resourceName, AccessModes.None, ( int ) timeout.TotalMilliseconds, out result );
                if ( result != ResourceOpenStatus.Success )
                    throw new InvalidOperationException( $"Failed {activity}; '{result}';. {resourceName}" );
                if ( this.VisaSession is null )
                    throw new InvalidOperationException( $"Failed {activity};. {resourceName}" );
                int currentTimeout = this.VisaSession.TimeoutMilliseconds;
                // This was added because Keysight VISA accepts a host device even if the device (e.g., at gpib0,7) is inaccessible.
                try
                {
                    // setting short timeout does not seem to return control faster here. Interesting.
                    this.VisaSession.TimeoutMilliseconds = 50;
                    _ = this.VisaSession.ReadStatusByte();
                }
                catch
                {
                    throw new NativeVisaException( NativeErrorCode.ResourceNotFound );
                }
                finally
                {
                    this.VisaSession.TimeoutMilliseconds = currentTimeout;
                }
            }
        }
        catch ( NativeVisaException ex )
        {
            this.DisposeSession();
            this.LastNativeError = new NativeError( ex.ErrorCode, resourceName, "@opening", "opening session" );
            Pith.NativeException exception = new( this.LastNativeError, ex );
            exception.AddExceptionData( ex );
            throw exception;
        }
        catch
        {
            this.DisposeSession();
            throw;
        }
    }

    /// <summary> Gets the sentinel indication that the VISA session is disposed. </summary>
    /// <value> The is session disposed. </value>
    public override bool IsSessionDisposed => this.VisaSession is null;

    /// <summary>
    /// Disposes the VISA <see cref="isr.VI.Pith.SessionBase">Session</see> ending access to the
    /// instrument.
    /// </summary>
    protected override void DisposeSession()
    {
        if ( this.VisaSession is not null )
        {
            try
            {
                this.VisaSession?.Dispose();
            }
            catch
            {
                throw;
            }
            finally
            {
                this.ResourceOpenState = Pith.ResourceOpenState.Unknown;
                this.VisaSession = null;
            }
        }
    }

    /// <summary> Discards the session events. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    protected override void DiscardAllEvents()
    {
        if ( this.IsSessionOpen )
        {
            try
            {
                this.VisaSession?.DiscardEvents( EventType.AllEnabled );
            }
            catch ( NativeVisaException ex )
            {
                this.LastNativeError = new NativeError( ex.ErrorCode, this.ResourceNameCaption, "@discarding", "discarding  all events" );
                throw new Pith.NativeException( this.LastNativeError, ex );
            }
        }
    }

    /// <summary>
    /// Checks if the candidate resource name exists. If so, assign to the
    /// <see cref="cc.isr.VI.Pith.SessionBase.ValidatedResourceName">validated resource name</see>
    /// </summary>
    /// <returns> <c>true</c> if it the resource exists; otherwise <c>false</c> </returns>
    public override bool ValidateCandidateResourceName()
    {
        using ResourcesProvider rm = new();
        return this.ValidateCandidateResourceName( rm );
    }

    /// <summary>
    /// Gets or sets the sentinel indicating if call backs are performed in a specific
    /// synchronization context.
    /// </summary>
    /// <remarks>
    /// For .NET Framework 2.0, use SynchronizeCallbacks to specify that the object marshals
    /// callbacks across threads appropriately.<para>
    /// DH: 3339 Setting true prevents display.
    /// </para><para>
    /// Note that setting to false also breaks display updates.
    /// </para>
    /// </remarks>
    /// <value>
    /// The sentinel indicating if call backs are performed in a specific synchronization context.
    /// </value>
    public override bool SynchronizeCallbacks
    {
        get
        {
            if ( this.IsSessionOpen )
            {
                base.SynchronizeCallbacks = this.VisaSession!.SynchronizeCallbacks;
            }

            return base.SynchronizeCallbacks;
        }

        set
        {
            base.SynchronizeCallbacks = value;
            if ( this.IsSessionOpen )
            {
                this.VisaSession!.SynchronizeCallbacks = value;
            }
        }
    }

    #endregion

    #region " read/write "

    /// <summary> Gets or sets the ASCII character used to end reading. </summary>
    /// <value> The termination character. </value>
    public override byte ReadTerminationCharacter
    {
        get => base.ReadTerminationCharacter;
        set
        {
            _ = this.VisaSession?.TerminationCharacter = value;
            base.ReadTerminationCharacter = value;
        }
    }

    /// <summary>
    /// Gets or sets the termination character enabled specifying whether the read operation ends
    /// when a termination character is received.
    /// </summary>
    /// <value> The termination character enabled. </value>
    public override bool ReadTerminationCharacterEnabled
    {
        get => base.ReadTerminationCharacterEnabled;
        set
        {
            _ = this.VisaSession?.TerminationCharacterEnabled = value;
            base.ReadTerminationCharacterEnabled = value;
        }
    }

    /// <summary> Gets or sets the timeout for I/O communication on this resource session. </summary>
    /// <value> The communication timeout. </value>
    public override TimeSpan CommunicationTimeout
    {
        get => base.CommunicationTimeout;
        set
        {
            _ = this.VisaSession?.TimeoutMilliseconds = ( int ) value.TotalMilliseconds;
            base.CommunicationTimeout = value;
        }
    }

    /// <summary> Query if 'readStatus' is read ended. </summary>
    /// <param name="readStatus"> The read status. </param>
    /// <returns> <c>true</c> if read ended; otherwise <c>false</c> </returns>
    private static bool IsReadEnded( ReadStatus readStatus )
    {
        return readStatus is ReadStatus.EndReceived or ReadStatus.TerminationCharacterEncountered;
    }

    /// <summary>
    /// Synchronously reads ASCII-encoded string data until an END indicator or
    /// <see cref="ReadTerminationCharacter">termination character</see>
    /// termination character is reached irrespective of the buffer size.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <exception cref="cc.isr.VI.Pith.NativeException">  Thrown when a Native error condition
    ///                                                 occurs. </exception>
    /// <returns>   The received message. </returns>
    ///
    public override string ReadFreeLine()
    {
        System.Text.StringBuilder builder = new();
        try
        {
            this.ClearLastError();
            ReadStatus endReadStatus = ReadStatus.Unknown;
            int bufferSize = this.ReadBufferSizeAttribute();
            if ( this.IsSessionOpen )
            {
                bool hitEndRead = false;
                do
                {
                    string msg = this.VisaSession!.RawIO.ReadString( bufferSize, out endReadStatus );
                    hitEndRead = IsReadEnded( endReadStatus );
                    _ = builder.Append( msg );
                    Pith.SessionBase.DoEventsAction?.Invoke();
                }
                while ( !hitEndRead );
                this.LastMessageReceived = builder.ToString();
            }
            else
            {
                this.LastMessageReceived = this.EmulatedReply;
            }
        }
        catch ( NativeVisaException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue
                ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, this.LastMessageSent, this.LastAction )
                : new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastMessageSent, this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
        catch ( IOTimeoutException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue
                ? new NativeError( NativeErrorCode.Timeout, this.OpenResourceName, this.LastNodeNumber.Value, this.LastMessageSent, this.LastAction )
                : new NativeError( NativeErrorCode.Timeout, this.OpenResourceName, this.LastMessageSent, this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
        finally
        {
            // must clear the reply after each reading otherwise could get cross information.
            this.EmulatedReply = string.Empty;
            this.RestartKeepAliveTimer();
        }
        return builder.ToString();
    }

    /// <summary>
    /// Synchronously reads ASCII-encoded string data. Read characters into the return string until
    /// an END indicator or <see cref="ReadTerminationCharacter">termination character</see>
    /// termination character is reached. Limited by the
    /// <see cref="InputBufferSize"/>. Will time out if end of line is not read before reading a
    /// buffer.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <exception cref="cc.isr.VI.Pith.NativeException">  Thrown when a Native error condition
    ///                                                  occurs. </exception>
    /// <returns>   The received message. </returns>
    ///
    public override string ReadFiniteLine()
    {
        try
        {
            this.ClearLastError();
            if ( this.IsSessionOpen )
            {
                this.LastMessageReceived = this.IsSessionOpen ? this.VisaSession!.RawIO.ReadString() : this.EmulatedReply;
                // must clear the reply after each reading otherwise could get cross information.
                this.EmulatedReply = string.Empty;
            }
            return this.LastMessageReceived ?? string.Empty;
        }
        catch ( NativeVisaException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue
                ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, this.LastMessageSent, this.LastAction )
                : new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastMessageSent, this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
        catch ( IOTimeoutException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue
                ? new NativeError( NativeErrorCode.Timeout, this.OpenResourceName, this.LastNodeNumber.Value, this.LastMessageSent, this.LastAction )
                : new NativeError( NativeErrorCode.Timeout, this.OpenResourceName, this.LastMessageSent, this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
        finally
        {
            this.RestartKeepAliveTimer();
        }
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks> David, 2020-07-23. </remarks>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected override string SyncWriteLine( string dataToWrite )
    {
        if ( !string.IsNullOrWhiteSpace( dataToWrite ) )
        {
            try
            {
                this.ClearLastError();
                if ( this.IsSessionOpen )
                    this.VisaSession!.FormattedIO.WriteLine( dataToWrite );

                this.LastMessageSent = dataToWrite;
            }
            catch ( NativeVisaException ex )
            {
                this.LastNativeError = this.LastNodeNumber.HasValue
                    ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, dataToWrite, this.LastAction )
                    : new NativeError( ex.ErrorCode, this.OpenResourceName, dataToWrite, this.LastAction );
                throw new Pith.NativeException( this.LastNativeError, ex );
            }
            finally
            {
                this.RestartKeepAliveTimer();
            }
        }

        return dataToWrite ?? string.Empty;
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.<para>
    /// Per IVI documentation: Converts the specified string to an ASCII string and appends it to the
    /// formatted I/O write buffer</para>
    /// </summary>
    /// <remarks> David, 2020-07-23. </remarks>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected override string SyncWrite( string dataToWrite )
    {
        if ( !string.IsNullOrWhiteSpace( dataToWrite ) )
        {
            try
            {
                this.ClearLastError();
                if ( this.IsSessionOpen )
                    this.VisaSession!.FormattedIO.Write( dataToWrite );

                this.LastMessageSent = dataToWrite;
            }
            catch ( NativeVisaException ex )
            {
                this.LastNativeError = this.LastNodeNumber.HasValue
                    ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, dataToWrite, this.LastAction )
                    : new NativeError( ex.ErrorCode, this.OpenResourceName, dataToWrite, this.LastAction );
                throw new Pith.NativeException( this.LastNativeError, ex );
            }
            finally
            {
                this.RestartKeepAliveTimer();
            }
        }

        return dataToWrite ?? string.Empty;
    }

    /// <summary> Size of the input buffer. </summary>
    private int _inputBufferSize;

    /// <summary> Gets the size of the input buffer. </summary>
    /// <value> The size of the input buffer. </value>
    public override int InputBufferSize
    {
        get
        {
            if ( this._inputBufferSize == 0 )
                this._inputBufferSize = this.ReadBufferSizeAttribute();
            return this._inputBufferSize;
        }
    }

    #endregion

    #region " registers "

    /// <summary> Reads status byte. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    /// <returns> The status byte. </returns>
    protected override Pith.ServiceRequests ThreadUnsafeReadStatusByte()
    {
        try
        {
            this.ClearLastError();
            Pith.ServiceRequests value = this.EmulatedStatusByte;
            this.EmulatedStatusByte = 0;
            if ( this.IsSessionOpen )
                value = ( Pith.ServiceRequests ) ( int ) this.VisaSession!.ReadStatusByte();

            this.StatusByte = value;
            return this.StatusByte;
        }
        catch ( NativeVisaException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue
                ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, "@STB", this.LastAction )
                : new NativeError( ex.ErrorCode, this.OpenResourceName, "@STB", this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
        finally
        {
            this.RestartKeepAliveTimer();
        }
    }

    /// <summary> Clears the device. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    protected override void Clear()
    {
        try
        {
            this.ClearLastError();
            if ( this.IsSessionOpen )
                this.VisaSession!.Clear();
        }
        catch ( NativeVisaException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue
                ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, "@DCL", this.LastAction )
                : new NativeError( ex.ErrorCode, this.OpenResourceName, "@DCL", this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
        finally
        {
            this.RestartKeepAliveTimer();
        }
    }

    /// <summary> Clears the device (SDC). </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    protected override void ClearDevice()
    {
        this.Clear();
#if false
        // failed using NI USB to GPIB. May not be necessary after all.
        if ( this.SupportsClearInterface && this.Enabled )
        {
            using var gi = new GpibInterfaceSession();
            gi.OpenSession( this.ResourceNameInfo.InterfaceResourceName );
            if ( gi.IsOpen )
            {
                gi.SelectiveDeviceClear( this.VisaSession.ResourceName );
            }
            else
            {
                throw new InvalidOperationException( $"Failed opening GPIB Interface Session {this.ResourceNameInfo.InterfaceResourceName}" );
            }
        }
#endif
    }

    #endregion

    #region " events "

    #region " suspend / resume srq "

    /// <summary> Resume service request handing. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    public override void ResumeServiceRequestHanding()
    {
        this.SetLastAction( "resuming service request" );
        string lastMessage = string.Empty;
        try
        {
            base.ResumeServiceRequestHanding();
        }
        catch ( NativeVisaException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, lastMessage, this.LastAction ) : new NativeError( ex.ErrorCode, this.OpenResourceName, lastMessage, this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
    }

    /// <summary> Suspends the service request handling. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    public override void SuspendServiceRequestHanding()
    {
        this.SetLastAction( "suspending service request" );
        string lastMessage = string.Empty;
        try
        {
            base.SuspendServiceRequestHanding();
        }
        catch ( NativeVisaException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, lastMessage, this.LastAction ) : new NativeError( ex.ErrorCode, this.OpenResourceName, lastMessage, this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
    }

    #endregion

    /// <summary> Visa session service request. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Message based session event information. </param>
    private void OnServiceRequested( object? sender, VisaEventArgs e )
    {
        if ( sender is not null && e is not null && EventType.ServiceRequest == e.EventType )
        {
            if ( this.ServiceRequestHandlingSuspended )
            {
                this.ServiceRequestType = e.EventType.ToString();
                this.SuspendedServiceRequestedCount += 1;
            }
            else
            {
                this.OnServiceRequested( EventArgs.Empty );
            }
        }
    }

    /// <summary> Type of the enabled event. </summary>
    private EventType _enabledEventType;

    /// <summary>
    /// Gets or sets or set (protected) the sentinel indication if a service request event handler
    /// was enabled and registered.
    /// </summary>
    /// <value>
    /// <c>true</c> if service request event is enabled and registered; otherwise, <c>false</c>.
    /// </value>
    public override bool ServiceRequestEventEnabled
    {
        get => EventType.ServiceRequest == this._enabledEventType;
        set
        {
            if ( this.ServiceRequestEventEnabled != value )
            {
                this._enabledEventType = value ? EventType.ServiceRequest : EventType.Custom;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Enables and adds the service request event handler. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    public override void EnableServiceRequestEventHandler()
    {
        string lastMessage = string.Empty;
        this.SetLastAction( "enabling service request" );
        try
        {
            this.ClearLastError();
            if ( !this.ServiceRequestEventEnabled )
            {
                if ( this.IsSessionOpen )
                {
                    // must define the handler before enabling the events.
                    // Firewall access must be granted to VXI instrument for service request handling;
                    // With Windows 1909, both public and private network check box access must be checked.
                    lastMessage = "add SRQ handler";
                    this.VisaSession!.ServiceRequest += this.OnServiceRequested;

                    // Enabling the VISA session events causes an exception of unsupported mechanism.
                    // Apparently, the service request event is enabled when adding the event handler.
                    // verified using NI Trace. The NI trace shows
                    // viEnableEvent (TCPIP0::192.168.0.144::INST0::INSTR (0x00000001), 0x3FFF200B (VI_EVENT_SERVICE_REQ), 2 (0x2), 0 (0x0))
                    // as success following with the same command as failure.
                    // Note that enabling causes the unsupported mechanism exception.
                    // Removed per above note: Me.VisaSession.EnableEvent(Ivi.Visa.EventType.ServiceRequest)
                }
                // this turns on the enabled sentinel
                lastMessage = "turning on service request enabled";
                this.ServiceRequestEventEnabled = true;
            }
        }
        catch ( NativeVisaException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue
                ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, lastMessage, this.LastAction )
                : new NativeError( ex.ErrorCode, this.OpenResourceName, lastMessage, this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
    }

    /// <summary> Disables and removes the service request event handler. </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    public override void DisableServiceRequestEventHandler()
    {
        string lastMessage = string.Empty;
        this.SetLastAction( "disabling service request" );
        try
        {
            this.ClearLastError();
            if ( this.ServiceRequestEventEnabled )
            {
                if ( this.IsSessionOpen )
                {
                    lastMessage = "discard events";
                    this.DiscardServiceRequests();

                    if ( this.VisaSession is not null )
                    {
                        // Apparently, the service request event is still enabled after removing the event handler using
                        // VisSession.DisableEvent(Ivi.Visa.EventType.ServiceRequest). Therefore the following line is commented out:
                        // this.VisaSession.DisableEvent(Ivi.Visa.EventType.ServiceRequest)

                        // Note that disabling twice does not cause an exception.
                        lastMessage = "remove SRQ handler";
                        this.VisaSession.ServiceRequest -= this.OnServiceRequested;
                    }
                }
                // this turns off the enabled sentinel
                lastMessage = "turning off service request enabled";
                this.ServiceRequestEventEnabled = false;
            }
        }
        catch ( NativeVisaException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue
                ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, lastMessage, this.LastAction )
                : new NativeError( ex.ErrorCode, this.OpenResourceName, lastMessage, this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
    }

    /// <summary> Discard pending service requests. </summary>
    public override void DiscardServiceRequests()
    {
        if ( this.IsSessionOpen )
            this.VisaSession!.DiscardEvents( EventType.ServiceRequest );
    }

    #endregion

    #region " trigger "

    /// <summary> Gets or sets the 'trigger' command. </summary>
    /// <value> The 'trigger' command. </value>
    private string TriggerCommand { get; set; } = "*TRG";

    /// <summary>
    /// Asserts a software or hardware trigger depending on the interface; Sends a bus trigger.
    /// </summary>
    /// <exception cref="cc.isr.VI.Pith.NativeException"> Thrown when a Native error condition occurs. </exception>
    public override void AssertTrigger()
    {
        try
        {
            this.ClearLastError();
            if ( this.IsSessionOpen )
                this.VisaSession!.AssertTrigger();
        }
        catch ( NativeVisaException ex )
        {
            this.LastNativeError = this.LastNodeNumber.HasValue
                ? new NativeError( ex.ErrorCode, this.OpenResourceName, this.LastNodeNumber.Value, this.TriggerCommand, this.LastAction )
                : new NativeError( ex.ErrorCode, this.OpenResourceName, this.TriggerCommand, this.LastAction );
            throw new Pith.NativeException( this.LastNativeError, ex );
        }
        finally
        {
            this.RestartKeepAliveTimer();
        }
    }

    #endregion

    #region " interface "

    /// <summary> Keeps the TCP/IP interface alive. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="tcpIpSession"> The TCP IP session. </param>
    /// <returns> A TimeSpan. </returns>
    [CLSCompliant( false )]
    public static TimeSpan KeepInterfaceAlive( ITcpipSession tcpIpSession )
    {
        if ( tcpIpSession is null ) throw new ArgumentNullException( nameof( tcpIpSession ) );
        Stopwatch sw = Stopwatch.StartNew();
        {
            if ( GlobalResourceManager.Open( $"TCPIP0::{tcpIpSession.Address}::{tcpIpSession.Port}::socket",
                AccessModes.None, 3000, out ResourceOpenStatus resourceOpenStatus ) is ITcpipSocketSession gi && resourceOpenStatus == ResourceOpenStatus.Success )
            {
                if ( gi.ResourceLockState == ResourceLockState.NoLock )
                    gi.LockResource( 1 );
                gi.UnlockResource();
            }
        }

        return sw.Elapsed;
    }

    /// <summary> Clears the interface. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    protected override void ImplementClearHardwareInterface()
    {
        if ( this.SupportsClearInterface && this.Enabled && this.ResourceNameInfo is not null && !string.IsNullOrWhiteSpace( this.ResourceNameInfo.InterfaceResourceName ) )
        {
            using GpibInterfaceSession gi = new();
            gi.OpenSession( this.ResourceNameInfo.InterfaceResourceName! );
            if ( gi.IsOpen )
            {
                gi.SelectiveDeviceClear( this.VisaSession!.ResourceName );
            }
            else
            {
                throw new InvalidOperationException( $"Failed opening GPIB Interface Session {this.ResourceNameInfo.InterfaceResourceName}" );
            }
        }
    }

    #endregion

    #region " ics 9065 keep alive "

    /// <summary> Sends a TCP/IP message to keep the ICS 9065 connection from timing out. </summary>
    /// <returns> <c>true</c> if success; otherwise <c>false</c> </returns>
    public override void KeepAlive()
    {
        if ( this.IsSessionOpen && this.VisaSession is not null && this.VisaSession.HardwareInterfaceType == HardwareInterfaceType.Tcp )
        {
            if ( this.VisaSession.ResourceLockState == Ivi.Visa.ResourceLockState.NoLock )
            {
                this.VisaSession.LockResource( ( int ) this.KeepAliveLockTimeout.TotalMilliseconds );
            }
            this.VisaSession.UnlockResource();
        }
    }

    #endregion
}
