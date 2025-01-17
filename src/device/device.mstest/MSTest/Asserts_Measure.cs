namespace cc.isr.VI.Device.MSTest;

public sealed partial class Asserts
{
    #region " measure subsystem "

    /// <summary> Assert initial subsystem values should match
    ///           . </summary>
    /// <param name="subsystem">      The subsystem. </param>
    /// <param name="subsystemsInfo"> Information describing the subsystems. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( MeasureSubsystemBase subsystem, VI.Settings.SubsystemsSettings? subsystemsInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystemsInfo, $"{nameof( subsystemsInfo )} should not be null." );
    }

    #endregion
}
