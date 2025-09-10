namespace cc.isr.VI.Pith;
public abstract partial class SessionBase
{
    /// <summary> Gets or sets the default Operation service request enable bitmask. </summary>
    /// <remarks>
    /// Exclude message available to prevent handling a service request on messages.
    /// </remarks>
    /// <statusByte> The default Operation service request enable bitmask. </statusByte>
    public ServiceRequests OperationServiceRequestEnableBitmask
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the standard service enable command. </summary>
    /// <remarks>
    /// This command combines the commands for <see cref="ClearActiveState()"/>,
    /// <see cref="ServiceRequestEnableCommandFormat"/> and
    /// <see cref="StandardEventEnableCommandFormat"/>. <para>
    /// SCPI: *CLS; *ESE {0:D}; *SRE {1:D}</para>
    /// </remarks>
    /// <statusByte> The standard service enable command. </statusByte>
    public string? StandardServiceEnableCommand
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the standard service enable command format. </summary>
    /// <remarks>
    /// <see cref="VI.Syntax.Ieee488Syntax.StandardServiceEnableCommandFormat"></see>
    /// This command combines the commands for <see cref="ClearActiveState()"/>,
    /// <see cref="ServiceRequestEnableCommandFormat"/> and
    /// <see cref="StandardEventEnableCommandFormat"/>.
    /// <code>
    /// Lua: _G.status.standard.enable={0} _G.status.request_enable={1}
    /// SCPI: *ESE {0:D}; *SRE {1:D} </code>
    /// </remarks>
    /// <statusByte> The standard service enable command format. </statusByte>
    public string? StandardServiceEnableCommandFormat { get; set; } = Syntax.Ieee488Syntax.StandardServiceEnableCommandFormat;

    /// <summary>
    /// Gets or sets the standard service operation complete enable command format.
    /// </summary>
    /// <remarks>
    /// <see cref="VI.Syntax.Ieee488Syntax.StandardServiceEnableOperationCompleteCommandFormat"> </see>
    /// This command combines the commands for <see cref="ClearActiveState()"/>,
    /// <see cref="ServiceRequestEnableCommandFormat"/>,
    /// <see cref="StandardEventEnableCommandFormat"/>, and <see cref="OperationCompleteCommand"/>.
    /// <code>
    /// Lua: _G.status.reset() _G.waitcomplete() _G.status.standard.enable={0} _G.status.request_enable={1} _G.opc()
    /// SCPI: *CLS; *WAI; *ESE {0:D}; *SRE {1:D}; *OPC </code>
    /// </remarks>
    /// <statusByte> The standard service enable operation complete enable command format. </statusByte>
    public string? StandardServiceEnableOperationCompleteCommandFormat { get; set; } = Syntax.Ieee488Syntax.StandardServiceEnableOperationCompleteCommandFormat;

    /// <summary>
    /// Gets or sets the standard service operation query complete enable command format.
    /// </summary>
    /// <remarks>
    /// <see cref="VI.Syntax.Ieee488Syntax.StandardServiceEnableOperationCompleteCommandFormat"> </see>
    /// This command combines the commands for <see cref="ClearActiveState()"/>,
    /// <see cref="ServiceRequestEnableCommandFormat"/>,
    /// <see cref="StandardEventEnableCommandFormat"/>, and <see cref="OperationCompleteCommand"/>.
    /// <code>
    /// Lua: _G.status.reset() _G.waitcomplete() _G.status.standard.enable={0} _G.status.request_enable={1} _G.waitcomplete() print('1')
    /// SCPI: *CLS; *WAI; *ESE {0:D}; *SRE {1:D}; *OPC? </code>
    /// </remarks>
    /// <statusByte> The standard service enable operation complete enable command format. </statusByte>
    public string? StandardServiceEnableQueryCompleteCommandFormat { get; set; } = Syntax.Ieee488Syntax.StandardServiceEnableQueryCompleteCommandFormat;

    /// <summary>
    /// Applies (Writes and then reads) the standard and service bitmasks to unmask events to let the
    /// device set the Event Summary bit (ESB) <see cref="Pith.ServiceRequests.StandardEventSummary"/> and
    /// Main Summary Status (MSS) upon any of the unmasked SCPI events. Uses *ESE to select
    /// (unmask) the events that set the ESB and *SRE to select (unmask) the events that will set the
    /// Main Summary Status (MSS) bit for requesting service. Does not issue *OPC; Does not define
    /// the default wait complete bitmasks.
    /// </summary>
    /// <remarks> 3475. Add Or VI.Syntax2.ServiceRequests.OperationEvent. </remarks>
    public void ApplyStandardServiceRequestEnableBitmasks()
    {
        this.ApplyStandardServiceRequestEnableBitmasks( this.RegistersBitmasksSettings.StandardEventEnableEventsBitmask,
            this.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );
    }

    /// <summary>
    /// Applies (Writes and then reads) the standard and service bitmasks to unmask events to let the
    /// device set the Event Summary bit (ESB) <see cref="Pith.ServiceRequests.StandardEventSummary"/>
    /// and Main Summary Status (MSS) upon any of the unmasked SCPI events. Uses *ESE to select
    /// (unmask) the events that set the ESB and *SRE to select (unmask) the events that will set the
    /// Main Summary Status (MSS) bit for requesting service. Does not issue *OPC; Does not define
    /// the default wait complete bitmasks.
    /// </summary>
    /// <remarks>   2024-09-16. </remarks>
    /// <param name="standardEventEnableBitmask">   Specifies standard events will issue an SRQ. </param>
    /// <param name="serviceRequestEnableBitmask">  Specifies which status registers will issue an
    ///                                             SRQ. </param>
    public void ApplyStandardServiceRequestEnableBitmasks( int standardEventEnableBitmask, int serviceRequestEnableBitmask )
    {
        string? commandFormat = this.StandardServiceEnableCommandFormat;
        if ( commandFormat is null || string.IsNullOrWhiteSpace( commandFormat ) )

            this.StandardServiceEnableCommand = string.Empty;

        else
        {
            this.SetLastAction( "writing standard and status enable bitmasks following clear execution state" );
            string? command = this.WriteLine( commandFormat, standardEventEnableBitmask, serviceRequestEnableBitmask );

            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );

            this.StandardServiceEnableCommand = command;
            _ = this.QueryServiceRequestEnableBitmask();
            _ = this.QueryStandardEventEnableBitmask();
        }
    }

    /// <summary>   Enables the standard service request events. </summary>
    /// <remarks>   2024-09-17. </remarks>
    public void EnableStandardServiceRequestEvents()
    {
        this.WriteStandardServiceRequestEnableBitmasks( this.RegistersBitmasksSettings.StandardEventEnableEventsBitmask,
            this.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );
    }

    /// <summary>
    /// Writes the standard and service bitmasks to unmask events to let the device set the Event
    /// Summary bit (ESB) <see cref="Pith.ServiceRequests.StandardEventSummary"/> and Main Summary
    /// Status (MSS) upon any of the unmasked SCPI events. Uses *ESE to select (unmask) the events
    /// that set the ESB and *SRE to select (unmask) the events that will set the Main Summary
    /// Status (MSS) bit for requesting service. Does not issue *OPC; Does not define the default wait complete
    /// bitmasks.
    /// </summary>
    /// <remarks>   2024-09-17. </remarks>
    public void WriteStandardServiceRequestEnableBitmasks()
    {
        this.WriteStandardServiceRequestEnableBitmasks( this.RegistersBitmasksSettings.StandardEventEnableEventsBitmask,
            this.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );
    }

    /// <summary>
    /// Writes the standard and service bitmasks to unmask events to let the device set the Event
    /// Summary bit (ESB) <see cref="Pith.ServiceRequests.StandardEventSummary"/> and Main Summary
    /// Status (MSS) upon any of the unmasked SCPI events. Uses *ESE to select (unmask) the events
    /// that set the ESB and *SRE to select (unmask) the events that will set the Main Summary
    /// Status (MSS) bit for requesting service. Does not issue *OPC; Does not define the default wait complete
    /// bitmasks.
    /// </summary>
    /// <remarks>   2024-09-16. </remarks>
    /// <param name="standardEventEnableBitmask">   Specifies standard events will issue an SRQ. </param>
    /// <param name="serviceRequestEnableBitmask">  Specifies which status registers will issue an
    ///                                             SRQ. </param>
    public void WriteStandardServiceRequestEnableBitmasks( int standardEventEnableBitmask, int serviceRequestEnableBitmask )
    {
        string? commandFormat = this.StandardServiceEnableCommandFormat;
        if ( commandFormat is null || string.IsNullOrWhiteSpace( commandFormat ) )
        {
            this.StandardServiceEnableCommand = string.Empty;
        }
        else
        {
            this.SetLastAction( "writing standard and status enable bitmasks" );
            string? command = this.WriteLine( commandFormat, standardEventEnableBitmask, serviceRequestEnableBitmask );
            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );

            this.StandardServiceEnableCommand = command;
            this.ServiceRequestEnableBitmask = ( ServiceRequests ) serviceRequestEnableBitmask;
            this.StandardEventEnableBitmask = ( StandardEvents ) standardEventEnableBitmask;
        }
    }
}
