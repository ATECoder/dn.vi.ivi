using System.Windows.Forms;
using cc.isr.WinControls.BindingExtensions;

namespace cc.isr.VI.WinControls;

/// <summary> A binding event handlers. </summary>
/// <remarks> (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para> </remarks>
public static class BindingEventHandlers
{
    /// <summary> The hexadecimal format provider. </summary>
    private static readonly Pith.HexFormatProvider _hexFormatProvider = Pith.HexFormatProvider.FormatProvider( 2 );

    /// <summary> The display x coordinate 2 event handler. </summary>
    // e.Value = If(e.Value Is Nothing, string.Empty, String . Format("{0:X2}", CInt(e.Value)))
    // e.Value = String . Format("0x{0:X2}", CInt(e.Value))
    internal static ConvertEventHandler DisplayX2EventHandler = ( sender, e ) => e.Value = _hexFormatProvider.Format( ( int ) e.Value! );

    /// <summary> The display register event handler. </summary>
    // this works: 
    // e.Value = Pith.RegisterValueFormatProvider.Format(Pith.RegisterValueFormatProvider.DefaultFormatString,
    // Pith.RegisterValueFormatProvider.DefaultNullValueCaption,
    // e.Value)
    // this works: e.Value = String .Format("0x{0:X2}", CInt(e.Value))
    // this works: e.DisplayEnumValue("0x{0:X2}")
    internal static ConvertEventHandler DisplayRegisterEventHandler = ( sender, e ) => e.Value = _hexFormatProvider.Format( e.Value! );

    /// <summary> The parse status register event handler. </summary>
    internal static ConvertEventHandler ParseStatusRegisterEventHandler = ( sender, e ) => e.Value = _hexFormatProvider.ToServiceRequests( e.Value!.ToString()! );

    /// <summary> The parse standard register event handler. </summary>
    internal static ConvertEventHandler ParseStandardRegisterEventHandler = ( sender, e ) => e.Value = _hexFormatProvider.ToStandardEvents( e.Value!.ToString()! );

    /// <summary> The invert display handler. </summary>
    internal static ConvertEventHandler InvertDisplayHandler = ( sender, e ) => e.WriteInverted();
}
