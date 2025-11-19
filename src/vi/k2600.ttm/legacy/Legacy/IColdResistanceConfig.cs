// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines an interface for configuring the measurement of cold resistance. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2009-02-02, 2.1.3320.x. </para>
/// </remarks>
public interface IColdResistanceConfig : ICloneable
{

    /// <summary> Gets or sets the integration period in number of power line cycles for measuring the
    /// cold resistance. </summary>
    /// <value> The aperture. </value>
    public float Aperture { get; set; }

    /// <summary> Gets or sets the current level to apply to the device for measuring the cold
    /// resistance. </summary>
    /// <value> The current level. </value>
    public float CurrentLevel { get; set; }

    /// <summary> Gets or sets the high limit for determining measurement pass fail condition. </summary>
    /// <value> The high limit. </value>
    public float HighLimit { get; set; }

    /// <summary> Gets or sets the low limit for determining measurement pass fail condition. </summary>
    /// <value> The low limit. </value>
    public float LowLimit { get; set; }

    /// <summary> Restores known state. </summary>
    public void ResetKnownState();

    /// <summary> Gets or sets the voltage limit. </summary>
    /// <value> The voltage limit. </value>
    public float VoltageLimit { get; set; }

}
