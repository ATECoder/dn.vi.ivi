namespace cc.isr.VI.Subsystem;
/// <summary>   Interface for presettable. </summary>
/// <remarks>   2024-09-10. </remarks>
public interface IPresettable
{
    /// <summary>
    /// Defines the clear execution state (CLS) by setting system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    /// <remarks> Clears the queues and sets all registers to zero. </remarks>
    public void DefineClearExecutionState();

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Customizes the reset state. </remarks>
    public void InitKnownState();

    /// <summary> Sets the subsystem known preset state. </summary>
    public void PresetKnownState();

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks> Clears the queues and sets all registers to zero. </remarks>
    public void DefineKnownResetState();
}
