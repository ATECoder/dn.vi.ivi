using System;
using System.Windows.Forms;
using Ivi.Visa.ConflictManager;

namespace cc.isr.Visa.ResourceExplorer;

/// <summary>   Form for viewing the explorer. </summary>
/// <remarks>   David, 2021-11-08. </remarks>
public partial class ExplorerForm : Form
{
    public ExplorerForm() => this.InitializeComponent();

    #region " form events "

    /// <summary>   Raises the <see cref="System.Windows.Forms.Form.Load" /> event. </summary>
    /// <remarks>   David, 2021-11-08. </remarks>
    /// <param name="e">    An <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnLoad( EventArgs e )
    {
        try
        {
            this.Text = "Visa Resources Explorer";
        }
        catch ( Exception )
        {
            throw;
        }
        finally
        {
            base.OnLoad( e );
        }
    }
    #endregion

    #region " control event handlers "

    private void FileExitMenuItem_Click( object? sender, EventArgs e )
    {
        this.Close();
    }

#if NET5_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>" )]
#endif
    private void ImplementationsListAllMenuItem_Click( object? sender, EventArgs e )
    {
        System.Collections.Generic.IEnumerable<VisaImplementation> installedVisas = Gac.GacLoader.EnumerateDotNetImplementations();
        System.Text.StringBuilder builder = new();
        if ( installedVisas is object )
        {
            foreach ( VisaImplementation implementation in installedVisas )
            {
                string spaces = "  ";
                _ = builder.AppendLine( $"{nameof( VisaImplementation )}.{nameof( VisaImplementation.Location )}: {implementation.Location}" );
                _ = builder.AppendLine( $"{spaces}{nameof( VisaImplementation )}.{nameof( VisaImplementation.ApiType )}: {implementation.ApiType}" );
                _ = builder.AppendLine( $"{spaces}{nameof( VisaImplementation )}.{nameof( VisaImplementation.Comments )}: {implementation.Comments}" );
                _ = builder.AppendLine( $"{spaces}{nameof( VisaImplementation )}.{nameof( VisaImplementation.Enabled )}: {implementation.Enabled}" );
                _ = builder.AppendLine( $"{spaces}{nameof( VisaImplementation )}.{nameof( VisaImplementation.FriendlyName )}: {implementation.FriendlyName}" );
                _ = builder.AppendLine( $"{spaces}{nameof( VisaImplementation )}.{nameof( VisaImplementation.ResourceManufacturerId )}: {implementation.ResourceManufacturerId}" );
            }
        }
        else
        {
            _ = builder.AppendLine( "Not Dot Net VISA implementations were found." );
        }
        this.globalTextBox.Text = builder.ToString();
    }

    #endregion
}
