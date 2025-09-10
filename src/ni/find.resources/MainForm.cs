using Ivi.Visa;

namespace NI.FindResources;
/// <summary>
/// This application shows the user how to use ResourceManager to
/// find all of the available resources on their system.  In the
/// example, they can select between several filters to narrow the
/// list.
/// </summary>
public partial class MainForm : System.Windows.Forms.Form
{
    private string? _filter;

    public MainForm()
    {
        //
        // Required for Windows Form Designer support
        //
        this.InitializeComponent();

        System.ComponentModel.ComponentResourceManager resources = new( this.GetType() );
        this.Icon = ( System.Drawing.Icon ) resources.GetObject( "$this.Icon", System.Globalization.CultureInfo.CurrentCulture )!;

        this._gpibTreeNode = new TreeNode( "GPIB" );
        this._vxiTreeNode = new TreeNode( "VXI" );
        this._serialTreeNode = new TreeNode( "Serial" );
        this._pxiTreeNode = new TreeNode( "PXI" );
        this._tcpIpTreeNode = new TreeNode( "TCP/IP" );
        this._uSBTreeNode = new TreeNode( "USB" );
        this.CleanResourceNodes();

        this.PopulateFilterList();
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            this._components?.Dispose();
        }
        base.Dispose( disposing );
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.Run( new MainForm() );
    }

    private void PopulateFilterList()
    {
        this._filterStringsListBox.Items.Clear();
        _ = this._filterStringsListBox.Items.Add( "?*" );
        _ = this._filterStringsListBox.Items.Add( "ASRL?*INSTR" );
        _ = this._filterStringsListBox.Items.Add( "GPIB?*" );
        _ = this._filterStringsListBox.Items.Add( "GPIB?*INSTR" );
        _ = this._filterStringsListBox.Items.Add( "GPIB?*INTFC" );
        _ = this._filterStringsListBox.Items.Add( "PXI?*" );
        _ = this._filterStringsListBox.Items.Add( "PXI?*BACKPLANE" );
        _ = this._filterStringsListBox.Items.Add( "PXI?*INSTR" );
        _ = this._filterStringsListBox.Items.Add( "TCPIP?*" );
        _ = this._filterStringsListBox.Items.Add( "TCPIP?*INSTR" );
        _ = this._filterStringsListBox.Items.Add( "TCPIP?*SOCKET" );
        _ = this._filterStringsListBox.Items.Add( "(TCPIP|GPIB|USB)?*INSTR" );
        _ = this._filterStringsListBox.Items.Add( "USB?*" );
        _ = this._filterStringsListBox.Items.Add( "USB?*INSTR" );
        _ = this._filterStringsListBox.Items.Add( "USB?*RAW" );
        _ = this._filterStringsListBox.Items.Add( "VXI?*" );
        _ = this._filterStringsListBox.Items.Add( "VXI?*BACKPLANE" );
        _ = this._filterStringsListBox.Items.Add( "VXI?*INSTR" );
        this._filterStringsListBox.SelectedIndex = 0;
    }

    private void AddToResourceTree()
    {
        if ( this._gpibTreeNode.Nodes.Count != 0 )
            _ = this._resourceTreeView.Nodes.Add( this._gpibTreeNode );
        if ( this._vxiTreeNode.Nodes.Count != 0 )
            _ = this._resourceTreeView.Nodes.Add( this._vxiTreeNode );
        if ( this._serialTreeNode.Nodes.Count != 0 )
            _ = this._resourceTreeView.Nodes.Add( this._serialTreeNode );
        if ( this._pxiTreeNode.Nodes.Count != 0 )
            _ = this._resourceTreeView.Nodes.Add( this._pxiTreeNode );
        if ( this._tcpIpTreeNode.Nodes.Count != 0 )
            _ = this._resourceTreeView.Nodes.Add( this._tcpIpTreeNode );
        if ( this._uSBTreeNode.Nodes.Count != 0 )
            _ = this._resourceTreeView.Nodes.Add( this._uSBTreeNode );
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0010:Add missing cases", Justification = "<Pending>" )]
    private void AddToResourceNode( string resourceName, HardwareInterfaceType intType )
    {
        switch ( intType )
        {
            case HardwareInterfaceType.Gpib:
                _ = this._gpibTreeNode.Nodes.Add( new TreeNode( resourceName ) );
                break;
            case HardwareInterfaceType.GpibVxi:
                _ = this._gpibTreeNode.Nodes.Add( new TreeNode( resourceName ) );
                break;
            case HardwareInterfaceType.Vxi:
                _ = this._vxiTreeNode.Nodes.Add( new TreeNode( resourceName ) );
                break;
            case HardwareInterfaceType.Serial:
                _ = this._serialTreeNode.Nodes.Add( new TreeNode( resourceName ) );
                break;
            case HardwareInterfaceType.Pxi:
                _ = this._pxiTreeNode.Nodes.Add( new TreeNode( resourceName ) );
                break;
            case HardwareInterfaceType.Tcp:
                _ = this._tcpIpTreeNode.Nodes.Add( new TreeNode( resourceName ) );
                break;
            case HardwareInterfaceType.Usb:
                _ = this._uSBTreeNode.Nodes.Add( new TreeNode( resourceName ) );
                break;
            default:
                break;
        }
    }

    /// <summary>   Searches for the first resources. </summary>
    /// <remarks>   David, 2021-11-12. </remarks>
    private void FindResources()
    {
        // This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
        // Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
        // requires additional VISA .NET implementations.
        try
        {
            IEnumerable<string> resources = Ivi.Visa.GlobalResourceManager.Find( this._filter ?? "TCPIP?*" );
            foreach ( string s in resources )
            {
                ParseResult parseResult = Ivi.Visa.GlobalResourceManager.Parse( s );
                this.AddToResourceNode( s, parseResult.InterfaceType );
            }
            this.AddToResourceTree();
        }
        catch ( Exception ex )
        {
            _ = MessageBox.Show( ex.Message );
        }
    }

    /// <summary>   Clean resource nodes. </summary>
    /// <remarks>   David, 2021-11-12. </remarks>
    private void CleanResourceNodes()
    {
        this._gpibTreeNode.Nodes.Clear();
        this._vxiTreeNode.Nodes.Clear();
        this._serialTreeNode.Nodes.Clear();
        this._pxiTreeNode.Nodes.Clear();
        this._tcpIpTreeNode.Nodes.Clear();
        this._uSBTreeNode.Nodes.Clear();
    }

    private void FindResourcesButton_Click( object? sender, System.EventArgs e )
    {
        this._filter = this._filterStringsListBox.Text;
        this.DisplayResources();
    }

    private static string GetCustomFilter()
    {
        CustomFilterDialog cff = new();
        _ = cff.ShowDialog();
        return cff.CustomFilter;
    }

    private void ClearButton_Click( object? sender, System.EventArgs e )
    {
        this._resourceTreeView.Nodes.Clear();
        this.CleanResourceNodes();
    }

    private void UseCustomStringButton_Click( object? sender, System.EventArgs e )
    {
        this._filter = GetCustomFilter();
        this.DisplayResources();
    }

    private void DisplayResources()
    {
        this._resourceTreeView.Nodes.Clear();
        this.CleanResourceNodes();
        this.FindResources();
        this._resourceTreeView.ExpandAll();
    }

    public string ResourceName
    {
        get
        {
            try
            {
                return this._resourceTreeView?.SelectedNode?.Text ?? string.Empty;
            }
            catch ( Exception )
            {
                return string.Empty;
            }
        }
    }
}
