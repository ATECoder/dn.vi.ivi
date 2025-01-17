using System;
using cc.isr.VI.Tsp.K2600.MSTest.Visa;

namespace cc.isr.VI.Tsp.K2600.MSTest.Source;

/// <summary>   K2600 Current Source unit tests. </summary>
/// <remarks>   David, 2017-10-10. </remarks>
[TestClass]
[Ignore( "Pending implementation" )]
[TestCategory( "k2600" )]
public class CurrentSourceTests : cc.isr.VI.Device.MSTest.Base.TestBase
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
        cc.isr.VI.Device.MSTest.Base.TestBase.InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
    public static void CleanupTestClass()
    {
        cc.isr.VI.Device.MSTest.Base.TestBase.CleanupBaseTestClass();
    }

    /// <summary>   Gets or sets the reference to the K2600 Device. </summary>
    /// <value> The reference to the K2600 Device. </value>
    private K2600Device? Device { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public override void InitializeBeforeEachTest()
    {
        // reported in the base class.
        // Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( cc.isr.VI.Tsp.K2600.CurrentSourceSubsystem ).Assembly.FullName}" );

        // create an instance of the Serilog logger. 
        SessionLogger.Instance.CreateSerilogLogger( typeof( CurrentSourceTests ) );
        Assert.IsTrue( Settings.AllSettings.Exists(), $"{nameof( Settings.AllSettings )}  settings file  {Settings.AllSettings.FilePath} should exist" );

        this.TestSiteSettings = Settings.AllSettings.TestSiteSettings;
        this.ResourceSettings = Settings.AllSettings.ResourceSettings;
        this.Device = K2600Device.Create();
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        this.Device.Session.ReadSettings( typeof( VisaSessionTests ), ".Session" );
        Assert.IsTrue( this.Device.Session.TimingSettings.Exists );
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

    #region " source current measure voltage "

    /// <summary> Assert that the source function <see cref="Settings.AllSettings.CurrentSourceSettings.SourceFunction"/>,
    ///           e.g., <see cref="SourceFunctionMode.CurrentDC"/>, should apply. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="device"> The device. </param>
    private static void AssertSourceFunctionShouldApply( K2600Device? device )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.SourceSubsystem );
        SourceFunctionMode expectedFunctionMode = Settings.AllSettings.SourceCurrentSettings.SourceFunction;
        SourceFunctionMode? sourceFunction = device.SourceSubsystem.ApplySourceFunction( expectedFunctionMode ).GetValueOrDefault( SourceFunctionMode.None );
        Assert.IsNotNull( sourceFunction );
        Assert.AreEqual( expectedFunctionMode, sourceFunction,
            $"{typeof( SourceSubsystemBase )}.{nameof( SourceSubsystemBase.SourceFunction )} is {sourceFunction} ; expected {expectedFunctionMode}" );
    }

    /// <summary> (Unit Test Method) source current measure voltage should pass. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [TestMethod( "01. Source Current Measure Voltage Should Read Voltage" )]
    public void SourceCurrentMeasureVoltageShouldReadVoltage()
    {
        try
        {
            cc.isr.VI.Device.MSTest.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );
            try
            {
                AssertSourceFunctionShouldApply( this.Device );
            }
            catch
            {
                throw;
            }
            finally
            {
                // TODO: change source subsystem to inherit from vi.source subsystem base have the SMU pass the session to this class with the 
                // and use a builder to build the commands adding the SMU prefix. 
                // cc.isr.VI.Device.MSTest.Asserts.ToggleOutput(device.SourceSubsystem, False)
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
