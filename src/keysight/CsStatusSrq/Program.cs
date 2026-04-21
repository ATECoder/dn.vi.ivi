using Ivi.Visa;

// Source - https://stackoverflow.com/q/79781879

// Connect
IMessageBasedSession vi = GlobalResourceManager.Open( "TCPIP0::192.168.0.150::inst0::INSTR" ) as IMessageBasedSession ??
    throw new InvalidOperationException( "Failed opening a session" );

// IDN query
vi.RawIO.Write( "*IDN?" );
string identity = vi.RawIO.ReadString().TrimEnd().Trim( "\n\r".ToCharArray() );
Console.WriteLine( $"IDN: {identity}" );

// IDN query using SRQ
vi.EnableEvent( EventType.ServiceRequest );
vi.DiscardEvents( EventType.ServiceRequest );
vi.RawIO.Write( "*CLS" );
Thread.Sleep( 10 );

string expectedEse = "1";
vi.RawIO.Write( $"*ESE {expectedEse}" );
Thread.Sleep( 10 );
vi.RawIO.Write( "*ESE?" );
string actualEse = vi.RawIO.ReadString().TrimEnd().Trim( "\n\r".ToCharArray() );
Console.WriteLine( $"ESE: {actualEse}" );
if ( actualEse != expectedEse )
    throw new InvalidOperationException( $"Expected ESE {expectedEse} != Actual ESE {actualEse}" );

string expectedSre = "32";
vi.RawIO.Write( $"*SRE {expectedSre}" );
Thread.Sleep( 10 );
vi.RawIO.Write( "*SRE?" );
string actualSre = vi.RawIO.ReadString().TrimEnd().Trim( "\n\r".ToCharArray() );
Console.WriteLine( $"SRE: {actualSre}" );
if ( actualSre != expectedSre )
    throw new InvalidOperationException( $"Expected SRE {expectedSre} != Actual SRE {actualSre}" );

int srqCount = 1;
vi.RawIO.Write( "*IDN?" );
vi.RawIO.Write( "*OPC" );
try
{
    Console.WriteLine( $"Awaiting SRQ #{srqCount}" );
    _ = vi.WaitOnEvent( EventType.ServiceRequest, 1000 );
    StatusByteFlags status = vi.ReadStatusByte();
    vi.DisableEvent( EventType.ServiceRequest );
    identity = vi.RawIO.ReadString().TrimEnd().Trim( "\n\r".ToCharArray() );
    Console.WriteLine( $"Status byte: &h{( int ) status:X2}, IDN: {identity}" );
}
catch ( Exception ex )
{
    Console.WriteLine( $"Exception while waiting for SRQ #{srqCount}:\n{ex}" );
}

Thread.Sleep( 100 );
// IDN query
vi.RawIO.Write( "*IDN?" );
identity = vi.RawIO.ReadString().TrimEnd().Trim( "\n\r".ToCharArray() );
Console.WriteLine( $"IDN: {identity}" );

// IDN query using SRQ
vi.EnableEvent( EventType.ServiceRequest );
vi.DiscardEvents( EventType.ServiceRequest );
vi.RawIO.Write( "*CLS" );
Thread.Sleep( 10 );

expectedEse = "1";
vi.RawIO.Write( $"*ESE {expectedEse}" );
Thread.Sleep( 10 );
vi.RawIO.Write( "*ESE?" );
actualEse = vi.RawIO.ReadString().TrimEnd().Trim( "\n\r".ToCharArray() );
Console.WriteLine( $"ESE: {actualEse}" );
if ( actualEse != expectedEse )
    throw new InvalidOperationException( $"Expected ESE {expectedEse} != Actual ESE {actualEse}" );

expectedSre = "32";
vi.RawIO.Write( $"*SRE {expectedSre}" );
Thread.Sleep( 10 );
vi.RawIO.Write( "*SRE?" );
actualSre = vi.RawIO.ReadString().TrimEnd().Trim( "\n\r".ToCharArray() ); ;
Console.WriteLine( $"SRE: {actualSre}" );
if ( actualSre != expectedSre )
    throw new InvalidOperationException( $"Expected SRE {expectedSre} != Actual SRE {actualSre}" );

srqCount += 1;
vi.RawIO.Write( "*IDN?" );
vi.RawIO.Write( "*OPC" );
try
{
    Console.WriteLine( $"Awaiting SRQ #{srqCount}" );
    _ = vi.WaitOnEvent( EventType.ServiceRequest, 1000 );
    StatusByteFlags status = vi.ReadStatusByte();
    vi.DisableEvent( EventType.ServiceRequest );
    identity = vi.RawIO.ReadString().TrimEnd().Trim( "\n\r".ToCharArray() );
    Console.WriteLine( $"Status byte: &h{( int ) status:X2}, IDN: {identity}" );
}
catch ( Exception ex )
{
    Console.WriteLine( $"Exception while waiting for SRQ #{srqCount}:\n{ex}" );
}

// Disconnect
vi.Dispose();
