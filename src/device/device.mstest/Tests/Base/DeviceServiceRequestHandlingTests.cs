// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Std.Logging;
using cc.isr.Std.Logging.ILoggerExtensions;
using cc.isr.Std.Listeners;
using cc.isr.VI.Pith.Settings;

namespace cc.isr.VI.Device.Tests.Base;

/// <summary>   A device service request tests base class. </summary>
/// <remarks>   David, 2021-03-25. </remarks>
public abstract class DeviceServiceRequestTests
{
    #region " construction and cleanup "

    /// <summary>   Initializes the test class before running the first test. </summary>
    /// <remarks>
    /// Use <see cref="InitializeTestClass(TestContext)"/> to run code before running the first test
    /// in the class.
    /// </remarks>
    /// <param name="testContext">  Gets or sets the test context which provides information about
    ///                             and functionality for the current test run. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0060:Remove unused parameter", Justification = "<Pending>" )]
    public static void InitializeTestClass( TestContext? testContext )
    {
        // string methodFullName = $"{testContext.FullyQualifiedTestClassName}.{System.Reflection.MethodBase.GetCurrentMethod()?.Name}";
        try
        {
            if ( Logger is null )
                TestBase.ConsoleOutputMemberMessage( "Initializing" );
            else
                Logger?.LogVerboseMultiLineMessage( "Initializing" );
        }
        catch ( Exception ex )
        {
            if ( Logger is null )
                TestBase.ConsoleOutputMemberMessage( $"Failed initializing the test class: {ex}" );
            else
                Logger.LogExceptionMultiLineMessage( "Failed initializing the test class:", ex );

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
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    public static void CleanupTestClass()
    { }

    private IDisposable? _loggerScope;

    /// <summary>   Gets or sets the trace listener. </summary>
    /// <value> The trace listener. </value>
    public LoggerTraceListener<DeviceServiceRequestTests>? TraceListener { get; set; }

    /// <summary> Initializes the trace listener and logger scope. </summary>
    /// <remarks> call this method from the subclass. </remarks>
    public void DefineTraceListener()
    {
        if ( Logger is not null && this.TraceListener is null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<DeviceServiceRequestTests>( Logger );
            _ = Trace.Listeners.Add( this.TraceListener );
            Asserts.DefineTraceListener( this.TraceListener! );
        }
    }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    public virtual void InitializeBeforeEachTest()
    {
        this.DefineTraceListener();

        // assert reading of test settings from the configuration file.
        Assert.IsNotNull( this.LocationSettings, $"{nameof( this.LocationSettings )} should not be null." );
        Assert.IsTrue( this.LocationSettings.Exists, $"{nameof( this.LocationSettings )} should exist in the JSon file." );
        double expectedUpperLimit = 12d;
        Assert.IsLessThan( expectedUpperLimit,
            Math.Abs( this.LocationSettings.TimeZoneOffset() ), $"{nameof( this.LocationSettings.TimeZoneOffset )} should be lower than {expectedUpperLimit}" );
        Assert.IsNotNull( this.ResourceSettings, $"{nameof( this.ResourceSettings )} should not be null." );
        Assert.IsTrue( this.ResourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist." );

        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        Assert.IsTrue( this.VisaSessionBase.Session.AllSettings.TimingSettings.Exists, $"{nameof( TimingSettings )} should exist." );
        Assert.IsTrue( this.VisaSessionBase.Session.AllSettings.RegistersBitmasksSettings.Exists, $"{nameof( RegistersBitmasksSettings )} should exist." );

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
    public static ILogger<DeviceServiceRequestTests>? Logger { get; } = LoggerProvider.CreateLogger<DeviceServiceRequestTests>();

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the location settings. </summary>
    /// <value> The location settings. </value>
    protected Json.AppSettings.Settings.LocationSettings? LocationSettings { get; set; }

    /// <summary>   Gets or sets the resource settings. </summary>
    /// <value> The resource settings. </value>
    protected ResourceSettings? ResourceSettings { get; set; }

    /// <summary>   Gets or sets the visa session base. </summary>
    /// <value> The visa session base. </value>
    protected VisaSessionBase? VisaSessionBase { get; set; }

    #endregion

    #region " open, close "

    /// <summary>   (Unit Test Method) device should open without device errors. </summary>
    /// <remarks>   David, 2021-07-04. </remarks>
    [TestMethod( DisplayName = "01. Device should open without device errors" )]
    public void DeviceShouldOpenWithoutDeviceErrors()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            this.VisaSessionBase.SubsystemSupportMode = SubsystemSupportMode.StatusOnly;
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.VisaSessionBase, this.ResourceSettings );
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

    #region " service request tests "

    /// <summary>   (Unit Test Method) service request handling should toggle. </summary>
    /// <remarks>
    /// This test will fail the first time it is run if Windows requests access through the Firewall.
    /// </remarks>
    [TestMethod( DisplayName = "02. Service request handling should toggle" )]
    public void ServiceRequestHandlingShouldToggle()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            this.VisaSessionBase.SubsystemSupportMode = SubsystemSupportMode.StatusOnly;
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertServiceRequestHandlingShouldToggle( this.VisaSessionBase.Session );
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

    /// <summary>   (Unit Test Method) service request should be handled by session. </summary>
    /// <remarks>   David, 2021-07-04. </remarks>
    [TestMethod( DisplayName = "03. Service request should be handled by session" )]
    public void ServiceRequestShouldBeHandledBySession()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            this.VisaSessionBase.SubsystemSupportMode = SubsystemSupportMode.StatusOnly;
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertServiceRequestShouldBeHandledBySession( this.VisaSessionBase, this.ResourceSettings );
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

    /// <summary>   (Unit Test Method) service request should be handled by device. </summary>
    /// <remarks>
    /// This test will fail the first time it is run if Windows requests access through the Firewall.
    /// </remarks>
    [TestMethod( DisplayName = "04. Service request should be handled by device" )]
    public void ServiceRequestShouldBeHandledByDevice()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            this.VisaSessionBase.SubsystemSupportMode = SubsystemSupportMode.StatusOnly;
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertServiceRequestShouldBeHandledByDevice( this.VisaSessionBase, this.ResourceSettings );
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
