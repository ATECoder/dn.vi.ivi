// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Pith.My;

/// <summary> Values that represent project trace event identifiers. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum ProjectTraceEventId
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not specified" )]
    None,

    /// <summary> An enum constant representing the pith option. </summary>
    [System.ComponentModel.Description( "Pith" )]
    Pith = 0x100 * 0x10,

    /// <summary> An enum constant representing the national visa option. </summary>
    [System.ComponentModel.Description( "NI Visa" )]
    NationalVisa = Pith + 0x1,

    /// <summary> An enum constant representing the foundation visa option. </summary>
    [System.ComponentModel.Description( "IVI Visa" )]
    FoundationVisa = Pith + 0x2,

    /// <summary> An enum constant representing the keysight visa option. </summary>
    [System.ComponentModel.Description( "Keysight Visa" )]
    KeysightVisa = Pith + 0x4,

    /// <summary> An enum constant representing the dummy visa option. </summary>
    [System.ComponentModel.Description( "Dummy Visa" )]
    DummyVisa = Pith + 0x5,

    /// <summary> An enum constant representing the device option. </summary>
    [System.ComponentModel.Description( "Device" )]
    Device = Pith + 0x6,

    /// <summary> An enum constant representing the tsp device option. </summary>
    [System.ComponentModel.Description( "Tsp Device" )]
    TspDevice = Pith + 0x7,

    /// <summary> An enum constant representing the tsp script option. </summary>
    [System.ComponentModel.Description( "Tsp Script " )]
    TspScript = Pith + 0x8,

    /// <summary> An enum constant representing the tsp 2 device option. </summary>
    [System.ComponentModel.Description( "Tsp2 Device" )]
    Tsp2Device = Pith + 0xA,

    /// <summary> An enum constant representing the facade option. </summary>
    [System.ComponentModel.Description( "Facade" )]
    Facade = Pith + 0x10,

    /// <summary> An enum constant representing the instrument option. </summary>
    [System.ComponentModel.Description( "Instrument" )]
    Instrument = Facade + 0x1,

    /// <summary> An enum constant representing the 5700 option. </summary>
    [System.ComponentModel.Description( "N5700 Power Supply" )]
    N5700 = Pith + 0x20,

    /// <summary> An enum constant representing the source measure option. </summary>
    [System.ComponentModel.Description( "FREE `" )]
    SourceMeasure = N5700 + 0x1,

    /// <summary> An enum constant representing the scanner option. </summary>
    [System.ComponentModel.Description( "FREE 2" )]
    Scanner = N5700 + 0x2,

    /// <summary> An enum constant representing the multimeter option. </summary>
    [System.ComponentModel.Description( "FREE 3" )]
    Multimeter = N5700 + 0x3,

    /// <summary> An enum constant representing the eg 2000 prober option. </summary>
    [System.ComponentModel.Description( "EG2000 Prober" )]
    EG2000Prober = N5700 + 0x4,

    /// <summary> An enum constant representing the ats 600 option. </summary>
    [System.ComponentModel.Description( "Temptonic ATS600" )]
    Ats600 = N5700 + 0x5,

    /// <summary> An enum constant representing the 2002 option. </summary>
    [System.ComponentModel.Description( "K2002 Meter" )]
    K2002 = N5700 + 0x6,

    /// <summary> An enum constant representing the 2700 option. </summary>
    [System.ComponentModel.Description( "K2700 Meter/Scanner" )]
    K2700 = N5700 + 0x7,

    /// <summary> An enum constant representing the 1700 option. </summary>
    [System.ComponentModel.Description( "Tegam T1700 Meter" )]
    T1700 = N5700 + 0x8,

    /// <summary> An enum constant representing the 7000 option. </summary>
    [System.ComponentModel.Description( "K7000 Switch" )]
    K7000 = N5700 + 0x9,

    /// <summary> An enum constant representing the 3700 option. </summary>
    [System.ComponentModel.Description( "K3700 Meter/Scanner" )]
    K3700 = N5700 + 0xA,

    /// <summary> An enum constant representing the ttm driver option. </summary>
    [System.ComponentModel.Description( "TTM Driver" )]
    TtmDriver = N5700 + 0xB,

    /// <summary> An enum constant representing the 2600 option. </summary>
    [System.ComponentModel.Description( "K2600 Source Meter" )]
    K2600 = N5700 + 0xC,

    /// <summary> An enum constant representing the 4990 option. </summary>
    [System.ComponentModel.Description( "E4990 Impedance Analyzer" )]
    E4990 = N5700 + 0xD,

    /// <summary> An enum constant representing the 2400 option. </summary>
    [System.ComponentModel.Description( "K2400 Source Meter" )]
    K2400 = N5700 + 0xE,

    /// <summary> An enum constant representing the 7500 option. </summary>
    [System.ComponentModel.Description( "K7500 Meter" )]
    K7500 = N5700 + 0xF,

    /// <summary> An enum constant representing the 34980 option. </summary>
    [System.ComponentModel.Description( "K34980 Meter/Scanner" )]
    K34980 = N5700 + 0x10,

    /// <summary> An enum constant representing the 3458 option. </summary>
    [System.ComponentModel.Description( "K3458 Meter" )]
    K3458 = N5700 + 0x11,

    /// <summary> An enum constant representing the 2450 option. </summary>
    [System.ComponentModel.Description( "K2450 Source Meter" )]
    K2450 = N5700 + 0x12,

    /// <summary> An enum constant representing the 7500 tsp option. </summary>
    [System.ComponentModel.Description( "K7500 TSP2 Meter" )]
    K7500Tsp = N5700 + 0x13,

    /// <summary> An enum constant representing the tsp rig option. </summary>
    [System.ComponentModel.Description( "TSP Rig" )]
    TspRig = N5700 + 0x14,

    /// <summary> An enum constant representing the device facade option. </summary>
    [System.ComponentModel.Description( "Device Facade" )]
    DeviceFacade = Pith + 0x40,

    /// <summary> An enum constant representing the ttm console option. </summary>
    [System.ComponentModel.Description( "TTM Console" )]
    TtmConsole = DeviceFacade + 0x1,

    /// <summary> An enum constant representing the ttm console option. </summary>
    [System.ComponentModel.Description( "CLT10" )]
    Clt10 = TtmConsole + 0x1,

    /// <summary> An enum constant representing the device tests option. </summary>
    [System.ComponentModel.Description( "Device Tests" )]
    DeviceTests = Pith + 0x50,

    /// <summary> An enum constant representing the try tests option. </summary>
    [System.ComponentModel.Description( "Try Tests" )]
    TryTests = DeviceTests + 0x1,

    /// <summary> An enum constant representing the eg 2000 prober tests option. </summary>
    [System.ComponentModel.Description( "EG2000 Tests" )]
    EG2000ProberTests = DeviceTests + 0x2,

    /// <summary> An enum constant representing the thermostream tests option. </summary>
    [System.ComponentModel.Description( "Thermostream Tests" )]
    ThermostreamTests = DeviceTests + 0x3,

    /// <summary> An enum constant representing the 2002 tests option. </summary>
    [System.ComponentModel.Description( "K2002 Tests" )]
    K2002Tests = DeviceTests + 0x4,

    /// <summary> An enum constant representing the 2700 tests option. </summary>
    [System.ComponentModel.Description( "K2700 Tests" )]
    K2700Tests = DeviceTests + 0x5,

    /// <summary> An enum constant representing the tegam tests option. </summary>
    [System.ComponentModel.Description( "Tegam Tests" )]
    TegamTests = DeviceTests + 0x6,

    /// <summary> An enum constant representing the 7000 tests option. </summary>
    [System.ComponentModel.Description( "K7000 Tests" )]
    K7000Tests = DeviceTests + 0x7,

    /// <summary> An enum constant representing the 3700 tests option. </summary>
    [System.ComponentModel.Description( "K3700 Tests" )]
    K3700Tests = DeviceTests + 0x8,

    /// <summary> An enum constant representing the ttm driver tests option. </summary>
    [System.ComponentModel.Description( "TTM Driver Tests" )]
    TtmDriverTests = DeviceTests + 0x9,

    /// <summary> An enum constant representing the 2600 tests option. </summary>
    [System.ComponentModel.Description( "K2600 Tests" )]
    K2600Tests = DeviceTests + 0xA,

    /// <summary> An enum constant representing the 4990 tests option. </summary>
    [System.ComponentModel.Description( "E4990 Tests" )]
    E4990Tests = DeviceTests + 0xB,

    /// <summary> An enum constant representing the 2400 tests option. </summary>
    [System.ComponentModel.Description( "K2400 Tests" )]
    K2400Tests = DeviceTests + 0xD,

    /// <summary> An enum constant representing the 7500 tests option. </summary>
    [System.ComponentModel.Description( "K7500 Tests" )]
    K7500Tests = DeviceTests + 0xD,

    /// <summary> An enum constant representing the 34980 tests option. </summary>
    [System.ComponentModel.Description( "K34980 Tests" )]
    K34980Tests = DeviceTests + 0xE,

    /// <summary> An enum constant representing the 3458 tests option. </summary>
    [System.ComponentModel.Description( "K3458 Tests" )]
    K3458Tests = DeviceTests + 0xF,

    /// <summary> An enum constant representing the 2450 tests option. </summary>
    [System.ComponentModel.Description( "K2450 Tests" )]
    K2450Tests = DeviceTests + 0x10,

    /// <summary> An enum constant representing the 7500 tsp tests option. </summary>
    [System.ComponentModel.Description( "K7500 TSP2 Tests" )]
    K7500TspTests = DeviceTests + 0x11,

    /// <summary> An enum constant representing the 5700 tests option. </summary>
    [System.ComponentModel.Description( "N5700 Tests" )]
    N5700Tests = DeviceTests + 0x12
}
