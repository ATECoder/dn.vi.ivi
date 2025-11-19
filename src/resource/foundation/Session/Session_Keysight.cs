namespace cc.isr.VI.Foundation;

public partial class Session
{
    /// <summary> Lists the attributes not supported for this vendor file. </summary>
    /// <value> The vendor unsupported attributes. </value>
    private static System.Collections.ObjectModel.Collection<Ivi.Visa.NativeVisaAttribute> VendorUnsupportedAttributes
    {
        get
        {
            field ??= new System.Collections.ObjectModel.Collection<Ivi.Visa.NativeVisaAttribute>() { { Ivi.Visa.NativeVisaAttribute.TcpKeepAlive } };

            return field!;
        }
    }
}
