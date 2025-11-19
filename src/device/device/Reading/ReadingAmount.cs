namespace cc.isr.VI;

/// <summary> Implements a reading <see cref="cc.isr.UnitsAmounts.Amount">amount</see>. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-01 </para>
/// </remarks>
public class ReadingAmount : ReadingValue
{
    #region " construction and cleanup "

    /// <summary>
    /// Constructs a measured value without specifying the value or its validity, which must be
    /// specified for the value to be made valid.
    /// </summary>
    /// <param name="readingType"> Type of the reading. </param>
    /// <param name="unit">        The unit. </param>
    public ReadingAmount( ReadingElementTypes readingType, cc.isr.UnitsAmounts.Unit unit )
        : base( readingType ) => this._amount = new cc.isr.UnitsAmounts.Amount( 0d, unit );

    /// <summary> Constructs a copy of an existing value. </summary>
    /// <param name="model"> The model. </param>
    public ReadingAmount( ReadingAmount model ) : base( model )
    {
        if ( model is not null )
            this._amount = new cc.isr.UnitsAmounts.Amount( model.Amount );
        else
            this._amount = new();
    }

    /// <summary>
    /// Constructs a measured value specifying the value or its validity, which must be specified for
    /// the value to be made valid.
    /// </summary>
    /// <param name="model"> The model. </param>
    public ReadingAmount( BufferReading model ) : base( BufferReading.Validated( model ).MeasuredElementType )
    {
        this.Value = model.HasReading ? model.Value : new double?();
        this._amount = model.Amount;
    }

    #endregion

    #region " equals "

    /// <summary> = casting operator. </summary>
    /// <param name="left">  The left hand side item to compare for equality. </param>
    /// <param name="right"> The left hand side item to compare for equality. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( ReadingAmount left, ReadingAmount right )
    {
        return left is null ? right is null : right is not null && Equals( left, right );
    }

    /// <summary> casting operator. </summary>
    /// <param name="left">  The left hand side item to compare for equality. </param>
    /// <param name="right"> The left hand side item to compare for equality. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( ReadingAmount left, ReadingAmount right )
    {
        return !Equals( left, right );
    }

    /// <summary> Returns True if equal. </summary>
    /// <remarks>
    /// Reading amounts are the same if they have the same <see cref="Amount"/>.
    /// </remarks>
    /// <param name="left">  The left hand side item to compare for equality. </param>
    /// <param name="right"> The left hand side item to compare for equality. </param>
    /// <returns> <c>true</c> if equals. </returns>
    public static bool Equals( ReadingAmount left, ReadingAmount right )
    {
        return left is null ? right is null : right is not null && Equals( left.Amount, right.Amount );
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
        return this.Equals( ( ReadingAmount ) obj );
    }

    /// <summary>
    /// Returns True if the value of the <paramref name="other"/> equals to the instance value.
    /// </summary>
    /// <remarks>
    /// Reading amounts are equal if they have the same <see cref="Amount"/>.
    /// </remarks>
    /// <param name="other"> The other <see cref="ReadingAmount">Range</see> to compare for equality
    /// with this instance. </param>
    /// <returns> A Boolean data type. </returns>
    public bool Equals( ReadingAmount other )
    {
        return other is not null && Equals( this, other );
    }

    /// <summary> Creates a unique hash code. </summary>
    /// <returns> An <see cref="int">integer</see> value. </returns>
    public override int GetHashCode()
    {
        return this.Amount.GetHashCode();
    }

    #endregion

    #region " amount "

    /// <summary> Gets the has value. </summary>
    /// <value> The has value. </value>
    public bool HasValue => this.Value.HasValue;

    /// <summary> Gets the symbol. </summary>
    /// <value> The symbol. </value>
    public string Symbol => this.Amount.Unit.Symbol;

    /// <summary> Returns a string that represents the current object. </summary>
    /// <returns> A <see cref="string" /> that represents the current object. </returns>
    public override string ToString()
    {
        return this.HasValue ? $"{this.Amount}" : $"-.---- {this.Amount.Unit}";
    }

    /// <summary> The amount. </summary>
    private cc.isr.UnitsAmounts.Amount _amount;

    /// <summary> The amount. </summary>
    /// <value> The amount. </value>
    public cc.isr.UnitsAmounts.Amount Amount
    {
        get => this._amount;
        set
        {
            value ??= new cc.isr.UnitsAmounts.Amount( VI.Syntax.ScpiSyntax.NotANumber, this.Amount.Unit );

            this._amount = value;
        }
    }

    /// <summary> Applies the unit described by unit. </summary>
    /// <param name="unit"> The unit. </param>
    public virtual void ApplyUnit( cc.isr.UnitsAmounts.Unit unit )
    {
        if ( this.Amount.Unit != unit )
        {
            this.Amount = new cc.isr.UnitsAmounts.Amount( VI.Syntax.ScpiSyntax.NotANumber, unit );
            this.Value = new double?();
        }
    }

    /// <summary>
    /// Parses the reading to create the specific reading type in the inherited class.
    /// </summary>
    /// <param name="rawValueReading"> The raw value reading. </param>
    /// <param name="rawUnitsReading"> The raw units reading. </param>
    /// <returns> <c>true</c> if parsed. </returns>
    public override bool TryApplyReading( string rawValueReading, string rawUnitsReading )
    {
        if ( base.TryApplyReading( rawValueReading, rawUnitsReading ) )
        {
            return this.TryApplyReading( rawValueReading );
        }
        else
        {
            this.Amount = new cc.isr.UnitsAmounts.Amount( VI.Syntax.ScpiSyntax.NotANumber, this.Amount.Unit );
            return false;
        }
    }

    /// <summary>
    /// Parses the reading to create the specific reading type in the inherited class.
    /// </summary>
    /// <param name="valueReading"> The value reading. </param>
    /// <returns> <c>true</c> if parsed. </returns>
    public override bool TryApplyReading( string valueReading )
    {
        if ( base.TryApplyReading( valueReading ) )
        {
            if ( this.Value.HasValue )
            {
                this.Amount = new cc.isr.UnitsAmounts.Amount( this.Value.Value, this.Amount.Unit );
            }

            return true;
        }
        else
        {
            this.Amount = new cc.isr.UnitsAmounts.Amount( VI.Syntax.ScpiSyntax.NotANumber, this.Amount.Unit );
            return false;
        }
    }

    #endregion
}
