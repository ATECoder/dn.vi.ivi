using System;
using System.Windows.Forms;

namespace cc.isr.Visa.IO.Demo.UI;

/// <summary>   Form for viewing the Visa IO user control. </summary>
/// <remarks>   David, 2021-07-17. </remarks>
public partial class VisaIoForm : Form
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-07-16. </remarks>
    public VisaIoForm()
    {
        this.InitializeComponent();
        this.Icon = cc.isr.Visa.IO.Demo.Properties.Resources.favicon;
        this.Text = $"Visa Interactive IO: {Gac.GacLoader.VisaPackageSource}";
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
