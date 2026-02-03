using Ivi.Visa;

namespace NI.SimpleReadWrite;

/// <summary> Summary description for Form1. </summary>
/// <remarks> David, 2020-10-11. </remarks>
public partial class MainForm : Form
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Form" /> class.
    /// </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    public MainForm()
    {
        //
        // Required for Windows Form Designer support
        //
        this.InitializeComponent();
        this.SetupControlState( false );

        this.ToolTip?.SetToolTip( this.MultipleResourcesCheckBox, "Check if opening multiple resources" );
        this.ToolTip?.SetToolTip( this.ReadStatusByte, "Read the status byte" );
        this.ToolTip?.SetToolTip( this.UsingTspCheckBox, "Check if using a TSP instrument" );

    }

    /// <summary> Clean up any resources being used. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="disposing"> <see langword="true" /> to release both managed and unmanaged
    ///                          resources; <see langword="false" /> to release only unmanaged
    ///                          resources. </param>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            this._session?.Dispose();
            this._components?.Dispose();
        }

        base.Dispose( disposing );
    }

    #region " open / close sessions "

    /// <summary> The last resource string. </summary>
    private string? _lastResourceString;

    /// <summary> Opens the session. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    private void OpenSession()
    {
        using SelectResource? sr = new();
        if ( this._lastResourceString is not null )
        {
            sr.ResourceName = this._lastResourceString;
        }

        DialogResult result = sr.ShowDialog( this );
        if ( result == DialogResult.OK )
        {
            this._lastResourceString = sr.ResourceName;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                this.ResourceNamesComboBox.DataSource = null;
                this.ResourceNamesComboBox.Items.Clear();
                this.ResourceNamesComboBox.Text = sr.ResourceName;
                this._session = ( IMessageBasedSession ) GlobalResourceManager.Open( sr.ResourceName );
                // setting to 1000 -- 1000; 1001 -- 1, 4000 -- 10000'
                this._session.TerminationCharacter = ( byte ) Math.Round( this.ReadTerminationCharacterNumeric.Value );
                this._session.TerminationCharacterEnabled = this.ReadTerminationEnabledCheckBox.Checked;
                this._session.TimeoutMilliseconds = ( int ) Math.Round( this.TimeoutNumeric.Value );
                this.SetupControlState( true );
            }
            catch ( InvalidCastException e1 )
            {
                _ = MessageBox.Show( $"Resource selected must be a message-based session: {e1}" );
            }
            catch ( Exception exp )
            {
                _ = MessageBox.Show( exp.Message );
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
    }

    /// <summary> Opens the sessions. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    private void OpenSessions()
    {
        using SelectResources sr = new();
        DialogResult result = sr.ShowDialog( this );
        if ( result == DialogResult.OK )
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                this._sessions = [];
                IEnumerable<string> names = sr.ResourceNames;
                this.ResourceNamesComboBox.DataSource = null;
                this.ResourceNamesComboBox.Items.Clear();
                this.ResourceNamesComboBox.DataSource = names;
                System.Text.StringBuilder builder = new();
                foreach ( string rs in names )
                {
                    this._lastResourceString = rs;
                    IMessageBasedSession visaSession = ( IMessageBasedSession ) GlobalResourceManager.Open( rs );
                    // setting to 1000 -- 1000; 1001 -- 1, 4000 -- 10000'
                    visaSession.TimeoutMilliseconds = 1001;
                    this._sessions.Add( visaSession );
                    int solution = 3;
                    if ( this.UsingTspCheckBox.Checked )
                    {
                        // TSP INITIALIZATION CODE:
                        visaSession.RawIO.Write( "_G.status.request_enable=0\n" );
                        if ( solution == 0 )
                        {
                        }
                        // Error
                        // getting error -286 TSP Runtime error
                        // instrument reports User Abort Error
                        // Visa reports USRE ABORT
                        else if ( solution == 1 )
                        {
                        }
                        // still getting error -286 TSP Runtime error
                        // but instrument connects and trace shows no error.
                        else if ( solution == 2 )
                        {
                            // no longer getting error -286 TSP Runtime error
                            Thread.Sleep( 11 );
                        }
                        else if ( solution == 3 )
                        {
                            // no longer getting error -286 TSP Runtime error
                            Thread.Sleep( 1 );
                        }

                    }
                    // this is the culprit:
                    visaSession.Clear();
                    if ( solution == 0 )
                    {
                    }
                    // error
                    else if ( solution == 1 )
                    {
                        Thread.Sleep( 11 );
                    }
                    else if ( solution == 2 )
                    {
                    }
                    else if ( solution == 3 )
                    {
                    }

                    if ( this.UsingTspCheckBox.Checked )
                        visaSession.RawIO.Write( "_G.waitcomplete() _G.print('1')\n" );
                    _ = builder.AppendLine( InsertCommonEscapeSequences( visaSession.RawIO.ReadString() ) );
                }

                this._readTextBox.Text = builder.ToString();
                if ( this._sessions.Count > 0 )
                {
                    this._session = this._sessions[0];
                    this.ResourceNamesComboBox.Text = this._session.ResourceName;
                    this.SetupControlState( this._session is not null );
                }
            }
            catch ( InvalidCastException e1 )
            {
                _ = MessageBox.Show( $"Resource selected must be a message-based session: {e1}" );
            }
            catch ( Exception exp )
            {
                _ = MessageBox.Show( exp.Message );
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
    }

    /// <summary> Opens session click. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void OpenSession_Click( object? sender, EventArgs e )
    {
        if ( this.MultipleResourcesCheckBox.Checked )
        {
            this.OpenSessions();
        }
        else
        {
            this.OpenSession();
        }
    }

    /// <summary> Closes session click. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void CloseSession_Click( object? sender, EventArgs e )
    {
        this.SetupControlState( false );
        if ( this._sessions is null )
        {
            this._session?.Dispose();
        }
        else
        {
            Queue<IMessageBasedSession> sq = new( this._sessions );
            this._sessions.Clear();
            while ( sq.Count > 0 )
            {
                IMessageBasedSession s = sq.Dequeue();
                s.Dispose();
            }
        }

        this._session = null;
    }

    #endregion

    #region " misc "

    /// <summary> Sets up the control state. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="isSessionOpen"> True if session is open, false if not. </param>
    private void SetupControlState( bool isSessionOpen )
    {
        this.OpenSessionButton.Enabled = !isSessionOpen;
        this.CloseSessionButton.Enabled = isSessionOpen;
        this.QueryButton.Enabled = isSessionOpen;
        this.WriteButton.Enabled = isSessionOpen;
        this.ReadButton.Enabled = isSessionOpen;
        this._writeTextBox.Enabled = isSessionOpen;
        this.ClearButton.Enabled = isSessionOpen;
        if ( isSessionOpen )
        {
            // _readTextBox.Text = string.Empty
            _ = this._writeTextBox.Focus();
        }
    }

    /// <summary> Replace common escape sequences. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="s"> The string. </param>
    /// <returns> A <see cref="string" />. </returns>
    private static string ReplaceCommonEscapeSequences( string s )
    {
        return s.Replace( @"\n", "\n" ).Replace( @"\r", "\r" );
    }

    /// <summary> Inserts a common escape sequences described by s. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="s"> The string. </param>
    /// <returns> A <see cref="string" />. </returns>
    private static string InsertCommonEscapeSequences( string s )
    {
        return s.Replace( "\n", @"\n" ).Replace( "\r", @"\r" );
    }

    /// <summary> Multiple resources check box checked changed. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void MultipleResourcesCheckBox_CheckedChanged( object? sender, EventArgs e )
    {
        if ( this.MultipleResourcesCheckBox.Checked )
        {
            this.OpenSessionButton.Text = "Open Sessions";
            this.CloseSessionButton.Text = "Close Sessions";
        }
        else
        {
            this.OpenSessionButton.Text = "Open Session";
            this.CloseSessionButton.Text = "Close Session";
        }
    }

    #endregion

    #region " sessions "

    /// <summary> The session. </summary>
    private IMessageBasedSession? _session;

    /// <summary> The sessions. </summary>
    private List<IMessageBasedSession>? _sessions;

    /// <summary> Resources combo box selected index changed. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ResourcesComboBox_SelectedIndexChanged( object? sender, EventArgs e )
    {
        if ( this._sessions is not null && this.ResourceNamesComboBox.SelectedIndex >= 0 && this._sessions.Count > this.ResourceNamesComboBox.SelectedIndex )
        {
            this._session = this._sessions[this.ResourceNamesComboBox.SelectedIndex];
        }
    }

    #endregion

    #region " read / write "

    /// <summary> Writes. </summary>
    /// <remarks> David, 2021-03-30. </remarks>
    /// <param name="value"> The value to write. </param>
    private void Write( string value )
    {
        this._readTextBox.AppendText( "»" );
        this._session?.RawIO.Write( ReplaceCommonEscapeSequences( value ) );
        this._readTextBox.AppendText( $"{value}{Environment.NewLine}" );
        this._readTextBox.SelectionStart = this._readTextBox.Text.Length;
    }

    /// <summary> Reads this object. </summary>
    /// <remarks> David, 2021-03-30. </remarks>
    private void Read()
    {
        this._readTextBox.AppendText( "«" );
        this._readTextBox.AppendText( $"{InsertCommonEscapeSequences( this._session?.RawIO.ReadString() ?? string.Empty )}{Environment.NewLine}" );
        this._readTextBox.SelectionStart = this._readTextBox.Text.Length;
    }

    /// <summary> Queries a click. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void Query_Click( object? sender, EventArgs e )
    {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
            this.Write( this._writeTextBox.Text );
            this.Read();
        }
        catch ( Exception exp )
        {
            this._readTextBox.AppendText( $"Error: {exp.Message}{Environment.NewLine}" );
            this._readTextBox.SelectionStart = this._readTextBox.Text.Length;
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    /// <summary> Writes a click. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void Write_Click( object? sender, EventArgs e )
    {
        try
        {
            this.Write( this._writeTextBox.Text );
        }
        catch ( Exception exp )
        {
            this._readTextBox.AppendText( $"Error: {exp.Message}{Environment.NewLine}" );
            this._readTextBox.SelectionStart = this._readTextBox.Text.Length;
            // System.Windows.Forms.MessageBox.Show(exp.ToString)
        }
    }

    /// <summary> Reads a click. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void Read_Click( object? sender, EventArgs e )
    {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
            this.Read();
        }
        catch ( Exception exp )
        {
            this._readTextBox.AppendText( $"Error: {exp.Message}{Environment.NewLine}" );
            this._readTextBox.SelectionStart = this._readTextBox.Text.Length;
        }
        // System.Windows.Forms.MessageBox.Show(exp.ToString)
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    /// <summary> Reads status byte click. </summary>
    /// <remarks> David, 2021-03-30. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadStatusByte_Click( object? sender, EventArgs e )
    {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
            this._readTextBox.AppendText( "»Read STB\n" );
            short? status = ( short? ) this._session?.ReadStatusByte();
            if ( status is null )
            {
                this._readTextBox.AppendText( "The ReadStatusByte command returning nothing.\n" );
            }
            else
            {
                this._readTextBox.AppendText( $"«0x{( int ) status:X2}\n" );
            }
            this._readTextBox.SelectionStart = this._readTextBox.Text.Length;
        }
        catch ( Exception exp )
        {
            this._readTextBox.AppendText( $"Error: {exp.Message}{Environment.NewLine}" );
            this._readTextBox.SelectionStart = this._readTextBox.Text.Length;
        }
        // System.Windows.Forms.MessageBox.Show(exp.ToString)
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    /// <summary> Clears the click. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void Clear_Click( object? sender, EventArgs e )
    {
        this._readTextBox.Text = string.Empty;
    }

    private void TimeoutNumeric_ValueChanged( object? sender, EventArgs e )
    {
        _ = this._session?.TimeoutMilliseconds = ( int ) Math.Round( this.TimeoutNumeric.Value );
    }

    private void ReadTerminationCharacter_ValueChanged( object? sender, EventArgs e )
    {
        if ( this._session is not null )
        {
            this._session.TerminationCharacter = ( byte ) Math.Round( this.ReadTerminationCharacterNumeric.Value );
            this._session.TerminationCharacterEnabled = this.ReadTerminationEnabledCheckBox.Checked;
        }
    }

    #endregion
}
