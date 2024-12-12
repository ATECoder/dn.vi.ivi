using Ivi.Visa;

namespace NI.SimpleAsynchronousReadWrite;
/// <summary>
/// Summary description for Form1.
/// </summary>
public partial class MainForm : System.Windows.Forms.Form
{
    private string? _lastResourceString;
    private IVisaAsyncResult? _asyncHandle;
    private IMessageBasedSession? _mbSession;

    public MainForm()
    {
        //
        // Required for Windows Form Designer support
        //
        this.InitializeComponent();
        this.SetupControlState( false );
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            this._mbSession?.Dispose();
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

    private void OpenSession_Click( object? sender, System.EventArgs e )
    {
        using SelectResourceDialog sr = new();
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
                this._mbSession = ( IMessageBasedSession ) Ivi.Visa.GlobalResourceManager.Open( sr.ResourceName );
                // Use SynchronizeCallbacks to specify that the object marshals callbacks across threads appropriately.
                this._mbSession.SynchronizeCallbacks = true;
                this.SetupControlState( true );
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

    private void CloseSession_Click( object? sender, System.EventArgs e )
    {
        this.SetupControlState( false );
        this._mbSession?.Dispose();
    }

    private readonly System.Diagnostics.Stopwatch _stopWatch = new();

    private void Write_Click( object? sender, System.EventArgs e )
    {
        try
        {
            this._stopWatch.Restart();
            this.SetupWaitingControlState( true );
            string? textToWrite = ReplaceCommonEscapeSequences( this._writeTextBox.Text );
            this._asyncHandle = this._mbSession?.RawIO.BeginWrite(
                textToWrite, new VisaAsyncCallback( this.OnWriteComplete ), textToWrite?.Length );
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
    }

    private void Read_Click( object? sender, System.EventArgs e )
    {
        try
        {
            this._stopWatch.Restart();
            this.SetupWaitingControlState( true );
            this._asyncHandle = this._mbSession?.RawIO.BeginRead(
                1024,
                new VisaAsyncCallback( this.OnReadComplete ),
                null );
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
    }

    private void Clear_Click( object? sender, System.EventArgs e )
    {
        this.ClearControls();
    }

    private void Terminate_Click( object? sender, System.EventArgs e )
    {
        this.SetupWaitingControlState( false );
        try
        {
            this._mbSession?.RawIO.AbortAsyncOperation( this._asyncHandle );
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
        }
    }

    private void OnWriteComplete( IVisaAsyncResult result )
    {
        try
        {
            this._stopWatch.Stop();
            this._elapsedLabel.Text = this._stopWatch.Elapsed.TotalMilliseconds.ToString( "0", System.Globalization.CultureInfo.CurrentCulture );
            this.SetupWaitingControlState( false );
            _ = this._mbSession?.RawIO.EndWrite( result );
            this._lastIOStatusTextBox.Text = "Success";
        }
        catch ( Exception exp )
        {
            this._readTextBox.Text = exp.Message;
        }
        this._elementsTransferredTextBox.Text = $"{result.Count}";
    }

    private void OnReadComplete( IVisaAsyncResult result )
    {
        try
        {
            this._stopWatch.Stop();
            this._elapsedLabel.Text = $"{this._stopWatch.Elapsed.TotalMilliseconds}";
            this.SetupWaitingControlState( false );
            string? responseString = this._mbSession?.RawIO.EndReadString( result );
            this._readTextBox.Text = InsertCommonEscapeSequences( responseString ?? string.Empty );
            this._lastIOStatusTextBox.Text = "Success";
        }
        catch ( Exception exp )
        {
            this._readTextBox.Text = exp.Message;
        }
        this._elementsTransferredTextBox.Text = $"{result.Count}";
    }

    private void SetupControlState( bool isSessionOpen )
    {
        this._openSessionButton.Enabled = !isSessionOpen;
        this._closeSessionButton.Enabled = isSessionOpen;
        this._writeButton.Enabled = isSessionOpen;
        this._readButton.Enabled = isSessionOpen;
        this._writeTextBox.Enabled = isSessionOpen;
        this._clearButton.Enabled = isSessionOpen;
        if ( isSessionOpen )
        {
            this.ClearControls();
            _ = this._writeTextBox.Focus();
        }
    }

    private void SetupWaitingControlState( bool operationIsInProgress )
    {
        if ( operationIsInProgress )
        {
            this._readTextBox.Text = string.Empty;
            this._elementsTransferredTextBox.Text = string.Empty;
            this._lastIOStatusTextBox.Text = string.Empty;
            this._elapsedLabel.Text = "0";
        }
        this._terminateButton.Enabled = operationIsInProgress;
        this._writeButton.Enabled = !operationIsInProgress;
        this._readButton.Enabled = !operationIsInProgress;
    }

    private static string? ReplaceCommonEscapeSequences( string s )
    {
        return (s != null) ? s.Replace( "\\n", "\n" ).Replace( "\\r", "\r" ) : s;
    }

    private static string? InsertCommonEscapeSequences( string s )
    {
        return (s != null) ? s.Replace( "\n", "\\n" ).Replace( "\r", "\\r" ) : s;
    }

    private void ClearControls()
    {
        this._readTextBox.Text = string.Empty;
        this._lastIOStatusTextBox.Text = string.Empty;
        this._elementsTransferredTextBox.Text = string.Empty;
    }
}
