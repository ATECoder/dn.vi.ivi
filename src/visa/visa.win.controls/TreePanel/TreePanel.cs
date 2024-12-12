using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cc.isr.Visa.WinControls;

/// <summary> A tree split panel for holding multiple controls. </summary>
/// <remarks> David, 2020-09-02. </remarks>
public partial class TreePanel : SplitContainer
{
    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    public TreePanel() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.PreCollapseSplitterDistance = this.SplitterDistance;
        this.navigatorTreeView.BeforeSelect += new TreeViewCancelEventHandler( this.NavigatorTreeView_BeforeSelect );
        this.navigatorTreeView.AfterSelect += new TreeViewEventHandler( this.NavigatorTreeView_afterSelect );
        this.navigatorTreeView.MouseEnter += new System.EventHandler( this.NavigatorTreeView_MouseEnter );
        this.navigatorTreeView.MouseLeave += new System.EventHandler( this.NavigatorTreeView_MouseLeave );
        this.topToolStrip.MouseEnter += new System.EventHandler( this.NavigatorTreeView_MouseEnter );
        this.topToolStrip.MouseLeave += new System.EventHandler( this.NavigatorTreeView_MouseLeave );
        this.pinButton.CheckedChanged += new System.EventHandler( this.PinButton_CheckChanged );

        this.navigatorTreeView.Nodes.Clear();
        this.TreeNodes = new Dictionary<string, TreeNode>();
        this.NodeControls = new Dictionary<string, Control>();
        this.NodesVisited = [];
        this.InitializingComponents = false;
        this.navigatorTreeView.Name = "navigatorTreeView";
        Point clientPosition = this.navigatorTreeView.PointToClient( System.Windows.Forms.Control.MousePosition );
        this.MouseHoverState = clientPosition.X > 0 && clientPosition.X < this.navigatorTreeView.Width && clientPosition.Y > 0 && clientPosition.Y < this.navigatorTreeView.Height
                               ? HoverControlState.InsideTreeView : HoverControlState.OutsideTreeView;
        this.MouseHoverTimer = new( 250 ) { AutoReset = false };
        this.MouseHoverTimer.Elapsed += new System.Timers.ElapsedEventHandler( this.MouseHoverTimer_Elapsed );
    }

    #region " construction and cleanup "

    /// <summary> Gets the initializing components. </summary>
    /// <value> The initializing components. </value>
    protected bool InitializingComponents { get; set; }

    /// <summary> Creates a new <see cref="TreePanel"/> </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <returns> A <see cref="TreePanel"/>. </returns>
    public static TreePanel Create()
    {
        return new TreePanel();
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control" />
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources;
    /// <c>false</c> to release only unmanaged
    /// resources when called from the runtime
    /// finalize. </param>
    protected override void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;
        try
        {
            if ( disposing )
            {
                this.InitializingComponents = true;
                this.navigatorTreeView.BeforeSelect -= this.NavigatorTreeView_BeforeSelect;
                this.navigatorTreeView.AfterSelect -= this.NavigatorTreeView_afterSelect;
                this.navigatorTreeView.MouseEnter -= this.NavigatorTreeView_MouseEnter;
                this.navigatorTreeView.MouseLeave -= this.NavigatorTreeView_MouseLeave;
                this.topToolStrip.MouseEnter -= this.NavigatorTreeView_MouseEnter;
                this.topToolStrip.MouseLeave -= this.NavigatorTreeView_MouseLeave;
                this.navigatorTreeView.Dispose();
                this.pinButton.CheckedChanged -= this.PinButton_CheckChanged;
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " collapse control "

    /// <summary>   Gets or sets the title. </summary>
    /// <value> The title. </value>
    [Bindable( true )]
    [DefaultValue( typeof( string ), "Title" )]
    [Category( "Appearance" )]
    [Description( "Gets or sets the title the Tree Panel" )]
    public string Title
    {
        get => this.titleLabel?.Text ?? string.Empty;
        set => this.titleLabel.Text = value;
    }

    /// <summary>   Gets or sets the minimum size of the tree. </summary>
    /// <value> The minimum size of the tree. </value>
    [Bindable( true )]
    [DefaultValue( 16 )]
    [Category( "Appearance" )]
    [Description( "Gets or sets the minimum size of the collapsed tree" )]
    public int MinTreeSize
    {
        get => this.Panel1MinSize;
        set => this.Panel1MinSize = value;
    }

    /// <summary>   The pre collapse splitter distance. </summary>
    private int PreCollapseSplitterDistance { get; set; }

    /// <summary>   Event handler. Called by PinButton for check changed events. </summary>
    /// <remarks>   David, 2021-03-18. </remarks>
    /// <param name="sender">   The source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void PinButton_CheckChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents )
            return;
        if ( this.pinButton.Checked )
        {
            // tree is pinned, check to unpin.
            this.pinButton.Image = Properties.Resources.CollapseLeft_16x;
            this.SplitterDistance = this.PreCollapseSplitterDistance;
            this.pinButton.ToolTipText = "Click to unpin";
        }
        else
        {
            // tree is unpinned, check to pin.
            this.pinButton.Image = Properties.Resources.PinnedItem_16x;
            this.Collapse();
            this.pinButton.ToolTipText = "Click to pin";
        }
    }

    private delegate void SafeCallDelegate( int value );

    private void SplitterDistanceSetter( int value )
    {
        this.SplitterDistance = value;
    }

    private void SafeSplitterDistanceSetter( int value )
    {
        if ( this.InvokeRequired )
        {
            SafeCallDelegate d = new( this.SafeSplitterDistanceSetter );
            _ = this.Invoke( d, [value] );
        }
        else
        {
            this.SplitterDistanceSetter( value );
        }
    }

    /// <summary>   Collapses this tree. </summary>
    /// <remarks>   David, 2021-03-18. </remarks>
    private void Collapse()
    {
        if ( this.SplitterDistance > this.MinTreeSize )
            this.PreCollapseSplitterDistance = this.SplitterDistance;
        this.SafeSplitterDistanceSetter( this.MinTreeSize );
        // this.SplitterDistance = this.MinTreeSize;
    }

    /// <summary>   Expands this tree. </summary>
    /// <remarks>   David, 2021-03-18. </remarks>
    private void Expand()
    {
        if ( this.SplitterDistance < this.PreCollapseSplitterDistance )
            this.SafeSplitterDistanceSetter( this.PreCollapseSplitterDistance );
        // this.SplitterDistance = this.PreCollapseSplitterDistance;
    }

    /// <summary>   Gets a value indicating whether the tree is pinned. </summary>
    /// <value> True if pinned, false if not. </value>
    public bool Pinned => this.pinButton.Checked;

    /// <summary>   Event handler. Called by NavigatorTreeView for mouse enter events. </summary>
    /// <remarks>   David, 2021-03-18. </remarks>
    /// <param name="sender">   The source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void NavigatorTreeView_MouseEnter( object? sender, EventArgs e )
    {
        if ( this.MouseHoverTimer is not null )
        {
            if ( this.MouseHoverState == HoverControlState.EnteringTreeView )
            {
                // if entering while contemplating leaving, abort entering.
                this.MouseHoverState = HoverControlState.InsideTreeView;
                this.MouseHoverTimer.Stop();
            }
            else
            {
                this.MouseHoverState = HoverControlState.EnteringTreeView;
                this.MouseHoverTimer.Start();

            }
        }
        else
        {
            // on mouse enter, if not pinned expand
            if ( !this.Pinned )
                this.Expand();
        }
    }

    /// <summary>   Event handler. Called by NavigatorTreeView for mouse leave events. </summary>
    /// <remarks>   David, 2021-03-18. </remarks>
    /// <param name="sender">   The source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void NavigatorTreeView_MouseLeave( object? sender, EventArgs e )
    {
        if ( this.MouseHoverTimer is not null )
        {
            if ( this.MouseHoverState == HoverControlState.EnteringTreeView )
            {
                // if leaving while contemplating entering, abort entering.
                this.MouseHoverState = HoverControlState.OutsideTreeView;
                this.MouseHoverTimer.Stop();
            }
            else
            {
                this.MouseHoverState = HoverControlState.ExitingTreeView;
                this.MouseHoverTimer.Start();
            }
        }
        else
        {
            // on mouse leave, if not pinned collapse
            if ( !this.Pinned )
                this.Collapse();
        }
    }

    #endregion

    #region " collapse control state machine "

    private enum HoverControlState
    {
        OutsideTreeView,
        EnteringTreeView,
        InsideTreeView,
        ExitingTreeView
    }

    private HoverControlState MouseHoverState { get; set; }

    private System.Timers.Timer MouseHoverTimer { get; set; }

    private void MouseHoverTimer_Elapsed( object? sender, System.Timers.ElapsedEventArgs e )
    {
        switch ( this.MouseHoverState )
        {
            case HoverControlState.OutsideTreeView:
                break;
            case HoverControlState.EnteringTreeView:
                // on mouse enter, expand if not pinned
                if ( !this.Pinned )
                    this.Expand();
                this.MouseHoverState = HoverControlState.InsideTreeView;
                break;
            case HoverControlState.InsideTreeView:
                break;
            case HoverControlState.ExitingTreeView:
                // on mouse leave, collapse if not pinned
                if ( !this.Pinned )
                    this.Collapse();
                this.MouseHoverState = HoverControlState.OutsideTreeView;
                break;
            default:
                break;
        }
    }

    #endregion

    #region " tree nodes "

    /// <summary> Clears the nodes. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    public void ClearNodes()
    {
        this.navigatorTreeView.Nodes.Clear();
        this.TreeNodes.Clear();
        this.NodeControls.Clear();
        this.NodesVisited.Clear();
    }

    /// <summary> Gets the number of nodes. </summary>
    /// <value> The number of nodes. </value>
    public int NodeCount => this.navigatorTreeView.Nodes.Count;

    /// <summary> Removes the node. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="nodeName"> Name of the node. </param>
    public void RemoveNode( string nodeName )
    {
        _ = this.TreeNodes.Remove( nodeName );
        _ = this.NodeControls.Remove( nodeName );
        this.navigatorTreeView.Nodes.RemoveByKey( nodeName );
    }

    /// <summary> Adds a node. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="nodeName">    Name of the node. </param>
    /// <param name="nodeCaption"> The node caption. </param>
    /// <param name="nodeControl"> The node control. </param>
    /// <returns> A TreeNode. </returns>
    public TreeNode AddNode( string nodeName, string nodeCaption, Control nodeControl )
    {
        TreeNode newNode = new() { Name = nodeName, Text = nodeCaption };
        this.TreeNodes.Add( nodeName, newNode );
        _ = this.navigatorTreeView.Nodes.Add( this.TreeNodes[nodeName] );
        this.NodeControls.Add( nodeName, nodeControl );
        return newNode;
    }

    /// <summary> Inserts a node. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="index">       Zero-based index of the. </param>
    /// <param name="nodeName">    Name of the node. </param>
    /// <param name="nodeCaption"> The node caption. </param>
    /// <param name="nodeControl"> The node control. </param>
    /// <returns> A TreeNode. </returns>
    public TreeNode InsertNode( int index, string nodeName, string nodeCaption, Control nodeControl )
    {
        TreeNode newNode = new() { Name = nodeName, Text = nodeCaption };
        this.TreeNodes.Add( nodeName, newNode );
        this.navigatorTreeView.Nodes.Insert( index, this.TreeNodes[nodeName] );
        this.NodeControls.Add( nodeName, nodeControl );
        return newNode;
    }

    /// <summary> Gets a node. </summary>
    /// <remarks> David, 2020-09-03. </remarks>
    /// <param name="nodeName"> Name of the node. </param>
    /// <returns> The node. </returns>
    public TreeNode GetNode( string nodeName )
    {
        return this.TreeNodes.TryGetValue( nodeName, out TreeNode? value ) ? value : new TreeNode();
    }

    #endregion

    #region " navigation "

    /// <summary> Gets a dictionary of node controls. </summary>
    /// <value> A dictionary of node controls. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public IDictionary<string, Control> NodeControls { get; private set; }

    /// <summary> Gets a dictionary of <see cref="TreeNode"/>. </summary>
    /// <value> A dictionary of nodes. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public IDictionary<string, TreeNode> TreeNodes { get; private set; }

    /// <summary> Gets the last node selected. </summary>
    /// <value> The last node selected. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public TreeNode? LastNodeSelected { get; private set; }

    /// <summary> Gets the last tree view node selected. </summary>
    /// <value> The last tree view node selected. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    protected string LastTreeViewNodeSelected => this.LastNodeSelected is null ? string.Empty : this.LastNodeSelected.Name;

    /// <summary> Gets or sets the nodes visited. </summary>
    /// <value> The nodes visited. </value>
    private List<string> NodesVisited { get; set; }

    /// <summary> Select active control. </summary>
    /// <remarks> David, 2020-09-10. </remarks>
    /// <param name="nodeName"> Name of the node. </param>
    /// <returns> True if it succeeds; otherwise, false. </returns>
    protected virtual bool SelectActiveControl( string nodeName )
    {
        if ( this.NodeControls.TryGetValue( nodeName, out Control? activeControl ) )
        {
            // turn off the visibility of the current panel, this turns off the visibility of the
            // contained controls, which will now be removed from the panel.
            this.Panel2.Hide();

            this.Panel2.Controls.Clear();
            if ( activeControl is not null )
            {
                activeControl.Dock = DockStyle.None;
                this.Panel2.Controls.Add( activeControl );
                activeControl.Dock = DockStyle.Fill;
                activeControl.Visible = true;
            }

            // turn on visibility on the panel -- this toggles the visibility of the contained controls,
            // which is required for the messages boxes.
            this.Panel2.Show();
            if ( !this.NodesVisited.Contains( nodeName ) )
            {
                this.NodesVisited.Add( nodeName );
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary> Called after a node is selected. Displays to relevant screen. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="nodeName"> The node. </param>
    protected virtual void OnNodeSelected( string nodeName )
    {
        _ = this.SelectActiveControl( nodeName );
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Console"/> is navigating.
    /// </summary>
    /// <remarks>
    /// Used to ignore changes in grids during the navigation. The grids go through selecting their
    /// rows when navigating.
    /// </remarks>
    /// <value> <c>true</c> if navigating; otherwise, <c>false</c>. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public bool Navigating { get; private set; }

    /// <summary> Handles the BeforeSelect event of the _navigatorTreeView control. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      The <see cref="TreeViewCancelEventArgs"/> instance
    /// containing the event data. </param>
    private void NavigatorTreeView_BeforeSelect( object? sender, TreeViewCancelEventArgs e )
    {
        if ( e is not null )
        {
            if ( this.NodeControls.TryGetValue( e.Node?.Name ?? string.Empty, out Control? activeControl ) )
            {
                if ( activeControl is not null )
                {
                    activeControl.Visible = false;
                }

                this.Navigating = true;
            }
        }
    }

    /// <summary> Handles the AfterSelect event of the Me._navigatorTreeView control. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      The <see cref="TreeViewEventArgs" /> instance
    /// containing the event data. </param>
    private void NavigatorTreeView_afterSelect( object? sender, TreeViewEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null )
        {
            return;
        }

        string activity;
        try
        {
            activity = $"{this.Name} handling navigator after select event";
            if ( this.LastNodeSelected is not null )
            {
                this.LastNodeSelected.BackColor = this.navigatorTreeView.BackColor;
            }

            if ( (e.Node?.IsSelected).GetValueOrDefault( false ) )
            {
                this.LastNodeSelected = e.Node;
                e.Node!.BackColor = SystemColors.Highlight;
                this.OnNodeSelected( this.LastTreeViewNodeSelected );
                this.AfterNodeSelected?.Invoke( this, e );
            }
        }
        catch ( Exception ex )
        {
            this.OnEventHandlerError( ex );
        }
        finally
        {
            this.Navigating = false;
        }
    }

    /// <summary> Select navigator tree view first node. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    public void SelectNavigatorTreeViewFirstNode()
    {
        if ( this.NodeCount > 0 )
        {
            this.navigatorTreeView.SelectedNode = this.navigatorTreeView.Nodes[0];
        }
    }

    /// <summary> Selects the navigator tree view node. </summary>
    /// <remarks> David, 2020-09-24. </remarks>
    /// <param name="nodeName"> The node. </param>
    public void SelectNavigatorTreeViewNode( string nodeName )
    {
        this.navigatorTreeView.SelectedNode = this.navigatorTreeView.Nodes[nodeName];
    }

    /// <summary> Event queue for all listeners interested in AfterNodeSelected events. </summary>
    public event EventHandler<TreeViewEventArgs>? AfterNodeSelected;

    #endregion

    #region " exception event handler "

    /// <summary>   Event queue for all listeners interested in <see cref="Exception"/> events. </summary>
    public event EventHandler<System.Threading.ThreadExceptionEventArgs>? ExceptionEventHandler;

    /// <summary>
    /// Raises the <see cref="EventHandler{TEventArgs}"/> with
    /// <see cref="System.Threading.ThreadExceptionEventArgs"/> event.
    /// </summary>
    /// <remarks>   David, 2021-05-03. </remarks>
    /// <param name="e">                                Event information to send to registered event
    ///                                                 handlers. </param>
    /// <param name="displayMessageBoxIfNoSubscribers"> (Optional) True to display message box if no
    ///                                                 subscribers. </param>
    protected virtual void OnEventHandlerError( System.Threading.ThreadExceptionEventArgs e, bool displayMessageBoxIfNoSubscribers = true )
    {
        EventHandler<System.Threading.ThreadExceptionEventArgs>? handler = this.ExceptionEventHandler;
        if ( handler is not null )
        {
            handler.Invoke( this, e );
        }
        else
        {
            if ( displayMessageBoxIfNoSubscribers )
            {
                _ = MessageBox.Show( e.Exception.ToString() );
            }
        }
    }

    /// <summary>
    /// Raises the <see cref="EventHandler{TEventArgs}"/> with
    /// <see cref="System.Threading.ThreadExceptionEventArgs"/> event.
    /// </summary>
    /// <remarks>   David, 2021-07-29. </remarks>
    /// <param name="exception">                        The exception. </param>
    /// <param name="displayMessageBoxIfNoSubscribers"> (Optional) True to display message box if no
    ///                                                 subscribers. </param>
    protected virtual void OnEventHandlerError( System.Exception exception, bool displayMessageBoxIfNoSubscribers = true )
    {
        this.OnEventHandlerError( new System.Threading.ThreadExceptionEventArgs( exception ), displayMessageBoxIfNoSubscribers );
    }

    #endregion
}
