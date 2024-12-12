using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using cc.isr.Std.EscapeSequencesExtensions;

namespace cc.isr.VI.DeviceWinControls;

/// <summary> A simple read write control. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-12-29 </para>
/// </remarks>
public partial class SessionView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    public SessionView() : base()
    {
        this.InitializingComponents = true;
        this.InitializeComponent();
        this.BindSettings();
        this.RefreshCommandListThis();
        this.OnConnectionChanged();
        this.InitializingComponents = false;
        this.AppendTermination = true;
        this._readTextBox.Text = $@"'Append Termination' is {(this.AppendTermination ? "" : "un")}checked--Termination characters are {(this.AppendTermination ? "not" : "")}required;
Write *ESE 255{Properties.Settings.Instance.Termination} if opening a stand alone VISA Session";
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control" />
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
                this.InitializingComponents = true;
                if ( this.components is not null )
                    this.components?.Dispose();
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " bind settings "

    /// <summary> Bind settings. </summary>
    private void BindSettings()
    {
        this._statusReadDelayNumeric.NumericUpDown.Minimum = 0m;
        this._statusReadDelayNumeric.NumericUpDown.Maximum = Properties.Settings.Instance.MaximumDelay;
        _ = this._statusReadDelayNumeric.NumericUpDown.DataBindings.Add( nameof( NumericUpDown.Maximum ), Properties.Settings.Instance, nameof( Properties.Settings.Instance.MaximumDelay ) );
        this._statusReadDelayNumeric.NumericUpDown.DecimalPlaces = 0;
        this._statusReadDelayNumeric.NumericUpDown.Value = Properties.Settings.Instance.StatusReadDelay;
        _ = this._statusReadDelayNumeric.NumericUpDown.DataBindings.Add( nameof( NumericUpDown.Value ), Properties.Settings.Instance, nameof( Properties.Settings.Instance.StatusReadDelay ) );
        this._readDelayNumeric.NumericUpDown.Minimum = 0m;
        this._readDelayNumeric.NumericUpDown.Maximum = Properties.Settings.Instance.MaximumDelay;
        _ = this._readDelayNumeric.NumericUpDown.DataBindings.Add( nameof( NumericUpDown.Maximum ), Properties.Settings.Instance, nameof( Properties.Settings.Instance.MaximumDelay ) );
        this._readDelayNumeric.NumericUpDown.DecimalPlaces = 0;
        this._readDelayNumeric.NumericUpDown.Value = Properties.Settings.Instance.ReadAfterWriteDelay;
        _ = this._readDelayNumeric.NumericUpDown.DataBindings.Add( nameof( NumericUpDown.Value ), Properties.Settings.Instance, nameof( Properties.Settings.Instance.ReadAfterWriteDelay ) );

        // Me._termination = Std.EscapeSequencesExtensions.NEW_LINE_ESCAPE
        this._termination = Properties.Settings.Instance.Termination;
    }

    #endregion

    #region " visa session base (device base) "

    /// <summary> Gets the visa session base. </summary>
    /// <value> The visa session base. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? VisaSessionBase { get; private set; }

    /// <summary> Binds the visa session to its controls. </summary>
    /// <param name="visaSessionBase"> The visa session view model. </param>
    public void BindVisaSessionBase( VisaSessionBase? visaSessionBase )
    {
        if ( this.VisaSessionBase is not null )
        {
            this.VisaSessionBase.PropertyChanged -= this.VisaSessionPropertyChanged;
            this.VisaSessionBase = null;
        }

        if ( visaSessionBase is not null )
        {
            this.VisaSessionBase = visaSessionBase;
            this.VisaSessionBase.PropertyChanged += this.VisaSessionPropertyChanged;
        }

        this.BindSessionBase( visaSessionBase );
    }

    /// <summary> Handles the session property changed action. </summary>
    /// <param name="sender">       Source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    protected virtual void HandlePropertyChanged( VisaSessionBase? sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( VI.VisaSessionBase.ServiceRequestReading ):
                {
                    if ( this._showServiceRequestReadingMenuItem.Checked )
                    {
                        this.UpdateReadMessage( sender.ServiceRequestReading );
                    }

                    break;
                }

            case nameof( VI.VisaSessionBase.PollReading ):
                {
                    if ( this._showPollReadingsMenuItem.Checked )
                    {
                        this.UpdateReadMessage( sender.PollReading );
                    }

                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Handles the Session property changed event. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void VisaSessionPropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null )
            return;
        string activity = $"handling {nameof( VI.VisaSessionBase )}.{e.PropertyName} change";
        try
        {
            this.HandlePropertyChanged( sender as VisaSessionBase, e.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " session base "

    /// <summary> Gets the session. </summary>
    /// <value> The session. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    protected Pith.SessionBase? SessionBase { get; private set; }

    /// <summary> Binds the Session base to its controls. </summary>
    /// <param name="visaSessionBase"> The visa session base. </param>
    private void BindSessionBase( VisaSessionBase? visaSessionBase )
    {
        if ( visaSessionBase is null )
        {
            this.BindSessionBaseThis( null );
        }
        else
        {
            this.BindSessionBaseThis( visaSessionBase.Session );
        }
    }

    /// <summary> Bind session base. </summary>
    /// <param name="sessionBase"> The session. </param>
    private void BindSessionBaseThis( Pith.SessionBase? sessionBase )
    {
        if ( this.SessionBase is not null )
        {
            this.SessionBase.StatusPrompt = $"Releasing session {this.SessionBase.ResourceNameCaption}";
            this.BindSessionViewModel( false, this.SessionBase );
            this.SessionBase = null;
        }

        if ( sessionBase is not null )
        {
            this.SessionBase = sessionBase;
            this.BindSessionViewModel( true, this.SessionBase );
        }

        this.OnConnectionChanged();
    }

    /// <summary> Gets the sentinel indication having an open session. </summary>
    /// <value> The is open. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public bool IsSessionOpen => this.SessionBase is not null && this.SessionBase.IsSessionOpen;

    /// <summary> Executes the connection changed action. </summary>
    private void OnConnectionChanged()
    {
        this._clearSessionMenuItem.Enabled = this.IsSessionOpen;
        this._queryButton.Enabled = this.IsSessionOpen;
        this._readButton.Enabled = this.IsSessionOpen;
        this._writeButton.Enabled = this.IsSessionOpen;
        this._readStatusMenuItem.Enabled = this.IsSessionOpen;
        this._eraseDisplayMenuItem.Enabled = true;
        if ( this.IsSessionOpen )
        {
            this.SessionBase!.StatusPrompt = "Session Open";
            this.ReadStatusRegister();
        }
        else if ( this.SessionBase is not null )
        {
            this.SessionBase.StatusPrompt = "Session Not open; open session";
        }
    }

    /// <summary> Bind session view model. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="viewModel"> The session view model. </param>
    private void BindSessionViewModel( bool add, Pith.SessionBase viewModel )
    {
        _ = this.AddRemoveBinding( this._clearSessionMenuItem, add, nameof( this.Enabled ), viewModel, nameof( Pith.SessionBase.IsDeviceOpen ) );
        _ = this.AddRemoveBinding( this._queryButton, add, nameof( this.Enabled ), viewModel, nameof( Pith.SessionBase.IsDeviceOpen ) );
        _ = this.AddRemoveBinding( this._readButton, add, nameof( this.Enabled ), viewModel, nameof( Pith.SessionBase.IsDeviceOpen ) );
        _ = this.AddRemoveBinding( this._writeButton, add, nameof( this.Enabled ), viewModel, nameof( Pith.SessionBase.IsDeviceOpen ) );
        _ = this.AddRemoveBinding( this._readStatusMenuItem, add, nameof( this.Enabled ), viewModel, nameof( Pith.SessionBase.IsDeviceOpen ) );
        this._eraseDisplayMenuItem.Enabled = true;
    }

    /// <summary> Reads delay numeric value changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadDelayNumeric_ValueChanged( object? sender, EventArgs e )
    {
        if ( this.SessionBase is not null )
        {
            this.SessionBase.ReadAfterWriteDelay = TimeSpan.FromMilliseconds( ( double ) this._readDelayNumeric.Value );
        }
    }

    /// <summary> Status read delay numeric value changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void StatusReadDelayNumeric_ValueChanged( object? sender, EventArgs e )
    {
        if ( this.SessionBase is not null )
        {
            this.SessionBase.StatusReadDelay = TimeSpan.FromMilliseconds( ( double ) this._statusReadDelayNumeric.Value );
        }
    }

    #endregion

    #region " commands "

    /// <summary> The termination. </summary>
    private string? _termination;

    /// <summary> Gets or sets the termination. </summary>
    /// <value> The termination. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public string? Termination
    {
        get => this._termination;
        set
        {
            if ( !string.Equals( this.Termination, value, StringComparison.Ordinal ) )
            {
                this._termination = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Dictionary of commands. </summary>
    private Dictionary<string, string>? _commandDictionary;

    /// <summary> Command dictionary. </summary>
    /// <returns> A Dictionary(Of String, String) </returns>
#if NET5_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Performance", "CA1863:Use 'CompositeFormat'", Justification = "<Pending>" )]
#endif
    private Dictionary<string, string>? CommandDictionary()
    {
        if ( this._commandDictionary is null )
        {
            this._commandDictionary = [];
            this.AddCommandThis( Syntax.Ieee488Syntax.ClearExecutionStateCommand );
            this.AddCommandThis( Syntax.Ieee488Syntax.IdentificationQueryCommand );
            this.AddCommandThis( Syntax.Ieee488Syntax.OperationCompleteCommand );
            this.AddCommandThis( Syntax.Ieee488Syntax.OperationCompletedQueryCommand );
            this.AddCommandThis( Syntax.Ieee488Syntax.ResetKnownStateCommand );
            this.AddCommandThis( string.Format( System.Globalization.CultureInfo.CurrentCulture, Syntax.Ieee488Syntax.ServiceRequestEnableCommandFormat, 255 ) );
            this.AddCommandThis( Syntax.Ieee488Syntax.ServiceRequestEnableQueryCommand );
            this.AddCommandThis( string.Format( System.Globalization.CultureInfo.CurrentCulture, Syntax.Ieee488Syntax.StandardEventEnableCommandFormat, 255 ) );
            this.AddCommandThis( Syntax.Ieee488Syntax.StandardEventEnableQueryCommand );
            this.AddCommandThis( Syntax.Ieee488Syntax.WaitCommand );
        }

        return this._commandDictionary;
    }

    /// <summary> Clears the commands. </summary>
    public void ClearCommands()
    {
        this._commandDictionary?.Clear();
    }

    /// <summary> Inserts a command. </summary>
    /// <param name="value">       The value. </param>
    /// <param name="termination"> The termination. </param>
    private void InsertCommandThis( string value, string termination )
    {
        if ( this.CommandDictionary() is not null && !this._commandDictionary!.ContainsKey( value ) )
        {
            Dictionary<string, string> dix = new( this._commandDictionary! );
            this._commandDictionary.Clear();
            this._commandDictionary.Add( value, value + termination );
            foreach ( string key in dix.Keys )
                this._commandDictionary.Add( key, dix[key] );
        }
    }

    /// <summary> Inserts a command. </summary>
    /// <param name="value"> The value. </param>
    private void InsertCommandThis( string value )
    {
        this.InsertCommandThis( value, Properties.Settings.Instance.Termination );
    }

    /// <summary> Inserts a command described by value. </summary>
    /// <param name="value"> The value. </param>
    public void InsertCommand( string value )
    {
        this.InsertCommandThis( value );
    }

    /// <summary> Adds a command. </summary>
    /// <param name="value">       The value. </param>
    /// <param name="termination"> The termination. </param>
    private void AddCommandThis( string value, string termination )
    {
        if ( this.CommandDictionary() is not null && !this._commandDictionary!.ContainsKey( value ) )
        {
            this._commandDictionary.Add( value, value + termination );
        }
    }

    /// <summary> Adds a command. </summary>
    /// <param name="value"> The value. </param>
    private void AddCommandThis( string value )
    {
        this.AddCommandThis( value, Properties.Settings.Instance.Termination );
    }

    /// <summary> Adds a command. </summary>
    /// <param name="value"> The value. </param>
    public void AddCommand( string value )
    {
        this.AddCommandThis( value );
    }

    /// <summary> Refresh command list. </summary>
    private void RefreshCommandListThis()
    {
        if ( this.CommandDictionary() is not null )
        {
            this._writeComboBox.Items.Clear();
            this._writeComboBox.Items.AddRange( [.. this._commandDictionary!.Values] );
            this._writeComboBox.SelectedIndex = 1;
        }
    }

    /// <summary> Refresh command list. </summary>
    public void RefreshCommandList()
    {
        this.RefreshCommandListThis();
    }

    #endregion

    #region " read and write "

    /// <summary> Reads the status register and handle any errors if 
    ///           the service request enable bits are set. </summary>
    public void ReadStatusRegister()
    {
        string activity = string.Empty;
        try
        {
            if ( this.SessionBase is not null && this.IsSessionOpen )
            {
                activity = $"reading status byte after {this.SessionBase.StatusReadDelay.TotalMilliseconds}ms delay";
                _ = Pith.SessionBase.AsyncDelay( this.SessionBase.StatusReadDelay );
                this.SessionBase!.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.ApplyStatusByte( this.SessionBase.ReadStatusByte() );
            }
        }
        catch ( Exception ex )
        {
            this._readTextBox.Text = ex.ToString();
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Gets or sets the append termination to the entered commands. </summary>
    /// <value> The append termination sentinel. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public bool AppendTermination
    {
        get => this._appendTerminationMenuItem.Checked;
        set => this._appendTerminationMenuItem.Checked = value;
    }

    /// <summary> Queries. </summary>
    /// <param name="textToWrite"> The text to write. </param>
    public void Query( string textToWrite )
    {
        this.Write( textToWrite );
        this.Read();
    }

    /// <summary> Writes. </summary>
    /// <param name="textToWrite"> The text to write. </param>
    public void Write( string textToWrite )
    {
        Cursor.Current = Cursors.WaitCursor;
        string activity = string.Empty;
        if ( !this.IsSessionOpen ) return;
        try
        {
            activity = $"writing {textToWrite}";
            this.SessionBase!.StatusPrompt = activity;
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.SessionBase.StartElapsedStopwatch();
            if ( this.AppendTermination )
                textToWrite = textToWrite.RemoveCommonEscapeSequences();
            else
                textToWrite = textToWrite.ReplaceCommonEscapeSequences();
            textToWrite = textToWrite.TrimEnd( ['\n'] );
            _ = this.SessionBase.WriteLine( textToWrite );
            this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime();

            activity = $"done {activity}";
            this.SessionBase.StatusPrompt = activity;
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
        }
        catch ( Exception ex )
        {
            this._readTextBox.Text = ex.ToString();
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            this.SessionBase!.StatusPrompt = $"failed {activity}";
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
        this.SessionBase?.ThrowDeviceExceptionIfError();
    }

    /// <summary> Reads a message from the session. </summary>
    public async void Read()
    {
        string activity = string.Empty;
        Cursor.Current = Cursors.WaitCursor;
        if ( !this.IsSessionOpen ) return;
        try
        {
            activity = "Awaiting read delay";
            this.SessionBase!.StatusPrompt = activity;
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.SessionBase.StatusPrompt = activity;
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            Application.DoEvents();
            this.SessionBase.StartElapsedStopwatch();
            await System.Threading.Tasks.Task.Delay( this.SessionBase.ReadAfterWriteDelay );
            activity = "Reading status register";
            this.SessionBase.StatusPrompt = activity;
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );

            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( this.SessionBase.StatusReadDelay );
            cc.isr.VI.Pith.ServiceRequests statusByte = this.SessionBase.ReadStatusByte();
            this.SessionBase.ThrowDeviceExceptionIfError( statusByte );
            if ( !this.SessionBase.IsErrorBitSet( statusByte ) && this.SessionBase.IsMessageAvailableBitSet( statusByte ) )
            {
                activity = "reading";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                string responseString = this.SessionBase.ReadLine();
                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
                string message = responseString.InsertCommonEscapeSequences();
                activity = $"received: {message}";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.UpdateReadMessage( message );
            }
            else
            {
                activity = "nothing to read";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            }
        }
        catch ( Exception ex )
        {
            this._readTextBox.Text = ex.ToString();
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            this.SessionBase!.StatusPrompt = $"failed {activity}";
        }
        finally
        {
            this.ReadStatusRegister();
            Cursor.Current = Cursors.Default;
        }
    }

    /// <summary> Erases the text boxes. </summary>
    public void Erase()
    {
        this._readTextBox.Text = string.Empty;
    }

    /// <summary> Clears the session. </summary>
    public void ClearSession()
    {
        string activity = string.Empty;
        if ( !this.IsSessionOpen ) return;
        try
        {
            if ( this.SessionBase is not null )
            {
                activity = "clearing active state";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();
                this.SessionBase.ClearActiveState( this.SessionBase.DeviceClearRefractoryPeriod );
                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
                activity = $"done {activity}";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            }
        }
        catch ( Exception ex )
        {
            this._readTextBox.Text = ex.ToString();
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            this.SessionBase!.StatusPrompt = $"failed {activity}";
        }
        finally
        {
            this.ReadStatusRegister();
            Cursor.Current = Cursors.Default;
        }
    }

    #endregion

    #region " control events "

    /// <summary> Reads status button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadStatusMenuItem_Click( object? sender, EventArgs e )
    {
        this.ReadStatusRegister();
    }

    /// <summary> Queries (write and then reads) the instrument. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void QueryButton_Click( object? sender, EventArgs e )
    {
        this.Query( this._writeComboBox.Text );
    }

    /// <summary> Writes a message to the instrument. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void WriteButton_Click( object? sender, EventArgs e )
    {
        this.Write( this._writeComboBox.Text );
    }

    /// <summary> Updates the read message described by message. </summary>
    /// <param name="message"> The message. </param>
    private void UpdateReadMessage( string? message )
    {
        System.Text.StringBuilder builder = new();
        if ( this._readTextBox.Text.Length > 0 )
        {
            _ = builder.AppendLine( this._readTextBox.Text );
        }

        _ = builder.Append( message );
        this._readTextBox.Text = builder.ToString();
    }

    /// <summary> Reads a message from the instrument. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ReadButton_Click( object? sender, EventArgs e )
    {
        this.Read();
    }

    /// <summary> Event handler. Called by _clearSessionButton for click events. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ClearSessionMenuItem_Click( object? sender, EventArgs e )
    {
        this.ClearSession();
    }

    /// <summary> Event handler. Called by clear for click events. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void EraseDisplayMenuItem_Click( object? sender, EventArgs e )
    {
        this.Erase();
    }

    #endregion


}
