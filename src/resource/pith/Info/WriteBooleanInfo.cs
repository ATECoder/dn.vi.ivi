using cc.isr.Enums;

namespace cc.isr.VI;

/// <summary>   Information about the query and parse actions. </summary>
/// <remarks>   David, 2021-04-12. </remarks>
public struct WriteBooleanInfo : IInfoReport
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="sentValue">        The sent value. </param>
    /// <param name="commandFormat">    The command format. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="elapsedTimes">     The elapsed. </param>
    public WriteBooleanInfo( bool sentValue, string commandFormat, string sentMessage, ElapsedTimeSpan[] elapsedTimes )
    {
        this.SentValue = sentValue;
        this.CommandFormat = commandFormat;
        this.SentMessage = sentMessage;
        this._elapsedTimes = elapsedTimes;
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="sentValue">    The sent value. </param>
    /// <param name="writeInfo">    Information describing the write. </param>
    public WriteBooleanInfo( bool sentValue, WriteInfo<int> writeInfo )
    {
        this.SentValue = sentValue;
        this.CommandFormat = writeInfo.CommandFormat;
        this.SentMessage = writeInfo.SentMessage;
        this._elapsedTimes = writeInfo.GetElapsedTimes();
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="sentValue">      The sent value. </param>
    /// <param name="commandFormat">     The command format. </param>
    /// <param name="sentMessage">      The message that was sent. </param>
    /// <param name="elapsedTime">      The elapsed time. </param>
    public WriteBooleanInfo( bool sentValue, string commandFormat, string sentMessage, TimeSpan elapsedTime )
    {
        this.SentValue = sentValue;
        this.CommandFormat = commandFormat;
        this.SentMessage = sentMessage;
        this._elapsedTimes = [new( ElapsedTimeIdentity.WriteTime, elapsedTime )];
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="sentValue">    The sent value. </param>
    /// <param name="executeInfo">  Information describing the execute. </param>
    public WriteBooleanInfo( bool sentValue, ExecuteInfo executeInfo )
    {
        this.SentValue = sentValue;
        this.CommandFormat = executeInfo.MessageToSend;
        this.SentMessage = executeInfo.SentMessage;
        this._elapsedTimes = executeInfo.GetElapsedTimes();
    }

    /// <summary>   Gets the empty <see cref="WriteBooleanInfo "/>. </summary>
    /// <value> The empty <see cref="WriteBooleanInfo "/>. </value>
    public static WriteBooleanInfo Empty => new( default, string.Empty, string.Empty, TimeSpan.Zero );

    /// <summary>   Gets or sets the sent value. </summary>
    /// <value> The sent value. </value>
    public bool SentValue { get; set; }

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
        _ = builder.AppendLine( $"Value {this.SentValue}; Command '{this.CommandFormat}'; Sent '{this.SentMessage}'" );
        _ = builder.AppendLine( $"  {BuildElapsedTimesMessage( this._elapsedTimes )}" );
        return builder.ToString();
    }
}
