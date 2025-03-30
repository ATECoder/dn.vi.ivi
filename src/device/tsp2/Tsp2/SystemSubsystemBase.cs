
namespace cc.isr.VI.Tsp2;

/// <summary> Defines a System Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-10-07 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class SystemSubsystemBase : VI.SystemSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
    /// Subsystem</see>. </param>
    protected SystemSubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
    }

    #endregion

    #region " syntax "

    /// <summary> Gets or sets the Firmware Version query command. </summary>
    /// <value> The Firmware Version query command. </value>
    protected override string FirmwareVersionQueryCommand { get; set; } = Syntax.Tsp.LocalNode.FirmwareVersionQueryCommand;

    #endregion

}
