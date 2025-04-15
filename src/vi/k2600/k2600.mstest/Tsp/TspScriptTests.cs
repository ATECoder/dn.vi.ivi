using System;
using System.Diagnostics;
using cc.isr.Std.Tests;
using cc.isr.Std.Tests.Extensions;
using Microsoft.Extensions.Logging;

namespace cc.isr.VI.Tsp.K2600.MSTest.Tsp;

/// <summary>   (Unit Test Class) a tsp script tests. </summary>
/// <remarks>   2025-04-10. </remarks>
[TestClass]
public class TspScriptTests
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
            {
                Logger.LogExceptionMultiLineMessage( "Failed initializing the test class:", ex );
            }

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
    public LoggerTraceListener<TspScriptTests>? TraceListener { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    public void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( cc.isr.VI.Tsp.Script.ScriptCompressor ).Assembly.FullName}" );

        // create an instance of the Serilog logger.
        SessionLogger.Instance.CreateSerilogLogger( typeof( TspScriptTests ) );

        if ( Logger is not null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<TspScriptTests>( Logger );
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
    public static ILogger<TspScriptTests>? Logger { get; } = LoggerProvider.CreateLogger<TspScriptTests>();

    #endregion

    /// <summary>   (Unit Test Method) string compressor should compress. </summary>
    /// <remarks>   2025-04-08. </remarks>
    [TestMethod( "01. Script Compressor Should Compress" )]
    public void ScriptCompressorShouldCompress()
    {
        string contents = "StringStringStringStringStringStringStringStringStringStringStringStringStringString";
        string compressed = cc.isr.VI.Tsp.Script.ScriptCompressor.Compress( contents );
        string decompressed = cc.isr.VI.Tsp.Script.ScriptCompressor.Decompress( compressed );
        Assert.AreEqual( contents, decompressed );
    }
}
