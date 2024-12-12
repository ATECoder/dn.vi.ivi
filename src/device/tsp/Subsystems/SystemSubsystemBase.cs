namespace cc.isr.VI.Tsp;

/// <summary> Defines a System Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-10-07 </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SystemSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
/// Subsystem</see>. </param>
public abstract class SystemSubsystemBase( Tsp.StatusSubsystemBase statusSubsystem ) : VI.SystemSubsystemBase( statusSubsystem )
{
    #region " syntax "

    /// <summary> Gets or sets the TSP revision query command. </summary>
    /// <value> The language revision query command. </value>
    protected override string LanguageRevisionQueryCommand { get; set; } = "_G.print(_G.localnode.revision)";

    /// <summary>   Not supported. </summary>
    protected override string PresetCommand { get; set; } = string.Empty;

    #endregion
}
