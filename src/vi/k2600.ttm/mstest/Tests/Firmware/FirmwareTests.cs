using System;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Firmware;

/// <summary>
/// This is a test class for the TTM Firmware.
/// </summary>
/// <remarks> David, 2020-10-12. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class FirmwareTests
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
        Console.WriteLine( $"Testing {typeof( cc.isr.VI.Tsp.K2600.K2600Device ).Assembly.FullName}" );

        // create an instance of the Serilog logger. 
        SessionLogger.Instance.CreateSerilogLogger( typeof( FirmwareTests ) );
        Assert.IsTrue( Settings.AllSettings.Exists(), $"{nameof( Settings.AllSettings )} settings file {Settings.AllSettings.FilePath} should exist" );

        Assert.IsTrue( Settings.AllSettings.TestSiteSettings.Exists, $"{nameof( Settings.AllSettings.TestSiteSettings )} should exist" );
        Assert.IsTrue( Settings.AllSettings.ResourceSettings.Exists, $"{nameof( Settings.AllSettings.ResourceSettings )} should exist" );

        Assert.IsTrue( this.TtmSettings.TtmMeterSettings.Exists, $"{nameof( this.TtmSettings.TtmMeterSettings )} should exist" );

        // read the TTM Driver settings
        this.TtmSettings.ReadSettings( this.GetType(), ".Driver" );
        Asserts.LegacyDriver = this.TtmSettings.TtmMeterSettings.LegacyDriver;

        this.TestSiteSettings = Settings.AllSettings.TestSiteSettings;
        this.ResourceSettings = Settings.AllSettings.ResourceSettings;
        cc.isr.VI.Tsp.VisaSession visaSession = new();
        Assert.IsNotNull( visaSession.Session );
        Assert.AreEqual( cc.isr.VI.Syntax.Tsp.Lua.ClearExecutionStateCommand, visaSession.Session.ClearExecutionStateCommand );
        visaSession.Session.ReadSettings( typeof( FirmwareTests ), ".Session" );
        Assert.IsTrue( visaSession.Session.TimingSettings.Exists,
            $"{nameof( VisaSession )}.{nameof( K2600Device.Session )}.{nameof( VisaSession.Session.TimingSettings )} does not exist." );

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
    internal cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings TtmSettings { get; set; } = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance;

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    internal Settings.TestSiteSettings TestSiteSettings { get; set; } = new();

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    internal Settings.ResourceSettings ResourceSettings { get; set; } = new();

    /// <summary>   Gets or sets the visa session base. </summary>
    /// <value> The visa session base. </value>
    internal VisaSessionBase? VisaSessionBase { get; set; }

    #endregion

    /// <summary>   (Unit Test Method) Assert that the session should open. </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    [TestMethod( "01. Session Should Open" )]
    public void SessionShouldOpen()
    {
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.ResourceSettings.ResourceName );
        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   (Unit Test Method) Assert that a current should be measured. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    [TestMethod( "02. Current Should Measure" )]
    public void CurrentShouldMeasure()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );
        string value = Asserts.AssertCurrentShouldBeMeasured( session );
        Console.Out.WriteLine( $"Measured Current {value:G5}" );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session );
    }

    /// <summary> (Unit Test Method) Assert that current should be measured multiple times. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [TestMethod( "03. Current Should Be Measured Multiple Times" )]
    public void CurrentShouldBeMeasuredMultipleTimes()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );
        for ( int i = 1; i <= 100; i++ )
        {
            string value = Asserts.AssertCurrentShouldBeMeasured( session );
            Console.Out.WriteLine( $"Measured Current #{i}: {value:G5}" );

            Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertCurrentShouldBeMeasured )}" );
        }
    }

    /// <summary>   (Unit Test Method) firmware syntax should not fail. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    [TestMethod( "04. Firmware Syntax Should Not Fail" )]
    public void FirmwareSyntaxShouldNotFail()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertTspSyntaxShouldNotFail( session, true );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTspSyntaxShouldNotFail )}" );

        Asserts.AssertMeterValueShouldReset( session, true );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertMeterValueShouldReset )}" );

        Asserts.AssertColdResistanceDefaultsShouldBeFetched( session, true );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertColdResistanceDefaultsShouldBeFetched )}" );

        Asserts.AssertInitialResistanceShouldReset( session, true );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertInitialResistanceShouldReset )}" );

        Asserts.AssertEstimatorShouldReset( session, true );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertEstimatorShouldReset )}" );

        Asserts.AssertThermalTransientShouldReset( session, true );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertThermalTransientShouldReset )}" );
    }

    /// <summary>   (Unit Test Method) framework should clear known state. </summary>
    /// <remarks>   2024-10-31. </remarks>
    [TestMethod( "05. Framework should clear known state" )]
    public void FrameworkShouldClearKnownState()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertInitialResistanceReadings( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertInitialResistanceReadings )}" );

        Asserts.AssertThermalTransientReadings( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertThermalTransientReadings )}" );

        Asserts.AssertFinalResistanceReadings( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertFinalResistanceReadings )}" );

        Asserts.AssertEstimatesShouldRead( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertEstimatesShouldRead )}" );
    }


    /// <summary>   (Unit Test Method) *TRG command should trigger measurements. </summary>
    /// <remarks>   2024-10-26. </remarks>
    [TestMethod( "06. TRG command should trigger measurements" )]
    public void TRGCommandShouldTriggerMeasurements()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertTheTRGCommandShouldTriggerMeasurements( session, TimeSpan.FromMilliseconds( 1000 ) );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTheTRGCommandShouldTriggerMeasurements )}" );

        Asserts.AssertInitialResistanceReadings( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertInitialResistanceReadings )}" );

        Asserts.AssertThermalTransientReadings( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertThermalTransientReadings )}" );

        Asserts.AssertFinalResistanceReadings( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertFinalResistanceReadings )}" );

        Asserts.AssertEstimatesShouldRead( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertEstimatesShouldRead )}" );
    }
}
