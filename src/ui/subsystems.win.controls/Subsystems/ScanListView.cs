using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A Scan list view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class ScanListView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public ScanListView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new <see cref="ScanView"/> </summary>
    /// <returns> A <see cref="ScanView"/>. </returns>
    public static ScanView Create()
    {
        ScanView? view = null;
        try
        {
            view = new ScanView();
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

    /// <summary> Gets or sets a list of scans. </summary>
    /// <value> A List of scans. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string ScanList
    {
        get => this._scanListLabel.Text ?? string.Empty;
        set
        {
            if ( !string.Equals( value, this.ScanList, StringComparison.Ordinal ) )
            {
                this._scanListLabel.Text = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets a list of candidate scans. </summary>
    /// <value> A List of candidate scans. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string CandidateScanList
    {
        get => this._candidateScanListComboBox.Text;
        set
        {
            if ( !string.Equals( value, this.CandidateScanList, StringComparison.Ordinal ) )
            {
                this.AddScanList( this._candidateScanListComboBox, value );
                this._candidateScanListComboBox.Text = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Adds a scan list 'value' to the combo box. </summary>
    /// <param name="comboBox"> The combo box. </param>
    /// <param name="value">    The value. </param>
    private void AddScanList( ToolStripComboBox comboBox, string value )
    {
        if ( this.Visible )
        {
            // check if we are asking for a new channel list
            if ( comboBox is not null && !string.IsNullOrEmpty( value ) && comboBox.FindStringExact( value ) < 0 )
            {
                // if we have a new string, add it to the channel list
                _ = comboBox.Items.Add( value );
            }
        }
    }

    /// <summary>
    /// Displays a scan list described by value and adds it tot he list of candidate scan lists.
    /// </summary>
    /// <param name="value"> The value. </param>
    public void DisplayScanList( string value )
    {
        this.AddScanList( this._candidateScanListComboBox, value );
        this.ScanList = value;
    }

    /// <summary> Applies the settings. </summary>
    public void ApplySettings()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} updating the scan list";
            _ = this.RouteSubsystem?.ApplyScanList( this._candidateScanListComboBox.Text );
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

    /// <summary> Reads the settings. </summary>
    private void ReadSettings()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} reading the scan list settings";
            if ( this.RouteSubsystem is not null )
            {
                ReadSettings( this.RouteSubsystem );
                this.ApplyPropertyChanged( this.RouteSubsystem );
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

    /// <summary> Saves to memory. </summary>
    public void SaveToMemory()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} checking candidate memory location {this._memoryLocationTextBox.Text}";
            if ( IsValidCandidateValue( this._memoryLocationTextBox.Text, 1, 10 ) )
            {
                activity = $"{this.Device.ResourceNameCaption} saving to memory";
                this.Device.ClearExecutionState();
                this.Device.Session?.EnableServiceRequestOnOperationCompletion();
                this.RouteSubsystem?.SaveChannelPattern( int.Parse( this._memoryLocationTextBox.Text, System.Globalization.CultureInfo.CurrentCulture ), TimeSpan.FromSeconds( 1d ) );
            }
            else
            {
                activity = $"Candidate memory location {this._memoryLocationTextBox.Text} is out of range [1,10]";
                _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Alert, activity );
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( activity );
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

    /// <summary> Opens all channels. </summary>
    public void OpenAllChannels()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} opening all channels";
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            _ = this.RouteSubsystem?.ApplyOpenAll( TimeSpan.FromSeconds( 1d ) );
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

    /// <summary> Closes scan list. </summary>
    public void CloseScanList()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} closing a channel";
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            _ = this.RouteSubsystem?.ApplyClosedChannels( this._candidateScanListComboBox.Text, TimeSpan.FromSeconds( 1d ) );
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

    /// <summary> Opens scan list. </summary>
    private void OpenScanList()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} opening a channel";
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            _ = this.RouteSubsystem?.ApplyOpenChannels( this._candidateScanListComboBox.Text, TimeSpan.FromSeconds( 1d ) );
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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( ScanListView ).SplitWords()}" );
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

    #region " route subsystem "

    /// <summary> Gets or sets the Route subsystem. </summary>
    /// <value> The Route subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public RouteSubsystemBase? RouteSubsystem { get; private set; }

    /// <summary> Bind Route subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( RouteSubsystemBase subsystem )
    {
        if ( this.RouteSubsystem is not null )
        {
            this.BindSubsystem( false, this.RouteSubsystem );
            this.RouteSubsystem = null;
        }

        this.RouteSubsystem = subsystem;
        if ( this.RouteSubsystem is not null )
            this.BindSubsystem( true, this.RouteSubsystem );
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, RouteSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.RouteSubsystemPropertyChanged;
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
            subsystem.PropertyChanged -= this.RouteSubsystemPropertyChanged;
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( RouteSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( RouteSubsystemBase.ScanList ) );
        this.HandlePropertyChanged( subsystem, nameof( RouteSubsystemBase.ClosedChannels ) );
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( RouteSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryScanList();
        _ = subsystem.QueryClosedChannels();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handle the Route subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( RouteSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( RouteSubsystemBase.ScanList ):
                {
                    this.DisplayScanList( subsystem.ScanList ?? string.Empty );
                    break;
                }

            case nameof( RouteSubsystemBase.ClosedChannels ):
                {
                    this._closedChannelsLabel.Text = subsystem.ClosedChannels;
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Route subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void RouteSubsystemPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( RouteSubsystemBase )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.RouteSubsystemPropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.RouteSubsystemPropertyChanged ), [sender, e] );

            else if ( sender is RouteSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " control event handlers "

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

    /// <summary> Query if 'candidateValue' is valid candidate value. </summary>
    /// <param name="candidateValue"> The candidate value. </param>
    /// <param name="minimum">        The minimum. </param>
    /// <param name="maximum">        The maximum. </param>
    /// <returns> True if valid candidate value, false if not. </returns>
    private static bool IsValidCandidateValue( string candidateValue, int minimum, int maximum )
    {
        return !string.IsNullOrWhiteSpace( candidateValue?.Trim() )
                        && int.TryParse( candidateValue, System.Globalization.NumberStyles.Integer,
                                         System.Globalization.CultureInfo.CurrentCulture, out int value ) && value >= minimum && value <= maximum;
    }

    /// <summary> Memory location text box validating. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Cancel event information. </param>
    private void MemoryLocationTextBox_Validating( object? sender, CancelEventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.Device?.ResourceNameCaption} validating memory location";
            e.Cancel = !IsValidCandidateValue( this._memoryLocationTextBox.Text, 1, 10 );
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
    }

    /// <summary> Saves to memory location menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void SaveToMemoryLocationMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.SaveToMemory();
    }

    /// <summary> Opens all menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void OpenAllMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.OpenAllChannels();
    }

    /// <summary> Opens menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void OpenMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.OpenScanList();
    }

    /// <summary> Closes menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void CloseMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.CloseScanList();
    }

    #endregion
}
