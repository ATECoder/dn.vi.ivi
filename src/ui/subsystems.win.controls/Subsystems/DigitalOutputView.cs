using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A Digital Output Subsystem user interface. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class DigitalOutputView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public DigitalOutputView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._digitalOutputLine1View.LineIdentity = Strobe_Line_Identity;
        this._digitalOutputLine1View.LineName = Strobe_Line_Identity;
        this._digitalOutputLine1View.LineNumber = Properties.Settings.Instance.StrobeLineNumber;
        this._digitalOutputLine1View.PulseWidth = TimeSpan.FromMilliseconds( Properties.Settings.Instance.StrobeDuration );
        this._digitalOutputLine1View.ActiveLevel = DigitalActiveLevels.Low;
        this._digitalOutputLine2View.LineIdentity = Bin_Line_Identity;
        this._digitalOutputLine2View.LineName = Bin_Line_Identity;
        this._digitalOutputLine2View.LineNumber = Properties.Settings.Instance.BinLineNumber;
        this._digitalOutputLine2View.PulseWidth = TimeSpan.FromMilliseconds( Properties.Settings.Instance.BinDuration );
        this._digitalOutputLine2View.ActiveLevel = DigitalActiveLevels.Low;
        this._digitalOutputLine3View.LineIdentity = Third_Line_Identity;
        this._digitalOutputLine3View.LineName = "Line";
    }

    /// <summary> Creates a new <see cref="TriggerView"/> </summary>
    /// <returns> A <see cref="TriggerView"/>. </returns>
    public static TriggerView Create()
    {
        TriggerView? view = null;
        try
        {
            view = new TriggerView();
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

    /// <summary> Adds a menu item. </summary>
    /// <param name="item"> The item. </param>
    public void AddMenuItem( ToolStripMenuItem item )
    {
        _ = this._subsystemSplitButton.DropDownItems.Add( item );
    }

    /// <summary> Applies the trigger plan settings. </summary>
    public virtual void ApplySettings()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} applying digital output 1 subsystem instrument settings";
            this._digitalOutputLine1View.ApplySettings();
            activity = $"{this.Device?.ResourceNameCaption} applying digital output 2 subsystem instrument settings";
            this._digitalOutputLine2View.ApplySettings();
            activity = $"{this.Device?.ResourceNameCaption} applying digital output 3 Subsystem instrument settings";
            this._digitalOutputLine3View.ApplySettings();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Reads instrument settings. </summary>
    public virtual void ReadSettings()
    {
        if ( this.InitializingComponents ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} reading digital output 1 subsystem instrument settings";
            this._digitalOutputLine1View.ReadSettings();
            activity = $"{this.Device?.ResourceNameCaption} reading digital output 2 subsystem instrument settings";
            this._digitalOutputLine2View.ReadSettings();
            activity = $"{this.Device?.ResourceNameCaption} reading digital output 3 Subsystem instrument settings";
            this._digitalOutputLine3View.ReadSettings();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Gets or sets the strobe line number. </summary>
    /// <value> The strobe line number. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public int StrobeLineNumber
    {
        get => this._digitalOutputLine1View.LineNumber;
        set => this._digitalOutputLine1View.LineNumber = value;
    }

    /// <summary> Gets or sets the strobe pulse width. </summary>
    /// <value> The strobe pulse width. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public TimeSpan StrobeDuration
    {
        get => this._digitalOutputLine1View.PulseWidth;
        set => this._digitalOutputLine1View.PulseWidth = value;
    }

    /// <summary> Gets or sets the bin line number. </summary>
    /// <value> The bin line number. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public int BinLineNumber
    {
        get => this._digitalOutputLine1View.LineNumber;
        set => this._digitalOutputLine1View.LineNumber = value;
    }

    /// <summary> Gets or sets the bin pulse width. </summary>
    /// <value> The bin pulse width. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public TimeSpan BinDuration
    {
        get => this._digitalOutputLine1View.PulseWidth;
        set => this._digitalOutputLine1View.PulseWidth = value;
    }

    /// <summary> Outputs a strobe pulse. </summary>
    public virtual void Strobe()
    {
        if ( this.InitializingComponents || this.Device is null || this._digitalOutputLine3View is null || this._digitalOutputLine3View.DigitalOutputSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing execution state";
            this.Device.ClearExecutionState();
            activity = $"{this.Device.ResourceNameCaption} initiating trigger plan";
            this._digitalOutputLine3View.DigitalOutputSubsystem.StartElapsedStopwatch();
            this._digitalOutputLine3View.DigitalOutputSubsystem.Strobe( this.StrobeLineNumber, this.StrobeDuration, this.BinLineNumber, this.BinDuration );
            this._digitalOutputLine3View.DigitalOutputSubsystem.StopElapsedStopwatch();
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemSplitButton, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
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
        this._digitalOutputLine1View.AssignDevice( value );
        this._digitalOutputLine2View.AssignDevice( value );
        this._digitalOutputLine3View.AssignDevice( value );
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem">   The subsystem. </param>
    protected virtual void BindSubsystem( DigitalOutputSubsystemBase subsystem )
    {
        int lineNumber = 0;
        TimeSpan pulseWidth = TimeSpan.Zero;
        DigitalActiveLevels activeLevel = DigitalActiveLevels.None;
        DigitalOutputLineView[] views = [this._digitalOutputLine1View, this._digitalOutputLine2View, this._digitalOutputLine3View];
        foreach ( DigitalOutputLineView digitalLineView in views )
        {
            switch ( digitalLineView.LineIdentity ?? "" )
            {
                case Strobe_Line_Identity:
                    {
                        lineNumber = Properties.Settings.Instance.StrobeLineNumber;
                        pulseWidth = TimeSpan.FromMilliseconds( Properties.Settings.Instance.StrobeDuration );
                        activeLevel = DigitalActiveLevels.Low;
                        break;
                    }

                case Bin_Line_Identity:
                    {
                        lineNumber = Properties.Settings.Instance.BinLineNumber;
                        pulseWidth = TimeSpan.FromMilliseconds( Properties.Settings.Instance.BinDuration );
                        activeLevel = DigitalActiveLevels.Low;
                        break;
                    }

                case Third_Line_Identity:
                    {
                        lineNumber = 2;
                        pulseWidth = TimeSpan.FromMilliseconds( 10d );
                        activeLevel = DigitalActiveLevels.Low;
                        break;
                    }

                default:
                    break;
            }

            digitalLineView.BindSubsystem( subsystem, lineNumber );
            if ( subsystem is null )
            {
                digitalLineView.PropertyChanged -= this.DigitalOutputLineViewPropertyChanged;
            }
            else
            {
                digitalLineView.PropertyChanged += this.DigitalOutputLineViewPropertyChanged;
                digitalLineView.LineNumber = lineNumber;
                digitalLineView.PulseWidth = pulseWidth;
                digitalLineView.ActiveLevel = activeLevel;
                this.HandlePropertyChanged( digitalLineView, nameof( DigitalOutputLineView.LineNumber ) );
            }
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

    #region " digital output line view "

    /// <summary> The strobe line identity. </summary>
    private const string Strobe_Line_Identity = "Strobe";

    /// <summary> The bin line identity. </summary>
    private const string Bin_Line_Identity = "Bin";
    private const string Third_Line_Identity = "3";

    /// <summary> Handle the Digital Output Line view property changed event. </summary>
    /// <remarks> David, 2020-11-13. </remarks>
    /// <param name="view">         The view. </param>
    /// <param name="propertyName"> Name of the property. </param>
    protected virtual void HandlePropertyChanged( DigitalOutputLineView view, string? propertyName )
    {
        if ( view is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( DigitalOutputLineView.LineNumber ):
                {
                    switch ( view.LineIdentity ?? "" )
                    {
                        case Strobe_Line_Identity:
                            {
                                Properties.Settings.Instance.StrobeLineNumber = view.LineNumber;
                                break;
                            }

                        case Bin_Line_Identity:
                            {
                                Properties.Settings.Instance.BinLineNumber = view.LineNumber;
                                break;
                            }

                        case Third_Line_Identity:
                            {
                                break;
                            }

                        default:
                            break;
                    }

                    break;
                }

            case nameof( DigitalOutputLineView.PulseWidth ):
                {
                    switch ( view.LineIdentity ?? "" )
                    {
                        case Strobe_Line_Identity:
                            {
                                Properties.Settings.Instance.StrobeDuration = ( int ) view.PulseWidth.TotalMilliseconds;
                                break;
                            }

                        case Bin_Line_Identity:
                            {
                                Properties.Settings.Instance.BinDuration = ( int ) view.PulseWidth.TotalMilliseconds;
                                break;
                            }

                        case Third_Line_Identity:
                            {
                                break;
                            }

                        default:
                            break;
                    }

                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Digital output line view property changed. </summary>
    /// <remarks> David, 2020-11-13. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void DigitalOutputLineViewPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( DigitalOutputLineView )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.DigitalOutputLineViewPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.DigitalOutputLineViewPropertyChanged ), [sender, e] );

            else if ( sender is DigitalOutputLineView digitalLineView )
                this.HandlePropertyChanged( digitalLineView, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " control event handlers "

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

    /// <summary> Initiate click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void InitiateButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.Strobe();
    }

    #endregion
}
