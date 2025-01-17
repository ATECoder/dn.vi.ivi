namespace cc.isr.VI.Pith;
/// <summary>   Information about the visa resource. </summary>
/// <remarks>   2024-10-19. </remarks>
/// <param name="resourceName"> Name of the resource. </param>
public class VisaResourceInfo( string resourceName )
{
    /// <summary>   Gets or sets the name of the VISA resource. </summary>
    /// <value> The name of the VISA resource. </value>
    public string ResourceName { get; private set; } = resourceName;

    #region " TCP/IP Visa Resource "

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
