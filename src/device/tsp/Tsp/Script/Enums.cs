namespace cc.isr.VI.Tsp.Script;

/// <summary>   A bit-field of flags for specifying script statuses. </summary>
/// <remarks>   2025-04-25. </remarks>
[Flags()]
public enum ScriptStatuses
{
    /// <summary>   A bit constant representing the none flag. </summary>
    [System.ComponentModel.Description( "Not set" )]
    None = 0,

    /// <summary>   A bit constant representing the unknown flag. </summary>
    [System.ComponentModel.Description( "Unknown" )]
    Unknown = 1,

    /// <summary>   A bit constant representing the loaded flag. </summary>
    [System.ComponentModel.Description( "Loaded" )]
    Loaded = 2,

    /// <summary>   A bit constant representing the activated flag. </summary>
    [System.ComponentModel.Description( "Activated" )]
    Activated = 4,

    /// <summary>   A bit constant representing the embedded flag. </summary>
    [System.ComponentModel.Description( "Embedded" )]
    Embedded = 8,

    /// <summary>   A bit constant representing the byte code flag. </summary>
    [System.ComponentModel.Description( "Byte Code" )]
    ByteCode = 16

}

/// <summary>   Enumerates the validation status. </summary>
/// <remarks>   2024-09-05. </remarks>
public enum FirmwareVersionStatus
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "No Specified" )]
    None = 0,

    /// <summary> Firmware is older than expected (released) indicating that the firmware needs to be updated. </summary>
    [System.ComponentModel.Description( "Version is older than expected" )]
    Older = 1,

    /// <summary> Firmware is current.  </summary>
    [System.ComponentModel.Description( "Version is current" )]
    Current = 2,

    /// <summary> The embedded firmware is newer than expected indicating that the distributed program is out of date. </summary>
    [System.ComponentModel.Description( "The embedded version is newer than expected" )]
    Newer = 3,

    /// <summary> The embedded version is not known (empty). 
    ///           The embedded firmware version was not set because the script was not loaded or embedded. </summary>
    [System.ComponentModel.Description( "The embedded version is not known (empty)" )]
    Unknown = 4,

    /// <summary> The next version of the firmware was not specified (empty). </summary>
    [System.ComponentModel.Description( "The next version of the firmware was not specified (empty)" )]
    NextVersionNotSet = 5,

    /// <summary> The version query command is missing -- version is nil. </summary>
    [System.ComponentModel.Description( "The version query command is missing -- version is nil" )]
    Missing = 6
}

