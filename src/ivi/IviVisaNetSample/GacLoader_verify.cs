using System.Text;

namespace Ivi.VisaNet;

public static partial class GacLoader
{
    /// <summary>   Verify visa implementation. </summary>
    /// <remarks>   2025-08-12. </remarks>
    /// <exception cref="IOException">  Thrown when an I/O failure occurred. </exception>
    /// <param name="details">  [out] The details including information about the Visa Net shared
    ///                         components and loaded implementation assemblies. </param>
    /// <returns>   The loaded <see cref="System.Reflection.Assembly"/> </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>" )]
    public static System.Reflection.Assembly? VerifyVisaImplementation( out string details )
    {
        StringBuilder stringBuilder = new();

        // get the VISa.NET shared components info.
        System.Reflection.Assembly? visaNetShareComponentsAssembly = null;
        System.Version? visaNetSharedComponentsVersion = new();
        try
        {
            visaNetShareComponentsAssembly = GacLoader.GetVisaNetShareComponentsAssembly();
            if ( visaNetShareComponentsAssembly is not null )
            {
                FileInfo fileInfo = new( visaNetShareComponentsAssembly.Location );
                System.Diagnostics.FileVersionInfo versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo( visaNetShareComponentsAssembly.Location );
                _ = stringBuilder.AppendLine( "VISA.NET Shared Components assembly:" );
                _ = stringBuilder.AppendLine( $"\tFull name: {visaNetShareComponentsAssembly.GetName().FullName}" );
                _ = stringBuilder.AppendLine( $"\tFile name: {fileInfo.Name}" );
                _ = stringBuilder.AppendLine( $"\tLocation:  {fileInfo.DirectoryName}" );
                _ = stringBuilder.AppendLine( $"\tVersion:   {visaNetShareComponentsAssembly.GetName().Version}" );
                _ = stringBuilder.AppendLine( $"\tProduct:   {versionInfo.ProductVersion}" );
                _ = stringBuilder.AppendLine( $"\tFile:      {versionInfo.FileVersion}" );
            }
        }
        catch ( Exception ex )
        {
            throw new System.IO.IOException( $"Failed locating VISA NET shared components containing the {nameof( Ivi.Visa.GlobalResourceManager )} type.", ex );
        }

        if ( visaNetShareComponentsAssembly is null )
            throw new System.IO.IOException( $"A VISA.NET shared components assembly was not found. Most likely, a vendor-specific VISA implementation such as Keysight I/O Suite or NI.Visa was not installed." );

        // Check whether VISA Shared Components is installed before using VISA.NET.
        // If access VISA.NET without the visaConfMgr.dll library, an unhandled exception will
        // be thrown during termination process due to a bug in the implementation of the
        // VISA.NET Shared Components, and the application will crash.

        // Get the version of the VISA Configuration Manager File Version Info.
        System.Diagnostics.FileVersionInfo? visaConfigManagerFileVersionInfo = null;
        try
        {
            visaConfigManagerFileVersionInfo = GacLoader.VisaConfigManagerFileVersionInfo();
            if ( visaConfigManagerFileVersionInfo is not null )
            {
                _ = stringBuilder.AppendLine( $"\n{visaConfigManagerFileVersionInfo.InternalName} assembly:" );
                _ = stringBuilder.AppendLine( $"\tLocation: {visaConfigManagerFileVersionInfo.FileName}" );
                _ = stringBuilder.AppendLine( $"\tProduct:  {visaConfigManagerFileVersionInfo.ProductVersion}" );
                _ = stringBuilder.AppendLine( $"\tFile:     {visaConfigManagerFileVersionInfo.FileVersion}" );
            }
        }
        catch ( System.IO.FileNotFoundException ex )
        {
            throw new System.IO.IOException( $"The VISA Config Manager '{GacLoader.VisaConfigManagerFileName}' was not found. Most likely, a vendor-specific VISA implementation such as Keysight I/O Suite or NI.Visa was not installed.",
                ex );
        }
        catch ( Exception ex )
        {
            throw new System.IO.IOException( $"Failed getting the VISA Config Manager info from '{GacLoader.VisaConfigManagerFileName}'.", ex );
        }

        if ( visaConfigManagerFileVersionInfo is null )
            throw new System.IO.IOException( $"Failed getting the VISA Config Manager {GacLoader.VisaConfigManagerFileName} info. Most likely, a vendor-specific VISA implementation such as Keysight I/O Suite or NI.Visa was not installed." );

#if NET5_0_OR_GREATER
#if KelaryVisa
        if ( !GacLoader.TryLoadInstalledVisaAssemblies( out details ).Any() )
        {
            throw new System.IO.IOException( $"No VISA implementation assemblies were found in the GAC.\n\t{details}.\n\tMost likely, a vendor-specific VISA implementation such as Keysight I/O Suite or NI.VISA was not installed." );
        }
#endif
#endif

        // get the information from the vendor-specific VISA implementation that gets loaded upon calling the IVI.Visa Resource Manager parse method.
        System.Reflection.Assembly? loadedAssembly = null;
        string findDetails = string.Empty;

        try
        {
            // force loading the vendor implementation assembly
            loadedAssembly = GacLoader.TryFindLoadedImplementation( out findDetails );

            if ( loadedAssembly is not null )
            {
                FileInfo fileInfo = new( loadedAssembly.Location );
                System.Diagnostics.FileVersionInfo versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo( loadedAssembly.Location );
                _ = stringBuilder.AppendLine( "\nLoaded vendor implementation assembly:" );
                _ = stringBuilder.AppendLine( $"\tFull Name: {loadedAssembly.GetName().FullName}" );
                _ = stringBuilder.AppendLine( $"\tFile name: {fileInfo.Name}" );
                _ = stringBuilder.AppendLine( $"\tLocation:  {fileInfo.DirectoryName}" );
                _ = stringBuilder.AppendLine( $"\tProduct:   {versionInfo.ProductVersion}" );
                _ = stringBuilder.AppendLine( $"\tFile:      {versionInfo.FileVersion}" );
            }
        }
        catch ( Exception ex )
        {
            throw new System.IO.IOException( $"Failed finding a loaded vendor implementation; {findDetails}", ex );
        }

        if ( loadedAssembly is null )
            throw new System.IO.IOException( $"Failed finding a loaded vendor implementation; {findDetails}" );

        details = stringBuilder.ToString();
        return loadedAssembly;
    }

    /// <summary>   Try verify visa implementation. </summary>
    /// <remarks>   2026-01-15. </remarks>
    /// <param name="details">  [out] The details including information about the Visa Net shared
    ///                         components and loaded implementation assemblies. </param>
    /// <returns>   The loaded <see cref="System.Reflection.Assembly"/> </returns>
    public static System.Reflection.Assembly? TryVerifyVisaImplementation( out string details )
    {
        try
        {
            return GacLoader.VerifyVisaImplementation( out details );
        }
        catch ( Exception ex )
        {
            details = GacLoader.BuildErrorMessage( ex );
            if ( ex.InnerException is not null )
                details += $"\nInner Exception:\n{GacLoader.BuildErrorMessage( ex.InnerException )}";
            return null;
        }
    }
}
