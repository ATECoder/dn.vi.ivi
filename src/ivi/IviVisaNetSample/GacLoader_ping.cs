using System.Net;

namespace Ivi.VisaNet;
public static partial class GacLoader
{
    /// <summary>   Attempts to ping. </summary>
    /// <remarks>   2025-08-12. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryPing( string resourceName, out string details )
    {
        bool outcome = false;
        try
        {
            System.Net.NetworkInformation.Ping ping = new();
            System.Net.NetworkInformation.PingOptions pingOptions = new( 4, true );
            byte[] buffer = [0, 0];
            if ( resourceName.StartsWith( "TCPIP", StringComparison.OrdinalIgnoreCase ) )
                resourceName = resourceName.Split( ':' )[2];
            if ( IPAddress.TryParse( resourceName, out IPAddress? address ) )
            {
                outcome = ping.Send( address, 1000, buffer, pingOptions ).Status == System.Net.NetworkInformation.IPStatus.Success;
                details = outcome
                    ? $"Instrument found at '{resourceName}'."
                    : $"Attempt Ping instrument at '{resourceName}' failed.";
            }
            else
            {
                details = $"Non TCPIP Instrument at '{resourceName}'.";
                outcome = true;
            }
        }
        catch ( Exception )
        {
            details = $"Exception occurred pinging {resourceName}; .";
        }
        return outcome;
    }
}
