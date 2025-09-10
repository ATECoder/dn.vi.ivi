namespace cc.isr.VI;

public partial class TriggerSubsystemBase
{
    #region " trigger layer bypass mode "

    /// <summary> Defines trigger layer bypass read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineTriggerLayerBypassReadWrites()
    {
        this.TriggerLayerBypassModeReadWrites = new();
        foreach ( TriggerLayerBypassModes enumValue in Enum.GetValues( typeof( TriggerLayerBypassModes ) ) )
            this.TriggerLayerBypassModeReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of trigger layer bypass mode parses. </summary>
    /// <value> A Dictionary of trigger layer bypass mode parses. </value>
    public Pith.EnumReadWriteCollection TriggerLayerBypassModeReadWrites { get; private set; } = [];

    /// <summary>
    /// Gets or sets the supported Function Mode. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> The supported trigger layer bypass modes. </value>
    public TriggerLayerBypassModes SupportedTriggerLayerBypassModes
    {
        get;
        set
        {
            if ( !this.SupportedTriggerLayerBypassModes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Returns true if bypassing the source trigger using
    /// <see cref="TriggerLayerBypassModes.Acceptor"/> .
    /// </summary>
    /// <value> True if trigger layer is bypassed. </value>
    public bool IsTriggerLayerBypass => this.TriggerLayerBypassMode == TriggerLayerBypassModes.Acceptor;

    /// <summary> Gets or sets the cached trigger layer bypass mode. </summary>
    /// <value>
    /// The <see cref="TriggerLayerBypassMode">trigger layer bypass mode</see> or none if not set or
    /// unknown.
    /// </value>
    public TriggerLayerBypassModes? TriggerLayerBypassMode
    {
        get;

        protected set
        {
            if ( !this.TriggerLayerBypassMode.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.IsTriggerLayerBypass ) );
            }
        }
    } = TriggerLayerBypassModes.None;

    /// <summary> Writes and reads back the trigger layer bypass mode. </summary>
    /// <param name="value"> The  trigger layer bypass mode. </param>
    /// <returns>
    /// The <see cref="TriggerLayerBypassMode">source trigger layer bypass mode</see> or none if
    /// unknown.
    /// </returns>
    public TriggerLayerBypassModes? ApplyTriggerLayerBypassMode( TriggerLayerBypassModes value )
    {
        _ = this.WriteTriggerLayerBypassMode( value );
        return this.QueryTriggerLayerBypassMode();
    }

    /// <summary> Gets or sets the trigger layer bypass mode query command. </summary>
    /// <value> The trigger layer bypass mode query command. </value>
    protected virtual string TriggerLayerBypassModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the trigger layer bypass mode. </summary>
    /// <returns>
    /// The <see cref="TriggerLayerBypassMode">trigger layer bypass mode</see> or none if unknown.
    /// </returns>
    public TriggerLayerBypassModes? QueryTriggerLayerBypassMode()
    {
        this.TriggerLayerBypassMode = this.Session.Query( this.TriggerLayerBypassMode.GetValueOrDefault( TriggerLayerBypassModes.None ), this.TriggerLayerBypassModeReadWrites,
            this.TriggerLayerBypassModeQueryCommand );
        return this.TriggerLayerBypassMode;
    }

    /// <summary> Gets or sets the trigger layer bypass mode command format. </summary>
    /// <value> The trigger layer bypass mode command format. </value>
    protected virtual string TriggerLayerBypassModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the trigger layer bypass mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The trigger layer bypass mode. </param>
    /// <returns>
    /// The <see cref="TriggerLayerBypassMode">trigger layer bypass mode</see> or none if unknown.
    /// </returns>
    public TriggerLayerBypassModes? WriteTriggerLayerBypassMode( TriggerLayerBypassModes value )
    {
        this.TriggerLayerBypassMode = this.Session.Write( value, this.TriggerLayerBypassModeCommandFormat, this.TriggerLayerBypassModeReadWrites );
        return this.TriggerLayerBypassMode;
    }

    #endregion

    #region " trigger source "

    /// <summary> Define trigger source read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineTriggerSourceReadWrites()
    {
        this.TriggerSourceReadWrites = new();
        foreach ( TriggerSources enumValue in Enum.GetValues( typeof( TriggerSources ) ) )
            this.TriggerSourceReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of trigger source parses. </summary>
    /// <value> A Dictionary of trigger source parses. </value>
    public Pith.EnumReadWriteCollection TriggerSourceReadWrites { get; private set; }

    /// <summary> Gets or sets the supported trigger sources. </summary>
    /// <value> The supported trigger sources. </value>
    public TriggerSources SupportedTriggerSources
    {
        get;
        set
        {
            if ( !this.SupportedTriggerSources.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached source TriggerSource. </summary>
    /// <value>
    /// The <see cref="TriggerSource">source Trigger Source</see> or none if not set or unknown.
    /// </value>
    public TriggerSources? TriggerSource
    {
        get;

        protected set
        {
            if ( !this.TriggerSource.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the source Trigger Source. </summary>
    /// <param name="value"> The  Source Trigger Source. </param>
    /// <returns>
    /// The <see cref="TriggerSource">source Trigger Source</see> or none if unknown.
    /// </returns>
    public TriggerSources? ApplyTriggerSource( TriggerSources value )
    {
        _ = this.WriteTriggerSource( value );
        return this.QueryTriggerSource();
    }

    /// <summary> Gets or sets the Trigger source query command. </summary>
    /// <remarks> SCPI: ":TRIG:SOUR?". </remarks>
    /// <value> The Trigger source query command. </value>
    protected virtual string TriggerSourceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the trigger source. </summary>
    /// <returns> The <see cref="TriggerSource">trigger source</see> or none if unknown. </returns>
    public TriggerSources? QueryTriggerSource()
    {
        this.TriggerSource = this.Session.Query( this.TriggerSource.GetValueOrDefault( TriggerSources.None ), this.TriggerSourceReadWrites,
            this.TriggerSourceQueryCommand );
        return this.TriggerSource;
    }

    /// <summary> Gets or sets the Trigger source command format. </summary>
    /// <remarks> SCPI: "{0}". </remarks>
    /// <value> The write Trigger source command format. </value>
    protected virtual string TriggerSourceCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Trigger Source without reading back the value from the device. </summary>
    /// <param name="value"> The Trigger Source. </param>
    /// <returns> The <see cref="TriggerSource">Trigger Source</see> or none if unknown. </returns>
    public TriggerSources? WriteTriggerSource( TriggerSources value )
    {
        this.TriggerSource = this.Session.Write( value, this.TriggerSourceCommandFormat, this.TriggerSourceReadWrites );
        return this.TriggerSource;
    }

    #endregion

    #region " trigger event "

    /// <summary> Define trigger event read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineTriggerEventReadWrites()
    {
        this.TriggerEventReadWrites = new();
        foreach ( TriggerEvents enumValue in Enum.GetValues( typeof( TriggerEvents ) ) )
            this.TriggerEventReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of trigger Event parses. </summary>
    /// <value> A Dictionary of trigger Event parses. </value>
    public Pith.EnumReadWriteCollection TriggerEventReadWrites { get; private set; }

    /// <summary> Gets or sets the supported trigger Events. </summary>
    /// <value> The supported trigger Events. </value>
    public TriggerEvents SupportedTriggerEvents
    {
        get;
        set
        {
            if ( !this.SupportedTriggerEvents.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached Event TriggerEvent. </summary>
    /// <value>
    /// The <see cref="TriggerEvent">Event Trigger Event</see> or none if not set or unknown.
    /// </value>
    public TriggerEvents? TriggerEvent
    {
        get;

        protected set
        {
            if ( !this.TriggerEvent.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Event Trigger Event. </summary>
    /// <param name="value"> The  Event Trigger Event. </param>
    /// <returns> The <see cref="TriggerEvent">Event Trigger Event</see> or none if unknown. </returns>
    public TriggerEvents? ApplyTriggerEvent( TriggerEvents value )
    {
        _ = this.WriteTriggerEvent( value );
        return this.QueryTriggerEvent();
    }

    /// <summary> Gets or sets the Trigger Event query command. </summary>
    /// <remarks> SCPI: ":TRIG:SOUR?". </remarks>
    /// <value> The Trigger Event query command. </value>
    protected virtual string TriggerEventQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the trigger Event. </summary>
    /// <returns> The <see cref="TriggerEvent">trigger Event</see> or none if unknown. </returns>
    public TriggerEvents? QueryTriggerEvent()
    {
        this.TriggerEvent = this.Session.Query( this.TriggerEvent.GetValueOrDefault( TriggerEvents.None ), this.TriggerEventReadWrites, this.TriggerEventQueryCommand );
        return this.TriggerEvent;
    }

    /// <summary> Gets or sets the Trigger Event command format. </summary>
    /// <remarks> SCPI: "{0}". </remarks>
    /// <value> The write Trigger Event command format. </value>
    protected virtual string TriggerEventCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Trigger Event without reading back the value from the device. </summary>
    /// <param name="value"> The Trigger Event. </param>
    /// <returns> The <see cref="TriggerEvent">Trigger Event</see> or none if unknown. </returns>
    public TriggerEvents? WriteTriggerEvent( TriggerEvents value )
    {
        this.TriggerEvent = this.Session.Write( value, this.TriggerEventCommandFormat, this.TriggerEventReadWrites );
        return this.TriggerEvent;
    }

    #endregion
}
/// <summary> Enumerates the trigger or arm layer bypass mode. </summary>
[Flags]
public enum TriggerLayerBypassModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 1 << 0,

    /// <summary> An enum constant representing the acceptor option. </summary>
    [System.ComponentModel.Description( "Acceptor (ACC)" )]
    Acceptor = 1 << 1,

    /// <summary> An enum constant representing the source option. </summary>
    [System.ComponentModel.Description( "Source (SOUR)" )]
    Source = 1 << 2
}
/// <summary> Enumerates the arm to trigger events. </summary>
[Flags]
public enum TriggerEvents : long
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None (NONE)" )]
    None = 0L,

    /// <summary> An enum constant representing the source option. </summary>
    [System.ComponentModel.Description( "Source (SOUR)" )]
    Source = 1 << 1,

    /// <summary> An enum constant representing the delay option. </summary>
    [System.ComponentModel.Description( "Delay (DEL)" )]
    Delay = 1 << 2,

    /// <summary> An enum constant representing the sense option. </summary>
    [System.ComponentModel.Description( "Sense (SENS)" )]
    Sense = 1 << 3,

    // TSP2 EVENTS
#if false
// this item must be inserted when instantiating the TSP2 trigger subsystem,
    /// <summary> An enum constant representing the TSP None option. </summary>
    [System.ComponentModel.Description( "None (trigger.EVENT_NONE)" )]
    TspNone = 1 << 4,
#endif

    /// <summary> An enum constant representing the display option. </summary>
    [System.ComponentModel.Description( "Display (trigger.EVENT_DISPLAY)" )]
    Display = 1 << 5,

    /// <summary> An enum constant representing the notify 1 option. generates a trigger event when
    /// the trigger model executes it </summary>
    [System.ComponentModel.Description( "Notify trigger block 1 (trigger.EVENT_NOTIFY1)" )]
    Notify1 = 1 << 6,

    /// <summary> An enum constant representing the notify 2 option. </summary>
    [System.ComponentModel.Description( "Notify trigger block 2 (trigger.EVENT_NOTIFY2)" )]
    Notify2 = 1 << 7,

    /// <summary> An enum constant representing the notify 3 option. </summary>
    [System.ComponentModel.Description( "Notify trigger block 3 (trigger.EVENT_NOTIFY3)" )]
    Notify3 = 1 << 8,

    /// <summary> An enum constant representing the notify 4 option. </summary>
    [System.ComponentModel.Description( "Notify trigger block 4 (trigger.EVENT_NOTIFY4)" )]
    Notify4 = 1 << 9,

    /// <summary> An enum constant representing the notify 5 option. </summary>
    [System.ComponentModel.Description( "Notify trigger block 5 (trigger.EVENT_NOTIFY5)" )]
    Notify5 = 1 << 10,

    /// <summary> An enum constant representing the notify 6 option. </summary>
    [System.ComponentModel.Description( "Notify trigger block 6 (trigger.EVENT_NOTIFY6)" )]
    Notify6 = 1 << 11,

    /// <summary> An enum constant representing the notify 7 option. </summary>
    [System.ComponentModel.Description( "Notify trigger block 7 (trigger.EVENT_NOTIFY7)" )]
    Notify7 = 1 << 12,

    /// <summary> An enum constant representing the notify 8 option. </summary>
    [System.ComponentModel.Description( "Notify trigger block 8 (trigger.EVENT_NOTIFY8)" )]
    Notify8 = 1 << 13,

    /// <summary> An enum constant representing the bus option. A command interface trigger (bus trigger):
    /// Any remote interface: *TRG or GPIB GET bus command  or VXI-11 command device_trigger  </summary>
    [System.ComponentModel.Description( "Bus command (trigger.EVENT_COMMAND)" )]
    Bus = 1 << 14,

    /// <summary> An enum constant representing the line edge option.
    /// Line edge (either rising, falling, or either based on the
    /// configuration of the line) detected on digital input line N (1 to 6)  </summary>
    [System.ComponentModel.Description( "Line Edge 1 (trigger.EVENT_DIGIO1)" )]
    LineEdge1 = 1 << 15,

    /// <summary> An enum constant representing the line edge 2 option. </summary>
    [System.ComponentModel.Description( "Line Edge 2 (trigger.EVENT_DIGIO2)" )]
    LineEdge2 = 1 << 16,

    /// <summary> An enum constant representing the line edge 3 option. </summary>
    [System.ComponentModel.Description( "Line Edge 3 (trigger.EVENT_DIGIO3)" )]
    LineEdge3 = 1 << 17,

    /// <summary> An enum constant representing the line edge 4 option. </summary>
    [System.ComponentModel.Description( "Line Edge 4 (trigger.EVENT_DIGIO4)" )]
    LineEdge4 = 1 << 18,

    /// <summary> An enum constant representing the line edge 5 option. </summary>
    [System.ComponentModel.Description( "Line Edge 5 (trigger.EVENT_DIGIO5)" )]
    LineEdge5 = 1 << 19,

    /// <summary> An enum constant representing the line edge 6 option. </summary>
    [System.ComponentModel.Description( "Line Edge 6 (trigger.EVENT_DIGIO6)" )]
    LineEdge6 = 1 << 20,

    /// <summary> An enum constant representing the tsp link 1 option.
    /// Line edge detected on TSP-Link synchronization line N (1 to 3) </summary>
    [System.ComponentModel.Description( "Tsp-Link Line Edge 1 (trigger.EVENT_TSPLINK1)" )]
    TspLink1 = 1 << 21,

    /// <summary> An enum constant representing the tsp link 2 option. </summary>
    [System.ComponentModel.Description( "Tsp-Link Line Edge 2 (trigger.EVENT_TSPLINK2)" )]
    TspLink2 = 1 << 22,

    /// <summary> An enum constant representing the tsp link 3 option. </summary>
    [System.ComponentModel.Description( "Tsp-Link Line Edge 3 (trigger.EVENT_TSPLINK3)" )]
    TspLink3 = 1 << 23,

    /// <summary> An enum constant representing the Tangent 1 option.
    /// Appropriate LXI trigger packet is received on LAN trigger object N (1 to 8) </summary>
    [System.ComponentModel.Description( "Lan Trigger 1 (trigger.EVENT_LAN1)" )]
    Lan1 = 1 << 24,

    /// <summary> An enum constant representing the LAN 2 option. </summary>
    [System.ComponentModel.Description( "Lan Trigger 2 (trigger.EVENT_LAN2)" )]
    Lan2 = 1 << 25,

    /// <summary> An enum constant representing the LAN 3 option. </summary>
    [System.ComponentModel.Description( "Lan Trigger 3 (trigger.EVENT_LAN3)" )]
    Lan3 = 1 << 26,

    /// <summary> An enum constant representing the LAN 4 option. </summary>
    [System.ComponentModel.Description( "Lan Trigger 4 (trigger.EVENT_LAN4)" )]
    Lan4 = 1 << 27,

    /// <summary> An enum constant representing the LAN 5 option. </summary>
    [System.ComponentModel.Description( "Lan Trigger 5 (trigger.EVENT_LAN5)" )]
    Lan5 = 1 << 28,

    /// <summary> An enum constant representing the LAN 6 option. </summary>
    [System.ComponentModel.Description( "Lan Trigger 6 (trigger.EVENT_LAN6)" )]
    Lan6 = 1 << 29,

    /// <summary> An enum constant representing the LAN 7 option. </summary>
    [System.ComponentModel.Description( "Lan Trigger 7 (trigger.EVENT_LAN7)" )]
    Lan7 = 1 << 30,

    /// <summary> An enum constant representing the LAN 8 option. </summary>
    [System.ComponentModel.Description( "Lan Trigger 8 (trigger.EVENT_LAN8)" )]
    Lan8 = 1 << 31,

    /// <summary> An enum constant representing the blender 1 option. </summary>
    [System.ComponentModel.Description( "Blender 1 (trigger.EVENT_BLENDER1)" )]
    Blender1 = 1L << 32,

    /// <summary> An enum constant representing the blender 2 option. </summary>
    [System.ComponentModel.Description( "Blender 2 (trigger.EVENT_BLENDER2)" )]
    Blender2 = 1L << 33,

    /// <summary> An enum constant representing the timer 1 option. </summary>
    [System.ComponentModel.Description( "Timer Expired 2 (trigger.EVENT_TIMER1)" )]
    Timer1 = 1L << 34,

    /// <summary> An enum constant representing the timer 2 option. </summary>
    [System.ComponentModel.Description( "Timer Expired 2 (trigger.EVENT_TIMER2)" )]
    Timer2 = 1L << 35,

    /// <summary> An enum constant representing the timer 3 option. </summary>
    [System.ComponentModel.Description( "Timer Expired 3 (trigger.EVENT_TIMER3)" )]
    Timer3 = 1L << 36,

    /// <summary> An enum constant representing the timer 4 option. </summary>
    [System.ComponentModel.Description( "Timer Expired 4 (trigger.EVENT_TIMER4)" )]
    Timer4 = 1L << 37,

    /// <summary> An enum constant representing the analog option. </summary>
    [System.ComponentModel.Description( "Analog Trigger (trigger.EVENT_ANALOGTRIGGER)" )]
    Analog = 1L << 38,

    /// <summary> An enum constant representing the external option. </summary>
    [System.ComponentModel.Description( "External in trigger (trigger.EVENT_EXTERNAL)" )]
    External = 1L << 39
}
/// <summary> Enumerates the trigger layer control sources. </summary>
[Flags]
public enum TriggerSources
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 1 << 0,

    /// <summary> An enum constant representing the bus option. </summary>
    [System.ComponentModel.Description( "Bus (BUS)" )]
    Bus = 1 << 1,

    /// <summary> An enum constant representing the external option. </summary>
    [System.ComponentModel.Description( "External (EXT)" )]
    External = 1 << 2,

    /// <summary> An enum constant representing the immediate option. </summary>
    [System.ComponentModel.Description( "Immediate (IMM)" )]
    Immediate = 1 << 3,

    /// <summary> An enum constant representing the trigger link option. </summary>
    [System.ComponentModel.Description( "Trigger Link (TLIN)" )]
    TriggerLink = 1 << 4,

    /// <summary> . </summary>
    [System.ComponentModel.Description( "Internal (INT)" )]
    Internal = 1 << 5,

    /// <summary> An enum constant representing the manual option. </summary>
    [System.ComponentModel.Description( "Manual (MAN)" )]
    Manual = 1 << 6,

    /// <summary> An enum constant representing the hold option. </summary>
    [System.ComponentModel.Description( "Hold (HOLD)" )]
    Hold = 1 << 7,

    /// <summary> An enum constant representing the timer option. </summary>
    [System.ComponentModel.Description( "Timer (TIM)" )]
    Timer = 1 << 8,

    /// <summary> An enum constant representing the LAN option. </summary>
    [System.ComponentModel.Description( "LXI (LAN)" )]
    Lan = 1 << 9,

    /// <summary> An enum constant representing the analog option. </summary>
    [System.ComponentModel.Description( "Analog (ATR)" )]
    Analog = 1 << 10,

    /// <summary> An enum constant representing the blender option. </summary>
    [System.ComponentModel.Description( "Blender (BLEND)" )]
    Blender = 1 << 11,

    /// <summary> An enum constant representing the digital option. </summary>
    [System.ComponentModel.Description( "Digital I/O (DIG)" )]
    Digital = 1 << 12
}
