using Ivi.Visa;
using System.ComponentModel;

namespace NI.SimpleReadWrite;

/// <summary> Summary description for SelectResource. </summary>
/// <remarks> David, 2020-10-11. </remarks>
public partial class SelectResource : Form
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Form" /> class.
    /// </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    public SelectResource()
    {
        base.Load += this.OnLoad;
        this.InitializeComponent();
    }

    /// <summary> Clean up any resources being used. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="disposing"> <see langword="true" /> to release both managed and unmanaged
    ///                          resources; <see langword="false" /> to release only unmanaged
    ///                          resources. </param>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }

        base.Dispose( disposing );
    }

    /// <summary> Event handler. Called by MyBase for load events. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information to send to registered event handlers. </param>
    private void OnLoad( object? sender, EventArgs e )
    {
        // This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
        // Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
        // requires additional VISA .NET implementations.
        string searchPattern = "(ASRL|GPIB|TCPIP|USB)?*";
        IEnumerable<string> resources = GlobalResourceManager.Find( searchPattern );
        foreach ( string s in resources )
            _ = this.AvailableResourcesListBox.Items.Add( s );
    }

    /// <summary> Available resources list box double click. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AvailableResourcesListBox_DoubleClick( object? sender, EventArgs e )
    {
        string? selectedString = this.AvailableResourcesListBox.SelectedItem as string;
        this.ResourceName = selectedString;
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    /// <summary> Available resources list box selected index changed. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AvailableResourcesListBox_SelectedIndexChanged( object? sender, EventArgs e )
    {
        string? selectedString = this.AvailableResourcesListBox.SelectedItem as string;
        this.ResourceName = selectedString;
    }

    /// <summary> Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string? ResourceName
    {
        get => this.visaResourceNameTextBox.Text;
        set => this.visaResourceNameTextBox.Text = value;
    }
}
