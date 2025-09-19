namespace cc.isr.VI.Tsp.K2600.MSTest.Measure;

/// <summary> K2600 Resistance Measurement unit tests. </summary>
/// <remarks> David, 2017-10-10 </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class ResistanceTests : Device.Tsp.Tests.Base.TestBase
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
        VI.Device.Tsp.Tests.Base.TestBase.InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
    public static void CleanupTestClass()
    {
        VI.Device.Tsp.Tests.Base.TestBase.CleanupBaseTestClass();
    }

    /// <summary>   Gets or sets the reference to the K2600 Device. </summary>
    /// <value> The reference to the K2600 Device. </value>
    private K2600Device? Device { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public override void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        cc.isr.VI.Device.Tests.Asserts.AssertVisaImplementationShouldBeLoaded();
        Console.WriteLine( $"\tTesting {typeof( cc.isr.VI.Tsp.K2600.MeasureResistanceSubsystem ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( ResistanceTests ) );

        this.TestSiteSettings = Settings.AllSettings.Instance.TestSiteSettings;
        this.ResourceSettings = Settings.AllSettings.Instance.ResourceSettings;
        this.Device = K2600Device.Create();
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        this.Device.Session.ReadSettings( this.GetType().Assembly, ".Session", true, true );
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

    #region " measure resistance "

    /// <summary>
    /// (Unit Test Method) the source function <see cref="Settings.AllSettings.SourceResistanceSettings.SourceFunction"/>
    /// e.g., <see cref="SourceFunctionMode.CurrentDC"/>, should apply.
    /// </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    [TestMethod( "01. Source function should apply" )]
    public void SourceFunctionShouldApply()
    {
        try
        {
            VI.Device.Tsp.Tests.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );
            try
            {
                VI.Device.Tsp.Tests.Asserts.AssertSourceFunctionShouldApply( this.Device?.SourceSubsystem, Settings.AllSettings.Instance.SourceResistanceSettings.SourceFunction );

                // TO_DO: implement measurement: toggle output. Set source level, read resistance, compare to range value, toggle output off.
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }
        catch
        {
            throw;
        }
        finally
        {
            VI.Device.Tsp.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    #endregion
}
