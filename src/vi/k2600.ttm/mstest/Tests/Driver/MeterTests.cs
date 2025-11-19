// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

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
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        cc.isr.VI.Device.Tests.Asserts.AssertVisaImplementationShouldBeLoaded();
        Console.WriteLine( $"\tTesting {typeof( cc.isr.VI.Tsp.K2600.Ttm.Meter ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( MeterTests ) );

        // read settings and throw if not found.
        this.LocationSettings = Settings.AllSettings.Instance.LocationSettings;

        // read the TTM Driver settings
        this.TtmSettings.ReadSettings( this.GetType(), ".Driver", true, true );
        Assert.IsTrue( this.TtmSettings.TtmMeterSettings.Exists, $"{nameof( this.TtmSettings.TtmMeterSettings )} should exist" );

        Asserts.LegacyDriver = this.TtmSettings.TtmMeterSettings.LegacyDriver;

        // instantiate a meter with associated visa sessions
        this.Meter = new();
        this.TspDevice = this.Meter.TspDevice;
        this.VisaSessionBase = this.TspDevice;

        VisaSession visaSession = new();
        Assert.IsNotNull( visaSession.Session );
        Assert.AreEqual( VI.Syntax.Tsp.Lua.ClearExecutionStateCommand, visaSession.Session.ClearExecutionStateCommand );
        visaSession.Session.ReadSettings( this.GetType(), ".Session", true, true );
        this.ResourceSettings = visaSession.Session.AllSettings.ResourceSettings;
        Assert.IsTrue( visaSession.Session.AllSettings.TimingSettings.Exists,
            $"{nameof( VisaSession )}.{nameof( MeterTests.TspDevice.Session )}.{nameof( VisaSession.Session.AllSettings.TimingSettings )} does not exist." );

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

    /// <summary>   Gets or sets the location settings. </summary>
    /// <value> The location settings. </value>
    internal cc.isr.Json.AppSettings.Settings.LocationSettings LocationSettings { get; set; } = new();

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
    [TestMethod( DisplayName = "01. Session should open" )]
    public void SessionShouldOpen()
    {
        Assert.IsNotNull( this.Meter, $"{nameof( this.Meter )} should not be null." );

        using Pith.SessionBase session = Asserts.AssetSessionShouldOpen( this.Meter, this.ResourceSettings.ResourceName, this.ResourceSettings.ResourceModel );
        session.ThrowDeviceExceptionIfError();
    }

}
