namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    /// <summary>   Query and throw an exception if operation incomplete. </summary>
    /// <remarks>   2024-10-04. </remarks>
    /// <param name="failureMessage">                   (Optional) (empty) Message describing the failure. </param>
    /// <param name="readAfterWriteDelayMilliseconds">  (Optional) (0) The asynchronous delay; uses read
    ///                                                 after write plus status delay if zero. </param>
    /// <param name="communicationTimeoutMilliseconds"> (Optional) (10000) The communication timeout in
    ///                                                 milliseconds. </param>
    /// <param name="memberName">                       (Optional) Name of the member. </param>
    /// <param name="sourcePath">                       (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">                 (Optional) Source line number. </param>
    public void QueryAndThrowIfOperationIncomplete( string failureMessage = "",
        int readAfterWriteDelayMilliseconds = 0,
        int communicationTimeoutMilliseconds = 10000,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay );
        _ = this.WriteLine( this.OperationCompletedQueryCommand );

        // read query reply and throw if reply is not 1.
        this.ReadAndThrowIfOperationIncomplete( failureMessage, readAfterWriteDelayMilliseconds, communicationTimeoutMilliseconds, memberName, sourcePath, sourceLineNumber );

        // throw if device errors
        this.ThrowDeviceExceptionIfError();
    }

    /// <summary>   Read and throw an exception if operation incomplete. </summary>
    /// <remarks>   2024-09-21. </remarks>
    /// <exception cref="NativeException">  Thrown when a Native error condition occurs. </exception>
    /// <param name="failureMessage">                   (Optional) (empty) Message describing the
    ///                                                 failure. </param>
    /// <param name="readAfterWriteDelayMilliseconds">  (Optional) (0) The asynchronous delay; uses
    ///                                                 read after write plus status delay if zero. </param>
    /// <param name="communicationTimeoutMilliseconds"> (Optional) (10000) The communication timeout
    ///                                                 in milliseconds. </param>
    /// <param name="memberName">                       (Optional) Name of the member. </param>
    /// <param name="sourcePath">                       (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">                 (Optional) Source line number. </param>
    public void ReadAndThrowIfOperationIncomplete( string failureMessage = "",
        int readAfterWriteDelayMilliseconds = 0,
        int communicationTimeoutMilliseconds = 10000,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        _ = readAfterWriteDelayMilliseconds == 0
            ? SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay )
            : SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( readAfterWriteDelayMilliseconds ) );

        try
        {
            this.StoreCommunicationTimeout( TimeSpan.FromMilliseconds( communicationTimeoutMilliseconds ) );

            string reply = this.ReadLineTrimEnd();
            if ( cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue != reply )
            {
                string location = $"at [{sourcePath}].{memberName}.Line#{sourceLineNumber}";
                if ( string.IsNullOrWhiteSpace( failureMessage ) )
                    failureMessage = $"operation incomplete (replied '{reply}' instead of '{cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue}')";
                throw new NativeException( $"{this.ResourceNameNodeCaption} {failureMessage} after {this.LastAction}\n\t{location}" );
            }
        }
        catch ( Exception )
        {
            throw;
        }
        finally
        {
            this.RestoreCommunicationTimeout();
        }
    }

    /// <summary>
    /// Check if status error. If so, discard unread data and throw a <see cref="DeviceException"/>.
    /// </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <exception cref="DeviceException">  Thrown when a Device error condition occurs. </exception>
    /// <param name="messageBitTimeout">    The operation completion timeout for waiting for the status
    ///                                     byte message bit when discarding unread data. </param>
    /// <param name="useLastActionDetails"> (Optional) (false) True to use last action details. </param>
    /// <param name="failureMessage">       (Optional) (empty) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    public void ThrowDeviceExceptionIfError( TimeSpan messageBitTimeout, bool useLastActionDetails = false,
        string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay );
        this.ThrowDeviceExceptionIfError( this.ReadStatusByte(), messageBitTimeout, useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
    }

    /// <summary>
    /// Check if status error. If so, discard unread data and throw a <see cref="DeviceException"/>.
    /// </summary>
    /// <remarks>   2024-09-17. </remarks>
    /// <param name="statusByte">           The status byte. </param>
    /// <param name="messageBitTimeout">    The operation completion timeout for waiting for the status
    ///                                     byte message bit when discarding unread data. </param>
    /// <param name="useLastActionDetails"> (Optional) (false) True to use last action details. </param>
    /// <param name="failureMessage">       (Optional) (empty) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    public void ThrowDeviceExceptionIfError( ServiceRequests statusByte, TimeSpan messageBitTimeout, bool useLastActionDetails = false,
        string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        if ( this.IsErrorBitSet( statusByte ) )
        {
            _ = this.DiscardUnreadData( messageBitTimeout );

            _ = this.QueryDeviceErrors( statusByte );

            this.ThrowDeviceException( useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
        }
    }

    /// <summary>
    /// Throw the <see cref="DeviceException"/> if status error reporting the last session <see cref="SessionBase.LastAction"/>
    /// or <see cref="SessionBase.LastActionDetails"/>.
    /// </summary>
    /// <remarks>   2024-09-11. </remarks>
    /// <param name="useLastActionDetails"> (Optional) (false) True to use last action detains. </param>
    /// <param name="failureMessage">       (Optional) (empty) The failure message. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    public void ThrowDeviceExceptionIfError( bool useLastActionDetails = false,
        string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay );
        this.ThrowDeviceExceptionIfError( this.ReadStatusByte(), useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
    }

    /// <summary>
    /// Throw the <see cref="DeviceException"/> if status error reporting the last session <see cref="SessionBase.LastAction"/>
    /// or <see cref="SessionBase.LastActionDetails"/>.
    /// </summary>
    /// <remarks>   2024-09-11. </remarks>
    /// <exception cref="DeviceException">  Thrown when a Device error condition occurs. </exception>
    /// <param name="statusByte">           The status byte. </param>
    /// <param name="useLastActionDetails"> (Optional) (false) True to use last action detains. </param>
    /// <param name="failureMessage">       (Optional) (empty) The failure message. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    public void ThrowDeviceExceptionIfError( ServiceRequests statusByte, bool useLastActionDetails = false,
        string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        if ( this.IsErrorBitSet( statusByte ) )
        {
            _ = this.QueryDeviceErrors( statusByte );

            this.ThrowDeviceException( useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
        }
    }

    /// <summary>   Throw device exception. </summary>
    /// <remarks>   2024-09-12. </remarks>
    /// <exception cref="DeviceException">  Thrown when a Device error condition occurs. </exception>
    /// <param name="useLastActionDetails"> (Optional) (false) True to use last action detains. </param>
    /// <param name="failureMessage">       (Optional) (empty) The failure message. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    public void ThrowDeviceException( bool useLastActionDetails = false, string failureMessage = "failed ",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string location = $"at [{sourcePath}].{memberName}.Line#{sourceLineNumber}";

        if ( this.HasErrorReport )
            throw new DeviceException( this.ResourceNameCaption ?? string.Empty,
                $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {failureMessage} with device errors\n\t{location}:\n\t{this.DeviceErrorPreamble}\n{this.DeviceErrorReport}." );
        if ( this.HasDeviceError )
            throw new DeviceException( this.ResourceNameCaption ?? string.Empty,
                $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {failureMessage} with device error\n\t{location}:\n\t{this.DeviceErrorPreamble}\n\t{this.LastErrorCompoundErrorMessage}." );
        else
            throw new DeviceException( this.ResourceNameCaption ?? string.Empty,
                $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {failureMessage} fetching device errors {location}." );
    }
}
