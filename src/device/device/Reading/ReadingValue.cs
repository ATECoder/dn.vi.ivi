namespace cc.isr.VI;

/// <summary> Implements a reading value. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-01 </para>
/// </remarks>
public class ReadingValue : ReadingEntity
{
    #region " construction and cleanup "

    /// <summary>
    /// Constructs a measured value without specifying the value or its validity, which must be
    /// specified for the value to be made valid.
    /// </summary>
    /// <param name="readingType"> Type of the reading. </param>
    public ReadingValue( ReadingElementTypes readingType ) : base( readingType ) => this.Generator = new RandomNumberGenerator();

    /// <summary> Constructs a copy of an existing value. </summary>
    /// <param name="model"> The model. </param>
    public ReadingValue( ReadingValue model ) : base( model )
    {
        if ( model is not null )
            this.Value = model.Value;
        this.Generator = new RandomNumberGenerator();
    }

    #endregion

    #region " equals "

    /// <summary> = casting operator. </summary>
    /// <param name="left">  The left hand side item to compare for equality. </param>
    /// <param name="right"> The left hand side item to compare for equality. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( ReadingValue left, ReadingValue right )
    {
        return left is null ? right is null : right is not null && Equals( left, right );
    }

    /// <summary> casting operator. </summary>
    /// <param name="left">  The left hand side item to compare for equality. </param>
    /// <param name="right"> The left hand side item to compare for equality. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( ReadingValue left, ReadingValue right )
    {
        return !Equals( left, right );
    }

    /// <summary> Returns True if equal. </summary>
    /// <remarks>
    /// Ranges are the same if the have the same
    /// <see cref="Type">min</see> and <see cref="Type">max</see> values.
    /// </remarks>
    /// <param name="left">  The left hand side item to compare for equality. </param>
    /// <param name="right"> The left hand side item to compare for equality. </param>
    /// <returns> <c>true</c> if equals. </returns>
    public static bool Equals( ReadingValue left, ReadingValue right )
    {
        return left is null
            ? right is null
            : right is not null
                && Nullable.Equals( left.Value, right.Value )
                && string.Equals( left.RawValueReading, right.RawValueReading )
                && string.Equals( left.RawUnitsReading, right.RawUnitsReading, StringComparison.Ordinal );
    }

    /// <summary>
    /// Determines whether the specified <see cref="object" /> is equal to the current
    /// <see cref="object" />.
    /// </summary>
    /// <param name="obj"> The <see cref="object" /> to compare with the current
    /// <see cref="object" />. </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="object" /> is equal to the current
    /// <see cref="object" />; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals( object obj )
    {
        return this.Equals( ( ReadingValue ) obj );
    }

    /// <summary>
    /// Returns True if the value of the <paramref name="other"/> equals to the instance value.
    /// </summary>
    /// <remarks>
    /// Ranges are the same if the have the same
    /// <see cref="Type">min</see> and <see cref="Type">max</see> values.
    /// </remarks>
    /// <param name="other"> The other <see cref="ReadingValue">Range</see> to compare for equality
    /// with this instance. </param>
    /// <returns> A Boolean data type. </returns>
    public bool Equals( ReadingValue other )
    {
        return other is not null && Equals( this, other );
    }

    /// <summary> Creates a unique hash code. </summary>
    /// <returns> An <see cref="int">integer</see> value. </returns>
    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }

    #endregion

    #region " value "

    /// <summary> Gets the value. </summary>
    /// <value> The value. </value>
    public double? Value { get; set; }

    /// <summary> Resets value to nothing. </summary>
    public override void Reset()
    {
        base.Reset();
        this.Value = new double?();
    }

    /// <summary>
    /// Applies the reading to create the specific reading type in the inherited class.
    /// </summary>
    /// <param name="rawValueReading"> The raw value reading. </param>
    /// <param name="rawUnitsReading"> The raw units reading. </param>
    /// <returns> <c>true</c> if parsed. </returns>
    public override bool TryApplyReading( string rawValueReading, string rawUnitsReading )
    {
        if ( base.TryApplyReading( rawValueReading, rawUnitsReading ) )
        {
            // convert reading to numeric
            return this.TryApplyReading( rawValueReading );
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Applies the reading to create the specific reading type in the inherited class.
    /// </summary>
    /// <param name="valueReading"> The value reading. </param>
    /// <returns> <c>true</c> if parsed. </returns>
    public override bool TryApplyReading( string valueReading )
    {
        if ( base.TryApplyReading( valueReading ) )
        {
            // convert reading to numeric
            if ( double.TryParse( valueReading, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent, System.Globalization.CultureInfo.CurrentCulture, out double value ) )
            {
                this.Value = value;
                return true;
            }
            else if ( double.TryParse( valueReading, out value ) )
            {
                this.Value = value;
                return true;
            }
            else
            {
                this.Value = VI.Syntax.ScpiSyntax.NotANumber;
                return false;
            }
        }
        else
        {
            this.Value = new double?();
            return false;
        }
    }

    #endregion

    #region " to string "

    /// <summary> Returns a string that represents the current object. </summary>
    /// <returns> A <see cref="string" /> that represents the current object. </returns>
    public override string ToString()
    {
        return this.Value.HasValue ? this.Value.Value.ToString() : this.RawValueReading ?? string.Empty;
    }

    #endregion

    #region " simulation "

    /// <summary> Gets the generator. </summary>
    /// <value> The generator. </value>
    public RandomNumberGenerator Generator { get; private set; }

    /// <summary> Holds the simulated value. </summary>
    /// <value> The simulated value. </value>
    public double SimulatedValue => this.Generator.Value;

    #endregion
}
