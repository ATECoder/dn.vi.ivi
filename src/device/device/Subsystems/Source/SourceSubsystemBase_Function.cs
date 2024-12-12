namespace cc.isr.VI;

public abstract partial class SourceSubsystemBase
{
    #region " range "

    /// <summary> Define function mode ranges. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="functionModeRanges">   The function mode ranges. </param>
    /// <param name="defaultFunctionRange"> The default function range. </param>
    public static void DefineFunctionModeRanges( RangeDictionary functionModeRanges, Std.Primitives.RangeR defaultFunctionRange )
    {
        if ( functionModeRanges is null ) throw new ArgumentNullException( nameof( functionModeRanges ) );
        functionModeRanges.Clear();
        foreach ( SourceFunctionModes functionMode in Enum.GetValues( typeof( SourceFunctionModes ) ) )
            functionModeRanges.Add( ( int ) functionMode, new Std.Primitives.RangeR( defaultFunctionRange ) );
    }

    /// <summary> Define function mode ranges. </summary>
    private void DefineFunctionModeRanges()
    {
        this.FunctionModeRanges = [];
        DefineFunctionModeRanges( this.FunctionModeRanges, this.DefaultFunctionRange );
    }

    /// <summary> Gets or sets the function mode ranges. </summary>
    /// <value> The function mode ranges. </value>
    public RangeDictionary FunctionModeRanges { get; private set; }

    /// <summary> Gets or sets the default function range. </summary>
    /// <value> The default function range. </value>
    public Std.Primitives.RangeR DefaultFunctionRange { get; set; }

    /// <summary> Converts a functionMode to a range. </summary>
    /// <param name="functionMode"> The function mode. </param>
    /// <returns> FunctionMode as an cc.isr.Std.Primitives.RangeR. </returns>
    public virtual Std.Primitives.RangeR ToRange( int functionMode )
    {
        return this.FunctionModeRanges[functionMode];
    }

    /// <summary> The function range. </summary>
    private Std.Primitives.RangeR _functionRange;

    /// <summary> The Range of the range. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The function range. </value>
    public Std.Primitives.RangeR FunctionRange
    {
        get => this._functionRange;
        set
        {
            if ( this.FunctionRange != value )
            {
                this._functionRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " decimal places "

    /// <summary> Gets or sets the default decimal places. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The default decimal places. </value>
    public int DefaultFunctionModeDecimalPlaces { get; set; }

    /// <summary> Define function mode decimal places. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="functionModeDecimalPlaces">        The function mode decimal places. </param>
    /// <param name="defaultFunctionModeDecimalPlaces"> The default decimal places. </param>
    public static void DefineFunctionModeDecimalPlaces( IntegerDictionary functionModeDecimalPlaces, int defaultFunctionModeDecimalPlaces )
    {
        if ( functionModeDecimalPlaces is null ) throw new ArgumentNullException( nameof( functionModeDecimalPlaces ) );
        functionModeDecimalPlaces.Clear();
        foreach ( SourceFunctionModes functionMode in Enum.GetValues( typeof( SourceFunctionModes ) ) )
            functionModeDecimalPlaces.Add( ( int ) functionMode, defaultFunctionModeDecimalPlaces );
    }

    /// <summary> Define function mode decimal places. </summary>
    private void DefineFunctionModeDecimalPlaces()
    {
        this.FunctionModeDecimalPlaces = [];
        DefineFunctionModeDecimalPlaces( this.FunctionModeDecimalPlaces, this.DefaultFunctionModeDecimalPlaces );
    }

    /// <summary> Gets or sets the function mode decimal places. </summary>
    /// <value> The function mode decimal places. </value>
    public IntegerDictionary FunctionModeDecimalPlaces { get; private set; }

    /// <summary> Converts a function Mode to a decimal places. </summary>
    /// <param name="functionMode"> The function mode. </param>
    /// <returns> FunctionMode as an Integer. </returns>
    public virtual int ToDecimalPlaces( int functionMode )
    {
        return this.FunctionModeDecimalPlaces[functionMode];
    }

    /// <summary> The function range decimal places. </summary>
    private int _functionRangeDecimalPlaces;

    /// <summary> Gets or sets the function range decimal places. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <exception cref="KeyNotFoundException">  Thrown when a Key Not Found error condition occurs. </exception>
    /// <value> The function range decimal places. </value>
    public int FunctionRangeDecimalPlaces
    {
        get => this._functionRangeDecimalPlaces;
        set
        {
            if ( this.FunctionRangeDecimalPlaces != value )
            {
                this._functionRangeDecimalPlaces = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " unit "

    /// <summary> Define function mode units. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <exception cref="KeyNotFoundException">  Thrown when a Key Not Found error condition occurs. </exception>
    /// <param name="functionModeUnits"> The function mode decimal places. </param>
    public static void DefineFunctionModeUnits( UnitDictionary functionModeUnits )
    {
        if ( functionModeUnits is null ) throw new ArgumentNullException( nameof( functionModeUnits ) );
        functionModeUnits[( int ) SourceFunctionModes.Current] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
        functionModeUnits[( int ) SourceFunctionModes.CurrentAC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
        functionModeUnits[( int ) SourceFunctionModes.CurrentDC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
        functionModeUnits[( int ) SourceFunctionModes.Memory] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) SourceFunctionModes.Voltage] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) SourceFunctionModes.VoltageAC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) SourceFunctionModes.VoltageDC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        foreach ( SourceFunctionModes functionMode in Enum.GetValues( typeof( SourceFunctionModes ) ) )
        {
            if ( functionMode != SourceFunctionModes.None && !functionModeUnits.ContainsKey( ( int ) functionMode ) )
            {
                throw new KeyNotFoundException( $"Unit not specified for source function mode {functionMode}" );
            }
        }
    }

    /// <summary> Gets or sets the default unit. </summary>
    /// <value> The default unit. </value>
    public cc.isr.UnitsAmounts.Unit DefaultFunctionUnit { get; set; }

    /// <summary> Define function mode units. </summary>
    private void DefineFunctionModeUnits()
    {
        this.FunctionModeUnits = [];
        DefineFunctionModeUnits( this.FunctionModeUnits );
    }

    /// <summary> Gets or sets the function mode decimal places. </summary>
    /// <value> The function mode decimal places. </value>
    public UnitDictionary FunctionModeUnits { get; private set; }

    /// <summary> Parse units. </summary>
    /// <param name="functionMode"> The  Multimeter Function Mode. </param>
    /// <returns> An cc.isr.UnitsAmounts.Unit. </returns>
    public virtual cc.isr.UnitsAmounts.Unit ToUnit( int functionMode )
    {
        return this.FunctionModeUnits[functionMode];
    }

    /// <summary> Gets or sets the function unit. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The function unit. </value>
    public cc.isr.UnitsAmounts.Unit FunctionUnit
    {
        get => this.Amount.Unit;
        set
        {
            if ( this.FunctionUnit != value )
            {
                this.NewAmount( value );
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " function mode "

    /// <summary> Define function mode read writes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="functionModeReadWrites"> A Dictionary of Source function mode parses. </param>
    public static void DefineFunctionModeReadWrites( Pith.EnumReadWriteCollection functionModeReadWrites )
    {
        if ( functionModeReadWrites is null ) throw new ArgumentNullException( nameof( functionModeReadWrites ) );
        functionModeReadWrites.Clear();
        foreach ( SourceFunctionModes functionMode in Enum.GetValues( typeof( SourceFunctionModes ) ) )
            functionModeReadWrites.Add( functionMode );
    }

    /// <summary> Define function mode read writes. </summary>
    private void DefineFunctionModeReadWrites()
    {
        this.FunctionModeReadWrites = [];
        DefineFunctionModeReadWrites( this.FunctionModeReadWrites );
    }

    /// <summary> Gets or sets a dictionary of Source function mode parses. </summary>
    /// <value> A Dictionary of Source function mode parses. </value>
    public Pith.EnumReadWriteCollection FunctionModeReadWrites { get; private set; }

    /// <summary> The supported function modes. </summary>
    private SourceFunctionModes _supportedFunctionModes;

    /// <summary>
    /// Gets or sets the supported Function Modes. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> The supported Source function modes. </value>
    public SourceFunctionModes SupportedFunctionModes
    {
        get => this._supportedFunctionModes;
        set
        {
            if ( !this.SupportedFunctionModes.Equals( value ) )
            {
                this._supportedFunctionModes = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Define function clear known state. </summary>
    protected virtual void DefineFunctionClearKnownState()
    {
    }

    /// <summary> The function mode. </summary>
    private SourceFunctionModes? _functionMode;

    /// <summary> Gets or sets the cached Source function mode. </summary>
    /// <value>
    /// The <see cref="FunctionMode">Source function mode</see> or none if not set or unknown.
    /// </value>
    public SourceFunctionModes? FunctionMode
    {
        get => this._functionMode;

        protected set
        {
            if ( !this.FunctionMode.Equals( value ) )
            {
                this._functionMode = value;
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
                    this.FunctionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
                }

                this.DefineFunctionClearKnownState();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Source function mode. </summary>
    /// <param name="value"> The  Source function mode. </param>
    /// <returns>
    /// The <see cref="FunctionMode">source Source function mode</see> or none if unknown.
    /// </returns>
    public virtual SourceFunctionModes? ApplyFunctionMode( SourceFunctionModes value )
    {
        _ = this.WriteFunctionMode( value );
        return this.QueryFunctionMode();
    }

    /// <summary> Gets or sets the Source function mode query command. </summary>
    /// <value> The Source function mode query command. </value>
    protected virtual string FunctionModeQueryCommand { get; set; } = string.Empty; // = ":SOUR:FUNC?"

    /// <summary> Queries the Source function mode. </summary>
    /// <returns>
    /// The <see cref="FunctionMode">Source function mode</see> or none if unknown.
    /// </returns>
    public virtual SourceFunctionModes? QueryFunctionMode()
    {
        return this.QueryFunctionMode( this.FunctionModeQueryCommand );
    }

    /// <summary> Queries the Source function mode. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>
    /// The <see cref="FunctionMode">Source function mode</see> or none if unknown.
    /// </returns>
    public virtual SourceFunctionModes? QueryFunctionMode( string queryCommand )
    {
        this.FunctionMode = this.Session.Query( this.FunctionMode.GetValueOrDefault( SourceFunctionModes.None ), this.FunctionModeReadWrites, queryCommand );
        return this.FunctionMode;
    }

    /// <summary> Gets or sets the Source function mode command format. </summary>
    /// <remarks> SCPI:  ":SOUR:FUNC {0}". </remarks>
    /// <value> The Source function mode command format. </value>
    protected virtual string FunctionModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Source function mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The Source function mode. </param>
    /// <returns>
    /// The <see cref="FunctionMode">Source function mode</see> or none if unknown.
    /// </returns>
    public virtual SourceFunctionModes? WriteFunctionMode( SourceFunctionModes value )
    {
        this.FunctionMode = this.Session.Write( value, this.FunctionModeCommandFormat, this.FunctionModeReadWrites );
        return this.FunctionMode;
    }

    #endregion
}
/// <summary>
/// Specifies the source function modes. Using flags permits using these values to define the
/// supported function modes.
/// </summary>
[Flags]
public enum SourceFunctionModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None = 0,

    /// <summary> An enum constant representing the voltage option. </summary>
    [System.ComponentModel.Description( "Voltage (VOLT)" )]
    Voltage = 1,

    /// <summary> An enum constant representing the current option. </summary>
    [System.ComponentModel.Description( "Current (CURR)" )]
    Current = 2,

    /// <summary> An enum constant representing the memory option. </summary>
    [System.ComponentModel.Description( "Memory (MEM)" )]
    Memory = 4,

    /// <summary> An enum constant representing the voltage Device-context option. </summary>
    [System.ComponentModel.Description( "DC Voltage (VOLT:DC)" )]
    VoltageDC = 8,

    /// <summary> An enum constant representing the current Device-context option. </summary>
    [System.ComponentModel.Description( "DC Current (CURR:DC)" )]
    CurrentDC = 16,

    /// <summary> An enum constant representing the voltage a c option. </summary>
    [System.ComponentModel.Description( "AC Voltage (VOLT:AC)" )]
    VoltageAC = 32,

    /// <summary> An enum constant representing the current a c option. </summary>
    [System.ComponentModel.Description( "AC Current (CURR:AC)" )]
    CurrentAC = 64
}
