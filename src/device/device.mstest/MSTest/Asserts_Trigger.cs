using cc.isr.Std.SplitExtensions;
using cc.isr.VI.Device.MSTest.Settings;

namespace cc.isr.VI.Device.MSTest;

public sealed partial class Asserts
{
    #region " trigger subsystem "

    /// <summary> Assert initial subsystem values should match. </summary>
    /// <param name="subsystem">      The subsystem. </param>
    /// <param name="subsystemsInfo"> Information describing the subsystems. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( TriggerSubsystemBase subsystem, SubsystemsSettingsBase? subsystemsInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystemsInfo, $"{nameof( subsystemsInfo )} should not be null." );
        string propertyName = nameof( TriggerSubsystemBase.TriggerSource ).SplitWords();
        TriggerSources expectedTriggerSource = subsystemsInfo.InitialTriggerSource;
        TriggerSources? actualTriggerSource = subsystem.QueryTriggerSource();
        Assert.IsTrue( actualTriggerSource.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedTriggerSource, actualTriggerSource.Value, $"{subsystem.ResourceNameCaption} initial {propertyName} should be expected" );
    }

    #endregion
}
