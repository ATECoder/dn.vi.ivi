using System.Windows.Forms;

namespace cc.isr.Visa.WinControls;

/// <summary>   Form for viewing the write/read user control. </summary>
/// <remarks>   David, 2021-07-17. </remarks>
public partial class SimpleWriteReadForm : Form
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-07-16. </remarks>
    public SimpleWriteReadForm()
    {
        this.InitializeComponent();
        this.Icon = Properties.Resources.favicon;
    }

}
