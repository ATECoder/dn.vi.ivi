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

    /// <summary>   Gets visa net share components product version. </summary>
    /// <remarks>   2026-01-16. </remarks>
    /// <returns>   The visa net share components product version. </returns>
    public static System.Version? GetVisaNetShareComponentsProductVersion()
    {
        System.Reflection.Assembly? assembly = GacLoader.GetVisaNetShareComponentsAssembly();
        if ( assembly is null )
            return null;

        System.Diagnostics.FileVersionInfo versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo( assembly.Location );
        return versionInfo.ProductVersion is not null ? new System.Version( versionInfo.ProductVersion ) : null;
    }
}
