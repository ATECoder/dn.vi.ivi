using System;
using System.Diagnostics;
using System.Linq;

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
            Trace.WriteLine( "Initializing", methodFullName );
        }
        catch ( Exception ex )
        {
            Trace.WriteLine( $"Failed initializing the test class: {ex}", methodFullName );

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
    [ClassCleanup( ClassCleanupBehavior.EndOfClass )]
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
        Console.WriteLine( $"Testing {typeof( Gac.Vendor ).Assembly.FullName}" );
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>	   
    [TestCleanup()]
    public virtual void CleanupAfterEachTest()
    {
        Trace.Listeners.Clear();
    }

    /// <summary>   (Unit Test Method) dot net implementations should exist. </summary>
    /// <remarks>   2024-07-13. </remarks>
    [TestMethod( "01. .NET Visa Implementation(s) should exist" )]
    public void DotNetVisaImplementationsShouldExist()
    {
        Gac.GacLoader.LoadInstalledVisaAssemblies();
        Assert.IsTrue( Gac.GacLoader.HasDotNetImplementations, "No .NET Visa implementations where found." );
    }

    /// <summary>   (Unit Test Method) ivi visa assembly should exist. </summary>
    /// <remarks>   2024-07-13. </remarks>
    [TestMethod( "02. IVI Visa Assembly Should Exist" )]
    public void IviVisaAssemblyShouldExist()
    {
        (bool success, string details) = Gac.Vendor.IsIviVisaAssemblyExists();
        Assert.IsTrue( success, details );
    }

    /// <summary>   (Unit Test Method) Keysight visa should exist. </summary>
    /// <remarks>   2024-07-13. </remarks>
    [TestMethod( "03. Keysight Visa Should Exist" )]
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
    [TestMethod( "04. Keysight Resource Manager Should Exist" )]
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
    [TestMethod( "05. National Instruments Visa Should Exist" )]
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
    [TestMethod( "06. National Instruments Resource Manager Should Exist" )]
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
    [TestMethod( "07. Visa Implementations Should Be Enumerated" )]
    public void VisaImplementationsShouldBeEnumerated()
    {
        System.Collections.Generic.IEnumerable<Ivi.Visa.ConflictManager.VisaImplementation> installedVisas = Gac.Vendor.EnumerateDotNetImplementations();
        Assert.IsTrue( installedVisas.Any(), $"{nameof( installedVisas )} should have items" );
        string visaImplementationName = nameof( Ivi.Visa.ConflictManager.VisaImplementation );

        foreach ( Ivi.Visa.ConflictManager.VisaImplementation implementation in installedVisas )
        {
            string spaces = "  ";
            Console.WriteLine( $"{visaImplementationName}.{nameof( Ivi.Visa.ConflictManager.VisaImplementation.Location )}: {implementation.Location}" );
            Console.WriteLine( $"{spaces}{visaImplementationName}.{nameof( Ivi.Visa.ConflictManager.VisaImplementation.ApiType )}: {implementation.ApiType}" );
            Console.WriteLine( $"{spaces}{visaImplementationName}.{nameof( Ivi.Visa.ConflictManager.VisaImplementation.Comments )}: {implementation.Comments}" );
            Console.WriteLine( $"{spaces}{visaImplementationName}.{nameof( Ivi.Visa.ConflictManager.VisaImplementation.Enabled )}: {implementation.Enabled}" );
            Console.WriteLine( $"{spaces}{visaImplementationName}.{nameof( Ivi.Visa.ConflictManager.VisaImplementation.FriendlyName )}: {implementation.FriendlyName}" );
            Console.WriteLine( $"{spaces}{visaImplementationName}.{nameof( Ivi.Visa.ConflictManager.VisaImplementation.ResourceManufacturerId )}: {implementation.ResourceManufacturerId}" );
        }
    }
}
