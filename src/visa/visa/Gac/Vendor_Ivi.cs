namespace cc.isr.Visa.Gac;

/// <summary>   Implementation Vendor. </summary>
/// <remarks>   David, 2022-02-25. </remarks>
public static partial class Vendor
{

    /// <summary>   (Immutable) filename of the ivi visa file. </summary>
    public const string IVI_VISA_FILENAME = "Ivi.Visa.dll";

#if true
    /// <summary>
    /// (Immutable) The IVI visa implementation version supported by the <see href="https://www.nuget.org/packages/Kelary.Ivi.Visa"/>.
    /// This is the version of the ivi.visa.dll as reported by Ivi.Visa.GlobalResourceManager.ImplementationVersion"
    /// package.
    /// </summary>
    public const string IVI_VISA_IMPLEMENTATION_VERSION = "8.0.0.0";

    /// <summary>
    /// (Immutable) the ivi visa specification version, which is hard coded into the
    /// Ivi.Visa.GlobalResourceManager.SpecificationVersion".
    /// </summary>
    public const string IVI_VISA_SPECIFICATION_VERSION = "8.0.0.0";

    /// <summary>   (Immutable) name of the ivi visa full. </summary>
    public const string IVI_VISA_FULL_NAME = "Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1";
#elif false
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

    /// <summary>   (Immutable) name of the ivi visa full. </summary>
    public const string IVI_VISA_FULL_NAME = "Ivi.Visa, Version=5.11.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1";
#endif
}
