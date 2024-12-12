namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by an Output Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class OutputSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="OutputSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected OutputSubsystemBase( StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.OutputOffModeReadWrites = [];
        this.DefineOutputOffModeReadWrites();
        this.OutputTerminalsModeReadWrites = [];
        this.DefineOutputTerminalsModeReadWrites();
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
        this.OutputOnState = false;
        this.OutputOffMode = OutputOffModes.Normal;
        this.OutputTerminalsMode = OutputTerminalsModes.Front;
    }

    #endregion

    #region " on/off state "

    /// <summary> State of the output on. </summary>
    private bool? _outputOnState;

    /// <summary> Gets or sets the cached output on/off state. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? OutputOnState
    {
        get => this._outputOnState;

        protected set
        {
            if ( !Equals( this.OutputOnState, value ) )
            {
                this._outputOnState = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the output on/off state. </summary>
    /// <param name="value"> if set to <c>true</c> if turning on; False if turning output off. </param>
    /// <returns> <c>true</c> if on; otherwise <c>false</c>. </returns>
    public bool? ApplyOutputOnState( bool value )
    {
        _ = this.WriteOutputOnState( value );
        return this.QueryOutputOnState();
    }

    /// <summary> Gets or sets the output on state query command. </summary>
    /// <value> The output on state query command. </value>
    protected virtual string OutputOnStateQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the output on/off state. Also sets the <see cref="OutputOnState">output on</see>
    /// sentinel.
    /// </summary>
    /// <returns> <c>true</c> if on; otherwise <c>false</c>. </returns>
    public virtual bool? QueryOutputOnState()
    {
        this.OutputOnState = this.Session.Query( this.OutputOnState.GetValueOrDefault( true ), this.OutputOnStateQueryCommand );
        return this.OutputOnState;
    }

    /// <summary> Gets or sets the output on state command format. </summary>
    /// <value> The output on state command format. </value>
    protected virtual string OutputOnStateCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the output on/off state. Does not read back from the instrument. </summary>
    /// <param name="value"> if set to <c>true</c> [is on]. </param>
    /// <returns> <c>true</c> if on; otherwise <c>false</c>. </returns>
    public virtual bool? WriteOutputOnState( bool value )
    {
        _ = this.Session.WriteLine( this.OutputOnStateQueryCommand, value.GetHashCode() );
        this.OutputOnState = value;
        return this.OutputOnState;
    }

    #endregion
}
