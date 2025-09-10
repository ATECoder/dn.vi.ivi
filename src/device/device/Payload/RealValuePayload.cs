namespace cc.isr.VI;
/// <summary>
/// A payload consisting of a single real value of <see cref="double"/> type.
/// </summary>
public class RealValuePayload : Pith.PayloadBase
{
    #region " construction and cleanup "

    /// <summary> Constructor for this class. </summary>
    public RealValuePayload() : base()
    {
        this.Range = Std.Primitives.RangeR.Full;
        this.PositiveOverflow = VI.Syntax.ScpiSyntax.Infinity;
        this.NegativeOverflow = VI.Syntax.ScpiSyntax.NegativeInfinity;
        this.Format = "0.00";
        this.Unit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.Amount = new UnitsAmounts.Amount( 0, this.Unit );
    }

    #endregion

    #region " simulate "

    /// <summary> Returns an emulated value. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="range"> The range. </param>
    /// <returns> A Double. </returns>
    public static double SimulateValue( Std.Primitives.RangeR range )
    {
        if ( range is null ) throw new ArgumentNullException( nameof( range ) );
        Random rand = new( DateTime.Now.Second );
        return range.Min + (range.Span * rand.NextDouble());
    }

    /// <summary> Gets the simulated value. </summary>
    /// <value> The simulated value. </value>
    public double SimulatedValue => SimulateValue( this.Range );

    /// <summary> Gets the simulated payload. </summary>
    /// <value> The simulated payload. </value>
    public override string SimulatedPayload => string.Format( this.Format, SimulateValue( this.Range ) );

    #endregion

    #region " range "

    /// <summary> Gets or sets the range. </summary>
    /// <value> The range. </value>
    public Std.Primitives.RangeR Range { get; set; }

    /// <summary> Gets or sets the positive overflow. </summary>
    /// <value> The positive overflow. </value>
    public double PositiveOverflow { get; set; }

    /// <summary> Gets or sets the negative overflow. </summary>
    /// <value> The negative overflow. </value>
    public double NegativeOverflow { get; set; }

    #endregion

    #region " query "

    /// <summary> Gets or sets the real value unit. </summary>
    /// <value> The real value. </value>
    public cc.isr.UnitsAmounts.Unit Unit { get; set; }

    /// <summary> Gets or sets the analog input read. </summary>
    /// <value> The analog input read. </value>
    public cc.isr.UnitsAmounts.Amount Amount { get; set; }

    /// <summary> Gets or sets the value. </summary>
    /// <value> The real value. </value>
    public double RealValue
    {
        get;
        set
        {
            if ( value != this.RealValue )
            {
                field = value;
                this.Amount = new cc.isr.UnitsAmounts.Amount( value, this.Unit );
            }
        }
    }

    /// <summary> Gets the real value caption. </summary>
    /// <value> The real value caption. </value>
    public string RealValueCaption => this.Amount.ToString( this.Format );

    /// <summary> Gets or sets the format to use. </summary>
    /// <value> The format. </value>
    public string Format { get; set; }

    /// <summary> Converts the reading to the specific payload such as a read number. </summary>
    /// <param name="reading"> The reading. </param>
    public override void FromReading( string reading )
    {
        base.FromReading( reading );
        bool localTryParse()
        {
            _ = this.RealValue;
            bool ret = double.TryParse( reading, out double parseResult ); this.RealValue = parseResult; return ret;
        }

        this.QueryStatus = localTryParse() ? this.QueryStatus | (Pith.PayloadStatus.QueryParsed & ~Pith.PayloadStatus.QueryParseFailed) : this.QueryStatus | (Pith.PayloadStatus.QueryParseFailed & ~Pith.PayloadStatus.QueryParsed & ~Pith.PayloadStatus.Okay);
    }

    #endregion

    #region " write "

    /// <summary>
    /// Converts the specific payload value to a <see cref="P:isr.VI.PayloadBase.Writing">value</see>
    /// to send to the session.
    /// </summary>
    /// <returns> A <see cref="string" />. </returns>
    public override string FromValue()
    {
        this.Writing = string.Format( this.Format, this.RealValue );
        return this.Writing;
    }

    #endregion
}
