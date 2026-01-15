using System.Reflection;
using System.Runtime.Versioning;

string resourceName = args[0];

Console.Write( $"\nTurn on the instrument at {resourceName} and press any key »" );
_ = Console.ReadKey();
Console.WriteLine();

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

Console.WriteLine( "\nRuntime Information:" );
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

System.Reflection.Assembly? loadedAssembly = Ivi.VisaNet.GacLoader.TryVerifyVisaImplementation( out string details );

if ( loadedAssembly is null )
    Console.WriteLine( $"\n*** VISA implementation verification failed.\n{details}" );
else
{
    Console.WriteLine( $"\nVISA implementation verified successfully.\n\n{details}" );

    try
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) )
        {
            Console.WriteLine( $"\n*** {nameof( resourceName )} cannot be null or empty." );
        }
        else
        {
            using Ivi.Visa.IVisaSession? session = Ivi.VisaNet.GacLoader.TryOpenSession( resourceName, out details );
            Console.WriteLine( details );
            if ( session is not null )
            {
                Console.WriteLine( $"Reading instrument identity..." );
                string identity = Ivi.VisaNet.GacLoader.TryQueryIdentity( session, out details );
                if ( string.IsNullOrWhiteSpace( identity ) )
                    Console.WriteLine( $"\t*** Failed to identify VISA resource '{resourceName}'.\n{details}" );
                else
                    Console.WriteLine( $"\tResource: {resourceName}\n\tIdentity: {identity}" );

                Console.WriteLine( Ivi.VisaNet.GacLoader.BuildSessionInterfaceImplementationReport( session ) );

                // if ( loadedAssembly is null )
                // {
                //     loadedAssembly = GacLoader.TryFindLoadedImplementation( out details );
                //     if ( loadedAssembly is null )
                //         Console.WriteLine( $"\n*** {details}" );
                //     else
                //         Console.WriteLine( details );
                // }
                // if ( loadedAssembly is not null )

                Console.WriteLine( Ivi.VisaNet.GacLoader.BuildSessionTypesReport( loadedAssembly, session ) );

                Console.WriteLine( $"Closing session to '{resourceName}'..." );
            }
        }
    }
    catch ( FileNotFoundException )
    {
        Console.WriteLine( $"\n*** VISA Config Manager {Ivi.VisaNet.GacLoader.VisaConfigManagerFileName} not found. Please install a vendor-specific VISA implementation." );
    }
}

Console.Write( "\nPress any key to finish »" );
_ = Console.ReadKey();
