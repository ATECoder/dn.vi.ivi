using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Enumerates the measurement outcomes. </summary>
/// <remarks> David, 2020-10-12. </remarks>
[Flags()]
public enum MeasurementOutcomes
{
    /// <summary> An enum constant representing the undefined (none) option. </summary>
    [Description( "Not Defined" )]
    None,

    /// <summary> An enum constant representing the part passed option. </summary>
    [Description( "Measured value is in range" )]
    PartPassed = 1,

    /// <summary> Out of range. </summary>
    [Description( "Measured values is out of range" )]
    PartFailed = 2,

    /// <summary> Measurement Failed. </summary>
    [Description( "Measurement Failed" )]
    MeasurementFailed = 4,

    /// <summary> Measurement Not made. </summary>
    [Description( "Measurement Not Made" )]
    MeasurementNotMade = 8,

    /// <summary> Instrument failed to measure because it hit the voltage limit. </summary>
    [Description( "Compliance" )]
    HitCompliance = 16,

    /// <summary> The message received from the instrument did not parse. </summary>
    [Description( "Unexpected Reading format" )]
    UnexpectedReadingFormat = 32,

    /// <summary> The message received from the instrument did not parse. </summary>
    [Description( "Unexpected MeasurementOutcome format" )]
    UnexpectedOutcomeFormat = 64,

    /// <summary> A failure occurred at the device level that was not parsed. </summary>
    [Description( "Unspecified Firmware MeasurementOutcome" )]
    UnspecifiedFirmwareOutcome = 128,

    /// <summary> One of a number of failure occurred at the program level. </summary>
    [Description( "Unspecified Program Failure" )]
    UnspecifiedProgramFailure = 256,

    /// <summary> Device Error. </summary>
    [Description( "Device Error" )]
    DeviceError = 512,

    /// <summary> An enum constant representing the failed contact check option. </summary>
    [Description( "Failed Contact Check" )]
    FailedContactCheck = 1024,

    /// <summary> An enum constant representing the open leads Firmware MeasurementOutcome. </summary>
    [Description( "Open Leads" )]
    OpenLeads = 2048,

    /// <summary> An enum constant representing the undefined 5 option. </summary>
    [Description( "Undefined5" )]
    Undefined5 = 4096,

    /// <summary> An enum constant representing the undefined 6 option. </summary>
    [Description( "Undefined6" )]
    Undefined6 = 8192,

    /// <summary> Unknown outcome - assert. </summary>
    [Description( "Unknown outcome - assert" )]
    UnknownOutcome = 2048 + 4096 + 8192
}

/// <summary> Enumerates the display screens and special status. </summary>
/// <remarks> David, 2020-10-12. </remarks>
[Flags()]
public enum DisplayScreens
{
    /// <summary> Not defined. </summary>
    [Description( "Not defined" )]
    None = 0,

    /// <summary> Custom lines are displayed. </summary>
    [Description( "Default screen" )]
    Default = 1,

    /// <summary> Cleared user screen mode. </summary>
    [Description( "User screen" )]
    User = 128,

    /// <summary> Last command displayed title. </summary>
    [Description( "Title is displayed" )]
    Title = 256,

    /// <summary> Custom lines are displayed. </summary>
    [Description( "Special display" )]
    Custom = 512,

    /// <summary> Measurement displayed. </summary>
    [Description( "Measurement is displayed" )]
    Measurement = 1024
}

/// <summary> Specifies the contact check speed modes. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum ContactCheckSpeedMode
{
    /// <summary> An enum constant representing the none option. </summary>
    [Description( "None" )]
    None,

    /// <summary> An enum constant representing the fast option. </summary>
    [Description( "Fast (CONTACT_FAST)" )]
    Fast,

    /// <summary> An enum constant representing the medium option. </summary>
    [Description( "Medium (CONTACT_MEDIUM)" )]
    Medium,

    /// <summary> An enum constant representing the slow option. </summary>
    [Description( "Slow (CONTACT_SLOW)" )]
    Slow
}

/// <summary> Values that represent ThermalTransientMeterEntity. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum ThermalTransientMeterEntity
{
    /// <summary> An enum constant representing the none option. </summary>
    [Description( "None" )]
    None = 0,

    /// <summary> Meter Main entity. </summary>
    [Description( "Meter Main (_G.ttm)" )]
    MeterMain,

    /// <summary> Initial resistance entity. </summary>
    [Description( "Initial MeasuredValue (_G.ttm.ir)" )]
    InitialResistance,

    /// <summary> Final resistance entity. </summary>
    [Description( "Initial MeasuredValue (_G.ttm.fr)" )]
    FinalResistance,

    /// <summary> Thermal Transient entity. </summary>
    [Description( "Thermal Transient (_G.ttm.tr)" )]
    Transient,

    /// <summary> Thermal Transient Estimator. </summary>
    [Description( "Thermal Transient Estimator (_G.ttm.est)" )]
    Estimator,

    /// <summary> Shunt resistance entity. </summary>
    [Description( "Shunt MeasuredValue (_G)" )]
    Shunt
}
