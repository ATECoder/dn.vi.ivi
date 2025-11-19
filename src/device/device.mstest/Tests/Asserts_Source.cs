// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Device.Tests;

public sealed partial class Asserts
{
    #region " source subsystem "

    /// <summary>   Assert initial subsystem values should match. </summary>
    /// <remarks>   2025-01-17. </remarks>
    /// <param name="subsystem">            The subsystem. </param>
    /// <param name="sourceSubsystemInfo">  Information describing the source subsystem. </param>
    public static void AssertSubsystemInitialValuesShouldMatch( SourceSubsystemBase subsystem, VI.Settings.SourceSubsystemSettings? sourceSubsystemInfo )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( sourceSubsystemInfo, $"{nameof( sourceSubsystemInfo )} should not be null." );
    }

    /// <summary> Assert output enabled should toggle. </summary>
    /// <param name="subsystem">     The subsystem. </param>
    /// <param name="outputEnabled"> True to enable, false to disable the output. </param>
    public static void AssertOutputEnabledShouldToggle( SourceSubsystemBase subsystem, bool outputEnabled )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        bool expectedOutputEnabled = outputEnabled;
        bool actualOutputEnabled = subsystem.ApplyOutputEnabled( expectedOutputEnabled ).GetValueOrDefault( !expectedOutputEnabled );
        Assert.AreEqual( expectedOutputEnabled, actualOutputEnabled, $"{typeof( SourceSubsystemBase )}.{nameof( SourceSubsystemBase.OutputEnabled )} is {actualOutputEnabled}; expected {expectedOutputEnabled}" );
    }

    #endregion
}
