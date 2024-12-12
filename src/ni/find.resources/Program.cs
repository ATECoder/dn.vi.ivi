using Ivi.Visa;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;

namespace NI.FindResources;

/// <summary>   A program. </summary>
/// <remarks>   David, 2021-07-22. </remarks>
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        TargetFrameworkAttribute? framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>();
        Console.WriteLine( $"Running under {framework?.FrameworkName ?? "--unable to resolve .NET Framework!"}." );

        // Get VISA.NET Shared Components version.
        Version? visaNetSharedComponentsVersion = typeof( GlobalResourceManager ).Assembly.GetName().Version;
        Console.WriteLine();
        Console.WriteLine( $"VISA.NET Shared Components version {visaNetSharedComponentsVersion?.ToString() ?? "was not resolved!"}." );

        // Check whether VISA Shared Components is installed before using VISA.NET.
        // If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
        // be thrown during termination process due to a bug in the implementation of the
        // VISA.NET Shared Components, and the application will crash.
        try
        {
            // Get an available version of the VISA Shared Components.
            FileVersionInfo visaSharedComponentsInfo = FileVersionInfo.GetVersionInfo( Path.Combine( Environment.SystemDirectory, "visaConfMgr.dll" ) );
            Console.WriteLine( $"The VISA Shared Components version {visaSharedComponentsInfo.ProductVersion} was detected." );
        }
        catch ( FileNotFoundException )
        {
            Console.WriteLine();
            Console.WriteLine( $"A VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} was not found. Please install a vendor-specific VISA implementation." );
            return;
        }

#if NET5_0_OR_GREATER
        Console.WriteLine();
        // Preloading installed VISA implementation assemblies for NET 5+
        Ivi.VisaNet.GacLoader.LoadInstalledVisaAssemblies();
#endif


#if NET5_0_OR_GREATER
        Application.SetHighDpiMode( HighDpiMode.SystemAware );
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault( false );
        Application.Run( new MainForm() );
    }
}
