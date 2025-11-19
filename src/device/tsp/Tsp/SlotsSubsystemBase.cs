namespace cc.isr.VI.Tsp;

/// <summary>   A slots subsystem base. </summary>
/// <remarks>   (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
///             Licensed under The MIT License.</para><para>
///             David, 2016-02-15 </para> </remarks>
/// <remarks>   Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class. </remarks>
/// <param name="maxSlotCount">     Number of maximum slots. </param>
/// <param name="statusSubsystem">  A reference to a <see cref="VI.StatusSubsystemBase">status
///                                 Subsystem</see>. </param>
public abstract class SlotsSubsystemBase( int maxSlotCount, Tsp.StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Customizes the reset state. </remarks>
    public override void InitKnownState()
    {
        base.InitKnownState();
        foreach ( SlotSubsystemBase s in this.Slots )
        {
            _ = s.QuerySlotExists();
            if ( s.IsSlotExists == true )
            {
                _ = s.QuerySupportsInterlock();
                _ = s.QueryInterlocksState();
            }
        }
    }

    #endregion

    #region " exists "

    /// <summary> The slots. </summary>
    /// <value> The slots. </value>
    public SlotCollection Slots { get; private set; } = [];

    /// <summary> Gets or sets the number of maximum slots. </summary>
    /// <value> The number of maximum slots. </value>
    public int MaximumSlotCount { get; private set; } = maxSlotCount;

    #endregion
}
/// <summary> Collection of slots. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-02-15 </para>
/// </remarks>
public class SlotCollection : System.Collections.ObjectModel.KeyedCollection<int, SlotSubsystemBase>
{
    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns> The key for the specified element. </returns>
    protected override int GetKeyForItem( SlotSubsystemBase item )
    {
        return item is not null ? item.SlotNumber : default;
    }
}
