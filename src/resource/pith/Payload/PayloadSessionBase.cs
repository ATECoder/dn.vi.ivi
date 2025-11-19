// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " query payload "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// Parses the Boolean return value.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="payload"> The payload. </param>
    /// <returns>
    /// <c>true</c> if <see cref="VI.Pith.PayloadStatus"/> is
    /// <see cref="VI.Pith.PayloadStatus.Okay"/>; otherwise <c>false</c>.
    /// </returns>
    public bool Query( PayloadBase payload )
    {
        if ( payload is null ) throw new ArgumentNullException( nameof( payload ) );
        this.EmulatedReply = payload.SimulatedPayload;
        payload.QueryStatus = PayloadStatus.Okay | PayloadStatus.Sent;
        payload.QueryMessage = payload.BuildQueryCommand();
        if ( string.IsNullOrWhiteSpace( payload.QueryMessage ) ) throw new ArgumentException( $"{nameof( payload )}.{nameof( payload.QueryMessage )} is null or white space." );
        payload.ReceivedMessage = this.QueryTrimEnd( payload.QueryMessage! );
        payload.QueryStatus |= PayloadStatus.QueryReceived;
        payload.FromReading( payload.ReceivedMessage ?? string.Empty );
        return PayloadStatus.Okay == (payload.QueryStatus & PayloadStatus.Okay);
    }

    #endregion

    #region " write payload "

    /// <summary> Writes the payload to the device. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="payload"> The payload. </param>
    /// <returns>
    /// <c>true</c> if <see cref="VI.Pith.PayloadStatus"/> is
    /// <see cref="VI.Pith.PayloadStatus.Okay"/>; otherwise <c>false</c>.
    /// </returns>
    public bool Write( PayloadBase payload )
    {
        if ( payload is null ) throw new ArgumentNullException( nameof( payload ) );
        payload.CommandStatus = PayloadStatus.Okay | PayloadStatus.Sent;
        payload.SentMessage = payload.BuildCommand();
        if ( string.IsNullOrWhiteSpace( payload.SentMessage ) ) throw new ArgumentException( $"{nameof( payload )}.{nameof( payload.SentMessage )} is null or white space." );
        _ = this.WriteLine( payload.SentMessage! );
        return PayloadStatus.Okay == (payload.CommandStatus & PayloadStatus.Okay);
    }

    #endregion
}
