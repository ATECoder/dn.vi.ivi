using System;
using System.Linq;
using cc.isr.Std.Tests;
using cc.isr.VI.Pith;
using cc.isr.Std.Tests.Extensions;

namespace cc.isr.VI.Device.Tests.Base;

/// <summary> The abstract VISA Resource Settings tests. </summary>
/// <remarks>
/// (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2017-10-11 </para>
/// </remarks>
public abstract class VisaResourceTests
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
                Logger.LogExceptionMultiLineMessage( "Failed initializing the test class:", ex );

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
    public LoggerTraceListener<VisaResourceTests>? TraceListener { get; set; }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    public virtual void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( SessionFactory ).Assembly.FullName}" );

        // assert reading of test settings from the configuration file.
        Assert.IsNotNull( this.TestSiteSettings );
        Assert.IsTrue( this.TestSiteSettings.Exists );
        double expectedUpperLimit = 12d;
        Assert.IsTrue( Math.Abs( this.TestSiteSettings.TimeZoneOffset() ) < expectedUpperLimit,
                       $"{nameof( this.TestSiteSettings.TimeZoneOffset )} should be lower than {expectedUpperLimit}" );

        if ( Logger is not null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<VisaResourceTests>( Logger );
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
    public static ILogger<VisaResourceTests>? Logger { get; } = LoggerProvider.CreateLogger<VisaResourceTests>();

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the test site settings. </summary>
    /// <value> The test site settings. </value>
    protected TestSiteSettings? TestSiteSettings { get; set; }

    #endregion

    #region " visa version tests "

    /// <summary> (Unit Test Method) Assert that the function visa versions are valid. </summary>
    /// <remarks>
    /// To test the 32bit version, change the default processor architecture from the Test, Test
    /// Settings menu.
    /// </remarks>
    [TestMethod( "01. Visa versions should be valid" )]
    public void VisaVersionsShouldBeValid()
    {
        (bool success, string details) = SessionFactory.ValidateFunctionalVisaVersions();
        Assert.IsTrue( success, details );
    }

    #endregion

    #region " visa resource test "

    /// <summary>   (Unit Test Method) Assert that visa implementations exit. </summary>
    /// <remarks>   2024-07-09. </remarks>
    [TestMethod( "02. Visa implementations should exit" )]
    public void VisaImplementationsShouldExit()
    {
        Version expectedVersion = new( Visa.Gac.Vendor.IVI_VISA_IMPLEMENTATION_VERSION );
        Version? actualVersion = Visa.Gac.GacLoader.VerifyVisaImplementationPresence();

        Assert.IsNotNull( actualVersion, "No Visa implementations were found." );
        Assert.AreEqual( expectedVersion, actualVersion );

        Visa.Gac.GacLoader.LoadInstalledVisaAssemblies();

        (bool success, string details) = Foundation.VisaVersionValidator.ValidateFunctionalVisaVersions();
        Assert.IsTrue( success, details );

    }

    /// <summary> (Unit Test Method) Assert that visa resources can be located for some filters. </summary>
    [TestMethod( "03. Visa resources should be found" )]
    public void VisaResourcesShouldBeFound()
    {
        // resourcesFilter = resourcesFilter.Replace("(", "[")
        // resourcesFilter = resourcesFilter.Replace(")", "]")
        // resourcesFilter = "[TCPIP|GPIB]*INSTR"
        // resourcesFilter = "(TCPIP|GPIB)*INSTR"
        string[]? resources;
        string? resourcesFilter;
        using ( ResourcesProviderBase rm = SessionFactory.Instance.Factory.ResourcesProvider() )
        {
            resourcesFilter = rm.ResourceFinder!.BuildMinimalResourcesFilter();
            resources = rm.FindResources( resourcesFilter ).ToArray();
        }
        Assert.IsNotNull( resources, nameof( resources ) );
        Assert.IsNotNull( resourcesFilter, nameof( resourcesFilter ) );

        Assert.IsTrue( resources.Length > 0, $"VISA Resources should exit for the {resourcesFilter} search pattern" );

        resourcesFilter = "[TCPIP|GPIB|USB]?*INSTR";
        bool usingLikePattern = false;
        string actualResourceFilter = ResourceNamesManager.TranslatePattern( resourcesFilter, usingLikePattern );
        string expectedResourceFiler = "(TCPIP|GPIB|USB)?*INSTR";
        Assert.AreEqual( expectedResourceFiler, actualResourceFilter, $"VISA Resources should translate when Using Like Pattern is {usingLikePattern}" );

        using ( ResourcesProviderBase rm = SessionFactory.Instance.Factory.ResourcesProvider() )
            resources = rm.FindResources( actualResourceFilter ).ToArray();
        Assert.IsTrue( resources.Length > 0, $"VISA Resources should exist for the {actualResourceFilter} search pattern" );

        usingLikePattern = true;
        actualResourceFilter = ResourceNamesManager.TranslatePattern( actualResourceFilter, usingLikePattern );
        expectedResourceFiler = resourcesFilter;
        Assert.AreEqual( expectedResourceFiler, actualResourceFilter, $"VISA Resources should translate when Using Like Pattern is {usingLikePattern}" );
        using ( ResourcesProviderBase rm = SessionFactory.Instance.Factory.ResourcesProvider() )
            resources = rm.FindResources( actualResourceFilter ).ToArray();
        Assert.IsNotNull( resources, nameof( resources ) );

        Assert.IsTrue( resources.Length > 0, $"VISA Resources should exist for the {actualResourceFilter} search pattern" );

        actualResourceFilter = actualResourceFilter.Replace( '?', '_' );
        using ( ResourcesProviderBase rm = SessionFactory.Instance.Factory.ResourcesProvider() )
            resources = rm.FindResources( actualResourceFilter ).ToArray();
        Assert.IsNotNull( resources, nameof( resources ) );
        Assert.IsFalse( resources.Length > 0, $"VISA Resources should not exist for the {actualResourceFilter} search pattern" );

    }

    #endregion

    #region " resource name info "

    /// <summary> (Unit Test Method) Assert that a visa resource could be added to the resource collection. </summary>
    [TestMethod( "04. Visa resource name should be added" )]
    public void VisaResourceNameShouldBeAdded()
    {
        ResourceNameInfoCollection resourceNameInfoCollection = [];
        int expectedCount;
        int actualCount;
        if ( !resourceNameInfoCollection.IsFileExists() )
        {
            // if a new file, add a known resource.
            string resourceName = "TCPIP0::192.168.0.254::gpib0,15::INSTR";
            resourceNameInfoCollection.Add( resourceName );
            resourceNameInfoCollection.WriteResources();
            Assert.IsTrue( resourceNameInfoCollection.IsFileExists() );
            expectedCount = resourceNameInfoCollection.Count;
            resourceNameInfoCollection.ReadResources();
            actualCount = resourceNameInfoCollection.Count;
            Assert.AreEqual( expectedCount, actualCount, $"{nameof( ResourceNameInfoCollection )}.count should match" );
            Assert.IsTrue( resourceNameInfoCollection.Contains( resourceName ), $"{nameof( ResourceNameInfoCollection )} should contain {resourceName}" );
        }

        resourceNameInfoCollection.ReadResources();
        Assert.IsTrue( resourceNameInfoCollection.Any(), $"{nameof( ResourceNameInfoCollection )} should have items" );
        resourceNameInfoCollection.WriteResources();
    }

    #endregion
}
