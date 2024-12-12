using System.ComponentModel;

namespace cc.isr.VI.Pith.Settings;

/// <summary>   The visa session timing settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
[CLSCompliant( false )]
public class TimingSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    private bool _exists;

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
	[Description( "True if this settings were found and read from the settings file." )]
    public bool Exists
    {
        get => this._exists;
        set => _ = this.SetProperty( ref this._exists, value );
    }

    private TimeSpan _openSessionTimeout = TimeSpan.FromSeconds( 0.5 );

    /// <summary>   Gets or sets the default open session timeout. </summary>
    /// <value> The default open session timeout. </value>
	[Description( "The time to wait for a session to open [0.5s]" )]
    public TimeSpan OpenSessionTimeout
    {
        get => this._openSessionTimeout;
        set => this.SetProperty( ref this._openSessionTimeout, value );
    }

    private TimeSpan _communicationTimeout = TimeSpan.FromSeconds( 3.05 );

    /// <summary>   Gets or sets the default open session timeout. </summary>
    /// <value> The default open session timeout. </value>
	[Description( "The time to wait query turnaround [3.05s]" )]
    public TimeSpan CommunicationTimeout
    {
        get => this._communicationTimeout;
        set => this.SetProperty( ref this._communicationTimeout, value );
    }

    private TimeSpan _initKnownStateTimeout = TimeSpan.FromSeconds( 10 );

    /// <summary>   Gets or sets the timeout for initializing all subsystem following a reset and clear. </summary>
    /// <value> The timeout for initialize the post reset clear state. </value>
	[Description( "The VISA session timeout for initializing all subsystem following a reset and clear [10s]" )]
    public TimeSpan InitKnownStateTimeout
    {
        get => this._initKnownStateTimeout;
        set => this.SetProperty( ref this._initKnownStateTimeout, value );
    }

    private TimeSpan _deviceClearRefractoryPeriod = TimeSpan.FromMilliseconds( 500 );

    /// <summary>   Gets or sets the device clear refractory period. </summary>
    /// <value> The device clear refractory period. </value>
	[Description( "The time to wait before returning control after a device clearing the instrument (*DCl) [500ms]" )]
    public TimeSpan DeviceClearRefractoryPeriod
    {
        get => this._deviceClearRefractoryPeriod;
        set => this.SetProperty( ref this._deviceClearRefractoryPeriod, value );
    }

    private TimeSpan _interfaceClearRefractoryPeriod = TimeSpan.FromMilliseconds( 500 );

    /// <summary>   Gets or sets the interface clear refractory period. </summary>
    /// <value> The interface clear refractory period. </value>
	[Description( "The time to wait before returning control after a clearing the instrument interface (selective device clear) [0.5s]" )]
    public TimeSpan InterfaceClearRefractoryPeriod
    {
        get => this._interfaceClearRefractoryPeriod;
        set => this.SetProperty( ref this._interfaceClearRefractoryPeriod, value );
    }

    private TimeSpan _resetRefractoryPeriod = TimeSpan.FromSeconds( 0.2 );

    /// <summary>   Gets or sets the reset refractory period. </summary>
    /// <value> The reset refractory period. </value>
	[Description( "The time to wait before returning control after resetting the session to its know state (*RST) [0.2s]" )]
    public TimeSpan ResetRefractoryPeriod
    {
        get => this._resetRefractoryPeriod;
        set => this.SetProperty( ref this._resetRefractoryPeriod, value );
    }

    private TimeSpan _clearRefractoryPeriod = TimeSpan.FromMilliseconds( 100 );

    /// <summary>   Gets or sets the clear refractory period. </summary>
    /// <value> The clear refractory period. </value>
	[Description( "The time to wait before returning control after clearing the instrument (*CLS) [100ms]" )]
    public TimeSpan ClearRefractoryPeriod
    {
        get => this._clearRefractoryPeriod;
        set => this.SetProperty( ref this._clearRefractoryPeriod, value );
    }

    private TimeSpan _statusReadTurnaroundTime = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the status read turnaround time. </summary>
    /// <value> The status read turnaround time. </value>
	[Description( "The time it takes to read the status byte [10ms]" )]
    public TimeSpan StatusReadTurnaroundTime
    {
        get => this._statusReadTurnaroundTime;
        set => this.SetProperty( ref this._statusReadTurnaroundTime, value );
    }

    private TimeSpan _statusReadDelay = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the status read delay. </summary>
    /// <value> The status read delay. </value>
	[Description( "The time to wait after a reading the status byte [10ms]" )]
    public TimeSpan StatusReadDelay
    {
        get => this._statusReadDelay;
        set => this.SetProperty( ref this._statusReadDelay, value );
    }

    private TimeSpan _readAfterWriteDelay = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the read after write delay. </summary>
    /// <value> The read delay. </value>
	[Description( "The time to wait before reading the instrument after a write [10ms]" )]
    public TimeSpan ReadAfterWriteDelay
    {
        get => this._readAfterWriteDelay;
        set => this.SetProperty( ref this._readAfterWriteDelay, value );
    }

    private TimeSpan _collectGarbageTimeout = TimeSpan.FromSeconds( 0.5 );

    /// <summary>   Gets or sets the time to wait for garbage collection. </summary>
    /// <value> The time to wait for garbage collection. </value>
	[Description( "The time to wait for garbage collection [0.5s]" )]
    public TimeSpan CollectGarbageTimeout
    {
        get => this._collectGarbageTimeout;
        set => this.SetProperty( ref this._collectGarbageTimeout, value );
    }

    private TimeSpan _operationCompletionRefractoryPeriod;

    /// <summary>
    /// Gets or sets the operation completion timeout (was status subsystem initialize refractory
    /// period) for instruments that do not support operation completion.
    /// </summary>
    /// <value> The operation completion timeout. </value>
	[Description( "Specifies the time to wait for operation completion [100ms]" )]
    public TimeSpan OperationCompletionRefractoryPeriod
    {
        get => this._operationCompletionRefractoryPeriod;
        set => this.SetProperty( ref this._operationCompletionRefractoryPeriod, value );
    }

    private MessageNotificationModes _sessionMessageNotificationModes = MessageNotificationModes.None;

    /// <summary>   Gets or sets the session message notification modes. </summary>
    /// <value> The session message notification modes. </value>
	[Description( "Specifies the modes of session notification, e.g., none or message received, sent or both [None; 0]" )]
    public MessageNotificationModes SessionMessageNotificationModes
    {
        get => this._sessionMessageNotificationModes;
        set => this.SetProperty( ref this._sessionMessageNotificationModes, value );
    }

    #region " ping "

    private TimeSpan _pingTimeout = TimeSpan.FromMilliseconds( 5 );

    /// <summary>   Gets or sets the ping timeout. </summary>
    /// <value> The ping timeout. </value>
	[Description( "The time to allow for ping results to come back [sms]" )]
    public TimeSpan PingTimeout
    {
        get => this._pingTimeout;
        set => this.SetProperty( ref this._pingTimeout, value );
    }

    private int _pingHops = 1;

    /// <summary>   Gets or sets the PingHops. </summary>
    /// <value> The PingHops. </value>
	[Description( "The number of hops to set when pinging an instrument [1]" )]
    public int PingHops
    {
        get => this._pingHops;
        set => this.SetProperty( ref this._pingHops, value );
    }

    #endregion

}
