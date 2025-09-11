using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using cc.isr.Enums;
using cc.isr.Std.NumericExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> A meter view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class MeterView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

#if NET9_0_OR_GREATER
    private readonly System.Threading.Lock _locker = new();
#else
    private readonly object _locker = new();
#endif

    private cc.isr.Tracing.WinForms.TextBoxTraceEventWriter TextBoxTextWriter { get; set; }

    private cc.isr.Tracing.WinForms.TraceAlertContainer TraceAlertContainer { get; set; }

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public MeterView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();

        this.TextBoxTextWriter = new( this._traceMessagesBox )
        {
            ContainerTreeNode = null,
            ContainerPanel = this._messagesTabPage,
            HandleCreatedCheckEnabled = false,
            TabCaption = "Log",
            CaptionFormat = "{0} " + Convert.ToChar( 0x1C2 ),
            ResetCount = 1000,
            PresetCount = 500,
            TraceLevel = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TraceLogSettings.TraceShowLevel
        };
        cc.isr.Tracing.TracingPlatform.Instance.AddTraceEventWriter( this.TextBoxTextWriter );

        this.TraceAlertContainer = new( this._measurementsHeader.AlertAnnunciator, this._traceMessagesBox )
        {
            AlertAnnunciatorText = "Alert",
            AlertLevel = TraceEventType.Error,
            AlertSoundEnabled = true
        };
        cc.isr.Tracing.TracingPlatform.Instance.AddTraceEventWriter( this.TraceAlertContainer );

        this.ApplyShuntResistanceButtonCaption = this._applyShuntResistanceConfigurationButton.Text;
        this.ApplyNewShuntResistanceButtonCaption = this._applyNewShuntResistanceConfigurationButton.Text;

        // hide the alerts
        this._measurementsHeader.ShowAlerts( false, false );
        this._measurementsHeader.ShowOutcome( false, false );
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new <see cref="MeterView"/> </summary>
    /// <remarks> David, 2020-10-12. </remarks>
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
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources;
    ///                                                   <c>false</c> to release only unmanaged
    ///                                                   resources when called from the runtime
    ///                                                   finalize. </param>
    protected override void Dispose( bool disposing )
    {
        if ( this.IsDisposed )
            return;
        try
        {
            if ( disposing )
            {
                this.InitializingComponents = true;
                this.TextBoxTextWriter.SuspendUpdatesReleaseIndicators();
                this.BindPart( null );
                this.AssignMeter( null );
                if ( this.Meter is not null )
                {
                    this.Meter.Dispose();
                    this.Meter = null;
                }

                this.components?.Dispose();
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " event handlers:  form "

    /// <summary>
    /// Called upon receiving the <see cref="System.Windows.Forms.Form.Load" /> event.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="e"> An <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnLoad( EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            activity = $"Loading the driver console form";
            Trace.CorrelationManager.StartLogicalOperation( System.Reflection.MethodBase.GetCurrentMethod()?.Name ?? string.Empty );
            if ( !this.DesignMode )
            {
                // add listeners before the first talker publish command
                this.AssignMeter( new Meter() );
            }

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );

            // build the navigator tree.
            this.BuildNavigatorTreeView();
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            base.OnLoad( e );
            this.Cursor = Cursors.Default;
            Trace.CorrelationManager.StopLogicalOperation();
            Pith.SessionBase.DoEventsAction?.Invoke();
            this.SelectNavigatorTreeViewNode( TreeViewNode.ConnectNode );
            _ = this._resourceSelectorConnector.Focus();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " device under test "

    /// <summary> Gets the device under test. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The device under test. </value>
    private DeviceUnderTest? DeviceUnderTest { get; set; }

    #endregion

    #region " shunt "

    #region " configure shunt"

    /// <summary> Bind controls. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void BindShuntControls()
    {
        if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )} is null." );
        if ( this.Part.ShuntResistance is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )}.{nameof( MeterView.Part.ShuntResistance )} is null." );

        TtmShuntSettings ttmShuntSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmShuntSettings;

        // set the GUI based on the current defaults.
        this._shuntResistanceCurrentRangeNumeric.Minimum = ( decimal ) ttmShuntSettings.CurrentMinimum;
        this._shuntResistanceCurrentRangeNumeric.Maximum = ( decimal ) ttmShuntSettings.CurrentMaximum;
        this._shuntResistanceCurrentRangeNumeric.DataBindings.Clear();
        this._shuntResistanceCurrentRangeNumeric.DataBindings.Add( new Binding( "Value", this.Part.ShuntResistance, "CurrentRange", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._shuntResistanceCurrentRangeNumeric.Value = (( decimal ) ttmShuntSettings.CurrentRange).Clip( this._shuntResistanceCurrentRangeNumeric.Minimum, this._shuntResistanceCurrentRangeNumeric.Maximum );
        this.Part.ShuntResistance.CurrentRange = ( double ) this._shuntResistanceCurrentRangeNumeric.Value;
        this._shuntResistanceCurrentLevelNumeric.Minimum = ( decimal ) ttmShuntSettings.CurrentMinimum;
        this._shuntResistanceCurrentLevelNumeric.Maximum = ( decimal ) ttmShuntSettings.CurrentMaximum;
        this._shuntResistanceCurrentLevelNumeric.DataBindings.Clear();
        this._shuntResistanceCurrentLevelNumeric.DataBindings.Add( new Binding( "Value", this.Part.ShuntResistance, "CurrentLevel", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._shuntResistanceCurrentLevelNumeric.Value = (( decimal ) ttmShuntSettings.CurrentLevel).Clip( this._shuntResistanceCurrentLevelNumeric.Minimum, this._shuntResistanceCurrentLevelNumeric.Maximum );
        this.Part.ShuntResistance.CurrentLevel = ( double ) this._shuntResistanceCurrentLevelNumeric.Value;
        this._shuntResistanceHighLimitNumeric.Minimum = ( decimal ) ttmShuntSettings.Minimum;
        this._shuntResistanceHighLimitNumeric.Maximum = ( decimal ) ttmShuntSettings.Maximum;
        this._shuntResistanceHighLimitNumeric.DataBindings.Clear();
        this._shuntResistanceHighLimitNumeric.DataBindings.Add( new Binding( "Value", this.Part.ShuntResistance, "HighLimit", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._shuntResistanceHighLimitNumeric.Value = (( decimal ) ttmShuntSettings.HighLimit).Clip( this._shuntResistanceHighLimitNumeric.Minimum, this._shuntResistanceHighLimitNumeric.Maximum );
        this.Part.ShuntResistance.HighLimit = ( double ) this._shuntResistanceHighLimitNumeric.Value;
        this._shuntResistanceLowLimitNumeric.Minimum = ( decimal ) ttmShuntSettings.Minimum;
        this._shuntResistanceLowLimitNumeric.Maximum = ( decimal ) ttmShuntSettings.Maximum;
        this._shuntResistanceLowLimitNumeric.DataBindings.Clear();
        this._shuntResistanceLowLimitNumeric.DataBindings.Add( new Binding( "Value", this.Part.ShuntResistance, "LowLimit", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._shuntResistanceLowLimitNumeric.Value = (( decimal ) ttmShuntSettings.LowLimit).Clip( this._shuntResistanceLowLimitNumeric.Minimum, this._shuntResistanceLowLimitNumeric.Maximum );
        this.Part.ShuntResistance.LowLimit = ( double ) this._shuntResistanceLowLimitNumeric.Value;
        this._shuntResistanceVoltageLimitNumeric.Minimum = ( decimal ) ttmShuntSettings.VoltageMinimum;
        this._shuntResistanceVoltageLimitNumeric.Maximum = ( decimal ) ttmShuntSettings.VoltageMaximum;
        this._shuntResistanceVoltageLimitNumeric.DataBindings.Clear();
        this._shuntResistanceVoltageLimitNumeric.DataBindings.Add( new Binding( "Value", this.Part.ShuntResistance, "VoltageLimit", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._shuntResistanceVoltageLimitNumeric.Value = (( decimal ) ttmShuntSettings.VoltageLimit).Clip( this._shuntResistanceVoltageLimitNumeric.Minimum, this._shuntResistanceVoltageLimitNumeric.Maximum );
        this.Part.ShuntResistance.VoltageLimit = ( double ) this._shuntResistanceVoltageLimitNumeric.Value;
    }

    /// <summary> Restore defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private static void RestoreShuntDefaults()
    {
        TtmShuntSettings ttmShuntSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmShuntSettings;

        ttmShuntSettings.CurrentRange = ttmShuntSettings.CurrentRangeDefault;
        ttmShuntSettings.CurrentLevel = ttmShuntSettings.CurrentLevelDefault;
        ttmShuntSettings.HighLimit = ttmShuntSettings.HighLimitDefault;
        ttmShuntSettings.LowLimit = ttmShuntSettings.LowLimitDefault;
        ttmShuntSettings.VoltageLimit = ttmShuntSettings.VoltageLimitDefault;
    }

    /// <summary> Copy shunt values to the settings store. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void CopyShuntSettings()
    {
        if ( this.Part is not null && this.Part.ShuntResistance is not null )
        {
            TtmShuntSettings ttmShuntSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmShuntSettings;

            ttmShuntSettings.CurrentRange = this.Part.ShuntResistance.CurrentRange;
            ttmShuntSettings.CurrentLevel = this.Part.ShuntResistance.CurrentLevel;
            ttmShuntSettings.HighLimit = this.Part.ShuntResistance.HighLimit;
            ttmShuntSettings.LowLimit = this.Part.ShuntResistance.LowLimit;
            ttmShuntSettings.VoltageLimit = this.Part.ShuntResistance.VoltageLimit;
        }
    }

    /// <summary> Updates the shunt bound values. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private void UpdateShuntBoundValues()
    {
        if ( this.Meter is not null && this.Meter.ShuntResistance is not null )
        {
            TtmShuntSettings ttmShuntSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmShuntSettings;
            ttmShuntSettings.CurrentLevel = this.Meter.ShuntResistance.CurrentLevel;
            this._shuntResistanceCurrentLevelNumeric.Value = (( decimal ) ttmShuntSettings.CurrentLevel).Clip( this._shuntResistanceCurrentLevelNumeric.Minimum, this._shuntResistanceCurrentLevelNumeric.Maximum );
            ttmShuntSettings.CurrentRange = this.Meter.ShuntResistance.CurrentRange;
            this._shuntResistanceCurrentRangeNumeric.Value = (( decimal ) ttmShuntSettings.CurrentRange).Clip( this._shuntResistanceCurrentRangeNumeric.Minimum, this._shuntResistanceCurrentRangeNumeric.Maximum );
            ttmShuntSettings.VoltageLimit = this.Meter.ShuntResistance.VoltageLimit;
            this._shuntResistanceVoltageLimitNumeric.Value = (( decimal ) ttmShuntSettings.VoltageLimit).Clip( this._shuntResistanceVoltageLimitNumeric.Minimum, this._shuntResistanceVoltageLimitNumeric.Maximum );
            ttmShuntSettings.HighLimit = this.Meter.ShuntResistance.HighLimit;
            this._shuntResistanceHighLimitNumeric.Value = (( decimal ) ttmShuntSettings.HighLimit).Clip( this._shuntResistanceHighLimitNumeric.Minimum, this._shuntResistanceHighLimitNumeric.Maximum );
            ttmShuntSettings.LowLimit = this.Meter.ShuntResistance.LowLimit;
            this._shuntResistanceLowLimitNumeric.Value = (( decimal ) ttmShuntSettings.LowLimit).Clip( this._shuntResistanceLowLimitNumeric.Minimum, this._shuntResistanceLowLimitNumeric.Maximum );
        }
    }

    /// <summary> Gets the apply new shunt resistance button caption. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The apply shunt resistance button caption. </value>
    private string ApplyNewShuntResistanceButtonCaption { get; set; }

    /// <summary> Gets the apply shunt resistance button caption. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The apply shunt resistance button caption. </value>
    private string ApplyShuntResistanceButtonCaption { get; set; }

    /// <summary> Is new shunt resistance settings. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns>
    /// <c>true</c> if settings where updated so that meter settings needs to be updated.
    /// </returns>
    private bool IsNewShuntResistanceSettings()
    {
        return this.DeviceUnderTest is not null && this.Part is not null && this.Part.ShuntResistance is not null
            && this.DeviceUnderTest is not null && this.DeviceUnderTest.ShuntResistance is not null
            && !this.Part.ShuntResistance.ConfigurationEquals( this.DeviceUnderTest.ShuntResistance );
    }

    /// <summary> Updates the shunt configuration button caption. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void UpdateShuntConfigButtonCaption()
    {
        string caption = this.ApplyShuntResistanceButtonCaption;
        string changedCaption = this.ApplyNewShuntResistanceButtonCaption;
        if ( this.IsNewShuntResistanceSettings() )
        {
            caption += " !";
            changedCaption += " !";
        }

        if ( !caption.Equals( this._applyShuntResistanceConfigurationButton.Text, StringComparison.Ordinal ) )
        {
            this._applyShuntResistanceConfigurationButton.Text = caption;
        }

        if ( !changedCaption.Equals( this._applyNewShuntResistanceConfigurationButton.Text, StringComparison.Ordinal ) )
        {
            this._applyNewShuntResistanceConfigurationButton.Text = changedCaption;
        }
    }

    /// <summary>
    /// Handles the Click event of the _applyShuntResistanceConfigurationButton control. Saves the
    /// configuration settings and sends them to the meter.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      The <see cref="EventArgs" /> instance containing the event data. </param>
    private void ApplyShuntResistanceConfigurationButton_Click( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Meter )} is null." );
            if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )} is null." );
            if ( this.Part.ShuntResistance is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )}.{nameof( MeterView.Part.ShuntResistance )} is null." );

            activity = $"{this.Meter.ResourceName} Configuring Shunt MeasuredValue";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Configuring Shunt MeasuredValue;. " );
            if ( this.Meter.IsDeviceOpen )
            {
                // not required.
                // Me.Meter.ClearExecutionState()
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Sending Shunt resistance configuration settings to the meter;. " );
                this.Meter.ConfigureShuntResistance( this.Part.ShuntResistance );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Shunt resistance measurement configured successfully;. " );
            }
            else
            {
                this.InfoProvider?.SetError( this._applyShuntResistanceConfigurationButton, "Meter not connected" );
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Meter not connected;. " );
            }
        }
        catch ( Exception ex )
        {
            this.InfoProvider?.SetError( this._applyShuntResistanceConfigurationButton, "Failed configuring shunt resistance" );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.UpdateShuntConfigButtonCaption();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Applies new shunt resistance settings.
    ///           Event handler. Called by _applyNewShuntResistanceConfigurationButton for click
    /// events.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplyNewShuntResistanceConfigurationButton_Click( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Meter )} is null." );
            if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )} is null." );
            if ( this.Part.ShuntResistance is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )}.{nameof( MeterView.Part.ShuntResistance )} is null." );

            activity = $"{this.Meter.ResourceName} Configuring Shunt MeasuredValue";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Configuring Shunt MeasuredValue;. " );
            if ( this.Meter.IsDeviceOpen )
            {
                // not required.
                // Me.Meter.ClearExecutionState()
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Sending Shunt resistance configuration settings to the meter;. " );
                this.Meter.ConfigureShuntResistanceChanged( this.Part.ShuntResistance );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Shunt resistance measurement configured successfully;. " );
            }
            else
            {
                this.InfoProvider?.SetError( this._applyShuntResistanceConfigurationButton, "Meter not connected" );
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Meter not connected;. " );
            }
        }
        catch ( Exception ex )
        {
            this.InfoProvider?.SetError( this._applyShuntResistanceConfigurationButton, "Failed configuring shunt resistance" );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.UpdateShuntConfigButtonCaption();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Event handler. Called by _restoreShuntResistanceDefaultsButton for click events.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void RestoreShuntResistanceDefaultsButton_Click( object? sender, EventArgs e )
    {
        if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )} is null." );
        if ( this.Part.ShuntResistance is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )}.{nameof( MeterView.Part.ShuntResistance )} is null." );

        // read the instrument default settings.
        RestoreShuntDefaults();

        // reset part to know state based on the current defaults
        this.Part.ShuntResistance.ResetKnownState();
        Pith.SessionBase.DoEventsAction?.Invoke();

        // bind.
        this.BindShuntControls();
    }

    #endregion

    #region " measure shunt "

    /// <summary> Measures Shunt resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void MeasureShuntResistanceButton_Click( object? sender, EventArgs e )
    {
        lock ( this._locker )
        {
            string activity = string.Empty;
            try
            {
                if ( this.Meter is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Meter )} is null." );
                if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )} is null." );
                if ( this.Part.ShuntResistance is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )}.{nameof( MeterView.Part.ShuntResistance )} is null." );

                activity = $"{this.Meter.ResourceName} measuring Shunt MeasuredValue";
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.SetError( this._shuntResistanceTextBox, "" );
                this.InfoProvider?.SetError( this._measureShuntResistanceButton, "" );
                this.InfoProvider?.SetIconPadding( this._measureShuntResistanceButton, -15 );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Measuring Shunt MeasuredValue...;. " );
                this.Meter.MeasureShuntResistance( this.Part.ShuntResistance );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Shunt MeasuredValue measured;. " );
            }
            catch ( Exception ex )
            {
                this._shuntResistanceTextBox.Text = string.Empty;
                this.InfoProvider?.SetError( this._measureShuntResistanceButton, "Failed Measuring Shunt MeasuredValue" );
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }

    #endregion

    #endregion

    #region " display "

    /// <summary> Displays the thermal transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="textBox">    The text box control. </param>
    /// <param name="resistance"> The resistance. </param>
    private void SetErrorProvider( TextBox textBox, MeasureBase resistance )
    {
        if ( (resistance.MeasurementOutcome & MeasurementOutcomes.PartFailed) != 0 )
        {
            this.InfoProvider?.SetError( textBox, "Value out of range" );
        }
        else if ( (resistance.MeasurementOutcome & MeasurementOutcomes.MeasurementFailed) != 0 )
        {
            this.InfoProvider?.SetError( textBox, "Measurement failed" );
        }
        else if ( (resistance.MeasurementOutcome & MeasurementOutcomes.MeasurementNotMade) != 0 )
        {
            this.InfoProvider?.SetError( textBox, "Measurement not made" );
        }
    }

    /// <summary> Displays the resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="textBox">    The text box control. </param>
    /// <param name="resistance"> The resistance. </param>
    private void ShowResistance( TextBox textBox, MeasureBase resistance )
    {
        textBox.Text = resistance.MeasuredValueCaption;
        this.SetErrorProvider( textBox, resistance );
    }

    /// <summary> Displays the thermal transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="textBox">    The text box control. </param>
    /// <param name="resistance"> The resistance. </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private void ShowThermalTransient( TextBox textBox, MeasureBase resistance )
    {
        textBox.Text = resistance.MeasuredValueCaption;
        this.SetErrorProvider( textBox, resistance );
    }

    #endregion

    #region " part "

    /// <summary> Gets the part. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The part. </value>
    private DeviceUnderTest? Part { get; set; }

    /// <summary> Bind part. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> True to show or False to hide the control. </param>
    private void BindPart( DeviceUnderTest? value )
    {
        if ( this.Part is not null )
            this._resourceSelectorConnector.Enabled = false;

        this.Part = value;
        if ( value is not null )
        {
            this._resourceSelectorConnector.Openable = true;
            this._resourceSelectorConnector.Clearable = true;
            this._resourceSelectorConnector.Searchable = true;
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Enabling controls;. " );
        }

        this.BindShuntResistance( value );
        this.OnMeasurementStatusChanged();
        if ( value is not null )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Ready - List resources and select one to connect too;. " );
            this._resourceSelectorConnector.Enabled = true;
        }
    }

    /// <summary> Query if 'e' is closing ready. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="e"> Cancel event information. </param>
    /// <returns> True if closing ready, false if not. </returns>
    public bool IsClosingReady( CancelEventArgs e )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( e, nameof( e ) );
#else
        if ( e is null ) throw new ArgumentNullException( nameof( e ) );
#endif
        try
        {
            if ( this._partsPanel.SaveEnabled )
            {
                DialogResult dialogResult = System.Windows.Forms.MessageBox.Show( "Data not saved. Select Yes to save, no to skip saving or cancel to cancel closing the program.", "SAVE DATA?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly );
                if ( dialogResult == DialogResult.Yes )
                {
                    this._partsPanel.SaveParts();
                }
                else if ( dialogResult == DialogResult.No )
                {
                }
                else if ( dialogResult == DialogResult.Cancel )
                {
                    e?.Cancel = true;
                }
                else
                {
                    e?.Cancel = true;
                }
            }

            if ( e is null || !e.Cancel )
            {
                this._partsPanel.CopySettings();
                this._ttmConfigurationPanel.CopySettings();
                this.CopyShuntSettings();

                if ( this.Meter is not null && this.Meter.TspDevice is not null && this.Meter.TspDevice.Session is not null && this.Meter.TspDevice.Session.Scribe is not null
                    && this.ResourceName is not null && !string.IsNullOrWhiteSpace( this.ResourceName ) )
                {
                    this.Meter.TspDevice.Session.ResourceSettings.ResourceName = this.ResourceName;
                    this.Meter.TspDevice.Session.Scribe.WriteSettings();
                }

                // flush the log.
                cc.isr.VI.SessionLogger.CloseAndFlush();

                // wait for timer to terminate all is actions
                _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 400d ) );

                // allow all events requiring the panel to execute on their thread.
                // this allows all timer events that where in progress to be consummated before closing the form.
                // this does not prevent timer exceptions in design mode.
                for ( int i = 1; i <= 1000; i++ )
                    Application.DoEvents();
            }
        }
        finally
        {
            Pith.SessionBase.DoEventsAction?.Invoke();
        }

        return !e.Cancel;
    }

    #endregion

    #region " part: shunt resistance "

    /// <summary> Gets the shunt resistance. </summary>
    /// <value> The shunt resistance. </value>
    private ShuntResistance? ShuntResistance { get; set; }

    /// <summary> Bind shunt resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> True to show or False to hide the control. </param>
    private void BindShuntResistance( DeviceUnderTest? value )
    {
        if ( this.ShuntResistance is not null )
        {
            this.ShuntResistance.PropertyChanged -= this.ShuntResistancePropertyChanged;
            this.ShuntResistance = null;
        }

        if ( value is not null )
        {
            this.ShuntResistance = value.ShuntResistance;
            if ( this.ShuntResistance is not null )
            {
                this.ShuntResistance.PropertyChanged += this.ShuntResistancePropertyChanged;
                this.HandlePropertyChanged( this.ShuntResistance, nameof( Ttm.ShuntResistance.MeasurementAvailable ) );
            }
        }
    }

    /// <summary> Raises the property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( ShuntResistance sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        this.UpdateShuntConfigButtonCaption();
        switch ( propertyName ?? string.Empty )
        {
            case nameof( Ttm.ShuntResistance.MeasurementAvailable ):
                {
                    if ( sender.MeasurementAvailable )
                        this.ShowResistance( this._shuntResistanceTextBox, sender );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _shuntResistance for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void ShuntResistancePropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"{this.Meter?.ResourceName} handling shunt resistance {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.ShuntResistancePropertyChanged ), [sender, e] );
            else if ( sender is ShuntResistance s )
                this.HandlePropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " parts "

    /// <summary> Parts panel property changed. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property changed event information. </param>
    private void PartsPanel_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is not null && e is not null && !string.IsNullOrWhiteSpace( e.PropertyName ) )
        {
        }
    }

    /// <summary> Raises the property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( ConfigurationView sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( ConfigurationView.IsNewConfigurationSettingAvailable ):
                {
                    if ( this._navigatorTreeView is not null && this._navigatorTreeView.Nodes is not null && this._navigatorTreeView.Nodes.Count > 0 )
                    {
                        string caption = TreeViewNode.ConfigureNode.Description();
                        if ( sender.IsNewConfigurationSettingAvailable )
                        {
                            caption += " *";
                        }

                        this._navigatorTreeView.Nodes[TreeViewNode.ConfigureNode.ToString()]!.Text = caption;
                    }

                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Ttm configuration panel property changed. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property changed event information. </param>
    private void TTMConfigurationPanel_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"{this.Meter?.ResourceName} handling configuration panel {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.TTMConfigurationPanel_PropertyChanged ), [sender, e] );
            else if ( sender is ConfigurationView s )
                this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " ttm meter "

    /// <summary> Gets reference to the thermal transient meter device. </summary>
    /// <value> The meter. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public Meter? Meter { get; private set; }

    /// <summary>   Assigns a Meter. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="value">        True to show or False to hide the control. </param>
    [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
    private void AssignMeterThis( Meter? value )
    {
        if ( this.Meter is not null )
        {
            this.Meter.PropertyChanged -= this.MeterPropertyChanged;
            this.Meter.Dispose();
            this.Meter = null;
        }

        this.Meter = value;
        if ( this.Meter is not null )
        {
            this.Meter.PropertyChanged += this.MeterPropertyChanged;

            if ( this.Meter.TspDevice is null ) throw new InvalidOperationException( $"A null {nameof( K2600.Ttm.Meter.TspDevice )} was assigned." );
            this.Meter.TspDevice.Enabled = true;
        }

        this.BindPart( new DeviceUnderTest() );
        this.AssignDevice( this.Meter?.TspDevice );
        Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> Gets the is meter that owns this item. </summary>
    /// <value> The is meter owner. </value>
    private bool IsMeterOwner { get; set; }

    /// <summary>   Assigns a Meter. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="value">        True to show or False to hide the control. </param>
    public void AssignMeter( Meter? value )
    {
        this.IsMeterOwner = false;
        this.AssignMeterThis( value );
    }

    /// <summary> Releases the Meter. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected void ReleaseMeter()
    {
        if ( this.IsMeterOwner )
        {
            this.Meter?.Dispose();
            this.Meter = null;
        }
        else
        {
            this.Meter = null;
        }
    }

    /// <summary> Gets the is Meter assigned. </summary>
    /// <value> The is Meter assigned. </value>
    public bool IsMeterAssigned => this.Meter is not null && !this.Meter.IsDisposed;

    /// <summary> Raises the property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private static void HandlePropertyChanged( Meter sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( Ttm.Meter.MeasurementCompleted ):
                {
                    if ( sender.MeasurementCompleted )
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{sender.ResourceName} measurement completed;. " );

                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by Meter for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void MeterPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"{this.Meter?.ResourceName} handling meter {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.MeterPropertyChanged ), [sender, e] );
            else if ( sender is Meter s )
                MeterView.HandlePropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " device "

    /// <summary> Gets the Tsp Device. </summary>
    /// <value> The Tsp device. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public K2600Device? TspDevice => this.Device;

    /// <summary> Gets or sets the device. </summary>
    /// <value> The device. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public K2600Device? Device { get; private set; }

    /// <summary> Assigns the device and binds the relevant subsystem values. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
    private void AssignDeviceThis( K2600Device? value )
    {
        if ( this.Device is not null )
        {
            this.Device.Opening -= this.DeviceOpening;
            this.Device.Opened -= this.DeviceOpened;
            this.Device.Closing -= this.DeviceClosing;
            this.Device.Closed -= this.DeviceClosed;
            this.Device.Initialized -= this.DeviceInitialized;
            this.Device.Initializing -= this.DeviceInitializing;
            if ( this.Device.SessionFactory is not null )
                this.Device.SessionFactory.PropertyChanged -= this.SessionFactoryPropertyChanged;
            this._resourceSelectorConnector.AssignSelectorViewModel( null );
            this._resourceSelectorConnector.AssignOpenerViewModel( null );
            this.Device = null;
        }

        this.Device = value;
        if ( this.Device is not null )
        {
            if ( this.Device.SessionFactory is null ) throw new InvalidOperationException( $"An assigned device has a null {nameof( VisaSessionBase.SessionFactory )}." );
            this._resourceSelectorConnector.AssignSelectorViewModel( this.Device.SessionFactory );
            this._resourceSelectorConnector.AssignOpenerViewModel( value );
            this.Device.Opening += this.DeviceOpening;
            this.Device.Opened += this.DeviceOpened;
            this.Device.Closing += this.DeviceClosing;
            this.Device.Closed += this.DeviceClosed;
            this.Device.Initialized += this.DeviceInitialized;
            this.Device.Initializing += this.DeviceInitializing;
            this.Device.SessionFactory.PropertyChanged += this.SessionFactoryPropertyChanged;
            this.Device.SessionFactory.CandidateResourceName = this.Device.Session is null ? "" : this.Device.Session.ResourceSettings.ResourceName;
            if ( this.Device.IsDeviceOpen )
            {
                this.DeviceOpened( this.Device, EventArgs.Empty );
            }
            else
            {
                this.DeviceClosed( this.Device, EventArgs.Empty );
            }
        }
    }

    /// <summary>   Assigns a device. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="value">        True to show or False to hide the control. </param>
    public void AssignDevice( K2600Device? value )
    {
        this.AssignDeviceThis( value );
    }

    /// <summary> Reads the status register and lets the session process the status byte. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected void ReadStatusRegister()
    {
        if ( this.Device is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Device )} is null." );
        if ( this.Device.Session is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Device )}.{nameof( MeterView.Device.Session )} is null." );
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

    #region " session factory "

    /// <summary> Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string? ResourceName
    {
        get => this.Device?.SessionFactory?.CandidateResourceName;
        set
        {
            if ( this.Device?.SessionFactory is not null )
                this.Device.SessionFactory.CandidateResourceName = value ?? string.Empty;
        }
    }

    /// <summary> Gets or sets the Search Pattern of the resource. </summary>
    /// <value> The Search Pattern of the resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string? ResourceFilter
    {
        get => this.Device?.SessionFactory?.ResourcesFilter;
        set
        {
            if ( this.Device?.SessionFactory is not null )
                this.Device.SessionFactory.ResourcesFilter = value ?? string.Empty;
        }
    }

    /// <summary> Executes the session factory property changed action. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       Specifies the object where the call originated. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( SessionFactory sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( SessionFactory.ValidatedResourceName ):
                {
                    if ( sender.IsOpen )
                    {
                        this._identityTextBox.Text = $"Resource {sender.ValidatedResourceName} connected";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resource connected;. {sender.ValidatedResourceName}" );
                    }
                    else
                    {
                        this._identityTextBox.Text = $"Resource {sender.ValidatedResourceName} located";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resource located;. {sender.ValidatedResourceName}" );
                    }

                    break;
                }

            case nameof( SessionFactory.CandidateResourceName ):
                {
                    if ( !sender.IsOpen )
                    {
                        this._identityTextBox.Text = $"Resource {sender.ValidatedResourceName}";
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Candidate resource;. {sender.ValidatedResourceName}" );
                    }

                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Session factory property changed. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void SessionFactoryPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"handling session factory {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.SessionFactoryPropertyChanged ), [sender, e] );
            else if ( sender is SessionFactory s )
                this.HandlePropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " device events "

    /// <summary>
    /// Event handler. Called upon device opening so as to instantiated all subsystems.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    ///                       <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected void DeviceOpening( object? sender, CancelEventArgs e )
    {
    }

    /// <summary> Updates the availability of the controls. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void OnMeasurementStatusChanged()
    {
        MeasurementSequenceState measurementSequenceState = MeasurementSequenceState.Idle;
        if ( this.MeasureSequencer is not null )
        {
            measurementSequenceState = this.MeasureSequencer.MeasurementSequenceState;
        }

        TriggerSequenceState triggerSequenceState = TriggerSequenceState.Idle;
        if ( this.TriggerSequencer is not null )
        {
            triggerSequenceState = this.TriggerSequencer.TriggerSequenceState;
        }

        bool enabled = this.Meter is not null && this.Meter.IsDeviceOpen && TriggerSequenceState.Idle == triggerSequenceState && MeasurementSequenceState.Idle == measurementSequenceState;
        this._connectGroupBox.Enabled = TriggerSequenceState.Idle == triggerSequenceState && MeasurementSequenceState.Idle == measurementSequenceState;
        this._ttmConfigurationPanel.Enabled = enabled;
        this._measureShuntResistanceButton.Enabled = enabled;
        this._applyShuntResistanceConfigurationButton.Enabled = enabled;
        this._applyNewShuntResistanceConfigurationButton.Enabled = enabled;
    }

    /// <summary>
    /// Event handler. Called after the device opened and all subsystems were defined.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    ///                                             <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected void DeviceOpened( object? sender, EventArgs e )
    {
        if ( this.Device is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Device )} is null." );
        if ( this.Device.Session is null )
            throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Device )}.{nameof( MeterView.Device.Session )} is null." );
        if ( this.Part is null )
            throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Part )} is null." );
        if ( this.Meter is null )
            throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Meter )} is null." );
        if ( this.Meter.TspDevice is null )
            throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Meter )}.{nameof( MeterView.Meter.TspDevice )} is null." );
        if ( this.Meter.TspDevice.StatusSubsystem is null )
            throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Meter )}.{nameof( MeterView.Meter.TspDevice )}.{nameof( MeterView.Meter.TspDevice.StatusSubsystem )} is null." );
        this.Device.LogSessionOpenAndEnabled();

        string activity = string.Empty;
        try
        {
            activity = "applying device under test configuration";
            // set reference tot he device under test.
            this.DeviceUnderTest = this.Meter.ConfigInfo;
            if ( this.SupportsParts )
                this._partHeader.DeviceUnderTest = this.Part;
            this._measurementsHeader.DeviceUnderTest = this.Part;
            this._measurementsHeader.Clear();
            this._thermalTransientHeader.DeviceUnderTest = this.Part;
            this._thermalTransientHeader.Clear();
            this._ttmConfigurationPanel.Meter = this.Meter;
            this._measurementPanel.Meter = this.Meter;
            if ( this.SupportsParts )
                this._partsPanel.Part = this.Part;
            this.Part.ClearPartInfo();
            this.Part.ClearMeasurements();
            if ( this.SupportsParts )
                this._partsPanel.ClearParts();
            if ( this.SupportsParts )
                this._partsPanel.ApplySettings();
            this.Meter.Part = this.Part;
            this.MeasureSequencer = this.Meter.MeasureSequencer;
            this.TriggerSequencer = this.Meter.TriggerSequencer;

            // initialize the device system state.
            activity = "Clearing Tsp Device active state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Meter.TspDevice.ClearActiveState();
            Pith.SessionBase.DoEventsAction?.Invoke();

            activity = $"Reading identity from {this.ResourceName}";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this._identityTextBox.Text = this.Meter.TspDevice.StatusSubsystem.QueryIdentity();
            Pith.SessionBase.DoEventsAction?.Invoke();

            activity = $"Resetting and Clearing meter";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Meter.ResetClear();
            Pith.SessionBase.DoEventsAction?.Invoke();

            // reset part to know state based on the current defaults
            this.Part.ResetKnownState();
            Pith.SessionBase.DoEventsAction?.Invoke();
            this._ttmConfigurationPanel.Part = this.Part;
            Pith.SessionBase.DoEventsAction?.Invoke();

            activity = $"binding controls";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.BindShuntControls();

            activity = $"enables controls";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.OnMeasurementStatusChanged();

            activity = $"Connected to {this.ResourceName}";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " opening / open "

    /// <summary>
    /// Attempts to open a session to the device using the specified resource name.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="resourceName">  The name of the resource. </param>
    /// <param name="resourceModel"> The model of the resource. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public (bool Success, string Details) TryOpenDeviceSession( string resourceName, string resourceModel )
    {
        if ( this.Device is null ) throw new InvalidOperationException( $"{nameof( MeterView )}.{nameof( MeterView.Device )} is null." );
        return this.Device.TryOpenSession( resourceName, resourceModel );
    }

    #endregion

    #region " initalizing / initialized  "

    /// <summary> Device initializing. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Cancel event information. </param>
    protected virtual void DeviceInitializing( object? sender, CancelEventArgs e )
    {
    }

    /// <summary> Device initialized. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    ///                                             <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceInitialized( object? sender, EventArgs e )
    {
    }

    #endregion

    #region " closing / closed "

    /// <summary> Event handler. Called when device is closing. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    ///                       <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceClosing( object? sender, CancelEventArgs e )
    {
        if ( this.Device is not null )
        {
            string activity = string.Empty;
            try
            {
                activity = $"Disabling meter timer";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this._meterTimer.Enabled = false;
                activity = $"Disconnecting from {this.ResourceName}";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.Device.Session?.DisableServiceRequestEventHandler();
            }
            catch ( Exception ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            }
        }
    }

    /// <summary> Event handler. Called when device is closed. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    ///                       <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceClosed( object? sender, EventArgs e )
    {
        if ( this.Device is not null && this.Device.Session is not null )
        {
            string activity = string.Empty;
            try
            {
                _ = this.Device.Session.IsSessionOpen
                    ? cc.isr.VI.SessionLogger.Instance.LogWarning( $"{this.Device.Session.ResourceNameCaption} closed but session still open;. " )
                    : this.Device.Session.IsDeviceOpen
                        ? cc.isr.VI.SessionLogger.Instance.LogWarning( $"{this.Device.Session.ResourceNameCaption} closed but emulated session still open;. " )
                        : cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Disconnected; Device access closed." );

                activity = $"disables controls";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.OnMeasurementStatusChanged();
            }
            catch ( Exception ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            }
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Disconnected; Device disposed." );
        }
    }

    #endregion

    #endregion

    #region " sequenced measurements "

#if NET9_0_OR_GREATER

#endif

    /// <summary>   Gets or sets the measure sequencer internal. </summary>
    /// <value> The measure sequencer internal. </value>
    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
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

    /// <summary> Gets or sets the sequencer. </summary>
    /// <value> The sequencer. </value>
    private MeasureSequencer? MeasureSequencer
    {
        get => this.MeasureSequencerInternal;
        set => this.MeasureSequencerInternal = value;
    }

    /// <summary> Handles the measure sequencer property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( MeasureSequencer sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
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
    private void MeasureSequencer_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"{this.Meter?.ResourceName} handling measure sequencer {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.MeasureSequencer_PropertyChanged ), [sender, e] );
            else if ( sender is MeasureSequencer s )
                this.OnPropertyChanged( s, e?.PropertyName! );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Ends a completed sequence. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private void OnMeasurementSequenceCompleted()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Measurement completed;. " );
        string activity = string.Empty;
        try
        {
            activity = $"{this.Meter?.ResourceName} handling measure completed event";
            // add part if auto add is enabled.
            if ( this._partsPanel.AutoAddEnabled )
            {
                activity = $"{this.Meter?.ResourceName} adding part";
                this._partsPanel.AddPart();
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> State of the last measurement sequence. </summary>
    private MeasurementSequenceState _lastMeasurementSequenceState;

    /// <summary> Handles the change in measurement state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="state"> The state. </param>
    private void OnMeasurementSequenceStateChanged( MeasurementSequenceState state )
    {
        if ( this._lastMeasurementSequenceState != state )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Processing the {( int ) state}{state.Description()} state;. " );
            this._lastMeasurementSequenceState = state;
        }

        switch ( state )
        {
            case MeasurementSequenceState.Aborted:
                {
                    break;
                }

            case MeasurementSequenceState.Completed:
                {
                    break;
                }

            case MeasurementSequenceState.Failed:
                {
                    break;
                }

            case MeasurementSequenceState.MeasureInitialResistance:
                {
                    break;
                }

            case MeasurementSequenceState.MeasureThermalTransient:
                {
                    break;
                }

            case MeasurementSequenceState.Idle:
                {
                    this.OnMeasurementStatusChanged();
                    break;
                }

            case var @case when @case == MeasurementSequenceState.None:
                {
                    break;
                }

            case MeasurementSequenceState.PostTransientPause:
                {
                    break;
                }

            case MeasurementSequenceState.MeasureFinalResistance:
                {
                    break;
                }

            case MeasurementSequenceState.Starting:
                {
                    this.OnMeasurementStatusChanged();
                    break;
                }

            default:
                {
                    Debug.Assert( !Debugger.IsAttached, "Unhandled state: " + state.ToString() );
                    break;
                }
        }
    }

    #endregion

    #region " triggered measurements "

#if NET9_0_OR_GREATER

#endif

    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
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
    private TriggerSequencer? TriggerSequencer
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
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
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
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void TriggerSequencer_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"{this.Meter?.ResourceName} handling trigger sequencer {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.TriggerSequencer_PropertyChanged ), [sender, e] );
            else if ( sender is TriggerSequencer s )
                this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> State of the last trigger sequence. </summary>
    private TriggerSequenceState _lastTriggerSequenceState;

    /// <summary> Handles the change in measurement state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="state"> The state. </param>
    private void OnTriggerSequenceStateChanged( TriggerSequenceState state )
    {
        if ( this._lastTriggerSequenceState != state )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Processing the {( int ) state}:{state.Description()} state;. " );
            this._lastTriggerSequenceState = state;
        }

        switch ( state )
        {
            case TriggerSequenceState.Aborted:
                {
                    this.OnMeasurementStatusChanged();
                    break;
                }

            case TriggerSequenceState.Stopped:
                {
                    break;
                }

            case TriggerSequenceState.Failed:
                {
                    break;
                }

            case TriggerSequenceState.WaitingForTrigger:
                {
                    break;
                }

            case TriggerSequenceState.MeasurementCompleted:
                {
                    break;
                }

            case TriggerSequenceState.ReadingValues:
                {
                    break;
                }

            case TriggerSequenceState.Idle:
                {
                    this.OnMeasurementStatusChanged();
                    break;
                }

            case var @case when @case == TriggerSequenceState.None:
                {
                    break;
                }

            case TriggerSequenceState.Starting:
                {
                    this.OnMeasurementStatusChanged();
                    break;
                }

            default:
                {
                    Debug.Assert( !Debugger.IsAttached, "Unhandled state: " + state.ToString() );
                    break;
                }
        }
    }

    #endregion

    #region " tab control events "

    /// <summary> Tabs draw item. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Draw item event information. </param>
    private void Tabs_DrawItem( object? sender, DrawItemEventArgs e )
    {
        TabPage page = this._tabs.TabPages[e.Index];
        System.Drawing.Rectangle paddedBounds = e.Bounds;
        System.Drawing.Color backClr = e.State == DrawItemState.Selected ? System.Drawing.SystemColors.ControlDark : page.BackColor;
        using ( System.Drawing.Brush brush = new System.Drawing.SolidBrush( backClr ) )
        {
            e.Graphics.FillRectangle( brush, paddedBounds );
        }

        int yOffset = e.State == DrawItemState.Selected ? -2 : 1;
        paddedBounds = e.Bounds;
        paddedBounds.Offset( 1, yOffset );
        TextRenderer.DrawText( e.Graphics, page.Text, page.Font, paddedBounds, page.ForeColor );
    }

    #endregion

    #region " navigation "

    /// <summary> Gets the support parts. </summary>
    /// <value> The support parts. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public bool SupportsParts { get; set; }

    /// <summary> Gets the supports shunt. </summary>
    /// <value> The supports shunt. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public bool SupportsShunt { get; set; }

    /// <summary> Enumerates the nodes. Each item is the same as the node name. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private enum TreeViewNode
    {
        /// <summary> An enum constant representing the connect node option. </summary>
        [Description( "CONNECT" )]
        ConnectNode,

        /// <summary> An enum constant representing the configure node option. </summary>
        [Description( "CONFIGURE" )]
        ConfigureNode,

        /// <summary> An enum constant representing the measure node option. </summary>
        [Description( "MEASURE" )]
        MeasureNode,

        /// <summary> An enum constant representing the shunt node option. </summary>
        [Description( "SHUNT" )]
        ShuntNode,

        /// <summary> An enum constant representing the parts node option. </summary>
        [Description( "PARTS" )]
        PartsNode,

        /// <summary> An enum constant representing the messages node option. </summary>
        [Description( "MESSAGES" )]
        MessagesNode
    }

    /// <summary> Builds the navigator tree view. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    private void BuildNavigatorTreeView()
    {
        this._partHeader.Visible = this.SupportsParts;
        this._splitContainer.Dock = DockStyle.Fill;
        this._splitContainer.SplitterDistance = 120;
        this._navigatorTreeView.Enabled = false;
        this._navigatorTreeView.Nodes.Clear();
        List<TreeNode> nodes = [];
#pragma warning disable CA2263
        foreach ( TreeViewNode node in Enum.GetValues( typeof( TreeViewNode ) ) )
#pragma warning restore CA2263
        {
            if ( node == TreeViewNode.PartsNode && !this.SupportsParts )
                continue;
            if ( node == TreeViewNode.ShuntNode && !this.SupportsShunt )
                continue;
            nodes.Add( new TreeNode( node.Description() ) );
            nodes[^0].Name = node.ToString();
            nodes[^0].Text = node.Description();
            if ( node == TreeViewNode.MessagesNode )
            {
                this.TextBoxTextWriter.ContainerTreeNode = nodes[^0];
                this.TextBoxTextWriter.TabCaption = "MESSAGES";
            }
        }

        this._navigatorTreeView.Nodes.AddRange( [.. nodes] );
        this._navigatorTreeView.Enabled = true;
    }

    /// <summary> The last node selected. </summary>
    private TreeNode? _lastNodeSelected;

    /// <summary> Gets the last tree view node selected. </summary>
    /// <value> The last tree view node selected. </value>
#pragma warning disable CA2263
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    private TreeViewNode? LastTreeViewNodeSelected => this._lastNodeSelected is null ? 0 : ( TreeViewNode ) ( int ) Enum.Parse( typeof( TreeViewNode ), this._lastNodeSelected.Name );
#pragma warning restore CA2263
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    private List<TreeViewNode> _nodesVisited = [];

    /// <summary> Called after a node is selected. Displays to relevant screen. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="node"> The node. </param>
    private void OnNodeSelected( TreeViewNode? node )
    {
        if ( node is null ) return;
        this._nodesVisited ??= [];

        Control? activeControl = null;
        Control? focusControl = null;
        DataGridView? activeDisplay = null;
        switch ( node )
        {
            case TreeViewNode.ConnectNode:
                {
                    activeControl = this._connectTabLayout;
                    focusControl = null;
                    activeDisplay = null;
                    break;
                }

            case TreeViewNode.ConfigureNode:
                {
                    activeControl = this._mainLayout;
                    focusControl = null;
                    activeDisplay = null;
                    break;
                }

            case TreeViewNode.MeasureNode:
                {
                    activeControl = this._ttmLayout;
                    focusControl = null;
                    activeDisplay = null;
                    break;
                }

            case TreeViewNode.ShuntNode:
                {
                    activeControl = this._shuntLayout;
                    focusControl = null;
                    activeDisplay = null;
                    break;
                }

            case TreeViewNode.PartsNode:
                {
                    activeControl = this._partsLayout;
                    focusControl = null;
                    activeDisplay = null;
                    break;
                }

            case TreeViewNode.MessagesNode:
                {
                    activeControl = this._traceMessagesBox;
                    focusControl = null;
                    activeDisplay = null;
                    break;
                }

            default:
                break;
        }

        // turn off the visibility of the current panel, this turns off the visibility of the
        // contained controls, which will now be removed from the panel.
        this._splitContainer.Panel2.Hide();
        this._splitContainer.Panel2.Controls.Clear();
        // If Me._splitContainer.Panel2.HasChildren Then
        // For Each Control As Control In .Controls
        // Me._splitContainer.Panel2.Controls.Remove(Control)
        // Next
        // End If

        if ( activeControl is not null )
        {
            activeControl.Dock = DockStyle.None;
            this._splitContainer.Panel2.Controls.Add( activeControl );
            activeControl.Dock = DockStyle.Fill;
        }

        // turn on visibility on the panel -- this toggles the visibility of the contained controls,
        // which is required for the messages boxes.
        this._splitContainer.Panel2.Show();
        if ( !this._nodesVisited.Contains( node.Value ) )
        {
            if ( focusControl is not null )
            {
                _ = focusControl.Focus();
            }

            this._nodesVisited.Add( node.Value );
        }

        if ( activeDisplay is not null )
        {
            // DataDirector.UpdateColumnDisplayOrder(activeDisplay, columnOrder)
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Console"/> is navigating.
    /// </summary>
    /// <remarks>
    /// Used to ignore changes in grids during the navigation. The grids go through selecting their
    /// rows when navigating.
    /// </remarks>
    /// <value> <c>true</c> if navigating; otherwise, <c>false</c>. </value>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0052:Remove unread private members", Justification = "<Pending>" )]
    private bool Navigating { get; set; }

    /// <summary> Handles the BeforeSelect event of the _navigatorTreeView control. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      The <see cref="TreeViewCancelEventArgs"/> instance
    ///                       containing the event data. </param>
    private void NavigatorTreeView_BeforeSelect( object? sender, TreeViewCancelEventArgs e )
    {
        this.Navigating = true;
    }

    /// <summary> Handles the AfterSelect event of the Me._navigatorTreeView control. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      The <see cref="TreeViewEventArgs" /> instance
    ///                       containing the event data. </param>
    private void NavigatorTreeView_afterSelect( object? sender, TreeViewEventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.Meter?.ResourceName} handling navigator after select event";
            _ = (this._lastNodeSelected?.BackColor = this._navigatorTreeView.BackColor);

            if ( sender is not null && sender is Control s && s.Enabled && e is not null && e.Node is not null && e.Node.IsSelected )
            {
                this._lastNodeSelected = e.Node;
                e.Node.BackColor = System.Drawing.SystemColors.Highlight;
                this.OnNodeSelected( this.LastTreeViewNodeSelected );
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.Navigating = false;
        }
    }

    /// <summary> Selects the navigator tree view node. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="node"> The node. </param>
    private void SelectNavigatorTreeViewNode( TreeViewNode node )
    {
        this._navigatorTreeView.SelectedNode = this._navigatorTreeView.Nodes[node.ToString()];
    }

    #endregion
}
