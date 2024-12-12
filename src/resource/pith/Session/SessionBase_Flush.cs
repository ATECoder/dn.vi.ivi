namespace cc.isr.VI.Pith;
public abstract partial class SessionBase : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDisposable
{
    /// <summary>
    /// Reads and discards all data from the VISA session until the END indicator is read.
    /// </summary>
    /// <remarks> Uses 10ms poll delay, 100ms awaitStatusTimeout. </remarks>
    [Obsolete( "Use <SessionBase>.DiscardUnreadData( <TimeSpan> )" )]
    public ServiceRequests DiscardUnreadData()
    {
        return this.DiscardUnreadData( TimeSpan.FromMilliseconds( 100d ) );
    }

    /// <summary> Information describing the discarded. </summary>
    private System.Text.StringBuilder _discardedData = new();

    /// <summary> Gets the information describing the discarded. </summary>
    /// <value> Information describing the discarded. </value>
    public string DiscardedData => this._discardedData.ToString();

    /// <summary> Clears the discarded messages. </summary>
    public void ClearDiscardedData()
    {
        _ = this._discardedData.Clear();
    }

    /// <summary>
    /// Reads and discards all data from the VISA session until the END indicator is read and no data
    /// are added to the output buffer.
    /// </summary>
    /// <remarks>
    /// This was modified to watch for both the message and error available bits and read the status
    /// byte without invoking the statusByte byte event handlers.
    /// </remarks>
    /// <param name="awaitStatusTimeout">   Specifies the time to wait for status byte message
    ///                                     available bitmask. </param>
    /// <returns>   The status byte. </returns>
    public ServiceRequests DiscardUnreadData( TimeSpan awaitStatusTimeout )
    {
        return this.DiscardUnreadData( this.ReadStatusByte(), awaitStatusTimeout );
    }

    /// <summary>
    /// Reads and discards all data from the VISA session until the END indicator is read and no data
    /// are added to the output buffer.
    /// </summary>
    /// <remarks>
    /// This was modified to watch for both the message and error available bits and read the status
    /// byte without invoking the statusByte byte event handlers.
    /// </remarks>
    /// <param name="statusByte">           The status byte. </param>
    /// <param name="awaitStatusTimeout">   Specifies the time to wait for status byte message
    ///                                     available bitmask. </param>
    /// <returns>   The status byte. </returns>
    public ServiceRequests DiscardUnreadData( ServiceRequests statusByte, TimeSpan awaitStatusTimeout )
    {
        // to make sure this does not last forever.
        DateTime endTime = DateTime.UtcNow.Add( TimeSpan.FromSeconds( 2d ) );

        this.ClearDiscardedData();

        _ = SessionBase.AsyncDelay( this.StatusReadDelay );

        bool done;
        ServiceRequests bitMask = this.MessageAvailableBitmask;
        do
        {
            if ( this.IsMessageAvailableBitSet( statusByte ) )
            {
                this.SetLastAction( "reading orphan data from the output buffer" );
                _ = this._discardedData.AppendLine( this.ReadLine() );
            }

            // this allows the status byte time to materialize per the status delay settings value.
            (_, statusByte, _) = this.AwaitStatusBitmask( bitMask, awaitStatusTimeout );

            done = 0 == (statusByte & bitMask);
        }
        while ( !done && DateTime.UtcNow < endTime );

        // if the error bit is set, get the session to raise the event requesting the status subsystem to handle the error.
        return statusByte;
    }

    /// <summary>   Discard unread data if not error. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <param name="timeout">      The awaitStatusTimeout. </param>
    /// <returns>   The ServiceRequests. </returns>
    public ServiceRequests DiscardUnreadDataIfNotError( ServiceRequests statusByte, TimeSpan timeout )
    {
        // to make sure this does not last forever.
        DateTime endTime = DateTime.UtcNow.Add( TimeSpan.FromSeconds( 2d ) );

        this._discardedData = new System.Text.StringBuilder();

        bool done;
        ServiceRequests bitMask = this.MessageAvailableBitmask | this.ErrorAvailableBitmask;
        do
        {
            if ( !this.IsErrorBitSet( statusByte ) && this.IsMessageAvailableBitSet( statusByte ) )
                _ = this._discardedData.AppendLine( this.ReadLine() );

            // this allows the status byte time to materialize per the status delay settings value.
            (_, statusByte, _) = this.AwaitStatusBitmask( bitMask, timeout );

            done = this.IsErrorBitSet( statusByte ) || (0 == (statusByte & bitMask));
        }
        while ( !done && DateTime.UtcNow < endTime );

        // if the error bit is set, get the session to raise the event requesting the status subsystem to handle the error.
        statusByte &= ~this.MessageAvailableBitmask;
        return statusByte;
    }
}
