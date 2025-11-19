namespace cc.isr.VI.Pith;

/// <summary>   Additional information for service request events. </summary>
/// <remarks>   2024-09-02. </remarks>
/// <param name="statusByte">   The status byte. </param>
public class ServiceRequestEventArgs( ServiceRequests statusByte ) : EventArgs
{
    /// <summary>   Gets or sets the status byte. </summary>
    /// <value> The status byte. </value>
    public ServiceRequests StatusByte { get; internal set; } = statusByte;
}
