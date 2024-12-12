using cc.isr.VI.Pith;

namespace cc.isr.VI;

/// <summary> Defines a System Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-01-13 </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
/// </remarks>
[CLSCompliant( false )]
public abstract class ChannelSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">status
    /// Subsystem</see>. </param>
    protected ChannelSubsystemBase( StatusSubsystemBase statusSubsystem ) : base( statusSubsystem ) => this.DefineClearExecutionState();

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        // clear values to force update.
        this._closedChannels = string.Empty;
        this._closedChannelsCaption = string.Empty;
        this.ClosedChannels = string.Empty;
    }

    #endregion

    #region " closed channels "

    /// <summary> The closed channels. </summary>
    private string? _closedChannels;

    /// <summary> Gets or sets the closed channels. </summary>
    /// <value> The closed channels. </value>
    public string? ClosedChannels
    {
        get => this._closedChannels;

        protected set
        {
            if ( base.SetProperty( ref this._closedChannels, value ) )
            {
                this.ClosedChannelsCaption = value;
            }
        }
    }

    /// <summary> The closed channels caption. </summary>
    private string? _closedChannelsCaption;

    /// <summary> Gets or sets the closed channels Caption. </summary>
    /// <value> The closed channels. </value>
    public string? ClosedChannelsCaption
    {
        get => this._closedChannelsCaption;
        protected set => _ = base.SetProperty( ref this._closedChannels, value is null ? "nil" : string.IsNullOrWhiteSpace( value ) ? "all open" : value );
    }

    /// <summary> Applies the closed channels described by value. </summary>
    /// <param name="value">   The scan list. </param>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplyClosedChannels( string value, TimeSpan timeout )
    {
        _ = this.WriteClosedChannels( value, timeout );
        return this.QueryClosedChannels();
    }

    /// <summary> Gets or sets the closed channels query command. </summary>
    /// <remarks> :ROUT:CLOS. </remarks>
    /// <value> The closed channels query command. </value>
    protected virtual string ClosedChannelsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries closed channels. </summary>
    /// <returns> The closed channels. </returns>
    public string? QueryClosedChannels()
    {
        string? value = this.Session.QueryTrimEnd( this.ClosedChannels ?? string.Empty, this.ClosedChannelsQueryCommand );
        this.ClosedChannels = Pith.SessionBase.EqualsNil( value ) ? string.Empty : value;
        return this.ClosedChannels;
    }

    /// <summary> Gets or sets the closed channels command format. </summary>
    /// <remarks> :ROUT:CLOS {0} </remarks>
    /// <value> The closed channels command format. </value>
    protected virtual string ClosedChannelsCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes a closed channels. </summary>
    /// <param name="value">   The scan list. </param>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string WriteClosedChannels( string value, TimeSpan timeout )
    {
        if ( !string.IsNullOrWhiteSpace( this.ClosedChannelsCommandFormat ) )
            _ = this.Session.WriteLine( $"{string.Format( this.ClosedChannelsCommandFormat, value )}; {this.Session.OperationCompleteCommand}" );
        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
        ServiceRequests statusByte = this.Session.AwaitOperationCompletion( timeout ).Status;
        this.Session.ThrowDeviceExceptionIfError( statusByte );
        this.ClosedChannels = value;
        return this.ClosedChannels;
    }

    /// <summary> Gets or sets the open channels command format. </summary>
    /// <remarks> SCPI: :ROUT:OPEN:ALL. </remarks>
    /// <value> The open channels command format. </value>
    protected virtual string OpenChannelsCommandFormat { get; set; } = string.Empty;

    /// <summary> Open the specified channels in the list and read back the closed channels. </summary>
    /// <param name="channelList"> List of channels. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplyOpenChannels( string channelList, TimeSpan timeout )
    {
        _ = this.WriteOpenChannels( channelList, timeout );
        return this.QueryClosedChannels();
    }

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
        this.ClosedChannels = null;
        return null;
    }

    /// <summary> Gets or sets the open channels command. </summary>
    /// <value> The open channels command. </value>
    protected virtual string OpenChannelsCommand { get; set; } = string.Empty;

    /// <summary> Opens all channels and reads back the closed channels. </summary>
    /// <param name="timeout"> The timeout. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string? ApplyOpenAll( TimeSpan timeout )
    {
        _ = this.WriteOpenAll( timeout );
        return this.QueryClosedChannels();
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
        this.ClosedChannels = null;
        return this.ClosedChannels;
    }

    #endregion
}
