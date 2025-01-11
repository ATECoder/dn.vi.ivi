using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;
using cc.isr.Enums.WinControls.ComboBoxEnumExtensions;
using cc.isr.WinControls.NumericUpDownExtensions;
using cc.isr.VI.SubsystemsWinControls.DataGridViewExtensions;
using cc.isr.WinControls.CheckStateExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A DMM meter view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class MeterView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public MeterView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this._toolStripPanel.SuspendLayout();
        int topLocation = 0;
        foreach ( ToolStrip ts in new ToolStrip[] { this._subsystemToolStrip, this._readingToolStrip, this._functionConfigurationToolStrip, this._filterToolStrip, this._infoToolStrip } )
        {
            ts.Dock = DockStyle.None;
            ts.Location = new System.Drawing.Point( 0, topLocation );
            topLocation += ts.Height;
        }

        this._toolStripPanel.ResumeLayout();
        this.InitializingComponents = false;
        this._apertureNumeric.NumericUpDown.Increment = 0.1m;
        this._apertureNumeric.NumericUpDown.Minimum = 0.001m;
        this._apertureNumeric.NumericUpDown.Maximum = 25m;
        this._apertureNumeric.NumericUpDown.DecimalPlaces = 3;
        this._apertureNumeric.NumericUpDown.Value = 1m;
        this._senseRangeNumeric.NumericUpDown.Increment = 1m;
        this._senseRangeNumeric.NumericUpDown.Minimum = 0m;
        this._senseRangeNumeric.NumericUpDown.Maximum = 1010m;
        this._senseRangeNumeric.NumericUpDown.DecimalPlaces = 3;
        this._senseRangeNumeric.NumericUpDown.Value = 0.105m;
        this._senseRangeNumeric.NumericUpDown.MaximumSize = new System.Drawing.Size( this._senseRangeNumeric.Size.Width + this._senseRangeNumeric.Size.Width, this._senseRangeNumeric.Size.Height + 2 );
        this._filterCountNumeric.NumericUpDown.Increment = 1m;
        this._filterCountNumeric.NumericUpDown.Minimum = 0m;
        this._filterCountNumeric.NumericUpDown.Maximum = 100m;
        this._filterCountNumeric.NumericUpDown.DecimalPlaces = 0;
        this._filterCountNumeric.NumericUpDown.Value = 1m;
        this._filterWindowNumeric.NumericUpDown.Increment = 1m;
        this._filterWindowNumeric.NumericUpDown.Minimum = 0m;
        this._filterWindowNumeric.NumericUpDown.Maximum = 10m;
        this._filterWindowNumeric.NumericUpDown.DecimalPlaces = 2;
        this._filterWindowNumeric.NumericUpDown.Value = 10m;
        this._resolutionDigitsNumeric.NumericUpDown.Increment = 1m;
        this._resolutionDigitsNumeric.NumericUpDown.Minimum = 4m;
        this._resolutionDigitsNumeric.NumericUpDown.Maximum = 9m;
        this._resolutionDigitsNumeric.NumericUpDown.DecimalPlaces = 0;
        this._resolutionDigitsNumeric.NumericUpDown.Value = 7m;
    }

    /// <summary> Creates a new <see cref="MeterView"/> </summary>
    /// <returns> A <see cref="MeterView"/>. </returns>
    public static MeterView Create()
    {
        MeterView? view = null;
        try
        {
            view = new MeterView();
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

    /// <summary> Gets or sets the supports measurement events. </summary>
    /// <value> The supports measurement events. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public bool SupportsMeasurementEvents
    {
        get => this._measureOptionsMenuItem.Enabled;
        set
        {
            if ( value != this.SupportsMeasurementEvents )
            {
                this._measureOptionsMenuItem.Enabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

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
            {
                this._dataGridViewThis.DataError -= this.DataGridView_DataError;
            }

            this._dataGridViewThis = value;
            if ( this._dataGridViewThis is not null )
            {
                this._dataGridViewThis.DataError += this.DataGridView_DataError;
            }
        }
    }

    /// <summary> Gets or sets or set the data grid view. </summary>
    /// <value> The data grid view. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public DataGridView? DataGridView
    {
        get => this.DataGridViewThis;
        set => this.DataGridViewThis = value;
    }

    /// <summary> Gets or sets the selected multimeter subsystem function mode. </summary>
    /// <value> The selected multimeter function mode. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public MultimeterFunctionModes SelectedMultimeterFunctionMode
    {
        get => this._senseFunctionComboBox.SelectedEnumValue( MultimeterFunctionModes.CurrentAC );
        set
        {
            if ( value != this.SelectedMultimeterFunctionMode && this._senseFunctionComboBox.ComboBox.Items.Count > 0 )
            {
                this.InitializingComponents = this.Device is not null && this.Device.IsDeviceOpen;
                _ = this._senseFunctionComboBox.SelectValue( value );
                this.NotifyPropertyChanged();
                this.InitializingComponents = false;
            }
        }
    }

    /// <summary> Gets the selected sense function mode. </summary>
    /// <value> The selected sense function mode. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public SenseFunctionModes SelectedSenseFunctionMode => this._senseFunctionComboBox.SelectedEnumValue( SenseFunctionModes.CurrentAC );

    /// <summary> Gets or sets the name of the subsystem. </summary>
    /// <value> The name of the subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string SubsystemName
    {
        get => this._readSplitButton?.Text ?? string.Empty;
        set
        {
            if ( !string.Equals( value, this.SubsystemName, StringComparison.Ordinal ) )
            {
                this._readSplitButton.Text = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Applies the function mode. </summary>
    public void ApplyFunctionMode()
    {
        if ( this.Device is null || this.SenseSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} applying {this.SubsystemName} function mode";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null && !Nullable.Equals( this.SelectedMultimeterFunctionMode, this.MultimeterSubsystem.FunctionMode ) )
            {
                this.MultimeterSubsystem.StartElapsedStopwatch();
                _ = this.MultimeterSubsystem.ApplyFunctionMode( this.SelectedMultimeterFunctionMode );
                this.MultimeterSubsystem.StopElapsedStopwatch();
            }
            else if ( this.SenseSubsystem is not null && this.SenseSubsystem.FunctionMode is not null )
            {
                if ( this.SelectedSenseFunctionMode != this.SenseSubsystem.FunctionMode.Value )
                {
                    this.SenseSubsystem.StartElapsedStopwatch();
                    _ = this.SenseSubsystem.ApplyFunctionMode( this.SelectedSenseFunctionMode );
                    this.SenseSubsystem.StopElapsedStopwatch();
                }
                else if ( this.SenseFunctionSubsystem is null || !Nullable.Equals( this.SenseFunctionSubsystem.FunctionMode, this.SenseSubsystem.FunctionMode ) )
                {
                    this.HandleFunctionModesChanged( this.SenseSubsystem );
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
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Reads function mode. </summary>
    public void ReadFunctionMode()
    {
        if ( this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} Reading {this.SubsystemName} function mode";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null )
            {
                this.MultimeterSubsystem.StartElapsedStopwatch();
                _ = this.MultimeterSubsystem.QueryFunctionMode();
                this.MultimeterSubsystem.StopElapsedStopwatch();
            }
            else if ( this.SenseSubsystem is not null )
            {
                this.SenseSubsystem.StartElapsedStopwatch();
                _ = this.SenseSubsystem.QueryFunctionMode();
                this.SenseSubsystem.StopElapsedStopwatch();
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

    /// <summary> Applies the settings onto the instrument. </summary>
    public void ApplySettings()
    {
        if ( this.Device is null || this.SenseFunctionSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device?.ResourceNameCaption} applying {this.SubsystemName} settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null && this.MultimeterSubsystem.FunctionMode is not null )
            {
                if ( this.SelectedMultimeterFunctionMode != this.MultimeterSubsystem.FunctionMode.Value )
                {
                    _ = this.InfoProvider?.Annunciate( this._applyFunctionModeButton, cc.isr.WinControls.InfoProviderLevel.Info, "Set function first" );
                }
                else
                {
                    this.ApplySettings( this.MultimeterSubsystem );
                }
            }
            else if ( this.SenseSubsystem is not null && this.SenseSubsystem.FunctionMode is not null )
            {
                if ( this.SelectedSenseFunctionMode != this.SenseFunctionSubsystem.FunctionMode )
                {
                    _ = this.InfoProvider?.Annunciate( this._applyFunctionModeButton, cc.isr.WinControls.InfoProviderLevel.Info, "Set function first" );
                }
                else
                {
                    this.ApplySettings( this.SenseFunctionSubsystem );
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
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Reads the settings from the instrument. </summary>
    public void ReadSettings()
    {
        if ( this.InitializingComponents || this.Device is null || this.FormatSubsystem is null || this.SystemSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} reading {this.SubsystemName} settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null )
            {
                if ( this.MultimeterSubsystem is null || this.MultimeterSubsystem.FunctionMode is null || this.SelectedMultimeterFunctionMode != this.MultimeterSubsystem.FunctionMode.Value )
                {
                    _ = this.InfoProvider?.Annunciate( this._applyFunctionModeButton, cc.isr.WinControls.InfoProviderLevel.Info, "Set function first" );
                }
                else
                {
                    ReadSettings( this.MultimeterSubsystem );
                    this.ApplyPropertyChanged( this.MultimeterSubsystem );
                }
            }
            else if ( this.SenseSubsystem is not null )
            {
                // this should select the sense function subsystem.
                ReadSettings( this.SenseSubsystem );
                this.ApplyPropertyChanged( this.SenseSubsystem );
                ReadSettings( this.FormatSubsystem );
                this.ApplyPropertyChanged( this.FormatSubsystem );
                ReadSettings( this.SystemSubsystem );
                this.ApplyPropertyChanged( this.SystemSubsystem );
                if ( this.SenseFunctionSubsystem is null || this.SenseFunctionSubsystem.FunctionMode is null || this.SelectedSenseFunctionMode != this.SenseFunctionSubsystem.FunctionMode.Value )
                {
                    this.HandleFunctionModesChanged( this.SenseSubsystem );
                }
                else
                {
                    ReadSettings( this.SenseFunctionSubsystem );
                    this.ApplyPropertyChanged( this.SenseFunctionSubsystem );
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
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Reads terminal state. </summary>
    public void ReadTerminalState()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} Reading {this.SubsystemName} terminals state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null )
            {
                _ = this.MultimeterSubsystem.QueryFrontTerminalsSelected();
            }
            else if ( this.SystemSubsystem is not null )
            {
                _ = this.SystemSubsystem.QueryFrontTerminalsSelected();
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

    /// <summary> Measure value. </summary>
    public void MeasureValue()
    {
        if ( this.InitializingComponents || this.Device is null || this.MeasureSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} querying terminal mode";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null )
            {
                _ = this.MultimeterSubsystem.QueryFrontTerminalsSelected();
            }
            else if ( this.SystemSubsystem is not null )
            {
                _ = this.SystemSubsystem.QueryFrontTerminalsSelected();
            }

            activity = $"{this.Device.ResourceNameCaption} measuring";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null )
            {
                Read( this.MultimeterSubsystem );
            }
            else
            {
                Read( this.MeasureSubsystem );
            }

            _ = this.Device.StatusSubsystemBase?.QueryMeasurementEventStatus();
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

    /// <summary> Gets or sets the type of the selected reading element. </summary>
    /// <value> The type of the selected reading element. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ReadingElementTypes SelectedReadingElementType
    {
        get => this._readingElementTypesComboBox.SelectedEnumValue( ReadingElementTypes.Reading );
        set
        {
            if ( value != this.SelectedReadingElementType && this._readingElementTypesComboBox.ComboBox.Items.Count > 0 )
            {
                _ = this._readingElementTypesComboBox.SelectValue( value );
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets a the types of reading elements. </summary>
    /// <value> A list of types of the reading elements. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public ReadingElementTypes ReadingElementTypes { get; private set; }

    /// <summary> Reading element types setter. </summary>
    /// <param name="candidateReadingElementTypes"> The candidates of reading element type. </param>
    public void ReadingElementTypesSetter( ReadingElementTypes candidateReadingElementTypes )
    {
        if ( candidateReadingElementTypes != this.ReadingElementTypes )
        {
            this.ReadingElementTypes = candidateReadingElementTypes & ~ReadingElementTypes.Units;
            try
            {
                this.InitializingComponents = true;
                this._readingElementTypesComboBox.ComboBox.ListEnumDescriptions( candidateReadingElementTypes, ReadingElementTypes.Units );
            }
            catch
            {
                throw;
            }
            finally
            {
                this.InitializingComponents = false;
            }

            this.NotifyPropertyChanged( nameof( this.ReadingElementTypes ) );
        }
    }

    /// <summary> Select active reading. </summary>
    public void SelectActiveReading()
    {
        if ( this.InitializingComponents ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} selecting a reading to display";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            if ( this.MultimeterSubsystem is not null )
            {
                this.MultimeterSubsystem.SelectActiveReading( this.SelectedReadingElementType );
            }
            else
            {
                this.MeasureSubsystem?.SelectActiveReading( this.SelectedReadingElementType );
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

    #region " device "

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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( MeterView ).SplitWords()}" );
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

    #region " public members: buffer display "

    /// <summary> Gets or sets the buffer readings. </summary>
    /// <value> The buffer readings. </value>
    private BufferReadingBindingList BufferReadings { get; set; } = [];

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

    #region " buffer subsystem "

    /// <summary> Gets or sets the Buffer subsystem. </summary>
    /// <value> The Buffer subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public BufferSubsystemBase? BufferSubsystem { get; private set; }

    /// <summary> Bind Buffer subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( BufferSubsystemBase subsystem )
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
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, BufferSubsystemBase subsystem )
    {
        if ( add )
            _ = (this.DataGridView?.Bind( subsystem.BufferReadingsBindingList, true ));
    }

    /// <summary> Reads a buffer. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ReadBuffer( BufferSubsystemBase subsystem )
    {
        if ( this.InitializingComponents ) return;
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

    #region " format subsystem "

    /// <summary> Gets or sets the Format subsystem. </summary>
    /// <value> The Format subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    private FormatSubsystemBase? FormatSubsystem { get; set; }

    /// <summary> Bind format subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( FormatSubsystemBase subsystem )
    {
        if ( this.FormatSubsystem is not null )
        {
            this.BindSubsystem( false, this.FormatSubsystem );
            this.FormatSubsystem = null;
        }

        this.FormatSubsystem = subsystem;
        if ( this.FormatSubsystem is not null )
        {
            this.BindSubsystem( true, this.FormatSubsystem );
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, FormatSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.FormatSubsystemPropertyChanged;
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.FormatSubsystemPropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( FormatSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( FormatSubsystemBase.Elements ) );
    }

    /// <summary> Reads the selected measurements settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( FormatSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryElements();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handle the format subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
#if NET5_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "<Pending>" )]
#endif
    private void HandlePropertyChanged( FormatSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            default:
                break;
                // removed: Elements are set from either measure  or multimeter subsystem
                // Case NameOf(FormatSubsystemBase.Elements)
                // Me.ReadingElementTypesSetter(subsystem.Elements)
        }
    }

    /// <summary> Format subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void FormatSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( this.FormatSubsystem )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.FormatSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.FormatSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is FormatSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " harmonics measure subsystem "

    /// <summary> Gets or sets the HarmonicsHarmonics Measure Subsystem. </summary>
    /// <value> The HarmonicsHarmonics Measure Subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    private HarmonicsMeasureSubsystemBase? HarmonicsMeasureSubsystem { get; set; }

    /// <summary> Bind HarmonicsHarmonics Measure Subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( HarmonicsMeasureSubsystemBase subsystem )
    {
        if ( this.HarmonicsMeasureSubsystem is not null )
        {
            this.BindSubsystem( false, this.HarmonicsMeasureSubsystem );
            this.HarmonicsMeasureSubsystem = null;
        }

        this.HarmonicsMeasureSubsystem = subsystem;
        if ( this.HarmonicsMeasureSubsystem is not null )
        {
            this.BindSubsystem( true, this.HarmonicsMeasureSubsystem );
            this.HandlePropertyChanged( subsystem, nameof( HarmonicsMeasureSubsystemBase.ReadingElementTypes ) );
            this.SelectActiveReading();
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, HarmonicsMeasureSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.HarmonicsMeasureSubsystemPropertyChanged;
        }
        else
        {
            subsystem.PropertyChanged -= this.HarmonicsMeasureSubsystemPropertyChanged;
        }
    }

    /// <summary> Handle the HarmonicsHarmonics Measure Subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( HarmonicsMeasureSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( HarmonicsMeasureSubsystemBase.PrimaryReadingValue ):
                {
                    break;
                }

            case nameof( HarmonicsMeasureSubsystemBase.ReadingElementTypes ):
                {
                    this.ReadingElementTypesSetter( subsystem.ReadingElementTypes );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary>   HarmonicsHarmonics Measure Subsystem property changed. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Property changed event information. </param>
    private void HarmonicsMeasureSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( this.HarmonicsMeasureSubsystem )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.HarmonicsMeasureSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.HarmonicsMeasureSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is HarmonicsMeasureSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Reads the given subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void Read( HarmonicsMeasureSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.Read();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handles the harmonics measure mode changed action. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected virtual void HandleHarmonicsMeasureModeChanged( HarmonicsMeasureSubsystemBase subsystem )
    {
        if ( subsystem is not null && subsystem.MeasureMode.HasValue )
        {
            // var value = subsystem.MeasureMode.Value;
            // was not used: bool valueChanged = this.SelectedSenseFunctionMode != subsystem.FunctionMode.Value;
            // this is done at the device level by applying the function mode to the measure subsystem
            // Me.MeasureSubsystem.Readings.Reading.ApplyUnit(subsystem.ToUnit(value))
            _ = this._senseFunctionComboBox.SelectValue( subsystem.MeasureMode.Value );
            this.NotifyPropertyChanged( nameof( this.SelectedHarmonicsMeasureMode ) );
        }
    }

    /// <summary> Gets the selected harmonics measure mode. </summary>
    /// <value> The selected harmonics measure mode. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public HarmonicsMeasureMode SelectedHarmonicsMeasureMode => this._senseFunctionComboBox.SelectedEnumValue( HarmonicsMeasureMode.Voltage );



    #endregion

    #region " measure subsystem "

    /// <summary> Gets or sets the Measure subsystem. </summary>
    /// <value> The Measure subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    private MeasureSubsystemBase? MeasureSubsystem { get; set; }

    /// <summary> Bind Measure subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( MeasureSubsystemBase subsystem )
    {
        if ( this.MeasureSubsystem is not null )
        {
            this.BindSubsystem( false, this.MeasureSubsystem );
            this.MeasureSubsystem = null;
        }

        this.MeasureSubsystem = subsystem;
        if ( this.MeasureSubsystem is not null )
        {
            this.BindSubsystem( true, this.MeasureSubsystem );
            this.HandlePropertyChanged( subsystem, nameof( MeasureSubsystemBase.ReadingElementTypes ) );
            this.SelectActiveReading();
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, MeasureSubsystemBase subsystem )
    {
        if ( add )
            subsystem.PropertyChanged += this.MeasureSubsystemPropertyChanged;
        else
            subsystem.PropertyChanged -= this.MeasureSubsystemPropertyChanged;
    }

    /// <summary> Handle the Measure subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( MeasureSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( MeasureSubsystemBase.PrimaryReadingValue ):
                break;

            case nameof( MeasureSubsystemBase.ReadingElementTypes ):
                {
                    this.ReadingElementTypesSetter( subsystem.ReadingElementTypes );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary>   Measure subsystem property changed. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Property changed event information. </param>
    private void MeasureSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( this.MeasureSubsystem )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.MeasureSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.MeasureSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is MeasureSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Reads the given subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void Read( MeasureSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.Read();
        subsystem.StopElapsedStopwatch();
    }

    #endregion

    #region " mutlimeter subsystem "

    /// <summary> Gets or sets the Multimeter subsystem. </summary>
    /// <value> The Multimeter subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public MultimeterSubsystemBase? MultimeterSubsystem { get; private set; }

    /// <summary> Bind Multimeter subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( MultimeterSubsystemBase subsystem )
    {
        if ( this.MultimeterSubsystem is not null )
        {
            this.BindSubsystem( false, this.MultimeterSubsystem );
            this.MultimeterSubsystem = null;
        }

        this.MultimeterSubsystem = subsystem;
        if ( this.MultimeterSubsystem is not null )
        {
            this.BindSubsystem( true, this.MultimeterSubsystem );
        }

        this._resolutionDigitsNumeric.Visible = false;
        this.SubsystemName = "DMM";
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, MultimeterSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.MultimeterSubsystemPropertyChanged;
            this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.SupportedFunctionModes ) );
            this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FunctionRange ) );
            this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.ReadingElementTypes ) );
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
            this.SelectActiveReading();
        }
        else
        {
            subsystem.PropertyChanged -= this.MultimeterSubsystemPropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( MultimeterSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FilterCountRange ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FilterWindowRange ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FunctionRange ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FunctionRangeDecimalPlaces ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.PowerLineCyclesRange ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.PowerLineCyclesDecimalPlaces ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.AutoDelayMode ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FunctionMode ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.AutoDelayEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.AutoRangeEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.AutoZeroEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FilterCount ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FilterEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FilterWindow ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.MovingAverageFilterEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FrontTerminalsSelected ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.FunctionUnit ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.OpenDetectorEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.PowerLineCycles ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.Range ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.ReadingAmounts ) );
        this.HandlePropertyChanged( subsystem, nameof( MultimeterSubsystemBase.ReadingElementTypes ) );
    }

    /// <summary> Reads the selected measurements settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( MultimeterSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryFunctionMode();
        _ = subsystem.QueryAutoDelayMode();
        _ = subsystem.QueryAutoRangeEnabled();
        _ = subsystem.QueryAutoZeroEnabled();
        _ = subsystem.QueryFilterCount();
        _ = subsystem.QueryFilterEnabled();
        _ = subsystem.QueryFilterWindow();
        _ = subsystem.QueryMovingAverageFilterEnabled();
        _ = subsystem.QueryFrontTerminalsSelected();
        _ = subsystem.QueryOpenDetectorEnabled();
        _ = subsystem.QueryMultimeterMeasurementUnit();
        _ = subsystem.QueryOpenDetectorEnabled();
        _ = subsystem.QueryPowerLineCycles();
        _ = subsystem.QueryRange();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handles the Multimeter subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( MultimeterSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( MultimeterSubsystemBase.AutoDelayMode ):
                {
                    break;
                }
            // todo: Handle auto delay mode; might be just on or off.
            // If subsystem.AutoDelayMode.HasValue Then Me.AutoDelayMode = subsystem.AutoDelayMode.Value
            // 
            case nameof( MultimeterSubsystemBase.AutoDelayEnabled ):
                {
                    this._autoDelayToggleButton.CheckState = subsystem.AutoDelayEnabled.ToCheckState();
                    break;
                }

            case nameof( MultimeterSubsystemBase.AutoRangeEnabled ):
                {
                    this._autoRangeToggleButton.CheckState = subsystem.AutoRangeEnabled.ToCheckState();
                    break;
                }

            case nameof( MultimeterSubsystemBase.AutoZeroEnabled ):
                {
                    this._autoZeroToggleButton.CheckState = subsystem.AutoZeroEnabled.ToCheckState();
                    break;
                }

            case nameof( MultimeterSubsystemBase.FilterCount ):
                {
                    if ( subsystem.FilterCount.HasValue )
                        this._filterCountNumeric.Value = subsystem.FilterCount.Value;
                    break;
                }

            case nameof( MultimeterSubsystemBase.FilterCountRange ):
                {
                    _ = this._filterCountNumeric.NumericUpDown.RangeSetter( subsystem.FilterCountRange.Min, ( decimal ) subsystem.FilterCountRange.Max );
                    this._filterCountNumeric.NumericUpDown.DecimalPlaces = 0;
                    break;
                }

            case nameof( MultimeterSubsystemBase.FilterEnabled ):
                {
                    this._filterEnabledToggleButton.CheckState = subsystem.FilterEnabled.ToCheckState();
                    break;
                }

            case nameof( MultimeterSubsystemBase.FilterWindow ):
                {
                    if ( subsystem.FilterWindow.HasValue )
                        this._filterWindowNumeric.Value = ( decimal ) (100d * subsystem.FilterWindow.Value);
                    break;
                }

            case nameof( MultimeterSubsystemBase.FilterWindowRange ):
                {
                    Std.Primitives.RangeR range = subsystem.FilterWindowRange.TransposedRange( 0d, 100d );
                    _ = this._filterWindowNumeric.NumericUpDown.RangeSetter( range.Min, range.Max );
                    this._filterWindowNumeric.NumericUpDown.DecimalPlaces = 0;
                    break;
                }

            case nameof( MultimeterSubsystemBase.MovingAverageFilterEnabled ):
                {
                    this._windowTypeToggleButton.CheckState = subsystem.MovingAverageFilterEnabled.ToCheckState();
                    break;
                }

            case nameof( MultimeterSubsystemBase.FrontTerminalsSelected ):
                {
                    // todo: bind front terminal state to a status label with visible if has value and F, R: use binding formatting functions
                    // Me._readTerminalStateButton.Checked = subsystem.FrontTerminalsSelected.GetValueOrDefault(False)
                    this._terminalStateReadButton.CheckState = subsystem.FrontTerminalsSelected.ToCheckState();
                    break;
                }

            case nameof( MultimeterSubsystemBase.FunctionMode ):
                {
                    this.SelectedMultimeterFunctionMode = subsystem.FunctionMode.GetValueOrDefault( MultimeterFunctionModes.VoltageDC );
                    break;
                }

            case nameof( MultimeterSubsystemBase.FunctionRange ):
                {
                    _ = this._senseRangeNumeric.NumericUpDown.RangeSetter( subsystem.FunctionRange.Min, subsystem.FunctionRange.Max );
                    break;
                }

            case nameof( MultimeterSubsystemBase.FunctionRangeDecimalPlaces ):
                {
                    this._senseRangeNumeric.NumericUpDown.DecimalPlaces = subsystem.DefaultFunctionModeDecimalPlaces;
                    break;
                }

            case nameof( MultimeterSubsystemBase.FunctionUnit ):
                {
                    this._functionLabel.Text = subsystem.FunctionUnit.ToString();
                    break;
                }

            case nameof( MultimeterSubsystemBase.OpenDetectorEnabled ):
                {
                    this._openDetectorToggleButton.CheckState = subsystem.OpenDetectorEnabled.ToCheckState();
                    break;
                }

            case nameof( MultimeterSubsystemBase.PowerLineCycles ):
                {
                    if ( subsystem.PowerLineCycles.HasValue )
                        this._apertureNumeric.Value = ( decimal ) subsystem.PowerLineCycles.Value;
                    break;
                }

            case nameof( MultimeterSubsystemBase.PowerLineCyclesRange ):
                {
                    _ = this._apertureNumeric.NumericUpDown.RangeSetter( subsystem.PowerLineCyclesRange.Min, subsystem.PowerLineCyclesRange.Max );
                    this._apertureNumeric.NumericUpDown.DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces;
                    break;
                }

            case nameof( MultimeterSubsystemBase.PowerLineCyclesDecimalPlaces ):
                {
                    this._apertureNumeric.NumericUpDown.DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces;
                    break;
                }

            case nameof( MultimeterSubsystemBase.Range ):
                {
                    if ( subsystem.Range.HasValue )
                        _ = this._senseRangeNumeric.NumericUpDown.ValueSetter( subsystem.Range.Value );
                    break;
                }

            case nameof( MultimeterSubsystemBase.ReadingAmounts ):
                {
                    break;
                }

            case nameof( MultimeterSubsystemBase.ReadingElementTypes ):
                {
                    this.ReadingElementTypesSetter( subsystem.ReadingElementTypes );
                    break;
                }

            case nameof( MultimeterSubsystemBase.SupportedFunctionModes ):
                {
                    bool init = this.InitializingComponents;
                    this.InitializingComponents = true;
                    this._senseFunctionComboBox.ComboBox.ListEnumDescriptions( subsystem.SupportedFunctionModes, ~subsystem.SupportedFunctionModes );
                    this.InitializingComponents = init;
                    this.SelectedMultimeterFunctionMode = subsystem.FunctionMode.GetValueOrDefault( MultimeterFunctionModes.VoltageDC );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Multimeter subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void MultimeterSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            if ( this.InvokeRequired )
            {
                activity = $"invoking {nameof( this.MultimeterSubsystem )}.{e.PropertyName} change";
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.MultimeterSubsystemPropertyChanged ), [sender, e] );
            }
            else if ( this._subsystemToolStrip.InvokeRequired )
            {
                activity = $"invoking {nameof( this.MultimeterSubsystem )}.{e.PropertyName} change";
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.MultimeterSubsystemPropertyChanged ), [sender, e] );
            }
            else if ( sender is MultimeterSubsystemBase s )
            {
                activity = $"handling {nameof( this.MultimeterSubsystem )}.{e.PropertyName} change";
                this.HandlePropertyChanged( s, e.PropertyName );
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Applies the selected measurements settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplySettings( MultimeterSubsystemBase subsystem )
    {
        if ( this.MultimeterSubsystem is null ) return;
        subsystem.StartElapsedStopwatch();
        if ( !Equals( subsystem.PowerLineCycles, this._apertureNumeric.Value ) )
        {
            _ = subsystem.ApplyPowerLineCycles( ( double ) this._apertureNumeric.Value );
        }

        // If Not Nullable.Equals(.AutoDelayMode, Me._autoDelayMode) Then
        // .ApplyAutoDelayMode(Me._autoDelayMode)
        // End If

        if ( !Nullable.Equals( subsystem.AutoRangeEnabled, this._autoRangeToggleButton.Checked ) )
        {
            _ = subsystem.ApplyAutoRangeEnabled( this._autoRangeToggleButton.Checked );
        }

        if ( !Nullable.Equals( subsystem.AutoZeroEnabled, this._autoZeroToggleButton.Checked ) )
        {
            _ = subsystem.ApplyAutoZeroEnabled( this._autoZeroToggleButton.Checked );
        }

        if ( !Nullable.Equals( subsystem.FilterEnabled, this._filterEnabledToggleButton.Checked ) )
        {
            _ = subsystem.ApplyFilterEnabled( this._filterEnabledToggleButton.Checked );
        }

        if ( !Equals( subsystem.FilterCount, this._filterCountNumeric.Value ) )
        {
            _ = subsystem.ApplyFilterCount( ( int ) this._filterCountNumeric.Value );
        }

        if ( !Nullable.Equals( subsystem.MovingAverageFilterEnabled, this._windowTypeToggleButton.Checked ) )
        {
            _ = subsystem.ApplyMovingAverageFilterEnabled( this._windowTypeToggleButton.Checked );
        }

        if ( !Nullable.Equals( subsystem.OpenDetectorEnabled, this._openDetectorToggleButton.Checked ) )
        {
            _ = subsystem.ApplyOpenDetectorEnabled( this._openDetectorToggleButton.Checked );
        }

        _ = this.MultimeterSubsystem.QueryMultimeterMeasurementUnit();
        _ = this.MultimeterSubsystem.QueryOpenDetectorEnabled();
        if ( subsystem.AutoRangeEnabled == true )
        {
            _ = subsystem.QueryRange();
        }
        else if ( !Equals( subsystem.Range, this._senseRangeNumeric.Value ) )
        {
            _ = subsystem.ApplyRange( ( int ) this._senseRangeNumeric.Value );
        }

        if ( !Nullable.Equals( subsystem.FilterWindow, 0.01d * ( double ) this._filterWindowNumeric.Value ) )
        {
            _ = subsystem.ApplyFilterWindow( 0.01d * ( double ) this._filterWindowNumeric.Value );
        }

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

    #region " sense subsystem "

    /// <summary> Gets or sets the Sense subsystem. </summary>
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
            this.BindSubsystem( false, this.SenseSubsystem );
            this.SenseSubsystem = null;
        }

        this.SenseSubsystem = subsystem;
        if ( this.SenseSubsystem is not null )
        {
            this.BindSubsystem( true, this.SenseSubsystem );
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, SenseSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.SenseSubsystemPropertyChanged;
            this.HandlePropertyChanged( subsystem, nameof( SenseSubsystemBase.SupportedFunctionModes ) );
            this.HandlePropertyChanged( subsystem, nameof( SenseSubsystemBase.FunctionRange ) );
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.SenseSubsystemPropertyChanged;
            this.BindSubsystem( default, "DMM" );
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( SenseSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( SenseSubsystemBase.FunctionMode ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseSubsystemBase.FunctionRange ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseSubsystemBase.FunctionRangeDecimalPlaces ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseSubsystemBase.FunctionUnit ) );
    }

    /// <summary> Reads the selected measurements settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( SenseSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryFunctionMode();
        // subsystem.QueryAutoRangeEnabled()
        // subsystem.QueryPowerLineCycles()
        // subsystem.QueryRange()
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handles the function modes changed action. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected virtual void HandleFunctionModesChanged( SenseSubsystemBase subsystem )
    {
        if ( subsystem is not null && subsystem.FunctionMode.HasValue )
        {
            SenseFunctionModes value = subsystem.FunctionMode.Value;
            if ( value != SenseFunctionModes.None )
            {
                // was not used: bool valueChanged = this.SelectedSenseFunctionMode != subsystem.FunctionMode.Value;
                // this is done at the device level by applying the function mode to the measure subsystem
                // Me.MeasureSubsystem.Readings.Reading.ApplyUnit(subsystem.ToUnit(value))
                _ = this._senseFunctionComboBox.SelectValue( subsystem.FunctionMode.Value );
                this.NotifyPropertyChanged( nameof( this.SelectedSenseFunctionMode ) );
            }
        }
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
            case nameof( SenseSubsystemBase.SupportedFunctionModes ):
                {
                    bool init = this.InitializingComponents;
                    this.InitializingComponents = true;
                    this._senseFunctionComboBox.ComboBox.ListEnumDescriptions( subsystem.SupportedFunctionModes, ~subsystem.SupportedFunctionModes );
                    this.InitializingComponents = init;
                    break;
                }

            case nameof( SenseSubsystemBase.FunctionMode ):
                {
                    this.HandleFunctionModesChanged( subsystem );
                    break;
                }

            case nameof( SenseSubsystemBase.FunctionRange ):
                {
                    _ = this._senseRangeNumeric.NumericUpDown.RangeSetter( subsystem.FunctionRange.Min, subsystem.FunctionRange.Max );
                    break;
                }

            case nameof( SenseSubsystemBase.FunctionRangeDecimalPlaces ):
                {
                    this._senseRangeNumeric.NumericUpDown.DecimalPlaces = subsystem.FunctionRangeDecimalPlaces;
                    break;
                }

            case nameof( SenseSubsystemBase.FunctionUnit ):
                {
                    this._functionLabel.Text = subsystem.FunctionUnit.ToString( System.Globalization.CultureInfo.CurrentCulture );
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

    /// <summary> Gets or sets the sense function subsystem. </summary>
    /// <value> The sense function subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public SenseFunctionSubsystemBase? SenseFunctionSubsystem { get; private set; }

    /// <summary> Bind Sense function subsystem. </summary>
    /// <param name="subsystem">     The subsystem. </param>
    /// <param name="subsystemName"> The name of the subsystem. </param>
    public void BindSubsystem( SenseFunctionSubsystemBase? subsystem, string subsystemName )
    {
        if ( this.SenseFunctionSubsystem is not null )
        {
            this.BindSubsystem( false, this.SenseFunctionSubsystem );
            this.SenseFunctionSubsystem = null;
        }

        this.SenseFunctionSubsystem = subsystem;
        if ( this.SenseFunctionSubsystem is not null )
        {
            this.BindSubsystem( true, this.SenseFunctionSubsystem );
            this._autoZeroToggleButton.Visible = this.SenseFunctionSubsystem.SupportsAutoZero;
            this._autoDelayToggleButton.Visible = this.SenseFunctionSubsystem.SupportsAutoDelay;
            this._openDetectorToggleButton.Visible = this.SenseFunctionSubsystem.SupportsOpenLeadDetector;
        }

        this.SubsystemName = string.IsNullOrEmpty( subsystemName ) ? "DMM" : subsystemName;
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, SenseFunctionSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.SenseFunctionSubsystemPropertyChanged;
            this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.AverageCountRange ) );
            this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.AveragePercentWindowRange ) );
            this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.FunctionRange ) );
            this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.FunctionRangeDecimalPlaces ) );
            this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.PowerLineCyclesRange ) );
            this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.PowerLineCyclesDecimalPlaces ) );
            this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.ResolutionDigitsRange ) );
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.SenseFunctionSubsystemPropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( SenseFunctionSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.FunctionMode ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.AutoRangeEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.AutoZeroEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.AverageCount ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.AverageEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.AverageFilterType ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.AveragePercentWindow ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.FunctionUnit ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.PowerLineCycles ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.Range ) );
        this.HandlePropertyChanged( subsystem, nameof( SenseFunctionSubsystemBase.ResolutionDigits ) );
    }

    /// <summary> Reads the selected measurements settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( SenseFunctionSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryFunctionMode();
        _ = subsystem.QueryAutoRangeEnabled();
        _ = subsystem.QueryAutoZeroEnabled();
        _ = subsystem.QueryAverageCount();
        _ = subsystem.QueryAverageEnabled();
        _ = subsystem.QueryAverageFilterType();
        _ = subsystem.QueryAveragePercentWindow();
        _ = subsystem.QueryPowerLineCycles();
        _ = subsystem.QueryRange();
        _ = subsystem.QueryResolutionDigits();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handle the Sense subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( SenseFunctionSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        // Me._senseRangeTextBox.SafeTextSetter(Me.Device.SenseRange(VI.ResourceAccessLevels.Cache).ToString(Globalization.CultureInfo.CurrentCulture))
        // Me._integrationPeriodTextBox.SafeTextSetter(Me.Device.SenseIntegrationPeriodCaption)
        switch ( propertyName ?? string.Empty )
        {
            case nameof( SenseFunctionSubsystemBase.AutoRangeEnabled ):
                {
                    this._autoRangeToggleButton.CheckState = subsystem.AutoRangeEnabled.ToCheckState();
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.AutoZeroEnabled ):
                {
                    this._autoZeroToggleButton.CheckState = subsystem.AutoZeroEnabled.ToCheckState();
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.AverageCount ):
                {
                    if ( subsystem.AverageCount.HasValue )
                        this._filterCountNumeric.Value = subsystem.AverageCount.Value;
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.AverageCountRange ):
                {
                    _ = this._filterCountNumeric.NumericUpDown.RangeSetter( subsystem.AverageCountRange.Min, ( decimal ) subsystem.AverageCountRange.Max );
                    this._filterCountNumeric.NumericUpDown.DecimalPlaces = 0;
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.AverageEnabled ):
                {
                    this._filterEnabledToggleButton.CheckState = subsystem.AverageEnabled.ToCheckState();
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.AveragePercentWindow ):
                {
                    if ( subsystem.AveragePercentWindow.HasValue )
                        this._filterWindowNumeric.Value = ( decimal ) subsystem.AveragePercentWindow.Value;
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.AveragePercentWindowRange ):
                {
                    Std.Primitives.RangeR range = subsystem.AveragePercentWindowRange.TransposedRange( 0d, 100d );
                    _ = this._filterWindowNumeric.NumericUpDown.RangeSetter( range.Min, range.Max );
                    this._filterWindowNumeric.NumericUpDown.DecimalPlaces = 0;
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.AverageFilterType ):
                {
                    this._windowTypeToggleButton.CheckState = subsystem.AverageFilterType.HasValue ? subsystem.AverageFilterType.Value == AverageFilterTypes.Moving ? CheckState.Checked : CheckState.Unchecked : CheckState.Indeterminate;
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.FunctionRange ):
                {
                    _ = this._senseRangeNumeric.NumericUpDown.RangeSetter( subsystem.FunctionRange.Min, subsystem.FunctionRange.Max );
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.FunctionRangeDecimalPlaces ):
                {
                    double range = subsystem.Range.GetValueOrDefault( 0.105d );
                    this._senseRangeNumeric.NumericUpDown.DecimalPlaces = ( int ) Math.Max( subsystem.FunctionRangeDecimalPlaces, Math.Min( 0d, subsystem.FunctionRangeDecimalPlaces - Math.Log( range ) ) );
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.FunctionUnit ):
                {
                    this._functionLabel.Text = subsystem.FunctionUnit.ToString();
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.PowerLineCycles ):
                {
                    if ( subsystem.PowerLineCycles.HasValue )
                        this._apertureNumeric.Value = ( decimal ) subsystem.PowerLineCycles.Value;
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.PowerLineCyclesRange ):
                {
                    _ = this._apertureNumeric.NumericUpDown.RangeSetter( subsystem.PowerLineCyclesRange.Min, subsystem.PowerLineCyclesRange.Max );
                    this._apertureNumeric.NumericUpDown.DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces;
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.PowerLineCyclesDecimalPlaces ):
                {
                    this._apertureNumeric.NumericUpDown.DecimalPlaces = subsystem.PowerLineCyclesDecimalPlaces;
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.Range ):
                {
                    if ( subsystem.Range.HasValue )
                        _ = this._senseRangeNumeric.NumericUpDown.ValueSetter( subsystem.Range.Value );
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.ResolutionDigits ):
                {
                    if ( subsystem.ResolutionDigits.HasValue )
                        this._resolutionDigitsNumeric.NumericUpDown.Value = ( decimal ) subsystem.ResolutionDigits.Value;
                    break;
                }

            case nameof( SenseFunctionSubsystemBase.ResolutionDigitsRange ):
                {
                    _ = this._resolutionDigitsNumeric.NumericUpDown.RangeSetter( subsystem.ResolutionDigitsRange.Min, ( decimal ) subsystem.ResolutionDigitsRange.Max );
                    this._resolutionDigitsNumeric.NumericUpDown.DecimalPlaces = 0;
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Sense function subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void SenseFunctionSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( SenseFunctionSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.SenseFunctionSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.SenseFunctionSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is SenseFunctionSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Applies the selected measurements settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplySettings( SenseFunctionSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        if ( !Equals( subsystem.PowerLineCycles, this._apertureNumeric.Value ) )
        {
            _ = subsystem.ApplyPowerLineCycles( ( double ) this._apertureNumeric.Value );
        }

        // If Not Nullable.Equals(.subsystemAutoDelayMode, Me._autoDelayMode) Then
        // subsystem.ApplyAutoDelayMode(Me._autoDelayMode)
        // End If

        if ( !Nullable.Equals( subsystem.AutoRangeEnabled, this._autoRangeToggleButton.Checked ) )
        {
            _ = subsystem.ApplyAutoRangeEnabled( this._autoRangeToggleButton.Checked );
        }

        if ( !Nullable.Equals( subsystem.AutoZeroEnabled, this._autoZeroToggleButton.Checked ) )
        {
            _ = subsystem.ApplyAutoZeroEnabled( this._autoZeroToggleButton.Checked );
        }

        if ( !Nullable.Equals( subsystem.AverageEnabled, this._filterEnabledToggleButton.Checked ) )
        {
            _ = subsystem.ApplyAverageEnabled( this._filterEnabledToggleButton.Checked );
        }

        if ( !Equals( subsystem.AverageCount, this._filterCountNumeric.Value ) )
        {
            _ = subsystem.ApplyAverageCount( ( int ) this._filterCountNumeric.Value );
        }

        AverageFilterTypes filterType = this._windowTypeToggleButton.CheckState == CheckState.Checked ? AverageFilterTypes.Moving : this._windowTypeToggleButton.CheckState == CheckState.Unchecked ? AverageFilterTypes.Repeat : AverageFilterTypes.None;
        if ( ( int? ) subsystem.AverageFilterType != ( int? ) filterType && filterType != AverageFilterTypes.None )
        {
            _ = subsystem.ApplyAverageFilterType( filterType );
        }

        if ( subsystem.AutoRangeEnabled == true )
        {
            _ = subsystem.QueryRange();
        }
        else if ( !Equals( subsystem.Range, this._senseRangeNumeric.Value ) )
        {
            _ = subsystem.ApplyRange( ( int ) this._senseRangeNumeric.Value );
        }

        if ( !Equals( subsystem.AveragePercentWindow, this._filterWindowNumeric.Value ) )
        {
            _ = subsystem.ApplyAveragePercentWindow( ( double ) this._filterWindowNumeric.Value );
        }

        if ( !Equals( subsystem.ResolutionDigits, this._resolutionDigitsNumeric.Value ) )
        {
            _ = subsystem.ApplyResolutionDigits( ( double ) this._resolutionDigitsNumeric.Value );
        }

        subsystem.StopElapsedStopwatch();
    }

    #endregion

    #region " system subsystem "

    /// <summary> Gets or sets the sense function subsystem. </summary>
    /// <value> The sense function subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public SystemSubsystemBase? SystemSubsystem { get; private set; }

    /// <summary> Bind system subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( SystemSubsystemBase subsystem )
    {
        if ( this.SystemSubsystem is not null )
        {
            this.BindSubsystem( false, this.SystemSubsystem );
            this.SystemSubsystem = null;
        }

        this.SystemSubsystem = subsystem;
        if ( this.SystemSubsystem is not null )
        {
            this.BindSubsystem( true, this.SystemSubsystem );
        }

        this._autoDelayToggleButton.Visible = false;
        this._openDetectorToggleButton.Visible = false;
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, SystemSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.SystemSubsystemPropertyChanged;
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.SystemSubsystemPropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( SystemSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( SystemSubsystemBase.FrontTerminalsSelected ) );
    }

    /// <summary> Reads the <see cref="SystemSubsystemBase"/> settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( SystemSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryFrontTerminalsSelected();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handles the Multimeter subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( SystemSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( SystemSubsystemBase.FrontTerminalsSelected ):
                {
                    this._terminalStateReadButton.CheckState = subsystem.FrontTerminalsSelected.ToCheckState();
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> System subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property changed event information. </param>
    private void SystemSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( SystemSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.SystemSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.SystemSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is SystemSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
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
        this.HandlePropertyChanged( subsystem, nameof( TraceSubsystemBase.PointsCount ) );
    }

    /// <summary> Reads the <see cref="TraceSubsystemBase"/> settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( TraceSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        // subsystem.QueryReadingElementTypes()
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handle the Trace subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "<Pending>" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    private void HandlePropertyChanged( TraceSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( TraceSubsystemBase.PointsCount ):
                {
                    break;
                }

            default:
                break;
        }
    }

    /// <summary>   Trace subsystem property changed. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Property changed event information. </param>
    private void TraceSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( this.TraceSubsystem )}.{e.PropertyName} change";
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

    #region " trigger subsystem "

    /// <summary> Gets or sets the trigger subsystem. </summary>
    /// <value> The trigger subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public TriggerSubsystemBase? TriggerSubsystem { get; private set; }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( TriggerSubsystemBase subsystem )
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

    #region " measurement: event driven "

    /// <summary> Gets or sets the trace readings. </summary>
    /// <value> The trace readings. </value>
    private List<ReadingAmounts>? TraceReadings { get; set; }

    /// <summary> Fetches buffered readings. </summary>
    /// <remarks> David, 2020-07-27. </remarks>
    /// <param name="values"> The values. </param>
    protected virtual void DisplayBufferedReadings( IList<ReadingAmounts> values )
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} adding buffered readings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TraceReadings ??= [];
            this.TraceReadings.AddRange( values );
            if ( this.DataGridView is null )
            {
                activity = $"{this.Device?.ResourceNameCaption} data grid view not used in this instance";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            }
            else
            {
                activity = $"{this.Device?.ResourceNameCaption} updating the display";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                if ( this.TraceReadings.Count == values.Count )
                {
                    _ = this.DataGridView.Display( values, false );
                }
                else
                {
                    this.DataGridView.Invalidate();
                }
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Fetches and displays buffered readings. </summary>
    protected virtual void FetchAndDisplayBufferedReadings()
    {
    }

    /// <summary> Handles the measurement completed request. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void HandleMeasurementCompletedRequest( MeasureSubsystemBase subsystem )
    {
        if ( this.Device is null || this.Device.Session is null || this.Device.StatusSubsystemBase is null
            || this.TriggerSubsystem is null || this.TriggerSubsystem.TriggerCount is null || this.TraceSubsystem is null ) return;
        string activity = $"{this.Device.ResourceNameCaption} handling measurement event";
        try
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.Device.ResourceModelCaption} SRQ: {this.Device.Session.ServiceRequestStatus:X};. " );
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            if ( this.Device.StatusSubsystemBase.Session.HasMeasurementEvent )
            {
                if ( this.TriggerSubsystem.TriggerCount.Value == 1 )
                {
                    if ( ( int? ) this.TraceSubsystem.FeedSource == ( int? ) FeedSources.None )
                    {
                        activity = $"{this.Device.ResourceNameCaption} fetching a single readings";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                        _ = subsystem.Fetch();
                    }
                    else
                        this.FetchAndDisplayBufferedReadings();
                }
                else
                    this.FetchAndDisplayBufferedReadings();

                this.TraceSubsystem.StopElapsedStopwatch();
                if ( this._autoInitiateMenuItem.Checked )
                {
                    activity = $"{this.Device?.ResourceNameCaption} initiating next measurement(s)";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.TraceSubsystem.StartElapsedStopwatch();
                    this.TraceSubsystem.ClearBuffer(); // ?@#  17-7-6
                    this.TriggerSubsystem.Initiate();
                }
            }
            else
            {
                activity = $"{this.Device?.ResourceNameCaption} measurement not available--is error?";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
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

    /// <summary> Handles the measurement completed request. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void HandleMeasurementCompletedRequest( MultimeterSubsystemBase subsystem )
    {
        if ( this.InitializingComponents || this.Device is null || this.TriggerSubsystem is null || this.BufferSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"{this.Device.ResourceNameCaption} handling SRQ: {this.Device.Session?.ServiceRequestStatus:X}";
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
                if ( this.TriggerSubsystem.TriggerCount == 1 )
                {
                    activity = $"{this.Device.ResourceNameCaption} fetching a single reading";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    _ = subsystem.MeasurePrimaryReading();
                }
                else
                {
                    activity = $"{this.Device.ResourceNameCaption} fetching buffered readings";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.BufferReadings.Add( this.BufferSubsystem.QueryBufferReadings() );
                    activity = $"{this.Device.ResourceNameCaption} updating the display";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.DataGridView?.Invalidate();
                    this.BufferSubsystem.StopElapsedStopwatch();
                }

                if ( this._autoInitiateMenuItem.Checked )
                {
                    activity = $"{this.Device.ResourceNameCaption} initiating next measurement(s)";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.BufferSubsystem.StartElapsedStopwatch();
                    this.BufferSubsystem.ClearBuffer(); // ?@3 removed 17-7-6
                    this.TriggerSubsystem.Initiate();
                }
            }
            else
            {
                activity = $"{this.Device?.ResourceNameCaption} trigger plan started; buffer empty";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
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

    /// <summary> Handles the measurement completed request. </summary>
    /// <param name="sender"> <see cref="object"/>
    /// instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void HandleMeasurementCompletedRequest( object? sender, EventArgs e )
    {
        if ( sender is null || e is null )
            return;
        if ( this.MeasureSubsystem is not null )
        {
            this.HandleMeasurementCompletedRequest( this.MeasureSubsystem );
        }
        else if ( this.MultimeterSubsystem is not null )
        {
            this.HandleMeasurementCompletedRequest( this.MultimeterSubsystem );
        }
    }

    /// <summary> Gets or sets the measurement complete handler added. </summary>
    /// <value> The measurement complete handler added. </value>
    private bool MeasurementCompleteHandlerAdded { get; set; }

    /// <summary> Adds measurement complete event handler. </summary>
    private void AddMeasurementCompleteEventHandler()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        if ( !this.MeasurementCompleteHandlerAdded )
        {
            // clear execution state before enabling events
            activity = $"{this.Device.ResourceNameCaption} Clearing execution state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            activity = $"{this.Device.ResourceNameCaption} Enabling session service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session?.EnableServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Adding device service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.AddServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Turning on measurement events";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            _ = this.Device.StatusSubsystemBase?.ApplyMeasurementEventEnableBitmask( this.Device.StatusSubsystemBase.MeasurementEventsBitmasks.All );
            // 
            // if handling buffer full, use the 4917 event to detect buffer full. 

            activity = $"{this.Device.ResourceNameCaption} Turning on status service request";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session?.ApplyStatusByteEnableBitmask( ( int ) this.Device.Session.OperationServiceRequestEnableBitmask );
            activity = $"{this.Device.ResourceNameCaption} Adding re-triggering event handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ServiceRequested += this.HandleMeasurementCompletedRequest;
            this.MeasurementCompleteHandlerAdded = true;
        }
    }

    /// <summary> Removes the measurement complete event handler. </summary>
    private void RemoveMeasurementCompleteEventHandler()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        if ( this.MeasurementCompleteHandlerAdded )
        {
            activity = $"{this.Device.ResourceNameCaption} Disabling session service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session?.DisableServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Removing device service request handler";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.RemoveServiceRequestEventHandler();
            activity = $"{this.Device.ResourceNameCaption} Turning off measurement events";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            // Me.Device.StatusSubsystemBase.ApplyMeasurementEventEnableBitmask(0)

            activity = $"{this.Device.ResourceNameCaption} Turning off status service request";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.Session?.ApplyStatusByteEnableBitmask( 0 );
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
        if ( this.InitializingComponents || sender is null || e is null || this.Device is null ) return;
        if ( sender is not ToolStripMenuItem menuItem ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} Aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.AbortTriggerPlan();
            if ( menuItem.Checked )
            {
                activity = $"{this.Device.ResourceNameCaption} Adding measurement completion handler";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.AddMeasurementCompleteEventHandler();
                this._measureValueButton.Text = "Initiated";
            }
            else
            {
                activity = $"{this.Device.ResourceNameCaption} Removing measurement completion handler";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.RemoveMeasurementCompleteEventHandler();
                this._measureValueButton.Text = "Measure";
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

    /// <summary> Aborts trigger plan. </summary>
    private void AbortTriggerPlan()
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

    /// <summary> Starts trigger plan. </summary>
    private void StartTriggerPlan()
    {
        if ( this.InitializingComponents || this.Device is null || this.TraceSubsystem is null || this.MeasureSubsystem is null || this.TriggerSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing execution state";
            this.Device.ClearExecutionState();
            activity = $"{this.Device.ResourceNameCaption} clearing buffer and display";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.TraceSubsystem.ClearBuffer();
            activity = $"{this.Device.ResourceNameCaption} initiating single trigger measurements(s)";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.MeasureSubsystem.StartElapsedStopwatch();
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

    /// <summary> Initiates this object. </summary>
    private void Initiate()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} Aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.AbortTriggerPlan();
            activity = $"{this.Device.ResourceNameCaption} Starting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.StartTriggerPlan();
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

    /// <summary> Aborts this object. </summary>
    private void Abort()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} Aborting trigger plan";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.AbortTriggerPlan();
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

    #region " control event handlers "

    /// <summary> Applies the function mode button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplyFunctionModeButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ApplyFunctionMode();
    }

    /// <summary> Automatic delay toggle button Check State Changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AutoDelayToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"Delay: {button.CheckState.ToCheckStateCaption( "auto", "~auto", "?" )}";
        }
    }

    /// <summary> Automatic range toggle button Check State Changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AutoRangeToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"{button.CheckState.ToCheckStateCaption( string.Empty, "~", "?" )}auto";
        }
    }

    /// <summary> Automatic zero toggle button Check State Changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AutoZeroToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"{button.CheckState.ToCheckStateCaption( string.Empty, "~", "?" )}auto zero";
        }
    }

    /// <summary> Filter enabled toggle button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void FilterEnabledToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"{button.CheckState.ToCheckStateCaption( "on", "off", "?on" )}";
        }
    }

    /// <summary> Opens detector toggle button Check State Changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void OpenDetectorToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"Open Detector: {button.CheckState.ToCheckStateCaption( "On", "Off", "?on" )}";
        }
    }

    /// <summary>
    /// Event handler. Called by _senseFunctionComboBox for selected index changed events.
    /// </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void SenseFunctionComboBox_SelectedIndexChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ApplyFunctionMode();
    }

    /// <summary> Displays terminal state. </summary>
    /// <param name="sender"> <see cref="object"/>
    /// instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void TerminalStateReadButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"Terminals: {button.CheckState.ToCheckStateCaption( "Front", "Rear", "?" )}";
        }
    }

    /// <summary> Terminal state read button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void TerminalStateReadButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ReadTerminalState();
    }

    /// <summary> Window type toggle button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void WindowTypeToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"{button.CheckState.ToCheckStateCaption( "Moving", "Repeating", "?filter" )}";
        }
    }

    /// <summary> Reads function mode menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadFunctionModeMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents ) return;
        this.ReadFunctionMode();
    }

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

    /// <summary> Starts stop measure value. </summary>
    private void StartStopMeasureValue()
    {
        if ( this.TriggerSubsystem is null ) return;
        if ( this._fetchOnMeasurementEventMenuItem.Checked )
        {
            if ( ( int? ) this.TriggerSubsystem.TriggerState == ( int? ) TriggerState.Idle )
            {
                this.Initiate();
                this._measureValueButton.Text = "Abort";
            }
            else
            {
                this.Abort();
                this._measureValueButton.Text = "Initiate";
            }
        }
        else
        {
            this.MeasureValue();
        }
    }

    /// <summary> Measure immediate menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void MeasureImmediateMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.MeasureValue();
    }

    /// <summary> Measure value button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void MeasureValueButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.StartStopMeasureValue();
    }

    /// <summary> Reading combo box selected index changed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void ReadingElementTypesComboBox_SelectedIndexChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.SelectActiveReading();
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

    #endregion
}
