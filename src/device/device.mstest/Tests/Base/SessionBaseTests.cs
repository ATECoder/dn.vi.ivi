using cc.isr.Std.Tests;
using cc.isr.Std.Tests.Extensions;

namespace cc.isr.VI.Device.Tests.Base;

/// <summary> the abstract session base tests. </summary>
/// <remarks>
/// (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2017-10-11 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class SessionBaseTests
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
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    public static void CleanupBaseTestClass()
    { }

    private IDisposable? _loggerScope;

    /// <summary>   Gets or sets the trace listener. </summary>
    /// <value> The trace listener. </value>
    public LoggerTraceListener<SessionBaseTests>? TraceListener { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    public virtual void InitializeBeforeEachTest()
    {
        // assert reading of test settings from the configuration file
        Assert.IsNotNull( this.TestSiteSettings );
        Assert.IsTrue( this.TestSiteSettings.Exists );
        double expectedUpperLimit = 12d;
        Assert.IsLessThan( expectedUpperLimit,
            Math.Abs( this.TestSiteSettings.TimeZoneOffset() ), $"{nameof( this.TestSiteSettings.TimeZoneOffset )} should be lower than {expectedUpperLimit}" );

        if ( Logger is not null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<SessionBaseTests>( Logger );
            _ = Trace.Listeners.Add( this.TraceListener );
        }
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    public virtual void CleanupAfterEachTest()
    {
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
    public static ILogger<SessionBaseTests>? Logger { get; } = LoggerProvider.CreateLogger<SessionBaseTests>();

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    protected TestSiteSettings? TestSiteSettings { get; set; }

    #endregion

    #region " session termination "

    /// <summary> (Unit Test Method) tests initial termination. </summary>
    [TestMethod( DisplayName = "01. Termination character should be line feed" )]
    public void TerminationCharacterShouldBeLineFeed()
    {
        using Pith.SessionBase session = SessionFactory.Instance.Factory.Session();
        Assert.AreEqual( Environment.NewLine.ToCharArray()[1], session.TerminationCharacters().ElementAtOrDefault( 0 ),
            "Initial termination character set to line feed" );
    }

    /// <summary>   (Unit Test Method)Assert that a new termination could be set. </summary>
    /// <remarks>   2024-08-30. </remarks>
    [TestMethod( DisplayName = "02. New termination character could be set" )]
    public void NewTerminationCharacterCouldBeSet()
    {
        using Pith.SessionBase session = SessionFactory.Instance.Factory.Session();
        char[] values = Environment.NewLine.ToCharArray();
        session.NewTermination( values );
        Assert.HasCount<char>( values.Length, session.TerminationCharacters() );
        for ( int i = 0, loopTo = values.Length - 1; i <= loopTo; i++ )
            Assert.AreEqual( values[i], session.TerminationCharacters().ElementAtOrDefault( i ) );
    }

    #endregion
}
