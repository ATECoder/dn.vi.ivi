// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines an interface for measured thermal transient voltage resistance. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-12-12, 3.0.3053.x. </para>
/// </remarks>

[CLSCompliant( false )]
public interface IThermalTransient : ICloneable
{
    /// <summary> Initializes known state. </summary>
    /// <remarks> This erases the last reading. </remarks>
    public void InitializeKnownState();

    /// <summary> Gets or sets the high limit for determining measurement pass fail condition. </summary>
    /// <value> The high limit. </value>
    public float HighLimit { get; set; }

    /// <summary> Gets or sets the low limit for determining measurement pass fail condition. </summary>
    /// <value> The low limit. </value>
    public float LowLimit { get; set; }

    /// <summary> Parses the reading and sets the voltage and outcome. </summary>
    /// <param name="voltageReading"> Specifies the voltage reading. </param>
    public void ParseReading( string voltageReading );

    /// <summary> Gets or sets the reading. </summary>
    /// <value> The reading. </value>
    public string Reading { get; }

    /// <summary> Gets or sets the voltage change. </summary>
    /// <value> The voltage. </value>
    public float? Voltage { get; }

    /// <summary>   Gets or sets the Okay reading. </summary>
    /// <value> The Okay reading. </value>
    public string OkayReading { get; set; }

    /// <summary>   Gets or sets the status reading. </summary>
    /// <value> The status reading. </value>
    public string StatusReading { get; set; }

    /// <summary>   Gets or sets the outcome reading. </summary>
    /// <value> The outcome reading. </value>
    public string OutcomeReading { get; set; }

    /// <summary>   Gets or sets the firmware name of the entity. </summary>
    /// <value> The the firmware name of the . </value>
    public string EntityName { get; set; }

    /// <summary>
    /// Reads voltage change, outcome, status and okay attributes of the thermal transient entity. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public void Read( Pith.SessionBase session );

    /// <summary>   Reads the limits. </summary>
    /// <remarks>   2025-02-13. </remarks>
    /// <param name="session">  The session. </param>
    public void ReadLimits( Pith.SessionBase session );
}
