using cc.isr.Enums;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    /// <summary>
    /// Returns the value of the event status register (ESR) byte plus the description if its
    /// constituent bit.
    /// </summary>
    /// <remarks>   2024-09-03. </remarks>
    /// <param name="value">        Specifies the value that was read from the status register. </param>
    /// <param name="delimiter">    The delimiter. </param>
    /// <returns>   Returns a detailed report of the event status register (ESR) byte. </returns>
    public string BuildStandardEventValueDescription( StandardEvents value, string delimiter = "; " )
    {
        this.StandardEventStatus = value;
        return this.BuildStandardEventValueDescription( delimiter );
    }

    /// <summary>
    /// Returns the value of the event status register (ESR) byte plus the description if its
    /// constituent bit.
    /// </summary>
    /// <remarks>   2024-09-04. </remarks>
    /// <param name="delimiter">    (Optional) The delimiter. </param>
    /// <returns>   Returns a detailed report of the event status register (ESR) byte. </returns>
    public string BuildStandardEventValueDescription( string delimiter = "; " )
    {
        if ( string.IsNullOrWhiteSpace( delimiter ) ) delimiter = "; ";

        System.Text.StringBuilder builder = new();
        foreach ( StandardEvents eventValue in Enum.GetValues( typeof( StandardEvents ) ) )
        {
            if ( eventValue != StandardEvents.None && eventValue != StandardEvents.All && (eventValue & this.StandardEventStatus) != 0 )
            {
                if ( builder.Length > 0 )
                    _ = builder.Append( delimiter );

                _ = builder.Append( eventValue.Description() );
            }
        }

        if ( builder.Length == 0 )
            _ = builder.Append( this.StandardRegisterCaption );
        else
        {
            _ = builder.Append( ")." );
            _ = builder.Insert( 0, $"{this.StandardRegisterCaption} (" );
        }

        return builder.ToString();
    }

    /// <summary> Gets or sets the cached Standard Event enable bit mask. </summary>
    /// <remarks> This property always handles the property changed event because the
    ///           standard register reflects the actual instrument state </remarks>
    /// <value>
    /// <c>null</c> if value is not known; otherwise <see cref="VI.Pith.StandardEvents">Standard
    /// Events</see>.
    /// </value>
    public StandardEvents? StandardEventStatus
    {
        get;
        set
        {
            if ( this.SetProperty( ref field, value ) )
            {
                this.StandardRegisterCaption = value.HasValue
                    ? string.Format( RegisterValueFormat, ( int ) value.Value )
                    : UnknownRegisterValueCaption;
            }
        }
    }

    /// <summary> Gets the standard event status has value. </summary>
    /// <value> The standard event status has value. </value>
    public bool StandardEventStatusHasValue => this.StandardEventStatus.HasValue;

    /// <summary> Gets or sets the standard event query command. </summary>
    /// <remarks> <see cref="VI.Syntax.Ieee488Syntax.StandardEventStatusQueryCommand"></see> </remarks>
    /// <value> The standard event query command. </value>
    public string? StandardEventStatusQueryCommand { get; set; } = Syntax.Ieee488Syntax.StandardEventStatusQueryCommand;

    /// <summary> Queries the Standard Event enable bit mask. </summary>
    /// <returns>
    /// <c>null</c> if value is not known; otherwise <see cref="VI.Pith.StandardEvents">Standard
    /// Events</see>.
    /// </returns>
    public StandardEvents? QueryStandardEventStatus()
    {
        if ( !string.IsNullOrWhiteSpace( this.StandardEventStatusQueryCommand ) )
            this.StandardEventStatus = ( StandardEvents? ) this.Query( 0, this.StandardEventStatusQueryCommand! );

        return this.StandardEventStatus;
    }

    /// <summary> Gets or sets the Standard register caption. </summary>
    /// <value> The Standard register caption. </value>
    public string? StandardRegisterCaption
    {
        get;
        set => _ = base.SetProperty( ref field, value ?? string.Empty );
    } = "0x..";

    /// <summary> Gets or sets the bitmask to enable standard event register events. </summary>
    /// <value> The bitmask to enable standard event register events [253, 0xFD]. </value>
    public StandardEvents StandardEventEnableEventsBitmask
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    } = StandardEvents.All & ~StandardEvents.RequestControl;

    /// <summary> Gets or sets the cached Standard Event enable bit mask. </summary>
    /// <value>
    /// <c>null</c> if value is not known; otherwise <see cref="VI.Pith.StandardEvents">Standard
    /// Events</see>.
    /// </value>
    public StandardEvents? StandardEventEnableBitmask
    {
        get;
        protected set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>
    /// Apply (WriteEnum and then read) bitmask setting the device to turn on the Event Summary Bit (ESM)
    /// upon any of the SCPI events unmasked by the bitmask. Uses *ERE to select (unmask) the events
    /// that will set the ESB.
    /// </summary>
    /// <param name="bitmask"> The bitmask; zero to disable all events. </param>
    public void ApplyStandardEventEnableBitmask( int bitmask )
    {
        this.WriteStandardEventEnableBitmask( bitmask );
        _ = this.QueryStandardEventEnableBitmask();
    }

    /// <summary>
    /// Apply (WriteEnum and then read) bitmask setting the device to turn on the Event Summary Bit (ESM)
    /// upon any of the SCPI events unmasked by the bitmask. Uses *ERE to select (unmask) the events
    /// that will set the ESB.
    /// </summary>
    /// <param name="commandFormat"> The standard event enable command format. </param>
    /// <param name="bitmask">       The bitmask; zero to disable all events. </param>
    public void ApplyStandardEventEnableBitmask( string commandFormat, int bitmask )
    {
        this.StandardEventEnableCommandFormat = commandFormat;
        this.ApplyStandardEventEnableBitmask( bitmask );
    }

    /// <summary> Gets the standard event enable query command. </summary>
    /// <remarks>
    /// SCPI: "*ESE?".
    /// <see cref="VI.Syntax.Ieee488Syntax.StandardEventEnableQueryCommand"> </see>
    /// </remarks>
    /// <value> The standard event enable query command. </value>
    public string StandardEventEnableQueryCommand { get; set; } = Syntax.Ieee488Syntax.StandardEventEnableQueryCommand;

    /// <summary> Queries the Standard Event enable bit mask. </summary>
    /// <returns>
    /// <c>null</c> if value is not known; otherwise <see cref="VI.Pith.StandardEvents">Standard
    /// Events</see>.
    /// </returns>
    public StandardEvents? QueryStandardEventEnableBitmask()
    {
        if ( !string.IsNullOrWhiteSpace( this.StandardEventEnableQueryCommand ) )
        {
            this.SetLastAction( "querying standard register enable bitmask" );
            this.StandardEventEnableBitmask = ( StandardEvents ) this.Query( 0, this.StandardEventEnableQueryCommand );
        }

        return this.StandardEventEnableBitmask;
    }

    /// <summary> Gets the supports standard event enable query. </summary>
    /// <value> The supports standard event enable query. </value>
    public bool SupportsStandardEventEnableQuery => !string.IsNullOrWhiteSpace( this.StandardEventEnableQueryCommand );

    /// <summary> Gets or sets the standard event enable command format. </summary>
    /// <remarks>
    /// SCPI: "*ESE {0:D}".
    /// <see cref="VI.Syntax.Ieee488Syntax.StandardEventEnableCommandFormat"> </see>
    /// </remarks>
    /// <value> The standard event enable command format. </value>
    public string StandardEventEnableCommandFormat { get; set; } = Syntax.Ieee488Syntax.StandardEventEnableCommandFormat;

    /// <summary> Writes the Standard Event enable bitmask. </summary>
    /// <param name="bitmask"> The bitmask; zero to disable all events. </param>
    public void WriteStandardEventEnableBitmask( int bitmask )
    {
        if ( string.IsNullOrWhiteSpace( this.StandardEventEnableCommandFormat ) )
            this.StandardEventEnableBitmask = StandardEvents.None;
        else
        {
            _ = this.WriteLine( this.StandardEventEnableCommandFormat, bitmask );
            this.StandardEventEnableBitmask = ( StandardEvents ) bitmask;
        }
    }

    /// <summary> Sets the subsystem registers to their reset state. </summary>
    private void ResetRegistersKnownState()
    {
        this.StandardEventStatus = new StandardEvents?();
    }

    /// <summary> Reads the event registers based on the service request register status. </summary>
    public void ReadStandardEventRegisters()
    {
        if ( this.HasStandardEvent )
            _ = this.QueryStandardEventStatus();
    }
}
