using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using cc.isr.Std.Tests;
using cc.isr.Std.Tests.Extensions;
using cc.isr.VI.Tsp.Script.ExportExtensions;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;
using cc.isr.VI.Tsp.Script;
using cc.isr.VI.Device.Tests.Base;

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
            // initialize the logger.
            _ = TspScriptTests.Logger?.BeginScope( methodFullName );
        }
        catch ( Exception ex )
        {
            if ( TspScriptTests.Logger is null )
                Trace.WriteLine( $"Failed initializing the test class: {ex}", methodFullName );
            else
            {
                TspScriptTests.Logger.LogExceptionMultiLineMessage( "Failed initializing the test class:", ex );
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
        Console.WriteLine( $"\tTesting {typeof( cc.isr.VI.Tsp.Script.ScriptCompressor ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( TspScriptTests ) );

        if ( TspScriptTests.Logger is not null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<TspScriptTests>( TspScriptTests.Logger );
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
    [TestMethod( "01. Script compressor should compress" )]
    public void ScriptCompressorShouldCompress()
    {
        string contents = "StringStringStringStringStringStringStringStringStringStringStringStringStringString -- end comment";
        contents = contents[..contents.IndexOf( Syntax.Tsp.Lua.CommentChunk, StringComparison.OrdinalIgnoreCase )].TrimEnd();
        string compressed = cc.isr.VI.Tsp.Script.ScriptCompressor.Compress( contents );
        string decompressed = cc.isr.VI.Tsp.Script.ScriptCompressor.Decompress( compressed );
        Assert.AreEqual( contents, decompressed );
    }

    /// <summary>   (Unit Test Method) script should trim and compress. </summary>
    /// <remarks>   2025-04-16. </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    [TestMethod( "02. Script should trim and compress" )]
    public void ScriptShouldTrimAndCompress()
    {
        string scriptName = "timeDisplayClear";
        string folderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
        string timerElapsedFunctionName = "timerElapsed";
        string timeDisplayFunctionName = "timeDisplay";
        StringBuilder scriptSource = new();
        _ = scriptSource.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Script.LoadScriptCommand} {scriptName}" );
        _ = scriptSource.AppendLine( "do" );
        _ = scriptSource.AppendLine( $"  {timerElapsedFunctionName}=function() return timer.measure.t() end" );
        _ = scriptSource.AppendLine( $"  {timeDisplayFunctionName}=function()" );
        _ = scriptSource.AppendLine( "    timer.reset()" );
        _ = scriptSource.AppendLine( "    display.clear()" );
        _ = scriptSource.AppendLine( $"    local elapsed = {timerElapsedFunctionName}()" );
        _ = scriptSource.AppendLine( "    print( \"display clear elapsed \" .. 1000 * elapsed .. \"ms\" )" );
        _ = scriptSource.AppendLine( "    timer.reset()" );
        _ = scriptSource.AppendLine( "  end" );
        _ = scriptSource.AppendLine( "end" );
        _ = scriptSource.AppendLine( cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand );

        // write the source to file.
        string fileTitle = $"{scriptName}_code";
        string filePath = Path.Combine( folderPath, $"{fileTitle}.tsp" );
        scriptSource.ToString().ExportScript( filePath, overWrite: true );

        string toFilePath = Path.Combine( folderPath, $"{fileTitle}_trimmed.tsp" );
        TestBase.ConsoleOutputMemberMessage( $"Trimming script file to '{toFilePath}'" );
        filePath.TrimScript( toFilePath, true );

        toFilePath = Path.Combine( folderPath, $"{toFilePath}c" );
        TestBase.ConsoleOutputMemberMessage( $"Compressing '{filePath}'\r\n\t\tto '{toFilePath}'" );
        System.IO.File.WriteAllText( toFilePath, ScriptCompressor.Compress( System.IO.File.ReadAllText( filePath ) ), System.Text.Encoding.Default );
    }

}
