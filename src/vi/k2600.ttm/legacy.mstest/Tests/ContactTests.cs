using System;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

/// <summary>
/// This is a test class for the TTM Meter Legacy Driver.
/// </summary>
/// <remarks> David, 2024-11-01. </remarks>
[TestClass]
[TestCategory( "k2600cc" )]
public class ContactTests
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
            OutputMessageMethodLine( "Initializing" );
        }
        catch ( Exception ex )
        {
            OutputMessageMethodLine( $"Failed initializing the test class: {ex}" );

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
        Console.WriteLine( $"Testing {typeof( LegacyDevice ).Assembly.FullName}" );

        // create an instance of the Serilog logger.
        SessionLogger.Instance.CreateSerilogLogger( typeof( ContactTests ) );

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

    /// <summary>   Output message with a full member line. </summary>
    /// <remarks>   2025-04-28. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void OutputMessageMemberLine( string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        Console.WriteLine( $"{message} @[{sourcePath}].{memberName}.Line#{sourceLineNumber}" );
    }

    /// <summary>   Output message method line. </summary>
    /// <remarks>   2025-04-28. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void OutputMessageMethodLine( string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        Console.WriteLine( $"{message} @[{memberName}.Line#{sourceLineNumber}" );
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

    /// <summary>   (Unit Test Method) measurement should trigger but fail open contact. </summary>
    /// <remarks>   2024-11-15. </remarks>
    [TestMethod( "01. Measurement should fail open contact" )]
    public void MeasurementShouldFailOpenContact()
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

}
