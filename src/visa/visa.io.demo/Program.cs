using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Forms;
using cc.isr.Logging.TraceLog;

namespace cc.isr.Visa.IO.Demo;

/// <summary>   A program. </summary>
/// <remarks>   David, 2021-07-22. </remarks>
internal static class Program
{
    /// <summary>   Gets or sets the trace logger. </summary>
    /// <value> The trace logger. </value>
    public static TraceLogger<UI.VisaIoForm> TraceLogger { get; set; } = new();

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        TargetFrameworkAttribute? framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>();
        Console.WriteLine();
        System.Reflection.Assembly? executionAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        Console.WriteLine( executionAssembly?.FullName );
        Console.WriteLine( $"\tRunning under {framework?.FrameworkName ?? "--unable to resolve .NET Framework!"} runtime {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );

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
        Console.WriteLine( $"Loaded VISA implementation: {cc.isr.Visa.Gac.GacLoader.LoadedImplementation?.Location}." );

        Program.TraceLogger.CreateLogger( typeof( UI.VisaIoForm ) );
        Program.TraceLogger.MinimumLogLevel = Properties.Settings.Instance.ApplicationLogLevel;

#if NET8_0_OR_GREATER
        Application.SetHighDpiMode( HighDpiMode.SystemAware );
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault( false );
        Application.Run( new UI.VisaIoForm() );
    }
}
