using System;
using cc.isr.Std.Tests;
using cc.isr.Std.Tests.Extensions;
using cc.isr.VI.Pith.Settings;

namespace cc.isr.VI.Device.Tests.Base;

/// <summary>  A Visa Session tests base class. </summary>
/// <remarks>   David, 2021-03-25. </remarks>
[TestClass]
[CLSCompliant( false )]
public abstract class VisaSessionTests
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
                CleanupTestBaseClass();
            }
            finally
            {
            }
        }

    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    public static void CleanupTestBaseClass()
    { }

    private IDisposable? _loggerScope;

    /// <summary>   Gets or sets the trace listener. </summary>
    /// <value> The trace listener. </value>
    public LoggerTraceListener<VisaSessionTests>? TraceListener { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    [TestInitialize()]
    public virtual void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"\t{cc.isr.Visa.Gac.GacLoader.LoadedImplementation?.Location}." );
        Console.WriteLine( $"\t{typeof( Ivi.Visa.IMessageBasedSession ).Assembly.FullName}" );
        Console.WriteLine( $"\t{typeof( cc.isr.Visa.Gac.Vendor ).Assembly.FullName}" );
        // handled in the sub class.
        // Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {TimeZoneInfo.Local}" );
        // Console.WriteLine( $"\tTesting {typeof( VisaSession ).Assembly.FullName}" );

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
            this.TraceListener = new LoggerTraceListener<VisaSessionTests>( Logger );
            _ = Trace.Listeners.Add( this.TraceListener );
        }

        Asserts.DefineTraceListener( this.TraceListener! );

        // cc.isr.VI.Device.MSTest.Asserts.SetEntryAssembly( entryAssembly );
        // if setting the entry assembly as above, there is no need to set the log settings file name.
        // cc.isr.Logging.TraceLog.Tracer.Instance.LogSettingsFileName = cc.isr.Logging.Orlog.Orlogger.BuildAppSettingsFileName( entryAssembly ); ;
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    [TestCleanup()]
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
    public static ILogger<VisaSessionTests>? Logger { get; } = LoggerProvider.CreateLogger<VisaSessionTests>();

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

    #region " resource should be found "

    /// <summary>   (Unit Test Method) resource name should be included. </summary>
    /// <remarks>   Finds the resource using the session factory resources manager. </remarks>
    [TestMethod( "01. Resource name should be included" )]
    public void ResourceNameShouldBeIncluded()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        Asserts.AssertVisaResourceManagerShouldIncludeResource( this.VisaSessionBase.Session, this.ResourceSettings );
    }

    /// <summary>   (Unit Test Method) resource name should be found. </summary>
    /// <remarks>   David, 2021-07-04. </remarks>
    [TestMethod( "02. Resource name should be found" )]
    public void ResourceNameShouldBeFound()
    {
        using VisaSession session = VisaSession.Create();
        Assert.IsNotNull( session.Session );
        session.Session.ReadSettings( this.GetType().Assembly, ".Session", true, true );
        Assert.IsTrue( session.Session.TimingSettings.Exists );
        Asserts.AssertVisaSessionShouldFindResource( session, this.ResourceSettings );
    }

    #endregion

    #region " trace message tests "

    /// <summary>   (Unit Test Method) trace message should be queued. </summary>
    /// <remarks>   Checks if the device adds a trace message to a trace listener. </remarks>
    [TestMethod( "03. Trace message should be queued" )]
    public void TraceMessageShouldBeQueued()
    {
        using VisaSession session = VisaSession.Create();
        Assert.IsNotNull( session.Session );
        session.Session.ReadSettings( this.GetType().Assembly, ".Session", true, true );
        Assert.IsTrue( session.Session.TimingSettings.Exists );
        Asserts.AssertDeviceTraceMessageShouldBeQueued( session );
    }

    #endregion

    #region " open/close "

    /// <summary>   (Unit Test Method) queries if a given visa session should open. </summary>
    /// <remarks>   David, 2021-07-04. </remarks>
    [TestMethod( "04. Visa session should open" )]
    public void VisaSessionShouldOpen()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
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

    /// <summary>
    /// (Unit Test Method) queries if a given visa session should open and clear execution state.
    /// </summary>
    /// <remarks>   2024-08-02. </remarks>
    [TestMethod( "05. Visa session should clear execution state" )]
    public void VisaSessionShouldClearExecutionState()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            Asserts.AssertVisaSessionShouldOpen( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertSessionShouldClearExecutionState( this.VisaSessionBase.Session );
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

    #region " session status and register bits "

    /// <summary>   (Unit Test Method) visa session should wait for status bitmask. </summary>
    /// <remarks>   David, 2021-07-04. </remarks>
    [TestMethod( "06. Visa session should wait for status bitmask" )]
    public void VisaSessionShouldWaitForStatusBitmask()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            Asserts.AssertVisaSessionShouldOpen( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertSessionShouldWaitForStatusBitmask( this.VisaSessionBase.Session );
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

    /// <summary>   (Unit Test Method) should wait for operation completion. </summary>
    /// <remarks>   David, 2021-07-04. </remarks>
    [TestMethod( "07. Session should wait for operation completion" )]
    public void VisaSessionShouldWaitForOperationCompletion()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        try
        {
            Asserts.AssertVisaSessionShouldOpen( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertSessionShouldWaitForOperationCompletion( this.VisaSessionBase.Session, this.VisaSessionBase.Session.OperationCompleteCommand );
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

    /// <summary>   (Unit Test Method) message available should wait for. </summary>
    /// <remarks>
    /// Initial Service Request Wait Complete Bitmask is 0x00<para>
    /// Initial Standard Event Enable Bitmask Is 0x00</para><para>
    /// DMM2002 status Byte 0x50 0x10 bitmask was Set</para>
    /// </remarks>
    [TestMethod( "08. Visa session should wait for the Message Available bitmask" )]
    public void VisaSessionShouldWaitForMessageAvailable()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );

        try
        {
            Asserts.AssertVisaSessionShouldOpen( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertSessionShouldWaitForMessageAvailable( this.VisaSessionBase.Session );
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

    /// <summary>
    /// (Unit Test Method) clear execution state should preserve enable bitmasks.
    /// </summary>
    /// <remarks>
    /// Initial Service Request Wait Complete Bitmask is 0x00<para>
    /// Initial Standard Event Enable Bitmask Is 0x00</para><para>
    /// DMM2002 status Byte 0x50 0x10 bitmask was Set</para>
    /// </remarks>
    [TestMethod( "09. Clear Execution State should preserve enable bitmasks" )]
    public void ClearExecutionStateShouldPreserveEnableBitmasks()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        Assert.AreEqual( this.VisaSessionBase.Session.StatusClearDistractive, this.VisaSessionBase.Session.ScpiExceptionsSettings.StatusClearDistractive );

        try
        {
            Asserts.AssertVisaSessionShouldOpen( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertNakedClsMightClearEnableBitmasks( this.VisaSessionBase.Session,
                this.VisaSessionBase.Session.RegistersBitmasksSettings.StandardEventEnableEventsBitmask,
                this.VisaSessionBase.Session.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );

            Asserts.AssertClearExecutionStatePreservesEnableBitmasks( this.VisaSessionBase.Session,
                this.VisaSessionBase.Session.RegistersBitmasksSettings.StandardEventEnableEventsBitmask,
                this.VisaSessionBase.Session.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );
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

    /// <summary>
    /// (Unit Test Method) clear execution state should preserve completion bitmasks.
    /// </summary>
    /// <remarks>   2024-09-16. </remarks>
    [TestMethod( "10. Clear Execution State should preserve completion bitmasks" )]
    public void ClearExecutionStateShouldPreserveCompletionBitmasks()
    {
        Assert.IsNotNull( this.VisaSessionBase );
        Assert.IsNotNull( this.VisaSessionBase.Session );
        Assert.AreEqual( this.VisaSessionBase.Session.StatusClearDistractive, this.VisaSessionBase.Session.ScpiExceptionsSettings.StatusClearDistractive );

        try
        {
            Asserts.AssertVisaSessionShouldOpen( this.VisaSessionBase, this.ResourceSettings );
            Asserts.AssertNakedClsMightClearEnableBitmasks( this.VisaSessionBase.Session,
                this.VisaSessionBase.Session.RegistersBitmasksSettings.StandardEventEnableOperationCompleteBitmask,
                this.VisaSessionBase.Session.RegistersBitmasksSettings.ServiceRequestEnableOperationCompleteBitmask );

            Asserts.AssertClearExecutionStatePreservesEnableBitmasks( this.VisaSessionBase.Session,
                this.VisaSessionBase.Session.RegistersBitmasksSettings.StandardEventEnableOperationCompleteBitmask,
                this.VisaSessionBase.Session.RegistersBitmasksSettings.ServiceRequestEnableOperationCompleteBitmask );
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
}
