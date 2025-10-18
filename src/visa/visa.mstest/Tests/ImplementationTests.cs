using System.Diagnostics;

namespace cc.isr.Visa.Tests;

/// <summary>   (Unit Test Class) Visa Vendor implementation tests. </summary>
/// <remarks>   2024-07-13. </remarks>
[TestClass]
public class ImplementationTests
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
            // Console.Out.WriteLine( $"{methodFullName} initializing" );
        }
        catch ( Exception ex )
        {
            Console.Out.WriteLine( $"{methodFullName} failed initializing the test class:\r\n\t{ex}" );

            // cleanup to meet strong guarantees

            try
            {
                CleanupTestClass();
            }
            finally
            {
            }
        }
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    [ClassCleanup]
    public static void CleanupTestClass()
    {
    }

    /// <summary>
    /// Gets or sets the test context which provides information about and functionality for the
    /// current test run.
    /// </summary>
    /// <value> The test context. </value>
    public TestContext? TestContext { get; set; }

    #endregion

    /// <summary> Initializes the test class instance before each test runs. </summary>
    [TestInitialize()]
    public virtual void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {TimeZoneInfo.Local}" );
        Asserts.AssertVisaImplementationShouldBeLoaded();
        Console.WriteLine( $"\tTesting {typeof( cc.isr.Visa.Gac.GacLoader ).Assembly.FullName}" );
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    [TestCleanup()]
    public virtual void CleanupAfterEachTest()
    {
        Trace.Listeners.Clear();
    }

    /// <summary>   (Unit Test Method) dot net implementations should exist. </summary>
    /// <remarks>   2024-07-13. </remarks>
    [TestMethod( DisplayName = "01. .NET Visa implementation(s) should exist" )]
    public void DotNetVisaImplementationsShouldExist()
    {
        _ = Gac.GacLoader.TryLoadInstalledVisaAssemblies( out _ );
        Assert.IsTrue( Gac.GacLoader.HasDotNetImplementations, "No .NET Visa implementations where found." );
    }

    /// <summary>   (Unit Test Method) ivi visa assembly should exist. </summary>
    /// <remarks>   2024-07-13. </remarks>
    [TestMethod( DisplayName = "02. IVI Visa assembly should exist" )]
    public void IviVisaAssemblyShouldExist()
    {
        (bool success, string details) = Gac.Vendor.IsIviVisaAssemblyExists();
        Assert.IsTrue( success, details );
    }

    /// <summary>   (Unit Test Method) Keysight visa should exist. </summary>
    /// <remarks>   2024-07-13. </remarks>
    [TestMethod( DisplayName = "03. Keysight Visa should exist" )]
    public void KeysightVisaShouldExist()
    {
        if ( Gac.Vendor.IsLoadedKeysightImplementation() )
        {
            (bool success, string details) = Gac.Vendor.IsKeysightVisaAssemblyExists();
            Assert.IsTrue( success, details );
        }
    }

    /// <summary>   (Unit Test Method) Keysight resource manager should exist. </summary>
    /// <remarks>   David, 2021-11-06. </remarks>
    [TestMethod( DisplayName = "04. Keysight resource manager should exist" )]
    public void KeysightResourceManagerShouldExist()
    {
        if ( Gac.Vendor.IsLoadedKeysightImplementation() )
        {
            (bool success, string details) = Gac.Vendor.IsKeysightResourceManagerExists();
            Assert.IsTrue( success, details );
        }
    }

    /// <summary>   National instruments visa should exist. </summary>
    /// <remarks>   2024-07-13. </remarks>
    [TestMethod( DisplayName = "05. NI Visa should exist" )]
    public void NationalInstrumentsVisaShouldExist()
    {
        if ( Gac.Vendor.IsLoadedNImplementation() )
        {
            (bool success, string details) = Gac.Vendor.IsNIVisaAssemblyExists();
            Assert.IsTrue( success, details );
        }
    }

    /// <summary>   National instruments resource manager should exist. </summary>
    /// <remarks>   2024-07-13. </remarks>
    [TestMethod( DisplayName = "06. NI Visa resource manager should exist" )]
    public void NationalInstrumentsResourceManagerShouldExist()
    {
        if ( Gac.Vendor.IsLoadedNImplementation() )
        {
            (bool success, string details) = Gac.Vendor.IsNIResourceManagerExists();
            Assert.IsTrue( success, details );
        }
    }

    /// <summary>   (Unit Test Method) visa implementations should be enumerated. </summary>
    /// <remarks>   2024-07-13. </remarks>
    [TestMethod( DisplayName = "07. Visa implementations should be enumerated" )]
    public void VisaImplementationsShouldBeEnumerated()
    {
        System.Collections.Generic.IEnumerable<Ivi.Visa.ConflictManager.VisaImplementation> installedVisas = Gac.Vendor.EnumerateDotNetImplementations();
        Assert.IsTrue( installedVisas.Any(), $"{nameof( installedVisas )} should have items" );
        string visaImplementationName = nameof( Ivi.Visa.ConflictManager.VisaImplementation );

        foreach ( Ivi.Visa.ConflictManager.VisaImplementation implementation in installedVisas )
        {
            Console.WriteLine( $"{visaImplementationName}" );
            Console.WriteLine( $"\t{nameof( Ivi.Visa.ConflictManager.VisaImplementation.Location )}: {implementation.Location}" );
            Console.WriteLine( $"\t{nameof( Ivi.Visa.ConflictManager.VisaImplementation.ApiType )}: {implementation.ApiType}" );
            Console.WriteLine( $"\t{nameof( Ivi.Visa.ConflictManager.VisaImplementation.Comments )}: {implementation.Comments}" );
            Console.WriteLine( $"\t{nameof( Ivi.Visa.ConflictManager.VisaImplementation.Enabled )}: {implementation.Enabled}" );
            Console.WriteLine( $"\t{nameof( Ivi.Visa.ConflictManager.VisaImplementation.FriendlyName )}: {implementation.FriendlyName}" );
            Console.WriteLine( $"\t{nameof( Ivi.Visa.ConflictManager.VisaImplementation.ResourceManufacturerId )}: {implementation.ResourceManufacturerId}" );
        }
    }
}
