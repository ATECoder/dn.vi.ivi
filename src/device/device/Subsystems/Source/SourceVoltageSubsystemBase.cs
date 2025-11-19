namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a SCPI Source Voltage Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SourceVoltageSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class SourceVoltageSubsystemBase( StatusSubsystemBase statusSubsystem ) : SourceFunctionSubsystemBase( statusSubsystem )
{
    #region " auto range "

    /// <summary> Gets or sets the automatic Range enabled query command. </summary>
    /// <value> The automatic Range enabled query command. </value>
    protected override string AutoRangeEnabledQueryCommand { get; set; } = ":SOUR:VOLT:RANG:AUTO?";

    /// <summary> Gets or sets the automatic Range enabled command Format. </summary>
    /// <value> The automatic Range enabled query command. </value>
    protected override string AutoRangeEnabledCommandFormat { get; set; } = ":SOUR:VOLT:RANG:AUTO {0:'ON';'ON';'OFF'}";

    #endregion

    #region " level "

    /// <summary> Gets or sets the Level query command. </summary>
    /// <value> The Level query command. </value>
    protected override string LevelQueryCommand { get; set; } = ":SOUR:VOLT?";

    /// <summary> Gets or sets the Level command format. </summary>
    /// <value> The Level command format. </value>
    protected override string LevelCommandFormat { get; set; } = ":SOUR:VOLT {0}";

    #endregion

    #region " sweep start level "

    /// <summary> Gets or sets the Sweep Start Level query command. </summary>
    /// <value> The Sweep Start Level query command. </value>
    protected override string SweepStartLevelQueryCommand { get; set; } = ":SOUR:VOLT:STAR?";

    /// <summary> Gets or sets the Sweep Start Level command format. </summary>
    /// <value> The Sweep Start Level command format. </value>
    protected override string SweepStartLevelCommandFormat { get; set; } = ":SOUR:VOLT:STAR {0}";

    #endregion

    #region " sweep stop level "

    /// <summary> Gets or sets the Sweep Stop Level query command. </summary>
    /// <value> The Sweep Stop Level query command. </value>
    protected override string SweepStopLevelQueryCommand { get; set; } = ":SOUR:VOLT:STOP?";

    /// <summary> Gets or sets the Sweep Stop Level command format. </summary>
    /// <value> The Sweep Stop Level command format. </value>
    protected override string SweepStopLevelCommandFormat { get; set; } = ":SOUR:VOLT:STOP {0}";

    #endregion

    #region " sweep mode "

    /// <summary> Gets or sets the Sweep Mode  query command. </summary>
    /// <value> The Sweep Mode  query command. </value>
    protected override string SweepModeQueryCommand { get; set; } = ":SOUR:VOLT:MODE?";

    /// <summary> Gets or sets the Sweep Mode  command format. </summary>
    /// <value> The Sweep Mode  command format. </value>
    protected override string SweepModeCommandFormat { get; set; } = ":SOUR:VOLT:MODE {0}";

    #endregion
}
