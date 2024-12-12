namespace cc.isr.VI.Foundation;

/// <summary> A visa version validator. </summary>
public sealed class VisaVersionValidator
{
    #region " construction "

    /// <summary> Initializes a new instance of the <see cref="object" /> class. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    private VisaVersionValidator() : base()
    {
    }

    #endregion

    /// <summary>
    /// Validates the implementation and specification visa versions against settings values.
    /// </summary>
    /// <remarks> David, 2020-04-12. </remarks>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public static (bool Success, string Details) ValidateFunctionalVisaVersions()
    {
        return VisaVersionValidator.ImplementationVersion < new Version( cc.isr.Visa.Gac.Vendor.IVI_VISA_IMPLEMENTATION_VERSION )
            ? (false, $"IVI VISA implementation version {VisaVersionValidator.ImplementationVersion} is lower than expected {cc.isr.Visa.Gac.Vendor.IVI_VISA_IMPLEMENTATION_VERSION} version")
            : VisaVersionValidator.SpecificationVersion < new Version( cc.isr.Visa.Gac.Vendor.IVI_VISA_SPECIFICATION_VERSION )
            ? (false, $"IVI VISA Specification version {VisaVersionValidator.SpecificationVersion} is lower than expected {cc.isr.Visa.Gac.Vendor.IVI_VISA_SPECIFICATION_VERSION} version")
            : (true, string.Empty);
    }

    /// <summary> Gets the version of this VISA.NET implementation. </summary>
    /// <value> The implementation version. </value>
    public static Version ImplementationVersion => Ivi.Visa.GlobalResourceManager.ImplementationVersion;

    /// <summary> Gets the specification version. </summary>
    /// <value> The specification version. </value>
    public static Version SpecificationVersion => Ivi.Visa.GlobalResourceManager.SpecificationVersion;

}
