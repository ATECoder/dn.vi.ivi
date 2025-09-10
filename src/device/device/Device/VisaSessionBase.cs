using System.Diagnostics;
using cc.isr.VI.ExceptionExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by session opening classes. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-24, . from device base. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class VisaSessionBase : IDisposable
{
    #region " construction and cleanup "

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="session"> The session. </param>
    protected VisaSessionBase( SessionBase session ) : base() => this.NewThis( session, false );

    /// <summary> Specialized constructor for use only by derived class. </summary>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    protected VisaSessionBase( StatusSubsystemBase statusSubsystem ) : base()
    {
        // initialize the collection of presettable subsystems with the status subsystem.
        this.Subsystems = [statusSubsystem];
        this.StatusSubsystemBase = statusSubsystem;
        this.NewThis( StatusSubsystemBase.Validated( statusSubsystem ).Session, false );
    }

    /// <summary> Initializes a new instance of the <see cref="VisaSessionBase" /> class. </summary>
    protected VisaSessionBase() : base()
    {
        this.Subsystems = [];
        this.NewThis( SessionFactory.Instance.Factory.Session(), true );
    }

    /// <summary> Initializes a new instance of the <see cref="VisaSessionBase" /> class. </summary>
    /// <param name="session">        A reference to a <see cref="SessionBase">message based
    /// session</see>. </param>
    /// <param name="isSessionOwner"> true if this object is session owner. </param>
    private void NewThis( SessionBase session, bool isSessionOwner )
    {
        this.ElapsedTimeStopwatch = new Stopwatch();
        if ( session is null )
        {
            isSessionOwner = true;
            this.Assign_Session( SessionFactory.Instance.Factory.Session(), isSessionOwner );
        }
        else
        {
            this.Assign_Session( session, isSessionOwner );
        }

        if ( this.Session is null )
            throw new InvalidOperationException( "Failed creating a new session" );

        if ( string.IsNullOrWhiteSpace( this.Session.ResourcesFilter ) )
            this.Session.ResourcesFilter = SessionFactory.Instance.Factory.ResourcesProvider().ResourceFinder!.BuildMinimalResourcesFilter();

        this.ApplyDefaultSyntax();
        base.ResourceClosedCaption = "<closed>";
        base.CandidateResourceModel = string.Empty;

        this._sessionFactory = new SessionFactory()
        {
            Searchable = true,
            PingFilterEnabled = false,
            ResourcesFilter = this.Session.ResourcesFilter!
        };
        this.PollTimerInternal = new System.Timers.Timer() { Enabled = false, AutoReset = true, Interval = 1000d };
        this._pollEnabled = new Std.Concurrent.ConcurrentToken<bool>() { Value = false };
    }

    /// <summary>   Applies the default syntax. </summary>
    /// <remarks>   2024-09-16. </remarks>
    protected abstract void ApplyDefaultSyntax();

    #region " i disposable support "

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

    /// <summary> Gets the disposed status. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The is disposed. </value>
    public bool IsDisposed { get; private set; }

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
                this._pollEnabled?.Dispose();
                this._pollEnabled = null;
                this.PollTimerInternal?.Dispose();
                this.PollTimerInternal = null;

                // release both managed and unmanaged resources
                this.Session?.DisableServiceRequestEventHandler();
                this.RemoveServiceRequestEventHandler();
                this.RemoveServiceRequestedEventHandlers();
                this.RemoveOpeningEventHandlers();
                this.RemoveOpenedEventHandlers();
                this.RemoveClosingEventHandlers();
                this.RemoveClosedEventHandlers();
                this.RemoveInitializingEventHandlers();
                this.RemoveInitializedEventHandlers();
                if ( this.IsSessionOwner )
                {
                    try
                    {
                        this.CloseSession();
                        this.Session = null;
                        // this.Assign_Session( null, true );
                    }
                    catch ( ObjectDisposedException ex )
                    {
                        Debug.Assert( !Debugger.IsAttached, ex.BuildMessage() );
                    }
                }

                this.StatusSubsystemBase = null;
                this.Subsystems.Clear();
            }
            else
            {
                // release only unmanaged resources.
            }
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, ex.BuildMessage() );
        }
        finally
        {
            this.IsDisposed = true;
        }
    }

    /// <summary> Finalizes this object. </summary>
    /// <remarks>
    /// David, 2015-11-21: Override because Dispose(disposing As Boolean) above has code to free
    /// unmanaged resources.
    /// </remarks>
    ~VisaSessionBase()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( false );
    }

    #endregion

    #endregion

    #region " i presettable "

    /// <summary> Clears the interface if interface clear is supported for this resource. </summary>
    public void ClearInterface()
    {
        if ( this.Session is not null && this.Session.SupportsClearInterface )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} clearing interface" );
            this.Session.ClearInterface();

            // this will publish a report if device errors occurred
            _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
            this.Session.ThrowDeviceExceptionIfError( this.Session.ReadStatusByte() );
        }
    }

    /// <summary>   Attempts to clear active state. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <returns>   (Success: True if success; false otherwise, Details) </returns>
    public override (bool success, string Details) TryClearActiveState()
    {
        try
        {
            this.ClearActiveState();
            return (true, string.Empty);
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            return (false, ex.BuildMessage());
        }
    }

    /// <summary>
    /// Issues <see cref="VI.Pith.SessionBase.ClearActiveState()">Selective device clear (SDC)</see>. and
    /// applies default settings and clears the Device.
    /// </summary>
    public override void ClearActiveState()
    {
        if ( this.Session is not null )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} clearing active state (SDC)" );
            this.Session.ClearActiveState( this.Session.DeviceClearRefractoryPeriod );

            _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
            this.Session.ThrowDeviceExceptionIfError( this.Session.ReadStatusByte() );
        }
    }

    /// <summary>
    /// Defines the clear execution state (CLS) by setting system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    public virtual void DefineClearExecutionState()
    {
        if ( this.Subsystems is not null && this.Session is not null )
        {
            if ( this.SubsystemSupportMode == SubsystemSupportMode.StatusOnly )
            {
                if ( this.StatusSubsystemBase is null )
                    throw new InvalidOperationException( $"{nameof( this.StatusSubsystemBase )} must not be null if {nameof( this.SubsystemSupportMode )} is {SubsystemSupportMode.StatusOnly}." );
                this.StatusSubsystemBase.DefineClearExecutionState();
            }
            else if ( this.SubsystemSupportMode == SubsystemSupportMode.Full )
            {
                if ( this.Subsystems is not null && this.Session is not null )
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} defining system clear execution state" );
                    this.Subsystems.DefineClearExecutionState();
                }
            }
            if ( this.Session is not null )
            {
                _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
                this.Session.ThrowDeviceExceptionIfError( this.Session.ReadStatusByte() );
            }
        }
    }

    /// <summary>
    /// Clears the queues and resets all registers to zero. Sets system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    /// <remarks> *CLS. </remarks>
    public void ClearExecutionState()
    {
        if ( this.Subsystems is not null && this.Session is not null )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} clearing execution state" );
            this.Session!.ClearExecutionState();
            this.DefineClearExecutionState();
        }
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Customizes the reset state. </remarks>
    public void InitKnownState()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} initializing known state" );
        if ( this.SubsystemSupportMode == SubsystemSupportMode.StatusOnly )
        {
            if ( this.StatusSubsystemBase is null )
                throw new InvalidOperationException( $"{nameof( this.StatusSubsystemBase )} must not be null if {nameof( this.SubsystemSupportMode )} is {SubsystemSupportMode.StatusOnly}." );
            this.StatusSubsystemBase.InitKnownState();
        }
        else if ( this.SubsystemSupportMode == SubsystemSupportMode.Full )
        {
            if ( this.Session is not null && this.Subsystems is not null && this.StatusSubsystemBase is not null )
            {
                this.Subsystems.InitKnownState();
                this.IsInitialized = true;

                _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
                this.Session.ThrowDeviceExceptionIfError( this.Session.ReadStatusByte() );
            }
        }
        this.IsInitialized = true;
    }

    /// <summary> Sets the system to its Preset known state. </summary>
    public void PresetKnownState()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} initializing known state" );
        if ( this.SubsystemSupportMode == SubsystemSupportMode.StatusOnly )
        {
            if ( this.StatusSubsystemBase is null )
                throw new InvalidOperationException( $"{nameof( this.StatusSubsystemBase )} must not be null if {nameof( this.SubsystemSupportMode )} is {SubsystemSupportMode.StatusOnly}." );
            this.StatusSubsystemBase.PresetKnownState();
        }
        else if ( this.SubsystemSupportMode == SubsystemSupportMode.Full )
        {
            if ( this.Subsystems is not null && this.Session is not null )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} presetting system known state" );
                this.Subsystems.PresetKnownState();

                _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
                this.Session.ThrowDeviceExceptionIfError( this.Session.ReadStatusByte() );
            }
        }
    }

    /// <summary> Defines the system and subsystems reset (RST) (default) known state. </summary>
    public virtual void DefineResetKnownState()
    {
        if ( this.SubsystemSupportMode == SubsystemSupportMode.StatusOnly )
        {
            if ( this.StatusSubsystemBase is null )
                throw new InvalidOperationException( $"{nameof( this.StatusSubsystemBase )} must not be null if {nameof( this.SubsystemSupportMode )} is {SubsystemSupportMode.StatusOnly}." );
            this.StatusSubsystemBase.DefineKnownResetState();
        }
        else if ( this.SubsystemSupportMode == SubsystemSupportMode.Full )
        {
            if ( this.Subsystems is not null && this.Session is not null )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} resetting subsystems known state" );
                this.Subsystems.DefineKnownResetState();

                _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
                this.Session.ThrowDeviceExceptionIfError( this.Session.ReadStatusByte() );
            }
        }

    }

    /// <summary>
    /// Resets the device to its known reset (RST) (default) state and defines the relevant system
    /// and subsystems state values.
    /// </summary>
    /// <remarks> *RST. </remarks>
    public virtual void ResetKnownState()
    {
        if ( this.Subsystems is not null && this.Session is not null )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} resetting known state" );
            this.Session.ResetKnownState();
            this.DefineResetKnownState();
        }
    }

    /// <summary>
    /// Resets, clears and initializes the device. Starts with issuing a selective-device-clear,
    /// reset (RST), Clear Status (CLS, and clear error queue) and initialize.
    /// </summary>
    public void ResetClearInit()
    {
        // issues selective device clear.
        this.ClearActiveState();

        // reset device
        this.ResetKnownState();

        // Clear the device Status and set more defaults
        this.ClearExecutionState();

        // initialize the device must be done after clear (5892)
        this.InitKnownState();
    }

    /// <summary>
    /// Resets, clears and initializes the device. Starts with issuing a selective-device-clear,
    /// reset (RST), Clear Status (CLS, and clear error queue) and initialize.
    /// </summary>
    /// <param name="timeout"> The timeout to use. This allows using a longer timeout than the
    /// minimal timeout set for the session. Typically, a source meter
    /// requires a 5000 milliseconds timeout. </param>
    public void ResetClearInit( TimeSpan timeout )
    {
        try
        {
            this.Session?.StoreCommunicationTimeout( timeout );
            this.ResetClearInit();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.Session?.RestoreCommunicationTimeout();
        }
    }

    #endregion

    #region " resource name info "

    /// <summary>
    /// Checks if the specified resource name exists. Use for checking if the instrument is turned on.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resourceName">    Name of the resource. </param>
    /// <param name="resourcesFilter"> The resources search pattern. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public static bool Find( string resourceName, string resourcesFilter )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) ) throw new ArgumentNullException( nameof( resourceName ) );
        using ResourcesProviderBase rm = SessionFactory.Instance.Factory.ResourcesProvider();
        return string.IsNullOrWhiteSpace( resourcesFilter ) ? rm.FindResources().ToArray().Contains( resourceName ) : rm.FindResources( resourcesFilter ).ToArray().Contains( resourceName );
    }

    /// <summary>
    /// Checks if the candidate resource name exists thus checking if the instrument is turned on.
    /// </summary>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool ValidateCandidateResourceName()
    {
        return this.IsDeviceOpen && this.Session!.ValidateCandidateResourceName( SessionFactory.Instance.Factory.ResourcesProvider() );
    }

    #endregion

    #region " session "

    /// <summary> true if this object is session owner. </summary>
    /// <value> The is session owner. </value>
    private bool IsSessionOwner { get; set; }

    /// <summary> Gets the session. </summary>
    /// <value> The session. </value>
    public SessionBase? Session { get; private set; }

    /// <summary>
    /// Constructor-safe session assignment. If session owner and session exists, must close first.
    /// </summary>
    /// <param name="session">        The value. </param>
    /// <param name="isSessionOwner"> true if this object is session owner. </param>
    [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
    private void Assign_Session( SessionBase session, bool isSessionOwner )
    {
        if ( this.Session is not null )
        {
            this.Session.PropertyChanged -= this.SessionPropertyChanged;
            VI.Pith.SessionBase.DoEventsAction?.Invoke();
            if ( this.IsSessionOwner )
            {
                this.Session.Dispose();
                // release the session
                // Trying to null the session raises an ObjectDisposedException
                // if session service request handler was not released.
                this.Session = null;
            }
        }

        this.IsSessionOwner = isSessionOwner;
        this.Session = session;
        if ( this.Session is not null )
        {
            this.Session.PropertyChanged += this.SessionPropertyChanged;
        }
    }

    /// <summary> Handles the session property changed action. </summary>
    /// <param name="sender">       Source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    protected virtual void HandlePropertyChanged( SessionBase sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( SessionBase.Enabled ):
                {
                    this.NotifyPropertyChanged( nameof( this.Enabled ) );
                    VI.Pith.SessionBase.DoEventsAction?.Invoke();
                    break;
                }

            case nameof( SessionBase.CandidateResourceName ):
                {
                    this.CandidateResourceName = sender.CandidateResourceName!;
                    break;
                }

            case nameof( SessionBase.CandidateResourceModel ):
                {
                    this.CandidateResourceModel = sender.CandidateResourceModel!;
                    break;
                }

            case nameof( SessionBase.OpenResourceModel ):
                {
                    this.OpenResourceModel = sender.OpenResourceModel!;
                    break;
                }

            case nameof( SessionBase.OpenResourceName ):
                {
                    this.OpenResourceName = sender.OpenResourceName!;
                    break;
                }

            case nameof( SessionBase.ResourceModelCaption ):
                {
                    this.ResourceModelCaption = sender.ResourceModelCaption!;
                    break;
                }

            case nameof( SessionBase.ResourceNameCaption ):
                {
                    this.ResourceNameCaption = sender.ResourceNameCaption!;
                    break;
                }

            case nameof( SessionBase.ServiceRequestEventEnabled ):
                {
                    if ( sender.ServiceRequestEventEnabled )
                        this.PollEnabled = false;
                    break;
                }

            case nameof( SessionBase.ResourcesFilter ):
                {
                    this.ResourcesFilter = sender.ResourcesFilter!;
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Handles the Session property changed event. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void SessionPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;

        string activity = $"handling {nameof( Pith.SessionBase )}.{e.PropertyName} change";
        try
        {
            if ( sender is Pith.SessionBase s ) this.HandlePropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary>
    /// Gets or sets the Enabled sentinel of the device. A device is enabled when hardware can be
    /// used.
    /// </summary>
    /// <value> <c>true</c> if hardware device is enabled; <c>false</c> otherwise. </value>
    public virtual bool Enabled
    {
        get => this.Session?.Enabled ?? false; set => this.Session?.Enabled = value;
    }

    #endregion

    #region " service request management "

    /// <summary> Gets the service request enabled bitmask. </summary>
    /// <value> The service request enabled bitmask. </value>
    public ServiceRequests ServiceRequestEnableBitmask => this.IsDeviceOpen
        ? this.Session!.ServiceRequestEnableBitmask.GetValueOrDefault( ServiceRequests.None )
        : ServiceRequests.None;

    /// <summary> Gets the session service request event enabled sentinel. </summary>
    /// <value> <c>true</c> if session service request event is enabled. </value>
    public bool ServiceRequestEventEnabled => this.IsSessionOpen && this.Session!.ServiceRequestEventEnabled;

    /// <summary>
    /// Gets or sets an indication if an handler was assigned to the service request event.
    /// </summary>
    /// <value> <c>true</c> if a handler was assigned to the service request event. </value>
    public bool ServiceRequestHandlerAssigned
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Registers the device service request handler. </summary>
    /// <remarks>
    /// The Session service request handler must be registered and enabled before registering the
    /// device event handler.
    /// </remarks>
    public void AddServiceRequestEventHandler()
    {
        if ( !this.ServiceRequestHandlerAssigned )
        {
            this.Session!.ServiceRequested += this.SessionBaseServiceRequested;
            this.ServiceRequestHandlerAssigned = true;
        }
    }

    /// <summary> Removes the device service request event handler. </summary>
    public void RemoveServiceRequestEventHandler()
    {
        if ( this.ServiceRequestHandlerAssigned && this.Session is not null )
            this.Session.ServiceRequested -= this.SessionBaseServiceRequested;
        this.ServiceRequestHandlerAssigned = false;
    }

    #endregion

    #region " subsystems "

    /// <summary> Enumerates the Presettable subsystems. </summary>
    /// <value> The subsystems. </value>
    public PresettableCollection Subsystems { get; private set; } = [];

    #endregion

    #region " status subsystem base "

    /// <summary> Gets or sets the Status SubsystemBase. </summary>
    /// <value> The Status SubsystemBase. </value>
    public StatusSubsystemBase? StatusSubsystemBase { get; private set; }

    #endregion
}
