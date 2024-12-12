using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A scanner view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class ScannerView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public ScannerView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new <see cref="ScannerView"/> </summary>
    /// <returns> A <see cref="ScannerView"/>. </returns>
    public static ScannerView Create()
    {
        ScannerView? view = null;
        try
        {
            view = new ScannerView();
            return view;
        }
        catch
        {
            view?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control" />
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources;
    /// <c>false</c> to release only unmanaged
    /// resources when called from the runtime
    /// finalize. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
                this.InitializingComponents = true;
                // make sure the device is unbound in case the form is closed without closing the device.
                this.AssignDeviceThis( null );
                if ( this.components is not null )
                {
                    this.components?.Dispose();
                    this.components = null;
                }
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " public members "

    /// <summary> Aborts the trigger plan. </summary>
    public void Abort()
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TriggerSubsystem.StartElapsedStopwatch();
            this.TriggerSubsystem.Abort();
            this.TriggerSubsystem.StopElapsedStopwatch();
            _ = this.Device.Session?.QueryOperationCompleted();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Initiates the trigger plan. </summary>
    public void Initiate()
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing execution state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            _ = this.Device.Session?.QueryOperationCompleted();
            activity = $"{this.Device.ResourceNameCaption} initiating trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TriggerSubsystem.StartElapsedStopwatch();
            this.TriggerSubsystem.Initiate();
            this.TriggerSubsystem.StopElapsedStopwatch();
        }
        catch ( Exception ex )
        {
            if ( this.Device.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
            try
            {
                activity = $"{this.Device.ResourceNameCaption} aborting trigger plan";
                this.TriggerSubsystem.Abort();
            }
            catch
            {
                if ( this.Device.Session is not null )
                    this.Device.Session.StatusPrompt = $"failed {activity}";
                activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
                _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
            }
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Sends the bus trigger. </summary>
    public void SendBusTrigger()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} sending bus trigger";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session?.AssertTrigger();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Builds bus trigger description. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public string BuildBusTriggerDescription()
    {
        if ( this.InitializingComponents ) return string.Empty;
        System.Text.StringBuilder builder = new();
        _ = builder.AppendLine( $"Scan plan with bus triggering:" );
        _ = builder.AppendLine( $"Scan list: {this._scanListView.ScanList}" );
        _ = builder.AppendLine( $"Arm layer 1: Count {1} {ArmSources.Immediate} delay {0}" );
        _ = builder.AppendLine( $"Arm layer 2: Count {1} {ArmSources.Immediate} delay {0}" );
        _ = builder.AppendLine( $"Trigger layer: Source {TriggerSources.Bus} count {9999} delay {0} bypass {TriggerLayerBypassModes.Acceptor}" );
        return builder.ToString();
    }

    /// <summary> Configure bus trigger plan. </summary>
    public void ConfigureBusTriggerPlan()
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null ||
            this.ArmLayer1Subsystem is null || this.ArmLayer2Subsystem is null || this.RouteSubsystem is null ) return;
        string title = this.Device.OpenResourceModel;
        string propertyName = string.Empty;
        string activity = $"{title} Configuring bus scan trigger plan";
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            activity = $"{title} aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TriggerSubsystem.Abort();
            _ = this.Device.Session?.QueryOperationCompleted();
            activity = $"{title} clearing execution state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            _ = this.Device.Session?.QueryOperationCompleted();
            propertyName = $"{nameof( TriggerSubsystemBase ).SplitWords()}.{nameof( TriggerSubsystemBase.ContinuousEnabled ).SplitWords()}";
            activity = $"{title} setting [{propertyName}]=False";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            _ = this.TriggerSubsystem.ApplyContinuousEnabled( false );
            this.TriggerSubsystem.Abort();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            _ = this.RouteSubsystem.ApplyOpenAll( TimeSpan.FromSeconds( 1d ) );
            _ = this.RouteSubsystem.QueryClosedChannels();
            _ = this.RouteSubsystem.ApplyScanList( this._scanListView.ScanList );
            _ = this.ArmLayer1Subsystem.ApplyArmSource( ArmSources.Immediate );
            _ = this.ArmLayer1Subsystem.ApplyArmCount( 1 );
            _ = this.ArmLayer1Subsystem.ApplyArmLayerBypassMode( TriggerLayerBypassModes.Acceptor );
            _ = this.ArmLayer2Subsystem.ApplyArmSource( ArmSources.Immediate );
            _ = this.ArmLayer2Subsystem.ApplyArmCount( 1 );
            _ = this.ArmLayer2Subsystem.ApplyDelay( TimeSpan.Zero );
            _ = this.TriggerSubsystem.ApplyTriggerSource( TriggerSources.Bus );
            _ = this.TriggerSubsystem.ApplyTriggerCount( 9999 ); // in place of infinite
            _ = this.TriggerSubsystem.ApplyDelay( TimeSpan.Zero );
            _ = this.TriggerSubsystem.ApplyTriggerLayerBypassMode( TriggerLayerBypassModes.Acceptor );
            if ( this.Device.Session != null )
                this.Device.Session.StatusPrompt = "Ready: Initiate (1) Meter; (2) 7001r";
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Builds external trigger description. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public string BuildExternalTriggerDescription()
    {
        if ( this.InitializingComponents )
            return string.Empty;
        System.Text.StringBuilder builder = new();
        _ = builder.AppendLine( "Scan plan with external triggering handshake with a meter:" );
        _ = builder.AppendLine( $"Scan list: {this._scanListView.ScanList}" );
        _ = builder.AppendLine( $"Arm layer 1: Count {1} {ArmSources.Immediate} delay {0}" );
        _ = builder.AppendLine( $"Arm layer 2: Count {1} {ArmSources.Immediate} delay {0}" );
        _ = builder.AppendLine( $"Trigger layer: Source {TriggerSources.External} count {9999} delay {0} bypass {TriggerLayerBypassModes.Source}" );
        return builder.ToString();
    }

    /// <summary> Configure external trigger plan. </summary>
    public void ConfigureExternalTriggerPlan()
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null
            || this.RouteSubsystem is null || this.ArmLayer1Subsystem is null || this.ArmLayer2Subsystem is null ) return;
        string activity = $"{this.Device.ResourceNameCaption} Configuring external scan trigger plan";
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            this.Device.ClearExecutionState();
            // enable service requests
            // Me.EnableServiceRequestEventHandler()
            // Me.Device.StatusSubsystem.EnableServiceRequest(Me.Device.Session.ServiceRequestEnableBitmask)

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            _ = this.TriggerSubsystem.ApplyContinuousEnabled( false );
            this.TriggerSubsystem.Abort();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            _ = this.RouteSubsystem.ApplyOpenAll( TimeSpan.FromSeconds( 1d ) );
            _ = this.RouteSubsystem.QueryClosedChannels();
            _ = this.RouteSubsystem.ApplyScanList( this._scanListView.ScanList ); // "(@1!1:1!10)"
            _ = this.ArmLayer1Subsystem.ApplyArmSource( ArmSources.Immediate );
            _ = this.ArmLayer1Subsystem.ApplyArmCount( 1 );
            _ = this.ArmLayer2Subsystem.ApplyArmSource( ArmSources.Immediate );
            _ = this.ArmLayer2Subsystem.ApplyArmCount( 1 );
            _ = this.ArmLayer2Subsystem.ApplyDelay( TimeSpan.Zero );
            _ = this.TriggerSubsystem.ApplyTriggerSource( TriggerSources.External );
            _ = this.TriggerSubsystem.ApplyTriggerCount( 9999 ); // in place of infinite
            _ = this.TriggerSubsystem.ApplyDelay( TimeSpan.Zero );
            _ = this.TriggerSubsystem.ApplyTriggerLayerBypassMode( TriggerLayerBypassModes.Source );
            if ( this.Device.Session != null )
                this.Device.Session.StatusPrompt = "Ready: Initiate (1) 7001; (2) Meter";
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " device "

    /// <summary> The device. </summary>

    /// <summary> Gets the device. </summary>
    /// <value> The device. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? Device { get; private set; }

    /// <summary> Assigns the device and binds the relevant subsystem values. </summary>
    /// <param name="value"> The value. </param>
    [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
    private void AssignDeviceThis( VisaSessionBase? value )
    {
        if ( this.Device is not null )
        {
            this.Device.PropertyChanged -= this.VisaSessionBasePropertyChanged;
            this.Device = null;
        }

        this.Device = value;
        this._slotView.AssignDevice( value );
        this._serviceRequestView.AssignDevice( value );
        this._scanListView.AssignDevice( value );
        if ( this.Device is not null )
        {
            this.Device.PropertyChanged += this.VisaSessionBasePropertyChanged;
            this.HandlePropertyChanged( this.Device, nameof( VisaSessionBase.OpenResourceModel ) );
        }
    }

    /// <summary> Assigns a device. </summary>
    /// <param name="value"> True to show or False to hide the control. </param>
    public void AssignDevice( VisaSessionBase? value )
    {
        this.AssignDeviceThis( value );
    }

    /// <summary> Reads the status register and lets the session process the status byte. </summary>
    protected void ReadStatusRegister()
    {
        if ( this.Device is null || this.Device.Session is null ) return;
        string activity = $"{this.Device.ResourceNameCaption} reading service request";
        try
        {
            this.Device.Session.ApplyStatusByte( this.Device.Session.ReadStatusByte() );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Handle the visa session base property changed event. </summary>
    /// <param name="device">       The visa session base. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( VisaSessionBase device, string? propertyName )
    {
        if ( device is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( VisaSessionBase.OpenResourceModel ):
                {
                    this._subsystemSplitButton.Text = device.OpenResourceModel;
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> visa session base  property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void VisaSessionBasePropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( VisaSessionBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.VisaSessionBasePropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.VisaSessionBasePropertyChanged ), [sender, e] );

            else if ( sender is VisaSessionBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " relay view "

    /// <summary> Handle the relay view property changed event. </summary>
    /// <param name="view">         The view. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( RelayView view, string? propertyName )
    {
        if ( view is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( RelayView.ChannelList ):
                {
                    this._infoTextBox.Text = $"Channel List: {view.ChannelList}";
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Relay view property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void RelayViewPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( this.RouteSubsystem )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.RelayViewPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.RelayViewPropertyChanged ), [sender, e] );

            else if ( sender is RelayView s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " scan list view "

    /// <summary> Handle the ScanList view property changed event. </summary>
    /// <param name="view">         The view. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( ScanListView view, string? propertyName )
    {
        if ( view is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( ScanListView.ScanList ):
                {
                    this._infoTextBox.Text = $"Scan List: {view.ScanList}";
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> ScanList view property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void ScanListViewPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( ScanListView )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.ScanListViewPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.ScanListViewPropertyChanged ), [sender, e] );

            else if ( sender is ScanListView s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " route subsystem "

    /// <summary> Gets or sets the route subsystem. </summary>
    /// <value> The route  subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public RouteSubsystemBase? RouteSubsystem { get; private set; }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected virtual void BindSubsystem( RouteSubsystemBase subsystem )
    {
        this._slotView.BindSubsystem( subsystem );
        this._scanListView.BindSubsystem( subsystem );
        if ( subsystem is null )
        {
            this.RouteSubsystem = null;
            this._relayView.PropertyChanged -= this.RelayViewPropertyChanged;
            this._scanListView.PropertyChanged -= this.ScanListViewPropertyChanged;
        }
        else
        {
            this.RouteSubsystem = subsystem;
            this._relayView.PropertyChanged += this.RelayViewPropertyChanged;
            this._scanListView.PropertyChanged += this.ScanListViewPropertyChanged;
            this.HandlePropertyChanged( this._relayView, nameof( RelayView.ChannelList ) );
        }
    }

    #endregion

    #region " arm layer subsystems "

    /// <summary> Gets or sets the arm layer 1 subsystem. </summary>
    /// <value> The arm layer 1 subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ArmLayerSubsystemBase? ArmLayer1Subsystem { get; private set; }

    /// <summary> Gets or sets the arm layer 2 subsystem. </summary>
    /// <value> The arm layer 2 subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ArmLayerSubsystemBase? ArmLayer2Subsystem { get; private set; }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem">   The subsystem. </param>
    /// <param name="layerNumber"> The layer number. </param>
    protected void BindSubsystem( ArmLayerSubsystemBase subsystem, int layerNumber )
    {
        if ( layerNumber == 1 )
            this.BindSubsystem1( subsystem );

        else
            this.BindSubsystem2( subsystem );
    }

    /// <summary> Bind subsystem 1. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected void BindSubsystem1( ArmLayerSubsystemBase subsystem )
    {
        if ( this.ArmLayer1Subsystem is not null )
            this.ArmLayer1Subsystem = null;

        this.ArmLayer1Subsystem = subsystem;
        if ( this.ArmLayer1Subsystem is not null )
        {
        }
    }

    /// <summary> Bind subsystem 2. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected void BindSubsystem2( ArmLayerSubsystemBase subsystem )
    {
        if ( this.ArmLayer2Subsystem is not null )
            this.ArmLayer2Subsystem = null;

        this.ArmLayer2Subsystem = subsystem;
        if ( this.ArmLayer2Subsystem is not null )
        {
        }
    }

    #endregion

    #region " trigger subsystem "

    /// <summary> Gets or sets the trigger subsystem. </summary>
    /// <value> The trigger subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public TriggerSubsystemBase? TriggerSubsystem { get; private set; }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected void BindSubsystem( TriggerSubsystemBase subsystem )
    {
        if ( this.TriggerSubsystem is not null )
        {
            this.TriggerSubsystem = null;
        }

        this.TriggerSubsystem = subsystem;
        if ( this.TriggerSubsystem is not null )
        {
        }
    }

    #endregion

    #region " control event handlers "

    /// <summary> Initiate click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void InitiateButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.Initiate();
    }

    /// <summary> Abort button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AbortButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.Abort();
    }

    /// <summary> Sends the bus trigger button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void SendBusTriggerButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.SendBusTrigger();
    }

    /// <summary> Explain external trigger plan menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ExplainExternalTriggerPlanMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"{this.Device?.ResourceNameCaption} building external scan trigger plan description";
        string description = string.Empty;
        try
        {
            description = this.BuildExternalTriggerDescription();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this._infoTextBox.Text = description;
        }
    }

    /// <summary> Configure external trigger plan menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ConfigureExternalTriggerPlanMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ConfigureExternalTriggerPlan();
    }

    /// <summary> Explain bus trigger plan menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ExplainBusTriggerPlanMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"{this.Device?.ResourceNameCaption} building bus trigger plan description";
        string description = string.Empty;
        try
        {
            description = this.BuildBusTriggerDescription();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this._infoTextBox.Text = description;
        }
    }

    /// <summary> Configure bus trigger plan menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ConfigureBusTriggerPlanMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ConfigureBusTriggerPlan();
    }

    #endregion
}
