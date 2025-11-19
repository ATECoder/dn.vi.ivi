// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Pith;

/// <summary> A Dummy message based session. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-20 </para>
/// </remarks>
[CLSCompliant( false )]
public class DummySession : SessionBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the MessageBasedSession object from the specified resource name.
    /// </summary>
    public DummySession() : base()
    {
    }

    #endregion

    #region " session "

    /// <summary> Gets the sentinel indicating whether this is a dummy session. </summary>
    /// <value> The dummy sentinel. </value>
    public override bool IsDummy { get; } = true;

    /// <summary>
    /// Gets the session open sentinel. When open, the session is capable of addressing the hardware.
    /// See also <see cref="P:VI.Pith.SessionBase.IsDeviceOpen" />.
    /// </summary>
    /// <value> The is session open. </value>
    public override bool IsSessionOpen => this.IsDeviceOpen;

    /// <summary> Initializes a dummy session. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="timeout">      The open timeout. </param>
    protected override void CreateSession( string resourceName, TimeSpan timeout )
    {
        this._isSessionDisposed = false;
        this.ClearLastError();
    }

    /// <summary> Discards session. </summary>
    protected override void DiscardAllEvents()
    {
        this.ClearLastError();
    }

    private bool _isSessionDisposed;

    /// <summary> Gets or sets the sentinel indication that the VISA session is disposed. </summary>
    /// <value> The is session disposed. </value>
    public override bool IsSessionDisposed => this._isSessionDisposed;

    /// <summary> Dispose session. </summary>
    protected override void DisposeSession()
    {
        this._isSessionDisposed = true;
    }

    /// <summary>
    /// Checks if the candidate resource name exists. If so, assign to the
    /// <see cref="cc.isr.VI.Pith.SessionBase.ValidatedResourceName">validated resource name</see>
    /// </summary>
    /// <returns> <c>true</c> if it the resource exists; otherwise <c>false</c> </returns>
    public override bool ValidateCandidateResourceName()
    {
        using DummyResourcesProvider rm = new();
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
    public override bool SynchronizeCallbacks { get; set; }

    #endregion

    #region " attributes "

    /// <summary> Gets read buffer size. </summary>
    /// <returns> The read buffer size. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private int Get_ReadBufferSize()
    {
        return 1024;
    }

    #endregion

    #region " read/write "

    /// <summary> Synchronously reads ASCII-encoded string data. </summary>
    /// <returns> The received message. </returns>
    public override string ReadFreeLine()
    {
        this.ClearLastError();
        this.LastMessageReceived = this.EmulatedReply;
        return this.LastMessageReceived;
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface. Terminates the
    /// data with the <see cref="SessionBase.ReadTerminationCharacter">termination character</see>. <para>
    /// Per IVI documentation: Appends a newline (0xA) to the formatted I/O write buffer, flushes the
    /// buffer, and sends an End-of-Line with the buffer if required.</para>
    /// </summary>
    /// <returns> The received message. </returns>
    public override string ReadFiniteLine()
    {
        this.ClearLastError();
        this.LastMessageReceived = this.EmulatedReply;
        return this.LastMessageReceived;
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface. Terminates the
    /// data with the <see cref="SessionBase.ReadTerminationCharacter">termination character</see>.
    /// </summary>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected override string SyncWriteLine( string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        this.ClearLastError();
        this.LastMessageSent = dataToWrite;
        return dataToWrite;
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.<para>
    /// Per IVI documentation: Converts the specified string to an ASCII string and appends it to the
    /// formatted I/O write buffer</para>
    /// </summary>
    /// <remarks> David, 2020-07-23. </remarks>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected override string SyncWrite( string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        this.ClearLastError();
        this.LastMessageSent = dataToWrite;
        return dataToWrite;
    }

    /// <summary> Gets the size of the input buffer. </summary>
    /// <value> The size of the input buffer. </value>
    public override int InputBufferSize => 4096;

    #endregion

    #region " registers "

    /// <summary> Reads status byte. </summary>
    /// <returns> The status byte. </returns>
    protected override ServiceRequests ThreadUnsafeReadStatusByte()
    {
        this.ClearLastError();
        this.StatusByte = this.EmulatedStatusByte;
        this.EmulatedStatusByte = 0;
        return this.StatusByte;
    }

    /// <summary> Clears the device. </summary>
    protected override void Clear()
    {
        this.ClearLastError();
    }

    #endregion

    #region " events "

    /// <summary> Discard service requests. </summary>
    public override void DiscardServiceRequests()
    {
        this.ClearLastError();
    }

    /// <summary>   Awaits service request status. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="timeout">          The open timeout. </param>
    /// <returns>   A (TimedOut As Boolean, Status As VI.Pith.ServiceRequests) </returns>
    public override (bool TimedOut, ServiceRequests StatusByte, TimeSpan Elapsed) AwaitServiceRequest( TimeSpan timeout )
    {
        this.MakeEmulatedReplyIfEmpty( this.ServiceRequestEnableBitmask.GetValueOrDefault( ServiceRequests.RequestingService ) );
        return (false, this.ServiceRequestEnableBitmask!.Value, TimeSpan.Zero);
    }

    /// <summary> True to enable, false to disable the service request event handler. </summary>
    private bool _serviceRequestEventHandlerEnabled;

    /// <summary>
    /// Gets or sets or set (protected) the sentinel indication if a service request event handler
    /// was enabled and registered.
    /// </summary>
    /// <value>
    /// <c>true</c> if service request event is enabled and registered; otherwise, <c>false</c>.
    /// </value>
    public override bool ServiceRequestEventEnabled
    {
        get => this._serviceRequestEventHandlerEnabled;
        set => _ = base.SetProperty( ref this._serviceRequestEventHandlerEnabled, value );
    }

    /// <summary> Enables the service request. </summary>
    public override void EnableServiceRequestEventHandler()
    {
        if ( !this.ServiceRequestEventEnabled )
        {
            this.ClearLastError();
            this.ServiceRequestEventEnabled = true;
        }
    }

    /// <summary> Disables the service request. </summary>
    public override void DisableServiceRequestEventHandler()
    {
        if ( this.ServiceRequestEventEnabled )
        {
            this.ClearLastError();
            this.ServiceRequestEventEnabled = false;
        }
    }

    #endregion

    #region " trigger "

    /// <summary>
    /// Asserts a software or hardware trigger depending on the interface; Sends a bus trigger.
    /// </summary>
    public override void AssertTrigger()
    {
        this.ClearLastError();
    }

    #endregion

    #region " interface "

    /// <summary> Clears the interface. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    protected override void ImplementClearHardwareInterface()
    {
        if ( this.SupportsClearInterface && this.IsSessionOpen )
        {
            using DummyGpibInterfaceSession gi = new();
            gi.OpenSession( this.ResourceNameInfo!.InterfaceResourceName! );
            if ( gi.IsOpen )
            {
                gi.SelectiveDeviceClear( this.OpenResourceName! );
            }
            else
            {
                throw new InvalidOperationException( $"Failed opening GPIB Interface Session {this.ResourceNameInfo.InterfaceResourceName}" );
            }
        }
    }

    /// <summary> Clears the device (SDC). </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    protected override void ClearDevice()
    {
        this.Clear();
        if ( this.SupportsClearInterface && this.IsSessionOpen )
        {
            using DummyGpibInterfaceSession gi = new();
            gi.OpenSession( this.ResourceNameInfo!.InterfaceResourceName! );
            if ( gi.IsOpen )
            {
                gi.SelectiveDeviceClear( this.OpenResourceName! );
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
    }

    #endregion
}
