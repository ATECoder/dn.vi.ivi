using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;
using cc.isr.WinControls.CheckStateExtensions;
using cc.isr.VI.SubsystemsWinControls.DataGridViewExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A buffer stream trigger monitor view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
[Obsolete( "Work in progress" )]
public partial class BufferStreamMonitorView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public BufferStreamMonitorView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._bufferSizeNumeric.NumericUpDown.CausesValidation = true;
        this._bufferSizeNumeric.NumericUpDown.Minimum = 0m;
        this._bufferSizeNumeric.NumericUpDown.Maximum = 27500000m;
    }

    /// <summary> Creates a new <see cref="BufferStreamMonitorView"/> </summary>
    /// <returns> A <see cref="BufferStreamMonitorView"/>. </returns>
    public static BufferStreamMonitorView Create()
    {
        BufferStreamMonitorView? view = null;
        try
        {
            view = new BufferStreamMonitorView();
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

    #region " public members: buffer "

    /// <summary> Applies the buffer capacity. </summary>
    /// <param name="capacity"> The capacity. </param>
    public void ApplyBufferCapacity( int capacity )
    {
        if ( this.Device is null || this.Device.Session is null ) return;
        string activity = string.Empty;
        try
        {
            if ( this.Device.IsDeviceOpen )
            {
                if ( this.BufferSubsystem is not null )
                {
                    activity = $"{this.Device.ResourceNameCaption} setting {typeof( TraceSubsystemBase )}.{nameof( BufferSubsystemBase.Capacity )} to {capacity}";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( activity );
                    ApplyBufferCapacity( this.BufferSubsystem, capacity );
                }
                else if ( this.TraceSubsystem is not null )
                {
                    activity = $"{this.Device.ResourceNameCaption} setting {typeof( TraceSubsystemBase )}.{nameof( TraceSubsystemBase.PointsCount )} to {capacity}";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( activity );
                    ApplyBufferCapacity( this.TraceSubsystem, capacity );
                }
            }
        }
        catch ( Exception ex )
        {
            this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
    }

    /// <summary> Reads the buffer. </summary>
    public void ReadBuffer()
    {
        if ( this.InitializingComponents || this.Device is null || this.Device.Session is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} reading buffer";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.BufferSubsystem is not null )
            {
                this.ReadBuffer( this.BufferSubsystem );
            }
            else if ( this.TraceSubsystem is not null )
            {
                this.ReadBuffer( this.TraceSubsystem );
            }
        }
        catch ( Exception ex )
        {
            this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Reads the given subsystem. </summary>
    public void Read()
    {
        if ( this.InitializingComponents || this.Device is null || this.Device.Session is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} querying terminal mode";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null )
            {
                ReadTerminalsState( this.MultimeterSubsystem );
            }
            else if ( this.SystemSubsystem is not null )
            {
                ReadSettings( this.SystemSubsystem );
                // Me.ApplyPropertyChanged(Me.SystemSubsystem)
            }

            activity = $"{this.Device.ResourceNameCaption} measuring";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null )
            {
                Read( this.MultimeterSubsystem );
            }
        }
        catch ( Exception ex )
        {
            this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " public members: buffer display "

    /// <summary> Gets or sets the data grid view. </summary>
    /// <value> The data grid view. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public DataGridView? DataGridView { get; set; }

    /// <summary> Gets the buffer readings. </summary>
    /// <value> The buffer readings. </value>
    private BufferReadingBindingList? BufferReadings { get; set; } = [];

    /// <summary> Displays a buffer readings. </summary>
    public void DisplayBufferReadings()
    {
        if ( this.InitializingComponents || this.BufferSubsystem is null || this.BufferReadings is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} reading buffer";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.BufferSubsystem.StartElapsedStopwatch();
            this.ClearBufferDisplay();
            this.BufferReadings.Add( this.BufferSubsystem.QueryBufferReadings() );
            this.BufferSubsystem.LastReading = this.BufferReadings.LastReading;
            this.BufferSubsystem.StopElapsedStopwatch();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Clears the buffer display. </summary>
    public void ClearBufferDisplay()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} clearing buffer display";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.BufferReadings is null )
            {
                this.BufferReadings = [];
                _ = (this.DataGridView?.Bind( this.BufferReadings, true ));
            }

            this.BufferReadings.Clear();
        }
        catch ( Exception ex )
        {
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
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
        if ( value is not null )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( BufferStreamMonitorView ).SplitWords()}" );
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

    #region " buffer subsystem "

    /// <summary> Gets or sets the Buffer subsystem. </summary>
    /// <value> The Buffer subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public BufferSubsystemBase? BufferSubsystem { get; private set; }

    /// <summary> Bind Buffer subsystem. </summary>
    /// <param name="subsystem">  The subsystem. </param>
    /// <param name="bufferName"> Name of the buffer. </param>
    public void BindSubsystem( BufferSubsystemBase subsystem, string bufferName )
    {
        if ( this.BufferSubsystem is not null )
        {
            this.BindSubsystem( false, this.BufferSubsystem );
            this.BufferSubsystem = null;
        }

        this.BufferSubsystem = subsystem;
        if ( this.BufferSubsystem is not null )
        {
            this.BindSubsystem( true, this.BufferSubsystem );
            this._subsystemSplitButton.Text = bufferName;
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, BufferSubsystemBase subsystem )
    {
        if ( this.DataGridView is null ) return;
        if ( add )
            _ = this.DataGridView.Bind( subsystem.BufferReadingsBindingList, true );
        Binding binding = this.AddRemoveBinding( this._bufferSizeNumeric.NumericUpDown, add, nameof( NumericUpDown.Value ), subsystem, nameof( BufferSubsystemBase.Capacity ) );
        // has to apply the value.
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        _ = this.AddRemoveBinding( this._bufferCountLabel, add, nameof( Control.Text ), subsystem, nameof( BufferSubsystemBase.ActualPointCount ) );
        _ = this.AddRemoveBinding( this._firstPointNumberLabel, add, nameof( Control.Text ), subsystem, nameof( BufferSubsystemBase.FirstPointNumber ) );
        _ = this.AddRemoveBinding( this._lastPointNumberLabel, add, nameof( Control.Text ), subsystem, nameof( BufferSubsystemBase.LastPointNumber ) );
        // must Not read setting when biding because the instrument may be locked Or in a trigger mode
        // The bound values should be sent when binding Or when applying property change.
        // ReadSettings( subsystem );
        // todo: Implement this: Me.ApplyPropertyChanged(Subsystem)
    }

    /// <summary> Applies the buffer capacity. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    /// <param name="capacity">  The capacity. </param>
    private static void ApplyBufferCapacity( BufferSubsystemBase subsystem, int capacity )
    {
        // overrides and set to the minimum size: Me._traceSizeNumeric.Value = Me._traceSizeNumeric.NumericUpDown.Minimum
        subsystem.StartElapsedStopwatch();
        _ = subsystem.ApplyCapacity( capacity );
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private static void ReadSettings( BufferSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryCapacity();
        _ = subsystem.QueryFirstPointNumber();
        _ = subsystem.QueryLastPointNumber();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Reads the buffer. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ReadBuffer( BufferSubsystemBase subsystem )
    {
        if ( this.InitializingComponents || this.BufferReadings is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} reading buffer";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            subsystem.StartElapsedStopwatch();
            this.ClearBufferDisplay();
            this.BufferReadings.Add( subsystem.QueryBufferReadings() );
            subsystem.LastReading = this.BufferReadings.LastReading;
            subsystem.StopElapsedStopwatch();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " trace subsystem "

    /// <summary> Gets or sets the Trace subsystem. </summary>
    /// <value> The Trace subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public TraceSubsystemBase? TraceSubsystem { get; private set; }

    /// <summary> Bind Trace subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( TraceSubsystemBase subsystem )
    {
        if ( this.TraceSubsystem is not null )
        {
            this.BindSubsystem( false, this.TraceSubsystem );
            this.TraceSubsystem = null;
        }

        this.TraceSubsystem = subsystem;
        if ( this.TraceSubsystem is not null )
        {
            this._subsystemSplitButton.Text = "Trace";
            this.BindSubsystem( true, this.TraceSubsystem );
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, TraceSubsystemBase subsystem )
    {
        if ( this.DataGridView is null ) return;
        if ( add )
            _ = this.DataGridView.Bind( subsystem.BufferReadingsBindingList, true );
        Binding binding = this.AddRemoveBinding( this._bufferSizeNumeric.NumericUpDown, add, nameof( NumericUpDown.Value ), subsystem, nameof( TraceSubsystemBase.PointsCount ) );
        // has to apply the value.
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        _ = this.AddRemoveBinding( this._bufferCountLabel, add, nameof( Control.Text ), subsystem, nameof( TraceSubsystemBase.ActualPointCount ) );
        this._firstPointNumberLabel.Visible = false;
        this._lastPointNumberLabel.Visible = false;
        // must Not read setting when biding because the instrument may be locked Or in a trigger mode
        // The bound values should be sent when binding Or when applying property change.
        // ReadSettings( subsystem );
        // todo: Implement this: this.ApplyPropertyChanged( subsystem );
    }

    /// <summary> Applies the buffer capacity. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    /// <param name="capacity">  The capacity. </param>
    private static void ApplyBufferCapacity( TraceSubsystemBase subsystem, int capacity )
    {
        // overrides and set to the minimum size: Me._traceSizeNumeric.Value = Me._traceSizeNumeric.NumericUpDown.Minimum
        subsystem.StartElapsedStopwatch();
        _ = subsystem.ApplyPointsCount( capacity );
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private static void ReadSettings( TraceSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryPointsCount();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Reads the buffer. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ReadBuffer( TraceSubsystemBase subsystem )
    {
        if ( this.BufferReadings is null ) return;
        subsystem.StartElapsedStopwatch();
        this.ClearBufferDisplay();
        this.BufferReadings.Add( subsystem.QueryBufferReadings() );
        subsystem.LastBufferReading = this.BufferReadings.LastReading;
        subsystem.StopElapsedStopwatch();
    }

    #endregion

    #region " mutlimeter "

    /// <summary> Gets or sets the Multimeter subsystem. </summary>
    /// <value> The Multimeter subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public MultimeterSubsystemBase? MultimeterSubsystem { get; private set; }

    /// <summary> Bind Multimeter subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindMultimeterSubsystem( MultimeterSubsystemBase subsystem )
    {
        if ( this.MultimeterSubsystem is not null )
        {
            this.MultimeterSubsystem = null;
        }

        this.MultimeterSubsystem = subsystem;
        if ( this.MultimeterSubsystem is not null )
        {
        }
    }

    /// <summary> Reads terminals state. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadTerminalsState( MultimeterSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryFrontTerminalsSelected();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Reads the given subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void Read( MultimeterSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.MeasurePrimaryReading();
        subsystem.StopElapsedStopwatch();
    }

    #endregion

    #region " trigger "

    /// <summary> Gets or sets the Trigger subsystem. </summary>
    /// <value> The Trigger subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public TriggerSubsystemBase? TriggerSubsystem { get; private set; }

    /// <summary> Bind Trigger subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private void BindTriggerSubsystem( TriggerSubsystemBase? subsystem )
    {
        if ( this.TriggerSubsystem is not null )
        {
            this.BindSubsystem( false, this.TriggerSubsystem );
            this.TriggerSubsystem = null;
        }

        this.TriggerSubsystem = subsystem;
        if ( this.TriggerSubsystem is not null )
            this.BindSubsystem( true, this.TriggerSubsystem );
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, TriggerSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.TriggerSubsystemPropertyChanged;
            // BufferStreamMonitorView.ReadSettings(subsystem)
            this.ApplyPropertyChanged( subsystem );
        }
        else
            subsystem.PropertyChanged -= this.TriggerSubsystemPropertyChanged;
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( TriggerSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.TriggerState ) );
    }

    /// <summary> Handle the Trigger subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( TriggerSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( TriggerSubsystemBase.TriggerState ):
                {
                    // todo: Bind trigger state label caption to the Subsystem reading label in the display view.
                    // todo: Move functionality (e.g., handler trigger plan state change) from the user interface to the subsystem
                    this._triggerStateLabel.Visible = subsystem.TriggerState.HasValue;
                    if ( subsystem.TriggerState.HasValue )
                    {
                        this._triggerStateLabel.Text = subsystem.TriggerState.Value.ToString();
                        if ( this.TriggerPlanStateChangeHandlerEnabled )
                        {
                            this.HandleTriggerPlanStateChange( subsystem.TriggerState.Value );
                        }

                        if ( this.BufferStreamingHandlerEnabled && !subsystem.IsTriggerStateActive() )
                        {
                            this.ToggleBufferStream();
                        }
                    }

                    break;
                }

            default:
                break;
                // ?? this causes a cross thread exception.
                // Me._triggerStateLabel.Invalidate()
        }
    }

    /// <summary> Trigger subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void TriggerSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( TriggerSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.TriggerSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.TriggerSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is TriggerSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Initiate trigger plan. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private void InitiateTriggerPlan()
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing execution state";
            this.Device.ClearExecutionState();
            activity = $"{this.Device?.ResourceNameCaption} initiating trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TriggerSubsystem.StartElapsedStopwatch();
            this.TriggerSubsystem.Initiate();
            _ = this.TriggerSubsystem.QueryTriggerState();
            this.TriggerSubsystem.StopElapsedStopwatch();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Aborts trigger plan. </summary>
    public void AbortTriggerPlan()
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} Aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TriggerSubsystem.Abort();
            _ = this.Device.Session?.QueryOperationCompleted();
            _ = this.TriggerSubsystem.QueryTriggerState();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " system subsystem "

    /// <summary> Gets or sets the System subsystem. </summary>
    /// <value> The System subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public SystemSubsystemBase? SystemSubsystem { get; private set; }

    /// <summary> Bind System subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( SystemSubsystemBase subsystem )
    {
        if ( this.SystemSubsystem is not null )
        {
            this.SystemSubsystem = null;
        }

        this.SystemSubsystem = subsystem;
        if ( this.SystemSubsystem is not null )
        {
            // must Not read setting when biding because the instrument may be locked Or in a trigger mode
            // The bound values should be sent when binding Or when applying property change.
            // ReadSettings( subsystem );
            // todo: Implement this: Me.ApplyPropertyChanged(Subsystem)
        }
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( SystemSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryFrontTerminalsSelected();
        subsystem.StopElapsedStopwatch();
    }

    #endregion

    #region " control event handlers: stream buffer "

    /// <summary> Gets or sets the buffer streaming handler enabled. </summary>
    /// <value> The buffer streaming handler enabled. </value>
    private bool BufferStreamingHandlerEnabled { get; set; }

    /// <summary> Starts or stop buffer stream. </summary>
    public void ToggleBufferStream()
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null || this.DataGridView is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            if ( this.BufferStreamingHandlerEnabled )
            {
                this.BufferStreamingHandlerEnabled = false;
                activity = $"{this.Device.ResourceNameCaption} Aborting trigger plan to stop buffer streaming";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.AbortTriggerPlan();
                _ = this.TriggerSubsystem.QueryTriggerState();
            }
            else
            {
                activity = $"{this.Device.ResourceNameCaption} clearing execution state";
                this.Device.ClearExecutionState();
                this.BufferStreamingHandlerEnabled = false;
                activity = $"{this.Device?.ResourceNameCaption} start buffer streaming";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.DataGridView.DataSource = null;
                this.TriggerSubsystem.Initiate();
                this.BufferSubsystem?.StartBufferStream( this.TriggerSubsystem, TimeSpan.FromMilliseconds( 5d ) );

                this.BufferStreamingHandlerEnabled = true;
            }

            this._toggleBufferStreamMenuItem.Checked = this.BufferStreamingHandlerEnabled;
        }
        catch ( Exception ex )
        {
            _ = (this.Device.Session?.StatusPrompt = $"failed {activity}");
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Stream buffer menu item check state changed. </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ToggleBufferStreamMenuItem_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripMenuItem menuItem )
            menuItem.Text = $"{menuItem.CheckState.ToCheckStateCaption( "Stop", "Start", "?" )} Buffer Stream";
    }

    /// <summary> Toggle buffer stream menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ToggleBufferStreamMenuItem_Click( object? sender, EventArgs e )
    {
        if ( sender is not ToolStripMenuItem menuItem ) return;
        try
        {
            menuItem.Enabled = false;
            this.ToggleBufferStream();
        }
        catch
        {
        }
        finally
        {
            menuItem.Enabled = true;
        }
    }

    #endregion

    #region " trigger monitor buffer streaming "

    /// <summary> Values that represent trigger plan states. </summary>
    private enum TriggerPlanState
    {
        /// <summary> An enum constant representing the none option. </summary>
        None,

        /// <summary> An enum constant representing the started option. </summary>
        Started,

        /// <summary> An enum constant representing the completed option. </summary>
        Completed
    }

    /// <summary> Gets or sets the trigger plan state change handler enabled. </summary>
    /// <value> The trigger plan state change handler enabled. </value>
    private bool TriggerPlanStateChangeHandlerEnabled { get; set; }

    /// <summary> Gets or sets the state of the local trigger plan. </summary>
    /// <value> The local trigger plan state. </value>
    private TriggerPlanState LocalTriggerPlanState { get; set; }

    /// <summary> Handles the trigger plan state change described by triggerState. </summary>
    /// <param name="triggerState"> State of the trigger. </param>
    private void HandleTriggerPlanStateChange( TriggerState triggerState )
    {
        if ( triggerState is TriggerState.Running or TriggerState.Waiting )
        {
            this.LocalTriggerPlanState = TriggerPlanState.Started;
        }
        else if ( triggerState == TriggerState.Idle && this.LocalTriggerPlanState == TriggerPlanState.Started )
        {
            this.LocalTriggerPlanState = TriggerPlanState.Completed;
            this.TryReadNewBufferReadings();
            if ( this._repeatMenuItem.Checked )
            {
                this.InitiateMonitorTriggerPlan( true );
            }
        }
        else
        {
            this.LocalTriggerPlanState = TriggerPlanState.None;
        }
    }

    /// <summary> Try read buffer. </summary>
    private void TryReadNewBufferReadings()
    {
        string activity = $"{this.Device?.ResourceNameCaption} reading";
        try
        {
            this.ReadNewBufferReadings();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
        }
    }

    /// <summary> Reads the buffer. </summary>
    private void ReadNewBufferReadings()
    {
        if ( this.BufferSubsystem is null || this.BufferReadings is null ) return;
        string activity = $"{this.Device?.ResourceNameCaption} fetching buffer count";
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
        // this assume buffer is cleared upon each new cycle
        int newBufferCount = this.BufferSubsystem.QueryActualPointCount().GetValueOrDefault( 0 );
        activity = $"buffer count {newBufferCount}";
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
        if ( newBufferCount > 0 )
        {
            activity = $"{this.Device?.ResourceNameCaption} fetching buffered readings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.BufferSubsystem.StartElapsedStopwatch();
            this.BufferReadings.Add( this.BufferSubsystem.QueryBufferReadings() );
            this.BufferSubsystem.StopElapsedStopwatch();
        }
    }

    /// <summary> Initiate monitor trigger plan. </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    /// <param name="stateChangeHandlingEnabled"> True to enable, false to disable the state change
    /// handling. </param>
    private void InitiateMonitorTriggerPlan( bool stateChangeHandlingEnabled )
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null ) return;
        string activity = $"{this.Device.ResourceNameCaption} clearing execution state";
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
        this.Device.ClearExecutionState();
        activity = $"{this.Device?.ResourceNameCaption} Initiating trigger plan and monitor";
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
        this.TriggerSubsystem.Initiate();
        this.ClearBufferDisplay();
        this.TriggerPlanStateChangeHandlerEnabled = stateChangeHandlingEnabled;
        _ = this.TriggerSubsystem.AsyncMonitorTriggerState( TimeSpan.FromMilliseconds( 5d ) );
    }

    /// <summary> Try restart trigger plan. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private void TryRestartTriggerPlan()
    {
        string activity = $"{this.Device?.ResourceNameCaption} Initiating trigger plan and monitor";
        try
        {
            this.InitiateMonitorTriggerPlan( true );
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
        }
    }

    /// <summary> Monitor active trigger plan menu item click. </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void MonitorActiveTriggerPlanMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.Device is null || this.TriggerSubsystem is null ) return;
        string activity = $"{this.Device?.ResourceNameCaption} start monitoring trigger plan";
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            this.TriggerPlanStateChangeHandlerEnabled = false;
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            _ = this.TriggerSubsystem.AsyncMonitorTriggerState( TimeSpan.FromMilliseconds( 5d ) );
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Initializes the monitor read repeat menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void InitMonitorReadRepeatMenuItem_Click( object? sender, EventArgs e )
    {
        string activity = $"{this.Device?.ResourceNameCaption} Initiating trigger plan and monitor";
        try
        {
            this.InitiateMonitorTriggerPlan( true );
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
        }
    }

    #endregion

    #region " trigger monitor event handling "

    /// <summary> Handles the measurement completed request. </summary>
    /// <param name="sender"> <see cref="object"/>
    /// instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void HandleMeasurementCompletedRequest( object? sender, EventArgs e )
    {
        if ( this.Device is null || this.Device.Session is null || this.BufferSubsystem is null
            || this.BufferReadings is null || this.DataGridView is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"{this.Device.ResourceNameCaption} handling SRQ: {this.Device.Session.ServiceRequestStatus:X}";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} kludge: reading buffer count";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );

            // this assume buffer is cleared upon each new cycle
            int newBufferCount = this.BufferSubsystem.QueryActualPointCount().GetValueOrDefault( 0 );
            if ( newBufferCount > 0 )
            {
                activity = $"{this.Device.ResourceNameCaption} kludge: buffer has data...";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                activity = $"{this.Device.ResourceNameCaption} handling measurement available";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                if ( false )
                {
#pragma warning disable CS0162 // Unreachable code detected
                    activity = $"{this.Device?.ResourceNameCaption} fetching a single reading";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    _ = this.MultimeterSubsystem.MeasurePrimaryReading();
#pragma warning restore CS0162 // Unreachable code detected
                }
                else
                {
                    activity = $"{this.Device.ResourceNameCaption} fetching buffered readings";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.BufferReadings.Add( this.BufferSubsystem.QueryBufferReadings() );
                    activity = $"{this.Device.ResourceNameCaption} updating the display";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.DataGridView.Invalidate();
                    this.BufferSubsystem.StopElapsedStopwatch();
                }

                if ( this._repeatMenuItem.Checked )
                {
                    activity = $"{this.Device.ResourceNameCaption} initiating next measurement(s)";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.BufferSubsystem.StartElapsedStopwatch();
                    this.BufferSubsystem.ClearBuffer(); // ?@3 removed 17-7-6
                    this.TriggerSubsystem?.Initiate();
                }
            }
            else
            {
                activity = $"{this.Device.ResourceNameCaption} trigger plan started; buffer empty";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            }
        }
        catch ( Exception ex )
        {
            _ = (this.Device.Session?.StatusPrompt = $"failed {activity}");
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Gets or sets the measurement complete handler added. </summary>
    /// <value> The measurement complete handler added. </value>
    private bool MeasurementCompleteHandlerAdded { get; set; }

    /// <summary> Adds measurement complete event handler. </summary>
    private void AddMeasurementCompleteEventHandler()
    {
        if ( this.Device is null || this.Device.Session is null ) return;
        string activity = string.Empty;
        if ( !this.MeasurementCompleteHandlerAdded )
        {
            // clear execution state before enabling events
            activity = $"{this.Device.ResourceNameCaption} Clearing execution state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            activity = $"{this.Device.ResourceNameCaption} Enabling session service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.EnableServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Adding device service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.AddServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Turning on measurement events";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            _ = this.Device.StatusSubsystemBase?.ApplyMeasurementEventEnableBitmask( 0x7FFF );
            //
            // if handling buffer full, use the 4917 event to detect buffer full.

            activity = $"{this.Device.ResourceNameCaption} Turning on status service request";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.ApplyStatusByteEnableBitmask( ( int ) this.Device.Session.OperationServiceRequestEnableBitmask );
            activity = $"{this.Device.ResourceNameCaption} Adding re-triggering event handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ServiceRequested += this.HandleMeasurementCompletedRequest;
            this.MeasurementCompleteHandlerAdded = true;
        }
    }

    /// <summary> Removes the measurement complete event handler. </summary>
    private void RemoveMeasurementCompleteEventHandler()
    {
        if ( this.Device is null || this.Device.Session is null ) return;
        string activity = string.Empty;
        if ( this.MeasurementCompleteHandlerAdded )
        {
            activity = $"{this.Device.ResourceNameCaption} Disabling session service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.DisableServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Removing device service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.RemoveServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Turning off measurement events";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            // Me.Device.StatusSubsystemBase.ApplyQuestionableEventEnableBitmask(0)

            activity = $"{this.Device.ResourceNameCaption} Turning off status service request";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.ApplyStatusByteEnableBitmask( 0 );
            activity = $"{this.Device.ResourceNameCaption} Removing re-triggering event handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ServiceRequested -= this.HandleMeasurementCompletedRequest;
            this.MeasurementCompleteHandlerAdded = false;
        }
    }

    /// <summary>
    /// Event handler. Called by HandleMeasurementEventMenuItem for check state changed events.
    /// </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void HandleMeasurementEventMenuItem_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        if ( sender is not ToolStripMenuItem menuItem ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} Aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.AbortTriggerPlan();
            if ( menuItem.Checked )
            {
                activity = $"{this.Device?.ResourceNameCaption} Adding measurement completion handler";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.AddMeasurementCompleteEventHandler();
            }
            else
            {
                activity = $"{this.Device?.ResourceNameCaption} Removing measurement completion handler";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.RemoveMeasurementCompleteEventHandler();
            }
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " buffer handler "

    /// <summary> Handles the buffer full request. </summary>
    /// <param name="sender"> <see cref="object"/>
    /// instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void HandleBufferFullRequest( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null
            || this.Device is null || this.Device.Session is null || this.Device.StatusSubsystemBase is null
            || this.BufferSubsystem is null || this.BufferReadings is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} handling SRQ: {this.Device.Session.ServiceRequestStatus:X2}";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.Device.Session.OperationCompleted == true )
            {
                activity = $"{this.Device.ResourceNameCaption} handling operation completed";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );

                // todo: See if can only do a set condition and not read this.
                int condition = this.Device.StatusSubsystemBase.QueryOperationEventCondition().GetValueOrDefault( 0 );
                activity = $"{this.Device.ResourceNameCaption} OPER: {condition:X2}";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );

                // If Bit 0 Is set then the buffer is full
                if ( (condition & (1 << this.BufferFullOperationConditionBitNumber)) != 0 )
                {
                    activity = $"{this.Device.ResourceNameCaption} handling buffer full";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    activity = $"{this.Device.ResourceNameCaption} fetching buffered readings";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.BufferReadings.Add( this.BufferSubsystem.QueryBufferReadings() );
                    activity = $"{this.Device.ResourceNameCaption} updating the display";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.BufferSubsystem.LastReading = this.BufferReadings.LastReading;
                    this.BufferSubsystem.StopElapsedStopwatch();
                    if ( this._repeatMenuItem.Checked && this.TriggerSubsystem is not null )
                    {
                        activity = $"{this.Device.ResourceNameCaption} initiating next measurement(s)";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                        this.BufferSubsystem.StartElapsedStopwatch();
                        this.BufferSubsystem.ClearBuffer(); // ?@# removed 17-7-6
                        this.TriggerSubsystem.Initiate();
                    }
                }
                else
                {
                    activity = $"{this.Device?.ResourceNameCaption} handling buffer clear: NOP";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                }
            }
            else
            {
                activity = $"{this.Device?.ResourceNameCaption} operation not completed";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            }
        }
        catch ( Exception ex )
        {
            this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Gets or sets the Buffer Full handler added. </summary>
    /// <value> The Buffer Full handler added. </value>
    private bool BufferFullHandlerAdded { get; set; }

    /// <summary> Gets or sets the buffer full operation condition bit number. </summary>
    /// <value> The buffer full operation condition bit number. </value>
    private int BufferFullOperationConditionBitNumber { get; set; }

    /// <summary> Adds Buffer Full event handler. </summary>
    private void AddBufferFullEventHandler()
    {
        if ( this.InitializingComponents
            || this.Device is null || this.Device.Session is null || this.Device.StatusSubsystemBase is null ) return;
        string activity = string.Empty;
        if ( !this.BufferFullHandlerAdded )
        {
            // clear execution state before enabling events
            activity = $"{this.Device.ResourceNameCaption} Clearing execution state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            activity = $"{this.Device.ResourceNameCaption} Enabling session service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.EnableServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Adding device service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.AddServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Turning on Buffer events";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.BufferFullOperationConditionBitNumber = 0;
            _ = this.Device.StatusSubsystemBase.ApplyOperationEventMap( this.BufferFullOperationConditionBitNumber,
                this.Device.StatusSubsystemBase.BufferFullEventNumber, this.Device.StatusSubsystemBase.BufferEmptyEventNumber );
            _ = this.Device.StatusSubsystemBase.ApplyOperationEventEnableBitmask( 1 << this.BufferFullOperationConditionBitNumber );
            activity = $"{this.Device.ResourceNameCaption} Turning on status service request";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.ApplyStatusByteEnableBitmask( ( int ) this.Device.Session.OperationServiceRequestEnableBitmask );
            activity = $"{this.Device.ResourceNameCaption} Adding re-triggering event handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ServiceRequested += this.HandleBufferFullRequest;
            this.BufferFullHandlerAdded = true;
            this.ClearBufferDisplay();
        }
    }

    /// <summary> Removes the Buffer Full event handler. </summary>
    private void RemoveBufferFullEventHandler()
    {
        if ( this.InitializingComponents
            || this.Device is null || this.Device.Session is null || this.Device.StatusSubsystemBase is null ) return;
        string activity = string.Empty;
        if ( this.BufferFullHandlerAdded )
        {
            activity = $"{this.Device.ResourceNameCaption} Disabling session service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.DisableServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Removing device service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.RemoveServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Turning off Buffer events";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.BufferFullOperationConditionBitNumber = 0;
            _ = this.Device.StatusSubsystemBase.ApplyOperationEventMap( this.BufferFullOperationConditionBitNumber, 0, 0 );
            _ = this.Device.StatusSubsystemBase.ApplyOperationEventEnableBitmask( 0 );
            activity = $"{this.Device.ResourceNameCaption} Turning off status service request";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.ApplyStatusByteEnableBitmask( 0 );
            activity = $"{this.Device.ResourceNameCaption} Removing re-triggering event handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ServiceRequested -= this.HandleBufferFullRequest;
            this.BufferFullHandlerAdded = false;
        }
    }

    /// <summary>
    /// Event handler. Called by _handleBufferEventMenuItem for check state changed events.
    /// </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void HandleBufferEventMenuItem_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        if ( sender is not ToolStripMenuItem menuItem ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} Aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.AbortTriggerPlan();
            if ( menuItem.Checked )
            {
                activity = $"{this.Device?.ResourceNameCaption} Adding Buffer completion handler";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.AddBufferFullEventHandler();
            }
            else
            {
                activity = $"{this.Device?.ResourceNameCaption} Removing Buffer completion handler";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.RemoveBufferFullEventHandler();
            }
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " trigger controls on the reading panel "

    /// <summary> Starts trigger plan. </summary>
    public void StartTriggerMonitor()
    {
        if ( this.Device is null || this.TriggerSubsystem is null || this.BufferSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing execution state";
            this.Device.ClearExecutionState();
            activity = $"{this.Device.ResourceNameCaption} clearing buffer and display";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.ClearBufferDisplay();
            this.BufferSubsystem.ClearBuffer();
            activity = $"{this.Device.ResourceNameCaption} initiating trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TriggerSubsystem.Initiate();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Event handler. Called by the Initiate Button for click events. Initiates a reading for
    /// retrieval by way of the service request event.
    /// </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AbortStartTriggerPlanMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} Aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.AbortTriggerPlan();
            activity = $"{this.Device?.ResourceNameCaption} Starting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.StartTriggerMonitor();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Initiate trigger plan menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void InitiateTriggerPlanMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.Device is null || this.TriggerSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing execution state";
            this.Device.ClearExecutionState();
            activity = $"{this.Device.ResourceNameCaption} initiating trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TriggerSubsystem.StartElapsedStopwatch();
            this.TriggerSubsystem.Initiate();
            _ = this.TriggerSubsystem.QueryTriggerState();
            this.TriggerSubsystem.StopElapsedStopwatch();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Event handler. Called by AbortButton for click events. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AbortButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.AbortTriggerPlan();
    }

    #endregion

    #region " buffer "

    /// <summary> Buffer size text box validating. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Cancel event information. </param>
    private void BufferSizeNumeric_Validating( object? sender, CancelEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ApplyBufferCapacity( ( int ) this._bufferSizeNumeric.Value );
    }

    /// <summary> Handles the DataError event of the _dataGridView control. </summary>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      The <see cref="DataGridViewDataErrorEventArgs"/> instance containing the
    /// event data. </param>
    private void BufferDataGridView_DataError( object? sender, DataGridViewDataErrorEventArgs e )
    {
        try
        {
            // prevent error reporting when adding a new row or editing a cell
            if ( sender is DataGridView grid )
            {
                if ( grid.CurrentRow is not null && grid.CurrentRow.IsNewRow )
                    return;
                if ( grid.IsCurrentCellInEditMode )
                    return;
                if ( grid.IsCurrentRowDirty )
                    return;
                string activity = $"{this.Device?.ResourceNameCaption} exception editing row {e.RowIndex} column {e.ColumnIndex};. {e.Exception}";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( activity );
                _ = this.InfoProvider?.Annunciate( grid, cc.isr.WinControls.InfoProviderLevel.Error, activity );
            }
        }
        catch
        {
        }
    }

    /// <summary> Event handler. Called by DisplayBufferMenuItem for click events. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void DisplayBufferMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.DisplayBufferReadings();
    }

    /// <summary> Event handler. Called by ClearBufferDisplayMenuItem for click events. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ClearBufferDisplayMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ClearBufferDisplay();
    }

    #endregion
}
