// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;

namespace cc.isr.VI.Pith;

/// <summary> Base class manager of VISA resource names. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-21 </para>
/// </remarks>
public sealed class ResourceNamesManager
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    private ResourceNamesManager() : base()
    {
    }

    #endregion

    #region " search patterns "

    /// <summary> Name of the GPIB resource base. </summary>
    public const string GpibResourceBaseName = "GPIB";

    /// <summary> Name of the GPIB vxi resource base. </summary>
    public const string GpibVxiResourceBaseName = "GpibVxi";

    /// <summary> Name of the serial resource base. </summary>
    public const string SerialResourceBaseName = "ASRL";

    /// <summary> Name of the TCP IP resource base. </summary>
    public const string TcpIPResourceBaseName = "TCPIP";

    /// <summary> Name of the USB resource base. </summary>
    public const string UsbResourceBaseName = "USB";

    /// <summary> Name of the pxi resource base. </summary>
    public const string PxiResourceBaseName = "Pxi";

    /// <summary> Name of the vxi resource base. </summary>
    public const string VxiResourceBaseName = "Vxi";

    /// <summary> Interface base name. </summary>
    public const string InterfaceBaseName = "INTFC";

    /// <summary> Name of the instrument base. </summary>
    public const string InstrumentBaseName = "INSTR";

    /// <summary> Name of the backplane base. </summary>
    public const string BackplaneBaseName = "BACKPLANE";

    /// <summary> Name of the raw base. </summary>
    public const string RawBaseName = "RAW";

    /// <summary> A pattern specifying all resources search. </summary>
    public const string AllResourcesFilter = "?*";

    /// <summary> A pattern specifying the Gpib, USB or TCPIP search. </summary>
    public const string GpibUsbTcpIPFilter = "(GPIB|USB|TCPIP)?*";

    /// <summary> A pattern specifying the Gpib, USB or TCPIP instrument search. </summary>
    public const string GpibUsbTcpIPInstrumentFilter = GpibUsbTcpIPFilter + InstrumentBaseName;

    /// <summary> The backplane filter format. </summary>
    public const string BackplaneFilterFormat = "{0}?*" + BackplaneBaseName;

    /// <summary> The raw filter format. </summary>
    public const string RawFilterFormat = "{0}?*" + RawBaseName;

    /// <summary> Interface filter format. </summary>
    public const string InterfaceFilterFormat = "{0}?*" + InterfaceBaseName;

    /// <summary> The instrument filter format. </summary>
    public const string InstrumentFilterFormat = "{0}?*" + InstrumentBaseName;

    /// <summary> The instrument board filter format. </summary>
    public const string InstrumentBoardFilterFormat = "{0}{1}?*" + InstrumentBaseName;

    /// <summary> Interface resource format. </summary>
    public const string InterfaceResourceFormat = "{0}{1}::" + InterfaceBaseName;

    /// <summary> Parse address. </summary>
    /// <param name="resourceName"> The name of the resource. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string ParseAddress( string resourceName )
    {
        string address = string.Empty;
        if ( !string.IsNullOrWhiteSpace( resourceName ) )
        {
            int addressLocation = resourceName.IndexOf( "::", StringComparison.OrdinalIgnoreCase ) + 2;
            int addressWidth = resourceName.LastIndexOf( "::", StringComparison.OrdinalIgnoreCase ) - addressLocation;
            address = resourceName.Substring( addressLocation, addressWidth );
        }

        return address;
    }

    /// <summary> Parse GPIB address. </summary>
    /// <param name="resourceName"> The name of the resource. </param>
    /// <returns> An Integer. </returns>
    public static int ParseGpibAddress( string resourceName )
    {
        string address = ParseAddress( resourceName );
        if ( int.TryParse( address, out int value ) )
        {
        }

        return value;
    }

    /// <summary> Parse interface number. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> An Integer. </returns>
    public static int ParseInterfaceNumber( string resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) ) throw new ArgumentNullException( nameof( resourceName ) );
        HardwareInterfaceType interfaceType = ParseHardwareInterfaceType( resourceName );
        string baseName = InterfaceResourceBaseName( interfaceType );
        string[] parts = resourceName.Split( ':' );
        if ( parts.Count() <= 0 || parts[0].Length <= baseName.Length || !int.TryParse( parts[0][baseName.Length..], out int result ) )
        {
            result = 0;
        }

        return result;
    }

    /// <summary> Parse resource type. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> A HardwareInterfaceType. </returns>
    public static ResourceType ParseResourceType( string resourceName )
    {
        ResourceType result = ResourceType.None;
        if ( !string.IsNullOrWhiteSpace( resourceName ) )
        {
            foreach ( ResourceType v in Enum.GetValues( typeof( ResourceType ) ) )
            {
                if ( v != ResourceType.None && resourceName.EndsWith( ResourceTypeBaseName( v ), StringComparison.OrdinalIgnoreCase ) )
                {
                    result = v;
                    break;
                }
            }
        }

        return result;
    }

    /// <summary> Resource type base name. </summary>
    /// <param name="resourceType"> Type of the resource. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string ResourceTypeBaseName( ResourceType resourceType )
    {
        string result = string.Empty;
        switch ( resourceType )
        {
            case ResourceType.None:
                {
                    break;
                }

            case ResourceType.Instrument:
                {
                    result = InstrumentBaseName;
                    break;
                }

            case ResourceType.Interface:
                {
                    result = InterfaceBaseName;
                    break;
                }

            case ResourceType.Backplane:
                {
                    result = BackplaneBaseName;
                    break;
                }

            default:
                break;
        }

        return result;
    }

    /// <summary> Parse hardware interface resource base name. </summary>
    /// <remarks> David, 2020-06-06. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string ParseInterfaceResourceBaseName( string resourceName )
    {
        string result = HardwareInterfaceType.Custom.ToString();
        if ( !string.IsNullOrWhiteSpace( resourceName ) )
        {
            foreach ( HardwareInterfaceType v in Enum.GetValues( typeof( HardwareInterfaceType ) ) )
            {
                if ( v != HardwareInterfaceType.Custom && resourceName.StartsWith( InterfaceResourceBaseName( v ), StringComparison.OrdinalIgnoreCase ) )
                {
                    result = InterfaceResourceBaseName( v );
                    break;
                }
            }
        }

        return result;
    }

    /// <summary> Parse hardware interface type. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> A HardwareInterfaceType. </returns>
    public static HardwareInterfaceType ParseHardwareInterfaceType( string resourceName )
    {
        HardwareInterfaceType result = HardwareInterfaceType.Custom;
        if ( !string.IsNullOrWhiteSpace( resourceName ) )
        {
            foreach ( HardwareInterfaceType v in Enum.GetValues( typeof( HardwareInterfaceType ) ) )
            {
                if ( v != HardwareInterfaceType.Custom && resourceName.StartsWith( InterfaceResourceBaseName( v ), StringComparison.OrdinalIgnoreCase ) )
                {
                    result = v;
                    break;
                }
            }
        }

        return result;
    }

    /// <summary> Returns the interface resource base name. </summary>
    /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    /// illegal values. </exception>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The interface base name. </returns>
    public static string InterfaceResourceBaseName( HardwareInterfaceType interfaceType )
    {
        switch ( interfaceType )
        {
            case HardwareInterfaceType.Gpib:
                {
                    return GpibResourceBaseName;
                }

            case HardwareInterfaceType.GpibVxi:
                {
                    return GpibVxiResourceBaseName;
                }

            case HardwareInterfaceType.Pxi:
                {
                    return PxiResourceBaseName;
                }

            case HardwareInterfaceType.Serial:
                {
                    return SerialResourceBaseName;
                }

            case HardwareInterfaceType.TcpIp:
                {
                    return TcpIPResourceBaseName;
                }

            case HardwareInterfaceType.Usb:
                {
                    return UsbResourceBaseName;
                }

            case HardwareInterfaceType.Vxi:
                {
                    return VxiResourceBaseName;
                }

            case HardwareInterfaceType.Custom:
                throw new ArgumentException( $"{interfaceType} interface has not been defined.", nameof( interfaceType ) );

            default:
                {
                    throw new ArgumentException( $"Unhandled case {interfaceType}", nameof( interfaceType ) );
                }
        }
    }

    /// <summary> Returns the Interface resource name. </summary>
    /// <param name="interfaceName"> The name of the interface. </param>
    /// <param name="boardNumber">   The board number. </param>
    /// <returns> The Interface resource name, e.g., 'GPIB?*INTFC'. </returns>
    public static string BuildInterfaceResourceName( string interfaceName, int boardNumber )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, InterfaceResourceFormat, interfaceName, boardNumber );
    }

    /// <summary> Returns the Interface resource name. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <param name="boardNumber">   The board number. </param>
    /// <returns> The Interface resource name, e.g., 'GPIB?*INTFC'. </returns>
    public static string BuildInterfaceResourceName( HardwareInterfaceType interfaceType, int boardNumber )
    {
        return BuildInterfaceResourceName( InterfaceResourceBaseName( interfaceType ), boardNumber );
    }

    /// <summary> Returns the Interface search pattern. </summary>
    /// <returns> The Interface search pattern '?*INTFC'. </returns>
    public static string BuildInterfaceFilter()
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, InterfaceFilterFormat, "" );
    }

    /// <summary> Returns the Interface search pattern. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The Interface search pattern, e.g., 'GPIB?*INTFC'. </returns>
    public static string BuildInterfaceFilter( HardwareInterfaceType interfaceType )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, InterfaceFilterFormat, InterfaceResourceBaseName( interfaceType ) );
    }

    /// <summary> Returns the Instrument search pattern. </summary>
    /// <returns> The Instrument search pattern, e.g., '?*INSTR'. </returns>
    public static string BuildInstrumentFilter()
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, InstrumentFilterFormat, "" );
    }

    /// <summary> Returns the Instrument search pattern. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The Instrument search pattern, e.g., 'GPIB?*INSTR'. </returns>
    public static string BuildInstrumentFilter( HardwareInterfaceType interfaceType )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, InstrumentFilterFormat, InterfaceResourceBaseName( interfaceType ) );
    }

    /// <summary> Returns the Instrument search pattern. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    /// <param name="interface1">       Type of the interface. </param>
    /// <param name="interface2">       The second interface. </param>
    /// <param name="usingLikePattern"> True to use the Like pattern, which uses square brackets for
    /// or alternative values. </param>
    /// <returns> The Instrument search pattern, e.g., '(GPIB|USB)?*INSTR'. </returns>
    public static string BuildInstrumentFilter( HardwareInterfaceType interface1, HardwareInterfaceType interface2, bool usingLikePattern )
    {
        return usingLikePattern
            ? $"[{InterfaceResourceBaseName( interface1 )}|{InterfaceResourceBaseName( interface2 )}]?*{InstrumentBaseName}"
            : $"({InterfaceResourceBaseName( interface1 )}|{InterfaceResourceBaseName( interface2 )})?*{InstrumentBaseName}";
    }

    /// <summary> Returns the Instrument search pattern. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    /// <param name="interface1">       Type of the interface. </param>
    /// <param name="interface2">       The second interface. </param>
    /// <param name="interface3">       The third interface. </param>
    /// <param name="usingLikePattern"> True to use the Like pattern, which uses square brackets for
    /// or alternative values. </param>
    /// <returns> The Instrument search pattern, e.g., '(GPIB|USB)?*INSTR' if not using like pattern
    /// or '[GPIB|USB]?*INSTR' if using like pattern. </returns>
    public static string BuildInstrumentFilter( HardwareInterfaceType interface1, HardwareInterfaceType interface2, HardwareInterfaceType interface3, bool usingLikePattern )
    {
        return usingLikePattern
            ? $"[{InterfaceResourceBaseName( interface1 )}|{InterfaceResourceBaseName( interface2 )}|{InterfaceResourceBaseName( interface3 )}]?*{InstrumentBaseName}"
            : $"({InterfaceResourceBaseName( interface1 )}|{InterfaceResourceBaseName( interface2 )}|{InterfaceResourceBaseName( interface3 )})?*{InstrumentBaseName}";
    }

    /// <summary> Returns the Instrument search pattern. </summary>
    /// <param name="interfaceType">   Type of the interface. </param>
    /// <param name="interfaceNumber"> The interface number (e.g., board or port number). </param>
    /// <returns> The Instrument search pattern, e.g., 'GPIB0?*INSTR'. </returns>
    public static string BuildInstrumentFilter( HardwareInterfaceType interfaceType, int interfaceNumber )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, InstrumentBoardFilterFormat, InterfaceResourceBaseName( interfaceType ), interfaceNumber );
    }

    /// <summary>   Translate pattern. </summary>
    /// <remarks>   David, 2021-11-11. </remarks>
    /// <param name="pattern">          Specifies the pattern. </param>
    /// <param name="usingLikePattern"> True to use the Like pattern, which uses square brackets for
    ///                                 or alternative values. </param>
    /// <returns>   A <see cref="string" />. </returns>
    public static string TranslatePattern( string pattern, bool usingLikePattern )
    {
        return usingLikePattern
            ? pattern.Replace( '(', '[' ).Replace( ')', ']' )
            : pattern.Replace( '[', '(' ).Replace( ']', ')' );
    }

    #endregion

    #region " resource names "

    /// <summary> The GPIB instrument resource format. </summary>
    private const string Gpib_Instrument_Resource_Format = "GPIB{0}::{1}::INSTR";

    /// <summary> Builds GPIB instrument resource. </summary>
    /// <param name="boardNumber"> The board number. </param>
    /// <param name="address">     The address. </param>
    /// <returns> The resource name. </returns>
    public static string BuildGpibInstrumentResource( int boardNumber, int address )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, Gpib_Instrument_Resource_Format, boardNumber, address );
    }

    /// <summary> Builds GPIB instrument resource. </summary>
    /// <param name="boardNumber"> The board number. </param>
    /// <param name="address">     The address. </param>
    /// <returns> The resource name. </returns>
    public static string BuildGpibInstrumentResource( string boardNumber, string address )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, Gpib_Instrument_Resource_Format, boardNumber, address );
    }

    /// <summary> The USB instrument resource format. </summary>
    private const string Usb_Instrument_Resource_Format = "USB{0}::0x{1:X}::0x{2:X}::{3}::INSTR";

    /// <summary> Builds GPIB instrument resource. </summary>
    /// <param name="boardNumber">    The board number. </param>
    /// <param name="manufacturerId"> Identifier for the manufacturer. </param>
    /// <param name="modelNumber">    The model number. </param>
    /// <param name="serialNumber">   The serial number. </param>
    /// <returns> The resource name. </returns>
    public static string BuildUsbInstrumentResource( int boardNumber, int manufacturerId, int modelNumber, int serialNumber )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, Usb_Instrument_Resource_Format, boardNumber, manufacturerId, modelNumber, serialNumber );
    }

    #endregion

    #region " ping "

    /// <summary> Enumerates resources that can be pinged or non-TCP/IP resources. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resources"> The resources. </param>
    /// <returns>
    /// An enumerator that allows for each to be used to process ping filter in this collection.
    /// </returns>
    public static IEnumerable<string> PingFilter( IEnumerable<string> resources )
    {
        if ( resources is null ) throw new ArgumentNullException( nameof( resources ) );
        List<string> l = [];
        foreach ( string resource in resources )
        {
            if ( !IsTcpIpResource( resource ) || PingTcpIpResource( resource ) )
            {
                l.Add( resource );
            }
        }

        return l;
    }

    /// <summary> Query if 'resourceName' is TCP IP resource. </summary>
    /// <param name="resourceName"> The name of the resource. </param>
    /// <returns> <c>true</c> if TCP IP resource; otherwise <c>false</c> </returns>
    public static bool IsTcpIpResource( string resourceName )
    {
        return !string.IsNullOrWhiteSpace( resourceName )
            && resourceName.StartsWith( HardwareInterfaceType.TcpIp.ToString(), StringComparison.OrdinalIgnoreCase );
    }

    /// <summary> Parses the IP address from the VISA Resource Name. </summary>
    /// <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="resourceName"> The name of the resource. </param>
    /// <returns> The resource TCP/IP address. </returns>
    public static string ToIPAddress( string resourceName )
    {
        return string.IsNullOrWhiteSpace( resourceName )
            ? throw new ArgumentNullException( nameof( resourceName ) )
            : !IsTcpIpResource( resourceName )
                ? throw new InvalidOperationException( $"Unable to extract the {HardwareInterfaceType.TcpIp} address from the Visa Resource Name {resourceName}" )
                : resourceName.Split( ':' )[2];
    }

    /// <summary>   Pings a TcpIP VISA resource. </summary>
    /// <remarks>   2024-10-19. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="resourceName">         The name of the VISA resource. </param>
    /// <param name="timeoutMilliseconds">  (Optional) (1000) The timeout in milliseconds. </param>
    /// <param name="pingHops">             (Optional) (2) The number of routing nodes that
    ///                                     can forward the Ping data before it is discarded. </param>
    /// <param name="doNotFragment">        (Optional) (true) a Boolean value that controls
    ///                                     fragmentation of the data sent to the remote host. </param>
    /// <returns>   <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public static bool PingTcpIpResource( string resourceName, int timeoutMilliseconds = 1000, int pingHops = 2, bool doNotFragment = true )
    {
        return !ResourceNamesManager.IsTcpIpResource( resourceName )
            || ResourceNamesManager.FastPing( ResourceNamesManager.ToIPAddress( resourceName ), timeoutMilliseconds, pingHops, doNotFragment );
    }

    /// <summary>   Attempts to ping a TCP/IP VISA resource. </summary>
    /// <remarks>   2025-11-10. </remarks>
    /// <param name="resourceName">         The name of the resource. </param>
    /// <param name="details">              [out] The details. </param>
    /// <param name="timeoutMilliseconds">  (Optional) (1000) The timeout in milliseconds. </param>
    /// <param name="pingHops">             (Optional) (2) The number of routing nodes that can
    ///                                     forward the Ping data before it is discarded. </param>
    /// <param name="doNotFragment">        (Optional) (true) a Boolean value that controls
    ///                                     fragmentation of the data sent to the remote host. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryPingTcpIpResource( string resourceName, out string details, int timeoutMilliseconds = 1000, int pingHops = 2, bool doNotFragment = true )
    {
        if ( resourceName == null )
        {
            details = $"{nameof( resourceName )} argument is null.";
            return false;
        }
        else if ( string.IsNullOrWhiteSpace( resourceName ) )
        {
            details = $"{nameof( resourceName )} argument is empty.";
            return false;
        }
        else if ( ResourceNamesManager.IsTcpIpResource( resourceName ) )
        {
            details = $"{resourceName} is not a TCP/IP resource.";
            return true;
        }
        else if ( !ResourceNamesManager.FastPing( ResourceNamesManager.ToIPAddress( resourceName ), timeoutMilliseconds, pingHops, doNotFragment ) )
        {
            details = $"Ping to {ResourceNamesManager.ToIPAddress( resourceName )} with {pingHops} hops timed out after {timeoutMilliseconds} ms.";
            return false;
        }
        else
        {
            details = string.Empty;
            return true;
        }
    }

    /// <summary>   Gets or sets the ping timeout. </summary>
    /// <value> The ping timeout. </value>
    public static TimeSpan PingTimeout { get; set; } = TimeSpan.FromMilliseconds( 1000 );

    /// <summary>   Gets or sets the ping hops. </summary>
    /// <value> The ping hops. </value>
    public static int PingHops { get; set; } = 1;

    /// <summary>   Fast ping. </summary>
    /// <remarks>   2024-10-19. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="ipAddress">            The IP destination for the ICMP echo message. </param>
    /// <param name="timeoutMilliseconds">  (Optional) (1000) The timeout in milliseconds. </param>
    /// <param name="pingHops">             (Optional) (2) The number of routing nodes that
    ///                                     can forward the Ping data before it is discarded. </param>
    /// <param name="doNotFragment">        (Optional) (true) a Boolean value that controls
    ///                                     fragmentation of the data sent to the remote host. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool FastPing( string ipAddress, int timeoutMilliseconds = 1000, int pingHops = 2, bool doNotFragment = true )
    {
        if ( string.IsNullOrWhiteSpace( ipAddress ) ) throw new ArgumentNullException( nameof( ipAddress ) );
        System.Net.NetworkInformation.Ping ping = new();
        System.Net.NetworkInformation.PingOptions pingOptions = new( pingHops, doNotFragment );
        byte[] buffer = [0, 0];
        return ping.Send( ipAddress, timeoutMilliseconds, buffer, pingOptions ).Status == System.Net.NetworkInformation.IPStatus.Success;
    }

    /// <summary>   Fast ping. </summary>
    /// <remarks>   David, 2020-08-07. </remarks>
    /// <param name="ipAddress">    The IP destination for the ICMP echo message. </param>
    /// <param name="timeout">      The timeout. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool FastPing( string ipAddress, TimeSpan timeout )
    {
        return ResourceNamesManager.FastPing( ipAddress, ( int ) timeout.TotalMilliseconds );
    }

    #endregion
}

/// <summary> Values that represent resource types. </summary>
public enum ResourceType
{
    /// <summary> An enum constant representing the none option. </summary>
    [Description( "Not specified" )]
    None = 0,

    /// <summary> An enum constant representing the instrument option. </summary>
    [Description( "Instrument" )]
    Instrument,

    /// <summary> An enum constant representing the interface] option. </summary>
    [Description( "Interface" )]
    Interface,

    /// <summary> An enum constant representing the backplane option. </summary>
    [Description( "Backplane" )]
    Backplane
}

/// <summary> Values that represent hardware interface types. </summary>
public enum HardwareInterfaceType
{
    /// <summary> An enum constant representing the custom option. </summary>
    [Description( "Custom" )]
    Custom = 0,

    /// <summary> An enum constant representing the GPIB interface. </summary>
    [Description( "GPIB" )]
    Gpib = 1,

    /// <summary> An enum constant representing the vxi option. </summary>
    [Description( "VXI" )]
    Vxi = 2,

    /// <summary> An enum constant representing the GPIB vxi option. </summary>
    [Description( "GPIB VXI" )]
    GpibVxi = 3,

    /// <summary> An enum constant representing the serial option. </summary>
    [Description( "Serial" )]
    Serial = 4,

    /// <summary> An enum constant representing the pxi option. </summary>
    [Description( "PXI" )]
    Pxi = 5,

    /// <summary> An enum constant representing the TCP/IP option. </summary>
    [Description( "TCPIP" )]
    TcpIp = 6,

    /// <summary> An enum constant representing the USB option. </summary>
    [Description( "USB" )]
    Usb = 7
}
