using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Enums;
using cc.isr.Std.SplitExtensions;
using cc.isr.VI.SubsystemsWinControls.ComboBoxExtensions;
using cc.isr.WinControls.CheckStateExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> An trace buffer view. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2020-01-13 </para>
/// </remarks>
public partial class TraceBufferView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public TraceBufferView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._sizeNumeric.NumericUpDown.Minimum = 0m;
        this._sizeNumeric.NumericUpDown.Maximum = int.MaxValue;
        this._sizeNumeric.NumericUpDown.DecimalPlaces = 0;
        this._sizeNumeric.NumericUpDown.Value = 2m;
        this._preTriggerCountNumeric.NumericUpDown.Minimum = 0m;
        this._preTriggerCountNumeric.NumericUpDown.Maximum = int.MaxValue;
        this._preTriggerCountNumeric.NumericUpDown.DecimalPlaces = 0;
        this._preTriggerCountNumeric.NumericUpDown.Value = 0m;

        // Hide to be done items
        this._elementGroupToggleButton.Visible = false;
        this._preTriggerCountNumeric.Visible = false;
        this._preTriggerCountNumericLabel.Visible = false;
        this._timestampFormatToggleButton.Visible = false;
    }

    /// <summary> Creates a new TraceBufferView. </summary>
    /// <returns> A TraceBufferView. </returns>
    public static TraceBufferView Create()
    {
        TraceBufferView? view = null;
        try
        {
            view = new TraceBufferView();
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

    /// <summary> Name of the buffer. </summary>
    private string _bufferName = string.Empty;

    /// <summary> Gets or sets the name of the buffer. </summary>
    /// <value> The name of the buffer. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string BufferName
    {
        get => this._bufferName;
        set
        {
            if ( !string.Equals( value, this.BufferName, StringComparison.Ordinal ) )
            {
                this._bufferName = value;
                this._bufferNameLabel.Text = value;
            }
        }
    }

    /// <summary> Gets or sets the size of the buffer. </summary>
    /// <value> The size of the buffer. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public int BufferSize
    {
        get => ( int ) this._sizeNumeric.Value;
        set
        {
            if ( value != this.BufferSize )
            {
                this._sizeNumeric.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the feed source. </summary>
    /// <value> The feed source. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public FeedSources FeedSource
    {
        get => ( FeedSources ) ( int ) (this._feedSourceComboBox.ComboBox.SelectedValue ?? 0);
        set
        {
            if ( value != this.FeedSource )
            {
                this._feedSourceComboBox.ComboBox.SelectedItem = value.ValueNamePair();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the feed control. </summary>
    /// <value> The feed control. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public FeedControls FeedControl
    {
        get => ( FeedControls ) ( int ) (this._feedControlComboBox.ComboBox.SelectedValue ?? 0);
        set
        {
            if ( ( int ) value != ( int ) this.FeedSource )
            {
                this._feedControlComboBox.ComboBox.SelectedItem = value.ValueNamePair();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Applies the settings onto the instrument. </summary>
    public void ApplySettings()
    {
        if ( this.InitializingComponents || this.Device is null || this.TraceSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} applying {this._subsystemSplitButton.Text} settings ";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TraceSubsystem.StartElapsedStopwatch();
            _ = this.TraceSubsystem.ApplyPointsCount( ( int ) this._sizeNumeric.Value );
            _ = this.TraceSubsystem.ApplyFeedSource( ( FeedSources ) ( int ) (this._feedSourceComboBox.ComboBox.SelectedValue ?? 0) );
            _ = this.TraceSubsystem.ApplyFeedControl( ( FeedControls ) ( int ) (this._feedControlComboBox.ComboBox.SelectedValue ?? 0) );
            this.TraceSubsystem.StopElapsedStopwatch();
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
        if ( this.InitializingComponents || this.Device is null || this.TraceSubsystem is null ) return;
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
            ReadSettings( this.TraceSubsystem );
            this.ApplyPropertyChanged( this.TraceSubsystem );
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

    /// <summary> Clears the buffer to its blank/initial state. </summary>
    public void Clear()
    {
        if ( this.InitializingComponents || this.Device is null || this.TraceSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} Clearing {this._subsystemSplitButton.Text}";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TraceSubsystem.StartElapsedStopwatch();
            this.TraceSubsystem.ClearBuffer();
            this.TraceSubsystem.StopElapsedStopwatch();
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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( TraceBufferView ).SplitWords()}" );
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

    #region " trace subsystem "

    /// <summary> Gets or sets the trace subsystem. </summary>
    /// <value> The trace subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public TraceSubsystemBase? TraceSubsystem { get; private set; }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem">  The subsystem. </param>
    /// <param name="bufferName"> The name of the buffer. </param>
    public void BindSubsystem( TraceSubsystemBase subsystem, string bufferName )
    {
        if ( this.TraceSubsystem is not null )
        {
            this.BindSubsystem( false, this.TraceSubsystem );
            this.TraceSubsystem = null;
        }

        this.BufferName = bufferName;
        this.TraceSubsystem = subsystem;
        if ( this.TraceSubsystem is not null )
        {
            this.BindSubsystem( true, this.TraceSubsystem );
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, TraceSubsystemBase subsystem )
    {
        if ( add )
        {
            this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.SupportedFeedControls ) );
            this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.SupportedFeedSources ) );
            subsystem.PropertyChanged += this.TraceSubsystemPropertyChanged;
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.TraceSubsystemPropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( TraceSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.AvailablePointCount ) );
        this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.PointsCount ) );
        this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.FeedControl ) );
        this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.FeedSource ) );
    }

    /// <summary> Reads the settings from the instrument. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( TraceSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryBufferFreePointCount();
        _ = subsystem.QueryPointsCount();
        _ = subsystem.QueryFeedSource();
        _ = subsystem.QueryFeedControl();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handle the Calculate subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( TraceSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( TraceSubsystemBase.AvailablePointCount ):
                {
                    this._freeCountLabel.Text = subsystem.AvailablePointCount is null ? "?" : subsystem.AvailablePointCount.Value.ToString();
                    break;
                }

            case nameof( TraceSubsystemBase.FeedControl ):
                {
                    if ( subsystem.FeedControl.HasValue && this._feedControlComboBox.ComboBox.Items.Count > 0 )
                    {
                        this._feedControlComboBox.ComboBox.SelectedItem = subsystem.FeedControl.Value.ValueNamePair();
                    }

                    break;
                }

            case nameof( TraceSubsystemBase.FeedSource ):
                {
                    if ( subsystem.FeedSource.HasValue && this._feedSourceComboBox.ComboBox.Items.Count > 0 )
                    {
                        this._feedSourceComboBox.ComboBox.SelectedItem = subsystem.FeedSource.Value.ValueNamePair();
                    }

                    break;
                }

            case nameof( TraceSubsystemBase.PointsCount ):
                {
                    if ( subsystem.PointsCount.HasValue )
                        this._sizeNumeric.Value = subsystem.PointsCount.Value;
                    break;
                }

            case nameof( TraceSubsystemBase.SupportedFeedControls ):
                {
                    this._feedControlComboBox.ComboBox.ListSupportedFeedControls( subsystem.SupportedFeedControls );
                    if ( subsystem.FeedControl.HasValue && this._feedControlComboBox.ComboBox.Items.Count > 0 )
                    {
                        this._feedControlComboBox.ComboBox.SelectedItem = subsystem.FeedControl.Value.ValueNamePair();
                    }

                    break;
                }

            case nameof( TraceSubsystemBase.SupportedFeedSources ):
                {
                    this._feedSourceComboBox.ComboBox.ListSupportedFeedSources( subsystem.SupportedFeedSources );
                    if ( subsystem.FeedSource.HasValue && this._feedSourceComboBox.ComboBox.Items.Count > 0 )
                    {
                        this._feedSourceComboBox.ComboBox.SelectedItem = subsystem.FeedSource.Value.ValueNamePair();
                    }

                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Trace subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void TraceSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( TraceSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.TraceSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.TraceSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is TraceSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " control event handlers "

    /// <summary> Clears the buffer button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ClearBufferButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.Clear();
    }

    /// <summary> Element group toggle button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ElementGroupToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"Elements: {button.CheckState.ToCheckStateCaption( "Full", "Compact", "?" )}";
        }
    }

    /// <summary> Timestamp format toggle button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void TimestampFormatToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"Timestamp: {button.CheckState.ToCheckStateCaption( "Absolute", "Delta", "?" )}";
        }
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

    #endregion
}
