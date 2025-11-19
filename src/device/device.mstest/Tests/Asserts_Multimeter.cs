// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Device.Tests;

public sealed partial class Asserts
{
    #region " multimeter subsystem "

    /// <summary>   Assert initial subsystem values should match. </summary>
    /// <remarks>   2025-01-17. </remarks>
    /// <param name="subsystem">            The subsystem. </param>
    /// <param name="senseSubsystemsInfo">  Information describing the sense subsystems. </param>
    /// <param name="systemSubsystemsInfo"> Information describing the system subsystems. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( MultimeterSubsystemBase subsystem, VI.Settings.SenseSubsystemSettings? senseSubsystemsInfo, VI.Settings.SystemSubsystemSettings? systemSubsystemsInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( senseSubsystemsInfo, $"{nameof( senseSubsystemsInfo )} should not be null." );
        Assert.IsNotNull( systemSubsystemsInfo, $"{nameof( systemSubsystemsInfo )} should not be null." );
        string propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.PowerLineCycles )}";
        double expectedPowerLineCycles = senseSubsystemsInfo.InitialPowerLineCycles;
        double? actualPowerLineCycles = subsystem.QueryPowerLineCycles().GetValueOrDefault( 0d );
        Assert.IsTrue( actualPowerLineCycles.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );

        double epsilon = systemSubsystemsInfo.LineFrequency / TimeSpan.TicksPerSecond;
        Assert.AreEqual( expectedPowerLineCycles, actualPowerLineCycles.Value, epsilon, $"{subsystem.ResourceNameCaption} {propertyName} value should match expect value withing {epsilon}" );
        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.AutoRangeEnabled )}";
        bool expectedBoolean = senseSubsystemsInfo.InitialAutoRangeEnabled;
        bool? actualBoolean = subsystem.QueryAutoRangeEnabled().GetValueOrDefault( false );
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedBoolean, actualBoolean.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.AutoZeroEnabled )}";
        expectedBoolean = senseSubsystemsInfo.InitialAutoZeroEnabled;
        actualBoolean = subsystem.QueryAutoZeroEnabled().GetValueOrDefault( false );
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedBoolean, actualBoolean.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.FunctionMode )}";
        MultimeterFunctionModes expectedFunctionMode = senseSubsystemsInfo.InitialMultimeterFunction;
        MultimeterFunctionModes? actualFunctionMode = subsystem.QueryFunctionMode();
        Assert.IsTrue( actualFunctionMode.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedFunctionMode, ( object ) actualFunctionMode, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.FilterEnabled )}";
        expectedBoolean = senseSubsystemsInfo.InitialFilterEnabled;
        actualBoolean = subsystem.QueryFilterEnabled();
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedBoolean, actualBoolean.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {propertyName}reading {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.MovingAverageFilterEnabled )}";
        expectedBoolean = senseSubsystemsInfo.InitialMovingAverageFilterEnabled;
        actualBoolean = subsystem.QueryMovingAverageFilterEnabled();
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedBoolean, actualBoolean.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.FilterWindow )}";
        double expectedFilterWindow = senseSubsystemsInfo.InitialFilterWindow;
        double? actualFilterWindow = subsystem.QueryFilterWindow();
        Assert.IsNotNull( actualFilterWindow );
        Assert.IsTrue( actualBoolean.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedFilterWindow, actualFilterWindow.Value, 0.1d * expectedFilterWindow, $"{subsystem.ResourceNameCaption} {propertyName} should match withing {epsilon}" );

        propertyName = $"initial {typeof( MultimeterSubsystemBase )}.{nameof( MultimeterSubsystemBase.FilterCount )}";
        int expectedFilterCount = senseSubsystemsInfo.InitialFilterCount;
        int? actualFilterCount = subsystem.QueryFilterCount();
        Assert.IsTrue( actualFilterCount.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedFilterCount, actualFilterCount.Value, $"{subsystem.ResourceNameCaption} {propertyName} should match" );
    }

    #endregion
}
