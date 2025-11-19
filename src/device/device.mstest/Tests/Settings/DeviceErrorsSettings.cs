using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace cc.isr.VI.Device.Tests.Settings;

/// <summary>   The Subsystems Test Settings base class. </summary>
/// <remarks>
/// <para>
/// David, 2018-02-12 </para>
/// </remarks>
/// <param name="filePath"> full path name of the file. </param>
[CLSCompliant( false )]
public class DeviceErrorsSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " exists "

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #region " device errors "

    private string _erroneousCommand = "*CLL";

    /// <summary> Gets or sets the erroneous command. </summary>
    /// <value> The erroneous command. </value>
    public virtual string ErroneousCommand
    {
        get => this._erroneousCommand;
        set => _ = this.SetProperty( ref this._erroneousCommand, value );
    }

    private int _errorAvailableMillisecondsDelay = 100;

    /// <summary> Gets or sets the error available milliseconds delay. </summary>
    /// <value> The error available milliseconds delay. </value>
    public virtual int ErrorAvailableMillisecondsDelay
    {
        get => this._errorAvailableMillisecondsDelay;
        set => _ = this.SetProperty( ref this._errorAvailableMillisecondsDelay, value );
    }

    private string _expectedCompoundErrorMessage = "-285,TSP Ieee488Syntax Error at line 1: unexpected symbol near `*',level=20";

    /// <summary> Gets or sets a message describing the expected compound error. </summary>
    /// <value> A message describing the expected compound error. </value>
    public virtual string ExpectedCompoundErrorMessage
    {
        get => this._expectedCompoundErrorMessage;
        set => _ = this.SetProperty( ref this._expectedCompoundErrorMessage, value );
    }

    private string _expectedErrorMessage = "TSP Ieee488Syntax error at line 1: unexpected symbol near `*'";

    /// <summary> Gets or sets a message describing the expected error. </summary>
    /// <value> A message describing the expected error. </value>
    public virtual string ExpectedErrorMessage
    {
        get => this._expectedErrorMessage;
        set => _ = this.SetProperty( ref this._expectedErrorMessage, value );
    }

    private int _expectedErrorNumber = -285;

    /// <summary> Gets or sets the expected error number. </summary>
    /// <value> The expected error number. </value>
    public virtual int ExpectedErrorNumber
    {
        get => this._expectedErrorNumber;
        set => _ = this.SetProperty( ref this._expectedErrorNumber, value );
    }

    private int _expectedErrorLevel = 20;

    /// <summary> Gets or sets the expected error level. </summary>
    /// <value> The expected error level. </value>
    public virtual int ExpectedErrorLevel
    {
        get => this._expectedErrorLevel;
        set => _ = this.SetProperty( ref this._expectedErrorLevel, value );
    }

    private string _parseCompoundErrorMessage = "-113,\"Undefined header;1;2018/05/26 14: 00:14.871\"";

    /// <summary> Gets or sets a message describing the parse compound error. </summary>
    /// <value> A message describing the parse compound error. </value>
    public virtual string ParseCompoundErrorMessage
    {
        get => this._parseCompoundErrorMessage;
        set => _ = this.SetProperty( ref this._parseCompoundErrorMessage, value );
    }

    private string _parseErrorMessage = "Undefined header";

    /// <summary> Gets or sets a message describing the Parse error. </summary>
    /// <value> A message describing the Parse error. </value>
    public virtual string ParseErrorMessage
    {
        get => this._parseErrorMessage;
        set => _ = this.SetProperty( ref this._parseErrorMessage, value );
    }

    private int _parseErrorNumber = -113;

    /// <summary> Gets or sets the parse error number. </summary>
    /// <value> The parse error number. </value>
    public virtual int ParseErrorNumber
    {
        get => this._parseErrorNumber;
        set => _ = this.SetProperty( ref this._parseErrorNumber, value );
    }

    private int _parseErrorLevel = 1;

    /// <summary> Gets or sets the parse error level. </summary>
    /// <value> The parse error level. </value>
    public virtual int ParseErrorLevel
    {
        get => this._parseErrorLevel;
        set => _ = this.SetProperty( ref this._parseErrorLevel, value );
    }

    #endregion
}

