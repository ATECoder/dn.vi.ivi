// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600.Tests.Visa;

/// <summary>   K2002 Visa Session unit tests. </summary>
/// <remarks>   David, 2017-10-10. </remarks>
[TestClass]
[TestCategory( "k2600" )]
public partial class VisaSessionTests : Device.Tests.Base.VisaSessionTests
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
        Device.Tests.Base.VisaSessionTests.InitializeBaseTestClass( testContext );
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup]
    public static void CleanupTestClass()
    {
        Device.Tests.Base.VisaSessionTests.CleanupTestBaseClass();
    }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestInitialize()]
    public override void InitializeBeforeEachTest()
    {
        base.DefineTraceListener();

        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {System.TimeZoneInfo.Local}" );
        cc.isr.VI.Device.Tests.Asserts.AssertVisaImplementationShouldBeLoaded();
        Console.WriteLine( $"\tTesting {typeof( K2600Device ).Assembly.FullName}" );

        // create an instance of the session logger.
        SessionLogger.Instance.CreateLogger( typeof( VisaSessionTests ) );

        this.LocationSettings = Settings.AllSettings.Instance.LocationSettings;
        this.ResourceSettings = Settings.AllSettings.Instance.ResourceSettings;
        VisaSession visaSession = new();
        Assert.IsNotNull( visaSession.Session );
        Assert.AreEqual( Syntax.Tsp.Lua.ClearExecutionStateCommand, visaSession.Session.ClearExecutionStateCommand );
        visaSession.Session.ReadSettings( this.GetType(), ".Session", true, true );
        Assert.IsTrue( visaSession.Session.AllSettings.TimingSettings.Exists, $"{nameof( VisaSession )}.{nameof( K2600Device.Session )}.{nameof( VisaSession.Session.AllSettings.TimingSettings )} does not exist." );

        this.VisaSessionBase = visaSession;
        base.InitializeBeforeEachTest();
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    /// <remarks> David, 2020-09-18. </remarks>
    [TestCleanup()]
    public override void CleanupAfterEachTest()
    {
        base.CleanupAfterEachTest();
        SessionLogger.CloseAndFlush();
    }

    #endregion
}

