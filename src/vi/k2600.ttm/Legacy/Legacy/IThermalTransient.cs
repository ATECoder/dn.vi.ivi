namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines an interface for measured thermal transient voltage resistance. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>  
/// David, 2013-12-12, 3.0.3053.x. </para>
/// </remarks>

public interface IThermalTransient : ICloneable
{
    /// <summary> Initializes known state. </summary>
    /// <remarks> This erases the last reading. </remarks>
    void InitializeKnownState();

    /// <summary> Gets or sets the high limit for determining measurement pass fail condition. </summary>
    /// <value> The high limit. </value>
    float HighLimit { get; set; }

    /// <summary> Gets or sets the low limit for determining measurement pass fail condition. </summary>
    /// <value> The low limit. </value>
    float LowLimit { get; set; }

    /// <summary> Parses the reading and sets the voltage and outcome. </summary>
    /// <param name="voltageReading"> Specifies the voltage reading. </param>
    void ParseReading( string voltageReading );

    /// <summary> Gets or sets the reading. </summary>
    /// <value> The reading. </value>
    string Reading { get; }

    /// <summary> Gets or sets the voltage change. </summary>
    /// <value> The voltage. </value>
    float? Voltage { get; }

    /// <summary>   Gets or sets the Okay reading. </summary>
    /// <value> The Okay reading. </value>
    string OkayReading { get; set; }

    /// <summary>   Gets or sets the status reading. </summary>
    /// <value> The status reading. </value>
    string StatusReading { get; set; }

    /// <summary>   Gets or sets the outcome reading. </summary>
    /// <value> The outcome reading. </value>
    string OutcomeReading { get; set; }

    /// <summary>   Gets or sets the firmware name of the entity. </summary>
    /// <value> The the firmware name of the . </value>
    string EntityName { get; set; }
}
