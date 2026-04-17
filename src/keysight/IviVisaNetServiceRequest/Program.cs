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
    private static void Main( string[] args )
    {
        string resourceName = args[0];
        ServiceRequestOptions? options = args.Length > 1
           ? ( ServiceRequestOptions ) Convert.ToInt32( args[1], System.Globalization.CultureInfo.CurrentCulture )
            : ServiceRequestOptions.CallBack | ServiceRequestOptions.Polling;

        // Separate try-catch for the instrument initialization prevents accessing uninitialized object
        try
        {
            TargetFrameworkAttribute? framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>();
            Console.WriteLine( $"Running under {framework?.FrameworkName ?? "--unable to resolve .NET Framework!"} runtime {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );

            // Get VISA.NET Shared Components version.
            Assembly visaNetShareComponentsAssembly = typeof( GlobalResourceManager ).Assembly;
            FileInfo fileInfo = new( visaNetShareComponentsAssembly.Location );
            System.Diagnostics.FileVersionInfo iviVisaVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo( visaNetShareComponentsAssembly.Location );
            Console.WriteLine( "\nVISA.NET Shared Components assembly:" );
            Console.WriteLine( $"\tFull name: {visaNetShareComponentsAssembly.GetName().FullName}" );
            Console.WriteLine( $"\tFile name: {fileInfo.Name}" );
            Console.WriteLine( $"\tLocation:  {fileInfo.DirectoryName}" );
            Console.WriteLine( $"\tVersion:   {visaNetShareComponentsAssembly.GetName().Version}" );
            Console.WriteLine( $"\tProduct:   {iviVisaVersionInfo.ProductVersion}" );
            Console.WriteLine( $"\tFile:      {iviVisaVersionInfo.FileVersion}" );

            // Console.WriteLine();
            // Version visaNetSharedComponentsVersion = visaNetShareComponentsAssembly.GetName()!.Version!;
            // Console.WriteLine( $"VISA.NET Shared Components version {visaNetSharedComponentsVersion}." );

            // Check whether VISA Shared Components is installed before using VISA.NET.
            // If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
            // be thrown during termination process due to a bug in the implementation of the
            // VISA.NET Shared Components, and the application will crash.
            try
            {
                // Get an available version of the VISA Shared Components.
                FileVersionInfo visaSharedComponentsInfo = FileVersionInfo.GetVersionInfo( Path.Combine( Environment.SystemDirectory, "visaConfMgr.dll" ) );
                Console.WriteLine( $"\nVISA Config Manager product version {visaSharedComponentsInfo.ProductVersion}" );
            }
            catch ( FileNotFoundException )
            {
                Console.WriteLine( $"\n***\tVISA implementation compatible with VISA.NET Shared Components {visaNetShareComponentsAssembly.GetName().Version} not found. Please install corresponding vendor-specific VISA implementation first." );
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
            VersionInfo deviceVersionInfo = new();

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
                if ( !io.ExecuteCommandOperationCompleted( command, CancellationToken.None, opcDelay ) ??
                        throw new InvalidCastException(
                            $"{nameof( SessionExtensionMethods.ExecuteCommandOperationCompleted )} returned a null value." ) )
                {
                    Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                    return;
                }

                command = "*CLS";
                opcDelay = 10;
                if ( !io.ExecuteCommandOperationCompleted( command, CancellationToken.None, opcDelay ) ??
                        throw new InvalidCastException(
                            $"{nameof( SessionExtensionMethods.ExecuteCommandOperationCompleted )} returned a null value." ) )
                {
                    Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                    return;
                }

                string identity = io.Query( "*IDN?" );

                Console.WriteLine( $"\n{resourceName} Identification string:\n\n{identity}" );

                deviceVersionInfo.Parse( identity );
            }
            catch ( VisaException e )
            {
                Console.WriteLine( $"\n***\t{resourceName} Instrument reports error(s):\n\t{e.Message}" );
                return;
            }
            finally
            {
            }

            // Configure
            Console.WriteLine( "Configuring Service Request (SRQ) on operation completion" );

            // Configure status system to set StatusByte, RequestService bit on EventStatusRegister, OperationComplete bit

            command = "*CLS";
            if ( !io.ExecuteCommandOperationCompleted( command, CancellationToken.None ) ??
                        throw new InvalidCastException(
                            $"{nameof( SessionExtensionMethods.ExecuteCommandOperationCompleted )} returned a null value." ) )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                return;
            }

            command = deviceVersionInfo.IsTspModel ? "status.reset()" : "STAT:PRES";
            if ( !io.ExecuteCommandOperationCompleted( command, CancellationToken.None ) ??
                        throw new InvalidCastException(
                            $"{nameof( SessionExtensionMethods.ExecuteCommandOperationCompleted )} returned a null value." ) )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                return;
            }

            int writeValue = 1; // 253; // 1 = Bit 0 operation completion
            command = $"*ESE {writeValue}";
            if ( !io.ExecuteCommandOperationCompleted( command, CancellationToken.None ) ??
                        throw new InvalidCastException(
                            $"{nameof( SessionExtensionMethods.ExecuteCommandOperationCompleted )} returned a null value." ) )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                return;
            }

            string queryCommand = "*ESE?";
            int eseValue = io.QueryInt( queryCommand );
            if ( eseValue != writeValue )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}; sent {writeValue} read {eseValue}" );
                return;
            }
            writeValue = 32; // 255 - 64; //  32 = StandardEventSummary - Bit 5

            command = $"*SRE {writeValue}";
            if ( !io.ExecuteCommandOperationCompleted( command, CancellationToken.None, 200 ) ??
                        throw new InvalidCastException(
                            $"{nameof( SessionExtensionMethods.ExecuteCommandOperationCompleted )} returned a null value." ) )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}" );
                return;
            }

            queryCommand = "*SRE?";
            int sreValue = io.QueryInt( queryCommand );
            if ( sreValue != writeValue )
            {
                Console.WriteLine( $"\n***\t{resourceName} Failed executing {command}; sent {writeValue} read {sreValue}" );
                return;
            }

            // test SRQs
            Console.WriteLine( "\nTesting Service Request (SRQ) on OperationComplete" );
            Console.WriteLine( $"\tStandard event status enable (ESE): {eseValue}" );
            Console.WriteLine( $"\tService request enable (SRE): {sreValue}\n" );

            int pollingIntervalMs = 50;
            int trialCount = 5; // Number of attempts to check for SRQ occurrence in polling technique

            // Polling for SRQ technique
            // Program polls StatusByte while waiting for SRQ or until max time elapses.
            command = "*OPC";

            if ( options.Value.HasFlag( ServiceRequestOptions.Polling ) )
            {
                Console.WriteLine( $"Polling status byte for {command}\n" );

                for ( int trialNo = 1; trialNo <= trialCount; trialNo++ )
                {

                    // Do something that will cause the SRQ event to be fired
                    // Most but not all instruments support *OPC...
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
                            Console.WriteLine( $"\tSRQ detected on trial #{trialNo}" );
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
                        Console.WriteLine( $"***\tSRQ did not poll by {command} in allotted time on trial #{trialNo}\n" );
                        break;
                    }
                    System.Threading.Thread.Sleep( 100 ); // time (ms) sets polling interval
                }
            }

            if ( options.Value.HasFlag( ServiceRequestOptions.WaitOnEvents ) )
            {
                // SRQ wait on event technique
                // Requires 488.2 compliant connection to instrument
                Console.WriteLine( $"Waiting on SRQ event for {command}\n" );

                // Enable SRQ event with wait on event
                io.EnableEvent( EventType.ServiceRequest ); // Enable SRQ events
                io.DiscardEvents( EventType.ServiceRequest );

                int waitOnEventTimeout = 100;

                for ( int trialNo = 1; trialNo <= trialCount; trialNo++ )
                {

                    // Do something that will cause the SRQ event to be fired
                    // Most but not all instruments support *OPC...
                    command = "*OPC";
                    io.WriteLine( command ); // Sets ESR Operation Complete bit 0 when all pending operations are complete

                    Stopwatch stopwatch = Stopwatch.StartNew();
                    try
                    {
                        VisaEventArgs visaEventArgs = io.WaitOnEvent( EventType.ServiceRequest, waitOnEventTimeout ); // Wait for SRQ event with timeout

                        if ( stopwatch.ElapsedMilliseconds > waitOnEventTimeout )
                        {
                            Console.WriteLine( $"\n***\tSRQ by {command} exceeded the allotted time on trial #{trialNo}\n" );

                            // The event arguments define two types:
                            // e.CustomEventType of type integer
                            // e.EvenType which should be EventType.ServiceRequest enum value.

                            Console.WriteLine( "\tEvent arguments:" );
                            Console.WriteLine( $"\t\tCustomEventType: {visaEventArgs.CustomEventType}" );
                            Console.WriteLine( $"\t\t      EventType: {visaEventArgs.EventType}" );
                            break;
                        }
                        else
                        {
                            Console.WriteLine( $"\tService Request Event #{trialNo} occurred after {stopwatch.ElapsedMilliseconds} ms" );
                            // The event arguments define two types:
                            // e.CustomEventType of type integer
                            // e.EvenType which should be EventType.ServiceRequest enum value.

                            Console.WriteLine( "\tEvent arguments:" );
                            Console.WriteLine( $"\t\tCustomEventType: {visaEventArgs.CustomEventType}" );
                            Console.WriteLine( $"\t\t      EventType: {visaEventArgs.EventType}" );
                        }
                    }
                    catch ( Ivi.Visa.IOTimeoutException )
                    {
                        Console.WriteLine( $"\n***\tSRQ by {command} timed out after {stopwatch.ElapsedMilliseconds} ms on trial #{trialNo}\n" );
                        break;
                    }
                }

                io.DisableEvent( EventType.ServiceRequest );
            }

            if ( options.Value.HasFlag( ServiceRequestOptions.CallBack ) )
            {
                // SRQ event callback technique
                // Requires 488.2 compliant connection to instrument
                // Driver asynchronously calls an event handler method when SRQ occurs.  Program execution may continue.
                Console.WriteLine( $"Calling back asynchronous SRQ event for {command}" );

                // Create event handler class and add (register) the SRQ event handler method.
                ServiceRequestHandler handler = new();
                io.ServiceRequest += handler.OnServiceRequest;

                // Enable SRQ event callbacks
                io.EnableEvent( EventType.ServiceRequest ); // Enable SRQ events
                io.DiscardEvents( EventType.ServiceRequest );

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
                        Console.WriteLine( $"\n***\tSRQ did not call back by {command} in allotted time on trial #{trialNo}\n" );
                        break;
                    }
                }
            }
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

/// <summary>   A bit-field of flags for specifying service request options. </summary>
/// <remarks>   2026-04-16. </remarks>
[Flags]
public enum ServiceRequestOptions
{
    None,
    Polling,
    WaitOnEvents,
    CallBack = 4
}
