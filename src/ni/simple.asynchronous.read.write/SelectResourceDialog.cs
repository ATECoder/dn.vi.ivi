using System.ComponentModel;

namespace NI.SimpleAsynchronousReadWrite;

/// <summary>   Dialog for selected a resource. </summary>
/// <remarks>   David, 2021-11-12. </remarks>
public partial class SelectResourceDialog : System.Windows.Forms.Form
{
    public SelectResourceDialog()
    {
        //
        // Required for Windows Form Designer support
        //
        this.InitializeComponent();

        System.Resources.ResourceManager resources = new( typeof( SelectResourceDialog ) );
        this.Icon = ( System.Drawing.Icon ) resources.GetObject( "$this.Icon", System.Globalization.CultureInfo.CurrentCulture )!;

    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
        base.Dispose( disposing );
    }

    private void OnLoad( object? sender, System.EventArgs e )
    {
        // This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
        // Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
        // requires additional VISA .NET implementations.
        foreach ( string s in Ivi.Visa.GlobalResourceManager.Find( "(ASRL|GPIB|TCPIP|USB)?*INSTR" ) )
        {
            _ = this._availableResourcesListBox.Items.Add( s );
        }
    }

    /// <summary>
    /// Event handler. Called by AvailableResourcesListBox for double click events.
    /// </summary>
    /// <remarks>   2024-11-19. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void AvailableResourcesListBox_DoubleClick( object? sender, System.EventArgs e )
    {
        string? selectedString = this._availableResourcesListBox.SelectedItem as string;
        this.ResourceName = selectedString;
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    /// <summary>
    /// Event handler. Called by AvailableResourcesListBox for selected index changed events.
    /// </summary>
    /// <remarks>   2024-11-19. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void AvailableResourcesListBox_SelectedIndexChanged( object? sender, System.EventArgs e )
    {
        string? selectedString = this._availableResourcesListBox.SelectedItem as string;
        this.ResourceName = selectedString;
    }

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string? ResourceName
    {
        get => this._visaResourceNameTextBox.Text;
        set => this._visaResourceNameTextBox.Text = value;
    }
}
