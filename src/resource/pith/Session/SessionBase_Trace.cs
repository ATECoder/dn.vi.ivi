using cc.isr.Std.StackTraceExtensions;
using cc.isr.Std.TrimExtensions;
using cc.isr.VI.Pith.ExceptionExtensions;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    /// <summary>
    /// Check if status error. If so, trace a <see cref="DeviceException"/> and error the error count.
    /// </summary>
    /// <remarks>
    /// This was moved from the link subsystem and I am not sure why this special method was
    /// constructed.
    /// </remarks>
    /// <param name="controllerNodeNumber"> The controller node number. </param>
    /// <param name="useLastActionDetails"> (Optional) True to use last action details. </param>
    /// <param name="failureMessage">       (Optional) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public (ServiceRequests StatusByte, string Details) TraceDeviceExceptionIfError( int controllerNodeNumber, bool useLastActionDetails = false,
        string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        _ = this.TraceInformation( useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
        (ServiceRequests statusByte, string details) = this.TraceDeviceExceptionIfError( useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
        if ( !this.IsErrorBitSet( statusByte ) && this.LastNodeNumber != controllerNodeNumber )
        {
            int? errorCount = this.QueryErrorQueueCount().GetValueOrDefault( 0 );
            if ( errorCount > 0 )
                details = cc.isr.VI.SessionLogger.Instance.LogWarning(
                $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {failureMessage}: encountered {this.ErrorQueueCount} device errors.\n\t{new StackFrame( true ).UserCallStack()}",
                memberName, sourcePath, sourceLineNumber );
        }

        return (statusByte, details);
    }

    /// <summary>
    /// Trace a <see cref="DeviceException"/> if status error reporting the last session <see cref="SessionBase.LastAction"/>
    /// or <see cref="SessionBase.LastActionDetails"/>.
    /// </summary>
    /// <remarks>   2024-09-11. </remarks>
    /// <param name="useLastActionDetails"> (Optional) True to use last action message. </param>
    /// <param name="failureMessage">       (Optional) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    /// <returns>   <c>true</c> if no errors; otherwise, <c>false</c>. </returns>
    public (ServiceRequests StatusByte, string Details) TraceDeviceExceptionIfError( bool useLastActionDetails = false,
        string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay );
        return this.TraceDeviceExceptionIfError( this.ReadStatusByte(), useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
    }

    /// <summary>
    /// Trace a <see cref="DeviceException"/> if status error reporting the last session <see cref="SessionBase.LastAction"/>
    /// or <see cref="SessionBase.LastActionDetails"/>.
    /// </summary>
    /// <remarks>   2024-09-11. </remarks>
    /// <param name="statusByte">           The status byte. </param>
    /// <param name="useLastActionDetails"> (Optional) True to use last action message. </param>
    /// <param name="failureMessage">       (Optional) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    /// <returns>   <c>true</c> if no errors; otherwise, <c>false</c>. </returns>
    public (ServiceRequests StatusByte, string Details) TraceDeviceExceptionIfError( ServiceRequests statusByte, bool useLastActionDetails = false,
        string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string details = string.Empty;
        if ( this.IsErrorBitSet( statusByte ) )
        {
            _ = this.QueryDeviceErrors( statusByte );

            details = this.TraceDeviceException( useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
        }
        return (statusByte, details);
    }

    /// <summary>
    /// Check if status error. If so, trace a <see cref="DeviceException"/> and error the error count.
    /// </summary>
    /// <remarks>
    /// This was moved from the link subsystem and I am not sure why this special method was
    /// constructed.
    /// </remarks>
    /// <param name="messageBitTimeout">    The operation completion timeout for waiting for the status
    ///                                     byte message bit when discarding unread data. </param>
    /// <param name="useLastActionDetails"> (Optional) True to use last action details. </param>
    /// <param name="failureMessage">       (Optional) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public (ServiceRequests StatusByte, string Details) TraceDeviceExceptionIfError( TimeSpan messageBitTimeout, bool useLastActionDetails = false,
        string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay + this.StatusReadDelay );
        return this.TraceDeviceExceptionIfError( this.ReadStatusByte(), messageBitTimeout, useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
    }

    /// <summary>
    /// Check if status error. If so, trace a <see cref="DeviceException"/> and error the error count.
    /// </summary>
    /// <remarks>
    /// This was moved from the link subsystem and I am not sure why this special method was
    /// constructed.
    /// </remarks>
    /// <param name="statusByte">           The status byte. </param>
    /// <param name="messageBitTimeout">    The operation completion timeout for waiting for the status
    ///                                     byte message bit when discarding unread data. </param>
    /// <param name="useLastActionDetails"> (Optional) True to use last action details. </param>
    /// <param name="failureMessage">       (Optional) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public (ServiceRequests StatusByte, string Details) TraceDeviceExceptionIfError( ServiceRequests statusByte, TimeSpan messageBitTimeout, bool useLastActionDetails = false,
        string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string details = string.Empty;
        if ( this.IsErrorBitSet( statusByte ) )
        {
            _ = this.DiscardUnreadData( messageBitTimeout );

            _ = this.QueryDeviceErrors( statusByte );

            details = this.TraceDeviceException( useLastActionDetails, failureMessage, memberName, sourcePath, sourceLineNumber );
        }
        return (statusByte, details);
    }

    /// <summary>   Trace device exception. </summary>
    /// <remarks>   2024-09-12. </remarks>
    /// <param name="useLastActionDetails"> (Optional) True to use last action message. </param>
    /// <param name="failureMessage">       (Optional) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    /// <returns>   A string. </returns>
    public string TraceDeviceException( bool useLastActionDetails = false, string failureMessage = "failed",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        System.Text.StringBuilder builder = new();
        // and trace the message
        if ( this.HasErrorReport )
            _ = builder.AppendLine( cc.isr.VI.SessionLogger.Instance.LogWarning(
                $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {failureMessage} with device errors:\n\t{this.DeviceErrorPreamble}\n\t{this.DeviceErrorReport}\n\t{new StackFrame( true ).UserCallStack()}",
                memberName, sourcePath, sourceLineNumber ) );
        if ( this.HasDeviceError )
            _ = builder.AppendLine( cc.isr.VI.SessionLogger.Instance.LogWarning(
                $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {failureMessage} with device error:\n\t{this.DeviceErrorPreamble}\n\t{this.LastErrorCompoundErrorMessage}\n\t{new StackFrame( true ).UserCallStack()}",
                memberName, sourcePath, sourceLineNumber ) );
        else
            _ = builder.AppendLine( cc.isr.VI.SessionLogger.Instance.LogWarning(
                $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {failureMessage} fetching device errors.\n\t{new StackFrame( true ).UserCallStack()}",
                memberName, sourcePath, sourceLineNumber ) );
        return builder.ToString().TrimEndNewLine();
    }

    /// <summary>   Trace failed operation. Can be used with queries. </summary>
    /// <remarks>   2024-09-12. </remarks>
    /// <param name="ex">                   The exception. </param>
    /// <param name="useLastActionDetails"> (Optional) True to use last action message. </param>
    /// <param name="failureMessage">       (Optional) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    public string TraceException( Exception ex, bool useLastActionDetails = false, string failureMessage = "exception",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        if ( ex is not null )
        {
            _ = ex.AddExceptionData();
            return cc.isr.VI.SessionLogger.Instance.LogError(
                $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {failureMessage}: {ex.Message}\n\t{new StackFrame( true ).UserCallStack()}",
                memberName, sourcePath, sourceLineNumber );
        }
        else
            return string.Empty;

    }

    /// <summary>   Trace visa failed operation. Can be used with queries. </summary>
    /// <remarks>   2024-09-12. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="ex">                   The exception. </param>
    /// <param name="useLastActionDetails"> (Optional) True to use last action message. </param>
    /// <param name="failureMessage">       (Optional) Message describing the failure. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    public string TraceException( Pith.NativeException ex, bool useLastActionDetails = false, string failureMessage = "VISA error",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        return cc.isr.VI.SessionLogger.Instance.LogWarning(
            $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {failureMessage}: {ex.InnerError?.BuildErrorCodeDetails()}\n\t{new StackFrame( true ).UserCallStack()}",
            memberName, sourcePath, sourceLineNumber );
    }

    /// <summary>   Trace information. </summary>
    /// <remarks>   2024-09-12. </remarks>
    /// <param name="useLastActionDetails"> (Optional) True to use last action message. </param>
    /// <param name="message">              (Optional) The message. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    public string TraceInformation( bool useLastActionDetails = false, string message = "done",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        return cc.isr.VI.SessionLogger.Instance.LogVerbose(
            $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {message}",
            memberName, sourcePath, sourceLineNumber );
    }

    /// <summary>   Trace warning. </summary>
    /// <remarks>   2024-09-12. </remarks>
    /// <param name="useLastActionDetails"> (Optional) True to use last action message. </param>
    /// <param name="message">              (Optional) The message. </param>
    /// <param name="memberName">           (Optional) Name of the member. </param>
    /// <param name="sourcePath">           (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber">     (Optional) Source line number. </param>
    public string TraceWarning( bool useLastActionDetails = false, string message = "done",
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        return cc.isr.VI.SessionLogger.Instance.LogWarning(
            $"{(useLastActionDetails ? this.LastActionDetails : this.LastAction)} {message}",
            memberName, sourcePath, sourceLineNumber );
    }

}
