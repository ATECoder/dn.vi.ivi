namespace Ivi.VisaNet;

/// <summary>   Implementation Vendor. </summary>
/// <remarks>   David, 2022-02-25. </remarks>
public static partial class GacLoader
{
    /// <summary>   Gets information describing the ivi visa assembly. </summary>
    /// <value> Information describing the ivi visa assembly. </value>
    public static VisaAssemblyInfo IviVisaAssemblyInfo => new()
    {
        FriendlyName = "Ivi.Visa",
        FolderPath = @".\",
        FileName = "Ivi.Visa.dll",
        FullName = "Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1",
        ResourceManagerTypeName = string.Empty,
        ImplementationVersion = "8.0.0.0",
        SpecificationVersion = "7.4.0.0",
        FileVersion = "8.0.2.0",
        IsVendorImplementation = false
    };

    /// <summary>   Gets information describing the Keysight visa assembly. </summary>
    /// <value> Information describing the Keysight visa assembly. </value>
    public static VisaAssemblyInfo KeysightVisaAssemblyInfo => new()
    {
        FriendlyName = "Keysight.Visa",
        FolderPath = @"C:\Program Files\IVI Foundation\VISA\Microsoft.NET\VendorAssemblies\kt\8.0\Keysight.Visa\Keysight.Visa.dll",
        FileName = "Keysight.Visa.dll",
        FullName = "Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73",
        ResourceManagerTypeName = "Keysight.Visa.ResourceManager, Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73",
        ImplementationVersion = "18.6.0.0",
        SpecificationVersion = "7.4.0.0",
        FileVersion = "18.6.5.0",
        IsVendorImplementation = true
    };

    /// <summary>   Gets information describing the National Instruments (NI) visa assembly. </summary>
    /// <value> Information describing the National Instruments (NI) visa assembly. </value>
    public static VisaAssemblyInfo NIVisaAssemblyInfo => new()
    {
        FriendlyName = "NationalInstruments.Visa",
        FolderPath = @"C:\Program Files\IVI Foundation\VISA\Microsoft.NET\VendorAssemblies\kt\25.0\NI.Visa\NI.Visa.dll",
        FileName = "NationalInstruments.Visa.dll",
        FullName = "NationalInstruments.Visa, Version=25.0.0.0, Culture=neutral, PublicKeyToken=2eaa5af0834e221d",
        ResourceManagerTypeName = "NationalInstruments.Visa.ResourceManager, NationalInstruments.Visa, Version=21.0.0.49304, Culture=neutral, PublicKeyToken=2eaa5af0834e221d",
        ImplementationVersion = "25.5.0.0",
        SpecificationVersion = "7.4.0.0",
        FileVersion = "25.5.0.0",
        IsVendorImplementation = true
    };

}

