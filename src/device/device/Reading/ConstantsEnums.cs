namespace cc.isr.VI;

/// <summary>   A measurand. </summary>
/// <remarks>   David, 2020-11-30. </remarks>
public static class Measurand
{
    /// <summary> The level compliance bit. </summary>
    public const int LevelComplianceBit = 32;

    /// <summary> The meta status bits base. </summary>
    public const int MetaStatusBitBase = LevelComplianceBit + 1;
}
/// <summary> Holds the measurand status bits. </summary>
/// <remarks>
/// Based above 32 bits so that these can be added to the extended 64 bit status word which lower
/// 32 bits hold the standard status word.
/// </remarks>
public enum MetaStatusBit
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None = 0,

    /// <summary> An enum constant representing the valid option. </summary>
    [System.ComponentModel.Description( "Valid" )]
    Valid = Measurand.MetaStatusBitBase,

    /// <summary> An enum constant representing the has value option. </summary>
    [System.ComponentModel.Description( "Has Value" )]
    HasValue = Measurand.MetaStatusBitBase + 1,

    /// <summary> An enum constant representing the not a number option. </summary>
    [System.ComponentModel.Description( "Not a number" )]
    NotANumber = Measurand.MetaStatusBitBase + 2,

    /// <summary> An enum constant representing the infinity option. </summary>
    [System.ComponentModel.Description( "Infinity" )]
    Infinity = Measurand.MetaStatusBitBase + 3,

    /// <summary> An enum constant representing the negative infinity option. </summary>
    [System.ComponentModel.Description( "Negative Infinity" )]
    NegativeInfinity = Measurand.MetaStatusBitBase + 4,

    /// <summary> An enum constant representing the hit status compliance option. </summary>
    [System.ComponentModel.Description( "Hit Status Compliance" )]
    HitStatusCompliance = Measurand.MetaStatusBitBase + 5,

    /// <summary> An enum constant representing the hit level compliance option. </summary>
    [System.ComponentModel.Description( "Hit Level Compliance" )]
    HitLevelCompliance = Measurand.MetaStatusBitBase + 6,

    /// <summary> An enum constant representing the hit range compliance option. </summary>
    [System.ComponentModel.Description( "Hit Range Compliance" )]
    HitRangeCompliance = Measurand.MetaStatusBitBase + 7,

    /// <summary> An enum constant representing the failed contact check option. </summary>
    [System.ComponentModel.Description( "Failed Contact Check" )]
    FailedContactCheck = Measurand.MetaStatusBitBase + 8,

    /// <summary> An enum constant representing the hit voltage protection option. </summary>
    [System.ComponentModel.Description( "Hit Voltage Protection" )]
    HitVoltageProtection = Measurand.MetaStatusBitBase + 9,

    /// <summary> An enum constant representing the hit over range option. </summary>
    [System.ComponentModel.Description( "Measured while over range" )]
    HitOverRange = Measurand.MetaStatusBitBase + 10,

    /// <summary> An enum constant representing the pass option. </summary>
    [System.ComponentModel.Description( "Pass" )]
    Pass = Measurand.MetaStatusBitBase + 11,

    /// <summary> An enum constant representing the high option. </summary>
    [System.ComponentModel.Description( "High" )]
    High = Measurand.MetaStatusBitBase + 12,

    /// <summary> An enum constant representing the low option. </summary>
    [System.ComponentModel.Description( "Low" )]
    Low = Measurand.MetaStatusBitBase + 13
}
/// <summary> Enumerates reading elements types the instrument is capable of. </summary>
[Flags()]
public enum ReadingElementTypes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None = 0,

    /// <summary> An enum constant representing the reading option. </summary>
    [System.ComponentModel.Description( "Reading (READ)" )]
    Reading = 1 << 0,

    /// <summary> An enum constant representing the timestamp option. </summary>
    [System.ComponentModel.Description( "Time Stamp (TST)" )]
    Timestamp = 1 << 1,

    /// <summary> An enum constant representing the units option. </summary>
    [System.ComponentModel.Description( "Units (UNIT)" )]
    Units = 1 << 2,

    /// <summary> An enum constant representing the reading number option. </summary>
    [System.ComponentModel.Description( "Reading Number (RNUM)" )]
    ReadingNumber = 1 << 3,

    /// <summary> An enum constant representing the source option. </summary>
    [System.ComponentModel.Description( "Source (SOUR)" )]
    Source = 1 << 4,

    /// <summary> An enum constant representing the compliance option. </summary>
    [System.ComponentModel.Description( "Compliance (COMP)" )]
    Compliance = 1 << 5,

    /// <summary> An enum constant representing the average voltage option. </summary>
    [System.ComponentModel.Description( "Average Voltage (AVOL)" )]
    AverageVoltage = 1 << 6,

    /// <summary> An enum constant representing the voltage option. </summary>
    [System.ComponentModel.Description( "Voltage (VOLT)" )]
    Voltage = 1 << 7,

    /// <summary> An enum constant representing the current option. </summary>
    [System.ComponentModel.Description( "Current (CURR)" )]
    Current = 1 << 8,

    /// <summary> An enum constant representing the resistance option. </summary>
    [System.ComponentModel.Description( "Resistance (RES)" )]
    Resistance = 1 << 9,

    /// <summary> An enum constant representing the time option. </summary>
    [System.ComponentModel.Description( "Time (TIME)" )]
    Time = 1 << 10,

    /// <summary> An enum constant representing the status option. </summary>
    [System.ComponentModel.Description( "Status (STAT)" )]
    Status = 1 << 11,

    /// <summary> An enum constant representing the channel option. </summary>
    [System.ComponentModel.Description( "Channel (CHAN)" )]
    Channel = 1 << 12,

    /// <summary> An enum constant representing the limits option. </summary>
    [System.ComponentModel.Description( "Limits (LIM)" )]
    Limits = 1 << 13,

    /// <summary> An enum constant representing the seconds option. </summary>
    [System.ComponentModel.Description( "Seconds (SEC)" )]
    Seconds = 1 << 14,

    /// <summary> An enum constant representing the primary option. </summary>
    [System.ComponentModel.Description( "Primary (PRI)" )]
    Primary = 1 << 15,

    /// <summary> An enum constant representing the secondary option. </summary>
    [System.ComponentModel.Description( "Secondary (SEC)" )]
    Secondary = 1 << 16
}

