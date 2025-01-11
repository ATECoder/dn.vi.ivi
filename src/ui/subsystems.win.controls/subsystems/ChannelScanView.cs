using System;
using System.ComponentModel;
using System.Linq;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A Channel Scan subsystem user interface. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class ChannelScanView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public ChannelScanView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new <see cref="ChannelScanView"/> </summary>
    /// <returns> A <see cref="ChannelScanView"/>. </returns>
    public static ChannelScanView Create()
    {
        ChannelScanView? view = null;
        try
        {
            view = new ChannelScanView();
            return view;
        }
        catch
        {
            view?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="System.Windows.Forms.Control" />
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

    /// <summary> Gets or sets the closed channels. </summary>
    /// <value> The closed channels. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string ClosedChannels
    {
        get => this._channelView.ClosedChannels;
        set => this._channelView.ClosedChannels = value;
    }

    /// <summary> Gets or sets the scan list function. </summary>
    /// <value> The scan list function. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string ScanListFunction
    {
        get => this.ScanView?.ScanListFunction ?? string.Empty;
        set
        {
            if ( this.ScanView is not null ) this.ScanView.ScanListFunction = value;
        }
    }

    /// <summary> Gets or sets the scan list of closed channels. </summary>
    /// <value> The closed channels. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string InternalScanList
    {
        get => this.ScanView?.InternalScanList ?? string.Empty;
        set
        {
            if ( this.ScanView is not null )
                this.ScanView.InternalScanList = value;
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
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( ChannelScanView ).SplitWords()}" );
        }

        this._channelView.AssignDevice( value );
        this._channelView.SubsystemName = "Route";
        if ( this.ScanView is not null )
        {
            this.ScanView.AssignDevice( value );
            this.ScanView.SubsystemName = "Route";
        }
    }

    /// <summary> Gets the system subsystem. </summary>
    /// <value> The system subsystem. </value>
    private SystemSubsystemBase? SystemSubsystem { get; set; }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected virtual void BindSubsystem( SystemSubsystemBase subsystem )
    {
        if ( this.SystemSubsystem is not null )
        {
            this.SystemSubsystem = null;
        }

        this.SystemSubsystem = subsystem;
        this._channelView.BindSubsystem( subsystem );
        this.ScanView?.BindSubsystem( subsystem );
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected virtual void BindSubsystem( RouteSubsystemBase? subsystem )
    {
        if ( this.SystemSubsystem is null ) return;
        if ( subsystem is null )
        {
            this._channelView.BindSubsystem( subsystem );
            this.ScanView?.BindSubsystem( subsystem );
        }
        else if ( this.SystemSubsystem.SupportsScanCardOption && this.SystemSubsystem.InstalledScanCards.Any() )
        {
            this._channelView.BindSubsystem( subsystem );
            this.ScanView?.BindSubsystem( subsystem );
        }
        else if ( !this.SystemSubsystem.SupportsScanCardOption )
            this._infoTextBox.Text = "No scan card";
    }

    /// <summary> Bind subsystem. </summary>
    /// <param name="subsystem"> The subsystem. </param>
    protected virtual void BindSubsystem( SenseSubsystemBase subsystem )
    {
        if ( subsystem is null )
            this.ScanView?.BindSubsystem( subsystem );

        else if ( this.SystemSubsystem is not null && this.SystemSubsystem.SupportsScanCardOption && this.SystemSubsystem.InstalledScanCards.Any() )
            this.ScanView?.BindSubsystem( subsystem );
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

    #region " control event handlers "

    /// <summary> Reads settings menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadSettingsMenuItem_Click( object? sender, EventArgs e )
    {
        this._channelView.ReadSettings();
        this.ScanView?.ReadSettings();
    }

    #endregion

    #region " controls "

    /// <summary> Gets the scan view. </summary>
    /// <value> The scan view. </value>
    protected ScanView? ScanView { get; private set; }

    #endregion
}
