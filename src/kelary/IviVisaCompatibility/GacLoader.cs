using System;
using System.Linq;
using System.Reflection;
using Ivi.Visa;
using Ivi.Visa.ConflictManager;

namespace Ivi.VisaNet;
/// <summary>
/// Class used to load .NET Framework assemblies located in GAC from .NET 5+
/// Required only for experimental using VISA.NET library in .NET 5+
/// </summary>
public static class GacLoader
{
    /// <summary>   (Immutable) the visa configuration management assembly name. </summary>
    public const string VisaConfigManagerFileName = "visaConfMgr.dll";

    /// <summary>   Load an assembly from the GAC. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <exception cref="FileNotFoundException">    . </exception>
    /// <param name="assemblyName"> The <see cref="System.Reflection.AssemblyName"/>. </param>
    /// <returns>   Loaded assembly. </returns>
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
    private static bool Contains( this string str, string substring, StringComparison comp )
    {
        if ( substring is null ) throw new ArgumentNullException( nameof( substring ), $"{nameof( substring )} cannot be null." );
        else if ( !Enum.IsDefined( typeof( StringComparison ), comp ) )
            throw new ArgumentException( $"{nameof( comp )} is not a member of {nameof( StringComparison )}", nameof( comp ) );
        return str.IndexOf( substring, comp ) >= 0;
    }

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
            _ = hexBuilder.Append( $"{b:x2}" ); // X2 ensures two-digit representation
        }
        return hexBuilder.ToString();
    }

    private static System.Reflection.Assembly? _visaNetShareComponentsAssembly;
    /// <summary>   Gets visa net share components assembly. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <returns>   The visa net share components version. </returns>
    public static System.Reflection.Assembly? GetVisaNetShareComponentsAssembly()
    {
        _visaNetShareComponentsAssembly ??= typeof( Ivi.Visa.GlobalResourceManager ).Assembly;
        return _visaNetShareComponentsAssembly;
    }

    /// <summary>   Gets the visa configuration manager file version info. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <returns>   The visa configuration manager file version info. </returns>
    public static System.Diagnostics.FileVersionInfo? VisaConfigManagerFileVersionInfo()
    {
        return System.Diagnostics.FileVersionInfo.GetVersionInfo( System.IO.Path.Combine( Environment.SystemDirectory, GacLoader.VisaConfigManagerFileName ) );
    }

    /// <summary>   Static constructor. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <exception cref="System.IO.IOException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    public static System.Version? VerifyVisaImplementationPresence()
    {
        // get the shared components version.
        System.Version? visaNetSharedComponentsVersion = new();
        try
        {
            Assembly visaNetSharedComponentsAssembly = typeof( Ivi.Visa.GlobalResourceManager ).Assembly;
            visaNetSharedComponentsVersion = visaNetSharedComponentsAssembly.GetName().Version;
            Console.WriteLine();
            Console.WriteLine( $"VISA.NET Shared Components {visaNetSharedComponentsAssembly.GetName()}." );
            Console.WriteLine( $"\tVersion: {System.Diagnostics.FileVersionInfo.GetVersionInfo( typeof( GlobalResourceManager ).Assembly.Location ).FileVersion}." );
        }
        catch ( Exception ex )
        {
            throw new System.IO.IOException( $"Failed locating VISA NET shared components containing the {nameof( Ivi.Visa.GlobalResourceManager )} type.", ex );
        }

        // Check whether VISA Shared Components is installed before using VISA.NET.
        // If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
        // be thrown during termination process due to a bug in the implementation of the
        // VISA.NET Shared Components, and the application will crash.
        try
        {
            // Get an available version of the VISA Shared Components.
            System.Diagnostics.FileVersionInfo? visaSharedComponentsInfo = GacLoader.VisaConfigManagerFileVersionInfo();
            if ( visaSharedComponentsInfo is not null )
                Console.WriteLine( $"\t{visaSharedComponentsInfo.InternalName} version {visaSharedComponentsInfo.ProductVersion} detected." );
            else
                throw new System.IO.IOException( $"\tFailed getting the VISA shared component {GacLoader.VisaConfigManagerFileName} info." );
        }
        catch ( System.IO.FileNotFoundException ex )
        {
            Console.WriteLine();
            throw new System.IO.IOException( $"VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} not found. Please install corresponding vendor-specific VISA implementation first.",
                ex );
        }

        catch ( System.IO.IOException )
        {
            throw;
        }

        return visaNetSharedComponentsVersion;

    }

    /// <summary>
    /// Preloading installed VISA implementation assemblies for NET 5+
    /// </summary>
    public static void LoadInstalledVisaAssemblies()
    {
        System.Collections.Generic.List<Ivi.Visa.ConflictManager.VisaImplementation> installedVisas = new Ivi.Visa.ConflictManager.ConflictManager().GetInstalledVisas( ApiType.DotNet );
        foreach ( Ivi.Visa.ConflictManager.VisaImplementation visaLibrary in installedVisas )
        {
            try
            {
                System.Reflection.Assembly installedAssembly = GacLoader.Load( new System.Reflection.AssemblyName( visaLibrary.Location.Substring( visaLibrary.Location.IndexOf( "," ) + 1 ) ) );
                Console.WriteLine( $"Loaded {installedAssembly.FullName}, {System.Diagnostics.FileVersionInfo.GetVersionInfo( installedAssembly.Location ).FileVersion}" );
            }
            catch ( Exception exception )
            {
                throw new System.IO.IOException( $"Failed to load assembly \"{visaLibrary.FriendlyName}\": {exception.Message}", exception );
            }
        }
    }
}
