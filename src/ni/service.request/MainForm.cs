//==================================================================================================
//
// Title      : MainForm.cs
// Purpose    : This application illustrates how to use the service request event and
//              the service request status byte to determine when generated data is ready
//              and how to read it.
//
//==================================================================================================

using Ivi.Visa;

namespace NI.ServiceRequest;
/// <summary>
/// Summary description for Form1.
/// </summary>
public partial class MainForm : System.Windows.Forms.Form
{
    private IMessageBasedSession? _session;

    public MainForm()
    {
        //
        // Required for Windows Form Designer support
        //
        this.InitializeComponent();
        this.InitializeUI();
        this._toolTip.SetToolTip( this._enableSRQButton, "Enable the instrument's SRQ event on MAV by sending the following command (varies by instrument):" );
        this._toolTip.SetToolTip( this._writeButton, "Send string to device" );
        this._toolTip.SetToolTip( this._closeButton, "Causes the control to release its handle to the device" );
        this._toolTip.SetToolTip( this._openButton, "The resource name of the device is set and the control attempts to connect to the device" );

        try
        {
            // This example uses an instance of the NationalInstruments.Visa.ResourceManager class to find resources on the system.
            // Alternatively, static methods provided by the Ivi.Visa.ResourceManager class may be used when an application
            // requires additional VISA .NET implementations.
            IEnumerable<string> validResources = Ivi.Visa.GlobalResourceManager.Find( "(GPIB|TCPIP|USB)?*INSTR" );
            foreach ( string? resource in validResources )
            {
                _ = this._resourceNameComboBox.Items.Add( resource );
            }
        }
        catch ( Exception )
        {
            _ = this._resourceNameComboBox.Items.Add( "No 488.2 INSTR resource found on the system" );
            this.UpdateResourceNameControls( true );
            this._closeButton.Enabled = false;
        }
        this._resourceNameComboBox.SelectedIndex = 0;
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            this.components?.Dispose();
            this._session?.Dispose();
        }
        base.Dispose( disposing );
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.Run( new MainForm() );
    }

    private void UpdateResourceNameControls( bool enable )
    {
        this._resourceNameComboBox.Enabled = enable;
        this._openButton.Enabled = enable;
        this._closeButton.Enabled = !enable;
        if ( enable )
        {
            _ = this._openButton.Focus();
        }
    }

    private void UpdateSRQControls( bool enable )
    {
        this._commandTextBox.Enabled = enable;
        this._enableSRQButton.Enabled = enable;
        if ( enable )
        {
            _ = this._enableSRQButton.Focus();
        }
    }

    private void UpdateWriteControls( bool enable )
    {
        this._writeTextBox.Enabled = enable;
        this._writeButton.Enabled = enable;
        if ( enable )
        {
            _ = this._writeButton.Focus();
        }
    }

    private void InitializeUI()
    {
        this.UpdateResourceNameControls( true );
        this.UpdateSRQControls( false );
        this.UpdateWriteControls( false );
    }


    // When the Open Session button is pressed, the resource name of the
    // device is set and the control attempts to connect to the device
    private void OpenButton_Click( object? sender, System.EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"Opening a {this._resourceNameComboBox.Text} VISA session";
            this._session = ( Ivi.Visa.IMessageBasedSession ) Ivi.Visa.GlobalResourceManager.Open( this._resourceNameComboBox.Text );

            activity = "Sending Selective Device Clear command (Session.Clear())";
            // this is required to clear the session (fixed Keysight service request issues;
            // partially as we still are having issues with second event.
            this._session.Clear();

            activity = "Initializing session";
            // Use SynchronizeCallbacks to specify that the object marshals callbacks across threads appropriately.
            this._session.SynchronizeCallbacks = true;
            this._session.TerminationCharacter = ( byte ) this._readTerminationCharacterNumeric.Value;
            this._session.TerminationCharacterEnabled = this._readTerminationEnabledCheckBox.Checked;

            activity = "updating controls";
            this.UpdateResourceNameControls( false );
            this.UpdateSRQControls( true );
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( $"Exception {activity}: {exp}" );
        }
    }


    // The Enable SRQ button writes the string that tells the instrument to
    // enable the SRQ bit
    private void EnableSRQButton_Click( object? sender, System.EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            // Registering a handler for an event automatically enables that event.
            activity = $"Registering the {nameof( IMessageBasedSession.ServiceRequest )} event handler";
            if ( this._session is not null ) this._session.ServiceRequest += this.OnServiceRequest;
            activity = $"writing {this._commandTextBox.Text}";
            this.WriteToSession( this._commandTextBox.Text );
            activity = "updating SRQ Controls";
            this.UpdateSRQControls( false );
            activity = "updating Write Controls";
            this.UpdateWriteControls( true );
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( $"Exception {activity}: {exp.Message}" );
        }
    }

    // Pressing Close Session causes the control to release its handle to the device
    private void CloseButton_Click( object? sender, System.EventArgs e )
    {
        if ( this._session is not null ) this._session.ServiceRequest -= this.OnServiceRequest;
        this._session?.Dispose();
        this.InitializeUI();
    }

    // Clicking the Write Button causes the Send String to be written to the device
    private void WriteButton_Click( object? sender, System.EventArgs e )
    {
        this.WriteToSession( this._writeTextBox.Text );
    }

    // Pressing the Clear button clears the read text box
    private void ClearButton_Click( object? sender, System.EventArgs e )
    {
        this._readTextBox.Clear();
    }

    private static string ReplaceCommonEscapeSequences( string s )
    {
        return s.Replace( "\\n", "\n" ).Replace( "\\r", "\r" );
    }

    private static string InsertCommonEscapeSequences( string s )
    {
        return s.Replace( "\n", "\\n" ).Replace( "\r", "\\r" );
    }

    private void WriteToSession( string txtWrite )
    {
        try
        {
            string textToWrite = ReplaceCommonEscapeSequences( txtWrite );
            this._session?.RawIO.Write( textToWrite );
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
    }

    private void OnServiceRequest( object? sender, VisaEventArgs e )
    {
        try
        {
            if ( this.InvokeRequired )
            {
                _ = this.Invoke( new Action<object, VisaEventArgs>( this.OnServiceRequest ), [sender, e] );
            }
            else
            {
                IMessageBasedSession? mbs = sender as IMessageBasedSession;
                StatusByteFlags? sb = mbs?.ReadStatusByte();

                if ( (sb & StatusByteFlags.MessageAvailable) != 0 )
                {
                    string? textRead = mbs?.RawIO.ReadString();
                    this._readTextBox.Text = InsertCommonEscapeSequences( textRead ?? string.Empty );
                    mbs?.DiscardEvents( EventType.AllEnabled );
                }
                else
                {
                    _ = MessageBox.Show( "MAV in status register is not set, which means that message is not available. Make sure the command to enable SRQ is correct, and the instrument is 488.2 compatible." );
                }
            }
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
    }
}
