using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A trigger subsystem user interface. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class TriggerView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public TriggerView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new <see cref="TriggerView"/> </summary>
    /// <returns> A <see cref="TriggerView"/>. </returns>
    public static TriggerView Create()
    {
        TriggerView? view = null;
        try
        {
            view = new TriggerView();
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

    /// <summary> Adds a menu item. </summary>
    /// <param name="item"> The item. </param>
    public void AddMenuItem( ToolStripMenuItem item )
    {
        _ = this._subsystemSplitButton.DropDownItems.Add( item );
    }

    /// <summary> Applies the trigger plan settings. </summary>
    public virtual void ApplySettings()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} applying arm 1 subsystem instrument settings";
            this._armLayer1View.ApplySettings();
            activity = $"{this.Device?.ResourceNameCaption} applying arm 2 subsystem instrument settings";
            this._armLayer2View.ApplySettings();
            activity = $"{this.Device?.ResourceNameCaption} applying trigger subsystem instrument settings";
            this._triggerLayer1View.ApplySettings();
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

    /// <summary> Reads instrument settings. </summary>
    public virtual void ReadSettings()
    {
        if ( this.InitializingComponents ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} reading arm 1 subsystem instrument settings";
            this._armLayer1View.ReadSettings();
            activity = $"{this.Device?.ResourceNameCaption} reading arm 2 subsystem instrument settings";
            this._armLayer2View.ReadSettings();
            activity = $"{this.Device?.ResourceNameCaption} reading trigger subsystem instrument settings";
            this._triggerLayer1View.ReadSettings();
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
    public virtual void Initiate()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing execution state";
            this.Device.ClearExecutionState();
            activity = $"{this.Device?.ResourceNameCaption} initiating trigger plan";
            this._triggerLayer1View.TriggerSubsystem?.StartElapsedStopwatch();
            this._triggerLayer1View.TriggerSubsystem?.Initiate();
            this._triggerLayer1View.TriggerSubsystem?.StopElapsedStopwatch();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
            try
            {
                activity = $"{this.Device?.ResourceNameCaption} aborting trigger plan";
                this._triggerLayer1View.TriggerSubsystem?.Abort();
            }
            catch
            {
                if ( this.Device?.Session is not null )
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

    /// <summary> Aborts the trigger plan. </summary>
    public virtual void Abort()
    {
        if ( this.InitializingComponents ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} aborting trigger plan";
            this._triggerLayer1View.TriggerSubsystem?.StartElapsedStopwatch();
            this._triggerLayer1View.TriggerSubsystem?.Abort();
            this._triggerLayer1View.TriggerSubsystem?.StopElapsedStopwatch();
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

    /// <summary> Sends the bus trigger. </summary>
    public virtual void SendBusTrigger()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} sending bus trigger";
            if ( this.Device?.Session is not null )
                this.Device.Session.AssertTrigger();
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

    #region " custom actions "

    /// <summary> Initiate wait read. </summary>
    public virtual void InitiateWaitRead()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing execution state";
            this.Device.ClearExecutionState();
            activity = $"{this.Device.ResourceNameCaption} initiating wait read";

            // Me.Device.ClearExecutionState()
            // set the service request
            // Me.Device.StatusSubsystem.ApplyMeasurementEventEnableBitmask(MeasurementEvents.All)
            // Me.Device.StatusSubsystem.EnableServiceRequest(Me.Device.Session.OperationServiceRequestEnableBitmask)
            // Me.Device.Session.Write("*SRE 1") ' Set MSB bit of SRE register
            // Me.Device.Session.Write("stat:MEAS:ptr 32767; ntr 0; ENAB 512") ' Set all PTR bits and clear all NTR bits for measurement events Set Buffer Full bit of Measurement
            // Me.Device.Session.Write(":trac:feed calc") ' Select Calculate as reading source
            // Me.Device.Session.Write(":trac:POIN 10")   ' Set buffer size to 10 points
            // Me.Device.Session.Write(":trac:egr full")  ' Select Full element group

            // trigger the initiation of the measurement letting the triggering or service request do the rest.
            activity = $"{this.Device.ResourceNameCaption} Initiating meter";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this._triggerLayer1View.TriggerSubsystem?.Initiate();
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
            this.Device = null;
        }

        this.Device = value;
        this._armLayer1View.AssignDevice( value );
        this._armLayer2View.AssignDevice( value );
        this._triggerLayer1View.AssignDevice( value );
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem">   The subsystem. </param>
    /// <param name="layerNumber"> The layer number. </param>
    protected virtual void BindSubsystem( ArmLayerSubsystemBase subsystem, int layerNumber )
    {
        switch ( layerNumber )
        {
            case 1:
                {
                    this._armLayer1View.BindSubsystem( subsystem, layerNumber );
                    break;
                }

            case 2:
                {
                    this._armLayer2View.BindSubsystem( subsystem, layerNumber );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem">   The subsystem. </param>
    /// <param name="layerNumber"> The layer number. </param>
    protected virtual void BindSubsystem( TriggerSubsystemBase subsystem, int layerNumber )
    {
        this._triggerLayer1View.BindSubsystem( subsystem, layerNumber );
        if ( subsystem is null )
        {
            this._triggerLayer1View.PropertyChanged -= this.TriggerLayer1ViewPropertyChanged;
        }
        else
        {
            this._triggerLayer1View.PropertyChanged += this.TriggerLayer1ViewPropertyChanged;
            this.HandlePropertyChanged( this._triggerLayer1View, nameof( TriggerLayerView.Count ) );
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

    #endregion

    #region " trigger layer 1 view "

    /// <summary> Gets the arm layer 1 source. </summary>
    /// <value> The arm layer 1 source. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ArmSources ArmLayer1Source => this._armLayer1View.Source;

    /// <summary> Gets the arm layer 2 source. </summary>
    /// <value> The arm layer 2 source. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ArmSources ArmLayer2Source => this._armLayer2View.Source;

    /// <summary> Gets the trigger layer source. </summary>
    /// <value> The trigger layer source. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public TriggerSources TriggerLayerSource => this._triggerLayer1View.Source;

    /// <summary> Gets the number of triggers. </summary>
    /// <value> The number of triggers. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public int TriggerCount => this._triggerLayer1View.Count;

    /// <summary> Handle the TriggerLayer1 view property changed event. </summary>
    /// <param name="view">         The view. </param>
    /// <param name="propertyName"> Name of the property. </param>
    protected virtual void HandlePropertyChanged( TriggerLayerView view, string? propertyName )
    {
        if ( view is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( TriggerLayerView.Source ):
                {
                    this._sendBusTriggerButton.Enabled = view.Source == TriggerSources.Bus;
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> TriggerLayer1 view property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void TriggerLayer1ViewPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( TriggerLayerView )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.TriggerLayer1ViewPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.TriggerLayer1ViewPropertyChanged ), [sender, e] );

            else if ( sender is TriggerLayerView s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " control event handlers "

    /// <summary> Applies the settings tool strip menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplySettingsToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ApplySettings();
    }

    /// <summary> Reads settings tool strip menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadSettingsToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ReadSettings();
    }

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

    #endregion
}
