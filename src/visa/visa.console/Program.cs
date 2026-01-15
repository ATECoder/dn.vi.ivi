using System.Reflection;
using System.Runtime.Versioning;
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
        System.Console.WriteLine( $"\tRunning under {framework?.FrameworkName ?? "--unable to resolve .NET Framework!"} runtime {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );

        if ( cc.isr.Visa.Gac.GacLoader.TryVerifyVisaImplementation( out string details ) is null )
        {
            System.Console.WriteLine( details );
            return;
        }

        Program.TraceLogger.CreateLogger( typeof( UI.VisaIoForm ) );
        Program.TraceLogger.MinimumLogLevel = Properties.Settings.Instance.ApplicationLogLevel;

#if NET5_0_OR_GREATER
        Application.SetHighDpiMode( HighDpiMode.SystemAware );
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault( false );
        Application.Run( new UI.VisaIoForm() );
    }
}
