using System;

namespace cc.isr.VI.Device.MSTest;

public sealed partial class Asserts
{
    #region " multimeter subsystem "

    /// <summary> Assert initial subsystem values should match. </summary>
    /// <param name="subsystem">      The subsystem. </param>
    /// <param name="subsystemsInfo"> Information describing the subsystems. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( MultimeterSubsystemBase subsystem, VI.Settings.SubsystemsSettings? subsystemsInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystemsInfo, $"{nameof( subsystemsInfo )} should not be null." );
        string propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.PowerLineCycles )}";
        double expectedPowerLineCycles = subsystemsInfo.InitialPowerLineCycles;
        double? actualPowerLineCycles = subsystem.QueryPowerLineCycles().GetValueOrDefault( 0d );
        Assert.IsTrue( actualPowerLineCycles.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );

        double epsilon = subsystemsInfo.LineFrequency / TimeSpan.TicksPerSecond;
        Assert.AreEqual( expectedPowerLineCycles, actualPowerLineCycles.Value, epsilon, $"{subsystem.ResourceNameCaption} {propertyName} value should match expect value withing {epsilon}" );
        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.AutoRangeEnabled )}";
        bool expectedBoolean = subsystemsInfo.InitialAutoRangeEnabled;
        bool? actualBoolean = subsystem.QueryAutoRangeEnabled().GetValueOrDefault( false );
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedBoolean, actualBoolean.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.AutoZeroEnabled )}";
        expectedBoolean = subsystemsInfo.InitialAutoZeroEnabled;
        actualBoolean = subsystem.QueryAutoZeroEnabled().GetValueOrDefault( false );
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedBoolean, actualBoolean.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.FunctionMode )}";
        MultimeterFunctionModes expectedFunctionMode = subsystemsInfo.InitialMultimeterFunction;
        MultimeterFunctionModes? actualFunctionMode = subsystem.QueryFunctionMode();
        Assert.IsTrue( actualFunctionMode.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedFunctionMode, ( object ) actualFunctionMode, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.FilterEnabled )}";
        expectedBoolean = subsystemsInfo.InitialFilterEnabled;
        actualBoolean = subsystem.QueryFilterEnabled();
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedBoolean, actualBoolean.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {propertyName}reading {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.MovingAverageFilterEnabled )}";
        expectedBoolean = subsystemsInfo.InitialMovingAverageFilterEnabled;
        actualBoolean = subsystem.QueryMovingAverageFilterEnabled();
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedBoolean, actualBoolean.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.FilterWindow )}";
        double expectedFilterWindow = subsystemsInfo.InitialFilterWindow;
        double? actualFilterWindow = subsystem.QueryFilterWindow();
        Assert.IsNotNull( actualFilterWindow );
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedFilterWindow, actualFilterWindow.Value, 0.1d * expectedFilterWindow, $"{subsystem.ResourceNameCaption} {propertyName} should match withing {epsilon}" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.FilterCount )}";
        int expectedFilterCount = subsystemsInfo.InitialFilterCount;
        int? actualFilterCount = subsystem.QueryFilterCount();
        Assert.IsTrue( actualFilterCount.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedFilterCount, actualFilterCount.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );
    }

    #endregion
}
