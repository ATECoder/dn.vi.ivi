namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines an interface for measured cold resistance. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>  
/// David, 2009-02-02, 2.1.3320.x. </para>
/// </remarks>
public interface IColdResistance : ICloneable
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

    /// <summary> Parses the reading and sets the resistance. </summary>
    /// <param name="resistanceReading"> Specifies the reading as received from the instrument. </param>
    public void ParseReading( string resistanceReading );

    /// <summary> Gets or sets the reading.  When set, the value is converted to resistance. </summary>
    /// <value> The reading. </value>
    public string Reading { get; }

    /// <summary> Gets or sets the measured resistance. </summary>
    /// <value> The resistance. </value>
    public float? Resistance { get; }

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
    /// Reads resistance, outcome, status and okay attributes of the cold resistance entity. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public void Read( Pith.SessionBase session );

    /// <summary>   Reads the limits. </summary>
    /// <remarks>   2025-02-13. </remarks>
    /// <param name="session">  The session. </param>
    public void ReadLimits( Pith.SessionBase session );
}
