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

    /// <summary>   A binary constant representing the unknown flag. </summary>
    [Description( "Unknown" )]
    Unknown = 128
}

/// <summary>   A bit-field of flags for specifying contact check options. </summary>
/// <remarks>   2024-10-31. </remarks>
[Flags()]
public enum ContactCheckOptions
{
    /// <summary>   A binary constant representing the none flag. </summary>
    [Description( "None" )]
    None = 0,

    /// <summary>   A binary constant representing the pre initial resistance contact check option. </summary>
    [Description( "Initial" )]
    Initial = 1,

    /// <summary>   A binary constant representing the pre trace  contact check option. </summary>
    [Description( "Pre-Trace" )]
    PreTrace = 2,

    /// <summary>   A binary constant representing the pre final resistance contact check option. </summary>
    [Description( "Final" )]
    Final = 4,
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

/// <summary>   Values that represent pass fail bits. </summary>
/// <remarks>   2025-01-27. </remarks>
[Flags()]
public enum PassFailBits
{
    /// <summary>   An enum constant representing the none flag. </summary>
    [Description( "None" )]
    None = 0,
    /// <summary>   An enum constant representing the pass flag. </summary>

    [Description( "Pass" )]
    Pass = 1,

    /// <summary>   An enum constant representing the low flag. </summary>
    [Description( "Low" )]
    Low = 2,

    /// <summary>   An enum constant representing the high flag. </summary>
    [Description( "High" )]
    High = 4,

    /// <summary>   A binary constant representing the unknown flag. </summary>
    [Description( "Unknown" )]
    Unknown = 128

}

/// <summary>   A bit-field of flags for specifying print options. </summary>
/// <remarks>   2025-01-27. </remarks>
[Flags()]
public enum PrintOptions
{
    /// <summary>   A binary constant representing the none flag. </summary>
    [Description( "None" )]
    None = 0,

    /// <summary>   A binary constant representing the header print option. </summary>
    [Description( "Header" )]
    Header = 1,

    /// <summary>   A binary constant representing the units print option. </summary>
    [Description( "Units" )]
    Units = 2,

    /// <summary>   A binary constant representing the values print option. </summary>
    [Description( "Values" )]
    Values = 4
}

/// <summary>   A bit-field of flags for specifying leads status bits. </summary>
/// <remarks>   2025-01-27. </remarks>
[Flags()]
public enum LeadsStatusBits
{
    /// <summary>   A binary constant representing the none flag. </summary>
    [Description( "None" )]
    Okay = 0,

    /// <summary>   A binary constant representing the open high leads status bit. </summary>
    [Description( "Open High Leads" )]
    OpenHighLeads = 1,

    /// <summary>   A binary constant representing the open low leads status bit. </summary>
    [Description( "Open Low Leads" )]
    OpenLowLeads = 2,

    /// <summary>   A binary constant representing the open high source leads status bit. </summary>
    [Description( "Open High Source Lead" )]
    OpenHighSourceLead = 4,

    /// <summary>   A binary constant representing the open low source leads status bit. </summary>
    [Description( "Open Low Source Lead" )]
    OpenLowSourceLead = 8,

    /// <summary>   A binary constant representing the unknown flag. </summary>
    [Description( "Unknown" )]
    Unknown = 128
}

/// <summary>   Values that represent digital lines. </summary>
/// <remarks>   2025-01-27. </remarks>
public enum DigitalLines
{
    /// <summary>   A constant representing the start line. </summary>
    [Description( "StartLine" )]
    StartLine = 1,

    /// <summary>   A constant representing the Start Acknowledge line. </summary>
    [Description( "StartAckLine" )]
    StartAckLine = 2,

    /// <summary>   A constant representing the done line. </summary>
    [Description( "DoneLine" )]
    DoneLine = 3,

    /// <summary>   A constant representing the Initial resistance line. </summary>
    [Description( "InitialLine" )]
    InitialLine = 4,

    /// <summary>   A constant representing the final resistance line. </summary>
    [Description( "FinalLine" )]
    FinalLine = 5,

    /// <summary>   A constant representing the transient trace line. </summary>
    [Description( "TransientLine" )]
    TransientLine = 6,

    /// <summary>   A constant representing the pass line. </summary>
    [Description( "PassLine" )]
    PassLine = 7,

    /// <summary>   A constant representing the Start Trigger line. </summary>
    [Description( "StartTriggerLine" )]
    StartTriggerLine = 8

}

/// <summary>   Values that represent pass fail digital levels. </summary>
/// <remarks>   2025-01-27. </remarks>
public enum PassFailDigitalLevels
{
    /// <summary>   A constant representing the failed level. </summary>
    [Description( "FailedLevel" )]
    FailedLevel = 0,

    /// <summary>   A constant representing the passed level. </summary>
    [Description( "PassedLevel" )]
    PassedLevel = 1
}

/// <summary>   Values that represent on off digital levels. </summary>
/// <remarks>   2025-01-27. </remarks>
public enum OnOffDigitalLevels
{
    /// <summary>   A constant representing the off level. </summary>
    [Description( "OffLevel" )]
    OffLevel = 0,

    /// <summary>   A constant representing the on level. </summary>
    [Description( "OnLevel" )]
    OnLevel = 1
}
