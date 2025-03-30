using System.Diagnostics;
using cc.isr.Enums;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp2;

/// <summary>
/// Defines the contract that must be implemented by a Source Measure Unit Measure Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class MeasureSubsystemBase : VI.MeasureSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasureSubsystemBase" /> class.
    /// </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    /// <param name="readingAmounts">  A reference to a
    /// <see cref="isr.VI.StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected MeasureSubsystemBase( VI.StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem, readingAmounts )
    {
        this.DefaultMeasurementUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionRange = cc.isr.VI.Ranges.NonnegativeFullRange;
        this.DefaultFunctionModeDecimalPlaces = 3;
        this.FunctionModeReadWrites.AddReplace( ( long ) SenseFunctionModes.VoltageDC, "smu.FUNC_DC_VOLTAGE", SenseFunctionModes.VoltageDC.DescriptionUntil() );
        this.FunctionModeReadWrites.AddReplace( ( long ) SenseFunctionModes.CurrentDC, "smu.FUNC_DC_CURRENT", SenseFunctionModes.CurrentDC.DescriptionUntil() );
        this.FunctionModeReadWrites.AddReplace( ( long ) SenseFunctionModes.Resistance, "smu.FUNC_RESISTANCE", SenseFunctionModes.Resistance.DescriptionUntil() );
    }

    #endregion

    #region " auto range state "

    /// <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    /// <remarks>
    /// When this command is set to off, you must set the range. If you do not set the range, the
    /// instrument remains at the range that was selected by auto range. When this command Is set to
    /// on, the instrument automatically goes to the most sensitive range to perform the measurement.
    /// If a range Is manually selected through the front panel Or a remote command, this command Is
    /// automatically set to off. Auto range selects the best range In which To measure the signal
    /// that Is applied To the input terminals of the instrument. When auto range Is enabled, the
    /// range increases at 120 percent of range And decreases occurs When the reading Is less than 10
    /// percent Of nominal range. For example, If you are On the 1 volt range And auto range Is
    /// enabled, the instrument auto ranges up To the 10 volt range When the measurement exceeds 1.2
    /// volts. It auto ranges down To the 100 mV range When the measurement falls below 1 volt.
    /// </remarks>
    /// <value>
    /// <c>null</c> if Auto Range Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public override bool? AutoRangeEnabled
    {
        get => base.AutoRangeEnabled;
        protected set
        {
            if ( !Equals( this.AutoRangeEnabled, value ) )
            {
                this.AutoRangeState = value.HasValue ? value.Value ? OnOffState.On : OnOffState.Off : new OnOffState?();
                base.AutoRangeEnabled = value;
            }
        }
    }

    /// <summary> State of the automatic range. </summary>
    private OnOffState? _autoRangeState;

    /// <summary> Gets or sets the Auto Range. </summary>
    /// <value> The automatic range state. </value>
    public OnOffState? AutoRangeState
    {
        get => this._autoRangeState;
        protected set
        {
            if ( !Nullable.Equals( value, this.AutoRangeState ) )
            {
                this._autoRangeState = value;
                this.AutoRangeEnabled = value.HasValue ? value.Value == OnOffState.On : new bool?();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the AutoRange state. </summary>
    /// <param name="value"> The Aperture. </param>
    /// <returns> An OnOffState? </returns>
    public OnOffState? ApplyAutoRangeState( OnOffState value )
    {
        _ = this.WriteAutoRangeState( value );
        return this.QueryAutoRangeState();
    }

    /// <summary> Gets or sets the Auto Range state query command. </summary>
    /// <value> The Auto Range state query command. </value>
    protected virtual string AutoRangeStateQueryCommand { get; set; } = "_G.print(_G.smu.measure.autorange)";

    /// <summary> Queries automatic range state. </summary>
    /// <returns> The automatic range state. </returns>
    public OnOffState? QueryAutoRangeState()
    {
        this.Session.LastAction = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Reading {nameof( this.AutoRangeState )};. " );
        this.Session.LastNodeNumber = new int?();
        string mode = this.AutoRangeState.ToString();
        this.Session.MakeEmulatedReplyIfEmpty( mode );
        mode = this.Session.QueryTrimEnd( this.AutoRangeStateQueryCommand );
        if ( string.IsNullOrWhiteSpace( mode ) )
        {
            string message = $"Failed fetching {nameof( this.AutoRangeState )}";
            Debug.Assert( !Debugger.IsAttached, message );
            this.AutoRangeState = new OnOffState?();
        }
        else
        {
            this.AutoRangeState = SessionBase.ParseContained<OnOffState>( mode.BuildDelimitedValue() );
        }

        return this.AutoRangeState;
    }

    /// <summary> The Auto Range state command format. </summary>
    /// <value> The automatic range state command format. </value>
    protected virtual string AutoRangeStateCommandFormat { get; set; } = "_G.smu.measure.autorange={0}";

    /// <summary> Writes an automatic range state. </summary>
    /// <param name="value"> The Aperture. </param>
    /// <returns> An OnOffState. </returns>
    public OnOffState WriteAutoRangeState( OnOffState value )
    {
        this.Session.LastAction = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Writing {nameof( this.AutoRangeState )}={value};. " );
        this.Session.LastNodeNumber = new int?();
        _ = this.Session.WriteLine( this.AutoRangeStateCommandFormat, value.ExtractBetween() );
        this.AutoRangeState = value;
        return value;
    }

    #endregion

    #region " measure unit "

    /// <summary> Converts a a measure unit to a measurement unit. </summary>
    /// <param name="value"> The Measure Unit. </param>
    /// <returns> Value as an cc.isr.UnitsAmounts.Unit. </returns>
    public cc.isr.UnitsAmounts.Unit ToMeasurementUnit( MeasureUnits value )
    {
        UnitsAmounts.Unit result = this.DefaultFunctionUnit;
        switch ( value )
        {
            case MeasureUnits.Ampere:
                {
                    result = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;
                    break;
                }

            case MeasureUnits.Volt:
                {
                    result = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
                    break;
                }

            case MeasureUnits.Ohm:
                {
                    result = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm;
                    break;
                }

            case MeasureUnits.Watt:
                {
                    result = cc.isr.UnitsAmounts.StandardUnits.EnergyUnits.Watt;
                    break;
                }
        }

        return result;
    }

    /// <summary> Writes and reads back the Measure Unit. </summary>
    /// <param name="value"> The  Measure Unit. </param>
    /// <returns> The <see cref="MeasureUnit">Measure Unit</see> or none if unknown. </returns>
    public MeasureUnits? ApplyMeasureUnit( MeasureUnits value )
    {
        _ = this.WriteMeasureUnit( value );
        return this.QueryMeasureUnit();
    }

    /// <summary> The Unit. </summary>
    private MeasureUnits? _measureUnit;

    /// <summary>
    /// Gets or sets the cached measure Unit.  This is the actual unit for measurement.
    /// </summary>
    /// <value>
    /// The <see cref="MeasureUnit">Measure Unit</see> or none if not set or unknown.
    /// </value>
    public MeasureUnits? MeasureUnit
    {
        get => this._measureUnit;
        protected set
        {
            if ( !Nullable.Equals( this.MeasureUnit, value ) )
            {
                this._measureUnit = value;
                if ( value.HasValue )
                {
                    this.MeasurementUnit = this.ToMeasurementUnit( value.Value );
                }

                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the Measure Unit query command. </summary>
    /// <value> The Unit query command. </value>
    protected virtual string MeasureUnitQueryCommand { get; set; } = "_G.print(_G.smu.measure.unit)";

    /// <summary> Queries the Measure Unit. </summary>
    /// <returns> The <see cref="MeasureUnit">Measure Unit</see> or none if unknown. </returns>
    public virtual MeasureUnits? QueryMeasureUnit()
    {
        string mode = this.MeasureUnit.ToString();
        this.Session.MakeEmulatedReplyIfEmpty( mode );
        mode = this.Session.QueryTrimEnd( this.MeasureUnitQueryCommand );
        if ( string.IsNullOrWhiteSpace( mode ) )
        {
            string message = $"Failed fetching {nameof( MeasureSubsystemBase )}.{nameof( this.MeasureUnit )}";
            Debug.Assert( !Debugger.IsAttached, message );
            this.MeasureUnit = new MeasureUnits?();
        }
        else
        {
            this.MeasureUnit = SessionBase.ParseContained<MeasureUnits>( mode.BuildDelimitedValue() );
        }

        return this.MeasureUnit;
    }

    /// <summary> Gets or sets the Measure Unit command format. </summary>
    /// <value> The Unit command format. </value>
    protected virtual string MeasureUnitCommandFormat { get; set; } = "_G.smu.measure.unit={0}";

    /// <summary> Writes the Measure Unit without reading back the value from the device. </summary>
    /// <param name="value"> The Unit. </param>
    /// <returns> The <see cref="MeasureUnit">Measure Unit</see> or none if unknown. </returns>
    public virtual MeasureUnits? WriteMeasureUnit( MeasureUnits value )
    {
        _ = this.Session.WriteLine( this.MeasureUnitCommandFormat, value.ExtractBetween() );
        this.MeasureUnit = value;
        return this.MeasureUnit;
    }

    #endregion

}

/// <summary> Specifies the units. </summary>
[Flags]
public enum MeasureUnits
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None = 0,

    /// <summary> An enum constant representing the volt option. </summary>
    [System.ComponentModel.Description( "Volt (smu.UNIT_VOLT)" )]
    Volt = 1,

    /// <summary> An enum constant representing the ohm option. </summary>
    [System.ComponentModel.Description( "Ohm (smu.UNIT_OHM)" )]
    Ohm = 2,

    /// <summary> An enum constant representing the ampere option. </summary>
    [System.ComponentModel.Description( "Ampere (smu.UNIT_AMP)" )]
    Ampere = 4,

    /// <summary> An enum constant representing the watt option. </summary>
    [System.ComponentModel.Description( "Watt (smu.UNIT_WATT)" )]
    Watt = 8
}
