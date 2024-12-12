using System;
using System.Windows.Forms;
using Ivi.Visa;

namespace cc.isr.Visa.WinControls;

/// <summary>   A service request. </summary>
/// <remarks>   David, 2021-12-08. </remarks>
public partial class ServiceRequest : UserControl
{
    #region " construction and cleanup "

    private IMessageBasedSession? _session;

    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-12-08. </remarks>
    public ServiceRequest()
    {
        this.InitializeComponent();
        this.InitializeUI();
        this.ListResourceNames();
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

    #endregion

    #region " user interfance management functions "

    /// <summary>   List resource names. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    private void ListResourceNames()
    {
        try
        {
            // uses static methods provided by the Ivi.Visa.ResourceManager class to find resources on this system.
            System.Collections.Generic.IEnumerable<string> validResources = Ivi.Visa.GlobalResourceManager.Find( "(GPIB|TCPIP|USB)?*INSTR" );
            foreach ( string? resource in validResources )
            {
                _ = this.ResourceNameComboBox.Items.Add( resource );
            }
        }
        catch ( Exception )
        {
            _ = this.ResourceNameComboBox.Items.Add( "No 488.2 INSTR resource found on the system" );
            this.UpdateResourceNameControls( true );
            this.CloseButton.Enabled = false;
        }
        this.ResourceNameComboBox.SelectedIndex = 0;
    }

    /// <summary>   Updates the resource name controls described by enable. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="enable">   True to enable, false to disable. </param>
    private void UpdateResourceNameControls( bool enable )
    {
        this.ResourceNameComboBox.Enabled = enable;
        this.OpenButton.Enabled = enable;
        this.CloseButton.Enabled = !enable;
        if ( enable )
        {
            _ = this.OpenButton.Focus();
        }
    }

    /// <summary>   Updates the srq controls described by enable. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="enable">   True to enable, false to disable. </param>
    private void UpdateSRQControls( bool enable )
    {
        this.CommandTextBox.Enabled = enable;
        this.EnableSRQButton.Enabled = enable;
        if ( enable )
        {
            _ = this.EnableSRQButton.Focus();
        }
    }

    /// <summary>   Updates the write controls described by enable. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="enable">   True to enable, false to disable. </param>
    private void UpdateWriteControls( bool enable )
    {
        this.WriteTextBox.Enabled = enable;
        this.WriteButton.Enabled = enable;
        if ( enable )
        {
            _ = this.WriteButton.Focus();
        }
    }

    /// <summary>   Initializes the user interface. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    private void InitializeUI()
    {
        this.UpdateResourceNameControls( true );
        this.UpdateSRQControls( false );
        this.UpdateWriteControls( false );
    }

    #endregion

    #region " control event handlers "

    /// <summary>
    /// When the Open Session button is pressed, the resource name of the device is set and the
    /// control attempts to connect to the device.
    /// </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void OpenButton_Click( object? sender, System.EventArgs e )
    {
        try
        {
            this._session = ( Ivi.Visa.IMessageBasedSession ) Ivi.Visa.GlobalResourceManager.Open( this.ResourceNameComboBox.Text );
            // Use SynchronizeCallbacks to specify that the object marshals callbacks across threads appropriately.
            this._session.SynchronizeCallbacks = true;
            this._session.TerminationCharacter = ( byte ) this.ReadTerminationCharacterNumeric.Value;
            this._session.TerminationCharacterEnabled = this.ReadTerminationEnabledCheckBox.Checked;

            this.UpdateResourceNameControls( false );
            this.UpdateSRQControls( true );
        }
        catch ( Exception ex )
        {
            this.ReadTextBox.AppendText( $"Error: {ex.Message}{Environment.NewLine}" );
            this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
        }
    }

    /// <summary>
    /// The Enable SRQ button writes the string that tells the instrument to enable the SRQ bit.
    /// </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void EnableSRQButton_Click( object? sender, System.EventArgs e )
    {
        string activity = "adding SRQ handler";
        try
        {   // Registering a handler for an event automatically enables that event.

            if ( this._session is not null )
                this._session.ServiceRequest += this.OnServiceRequest;
            activity = $"writing {this.CommandTextBox.Text}";
            this.WriteToSession( this.CommandTextBox.Text );
            activity = "updating SRQ Controls";
            this.UpdateSRQControls( false );
            activity = "updating Write Controls";
            this.UpdateWriteControls( true );
        }
        catch ( Exception ex )
        {
            this.ReadTextBox.AppendText( $"Error: {ex.Message}{Environment.NewLine}" );
            this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
        }
    }

    /// <summary>
    /// Pressing Close Session causes the control to release its handle to the device.
    /// </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void CloseButton_Click( object? sender, System.EventArgs e )
    {
        if ( this._session is not null )
            this._session.ServiceRequest -= this.OnServiceRequest;
        this._session?.Dispose();
        this.InitializeUI();
    }

    /// <summary>
    /// Clicking the Write Button causes the Send String to be written to the device.
    /// </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void WriteButton_Click( object? sender, System.EventArgs e )
    {
        this.WriteToSession( this.WriteTextBox.Text );
    }

    /// <summary>   Pressing the Clear button clears the read text box. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void ClearButton_Click( object? sender, System.EventArgs e )
    {
        this.ReadTextBox.Clear();
    }

    #endregion

    #region " helper functions "

    /// <summary>   Replace common escape sequences. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="s">    The string. </param>
    /// <returns>   A <see cref="string" />. </returns>
    private static string ReplaceCommonEscapeSequences( string s )
    {
        return s.Replace( "\\n", "\n" ).Replace( "\\r", "\r" );
    }

    /// <summary>   Inserts a common escape sequences described by s. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="s">    The string. </param>
    /// <returns>   A <see cref="string" />. </returns>
    private static string InsertCommonEscapeSequences( string s )
    {
        return s.Replace( "\n", "\\n" ).Replace( "\r", "\\r" );
    }

    /// <summary>   Writes to session. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="txtWrite"> The text write. </param>
    private void WriteToSession( string txtWrite )
    {
        try
        {
            string textToWrite = ServiceRequest.ReplaceCommonEscapeSequences( txtWrite );
            this._session?.RawIO.Write( textToWrite );
        }
        catch ( Exception ex )
        {
            this.ReadTextBox.AppendText( $"Error: {ex.Message}{Environment.NewLine}" );
            this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
        }
    }

    /// <summary>   Raises the visa event. </summary>
    /// <remarks>   David, 2021-07-26. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information to send to registered event handlers. </param>
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
                IMessageBasedSession? mbs = ( IMessageBasedSession? ) sender;
                StatusByteFlags? sb = mbs?.ReadStatusByte();

                if ( (sb & StatusByteFlags.MessageAvailable) != 0 )
                {
                    string? textRead = mbs?.RawIO.ReadString();
                    this.ReadTextBox.Text = ServiceRequest.InsertCommonEscapeSequences( textRead ?? string.Empty );
                }
                else
                {
                    _ = MessageBox.Show( "MAV in status register is not set, which means that message is not available. Make sure the command to enable SRQ is correct, and the instrument is 488.2 compatible." );
                }
            }
        }
        catch ( Exception ex )
        {
            this.ReadTextBox.AppendText( $"Error: {ex.Message}{Environment.NewLine}" );
            this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
        }
    }

    #endregion
}
