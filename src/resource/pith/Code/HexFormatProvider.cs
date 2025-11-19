// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Std.FormatProviders;

namespace cc.isr.VI.Pith;

/// <summary> An Hex format provider. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-11-21 </para>
/// </remarks>
public class HexFormatProvider : DigitalValueFormatProvider
{
    /// <summary> Converts a value to a service requests enum value. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> Value as a ServiceRequests? </returns>
    public ServiceRequests? ToServiceRequests( string value )
    {
        return ParseEnumValue<ServiceRequests>( this.Prefix, value, System.Globalization.NumberStyles.HexNumber );
    }

    /// <summary> Converts a value to a standard events enum value. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> Value as a StandardEvents? </returns>
    public StandardEvents? ToStandardEvents( string value )
    {
        return ParseEnumValue<StandardEvents>( this.Prefix, value, System.Globalization.NumberStyles.HexNumber );
    }

    /// <summary> Hexadecimal value format provider. </summary>
    /// <param name="byteCount"> Number of bytes. </param>
    /// <returns> A RegisterValueFormatProvider. </returns>
    public static HexFormatProvider FormatProvider( int byteCount )
    {
        return FormatProvider( DefaultHexPrefix, byteCount );
    }

    /// <summary> Hexadecimal value format provider. </summary>
    /// <param name="prefix">    The prefix. </param>
    /// <param name="byteCount"> Number of bytes. </param>
    /// <returns> A RegisterValueFormatProvider. </returns>
    public static HexFormatProvider FormatProvider( string prefix, int byteCount )
    {
        HexFormatProvider result = new()
        {
            Prefix = prefix,
            FormatString = $"{prefix}{{0:X{byteCount}}}",
            NullValueCaption = $"{prefix}{new string( '.', byteCount )}"
        };
        return result;
    }
}
