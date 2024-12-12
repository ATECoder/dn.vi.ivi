using System;
using System.Diagnostics;
using cc.isr.VI.Tsp.SessionBaseExtensions;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;

namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Firmware;
/// <summary>   An asserts. </summary>
/// <remarks>   2024-10-31. </remarks>
internal static partial class Asserts
{
    /// <summary>   Gets or sets the legacy driver. </summary>
    /// <voltageChange> The legacy driver. </voltageChange>
    public static int LegacyDriver { get; set; } = 1;

    /// <summary>   Gets or sets a value indicating whether the legacy firmware. </summary>
    /// <voltageChange> True if legacy firmware, false if not. </voltageChange>
    public static bool LegacyFirmware { get; set; }

    /// <summary> Logs an iterator. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The voltageChange. </param>
    public static void LogIT( string value )
    {
        Console.Out.WriteLine( value );
    }

    /// <summary>   Assert orphan messages or device errors. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="memberName">   (Optional) Name of the member. </param>
    public static void AssertOrphanMessagesOrDeviceErrors( Pith.SessionBase? session, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "" )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"VISA session to '{nameof( session.ResourceNameCaption )}' must be open." );
        string orphanMessages = session.ReadLines( session.StatusReadDelay, TimeSpan.FromMilliseconds( 100 ), false );
        Assert.IsTrue( string.IsNullOrWhiteSpace( orphanMessages ), $"The following messages were left on the output buffer after {memberName}:/n{orphanMessages}" );
        session.ThrowDeviceExceptionIfError( failureMessage: $"{nameof( Pith.SessionBase.ReadLines )} after {memberName} failed" );
    }

    /// <summary>   Assert the the *TRG command should trigger measurements. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="timeout">  The timeout. </param>
    /// <returns>   A string. </returns>
    public static void AssertTheTRGCommandShouldTriggerMeasurements( Pith.SessionBase? session, TimeSpan timeout )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        bool messageAvailable = false;

        string expectedReading = "OPC";
        _ = session.WriteLine( $"prepareForTrigger(true,'{expectedReading}') " );

        //allow some tim for the trigger loop to start..
        _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );
        session.AssertTrigger();
        // _ = session.WriteLine( "*TRG " );

        Stopwatch sw = Stopwatch.StartNew();
        while ( !messageAvailable && sw.Elapsed < timeout )
        {
            _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 10 ) );
            messageAvailable = session.IsMeasurementEventBitSet( session.ReadStatusByte() );
        }

        Assert.IsTrue( messageAvailable, "The trigger wait should not timeout." );

        string reading = session.ReadLineTrimEnd();
        Assert.IsFalse( string.IsNullOrEmpty( reading ), $"Triggered output should not be null or empty." );
        Assert.AreEqual( expectedReading, reading, "The triggered output should match the expected reading" );
    }

    /// <summary>   Assert estimates should read. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="session">              The session. </param>
    /// <param name="initialResistance">    The initial resistance. </param>
    public static void AssertEstimatesShouldRead( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        int? outcome = session.QueryNullableIntegerThrowIfError( "print(ttm.est.outcome) ", "Estimator outcome" );
        double? initialVoltage = session.QueryNullableIntegerThrowIfError( "print(ttm.est.initialVoltage) ", "Estimator Initial Voltage" );
        double? finalVoltage = session.QueryNullableDoubleThrowIfError( "print(ttm.est.finalVoltage) ", "Estimator Final Voltage" );
        double? voltageChange = session.QueryNullableDoubleThrowIfError( "print(ttm.est.voltageChange) ", "Estimator Voltage Change" );

        // the legacy driver does not call the estimator estimate and, therefore, could be tested even if testing compliance with the legacy driver.

        if ( !Asserts.LegacyFirmware && outcome.HasValue )
            _ = session.WriteLine( "ttm.est:estimate( ttm.ir.resistance, ttm.tr ) " );

        double? temperatureChange = session.QueryNullableDoubleThrowIfError( "print(ttm.est.temperatureChange) ", "Estimator Temperature Change" );
        double? thermalConductance = session.QueryNullableDoubleThrowIfError( "print(ttm.est.thermalConductance) ", "Estimator Thermal Conductance" );
        double? thermalTimeConstant = session.QueryNullableDoubleThrowIfError( "print(ttm.est.thermalTimeConstant) ", "Estimator Thermal Time Constant" );
        double? thermalCapacitance = session.QueryNullableDoubleThrowIfError( "print(ttm.est.thermalCapacitance) ", "Estimator Thermal Capacitance" );
        if ( outcome.HasValue )
        {
            Assert.IsNotNull( initialVoltage, $"{nameof( initialVoltage )} should not be null if {nameof( outcome )} is not null." );
            Assert.IsNotNull( finalVoltage, $"{nameof( finalVoltage )} should not be null if {nameof( outcome )} is not null." );
            double expectedVoltageChange = finalVoltage.Value - initialVoltage.Value;
            Assert.IsNotNull( voltageChange, $"{nameof( voltageChange )} should not be null if {nameof( outcome )} is not null." );
            Assert.AreEqual( expectedVoltageChange, voltageChange.Value, 0.0001, $"{nameof( voltageChange )} should equal expected value." );

            // the legacy driver is agnostic of the estimated values and, therefore, could be tested even if testing compliance with the legacy driver.

            if ( !Asserts.LegacyFirmware )
            {
                Assert.IsNotNull( temperatureChange, $"{nameof( temperatureChange )} should not be null if {nameof( outcome )} is not null." );
                Assert.IsNotNull( thermalConductance, $"{nameof( thermalConductance )} should not be null if {nameof( outcome )} is not null." );
                Assert.IsNotNull( thermalTimeConstant, $"{nameof( thermalTimeConstant )} should not be null if {nameof( outcome )} is noy null." );
                Assert.IsNotNull( thermalCapacitance, $"{nameof( thermalCapacitance )} should not be null if {nameof( outcome )} is not null." );
            }
        }
        else
        {
            Assert.IsNull( initialVoltage, $"{nameof( initialVoltage )} {initialVoltage} should be null if {nameof( outcome )} is null." );
            Assert.IsNull( finalVoltage, $"{nameof( finalVoltage )} {finalVoltage} should be null if {nameof( outcome )} is null." );
            Assert.IsNull( voltageChange, $"{nameof( voltageChange )} {finalVoltage} should be null if {nameof( outcome )} is null." );

            Assert.IsNull( temperatureChange, $"{nameof( temperatureChange )} {temperatureChange} should be null if {nameof( outcome )} is null." );
            Assert.IsNull( thermalConductance, $"{nameof( thermalConductance )} {thermalConductance} should be null if {nameof( outcome )} is null." );
            Assert.IsNull( thermalTimeConstant, $"{nameof( thermalTimeConstant )} {thermalTimeConstant} should be null if {nameof( outcome )} is null." );
            Assert.IsNull( thermalCapacitance, $"{nameof( thermalCapacitance )} {thermalCapacitance} should be null if {nameof( outcome )} is null." );

        }
    }

    /// <summary>   Assert Initial Resistance should read low, high and pass. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool? Low, bool? High, bool? Pass) AssertInitialResistanceShouldReadLowHighPass( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        // the legacy driver is agnostic to the low and high properties and, therefore, could be tested even if testing the legacy driver.

        bool? low = Asserts.LegacyFirmware
            ? new bool?()
            : session.QueryNullableBoolThrowIfError( "print(ttm.ir.low) ", "Initial MeasuredValue Low" );

        bool? high = Asserts.LegacyFirmware
            ? new bool?()
            : session.QueryNullableBoolThrowIfError( "print(ttm.ir.high) ", "Initial MeasuredValue High" );

        bool? pass = session.QueryNullableBoolThrowIfError( "print(ttm.ir.pass) ", "Initial MeasuredValue Pass" );
        return (low, high, pass);
    }

    /// <summary>   Assert final resistance should read low, high and pass. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool? Low, bool? High, bool? Pass) AssertFinalResistanceShouldReadLowHighPass( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        // the legacy driver is agnostic to the low and high properties and, therefore, could be tested even if testing the legacy driver.

        bool? low = Asserts.LegacyFirmware
            ? new bool?()
            : session.QueryNullableBoolThrowIfError( "print(ttm.fr.low) ", "Final MeasuredValue Low" );

        bool? high = Asserts.LegacyFirmware
            ? new bool?()
            : session.QueryNullableBoolThrowIfError( "print(ttm.fr.high) ", "Final MeasuredValue High" );

        bool? pass = session.QueryNullableBoolThrowIfError( "print(ttm.fr.pass) ", "Final MeasuredValue Pass" );
        return (low, high, pass);
    }

    /// <summary>   Assert thermal transient should read low, high and pass. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool? Low, bool? High, bool? Pass) AssertThermalTransientShouldReadLowHighPass( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        // the low and high properties are not used by the legacy driver and, therefore, could be tested even if testing the legacy driver.

        bool? low = Asserts.LegacyFirmware
            ? new bool?()
            : session.QueryNullableBoolThrowIfError( "print(ttm.tr.low) ", "Thermal Transient Low" );

        bool? high = Asserts.LegacyFirmware
            ? new bool?()
            : session.QueryNullableBoolThrowIfError( "print(ttm.tr.high) ", "Thermal Transient High" );

        bool? pass = session.QueryNullableBoolThrowIfError( "print(ttm.tr.pass) ", "Thermal Transient Pass" );
        return (low, high, pass);
    }


    /// <summary>   Assert Initial Resistance should read limits. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (double LowLimit, double HighLimit) AssertInitialResistanceShouldReadLimits( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        double lowLimit = session.QueryDoubleThrowIfError(  "print(ttm.ir.lowLimit) ", "Initial MeasuredValue Low Limit" );
        double highLimit = session.QueryDoubleThrowIfError(  "print(ttm.ir.highLimit) ", "Initial MeasuredValue High Limit" );
        return (lowLimit, highLimit);
    }

    /// <summary>   Assert Final Resistance should read limits. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (double LowLimit, double HighLimit) AssertFinalResistanceShouldReadLimits( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        double lowLimit = session.QueryDoubleThrowIfError(  "print(ttm.fr.lowLimit) ", "Final MeasuredValue Low Limit" );
        double highLimit = session.QueryDoubleThrowIfError(  "print(ttm.fr.highLimit) ", "Final MeasuredValue High Limit" );
        return (lowLimit, highLimit);
    }

    /// <summary>   Assert thermal transient should read limits. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (double LowLimit, double HighLimit) AssertThermalTransientShouldReadLimits( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        double lowLimit = session.QueryDoubleThrowIfError(  "print(ttm.tr.lowLimit) ", "Thermal Transient Low Limit" );
        double highLimit = session.QueryDoubleThrowIfError(  "print(ttm.tr.highLimit) ", "Thermal Transient High Limit" );
        return (lowLimit, highLimit);
    }

    /// <summary>   Assert Initial Resistance should be fetched. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (int? Outcome, int? status, bool okayAndPass, double? Resistance) AssertInitialResistanceShouldBeFetched( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        bool okayAndPass = session.QueryBoolThrowIfError(  "print(ttm.ir:isOkayAndPass()) ", "Initial MeasuredValue Initial and Pass" );
        int? outcome = session.QueryNullableIntegerThrowIfError( "print(ttm.ir.outcome) ", "Initial MeasuredValue MeasurementOutcome" );
        int? status = session.QueryNullableIntegerThrowIfError( "print(ttm.ir.status) ", "Initial MeasuredValue Buffer StatusReading" );
        double? resistance = session.QueryNullableDoubleThrowIfError( "print(ttm.ir.resistance) ", "Initial MeasuredValue" );
        return (outcome, status, okayAndPass, resistance);
    }

    /// <summary>   Assert thermal transient should be fetched. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (int? Outcome, int? Status, bool okayAndPass, double? VoltageChange) AssertThermalTransientShouldBeFetched( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        bool okayAndPass = session.QueryBoolThrowIfError(  "print(ttm.tr:isOkayAndPass()) ", "Initial MeasuredValue Initial and Pass" );
        int? outcome = session.QueryNullableIntegerThrowIfError( "print(ttm.tr.outcome) ", "Thermal Transient MeasurementOutcome" );
        int? status = session.QueryNullableIntegerThrowIfError( "print(ttm.tr.status) ", "Thermal Transient Buffer StatusReading" );
        double? value = session.QueryNullableDoubleThrowIfError( "print(ttm.tr.voltageChange) ", "Thermal Transient" );
        return (outcome, status, okayAndPass, value);
    }

    /// <summary>   Assert Final Resistance should be fetched. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (int? Outcome, int? Status, bool okayAndPass, double? Resistance) AssertFinalResistanceShouldBeFetched( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        bool okayAndPass = session.QueryBoolThrowIfError(  "print(ttm.fr:isOkayAndPass()) ", "Initial MeasuredValue Initial and Pass" );
        int? outcome = session.QueryNullableIntegerThrowIfError( "print(ttm.fr.outcome) ", "Final MeasuredValue MeasurementOutcome" );
        int? status = session.QueryNullableIntegerThrowIfError( "print(ttm.fr.status) ", "Final MeasuredValue Buffer StatusReading" );
        double? resistance = session.QueryNullableDoubleThrowIfError( "print(ttm.fr.resistance) ", "Final MeasuredValue" );
        return (outcome, status, okayAndPass, resistance);
    }

    /// <summary>   Assert initial resistance should read contact check. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool? Okay, double? LowLeadsResistance, double? HighLeadsResistance) AssertInitialResistanceShouldReadContactCheck( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        bool? okay = session.QueryNullableBoolThrowIfError( "print(ttm.ir.contactsOkay) ", "Initial MeasuredValue Contacts Initial" );
        double? low = session.QueryNullableDoubleThrowIfError( "print(ttm.ir.lowContact) ", "Initial MeasuredValue Low Leads MeasuredValue" );
        double? high = session.QueryNullableDoubleThrowIfError( "print(ttm.ir.highContact) ", "Initial MeasuredValue High Leads MeasuredValue" );
        return (okay, low, high);
    }

    /// <summary>   Assert final resistance should read contact check. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool? Okay, double? LowLeadsResistance, double? HighLeadsResistance) AssertFinalResistanceShouldReadContactCheck( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        bool? okay = session.QueryNullableBoolThrowIfError( "print(ttm.fr.contactsOkay) ", "Final MeasuredValue Contacts Initial" );
        double? low = session.QueryNullableDoubleThrowIfError( "print(ttm.fr.lowContact) ", "Final MeasuredValue Low Leads MeasuredValue" );
        double? high = session.QueryNullableDoubleThrowIfError( "print(ttm.fr.highContact) ", "Final MeasuredValue High Leads MeasuredValue" );
        return (okay, low, high);
    }

    /// <summary>   Assert trace should read contact check. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool? Okay, double? LowLeadsResistance, double? HighLeadsResistance) AssertTraceShouldReadContactCheck( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        bool? okay = session.QueryNullableBoolThrowIfError( "print(ttm.tr.contactsOkay) ", "Trace Contacts Initial" );
        double? low = session.QueryNullableDoubleThrowIfError( "print(ttm.tr.lowContact) ", "Trace Low Leads MeasuredValue" );
        double? high = session.QueryNullableDoubleThrowIfError( "print(ttm.tr.highContact) ", "Trace High Leads MeasuredValue" );
        return (okay, low, high);
    }

    /// <summary>   Assert meter should read contact check options. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   An int? </returns>
    public static int? AssertMeterShouldReadContactCheckOptions( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        // the legacy driver is agnostic of contact checks and, therefore, these attributes can be tested

        return Asserts.LegacyFirmware
            ? new int?()
            : session.QueryIntegerThrowIfError( "print(ttm.contactCheckOptionsGetter()) ", "Meter Values Contact Check Options" );
    }

    /// <summary>   Assert meter should read contact check limit. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A double? </returns>
    public static double? AssertMeterShouldReadContactCheckLimit( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        // the legacy driver is agnostic of contact checks and, therefore, these attributes can be tested

        return Asserts.LegacyFirmware
            ? new double?()
            : session.QueryDoubleThrowIfError(  "print(ttm.contactLimitGetter()) ", "Meter Values Contact Check Options" );
    }

    /// <summary>   Assert contact check should conform. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="outcomes"> The outcomes. </param>
    /// <param name="options">  Options for controlling the operation. </param>
    /// <param name="option">   The option. </param>
    /// <param name="limit">    The limit. </param>
    /// <param name="okay">     True to okay. </param>
    /// <param name="low">      True to low. </param>
    /// <param name="high">     True to high. </param>
    public static void AssertContactCheckShouldConform( int? outcomes, int? options, ContactCheckOptions option, double? limit, bool? okay, double? low, double? high )
    {
        Assert.IsNotNull( options, $"Meter Contact Check {nameof( options )} should not be null." );
        Assert.IsNotNull( limit, $"Leads resistance {nameof( limit )} should not be null." );

        if ( 0 == (options.Value & ( int ) option) )
        {
            // if the specified option is net enabled, the contact check values should be null.
            Assert.IsFalse( okay.HasValue, $"{nameof( okay )} should not have a value {okay} if the options {options} do not include {option}." );
            Assert.IsFalse( low.HasValue, $"{nameof( low )} contact resistance should not have a value {low}if the options {options} do not include {option}." );
            Assert.IsFalse( high.HasValue, $"{nameof( high )} contact resistance should not have a value {high}if the options {options} do not include {option}." );
        }
        else
        {
            if ( outcomes.HasValue )
            {
                if ( 0 == (( FirmwareOutcomes ) outcomes & FirmwareOutcomes.openLeads) )
                {
                    // if leads are not open okay should be true and low and high should be zero
                    Assert.IsTrue( okay, $"{nameof( okay )} value should not be {okay} if contact check did not fail if outcome has no value." );
                    double expectedValue = 0;
                    Assert.IsNotNull( low, $"{nameof( low )} contact resistance should not be null if leads are not open." );
                    Assert.AreEqual( expectedValue, low.Value, $"{nameof( low )} contact resistance should equal the expected value if leads are not open." );
                    Assert.IsNotNull( high, $"{nameof( high )} contact resistance should not be null if leads are not open." );
                    Assert.AreEqual( expectedValue, high.Value, $"{nameof( high )} contact resistance should equal the expected value if leads are not open." );
                }
                else
                {
                    // if leads are open okay should be false and low and high should be higher than the limit
                    Assert.IsFalse( okay, $"{nameof( okay )} value should not be {okay} if contact check did not fail if outcome has no value." );

                    Assert.IsNotNull( low, $"{nameof( low )} contact resistance should not be null if leads are open." );
                    Assert.IsNotNull( high, $"{nameof( high )} contact resistance should not be null if leads are open." );
                    Assert.IsTrue( (low.Value > limit.Value) || (high.Value > limit.Value),
                        $"{nameof( low )} ({low}) and/or {nameof( high )} ({high}) contact values should exceed the contact check threshold {limit}." );
                }
            }
            else
            {
                // if outcome has no value., than the rest of the items should not have values
                Assert.IsFalse( okay.HasValue, $"{nameof( okay )} should not have a value {okay} if outcome has no value." );
                Assert.IsFalse( low.HasValue, $"{nameof( low )} contact resistance should not have a value {low} if outcome has no value." );
                Assert.IsFalse( high.HasValue, $"{nameof( high )} contact resistance should not have a value {high} if outcome has no value." );
            }
        }
    }

    /// <summary>   Assert measurement should conform. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="outcome">      The outcome. </param>
    /// <param name="status">       The status. </param>
    /// <param name="okayAndPass">  True to okay and pass. </param>
    /// <param name="measurement">  The measurement. </param>
    /// <param name="lowLimit">     The low limit. </param>
    /// <param name="highLimit">    The high limit. </param>
    /// <param name="low">          True to low. </param>
    /// <param name="high">         True to high. </param>
    /// <param name="pass">         True to pass. </param>
    public static void AssertMeasurementShouldConform( int? outcome, int? status, bool okayAndPass,
        double? measurement, double lowLimit, double highLimit, bool? low, bool? high, bool? pass )
    {
        if ( outcome.HasValue )
        {
            FirmwareOutcomes firmwareOutcome = ( FirmwareOutcomes ) outcome.Value;
            if ( firmwareOutcome == cc.isr.VI.Tsp.K2600.Ttm.Syntax.FirmwareOutcomes.Okay )
            {
                // check that values exist.
                Assert.IsNotNull( measurement, $"{nameof( measurement )} should not be null if outcome {firmwareOutcome} is okay." );
                Assert.IsNotNull( pass, $"{nameof( pass )} should not be null if outcome {firmwareOutcome} is okay." );
                Assert.AreEqual( okayAndPass, pass, $"{nameof( pass )} {pass} should equal {nameof( okayAndPass )} {okayAndPass} if outcome is okay." );

                // the legacy driver is agnostic to the low and high properties and, therefore, could be tested even if testing the legacy driver.

                if ( !Asserts.LegacyFirmware )
                {
                    Assert.IsNotNull( low, $"{nameof( low )} should not be null if outcome {firmwareOutcome} is okay." );
                    Assert.IsNotNull( high, $"{nameof( high )} should not be null if outcome {firmwareOutcome} is okay." );
                    Assert.AreEqual( pass.Value, !(low.Value || high.Value), $"{nameof( pass )} {pass} should equal not (low {low} or high {high})" );

                    Assert.AreEqual( low, measurement < lowLimit,
                        $"{nameof( low )} {low} should equals {measurement < lowLimit} if {nameof( measurement )} {measurement} is less than {nameof( lowLimit )} {lowLimit}." );
                    Assert.AreEqual( high, measurement > highLimit,
                        $"{nameof( high )} {high} should equals {measurement > highLimit} if {nameof( measurement )} {measurement} is greater than {nameof( highLimit )} {highLimit}." );
                }
            }
            else
            {
                Asserts.AssertMeasurementShouldConformOnNotOkay( status, okayAndPass, measurement, low, high, pass );

                // legacy driver is oblivious to contact check but expects NaN if contact check failed, therefore, this needs to be tested.

                if ( !Asserts.LegacyFirmware )
                {
                    if ( 0 != (firmwareOutcome & cc.isr.VI.Tsp.K2600.Ttm.Syntax.FirmwareOutcomes.openLeads) )
                    {
                        // if contact check failed, the voltage change reading should be NaN in millivolts
                        Assert.IsNotNull( measurement, $"{nameof( measurement )} should have a value after contact check." );
                        if ( (0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber) > measurement )
                            Assert.Fail( $"{nameof( measurement )} should be set to {cc.isr.VI.Syntax.ScpiSyntax.NotANumber} or {0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber} if contact check failed." );
                    }
                }
            }
        }
        else
        {
            Asserts.AssertMeasurementShouldConformOnNotOkay( status, okayAndPass, measurement, low, high, pass );
        }
    }

    /// <summary>   Assert measurement should conform . </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="status">       The status. </param>
    /// <param name="okayAndPass">  True to okay and pass. </param>
    /// <param name="measurement">  The measurement. </param>
    /// <param name="low">          True to low. </param>
    /// <param name="high">         True to high. </param>
    /// <param name="pass">         True to pass. </param>
    public static void AssertMeasurementShouldConformOnNotOkay( int? status, bool okayAndPass, double? measurement, bool? low, bool? high, bool? pass )
    {
        // the TTM initializes pass and measurement to nil
        Assert.IsFalse( measurement.HasValue, $"'{nameof( measurement )}' should not have a value if measurements were not made." );
        Assert.IsFalse( pass.HasValue, $"'{nameof( pass )}' should not hae a value if measurements were not made." );
        Assert.IsFalse( okayAndPass, $"'{nameof( okayAndPass )}' must not be true if outcome is nil." );

        // the legacy driver is agnostic to the low and high properties and, therefore, could be tested even if testing the legacy driver.

        // if outcome is null, the measurement was not made.
        if ( Asserts.LegacyFirmware )
        {
            Assert.IsFalse( low.HasValue, "'Low' is not set with the legacy TTM." );
            Assert.IsFalse( high.HasValue, "'High' is not set with the legacy TTM." );
        }
        else
        {
            // status is not initialized to nil with trace
            Assert.IsFalse( status.HasValue, $"'{nameof( status )}' should not have a value if measurements were not made." );

            // the TTM initializes the low, high, and pass to nil
            Assert.IsFalse( low.HasValue, "'Low' should not have a value if measurements were not made." );
            Assert.IsFalse( high.HasValue, "'High' should not have a value if measurements were not made." );
        }
    }

    /// <summary>   Assert framework should clear known state. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="session">  The session. </param>
    public static void AssertFrameworkShouldClearKnownState( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        // issue the TTM clear command
        _ = session.WriteLine( "ttm.clearMeasurements() " );
    }

    /// <summary>   Assert Initial Resistance readings. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    public static void AssertInitialResistanceReadings( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        (int? outcome, int? status, bool okayAndPass, double? resistance) = Asserts.AssertInitialResistanceShouldBeFetched( session );
        (bool? low, bool? high, bool? pass) = Asserts.AssertInitialResistanceShouldReadLowHighPass( session );
        (double lowLimit, double highLimit) = Asserts.AssertInitialResistanceShouldReadLimits( session );
        Asserts.AssertMeasurementShouldConform( outcome, status, okayAndPass, resistance, lowLimit, highLimit, low, high, pass );
        if ( !outcome.HasValue )
            // status is initialized to nil with cold resistance
            Assert.IsFalse( status.HasValue, $"'{nameof( status )}' should not have a value if measurements were not made." );

        // the legacy driver is oblivious to contact check but will see the NaN result if contact check failed.

        if ( !Asserts.LegacyFirmware )
        {
            int? contactOptions = Asserts.AssertMeterShouldReadContactCheckOptions( session );
            double? limit = Asserts.AssertMeterShouldReadContactCheckLimit( session );
            (bool? okay, double? lowLeads, double? highLeads) = Asserts.AssertInitialResistanceShouldReadContactCheck( session );
            Asserts.AssertContactCheckShouldConform( outcome, contactOptions, ContactCheckOptions.Initial, limit, okay, lowLeads, highLeads );

            if ( ( int ) ContactCheckOptions.Initial == (contactOptions & ( int ) ContactCheckOptions.Initial) )
            {
                if ( okay.HasValue && !okay.Value )
                {
                    // if contact check failed, the voltage change reading should be NaN in millivolts
                    Assert.IsNotNull( resistance, $"{nameof( resistance )} should have a value after contact check." );
                    Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, resistance, $"{nameof( resistance )} should be set to NaN if contact check failed." );
                }
            }
        }
    }

    /// <summary>   Assert Final Resistance readings. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    public static void AssertFinalResistanceReadings( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        (int? outcome, int? status, bool okayAndPass, double? resistance) = Asserts.AssertFinalResistanceShouldBeFetched( session );
        (bool? low, bool? high, bool? pass) = Asserts.AssertFinalResistanceShouldReadLowHighPass( session );
        (double lowLimit, double highLimit) = Asserts.AssertFinalResistanceShouldReadLimits( session );
        Asserts.AssertMeasurementShouldConform( outcome, status, okayAndPass, resistance, lowLimit, highLimit, low, high, pass );
        if ( !outcome.HasValue )
            // status is initialized to nil with cold resistance
            Assert.IsFalse( status.HasValue, $"'{nameof( status )}' should not have a value if measurements were not made." );

        // the legacy driver is oblivious to contact check but will see the NaN result if contact check failed.

        if ( !Asserts.LegacyFirmware )
        {
            int? contactOptions = Asserts.AssertMeterShouldReadContactCheckOptions( session );
            double? limit = Asserts.AssertMeterShouldReadContactCheckLimit( session );
            (bool? okay, double? lowLeads, double? highLeads) = Asserts.AssertFinalResistanceShouldReadContactCheck( session );
            Asserts.AssertContactCheckShouldConform( outcome, contactOptions, ContactCheckOptions.Final, limit, okay, lowLeads, highLeads );

            if ( ( int ) ContactCheckOptions.Final == (contactOptions & ( int ) ContactCheckOptions.Final) )
            {
                if ( okay.HasValue && !okay.Value )
                {
                    // if contact check failed, the voltage change reading should be NaN in millivolts
                    Assert.IsNotNull( resistance, $"{nameof( resistance )} should have a value after contact check." );
                    Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, resistance, $"{nameof( resistance )} should be set to NaN if contact check failed." );
                }
            }
        }
    }

    /// <summary>   Assert thermal transient readings. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    public static void AssertThermalTransientReadings( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        (int? outcome, int? status, bool okayAndPass, double? voltageChange) = Asserts.AssertThermalTransientShouldBeFetched( session );

        // the low and high properties are not used by the legacy driver and, therefore, could be tested even if testing the legacy driver.

        (bool? low, bool? high, bool? pass) = Asserts.AssertThermalTransientShouldReadLowHighPass( session );
        (double lowLimit, double highLimit) = Asserts.AssertThermalTransientShouldReadLimits( session );
        Asserts.AssertMeasurementShouldConform( outcome, status, okayAndPass, voltageChange, lowLimit, highLimit, low, high, pass );

        // the legacy driver is oblivious to contact check but will see the NaN result if contact check failed.

        if ( !Asserts.LegacyFirmware )
        {
            int? contactOptions = Asserts.AssertMeterShouldReadContactCheckOptions( session );
            Assert.IsNotNull( contactOptions, nameof( contactOptions ) );
            double? limit = Asserts.AssertMeterShouldReadContactCheckLimit( session );
            (bool? okay, double? lowLeads, double? highLeads) = Asserts.AssertTraceShouldReadContactCheck( session );
            Asserts.AssertContactCheckShouldConform( outcome, contactOptions, ContactCheckOptions.PreTrace, limit, okay, lowLeads, highLeads );

            if ( ( int ) ContactCheckOptions.PreTrace == (contactOptions & ( int ) ContactCheckOptions.PreTrace) )
            {
                if ( okay.HasValue && !okay.Value )
                {
                    // if contact check failed, the voltage change reading should be NaN in millivolts
                    Assert.IsNotNull( voltageChange, $"{nameof( voltageChange )} should have a value after contact check." );
                    Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, 1000 * voltageChange, $"{nameof( voltageChange )} should be set to NaN if contact check failed." );
                }
            }
        }
    }

}
