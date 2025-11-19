// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary>   An elapsed time span. </summary>
/// <remarks>   David, 2021-04-10. </remarks>
/// <remarks>   Constructor. </remarks>
/// <remarks>   David, 2021-04-10. </remarks>
/// <param name="identity"> The identity. </param>
/// <param name="elapsed">  The elapsed. </param>
public struct ElapsedTimeSpan( ElapsedTimeIdentity identity, TimeSpan elapsed )
{
    /// <summary>   Gets or sets the identity. </summary>
    /// <value> The identity. </value>
    public ElapsedTimeIdentity Identity { get; set; } = identity;

    /// <summary>   Gets or sets the elapsed. </summary>
    /// <value> The elapsed. </value>
    public TimeSpan Elapsed { get; set; } = elapsed;
}
/// <summary>   Values that represent elapsed time identities. </summary>
/// <remarks>   David, 2021-04-10. </remarks>
public enum ElapsedTimeIdentity
{
    /// <summary>   An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None,

    /// <summary>   An enum constant representing the write time option. </summary>
    [System.ComponentModel.Description( "WriteEnum Time" )]
    WriteTime,

    /// <summary>   An enum constant representing the write time option. </summary>
    [System.ComponentModel.Description( "Read Time" )]
    ReadTime,

    /// <summary>   An enum constant representing the query time option. </summary>
    [System.ComponentModel.Description( "Query Time" )]
    QueryTime,

    /// <summary>   An enum constant representing time to determine that the instrument is ready to receive a write command. </summary>
    [System.ComponentModel.Description( "WriteEnum Ready Delay" )]
    WriteReadyDelay,

    /// <summary>   An enum constant representing time to determine that the instrument is ready receive a write command. </summary>
    [System.ComponentModel.Description( "Read Ready Delay" )]
    ReadReadyDelay,

    /// <summary>   An enum constant representing the read delay option. </summary>
    [System.ComponentModel.Description( "Read Delay" )]
    ReadDelay,

    /// <summary>   An enum constant representing the total write time option. </summary>
    [System.ComponentModel.Description( "Total WriteEnum Time" )]
    TotalWriteTime,

}
