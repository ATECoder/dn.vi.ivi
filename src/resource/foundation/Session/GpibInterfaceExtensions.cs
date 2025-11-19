// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Foundation;

/// <summary> Extensions for GPIB Interface. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2005-01-21, 1.0.1847.x. </para></remarks>
internal static class GpibInterfaceExtensions
{
    #region " GPIB interface "

    /// <summary> Returns all instruments to some default state. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="value"> The value. </param>
    public static void ClearDevices( this Ivi.Visa.IGpibInterfaceSession value )
    {
        if ( value is null )
        {
            throw new ArgumentNullException( nameof( value ) );
        }
        // Transmit the DCL command to the interface.
        _ = value.SendCommand( [.. Syntax.Ieee488Syntax.BuildDeviceClear()] );
    }

    /// <summary> Clears the specified device. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="value">       The value. </param>
    /// <param name="gpibAddress"> The instrument address. </param>
    public static void SelectiveDeviceClear( this Ivi.Visa.IGpibInterfaceSession value, int gpibAddress )
    {
        if ( value is null ) throw new ArgumentNullException( nameof( value ) );
        _ = value.SendCommand( [.. Syntax.Ieee488Syntax.BuildSelectiveDeviceClear( ( byte ) gpibAddress )] );
    }

    /// <summary> Clears the specified device. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="value">        The value. </param>
    /// <param name="resourceName"> Name of the resource. </param>
    public static void SelectiveDeviceClear( this Ivi.Visa.IGpibInterfaceSession value, string resourceName )
    {
        if ( value is null ) throw new ArgumentNullException( nameof( value ) );
        if ( string.IsNullOrWhiteSpace( resourceName ) ) throw new ArgumentNullException( nameof( resourceName ) );
        Pith.ResourceNameInfo resourceNameInfo = new( resourceName );
        if ( resourceNameInfo.GpibAddress > 0 )
            value.SelectiveDeviceClear( resourceNameInfo.GpibAddress );
    }

    /// <summary> Clears the interface. Resets interface if instruments are not connected. </summary>
    /// <param name="value"> The value. </param>
    public static void ClearInterface( this Ivi.Visa.IGpibInterfaceSession value )
    {
        value.SendInterfaceClear();
        value.ClearDevices();
    }

    #endregion
}
