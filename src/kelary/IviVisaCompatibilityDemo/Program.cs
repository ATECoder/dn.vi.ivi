using Ivi.Visa;
using Ivi.VisaNet;
using System.Reflection;
using System.Runtime.Versioning;

string resourceName = args[0];

Console.WriteLine();
Console.WriteLine( $"Make sure that the instrument at {resourceName} is turned on." );

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

Console.WriteLine( $"\nReading '{resourceName}' identity..." );
if ( GacLoader.TryQueryIdentity( resourceName, out string? identity, true ) )
{
    Console.WriteLine( $"\tVISA resource '{resourceName}' identified as:\n\t{identity}" );
}
else
{
    Console.WriteLine( $"Failed to identify VISA resource '{resourceName}'." );
}

Console.WriteLine();
Console.WriteLine( "Press any key to finish." );
_ = Console.ReadKey();

