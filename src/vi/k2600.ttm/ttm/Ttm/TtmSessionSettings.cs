using System.ComponentModel;
using cc.isr.VI.Pith;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class TtmSessionSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmSessionSettings() { }

    #endregion

    #region " exists "

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #region " session "

    /// <summary>   Gets or sets the session message notification modes. </summary>
    /// <value> The session message notification modes. </value>
    [ObservableProperty]
    [Description( "Specifies the modes of session notification, e.g., none or message received, sent or both [None; 0]" )]
    public partial MessageNotificationModes SessionMessageNotificationModes { get; set; } = MessageNotificationModes.None;

    /// <summary>   Gets or sets the trace log level. </summary>
    /// <value> The trace log level. </value>
    [ObservableProperty]
    [Description( "Specifies the trace level [Information; 8]" )]
    public partial System.Diagnostics.TraceEventType TraceLogLevel { get; set; } = System.Diagnostics.TraceEventType.Information;

    /// <summary>   Gets or sets the trace Show level. </summary>
    /// <value> The trace Show level. </value>
    [ObservableProperty]
    [Description( "Specifies the trace show level [Information; 8]" )]
    public partial System.Diagnostics.TraceEventType TraceShowLevel { get; set; } = System.Diagnostics.TraceEventType.Information;

    /// <summary>   Gets or sets a value indicating whether the device is enabled. </summary>
    /// <value> True if device enabled, false if not. </value>
    [ObservableProperty]
    [Description( "True if the device is enabled [true]" )]
    public partial bool DeviceEnabled { get; set; } = true;

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [ObservableProperty]
    [Description( "Specified the resource name [TCPIP0::192.168.0.150::inst0::INSTR]" )]
    public partial string ResourceName { get; set; } = "TCPIP0::192.168.0.150::inst0::INSTR";

    #endregion
}
