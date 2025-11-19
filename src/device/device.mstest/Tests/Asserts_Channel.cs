// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

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
