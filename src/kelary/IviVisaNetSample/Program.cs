using Ivi.Visa;
using System;
using System.Diagnostics;
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
Console.WriteLine( $"Running under {framework?.FrameworkName}." );

// Get VISA.NET Shared Components version.
Version? visaNetSharedComponentsVersion = typeof( GlobalResourceManager ).Assembly.GetName().Version;
Console.WriteLine();
Console.WriteLine( $"VISA.NET Shared Components version {visaNetSharedComponentsVersion}." );

// Check whether VISA Shared Components is installed before using VISA.NET.
// If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
// be thrown during termination process due to a bug in the implementation of the
// VISA.NET Shared Components, and the application will crash.
try
{
    // Get an available version of the VISA Shared Components.
    FileVersionInfo visaSharedComponentsInfo = FileVersionInfo.GetVersionInfo( Path.Combine( Environment.SystemDirectory, "visaConfMgr.dll" ) );
    Console.WriteLine( $"VISA Shared Components version {visaSharedComponentsInfo.ProductVersion} detected." );
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
    using IVisaSession resource = GlobalResourceManager.Open( resourceName, AccessModes.ExclusiveLock, 2000 );
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
