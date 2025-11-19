// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary> A channel source measure elements. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-01-13 </para>
/// </remarks>
public class ChannelSourceMeasure
{
    /// <summary> Constructor. </summary>
    /// <param name="title">              The title. </param>
    /// <param name="sourceChannelList">  A List of source channels. </param>
    /// <param name="measureChannelList"> A List of measure channels. </param>
    public ChannelSourceMeasure( string title, string sourceChannelList, string measureChannelList ) : base()
    {
        this.Title = title;
        this.ChannelList = string.Empty;
        this.SourceChannelList = ToSortedList( sourceChannelList );
        this.MeasureChannelList = ToSortedList( measureChannelList );
        this.MergeChannelLists();
    }

    /// <summary> Constructor. </summary>
    /// <param name="channelSourceMeasure"> The channel source measure. </param>
    public ChannelSourceMeasure( ChannelSourceMeasure channelSourceMeasure ) : this( Validated( channelSourceMeasure ).Title, channelSourceMeasure.SourceChannelList, channelSourceMeasure.MeasureChannelList )
    {
        this.Current = channelSourceMeasure.Current;
        this.Voltage = channelSourceMeasure.Voltage;
    }

    /// <summary> Validated the given channel source measure. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="channelSourceMeasure"> The channel source measure. </param>
    /// <returns> A ChannelSourceMeasure. </returns>
    public static ChannelSourceMeasure Validated( ChannelSourceMeasure channelSourceMeasure )
    {
        return channelSourceMeasure is null ? throw new ArgumentNullException( nameof( channelSourceMeasure ) ) : channelSourceMeasure;
    }

    /// <summary> Converts this object to a sorted list. </summary>
    /// <param name="list"> The list. </param>
    /// <returns> The given data converted to a <see cref="string" />. </returns>
    private static string ToSortedList( string list )
    {
        return ToSortedList( list, ChannelListDelimiter( list ) );
    }

    /// <summary> Channel list delimiter. </summary>
    /// <param name="list"> The list. </param>
    /// <returns> A Char. </returns>
    private static char ChannelListDelimiter( string list )
    {
        char result = ';';
        if ( !list.Contains( result.ToString() ) )
        {
            result = ',';
        }

        return result;
    }

    /// <summary> Converts this object to a sorted list. </summary>
    /// <param name="list">      The list. </param>
    /// <param name="delimiter"> The delimiter. </param>
    /// <returns> The given data converted to a <see cref="string" />. </returns>
    private static string ToSortedList( string list, char delimiter )
    {
        System.Text.StringBuilder result = new();
        List<string> l = [.. list.Split( delimiter )];
        l.Sort();
        foreach ( string s in l )
        {
            if ( result.Length > 0 )
                _ = result.Append( delimiter );
            _ = result.Append( s );
        }

        return result.ToString();
    }

    /// <summary> Gets the title. </summary>
    /// <value> The title. </value>
    public string Title { get; private set; }

    /// <summary> Gets a list of source channels. </summary>
    /// <value> A List of source channels. </value>
    public string SourceChannelList { get; private set; }

    /// <summary> Gets a list of measure channels. </summary>
    /// <value> A List of measure channels. </value>
    public string MeasureChannelList { get; private set; }

    /// <summary> Merge channel lists. </summary>
    private void MergeChannelLists()
    {
        if ( string.IsNullOrWhiteSpace( this.SourceChannelList ) )
        {
            this.ChannelList = string.IsNullOrWhiteSpace( this.MeasureChannelList ) ? string.Empty : ToSortedList( this.MeasureChannelList );
        }
        else if ( string.IsNullOrWhiteSpace( this.MeasureChannelList ) )
        {
            this.ChannelList = ToSortedList( this.SourceChannelList );
        }
        else
        {
            System.Text.StringBuilder builder = new();
            _ = builder.Append( this.SourceChannelList );
            _ = builder.Append( ChannelListDelimiter( builder.ToString() ) );
            _ = builder.Append( this.MeasureChannelList );
            this.ChannelList = ToSortedList( builder.ToString() );
        }
    }

    /// <summary> Gets a list of channels. </summary>
    /// <value> A List of channels. </value>
    public string ChannelList { get; private set; }

    /// <summary> Gets the sentinel indicating if the measure has a non zero current value. </summary>
    /// <value> The sentinel indicating if the measure has a non zero current value. </value>
    public bool HasValue => this.Current != 0d;

    /// <summary> Gets the resistance. </summary>
    /// <value>
    /// The sheet resistance or <see cref="double.NaN"/> if not <see cref="HasValue"/>.
    /// </value>
    public double Resistance => this.HasValue ? this.Voltage / this.Current : double.NaN;

    /// <summary> Gets or sets the voltage. </summary>
    /// <value> The voltage. </value>
    public double Voltage { get; set; }

    /// <summary> Gets or sets the current. </summary>
    /// <value> The current. </value>
    public double Current { get; set; }
}
/// <summary>
/// Channel source measure Collection: an ordered collection of source measures.
/// </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-01-13 </para>
/// </remarks>
public class ChannelSourceMeasureCollection : System.Collections.ObjectModel.KeyedCollection<string, ChannelSourceMeasure>
{
    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns> The key for the specified element. </returns>
    protected override string GetKeyForItem( ChannelSourceMeasure item )
    {
        return item is null ? throw new ArgumentNullException( nameof( item ) ) : item.Title;
    }

    /// <summary> Adds a new source measure. </summary>
    /// <param name="title">              The title. </param>
    /// <param name="sourceChannelList">  List of channels. </param>
    /// <param name="measureChannelList"> List of measure channels. </param>
    public void AddSourceMeasure( string title, string sourceChannelList, string measureChannelList )
    {
        this.Add( new ChannelSourceMeasure( title, sourceChannelList, measureChannelList ) );
    }
}
