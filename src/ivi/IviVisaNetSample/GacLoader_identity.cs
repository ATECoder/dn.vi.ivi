using System.Reflection;
using Ivi.Visa;

namespace Ivi.VisaNet;
public static partial class GacLoader
{
    /// <summary>   Queries the instrument identity string. </summary>
    /// <remarks>   2025-08-12. </remarks>
    /// <exception cref="ArgumentException">            Thrown when one or more arguments have
    ///                                                 unsupported or illegal values. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="verbose">      (Optional) True to verbose. </param>
    /// <returns>   The identity. </returns>
    public static string QueryIdentity( string resourceName, bool verbose = false )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) )
            throw new ArgumentException( $"{nameof( resourceName )} cannot be null or empty.", nameof( resourceName ) );
        // Connect to the instrument.
        if ( verbose ) Console.WriteLine( $"Opening a VISA session to '{resourceName}' by:" );
        if ( verbose ) Console.WriteLine( $"\tIvi.Visa.{nameof( GlobalResourceManager )}.{nameof( GlobalResourceManager.ImplementationVersion )}:{GlobalResourceManager.ImplementationVersion}" );
        if ( verbose ) Console.WriteLine( $"\tIvi.Visa.{nameof( GlobalResourceManager )}.{nameof( GlobalResourceManager.SpecificationVersion )}:{GlobalResourceManager.SpecificationVersion}" );
        using Ivi.Visa.IVisaSession visaSession = Ivi.Visa.GlobalResourceManager.Open( resourceName, Ivi.Visa.AccessModes.ExclusiveLock, 2000 )
            ?? throw new InvalidOperationException( $"\tFailed to open VISA session for resource '{resourceName}'." );
        if ( verbose ) Console.WriteLine( $"\tVISA Session open by {visaSession.ResourceManufacturerName} VISA.NET Implementation version {visaSession.ResourceImplementationVersion}" );
        if ( visaSession is Ivi.Visa.IMessageBasedSession messageBasedSession )
        {
            // Ensure termination character is enabled as here in example we use a SOCKET connection.
            messageBasedSession.TerminationCharacterEnabled = true;
            // Request information about an instrument.
            if ( verbose ) Console.WriteLine( "\tReading instrument identification string..." );
            messageBasedSession.FormattedIO.WriteLine( "\t*IDN?" );
            return messageBasedSession.FormattedIO.ReadLine();
        }
        else
        {
            throw new InvalidOperationException( "Not a message-based session." );
        }
    }

    /// <summary>   Attempts to query identity. </summary>
    /// <remarks>   2025-08-14. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="identity">     [out] The identity. </param>
    /// <param name="verbose">      (Optional) True to verbose. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryQueryIdentity( string resourceName, out string identity, bool verbose = false )
    {
        identity = string.Empty;
        try
        {
            Assembly? visaNetSharedComponentsAssembly = GacLoader.GetVisaNetShareComponentsAssembly();
            Version? visaNetSharedComponentsVersion = visaNetSharedComponentsAssembly?.GetName().Version;

            try
            {
                identity = GacLoader.QueryIdentity( resourceName, verbose );
                return true;
            }
            catch ( Exception exception )
            {
                if ( exception is TypeInitializationException && exception.InnerException is DllNotFoundException )
                {
                    // VISA Shared Components is not installed.
                    Console.WriteLine( $"VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} not found. Please install corresponding vendor-specific VISA implementation first." );
                }
                else if ( exception is VisaException && exception.Message == "No vendor-specific VISA .NET implementation is installed." )
                {
                    // Vendor-specific VISA.NET implementation is not available.
                    Console.WriteLine( $"VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} not found. Please install corresponding vendor-specific VISA implementation first." );
                }
                else if ( exception is EntryPointNotFoundException )
                {
                    // Installed VISA Shared Components are not compatible with VISA.NET Shared Components.
                    Console.WriteLine( $"Installed VISA Shared Components version {visaNetSharedComponentsVersion} does not support VISA.NET. Please upgrade VISA implementation." );
                }
                else
                {
                    // Handle remaining errors.
                    Console.WriteLine( $"Exception: {exception.Message}" );
                }
            }

        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"Failed getting Visa.NET Shared Components Version;\n{ex.Message}." );
        }
        return false;
    }
}
