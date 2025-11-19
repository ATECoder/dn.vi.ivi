namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " execute command with status read "

    /// <summary>   Executes the command after waiting for 'status ready' operation. </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="readyToQueryTimeout">  The ready to query timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <param name="isStatusReady">        The is status ready. </param>
    /// <param name="isStatusError">        The is status error. </param>
    /// <returns>   <see cref="ExecuteInfo"/>. </returns>
    public ExecuteInfo ExecuteStatusReady( TimeSpan readyToQueryTimeout, string dataToWrite,
                                          Func<int, bool> isStatusReady, Func<int, (bool, string, int)> isStatusError )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        Stopwatch sw = Stopwatch.StartNew();

        (bool timedOut, int readyStatusByte, TimeSpan elapsed) = this.AwaitStatus( readyToQueryTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready before executing '{dataToWrite}'" );
#endif
        ElapsedTimeSpan timeToQuery = new( ElapsedTimeIdentity.WriteReadyDelay, sw.Elapsed );
        TimeSpan temp = sw.Elapsed;

        // check for existing errors.
        (bool hasError, string details, int errorStatusByte) = isStatusError.Invoke( readyStatusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{details}' occurred before executing '{dataToWrite}'" );

        // if no error, send message to the instrument.
        string sentMessage = this.WriteLine( dataToWrite );

        ElapsedTimeSpan writeTime = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );

        return new ExecuteInfo( dataToWrite, sentMessage,
            [timeToQuery, writeTime, new( ElapsedTimeIdentity.QueryTime, sw.Elapsed )] );
    }

    #endregion

    #region " query with status read "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <param name="statusReadDelay"> The read delay. </param>
    /// <param name="readDelay">       The read delay. </param>
    /// <param name="dataToWrite">     The data to write. </param>
    /// <returns>   <see cref="QueryInfo"/>. </returns>
    public QueryInfo QueryIfNoStatusError( TimeSpan statusReadDelay, TimeSpan readDelay, string dataToWrite )
    {
        Stopwatch sw = Stopwatch.StartNew();
        string sentMessage = this.WriteLine( dataToWrite );

        ElapsedTimeSpan writeTime = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed );
        TimeSpan temp = sw.Elapsed;

        QueryInfo queryInfo = QueryInfo.Empty;
        System.Threading.Thread.SpinWait( 10 );
        _ = SessionBase.AsyncDelay( statusReadDelay );
        ServiceRequests statusByte = this.ReadStatusByte();
        if ( this.IsErrorBitSet( statusByte ) )
            this.ThrowDeviceExceptionIfError( statusByte );
        else
        {
            ElapsedTimeSpan statusDelay = new( ElapsedTimeIdentity.ReadReadyDelay, sw.Elapsed.Subtract( temp ) );
            temp = sw.Elapsed;

            System.Threading.Thread.SpinWait( 10 );
            _ = SessionBase.AsyncDelay( readDelay );

            ElapsedTimeSpan readDelayTime = new( ElapsedTimeIdentity.ReadDelay, sw.Elapsed.Subtract( temp ) );
            temp = sw.Elapsed;

            string? receivedMessage = this.ReadLine();

            ElapsedTimeSpan readTime = new( ElapsedTimeIdentity.ReadTime, sw.Elapsed.Subtract( temp ) );
            queryInfo = new( dataToWrite, sentMessage, receivedMessage,
                    [writeTime, statusDelay, readDelayTime, readTime, new( ElapsedTimeIdentity.QueryTime, sw.Elapsed )] );
        }
        return queryInfo;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="readReadyDelay">   The read ready delay. </param>
    /// <param name="readDelay">        The read delay. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <param name="queryStatusError"> The status read. </param>
    /// <returns>   <see cref="QueryInfo"/>. </returns>
    public QueryInfo QueryIfNoStatusError( TimeSpan readReadyDelay, TimeSpan readDelay, string dataToWrite,
        Func<(bool, string, int)> queryStatusError )
    {
        Stopwatch sw = Stopwatch.StartNew();
        string sentMessage = this.WriteLine( dataToWrite );
        ElapsedTimeSpan writeTime = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed );
        TimeSpan temp = sw.Elapsed;

        System.Threading.Thread.SpinWait( 10 );
        _ = SessionBase.AsyncDelay( readReadyDelay );

        (bool hasError, string details, int statusByte) = queryStatusError.Invoke();
        if ( hasError )
        {
            throw new InvalidOperationException( $"Status error 0x{statusByte:X2}:'{details}' occurred querying '{dataToWrite}'" );
        }
        else
        {
            ElapsedTimeSpan readReadyTime = new( ElapsedTimeIdentity.ReadDelay, sw.Elapsed.Subtract( temp ) );
            temp = sw.Elapsed;

            System.Threading.Thread.SpinWait( 10 );
            _ = SessionBase.AsyncDelay( readDelay );

            ElapsedTimeSpan readDelayTime = new( ElapsedTimeIdentity.ReadDelay, sw.Elapsed.Subtract( temp ) );
            temp = sw.Elapsed;

            string? receivedMessage = this.ReadLine();
            ElapsedTimeSpan readTime = new( ElapsedTimeIdentity.ReadTime, sw.Elapsed.Subtract( temp ) );
            return new QueryInfo( dataToWrite, sentMessage, receivedMessage,
                        [writeTime, readReadyTime, readDelayTime, readTime, new( ElapsedTimeIdentity.QueryTime, sw.Elapsed )] );
        }
    }

    #endregion

    #region " query with status ready "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="readyToQueryTimeout">  The ready to query timeout. </param>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <param name="isStatusReady">        The is status ready. </param>
    /// <param name="isStatusError">        The is status error. </param>
    /// <returns> <see cref="QueryInfo"/> </returns>
    public QueryInfo QueryStatusReady( TimeSpan readyToQueryTimeout, TimeSpan readyToReadTimeout, string dataToWrite,
        Func<int, bool> isStatusReady, Func<int, (bool, string, int)> isStatusError )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        Stopwatch sw = Stopwatch.StartNew();

        (bool timedOut, int waitStatusByte, TimeSpan elapsed) = this.AwaitStatus( readyToQueryTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready before querying '{dataToWrite}'" );
#endif
        ElapsedTimeSpan timeToQuery = new( ElapsedTimeIdentity.WriteReadyDelay, elapsed );
        TimeSpan temp = sw.Elapsed;

        // check for existing errors.
        (bool hasError, string errorDetails, int errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{errorDetails}' occurred before writing '{dataToWrite}'" );

        // if no error, send message to the instrument.
        string sentMessage = this.WriteLine( dataToWrite );

        ElapsedTimeSpan writeTime = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;

        // wait while busy
        (timedOut, waitStatusByte, elapsed) = this.AwaitStatus( readyToReadTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready reading '{dataToWrite}'" );
#endif
        ElapsedTimeSpan timeToRead = new( ElapsedTimeIdentity.ReadDelay, elapsed );

        // check for query errors:
        (hasError, errorDetails, errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{errorDetails}' occurred querying '{dataToWrite}'" );
        temp = sw.Elapsed;

        // if no error, read:
        string? receivedMessage = this.ReadLine();
        ElapsedTimeSpan readTime = new( ElapsedTimeIdentity.ReadTime, sw.Elapsed.Subtract( temp ) );
        return new QueryInfo( dataToWrite, sentMessage, receivedMessage,
                [timeToQuery, writeTime, timeToRead, readTime, new( ElapsedTimeIdentity.QueryTime, sw.Elapsed )] );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <remarks>
    /// David, 2021-04-05. <para>
    /// Set short ready to query timeout in case of buffer reading. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="readyToQueryTimeout">  The ready to query timeout. </param>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <param name="isStatusReady">        The is status ready. </param>
    /// <param name="hasQueryResult">   The status byte indicates having a query result. </param>
    /// <param name="isStatusError">        The is status error. </param>
    /// <returns> <see cref="QueryInfo"/> </returns>
    public QueryInfo QueryIfStatusReady( TimeSpan readyToQueryTimeout,
        TimeSpan readyToReadTimeout, string dataToWrite, Func<int, bool> isStatusReady, Func<int, bool> hasQueryResult, Func<int, (bool, string, int)> isStatusError )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        Stopwatch sw = Stopwatch.StartNew();

        (bool timedOut, int waitStatusByte, TimeSpan elapsed) = this.AwaitStatus( readyToQueryTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready before querying '{dataToWrite}'" );
#endif
        // check for existing errors.
        (bool hasError, string errorDetails, int errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{errorDetails}' occurred before querying '{dataToWrite}'" );
        ElapsedTimeSpan timeToQuery = new( ElapsedTimeIdentity.WriteReadyDelay, elapsed );
        TimeSpan temp = sw.Elapsed;

        // if no error, send query message to the instrument.
        string sentMessage = this.WriteLine( dataToWrite );

        ElapsedTimeSpan writeTime = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;

        // wait while busy
        (timedOut, waitStatusByte, elapsed) = this.AwaitStatus( readyToReadTimeout, hasQueryResult );
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting query result status after querying '{dataToWrite}'" );

        ElapsedTimeSpan timeToRead = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;

        // check for query errors:
        (hasError, errorDetails, errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{errorDetails}' occurred querying '{dataToWrite}'" );

        // if no error, read:
        string? receivedMessage = this.ReadLine();
        ElapsedTimeSpan readTime = new( ElapsedTimeIdentity.ReadTime, sw.Elapsed.Subtract( temp ) );
        return new QueryInfo( dataToWrite, sentMessage, receivedMessage,
                [timeToQuery, writeTime, timeToRead, readTime, new( ElapsedTimeIdentity.QueryTime, sw.Elapsed )] );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <remarks>
    /// David, 2021-04-05. <para>
    /// Set short ready to query timeout in case of buffer reading. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="statusByte">           The status byte. </param>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <param name="isStatusReady">        The is status ready. </param>
    /// <param name="hasQueryResult">       The is query status ready. </param>
    /// <param name="isStatusError">        The is status error. </param>
    /// <returns> A Tuple: ( bool MessageFetched, string ReceivedMessage, <see cref="QueryInfo"/> QueryInfo ). </returns>
    public QueryInfo QueryIfStatusReady( int statusByte,
        TimeSpan readyToReadTimeout, string dataToWrite, Func<int, bool> isStatusReady, Func<int, bool> hasQueryResult, Func<int, (bool, string, int)> isStatusError )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        Stopwatch sw = Stopwatch.StartNew();

        // check for existing errors.
        (bool hasError, string errorDetails, int errorStatusByte) = isStatusError.Invoke( statusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{statusByte:X2}:'{errorDetails}' occurred before querying '{dataToWrite}'" );

        // check if the status indicates that a query result is available.
        bool queryResultExists = hasQueryResult.Invoke( statusByte );
        if ( !queryResultExists ) return QueryInfo.Empty;

        ElapsedTimeSpan timeToQuery = new( ElapsedTimeIdentity.WriteReadyDelay, sw.Elapsed );
        TimeSpan temp = sw.Elapsed;

        // if no error, send query message to the instrument.
        string sentMessage = this.WriteLine( dataToWrite );

        ElapsedTimeSpan writeTime = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;

        // wait while busy
        (bool timedOut, int waitStatusByte, TimeSpan elapsed) = this.AwaitStatus( readyToReadTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready after querying '{dataToWrite}'" );
#endif
        ElapsedTimeSpan timeToRead = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;

        // check for query errors:
        (hasError, errorDetails, errorStatusByte) = isStatusError.Invoke( statusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{statusByte:X2}:'{errorDetails}' occurred querying '{dataToWrite}'" );

        // if no error, read:
        string? receivedMessage = this.ReadLine();
        ElapsedTimeSpan readTime = new( ElapsedTimeIdentity.ReadTime, sw.Elapsed.Subtract( temp ) );
        return new QueryInfo( dataToWrite, sentMessage, receivedMessage,
                [timeToQuery, writeTime, timeToRead, readTime, new( ElapsedTimeIdentity.QueryTime, sw.Elapsed )] );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <remarks>   David, 2021-04-05. <para>
    /// Set short ready to query timeout in case of buffer reading. </para></remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="readyToQueryTimeout">  The ready to query timeout. </param>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <param name="isStatusReady">        The is status ready. </param>
    /// <param name="isStatusError">        The is status error. </param>
    /// <returns> A Tuple: ( string ReceivedMessage, <see cref="QueryInfo"/> QueryInfo ). </returns>
    public QueryInfo QueryIfStatusReady( TimeSpan readyToQueryTimeout,
        TimeSpan readyToReadTimeout, string dataToWrite, Func<int, bool> isStatusReady, Func<int, (bool, string, int)> isStatusError )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        Stopwatch sw = Stopwatch.StartNew();

        (bool timedOut, int waitStatusByte, TimeSpan elapsed) = this.AwaitStatus( readyToQueryTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready before querying '{dataToWrite}'" );
#endif
        ElapsedTimeSpan timeToQuery = new( ElapsedTimeIdentity.WriteReadyDelay, sw.Elapsed );
        TimeSpan temp = sw.Elapsed;

        // check for existing errors.
        (bool hasError, string errorDetails, int errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{errorDetails}' occurred before querying '{dataToWrite}'" );

        // if no error, send query message to the instrument.
        string sentMessage = this.WriteLine( dataToWrite );

        ElapsedTimeSpan writeTime = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;

        // wait while busy
        (timedOut, waitStatusByte, elapsed) = this.AwaitStatus( readyToReadTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready after querying '{dataToWrite}'" );
#endif
        ElapsedTimeSpan timeToRead = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;

        // check for query errors:
        (hasError, errorDetails, errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{errorDetails}' occurred querying '{dataToWrite}'" );

        // if no error, read:
        string? receivedMessage = this.ReadLine();
        ElapsedTimeSpan readTime = new( ElapsedTimeIdentity.ReadTime, sw.Elapsed.Subtract( temp ) );
        return new QueryInfo( dataToWrite, sentMessage, receivedMessage,
                [timeToQuery, writeTime, timeToRead, readTime, new( ElapsedTimeIdentity.QueryTime, sw.Elapsed )] );
    }

    /// <summary>   Reads if status ready. </summary>
    /// <remarks>   David, 2021-04-16. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <param name="isStatusReady">        The is status ready. </param>
    /// <param name="isStatusError">        The is status error. </param>
    /// <returns>   <see cref="ReadInfo"/> </returns>
    public ReadInfo ReadIfStatusReady( TimeSpan readyToReadTimeout,
        Func<int, bool> isStatusReady, Func<int, (bool, string, int)> isStatusError )
    {
        Stopwatch sw = Stopwatch.StartNew();

        // wait while busy
        (bool timedOut, int waitStatusByte, TimeSpan elapsed) = this.AwaitStatus( readyToReadTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready after querying '{dataToWrite}'" );
#endif
        ElapsedTimeSpan timeToRead = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed );
        TimeSpan temp = sw.Elapsed;

        // check for query errors:
        (bool hasError, string errorDetails, int errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{errorDetails}' attempting to read" );

        // if no error, read:
        string? receivedMessage = this.ReadLine();
        ElapsedTimeSpan readTime = new( ElapsedTimeIdentity.ReadTime, sw.Elapsed.Subtract( temp ) );
        return new ReadInfo( receivedMessage ?? string.Empty,
                [timeToRead, readTime, new( ElapsedTimeIdentity.ReadTime, sw.Elapsed )] );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <remarks>
    /// David, 2021-04-05. <para>
    /// Set short ready to query timeout in case of buffer reading. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="statusByte">           The status byte. </param>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <param name="isStatusReady">        The is status ready. </param>
    /// <param name="isStatusError">        The is status error. </param>
    /// <returns> A Tuple: ( bool MessageFetched, string ReceivedMessage, <see cref="QueryInfo"/> QueryInfo ). </returns>
    public QueryInfo QueryIfStatusReady( int statusByte,
        TimeSpan readyToReadTimeout, string dataToWrite, Func<int, bool> isStatusReady, Func<int, (bool, string, int)> isStatusError )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        Stopwatch sw = Stopwatch.StartNew();

        // check for existing errors.
        (bool hasError, string errorDetails, int errorStatusByte) = isStatusError.Invoke( statusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{statusByte:X2}:'{errorDetails}' occurred before querying '{dataToWrite}'" );

        ElapsedTimeSpan timeToQuery = new( ElapsedTimeIdentity.WriteReadyDelay, sw.Elapsed );
        TimeSpan temp = sw.Elapsed;

        // if no error, send query message to the instrument.
        string sentMessage = this.WriteLine( dataToWrite );

        ElapsedTimeSpan writeTime = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;


        // wait while busy
        (bool timedOut, int waitStatusByte, TimeSpan elapsed) = this.AwaitStatus( readyToReadTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready after querying '{dataToWrite}'" );
#endif
        ElapsedTimeSpan timeToRead = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;

        // check for query errors:
        (hasError, errorDetails, errorStatusByte) = isStatusError.Invoke( statusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{statusByte:X2}:'{errorDetails}' occurred querying '{dataToWrite}'" );

        // if no error, read:
        string? receivedMessage = this.ReadLine();
        ElapsedTimeSpan readTime = new( ElapsedTimeIdentity.ReadTime, sw.Elapsed.Subtract( temp ) );
        return new QueryInfo( dataToWrite, sentMessage, receivedMessage,
                [timeToQuery, writeTime, timeToRead, readTime, new( ElapsedTimeIdentity.QueryTime, sw.Elapsed )] );
    }

    /// <summary>   Awaits for status ready. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="statusReadyTimeout">   The status ready timeout. </param>
    /// <param name="isStatusReady">        The is status ready. </param>
    /// <param name="isStatusError">        The is status error. </param>
    /// <returns>   A Tuple ( string, TimeSpan )( message sent, Elapsed time). </returns>
    public (bool TimedOut, int statusByte, TimeSpan Elapsed) AwaitStatusReady( TimeSpan statusReadyTimeout, Func<int, bool> isStatusReady, Func<int, (bool, string, int)> isStatusError )
    {
        Stopwatch sw = Stopwatch.StartNew();

        (bool timedOut, int waitStatusByte, TimeSpan elapsed) = this.AwaitStatus( statusReadyTimeout, isStatusReady );

        // check for existing errors.
        (bool hasError, string details, int errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        return hasError
            ? throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{details}' occurred awaiting status ready" )
            : (timedOut, waitStatusByte, sw.Elapsed);
    }

    /// <summary>   Writes the status ready. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="readyToWriteTimeout">  The ready to write timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <param name="isStatusReady">        The is status ready. </param>
    /// <param name="isStatusError">        The is status error. </param>
    /// <returns>   <see cref="ExecuteInfo"/>). </returns>
    public ExecuteInfo WriteStatusReady( TimeSpan readyToWriteTimeout, string dataToWrite, Func<int, bool> isStatusReady, Func<int, (bool, string, int)> isStatusError )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        Stopwatch sw = Stopwatch.StartNew();

        (bool timedOut, int waitStatusByte, TimeSpan elapsed) = this.AwaitStatus( readyToWriteTimeout, isStatusReady );
#if false
        // instrument may timeout while still able to proceed. The wait status is effectively used to determine if an error exists from the
        // previous operation
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout awaiting status ready before writing '{dataToWrite}'" );
#endif
        ElapsedTimeSpan timeToQuery = new( ElapsedTimeIdentity.WriteReadyDelay, sw.Elapsed );
        TimeSpan temp = sw.Elapsed;

        // check for existing errors.
        (bool hasError, string details, int errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        if ( hasError )
            throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{details}' occurred before writing '{dataToWrite}'" );

        // if no error, send message to the instrument.
        string? messageSent = this.WriteLine( dataToWrite );

        ElapsedTimeSpan writeTime = new( ElapsedTimeIdentity.WriteTime, sw.Elapsed.Subtract( temp ) );
        temp = sw.Elapsed;

        // check for syntax error:
        (_, waitStatusByte, _) = this.AwaitStatus( readyToWriteTimeout, isStatusReady );

        // check for existing errors.
        (hasError, details, errorStatusByte) = isStatusError.Invoke( waitStatusByte );
        return hasError
            ? throw new InvalidOperationException( $"Status error 0x{errorStatusByte:X2}:'{details}' occurred writing '{dataToWrite}'" )
            : new ExecuteInfo( dataToWrite, messageSent,
                               [timeToQuery, writeTime, new( ElapsedTimeIdentity.TotalWriteTime, sw.Elapsed )] );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <param name="statusReadDelay"> The read delay. </param>
    /// <param name="readDelay">       The read delay. </param>
    /// <param name="dataToWrite">     The data to write. </param>
    /// <returns> The trim end. </returns>
    public string? QueryTrimEnd( TimeSpan statusReadDelay, TimeSpan readDelay, string dataToWrite )
    {
        return this.QueryIfNoStatusError( statusReadDelay, readDelay, dataToWrite ).ReceivedMessage?.TrimEnd( this.TerminationCharacters() );
    }

    #endregion
}
