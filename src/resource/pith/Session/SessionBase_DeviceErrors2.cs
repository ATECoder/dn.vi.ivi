using cc.isr.Std.EscapeSequencesExtensions;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.Std.TrimExtensions;

namespace cc.isr.VI.Pith;

public abstract partial class SessionBase
{
    #region " device errors: clear "

    /// <summary> Gets or sets the clear error queue command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:QUE:CLEAR".
    /// <see cref="VI.Syntax.ScpiSyntax.ClearErrorQueueCommand"> </see>
    /// </remarks>
    /// <value> The clear error queue command. </value>
    public string ClearErrorQueueCommand { get; set; } = VI.Syntax.ScpiSyntax.ClearSystemErrorQueueCommand;

    /// <summary> Clears messages from the error queue. </summary>
    /// <remarks>
    /// Sends the <see cref="ClearErrorQueueCommand">clear error queue</see> message.
    /// </remarks>
    public virtual void ClearErrorQueue()
    {
        this.ClearErrorCache();
        if ( !string.IsNullOrWhiteSpace( this.ClearErrorQueueCommand ) )
        {
            this.SetLastAction( "clearing the error queue" );
            _ = this.WriteLine( this.ClearErrorQueueCommand );
            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );
        }
        this.ErrorQueueCount = new int?();
    }

    /// <summary> Clears the error cache. </summary>
    public virtual void ClearErrorCache()
    {
        this.ClearErrorReport();
        this.DeviceErrorQueue.Clear();
        this.DeviceErrorBuilder = new System.Text.StringBuilder();
    }

    #endregion

    #region " device errors: query commands "

    /// <summary> Gets or sets the last error query command. </summary>
    /// <remars>
    /// ATS600: EROR? <para>
    /// EG2000: ?E     </para><para>
    /// K3458: ERRSTR? </para><para>
    /// T1700: U1x </para>
    /// </remars>
    /// <value> The last error query command. </value>
    public string DeviceErrorQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets or sets the last system error query command. </summary>
    /// <remarks> <see cref="VI.Syntax.ScpiSyntax.LastSystemErrorQueryCommand"/>
    /// e.g., <c>:SYST:ERR?</c>
    /// </remarks>
    /// <value> The last error query command. </value>
    public string LastSystemErrorQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets or sets the 'Next Error' query command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:QUE?" <para>
    /// <c>_G.print(string.format('%d,%s,level=%d',_G.errorqueue.next()))</c> </para><para>
    /// <c>_G.print(string.format('%d,%s,level=%d',_G.eventlog.next(eventlog.SEV_ERROR)))</c> </para>
    /// <see cref="VI.Syntax.ScpiSyntax.NextErrorQueryCommand"> </see>
    /// </remarks>
    /// <value> The error queue query command. </value>
    public string NextDeviceErrorQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets or sets the 'Next Error' query command. </summary>
    /// <remarks>
    /// Sample commands:  <para>
    /// :SYST:ERR?  </para>
    /// </remarks>
    /// <value> The error queue query command. </value>
    public string DequeueErrorQueryCommand { get; set; } = string.Empty;

    #endregion

    #region " error queue count "

    /// <summary> Gets or sets the cached error queue count. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? ErrorQueueCount
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets The ErrorQueueCount query command. </summary>
    /// <value> The ErrorQueueCount query command. </value>
    public string ErrorQueueCountQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The ErrorQueueCount. </summary>
    /// <returns> The ErrorQueueCount or none if unknown. </returns>
    public int? QueryErrorQueueCount()
    {
        this.ErrorQueueCount = this.Query( this.ErrorQueueCount, this.ErrorQueueCountQueryCommand );
        return this.ErrorQueueCount;
    }

    #endregion

    #region " device errors: read and report "

    /// <summary> Gets or sets the device errors queue. </summary>
    /// <value> The device errors. </value>
    public DeviceErrorQueue DeviceErrorQueue { get; private set; }

    /// <summary> Gets or sets a message describing the no error compound message. </summary>
    /// <value> A message describing the no error compound. </value>
    public string NoErrorCompoundMessage { get; set; }

    /// <summary> Gets or sets the reading device errors. </summary>
    /// <value> The reading device errors. </value>
    public bool ReadingDeviceErrors
    {
        get;

        protected set
        {
            if ( this.SetProperty( ref field, value ) )
                if ( value ) _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} Reading device errors;. " );
        }
    }

    /// <summary> The device errors. </summary>
    /// <value> The device error builder. </value>
    protected System.Text.StringBuilder DeviceErrorBuilder { get; set; }

    /// <summary> Enqueues device error. </summary>
    /// <param name="compoundErrorMessage"> Message describing the compound error. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    protected virtual DeviceError EnqueueDeviceError( string compoundErrorMessage )
    {
        DeviceError deviceError = (this.CommandLanguage == Syntax.CommandLanguage.Scpi)
            ? new( this.NoErrorCompoundMessage )
            : new Syntax.Tsp.TspDeviceError( this.NoErrorCompoundMessage );
        deviceError.Parse( compoundErrorMessage );
        if ( deviceError.IsError )
            this.DeviceErrorQueue.Enqueue( deviceError );
        return deviceError;
    }

    /// <summary> Enqueue last error. </summary>
    /// <param name="compoundErrorMessage"> Message describing the compound error. </param>
    /// <remarks> This is used for reading device errors for the EG2000 prober.</remarks>
    /// <returns> A DeviceError. </returns>
    public DeviceError EnqueueLastError( string compoundErrorMessage )
    {
        DeviceError de = this.EnqueueDeviceError( compoundErrorMessage );
        if ( de.IsError )
        {
            _ = this.DeviceErrorBuilder.AppendLine( $"{this.ResourceNameCaption} Last Error: \n{de.ErrorMessage}" );
        }

        return de;
    }

    /// <summary>   Reads the device errors. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>   The device errors. </returns>
    protected virtual string QueryErrorQueue( ServiceRequests statusByte, string queryCommand )
    {
        if ( !this.IsErrorBitSet( statusByte ) || queryCommand is null || string.IsNullOrWhiteSpace( queryCommand ) )
            return string.Empty;
        try
        {
            System.Text.StringBuilder builder = new();
            if ( !string.IsNullOrWhiteSpace( queryCommand ) && this.IsErrorBitSet( statusByte ) )
            {
                DeviceError de = new( this.NoErrorCompoundMessage );
                do
                {
                    de = this.EnqueueDeviceError( this.QueryTrimEnd( queryCommand ) );

                    if ( de.IsError )
                    {
                        _ = builder.AppendLine( de.CompoundErrorMessage );

                        _ = SessionBase.AsyncDelay( this.StatusReadDelay );
                        statusByte = this.ReadStatusByte();
                    }
                }
                while ( this.IsErrorBitSet( statusByte ) && de.IsError );

                // this is a kludge because the 7510 does not clear the error queue.
                if ( !de.IsError && this.IsErrorBitSet( statusByte ) )
                    _ = this.WriteLine( this.ClearErrorQueueCommand );
            }

            return builder.ToString().TrimEndNewLine();
        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary>   Enqueue device error. </summary>
    /// <remarks>   2024-10-07. </remarks>
    /// <returns>   A DeviceError? </returns>
    public virtual DeviceError? EnqueueDeviceError()
    {
        System.Text.StringBuilder builder = new();

        // There are currently two queue reading commands and two single error reading commands.
        string queryCommand = !string.IsNullOrWhiteSpace( this.NextDeviceErrorQueryCommand )
            ? this.NextDeviceErrorQueryCommand
            : !string.IsNullOrWhiteSpace( this.DequeueErrorQueryCommand )
            ? this.DequeueErrorQueryCommand
            : string.Empty;

        DeviceError? de = null;
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
        {
            de = this.EnqueueDeviceError( this.QueryTrimEnd( queryCommand ) );
        }

        else if ( !string.IsNullOrWhiteSpace( this.DeviceErrorQueryCommand ) )
        {
            // Query last error:
            de = this.EnqueueDeviceError( this.QueryTrimEnd( this.DeviceErrorQueryCommand ) );
        }
        else if ( !string.IsNullOrWhiteSpace( this.LastSystemErrorQueryCommand ) )
        {
            // Query last system error:
            _ = this.WriteLine( this.LastSystemErrorQueryCommand );
            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );
            Pith.SessionBase.DoEventsAction?.Invoke();

            de = this.EnqueueDeviceError( this.ReadLine() );
        }
        return de;
    }

    /// <summary>   Queries any existing device errors. </summary>
    /// <remarks>
    /// 2024-09-02. <para>
    /// The errors are stored in <see cref="DeviceErrorQueue"/>
    /// and the error messages are accumulated in <see cref="DeviceErrorBuilder"/> and in the <see cref="SessionBase.DeviceErrorReport"/>.
    ///
    /// <para>
    /// Use <see cref="QueryAndReportDeviceErrors"/> or <see cref="TryQueryAndReportDeviceErrors"/>
    /// to create the <see cref="SessionBase.DeviceErrorReport"/> and standardEventRegisterDetails
    /// the errors.</para>
    /// </para>
    /// </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <param name="indent">       (Optional) The indent. </param>
    /// <returns>   The device errors error. </returns>
    protected virtual string QueryDeviceErrorsThis( ServiceRequests statusByte, string indent = "\t" )
    {
        if ( !this.IsErrorBitSet( statusByte ) ) return string.Empty;

        this.ClearErrorCache();
        int errorCount = 0;

        System.Text.StringBuilder preBuilder = new();

        if ( !string.IsNullOrWhiteSpace( this.LastAction ) )
            _ = preBuilder.AppendLine( $"{indent}Action: {this.LastAction.InsertCommonEscapeSequences()}" );

        if ( !string.IsNullOrWhiteSpace( this.LastMessageSent ) )
            _ = preBuilder.AppendLine( $"{indent}=> '{this.LastMessageSent.InsertCommonEscapeSequences()}'" );

        if ( !string.IsNullOrWhiteSpace( this.LastMessageReceived ) )
            _ = preBuilder.AppendLine( $"{indent}<= '{this.LastMessageReceived.InsertCommonEscapeSequences()}'" );

        if ( this.IsMeasurementEventBitSet( statusByte ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning(
                $"{this.ResourceNameCaption} status byte {( int ) statusByte:X2} has both the message and error bits set." );
            statusByte = this.DiscardUnreadData( this.StatusReadDelay );
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning(
                $"{this.ResourceNameCaption} the following messages '{this.DiscardedData}' were discarded before reading the device errors." );

            // Developer: This is a bug and run condition;
            // On reading the status register, the Session Base class is expected to turn off
            // the error available flag if an message is present in the presence of an error.
            // Please check if a run condition had occurred causing the message to appear after the status
            // register was read as indicating that no such message existed.
            // 2024/08/29: Reading the message at this point caused Query Unterminated error 420.
            //   A fix is on the todo list. For now, we assume that the message available flag is bogus.
            // this.LastUnreadMessage = this.ReadLineTrimEnd();
            // _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{this.ResourceNameCaption} message {this.LastUnreadMessage} was added to the cache of orphan messages before reading device errors in order to prevent a QueryEnum Unterminated error" );
        }

        System.Text.StringBuilder builder = new();

        // There are currently two queue reading commands and two single error reading commands.
        string queryCommand = !string.IsNullOrWhiteSpace( this.NextDeviceErrorQueryCommand )
            ? this.NextDeviceErrorQueryCommand
            : !string.IsNullOrWhiteSpace( this.DequeueErrorQueryCommand )
            ? this.DequeueErrorQueryCommand
            : string.Empty;

        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
        {

            if ( !string.IsNullOrWhiteSpace( queryCommand ) && this.IsErrorBitSet( statusByte ) )
            {
                DeviceError de;
                do
                {
                    de = this.EnqueueDeviceError( this.QueryTrimEnd( queryCommand ) );

                    if ( de.IsError )
                    {
                        errorCount += 1;
                        _ = builder.AppendLine( $"{indent}{de.CompoundErrorMessage}" );

                        _ = SessionBase.AsyncDelay( this.StatusReadDelay );
                        statusByte = this.ReadStatusByte();
                    }
                }
                while ( this.IsErrorBitSet( statusByte ) && de.IsError );

                // this is a kludge because the 7510 does not clear the error queue.
                if ( !de.IsError && this.IsErrorBitSet( statusByte ) )
                    _ = this.WriteLine( this.ClearErrorQueueCommand );
            }

        }

        else if ( !string.IsNullOrWhiteSpace( this.DeviceErrorQueryCommand ) )
        {
            // Query last error:
            DeviceError de = this.EnqueueDeviceError( this.QueryTrimEnd( this.DeviceErrorQueryCommand ) );
            if ( de.IsError )
            {
                errorCount += 1;
                _ = builder.AppendLine( $"{indent}{de.CompoundErrorMessage}" );
            }
        }
        else if ( !string.IsNullOrWhiteSpace( this.LastSystemErrorQueryCommand ) )
        {
            // Query last system error:
            _ = this.WriteLine( this.LastSystemErrorQueryCommand );
            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );
            Pith.SessionBase.DoEventsAction?.Invoke();

            DeviceError de = this.EnqueueDeviceError( this.ReadLine() );
            if ( de.IsError )
            {
                errorCount += 1;
                _ = builder.AppendLine( $"{indent}{de.CompoundErrorMessage}" );
            }
        }


        if ( builder.Length > 0 )
        {
            _ = this.QueryStandardEventStatus();

            _ = preBuilder.AppendLine( $"{indent}ESR byte: {this.BuildStandardEventValueDescription()}" );

            _ = this.DeviceErrorBuilder.Append( builder.ToString().TrimEndNewLine() );

            DeviceError lastError = this.DeviceErrorQueue.LastError;
            this.LastErrorCompoundErrorMessage = lastError.CompoundErrorMessage;
            this.HasDeviceError = lastError.IsError;
            this.DeviceErrorReport = this.DeviceErrorBuilder.ToString();
            this.DeviceErrorReportCount = errorCount;
            this.DeviceErrorPreamble = preBuilder.ToString().TrimEndNewLine();
        }

        return this.DeviceErrorReport;
    }

    /// <summary>   Queries and reports device errors. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    public void QueryAndReportDeviceErrors( ServiceRequests statusByte )
    {
        if ( this.IsErrorBitSet( statusByte ) )
        {
            _ = this.QueryDeviceErrors( statusByte );
            if ( this.HasDeviceError )
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning(
                    $"{this.ResourceNameCaption} last device error:\n{this.DeviceErrorPreamble}\n\t{this.LastErrorCompoundErrorMessage}\n{new StackFrame( true ).UserCallStack()}" );
            if ( this.HasErrorReport && this.DeviceErrorReportCount > 1 )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning(
                    $"{this.ResourceNameCaption} device errors:\n{this.DeviceErrorPreamble}\n{this.DeviceErrorReport}\n{new StackFrame( true ).UserCallStack()}" );
            }
        }
    }

    /// <summary>   Queries any existing device errors. </summary>
    /// <remarks>
    /// 2024-09-02. <para>
    /// The errors are stored in <see cref="DeviceErrorQueue"/>
    /// and the error messages are accumulated in <see cref="DeviceErrorBuilder"/> and in the <see cref="SessionBase.DeviceErrorReport"/>.
    /// <para>
    /// Use <see cref="QueryAndReportDeviceErrors"/> or <see cref="TryQueryAndReportDeviceErrors"/>
    /// to create the <see cref="SessionBase.DeviceErrorReport"/> and standardEventRegisterDetails the errors.</para>
    /// </para>
    /// </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <returns>   The device errors error. </returns>
    public virtual string QueryDeviceErrors( ServiceRequests statusByte )
    {
        if ( !this.IsErrorBitSet( statusByte ) ) return string.Empty;
        if ( this.ReadingDeviceErrors )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogDebug(
                $"{this.ResourceNameCaption} attempt to enter {nameof( SessionBase.QueryDeviceErrors )} while reading previous device error(s) was denied." );
            return string.Empty;
        }
        try
        {
            this.ReadingDeviceErrors = true;
            return this.QueryDeviceErrorsThis( statusByte );
        }
        catch
        {
            throw;
        }
        finally
        {
            this.ReadingDeviceErrors = false;
        }
    }

    /// <summary>   Queries and reports device errors. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <param name="statusByte">   The status byte. </param>
    /// <returns>   (Success, Details) </returns>
    public (bool Success, string Details) TryQueryAndReportDeviceErrors( ServiceRequests statusByte )
    {
        (bool Success, string Details) result = (true, string.Empty);
        if ( this.IsErrorBitSet( statusByte ) )
        {
            string activity = "reading and reporting device errors";
            try
            {
                this.QueryAndReportDeviceErrors( statusByte );
            }
            catch ( Exception ex )
            {
                result = (false, cc.isr.VI.SessionLogger.Instance.LogException( ex, activity ));
            }
            finally
            {
            }
        }
        return result;
    }

    #endregion
}
