using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using cc.isr.VI.Pith;

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

    /// <summary>   Assert script should delete. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <param name="device">       The reference to the K2600 Device. </param>
    /// <param name="scriptName">   Name of the script. </param>
    private static void AssertScriptShouldDelete( VisaSessionBase device, string scriptName )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Pith.SessionBase session = device.Session;
        Assert.IsTrue( session.IsSessionOpen, "Session must be open" );
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );
        try
        {
            Trace.WriteLine( "checking if the script exists;. ", methodFullName );
            if ( !session.IsNil( scriptName ) )
            {
                Trace.WriteLine( "deleting the saved script;. ", methodFullName );
                _ = session.WriteLine( $"script.delete({scriptName})" );
                _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );
                cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );
            }
        }
        catch ( Exception )
        {
        }
        finally
        {
        }

    }

    /// <summary>   Assert script should load and run. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <param name="device">               The reference to the K2600 Device. </param>
    /// <param name="scriptName">           Name of the script. </param>
    /// <param name="scriptSource">         The script source. </param>
    /// <param name="scriptElementName">    (Optional) [empty] Name of the script element. </param>
    private static void AssertScriptShouldLoadAndRun( VisaSessionBase device, string scriptName, string scriptSource,
        string scriptElementName = "" )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Pith.SessionBase session = device.Session;
        Assert.IsTrue( session.IsSessionOpen, "Session must be open" );
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );
        try
        {
            Trace.WriteLine( "Writing script load commands;. ", methodFullName );
            _ = session.WriteLine( scriptSource );
            _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            Trace.WriteLine( "checking if the script exists;. ", methodFullName );
            Assert.IsFalse( session.IsNil( scriptName ), $"The script {scriptName} should exist after loading." );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            Trace.WriteLine( "running the script;. ", methodFullName );
            _ = session.WriteLine( $"{scriptName}.run()" );
            _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            if ( !string.IsNullOrWhiteSpace( scriptElementName ) )
            {
                Trace.WriteLine( $"looking for {scriptElementName};. ", methodFullName );
                _ = session.WriteLine( $"_G.print( {scriptElementName} )" );
                string reply = session.ReadFreeLineTrimEnd();
                Assert.IsFalse( string.IsNullOrWhiteSpace( reply ), $"The script {scriptName} should have printed the {scriptElementName} object identity." );
                Assert.AreNotEqual( "nil", reply, $"The script {scriptName} should have printed the {scriptElementName} object identity not {reply}." );

                cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );
            }

        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary>   Assert script should save to nvm. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <param name="device">               The reference to the K2600 Device. </param>
    /// <param name="scriptName">           Name of the script. </param>
    /// <param name="scriptElementName">    (Optional) [empty] Name of the script element. </param>
    ///
    /// ### <param name="scriptSource"> The script source. </param>
    private static void AssertScriptShouldSaveToNVM( VisaSessionBase device, string scriptName, string scriptElementName = "" )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Pith.SessionBase session = device.Session;
        Assert.IsTrue( session.IsSessionOpen, "Session must be open" );
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );
        try
        {
            Trace.WriteLine( "checking if the script exists;. ", methodFullName );
            Assert.IsFalse( session.IsNil( scriptName ), $"The script {scriptName} should exist after loading." );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            if ( !string.IsNullOrWhiteSpace( scriptElementName ) )
            {
                Trace.WriteLine( $"looking for {scriptElementName};. ", methodFullName );
                _ = session.WriteLine( $"_G.print( {scriptElementName} )" );
                string reply = session.ReadFreeLineTrimEnd();
                Assert.IsFalse( string.IsNullOrWhiteSpace( reply ), $"The script {scriptName} should have printed the {scriptElementName} object identity." );
                Assert.AreNotEqual( "nil", reply, $"The script {scriptName} should have printed the {scriptElementName} object identity not {reply}." );

                cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );
            }

            Trace.WriteLine( "saving the script to NVM;. ", methodFullName );
            _ = session.WriteLine( $"{scriptName}.save()" );
            _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 500 ) );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            Trace.WriteLine( "running the saved script after saving to NVM;. ", methodFullName );
            _ = session.WriteLine( $"{scriptName}.run()" );
            _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            if ( !string.IsNullOrWhiteSpace( scriptElementName ) )
            {
                Trace.WriteLine( $"looking for {scriptElementName} after running the NVM script;. ", methodFullName );
                _ = session.WriteLine( $"_G.print( {scriptElementName} )" );
                string reply = session.ReadFreeLineTrimEnd();
                Assert.IsFalse( string.IsNullOrWhiteSpace( reply ), $"The script {scriptName} should have printed the {scriptElementName} object identity." );
                Assert.AreNotEqual( "nil", reply, $"The script {scriptName} should have printed the {scriptElementName} object identity not {reply}." );

                cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );
            }

        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary>   Assert script should convert to binary. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <param name="device">               The reference to the K2600 Device. </param>
    /// <param name="scriptName">           Name of the script. </param>
    /// <param name="scriptElementName">    (Optional) [empty] Name of the script element. </param>
    ///
    /// ### <param name="scriptSource"> The script source. </param>
    private static void AssertScriptShouldConvertToBinary( VisaSessionBase device, string scriptName, string scriptElementName = "" )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Pith.SessionBase session = device.Session;
        Assert.IsTrue( session.IsSessionOpen, "Session must be open" );
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );
        try
        {
            Trace.WriteLine( "checking if the script exists;. ", methodFullName );
            Assert.IsFalse( session.IsNil( scriptName ), $"The script {scriptName} should exist after loading." );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            if ( !string.IsNullOrWhiteSpace( scriptElementName ) )
            {
                Trace.WriteLine( $"looking for {scriptElementName};. ", methodFullName );
                _ = session.WriteLine( $"_G.print( {scriptElementName} )" );
                string reply = session.ReadFreeLineTrimEnd();
                Assert.IsFalse( string.IsNullOrWhiteSpace( reply ), $"The script {scriptName} should have printed the {scriptElementName} object identity." );
                Assert.AreNotEqual( "nil", reply, $"The script {scriptName} should have printed the {scriptElementName} object identity not {reply}." );

                cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );
            }

            Trace.WriteLine( "converting the script to binary;. ", methodFullName );
            _ = session.WriteLine( $"{scriptName}.source=nil" );
            _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            Trace.WriteLine( "running the saved script after conversion to binary;. ", methodFullName );
            _ = session.WriteLine( $"{scriptName}.run()" );
            _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            if ( !string.IsNullOrWhiteSpace( scriptElementName ) )
            {
                Trace.WriteLine( $"looking for {scriptElementName} after running the binary script;. ", methodFullName );
                _ = session.WriteLine( $"_G.print( {scriptElementName} )" );
                string reply = session.ReadFreeLineTrimEnd();
                Assert.IsFalse( string.IsNullOrWhiteSpace( reply ), $"The script {scriptName} should have printed the {scriptElementName} object identity." );
                Assert.AreNotEqual( "nil", reply, $"The script {scriptName} should have printed the {scriptElementName} object identity not {reply}." );

                cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
                cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );
            }
        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary>   Assert script should export. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <param name="device">               The reference to the K2600 Device. </param>
    /// <param name="scriptName">           Name of the script. </param>
    /// <param name="exportFileName">       (Optional) [empty] Filename of the export file. </param>
    private static void AssertScriptShouldExport( VisaSessionBase device, string scriptName, string exportFileName )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Pith.SessionBase session = device.Session;
        Assert.IsTrue( session.IsSessionOpen, "Session must be open" );
        System.Reflection.MethodBase? methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
        string methodFullName = $"{methodInfo?.DeclaringType?.Name}.{methodInfo?.Name}";
        Console.WriteLine( $"@{methodFullName}" );
        try
        {
            Trace.WriteLine( "checking if the script exists;. ", methodFullName );
            Assert.IsFalse( session.IsNil( scriptName ), $"The script {scriptName} should exist after loading." );
            cc.isr.VI.Device.Tests.Asserts.AssertMessageQueue();
            cc.isr.VI.Device.Tests.Asserts.AssertOnDeviceErrors( device );

            string scriptSource = session.Query( $"G_print( {scriptName}.source ) " );
            _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( scriptSource ), $"The script {scriptName} source fetched from should not be empty." );
            System.IO.File.WriteAllText( exportFileName, scriptSource );
        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

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
            TspScriptTests.AssertScriptShouldLoadAndRun( this.Device, scriptName, scriptSource.ToString(), timerElapsedFunctionName );
            TspScriptTests.AssertScriptShouldSaveToNVM( this.Device, scriptName, timerElapsedFunctionName );
            TspScriptTests.AssertScriptShouldConvertToBinary( this.Device, scriptName, timerElapsedFunctionName );
            TspScriptTests.AssertScriptShouldSaveToNVM( this.Device, scriptName, timerElapsedFunctionName );
            string filePath = Path.Combine( folderPath, $"{scriptName}.exported.tspb" );
            TspScriptTests.AssertScriptShouldExport( this.Device, scriptName, filePath );
            TspScriptTests.AssertScriptShouldDelete( this.Device, scriptName );
        }
        catch
        {
            throw;
        }
        finally
        {
            TspScriptTests.AssertScriptShouldDelete( this.Device, scriptName );
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
            TspScriptTests.AssertScriptShouldLoadAndRun( this.Device, scriptName, scriptSource, timerElapsedFunctionName );
            TspScriptTests.AssertScriptShouldSaveToNVM( this.Device, scriptName, timerElapsedFunctionName );
            string filePath = Path.Combine( folderPath, $"{scriptName}.exported.tsp" );
            TspScriptTests.AssertScriptShouldExport( this.Device, scriptName, filePath );
            TspScriptTests.AssertScriptShouldConvertToBinary( this.Device, scriptName, timerElapsedFunctionName );
            TspScriptTests.AssertScriptShouldSaveToNVM( this.Device, scriptName, timerElapsedFunctionName );

            if ( string.Equals( "2600A", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "A";
            else if ( string.Equals( "2600B", this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily, StringComparison.OrdinalIgnoreCase ) )
                fileTitle += "B";
            else
                Assert.Fail( $"The model family {this.Device.StatusSubsystemBase.VersionInfoBase.ModelFamily} is not supported." );

            filePath = Path.Combine( folderPath, $"{fileTitle}.exported.tspb" );
            TspScriptTests.AssertScriptShouldExport( this.Device, scriptName, filePath );
            TspScriptTests.AssertScriptShouldDelete( this.Device, scriptName );
        }
        catch
        {
            throw;
        }
        finally
        {
            TspScriptTests.AssertScriptShouldDelete( this.Device, scriptName );
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

            TspScriptTests.AssertScriptShouldLoadAndRun( this.Device, scriptName, scriptSource, timerElapsedFunctionName );
            TspScriptTests.AssertScriptShouldSaveToNVM( this.Device, scriptName, timerElapsedFunctionName );
            string filePath = Path.Combine( folderPath, $"{fileTitle}.tspb.exported.tspb" );
            TspScriptTests.AssertScriptShouldExport( this.Device, scriptName, filePath );
            TspScriptTests.AssertScriptShouldDelete( this.Device, scriptName );
        }
        catch
        {
            throw;
        }
        finally
        {
            TspScriptTests.AssertScriptShouldDelete( this.Device, scriptName );
            cc.isr.VI.Device.Tests.Asserts.AssertDeviceShouldCloseWithoutErrors( this.Device );
        }
    }

    #endregion
}
