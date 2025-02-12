using System.Diagnostics;
using System;
using cc.isr.VI.Tsp.SessionBaseExtensions;
using cc.isr.VI.Tsp.ParseStringExtensions;
using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

internal static partial class Asserts
{
    /// <summary>   Assert meter should preset. </summary>
    /// <remarks>   2024-11-15. </remarks>
    /// <param name="legacyDevice"> The legacy device. </param>
    public static void AssertMeterShouldInitializeKnowState( LegacyDevice? legacyDevice )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsTrue( legacyDevice.IsConnected, "The driver should be connected if the session opened." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice, $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.Meter )}.{nameof( LegacyDevice.Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Meter meter = legacyDevice.Meter;
        Pith.SessionBase session = legacyDevice.Meter.TspDevice.Session;

        // the legacy software has no definition for open leads.
        Asserts.LegacyFirmware = MeterSubsystem.LegacyFirmware;

        // initialize meter state: this reads the meter model number.
        meter.InitKnownState();

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );
    }

    /// <summary>   Assert meter should preset. </summary>
    /// <remarks>   2024-11-15. </remarks>
    /// <param name="legacyDevice"> The legacy device. </param>
    public static void AssertMeterShouldPreset( LegacyDevice? legacyDevice )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsTrue( legacyDevice.IsConnected, "The driver should be connected if the session opened." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice, $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.Meter )}.{nameof( LegacyDevice.Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Meter meter = legacyDevice.Meter;
        Pith.SessionBase session = legacyDevice.Meter.TspDevice.Session;

        // detect the legacy firmware
        string ttmObjectName = "ttm";
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        ttmObjectName = "ttm.OutcomeBits";
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        ttmObjectName = "ttm.OutcomeBits.okay";
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        // the legacy software has no definition for open leads.
        Asserts.LegacyFirmware = MeterSubsystem.LegacyFirmware;

        // preset the meter configuration
        meter.PresetKnownState();
        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

        // prepare the device under test.
        DeviceUnderTest deviceUnderTest = meter.ConfigInfo;

        // test DUT initialization
        deviceUnderTest.ClearPartInfo();
        deviceUnderTest.ClearMeasurements();

        deviceUnderTest.PartNumber = "part number";
        deviceUnderTest.LotId = "lot id";
        deviceUnderTest.OperatorId = "operator id";
        meter.Part = deviceUnderTest;

        Assert.IsNotNull( meter.MeasureSequencer, $"{nameof( Meter )}.{nameof( Meter.MeasureSequencer )} should not be null." );
        Assert.IsNotNull( meter.TriggerSequencer, $"{nameof( Meter )}.{nameof( Meter.TriggerSequencer )} should not be null." );
    }

    /// <summary>   Assert source should configure. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <param name="legacyDevice">         The legacy device. </param>
    /// <param name="sourceOutputOption">   The current source output option. </param>
    public static void AssertSourceShouldConfigure( LegacyDevice? legacyDevice, SourceOutputOption sourceOutputOption )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsTrue( legacyDevice.IsConnected, "The driver should be connected if the session opened." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice, $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.Meter )}.{nameof( LegacyDevice.Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Meter meter = legacyDevice.Meter;
        Pith.SessionBase session = legacyDevice.Meter.TspDevice.Session;

        float value = 0.01f;

        legacyDevice.ColdResistanceConfig.CurrentLevel = value;
        Assert.AreEqual( value, legacyDevice.ColdResistanceConfig.CurrentLevel, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceConfig )}.{nameof( LegacyDevice.ColdResistanceConfig.CurrentLevel )} should be assigned to the expected value." );
        value *= 0.9f;
        _ = legacyDevice.ColdResistanceCurrentLevelSetter( value );

        if ( Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption )
        {
            Assert.AreEqual( value, legacyDevice.ColdResistanceCurrentLevel, value * 0.001,
                $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceCurrentLevel )} should be set to the expected value." );
            _ = legacyDevice.ColdResistanceCurrentLevelGetter();
            Assert.AreEqual( value, legacyDevice.ColdResistanceCurrentLevel, value * 0.001,
                $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceCurrentLevel )} should be gotten equal to the expected value." );
        }
        else
        {
            // with new software and voltage source, the current level setter sets the ir.limit or fr.limit, which called voltage limit
            _ = legacyDevice.ColdResistanceVoltageLimitGetter();
            Assert.AreEqual( value, legacyDevice.ColdResistanceVoltageLimit, value * 0.001,
                $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceVoltageLimit )} should be gotten equal to the expected value." );

        }
        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

        value = 0.02f;
        legacyDevice.ColdResistanceConfig.VoltageLimit = value;
        Assert.AreEqual( value, legacyDevice.ColdResistanceConfig.VoltageLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceConfig )}.{nameof( LegacyDevice.ColdResistanceConfig.VoltageLimit )} should be assigned to the expected value." );
        value *= 0.9f;
        _ = legacyDevice.ColdResistanceVoltageLimitSetter( value );

        if ( Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption )
        {
            Assert.AreEqual( value, legacyDevice.ColdResistanceVoltageLimit, value * 0.001,
                $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceVoltageLimit )} should be set to the expected value." );
            _ = legacyDevice.ColdResistanceVoltageLimitGetter();
            Assert.AreEqual( value, legacyDevice.ColdResistanceVoltageLimit, value * 0.001,
                $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceVoltageLimit )} should be gotten equal to the expected value." );

        }
        else
        {
            // with new software and voltage source, the voltage limit setter sets the level, which is called current level
            _ = legacyDevice.ColdResistanceCurrentLevelGetter();
            Assert.AreEqual( value, legacyDevice.ColdResistanceCurrentLevel, value * 0.001,
                $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceCurrentLevel )} should be gotten equal to the expected value." );
        }
        Asserts.AssertOrphanMessagesOrDeviceErrors( session );
    }

    /// <summary>   Assert measurements should configure. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="legacyDevice"> The legacy device. </param>
    public static void AssertMeasurementsShouldConfigure( LegacyDevice? legacyDevice )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsTrue( legacyDevice.IsConnected, "The driver should be connected if the session opened." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice, $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.Meter )}.{nameof( LegacyDevice.Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Meter meter = legacyDevice.Meter;
        Pith.SessionBase session = legacyDevice.Meter.TspDevice.Session;

        if ( !legacyDevice.ColdResistanceSourceOutputGetter( out SourceOutputOption initialSourceOutputOption ) )
        {
            if ( !legacyDevice.ColdResistanceSourceOutputSetter( initialSourceOutputOption ) )
                throw new InvalidOperationException( "Failed setting the initial source output option." );
        }

        SourceOutputOption alternateOption = Ttm.MeterSubsystem.LegacyFirmware
            ? SourceOutputOption.Current
            : initialSourceOutputOption == SourceOutputOption.Current
                ? SourceOutputOption.Voltage
                : SourceOutputOption.Current;

        Asserts.AssertSourceShouldConfigure( legacyDevice, initialSourceOutputOption );

        if ( alternateOption != initialSourceOutputOption )
        {
            Assert.IsTrue( legacyDevice.ColdResistanceSourceOutputSetter( alternateOption ),
                $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceSourceOutputSetter )} should set to {alternateOption}" );

            // validate the alternate option settings.
            Asserts.AssertSourceShouldConfigure( legacyDevice, alternateOption );

            // restore the initial settings
            Assert.IsTrue( legacyDevice.ColdResistanceSourceOutputSetter( initialSourceOutputOption ),
                $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceSourceOutputSetter )} should set to {initialSourceOutputOption}" );

            // validate the initial option settings.
            Asserts.AssertSourceShouldConfigure( legacyDevice, initialSourceOutputOption );

        }

        float value = 2.6f;
        legacyDevice.ColdResistanceConfig.HighLimit = value;
        Assert.AreEqual( value, legacyDevice.ColdResistanceConfig.HighLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceConfig )}.{nameof( LegacyDevice.ColdResistanceConfig.HighLimit )} should be assigned to the expected value." );
        value *= 1.1f;
        _ = legacyDevice.ColdResistanceHighLimitSetter( value );
        Assert.AreEqual( value, legacyDevice.InitialResistance.HighLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.InitialResistance )}.{nameof( LegacyDevice.InitialResistance.HighLimit )} should be set to the expected value." );
        Assert.AreEqual( value, legacyDevice.FinalResistance.HighLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.FinalResistance )}.{nameof( LegacyDevice.FinalResistance.HighLimit )} should be set to the expected value." );
        _ = legacyDevice.ColdResistanceHighLimitGetter();
        Assert.AreEqual( value, legacyDevice.InitialResistance.HighLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.InitialResistance )}.{nameof( LegacyDevice.InitialResistance.HighLimit )} should be gotten equal to the expected value." );
        Assert.AreEqual( value, legacyDevice.FinalResistance.HighLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.FinalResistance )}.{nameof( LegacyDevice.FinalResistance.HighLimit )} should be gotten equal to the expected value." );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

        value = 1.8f;
        legacyDevice.ColdResistanceConfig.LowLimit = value;
        Assert.AreEqual( value, legacyDevice.ColdResistanceConfig.LowLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceConfig )}.{nameof( LegacyDevice.ColdResistanceConfig.LowLimit )} should be assigned to the expected value." );
        value *= 0.9f;
        _ = legacyDevice.ColdResistanceLowLimitSetter( value );
        Assert.AreEqual( value, legacyDevice.InitialResistance.LowLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.InitialResistance )}.{nameof( LegacyDevice.InitialResistance.LowLimit )} should be set to the expected value." );
        Assert.AreEqual( value, legacyDevice.FinalResistance.LowLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.FinalResistance )}.{nameof( LegacyDevice.FinalResistance.LowLimit )} should be set to the expected value." );
        _ = legacyDevice.ColdResistanceLowLimitGetter();
        Assert.AreEqual( value, legacyDevice.InitialResistance.LowLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.InitialResistance )}.{nameof( LegacyDevice.InitialResistance.LowLimit )} should be gotten equal to the expected value." );
        Assert.AreEqual( value, legacyDevice.FinalResistance.LowLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.FinalResistance )}.{nameof( LegacyDevice.FinalResistance.LowLimit )} should be gotten equal to the expected value." );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

        value = 0.01f;
        legacyDevice.PostTransientDelayConfig = value;
        Assert.AreEqual( value, legacyDevice.PostTransientDelayConfig, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.PostTransientDelayConfig )} should be assigned to the expected value." );
        value *= 1.1f;
        _ = legacyDevice.PostTransientDelaySetter( value );
        Assert.AreEqual( value, legacyDevice.PostTransientDelay, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.PostTransientDelay )} should be set to the expected value." );
        _ = legacyDevice.PostTransientDelayGetter();
        Assert.AreEqual( value, legacyDevice.PostTransientDelay, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.PostTransientDelay )} should be gotten equal to the expected value." );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

        value = 0.27f;
        legacyDevice.ThermalTransientConfig.CurrentLevel = value;
        Assert.AreEqual( value, legacyDevice.ThermalTransientConfig.CurrentLevel, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientConfig )}.{nameof( LegacyDevice.ThermalTransientConfig.CurrentLevel )} should be assigned to the expected value." );
        value *= 1.1f;
        _ = legacyDevice.ThermalTransientCurrentLevelSetter( value );
        Assert.AreEqual( value, legacyDevice.ThermalTransientCurrentLevel, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientCurrentLevel )} should be set to the expected value." );
        _ = legacyDevice.ThermalTransientCurrentLevelGetter();
        Assert.AreEqual( value, legacyDevice.ThermalTransientCurrentLevel, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientCurrentLevel )} should be gotten equal to the expected value." );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

        value = 0.099f;
        legacyDevice.ThermalTransientConfig.AllowedVoltageChange = value;
        Assert.AreEqual( value, legacyDevice.ThermalTransientConfig.AllowedVoltageChange, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientConfig )}.{nameof( LegacyDevice.ThermalTransientConfig.AllowedVoltageChange )} should be assigned to the expected value." );
        value *= 0.9f;
        _ = legacyDevice.ThermalTransientVoltageChangeSetter( value );
        Assert.AreEqual( value, legacyDevice.ThermalTransientVoltageChange, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientVoltageChange )} should be set to the expected value." );
        _ = legacyDevice.ThermalTransientVoltageChangeGetter();
        Assert.AreEqual( value, legacyDevice.ThermalTransientVoltageChange, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientVoltageChange )} should be gotten equal to the expected value." );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

        value = 0.19f;
        legacyDevice.ThermalTransientConfig.HighLimit = value;
        Assert.AreEqual( value, legacyDevice.ThermalTransientConfig.HighLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientConfig )}.{nameof( LegacyDevice.ThermalTransientConfig.HighLimit )} should be assigned to the expected value." );
        value *= 1.1f;
        _ = legacyDevice.ThermalTransientHighLimitSetter( value );
        Assert.AreEqual( value, legacyDevice.ThermalTransient.HighLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransient )}.{nameof( LegacyDevice.ThermalTransient.HighLimit )} should be set to the expected value." );
        _ = legacyDevice.ThermalTransientHighLimitGetter();
        Assert.AreEqual( value, legacyDevice.ThermalTransient.HighLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransient )}.{nameof( LegacyDevice.ThermalTransient.HighLimit )} should be gotten equal to the expected value." );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );

        value = 0.006f;
        legacyDevice.ThermalTransientConfig.LowLimit = value;
        Assert.AreEqual( value, legacyDevice.ThermalTransientConfig.LowLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientConfig )}.{nameof( LegacyDevice.ThermalTransientConfig.LowLimit )} should be assigned to the expected value." );
        value *= 0.9f;
        _ = legacyDevice.ThermalTransientLowLimitSetter( value );
        Assert.AreEqual( value, legacyDevice.ThermalTransient.LowLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransient )}.{nameof( LegacyDevice.ThermalTransient.LowLimit )} should be set to the expected value." );
        _ = legacyDevice.ThermalTransientLowLimitGetter();
        Assert.AreEqual( value, legacyDevice.ThermalTransient.LowLimit, value * 0.001,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransient )}.{nameof( LegacyDevice.ThermalTransient.LowLimit )} should be gotten equal to the expected value." );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );
    }

    /// <summary>   Assert trigger cycle should start. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="legacyDevice">                         The legacy device. </param>
    /// <param name="timeout">                              The timeout. </param>
    /// <param name="validateTriggerCycleCompleteMessage">  True to validate trigger cycle complete
    ///                                                     message. </param>
    public static void AssertTriggerCycleShouldStart( LegacyDevice? legacyDevice, TimeSpan timeout, bool validateTriggerCycleCompleteMessage )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice!.Session, $"{nameof( legacyDevice )} should not be null." );
        Pith.SessionBase session = legacyDevice.Meter.TspDevice.Session;

        // start the trigger cycle
        _ = legacyDevice.PrepareForTrigger();

        Assert.IsTrue( legacyDevice.Meter.IsAwaitingTrigger, "awaiting trigger should be true." );

        //allow some time for the trigger loop to start..
        _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );

        session.AssertTrigger();

        bool messageAvailable = false;
        Stopwatch sw = Stopwatch.StartNew();
        while ( !messageAvailable && sw.Elapsed < timeout )
        {
            _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 10 ) );
            messageAvailable = legacyDevice.IsMeasurementCompleted();
        }

        Assert.IsTrue( messageAvailable, "The trigger wait should not timeout." );
        Assert.IsFalse( legacyDevice.Meter.IsAwaitingTrigger, "awaiting trigger should be false after trigger." );

        if ( validateTriggerCycleCompleteMessage )
        {
            string reading = session.ReadLineTrimEnd();
            Assert.IsFalse( string.IsNullOrEmpty( reading ), $"Triggered output should not be null or empty." );
            Assert.AreEqual( legacyDevice.MeasurementCompletedReply, reading, "The trigger cycle completion reply should match the expected value" );
        }

        sw.Stop();
        TimeSpan timeSpan = sw.Elapsed;
        Console.WriteLine( $"Trigger Cycle Time: {timeSpan:s\\.fff}s" );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );
    }

    /// <summary>   Output cold resistance. </summary>
    /// <remarks>   2025-01-24. </remarks>
    /// <param name="resistance">   The resistance. </param>
    private static void OutputColdResistance( IColdResistance resistance )
    {
        Console.WriteLine( $"{resistance.EntityName} values:" );
        Console.WriteLine( $"  Okay Outcome: {resistance.OkayReading}" );
        Console.WriteLine( $"        Status: {resistance.StatusReading}" );
        Console.WriteLine( $"       Outcome: {resistance.OutcomeReading}" );
        Console.WriteLine( $"       Reading: {resistance.Reading}" );
        Console.WriteLine( $"    Resistance: {resistance.Resistance}" );
    }

    /// <summary>   Output trace. </summary>
    /// <remarks>   2025-01-24. </remarks>
    /// <param name="trace">    The trace. </param>
    private static void OutputTrace( IThermalTransient trace )
    {
        Console.WriteLine( $"{trace.EntityName} values:" );
        Console.WriteLine( $"    Okay Outcome: {trace.OkayReading}" );
        Console.WriteLine( $"          Status: {trace.StatusReading}" );
        Console.WriteLine( $"         Outcome: {trace.OutcomeReading}" );
        Console.WriteLine( $"         Reading: {trace.Reading}" );
        Console.WriteLine( $"  Voltage Change: {trace.Voltage}" );
    }

    /// <summary>   Assert measurements should read. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="legacyDevice">                     The legacy device. </param>
    /// <param name="readTriggerCycleCompleteMessage">  True to read trigger cycle complete message. </param>
    public static void AssertMeasurementsShouldRead( LegacyDevice? legacyDevice, bool readTriggerCycleCompleteMessage )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice!.Session, $"{nameof( legacyDevice )} should not be null." );

        // the trigger cycle complete message was already read
        _ = legacyDevice.ReadMeasurements( readTriggerCycleCompleteMessage );

        if ( !string.IsNullOrWhiteSpace( legacyDevice.LastOrphanMessages ) )
            Console.WriteLine( "Orphan orphan messages:/n{this.LastOrphanMessages}" );
        Asserts.OutputColdResistance( legacyDevice.InitialResistance );
        Asserts.OutputColdResistance( legacyDevice.FinalResistance );
        Asserts.OutputTrace( legacyDevice.ThermalTransient );
    }

    /// <summary>   Assert cold resistance reading should validate. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="legacyDevice"> The legacy device. </param>
    /// <param name="resistance">   The resistance. </param>
    public static void AssertColdResistanceReadingShouldValidate( LegacyDevice? legacyDevice, IColdResistance resistance )
    {
        Assert.IsNotNull( resistance, $"{nameof( resistance )} should not be null." );
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice!.Session, $"{nameof( legacyDevice )} should not be null." );
        Pith.SessionBase session = legacyDevice.Meter.TspDevice.Session;

        if ( resistance.OutcomeReading == SessionBase.NilValue )
        {
            Assert.IsNull( resistance.Resistance, $"{nameof( ColdResistance.Resistance )} should be null if measurement was not made." );
        }
        else
        {
            Assert.IsTrue( resistance.OutcomeReading.TryParseNullableInteger( out int? outcomeValue ) );
            Assert.IsNotNull( outcomeValue, $"{nameof( ColdResistance.OutcomeReading )} should be not not be null if measurement was made." );

            Assert.IsNotNull( resistance.Resistance, $"{nameof( ColdResistance.Resistance )} should be not be null if measurement was made." );
            int? status = ( int? ) session.QueryNullableDoubleThrowIfError( $"print({resistance.EntityName}.status) ", "Cold Resistance status" );
            Assert.IsNotNull( status, $"{resistance.EntityName}.status should be not be null if measurement was made." );

            int? passFailOutcome = ( int? ) session.QueryNullableDoubleThrowIfError( $"print({resistance.EntityName}.passFailOutcome) ", "Cold Resistance pass fail outcome" );
            Assert.IsNotNull( passFailOutcome, $"{resistance.EntityName}.passFailOutcome should be not be null if measurement was made." );

            float resistanceValue = resistance.Resistance.Value;
            // if measurement was made
            float low = legacyDevice.ColdResistanceConfig.LowLimit;
            float high = legacyDevice.ColdResistanceConfig.HighLimit;
            if ( ( int ) PassFailBits.Unknown == passFailOutcome.Value )
            {
                // failed contact check.
                Assert.AreEqual( ( int ) FirmwareOutcomes.BadStatus, ( int ) FirmwareOutcomes.BadStatus & outcomeValue.Value,
                    $"Outcome value {outcomeValue.Value} bad status bit {FirmwareOutcomes.BadStatus} should be set if contact check failed." );
            }
            else if ( resistanceValue < low )
            {
                Assert.AreEqual( ( int ) PassFailBits.Low, passFailOutcome.Value,
                    $"Pass fail value {passFailOutcome.Value} must be 'low' if resistance {resistanceValue} is lower than {low}." );
            }
            else if ( resistanceValue > high )
            {
                Assert.AreEqual( ( int ) PassFailBits.High, passFailOutcome.Value,
                    $"Pass fail value {passFailOutcome.Value} must be 'high' if resistance {resistanceValue} is high than {high}." );
            }
            else
            {
                Assert.AreEqual( ( int ) PassFailBits.Pass, passFailOutcome.Value,
                    $"Pass fail value {passFailOutcome.Value} must be 'pass' if resistance {resistanceValue} is within [{low},{high}]." );
            }

            Assert.IsTrue( resistance.StatusReading.TryParseNullableInteger( out int? statusValue ) );
            Assert.IsNotNull( statusValue, $"status {statusValue} should not be null if measurement was made." );
            int bitValue;
            if ( outcomeValue.Value == 0 )
            {
                bitValue = 2;
                Assert.AreEqual( bitValue & statusValue, 0,
                    $"status {statusValue} must not have the over temp bit {bitValue} set if measurement was made." );
                // compliance is acceptable
                // bitValue = 64;
                // Assert.IsTrue( 0 == (bitValue & statusValue), $"status {statusValue} must not have the compliance bit {bitValue} set if measurement was made." );
            }
        }

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );
    }
    /// <summary>   Assert initial resistance reading should validate. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="legacyDevice"> The legacy device. </param>
    public static void AssertInitialResistanceReadingShouldValidate( LegacyDevice? legacyDevice )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice!.Session, $"{nameof( legacyDevice )} should not be null." );
        AssertColdResistanceReadingShouldValidate( legacyDevice, legacyDevice.InitialResistance );
    }

    /// <summary>   Assert trigger cycle should abort. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="legacyDevice"> The legacy device. </param>
    public static void AssertTriggerCycleShouldAbort( LegacyDevice? legacyDevice )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsNotNull( legacyDevice.Meter.TspDevice!.Session, $"{nameof( legacyDevice )} should not be null." );
        Pith.SessionBase session = legacyDevice.Meter.TspDevice.Session;

        // start the trigger cycle
        _ = legacyDevice.PrepareForTrigger();

        Assert.IsTrue( legacyDevice.Meter.IsAwaitingTrigger, "awaiting trigger should be true." );

        //allow some time for the trigger loop to start..
        _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );

        _ = legacyDevice.AbortMeasurement( 1.0 );

        Assert.IsFalse( legacyDevice.Meter.IsAwaitingTrigger, "awaiting trigger should be false after trigger." );

        Asserts.AssertOrphanMessagesOrDeviceErrors( session );
    }
}
