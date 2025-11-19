// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary>   Interface for information report. </summary>
/// <remarks>   David, 2021-04-12. </remarks>
public interface IInfoReport
{
    /// <summary>   Builds information report. </summary>
    /// <returns>   A <see cref="string" />. </returns>
    public string BuildInfoReport();
}
