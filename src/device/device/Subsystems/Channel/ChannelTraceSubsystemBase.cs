namespace cc.isr.VI;

/// <summary> Defines the Channel Trace subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-07-06, 4.0.6031. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class ChannelTraceSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelTraceSubsystemBase" /> class.
    /// </summary>
    /// <param name="traceNumber">     The Trace number. </param>
    /// <param name="channelNumber">   The channel number. </param>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    protected ChannelTraceSubsystemBase( int traceNumber, int channelNumber, StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.TraceNumber = traceNumber;
        this.ChannelNumber = channelNumber;
        this.TraceParameterReadWrites = [];
        this.DefineTraceParameterReadWrites();
    }

    #endregion

    #region " channel "

    /// <summary> Gets or sets the channel number. </summary>
    /// <value> The channel number. </value>
    public int ChannelNumber { get; private set; }

    #endregion

    #region " trace "

    /// <summary> Gets or sets the Trace number. </summary>
    /// <value> The Trace number. </value>
    public int TraceNumber { get; private set; }

    #endregion

    #region " auto scale "

    /// <summary> Gets or sets the auto scale command. </summary>
    /// <remarks> SCPI: ":DISP:WIND{0}:TRAC{1}:Y:AUTO". </remarks>
    /// <value> The auto scale command. </value>
    protected virtual string AutoScaleCommand { get; set; } = string.Empty;

    /// <summary> Automatic scale. </summary>
    public void AutoScale()
    {
        if ( !string.IsNullOrWhiteSpace( this.AutoScaleCommand ) )
        {
            _ = this.Session.WriteLine( string.Format( this.AutoScaleCommand, this.ChannelNumber, this.TraceNumber ) );
        }
    }

    #endregion

    #region " select "

    /// <summary> Gets or sets the Select command. </summary>
    /// <remarks> SCPI: ":CALC{0):PAR{1}:SEL". </remarks>
    /// <value> The Select command. </value>
    protected virtual string SelectCommand { get; set; } = string.Empty;

    /// <summary> Selects this channel as the active trace. </summary>
    public void Select()
    {
        if ( !string.IsNullOrWhiteSpace( this.SelectCommand ) )
        {
            _ = this.Session.WriteLine( this.SelectCommand, this.ChannelNumber, this.TraceNumber );
        }
    }

    #endregion
}
