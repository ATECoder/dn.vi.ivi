using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;
using cc.isr.VI.SubsystemsWinControls.DataGridViewExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A Buffer Stream view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class BufferStreamView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public BufferStreamView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._bufferSizeNumeric.NumericUpDown.CausesValidation = true;
        this._bufferSizeNumeric.NumericUpDown.Minimum = 0m;
        this._bufferSizeNumeric.NumericUpDown.Maximum = 27500000m;
        this._startBufferStreamMenuItem.Enabled = false;
        this._assertBusTriggerButton.Enabled = false;
    }

    /// <summary> Creates a new <see cref="BufferStreamView"/> </summary>
    /// <returns> A <see cref="BufferStreamView"/>. </returns>
    public static BufferStreamView Create()
    {
        BufferStreamView? view = null;
        try
        {
            view = new BufferStreamView();
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

    #region " settings "

    /// <summary>   Gets or sets the timing settings. </summary>
    /// <value> The timing settings. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public BufferStreamViewSettings Settings { get; private set; } = BufferStreamViewSettings.Instance;

    #endregion

    #region " public members: buffer "

    /// <summary> Applies the buffer capacity. </summary>
    /// <param name="capacity"> The capacity. </param>
    public void ApplyBufferCapacity( int capacity )
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            if ( this.Device.IsDeviceOpen )
            {
                if ( this.BufferSubsystem is not null )
                {
                    activity = $"{this.Device?.ResourceNameCaption} setting {typeof( TraceSubsystemBase )}.{nameof( BufferSubsystemBase.Capacity )} to {capacity}";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( activity );
                    ApplyBufferCapacity( this.BufferSubsystem, capacity );
                }
                else if ( this.TraceSubsystem is not null )
                {
                    activity = $"{this.Device?.ResourceNameCaption} setting {typeof( TraceSubsystemBase )}.{nameof( TraceSubsystemBase.PointsCount )} to {capacity}";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( activity );
                    ApplyBufferCapacity( this.TraceSubsystem, capacity );
                }
            }
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
    }

    /// <summary> Reads the buffer. </summary>
    public void ReadBuffer()
    {
        if ( this.InitializingComponents ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} reading buffer";
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

    #region " public members: buffer display "

    private DataGridView? _dataGridViewThis;

    /// <summary>   Gets or sets the reference to the data grid view. </summary>
    /// <value> The reference to the data grid view. </value>
    private DataGridView? DataGridViewThis
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get => this._dataGridViewThis;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( this._dataGridViewThis is not null )
                this._dataGridViewThis.DataError -= this.DataGridView_DataError;

            this._dataGridViewThis = value;
            if ( this._dataGridViewThis is not null )
                this._dataGridViewThis.DataError += this.DataGridView_DataError;
        }
    }

    /// <summary> Gets or sets the data grid view. </summary>
    /// <value> The data grid view. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public DataGridView? DataGridView
    {
        get => this.DataGridViewThis;
        set => this.DataGridViewThis = value;
    }

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
            this.DataGridView = null;
            this.Device = null;
        }

        this.Device = value;
        if ( value is not null )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( BufferStreamView ).SplitWords()}" );
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

    /// <summary> Gets the Buffer subsystem. </summary>
    /// <value> The Buffer subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
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
        if ( add )
            _ = (this.DataGridView?.Bind( subsystem.BufferReadingsBindingList, true ));
        Binding binding = this.AddRemoveBinding( this._bufferSizeNumeric.NumericUpDown, add, nameof( NumericUpDown.Value ), subsystem, nameof( BufferSubsystemBase.Capacity ) );
        // has to apply the value.
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        _ = this.AddRemoveBinding( this._bufferCountLabel, add, nameof( Control.Text ), subsystem, nameof( BufferSubsystemBase.ActualPointCount ) );
        _ = this.AddRemoveBinding( this._firstPointNumberLabel, add, nameof( Control.Text ), subsystem, nameof( BufferSubsystemBase.FirstPointNumber ) );
        _ = this.AddRemoveBinding( this._lastPointNumberLabel, add, nameof( Control.Text ), subsystem, nameof( BufferSubsystemBase.LastPointNumber ) );
        // must Not read setting when biding because the instrument may be locked Or in a trigger mode
        // The bound values should be sent when binding Or when applying property change.
        // ReadSettings( subsystem );
        if ( add )
        {
            if ( this.BufferSubsystem is not null )
                this.BufferSubsystem.PropertyChanged += this.HandleBufferSubsystemPropertyChange;
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            if ( this.BufferSubsystem != null )
                this.BufferSubsystem.PropertyChanged -= this.HandleBufferSubsystemPropertyChange;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( BufferSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( BufferSubsystemBase.BufferStreamingEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( BufferSubsystemBase.BufferStreamingActive ) );
        this.HandlePropertyChanged( subsystem, nameof( BufferSubsystemBase.BufferReadingsCount ) );
    }

    /// <summary> Handles the Buffer subsystem property change. </summary>
    /// <param name="subsystem">    The sender. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( BufferSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is not null && !string.IsNullOrWhiteSpace( propertyName ) )
        {
            switch ( propertyName ?? string.Empty )
            {
                case nameof( BufferSubsystemBase.BufferReadingsCount ):
                    {
                        if ( subsystem.BufferReadingsCount > 0 )
                        {
                            string message = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Streaming reading #{subsystem.BufferReadingsCount}: {subsystem.LastReading.Amount} 0x{subsystem.LastReading.StatusWord:X4}" );
                            subsystem.Session.StatusPrompt = message;
                        }

                        break;
                    }

                case nameof( BufferSubsystemBase.BufferStreamingEnabled ):
                    {
                        this.NotifyPropertyChanged( nameof( BufferSubsystemBase.BufferStreamingEnabled ) );
                        break;
                    }

                case nameof( BufferSubsystemBase.BufferStreamingActive ):
                    {
                        this._configureStreamingMenuItem.Enabled = !subsystem.BufferStreamingActive;
                        if ( subsystem.BufferReadingsCount > 0 && !subsystem.BufferStreamingActive )
                        {
                            string message = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Streaming ended reading #{subsystem.BufferReadingsCount}: {subsystem.LastReading.Amount} 0x{subsystem.LastReading.StatusWord:X4}" );
                            subsystem.Session.StatusPrompt = message;
                        }

                        this.NotifyPropertyChanged( nameof( BufferSubsystemBase.BufferStreamingActive ) );
                        break;
                    }

                default:
                    break;
            }
        }
    }

    /// <summary> Handles the Buffer subsystem property change. </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Property changed event information. </param>
    private void HandleBufferSubsystemPropertyChange( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( BufferSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.HandleBufferSubsystemPropertyChange ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.HandleBufferSubsystemPropertyChange ), [sender, e] );

            else if ( sender is BufferSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( BufferSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryCapacity();
        _ = subsystem.QueryFirstPointNumber();
        _ = subsystem.QueryLastPointNumber();
        subsystem.StopElapsedStopwatch();
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

    /// <summary> Gets the Trace subsystem. </summary>
    /// <value> The Trace subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
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
        if ( this.TraceSubsystem is null ) return;
        if ( add )
            _ = (this.DataGridView?.Bind( subsystem.BufferReadingsBindingList, true ));
        Binding binding = this.AddRemoveBinding( this._bufferSizeNumeric.NumericUpDown, add, nameof( NumericUpDown.Value ), subsystem, nameof( TraceSubsystemBase.PointsCount ) );
        // has to apply the value.
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        _ = this.AddRemoveBinding( this._bufferCountLabel, add, nameof( Control.Text ), subsystem, nameof( TraceSubsystemBase.ActualPointCount ) );
        this._firstPointNumberLabel.Visible = false;
        this._lastPointNumberLabel.Visible = false;
        // must Not read setting when biding because the instrument may be locked Or in a trigger mode
        // The bound values should be sent when binding Or when applying property change.
        // ReadSettings( subsystem );
        if ( add )
        {
            this.TraceSubsystem.PropertyChanged += this.HandleTraceSubsystemPropertyChange;
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            this.TraceSubsystem.PropertyChanged -= this.HandleTraceSubsystemPropertyChange;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( TraceSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.BufferStreamingEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.BufferStreamingActive ) );
        this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.BufferReadingsCount ) );
    }

    /// <summary> Handles the trace subsystem property change. </summary>
    /// <param name="subsystem">    The sender. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( TraceSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is not null && !string.IsNullOrWhiteSpace( propertyName ) )
        {
            switch ( propertyName ?? string.Empty )
            {
                case nameof( TraceSubsystemBase.BufferReadingsCount ):
                    {
                        if ( subsystem.BufferReadingsCount > 0 )
                        {
                            string message = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Streaming reading #{subsystem.BufferReadingsCount}: {subsystem.LastBufferReading.Amount} 0x{subsystem.LastBufferReading.StatusWord:X4}" );
                            subsystem.Session.StatusPrompt = message;
                        }

                        break;
                    }

                case nameof( TraceSubsystemBase.BufferStreamingEnabled ):
                    {
                        this.NotifyPropertyChanged( nameof( TraceSubsystemBase.BufferStreamingEnabled ) );
                        break;
                    }

                case nameof( TraceSubsystemBase.BufferStreamingActive ):
                    {
                        this._configureStreamingMenuItem.Enabled = !subsystem.BufferStreamingActive;
                        if ( subsystem.BufferReadingsCount > 0 && !subsystem.BufferStreamingActive )
                        {
                            string message = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Streaming ended reading #{subsystem.BufferReadingsCount}: {subsystem.LastBufferReading.Amount} 0x{subsystem.LastBufferReading.StatusWord:X4}" );
                            subsystem.Session.StatusPrompt = message;
                        }

                        this.NotifyPropertyChanged( nameof( TraceSubsystemBase.BufferStreamingActive ) );
                        break;
                    }

                default:
                    break;
            }
        }
    }

    /// <summary> Handles the trace subsystem property change. </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Property changed event information. </param>
    private void HandleTraceSubsystemPropertyChange( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( TraceSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.TriggerSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.HandleTraceSubsystemPropertyChange ), [sender, e] );

            else if ( sender is TraceSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( TraceSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryPointsCount();
        subsystem.StopElapsedStopwatch();
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

    #region " trigger "

    /// <summary> Gets the Trigger subsystem. </summary>
    /// <value> The Trigger subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public TriggerSubsystemBase? TriggerSubsystem { get; private set; }

    /// <summary> Bind Trigger subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( TriggerSubsystemBase subsystem )
    {
        if ( this.TriggerSubsystem is not null )
        {
            this.BindSubsystem( false, this.TriggerSubsystem );
            this.TriggerSubsystem = null;
        }

        this.TriggerSubsystem = subsystem;
        if ( this.TriggerSubsystem is not null )
        {
            this.BindSubsystem( true, this.TriggerSubsystem );
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, TriggerSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.TriggerSubsystemPropertyChanged;
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
                    if ( subsystem.SupportsTriggerState && subsystem.TriggerState.HasValue )
                    {
                        this._triggerStateLabel.Text = subsystem.TriggerState.Value.ToString();
                        if ( !subsystem.IsTriggerStateActive() && this._startBufferStreamMenuItem.Checked )
                        {
                            this._startBufferStreamMenuItem.Checked = false;
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
            if ( this.Device.Session is not null )
                _ = this.Device.Session.QueryOperationCompleted();
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

    /// <summary> Gets the System subsystem. </summary>
    /// <value> The System subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public SystemSubsystemBase? SystemSubsystem { get; private set; }

    /// <summary> Bind System subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( SystemSubsystemBase subsystem )
    {
        if ( this.SystemSubsystem is not null )
        {
            this.SystemSubsystem.PropertyChanged -= this.HandleSystemSubsystemPropertyChange;
            this.SystemSubsystem = null;
        }

        this.SystemSubsystem = subsystem;
        if ( this.SystemSubsystem is not null )
        {
            // must Not read setting when biding because the instrument may be locked Or in a trigger mode
            // The bound values should be sent when binding Or when applying property change.
            // ReadSettings( subsystem );
            // Me.ApplyPropertyChanged(subsystem)
            this.SystemSubsystem.PropertyChanged += this.HandleSystemSubsystemPropertyChange;
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

    /// <summary> Handles the property changed. </summary>
    /// <remarks> David, 2020-04-09. </remarks>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( SystemSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is not null && !string.IsNullOrWhiteSpace( propertyName ) )
        {
            switch ( propertyName ?? string.Empty )
            {
                case nameof( SystemSubsystemBase.SupportsScanCardOption ):
                    {
                        this._usingScanCardMenuItem.Enabled = subsystem.SupportsScanCardOption;
                        if ( !subsystem.SupportsScanCardOption )
                            this._usingScanCardMenuItem.Checked = false;

                        break;
                    }

                default:
                    break;
            }
        }
    }

    /// <summary> Handles the system subsystem property change. </summary>
    /// <remarks> David, 2020-04-09. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property changed event information. </param>
    private void HandleSystemSubsystemPropertyChange( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( SystemSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.HandleSystemSubsystemPropertyChange ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.HandleSystemSubsystemPropertyChange ), [sender, e] );

            else if ( sender is SystemSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " arm layer 1 subsystem "

    /// <summary> Gets the ArmLayer1 subsystem. </summary>
    /// <value> The ArmLayer1 subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ArmLayerSubsystemBase? ArmLayer1Subsystem { get; private set; }

    /// <summary> Bind ArmLayer1 subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem1( ArmLayerSubsystemBase subsystem )
    {
        if ( this.ArmLayer1Subsystem is not null )
        {
            this.ArmLayer1Subsystem = null;
        }

        this.ArmLayer1Subsystem = subsystem;
        if ( this.ArmLayer1Subsystem is not null )
        {
        }
    }

    #endregion

    #region " arm layer 2 subsystem "

    /// <summary> Gets the ArmLayer2 subsystem. </summary>
    /// <value> The ArmLayer2 subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ArmLayerSubsystemBase? ArmLayer2Subsystem { get; private set; }

    /// <summary> Bind ArmLayer2 subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem2( ArmLayerSubsystemBase subsystem )
    {
        if ( this.ArmLayer2Subsystem is not null )
        {
            this.ArmLayer2Subsystem = null;
        }

        this.ArmLayer2Subsystem = subsystem;
        if ( this.ArmLayer2Subsystem is not null )
        {
        }
    }

    #endregion

    #region " digital output subsystem "

    /// <summary> Gets the digital output subsystem. </summary>
    /// <value> The digital output subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public DigitalOutputSubsystemBase? DigitalOutputSubsystem { get; private set; }

    /// <summary> Bind ArmLayer2 subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( DigitalOutputSubsystemBase subsystem )
    {
        if ( this.DigitalOutputSubsystem is not null )
        {
            this.DigitalOutputSubsystem = null;
        }

        this.DigitalOutputSubsystem = subsystem;
        if ( this.DigitalOutputSubsystem is not null )
        {
        }
    }

    #endregion

    #region " sense subsystem "

    /// <summary> Gets the Sense subsystem. </summary>
    /// <value> The Sense subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public SenseSubsystemBase? SenseSubsystem { get; private set; }

    /// <summary> Bind Sense subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( SenseSubsystemBase subsystem )
    {
        if ( this.SenseSubsystem is not null )
        {
            this.SenseSubsystem.PropertyChanged -= this.SenseSubsystemPropertyChanged;
            this.SenseSubsystem = null;
        }

        this.SenseSubsystem = subsystem;
        if ( this.SenseSubsystem is not null )
        {
            this.SenseSubsystem.PropertyChanged += this.SenseSubsystemPropertyChanged;
        }
    }

    /// <summary> Gets the function mode changed. </summary>
    /// <value> The function mode changed. </value>
    public bool FunctionModeChanged => !Nullable.Equals( this.SenseFunctionSubsystem?.FunctionMode, this.SenseSubsystem?.FunctionMode );

    /// <summary> Handles the function modes changed action. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected virtual void HandleFunctionModesChanged( SenseSubsystemBase subsystem )
    {
    }

    /// <summary> Handle the Sense subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( SenseSubsystemBase subsystem, string? propertyName )
    {
        if ( this.InitializingComponents || subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        // Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        // Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        switch ( propertyName ?? string.Empty )
        {
            case nameof( SenseSubsystemBase.FunctionMode ):
                {
                    if ( this.FunctionModeChanged && this.SenseSubsystem is not null )
                        this.HandleFunctionModesChanged( this.SenseSubsystem );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Sense subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void SenseSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( SenseSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.SenseSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.SenseSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is SenseSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " sense function subsystem "

    /// <summary> Gets the sense function subsystem. </summary>
    /// <value> The sense function subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public SenseFunctionSubsystemBase? SenseFunctionSubsystem { get; private set; }

    /// <summary> Bind Sense function subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( SenseFunctionSubsystemBase subsystem )
    {
        if ( this.SenseFunctionSubsystem is not null )
        {
            this.SenseFunctionSubsystem = null;
        }

        this.SenseFunctionSubsystem = subsystem;
        if ( this.SenseFunctionSubsystem is not null )
        {
        }
    }

    #endregion

    #region " binning subsystem "

    /// <summary> Gets the <see cref="VI.BinningSubsystemBase"/>. </summary>
    /// <value> The <see cref="VI.BinningSubsystemBase"/>. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public BinningSubsystemBase? BinningSubsystem { get; private set; }

    /// <summary> Bind <see cref="VI.BinningSubsystemBase"/>. </summary>
    /// <param name="subsystem"> The <see cref="VI.BinningSubsystemBase"/>. </param>
    public void BindSubsystem( BinningSubsystemBase subsystem )
    {
        if ( this.BinningSubsystem is not null )
        {
            this.BinningSubsystem = null;
        }

        this.BinningSubsystem = subsystem;
        if ( this.BinningSubsystem is not null )
        {
        }
    }

    #endregion

    #region " route subsystem "

    /// <summary> Gets the Route subsystem. </summary>
    /// <value> The Route subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public RouteSubsystemBase? RouteSubsystem { get; private set; }

    /// <summary> Bind Route subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( RouteSubsystemBase subsystem )
    {
        if ( this.RouteSubsystem is not null )
        {
            this.RouteSubsystem = null;
        }

        this.RouteSubsystem = subsystem;
        if ( this.RouteSubsystem is not null )
        {
        }
    }

    /// <summary>
    /// Configures four wire resistance scan using virtual instrument library functions.
    /// </summary>
    /// <remarks> David, 2020-04-11. </remarks>
    /// <param name="scanList">      List of scans. </param>
    /// <param name="sampleCount">   Number of samples. </param>
    /// <param name="triggerSource"> The trigger source. </param>
    /// <param name="triggerCount">  Number of triggers. </param>
    /// <returns> A <see cref="string" />. </returns>
    public string ConfigureFourWireResistanceScan( string scanList, int sampleCount, ArmSources triggerSource, int triggerCount )
    {
        string result = string.Empty;
        // this is required for getting the correct function mode when fetching buffers.
        _ = this.SenseSubsystem?.ApplyFunctionMode( SenseFunctionModes.ResistanceFourWire );
        _ = this.TriggerSubsystem?.ApplyContinuousEnabled( false );
        _ = this.RouteSubsystem?.ApplySelectedScanListType( ScanListType.None );
        if ( this.SenseSubsystem is not null )
            _ = this.RouteSubsystem?.ApplyScanListFunction( "(@1:10)", SenseFunctionModes.None, this.SenseSubsystem.FunctionModeReadWrites );
        if ( this.SenseSubsystem is not null )
            _ = this.RouteSubsystem?.ApplyScanListFunction( scanList, SenseFunctionModes.ResistanceFourWire, this.SenseSubsystem.FunctionModeReadWrites );
        _ = this.TriggerSubsystem?.ApplyTriggerCount( sampleCount );
        _ = this.TriggerSubsystem?.ApplyTriggerSource( TriggerSources.Immediate );
        _ = this.ArmLayer2Subsystem?.ApplyArmCount( triggerCount );
        _ = this.ArmLayer2Subsystem?.ApplyArmSource( triggerSource );
        if ( ArmSources.Timer == triggerSource )
        {
            _ = this.ArmLayer2Subsystem?.ApplyTimerTimeSpan( TimeSpan.FromMilliseconds( 600d ) );
        }

        this.TraceSubsystem?.ClearBuffer();
        _ = this.TraceSubsystem?.ApplyPointsCount( sampleCount );
        _ = this.TraceSubsystem?.ApplyFeedSource( FeedSources.Sense );
        _ = this.TraceSubsystem?.ApplyFeedControl( FeedControls.Next );
        _ = this.RouteSubsystem?.ApplySelectedScanListType( ScanListType.Internal );
        return result;
    }

    #endregion

    #region " configure buffer stream "

    /// <summary> Restore Trigger State. </summary>
    private void RestoreState()
    {
        if ( this.InitializingComponents || this.Device is null || this.Device.Session is null ) return;
        string activity = string.Empty;
        string title = this.Device.OpenResourceModel;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{title} aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.SetLastAction( "aborting the trigger subsystem" );
            this.TriggerSubsystem?.Abort();
            this.Device.Session.SetLastAction( "querying operation completion before resetting known state" );
            _ = this.Device.Session.QueryOperationCompleted();
            activity = $"{title} restoring device state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session.SetLastAction( "resetting known state" );
            this.Device.ResetKnownState();
            this.Device.Session.SetLastAction( "querying operation completion after resetting known state" );
            _ = this.Device.Session.QueryOperationCompleted();
            this.Device.Session.SetLastAction( "clearing execution state" );
            this.Device.ClearExecutionState();
            this.Device.Session.SetLastAction( "querying operation completion after clearing execution state" );
            _ = this.Device.Session.QueryOperationCompleted();
            _ = this.TriggerSubsystem?.ApplyContinuousEnabled( false );
            this.Device.Session.SetLastAction( "enabling monitoring of operation completion" );
            this.Device.Session.EnableServiceRequestOnOperationCompletion();
            this.Device.Session.StatusPrompt = "Instrument state restored";
            this._startBufferStreamMenuItem.Enabled = false;
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

    /// <summary> Configure trigger plan. </summary>
    /// <remarks> David, 2020-04-09. </remarks>
    /// <param name="triggerCount">  Number of triggers. </param>
    /// <param name="sampleCount">   Number of samples. </param>
    /// <param name="triggerSource"> The trigger source. </param>
    protected virtual void ConfigureTriggerPlan( int triggerCount, int sampleCount, TriggerSources triggerSource )
    {
        if ( this.Device is null || this.Device.Session is null
            || this.TriggerSubsystem is null || this.ArmLayer1Subsystem is null || this.ArmLayer2Subsystem is null ) return;
        _ = this.TriggerSubsystem.ApplyContinuousEnabled( false );
        this.TriggerSubsystem.Abort();
        this.TriggerSubsystem.ClearTriggerModel();
        this.Device.Session.Wait();
        this.Device.Session.ThrowDeviceExceptionIfError( this.Device.Session.ReadStatusByte() );

        _ = this.TriggerSubsystem.ApplyAutoDelayEnabled( false );
        this.TriggerSubsystem.Session.EnableServiceRequestOnOperationCompletion();
        this.TriggerSubsystem.Session.ThrowDeviceExceptionIfError( this.TriggerSubsystem.Session.ReadStatusByte() );

        _ = this.ArmLayer1Subsystem.ApplyArmSource( ArmSources.Immediate );
        _ = this.ArmLayer1Subsystem.ApplyArmCount( 1 );
        _ = this.ArmLayer1Subsystem.ApplyArmLayerBypassMode( TriggerLayerBypassModes.Acceptor );
        _ = this.ArmLayer2Subsystem.ApplyArmSource( ArmSources.Immediate );
        _ = this.ArmLayer2Subsystem.ApplyArmCount( sampleCount );
        _ = this.ArmLayer2Subsystem.ApplyDelay( TimeSpan.Zero );
        _ = this.ArmLayer2Subsystem.ApplyArmLayerBypassMode( TriggerLayerBypassModes.Acceptor );
        _ = this.TriggerSubsystem.ApplyTriggerSource( triggerSource );
        _ = this.TriggerSubsystem.ApplyTriggerCount( triggerCount );
        _ = this.TriggerSubsystem.ApplyDelay( TimeSpan.Zero );
        _ = this.TriggerSubsystem.ApplyTriggerLayerBypassMode( TriggerLayerBypassModes.Acceptor );
    }

    /// <summary> Configure measurement. </summary>
    /// <remarks> David, 2020-07-25. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    private void ConfigureMeasurement()
    {
        if ( this.Device is null || this.Device.Session is null
            || this.SystemSubsystem is null || this.SenseSubsystem is null || this.SenseFunctionSubsystem is null ) return;
        _ = this.SystemSubsystem.ApplyAutoZeroEnabled( true );
        _ = this.SenseSubsystem.ApplyFunctionMode( this.Settings.StreamBufferSenseFunctionMode );
        _ = this.Device.Session.QueryOperationCompleted();
        // verify that function mode change occurred
        if ( this.FunctionModeChanged )
        {
            throw new InvalidOperationException( $"Failed settings new {nameof( SenseFunctionSubsystemBase ).SplitWords()} to {this.Settings.StreamBufferSenseFunctionMode} from {this.SenseFunctionSubsystem.FunctionMode}" );
        }

        _ = this.SenseFunctionSubsystem.ApplyPowerLineCycles( 1d );
        bool autoRangeEnabled = true;
        bool? actualAutoRangeEnabled = this.SenseFunctionSubsystem.ApplyAutoRangeEnabled( autoRangeEnabled );
        if ( !Nullable.Equals( actualAutoRangeEnabled, autoRangeEnabled ) )
        {
            throw new InvalidOperationException( $"Failed settings {nameof( SenseFunctionSubsystemBase.AutoRangeEnabled ).SplitWords()} to {autoRangeEnabled}" );
        }

        _ = this.SenseFunctionSubsystem.ApplyResolutionDigits( 9d );
        _ = this.SenseFunctionSubsystem.ApplyOpenLeadDetectorEnabled( true );
        // Me.SenseFunctionSubsystem.ApplyPowerLineCycles(1)
        // Me.SenseFunctionSubsystem.ApplyRange(My.Settings.NominalResistance)

    }

    /// <summary> Configure digital output. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected virtual void ConfigureDigitalOutput( DigitalOutputSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.ApplyDigitalActiveLevels( [1, 2, 3, 4], [DigitalActiveLevels.Low, DigitalActiveLevels.Low, DigitalActiveLevels.Low, DigitalActiveLevels.Low] );
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Configure limit binning. </summary>
    protected virtual void ConfigureLimitBinning()
    {
        if ( this.BinningSubsystem is null ) return;
        double expectedResistance = this.Settings.NominalResistance;
        double resistanceTolerance = this.Settings.ResistanceTolerance;
        double openLimit = this.Settings.OpenLimit;
        int passOutputValue = this.Settings.PassBitmask;
        int failOutputValue = this.Settings.FailBitmask;
        int overflowOutputValue = this.Settings.OverflowBitmask;

        // sets the expected duration of the binning probe, which is used to wait before 
        // enabling the next measurement cycle. 
        this.BinningSubsystem.BinningStrobeDuration = TimeSpan.FromMilliseconds( this.Settings.BinningStrobeDuration );
        _ = this.BinningSubsystem.ApplyPassSource( passOutputValue );

        // limit 2 is set for the nominal values
        double expectedValue = expectedResistance * (1d - resistanceTolerance);
        _ = this.BinningSubsystem.ApplyLimit2LowerLevel( expectedValue );
        expectedValue = expectedResistance * (1d + resistanceTolerance);
        _ = this.BinningSubsystem.ApplyLimit2UpperLevel( expectedValue );
        _ = this.BinningSubsystem.ApplyLimit2AutoClear( true );
        _ = this.BinningSubsystem.ApplyLimit2Enabled( true );
        _ = this.BinningSubsystem.ApplyLimit2LowerSource( failOutputValue );
        _ = this.BinningSubsystem.ApplyLimit2UpperSource( failOutputValue );

        // limit 1 is set for the overflow
        expectedValue = expectedResistance * resistanceTolerance;
        _ = this.BinningSubsystem.ApplyLimit1LowerLevel( expectedValue );
        _ = this.BinningSubsystem.ApplyLimit1UpperLevel( openLimit );
        _ = this.BinningSubsystem.ApplyLimit1AutoClear( true );
        _ = this.BinningSubsystem.ApplyLimit1Enabled( true );
        _ = this.BinningSubsystem.ApplyLimit1LowerSource( overflowOutputValue );
        _ = this.BinningSubsystem.ApplyLimit1UpperSource( overflowOutputValue );
        _ = this.BinningSubsystem.ApplyBinningStrobeEnabled( true );
    }

    /// <summary> Configure trace for fetching only. </summary>
    /// <param name="binningStrokeDuration"> Duration of the binning stroke. </param>
    protected virtual void ConfigureTrace( TimeSpan binningStrokeDuration )
    {
        if ( this.TraceSubsystem is null ) return;
        this.TraceSubsystem.ClearBuffer();
        this.TraceSubsystem.BinningDuration = binningStrokeDuration;
        _ = this.TraceSubsystem.ApplyFeedSource( FeedSources.Sense );
        _ = this.TraceSubsystem.ApplyFeedControl( FeedControls.Never );
    }

    /// <summary> Builds buffer streaming description. </summary>
    /// <returns> A <see cref="string" />. </returns>
    public virtual string BuildBufferStreamingDescription()
    {
        if ( this.InitializingComponents ) return string.Empty;
        System.Text.StringBuilder builder = new();
        _ = builder.AppendLine( $"Buffer streaming plan:" );
        _ = builder.AppendLine( $"Measurement: {this.Settings.StreamBufferSenseFunctionMode}; auto zero; {1} NPLC; auto range; 8.5 digits." );
        _ = builder.AppendLine( $"Limits, Ω: Pass: {this.Settings.NominalResistance}±{this.Settings.NominalResistance * this.Settings.ResistanceTolerance}; Open: {this.Settings.OpenLimit}." );
        _ = builder.AppendLine( $"Binning Bitmasks: Pass={this.Settings.PassBitmask}; Fail={this.Settings.FailBitmask}; Open={this.Settings.OverflowBitmask}." );
        _ = builder.AppendLine( $"Using Scan Card: {this._usingScanCardMenuItem.Checked}" );
        if ( this._usingScanCardMenuItem.Checked )
        {
            _ = builder.AppendLine( $"Arm layer 1: {1} count; {ArmSources.Immediate}; {0} delay." );
            _ = builder.AppendLine( $"Arm layer 2: {this.Settings.StreamTriggerCount} count; {this.Settings.StreamBufferArmSource}; {0} delay." );
            _ = builder.AppendLine( $"Trigger: {ArmSources.Immediate}; {this.Settings.ScanCardSampleCount} count; {0} delay; {TriggerLayerBypassModes.Acceptor} bypass." );
        }
        else
        {
            _ = builder.AppendLine( $"Arm layer 1: {1} count; {ArmSources.Immediate}; {0} delay." );
            _ = builder.AppendLine( $"Arm layer 2: {1} count; {ArmSources.Immediate}; {0} delay." );
            _ = builder.AppendLine( $"Trigger: {this.Settings.StreamBufferTriggerSource}; {this.Settings.StreamTriggerCount} count; {0} delay; {TriggerLayerBypassModes.Acceptor} bypass." );
        }

        return builder.ToString().TrimEnd( Environment.NewLine.ToCharArray() );
    }

    /// <summary> Configure buffer stream. </summary>
    private void ConfigureBufferStream()
    {
        if ( this.InitializingComponents || this.TraceSubsystem is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} Configure trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.ConfigureTriggerPlan( this.Settings.StreamTriggerCount, 1, this.Settings.StreamBufferTriggerSource );
            if ( this.SenseSubsystem is not null && (!this.SenseSubsystem.FunctionMode.HasValue
                || ( int? ) this.SenseSubsystem.FunctionMode != ( int? ) this.Settings.StreamBufferSenseFunctionMode) )
                this.ConfigureMeasurement();
            if ( this.BinningSubsystem is not null && (!this.BinningSubsystem.Limit1AutoClear.GetValueOrDefault( false ) || !this.BinningSubsystem.Limit1Enabled.GetValueOrDefault( false )) )
            {
                this.ConfigureLimitBinning();
                this.ConfigureDigitalOutput( this.DigitalOutputSubsystem );
            }

            if ( this.TraceSubsystem is not null && this.TraceSubsystem.FeedSource.GetValueOrDefault( FeedSources.None ) != FeedSources.Sense )
            {
                this.ConfigureTrace( this.BinningSubsystem is not null ? this.BinningSubsystem.BinningStrobeDuration
                    : TimeSpan.FromMilliseconds( this.Settings.BinningStrobeDuration ) );
            }

            if ( this._usingScanCardMenuItem.Checked )
            {
                // the fetched buffer includes only reading values.
                this.TraceSubsystem!.OrderedReadingElementTypes = [ReadingElementTypes.Reading];
                _ = this.ConfigureFourWireResistanceScan( this.Settings.ScanCardScanList,
                    this.Settings.ScanCardSampleCount, this.Settings.StreamBufferArmSource, this.Settings.StreamTriggerCount );
            }

            _ = this.BufferSubsystem is not null
                ? (this.DataGridView?.Bind( this.BufferSubsystem.BufferReadingsBindingList, true ))
                : this.DataGridView?.Bind( this.TraceSubsystem!.BufferReadingsBindingList, true );

            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = "Ready to commence buffer stream";
            this._startBufferStreamMenuItem.Enabled = true;
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

    #region " commence buffer stream "

    /// <summary> Gets the buffer streaming enabled. </summary>
    /// <value> The buffer streaming enabled. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public bool BufferStreamingEnabled => this.BufferSubsystem is not null
                ? this.BufferSubsystem.BufferStreamingEnabled
                : this.TraceSubsystem is not null && this.TraceSubsystem.BufferStreamingEnabled;

    /// <summary> Gets the buffer streaming Active. </summary>
    /// <value> The buffer streaming Active. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public bool BufferStreamingActive => this.BufferSubsystem is not null
                ? this.BufferSubsystem.BufferStreamingActive
                : this.TraceSubsystem is not null && this.TraceSubsystem.BufferStreamingActive;

    /// <summary> Stops buffer streaming. </summary>
    private void StopBufferStreaming()
    {
        if ( this.InitializingComponents || this.TraceSubsystem is null || this.TriggerSubsystem is null ) return;
        TimeSpan timeout = TraceSubsystemBase.EstimateStreamStopTimeoutInterval( this.TraceSubsystem.StreamCycleDuration,
            TimeSpan.FromMilliseconds( this.Settings.BufferStreamPollInterval ), 1.5d );
        if ( this.BufferSubsystem is not null )
            this.BufferSubsystem.BufferStreamTasker.AsyncCompleted -= this.BufferStreamTasker_asyncCompleted;
        if ( this.TraceSubsystem is not null )
            this.TraceSubsystem.BufferStreamTasker.AsyncCompleted -= this.BufferStreamTasker_asyncCompleted;
        (bool success, string details) = this.BufferSubsystem is not null
            ? this.BufferSubsystem.StopBufferStream( timeout )
            : this.TraceSubsystem!.StopBufferStream( timeout );
        if ( success )
        {
            this.TriggerSubsystem.Abort();
            this.ReadStatusRegister();
            _ = this.TriggerSubsystem.QueryTriggerState();
            this.ReadStatusRegister();
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"buffer streaming failed {details}" );
        }
    }

    /// <summary>
    /// Event handler. Called by BufferStreamTasker for asynchronous completed events.
    /// </summary>
    /// <remarks> David, 2020-08-06. </remarks>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Asynchronous completed event information. </param>
    private void BufferStreamTasker_asyncCompleted( object? sender, AsyncCompletedEventArgs e )
    {
        string activity = "Handling buffer stream completed event";
        if ( sender is null || e is null )
            return;
        try
        {
            if ( e.Error is not null )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( e.Error, activity );
                this.StopBufferStreaming();
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Starts buffer streaming. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void StartBufferStreaming( TraceSubsystemBase subsystem )
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null || this.SenseSubsystem is null ) return;
        this.Device.ClearExecutionState();
        this.TriggerSubsystem.Initiate();
        subsystem.BufferStreamTasker.AsyncCompleted += this.BufferStreamTasker_asyncCompleted;
        subsystem.StartBufferStream( this.TriggerSubsystem,
                                     TimeSpan.FromMilliseconds( this.Settings.BufferStreamPollInterval ), this.SenseSubsystem.FunctionUnit );
    }

    /// <summary> Starts buffer streaming. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void StartBufferStreaming( BufferSubsystemBase subsystem )
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null || this.SenseSubsystem is null ) return;
        this.Device.ClearExecutionState();
        subsystem.BufferReadingUnit = this.SenseSubsystem.FunctionUnit;
        this.TriggerSubsystem.Initiate();
        subsystem.BufferStreamTasker.AsyncCompleted += this.BufferStreamTasker_asyncCompleted;
        subsystem.StartBufferStream( this.TriggerSubsystem, TimeSpan.FromMilliseconds( this.Settings.BufferStreamPollInterval ) );
    }

    /// <summary> Commence buffer stream. </summary>
    private void CommenceBufferStream()
    {
        if ( this.InitializingComponents || this.Device is null
            || this.Device.StatusSubsystemBase is null || this.Device.Session is null || this.TraceSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} Commencing buffer stream";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.BufferSubsystem is not null )
            {
                this.StartBufferStreaming( this.BufferSubsystem );
            }
            else
            {
                this.StartBufferStreaming( this.TraceSubsystem );
            }

            if ( this.Device.StatusSubsystemBase.Session.ErrorAvailable )
            {
                this.StopBufferStreaming();
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning(
                    $"{activity} encountered device errors: {this.Device.StatusSubsystemBase.Session.DeviceErrorPreamble}\n{this.Device.StatusSubsystemBase.Session.DeviceErrorReport}" );
                this.Device.Session.StatusPrompt = "Streaming aborted";
            }
            else
            {
                this.Device.Session.StatusPrompt = this._usingScanCardMenuItem.Checked ? $"Streaming started--awaiting {this.Settings.StreamBufferArmSource} triggers" : $"Streaming started--awaiting {this.Settings.StreamBufferTriggerSource} triggers";
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

    #region " control event handlers: stream buffer "

    /// <summary> Starts buffer stream. </summary>
    public void StartBufferStream()
    {
        if ( this.InitializingComponents ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            if ( this._startBufferStreamMenuItem.Checked )
            {
                this.CommenceBufferStream();
            }
            else
            {
                activity = $"{this.Device?.ResourceNameCaption} stopping buffer streaming";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.StopBufferStreaming();
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

    /// <summary> Stream buffer menu item check state changed. </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void StartBufferStreamMenuItem_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.StartBufferStream();
        this._assertBusTriggerButton.Enabled = this._startBufferStreamMenuItem.Checked && (this._usingScanCardMenuItem.Checked ? ArmSources.Bus == this.Settings.StreamBufferArmSource : TriggerSources.Bus == this.Settings.StreamBufferTriggerSource);
    }

    /// <summary> Configure streaming menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ConfigureStreamingMenuItem_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SystemSubsystem is null ) return;
        if ( this._configureStreamingMenuItem.Checked )
        {
            if ( this._usingScanCardMenuItem.Checked && this.SystemSubsystem.QueryFrontTerminalsSelected().GetValueOrDefault( false ) )
            {
                _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, "Set Rear Terminal" );
                return;
            }

            this.ConfigureBufferStream();
        }
        else
        {
            this.RestoreState();
        }
    }

    /// <summary> Explain buffer streaming menu item click. </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Event information. </param>
    private void ExplainBufferStreamingMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"{this.Device?.ResourceNameCaption} building external scan trigger plan description";
        try
        {
            cc.isr.WinControls.PopupContainer.PopupInfo( this, this.BuildBufferStreamingDescription(), this._subsystemToolStrip.Location, this.Size );
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            this.InfoProvider?.SetIconPadding( this, -15 );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
        }
    }

    /// <summary> Assert bus trigger button click. </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Event information. </param>
    private void AssertBusTriggerButton_Click( object? sender, EventArgs e )
    {
        if ( this.TraceSubsystem is not null )
            this.TraceSubsystem.BusTriggerRequested = true;
    }

    #endregion

    #region " control event handlers "

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
    private void DataGridView_DataError( object? sender, DataGridViewDataErrorEventArgs e )
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

    /// <summary> Displays a buffer menu item click. </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Event information. </param>
    private void DisplayBufferMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.DisplayBufferReadings();
    }

    /// <summary> Clears the buffer display menu item click. </summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Event information. </param>
    private void ClearBufferDisplayMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ClearBufferDisplay();
    }

    #endregion
}
