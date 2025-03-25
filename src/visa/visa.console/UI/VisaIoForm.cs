using System;
using System.Windows.Forms;

namespace cc.isr.Visa.Console.UI;

/// <summary>   Form for viewing the Visa IO user control. </summary>
/// <remarks>   David, 2021-07-17. </remarks>
public partial class VisaIoForm : Form
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-07-16. </remarks>
    public VisaIoForm()
    {
        this.InitializeComponent();
        this.Icon = cc.isr.Visa.Console.Properties.Resources.favicon;
    }

    protected override void OnLoad( EventArgs e )
    {
        base.OnLoad( e );
        _ = Program.TraceLogger.LogVerbose( "VISA IO Form loaded" );
    }

    protected override void OnShown( EventArgs e )
    {
        base.OnShown( e );
        _ = Program.TraceLogger.LogVerbose( "VISA IO Form shown" );
    }

}
