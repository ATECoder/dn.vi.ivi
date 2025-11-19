// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

public abstract partial class SenseSubsystemBase
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
        foreach ( SenseFunctionModes functionMode in Enum.GetValues( typeof( SenseFunctionModes ) ) )
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
    public int DefaultFunctionModeDecimalPlaces { get; set; } = 3;

    /// <summary> Define function mode decimal places. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="functionModeDecimalPlaces">        The function mode decimal places. </param>
    /// <param name="defaultFunctionModeDecimalPlaces"> The default decimal places. </param>
    public static void DefineFunctionModeDecimalPlaces( IntegerDictionary functionModeDecimalPlaces, int defaultFunctionModeDecimalPlaces )
    {
        if ( functionModeDecimalPlaces is null ) throw new ArgumentNullException( nameof( functionModeDecimalPlaces ) );
        functionModeDecimalPlaces.Clear();
        foreach ( SenseFunctionModes functionMode in Enum.GetValues( typeof( SenseFunctionModes ) ) )
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

    #region " units "

    /// <summary> Gets or sets the default unit. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <exception cref="KeyNotFoundException">  Thrown when a Key Not Found error condition occurs. </exception>
    /// <value> The default unit. </value>
    public cc.isr.UnitsAmounts.Unit DefaultFunctionUnit { get; set; } = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;

    /// <summary> Define function mode units. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <exception cref="KeyNotFoundException">  Thrown when a Key Not Found error condition occurs. </exception>
    /// <param name="functionModeUnits"> The function mode decimal places. </param>
    public static void DefineFunctionModeUnits( UnitDictionary functionModeUnits )
    {
        if ( functionModeUnits is null ) throw new ArgumentNullException( nameof( functionModeUnits ) );
        functionModeUnits[( int ) SenseFunctionModes.Continuity] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm;
        functionModeUnits[( int ) SenseFunctionModes.Current] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
        functionModeUnits[( int ) SenseFunctionModes.CurrentAC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
        functionModeUnits[( int ) SenseFunctionModes.CurrentACDC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
        functionModeUnits[( int ) SenseFunctionModes.CurrentDC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
        functionModeUnits[( int ) SenseFunctionModes.Diode] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) SenseFunctionModes.Frequency] = cc.isr.UnitsAmounts.StandardUnits.FrequencyUnits.Hertz;
        functionModeUnits[( int ) SenseFunctionModes.Memory] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) SenseFunctionModes.Period] = cc.isr.UnitsAmounts.StandardUnits.TimeUnits.Second;
        functionModeUnits[( int ) SenseFunctionModes.Resistance] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm;
        functionModeUnits[( int ) SenseFunctionModes.ResistanceFourWire] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm;
        functionModeUnits[( int ) SenseFunctionModes.StatusElement] = cc.isr.UnitsAmounts.StandardUnits.UnitlessUnits.Status;
        functionModeUnits[( int ) SenseFunctionModes.Temperature] = cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.Kelvin;
        functionModeUnits[( int ) SenseFunctionModes.TimestampElement] = cc.isr.UnitsAmounts.StandardUnits.TimeUnits.Second;
        functionModeUnits[( int ) SenseFunctionModes.Voltage] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) SenseFunctionModes.VoltageAC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) SenseFunctionModes.VoltageACDC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) SenseFunctionModes.VoltageDC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        foreach ( SenseFunctionModes functionMode in Enum.GetValues( typeof( SenseFunctionModes ) ) )
        {
            if ( functionMode != SenseFunctionModes.None && !functionModeUnits.ContainsKey( ( int ) functionMode ) )
            {
                throw new KeyNotFoundException( $"Unit not specified for Sense function mode {functionMode}" );
            }
        }
    }

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

    #region " function mode "

    /// <summary> Defines function clear known state. </summary>
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
        this.NotifyPropertyChanged( nameof( MeasureSubsystemBase.ReadingAmounts ) );
    }

    /// <summary> Define function mode read writes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="functionModeReadWrites"> A Dictionary of Sense function mode parses. </param>
    public static void DefineFunctionModeReadWrites( Pith.EnumReadWriteCollection functionModeReadWrites )
    {
        if ( functionModeReadWrites is null ) throw new ArgumentNullException( nameof( functionModeReadWrites ) );
        functionModeReadWrites.Clear();
        foreach ( SenseFunctionModes functionMode in Enum.GetValues( typeof( SenseFunctionModes ) ) )
            functionModeReadWrites.Add( functionMode );
    }

    /// <summary> Define function mode read writes. </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="readValueDecorator">  The read value decorator. Default "{0}"; K2002:
    /// """{0}""". </param>
    /// <param name="writeValueDecorator"> The write value decorator. Default "{0}"; K2002: "'{0}'". </param>
    public void DefineFunctionModeReadWrites( string readValueDecorator, string writeValueDecorator )
    {
        this.FunctionModeReadWrites = new Pith.EnumReadWriteCollection()
        {
            ReadValueDecorator = readValueDecorator,
            WriteValueDecorator = writeValueDecorator
        };
        DefineFunctionModeReadWrites( this.FunctionModeReadWrites );
    }

    /// <summary> Define function mode read writes. </summary>
    private void DefineFunctionModeReadWrites()
    {
        this.FunctionModeReadWrites = [];
        DefineFunctionModeReadWrites( this.FunctionModeReadWrites );
    }

    /// <summary> Gets or sets a dictionary of Sense function mode parses. </summary>
    /// <value> A Dictionary of Sense function mode parses. </value>
    public Pith.EnumReadWriteCollection FunctionModeReadWrites { get; private set; }

    /// <summary>
    /// Gets or sets the supported Function Modes. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> The supported Sense function modes. </value>
    public SenseFunctionModes SupportedFunctionModes
    {
        get;
        set
        {
            if ( !this.SupportedFunctionModes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached Sense function mode. </summary>
    /// <value>
    /// The <see cref="FunctionMode">Sense function mode</see> or none if not set or unknown.
    /// </value>
    public SenseFunctionModes? FunctionMode
    {
        get;

        protected set
        {
            if ( !this.FunctionMode.Equals( value ) )
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
                    this.FunctionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
                }

                this.DefineFunctionClearKnownState();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sense function mode. </summary>
    /// <param name="value"> The  Sense function mode. </param>
    /// <returns>
    /// The <see cref="FunctionMode">source Sense function mode</see> or none if unknown.
    /// </returns>
    public virtual SenseFunctionModes? ApplyFunctionMode( SenseFunctionModes value )
    {
        _ = this.WriteFunctionMode( value );
        return this.QueryFunctionMode();
    }

    /// <summary> Gets or sets the Sense function mode query command. </summary>
    /// <value> The Sense function mode query command. </value>
    protected virtual string FunctionModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Sense function mode. </summary>
    /// <returns> The <see cref="FunctionMode">Sense function mode</see> or none if unknown. </returns>
    public virtual SenseFunctionModes? QueryFunctionMode()
    {
        return this.QueryFunctionMode( this.FunctionModeQueryCommand );
    }

    /// <summary> Queries the Sense function mode. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns> The <see cref="FunctionMode">Sense function mode</see> or none if unknown. </returns>
    public virtual SenseFunctionModes? QueryFunctionMode( string queryCommand )
    {
        this.FunctionMode = this.Session.Query( this.FunctionMode.GetValueOrDefault( SenseFunctionModes.None ), this.FunctionModeReadWrites, queryCommand );
        return this.FunctionMode;
    }

    /// <summary>
    /// Queries the Sense Function Mode. Also sets the <see cref="FunctionMode"></see> cached value.
    /// </summary>
    /// <returns> The Sense Function Mode or null if unknown. </returns>
    [Obsolete( "Now using double quests for both read and writes" )]
    public SenseFunctionModes? QueryFunctionModeTrimQuotes()
    {
        // the instrument expects single quotes when writing the value but sends back items delimited with double quotes.
        string reading = this.Session.QueryTrimEnd( this.FunctionModeQueryCommand ).Trim( '"' );
        long v = this.Session.Parse( this.FunctionModeReadWrites, reading );
        this.FunctionMode = Enum.IsDefined( typeof( SenseFunctionModes ), v ) ? ( SenseFunctionModes ) ( int ) Enum.ToObject( typeof( SenseFunctionModes ), v ) : new SenseFunctionModes?();
        return this.FunctionMode;
    }

    /// <summary> Gets or sets the Sense function mode command format. </summary>
    /// <value> The Sense function mode command format. </value>
    protected virtual string FunctionModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Sense function mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The Sense function mode. </param>
    /// <returns> The <see cref="FunctionMode">Sense function mode</see> or none if unknown. </returns>
    public virtual SenseFunctionModes? WriteFunctionMode( SenseFunctionModes value )
    {
        this.FunctionMode = this.Session.Write( value, this.FunctionModeCommandFormat, this.FunctionModeReadWrites );
        return this.FunctionMode;
    }

    #endregion
}
/// <summary> Specifies the sense function modes. </summary>
/// <remarks> David, 2020-10-12. </remarks>
[Flags()]
public enum SenseFunctionModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not specified (NONE)" )]
    None = 0,

    /// <summary> An enum constant representing the voltage option. </summary>
    [System.ComponentModel.Description( "Voltage (VOLT)" )]
    Voltage = 1,

    /// <summary> An enum constant representing the current option. </summary>
    [System.ComponentModel.Description( "Current (CURR)" )]
    Current = Voltage << 1,

    /// <summary> An enum constant representing the voltage Device-context option. </summary>
    [System.ComponentModel.Description( "DC Voltage (VOLT:DC)" )]
    VoltageDC = Current << 1,

    /// <summary> An enum constant representing the current Device-context option. </summary>
    [System.ComponentModel.Description( "DC Current (CURR:DC)" )]
    CurrentDC = VoltageDC << 1,

    /// <summary> An enum constant representing the voltage a c option. </summary>
    [System.ComponentModel.Description( "AC Voltage (VOLT:AC)" )]
    VoltageAC = CurrentDC << 1,

    /// <summary> An enum constant representing the current a c option. </summary>
    [System.ComponentModel.Description( "AC Current (CURR:AC)" )]
    CurrentAC = VoltageAC << 1,

    /// <summary> An enum constant representing the resistance option. </summary>
    [System.ComponentModel.Description( "Resistance (RES)" )]
    Resistance = CurrentAC << 1,

    /// <summary> An enum constant representing the resistance four wire option. </summary>
    [System.ComponentModel.Description( "Four-Wire Resistance (FRES)" )]
    ResistanceFourWire = Resistance << 1,

    /// <summary> An enum constant representing the temperature option. </summary>
    [System.ComponentModel.Description( "Temperature (TEMP)" )]
    Temperature = ResistanceFourWire << 1,

    /// <summary> An enum constant representing the frequency option. </summary>
    [System.ComponentModel.Description( "Frequency (FREQ)" )]
    Frequency = Temperature << 1,

    /// <summary> An enum constant representing the period option. </summary>
    [System.ComponentModel.Description( "Period (PER)" )]
    Period = Frequency << 1,

    /// <summary> An enum constant representing the continuity option. </summary>
    [System.ComponentModel.Description( "Continuity (CONT)" )]
    Continuity = Period << 1,

    /// <summary> An enum constant representing the timestamp element option. </summary>
    [System.ComponentModel.Description( "Timestamp element (TIME)" )]
    TimestampElement = Continuity << 1,

    /// <summary> An enum constant representing the status element option. </summary>
    [System.ComponentModel.Description( "Status Element (STAT)" )]
    StatusElement = TimestampElement << 1,

    /// <summary> An enum constant representing the memory option. </summary>
    [System.ComponentModel.Description( "Memory (MEM)" )]
    Memory = StatusElement << 1,

    /// <summary> An enum constant representing the diode option. </summary>
    [System.ComponentModel.Description( "Diode (DIOD)" )]
    Diode = Memory << 1,

    /// <summary> An enum constant representing the current a cdc option. </summary>
    [System.ComponentModel.Description( "AC/DC Current (CURR:ACDC)" )]
    CurrentACDC = Diode << 1,

    /// <summary> An enum constant representing the voltage a cdc option. </summary>
    [System.ComponentModel.Description( "AC/DC Voltage (VOLT:ACDC)" )]
    VoltageACDC = CurrentACDC << 1
}
