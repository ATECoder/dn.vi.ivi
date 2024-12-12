namespace cc.isr.VI.Pith;

public abstract partial class SessionBase
{
    private bool _hasErrorReport;

    /// <summary> Gets or sets the has error report. </summary>
    /// <value> The has error report. </value>
    public bool HasErrorReport
    {
        get => this._hasErrorReport;
        set => _ = this.SetProperty( ref this._hasErrorReport, value );
    }

    private string _deviceErrorReport = string.Empty;

    /// <summary> Gets or sets the device error report. </summary>
    /// <value> The device error report. </value>
    public string DeviceErrorReport
    {
        get => this._deviceErrorReport;
        set
        {
            if ( this.SetProperty( ref this._deviceErrorReport, value ) )
                this.HasErrorReport = !string.IsNullOrWhiteSpace( value );
        }
    }

    private int _deviceErrorReportCount;

    /// <summary>   Gets or sets the number of errors that are included in the device error report. </summary>
    /// <value> The number of errors that are included in the device error report. </value>
    public int DeviceErrorReportCount
    {
        get => this._deviceErrorReportCount;
        set => _ = this.SetProperty( ref this._deviceErrorReportCount, value );
    }

    private bool _hasDeviceError;

    /// <summary> Gets or sets the has device error. </summary>
    /// <value> The has device error. </value>
    public bool HasDeviceError
    {
        get => this._hasDeviceError;
        set
        {
            if ( this.SetProperty( ref this._hasDeviceError, value ) )
                this.ErrorForeColor = SelectErrorColor( value );
        }
    }

    private string _deviceErrorPreamble = string.Empty;

    /// <summary>
    /// Gets or sets the device error preamble including the <see cref="StandardRegisterCaption"/>
    /// and last received and sent messages.
    /// </summary>
    /// <value> The device error preamble. </value>
    public string DeviceErrorPreamble
    {
        get => this._deviceErrorPreamble;
        set => _ = this.SetProperty( ref this._deviceErrorPreamble, value );
    }

    private string _lastErrorCompoundErrorMessage = string.Empty;

    /// <summary> Gets or sets a compound message describing the last error. </summary>
    /// <value> A compound message describing the last error. </value>
    public string LastErrorCompoundErrorMessage
    {
        get => this._lastErrorCompoundErrorMessage;
        set => _ = this.SetProperty( ref this._lastErrorCompoundErrorMessage, value );
    }

    /// <summary> Select error color. </summary>
    /// <param name="isError"> True if is error, false if not. </param>
    /// <returns> A Drawing.Color. </returns>
    protected static System.Drawing.Color SelectErrorColor( bool isError )
    {
        return isError ? System.Drawing.Color.OrangeRed : System.Drawing.Color.Aquamarine;
    }

    /// <summary> The error foreground color. </summary>
    private System.Drawing.Color _errorForeColor = System.Drawing.Color.White;

    /// <summary> Gets or sets the color of the error foreground. </summary>
    /// <value> The color of the error foreground. </value>
    public System.Drawing.Color ErrorForeColor
    {
        get => this._errorForeColor;
        set => _ = this.SetProperty( ref this._errorForeColor, value );
    }

    /// <summary> Clears the error report. </summary>
    public virtual void ClearErrorReport()
    {
        this.DeviceErrorPreamble = string.Empty;
        this.DeviceErrorReportCount = 0;
        this.DeviceErrorReport = string.Empty;
        this.LastErrorCompoundErrorMessage = string.Empty;
        this.HasDeviceError = false;
    }
}
