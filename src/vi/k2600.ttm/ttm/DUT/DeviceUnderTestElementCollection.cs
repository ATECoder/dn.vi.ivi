namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Collection of device under test elements. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public partial class DeviceUnderTestElementCollection : System.Collections.ObjectModel.Collection<DeviceUnderTestElementBase>
{
    #region " construction "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public DeviceUnderTestElementCollection() : base()
    {
    }

    #endregion

    #region " i presettable "

    /// <summary>
    /// Sets values to their clear exception state. Clears the queues and resets all registers to
    /// zero. Sets the subsystem properties to the following CLS default values:<para>
    /// </para>
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void DefineClearExecutionState()
    {
        foreach ( DeviceUnderTestElementBase element in this.Items )
            element.DefineClearExecutionState();
    }

    /// <summary>
    /// Performs a reset and additional custom setting for the subsystem:<para>
    /// </para>
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void InitKnownState()
    {
        foreach ( DeviceUnderTestElementBase element in this.Items )
            element.InitKnownState();
    }

    /// <summary>
    /// Gets subsystem to the following default system preset values:<para>
    /// </para>
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void PresetKnownState()
    {
        foreach ( DeviceUnderTestElementBase element in this.Items )
            element.PresetKnownState();
    }

    /// <summary>
    /// Restore member properties to the following RST or System Preset values:<para>
    /// </para>
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void ResetKnownState()
    {
        foreach ( DeviceUnderTestElementBase element in this.Items )
            element.ResetKnownState();
    }

    #endregion
}
