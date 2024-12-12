using cc.isr.Enums;
using cc.isr.Std.EscapeSequencesExtensions;

namespace cc.isr.VI;

/// <summary>   Information about the query action. </summary>
/// <remarks>   David, 2021-04-12. </remarks>
public struct QueryInfo : IInfoReport
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="queryCommand">     The query command. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="receivedMessage">  The message that was received. </param>
    /// <param name="elapsedTimes">     The elapsed. </param>
    public QueryInfo( string queryCommand, string sentMessage, string receivedMessage, ElapsedTimeSpan[] elapsedTimes )
    {
        this.QueryCommand = queryCommand;
        this.SentMessage = sentMessage;
        this.ReceivedMessage = receivedMessage;
        this._elapsedTimes = elapsedTimes;
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="queryCommand">     The query command. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="receivedMessage">  The message that was received. </param>
    /// <param name="elapsedTime">      The elapsed time. </param>
    public QueryInfo( string queryCommand, string sentMessage, string receivedMessage, TimeSpan elapsedTime )
    {
        this.QueryCommand = queryCommand;
        this.SentMessage = sentMessage;
        this.ReceivedMessage = receivedMessage;
        this._elapsedTimes = [new( ElapsedTimeIdentity.WriteTime, elapsedTime )];
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-16. </remarks>
    /// <param name="readInfo"> Information describing the read. </param>
    public QueryInfo( ReadInfo readInfo )
    {
        this.QueryCommand = string.Empty;
        this.SentMessage = string.Empty;
        this.ReceivedMessage = readInfo.ReceivedMessage;
        this._elapsedTimes = readInfo.GetElapsedTimes();
    }

    /// <summary>   Gets the empty <see cref="QueryInfo"/>. </summary>
    /// <value> The empty <see cref="QueryInfo"/>. </value>
    public static QueryInfo Empty => new( string.Empty, string.Empty, string.Empty, TimeSpan.Zero );

    /// <summary>   Gets or sets the 'query' command. </summary>
    /// <value> The 'query' command. </value>
    public string QueryCommand { get; set; }

    /// <summary>   Gets or sets the message that was sent. </summary>
    /// <value> The message that was sent. </value>
    public string SentMessage { get; set; }

    /// <summary>   Gets or sets a the message that was received. </summary>
    /// <value> The message that was received. </value>
    public string ReceivedMessage { get; set; }

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
        _ = builder.AppendLine( $"Command '{this.QueryCommand}'; Sent '{this.SentMessage}'; Received '{this.ReceivedMessage.InsertCommonEscapeSequences()}'" );
        _ = builder.AppendLine( $"  {BuildElapsedTimesMessage( this._elapsedTimes )}" );
        return builder.ToString();
    }
}
