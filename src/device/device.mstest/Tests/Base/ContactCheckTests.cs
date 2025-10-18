using cc.isr.Std.Tests;
using cc.isr.Std.Tests.Extensions;
using cc.isr.VI.Device.Tests.Settings;
using cc.isr.VI.Pith.Settings;

namespace cc.isr.VI.Device.Tests.Base;

/// <summary>  Contact Check tests base class. </summary>
/// <remarks>   David, 2021-03-25. </remarks>
public abstract class ContactCheckTests
{
    #region " construction and cleanup "

    /// <summary>   Initializes the test class before running the first test. </summary>
    /// <remarks>
    /// Use <see cref="InitializeTestClass(TestContext)"/> to run code before running the first test
    /// in the class.
    /// </remarks>
    /// <param name="testContext">  Gets or sets the test context which provides information about
    ///                             and functionality for the current test run. </param>
    public static void InitializeBaseTestClass( TestContext testContext )
    {
        string methodFullName = $"{testContext.FullyQualifiedTestClassName}.{System.Reflection.MethodBase.GetCurrentMethod()?.Name}";
        try
        {
            // initialize the logger.
            _ = Logger?.BeginScope( methodFullName );
        }
        catch ( Exception ex )
        {
            if ( Logger is null )
                Trace.WriteLine( $"Failed initializing the test class: {ex}", methodFullName );
            else
                Logger.LogExceptionMultiLineMessage( "Failed initializing the test class:", ex );

            // cleanup to meet strong guarantees

            try
            {
                CleanupBaseTestClass();
            }
            finally
            {
            }
        }
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupBaseTestClass"/> to run code after all tests in the class have run. </remarks>
    public static void CleanupBaseTestClass()
    { }

    private IDisposable? _loggerScope;

    /// <summary>   Gets or sets the trace listener. </summary>
    /// <value> The trace listener. </value>
    public LoggerTraceListener<ContactCheckTests>? TraceListener { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    public virtual void InitializeBeforeEachTest()
    {
        // assert reading of test settings from the configuration file.
        Assert.IsNotNull( this.TestSiteSettings, $"{nameof( this.TestSiteSettings )} should not be null." );
        Assert.IsTrue( this.TestSiteSettings.Exists, $"{nameof( this.TestSiteSettings )} should exist in the JSon file." );
        double expectedUpperLimit = 12d;
        Assert.IsLessThan( expectedUpperLimit,
            Math.Abs( this.TestSiteSettings.TimeZoneOffset() ), $"{nameof( this.TestSiteSettings.TimeZoneOffset )} should be lower than {expectedUpperLimit}" );
        Assert.IsNotNull( this.ResourceSettings, $"{nameof( this.ResourceSettings )} should not be null." );
        Assert.IsTrue( this.ResourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist." );

        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        Assert.IsTrue( this.VisaSessionBase.Session.TimingSettings.Exists, $"{nameof( TimingSettings )} should exist." );
        Assert.IsTrue( this.VisaSessionBase.Session.RegistersBitmasksSettings.Exists, $"{nameof( RegistersBitmasksSettings )} should exist." );

        if ( Logger is not null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<ContactCheckTests>( Logger );
            _ = Trace.Listeners.Add( this.TraceListener );
        }

        Asserts.DefineTraceListener( this.TraceListener! );

        // cc.isr.VI.Device.MSTest.Asserts.SetEntryAssembly( entryAssembly );
        // if setting the entry assembly as above, there is no need to set the log settings file name.
        // cc.isr.Logging.TraceLog.Tracer.Instance.LogSettingsFileName = cc.isr.Logging.Orlog.Orlogger.BuildAppSettingsFileName( entryAssembly ); ;
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    public virtual void CleanupAfterEachTest()
    {
        this.VisaSessionBase?.Dispose();
        Assert.IsNotNull( this.TraceListener );
        Assert.IsFalse( this.TraceListener.Any( TraceEventType.Error ),
            $"{nameof( this.TraceListener )} should have no {TraceEventType.Error} messages:" +
            $"{Environment.NewLine}{string.Join( Environment.NewLine, [.. this.TraceListener!.Messages[TraceEventType.Error].ToArray()] )}" );
        this._loggerScope?.Dispose();
        this.TraceListener?.Dispose();
        Trace.Listeners.Clear();
    }

    /// <summary>
    /// Gets or sets the test context which provides information about and functionality for the
    /// current test run.
    /// </summary>
    /// <value> The test context. </value>
    public TestContext? TestContext { get; set; }

    /// <summary>   Gets a logger instance for this category. </summary>
    /// <value> The logger. </value>
    public static ILogger<ContactCheckTests>? Logger { get; } = LoggerProvider.CreateLogger<ContactCheckTests>();

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the i/o settings. </summary>
    /// <value> The i/o settings. </value>
    protected IOSettings? IOSettings { get; set; }

    /// <summary>   Gets or sets the device errors settings. </summary>
    /// <value> The device errors settings. </value>
    protected DeviceErrorsSettings? DeviceErrorsSettings { get; set; }

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    protected TestSiteSettings? TestSiteSettings { get; set; }

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    protected ResourceSettings? ResourceSettings { get; set; }

    /// <summary>   Gets or sets the visa session base. </summary>
    /// <value> The visa session base. </value>
    protected VisaSessionBase? VisaSessionBase { get; set; }

    #endregion

    #region " contact check susbsystem "

    /// <summary>   Assert check contacts. </summary>
    /// <remarks>   2025-01-23. </remarks>
    /// <param name="highOkay">         True to high okay. </param>
    /// <param name="lowOkay">          True to low okay. </param>
    /// <param name="contactThreshold"> The contact threshold. </param>
    protected abstract void AssertCheckContacts( bool highOkay, bool lowOkay, int contactThreshold );

    /// <summary>   Gets or sets the contact check threshold. </summary>
    /// <value> The contact check threshold. </value>
    public int ContactCheckThreshold { get; set; } = 75;

    /// <summary>   (Unit Test Method) contact check should pass. </summary>
    /// <remarks>   2025-01-23. </remarks>
    [TestMethod( DisplayName = "01. Contact check should pass" )]
    public void ContactCheckShouldPass()
    {
        this.AssertCheckContacts( true, true, this.ContactCheckThreshold );
    }

    /// <summary>   (Unit Test Method) contact check should detect open sense low. </summary>
    /// <remarks>   2025-01-23. </remarks>
    [TestMethod( DisplayName = "02. Contact check detect open low sense terminal" )]
    public void ContactCheckShouldDetectOpenSenseLow()
    {
        this.AssertCheckContacts( true, false, this.ContactCheckThreshold );
    }

    /// <summary>   (Unit Test Method) contact check should detect open source low. </summary>
    /// <remarks>   2025-01-23. <para>
    /// The 2600 measure the DUT resistance when either the low or high source lead is open.
    /// Consequently, the contact check will pass if the DUT resistance is lower than the contact check threshold. </para></remarks>
    [TestMethod( DisplayName = "03. Contact check detect open low source terminal" )]
    public void ContactCheckShouldDetectOpenSourceLow()
    {
        this.AssertCheckContacts( true, true, this.ContactCheckThreshold );
    }

    /// <summary>   (Unit Test Method) contact check should detect open leads. </summary>
    /// <remarks>   2025-09-12. </remarks>
    [TestMethod( DisplayName = "04. Contact check detect open leads (>75 ohms)" )]
    public void ContactCheckShouldDetectOpenLeads()
    {
        this.AssertCheckContacts( false, false, this.ContactCheckThreshold );
    }

    #endregion
}
