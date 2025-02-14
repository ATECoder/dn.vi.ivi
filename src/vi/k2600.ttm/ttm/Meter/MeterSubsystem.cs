namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> The Meter Subsystem. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-23 </para>
/// </remarks>
public class MeterSubsystem : MeterSubsystemBase
{
    #region " construction and cloning "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="statusSubsystem">  The status subsystem. </param>
    /// <param name="resistance">       The cold meterMain. </param>
    /// <param name="meterEntity">      The meter entity type. </param>
    public MeterSubsystem( StatusSubsystemBase statusSubsystem, MeterMain resistance, ThermalTransientMeterEntity meterEntity ) : base( statusSubsystem )
    {
        this.MeterEntity = meterEntity;
        this.MeterMain = resistance;
    }

    #endregion

    #region " i presettable "

    /// <summary> Defines the parameters for the Clears known state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineClearExecutionState()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Clearing {0} meterMain execution state;. ", this.EntityName );
        base.DefineClearExecutionState();
        // this.ColdResistance.DefineClearExecutionState();
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
        // B = this.Session.IsTrue("{0}.level==nil", this.EntityName)
        // string val = this.Session.QueryPrintStringFormat("%9.6f", "_G.ttm.coldResistance.Defaults.level")
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Resetting {0} meterMain known state;. ", this.EntityName );
        base.DefineKnownResetState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this.MeterMain.ResetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    /// <summary>   Sets the subsystem known preset state. </summary>
    /// <remarks>   2024-11-15. </remarks>
    public override void PresetKnownState()
    {
        base.PresetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        this.MeterMain.PresetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }
    #endregion

    #region " device under test element: MeterMain "

    /// <summary> Gets the <see cref="ColdResistance">cold meterMain</see>. </summary>
    /// <value> The cold meterMain. </value>
    public MeterMain MeterMain { get; set; }

    /// <summary> Gets the <see cref="DeviceUnderTestElementBase">of the meter element</see>. </summary>
    /// <value> The device under test element base of the Meter element. </value>
    protected override DeviceUnderTestElementBase DeviceUnderTestElement => this.MeterMain;

    /// <summary>   Parses the firmware version returns true if this code is addressing the legacy firmware version 2.3.x. </summary>
    /// <value> True if legacy firmware, false if not. </value>
    public static bool LegacyFirmware => (2 == FirmwareVersion.Major) && (3 == FirmwareVersion.Minor);

    /// <summary>   Gets or sets the firmware version. </summary>
    /// <value> The firmware version. </value>
    public static Version FirmwareVersion { get; set; } = new Version( 0, 0, 0 );

    #endregion

    #region " configure "

    /// <summary> Reads instrument defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ReadInstrumentDefaults()
    {
        // this reads smuI: base.ReadInstrumentDefaults();

        bool localTryQueryPrintLegacyDriver()
        {
            int result = Properties.Settings.Instance.TtmMeterSettings.LegacyDriverDefault;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.legacyDriver", this.DefaultsName );
            Properties.Settings.Instance.TtmMeterSettings.LegacyDriverDefault = result;
            return ret;
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintLegacyDriver()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default meter legacy driver;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );


        bool localTryQueryPrintContactLimit()
        {
            int result = Properties.Settings.Instance.TtmMeterSettings.ContactCheckThresholdDefault;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.contactLimit", this.DefaultsName );
            Properties.Settings.Instance.TtmMeterSettings.ContactCheckThresholdDefault = result;
            return ret;
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintContactLimit()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default contact check threshold;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );


        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        bool localTryQueryPrintSmuName()
        {
            string reply = this.Session.QueryTrimEnd( $"_G.print({this.DefaultsName}.smuName)" );
            if ( string.IsNullOrWhiteSpace( reply ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default source measure unit name;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );
                return false;
            }
            else
            {
                Properties.Settings.Instance.TtmMeterSettings.SourceMeasureUnitDefault = reply;
                return true;
            }
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintSmuName()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default source measure unit name;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        bool localTryQueryPrintContactCheckOptions()
        {
            int result = Properties.Settings.Instance.TtmMeterSettings.ContactCheckThresholdDefault;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.contactLimit", this.DefaultsName );
            Properties.Settings.Instance.TtmMeterSettings.ContactCheckThresholdDefault = result;
            return ret;
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintContactCheckOptions()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default contact check options;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        bool localTryQueryPrintPostTransientDelay()
        {
            double result = Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayDefault;
            bool ret = this.Session.TryQueryPrint( 7.4m, ref result, "{0}.postTransientDelay", this.DefaultsName );
            Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayDefault = result;
            return ret;
        }

        // the legacy firmware post transient delay default is a local member.
        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintPostTransientDelay()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default cold meterMain aperture;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

    }

    /// <summary>   Configures the meter for its meter main element. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="meterMain"> The meter main element. </param>
    public void Configure( MeterMain meterMain )
    {
        if ( meterMain is null ) throw new ArgumentNullException( nameof( meterMain ) );
        base.Configure( meterMain );

        string details;
        if ( !MeterSubsystem.LegacyFirmware )
        {
            details = "legacy driver";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.LegacyDriver};. " );
            _ = this.ApplyLegacyDriver( meterMain.LegacyDriver );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.LegacyDriver};. " );

            details = "contact check options";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.ContactCheckOptions};. " );
            _ = this.ApplyContactCheckOptions( meterMain.ContactCheckOptions );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.ContactCheckOptions};. " );

            details = "contact check threshold";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.ContactLimit};. " );
            _ = this.ApplyContactLimit( meterMain.ContactLimit );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.ContactLimit};. " );

            details = "source measure unit";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.SourceMeasureUnit};. " );
            _ = this.ApplySourceMeasureUnit( meterMain.SourceMeasureUnit );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.SourceMeasureUnit};. " );
        }

        details = "post transient delay";
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.PostTransientDelay};. " );
        _ = this.ApplyPostTransientDelay( meterMain.PostTransientDelay );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.PostTransientDelay};. " );

        this.Session.ThrowDeviceExceptionIfError();
    }

    /// <summary> Applies changed meter configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="meterMain"> The meter main element. </param>
    public void ConfigureChanged( MeterMain meterMain )
    {
        if ( meterMain is null ) throw new ArgumentNullException( nameof( meterMain ) );
        base.Configure( meterMain );

        if ( !this.MeterMain.PostTransientDelay.Equals( meterMain.PostTransientDelay ) )
        {
            string details;
            if ( !MeterSubsystem.LegacyFirmware )
            {
                details = "legacy driver";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.LegacyDriver};. " );
                _ = this.ApplyLegacyDriver( meterMain.LegacyDriver );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.LegacyDriver};. " );

                details = "contact check options";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.ContactCheckOptions};. " );
                _ = this.ApplyContactCheckOptions( meterMain.ContactCheckOptions );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.ContactCheckOptions};. " );

                details = "contact check threshold";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.ContactLimit};. " );
                _ = this.ApplyContactLimit( meterMain.ContactLimit );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.ContactLimit};. " );

                details = "source measure unit";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.SourceMeasureUnit};. " );
                _ = this.ApplySourceMeasureUnit( meterMain.SourceMeasureUnit );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.SourceMeasureUnit};. " );
            }

            details = "post transient delay";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.PostTransientDelay};. " );
            _ = this.ApplyPostTransientDelay( meterMain.PostTransientDelay );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.PostTransientDelay};. " );
        }

        this.Session.ThrowDeviceExceptionIfError();
    }

    /// <summary> Queries the configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void QueryConfiguration()
    {
        base.QueryConfiguration();
        _ = this.QueryLegacyDriver();
        _ = this.QuerySourceMeasureUnit();
        _ = this.QueryContactCheckOptions();
        _ = this.QueryContactLimit();
        _ = this.QueryPostTransientDelay();
        this.Session.ThrowDeviceExceptionIfError();
    }

    #endregion

    #region " legacy driver "

    private int? _legacyDriver;

    /// <summary> Gets or sets the cached legacy driver flag. </summary>
    public int? LegacyDriver
    {
        get => this._legacyDriver;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmMeterSettings.LegacyDriver;
            this.MeterMain.LegacyDriver = value.Value;
            _ = this.SetProperty( ref this._legacyDriver, value );
        }
    }

    /// <summary> Writes and reads back the legacy driver flag. </summary>
    /// <param name="value"> The legacy driver. </param>
    /// <returns> The legacy driver flag. </returns>
    public int? ApplyLegacyDriver( int value )
    {
        _ = this.WriteLegacyDriver( value );
        return this.QueryLegacyDriver();
    }

    /// <summary> Queries the legacy driver. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The legacy driver or none if unknown. </returns>
    public int? QueryLegacyDriver()
    {
        const int printFormat = 1;
        if ( MeterSubsystem.LegacyFirmware )
            this.LegacyDriver = 1;
        else
            this.LegacyDriver = this.Session.QueryPrint( this.LegacyDriver.GetValueOrDefault( 1 ), printFormat, $"{this.EntityName}.legacyDriverGetter()" );
        return this.LegacyDriver;
    }

    /// <summary>
    /// Writes the legacy driver flag without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The legacy driver. </param>
    /// <returns> The legacy driver flag. </returns>
    public int? WriteLegacyDriver( int value )
    {
        if ( !MeterMain.ValidateLegacyDriver( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );
        if ( !MeterSubsystem.LegacyFirmware )
            _ = this.Session.WriteLine( $"{this.BaseEntityName}.legacyDriverSetter({value})" );
        this.LegacyDriver = value;
        return this.LegacyDriver;
    }

    #endregion

    #region " Contact Limit "

    private int? _contactLimit;

    /// <summary> Gets or sets the cached Contact Limit flag. </summary>
    public int? ContactLimit
    {
        get => this._contactLimit;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmMeterSettings.ContactCheckThreshold;
            this.MeterMain.ContactLimit = value.Value;
            _ = this.SetProperty( ref this._contactLimit, value );
        }
    }

    /// <summary> Writes and reads back the Contact Limit flag. </summary>
    /// <param name="value"> The Contact Limit. </param>
    /// <returns> The Contact Limit flag. </returns>
    public int? ApplyContactLimit( int value )
    {
        _ = this.WriteContactLimit( value );
        return this.QueryContactLimit();
    }

    /// <summary> Queries the Contact Limit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Contact Limit or none if unknown. </returns>
    public int? QueryContactLimit()
    {
        const int printFormat = 1;
        if ( MeterSubsystem.LegacyFirmware )
            this.ContactLimit = 10;
        else
            this.ContactLimit = this.Session.QueryPrint( this.ContactLimit.GetValueOrDefault( 1 ), printFormat, $"{this.EntityName}.contactLimitGetter()" );
        return this.ContactLimit;
    }

    /// <summary>
    /// Writes the Contact Limit flag without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Contact Limit. </param>
    /// <returns> The Contact Limit flag. </returns>
    public int? WriteContactLimit( int value )
    {
        if ( !MeterMain.ValidateContactThreshold( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );
        if ( !MeterSubsystem.LegacyFirmware )
            _ = this.Session.WriteLine( $"{this.BaseEntityName}.contactLimitSetter({value})" );
        this.ContactLimit = value;
        return this.ContactLimit;
    }

    #endregion

    #region " Contact Check Option "

    private Syntax.ContactCheckOptions? _contactCheckOptions;

    /// <summary> Gets or sets the cached Contact Check Option flag. </summary>
    public Syntax.ContactCheckOptions? ContactCheckOptions
    {
        get => this._contactCheckOptions;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmMeterSettings.ContactCheckOptions;
            this.MeterMain.ContactCheckOptions = value.Value;
            _ = this.SetProperty( ref this._contactCheckOptions, value );
        }
    }

    /// <summary> Writes and reads back the Contact Check Option flag. </summary>
    /// <param name="value"> The Contact Check Option. </param>
    /// <returns> The Contact Check Option flag. </returns>
    public Syntax.ContactCheckOptions? ApplyContactCheckOptions( Syntax.ContactCheckOptions value )
    {
        _ = this.WriteContactCheckOptions( value );
        return this.QueryContactCheckOptions();
    }

    /// <summary> Queries the Contact Check Option. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Contact Check Option or none if unknown. </returns>
    public Syntax.ContactCheckOptions? QueryContactCheckOptions()
    {
        const int printFormat = 1;
        if ( MeterSubsystem.LegacyFirmware )
            this.ContactCheckOptions = Syntax.ContactCheckOptions.Initial;
        else
            this.ContactCheckOptions = ( Syntax.ContactCheckOptions ) this.Session.QueryPrint( ( int ) this.ContactCheckOptions.GetValueOrDefault( Syntax.ContactCheckOptions.Initial ),
                printFormat, $"{this.EntityName}.contactCheckOptionsGetter()" );
        return this.ContactCheckOptions;
    }

    /// <summary>
    /// Writes the Contact Check Option flag without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Contact Check Option. </param>
    /// <returns> The Contact Check Option flag. </returns>
    public Syntax.ContactCheckOptions? WriteContactCheckOptions( Syntax.ContactCheckOptions value )
    {
        if ( !MeterMain.ValidateContactCheckOptions( ( int ) value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        if ( !MeterSubsystem.LegacyFirmware )
            _ = this.Session.WriteLine( $"{this.BaseEntityName}.contactCheckOptionsSetter({( int ) value})" );
        this.ContactCheckOptions = value;
        return this.ContactCheckOptions;
    }

    #endregion

    #region " post transient delay "

    private double? _postTransientDelay;

    /// <summary> Gets or sets the cached Source Post Transient Delay. </summary>
    /// <value>
    /// The Source Post Transient Delay.
    /// </value>
    public double? PostTransientDelay
    {
        get => this._postTransientDelay;

        protected set
        {
            value ??= Properties.Settings.Instance.TtmMeterSettings.PostTransientDelay;
            this.MeterMain.PostTransientDelay = value.Value;
            _ = this.SetProperty( ref this._postTransientDelay, value );
        }
    }

    /// <summary> Writes and reads back the source Post Transient Delay. </summary>
    /// <param name="value"> The Post Transient Delay. </param>
    /// <returns> The Post Transient Delay. </returns>
    public double? ApplyPostTransientDelay( double value )
    {
        _ = this.WritePostTransientDelay( value );
        return this.QueryPostTransientDelay();
    }

    /// <summary> Queries the Post Transient Delay. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Post Transient Delay or none if unknown. </returns>
    public double? QueryPostTransientDelay()
    {
        const decimal printFormat = 9.3m;
        this.PostTransientDelay = this.Session.QueryPrint( this.PostTransientDelay.GetValueOrDefault( 0.5d ), printFormat,
            $"{this.BaseEntityName}.postTransientDelayGetter()" );
        return this.PostTransientDelay;
    }

    /// <summary>
    /// Writes the source Post Transient Delay without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Post Transient Delay. </param>
    /// <returns> The Source Post Transient Delay. </returns>
    public double? WritePostTransientDelay( double value )
    {
        if ( !MeterMain.ValidatePostTransientDelay( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );

        _ = this.Session.WriteLine( $"{this.BaseEntityName}.postTransientDelaySetter({value})" );
        this.PostTransientDelay = value;
        return this.PostTransientDelay;
    }

    #endregion

    #region " source measure unit "

    /// <summary>
    /// Queries the Source Measure Unit. Also sets the <see cref="SourceMeasureUnit">Source Measure
    /// Unit</see>.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <returns> The Source Measure Unit. </returns>
    public override string QuerySourceMeasureUnit()
    {
        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( this.MeterEntity ) )
            this.SourceMeasureUnit = Syntax.ThermalTransient.QuerySourceMeasureUnit( this.Session, this.MeterEntity );

        return this.SourceMeasureUnit;
    }

    /// <summary> Programs the Source Measure Unit. Does not read back from the instrument. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    ///                                      illegal values. </exception>
    /// <param name="value"> the Source Measure Unit, e.g., 'smua' or 'smub'. </param>
    /// <returns> The Source Measure Unit, e.g., 'smua' or 'smub'. </returns>
    public override string WriteSourceMeasureUnit( string value )
    {
        if ( Syntax.ThermalTransient.RequiresSourceMeasureUnit( this.MeterEntity ) )
            value = Syntax.ThermalTransient.WriteSourceMeasureUnit( this.Session, this.MeterEntity, value );

        this.SourceMeasureUnit = value;
        return this.SourceMeasureUnit;
    }

    #endregion

}
