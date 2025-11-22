using System.Text;
using cc.isr.VI.Device.Tsp.Tests;
using cc.isr.VI.Device.Tsp.Tests.Base;
using cc.isr.Std.ReaderExtensions;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.K2600.Tests.Tsp;

/// <summary>   (Unit Test Class) a tsp session script tests. </summary>
/// <remarks>   2025-04-10. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class TspSessionScriptTests : ScriptTests
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
        ScriptTests.InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup]
    public static void CleanupTestClass()
    {
        ScriptTests.CleanupBaseTestClass();
    }

    /// <summary>   Gets or sets the reference to the K2600 Device. </summary>
    /// <value> The reference to the K2600 Device. </value>
    private K2600Device? Device { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public override void InitializeBeforeEachTest()
    {
        base.DefineTraceListener();

        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        cc.isr.VI.Device.Tests.Asserts.AssertVisaImplementationShouldBeLoaded();
        Console.WriteLine( $"\tTesting {typeof( Script.ScriptInfo ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( TspSessionScriptTests ) );

        this.LocationSettings = Settings.AllSettings.Instance.LocationSettings;
        this.ResourceSettings = Settings.AllSettings.Instance.ResourceSettings;
        this.Device = K2600Device.Create();
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        this.Device.Session.ReadSettings( this.GetType(), overwriteAllUsersFile: true, overwriteThisUserFile: true );
        Assert.IsTrue( this.Device.Session.AllSettings.TimingSettings.Exists, $"{nameof( K2600Device )}.{nameof( K2600Device.Session )}.{nameof( K2600Device.Session.AllSettings.TimingSettings )} does not exist." );
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
    [TestMethod( DisplayName = "01. Script should load and run" )]
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

            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            // write the source to file.
            string fileTitle = $"{scriptName}_code";
            string filePath = Path.Combine( folderPath, $"{fileTitle}{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );
            if ( !scriptSource.ToString().TryExportToFile( filePath, true, true, out string details ) )
                Assert.Fail( details );

            this.Device.Session.DeleteScript( scriptName, true );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            string outFilePath = Path.Combine( folderPath, $"{fileTitle}_trimmed{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );
            TestBase.ConsoleOutputMemberMessage( $"Trimming script file to '{outFilePath}'" );
            filePath.TrimScript( outFilePath, true );

            TestBase.ConsoleOutputMemberMessage( $"Importing script from trimmed '{outFilePath}' file" );
            this.Device.Session.ImportScript( scriptName, outFilePath, TimeSpan.Zero, false, true );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, false, true );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            TestBase.ConsoleOutputMemberMessage( $"Converting loaded script to byte code format" );
            this.Device.Session.ConvertToByteCode( scriptName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RemoveEmbeddedScript( scriptName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, true, false );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            filePath = Path.Combine( folderPath, $"{fileTitle}_exported{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension}" );
            TestBase.ConsoleOutputMemberMessage( $"Exporting byte code script to '{filePath}' file." );
            if ( !this.Device.Session.TryExportScript( scriptName, filePath, true, out details ) )
                Assert.Fail( details );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.DeleteScript( scriptName, true );
            Asserts.AssertMessageQueue();
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
                this.Device.Session.DeleteScript( scriptName, true );
                Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
                Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
            }
        }
    }

    /// <summary>   (Unit Test Method) script should load and run from file. </summary>
    /// <remarks>   2025-04-10. </remarks>
    [TestMethod( DisplayName = "02. Script should load and run from file" )]
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

            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            // write the source to file.
            fileTitle = $"{fileTitle}_file";
            string filePath = Path.Combine( folderPath, $"{fileTitle}{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );
            if ( !scriptSource.ToString().TryExportToFile( filePath, true, true, out string details ) )
                Assert.Fail( details );

            this.Device.Session.DeleteScript( scriptName, true );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            string outputFilePath = Path.Combine( folderPath, $"{fileTitle}_trimmed{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );
            TestBase.ConsoleOutputMemberMessage( $"Trimming script file to '{outputFilePath}'" );
            filePath.TrimScript( outputFilePath, true );

            TestBase.ConsoleOutputMemberMessage( $"Importing script from trimmed '{outputFilePath}' file" );
            this.Device.Session.ImportScript( scriptName, outputFilePath, TimeSpan.Zero, false, true );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, false, true );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            filePath = Path.Combine( folderPath, $"{fileTitle}_exported{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}" );

            if ( !this.Device.Session.TryExportScript( scriptName, filePath, true, out details ) )
                Assert.Fail( details );

            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.ConvertToByteCode( scriptName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RemoveEmbeddedScript( scriptName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, true, false );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            if ( string.Equals( "2600A", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "_2600A";
            else if ( string.Equals( "2600B", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "_2600B";
            else
                Assert.Fail( $"The model family {this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily} is not supported." );

            filePath = Path.Combine( folderPath, $"{fileTitle}_exported{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension}" );

            if ( !this.Device.Session.TryExportScript( scriptName, filePath, true, out details ) )
                Assert.Fail( details );

            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.DeleteScript( scriptName, true );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
        }
        catch
        {
            throw;
        }
        finally
        {
            if ( this.Device.Session is not null && this.Device.Session.IsSessionOpen && this.Device.Session.IsScriptExists( scriptName, out _ ) )
            {
                this.Device.Session.DeleteScript( scriptName, true );
                Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
                Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
            }
        }
    }

    /// <summary>   (Unit Test Method) byte code script should load and run from file. </summary>
    /// <remarks>   2025-04-10. </remarks>
    [TestMethod( DisplayName = "03. Byte code script should load and run from file" )]
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

            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

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
            if ( !scriptSource.TryExportToFile( filePath, true, true, out string details ) )
                Assert.Fail( details );

            this.Device.Session.DeleteScript( scriptName, true );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            TestBase.ConsoleOutputMemberMessage( $"Importing script from binary '{filePath}' file" );
            this.Device.Session.ImportScript( scriptName, filePath, TimeSpan.Zero, false, false );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.EmbedScript( scriptName, true, true );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            filePath = Path.Combine( folderPath, $"{fileTitle}_exported{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension}" );

            TestBase.ConsoleOutputMemberMessage( $"Exporting to byte code '{filePath}' file" );
            if ( !this.Device.Session.TryExportScript( scriptName, filePath, true, out details ) )
                Assert.Fail( details );

            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.DeleteScript( scriptName, true );
            Asserts.AssertMessageQueue();
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
                Assert.IsTrue( this.Device.Session.TryDeleteScript( scriptName, true, out string details ), details );
                Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
                Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
            }
        }
    }

    #endregion

    #region " function tests "

    /// <summary>   (Unit Test Method) Assert that the Lua syntax should call function. </summary>
    /// <remarks>   2024-08-20. </remarks>
    [TestMethod( DisplayName = "04. Lua syntax should call function" )]
    public void LuaSyntaxShouldCallFunction()
    {
        Assert.IsNotNull( this.Device, $"{nameof( K2600Device )} must not be null." );
        Assert.IsNotNull( this.Device.Session, $"{nameof( K2600Device )}.{nameof( K2600Device.Session )} must not be null." );
        Assert.IsNotNull( this.Device.StatusSubsystemBase, $"{nameof( K2600Device )}.{nameof( K2600Device.StatusSubsystemBase )} must not be null." );
        int firstArgument = 1;
        int secondArgument = 2;
        string expectedSum = $"{firstArgument} + {secondArgument} = {firstArgument + secondArgument}\n";
        // expectedResult = "1 + 2 = 3\n"
        string functionName = "sum";
        string functionArguments = $"{firstArgument},{secondArgument}";
        StringBuilder stringBuilder = new();
        _ = stringBuilder.AppendLine( "function sum ( ... )" );
        _ = stringBuilder.AppendLine( "  local result = 0" );
        _ = stringBuilder.AppendLine( $"  for index, value in {Lexemes.Lua.IPairsLexeme}( arg ) do" );
        _ = stringBuilder.AppendLine( "     result = result + value" );
        _ = stringBuilder.AppendLine( "  end" );
        _ = stringBuilder.AppendLine( "  print ( table.concat( arg , \" + \" ) .. \" = \" .. result )" );
        _ = stringBuilder.AppendLine( "end" );

        try
        {
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );
            try
            {
                Asserts.AssertFunctionShouldBeCalled( this.Device.Session, functionName,
                    stringBuilder.ToString(), functionArguments, expectedSum, Settings.AllSettings.Instance.TspScriptSettings.CallFunctionResultDelay );
            }
            catch ( Exception )
            {
                throw;
            }
            finally
            {
                // remove the function
                Asserts.AssertFunctionShouldNull( this.Device.Session, functionName );
            }
        }
        catch
        {
            throw;
        }
        finally
        {
            Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    #endregion

    #region " script tests "

    /// <summary>
    /// (Unit Test Method) script auto run state should be revered and restored to its initial state.
    /// </summary>
    /// <remarks>   2024-10-03. </remarks>
    [TestMethod( DisplayName = "05. Auto run should be toggled" )]
    public void ScriptAutoRunShouldToggle()
    {
        Assert.IsNotNull( this.Device, $"{nameof( K2600Device )} must not be null." );
        Assert.IsNotNull( this.Device.Session, $"{nameof( K2600Device )}.{nameof( K2600Device.Session )} must not be null." );
        try
        {
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );
            string scriptName = string.Empty;
            try
            {
                (scriptName, string scriptFunctionName) = this.Device.Session.LoadTimeDisplayClearScript();

                if ( !this.Device.Session.IsNil( scriptName ) )
                {
                    // script should run and create the function.
                    this.Device.Session.RunScript( scriptName, scriptFunctionName );

                    TestBase.ConsoleOutputMemberMessage( $"Toggling auto run for the '{scriptName}' script." );
                    bool initialState = this.Device.Session.IsAutoRun( scriptName );
                    if ( initialState )
                    {
                        TestBase.ConsoleOutputMemberMessage( $"'{scriptName}' script auto run was 'yes'." );
                        this.Device.Session.TurnOffAutoRun( scriptName );
                        Assert.IsFalse( this.Device.Session.IsAutoRun( scriptName ), "Auto run should be turned off." );
                        TestBase.ConsoleOutputMemberMessage( $"'{scriptName}' script auto run is 'no'." );
                    }
                    else
                    {
                        TestBase.ConsoleOutputMemberMessage( $"'{scriptName}' script auto run was 'no'." );
                        this.Device.Session.TurnOnAutoRun( scriptName );
                        Assert.IsTrue( this.Device.Session.IsAutoRun( scriptName ), "Auto run should be turned on." );
                        TestBase.ConsoleOutputMemberMessage( $"'{scriptName}' script auto run is 'yes'." );
                    }

                    // restore initial state.
                    if ( initialState )
                    {
                        this.Device.Session.TurnOnAutoRun( scriptName );
                        Assert.IsTrue( this.Device.Session.IsAutoRun( scriptName ), "Auto run should be restored." );
                        TestBase.ConsoleOutputMemberMessage( $"'{scriptName}' script auto run is 'yes'." );
                    }
                    else
                    {
                        this.Device.Session.TurnOffAutoRun( scriptName );
                        Assert.IsFalse( this.Device.Session.IsAutoRun( scriptName ), "Auto run should be restored." );
                        TestBase.ConsoleOutputMemberMessage( $"'{scriptName}' script auto run is 'no'." );
                    }
                }
                else
                    TestBase.ConsoleOutputMemberMessage( $"'{scriptName}' script not found." );
            }
            catch
            {
                throw;
            }
            finally
            {
                Assert.IsTrue( this.Device.Session.TryDeleteScript( scriptName, true, out string details ), details );
            }
        }
        catch
        {
            throw;
        }
        finally
        {
            Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    #endregion
}
