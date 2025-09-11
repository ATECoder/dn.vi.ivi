using cc.isr.Enums;
using cc.isr.Std.NullableExtensions;
using cc.isr.VI.ExceptionExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp;

/// <summary> Defines the local node for a TSP System. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-11-01. Based on legacy status subsystem. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SystemSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
/// Subsystem</see>. </param>
public abstract class LocalNodeSubsystemBase( Tsp.StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary> The initialize timeout. </summary>
    private TimeSpan _initializeTimeout = TimeSpan.FromMilliseconds( 30000d );

    /// <summary> Gets or sets the time out for doing a reset and clear on the instrument. </summary>
    /// <value> The connect timeout. </value>
    public TimeSpan InitializeTimeout
    {
        get => this._initializeTimeout;
        set => _ = this.SetProperty( ref this._initializeTimeout, value );
    }

    /// <summary> Defines the active state of the local node. </summary>
    public void ClearActiveState()
    {
        this.ExecutionState = TspExecutionState.IdleReady;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the execution state on clear should be read.
    /// </summary>
    /// <value> True if read execution state on clear, false if not. </value>
    public bool ReadExecutionStateOnClear
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = false;

    /// <summary>
    /// Defines the clear execution state (CLS) by setting system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    public override void DefineClearExecutionState()
    {
        if ( this.ProcessExecutionStateEnabled && this.ReadExecutionStateOnClear )
            _ = this.ReadExecutionState();

        // Set all cached values that get reset by CLS
        this.ClearStatus();
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Customizes the reset state. </remarks>
    public override void InitKnownState()
    {
        base.InitKnownState();

        // the instrument defaults to error and prompts off.
        this.ShowErrors = false;
        this.ShowErrors = false;
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();

        // clear elements.
        this.ClearStatus();

        // enable processing of execution state.
        this.ProcessExecutionStateEnabled = true;

        if ( this.ShowsPromptsOnResetKnownState )
        {
            try
            {
                this.Session.StoreCommunicationTimeout( this.InitializeTimeout );
                // turn prompts off. This may not be necessary.
                this.Session.SetLastAction( "turning prompts off" );
                this.TurnPromptsOff();
            }
            catch ( Exception ex )
            {
                _ = ex.AddExceptionData();
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Exception ignored turning off prompts output;. {ex.BuildMessage()}" );
            }
            finally
            {
                this.Session.RestoreCommunicationTimeout();
            }
        }
        else
        {
            // read the prompts status
            this.Session.SetLastAction( "querying prompts state" );
            _ = this.QueryShowPrompts();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose(
                $"{this.Session.ResourceNameCaption} prompts output is {this.ShowPrompts.ToDataString( "on", "off", "na" )}; execution state is {this.ExecutionState}." );
        }

        if ( this.ShowsErrorsOnResetKnownState )
        {
            try
            {
                this.Session.StoreCommunicationTimeout( this.InitializeTimeout );
                // turn errors off. This may not be necessary.
                this.Session.SetLastAction( "turning errors off" );
                this.TurnErrorsOff();
            }
            catch ( Exception ex )
            {
                _ = ex.AddExceptionData();
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Exception ignored turning off errors output;. {ex.BuildMessage()}" );
            }
            finally
            {
                this.Session.RestoreCommunicationTimeout();
            }
        }
        else
        {
            // read the errors status
            this.Session.SetLastAction( "querying errors state" );
            _ = this.QueryShowErrors();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose(
                $"{this.Session.ResourceNameCaption} errors output is {this.ShowErrors.ToDataString( "on", "off", "na" )}; execution state is {this.ExecutionState}." );
        }

        this.ExecutionState = new TspExecutionState?();
    }

    #endregion

    #region " session "

    /// <summary> Handles Session property change. </summary>
    /// <param name="sender">       Source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    protected override void HandlePropertyChanged( Pith.SessionBase sender, string? propertyName )
    {
        base.HandlePropertyChanged( sender, propertyName );
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName )
        {
            case nameof( Pith.SessionBase.LastMessageReceived ):
                {
                    // parse the command to get the TSP execution state.
                    _ = this.ParseExecutionState( this.Session.LastMessageReceived, TspExecutionState.IdleReady );
                    break;
                }

            case nameof( Pith.SessionBase.LastMessageSent ):
                {
                    // set the TSP status
                    this.ExecutionState = TspExecutionState.Processing;
                    break;
                }

            case nameof( Pith.SessionBase.ResourceNameCaption ):
                {
                    this.NotifyPropertyChanged( nameof( this.ResourceNameCaption ) );
                    break;
                }

            case nameof( Pith.SessionBase.ResourceModelCaption ):
                {
                    this.NotifyPropertyChanged( nameof( this.ResourceModelCaption ) );
                    break;
                }

            default:
                break;
        }
    }

    #endregion

    #region " asset trigger "

    /// <summary> Issues a hardware trigger. </summary>
    public void AssertTrigger()
    {
        this.ExecutionState = TspExecutionState.IdleReady;
        this.Session.AssertTrigger();
    }

    #endregion

    #region " execution state "

    /// <summary> Gets or sets the process execution state enabled. </summary>
    /// <remarks> Processing execution state is a bit counter intuitive.
    ///           When enabled execution state is not read when checking prompts or errors. </remarks>
    /// <value> The process execution state enabled. </value>
    public bool ProcessExecutionStateEnabled
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>
    /// Gets or sets the last TSP execution state. Setting the last state is useful when closing the
    /// Tsp System.
    /// </summary>
    /// <value> The last state. </value>
    public TspExecutionState? ExecutionState
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets the instrument Execution State caption. </summary>
    /// <value> The state caption. </value>
    public string ExecutionStateCaption => this.ExecutionState.HasValue ? this.ExecutionState.Value.Description() : "N/A";

    /// <summary> Parses the state of the TSP prompt and saves it in the state cache value. </summary>
    /// <param name="value">        Specifies the read buffer. </param>
    /// <param name="defaultValue"> The default value. </param>
    /// <returns> The instrument Execution State. </returns>
    public TspExecutionState ParseExecutionState( string value, TspExecutionState defaultValue )
    {
        TspExecutionState state = defaultValue;
        if ( string.IsNullOrWhiteSpace( value ) || value.Length < 4 )
        {
        }
        else
        {
            value = value[..4];
            if ( value.StartsWith( Syntax.Tsp.Constants.ReadyPrompt, true, System.Globalization.CultureInfo.CurrentCulture ) )
            {
                state = TspExecutionState.IdleReady;
            }
            else if ( value.StartsWith( Syntax.Tsp.Constants.ContinuationPrompt, true, System.Globalization.CultureInfo.CurrentCulture ) )
            {
                state = TspExecutionState.IdleContinuation;
            }
            else if ( value.StartsWith( Syntax.Tsp.Constants.ErrorPrompt, true, System.Globalization.CultureInfo.CurrentCulture ) )
            {
                state = TspExecutionState.IdleError;
            }
            else
            {
                // no prompt -- set to the default state
                state = defaultValue;
            }
        }

        this.ExecutionState = state;
        return state;
    }

    /// <summary> Reads the state of the TSP prompt and saves it in the state cache value. </summary>
    /// <returns> The instrument Execution State. </returns>
    public TspExecutionState? ReadExecutionState()
    {
        // check status of the prompt flag.
        if ( this.ShowPrompts.HasValue )
        {
            // if prompts are on,
            if ( this.ShowPrompts.Value )
            {
                this.Session.SetLastAction( $"calling {nameof( SessionBase.AwaitErrorOrMessageAvailableBits )} #1" );
                // do a read. This raises an event that parses the state
                ServiceRequests statusByte = this.Session.AwaitErrorOrMessageAvailableBits( TimeSpan.FromMilliseconds( 1d ), 3 );
                this.Session.ThrowDeviceExceptionIfError( statusByte );
                if ( this.Session.IsMessageAvailableBitSet( statusByte ) )
                    _ = this.Session.ReadLine();
            }
            else
                this.ExecutionState = TspExecutionState.Unknown;
        }

        else
        {
            this.Session.SetLastAction( $"calling {nameof( SessionBase.AwaitErrorOrMessageAvailableBits )} #2" );

            // check if we have data in the output buffer.
            ServiceRequests statusByte = this.Session.AwaitErrorOrMessageAvailableBits( TimeSpan.FromMilliseconds( 1d ), 3 );

            this.Session.ThrowDeviceExceptionIfError( statusByte );

            if ( this.Session.IsMessageAvailableBitSet( statusByte ) )

                // if data exists in the buffer, it may indicate that the prompts are already on
                // so just go read the output buffer. Once read, the status will be parsed.
                _ = this.Session.ReadLine();

            else
            {
                this.Session.SetLastAction( $"calling {nameof( LocalNodeSubsystemBase.QueryShowPrompts )}" );

                // if we have no value then we must first read the prompt status
                // once read, the status will be parsed.
                _ = this.QueryShowPrompts();
            }
        }

        return this.ExecutionState;
    }

    #endregion

    #region " show errors "

    /// <summary>
    /// Gets or sets a value indicating whether the instrument turns on the automatic display of errors upon restart or on reset to known state.
    /// </summary>
    /// <value> True if the instrument turns on the automatic display of errors upon restart or on reset to known state, false if not. </value>
    public bool ShowsErrorsOnResetKnownState
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = false;

    /// <summary> Gets or sets the Show Errors sentinel. </summary>
    /// <remarks>
    /// When true, the unit will automatically display the errors stored in the error queue, and then
    /// clear the queue. Errors will be processed at the end of executing a command message (just
    /// prior to issuing a prompt if prompts are enabled). When false, errors will not display.
    /// Errors will be left in the error queue and must be explicitly read or cleared. The error
    /// prompt (TSP?) is enabled.
    /// </remarks>
    /// <value> <c>true</c> to show errors; otherwise <c>false</c>. </value>
    public bool? ShowErrors
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>   Gets a value indicating whether the <see cref="ShowErrors"/> is enabled. </summary>
    /// <value> True if <see cref="ShowErrors"/> is enabled, false if not. </value>
    public bool ShowErrorsEnabled => this.ShowErrors.GetValueOrDefault( false );

    /// <summary> Writes and reads back the show errors sentinel. </summary>
    /// <param name="value"> <c>true</c> to show errors; otherwise, <c>false</c>. </param>
    /// <returns> <c>true</c> if on; otherwise <c>false</c>. </returns>
    public bool? ApplyShowErrors( bool value )
    {
        _ = this.WriteShowErrors( value );
        return this.QueryShowErrors();
    }

    /// <summary>
    /// Reads the condition for showing errors or leaving error messages in the instrument error
    /// queue.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   <c>true</c> to show errors; otherwise <c>false</c>. </returns>
    public bool? QueryShowErrors()
    {
        this.Session.SetLastAction( "querying show-errors state" );
        this.ShowErrors = this.Session.QueryPrint( false, VI.Syntax.Tsp.LocalNode.ShowErrors );
        if ( !this.ProcessExecutionStateEnabled )
            // read execution state explicitly, because session events are disabled.
            _ = this.ReadExecutionState();

        return this.ShowErrors;
    }

    /// <summary>
    /// Sets the condition for showing errors or leaving error messages in the instrument error queue.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="value">    true to value. </param>
    /// <returns>
    /// <c>true</c> to show errors; otherwise <c>false</c> to leave error messages in the instrument
    /// error queue.
    /// </returns>
    public bool? WriteShowErrors( bool value )
    {
        this.Session.SetLastAction( $"{value.GetHashCode():showing;showing;hiding} errors" );
        this.Session.LastNodeNumber = default;
        _ = this.Session.WriteLine( VI.Syntax.Tsp.LocalNode.ShowErrorsSetterCommand, value.GetHashCode() );
        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay );
        this.ShowErrors = value;
        if ( !this.ProcessExecutionStateEnabled )
        {
            // read execution state explicitly, because session events are disabled.
            _ = this.ReadExecutionState();
        }

        return this.ShowErrors;
    }

    /// <summary>
    /// Turns off automatic error outputs leaving error messages in the instrument error queue. It
    /// seems that the new systems come with prompts and errors off when the instrument is started or
    /// reset so this may not be needed.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="DeviceException">  Thrown when a Device error condition occurs. </exception>
    public void TurnErrorsOff()
    {
        ServiceRequests statusByte = this.Session.ReadStatusByte();

        // flush any pending messages.
        this.Session.SetLastAction( $"{nameof( SessionBase.DiscardUnreadData )} #1. Status byte: 0x{statusByte:X2}" );
        _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
        if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
            _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Unread data discarded after turning prompts off;. Data: {this.Session.DiscardedData}." );

        string showErrorsCommand = "<failed to issue>";
        try
        {
            // turn off error transmissions
            _ = this.WriteShowErrors( false );
            showErrorsCommand = this.Session.LastMessageSent;
        }
        catch
        {
        }

        statusByte = this.Session.ReadStatusByte();

        // flush any pending messages again in case turning off errors added stuff to the buffer.
        this.Session.SetLastAction( $"{nameof( SessionBase.DiscardUnreadData )} #2. Status byte: 0x{statusByte:X2}" );
        _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
        if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Unread data discarded after turning errors off;. Data: {this.Session.DiscardedData}." );
        }

        // now validate
        if ( this.QueryShowErrors().GetValueOrDefault( true ) )
        {
            throw new cc.isr.VI.Pith.DeviceException( this.ResourceNameCaption ?? $"{nameof( this.ResourceNameCaption )} is null",
                $"failed turning off automatic error display using {showErrorsCommand}; error display is still on." );
        }
    }


    #endregion

    #region " show prompts "

    /// <summary>
    /// Gets or sets a value indicating whether the instrument turns on the automatic display of Prompts upon restart or on reset to known state.
    /// </summary>
    /// <value> True if the instrument turns on the automatic display of Prompts upon restart or on reset to known state, false if not. </value>
    public bool ShowsPromptsOnResetKnownState
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = false;

    /// <summary> Gets or sets the Show Prompts sentinel. </summary>
    /// <remarks>
    /// When true, prompts are issued after each command message is processed by the instrument.<para>
    /// When false prompts are not issued.</para><para>
    /// Command messages do not generate prompts. Rather, the TSP instrument generates prompts in
    /// response to command messages. When prompting is enabled, the instrument generates prompts in
    /// response to command messages. There are three prompts that might be returned:</para><para>
    /// “TSP&gt;” is the standard prompt. This prompt indicates that everything is normal and the
    /// command is done processing.</para><para>
    /// “TSP?” is issued if there are entries in the error queue when the prompt is issued. Like the
    /// “TSP&gt;” prompt, it indicates the command is done processing. It does not mean the previous
    /// command generated an error, only that there are still errors in the queue when the command
    /// was done processing.</para><para>
    /// “&gt;&gt;&gt;&gt;” is the continuation prompt. This prompt is used when downloading scripts
    /// or flash images. When downloading scripts or flash images, many command messages must be sent
    /// as a unit. The continuation prompt indicates that the instrument is expecting more messages
    /// as part of the current command.</para>
    /// </remarks>
    /// <value> The show prompts. </value>
    public bool? ShowPrompts
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>   Gets a value indicating whether the <see cref="ShowPrompts"/> is enabled. </summary>
    /// <value> True if <see cref="ShowPrompts"/> is enabled, false if not. </value>
    public bool ShowPromptsEnabled => this.ShowPrompts.GetValueOrDefault( false );


    /// <summary> Writes and reads back the show Prompts sentinel. </summary>
    /// <param name="value"> <c>true</c> to show Prompts; otherwise, <c>false</c>. </param>
    /// <returns> <c>true</c> if on; otherwise <c>false</c>. </returns>
    public bool? ApplyShowPrompts( bool value )
    {
        _ = this.WriteShowPrompts( value );
        return this.QueryShowPrompts();
    }

    /// <summary> Queries the condition for showing prompts. Controls prompting. </summary>
    /// <returns> <c>true</c> to show prompts; otherwise <c>false</c>. </returns>
    public bool? QueryShowPrompts()
    {
        this.Session.SetLastAction( "querying show-prompts state" );
        this.ShowPrompts = this.Session.QueryPrint( false, VI.Syntax.Tsp.LocalNode.ShowPrompts );
        if ( !this.ProcessExecutionStateEnabled )
        {
            // read execution state explicitly, because session events are disabled.
            _ = this.ReadExecutionState();
        }

        return this.ShowPrompts;
    }

    /// <summary> Sets the condition for showing prompts. Controls prompting. </summary>
    /// <param name="value"> true to value. </param>
    /// <returns> <c>true</c> to show prompts; otherwise <c>false</c>. </returns>
    public bool? WriteShowPrompts( bool value )
    {
        this.Session.SetLastAction( $"{value.GetHashCode():showing;showing;hiding} prompts" );
        this.Session.LastNodeNumber = default;
        _ = this.Session.WriteLine( VI.Syntax.Tsp.LocalNode.ShowPromptsSetterCommand, value.GetHashCode() );
        _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay );
        this.ShowPrompts = value;
        if ( !this.ProcessExecutionStateEnabled )
        {
            // read execution state explicitly, because session events are disabled.
            _ = this.ReadExecutionState();
        }

        return this.ShowPrompts;
    }

    /// <summary>
    /// Turns off automatic prompts. It seems that new systems come with prompts and errors off
    /// when the instrument is started or reset so this is not needed.
    /// </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    public void TurnPromptsOff()
    {
        ServiceRequests statusByte = this.Session.ReadStatusByte();

        // flush any pending messages.
        this.Session.SetLastAction( $"{nameof( SessionBase.DiscardUnreadData )} #1. Status byte: 0x{statusByte:X2}" );
        _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
        string showPromptsCommand = "<failed to issue>";
        try
        {
            // turn off prompt transmissions
            _ = this.WriteShowPrompts( false );
            showPromptsCommand = this.Session.LastMessageSent;
        }
        catch
        {
        }

        // flush any pending messages again in case turning off errors added stuff to the buffer.
        statusByte = this.Session.ReadStatusByte();
        _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );
        this.Session.SetLastAction( $"{nameof( SessionBase.DiscardUnreadData )} #2. Status byte: 0x{statusByte:X2}" );
        _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
        if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
            _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Unread data discarded after turning errors off;. Data: {this.Session.DiscardedData}." );

        // now validate
        if ( this.QueryShowPrompts().GetValueOrDefault( true ) )
            throw new cc.isr.VI.Pith.DeviceException( this.ResourceNameCaption ?? $"{nameof( this.ResourceNameCaption )} is null",
                $"Failed turning off automatic prompt display using '{showPromptsCommand}'; prompts display is still on." );
    }


    #endregion

    #region " status "

    /// <summary> Gets or sets the stack for storing the show errors states. </summary>
    /// <value> A stack of show errors. </value>
    private Stack<bool?> ShowErrorsStack { get; set; } = new Stack<bool?>();

    /// <summary> Gets or sets the stack for storing the show prompts states. </summary>
    /// <value> A stack of show prompts. </value>
    private Stack<bool?> ShowPromptsStack { get; set; } = new Stack<bool?>();

    /// <summary> Clears the status. </summary>
    public void ClearStatus()
    {
        // clear the stacks
        this.ShowErrorsStack.Clear();
        this.ShowPromptsStack.Clear();
        this.ExecutionState = this.Session.IsDeviceOpen ? TspExecutionState.IdleReady : TspExecutionState.Closed;
    }

    /// <summary> Restores the status of errors and prompts. </summary>
    public void RestoreStatus()
    {
        bool? lastValue = this.ShowErrorsStack.Pop();
        if ( lastValue.HasValue )
        {
            _ = this.WriteShowErrors( lastValue.Value );
        }

        lastValue = this.ShowPromptsStack.Pop();
        if ( lastValue.HasValue )
        {
            _ = this.WriteShowPrompts( lastValue.Value );
        }
    }

    /// <summary> Saves the current status of errors and prompts. </summary>
    public void StoreStatus()
    {
        this.ShowErrorsStack.Push( this.QueryShowErrors() );
        this.ShowPromptsStack.Push( this.QueryShowPrompts() );
    }

    #endregion
}
/// <summary> Enumerates the TSP Execution State. </summary>
public enum TspExecutionState
{
    /// <summary> Not defined. </summary>
    [System.ComponentModel.Description( "Not defined" )]
    None,

    /// <summary> Closed. </summary>
    [System.ComponentModel.Description( "Closed" )]
    Closed,

    /// <summary> Received the continuation prompt.
    /// Send between lines when loading a script indicating that
    /// TSP received script line successfully and is waiting for next line
    /// or the end script command. </summary>
    [System.ComponentModel.Description( "Continuation" )]
    IdleContinuation,

    /// <summary> Received the error prompt. Error occurred;
    /// handle as desired. Use “Error Queue” commands to read and clear errors. </summary>
    [System.ComponentModel.Description( "Error" )]
    IdleError,

    /// <summary> Received the ready prompt. For example, TSP received script successfully and is ready for next command. </summary>
    [System.ComponentModel.Description( "Ready" )]
    IdleReady,

    /// <summary> A command was sent to the instrument. </summary>
    [System.ComponentModel.Description( "Processing" )]
    Processing,

    /// <summary> Cannot tell because prompt are off. </summary>
    [System.ComponentModel.Description( "Unknown" )]
    Unknown
}
