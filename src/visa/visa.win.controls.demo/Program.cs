using System.Reflection;
using System.Runtime.Versioning;

namespace cc.isr.Visa.WinControls;

/// <summary>   A program. </summary>
/// <remarks>   David, 2021-07-22. </remarks>
internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        TargetFrameworkAttribute? framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>();
        Console.WriteLine();
        System.Reflection.Assembly? executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        Console.WriteLine( executingAssembly?.FullName );
        Console.WriteLine( $"Running under {framework?.FrameworkName}." );

        // Check whether VISA Shared Components is installed before using VISA.NET.
        // If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
        // be thrown during termination process due to a bug in the implementation of the
        // VISA.NET Shared Components, and the application will crash.
        Version? visaNetSharedComponentsVersion;
        try
        {
            visaNetSharedComponentsVersion = cc.isr.Visa.Gac.GacLoader.VerifyVisaImplementationPresence();
        }
        catch ( System.IO.IOException ex )
        {
            Console.WriteLine();
            Console.WriteLine( ex.ToString() );
            return;
        }

        // Preload installed VISA implementation assemblies
        cc.isr.Visa.Gac.GacLoader.LoadInstalledVisaAssemblies();

#if NET5_0_OR_GREATER
        Application.SetHighDpiMode( HighDpiMode.SystemAware );
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault( false );
        Application.Run( new Demo.Switchboard() );
    }
}
