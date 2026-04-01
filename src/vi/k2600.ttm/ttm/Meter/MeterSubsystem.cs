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

    /// <summary> Gets the <see cref="MeterMain">meter main</see>. </summary>
    /// <value> The meter main. </value>
    public MeterMain MeterMain { get; set; }

    /// <summary> Gets the <see cref="DeviceUnderTestElementBase">of the meter element</see>. </summary>
    /// <value> The device under test element base of the Meter element. </value>
    protected override DeviceUnderTestElementBase DeviceUnderTestElement => this.MeterMain;

    /// <summary>   Parses the firmware version returns true if this code is addressing the legacy firmware version 2.3.x. </summary>
    /// <value> True if legacy firmware, false if not. </value>
    public static bool LegacyFirmware { get; set; }

    /// <summary>   Gets a value indicating whether the expected legacy firmware. </summary>
    /// <value> True if expected legacy firmware, false if not. </value>
    public static bool ExpectedLegacyFirmware => (2 == FirmwareVersion.Major) && (3 == FirmwareVersion.Minor);

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
            int result = Properties.DriverSettings.Instance.MeterDefaults.LegacyDriver;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.legacyDriver", this.DefaultsName );
            Properties.DriverSettings.Instance.MeterDefaults.LegacyDriver = result;
            return ret;
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintLegacyDriver()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default meter legacy driver;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        bool localTryQueryPrintContactLimit()
        {
            int result = Properties.DriverSettings.Instance.MeterDefaults.ContactCheckThreshold;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.contactLimit", this.DefaultsName );
            Properties.DriverSettings.Instance.MeterDefaults.ContactCheckThreshold = result;
            return ret;
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintContactLimit()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default contact check threshold;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        bool localTryQueryPrintOpenLimit()
        {
            int result = Properties.DriverSettings.Instance.MeterDefaults.OpenLimit;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.openLimit", this.DefaultsName );
            Properties.DriverSettings.Instance.MeterDefaults.OpenLimit = result;
            return ret;
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintOpenLimit()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default DUT open limit;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        bool localTryQueryPrintSourceSenseShunt()
        {
            int result = Properties.DriverSettings.Instance.MeterDefaults.SourceSenseShunt;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.shunt", this.DefaultsName );
            Properties.DriverSettings.Instance.MeterDefaults.SourceSenseShunt = result;
            return ret;
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintSourceSenseShunt()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default source-sense shunt;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

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
                Properties.DriverSettings.Instance.MeterDefaults.SourceMeasureUnit = reply;
                return true;
            }
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintSmuName()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default source measure unit name;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        bool localTryQueryPrintContactCheckOptions()
        {
            int result = Properties.DriverSettings.Instance.MeterDefaults.ContactCheckThreshold;
            bool ret = this.Session.TryQueryPrint( 1, ref result, "{0}.contactLimit", this.DefaultsName );
            Properties.DriverSettings.Instance.MeterDefaults.ContactCheckThreshold = result;
            return ret;
        }

        if ( !(MeterSubsystem.LegacyFirmware || localTryQueryPrintContactCheckOptions()) )
            _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"failed reading default contact check options;. Sent:'{this.Session.LastMessageSent}; Received:'{this.Session.LastMessageReceived}'." );

        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();

        bool localTryQueryPrintPostTransientDelay()
        {
            double result = Properties.DriverSettings.Instance.MeterDefaults.PostTransientDelay;
            bool ret = this.Session.TryQueryPrint( 7.4m, ref result, "{0}.postTransientDelay", this.DefaultsName );
            Properties.DriverSettings.Instance.MeterDefaults.PostTransientDelay = result;
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

            details = "open limit";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.OpenLimit};. " );
            _ = this.ApplyContactLimit( meterMain.OpenLimit );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.OpenLimit};. " );

            details = "source-sense shunt";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.SourceSenseShunt};. " );
            _ = this.ApplySourceSenseShunt( meterMain.SourceSenseShunt );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.SourceSenseShunt};. " );

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

                details = "open limit";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.OpenLimit};. " );
                _ = this.ApplyOpenLimit( meterMain.OpenLimit );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.OpenLimit};. " );

                details = "source-sense shunt";
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting {this.BaseEntityName} {details} to {meterMain.SourceSenseShunt};. " );
                _ = this.ApplySourceSenseShunt( meterMain.SourceSenseShunt );
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.BaseEntityName} {details} set to {meterMain.SourceSenseShunt};. " );

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
        _ = this.QueryOpenLimit();
        _ = this.QuerySourceSenseShunt();
        _ = this.QueryPostTransientDelay();
        _ = this.QuerySourceSenseShunt();
        this.Session.ThrowDeviceExceptionIfError();
    }

    #endregion

    #region " legacy driver "

    /// <summary> Gets or sets the cached legacy driver flag. </summary>
    public int? LegacyDriver
    {
        get;

        protected set
        {
            value ??= Properties.DriverSettings.Instance.MeterSettings.LegacyDriver;
            this.MeterMain.LegacyDriver = value.Value;
            _ = this.SetProperty( ref field, value );
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

    /// <summary> Gets or sets the cached Contact Limit value. </summary>
    public int? ContactLimit
    {
        get;

        protected set
        {
            value ??= Properties.DriverSettings.Instance.MeterSettings.ContactCheckThreshold;
            this.MeterMain.ContactLimit = value.Value;
            _ = this.SetProperty( ref field, value );
        }
    }

    /// <summary> Writes and reads back the Contact Limit value. </summary>
    /// <param name="value"> The Contact Limit. </param>
    /// <returns> The Contact Limit value. </returns>
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
    /// Writes the Contact Limit value without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Contact Limit. </param>
    /// <returns> The Contact Limit value. </returns>
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

    /// <summary> Gets or sets the cached Contact Check Options value. </summary>
    public Syntax.ContactCheckOptions? ContactCheckOptions
    {
        get;

        protected set
        {
            value ??= Properties.DriverSettings.Instance.MeterSettings.ContactCheckOptions;
            this.MeterMain.ContactCheckOptions = value.Value;
            _ = this.SetProperty( ref field, value );
        }
    }

    /// <summary> Writes and reads back the Contact Check Options value. </summary>
    /// <param name="value"> The Contact Check Option. </param>
    /// <returns> The Contact Check Options value. </returns>
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
    /// Writes the Contact Check Options value without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Contact Check Option. </param>
    /// <returns> The Contact Check Options value. </returns>
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

    #region " Open Limit "

    /// <summary> Gets or sets the cached Open Limit value. </summary>
    public int? OpenLimit
    {
        get;

        protected set
        {
            value ??= Properties.DriverSettings.Instance.MeterSettings.OpenLimit;
            this.MeterMain.OpenLimit = value.Value;
            _ = this.SetProperty( ref field, value );
        }
    }

    /// <summary>   Writes and reads back the Open Limit value. </summary>
    /// <remarks>   2026-03-24. </remarks>
    /// <param name="openLimit">    The Open Limit. </param>
    /// <returns>   The Open Limit value. </returns>
    public int? ApplyOpenLimit( int openLimit )
    {
        _ = this.WriteOpenLimit( openLimit );
        return this.QueryOpenLimit();
    }

    /// <summary> Queries the Open Limit. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Open Limit or none if unknown. </returns>
    public int? QueryOpenLimit()
    {
        const int printFormat = 1;
        if ( MeterSubsystem.LegacyFirmware )
            this.OpenLimit = 10;
        else
            this.OpenLimit = this.Session.QueryPrint( this.OpenLimit.GetValueOrDefault( 1 ), printFormat, $"{this.EntityName}.openLimitGetter()" );
        return this.OpenLimit;
    }

    /// <summary>
    /// Writes the Open Limit without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Open Limit. </param>
    /// <returns> The Open Limit value. </returns>
    public int? WriteOpenLimit( int value )
    {
        if ( !MeterMain.ValidateOpenLimit( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );
        if ( !MeterSubsystem.LegacyFirmware )
            _ = this.Session.WriteLine( $"{this.BaseEntityName}.openLimitSetter({value})" );
        this.OpenLimit = value;
        return this.OpenLimit;
    }

    #endregion

    #region " post transient delay "

    /// <summary> Gets or sets the cached Source Post Transient Delay. </summary>
    /// <value>
    /// The Source Post Transient Delay.
    /// </value>
    public double? PostTransientDelay
    {
        get;

        protected set
        {
            value ??= Properties.DriverSettings.Instance.MeterSettings.PostTransientDelay;
            this.MeterMain.PostTransientDelay = value.Value;
            _ = this.SetProperty( ref field, value );
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

    #region " Source-Sense shunt "

    /// <summary> Gets or sets the cached Source-Sense shunt value. </summary>
    public int? SourceSenseShunt
    {
        get;

        protected set
        {
            value ??= Properties.DriverSettings.Instance.MeterSettings.SourceSenseShunt;
            this.MeterMain.SourceSenseShunt = value.Value;
            _ = this.SetProperty( ref field, value );
        }
    }

    /// <summary>   Writes and reads back the Source-Sense shunt value. </summary>
    /// <remarks>   2026-03-24. </remarks>
    /// <param name="shunt">    The Source-Sense shunt. </param>
    /// <returns>   The Source-Sense shunt value. </returns>
    public int? ApplySourceSenseShunt( int shunt )
    {
        _ = this.WriteSourceSenseShunt( shunt );
        return this.QuerySourceSenseShunt();
    }

    /// <summary> Queries the Source-Sense shunt. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The Source-Sense shunt or none if unknown. </returns>
    public int? QuerySourceSenseShunt()
    {
        const int printFormat = 1;
        if ( MeterSubsystem.LegacyFirmware )
            this.SourceSenseShunt = 10;
        else
            this.SourceSenseShunt = this.Session.QueryPrint( this.SourceSenseShunt.GetValueOrDefault( 1 ), printFormat, $"{this.EntityName}.senseShuntGetter()" );
        return this.SourceSenseShunt;
    }

    /// <summary>
    /// Writes the Source-Sense shunt without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///                                                the required range. </exception>
    /// <param name="value"> The Source-Sense shunt. </param>
    /// <returns> The Source-Sense shunt value. </returns>
    public int? WriteSourceSenseShunt( int value )
    {
        if ( !MeterMain.ValidateContactThreshold( value, out string details ) )
            throw new ArgumentOutOfRangeException( nameof( value ), details );
        if ( !MeterSubsystem.LegacyFirmware )
            _ = this.Session.WriteLine( $"{this.BaseEntityName}.senseShuntSetter({value})" );
        this.SourceSenseShunt = value;
        return this.SourceSenseShunt;
    }

    #endregion
}
