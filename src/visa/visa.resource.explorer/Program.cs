using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Forms;
using cc.isr.Visa.ResourceExplorer;

#if NET5_0_OR_GREATER
_ = Application.SetHighDpiMode( HighDpiMode.SystemAware );
#endif
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault( false );

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

// open the explorer form.
Application.Run( new ExplorerForm() );

