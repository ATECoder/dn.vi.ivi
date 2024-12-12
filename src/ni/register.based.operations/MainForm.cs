using Ivi.Visa;

namespace NI.RegisterBasedOperations;
/// <summary>
/// Summary description for Form1.
/// </summary>
public partial class MainForm : Form
{
    public MainForm()
    {
        //
        // Required for Windows Form Designer support
        //
        this.InitializeComponent();
        this.SetupUI( false );
        this.PopulateComboBoxes();
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            this._pxiSession?.Dispose();
        }
        base.Dispose( disposing );
    }

    private void OpenButton_Click( object? sender, EventArgs e )
    {
        this._pxiSession?.Dispose();

        try
        {
            this._pxiSession = ( IPxiSession ) Ivi.Visa.GlobalResourceManager.Open( this._resourceNameComboBox.Text );
            this.SetupUI( true );
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
    }

    private void CloseButton_Click( object? sender, EventArgs e )
    {
        try
        {
            if ( this._pxiSession is not null )
            {
                this._pxiSession.Dispose();
                this.SetupUI( false );
            }
        }
        catch ( Exception ex )
        {
            _ = MessageBox.Show( ex.Message );
        }
    }

    private void ClearButton_Click( object? sender, EventArgs e )
    {
        this._resultTextBox.Clear();
    }

    /// <summary>   Performs an "InXX" operation. </summary>
    /// <remarks>   David, 2021-06-27. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void InButton_Click( object? sender, EventArgs e )
    {
        try
        {
            if ( this._spaceComboBox.SelectedItem is AddressSpace space
                && this._widthComboBox.SelectedItem is DataWidth width )
            {
                int offset = ( int ) this._offsetNumericUpDown.Value;

                switch ( width )
                {
                    case DataWidth.Width8:
                        this._resultTextBox.AppendText( MainForm.GetOperationText( "In8" ) );
                        byte data8 = this._pxiSession.In8( space, offset );
                        this._resultTextBox.AppendText( MainForm.GetDataText( $"{data8:x2}" ) );
                        break;

                    case DataWidth.Width16:
                        this._resultTextBox.AppendText( MainForm.GetOperationText( "In16" ) );
                        short data16 = this._pxiSession.In16( space, offset );
                        this._resultTextBox.AppendText( MainForm.GetDataText( $"{data16:x4}" ) );
                        break;

                    case DataWidth.Width32:
                        this._resultTextBox.AppendText( MainForm.GetOperationText( "In32" ) );
                        int data32 = this._pxiSession.In32( space, offset );
                        this._resultTextBox.AppendText( MainForm.GetDataText( $"{data32:x8}" ) );
                        break;

                    case DataWidth.Width64:
                        this._resultTextBox.AppendText( MainForm.GetOperationText( "In64" ) );
                        long data64 = this._pxiSession.In64( space, offset );
                        this._resultTextBox.AppendText( MainForm.GetDataText( $"{data64:x16}" ) );
                        break;
                    default:
                        break;
                }
            }
        }
        catch ( Exception ex )
        {
            this._resultTextBox.AppendText( MainForm.GetOperationText( ex.Message ) );
        }
        this.ScrollToBottomOfResultTextBox();
    }

    /// <summary>   Perform a "MoveInXX" operation. </summary>
    /// <remarks>   David, 2021-06-27. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void MoveInButton_Click( object? sender, EventArgs e )
    {
        try
        {
            if ( this._spaceComboBox.SelectedItem is AddressSpace space
                && this._widthComboBox.SelectedItem is DataWidth width )
            {
                int offset = ( int ) this._offsetNumericUpDown.Value;
                int length = ( int ) this._numElementsNumericUpDown.Value;

                switch ( width )
                {
                    case DataWidth.Width8:
                        this._resultTextBox.AppendText( MainForm.GetOperationText( "MoveIn8" ) );
                        byte[] data8 = this._pxiSession.MoveIn8( space, offset, length );
                        this.ShowArray( data8 );
                        break;

                    case DataWidth.Width16:
                        this._resultTextBox.AppendText( MainForm.GetOperationText( "MoveIn16" ) );
                        short[] data16 = this._pxiSession.MoveIn16( space, offset, length );
                        this.ShowArray( data16 );
                        break;

                    case DataWidth.Width32:
                        this._resultTextBox.AppendText( MainForm.GetOperationText( "MoveIn32" ) );
                        int[] data32 = this._pxiSession.MoveIn32( space, offset, length );
                        this.ShowArray( data32 );
                        break;

                    case DataWidth.Width64:
                        this._resultTextBox.AppendText( MainForm.GetOperationText( "MoveIn64" ) );
                        long[] data64 = this._pxiSession.MoveIn64( space, offset, length );
                        this.ShowArray( data64 );
                        break;
                    default:
                        break;
                }
            }
        }
        catch ( Exception ex )
        {
            this._resultTextBox.AppendText( MainForm.GetOperationText( ex.Message ) );
        }
        this.ScrollToBottomOfResultTextBox();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Usage", "CA2263:Prefer generic overload when type is known", Justification = "incorrect recommendation." )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    private void PopulateComboBoxes()
    {
        try
        {
            IEnumerable<string> pxiResources = Ivi.Visa.GlobalResourceManager.Find( "PXI?*INSTR" );
            foreach ( string? resource in pxiResources )
            {
                _ = this._resourceNameComboBox.Items.Add( resource );
            }
            // Add PXI specific address spaces only
            for ( AddressSpace space = AddressSpace.PxiConfiguration; space <= AddressSpace.PxiBar5; ++space )
            {
                _ = this._spaceComboBox.Items.Add( space );
            }
            this._spaceComboBox.SelectedIndex = 0;

            foreach ( DataWidth width in Enum.GetValues( typeof( DataWidth ) ) )
            {
                _ = this._widthComboBox.Items.Add( width );
            }
            this._widthComboBox.SelectedIndex = 0;
        }
        catch ( Exception )
        {
            _ = this._resourceNameComboBox.Items.Add( "No PXI Resource found on the system" );
            this._resourceNameComboBox.Enabled = false;
            this._openButton.Enabled = false;
        }
        this._resourceNameComboBox.SelectedIndex = 0;
    }

    private void SetupUI( bool sessionActive )
    {
        this._resourceNameComboBox.Enabled = !sessionActive;
        this._openButton.Enabled = !sessionActive;
        this._closeButton.Enabled = sessionActive;
        this._moveInButton.Enabled = sessionActive;
        this._inButton.Enabled = sessionActive;
        this._spaceLabel.Enabled = sessionActive;
        this._spaceComboBox.Enabled = sessionActive;
        this._widthLabel.Enabled = sessionActive;
        this._widthComboBox.Enabled = sessionActive;
        this._offsetLabel.Enabled = sessionActive;
        this._offsetNumericUpDown.Enabled = sessionActive;
        this._numElementsLabel.Enabled = sessionActive;
        this._numElementsNumericUpDown.Enabled = sessionActive;
    }

    private void ScrollToBottomOfResultTextBox()
    {
        this._resultTextBox.SelectAll();
    }

    private static string GetOperationText( string operation )
    {
        return operation + Environment.NewLine;
    }

    private static string GetDataText( string data )
    {
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, "Data = {0}", data ) + Environment.NewLine;
    }

    private void ShowArray( Array data )
    {
        int i = 0;
        foreach ( object o in data )
        {
            string formattedValue = string.Empty;
            if ( o is byte @byte )
            {
                formattedValue = @byte.ToString( "x", System.Globalization.CultureInfo.CurrentCulture );
            }
            else if ( o is short int2 )
            {
                formattedValue = int2.ToString( "x", System.Globalization.CultureInfo.CurrentCulture );
            }
            else if ( o is int @int )
            {
                formattedValue = @int.ToString( "x", System.Globalization.CultureInfo.CurrentCulture );
            }
            else if ( o is long int1 )
            {
                formattedValue = int1.ToString( "x", System.Globalization.CultureInfo.CurrentCulture );
            }
            this._resultTextBox.AppendText( string.Format( System.Globalization.CultureInfo.CurrentCulture, "Data({0} = {1})", i++, formattedValue ) + Environment.NewLine );
        }
    }

#if false

    // unused code.

    private readonly NumericUpDown[] _numOutputArray;

    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private byte[] BuildByteOutputData()
    {
        int numElements = ( int ) this._numElementsNumericUpDown.Value;
        byte[] od = new byte[numElements];
        for ( int i = 0; i < numElements; i++ )
        {
            od[i] = ( byte ) this._numOutputArray[i].Value;
        }
        return od;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private short[] BuildShortOutputData()
    {
        int numElements = ( int ) this._numElementsNumericUpDown.Value;
        short[] od = new short[numElements];
        for ( int i = 0; i < numElements; i++ )
        {
            od[i] = ( short ) this._numOutputArray[i].Value;
        }
        return od;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private int[] BuildIntOutputData()
    {
        int numElements = ( int ) this._numElementsNumericUpDown.Value;
        int[] od = new int[numElements];
        for ( int i = 0; i < numElements; i++ )
        {
            od[i] = ( int ) this._numOutputArray[i].Value;
        }
        return od;
    }
#endif

}
