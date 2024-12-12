using System;
using cc.isr.Std.Tests;
using cc.isr.Std.Tests.Extensions;
using cc.isr.VI.Device.MSTest.Settings;
using cc.isr.VI.Pith.Settings;

namespace cc.isr.VI.Device.MSTest.Base;

/// <summary>  Subsystem tests base class. </summary>
/// <remarks>   David, 2021-03-25. </remarks>
public abstract class SubsystemsTests
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
            if ( Logger is null )
                Trace.WriteLine( "Initializing", methodFullName );
            else
                Logger?.LogVerboseMultiLineMessage( "Initializing" );
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
    public LoggerTraceListener<SubsystemsTests>? TraceListener { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>											   
    public virtual void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( cc.isr.VI.SubsystemBase ).Assembly.FullName}" );

        // assert reading of test settings from the configuration file.
        Assert.IsNotNull( this.TestSiteSettings, $"{nameof( this.TestSiteSettings )} should not be null." );
        Assert.IsTrue( this.TestSiteSettings.Exists, $"{nameof( this.TestSiteSettings )} should exist in the JSon file." );
        double expectedUpperLimit = 12d;
        Assert.IsTrue( Math.Abs( this.TestSiteSettings.TimeZoneOffset() ) < expectedUpperLimit,
                       $"{nameof( this.TestSiteSettings.TimeZoneOffset )} should be lower than {expectedUpperLimit}" );
        Assert.IsNotNull( this.ResourceSettings, $"{nameof( this.ResourceSettings )} should not be null." );
        Assert.IsTrue( this.ResourceSettings.Exists, $"{nameof( Settings.ResourceSettingsBase )} should exist." );

        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        Assert.IsTrue( this.VisaSessionBase.Session.TimingSettings.Exists, $"{nameof( TimingSettings )} should exist." );
        Assert.IsTrue( this.VisaSessionBase.Session.RegistersBitmasksSettings.Exists, $"{nameof( RegistersBitmasksSettings )} should exist." );

        Assert.IsNotNull( this.SubsystemsSettings, $"{nameof( this.SubsystemsSettings )} {nameof( SubsystemsSettingsBase )} should exist" );
        Assert.IsTrue( this.SubsystemsSettings.Exists, $"{nameof( this.SubsystemsSettings )} should ne enabled" );

        if ( Logger is not null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<SubsystemsTests>( Logger );
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
    public static ILogger<SubsystemsTests>? Logger { get; } = LoggerProvider.CreateLogger<SubsystemsTests>();

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the subsystems settings. </summary>
    /// <value> The subsystems settings. </value>
    protected SubsystemsSettingsBase? SubsystemsSettings { get; set; }

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    protected TestSiteSettingsBase? TestSiteSettings { get; set; }

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    protected ResourceSettingsBase? ResourceSettings { get; set; }

    /// <summary>   Gets or sets the visa session base. </summary>
    /// <value> The visa session base. </value>
    protected VisaSessionBase? VisaSessionBase { get; set; }

    #endregion

    #region " status susbsystem "

    /// <summary>   Opens session and check subsystems status. </summary>
    /// <remarks>   2024-08-01. </remarks>
    /// <param name="readErrorEnabled"> True to read errors; otherwise false. </param>
    /// <param name="subsystemsInfo">   Information describing the subsystems. </param>
    protected abstract void AssertOpenSessionCheckStatus( bool readErrorEnabled );

    /// <summary>   (Unit Test Method) opens session check status should pass. </summary>
    /// <remarks>   David, 2021-07-04. </remarks>
    [TestMethod( "01. Open Session Check Status Should Pass All Tests" )]
    public void OpenSessionCheckStatusShouldPass()
    {
        Assert.IsNotNull( this.SubsystemsSettings );
        this.AssertOpenSessionCheckStatus( false );
    }

    /// <summary> (Unit Test Method) tests open session read device errors. </summary>
    [TestMethod( "02. Open Session Should Pass All Tests and Read Device Errors" )]
    public void OpenSessionReadDeviceErrorsShouldPass()
    {
        Assert.IsNotNull( this.SubsystemsSettings );
        this.AssertOpenSessionCheckStatus( true );
    }

    #endregion

}
