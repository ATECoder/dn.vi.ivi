using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

/// <summary>
/// This is a test class for the TTM Meter Legacy Driver.
/// </summary>
/// <remarks> David, 2024-11-01. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class DeviceTests
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
    public static void InitializeTestClass( TestContext testContext )
    {
        string methodFullName = $"{testContext.FullyQualifiedTestClassName}.{System.Reflection.MethodBase.GetCurrentMethod()?.Name}";
        try
        {
            // Console.WriteLine( $"{methodFullName} initializing" );
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"{methodFullName} failed initializing:\r\n\t{ex}" );

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
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Console.WriteLine( $"\t{typeof( cc.isr.Visa.Gac.Vendor ).Assembly.FullName}" );
        if ( cc.isr.Visa.Gac.GacLoader.LoadedImplementation is not null )
            // the implementation is loaded after the device opens and then stays loaded. 
            Console.WriteLine( $"\t{cc.isr.Visa.Gac.GacLoader.LoadedImplementation?.Location}." );
        Console.WriteLine( $"\tTesting {typeof( LegacyDevice ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( DeviceTests ) );

        // read the TTM Driver settings
        // this.TtmSettings.ReadSettings( this.GetType().Assembly, ".Driver", true, true );
        // Assert.IsTrue( this.TtmSettings.TtmMeterSettings.Exists, $"{nameof( this.TtmSettings.TtmMeterSettings )} should exist" );
        // Asserts.LegacyDriver = this.TtmSettings.TtmMeterSettings.LegacyDriver;

        // read the settings and throw if not found
        this.TestSiteSettings = AllSettings.Instance.TestSiteSettings;
        Assert.IsNotNull( AllSettings.Instance.Scribe, $"{nameof( AllSettings )}.{nameof( AllSettings.Instance )}.{nameof( AllSettings.Instance.Scribe )} must not be null." );
        AllSettings.Instance.Scribe.InitializeSettingsFiles();
        AllSettings.Instance.Scribe.ReadSettings();

        // instantiate the legacy device, meter, TSP device and associated visa sessions
        this.LegacyDevice = new( this.GetType(), ".Driver", true, true );
        Assert.IsNotNull( this.LegacyDevice );

        Assert.IsTrue( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings.Exists,
            $"{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings )} should exist" );
        Asserts.LegacyDriver = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings.LegacyDriver;

        this.Meter = this.LegacyDevice.Meter;
        Assert.IsNotNull( this.LegacyDevice.Meter.TspDevice );

        this.TspDevice = this.Meter.TspDevice;
        Assert.IsNotNull( this.TspDevice );

        this.VisaSessionBase = this.TspDevice;
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        Assert.AreEqual( VI.Syntax.Tsp.Lua.ClearExecutionStateCommand, this.VisaSessionBase.Session.ClearExecutionStateCommand );
        this.VisaSessionBase.Session.ReadSettings( this.GetType().Assembly, ".Session", true, true );
        this.ResourceSettings = this.VisaSessionBase.Session.ResourceSettings;

        Assert.IsTrue( this.VisaSessionBase.Session.TimingSettings.Exists,
            $"{nameof( K2600Device )}.{nameof( K2600Device.Session )}.{nameof( K2600Device.Session.TimingSettings )} does not exist." );
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
    internal TestSiteSettings TestSiteSettings { get; set; } = new();

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

    /// <summary>   Gets or sets the legacy device. </summary>
    /// <value> The legacy device. </value>
    internal LegacyDevice? LegacyDevice { get; set; }

    #endregion

    /// <summary>   (Unit Test Method) Assert that the session should open. </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    [TestMethod( "01. Session should open" )]
    public void SessionShouldOpen()
    {
        Assert.IsNotNull( this.LegacyDevice, $"{nameof( this.LegacyDevice )} should not be null." );

        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.LegacyDevice, this.ResourceSettings.ResourceName );

        _ = this.LegacyDevice.Disconnect();
    }

    [TestMethod( "02. Meter should initialize" )]
    public void MeterShouldInitialize()
    {
        Assert.IsNotNull( this.LegacyDevice, $"{nameof( this.LegacyDevice )} should not be null." );

        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.LegacyDevice, this.ResourceSettings.ResourceName );

        Asserts.AssertMeterShouldInitializeKnowState( this.LegacyDevice );

        _ = this.LegacyDevice.Disconnect();
    }

    [TestMethod( "03. Meter should preset" )]
    public void MeterShouldPreset()
    {
        Assert.IsNotNull( this.LegacyDevice, $"{nameof( this.LegacyDevice )} should not be null." );

        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.LegacyDevice, this.ResourceSettings.ResourceName );
        session.ThrowDeviceExceptionIfError();

        Asserts.AssertMeterShouldPreset( this.LegacyDevice );

        _ = this.LegacyDevice.Disconnect();
    }

    /// <summary>   (Unit Test Method) measurements should configure. </summary>
    /// <remarks>   2024-11-08. </remarks>
    [TestMethod( "04. Measurements Should Configure" )]
    public void MeasurementsShouldConfigure()
    {
        Assert.IsNotNull( this.LegacyDevice, $"{nameof( this.LegacyDevice )} should not be null." );

        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.LegacyDevice, this.ResourceSettings.ResourceName );
        session.ThrowDeviceExceptionIfError();

        Asserts.AssertMeasurementsShouldConfigure( this.LegacyDevice );

        _ = this.LegacyDevice.Disconnect();
    }

    /// <summary>   (Unit Test Method) measurement should trigger. </summary>
    /// <remarks>   2024-11-15. </remarks>
    [TestMethod( "05. Measurement should trigger" )]
    public void MeasurementShouldTrigger()
    {
        Assert.IsNotNull( this.LegacyDevice, $"{nameof( this.LegacyDevice )} should not be null." );

        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.LegacyDevice, this.ResourceSettings.ResourceName );

        Stopwatch sw = Stopwatch.StartNew();
        bool validateTriggerCycleReplyMessage = true;
        TimeSpan timeout = TimeSpan.FromSeconds( 1 );
        Asserts.AssertTriggerCycleShouldStart( this.LegacyDevice, timeout, validateTriggerCycleReplyMessage );
        Asserts.AssertMeasurementsShouldRead( this.LegacyDevice, !validateTriggerCycleReplyMessage );
        sw.Stop();
        TimeSpan timeSpan = sw.Elapsed;
        Console.WriteLine( $"         Elapsed: {timeSpan:s\\.fff}s" );
        Asserts.AssertInitialResistanceReadingShouldValidate( this.LegacyDevice );
        Asserts.AssertTriggerCycleShouldAbort( this.LegacyDevice );

        _ = this.LegacyDevice.Disconnect();
    }

    /// <summary>   (Unit Test Method) trigger cycle should abort. </summary>
    /// <remarks>   2024-11-08. </remarks>
    [TestMethod( "06. Trigger cycle should abort" )]
    public void TriggerCycleShouldAbort()
    {
        Assert.IsNotNull( this.LegacyDevice, $"{nameof( this.LegacyDevice )} should not be null." );

        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.LegacyDevice, this.ResourceSettings.ResourceName );

        Stopwatch sw = Stopwatch.StartNew();
        Asserts.AssertTriggerCycleShouldAbort( this.LegacyDevice );
        sw.Stop();
        TimeSpan timeSpan = sw.Elapsed;
        Console.WriteLine( $"Abort Time: {timeSpan:s\\.fff}s" );

        _ = this.LegacyDevice.Disconnect();
    }
}
