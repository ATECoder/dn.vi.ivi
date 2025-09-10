using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Enums;
using cc.isr.Std.SplitExtensions;
using cc.isr.WinControls.CheckStateExtensions;
using cc.isr.VI.SubsystemsWinControls.ComboBoxExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> An Trigger Layer view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class TriggerLayerView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public TriggerLayerView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._countNumeric.NumericUpDown.Minimum = 1m;
        this._countNumeric.NumericUpDown.Maximum = 99999m;
        this._countNumeric.NumericUpDown.DecimalPlaces = 0;
        this._countNumeric.NumericUpDown.Value = 1m;
        this._inputLineNumeric.NumericUpDown.Minimum = 1m;
        this._inputLineNumeric.NumericUpDown.Maximum = 6m;
        this._inputLineNumeric.NumericUpDown.DecimalPlaces = 0;
        this._inputLineNumeric.NumericUpDown.Value = 2m;
        this._outputLineNumeric.NumericUpDown.Minimum = 1m;
        this._outputLineNumeric.NumericUpDown.Maximum = 6m;
        this._outputLineNumeric.NumericUpDown.DecimalPlaces = 0;
        this._outputLineNumeric.NumericUpDown.Value = 1m;
        this._timerIntervalNumeric.NumericUpDown.Minimum = 0m;
        this._timerIntervalNumeric.NumericUpDown.Maximum = 99999m;
        this._timerIntervalNumeric.NumericUpDown.DecimalPlaces = 3;
        this._timerIntervalNumeric.NumericUpDown.Value = 1m;
        this._delayNumeric.NumericUpDown.Minimum = 0m;
        this._delayNumeric.NumericUpDown.Maximum = 99999m;
        this._delayNumeric.NumericUpDown.DecimalPlaces = 3;
        this._delayNumeric.NumericUpDown.Value = 0m;
    }

    /// <summary> Creates a new TriggerLayerView. </summary>
    /// <returns> A TriggerLayerView. </returns>
    public static TriggerLayerView Create()
    {
        TriggerLayerView? view = null;
        try
        {
            view = new TriggerLayerView();
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


    /// <summary> Gets or sets the layer number. </summary>
    /// <value> The layer number. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public int LayerNumber
    {
        get;
        set
        {
            if ( value != this.LayerNumber )
            {
                field = value;
                this._subsystemSplitButton.Text = $"Trig{value}";
            }
        }
    }

    /// <summary> Gets or sets the number of triggers. </summary>
    /// <value> The number of triggers. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public int Count
    {
        get => this._infiniteCountButton.CheckState == CheckState.Checked ? int.MaxValue : ( int ) this._countNumeric.Value;
        set
        {
            if ( value != this.Count )
            {
                if ( value == int.MaxValue )
                {
                    this._infiniteCountButton.CheckState = CheckState.Checked;
                }
                else
                {
                    this._infiniteCountButton.CheckState = CheckState.Unchecked;
                    this._countNumeric.Value = value;
                }

                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the trigger source. </summary>
    /// <value> The trigger source. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public TriggerSources Source
    {
        get => ( TriggerSources ) ( int ) (this._sourceComboBox.ComboBox.SelectedValue ?? 0);
        set
        {
            if ( value != this.Source )
            {
                this._sourceComboBox.ComboBox.SelectedItem = value.ValueNamePair();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Applies the settings onto the instrument. </summary>
    public void ApplySettings()
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device?.ResourceNameCaption} applying {this._subsystemSplitButton.Text} settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TriggerSubsystem.StartElapsedStopwatch();
            _ = this._infiniteCountButton.CheckState == CheckState.Checked
                ? this.TriggerSubsystem.ApplyTriggerCount( int.MaxValue )
                : this.TriggerSubsystem.ApplyTriggerCount( ( int ) this._countNumeric.Value );

            _ = this.TriggerSubsystem.ApplyInputLineNumber( ( int ) this._inputLineNumeric.Value );
            _ = this.TriggerSubsystem.ApplyOutputLineNumber( ( int ) this._outputLineNumeric.Value );
            _ = this.TriggerSubsystem.ApplyTriggerSource( ( TriggerSources ) ( int ) (this._sourceComboBox.ComboBox.SelectedValue ?? 0) );
            _ = this.TriggerSubsystem.ApplyTriggerLayerBypassMode( this._bypassToggleButton.CheckState == CheckState.Checked ? TriggerLayerBypassModes.Source : TriggerLayerBypassModes.Acceptor );
            _ = this.TriggerSubsystem.ApplyDelay( TimeSpan.FromMilliseconds( ( double ) (1000m * this._delayNumeric.Value) ) );
            _ = this.TriggerSubsystem.ApplyTimerTimeSpan( TimeSpan.FromMilliseconds( ( double ) (1000m * this._timerIntervalNumeric.Value) ) );
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
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Reads the settings from the instrument. </summary>
    public void ReadSettings()
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} reading  {this._subsystemSplitButton.Text} settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            ReadSettings( this.TriggerSubsystem );
            this.ApplyPropertyChanged( this.TriggerSubsystem );
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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( TriggerLayerView ).SplitWords()}" );
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

    #region " trigger subsystem "

    /// <summary> Gets or sets the trigger subsystem. </summary>
    /// <value> The trigger subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public TriggerSubsystemBase? TriggerSubsystem { get; private set; }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem">   The subsystem. </param>
    /// <param name="layerNumber"> The layer number. </param>
    public void BindSubsystem( TriggerSubsystemBase subsystem, int layerNumber )
    {
        if ( this.TriggerSubsystem is not null )
        {
            this.BindSubsystem( false, this.TriggerSubsystem );
            this.TriggerSubsystem = null;
        }

        this.LayerNumber = layerNumber;
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
            this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.SupportedTriggerSources ) );
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.TriggerSubsystemPropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( TriggerSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.TriggerCount ) );
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.TriggerSource ) );
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.TriggerLayerBypassMode ) );
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.InputLineNumber ) );
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.IsTriggerLayerBypass ) );
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.IsTriggerCountInfinite ) );
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.OutputLineNumber ) );
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.Delay ) );
        this.HandlePropertyChanged( subsystem, nameof( TriggerSubsystemBase.TimerInterval ) );
    }

    /// <summary> Reads the settings from the instrument. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( TriggerSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryTriggerCount();
        _ = subsystem.QueryInputLineNumber();
        _ = subsystem.QueryOutputLineNumber();
        _ = subsystem.QueryTriggerSource();
        _ = subsystem.QueryTriggerLayerBypassMode();
        _ = subsystem.QueryDelay();
        _ = subsystem.QueryTimerTimeSpan();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handle the Calculate subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( TriggerSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( TriggerSubsystemBase.TriggerCount ):
                {
                    if ( subsystem.TriggerCount.HasValue )
                        this._countNumeric.Value = subsystem.TriggerCount.Value;
                    break;
                }

            case nameof( TriggerSubsystemBase.TriggerSource ):
                {
                    if ( subsystem.TriggerSource.HasValue && this._sourceComboBox.ComboBox.Items.Count > 0 )
                    {
                        this._sourceComboBox.ComboBox.SelectedItem = subsystem.TriggerSource.Value.ValueNamePair();
                    }

                    this._sendBusTriggerMenuItem.Enabled = Nullable.Equals( subsystem.TriggerSource, TriggerSources.Bus );
                    this.NotifyPropertyChanged( nameof( this.Source ) );
                    break;
                }

            case nameof( TriggerSubsystemBase.SupportedTriggerSources ):
                {
                    this.InitializingComponents = true;
                    this._sourceComboBox.ComboBox.ListSupportedTriggerSources( subsystem.SupportedTriggerSources );
                    this.InitializingComponents = false;
                    if ( subsystem.TriggerSource.HasValue && this._sourceComboBox.ComboBox.Items.Count > 0 )
                    {
                        this._sourceComboBox.ComboBox.SelectedItem = subsystem.TriggerSource.Value.ValueNamePair();
                    }

                    break;
                }

            case nameof( TriggerSubsystemBase.IsTriggerLayerBypass ):
                {
                    this._bypassToggleButton.CheckState = subsystem.TriggerLayerBypassMode.HasValue ? subsystem.TriggerLayerBypassMode.Value == TriggerLayerBypassModes.Acceptor ? CheckState.Checked : CheckState.Unchecked : CheckState.Indeterminate;
                    break;
                }

            case nameof( TriggerSubsystemBase.InputLineNumber ):
                {
                    if ( subsystem.InputLineNumber.HasValue )
                        this._inputLineNumeric.Value = subsystem.InputLineNumber.Value;
                    break;
                }

            case nameof( TriggerSubsystemBase.IsTriggerCountInfinite ):
                {
                    this._infiniteCountButton.CheckState = subsystem.IsTriggerCountInfinite.ToCheckState();
                    break;
                }

            case var @case when @case == nameof( TriggerSubsystemBase.IsTriggerLayerBypass ):
                {
                    this._bypassToggleButton.CheckState = subsystem.IsTriggerLayerBypass.ToCheckState();
                    break;
                }

            case nameof( TriggerSubsystemBase.OutputLineNumber ):
                {
                    if ( subsystem.OutputLineNumber.HasValue )
                        this._outputLineNumeric.Value = subsystem.OutputLineNumber.Value;
                    break;
                }

            case nameof( TriggerSubsystemBase.Delay ):
                {
                    if ( subsystem.Delay.HasValue )
                        this._delayNumeric.Value = ( decimal ) (0.001d * subsystem.Delay.Value.TotalMilliseconds);
                    break;
                }

            case nameof( TriggerSubsystemBase.TimerInterval ):
                {
                    if ( subsystem.TimerInterval.HasValue )
                        this._timerIntervalNumeric.Value = ( decimal ) (0.001d * subsystem.TimerInterval.Value.TotalMilliseconds);
                    break;
                }

            default:
                break;
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
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
    }

    #endregion

    #region " control event handlers "

    /// <summary> Bypass toggle button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void BypassToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        this._bypassToggleButton.Text = this._bypassToggleButton.CheckState.ToCheckStateCaption( "Bypass", "~Bypass", "Bypass?" );
    }

    /// <summary> Infinite count button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void InfiniteCountButton_CheckStateChanged( object? sender, EventArgs e )
    {
        this._infiniteCountButton.Text = this._infiniteCountButton.CheckState.ToCheckStateCaption( "Infinite", "Finite", "INF?" );
    }

    /// <summary> Applies the settings menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplySettingsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ApplySettings();
    }

    /// <summary> Reads settings menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadSettingsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ReadSettings();
    }

    /// <summary> Sends the bus trigger menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void SendBusTriggerMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.Device is null || this.Device.Session is null ) return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.Device.ResourceNameCaption} sending bus trigger";
                this.Device.Session.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.Device.Session.AssertTrigger();
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
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion
}
