namespace cc.isr.VI.Tsp.K2600.MSTest.Visa;

/// <summary>   K2002 Visa Session unit tests. </summary>
/// <remarks>   David, 2017-10-10. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public partial class VisaSessionTests : Device.Tests.Base.VisaSessionTests
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
        Device.Tests.Base.VisaSessionTests.InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
    public static void CleanupTestClass()
    {
        Device.Tests.Base.VisaSessionTests.CleanupTestBaseClass();
    }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public override void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Console.WriteLine( $"\tTesting {typeof( cc.isr.VI.Tsp.K2600.K2600Device ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( VisaSessionTests ) );

        this.TestSiteSettings = Settings.AllSettings.Instance.TestSiteSettings;
        this.ResourceSettings = Settings.AllSettings.Instance.ResourceSettings;
        cc.isr.VI.Tsp.VisaSession visaSession = new();
        Assert.IsNotNull( visaSession.Session );
        Assert.AreEqual( Syntax.Tsp.Lua.ClearExecutionStateCommand, visaSession.Session.ClearExecutionStateCommand );
        visaSession.Session.ReadSettings( this.GetType().Assembly, ".Session", true, true );
        Assert.IsTrue( visaSession.Session.TimingSettings.Exists, $"{nameof( VisaSession )}.{nameof( K2600Device.Session )}.{nameof( VisaSession.Session.TimingSettings )} does not exist." );

        this.VisaSessionBase = visaSession;
        base.InitializeBeforeEachTest();
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestCleanup()]
    public override void CleanupAfterEachTest()
    {
        base.CleanupAfterEachTest();
        SessionLogger.CloseAndFlush();
    }

    #endregion
}

