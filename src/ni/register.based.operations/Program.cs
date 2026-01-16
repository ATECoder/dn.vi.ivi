using Ivi.Visa;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;

namespace NI.RegisterBasedOperations;

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
        Console.WriteLine( $"Running under {framework?.FrameworkName ?? "--unable to resolve .NET Framework!"} runtime {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );

        // Get VISA.NET Shared Components version.
        Assembly visaNetSharedComponentsAssembly = typeof( GlobalResourceManager ).Assembly;
        Version? visaNetSharedComponentsVersion = visaNetSharedComponentsAssembly.GetName().Version;
        Console.WriteLine();
        Console.WriteLine( $"VISA.NET Shared Components {visaNetSharedComponentsAssembly.GetName()}." );
        Console.WriteLine( $"	Version: {System.Diagnostics.FileVersionInfo.GetVersionInfo( typeof( GlobalResourceManager ).Assembly.Location ).FileVersion}." );

        // Check whether VISA Shared Components is installed before using VISA.NET.
        // If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
        // be thrown during termination process due to a bug in the implementation of the
        // VISA.NET Shared Components, and the application will crash.
        try
        {
            // Get an available version of the VISA Shared Components.
            string visaConfigManagerPath = Path.Combine( Environment.SystemDirectory, "visaConfMgr.dll" );
            FileVersionInfo visaSharedComponentsInfo = FileVersionInfo.GetVersionInfo( visaConfigManagerPath );
            Console.WriteLine( $"	{visaSharedComponentsInfo.InternalName} version {visaSharedComponentsInfo.ProductVersion} detected." );
        }
        catch ( FileNotFoundException )
        {
            Console.WriteLine();
            Console.WriteLine( $"A VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} was not found. Please install a vendor-specific VISA implementation." );
            return;
        }

#if NET5_0_OR_GREATER
        Application.SetHighDpiMode( HighDpiMode.SystemAware );
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault( false );
        Application.Run( new MainForm() );
    }
}
