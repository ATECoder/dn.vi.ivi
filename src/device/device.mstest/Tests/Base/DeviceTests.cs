using System;
using cc.isr.Std.Tests;
using cc.isr.Std.Tests.Extensions;
using cc.isr.VI.Device.Tests;
using cc.isr.VI.Pith.Settings;

namespace cc.isr.VI.Device.Tests.Base;

/// <summary>   A device tests base class. </summary>
/// <remarks>   David, 2021-03-25. </remarks>
public abstract class DeviceTests
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
    public LoggerTraceListener<DeviceTests>? TraceListener { get; set; }

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
        Assert.IsTrue( this.ResourceSettings.Exists, $"{nameof( this.ResourceSettings )} should exist." );

        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        Assert.IsTrue( this.VisaSessionBase.Session.TimingSettings.Exists, $"{nameof( TimingSettings )} should exist." );
        Assert.IsTrue( this.VisaSessionBase.Session.RegistersBitmasksSettings.Exists, $"{nameof( RegistersBitmasksSettings )} should exist." );

        if ( Logger is not null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<DeviceTests>( Logger );
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
    public static ILogger<DeviceTests>? Logger { get; } = LoggerProvider.CreateLogger<DeviceTests>();

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    protected TestSiteSettingsBase? TestSiteSettings { get; set; }

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    protected ResourceSettings? ResourceSettings { get; set; }

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    protected Settings.DeviceErrorsSettings? DeviceErrorsSettings { get; set; }

    /// <summary>   Gets or sets the visa session base. </summary>
    /// <value> The visa session base. </value>
    protected VisaSessionBase? VisaSessionBase { get; set; }

    #endregion

    #region " trace messages "

    /// <summary>   (Unit Test Method) trace message should be queued. </summary>
    /// <remarks>   Checks if the device adds a trace message to a trace listener. </remarks>
    [TestMethod( "01. Trace Message Should Be Queued" )]
    public void TraceMessageShouldBeQueued()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Asserts.AssertDeviceTraceMessageShouldBeQueued( this.VisaSessionBase );
    }

    #endregion

    #region " visa session base tests "

    /// <summary> (Unit Test Method) queries if a given visa session base should open. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [TestMethod( "02. Visa Session Base Should Open" )]
    public void VisaSessionBaseShouldOpen()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        try
        {
            Asserts.AssertVisaSessionShouldOpen( this.VisaSessionBase, this.ResourceSettings );
        }
        catch
        {
            throw;
        }
        finally
        {
            Asserts.AssertVisaSessionShouldClose( this.VisaSessionBase );
        }
    }

    #endregion

    #region " visa session base + status subsystem tests "

    /// <summary>   (Unit Test Method) device should open without device errors. </summary>
    /// <remarks>   Tests opening and closing a VISA session. </remarks>
    [TestMethod( "03. Device Should Open Without Device Errors" )]
    public void DeviceShouldOpenWithoutDeviceErrors()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            Asserts.AssertSessionInitialValuesShouldMatch( this.VisaSessionBase.Session, this.ResourceSettings );
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertSessionOpenValuesShouldMatch( this.VisaSessionBase.Session, this.ResourceSettings );
        }
        catch
        {
            throw;
        }
        finally
        {
            Asserts.AssertDeviceShouldCloseWithoutErrors( this.VisaSessionBase );
        }
    }

    [TestMethod( "04. Session Should Collect Garbage" )]
    public void SessionShouldCollectGarbage()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertSessionShouldCollectGarbage( this.VisaSessionBase.Session, false );
        }
        catch
        {
            throw;
        }
        finally
        {
            Asserts.AssertDeviceShouldCloseWithoutErrors( this.VisaSessionBase );
        }
    }

    #endregion

    #region " Session Base "

    /// <summary> (Unit Test Method) tests session should read device errors. </summary>
    [TestMethod( "05. Session Should Read Device Error" )]
    public void SessionShouldReadDeviceErrors()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.VisaSessionBase, this.ResourceSettings );
            try
            {
                Asserts.AssertSessionShouldReadDeviceErrors( this.VisaSessionBase.Session, this.DeviceErrorsSettings );
            }
            catch ( Exception )
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
            Asserts.AssertDeviceShouldCloseWithoutErrors( this.VisaSessionBase );
        }
    }

    /// <summary> (Unit Test Method) tests session should clear device errors. </summary>
    [TestMethod( "06. Session Should Clear Device Error" )]
    public void SessionShouldClearDeviceErrors()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.VisaSessionBase, this.ResourceSettings );
            try
            {
                Asserts.AssertSessionShouldClearDeviceErrors( this.VisaSessionBase.Session, this.DeviceErrorsSettings );
            }
            catch ( Exception )
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
            Asserts.AssertDeviceShouldCloseWithoutErrors( this.VisaSessionBase );
        }
    }

    #endregion
}
