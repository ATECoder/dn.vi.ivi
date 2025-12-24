using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using cc.isr.Std;
using cc.isr.Std.Logging;
using cc.isr.VI.Tsp.Script;
using Microsoft.Extensions.Logging;

namespace cc.isr.VI.Tsp.K2600.Tests.Tsp;

/// <summary>   (Unit Test Class) a tsp session script tests. </summary>
/// <remarks>   2025-04-10. </remarks>
[TestClass]
public class TspScriptTests
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
            TraceListener = new Tracing.TestWriterQueueTraceListener( $"{testContext.FullyQualifiedTestClassName}.TraceListener",
                SourceLevels.Warning );
            _ = Trace.Listeners.Add( TraceListener );
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"Failed initializing the test class: {ex}", methodFullName );

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
    [ClassCleanup]
    public static void CleanupTestClass()
    {
        Trace.Listeners.Remove( TraceListener );
        TraceListener?.Dispose();
    }

    private IDisposable? _loggerScope;

    /// <summary> Initializes the test class instance before each test runs. </summary>
    [TestInitialize()]
    public void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {TimeZoneInfo.Local}" );

        this._loggerScope = Logger?.BeginScope( this.TestContext?.TestName ?? string.Empty );
        if ( TraceListener is not null && !TraceListener.Queue.IsEmpty )
            Assert.Fail( $"Errors or warnings were traced:{Environment.NewLine}{string.Join( Environment.NewLine, [.. TraceListener.Queue] )}" );
        TraceListener?.ClearQueue();
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestCleanup()]
    public void CleanupAfterEachTest()
    {
        this._loggerScope?.Dispose();
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

    /// <summary>   Gets a logger instance for this category. </summary>
    /// <value> The logger. </value>
    public static ILogger<TspScriptTests>? Logger { get; } = LoggerProvider.CreateLogger<TspScriptTests>();

    #endregion

    #region " test initialization tests "

    /// <summary>   (Unit Test Method) Assets that tests should initialize. </summary>
    /// <remarks>   2024-08-17. </remarks>
    [TestMethod( DisplayName = "00. Init: Tests should initialize" )]
    public void TestsShouldInitialize()
    {
    }

    #endregion

    #region " Trim "

    /// <summary>   (Unit Test Method) tsp script should be trimmed to file. </summary>
    /// <remarks>   2025-04-11. </remarks>
    [TestMethod( DisplayName = "01. TSP script should be trimmed" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    public void TspScriptShouldBeTrimmed()
    {
        Assembly assembly = typeof( cc.isr.VI.Tsp.Script.ScriptInfo ).Assembly;
        Console.WriteLine( $"\t{assembly.FullName} {assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName}" );
        assembly = typeof( cc.isr.VI.Syntax.Tsp.TspScriptParser ).Assembly;
        Console.WriteLine( $"\t{assembly.FullName} {assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName}" );

        string sourceFolderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
        string sourceFileTitle = "timeDisplayClear";
        string trimmedFileTitle = "timeDisplayClear_Trimmed";
        string fileExtension = cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension;
        string source = System.IO.File.ReadAllText( System.IO.Path.Combine( sourceFolderPath, sourceFileTitle + fileExtension ) );
        string trimmed = cc.isr.VI.Syntax.Tsp.TspScriptParser.TrimTspSourceCode( source, true );
        string expectedTrimmed = System.IO.File.ReadAllText( System.IO.Path.Combine( sourceFolderPath, trimmedFileTitle + fileExtension ) );
        StringBuilder builder = new();
        _ = builder.Append( $"The trimmed source should equal the trimmed source from the expected file:" );
        _ = builder.AppendLine( $"\texpected trimmed: {System.IO.Path.Combine( sourceFolderPath, trimmedFileTitle + fileExtension )}" );
        _ = builder.AppendLine( $"\toriginal source: {System.IO.Path.Combine( sourceFolderPath, sourceFileTitle + fileExtension )}" );
        Assert.AreEqual( expectedTrimmed, trimmed, builder.ToString() );
    }

    /// <summary>   (Unit Test Method) tsp byte code script should be trimmed. </summary>
    /// <remarks>   2025-04-11. </remarks>
    [TestMethod( DisplayName = "02. TSP byte code script should be trimmed" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    public void TspByteCodeScriptShouldBeTrimmed()
    {
        Assembly assembly = typeof( cc.isr.VI.Tsp.Script.ScriptInfo ).Assembly;
        Console.WriteLine( $"\t{assembly.FullName} {assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName}" );
        assembly = typeof( cc.isr.VI.Syntax.Tsp.TspScriptParser ).Assembly;
        Console.WriteLine( $"\t{assembly.FullName} {assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName}" );

        string sourceFolderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
        string sourceFileTitle = "timeDisplayClear_2600B";
        string trimmedFileTitle = "timeDisplayClear_2600B_Trimmed";
        string fileExtension = ScriptInfo.ScriptByteCodeFileExtension;
        string source = System.IO.File.ReadAllText( System.IO.Path.Combine( sourceFolderPath, sourceFileTitle + fileExtension ) );
        string trimmed = cc.isr.VI.Syntax.Tsp.TspScriptParser.TrimTspSourceCode( source, false );
        string expectedTrimmed = System.IO.File.ReadAllText( System.IO.Path.Combine( sourceFolderPath, trimmedFileTitle + fileExtension ) );
        StringBuilder builder = new();
        _ = builder.Append( $"The trimmed source should equal the trimmed source from the expected file:" );
        _ = builder.AppendLine( $"\texpected trimmed: {System.IO.Path.Combine( sourceFolderPath, trimmedFileTitle + fileExtension )}" );
        _ = builder.AppendLine( $"\toriginal source: {System.IO.Path.Combine( sourceFolderPath, sourceFileTitle + fileExtension )}" );
        Assert.AreEqual( expectedTrimmed, trimmed, builder.ToString() );
    }

    #endregion

    #region " script format "

    /// <summary>
    /// (Unit Test Method) script information should return expected file extension.
    /// </summary>
    /// <remarks>   2025-09-18. </remarks>
    /// <param name="format">   Describes the format to use. </param>
    /// <param name="expected"> The expected. </param>
    [DataRow( FileFormats.None, ScriptInfo.ScriptFileExtension )]
    [DataRow( FileFormats.Encrypted, ScriptInfo.ScriptEncryptedFileExtension )]
    [DataRow( FileFormats.Compressed, ScriptInfo.ScriptCompressedFileExtension )]
    [DataRow( FileFormats.Compressed | FileFormats.Encrypted, ScriptInfo.ScriptCompressedEncryptedFileExtension )]
    [DataRow( FileFormats.ByteCode, ScriptInfo.ScriptByteCodeFileExtension )]
    [DataRow( FileFormats.ByteCode | FileFormats.Encrypted, ScriptInfo.ScriptByteCodeEncryptedFileExtension )]
    [DataRow( FileFormats.ByteCode | FileFormats.Compressed, ScriptInfo.ScriptByteCodeCompressedFileExtension )]
    [DataRow( FileFormats.ByteCode | FileFormats.Compressed | FileFormats.Encrypted, ScriptInfo.ScriptByteCodeCompressedEncryptedFileExtension )]
    [TestMethod()]
    public void ScriptFormatShouldParse( FileFormats format, string expected )
    {
        Assembly assembly = typeof( cc.isr.VI.Tsp.Script.ScriptInfo ).Assembly;
        Console.WriteLine( $"\t{assembly.FullName} {assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName}" );

        string result = ScriptInfo.SelectScriptFileExtension( format );
        Assert.AreEqual( expected, result, $".. for {format}" );
    }

    #endregion
}
