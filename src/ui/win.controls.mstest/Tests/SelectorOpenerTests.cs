using System;

namespace cc.isr.VI.WinControls.MSTest;

/// <summary> A resource tests. </summary>
/// <remarks>
/// (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2017-10-11 </para>
/// </remarks>
[TestClass]
public class SelectorOpenerTests
{
    #region " construction and cleanup "

    /// <summary> Initializes the test class before running the first test. </summary>
    /// <remarks>
    /// Use <see cref="InitializeTestClass(TestContext)"/> to run code before running the first test in the class.
    /// </remarks>
    /// <param name="testContext"> Gets or sets the test context which provides information about
    /// and functionality for the current test run. </param>
    [ClassInitialize()]
    public static void InitializeTestClass( TestContext testContext )
    {
        string methodFullName = $"{testContext.FullyQualifiedTestClassName}.{System.Reflection.MethodBase.GetCurrentMethod()?.Name}";
        try
        {
            TraceListener = new( $"{testContext.FullyQualifiedTestClassName}.TraceListener",
                System.Diagnostics.SourceLevels.Warning );
            _ = System.Diagnostics.Trace.Listeners.Add( TraceListener );
            Trace.WriteLine( "Initializing", methodFullName );
        }
        catch ( Exception ex )
        {
            Trace.WriteLine( $"Failed initializing the test class: {ex}", methodFullName );

            // cleanup to meet strong guarantees
            try
            {
                CleanupTestClass();
            }
            finally
            {
            }

            throw;
        }
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
    public static void CleanupTestClass()
    {
        System.Diagnostics.Trace.Listeners.Remove( TraceListener );
        TraceListener?.Dispose();
    }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    [TestInitialize()]
    public void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( VI.WinControls.BindingEventHandlers ).Assembly.FullName}" );
        TraceListener?.ClearQueue();
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestCleanup()]
    public void CleanupAfterEachTest()
    {
        if ( TraceListener is not null && !TraceListener.Queue.IsEmpty )
            Assert.Fail( $"Errors or warnings were traced:{Environment.NewLine}{string.Join( Environment.NewLine, [.. TraceListener.Queue] )}" );
    }

    /// <summary>   Gets or sets the trace listener. </summary>
    /// <value> The trace listener. </value>
    private static Tracing.TestWriterQueueTraceListener? TraceListener { get; set; }

    /// <summary>
    /// Gets or sets the test context which provides information about and functionality for the
    /// current test run.
    /// </summary>
    /// <value> The test context. </value>
    public TestContext? TestContext { get; set; }

    #endregion

    #region " selector opener test "

    /// <summary>   (Unit Test Method) selector should enumerate resources. </summary>
    /// <remarks>   David, 2021-07-06. </remarks>
    [TestMethod()]
    public void SelectorShouldEnumerateResources()
    {
        Assert.IsNotNull( SessionFactory.Instance.Factory.ResourcesProvider() );
        Assert.IsNotNull( SessionFactory.Instance.Factory.ResourcesProvider().ResourceFinder );
        using isr.WinControls.SelectorOpener control = new();
        cc.isr.Std.Notifiers.SelectResourceBase selector = new SessionFactory()
        {
            Searchable = true,
            ResourcesFilter = SessionFactory.Instance.Factory.ResourcesProvider().ResourceFinder!.BuildMinimalResourcesFilter()
        };
        DeviceManager.AssertSelectorShouldEnumerateResources( control, selector );
    }

    #endregion
}
