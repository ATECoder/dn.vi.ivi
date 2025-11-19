using cc.isr.Enums;

namespace cc.isr.VI.Pith;

/// <summary> Defines an enumerated value for reading and writing device messages. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-04-09 </para>
/// </remarks>
/// <remarks> Constructor. </remarks>
/// <param name="enumValue">   The enum value. </param>
/// <param name="readValue">   The read value. </param>
/// <param name="writeValue">  The write value. </param>
/// <param name="description"> The description. </param>
public class EnumReadWrite( long enumValue, string readValue, string writeValue, string description ) : object()
{
    /// <summary> Constructor. </summary>
    /// <param name="enumValue">   The enum value. </param>
    /// <param name="readValue">   The read value. </param>
    /// <param name="description"> The description. </param>
    public EnumReadWrite( int enumValue, string readValue, string description ) : this( enumValue, readValue, readValue, description )
    {
    }

    /// <summary> Gets or sets the read value. </summary>
    /// <value> The read value. </value>
    public string ReadValue { get; set; } = readValue;

    /// <summary> Gets or sets the enum value. </summary>
    /// <value> The enum value. </value>
    public long EnumValue { get; set; } = enumValue;

    /// <summary> Gets or sets the description. </summary>
    /// <value> The description. </value>
    public string Description { get; set; } = description;

    /// <summary> Gets or sets the write value. </summary>
    /// <value> The write value. </value>
    public string WriteValue { get; set; } = writeValue;
}
/// <summary> Collection of enum reading and writing values. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-04-09 </para>
/// </remarks>
public class EnumReadWriteCollection : System.Collections.ObjectModel.KeyedCollection<long, EnumReadWrite>
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="System.Collections.ObjectModel.KeyedCollection{Long, EnumReadWrite}" /> class that uses the default
    /// equality comparer.
    /// </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    public EnumReadWriteCollection() : base() => this.ReadValueLookup = new ReadValueCollection();

    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns> The key for the specified element. </returns>
    protected override long GetKeyForItem( EnumReadWrite item )
    {
        return item is null ? throw new ArgumentNullException( nameof( item ) ) : item.EnumValue;
    }

    /// <summary> Collection of read values. </summary>
    private class ReadValueCollection : System.Collections.ObjectModel.KeyedCollection<string, EnumReadWrite>
    {
        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <param name="item"> The element from which to extract the key. </param>
        /// <returns> The key for the specified element. </returns>
        protected override string GetKeyForItem( EnumReadWrite item )
        {
            return item is null ? throw new ArgumentNullException( nameof( item ) ) : item.ReadValue;
        }
    }

    /// <summary> Gets or sets the read value lookup. </summary>
    /// <value> The read value lookup. </value>
    private ReadValueCollection ReadValueLookup { get; set; }

    /// <summary>
    /// Removes all items from the <see cref="System.Collections.ICollection" />.
    /// </summary>
    public new void Clear()
    {
        base.Clear();
        this.ReadValueLookup.Clear();
    }

    /// <summary>   Select item by the <see cref="EnumReadWrite.ReadValue"/>. </summary>
    /// <remarks>   David, 2021-06-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="readValue">    The code. </param>
    /// <returns>   Enum read write value. </returns>
    public EnumReadWrite SelectItem( string readValue )
    {
        return string.IsNullOrWhiteSpace( readValue )
            ? throw new ArgumentNullException( nameof( readValue ) )
            : this.ReadValueLookup.Contains( readValue )
                ? this.ReadValueLookup[readValue]
                : this.ReadValueLookup.Contains( readValue.Trim( '\"' ) )
                    ? this.ReadValueLookup[readValue.Trim( '\"' )]
                    : throw new KeyNotFoundException( $"Key {readValue} not found in the {nameof( EnumReadWriteCollection )}" );
    }

    /// <summary>
    /// Select item or default. This is required if Subsystem enum value is set at None whereas None
    /// is not an instrument option.
    /// </summary>
    /// <param name="enumValue"> The value. </param>
    /// <returns> An EnumReadWrite. </returns>
    public EnumReadWrite SelectItemOrDefault( long enumValue )
    {
        return this.Contains( enumValue ) ? this[enumValue] : this.Items.FirstOrDefault();
    }

    /// <summary> Select item by the <see cref="EnumReadWrite.EnumValue"/>. </summary>
    /// <param name="enumValue"> The value. </param>
    /// <returns> A CodeValueDescription. </returns>
    public EnumReadWrite SelectItem( long enumValue )
    {
        return this[enumValue];
    }

    /// <summary> Determine if 'readValue' exists. </summary>
    /// <param name="readValue"> The code. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool Exists( string readValue )
    {
        return this.ReadValueLookup.Contains( readValue ) || this.ReadValueLookup.Contains( readValue.Trim( '\"' ) );
    }

    /// <summary> Determine if 'enumValue' exists. </summary>
    /// <param name="enumValue"> The value. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool Exists( long enumValue )
    {
        return this.Contains( enumValue );
    }

    /// <summary> Adds or replaces an item. </summary>
    /// <param name="enumValue">   The value. </param>
    /// <param name="readValue">   The code. </param>
    /// <param name="description"> The description. </param>
    public void Add( long enumValue, string readValue, string description )
    {
        this.AddReplace( enumValue, readValue, readValue, description );
    }

    /// <summary> Gets or sets the read value decorator. </summary>
    /// <value> The read value decorator. </value>
    public string ReadValueDecorator { get; set; } = "{0}";

    /// <summary> Gets or sets the write value decorator. </summary>
    /// <value> The write value decorator. </value>
    public string WriteValueDecorator { get; set; } = "{0}";

    /// <summary> Adds or replaces an item. </summary>
    /// <param name="enumItem"> The enum item to add. </param>
    public void Add( Enum enumItem )
    {
        string readValue = enumItem.ExtractBetween();
        string writeValue = string.Format( this.WriteValueDecorator, readValue );
        readValue = string.Format( this.ReadValueDecorator, readValue );
        string description = enumItem.DescriptionUntil();
        long enumValue = Convert.ToInt64( enumItem, System.Globalization.CultureInfo.CurrentCulture );
        this.AddReplace( enumValue, readValue, writeValue, description );
    }

    /// <summary> Adds or replaces an item. </summary>
    /// <param name="enumValue">   The value. </param>
    /// <param name="readValue">   The code. </param>
    /// <param name="writeValue">  The write value. </param>
    /// <param name="description"> The description. </param>
    public void Add( long enumValue, string readValue, string writeValue, string description )
    {
        this.AddReplace( enumValue, readValue, writeValue, description );
    }

    /// <summary> Adds or replaces an item. </summary>
    /// <param name="enumValue">   The value. </param>
    /// <param name="readValue">   The code. </param>
    /// <param name="description"> The description. </param>
    public void AddReplace( long enumValue, string readValue, string description )
    {
        this.AddReplace( enumValue, readValue, readValue, description );
    }

    /// <summary> Adds or replaces an item. </summary>
    /// <param name="enumValue">   The value. </param>
    /// <param name="readValue">   The code. </param>
    /// <param name="writeValue">  The write value. </param>
    /// <param name="description"> The description. </param>
    public void AddReplace( long enumValue, string readValue, string writeValue, string description )
    {
        this.AddReplace( new EnumReadWrite( enumValue, readValue, writeValue, description ) );
    }

    /// <summary> Adds or replaces an item. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="enumReadWrite"> The enum read write. </param>
    public void AddReplace( EnumReadWrite enumReadWrite )
    {
        if ( enumReadWrite is null ) throw new ArgumentNullException( nameof( enumReadWrite ) );
        _ = this.RemoveAt( enumReadWrite );
        this.Add( enumReadWrite );
        this.ReadValueLookup.Add( enumReadWrite );
    }

    /// <summary>
    /// Attempts to remove an item if its <see cref="EnumReadWrite.ReadValue"/> or
    /// <see cref="EnumReadWrite.EnumValue"/> exist.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="enumCodeValue"> The <see cref="EnumReadWrite"/> . </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool RemoveAt( EnumReadWrite enumCodeValue )
    {
        return enumCodeValue is null
            ? throw new ArgumentNullException( nameof( enumCodeValue ) )
            : this.RemoveAt( enumCodeValue.ReadValue ) || this.RemoveAt( enumCodeValue.EnumValue );
    }

    /// <summary> Removes an item if its <see cref="EnumReadWrite.ReadValue"/> exists. </summary>
    /// <param name="readValue"> The code. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool RemoveAt( string readValue )
    {
        if ( this.Exists( readValue ) )
        {
            EnumReadWrite value = this.SelectItem( readValue );
            _ = this.ReadValueLookup.Remove( readValue );
            if ( this.Exists( value.EnumValue ) )
                _ = this.Remove( value.EnumValue );
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary> Remove an item if its <see cref="EnumReadWrite.EnumValue"/> exists. </summary>
    /// <param name="enumValue"> The enum value to remove. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool RemoveAt( byte enumValue )
    {
        return this.RemoveAt( ( long ) enumValue );
    }

    /// <summary> Remove an item if its <see cref="EnumReadWrite.EnumValue"/> exists. </summary>
    /// <param name="enumValue"> The enum value to remove. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool RemoveAt( short enumValue )
    {
        return this.RemoveAt( ( long ) enumValue );
    }

    /// <summary> Remove an item if its <see cref="EnumReadWrite.EnumValue"/> exists. </summary>
    /// <param name="enumValue"> The value. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public new bool RemoveAt( int enumValue )
    {
        return this.RemoveAt( ( long ) enumValue );
    }

    /// <summary> Remove an item if its <see cref="EnumReadWrite.EnumValue"/> exists. </summary>
    /// <param name="enumValue"> The enum value to remove. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool RemoveAt( long enumValue )
    {
        if ( this.Exists( enumValue ) )
        {
            EnumReadWrite value = this.SelectItem( enumValue );
            _ = this.Remove( enumValue );
            if ( this.Exists( value.ReadValue ) )
                _ = this.ReadValueLookup.Remove( value.ReadValue );
            return true;
        }
        else
        {
            return false;
        }
    }
}
