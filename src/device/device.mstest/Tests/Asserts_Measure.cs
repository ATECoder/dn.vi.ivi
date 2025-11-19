// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Device.Tests;

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
