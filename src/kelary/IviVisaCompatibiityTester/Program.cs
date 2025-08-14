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
    {
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

        try
        {
            Console.WriteLine( GacLoader.IdentifyVisaSession( resourceName ) );
        }
        catch ( Exception exception )
        {
            Console.WriteLine( $"Exception: {exception.Message}" );
        }
    }
    else
        Console.WriteLine( details );
}

Console.WriteLine();
Console.WriteLine( "Press any key to finish." );
_ = Console.ReadKey();
