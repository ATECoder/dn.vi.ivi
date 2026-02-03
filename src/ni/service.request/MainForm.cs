using Ivi.Visa;
using NI.ServiceRequest.SessionExtensions;

namespace NI.ServiceRequest;

/// <summary>
/// This application illustrates how to use the service request event and the service request
/// status byte to determine when generated data is ready and how to read it.
/// </summary>
/// <remarks>   2026-01-27. </remarks>
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
        this._commandTextBox.Text = "*CLS; *ESE 253; *SRE 255\\n";
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
            this._statusByteTimer?.Dispose();
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

    private System.Timers.Timer? _statusByteTimer;

    /// <summary>
    /// When the Open Session button is pressed, the resource name of the device is set and the
    /// control attempts to connect to the device.
    /// </summary>
    /// <remarks>   2026-01-27. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void OpenButton_Click( object? sender, System.EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"Opening a {this._resourceNameComboBox.Text} VISA session";
            this._session = ( Ivi.Visa.IMessageBasedSession ) Ivi.Visa.GlobalResourceManager.Open( this._resourceNameComboBox.Text );

            activity = "Sending Selective Device Clear command (Session.Clear())";
            // this is required to clear the session (fixed Keysight service request issues;
            // partially as we still are having issues with second event.)
            // ... 20260202: Per Keysight, the SRQ must be cleared on the device side for the second event to occur.
            // 20260202: This still does not work!
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


    /// <summary>
    /// The Enable SRQ button writes the string that tells the instrument to enable the SRQ bit.
    /// </summary>
    /// <remarks>   2026-01-27. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void EnableSRQButton_Click( object? sender, System.EventArgs e )
    {
        string activity = string.Empty;
        try
        {

#if false
#endif

            activity = $"writing {this._commandTextBox.Text}";
#if true
            if ( !(this._session.ExecuteCommandsOperationCompleted( this._commandTextBox.Text.ReplaceCommonEscapeSequences(), CancellationToken.None ).Result ?? false) )
                MessageBox.Show( $"Failed {activity}" );
#elif false
            this._session.WriteLine( "*CLS" );
            this._session.WriteLine( "*ESE 253" );
            this._session.WriteLine( "*ESE?" );
            this.actualEseLabel.Text = this._session?.RawIO.ReadString() ?? "";

            this._session.WriteLine( "*SRE 255" );
            this._session.WriteLine( "*SRE?" );
            this.actualSreLabel.Text = this._session?.RawIO.ReadString() ?? "";
#else
            this._session.WriteLine( "*CLS" );
            this._session.WriteLine( "status.standard.enable =253" );
            this._session.WriteLine( "print(tostring(status.standard.enable))" );
            this.actualEseLabel.Text = this._session?.RawIO.ReadString() ?? "";

            this._session.WriteLine( "status.request_enable =255" );
            this._session.WriteLine( "print(tostring(status.request_enable))" );
            this.actualSreLabel.Text = this._session?.RawIO.ReadString() ?? "";
#endif

            activity = $"reading ESE";
            this.actualEseLabel.Text = this._session.QueryHex( "*ESE?" );

            activity = $"reading SRE";
            this.actualSreLabel.Text = this._session.QueryHex( "*SRE?" );

            activity = "disabling SRQ Controls";
            this.UpdateSRQControls( false );

            activity = "enabling Write Controls";
            this.UpdateWriteControls( true );

            if ( this.serialPollCheckBox.Checked )
            {
                activity = "Starting Status Byte Timer";
                this._statusByteTimer = new System.Timers.Timer( 500 );
                this._statusByteTimer.Elapsed += this.OnTimerEvent;
                this._statusByteTimer.Start();
            }
            else
            {
                // Registering a handler for an event automatically enables that event.
                activity = $"Registering the {nameof( IMessageBasedSession.ServiceRequest )} event handler";
                if ( this._session is not null )
                {
                    this._session.ServiceRequest += this.OnServiceRequest;
                    // the event handler is installed and the event is enabled when the handler is added, but we enable it again to be sure.
                    // this._session.EnableEvent( EventType.ServiceRequest );
                }
            }

        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( $"Exception {activity}: {exp.Message}" );
        }
    }

    /// <summary>
    /// Pressing Close Session causes the control to release its handle to the device.
    /// </summary>
    /// <remarks>   2026-01-27. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void CloseButton_Click( object? sender, System.EventArgs e )
    {
        if ( this._session is not null ) this._session.ServiceRequest -= this.OnServiceRequest;
        this._session?.Dispose();
        this.InitializeUI();
    }

    /// <summary>
    /// Clicking the Write Button causes the Send String to be written to the device.
    /// </summary>
    /// <remarks>   2026-01-27. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void WriteButton_Click( object? sender, System.EventArgs e )
    {
        try
        {
#if true
            if ( this.serialPollCheckBox.Checked )
            {
                this._statusByteTimer = new System.Timers.Timer( 500 );
                this._statusByteTimer.Elapsed += this.OnTimerEvent;
                this._statusByteTimer.Start();
            }
            else
            {
                // Registering a handler for an event automatically enables that event.
                if ( this._session is not null )
                {
                    // this._session.DisableEvent( EventType.ServiceRequest );
                    this._session.ServiceRequest -= this.OnServiceRequest;
                    this.actualEseLabel.Text = this._session.QueryHex( "*ESE?" );
                    this.actualSreLabel.Text = this._session.QueryHex( "*SRE?" );
                    this._session.ServiceRequest += this.OnServiceRequest;
                    // the event handler is installed and the event is enabled when the handler is added, but we enable it again to be sure.
                    // this._session.EnableEvent( EventType.ServiceRequest );
                    StatusByteFlags? sb = this._session.ReadStatusByte();
                    this.statusLabel.Text = $"0x{( int ) sb:X2}";
                }
            }

#endif
            this._session.WriteLine( this._writeTextBox.Text.ReplaceCommonEscapeSequences() );
            this.readButton.Enabled = true;
            this._enableSRQButton.Enabled = true;
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
    }

    /// <summary>   Event handler. Called by ReadButton for click events. </summary>
    /// <remarks>   2026-01-27. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void ReadButton_Click( object? sender, System.EventArgs e )
    {
        try
        {
            string? textRead = this._session?.RawIO.ReadString();
            this._readTextBox.Text = (textRead ?? string.Empty).InsertCommonEscapeSequences();
            this.readButton.Enabled = false;
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
    }

    /// <summary>   Pressing the Clear button clears the read text box. </summary>
    /// <remarks>   2026-01-27. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void ClearButton_Click( object? sender, System.EventArgs e )
    {
        this._readTextBox.Clear();
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
                this.readButton.Enabled = false;
                if ( sender is IMessageBasedSession mbs )
                {
                    StatusByteFlags? sb = mbs.ReadStatusByte();
                    this.statusLabel.Text = $"0x{( int ) sb:X2}";
                    if ( (sb & StatusByteFlags.MessageAvailable) != 0 )
                    {
                        string? textRead = mbs.RawIO.ReadString();
                        this._readTextBox.Text = (textRead ?? string.Empty).InsertCommonEscapeSequences();
                        // mbs.DiscardEvents( EventType.AllEnabled );
                        //mbs.ServiceRequest += this.OnServiceRequest;
                        // mbs.EnableEvent( EventType.ServiceRequest );
                        sb = mbs.ReadStatusByte();
                        this.statusLabel.Text = $"0x{( int ) sb:X2}";
                        // mbs.Clear();
                    }
                    else
                    {
                        _ = MessageBox.Show( "MAV in status register is not set, which means that message is not available. Make sure the command to enable SRQ is correct, and the instrument is 488.2 compatible." );
                    }
                }
            }
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
    }

    private bool HandleStatusByte( StatusByteFlags sb )
    {
        try
        {
            this.statusLabel.Text = $"0x{( int ) sb:X2}";
            if ( (sb & StatusByteFlags.MessageAvailable) != 0 )
            {
                string? textRead = this._session?.RawIO.ReadString();
                this._readTextBox.Text = (textRead ?? string.Empty).InsertCommonEscapeSequences();
                this.readButton.Enabled = false;
                return true;
            }
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
        return false;
    }

    private void OnTimerEvent( object? sender, System.Timers.ElapsedEventArgs e )
    {
        if ( this._session is null )
        {
            this._statusByteTimer?.Stop();
            return;
        }

        try
        {
            StatusByteFlags sb = this._session.ReadStatusByte();
            if ( this.HandleStatusByte( sb ) )
                this._statusByteTimer?.Stop();
        }
        catch ( Exception exp )
        {
            this._statusByteTimer?.Stop();
            _ = MessageBox.Show( exp.Message );
        }
    }

}
