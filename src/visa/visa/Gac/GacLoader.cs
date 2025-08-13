using System.Net;
using Ivi.Visa;
using Ivi.Visa.ConflictManager;

namespace cc.isr.Visa.Gac;
/// <summary>
/// Class used to load .NET Framework assemblies located in GAC from .NET 5+
/// Required only for experimental using VISA.NET library in .NET 5+
/// </summary>
public static class GacLoader
{
    /// <summary>   (Immutable) the visa configuration management assembly name. </summary>
    public const string VisaConfigManagerFileName = "visaConfMgr.dll";

    /// <summary>   Load an assembly from the GAC. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <exception cref="FileNotFoundException">    . </exception>
    /// <param name="assemblyName"> The <see cref="System.Reflection.AssemblyName"/>. </param>
    /// <returns>   Loaded assembly. </returns>
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
    private static bool Contains( this string str, string substring, StringComparison comp )
    {
        if ( substring is null ) throw new ArgumentNullException( nameof( substring ), $"{nameof( substring )} cannot be null." );
        else if ( !Enum.IsDefined( typeof( StringComparison ), comp ) )
            throw new ArgumentException( $"{nameof( comp )} is not a member of {nameof( StringComparison )}", nameof( comp ) );
        return str.IndexOf( substring, comp ) >= 0;
    }

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
            _ = hexBuilder.Append( $"{b:x2}" ); // X2 ensures two-digit representation
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

    /// <summary>   Gets visa net share components version. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <returns>   The visa net share components version. </returns>
    public static System.Version? GetVisaNetShareComponentsVersion()
    {
        System.Reflection.Assembly? assembly = GacLoader.GetVisaNetShareComponentsAssembly();
        return assembly?.GetName().Version;
    }

    /// <summary>   Gets visa shared components information. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <returns>   The visa shared components information. </returns>
    public static System.Diagnostics.FileVersionInfo? VisaConfigManagerFileVersionInfo()
    {
        // Get an available version of the VISA Shared Components.
        return System.Diagnostics.FileVersionInfo.GetVersionInfo( System.IO.Path.Combine( Environment.SystemDirectory, VisaConfigManagerFileName ) );
    }

    /// <summary>   Static constructor. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <exception cref="System.IO.IOException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    public static System.Version? VerifyVisaImplementationPresence()
    {
        // get the shared components version.
        System.Version? visaNetSharedComponentsVersion = new();
        try
        {
            visaNetSharedComponentsVersion = GacLoader.GetVisaNetShareComponentsVersion();
            Console.WriteLine( $"{nameof( GacLoader )}: VISA.NET Shared Components version {visaNetSharedComponentsVersion}." );
        }
        catch ( Exception ex )
        {
            throw new System.IO.IOException( $"Failed locating VISA NET shared components containing the {nameof( Ivi.Visa.GlobalResourceManager )} type.", ex );
        }

        // Check whether VISA Shared Components is installed before using VISA.NET.
        // If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
        // be thrown during termination process due to a bug in the implementation of the
        // VISA.NET Shared Components, and the application will crash.
        try
        {
            // Get an available version of the VISA Shared Components.
            System.Diagnostics.FileVersionInfo? visaSharedComponentsInfo = GacLoader.VisaConfigManagerFileVersionInfo();

            if ( visaSharedComponentsInfo is not null )
                Console.WriteLine( $"{nameof( GacLoader )}: VISA Shared Components version {visaSharedComponentsInfo.ProductVersion} detected." );
            else
                throw new System.IO.IOException( $"Failed getting the VISA shared component {GacLoader.VisaConfigManagerFileName} info." );
        }
        catch ( System.IO.FileNotFoundException ex )
        {
            Console.WriteLine();
            throw new System.IO.IOException( $"VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} not found. Please install corresponding vendor-specific VISA implementation first.",
                ex );
        }

        catch ( System.IO.IOException )
        {
            throw;
        }

        return visaNetSharedComponentsVersion;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this object has dot net implementations.
    /// </summary>
    /// <value> True if this object has dot net implementations, false if not. </value>
    public static bool? HasDotNetImplementations { get; private set; }

    /// <summary>   Gets or sets the loaded implementation. </summary>
    /// <value> The loaded implementation. </value>
    public static Ivi.Visa.ConflictManager.VisaImplementation? LoadedImplementation { get; private set; }

    /// <summary>   Loads installed visa assemblies. </summary>
    /// <remarks>   2025-08-12. </remarks>
    /// <exception cref="IOException">  Thrown when an I/O failure occurred. </exception>
    /// <param name="verbose">  (Optional) True to verbose. </param>
    public static void LoadInstalledVisaAssemblies( bool verbose = false )
    {
        // skip if already loaded.
        if ( GacLoader.LoadedImplementation is not null ) return;

        System.Collections.Generic.List<Ivi.Visa.ConflictManager.VisaImplementation> installedVisas = new Ivi.Visa.ConflictManager.ConflictManager().GetInstalledVisas( ApiType.DotNet );
        GacLoader.HasDotNetImplementations = installedVisas.Count > 0;
        foreach ( Ivi.Visa.ConflictManager.VisaImplementation visaLibrary in installedVisas )
        {
            try
            {
                // load the installed VISA assembly.
                System.Reflection.Assembly visaAssembly = GacLoader.Load( new System.Reflection.AssemblyName(
                    visaLibrary.Location.Substring( visaLibrary.Location.IndexOf( "," ) + 1 ) ) );
                if ( verbose )
                    Console.WriteLine( $"Loaded {visaAssembly.FullName}, {System.Diagnostics.FileVersionInfo.GetVersionInfo( visaAssembly.Location ).FileVersion}" );
                GacLoader.LoadedImplementation = visaLibrary;
            }
            catch ( Exception exception )
            {
                throw new System.IO.IOException( $"Failed to load assembly '{visaLibrary.FriendlyName}': {exception.Message}", exception );
            }
        }
    }

    /// <summary>   Queries the instrument identity. </summary>
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
        if ( verbose ) Console.WriteLine( $"Opening a VISA session to '{resourceName}'..." );
        using IVisaSession resource = Ivi.Visa.GlobalResourceManager.Open( resourceName, AccessModes.ExclusiveLock, 2000 );
        if ( resource is IMessageBasedSession session )
        {
            // Ensure termination character is enabled as here in example we use a SOCKET connection.
            session.TerminationCharacterEnabled = true;
            // Request information about an instrument.
            if ( verbose ) Console.WriteLine( "\tReading instrument identification string..." );
            session.FormattedIO.WriteLine( "*IDN?" );
            return session.FormattedIO.ReadLine();
        }
        else
        {
            throw new InvalidOperationException( "Not a message-based session." );
        }
    }

    /// <summary>   Attempts to ping. </summary>
    /// <remarks>   2025-08-12. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="details">      [out] The details. </param>
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
                details = $"Instrument resource name is not TCPIP '{resourceName}'.";
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
