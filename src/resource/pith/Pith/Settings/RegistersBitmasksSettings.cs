using System.ComponentModel;

namespace cc.isr.VI.Pith.Settings;

/// <summary>   The visa session registers bitmasks settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
[CLSCompliant( false )]
public class RegistersBitmasksSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings were found and read from the settings file." )]
    public bool Exists
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>   Gets or sets the Error available bitmask. </summary>
    /// <value> The Error available bitmask. </value>
    [Description( "The error available bitmask [4]" )]
    public int ErrorAvailableBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = ( int ) ServiceRequests.ErrorAvailable;

    /// <summary>   Gets or sets the message available bitmask. </summary>
    /// <value> The message available bitmask. </value>
    [Description( "The message available service request bitmask [16; 0x10]" )]
    public int MessageAvailableBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = ( int ) ServiceRequests.MessageAvailable;

    /// <summary> Gets or sets the bitmask indicating that a device requested a service that was
    ///           enabled in the standard register. </summary>
    /// <value> The standard event summary bitmask. </value>
    [Description( "The standard event summary bitmask [0x20, 32]" )]
    public int EventSummaryBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = ( int ) ServiceRequests.StandardEventSummary;

    /// <summary>   Gets or sets the bitmask indicating that a device requested service. </summary>
    /// <value> The requesting service bitmask. </value>
    [Description( "The requested service service request event bitmask [64; 0x40]" )]
    public int RequestingServiceBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = ( int ) ServiceRequests.RequestingService;

    /// <summary>
    /// Gets or sets the bitmask to enable service request events on operation completion which
    /// excludes the message available event. Now includes message available [0xBF, 191]. Was [0xAF, 175]
    /// </summary>
    /// <value> The service request operation complete event enable bitmask. </value>
    [Description( "The bit mask to enable the service request operation complete event [0xBF, 191]" )]
    public int ServiceRequestEnableOperationCompleteBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the bitmask to enable standard event register operation complete events [0xFD, 253]. </summary>
    /// <value> The the bitmask to enable standard event register operation complete events. </value>
    [Description( "The bitmask to enable standard event register operation complete events [0xFD, 253]" )]
    public int StandardEventEnableOperationCompleteBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the bitmask to enable service request events [0xBF, 191]. </summary>
    /// <value> The service request events enable bitmask. </value>
    [Description( "The bitmask to enable service request events [0xBF, 191]" )]
    public int ServiceRequestEnableEventsBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the bitmask to enable standard events [0xFD, 253]. </summary>
    /// <value> The bitmask to enable standard events. </value>
    [Description( "The bitmask to enable standard event register events [0xFD, 253]" )]
    public int StandardEventEnableEventsBitmask
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

}
