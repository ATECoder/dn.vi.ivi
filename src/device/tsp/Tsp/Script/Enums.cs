namespace cc.isr.VI.Tsp.Script;

/// <summary>   Enumerates the script file formats. </summary>
/// <remarks>   David, 2020-10-12. </remarks>
[Flags()]
public enum ScriptFileFormats
{
    /// <summary> An enum constant representing the uncompressed human readable option. </summary>
    [System.ComponentModel.Description( "Uncompressed Human Readable" )]
    None = 0,

    /// <summary> An enum constant representing the compressed option. </summary>
    [System.ComponentModel.Description( "Compressed" )]
    Compressed = 1,

    /// <summary> An enum constant representing the byte code option. </summary>
    [System.ComponentModel.Description( "Byte Code format" )]
    ByteCode = 2,
}

/// <summary>   A bit-field of flags for specifying script statuses. </summary>
/// <remarks>   2025-04-25. </remarks>
[Flags()]
public enum ScriptStatuses
{
    /// <summary>   A bit constant representing the none flag. </summary>
    [System.ComponentModel.Description( "Not set" )]
    None = 0,

    /// <summary>   A bit constant representing the unknown flag. </summary>
    [System.ComponentModel.Description( "Unknow" )]
    Unknown = 1,

    /// <summary>   A bit constant representing the loaded flag. </summary>
    [System.ComponentModel.Description( "Loaded" )]
    Loaded = 2,

    /// <summary>   A bit constant representing the activated flag. </summary>
    [System.ComponentModel.Description( "Activated" )]
    Activated = 4,

    /// <summary>   A bit constant representing the saved flag. </summary>
    [System.ComponentModel.Description( "Saved" )]
    Saved = 8,

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
    [System.ComponentModel.Description( "Version as older than expected" )]
    Older = 1,

    /// <summary> Firmware is current.  </summary>
    [System.ComponentModel.Description( "Version is current" )]
    Current = 2,

    /// <summary> Embedded firmware is newer than expected indicating that the distributed program is out of date. </summary>
    [System.ComponentModel.Description( "Version as new than expected" )]
    Newer = 3,

    /// <summary> Embedded firmware version was not set because the script was not loaded or saved. </summary>
    [System.ComponentModel.Description( "Embedded version not known (empty)" )]
    Unknown = 4,

    /// <summary> Released firmware was not set. </summary>
    [System.ComponentModel.Description( "released firmware version was not specified (empty)" )]
    ReleaseVersionNotSet = 5,

    /// <summary> Version command function does not exist. </summary>
    [System.ComponentModel.Description( "Version command is missing -- version is nil" )]
    Missing = 6
}

