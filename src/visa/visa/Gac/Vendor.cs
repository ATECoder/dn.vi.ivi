using Ivi.Visa;
using Ivi.Visa.ConflictManager;

namespace cc.isr.Visa.Gac;

/// <summary>   Implementation Vendor. </summary>
/// <remarks>   David, 2022-02-25. </remarks>
public static class Vendor
{
    /// <summary>   Creates vendor resource manager. </summary>
    /// <remarks>   David, 2021-11-06. </remarks>
    /// <param name="typeName"> Name of the type. </param>
    /// <returns>   The new vendor resource manager. </returns>
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
            return Vendor.CreateVendorResourceManager( typeName ) is null
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
    public static IEnumerable<VisaImplementation> EnumerateDotNetImplementations()
    {
        ConflictManager conflictManager = new();
        return conflictManager.GetInstalledVisas( ApiType.DotNet );
    }

    /// <summary>   Query if this object has dot net implementations. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   True if dot net implementations, false if not. </returns>
    public static bool HasDotNetImplementations()
    {
        ConflictManager conflictManager = new();
        return conflictManager.GetInstalledVisas( ApiType.DotNet ).Count > 0;
    }

    /// <summary>   Try find implementation. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <param name="friendlyName"> Name of the friendly. </param>
    /// <returns>   A Tuple. </returns>
    public static (Ivi.Visa.ConflictManager.VisaImplementation? implementation, string details) TryFindImplementation( string friendlyName )
    {
        System.Collections.Generic.IEnumerable<Ivi.Visa.ConflictManager.VisaImplementation> installedVisas = cc.isr.Visa.Gac.Vendor.EnumerateDotNetImplementations();
        if ( installedVisas is null || installedVisas.Count() == 0 )
            return (null, "No VISA implementation where found.");
        else
        {
            foreach ( Ivi.Visa.ConflictManager.VisaImplementation implementation in installedVisas )
            {
                if ( string.Equals( friendlyName, implementation.FriendlyName, StringComparison.OrdinalIgnoreCase ) )
                    return (implementation, string.Empty);
            }
            return (null, $"No VIA implementation with the '{friendlyName}' was found among the existing {installedVisas.Count()} implementation(s).");
        }
    }

    /// <summary>   Checks if an assembly with the specified full name exists in the specified path. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <param name="path">             Full pathname of the file. </param>
    /// <param name="fileName">         Filename of the file. </param>
    /// <param name="expectedFullName"> The expected full name of the assembly. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) IsAssemblyExists( string path, string fileName, string expectedFullName )
    {
        if ( string.IsNullOrWhiteSpace( path ) )
            return (false, "The provided path is null or white space.");
        else
        {
            FileInfo? fi = new( System.IO.Path.Combine( path, fileName ) );
            if ( fi is not null && fi.Exists )
            {
                System.Reflection.Assembly? candidateAssembly = System.Reflection.Assembly.LoadFile( fi.FullName );
                Console.Out.WriteLine( candidateAssembly.FullName );
                return string.Equals( expectedFullName, candidateAssembly.FullName, StringComparison.Ordinal )
                    ? (true, string.Empty)
                    : (false, $"Mismatch between expected {expectedFullName} and actual {candidateAssembly.FullName} assembly full name.");
            }
            else
                return (false, $"Expected assembly {path} was not found.");

        }
    }

    /// <summary>   Checks if an assembly with the specified full name exists in the current execution path. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <param name="fileName">         Filename of the file. </param>
    /// <param name="expectedFullName"> The expected full name of the assembly. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) IsAssemblyExists( string fileName, string expectedFullName )
    {
        string? path;
        System.Reflection.Assembly? executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        if ( executingAssembly is null )
        {
            return (false, "Failed getting the executing assembly.");
        }
        else
        {
            FileInfo? fi = new( executingAssembly.Location );
            if ( fi is null )
                return (false, "Failed getting the executing assembly file information.");
            else
                path = fi.Directory!.FullName;
        }

        if ( string.IsNullOrWhiteSpace( path ) )
        {
            return (false, "Failed getting the executing assembly path.");
        }
        else
        {
            return Vendor.IsAssemblyExists( path, fileName, expectedFullName );
        }
    }

    /// <summary>
    /// (Immutable) The IVI visa implementation version supported by the <see href="https://www.nuget.org/packages/Kelary.Ivi.Visa"/>.
    /// This is the version of the ivi.visa.dll as reported by Ivi.Visa.GlobalResourceManager.ImplementationVersion"
    /// package.
    /// </summary>
    public const string IVI_VISA_IMPLEMENTATION_VERSION = "5.11.0.0";

    /// <summary>
    /// (Immutable) the ivi visa specification version, which is hard coded into the
    /// Ivi.Visa.GlobalResourceManager.SpecificationVersion".
    /// </summary>
    public const string IVI_VISA_SPECIFICATION_VERSION = "5.7.0.0";

    /// <summary>   (Immutable) filename of the ivi visa file. </summary>
    public const string IVI_VISA_FILENAME = "Ivi.Visa.dll";

    /// <summary>   (Immutable) name of the ivi visa full. </summary>
    public const string IVI_VISA_FULL_NAME = "Ivi.Visa, Version=5.11.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1";

    /// <summary>   Checks if an IVI Visa assembly with the specified full name exists in the current execution path. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) IsIviVisaAssemblyExists()
    {
        return Vendor.IsAssemblyExists( IVI_VISA_FILENAME, IVI_VISA_FULL_NAME );
    }

    /// <summary>   (Immutable) path of the Keysight visa file. </summary>
    public const string KEYSIGHT_VISA_PATH = @"C:\Windows\assembly\GAC_MSIL\Keysight.Visa\18.2.0.0__7a01cdb2a9131f73";

    /// <summary>   (Immutable) filename of the Keysight visa file. </summary>
    public const string KEYSIGHT_VISA_FILENAME = "Keysight.Visa.dll";

    /// <summary>   (Immutable) name of the keysight visa full. </summary>
    public const string KEYSIGHT_VISA_FRIENDLY_NAME = "Keysight.Visa";

    /// <summary>   (Immutable) full name of the Keysight visa. </summary>
    public const string KEYSIGHT_VISA_FULL_NAME = "Keysight.Visa, Version=18.2.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73";

    /// <summary>   (Immutable) full name of the Keysight visa resource manager. </summary>
    public const string KEYSIGHT_RESOURCE_MANAGER_TYPE_NAME = "Keysight.Visa.ResourceManager, Keysight.Visa, Version=18.2.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73";

    /// <summary>   Has keysight visa implementation. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) HasKeysightVisaImplementation()
    {
        (Ivi.Visa.ConflictManager.VisaImplementation? implementation, string details) = Vendor.TryFindImplementation( KEYSIGHT_VISA_FRIENDLY_NAME );
        if ( implementation is null )
            return (false, details);
        else if ( !implementation.Enabled )
            return (false, $"The {KEYSIGHT_VISA_FRIENDLY_NAME} was found but is not enabled.");
        else
            return (true, string.Empty);
    }

    /// <summary>   Checks if the expected Keysight Visa assembly exists in the GAC. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) IsKeysightVisaAssemblyExists()
    {
        return Vendor.IsAssemblyExists( KEYSIGHT_VISA_PATH, KEYSIGHT_VISA_FILENAME, KEYSIGHT_VISA_FULL_NAME );
    }

    /// <summary>   Queries the expected Keysight resource manager exists. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) IsKeysightResourceManagerExists()
    {
        return Vendor.IsResourceMangerExists( KEYSIGHT_RESOURCE_MANAGER_TYPE_NAME );
    }

    /// <summary>   Query if the Gac Loader loaded the keysight implementation. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   True if loaded keysight implementation, false if not. </returns>
    public static bool IsLoadedKeysightImplementation()
    {
        return GacLoader.HasDotNetImplementations && GacLoader.LoadedImplementation is not null
            && string.Equals( Vendor.KEYSIGHT_VISA_FRIENDLY_NAME, GacLoader.LoadedImplementation.FriendlyName, StringComparison.OrdinalIgnoreCase );
    }

    /// <summary>   (Immutable) path of the NI visa file. </summary>
    public const string NI_VISA_PATH = @"C:\Windows\assembly\GAC_MSIL\NationalInstruments.Visa\21.0.0.0__2eaa5af0834e221d";

    /// <summary>   (Immutable) filename of the NI visa file. </summary>
    public const string NI_VISA_FILENAME = "NationalInstruments.Visa.dll";

    /// <summary>   (Immutable) name of the keysight visa full. </summary>
    public const string NI_VISA_FRIENDLY_NAME = "NationalInstruments.Visa";

    /// <summary>   (Immutable) full name of the NI visa. </summary>
    public const string NI_VISA_FULL_NAME = "NationalInstruments.Visa, Version=21.0.0.0, Culture=neutral, PublicKeyToken=2eaa5af0834e221d";

    /// <summary>   (Immutable) full name of the Keysight visa resource manager. </summary>
    public const string NI_RESOURCE_MANAGER_TYPE_NAME = "NationalInstruments.Visa.ResourceManager, NationalInstruments.Visa, Version=21.0.0.49304, Culture=neutral, PublicKeyToken=2eaa5af0834e221d";

    /// <summary>   Has NI visa implementation. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) HasNIVisaImplementation()
    {
        (Ivi.Visa.ConflictManager.VisaImplementation? implementation, string details) = Vendor.TryFindImplementation( NI_VISA_FRIENDLY_NAME );
        if ( implementation is null )
            return (false, details);
        else if ( !implementation.Enabled )
            return (false, $"The {NI_VISA_FRIENDLY_NAME} was found but is not enabled.");
        else
            return (true, string.Empty);
    }

    /// <summary>   Checks if the expected National Instruments Visa assembly exists in the GAC. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) IsNIVisaAssemblyExists()
    {
        return Vendor.IsAssemblyExists( NI_VISA_PATH, NI_VISA_FILENAME, NI_VISA_FULL_NAME );
    }

    /// <summary>   Queries if the expected National Instruments resource manager exists. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public static (bool Success, string Details) IsNIResourceManagerExists()
    {
        return Vendor.IsResourceMangerExists( NI_RESOURCE_MANAGER_TYPE_NAME );
    }

    /// <summary>   Query if the Gac Loader loaded the national instruments implementation. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   True if loaded national instruments implementation, false if not. </returns>
    public static bool IsLoadedNImplementation()
    {
        return GacLoader.HasDotNetImplementations && GacLoader.LoadedImplementation is not null
            && string.Equals( Vendor.NI_VISA_FRIENDLY_NAME, GacLoader.LoadedImplementation.FriendlyName, StringComparison.OrdinalIgnoreCase );
    }

}

