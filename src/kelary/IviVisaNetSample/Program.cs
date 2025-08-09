using Ivi.Visa;
using Ivi.VisaNet;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

#nullable enable

string resourceName = args[0];

Console.WriteLine();
Console.WriteLine( $"Make sure that the instrument at {resourceName} is turned on." );

TargetFrameworkAttribute? framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>();
Console.WriteLine();
System.Reflection.Assembly? executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
Console.WriteLine( executingAssembly?.FullName );
Console.WriteLine( $"\tRunning under {framework?.FrameworkName ?? "--unable to resolve .NET Framework!"} runtime {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );

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

// Get VISA.NET Shared Components version.
Assembly? visaNetSharedComponentsAssembly = GacLoader.GetVisaNetShareComponentsAssembly();
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

// Check whether VISA Shared Components is installed before using VISA.NET.
// If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
// be thrown during termination process due to a bug in the implementation of the
// VISA.NET Shared Components, and the application will crash.
try
{
    // Get an available version of the VISA Shared Components.
    System.Diagnostics.FileVersionInfo? visaSharedComponentsInfo = GacLoader.VisaConfigManagerFileVersionInfo();
    if ( visaSharedComponentsInfo is not null )
        Console.WriteLine( $"\t{visaSharedComponentsInfo.InternalName} version {visaSharedComponentsInfo.ProductVersion} detected." );
    else
        Console.WriteLine( $"\tFailed getting the VISA shared component {GacLoader.VisaConfigManagerFileName} info." );
}
catch ( FileNotFoundException )
{
    Console.WriteLine();
    Console.WriteLine( $"VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} not found. Please install corresponding vendor-specific VISA implementation first." );
    return;
}

// #if NET5_0_OR_GREATER
Console.WriteLine();
// Preloading installed VISA implementation assemblies for NET 5+
Ivi.VisaNet.GacLoader.LoadInstalledVisaAssemblies();
// #endif

try
{
    // Connect to the instrument.
    Console.WriteLine();
    Console.WriteLine( $"Opening a VISA session to '{resourceName}'..." );
    using IVisaSession resource = Ivi.Visa.GlobalResourceManager.Open( resourceName, AccessModes.ExclusiveLock, 2000 );
    if ( resource is IMessageBasedSession session )
    {
        // Ensure termination character is enabled as here in example we use a SOCKET connection.
        session.TerminationCharacterEnabled = true;

        // Request information about an instrument.
        Console.WriteLine();
        Console.WriteLine( "Reading instrument identification string..." );
        session.FormattedIO.WriteLine( "*IDN?" );
        string instrumentInfo = session.FormattedIO.ReadLine();
        Console.WriteLine();
        Console.WriteLine( $"{resourceName} Identification string:\n\n{instrumentInfo}" );
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

Console.WriteLine();
Console.WriteLine( "Press any key to finish." );
_ = Console.ReadKey();
