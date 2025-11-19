namespace cc.isr.VI;

/// <summary> A resistance range current. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-04-09 </para>
/// </remarks>
public class ResistanceRangeCurrent
{
    /// <summary> Constructor. </summary>
    /// <param name="range">   The range. </param>
    /// <param name="current"> The current. </param>
    /// <param name="caption"> The caption. </param>
    public ResistanceRangeCurrent( decimal range, decimal current, string caption ) : base()
    {
        this.ResistanceRange = range;
        this.RangeCurrent = current;
        this.Caption = caption;
    }

    /// <summary> Constructor. </summary>
    /// <param name="range">   The range. </param>
    /// <param name="current"> The current. </param>
    public ResistanceRangeCurrent( decimal range, decimal current ) : base()
    {
        this.ResistanceRange = range;
        this.RangeCurrent = current;
        this.Caption = $"{range:0}{cc.isr.UnitsAmounts.StandardUnits.UnitSymbols.Omega} {1000m * current}mA";
    }

    /// <summary> Gets or sets the resistance range. </summary>
    /// <value> The resistance range. </value>
    public decimal ResistanceRange { get; set; }

    /// <summary> Gets or sets the range current. </summary>
    /// <value> The range current. </value>
    public decimal RangeCurrent { get; set; }

    /// <summary> Gets or sets the caption. </summary>
    /// <value> The caption. </value>
    public string Caption { get; set; }

    /// <summary> Convert this object into a string representation. </summary>
    /// <returns> A <see cref="string" /> that represents this object. </returns>
    public new string ToString()
    {
        return this.Caption;
    }
}
/// <summary> Collection of resistance range currents. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-04-09 </para>
/// </remarks>
public class ResistanceRangeCurrentCollection : List<ResistanceRangeCurrent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="System.Collections.Generic.List{T}" /> class
    /// that is empty and has the default initial capacity.
    /// </summary>
    public ResistanceRangeCurrentCollection() : base()
    {
    }

    /// <summary> Validates the given value. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="value"> The value. </param>
    /// <returns> A ResistanceRangeCurrentCollection. </returns>
    public static ResistanceRangeCurrentCollection Validate( ResistanceRangeCurrentCollection value )
    {
        return value is null ? throw new ArgumentNullException( nameof( value ) ) : value;
    }

    /// <summary> Constructor. </summary>
    /// <param name="values"> The values. </param>
    public ResistanceRangeCurrentCollection( ResistanceRangeCurrentCollection values ) : base()
    {
        if ( values?.Any() == true )
        {
            foreach ( ResistanceRangeCurrent v in values )
                this.Add( v );
        }
    }

    /// <summary>
    /// Returns the first resistance range greater or equal to the specified range.
    /// </summary>
    /// <param name="caption"> The caption. </param>
    /// <returns> A ResistanceRangeCurrent. </returns>
    public ResistanceRangeCurrent MatchResistanceRange( string caption )
    {
        // select the first range greater than or equal the specified range
        int i = this.FindIndex( x => string.Compare( x.Caption, caption, false ) >= 0 );
        return this[i >= 0 ? i : 0];
    }

    /// <summary>
    /// Returns the first resistance range greater or equal to the specified range.
    /// </summary>
    /// <param name="range"> The range. </param>
    /// <returns> A ResistanceRangeCurrent. </returns>
    public ResistanceRangeCurrent MatchResistanceRange( decimal range )
    {
        // select the first range greater than or equal the specified range
        int i = this.FindIndex( x => x.ResistanceRange >= range );
        return this[i >= 0 ? i : 0];
    }

    /// <summary>
    /// Returns the first resistance range greater or equal to the specified range.
    /// </summary>
    /// <param name="range">   The range. </param>
    /// <param name="current"> The current. </param>
    /// <returns> A ResistanceRangeCurrent. </returns>
    public ResistanceRangeCurrent MatchResistanceRange( decimal range, decimal current )
    {
        // select the first range greater than or equal the specified range
        int i = this.FindIndex( x => x.ResistanceRange >= range && x.RangeCurrent >= current );
        return this[i >= 0 ? i : 0];
    }
}
