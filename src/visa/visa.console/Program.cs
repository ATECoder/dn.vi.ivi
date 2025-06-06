using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Forms;
using cc.isr.Logging.TraceLog;

namespace cc.isr.Visa.Console;

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
        System.Console.WriteLine();
        System.Reflection.Assembly? executionAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        System.Console.WriteLine( executionAssembly?.FullName );
        System.Console.WriteLine( $"Running under {framework?.FrameworkName}." );

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
            System.Console.WriteLine();
            System.Console.WriteLine( ex.ToString() );
            return;
        }

        // Preload installed VISA implementation assemblies
        cc.isr.Visa.Gac.GacLoader.LoadInstalledVisaAssemblies();
        System.Console.WriteLine( $"Loaded VISA implementation: {cc.isr.Visa.Gac.GacLoader.LoadedImplementation?.Location}." );

        Program.TraceLogger.CreateSerilogLogger( typeof( UI.VisaIoForm ) );
        Program.TraceLogger.MinimumLogLevel = Properties.Settings.Instance.ApplicationLogLevel;

#if NET8_0_OR_GREATER
        Application.SetHighDpiMode( HighDpiMode.SystemAware );
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault( false );
        Application.Run( new UI.VisaIoForm() );
    }
}
