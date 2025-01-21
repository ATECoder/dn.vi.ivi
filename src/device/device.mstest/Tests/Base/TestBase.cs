using System;
using cc.isr.Std.Tests;
using cc.isr.Std.Tests.Extensions;
using cc.isr.VI.Device.Tests;
using cc.isr.VI.Pith;
using cc.isr.VI.Pith.Settings;

namespace cc.isr.VI.Device.Tests.Base;

/// <summary>   A device status only tests base class. </summary>
/// <remarks>   David, 2021-03-25. </remarks>
public abstract class TestBase
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
                Console.Out.WriteLine( $"Initializing {methodFullName}" );
            else
                Logger?.LogVerboseMultiLineMessage( "Initializing" );
        }
        catch ( Exception ex )
        {
            if ( Logger is null )
                Console.WriteLine( $"Failed initializing the test class: {ex}", methodFullName );
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
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    public static void CleanupBaseTestClass()
    { }

    private IDisposable? _loggerScope;

    /// <summary>   Gets or sets the trace listener. </summary>
    /// <value> The trace listener. </value>
    protected LoggerTraceListener<TestBase>? TraceListener { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>											   
    public virtual void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( VisaSessionBase ).Assembly.FullName}" );

        // assert reading of test settings from the configuration file.
        Assert.IsNotNull( this.TestSiteSettings, $"{nameof( this.TestSiteSettings )} should not be null." );
        Assert.IsTrue( this.TestSiteSettings.Exists, $"{nameof( this.TestSiteSettings )} should exist in the JSon file." );
        double expectedUpperLimit = 12d;
        Assert.IsTrue( Math.Abs( this.TestSiteSettings.TimeZoneOffset() ) < expectedUpperLimit,
                       $"{nameof( this.TestSiteSettings.TimeZoneOffset )} should be lower than {expectedUpperLimit}" );
        Assert.IsNotNull( this.ResourceSettings, $"{nameof( this.ResourceSettings )} should not be null." );
        Assert.IsTrue( this.ResourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist." );

        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        Assert.IsTrue( this.VisaSessionBase.Session.TimingSettings.Exists, $"{nameof( TimingSettings )} should exist." );
        Assert.IsTrue( this.VisaSessionBase.Session.RegistersBitmasksSettings.Exists, $"{nameof( RegistersBitmasksSettings )} should exist." );

        if ( Logger is not null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<TestBase>( Logger! );
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
    public static ILogger<TestBase>? Logger { get; } = LoggerProvider.CreateLogger<TestBase>();

    #endregion

    #region " settings "

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

    #region " Test Site Settings tests  "

    /// <summary>   Assert resource name should ping. </summary>
    /// <remarks>   2024-08-26. </remarks>
    public static void AssertResourceNameShouldPing( SessionBase session, ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( session, $"{nameof( SessionBase )} should not be null." );
        Assert.IsNotNull( resourceSettings, $"{nameof( resourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( SessionBase )}.{nameof( Pith.Settings.ResourceSettings )} should exist." );
        int trials = 0;
        bool pinged = false;
        TimeSpan postPingDelay = TimeSpan.FromMilliseconds( 500 );
        Stopwatch sw = Stopwatch.StartNew();
        while ( trials < 3 && !pinged )
        {
            sw = Stopwatch.StartNew();
            pinged = resourceSettings.ResourcePinged;
            trials += 1;
            if ( !pinged )
                _ = SessionBase.AsyncDelay( postPingDelay );
        }
        TimeSpan elapsed = sw.Elapsed;
        Assert.IsTrue( pinged, $"{resourceSettings.ResourceName} ping failed after {elapsed:s\\.fff}s after {trials} trials." );
        Console.Out.WriteLine( $"\n{resourceSettings.ResourceName} pinged in {elapsed:s\\.fff}s after {trials} trials.\n" );
    }

    /// <summary>   (Unit Test Method) settings is local pacific standard time. </summary>
    /// <remarks>   David, 2020-09-23. </remarks>
    [TestMethod( "00. Local time zone should equals the expected time zone" )]
    public void SettingsIsLocalPacificStandardTime()
    {
        // Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        // Console.WriteLine( $"Text context {this.TestContext?.GetHashCode():exists;exists;null}" );
        Assert.IsNotNull( this.TestSiteSettings, $"{nameof( this.TestSiteSettings )} should not be null." );
        TimeZoneInfo tz = TimeZoneInfo.Local;
        string expected = this.TestSiteSettings.TimeZone();
        string actual = tz.Id;
        Assert.AreEqual( expected, actual );
    }

    #endregion
}
