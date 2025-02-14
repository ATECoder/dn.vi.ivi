using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines a measured cold resistance element. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>  
/// David, 2009-02-02, 2.1.3320.x. </para>
/// </remarks>
/// <remarks>   Default constructor. </remarks>
/// <remarks>   2024-11-07. </remarks>
/// <param name="entityName">   The the firmware name of the entity, e.g., '_G.ttm.ir' . </param>
public class ColdResistance( string entityName ) : object(), IColdResistance
{

    /// <summary> Clones an existing measurement. </summary>
    /// <param name="value"> The value. </param>
    public ColdResistance( IColdResistance value ) : this( value.EntityName )
    {
        if ( value is not null )
        {
            this.Resistance = value.Resistance;
            this.HighLimit = value.HighLimit;
            this.LowLimit = value.LowLimit;
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
        return new ColdResistance( this );
    }

    /// <summary> Initializes known state. </summary>
    public void InitializeKnownState()
    {
        this.StatusReading = string.Empty;
        this.OutcomeReading = string.Empty;
        this.OkayReading = string.Empty;
        this.Reading = string.Empty;
        this.Resistance = default;
    }

    /// <summary> Gets the high limit for determining measurement pass fail condition. </summary>
    /// <value> The high limit. </value>
    public float HighLimit { get; set; }

    /// <summary> Gets the low limit for determining measurement pass fail condition. </summary>
    /// <value> The low limit. </value>
    public float LowLimit { get; set; }

    /// <summary> Sets the reading. </summary>
    /// <param name="resistanceReading"> Specifies the reading as received from the instrument. </param>
    public void ParseReading( string resistanceReading )
    {
        this.Reading = resistanceReading;
        if ( string.IsNullOrWhiteSpace( resistanceReading ) || string.Equals( resistanceReading, SessionBase.NilValue ) )
            this.Resistance = default;
        else
        {
            if ( float.TryParse( this.Reading, this._numberFormat, System.Globalization.CultureInfo.InvariantCulture, out float value ) )
                this.Resistance = value;
            else
                this.Resistance = default;
        }
    }

    private readonly System.Globalization.NumberStyles _numberFormat = System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent;

    /// <summary> Gets the reading.  When set, the value is converted to resistance. </summary>
    /// <value> The reading. </value>
    public string Reading { get; private set; } = string.Empty;

    /// <summary> Gets the measured resistance. </summary>
    /// <value> The resistance. </value>
    public float? Resistance { get; private set; } = default;

    /// <summary>   Gets or sets the Okay reading. </summary>
    /// <value> The Okay reading. </value>
    public string OkayReading { get; set; } = string.Empty;

    /// <summary>   Gets or sets the status reading. </summary>
    /// <value> The status reading. </value>
    public string StatusReading { get; set; } = string.Empty;

    /// <summary>   Gets or sets the outcome reading. </summary>
    /// <value> The outcome reading. </value>
    public string OutcomeReading { get; set; } = string.Empty;

    /// <summary>   Gets or sets the firmware name of the entity. </summary>
    /// <value> The the firmware name of the . </value>
    public string EntityName { get; set; } = entityName;

    /// <summary>
    /// Reads resistance, outcome, status and okay attributes of the cold resistance entity. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public void Read( Pith.SessionBase session )
    {
        this.Reading = session.QueryPrintTrimEnd( $"{this.EntityName}.resistance" ) ?? string.Empty;
        this.StatusReading = session.QueryPrintTrimEnd( $"{this.EntityName}.status" ) ?? string.Empty;
        this.OutcomeReading = session.QueryPrintTrimEnd( $"{this.EntityName}.outcome" ) ?? string.Empty;
        this.OkayReading = session.QueryPrintTrimEnd( $"{this.EntityName}:isOkay() " ) ?? string.Empty;
    }

    /// <summary>   Reads the limits. </summary>
    /// <remarks>   2025-02-13. </remarks>
    /// <param name="session">  The session. </param>
    public void ReadLimits( Pith.SessionBase session )
    {
        this.LowLimit = ( float ) session.QueryNullableDoubleThrowIfError( $"print({this.EntityName}.lowLimit) ",
            "Cold Resistance Low Limit" ).GetValueOrDefault( 0 );
        this.HighLimit = ( float ) session.QueryNullableDoubleThrowIfError( $"print({this.EntityName}.highLimit) ",
            "Cold Resistance High Limit" ).GetValueOrDefault( 0 );
    }
}
