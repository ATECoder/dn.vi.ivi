using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using cc.isr.VI.DeviceWinControls.Views;

namespace cc.isr.VI.DeviceWinControls;

/// <summary> Provides a user interface for a <see cref="VisaSession"/> </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2018-12-24, 6.0.6932.x"> Create based on the read and write control panel </para></remarks>
[DisplayName( "VISA Tree View" )]
[Description( "Tree View interface for VISA Sessions" )]
[System.Drawing.ToolboxBitmap( typeof( VisaView ) )]
public partial class VisaTreeView : cc.isr.WinControls.ModelViewLoggerBase, IVisaView
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public VisaTreeView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the designer.
        this.InitializeComponent();
        this._treePanel.Dock = DockStyle.Fill;
        this._treePanel.SplitterDistance = 110;
        this._treePanel.ClearNodes();
        this.InsertViewControl( 0, Messages_Node_Name, Messages_Node_Name, this._traceMessagesBox );
        this.InsertViewControl( 0, Session_Node_Name, Session_Node_Name, this._sessionPanel );

        this.TextBoxTextWriter = new( this._traceMessagesBox )
        {
            ContainerTreeNode = this._treePanel.GetNode( Messages_Node_Name ),
            HandleCreatedCheckEnabled = false,
            TabCaption = "Log",
            CaptionFormat = "{0} " + Convert.ToChar( 0x1C2 ),
            ResetCount = 1000,
            PresetCount = 500,
            TraceLevel = TraceEventType.Verbose
        };
        cc.isr.Tracing.TracingPlatform.Instance.AddTraceEventWriter( this.TextBoxTextWriter );

        this._treePanel.Enabled = true;
        this.InitializingComponents = false;
    }

    /// <summary> Constructor. </summary>
    /// <param name="visaSessionBase"> The visa session. </param>
    public VisaTreeView( VisaSessionBase? visaSessionBase ) : this() =>
        // assigns the visa session.
        this.BindVisaSessionBaseThis( visaSessionBase );

    /// <summary>
    /// Releases the unmanaged resources used by the cc.isr.VI.DeviceWinControls.VisaView and optionally releases
    /// the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
                this.InitializingComponents = true;
                if ( this.TextBoxTextWriter != null )
                    cc.isr.Tracing.TracingPlatform.Instance.RemoveTraceEventWriter( this.TextBoxTextWriter );
                // make sure the session is unbound in case the form is closed without closing the session.
                this.BindVisaSessionBaseThis( null );
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

    #region " form events "

    /// <summary> Handles the <see cref="System.Windows.Forms.UserControl.Load" /> event. </summary>
    /// <param name="e"> An <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnLoad( EventArgs e )
    {
        try
        {
            this.SelectNavigatorTreeViewFirstNode();
        }
        finally
        {
            base.OnLoad( e );
        }
    }

    #endregion

    #region " visa session base (device base) "

    /// <summary> Gets the device. </summary>
    /// <value> The device. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? VisaSessionBase { get; private set; }

    /// <summary> Assigns a <see cref="VisaSessionBase">Visa session</see> </summary>
    /// <param name="visaSessionBase"> The assigned visa session or nothing to release the session. </param>
    public virtual void BindVisaSessionBase( VisaSessionBase? visaSessionBase )
    {
        this.BindVisaSessionBaseThis( visaSessionBase );
    }

    /// <summary> Assigns a <see cref="VisaSessionBase">Visa session</see> </summary>
    /// <remarks>
    /// The synchronization context is captured as part of the property change and other event
    /// handlers and is no longer needed here.
    /// </remarks>
    /// <param name="visaSessionBase"> The assigned visa session or nothing to release the session. </param>
    private void BindVisaSessionBaseThis( VisaSessionBase? visaSessionBase )
    {
        if ( this.VisaSessionBase is not null )
        {
            // this might remove listeners belonging to the assigned talker.
            // If Me.VisaSessionBase.Talker IsNot Nothing Then Me.VisaSessionBase.Talker.RemoveListeners()
            this.VisaSessionBase.Opening -= this.HandleDeviceOpening;
            this.VisaSessionBase.Opened -= this.HandleDeviceOpened;
            this.VisaSessionBase.Closing -= this.HandleDeviceClosing;
            this.VisaSessionBase.Closed -= this.HandleDeviceClosed;
            this._selectorOpener.AssignSelectorViewModel( null );
            this._selectorOpener.AssignOpenerViewModel( null );
            this.BindVisaSession( false, this.VisaSessionBase );
            this.BindSession( false, this.VisaSessionBase?.Session );
            this.VisaSessionBase = null;
        }

        this.VisaSessionBase = visaSessionBase;
        if ( this.VisaSessionBase is not null )
        {
            if ( this.VisaSessionBase.Session is null )
                throw new InvalidOperationException( $"The {nameof( VisaTreeView.VisaSessionBase )}.{nameof( VisaTreeView.VisaSessionBase.Session )} must be set." );
            if ( !this.VisaSessionBase.Session.TimingSettings.Exists )
                throw new InvalidOperationException( $"The {nameof( VisaTreeView.VisaSessionBase )}.{nameof( VisaTreeView.VisaSessionBase.Session )}.{nameof( VisaTreeView.VisaSessionBase.Session.TimingSettings )} must exists; Timing Settings were not read." );
            if ( !this.VisaSessionBase.Session.RegistersBitmasksSettings.Exists )
                throw new InvalidOperationException( $"The {nameof( VisaTreeView.VisaSessionBase )}.{nameof( VisaTreeView.VisaSessionBase.Session )}.{nameof( VisaTreeView.VisaSessionBase.Session.RegistersBitmasksSettings )} must exists; Registers Bitmasks Settings were not read." );

            this.VisaSessionBase.Opening += this.HandleDeviceOpening;
            this.VisaSessionBase.Opened += this.HandleDeviceOpened;
            this.VisaSessionBase.Closing += this.HandleDeviceClosing;
            this.VisaSessionBase.Closed += this.HandleDeviceClosed;
            this._selectorOpener.AssignSelectorViewModel( this.VisaSessionBase.SessionFactory );
            this._selectorOpener.AssignOpenerViewModel( visaSessionBase );
            this.BindVisaSession( true, this.VisaSessionBase );
            this.BindSession( true, this.VisaSessionBase?.Session );
            if ( this.VisaSessionBase?.Session is not null )
            {
                if ( !string.IsNullOrWhiteSpace( this.VisaSessionBase.CandidateResourceName ) )
                    this.VisaSessionBase.Session.CandidateResourceName = this.VisaSessionBase.CandidateResourceName;
                if ( !string.IsNullOrWhiteSpace( this.VisaSessionBase.CandidateResourceModel ) )
                    this.VisaSessionBase.Session.CandidateResourceModel = this.VisaSessionBase.CandidateResourceModel;
                if ( this.VisaSessionBase.IsDeviceOpen )
                {
                    this.VisaSessionBase.Session.OpenResourceName = this.VisaSessionBase.OpenResourceName;
                    this.VisaSessionBase.Session.OpenResourceModel = this.VisaSessionBase.OpenResourceModel;
                }
            }

        }

        this._sessionView.BindVisaSessionBase( visaSessionBase );
        this.DisplayView?.BindVisaSessionBase( visaSessionBase );
        this.StatusView?.BindVisaSessionBase( visaSessionBase );
    }

    /// <summary> Bind visa session. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    /// <param name="add">             True to add. </param>
    /// <param name="visaSessionBase"> The visa session. </param>
    private void BindVisaSession( bool add, VisaSessionBase visaSessionBase )
    {
        foreach ( Control t in this._treePanel.NodeControls.Values )
        {
            if ( !(ReferenceEquals( t, this._traceMessagesBox ) || ReferenceEquals( t, this._sessionPanel )) )
            {
                _ = this.AddRemoveBinding( t, add, nameof( this.Enabled ), visaSessionBase, nameof( visaSessionBase.IsSessionOpen ) );
            }
        }
    }

    /// <summary> Bind session. </summary>
    /// <param name="add">       True to add. </param>
    /// <param name="viewModel"> The view model. </param>
    private void BindSession( bool add, Pith.SessionBase? viewModel )
    {
        if ( viewModel is not null )
            _ = this.AddRemoveBinding( this._statusPromptLabel, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.StatusPrompt ) );
    }

    #endregion

    #region " device event handlers "

    /// <summary> Gets the resource name caption. </summary>
    /// <value> The resource name caption. </value>
    private string ResourceNameCaption { get; set; } = string.Empty;

    /// <summary> Executes the device Closing action. </summary>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected virtual void OnDeviceClosing( CancelEventArgs? e )
    {
    }

    /// <summary> Handles the device Closing. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void HandleDeviceClosing( object? sender, CancelEventArgs? e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.ResourceNameCaption} UI handling device closing";
            this.OnDeviceClosing( e );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Executes the device Closed action. </summary>
    protected virtual void OnDeviceClosed()
    {
    }

    /// <summary> Handles the device Close. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void HandleDeviceClosed( object? sender, EventArgs? e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.ResourceNameCaption} UI handling device closed";
            this.OnDeviceClosed();
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Executes the device Opening action. </summary>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected virtual void OnDeviceOpening( CancelEventArgs? e )
    {
        this.ResourceNameCaption = this.VisaSessionBase?.ResourceNameCaption ?? string.Empty;
    }

    /// <summary> Handles the device Opening. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void HandleDeviceOpening( object? sender, CancelEventArgs? e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.VisaSessionBase?.ResourceNameCaption} UI handling device Opening";
            this.OnDeviceOpening( e );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Executes the device opened action. </summary>
    protected virtual void OnDeviceOpened()
    {
        this.ResourceNameCaption = this.VisaSessionBase?.ResourceNameCaption ?? string.Empty;
    }

    /// <summary> Handles the device opened. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void HandleDeviceOpened( object? sender, EventArgs? e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.VisaSessionBase?.ResourceNameCaption} UI handling device opened";
            this.OnDeviceOpened();
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " customization "

    /// <summary> Gets the Display view. </summary>
    /// <value> The Display view. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public DisplayView? DisplayView { get; private set; }

    /// <summary> Gets the status view. </summary>
    /// <value> The status view. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public StatusView? StatusView { get; private set; }

    #endregion

    #region " navigation "

    /// <summary> Name of the session node. </summary>
    private const string Session_Node_Name = "Session";

    /// <summary> Name of the messages node. </summary>
    private const string Messages_Node_Name = "Messages";

    /// <summary> Gets the number of views. </summary>
    /// <value> The number of tabs. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public int ViewsCount => this._treePanel.NodeCount;

    /// <summary> Removes the node. </summary>
    /// <param name="nodeName"> Name of the node. </param>
    public void RemoveNode( string nodeName )
    {
        this._treePanel.RemoveNode( nodeName );
    }

    /// <summary> Adds (inserts) a View. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="view">        The view control. </param>
    /// <param name="viewIndex">   Zero-based index of the view. </param>
    /// <param name="viewName">    The name of the view. </param>
    /// <param name="viewCaption"> The caption of the view. </param>
    public void AddView( cc.isr.WinControls.ModelViewLoggerBase view, int viewIndex, string viewName, string viewCaption )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( view, nameof( view ) );
#else
        if ( view is null ) throw new ArgumentNullException( nameof( view ) );
#endif
        this.AddView( view as Control, viewIndex, viewName, viewCaption );
    }

    /// <summary> Adds (inserts) a View. </summary>
    /// <param name="view"> The view control. </param>
    public void AddView( VisaViewControl view )
    {
        this.AddView( view.Control, view.Index, view.Name, view.Caption );
    }

    /// <summary> Adds (inserts) a View. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="view">        The view control. </param>
    /// <param name="viewIndex">   Zero-based index of the view. </param>
    /// <param name="viewName">    The name of the view. </param>
    /// <param name="viewCaption"> The caption of the view. </param>
    public void AddView( Control? view, int viewIndex, string viewName, string viewCaption )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( view, nameof( view ) );
#else
        if ( view is null ) throw new ArgumentNullException( nameof( view ) );
#endif
        this.InsertViewControl( viewIndex, viewName, viewCaption, view );
    }

    /// <summary> Adds (inserts) a View. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="index">   Zero-based index of the. </param>
    /// <param name="name">    Name of the node. </param>
    /// <param name="caption"> The node caption. </param>
    /// <param name="view">    The view control. </param>
    public void InsertViewControl( int index, string name, string caption, Control? view )
    {
#if NET5_0_OR_GREATER
        ArgumentNullException.ThrowIfNull( view, nameof( view ) );
#else
        if ( view is null ) throw new ArgumentNullException( nameof( view ) );
#endif
        view.Visible = false;
        _ = this._treePanel.InsertNode( index, name, caption, view );
    }

    /// <summary> Gets the last tree view node selected. </summary>
    /// <value> The last tree view node selected. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    protected string LastTreeViewNodeSelected => this._treePanel.LastNodeSelected is null ? string.Empty : this._treePanel.LastNodeSelected.Name;

    /// <summary> Select navigator tree view first node. </summary>
    public void SelectNavigatorTreeViewFirstNode()
    {
        this._treePanel.SelectNavigatorTreeViewFirstNode();
    }

    /// <summary> Selects the navigator tree view node. </summary>
    /// <param name="nodeName"> The node. </param>
    public void SelectNavigatorTreeViewNode( string nodeName )
    {
        this._treePanel.SelectNavigatorTreeViewNode( nodeName );
    }

    /// <summary> After node selected. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    /// <param name="e"> Tree view event information. </param>
    protected virtual void AfterNodeSelected( TreeViewEventArgs? e )
    {
    }

    /// <summary> Tree panel after node selected. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Tree view event information. </param>
    private void TreePanel_afterNodeSelected( object? sender, TreeViewEventArgs? e )
    {
        this.AfterNodeSelected( e );
    }

    #endregion

    #region " unit tests internals "

    /// <summary> Gets the number of internal resource names. </summary>
    /// <value> The number of internal resource names. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public int ResourceNamesCount => this._selectorOpener.ResourceNamesCount;

    /// <summary> Gets the name of the internal selected resource. </summary>
    /// <value> The name of the internal selected resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string SelectedResourceName => this._selectorOpener.SelectedResourceName;

    #endregion

    #region " proptected properties "

    /// <summary>   Gets or sets the trace alert container. </summary>
    /// <value> The trace alert container. </value>
    [CLSCompliant( false )]
    protected cc.isr.Tracing.WinForms.TraceAlertContainer? TraceAlertContainer { get; set; }

    /// <summary>   Gets or sets the text box text writer. </summary>
    /// <value> The text box text writer. </value>
    [CLSCompliant( false )]
    protected cc.isr.Tracing.WinForms.TextBoxTraceEventWriter? TextBoxTextWriter { get; set; }

    #endregion

    #region " Text Box Text Writer "

    /// <summary>   Gets or sets the display level for log and trace messages. </summary>
    /// <remarks>
    /// The maximum trace event type for displaying logged and trace events. Only messages with a
    /// message <see cref="System.Diagnostics.TraceEventType"/> level that is same or higher than
    /// this level are displayed.
    /// </remarks>
    /// <value> The message display level. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public TraceEventType MessageDisplayLevel
    {
        get => this.TextBoxTextWriter is null ? TraceEventType.Verbose : this.TextBoxTextWriter.TraceLevel;
        set
        {
            if ( this.TextBoxTextWriter is not null ) this.TextBoxTextWriter.TraceLevel = value;
        }
    }

    #endregion
}
