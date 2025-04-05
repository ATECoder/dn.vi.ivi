using System.ComponentModel;
using cc.isr.Std.TimeSpanExtensions;
using cc.isr.Std.EscapeSequencesExtensions;
using cc.isr.VI.Pith.ExceptionExtensions;

namespace cc.isr.VI.Pith;

/// <summary> Base class for SessionBase. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class SessionBase : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDisposable
{
    #region " construction and cleanup "

    /// <summary> Specialized constructor for use only by derived class. </summary>
    protected SessionBase() : base()
    {
        this._operationCompleted = new bool?();
        this.Enabled = true;
        this._lastMessageReceived = string.Empty;
        this._lastMessageSent = string.Empty;
        this.UseDefaultTerminationThis();
        this.CommunicationTimeouts = new Stack<TimeSpan>();
        this.TimingSettings.PropertyChanged += this.HandleSettingsPropertyChanged;
        this.IOSettings.PropertyChanged += this.HandleSettingsPropertyChanged;
        this.ResourceSettings.PropertyChanged += this.HandleSettingsPropertyChanged;

        this.DeviceErrorBuilder = new System.Text.StringBuilder();
        this.NoErrorCompoundMessage = VI.Syntax.ScpiSyntax.NoErrorCompoundMessage;
        this.DeviceErrorQueue = new DeviceErrorQueue( VI.Syntax.ScpiSyntax.NoErrorCompoundMessage );
    }

    /// <summary> Validates the given visa session. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session"> The visa session. </param>
    /// <returns> A SessionBase. </returns>
    public static SessionBase Validated( SessionBase session )
    {
        return session is null ? throw new ArgumentNullException( nameof( session ) ) : session;
    }

    #region " disposable support "

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    /// resources.
    /// </summary>
    /// <remarks>
    /// Do not make this method Overridable (virtual) because a derived class should not be able to
    /// override this method.
    /// </remarks>
    public void Dispose()
    {
        this.Dispose( true );
        // Take this object off the finalization(Queue) and prevent finalization code
        // from executing a second time.
        GC.SuppressFinalize( this );
    }

    /// <summary> Gets or sets the disposed status. </summary>
    /// <value> The is disposed. </value>
    protected bool IsDisposed { get; private set; }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected virtual void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;

        try
        {
            if ( disposing )
            {
                this.MessageNotificationModes = MessageNotificationModes.None;
                this.LastNativeError = null;
                this.DiscardAllEvents();
                this.RemoveServiceRequestedEventHandlers();
                this.CommunicationTimeouts.Clear();
                this.StatusByteLocker?.Dispose();
            }
            // dispose of unmanaged code here; unmanaged code gets disposed if disposing or not.
            this.TimingSettings.PropertyChanged -= this.HandleSettingsPropertyChanged;
            this.IOSettings.PropertyChanged -= this.HandleSettingsPropertyChanged;
            this.ResourceSettings.PropertyChanged -= this.HandleSettingsPropertyChanged;
        }
        finally
        {
            this.IsDisposed = true;
        }
    }

    /// <summary> Finalizes this object. </summary>
    /// <remarks>
    /// Overrides should Dispose(disposing As Boolean) has code to free unmanaged resources.
    /// </remarks>
    ~SessionBase()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( false );
    }

    #endregion

    #endregion

    #region " Property change notification "

    /// <summary>   Notifies a property changed. </summary>
    /// <remarks>   David, 2021-02-01. </remarks>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected void NotifyPropertyChanged( [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
    {
        base.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
    }

    #endregion

    #region " syntax "

    /// <summary>   Applies the default syntax. </summary>
    /// <remarks>   2024-09-16. </remarks>
    /// <param name="commandLanguage">  The command language. </param>
    public void ApplyDefaultSyntax( cc.isr.VI.Syntax.CommandLanguage commandLanguage )
    {
        if ( cc.isr.VI.Syntax.CommandLanguage.Scpi == commandLanguage )
            this.ApplyIeee488Syntax();
        else if ( cc.isr.VI.Syntax.CommandLanguage.Tsp == commandLanguage )
            this.ApplyTspSyntax();
        this.CommandLanguage = commandLanguage;
    }

    private cc.isr.VI.Syntax.CommandLanguage _commandLanguage;

    /// <summary>   Gets or sets the command language. </summary>
    /// <value> The command language. </value>
    public cc.isr.VI.Syntax.CommandLanguage CommandLanguage
    {
        get => this._commandLanguage;
        private set => _ = this.SetProperty( ref this._commandLanguage, value );
    }


    /// <summary>   Applies the IEEE 488 syntax. </summary>
    /// <remarks>   2024-09-14. </remarks>
    private void ApplyIeee488Syntax()
    {
        this.DeviceClearDelayPeriod = TimeSpan.FromMilliseconds( 1d );

        this.ClearExecutionStateCommand = Syntax.Ieee488Syntax.ClearExecutionStateCommand;
        this.ClearExecutionStateQueryCompleteCommand = Syntax.Ieee488Syntax.ClearExecutionStateQueryCompleteCommand;
        this.ClearExecutionStateWaitCommand = Syntax.Ieee488Syntax.ClearExecutionStateWaitCommand;
        this.ClearExecutionStateOperationCompleteCommand = Syntax.Ieee488Syntax.ClearExecutionStateOperationCompleteCommand;
        this.CollectGarbageOperationCompleteCommand = string.Empty;
        this.CollectGarbageQueryCompleteCommand = Syntax.Tsp.Lua.CollectGarbageQueryCompleteCommand;
        this.CollectGarbageOperationCompleteCommandFormat = string.Empty;
        this.CollectGarbageQueryCompleteCommandFormat = string.Empty;
        this.IdentificationQueryCommand = Syntax.Ieee488Syntax.IdentificationQueryCommand;
        this.OperationCompleteCommand = Syntax.Ieee488Syntax.OperationCompleteCommand;
        this.OperationCompletedQueryCommand = Syntax.Ieee488Syntax.OperationCompletedQueryCommand;
        this.ResetKnownStateCommand = Syntax.Ieee488Syntax.ResetKnownStateCommand;
        this.ResetKnownStateWaitCommand = Syntax.Ieee488Syntax.ResetKnownStateWaitCommand;

        this.ServiceRequestEnableCommandFormat = Syntax.Ieee488Syntax.ServiceRequestEnableCommandFormat;
        this.ServiceRequestEnableQueryCommand = Syntax.Ieee488Syntax.ServiceRequestEnableQueryCommand;
        this.StatusByteQueryCommand = Syntax.Ieee488Syntax.StatusByteQueryCommand;

        this.StandardEventEnableCommandFormat = Syntax.Tsp.Status.StandardEventEnableCommandFormat;
        this.StandardEventEnableQueryCommand = Syntax.Ieee488Syntax.StandardEventEnableQueryCommand;
        this.StandardEventStatusQueryCommand = Syntax.Ieee488Syntax.StandardEventStatusQueryCommand;

        this.StandardServiceEnableCommandFormat = Syntax.Ieee488Syntax.StandardServiceEnableCommandFormat;
        this.StandardServiceEnableOperationCompleteCommandFormat = Syntax.Ieee488Syntax.StandardServiceEnableOperationCompleteCommandFormat;
        this.StandardServiceEnableQueryCompleteCommandFormat = Syntax.Ieee488Syntax.StandardServiceEnableQueryCompleteCommandFormat;

        this.WaitCommand = Syntax.Ieee488Syntax.WaitCommand;

        this.ClearErrorQueueCommand = VI.Syntax.ScpiSyntax.ClearSystemErrorQueueCommand;
        this.DeviceErrorQueryCommand = string.Empty;
        this.DequeueErrorQueryCommand = string.Empty; // VI.Syntax.Ieee488Syntax.LastSystemErrorQueryCommand
        this.ErrorQueueCountQueryCommand = string.Empty;
        this.LastSystemErrorQueryCommand = string.Empty;
        this.NextDeviceErrorQueryCommand = string.Empty;
        this.NoErrorCompoundMessage = VI.Syntax.ScpiSyntax.NoErrorCompoundMessage;
        this.DeviceErrorQueue = new DeviceErrorQueue( VI.Syntax.ScpiSyntax.NoErrorCompoundMessage );

        // IO PROPERTIES

        // The ASCII character used to end reading [10]
        this.ReadTerminationCharacter = Std.EscapeSequencesExtensions.EscapeSequencesExtensionMethods.NEW_LINE_VALUE;

        // The value indicating whether the read operation ends when a termination character is received [true]
        this.ReadTerminationCharacterEnabled = true;

        // STATUS REGISTER PROPERTIES

        this.ErrorAvailableBitmask = Pith.ServiceRequests.ErrorAvailable;
        this.MeasurementEventBitmask = Pith.ServiceRequests.MeasurementEvent;
        this.MessageAvailableBitmask = Pith.ServiceRequests.MessageAvailable;
        this.OperationEventSummaryBitmask = Pith.ServiceRequests.OperationEvent;
        this.QuestionableEventBitmask = Pith.ServiceRequests.QuestionableEvent;
        this.RequestingServiceBitmask = Pith.ServiceRequests.RequestingService;
        this.StandardEventSummaryBitmask = Pith.ServiceRequests.StandardEventSummary;
        this.SystemEventBitmask = Pith.ServiceRequests.SystemEvent;

        this.StatusBusyBitmask = ~0;
        this.StatusDataReadyBitmask = ( int ) Pith.ServiceRequests.MessageAvailable;
        this.StatusQueryResultReadyBitmask = ( int ) Pith.ServiceRequests.MessageAvailable;

        this.ServiceRequestEnableOperationCompleteBitmask = ( ServiceRequests ) 191;
        this.StandardEventEnableOperationCompleteBitmask = ( StandardEvents ) 253;
        this.ServiceRequestEnableEventsBitmask = ( ServiceRequests ) 191;
        this.StandardEventEnableEventsBitmask = ( StandardEvents ) 253;

        // SCPI EXCEPTIONS

        // Compound common commands, e.g., *CLS *OPC, of SCPI instruments need not execute as separate commands
        this.SplitCommonCommands = false;

        // Per the SCPI specifications, *CLS does not clear the event enable registers
        this.StatusClearDistractive = false;

        // Per the SCPI specifications, *CLS must clear the status byte by clearing the error queue and all standard events
        this.ClearsDeviceStructures = true;

    }

    /// <summary>   Applies the tsp syntax. </summary>
    /// <remarks>   2024-09-14. </remarks>
    public void ApplyTspSyntax()
    {
        this.DeviceClearDelayPeriod = TimeSpan.FromMilliseconds( 10d );

        this.ClearExecutionStateCommand = Syntax.Tsp.Lua.ClearExecutionStateCommand;
        this.ClearExecutionStateQueryCompleteCommand = Syntax.Tsp.Lua.ClearExecutionStateQueryCompleteCommand;
        this.ClearExecutionStateWaitCommand = Syntax.Tsp.Lua.ClearExecutionStateWaitCommand;
        this.ClearExecutionStateOperationCompleteCommand = Syntax.Tsp.Lua.ClearExecutionStateOperationCompleteCommand;
        this.CollectGarbageOperationCompleteCommand = Syntax.Tsp.Lua.CollectGarbageOperationCompleteCommand;
        this.CollectGarbageQueryCompleteCommand = Syntax.Tsp.Lua.CollectGarbageQueryCompleteCommand;
        this.CollectGarbageOperationCompleteCommandFormat = Syntax.Tsp.Node.CollectGarbageOperationCompleteCommandFormat;
        this.CollectGarbageQueryCompleteCommandFormat = Syntax.Tsp.Node.CollectGarbageQueryCompleteCommandFormat;
        this.IdentificationQueryCommand = Syntax.Tsp.LocalNode.IdentificationQueryCommand;
        this.OperationCompleteCommand = Syntax.Tsp.Lua.OperationCompleteCommand;
        this.OperationCompletedQueryCommand = Syntax.Tsp.Lua.OperationCompletedQueryCommand;
        this.ResetKnownStateCommand = Syntax.Tsp.Lua.ResetKnownStateCommand;
        this.ResetKnownStateWaitCommand = Syntax.Tsp.Lua.ResetKnownStateWaitCommand;

        this.ServiceRequestEnableCommandFormat = Syntax.Tsp.Status.ServiceRequestEnableCommandFormat;
        this.ServiceRequestEnableQueryCommand = Syntax.Tsp.Status.ServiceRequestEnableQueryCommand;
        this.StatusByteQueryCommand = Syntax.Tsp.Status.ServiceRequestEventQueryCommand;

#if false
        // not implemented yet.
        this.ServiceRequestEnableNodeCommandFormat = Syntax.Tsp.Node.ServiceRequestEnableCommandFormat;
        this.ServiceRequestEventQueryNodeCommandFormat = Syntax.Tsp.Node.ServiceRequestEventQueryCommandFormat
        this.StatusByteQueryNodeCommand = Syntax.Tsp.Node.ServiceRequestEventQueryCommandFormat;
#endif

        this.StandardEventEnableCommandFormat = Syntax.Tsp.Status.StandardEventEnableCommandFormat;
        this.StandardEventEnableQueryCommand = Syntax.Tsp.Status.StandardEventEnableQueryCommand;
        this.StandardEventStatusQueryCommand = Syntax.Tsp.Status.StandardEventStatusQueryCommand;

#if false
        // not implemented yet.
        this.StandardEventEnableNodeCommandFormat = Syntax.Tsp.Node.StandardEventEnableCommandFormat;
        this.StandardEventStatusQueryNodeCommand = Syntax.Tsp.Node.StandardEventQueryCommandFormat;
        this.StandardEventEnableQueryNodeCommand = Syntax.Tsp.Node.StandardEventEnableQueryCommandFormat;

        this.StandardServiceEnableNodeCommandFormat = Syntax.Tsp.Node.StandardServiceEnableCommandFormat;
#endif

        this.StandardServiceEnableCommandFormat = Syntax.Tsp.Status.StandardServiceEnableCommandFormat;
        this.StandardServiceEnableOperationCompleteCommandFormat = Syntax.Tsp.Status.StandardServiceEnableOperationCompleteCommandFormat;
        this.StandardServiceEnableQueryCompleteCommandFormat = Syntax.Tsp.Status.StandardServiceEnableQueryCompleteCommandFormat;

        this.WaitCommand = Syntax.Tsp.Lua.WaitCommand;

        this.ClearErrorQueueCommand = VI.Syntax.Tsp.ErrorQueue.ClearErrorQueueWaitCommand;
        this.DeviceErrorQueryCommand = string.Empty;
        this.DequeueErrorQueryCommand = string.Empty;
        this.ErrorQueueCountQueryCommand = "_G.print(_G.string.format('%d',_G.errorqueue.count))";
        this.LastSystemErrorQueryCommand = string.Empty;
        this.NextDeviceErrorQueryCommand = VI.Syntax.Tsp.ErrorQueue.ErrorQueueQueryCommand;
        this.NoErrorCompoundMessage = VI.Syntax.ScpiSyntax.NoErrorCompoundMessage;
        this.DeviceErrorQueue = new DeviceErrorQueue( VI.Syntax.ScpiSyntax.NoErrorCompoundMessage );

        // IO PROPERTIES

        // The ASCII character used to end reading [10]
        this.ReadTerminationCharacter = Std.EscapeSequencesExtensions.EscapeSequencesExtensionMethods.NEW_LINE_VALUE;

        // The value indicating whether the read operation ends when a termination character is received [true]
        this.ReadTerminationCharacterEnabled = true;

        // STATUS REGISTER PROPERTIES
        this.ErrorAvailableBitmask = Pith.ServiceRequests.ErrorAvailable;
        this.MeasurementEventBitmask = Pith.ServiceRequests.MeasurementEvent;
        this.MessageAvailableBitmask = Pith.ServiceRequests.MessageAvailable;
        this.OperationEventSummaryBitmask = Pith.ServiceRequests.OperationEvent;
        this.QuestionableEventBitmask = Pith.ServiceRequests.QuestionableEvent;
        this.RequestingServiceBitmask = Pith.ServiceRequests.RequestingService;
        this.StandardEventSummaryBitmask = Pith.ServiceRequests.StandardEventSummary;
        this.SystemEventBitmask = Pith.ServiceRequests.SystemEvent;

        this.StatusBusyBitmask = ~0;
        this.StatusDataReadyBitmask = ( int ) Pith.ServiceRequests.MessageAvailable;
        this.StatusQueryResultReadyBitmask = ( int ) Pith.ServiceRequests.MessageAvailable;

        this.ServiceRequestEnableOperationCompleteBitmask = ( ServiceRequests ) 191;
        this.StandardEventEnableOperationCompleteBitmask = ( StandardEvents ) 253;
        this.ServiceRequestEnableEventsBitmask = ( ServiceRequests ) 191;
        this.StandardEventEnableEventsBitmask = ( StandardEvents ) 253;

        // SCPI EXCEPTIONS

        // Compound common commands, e.g., *CLS *OPC, of TSP instruments such as the 2600 must execute as separate commands
        this.SplitCommonCommands = true;

        // Contrary to the SCPI specifications, the 2600 instrument *CLS clears the event enable registers
        this.StatusClearDistractive = true;

        // Per the SCPI specifications, *CLS must clear the status byte by clearing the error queue and all standard events
        // the 2600 breaks this.
        this.ClearsDeviceStructures = false;
    }

    private void ApplyTsp2Syntax()
    {
        // the session instance exists at this point.
        // TO_DO: Needs updating based on the Apply Tsp Syntax method
        this.ClearExecutionStateCommand = Syntax.Tsp.Lua.ClearExecutionStateCommand;
        this.OperationCompletedQueryCommand = Syntax.Tsp.Status.OperationEventConditionQueryCommand;
        this.ResetKnownStateCommand = Syntax.Tsp.Lua.ResetKnownStateCommand;
        this.ServiceRequestEnableCommandFormat = Syntax.Tsp.Status.ServiceRequestEnableCommandFormat;
        this.ServiceRequestEnableQueryCommand = Syntax.Tsp.Status.ServiceRequestEnableQueryCommand;
        this.StandardEventStatusQueryCommand = Syntax.Tsp.Status.StandardEventStatusQueryCommand;
        this.StandardEventEnableQueryCommand = Syntax.Tsp.Status.StandardEventEnableQueryCommand;
        this.StandardServiceEnableCommandFormat = Syntax.Tsp.Status.StandardServiceEnableCommandFormat;
        this.WaitCommand = Syntax.Tsp.Lua.WaitCommand;
        this.ErrorAvailableBitmask = Pith.ServiceRequests.ErrorAvailable;
        this.MeasurementEventBitmask = Pith.ServiceRequests.MeasurementEvent;
        this.MessageAvailableBitmask = Pith.ServiceRequests.MessageAvailable;
        this.OperationEventSummaryBitmask = Pith.ServiceRequests.OperationEvent;
        this.QuestionableEventBitmask = Pith.ServiceRequests.QuestionableEvent;
        this.RequestingServiceBitmask = Pith.ServiceRequests.RequestingService;
        this.StandardEventSummaryBitmask = Pith.ServiceRequests.StandardEventSummary;
        this.SystemEventBitmask = Pith.ServiceRequests.SystemEvent;
    }

    #endregion

    #region " delays with or without do events actions "

    /// <summary>   Gets or sets the do events action. </summary>
    /// <value> The do events action. </value>
    public static Action? DoEventsAction { get; set; }

    /// <summary>   Gets or sets the do events delay threshold. </summary>
    /// <value> The do events delay threshold. Initialized at 16 ms.</value>
    public static TimeSpan DoEventsDelayThreshold { get; set; } = TimeSpan.FromMilliseconds( 16 );

    /// <summary>
    /// Delays operations by waiting for a task to complete. If defined and the <paramref name="delay"/>
    /// exceeds the <paramref name="doEventsDelayThreshold"/>, the <see cref="DoEventsAction"/>
    /// is invoked on each loop of the
    /// <see cref="cc.isr.Std.StopwatchExtensions.StopwatchExtensionMethods.AsyncLetElapseUntil(Stopwatch, TimeSpan, TimeSpan, Func{bool}, Action?)"/>
    /// method.
    /// </summary>
    /// <remarks>   2024-07-05. </remarks>
    /// <param name="delay">                    The delay. </param>
    /// <param name="doEventsDelayThreshold">   The do events delay threshold. </param>
    /// <param name="spinWaitIterations">       (Optional) The spin wait iterations. </param>
    /// <param name="minDelayMilliseconds">     (Optional) The minimum delay milliseconds. </param>
    /// <returns>   A TimeSpan. </returns>
    public static TimeSpan AsyncDelay( TimeSpan delay, TimeSpan doEventsDelayThreshold, int spinWaitIterations = 10, int minDelayMilliseconds = 1 )
    {
        if ( spinWaitIterations > 0 )
            System.Threading.Thread.SpinWait( spinWaitIterations );
        return delay > TimeSpan.Zero
            ? delay > doEventsDelayThreshold
                ? SessionBase.DoEventsAction is not null
                    ? delay.AsyncWait( SessionBase.DoEventsAction )
                    : delay.AsyncWait()
                : delay.AsyncWait()
            : minDelayMilliseconds > 0
                ? TimeSpan.FromMilliseconds( minDelayMilliseconds ).AsyncWait()
                : TimeSpan.Zero;
    }

    /// <summary>
    /// Delays operations by waiting for a task to complete. If defined and the <paramref name="delay"/>
    /// exceeds the <see cref="SessionBase.DoEventsDelayThreshold"/>, the <see cref="DoEventsAction"/>
    /// is invoked on each loop of the
    /// <see cref="cc.isr.Std.StopwatchExtensions.StopwatchExtensionMethods.AsyncLetElapseUntil(Stopwatch, TimeSpan, TimeSpan, Func{bool}, Action?)"/>
    /// method.
    /// </summary>
    /// <remarks>   2024-08-10. </remarks>
    /// <param name="delay">    The delay. </param>
    /// <returns>   A TimeSpan. </returns>
    public static TimeSpan AsyncDelay( TimeSpan delay )
    {
        return AsyncDelay( delay, SessionBase.DoEventsDelayThreshold );
    }

    #endregion

    #region " session "

    /// <summary>
    /// Gets or sets the Enabled sentinel. When enabled, the session is allowed to engage actual
    /// hardware; otherwise, opening the session does not attempt to link to the hardware.
    /// </summary>
    /// <value> The enabled sentinel. </value>
    public bool Enabled { get; set; }

    /// <summary> Gets or sets the awaitStatusTimeout for opening a session. </summary>
    /// <value> The awaitStatusTimeout for opening a session. </value>
    public TimeSpan OpenTimeout { get; set; }

    /// <summary>
    /// Gets or sets the session open sentinel. When open, the session is capable of addressing the
    /// hardware. See also <see cref="IsDeviceOpen"/>.
    /// </summary>
    /// <value> The is session open. </value>
    public abstract bool IsSessionOpen { get; }

    /// <summary> True if device is open, false if not. </summary>
    private bool _isDeviceOpen;

    /// <summary>
    /// Gets or sets the Device Open sentinel. When open, the device is capable of addressing real
    /// hardware if the session is open. See also <see cref="IsSessionOpen"/>.
    /// </summary>
    /// <value> The is device open. </value>
    public bool IsDeviceOpen
    {
        get => this._isDeviceOpen;

        protected set
        {
            if ( _ = base.SetProperty( ref this._isDeviceOpen, value ) )
                this.UpdateCaptions();
        }
    }

    /// <summary> Gets or sets the state of the resource open. </summary>
    /// <value> The resource open state. </value>
    public ResourceOpenState ResourceOpenState { get; set; }

    /// <summary> Gets or sets the sentinel indicating whether this is a dummy session. </summary>
    /// <value> The dummy sentinel. </value>
    public abstract bool IsDummy { get; }

    /// <summary> Executes the session open actions. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <param name="resourceName">  The name of the resource. </param>
    /// <param name="resourceModel"> The short title of the device. </param>
    protected virtual void OnSessionOpen( string resourceName, string resourceModel )
    {
        this.OnSessionOpen( resourceName, resourceModel, ResourceOpenState.Success );
    }

    /// <summary> Executes the session open actions. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <param name="resourceName">      The name of the resource. </param>
    /// <param name="resourceModel">     The short title of the device. </param>
    /// <param name="resourceOpenState"> The resource open state. </param>
    protected virtual void OnSessionOpen( string resourceName, string resourceModel, ResourceOpenState resourceOpenState )
    {
        // Use SynchronizeCallbacks to specify that the object marshals callbacks across threads appropriately.
        this.SynchronizeCallbacks = true;
        this.OpenResourceName = resourceName;
        this.OpenResourceModel = resourceModel;
        this.CandidateResourceModel = this.OpenResourceModel;
        this.ResourceNameCaption = $"{this.OpenResourceModel}.{this.OpenResourceName}";
        this.ResourceModelCaption = this.OpenResourceModel;
        this.ResourceOpenState = resourceOpenState;
        this.IsDeviceOpen = resourceOpenState != ResourceOpenState.Unknown;
        this.SetLastAction( $"{resourceName} open" );
        this.StatusPrompt = this.LastAction;
        this.HandleSessionOpen( resourceName, resourceModel );
        this.CommunicationTimeouts.Push( this.CommunicationTimeout );
    }

    /// <summary> Creates a session. </summary>
    /// <remarks> Throws an exception if the resource is not accessible. </remarks>
    /// <param name="resourceName"> The name of the resource. </param>
    /// <param name="timeout">      The awaitStatusTimeout. </param>
    protected abstract void CreateSession( string resourceName, TimeSpan timeout );

    /// <summary> Check if a session can be created. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    /// <param name="resourceName"> The name of the resource. </param>
    /// <param name="timeout">       The awaitStatusTimeout for opening (creating) a session. </param>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public (bool Success, string Details) CanCreateSession( string resourceName, TimeSpan timeout )
    {
        try
        {
            this.CreateSession( resourceName, timeout );
            return (true, string.Empty);
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            return (false, ex.BuildMessage());
        }
        finally
        {
            try
            {
                this.DisposeSession();
            }
            catch
            {
            }
            finally
            {
            }
        }
    }

    /// <summary>
    /// Opens a VISA <see cref="VI.Pith.SessionBase">Session</see> to access the instrument.
    /// </summary>
    /// <remarks>
    /// Call this first. The synchronization context is captured as part of the property change and
    /// other event handlers and is no longer needed here.
    /// </remarks>
    /// <param name="resourceName">  The name of the resource. </param>
    /// <param name="resourceModel"> The short title of the device. </param>
    /// <param name="timeout">       The awaitStatusTimeout for opening (creating) a session. </param>
    public void OpenSession( string resourceName, string resourceModel, TimeSpan timeout )
    {
        try
        {
            this.ClearLastError();
            this.ResourceNameInfo = new ResourceNameInfo( resourceName );
            this.CandidateResourceName = resourceName;
            this.CandidateResourceModel = resourceModel;
            this.SetLastAction( $"opening {resourceName}" );
            this.StatusPrompt = this.LastAction;
            this.CommunicationTimeouts.Clear();
            this.CreateSession( resourceName, timeout );
            this.OnSessionOpen( resourceName, resourceModel );
        }
        catch ( Exception )
        {
            throw;
        }
    }

    /// <summary>
    /// Opens a VISA <see cref="VI.Pith.SessionBase">Session</see> to access the instrument.
    /// </summary>
    /// <remarks>
    /// Call this first. The synchronization context is captured as part of the property change and
    /// other event handlers and is no longer needed here.
    /// </remarks>
    /// <param name="resourceName">  The name of the resource. </param>
    /// <param name="resourceModel"> The short title of the device. </param>
    public void OpenSession( string resourceName, string resourceModel )
    {
        this.OpenSession( resourceName, resourceModel, this.OpenTimeout );
    }

    /// <summary>
    /// Opens a VISA <see cref="VI.Pith.SessionBase">Session</see> to access the instrument.
    /// </summary>
    /// <remarks>
    /// Call this first. The synchronization context is captured as part of the property change and
    /// other event handlers and is no longer needed here.
    /// </remarks>
    /// <param name="resourceName"> The name of the resource. </param>
    public void OpenSession( string resourceName )
    {
        this.OpenSession( resourceName, resourceName, this.OpenTimeout );
    }

    /// <summary>
    /// Opens a VISA <see cref="VI.Pith.SessionBase">Session</see> to access the instrument.
    /// </summary>
    /// <remarks>
    /// Call this first. The synchronization context is captured as part of the property change and
    /// other event handlers and is no longer needed here.
    /// </remarks>
    /// <param name="resourceName"> The name of the resource. </param>
    /// <param name="timeout">      The awaitStatusTimeout. </param>
    public void OpenSession( string resourceName, TimeSpan timeout )
    {
        this.OpenSession( resourceName, resourceName, timeout );
    }

    /// <summary> Gets or sets the sentinel indication that the VISA session is disposed. </summary>
    /// <value> The is session disposed. </value>
    public abstract bool IsSessionDisposed { get; }

    /// <summary>
    /// Disposes the VISA <see cref="VI.Pith.SessionBase">Session</see> ending access to the
    /// instrument.
    /// </summary>
    protected abstract void DisposeSession();

    /// <summary> Discard all events. </summary>
    protected abstract void DiscardAllEvents();

    /// <summary>
    /// Closes the VISA <see cref="VI.Pith.SessionBase">Session</see> to access the instrument.
    /// </summary>
    public void CloseSession()
    {
        this.SetLastAction( $"{this.OpenResourceName} clearing last error" );
        this.ClearLastError();
        this.CommunicationTimeouts.Clear();
        if ( this.IsDeviceOpen )
        {
            this.SetLastAction( $"{this.OpenResourceName} discarding all events" );
            this.DiscardAllEvents();
            this.SetLastAction( $"{this.OpenResourceName} disabling service request" );
            this.DisableServiceRequestEventHandler();
            this.IsDeviceOpen = false;
        }

        this.SetLastAction( $"{this.OpenResourceName} closed" );
        this.StatusPrompt = this.LastAction;

        // disposes the session.
        if ( this.IsSessionOpen )
            this.DisposeSession();

        // must use sync notification to prevent race condition if disposing the session.
        base.OnPropertyChanged( new PropertyChangedEventArgs( nameof( this.IsSessionOpen ) ) );
    }


    /// <summary>   Builds caller member message. </summary>
    /// <remarks>   2024-09-04. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    /// <returns>   A string. </returns>
    public static string BuildCallerMemberMessage( string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        return $"{message} at [{sourcePath}].{memberName}.Line#{sourceLineNumber}";
    }

    /// <summary>   Sets the last action message. </summary>
    /// <remarks>   2024-09-04. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public void SetLastAction( string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        this.LastAction = $"{this.ResourceNameNodeCaption} {message} at [{sourcePath}].{memberName}.Line#{sourceLineNumber}";
    }

    private string _lastAction = string.Empty;

    /// <summary> Gets or sets the last action. </summary>
    /// <value> The last action. </value>
    public string LastAction
    {
        get => this._lastAction;
        set => _ = base.SetProperty( ref this._lastAction, value );
    }

    /// <summary>   Sets the last Action detailed message. </summary>
    /// <remarks>   2024-09-04. </remarks>
    /// <param name="message">          The detailed message. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) full path name of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public void SetLastActionDetails( string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        this.LastActionDetails = $"{this.ResourceNameNodeCaption} {message} at [{sourcePath}].{memberName}.Line#{sourceLineNumber}";
    }

    private string _lastActionDetails = string.Empty;

    /// <summary> Gets or sets the last Action detailed message. </summary>
    /// <value> The last action detailed message. </value>
    public string LastActionDetails
    {
        get => this._lastActionDetails;
        set => _ = base.SetProperty( ref this._lastActionDetails, value );
    }

    /// <summary>   Gets the node number caption. </summary>
    /// <value> The node number caption. </value>
    public string LastNodeNumberCaption => this.LastNodeNumber.HasValue ? $".Node# {this.LastNodeNumber:00}" : string.Empty;

    /// <summary> The last node number. </summary>
    private int? _lastNodeNumber;

    /// <summary> Gets or sets the last node. </summary>
    /// <value> The last node. </value>
    public int? LastNodeNumber
    {
        get => this._lastNodeNumber;
        set => _ = base.SetProperty( ref this._lastNodeNumber, value );
    }

    /// <summary>
    /// Gets the sentinel indicating if call backs are performed in a specific synchronization
    /// context.
    /// </summary>
    /// <value>
    /// The sentinel indicating if call backs are performed in a specific synchronization context.
    /// </value>
    public virtual bool SynchronizeCallbacks { get; set; }

    #region " error / status "

    /// <summary> Clears the last error. </summary>
    protected void ClearLastError()
    {
        this.LastNativeError = NativeErrorBase.Success;
    }

    /// <summary> Gets the last native error. </summary>
    /// <value> The last native error. </value>
    protected NativeErrorBase? LastNativeError { get; set; }

    #endregion

    #region " message events "

    private BindingList<KeyValuePair<MessageNotificationModes, string>>? _messageNotificationModeKeyValuePairs;

    /// <summary>   Gets the message notification modes as a key value pairs binding list. </summary>
    /// <value> The message notification modes. </value>
    public BindingList<KeyValuePair<MessageNotificationModes, string>>? MessageNotificationModeKeyValuePairs
    {
        get
        {
            if ( this._messageNotificationModeKeyValuePairs is null || !this._messageNotificationModeKeyValuePairs.Any() )
            {
                this._messageNotificationModeKeyValuePairs = [];
                foreach ( KeyValuePair<Enum, string> item in cc.isr.Enums.EnumExtensions.ValueNamePairs( typeof( MessageNotificationModes ) ) )
                {
                    this._messageNotificationModeKeyValuePairs.Add( new KeyValuePair<MessageNotificationModes, string>( ( MessageNotificationModes ) item.Key, item.Value ) );
                }
            }
            return this._messageNotificationModeKeyValuePairs;
        }
    }

    private KeyValuePair<MessageNotificationModes, string>? _messageNotificationModeKeyValuePair;

    /// <summary>   Gets or sets the message notification mode key value pair. </summary>
    /// <value> The message notification mode. </value>
    public KeyValuePair<MessageNotificationModes, string>? MessageNotificationModeKeyValuePair
    {
        get => this._messageNotificationModeKeyValuePair;
        set
        {
            if ( value is not null )
            {
                KeyValuePair<MessageNotificationModes, string> newValue = ( KeyValuePair<MessageNotificationModes, string> ) value!;
                if ( this.MessageNotificationModeKeyValuePair is null )
                {
                    this._messageNotificationModeKeyValuePair = value;
                    this.MessageNotificationModes = newValue.Key;
                    base.OnPropertyChanged( new PropertyChangedEventArgs( nameof( this.MessageNotificationModeKeyValuePair ) ) );
                }
                else
                {
                    KeyValuePair<MessageNotificationModes, string> oldValue = ( KeyValuePair<MessageNotificationModes, string> ) this.MessageNotificationModeKeyValuePair!;
                    if ( newValue.Key != oldValue.Key )
                    {
                        this._messageNotificationModeKeyValuePair = value;
                        this.MessageNotificationModes = newValue.Key;
                        base.OnPropertyChanged( new PropertyChangedEventArgs( nameof( this.MessageNotificationModeKeyValuePair ) ) );
                    }
                }
            }
        }
    }

    private MessageNotificationModes _messageNotificationModes = MessageNotificationModes.None;

    /// <summary>   Gets or sets the message notification modes. </summary>
    /// <value> The message notification modes. </value>
    public MessageNotificationModes MessageNotificationModes
    {
        get => this._messageNotificationModes;
        set
        {
            if ( _ = base.SetProperty( ref this._messageNotificationModes, value ) )
            {
                this.MessageNotificationModeKeyValuePair = ToMessageNotificationModes( value );
            }
        }
    }

    /// <summary> Converts a value to a Message Notification Modes. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> Value as a KeyValuePair(Of NotifySyncLevel, String) </returns>
    public static KeyValuePair<MessageNotificationModes, string> ToMessageNotificationModes( MessageNotificationModes value )
    {
        return new KeyValuePair<MessageNotificationModes, string>( value, value.ToString() );
    }

    private string _lastMessageReceived = string.Empty;

    /// <summary>
    /// Gets or sets the last message Received and report a changed property if the
    /// <see cref="MessageNotificationModes"/> <see cref="MessageNotificationModes.Received"/> flag is on.
    /// </summary>
    /// <remarks>
    /// The last message sent is posted asynchronously. This may not be processed fast enough with
    /// TSP devices to determine the state of the instrument.
    /// </remarks>
    /// <value> The last message Received. </value>
    public string LastMessageReceived
    {
        get => this._lastMessageReceived;

        protected set
        {
            this._lastMessageReceived = value;
            if ( 0 != (MessageNotificationModes.Received & this.MessageNotificationModes) )
                base.OnPropertyChanged( new PropertyChangedEventArgs( nameof( this.LastMessageReceived ) ) );
        }
    }

    private string _lastMessageSent = string.Empty;

    /// <summary>
    /// Gets or sets the last message Sent and report a changed property if the
    /// <see cref="MessageNotificationModes"/> <see cref="MessageNotificationModes.Sent"/> flag is on.
    /// </summary>
    /// <remarks>   The last message sent is posted asynchronously. </remarks>
    /// <value> The last message Sent. </value>
    public string LastMessageSent
    {
        get => this._lastMessageSent;

        protected set
        {
            this._lastMessageSent = value;
            if ( 0 != (MessageNotificationModes.Sent & this.MessageNotificationModes) )
                base.OnPropertyChanged( new PropertyChangedEventArgs( nameof( this.LastMessageSent ) ) );
        }
    }

    #endregion

    #endregion

    #region " read / write "

    private TimeSpan _communicationTimeout = TimeSpan.FromMilliseconds( 3000 );

    /// <summary> Gets the awaitStatusTimeout for I/O communication on this resource session. </summary>
    /// <value> The communication awaitStatusTimeout. </value>
    public virtual TimeSpan CommunicationTimeout
    {
        get => this._communicationTimeout;
        set => _ = base.SetProperty( ref this._communicationTimeout, value );
    }

    private byte _readTerminationCharacter = Std.EscapeSequencesExtensions.EscapeSequencesExtensionMethods.NEW_LINE_VALUE;

    /// <summary> Gets the ASCII character used to end reading. </summary>
    /// <value> The read termination character. </value>
    public virtual byte ReadTerminationCharacter
    {
        get => this._readTerminationCharacter;
        set => _ = base.SetProperty( ref this._readTerminationCharacter, value );
    }

    private bool _readTerminationCharacterEnabled = true;

    /// <summary>
    /// Gets the value indicating whether the read operation ends when a termination character is received.
    /// </summary>
    /// <value> The termination character enabled value. </value>
    public virtual bool ReadTerminationCharacterEnabled
    {
        get => this._readTerminationCharacterEnabled;
        set => _ = base.SetProperty( ref this._readTerminationCharacterEnabled, value );
    }

    /// <summary> Gets the emulated reply. </summary>
    /// <value> The emulated reply. </value>
    public string EmulatedReply { get; set; } = string.Empty;

    /// <summary>
    /// Synchronously reads ASCII-encoded string data. Read characters into the return string until
    /// an END indicator or <see cref="ReadTerminationCharacter">termination character</see>
    /// termination character is reached. Limited by the
    /// <see cref="InputBufferSize"/>. Will time out if end of line is not read before reading a
    /// buffer.
    /// </summary>
    /// <returns> The received message. </returns>
    public abstract string ReadFiniteLine();

    /// <summary>
    /// Tries to <see cref="ReadFiniteLine"/> returning an nullable string on an exception.
    /// </summary>
    /// <returns> The received message. </returns>
    public string? TryReadFiniteLine()
    {
        try
        {
            return this.ReadFiniteLine();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Synchronously reads ASCII-encoded string data until an END indicator or
    /// <see cref="ReadTerminationCharacter">termination character</see>
    /// termination character is reached irrespective of the buffer size.
    /// </summary>
    /// <returns> The received message. </returns>
    public abstract string ReadFreeLine();

    /// <summary>
    /// Tries to <see cref="ReadFreeLine"/> returning an nullable string on an exception.
    /// </summary>
    /// <returns> The received message. </returns>
    public string? TryReadFreeLine()
    {
        try
        {
            return this.ReadFreeLine();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks> David, 2020-07-23. </remarks>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected abstract string SyncWriteLine( string dataToWrite );

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.<para>
    /// Per IVI documentation: Converts the specified string to an ASCII string and appends it to the
    /// formatted I/O write buffer</para>
    /// </summary>
    /// <remarks> David, 2020-07-23. </remarks>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected abstract string SyncWrite( string dataToWrite );

    /// <summary> Gets or set the value of the last status byte. </summary>
    /// <value> The status byte. </value>
    public ServiceRequests StatusByte { get; set; }

    /// <summary> Thread unsafe read status byte. </summary>
    /// <returns> The VI.Pith.ServiceRequests. </returns>
    protected abstract ServiceRequests ThreadUnsafeReadStatusByte();

    /// <summary> Gets the status byte locker. </summary>
    /// <value> The status byte locker. </value>
    private ReaderWriterLockSlim StatusByteLocker { get; set; } = new ReaderWriterLockSlim();

    /// <summary> Reads status byte. </summary>
    /// <returns> The status byte. </returns>
    public virtual ServiceRequests ReadStatusByte()
    {
        this.StatusByteLocker.EnterReadLock();
        try
        {
            return this.ThreadUnsafeReadStatusByte();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.StatusByteLocker.ExitReadLock();
        }
    }

    /// <summary> Gets the size of the input buffer. </summary>
    /// <value> The size of the input buffer. </value>
    public abstract int InputBufferSize { get; }

    #endregion

    #region " emulation "

    /// <summary> Makes emulated reply. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeTrueFalseReply( bool value )
    {
        this.EmulatedReply = ToTrueFalse( value );
    }

    /// <summary> Makes emulated reply if empty. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeTrueFalseReplyIfEmpty( bool value )
    {
        if ( string.IsNullOrWhiteSpace( this.EmulatedReply ) )
        {
            this.MakeTrueFalseReply( value );
        }
    }

    /// <summary> Makes emulated reply. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReply( bool value )
    {
        this.EmulatedReply = ToOneZero( value );
    }

    /// <summary> Makes emulated reply if empty. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReplyIfEmpty( bool value )
    {
        if ( string.IsNullOrWhiteSpace( this.EmulatedReply ) )
        {
            this.MakeEmulatedReply( value );
        }
    }

    /// <summary> Makes emulated reply. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReply( double value )
    {
        this.EmulatedReply = value.ToString();
    }

    /// <summary> Makes emulated reply if empty. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReplyIfEmpty( double value )
    {
        if ( string.IsNullOrWhiteSpace( this.EmulatedReply ) )
        {
            this.MakeEmulatedReply( value );
        }
    }

    /// <summary>   Makes emulated reply. </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="value">    The emulated value. </param>
    public void MakeEmulatedReply( decimal value )
    {
        this.EmulatedReply = value.ToString();
    }

    /// <summary>   Makes emulated reply if empty. </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="value">    The emulated value. </param>
    public void MakeEmulatedReplyIfEmpty( decimal value )
    {
        if ( string.IsNullOrWhiteSpace( this.EmulatedReply ) )
        {
            this.MakeEmulatedReply( value );
        }
    }

    /// <summary> Makes emulated reply. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReply( int value )
    {
        this.EmulatedReply = value.ToString();
    }

    /// <summary> Makes emulated reply if empty. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReplyIfEmpty( int value )
    {
        if ( string.IsNullOrWhiteSpace( this.EmulatedReply ) )
        {
            this.MakeEmulatedReply( value );
        }
    }

    /// <summary> Makes emulated reply. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReply( TimeSpan value )
    {
        this.EmulatedReply = value.ToString();
    }

    /// <summary> Makes emulated reply if empty. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReplyIfEmpty( TimeSpan value )
    {
        if ( string.IsNullOrWhiteSpace( this.EmulatedReply ) )
        {
            this.MakeEmulatedReply( value );
        }
    }

    /// <summary> Makes emulated reply. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReply( string value )
    {
        this.EmulatedReply = value;
    }

    /// <summary> Makes emulated reply if empty. </summary>
    /// <param name="value"> The emulated value. </param>
    public void MakeEmulatedReplyIfEmpty( string value )
    {
        if ( string.IsNullOrWhiteSpace( this.EmulatedReply ) )
        {
            this.MakeEmulatedReply( value );
        }
    }

    #endregion

    #region " trigger "

    /// <summary>
    /// Asserts a software or hardware trigger depending on the interface; Sends a bus trigger.
    /// </summary>
    public abstract void AssertTrigger();

    #endregion

    #region " interface "

    /// <summary> Supports clear interface. </summary>
    /// <value> <c>true</c> if supports clearing the interface. </value>
    public bool SupportsClearInterface => this.ResourceNameInfo is not null && (HardwareInterfaceType.Gpib == this.ResourceNameInfo.InterfaceType);

    /// <summary> The interface clear refractory period. </summary>
    private TimeSpan _interfaceClearRefractoryPeriod;

    /// <summary> Gets or sets the Interface clear refractory period. </summary>
    /// <value> The Interface clear refractory period. </value>
    public TimeSpan InterfaceClearRefractoryPeriod
    {
        get => this._interfaceClearRefractoryPeriod;
        set => _ = base.SetProperty( ref this._interfaceClearRefractoryPeriod, value );
    }

    /// <summary> Clears the interface. </summary>
    public void ClearInterface()
    {
        if ( this.SupportsClearInterface )
        {
            this.ImplementClearHardwareInterface();
            _ = SessionBase.AsyncDelay( this.InterfaceClearRefractoryPeriod );
        }
    }

    /// <summary> Implement clear hardware interface. </summary>
    protected abstract void ImplementClearHardwareInterface();

    /// <summary> Clears the device (CDC). </summary>
    /// <remarks>
    /// When communicating with a message-based device, particularly when you are first developing
    /// your program, you may need to tell the device to clear its I/O buffers so that you can start
    /// again. In addition, if a device has more information than you need, you may want to read
    /// until you have everything you need and then tell the device to throw the rest away. The
    /// <c>viClear()</c> operation performs these tasks. More specifically, the clear operation lets
    /// a controller send the device clear command to the device it is associated with, as specified
    /// by the interface specification and the type of device. The action that the device takes
    /// depends on the interface to which it is connected. <para>
    /// For a GPIB device, the controller sends the IEEE 488.1 SDC (04h) command.</para><para>
    /// For a VXI or MXI device, the controller sends the Word Serial Clear (FFFFh) command.</para>
    /// <para>
    /// For the ASRL INSTR or TCPIP SOCKET resource, the controller sends the string "*CLS\n". The
    /// I/O protocol must be set to VI_PROT_4882_STRS for this service to be available to these
    /// resources.</para>
    /// For more details on these clear commands, refer to your device documentation, the IEEE 488.1
    /// standard, or the VXI bus specification. <para>
    /// Source: NI-Visa HTML help.</para>
    /// </remarks>
    protected abstract void Clear();

    /// <summary> Clears the device (SDC). </summary>
    protected abstract void ClearDevice();

    /// <summary>
    /// Clears the active state. Issues selective device clear. Waits for the
    /// <see cref="DeviceClearRefractoryPeriod"/> before releasing control.
    /// </summary>
    public virtual void ClearActiveState()
    {
        this.ClearActiveState( this.DeviceClearRefractoryPeriod );
    }

    /// <summary>
    /// Clears the active state. Issues selective device clear. Waits for the
    /// <paramref name="refractoryPeriod"/> before releasing control.
    /// </summary>
    /// <remarks>   David, 2021-05-27. </remarks>
    /// <param name="refractoryPeriod">     The refractory period. </param>
    /// <param name="delayMilliseconds">    (Optional) The delay before issuing the clear. <para>
    ///                                     A delay is required before issuing a device clear on some TSP
    ///                                     devices, such as the 2600. First a 1ms delay was added.
    ///                                     Without the delay, the instrument had error -286 TSP Runtime
    ///                                     Error and User Abort error if the clear command was issued
    ///                                     after turning off the status _G.status.request_enable=0
    ///                                     Thereafter, the instrument has a resource not found error
    ///                                     when trying to connect after failing to connect because
    ///                                     instruments were off. Stopping here in debug mode seems to
    ///                                     have alleviated this issue.  So, the delay was increased to
    ///                                     10 ms.
    ///                                     </para> </param>
    public virtual void ClearActiveState( TimeSpan refractoryPeriod, int delayMilliseconds = 10 )
    {
        this.StatusPrompt = $"{this.OpenResourceName} SDC";

        _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( delayMilliseconds ) );
        this.ClearDevice();
        _ = SessionBase.AsyncDelay( refractoryPeriod );

        this.StatusPrompt = $"{this.OpenResourceName} SDC; done";
        _ = this.QueryOperationCompleted();
    }

    /// <summary> The device clear delay period. </summary>
    private TimeSpan _deviceClearDelayPeriod;

    /// <summary> Gets or sets the device clear Delay period. </summary>
    /// <value> The device clear Delay period. </value>
    public TimeSpan DeviceClearDelayPeriod
    {
        get => this._deviceClearDelayPeriod;
        set => _ = base.SetProperty( ref this._deviceClearDelayPeriod, value );
    }

    /// <summary> The device clear refractory period. </summary>
    private TimeSpan _deviceClearRefractoryPeriod;

    /// <summary> Gets or sets the device clear refractory period. </summary>
    /// <value> The device clear refractory period. </value>
    public TimeSpan DeviceClearRefractoryPeriod
    {
        get => this._deviceClearRefractoryPeriod;
        set => _ = base.SetProperty( ref this._deviceClearRefractoryPeriod, value );
    }

    private TimeSpan _operationCompletionRefractoryPeriod;

    /// <summary>
    /// Gets or sets the operation completion awaitStatusTimeout (was status subsystem initialize refractory
    /// period) for instruments that do not support operation completion.
    /// </summary>
    /// <value> The operation completion awaitStatusTimeout. </value>
    public TimeSpan OperationCompletionRefractoryPeriod
    {
        get => this._operationCompletionRefractoryPeriod;
        set => this.SetProperty( ref this._operationCompletionRefractoryPeriod, value );
    }

    private TimeSpan _initKnownStateTimeout;

    /// <summary>
    /// Gets or sets the initialization awaitStatusTimeout, which is use to initialize TSP nodes.
    /// </summary>
    /// <value> The initialize awaitStatusTimeout. </value>
    public TimeSpan InitKnownStateTimeout
    {
        get => this._initKnownStateTimeout;
        set => this.SetProperty( ref this._initKnownStateTimeout, value );
    }

    #endregion

    #region " override not required "

    #region " termination "

    /// <summary> The termination sequence. </summary>
    private string? _terminationSequence;

    /// <summary> Gets or sets the termination sequence. </summary>
    /// <value> The termination sequence. </value>
    public string? TerminationSequence
    {
        get => this._terminationSequence;
        set
        {
            if ( base.SetProperty( ref this._terminationSequence, value ) )
                _ = this.DefineTermination( (value ?? "").ReplaceCommonEscapeSequences().ToCharArray() );
        }
    }

    /// <summary> Determine if we are equal. </summary>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> True if equal, false if not. </returns>
    private static bool AreEqual( IEnumerable<char> left, IEnumerable<char> right )
    {
        bool result = true;
        if ( left is null )
        {
            result = right is null;
        }
        else if ( right is null )
        {
            result = false;
        }
        else if ( left.Any() )
        {
            if ( right.Any() )
            {
                if ( left.Count() == right.Count() )
                {
                    for ( int i = 0, loopTo = left.Count() - 1; i <= loopTo; i++ )
                    {
                        if ( left.ElementAtOrDefault( i ) != right.ElementAtOrDefault( i ) )
                        {
                            result = false;
                            break;
                        }
                    }
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
        }

        return result;
    }

    /// <summary> The termination characters. </summary>
    private char[] _terminationCharacters = ['\n'];

    /// <summary>   Termination characters. </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <returns>   A char[]. </returns>
    public char[] TerminationCharacters()
    {
        return this._terminationCharacters;
    }

    /// <summary>   Enumerates define termination in this collection. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="value">    The value. </param>
    /// <returns>
    /// An enumerator that allows foreach to be used to process define termination in this collection.
    /// </returns>
    public IEnumerable<char> DefineTermination( IEnumerable<char> value )
    {
        if ( this._terminationCharacters is null || !AreEqual( value, this._terminationCharacters! ) )
        {
            this._terminationCharacters = new char[(value.Count())];
            Array.Copy( value.ToArray(), this._terminationCharacters, value.Count() );
            base.OnPropertyChanged( new PropertyChangedEventArgs( nameof( this.TerminationCharacters ) ) );
            this.TerminationSequence = new string( this._terminationCharacters );
        }
        return value;
    }

    /// <summary> Use default termination. </summary>
    private void UseDefaultTerminationThis()
    {
        this.NewTerminationThis( Environment.NewLine.ToCharArray()[1] );
    }

    /// <summary> Use default termination. </summary>
    public void UseDefaultTermination()
    {
        this.UseDefaultTerminationThis();
    }

    /// <summary> Creates a new termination. </summary>
    /// <param name="value"> The emulated value. </param>
    private void NewTerminationThis( char value )
    {
        _ = this.DefineTermination( [value] );
    }

    /// <summary> Creates a new termination. </summary>
    /// <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="values"> The values. </param>
    private void NewTerminationThis( char[] values )
    {
        if ( values is null ) throw new ArgumentNullException( nameof( values ) );
        if ( values.Length == 0 )
            throw new InvalidOperationException( "Failed creating new termination; Source array must have at least one character" );
        char[] terms = new char[values.Length];
        Array.Copy( values, terms, values.Length );
        _ = this.DefineTermination( terms );
    }

    /// <summary> Creates a new termination. </summary>
    /// <param name="values"> The values. </param>
    public void NewTermination( char[] values )
    {
        this.NewTerminationThis( values );
    }

    /// <summary> Use new line termination. </summary>
    public void UseNewLineTermination()
    {
        this.NewTerminationThis( Environment.NewLine.ToCharArray() );
    }

    /// <summary> Use line feed termination. </summary>
    public void UseLinefeedTermination()
    {
        this.NewTerminationThis( Environment.NewLine.ToCharArray()[1] );
    }

    /// <summary> Use Carriage Return termination. </summary>
    public void UseCarriageReturnTermination()
    {
        this.NewTerminationThis( Environment.NewLine.ToCharArray()[0] );
    }

    #endregion

    #region " execute long commands "

    /// <summary> Executes the command using the specified awaitStatusTimeout. </summary>
    /// <param name="command"> The command. </param>
    /// <param name="timeout"> The awaitStatusTimeout. </param>
    public void Execute( string command, TimeSpan timeout )
    {
        if ( string.IsNullOrWhiteSpace( command ) ) throw new ArgumentNullException( nameof( command ) );
        try
        {
            this.StoreCommunicationTimeout( timeout );
            _ = this.WriteLine( command );
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RestoreCommunicationTimeout();
        }
    }

    /// <summary>
    /// Executes the <see cref="Action">action</see> using the specified awaitStatusTimeout.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="action">  The action. </param>
    /// <param name="timeout"> The awaitStatusTimeout. </param>
    public void Execute( Action action, TimeSpan timeout )
    {
        if ( action is null ) throw new ArgumentNullException( nameof( action ) );
        try
        {
            this.StoreCommunicationTimeout( timeout );
            action();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RestoreCommunicationTimeout();
        }
    }

    /// <summary> Executes the command using the specified awaitStatusTimeout. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="action">  The action. </param>
    /// <param name="timeout"> The awaitStatusTimeout. </param>
    /// <returns> A Boolean? </returns>
    public bool? Execute( Func<bool?> action, TimeSpan timeout )
    {
        if ( action is null ) throw new ArgumentNullException( nameof( action ) );
        try
        {
            this.StoreCommunicationTimeout( timeout );
            return action();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RestoreCommunicationTimeout();
        }
    }

    #endregion

    #region " write line "

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.<para>
    /// Per IVI documentation: Converts the specified string to an ASCII string and appends it to the
    /// formatted I/O write buffer</para>
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns> the sent message. </returns>
    public string Write( string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        return this.SyncWrite( dataToWrite );
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface. Terminates the
    /// data with the <see cref="ReadTerminationCharacter">termination character</see>. <para>
    /// Per IVI documentation: Appends a newline (0xA) to the formatted I/O write buffer, flushes the
    /// buffer, and sends an end-of-line with the buffer if required.</para>
    /// </summary>
    /// <param name="format"> The format of the data to write. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string WriteLine( string format, params object[] args )
    {
        if ( string.IsNullOrWhiteSpace( format ) ) throw new ArgumentNullException( nameof( format ) );
        return this.WriteLine( string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    private bool _splitCommonCommands;

    /// <summary>   Gets or sets a value indicating whether to split common commands when writing to the instrument. </summary>
    /// <remarks> TSP instruments do not accept common command separate by semicolon. This commands need to be split and executed as separate commands. </remarks>
    /// <value> True to split common commands when writing to the instrument, false if not. </value>
    public bool SplitCommonCommands
    {
        get => this._splitCommonCommands;
        set => _ = base.SetProperty( ref this._splitCommonCommands, value );
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface. Converts the
    /// specified string to an ASCII string and appends it to the formatted I/O write buffer. Appends
    /// a newline (0xA) to the formatted I/O write buffer, flushes the buffer, and sends an END with
    /// the buffer if required.
    /// </summary>
    /// <remarks>
    /// TSP instruments do not accept common command separate by semicolon. These commands need to be
    /// split and executed as separate commands. A delay is inserted after commands that have
    /// refractory periods defined such as reset or clear.
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   A <see cref="string" />. </returns>
    public string WriteLine( string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        const char compound_Commands_Separator = ';';

        if ( this.SplitCommonCommands && dataToWrite.Contains( compound_Commands_Separator ) )
        {
#pragma warning disable IDE0306 // Simplify collection initialization
            Queue<string> q = new( dataToWrite.Split( compound_Commands_Separator ) );
#pragma warning restore IDE0306 // Simplify collection initialization
            while ( q.Any() )
            {
                string command = q.Dequeue().Trim();
                if ( !string.IsNullOrWhiteSpace( command ) )
                {
                    _ = this.SyncWriteLine( command );
                    if ( command.Equals( this.ResetKnownStateCommand ) )
                        _ = SessionBase.AsyncDelay( this.ResetRefractoryPeriod );
                    else if ( command.Equals( this.ClearExecutionStateCommand ) )
                        _ = SessionBase.AsyncDelay( this.ClearRefractoryPeriod );
                    else
                        _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );
                }
            }
        }
        else
            _ = this.SyncWriteLine( dataToWrite );
        return dataToWrite;
    }

    /// <summary> Writes the value without reading back the presentValue from the device. </summary>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <returns> The presentValue. </returns>
    public int WriteLine( int value, string commandFormat )
    {
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
            _ = this.WriteLine( commandFormat, value );
        return value;
    }

    /// <summary> Writes the value without reading back the presentValue from the device. </summary>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <returns> The presentValue. </returns>
    public bool WriteLine( bool value, string commandFormat )
    {
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
            _ = this.WriteLine( commandFormat, value.GetHashCode() );
        return value;
    }

    #endregion

    #region " write lines "

    /// <summary>   Writes the lines. </summary>
    /// <remarks>   2024-12-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <param name="separator">    (Optional) The separator. </param>
    /// <param name="lineDelay">    (Optional) The line delay. </param>
    public void WriteLines( string dataToWrite, string separator = "\n", TimeSpan? lineDelay = null )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return;
        if ( string.IsNullOrEmpty( separator ) ) throw new ArgumentNullException( separator );
        string[] lines = dataToWrite.Split( separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
        this.WriteLines( lines, lineDelay );
    }

    /// <summary>   Writes the lines. </summary>
    /// <remarks>   2024-12-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="lines">        The lines. </param>
    /// <param name="lineDelay">    (Optional) The line delay. </param>
    public void WriteLines( string[] lines, TimeSpan? lineDelay = null )
    {
        if ( lines is null || (0 == lines.Length) ) return;
        lineDelay ??= this.ReadAfterWriteDelay;
        foreach ( string line in lines )
        {
            string scriptLine = line.Trim();
            if ( !string.IsNullOrWhiteSpace( scriptLine ) )
            {
                _ = this.SyncWriteLine( scriptLine );
                if ( lineDelay > TimeSpan.Zero )
                    _ = SessionBase.AsyncDelay( lineDelay.Value );
            }
        }
    }

    #endregion

    #region " write line escaped "

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface. Replaces Escape
    /// characters and Terminates the data with the <see cref="ReadTerminationCharacter">termination
    /// character</see>.
    /// </summary>
    /// <param name="format"> The format of the data to write. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string WriteEscapedLine( string format, params object[] args )
    {
        if ( string.IsNullOrWhiteSpace( format ) ) throw new ArgumentNullException( nameof( format ) );
        return this.WriteEscapedLine( string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string WriteEscapedLine( string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        return this.WriteLine( dataToWrite.ReplaceCommonEscapeSequences() );
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   The <see cref="ExecuteInfo"/>. </returns>
    public ExecuteInfo WriteEscapedLineElapsed( string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        Stopwatch sw = Stopwatch.StartNew();
        return new ExecuteInfo( dataToWrite, this.WriteEscapedLine( dataToWrite ), sw.Elapsed );
    }

    #endregion

    #region " write line elapsed "

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   The <see cref="ExecuteInfo"/>. </returns>
    public ExecuteInfo WriteLineElapsed( string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        Stopwatch sw = Stopwatch.StartNew();
        return new ExecuteInfo( dataToWrite, this.WriteLine( dataToWrite ), sw.Elapsed );
    }

    /// <summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="format">   The format of the data to write. </param>
    /// <param name="args">     The format arguments. </param>
    /// <returns>   The <see cref="ExecuteInfo"/>. </returns>
    public ExecuteInfo WriteLineElapsed( string format, params object[] args )
    {
        if ( string.IsNullOrWhiteSpace( format ) ) throw new ArgumentNullException( nameof( format ) );
        return this.WriteLineElapsed( string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Writes the presentValue without reading back the presentValue from the device and returns the elapsed time.
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">            The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat">    The command format. </param>
    /// <returns>   A Tuple: (int SentValue, <see cref="WriteInfo{T}"/> WriteInfo ) . </returns>
    public (int SentValue, WriteInfo<int> WriteInfo) WriteLineElapsed( int value, string commandFormat )
    {
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
        {
            ExecuteInfo executeInfo = this.WriteLineElapsed( commandFormat, value );
            return (value, new WriteInfo<int>( value, commandFormat, executeInfo.SentMessage, executeInfo.GetElapsedTimes() ));
        }
        return (value, WriteInfo<int>.Empty);
    }

    /// <summary>
    /// Writes the presentValue without reading back the presentValue from the device and returns the elapsed time.
    /// Synchronously writes ASCII-encoded string data to the device or interface. Converts the
    /// specified string to an ASCII string and appends it to the formatted I/O write buffer. Appends
    /// a newline (0xA) to the formatted I/O write buffer, flushes the buffer, and sends an END with
    /// the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">            The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat">    The command format. </param>
    /// <returns>   A Tuple: ( bool vSentValue, ExecuteInfo ExecuteInfo ). </returns>
    public (bool SentValue, WriteBooleanInfo WriteBooleanInfo) WriteLineElapsed( bool value, string commandFormat )
    {
        return string.IsNullOrWhiteSpace( commandFormat )
            ? (value, WriteBooleanInfo.Empty)
            : (value, new WriteBooleanInfo( value, this.WriteLineElapsed( commandFormat, value.GetHashCode() ) ));
    }

    #endregion

    #region " write line if defined "

    /// <summary>   Write the command if the command is not null or empty. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="command">  The command to execute. </param>
    /// <returns>   A string. </returns>
    public string WriteLineIfDefined( string command )
    {
        return string.IsNullOrWhiteSpace( command )
            ? string.Empty
            : this.WriteLine( command );
    }

    /// <summary>   Write the command if the command is not null or empty. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="commandFormat">    The command format. </param>
    /// <param name="args">             A variable-length parameters list containing arguments. </param>
    /// <returns>   A string. </returns>
    public string WriteLineIfDefined( string commandFormat, params object[] args )
    {
        return string.IsNullOrWhiteSpace( commandFormat )
            ? string.Empty
            : this.WriteLine( string.Format( System.Globalization.CultureInfo.InvariantCulture, commandFormat, args ) );
    }

    /// <summary>   Write the command if defined and time the operation. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="command">  The command to execute. </param>
    /// <returns>   A Tuple. </returns>
    public (string MessageToSend, string SentMessage, TimeSpan Elapsed) WriteLineElapsedIfDefined( string command )
    {
        return string.IsNullOrWhiteSpace( command )
            ? (string.Empty, string.Empty, TimeSpan.Zero)
            : this.WriteLineElapsedThis( command );
    }

    /// <summary>   Write the command if defined and time the operation. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="commandFormat">    The command format. </param>
    /// <param name="args">             A variable-length parameters list containing arguments. </param>
    /// <returns>   A Tuple. </returns>
    public (string MessageToSend, string SentMessage, TimeSpan Elapsed) WriteLineElapsedIfDefined( string commandFormat, params object[] args )
    {
        return string.IsNullOrWhiteSpace( commandFormat )
            ? (string.Empty, string.Empty, TimeSpan.Zero)
            : this.WriteLineElapsedThis( string.Format( System.Globalization.CultureInfo.InvariantCulture, commandFormat, args ) );
    }

    /// <summary>   Write the command if defined and time the operation. </summary>
    /// <remarks>   David, 2021-04-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   A Tuple. </returns>
    private (string MessageToSend, string SentMessage, TimeSpan Elapsed) WriteLineElapsedThis( string dataToWrite )
    {
        Stopwatch sw = Stopwatch.StartNew();
        return (dataToWrite, this.WriteLine( dataToWrite ), sw.Elapsed);
    }

    #endregion

    #region " read "

    /// <summary>
    /// Synchronously reads ASCII-encoded string data until an END indicator or
    /// <see cref="ReadTerminationCharacter">termination character</see>
    /// termination character is reached irrespective of the buffer size.
    /// </summary>
    /// <returns> The string. </returns>
    public string ReadLine()
    {
        return this.ReadFreeLine();
    }

    /// <summary>
    /// Tries to <see cref="ReadLine"/> returning a nullable string if an exception occurred.
    /// </summary>
    /// <returns> The nullable <see cref="string"/> if an exception was thrown. </returns>
    public string? TryReadLine()
    {
        try
        {
            return this.ReadLine();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// <see cref="ReadFiniteLine"/> measuring the readAfterWriteDelay time.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <returns>   <see cref="ReadInfo"/> </returns>
    public ReadInfo ReadElapsed()
    {
        Stopwatch sw = Stopwatch.StartNew();
        string s = this.ReadFreeLine();
        return new ReadInfo( s, sw.Elapsed );
    }

    /// <summary>
    /// Tries to <see cref="ReadElapsed"/> returning a nullable <see cref="ReadInfo"/> if an exception occurred.
    /// </summary>
    /// <returns> The nullable <see cref="ReadInfo"/> if an exception was thrown. </returns>
    public ReadInfo? TryReadElapsed()
    {
        try
        {
            return this.ReadElapsed();
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region " query "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by the <see cref="ReadAfterWriteDelay"/>
    /// and then by a synchronous read.
    /// </summary>
    /// <remarks>   2024-09-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <param name="readAfterWriteDelay">  The timespan to delay reading after writing. </param>
    /// <returns>
    /// The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>.
    /// </returns>
    public string Query( TimeSpan readAfterWriteDelay, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        _ = this.WriteLine( dataToWrite );
        _ = SessionBase.AsyncDelay( readAfterWriteDelay );
        return this.ReadLine();
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by the <see cref="ReadAfterWriteDelay"/>
    /// and then by a synchronous read.
    /// </summary>
    /// <remarks>   2024-08-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>
    /// The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>.
    /// </returns>
    public string Query( string dataToWrite )
    {
        return this.Query( this.ReadAfterWriteDelay, dataToWrite );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="readAfterWriteDelay">  The timespan to delay reading after writing. </param>
    /// <param name="format">               The format of the data to write. </param>
    /// <param name="args">                 The format arguments. </param>
    /// <returns>
    /// The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>.
    /// </returns>
    public string Query( TimeSpan readAfterWriteDelay, string format, params object[] args )
    {
        if ( string.IsNullOrWhiteSpace( format ) ) throw new ArgumentNullException( nameof( format ) );
        _ = this.WriteLine( format, args );
        _ = SessionBase.AsyncDelay( readAfterWriteDelay );
        return this.ReadLine();
    }

    /// <summary>
    /// Tries to query returning a nullable <see cref="string"/> if
    /// failed.
    /// </summary>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns>
    /// The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>.
    /// </returns>
    public string? TryQuery( string dataToWrite )
    {
        return this.TryQuery( this.ReadAfterWriteDelay, dataToWrite );
    }

    /// <summary>   Tries to query returning a nullable <see cref="string"/> if failed. </summary>
    /// <remarks>   2024-09-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="readAfterWriteDelay">  The timespan to delay reading after writing. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <returns>
    /// The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>.
    /// </returns>
    public string? TryQuery( TimeSpan readAfterWriteDelay, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        try
        {
            return this.Query( readAfterWriteDelay, dataToWrite );
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by the <see cref="ReadAfterWriteDelay"/>
    /// and then by a synchronous read.
    /// </summary>
    /// <param name="format"> The format of the data to write. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns>
    /// The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>.
    /// </returns>
    public string Query( string format, params object[] args )
    {
        return this.Query( this.ReadAfterWriteDelay, format, args );
    }

    /// <summary>   Tries to query returning a nullable <see cref="string"/> if failed. </summary>
    /// <remarks>   2024-07-05. </remarks>
    /// <param name="readAfterWriteDelay">  The timespan to delay reading after writing. </param>
    /// <param name="format">               The format of the data to write. </param>
    /// <param name="args">                 The format arguments. </param>
    /// <returns>
    /// The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>.
    /// </returns>
    public string? TryQuery( TimeSpan readAfterWriteDelay, string format, params object[] args )
    {
        try
        {
            return this.Query( readAfterWriteDelay, format, args );
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data. A final read is performed after a
    /// delay.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="readAfterWriteDelay">  The timespan to delay reading after writing. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   <see cref="QueryInfo"/> </returns>
    public QueryInfo QueryElapsed( TimeSpan readAfterWriteDelay, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        Stopwatch sw = Stopwatch.StartNew();

        string sentMessage = this.WriteLine( dataToWrite );
        TimeSpan writeTime = sw.Elapsed;

        TimeSpan readAfterWriteDelayNew = SessionBase.AsyncDelay( readAfterWriteDelay );

        string? receivedMessage = this.ReadLine();
        TimeSpan readTime = sw.Elapsed.Subtract( writeTime );

        return new QueryInfo( dataToWrite, sentMessage, receivedMessage,
                              [ new( ElapsedTimeIdentity.WriteTime, writeTime ),
                                new( ElapsedTimeIdentity.ReadDelay, readAfterWriteDelayNew ),
                                new( ElapsedTimeIdentity.ReadTime, readTime ),
                                new( ElapsedTimeIdentity.QueryTime, sw.Elapsed ) ] );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by the <see cref="ReadAfterWriteDelay"/>
    /// and then by a synchronous read.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   The net write plus read <see cref="QueryInfo"/> </returns>
    public QueryInfo QueryElapsed( string dataToWrite )
    {
        return this.QueryElapsed( this.ReadAfterWriteDelay, dataToWrite );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="format">   The format of the data to write. </param>
    /// <param name="args">     The format arguments. </param>
    /// <returns>   <see cref="QueryInfo"/> </returns>
    public QueryInfo QueryElapsed( string format, params object[] args )
    {
        return this.QueryElapsed( this.ReadAfterWriteDelay, format, args );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="readAfterWriteDelay">  The timespan to delay reading after writing. </param>
    /// <param name="format">       The format of the data to write. </param>
    /// <param name="args">         The format arguments. </param>
    /// <returns>   <see cref="QueryInfo"/> </returns>
    public QueryInfo QueryElapsed( TimeSpan readAfterWriteDelay, string format, params object[] args )
    {
        return this.QueryElapsed( readAfterWriteDelay, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    #endregion

    #region " query trim end "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// </summary>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns>
    /// The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public string QueryTrimTermination( string dataToWrite )
    {
        return this.Query( dataToWrite ).TrimEnd( this.TerminationCharacters() );
    }

    /// <summary>
    /// Queries the device and returns a string save the termination character. Expects terminated
    /// query command.
    /// </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <param name="readAfterWriteDelay">  The timespan to delay reading after writing. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <returns>
    /// The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public string QueryTrimEnd( TimeSpan readAfterWriteDelay, string dataToWrite )
    {
        return this.Query( readAfterWriteDelay, dataToWrite ).TrimEnd( this.TerminationCharacters() );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// </summary>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns>
    /// The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public string QueryTrimEnd( string dataToWrite )
    {
        return this.Query( dataToWrite ).TrimEnd( this.TerminationCharacters() );
    }

    /// <summary>
    /// Queries the device and returns a string save the termination character. Expects terminated
    /// query command.
    /// </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <param name="readAfterWriteDelay">  The timespan to delay reading after writing. </param>
    /// <param name="format">               The format of the data to write. </param>
    /// <param name="args">                 The format arguments. </param>
    /// <returns>
    /// The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public string QueryTrimEnd( TimeSpan readAfterWriteDelay, string format, params object[] args )
    {
        return this.Query( readAfterWriteDelay, format, args ).TrimEnd( this.TerminationCharacters() );
    }

    /// <summary>
    /// Queries the device and returns a string save the termination character. Expects terminated
    /// query command.
    /// </summary>
    /// <param name="format"> The format of the data to write. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns>
    /// The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public string QueryTrimEnd( string format, params object[] args )
    {
        return this.Query( format, args ).TrimEnd( this.TerminationCharacters() );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// </summary>
    /// <param name="readAfterWriteDelay"> The timespan to delay reading after writing. </param>
    /// <param name="format">    The format of the data to write. </param>
    /// <param name="args">      The format arguments. </param>
    /// <returns>
    /// The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public string QueryTrimTermination( TimeSpan readAfterWriteDelay, string format, params object[] args )
    {
        return this.Query( readAfterWriteDelay, format, args ).TrimEnd( this.TerminationCharacters() );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// </summary>
    /// <param name="format"> The format of the data to write. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns>
    /// The <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see> without the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public string QueryTrimTermination( string format, params object[] args )
    {
        return this.Query( format, args ).TrimEnd( this.TerminationCharacters() );
    }

    #endregion

    #region " read trim end "

    /// <summary>
    /// Synchronously reads ASCII-encoded string data. Reads up to the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </summary>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public string ReadLineTrimTermination()
    {
        return this.ReadLine().TrimEnd( this.TerminationCharacters() );
    }

    /// <summary>
    /// Synchronously reads ASCII-encoded string data. Reads up to the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </summary>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public string ReadLineTrimEnd()
    {
        return this.ReadLine().TrimEnd( this.TerminationCharacters() );
    }

    /// <summary> Reads free line trim end. </summary>
    /// <returns> The free line trim end. </returns>
    public string ReadFreeLineTrimEnd()
    {
        return this.ReadFreeLine().TrimEnd( this.TerminationCharacters() );
    }

    /// <summary>
    /// Synchronously reads ASCII-encoded string data. Reads up to the
    /// <see cref="TerminationCharacters">termination character</see>.
    /// </summary>
    /// <returns>
    /// The received message without the carriage return (13) and line feed (10) characters.
    /// </returns>
    public string ReadLineTrimNewLine()
    {
        return this.ReadLine().TrimEnd( [Convert.ToChar( 13 ), Convert.ToChar( 10 )] );
    }

    /// <summary>
    /// Synchronously reads ASCII-encoded string data. Reads up to the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </summary>
    /// <returns> The received message without the line feed (10) characters. </returns>
    public string ReadLineTrimLinefeed()
    {
        return this.ReadLine().TrimEnd( Convert.ToChar( 10 ) );
    }

    #endregion

    #region " read lines "

    /// <summary>
    /// Reads multiple lines from the instrument until data is no longer available.
    /// </summary>
    /// <remarks>   David, 2021-11-15. </remarks>
    /// <param name="pollInterval">     Time to wait between service requests. </param>
    /// <param name="readLineTimeout">  Specifies the time to wait for message available. </param>
    /// <param name="trimEnd">          Specifies a directive to trim the end character from each
    ///                                 line. </param>
    /// <returns>   Data. </returns>
    public string ReadLines( TimeSpan pollInterval, TimeSpan readLineTimeout, bool trimEnd )
    {
        return this.ReadLines( pollInterval, readLineTimeout, false, trimEnd );
    }

    /// <summary>
    /// Reads multiple lines from the instrument until data is no longer available.
    /// </summary>
    /// <remarks>   David, 2021-11-15. </remarks>
    /// <param name="pollInterval"> Time to wait between service requests. </param>
    /// <param name="readLineTimeout">      Specifies the time to wait for message available. </param>
    /// <param name="trimSpaces">   Specifies a directive to trim leading and trailing spaces from
    ///                             each line. This also trims the end character. </param>
    /// <param name="trimEnd">      Specifies a directive to trim the end character from each line. </param>
    /// <returns>   Data. </returns>
    public string ReadLines( TimeSpan pollInterval, TimeSpan readLineTimeout, bool trimSpaces, bool trimEnd )
    {
        System.Text.StringBuilder builder = new();
        Stopwatch sw = Stopwatch.StartNew();
        bool timedOut = false;
        bool errorOccurred = false;
        ServiceRequests statusByte = ServiceRequests.None;
        while ( !(timedOut || errorOccurred) )
        {
            // allow message available time to materialize
            _ = SessionBase.AsyncDelay( this.StatusReadDelay );
            statusByte = this.ReadStatusByte();
            errorOccurred = this.IsErrorBitSet( statusByte );

            // wait for message available (was measurement) events
            while ( !(this.IsMessageAvailableBitSet( statusByte ) || timedOut) )
            {
                timedOut = sw.Elapsed > readLineTimeout;
                _ = SessionBase.AsyncDelay( pollInterval );
                statusByte = this.ReadStatusByte();
            }

            if ( this.IsMessageAvailableBitSet( statusByte ) )
            {
                timedOut = false;
                sw.Restart();
                if ( trimSpaces )
                {
                    string? s1 = this.ReadLine();
                    if ( s1 is not null ) _ = builder.AppendLine( s1.Trim() );
                }
                else if ( trimEnd )
                {
                    string? s1 = this.ReadLineTrimEnd();
                    if ( s1 is not null ) _ = builder.AppendLine( s1 );
                }
                else
                {
                    string? s1 = this.ReadLine();
                    if ( s1 is not null ) _ = builder.AppendLine( s1 );
                }
            }
        }

        this.ThrowDeviceExceptionIfError( statusByte );

        return builder.ToString();
    }

    /// <summary> Reads multiple lines from the instrument until awaitStatusTimeout. </summary>
    /// <param name="timeout">        Specifies the time in millisecond to wait for message available. </param>
    /// <param name="trimSpaces">     Specifies a directive to trim leading and trailing spaces from
    /// each line. </param>
    /// <param name="expectedLength"> Specifies the amount of data expected without trimming. </param>
    /// <returns> Data. </returns>
    public string ReadLines( TimeSpan timeout, bool trimSpaces, int expectedLength )
    {
        try
        {
            System.Text.StringBuilder builder = new();
            this.StoreCommunicationTimeout( timeout );
            int currentLength = 0;
            while ( currentLength < expectedLength )
            {
                string? buffer = this.ReadLine();
                expectedLength += buffer?.Length ?? 0;
                if ( buffer is not null )
                    _ = trimSpaces ? builder.AppendLine( buffer.Trim() ) : builder.AppendLine( buffer );
            }

            return builder.ToString();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.RestoreCommunicationTimeout();
        }
    }

    #endregion

    #endregion

    #region " timeout management "

    /// <summary> Gets or sets the reference to the stack of communication timeouts. </summary>
    /// <value> The timeouts. </value>
    protected Stack<TimeSpan> CommunicationTimeouts { get; private set; }

    private TimeSpan _timeoutCandidate;

    /// <summary> Gets or sets the milliseconds awaitStatusTimeout candidate. </summary>
    /// <value> The milliseconds awaitStatusTimeout candidate. </value>
    public TimeSpan TimeoutCandidate
    {
        get => this._timeoutCandidate;
        set => _ = base.SetProperty( ref this._timeoutCandidate, value );
    }

    /// <summary> Restores the last awaitStatusTimeout from the stack. </summary>
    public void RestoreCommunicationTimeout()
    {
        if ( this.CommunicationTimeouts.Any() )
            this.CommunicationTimeout = this.CommunicationTimeouts.Pop();

        this.TimeoutCandidate = this.CommunicationTimeout;
    }

    /// <summary> Saves the current awaitStatusTimeout and sets a new setting awaitStatusTimeout. </summary>
    /// <param name="timeout"> Specifies the new awaitStatusTimeout. </param>
    public void StoreCommunicationTimeout( TimeSpan timeout )
    {
        this.CommunicationTimeouts.Push( this.CommunicationTimeout );
        this.CommunicationTimeout = timeout;
        this.TimeoutCandidate = timeout;
    }

    #endregion

    #region " status prompt "

    /// <summary> The status prompt. </summary>
    private string? _statusPrompt;

    /// <summary> Gets or sets a prompt describing the Status. </summary>
    /// <value> A prompt describing the Status. </value>
    public string? StatusPrompt
    {
        get => this._statusPrompt;
        set => _ = base.SetProperty( ref this._statusPrompt, value );
    }

    #endregion
}
/// <summary>   A bit-field of flags for specifying message notification modes. </summary>
/// <remarks>   David, 2021-08-16. </remarks>
[Flags]
public enum MessageNotificationModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [Description( "No notification" )]
    None = 0,

    /// <summary> An enum constant representing the Sent option. </summary>
    [Description( "notifies on sent messages" )]
    Sent = 1,

    /// <summary> An enum constant representing the Received option. </summary>
    [Description( "notifies on received messages" )]
    Received = 2,

    /// <summary> An enum constant representing the Both option. </summary>
    [Description( "notifies on sent and received messages" )]
    Both = 3

}
