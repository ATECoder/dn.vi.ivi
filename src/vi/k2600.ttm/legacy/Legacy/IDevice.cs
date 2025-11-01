namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines an interface for the Thermal Transient Device. This is an apparatus with a
/// device address. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2024-11-07, 8.1.9077.x. A minimal interface stripped to match the current FET implementation. </para><para>
/// David, 2013-12-12, 3.0.3053.x. </para>
/// </remarks>
public interface IDevice : IApparatus
{
    #region " connect "

    /// <summary> Connects to the thermal transient device. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> <c>True</c> if the device connected. </returns>
    public bool Connect( string resourceName );

    /// <summary> Gets or sets the sentinel indicating if the thermal transient meter is connected. </summary>
    /// <value> The is connected. </value>
    public bool IsConnected { get; }

    /// <summary> Aborts execution, resets the device to known state and clears queues and register. </summary>
    /// <remarks> Issues <see cref="VisaSessionBase.ClearActiveState">clear active
    /// state</see>, *RST, and *CLS Typically implements SDC, RST, CLS and clearing error queue. </remarks>
    public bool ResetAndClear();

    /// <summary> Disconnects from the thermal transient meter. </summary>
    /// <returns> <c>True</c> if the device  disconnected. </returns>
    public bool Disconnect();

    /// <summary> Gets or sets the instrument resource address. </summary>
    /// <value> The name of the resource. </value>
    public string ResourceName { get; set; }

    /// <summary>   Clears the output buffer. </summary>
    /// <returns>
    /// A string containing any orphan messages that were left unread in the instrument output buffer.
    /// </returns>
    public string FlushRead();

    #endregion

    #region " synchronizer  "

    /// <summary> Sets the <see cref="System.ComponentModel.ISynchronizeInvoke">synchronization
    /// object</see>
    /// to use for marshaling events. </summary>
    /// <param name="value"> The value. </param>
    public void SynchronizerSetter( System.ComponentModel.ISynchronizeInvoke value );

    #endregion

    #region " trigger "

    /// <summary> Sends a signal to abort the triggered measurement. </summary>
    public bool AbortMeasurement( double timeout = 1.0 );

    /// <summary>
    /// Primes the instrument to wait for a trigger. This clears the digital outputs and loops until
    /// trigger or <see cref="Pith.SessionBase.AssertTrigger"/> command or external *TRG command.
    /// </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="lockLocal">        (Optional) When true, displays the title and locks the local
    ///                                 key. </param>
    /// <param name="onCompleteReply">  (Optional) The operation completion reply. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool PrepareForTrigger( bool lockLocal = false, string onCompleteReply = "OPC" );

    /// <summary> Reads the status byte and returns true if the message available bit is set. </summary>
    public bool IsMeasurementCompleted();

    /// <summary>   Read the measurements from the instrument. </summary>
    /// <param name="readTriggerCycleReply">    (Optional) [true] True to read trigger
    ///                                         cycle reply before reading the measurements. </param>
    /// <returns>   The measurements. </returns>
    public bool ReadMeasurements( bool readTriggerCycleReply = true );

    #endregion

    #region " reporting "

    /// <summary> Sends a message to the client.
    /// </summary>
    public event EventHandler<MessageEventArgs> MessageAvailable;

    /// <summary> Sends an Exception message to the client. </summary>
    public event EventHandler<System.Threading.ThreadExceptionEventArgs> ExceptionAvailable;

    #endregion
}
