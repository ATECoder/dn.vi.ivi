using System.ComponentModel;
using System.Diagnostics;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary>   A legacy device. </summary>
/// <remarks>   2024-11-07. </remarks>
public partial class LegacyDevice : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDevice
{
    #region " construction and cleanup "

    private readonly Type _settingAssemblyMemberType;
    private readonly string _settingsFileSuffix;

    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="settingAssemblyMemberType">    Type of a member that is defined in the assembly
    ///                                             where the settings file for the meter is stored. </param>
    /// <param name="settingsFileSuffix">           (Optional) [".Driver"] The settings file
    ///                                             suffix, e.g., which replaces suffix in this file name
    ///                                             cc.isr.VI.Tsp.K2600.Ttm.MSTest.suffix.json. </param>
    public LegacyDevice( Type settingAssemblyMemberType, string settingsFileSuffix = ".Driver" ) : base()
    {
        this._settingsFileSuffix = settingsFileSuffix;
        this._settingAssemblyMemberType = settingAssemblyMemberType;
        this.Meter = new();
        if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( this.Meter )} is null." );

        // read settings
        cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.ReadSettings( settingAssemblyMemberType, settingsFileSuffix );
        if ( !cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings.Exists )
            throw new InvalidOperationException( $"Failed reading the thermal transient settings {nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings )}" );

        // the meter configuration is instantiated upon meter instantiation.
        if ( this.Meter.ConfigInfo is null ) throw new InvalidOperationException( $"{nameof( this.Meter )}.{nameof( this.Meter.ConfigInfo )} is null." );
        if ( this.Meter.ConfigInfo.InitialResistance is null ) throw new InvalidOperationException( $"{nameof( this.Meter )}.{nameof( this.Meter.ConfigInfo )}.{nameof( this.Meter.ConfigInfo.InitialResistance )} is null." );
        if ( this.Meter.ConfigInfo.FinalResistance is null ) throw new InvalidOperationException( $"{nameof( this.Meter )}.{nameof( this.Meter.ConfigInfo )}.{nameof( this.Meter.ConfigInfo.FinalResistance )} is null." );
        if ( this.Meter.ConfigInfo.ShuntResistance is null ) throw new InvalidOperationException( $"{nameof( this.Meter )}.{nameof( this.Meter.ConfigInfo )}.{nameof( this.Meter.ConfigInfo.ShuntResistance )} is null." );
        if ( this.Meter.ConfigInfo.ThermalTransient is null ) throw new InvalidOperationException( $"{nameof( this.Meter )}.{nameof( this.Meter.ConfigInfo )}.{nameof( this.Meter.ConfigInfo.ThermalTransient )} is null." );

        this.InitialResistance = new ColdResistance( "_G.ttm.ir" );
        this.FinalResistance = new ColdResistance( "_G.ttm.fr" );
        this.ThermalTransient = new ThermalTransient();
        this.ColdResistanceConfig = new ColdResistanceConfig( this.Meter.ConfigInfo.InitialResistance, this.Meter.ConfigInfo.FinalResistance );
        this.ThermalTransientConfig = new ThermalTransientConfig( this.Meter.ConfigInfo.ThermalTransient );

        this.LastStatusReading = string.Empty;
        this.LastReading = string.Empty;
        this.LastOutcomeReading = string.Empty;
        this.LastOkayReading = string.Empty;
        this.LastOrphanMessages = string.Empty;
        this.TriggerCycleCompleteMessage = string.Empty;
        this.InitializeKnownState();

        // the legacy device defaults to SMUA
        this.SourceChannel = "smua";

        // set initial values out of range so that first apply will set them to the correct defaults.
        this.ColdResistanceCurrentLevel = 0f; // 0.01
        this.ColdResistanceVoltageLimit = 0f; // 0.1
        this.ColdResistanceHighLimit = 0f;
        this.ColdResistanceLowLimit = 0f;
        this.ThermalTransientCurrentLevel = 0f; // 0.27
        this.ThermalTransientVoltageChange = 0f; // 0.099
        this.ThermalTransientHighLimit = 0f;
        this.ThermalTransientLowLimit = 0f;
        this.PostTransientDelay = 0; // 0.5
    }

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
    /// <value> The is disposed. </value>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    ///                          release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected virtual void Dispose( bool disposing )
    {
        if ( this.IsDisposed )
            return;
        try
        {
            if ( disposing )
            {
                try
                {
                    if ( this.Meter.TspDevice is not null && this.Meter.TspDevice.IsDeviceOpen ) { this.Meter.TspDevice.CloseSession(); }
                    this.Meter.Dispose();
                }
                catch ( Exception ex )
                {
                    Debug.Assert( !Debugger.IsAttached, ex.ToString() );
                }
            }
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, ex.ToString() );
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
    ~LegacyDevice()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( false );
    }

    #endregion

    #endregion

    #region " notify property change implementation "

    /// <summary>   Notifies a property changed. </summary>
    /// <remarks>   David, 2021-02-01. </remarks>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected void NotifyPropertyChanged( [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
    {
        this.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
    }

    /// <summary>   Removes the property changed event handlers. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    protected void RemovePropertyChangedEventHandlers()
    {
        MulticastDelegate? event_delegate = ( MulticastDelegate ) this.GetType().GetField( nameof( this.PropertyChanged ),
                                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                                        | System.Reflection.BindingFlags.GetField ).GetValue( this );
        Delegate[]? delegates = event_delegate.GetInvocationList();
        if ( delegates is not null )
        {
            foreach ( Delegate? item in delegates )
            {
                this.PropertyChanged -= ( PropertyChangedEventHandler ) item;
            }
        }
    }

    #endregion

    #region " meter "

    /// <summary>   (Immutable) the meter. </summary>
    /// <value> The meter. </value>
    public Meter Meter { get; }

    /// <summary>   Gets or sets source channel. </summary>
    /// <value> The source channel. </value>
    public string SourceChannel { get; set; } = "smua";

    #endregion

    #region " connect "

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    public string ResourceName { get; set; } = string.Empty;

    /// <summary>   Connects to the thermal transient device. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   <c>True</c> if the device connected. </returns>
    public bool Connect( string resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) ) throw new ArgumentNullException( nameof( resourceName ) );
        if ( this.Meter.TspDevice is null ) throw new InvalidOperationException( $"{nameof( Ttm.Meter.TspDevice )} is null." );
        this.ResourceName = resourceName;
        this.Meter.TspDevice.OpenSession( resourceName, "260x" );
        return this.Meter.IsDeviceOpen;
    }

    /// <summary>   Gets the sentinel indicating if the thermal transient meter is connected. </summary>
    /// <value> The is connected. </value>
    public bool IsConnected => this.Meter.IsSessionOpen;

    /// <summary> Aborts execution, resets the device to known state and clears queues and register. </summary>
    /// <remarks> Issues <see cref="SessionBase.Clear">clear active
    /// state</see>, *RST, and *CLS Typically implements SDC, RST, CLS and clearing error queue. </remarks>
    public bool ResetAndClear()
    {
        if ( !this.IsConnected ) return false;
        K2600Device device = this.Meter!.TspDevice!;
        device.ClearActiveState();
        this.Meter?.ResetKnownState();
        this.Meter?.ClearExecutionState();
        device.Session?.QueryAndThrowIfOperationIncomplete();
        return true;
    }

    /// <summary> Disconnects from the thermal transient meter. </summary>
    /// <returns> <c>True</c> if the device  disconnected. </returns>
    public bool Disconnect()
    {
        string synopsis = "Disconnecting";
        try
        {
            this.OnMessageAvailable( TraceEventType.Verbose, synopsis, "Disconnecting" );
            if ( this.Meter.TspDevice is not null && this.Meter.TspDevice.IsDeviceOpen ) { this.Meter.TspDevice.CloseSession(); }
            this.Meter.Dispose();
            return !this.IsConnected;
        }
        catch ( Exception ex )
        {
            synopsis = "Exception occurred Disconnecting to the device";
            this.OnExceptionAvailable( ex, synopsis );
            return !this.IsConnected;
        }
    }

    #endregion

    #region " Trigger "

    /// <summary>   Sends a signal to abort the triggered measurement. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool AbortMeasurement()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        // allow time for the measurement to terminate.
        if ( this.Meter!.IsAwaitingTrigger )
        {
            this.Meter!.IsAwaitingTrigger = false;

            session.SetLastAction( "asserting trigger" );

            // send the trigger command
            session.AssertTrigger();

            // allow time for the measurement to terminate.
            _ = SessionBase.AsyncDelay( TimeSpan.FromSeconds( 0.4 + this.PostTransientDelay ) );

            // clear the complete message from the output buffer
            _ = this.FlushRead();
        }
        // this.Meter.AbortTriggerSequenceIf();
        return true;
    }

    /// <summary>   Gets or sets the measurement completed reply. </summary>
    /// <value> The measurement completed reply. </value>
    public string MeasurementCompletedReply { get; set; } = "OPC";

    /// <summary>
    /// Primes the instrument to wait for a trigger. This clears the digital outputs and loops until
    /// trigger or <see cref="SessionBase.AssertTrigger"/> command or external *TRG command.
    /// </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="lockLocal">        (Optional) When true, displays the title and locks the local
    ///                                 key. </param>
    /// <param name="onCompleteReply">  (Optional) The operation completion reply. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public bool PrepareForTrigger( bool lockLocal = false, string onCompleteReply = "OPC" )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        this.MeasurementCompletedReply = onCompleteReply;

        _ = this.EnableWaitComplete();

        this.Meter!.IsAwaitingTrigger = true;

        session.SetLastAction( "waiting for trigger" );
        _ = session.WriteLine( "prepareForTrigger({0},'{1}') waitcomplete() ",
                lockLocal ? cc.isr.VI.Syntax.Tsp.Lua.TrueValue : cc.isr.VI.Syntax.Tsp.Lua.FalseValue, onCompleteReply );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        return true;
    }

    /// <summary> Reads the status byte and returns true if the message available bit is set. </summary>
    public bool IsMeasurementCompleted()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;
        bool completed = session.IsMessageAvailableBitSet( session.ReadStatusByte() );
        this.Meter!.IsAwaitingTrigger = !completed;
        return completed;
    }

    /// <summary>   Clears the output buffer. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <returns>
    /// A string containing any orphan messages that were left unread in the instrument output buffer.
    /// </returns>
    public string FlushRead()
    {
        if ( !this.IsConnected ) return string.Empty;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;
        this.OnMessageAvailable( TraceEventType.Verbose, "Clearing Output Buffer", "Instrument '{0}' clearing output buffer", this.ResourceName );
        return session.ReadLines( session.StatusReadDelay, TimeSpan.FromMilliseconds( 100 ), false );
        // StringBuilder builder = new();
        // while ( session.IsMessageAvailableBitSet( session.ReadStatusByte() ) )
        // {
        //     _ = builder.AppendLine( session.ReadLine() );
        //     _ = Pith.SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        // }
        // return builder.ToString();
    }

    /// <summary>   Gets or sets the trigger cycle complete message. </summary>
    /// <value> The trigger cycle complete message. </value>
    public string TriggerCycleCompleteMessage { get; private set; }

    /// <summary>   Read the measurements from the instrument. </summary>
    /// <remarks>   2024-11-15. </remarks>
    /// <param name="readTriggerCycleReply"> (Optional) (true) True to read trigger cycle reply. </param>
    /// <returns>   The measurements. </returns>
    public bool ReadMeasurements( bool readTriggerCycleReply = true )
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;
        if ( readTriggerCycleReply )
        {
            this.OnMessageAvailable( TraceEventType.Verbose, "Reading trigger cycle complete message",
                "Instrument '{0}' reading trigger cycle completion message", this.ResourceName );
            this.Meter!.IsAwaitingTrigger = false;
            this.TriggerCycleCompleteMessage = session.ReadLineTrimEnd();
            if ( string.IsNullOrWhiteSpace( this.TriggerCycleCompleteMessage ) )
                this.OnMessageAvailable( TraceEventType.Verbose, "Unexpected empty trigger cycle reply message upon end of measurement",
                    "Instrument '{0}' end of trigger cycle message is empty", this.ResourceName );
            else if ( string.Equals( this.MeasurementCompletedReply, this.TriggerCycleCompleteMessage, StringComparison.Ordinal ) )
                this.OnMessageAvailable( TraceEventType.Verbose, "Received end of measurement",
                    "Instrument '{0}' sent this trigger cycle complete message: {1}", this.ResourceName, this.TriggerCycleCompleteMessage );
            else
                this.OnMessageAvailable( TraceEventType.Verbose, "Received unexpected end of measurement message",
                    "Instrument '{0}' trigger cycle complete message returned {1} instead of {2}", this.ResourceName, this.TriggerCycleCompleteMessage, this.MeasurementCompletedReply );
        }
        this.LastOrphanMessages = this.FlushRead();
        if ( !string.IsNullOrWhiteSpace( this.LastOrphanMessages ) )
            this.OnMessageAvailable( TraceEventType.Warning, "Found orphan messages",
                "Instrument '{0}' reading measurements found orphan messages:/n{0}", this.ResourceName, this.LastOrphanMessages );
        this.OnMessageAvailable( TraceEventType.Verbose, "Reading Measurements", "Instrument '{0}' reading measurements", this.ResourceName );
        _ = this.ReadInitialResistance( this.InitialResistance );
        _ = this.ReadFinalResistance( this.FinalResistance );
        return this.ReadThermalTransient( this.ThermalTransient );
    }

    #endregion

    #region " i device: message available "

    /// <summary> 
    /// Sends a message to the client.
    /// </summary>
    public event EventHandler<MessageEventArgs>? MessageAvailable;

    /// <summary> Raises the message Available event. </summary>
    /// <param name="e"> Specifies the
    /// <see cref="MessageEventArgs">message event arguments</see>. </param>
    private void OnMessageAvailable( MessageEventArgs e )
    {
        if ( this.Synchronizer is not null && this.Synchronizer.InvokeRequired )
        {
            _ = (this.Synchronizer?.Invoke( new Action<MessageEventArgs>( this.OnMessageAvailable ), [e] ));
        }
        else
        {
            EventHandler<MessageEventArgs>? evt = this.MessageAvailable;
            evt?.Invoke( this, e );
        }
    }

    /// <summary> Raises the message Available event. </summary>
    /// <param name="broadcastLevel"> Specifies the event arguments message
    /// <see cref="System.Diagnostics.TraceEventType">broadcast level</see>. </param>
    /// <param name="traceLevel">     Specifies the event arguments message
    /// <see cref="System.Diagnostics.TraceEventType">trace level</see>. </param>
    /// <param name="synopsis">       Specifies the message Synopsis. </param>
    /// <param name="format">         Specifies the message details. </param>
    /// <param name="args">           Arguments to use in the format statement. </param>
    private void OnMessageAvailable( TraceEventType broadcastLevel, TraceEventType traceLevel, string synopsis, string format, params object[] args )
    {
        this.OnMessageAvailable( new MessageEventArgs( broadcastLevel, traceLevel, synopsis, "TTM:: " + format, args ) );
    }

    /// <summary> Raises a message. </summary>
    /// <param name="level">    Specifies the event arguments message
    /// <see cref="System.Diagnostics.TraceEventType">levels</see>. </param>
    /// <param name="synopsis"> Specifies the message short synopsis. </param>
    /// <param name="format">   Specifies the format for building the message detains. </param>
    /// <param name="args">     Specifies the format arguments. </param>
    private void OnMessageAvailable( TraceEventType level, string synopsis, string format, params object[] args )
    {
        this.OnMessageAvailable( level, level, synopsis, format, args );
    }

    #endregion

    #region " i device: synchronizer "

    /// <summary> Gets or sets the <see cref="System.ComponentModel.ISynchronizeInvoke">object</see>
    /// to use for marshaling events. </summary>
    /// <value> The synchronizer. </value>
    private System.ComponentModel.ISynchronizeInvoke? Synchronizer { get; set; }

    /// <summary> Sets the <see cref="System.ComponentModel.ISynchronizeInvoke">synchronization
    /// object</see>
    /// to use for marshaling events. </summary>
    /// <param name="value"> The meter device. </param>
    public void SynchronizerSetter( System.ComponentModel.ISynchronizeInvoke value )
    {
        this.Synchronizer = value;
    }

    #endregion

    #region " report visa operation okay "

    /// <summary> Reports on VISA or device errors. Can be used with queries because the report does
    /// not require querying the instrument. </summary>
    /// <remarks> This method is called after a VISA operation, such as a query, that could cause a
    /// query terminated failure if accessing the instrument. </remarks>
    /// <param name="synopsis"> The report synopsis. </param>
    /// <param name="format">   The report details format. </param>
    /// <param name="args">     The report details arguments. </param>
    /// <returns> True if no error occurred. </returns>
    public bool ReportVisaOperationOkay( string synopsis, string format, params object[] args )
    {
        Pith.SessionBase? session = this.Meter.TspDevice!.Session;
        if ( session is not null && session.IsSessionOpen )
        {
            ServiceRequests statusBute = session.ReadStatusByte();
            if ( session.IsErrorBitSet( statusBute ) )
            {
                string details = string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args );
                this.OnMessageAvailable( TraceEventType.Verbose, synopsis,
                    "{0}. Instrument {1} encountered VISA errors:\n{2}\n{3}", details, this.ResourceName, session.QueryDeviceErrors( statusBute ),
                    new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }
        }
        return true;
    }

    /// <summary> Reports on device errors. Can only be used after receiving a full reply from the
    /// instrument. Use <see cref="ReportVisaOperationOkay">report visa operation</see> to avoid
    /// accessing the information from the instrument. </summary>
    /// <remarks> This method is called after a VISA operation, such as a query, that could cause a
    /// query terminated failure if accessing the instrument. </remarks>
    /// <param name="synopsis">       The report synopsis. </param>
    /// <param name="format">         The report details format. </param>
    /// <param name="args">           The report details arguments. </param>
    /// <returns> True if no error occurred. </returns>
    public bool ReportDeviceOperationOkay( string synopsis, string format, params object[] args )
    {
        Pith.SessionBase? session = this.Meter.TspDevice!.Session;
        if ( session is not null && session.IsSessionOpen )
        {
            ServiceRequests statusBute = session.ReadStatusByte();
            if ( session.IsErrorBitSet( statusBute ) )
            {
                string details = string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args );
                this.OnMessageAvailable( TraceEventType.Verbose, synopsis,
                    "{0}. Instrument {1} encountered VISA errors:\n{2}\n{3}", details, this.ResourceName, session.QueryDeviceErrors( statusBute ),
                    new StackTrace().ParseCallStack( 4, 10, CallStackType.UserCallStack ) );
                return false;
            }
        }
        return true;
    }

    /// <summary> Reports on VISA or device errors. Can only be used after receiving a full reply from
    /// the instrument. Use <see cref="ReportVisaOperationOkay">report visa operation</see> to avoid
    /// accessing the information from the instrument. </summary>
    /// <remarks> This method is called after a VISA operation. It checked for error and sends
    /// <see cref="MessageAvailable">message available</see> if an error occurred. </remarks>
    /// <param name="synopsis">       The report synopsis. </param>
    /// <param name="format">         The report details format. </param>
    /// <param name="args">           The report details arguments. </param>
    /// <returns> True if no error occurred. </returns>
    public bool ReportVisaDeviceOperationOkay( string synopsis, string format, params object[] args )
    {
        return this.ReportVisaOperationOkay( synopsis, format, args );
        // these are now the same because we are no longer reporting the visa status code
        // which is specific to the defunct NI VISA NS. This code, which was storing the error code
        // of unhandled VISA exception, is no longer necessary because the ISR framework 
        // attempts to handle all exceptions. It is also assumed that the calling assembly traps unhandled exceptions
        //  && this.ReportDeviceOperationOkay( synopsis, format, args );
    }

    #endregion

    #region " i device: exception available "

    /// <summary> 
    /// Sends an Exception message to the client.
    /// </summary>
    public event EventHandler<System.Threading.ThreadExceptionEventArgs>? ExceptionAvailable;

    /// <summary> Raises the Exception Available event. </summary>
    /// <remarks> This method raises the exception if the exception messaging is not handled. </remarks>
    /// <param name="e"> Specifies the
    /// <see cref="System.Threading.ThreadExceptionEventArgs">Exception event arguments</see>. </param>
    private void OnExceptionAvailable( System.Threading.ThreadExceptionEventArgs e )
    {
        if ( this.Synchronizer is not null && this.Synchronizer.InvokeRequired )
        {
            _ = this.Synchronizer.Invoke( new Action<System.Threading.ThreadExceptionEventArgs>( this.OnExceptionAvailable ), [e] );
        }
        else
        {
            EventHandler<ThreadExceptionEventArgs>? evt = ExceptionAvailable;
            if ( evt is null )
                throw new InvalidOperationException( "An exception occurred but no handlers were set to receive the exception message. Check inner exception for details.", e.Exception );
            else
                evt.Invoke( this, e );
        }
    }

    /// <summary> Raises the Exception Available event. </summary>
    /// <remarks> This method raises the exception if the exception messaging is not handled. </remarks>
    /// <param name="exception">     . </param>
    /// <param name="shortSynopsis"> Specifies the synopsis message. </param>
    private void OnExceptionAvailable( Exception exception, string shortSynopsis )
    {
        exception.Data.Add( "Synopsis", shortSynopsis );
        this.OnExceptionAvailable( new System.Threading.ThreadExceptionEventArgs( exception ) );
    }

    /// <summary> Raises the Exception Available event. </summary>
    /// <remarks> This method raises the exception if the exception messaging is not handled. </remarks>
    /// <param name="exception"> . </param>
    /// <param name="format">    . </param>
    /// <param name="args">      . </param>
    private void OnExceptionAvailable( Exception exception, string format, params object[] args )
    {
        this.OnExceptionAvailable( exception, string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
    }

    #endregion
}
