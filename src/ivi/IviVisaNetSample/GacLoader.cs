#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1305

namespace Ivi.VisaNet;
#pragma warning restore IDE0079 // Remove unnecessary suppression

/// <summary>
/// Class used to load .NET Framework assemblies located in GAC from .NET 5+,
/// Required only for experimental using VISA.NET library in .NET 5+.
/// </summary>
/// <remarks>
/// Loading is not required for implementations using IviFoundation.Visa packages from 8.0.2 and above.
/// </remarks>
public static partial class GacLoader
{
    /// <summary>   (Immutable) the visa configuration management assembly name. </summary>
    public const string VisaConfigManagerFileName = "visaConfMgr.dll";

    /// <summary>   Gets the file version information of the visa configuration manager. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <returns>   The file version information of the visa configuration manager. </returns>
    public static System.Diagnostics.FileVersionInfo? VisaConfigManagerFileVersionInfo()
    {
        return System.Diagnostics.FileVersionInfo.GetVersionInfo( System.IO.Path.Combine( Environment.SystemDirectory, GacLoader.VisaConfigManagerFileName ) );
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

    /// <summary>   Gets visa net share components version. </summary>
    /// <remarks>   2024-07-02. </remarks>
    /// <returns>   The visa net share components version. </returns>
    public static System.Version? GetVisaNetShareComponentsVersion()
    {
        System.Reflection.Assembly? assembly = GacLoader.GetVisaNetShareComponentsAssembly();
        return assembly?.GetName().Version;
    }

    /// <summary>   Verify visa implementation presence. </summary>
    /// <remarks>   2025-08-12. </remarks>
    /// <exception cref="IOException">  Thrown when an I/O failure occurred. </exception>
    /// <param name="verbose">  (Optional) True to verbose. </param>
    /// <returns>   A System.Version? </returns>
    public static System.Version? VerifyVisaImplementationPresence( bool verbose = false )
    {
        // get the shared components version.
        System.Version? visaNetSharedComponentsVersion = new();
        try
        {
            System.Reflection.Assembly? assembly = GacLoader.GetVisaNetShareComponentsAssembly();
            if ( assembly is null )
                if ( verbose )
                    Console.WriteLine( $"\nVISA.NET Shared Components assembly not found." );
                else
                    throw new System.IO.IOException( $"*** Failed locating VISA NET shared components assembly" );
            else
            {
                visaNetSharedComponentsVersion = assembly.GetName().Version;
                if ( verbose )
                {
                    Console.WriteLine( $"\nVISA.NET Shared Components {assembly.GetName()}." );
                    Console.WriteLine( $"\tVersion: {System.Diagnostics.FileVersionInfo.GetVersionInfo( assembly.Location ).FileVersion}." );
                }
            }
        }
        catch ( Exception ex )
        {
            throw new System.IO.IOException( $"*** Failed locating VISA NET shared components containing the {nameof( Ivi.Visa.GlobalResourceManager )} type.", ex );
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
            {
                if ( verbose )
                    Console.WriteLine( $"\t{visaSharedComponentsInfo.InternalName} version {visaSharedComponentsInfo.ProductVersion} detected." );
            }
            else
                throw new System.IO.IOException( $"\t*** Failed getting the VISA shared component {GacLoader.VisaConfigManagerFileName} info." );
        }
        catch ( System.IO.FileNotFoundException ex )
        {
            Console.WriteLine();
            throw new System.IO.IOException( $"*** VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} not found. Please install corresponding vendor-specific VISA implementation first.",
                ex );
        }

        catch ( System.IO.IOException )
        {
            throw;
        }

        return visaNetSharedComponentsVersion;

    }
}
