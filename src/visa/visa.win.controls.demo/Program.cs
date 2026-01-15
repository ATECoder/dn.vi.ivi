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
        Console.WriteLine( $"\tRunning under {framework?.FrameworkName ?? "--unable to resolve .NET Framework!"} runtime {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}" );

        if ( cc.isr.Visa.Gac.GacLoader.TryVerifyVisaImplementation( out string details ) is null )
        {
            Console.WriteLine( details );
            return;
        }

#if NET5_0_OR_GREATER
        Application.SetHighDpiMode( HighDpiMode.SystemAware );
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault( false );
        Application.Run( new Demo.Switchboard() );
    }
}
