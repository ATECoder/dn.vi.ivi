using System.ComponentModel;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class TtmSessionSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmSessionSettings() { }

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

    #region " session "

    private MessageNotificationModes _sessionMessageNotificationModes = MessageNotificationModes.None;

    /// <summary>   Gets or sets the session message notification modes. </summary>
    /// <value> The session message notification modes. </value>
    [Description( "Specifies the modes of session notification, e.g., none or message received, sent or both [None; 0]" )]
    public MessageNotificationModes SessionMessageNotificationModes
    {
        get => this._sessionMessageNotificationModes;
        set => this.SetProperty( ref this._sessionMessageNotificationModes, value );
    }

    private System.Diagnostics.TraceEventType _traceLogLevel = System.Diagnostics.TraceEventType.Information;

    /// <summary>   Gets or sets the trace log level. </summary>
    /// <value> The trace log level. </value>
    [Description( "Specifies the trace level [Information; 8]" )]
    public System.Diagnostics.TraceEventType TraceLogLevel
    {
        get => this._traceLogLevel;
        set => this.SetProperty( ref this._traceLogLevel, value );
    }

    private System.Diagnostics.TraceEventType _traceShowLevel = System.Diagnostics.TraceEventType.Information;

    /// <summary>   Gets or sets the trace Show level. </summary>
    /// <value> The trace Show level. </value>
    [Description( "Specifies the trace show level [Information; 8]" )]
    public System.Diagnostics.TraceEventType TraceShowLevel
    {
        get => this._traceShowLevel;
        set => this.SetProperty( ref this._traceShowLevel, value );
    }

    private bool _deviceEnabled = true;

    /// <summary>   Gets or sets a value indicating whether the device is enabled. </summary>
    /// <value> True if device enabled, false if not. </value>
    [Description( "True if the device is enabled [true]" )]
    public bool DeviceEnabled
    {
        get => this._deviceEnabled;
        set => this.SetProperty( ref this._deviceEnabled, value );
    }

    private string _resourceName = "TCPIP0::192.168.0.150::inst0::INSTR";

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [Description( "Specified the resource name [TCPIP0::192.168.0.150::inst0::INSTR]" )]
    public string ResourceName
    {
        get => this._resourceName;
        set => this.SetProperty( ref this._resourceName, value );
    }

    #endregion
}
