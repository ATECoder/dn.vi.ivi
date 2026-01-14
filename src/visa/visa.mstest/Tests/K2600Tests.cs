namespace cc.isr.Visa.Tests;

/// <summary>   (Unit Test Class) for the Keithley 2450 instrument using the ICS-9065 interface. </summary>
/// <remarks>   David, 2021-11-05. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class K2600Tests : InstrumentTests
{
    #region " construction and cleanup "

    /// <summary>   Initializes the test class before running the first test. </summary>
    /// <remarks>
    /// Use <see cref="InitializeTestClass(TestContext)"/> to run code before running the first test
    /// in the class.
    /// </remarks>
    /// <param name="testContext">  Gets or sets the test context which provides information about
    ///                             and functionality for the current test run. </param>
    [ClassInitialize()]
    public static void InitializeTestClass( TestContext testContext )
    {
        InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup]
    public static void CleanupTestClass()
    {
        CleanupBaseTestClass();
    }

    #endregion

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public override void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Asserts.AssertVisaImplementationShouldBeLoaded();
        Console.WriteLine( $"\tTesting {string.Join( ",", cc.isr.Visa.Gac.GacLoader.VisaImplementationFriendlyNames )}." );

        // inst0 must be lower case
        // this.ResourceName = "TCPIP0::192.168.0.150::inst0::INSTR";
        // this.ResourceName = "TCPIP0::192.168.0.50::inst0::INSTR";
        // this.ResourceName = "TCPIP0::192.168.0.254::gpib0,26::INSTR";
        this.ResourceName = "TCPIP0::192.168.0.150::inst0::INSTR";
        this.EnabledServiceRequestCommand = "*SRE 16\n";
        this.EnableOperationCompletionEvent = "*ESE 1\n";
        this.OperationCompletionQueryCommand = "*OPC?\n";
        this.ExpectedServiceRequestMessage = "1\\n";
        this.ServiceRequestTestRepeatCount = 1; // with Keysight -- only a single repeat count
        this.SessionOpenTimeout = TimeSpan.FromMilliseconds( 3000 );
        this.CommunicationTimeout = TimeSpan.FromMilliseconds( 300 );
        this.ReadAfterWriteDelay = TimeSpan.FromMilliseconds( 10 );

        this.MessageToWrite = "*IDN?\n";
        this.ExpectedMaximumWriteTime = TimeSpan.FromMilliseconds( 100 );
        this.ExpectedMaximumReadTime = TimeSpan.FromMilliseconds( 100 );
        base.InitializeBeforeEachTest();
    }

    /// <summary>   Method for implementing instrument specific syntax tests. </summary>
    /// <remarks>   2024-09-25. </remarks>
    /// <param name="session">  The session. </param>
    [CLSCompliant( false )]
    protected override void AssertSyntaxTestsShouldPass( Ivi.Visa.IMessageBasedSession? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        // test the code snippet that caused a C stack overflow in the Cold Resistance self init method.
        session.RawIO.Write( "isOkay = function() return smua == nil or smua == smua end print( isOkay() )\n" );
        string reply = session.RawIO.ReadString();
        string expectedReply = "true\n";
        Assert.AreEqual( expectedReply, reply );

        session.RawIO.Write( "encapsulateNil = function( fromValue, toValue ) if nil == fromValue then return toValue else return fromValue end end \n" );
        string fromValue = "nil";
        expectedReply = "true";
        session.RawIO.Write( $"print( encapsulateNil( {fromValue}, {expectedReply} ) ) \n" );
        reply = session.RawIO.ReadString().Trim( "\n".ToCharArray() );
        Assert.AreEqual( expectedReply, reply );

        fromValue = "true";
        expectedReply = "true";
        session.RawIO.Write( $"print( encapsulateNil( {fromValue}, {expectedReply} ) ) \n" );
        reply = session.RawIO.ReadString().Trim( "\n".ToCharArray() );
        Assert.AreEqual( expectedReply, reply );

        session.RawIO.Write( "fromBool = function( boolValue, trueValue, falseOrNilValue ) if boolValue then return trueValue else return falseOrNilValue end end \n" );
        string boolValue = "nil";
        string trueValue = "1";
        string falseOrNilValue = "-1";
        expectedReply = "-1.00000e+00";
        session.RawIO.Write( $"print( fromBool( {boolValue}, {trueValue}, {falseOrNilValue} ) ) \n" );
        reply = session.RawIO.ReadString().Trim( "\n".ToCharArray() );
        Assert.AreEqual( expectedReply, reply );

        boolValue = "true";
        expectedReply = "1.00000e+00";
        session.RawIO.Write( $"print( fromBool( {boolValue}, {trueValue}, {falseOrNilValue} ) ) \n" );
        reply = session.RawIO.ReadString().Trim( "\n".ToCharArray() );
        Assert.AreEqual( expectedReply, reply );

        boolValue = "false";
        expectedReply = "-1.00000e+00";
        session.RawIO.Write( $"print( fromBool( {boolValue}, {trueValue}, {falseOrNilValue} ) ) \n" );
        reply = session.RawIO.ReadString().Trim( "\n".ToCharArray() );
        Assert.AreEqual( expectedReply, reply );

    }
}

