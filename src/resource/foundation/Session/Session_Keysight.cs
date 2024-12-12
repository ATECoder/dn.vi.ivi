namespace cc.isr.VI.Foundation;

public partial class Session
{
    /// <summary> The vendor unsupported attributes. </summary>
    private static System.Collections.ObjectModel.Collection<Ivi.Visa.NativeVisaAttribute>? _vendorUnsupportedAttributes;

    /// <summary> Lists the attributes not supported for this vendor file. </summary>
    /// <value> The vendor unsupported attributes. </value>
    private static System.Collections.ObjectModel.Collection<Ivi.Visa.NativeVisaAttribute> VendorUnsupportedAttributes
    {
        get
        {
            _vendorUnsupportedAttributes ??= new System.Collections.ObjectModel.Collection<Ivi.Visa.NativeVisaAttribute>() { { Ivi.Visa.NativeVisaAttribute.TcpKeepAlive } };

            return _vendorUnsupportedAttributes!;
        }
    }
}
