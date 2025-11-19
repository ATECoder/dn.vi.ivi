// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;
/// <summary>
/// Defines the collection of <see cref="ReadingEntity">reading elements</see>.
/// </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-11-02 </para>
/// </remarks>
public class ReadingEntityCollection : System.Collections.ObjectModel.KeyedCollection<ReadingElementTypes, ReadingEntity>
{
    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns> The key for the specified element. </returns>
    protected override ReadingElementTypes GetKeyForItem( ReadingEntity item )
    {
        return item is null ? throw new ArgumentNullException( nameof( item ) ) : item.ReadingType;
    }

    /// <summary> Default constructor. </summary>
    public ReadingEntityCollection() : base() => this.Elements = ReadingElementTypes.None;

    /// <summary> Constructor. </summary>
    /// <param name="model"> The model. </param>
    public ReadingEntityCollection( ReadingEntityCollection model ) : this()
    {
        if ( model is not null )
        {
            foreach ( ReadingEntity entity in model )
                this.Add( entity );
        }
    }

    /// <summary> Gets a list of types of the elements. </summary>
    /// <value> A list of types of the elements. </value>
    public IEnumerable<ReadingElementTypes> ElementTypes => this.Dictionary.Keys;

    /// <summary> Gets the reading entities. </summary>
    /// <value> The reading entities. </value>
    public IEnumerable<ReadingEntity> ReadingEntities => this.Dictionary is null ? Array.Empty<ReadingEntity>() : this.Dictionary.Values;

    /// <summary> Gets or sets the reading elements. </summary>
    /// <value> The elements. </value>
    public ReadingElementTypes Elements { get; private set; }

    /// <summary> Adds an item. </summary>
    /// <param name="item"> The item. </param>
    public new void Add( ReadingEntity item )
    {
        if ( item is null )
            return;
        base.Add( item );
        // add the length of the delimiter.
        if ( this.ReadingsLength > 0 )
            this.ReadingsLength += 1;
        this.ReadingsLength += item.ReadingLength;
        this.Elements |= item.ReadingType;
    }

    /// <summary> Adds if reading entity type is included in the mask. </summary>
    /// <param name="mask"> The mask. </param>
    /// <param name="item"> The item. </param>
    public void AddIf( ReadingElementTypes mask, ReadingEntity? item )
    {
        if ( item is not null && (mask & item.ReadingType) != 0 )
            this.Add( item );
    }

    /// <summary> Include unit suffix if. </summary>
    /// <param name="value"> The value. </param>
    public void IncludeUnitSuffixIf( ReadingElementTypes value )
    {
        // Units is a property of each element. If units are turned on, each element units is enabled.
        if ( (value & ReadingElementTypes.Units) != 0 )
        {
            foreach ( ReadingEntity e in this )
                e.IncludesUnitsSuffix = true;
        }
    }

    /// <summary> Returns the total length of the reading elements including delimiters. </summary>
    /// <value> The length of the elements. </value>
    public int ReadingsLength { get; private set; }

    /// <summary> Resets all values to null. </summary>
    public void Reset()
    {
        foreach ( ReadingEntity r in this )
            r.Reset();
    }

    /// <summary> Gets the list of raw readings. </summary>
    /// <returns> A list of raw readings. </returns>
    public string[] ToRawReadings()
    {
        List<string> values = [];
        foreach ( ReadingEntity readingItem in this )
            values.Add( readingItem.RawValueReading );
        return [.. values];
    }

    /// <summary> Returns the meta status or new if does not exist. </summary>
    /// <param name="readingType"> Type of the reading. </param>
    /// <returns> The MetaStatus. </returns>
    public MetaStatus MetaStatus( ReadingElementTypes readingType )
    {
        MetaStatus result = new();
        if ( this.Contains( readingType ) )
        {
            MeasuredAmount? amount = this[readingType] as MeasuredAmount;
            if ( amount is not null )
                result = amount.MetaStatus;
        }

        return result;
    }

    /// <summary> Reading amount. </summary>
    /// <param name="readingType"> Type of the reading. </param>
    /// <returns> A ReadingAmount. </returns>
    public ReadingAmount ReadingAmount( ReadingElementTypes readingType )
    {
        ReadingAmount? result = null;
        if ( this.Contains( readingType ) )
        {
            result = this[readingType] as ReadingAmount;
        }
        result ??= new ReadingAmount( readingType, cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt );
        return result;
    }

    /// <summary> Reading value. </summary>
    /// <param name="readingType"> Type of the reading. </param>
    /// <returns> A ReadingValue. </returns>
    public ReadingValue ReadingValue( ReadingElementTypes readingType )
    {
        ReadingValue? result = null;
        if ( this.Contains( readingType ) )
        {
            result = this[readingType] as ReadingValue;
        }

        result ??= new ReadingValue( readingType );
        return result;
    }

    /// <summary> Reading status. </summary>
    /// <param name="readingType"> Type of the reading. </param>
    /// <returns> The ReadingStatus. </returns>
    public ReadingStatus ReadingStatus( ReadingElementTypes readingType )
    {
        ReadingStatus? result = null;
        if ( this.Contains( readingType ) )
        {
            result = this[readingType] as ReadingStatus;
        }

        result ??= new ReadingStatus( readingType );
        return result;
    }
}
