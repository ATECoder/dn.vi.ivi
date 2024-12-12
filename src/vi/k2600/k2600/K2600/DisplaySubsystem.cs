namespace cc.isr.VI.Tsp.K2600;

/// <summary> Display subsystem. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-14 </para>
/// </remarks>
/// <remarks> Initializes a new instance of the <see cref="DisplaySubsystem" /> class. </remarks>
/// <remarks> David, 2020-10-12. </remarks>
/// <param name="statusSubsystem">  A reference to the
///                                 <see cref="P:isr.VI.Tsp.StatusSubsystemBase">TSP status
///                                 Subsystem</see>. </param>
public class DisplaySubsystem( Tsp.StatusSubsystemBase statusSubsystem ) : DisplaySubsystemBase( statusSubsystem )
{
    #region " commands: enabled "

    /// <summary> Gets or sets the display enable command format. </summary>
    /// <value> The display enable command format. </value>
    protected override string DisplayEnableCommandFormat { get; set; } = string.Empty;

    /// <summary> Gets or sets the display enabled query command. </summary>
    /// <value> The display enabled query command. </value>
    protected override string DisplayEnabledQueryCommand { get; set; } = string.Empty;

    #endregion
}
