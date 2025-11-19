// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

public abstract partial class MultimeterSubsystemBase
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
        foreach ( MultimeterFunctionModes functionMode in Enum.GetValues( typeof( MultimeterFunctionModes ) ) )
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
    /// <value> The function range. </value>
    public Std.Primitives.RangeR FunctionRange
    {
        get => this._functionRange;

        protected set
        {
            if ( this.FunctionRange != value )
            {
                this._functionRange = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Establish range. </summary>
    /// <param name="initialValue">     The initial value. </param>
    /// <param name="rangeScaleFactor"> The range scale factor. </param>
    /// <param name="countOut">         The count out. </param>
    /// <returns> The MetaStatus. </returns>
    public MetaStatus EstablishRange( double initialValue, double rangeScaleFactor, int countOut )
    {
        countOut -= 1;
        if ( this.AutoRangeEnabled.GetValueOrDefault( true ) )
        {
            // disabling auto range
            _ = this.ApplyAutoRangeEnabled( false );
        }

        // set range candidate
        _ = this.ApplyRange( initialValue );

        // take a measurement
        double? value = this.MeasureReadingAmounts();

        // get the meta status
        MetaStatus result = this.PrimaryReading!.MetaStatus;

        // check if success
        if ( value.HasValue && this.PrimaryReading.MetaStatus.IsSet() )
        {
            if ( this.PrimaryReading.MetaStatus.Infinity || this.PrimaryReading.MetaStatus.NegativeInfinity )
            {
                // if infinity, increase the range
                initialValue *= rangeScaleFactor;
                if ( countOut > 0 )
                {
                    result = this.EstablishRange( initialValue, rangeScaleFactor, countOut );
                }
            }
            else
            {
                // if has value then done.
            }
        }
        else if ( countOut > 0 )
        {
            // 2-wire range measurement failed --trying again
            result = this.EstablishRange( initialValue, rangeScaleFactor, countOut );
        }
        else
        {
            // if count out, done
        }

        return result;
    }

    #endregion

    #region " decimal places "

    /// <summary> Define function mode decimal places. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="functionModeDecimalPlaces">        The function mode decimal places. </param>
    /// <param name="defaultFunctionModeDecimalPlaces"> The default decimal places. </param>
    public static void DefineFunctionModeDecimalPlaces( IntegerDictionary functionModeDecimalPlaces, int defaultFunctionModeDecimalPlaces )
    {
        if ( functionModeDecimalPlaces is null ) throw new ArgumentNullException( nameof( functionModeDecimalPlaces ) );
        functionModeDecimalPlaces.Clear();
        foreach ( MultimeterFunctionModes functionMode in Enum.GetValues( typeof( MultimeterFunctionModes ) ) )
            functionModeDecimalPlaces.Add( ( int ) functionMode, defaultFunctionModeDecimalPlaces );
    }

    /// <summary> Define function mode decimal places. </summary>
    private void DefineFunctionModeDecimalPlaces()
    {
        this.FunctionModeDecimalPlaces = [];
        DefineFunctionModeDecimalPlaces( this.FunctionModeDecimalPlaces, this.DefaultFunctionModeDecimalPlaces );
    }

    /// <summary> Gets or sets the default decimal places. </summary>
    /// <value> The default decimal places. </value>
    public int DefaultFunctionModeDecimalPlaces { get; set; }

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

        protected set
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
        functionModeUnits[( int ) MultimeterFunctionModes.CurrentAC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
        functionModeUnits[( int ) MultimeterFunctionModes.CurrentDC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
        functionModeUnits[( int ) MultimeterFunctionModes.ResistanceTwoWire] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm;
        functionModeUnits[( int ) MultimeterFunctionModes.ResistanceFourWire] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm;
        functionModeUnits[( int ) MultimeterFunctionModes.VoltageAC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) MultimeterFunctionModes.VoltageDC] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) MultimeterFunctionModes.Capacitance] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Farad;
        functionModeUnits[( int ) MultimeterFunctionModes.Continuity] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm;
        functionModeUnits[( int ) MultimeterFunctionModes.Diode] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        functionModeUnits[( int ) MultimeterFunctionModes.Frequency] = cc.isr.UnitsAmounts.StandardUnits.FrequencyUnits.Hertz;
        functionModeUnits[( int ) MultimeterFunctionModes.Period] = cc.isr.UnitsAmounts.StandardUnits.TimeUnits.Second;
        functionModeUnits[( int ) MultimeterFunctionModes.Ratio] = cc.isr.UnitsAmounts.StandardUnits.UnitlessUnits.Ratio;
        functionModeUnits[( int ) MultimeterFunctionModes.ResistanceCommonSide] = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm;
        functionModeUnits[( int ) MultimeterFunctionModes.Temperature] = cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.Kelvin;
        foreach ( MultimeterFunctionModes functionMode in Enum.GetValues( typeof( MultimeterFunctionModes ) ) )
        {
            if ( functionMode != MultimeterFunctionModes.None && !functionModeUnits.ContainsKey( ( int ) functionMode ) )
            {
                throw new KeyNotFoundException( $"Unit not specified for multimeter function mode {functionMode}" );
            }
        }
    }

    /// <summary> Define function mode units. </summary>
    private void DefineFunctionModeUnits()
    {
        this.FunctionModeUnits = [];
        DefineFunctionModeUnits( this.FunctionModeUnits );
    }

    /// <summary> Gets or sets the default unit. </summary>
    /// <value> The default unit. </value>
    public cc.isr.UnitsAmounts.Unit DefaultFunctionUnit { get; set; }

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

        protected set
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

    /// <summary> Define function clear known state. </summary>
    protected virtual void DefineFunctionClearKnownState()
    {
        if ( this.ReadingAmounts.HasReadingElements() )
        {
            this.ReadingAmounts.ActiveReadingAmount().ApplyUnit( this.FunctionUnit );
        }

        this.ReadingAmounts.Reset();
        this.NotifyPropertyChanged( nameof( MeasureSubsystemBase.ReadingAmounts ) );
        _ = this.ParsePrimaryReading( string.Empty );
    }

    /// <summary> Define function mode read writes. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="functionModeReadWrites"> A Dictionary of multimeter function mode parses. </param>
    public static void DefineFunctionModeReadWrites( Pith.EnumReadWriteCollection functionModeReadWrites )
    {
        if ( functionModeReadWrites is null ) throw new ArgumentNullException( nameof( functionModeReadWrites ) );
        functionModeReadWrites.Clear();
        foreach ( MultimeterFunctionModes functionMode in Enum.GetValues( typeof( MultimeterFunctionModes ) ) )
            functionModeReadWrites.Add( functionMode );
    }

    /// <summary> Define function mode read writes. </summary>
    private void DefineFunctionModeReadWrites()
    {
        this.FunctionModeReadWrites = [];
        DefineFunctionModeReadWrites( this.FunctionModeReadWrites );
    }

    /// <summary> Gets or sets a dictionary of multimeter function mode parses. </summary>
    /// <value> A Dictionary of multimeter function mode parses. </value>
    public Pith.EnumReadWriteCollection FunctionModeReadWrites { get; private set; }

    /// <summary>
    /// Gets or sets the supported Function Modes. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> The supported multimeter function modes. </value>
    public MultimeterFunctionModes SupportedFunctionModes
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

    /// <summary> Gets or sets the cached multimeter function mode. </summary>
    /// <value>
    /// The <see cref="FunctionMode">multimeter function mode</see> or none if not set or unknown.
    /// </value>
    public MultimeterFunctionModes? FunctionMode
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
                    this.FunctionOpenDetectorCapable = this.IsOpenDetectorCapable( ( int ) value.Value );
                }
                else
                {
                    this.FunctionRange = this.DefaultFunctionRange;
                    this.FunctionUnit = this.DefaultFunctionUnit;
                    this.FunctionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
                    this.FunctionOpenDetectorCapable = this.DefaultOpenDetectorCapable;
                }

                if ( !this.FunctionOpenDetectorCapable )
                    this.OpenDetectorEnabled = false;
                this.DefineFunctionClearKnownState();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the multimeter function mode. </summary>
    /// <param name="value"> The  multimeter function mode. </param>
    /// <returns>
    /// The <see cref="FunctionMode">source multimeter function mode</see> or none if unknown.
    /// </returns>
    public virtual MultimeterFunctionModes? ApplyFunctionMode( MultimeterFunctionModes value )
    {
        _ = this.WriteFunctionMode( value );
        return this.QueryFunctionMode();
    }

    /// <summary> Gets or sets the multimeter function mode query command. </summary>
    /// <value> The multimeter function mode query command. </value>
    protected virtual string FunctionModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the multimeter function mode. </summary>
    /// <returns>
    /// The <see cref="FunctionMode">multimeter function mode</see> or none if unknown.
    /// </returns>
    public virtual MultimeterFunctionModes? QueryFunctionMode()
    {
        return this.QueryFunctionMode( this.FunctionModeQueryCommand );
    }

    /// <summary> Queries the multimeter function mode. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>
    /// The <see cref="FunctionMode">multimeter function mode</see> or none if unknown.
    /// </returns>
    public virtual MultimeterFunctionModes? QueryFunctionMode( string queryCommand )
    {
        this.FunctionMode = this.Session.Query( this.FunctionMode.GetValueOrDefault( MultimeterFunctionModes.None ), this.FunctionModeReadWrites, queryCommand );
        return this.FunctionMode;
    }

    /// <summary> Gets or sets the multimeter function mode command format. </summary>
    /// <value> The multimeter function mode command format. </value>
    protected virtual string FunctionModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the multimeter function mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The multimeter function mode. </param>
    /// <returns>
    /// The <see cref="FunctionMode">multimeter function mode</see> or none if unknown.
    /// </returns>
    public virtual MultimeterFunctionModes? WriteFunctionMode( MultimeterFunctionModes value )
    {
        this.FunctionMode = this.Session.Write( value, this.FunctionModeCommandFormat, this.FunctionModeReadWrites );
        return this.FunctionMode;
    }

    #endregion

    #region " input impedance mode "

    /// <summary> Define input impedance mode read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineInputImpedanceModeReadWrites()
    {
        this.InputImpedanceModeReadWrites = new();
        foreach ( InputImpedanceModes enumValue in Enum.GetValues( typeof( InputImpedanceModes ) ) )
            this.InputImpedanceModeReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of multimeter Input Impedance Mode parses. </summary>
    /// <value> A Dictionary of multimeter Input Impedance Mode parses. </value>
    public Pith.EnumReadWriteCollection InputImpedanceModeReadWrites { get; private set; }

    /// <summary>
    /// Gets or sets the supported Input Impedance Modes. This is a subset of the InputImpedances
    /// supported by the instrument.
    /// </summary>
    /// <value> The supported multimeter Input Impedance Modes. </value>
    public InputImpedanceModes SupportedInputImpedanceModes
    {
        get;
        set
        {
            if ( !this.SupportedInputImpedanceModes.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached multimeter Input Impedance Mode. </summary>
    /// <value>
    /// The <see cref="InputImpedanceMode">multimeter Input Impedance Mode</see> or none if not set
    /// or unknown.
    /// </value>
    public InputImpedanceModes? InputImpedanceMode
    {
        get;

        protected set
        {
            if ( !this.InputImpedanceMode.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the multimeter Input Impedance Mode. </summary>
    /// <param name="value"> The  multimeter Input Impedance Mode. </param>
    /// <returns>
    /// The <see cref="InputImpedanceMode">source multimeter Input Impedance Mode</see> or none if
    /// unknown.
    /// </returns>
    public virtual InputImpedanceModes? ApplyInputImpedanceMode( InputImpedanceModes value )
    {
        _ = this.WriteInputImpedanceMode( value );
        return this.QueryInputImpedanceMode();
    }

    /// <summary> Gets or sets the multimeter Input Impedance Mode query command. </summary>
    /// <value> The multimeter Input Impedance Mode query command. </value>
    protected virtual string InputImpedanceModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the multimeter Input Impedance Mode. </summary>
    /// <returns>
    /// The <see cref="InputImpedanceMode">multimeter Input Impedance Mode</see> or none if unknown.
    /// </returns>
    public virtual InputImpedanceModes? QueryInputImpedanceMode()
    {
        return this.QueryInputImpedanceMode( this.InputImpedanceModeQueryCommand );
    }

    /// <summary> Queries the multimeter Input Impedance Mode. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>
    /// The <see cref="InputImpedanceMode">multimeter Input Impedance Mode</see> or none if unknown.
    /// </returns>
    public virtual InputImpedanceModes? QueryInputImpedanceMode( string queryCommand )
    {
        this.InputImpedanceMode = this.Session.Query( this.InputImpedanceMode.GetValueOrDefault( InputImpedanceModes.None ), this.InputImpedanceModeReadWrites, queryCommand );
        return this.InputImpedanceMode;
    }

    /// <summary> Gets or sets the multimeter Input Impedance Mode command format. </summary>
    /// <value> The multimeter Input Impedance Mode command format. </value>
    protected virtual string InputImpedanceModeCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the multimeter Input Impedance Mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The multimeter Input Impedance Mode. </param>
    /// <returns>
    /// The <see cref="InputImpedanceMode">multimeter Input Impedance Mode</see> or none if unknown.
    /// </returns>
    public virtual InputImpedanceModes? WriteInputImpedanceMode( InputImpedanceModes value )
    {
        this.InputImpedanceMode = this.Session.Write( value, this.InputImpedanceModeCommandFormat, this.InputImpedanceModeReadWrites );
        return this.InputImpedanceMode;
    }

    #endregion

    #region " measurement unit "

    /// <summary> Define multimeter measurement unit read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineMultimeterMeasurementUnitReadWrites()
    {
        this.MultimeterMeasurementUnitReadWrites = new();
        foreach ( MultimeterMeasurementUnits enumValue in Enum.GetValues( typeof( MultimeterMeasurementUnits ) ) )
            this.MultimeterMeasurementUnitReadWrites.Add( enumValue );
    }

    /// <summary> Converts a a Multimeter unit to a measurement unit. </summary>
    /// <param name="value"> The Multimeter Unit. </param>
    /// <returns> Value as an cc.isr.UnitsAmounts.Unit. </returns>
    public cc.isr.UnitsAmounts.Unit ToMeasurementUnit( MultimeterMeasurementUnits value )
    {
        cc.isr.UnitsAmounts.Unit result = this.FunctionUnit;
        switch ( value )
        {
            case MultimeterMeasurementUnits.Celsius:
                {
                    result = cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.DegreeCelsius;
                    break;
                }

            case MultimeterMeasurementUnits.Decibel:
                {
                    result = cc.isr.UnitsAmounts.StandardUnits.UnitlessUnits.Decibel;
                    break;
                }

            case MultimeterMeasurementUnits.Fahrenheit:
                {
                    result = cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.DegreeFahrenheit;
                    break;
                }

            case MultimeterMeasurementUnits.Kelvin:
                {
                    result = cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.Kelvin;
                    break;
                }

            case MultimeterMeasurementUnits.Volt:
                {
                    result = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
                    break;
                }

            case MultimeterMeasurementUnits.None:
                break;

            default:
                {
                    result = this.FunctionUnit;
                    break;
                }
        }

        return result;
    }

    /// <summary> Gets or sets a dictionary of multimeter Input Impedance Mode parses. </summary>
    /// <value> A Dictionary of multimeter Input Impedance Mode parses. </value>
    public Pith.EnumReadWriteCollection MultimeterMeasurementUnitReadWrites { get; private set; }

    /// <summary>
    /// Gets or sets the supported Input Impedance Modes. This is a subset of the InputImpedances
    /// supported by the instrument.
    /// </summary>
    /// <value> The supported multimeter Input Impedance Modes. </value>
    public MultimeterMeasurementUnits SupportedMultimeterMeasurementUnits
    {
        get;
        set
        {
            if ( !this.SupportedMultimeterMeasurementUnits.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached multimeter Input Impedance Mode. </summary>
    /// <value>
    /// The <see cref="MultimeterMeasurementUnit">multimeter Input Impedance Mode</see> or none if
    /// not set or unknown.
    /// </value>
    public MultimeterMeasurementUnits? MultimeterMeasurementUnit
    {
        get;

        protected set
        {
            if ( !this.MultimeterMeasurementUnit.Equals( value ) )
            {
                field = value;
                if ( value.HasValue )
                {
                    this.FunctionUnit = this.ToMeasurementUnit( value.Value );
                }

                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the multimeter Input Impedance Mode. </summary>
    /// <param name="value"> The  multimeter Input Impedance Mode. </param>
    /// <returns>
    /// The <see cref="MultimeterMeasurementUnit">source multimeter Input Impedance Mode</see> or
    /// none if unknown.
    /// </returns>
    public virtual MultimeterMeasurementUnits? ApplyMultimeterMeasurementUnit( MultimeterMeasurementUnits value )
    {
        _ = this.WriteMultimeterMeasurementUnit( value );
        return this.QueryMultimeterMeasurementUnit();
    }

    /// <summary> Gets or sets the multimeter Input Impedance Mode query command. </summary>
    /// <value> The multimeter Input Impedance Mode query command. </value>
    protected virtual string MultimeterMeasurementUnitQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the multimeter Input Impedance Mode. </summary>
    /// <returns>
    /// The <see cref="MultimeterMeasurementUnit">multimeter Input Impedance Mode</see> or none if
    /// unknown.
    /// </returns>
    public virtual MultimeterMeasurementUnits? QueryMultimeterMeasurementUnit()
    {
        return this.QueryMultimeterMeasurementUnit( this.MultimeterMeasurementUnitQueryCommand );
    }

    /// <summary> Queries the multimeter Input Impedance Mode. </summary>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>
    /// The <see cref="MultimeterMeasurementUnit">multimeter Input Impedance Mode</see> or none if
    /// unknown.
    /// </returns>
    public virtual MultimeterMeasurementUnits? QueryMultimeterMeasurementUnit( string queryCommand )
    {
        this.MultimeterMeasurementUnit = this.Session.Query( this.MultimeterMeasurementUnit.GetValueOrDefault( MultimeterMeasurementUnits.None ), this.MultimeterMeasurementUnitReadWrites,
            queryCommand );
        return this.MultimeterMeasurementUnit;
    }

    /// <summary> Gets or sets the multimeter Input Impedance Mode command format. </summary>
    /// <value> The multimeter Input Impedance Mode command format. </value>
    protected virtual string MultimeterMeasurementUnitCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the multimeter Input Impedance Mode without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The multimeter Input Impedance Mode. </param>
    /// <returns>
    /// The <see cref="MultimeterMeasurementUnit">multimeter Input Impedance Mode</see> or none if
    /// unknown.
    /// </returns>
    public virtual MultimeterMeasurementUnits? WriteMultimeterMeasurementUnit( MultimeterMeasurementUnits value )
    {
        this.MultimeterMeasurementUnit = this.Session.Write( value, this.MultimeterMeasurementUnitCommandFormat, this.MultimeterMeasurementUnitReadWrites );
        return this.MultimeterMeasurementUnit;
    }

    #endregion
}
/// <summary> Values that represent input impedance modes. </summary>
[Flags]
public enum InputImpedanceModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 0,

    /// <summary> An enum constant representing the automatic option. </summary>
    [System.ComponentModel.Description( "Automatic (dmm.IMPEDANCE_AUTO)" )]
    Automatic = 1,

    /// <summary> An enum constant representing the ten mega ohm option. </summary>
    [System.ComponentModel.Description( "Ten Mega Ohm (dmm.IMPEDANCE_10M)" )]
    TenMegaOhm = 2
}
/// <summary> Specifies the units. </summary>
[Flags]
public enum MultimeterMeasurementUnits
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 0,

    /// <summary> An enum constant representing the volt option. </summary>
    [System.ComponentModel.Description( "Volt (dmm.UNIT_VOLT)" )]
    Volt = 1,

    /// <summary> An enum constant representing the decibel option. </summary>
    [System.ComponentModel.Description( "Decibel (dmm.UNIT_DB)" )]
    Decibel = 2,

    /// <summary> An enum constant representing the Celsius option. </summary>
    [System.ComponentModel.Description( "Celsius (dmm.UNIT_CELCIUS)" )]
    Celsius = 4,

    /// <summary> An enum constant representing the kelvin option. </summary>
    [System.ComponentModel.Description( "Kelvin (dmm.UNIT_KELVIN)" )]
    Kelvin = 8,

    /// <summary> An enum constant representing the Fahrenheit option. </summary>
    [System.ComponentModel.Description( "Fahrenheit (dmm.UNIT_FAHRENHEIT)" )]
    Fahrenheit = 16
}
/// <summary> Specifies the function modes. </summary>
[Flags]
public enum MultimeterFunctionModes
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 0,

    /// <summary> An enum constant representing the voltage Device-context option. </summary>
    [System.ComponentModel.Description( "DC Voltage (dmm.FUNC_DC_VOLTAGE)" )]
    VoltageDC = 1,

    /// <summary> An enum constant representing the voltage a c option. </summary>
    [System.ComponentModel.Description( "AC Voltage (dmm.FUNC_AC_VOLTAGE)" )]
    VoltageAC = 2,

    /// <summary> An enum constant representing the current Device-context option. </summary>
    [System.ComponentModel.Description( "DC Current (dmm.FUNC_DC_CURRENT)" )]
    CurrentDC = 4,

    /// <summary> An enum constant representing the current a c option. </summary>
    [System.ComponentModel.Description( "AC Current (dmm.FUNC_AC_CURRENT)" )]
    CurrentAC = 8,

    /// <summary> An enum constant representing the temperature option. </summary>
    [System.ComponentModel.Description( "Temperature (dmm.FUNC_TEMPERATURE)" )]
    Temperature = 16,

    /// <summary> An enum constant representing the resistance common side option. </summary>
    [System.ComponentModel.Description( "Resistance Common Side (commonsideohms)" )]
    ResistanceCommonSide = 32,

    /// <summary> An enum constant representing the resistance two wire option. </summary>
    [System.ComponentModel.Description( "Resistance 2-Wire (dmm.FUNC_RESISTANCE)" )]
    ResistanceTwoWire = 64,

    /// <summary> An enum constant representing the resistance four wire option. </summary>
    [System.ComponentModel.Description( "Resistance 4-Wire (dmm.FUNC_4W_RESISTANCE)" )]
    ResistanceFourWire = 128,

    /// <summary> An enum constant representing the diode option. </summary>
    [System.ComponentModel.Description( "Diode (dmm.FUNC_DIODE)" )]
    Diode = 256,

    /// <summary> An enum constant representing the capacitance option. </summary>
    [System.ComponentModel.Description( "Capacitance (dmm.FUNC_CAPACITANCE)" )]
    Capacitance = 512,

    /// <summary> An enum constant representing the continuity option. </summary>
    [System.ComponentModel.Description( "Continuity (dmm.FUNC_CONTINUITY)" )]
    Continuity = 1024,

    /// <summary> An enum constant representing the frequency option. </summary>
    [System.ComponentModel.Description( "Frequency (dmm.FUNC_ACV_FREQUENCY)" )]
    Frequency = 2048,

    /// <summary> An enum constant representing the period option. </summary>
    [System.ComponentModel.Description( "Period (dmm.FUNC_ACV_PERIOD)" )]
    Period = 4096,

    /// <summary> An enum constant representing the ratio option. </summary>
    [System.ComponentModel.Description( "Ratio (dmm.FUNC_DCV_RATIO)" )]
    Ratio = 8192
}
