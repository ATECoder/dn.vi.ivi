using System;
using System.Collections.Generic;
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

// preload the VISA assemblies.
if ( cc.isr.Visa.Gac.GacLoader.TryLoadInstalledVisaAssemblies( out string details ) is IList<Assembly> installedAssemblies && installedAssemblies.Count > 0 )
{
    int count = installedAssemblies.Count;
    if ( count > 1 )
        Console.WriteLine( $"\nLoaded multiple ({count}) VISA .NET implementation assemblies:\n\t{details}" );
    else
        Console.WriteLine( $"\nLoaded VISA .NET implementation assembly:\n\t{details}" );
    // foreach ( Assembly assembly in assemblies )
    // {
    //     Console.WriteLine( $"\t{assembly.FullName}, {System.Diagnostics.FileVersionInfo.GetVersionInfo( assembly.Location ).FileVersion}" );
    // }
}
else
{
    Console.WriteLine( $"\nNo VISA .NET implementation assemblies loaded:\n\t{details}" );
    return;
}

// open the explorer form.
Application.Run( new ExplorerForm() );

