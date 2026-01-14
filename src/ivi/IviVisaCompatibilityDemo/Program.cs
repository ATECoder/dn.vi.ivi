using System.Reflection;
using System.Runtime.Versioning;
using Ivi.Visa;

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
Assembly? visaNetSharedComponentsAssembly = Ivi.VisaNet.GacLoader.GetVisaNetShareComponentsAssembly();

// Get VISA.NET Shared Components version.
Version? visaNetSharedComponentsVersion = visaNetSharedComponentsAssembly?.GetName().Version;
Console.WriteLine();

if ( visaNetSharedComponentsAssembly is null )
{
    Console.WriteLine( $"*** VISA.NET Shared Components assembly not found. Please install a vendor-specific VISA implementation." );
}
else
{
    Console.WriteLine( $"VISA.NET Shared Components {visaNetSharedComponentsAssembly.GetName()}." );
    Console.WriteLine( $"\tVersion: {System.Diagnostics.FileVersionInfo.GetVersionInfo( typeof( GlobalResourceManager ).Assembly.Location ).FileVersion}." );

    try
    {
        // Get the version of the VISA Configuration Manager File Version Info.
        System.Diagnostics.FileVersionInfo? visaConfigManagerFileVersionInfo = Ivi.VisaNet.GacLoader.VisaConfigManagerFileVersionInfo();
        if ( visaConfigManagerFileVersionInfo is not null )
        {
            Console.WriteLine( $"\t{visaConfigManagerFileVersionInfo.InternalName} version {visaConfigManagerFileVersionInfo.ProductVersion} detected." );

            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                Console.WriteLine( $"\n*** {nameof( resourceName )} cannot be null or empty." );
            }
            else
            {
                using Ivi.Visa.IVisaSession? session = Ivi.VisaNet.GacLoader.TryOpenSession( resourceName, out string details );
                Console.WriteLine( details );
                if ( session is not null )
                {
                    Console.WriteLine( $"Reading '{resourceName}' identity..." );
                    string identity = Ivi.VisaNet.GacLoader.TryQueryIdentity( session, out details );
                    if ( string.IsNullOrWhiteSpace( identity ) )
                        Console.WriteLine( $"\t*** Failed to identify VISA resource '{resourceName}'.\n{details}" );
                    else
                        Console.WriteLine( $"\tVISA resource '{resourceName}' identified as:\n\t{identity}" );

                    Console.WriteLine( Ivi.VisaNet.GacLoader.BuildSessionInterfaceImplementationReport( session ) );

                    Console.WriteLine( Ivi.VisaNet.GacLoader.BuildSessionIdentityReport( session ) );

                    Console.WriteLine( $"\nClosing session to '{resourceName}'..." );
                }
            }
        }
        else
            Console.WriteLine( $"\t*** Failed getting the VISA Config Manager {Ivi.VisaNet.GacLoader.VisaConfigManagerFileName} info." );
    }
    catch ( FileNotFoundException )
    {
        Console.WriteLine( $"\n*** VISA Config Manager {Ivi.VisaNet.GacLoader.VisaConfigManagerFileName} not found. Please install a vendor-specific VISA implementation." );
    }
}

Console.Write( "\nPress any key to finish »" );
_ = Console.ReadKey();
