namespace cc.isr.VI;

/// <summary> Implements a reading element. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-01 </para>
/// </remarks>
/// <remarks>
/// Constructs a measured value without specifying the value or its validity, which must be
/// specified for the value to be made valid.
/// </remarks>
/// <param name="readingType"> The type of the reading. </param>
public class ReadingEntity( ReadingElementTypes readingType ) : object()
{
    #region " construction and cleanup "

    /// <summary> Constructs a copy of an existing value. </summary>
    /// <param name="model"> The model. </param>
    public ReadingEntity( ReadingEntity model ) : this( ReadingElementTypes.None )
    {
        if ( model is not null )
        {
            this.ReadingType = model.ReadingType;
            this.Heading = model.Heading;
            this.RawValueReading = model.RawValueReading;
            this.IncludesUnitsSuffix = model.IncludesUnitsSuffix;
        }
    }

    #endregion

    #region " static "

    /// <summary>
    /// Remove unit characters from SCPI data. Some instruments append units to the end of the
    /// fetched values. This methods removes alpha characters as well as the number sign which the
    /// instruments append to the reading number.
    /// </summary>
    /// <param name="value"> A delimited string of values. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string TrimUnits( string value )
    {
        return TrimUnits( value, "," );
    }

    /// <summary>
    /// Remove unit characters from SCPI data. Some instruments append units to the end of the
    /// fetched values. This methods removes alpha characters as well as the number sign which
    /// instruments append to the reading number.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="value">     A delimited string of values. </param>
    /// <param name="delimiter"> The delimiter. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string TrimUnits( string value, string delimiter )
    {
        const string unitCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#";
        if ( string.IsNullOrWhiteSpace( delimiter ) ) throw new ArgumentNullException( nameof( delimiter ) );
        System.Text.StringBuilder dataBuilder = new();
        if ( !string.IsNullOrWhiteSpace( value ) )
        {
            if ( value.Contains( delimiter ) )
            {
                foreach ( string dataElement in value.Split( delimiter.ToCharArray() ) )
                {
                    if ( dataBuilder.Length > 0 )
                        _ = dataBuilder.Append( delimiter );
                    _ = dataBuilder.Append( dataElement.TrimEnd( unitCharacters.ToCharArray() ) );
                }
            }
            else
            {
                _ = dataBuilder.Append( value.TrimEnd( unitCharacters.ToCharArray() ) );
            }
        }

        return dataBuilder.ToString();
    }

    #endregion

    #region " equals "

    /// <summary> = casting operator. </summary>
    /// <param name="left">  The left hand side item to compare for equality. </param>
    /// <param name="right"> The left hand side item to compare for equality. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( ReadingEntity left, ReadingEntity right )
    {
        return left is null ? right is null : right is not null && Equals( left, right );
    }

    /// <summary> casting operator. </summary>
    /// <param name="left">  The left hand side item to compare for equality. </param>
    /// <param name="right"> The left hand side item to compare for equality. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( ReadingEntity left, ReadingEntity right )
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
    public static bool Equals( ReadingEntity left, ReadingEntity right )
    {
        return left is null
            ? right is null
            : right is not null
&& string.Equals( left.RawValueReading, right.RawValueReading, StringComparison.Ordinal ) && string.Equals( left.RawUnitsReading, right.RawUnitsReading, StringComparison.Ordinal );
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
        return this.Equals( ( ReadingEntity ) obj );
    }

    /// <summary>
    /// Returns True if the value of the <paramref name="other"/> equals to the instance value.
    /// </summary>
    /// <remarks>
    /// Ranges are the same if the have the same
    /// <see cref="Type">min</see> and <see cref="Type">max</see> values.
    /// </remarks>
    /// <param name="other"> The other <see cref="ReadingEntity">Range</see> to compare for equality
    /// with this instance. </param>
    /// <returns> A Boolean data type. </returns>
    public bool Equals( ReadingEntity other )
    {
        return other is not null && Equals( this, other );
    }

    /// <summary> Creates a unique hash code. </summary>
    /// <returns> An <see cref="int">integer</see> value. </returns>
    public override int GetHashCode()
    {
        return this.RawValueReading.GetHashCode();
    }

    #endregion

    #region " reset "

    /// <summary> Resets value to nothing. </summary>
    public virtual void Reset()
    {
        this.RawValueReading = string.Empty;
    }

    #endregion

    #region " reading "

    /// <summary> Gets or sets the type of the reading. </summary>
    /// <value> The type of the reading. </value>
    public ReadingElementTypes ReadingType { get; set; } = readingType;

    /// <summary>
    /// Applies the reading to create the specific reading type in the inherited class.
    /// </summary>
    /// <param name="rawValueReading"> The raw value reading. </param>
    /// <param name="rawUnitsReading"> The raw units reading. </param>
    /// <returns> True if it succeeds; otherwise, false. </returns>
    public virtual bool TryApplyReading( string rawValueReading, string rawUnitsReading )
    {
        // save the readings 
        if ( string.IsNullOrWhiteSpace( rawValueReading ) )
            rawValueReading = string.Empty;
        if ( string.IsNullOrWhiteSpace( rawUnitsReading ) )
            rawValueReading = string.Empty;
        this.RawValueReading = rawValueReading;
        this.RawUnitsReading = rawUnitsReading;
        return true;
    }

    /// <summary>
    /// Applies the reading to create the specific reading type in the inherited class.
    /// </summary>
    /// <param name="rawValueReading"> The value reading. </param>
    /// <returns> True if it succeeds; otherwise, false. </returns>
    public virtual bool TryApplyReading( string rawValueReading )
    {
        if ( string.IsNullOrWhiteSpace( rawValueReading ) )
        {
            rawValueReading = string.Empty;
        }

        this.RawValueReading = rawValueReading;
        return true;
    }

    /// <summary> Attempts to evaluate using the applied reading and given status. </summary>
    /// <param name="reading"> The reading. </param>
    /// <returns> <c>true</c> if evaluated. </returns>
    public virtual bool TryEvaluate( double reading )
    {
        return true;
    }

    /// <summary> Attempts to evaluate using the applied reading and given status. </summary>
    /// <param name="status"> The status. </param>
    /// <returns> <c>true</c> if evaluated. </returns>
    public virtual bool TryEvaluate( long status )
    {
        return true;
    }

    /// <summary> Returns a string that represents the current object. </summary>
    /// <returns> A <see cref="string" /> that represents the current object. </returns>
    public override string ToString()
    {
        return this.RawValueReading;
    }

    /// <summary>
    /// Gets or sets the sentinel indicating if the reading includes a units suffix.
    /// </summary>
    /// <value> <c>true</c> if the reading includes units. </value>
    public bool IncludesUnitsSuffix { get; set; }

    /// <summary> Gets or sets the raw value reading text. </summary>
    /// <value> The value reading. </value>
    public string RawValueReading { get; set; } = string.Empty;

    /// <summary>   Gets or sets the reading caption. This is the reading from the instrument. </summary>
    /// <value> The reading caption. </value>
    public string ReadingCaption { get; set; } = string.Empty;

    /// <summary> Gets or sets the length of the reading. </summary>
    /// <value> The length of the reading. </value>
    public int ReadingLength { get; set; }

    /// <summary> Gets or sets the heading. </summary>
    /// <value> The heading. </value>
    public string Heading { get; set; } = string.Empty;

    /// <summary> Gets or sets the raw units reading. </summary>
    /// <value> The units reading. </value>
    public string RawUnitsReading { get; set; } = string.Empty;

    #endregion
}

