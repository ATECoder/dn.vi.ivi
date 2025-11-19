// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> The Cold Resistance Measure Subsystem. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-23 </para>
/// </remarks>
public class ColdResistanceMeasure : MeasureSubsystemBase
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="statusSubsystem"> The status subsystem. </param>
    /// <param name="resistance">      The cold resistance. </param>
    /// <param name="meterEntity">     The meter entity type. </param>
    public ColdResistanceMeasure( StatusSubsystemBase statusSubsystem, ColdResistance resistance, ThermalTransientMeterEntity meterEntity ) : base( statusSubsystem )
    {
        this.MeterEntity = meterEntity;
        this.ColdResistance = resistance;
    }

    #endregion

    #region " i presettable "

    /// <summary> Defines the parameters for the Clears known state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineClearExecutionState()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing {0} resistance execution state;. ", this.EntityName );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        base.DefineClearExecutionState();
        this.ColdResistance.DefineClearExecutionState();
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineKnownResetState()
    {
        // this.Session.WriteLine("{0}.level = nil", this.EntityName)
        // bool b = this.Session.IsTrue("{0}.level==nil", this.EntityName)
        // this.Session.WriteLine("{0}.level = 0.09", this.EntityName)
        // b = this.Session.IsTrue("{0}.level==nil", this.EntityName)
        // string val = this.Session.QueryPrintStringFormat("%9.6f", "_G.ttm.coldResistance.Defaults.level")

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resetting {0} resistance known state;. ", this.EntityName );
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        base.DefineKnownResetState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this.ColdResistance.ResetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary>   Sets the subsystem known preset state. Read and apply the instrument defaults and query the configurations. </summary>
    /// <remarks>   2024-11-15.  </remarks>
    public override void PresetKnownState()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Presetting {0} resistance known state;. ", this.EntityName );

        base.PresetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this.ColdResistance.PresetKnownState();

        // query the actual source output.
        // the unit tests must be aware of the actual source output for validations.
        _ = this.QuerySourceOutputOption();
    }

    #endregion

    #region " device under test element: cold resistance "

    /// <summary> Gets the <see cref="ColdResistance">cold resistance</see>. </summary>
    /// <value> The cold resistance. </value>
    public ColdResistance ColdResistance { get; set; }

    /// <summary> Gets the <see cref="MeasureBase">primary measurement base of the cold resistance.</see>. </summary>
    /// <value> The primary measurement base of the cold resistance. </value>
    protected override MeasureBase PrimaryMeasurement => this.ColdResistance;

    #endregion

    #region " current level "

    /// <summary>   (Immutable) the default current level. </summary>
    public const double DefaultCurrentLevel = 0.01d;

    private double? _currentLevel;

    /// <summary> Gets or sets the cached Source Current Level. </summary>
    /// <value> The Source Current Level. </value>
    public virtual double? CurrentLevel
    {
        get => this._currentLevel;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmResistanceSettings.CurrentLevel;
            this.ColdResistance.CurrentLevel = value.Value;
            _ = this.SetProperty( ref this._currentLevel, value );
        }
    }

    /// <summary> Writes and reads back the source Current Level. </summary>
    /// <remarks>
    /// This command set the immediate output Current Level. The value is in Amperes. The immediate
    /// Current Level is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <param name="value"> The Current Level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? ApplyCurrentLevel( double value )
    {
        _ = this.WriteCurrentLevel( value );
        return this.QueryCurrentLevel();
    }

    /// <summary> Queries the Current Level. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Current Level or none if unknown. </returns>
    public double? QueryCurrentLevel()
    {
        if ( this.SourceOutputOption is null ) throw new InvalidOperationException( "Source output option must not be null." );
        if ( this.SourceOutputOption != Syntax.SourceOutputOption.Current ) throw new InvalidOperationException( $"Invalid source output option of '{this.SourceOutputOption}'. Source output must be {Syntax.SourceOutputOption.Current} for getting the current level." );
        const decimal printFormat = 9.6m;
        this.CurrentLevel = MeterSubsystem.LegacyFirmware
            ? this.Session.QueryPrint( this.CurrentLevel.GetValueOrDefault( ColdResistanceMeasure.DefaultCurrentLevel ), printFormat, "{0}.level", this.EntityName )
            : this.Session.QueryPrint( this.CurrentLevel.GetValueOrDefault( ColdResistanceMeasure.DefaultCurrentLevel ), printFormat, "{0}.levelI", this.EntityName );
        return this.CurrentLevel;
    }

    /// <summary>
    /// Writes the source Current Level without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Current Level. The value is in Amperes. The immediate
    /// CurrentLevel is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Current Level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? WriteCurrentLevel( double value )
    {
        if ( this.SourceOutputOption is null ) throw new InvalidOperationException( "Source output option must not be null." );
        if ( this.SourceOutputOption != Syntax.SourceOutputOption.Current ) throw new InvalidOperationException( $"Invalid source output option of '{this.SourceOutputOption}'. Source output must be {Syntax.SourceOutputOption.Current} for writing the current level." );
        _ = ColdResistance.ValidateCurrentRange( value, out string details )
            ? this.Session.WriteLine( "{0}:levelSetter({1})", this.EntityName, value )
            : throw new ArgumentOutOfRangeException( nameof( value ), details );
        this.CurrentLevel = value;
        return this.CurrentLevel;
    }

    #endregion

    #region " current limit "

    /// <summary>   (Immutable) the default current limit. </summary>
    public const double DefaultCurrentLimit = 0.01d;

    private double? _currentLimit;

    /// <summary> Gets or sets the cached Source Current Limit. </summary>
    /// <value> The Source Current Limit. </value>
    public virtual double? CurrentLimit
    {
        get => this._currentLimit;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmResistanceSettings.CurrentLimit;
            this.ColdResistance.CurrentLimit = value.Value;
            _ = this.SetProperty( ref this._currentLimit, value );
        }
    }

    /// <summary> Writes and reads back the source Current Limit. </summary>
    /// <remarks>
    /// This command set the immediate output Current Limit. The value is in Amperes. The immediate
    /// Current Limit is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <param name="value"> The Current Limit. </param>
    /// <returns> The Source Current Limit. </returns>
    public double? ApplyCurrentLimit( double value )
    {
        _ = this.WriteCurrentLimit( value );
        return this.QueryCurrentLimit();
    }

    /// <summary> Queries the Current Limit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Current Limit or none if unknown. </returns>
    public double? QueryCurrentLimit()
    {
        if ( MeterSubsystem.LegacyFirmware ) throw new InvalidOperationException( "Invalid query of current limit. The legacy firmware does not support using a voltage source." );
        if ( this.SourceOutputOption is null ) throw new InvalidOperationException( "Source output option must not be null." );
        if ( this.SourceOutputOption != Syntax.SourceOutputOption.Voltage ) throw new InvalidOperationException( $"Invalid source output option of '{this.SourceOutputOption}'. Source output must be {Syntax.SourceOutputOption.Voltage} for querying the current limit." );
        const decimal printFormat = 9.6m;
        this.CurrentLimit = this.Session.QueryPrint( this.CurrentLimit.GetValueOrDefault( ColdResistanceMeasure.DefaultCurrentLimit ), printFormat, "{0}.limitI", this.EntityName );
        return this.CurrentLimit;
    }

    /// <summary>
    /// Writes the source Current Limit without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Current Limit. The value is in Amperes. The immediate
    /// CurrentLimit is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Current Limit. </param>
    /// <returns> The Source Current Limit. </returns>
    public double? WriteCurrentLimit( double value )
    {
        if ( MeterSubsystem.LegacyFirmware ) throw new InvalidOperationException( "Invalid wiring of current limit. The legacy firmware does not support using a voltage source." );
        if ( this.SourceOutputOption is null ) throw new InvalidOperationException( "Source output option must not be null." );
        if ( this.SourceOutputOption != Syntax.SourceOutputOption.Voltage ) throw new InvalidOperationException( $"Invalid source output option of '{this.SourceOutputOption}'. Source output must be {Syntax.SourceOutputOption.Voltage} when setting a current limit." );
        _ = ColdResistance.ValidateCurrentRange( value, out string details )
            ? this.Session.WriteLine( "{0}:limitSetter({1})", this.EntityName, value )
            : throw new ArgumentOutOfRangeException( nameof( value ), details );

        this.CurrentLimit = value;
        return this.CurrentLimit;
    }

    #endregion

    #region " voltage level "

    /// <summary>   (Immutable) the default voltage Level. </summary>
    public const double DefaultVoltageLevel = 0.1d;

    private double? _voltageLevel;

    /// <summary> Gets or sets the cached Source Voltage Level. </summary>
    /// <value> The Source Voltage Level. </value>
    public virtual double? VoltageLevel
    {
        get => this._voltageLevel;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmResistanceSettings.VoltageLevel;
            this.ColdResistance.VoltageLevel = value.Value;
            _ = this.SetProperty( ref this._voltageLevel, value );
        }
    }

    /// <summary> Writes and reads back the source Voltage Level. </summary>
    /// <remarks>
    /// This command set the immediate output Voltage Level. The value is in Volts. The immediate
    /// Voltage Level is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Voltage Level. </param>
    /// <returns> The Source Voltage Level. </returns>
    public double? ApplyVoltageLevel( double value )
    {
        _ = this.WriteVoltageLevel( value );
        return this.QueryVoltageLevel();
    }

    /// <summary> Queries the Voltage Level. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Voltage Level or none if unknown. </returns>
    public double? QueryVoltageLevel()
    {
        if ( MeterSubsystem.LegacyFirmware ) throw new InvalidOperationException( "Invalid query of voltage level. The legacy firmware does not support using a voltage source." );
        if ( this.SourceOutputOption is null ) throw new InvalidOperationException( "Source output option must not be null." );
        if ( this.SourceOutputOption != Syntax.SourceOutputOption.Voltage ) throw new InvalidOperationException( $"Invalid source output option of '{this.SourceOutputOption}'. Source output must be {Syntax.SourceOutputOption.Voltage} for querying the voltage level." );
        const decimal printFormat = 9.6m;
        this.VoltageLevel = this.Session.QueryPrint( this.VoltageLevel.GetValueOrDefault( ColdResistanceMeasure.DefaultVoltageLevel ), printFormat, "{0}.levelV", this.EntityName );

        return this.VoltageLevel;
    }

    /// <summary>
    /// Writes the source Voltage Level without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Voltage Level. The value is in Volts. The immediate
    /// VoltageLevel is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Voltage Level. </param>
    /// <returns> The Source Voltage Level. </returns>
    public double? WriteVoltageLevel( double value )
    {
        if ( MeterSubsystem.LegacyFirmware ) throw new InvalidOperationException( "Invalid writing of voltage level. The legacy firmware does not support using a voltage source." );
        if ( this.SourceOutputOption is null ) throw new InvalidOperationException( "Source output option must not be null." );
        if ( this.SourceOutputOption != Syntax.SourceOutputOption.Voltage ) throw new InvalidOperationException( $"Invalid source output option of '{this.SourceOutputOption}'. Source output must be {Syntax.SourceOutputOption.Voltage} for setting the voltage level." );
        _ = ColdResistance.ValidateVoltageRange( value, out string details )
            ? this.Session.WriteLine( "{0}:levelSetter({1})", this.EntityName, value )
            : throw new ArgumentOutOfRangeException( nameof( value ), details );
        this.VoltageLevel = value;
        return this.VoltageLevel;
    }

    #endregion

    #region " voltage limit "

    /// <summary>   (Immutable) the default voltage limit. </summary>
    public const double DefaultVoltageLimit = 0.1d;

    private double? _voltageLimit;

    /// <summary> Gets or sets the cached Source Voltage Limit. </summary>
    /// <value> The Source Voltage Limit. </value>
    public virtual double? VoltageLimit
    {
        get => this._voltageLimit;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmResistanceSettings.VoltageLimit;
            this.ColdResistance.VoltageLimit = value.Value;
            _ = this.SetProperty( ref this._voltageLimit, value );
        }
    }

    /// <summary> Writes and reads back the source Voltage Limit. </summary>
    /// <remarks>
    /// This command set the immediate output Voltage Limit. The value is in Volts. The immediate
    /// Voltage Limit is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Voltage Limit. </param>
    /// <returns> The Source Voltage Limit. </returns>
    public double? ApplyVoltageLimit( double value )
    {
        _ = this.WriteVoltageLimit( value );
        return this.QueryVoltageLimit();
    }

    /// <summary> Queries the Voltage Limit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Voltage Limit or none if unknown. </returns>
    public double? QueryVoltageLimit()
    {
        if ( this.SourceOutputOption is null ) throw new InvalidOperationException( "Source output option must not be null." );
        if ( this.SourceOutputOption != Syntax.SourceOutputOption.Current ) throw new InvalidOperationException( $"Invalid source output option of '{this.SourceOutputOption}'. Source output must be {Syntax.SourceOutputOption.Current} when querying the voltage limit." );
        const decimal printFormat = 9.6m;
        this.VoltageLimit = MeterSubsystem.LegacyFirmware
            ? this.Session.QueryPrint( this.VoltageLimit.GetValueOrDefault( ColdResistanceMeasure.DefaultVoltageLimit ), printFormat, "{0}.limit", this.EntityName )
            : this.Session.QueryPrint( this.VoltageLimit.GetValueOrDefault( ColdResistanceMeasure.DefaultVoltageLimit ), printFormat, "{0}.limitV", this.EntityName );
        return this.VoltageLimit;
    }

    /// <summary>
    /// Writes the source Voltage Limit without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Voltage Limit. The value is in Volts. The immediate
    /// VoltageLimit is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Voltage Limit. </param>
    /// <returns> The Source Voltage Limit. </returns>
    public double? WriteVoltageLimit( double value )
    {
        if ( this.SourceOutputOption is null ) throw new InvalidOperationException( "Source output option must not be null." );
        if ( this.SourceOutputOption != Syntax.SourceOutputOption.Current ) throw new InvalidOperationException( $"Invalid source output option of '{this.SourceOutputOption}'. Source output must be {Syntax.SourceOutputOption.Current} when setting the voltage limit." );
        _ = ColdResistance.ValidateVoltageRange( value, out string details )
            ? this.Session.WriteLine( "{0}:limitSetter({1})", this.EntityName, value )
            : throw new ArgumentOutOfRangeException( nameof( value ), details );
        this.VoltageLimit = value;
        return this.VoltageLimit;
    }

    #endregion

    #region " fail status "

    private int? _failStatus;

    /// <summary> Gets or sets the cached fail status. </summary>
    /// <value> The Source Voltage Limit. </value>
    public virtual int? FailStatus
    {
        get => this._failStatus;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmResistanceSettings.FailStatus;
            this.ColdResistance.FailStatus = value.Value;
            _ = this.SetProperty( ref this._failStatus, value );
        }
    }

    /// <summary> Writes and reads back the fail status. </summary>
    /// <param name="value"> The Voltage Limit. </param>
    /// <returns> The Source Voltage Limit. </returns>
    public int? ApplyFailStatus( int value )
    {
        _ = this.WriteFailStatus( value );
        return this.QueryFailStatus();
    }

    /// <summary> Queries the Voltage Limit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Voltage Limit or none if unknown. </returns>
    public int? QueryFailStatus()
    {
        const int printFormat = 1;
        this.FailStatus = MeterSubsystem.LegacyFirmware
            ? 66
            : this.Session.QueryPrint( this.FailStatus.GetValueOrDefault( 2 ), printFormat, "{0}.failStatus", this.EntityName );
        return this.FailStatus;
    }

    /// <summary>
    /// Writes the source Voltage Limit without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output Voltage Limit. The value is in Volts. The immediate
    /// FailStatus is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Voltage Limit. </param>
    /// <returns> The Source Voltage Limit. </returns>
    public int? WriteFailStatus( int value )
    {
        _ = MeterSubsystem.LegacyFirmware
                ? value.ToString()
                : this.Session.WriteLine( "{0}:failStatusSetter({1})", this.EntityName, value );
        this.FailStatus = value;
        return this.FailStatus;
    }

    #endregion

    #region " source output option "

    private Syntax.SourceOutputOption? _sourceOutputOption;

    /// <summary> Gets or sets the cached Source SourceOutputOption. </summary>
    /// <value> The Source SourceOutputOption. </value>
    public virtual Syntax.SourceOutputOption? SourceOutputOption
    {
        get => this._sourceOutputOption;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmResistanceSettings.SourceOutput;
            this.ColdResistance.SourceOutputOption = value.Value;
            _ = this.SetProperty( ref this._sourceOutputOption, value );
        }
    }

    /// <summary> Writes and reads back the source SourceOutputOption. </summary>
    /// <remarks>
    /// This command set the immediate output SourceOutputOption. The value is in Volts. The immediate
    /// SourceOutputOption is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The SourceOutputOption. </param>
    /// <returns> The Source SourceOutputOption. </returns>
    public Syntax.SourceOutputOption? ApplySourceOutputOption( Syntax.SourceOutputOption value )
    {
        _ = this.WriteSourceOutputOption( value );
        return this.QuerySourceOutputOption();
    }

    /// <summary> Queries the SourceOutputOption. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The SourceOutputOption or none if unknown. </returns>
    public Syntax.SourceOutputOption? QuerySourceOutputOption()
    {
        if ( MeterSubsystem.LegacyFirmware )
            this.SourceOutputOption = Syntax.SourceOutputOption.Current;
        else
        {
            this.Session.SetLastAction( "querying source output" );
            string? reply = this.Session.QueryPrintTrimEnd( "{0}.sourceOutput", this.EntityName );
            if ( reply is null || string.IsNullOrEmpty( reply ) ) throw new InvalidOperationException( "Output source query returned an empty or null string." );
            if ( Enum.TryParse( reply, out Syntax.SourceOutputOption result ) )
                this.SourceOutputOption = result;
            else
                throw new InvalidOperationException( $"failed parsing {reply} to a {nameof( Syntax.SourceOutputOption )} Enum value." );
        }
        return this.SourceOutputOption;
    }

    /// <summary>
    /// Writes the source SourceOutputOption without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output SourceOutputOption. The value is in Volts. The immediate
    /// SourceOutputOption is the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The SourceOutputOption. </param>
    /// <returns> The Source SourceOutputOption. </returns>
    public Syntax.SourceOutputOption? WriteSourceOutputOption( Syntax.SourceOutputOption value )
    {
        if ( MeterSubsystem.LegacyFirmware )
            this.SourceOutputOption = Syntax.SourceOutputOption.Current;
        else
            _ = this.Session.WriteLine( "{0}:sourceOutputSetter('{1}')", this.EntityName, value );
        this.SourceOutputOption = value;
        return this.SourceOutputOption;
    }

    #endregion

    #region " configure "

    /// <summary> Reads instrument defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ReadInstrumentDefaults()
    {
        base.ReadInstrumentDefaults();

        string fieldCaption = "cold resistance current level";
        bool localTryQuerySourceOutput()
        {
            bool ret;
            Syntax.SourceOutputOption result = Properties.Settings.Instance.TtmResistanceSettings.SourceOutputDefault;
            if ( MeterSubsystem.LegacyFirmware )
            {
                result = Syntax.SourceOutputOption.Current;
                ret = true;
            }
            else
            {
                string? reply = this.Session.QueryPrintTrimEnd( "{0}.sourceOutput", this.EntityName );
                ret = reply is not null && !string.IsNullOrEmpty( reply ) && Enum.TryParse( reply, out result );
            }
            Properties.Settings.Instance.TtmResistanceSettings.SourceOutputDefault = result; return ret;
        }

        if ( !localTryQuerySourceOutput() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        Syntax.SourceOutputOption sourceOutput = Properties.Settings.Instance.TtmResistanceSettings.SourceOutputDefault;

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance aperture";
        bool localTryQueryAperture()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.ApertureDefault;
            bool ret = this.Session.TryQueryPrint( 7.4m, ref result, "{0}.aperture", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.ApertureDefault = result;
            return ret;
        }

        if ( !localTryQueryAperture() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance current level";
        bool localTryQueryCurrentLevel()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.CurrentLevelDefault;
            bool ret = MeterSubsystem.LegacyFirmware
                ? this.Session.TryQueryPrint( 9.6m, ref result, "{0}.level", this.DefaultsName )
                : this.Session.TryQueryPrint( 9.6m, ref result, "{0}.levelI", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.CurrentLevelDefault = result; return ret;
        }

        if ( (sourceOutput == Syntax.SourceOutputOption.Current) && !localTryQueryCurrentLevel() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance current limit";
        bool localTryQueryCurrentLimit()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.CurrentLimitDefault;
            bool ret = MeterSubsystem.LegacyFirmware || this.Session.TryQueryPrint( 9.6m, ref result, "{0}.limitI", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.CurrentLimitDefault = result; return ret;
        }

        if ( !MeterSubsystem.LegacyFirmware && (sourceOutput == Syntax.SourceOutputOption.Voltage) && !localTryQueryCurrentLimit() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance minimum current";
        bool localTryQueryMinCurrent()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.CurrentMinimum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.minCurrent", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.CurrentMinimum = result;
            return ret;
        }

        if ( !localTryQueryMinCurrent() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance maximum current";
        bool localTryQueryMaxCurrent()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.CurrentMaximum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.maxCurrent", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.CurrentMaximum = result;
            return ret;
        }

        if ( !localTryQueryMaxCurrent() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance voltage level";
        bool localTryQueryVoltageLevel()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.VoltageLevelDefault;
            bool ret = MeterSubsystem.LegacyFirmware || this.Session.TryQueryPrint( 9.6m, ref result, "{0}.levelV", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.VoltageLevelDefault = result;
            return ret;
        }

        if ( !MeterSubsystem.LegacyFirmware && (sourceOutput == Syntax.SourceOutputOption.Voltage) && !localTryQueryVoltageLevel() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance voltage limit";
        bool localTryQueryVoltageLimit()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.VoltageLimitDefault;
            bool ret = MeterSubsystem.LegacyFirmware
                ? this.Session.TryQueryPrint( 9.6m, ref result, "{0}.limit", this.DefaultsName )
                : this.Session.TryQueryPrint( 9.6m, ref result, "{0}.limitV", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.VoltageLimitDefault = result;
            return ret;
        }

        if ( (sourceOutput == Syntax.SourceOutputOption.Current) && !localTryQueryVoltageLimit() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance minimum voltage";
        bool localTryQueryMinVoltage()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.VoltageMinimum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.minVoltage", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.VoltageMinimum = result;
            return ret;
        }

        if ( !localTryQueryMinVoltage() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance maximum voltage";
        bool maxVoltage()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.VoltageMaximum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.maxVoltage", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.VoltageMaximum = result;
            return ret;
        }

        if ( !maxVoltage() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance low limit";
        bool localTryQueryLowLimit()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.LowLimitDefault;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.lowLimit", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.LowLimitDefault = result;
            return ret;
        }

        if ( !localTryQueryLowLimit() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance high limit";
        bool localTryQueryHighLimit()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.HighLimitDefault;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.highLimit", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.HighLimitDefault = result;
            return ret;
        }

        if ( !localTryQueryHighLimit() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance minimum resistance";
        bool localTryQueryMinResistance()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.Minimum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.minResistance", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.Minimum = result;
            return ret;
        }

        if ( !localTryQueryMinResistance() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        fieldCaption = "cold resistance maximum resistance";
        bool localTryQueryMaxResistance()
        {
            double result = Properties.Settings.Instance.TtmResistanceSettings.Maximum;
            bool ret = this.Session.TryQueryPrint( 9.6m, ref result, "{0}.maxResistance", this.DefaultsName );
            Properties.Settings.Instance.TtmResistanceSettings.Maximum = result;
            return ret;
        }

        if ( !localTryQueryMaxResistance() )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default {fieldCaption};. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary> Configures the meter for making the cold resistance measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The cold resistance. </param>
    public void Configure( ColdResistanceBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        base.Configure( resistance );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Source Output to {resistance.SourceOutputOption};. " );
        _ = this.ApplySourceOutputOption( resistance.SourceOutputOption );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        if ( resistance.SourceOutputOption == Syntax.SourceOutputOption.Current )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Current Level to {resistance.CurrentLevel};. " );
            _ = this.ApplyCurrentLevel( resistance.CurrentLevel );

            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Voltage Limit to {resistance.VoltageLimit};. " );
            _ = this.ApplyVoltageLimit( resistance.VoltageLimit );

            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        }
        else
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Voltage Level to {resistance.VoltageLevel};. " );
            _ = this.ApplyVoltageLevel( resistance.VoltageLevel );

            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Current Limit to {resistance.CurrentLimit};. " );
            _ = this.ApplyCurrentLimit( resistance.CurrentLimit );

            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        }
        this.Session.ThrowDeviceExceptionIfError();
        this.ColdResistance.CheckThrowUnequalConfiguration( resistance );
    }

    /// <summary> Applies changed meter configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The cold resistance. </param>
    public void ConfigureChanged( ColdResistanceBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        this.Session.SetLastAction( "configuring Cold Resistance measurement" );
        base.ConfigureChanged( resistance );
        if ( !this.ColdResistance.ConfigurationEquals( resistance ) )
        {
            if ( !this.SourceOutputOption.Equals( resistance.SourceOutputOption ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Source Output to {resistance.SourceOutputOption};. " );
                _ = this.ApplySourceOutputOption( resistance.SourceOutputOption );
            }

            if ( (resistance.SourceOutputOption == Syntax.SourceOutputOption.Current) && !this.CurrentLevel.Equals( resistance.CurrentLevel ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Current Level to {resistance.CurrentLevel};. " );
                _ = this.ApplyCurrentLevel( resistance.CurrentLevel );
            }

            if ( (resistance.SourceOutputOption == Syntax.SourceOutputOption.Current) && !this.VoltageLimit.Equals( resistance.VoltageLimit ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Voltage Limit to {resistance.VoltageLimit};. " );
                _ = this.ApplyVoltageLimit( resistance.VoltageLimit );
            }

            if ( (resistance.SourceOutputOption == Syntax.SourceOutputOption.Voltage) && !this.VoltageLevel.Equals( resistance.VoltageLevel ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Voltage Level to {resistance.VoltageLevel};. " );
                _ = this.ApplyVoltageLevel( resistance.VoltageLevel );
            }

            if ( (resistance.SourceOutputOption == Syntax.SourceOutputOption.Voltage) && !this.CurrentLimit.Equals( resistance.CurrentLimit ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.EntityName} Current Limit to {resistance.CurrentLimit};. " );
                _ = this.ApplyCurrentLimit( resistance.CurrentLimit );
            }
        }

        this.Session.ThrowDeviceExceptionIfError();
        this.ColdResistance.CheckThrowUnequalConfiguration( resistance );
    }

    /// <summary> Queries the configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void QueryConfiguration()
    {
        base.QueryConfiguration();
        _ = this.QuerySourceOutputOption();
        if ( this.SourceOutputOption == Syntax.SourceOutputOption.Current )
        {
            _ = this.QueryCurrentLevel();
            _ = this.QueryVoltageLimit();
        }
        else
        {
            _ = this.QueryVoltageLevel();
            _ = this.QueryCurrentLimit();
        }
        _ = this.QueryFailStatus();
        this.Session.ThrowDeviceExceptionIfError();
    }

    #endregion

    #region " measure "

    /// <summary> Measures the Final resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The resistance. </param>
    public new void Measure( MeasureBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        this.Session.MakeEmulatedReply( resistance.GenerateRandomReading( this.LowLimit ?? 0, this.HighLimit ?? 0 ) );
        base.Measure( resistance );
        this.ReadResistance( resistance );
        // this.EmulateResistance(resistance)
    }

    #endregion

    #region " read "

    /// <summary>
    /// Reads the resistance. Sets the last <see cref="MeasureSubsystemBase.FirmwareReading">reading</see>,
    /// <see cref="MeasureSubsystemBase.FirmwareOutcomeReading">outcome</see> <see cref="MeasureSubsystemBase.FirmwareStatusReading">status</see> and
    /// <see cref="MeasureSubsystemBase.MeasurementAvailable">Measurement available sentinel</see>.
    /// The outcome is left empty if measurements were not made.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    public void ReadResistance()
    {
        this.FirmwareReading = string.Empty;
        this.FirmwareOutcomeReading = string.Empty;
        if ( this.QueryFirmwareOutcome() is null )
        {
            this.FirmwareReading = cc.isr.VI.Syntax.Tsp.Lua.NilValue;
            this.FirmwareOutcomeReading = cc.isr.VI.Syntax.Tsp.Lua.NilValue;
            this.FirmwareStatusReading = cc.isr.VI.Syntax.Tsp.Lua.NilValue;
            this.FirmwareOkayReading = cc.isr.VI.Syntax.Tsp.Lua.NilValue;
            throw new InvalidOperationException( "Measurement not made." );
        }
        else if ( this.QueryFirmwareOkay().GetValueOrDefault( false ) )
        {
            this.Session.SetLastAction( "reading resistance" );
            _ = this.QueryResistance();
            _ = this.QueryFirmwareStatus();
            // this.FirmwareReading = this.Session.QueryPrintStringFormatTrimEnd( 8.5m, "{0}.resistance", this.EntityName );
            this.Session.ThrowDeviceExceptionIfError();
            // this.FirmwareOutcomeReading = "0";
            // this.FirmwareStatusReading = string.Empty;
        }
        else
        {
            // if outcome failed, read and parse the outcome and status.
            this.Session.SetLastAction( "reading outcome" );
            this.FirmwareOutcomeReading = this.Session.QueryPrintStringFormatTrimEnd( 1, "{0}.outcome", this.EntityName );
            this.Session.ThrowDeviceExceptionIfError();

            this.Session.SetLastAction( "reading status" );
            this.FirmwareStatusReading = this.Session.QueryPrintStringFormatTrimEnd( 1, "{0}.status", this.EntityName );
            this.Session.ThrowDeviceExceptionIfError();
            this.FirmwareReading = string.Empty;
        }

        this.ColdResistance.FirmwareOutcomeReading = this.FirmwareOutcomeReading;
        this.MeasurementAvailable = true;
    }

    /// <summary> Reads cold resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The cold resistance. </param>
    public void ReadResistance( MeasureBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        this.Session.SetLastAction( "reading MeasuredValue" );
        this.ReadResistance();
        this.Session.ThrowDeviceExceptionIfError();
        MeasurementOutcomes measurementOutcome = MeasurementOutcomes.None;
        if ( this.FirmwareOutcomeReading != "0" )
        {
            string details = string.Empty;
            measurementOutcome = this.ParseOutcome( ref details );
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"Measurement failed;. Details: {0}", details );
        }

        if ( this.FirmwareReading is null ) throw new InvalidOperationException( $"{nameof( this.FirmwareReading )} is null." );

        this.ColdResistance.ParseReading( this.FirmwareReading, measurementOutcome );
        resistance.ParseReading( this.FirmwareReading, measurementOutcome );
    }

    /// <summary> Emulates cold resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resistance"> The cold resistance. </param>
    public void EmulateResistance( MeasureBase resistance )
    {
        if ( resistance is null ) throw new ArgumentNullException( nameof( resistance ) );
        this.ColdResistance.EmulateReading();
        this.FirmwareReading = resistance.Reading;
        this.FirmwareOutcomeReading = resistance.FirmwareOutcomeReading;
        this.FirmwareStatusReading = resistance.FirmwareStatusReading;
        this.FirmwareOkayReading = resistance.FirmwareOkayReading;
        MeasurementOutcomes measurementOutcome = MeasurementOutcomes.None;

        if ( this.FirmwareReading is null ) throw new InvalidOperationException( $"{nameof( this.FirmwareReading )} is null." );
        this.ColdResistance.ParseReading( this.FirmwareReading, measurementOutcome );

        resistance.ParseReading( this.FirmwareReading, measurementOutcome );
    }

    #endregion
}
