using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using cc.isr.Std.NumericExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> Panel for editing the configuration. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-02-25 </para>
/// </remarks>
public partial class ConfigurationViewBase : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary>
    /// A private constructor for this class making it not publicly creatable. This ensure using the
    /// class as a singleton.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected ConfigurationViewBase() : base()
    {
        this.InitializeComponent();
        this.ApplyConfigurationButtonCaption = this._applyConfigurationButton.Text;
        this.ApplyNewConfigurationButtonCaption = this._applyNewConfigurationButton.Text;
    }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    ///                          release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( disposing )
            {
                this.ReleaseResources();
                this.components?.Dispose();
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " on control events "

    /// <summary> Releases the resources. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected virtual void ReleaseResources()
    {
        this.DeviceUnderTest = null;
        this.PartInternal = null;
    }

    #endregion

    #region " part "


    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    private DeviceUnderTest? PartInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.Part_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.Part_PropertyChanged;
        }
    }

    /// <summary> Gets or sets the part. </summary>
    /// <value> The part. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public DeviceUnderTest? Part
    {
        get => this.PartInternal;
        set => this.PartInternal = value;
    }

    /// <summary> Executes the property changed action. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( DeviceUnderTest sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( Ttm.DeviceUnderTest.InitialResistance ):
                {
                    this.OnConfigurationChanged();
                    break;
                }

            case nameof( Ttm.DeviceUnderTest.FinalResistance ):
                {
                    this.OnConfigurationChanged();
                    break;
                }

            case nameof( Ttm.DeviceUnderTest.ShuntResistance ):
                {
                    break;
                }

            case nameof( Ttm.DeviceUnderTest.ThermalTransient ):
                {
                    this.OnConfigurationChanged();
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _initialResistance for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void Part_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        string activity = string.Empty;
        try
        {
            activity = $"handling part {e?.PropertyName} property changed event";
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.Part_PropertyChanged ), [sender, e] );
            else if ( sender is DeviceUnderTest s )
                this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Bind controls. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void BindInitialResistanceControls()
    {
        if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( ConfigurationViewBase )}.{nameof( this.Part )} is null." );
        if ( this.Part.InitialResistance is null ) throw new InvalidOperationException( $"{nameof( ConfigurationViewBase )}.{nameof( this.Part )}..{nameof( this.Part.InitialResistance )} is null." );

        TtmMeterSettings ttmMeterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
        TtmResistanceSettings ttmResistanceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings;

        // set the GUI based on the current defaults.
        this._checkContactsCheckBox.DataBindings.Clear();
        this._checkContactsCheckBox.DataBindings.Add( new Binding( "Checked", this.Part, "ContactCheckEnabled",
            true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._checkContactsCheckBox.Checked = ttmMeterSettings.ContactCheckEnabled;
        this.Part.ContactCheckEnabled = this._checkContactsCheckBox.Checked;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        this._coldVoltageNumeric.Minimum = ( decimal ) ttmResistanceSettings.VoltageMinimum;
        this._coldVoltageNumeric.Maximum = ( decimal ) ttmResistanceSettings.VoltageMaximum;
        this._coldVoltageNumeric.DataBindings.Clear();
        this._coldVoltageNumeric.DataBindings.Add( new Binding( "Value", this.Part.InitialResistance, "VoltageLimit", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._coldVoltageNumeric.Value = (( decimal ) ttmResistanceSettings.VoltageLimit).Clip( this._coldVoltageNumeric.Minimum, this._coldVoltageNumeric.Maximum );
        this.Part.InitialResistance.VoltageLimit = ( double ) this._coldVoltageNumeric.Value;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        this._coldCurrentNumeric.Minimum = ( decimal ) ttmResistanceSettings.CurrentMinimum;
        this._coldCurrentNumeric.Maximum = ( decimal ) ttmResistanceSettings.CurrentMaximum;
        this._coldCurrentNumeric.DataBindings.Clear();
        this._coldCurrentNumeric.DataBindings.Add( new Binding( "Value", this.Part.InitialResistance, "CurrentLevel", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._coldCurrentNumeric.Value = (( decimal ) ttmResistanceSettings.CurrentLevel).Clip( this._coldCurrentNumeric.Minimum, this._coldCurrentNumeric.Maximum );
        this.Part.InitialResistance.CurrentLevel = ( double ) this._coldCurrentNumeric.Value;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        this._coldResistanceHighLimitNumeric.Minimum = ( decimal ) ttmResistanceSettings.Minimum;
        this._coldResistanceHighLimitNumeric.Maximum = ( decimal ) ttmResistanceSettings.Maximum;
        this._coldResistanceHighLimitNumeric.DataBindings.Clear();
        this._coldResistanceHighLimitNumeric.DataBindings.Add( new Binding( "Value", this.Part.InitialResistance, "HighLimit", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._coldResistanceHighLimitNumeric.Value = (( decimal ) ttmResistanceSettings.HighLimit).Clip( this._coldResistanceHighLimitNumeric.Minimum, this._coldResistanceHighLimitNumeric.Maximum );
        this.Part.InitialResistance.HighLimit = ( double ) this._coldResistanceHighLimitNumeric.Value;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        this._coldResistanceLowLimitNumeric.Minimum = ( decimal ) ttmResistanceSettings.Minimum;
        this._coldResistanceLowLimitNumeric.Maximum = ( decimal ) ttmResistanceSettings.Maximum;
        this._coldResistanceLowLimitNumeric.DataBindings.Clear();
        this._coldResistanceLowLimitNumeric.DataBindings.Add( new Binding( "Value", this.Part.InitialResistance, "LowLimit", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._coldResistanceLowLimitNumeric.Value = (( decimal ) ttmResistanceSettings.LowLimit).Clip( this._coldResistanceLowLimitNumeric.Minimum, this._coldResistanceLowLimitNumeric.Maximum );
        this.Part.InitialResistance.LowLimit = ( double ) this._coldResistanceLowLimitNumeric.Value;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary>   Bind controls. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    public void BindThermalTransientControls()
    {
        if ( this.Part is null ) throw new InvalidOperationException( $"{nameof( ConfigurationViewBase )}.{nameof( this.Part )} is null." );
        if ( this.Part.ThermalTransient is null ) throw new InvalidOperationException( $"{nameof( ConfigurationViewBase )}.{nameof( this.Part )}..{nameof( this.Part.ThermalTransient )} is null." );
        if ( this.Part.MeterMain is null ) throw new InvalidOperationException( $"{nameof( ConfigurationViewBase )}.{nameof( this.Part )}..{nameof( this.Part.MeterMain )} is null." );

        TtmMeterSettings ttmMeterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
        TtmTraceSettings ttmTraceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings;

        // set the GUI based on the current defaults.
        this._postTransientDelayNumeric.Minimum = ( decimal ) ttmMeterSettings.PostTransientDelayMinimum;
        this._postTransientDelayNumeric.Maximum = ( decimal ) ttmMeterSettings.PostTransientDelayMaximum;
        this._postTransientDelayNumeric.DataBindings.Clear();
        this._postTransientDelayNumeric.DataBindings.Add( new Binding( "Value", this.Part.ThermalTransient, "PostTransientDelay", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._postTransientDelayNumeric.Value = (( decimal ) ttmMeterSettings.PostTransientDelay).Clip( this._postTransientDelayNumeric.Minimum, this._postTransientDelayNumeric.Maximum );
        this.Part.MeterMain.PostTransientDelay = ( double ) this._postTransientDelayNumeric.Value;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this._thermalVoltageNumeric.Minimum = ( decimal ) ttmTraceSettings.VoltageMinimum;
        this._thermalVoltageNumeric.Maximum = ( decimal ) ttmTraceSettings.VoltageMaximum;
        this._thermalVoltageNumeric.DataBindings.Clear();
        this._thermalVoltageNumeric.DataBindings.Add( new Binding( "Value", this.Part.ThermalTransient, "AllowedVoltageChange", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._thermalVoltageNumeric.Value = (( decimal ) ttmTraceSettings.VoltageChange).Clip( this._thermalVoltageNumeric.Minimum, this._thermalVoltageNumeric.Maximum );
        this.Part.ThermalTransient.AllowedVoltageChange = ( double ) this._thermalVoltageNumeric.Value;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this._thermalCurrentNumeric.Minimum = ( decimal ) ttmTraceSettings.CurrentMinimum;
        this._thermalCurrentNumeric.Maximum = ( decimal ) ttmTraceSettings.CurrentMaximum;
        this._thermalCurrentNumeric.DataBindings.Clear();
        this._thermalCurrentNumeric.DataBindings.Add( new Binding( "Value", this.Part.ThermalTransient, "CurrentLevel", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._thermalCurrentNumeric.Value = (( decimal ) ttmTraceSettings.CurrentLevel).Clip( this._thermalCurrentNumeric.Minimum, this._thermalCurrentNumeric.Maximum );
        this.Part.ThermalTransient.CurrentLevel = ( double ) this._thermalCurrentNumeric.Value;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this._thermalVoltageLimitNumeric.Minimum = ( decimal ) ttmTraceSettings.VoltageMinimum;
        this._thermalVoltageLimitNumeric.Maximum = ( decimal ) ttmTraceSettings.VoltageMaximum;
        this._thermalVoltageLimitNumeric.DataBindings.Clear();
        this._thermalVoltageLimitNumeric.DataBindings.Add( new Binding( "Value", this.Part.ThermalTransient, "VoltageLimit", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._thermalVoltageLimitNumeric.Value = (( decimal ) ttmTraceSettings.VoltageLimit).Clip( this._thermalVoltageLimitNumeric.Minimum, this._thermalVoltageLimitNumeric.Maximum );
        this.Part.ThermalTransient.VoltageLimit = ( double ) this._thermalVoltageLimitNumeric.Value;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this._thermalVoltageHighLimitNumeric.Minimum = ( decimal ) ttmTraceSettings.VoltageChangeMinimum;
        this._thermalVoltageHighLimitNumeric.Maximum = ( decimal ) ttmTraceSettings.VoltageChangeMaximum;
        this._thermalVoltageHighLimitNumeric.DataBindings.Clear();
        this._thermalVoltageHighLimitNumeric.DataBindings.Add( new Binding( "Value", this.Part.ThermalTransient, "HighLimit", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._thermalVoltageHighLimitNumeric.Value = (( decimal ) ttmTraceSettings.HighLimit).Clip( this._thermalVoltageHighLimitNumeric.Minimum, this._thermalVoltageHighLimitNumeric.Maximum );
        this.Part.ThermalTransient.HighLimit = ( double ) this._thermalVoltageHighLimitNumeric.Value;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this._thermalVoltageLowLimitNumeric.Minimum = ( decimal ) ttmTraceSettings.VoltageChangeMinimum;
        this._thermalVoltageLowLimitNumeric.Maximum = ( decimal ) ttmTraceSettings.VoltageChangeMaximum;
        this._thermalVoltageLowLimitNumeric.DataBindings.Clear();
        this._thermalVoltageLowLimitNumeric.DataBindings.Add( new Binding( "Value", this.Part.ThermalTransient, "LowLimit", true, DataSourceUpdateMode.OnPropertyChanged ) );
        this._thermalVoltageLowLimitNumeric.Value = (( decimal ) ttmTraceSettings.LowLimit).Clip( this._thermalVoltageLowLimitNumeric.Minimum, this._thermalVoltageLowLimitNumeric.Maximum );
        this.Part.ThermalTransient.LowLimit = ( double ) this._thermalVoltageLowLimitNumeric.Value;
    }

    #endregion

    #region " device under test "

    /// <summary> Gets or sets the device under test. </summary>
    /// <value> The device under test. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public DeviceUnderTest? DeviceUnderTest { get; set; }

    #endregion

    #region " configure "

    /// <summary> Restore defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private static void RestoreDefaults()
    {
        TtmMeterSettings ttmMeterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
        TtmResistanceSettings ttmResistanceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings;
        TtmTraceSettings ttmTraceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings;

        ttmMeterSettings.ContactCheckEnabled = ttmMeterSettings.ContactCheckEnabledDefault;
        ttmMeterSettings.ContactCheckThreshold = ttmMeterSettings.ContactCheckThresholdDefault;
        ttmResistanceSettings.VoltageLimit = ttmResistanceSettings.VoltageLimitDefault;
        ttmResistanceSettings.CurrentLevel = ttmResistanceSettings.CurrentLevelDefault;
        ttmResistanceSettings.HighLimit = ttmResistanceSettings.HighLimitDefault;
        ttmResistanceSettings.LowLimit = ttmResistanceSettings.LowLimitDefault;
        ttmMeterSettings.PostTransientDelay = ttmMeterSettings.PostTransientDelayDefault;
        ttmTraceSettings.VoltageChange = ttmTraceSettings.VoltageChangeDefault;
        ttmTraceSettings.CurrentLevel = ttmTraceSettings.CurrentLevelDefault;
        ttmTraceSettings.HighLimit = ttmTraceSettings.HighLimitDefault;
        ttmTraceSettings.LowLimit = ttmTraceSettings.LowLimitDefault;
        ttmTraceSettings.VoltageLimit = ttmTraceSettings.VoltageLimitDefault;
    }

    /// <summary> Copy part values to the settings store. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void CopySettings()
    {
        if ( this.Part is not null && this.Part.InitialResistance is not null && this.Part.ThermalTransient is not null && this.Part.MeterMain is not null )
        {
            TtmMeterSettings ttmMeterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
            TtmResistanceSettings ttmResistanceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings;
            TtmTraceSettings ttmTraceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings;

            ttmMeterSettings.SourceMeasureUnit = this.Part.InitialResistance.SourceMeasureUnit;
            ttmMeterSettings.ContactCheckEnabled = this.Part.ContactCheckEnabled;
            ttmMeterSettings.ContactCheckThreshold = this.Part.ContactCheckThreshold;
            ttmResistanceSettings.VoltageLimit = this.Part.InitialResistance.VoltageLimit;
            ttmResistanceSettings.CurrentLevel = this.Part.InitialResistance.CurrentLevel;
            ttmResistanceSettings.HighLimit = this.Part.InitialResistance.HighLimit;
            ttmResistanceSettings.LowLimit = this.Part.InitialResistance.LowLimit;
            ttmMeterSettings.PostTransientDelay = this.Part.MeterMain.PostTransientDelay;
            ttmTraceSettings.VoltageChange = this.Part.ThermalTransient.AllowedVoltageChange;
            ttmTraceSettings.CurrentLevel = this.Part.ThermalTransient.CurrentLevel;
            ttmTraceSettings.HighLimit = this.Part.ThermalTransient.HighLimit;
            ttmTraceSettings.LowLimit = this.Part.ThermalTransient.LowLimit;
            ttmTraceSettings.VoltageLimit = this.Part.ThermalTransient.VoltageLimit;
        }
    }

    /// <summary> Gets or sets the apply new configuration button caption. </summary>
    /// <value> The apply new configuration button caption. </value>
    private string ApplyNewConfigurationButtonCaption { get; set; }

    /// <summary> Gets or sets the configuration button caption. </summary>
    /// <value> The apply configuration button caption. </value>
    private string ApplyConfigurationButtonCaption { get; set; }

    /// <summary> Checks if new configuration settings need to be applied. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns>
    /// <c>true</c> if settings where updated so that meter settings needs to be updated.
    /// </returns>
    private bool IsNewConfigurationSettings()
    {
        return this.Part is not null && this.DeviceUnderTest is not null && !this.Part.ThermalTransientConfigurationEquals( this.DeviceUnderTest );
    }

#if NET9_0_OR_GREATER

#endif

    /// <summary> Gets or sets the is new configuration setting available. </summary>
    /// <value> The is new configuration setting available. </value>
    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public bool IsNewConfigurationSettingAvailable
    {
        get;
        set
        {
            if ( !value.Equals( this.IsNewConfigurationSettingAvailable ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Updates the configuration status. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void OnConfigurationChanged()
    {
        string caption = this.ApplyConfigurationButtonCaption;
        string changedCaption = this.ApplyNewConfigurationButtonCaption;
        this.IsNewConfigurationSettingAvailable = this.IsNewConfigurationSettings();
        if ( this.IsNewConfigurationSettingAvailable )
        {
            caption += " !";
            changedCaption += " !";
        }

        if ( !caption.Equals( this._applyConfigurationButton.Text, StringComparison.Ordinal ) )
        {
            this._applyConfigurationButton.Text = caption;
        }

        if ( !changedCaption.Equals( this._applyNewConfigurationButton.Text, StringComparison.Ordinal ) )
        {
            this._applyNewConfigurationButton.Text = changedCaption;
        }
    }

    /// <summary> Configures changed values. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="part"> The Part. </param>
    protected virtual void ConfigureChanged( DeviceUnderTest part )
    {
    }

    /// <summary>
    /// Event handler. Called by _applyNewThermalTransientConfigurationButton for click events.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplyNewConfigurationButton_Click( object? sender, EventArgs e )
    {
        if ( this.Part is null || this.DeviceUnderTest is null || this.Part.InitialResistance is null || this.Part.FinalResistance is null )
        {
            this.InfoProvider?.SetError( this._applyNewConfigurationButton, "Meter not connected" );
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Meter not connected;. " );
            return;
        }

        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"Applying configuration";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Part.FinalResistance.CopyConfiguration( this.Part.InitialResistance );
            if ( !this.DeviceUnderTest.InfoConfigurationEquals( this.Part ) )
            {
                // clear execution state is not required - done before the measurement.
                // Me.Meter.ClearExecutionState()
                activity = $"Configuring Thermal Transient";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.ConfigureChanged( this.Part );
            }
        }
        catch ( InvalidOperationException ex )
        {
            this.InfoProvider?.SetError( this._applyNewConfigurationButton, "failed configuring--most one configuration element did not succeed in setting the correct value. See messages for details." );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        catch ( ArgumentException ex )
        {
            this.InfoProvider?.SetError( this._applyNewConfigurationButton, "failed configuring--most likely due to a validation issue. See messages for details." );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        catch ( Exception ex )
        {
            this.InfoProvider?.SetError( this._applyNewConfigurationButton, "failed configuring thermal transient" );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.OnConfigurationChanged();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Configures all values. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="part"> The Part. </param>
    protected virtual void Configure( DeviceUnderTest part )
    {
    }

    /// <summary> Saves the configuration settings and sends them to the meter. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplyConfigurationButton_Click( object? sender, EventArgs e )
    {
        if ( this.Part is null || this.DeviceUnderTest is null || this.Part.InitialResistance is null || this.Part.FinalResistance is null )
        {
            this.InfoProvider?.SetError( this._applyConfigurationButton, "Meter not connected" );
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Meter not connected;. " );
            return;
        }

        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"Applying configuration";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Part.FinalResistance.CopyConfiguration( this.Part.InitialResistance );
            // clear execution state is not required - done before the measurement.
            // Me.Meter.ClearExecutionState()
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Configuring Thermal Transient;. " );
            this.Configure( this.Part );
        }
        catch ( InvalidOperationException ex )
        {
            this.InfoProvider?.SetError( this._applyNewConfigurationButton, "failed configuring--most one configuration element did not succeed in setting the correct value. See messages for details." );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        catch ( ArgumentException ex )
        {
            this.InfoProvider?.SetError( this._applyNewConfigurationButton, "failed configuring--most likely due to a validation issue. See messages for details." );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        catch ( Exception ex )
        {
            this.InfoProvider?.SetError( this._applyNewConfigurationButton, "failed configuring thermal transient" );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            this.OnConfigurationChanged();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Event handler. Called by _restoreDefaultsButton for click events. Restores the defaults
    /// settings.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void RestoreDefaultsButton_Click( object? sender, EventArgs e )
    {
        // read the instrument default settings.
        RestoreDefaults();

        // reset part to know state based on the current defaults
        this.Part?.ResetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        // bind.
        this.BindInitialResistanceControls();
        this.BindThermalTransientControls();
    }

    #endregion
}
