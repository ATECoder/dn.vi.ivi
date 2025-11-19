// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Enums;

namespace cc.isr.VI;

/// <summary>   Information about the Read action. </summary>
/// <remarks>   David, 2021-04-12. </remarks>
public struct ReadInfo : IInfoReport
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="receivedMessage">  The message that was Received. </param>
    /// <param name="elapsedTimes">     List of times of the elapsed. </param>
    public ReadInfo( string receivedMessage, ElapsedTimeSpan[] elapsedTimes )
    {
        this.ReceivedMessage = receivedMessage;
        this._elapsedTimes = elapsedTimes;
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="receivedMessage">  The message that was Received. </param>
    /// <param name="elapsedTime">      The elapsed time. </param>
    public ReadInfo( string receivedMessage, TimeSpan elapsedTime )
    {
        this.ReceivedMessage = receivedMessage;
        this._elapsedTimes = [new( ElapsedTimeIdentity.ReadTime, elapsedTime )];
    }

    /// <summary>   Gets the empty <see cref="ReadInfo"/>. </summary>
    /// <value> The empty <see cref="ReadInfo"/>. </value>
    public static ReadInfo Empty => new( string.Empty, TimeSpan.Zero );

    /// <summary>   Gets or sets the message that was Received. </summary>
    /// <value> The message that was Received. </value>
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
        _ = builder.AppendLine( $"Received {this.ReceivedMessage}" );
        _ = builder.AppendLine( $"  {BuildElapsedTimesMessage( this._elapsedTimes )}" );
        return builder.ToString();
    }

}
