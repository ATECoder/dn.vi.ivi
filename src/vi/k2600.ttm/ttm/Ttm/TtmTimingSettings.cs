using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Timing. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class TtmTimingSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmTimingSettings() { }

    #endregion

    #region " exists "

    private bool _exists;

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
	[Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get => this._exists;
        set => _ = this.SetProperty( ref this._exists, value );
    }

    #endregion

    #region " timing properties "

    // TODO: use session data.

    private TimeSpan _resetRefractoryPeriod = TimeSpan.FromSeconds( 0.2 );

    /// <summary>   Gets or sets the reset refractory period. </summary>
    /// <value> The reset refractory period. </value>
	[Description( "The time to wait before returning control after resetting the instrument (*RST)" )]
    public TimeSpan ResetRefractoryPeriod
    {
        get => this._resetRefractoryPeriod;
        set => this.SetProperty( ref this._resetRefractoryPeriod, value );
    }

    private TimeSpan _initRefractoryPeriod = TimeSpan.FromMilliseconds( 100 );

    /// <summary>   Gets or sets the initialize refractory period. </summary>
    /// <value> The initialize refractory period. </value>
	[Description( "The time to wait before returning control after initializing the instrument" )]
    public TimeSpan InitRefractoryPeriod
    {
        get => this._initRefractoryPeriod;
        set => this.SetProperty( ref this._initRefractoryPeriod, value );
    }

    private TimeSpan _clearRefractoryPeriod = TimeSpan.FromMilliseconds( 100 );

    /// <summary>   Gets or sets the clear refractory period. </summary>
    /// <value> The clear refractory period. </value>
	[Description( "The time to wait before returning control after clearing the instrument (*CLS)" )]
    public TimeSpan ClearRefractoryPeriod
    {
        get => this._clearRefractoryPeriod;
        set => this.SetProperty( ref this._clearRefractoryPeriod, value );
    }

    private Pith.MessageNotificationModes _sessionMessageNotificationModes = Pith.MessageNotificationModes.None;

    /// <summary>   Gets or sets the session message notification modes. </summary>
    /// <value> The session message notification modes. </value>
	[Description( "Specifies the modes of session notification, e.g., none or message received, sent or both" )]
    public Pith.MessageNotificationModes SessionMessageNotificationModes
    {
        get => this._sessionMessageNotificationModes;
        set => this.SetProperty( ref this._sessionMessageNotificationModes, value );
    }

    private TimeSpan _statusReadTurnaroundTime = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the status read turnaround time. </summary>
    /// <value> The status read turnaround time. </value>
	[Description( "The time it takes to read the status byte" )]
    public TimeSpan StatusReadTurnaroundTime
    {
        get => this._statusReadTurnaroundTime;
        set => this.SetProperty( ref this._statusReadTurnaroundTime, value );
    }

    private TimeSpan _readAfterWriteDelay = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the read delay. </summary>
    /// <value> The read delay. </value>
	[Description( "The time to wait before reading the instrument after a write" )]
    public TimeSpan ReadAfterWriteDelay
    {
        get => this._readAfterWriteDelay;
        set => this.SetProperty( ref this._readAfterWriteDelay, value );
    }

    private TimeSpan _statusReadDelay = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the status read delay. </summary>
    /// <value> The status read delay. </value>
	[Description( "The time to wait after a reading the status byte" )]
    public TimeSpan StatusReadDelay
    {
        get => this._statusReadDelay;
        set => this.SetProperty( ref this._statusReadDelay, value );
    }

    private TimeSpan _initializeTimeout = TimeSpan.FromSeconds( 10 );

    /// <summary>   Gets or sets the initialize timeout. </summary>
    /// <value> The initialize timeout. </value>
	[Description( "The VISA session timeout to set for initializing the instrument" )]
    public TimeSpan InitializeTimeout
    {
        get => this._initializeTimeout;
        set => this.SetProperty( ref this._initializeTimeout, value );
    }

    private TimeSpan _deviceClearRefractoryPeriod = TimeSpan.FromMilliseconds( 500 );

    /// <summary>   Gets or sets the device clear refractory period. </summary>
    /// <value> The device clear refractory period. </value>
	[Description( "The time to wait before returning control after a device clearing the instrument (*DCl)" )]
    public TimeSpan DeviceClearRefractoryPeriod
    {
        get => this._deviceClearRefractoryPeriod;
        set => this.SetProperty( ref this._deviceClearRefractoryPeriod, value );
    }

    private TimeSpan _interfaceClearRefractoryPeriod = TimeSpan.FromMilliseconds( 500 );

    /// <summary>   Gets or sets the interface clear refractory period. </summary>
    /// <value> The interface clear refractory period. </value>
	[Description( "The time to wait before returning control after a clearing the instrument interface (selective device clear)" )]
    public TimeSpan InterfaceClearRefractoryPeriod
    {
        get => this._interfaceClearRefractoryPeriod;
        set => this.SetProperty( ref this._interfaceClearRefractoryPeriod, value );
    }

    #endregion
}
