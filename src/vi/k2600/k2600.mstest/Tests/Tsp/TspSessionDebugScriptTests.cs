using cc.isr.VI.Device.Tsp.Tests;
using cc.isr.VI.Device.Tsp.Tests.Base;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.K2600.Tests.Tsp;

/// <summary>   (Unit Test Class) a tsp session script tests. </summary>
/// <remarks>   2025-04-10. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class TspSessionDebugScriptTests : Device.Tsp.Tests.Base.ScriptTests
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
    [ClassCleanup]
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
        base.DefineTraceListener();

        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        cc.isr.VI.Device.Tests.Asserts.AssertVisaImplementationShouldBeLoaded();
        Console.WriteLine( $"\tTesting {typeof( Script.ScriptInfo ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( TspSessionDebugScriptTests ) );

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

    #region " methods "

    /// <summary>
    /// Assert script should import and run. This method deletes the existing <paramref name="scriptName"/>
    /// before importing and upon exit.
    /// </summary>
    /// <remarks>   2025-04-20. </remarks>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Name of the script. </param>
    /// <param name="filePath">                 Full pathname of the file. </param>
    /// <param name="scriptFunctionName">       Name of the script function. </param>
    /// <param name="functionExpectedValue">    The function expected value. </param>
    /// <param name="deleteOnExit">             True to delete the on exit. </param>
    public static void AssertScriptShouldImportAndRun( Pith.SessionBase session, string scriptName,
        string filePath, string scriptFunctionName, string functionExpectedValue, bool deleteOnExit )
    {
        Assert.IsNotNull( session );
        Assert.IsTrue( session.IsDeviceOpen );

        try
        {
            TestBase.ConsoleOutputMemberMessage( $"Importing script from '{filePath}' file" );
            session.DeleteScript( scriptName, true );
            session.ImportScript( scriptName, filePath, TimeSpan.Zero, false, false );
            // session.ImportScript( scriptName, filePath );
            session.RunScript( scriptName, scriptFunctionName );

            string actualFunctionValue = session.QueryTrimEnd( $"_G.print( {scriptFunctionName}() )" );
            Assert.AreEqual( functionExpectedValue, actualFunctionValue );
        }
        catch
        {
            throw;
        }
        finally
        {
            if ( deleteOnExit && session is not null && session.IsSessionOpen )
                session.DeleteScript( scriptName, true );
        }
    }

    #endregion

    #region " load and run "

    /// <summary>   (Unit Test Method) script should import and run. </summary>
    /// <remarks>   2025-04-20. </remarks>
    [TestMethod( DisplayName = "01. Script should import and run" )]
    public void ScriptShouldImportAndRun()
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        Assert.IsNotNull( this.Device.StatusSubsystemBase );

        try
        {
            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            string inFolderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
            string fileTitle = "timeDisplayClearFunctions";
            string scriptName = fileTitle;
            // string timerElapsedFunctionName = "timerElapsed";
            string versionFunctionName = "getVersion";
            string versionValue = "2.4.9181";
            string inFilePath = System.IO.Path.Combine( inFolderPath, fileTitle + cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension );

            TspSessionDebugScriptTests.AssertScriptShouldImportAndRun( this.Device.Session, scriptName, inFilePath, versionFunctionName, versionValue, true );

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
                Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
            }
        }
    }

    /// <summary>   (Unit Test Method) byte code script should import and run. </summary>
    /// <remarks>   2025-04-20. </remarks>
    [TestMethod( DisplayName = "02. byte code script should import and run" )]
    public void BinaryScriptShouldImportAndRun()
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        Assert.IsNotNull( this.Device.StatusSubsystemBase );

        try
        {

            Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            string outFolderPath = cc.isr.Std.PathExtensions.PathMethods.GetTempPath( ["~cc.isr", "vi.tsp.k2600",
                nameof( TspSessionDebugScriptTests ), nameof( TspSessionDebugScriptTests.ScriptShouldImportAndRun )] );

            string inFolderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
            string fileTitle = "timeDisplayClearFunctions";
            string scriptName = fileTitle;
            // string timerElapsedFunctionName = "timerElapsed";
            string versionFunctionName = "getVersion";
            string versionValue = "2.4.9181";
            string inFilePath = System.IO.Path.Combine( inFolderPath, fileTitle + cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension );

            TspSessionDebugScriptTests.AssertScriptShouldImportAndRun( this.Device.Session, scriptName, inFilePath, versionFunctionName, versionValue, false );

            this.Device.Session.ConvertToByteCode( scriptName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, versionFunctionName );
            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            if ( string.Equals( "2600A", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "_2600A";
            else if ( string.Equals( "2600B", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "_2600B";
            else
                Assert.Fail( $"The model family {this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily} is not supported." );

            string outFilePath = Path.Combine( outFolderPath, $"{fileTitle}{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension}" );

            if ( !this.Device.Session.TryExportScript( scriptName, outFilePath, true, out string details ) )
                Assert.Fail( details );

            Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            TspSessionDebugScriptTests.AssertScriptShouldImportAndRun( this.Device.Session, scriptName, inFilePath, versionFunctionName, versionValue, true );

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
                Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );
                Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
            }
        }
    }

    #endregion
}
