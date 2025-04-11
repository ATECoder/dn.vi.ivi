using System;
using System.IO;
using System.Text;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.K2600.MSTest.Tsp;

/// <summary>   (Unit Test Class) a tsp script tests. </summary>
/// <remarks>   2025-04-10. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public class TspScriptTests : Device.Tests.Base.ScriptTests
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
        // reported in the base class
        // Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( cc.isr.VI.Tsp.K2600.MeasureResistanceSubsystem ).Assembly.FullName}" );

        // create an instance of the Serilog logger.
        SessionLogger.Instance.CreateSerilogLogger( typeof( TspScriptTests ) );

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
    [TestMethod( "01. Script Should Load and Run" )]
    public void ScriptShouldLoadAndRun()
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );

        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );
        string scriptName = "timeDisplayClear";
        try
        {
            string folderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
            string loadCommand = $"{cc.isr.VI.Syntax.Tsp.Script.LoadScriptCommand} {scriptName}";
            string timerElapsedFunctionName = "timerElapsed";
            string timerElapsed = $"{timerElapsedFunctionName}=function() return timer.measure.t() end";
            StringBuilder scriptSource = new();
            _ = scriptSource.AppendLine( loadCommand );
            _ = scriptSource.AppendLine( timerElapsed );
            _ = scriptSource.AppendLine( "timer.reset()" );
            _ = scriptSource.AppendLine( "display.clear()" );
            _ = scriptSource.AppendLine( "local elapsed = timer.measure.t()" );
            _ = scriptSource.AppendLine( "print( \"display clear elapsed \" .. 1000 * elapsed .. \"ms\" )" );
            _ = scriptSource.AppendLine( "timer.reset()" );
            _ = scriptSource.AppendLine( cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand );

            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            this.Device.Session.LoadScript( scriptName, scriptSource.ToString() );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.SaveScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.ConvertToBinary( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.SaveScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            string filePath = Path.Combine( folderPath, $"{scriptName}.exported.tspb" );
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
            this.Device.Session.DeleteScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    /// <summary>   (Unit Test Method) script should load and run from file. </summary>
    /// <remarks>   2025-04-10. </remarks>
    [TestMethod( "02. Script Should Load and run from file" )]
    public void ScriptShouldLoadAndRunFromFile()
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        Assert.IsNotNull( this.Device.StatusSubsystemBase );

        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );
        string scriptName = "timeDisplayClear";
        try
        {
            string folderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
            string fileTitle = "timeDisplayClearScript";
            string timerElapsedFunctionName = "timerElapsed";
            string scriptSource = System.IO.File.ReadAllText( System.IO.Path.Combine( folderPath, fileTitle ) );

            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            this.Device.Session.LoadScript( scriptName, scriptSource.ToString() );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.SaveScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            string filePath = Path.Combine( folderPath, $"{scriptName}.exported.tsp" );

            this.Device.Session.ExportScript( scriptName, filePath, true );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.ConvertToBinary( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.SaveScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            if ( string.Equals( "2600A", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "A";
            else if ( string.Equals( "2600B", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "B";
            else
                Assert.Fail( $"The model family {this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily} is not supported." );

            filePath = Path.Combine( folderPath, $"{fileTitle}.exported.tspb" );

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
            this.Device.Session.DeleteScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    /// <summary>   (Unit Test Method) binary script should load and run from file. </summary>
    /// <remarks>   2025-04-10. </remarks>
    [TestMethod( "03. Binary Script Should Load and run from file" )]
    public void BinaryScriptShouldLoadAndRunFromFile()
    {
        Assert.IsNotNull( this.Device );
        Assert.IsNotNull( this.Device.Session );
        Assert.IsNotNull( this.Device.StatusSubsystemBase );

        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );
        string scriptName = "timeDisplayClear";
        try
        {
            string folderPath = "C:\\my\\lib\\tsp\\tsp.1\\core\\tests";
            string fileTitle = "timeDisplayClearScript";
            string timerElapsedFunctionName = "timerElapsed";

            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldOpenWithoutDeviceErrors( this.Device, this.ResourceSettings );

            if ( string.Equals( "2600A", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "A";
            else if ( string.Equals( "2600B", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "B";
            else
                Assert.Fail( $"The model family {this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily} is not supported." );

            string scriptSource = System.IO.File.ReadAllText( System.IO.Path.Combine( folderPath, fileTitle + ".tspb" ) );

            this.Device.Session.LoadScript( scriptName, scriptSource.ToString() );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.SaveScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            this.Device.Session.RunScript( scriptName, timerElapsedFunctionName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            string filePath = Path.Combine( folderPath, $"{fileTitle}.tspb.exported.tspb" );

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
            this.Device.Session.DeleteScript( scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( this.Device );

            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    #endregion
}
