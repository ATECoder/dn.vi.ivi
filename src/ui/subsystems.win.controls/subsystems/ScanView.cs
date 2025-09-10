using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> An Route Subsystem Scan view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class ScanView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public ScanView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._subsystemName = "Route";
    }

    /// <summary> Creates a new ScanView. </summary>
    /// <returns> A ScanView. </returns>
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

    /// <summary> Adds a menu item. </summary>
    /// <param name="item"> The item. </param>
    public void AddMenuItem( ToolStripMenuItem item )
    {
        _ = this._subsystemSplitButton.DropDownItems.Add( item );
    }

    /// <summary> Name of the subsystem. </summary>
    private string _subsystemName;

    /// <summary> Gets or sets the name of the subsystem. </summary>
    /// <value> The name of the subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string SubsystemName
    {
        get => this._subsystemName;
        set
        {
            if ( !string.Equals( value, this.SubsystemName, StringComparison.Ordinal ) )
            {
                this._subsystemName = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the closed channels. </summary>
    /// <value> The closed channels. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string ClosedChannels
    {
        get;
        set
        {
            if ( !string.Equals( value, this.ClosedChannels, StringComparison.Ordinal ) )
            {
                field = value;
                this._internalScanListLabel.Text = $"Closed: {(string.IsNullOrEmpty( value ) ? "?" : value)}";
            }
        }
    } = string.Empty;

    /// <summary> Gets or sets the scan list function. </summary>
    /// <value> The scan list function. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string ScanListFunction
    {
        get => this._scanListFunctionLabel.Text ?? string.Empty;
        set
        {
            if ( !string.Equals( value, this.ScanListFunction, StringComparison.Ordinal ) )
            {
                if ( string.IsNullOrEmpty( value ) )
                {
                    this._scanListFunctionLabel.Text = "?";
                }
                else
                {
                    this._scanListFunctionLabel.Text = value;
                    this.AddScanList( value );
                }

                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the scan list of closed channels. </summary>
    /// <value> The closed channels. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string InternalScanList
    {
        get => this._internalScanListLabel.Text ?? string.Empty;
        set
        {
            if ( !string.Equals( value, this.InternalScanList, StringComparison.Ordinal ) )
            {
                if ( string.IsNullOrEmpty( value ) )
                {
                    this._internalScanListLabel.Text = "?";
                }
                else
                {
                    this._internalScanListLabel.Text = value;
                    this.AddScanList( value );
                }

                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Displays a scan list described by value and adds it tot he list of candidate scan lists.
    /// </summary>
    /// <param name="value"> The value. </param>
    public void AddScanList( string value )
    {
        if ( this.Visible && !string.IsNullOrEmpty( value ) )
        {
            // check if we are asking for a new scan list
            if ( this._candidateScanListComboBox.FindString( value ) < 0 )
            {
                // if we have a new string, add it to the scan list
                _ = this._candidateScanListComboBox.Items.Add( value );
            }
        }
    }

    /// <summary> Applies the internal scan list. </summary>
    public void ApplyInternalScanList()
    {
        if ( this.InitializingComponents || this.Device is null || this.RouteSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} applying {this.SubsystemName} settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.RouteSubsystem.StartElapsedStopwatch();
            _ = this.RouteSubsystem.ApplyScanList( this._candidateScanListComboBox.Text );
            this.RouteSubsystem.StopElapsedStopwatch();
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

    /// <summary> Applies the internal scan function list. </summary>
    public void ApplyInternalScanFunctionList()
    {
        if ( this.InitializingComponents || this.Device is null || this.RouteSubsystem is null || this.SenseSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} applying {this.SubsystemName} settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.RouteSubsystem.StartElapsedStopwatch();
            _ = this.RouteSubsystem.WriteScanListFunction( this._candidateScanListComboBox.Text, this.SenseSubsystem.FunctionMode.GetValueOrDefault( SenseFunctionModes.ResistanceFourWire ), this.SenseSubsystem.FunctionModeReadWrites );
            this.RouteSubsystem.StopElapsedStopwatch();
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

    /// <summary> Select internal scan list. </summary>
    public void SelectInternalScanList()
    {
        if ( this.InitializingComponents || this.Device is null || this.RouteSubsystem is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} clearing exception state";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Device.ClearExecutionState();
            this.Device.Session?.EnableServiceRequestOnOperationCompletion();
            activity = $"{this.Device.ResourceNameCaption} applying {this.SubsystemName} settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.RouteSubsystem.StartElapsedStopwatch();
            _ = this.RouteSubsystem.ApplySelectedScanListType( "INT" );
            this.RouteSubsystem.StopElapsedStopwatch();
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

    /// <summary> Releases the internal scan list. </summary>
    public void ReleaseInternalScanList()
    {
        if ( this.InitializingComponents || this.Device is null || this.RouteSubsystem is null ) return;
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
            this.RouteSubsystem.StartElapsedStopwatch();
            _ = this.RouteSubsystem.ApplySelectedScanListType( "NONE" );
            this.RouteSubsystem.StopElapsedStopwatch();
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
    public void ReadSettings()
    {
        if ( this.InitializingComponents || this.Device is null || this.RouteSubsystem is null || this.SystemSubsystem is null ) return;
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
            activity = $"{this.Device.ResourceNameCaption} reading system subsystem settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            ReadSettings( this.SystemSubsystem );
            this.ApplyPropertyChanged( this.SystemSubsystem );
            activity = $"{this.Device.ResourceNameCaption} reading {this.SubsystemName} settings";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            ReadSettings( this.RouteSubsystem );
            this.ApplyPropertyChanged( this.RouteSubsystem );
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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( ScanView ).SplitWords()}" );
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
    public void BindSubsystem( RouteSubsystemBase? subsystem )
    {
        if ( this.RouteSubsystem is not null )
        {
            this.BindSubsystem( false, this.RouteSubsystem );
            this.RouteSubsystem = null;
        }

        this.RouteSubsystem = subsystem;
        if ( this.RouteSubsystem is not null )
        {
            this.BindSubsystem( true, this.RouteSubsystem );
        }
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="subsystem"> The subsystem. </param>
    private void BindSubsystem( bool add, RouteSubsystemBase subsystem )
    {
        if ( add )
        {
            subsystem.PropertyChanged += this.RouteSubsystemBasePropertyChanged;
            // must not read setting when biding because the instrument may be locked or in a trigger mode
            // The bound values should be sent when binding or when applying property change.
            // ReadSettings( subsystem );
            this.ApplyPropertyChanged( subsystem );
        }
        else
        {
            subsystem.PropertyChanged -= this.RouteSubsystemBasePropertyChanged;
        }
    }

    /// <summary> Applies the property changed described by subsystem. </summary>
    /// <remarks> David, 2020-04-04. </remarks>
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( RouteSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( RouteSubsystemBase.ClosedChannel ) );
        this.HandlePropertyChanged( subsystem, nameof( RouteSubsystemBase.ScanList ) );
        this.HandlePropertyChanged( subsystem, nameof( RouteSubsystemBase.ScanListFunction ) );
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( RouteSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryClosedChannel();
        _ = subsystem.QueryScanList();
        _ = subsystem.QueryScanListFunction();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handles the ROUTE subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( RouteSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( RouteSubsystemBase.ClosedChannel ):
                {
                    this.ClosedChannels = subsystem.ClosedChannel ?? string.Empty;
                    break;
                }

            case nameof( RouteSubsystemBase.ScanList ):
                {
                    this.InternalScanList = subsystem.ScanList ?? string.Empty;
                    break;
                }

            case nameof( RouteSubsystemBase.ScanListFunction ):
                {
                    this.ScanListFunction = subsystem.ScanListFunction ?? string.Empty;
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> ROUTE subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void RouteSubsystemBasePropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        string activity = $"handling {nameof( this.RouteSubsystem )}.{e.PropertyName} change";
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.RouteSubsystemBasePropertyChanged ), [sender, e] );

            else if ( this._subsystemToolStrip.InvokeRequired )
                // Because ToolStripItems derive directly from Component instead of from Control, their containing ToolStrip's invoke should be used
                _ = this._subsystemToolStrip.Invoke( new Action<object, PropertyChangedEventArgs>( this.RouteSubsystemBasePropertyChanged ), [sender, e] );

            else if ( sender is RouteSubsystemBase s )
                this.HandlePropertyChanged( s, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " sense "

    /// <summary> Gets or sets the Sense subsystem. </summary>
    /// <value> The Sense subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public SenseSubsystemBase? SenseSubsystem { get; private set; }

    /// <summary> Bind Sense subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    public void BindSubsystem( SenseSubsystemBase? subsystem )
    {
        if ( this.SenseSubsystem is not null )
        {
            this.SenseSubsystem = null;
        }

        this.SenseSubsystem = subsystem;
        if ( this.SenseSubsystem is not null )
        {
        }
    }

    #endregion

    #region " system subsystem "

    /// <summary> Gets or sets the System subsystem. </summary>
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
            this.BindSubsystem( false, this.SystemSubsystem );
            this.SystemSubsystem = null;
        }

        this.SystemSubsystem = subsystem;
        if ( this.SystemSubsystem is not null )
        {
            this.BindSubsystem( true, this.SystemSubsystem );
        }
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
        this.HandlePropertyChanged( subsystem, nameof( SystemSubsystemBase.InstalledScanCards ) );
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( SystemSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryInstalledScanCards();
        subsystem.StopElapsedStopwatch();
    }

    /// <summary> Handles the System subsystem property changed event. </summary>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( SystemSubsystemBase subsystem, string? propertyName )
    {
        if ( subsystem is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( SystemSubsystemBase.InstalledScanCards ):
                {
                    // this control is disabled if the system supports scan cards and none was found
                    this.Enabled = subsystem.InstalledScanCards.Any() || !subsystem.SupportsScanCardOption;
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> System subsystem property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
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

    #region " control events "

    /// <summary> Reads settings menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadSettingsMenuItem_Click( object? sender, EventArgs e )
    {
        this.ReadSettings();
    }

    /// <summary> Applies the internal scan function list menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplyInternalScanFunctionListMenuItem_Click( object? sender, EventArgs e )
    {
        this.ApplyInternalScanFunctionList();
    }

    /// <summary> Applies the internal scan list menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ApplyInternalScanListMenuItem_Click( object? sender, EventArgs e )
    {
        this.ApplyInternalScanList();
    }

    /// <summary> Releases the internal scan list menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReleaseInternalScanListMenuItem_Click( object? sender, EventArgs e )
    {
        this.ReleaseInternalScanList();
    }

    /// <summary> Select internal scan list menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void SelectInternalScanListMenuItem_Click( object? sender, EventArgs e )
    {
        this.SelectInternalScanList();
    }

    #endregion


}
