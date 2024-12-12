using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ivi.Visa.ConflictManager;

namespace Ivi.VisaNet;
/// <summary>
/// Class used to load .NET Framework assemblies located in GAC from .NET 5+
/// Required only for experimental using VISA.NET library in .NET 5+
/// </summary>
public static class GacLoader
{
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
                if ( subfolder.Contains( BytesToHex( assemblyName.GetPublicKeyToken()! ), StringComparison.OrdinalIgnoreCase )
                    && subfolder.Contains( assemblyName.Version!.ToString(), StringComparison.OrdinalIgnoreCase ) )
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
    /// A <see cref="string" /> extension method that query if this String contains the given str.
    /// </summary>
    /// <remarks>   2023-04-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="str">          The string to act on. </param>
    /// <param name="substring">    The substring. </param>
    /// <param name="comp">         The string comparison option. </param>
    /// <returns>   True if the substring is contained in the String, false if not. </returns>
#if NET20_OR_GREATER
    private static bool Contains( this string str, string substring, StringComparison comp )
    {
        if ( substring is null ) throw new ArgumentNullException( nameof( substring ), $"{nameof( substring )} cannot be null." );
        else if ( !Enum.IsDefined( typeof( StringComparison ), comp ) )
            throw new ArgumentException( $"{nameof( comp )} is not a member of {nameof( StringComparison )}", nameof( comp ) );
        return str.IndexOf( substring, comp ) >= 0;
    }
#endif

    /// <summary>   Bytes to hexadecimal. </summary>
    /// <remarks>   2024-07-01. </remarks>
    /// <param name="byteArray">    Array of bytes. </param>
    /// <returns>   A string. </returns>
    private static string BytesToHex( byte[] byteArray )
    {
        // Convert byte array to hexadecimal string
        StringBuilder hexBuilder = new();
        foreach ( byte b in byteArray )
        {
#if NET5_0_OR_GREATER
            _ = hexBuilder.Append( System.Globalization.CultureInfo.CurrentCulture, $"{b:x2}" ); // X2 ensures two-digit representation
#else
            _ = hexBuilder.Append( $"{b:x2}" ); // X2 ensures two-digit representation
#endif
        }
        return hexBuilder.ToString();
    }

    /// <summary>
    /// Preloading installed VISA implementation assemblies for NET 5+
    /// </summary>
    public static void LoadInstalledVisaAssemblies()
    {
        System.Collections.Generic.List<VisaImplementation> installedVisas = new ConflictManager().GetInstalledVisas( ApiType.DotNet );
        foreach ( Ivi.Visa.ConflictManager.VisaImplementation visaLibrary in installedVisas )
        {
            try
            {
#if NET20_OR_GREATER
                Assembly installedAssembly = GacLoader.Load( new AssemblyName( visaLibrary.Location.Substring( visaLibrary.Location.IndexOf( "," ) + 1 ) ) );
#else
                Assembly installedAssembly = GacLoader.Load( new AssemblyName( visaLibrary.Location[(visaLibrary.Location.IndexOf( ',' ) + 1)..] ) );
#endif
                Console.WriteLine( "Loaded Visa Implementation:" );
                Console.WriteLine( $"   Friendly name: \"{visaLibrary.FriendlyName}\"." );
                Console.WriteLine( $"       Full name: \"{installedAssembly.FullName}\"." );
            }
            catch ( Exception exception )
            {
                Console.WriteLine( $"Failed to load assembly \"{visaLibrary.FriendlyName}\": {exception.Message}" );
            }
        }
    }
}
