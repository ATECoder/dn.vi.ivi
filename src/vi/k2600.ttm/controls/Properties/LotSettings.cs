using System.ComponentModel;
using System.Text.Json.Serialization;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary>   A settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public class LotSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public LotSettings() { }

    #endregion

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

    #region " ttm properties "

    private System.Diagnostics.TraceEventType _traceLogLevel = System.Diagnostics.TraceEventType.Information;

    /// <summary>   Gets or sets the trace log level. </summary>
    /// <value> The trace log level. </value>
	public System.Diagnostics.TraceEventType TraceLogLevel
    {
        get => this._traceLogLevel;
        set => this.SetProperty( ref this._traceLogLevel, value );
    }

    private System.Diagnostics.TraceEventType _traceShowLevel = System.Diagnostics.TraceEventType.Information;

    /// <summary>   Gets or sets the trace Show level. </summary>
    /// <value> The trace Show level. </value>
	public System.Diagnostics.TraceEventType TraceShowLevel
    {
        get => this._traceShowLevel;
        set => this.SetProperty( ref this._traceShowLevel, value );
    }

    private bool _automaticallyAddPart;

    /// <summary>   Gets or sets a value indicating whether the automatically add part. </summary>
    /// <value> True if automatically add part, false if not. </value>
	public bool AutomaticallyAddPart
    {
        get => this._automaticallyAddPart;
        set => this.SetProperty( ref this._automaticallyAddPart, value );
    }

    private bool _deviceEnabled = true;

    /// <summary>   Gets or sets a value indicating whether the device is enabled. </summary>
    /// <value> True if device enabled, false if not. </value>
	public bool DeviceEnabled
    {
        get => this._deviceEnabled;
        set => this.SetProperty( ref this._deviceEnabled, value );
    }

    private string _ftpAddress = "ftp://ttm%40isr.cc:ttm2.2@ftp.isr.cc";

    /// <summary>   Gets or sets the FTP address. </summary>
    /// <value> The FTP address. </value>
	public string FtpAddress
    {
        get => this._ftpAddress;
        set => this.SetProperty( ref this._ftpAddress, value );
    }

    private string _lotNumber = "Lot 1";

    /// <summary>   Gets or sets the lot number. </summary>
    /// <value> The lot number. </value>
	public string LotNumber
    {
        get => this._lotNumber;
        set => this.SetProperty( ref this._lotNumber, value );
    }

    private string _partNumber = "Part 1";

    /// <summary>   Gets or sets the part number. </summary>
    /// <value> The part number. </value>
	public string PartNumber
    {
        get => this._partNumber;
        set => this.SetProperty( ref this._partNumber, value );
    }

    private string _dataFolder = "c:\\users\\public\\documents\\ttm";

    /// <summary>   Gets or sets the pathname of the data folder. </summary>
    /// <value> The pathname of the data folder. </value>
	public string DataFolder
    {
        get => this._dataFolder;
        set => this.SetProperty( ref this._dataFolder, value );
    }

    private string _measurementsFolderName = "measurements";

    /// <summary>   Gets or sets the pathname of the measurements folder. </summary>
    /// <value> The pathname of the measurements folder. </value>
	public string MeasurementsFolderName
    {
        get => this._measurementsFolderName;
        set => this.SetProperty( ref this._measurementsFolderName, value );
    }

    /// <summary>   Gets the pathname of the measurements folder. </summary>
    /// <value> The pathname of the measurements folder. </value>
	public string MeasurementsFolder => System.IO.Path.Combine( this.DataFolder, this.MeasurementsFolderName );

    private string _partsFileName = "Part1.csv";

    /// <summary>   Gets or sets the full pathname of the parts file. </summary>
    /// <value> The full pathname of the parts file. </value>
	public string PartsFileName
    {
        get => this._partsFileName;
        set => this.SetProperty( ref this._partsFileName, value );
    }

    /// <summary>   Gets the full pathname of the parts file. </summary>
    /// <value> The full pathname of the parts file. </value>
    [JsonIgnore]
    public string PartsFilePath => System.IO.Path.Combine( this.MeasurementsFolder, this.PartsFileName );

    private string _operatorId = "Operator 1";

    /// <summary>   Gets or sets the identifier of the operator. </summary>
    /// <value> The identifier of the operator. </value>
	public string OperatorId
    {
        get => this._operatorId;
        set => this.SetProperty( ref this._operatorId, value );
    }

    private string _resourceName = "TCPIP0::192.168.0.150::inst0::INSTR";

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    public string ResourceName
    {
        get => this._resourceName;
        set => this.SetProperty( ref this._resourceName, value );
    }

    private string _setting = string.Empty;

    /// <summary>   Gets or sets the setting. </summary>
    /// <value> The setting. </value>
	public string Setting
    {
        get => this._setting;
        set => this.SetProperty( ref this._setting, value );
    }

    private string _traceFileName = "Trace1.csv";

    /// <summary>   Gets or sets the full pathname of the trace file. </summary>
    /// <value> The full pathname of the trace file. </value>
	public string TraceFileName
    {
        get => this._traceFileName;
        set => this.SetProperty( ref this._traceFileName, value );
    }

    /// <summary>   Gets the full pathname of the trace file. </summary>
    /// <value> The full pathname of the trace file. </value>
    [JsonIgnore]
    public string TraceFilePath => System.IO.Path.Combine( this.MeasurementsFolder, this.TraceFileName );

    private double _lineFrequency = 60;

    /// <summary>   Gets or sets the line frequency. </summary>
    /// <value> The line frequency. </value>
	public double LineFrequency
    {
        get => this._lineFrequency;
        set => this.SetProperty( ref this._lineFrequency, value );
    }

    #endregion
}
