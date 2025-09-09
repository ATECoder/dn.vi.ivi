using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Ivi.Visa;
using Ivi.Visa.ConflictManager;

#pragma warning disable CA1305
#pragma warning disable IDE0150

#nullable enable

namespace Ivi.VisaNet;
/// <summary>
/// Class used to load .NET Framework assemblies located in GAC from .NET 5+
/// Required only for experimental using VISA.NET library in .NET 5+
/// </summary>
public static class GacLoader
{
    /// <summary>   (Immutable) the visa configuration management assembly name. </summary>
    public const string VisaConfigManagerFileName = "visaConfMgr.dll";

    /// <summary>
    /// Load an assembly from the GAC.
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns>Loaded assembly</returns>
    /// <exception cref="FileNotFoundException"></exception>
    private static System.Reflection.Assembly Load( System.Reflection.AssemblyName assemblyName )
    {
        string[] gacPaths =
        [
           $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\Microsoft.NET\\assembly\\GAC_MSIL\\{assemblyName.Name}",
           $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\assembly\\GAC_MSIL\\{assemblyName.Name}",
        ];

        foreach ( string folder in gacPaths.Where( System.IO.Directory.Exists ) )
        {
            foreach ( string subfolder in System.IO.Directory.EnumerateDirectories( folder ) )
            {
                if ( subfolder.Contains( BytesToHex( assemblyName.GetPublicKeyToken()! ), StringComparison.OrdinalIgnoreCase )
                    && subfolder.Contains( assemblyName.Version!.ToString(), StringComparison.OrdinalIgnoreCase ) )
                {
                    string assemblyPath = System.IO.Path.Combine( subfolder, assemblyName.Name + ".dll" );
                    if ( System.IO.File.Exists( assemblyPath ) )
                        return System.Reflection.Assembly.LoadFrom( assemblyPath );
                }
            }
        }
        throw new System.IO.FileNotFoundException( $"Assembly {assemblyName} not found." );
    }

    /// <summary>
    /// A <see cref="string" /> extension method that query if this String contains the given str.
    /// </summary>
    /// <remarks>   2023-04-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="str">          The string to act on. </param>
    /// <param name="substring">    The substring. </param>
    /// <param name="comp">         The string comparison option. </param>
    /// <returns>   True if the substring is contained in the String, false if not. </returns>
#if NET20_OR_GREATER
    private static bool Contains( this string str, string substring, StringComparison comp )
    {
        if ( substring is null ) throw new ArgumentNullException( nameof( substring ), $"{nameof( substring )} cannot be null." );
        else if ( !Enum.IsDefined( typeof( StringComparison ), comp ) )
            throw new ArgumentException( $"{nameof( comp )} is not a member of {nameof( StringComparison )}", nameof( comp ) );
        return str.IndexOf( substring, comp ) >= 0;
    }
#endif

    /// <summary>   Bytes to hexadecimal. </summary>
    /// <remarks>   2024-07-01. </remarks>
    /// <param name="byteArray">    Array of bytes. </param>
    /// <returns>   A string. </returns>
    private static string BytesToHex( byte[] byteArray )
    {
        // Convert byte array to hexadecimal string
        System.Text.StringBuilder hexBuilder = new();
        foreach ( byte b in byteArray )
        {
#if NET5_0_OR_GREATER
            _ = hexBuilder.Append( System.Globalization.CultureInfo.CurrentCulture, $"{b:x2}" ); // X2 ensures two-digit representation
#else
            _ = hexBuilder.Append( $"{b:x2}" ); // X2 ensures two-digit representation
#endif
        }
        return hexBuilder.ToString();
    }

    private static System.Reflection.Assembly? _visaNetShareComponentsAssembly;
    /// <summary>   Gets visa net share components assembly. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <returns>   The visa net share components version. </returns>
    public static System.Reflection.Assembly? GetVisaNetShareComponentsAssembly()
    {
        _visaNetShareComponentsAssembly ??= typeof( Ivi.Visa.GlobalResourceManager ).Assembly;
        return _visaNetShareComponentsAssembly;
    }

    /// <summary>   Gets the visa configuration manager file version info. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <returns>   The visa configuration manager file version info. </returns>
    public static System.Diagnostics.FileVersionInfo? VisaConfigManagerFileVersionInfo()
    {
        return System.Diagnostics.FileVersionInfo.GetVersionInfo( System.IO.Path.Combine( Environment.SystemDirectory, GacLoader.VisaConfigManagerFileName ) );
    }

    /// <summary>
    /// Preloading installed VISA implementation assemblies for NET 5+
    /// </summary>
    public static void LoadInstalledVisaAssemblies()
    {
        System.Collections.Generic.List<VisaImplementation> installedVisas = new ConflictManager().GetInstalledVisas( ApiType.DotNet );
        foreach ( Ivi.Visa.ConflictManager.VisaImplementation visaLibrary in installedVisas )
        {
            try
            {
                string fileName =
#if NET20_OR_GREATER
                visaLibrary.Location.Substring( visaLibrary.Location.IndexOf( ',' ) + 1 );
#else
                visaLibrary.Location[(visaLibrary.Location.IndexOf( ',' ) + 1)..];
#endif
                System.Reflection.Assembly installedAssembly = GacLoader.Load( new System.Reflection.AssemblyName( fileName ) );
                Console.WriteLine( $"Loaded {installedAssembly.FullName}, {System.Diagnostics.FileVersionInfo.GetVersionInfo( installedAssembly.Location ).FileVersion}" );
            }
            catch ( Exception exception )
            {
                Console.WriteLine( $"Failed to load assembly {visaLibrary.FriendlyName}: {exception.Message}" );
            }
        }
    }

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

    /// <summary>   Query if 'typeName' exists in the 'Ivi.Visa' assembly. </summary>
    /// <remarks>   2025-08-18. </remarks>
    /// <exception cref="ArgumentException">            Thrown when one or more arguments have
    ///                                                 unsupported or illegal values. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="typeName"> Name of the type. </param>
    /// <returns>   True if type exists, false if not. </returns>
    public static bool IsTypeExists( string typeName )
    {
        if ( string.IsNullOrWhiteSpace( typeName ) )
            throw new ArgumentException( $"{nameof( typeName )} cannot be null or empty.", nameof( typeName ) );
        System.Reflection.Assembly? assembly = GacLoader.GetVisaNetShareComponentsAssembly()
            ?? throw new InvalidOperationException( "VISA.NET Shared Components assembly is not loaded." );
        _ = System.Reflection.Assembly.LoadFrom( assembly.Location );
        Type? type = assembly.GetType( typeName, false, true );
        return type is not null;
    }

    /// <summary>   Query if the specific interface is implemented in 'visaSession' . </summary>
    /// <remarks>   2025-08-18. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="ArgumentException">            Thrown when one or more arguments have
    ///                                                 unsupported or illegal values. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="visaSession">      The visa session. </param>
    /// <param name="interfaceName">    Name of the interface. </param>
    /// <returns>   True if interface implemented, false if not. </returns>
    [CLSCompliant( false )]
    public static bool IsInterfaceImplemented( Ivi.Visa.IVisaSession visaSession, string interfaceName )
    {
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( interfaceName ) ) throw new ArgumentException( $"{nameof( interfaceName )} cannot be null or empty.", nameof( interfaceName ) );
        if ( GacLoader.IsTypeExists( interfaceName ) )
        {
            return visaSession.GetType().GetInterfaces().Any( i => i.FullName == interfaceName || i.Name == interfaceName );
        }
        else
            throw new InvalidOperationException( $"The type '{interfaceName}' does not exist in the namespace '{nameof( Ivi )}.{nameof( Ivi.Visa )}'." );
    }

    /// <summary>   Validates the visa session interface. </summary>
    /// <remarks>   2025-08-18. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="visaSession">      The visa session. </param>
    /// <param name="interfaceName">    Name of the interface. </param>
    /// <returns>   A string. </returns>
    [CLSCompliant( false )]
    public static string ValidateVisaSessionInterface( Ivi.Visa.IVisaSession visaSession, string interfaceName )
    {
        StringBuilder sb = new();
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( interfaceName ) ) throw new ArgumentException( $"{nameof( interfaceName )} cannot be null or empty.", nameof( interfaceName ) );
        if ( GacLoader.IsTypeExists( interfaceName ) )
        {
            // The type exists in the assembly, so we can use it.
            _ = sb.Append( visaSession.GetType().GetInterfaces().Any( interfaceType => interfaceType.FullName == interfaceName || interfaceType.Name == interfaceName )
                ? $"\tis"
                : $"\tis not" );
            _ = sb.AppendLine( $" a '{interfaceName}'." );
        }
        else
        {
            // The type does not exist in the assembly.
            _ = sb.AppendLine( $"\tThe type '{interfaceName}' does not exist in the namespace '{nameof( Ivi )}.{nameof( Ivi.Visa )}'." );
        }
        return sb.ToString();
    }

    /// <summary>   Identify visa session. </summary>
    /// <remarks>   2025-08-13. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   A string. </returns>
    public static string IdentifyVisaSession( string resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) )
            return $"{nameof( resourceName )} is null or empty or white space.";

        System.Text.StringBuilder sb = new();
        _ = sb.AppendLine( $"Identifying '{resourceName}' session implementations by type casting:" );

        using Ivi.Visa.IVisaSession visaSession = Ivi.Visa.GlobalResourceManager.Open( resourceName, Ivi.Visa.AccessModes.ExclusiveLock, 2000 );

        if ( visaSession is Ivi.Visa.IMessageBasedSession )
        {
            _ = sb.AppendLine( $"\tis a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.IMessageBasedSession )}'." );

            _ = sb.Append( visaSession is Keysight.Visa.MessageBasedSession
                ? $"\tis"
                : $"\tis not" );
            _ = sb.AppendLine( $" a '{nameof( Keysight )}.{nameof( Keysight.Visa )}.{nameof( Keysight.Visa.MessageBasedSession )}'." );

            if ( resourceName.StartsWith( "TCPIP", StringComparison.OrdinalIgnoreCase ) )
            {

                if ( visaSession is Ivi.Visa.IMessageBasedSession )
                {
                    _ = sb.Append( visaSession is Keysight.Visa.TcpipSession
                        ? $"\tis"
                        : $"\tis not" );
                    _ = sb.AppendLine( $" a '{nameof( Keysight )}.{nameof( Keysight.Visa )}.{nameof( Keysight.Visa.TcpipSession )}'." );

                    _ = sb.Append( visaSession is Ivi.Visa.IVisaSession
                        ? $"\tis"
                        : $"\tis not" );
                    _ = sb.AppendLine( $" a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.IVisaSession )}'." );

                    _ = sb.Append( visaSession is Ivi.Visa.ITcpipSession
                        ? $"\tis"
                        : $"\tis not" );
                    _ = sb.AppendLine( $" a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.ITcpipSession )}'." );

#if NET9_0_OR_GREATER
                    _ = sb.AppendLine( $"\nIdentifying '{resourceName}' session implementations by type names:" );

                    string typeName = "Ivi.Visa.ITcpipSession";
                    try
                    {
                        _ = sb.Append( GacLoader.ValidateVisaSessionInterface( visaSession, typeName ) );
                    }
                    catch ( Exception ex )
                    {
                        _ = sb.AppendLine( $"\tException handling 'typeName':\n\t\t{ex.Message}." );
                    }

                    try
                    {
                        typeName = "Ivi.Visa.ITcpipSession2";
                        _ = sb.Append( GacLoader.ValidateVisaSessionInterface( visaSession, typeName ) );
                    }
                    catch ( Exception ex )
                    {
                        _ = sb.AppendLine( $"\tException handling 'typeName':\n\t\t{ex.Message}." );
                    }
#else
                    try
                    {
                        _ = sb.Append( visaSession is Ivi.Visa.ITcpipSession2
                            ? $"\tis"
                            : $"\tis not" );
                        _ = sb.AppendLine( $" a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.ITcpipSession2 )}'." );
                    }
                    catch ( Exception ex )
                    {
                        _ = sb.AppendLine( $" Exception handling '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.ITcpipSession2 )}'; {ex.Message}." );
                    }
#endif
                }
                else
                {
                    _ = sb.AppendLine( $"\tis not a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.IMessageBasedSession )}'." );
                }
            }
            else
            {
                _ = sb.AppendLine( $"\tResource '{resourceName}' is not a TCPIP resource." );
            }
        }
        else
            _ = sb.AppendLine( $"\tis not a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.IMessageBasedSession )}'." );

        return sb.ToString();

    }

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
