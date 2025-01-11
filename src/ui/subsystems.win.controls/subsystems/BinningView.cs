using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;
using cc.isr.WinControls.CheckStateExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A Calculate Subsystem view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class BinningView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public BinningView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._passBitPatternNumeric.NumericUpDown.Minimum = 1m;
        this._passBitPatternNumeric.NumericUpDown.Maximum = 63m;
        this._passBitPatternNumeric.NumericUpDown.Value = 32m;
        this._passBitPatternNumeric.NumericUpDown.DecimalPlaces = 0;
        this._binningStrobeDurationNumeric.NumericUpDown.Minimum = 0m;
        this._binningStrobeDurationNumeric.NumericUpDown.Maximum = 999m;
        this._binningStrobeDurationNumeric.NumericUpDown.Value = 0m;
        this._binningStrobeDurationNumeric.NumericUpDown.DecimalPlaces = 2;
    }

    /// <summary> Creates a new Calculate View. </summary>
    /// <returns> A Calculate View. </returns>
    public static BinningView Create()
    {
        BinningView? view = null;
        try
        {
            view = new BinningView();
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

    /// <summary> Applies the settings. </summary>
    public void ApplySettings()
    {
        if ( this.Device is null || this.BinningSubsystem is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} applying Binning subsystem state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.BinningSubsystem.StartElapsedStopwatch();
            _ = this.BinningSubsystem.ApplyPassSource( ( int ) this._passBitPatternNumeric.Value );
            _ = this.BinningSubsystem.ApplyBinningStrobeEnabled( this._BinningStrobeToggleButton.CheckState == CheckState.Checked );
            this.BinningSubsystem.StopElapsedStopwatch();
            this.ApplySettings( this.DigitalOutputSubsystem );
            this._limit1View.ApplySettings();
            this._limit2View.ApplySettings();
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

    /// <summary> Reads the settings. </summary>
    public void ReadSettings()
    {
        if ( this.InitializingComponents || this.Device is null || this.BinningSubsystem is null || this.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device?.ResourceNameCaption} reading Binning subsystem state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            ReadSettings( this.BinningSubsystem );
            this.ApplyPropertyChanged( this.BinningSubsystem );
            ReadSettings( this.DigitalOutputSubsystem );
            this.ApplyPropertyChanged( this.DigitalOutputSubsystem );
            this._limit1View.ReadSettings();
            this._limit2View.ReadSettings();
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

    /// <summary> Preform a limit test and read the fail state. </summary>
    public void PerformLimitTest()
    {
        if ( this.InitializingComponents || this.Device is null || this.BinningSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device?.ResourceNameCaption} perform Binning limit test";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            PerformLimitTest( this.BinningSubsystem );
            this.ReadLimitTestState();
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

    /// <summary> read limit test result. </summary>
    public void ReadLimitTestState()
    {
        if ( this.InitializingComponents || this.Device is null || this.BinningSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            this.BinningSubsystem.StartElapsedStopwatch();
            _ = this.BinningSubsystem.QueryLimitsFailed();
            _ = this.BinningSubsystem.QueryLimit1Failed();
            _ = this.BinningSubsystem.QueryLimit2Failed();
            _ = this.Device.StatusSubsystemBase?.QueryMeasurementEventStatus();
            this.BinningSubsystem.StopElapsedStopwatch();
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

    /// <summary> Gets or sets the device. </summary>
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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( BinningView ).SplitWords()}" );
        }

        this._limit1View.AssignDevice( value );
        this._limit2View.AssignDevice( value );
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

    #region " binning "

    /// <summary> Gets or sets the <see cref="BinningSubsystemBase"/>. </summary>
    /// <value> The <see cref="BinningSubsystemBase"/>. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public BinningSubsystemBase? BinningSubsystem { get; private set; }

    /// <summary> Binds the <see cref="BinningSubsystemBase"/> subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( BinningSubsystemBase? subsystem )
    {
        if ( this.BinningSubsystem is not null )
        {
            this.BindSubsystem( false, this.BinningSubsystem );
            this.BinningSubsystem = null;
        }

        this.BinningSubsystem = subsystem;
        if ( this.BinningSubsystem is not null )
            this.BindSubsystem( true, this.BinningSubsystem );

        this._limit1View.BindSubsystem( subsystem, 1 );
        this._limit2View.BindSubsystem( subsystem, 2 );
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, BinningSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.BinningSubsystemBasePropertyChanged;
            // must Not read setting when biding because the instrument may be locked Or in a trigger mode
            // The bound values should be sent when binding Or when applying property change.
            // ReadSettings( subsystem );
            // ReadLimitTestState( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
            subsystem.PropertyChanged -= this.BinningSubsystemBasePropertyChanged;
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( BinningSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.BinningStrobeEnabled ) );
        this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.LimitsFailed ) );
        this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.PassSource ) );
    }

    /// <summary> Tests perform limit. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void PerformLimitTest( BinningSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        subsystem.Immediate();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> read limit test result. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadLimitTestState( BinningSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryLimitsFailed();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( BinningSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryPassSource();
        _ = subsystem.QueryBinningStrobeEnabled();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handle the Binning subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( BinningSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( BinningSubsystemBase.BinningStrobeEnabled ):
                {
                    this._BinningStrobeToggleButton.CheckState = subsystem.BinningStrobeEnabled.ToCheckState();
                    break;
                }

            case nameof( BinningSubsystemBase.PassSource ):
                {
                    if ( subsystem.PassSource.HasValue )
                        this._passBitPatternNumeric.Value = subsystem.PassSource.Value;
                    break;
                }

            case nameof( BinningSubsystemBase.LimitsFailed ):
                {
                    this._limitFailedButton.CheckState = subsystem.LimitsFailed.ToCheckState();
                    break;
                }

            case nameof( BinningSubsystemBase.BinningStrobeDuration ):
                {
                    this._binningStrobeDurationNumeric.Value = ( decimal ) (subsystem.BinningStrobeDuration.Ticks / ( double ) TimeSpan.TicksPerMillisecond);
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Binning subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void BinningSubsystemBasePropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( BinningSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.BinningSubsystemBasePropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.BinningSubsystemBasePropertyChanged ), [sender, e] );

            else if ( sender is BinningSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " digital output subsystem "

    /// <summary> Gets or sets the digital output subsystem. </summary>
    /// <value> The digital output subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public DigitalOutputSubsystemBase? DigitalOutputSubsystem { get; private set; }

    /// <summary> Bind ArmLayer2 subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( DigitalOutputSubsystemBase subsystem )
    {
        if ( this.DigitalOutputSubsystem is not null )
            this.DigitalOutputSubsystem = null;

        this.DigitalOutputSubsystem = subsystem;
        if ( this.DigitalOutputSubsystem is not null )
            this.BindSubsystem( true, this.DigitalOutputSubsystem );
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public static void ReadSettings( DigitalOutputSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryDigitalActiveLevels( [1, 2, 3, 4] );
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Applies the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void ApplySettings( DigitalOutputSubsystemBase subsystem )
    {
        ApplySettings( subsystem, this._digitalOutputActiveHighMenuItem.Checked ? DigitalActiveLevels.High : DigitalActiveLevels.Low );
    }

    /// <summary> Applies the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    /// <param name="polarity">  The polarity. </param>
    public static void ApplySettings( DigitalOutputSubsystemBase subsystem, DigitalActiveLevels polarity )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.ApplyDigitalActiveLevel( [1, 2, 3, 4], polarity );
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, DigitalOutputSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.DigitalOutputSubsystemBasePropertyChanged;
            // must Not read setting when biding because the instrument may be locked Or in a trigger mode
            // The bound values should be sent when binding Or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
            subsystem.PropertyChanged -= this.DigitalOutputSubsystemBasePropertyChanged;
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( DigitalOutputSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( this.DigitalOutputSubsystem.CurrentDigitalActiveLevel ) );
    }

    /// <summary> Handle the Calculate subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( DigitalOutputSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( DigitalOutputSubsystemBase.CommonDigitalActiveLevel ):
                {
                    this._digitalOutputActiveHighMenuItem.CheckState = (subsystem.CommonDigitalActiveLevel.HasValue
                                                                        ? subsystem.CommonDigitalActiveLevel.Value == DigitalActiveLevels.High
                                                                        : new bool?()).ToCheckState();
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Digital Output subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void DigitalOutputSubsystemBasePropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( DigitalOutputSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.DigitalOutputSubsystemBasePropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.DigitalOutputSubsystemBasePropertyChanged ), [sender, e] );

            else if ( sender is DigitalOutputSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " control event handlers "

    /// <summary> Pass bit pattern numeric button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void PassBitPatternNumericButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"Pass {(button.Checked ? "0x" : "0d")}";
            this._passBitPatternNumeric.NumericUpDown.Hexadecimal = button.Checked;
        }
    }

    /// <summary> Binning strobe toggle button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void BinningStrobeToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"Strobe: {button.CheckState.ToCheckStateCaption( "On", "Off", "?" )}";
        }
    }

    /// <summary> Limit failed button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void LimitFailedButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = button.CheckState.ToCheckStateCaption( "Fail", "Pass", "P/F ?" );
        }
    }

    /// <summary> Applies the settings menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplySettingsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents ) return;
        this.ApplySettings();
    }

    /// <summary> Reads settings menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadSettingsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents ) return;
        this.ReadSettings();
    }

    /// <summary> Reads state menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadStateMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents ) return;
        this.ReadLimitTestState();
    }

    /// <summary> Preform limit test menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void PreformLimitTestMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents ) return;
        this.PerformLimitTest();
    }

    #endregion

}
