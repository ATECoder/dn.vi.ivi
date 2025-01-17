using System;
using cc.isr.VI.Tsp.K2600.MSTest.Settings;
using cc.isr.VI.Tsp.K2600.MSTest.Visa;

namespace cc.isr.VI.Tsp.K2600.MSTest.Subsystems;

/// <summary> K2600 Subsystems unit tests. </summary>
/// <remarks>  David, 2017-10-10 </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class SubsystemsTests : cc.isr.VI.Device.MSTest.Base.SubsystemsTests
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
        cc.isr.VI.Device.MSTest.Base.SubsystemsTests.InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
    public static void CleanupTestClass()
    {
        cc.isr.VI.Device.MSTest.Base.SubsystemsTests.CleanupBaseTestClass();
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
        Assert.IsTrue( Settings.AllSettings.Exists(), $"{nameof( Settings.AllSettings )} settings file {Settings.AllSettings.FilePath} should exist" );

        this.TestSiteSettings = Settings.AllSettings.TestSiteSettings;
        this.ResourceSettings = Settings.AllSettings.ResourceSettings;
        this.DeviceErrorsSettings = Settings.AllSettings.DeviceErrorsSettings;
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

            cc.isr.VI.Device.MSTest.Asserts.AssertSessionInitialValuesShouldMatch( this.Device.Session, this.ResourceSettings );
            cc.isr.VI.Device.MSTest.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );
            cc.isr.VI.Device.MSTest.Asserts.AssertDeviceShouldPresetKnownState( this.Device );
            cc.isr.VI.Device.MSTest.Asserts.AssertDeviceShouldInitializeKnownState( this.Device );
            cc.isr.VI.Device.MSTest.Asserts.AssertSessionOpenValuesShouldMatch( this.Device.Session, this.ResourceSettings );
            cc.isr.VI.Device.MSTest.Asserts.AssertDeviceModelShouldMatch( this.Device.StatusSubsystemBase, this.ResourceSettings );
            cc.isr.VI.Device.MSTest.Asserts.AssertDeviceErrorsShouldMatch( this.Device.StatusSubsystemBase, this.DeviceErrorsSettings );
            cc.isr.VI.Device.MSTest.Asserts.AssertTerminationValuesShouldMatch( this.Device.Session, AllSettings.IOSettings );
            cc.isr.VI.Device.MSTest.Asserts.AssertLineFrequencyShouldMatch( this.Device.StatusSubsystem, AllSettings.SystemSubsystemSettings );
            cc.isr.VI.Device.MSTest.Asserts.AssertIntegrationPeriodShouldMatch( this.Device.StatusSubsystem, AllSettings.SenseSubsystemSettings, AllSettings.SystemSubsystemSettings );
            cc.isr.VI.Device.MSTest.Asserts.AssertSessionDeviceErrorsShouldClear( this.Device, AllSettings.DeviceErrorsSettings );
            cc.isr.VI.Device.MSTest.Asserts.AssertOrphanMessagesShouldBeEmpty( this.Device.StatusSubsystemBase );
            if ( readErrorEnabled )
                try
                {
                    cc.isr.VI.Device.MSTest.Asserts.AssertDeviceErrorsShouldRead( this.Device, AllSettings.DeviceErrorsSettings );
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
            cc.isr.VI.Device.MSTest.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    #endregion
}
