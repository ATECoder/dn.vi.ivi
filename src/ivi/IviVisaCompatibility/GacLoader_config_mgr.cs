using System.Reflection;
using System.Text;
using Ivi.Visa;

namespace Ivi.VisaNet;

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

    /// <summary>   Gets a list of qualified names of the visa implementations. </summary>
    /// <value> A list of qualified names of the visa implementations. </value>
    public static IList<string> VisaImplementationQualifiedNames { get; } = [];

    /// <summary>   Gets a list of names of the loaded implementation friendlies. </summary>
    /// <value> A list of names of the loaded implementation friendlies. </value>
    public static IList<string> VisaImplementationFriendlyNames { get; } = [];

    /// <summary>   Gets a list of names of the loaded implementation files. </summary>
    /// <value> A list of names of the loaded implementation files. </value>
    public static IList<string> VisaImplementationFileNames { get; } = [];

    /// <summary>   Enumerate the installed VISA implementation assemblies. </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <param name="details">  [out] The details. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0057:Use range operator", Justification = "<Pending>" )]
    public static bool TryEnumerateInstalledVisaAssemblies( out string details )
    {
        StringBuilder sb = new();
        bool hasDotNetImplementations = false;
        GacLoader.VisaImplementationQualifiedNames.Clear();
        GacLoader.VisaImplementationFriendlyNames.Clear();
        GacLoader.VisaImplementationFileNames.Clear();

        foreach ( Ivi.Visa.ConflictManager.VisaImplementation visaLibrary in new Ivi.Visa.ConflictManager.ConflictManager().GetInstalledVisas( Ivi.Visa.ConflictManager.ApiType.DotNet ) )
        {
            try
            {
                hasDotNetImplementations = true;
                GacLoader.VisaImplementationQualifiedNames.Add( visaLibrary.Location );
                GacLoader.VisaImplementationFriendlyNames.Add( visaLibrary.FriendlyName );
                GacLoader.VisaImplementationFileNames.Add( visaLibrary.Location.Substring( visaLibrary.Location.IndexOf( ',' ) + 1 ) );
            }
            catch ( Exception exception )
            {
                details = $"*** Failed to enumerate VISA Implementation {visaLibrary.FriendlyName}: {GacLoader.BuildErrorMessage( exception )}";
            }
        }
        details = sb.ToString();
        return hasDotNetImplementations;
    }

    /// <summary>   Creates vendor resource manager. </summary>
    /// <remarks>   David, 2021-11-06. </remarks>
    /// <param name="typeName"> Name of the type. </param>
    /// <returns>   The new vendor resource manager. </returns>
    [CLSCompliant( false )]
    public static IResourceManager? CreateVendorResourceManager( string typeName )
    {
        Type? type = Type.GetType( typeName );
        return type is null
                ? default
                : Activator.CreateInstance( type ) is IResourceManager resourceManager
                    ? resourceManager
                    : null;
    }

    /// <summary>   Queries if a resource manger matching the specified type nam exists. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <param name="typeName"> Name of the type. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) IsResourceMangerExists( string typeName )
    {
        if ( string.IsNullOrWhiteSpace( typeName ) )
            return (false, $"The provided {nameof( typeName )} is null or white space.");
        else
        {
            return GacLoader.CreateVendorResourceManager( typeName ) is null
                ? (false, $"Could not create a resource manage for the provided '{typeName}' type name")
                : (true, string.Empty);
        }
    }

    /// <summary>   Enumerates the .NET implementations in this collection. </summary>
    /// <remarks>   David, 2021-11-06. </remarks>
    /// <returns>
    /// An enumerator that allows foreach to be used to process the implementations in this
    /// collection.
    /// </returns>
    [CLSCompliant( false )]
    public static IEnumerable<Ivi.Visa.ConflictManager.VisaImplementation> EnumerateDotNetImplementations()
    {
        Ivi.Visa.ConflictManager.ConflictManager conflictManager = new();
        return conflictManager.GetInstalledVisas( Ivi.Visa.ConflictManager.ApiType.DotNet );
    }

    /// <summary>   Query if this object has dot net implementations. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   True if dot net implementations; otherwise false. </returns>
    public static bool HasDotNetImplementations()
    {
        Ivi.Visa.ConflictManager.ConflictManager conflictManager = new();
        return conflictManager.GetInstalledVisas( Ivi.Visa.ConflictManager.ApiType.DotNet ).Count > 0;
    }

    /// <summary>   Try find implementation. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <param name="friendlyName"> Name of the friendly. </param>
    /// <returns>   A Tuple. </returns>
    [CLSCompliant( false )]
    public static (Ivi.Visa.ConflictManager.VisaImplementation? implementation, string details) TryFindImplementation( string friendlyName )
    {
        System.Collections.Generic.IEnumerable<Ivi.Visa.ConflictManager.VisaImplementation> installedVisas = GacLoader.EnumerateDotNetImplementations();
        if ( installedVisas is null || !installedVisas.Any() )
            return (null, "No VISA implementation where found.");
        else
        {
            foreach ( Ivi.Visa.ConflictManager.VisaImplementation implementation in installedVisas )
            {
                if ( string.Equals( friendlyName, implementation.FriendlyName, StringComparison.OrdinalIgnoreCase ) )
                    return (implementation, string.Empty);
            }
            return (null, $"No VISA implementation with the '{friendlyName}' was found among the existing {installedVisas.Count()} implementation(s).");
        }
    }

    /// <summary>
    /// Checks if an IVI Visa assembly with the specified full name exists in the current execution
    /// path.
    /// </summary>
    /// <remarks>
    /// 2024-07-13. <para>
    /// 2025-08-12: Ivi.Visa.dll no longer exist in the execution path for the .NET framework. The
    /// shared assembly detected by the GAC Loader is used to verify the existence of the IVI Visa
    /// assembly.
    /// </para>
    /// </remarks>
    /// <param name="fileName"> (Optional) Filename of the file, e.g., Ivi.Visa.dll. </param>
    /// <param name="fullName"> (Optional) The visa assembly full name, e.g., Ivi.Visa,
    ///                         Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) IsIviVisaAssemblyExists( string fileName = "Ivi.Visa.dll",
        string fullName = "Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1" )
    {
        if ( GacLoader.GetVisaNetShareComponentsAssembly() is not System.Reflection.Assembly visaNetSharedComponentsAssembly )
            return (false, "Failed getting the VISA.NET Shared Components assembly.");
        else if ( string.IsNullOrWhiteSpace( fileName ) || string.IsNullOrWhiteSpace( fullName ) )
            return (false, "The IVI filename or full name is null or white space.");
        else if ( visaNetSharedComponentsAssembly.ManifestModule.Name is not string assemblyName )
            return (false, $"The VISA.NET Shared Components assembly name failed to return value.");
        else if ( !string.Equals( fileName, assemblyName, StringComparison.OrdinalIgnoreCase ) )
            return (false, $"The VISA.NET Shared Components assembly name '{assemblyName}' does not match the expected IVI VISA filename '{fileName}'.");
        else if ( visaNetSharedComponentsAssembly.FullName is not string actualFullName )
            return (false, "The VISA.NET Shared Components assembly full name is null or white space.");
        else if ( !string.Equals( fullName, actualFullName, StringComparison.OrdinalIgnoreCase ) )
            return (false, $"The VISA.NET Shared Components assembly full name '{actualFullName}' does not match the expected IVI VISA full name '{fullName}'.");
        else
            return (true, string.Empty);
    }

    /// <summary>   Query if the assembly with the 'assemblyFullName' full name is loaded. </summary>
    /// <remarks>   2026-01-13. </remarks>
    /// <param name="assemblyFullName"> Full name of the assembly. </param>
    /// <returns>   True if assembly loaded; otherwise false. </returns>
    public static bool IsAssemblyLoaded( string assemblyFullName )
    {
        // Get all assemblies currently loaded into the current AppDomain
        Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Check if any assembly in the loaded list matches the full name
        return loadedAssemblies.Any( assembly => assembly.FullName == assemblyFullName );
    }

    /// <summary>   Query if assembly with the 'assemblyName' is loaded. </summary>
    /// <remarks>   2026-01-13. </remarks>
    /// <param name="assemblyName"> Name of the assembly. </param>
    /// <returns>   True if assembly loaded; otherwise false. </returns>
    public static bool IsAssemblyNameLoaded( string assemblyName )
    {
        // Get all assemblies currently loaded into the current AppDomain
        Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Check if any assembly in the loaded list matches the full name
        return loadedAssemblies.Any( assembly => assembly.GetName().Name == assemblyName );
    }

    /// <summary>   Try find assembly by friendly name. </summary>
    /// <remarks>   2026-01-13. </remarks>
    /// <param name="friendlyName"> Name of the friendly. </param>
    /// <returns>   A Tuple. </returns>
    public static (Assembly?, string details) TryFindLoadedAssemblyByFriendlyName( string friendlyName )
    {
        // Get all assemblies currently loaded into the current AppDomain
        Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach ( Assembly assembly in loadedAssemblies )
        {
            if ( string.Equals( assembly.GetName().Name, friendlyName, StringComparison.OrdinalIgnoreCase ) )
                return (assembly, string.Empty);
        }
        return (null, $"No assembly with the friendly name '{friendlyName}' was found among the loaded assemblies.");
    }

    /// <summary>   Try find loaded implementation. </summary>
    /// <remarks>   2026-01-14. </remarks>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   An Assembly? </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0057:Use range operator", Justification = "<Pending>" )]
    public static Assembly? TryFindLoadedImplementation( out string details )
    {
        Assembly? loadedAssembly = null;
        StringBuilder failureBuilder = new();
        StringBuilder sb = new();

        // use this method to force loading the vendor implementation assembly
        _ = Ivi.Visa.GlobalResourceManager.TryParse( "GPIB::24::INST", out _ );

        foreach ( Ivi.Visa.ConflictManager.VisaImplementation visaLibrary in new Ivi.Visa.ConflictManager.ConflictManager().GetInstalledVisas( Ivi.Visa.ConflictManager.ApiType.DotNet ) )
        {
            string line;
            try
            {
                string fullName = visaLibrary.Location.Substring( visaLibrary.Location.IndexOf( ',' ) + 1 );
                string fileName = fullName.Substring( 0, fullName.IndexOf( ',' ) ).Trim();

                // because we have a session, the implementation must be loaded now.
                (loadedAssembly, line) = Ivi.VisaNet.GacLoader.TryFindLoadedAssemblyByFriendlyName( fileName );
                if ( loadedAssembly is null )
                    _ = failureBuilder.AppendLine( line );
                else
                {
                    line = $"Loaded {loadedAssembly.GetName().FullName}.";
                    _ = sb.AppendLine( line );
                    line = $"\t{loadedAssembly.Location}.";
                    _ = sb.AppendLine( line );
                    line = $"\tVersion {System.Diagnostics.FileVersionInfo.GetVersionInfo( loadedAssembly.Location ).ProductVersion}.";
                    _ = sb.AppendLine( line );
                }
            }
            catch ( Exception exception )
            {
                line = $"Failed building session identity report for {visaLibrary.FriendlyName}:\n\t*** {GacLoader.BuildErrorMessage( exception )}";
                _ = failureBuilder.AppendLine( line );
            }
        }
        details = loadedAssembly is null ? failureBuilder.ToString() : sb.ToString();
        return loadedAssembly;
    }
}
