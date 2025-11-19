using cc.isr.Enums;

namespace cc.isr.VI.Tsp;

/// <summary> Defines a Multimeter Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-01-15 </para>
/// </remarks>
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
    protected MultimeterSubsystemBase( Tsp.StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem, readingAmounts )
    {
        this.FunctionModeReadWrites.Clear();
        this.FunctionModeReadWrites.Add( ( long ) MultimeterFunctionModes.CurrentAC, "accurrent", MultimeterFunctionModes.CurrentAC.DescriptionUntil() );
        this.FunctionModeReadWrites.Add( ( long ) MultimeterFunctionModes.ResistanceCommonSide, "commonsideohms", MultimeterFunctionModes.ResistanceCommonSide.DescriptionUntil() );
        this.FunctionModeReadWrites.Add( ( long ) MultimeterFunctionModes.ResistanceFourWire, "fourwireohms", MultimeterFunctionModes.ResistanceFourWire.DescriptionUntil() );
        this.FunctionModeReadWrites.Add( ( long ) MultimeterFunctionModes.ResistanceTwoWire, "twowireohms", MultimeterFunctionModes.ResistanceTwoWire.DescriptionUntil() );
        this.FunctionModeReadWrites.Add( ( long ) MultimeterFunctionModes.VoltageAC, "acvolts", MultimeterFunctionModes.VoltageAC.DescriptionUntil() );
        this.FunctionModeReadWrites.Add( ( long ) MultimeterFunctionModes.VoltageDC, "dcvolts", MultimeterFunctionModes.VoltageDC.DescriptionUntil() );
        this.SupportedFunctionModes = MultimeterFunctionModes.CurrentAC;
        foreach ( Pith.EnumReadWrite kvp in this.FunctionModeReadWrites )
            this.SupportedFunctionModes |= ( MultimeterFunctionModes ) ( int ) kvp.EnumValue;
    }

    #endregion
}
