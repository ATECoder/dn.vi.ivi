namespace NI.FindResources;
/// <summary>
/// Lets the user enter the custom filter string to find the available resources with. Public
/// property CustomFilter returns the custom filter string.
/// </summary>
/// <remarks>   David, 2021-11-12. </remarks>
public partial class CustomFilterDialog : System.Windows.Forms.Form
{
    public CustomFilterDialog() => this.InitializeComponent();

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

    private void OkayButton_Click( object? sender, System.EventArgs e )
    {
        this.Close();
    }

    public string CustomFilter => this._customFilterTextBox.Text;

}
