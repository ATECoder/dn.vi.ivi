// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;
/// <summary>
/// Provides a simulated value for testing measurements without having the benefit of instruments.
/// </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2005-01-15, 1.0.1841.x. </para>
/// </remarks>
public class RandomNumberGenerator
{
    /// <summary> Default constructor. </summary>
    public RandomNumberGenerator() : base()
    {
        this.Generator = new Random( ( int ) (DateTimeOffset.Now.Ticks % int.MaxValue) );
        this.Min = 0d;
        this.Max = 1d;
    }

    /// <summary> Holds a shared reference to the number generator. </summary>
    /// <value> The generator. </value>
    private Random Generator { get; set; }

    /// <summary> Gets the range. </summary>
    /// <value> The range. </value>
    private double Range => this.Max - this.Min;

    /// <summary> Gets the minimum. </summary>
    /// <value> The minimum value. </value>
    public double Min { get; set; }

    /// <summary> Gets the maximum. </summary>
    /// <value> The maximum value. </value>
    public double Max { get; set; }

    /// <summary> Returns a simulated value. </summary>
    /// <value> The value. </value>
    public double Value => (this.Generator.NextDouble() * this.Range) + this.Min;
}
