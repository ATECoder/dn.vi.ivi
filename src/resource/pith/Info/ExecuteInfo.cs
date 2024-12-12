using cc.isr.Enums;

namespace cc.isr.VI;

/// <summary>   Information about the WriteLine action. </summary>
/// <remarks>   David, 2021-04-12. </remarks>
public struct ExecuteInfo : IInfoReport
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   2024-09-17. </remarks>
    /// <param name="tuple">    The tuple. </param>
    public ExecuteInfo( (string MessageToSend, string SentMessage, TimeSpan Elapsed) tuple )
    {
        this.MessageToSend = tuple.MessageToSend;
        this.SentMessage = tuple.SentMessage;
        this._elapsedTimes = [new( ElapsedTimeIdentity.WriteTime, tuple.Elapsed )];
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="messageToSend">    The message to send. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="elapsed">          The elapsed. </param>
    public ExecuteInfo( string messageToSend, string sentMessage, ElapsedTimeSpan[] elapsed )
    {
        this.MessageToSend = messageToSend;
        this.SentMessage = sentMessage;
        this._elapsedTimes = elapsed;
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="messageToSend">    The message to send. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="elapsedTime">      The elapsed time. </param>
    public ExecuteInfo( string messageToSend, string sentMessage, TimeSpan elapsedTime )
    {
        this.MessageToSend = messageToSend ?? string.Empty;
        this.SentMessage = sentMessage ?? string.Empty;
        this._elapsedTimes = [new( ElapsedTimeIdentity.WriteTime, elapsedTime )];
    }

    /// <summary>   Gets the empty <see cref="ExecuteInfo"/>. </summary>
    /// <value> The empty <see cref="ExecuteInfo"/>. </value>
    public static ExecuteInfo Empty => new( string.Empty, string.Empty, TimeSpan.Zero );

    /// <summary>   Gets or sets the message to send. </summary>
    /// <value> The message to send. </value>
    public string MessageToSend { get; set; }

    /// <summary>   Gets or sets the message that was sent. </summary>
    /// <value> The message that was sent. </value>
    public string SentMessage { get; set; }

    /// <summary>   (Immutable) list of times of the elapsed. </summary>
    private readonly ElapsedTimeSpan[] _elapsedTimes;

    /// <summary>   Gets the elapsed times. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <returns>   The elapsed times. </returns>
    public readonly ElapsedTimeSpan[] GetElapsedTimes()
    {
        return this._elapsedTimes;
    }

    /// <summary>   Builds elapsed times message. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="elapsedTimes"> The elapsed. </param>
    /// <returns>   A <see cref="string" />. </returns>
    private static string BuildElapsedTimesMessage( ElapsedTimeSpan[] elapsedTimes )
    {
        System.Text.StringBuilder builder = new();
        for ( int i = 0; i < elapsedTimes.Length; i++ )
        {
            _ = builder.Append( $"{elapsedTimes[i].Identity.Description()}: {elapsedTimes[i].Elapsed:ss\\.fff}; " );
        }
        return builder.ToString();
    }

    /// <summary>   Builds the information report. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <returns>   A <see cref="string" />. </returns>
    public readonly string BuildInfoReport()
    {
        System.Text.StringBuilder builder = new();
        _ = builder.AppendLine( $"Message to send {this.MessageToSend}; Sent '{this.SentMessage}'" );
        _ = builder.AppendLine( $"  {BuildElapsedTimesMessage( this._elapsedTimes )}" );
        return builder.ToString();
    }


}
