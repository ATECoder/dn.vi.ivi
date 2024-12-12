using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> An Route Subsystem channel view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class ChannelView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public ChannelView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this._subsystemName = "Route";
        this._channelNumberNumeric.NumericUpDown.Minimum = 1m;
        this._channelNumberNumeric.NumericUpDown.Maximum = 10m;
        this._channelNumberNumeric.NumericUpDown.Value = 1m;
        this._channelNumberNumeric.NumericUpDown.DecimalPlaces = 0;
    }

    /// <summary> Creates a new ChannelView. </summary>
    /// <returns> A ChannelView. </returns>
    public static ChannelView Create()
    {
        ChannelView? view = null;
        try
        {
            view = new ChannelView();
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

    /// <summary> The closed channels. </summary>
    private string _closedChannels = string.Empty;

    /// <summary> Gets or sets the closed channels. </summary>
    /// <value> The closed channels. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string ClosedChannels
    {
        get => this._closedChannels;
        set
        {
            if ( !string.Equals( value, this.ClosedChannels, StringComparison.Ordinal ) )
            {
                this._closedChannels = value;
                this._closedChannelLabel.Text = $"Closed: {(string.IsNullOrEmpty( value ) ? "?" : value)}";
            }
        }
    }

    /// <summary> Closes the channel. </summary>
    public void CloseChannel()
    {
        if ( this.Device is null || this.RouteSubsystem is null ) return;
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
            _ = this.RouteSubsystem.ApplyClosedChannel( $"(@{this._channelNumberNumeric.Value})", TimeSpan.FromSeconds( 1d ) );
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

    /// <summary> Opens the channel. </summary>
    public void OpenChannel()
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
            _ = this.RouteSubsystem.ApplyOpenChannel( $"(@{this._channelNumberNumeric.Value})", TimeSpan.FromSeconds( 1d ) );
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

    /// <summary> Opens the channels. </summary>
    public void OpenChannels()
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
            _ = this.RouteSubsystem.WriteOpenAll( TimeSpan.FromSeconds( 1d ) );
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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( ChannelView ).SplitWords()}" );
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
    /// <param name="subsystem"> The subsystem. </param>
    private void ApplyPropertyChanged( RouteSubsystemBase subsystem )
    {
        this.HandlePropertyChanged( subsystem, nameof( RouteSubsystemBase.ClosedChannel ) );
    }

    /// <summary> Reads the settings. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    private static void ReadSettings( RouteSubsystemBase subsystem )
    {
        subsystem.StartElapsedStopwatch();
        _ = subsystem.QueryClosedChannel();
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
            this.BindSubsystem( true, this.SystemSubsystem );
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
            subsystem.PropertyChanged -= this.SystemSubsystemPropertyChanged;
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

    /// <summary> Opens channels button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void OpenChannelsButton_Click( object? sender, EventArgs e )
    {
        this.OpenChannels();
    }

    /// <summary> Opens channel button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void OpenChannelButton_Click( object? sender, EventArgs e )
    {
        this.OpenChannel();
    }

    /// <summary> Closes channel button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void CloseChannelButton_Click( object? sender, EventArgs e )
    {
        this.CloseChannel();
    }

    /// <summary> Closes channel menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void CloseChannelMenuItem_Click( object? sender, EventArgs e )
    {
        this.CloseChannel();
    }

    /// <summary> Opens channel menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void OpenChannelMenuItem_Click( object? sender, EventArgs e )
    {
        this.OpenChannel();
    }

    /// <summary> Opens all menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void OpenAllMenuItem_Click( object? sender, EventArgs e )
    {
        this.OpenChannels();
    }

    /// <summary> Reads settings menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadSettingsMenuItem_Click( object? sender, EventArgs e )
    {
        this.ReadSettings();
    }

    #endregion
}
