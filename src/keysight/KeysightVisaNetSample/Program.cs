using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using Ivi.Visa;

namespace Ivi.VisaNet;

/// <summary>
/// Example of manufacturer-independent interface Ivi.Visa.NET
/// Before running the example, adjust the ResourceName to fit your instrument
/// </summary>
internal sealed class Program
{
    private static void Main( string[] args )
    {
        string resourceName = args[0];

        TargetFrameworkAttribute? framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>();
        Console.WriteLine( $"Running under {framework?.FrameworkName ?? "--unable to resolve .NET Framework!"} runtime {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );

        // Get VISA.NET Shared Components version.
        Version visaNetSharedComponentsVersion = typeof( GlobalResourceManager ).Assembly!.GetName()!.Version!;
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

        try
        {
            string filter = "TCPIP?*INSTR";
            Console.WriteLine();
            Console.WriteLine( "Executing GlobalResourceManager.Find( {0} )", "\"" + filter + "\" " );
            System.Collections.Generic.IEnumerable<string> resources = GlobalResourceManager.Find( filter );

        }
        catch ( Exception e )
        {
            Console.WriteLine();
            Console.WriteLine( "Error finding resources:\n{0}", e.Message );
            Console.Write( "\nPress any key to finish »" );
            _ = Console.ReadKey();
            return;
        }

        IMessageBasedSession? io;
        // Separate try-catch for the instrument initialization prevents accessing uninitialized object
        try
        {
            // Initialization:
            ParseResult parseResult = GlobalResourceManager.Parse( resourceName );
            io = GlobalResourceManager.Open( resourceName ) as IMessageBasedSession;
        }
        catch ( NativeVisaException e )
        {
            Console.WriteLine();
            Console.WriteLine( "Error initializing the io session at '{0}' :\n{1}", resourceName, e.Message );
            Console.Write( "\nPress any key to finish »" );
            _ = Console.ReadKey();
            return;
        }

        if ( io is null )
        {
            Console.WriteLine();
            Console.WriteLine( "Error: Unable to cast the opened session to IMessageBasedSession at '{0}'", resourceName );
            Console.Write( "\nPress any key to finish »" );
            _ = Console.ReadKey();
            return;
        }


        io.Clear();

        // Timeout for VISA Read Operations
        io.TimeoutMilliseconds = 3000;


        // try block to catch any InstrumentErrorException()
        try
        {
            Console.WriteLine();
            Console.WriteLine( $"IVI {nameof( GlobalResourceManager )} {nameof( GlobalResourceManager.ImplementationVersion )}:{GlobalResourceManager.ImplementationVersion} {nameof( GlobalResourceManager.SpecificationVersion )}:{GlobalResourceManager.SpecificationVersion}" );
            Console.WriteLine( $"Selected the VISA.NET Implementation by {io.ResourceManufacturerName} version {io.ResourceImplementationVersion}\n" );

            io.RawIO.Write( "*RST;*CLS" ); // Reset the instrument, clear the Error queue
            io.RawIO.Write( "*IDN?" );
            string idnResponse = io.RawIO.ReadString();

            Console.WriteLine();
            Console.WriteLine( $"{resourceName} Identification string:\n\n{idnResponse}" );
        }
        catch ( VisaException e )
        {
            Console.WriteLine( $"{resourceName} Instrument reports error(s):\n{e.Message}" );
        }
        finally
        {
            Console.Write( "\nPress any key to finish »" );
            _ = Console.ReadKey();
        }
    }
}
