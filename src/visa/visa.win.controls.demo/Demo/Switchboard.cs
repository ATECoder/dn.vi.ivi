namespace cc.isr.Visa.WinControls.Demo;

public partial class Switchboard : Form
{
    public Switchboard()
    {
        this.InitializeComponent();

#if true
#pragma warning disable CS8605 // Unboxing a possibly null value.
        IEnumerable<Tuple<AvailableForm, string>>? enums = typeof( AvailableForm ).GetFields().Skip( 1 )
            .Select( x => Tuple.Create( ( AvailableForm ) x.GetValue( default ), x.Name ) );
#pragma warning restore CS8605 // Unboxing a possibly null value.
#else
        var enums = typeof( AvailableForm ).GetFields().Skip( 1 )
            .Select( x => Tuple.Create( ( AvailableForm ) x.GetValue( null ),
                                                          x.GetCustomAttributes<DescriptionAttribute>().First().Description ) );
#endif
        this.AvailableForms.DataSource = new List<Tuple<AvailableForm, string>>( enums );
        this.AvailableForms.ValueMember = "Item1";
        this.AvailableForms.DisplayMember = "Item2";
    }

    private void OpenFormButton_Click( object? sender, EventArgs e )
    {
        if ( this.AvailableForms.Items is null || this.AvailableForms.Items.Count == 0 )
        {
            _ = MessageBox.Show( "Failed to populate the list of available forms." );
            return;
        }

        if ( this.AvailableForms.SelectedValue is not AvailableForm availableForm )
        {
            _ = MessageBox.Show( $"The selected value {this.AvailableForms.SelectedValue} is not a valid {nameof( cc.isr.Visa.WinControls.Demo.AvailableForm )}." );
            return;
        }

        switch ( availableForm )
        {
            case AvailableForm.ResourceNameSelectorForm:
                {
                    using Form form = new cc.isr.Visa.WinControls.ResourceNameSelectorForm();
                    _ = form.ShowDialog( this );
                }
                break;
            case AvailableForm.ResourceNamesSelectorForm:
                {
                    using Form form = new ResourceNamesSelectorForm();
                    _ = form.ShowDialog( this );
                }
                break;
            case AvailableForm.SimpleReadWriteForm:
                {
                    using Form form = new SimpleWriteReadForm();
                    _ = form.ShowDialog( this );
                }
                break;
            case AvailableForm.ServiceRequestForm:
                {
                    using Form form = new ServiceRequestForm();
                    _ = form.ShowDialog( this );
                }
                break;
            default:
                {
                    using Form form = new cc.isr.Visa.WinControls.ResourceNameSelectorForm();
                    _ = form.ShowDialog( this );
                }
                break;
        }

    }
}
/// <summary>   Values that represent available forms. </summary>
/// <remarks>   David, 2021-07-16. </remarks>
internal enum AvailableForm
{
    /// <summary>   An enum constant representing the resource name selector form option. </summary>
    ResourceNameSelectorForm,

    /// <summary>   An enum constant representing the resource names selector form option. </summary>
    ResourceNamesSelectorForm,

    /// <summary>   An enum constant representing the simple read write form option. </summary>
    SimpleReadWriteForm,

    /// <summary>   An enum constant representing the service request form option. </summary>
    ServiceRequestForm

}
