namespace Ivi.VisaNet;

/// <summary>   Information about the visa assembly. </summary>
/// <remarks>   2026-01-13. </remarks>
public class VisaAssemblyInfo
{
    /// <summary>   Gets or sets the friend name of the assembly. </summary>
    /// <value> The friendly name of the assembly. </value>
    public string FriendlyName { get; set; } = string.Empty;

    /// <summary>   Gets or sets the full pathname of the assembly folder. </summary>
    /// <value> The full pathname of the assembly folder. </value>
    public string FolderPath { get; set; } = string.Empty;

    /// <summary>   Gets or sets the filename of the file. </summary>
    /// <value> The name of the file. </value>
    public string FileName { get; set; } = string.Empty;

    /// <summary>   Gets or sets the name of the full. </summary>
    /// <value> The name of the full. </value>
    public string FullName { get; set; } = string.Empty;

    /// <summary>   Gets or sets the name of the resource manager type. </summary>
    /// <value> The name of the resource manager type. </value>
    public string ResourceManagerTypeName { get; set; } = string.Empty;

    /// <summary>   Gets or sets the implementation version. </summary>
    /// <remarks>   This is the version of the assembly as reported by the Resource Manager
    ///             <see cref="Ivi.Visa.GlobalResourceManager.ImplementationVersion"/>. </remarks>
    /// <value> The implementation version. </value>
    public string ImplementationVersion { get; set; } = string.Empty;

    /// <summary>   Gets or sets the specification version. </summary>
    /// <remarks>   This is the version of the VISA specification as reported by the Resource Manager.
    ///             <see cref="Ivi.Visa.GlobalResourceManager.SpecificationVersion"/>. </remarks>
    /// <value> The specification version. </value>
    public string SpecificationVersion { get; set; } = string.Empty;

    /// <summary>   Gets or sets the file version. </summary>
    /// <value> The file version. </value>
    public string FileVersion { get; set; } = string.Empty;

    /// <summary>   Gets or sets a value indicating whether this object is a vendor implementation. </summary>
    /// <value> True if this object is a vendor implementation; otherwise false. </value>
    public bool IsVendorImplementation { get; set; }

    /// <summary>   Has Keysight visa implementation. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public (bool Success, string Details) HasVisaImplementation()
    {
        (Ivi.Visa.ConflictManager.VisaImplementation? implementation, string details) = GacLoader.TryFindImplementation( this.FriendlyName );
        if ( implementation is null )
            return (false, details);
        else if ( !implementation.Enabled )
            return (false, $"The {this.FriendlyName} was found but is not enabled.");
        else
            return (true, string.Empty);
    }

    /// <summary>   Checks if the expected Keysight Visa assembly exists in the GAC. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public (bool Success, string Details) VisaAssemblyExists()
    {
        FileInfo? fi = new( System.IO.Path.Combine( this.FolderPath, this.FileName ) );
        return fi.Exists
            ? (true, $"The {this.FriendlyName} assembly was found at: {fi.FullName}")
            : (false, $"The {this.FriendlyName} assembly was not found at: {fi.FullName}");
    }

    /// <summary>   Queries the expected Keysight resource manager exists. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   A Tuple. </returns>
    public (bool Success, string Details) ResourceManagerExists()
    {
        return GacLoader.IsResourceMangerExists( this.ResourceManagerTypeName );
    }

    /// <summary>   Query if the implementation is loaded. </summary>
    /// <remarks>   2024-07-13. </remarks>
    /// <returns>   True if the implementation is loaded; otherwise false. </returns>
    public bool IsLoaded()
    {
        return GacLoader.HasDotNetImplementations() && GacLoader.IsAssemblyLoaded( this.FullName );
    }

}
