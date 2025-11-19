// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Thermal Transient Estimator Measure Subsystem. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-23 </para>
/// </remarks>
public class ThermalTransientEstimator : MeterSubsystemBase
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    public ThermalTransientEstimator( VI.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem ) => this.MeterEntity = ThermalTransientMeterEntity.Estimator;

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        /* Moved to Preset
            this.ReadInstrumentDefaults();
            this.ApplyInstrumentDefaults();
            this.QueryConfiguration();
         */
    }

    /// <summary>   Sets the subsystem known preset state. </summary>
    /// <remarks>   2024-11-15. </remarks>
    public override void PresetKnownState()
    {
        base.PresetKnownState();
        this.ReadInstrumentDefaults();
        this.ApplyInstrumentDefaults();
        this.QueryConfiguration();
    }

    #endregion

    #region " device under test element: Estimator "

    /// <summary> Gets the <see cref="ColdResistance">cold meterMain</see>. </summary>
    /// <value> The cold meterMain. </value>
    public Estimator Estimator { get; set; } = new Estimator();

    /// <summary> Gets the <see cref="DeviceUnderTestElementBase">of the estimator</see>. </summary>
    /// <value> The device under test element base of the estimator. </value>
    protected override DeviceUnderTestElementBase DeviceUnderTestElement => this.Estimator;

    #endregion

    #region " thermal coefficient "

    /// <summary> Gets or sets the cached Thermal Coefficient. </summary>
    /// <value> The Thermal Coefficient. </value>
    public double? ThermalCoefficient
    {
        get;
        protected set
        {
            value ??= Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficient;
            this.Estimator.ThermalCoefficient = value.Value;
            _ = this.SetProperty( ref field, value );
        }
    }

    /// <summary> Writes and reads back the Thermal Coefficient. </summary>
    /// <remarks>
    /// This command set the immediate output Thermal Coefficient. The value is in Volts. The
    /// immediate ThermalCoefficient is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Thermal Coefficient. </param>
    /// <returns> The Thermal Coefficient. </returns>
    public double? ApplyThermalCoefficient( double value )
    {
        _ = this.WriteThermalCoefficient( value );
        return this.QueryThermalCoefficient();
    }

    /// <summary> Queries the Thermal Coefficient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The ThermalCoefficient or none if unknown. </returns>
    public double? QueryThermalCoefficient()
    {
        const decimal printFormat = 9.6m;
        this.ThermalCoefficient = this.Session.QueryPrint( this.ThermalCoefficient.GetValueOrDefault( 0d ), printFormat, "{0}.thermalCoefficient", this.EntityName );
        return this.ThermalCoefficient;
    }

    /// <summary>
    /// Writes the ThermalCoefficient without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Thermal Coefficient. The value is in Volts. The
    /// immediate ThermalCoefficient is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Thermal Coefficient. </param>
    /// <returns> The Thermal Coefficient. </returns>
    public double? WriteThermalCoefficient( double value )
    {
        if ( !ThermalTransient.ValidateThermalCoefficient( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( "{0}:thermalCoefficientSetter({1})", this.EntityName, value );
        this.ThermalCoefficient = value;
        return this.ThermalCoefficient;
    }

    #endregion

    #region " configure "

    /// <summary> Applies the instrument defaults. </summary>
    /// <remarks> This is required until the reset command gets implemented. </remarks>
    public override void ApplyInstrumentDefaults()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Applying {0} defaults.", this.EntityName );
        _ = this.Session.WriteLine( "{0}:thermalCoefficientSetter({1}.thermalCoefficient)", this.EntityName, this.DefaultsName );
        _ = this.Session.QueryOperationCompleted();
    }

    /// <summary> Reads instrument defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ReadInstrumentDefaults()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Reading {0} defaults.", this.EntityName );
        bool localTryQueryPrint() { double result = Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficientDefault; bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.thermalCoefficient", this.DefaultsName ); Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficientDefault = result; return ret; }

        if ( !localTryQueryPrint() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default thermal transient estimator thermal coefficient;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );
    }

    /// <summary> Queries the configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void QueryConfiguration()
    {
        this.Session.SetLastAction( $"reading {this.EntityName} configuration." );
        _ = this.QueryThermalCoefficient();
        this.Session.ThrowDeviceExceptionIfError();
    }

    #endregion

    #region " read "

    /// <summary>
    /// Reads the thermal transient. Sets the last <see cref="MeasureSubsystemBase.FirmwareReading">reading</see>,
    /// <see cref="MeasureSubsystemBase.FirmwareOutcomeReading">outcome</see> <see cref="MeasureSubsystemBase.FirmwareStatusReading">status</see> and
    /// <see cref="MeasureSubsystemBase.MeasurementAvailable">Measurement available sentinel</see>.
    /// The outcome is left empty if measurements were not made.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    public void ReadThermalTransient()
    {
        if ( this.QueryOutcomeNil() )
        {
            this.LastReading = string.Empty;
            this.LastOutcome = string.Empty;
            this.LastMeasurementStatus = string.Empty;
            throw new InvalidOperationException( "Measurement not made." );
        }
        else if ( this.QueryOkay() )
        {
            this.Session.SetLastAction( "reading voltage change" );
            this.LastReading = this.Session.QueryPrintStringFormatTrimEnd( 9.6m, "{0}.voltageChange", this.EntityName );
            this.Session.ThrowDeviceExceptionIfError();
            this.LastOutcome = "0";
            this.LastMeasurementStatus = string.Empty;
        }
        else
        {
            // if outcome failed, read and parse the outcome and status.
            this.Session.SetLastAction( "reading outcome" );
            this.LastOutcome = this.Session.QueryPrintStringFormatTrimEnd( 1, "{0}.outcome", this.EntityName );
            this.Session.ThrowDeviceExceptionIfError();
            this.Session.SetLastAction( "reading status" );
            this.LastMeasurementStatus = this.Session.QueryPrintStringFormatTrimEnd( 1, "{0}.status", this.EntityName );
            this.Session.ThrowDeviceExceptionIfError();
            this.LastReading = string.Empty;
        }
    }

    #endregion
}
