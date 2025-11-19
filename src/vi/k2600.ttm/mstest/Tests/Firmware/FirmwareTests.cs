// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

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
    [ClassCleanup]
    public static void CleanupTestClass()
    {
    }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public void InitializeBeforeEachTest()
    {
        // reported in the base class
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        cc.isr.VI.Device.Tests.Asserts.AssertVisaImplementationShouldBeLoaded();
        Console.WriteLine( $"\tTesting {typeof( cc.isr.VI.Tsp.K2600.Ttm.Meter ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( FirmwareTests ) );

        // read settings and throw if not found.
        this.LocationSettings = Settings.AllSettings.Instance.LocationSettings;

        // read the TTM Driver settings and throw if not found.
        this.TtmSettings.ReadSettings( this.GetType(), ".Driver", true, true );
        cc.isr.VI.Tsp.K2600.Ttm.TtmMeterSettings meterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
        Assert.IsTrue( meterSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings )} should exist." );
        cc.isr.VI.Tsp.K2600.Ttm.TtmResistanceSettings resistanceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings;
        Assert.IsTrue( resistanceSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings )} should exist." );
        Assert.AreEqual( 0.1, resistanceSettings.VoltageMaximum );
        Asserts.LegacyDriver = this.TtmSettings.TtmMeterSettings.LegacyDriver;

        cc.isr.VI.Tsp.VisaSession visaSession = new();
        Assert.IsNotNull( visaSession.Session );
        Assert.AreEqual( cc.isr.VI.Syntax.Tsp.Lua.ClearExecutionStateCommand, visaSession.Session.ClearExecutionStateCommand );

        // read settings and throw if not found.
        visaSession.Session.ReadSettings( this.GetType(), ".Session", true, true );
        this.ResourceSettings = visaSession.Session.AllSettings.ResourceSettings;

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

    /// <summary>   Gets or sets the location settings. </summary>
    /// <value> The location settings. </value>
    internal cc.isr.Json.AppSettings.Settings.LocationSettings LocationSettings { get; set; } = new();

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
    [TestMethod( DisplayName = "01. Session should open" )]
    public void SessionShouldOpen()
    {
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.ResourceSettings.ResourceName );
        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   (Unit Test Method) Assert that a current should be measured. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    [TestMethod( DisplayName = "02. Current should measure" )]
    public void CurrentShouldMeasure()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );
        string value = Asserts.AssertCurrentShouldBeMeasured( session );
        Console.Out.WriteLine( $"Measured Current {value:G5}" );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary> (Unit Test Method) Assert that current should be measured multiple times. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [TestMethod( DisplayName = "03. Current should be measured multiple times" )]
    public void CurrentShouldBeMeasuredMultipleTimes()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );
        for ( int i = 1; i <= 100; i++ )
        {
            string value = Asserts.AssertCurrentShouldBeMeasured( session );
            Console.Out.WriteLine( $"Measured Current #{i}: {value:G5}" );

            VI.Device.Tests.Asserts.AssertOrphanMessages( session );
            VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
        }
    }

    /// <summary>   (Unit Test Method) tsp syntax should not fail. </summary>
    /// <remarks>   2025-02-06. </remarks>
    [TestMethod( DisplayName = "04. TSP syntax should not fail" )]
    public void TspSyntaxShouldNotFail()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertTspSyntaxShouldNotFail( session, false, true );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary>   (Unit Test Method) meter value should reset. </summary>
    /// <remarks>   2025-02-06. </remarks>
    [TestMethod( DisplayName = "05. Meter value should reset" )]
    public void MeterValueShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertMeterValueShouldReset( session );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary>   (Unit Test Method) cold resistance defaults should equal settings. </summary>
    /// <remarks>   2025-02-06. </remarks>
    [TestMethod( DisplayName = "06. Cold resistance defaults should equal settings" )]
    public void ColdResistanceDefaultsShouldEqualSettings()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertColdResistanceDefaultsShouldEqualSettings( session );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary>   (Unit Test Method) initial resistance should reset. </summary>
    /// <remarks>   2025-02-06. </remarks>
    [TestMethod( DisplayName = "07. Initial resistance should reset" )]
    public void InitialResistanceShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertColdResistanceShouldReset( session, Asserts.IR );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    [TestMethod( DisplayName = "08. Final resistance should reset" )]
    public void FinalResistanceShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertColdResistanceShouldReset( session, Asserts.FR );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    [TestMethod( DisplayName = "09. Estimator should reset" )]
    public void EstimatorShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertEstimatorShouldReset( session, true );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary>   (Unit Test Method) thermal transient defaults should equal settings. </summary>
    /// <remarks>   2025-02-18. </remarks>
    [TestMethod( DisplayName = "10. Thermal transient defaults should equal settings" )]
    public void ThermalTransientDefaultsShouldEqualSettings()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertThermalTransientDefaultsShouldEqualSettings( session );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary>   (Unit Test Method) thermal transient should reset. </summary>
    /// <remarks>   2025-02-18. </remarks>
    [TestMethod( DisplayName = "11. Thermal transient should reset" )]
    public void ThermalTransientShouldReset()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertThermalTransientShouldReset( session );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary>   (Unit Test Method) framework should clear known state. </summary>
    /// <remarks>   2024-10-31. </remarks>
    [TestMethod( DisplayName = "12. Framework should clear known state" )]
    public void FrameworkShouldClearKnownState()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        Asserts.AssertTtmElementReadings( session, Asserts.IR );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        Asserts.AssertTtmElementReadings( session, Asserts.TR );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        Asserts.AssertTtmElementReadings( session, Asserts.FR );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        Asserts.AssertEstimatesShouldRead( session );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }


    /// <summary>   (Unit Test Method) measurement should be triggered. </summary>
    /// <remarks>   2025-02-07. </remarks>
    [TestMethod( DisplayName = "13. Measurement should be triggered" )]
    public void MeasurementShouldBeTriggered()
    {
        string resourceName = this.ResourceSettings.ResourceName;
        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( resourceName );

        TimeSpan timeout = TimeSpan.FromSeconds( 1 );
        Asserts.AssertTriggerCycleShouldStart( session, timeout, true );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        Asserts.AssertTtmElementReadings( session, Asserts.IR );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        Asserts.AssertTtmElementReadings( session, Asserts.TR );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        Asserts.AssertTtmElementReadings( session, Asserts.FR );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        Asserts.AssertEstimatesShouldRead( session );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        if ( !MeterSubsystem.LegacyFirmware )
        {
            Asserts.AssertEstimatesShouldEstimate( session );

            VI.Device.Tests.Asserts.AssertOrphanMessages( session );
            VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
        }
    }
}
