namespace cc.isr.Visa.Tests;

/// <summary>   (Unit Test Class) for the Keithley 2450 instrument using the ICS-9065 interface. </summary>
/// <remarks>   David, 2021-11-05. </remarks>
[TestClass]
[TestCategory( "k2450" )]
public class K2450Tests : InstrumentTests
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
        Console.WriteLine( $"\tTesting {typeof( Ivi.Visa.IMessageBasedSession ).Assembly.FullName}" );

        // inst0 must be lower case
        this.ResourceName = "TCPIP0::192.168.0.152::inst0::INSTR";
        this.EnabledServiceRequestCommand = "*SRE 16\n";
        this.EnableOperationCompletionEvent = "*ESE 1\n";
        this.OperationCompletionQueryCommand = "*OPC?\n";
        this.ExpectedServiceRequestMessage = "1\\n";
        this.ServiceRequestTestRepeatCount = 1; // with keysight -- only a single repeat count
        this.SessionOpenTimeout = TimeSpan.FromMilliseconds( 3000 );
        this.CommunicationTimeout = TimeSpan.FromMilliseconds( 300 );
        this.ReadAfterWriteDelay = TimeSpan.FromMilliseconds( 10 );

        this.MessageToWrite = "*IDN?\n";
        this.ExpectedMaximumWriteTime = TimeSpan.FromMilliseconds( 100 );
        this.ExpectedMaximumReadTime = TimeSpan.FromMilliseconds( 100 );
        base.InitializeBeforeEachTest();
    }
}

