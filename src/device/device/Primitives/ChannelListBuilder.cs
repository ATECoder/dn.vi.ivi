namespace cc.isr.VI;

/// <summary> Builds a channel list. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2005-01-21, 1.0.1847.x. </para>
/// </remarks>
public class ChannelListBuilder
{
    #region " construction and cleanup "

    /// <summary> Constructs this class. </summary>
    public ChannelListBuilder() : base() => this._channelListStringBuilder = new System.Text.StringBuilder( string.Empty );

    /// <summary> Constructs this class. </summary>
    /// <param name="channelList"> A channel list to initialize. </param>
    public ChannelListBuilder( string channelList ) : this() => this.ChannelList = channelList;

    #endregion

    #region " shared: channel list element builders "

    /// <summary> Constructs a channel list element. </summary>
    /// <param name="memoryLocation"> Specifies the memory location. </param>
    /// <returns> A list of. </returns>
    public static string BuildElement( int memoryLocation )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, "M{0}", memoryLocation );
    }

    /// <summary> Constructs a channel list element. </summary>
    /// <param name="slotNumber">  Specifies the card slot number in the relay mainframe. </param>
    /// <param name="relayNumber"> Specifies the relay number in the slot. </param>
    /// <returns> A list of. </returns>
    public static string BuildElement( int slotNumber, int relayNumber )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, "{0}!{1}", slotNumber, relayNumber );
    }

    /// <summary> Constructs a channel list element. </summary>
    /// <param name="slotNumber">   Specifies the card slot number in the relay mainframe. </param>
    /// <param name="rowNumber">    Specifies the row number of the relay. </param>
    /// <param name="columnNumber"> Specifies the column number of the relay. </param>
    /// <returns> The channel list element string. </returns>
    public static string BuildElement( int slotNumber, int rowNumber, int columnNumber )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, "{0}!{1}!{2}", slotNumber, rowNumber, columnNumber );
    }

    /// <summary> Builds a range. </summary>
    /// <param name="fromChannelElement"> Specifies the starting channel element to add. </param>
    /// <param name="toChannelElement">   Specifies the ending channel element to add. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string BuildRange( string fromChannelElement, string toChannelElement )
    {
        return $"{fromChannelElement}:{toChannelElement}";
    }

    #endregion

    #region " add channel element "

    /// <summary> Adds a relay channel to the channel list. </summary>
    /// <param name="channelElement"> Specifies the channel element to add. </param>
    public void AddChannel( string channelElement )
    {
        if ( this.ElementCount > 0 )
            _ = this._channelListStringBuilder.Append( "," );

        _ = this._channelListStringBuilder.Append( channelElement );
        this.ElementCount += 1;
    }

    /// <summary> Adds a memory location to the channel list. </summary>
    /// <param name="memoryLocation"> Specifies the memory location. </param>
    public void AddChannel( int memoryLocation )
    {
        this.AddChannel( BuildElement( memoryLocation ) );
    }

    /// <summary> Adds a card relay channel to the channel list. </summary>
    /// <param name="slotNumber">  Specifies the card slot number in the relay mainframe. </param>
    /// <param name="relayNumber"> Specifies the relay number in the slot. </param>
    public void AddChannel( int slotNumber, int relayNumber )
    {
        this.AddChannel( BuildElement( slotNumber, relayNumber ) );
    }

    /// <summary> Adds a matrix relay channel to the channel list. </summary>
    /// <param name="slotNumber">   Specifies the card slot number in the relay mainframe. </param>
    /// <param name="rowNumber">    Specifies the row number of the relay. </param>
    /// <param name="columnNumber"> Specifies the column number of the relay. </param>
    public void AddChannel( int slotNumber, int rowNumber, int columnNumber )
    {
        this.AddChannel( BuildElement( slotNumber, rowNumber, columnNumber ) );
    }

    /// <summary> Adds a range of relay channels to the channel list. </summary>
    /// <param name="fromChannelElement"> Specifies the starting channel element to add. </param>
    /// <param name="toChannelElement">   Specifies the ending channel element to add. </param>
    public void AddChannelRange( string fromChannelElement, string toChannelElement )
    {
        if ( this.ElementCount > 0 )
            _ = this._channelListStringBuilder.Append( "," );

        _ = this._channelListStringBuilder.Append( BuildRange( fromChannelElement, toChannelElement ) );
        this.ElementCount += 1;
    }

    /// <summary> Adds a card relay range of channels channel to the channel list. </summary>
    /// <param name="fromSlotNumber">  Specifies the starting card slot number in the relay
    /// mainframe. </param>
    /// <param name="fromRelayNumber"> Specifies the starting relay number in the slot. </param>
    /// <param name="toSlotNumber">    Specifies the ending card slot number in the relay mainframe. </param>
    /// <param name="toRelayNumber">   Specifies the ending relay number in the slot. </param>
    public void AddChannelRange( int fromSlotNumber, int fromRelayNumber, int toSlotNumber, int toRelayNumber )
    {
        this.AddChannelRange( BuildElement( fromSlotNumber, fromRelayNumber ), BuildElement( toSlotNumber, toRelayNumber ) );
    }

    /// <summary> Adds a matrix relay range of channels to the channel list. </summary>
    /// <param name="fromSlotNumber">   Specifies the starting card slot number in the relay
    /// mainframe. </param>
    /// <param name="fromRowNumber">    Specifies the starting row number of the relay. </param>
    /// <param name="fromColumnNumber"> from column number. </param>
    /// <param name="toSlotNumber">     Specifies the ending card slot number in the relay mainframe. </param>
    /// <param name="toRowNumber">      Specifies the ending row number of the relay. </param>
    /// <param name="toColumnNumber">   Specifies the ending column number of the relay. </param>
    public void AddChannel( int fromSlotNumber, int fromRowNumber, int fromColumnNumber, int toSlotNumber, int toRowNumber, int toColumnNumber )
    {
        this.AddChannelRange( BuildElement( fromSlotNumber, fromRowNumber, fromColumnNumber ), BuildElement( toSlotNumber, toRowNumber, toColumnNumber ) );
    }

    /// <summary> The channel list string builder. </summary>
    private System.Text.StringBuilder _channelListStringBuilder;

    /// <summary> Gets or sets the channel list sans the '(@' prefix and ')' suffix. </summary>
    /// <value> A List of naked channels. </value>
    public string NakedChannelList
    {
        get => this._channelListStringBuilder.ToString();
        set
        {
            this._channelListStringBuilder = new System.Text.StringBuilder( value );
            this.ElementCount = this._channelListStringBuilder.ToString().Split( ',' ).Length;
        }
    }

    /// <summary>
    /// Gets or sets an operation-able channel list that can be passed to the route and scan commands.
    /// </summary>
    /// <remarks>
    /// The channel list specifies the channels to be closed or opened.  Each comma delimited channel
    /// in the list is made up of either a exclamation point separated two Int32 card relay number
    /// (i.e., slot#!relay#) or a three Int32 exclamation point separated matrix relay number (i.e.,
    /// slot@!row#!column#) or a memory location (M#).  The channel list begins with a prefix '(@'
    /// and ends with a suffix ")".
    /// </remarks>
    /// <value> A List of channels. </value>
    public string ChannelList
    {
        get => string.Format( System.Globalization.CultureInfo.CurrentCulture, "(@{0})", this._channelListStringBuilder );
        set
        {
            if ( !string.IsNullOrWhiteSpace( value ) )
                this.NakedChannelList = value[2..].TrimEnd( ')' ).Trim();
        }
    }

    /// <summary>
    /// Returns the number of elements in the channel list.  This is not the same as the number of
    /// channels.
    /// </summary>
    /// <value> The number of elements. </value>
    public int ElementCount { get; private set; }

    #endregion
}
