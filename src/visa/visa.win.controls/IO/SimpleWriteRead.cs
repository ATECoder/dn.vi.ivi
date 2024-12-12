using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Ivi.Visa;

namespace cc.isr.Visa.WinControls;

/// <summary>   A simple write/read. user control </summary>
/// <remarks>   David, 2021-07-17. </remarks>
public partial class SimpleWriteRead : UserControl
{
    #region " construction and cleanup "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    public SimpleWriteRead()
    {
        this.InitializeComponent();
        this.SetupControlState( false );
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( this._sessions is not null )
            {
                Queue<IMessageBasedSession> sq = new( this._sessions );
                this._sessions.Clear();
                while ( sq.Count > 0 )
                {
                    IMessageBasedSession s = sq.Dequeue();
                    s?.Dispose();
                }
            }

            this._session?.Dispose();
            this._session = null;

            this.components?.Dispose();
        }

        base.Dispose( disposing );
    }

    #endregion

    #region " event handlers "

    /// <summary>   Event handler. Called by OpenSessionButton for click events. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void OpenSessionButton_Click( object? sender, EventArgs e )
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

    /// <summary>   Event handler. Called by CloseSessionButton for click events. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void CloseSessionButton_Click( object? sender, EventArgs e )
    {
        this.CloseSessions();
    }

    /// <summary>   Event handler. Called by ReadStatusByteButton for click events. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void ReadStatusByteButton_Click( object? sender, EventArgs e )
    {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
            this.ReadStatusByte();
        }
        catch ( Exception ex )
        {
            this.ReadTextBox.AppendText( $"Error: {ex.Message}\n" );
            this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    /// <summary>   Event handler. Called by QueryButton for click events. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void QueryButton_Click( object? sender, EventArgs e )
    {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
            this.Write( this.WriteTextBox.Text );
            this.Read();
        }
        catch ( Exception ex )
        {
            this.ReadTextBox.AppendText( $"Error: {ex.Message}\n" );
            this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    /// <summary>   Event handler. Called by WriteButton for click events. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void WriteButton_Click( object? sender, EventArgs e )
    {
        try
        {
            this.Write( this.WriteTextBox.Text );
        }
        catch ( Exception ex )
        {
            this.ReadTextBox.AppendText( $"Error: {ex.Message}\n" );
            this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
        }
    }

    /// <summary>   Event handler. Called by ReadButton for click events. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void ReadButton_Click( object? sender, EventArgs e )
    {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
            this.Read();
        }
        catch ( Exception ex )
        {
            this.ReadTextBox.AppendText( $"Error: {ex.Message}\n" );
            this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    /// <summary>
    /// Event handler. Called by MultipleResourcesCheckBox_CheckedChanged for 1 events.
    /// </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void MultipleResourcesCheckBox_CheckedChanged_1( object? sender, EventArgs e )
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

    /// <summary>
    /// Event handler. Called by ResourceNamesComboBox for selected index changed events.
    /// </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void ResourceNamesComboBox_SelectedIndexChanged( object? sender, EventArgs e )
    {
        if ( this._sessions is not null && this.ResourceNamesComboBox.SelectedIndex >= 0 && this._sessions.Count > this.ResourceNamesComboBox.SelectedIndex )
        {
            this._session = this._sessions[this.ResourceNamesComboBox.SelectedIndex];
        }
    }

    /// <summary>   Event handler. Called by ClearButton for click events. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void ClearButton_Click( object? sender, EventArgs e )
    {
        this.ReadTextBox.Text = string.Empty;
    }

    /// <summary>
    /// Event handler. Called by ReadTerminationCharacterNumeric for value changed events.
    /// </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void ReadTerminationCharacterNumeric_ValueChanged( object? sender, EventArgs e )
    {
        this.ApplyReadTerminationCharacter( this.ReadTerminationCharacterNumeric.Value, this.ReadTerminationEnabledCheckBox.Checked );
    }

    /// <summary>
    /// Event handler. Called by ReadTerminationEnabledCheckBox for checked changed events.
    /// </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void ReadTerminationEnabledCheckBox_CheckedChanged( object? sender, EventArgs e )
    {
        this.ApplyReadTerminationCharacter( this.ReadTerminationCharacterNumeric.Value, this.ReadTerminationEnabledCheckBox.Checked );
    }

    /// <summary>   Event handler. Called by TimeoutNumeric for value changed events. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    private void TimeoutNumeric_ValueChanged( object? sender, EventArgs e )
    {
        this.ApplyTimeoutMilliseconds( this.TimeoutNumeric.Value );
    }

    #endregion

    #region " open / close sessions "

    /// <summary> The last resource string. </summary>
    private string? _lastResourceString;

    /// <summary> Opens the session. </summary>
    /// <remarks> David, 2020-10-11. </remarks>
    private void OpenSession()
    {
        using ResourceNameSelectorForm sr = new();
        if ( this._lastResourceString is not null )
        {
            sr.ResourceName = this._lastResourceString;
        }

        DialogResult result = sr.ShowDialog( this );
        if ( result == DialogResult.OK )
        {
            this._lastResourceString = sr.SelectedResourceName;
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
                this.ReadTextBox.AppendText( $"Resource selected must be a message-based session:  {e1.Message}\n" );
                this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
            }
            catch ( Exception ex )
            {
                this.ReadTextBox.AppendText( $"Error: {ex.Message}\n" );
                this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
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
        using ResourceNamesSelectorForm sr = new();
        DialogResult result = sr.ShowDialog( this );
        if ( result == DialogResult.OK )
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                this._sessions = [];
                IEnumerable<string>? names = sr.SelectedResourceNames;
                if ( names is null ) { return; }

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

                this.ReadTextBox.Text = builder.ToString();
                if ( this._sessions.Count > 0 )
                {
                    this._session = this._sessions[0];
                    this.ResourceNamesComboBox.Text = this._session.ResourceName;
                    this.SetupControlState( this._session is object );
                }
            }
            catch ( InvalidCastException e1 )
            {
                this.ReadTextBox.AppendText( $"Resource selected must be a message-based session: {e1}\n" );
                this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
            }
            catch ( Exception ex )
            {
                this.ReadTextBox.AppendText( $"Error: {ex.Message}\n" );
                this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
    }

    /// <summary>   Closes the sessions. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    private void CloseSessions()
    {
        this.SetupControlState( false );
        if ( this._sessions is not null )
        {
            Queue<IMessageBasedSession> sq = new( this._sessions );
            this._sessions.Clear();
            while ( sq.Count > 0 )
            {
                IMessageBasedSession s = sq.Dequeue();
                s.Dispose();
            }
        }
        else if ( this._session is object )
        {
            this._session.Dispose();
            this._session = null;
        }
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
        this.WriteTextBox.Enabled = isSessionOpen;
        this.ReadStatusByteButton.Enabled = isSessionOpen;
        this.ClearButton.Enabled = true;
        if ( isSessionOpen )
        {
            _ = this.WriteTextBox.Focus();
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

    #endregion

    #region " sessions "

    /// <summary> The session. </summary>
    private IMessageBasedSession? _session;

    /// <summary> The sessions. </summary>
    private List<IMessageBasedSession>? _sessions;

    #endregion

    #region " read / write "

    /// <summary> Writes a session message. </summary>
    /// <remarks> David, 2021-03-30. </remarks>
    /// <param name="value"> The value to write. </param>
    private void Write( string value )
    {
        this.ReadTextBox.AppendText( "Write: " );
        this._session?.RawIO.Write( ReplaceCommonEscapeSequences( value ) );
        this.ReadTextBox.AppendText( $"{value}\n" );
        this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
    }

    /// <summary> Reads a session message. </summary>
    /// <remarks> David, 2021-03-30. </remarks>
    private void Read()
    {
        this.ReadTextBox.AppendText( "Read: " );
        this.ReadTextBox.AppendText( $"{InsertCommonEscapeSequences( this._session?.RawIO.ReadString() ?? string.Empty )}\n" );
        this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
    }

    /// <summary>   Reads status byte. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    private void ReadStatusByte()
    {
        this.ReadTextBox.AppendText( "Status: " );
        int? status = ( int? ) this._session?.ReadStatusByte();
        this.ReadTextBox.AppendText( $"{status:X2}\n" );
        this.ReadTextBox.SelectionStart = this.ReadTextBox.Text.Length;
    }

    /// <summary>   Applies the timeout milliseconds described by timeoutMilliseconds. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="timeoutMilliseconds">  The timeout in milliseconds. </param>
    private void ApplyTimeoutMilliseconds( decimal timeoutMilliseconds )
    {
        this.ApplyTimeoutMilliseconds( ( int ) Math.Round( timeoutMilliseconds ) );
    }

    /// <summary>   Applies the timeout milliseconds described by timeoutMilliseconds. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="timeoutMilliseconds">  The timeout in milliseconds. </param>
    private void ApplyTimeoutMilliseconds( int timeoutMilliseconds )
    {
        if ( this._session is object )
        {
            this._session.TimeoutMilliseconds = timeoutMilliseconds;
        }
    }

    /// <summary>   Applies the read termination character. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="terminationCharacter"> The termination character. </param>
    /// <param name="enabled">              True to enable, false to disable. </param>
    private void ApplyReadTerminationCharacter( decimal terminationCharacter, bool enabled )
    {
        this.ApplyReadTerminationCharacter( ( byte ) Math.Round( terminationCharacter ), enabled );
    }

    /// <summary>   Applies the read termination character. </summary>
    /// <remarks>   David, 2021-07-17. </remarks>
    /// <param name="terminationCharacter"> The termination character. </param>
    /// <param name="enabled">              True to enable, false to disable. </param>
    private void ApplyReadTerminationCharacter( byte terminationCharacter, bool enabled )
    {
        if ( this._session is object )
        {
            this._session.TerminationCharacter = terminationCharacter;
            this._session.TerminationCharacterEnabled = enabled;
        }
    }

    #endregion
}
