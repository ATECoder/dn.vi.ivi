using System;
using System.Diagnostics;
using cc.isr.VI.Tsp.K2600.MSTest.Settings;
using cc.isr.VI.Tsp.K2600.MSTest.Visa;

namespace cc.isr.VI.Tsp.K2600.MSTest.Subsystems;

/// <summary> K2600 Subsystems unit tests. </summary>
/// <remarks>  David, 2017-10-10 </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class SubsystemsTests : Device.Tests.Base.SubsystemsTests
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
        VI.Device.Tests.Base.SubsystemsTests.InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
    public static void CleanupTestClass()
    {
        VI.Device.Tests.Base.SubsystemsTests.CleanupBaseTestClass();
    }

    /// <summary>   Gets or sets the reference to the K2600 Device. </summary>
    /// <value> The reference to the K2600 Device. </value>
    private K2600Device? Device { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public override void InitializeBeforeEachTest()
    {
        // reported in the base class
        // Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( cc.isr.VI.Tsp.K2600.MeasureResistanceSubsystem ).Assembly.FullName}" );

        // create an instance of the Serilog logger.
        SessionLogger.Instance.CreateSerilogLogger( typeof( SubsystemsTests ) );

        this.TestSiteSettings = Settings.AllSettings.Instance.TestSiteSettings;
        this.ResourceSettings = Settings.AllSettings.Instance.ResourceSettings;
        this.DeviceErrorsSettings = Settings.AllSettings.Instance.DeviceErrorsSettings;
        this.Device = K2600Device.Create();
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        this.Device.Session.ReadSettings( typeof( VisaSessionTests ), ".Session" );
        Assert.IsTrue( this.Device.Session.TimingSettings.Exists, $"{nameof( K2600Device )}.{nameof( K2600Device.Session )}.{nameof( K2600Device.Session.TimingSettings )} does not exist." );
        this.VisaSessionBase = this.Device;
        base.InitializeBeforeEachTest();
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestCleanup()]
    public override void CleanupAfterEachTest()
    {
        base.CleanupAfterEachTest();
        this.Device?.Dispose();
        this.Device = null;
        SessionLogger.CloseAndFlush();
    }

    #endregion

    #region " status susbsystem "

    /// <summary>   Assert open session check status. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="readErrorEnabled"> True to enable, false to disable the read error. </param>
    /// <param name="resourceInfo">     Information describing the resource. </param>
    /// <param name="statusSettings">   The status settings. </param>
    /// <param name="subsystemsInfo">   Information describing the subsystems. </param>
    protected override void AssertOpenSessionCheckStatus( bool readErrorEnabled )
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        Assert.IsNotNull( this.Device.StatusSubsystemBase );
        Assert.IsNotNull( this.Device.StatusSubsystem );
        try
        {
            System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
            Console.WriteLine( $"@{methodFullName}" );

            cc.isr.VI.Device.Tests.Asserts.AssertSessionInitialValuesShouldMatch( this.Device.Session, this.ResourceSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldPresetKnownState( this.Device );
            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldInitializeKnownState( this.Device );
            cc.isr.VI.Device.Tests.Asserts.AssertSessionOpenValuesShouldMatch( this.Device.Session, this.ResourceSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertDeviceModelShouldMatch( this.Device.StatusSubsystemBase, this.ResourceSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertDeviceErrorsShouldMatch( this.Device.StatusSubsystemBase, this.DeviceErrorsSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertTerminationValuesShouldMatch( this.Device.Session, AllSettings.Instance.IOSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertLineFrequencyShouldMatch( this.Device.StatusSubsystem, AllSettings.Instance.SystemSubsystemSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertIntegrationPeriodShouldMatch( this.Device.StatusSubsystem, AllSettings.Instance.SenseSubsystemSettings, AllSettings.Instance.SystemSubsystemSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertSessionDeviceErrorsShouldClear( this.Device, AllSettings.Instance.DeviceErrorsSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertOrphanMessagesShouldBeEmpty( this.Device.StatusSubsystemBase );
            if ( readErrorEnabled )
                try
                {
                    cc.isr.VI.Device.Tests.Asserts.AssertDeviceErrorsShouldRead( this.Device, AllSettings.Instance.DeviceErrorsSettings );
                }
                catch
                {
                    throw;
                }
                finally
                {
                    // clear the trace as we know it has errors that were purposely created.
                    this.TraceListener?.Clear();
                }
        }
        catch
        {
            throw;
        }
        finally
        {
            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    #endregion

    #region " contact check susbsystem "

    /// <summary>   Assert contact subsystem should check contacts. </summary>
    /// <remarks>   2025-01-23. </remarks>
    /// <param name="device">   The reference to the K2600 Device. </param>
    /// <param name="highOkay"> True to high okay. </param>
    /// <param name="lowOkay">  True to low okay. </param>
    private static void AssertContactSubsystemShouldCheckContacts( K2600Device? device, bool highOkay, bool lowOkay )
    {
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );
        Assert.IsNotNull( device, $"{nameof( device )} should not be null." );
        Assert.IsNotNull( device.Session, $"{nameof( device )}.{nameof( device.Session )} should not be null." );
        Assert.IsTrue( device.IsDeviceOpen, $"{device.ResourceNameCaption} session should be open." );
        Assert.IsNotNull( device.ContactSubsystem, $"{nameof( device )}.{nameof( device.ContactSubsystem )} should not be null." );

        int contactThreshold = 100;
        ContactSubsystem contactSubsystem = device.ContactSubsystem;
        ContactCheckSpeedMode? speedMode = contactSubsystem.ApplyContactCheckSpeedMode( ContactCheckSpeedMode.Fast );
        Console.WriteLine( $"Contact check speed: {speedMode}" );
        double? contactCheckThreshold = contactSubsystem.ApplyContactCheckThreshold( contactThreshold );
        Console.WriteLine( $"Contact check threshold: {contactCheckThreshold}" );
        Stopwatch sw = Stopwatch.StartNew();
        bool? contactCheckPassed = contactSubsystem.QueryContactCheckOkay();
        sw.Stop();
        Console.WriteLine( $"Contact check time: {sw.ElapsedMilliseconds:#.0} ms" );
        Console.WriteLine( $"Contact check passed: {contactCheckPassed}" );
        Assert.IsNotNull( contactCheckPassed, $"{nameof( contactCheckPassed )} should not be null." );
        string contactResistances = contactSubsystem.QueryContactResistances();
        Console.WriteLine( $"Contact high low resistances: {contactResistances}" );
        (double? highResistance, double? lowResistance) = contactSubsystem.ParseContactResistances();
        bool lowContactPassed = lowResistance.GetValueOrDefault( 0 ) < contactThreshold;
        string lowContactOutcome = lowContactPassed ? "passed" : "high";
        Console.WriteLine( $"Low contacts resistance: {lowResistance:#.00} {lowContactOutcome}" );
        bool highContactPassed = highResistance.GetValueOrDefault( 0 ) < contactThreshold;
        string highContactOutcome = highContactPassed ? "passed" : "high";
        Console.WriteLine( $"High contacts resistance: {highResistance:#.00} {highContactOutcome}" );
        Assert.AreEqual( highOkay && lowOkay, contactCheckPassed.Value, $"{nameof( contactCheckPassed )} should be {highOkay} and {lowOkay}." );
        Assert.AreEqual( highOkay, highContactPassed, $"{nameof( highContactPassed )} should be {(highOkay ? """passed""" : """high""")}." );
        Assert.AreEqual( lowOkay, lowContactPassed, $"{nameof( lowContactPassed )} should be {(lowOkay ? """passed""" : """high""")}." );
    }

    /// <summary>   Assert check contacts. </summary>
    /// <remarks>   2025-01-23. </remarks>
    /// <param name="highOkay"> True if high contacts are okay. </param>
    /// <param name="lowOkay">  True if low contacts are okay. </param>
    protected override void AssertCheckContacts( bool highOkay, bool lowOkay )
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        Assert.IsNotNull( this.Device.StatusSubsystemBase );
        Assert.IsNotNull( this.Device.StatusSubsystem );
        try
        {
            System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
            Console.WriteLine( $"@{methodFullName}" );

            cc.isr.VI.Device.Tests.Asserts.AssertSessionInitialValuesShouldMatch( this.Device.Session, this.ResourceSettings );
            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );
            SubsystemsTests.AssertContactSubsystemShouldCheckContacts( this.Device, highOkay, lowOkay );
        }
        catch
        {
            throw;
        }
        finally
        {
            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    #endregion
}
