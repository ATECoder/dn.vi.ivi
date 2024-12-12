using System.ComponentModel;
using System.Diagnostics;
using cc.isr.Enums;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp;

/// <summary> Defines the contract that must be implemented by a Source Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SourceSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
/// Subsystem</see>. </param>
public class SourceSubsystemBase( Tsp.StatusSubsystemBase statusSubsystem ) : SourceMeasureUnitBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.SourceFunction = SourceFunctionMode.VoltageDC;
    }

    #endregion

    #region " source function "

    /// <summary> The Source Function. </summary>
    private SourceFunctionMode? _sourceFunction;

    /// <summary> Gets or sets the cached Source Function. </summary>
    /// <value>
    /// The <see cref="SourceFunctionMode">Source Function</see> or none if not set or unknown.
    /// </value>
    public SourceFunctionMode? SourceFunction
    {
        get => this._sourceFunction;
        protected set => _ = this.SetProperty( ref this._sourceFunction, value );
    }

    /// <summary> Writes and reads back the Source Function. </summary>
    /// <param name="value"> The  Source Function. </param>
    /// <returns>
    /// The <see cref="SourceFunctionMode">Source Function</see> or none if unknown.
    /// </returns>
    public SourceFunctionMode? ApplySourceFunction( SourceFunctionMode value )
    {
        _ = this.WriteSourceFunction( value );
        return this.QuerySourceFunction();
    }

    /// <summary> Queries the Source Function. </summary>
    /// <returns>
    /// The <see cref="SourceFunctionMode">Source Function</see> or none if unknown.
    /// </returns>
    public SourceFunctionMode? QuerySourceFunction()
    {
        string currentValue = this.SourceFunction.ToString();
        this.Session.MakeEmulatedReplyIfEmpty( currentValue );
        currentValue = this.Session.QueryTrimEnd( $"_G.print({this.SourceMeasureUnitReference}.source.func)" );
        if ( string.IsNullOrWhiteSpace( currentValue ) )
        {
            string message = "Failed fetching Source Function";
            Debug.Assert( !Debugger.IsAttached, message );
            this.SourceFunction = new SourceFunctionMode?();
        }
        else
        {
            // var se = new StringEnumerator<SourceFunctionMode>();
            // this.SourceFunction = se.ParseContained( currentValue.BuildDelimitedValue() );
            this.SourceFunction = SessionBase.ParseContained<SourceFunctionMode>( currentValue.BuildDelimitedValue() );
        }

        return this.SourceFunction;
    }

    /// <summary> Writes the Source Function Without reading back the value from the device. </summary>
    /// <param name="value"> The Source Function. </param>
    /// <returns>
    /// The <see cref="SourceFunctionMode">Source Function</see> or none if unknown.
    /// </returns>
    public SourceFunctionMode? WriteSourceFunction( SourceFunctionMode value )
    {
        _ = this.Session.WriteLine( "{0}.source.func = {0}.{1}", this.SourceMeasureUnitReference, value.ExtractBetween() );
        this.SourceFunction = value;
        return this.SourceFunction;
    }

    #endregion
}
/// <summary> Specifies the source function modes. </summary>
public enum SourceFunctionMode
{
    /// <summary> An enum constant representing the none option. </summary>
    [Description( "None" )]
    None,

    /// <summary> An enum constant representing the voltage Device-context option. </summary>
    [Description( "DC Voltage (OUTPUT_DCVOLTS)" )]
    VoltageDC,

    /// <summary> An enum constant representing the current Device-context option. </summary>
    [Description( "DC Current (OUTPUT_DCAMPS)" )]
    CurrentDC
}
