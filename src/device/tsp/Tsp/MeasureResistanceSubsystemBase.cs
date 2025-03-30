namespace cc.isr.VI.Tsp;
/// <summary>
/// Defines the contract that must be implemented by a Measure Resistance Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="MeasureResistanceSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
/// Subsystem</see>. </param>
public class MeasureResistanceSubsystemBase( Tsp.StatusSubsystemBase statusSubsystem ) : SourceMeasureUnitBase( statusSubsystem )
{
    #region " reading "

    /// <summary> The reading. </summary>
    private string? _reading;

    /// <summary>
    /// Gets or sets or sets (protected) the reading.  When set, the value is converted to
    /// resistance.
    /// </summary>
    /// <value> The reading. </value>
    public string? Reading
    {
        get => this._reading;
        protected set => _ = this.SetProperty( ref this._reading, value );
    }

    #endregion

    #region " resistance "

    /// <summary> The resistance. </summary>
    private double? _resistance;

    /// <summary> Gets or sets (protected) the measured resistance. </summary>
    /// <value> The resistance. </value>
    public double? Resistance
    {
        get => this._resistance;
        protected set => _ = this.SetProperty( ref this._resistance, value );
    }

    /// <summary> Measures and reads the resistance. </summary>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    /// type. </exception>
    public void Measure()
    {
        string printFormat = "%8.5f";
        _ = this.Session.WriteLine( "{0}.source.output = {0}.OUTPUT_ON waitcomplete() print(string.format('{1}',{0}.measure.r())) ", this.SourceMeasureUnitReference, printFormat );
        this.Reading = this.Session.ReadLine();
        if ( string.IsNullOrWhiteSpace( this.Reading ) )
        {
            this.Resistance = new double?();
        }
        else if ( double.TryParse( this.Reading, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                                   System.Globalization.CultureInfo.InvariantCulture, out double value ) )
        {
            this.Resistance = value;
        }
        else
        {
            this.Resistance = new double?();
            throw new InvalidCastException( string.Format( System.Globalization.CultureInfo.InvariantCulture, "Failed parsing {0} to number reading '{1}'", this.Reading, this.Session.LastMessageSent ) );
        }
    }

    #endregion
}
