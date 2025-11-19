namespace cc.isr.VI.Pith;

/// <summary> A payload base class. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-02-28 </para>
/// </remarks>
[CLSCompliant( true )]
public abstract class PayloadBase
{
    #region " construction "

    /// <summary> Specialized default constructor for use only by derived class. </summary>
    protected PayloadBase() : base()
    {
    }

    #endregion

    #region " read "

    /// <summary> Gets or sets the reading value as read from the VISA session. </summary>
    /// <value> The reading. </value>
    public string Reading { get; private set; } = string.Empty;

    /// <summary> Converts the reading to the specific payload such as a read number. </summary>
    /// <param name="reading"> The reading. </param>
    public virtual void FromReading( string reading )
    {
        this.Reading = reading;
    }

    /// <summary> Gets or sets the query command. </summary>
    /// <remarks> This <see cref="string"/> is used query the device. </remarks>
    /// <value> The query command. </value>
    public string QueryCommand { get; set; } = string.Empty;

    /// <summary> Builds query command. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public virtual string BuildQueryCommand()
    {
        return this.QueryCommand;
    }

    /// <summary>
    /// Gets or sets the message that was last sent to the session for querying the device.
    /// </summary>
    /// <value> A query message that was last sent to the device. </value>
    public string QueryMessage { get; set; } = string.Empty;

    /// <summary> Gets or sets the message that was last received from the session. </summary>
    /// <value> A message that was last received. </value>
    public string ReceivedMessage { get; set; } = string.Empty;

    /// <summary> Gets or sets the query status. </summary>
    /// <value> The query status. </value>
    public PayloadStatus QueryStatus { get; set; } = new PayloadStatus();

    /// <summary>
    /// Gets or sets the query status details containing any info on the query status, such as if the
    /// query failed to parse a value from the reading.
    /// </summary>
    /// <value> The query details. </value>
    public string QueryStatusDetails { get; set; } = string.Empty;

    /// <summary> Gets or sets the emulated payload equaling the emulated reply. </summary>
    /// <value> The emulated payload reply. </value>
    public abstract string SimulatedPayload { get; }

    #endregion

    #region " write "

    /// <summary> Gets or sets the writing value corresponding to the . </summary>
    /// <value> The writing. </value>
    public string Writing { get; set; } = string.Empty;

    /// <summary>
    /// Converts the specific payload value to a <see cref="Writing">value</see> to send to the
    /// session.
    /// </summary>
    /// <returns> A <see cref="string" />. </returns>
    public abstract string FromValue();

    /// <summary> Gets or sets the command format. </summary>
    /// <remarks>
    /// This <see cref="string"/> is used to format the <see cref="Writing"/> message.
    /// </remarks>
    /// <value> The command format. </value>
    public string CommandFormat { get; set; } = string.Empty;

    /// <summary> Builds query command. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public virtual string BuildCommand()
    {
        return string.Format( this.CommandFormat, this.FromValue() );
    }

    /// <summary> Gets or sets the message that was last sent. </summary>
    /// <value> A message that was last sent. </value>
    public string SentMessage { get; set; } = string.Empty;

    /// <summary> Gets or sets the Command status. </summary>
    /// <value> The Command status. </value>
    public PayloadStatus CommandStatus { get; set; } = new PayloadStatus();

    /// <summary>
    /// Gets or sets the Command status details containing any info on the Command status, such as if
    /// the Command failed to parse a value from the reading.
    /// </summary>
    /// <value> The Command details. </value>
    public string CommandStatusDetails { get; set; } = string.Empty;

    #endregion
}
/// <summary> A bit-field of flags for specifying payload status. </summary>
[Flags, CLSCompliant( true )]
public enum PayloadStatus
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None = 0,

    /// <summary> An enum constant representing the okay option. </summary>
    [System.ComponentModel.Description( "Okay" )]
    Okay = 1 << 0,

    /// <summary> An enum constant representing the sent option. </summary>
    [System.ComponentModel.Description( "Sent" )]
    Sent = 1 << 1,

    /// <summary> An enum constant representing the query received option. </summary>
    [System.ComponentModel.Description( "Query Received" )]
    QueryReceived = 1 << 2,

    /// <summary> An enum constant representing the query parsed option. </summary>
    [System.ComponentModel.Description( "Query Parsed" )]
    QueryParsed = 1 << 3,

    /// <summary> An enum constant representing the query parse failed option. </summary>
    [System.ComponentModel.Description( "Query Parse Failed" )]
    QueryParseFailed = 1 << 4,

    /// <summary> An enum constant representing the failed option. </summary>
    [System.ComponentModel.Description( "Failed" )]
    Failed = 1 << 5
}
