

namespace cc.isr.VI.Tsp2;

/// <summary> Defines a Multimeter Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-01-15 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class MultimeterSubsystemBase : VI.MultimeterSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
    /// </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">status
    /// Subsystem</see>. </param>
    /// <param name="readingAmounts">  The reading amounts. </param>
    protected MultimeterSubsystemBase( VI.StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem, readingAmounts )
    {
        _ = this.FunctionModeReadWrites.RemoveAt( ( int ) MultimeterFunctionModes.ResistanceCommonSide );
        this.SupportedFunctionModes = MultimeterFunctionModes.CurrentAC;
        foreach ( Pith.EnumReadWrite kvp in this.FunctionModeReadWrites )
            this.SupportedFunctionModes |= ( MultimeterFunctionModes ) ( int ) (kvp.EnumValue);
    }

    #endregion

}
