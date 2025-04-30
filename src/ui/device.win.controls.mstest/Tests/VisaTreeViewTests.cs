using System;
using System.Threading.Tasks;

namespace cc.isr.VI.DeviceWinControls.Tests;

/// <summary> Visa Tree View unit tests. </summary>
/// <remarks>
/// (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2017-10-10 </para>
/// </remarks>
[TestClass()]
public class VisaTreeViewTests : cc.isr.VI.DeviceWinControls.Tests.Base.IVisaViewTests
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
        VI.Device.Tests.Base.DeviceTests.InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
    public static void CleanupTestClass()
    {
        VI.Device.Tests.Base.DeviceTests.CleanupBaseTestClass();
    }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public override void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        Console.WriteLine( $"\tTesting {typeof( cc.isr.VI.DeviceWinControls.VisaTreeView ).Assembly.FullName}" );

        // create an instance of the Serilog logger.
        SessionLogger.Instance.CreateSerilogLogger( typeof( VisaTreeViewTests ) );

        this.TestSiteSettings = AllSettings.Instance.TestSiteSettings;
        this.ResourceSettings = AllSettings.Instance.ResourceSettings;

        VisaSession visaSession = new();
        Assert.IsNotNull( visaSession.Session );
        Assert.AreEqual( VI.Syntax.Ieee488Syntax.ClearExecutionStateCommand, visaSession.Session.ClearExecutionStateCommand );
        visaSession.Session.ReadSettings( this.GetType().Assembly, ".Session", true, true );
        Assert.IsTrue( visaSession.Session.TimingSettings.Exists,
            $"{nameof( VisaSession )}.{nameof( VisaSession.Session )}.{nameof( VisaSession.Session.TimingSettings )} does not exist." );

        this.VisaSessionBase = visaSession;

        base.InitializeBeforeEachTest();
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestCleanup()]
    public override void CleanupAfterEachTest()
    {
        base.CleanupAfterEachTest();
        this.VisaSessionBase?.Dispose();
        this.VisaSessionBase = null;
        SessionLogger.CloseAndFlush();
    }

    #endregion

    #region " device open tests "

    /// <summary> (Unit Test Method) asserts that a resource name should be selected. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [TestMethod( "02. Visa tree view resource name should be selected" )]
    public void ResourceNameShouldBeSelected()
    {
        using VisaTreeView? view = new( this.VisaSessionBase );
        Asserts.AssertResourceNameShouldBeSelected( view, this.ResourceSettings );
    }

    /// <summary>   (Unit Test Method) asserts that a visa session should open. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    [TestMethod( "03. Visa Tree View Session Should Open." )]
    public void VisaTreeViewSessionShouldOpen()
    {
        Assert.IsNotNull( this.VisaSessionBase, $"{nameof( this.VisaSessionBase )} should not be null." );
        Assert.IsNotNull( this.VisaSessionBase.Session, $"{nameof( this.VisaSessionBase )}.{nameof( this.VisaSessionBase.Session )} should not be null." );
        int trialNumber = 0;
        using VisaTreeView view = new( this.VisaSessionBase );
        try
        {
            Asserts.AssertVisaViewSessionShouldOpen( trialNumber, view, this.ResourceSettings );
            Asserts.AssertSessionResourceNamesShouldMatch( this.VisaSessionBase.Session, this.ResourceSettings );
        }
        catch
        {
            throw;
        }
        finally
        {
            Asserts.AssertVisaViewSessionShouldClose( trialNumber, view );
        }
    }

    /// <summary>   (Unit Test Method) asserts that a visa session should open and close twice. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [TestMethod( "04. Visa tree view session should open twice" )]
    public void VisaTreeViewSessionShouldOpenTwice()
    {
        Assert.IsNotNull( this.VisaSessionBase, $"{nameof( this.VisaSessionBase )} should not be null." );
        Assert.IsNotNull( this.VisaSessionBase.Session, $"{nameof( this.VisaSessionBase )}.{nameof( this.VisaSessionBase.Session )} should not be null." );
        int trialNumber = 0;
        using ( VisaTreeView view = new( this.VisaSessionBase ) )
        {
            Asserts.AssertVisaViewSessionShouldOpenAndClose( trialNumber, view, this.ResourceSettings );
        }

        trialNumber += 1;
        using ( VisaTreeView view = new( this.VisaSessionBase ) )
        {
            Asserts.AssertVisaViewSessionShouldOpenAndClose( trialNumber, view, this.ResourceSettings );
        }

        trialNumber += 1;
        using ( VisaTreeView view = new( this.VisaSessionBase ) )
        {
            Asserts.AssertVisaViewSessionShouldOpenAndClose( trialNumber, view, this.ResourceSettings );
            Task.Delay( TimeSpan.FromMilliseconds( 100d ) ).Wait();
            trialNumber += 1;
            Asserts.AssertVisaViewSessionShouldOpenAndClose( trialNumber, view, this.ResourceSettings );
        }
    }

    #endregion

    #region " assigned device tests "

    /// <summary>   (Unit Test Method) asserts that a visa session should bind a visa session. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [TestMethod( "05. Visa tree view should bind a Visa session" )]
    public void VisaTreeViewShouldBindVisaSession()
    {
        int trialNumber = 0;
        using VisaTreeView view = new();
        view.BindVisaSessionBase( this.VisaSessionBase );
        Asserts.AssertVisaViewSessionShouldOpenAndClose( trialNumber, view, this.ResourceSettings );
    }

    /// <summary>   (Unit Test Method) asserts that a visa session should open and close twice. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [TestMethod( "06. Visa tree view should bind an open Visa session" )]
    public void VisaTreeViewShouldBindOpenVisaSession()
    {
        int trialNumber = 0;
        using VisaTreeView view = new( this.VisaSessionBase );
        try
        {
            Asserts.AssertVisaViewSessionShouldOpen( trialNumber, view, this.ResourceSettings );
            view.BindVisaSessionBase( this.VisaSessionBase );
        }
        catch
        {
            throw;
        }
        finally
        {
            Asserts.AssertVisaViewSessionShouldClose( trialNumber, view );
        }
    }

    #endregion
}
