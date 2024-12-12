using cc.isr.VI.Pith;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by a Route Subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2005-01-15, 1.0.1841.x. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class RouteSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary> Initializes a new instance of the <see cref="RouteSubsystemBase" /> class. </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected RouteSubsystemBase( StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this._scanList = string.Empty;
        this.TerminalsModeReadWrites = [];
        this.DefineTerminalsModeReadWrites();
    }

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.ScanList = string.Empty;
        this.TerminalsMode = RouteTerminalsModes.Front;
    }

    #endregion

    #region " closed channel "

    /// <summary> The closed channel. </summary>
    private string? _closedChannel;

    /// <summary> Gets or sets the closed Channel. </summary>
    /// <remarks> Nothing is not set. </remarks>
    /// <value> The closed Channel. </value>
    public string? ClosedChannel
    {
        get => this._closedChannel;

        protected set
        {
            if ( !string.Equals( value, this.ClosedChannel, StringComparison.Ordinal ) )
            {
                this._closedChannel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Applies the closed Channel described by value. </summary>
    /// <param name="value">   The scan list. </param>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplyClosedChannel( string value, TimeSpan timeout )
    {
        _ = this.WriteClosedChannel( value, timeout );
        return this.QueryClosedChannel();
    }

    /// <summary> Gets or sets the closed Channel query command. </summary>
    /// <remarks> :ROUT:CLOS. </remarks>
    /// <value> The closed Channel query command. </value>
    protected virtual string ClosedChannelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries closed Channel. </summary>
    /// <returns> The closed Channel. </returns>
    public string? QueryClosedChannel()
    {
        this.ClosedChannel = this.Session.QueryTrimEnd( this.ClosedChannel ?? string.Empty, this.ClosedChannelQueryCommand );
        return this.ClosedChannel;
    }

    /// <summary> Gets or sets the closed Channel command format. </summary>
    /// <remarks> :ROUT:CLOS {0} </remarks>
    /// <value> The closed Channel command format. </value>
    protected virtual string ClosedChannelCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes a closed Channel. </summary>
    /// <param name="value">   The scan list. </param>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? WriteClosedChannel( string value, TimeSpan timeout )
    {
        _ = this.Session.WriteLine( this.ClosedChannelCommandFormat, value );
        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
        ServiceRequests statusByte = this.Session.AwaitOperationCompletion( timeout ).Status;
        this.Session.ThrowDeviceExceptionIfError( statusByte );

        this.ClosedChannel = value;
        this.ClosedChannels = null;
        this.ClosedChannelsState = null;
        this.OpenChannels = null;
        this.OpenChannelsState = null;
        return this.ClosedChannel;
    }

    /// <summary> Gets or sets the open Channel command format. </summary>
    /// <remarks> :ROUT:OPEN {0} </remarks>
    /// <value> The open Channel command format. </value>
    protected virtual string OpenChannelCommandFormat { get; set; } = string.Empty;

    /// <summary> Applies the open channel list and reads back the list. </summary>
    /// <param name="channelList"> List of Channel. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplyOpenChannel( string? channelList, TimeSpan timeout )
    {
        _ = this.WriteOpenChannel( channelList, timeout );
        return this.QueryClosedChannel();
    }

    /// <summary> Opens the specified Channel in the list. </summary>
    /// <param name="channelList"> List of Channel. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? WriteOpenChannel( string? channelList, TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( this.OpenChannelCommandFormat ) )
            _ = this.Session.WriteLine( $"{string.Format( this.OpenChannelCommandFormat, channelList )}; {this.Session.OperationCompleteCommand}" );

        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
        ServiceRequests statusByte = this.Session.AwaitOperationCompletion( timeout ).Status;
        this.Session.ThrowDeviceExceptionIfError( statusByte );

        // set to nothing to indicate that the value is not known -- requires reading.
        this.ClosedChannel = null;
        this.ClosedChannels = null;
        this.ClosedChannelsState = null;
        this.OpenChannels = null;
        this.OpenChannelsState = null;
        return this.ClosedChannel;
    }

    #endregion

    #region " closed channels "

    /// <summary> State of the closed channels. </summary>
    private string? _closedChannelsState;

    /// <summary> Gets or sets the closed channels state. </summary>
    /// <remarks> Nothing is not set. </remarks>
    /// <value> The closed channels. </value>
    public string? ClosedChannelsState
    {
        get => this._closedChannelsState;

        protected set
        {
            if ( !string.Equals( value, this.ClosedChannelsState, StringComparison.Ordinal ) )
            {
                this._closedChannelsState = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The closed channels checked. </summary>
    private string? _closedChannelsChecked;

    /// <summary> Gets or sets the closed channels checked for state. </summary>
    /// <remarks> Nothing is not set. </remarks>
    /// <value> The closed channels. </value>
    public string? ClosedChannelsChecked
    {
        get => this._closedChannelsChecked;

        protected set
        {
            if ( !string.Equals( value, this.ClosedChannelsChecked, StringComparison.Ordinal ) )
            {
                this._closedChannelsChecked = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the closed channels query command. </summary>
    /// <remarks> :ROUT:CLOS? {0} </remarks>
    /// <value> The closed channels query command. </value>
    protected virtual string ClosedChannelsStateQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets the supports closed channels state query. </summary>
    /// <value> The supports closed channels state query. </value>
    public bool SupportsClosedChannelsStateQuery => !string.IsNullOrWhiteSpace( this.ClosedChannelsStateQueryCommand );

    /// <summary> Queries closed channels state. </summary>
    /// <param name="channelList"> List of Channel. </param>
    /// <returns> The closed channels state. </returns>
    public string? QueryClosedChannelsState( string channelList )
    {
        this.ClosedChannelsState = this.Session.QueryTrimEnd( this.ClosedChannelsState, string.Format( this.ClosedChannelsStateQueryCommand, channelList ) );
        this.ClosedChannelsChecked = channelList;
        return this.ClosedChannelsState;
    }

    /// <summary> The closed channels. </summary>
    private string? _closedChannels;

    /// <summary> Gets or sets the closed channels. </summary>
    /// <remarks> Nothing is not set. </remarks>
    /// <value> The closed channels. </value>
    public string? ClosedChannels
    {
        get => this._closedChannels;

        protected set
        {
            if ( !string.Equals( value, this.ClosedChannels, StringComparison.Ordinal ) )
            {
                this._closedChannels = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Applies the closed channels described by value. </summary>
    /// <param name="channelList"> The scan list. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplyClosedChannels( string channelList, TimeSpan timeout )
    {
        _ = this.WriteClosedChannels( channelList, timeout );
        if ( this.SupportsClosedChannelsQuery )
        {
            _ = this.QueryClosedChannels();
        }

        if ( this.SupportsClosedChannelsStateQuery )
        {
            _ = this.QueryClosedChannelsState( channelList );
        }

        return this.ClosedChannels;
    }

    /// <summary> Gets the closed channels query command. </summary>
    /// <remarks> :ROUT:CLOS? </remarks>
    /// <value> The closed channels query command. </value>
    protected virtual string ClosedChannelsQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets the supports closed channels query. </summary>
    /// <value> The supports closed channels query. </value>
    public bool SupportsClosedChannelsQuery => !string.IsNullOrWhiteSpace( this.ClosedChannelsQueryCommand );

    /// <summary> Queries closed channels. </summary>
    /// <returns> The closed channels. </returns>
    public string? QueryClosedChannels()
    {
        this.ClosedChannels = this.Session.QueryTrimEnd( this.ClosedChannels ?? string.Empty, this.ClosedChannelsQueryCommand );
        return this.ClosedChannels;
    }

    /// <summary> Gets or sets the closed channels command format. </summary>
    /// <remarks> :ROUT:CLOS {0} </remarks>
    /// <value> The closed channels command format. </value>
    protected virtual string ClosedChannelsCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes a closed channels. </summary>
    /// <param name="channelList"> The scan list. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? WriteClosedChannels( string channelList, TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( this.ClosedChannelsCommandFormat ) )
            _ = this.Session.WriteLine( $"{string.Format( this.ClosedChannelsCommandFormat, channelList )}; {this.Session.OperationCompleteCommand}" );

        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
        ServiceRequests statusByte = this.Session.AwaitOperationCompletion( timeout ).Status;
        this.Session.ThrowDeviceExceptionIfError( statusByte );

        // set to nothing to indicate that the value is not known -- requires reading.
        this.ClosedChannel = null;
        this.ClosedChannelsState = null;
        this.OpenChannels = null;
        this.OpenChannelsState = null;
        this.ClosedChannels = channelList;
        return this.ClosedChannels;
    }

    #endregion

    #region " open channels "

    /// <summary> Builds channel list. </summary>
    /// <param name="slotNumber">    The slot number. </param>
    /// <param name="channelsState"> State of the channels. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string BuildChannelList( int slotNumber, string channelsState )
    {
        ChannelListBuilder builder = new();
        if ( !string.IsNullOrWhiteSpace( channelsState ) )
        {
            Queue<string> q = new( channelsState.Split( ',' ) );
            int relayNumber = 0;
            while ( q.Any() )
            {
                relayNumber += 1;
                if ( q.Dequeue() == "1" )
                {
                    builder.AddChannel( slotNumber, relayNumber );
                }
            }
        }

        return builder.ToString();
    }

    /// <summary> Builds channel list. </summary>
    /// <param name="channelList">   List of channels. </param>
    /// <param name="channelsState"> State of the channels. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string BuildChannelList( string channelList, string channelsState )
    {
        ChannelListBuilder builder = new();
        if ( !string.IsNullOrWhiteSpace( channelsState ) )
        {
            Queue<string> channels = new( channelList.TrimStart( "(@".ToCharArray() ).TrimEnd( ')' ).Split( ',' ) );
            Queue<string> states = new( channelsState.Split( ',' ) );
            while ( states.Any() && channels.Any() )
            {
                if ( states.Dequeue() == "1" )
                {
                    builder.AddChannel( channels.Dequeue() );
                }
                else
                {
                    _ = channels.Dequeue();
                }
            }
        }

        return builder.ToString();
    }

    /// <summary> State of the open channels. </summary>
    private string? _openChannelsState;

    /// <summary> Gets or sets the Open channels state. </summary>
    /// <remarks> Nothing is not set. </remarks>
    /// <value> The Open channels state. </value>
    public string? OpenChannelsState
    {
        get => this._openChannelsState;

        protected set
        {
            if ( !string.Equals( value, this.OpenChannelsState, StringComparison.Ordinal ) )
            {
                this._openChannelsState = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The open channels checked. </summary>
    private string? _openChannelsChecked;

    /// <summary> Gets or sets the Open channels Checked. </summary>
    /// <remarks> Nothing is not set. </remarks>
    /// <value> The Open channels Checked. </value>
    public string? OpenChannelsChecked
    {
        get => this._openChannelsChecked;

        protected set
        {
            if ( !string.Equals( value, this.OpenChannelsChecked, StringComparison.Ordinal ) )
            {
                this._openChannelsChecked = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the Open channels query command. </summary>
    /// <remarks> :ROUT:CLOS? {0} </remarks>
    /// <value> The Open channels query command. </value>
    protected virtual string OpenChannelsStateQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets the supports open channels state query. </summary>
    /// <value> The supports open channels sate query. </value>
    public bool SupportsOpenChannelsStateQuery => !string.IsNullOrWhiteSpace( this.OpenChannelsStateQueryCommand );

    /// <summary> Queries Open channels state. </summary>
    /// <param name="channelList"> List of Channel. </param>
    /// <returns> The Open channels state. </returns>
    public string? QueryOpenChannelsState( string channelList )
    {
        this.OpenChannelsState = this.Session.QueryTrimEnd( this.OpenChannelsState ?? string.Empty, string.Format( this.OpenChannelsStateQueryCommand, channelList ) );
        this.OpenChannelsChecked = channelList;
        return this.OpenChannelsState;
    }

    /// <summary> The open channels. </summary>
    private string? _openChannels;

    /// <summary> Gets or sets the Open channels. </summary>
    /// <remarks> Nothing is not set. </remarks>
    /// <value> The Open channels. </value>
    public string? OpenChannels
    {
        get => this._openChannels;

        protected set
        {
            if ( !string.Equals( value, this.OpenChannels, StringComparison.Ordinal ) )
            {
                this._openChannels = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Applies the Open channels described by value. </summary>
    /// <param name="channelList"> The scan list. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplyOpenChannels( string channelList, TimeSpan timeout )
    {
        _ = this.WriteOpenChannels( channelList, timeout );
        if ( this.SupportsOpenChannelsQuery )
        {
            _ = this.QueryOpenChannels();
        }

        if ( this.SupportsOpenChannelsStateQuery )
        {
            _ = this.QueryOpenChannelsState( channelList );
        }

        return this.OpenChannels;
    }

    /// <summary> Gets the Open channels query command. </summary>
    /// <remarks> :ROUT:OPEN? </remarks>
    /// <value> The Open channels query command. </value>
    protected virtual string OpenChannelsQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets the supports open channels query. </summary>
    /// <value> The supports open channels query. </value>
    public bool SupportsOpenChannelsQuery => !string.IsNullOrWhiteSpace( this.OpenChannelsQueryCommand );

    /// <summary> Queries Open channels. </summary>
    /// <returns> The Open channels. </returns>
    public string? QueryOpenChannels()
    {
        this.OpenChannels = this.Session.QueryTrimEnd( this.OpenChannels ?? string.Empty, this.OpenChannelsQueryCommand );
        return this.OpenChannels;
    }

    /// <summary> Gets or sets the open channels command format. </summary>
    /// <remarks> :ROUT:OPEN {0} </remarks>
    /// <value> The open channels command format. </value>
    protected virtual string OpenChannelsCommandFormat { get; set; } = string.Empty;

    /// <summary> Opens the specified channels in the list. </summary>
    /// <param name="channelList"> List of channels. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? WriteOpenChannels( string channelList, TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( this.OpenChannelsCommandFormat ) )
            _ = this.Session.WriteLine( $"{string.Format( this.OpenChannelsCommandFormat, channelList )}; {this.Session.OperationCompleteCommand}" );

        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
        ServiceRequests statusByte = this.Session.AwaitOperationCompletion( timeout ).Status;
        this.Session.ThrowDeviceExceptionIfError( statusByte );

        // set to nothing to indicate that the value is not known -- requires reading.
        this.ClosedChannel = null;
        this.ClosedChannels = null;
        this.ClosedChannelsState = null;
        this.OpenChannelsState = null;
        this.OpenChannels = channelList;
        return this.OpenChannels;
    }

    #endregion

    #region " open all channels "

    /// <summary> Gets or sets the open channels command. </summary>
    /// <value> The open channels command. </value>
    protected virtual string OpenChannelsCommand { get; set; } = string.Empty;

    /// <summary>
    /// Applies the open all command, wait for timeout and read back the closed channels.
    /// </summary>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplyOpenAll( TimeSpan timeout )
    {
        _ = this.WriteOpenAll( timeout );
        return this.QueryOpenChannels();
    }

    /// <summary> Opens all channels. </summary>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? WriteOpenAll( TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( this.OpenChannelsCommand ) )
            _ = this.Session.WriteLine( $"{this.OpenChannelsCommand}; {this.Session.OperationCompleteCommand}" );

        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
        ServiceRequests statusByte = this.Session.AwaitOperationCompletion( timeout ).Status;
        this.Session.ThrowDeviceExceptionIfError( statusByte );

        // set to nothing to indicate that the value is not known -- requires reading.
        this.ClosedChannel = null;
        this.ClosedChannels = null;
        this.ClosedChannelsState = null;
        this.OpenChannels = null;
        this.OpenChannelsState = null;
        return this.OpenChannels;
    }

    #endregion

    #region " channel pattern = memory scans "

    /// <summary> Gets or sets the recall channel pattern command format. </summary>
    /// <value> The recall channel pattern command format. </value>
    protected virtual string RecallChannelPatternCommandFormat { get; set; } = string.Empty;

    /// <summary> Recalls channel pattern from a memory location. </summary>
    /// <param name="memoryLocation"> Specifies a memory location between 1 and 100. </param>
    /// <param name="timeout">        The timeout. </param>
    public void RecallChannelPattern( int memoryLocation, TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( this.RecallChannelPatternCommandFormat ) )
        {
            _ = this.Session.WriteLine( $"{string.Format( this.RecallChannelPatternCommandFormat, memoryLocation )}; {this.Session.OperationCompleteCommand}" );
            _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
            ServiceRequests statusByte = this.Session.AwaitOperationCompletion( timeout ).Status;
            this.Session.ThrowDeviceExceptionIfError( statusByte );
        }
    }

    /// <summary> Gets or sets the save channel pattern command format. </summary>
    /// <value> The save channel pattern command format. </value>
    protected virtual string SaveChannelPatternCommandFormat { get; set; } = string.Empty;

    /// <summary> Saves existing channel pattern into a memory location. </summary>
    /// <param name="memoryLocation"> Specifies a memory location between 1 and 100. </param>
    /// <param name="timeout">        The timeout. </param>
    public void SaveChannelPattern( int memoryLocation, TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( this.SaveChannelPatternCommandFormat ) )
        {
            _ = this.Session.WriteLine( $"{string.Format( this.SaveChannelPatternCommandFormat, memoryLocation )}; {this.Session.OperationCompleteCommand}" );
            _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
            ServiceRequests statusByte = this.Session.AwaitOperationCompletion( timeout ).Status;
            this.Session.ThrowDeviceExceptionIfError( statusByte );
        }
    }

    /// <summary> Saves a channel list to a memory item. </summary>
    /// <param name="channelList">    List of channels. </param>
    /// <param name="memoryLocation"> Specifies a memory location between 1 and 100. </param>
    /// <param name="timeout">        The timeout. </param>
    /// <returns> The memory location. </returns>
    public int SaveChannelPattern( string channelList, int memoryLocation, TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( channelList ) )
        {
            _ = this.WriteClosedChannels( channelList, timeout );
            this.SaveChannelPattern( memoryLocation, timeout );
            _ = this.WriteOpenAll( timeout );
        }

        return memoryLocation;
    }

    /// <summary>
    /// Gets or sets the one-based location of the first memory location of the default channel
    /// pattern set.
    /// </summary>
    /// <value> The first memory location of the default channel pattern set. </value>
    public int FirstMemoryLocation { get; private set; }

    /// <summary>
    /// Gets or sets the one-based location of the memory location of the default channel pattern set.
    /// </summary>
    /// <value> The last automatic scan index. </value>
    public int LastMemoryLocation { get; private set; }

    /// <summary> Initializes the memory locations. </summary>
    public void InitializeMemoryLocation()
    {
        this.FirstMemoryLocation = 0;
        this.LastMemoryLocation = 0;
    }

    /// <summary>
    /// Adds a channel list to the <see cref="LastMemoryLocation">+1: first available memory
    /// location</see>.
    /// </summary>
    /// <param name="channelList"> List of channels. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns> The new memory location. </returns>
    public int MemorizeChannelPattern( string channelList, TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( channelList ) )
        {
            this.LastMemoryLocation += 1;
            if ( this.LastMemoryLocation == 1 )
            {
                this.FirstMemoryLocation = this.LastMemoryLocation;
            }

            return this.SaveChannelPattern( channelList, this.LastMemoryLocation, timeout );
        }

        return this.LastMemoryLocation;
    }

    #endregion

    #region " scan list "

    /// <summary> Gets or sets the scan list persists after toggling power. </summary>
    /// <remarks>
    /// If the scan lists persists after toggling power, the scan list initial values cannot be
    /// checked.
    /// </remarks>
    /// <value> The scan list persists. </value>
    public virtual bool ScanListPersists { get; set; }

    /// <summary> List of scans. </summary>
    private string? _scanList;

    /// <summary> Gets or sets the cached Scan List. </summary>
    /// <value> A List of scans. </value>
    public string? ScanList
    {
        get => this._scanList;

        protected set
        {
            if ( string.IsNullOrWhiteSpace( value ) )
                value = string.Empty;
            if ( !string.Equals( value, this.ScanList, StringComparison.Ordinal ) )
            {
                this._scanList = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Scan List. </summary>
    /// <param name="value"> The scan list. </param>
    /// <returns> A List of scans. </returns>
    public string? ApplyScanList( string value )
    {
        _ = this.WriteScanList( value );
        return this.QueryScanList();
    }

    /// <summary> Gets or sets the scan list command query. </summary>
    /// <value> The scan list query command. </value>
    protected virtual string ScanListQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Scan List. Also sets the <see cref="ScanList">Route on</see> sentinel.
    /// </summary>
    /// <returns> A List of scans. </returns>
    public string? QueryScanList()
    {
        this.ScanList = this.Session.QueryTrimEnd( this.ScanList ?? string.Empty, this.ScanListQueryCommand );
        return this.ScanList;
    }

    /// <summary> Gets or sets the scan list command format. </summary>
    /// <value> The scan list command format. </value>
    protected virtual string ScanListCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Scan List. Does not read back from the instrument. </summary>
    /// <param name="value"> The scan list. </param>
    /// <returns> A List of scans. </returns>
    public string? WriteScanList( string value )
    {
        _ = this.Session.WriteLine( this.ScanListCommandFormat, value );
        this.ScanList = value;
        return this.ScanList;
    }

    #endregion

    #region " scan list function "

    /// <summary> List of scans. </summary>
    private string? _scanListFunction;

    /// <summary> Gets or sets the cached Scan List. </summary>
    /// <value> A List of scans. </value>
    public string? ScanListFunction
    {
        get => this._scanListFunction;

        protected set
        {
            if ( string.IsNullOrWhiteSpace( value ) )
                value = string.Empty;
            if ( !string.Equals( value, this.ScanListFunction, StringComparison.Ordinal ) )
            {
                this._scanListFunction = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Applies the scan list function described by value. </summary>
    /// <param name="value">         The scan list. </param>
    /// <param name="functionMode">  The function mode. </param>
    /// <param name="functionModes"> The function modes. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplyScanListFunction( string value, SenseFunctionModes functionMode, Pith.EnumReadWriteCollection functionModes )
    {
        _ = this.WriteScanListFunction( value, functionMode, functionModes );
        return this.QueryScanListFunction();
    }

    /// <summary> Gets the scan list function command query. </summary>
    /// <value> The scan list query command. </value>
    protected virtual string ScanListFunctionQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Scan List function. Also sets the <see cref="ScanList">Route on</see> sentinel.
    /// </summary>
    /// <returns> A List of scans. </returns>
    public string? QueryScanListFunction()
    {
        this.ScanList = this.Session.QueryTrimEnd( this.ScanList ?? string.Empty, this.ScanListQueryCommand );
        return $"{this.ScanList},{this.ScanListFunction}";
    }

    /// <summary> Gets the scan list function command format. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The scan list command format. </value>
    protected virtual string ScanListFunctionCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Scan List. Does not read back from the instrument. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="value">         The scan list. </param>
    /// <param name="functionMode">  The function mode. </param>
    /// <param name="functionModes"> The function modes. </param>
    /// <returns> A List of scans. </returns>
    public string? WriteScanListFunction( string value, SenseFunctionModes functionMode, Pith.EnumReadWriteCollection functionModes )
    {
        if ( functionModes is null ) throw new ArgumentNullException( nameof( functionModes ) );
        _ = this.Session.WriteLine( this.ScanListFunctionCommandFormat, value, functionModes.SelectItem( ( long ) functionMode ).WriteValue );
        this.ScanListFunction = functionModes.SelectItem( ( long ) functionMode ).WriteValue;
        this.ScanList = value;
        return $"{this.ScanList},{this.ScanListFunction}";
    }

    #endregion

    #region " selected scan list type "

    /// <summary> Define function mode read writes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="scanListTypeReadWrites"> The scan list type read writes. </param>
    public static void DefineScanListTypeReadWrites( Pith.EnumReadWriteCollection scanListTypeReadWrites )
    {
        if ( scanListTypeReadWrites is null ) throw new ArgumentNullException( nameof( scanListTypeReadWrites ) );
        scanListTypeReadWrites.Clear();
        foreach ( ScanListType scanListType in Enum.GetValues( typeof( ScanListType ) ) )
            scanListTypeReadWrites.Add( scanListType );
    }

    /// <summary> Define function mode read writes. </summary>
    protected virtual void DefineScanListTypeReadWrites()
    {
        this._scanListTypeReadWrites = [];
        DefineScanListTypeReadWrites( this.ScanListTypeReadWrites );
    }

    /// <summary> The scan list type read writes. </summary>
    private Pith.EnumReadWriteCollection? _scanListTypeReadWrites;

    /// <summary> Gets a dictionary of Sense function mode parses. </summary>
    /// <value> A Dictionary of Sense function mode parses. </value>
    public Pith.EnumReadWriteCollection ScanListTypeReadWrites
    {
        get
        {
            if ( this._scanListTypeReadWrites is null )
            {
                this.DefineScanListTypeReadWrites();
            }

            return this._scanListTypeReadWrites!;
        }
    }

    /// <summary> Type of the scan list. </summary>
    private ScanListType _scanListType;

    /// <summary>
    /// Gets or sets the supported Function Modes. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> The supported Sense function modes. </value>
    public ScanListType ScanListType
    {
        get => this._scanListType;
        set
        {
            if ( !this.ScanListType.Equals( value ) )
            {
                this._scanListType = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> List of scans. </summary>
    private string? _selectedScanListType;

    /// <summary> Gets or sets the cached Selected Scan List Type. </summary>
    /// <value> A List of scans. </value>
    public string? SelectedScanListType
    {
        get => this._selectedScanListType;

        protected set
        {
            if ( string.IsNullOrWhiteSpace( value ) ) value = string.Empty;
            if ( !string.Equals( value, this.SelectedScanListType, StringComparison.Ordinal ) )
            {
                this._selectedScanListType = value;
                this.ScanListType = ( ScanListType ) ( int ) this.ScanListTypeReadWrites.SelectItem( value! ).EnumValue;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Selected Scan List Type. </summary>
    /// <param name="value"> The Selected Scan List Type. </param>
    /// <returns> A List of scans. </returns>
    public ScanListType ApplySelectedScanListType( ScanListType value )
    {
        _ = this.WriteSelectedScanListType( value );
        _ = this.QuerySelectedScanListType();
        return this.ScanListType;
    }

    /// <summary> Writes and reads back the Selected Scan List Type. </summary>
    /// <param name="value"> The Selected Scan List Type. </param>
    /// <returns> A List of scans. </returns>
    public string? ApplySelectedScanListType( string value )
    {
        _ = this.WriteSelectedScanListType( value );
        return this.QuerySelectedScanListType();
    }

    /// <summary> Gets or sets the Selected Scan List Type command query. </summary>
    /// <value> The Selected Scan List Type query command. </value>
    protected virtual string SelectedScanListTypeQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Selected Scan List Type. Also sets the <see cref="SelectedScanListType">Route
    /// on</see> sentinel.
    /// </summary>
    /// <returns> A List of scans. </returns>
    public string? QuerySelectedScanListType()
    {
        this.SelectedScanListType = this.Session.QueryTrimEnd( this.SelectedScanListType ?? string.Empty, this.SelectedScanListTypeQueryCommand );
        return this.SelectedScanListType;
    }

    /// <summary> Gets or sets the Selected Scan List Type command format. </summary>
    /// <value> The Selected Scan List Type command format. </value>
    protected virtual string SelectedScanListTypeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Selected Scan List Type. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> The Selected Scan List Type. </param>
    /// <returns> A List of scans. </returns>
    public string? WriteSelectedScanListType( string value )
    {
        _ = this.Session.WriteLine( this.SelectedScanListTypeCommandFormat, value );
        this.SelectedScanListType = value;
        return this.SelectedScanListType;
    }

    /// <summary>
    /// Writes the Selected Scan List Type. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> The Selected Scan List Type. </param>
    /// <returns> A List of scans. </returns>
    public virtual ScanListType WriteSelectedScanListType( ScanListType value )
    {
        this.SelectedScanListType = this.WriteSelectedScanListType( this.ScanListTypeReadWrites.SelectItem( ( long ) value ).WriteValue );
        return this.ScanListType;
    }

    #endregion

    #region " slot card type "

    /// <summary> Gets or sets the slot card type query command format. </summary>
    /// <value> The slot card type query command format. </value>
    protected virtual string SlotCardTypeQueryCommandFormat { get; set; } = string.Empty;

    /// <summary> Gets or sets a list of types of the slot cards. </summary>
    /// <value> A list of types of the slot cards. </value>
    private IDictionary<int, string>? SlotCardTypes { get; set; }

    /// <summary> Slot card type. </summary>
    /// <param name="slotNumber"> The slot number. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? SlotCardType( int slotNumber )
    {
        return this.SlotCardTypes?.ContainsKey( slotNumber ) == true ? this.SlotCardTypes[slotNumber] : string.Empty;
    }

    /// <summary> Applies the card type. </summary>
    /// <param name="cardNumber"> The card number. </param>
    /// <param name="cardType">   Type of the card. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplySlotCardType( int cardNumber, string cardType )
    {
        _ = this.WriteSlotCardType( cardNumber, cardType );
        return this.QuerySlotCardType( cardNumber );
    }

    /// <summary> Queries the Slot Card Type. </summary>
    /// <param name="slotNumber"> The slot number. </param>
    /// <returns> A Slot Card Type. </returns>
    public string? QuerySlotCardType( int slotNumber )
    {
        string? value = string.IsNullOrWhiteSpace( this.SlotCardTypeQueryCommandFormat )
            ? string.Empty
            : this.Session.QueryTrimEnd( "", string.Format( this.SlotCardTypeQueryCommandFormat, slotNumber ) );

        this.SlotCardTypes ??= new Dictionary<int, string>();
        if ( this.SlotCardTypes.ContainsKey( slotNumber ) )
        {
            _ = this.SlotCardTypes.Remove( slotNumber );
        }

        if ( !string.IsNullOrWhiteSpace( value ) )
        {
            this.SlotCardTypes.Add( slotNumber, value! );
        }

        this.NotifyPropertyChanged( nameof( RouteSubsystemBase.SlotCardType ) );
        return value;
    }

    /// <summary> Gets or sets the slot card type command format. </summary>
    /// <value> The slot card type command format. </value>
    protected virtual string SlotCardTypeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes a slot card type. </summary>
    /// <param name="cardNumber"> The card number. </param>
    /// <param name="cardType">   Type of the card. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string WriteSlotCardType( int cardNumber, string cardType )
    {
        if ( !string.IsNullOrWhiteSpace( this.SlotCardTypeCommandFormat ) )
        {
            _ = this.Session.WriteLine( "", string.Format( this.SlotCardTypeCommandFormat, cardNumber, cardType ) );
        }

        return cardType;
    }

    #endregion

    #region " slot card settling time "

    /// <summary> Gets or sets the slot card settling time query command format. </summary>
    /// <value> The slot card settling time query command format. </value>
    protected virtual string SlotCardSettlingTimeQueryCommandFormat { get; set; } = string.Empty;

    /// <summary> Gets or sets a list of times of the slot card settlings. </summary>
    /// <value> A list of times of the slot card settlings. </value>
    private IDictionary<int, TimeSpan>? SlotCardSettlingTimes { get; set; }

    /// <summary> Slot card settling time. </summary>
    /// <param name="slotNumber"> The slot number. </param>
    /// <returns> A TimeSpan. </returns>
    public TimeSpan SlotCardSettlingTime( int slotNumber )
    {
        TimeSpan ts = TimeSpan.Zero;
        if ( this.SlotCardSettlingTimes?.ContainsKey( slotNumber ) == true )
        {
            ts = this.SlotCardSettlingTimes[slotNumber];
        }

        return ts;
    }

    /// <summary> Applies the slot card settling time. </summary>
    /// <param name="cardNumber">   The card number. </param>
    /// <param name="settlingTime"> The settling time. </param>
    /// <returns> A TimeSpan. </returns>
    public TimeSpan ApplySlotCardSettlingTime( int cardNumber, TimeSpan settlingTime )
    {
        _ = this.WriteSlotCardSettlingTime( cardNumber, settlingTime );
        return this.QuerySlotCardSettlingTime( cardNumber );
    }

    /// <summary> Queries the Slot Card settling time. </summary>
    /// <param name="slotNumber"> The slot number. </param>
    /// <returns> A Slot Card settling time. </returns>
    public TimeSpan QuerySlotCardSettlingTime( int slotNumber )
    {
        TimeSpan ts = TimeSpan.Zero;
        double? value = default;
        if ( !string.IsNullOrWhiteSpace( this.SlotCardSettlingTimeQueryCommandFormat ) )
        {
            value = this.Session.Query( new double?(), string.Format( this.SlotCardSettlingTimeQueryCommandFormat, slotNumber ) );
            ts = value.HasValue ? TimeSpan.FromTicks( ( long ) (TimeSpan.TicksPerSecond * value.Value) ) : TimeSpan.Zero;
        }

        this.SlotCardSettlingTimes ??= new Dictionary<int, TimeSpan>();
        if ( this.SlotCardSettlingTimes.ContainsKey( slotNumber ) )
        {
            _ = this.SlotCardSettlingTimes.Remove( slotNumber );
        }

        if ( value.HasValue )
        {
            this.SlotCardSettlingTimes.Add( slotNumber, ts );
        }

        this.NotifyPropertyChanged( nameof( RouteSubsystemBase.SlotCardSettlingTime ) );
        return ts;
    }

    /// <summary> Gets or sets the slot card settling time command format. </summary>
    /// <value> The slot card settling time command format. </value>
    protected virtual string SlotCardSettlingTimeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes a slot card settling time. </summary>
    /// <param name="cardNumber">   The card number. </param>
    /// <param name="settlingTime"> The settling time. </param>
    /// <returns> A TimeSpan. </returns>
    public TimeSpan WriteSlotCardSettlingTime( int cardNumber, TimeSpan settlingTime )
    {
        return this.WriteSlotCardSettlingTime( cardNumber, settlingTime.TotalSeconds );
    }

    /// <summary> Writes a slot card settling time. </summary>
    /// <param name="cardNumber">   The card number. </param>
    /// <param name="settlingTime"> The settling time. </param>
    /// <returns> A TimeSpan. </returns>
    public TimeSpan WriteSlotCardSettlingTime( int cardNumber, double settlingTime )
    {
        if ( !string.IsNullOrWhiteSpace( this.SlotCardSettlingTimeCommandFormat ) )
        {
            _ = this.Session.WriteLine( settlingTime, string.Format( this.SlotCardSettlingTimeCommandFormat, cardNumber, settlingTime ) );
        }

        return TimeSpan.FromSeconds( settlingTime );
    }

    #endregion
}
/// <summary> Specifies the Scan List Types. </summary>
public enum ScanListType
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None (NONE)" )]
    None,

    /// <summary>   An enum constant representing the internal option. </summary>
    [System.ComponentModel.Description( "Internal (INT)" )]
    Internal,

    /// <summary> An enum constant representing the external option. </summary>
    [System.ComponentModel.Description( "External (EXT)" )]
    External,

    /// <summary> An enum constant representing the ratio option. </summary>
    [System.ComponentModel.Description( "Ratio (RAT)" )]
    Ratio,

    /// <summary> An enum constant representing the delta option. </summary>
    [System.ComponentModel.Description( "Delta (DELT)" )]
    Delta
}
