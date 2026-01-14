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

// open the explorer form.
Application.Run( new ExplorerForm() );

