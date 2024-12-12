namespace cc.isr.VI;

/// <summary>   Information about the query and parse actions. </summary>
/// <remarks>   David, 2021-04-12. </remarks>
/// <typeparam name="T">    Generic type parameter. </typeparam>
/// <remarks>   Constructor. </remarks>
/// <remarks>   David, 2021-04-12. </remarks>
/// <param name="hasValue">         True if this object has value, false if not. </param>
/// <param name="parsedValue">      The parsed value. </param>
/// <param name="parsedMessage">    The message that was parsed. </param>
/// <param name="receivedMessage">  The message that was received. </param>
public struct ParseInfo<T>( bool hasValue, T? parsedValue, string parsedMessage, string receivedMessage ) where T : IComparable<T>, IEquatable<T>, IFormattable
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="parsedValue">      The parsed value. </param>
    /// <param name="parsedMessage">    The message that was parsed. </param>
    /// <param name="receivedMessage">  The message that was received. </param>
    public ParseInfo( T parsedValue, string parsedMessage, string receivedMessage ) : this( true, parsedValue, parsedMessage, receivedMessage )
    {
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="parsedValue">      The parsed value. </param>
    /// <param name="parsedMessage">    The message that was parsed. </param>
    public ParseInfo( T parsedValue, string parsedMessage ) : this( true, parsedValue, parsedMessage, parsedMessage )
    {
    }

    /// <summary>   Gets the empty <see cref="ParseInfo{T}"/>. </summary>
    /// <value> The empty <see cref="ParseInfo{T}"/>. </value>
    public static ParseInfo<T> Empty => new( false, default, string.Empty, string.Empty );

    /// <summary>   Gets or sets the parsed value. </summary>
    /// <value> The parsed value. </value>
    public T? ParsedValue { get; set; } = parsedValue;

    /// <summary>   Gets or sets a value indicating whether this object has value. </summary>
    /// <value> True if this object has value, false if not. </value>
    public bool HasValue { get; set; } = hasValue;

    /// <summary>   Gets or sets a the message that was received. </summary>
    /// <value> The message that was received. </value>
    public string ReceivedMessage { get; set; } = receivedMessage;

    /// <summary>   Gets or sets the message that was parsed. </summary>
    /// <value> The message that was parsed. </value>
    public string ParsedMessage { get; set; } = parsedMessage;

}
