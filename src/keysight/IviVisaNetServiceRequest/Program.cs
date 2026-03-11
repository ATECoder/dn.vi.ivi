using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using Ivi.Visa;
using Ivi.VisaNet.SessionExtensions;

namespace Ivi.VisaNet;

/// <summary>
/// Example of Ivi.Visa.NET service Request Handling.
/// Demonstrates how to setup the instrument status system to request service when
/// a supported condition occurs and how to detect the SRQ using 2 techniques:
///     1. Polling the instrument status byte 
///     2. Asynchronous SRQ event callback
/// </summary>
internal sealed class Program
{
    private static async Task Main( string[] args )
    {
        string resourceName = args[0];

        // Separate try-catch for the instrument initialization prevents accessing uninitialized object
        try
        {
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
                Console.WriteLine( $"\n***\tVISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} not found. Please install corresponding vendor-specific VISA implementation first." );
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
                Console.WriteLine( $"\n***\tError finding resources:\n\t{e.Message}" );
                return;
            }

            // Initialization:
            ParseResult parseResult = GlobalResourceManager.Parse( resourceName );
            using IMessageBasedSession? io = GlobalResourceManager.Open( resourceName ) as IMessageBasedSession;

            if ( io is null )
            {
                Console.WriteLine();
                Console.WriteLine( "Error: Unable to cast the opened session to IMessageBasedSession at '{0}'", resourceName );
                return;
            }

            io.Clear();

            // Timeout for VISA Read Operations
            io.TimeoutMilliseconds = 3000;

            // get the model to implement the status preset command.
            VersionInfo versionInfo = new();

            string command = string.Empty;
            int opcDelay = 10;

            // try block to catch any InstrumentErrorException()
            try
            {
                Console.WriteLine();
                Console.WriteLine( $"IVI {nameof( GlobalResourceManager )} {nameof( GlobalResourceManager.ImplementationVersion )}:{GlobalResourceManager.ImplementationVersion} {nameof( GlobalResourceManager.SpecificationVersion )}:{GlobalResourceManager.SpecificationVersion}" );
                Console.WriteLine( $"Selected the VISA.NET Implementation by {io.ResourceManufacturerName} version {io.ResourceImplementationVersion}\n" );

                // Reset the instrument
                command = "*RST";
                opcDelay = 100; // Some instruments require a longer delay after reset before they can accept another command
                if ( !( bool ) await io.ExecuteCommandOperationCompleted( command, CancellationToken.None, opcDelay ) )
                {
                    Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                    return;
                }

                command = "*CLS";
                opcDelay = 10;
                if ( !( bool ) await io.ExecuteCommandOperationCompleted( command, CancellationToken.None, opcDelay ) )
                {
                    Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                    return;
                }

                string identity = io.Query( "*IDN?" );

                Console.WriteLine( $"\n{resourceName} Identification string:\n\n{identity}" );

                versionInfo.Parse( identity );
            }
            catch ( VisaException e )
            {
                Console.WriteLine( $"\n***\t{resourceName} Instrument reports error(s):\n\t{e.Message}" );
                return;
            }
            finally
            {
            }

            // Configure and test SRQs
            Console.WriteLine( "Testing Service Request (SRQ) on OperationComplete...\n" );

            // Configure status system to set StatusByte, RequestService bit on EventStatusRegister, OperationComplete bit

            command = "*CLS";
            if ( !( bool ) await io.ExecuteCommandOperationCompleted( command, CancellationToken.None ) )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                return;
            }

            command = versionInfo.IsTspModel ? "status.reset()" : "STAT:PRES";
            if ( !( bool ) await io.ExecuteCommandOperationCompleted( command, CancellationToken.None ) )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                return;
            }

            int writeValue = 253; // Bit 0 operation completion
            command = $"*ESE {writeValue}";
            if ( !( bool ) await io.ExecuteCommandOperationCompleted( command, CancellationToken.None ) )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                return;
            }

            string queryCommand = "*ESE?";
            int readValue = io.QueryInt( queryCommand );
            if ( readValue != writeValue )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}; sent {writeValue} read {readValue}" );
                return;
            }

            writeValue = 255 - 64; //  32 = StandardEventSummary - Bit 5

            command = $"*SRE {writeValue}";
            if ( !( bool ) await io.ExecuteCommandOperationCompleted( command, CancellationToken.None, 200 ) )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                return;
            }

            queryCommand = "*SRE?";
            readValue = io.QueryInt( queryCommand );
            if ( readValue != writeValue )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}; sent {writeValue} read {readValue}" );
                return;
            }

            int pollingIntervalMs = 50;
            int trialCount = 5; // Number of attempts to check for SRQ occurrence in polling technique

            for ( int trialNo = 1; trialNo <= trialCount; trialNo++ )
            {

                // Polling for SRQ technique
                // Program polls StatusByte while waiting for SRQ or until max time elapses.
                Console.WriteLine( $"Testing polling status byte for SRQ trial #{trialNo}" );

                // Do something that will cause the SRQ event to be fired
                // Most but not all instruments support *OPC...
                command = "*OPC";
                io.WriteLine( command ); // Sets ESR Operation Complete bit 0 when all pending operations are complete

                int requestServiceSummary = 64; // Bit 6 RequestServiceSummary bit in StatusByte is set when any enabled event condition occurs.
                                                // This bit is used for SRQ generation if enabled in ServiceRequestEnableRegister and the instrument
                                                // is configured to generate an SRQ on that condition.

                // Polling loop
                bool srqOccurred = false;
                StatusByteFlags statusByte;
                for ( int i = 0; i < 10; i++ ) // Wait up to 500ms (iterations * sleep time) for SRQ
                {
                    System.Threading.Thread.Sleep( pollingIntervalMs ); // time (ms) sets polling interval
                    statusByte = io.ReadStatusByte();

                    if ( (( int ) statusByte & requestServiceSummary) == requestServiceSummary )
                    {
                        Console.WriteLine( $"\tSRQ detected trial #{trialNo}" );
                        // Service SRQ
                        // Your code to query additional status registers to determine the specific source of the SRQ,
                        // if needed, and to take appropriate action to service the SRQ.
                        Console.WriteLine( $"\t\tSerialPoll(): {statusByte}" );
                        Console.WriteLine( $"\t\tEventStatusRegister: {io.QueryHex( "*ESR?" )}\n" );
                        srqOccurred = true;
                        break;
                    }
                }
                if ( !srqOccurred )
                {
                    Console.WriteLine( $"***\tSRQ did not poll by {command} in allotted time trial #{trialNo}\n" );
                    break;
                }
                System.Threading.Thread.Sleep( 100 ); // time (ms) sets polling interval
            }

            // SRQ event callback technique
            // Requires 488.2 compliant connection to instrument
            // Driver asynchronously calls an event handler method when SRQ occurs.  Program execution may continue.
            Console.WriteLine( $"Testing asynchronous SRQ event callback" );

            // Create event handler class and add (register) the SRQ event handler method.
            ServiceRequestHandler handler = new();
            io.ServiceRequest += handler.OnServiceRequest;

            // Enable SRQ event callbacks
            io.EnableEvent( EventType.ServiceRequest ); // Enable SRQ events

            pollingIntervalMs = 50;
            for ( int trialNo = 1; trialNo <= trialCount; trialNo++ )
            {
                handler.PreviousCallCount = handler.CallCount; // Reset handler call count for next trial

                // Do something that will cause the SRQ event to be fired
                // Most but not all instruments support *OPC...
                command = "*OPC";
                io.WriteLine( command ); // Sets ESR Operation Complete bit 0 when all pending operations are complete

                Stopwatch stopwatch = Stopwatch.StartNew();
                while ( stopwatch.ElapsedMilliseconds < 500 && !handler.HandlerCalled )
                {
                    System.Threading.Thread.Sleep( pollingIntervalMs ); // time (ms) sets polling interval
                }

                if ( !handler.HandlerCalled )
                {
                    Console.WriteLine( $"\n***\tSRQ did not call back by {command} in allotted time trial #{trialNo}\n" );
                    break;
                }
            }

            // End configure and test SRQs

            // Check instrument for errors
            // ErrorQueryResult result;
            // Console.WriteLine();
            // do
            // {
            //     result = driver.Utility.ErrorQuery();
            //     Console.WriteLine( "ErrorQuery: {0}, {1}", result.Code, result.Message );
            // } while ( result.Code != 0 );

        }
        catch ( NativeVisaException e )
        {
            Console.WriteLine( $"\n***\tError initializing the io session at '{resourceName}' :\n{e.Message}" );
        }
        finally
        {
            Console.Write( "\nPress any key to finish »" );
            _ = Console.ReadKey();
        }
    }
}
