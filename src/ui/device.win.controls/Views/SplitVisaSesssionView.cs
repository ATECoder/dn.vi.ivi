using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Json.AppSettings.Services;
using cc.isr.Json.AppSettings.ViewModels;
using cc.isr.Json.AppSettings.WinForms;

namespace cc.isr.VI.DeviceWinControls.Views;

/// <summary> A visa session view. </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-04-24 </para>
/// </remarks>
public class SplitVisaSessionView : WinControls.VisaTreePanel
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public SplitVisaSessionView() : base()
    {
        this.AssignTopHeader( new DisplayView() );
        this.AssignVisaSessionStatusView( new SessionStatusView() );
    }

    /// <summary> Creates a new <see cref="SplitVisaSessionView"/> </summary>
    /// <returns> A <see cref="SplitVisaSessionView"/>. </returns>
    public static new SplitVisaSessionView Create()
    {
        SplitVisaSessionView? view = null;
        try
        {
            view = new SplitVisaSessionView();
            return view;
        }
        catch
        {
            view?.Dispose();
            throw;
        }
    }

    #endregion

    #region " visa session base (device base) "

    /// <summary> Binds the visa session base (device base) to its controls. </summary>
    /// <param name="visaSessionBase"> The visa session base (device base) view model. </param>
    public override void BindVisaSessionBase( VisaSessionBase? visaSessionBase )
    {
        base.BindVisaSessionBase( visaSessionBase );
        this.TopHeader?.BindVisaSessionBase( visaSessionBase );
        this.SessionStatusView?.BindVisaSessionBase( visaSessionBase );
        if ( this.SessionStatusView != null && this.SessionStatusView.StatusView is not null )
            this.SessionStatusView.StatusView.UserInterfaceSettings = Properties.Settings.Scribe;
    }

    #endregion

    #region " device events "

    /// <summary>
    /// Event handler. Called upon device opening so as to instantiated all subsystems.
    /// </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected override void DeviceOpening( object? sender, CancelEventArgs e )
    {
        base.DeviceOpening( sender, e );
        _ = SessionLogger.Instance.LogVerbose( $"Opening access to {this.ResourceName};. " );
    }

    /// <summary>
    /// Event handler. Called after the device opened and all subsystems were defined.
    /// </summary>
    /// <param name="sender"> <see cref="object" /> instance of this
    /// <see cref="Control" /> </param>
    /// <param name="e">      Event information. </param>
    protected override void DeviceOpened( object? sender, EventArgs e )
    {
        base.DeviceOpened( sender, e );
        string activity = string.Empty;
        try
        {
            activity = "displaying top header resource name";
            _ = SessionLogger.Instance.LogVerbose( $"{activity};. " );
        }
        catch ( Exception ex )
        {
            _ = SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Device initializing. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Cancel event information. </param>
    protected override void DeviceInitializing( object? sender, CancelEventArgs e )
    {
        base.DeviceInitializing( sender, e );
    }

    /// <summary> Device initialized. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected override void DeviceInitialized( object? sender, EventArgs e )
    {
        base.DeviceInitialized( sender, e );
    }

    /// <summary> Event handler. Called when device is closing. </summary>
    /// <param name="sender"> <see cref="object" /> instance of this
    /// <see cref="Control" />
    /// </param>
    /// <param name="e">      Event information. </param>
    protected override void DeviceClosing( object? sender, CancelEventArgs e )
    {
        base.DeviceClosing( sender, e );
        if ( this.VisaSessionBase is not null )
        {
            string activity = string.Empty;
            try
            {
            }
            catch ( Exception ex )
            {
                _ = SessionLogger.Instance.LogException( ex, activity );
            }
        }
    }

    /// <summary> Event handler. Called when device is closed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected override void DeviceClosed( object? sender, EventArgs e )
    {
        base.DeviceClosed( sender, e );
        if ( this.VisaSessionBase is not null )
        {
            string activity = string.Empty;
            try
            {
            }
            catch ( Exception ex )
            {
                _ = SessionLogger.Instance.LogException( ex, activity );
            }
        }
    }

    #endregion

    #region " top header "

    /// <summary> Gets or sets the top header. </summary>
    /// <value> The top header. </value>
    private DisplayView? TopHeader { get; set; }

    /// <summary> Assign top header. </summary>
    /// <param name="topHeader"> The top header. </param>
    private void AssignTopHeader( DisplayView topHeader )
    {
        this.TopHeader = topHeader;
        this.AddHeader( topHeader );
    }

    #endregion

    #region " visa session status connector view "

    /// <summary> Gets or sets the session status view. </summary>
    /// <value> The session status view. </value>
    private SessionStatusView? SessionStatusView { get; set; }

    /// <summary> Gets or sets the name of the visa session status node. </summary>
    /// <value> The name of the visa session status node. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string VisaSessionStatusNodeName { get; private set; } = "Session";

    /// <summary> Assign visa session status view. </summary>
    /// <param name="sessionStatusView"> The session status view. </param>
    private void AssignVisaSessionStatusView( SessionStatusView sessionStatusView )
    {
        this.SessionStatusView = sessionStatusView;
        if ( this.TopHeader != null )
            _ = this.AddNode( this.VisaSessionStatusNodeName, this.VisaSessionStatusNodeName, this.TopHeader );
    }

    #endregion

    #region " settings "

    /// <summary>   Opens the settings editor. </summary>
    /// <remarks>   David, 2021-12-08. </remarks>
    /// <returns>   A System.Windows.Forms.DialogResult. </returns>
    public static DialogResult OpenSettingsEditor()
    {
        Form form = new JsonSettingsEditorForm( "Device UI Settings Editor",
            new AppSettingsEditorViewModel( Properties.Settings.Scribe, SimpleServiceProvider.GetInstance() ) );
        return form.ShowDialog();
    }

    #endregion


}
