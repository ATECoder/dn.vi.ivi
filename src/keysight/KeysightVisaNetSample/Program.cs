using Ivi.Visa;
using Ivi.VisaNet;
using System.Reflection;
using System.Runtime.Versioning;

string resourceName = args[0];

Console.Write( $"\nTurn on the instrument at {resourceName} and press any key »" );
_ = Console.ReadKey();

TargetFrameworkAttribute? framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>();
Console.WriteLine();
System.Reflection.Assembly? executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
Console.WriteLine( executingAssembly?.FullName );
Console.WriteLine( $"\tRunning under {framework?.FrameworkName
    ?? "--unable to resolve .NET Framework!"} runtime {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );

// No, there is no direct API in .NET to retrieve the `<supportedRuntime>` value from `App.config` at runtime.
// The .NET runtime uses this setting only during application startup to select the appropriate CLR version.
// Once the process starts, you can only query the runtime version that is actually loaded.
//
// To get the loaded runtime version, use `System.Environment.Version` or `System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription`:
//
// ```c#
// Console.WriteLine( System.Environment.Version );
// Console.WriteLine( System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription );
// ```
//
// These will show the actual runtime in use, not the value from `App.config`. If you need the config value, you must manually parse `App.config` as an XML file.

Console.WriteLine( "Runtime Information:" );
Console.WriteLine( $"\tFramework Description: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );
Console.WriteLine( $"\t      OS Architecture: {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture}" );
Console.WriteLine( $"\t       OS Description: {System.Runtime.InteropServices.RuntimeInformation.OSDescription} (is Windows 11 if build >= 22000)" );
Console.WriteLine( $"\t Process Architecture: {System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture}" );
#if NET5_0_OR_GREATER
Console.WriteLine( $"\t   Runtime Identifier: {System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier}" );
#endif

// Check whether VISA Shared Components is installed before using VISA.NET.
// If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
// be thrown during termination process due to a bug in the implementation of the
// VISA.NET Shared Components, and the application will crash.
Assembly? visaNetSharedComponentsAssembly = GacLoader.GetVisaNetShareComponentsAssembly();

// Get VISA.NET Shared Components version.
Version? visaNetSharedComponentsVersion = visaNetSharedComponentsAssembly?.GetName().Version;
Console.WriteLine();
if ( visaNetSharedComponentsAssembly is null )
{
    Console.WriteLine( $"VISA.NET Shared Components assembly not found. Please install corresponding vendor-specific VISA implementation first." );
}
else
{
    Console.WriteLine( $"VISA.NET Shared Components {visaNetSharedComponentsAssembly.GetName()}." );
    Console.WriteLine( $"\tVersion: {System.Diagnostics.FileVersionInfo.GetVersionInfo( typeof( GlobalResourceManager ).Assembly.Location ).FileVersion}." );
}

try
{
    // Get the version of the VISA Configuration Manager File Version Info.
    System.Diagnostics.FileVersionInfo? visaConfigManagerFileVersionInfo = Ivi.VisaNet.GacLoader.VisaConfigManagerFileVersionInfo();
    if ( visaConfigManagerFileVersionInfo is not null )
        Console.WriteLine( $"\t{visaConfigManagerFileVersionInfo.InternalName} version {visaConfigManagerFileVersionInfo.ProductVersion} detected." );
    else
        Console.WriteLine( $"\tFailed getting the VISA Config Manager {Ivi.VisaNet.GacLoader.VisaConfigManagerFileName} info." );
}
catch ( FileNotFoundException )
{
    Console.WriteLine();
    Console.WriteLine( $"VISA Config Manager {Ivi.VisaNet.GacLoader.VisaConfigManagerFileName} not found. Please install a vendor-specific VISA implementation." );
    return;
}

if ( Ivi.VisaNet.GacLoader.TryLoadInstalledVisaAssemblies( out string details ) is IList<Assembly> installedAssemblies && installedAssemblies.Count > 0 )
{
    int count = installedAssemblies.Count;
    if ( count > 1 )
        Console.WriteLine( $"\nLoaded multiple ({count}) VISA .NET implementation assemblies:\n\t{details}" );
    else
        Console.WriteLine( $"\nLoaded VISA .NET implementation assembly:\n\t{details}" );
    // foreach ( Assembly assembly in assemblies )
    // {
    //     Console.WriteLine( $"\t{assembly.FullName}, {System.Diagnostics.FileVersionInfo.GetVersionInfo( assembly.Location ).FileVersion}" );
    // }
}
else
{
    Console.WriteLine( $"\nNo VISA .NET implementation assemblies loaded:\n\t{details}" );
    return;
}

if ( installedAssemblies is not null && installedAssemblies.Any() )
{
    foreach ( Assembly installedAssembly in installedAssemblies )
    {
        // Version? installedVersion = installedAssembly.GetName().Version;
        // Console.WriteLine( $"\tLoaded {installedAssembly.FullName}." );
        // Console.WriteLine( $"\tVersion: {System.Diagnostics.FileVersionInfo.GetVersionInfo( installedAssembly.Location ).FileVersion}." );

        if ( !string.IsNullOrWhiteSpace( resourceName ) )
        {
            Console.WriteLine( $"\nPinging '{resourceName}'..." );

            if ( Ivi.VisaNet.GacLoader.TryPing( resourceName, out details ) )
            {
                Console.WriteLine();
                Console.WriteLine( Ivi.VisaNet.GacLoader.IdentifyVisaSession( installedAssembly, resourceName ) );

                Console.WriteLine( $"Reading '{resourceName}' identity..." );
                if ( Ivi.VisaNet.GacLoader.TryQueryIdentity( resourceName, out string identity, true ) )
                    Console.WriteLine( $"\tVISA resource '{resourceName}' identified as:\n\t{identity}" );
                else
                    Console.WriteLine( $"Failed to identify VISA resource '{resourceName}'." );
            }
            else
                Console.WriteLine( details );
        }
        else
        {
            Console.WriteLine( $"{nameof( resourceName )} is empty." );
        }

    }
}

Console.Write( "\nPress any key to finish »" );
_ = Console.ReadKey();
