using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;
using cc.isr.WinControls.CheckStateExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A Limit Subsystem view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class LimitView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public LimitView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._upperLimitBitPatternNumeric.NumericUpDown.Minimum = 1m;
        this._upperLimitBitPatternNumeric.NumericUpDown.Maximum = 63m;
        this._upperLimitBitPatternNumeric.NumericUpDown.Value = 16m;
        this._lowerLimitBitPatternNumeric.NumericUpDown.Minimum = 1m;
        this._lowerLimitBitPatternNumeric.NumericUpDown.Maximum = 63m;
        this._lowerLimitBitPatternNumeric.NumericUpDown.Value = 48m;
        this._upperLimitDecimalsNumeric.NumericUpDown.Minimum = 0m;
        this._upperLimitDecimalsNumeric.NumericUpDown.Maximum = 10m;
        this._upperLimitDecimalsNumeric.NumericUpDown.DecimalPlaces = 0;
        this._upperLimitDecimalsNumeric.NumericUpDown.Value = 3m;
        this._upperLimitNumeric.NumericUpDown.Minimum = 0m;
        this._upperLimitNumeric.NumericUpDown.Maximum = 5000000m;
        this._upperLimitNumeric.NumericUpDown.DecimalPlaces = 3;
        this._upperLimitNumeric.NumericUpDown.Value = 9m;
        this._lowerLimitNumeric.NumericUpDown.Minimum = 0m;
        this._lowerLimitNumeric.NumericUpDown.Maximum = 5000000m;
        this._lowerLimitNumeric.NumericUpDown.DecimalPlaces = 3;
        this._lowerLimitNumeric.NumericUpDown.Value = 90m;
        this._lowerLimitDecimalsNumeric.NumericUpDown.Minimum = 0m;
        this._lowerLimitDecimalsNumeric.NumericUpDown.Maximum = 10m;
        this._lowerLimitDecimalsNumeric.NumericUpDown.DecimalPlaces = 0;
        this._lowerLimitDecimalsNumeric.NumericUpDown.Value = 3m;
        this._upperLimitNumeric.NumericUpDown.DecimalPlaces = ( int ) this._upperLimitDecimalsNumeric.Value;
        this._lowerLimitNumeric.NumericUpDown.DecimalPlaces = ( int ) this._lowerLimitDecimalsNumeric.Value;
    }

    /// <summary> Creates a new LimitView. </summary>
    /// <returns> A LimitView. </returns>
    public static LimitView Create()
    {
        LimitView? view = null;
        try
        {
            view = new LimitView();
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

    /// <summary> The limit number. </summary>
    private int _limitNumber;

    /// <summary> Gets or sets the limit number. </summary>
    /// <value> The limit number. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public int LimitNumber
    {
        get => this._limitNumber;
        set
        {
            if ( value != this.LimitNumber )
            {
                this._limitNumber = value;
                this._subsystemSplitButton.Text = $"Limit{value}";
            }
        }
    }

    /// <summary> Applies the settings onto the instrument. </summary>
    public void ApplySettings()
    {
        if ( this.Device is null || this.BinningSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} applying Binning subsystem {this._subsystemSplitButton.Text} state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.BinningSubsystem.StartElapsedStopwatch();
            if ( this.LimitNumber == 1 )
            {
                _ = this.BinningSubsystem.ApplyLimit1AutoClear( this._autoClearToggleButton.CheckState == CheckState.Checked );
                _ = this.BinningSubsystem.ApplyLimit1Enabled( this._limitEnabledToggleButton.CheckState == CheckState.Checked );
                _ = this.BinningSubsystem.ApplyLimit1LowerLevel( ( double ) this._lowerLimitNumeric.Value );
                _ = this.BinningSubsystem.ApplyLimit1LowerSource( ( int ) this._lowerLimitBitPatternNumeric.Value );
                _ = this.BinningSubsystem.ApplyLimit1UpperLevel( ( double ) this._upperLimitNumeric.Value );
                _ = this.BinningSubsystem.ApplyLimit1UpperSource( ( int ) this._lowerLimitBitPatternNumeric.Value );
            }
            else
            {
                _ = this.BinningSubsystem.ApplyLimit2AutoClear( this._autoClearToggleButton.CheckState == CheckState.Checked );
                _ = this.BinningSubsystem.ApplyLimit2Enabled( this._limitEnabledToggleButton.CheckState == CheckState.Checked );
                _ = this.BinningSubsystem.ApplyLimit2LowerLevel( ( double ) this._lowerLimitNumeric.Value );
                _ = this.BinningSubsystem.ApplyLimit2LowerSource( ( int ) this._lowerLimitBitPatternNumeric.Value );
                _ = this.BinningSubsystem.ApplyLimit2UpperLevel( ( double ) this._upperLimitNumeric.Value );
                _ = this.BinningSubsystem.ApplyLimit2UpperSource( ( int ) this._lowerLimitBitPatternNumeric.Value );
            }

            this.BinningSubsystem.StopElapsedStopwatch();
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

    /// <summary> Reads settings from the instrument. </summary>
    public void ReadSettings()
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
            activity = $"{this.Device.ResourceNameCaption} reading Binning subsystem {this._subsystemSplitButton.Text} state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            ReadSettings( this.BinningSubsystem, this.LimitNumber );
            this.ApplyPropertyChanged( this.BinningSubsystem );
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
            this.BinningSubsystem.StartElapsedStopwatch();
            this.BinningSubsystem.Immediate();
            _ = this.BinningSubsystem.QueryLimitsFailed();
            _ = this.BinningSubsystem.QueryLimit1Failed();
            _ = this.BinningSubsystem.QueryLimit2Failed();
            this.BinningSubsystem.StopElapsedStopwatch();
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
            _ = this.LimitNumber == 1 ? this.BinningSubsystem.QueryLimit1Failed() : this.BinningSubsystem.QueryLimit2Failed();

            _ = this.Device.StatusSubsystemBase?.QueryMeasurementEventStatus();
            this.BinningSubsystem.StopElapsedStopwatch();
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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( LimitView ).SplitWords()}" );
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

    #region " binning "

    /// <summary> Gets or sets the binning subsystem. </summary>
    /// <value> The binning subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public BinningSubsystemBase? BinningSubsystem { get; private set; }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem">   The subsystem. </param>
    /// <param name="limitNumber"> The limit number. </param>
    public void BindSubsystem( BinningSubsystemBase? subsystem, int limitNumber )
    {
        if ( this.BinningSubsystem is not null )
        {
            this.BindSubsystem( false, this.BinningSubsystem );
            this.BinningSubsystem = null;
        }

        this.LimitNumber = limitNumber;
        this.BinningSubsystem = subsystem;
        if ( this.BinningSubsystem is not null )
        {
            this.BindSubsystem( true, this.BinningSubsystem );
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, BinningSubsystemBase? subsystem )
    {
        if ( subsystem is null ) return;
        if ( add )
        {
            subsystem.PropertyChanged += this.BinningSubsystemPropertyChanged;
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( this.BinningSubsystem, this.LimitNumber );
            // this.ReadLimitTestState();
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.BinningSubsystemPropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( BinningSubsystemBase subsystem )
    {
        switch ( this.LimitNumber )
        {
            case 1:
                {
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit1AutoClear ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit1Enabled ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit1Failed ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit1LowerLevel ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit1LowerSource ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit1UpperLevel ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit1UpperSource ) );
                    break;
                }

            case 2:
                {
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit2AutoClear ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit2Enabled ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit2Failed ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit2LowerLevel ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit2LowerSource ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit2UpperLevel ) );
                    this.HandlePropertyChanged( subsystem, nameof( BinningSubsystemBase.Limit2UpperSource ) );
                    break;
                }

            default:
                {
                    throw new InvalidOperationException( $"Limit number {this.LimitNumber} must be either 1 or 2" );
                }
        }
    }

    /// <summary> Reads settings from the instrument. </summary>
    /// <param name="subsystem">   The subsystem. </param>
    /// <param name="limitNumber"> The limit number. </param>
    private static void ReadSettings( BinningSubsystemBase subsystem, int limitNumber )
    {
        subsystem.StartElapsedStopwatch();
        if ( limitNumber == 1 )
        {
            _ = subsystem.QueryLimit1AutoClear();
            _ = subsystem.QueryLimit1Enabled();
            _ = subsystem.QueryLimit1LowerLevel();
            _ = subsystem.QueryLimit1LowerSource();
            _ = subsystem.QueryLimit1UpperLevel();
            _ = subsystem.QueryLimit1UpperSource();
        }
        else
        {
            _ = subsystem.QueryLimit2AutoClear();
            _ = subsystem.QueryLimit2Enabled();
            _ = subsystem.QueryLimit2LowerLevel();
            _ = subsystem.QueryLimit2LowerSource();
            _ = subsystem.QueryLimit2UpperLevel();
            _ = subsystem.QueryLimit2UpperSource();
        }

        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handle the Calculate subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( BinningSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        if ( this.LimitNumber == 1 )
        {
            switch ( propertyName ?? string.Empty )
            {
                case nameof( BinningSubsystemBase.Limit1AutoClear ):
                    {
                        this._autoClearToggleButton.CheckState = subsystem.Limit1AutoClear.ToCheckState();
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit1Enabled ):
                    {
                        this._limitEnabledToggleButton.CheckState = subsystem.Limit1Enabled.ToCheckState();
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit1Failed ):
                    {
                        this._limitFailedButton.CheckState = subsystem.Limit1Failed.ToCheckState();
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit1LowerLevel ):
                    {
                        if ( subsystem.Limit1LowerLevel.HasValue )
                            this._lowerLimitNumeric.Value = ( decimal ) subsystem.Limit1LowerLevel.Value;
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit1LowerSource ):
                    {
                        if ( subsystem.Limit1LowerSource.HasValue )
                            this._lowerLimitBitPatternNumeric.Value = subsystem.Limit1LowerSource.Value;
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit1UpperLevel ):
                    {
                        if ( subsystem.Limit1UpperLevel.HasValue )
                            this._upperLimitNumeric.Value = ( decimal ) subsystem.Limit1UpperLevel.Value;
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit1UpperSource ):
                    {
                        if ( subsystem.Limit1UpperSource.HasValue )
                            this._upperLimitBitPatternNumeric.Value = subsystem.Limit1UpperSource.Value;
                        break;
                    }

                default:
                    break;
            }
        }
        else
        {
            switch ( propertyName ?? string.Empty )
            {
                case nameof( BinningSubsystemBase.Limit2AutoClear ):
                    {
                        this._autoClearToggleButton.CheckState = subsystem.Limit2AutoClear.ToCheckState();
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit2Enabled ):
                    {
                        this._limitEnabledToggleButton.CheckState = subsystem.Limit2Enabled.ToCheckState();
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit2Failed ):
                    {
                        this._limitFailedButton.CheckState = subsystem.Limit2Failed.ToCheckState();
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit2LowerLevel ):
                    {
                        if ( subsystem.Limit2LowerLevel.HasValue )
                            this._lowerLimitNumeric.Value = ( decimal ) subsystem.Limit2LowerLevel.Value;
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit2LowerSource ):
                    {
                        if ( subsystem.Limit2LowerSource.HasValue )
                            this._lowerLimitBitPatternNumeric.Value = subsystem.Limit2LowerSource.Value;
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit2UpperLevel ):
                    {
                        if ( subsystem.Limit2UpperLevel.HasValue )
                            this._upperLimitNumeric.Value = ( decimal ) subsystem.Limit2UpperLevel.Value;
                        break;
                    }

                case nameof( BinningSubsystemBase.Limit2UpperSource ):
                    {
                        if ( subsystem.Limit2UpperSource.HasValue )
                            this._upperLimitBitPatternNumeric.Value = subsystem.Limit2UpperSource.Value;
                        break;
                    }

                default:
                    break;
            }
        }
    }

    /// <summary> Binning subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void BinningSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( BinningSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.BinningSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.BinningSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is BinningSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " control event handlers: calculate "

    /// <summary> Automatic clear button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AutoClearButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"Auto Clear: {button.CheckState.ToCheckStateCaption( "On", "Off", "?" )}";
        }
    }

    /// <summary> Limit enabled toggle button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void LimitEnabledToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = button.CheckState.ToCheckStateCaption( "Enabled", "Disabled", "Enabled ?" );
        }
    }

    /// <summary> Limit failed toggle button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void LimitFailedToggleButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = button.CheckState.ToCheckStateCaption( "Fail", "Pass", "P/F ?" );
        }
    }

    /// <summary> Lower bit pattern numeric button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void LowerBitPatternNumericButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            this._lowerLimitBitPatternNumeric.NumericUpDown.Hexadecimal = button.Checked;
            button.Text = $"Source {(button.Checked ? "0x" : "0d")}";
        }
    }

    /// <summary> Upper limit bit pattern numeric button check state changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void UpperLimitBitPatternNumericButton_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripButton button )
        {
            button.Text = $"Source {(button.Checked ? "0x" : "0d")}";
            this._upperLimitBitPatternNumeric.NumericUpDown.Hexadecimal = button.Checked;
        }
    }

    /// <summary> Upper limit decimals numeric value changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void UpperLimitDecimalsNumeric_ValueChanged( object? sender, EventArgs e )
    {
        if ( this._upperLimitNumeric is not null )
        {
            this._upperLimitNumeric.NumericUpDown.DecimalPlaces = ( int ) this._upperLimitDecimalsNumeric.Value;
        }
    }

    /// <summary> Lower limit decimals numeric value changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void LowerLimitDecimalsNumeric_ValueChanged( object? sender, EventArgs e )
    {
        if ( this._lowerLimitNumeric is not null )
        {
            this._lowerLimitNumeric.NumericUpDown.DecimalPlaces = ( int ) this._lowerLimitDecimalsNumeric.Value;
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

    /// <summary> Performs the limit strip menu item click action. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void PerformLimitStripMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents ) return;
        this.PerformLimitTest();
    }

    /// <summary> Reads limit test menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadLimitTestMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents ) return;
        this.ReadLimitTestState();
    }

    #endregion


}
