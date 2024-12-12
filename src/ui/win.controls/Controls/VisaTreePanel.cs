using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace cc.isr.VI.WinControls;

/// <summary> A tree panel for VISA Sessions </summary>
/// <remarks> (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para></remarks>
[DisplayName( "Visa Tree Panel" )]
[Description( "Tree Panel for VISA Session Base and Trace Messages" )]
[System.Drawing.ToolboxBitmap( typeof( VisaTreePanel ) )]
public partial class VisaTreePanel : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public VisaTreePanel() : base()
    {
        this.InitializingComponents = true;

        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this._layout.Dock = DockStyle.Fill;
        this._treePanel.Dock = DockStyle.Fill;
        this._treePanel.SplitterDistance = 120;
        this._treePanel.ClearNodes();
        _ = this._treePanel.AddNode( Messages_Node_Name, Messages_Node_Name, this.TraceMessagesBox ??
                throw new InvalidOperationException( $"{nameof( VisaTreePanel )}.{nameof( VisaTreePanel.TraceMessagesBox )} is null." ) );

        this.TextBoxTextWriter = new( this.TraceMessagesBox )
        {
            ContainerTreeNode = this._treePanel.GetNode( Messages_Node_Name ),
            HandleCreatedCheckEnabled = false,
            TabCaption = "Log",
            CaptionFormat = "{0} " + Convert.ToChar( 0x1C2 ),
            ResetCount = 1000,
            PresetCount = 500,
            TraceLevel = Properties.Settings.Instance.MessageDisplayLevel
        };
        cc.isr.Tracing.TracingPlatform.Instance.AddTraceEventWriter( this.TextBoxTextWriter );

        this._treePanel.Enabled = true;
        this.InitializingComponents = false;
    }

    /// <summary> Constructor. </summary>
    /// <remarks> Assigns the visa session. </remarks>
    /// <param name="visaSessionBase"> The visa session. </param>
    public VisaTreePanel( VisaSessionBase? visaSessionBase ) : this() => this.BindVisaSessionBaseThis( visaSessionBase );

    /// <summary> Creates a new <see cref="VisaTreePanel"/> </summary>
    /// <returns> A <see cref="VisaTreePanel"/>. </returns>
    public static VisaTreePanel Create()
    {
        VisaTreePanel? view = null;
        try
        {
            view = new VisaTreePanel();
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
                cc.isr.Tracing.TracingPlatform.Instance.RemoveTraceEventWriter( this.TextBoxTextWriter );

                // make sure the session is unbound in case the form is closed without closing the session.
                this.BindVisaSessionBase( null );

                this.components?.Dispose();
                this.components = null;
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " event handlers: control "

    /// <summary>
    /// Called upon receiving the <see cref="System.Windows.Forms.Form.Load" /> event.
    /// </summary>
    /// <param name="e"> An <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnLoad( EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            activity = $"Loading the driver console form";
            Trace.CorrelationManager.StartLogicalOperation( System.Reflection.MethodBase.GetCurrentMethod()?.Name ?? "OnLoad" );

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
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
        }
    }

    #endregion

    #region " visa session base (device base) "

    /// <summary> Gets or sets the device. </summary>
    /// <value> The device. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? VisaSessionBase { get; private set; }

    /// <summary> Bind visa session base. </summary>
    /// <param name="visaSessionBase"> True to show or False to hide the control. </param>
    private void BindVisaSessionBaseThis( VisaSessionBase? visaSessionBase )
    {
        if ( this.VisaSessionBase is not null )
        {
            this.VisaSessionBase.Opening -= this.DeviceOpening;
            this.VisaSessionBase.Opened -= this.DeviceOpened;
            this.VisaSessionBase.Closing -= this.DeviceClosing;
            this.VisaSessionBase.Closed -= this.DeviceClosed;
            this.VisaSessionBase.Initialized -= this.DeviceInitialized;
            this.VisaSessionBase.Initializing -= this.DeviceInitializing;
            if ( this.VisaSessionBase.SessionFactory is not null )
                this.VisaSessionBase.SessionFactory.PropertyChanged -= this.SessionFactoryPropertyChanged;
            this.VisaSessionBase = null;
        }

        this.VisaSessionBase = visaSessionBase;
        if ( this.VisaSessionBase is not null )
        {
            this.VisaSessionBase.Opening += this.DeviceOpening;
            this.VisaSessionBase.Opened += this.DeviceOpened;
            this.VisaSessionBase.Closing += this.DeviceClosing;
            this.VisaSessionBase.Closed += this.DeviceClosed;
            this.VisaSessionBase.Initialized += this.DeviceInitialized;
            this.VisaSessionBase.Initializing += this.DeviceInitializing;
            if ( this.VisaSessionBase.SessionFactory is not null )
                this.VisaSessionBase.SessionFactory.PropertyChanged += this.SessionFactoryPropertyChanged;
            // Me.VisaSessionBase.SessionFactory.CandidateResourceName = cc.isr.VI.Ttm.My.Settings.ResourceName
            if ( this.VisaSessionBase.IsSessionOpen )
                this.DeviceOpened( this.VisaSessionBase, EventArgs.Empty );
            else
                this.DeviceClosed( this.VisaSessionBase, EventArgs.Empty );

            this.HandlePropertyChanged( this.VisaSessionBase.SessionFactory, nameof( this.VisaSessionBase.SessionFactory.IsOpen ) );
        }
    }

    /// <summary> Assigns a device. </summary>
    /// <param name="visaSessionBase"> True to show or False to hide the control. </param>
    public virtual void BindVisaSessionBase( VisaSessionBase? visaSessionBase )
    {
        this.BindVisaSessionBaseThis( visaSessionBase );
    }

    /// <summary> Reads the status register and lets the session process the status byte. </summary>
    protected void ReadStatusRegister()
    {
        if ( this.VisaSessionBase is null ) throw new InvalidOperationException( $"{nameof( VisaTreePanel )}.{nameof( VisaTreePanel.VisaSessionBase )} is null." );
        if ( this.VisaSessionBase.Session is null ) throw new InvalidOperationException( $"{nameof( VisaTreePanel )}.{nameof( VisaTreePanel.VisaSessionBase )}.{nameof( VisaTreePanel.VisaSessionBase.Session )} is null." );
        string activity = $"{this.VisaSessionBase.ResourceNameCaption} reading service request";
        try
        {
            this.VisaSessionBase.Session.ApplyStatusByte( this.VisaSessionBase.Session.ReadStatusByte() );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " session factory "

    /// <summary> Name of the resource. </summary>
    private string? _resourceName;

    /// <summary> Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string? ResourceName
    {
        get => this._resourceName;
        set
        {
            if ( value is null || string.IsNullOrWhiteSpace( value ) ) value = string.Empty;
            if ( this.SetProperty( ref this._resourceName, value ) )
            {
                if ( this.VisaSessionBase is not null )
                    this.VisaSessionBase.CandidateResourceName = value;
            }

        }
    }

    private string? _resourceFilter;

    /// <summary> Gets or sets the Search Pattern of the resource. </summary>
    /// <value> The Search Pattern of the resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string? ResourceFilter
    {
        get => this._resourceFilter;
        set
        {
            if ( value is null || string.IsNullOrWhiteSpace( value ) ) value = string.Empty;
            if ( this.SetProperty( ref this._resourceFilter, value ) )
            {
                if ( this.VisaSessionBase is not null )
                    this.VisaSessionBase.ResourcesFilter = value;
            }

        }
    }

    /// <summary> Executes the session factory property changed action. </summary>
    /// <param name="sender">       Specifies the object where the call originated. </param>
    /// <param name="propertyName"> Name of the property. </param>
    protected virtual void HandlePropertyChanged( SessionFactory? sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( SessionFactory.ValidatedResourceName ):
                {
                    _ = sender.IsOpen
                        ? cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resource connected;. {sender.ValidatedResourceName}" )
                        : cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resource located;. {sender.ValidatedResourceName}" );

                    this.ResourceName = sender.ValidatedResourceName;
                    break;
                }

            case nameof( SessionFactory.CandidateResourceName ):
                {
                    if ( !sender.IsOpen )
                    {
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Candidate resource;. {sender.ValidatedResourceName}" );
                    }

                    this.ResourceName = sender.CandidateResourceName;
                    break;
                }

            case nameof( SessionFactory.ResourcesFilter ):
                {
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Session factory property changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void SessionFactoryPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
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
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceOpening( object? sender, CancelEventArgs e )
    {
    }

    /// <summary>
    /// Event handler. Called after the device opened and all subsystems were defined.
    /// </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceOpened( object? sender, EventArgs e )
    {
        if ( this.VisaSessionBase is null ) throw new InvalidOperationException( $"{nameof( ConnectorView )}.{nameof( ConnectorView.VisaSessionBase )} is null." );
        if ( this.VisaSessionBase.Session is null ) throw new InvalidOperationException( $"{nameof( ConnectorView )}.{nameof( ConnectorView.VisaSessionBase )}.{nameof( ConnectorView.VisaSessionBase.Session )} is null." );
        this.VisaSessionBase.LogSessionOpenAndEnabled();

        string activity = string.Empty;
        try
        {
            activity = $"Opened {this.ResourceName}";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
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
    /// <remarks> David, 2020-06-08. </remarks>
    /// <param name="resourceName">  The name of the resource. </param>
    /// <param name="resourceModel"> The model of the resource. </param>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public (bool Success, string Details) TryOpenDeviceSession( string resourceName, string resourceModel )
    {
        if ( this.VisaSessionBase is null ) throw new InvalidOperationException( $"{nameof( ConnectorView )}.{nameof( ConnectorView.VisaSessionBase )} is null." );

        string activity = $"opening {resourceName} VISA session";
        try
        {
            (bool success, string details) = this.VisaSessionBase.TryOpenSession( resourceName, resourceModel );
            return success ? (success, details) : (success, $"failed {activity};. {details}");
        }
        catch ( Exception ex )
        {
            return (false, $"Exception {activity};. {ex}");
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    #endregion

    #region " initalizing / initialized  "

    /// <summary> Device initializing. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Cancel event information. </param>
    protected virtual void DeviceInitializing( object? sender, CancelEventArgs e )
    {
    }

    /// <summary> Device initialized. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceInitialized( object? sender, EventArgs e )
    {
    }

    #endregion

    #region " closing / closed "

    /// <summary> Event handler. Called when device is closing. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceClosing( object? sender, CancelEventArgs e )
    {
        if ( this.VisaSessionBase is not null )
        {
            string activity = string.Empty;
            try
            {
                activity = $"Disconnecting from {this.ResourceName}";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.VisaSessionBase.Session?.DisableServiceRequestEventHandler();
            }
            catch ( Exception ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            }
        }
    }

    /// <summary> Event handler. Called when device is closed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    protected virtual void DeviceClosed( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        if ( this.VisaSessionBase is not null )
        {
            try
            {
                if ( this.VisaSessionBase.Session is not null && this.VisaSessionBase.Session.IsSessionOpen )
                {
                    activity = $"{this.VisaSessionBase.Session.ResourceNameCaption} closed but session is still open";
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{activity};. " );
                }
                else if ( this.VisaSessionBase.Session is not null && this.VisaSessionBase.Session.IsDeviceOpen )
                {
                    activity = $"{this.VisaSessionBase.Session.ResourceNameCaption} closed but emulated session is still open";
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{activity};. " );
                }
                else
                {
                    activity = $"Disconnected from {this.ResourceName}";
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                }
            }
            catch ( Exception ex )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            }
        }
        else
        {
            activity = $"Already disconnected; device disposed";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
        }
    }

    #endregion

    #region " layout "

    /// <summary> Refresh layout. </summary>
    private void RefreshLayout()
    {
        this._layout.RowStyles.Clear();
        _ = this._layout.RowStyles.Add( new RowStyle( SizeType.AutoSize ) );
        _ = this._layout.RowStyles.Add( new RowStyle( SizeType.Percent, 100f ) );
        _ = this._layout.RowStyles.Add( new RowStyle( SizeType.AutoSize ) );
        if ( this.HeaderControl is not null )
            this._layout.SetRow( this.HeaderControl, 0 );

        if ( this._treePanel is not null )
            this._layout.SetRow( this._treePanel, 1 );

        if ( this.FooterControl is not null )
            this._layout.SetRow( this.FooterControl, 0 );

        this._layout.Refresh();
    }

    /// <summary> Gets the header control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The header control. </value>
    private Control HeaderControl { get; set; } = new();

    /// <summary> Adds a header. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="headerControl"> The control. </param>
    public void AddHeader( Control headerControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( headerControl, nameof( headerControl ) );
#else
        if ( headerControl is null ) throw new ArgumentNullException( nameof( headerControl ) );
#endif

        this.HeaderControl = headerControl;
        this._layout.Controls.Add( headerControl, 1, 0 );
        headerControl.Dock = DockStyle.Top;
        this.RefreshLayout();
    }

    /// <summary> Gets the footer control. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The footer control. </value>
    private Control FooterControl { get; set; } = new();

    /// <summary> Adds a footer. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="footerControl"> The footer control. </param>
    public void AddFooter( Control footerControl )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( footerControl, nameof( footerControl ) );
#else
        if ( footerControl is null ) throw new ArgumentNullException( nameof( footerControl ) );
#endif

        this.FooterControl = footerControl;
        this._layout.Controls.Add( footerControl, 1, 2 );
        footerControl.Dock = DockStyle.Bottom;
        this.RefreshLayout();
    }

    /// <summary> Name of the messages node. </summary>
    private const string Messages_Node_Name = "Messages";

    /// <summary> Selects the navigator tree view node. </summary>
    /// <param name="nodeName"> The node. </param>
    public void SelectNavigatorTreeViewNode( string nodeName )
    {
        this._treePanel.SelectNavigatorTreeViewNode( nodeName );
    }

    /// <summary> Adds a node. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    /// <param name="nodeName">    The node. </param>
    /// <param name="nodeCaption"> The node caption. </param>
    /// <param name="nodeControl"> The node control. </param>
    /// <returns> A TreeNode. </returns>
    public TreeNode AddNode( string nodeName, string nodeCaption, Control nodeControl )
    {
        return this._treePanel.AddNode( nodeName, nodeCaption, nodeControl );
    }

    /// <summary> Inserts a node. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    /// <param name="index">       Zero-based index of the. </param>
    /// <param name="nodeName">    The node. </param>
    /// <param name="nodeCaption"> The node caption. </param>
    /// <param name="nodeControl"> The node control. </param>
    /// <returns> A TreeNode. </returns>
    public TreeNode InsertNode( int index, string nodeName, string nodeCaption, Control nodeControl )
    {
        return this._treePanel.InsertNode( index, nodeName, nodeCaption, nodeControl );
    }

    /// <summary> Removes the node described by nodeName. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    /// <param name="nodeName"> The node. </param>
    public void RemoveNode( string nodeName )
    {
        this._treePanel.RemoveNode( nodeName );
    }

    /// <summary> After node selected. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    /// <param name="e"> Tree view event information. </param>
    protected virtual void AfterNodeSelected( TreeViewEventArgs e )
    {
    }

    /// <summary> Tree panel after node selected. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Tree view event information. </param>
    private void TreePanel_afterNodeSelected( object? sender, TreeViewEventArgs e )
    {
        this.AfterNodeSelected( e );
    }

    #endregion

    #region " alert annunciator "

    /// <summary>   Adds an alert annunciator. </summary>
    /// <remarks>   David, 2021-12-08. </remarks>
    /// <param name="alertAnnunciator"> The alert annunciator. </param>
    /// <param name="alertText">        The alert text. </param>
    /// <param name="alertLevel">       The alert level. </param>
    protected void AddAlertAnnunciator( Control alertAnnunciator, string alertText, TraceEventType alertLevel )
    {
        this.TraceAlertContainer = new( alertAnnunciator, this.TraceMessagesBox );
        this.AddAlertAnnunciator( alertText, alertLevel );
    }

    /// <summary>   Adds an alert annunciator. </summary>
    /// <remarks>   David, 2021-12-08. </remarks>
    /// <param name="alertAnnunciator"> The alert annunciator. </param>
    /// <param name="alertText">        The alert text. </param>
    /// <param name="alertLevel">       The alert level. </param>
    protected void AddAlertAnnunciator( ToolStripItem alertAnnunciator, string alertText, TraceEventType alertLevel )
    {
        this.TraceAlertContainer = new( alertAnnunciator, this.TraceMessagesBox );
        this.AddAlertAnnunciator( alertText, alertLevel );
    }

    /// <summary>   Adds an alert annunciator. </summary>
    /// <remarks>   David, 2021-12-08. </remarks>
    /// <param name="alertText">    The alert text. </param>
    /// <param name="alertLevel">   The alert level. </param>
    private void AddAlertAnnunciator( string alertText, TraceEventType alertLevel )
    {
        if ( this.TraceAlertContainer is null ) throw new InvalidOperationException( $"{nameof( VisaTreePanel )}.{nameof( VisaTreePanel.TraceAlertContainer )} is null." );
        this.TraceAlertContainer.AlertAnnunciatorText = alertText;
        this.TraceAlertContainer.AlertLevel = alertLevel;
        this.TraceAlertContainer.AlertSoundEnabled = true;
        cc.isr.Tracing.TracingPlatform.Instance.AddTraceEventWriter( this.TraceAlertContainer );
    }

    /// <summary>   Removes the alert annunciator. </summary>
    /// <remarks>   David, 2021-12-08. </remarks>
    protected void RemoveAlertAnnunciator()
    {
        if ( this.TraceAlertContainer is not null )
            cc.isr.Tracing.TracingPlatform.Instance.RemoveTraceEventWriter( this.TraceAlertContainer );
    }

    #endregion

    #region " protected controls "

    /// <summary>   Gets or sets the trace alert container. </summary>
    /// <value> The trace alert container. </value>
    [CLSCompliant( false )]
    protected cc.isr.Tracing.WinForms.TraceAlertContainer? TraceAlertContainer { get; set; }

    /// <summary>   Gets or sets the text box text writer. </summary>
    /// <value> The text box text writer. </value>
    [CLSCompliant( false )]
    protected cc.isr.Tracing.WinForms.TextBoxTraceEventWriter TextBoxTextWriter { get; set; }

    /// <summary> Gets the trace messages box. </summary>
    /// <value> The trace messages box. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    protected cc.isr.Logging.TraceLog.WinForms.MessagesBox TraceMessagesBox { get; private set; }

    /// <summary> Gets the status strip. </summary>
    /// <value> The status strip. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    protected StatusStrip StatusStrip { get; private set; } = new();

    /// <summary> Gets the status label. </summary>
    /// <value> The status label. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    protected ToolStripStatusLabel StatusLabel { get; private set; } = new();

    /// <summary> Gets the progress bar. </summary>
    /// <value> The progress bar. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    protected cc.isr.WinControls.StatusStripCustomProgressBar ProgressBar { get; private set; } = new();

    /// <summary> Gets or sets the progress percent. </summary>
    /// <value> The progress percent. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public int ProgressPercent
    {
        get => this.ProgressBar.Value;
        set
        {
            if ( value != this.ProgressPercent )
            {
                this.ProgressBar.Value = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

}
