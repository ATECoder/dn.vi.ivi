using System.ComponentModel;

namespace cc.isr.VI.Pith.Settings;

/// <summary>   The visa session settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
[CLSCompliant( false )]
public class CommandsSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    #region " keep alive commands "

    private string _keepAliveQueryCommand = "*OPC?";

    /// <summary> Gets or sets the keep alive query command. </summary>
    /// <value> The keep alive query command. </value>
    public virtual string KeepAliveQueryCommand
    {
        get => this._keepAliveQueryCommand;
        set => _ = this.SetProperty( ref this._keepAliveQueryCommand, value );
    }

    private string _keepAliveCommand = "*OPC";

    /// <summary> Gets or sets the keep-alive command. </summary>
    /// <value> The keep-alive command. </value>
    public virtual string KeepAliveCommand
    {
        get => this._keepAliveCommand;
        set => _ = this.SetProperty( ref this._keepAliveCommand, value );
    }

    #endregion

    #region " session commands "

    private string _clearExecutionStateCommand = "*CLS";

    /// <summary>   Gets or sets the non-distractive status clear command. </summary>
    /// <value> The non-distractive status clear command. </value>
    [Description( "The non-distractive status clear command [*CLS]" )]
    public virtual string ClearExecutionStateCommand
    {
        get => this._clearExecutionStateCommand;
        set => _ = this.SetProperty( ref this._clearExecutionStateCommand, value );
    }

    private string _identificationQueryCommand = "*IDN?";

    /// <summary>   Gets or sets the identification query command. </summary>
    /// <value> The identification query command. </value>
    [Description( "The identification query command [*IDN?]" )]
    public virtual string IdentificationQueryCommand
    {
        get => this._identificationQueryCommand;
        set => _ = this.SetProperty( ref this._identificationQueryCommand, value );
    }

    private string _operationCompleteCommand = "*OPC";

    /// <summary>   Gets or sets the operation completion command. Note that
    ///             the command must follow a the <see cref="ClearExecutionStateCommand"/>.
    ///             However, if the status clear command is distractive, the enabled standard events must be
    ///             restored. This is taken care of but the <see cref="SessionBase.ClearExecutionState"/> method. </summary>
    /// <value> The 'OPC' test command. </value>
    [Description( "The operation completion invocation command [*CLS; *OPC]" )]
    public virtual string OperationCompleteCommand
    {
        get => this._operationCompleteCommand;
        set => _ = this.SetProperty( ref this._operationCompleteCommand, value );
    }

    private string _operationCompletedQueryCommand = "*OPC?";

    /// <summary>   Gets or sets the 'operation complete' query command. </summary>
    /// <value> The 'operation complete' query command. </value>
    [Description( "The operation complete query command [*OPC?]" )]
    public virtual string OperationCompletedQueryCommand
    {
        get => this._operationCompletedQueryCommand;
        set => _ = this.SetProperty( ref this._operationCompletedQueryCommand, value );
    }

    private string _operationCompletedReplyMessage = Syntax.ScpiSyntax.OperationCompletedValue;

    /// <summary>   Gets or sets a message describing the operation completed reply. </summary>
    /// <value> A message describing the operation completed reply. </value>
    [Description( "The operation complete reply message [1]" )]
    public virtual string OperationCompletedReplyMessage
    {
        get => this._operationCompletedReplyMessage;
        set => _ = this.SetProperty( ref this._operationCompletedReplyMessage, value );
    }

    /// <summary>   Gets or sets the 'reset known state' command. </summary>
    /// <value> The 'reset known state' command. </value>
    [Description( "The command to reset the instrument to its 'known' state [*RST]" )]
    public string ResetKnownStateCommand
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "*RST";

    /// <summary> Gets or sets the service request enable command format. </summary>
    /// <value> The service request enable command format. </value>
    [Description( "The service request enable command format [*SRE {0}]" )]
    public string ServiceRequestEnableCommandFormat
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "*SRE {0}";

    /// <summary> Gets or sets the service request enable query command. </summary>
    /// <value> The service request enable query command. </value>
    [Description( "The service request enable query command [*SRE?]" )]
    public string ServiceRequestEnableQueryCommand
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "*SRE?";

    /// <summary> Gets or sets the Standard Event enable query command. </summary>
    /// <value> The Standard Event enable query command. </value>
    [Description( "The Standard Event enable command format [*ESE {0}]" )]
    public string StandardEventEnableCommandFormat
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "*ESE {0}";

    /// <summary> Gets or sets the Standard Event enable query command. </summary>
    /// <value> The Standard Event enable query command. </value>
    [Description( "The Standard Event enable query command [*ESE?]" )]
    public string StandardEventEnableQueryCommand
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "*ESE?";

    /// <summary> Gets or sets the Standard Event status query command. </summary>
    /// <value> The Standard Event query command. </value>
    [Description( "The Standard Event status query command [*ESR?]" )]
    public string StandardEventStatusQueryCommand
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "*ESR?";

    private string _statusByteQueryCommand = "*STB?";

    /// <summary>   Gets or sets the 'Read Status Byte query' command. </summary>
    /// <value> The 'Read Status Byte query' command. </value>
    [Description( "The read status byte query command [*STB?]" )]
    public virtual string StatusByteQueryCommand
    {
        get => this._statusByteQueryCommand;
        set => _ = this.SetProperty( ref this._statusByteQueryCommand, value );
    }

    /// <summary>   Gets or sets the wait to continue command. </summary>
    /// <value> The wait to continue. </value>
    [Description( "The wait to continue command [*WAI]" )]
    public string WaitToContinueCommand
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "*WAI";

    #endregion
}
