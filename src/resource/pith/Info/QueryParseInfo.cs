using cc.isr.Enums;
using cc.isr.Std.EscapeSequencesExtensions;

namespace cc.isr.VI;

/// <summary>   Information about the query and parse actions. </summary>
/// <remarks>   David, 2021-04-12. </remarks>
/// <typeparam name="T">    Generic type parameter. </typeparam>
public struct QueryParseInfo<T> : IInfoReport where T : IComparable<T>, IEquatable<T>, IFormattable
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="parsedValue">      The sent value. </param>
    /// <param name="queryCommand">     The query command. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="receivedMessage">  The message that was received. </param>
    /// <param name="parsedMessage">    The message that was parsed. </param>
    /// <param name="elapsedTimes">     The elapsed. </param>
    public QueryParseInfo( T parsedValue, string queryCommand, string sentMessage, string receivedMessage, string parsedMessage, ElapsedTimeSpan[] elapsedTimes )
    {
        this.HasValue = true;
        this.ParsedValue = parsedValue;
        this.QueryCommand = queryCommand;
        this.SentMessage = sentMessage;
        this.ReceivedMessage = receivedMessage;
        this.ParsedMessage = parsedMessage;
        this._elapsedTimes = elapsedTimes;
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="parsedValue">      The sent value. </param>
    /// <param name="queryParseInfo">   Information describing the query parse. </param>
    public QueryParseInfo( T parsedValue, QueryParseInfo<int> queryParseInfo )
    {
        this.HasValue = true;
        this.ParsedValue = parsedValue;
        this.QueryCommand = queryParseInfo.QueryCommand;
        this.SentMessage = queryParseInfo.SentMessage;
        this.ReceivedMessage = queryParseInfo.ReceivedMessage;
        this.ParsedMessage = queryParseInfo.ParsedMessage;
        this._elapsedTimes = queryParseInfo.GetElapsedTimes();
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="parsedValue">      The sent value. </param>
    /// <param name="queryCommand">     The query command. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="receivedMessage">  The message that was received. </param>
    /// <param name="parsedMessage">    The message that was parsed. </param>
    /// <param name="elapsedTime">      The elapsed time. </param>
    public QueryParseInfo( T? parsedValue, string queryCommand, string sentMessage, string receivedMessage, string parsedMessage, TimeSpan elapsedTime )
    {
        this.HasValue = true;
        this.ParsedValue = parsedValue;
        this.QueryCommand = queryCommand;
        this.SentMessage = sentMessage;
        this.ReceivedMessage = receivedMessage;
        this.ParsedMessage = parsedMessage;
        this._elapsedTimes = [new( ElapsedTimeIdentity.WriteTime, elapsedTime )];
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="parsedValue">      The sent value. </param>
    /// <param name="parsedMessage">    The message that was parsed. </param>
    /// <param name="queryInfo">        Information describing the query. </param>
    public QueryParseInfo( T parsedValue, string parsedMessage, QueryInfo queryInfo )
    {
        this.ParsedValue = parsedValue;
        this.HasValue = true;
        this.QueryCommand = queryInfo.QueryCommand;
        this.SentMessage = queryInfo.SentMessage;
        this.ReceivedMessage = queryInfo.ReceivedMessage;
        this.ParsedMessage = parsedMessage;
        this._elapsedTimes = queryInfo.GetElapsedTimes();
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="hasValue">         True if this object has value, false if not. </param>
    /// <param name="parsedValue">      The parsed value. </param>
    /// <param name="parsedMessage">    The message that was parsed. </param>
    /// <param name="queryInfo">        Information describing the query. </param>
    public QueryParseInfo( bool hasValue, T parsedValue, string parsedMessage, QueryInfo queryInfo )
    {
        this.HasValue = hasValue;
        this.ParsedValue = parsedValue;
        this.QueryCommand = queryInfo.QueryCommand;
        this.SentMessage = queryInfo.SentMessage;
        this.ReceivedMessage = queryInfo.ReceivedMessage;
        this.ParsedMessage = parsedMessage;
        this._elapsedTimes = queryInfo.GetElapsedTimes();
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="queryInfo">    Information describing the query. </param>
    /// <param name="parseInfo">    Information describing the parse. </param>
    public QueryParseInfo( QueryInfo queryInfo, ParseInfo<T> parseInfo )
    {
        this.HasValue = parseInfo.HasValue;
        this.ParsedValue = parseInfo.ParsedValue;
        this.QueryCommand = queryInfo.QueryCommand;
        this.SentMessage = queryInfo.SentMessage;
        this.ReceivedMessage = queryInfo.ReceivedMessage;
        this.ParsedMessage = parseInfo.ParsedMessage;
        this._elapsedTimes = queryInfo.GetElapsedTimes();
    }

    /// <summary>   Gets the empty <see cref="QueryParseInfo{T}"/>. </summary>
    /// <value> The empty <see cref="QueryParseInfo{T}"/>. </value>
    public static QueryParseInfo<T> Empty => new( default, string.Empty, string.Empty, string.Empty, string.Empty, TimeSpan.Zero );

    /// <summary>   Gets or sets the parsed value. </summary>
    /// <value> The parsed value. </value>
    public T? ParsedValue { get; set; }

    /// <summary>   Gets or sets a value indicating whether this object has value. </summary>
    /// <value> True if this object has value, false if not. </value>
    public bool HasValue { get; set; }

    /// <summary>   Gets or sets the 'query' command. </summary>
    /// <value> The 'query' command. </value>
    public string QueryCommand { get; set; }

    /// <summary>   Gets or sets the message that was sent. </summary>
    /// <value> The message that was sent. </value>
    public string SentMessage { get; set; }

    /// <summary>   Gets or sets a the message that was received. </summary>
    /// <value> The message that was received. </value>
    public string ReceivedMessage { get; set; }

    /// <summary>   Gets or sets the message that was parsed. </summary>
    /// <value> The message that was parsed. </value>
    public string ParsedMessage { get; set; }

    private readonly ElapsedTimeSpan[] _elapsedTimes;

    /// <summary>   Gets or sets the elapsed times. </summary>
    /// <returns> The elapsed times. </returns>
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
        _ = builder.AppendLine( $"Command '{this.QueryCommand}'; Sent '{this.SentMessage}'; Parsed {(this.HasValue ? this.ParsedValue : "'nil'")}:'{this.ReceivedMessage.InsertCommonEscapeSequences()}':'{this.ParsedMessage}'" );
        _ = builder.AppendLine( $"  {BuildElapsedTimesMessage( this._elapsedTimes )}" );
        return builder.ToString();
    }
}
