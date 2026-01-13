using System.Reflection;
using System.Text;
using Ivi.Visa.ConflictManager;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1305

namespace Ivi.VisaNet;
#pragma warning restore IDE0079 // Remove unnecessary suppression

/// <summary>
/// Class used to load .NET Framework assemblies located in GAC from .NET 5+
/// Required only for experimental using VISA.NET library in .NET 5+
/// </summary>
public static partial class GacLoader
{
    /// <summary>
    /// Gets or sets a value indicating whether this object has dot net implementations.
    /// </summary>
    /// <value> True if this object has dot net implementations, false if not. </value>
    public static bool? HasDotNetImplementations { get; private set; }

    /// <summary>   Bytes to hexadecimal. </summary>
    /// <remarks>   2024-07-01. </remarks>
    /// <param name="byteArray">    Array of bytes. </param>
    /// <returns>   A string. </returns>
    private static string BytesToHex( byte[] byteArray )
    {
        // Convert byte array to hexadecimal string
        System.Text.StringBuilder hexBuilder = new();
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

    /// <summary>
    /// Load an assembly from the GAC.
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns>Loaded assembly</returns>
    /// <exception cref="FileNotFoundException"></exception>
    private static System.Reflection.Assembly Load( System.Reflection.AssemblyName assemblyName )
    {
        string[] gacPaths =
        [
           $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\Microsoft.NET\\assembly\\GAC_MSIL\\{assemblyName.Name}",
           $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\assembly\\GAC_MSIL\\{assemblyName.Name}",
        ];

        foreach ( string folder in gacPaths.Where( System.IO.Directory.Exists ) )
        {
            foreach ( string subfolder in System.IO.Directory.EnumerateDirectories( folder ) )
            {
                if ( subfolder.Contains( BytesToHex( assemblyName.GetPublicKeyToken()! ), StringComparison.OrdinalIgnoreCase )
                    && subfolder.Contains( assemblyName.Version!.ToString(), StringComparison.OrdinalIgnoreCase ) )
                {
                    string assemblyPath = System.IO.Path.Combine( subfolder, assemblyName.Name + ".dll" );
                    if ( System.IO.File.Exists( assemblyPath ) )
                        return System.Reflection.Assembly.LoadFrom( assemblyPath );
                }
            }
        }
        throw new System.IO.FileNotFoundException( $"Assembly {assemblyName} not found." );
    }

    /// <summary>   Gets a list of names of the loaded implementation friendlies. </summary>
    /// <value> A list of names of the loaded implementation friendlies. </value>
    public static IList<string> LoadedImplementationFriendlyNames { get; } = [];

    /// <summary>   Gets a list of names of the loaded implementation files. </summary>
    /// <value> A list of names of the loaded implementation files. </value>
    public static IList<string> LoadedImplementationFileNames { get; } = [];

    /// <summary>   Try to load all the installed VISA implementation assemblies. </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>
    /// An enumerator that allows foreach to be used to process load installed visa assemblies in
    /// this collection.
    /// </returns>
    public static IEnumerable<Assembly> TryLoadInstalledVisaAssemblies( out string details )
    {
        StringBuilder sb = new();
        IList<Assembly> installedAssemblies = [];
        GacLoader.HasDotNetImplementations = false;
        GacLoader.LoadedImplementationFriendlyNames.Clear();
        GacLoader.LoadedImplementationFileNames.Clear();

        foreach ( Ivi.Visa.ConflictManager.VisaImplementation visaLibrary in new Ivi.Visa.ConflictManager.ConflictManager().GetInstalledVisas( ApiType.DotNet ) )
        {
            try
            {
                string fileName =
#if NET20_OR_GREATER
                visaLibrary.Location.Substring( visaLibrary.Location.IndexOf( ',' ) + 1 );
#else
                visaLibrary.Location[(visaLibrary.Location.IndexOf( ',' ) + 1)..];
#endif
                Assembly installedAssembly = GacLoader.Load( new System.Reflection.AssemblyName( fileName ) );
                _ = sb.Append( $"{installedAssembly.FullName}, {System.Diagnostics.FileVersionInfo.GetVersionInfo( installedAssembly.Location ).FileVersion}" );
                installedAssemblies.Add( installedAssembly );

                GacLoader.HasDotNetImplementations = true;
                GacLoader.LoadedImplementationFriendlyNames.Add( visaLibrary.FriendlyName );
                GacLoader.LoadedImplementationFileNames.Add( fileName );
            }
            catch ( Exception exception )
            {
                _ = sb.Append( $"*** Failed to load assembly {visaLibrary.FriendlyName}: {exception.Message}" );
            }
        }
        details = sb.ToString();
        return installedAssemblies;
    }

    /// <summary>   Loads installed visa assemblies. </summary>
    /// <remarks>   2025-09-18. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    public static void LoadInstalledVisaAssemblies()
    {
        if ( GacLoader.TryLoadInstalledVisaAssemblies( out string details ) is not IList<Assembly> assemblies || (assemblies.Count == 0) )
        {
            throw new InvalidOperationException( $"\nNo VISA .NET implementation assemblies loaded: {details}" );
        }
    }
}
