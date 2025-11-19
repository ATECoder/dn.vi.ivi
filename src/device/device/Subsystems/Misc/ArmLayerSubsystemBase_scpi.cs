// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

public partial class ArmLayerSubsystemBase
{
    #region " arm layer bypass mode "

    /// <summary> Define arm layer bypass mode read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineArmLayerBypassModeReadWrites()
    {
        this.ArmLayerBypassModeReadWrites = new();
        foreach ( TriggerLayerBypassModes enumValue in Enum.GetValues( typeof( TriggerLayerBypassModes ) ) )
            this.ArmLayerBypassModeReadWrites.Add( enumValue );
    }

    /// <summary> Gets a dictionary of trigger layer bypass mode parses. </summary>
    /// <value> A Dictionary of trigger layer bypass mode parses. </value>
    public Pith.EnumReadWriteCollection ArmLayerBypassModeReadWrites { get; private set; }

    /// <summary>
    /// Returns true if bypassing the source Arm using <see cref="TriggerLayerBypassModes.Acceptor"/>
    /// .
    /// </summary>
    /// <value> True if Arm layer is bypassed. </value>
    public bool IsArmLayerBypass => this.ArmLayerBypassMode == TriggerLayerBypassModes.Acceptor;

    /// <summary> Gets or sets the cached arm layer bypass mode. </summary>
    /// <value>
    /// The <see cref="ArmLayerBypassMode">Arm Layer Bypass Mode</see> or none if not set or unknown.
    /// </value>
    public TriggerLayerBypassModes ArmLayerBypassMode
    {
        get;

        protected set
        {
            if ( !this.ArmLayerBypassMode.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( this.IsArmLayerBypass ) );
            }
        }
    } = TriggerLayerBypassModes.None;

    /// <summary> Writes and reads back the Arm Layer Bypass Mode. </summary>
    /// <param name="value"> The Arm Layer Bypass Mode. </param>
    /// <returns>
    /// The <see cref="ArmLayerBypassMode">source  Arm Layer Bypass Mode</see> or none if unknown.
    /// </returns>
    public TriggerLayerBypassModes ApplyArmLayerBypassMode( TriggerLayerBypassModes value )
    {
        _ = this.WriteArmLayerBypassMode( value );
        return this.QueryArmLayerBypassMode();
    }

    /// <summary> Gets or sets the Arm Layer Bypass Mode query command. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:DIR?". </remarks>
    /// <value> The Arm Layer Bypass Mode query command. </value>
    protected virtual string ArmLayerBypassModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Arm Layer Bypass Mode. </summary>
    /// <returns>
    /// The <see cref="ArmLayerBypassMode"> Arm Layer Bypass Mode</see> or none if unknown.
    /// </returns>
    public TriggerLayerBypassModes QueryArmLayerBypassMode()
    {
        this.ArmLayerBypassMode = this.Session.QueryEnum( this.ArmLayerBypassMode, this.ArmLayerBypassModeQueryCommand );
        return this.ArmLayerBypassMode;
    }

    /// <summary> Gets or sets the Arm Layer Bypass Mode command format. </summary>
    /// <remarks> SCPI: ":ARM:LAYx:DIR {0}". </remarks>
    /// <value> The Arm Layer Bypass Mode command format. </value>
    protected virtual string ArmLayerBypassModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Arm Layer Bypass Mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The Arm Layer Bypass Mode. </param>
    /// <returns>
    /// The <see cref="ArmLayerBypassMode"> ARM Direction</see> or none if unknown.
    /// </returns>
    public TriggerLayerBypassModes WriteArmLayerBypassMode( TriggerLayerBypassModes value )
    {
        this.ArmLayerBypassMode = this.Session.WriteEnum( value, this.ArmLayerBypassModeCommandFormat );
        return this.ArmLayerBypassMode;
    }

    #region " overrides "

    /// <summary> Arm Layer Bypass Mode getter. </summary>
    /// <returns> A Nullable(Of T) </returns>
    public int ArmLayerBypassModeGetter()
    {
        return ( int ) this.ArmLayerBypassMode;
    }

    /// <summary> Arm Layer Bypass Mode setter. </summary>
    /// <param name="value"> The current ArmCount. </param>
    public void ArmLayerBypassModeSetter( int value )
    {
        this.ArmLayerBypassMode = ( TriggerLayerBypassModes ) value;
    }

    /// <summary> Arm Layer Bypass Mode reader. </summary>
    /// <returns> An Integer? </returns>
    public int ArmLayerBypassModeReader()
    {
        return ( int ) this.QueryArmLayerBypassMode();
    }

    /// <summary> Arm Layer Bypass Mode writer. </summary>
    /// <param name="value"> The Arm Layer Bypass Mode. </param>
    /// <returns> An Integer? </returns>
    public int ArmLayerBypassModeWriter( int value )
    {
        return ( int ) this.WriteArmLayerBypassMode( ( TriggerLayerBypassModes ) value );
    }

    #endregion

    #endregion

    #region " arm source "

    /// <summary> Define arm source read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineArmSourceReadWrites()
    {
        this.ArmSourceReadWrites = new();
        foreach ( ArmSources enumValue in Enum.GetValues( typeof( ArmSources ) ) )
            this.ArmSourceReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of Arm source parses. </summary>
    /// <value> A Dictionary of Arm source parses. </value>
    public Pith.EnumReadWriteCollection ArmSourceReadWrites { get; private set; }

    /// <summary> Gets or sets the supported Arm sources. </summary>
    /// <value> The supported Arm sources. </value>
    public ArmSources SupportedArmSources
    {
        get;
        set
        {
            if ( !this.SupportedArmSources.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached source ArmSource. </summary>
    /// <value>
    /// The <see cref="ArmSource">source Arm Source</see> or none if not set or unknown.
    /// </value>
    public ArmSources? ArmSource
    {
        get;

        protected set
        {
            if ( !this.ArmSource.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the source Arm Source. </summary>
    /// <param name="value"> The  Source Arm Source. </param>
    /// <returns> The <see cref="ArmSource">source Arm Source</see> or none if unknown. </returns>
    public ArmSources ApplyArmSource( ArmSources value )
    {
        _ = this.WriteArmSource( value );
        return this.QueryArmSource();
    }

    /// <summary> Gets or sets the Arm source query command. </summary>
    /// <remarks> SCPI: ":ARM:SOUR?". </remarks>
    /// <value> The Arm source query command. </value>
    protected virtual string ArmSourceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Arm source. </summary>
    /// <returns> The <see cref="ArmSource">Arm source</see> or none if unknown. </returns>
    public ArmSources QueryArmSource()
    {
        this.ArmSource = this.Session.Query( this.ArmSource.GetValueOrDefault( ArmSources.All ), this.ArmSourceReadWrites, this.ArmSourceQueryCommand );
        return this.ArmSource.Value;
    }

    /// <summary> Gets or sets the Arm source command format. </summary>
    /// <remarks> SCPI: ":ARM:SOUR {0}". </remarks>
    /// <value> The write Arm source command format. </value>
    protected virtual string ArmSourceCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Arm Source without reading back the value from the device. </summary>
    /// <param name="value"> The Arm Source. </param>
    /// <returns> The <see cref="ArmSource">Arm Source</see> or none if unknown. </returns>
    public ArmSources WriteArmSource( ArmSources value )
    {
        this.ArmSource = this.Session.Write( value, this.ArmSourceCommandFormat, this.ArmSourceReadWrites );
        return this.ArmSource.Value;
    }

    #endregion
}
/// <summary> Enumerates the arm layer control sources. </summary>
[Flags]
public enum ArmSources
{
#if false
    // defining a zero flags value causes unexpected results.
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 0,
#endif

    /// <summary> An enum constant representing the bus option. </summary>
    [System.ComponentModel.Description( "Bus (BUS)" )]
    Bus = 1,

    /// <summary> An enum constant representing the external option. </summary>
    [System.ComponentModel.Description( "External (EXT)" )]
    External = 2,

    /// <summary> An enum constant representing the hold option. </summary>
    [System.ComponentModel.Description( "Hold operation (HOLD)" )]
    Hold = 4,

    /// <summary> An enum constant representing the immediate option. </summary>
    [System.ComponentModel.Description( "Immediate (IMM)" )]
    Immediate = 8,

    /// <summary> An enum constant representing the manual option. </summary>
    [System.ComponentModel.Description( "Manual (MAN)" )]
    Manual = 16,

    /// <summary> An enum constant representing the timer option. </summary>
    [System.ComponentModel.Description( "Timer (TIM)" )]
    Timer = 32,

    /// <summary> Event detection for the arm layer is satisfied when either a positive-going or
    /// a negative-going pulse (via the SOT line of the Digital I/O) is received. </summary>
    [System.ComponentModel.Description( "SOT Pulsed High or Low (BSTES)" )]
    StartTestBoth = 64,

    /// <summary> Event detection for the arm layer is satisfied when a positive-going pulse
    /// (via the SOT line of the Digital I/O) is received.  </summary>
    [System.ComponentModel.Description( "SOT Pulsed High (PSTES)" )]
    StartTestHigh = 128,

    /// <summary> Event detection for the arm layer is satisfied when a negative-going pulse
    /// (via the SOT line of the Digital I/O) is received. </summary>
    [System.ComponentModel.Description( "SOT Pulsed High (NSTES)" )]
    StartTestLow = 256,

    /// <summary> Event detection occurs when an input trigger via the Trigger Link input line is
    /// received. See “Trigger link,” 2400 manual page 11-19, For more information. With TLINk selected, you
    /// can Loop around the Arm Event Detector by setting the Event detector bypass. </summary>
    [System.ComponentModel.Description( "Trigger Link (TLIN)" )]
    TriggerLink = 512,

    /// <summary> An enum constant representing all option. </summary>
    [System.ComponentModel.Description( "All" )]
    All = 1023
}
