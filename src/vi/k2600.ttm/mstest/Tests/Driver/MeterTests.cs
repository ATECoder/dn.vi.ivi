using System;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Driver;

/// <summary>
/// This is a test class for the TTM Meter.
/// </summary>
/// <remarks> David, 2020-10-12. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class MeterTests
{
    #region " construction and cleanup "

    /// <summary>
    /// Gets or sets the test context which provides information about and functionality for the
    /// current test run.
    /// </summary>
    /// <value> The test context. </value>
    public TestContext? TestContext { get; set; }

    /// <summary> Initializes the test class before running the first test. </summary>
    /// <remarks>
    /// Use <see cref="InitializeTestClass(TestContext)"/> to run code before running the first test in the class.
    /// </remarks>
    /// <param name="testContext"> Gets or sets the test context which provides information about
    ///                            and functionality for the current test run. </param>
    [ClassInitialize()]
    [CLSCompliant( false )]
    public static void InitializeTestClass( TestContext testContext )
    {
        string methodFullName = $"{testContext.FullyQualifiedTestClassName}.{System.Reflection.MethodBase.GetCurrentMethod()?.Name}";
        try
        {
            Trace.WriteLine( "Initializing", methodFullName );
        }
        catch ( Exception ex )
        {
            Trace.WriteLine( $"Failed initializing the test class: {ex}", methodFullName );

            // cleanup to meet strong guarantees

            try
            {
                CleanupTestClass();
            }
            finally
            {
            }
        }
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
    public static void CleanupTestClass()
    {
    }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public void InitializeBeforeEachTest()
    {
        // reported in the base class
        // Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( K2600Device ).Assembly.FullName}" );

        // create an instance of the Serilog logger. 
        SessionLogger.Instance.CreateSerilogLogger( typeof( MeterTests ) );
        Assert.IsTrue( Settings.AllSettings.Exists(), $"{nameof( Settings.AllSettings )} settings file {Settings.AllSettings.FilePath} should exist" );

        Assert.IsTrue( Settings.AllSettings.TestSiteSettings.Exists, $"{nameof( Settings.AllSettings.TestSiteSettings )} should exist" );
        Assert.IsTrue( Settings.AllSettings.ResourceSettings.Exists, $"{nameof( Settings.AllSettings.ResourceSettings )} should exist" );

        // read the TTM Driver settings
        this.TtmSettings.ReadSettings( this.GetType(), ".Driver" );
        Assert.IsTrue( this.TtmSettings.TtmMeterSettings.Exists, $"{nameof( this.TtmSettings.TtmMeterSettings )} should exist" );

        Asserts.LegacyDriver = this.TtmSettings.TtmMeterSettings.LegacyDriver;

        this.TestSiteSettings = Settings.AllSettings.TestSiteSettings;
        this.ResourceSettings = Settings.AllSettings.ResourceSettings;

        // instantiate a meter with associated visa sessions
        this.Meter = new();
        this.TspDevice = this.Meter.TspDevice;
        this.VisaSessionBase = this.TspDevice;

        VisaSession visaSession = new();
        Assert.IsNotNull( visaSession.Session );
        Assert.AreEqual( VI.Syntax.Tsp.Lua.ClearExecutionStateCommand, visaSession.Session.ClearExecutionStateCommand );
        visaSession.Session.ReadSettings( typeof( MeterTests ), ".Session" );
        Assert.IsTrue( visaSession.Session.TimingSettings.Exists,
            $"{nameof( VisaSession )}.{nameof( MeterTests.TspDevice.Session )}.{nameof( VisaSession.Session.TimingSettings )} does not exist." );

        this.VisaSessionBase = visaSession;
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestCleanup()]
    public void CleanupAfterEachTest()
    {
        SessionLogger.CloseAndFlush();
    }

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the ttm settings. </summary>
    /// <value> The ttm settings. </value>
    internal Properties.Settings TtmSettings { get; set; } = Properties.Settings.Instance;

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    internal Settings.TestSiteSettings TestSiteSettings { get; set; } = new();

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    internal Pith.Settings.ResourceSettings ResourceSettings { get; set; } = new();

    /// <summary>   Gets or sets the visa session base. </summary>
    /// <value> The visa session base. </value>
    internal VisaSessionBase? VisaSessionBase { get; set; }

    /// <summary>   Gets or sets the meter. </summary>
    /// <value> The meter. </value>
    internal Meter? Meter { get; set; }

    /// <summary>   Gets or sets the 2600 device. </summary>
    /// <value> The k 2600 device. </value>
    internal K2600Device? TspDevice { get; set; }

    #endregion

    /// <summary>   (Unit Test Method) Assert that the session should open. </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    [TestMethod( "01. Session Should Open" )]
    public void SessionShouldOpen()
    {
        Assert.IsNotNull( this.Meter, $"{nameof( this.Meter )} should not be null." );

        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.Meter, this.ResourceSettings.ResourceName, this.ResourceSettings.ResourceModel );
        session.ThrowDeviceExceptionIfError();
    }

}
