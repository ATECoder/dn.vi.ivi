using Ivi.Visa;

namespace NI.SimpleReadWrite;

/// <summary> A select resources. </summary>
/// <remarks> David, 2020-10-11. </remarks>
public partial class SelectResources
{
    public SelectResources() => this.InitializeComponent();

    /// <summary> Event handler. Called by MyBase for load events. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information to send to registered event handlers. </param>
    private void OnLoad( object? sender, EventArgs e )
    {
        // This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
        // Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
        // requires additional VISA .NET implementations.
        IEnumerable<string> resources = GlobalResourceManager.Find( "(ASRL|GPIB|TCPIP|USB)?*" );
        foreach ( string s in resources )
            _ = this.AvailableResourceNamesListBox.Items.Add( s );
    }

    /// <summary> Adds a button click to 'e'. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AddResourceNameButton_Click( object? sender, EventArgs e )
    {
        if ( this.AvailableResourceNamesListBox.SelectedItem is string selectedString )
            _ = this.SelectedResourceNamesListBox.Items.Add( selectedString );
    }

    /// <summary> Removes the button click. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void RemoveResourceNameButton_Click( object? sender, EventArgs e )
    {
        if ( this.AvailableResourceNamesListBox.SelectedItem is string selectedString )
            this.SelectedResourceNamesListBox.Items.Remove( selectedString );
    }

    /// <summary> Gets a list of names of the resources. </summary>
    /// <value> A list of names of the resources. </value>
    public IEnumerable<string> ResourceNames
    {
        get
        {
            List<string> l = [];
            foreach ( object item in this.SelectedResourceNamesListBox.Items )
                l.Add( ( string ) item );
            return [.. l];
        }
    }
}
