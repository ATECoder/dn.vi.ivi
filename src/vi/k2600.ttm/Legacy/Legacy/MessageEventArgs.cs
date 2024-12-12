using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines an event arguments class for messages. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>  
/// David, 2009-05-01, 1.1.3408.x. </para><para>
/// David, 2013-08-05, 3.0.3053.x. </para>
/// </remarks>
/// <remarks> Initializes a new instance of the <see cref="MessageEventArgs" /> class. </remarks>
/// <param name="broadcast"> The broadcast. </param>
/// <param name="trace">     The trace. </param>
/// <param name="synopsis">  The synopsis. </param>
/// <param name="details">   The details. </param>
public class MessageEventArgs( TraceEventType broadcast, TraceEventType trace, string synopsis, string details ) : EventArgs()
{

    #region " construction and cleanup "

    /// <summary> Initializes a new instance of the <see cref="MessageEventArgs" /> class. </summary>
    /// <param name="synopsis"> The synopsis. </param>
    /// <param name="format">   The format. </param>
    /// <param name="args">     The Arguments. </param>
    public MessageEventArgs( string synopsis, string format, params object[] args ) : this( TraceEventType.Information, TraceEventType.Information, synopsis, format, args )
    {
    }

    /// <summary> Initializes a new instance of the <see cref="MessageEventArgs" /> class. </summary>
    /// <param name="trace">    The trace. </param>
    /// <param name="synopsis"> The synopsis. </param>
    /// <param name="format">   The format. </param>
    /// <param name="args">     The Arguments. </param>
    public MessageEventArgs( TraceEventType trace, string synopsis, string format, params object[] args ) : this( trace, trace, synopsis, format, args )
    {
    }

    /// <summary> Constructor. </summary>
    /// <param name="broadcast"> The broadcast. </param>
    /// <param name="trace">     The trace. </param>
    /// <param name="synopsis">  The synopsis. </param>
    /// <param name="format">    The format. </param>
    /// <param name="args">     The Arguments. </param>
    public MessageEventArgs( TraceEventType broadcast, TraceEventType trace, string synopsis, string format, params object[] args ) : this( broadcast, trace, synopsis, string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) )
    {
    }

    /// <summary> Initializes a new instance of the <see cref="MessageEventArgs" /> class. </summary>
    /// <param name="synopsis"> The synopsis. </param>
    /// <param name="details">  The details. </param>
    public MessageEventArgs( string synopsis, string details ) : this( TraceEventType.Information, TraceEventType.Information, synopsis, details ) { }

    #endregion

    #region " properties "

    /// <summary> Gets the message details. </summary>
    /// <value> The details. </value>
    public string Details { get; } = details;

    /// <summary> Gets the event synopsis. </summary>
    /// <value> The synopsis. </value>
    public string Synopsis { get; } = synopsis;

    /// <summary> Gets the event time stamp. </summary>
    /// <value> The timestamp. </value>
    public DateTime Timestamp { get; } = DateTime.Now;

    /// <summary> Gets the trace level. </summary>
    /// <value> The trace level. </value>
    public TraceEventType TraceLevel { get; } = trace;

    /// <summary> Gets the Broadcast level. </summary>
    /// <value> The broadcast level. </value>
    public TraceEventType BroadcastLevel { get; } = broadcast;

    #endregion

    #region " methods "

    /// <summary> Describes the format to use. </summary>
    private readonly string _format = "{0:HH:mm:ss.fff}, {1}, {2}, {3}";

    /// <summary> Returns a message based on the default format. </summary>
    /// <returns> A string. </returns>
    public override string ToString()
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, this._format, this.Timestamp, this.TraceLevel, this.Synopsis, this.Details );
    }

    #endregion

}
