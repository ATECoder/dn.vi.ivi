using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm.Syntax;

/// <summary> Enumerates the firmware outcomes. </summary>
/// <remarks> David, 2020-10-12. </remarks>
[Flags()]
public enum FirmwareOutcomes
{
    /// <summary>   A binary constant representing the okay flag. </summary>
    [Description( "Initial" )]
    Okay = 0,

    /// <summary>   A binary constant indicating that sampling return bad status from the buffer. 
    ///             Also indicates open leads when the firmware is compatibility is set for the legacy driver.. </summary>
    [Description( "Bad status" )]
    badStatus = 1,

    /// <summary>   A binary constant indicating that sampling returned bad time stamps. </summary>
    [Description( "Base Time Stamps" )]
    badTimeStamps = 2,

    /// <summary>   A binary constant indicating that configuration failed. </summary>
    [Description( "Configuration Failed" )]
    configFailed = 4,

    /// <summary>   A binary constant indicating that pulse initiation failed. </summary>
    [Description( "Initiation Failed" )]
    initiationFailed = 8,

    /// <summary>   A binary constant indicating that scripts not loaded completely. </summary>
    [Description( "Scripts Not Loaded Completely." )]
    loadFailed = 16,

    /// <summary>   A binary constant indicating that a measurement failed (e.g., a null value returned from the voltage buffer). </summary>
    [Description( "Measurement Failed" )]
    measurementFailed = 32,

    /// <summary>   A binary constant indicating that the leads resistance is higher than the minimum acceptable value. </summary>
    [Description( "Open Leads" )]
    openLeads = 64,
}

/// <summary>   A bit-field of flags for specifying contact check options. </summary>
/// <remarks>   2024-10-31. </remarks>
[Flags()]
public enum ContactCheckOptions
{
    /// <summary>   A binary constant representing the initial flag. </summary>
    [Description( "Initial" )]
    Initial = 0,

    /// <summary>   A binary constant representing the pre trace flag. </summary>
    [Description( "Pre-Trace" )]
    PreTrace = 1,

    /// <summary>   A binary constant representing the final flag. </summary>
    [Description( "Final" )]
    Final = 2,
}

/// <summary>   Values that represent source output options. </summary>
/// <remarks>   2024-11-01. </remarks>
public enum SourceOutputOption
{
    /// <summary>   A constant representing the Current source output option. </summary>
    [Description( "Current" )]
    Current = 0,

    /// <summary>   A constant representing the Voltage source output option. </summary>
    [Description( "Voltage" )]
    Voltage = 1

}
