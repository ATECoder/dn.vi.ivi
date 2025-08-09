using Ivi.Visa;
using Ivi.VisaNet;
using System.Net;
using System.Reflection;
using System.Runtime.Versioning;

Console.WriteLine( "Checking IVI VISA Compatibility" );

string resourceName = string.Empty;

if ( args is not null && args.Length > 0 )
{
    Console.Write( "Command: " );
    foreach ( string arg in args )
        Console.Write( $"{arg}; " );
    Console.WriteLine();
    resourceName = args[0];

    Console.WriteLine();
    Console.WriteLine( $"Make sure that the instrument at {resourceName} is turned on." );
}

TargetFrameworkAttribute? framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>();
Console.WriteLine();
System.Reflection.Assembly? executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
Console.WriteLine( executingAssembly?.FullName );
Console.WriteLine( $"\tRunning under {framework?.FrameworkName} runtime {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );

// Check whether VISA Shared Components is installed before using VISA.NET.
// If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
// be thrown during termination process due to a bug in the implementation of the
// VISA.NET Shared Components, and the application will crash.
Version? visaNetSharedComponentsVersion;
try
{
    visaNetSharedComponentsVersion = GacLoader.VerifyVisaImplementationPresence();
}
catch ( System.IO.IOException ex )
{
    Console.WriteLine();
    Console.WriteLine( ex.ToString() );
    return;
}

// Preload installed VISA implementation assemblies
Ivi.VisaNet.GacLoader.LoadInstalledVisaAssemblies();

if ( !string.IsNullOrWhiteSpace( resourceName ) )
{
    if ( TryPing( resourceName ) )
        QueryIdentity();
}

Console.WriteLine();
Console.WriteLine( "Press any key to finish." );
_ = Console.ReadKey();


bool TryPing( string resourceName )
{
    bool outcome;
    try
    {
        System.Net.NetworkInformation.Ping ping = new();
        System.Net.NetworkInformation.PingOptions pingOptions = new( 4, true );
        byte[] buffer = [0, 0];
        if ( resourceName.StartsWith( "TCPIP", StringComparison.OrdinalIgnoreCase ) )
            resourceName = resourceName.Split( ":" )[2];
        if ( IPAddress.TryParse( resourceName, out IPAddress? address ) )
        {
            outcome = ping.Send( address, 1000, buffer, pingOptions ).Status == System.Net.NetworkInformation.IPStatus.Success;
            if ( outcome )
                Console.WriteLine( $"Instrument found at '{resourceName}'." );
            else
                Console.WriteLine( $"Attempt Ping instrument at '{resourceName}' failed." );
        }
        else
        {
            Console.WriteLine( $"Instrument at '{resourceName}'." );
            outcome = true;
        }
    }
    catch ( Exception )
    {
        Console.WriteLine( $"Exception occurred pinging {resourceName}." );
        throw;
    }
    return outcome;
}

void QueryIdentity()
{
    try
    {
        // Connect to the instrument.
        Console.WriteLine();
        Console.WriteLine( $"Opening a VISA session to '{resourceName}'..." );
        using IVisaSession resource = GlobalResourceManager.Open( resourceName, AccessModes.ExclusiveLock, 2000 );
        if ( resource is IMessageBasedSession session )
        {
            // Ensure termination character is enabled as here in example we use a SOCKET connection.
            session.TerminationCharacterEnabled = true;

            // Request information about an instrument.
            session.FormattedIO.WriteLine( "*IDN?" );
            string instrumentInfo = session.FormattedIO.ReadLine();
            Console.WriteLine( $"ID: {instrumentInfo}" );
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine( "Not a message-based session." );
        }
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

