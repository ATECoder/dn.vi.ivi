using System.Text;
using cc.isr.VI.Device.Tests.Base;
using cc.isr.VI.Tsp.Script.ExportExtensions;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.K2600.MSTest.Tsp;

/// <summary>   (Unit Test Class) a tsp session script tests. </summary>
/// <remarks>   2025-04-10. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class TspSessionScriptTests : Device.Tests.Base.ScriptTests
{
    #region " construction and cleanup "

    /// <summary>   Initializes the test class before running the first test. </summary>
    /// <remarks>
    /// Use <see cref="InitializeTestClass(TestContext)"/> to run code before running the first test
    /// in the class.
    /// </remarks>
    /// <param name="testContext">  Gets or sets the test context which provides information about
    ///                             and functionality for the current test run. </param>
    [ClassInitialize()]
    public static void InitializeTestClass( TestContext testContext )
    {
        VI.Device.Tests.Base.SubsystemsTests.InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
    public static void CleanupTestClass()
    {
        VI.Device.Tests.Base.SubsystemsTests.CleanupBaseTestClass();
    }

    /// <summary>   Gets or sets the reference to the K2600 Device. </summary>
    /// <value> The reference to the K2600 Device. </value>
    private K2600Device? Device { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public override void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        cc.isr.VI.Device.Tests.Asserts.AssertVisaImplementationShouldBeLoaded();
        Console.WriteLine( $"\tTesting {typeof( cc.isr.VI.Tsp.Script.ScriptInfo ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( TspSessionScriptTests ) );

        this.TestSiteSettings = Settings.AllSettings.Instance.TestSiteSettings;
        this.ResourceSettings = Settings.AllSettings.Instance.ResourceSettings;
        this.Device = K2600Device.Create();
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        this.Device.Session.ReadSettings( this.GetType().Assembly, ".Session", true, true );
        Assert.IsTrue( this.Device.Session.TimingSettings.Exists, $"{nameof( K2600Device )}.{nameof( K2600Device.Session )}.{nameof( K2600Device.Session.TimingSettings )} does not exist." );
        this.VisaSessionBase = this.Device;
        base.InitializeBeforeEachTest();
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestCleanup()]
    public override void CleanupAfterEachTest()
    {
        base.CleanupAfterEachTest();
        this.Device?.Dispose();
        this.Device = null;
        SessionLogger.CloseAndFlush();
    }

    #endregion

    #region " load and run "

    /// <summary> (Unit Test Method) tests visa resource. </summary>
    /// <remarks> Finds the resource using the session factory resources manager. </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    [TestMethod( "01. Script should load and run" )]
    public void ScriptShouldLoadAndRun()
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );

        string scriptName = "timeDisplayClear";
        try
        {
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

            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            // write the source to file.
            string fileTitle = $"{scriptName}_code";
            string filePath = Path.Combine( folderPath, $"{fileTitle}{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );
            scriptSource.ToString().ExportScript( filePath, overWrite: true );

            this.Device.Session.DeleteScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            string outFilePath = Path.Combine( folderPath, $"{fileTitle}_trimmed{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );
            TestBase.ConsoleOutputMemberMessage( $"Trimming script file to '{outFilePath}'" );
            filePath.TrimScript( outFilePath, true );

            TestBase.ConsoleOutputMemberMessage( $"Importing script from trimmed '{outFilePath}' file" );
            this.Device.Session.ImportScript( scriptName, outFilePath, TimeSpan.Zero, false, true, false );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, false, true );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            TestBase.ConsoleOutputMemberMessage( $"Converting loaded script to byte code format" );
            this.Device.Session.ConvertToByteCode( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RemoveEmbeddedScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, true, false );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            filePath = Path.Combine( folderPath, $"{fileTitle}_exported{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension}" );
            TestBase.ConsoleOutputMemberMessage( $"Exporting byte code script to '{filePath}' file." );
            this.Device.Session.ExportScript( scriptName, filePath, true );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.DeleteScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

        }
        catch
        {
            throw;
        }
        finally
        {
            if ( this.Device.Session is not null && this.Device.Session.IsSessionOpen )
            {
                this.Device.Session.DeleteScript( scriptName );
                cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
                cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
            }
        }
    }

    /// <summary>   (Unit Test Method) script should load and run from file. </summary>
    /// <remarks>   2025-04-10. </remarks>
    [TestMethod( "02. Script should load and run from file" )]
    public void ScriptShouldLoadAndRunFromFile()
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        Assert.IsNotNull( this.Device.StatusSubsystemBase );

        string scriptName = "timeDisplayClear";
        try
        {
            string folderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
            string fileTitle = "timeDisplayClear";
            string timerElapsedFunctionName = "timerElapsed";
            string scriptSource = System.IO.File.ReadAllText( System.IO.Path.Combine( folderPath, fileTitle + cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension ) );

            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            // write the source to file.
            fileTitle = $"{fileTitle}_file";
            string filePath = Path.Combine( folderPath, $"{fileTitle}{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );
            scriptSource.ToString().ExportScript( filePath, overWrite: true );

            this.Device.Session.DeleteScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            string outputFilePath = Path.Combine( folderPath, $"{fileTitle}_trimmed{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );
            TestBase.ConsoleOutputMemberMessage( $"Trimming script file to '{outputFilePath}'" );
            filePath.TrimScript( outputFilePath, true );

            TestBase.ConsoleOutputMemberMessage( $"Importing script from trimmed '{outputFilePath}' file" );
            this.Device.Session.ImportScript(scriptName, outputFilePath, TimeSpan.Zero, false, true, false);
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, false, true );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            filePath = Path.Combine( folderPath, $"{fileTitle}_exported{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );

            this.Device.Session.ExportScript( scriptName, filePath, true );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.ConvertToByteCode( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RemoveEmbeddedScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, true, false );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            if ( string.Equals( "2600A", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "_2600A";
            else if ( string.Equals( "2600B", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "_2600B";
            else
                Assert.Fail( $"The model family {this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily} is not supported." );

            filePath = Path.Combine( folderPath, $"{fileTitle}_exported{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension}" );

            this.Device.Session.ExportScript( scriptName, filePath, true );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.DeleteScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
        }
        catch
        {
            throw;
        }
        finally
        {
            if ( this.Device.Session is not null && this.Device.Session.IsSessionOpen )
            {
                this.Device.Session.DeleteScript( scriptName );
                cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
                cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
            }
        }
    }

    /// <summary>   (Unit Test Method) byte code script should load and run from file. </summary>
    /// <remarks>   2025-04-10. </remarks>
    [TestMethod( "03. byte code script should load and run from file" )]
    public void BinaryScriptShouldLoadAndRunFromFile()
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        Assert.IsNotNull( this.Device.StatusSubsystemBase );

        string scriptName = "timeDisplayClear";
        try
        {
            string folderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
            string fileTitle = "timeDisplayClear";
            string timerElapsedFunctionName = "timerElapsed";

            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            if ( string.Equals( "2600A", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "_2600A";
            else if ( string.Equals( "2600B", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "_2600B";
            else
                Assert.Fail( $"The model family {this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily} is not supported." );

            string scriptSource = System.IO.File.ReadAllText( System.IO.Path.Combine( folderPath, fileTitle + cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension ) );

            // tag the file as imported as byte code
            // write the source to file.
            fileTitle += "_byte_code";
            string filePath = Path.Combine( folderPath, $"{fileTitle}{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension}" );
            scriptSource.ToString().ExportScript( filePath, overWrite: true );

            this.Device.Session.DeleteScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            TestBase.ConsoleOutputMemberMessage( $"Importing script from binary '{filePath}' file" );
            this.Device.Session.ImportScript( scriptName, filePath, TimeSpan.Zero, false, false, false );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, true, true );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            filePath = Path.Combine( folderPath, $"{fileTitle}_exported{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension}" );

            TestBase.ConsoleOutputMemberMessage( $"Exporting to byte code '{filePath}' file" );
            this.Device.Session.ExportScript( scriptName, filePath, true );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.DeleteScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
        }
        catch
        {
            throw;
        }
        finally
        {
            if ( this.Device.Session is not null && this.Device.Session.IsSessionOpen )
            {
                this.Device.Session.DeleteScript( scriptName );
                cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
                cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
            }
        }
    }

    #endregion
}
