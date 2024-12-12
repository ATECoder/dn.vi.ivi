using cc.isr.VI.Pith;
using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm.Console.UI;
public class ConsoleSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{

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

    private string _k2600ResourceName = "TCPIP0::192.168.0.50::inst0::INSTR";

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    public string K2600ResourceName
    {
        get => this._k2600ResourceName;
        set => this.SetProperty( ref this._k2600ResourceName, value );
    }

    private string _k2600ResourceModel = "TTM 2024";

    /// <summary>   Gets or sets the 2600 resource model. </summary>
    /// <value> The k 2600 resource model. </value>
	public string K2600ResourceModel
    {
        get => this._k2600ResourceModel;
        set => this.SetProperty( ref this._k2600ResourceModel, value );
    }

    private TimeSpan _resourceNameSelectionTimeout = TimeSpan.FromSeconds( 5 );

    /// <summary>   Gets or sets the resource name selection timeout. </summary>
    /// <value> The resource name selection timeout. </value>
	public TimeSpan ResourceNameSelectionTimeout
    {
        get => this._resourceNameSelectionTimeout;
        set => this.SetProperty( ref this._resourceNameSelectionTimeout, value );
    }

    private Microsoft.Extensions.Logging.LogLevel _traceLevel = Microsoft.Extensions.Logging.LogLevel.Trace;

    /// <summary>   Gets or sets the trace level. </summary>
    /// <value> The trace level. </value>
	public Microsoft.Extensions.Logging.LogLevel TraceLevel
    {
        get => this._traceLevel;
        set => this.SetProperty( ref this._traceLevel, value );
    }

    private string _closedCaption = "closed";

    /// <summary>   Gets or sets the closed caption. </summary>
    /// <value> The closed caption. </value>
	[Description( "The caption to display when the resource is closed" )]
    public string ClosedCaption
    {
        get => this._closedCaption;
        set => this.SetProperty( ref this._closedCaption, value );
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
