// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Pith;

public abstract partial class SessionBase
{
    /// <summary>   Await query result status ready. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="readyToQueryTimeout">  The ready to query timeout. </param>
    /// <returns>   A Tuple: (bool TimedOut, int StatusByte, TimeSpan Elapsed). </returns>
    public (bool TimedOut, int StatusByte, TimeSpan Elpased) AwaitQueryResultStatusReady( TimeSpan readyToQueryTimeout )
    {
        return this.AwaitStatusReady( readyToQueryTimeout, this.IsStatusQueryResultReady, this.IsStatusError );
    }

    /// <summary>   Queries if status data ready. </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <param name="readyToQueryTimeout">  The ready to query timeout. </param>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <returns>   A Tuple: (bool MessageFetched, string ReceivedMessage, QueryInfo QueryInfo). </returns>
    public QueryInfo QueryIfStatusDataReady( TimeSpan readyToQueryTimeout, TimeSpan readyToReadTimeout, string dataToWrite )
    {
        return this.QueryIfStatusReady( readyToQueryTimeout, readyToReadTimeout, dataToWrite, this.IsStatusDataReady, this.IsStatusError );
    }

    /// <summary>   Queries if status data ready. </summary>
    /// <remarks>   David, 2021-04-06. </remarks>
    /// <param name="statusByte">           The status byte. </param>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <returns> QueryInfo </returns>
    public QueryInfo QueryIfStatusDataReady( int statusByte, TimeSpan readyToReadTimeout, string dataToWrite )
    {
        return this.QueryIfStatusReady( statusByte, readyToReadTimeout, dataToWrite, ( statusByte ) => !this.IsStatusBusy( statusByte ),
                                                            this.IsStatusDataReady, this.IsStatusError );
    }

    /// <summary>   Queries the specified data to write checking for status ready. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="readyToQueryTimeout">  The ready to query timeout. </param>
    /// <param name="readyToReadTimeout">   The ready to read timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <returns>   A Tuple: QueryInfo . </returns>
    public QueryInfo QueryStatusReady( TimeSpan readyToQueryTimeout, TimeSpan readyToReadTimeout, string dataToWrite )
    {
        return this.QueryStatusReady( readyToQueryTimeout, readyToReadTimeout, dataToWrite, ( statusByte ) => !this.IsStatusBusy( statusByte ), this.IsStatusError );
    }

    /// <summary>   Writes the specified data to write checking for status ready. </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="readyToWriteTimeout">  The ready to write timeout. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <returns>   <see cref="ExecuteInfo"/>. </returns>
    public ExecuteInfo WriteStatusReady( TimeSpan readyToWriteTimeout, string dataToWrite )
    {
        return this.WriteStatusReady( readyToWriteTimeout, dataToWrite, ( statusByte ) => !this.IsStatusBusy( statusByte ), this.IsStatusError );
    }

    /// <summary>   Await status. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <param name="timeout">  The timeout. </param>
    /// <returns>   A Tuple. </returns>
    public virtual (bool TimedOut, int StatusByte, TimeSpan Elapsed) AwaitStatus( TimeSpan timeout )
    {
        return this.AwaitStatus( timeout, ( statusByte ) => !this.IsStatusBusy( statusByte ) );
    }

}
