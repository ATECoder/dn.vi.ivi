using cc.isr.Enums;

namespace cc.isr.VI.Tsp2;

/// <summary> Defines the contract that must be implemented by a SCPI Trigger Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class TriggerSubsystemBase : VI.TriggerSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    protected TriggerSubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.TriggerEventReadWrites.AddReplace( ( long ) TriggerEvents.None, "trigger.EVENT_NONE", TriggerEvents.None.DescriptionUntil() );
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
        this.AutoDelayEnabled = false;
        this.TriggerCount = 1;
        this.Delay = TimeSpan.Zero;
        this.TriggerLayerBypassMode = TriggerLayerBypassModes.Acceptor;
        this.InputLineNumber = 1;
        this.OutputLineNumber = 2;
        this.TriggerSource = TriggerSources.Immediate;
        this.TimerInterval = TimeSpan.FromSeconds( 0.1d );
        this.SupportedTriggerSources = TriggerSources.Bus | TriggerSources.External | TriggerSources.Immediate;
        this.ContinuousEnabled = false;
        this.TriggerState = VI.TriggerState.None;
    }

    #endregion

    #region " blender clear "

    /// <summary> Gets or sets the trigger blender clear command format. </summary>
    /// <value> The trigger blender clear command format. </value>
    protected virtual string TriggerBlenderClearCommandFormat { get; set; } = "trigger.blender[{0}].clear()";

    /// <summary> Trigger blender clear. </summary>
    /// <param name="blenderNumber"> The blender number. </param>
    public void TriggerBlenderClear( int blenderNumber )
    {
        _ = this.Session.WriteLineIfDefined( this.TriggerBlenderClearCommandFormat, ( object ) blenderNumber );
    }

    #endregion

    #region " blender or enabled  "

    /// <summary> Applies the trigger blender or enabled described by value. </summary>
    /// <param name="value"> True to value. </param>
    /// <returns> A Boolean? </returns>
    public bool? ApplyTriggerBlenderOrEnabled( bool value )
    {
        _ = this.WriteAutoDelayEnabled( value );
        return this.QueryAutoDelayEnabled();
    }

    /// <summary> Gets or sets the trigger blender or enable query command format. </summary>
    /// <value> The trigger blender or enable query command format. </value>
    protected virtual string TriggerBlenderOrEnableQueryCommandFormat { get; set; } = "_G.print(trigger.blender[{0}].orenable=true)";

    /// <summary> Queries trigger blender or enabled. </summary>
    /// <param name="blenderNumber"> The blender number. </param>
    /// <returns> The trigger blender or enabled. </returns>
    public bool? QueryTriggerBlenderOrEnabled( int blenderNumber )
    {
        this.AutoDelayEnabled = this.Session.Query( false, string.Format( this.TriggerBlenderOrEnableQueryCommandFormat, blenderNumber ) );
        return this.AutoDelayEnabled;
    }

    /// <summary> Gets or sets the trigger blender or enable command format. </summary>
    /// <value> The trigger blender or enable command format. </value>
    protected virtual string TriggerBlenderOrEnableCommandFormat { get; set; } = "trigger.blender[{0}].orenable={1}";

    /// <summary> Writes a trigger blender or enables. </summary>
    /// <param name="blenderNumber"> The blender number. </param>
    /// <param name="value">         True to value. </param>
    public void WriteTriggerBlenderOrEnables( int blenderNumber, bool value )
    {
        _ = this.Session.WriteLineIfDefined( this.TriggerBlenderOrEnableCommandFormat, blenderNumber, value );
    }

    #endregion

    #region " command syntax "

    #region " abort / init commands "

    /// <summary> Gets or sets the Abort command. </summary>
    /// <value> The Abort command. </value>
    protected override string AbortCommand { get; set; } = "trigger.model.abort()";

    /// <summary> Gets or sets the initiate command. </summary>
    /// <value> The initiate command. </value>
    protected override string InitiateCommand { get; set; } = "trigger.model.initiate()";

    /// <summary> Gets or sets the clear command. </summary>
    /// <value> The clear command. </value>
    protected override string ClearCommand { get; set; } = "trigger.extin.clear()";

    /// <summary> Gets or sets the clear  trigger model command. </summary>
    /// <remarks> SCPI: ":TRIG:LOAD 'EMPTY'". </remarks>
    /// <value> The clear command. </value>
    protected override string ClearTriggerModelCommand { get; set; } = "trigger.model.clear()";


    #endregion

    #region " trigger state "

    /// <summary> Gets or sets the Trigger State query command. </summary>
    /// <remarks>
    /// <c>SCPI: :TRIG:STAT? <para>
    /// TSP2: trigger.model.state()
    /// </para></c>
    /// </remarks>
    /// <value> The Trigger State query command. </value>
    protected override string TriggerStateQueryCommand { get; set; } = "_G.print(trigger.model.state())";

    #endregion

    #region " trigger count "

    /// <summary> Gets or sets trigger count query command. </summary>
    /// <value> The trigger count query command. </value>
    protected override string TriggerCountQueryCommand { get; set; } = string.Empty; // ":TRIG:COUN?"

    /// <summary> Gets or sets trigger count command format. </summary>
    /// <value> The trigger count command format. </value>
    protected override string TriggerCountCommandFormat { get; set; } = string.Empty; // ":TRIG:COUN {0}"

    #endregion

    #region " delay "

    /// <summary> Gets or sets the delay command format. </summary>
    /// <value> The delay command format. </value>
    protected override string DelayCommandFormat { get; set; } = string.Empty; // ":TRIG:DEL {0:s\.FFFFFFF}"

    /// <summary> Gets or sets the Delay format for converting the query to time span. </summary>
    /// <value> The Delay query command. </value>
    protected override string DelayFormat { get; set; } = string.Empty; // "s\.FFFFFFF"

    /// <summary> Gets or sets the delay query command. </summary>
    /// <value> The delay query command. </value>
    protected override string DelayQueryCommand { get; set; } = string.Empty; // ":TRIG:DEL?"

    #endregion

    #endregion

}
