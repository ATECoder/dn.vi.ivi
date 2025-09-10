namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a SCPI Source Current Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SourceCurrentSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class SourceCurrentSubsystemBase( StatusSubsystemBase statusSubsystem ) : SourceFunctionSubsystemBase( statusSubsystem )
{
    #region " auto range "

    /// <summary> Gets or sets the automatic Range enabled query command. </summary>
    /// <value> The automatic Range enabled query command. </value>
    protected override string AutoRangeEnabledQueryCommand { get; set; } = ":SOUR:CURR:RANG:AUTO?";

    /// <summary> Gets or sets the automatic Range enabled command Format. </summary>
    /// <value> The automatic Range enabled query command. </value>
    protected override string AutoRangeEnabledCommandFormat { get; set; } = ":SOUR:CURR:RANG:AUTO {0:'ON';'ON';'OFF'}";

    #endregion

    #region " level "

    /// <summary> Gets or sets the Level query command. </summary>
    /// <value> The Level query command. </value>
    protected override string LevelQueryCommand { get; set; } = ":SOUR:CURR?";

    /// <summary> Gets or sets the Level command format. </summary>
    /// <value> The Level command format. </value>
    protected override string LevelCommandFormat { get; set; } = ":SOUR:CURR {0}";

    #endregion

    #region " protection enabled "

    /// <summary> The protection enabled. </summary>

    /// <summary>
    /// Gets or sets a cached value indicating whether source current protection is enabled.
    /// </summary>
    /// <remarks>
    /// :SOURCE:CURR:PROT:STAT The setter enables or disables the over-current protection (OCP)
    /// function. The enabled state is On (1); the disabled state is Off (0). If the over-current
    /// protection function is enabled and the output goes into constant current operation, the
    /// output is disabled and OCP is set in the Questionable Condition status register. The *RST
    /// value = Off.
    /// </remarks>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? ProtectionEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.ProtectionEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Protection Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyProtectionEnabled( bool value )
    {
        _ = this.WriteProtectionEnabled( value );
        return this.QueryProtectionEnabled();
    }

    /// <summary> Gets or sets the Protection enabled query command. </summary>
    /// <remarks> SCPI: ":SOUR:CURR:PROT:STAT?". </remarks>
    /// <value> The Protection enabled query command. </value>
    protected virtual string ProtectionEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Protection Enabled sentinel. Also sets the
    /// <see cref="ProtectionEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryProtectionEnabled()
    {
        this.ProtectionEnabled = this.Session.Query( this.ProtectionEnabled, this.ProtectionEnabledQueryCommand );
        return this.ProtectionEnabled;
    }

    /// <summary> Gets or sets the Protection enabled command Format. </summary>
    /// <remarks> SCPI: ":SOUR:CURR:PROT:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Protection enabled query command. </value>
    protected virtual string ProtectionEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Protection Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteProtectionEnabled( bool value )
    {
        this.ProtectionEnabled = this.Session.WriteLine( value, this.ProtectionEnabledCommandFormat );
        return this.ProtectionEnabled;
    }

    #endregion

    #region " sweep start level "

    /// <summary> Gets or sets the Sweep Start Level query command. </summary>
    /// <value> The Sweep Start Level query command. </value>
    protected override string SweepStartLevelQueryCommand { get; set; } = ":SOUR:CURR:STAR?";

    /// <summary> Gets or sets the Sweep Start Level command format. </summary>
    /// <value> The Sweep Start Level command format. </value>
    protected override string SweepStartLevelCommandFormat { get; set; } = ":SOUR:CURR:STAR {0}";

    #endregion

    #region " sweep stop level "

    /// <summary> Gets or sets the Sweep Stop Level query command. </summary>
    /// <value> The Sweep Stop Level query command. </value>
    protected override string SweepStopLevelQueryCommand { get; set; } = ":SOUR:CURR:STOP?";

    /// <summary> Gets or sets the Sweep Stop Level command format. </summary>
    /// <value> The Sweep Stop Level command format. </value>
    protected override string SweepStopLevelCommandFormat { get; set; } = ":SOUR:CURR:STOP {0}";

    #endregion

    #region " sweep mode "

    /// <summary> Gets or sets the Sweep Mode  query command. </summary>
    /// <value> The Sweep Mode  query command. </value>
    protected override string SweepModeQueryCommand { get; set; } = ":SOUR:CURR:MODE?";

    /// <summary> Gets or sets the Sweep Mode  command format. </summary>
    /// <value> The Sweep Mode  command format. </value>
    protected override string SweepModeCommandFormat { get; set; } = ":SOUR:CURR:MODE {0}";

    #endregion
}
