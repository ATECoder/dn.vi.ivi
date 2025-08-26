using cc.isr.Logging.TraceLog;
using cc.isr.VI.Tsp.K2600.Ttm.Console.UI;

namespace cc.isr.VI.Tsp.K2600.Ttm.Console;

/// <summary>   A program. </summary>
/// <remarks>   David, 2021-07-22. </remarks>
internal static class Program
{
    /// <summary>   Gets or sets the trace logger. </summary>
    /// <value> The trace logger. </value>
    public static TraceLogger<TtmForm> TraceLogger { get; set; } = new();

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        TraceLogger.CreateLogger( typeof( TtmForm ) );
        TraceLogger.MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Trace;

#if NET5_0_OR_GREATER
        Application.SetHighDpiMode( HighDpiMode.SystemAware );
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault( false );
        Properties.Settings.Scribe?.ReadSettings();
        Application.Run( new TtmForm() );
    }
}
