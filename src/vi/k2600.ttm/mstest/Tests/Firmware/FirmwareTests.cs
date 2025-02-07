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
        Console.WriteLine( $"Testing {typeof( cc.isr.VI.Tsp.K2600.Ttm.Meter ).Assembly.FullName}" );

        // create an instance of the Serilog logger. 
        SessionLogger.Instance.CreateSerilogLogger( typeof( FirmwareTests ) );

        // read settings and throw if not found.
        this.TestSiteSettings = Settings.AllSettings.Instance.TestSiteSettings;

        // read the TTM Driver settings and throw if not found.
        this.TtmSettings.ReadSettings( this.GetType(), ".Driver" );
        cc.isr.VI.Tsp.K2600.Ttm.TtmMeterSettings meterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
        Assert.IsTrue( meterSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings )} should exist." );
        cc.isr.VI.Tsp.K2600.Ttm.TtmResistanceSettings resistanceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings;
        Assert.IsTrue( resistanceSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings )} should exist." );
        Assert.AreEqual( 9.999, resistanceSettings.VoltageMaximum );
        Asserts.LegacyDriver = this.TtmSettings.TtmMeterSettings.LegacyDriver;

        cc.isr.VI.Tsp.VisaSession visaSession = new();
        Assert.IsNotNull( visaSession.Session );
        Assert.AreEqual( cc.isr.VI.Syntax.Tsp.Lua.ClearExecutionStateCommand, visaSession.Session.ClearExecutionStateCommand );

        // read settings and throw if not found.
        visaSession.Session.ReadSettings( typeof( FirmwareTests ), ".Session" );
        this.ResourceSettings = visaSession.Session.ResourceSettings;

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
    internal Pith.Settings.ResourceSettings ResourceSettings { get; set; } = new();

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

    /// <summary>   (Unit Test Method) tsp syntax should not fail. </summary>
    /// <remarks>   2025-02-06. </remarks>
    [TestMethod( "04. TSP Syntax Should Not Fail" )]
    public void TspSyntaxShouldNotFail()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertTspSyntaxShouldNotFail( session, true );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTspSyntaxShouldNotFail )}" );
    }

    /// <summary>   (Unit Test Method) meter value should reset. </summary>
    /// <remarks>   2025-02-06. </remarks>
    [TestMethod( "05. Meter Value Should Reset" )]
    public void MeterValueShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertMeterValueShouldReset( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertMeterValueShouldReset )}" );
    }

    /// <summary>   (Unit Test Method) cold resistance defaults should equal settings. </summary>
    /// <remarks>   2025-02-06. </remarks>
    [TestMethod( "06. Cold Resistance Defaults Should Equal Settings" )]
    public void ColdResistanceDefaultsShouldEqualSettings()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertColdResistanceDefaultsShouldEqualSettings( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertColdResistanceDefaultsShouldEqualSettings )}" );
    }

    /// <summary>   (Unit Test Method) initial resistance should reset. </summary>
    /// <remarks>   2025-02-06. </remarks>
    [TestMethod( "07. InitialResistanceShouldReset" )]
    public void InitialResistanceShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertColdResistanceShouldReset( session, Asserts.IR );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertColdResistanceShouldReset )}" );
    }

    [TestMethod( "08. Final Resistance Should Reset" )]
    public void FinalResistanceShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertColdResistanceShouldReset( session, Asserts.FR );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertColdResistanceShouldReset )}" );
    }

    [TestMethod( "09. Estimator Should Reset" )]
    public void EstimatorShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertEstimatorShouldReset( session, true );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertEstimatorShouldReset )}" );
    }

    [TestMethod( "10. ThermalTransientShouldReset" )]
    public void ThermalTransientShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertThermalTransientShouldReset( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertThermalTransientShouldReset )}" );
    }

    /// <summary>   (Unit Test Method) framework should clear known state. </summary>
    /// <remarks>   2024-10-31. </remarks>
    [TestMethod( "11. Framework should clear known state" )]
    public void FrameworkShouldClearKnownState()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertTtmElementReadings( session, Asserts.IR );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTtmElementReadings )} TTM Elements {Asserts.IR}" );

        Asserts.AssertTtmElementReadings( session, Asserts.TR );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTtmElementReadings )} TTM Elements {Asserts.TR}" );

        Asserts.AssertTtmElementReadings( session, Asserts.FR );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTtmElementReadings )} TTM Elements {Asserts.FR}" );

        Asserts.AssertEstimatesShouldRead( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertEstimatesShouldRead )}" );
    }


    /// <summary>   (Unit Test Method) *TRG command should trigger measurements. </summary>
    /// <remarks>   2024-10-26. </remarks>
    [TestMethod( "12. TRG command should trigger measurements" )]
    public void TRGCommandShouldTriggerMeasurements()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertTheTRGCommandShouldTriggerMeasurements( session, TimeSpan.FromMilliseconds( 1000 ) );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTheTRGCommandShouldTriggerMeasurements )}" );

        Asserts.AssertTtmElementReadings( session, Asserts.IR );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTtmElementReadings )} TTM Elements {Asserts.IR}" );

        Asserts.AssertTtmElementReadings( session, Asserts.TR );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTtmElementReadings )} TTM Elements {Asserts.TR}" );

        Asserts.AssertTtmElementReadings( session, Asserts.FR );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertTtmElementReadings )} TTM Elements {Asserts.FR}" );

        Asserts.AssertEstimatesShouldRead( session );
        Asserts.AssertOrphanMessagesOrDeviceErrors( session, $"method {nameof( Asserts.AssertEstimatesShouldRead )}" );
    }
}
