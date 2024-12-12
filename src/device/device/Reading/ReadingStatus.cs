namespace cc.isr.VI;

/// <summary> Defines a <see cref="int">Status</see> reading. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-01 </para>
/// </remarks>
public class ReadingStatus : ReadingValue
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    /// <param name="readingType"> Type of the reading. </param>
    public ReadingStatus( ReadingElementTypes readingType ) : base( readingType )
    {
    }

    /// <summary> Constructs a copy of an existing value. </summary>
    /// <param name="model"> The model. </param>
    public ReadingStatus( ReadingStatus model ) : base( model )
    {
    }

    #endregion

    #region " values "

    /// <summary> Gets the Status Value. </summary>
    /// <remarks> Handles the case where the status value was saved as infinite. </remarks>
    /// <value> The Status Value. </value>
    public long? StatusValue => this.Value.HasValue ? ( long ) (this.Value.Value < 0d ? 0d : this.Value.Value > long.MaxValue ? long.MaxValue : this.Value.Value) : new long?();

    /// <summary> QueryEnum if 'bit' is bit. </summary>
    /// <param name="bit"> The bit. </param>
    /// <returns> <c>true</c> if bit; otherwise <c>false</c> </returns>
    public bool IsBit( int bit )
    {
        return this.StatusValue.HasValue && (1L & (this.StatusValue.Value >> bit)) == 1L;
    }

    /// <summary> Returns a string that represents the current object. </summary>
    /// <returns> A <see cref="string" /> that represents the current object. </returns>
    public override string ToString()
    {
        return this.StatusValue.HasValue ? $"0x{this.StatusValue.Value:X}" : "empty";
    }

    /// <summary> Returns a string that represents the current object. </summary>
    /// <param name="nibbleCount"> Number of nibbles. </param>
    /// <returns> A <see cref="string" /> that represents the current object. </returns>
    public string ToString( int nibbleCount )
    {
        return this.StatusValue.HasValue ? string.Format( string.Format( System.Globalization.CultureInfo.CurrentCulture, "0x{{0:X{0}}}", nibbleCount ), this.StatusValue.Value ) : this.ToString();
    }

    #endregion
}
