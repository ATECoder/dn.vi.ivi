// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

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
