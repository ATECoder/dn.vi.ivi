using System.ComponentModel;
using System.Diagnostics;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm;
/// <summary>
/// Defines the thermal transient meter including measurement and configuration. The Thermal
/// Transient meter includes the elements that define a complete set of thermal transient
/// measurements and settings.
/// </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2010-09-17, 2.2.3912.x. </para>
/// </remarks>
public partial class Meter : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDisposable
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public Meter() : base()
    {
        this.TspDeviceInternal = new K2600Device();
        this.ConfigInfo = new DeviceUnderTest();
        if ( this.ConfigInfo is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )} is null." );
        if ( this.ConfigInfo.InitialResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )}.{nameof( this.ConfigInfo.InitialResistance )} is null." );
        if ( this.ConfigInfo.FinalResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )}.{nameof( this.ConfigInfo.FinalResistance )} is null." );
        if ( this.ConfigInfo.ThermalTransient is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )}.{nameof( this.ConfigInfo.ThermalTransient )} is null." );
    }

    /// <summary> Creates a new Device. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A Device. </returns>
    public static Meter Create()
    {
        Meter device;
        try
        {
            device = new Meter();
        }
        catch
        {
            throw;
        }

        return device;
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
                    this.OnClosing();
                    this.TspDeviceInternal?.Dispose();
                    this.TspDeviceInternal = null;
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
    ~Meter()
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

    #region " Tsp Device "


    private K2600Device? TspDeviceInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
            {
                field.Closing -= this.TspDevice_Closing;
                field.Closed -= this.TspDevice_Closed;
                field.Opening -= this.TspDevice_Opening;
                field.Opened -= this.TspDevice_Opened;
                field.Initializing -= this.TspDevice_Initializing;
                field.Initialized -= this.TspDevice_Initialized;
                field.PropertyChanged -= this.TspDevice_PropertyChanged;
            }

            field = value;
            if ( field is not null )
            {
                field.Closing += this.TspDevice_Closing;
                field.Closed += this.TspDevice_Closed;
                field.Opening += this.TspDevice_Opening;
                field.Opened += this.TspDevice_Opened;
                field.Initializing += this.TspDevice_Initializing;
                field.Initialized += this.TspDevice_Initialized;
                field.PropertyChanged += this.TspDevice_PropertyChanged;
            }
        }
    }

    /// <summary> Gets the Tsp device. </summary>
    /// <value> The Tsp device. </value>
    public K2600Device? TspDevice => this.TspDeviceInternal;

    /// <summary> Gets a value indicating whether the subsystem has an open Device open. </summary>
    /// <value> <c>true</c> if the device has an open Device; otherwise, <c>false</c>. </value>
    public bool IsDeviceOpen => this.TspDeviceInternal is not null && this.TspDevice is not null && this.TspDevice.IsDeviceOpen;

    /// <summary> Gets a value indicating whether the subsystem has an open session open. </summary>
    /// <value> <c>true</c> if the device has an open session; otherwise, <c>false</c>. </value>
    public bool IsSessionOpen => this.TspDeviceInternal is not null && this.TspDevice is not null
        && this.TspDevice.Session is not null && this.TspDevice.Session.IsSessionOpen;

    /// <summary> Gets the name of the resource. </summary>
    /// <value> The name of the resource or &lt;closed&gt; if not open. </value>
    public string ResourceName => this.TspDevice is null
                ? "<closed>"
                : this.TspDevice.IsDeviceOpen ? this.TspDevice.OpenResourceName : this.TspDevice.CandidateResourceName;

    /// <summary> Disposes of the local elements. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void OnClosing()
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.ResourceName} handling closing meter";
            try
            {
                this.AbortTriggerSequenceIf();
            }
            catch ( Exception ex )
            {
                Debug.Assert( !Debugger.IsAttached, ex.ToString() );
            }

            try
            {
                this.MeasureSequencerInternal?.Dispose();
                this.MeasureSequencerInternal = null;
            }
            catch ( Exception ex )
            {
                Debug.Assert( !Debugger.IsAttached, ex.ToString() );
            }

            try
            {
                this.TriggerSequencerInternal?.Dispose();
                this.TriggerSequencerInternal = null;
            }
            catch ( Exception ex )
            {
                Debug.Assert( !Debugger.IsAttached, ex.ToString() );
            }

            try
            {
                this.BindShuntResistance( null );
                this.BindInitialResistance( null );
                this.BindFinalResistance( null );
                this.BindThermalTransient( null );
                this.BindThermalTransientEstimator( null );
            }
            catch ( Exception ex )
            {
                Debug.Assert( !Debugger.IsAttached, ex.ToString() );
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary>   Event handler. Called by TspDevice for closing events. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void TspDevice_Closing( object? sender, EventArgs e )
    {
        if ( sender is not null && this.IsDeviceOpen )
            this.OnClosing();
    }

    /// <summary>
    /// Event handler. Called by the TspDevice when closed after all subsystems were disposed.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void TspDevice_Closed( object? sender, EventArgs e )
    {
    }

    /// <summary>
    /// Event handler. Called by the TspDevice before initializing the subsystems.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void TspDevice_Opening( object? sender, EventArgs e )
    {
        this.MeasureSequencer = new MeasureSequencer();
        this.TriggerSequencer = new TriggerSequencer();
    }

    /// <summary>
    /// Event handler. Called by TspDevice when opened after initializing the subsystems.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void TspDevice_Opened( object? sender, EventArgs e )
    {
        string activity = string.Empty;

        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.Session is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice.Session )} is null." );
        if ( !this.TspDevice.Session.IsDeviceOpen ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice.Session )} must be open." );
        if ( this.TspDevice.StatusSubsystem is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.StatusSubsystem )} is null." );

        try
        {
            activity = $"{this.ResourceName} handling Tsp device opening event";

            // This is done in TSP
            // if ( this.IsDeviceOpen ) _ = this.TspDevice.Session!.WriteLine( "ttm=_G.isr.meters.thermalTransient" );

            // read the firmware version to determine if using hte legacy software.
            MeterSubsystem.LegacyFirmware = !this.TspDevice.Session.IsNil( Syntax.ThermalTransient.LegacyVersionGetter.TrimEnd( "()".ToCharArray() ) )
                || (!this.TspDevice.Session.IsNil( Syntax.ThermalTransient.PresentVersionGetter.TrimEnd( "()".ToCharArray() ) )
                  ? false
                  : throw new InvalidOperationException( "" ));

            // get the meter firmware version
            MeterSubsystem.FirmwareVersion = new Version( this.TspDevice.Session.QueryTrimEnd( Syntax.ThermalTransient.VersionQueryCommand ) );

            // meter subsystem must be first
            this.BindMeterSubsystem( new MeterSubsystem( this.TspDevice.StatusSubsystem, this.ConfigInfo.MeterMain!, ThermalTransientMeterEntity.MeterMain ) );

            // initial resistance must be first.
            this.BindInitialResistance( new ColdResistanceMeasure( this.TspDevice.StatusSubsystem, this.ConfigInfo.InitialResistance!, ThermalTransientMeterEntity.InitialResistance ) );
            this.BindFinalResistance( new ColdResistanceMeasure( this.TspDevice.StatusSubsystem, this.ConfigInfo.FinalResistance!, ThermalTransientMeterEntity.FinalResistance ) );
            this.BindShuntResistance( new ShuntResistance() );
            this.BindThermalTransientEstimator( new ThermalTransientEstimator( this.TspDevice.StatusSubsystem ) );
            this.BindThermalTransient( new ThermalTransientMeasure( this.TspDevice.StatusSubsystem, this.ConfigInfo.ThermalTransient! ) );
            if ( this.ThermalTransient is null ) throw new InvalidOperationException( $"Meter {nameof( this.ThermalTransient )} is null." );
            this.TspDevice.AddSubsystem( this.ThermalTransient );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary>   Event handler. Called by TspDevice for initializing events. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Cancel event information. </param>
    private void TspDevice_Initializing( object? sender, System.ComponentModel.CancelEventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.ResourceName} initializing Tsp Device";
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary>   Event handler. Called by TspDevice for initialized events. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void TspDevice_Initialized( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.ResourceName} initialized the Tsp Device";
        }
        // this is already done.  Me.TspDevice.AddListeners(Me.Talker)
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Raises the system. component model. property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( K2600Device sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( K2600Device.IsDeviceOpen ):
                {
                    _ = sender.IsDeviceOpen
                        ? cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{sender.ResourceNameCaption} open;. " )
                        : cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{sender.ResourceNameCaption} close;. " );

                    break;
                }

            default:
                break;
        }
    }

    /// <summary>   Event handler. Called by TspDevice for property changed events. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Property Changed event information. </param>
    private void TspDevice_PropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        string activity = string.Empty;
        if ( sender is null || e is null ) return;
        try
        {
            activity = $"{this.ResourceName} handling the Tsp Device {e.PropertyName} property changed event.";
            if ( sender is K2600Device s ) this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " i presettable "

    /// <summary> Sets values to their known clear execution state. </summary>
    /// <remarks> This erases the last reading. </remarks>
    public virtual void DefineClearExecutionState()
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.Session is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.Session )} is null." );
        if ( this.TspDevice.DisplaySubsystem is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.DisplaySubsystem )} is null." );
        if ( this.ConfigInfo is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )} is null." );
        if ( this.ConfigInfo.InitialResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )}.{nameof( this.ConfigInfo.InitialResistance )} is null." );
        if ( this.ConfigInfo.FinalResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )}.{nameof( this.ConfigInfo.FinalResistance )} is null." );
        if ( this.ConfigInfo.ThermalTransient is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )}.{nameof( this.ConfigInfo.ThermalTransient )} is null." );
        if ( this.ShuntResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.ShuntResistance )} is null." );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing device under test execution state;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        this.ConfigInfo.DefineClearExecutionState();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing shunt execution state;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        this.ShuntResistance.DefineClearExecutionState();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Displaying title;. " );

        this.TspDevice.DisplaySubsystem.DisplayTitle( $"TTM {MeterSubsystem.FirmwareVersion}", "Integrated Scientific Resources" );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary>
    /// Clears the queues and resets all registers to zero. Sets system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    /// <remarks> *CLS. </remarks>
    public void ClearExecutionState()
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing the Tsp Device execution state;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        this.TspDevice.ClearExecutionState();
        this.DefineClearExecutionState();
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Use this method to customize the reset. </remarks>
    public virtual void InitKnownState()
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.ConfigInfo is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )} is null." );
        if ( this.ShuntResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.ShuntResistance )} is null." );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Initializing part configuration  to known state;. " );
        this.ConfigInfo.InitKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Initializing shunt to known state;. " );
        this.ShuntResistance.InitKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Initializing the Tsp Device to known state;. " );
        this.TspDevice.InitKnownState();
    }

    /// <summary> Sets the known preset state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void PresetKnownState()
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.ConfigInfo is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )} is null." );
        if ( this.ShuntResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.ShuntResistance )} is null." );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Presetting part configuration to known state;. " );
        this.ConfigInfo.PresetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Presetting shunt to known state;. " );
        this.ShuntResistance.PresetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Presetting the Tsp Device known state;. " );
        this.TspDevice.PresetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> Preforms a full reset, initialize and clear. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void ResetClear()
    {
        this.ResetKnownState();
        this.InitKnownState();
        this.ClearExecutionState();
    }

    /// <summary> Sets the known reset (default) state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void ResetKnownState()
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.ConfigInfo is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )} is null." );
        if ( this.ShuntResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.ShuntResistance )} is null." );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resetting part configuration to known state;. " );
        this.ConfigInfo.ResetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resetting shunt to known state;. " );
        this.ShuntResistance.ResetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resetting the Tsp Device known state;. " );
        this.TspDevice.ResetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    #endregion

    #region " subsystems: Meter Subsystem "

    /// <summary>   Gets or sets the meter subsystem. </summary>
    /// <value> The meter subsystem. </value>
    public MeterSubsystem? MeterSubsystem { get; private set; }

    /// <summary> Binds the meter subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindMeterSubsystem( MeterSubsystem? subsystem )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.MeterSubsystem is not null )
        {
            this.TspDevice.RemoveSubsystem( this.MeterSubsystem );
            this.MeterSubsystem = null;
        }

        this.MeterSubsystem = subsystem;
        if ( this.MeterSubsystem is not null )
            this.TspDevice.AddSubsystem( this.MeterSubsystem );
    }

    #endregion

    #region " subsystems: initial resistance "

    /// <summary>
    /// Gets or sets reference to the <see cref="ColdResistanceMeasure">meter initial cold
    /// resistance</see>
    /// </summary>
    /// <exception cref="ArgumentNullException">       Thrown when one or more required arguments
    ///                                                are null. </exception>
    /// <exception cref="InvalidOperationException">   Thrown when the requested operation is
    ///                                                invalid. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <value> The initial resistance. </value>
    public ColdResistanceMeasure? InitialResistance { get; private set; }

    /// <summary> Binds the initial resistance subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindInitialResistance( ColdResistanceMeasure? subsystem )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.InitialResistance is not null )
        {
            this.TspDevice.RemoveSubsystem( this.InitialResistance );
            this.InitialResistance = null;
        }

        this.InitialResistance = subsystem;
        if ( this.InitialResistance is not null )
            this.TspDevice.AddSubsystem( this.InitialResistance );
    }

    #endregion

    #region " subsystems: final resistance "

    /// <summary>
    /// Gets or sets reference to the <see cref="ColdResistanceMeasure">meter Final cold
    /// resistance</see>
    /// </summary>
    /// <exception cref="ArgumentNullException">       Thrown when one or more required arguments
    ///                                                are null. </exception>
    /// <exception cref="InvalidOperationException">   Thrown when the requested operation is
    ///                                                invalid. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <value> The Final resistance. </value>
    public ColdResistanceMeasure? FinalResistance { get; private set; }

    /// <summary> Binds the Final resistance subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindFinalResistance( ColdResistanceMeasure? subsystem )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.FinalResistance is not null )
        {
            this.TspDevice.RemoveSubsystem( this.FinalResistance );
            this.FinalResistance = null;
        }

        this.FinalResistance = subsystem;
        if ( this.FinalResistance is not null )
            this.TspDevice.AddSubsystem( this.FinalResistance );
    }

    #endregion

    #region " subsystems: shunt resistance "

    /// <summary>
    /// Gets or sets reference to the <see cref="ShuntResistance">Shunt resistance</see>
    /// </summary>
    /// <exception cref="ArgumentNullException">       Thrown when one or more required arguments
    ///                                                are null. </exception>
    /// <exception cref="InvalidOperationException">   Thrown when the requested operation is
    ///                                                invalid. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <value> The shunt resistance. </value>
    public ShuntResistance? ShuntResistance { get; private set; }

    /// <summary> Binds the Shunt resistance subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindShuntResistance( ShuntResistance? subsystem )
    {
        if ( this.ShuntResistance is not null ) this.ShuntResistance = null;
        this.ShuntResistance = subsystem;
    }

    #endregion

    #region " subsystems: thermal transient "

    /// <summary>
    /// Gets or sets reference to the <see cref="ThermalTransientMeasure">meter thermal transient</see>
    /// </summary>
    /// <exception cref="ArgumentNullException">       Thrown when one or more required arguments
    ///                                                are null. </exception>
    /// <exception cref="InvalidOperationException">   Thrown when the requested operation is
    ///                                                invalid. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <value> The thermal transient. </value>
    public ThermalTransientMeasure? ThermalTransient { get; private set; }

    /// <summary> Binds the Final resistance subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindThermalTransient( ThermalTransientMeasure? subsystem )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.ThermalTransient is not null )
        {
            this.TspDevice.RemoveSubsystem( this.ThermalTransient );
            this.ThermalTransient = null;
        }

        this.ThermalTransient = subsystem;
        if ( this.ThermalTransient is not null )
            this.TspDevice.AddSubsystem( this.ThermalTransient );
    }

    #endregion

    #region " subsystems: estimator "

    /// <summary>
    /// Gets or sets reference to the <see cref="ThermalTransientEstimator">thermal transient
    /// estimator</see>
    /// </summary>
    /// <exception cref="ArgumentNullException">       Thrown when one or more required arguments
    ///                                                are null. </exception>
    /// <exception cref="InvalidOperationException">   Thrown when the requested operation is
    ///                                                invalid. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <value> The thermal transient estimator. </value>
    public ThermalTransientEstimator? ThermalTransientEstimator { get; private set; }

    /// <summary> Binds the Final resistance subsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindThermalTransientEstimator( ThermalTransientEstimator? subsystem )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.ThermalTransientEstimator is not null )
        {
            this.TspDevice.RemoveSubsystem( this.ThermalTransientEstimator );
            this.ThermalTransientEstimator = null;
        }

        this.ThermalTransientEstimator = subsystem;
        if ( this.ThermalTransientEstimator is not null )
            this.TspDevice.AddSubsystem( this.ThermalTransientEstimator );
    }

    #endregion

    #region " configure "

    /// <summary> Gets or sets the <see cref="ConfigInfo">Device under test</see>. </summary>
    /// <exception cref="ArgumentNullException">       Thrown when one or more required arguments
    ///                                                are null. </exception>
    /// <exception cref="InvalidOperationException">   Thrown when the requested operation is
    ///                                                invalid. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <value> Information describing the configuration. </value>
    public DeviceUnderTest ConfigInfo { get; set; }

    /// <summary> Configure part information. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="configurationInformation"> The <see cref="ConfigInfo">Device under test</see>. </param>
    public void ConfigurePartInfo( DeviceUnderTest configurationInformation )
    {
        if ( configurationInformation is null ) throw new ArgumentNullException( nameof( configurationInformation ) );
        if ( this.ConfigInfo is null ) throw new InvalidOperationException( $"Meter {nameof( this.ConfigInfo )} is null" );
        this.ConfigInfo.PartNumber = configurationInformation.PartNumber;
        this.ConfigInfo.OperatorId = configurationInformation.OperatorId;
        this.ConfigInfo.LotId = configurationInformation.LotId;
        this.ConfigInfo.ContactCheckEnabled = configurationInformation.ContactCheckEnabled;
        this.ConfigInfo.ContactCheckThreshold = configurationInformation.ContactCheckThreshold;
    }

    /// <summary> Configures the given device under test. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException">       Thrown when one or more required arguments
    ///                                                are null. </exception>
    /// <exception cref="InvalidOperationException">   Thrown when the requested operation is
    ///                                                invalid. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="configurationInformation"> The <see cref="ConfigInfo">Device under test</see>. </param>
    public void Configure( DeviceUnderTest configurationInformation )
    {
        if ( configurationInformation is null )
        {
            throw new ArgumentNullException( nameof( configurationInformation ) );
        }
        else if ( configurationInformation.InitialResistance is null )
        {
            throw new InvalidOperationException( "Initial MeasuredValue null detected in device under test." );
        }
        else if ( this.InitialResistance is null )
        {
            throw new InvalidOperationException( "Meter Initial MeasuredValue null detected in device under test." );
        }
        else if ( configurationInformation.FinalResistance is null )
        {
            throw new InvalidOperationException( "Final MeasuredValue null detected in device under test." );
        }
        else if ( this.FinalResistance is null )
        {
            throw new InvalidOperationException( "Meter Final MeasuredValue null detected in device under test." );
        }
        else if ( configurationInformation.ThermalTransient is null )
        {
            throw new InvalidOperationException( "Thermal Transient null detected in device under test." );
        }
        else if ( this.ThermalTransient is null )
        {
            throw new InvalidOperationException( "Meter Thermal Transient null detected in device under test." );
        }
        else if ( this.ConfigInfo is null )
        {
            throw new InvalidOperationException( "Meter Configuration null detected in device under test." );
        }
        else if ( this.ConfigInfo.InitialResistance is null )
        {
            throw new InvalidOperationException( "Meter Configuration Initial MeasuredValue null detected in device under test." );
        }

        if ( !this.ConfigInfo.SourceMeasureUnitEquals( configurationInformation ) )
        {
            if ( !this.InitialResistance.SourceMeasureUnitExists( this.ConfigInfo.InitialResistance.SourceMeasureUnit ) )
            {
                throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, "Source measure unit name {0} is invalid.", this.ConfigInfo.InitialResistance.SourceMeasureUnit ) );
            }
        }

        if ( !Ttm.ThermalTransient.ValidateVoltageLimit( configurationInformation.ThermalTransient.VoltageLimit,
            configurationInformation.InitialResistance.HighLimit, configurationInformation.ThermalTransient.CurrentLevel,
            configurationInformation.ThermalTransient.AllowedVoltageChange, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( configurationInformation ), details );

        this.ConfigurePartInfo( configurationInformation );
        this.InitialResistance.Configure( configurationInformation.InitialResistance );
        this.FinalResistance.Configure( configurationInformation.FinalResistance );
        this.ThermalTransient.Configure( configurationInformation.ThermalTransient );
    }

    /// <summary> Configures the given device under test for changed values. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException">       Thrown when one or more required arguments
    ///                                                are null. </exception>
    /// <exception cref="InvalidOperationException">   Thrown when the requested operation is
    ///                                                invalid. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="configurationInformation"> The <see cref="ConfigInfo">Device under test</see>. </param>
    public void ConfigureChanged( DeviceUnderTest configurationInformation )
    {
        if ( configurationInformation is null )
        {
            throw new ArgumentNullException( nameof( configurationInformation ) );
        }
        else if ( configurationInformation.InitialResistance is null )
        {
            throw new InvalidOperationException( "Initial MeasuredValue null detected in device under test." );
        }
        else if ( this.InitialResistance is null )
        {
            throw new InvalidOperationException( "Meter Initial MeasuredValue null detected in device under test." );
        }
        else if ( configurationInformation.FinalResistance is null )
        {
            throw new InvalidOperationException( "Final MeasuredValue null detected in device under test." );
        }
        else if ( this.FinalResistance is null )
        {
            throw new InvalidOperationException( "Meter Final MeasuredValue null detected in device under test." );
        }
        else if ( configurationInformation.ThermalTransient is null )
        {
            throw new InvalidOperationException( "Thermal Transient null detected in device under test." );
        }
        else if ( this.ThermalTransient is null )
        {
            throw new InvalidOperationException( "Meter Thermal Transient null detected in device under test." );
        }
        else if ( this.ConfigInfo is null )
        {
            throw new InvalidOperationException( "Meter Configuration null detected in device under test." );
        }
        else if ( this.ConfigInfo.InitialResistance is null )
        {
            throw new InvalidOperationException( "Meter Configuration Initial MeasuredValue null detected in device under test." );
        }

        if ( !this.ConfigInfo.SourceMeasureUnitEquals( configurationInformation ) )
        {
            if ( !this.InitialResistance.SourceMeasureUnitExists( this.ConfigInfo.InitialResistance.SourceMeasureUnit ) )
            {
                throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, "Source measure unit name {0} is invalid.", this.ConfigInfo.InitialResistance.SourceMeasureUnit ) );
            }
        }

        if ( !Ttm.ThermalTransient.ValidateVoltageLimit( configurationInformation.ThermalTransient.VoltageLimit,
            configurationInformation.InitialResistance.HighLimit, configurationInformation.ThermalTransient.CurrentLevel,
            configurationInformation.ThermalTransient.AllowedVoltageChange, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( configurationInformation ), details );

        this.ConfigurePartInfo( configurationInformation );
        this.InitialResistance.ConfigureChanged( configurationInformation.InitialResistance );
        this.FinalResistance.ConfigureChanged( configurationInformation.FinalResistance );
        this.ThermalTransient.ConfigureChanged( configurationInformation.ThermalTransient );
    }

    #endregion

    #region " ttm framework: source channel "

    /// <summary> Source measure unit. </summary>

    /// <summary> Gets or sets (protected) the source measure unit "smua" or "smub". </summary>
    /// <value> The source measure unit. </value>
    public string SourceMeasureUnit
    {
        get;

        protected set
        {
            if ( string.IsNullOrWhiteSpace( value ) )
                value = string.Empty;
            if ( !value.Equals( this.SourceMeasureUnit ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = Syntax.ThermalTransient.DefaultSourceMeterName;

    /// <summary> Programs the Source Measure Unit. Does not read back from the instrument. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="value"> the Source Measure Unit, e.g., 'smua' or 'smub'. </param>
    /// <returns> The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    public string WriteSourceMeasureUnit( string value )
    {
        if ( string.IsNullOrWhiteSpace( value ) ) throw new ArgumentNullException( value );
        if ( this.InitialResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.InitialResistance )} is null." );
        else if ( this.FinalResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.FinalResistance )} is null." );
        else if ( this.ThermalTransient is null )
            throw new InvalidOperationException( $"Meter {nameof( this.ThermalTransient )} is null." );
        else if ( this.TspDevice is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        else if ( this.TspDevice.SourceMeasureUnit is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SourceMeasureUnit )} is null." );

        string unitNumber = value[^1..];
        this.TspDevice.SourceMeasureUnit.UnitNumber = unitNumber;
        _ = this.InitialResistance.WriteSourceMeasureUnit( value );
        _ = this.FinalResistance.WriteSourceMeasureUnit( value );
        _ = this.ThermalTransient.WriteSourceMeasureUnit( value );
        this.SourceMeasureUnit = value;
        return this.SourceMeasureUnit;
    }

    #endregion

    #region " ttm framework: meters firmware "

    /// <summary> Checks if the firmware version command exists. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns>
    /// <c>true</c> if the firmware version command exists; otherwise, <c>false</c>.
    /// </returns>
    public bool FirmwareVersionQueryCommandExists()
    {
        return this.IsSessionOpen && this.TspDevice is not null && this.TspDevice.Session is not null
            && !this.TspDevice.Session.IsNil( Syntax.ThermalTransient.VersionQueryCommand.TrimEnd( "()".ToCharArray() ) );
    }

    /// <summary> Gets or sets the version of the current firmware release. </summary>
    /// <value> The firmware released version. </value>
    public string FirmwareReleasedVersion
    {
        get
        {
            if ( string.IsNullOrWhiteSpace( field ) )
                _ = this.QueryFirmwareVersion();

            return field;
        }

        protected set
        {
            if ( string.IsNullOrWhiteSpace( value ) )
                value = string.Empty;
            if ( !value.Equals( this.FirmwareReleasedVersion ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    } = string.Empty;

    /// <summary>
    /// Queries the embedded firmware version from a remote node and saves it to
    /// <see cref="FirmwareReleasedVersion">the firmware version cache.</see>
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The firmware version. </returns>
    public string QueryFirmwareVersion()
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.Session is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.Session )} is null." );

        this.FirmwareReleasedVersion = this.FirmwareVersionQueryCommandExists()
            ? this.TspDevice.Session.QueryTrimEnd( Syntax.ThermalTransient.VersionQueryCommand )
            : cc.isr.VI.Syntax.Tsp.Lua.NilValue;
        return this.FirmwareReleasedVersion;
    }

    #endregion

    #region " ttm framework: shunt "

    /// <summary> Configures the meter for making Shunt MeasuredValue measurements. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The shunt resistance. </param>
    public void ConfigureShuntResistance( ShuntResistanceBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        if ( this.InitialResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.InitialResistance )} is null." );
        else if ( this.FinalResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.FinalResistance )} is null." );
        else if ( this.ThermalTransient is null )
            throw new InvalidOperationException( $"Meter {nameof( this.ThermalTransient )} is null." );
        else if ( this.ShuntResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.ShuntResistance )} is null." );
        else if ( this.TspDevice is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        else if ( this.TspDevice.Session is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.Session )} is null." );
        else if ( this.TspDevice.SourceMeasureUnit is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SourceMeasureUnit )} is null." );
        else if ( this.TspDevice.SourceSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SourceSubsystem )} is null." );
        else if ( this.TspDevice.CurrentSourceSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.CurrentSourceSubsystem )} is null." );
        else if ( this.TspDevice.SenseSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SenseSubsystem )} is null." );
        else if ( this.TspDevice.MeasureVoltageSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.MeasureVoltageSubsystem )} is null." );
        else if ( this.TspDevice.DisplaySubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.DisplaySubsystem )} is null." );
        else if ( this.TspDevice.StatusSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.StatusSubsystem )} is null." );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Configuring shunt resistance measurement;. " );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to DC current source {SourceFunctionMode.CurrentDC};. " );
        _ = this.TspDevice.SourceSubsystem.ApplySourceFunction( SourceFunctionMode.CurrentDC );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to DC current range {resistance.CurrentRange};. " );
        _ = this.TspDevice.CurrentSourceSubsystem.ApplyRange( resistance.CurrentRange );
        this.ShuntResistance.CurrentRange = this.TspDevice.CurrentSourceSubsystem.Range.GetValueOrDefault( 0d );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to DC current Level {resistance.CurrentLevel};. " );
        _ = this.TspDevice.CurrentSourceSubsystem.ApplyLevel( resistance.CurrentLevel );
        this.ShuntResistance.CurrentLevel = this.TspDevice.CurrentSourceSubsystem.Level.GetValueOrDefault( 0d );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to DC current Voltage Limit {resistance.VoltageLimit};. " );
        _ = this.TspDevice.CurrentSourceSubsystem.ApplyVoltageLimit( resistance.VoltageLimit );
        this.ShuntResistance.VoltageLimit = this.TspDevice.CurrentSourceSubsystem.VoltageLimit.GetValueOrDefault( 0d );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to four wire sense;. " );
        _ = this.TspDevice.SenseSubsystem.ApplySenseMode( SenseActionMode.Remote );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to auto voltage range;. " );
        _ = this.TspDevice.MeasureVoltageSubsystem.ApplyAutoRangeVoltageEnabled( true );
        this.TspDevice.DisplaySubsystem.ClearDisplay();
        this.TspDevice.DisplaySubsystem.DisplayLine( 2, $"SrcI:{this.ShuntResistance.CurrentLevel}A LimV:{this.ShuntResistance.VoltageLimit}V" );
        this.TspDevice.Session.ThrowDeviceExceptionIfError();
        this.ShuntResistance.CheckThrowUnequalConfiguration( resistance );
    }

    /// <summary> Apply changed Shunt MeasuredValue configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The shunt resistance. </param>
    public void ConfigureShuntResistanceChanged( ShuntResistanceBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );

        if ( this.InitialResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.InitialResistance )} is null." );
        else if ( this.FinalResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.FinalResistance )} is null." );
        else if ( this.ThermalTransient is null )
            throw new InvalidOperationException( $"Meter {nameof( this.ThermalTransient )} is null." );
        else if ( this.ShuntResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.ShuntResistance )} is null." );
        else if ( this.TspDevice is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        else if ( this.TspDevice.Session is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.Session )} is null." );
        else if ( this.TspDevice.SourceMeasureUnit is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SourceMeasureUnit )} is null." );
        else if ( this.TspDevice.SourceSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SourceSubsystem )} is null." );
        else if ( this.TspDevice.CurrentSourceSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.CurrentSourceSubsystem )} is null." );
        else if ( this.TspDevice.SenseSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SenseSubsystem )} is null." );
        else if ( this.TspDevice.MeasureVoltageSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.MeasureVoltageSubsystem )} is null." );
        else if ( this.TspDevice.DisplaySubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.DisplaySubsystem )} is null." );
        else if ( this.TspDevice.StatusSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.StatusSubsystem )} is null." );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to DC current source;. " );
        _ = this.TspDevice.SourceSubsystem.ApplySourceFunction( SourceFunctionMode.CurrentDC );
        if ( !this.ShuntResistance.ConfigurationEquals( resistance ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Configuring shunt resistance measurement;. " );
            if ( !this.ShuntResistance.CurrentRange.Equals( resistance.CurrentRange ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to DC current range {resistance.CurrentRange};. " );
                _ = this.TspDevice.CurrentSourceSubsystem.ApplyRange( resistance.CurrentRange );
                this.ShuntResistance.CurrentRange = this.TspDevice.CurrentSourceSubsystem.Range.GetValueOrDefault( 0d );
            }

            if ( !this.ShuntResistance.CurrentLevel.Equals( resistance.CurrentLevel ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to DC current Level {resistance.CurrentLevel};. " );
                _ = this.TspDevice.CurrentSourceSubsystem.ApplyLevel( resistance.CurrentLevel );
                this.ShuntResistance.CurrentLevel = this.TspDevice.CurrentSourceSubsystem.Level.GetValueOrDefault( 0d );
            }

            if ( !this.ShuntResistance.VoltageLimit.Equals( resistance.VoltageLimit ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to DC current Voltage Limit {resistance.VoltageLimit};. " );
                _ = this.TspDevice.CurrentSourceSubsystem.ApplyVoltageLimit( resistance.VoltageLimit );
                this.ShuntResistance.VoltageLimit = this.TspDevice.CurrentSourceSubsystem.VoltageLimit.GetValueOrDefault( 0d );
            }

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to four wire sense;. " );
            _ = this.TspDevice.SenseSubsystem.ApplySenseMode( SenseActionMode.Remote );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.TspDevice.SourceMeasureUnit} to auto voltage range;. " );
            _ = this.TspDevice.MeasureVoltageSubsystem.ApplyAutoRangeVoltageEnabled( true );
        }

        this.TspDevice.DisplaySubsystem.ClearDisplay();
        this.TspDevice.DisplaySubsystem.DisplayLine( 2, $"SrcI:{this.ShuntResistance.CurrentLevel}A LimV:{this.ShuntResistance.VoltageLimit}V" );
        this.TspDevice.Session.ThrowDeviceExceptionIfError();
        this.ShuntResistance.CheckThrowUnequalConfiguration( resistance );
    }

    /// <summary> Measures the Shunt resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The shunt resistance. </param>
    public void MeasureShuntResistance( MeasureBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );

        if ( this.InitialResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.InitialResistance )} is null." );
        else if ( this.FinalResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.FinalResistance )} is null." );
        else if ( this.ThermalTransient is null )
            throw new InvalidOperationException( $"Meter {nameof( this.ThermalTransient )} is null." );
        else if ( this.ShuntResistance is null )
            throw new InvalidOperationException( $"Meter {nameof( this.ShuntResistance )} is null." );
        else if ( this.TspDevice is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        else if ( this.TspDevice.Session is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.Session )} is null." );
        else if ( this.TspDevice.SourceMeasureUnit is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SourceMeasureUnit )} is null." );
        else if ( this.TspDevice.SourceSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SourceSubsystem )} is null." );
        else if ( this.TspDevice.CurrentSourceSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.CurrentSourceSubsystem )} is null." );
        else if ( this.TspDevice.SenseSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.SenseSubsystem )} is null." );
        else if ( this.TspDevice.MeasureVoltageSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.MeasureVoltageSubsystem )} is null." );
        else if ( this.TspDevice.MeasureResistanceSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.MeasureResistanceSubsystem )} is null." );
        else if ( this.TspDevice.DisplaySubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.DisplaySubsystem )} is null." );
        else if ( this.TspDevice.StatusSubsystem is null )
            throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.StatusSubsystem )} is null." );

        this.ShuntResistance.HighLimit = resistance.HighLimit;
        this.ShuntResistance.LowLimit = resistance.LowLimit;
        this.TspDevice.MeasureResistanceSubsystem.Measure();
        string? reading = this.TspDevice.MeasureResistanceSubsystem.Reading
            ?? throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.MeasureResistanceSubsystem )}.{nameof( this.TspDevice.MeasureResistanceSubsystem.Reading )} is null." );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Parsing shunt resistance reading {reading};. " );
        this.TspDevice.Session.ThrowDeviceExceptionIfError();
        this.ShuntResistance.ParseReading( reading, MeasurementOutcomes.None );
        resistance.ParseReading( reading, MeasurementOutcomes.None );
        this.TspDevice.DisplaySubsystem.DisplayLine( 1, reading.Trim() );
        this.TspDevice.DisplaySubsystem.DisplayCharacter( 1, reading.Length, 18 );
        this.ShuntResistance.MeasurementAvailable = true;
    }

    #endregion

    #region " ttm framework: measure "

    /// <summary> Makes all measurements. Reading is done after all measurements are done. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void Measure()
    {
        if ( this.IsSessionOpen )
        {
            _ = this.TspDevice?.Session?.WriteLine( "_G.ttm.measure()  waitcomplete() " );
        }
    }

    #endregion

    #region " ttm framework: trigger "

    /// <summary> True if the meter was enabled to respond to trigger events. </summary>
    public bool IsAwaitingTrigger { get; set; }

    /// <summary> Abort triggered measurements. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void AbortTriggerSequenceIf()
    {
        if ( this.IsSessionOpen && this.IsAwaitingTrigger && this.TriggerSequencer is not null && this.TspDevice is not null
            && this.TspDevice.Session is not null && this.ThermalTransient is not null && this.MeterSubsystem is not null )
        {
            this.TriggerSequencer.RestartSignal = TriggerSequenceSignal.Stop;
            this.TspDevice.Session.AssertTrigger();
            // allow time for the measurement to terminate.
            System.Threading.Thread.Sleep( ( int ) Math.Round( 1000d * this.MeterSubsystem.MeterMain.PostTransientDelay ) + 400 );
            this.IsAwaitingTrigger = false;
        }
    }

    /// <summary>
    /// Primes the instrument to wait for a trigger. This clears the digital outputs and loops until
    /// trigger or <see cref="SessionBase.AssertTrigger"/> command or external *TRG command.
    /// </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="lockLocal">        (Optional) When true, displays the title and locks the local
    ///                                 key. </param>
    /// <param name="onCompleteReply">  (Optional) The operation completion reply. </param>
    public void PrepareForTrigger( bool lockLocal = false, string onCompleteReply = "OPC" )
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.Session is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.Session )} is null." );
        SessionBase session = this.TspDevice.Session;

        this.IsAwaitingTrigger = true;
        session.MessageAvailableBitmask = Pith.ServiceRequests.MessageAvailable;

        session.SetLastAction( "clearing execution state." );
        session.ClearExecutionState();

        session.SetLastAction( "enabling service request on completion." );
        session.EnableServiceRequestOnOperationCompletion();

        session.SetLastAction( "waiting for trigger" );
        _ = session.WriteLine( "prepareForTrigger({0},'{1}') waitcomplete() ",
                lockLocal ? cc.isr.VI.Syntax.Tsp.Lua.TrueValue : cc.isr.VI.Syntax.Tsp.Lua.FalseValue, onCompleteReply );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
    }

    /// <summary> Gets or sets the measurement completed. </summary>
    /// <value> The measurement completed. </value>
    public bool MeasurementCompleted
    {
        get;
        set
        {
            if ( !value.Equals( this.MeasurementCompleted ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Reads the measurements from the instrument. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="initialResistance"> The initial resistance. </param>
    /// <param name="finalResistance">   The final resistance. </param>
    /// <param name="thermalTransient">  The thermal transient. </param>
    public void ReadMeasurements( MeasureBase initialResistance, MeasureBase finalResistance, MeasureBase thermalTransient )
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.Session is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.Session )} is null." );
        if ( this.InitialResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.InitialResistance )} is null." );
        if ( this.FinalResistance is null ) throw new InvalidOperationException( $"Meter {nameof( this.FinalResistance )} is null." );
        if ( this.ThermalTransient is null ) throw new InvalidOperationException( $"Meter {nameof( this.ThermalTransient )} is null." );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"'{0}' reading measurements;. ", this.ResourceName );
        this.IsAwaitingTrigger = false;
        this.InitialResistance.ReadResistance( initialResistance );
        this.FinalResistance.ReadResistance( finalResistance );
        this.ThermalTransient.ReadThermalTransient( thermalTransient );
    }

    #endregion

    #region " check contacts "

    /// <summary> Checks contact resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="threshold"> The threshold. </param>
    public void CheckContacts( int threshold )
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.ContactSubsystem is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.ContactSubsystem )} is null." );

        this.TspDevice.Session!.SetLastAction( "measuring contacts" );

        _ = this.TspDevice.ContactSubsystem.CheckContacts( threshold );
        if ( !this.TspDevice.ContactSubsystem.ContactCheckOkay.HasValue )
        {
            throw new cc.isr.VI.Pith.DeviceException( this.TspDevice.ResourceNameCaption, "Failed Measuring contacts;. " );
        }
        else if ( !this.TspDevice.ContactSubsystem.ContactCheckOkay.Value )
        {
            throw new cc.isr.VI.Pith.DeviceException( this.TspDevice.ResourceNameCaption, $"High contact resistances;. Values: '{this.TspDevice.ContactSubsystem.ContactResistances}'" );
        }
    }

    /// <summary> Checks contact resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="threshold"> The threshold. </param>
    /// <param name="details">   [out] The details. </param>
    /// <returns> <c>true</c> if contacts checked okay. </returns>
    public bool TryCheckContacts( int threshold, out string details )
    {
        if ( this.TspDevice is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.ContactSubsystem is null ) throw new InvalidOperationException( $"Meter {nameof( this.TspDevice )}.{nameof( this.TspDevice.ContactSubsystem )} is null." );

        this.TspDevice.Session!.SetLastAction( "measuring contacts" );
        _ = this.TspDevice.ContactSubsystem.CheckContacts( threshold );
        if ( !this.TspDevice.ContactSubsystem.ContactCheckOkay.HasValue )
        {
            details = "Failed Measuring contacts;. ";
            return false;
        }
        else if ( this.TspDevice.ContactSubsystem.ContactCheckOkay.Value )
        {
            details = string.Empty;
            return true;
        }
        else
        {
            details = $"High contact resistances;. Values: '{this.TspDevice.ContactSubsystem.ContactResistances}'";
            return false;
        }
    }

    #endregion

    #region " part measurements "

    /// <summary> Gets or sets the part. </summary>
    /// <remarks>
    /// The part represents the actual device under test. These are the values that gets displayed on
    /// the header and saved.
    /// </remarks>
    /// <value> The part. </value>
    public DeviceUnderTest? Part { get; set; }

    /// <summary> Measures and fetches the initial resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The cold resistance entity where the results are saved. </param>
    public void MeasureInitialResistance( ColdResistance value )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.DisplaySubsystem is null ) throw new NativeException( $"{nameof( this.TspDevice )}.{nameof( this.TspDevice.DisplaySubsystem )} is null." );
        if ( this.Part is null ) throw new NativeException( $"{nameof( this.Part )} is null." );
        if ( this.Part.InitialResistance is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.InitialResistance )} is null." );
        if ( this.InitialResistance is null ) throw new NativeException( $"Meter {nameof( this.InitialResistance )} is null." );

        // clears display if not in measurement state
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing meter display;. " );
        this.TspDevice.DisplaySubsystem.ClearDisplayMeasurement();
        bool contactsOkay;
        if ( this.Part.ContactCheckEnabled )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Checking contacts;. " );
            contactsOkay = this.TryCheckContacts( this.Part.ContactCheckThreshold, out string details );

            this.Part.InitialResistance.ApplyContactCheckOutcome( MeasurementOutcomes.FailedContactCheck );
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( details );
        }
        else
            contactsOkay = true;

        if ( contactsOkay )
        {
            if ( this.TspDevice.Enabled )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Measuring Initial MeasuredValue...;. " );
                this.InitialResistance.Measure( value );
            }
            else
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Emulating Initial MeasuredValue...;. " );
                this.InitialResistance.EmulateResistance( value );
            }

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Got Initial MeasuredValue;. " );
        }
    }

    /// <summary> Measures and fetches the Final resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The cold resistance entity where the results are saved. </param>
    public void MeasureFinalResistance( ColdResistance value )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.DisplaySubsystem is null ) throw new NativeException( $"{nameof( this.TspDevice )}.{nameof( this.TspDevice.DisplaySubsystem )} is null." );
        if ( this.Part is null ) throw new NativeException( $"{nameof( this.Part )} is null." );
        if ( this.Part.FinalResistance is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.FinalResistance )} is null." );
        if ( this.FinalResistance is null ) throw new NativeException( $"Meter {nameof( this.FinalResistance )} is null." );

        if ( this.TspDevice.Enabled )
        {
            // clears display if not in measurement state
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing meter display;. " );
            this.TspDevice.DisplaySubsystem.ClearDisplayMeasurement();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Measuring Final MeasuredValue...;. " );
            this.FinalResistance.Measure( value );
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Emulating Final MeasuredValue...;. " );
            this.FinalResistance.EmulateResistance( value );
        }

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Got Final MeasuredValue;. " );
    }

    /// <summary> Measures and fetches the thermal transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The cold resistance entity where the results are saved. </param>
    public void MeasureThermalTransient( ThermalTransient value )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.DisplaySubsystem is null ) throw new NativeException( $"{nameof( this.TspDevice )}.{nameof( this.TspDevice.DisplaySubsystem )} is null." );
        if ( this.Part is null ) throw new NativeException( $"{nameof( this.Part )} is null." );
        if ( this.Part.ThermalTransient is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.ThermalTransient )} is null." );
        if ( this.Part.MeterMain is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.MeterMain )} is null." );
        if ( this.ThermalTransient is null ) throw new NativeException( $"Meter {nameof( this.ThermalTransient )} is null." );
        if ( this.MeterSubsystem is null ) throw new NativeException( $"Meter {nameof( this.MeterSubsystem )} is null." );

        // clears display if not in measurement state
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing meter display;. " );
        this.TspDevice.DisplaySubsystem.ClearDisplayMeasurement();
        if ( this.TspDevice.Enabled )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Measuring Thermal Transient...;. " );
            this.ThermalTransient.Measure( value );
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Emulating Thermal Transient...;. " );
            this.ThermalTransient.EmulateThermalTransient( value );
        }

        this.MeasureSequencer?.StartFinalResistanceTime( TimeSpan.FromMilliseconds( 1000d * this.Part.MeterMain.PostTransientDelay ) );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Got Thermal Transient;. " );
    }

    #endregion

    #region " sequenced measurements "


    private MeasureSequencer? MeasureSequencerInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.MeasureSequencer_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.MeasureSequencer_PropertyChanged;
        }
    }

    /// <summary>   Gets or sets the sequencer. </summary>
    /// <value> The sequencer. </value>
    public MeasureSequencer? MeasureSequencer
    {
        get => this.MeasureSequencerInternal;
        protected set => this.MeasureSequencerInternal = value;
    }

    /// <summary> Handles the measure sequencer property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( MeasureSequencer sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( Ttm.MeasureSequencer.MeasurementSequenceState ):
                {
                    this.OnMeasurementSequenceStateChanged( sender.MeasurementSequenceState );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _measureSequencer for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void MeasureSequencer_PropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"{this.ResourceName} handling measure sequencer {e.PropertyName} property changed event";
            if ( sender is MeasureSequencer s ) this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Event handler. Called by  for  events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="currentState"> The current state. </param>
    private void OnMeasurementSequenceStateChanged( MeasurementSequenceState currentState )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.DisplaySubsystem is null ) throw new NativeException( $"{nameof( this.TspDevice )}.{nameof( this.TspDevice.DisplaySubsystem )} is null." );
        if ( this.TspDevice.Session is null ) throw new NativeException( $"{nameof( this.TspDevice )}.{nameof( this.TspDevice.Session )} is null." );
        if ( this.MeasureSequencer is null ) throw new NativeException( $"{nameof( this.MeasureSequencer )} is null." );
        if ( this.Part is null ) throw new NativeException( $"{nameof( this.Part )} is null." );
        if ( this.Part.InitialResistance is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.InitialResistance )} is null." );
        if ( this.Part.ThermalTransient is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.ThermalTransient )} is null." );
        if ( this.Part.FinalResistance is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.FinalResistance )} is null." );


        switch ( currentState )
        {
            case MeasurementSequenceState.Idle:
                {
                    // Waiting for the step signal to start.
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Ready to start a measurement sequence;. " );
                    break;
                }

            case MeasurementSequenceState.Aborted:
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Measurement sequence aborted;. " );
                    // step to the idle state.
                    this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Step );
                    break;
                }

            case MeasurementSequenceState.Failed:
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Measurement sequence failed;. " );
                    // step to the aborted state.
                    this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Step );
                    break;
                }

            case MeasurementSequenceState.Completed:
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Measurement sequence completed;. " );

                    // step to the idle state.
                    this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Step );
                    break;
                }

            case MeasurementSequenceState.Starting:
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Measurement sequence starting;. " );
                    this.Part.ClearMeasurements();

                    // step to the Measure Initial MeasuredValue state.
                    this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Step );
                    break;
                }

            case MeasurementSequenceState.MeasureInitialResistance:
                {
                    string activity = string.Empty;
                    try
                    {
                        activity = $"{this.ResourceName} Measuring initial resistance";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                        this.MeasureInitialResistance( this.Part.InitialResistance );
                        if ( this.Part.InitialResistance.MeasurementOutcome == MeasurementOutcomes.PartPassed )
                        {
                            // step to the Measure Thermal Transient.
                            this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Step );
                        }
                        else
                        {
                            // if failure, do not continue.
                            this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Failure );
                        }
                    }
                    catch ( Exception ex )
                    {
                        _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
                        this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Failure );
                    }

                    break;
                }

            case MeasurementSequenceState.MeasureThermalTransient:
                {
                    string activity = string.Empty;
                    try
                    {
                        activity = $"{this.ResourceName} measuring thermal resistance";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                        this.MeasureThermalTransient( this.Part.ThermalTransient );
                        // step to the post transient delay
                        this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Step );
                    }
                    catch ( Exception ex )
                    {
                        _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
                        this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Failure );
                    }

                    break;
                }

            case MeasurementSequenceState.PostTransientPause:
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Delaying final resistance measurement;. " );
                    break;
                }

            case MeasurementSequenceState.MeasureFinalResistance:
                {
                    string activity = string.Empty;
                    try
                    {
                        activity = $"{this.ResourceName} measuring final resistance";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                        this.MeasureFinalResistance( this.Part.FinalResistance );
                        // step to the completed state
                        this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Step );
                    }
                    catch ( Exception ex )
                    {
                        _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
                        this.MeasureSequencer.Enqueue( MeasurementSequenceSignal.Failure );
                    }

                    break;
                }

            default:
                {
                    Debug.Assert( !Debugger.IsAttached, "Unhandled state: " + currentState.ToString() );
                    break;
                }
        }
    }

    #endregion

    #region " trigger sequence "


    private TriggerSequencer? TriggerSequencerInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.TriggerSequencer_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.TriggerSequencer_PropertyChanged;
        }
    }

    /// <summary> Gets or sets the trigger sequencer. </summary>
    /// <value> The sequencer. </value>
    public TriggerSequencer? TriggerSequencer
    {
        get => this.TriggerSequencerInternal;
        set => this.TriggerSequencerInternal = value;
    }

    /// <summary> Handles the trigger sequencer property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( TriggerSequencer sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( Ttm.TriggerSequencer.AssertRequested ):
                {
                    if ( sender.AssertRequested )
                        this.TspDevice?.Session?.AssertTrigger();

                    break;
                }

            case nameof( Ttm.TriggerSequencer.TriggerSequenceState ):
                {
                    this.OnTriggerSequenceStateChanged( sender.TriggerSequenceState );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _triggerSequencer for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void TriggerSequencer_PropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"{this.ResourceName} handling trigger sequence {e?.PropertyName} property changed event";
            if ( sender is TriggerSequencer s ) this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Event handler. Called by  for  events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="currentState"> The current state. </param>
    private void OnTriggerSequenceStateChanged( TriggerSequenceState currentState )
    {
        if ( this.TspDevice is null ) throw new NativeException( $"{nameof( this.TspDevice )} is null." );
        if ( this.TspDevice.Session is null ) throw new NativeException( $"{nameof( this.TspDevice )}.{nameof( this.TspDevice.Session )} is null." );
        if ( this.TriggerSequencer is null ) throw new NativeException( $"{nameof( this.TriggerSequencer )} is null." );
        if ( this.Part is null ) throw new NativeException( $"{nameof( this.Part )} is null." );
        if ( this.Part.InitialResistance is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.InitialResistance )} is null." );
        if ( this.Part.ThermalTransient is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.ThermalTransient )} is null." );
        if ( this.Part.FinalResistance is null ) throw new NativeException( $"{nameof( this.Part )}.{nameof( this.Part.FinalResistance )} is null." );
        switch ( currentState )
        {
            case TriggerSequenceState.Idle:
                {
                    // Waiting for the step signal to start.
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Ready to start a trigger sequence;. " );
                    break;
                }

            case TriggerSequenceState.Aborted:
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Measurement sequence aborted;. " );

                    // step to the idle state.
                    this.TriggerSequencer.Enqueue( TriggerSequenceSignal.Step );
                    break;
                }

            case TriggerSequenceState.Failed:
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Measurement sequence failed;. " );
                    // step to the aborted state.
                    this.TriggerSequencer.Enqueue( TriggerSequenceSignal.Step );
                    break;
                }

            case TriggerSequenceState.Stopped:
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Measurement sequence stopped;. " );

                    // step to the idle state.
                    this.TriggerSequencer.Enqueue( TriggerSequenceSignal.Step );
                    break;
                }

            case TriggerSequenceState.Starting:
                {
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Measurement sequence starting;. " );
                    try
                    {
                        this.PrepareForTrigger();
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Monitoring instrument for measurements;. " );
                        // step to the Waiting state.
                        this.TriggerSequencer.Enqueue( TriggerSequenceSignal.Step );
                    }
                    catch ( Exception ex )
                    {
                        _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Exception occurred preparing instrument for waiting for trigger;. {ex}" );
                        // step to the failed state.
                        this.TriggerSequencer.Enqueue( TriggerSequenceSignal.Failure );
                    }

                    break;
                }

            case TriggerSequenceState.WaitingForTrigger:
                {
                    _ = SessionBase.AsyncDelay( this.TspDevice.Session.StatusReadDelay );
                    ServiceRequests statusByte = this.TspDevice.Session.ReadStatusByte();
                    this.TspDevice.Session.ApplyStatusByte( statusByte );
                    if ( this.TspDevice.Session.IsErrorBitSet( statusByte ) )
                    { }
                    else if ( this.TspDevice.Session.IsMessageAvailableBitSet( statusByte ) )
                    {
                        // step to the reading state.
                        this.TriggerSequencer.Enqueue( TriggerSequenceSignal.Step );
                    }
                    else if ( this.TriggerSequencer.AssertRequested )
                    {
                        this.TriggerSequencer.AssertRequested = false;
                        this.TspDevice.Session.AssertTrigger();
                    }

                    break;
                }

            case TriggerSequenceState.MeasurementCompleted:
                {
                    // clear part measurements.
                    this.Part.ClearMeasurements();

                    // step to the reading state.
                    this.TriggerSequencer.Enqueue( TriggerSequenceSignal.Step );
                    break;
                }

            case TriggerSequenceState.ReadingValues:
                {
                    try
                    {
                        this.ReadMeasurements( this.Part.InitialResistance, this.Part.FinalResistance, this.Part.ThermalTransient );
                        // step to the starting state.
                        this.TriggerSequencer.Enqueue( TriggerSequenceSignal.Step );
                    }
                    catch ( Exception ex )
                    {
                        _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Exception occurred reading measurements;. {ex}" );
                        // step to the failed state.
                        this.TriggerSequencer.Enqueue( TriggerSequenceSignal.Failure );
                    }

                    break;
                }

            default:
                {
                    Debug.Assert( !Debugger.IsAttached, "Unhandled state: " + currentState.ToString() );
                    break;
                }
        }
    }

    #endregion
}
