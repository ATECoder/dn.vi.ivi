using Ivi.Visa;

namespace Ivi.VisaNet;

// Source - https://stackoverflow.com/q/79781879
// Posted by Wollmich, modified by community. See post 'Timeline' for change history
// Retrieved 2026-04-10, License - CC BY-SA 4.0
internal sealed class Program
{
    private static void Main( string[] args )
    {
        string resourceName = string.IsNullOrWhiteSpace( args[0] )
            ? "TCPIP0::192.168.0.150::inst0::INSTR"
            : args[0];

        Options options = args.Length > 1
            ? ( Options ) Convert.ToInt32( args[1], System.Globalization.CultureInfo.CurrentCulture )
            : Options.QueryIdentity;

        int timeout = args.Length > 2
            ? Convert.ToInt32( args[2], System.Globalization.CultureInfo.CurrentCulture )
            : 1000;

        // Connect
        using IMessageBasedSession? vi = GlobalResourceManager.Open( resourceName ) as IMessageBasedSession
            ?? throw new InvalidOperationException( $"GlobalResourceManager.Open method returned null;" );

        // Set TCP Keep Alive
        if ( vi.ResourceName.StartsWith( "TCPIP", StringComparison.OrdinalIgnoreCase ) )
        {
            (( INativeVisaSession ) vi).SetAttributeBoolean( NativeVisaAttribute.TcpKeepAlive, true );
        }

        if ( options.HasFlag( Options.QueryIdentity ) )
        {
            // IDN query
            vi.RawIO.Write( "*IDN?" );
            Console.WriteLine( vi.RawIO.ReadString() );
        }

        // IDN query using SRQ
        if ( options.HasFlag( Options.SingleServiceRequestQuery ) )
        {
            vi.EnableEvent( EventType.ServiceRequest );
            vi.DiscardEvents( EventType.ServiceRequest );
            vi.RawIO.Write( "*CLS;*ESE 1;*SRE 32" );
            vi.RawIO.Write( "*IDN?;*OPC" );
            Console.WriteLine( "Waiting for service request" );
            VisaEventArgs visaEventArgs = vi.WaitOnEvent( EventType.ServiceRequest, timeout );
            if ( visaEventArgs is null )
            {
                Console.WriteLine( "No event received." );
                return;
            }
            else if ( visaEventArgs.EventType != EventType.ServiceRequest )
            {
                Console.WriteLine( $"Unexpected event type: {visaEventArgs.EventType}" );
                return;
            }
            else if ( visaEventArgs.EventType == EventType.ServiceRequest )
            {
                StatusByteFlags statusByte = vi.ReadStatusByte();
                vi.DisableEvent( EventType.ServiceRequest );
                Console.WriteLine( $"Status byte: &h{statusByte:X2}" );
                Console.WriteLine( vi.RawIO.ReadString() );
            }
        }

        // IDN query using multiple service requests
        if ( options.HasFlag( Options.MultipleServiceRequestsQueries ) )
        {
            vi.EnableEvent( EventType.ServiceRequest );
            vi.DiscardEvents( EventType.ServiceRequest );
            vi.RawIO.Write( "*CLS;*ESE 1;*SRE 32" );
            for ( int i = 0; i < 5; i++ )
            {
                vi.RawIO.Write( "*IDN?;*OPC" );
                Console.WriteLine( $"Waiting for service request #{i + 1}" );
                VisaEventArgs visaEventArgs = vi.WaitOnEvent( EventType.ServiceRequest, 1000 );
                if ( visaEventArgs is null )
                {
                    Console.WriteLine( "No event received." );
                    return;
                }
                else if ( visaEventArgs.EventType != EventType.ServiceRequest )
                {
                    Console.WriteLine( $"Unexpected event type: {visaEventArgs.EventType}" );
                    return;
                }
                else if ( visaEventArgs.EventType == EventType.ServiceRequest )
                {
                    StatusByteFlags statusByte = vi.ReadStatusByte();
                    vi.DisableEvent( EventType.ServiceRequest );
                    Console.WriteLine( $"Status byte: &h{statusByte:X2}" );
                    Console.WriteLine( vi.RawIO.ReadString() );
                }
            }
        }

        if ( options.HasFlag( Options.MonitorKeepAlive ) )
        {
            // Sleep to monitor keep alive traffic using Wireshark
            Thread.Sleep( 5 * 60 * 60 * 1000 );
            // IDN query
            vi.RawIO.Write( "*IDN?" );
            Console.WriteLine( vi.RawIO.ReadString() );
            // IDN query using SRQ
            vi.EnableEvent( EventType.ServiceRequest );
            vi.DiscardEvents( EventType.ServiceRequest );
            vi.RawIO.Write( "*CLS;*ESE 1;*SRE 32" );
            vi.RawIO.Write( "*IDN?;*OPC" );
            Console.WriteLine( "Waiting for service request" );
            _ = vi.WaitOnEvent( EventType.ServiceRequest, 1000 );
            StatusByteFlags statusByte = vi.ReadStatusByte();
            vi.DisableEvent( EventType.ServiceRequest );
            Console.WriteLine( $"Status byte: &h{statusByte:X2}" );
            Console.WriteLine( vi.RawIO.ReadString() );
        }

        // Disconnect
        vi.Dispose();
    }
}

[Flags]
public enum Options
{
    None = 0,
    QueryIdentity = 1,
    SingleServiceRequestQuery = 2,
    MultipleServiceRequestsQueries = 4,
    MonitorKeepAlive = 8
}
