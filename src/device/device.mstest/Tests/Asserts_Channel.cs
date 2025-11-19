namespace cc.isr.VI.Device.Tests;

public sealed partial class Asserts
{
    #region " channel subsystem "

    /// <summary> Assert initial subsystem values should match. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( ChannelSubsystemBase subsystem )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsTrue( string.IsNullOrWhiteSpace( subsystem.ClosedChannels ), $"Scan list {subsystem.ClosedChannels}; expected empty" );
    }

    #endregion
}
