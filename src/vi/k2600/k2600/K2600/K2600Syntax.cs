// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600;
/// <summary>   A 2600 syntax. </summary>
/// <remarks>   2024-11-14. </remarks>
public static class K2600Syntax
{
    /// <summary>   Gets or sets the default source meter name. </summary>
    /// <value> The default source meter name. </value>
    public static string DefaultSourceMeterName { get; set; } = "smua";

    /// <summary>   Gets or sets a list of names of the source measure units. </summary>
    /// <value> A list of names of the source measure units. </value>
    public static string SourceMeasureUnitNames { get; set; } = "smua,smub";
}
