// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Pith;

/// <summary>   Additional information for service request events. </summary>
/// <remarks>   2024-09-02. </remarks>
/// <param name="statusByte">   The status byte. </param>
public class ServiceRequestEventArgs( ServiceRequests statusByte ) : EventArgs
{
    /// <summary>   Gets or sets the status byte. </summary>
    /// <value> The status byte. </value>
    public ServiceRequests StatusByte { get; internal set; } = statusByte;
}
