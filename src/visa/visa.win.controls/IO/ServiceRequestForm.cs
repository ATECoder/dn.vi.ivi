using System.Windows.Forms;

namespace cc.isr.Visa.WinControls;

/// <summary>   Form for viewing the service request. </summary>
/// <remarks>   David, 2021-07-17. </remarks>
public partial class ServiceRequestForm : Form
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-07-16. </remarks>
    public ServiceRequestForm()
    {
        this.InitializeComponent();
        this.Icon = Properties.Resources.favicon;
    }

}
