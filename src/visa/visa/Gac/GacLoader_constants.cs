namespace cc.isr.Visa.Gac;

public static partial class GacLoader
{
#if KeysightVisa
    /// <summary>   (Immutable) the visa package source. </summary>
    public static readonly string VisaPackageSource = "Keysight.Visa";
#elif IviVisa
    /// <summary>   (Immutable) the visa package source. </summary>
    public static readonly string VisaPackageSource = "IVI.Visa";
#elif KelaryVisa
    /// <summary>   (Immutable) the visa package source. </summary>
    public static readonly string VisaPackageSource = "Kelary.Visa";
#else
    /// <summary>   (Immutable) the visa package source. </summary>
    public static readonly string VisaPackageSource = "Unknown.Visa";
#endif
}
