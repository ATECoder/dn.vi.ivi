using System;
using System.Collections.Generic;
using System.Diagnostics;
using cc.isr.Std;
using cc.isr.Enums;
using cc.isr.VI.ExceptionExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp2
{
    /// <summary> Defines a local node subsystem. </summary>
    /// <remarks>
    /// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
    /// Licensed under The MIT License. </para><para>
    /// David, 2016-11-01. Based on legacy status subsystem. </para>
    /// </remarks>
    public abstract class LocalNodeSubsystemBase : SubsystemBase
    {
        #region " construction and cleanup "

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSubsystemBase" /> class.
        /// </summary>
        /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
        /// Subsystem</see>. </param>
        protected LocalNodeSubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
        {
            this._initializeTimeout = TimeSpan.FromMilliseconds( 30000d );
            this.ShowEventsStack = new Stack<Syntax.Tsp.EventLogModes?>();
            this.ShowPromptsStack = new Stack<PromptsState>();
            if ( statusSubsystem is not null )
                statusSubsystem.Session.PropertyChanged += this.SessionPropertyChanged;
        }

        #endregion

        #region " i presettable "

        /// <summary> The initialize timeout. </summary>
        private TimeSpan _initializeTimeout;

        /// <summary> Gets or sets the time out for doing a reset and clear on the instrument. </summary>
        /// <value> The connect timeout. </value>
        public TimeSpan InitializeTimeout
        {
            get => this._initializeTimeout;
            set
            {
                if ( !value.Equals( this.InitializeTimeout ) )
                {
                    this._initializeTimeout = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> Clears the active state. Issues selective device clear. </summary>
        public void ClearActiveState()
        {
            this.ExecutionState = TspExecutionState.IdleReady;
        }

        /// <summary> Sets values to their known clear execution state. </summary>
        public override void DefineClearExecutionState()
        {
            base.DefineClearExecutionState();
            _ = this.ReadExecutionState();
            // Set all cached values that get reset by CLS
            this.ClearStatus();
            _ = this.Session.QueryOperationCompleted();
        }

        /// <summary> Sets the known initial post reset state. </summary>
        public override void InitKnownState()
        {
            base.InitKnownState();
            try
            {
                this.Session.StoreCommunicationTimeout( this.InitializeTimeout );
                // turn prompts off. This may not be necessary.
                this.WriteAutoInstrumentMessages( PromptsState.Disable, Syntax.Tsp.EventLogModes.None );
            }
            catch ( Exception ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Exception {ex.Message} ignored turning off prompts" );
            }
            finally
            {
                this.Session.RestoreCommunicationTimeout();
            }

            try
            {
                // flush the input buffer in case the instrument has some leftovers.
                _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
                if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Data discarded after turning prompts and errors off;. Data: {this.Session.DiscardedData }." );
                }
            }
            catch ( Pith.NativeException ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, "Exception ignored clearing read buffer" );
            }

            try
            {
                // flush write may cause the instrument to send off a new data.
                _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
                if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Unread data discarded after discarding unset data;. Data: {this.Session.DiscardedData}." );
                }
            }
            catch ( Pith.NativeException ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, "Exception ignored clearing read buffer" );
            }
        }

        /// <summary>
        /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
        /// default values.
        /// </summary>
        public override void DefineKnownResetState()
        {
            // clear elements.
            this.ClearStatus();
            base.DefineKnownResetState();

            // enable processing of execution state.
            this.ProcessExecutionStateEnabled = true;

            // read the prompts status
            _ = this.QueryPromptsState();

            // read the errors status
            _ = this.QueryShowEvents();
            _ = this.Session.QueryOperationCompleted();
            this.ExecutionState = new TspExecutionState?();
        }

        #endregion

        #region " session "

        /// <summary> Handles the Session property changed event. </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Property Changed event information. </param>
        private void SessionPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            if ( sender is object && e is object && !string.IsNullOrWhiteSpace( e.PropertyName ) )
            {
                this.OnSessionPropertyChanged( e );
            }
        }

        /// <summary> Handles the property changed event. </summary>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        protected void OnSessionPropertyChanged( System.ComponentModel.PropertyChangedEventArgs e )
        {
            if ( this.ProcessExecutionStateEnabled && e is object && !string.IsNullOrWhiteSpace( e.PropertyName ) )
            {
                switch ( e.PropertyName ?? "" )
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
                }
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

        /// <summary> True to enable, false to disable the process execution state. </summary>
        private bool _processExecutionStateEnabled;

        /// <summary> Gets or sets the process execution state enabled. </summary>
        /// <value> The process execution state enabled. </value>
        public bool ProcessExecutionStateEnabled
        {
            get => this._processExecutionStateEnabled;
            set
            {
                if ( !value.Equals( this._processExecutionStateEnabled ) )
                {
                    this._processExecutionStateEnabled = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> State of the execution. </summary>
        private TspExecutionState? _executionState;

        /// <summary>
        /// Gets or sets the last TSP execution state. Setting the last state is useful when closing the
        /// Tsp System.
        /// </summary>
        /// <value> The last state. </value>
        public TspExecutionState? ExecutionState
        {
            get => this._executionState;
            set
            {
                if ( value.HasValue && !this.ExecutionState.HasValue || !value.HasValue && this.ExecutionState.HasValue || !value.Equals( this.ExecutionState ) )
                {
                    this._executionState = value;
                    this.NotifyPropertyChanged();
                }
            }
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
                value = value.Substring( 0, 4 );
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
            if ( this.PromptsState != PromptsState.None )
            {
                // if prompts are on,
                if ( this.PromptsState == PromptsState.Enable )
                {
                    // do a read. This raises an event that parses the state
                    if ( this.Session.AwaitErrorOrMessageAvailableBits( TimeSpan.FromMilliseconds( 1d ), 3 ) )
                    {
                        _ = this.Session.ReadLine();
                    }
                }
                else
                {
                    this.ExecutionState = TspExecutionState.Unknown;
                }
            }

            // check if we have data in the output buffer.
            else if ( this.Session.AwaitErrorOrMessageAvailableBits( TimeSpan.FromMilliseconds( 1d ), 3 ) )
            {
                // if data exists in the buffer, it may indicate that the prompts are already on
                // so just go read the output buffer. Once read, the status will be parsed.
                _ = this.Session.ReadLine();
            }
            else
            {
                // if we have no value then we must first read the prompt status
                // once read, the status will be parsed.
                _ = this.QueryPromptsState();
            }

            return this.ExecutionState;
        }

        #endregion

        #region " shows events mode "

        /// <summary> The show events. </summary>
        private Syntax.Tsp.EventLogModes? _showEvents;

        /// <summary> Gets or sets the cached Shows Events Mode. </summary>
        /// <value> The show events. </value>
        public Syntax.Tsp.EventLogModes? ShowEvents
        {
            get => this._showEvents;
            protected set
            {
                if ( !Nullable.Equals( this.ShowEvents, value ) )
                {
                    this._showEvents = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> Writes and reads back the Shows Events Mode . </summary>
        /// <param name="value"> the Shows Events Mode . </param>
        /// <returns> A List of scans. </returns>
        public Syntax.Tsp.EventLogModes? ApplyShowEvents( Syntax.Tsp.EventLogModes? value )
        {
            _ = this.WriteShowEvents( value );
            return this.QueryShowEvents();
        }

        /// <summary> Gets or sets the Shows Events Mode query command. </summary>
        /// <value> the Shows Events Mode query command. </value>
        protected virtual string ShowEventsQueryCommand { get; set; } = "_G.print(_G.localnode.showevents)";

        /// <summary> Queries the Shows Events Modes. </summary>
        /// <returns>
        /// The <see cref="VI.Syntax.Tsp.EventLogModes">Shows Events Modes</see> or none if
        /// unknown.
        /// </returns>
        public Syntax.Tsp.EventLogModes? QueryShowEvents()
        {
            string mode = this.ShowEvents.ToString();
            this.Session.MakeEmulatedReplyIfEmpty( mode );
            mode = this.Session.QueryTrimEnd( this.ShowEventsQueryCommand );
            if ( string.IsNullOrWhiteSpace( mode ) )
            {
                string message = "Failed fetching Shows Events Modes";
                Debug.Assert( !Debugger.IsAttached, message );
                this.ShowEvents = new Syntax.Tsp.EventLogModes?();
            }
            else
            {
                this.ShowEvents = int.TryParse( mode, out int eventMode ) ? ( Syntax.Tsp.EventLogModes ) ( int ) (eventMode) : new Syntax.Tsp.EventLogModes?();
            }

            return this.ShowEvents;
        }

        /// <summary> Gets or sets the Shows Events Mode command format. </summary>
        /// <remarks> SCPI Base Command: ":FORM:ELEM {0}". </remarks>
        /// <value> the Shows Events Mode command format. </value>
        protected virtual string ShowEventsCommandFormat { get; set; } = "_G.localnode.showevents={0}";

        /// <summary>
        /// Writes the Shows Events Modes without reading back the value from the device.
        /// </summary>
        /// <param name="value"> the Shows Events Mode. </param>
        /// <returns>
        /// The <see cref="VI.Syntax.Tsp.EventLogModes">Shows Events Modes</see> or none if
        /// unknown.
        /// </returns>
        public Syntax.Tsp.EventLogModes? WriteShowEvents( Syntax.Tsp.EventLogModes? value )
        {
            _ = this.Session.WriteLine( this.ShowEventsCommandFormat, ( object ) ( int ) value );
            this.ShowEvents = value;
            return this.ShowEvents;
        }

        #endregion

        #region " prompts state "

        /// <summary> State of the prompts. </summary>
        private PromptsState _promptsState;

        /// <summary> Gets or sets the prompts state sentinel. </summary>
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
        /// <value> The prompts state. </value>
        public PromptsState PromptsState
        {
            get => this._promptsState;
            protected set
            {
                if ( !Equals( value, this.PromptsState ) )
                {
                    this._promptsState = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        /// <summary> Writes and reads back the prompts state. </summary>
        /// <param name="value"> true to value. </param>
        /// <returns> A PromptsState. </returns>
        public PromptsState ApplyPromptsState( PromptsState value )
        {
            _ = this.WritePromptsState( value );
            return this.QueryPromptsState();
        }

        /// <summary> Gets or sets the prompts state query command. </summary>
        /// <value> The prompts state query command. </value>
        protected virtual string PromptsStateQueryCommand { get; set; } = "_G.print(_G.localnode.prompts)";

        /// <summary> Queries the condition for showing prompts. Controls prompting. </summary>
        /// <returns> The prompts state. </returns>
        public PromptsState QueryPromptsState()
        {
            string mode = this.PromptsState.ToString();
            this.Session.MakeEmulatedReplyIfEmpty( mode );
            mode = this.Session.QueryTrimEnd( this.PromptsStateQueryCommand );
            if ( string.IsNullOrWhiteSpace( mode ) )
            {
                string message = "Failed fetching prompts state";
                Debug.Assert( !Debugger.IsAttached, message );
                this.PromptsState = PromptsState.None;
            }
            else
            {
                this.PromptsState = SessionBase.ParseContained<PromptsState>( mode.BuildDelimitedValue() );
            }

            if ( !this.ProcessExecutionStateEnabled )
            {
                // read execution state explicitly, because session events are disabled.
                _ = this.ReadExecutionState();
            }

            return this.PromptsState;
        }

        /// <summary> The prompts state command format. </summary>
        /// <value> The prompts state command format. </value>
        protected virtual string PromptsStateCommandFormat { get; set; } = "_G.localnode.prompts={0}";

        /// <summary> Sets the condition for showing prompts. Controls prompting. </summary>
        /// <param name="value"> true to value. </param>
        /// <returns> <c>true</c> to prompts state; otherwise <c>false</c>. </returns>
        public PromptsState WritePromptsState( PromptsState value )
        {
            this.Session.LastAction = cc.isr.VI.SessionLogger.Instance.LogInformation( $"{value} prompts;. " );
            this.Session.LastNodeNumber = new int?();
            if ( value != PromptsState.None )
            {
                _ = this.Session.WriteLine( this.PromptsStateCommandFormat, value.ExtractBetween() );
            }

            this.PromptsState = value;
            if ( !this.ProcessExecutionStateEnabled )
            {
                // read execution state explicitly, because session events are disabled.
                _ = this.ReadExecutionState();
            }

            return this.PromptsState;
        }

        /// <summary>
        /// Sets the instrument to automatically send or stop sending instrument prompts and events.
        /// </summary>
        /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
        /// <param name="prompts"> The prompts. </param>
        /// <param name="events">  The events. </param>
        public void WriteAutoInstrumentMessages( PromptsState prompts, Syntax.Tsp.EventLogModes? events )
        {
            // flush the input buffer in case the instrument has some leftovers.
            _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
            string showPromptsCommand = "<failed to issue>";
            try
            {
                // sets prompt transmissions
                _ = this.WritePromptsState( prompts );
                showPromptsCommand = this.Session.LastMessageSent;
            }
            catch
            {
            }

            // flush again in case turning off prompts added stuff to the buffer.
            _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
            if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Unread data discarded after turning prompts off;. Data: {this.Session.DiscardedData}." );
            }

            string showErrorsCommand = "<failed to issue>";
            try
            {
                // turn off event log transmissions
                _ = this.WriteShowEvents( events );
                showErrorsCommand = this.Session.LastMessageSent;
            }
            catch
            {
            }

            // flush again in case turning off errors added stuff to the buffer.
            _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
            if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Unread data discarded after turning errors off;. Data: {this.Session.DiscardedData}." );
            }

            // now validate
            _ = this.QueryShowEvents();
            if ( !this.ShowEvents.HasValue )
            {
                throw new OperationFailedException( this.ResourceNameCaption, showErrorsCommand, "turning off automatic event display failed; value not set." );
            }
            else if ( ( int? ) this.ShowEvents.Value != ( int? ) events == true )
            {
                throw new OperationFailedException( this.ResourceNameCaption, showPromptsCommand, $"turning off test script prompts failed; showing {this.ShowEvents} expected {events}." );
            }
        }

        #endregion

        #region " status "

        /// <summary> Gets or sets the stack for storing the show events states. </summary>
        /// <value> A stack of show events. </value>
        private Stack<Syntax.Tsp.EventLogModes?> ShowEventsStack { get; set; }

        /// <summary> Gets or sets the stack for storing the prompts state states. </summary>
        /// <value> A stack of show prompts. </value>
        private Stack<PromptsState> ShowPromptsStack { get; set; }

        /// <summary> Clears the status. </summary>
        public void ClearStatus()
        {
            // clear the stacks
            this.ShowEventsStack.Clear();
            this.ShowPromptsStack.Clear();
            this.ExecutionState = this.Session.IsDeviceOpen ? TspExecutionState.IdleReady : TspExecutionState.Closed;
        }

        /// <summary> Restores the status of errors and prompts. </summary>
        public void RestoreStatus()
        {
            if ( this.ShowEventsStack.Count > 0 )
            {
                Syntax.Tsp.EventLogModes? lastShowEvent = this.ShowEventsStack.Pop();
                if ( lastShowEvent.HasValue )
                {
                    _ = this.WriteShowEvents( lastShowEvent.Value );
                }
            }

            if ( this.ShowPromptsStack.Count > 0 )
            {
                _ = this.WritePromptsState( this.ShowPromptsStack.Pop() );
            }
        }

        /// <summary> Saves the current status of errors and prompts. </summary>
        public void StoreStatus()
        {
            this.ShowEventsStack.Push( this.QueryShowEvents() );
            this.ShowPromptsStack.Push( this.QueryPromptsState() );
        }

        #endregion

    }

    /// <summary> Values that represent on off states. </summary>
    public enum OnOffState
    {
        /// <summary> An enum constant representing the off] option. </summary>
        [System.ComponentModel.Description( "Off (smu.OFF)" )]
        Off = 0,

        /// <summary> An enum constant representing the on] option. </summary>
        [System.ComponentModel.Description( "On (smu.ON)" )]
        On = 1
    }

    /// <summary> Values that represent prompts states. </summary>
    public enum PromptsState
    {
        /// <summary> An enum constant representing the none option. </summary>
        [System.ComponentModel.Description( "Not defined" )]
        None,

        /// <summary> An enum constant representing the disable option. </summary>
        [System.ComponentModel.Description( "Disable (localnode.DISABLE)" )]
        Disable = 1,

        /// <summary> An enum constant representing the enable option. </summary>
        [System.ComponentModel.Description( "Enable (localnode.ENABLE)" )]
        Enable = 2
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
        /// handle as desired. Use “errorqueue” commands to read and clear errors. </summary>
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
}
