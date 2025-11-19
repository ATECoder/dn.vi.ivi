// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Enums;

namespace cc.isr.VI;

/// <summary>   Information about the write action. </summary>
/// <remarks>   David, 2021-04-12. </remarks>
/// <typeparam name="T">    Generic type parameter. </typeparam>
public struct WriteInfo<T> : IInfoReport where T : IComparable<T>, IEquatable<T>, IFormattable
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="sentValue">        The sent value. </param>
    /// <param name="commandFormat">    The command format. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="elapsedTimes">     List of times of the elapsed. </param>
    public WriteInfo( T sentValue, string commandFormat, string sentMessage, ElapsedTimeSpan[] elapsedTimes )
    {
        this.SentValue = sentValue;
        this.CommandFormat = commandFormat;
        this.SentMessage = sentMessage;
        this._elapsedTimes = elapsedTimes;
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="sentValue">        The sent value. </param>
    /// <param name="commandFormat">    The command format. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="elapsedTime">      The elapsed time. </param>
    public WriteInfo( T? sentValue, string commandFormat, string sentMessage, TimeSpan elapsedTime )
    {
        this.SentValue = sentValue;
        this.CommandFormat = commandFormat;
        this.SentMessage = sentMessage;
        this._elapsedTimes = [new( ElapsedTimeIdentity.WriteTime, elapsedTime )];
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <param name="sentValue">    The sent value. </param>
    /// <param name="executeInfo">  Information describing the execute. </param>
    public WriteInfo( T sentValue, ExecuteInfo executeInfo )
    {
        this.SentValue = sentValue;
        this.CommandFormat = executeInfo.MessageToSend;
        this.SentMessage = executeInfo.SentMessage;
        this._elapsedTimes = executeInfo.GetElapsedTimes();
    }

    /// <summary>   Gets the empty <see cref="WriteInfo{T}"/>. </summary>
    /// <value> The empty <see cref="WriteInfo{T}"/>. </value>
    public static WriteInfo<T> Empty => new( default, string.Empty, string.Empty, TimeSpan.Zero );

    /// <summary>   Gets or sets the sent value. </summary>
    /// <value> The sent value. </value>
    public T? SentValue { get; set; }

    /// <summary>   Gets or sets the command format. </summary>
    /// <value> The command format. </value>
    public string CommandFormat { get; set; }

    /// <summary>   Gets or sets the message that was sent. </summary>
    /// <value> The message that was sent. </value>
    public string SentMessage { get; set; }

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
        _ = builder.AppendLine( $"Sent Value {this.SentValue}; Command '{this.CommandFormat}'; Sent '{this.SentMessage}'" );
        _ = builder.AppendLine( $"  {BuildElapsedTimesMessage( this._elapsedTimes )}" );
        return builder.ToString();
    }
}
