using System.Reflection;
using Ivi.Visa;

namespace cc.isr.Visa.Gac;

public static partial class GacLoader
{
    /// <summary>   Try open session. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   An Ivi.Visa.IVisaSession? </returns>
    [CLSCompliant( false )]
    public static Ivi.Visa.IVisaSession? TryOpenSession( string resourceName, out string details )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) )
        {
            details = $"{nameof( resourceName )} is null or empty or white space.";
            return null;
        }
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
        System.Text.StringBuilder sb = new();
        string line = $"Opening a VISA session to '{resourceName}' by:";
        _ = sb.AppendLine( line );
        line = $"\tIvi.Visa.{nameof( GlobalResourceManager )}.{nameof( GlobalResourceManager.ImplementationVersion )}:{GlobalResourceManager.ImplementationVersion}";
        _ = sb.AppendLine( line );
        line = $"\tIvi.Visa.{nameof( GlobalResourceManager )}.{nameof( GlobalResourceManager.SpecificationVersion )}:{GlobalResourceManager.SpecificationVersion}";
        _ = sb.AppendLine( line );
        try
        {
            Ivi.Visa.IVisaSession visaSession = Ivi.Visa.GlobalResourceManager.Open( resourceName, Ivi.Visa.AccessModes.ExclusiveLock, 2000 );
            line = $"\t{visaSession.GetType()} Visa session opened to '{resourceName}'.";
            _ = sb.AppendLine( line );
            details = sb.ToString();
            return visaSession;
        }
        catch ( Exception ex )
        {
            line = $"*** Failed opening session to '{resourceName}'; {GacLoader.BuildErrorMessage( ex )}";
            _ = sb.AppendLine( line );
            details = sb.ToString();
            return null;
        }
    }

    /// <summary>   Builds error message. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <param name="exception">    The exception. </param>
    /// <returns>   A string. </returns>
    public static string BuildErrorMessage( Exception exception )
    {
        Assembly? visaNetSharedComponentsAssembly = GacLoader.GetVisaNetShareComponentsAssembly();
        Version? visaNetSharedComponentsVersion = visaNetSharedComponentsAssembly?.GetName().Version;

        if ( exception is TypeInitializationException && exception.InnerException is DllNotFoundException )
        {
            // VISA Shared Components is not installed.
            return $"VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} not found. Please install corresponding vendor-specific VISA implementation first.";
        }
        else if ( exception is VisaException && exception.Message == "No vendor-specific VISA .NET implementation is installed." )
        {
            // Vendor-specific VISA.NET implementation is not available.
            return $"VISA implementation compatible with VISA.NET Shared Components {visaNetSharedComponentsVersion} not found. Please install corresponding vendor-specific VISA implementation first.";
        }
        else if ( exception is VisaException )
        {
            // General VISA Exception.
            return $"VISA Exception: {exception}";
        }
        else if ( exception is EntryPointNotFoundException )
        {
            // Installed VISA Shared Components are not compatible with VISA.NET Shared Components.
            return $"Installed VISA Shared Components version {visaNetSharedComponentsVersion} does not support VISA.NET. Please upgrade VISA implementation.";
        }
        else
        {
            // Handle remaining errors.
            return $"Exception: {exception}";
        }
    }
}
