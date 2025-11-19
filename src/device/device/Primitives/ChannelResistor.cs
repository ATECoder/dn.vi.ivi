// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary> A channel resistor. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-01-13 </para>
/// </remarks>
/// <remarks> Constructor. </remarks>
/// <param name="title">       The title. </param>
/// <param name="channelList"> A list of channels. </param>
public class ChannelResistor( string title, string channelList ) : object()
{
    /// <summary> Gets the title. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The title. </value>
    public string Title { get; private set; } = title;

    /// <summary> Gets a list of channels. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> A list of channels. </value>
    public string ChannelList { get; private set; } = channelList;

    /// <summary> Gets the resistance. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The resistance. </value>
    public double Resistance { get; set; }

    /// <summary> Gets the status. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The status. </value>
    public MetaStatus Status { get; set; } = new MetaStatus();
}
/// <summary> channel Resistor Collection: an ordered collection of resistors. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-01-13 </para>
/// </remarks>
public class ChannelResistorCollection : System.Collections.ObjectModel.KeyedCollection<string, ChannelResistor>
{
    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns> The key for the specified element. </returns>
    protected override string GetKeyForItem( ChannelResistor item )
    {
        return item is null ? throw new ArgumentNullException( nameof( item ) ) : item.Title;
    }

    /// <summary> Adds a new resistor. </summary>
    /// <param name="title">       The title. </param>
    /// <param name="channelList"> List of channels. </param>
    public void AddResistor( string title, string channelList )
    {
        this.Add( new ChannelResistor( title, channelList ) );
    }

    /// <summary> Gets the resistances. </summary>
    /// <value> The resistances. </value>
    public IList<double> Resistances
    {
        get
        {
            List<double> l = [];
            foreach ( ChannelResistor resistor in this )
                l.Add( resistor.Resistance );
            return l;
        }
    }

    /// <summary> Builds status report. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public string BuildStatusReport()
    {
        System.Text.StringBuilder infoBuilder = new();
        foreach ( ChannelResistor r in this )
            _ = infoBuilder.AppendLine( $"{r.Title}:{r.Status.ToLongDescription( "" )}" );
        return infoBuilder.ToString().TrimEnd( Environment.NewLine.ToCharArray() );
    }

    /// <summary> Gets the status. </summary>
    /// <value> The status. </value>
    public MetaStatus Status
    {
        get
        {
            MetaStatus result = new();
            foreach ( ChannelResistor r in this )
                result.Append( r.Status );
            return result;
        }
    }
}
