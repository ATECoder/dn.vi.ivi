using System;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace cc.isr.VI.Device.MSTest.Settings;

/// <summary>   The Subsystems Test Settings base class. </summary>
/// <remarks>
/// <para>
/// David, 2018-02-12 </para>
/// </remarks>
/// <param name="filePath"> Full pathname of the file. </param>
[CLSCompliant( false )]
public class SubsystemsSettingsBase() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " exists "

    private bool _exists;

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
	[Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get => this._exists;
        set => _ = this.SetProperty( ref this._exists, value );
    }

    #endregion

    #region " device session information "

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

    private bool _readTerminationEnabled = true;

    /// <summary> Gets or sets the read termination enabled. </summary>
    /// <value> The read termination enabled. </value>
	public virtual bool ReadTerminationEnabled
    {
        get => this._readTerminationEnabled;
        set => _ = this.SetProperty( ref this._readTerminationEnabled, value );
    }

    private int _readTerminationCharacter = 10;

    /// <summary> Gets or sets the read termination character. </summary>
    /// <value> The read termination character. </value>
	public virtual int ReadTerminationCharacter
    {
        get => this._readTerminationCharacter;
        set => _ = this.SetProperty( ref this._readTerminationCharacter, value );
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

    #region " initial values: route subsystem "

    private string _initialClosedChannels = "(@)";

    /// <summary> Gets or sets the initial closed channels. </summary>
    /// <value> The initial closed channels. </value>
	public virtual string InitialClosedChannels
    {
        get => this._initialClosedChannels;
        set => _ = this.SetProperty( ref this._initialClosedChannels, value );
    }

    private string _initialScanList = "(@)";

    /// <summary> Gets or sets the Initial scan list settings. </summary>
    /// <value> The initial scan list settings. </value>
	public virtual string InitialScanList
    {
        get => this._initialScanList;
        set => _ = this.SetProperty( ref this._initialScanList, value );
    }

    #endregion

    #region " initial values: scanner "

    private bool _scanCardInstalled = true;

    /// <summary> Gets or sets the scan card installed. </summary>
    /// <value> The scan card installed. </value>
	public virtual bool ScanCardInstalled
    {
        get => this._scanCardInstalled;
        set => _ = this.SetProperty( ref this._scanCardInstalled, value );
    }

    private int? _scanCardCount = new();

    /// <summary> Gets or sets the number of scan cards. </summary>
    /// <value> The number of scan cards. </value>
	public virtual int? ScanCardCount
    {
        get => this._scanCardCount;
        set => _ = this.SetProperty( ref this._scanCardCount, value );
    }

    #endregion

    #region " initial values: sense subsystem "

    private bool _frontTerminalsControlEnabled;

    /// <summary>
    /// Gets or sets the front terminals control enabled. With manual front terminals switch, this
    /// would normally be set to <c><see langword="False"/></c>.
    /// </summary>
    /// <value> The front terminals control enabled. </value>
	public virtual bool FrontTerminalsControlEnabled
    {
        get => this._frontTerminalsControlEnabled;
        set => _ = this.SetProperty( ref this._frontTerminalsControlEnabled, value );
    }

    private bool _initialFrontTerminalsSelected = true;

    /// <summary> Gets or sets the initial front terminals selected. </summary>
    /// <value> The initial front terminals selected. </value>
	public virtual bool InitialFrontTerminalsSelected
    {
        get => this._initialFrontTerminalsSelected;
        set => _ = this.SetProperty( ref this._initialFrontTerminalsSelected, value );
    }

    private double _initialPowerLineCycles = 1;

    /// <summary> Gets or sets the Initial power line cycles settings. </summary>
    /// <value> The power line cycles settings. </value>
	public virtual double InitialPowerLineCycles
    {
        get => this._initialPowerLineCycles;
        set => _ = this.SetProperty( ref this._initialPowerLineCycles, value );
    }

    private bool _initialAutoDelayEnabled;

    /// <summary> Gets or sets the Initial auto Delay Exists settings. </summary>
    /// <value> The auto Delay settings. </value>
	public virtual bool InitialAutoDelayEnabled
    {
        get => this._initialAutoDelayEnabled;
        set => _ = this.SetProperty( ref this._initialAutoDelayEnabled, value );
    }

    private bool _initialAutoRangeEnabled = true;

    /// <summary> Gets or sets the Initial auto Range enabled settings. </summary>
    /// <value> The auto Range settings. </value>
	public virtual bool InitialAutoRangeEnabled
    {
        get => this._initialAutoRangeEnabled;
        set => _ = this.SetProperty( ref this._initialAutoRangeEnabled, value );
    }

    private bool _initialAutoZeroEnabled = true;

    /// <summary> Gets or sets the Initial auto zero Exists settings. </summary>
    /// <value> The auto zero settings. </value>
	public virtual bool InitialAutoZeroEnabled
    {
        get => this._initialAutoZeroEnabled;
        set => _ = this.SetProperty( ref this._initialAutoZeroEnabled, value );
    }

    private SenseFunctionModes _initialSenseFunction = SenseFunctionModes.VoltageDC;

    /// <summary> Gets or sets the initial sense function. </summary>
    /// <value> The initial sense function. </value>
	public virtual SenseFunctionModes InitialSenseFunction
    {
        get => this._initialSenseFunction;
        set => _ = this.SetProperty( ref this._initialSenseFunction, value );
    }

    private MultimeterFunctionModes _initialMultimeterFunction = MultimeterFunctionModes.VoltageDC;

    /// <summary> Gets or sets the initial multimeter function. </summary>
    /// <value> The initial multimeter function. </value>
	public virtual MultimeterFunctionModes InitialMultimeterFunction
    {
        get => this._initialMultimeterFunction;
        set => _ = this.SetProperty( ref this._initialMultimeterFunction, value );
    }

    private bool _initialFilterEnabled;

    /// <summary> Gets or sets the initial filter enabled. </summary>
    /// <value> The initial filter enabled. </value>
	public virtual bool InitialFilterEnabled
    {
        get => this._initialFilterEnabled;
        set => _ = this.SetProperty( ref this._initialFilterEnabled, value );
    }

    private bool _initialMovingAverageFilterEnabled;

    /// <summary> Gets or sets the initial moving average filter enabled. </summary>
    /// <value> The initial moving average filter enabled. </value>
	public virtual bool InitialMovingAverageFilterEnabled
    {
        get => this._initialMovingAverageFilterEnabled;
        set => _ = this.SetProperty( ref this._initialMovingAverageFilterEnabled, value );
    }

    private int _initialFilterCount = 10;

    /// <summary> Gets or sets the number of initial filter points. </summary>
    /// <value> The number of initial filter points. </value>
	public virtual int InitialFilterCount
    {
        get => this._initialFilterCount;
        set => _ = this.SetProperty( ref this._initialFilterCount, value );
    }

    private double _initialFilterWindow = 0.001;

    /// <summary> Gets or sets the initial filter window. </summary>
    /// <value> The initial filter window. </value>
	public virtual double InitialFilterWindow
    {
        get => this._initialFilterWindow;
        set => _ = this.SetProperty( ref this._initialFilterWindow, value );
    }

    private bool _initialRemoteSenseSelected = true;

    /// <summary> Gets or sets the initial remote sense selected. </summary>
    /// <value> The initial remote sense selected. </value>
	public virtual bool InitialRemoteSenseSelected
    {
        get => this._initialRemoteSenseSelected;
        set => _ = this.SetProperty( ref this._initialRemoteSenseSelected, value );
    }

    #endregion

    #region " initial values: source measure unit "

    private SourceFunctionModes _initialSourceFunction = SourceFunctionModes.VoltageDC;

    /// <summary> Gets or sets the initial source function mode. </summary>
    /// <value> The initial source function mode. </value>
	public virtual SourceFunctionModes InitialSourceFunction
    {
        get => this._initialSourceFunction;
        set => _ = this.SetProperty( ref this._initialSourceFunction, value );
    }

    private double _initialSourceLevel;

    /// <summary> Gets or sets the initial source level. </summary>
    /// <value> The initial source level. </value>
	public virtual double InitialSourceLevel
    {
        get => this._initialSourceLevel;
        set => _ = this.SetProperty( ref this._initialSourceLevel, value );
    }

    private double _initialSourceLimit = 0.000105;

    /// <summary> Gets or sets the initial source limit. </summary>
    /// <value> The initial source limit. </value>
	public virtual double InitialSourceLimit
    {
        get => this._initialSourceLimit;
        set => _ = this.SetProperty( ref this._initialSourceLimit, value );
    }

    private double _maximumOutputPower;

    /// <summary> Gets or sets the maximum output power of the instrument. </summary>
    /// <value> The maximum output power . </value>
	public virtual double MaximumOutputPower
    {
        get => this._maximumOutputPower;
        set => _ = this.SetProperty( ref this._maximumOutputPower, value );
    }

    #endregion

    #region " initial values: system subsystem "

    private double _lineFrequency = 60;

    /// <summary> Gets or sets the line frequency. </summary>
    /// <value> The line frequency. </value>
	public virtual double LineFrequency
    {
        get => this._lineFrequency;
        set => _ = this.SetProperty( ref this._lineFrequency, value );
    }

    #endregion

    #region " initial values: trigger subsystem "

    private TriggerSources _initialTriggerSource = TriggerSources.Immediate;

    /// <summary> Gets or sets the initial trigger source. </summary>
    /// <value> The initial trigger source. </value>
	public virtual TriggerSources InitialTriggerSource
    {
        get => this._initialTriggerSource;
        set => _ = this.SetProperty( ref this._initialTriggerSource, value );
    }

    #endregion
}

