using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Enums;
using cc.isr.Std.SplitExtensions;
using cc.isr.WinControls.CheckStateExtensions;
using cc.isr.VI.SubsystemsWinControls.ComboBoxExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> An Arm Layer view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class ArmLayerView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public ArmLayerView() : base()
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

    /// <summary> Creates a new ArmLayerView. </summary>
    /// <returns> An ArmLayerView. </returns>
    public static ArmLayerView Create()
    {
        ArmLayerView? view = null;
        try
        {
            view = new ArmLayerView();
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

    private int _layerNumber;

    /// <summary> Gets or sets the layer number. </summary>
    /// <value> The layer number. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public int LayerNumber
    {
        get => this._layerNumber;
        set
        {
            if ( value != this.LayerNumber )
            {
                this._layerNumber = value;
                this._subsystemSplitButton.Text = $"Arm{value}";
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
                    this._infiniteCountButton.CheckState = CheckState.Checked;

                else
                {
                    this._infiniteCountButton.CheckState = CheckState.Unchecked;
                    this._countNumeric.Value = value;
                }

                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the arm source. </summary>
    /// <value> The arm source. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ArmSources Source
    {
        get => ( ArmSources ) ( int ) (this._sourceComboBox.ComboBox.SelectedValue ?? 0);
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
        if ( this.Device is null || this.Device.Session is null || this.ArmLayerSubsystem is null ) return;
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
            this.ArmLayerSubsystem.StartElapsedStopwatch();
            _ = this._infiniteCountButton.CheckState == CheckState.Checked
                ? this.ArmLayerSubsystem.ApplyArmCount( int.MaxValue )
                : this.ArmLayerSubsystem.ApplyArmCount( ( int ) this._countNumeric.Value );

            _ = this.ArmLayerSubsystem.ApplyInputLineNumber( ( int ) this._inputLineNumeric.Value );
            _ = this.ArmLayerSubsystem.ApplyOutputLineNumber( ( int ) this._outputLineNumeric.Value );
            _ = this.ArmLayerSubsystem.ApplyArmSource( ( ArmSources ) ( int ) (this._sourceComboBox.ComboBox.SelectedValue ?? 0) );
            _ = this.ArmLayerSubsystem.ApplyArmLayerBypassMode( this._bypassToggleButton.CheckState == CheckState.Checked ? TriggerLayerBypassModes.Source : TriggerLayerBypassModes.Acceptor );
            if ( this.LayerNumber == 2 )
            {
                _ = this.ArmLayerSubsystem.ApplyDelay( TimeSpan.FromMilliseconds( ( double ) (1000m * this._delayNumeric.Value) ) );
                _ = this.ArmLayerSubsystem.ApplyTimerTimeSpan( TimeSpan.FromMilliseconds( ( double ) (1000m * this._timerIntervalNumeric.Value) ) );
            }

            this.ArmLayerSubsystem.StopElapsedStopwatch();
        }
        catch ( Exception ex )
        {
            if ( this.Device.Session is not null )
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
        if ( this.InitializingComponents || this.Device is null || this.Device.Session is null || this.ArmLayerSubsystem is null ) return;
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
            ReadSettings( this.ArmLayerSubsystem );
            this.ApplyPropertyChanged( this.ArmLayerSubsystem );
        }
        catch ( Exception ex )
        {
            if ( this.Device.Session is not null )
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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( ArmLayerView ).SplitWords()}" );
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
        string activity = $"{this.Device?.ResourceNameCaption} reading service request";
        try
        {
            this.Device?.Session!.ApplyStatusByte( this.Device.Session.ReadStatusByte() );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " subsystem "

    /// <summary> Gets or sets the arm layer subsystem. </summary>
    /// <value> The arm layer subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ArmLayerSubsystemBase? ArmLayerSubsystem { get; private set; }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem">   The subsystem. </param>
    /// <param name="layerNumber"> The layer number. </param>
    public void BindSubsystem( ArmLayerSubsystemBase subsystem, int layerNumber )
    {
        if ( this.ArmLayerSubsystem is not null )
        {
            this.BindSubsystem( false, this.ArmLayerSubsystem );
            this.ArmLayerSubsystem = null;
        }

        this.LayerNumber = layerNumber;
        this.ArmLayerSubsystem = subsystem;
        if ( this.ArmLayerSubsystem is not null )
        {
            this.BindSubsystem( true, this.ArmLayerSubsystem );
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, ArmLayerSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.ArmLayerSubsystemPropertyChanged;
            this.HandlePropertyChanged( subsystem, nameof( ArmLayerSubsystemBase.SupportedArmSources ) );
            // must Not read setting when biding because the instrument may be locked Or in a trigger mode
            // The bound values should be sent when binding Or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.ArmLayerSubsystemPropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( ArmLayerSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( ArmLayerSubsystemBase.ArmCount ) );
        this.HandlePropertyChanged( subsystem, nameof( ArmLayerSubsystemBase.ArmSource ) );
        this.HandlePropertyChanged( subsystem, nameof( ArmLayerSubsystemBase.IsArmCountInfinite ) );
        this.HandlePropertyChanged( subsystem, nameof( ArmLayerSubsystemBase.IsArmLayerBypass ) );
        this.HandlePropertyChanged( subsystem, nameof( ArmLayerSubsystemBase.InputLineNumber ) );
        this.HandlePropertyChanged( subsystem, nameof( ArmLayerSubsystemBase.OutputLineNumber ) );
        if ( subsystem.LayerNumber == 2 )
        {
            this.HandlePropertyChanged( subsystem, nameof( ArmLayerSubsystemBase.Delay ) );
            this.HandlePropertyChanged( subsystem, nameof( ArmLayerSubsystemBase.TimerInterval ) );
        }
    }

    /// <summary> Handle the Calculate subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( ArmLayerSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( ArmLayerSubsystemBase.ArmCount ):
                {
                    if ( subsystem.ArmCount.HasValue )
                        this._countNumeric.Value = subsystem.ArmCount.Value;
                    break;
                }

            case nameof( ArmLayerSubsystemBase.ArmSource ):
                {
                    if ( subsystem.ArmSource.HasValue && this._sourceComboBox.ComboBox.Items.Count > 0 )
                    {
                        this._sourceComboBox.ComboBox.SelectedItem = subsystem.ArmSource.Value.ValueNamePair();
                    }

                    break;
                }

            case nameof( ArmLayerSubsystemBase.SupportedArmSources ):
                {
                    this.InitializingComponents = true;
                    this._sourceComboBox.ComboBox.ListSupportedArmSources( subsystem.SupportedArmSources );
                    this.InitializingComponents = false;
                    if ( subsystem.ArmSource.HasValue && this._sourceComboBox.ComboBox.Items.Count > 0 )
                    {
                        this._sourceComboBox.ComboBox.SelectedItem = subsystem.ArmSource.Value.ValueNamePair();
                    }

                    break;
                }

            case nameof( ArmLayerSubsystemBase.InputLineNumber ):
                {
                    if ( subsystem.InputLineNumber.HasValue )
                        this._inputLineNumeric.Value = subsystem.InputLineNumber.Value;
                    break;
                }

            case nameof( ArmLayerSubsystemBase.IsArmCountInfinite ):
                {
                    this._infiniteCountButton.CheckState = subsystem.IsArmCountInfinite.ToCheckState();
                    break;
                }

            case nameof( ArmLayerSubsystemBase.IsArmLayerBypass ):
                {
                    this._bypassToggleButton.CheckState = subsystem.IsArmLayerBypass.ToCheckState();
                    break;
                }

            case nameof( ArmLayerSubsystemBase.OutputLineNumber ):
                {
                    if ( subsystem.OutputLineNumber.HasValue )
                        this._outputLineNumeric.Value = subsystem.OutputLineNumber.Value;
                    break;
                }

            case nameof( ArmLayerSubsystemBase.Delay ):
                {
                    if ( subsystem.Delay.HasValue )
                        this._delayNumeric.Value = ( decimal ) (0.001d * subsystem.Delay.Value.TotalMilliseconds);
                    break;
                }

            case nameof( ArmLayerSubsystemBase.TimerInterval ):
                {
                    if ( subsystem.TimerInterval.HasValue )
                        this._timerIntervalNumeric.Value = ( decimal ) (0.001d * subsystem.TimerInterval.Value.TotalMilliseconds);
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Arm layer subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void ArmLayerSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( ArmLayerSubsystemBase )}.{e.PropertyName} change";
        IAsyncResult? controlAsyncResult = null;
        IAsyncResult? stripAsyncResult = null;
        try
        {
            if ( this.InvokeRequired )
                // _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.ArmLayerSubsystemPropertyChanged ), new object[] { sender, e } );
                controlAsyncResult = this.BeginInvoke( new Action<object, PropertyChangedEventArgs>( this.ArmLayerSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                // _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.ArmLayerSubsystemPropertyChanged ), new object[] { sender, e } );
                stripAsyncResult = this._subsystemToolStrip.BeginInvoke( new Action<object, PropertyChangedEventArgs>( this.ArmLayerSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is ArmLayerSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            // https://StackOverflow.com/questions/22356/cleanest-way-to-invoke-cross-thread-events
            if ( controlAsyncResult is not null ) _ = this.EndInvoke( controlAsyncResult );
            if ( stripAsyncResult is not null ) _ = this._subsystemToolStrip.EndInvoke( stripAsyncResult );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Reads the settings from the instrument. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( ArmLayerSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryArmCount();
        _ = subsystem.QueryInputLineNumber();
        _ = subsystem.QueryOutputLineNumber();
        _ = subsystem.QueryArmSource();
        _ = subsystem.QueryArmLayerBypassMode();
        if ( subsystem.LayerNumber == 2 )
        {
            _ = subsystem.QueryDelay();
            _ = subsystem.QueryTimerTimeSpan();
        }

        subsystem.StopElapsedStopwatch();
    }

    #endregion

    #region " control event handlers "

    /// <summary> Applies the menu ite click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplyMenuIte_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents ) return;
        this.ApplySettings();
    }

    /// <summary> Reads menu ite click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadMenuIte_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents ) return;
        this.ReadSettings();
    }

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
        this._infiniteCountButton.Text = this._infiniteCountButton.CheckState.ToCheckStateCaption( "Infinite", "Finite", "Infinite?" );
    }

    #endregion


}
