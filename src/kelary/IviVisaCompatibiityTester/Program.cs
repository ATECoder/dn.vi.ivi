using Ivi.Visa;
using Ivi.VisaNet;
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
    visaNetSharedComponentsVersion = GacLoader.VerifyVisaImplementationPresence( true );
}
catch ( System.IO.IOException ex )
{
    Console.WriteLine();
    Console.WriteLine( ex.ToString() );
    return;
}

// Preload installed VISA implementation assemblies
Ivi.VisaNet.GacLoader.LoadInstalledVisaAssemblies( true );

if ( !string.IsNullOrWhiteSpace( resourceName ) )
{
    if ( GacLoader.TryPing( resourceName, out string details ) )
        QueryIdentity();
    else
        Console.WriteLine( details );
}

Console.WriteLine();
Console.WriteLine( "Press any key to finish." );
_ = Console.ReadKey();

void QueryIdentity()
{
    try
    {
        Console.WriteLine( $"ID: {GacLoader.QueryIdentity( resourceName, true )} " );
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

