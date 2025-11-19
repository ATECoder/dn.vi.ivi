// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines a measured thermal transient resistance and voltage. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2009-02-02, 2.1.3320.x. </para><para>
/// David, 2013-08-05, 3.0.3053.x. </para>
/// </remarks>
public class ThermalTransient : IThermalTransient
{
    /// <summary> Default constructor. </summary>
    public ThermalTransient()
    {
        this.Reading = string.Empty;
        this.StatusReading = string.Empty;
        this.OutcomeReading = string.Empty;
        this.OkayReading = string.Empty;
        this.Voltage = default;
    }

    /// <summary> Clones an existing measurement. </summary>
    /// <param name="value"> The value. </param>
    [CLSCompliant( false )]
    public ThermalTransient( IThermalTransient value ) : this()
    {
        if ( value is not null )
        {
            this.Reading = value.Reading;
            this.HighLimit = value.HighLimit;
            this.LowLimit = value.LowLimit;
            this.Voltage = value.Voltage;
            this.Reading = value.Reading;
            this.StatusReading = value.StatusReading;
            this.OutcomeReading = value.OutcomeReading;
            this.OkayReading = value.OkayReading;
        }
    }

    /// <summary> Clones the measurement. </summary>
    /// <returns> A copy of this object. </returns>
    public object Clone()
    {
        return new ThermalTransient( this );
    }

    /// <summary> Initializes known state. </summary>
    /// <remarks> This erases the last reading. </remarks>
    public void InitializeKnownState()
    {
        this.Reading = string.Empty;
        this.StatusReading = string.Empty;
        this.OutcomeReading = string.Empty;
        this.OkayReading = string.Empty;
        this.Voltage = default;
    }

    /// <summary> Gets the high limit for determining measurement pass fail condition. </summary>
    /// <value> The high limit. </value>
    public float HighLimit { get; set; }

    /// <summary> Gets the low limit for determining measurement pass fail condition. </summary>
    /// <value> The low limit. </value>
    public float LowLimit { get; set; }

    /// <summary> Sets the readings. </summary>
    /// <param name="voltageReading"> Specifies the voltage reading. </param>
    public void ParseReading( string voltageReading )
    {
        this.Reading = voltageReading;
        if ( string.IsNullOrWhiteSpace( voltageReading ) || string.Equals( voltageReading, SessionBase.NilValue ) )
            this.Voltage = default;
        else
        {
            if ( float.TryParse( voltageReading, this._numberFormat, System.Globalization.CultureInfo.InvariantCulture, out float value ) )
                this.Voltage = value;
            else
                this.Voltage = default;
        }
    }

    /// <summary> The number format. </summary>
    private readonly System.Globalization.NumberStyles _numberFormat = System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent;

    /// <summary> Gets the reading.  When set, the value is converted to voltage. </summary>
    /// <value> The reading. </value>
    public string Reading { get; private set; } = string.Empty;

    /// <summary> Gets the measured transient voltage. </summary>
    /// <value> The voltage. </value>
    public float? Voltage { get; set; } = default;

    /// <summary>   Gets or sets the firmware name of the entity. </summary>
    /// <value> The the firmware name of the . </value>
    public string EntityName { get; set; } = "_G.ttm.tr";

    /// <summary>   Gets or sets the Okay reading. </summary>
    /// <value> The Okay reading. </value>
    public string OkayReading { get; set; } = string.Empty;

    /// <summary>   Gets or sets the status reading. </summary>
    /// <value> The status reading. </value>
    public string StatusReading { get; set; } = string.Empty;

    /// <summary>   Gets or sets the outcome reading. </summary>
    /// <value> The outcome reading. </value>
    public string OutcomeReading { get; set; } = string.Empty;

    /// <summary>
    /// Reads voltage change, outcome, status and okay attributes of the thermal transient entity. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    [CLSCompliant( false )]
    public void Read( Pith.SessionBase session )
    {
        this.Reading = session.QueryPrintTrimEnd( $"{this.EntityName}.voltageChange" ) ?? string.Empty;
        this.StatusReading = session.QueryPrintTrimEnd( $"{this.EntityName}.status" ) ?? string.Empty;
        this.OutcomeReading = session.QueryPrintTrimEnd( $"{this.EntityName}.outcome" ) ?? string.Empty;
        this.OkayReading = session.QueryPrintTrimEnd( $"{this.EntityName}:isOkay() " ) ?? string.Empty;
    }

    /// <summary>   Reads the limits. </summary>
    /// <remarks>   2025-02-13. </remarks>
    /// <param name="session">  The session. </param>
    [CLSCompliant( false )]
    public void ReadLimits( Pith.SessionBase session )
    {
        this.LowLimit = ( float ) session.QueryNullableDoubleThrowIfError( $"print({this.EntityName}.lowLimit) ",
            "Voltage Change Low Limit" ).GetValueOrDefault( 0 );
        this.HighLimit = ( float ) session.QueryNullableDoubleThrowIfError( $"print({this.EntityName}.highLimit) ",
            "Voltage Change High Limit" ).GetValueOrDefault( 0 );
    }


}
