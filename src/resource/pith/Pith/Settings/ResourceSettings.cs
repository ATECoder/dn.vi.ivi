using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace cc.isr.VI.Pith.Settings;

/// <summary>   Resource test settings class for a single resource. </summary>
/// <remarks>
/// <para>
/// David, 2018-02-12 </para>
/// </remarks>
[CLSCompliant( false )]
public class ResourceSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    #region " resource name and title "

    private string _resourceName = "TCPIP0::192.168.0.50::INST0::INSTR";

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [Description( "The instrument VISA resource name" )]
    public string ResourceName
    {
        get => this._resourceName;
        set
        {
            if ( this.SetProperty( ref this._resourceName, value ) )
            {
                this._isTcpIpResource = null;
                this._resourcePinged = null;
            }
        }
    }

    private string _resourceModel = "2601A";

    /// <summary>   Gets or sets the resource model that is associated with the specified resource name. </summary>
    /// <value> The resource model. </value>
    [Description( "The the resource models that is associated with the specified resource name [2601A]" )]
    public virtual string ResourceModel
    {
        get => this._resourceModel;
        set => _ = this.SetProperty( ref this._resourceModel, value );
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

    private TimeSpan _resourceNameSelectionTimeout = TimeSpan.FromSeconds( 10 );

    /// <summary>   Gets or sets the resource name selection timeout. </summary>
    /// <value> The resource name selection timeout. </value>
    [Description( "Specifies the time to wait when selecting a resource name [10s]" )]
    public TimeSpan ResourceNameSelectionTimeout
    {
        get => this._resourceNameSelectionTimeout;
        set => _ = this.SetProperty( ref this._resourceNameSelectionTimeout, value );
    }

    #endregion

    #region " resource info "

    private string _language = "SCPI";

    /// <summary> Gets or sets the language. </summary>
    /// <value> The language. </value>
    [Description( "The instrument language model either SCPI or TSP [SCPI]" )]
    public virtual string Language
    {
        get => this._language;
        set => _ = this.SetProperty( ref this._language, value );
    }

    private string _firmwareRevision = "2.2.6";

    /// <summary> Gets or sets the firmware revision. </summary>
    /// <value> The firmware revision. </value>
    [Description( "The firmware revision associated with this resource model [2.2.6]" )]
    public virtual string FirmwareRevision
    {
        get => this._firmwareRevision;
        set => _ = this.SetProperty( ref this._firmwareRevision, value );
    }

    #endregion

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
                this._isTcpIpResource = ResourceNamesManager.IsTcpIpResource( this.ResourceName );
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
                this._resourcePinged = !this.IsTcpIpResource || ResourceNamesManager.PingTcpIpResource( this.ResourceName );
            return this._resourcePinged.Value;
        }
    }

    #endregion
}

