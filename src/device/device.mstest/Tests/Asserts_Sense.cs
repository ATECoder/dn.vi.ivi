using System;

namespace cc.isr.VI.Device.Tests;

public sealed partial class Asserts
{
    #region " sense subsystem "

    /// <summary>   Assert initial sense subsystem values should match. </summary>
    /// <remarks>   2025-01-17. </remarks>
    /// <param name="subsystem">            The subsystem. </param>
    /// <param name="subsystemsInfo">       Information describing the subsystems. </param>
    /// <param name="systemSubsystemInfo">  Information describing the system subsystem. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( SenseSubsystemBase subsystem, VI.Settings.SenseSubsystemSettings? subsystemsInfo,
        VI.Settings.SystemSubsystemSettings systemSubsystemInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( subsystemsInfo, $"{nameof( subsystemsInfo )} should not be null." );
        string? deviceName = subsystem.ResourceNameCaption;
        string propertyName = $"{typeof( SenseSubsystemBase )}.{nameof( SenseSubsystemBase.PowerLineCycles )}";
        double expectedPowerLineCycles = subsystemsInfo.InitialPowerLineCycles;
        double actualPowerLineCycles = subsystem.QueryPowerLineCycles().GetValueOrDefault( 0d );
        Assert.AreEqual( expectedPowerLineCycles, actualPowerLineCycles, systemSubsystemInfo.LineFrequency / TimeSpan.TicksPerSecond, $"[{deviceName}].[{propertyName}] should equal expected value" );
        propertyName = $"{typeof( SenseSubsystemBase )}.{nameof( SenseSubsystemBase.AutoRangeEnabled )}";
        bool expectedBoolean = subsystemsInfo.InitialAutoRangeEnabled;
        bool actualBoolean = subsystem.QueryAutoRangeEnabled().GetValueOrDefault( false );
        Assert.AreEqual( expectedBoolean, actualBoolean, $"[{deviceName}].[{propertyName}] should equal expected value" );
        propertyName = $"{typeof( SenseSubsystemBase )}.{nameof( SenseSubsystemBase.FunctionMode )}";
        SenseFunctionModes actualFunctionMode = subsystem.QueryFunctionMode().GetValueOrDefault( SenseFunctionModes.Resistance );
        SenseFunctionModes expectedFunctionMode = subsystemsInfo.InitialSenseFunction;
        Assert.AreEqual( expectedFunctionMode, actualFunctionMode, $"[{deviceName}].[{propertyName}] should equal expected value" );
    }

    /// <summary> Assert sense subsystem function mode should toggle. </summary>
    /// <remarks> David, 2020-07-29. </remarks>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="toggleFirst">  The toggle first. </param>
    /// <param name="toggleSecond"> The toggle second. </param>
    public static void AssertFunctionModeShouldToggle( SenseSubsystemBase subsystem, SenseFunctionModes toggleFirst, SenseFunctionModes toggleSecond )
    {
        SenseFunctionModes initialFunctionMode = subsystem.QueryFunctionMode().GetValueOrDefault( SenseFunctionModes.None );
        SenseFunctionModes expectedFunctionMode = initialFunctionMode == toggleFirst ? toggleSecond : toggleFirst;
        SenseFunctionModes actualFunctionMode = subsystem.ApplyFunctionMode( expectedFunctionMode ).GetValueOrDefault( SenseFunctionModes.None );
        Assert.AreEqual( expectedFunctionMode, actualFunctionMode, $"{typeof( SenseSubsystemBase )}.{nameof( SenseSubsystemBase.FunctionMode )} should be toggled" );

        // restore function mode
        actualFunctionMode = subsystem.ApplyFunctionMode( initialFunctionMode ).GetValueOrDefault( SenseFunctionModes.None );
        Assert.AreEqual( initialFunctionMode, actualFunctionMode, $"{typeof( SenseSubsystemBase )}.{nameof( SenseSubsystemBase.FunctionMode )} should be restored" );
    }

    #endregion
}
