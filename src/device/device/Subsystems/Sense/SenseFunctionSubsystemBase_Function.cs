// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

public abstract partial class SenseFunctionSubsystemBase
{
    #region " range "

    /// <summary> Define function mode ranges. </summary>
    private void DefineFunctionModeRanges()
    {
        this.FunctionModeRanges = [];
        SenseSubsystemBase.DefineFunctionModeRanges( this.FunctionModeRanges, this.DefaultFunctionRange );
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
    /// <value> The default decimal places. </value>
    public int DefaultFunctionModeDecimalPlaces { get; set; }

    /// <summary> Define function mode decimal places. </summary>
    private void DefineFunctionModeDecimalPlaces()
    {
        this.FunctionModeDecimalPlaces = [];
        SenseSubsystemBase.DefineFunctionModeDecimalPlaces( this.FunctionModeDecimalPlaces, this.DefaultFunctionModeDecimalPlaces );
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

    /// <summary> Gets or sets the default unit. </summary>
    /// <value> The default unit. </value>
    public cc.isr.UnitsAmounts.Unit DefaultFunctionUnit { get; set; }

    /// <summary> Define function mode units. </summary>
    private void DefineFunctionModeUnits()
    {
        this.FunctionModeUnits = [];
        SenseSubsystemBase.DefineFunctionModeUnits( this.FunctionModeUnits );
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
        this.NotifyPropertyChanged( nameof( MeasureSubsystemBase.ReadingAmounts ) );
    }

    /// <summary> Define function mode read writes. </summary>
    private void DefineFunctionModeReadWrites()
    {
        this.FunctionModeReadWrites = [];
        SenseSubsystemBase.DefineFunctionModeReadWrites( this.FunctionModeReadWrites );
    }

    /// <summary> Define function mode read writes. </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="readValueDecorator">  The read value decorator. </param>
    /// <param name="writeValueDecorator"> The write value decorator. </param>
    public void DefineFunctionModeReadWrites( string readValueDecorator, string writeValueDecorator )
    {
        this.FunctionModeReadWrites = new Pith.EnumReadWriteCollection()
        {
            ReadValueDecorator = readValueDecorator,
            WriteValueDecorator = writeValueDecorator
        };
        SenseSubsystemBase.DefineFunctionModeReadWrites( this.FunctionModeReadWrites );
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
