using cc.isr.VI.Pith;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm.Console.UI;
public partial class ConsoleSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{

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


    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [ObservableProperty]
    public partial string K2600ResourceName { get; set; } = "TCPIP0::192.168.0.50::inst0::INSTR";

    /// <summary>   Gets or sets the 2600 resource model. </summary>
    /// <value> The k 2600 resource model. </value>
    [ObservableProperty]
    public partial string K2600ResourceModel { get; set; } = "TTM 2024";

    /// <summary>   Gets or sets the resource name selection timeout. </summary>
    /// <value> The resource name selection timeout. </value>
    [ObservableProperty]
    public partial TimeSpan ResourceNameSelectionTimeout { get; set; } = TimeSpan.FromSeconds( 5 );

    /// <summary>   Gets or sets the trace level. </summary>
    /// <value> The trace level. </value>
    [ObservableProperty]
    public partial Microsoft.Extensions.Logging.LogLevel TraceLevel { get; set; } = Microsoft.Extensions.Logging.LogLevel.Trace;

    /// <summary>   Gets or sets the closed caption. </summary>
    /// <value> The closed caption. </value>
    [ObservableProperty]
    [Description( "The caption to display when the resource is closed" )]
    public partial string ClosedCaption { get; set; } = "closed";

    #region " ping "

    /// <summary>   Gets or sets the ping timeout. </summary>
    /// <value> The ping timeout. </value>
    [ObservableProperty]
    [Description( "The time to allow for ping results to come back [sms]" )]
    public partial TimeSpan PingTimeout { get; set; } = TimeSpan.FromMilliseconds( 5 );

    /// <summary>   Gets or sets the PingHops. </summary>
    /// <value> The PingHops. </value>
    [ObservableProperty]
    [Description( "The number of hops to set when pinging an instrument [1]" )]
    public partial int PingHops { get; set; } = 1;

    private bool? _isTcpIpResource;

    /// <summary>   Gets value indicating if the resource name is a TcpIp resource. </summary>
    /// <value> <c>true</c> if the resource is a TcpIp resource; otherwise <c>false</c>. </value>
    public bool IsTcpIpResource
    {
        get
        {
            if ( !this._isTcpIpResource.HasValue )
                this._isTcpIpResource = ResourceNamesManager.IsTcpIpResource( this.K2600ResourceName );
            return this._isTcpIpResource.Value;
        }
    }

    private bool? _resourcePinged;

    /// <summary> Gets the resource pinged. </summary>
    /// <value> The resource pinged. </value>
    public virtual bool ResourcePinged
    {
        get
        {
            if ( !this._resourcePinged.HasValue )
                this._resourcePinged = !this.IsTcpIpResource || ResourceNamesManager.PingTcpIpResource( this.K2600ResourceName );
            return this._resourcePinged.Value;
        }
    }

    #endregion

}
