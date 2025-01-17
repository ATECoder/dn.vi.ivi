namespace cc.isr.VI.Device.MSTest;

public sealed partial class Asserts
{
    #region " measure subsystem "

    /// <summary> Assert initial subsystem values should match
    ///           . </summary>
    /// <param name="subsystem">      The subsystem. </param>
    /// <param name="subsystemInfo"> Information describing the subsystems. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( MeasureSubsystemBase subsystem, VI.Settings.SenseSubsystemSettings? subsystemInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystemInfo, $"{nameof( subsystemInfo )} should not be null." );
    }

    #endregion
}
