#if NET5_0_OR_GREATER

using Ivi.Visa.ConflictManager;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ivi.VisaNet;
/// <summary>
/// Class used to load .NET Framework assemblies located in GAC from .NET 5+
/// Requred only for experimental using VISA.NET library in .NET 5+
/// </summary>
internal static class GacLoader
{
    /// <summary>   (Immutable) the visa configuration management assembly name. </summary>
    public const string VisaConfigManagerFileName = "visaConfMgr.dll";

    /// <summary>
    /// Load an assembly from the GAC.
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns>Loaded assembly</returns>
    /// <exception cref="FileNotFoundException"></exception>
    private static Assembly Load( AssemblyName assemblyName )
    {
        string[] gacPaths =
        [
           $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\Microsoft.NET\\assembly\\GAC_MSIL\\{assemblyName.Name}",
           $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\assembly\\GAC_MSIL\\{assemblyName.Name}",
        ];

        foreach ( string folder in gacPaths.Where( Directory.Exists ) )
        {
            foreach ( string subfolder in Directory.EnumerateDirectories( folder ) )
            {
                if ( subfolder.Contains( Convert.ToHexString( assemblyName.GetPublicKeyToken() ), StringComparison.OrdinalIgnoreCase )
                    && subfolder.Contains( assemblyName.Version.ToString(), StringComparison.OrdinalIgnoreCase ) )
                {
                    string assemblyPath = Path.Combine( subfolder, assemblyName.Name + ".dll" );
                    if ( File.Exists( assemblyPath ) )
                        return Assembly.LoadFrom( assemblyPath );
                }
            }
        }
        throw new FileNotFoundException( $"Assembly {assemblyName} not found." );
    }

    /// <summary>
    /// Preloading installed VISA implementation assemblies for NET 5+
    /// </summary>
    public static void LoadInstalledVisaAssemblies()
    {
        System.Collections.Generic.List<VisaImplementation> installedVisas = new ConflictManager().GetInstalledVisas( ApiType.DotNet );
        foreach ( VisaImplementation visaLibrary in installedVisas )
        {
            try
            {
                Assembly installedAssembly = GacLoader.Load( new AssemblyName( visaLibrary.Location[(visaLibrary.Location.IndexOf( ',' ) + 1)..] ) );
                // Console.WriteLine( $"Loaded assembly \"{visaLibrary.FriendlyName}\"." );
                Console.WriteLine( $"Loaded {installedAssembly.FullName}, {System.Diagnostics.FileVersionInfo.GetVersionInfo( installedAssembly.Location ).FileVersion}" );
            }
            catch ( Exception exception )
            {
                Console.WriteLine( $"Failed to load assembly \"{visaLibrary.FriendlyName}\": {exception.Message}" );
            }
        }
    }
}
#endif
