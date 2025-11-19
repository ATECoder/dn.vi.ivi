// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;
using cc.isr.Std.NumericExtensions;
using cc.isr.Std.StackTraceExtensions;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;
using cc.isr.VI.Tsp.ParseStringExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm;
/// <summary>
/// Defines the contract that must be implemented by a Thermal Transient Meter Measurement Subsystems.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-12-12, 3.0.5093. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SubsystemBase" /> class.
/// </remarks>
/// <remarks> David, 2020-10-12. </remarks>
/// <param name="statusSubsystem"> The status subsystem. </param>
public abstract class MeasureSubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary> Sets the known clear state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineClearExecutionState()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing {this.EntityName};. " );
        base.DefineClearExecutionState();
        this.FirmwareReading = string.Empty;
        this.FirmwareOutcomeReading = string.Empty;
        this.FirmwareStatusReading = string.Empty;
        this.MeasurementEventCondition = 0;
        this.MeasurementAvailable = false;
        // this is not required -- is done internally
        // this.Session.WriteLine("{0}:clear()", this.EntityName)
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> Sets the known reset (default) state. </summary>
    /// <remarks>
    /// Reads instrument defaults, applies the values to the instrument and reads them back as
    /// configuration values. This ensures that the instrument state is mirrored in code.
    /// </remarks>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resetting {this.EntityName};. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        /* Moved to preset:
            this.ReadInstrumentDefaults();
            this.ApplyInstrumentDefaults();
            this.QueryConfiguration();
         */
    }

    /// <summary>   Sets the subsystem known preset state. Read and apply the instrument defaults and query the configurations. </summary>
    /// <remarks>   2024-11-15.  </remarks>
    public override void PresetKnownState()
    {
        base.PresetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this.ReadInstrumentDefaults();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this.ApplyInstrumentDefaults();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        // this causes a problem requiring the make an initial resistance measurement before
        // the thermal transient.
        // this.Session.WriteLine("{0}:init()", this.EntityName)

        this.QueryConfiguration();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    #endregion

    #region " entity "

    /// <summary> The meter entity. </summary>

    /// <summary> Gets or sets the meter entity. </summary>
    /// <sourceMeasureUnitName> The meter entity. </sourceMeasureUnitName>
    public ThermalTransientMeterEntity MeterEntity
    {
        get;
        set
        {
            if ( !value.Equals( this.MeterEntity ) )
            {
                field = value;
                this.NotifyPropertyChanged();

                this.EntityName = Syntax.ThermalTransient.SelectEntityName( value );
                this.BaseEntityName = Syntax.ThermalTransient.SelectBaseEntityName( value );
                this.DefaultsName = Syntax.ThermalTransient.SelectEntityDefaultsName( value );

            }
        }
    }

    /// <summary> Gets or sets the Defaults name. </summary>
    /// <sourceMeasureUnitName> The Source Measure Unit. </sourceMeasureUnitName>
    public string DefaultsName
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    } = string.Empty;

    /// <summary> Gets or sets the entity name. </summary>
    /// <sourceMeasureUnitName> The Source Measure Unit. </sourceMeasureUnitName>
    public string EntityName
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    } = Syntax.ThermalTransient.InitialEntityName;

    /// <summary> Gets or sets the Base Entity name. </summary>
    /// <sourceMeasureUnitName> The Meter entity. </sourceMeasureUnitName>
    public string? BaseEntityName
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #region " device under test element: Primary Measure Base "

    /// <summary> Gets the <see cref="MeasureBase">primary measurement element</see>. </summary>
    /// <sourceMeasureUnitName> The primary measurement. </sourceMeasureUnitName>
    protected abstract MeasureBase PrimaryMeasurement { get; }

    #endregion

    #region " source measure unit "

    /// <summary> Queries if a given source measure unit exists. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sourceMeasureUnitName"> Name of the source measure unit, e.g., 'smua' or 'smub'. </param>
    /// <returns> <c>true</c> if the source measure unit exists; otherwise <c>false</c> </returns>
    public bool SourceMeasureUnitExists( string sourceMeasureUnitName )
    {
        this.Session.MakeTrueFalseReplyIfEmpty( string.Equals( sourceMeasureUnitName, Syntax.ThermalTransient.DefaultSourceMeterName, StringComparison.OrdinalIgnoreCase ) );
        return !this.Session.IsNil( $"{cc.isr.VI.Syntax.Tsp.Constants.LocalNode}.{sourceMeasureUnitName}" );
    }

    private string _sourceMeasureUnit = Syntax.ThermalTransient.DefaultSourceMeterName;

    /// <summary> Gets or sets the cached Source Measure Unit. </summary>
    /// <sourceMeasureUnitName> The Source Measure Unit. </sourceMeasureUnitName>
    public virtual string SourceMeasureUnit
    {
        get => this._sourceMeasureUnit;
        protected set => _ = this.SetProperty( ref this._sourceMeasureUnit, value );
    }

    /// <summary>
    /// Queries the Source Measure Unit. Also sets the <see cref="SourceMeasureUnit">Source Measure
    /// Unit</see>.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <returns> The Source Measure Unit. </returns>
    public virtual string QuerySourceMeasureUnit()
    {
        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( this.MeterEntity ) )
            this.SourceMeasureUnit = Syntax.ThermalTransient.QuerySourceMeasureUnit( this.Session, this.MeterEntity );

        return this.SourceMeasureUnit;
    }

    /// <summary>   Programs the Source Measure Unit. Does not read back from the instrument. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="sourceMeasureUnitName">    the Source Measure Unit name, e.g., 'smua' or 'smub'. </param>
    /// <returns>   The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    public virtual string WriteSourceMeasureUnit( string sourceMeasureUnitName )
    {
        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( this.MeterEntity ) )
            sourceMeasureUnitName = Syntax.ThermalTransient.WriteSourceMeasureUnit( this.Session, this.MeterEntity, sourceMeasureUnitName );

        this.SourceMeasureUnit = sourceMeasureUnitName;
        return this.SourceMeasureUnit;
    }

    /// <summary>   Writes and reads back the Source Measure Unit. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="sourceMeasureUnitName">    the name of the Source Measure Unit, e.g., 'smua' or
    ///                                         'smub'. </param>
    /// <returns>   The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    public virtual string ApplySourceMeasureUnit( string sourceMeasureUnitName )
    {
        _ = this.WriteSourceMeasureUnit( sourceMeasureUnitName );
        return this.QuerySourceMeasureUnit();
    }

    #endregion

    #region " aperture "

    /// <summary> The Aperture. </summary>
    private double? _aperture;

    /// <summary> Gets or sets the cached Source Aperture. </summary>
    /// <sourceMeasureUnitName> The Source Aperture. </sourceMeasureUnitName>
    public virtual double? Aperture
    {
        get => this._aperture;

        protected set
        {
            this.PrimaryMeasurement.Aperture = value ?? 0d;
            if ( this.Aperture.Differs( value, 0.000001d ) )
            {
                this._aperture = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the source Aperture. </summary>
    /// <remarks>
    /// This command set the immediate output Aperture. The sourceMeasureUnitName is in Amperes. The immediate
    /// Aperture is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <param name="value"> The Aperture. </param>
    /// <returns> The Source Aperture. </returns>
    public double? ApplyAperture( double value )
    {
        _ = this.WriteAperture( value );
        return this.QueryAperture();
    }

    /// <summary> Queries the Aperture. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Aperture or none if unknown. </returns>
    public double? QueryAperture()
    {
        // TODO: Use FirmwareOutcomeReading and nullable string extensions for reading and parsing instead of the formatted queries.
        const decimal printFormat = 7.4m;
        this.Aperture = this.Session.QueryPrint( this.Aperture.GetValueOrDefault( 1d / 60d ), printFormat, "{0}.aperture", this.EntityName );
        return this.Aperture;
    }

    /// <summary>
    /// Writes the source Aperture without reading back the sourceMeasureUnitName from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Aperture. The sourceMeasureUnitName is in Amperes. The
    /// immediate Aperture is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">  Thrown when one or more arguments are outside
    ///                                                 the required range. </exception>
    /// <param name="value">    The Aperture. </param>
    /// <returns>   The Source Aperture. </returns>
    public virtual double? WriteAperture( double value )
    {
        if ( this.MeterEntity == ThermalTransientMeterEntity.Transient )
        {
            if ( !ThermalTransient.ValidateAperture( value, out string details ) )
                throw new ArgumentOutOfRangeException( nameof( value ), details );
        }
        else if ( !ColdResistance.ValidateAperture( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( "{0}:apertureSetter({1})", this.EntityName, value );
        this.Aperture = value;
        return this.Aperture;
    }

    #endregion

    #region " high limit "

    /// <summary> The High Limit. </summary>
    private double? _highLimit;

    /// <summary> Gets or sets the cached High Limit. </summary>
    /// <sourceMeasureUnitName> The High Limit. </sourceMeasureUnitName>
    public virtual double? HighLimit
    {
        get => this._highLimit;

        protected set
        {
            this.PrimaryMeasurement.HighLimit = value ?? 0d;
            if ( this.HighLimit.Differs( value, 0.000001d ) )
            {
                this._highLimit = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the High Limit. </summary>
    /// <remarks>
    /// This command set the immediate output High Limit. The sourceMeasureUnitName is in Ohms. The immediate High
    /// Limit is the output High setting. At *RST, the High values = 0.
    /// </remarks>
    /// <param name="value"> The High Limit. </param>
    /// <returns> The High Limit. </returns>
    public double? ApplyHighLimit( double value )
    {
        _ = this.WriteHighLimit( value );
        return this.QueryHighLimit();
    }

    /// <summary> Queries the High Limit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The High Limit or none if unknown. </returns>
    public double? QueryHighLimit()
    {
        const decimal printFormat = 9.6m;
        this.HighLimit = this.Session.QueryPrint( this.HighLimit.GetValueOrDefault( 0.1d ), printFormat, "{0}.highLimit", this.EntityName );
        return this.HighLimit;
    }

    /// <summary> Writes the High Limit without reading back the sourceMeasureUnitName from the device. </summary>
    /// <remarks>
    /// This command sets the immediate output High Limit. The sourceMeasureUnitName is in Ohms. The immediate
    /// HighLimit is the output High setting. At *RST, the High values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The High Limit. </param>
    /// <returns> The High Limit. </returns>
    public double? WriteHighLimit( double value )
    {
        if ( this.MeterEntity == ThermalTransientMeterEntity.Transient )
        {
            if ( !ThermalTransient.ValidateLimit( value, out string details ) )
                throw new ArgumentOutOfRangeException( nameof( value ), details );
        }
        else if ( !ColdResistance.ValidateLimit( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( "{0}:highLimitSetter({1})", this.EntityName, value );
        this.HighLimit = value;
        return this.HighLimit;
    }

    #endregion

    #region " low limit "

    /// <summary> The Low Limit. </summary>
    private double? _lowLimit;

    /// <summary> Gets or sets the cached Low Limit. </summary>
    /// <sourceMeasureUnitName> The Low Limit. </sourceMeasureUnitName>
    public virtual double? LowLimit
    {
        get => this._lowLimit;

        protected set
        {
            this.PrimaryMeasurement.LowLimit = value ?? 0d;
            if ( this.LowLimit.Differs( value, 0.000001d ) )
            {
                this._lowLimit = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Low Limit. </summary>
    /// <remarks>
    /// This command set the immediate output Low Limit. The sourceMeasureUnitName is in Ohms. The immediate Low
    /// Limit is the output Low setting. At *RST, the Low values = 0.
    /// </remarks>
    /// <param name="value"> The Low Limit. </param>
    /// <returns> The Low Limit. </returns>
    public double? ApplyLowLimit( double value )
    {
        _ = this.WriteLowLimit( value );
        return this.QueryLowLimit();
    }

    /// <summary> Queries the Low Limit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Low Limit or none if unknown. </returns>
    public double? QueryLowLimit()
    {
        const decimal printFormat = 9.6m;
        this.LowLimit = this.Session.QueryPrint( this.LowLimit.GetValueOrDefault( 0.01d ), printFormat, "{0}.lowLimit", this.EntityName );
        return this.LowLimit;
    }

    /// <summary> Writes the Low Limit without reading back the sourceMeasureUnitName from the device. </summary>
    /// <remarks>
    /// This command sets the immediate output Low Limit. The sourceMeasureUnitName is in Ohms. The immediate
    /// LowLimit is the output Low setting. At *RST, the Low values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Low Limit. </param>
    /// <returns> The Low Limit. </returns>
    public double? WriteLowLimit( double value )
    {
        if ( this.MeterEntity == ThermalTransientMeterEntity.Transient )
        {
            if ( !ThermalTransient.ValidateLimit( value, out string details ) )
                throw new ArgumentOutOfRangeException( nameof( value ), details );
        }
        else if ( !ColdResistance.ValidateLimit( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( "{0}:lowLimitSetter({1})", this.EntityName, value );
        this.LowLimit = value;
        return this.LowLimit;
    }

    #endregion

    #region " configure "

    /// <summary> Applies the instrument defaults. </summary>
    /// <remarks> This is required until the reset command gets implemented. </remarks>
    public virtual void ApplyInstrumentDefaults()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Applying {this.EntityName} defaults;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        _ = this.Session.WriteLine( $"{this.EntityName}:currentSourceChannelSetter({this.DefaultsName}.smuI)" );
        _ = this.Session.QueryOperationCompleted();
        _ = this.Session.WriteLine( $"{this.EntityName}:apertureSetter({this.DefaultsName}.aperture)" );
        _ = this.Session.QueryOperationCompleted();
        _ = this.Session.WriteLine( $"{this.EntityName}:levelSetter({this.DefaultsName}.level)" );
        _ = this.Session.QueryOperationCompleted();
        _ = this.Session.WriteLine( $"{this.EntityName}:limitSetter({this.DefaultsName}.limit)" );
        _ = this.Session.QueryOperationCompleted();
        _ = this.Session.WriteLine( $"{this.EntityName}:lowLimitSetter({this.DefaultsName}.lowLimit)" );
        _ = this.Session.QueryOperationCompleted();
        _ = this.Session.WriteLine( $"{this.EntityName}:highLimitSetter({this.DefaultsName}.highLimit)" );
        _ = this.Session.QueryOperationCompleted();
    }

    /// <summary> Reads instrument defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void ReadInstrumentDefaults()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Reading {this.EntityName} defaults;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( this.MeterEntity ) )
        {
            Properties.Settings.Instance.TtmMeterSettings.SourceMeasureUnitDefault = this.Session.QueryTrimEnd( $"_G.print({this.DefaultsName}.smuI)" );
            if ( string.IsNullOrWhiteSpace( Properties.Settings.Instance.TtmMeterSettings.SourceMeasureUnitDefault ) )
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default source measure unit name;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );
        }

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> Applies changed meter configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The resistance. </param>
    public virtual void ConfigureChanged( MeasureBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        if ( this.PrimaryMeasurement is null ) throw new InvalidOperationException( $"Meter {nameof( this.PrimaryMeasurement )} is null." );
        if ( !resistance.ConfigurationEquals( this.PrimaryMeasurement ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Configuring {this.EntityName} resistance measurement;. " );
            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
            if ( !string.Equals( this.PrimaryMeasurement.SourceMeasureUnit, resistance.SourceMeasureUnit, StringComparison.Ordinal ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} source measure unit to {resistance.SourceMeasureUnit};. " );
                _ = this.ApplySourceMeasureUnit( resistance.SourceMeasureUnit );
            }

            if ( !this.PrimaryMeasurement.Aperture.Equals( resistance.Aperture ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} aperture to {resistance.Aperture};. " );
                _ = this.ApplyAperture( resistance.Aperture );
            }

            if ( !this.PrimaryMeasurement.HighLimit.Equals( resistance.HighLimit ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} high limit to {resistance.HighLimit};. " );
                _ = this.ApplyHighLimit( resistance.HighLimit );
            }

            if ( !this.PrimaryMeasurement.LowLimit.Equals( resistance.LowLimit ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Low limit to {resistance.LowLimit};. " );
                _ = this.ApplyLowLimit( resistance.LowLimit );
            }

            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    /// <summary> Configures the meter for making the resistance measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The resistance. </param>
    public virtual void Configure( MeasureBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Configuring {this.EntityName} resistance measurement;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} source measure unit to {resistance.SourceMeasureUnit};. " );
        _ = this.ApplySourceMeasureUnit( resistance.SourceMeasureUnit );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} aperture to {resistance.Aperture};. " );
        _ = this.ApplyAperture( resistance.Aperture );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} high limit to {resistance.HighLimit};. " );
        _ = this.ApplyHighLimit( resistance.HighLimit );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Low limit to {resistance.LowLimit};. " );
        _ = this.ApplyLowLimit( resistance.LowLimit );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> Queries the configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void QueryConfiguration()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Reading {0} meter configuration;. ", this.EntityName );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        _ = this.QuerySourceMeasureUnit();
        _ = this.QueryAperture();
        _ = this.QueryHighLimit();
        _ = this.QueryLowLimit();
    }

    #endregion

    #region " measure "

    /// <summary> Measures the Final resistance and returns. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0010:Add missing cases", Justification = "<Pending>" )]
    public void Measure()
    {
        this.DefineClearExecutionState();
        this.Session.EnableServiceRequestOnOperationCompletion();
        switch ( this.MeterEntity )
        {
            case ThermalTransientMeterEntity.FinalResistance:
                {
                    _ = this.Session.WriteLine( "_G.ttm.measureFinalResistance() waitcomplete()" );
                    break;
                }

            case ThermalTransientMeterEntity.InitialResistance:
                {
                    _ = this.Session.WriteLine( "_G.ttm.measureInitialResistance() waitcomplete()" );
                    break;
                }

            case ThermalTransientMeterEntity.Transient:
                {
                    _ = this.Session.WriteLine( "_G.ttm.measureThermalTransient() waitcomplete()" );
                    break;
                }

            default:
                {
                    break;
                }
        }

        switch ( this.MeterEntity )
        {
            case ThermalTransientMeterEntity.FinalResistance:
            case ThermalTransientMeterEntity.InitialResistance:
            case ThermalTransientMeterEntity.Transient:
                {
                    this.Session.SetLastAction( "measuring Final MeasuredValue" );
                    _ = this.Session.QueryOperationCompleted();
                    this.Session.ThrowDeviceExceptionIfError();
                    break;
                }

            default:
                {
                    break;
                }
        }
    }

    /// <summary> Measures the Thermal Transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The Thermal Transient element. </param>
    public void Measure( MeasureBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        resistance.DefineClearExecutionState();
        this.Session.SetLastAction( $"measuring Thermal Transient" );
        this.Measure();
        this.Session.ThrowDeviceExceptionIfError();
    }

    #endregion

    #region " firmware readings "

    /// <summary>   Queries the meter okay sentinel. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <returns>   The firmware okay. </returns>
    public bool? QueryFirmwareOkay()
    {
        this.Session.MakeEmulatedReplyIfEmpty( cc.isr.VI.Syntax.Tsp.Lua.NilValue );
        this.FirmwareOkayReading = this.Session.QueryPrintTrimEnd( $"{this.EntityName}:isOkay() " ) ?? string.Empty;
        if ( this.FirmwareOkayReading.TryParseNullableBool( out bool? result ) )
            return result;
        else
            throw new InvalidOperationException( $"The {nameof( MeasureSubsystemBase.FirmwareOkayReading )} '{this.FirmwareOkayReading}' failed to parse to a true/false sourceMeasureUnitName." );
        // return this.Session.IsTrue( "{0}:isOkay()", this.EntityName );
    }

    /// <summary>   Queries the meter outcome. If null, measurement was not made. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <returns>   The firmware outcome. </returns>
    public int? QueryFirmwareOutcome()
    {
        this.Session.MakeEmulatedReplyIfEmpty( cc.isr.VI.Syntax.Tsp.Lua.NilValue );
        this.FirmwareOutcomeReading = this.Session.QueryPrintTrimEnd( $"{this.EntityName}.outcome" ) ?? string.Empty;
        if ( this.FirmwareOutcomeReading.TryParseNullableInteger( out int? result ) )
            return result;
        else
            throw new InvalidOperationException( $"The {nameof( MeasureSubsystemBase.FirmwareOutcomeReading )} '{this.FirmwareOutcomeReading}' failed to parse to an integer sourceMeasureUnitName." );
        // return this.Session.IsTrue( "{0}.outcome==nil", this.EntityName );
    }

    /// <summary> Queries the meter outcome status. If nil, measurement was not made. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> <c>true</c> if entity outcome is nil; otherwise, <c>false</c>. </returns>
    public int? QueryFirmwareStatus()
    {
        this.Session.MakeEmulatedReplyIfEmpty( cc.isr.VI.Syntax.Tsp.Lua.NilValue );
        this.FirmwareStatusReading = this.Session.QueryPrintTrimEnd( $"{this.EntityName}.status" ) ?? string.Empty;
        if ( this.FirmwareStatusReading.TryParseNullableInteger( out int? result ) )
            return result;
        else
            throw new InvalidOperationException( $"The {nameof( MeasureSubsystemBase.FirmwareStatusReading )} '{this.FirmwareStatusReading}' failed to parse to an integer sourceMeasureUnitName." );
    }

    /// <summary>   Queries the resistance. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <returns>   The resistance. </returns>
    public double? QueryResistance()
    {
        this.Session.MakeEmulatedReplyIfEmpty( cc.isr.VI.Syntax.Tsp.Lua.NilValue );
        this.FirmwareReading = this.Session.QueryPrintTrimEnd( $"{this.EntityName}.resistance" ) ?? string.Empty;
        if ( this.FirmwareReading.TryParseNullableDouble( out double? result ) )
            return result;
        else
            throw new InvalidOperationException( $"The {nameof( MeasureSubsystemBase.FirmwareReading )} '{this.FirmwareReading}' failed to parse to a double sourceMeasureUnitName." );
    }

    /// <summary>   Queries voltage change. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <returns>   The voltage change. </returns>
    public double? QueryVoltageChange()
    {
        this.Session.MakeEmulatedReplyIfEmpty( cc.isr.VI.Syntax.Tsp.Lua.NilValue );
        this.FirmwareReading = this.Session.QueryPrintTrimEnd( $"{this.EntityName}.voltageChange" ) ?? string.Empty;
        if ( this.FirmwareReading.TryParseNullableDouble( out double? result ) )
            return result;
        else
            throw new InvalidOperationException( $"The {nameof( MeasureSubsystemBase.FirmwareReading )} '{this.FirmwareReading}' failed to parse to a double sourceMeasureUnitName." );
    }

    #endregion

    #region " read "

    /// <summary> Gets or sets (protected) the firmware reading. </summary>
    /// <sourceMeasureUnitName> The last reading. </sourceMeasureUnitName>
    public string? FirmwareReading
    {
        get;

        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets (protected) the firmware outcome reading. </summary>
    /// <sourceMeasureUnitName> The outcome reading. </sourceMeasureUnitName>
    public string? FirmwareOutcomeReading
    {
        get;

        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets (protected) the firmware okay reading. </summary>
    /// <sourceMeasureUnitName> The firmware okay Reading. </sourceMeasureUnitName>
    public string? FirmwareOkayReading
    {
        get;

        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets (protected) the firmware status reading. </summary>
    /// <sourceMeasureUnitName> The firmware status reading. </sourceMeasureUnitName>
    public string? FirmwareStatusReading
    {
        get;

        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets (protected) the measurement available. </summary>
    /// <sourceMeasureUnitName> The measurement available. </sourceMeasureUnitName>
    public bool MeasurementAvailable
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Gets or sets the cached Condition of the measurement register events. </summary>
    /// <sourceMeasureUnitName> <c>null</c> if sourceMeasureUnitName is not known;. </sourceMeasureUnitName>
    public int MeasurementEventCondition
    {
        get;

        protected set
        {
            if ( !this.MeasurementEventCondition.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Reads the condition of the measurement register event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> System.Nullable{System.Int32}. </returns>
    public bool TryQueryMeasurementEventCondition()
    {
        int result = this.MeasurementEventCondition;
        return this.Session.TryQueryPrint( 1, ref result, "_G.status.measurement.condition" );
        // this.MeasurementEventCondition = result;
    }

    /// <summary> Parses the outcome. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="details"> [out] The details. </param>
    /// <returns> The parsed outcome. </returns>
    public MeasurementOutcomes ParseOutcome( ref string details )
    {
        MeasurementOutcomes outcome = MeasurementOutcomes.None;
        System.Text.StringBuilder detailsBuilder = new();
        int measurementCondition = default;
        if ( string.IsNullOrWhiteSpace( this.FirmwareOutcomeReading ) )
        {
            outcome |= MeasurementOutcomes.MeasurementNotMade;
            _ = detailsBuilder.AppendFormat( "Failed parsing outcome -- outcome is nothing indicating measurement not made;. " );
        }
        else if ( int.TryParse( this.FirmwareOutcomeReading, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out int numericValue ) )
        {
            if ( numericValue < 0 )
            {
                if ( detailsBuilder.Length > 0 )
                    _ = detailsBuilder.AppendLine();
                _ = detailsBuilder.AppendFormat( "Failed parsing outcome '{0}';. ", this.FirmwareOutcomeReading );
                outcome = MeasurementOutcomes.MeasurementFailed;
                outcome |= MeasurementOutcomes.UnexpectedOutcomeFormat;
            }
            // interpret the outcome.
            else if ( numericValue != 0 )
            {
                outcome = MeasurementOutcomes.MeasurementFailed;
                if ( 0 != (numericValue & ( int ) FirmwareOutcomes.BadStatus) )
                {
                    // this is a bad status - could be compliance or other things.
                    if ( !string.IsNullOrWhiteSpace( this.FirmwareStatusReading ) )
                    {
                        if ( detailsBuilder.Length > 0 )
                            _ = detailsBuilder.AppendLine();
                        _ = detailsBuilder.AppendFormat( "MeasurementOutcome status={0};. ", this.FirmwareStatusReading );
                    }

                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    if ( this.TryQueryMeasurementEventCondition() )
                    {
                        measurementCondition = this.MeasurementEventCondition;
                        _ = detailsBuilder.AppendFormat( "Measurement condition=0x{0:X4};. ", measurementCondition );
                    }
                    else
                    {
                        measurementCondition = 0xFFFF;
                        _ = detailsBuilder.AppendFormat( "Failed getting measurement condition. Set to 0x{0:X4};. ", measurementCondition );
                    }

                    outcome |= MeasurementOutcomes.HitCompliance;
                }

                if ( (numericValue & ( int ) FirmwareOutcomes.BadTimeStamps) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Sampling returned bad time stamps;. Device reported outcome=0x{0:X4} and status measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }

                if ( (numericValue & ( int ) FirmwareOutcomes.ConfigFailed) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Configuration failed;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }

                if ( (numericValue & ( int ) FirmwareOutcomes.InitiationFailed) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Initiation failed;. Device reported outcome=0x{0:X4} and status measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }

                if ( (numericValue & ( int ) FirmwareOutcomes.LoadFailed) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Load failed;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }

                if ( (numericValue & ( int ) FirmwareOutcomes.MeasurementFailed) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Measurement failed;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.MeasurementFailed;
                }

                if ( (numericValue & ( int ) FirmwareOutcomes.OpenLeads) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Open Leads;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.OpenLeads;
                }

                if ( (numericValue & ((2 * ( int ) FirmwareOutcomes.OpenLeads) - 1)) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Unknown firmware outcome;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }
            }
        }
        else
        {
            if ( detailsBuilder.Length > 0 )
                _ = detailsBuilder.AppendLine();
            _ = detailsBuilder.AppendFormat( "Failed parsing outcome '{0}';. {1}{2}.", this.FirmwareOutcomeReading, Environment.NewLine, new StackFrame( true ).UserCallStack() );
            outcome = MeasurementOutcomes.MeasurementFailed | MeasurementOutcomes.UnexpectedReadingFormat;
        }

        details = detailsBuilder.ToString();
        return outcome;
    }

    #endregion
}
