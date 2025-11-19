// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.Device.Tests;

public sealed partial class Asserts
{
    #region " trigger subsystem "

    /// <summary>   Assert initial subsystem values should match. </summary>
    /// <remarks>   2025-01-17. </remarks>
    /// <param name="subsystem">                The subsystem. </param>
    /// <param name="triggerSubsystemsInfo">    Information describing the trigger subsystems. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( TriggerSubsystemBase subsystem, VI.Settings.TriggerSubsystemSettings? triggerSubsystemsInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( triggerSubsystemsInfo, $"{nameof( triggerSubsystemsInfo )} should not be null." );
        string propertyName = nameof( TriggerSubsystemBase.TriggerSource ).SplitWords();
        TriggerSources expectedTriggerSource = triggerSubsystemsInfo.InitialTriggerSource;
        TriggerSources? actualTriggerSource = subsystem.QueryTriggerSource();
        Assert.IsTrue( actualTriggerSource.HasValue, $"{subsystem.ResourceNameCaption} {propertyName} should have a value" );
        Assert.AreEqual( expectedTriggerSource, actualTriggerSource.Value, $"{subsystem.ResourceNameCaption} initial {propertyName} should be expected" );
    }

    #endregion
}
