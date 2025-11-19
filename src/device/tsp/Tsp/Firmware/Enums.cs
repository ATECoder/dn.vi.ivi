using System.ComponentModel;

namespace cc.isr.VI.Tsp.Firmware;

/// <summary> Values that represent the firmware status. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum FirmwareStatus
{
    /// <summary> An enum constant representing the current option. </summary>
    [Description( "The embedded firmware is up to date." )]
    Current = 0,

    /// <summary> An enum constant representing the new version available option. </summary>
    [Description( "A new embedded firmware is available to download." )]
    NewVersionAvailable = 1,

    /// <summary> An enum constant representing the load firmware option. </summary>
    [Description( "The firmware needs to be loaded." )]
    LoadFirmware = 2,

    /// <summary> An enum constant representing the update required option. </summary>
    [Description( "The firmware needs updating." )]
    UpdateRequired = 3,
}

