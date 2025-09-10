using cc.isr.Std.EscapeSequencesExtensions;

namespace cc.isr.VI;

public abstract partial class HarmonicsMeasureSubsystemBase
{
    #region " range "

    /// <summary> Define measure mode ranges. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="measureModeRanges">   The measure mode ranges. </param>
    /// <param name="defaultFunctionRange"> The default range. </param>
    public static void DefineMeasureModeRanges( RangeDictionary measureModeRanges, Std.Primitives.RangeR defaultFunctionRange )
    {
        if ( measureModeRanges is null ) throw new ArgumentNullException( nameof( measureModeRanges ) );
        measureModeRanges.Clear();
        foreach ( HarmonicsMeasureMode functionMode in Enum.GetValues( typeof( HarmonicsMeasureMode ) ) )
            measureModeRanges.Add( ( int ) functionMode, new Std.Primitives.RangeR( defaultFunctionRange ) );
    }

    /// <summary> Define Measure Mode ranges. </summary>
    private void DefineMeasureModeRanges()
    {
        this.MeasureModeRanges = [];
        HarmonicsMeasureSubsystemBase.DefineMeasureModeRanges( this.MeasureModeRanges, this.DefaultFunctionRange );
    }

    /// <summary> Gets or sets the Measure Mode ranges. </summary>
    /// <value> The Measure Mode ranges. </value>
    public RangeDictionary MeasureModeRanges { get; private set; }

    /// <summary> Gets or sets the default function range. </summary>
    /// <value> The default function range. </value>
    public Std.Primitives.RangeR DefaultFunctionRange { get; set; }

    /// <summary> Converts a MeasureMode to a range. </summary>
    /// <param name="measureMode"> The Measure Mode. </param>
    /// <returns> MeasureMode as an cc.isr.Std.Primitives.RangeR. </returns>
    public virtual Std.Primitives.RangeR ToRange( int measureMode )
    {
        return this.MeasureModeRanges[measureMode];
    }

    /// <summary> The Range of the range. </summary>
    /// <value> The function range. </value>
    public Std.Primitives.RangeR? FunctionRange
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    #endregion

    #region " decimal places "

    /// <summary> Gets or sets the default decimal places. </summary>
    /// <value> The default decimal places. </value>
    public int DefaultMeasureModeDecimalPlaces { get; set; } = 3;

    /// <summary> Define measure mode decimal places. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="measureModeDecimalPlaces">        The measure mode decimal places. </param>
    /// <param name="defaultFunctionModeDecimalPlaces"> The default decimal places. </param>
    public static void DefineMeasureModeDecimalPlaces( IntegerDictionary measureModeDecimalPlaces, int defaultFunctionModeDecimalPlaces )
    {
        if ( measureModeDecimalPlaces is null ) throw new ArgumentNullException( nameof( measureModeDecimalPlaces ) );
        measureModeDecimalPlaces.Clear();
        foreach ( HarmonicsMeasureMode functionMode in Enum.GetValues( typeof( HarmonicsMeasureMode ) ) )
            measureModeDecimalPlaces.Add( ( int ) functionMode, defaultFunctionModeDecimalPlaces );
    }

    /// <summary> Define Measure Mode decimal places. </summary>
    private void DefineMeasureModeDecimalPlaces()
    {
        this.MeasureModeDecimalPlaces = [];
        HarmonicsMeasureSubsystemBase.DefineMeasureModeDecimalPlaces( this.MeasureModeDecimalPlaces, this.DefaultMeasureModeDecimalPlaces );
    }

    /// <summary> Gets or sets the Measure Mode decimal places. </summary>
    /// <value> The Measure Mode decimal places. </value>
    public IntegerDictionary MeasureModeDecimalPlaces { get; private set; }

    /// <summary> Converts a Measure Mode to a decimal places. </summary>
    /// <param name="measureMode"> The Measure Mode. </param>
    /// <returns> MeasureMode as an Integer. </returns>
    public virtual int ToDecimalPlaces( int measureMode )
    {
        return this.MeasureModeDecimalPlaces[measureMode];
    }

    /// <summary> Gets or sets the function range decimal places. </summary>
    /// <value> The function range decimal places. </value>
    public int FunctionRangeDecimalPlaces
    {
        get;
        set
        {
            if ( this.FunctionRangeDecimalPlaces != value )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " units "

    /// <summary> Gets or sets the default unit. </summary>
    /// <value> The default unit. </value>
    public cc.isr.UnitsAmounts.Unit DefaultFunctionUnit { get; set; } = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;

    /// <summary> Define measure mode units. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <exception cref="KeyNotFoundException">  Thrown when a Key Not Found error condition occurs. </exception>
    /// <param name="functionModeUnits"> The measure mode decimal places. </param>
    public static void DefineMeasureModeUnits( UnitDictionary functionModeUnits )
    {
        if ( functionModeUnits is null ) throw new ArgumentNullException( nameof( functionModeUnits ) );
        functionModeUnits[( int ) HarmonicsMeasureMode.Voltage] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) HarmonicsMeasureMode.Decibel] = cc.isr.UnitsAmounts.StandardUnits.UnitlessUnits.Decibel;
        foreach ( HarmonicsMeasureMode functionMode in Enum.GetValues( typeof( HarmonicsMeasureMode ) ) )
        {
            if ( !functionModeUnits.ContainsKey( ( int ) functionMode ) )
            {
                throw new KeyNotFoundException( $"Unit not specified for {nameof( HarmonicsMeasureMode )}.{functionMode}" );
            }
        }
    }

    /// <summary> Defines Measure Mode units. </summary>
    private void DefineMeasureModeUnits()
    {
        this.MeasureModeUnits = [];
        DefineMeasureModeUnits( this.MeasureModeUnits );
    }

    /// <summary> Gets or sets the Measure Mode decimal places. </summary>
    /// <value> The Measure Mode decimal places. </value>
    public UnitDictionary MeasureModeUnits { get; private set; }

    /// <summary> Parse units. </summary>
    /// <param name="measureMode"> The  Multimeter Measure Mode. </param>
    /// <returns> An cc.isr.UnitsAmounts.Unit. </returns>
    public virtual cc.isr.UnitsAmounts.Unit ToUnit( int measureMode )
    {
        return this.MeasureModeUnits[measureMode];
    }

    /// <summary> Gets or sets the function unit. </summary>
    /// <value> The function unit. </value>
    public cc.isr.UnitsAmounts.Unit FunctionUnit
    {
        get => this.PrimaryReading!.Amount.Unit;
        set
        {
            if ( this.FunctionUnit != value )
            {
                this.PrimaryReading!.ApplyUnit( value );
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " measure mode "

    /// <summary> Define function clear known state. </summary>
    protected virtual void DefineFunctionClearKnownState()
    {
        if ( this.ReadingAmounts.HasReadingElements() )
        {
            this.ReadingAmounts.ActiveReadingAmount().ApplyUnit( this.FunctionUnit );
        }

        this.ReadingAmounts.Reset();
        _ = this.ParsePrimaryReading( string.Empty );
        this.ReadingAmounts.PrimaryReading!.ApplyUnit( this.FunctionUnit );
        _ = this.ReadingAmounts.TryParse( string.Empty );
        this.NotifyPropertyChanged( nameof( this.ReadingAmounts ) );
    }

    /// <summary> Define measure mode read writes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="measureModeReadWrites"> A Dictionary of measure mode parses. </param>
    public static void DefineMeasureModeReadWrites( Pith.EnumReadWriteCollection measureModeReadWrites )
    {
        if ( measureModeReadWrites is null ) throw new ArgumentNullException( nameof( measureModeReadWrites ) );
        measureModeReadWrites.Clear();
        foreach ( HarmonicsMeasureMode functionMode in Enum.GetValues( typeof( HarmonicsMeasureMode ) ) )
            measureModeReadWrites.Add( functionMode );
    }

    /// <summary> Define Measure Mode read writes. </summary>
    protected virtual void DefineMeasureModeReadWrites()
    {
        this.MeasureModeReadWrites = [];
        DefineMeasureModeReadWrites( this.MeasureModeReadWrites );
    }

    /// <summary> Define Measure Mode read writes. </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="readValueDecorator">  The read value decorator. e.g., """{0}""". </param>
    /// <param name="writeValueDecorator"> The write value decorator, e.g., "'{0}'". </param>
    public void DefineMeasureModeReadWrites( string readValueDecorator, string writeValueDecorator )
    {
        this.MeasureModeReadWrites = new Pith.EnumReadWriteCollection()
        {
            ReadValueDecorator = readValueDecorator,
            WriteValueDecorator = writeValueDecorator
        };
        DefineMeasureModeReadWrites( this.MeasureModeReadWrites );
    }

    /// <summary> Gets or sets a dictionary of Harmonics Measure Mode parses. </summary>
    /// <value> A Dictionary of Harmonics Measure Mode parses. </value>
    public Pith.EnumReadWriteCollection MeasureModeReadWrites { get; private set; }

    /// <summary>
    /// Gets or sets the supported Measure Modes. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> The supported Harmonics Measure Mode. </value>
    public HarmonicsMeasureMode SupportedMeasureModes
    {
        get;
        set
        {
            if ( !this.SupportedMeasureModes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached Harmonics Measure Mode. </summary>
    /// <value>
    /// The <see cref="MeasureMode">Harmonics Measure Mode</see> or none if not set or unknown.
    /// </value>
    public HarmonicsMeasureMode? MeasureMode
    {
        get;
        set
        {
            if ( !this.MeasureMode.Equals( value ) )
            {
                field = value;
                if ( value.HasValue )
                {
                    this.FunctionRange = this.ToRange( ( int ) value.Value );
                    this.FunctionUnit = this.ToUnit( ( int ) value.Value );
                    this.FunctionRangeDecimalPlaces = this.ToDecimalPlaces( ( int ) value.Value );
                }
                else
                {
                    this.FunctionRange = this.DefaultFunctionRange;
                    this.FunctionUnit = this.DefaultFunctionUnit;
                    this.FunctionRangeDecimalPlaces = this.DefaultMeasureModeDecimalPlaces;
                }

                this.DefineFunctionClearKnownState();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Writes and reads back the Harmonics Measure Mode. </summary>
    /// <remarks>   David, 2021-04-12. </remarks>
    /// <param name="value">        The  Harmonics Measure Mode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (HarmonicsMeasureMode ParsedValue, <see cref="WriteInfo{T}"/> WriteInfo,
    /// <see cref="QueryParseInfo{T}"/> QueryParseInfo) .
    /// </returns>
    public (HarmonicsMeasureMode ParsedValue, WriteInfo<int> WriteInfo, QueryParseInfo<int> QueryParseInfo) ApplyMeasureMode( HarmonicsMeasureMode value, bool checkStatus = false )
    {
        (HarmonicsMeasureMode? _, WriteInfo<int> writeInfo) = this.WriteMeasureMode( value, checkStatus );
        (HarmonicsMeasureMode parsedValue, QueryParseInfo<int> queryParseInfo) = this.QueryMeasureMode( checkStatus );
        return (parsedValue, writeInfo, queryParseInfo);
    }

    /// <summary> Gets or sets the Harmonics Measure Mode query command. </summary>
    /// <value> The Harmonics Measure Mode query command. </value>
    protected virtual string MeasureModeQueryCommand { get; set; } = string.Empty;

    /// <summary>   Writes the measure mode query command. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <returns>   An ExecuteInfo. </returns>
    public virtual ExecuteInfo WriteMeasureModeQueryCommand()
    {
        return this.Session.WriteLineElapsed( this.MeasureModeQueryCommand );
    }

    /// <summary>   Queries parse measure mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (HarmonicsMeasureMode ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    protected (HarmonicsMeasureMode ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryParseMeasureMode( bool checkStatus = false )
    {
        QueryInfo qi = checkStatus
            ? this.Session.QueryStatusReady( this.ReadyToQueryTimeout, this.ReadyToReadTimeout, this.MeasureModeQueryCommand )
            : this.Session.QueryElapsed( this.MeasureModeQueryCommand );
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseMeasureMode( qi.ReceivedMessage, out HarmonicsMeasureMode parsedValue );
        return hasValue
            ? (parsedValue, new QueryParseInfo<int>( qi, parseInfo ))
            : throw new InvalidCastException( $"Failed parsing '{qi.ReceivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.MeasureModeQueryCommand}'" );
    }

    /// <summary>   Parse measure mode. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <returns>
    /// A tuple: (HarmonicsMeasureMode ParsedValue, <see cref="ParseInfo{T}"/> ParseInfo).
    /// </returns>
    public (HarmonicsMeasureMode ParsedValue, ParseInfo<int> ParseInfo) ParseMeasureMode( string receivedMessage )
    {
        (bool hasValue, ParseInfo<int> parseInfo) = this.TryParseMeasureMode( receivedMessage, out HarmonicsMeasureMode parsedValue );
        return hasValue
            ? (parsedValue, parseInfo)
            : throw new InvalidCastException( $"Failed parsing '{receivedMessage.InsertCommonEscapeSequences()}' from '{parseInfo.ParsedMessage}' for query '{this.MeasureModeQueryCommand}'" );
    }

    /// <summary>
    /// Attempts to parse a measure mode from the given data, returning a default value rather
    /// than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="receivedMessage">  Message describing the received. </param>
    /// <param name="parsedValue">      [out] The parsed value. </param>
    /// <returns> A tuple: (bool HasValue, <see cref="ParseInfo{T}"/> ParseInfo) </returns>
    public abstract (bool HasValue, ParseInfo<int> ParseInfo) TryParseMeasureMode( string receivedMessage, out HarmonicsMeasureMode parsedValue );

    /// <summary>   Queries the Harmonics Measure Mode. </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (HarmonicsMeasureMode ParsedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo)
    /// </returns>
    public (HarmonicsMeasureMode ParsedValue, QueryParseInfo<int> QueryParseInfo) QueryMeasureMode( bool checkStatus = false )
    {
        (HarmonicsMeasureMode ParsedValue, QueryParseInfo<int> QueryParseInfo) reply = this.QueryParseMeasureMode( checkStatus );
        this.MeasureMode = reply.ParsedValue;
        return reply;
    }

    /// <summary>
    /// Queries the Harmonics Measure Mode. Also sets the <see cref="MeasureMode"></see> cached value.
    /// </summary>
    /// <returns> The Harmonics Measure Mode or null if unknown. </returns>
    [Obsolete( "Now using double quests for both read and writes" )]
    public HarmonicsMeasureMode? QueryMeasureModeTrimQuotes()
    {
        // the instrument expects single quotes when writing the value but sends back items delimited with double quotes.
        string reading = this.Session.QueryTrimEnd( this.MeasureModeQueryCommand ).Trim( '"' );
        long v = this.Session.Parse( this.MeasureModeReadWrites, reading );
        this.MeasureMode = Enum.IsDefined( typeof( HarmonicsMeasureMode ), v ) ? ( HarmonicsMeasureMode ) ( int ) Enum.ToObject( typeof( HarmonicsMeasureMode ), v ) : new HarmonicsMeasureMode?();
        return this.MeasureMode;
    }

    /// <summary> Gets or sets the Harmonics Measure Mode command format. </summary>
    /// <value> The Harmonics Measure Mode command format. </value>
    protected virtual string MeasureModeCommandFormat { get; set; } = string.Empty;

    /// <summary>   Gets or sets the measure model refractory time span. </summary>
    /// <value> The measure model refractory time span. </value>
    public abstract TimeSpan MeasureModelRefractoryTimeSpan { get; set; }

    /// <summary>
    /// Writes the Harmonics Measure Mode without reading back the value from the device.
    /// </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <param name="value">        The Harmonics Measure Mode. </param>
    /// <param name="checkStatus">  (Optional) True to check status. </param>
    /// <returns>
    /// A tuple: (HarmonicsMeasureMode? sentValue, string SentMessage, TimeSpan ElapsedTime)
    /// Time Span: WriteEnum Delay.
    /// </returns>
    public (HarmonicsMeasureMode? SentValue, WriteInfo<int> WriteInfo) WriteMeasureMode( HarmonicsMeasureMode value, bool checkStatus = false )
    {
        if ( checkStatus )
        {
            ExecuteInfo executeInfo = this.Session.WriteStatusReady( TimeSpan.FromMilliseconds( 100 ),
                 string.Format( this.MeasureModeCommandFormat, this.MeasureModeReadWrites.SelectItem( ( int ) value ).WriteValue ) );
            return (value, new WriteInfo<int>( ( int ) value, executeInfo ));
        }
        else
        {
            ExecuteInfo executeInfo = this.Session.WriteLineElapsed( string.Format( this.MeasureModeCommandFormat, this.MeasureModeReadWrites.SelectItem( ( int ) value ).WriteValue ) );
            this.MeasureMode = value; // this.Session.Write( value, this.MeasurementStartModeCommandFormat );
            return (this.MeasureMode, new WriteInfo<int>( ( int ) value, this.MeasureModeCommandFormat, executeInfo.SentMessage, executeInfo.GetElapsedTimes() ));
        }
    }

    #endregion
}
/// <summary> Specifies the Harmonics Measure mode. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum HarmonicsMeasureMode
{
    /// <summary> An enum constant representing the voltage option. </summary>
    [System.ComponentModel.Description( "Voltage (Volt)" )]
    Voltage = 0,

    /// <summary> An enum constant representing the decibel option. </summary>
    [System.ComponentModel.Description( "Decibel (dB)" )]
    Decibel = 1

}
