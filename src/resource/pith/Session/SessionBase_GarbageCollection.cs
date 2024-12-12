namespace cc.isr.VI.Pith;

public abstract partial class SessionBase
{
    /// <summary>   Gets or sets the 'collect garbage query complete' command. </summary>
    /// <value> The 'collect garbage query complete' command. </value>
    public string CollectGarbageQueryCompleteCommand { get; protected set; } = string.Empty;

    /// <summary>   A Pith.SessionBase extension method that collect garbage with a complete query. </summary>
    /// <remarks>   2024-09-11. </remarks>
    /// <returns>   A Tuple. </returns>
    public bool CollectGarbageQueryComplete()
    {
        if ( !this.IsSessionOpen || string.IsNullOrWhiteSpace( this.CollectGarbageQueryCompleteCommand ) ) return true;

        // send the collect garbage command
        this.SetLastAction( $"writing collect garbage query command" );
        _ = this.WriteLine( this.CollectGarbageQueryCompleteCommand );
        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay );

        this.SetLastAction( $"reading garbage collection completion reply" );
        string reply = this.ReadLineTrimEnd();
        Console.WriteLine( $"Received {reply} from complete query after garbage collection" );
        return cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue == reply;
    }

    /// <summary>   Gets or sets the 'collect garbage operation complete' command. </summary>
    /// <value> The 'collect garbage operation complete' command. </value>
    public string CollectGarbageOperationCompleteCommand { get; protected set; } = string.Empty;

    /// <summary>   A Pith.SessionBase extension method that collect garbage wait complete. </summary>
    /// <remarks>   2024-09-11. </remarks>
    /// <param name="operationCompletionTimeout">   The operation completion timeout. </param>
    /// <returns>   A Tuple. </returns>
    public (ServiceRequests Status, string Details) CollectGarbageWaitComplete( TimeSpan operationCompletionTimeout )
    {
        if ( !this.IsSessionOpen || string.IsNullOrWhiteSpace( this.CollectGarbageOperationCompleteCommand ) ) return (ServiceRequests.None, string.Empty);

        // send the collect garbage command
        this.SetLastAction( $"writing collect garbage wait complete" );

        _ = this.WriteLine( Syntax.Tsp.Lua.CollectGarbageOperationCompleteCommand );

        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay );
        ServiceRequests statusByte = this.ReadStatusByte();
        if ( this.IsErrorBitSet( statusByte ) )
            return (statusByte, $"Status error after {this.LastAction}.");

        this.SetLastAction( $"waiting for operation completion" );
        (bool timedOut, statusByte, TimeSpan elapsed) = this.AwaitOperationCompletion( operationCompletionTimeout );
        return this.IsErrorBitSet( statusByte )
            ? (statusByte, $"Status error after {this.LastAction}; {timedOut.GetHashCode():'timed out';'timedout';'completed'} after {elapsed:s\\.fff}ms.")
            : (statusByte, string.Empty);
    }

    /// <summary>   Gets or sets the collect garbage query complete command format. </summary>
    /// <value> The collect garbage query complete command format. </value>
    public string CollectGarbageQueryCompleteCommandFormat { get; protected set; } = string.Empty;

    /// <summary>   A Pith.SessionBase extension method that collect garbage query  complete. </summary>
    /// <remarks>   2024-09-11. </remarks>
    /// <param name="nodeNumber">                   The node number. </param>
    /// <returns>   A Tuple. </returns>
    public bool CollectGarbageQueryComplete( int nodeNumber )
    {
        if ( !this.IsSessionOpen || string.IsNullOrWhiteSpace( this.CollectGarbageOperationCompleteCommandFormat ) ) return true;

        this.LastNodeNumber = nodeNumber;

        // send the collect garbage command
        this.SetLastAction( $"writing node collect garbage query complete command" );
        _ = this.WriteLine( this.CollectGarbageQueryCompleteCommandFormat, nodeNumber );

        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay );

        this.SetLastAction( $"reading garbage collection completion reply" );
        string reply = this.ReadLineTrimEnd();
        Console.WriteLine( $"Received {reply} from complete query after garbage collection" );
        return cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue == reply;
    }


    /// <summary>   Gets or sets the collect garbage operation complete command format. </summary>
    /// <value> The collect garbage operation complete command format. </value>
    public string CollectGarbageOperationCompleteCommandFormat { get; protected set; } = string.Empty;

    /// <summary>   A Pith.SessionBase extension method that collect garbage wait complete. </summary>
    /// <remarks>   2024-09-11. </remarks>
    /// <param name="nodeNumber">                   The node number. </param>
    /// <param name="operationCompletionTimeout">   The operation completion timeout. </param>
    /// <returns>   A Tuple. </returns>
    public (ServiceRequests Status, string Details) CollectGarbageWaitComplete( int nodeNumber, TimeSpan operationCompletionTimeout )
    {
        if ( !this.IsSessionOpen || string.IsNullOrWhiteSpace( this.CollectGarbageOperationCompleteCommandFormat ) ) return (ServiceRequests.None, string.Empty);

        this.LastNodeNumber = nodeNumber;

        // send the collect garbage command
        this.SetLastAction( $"writing {string.Format( Syntax.Tsp.Node.CollectGarbageOperationCompleteCommandFormat, nodeNumber )}" );
        _ = this.WriteLine( this.CollectGarbageOperationCompleteCommandFormat, nodeNumber );

        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay );
        ServiceRequests statusByte = this.ReadStatusByte();
        if ( this.IsErrorBitSet( statusByte ) )
            return (statusByte, $"Status error after {this.LastAction}.");

        this.SetLastAction( $"waiting for operation completion on node {nodeNumber}" );
        (bool timedOut, statusByte, TimeSpan elapsed) = this.AwaitOperationCompletion( operationCompletionTimeout );
        return this.IsErrorBitSet( statusByte )
            ? (statusByte, $"Status error after {this.LastAction}; {timedOut.GetHashCode():'timed out';'timedout';'completed'} after {elapsed:s\\.fff}ms.")
            : (statusByte, string.Empty);
    }

}
