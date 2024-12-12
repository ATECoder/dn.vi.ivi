using System.Diagnostics;
using cc.isr.Std.StackTraceExtensions;

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
public abstract class MeterSubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary> Sets the known clear state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineClearExecutionState()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing {this.EntityName};. " );
        base.DefineClearExecutionState();
        this.LastReading = string.Empty;
        this.LastOutcome = string.Empty;
        this.LastMeasurementStatus = string.Empty;
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

        /* Moved to preset
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
    private ThermalTransientMeterEntity _meterEntity;

    /// <summary> Gets or sets the meter entity. </summary>
    /// <sourceMeasureUnitName> The meter entity. </sourceMeasureUnitName>
    public ThermalTransientMeterEntity MeterEntity
    {
        get => this._meterEntity;
        set
        {
            if ( !value.Equals( this.MeterEntity ) )
            {
                this._meterEntity = value;
                this.NotifyPropertyChanged();

                this.EntityName = Syntax.ThermalTransient.SelectEntityName( value );
                this.BaseEntityName = Syntax.ThermalTransient.SelectBaseEntityName( value );
                this.DefaultsName = Syntax.ThermalTransient.SelectEntityDefaultsName( value );
            }
        }
    }

    private string _defaultsName = string.Empty;

    /// <summary> Gets or sets the name of the default element of this script. </summary>
    /// <sourceMeasureUnitName> The name of the default name element. </sourceMeasureUnitName>
    public string DefaultsName
    {
        get => this._defaultsName;
        protected set => _ = this.SetProperty( ref this._defaultsName, value );
    }

    private string _entityName = Syntax.ThermalTransient.InitialEntityName;

    /// <summary> Gets or sets the entity name. </summary>
    /// <sourceMeasureUnitName> The entity name. </sourceMeasureUnitName>
    public string EntityName
    {
        get => this._entityName;
        protected set => _ = this.SetProperty( ref this._entityName, value );
    }

    private string? _baseEntityName;

    /// <summary> Gets or sets the Base Entity name. </summary>
    /// <sourceMeasureUnitName> The Meter entity. </sourceMeasureUnitName>
    public string? BaseEntityName
    {
        get => this._baseEntityName;
        protected set => _ = this.SetProperty( ref this._baseEntityName, value );
    }

    #endregion

    #region " device under test element: Device Under Test Element Base "

    /// <summary> Gets the <see cref="DeviceUnderTestElementBase">device under test element</see>. </summary>
    /// <sourceMeasureUnitName> The device under test element. </sourceMeasureUnitName>
    protected abstract DeviceUnderTestElementBase DeviceUnderTestElement { get; }

    #endregion

    #region " source measure unit "

    private string _sourceMeasureUnit = Syntax.ThermalTransient.DefaultSourceMeterName;

    /// <summary> Gets or sets the cached Source Measure Unit. </summary>
    /// <sourceMeasureUnitName> The Source Measure Unit. </sourceMeasureUnitName>
    public virtual string SourceMeasureUnit
    {
        get => this._sourceMeasureUnit;
        protected set
        {
            if ( string.IsNullOrWhiteSpace( value ) ) value = Properties.Settings.Instance.TtmMeterSettings.SourceMeasureUnit;
            this.DeviceUnderTestElement.SourceMeasureUnit = value;
            _ = this.SetProperty( ref this._sourceMeasureUnit, value );
        }
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
    /// <param name="sourceMeasureUnitName">    the name of the Source Measure Unit, e.g., 'smua' or 'smub'. </param>
    /// <returns>   The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    public virtual string WriteSourceMeasureUnit( string sourceMeasureUnitName )
    {
        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( this.MeterEntity ) )
            sourceMeasureUnitName = Syntax.ThermalTransient.WriteSourceMeasureUnit( this.Session, this.MeterEntity, sourceMeasureUnitName );

        this.SourceMeasureUnit = sourceMeasureUnitName;
        return this.SourceMeasureUnit;
    }

    /// <summary> Writes and reads back the Source Measure Unit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sourceMeasureUnitName"> the name of the  Source Measure Unit, e.g., 'smua' or 'smub'. </param>
    /// <returns> The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    public virtual string ApplySourceMeasureUnit( string sourceMeasureUnitName )
    {
        _ = this.WriteSourceMeasureUnit( sourceMeasureUnitName );
        return this.QuerySourceMeasureUnit();
    }

    #endregion

    #region " configure "

    /// <summary> Applies the instrument defaults. </summary>
    /// <remarks> This is required until the reset command gets implemented. </remarks>
    public virtual void ApplyInstrumentDefaults()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Applying {this.EntityName} defaults;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        Syntax.ThermalTransient.WriteSourceMeasureUnitDefaults( this.Session, this.MeterEntity );
    }

    /// <summary> Reads instrument defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void ReadInstrumentDefaults()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Reading {this.EntityName} defaults;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( this.MeterEntity ) )
            Properties.Settings.Instance.TtmMeterSettings.SourceMeasureUnitDefault = Syntax.ThermalTransient.ReadSourceMeasureUnitDefaults( this.Session, this.MeterEntity );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> Applies changed meter configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The resistance. </param>
    public virtual void ConfigureChanged( DeviceUnderTestElementBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        if ( this.DeviceUnderTestElement is null ) throw new InvalidOperationException( $"Meter {nameof( this.DeviceUnderTestElement )} is null." );
        if ( !resistance.ConfigurationEquals( this.DeviceUnderTestElement ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Configuring {this.EntityName} resistance measurement;. " );
            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
            if ( !string.Equals( this.DeviceUnderTestElement.SourceMeasureUnit, resistance.SourceMeasureUnit, StringComparison.Ordinal ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} source measure unit to {resistance.SourceMeasureUnit};. " );
                _ = this.ApplySourceMeasureUnit( resistance.SourceMeasureUnit );
            }

            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    /// <summary> Configures the meter for making the resistance measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The resistance. </param>
    public virtual void Configure( DeviceUnderTestElementBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Configuring {this.EntityName} element;. " );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} source measure unit to {resistance.SourceMeasureUnit};. " );
        _ = this.ApplySourceMeasureUnit( resistance.SourceMeasureUnit );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> Queries the configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void QueryConfiguration()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Reading {0} meter configuration;. ", this.EntityName );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        _ = this.QuerySourceMeasureUnit();
    }

    #endregion

    #region " okay "

    /// <summary> Queries the meter okay sentinel. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> <c>true</c> if entity okay status is True; otherwise, <c>false</c>. </returns>
    public bool QueryOkay()
    {
        this.Session.MakeEmulatedReplyIfEmpty( true );
        return this.Session.IsTrue( "{0}:isOkay()", this.EntityName );
    }

    #endregion

    #region " outcome "

    /// <summary> Queries the meter outcome status. If nil, measurement was not made. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> <c>true</c> if entity outcome is nil; otherwise, <c>false</c>. </returns>
    public bool QueryOutcomeNil()
    {
        this.Session.MakeEmulatedReplyIfEmpty( true );
        return this.Session.IsTrue( "{0}.outcome==nil", this.EntityName );
    }

    #endregion

    #region " read "

    /// <summary> The last reading. </summary>
    private string? _lastReading;

    /// <summary> Gets or sets (protected) the last reading. </summary>
    /// <sourceMeasureUnitName> The last reading. </sourceMeasureUnitName>
    public string? LastReading
    {
        get => this._lastReading;

        protected set
        {
            this._lastReading = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> The last outcome. </summary>
    private string? _lastOutcome;

    /// <summary> Gets or sets (protected) the last outcome. </summary>
    /// <sourceMeasureUnitName> The last outcome. </sourceMeasureUnitName>
    public string? LastOutcome
    {
        get => this._lastOutcome;

        protected set
        {
            this._lastOutcome = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> The last measurement status. </summary>
    private string? _lastMeasurementStatus;

    /// <summary> Gets or sets (protected) the last measurement status. </summary>
    /// <sourceMeasureUnitName> The last measurement status. </sourceMeasureUnitName>
    public string? LastMeasurementStatus
    {
        get => this._lastMeasurementStatus;

        protected set
        {
            this._lastMeasurementStatus = value;
            this.NotifyPropertyChanged();
        }
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
        if ( string.IsNullOrWhiteSpace( this.LastOutcome ) )
        {
            outcome |= MeasurementOutcomes.MeasurementNotMade;
            _ = detailsBuilder.AppendFormat( "Failed parsing outcome -- outcome is nothing indicating measurement not made;. " );
        }
        else if ( int.TryParse( this.LastOutcome, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out int numericValue ) )
        {
            if ( numericValue < 0 )
            {
                if ( detailsBuilder.Length > 0 )
                    _ = detailsBuilder.AppendLine();
                _ = detailsBuilder.AppendFormat( "Failed parsing outcome '{0}';. ", this.LastOutcome );
                outcome = MeasurementOutcomes.MeasurementFailed;
                outcome |= MeasurementOutcomes.UnexpectedOutcomeFormat;
            }
            // interpret the outcome.
            else if ( numericValue != 0 )
            {
                outcome = MeasurementOutcomes.MeasurementFailed;
                if ( (numericValue & 1) == 1 )
                {
                    // this is a bad status - could be compliance or other things.
                    if ( !string.IsNullOrWhiteSpace( this.LastMeasurementStatus ) )
                    {
                        if ( detailsBuilder.Length > 0 )
                            _ = detailsBuilder.AppendLine();
                        _ = detailsBuilder.AppendFormat( "MeasurementOutcome status={0};. ", this.LastMeasurementStatus );
                    }

                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();

                    outcome |= MeasurementOutcomes.HitCompliance;
                }

                if ( (numericValue & 2) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Sampling returned bad time stamps;. Device reported outcome=0x{0:X4} and status measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }

                if ( (numericValue & 4) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Configuration failed;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }

                if ( (numericValue & 8) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Initiation failed;. Device reported outcome=0x{0:X4} and status measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }

                if ( (numericValue & 16) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Load failed;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }

                if ( (numericValue & ~31) != 0 )
                {
                    if ( detailsBuilder.Length > 0 )
                        _ = detailsBuilder.AppendLine();
                    _ = detailsBuilder.AppendFormat( "Unknown failure;. Device reported outcome=0x{0:X4} and measurement condition=0x{1:X4}", numericValue, measurementCondition );
                    outcome |= MeasurementOutcomes.UnspecifiedFirmwareOutcome;
                }
            }
        }
        else
        {
            if ( detailsBuilder.Length > 0 )
                _ = detailsBuilder.AppendLine();
            _ = detailsBuilder.AppendFormat( "Failed parsing outcome '{0}';. {1}{2}.", this.LastOutcome, Environment.NewLine, new StackFrame( true ).UserCallStack() );
            outcome = MeasurementOutcomes.MeasurementFailed | MeasurementOutcomes.UnexpectedReadingFormat;
        }

        details = detailsBuilder.ToString();
        return outcome;
    }

    #endregion
}
