namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a Source Channel Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-07-06, 4.0.6031. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SourceSubsystemBase" /> class.
/// </remarks>
/// <param name="channelNumber">   The channel number. </param>
/// <param name="statusSubsystem"> The status subsystem. </param>
[CLSCompliant( false )]
public abstract class SourceChannelSubsystemBase( int channelNumber, StatusSubsystemBase statusSubsystem ) : SourceFunctionSubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary> Sets values to their known clear execution state. </summary>
    public override void DefineClearExecutionState()
    {
        base.DefineClearExecutionState();
        this.DefineFunctionClearKnownState();
    }

    #endregion

    #region " channel "

    /// <summary> Gets or sets the channel number. </summary>
    /// <value> The channel number. </value>
    public int ChannelNumber { get; private set; } = channelNumber;

    #endregion
}
