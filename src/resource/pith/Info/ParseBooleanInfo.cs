// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary>   Information about the query and parse actions. </summary>
/// <remarks>   David, 2021-04-12. </remarks>
/// <remarks>   Constructor. </remarks>
/// <remarks>   David, 2021-04-12. </remarks>
/// <param name="hasValue">         True if this object has value, false if not. </param>
/// <param name="parsedValue">      The sent value. </param>
/// <param name="parsedMessage">    The message that was parsed. </param>
/// <param name="receivedMessage">  The message that was received. </param>
public struct ParseBooleanInfo( bool hasValue, bool parsedValue, string parsedMessage, string receivedMessage )
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="parsedValue">      The sent value. </param>
    /// <param name="parsedMessage">    The message that was parsed. </param>
    /// <param name="receivedMessage">  The message that was received. </param>
    public ParseBooleanInfo( bool parsedValue, string parsedMessage, string receivedMessage ) : this( true, parsedValue, parsedMessage, receivedMessage )
    {
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="parsedValue">      The sent value. </param>
    /// <param name="parsedMessage">    The message that was parsed. </param>
    public ParseBooleanInfo( bool parsedValue, string parsedMessage ) : this( true, parsedValue, parsedMessage, parsedMessage )
    {
    }

    /// <summary>   Gets the empty <see cref="ParseBooleanInfo"/>. </summary>
    /// <value> The empty <see cref="ParseBooleanInfo"/>. </value>
    public static ParseBooleanInfo Empty => new( false, default, string.Empty, string.Empty );

    /// <summary>   Gets or sets the parsed value. </summary>
    /// <value> The parsed value. </value>
    public bool ParsedValue { get; set; } = parsedValue;

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
