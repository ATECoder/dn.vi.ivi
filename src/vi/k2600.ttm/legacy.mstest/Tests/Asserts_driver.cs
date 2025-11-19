using System.Diagnostics;
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

        // initialize meter state: this reads the meter model number.
        meter.InitKnownState();

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
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
        string ttmObjectName = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ThermalTransient.ThermalTransientBaseEntityName;
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        ttmObjectName = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ThermalTransient.OutcomeBitsName;
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        ttmObjectName = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ThermalTransient.OutcomeBitsOkayName;
        Assert.IsFalse( session.IsNil( ttmObjectName ), $"{ttmObjectName} should not be nil." );

        // preset the meter configuration
        meter.PresetKnownState();

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

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

    /// <summary>   Current source current level or voltage source current limit getter. </summary>
    /// <remarks>   2025-02-13. </remarks>
    /// <param name="legacyDevice">         The legacy device. </param>
    /// <param name="sourceOutputOption">   The current source output option. </param>
    /// <returns>   A double. </returns>
    private static double CurrentGetter( LegacyDevice? legacyDevice, SourceOutputOption sourceOutputOption )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsTrue( legacyDevice.IsConnected, "The driver should be connected if the session opened." );
        string modality = (Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption)
            ? "current source current level"
            : "voltage source current limit";
        double current;
        bool success;
        // The legacy driver level setter sets a current value.
        // This is the current level of the current source or the current limit of a voltage source.
        if ( Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption )
        {
            // With the legacy firmware or with the current source, the level setter sets the current level (ir.level and fr.level) of the source.
            // The legacy driver current level getter reads these levels (ir.level and fr.level) thus reading
            // the current level that was set by the level setter.
            success = legacyDevice.ColdResistanceCurrentLevelGetter();
            Assert.IsTrue( success, $"The {modality} getter should succeed." );
            current = legacyDevice.ColdResistanceCurrentLevel;
        }
        else
        {
            // With the new firmware and voltage source, the level setter sets the current limit (ir.limit and fr.limit) of the source.
            // The legacy driver voltage limit getter reads these limits (ir.limit and fr.limit) thus reading
            // current that was set by the level setter.
            success = legacyDevice.ColdResistanceVoltageLimitGetter();
            Assert.IsTrue( success, $"The {modality} getter should succeed." );
            current = legacyDevice.ColdResistanceVoltageLimit;
        }
        Assert.IsGreaterThan( 0, current, $"The {modality} must be positive." );
        return current;
    }

    /// <summary>   Current source voltage limit or voltage source voltage level getter. </summary>
    /// <remarks>   2025-02-13. </remarks>
    /// <param name="legacyDevice">         The legacy device. </param>
    /// <param name="sourceOutputOption">   The current source output option. </param>
    /// <returns>   A double. </returns>
    private static double VoltageGetter( LegacyDevice? legacyDevice, SourceOutputOption sourceOutputOption )
    {
        Assert.IsNotNull( legacyDevice, $"{nameof( legacyDevice )} should not be null." );
        Assert.IsTrue( legacyDevice.IsConnected, "The driver should be connected if the session opened." );
        double voltage;
        // The legacy driver limit setter sets a voltage value.
        // This is the voltage limit of the current source or the voltage level of a voltage source.
        string modality = (Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption)
            ? "current source voltage limit"
            : "voltage source voltage level";
        if ( Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption )
        {
            // With the legacy firmware or with the current source, the limit setter sets the voltage limits (ir.limit and fr.limit) of the source.
            // The legacy driver voltage limit getter reads these limits (ir.limit and fr.limit) thus reading
            // the voltage limit that was set by the limit setter.
            bool success = legacyDevice.ColdResistanceVoltageLimitGetter();
            Assert.IsTrue( success, $"The {modality} getter should succeed." );
            voltage = legacyDevice.ColdResistanceVoltageLimit;
        }
        else
        {
            // With the new firmware and voltage source, the limit setter sets the voltage level (ir.level and fr.level) of the source.
            // The legacy driver current level getter reads these levels (ir.level and fr.level) thus reading
            // voltage that was set by the limit setter.
            bool success = legacyDevice.ColdResistanceCurrentLevelGetter();
            Assert.IsTrue( success, $"The {modality} getter should succeed." );
            voltage = legacyDevice.ColdResistanceCurrentLevel;
        }
        Assert.IsGreaterThan( 0, voltage, $"The {modality} must be positive." );
        return voltage;
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

        // the following values are set to allow testing for both voltage and current source to make sure
        // the the preset values do not become out of range when changing the source output option.
        double maxValue = Ttm.Properties.Settings.Instance.TtmResistanceSettings.VoltageMaximum;
        maxValue = Math.Min( maxValue, Ttm.Properties.Settings.Instance.TtmResistanceSettings.CurrentMaximum );

        double expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings.CurrentLevelDefault;
        double actualValue = legacyDevice.ColdResistanceConfig.CurrentLevel;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceConfig )}.{nameof( LegacyDevice.ColdResistanceConfig.CurrentLevel )} should equal the settings value." );

        // get actual current source current level or voltage source current limit
        double actualCurrent = Asserts.CurrentGetter( legacyDevice, sourceOutputOption );

        // ensure that the value is below the internal maximum limit.
        actualCurrent = Math.Min( maxValue, actualCurrent );

        string modality = (Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption)
            ? "current source current level"
            : "voltage source current limit";

        // alter the actual value
        expectedValue = 0.9 * actualCurrent;
        bool success = legacyDevice.ColdResistanceCurrentLevelSetter( ( float ) expectedValue );
        Assert.IsTrue( success, $"The {modality} setter should succeed." );
        actualValue = Asserts.CurrentGetter( legacyDevice, sourceOutputOption );
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue, $"{modality} should equal the changed value." );

        // restore the value
        expectedValue = actualCurrent;
        success = legacyDevice.ColdResistanceCurrentLevelSetter( ( float ) expectedValue );
        Assert.IsTrue( success, $"The {modality} setter should succeed." );
        actualValue = Asserts.CurrentGetter( legacyDevice, sourceOutputOption );
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue, $"{modality} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings.VoltageLimitDefault;
        actualValue = legacyDevice.ColdResistanceConfig.VoltageLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceConfig )}.{nameof( LegacyDevice.ColdResistanceConfig.VoltageLimit )} should equal the settings value." );

        modality = (Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption)
            ? "current source voltage limit"
            : "voltage source voltage level";

        // get actual value
        double actualVoltage = Asserts.VoltageGetter( legacyDevice, sourceOutputOption );

        // ensure that the value is below the internal maximum limit.
        actualVoltage = Math.Min( maxValue, actualVoltage );

        // alter the actual value
        expectedValue = 0.9 * actualVoltage;
        success = legacyDevice.ColdResistanceVoltageLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, $"The {modality} setter should succeed." );
        actualValue = Asserts.VoltageGetter( legacyDevice, sourceOutputOption );
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue, $"{modality} should equal the changed value." );

        // restore the value
        expectedValue = actualVoltage;
        success = legacyDevice.ColdResistanceVoltageLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, $"The {modality} setter should succeed." );
        actualValue = Asserts.VoltageGetter( legacyDevice, sourceOutputOption );
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue, $"{modality} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
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

        double expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings.HighLimitDefault;
        double actualValue = legacyDevice.ColdResistanceConfig.HighLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceConfig )}.{nameof( LegacyDevice.ColdResistanceConfig.HighLimit )} should equal the settings value." );

        // get actual value
        bool success = legacyDevice.ColdResistanceHighLimitGetter();
        Assert.IsTrue( success, "The cold resistance high limit getter should succeed." );
        double actualHighLimit = legacyDevice.ColdResistanceHighLimit;
        Assert.IsGreaterThan( 0, actualHighLimit, $"{nameof( actualHighLimit )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualHighLimit;
        success = legacyDevice.ColdResistanceHighLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The cold resistance high limit setter should succeed." );
        success = legacyDevice.ColdResistanceHighLimitGetter();
        Assert.IsTrue( success, "The cold resistance high limit getter should succeed." );
        actualValue = legacyDevice.ColdResistanceHighLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceHighLimit )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualHighLimit;
        success = legacyDevice.ColdResistanceHighLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The cold resistance high limit setter should succeed." );
        success = legacyDevice.ColdResistanceHighLimitGetter();
        Assert.IsTrue( success, "The cold resistance high limit getter should succeed." );
        actualValue = legacyDevice.ColdResistanceHighLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceHighLimit )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings.LowLimitDefault;
        actualValue = legacyDevice.ColdResistanceConfig.LowLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceConfig )}.{nameof( LegacyDevice.ColdResistanceConfig.LowLimit )} should equal the settings value." );

        // get actual value
        success = legacyDevice.ColdResistanceLowLimitGetter();
        Assert.IsTrue( success, "The cold resistance low limit getter should succeed." );
        double actualLowLimit = legacyDevice.ColdResistanceLowLimit;
        Assert.IsGreaterThan( 0, actualLowLimit, $"{nameof( actualLowLimit )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualLowLimit;
        success = legacyDevice.ColdResistanceLowLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The cold resistance low limit setter should succeed." );
        success = legacyDevice.ColdResistanceLowLimitGetter();
        Assert.IsTrue( success, "The cold resistance low limit getter should succeed." );
        actualValue = legacyDevice.ColdResistanceLowLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceLowLimit )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualLowLimit;
        success = legacyDevice.ColdResistanceLowLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The cold resistance low limit setter should succeed." );
        success = legacyDevice.ColdResistanceLowLimitGetter();
        Assert.IsTrue( success, "The cold resistance low limit getter should succeed." );
        actualValue = legacyDevice.ColdResistanceLowLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ColdResistanceLowLimit )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayDefault;
        actualValue = legacyDevice.PostTransientDelayConfig;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.PostTransientDelay )} should equal the settings value." );

        // get actual value
        success = legacyDevice.PostTransientDelayGetter();
        Assert.IsTrue( success, "The Post Transient Delay getter should succeed." );
        double actualPostTransientDelay = legacyDevice.PostTransientDelay;
        Assert.IsGreaterThan( 0, actualPostTransientDelay, $"{nameof( actualPostTransientDelay )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualPostTransientDelay;
        success = legacyDevice.PostTransientDelaySetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The Post Transient Delay setter should succeed." );
        success = legacyDevice.PostTransientDelayGetter();
        Assert.IsTrue( success, "The Post Transient Delay getter should succeed." );
        actualValue = legacyDevice.PostTransientDelay;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.PostTransientDelay )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualPostTransientDelay;
        success = legacyDevice.PostTransientDelaySetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The Post Transient Delay setter should succeed." );
        success = legacyDevice.PostTransientDelayGetter();
        Assert.IsTrue( success, "The Post Transient Delay getter should succeed." );
        actualValue = legacyDevice.PostTransientDelay;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.PostTransientDelay )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings.CurrentLevelDefault;
        actualValue = legacyDevice.ThermalTransientConfig.CurrentLevel;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientConfig )}.{nameof( LegacyDevice.ThermalTransientConfig.CurrentLevel )} should equal the settings value." );

        // get actual value
        success = legacyDevice.ThermalTransientCurrentLevelGetter();
        Assert.IsTrue( success, "The thermal transient current level getter should succeed." );
        double actualCurrentLevel = legacyDevice.ThermalTransientCurrentLevel;
        Assert.IsGreaterThan( 0, actualCurrentLevel, $"{nameof( actualCurrentLevel )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualCurrentLevel;
        success = legacyDevice.ThermalTransientCurrentLevelSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The thermal transient current level setter should succeed." );
        success = legacyDevice.ThermalTransientCurrentLevelGetter();
        Assert.IsTrue( success, "The thermal transient current level getter should succeed." );
        actualValue = legacyDevice.ThermalTransientCurrentLevel;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientCurrentLevel )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualCurrentLevel;
        success = legacyDevice.ThermalTransientCurrentLevelSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The thermal transient current level setter should succeed." );
        success = legacyDevice.ThermalTransientCurrentLevelGetter();
        Assert.IsTrue( success, "The thermal transient current level getter should succeed." );
        actualValue = legacyDevice.ThermalTransientCurrentLevel;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientCurrentLevel )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings.VoltageChangeDefault;
        actualValue = legacyDevice.ThermalTransientConfig.AllowedVoltageChange;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientConfig )}.{nameof( LegacyDevice.ThermalTransientConfig.AllowedVoltageChange )} should equal the settings value." );

        // get actual value
        success = legacyDevice.ThermalTransientVoltageChangeGetter();
        Assert.IsTrue( success, "The thermal transient voltage change getter should succeed." );
        double actualAllowedVoltageChange = legacyDevice.ThermalTransientVoltageChange;
        Assert.IsGreaterThan( 0, actualAllowedVoltageChange, $"{nameof( actualAllowedVoltageChange )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualAllowedVoltageChange;
        success = legacyDevice.ThermalTransientVoltageChangeSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The thermal transient voltage change setter should succeed." );
        success = legacyDevice.ThermalTransientVoltageChangeGetter();
        Assert.IsTrue( success, "The thermal transient voltage change getter should succeed." );
        actualValue = legacyDevice.ThermalTransientVoltageChange;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientVoltageChange )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualAllowedVoltageChange;
        success = legacyDevice.ThermalTransientVoltageChangeSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The thermal transient voltage change setter should succeed." );
        success = legacyDevice.ThermalTransientVoltageChangeGetter();
        Assert.IsTrue( success, "The thermal transient voltage change getter should succeed." );
        actualValue = legacyDevice.ThermalTransientVoltageChange;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientVoltageChange )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings.HighLimitDefault;
        actualValue = legacyDevice.ThermalTransientConfig.HighLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientConfig )}.{nameof( LegacyDevice.ThermalTransientConfig.HighLimit )} should equal the settings value." );

        // get actual value
        success = legacyDevice.ThermalTransientHighLimitGetter();
        Assert.IsTrue( success, "The cold resistance High limit getter should succeed." );
        actualHighLimit = legacyDevice.ThermalTransientHighLimit;
        Assert.IsGreaterThan( 0, actualHighLimit, $"{nameof( actualHighLimit )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualHighLimit;
        success = legacyDevice.ThermalTransientHighLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The cold resistance High limit setter should succeed." );
        success = legacyDevice.ThermalTransientHighLimitGetter();
        Assert.IsTrue( success, "The cold resistance High limit getter should succeed." );
        actualValue = legacyDevice.ThermalTransientHighLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientHighLimit )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualHighLimit;
        success = legacyDevice.ThermalTransientHighLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The cold resistance High limit setter should succeed." );
        success = legacyDevice.ThermalTransientHighLimitGetter();
        Assert.IsTrue( success, "The cold resistance High limit getter should succeed." );
        actualValue = legacyDevice.ThermalTransientHighLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.001 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientHighLimit )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings.LowLimitDefault;
        actualValue = legacyDevice.ThermalTransientConfig.LowLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientConfig )}.{nameof( LegacyDevice.ThermalTransientConfig.LowLimit )} should equal the settings value." );

        // get actual value
        success = legacyDevice.ThermalTransientLowLimitGetter();
        Assert.IsTrue( success, "The cold resistance low limit getter should succeed." );
        actualLowLimit = legacyDevice.ThermalTransientLowLimit;
        Assert.IsGreaterThan( 0, actualLowLimit, $"{nameof( actualLowLimit )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualLowLimit;
        success = legacyDevice.ThermalTransientLowLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The cold resistance low limit setter should succeed." );
        success = legacyDevice.ThermalTransientLowLimitGetter();
        Assert.IsTrue( success, "The cold resistance low limit getter should succeed." );
        actualValue = legacyDevice.ThermalTransientLowLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientLowLimit )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualLowLimit;
        success = legacyDevice.ThermalTransientLowLimitSetter( ( float ) expectedValue );
        Assert.IsTrue( success, "The cold resistance low limit setter should succeed." );
        success = legacyDevice.ThermalTransientLowLimitGetter();
        Assert.IsTrue( success, "The cold resistance low limit getter should succeed." );
        actualValue = legacyDevice.ThermalTransientLowLimit;
        Assert.AreEqual( expectedValue, actualValue, 0.01 * expectedValue,
            $"{nameof( LegacyDevice )}.{nameof( LegacyDevice.ThermalTransientLowLimit )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
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

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
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
            Console.WriteLine( "Orphan orphan messages:\r\n\t{this.LastOrphanMessages}" );
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
            resistance.ReadLimits( session );
            Assert.IsGreaterThan( 0, resistance.LowLimit, $"{nameof( ColdResistance.LowLimit )} should be positive." );
            Assert.IsGreaterThan( 0, resistance.HighLimit, $"{nameof( ColdResistance.HighLimit )} should be positive." );

            Assert.IsTrue( resistance.OutcomeReading.TryParseNullableInteger( out int? outcomeValue ) );
            Assert.IsNotNull( outcomeValue, $"{nameof( ColdResistance.OutcomeReading )} should be not not be null if measurement was made." );

            int? status = session.QueryNullableIntegerThrowIfError( $"print({resistance.EntityName}.status) ", "Cold Resistance status" );
            Assert.IsNotNull( status, $"{resistance.EntityName}.status should be not be null if measurement was made." );

            Assert.IsTrue( resistance.StatusReading.TryParseNullableInteger( out int? statusValue ) );
            Assert.IsNotNull( statusValue, $"status {statusValue} should not be null if measurement was made." );
            int bitValue;
            if ( outcomeValue == ( int ) FirmwareOutcomes.Okay )
            {
                bitValue = ( int ) cc.isr.VI.BufferElementStatusBits.OverTemp;
                Assert.AreEqual( 0, bitValue & statusValue,
                    $"status {statusValue} must not have the over-temp bit {bitValue} set if measurement was made." );
                // compliance is acceptable
                // bitValue = 64;
                // Assert.IsTrue( 0 == (bitValue & statusValue), $"status {statusValue} must not have the compliance bit {bitValue} set if measurement was made." );
            }

            if ( MeterSubsystem.LegacyFirmware )
            {
                if ( outcomeValue == ( int ) FirmwareOutcomes.Okay )
                {
                    Assert.IsNotNull( resistance.Resistance, $"{nameof( ColdResistance.Resistance )} should be not be null if measurement was made." );

                    float resistanceValue = resistance.Resistance.Value;
                    // if measurement was made
                    float low = resistance.LowLimit;
                    float high = resistance.HighLimit;
                    bool? passed = session.QueryNullableBoolThrowIfError( $"print({resistance.EntityName}.pass) ", "Cold Resistance pass value" );
                    Assert.IsNotNull( passed, $"Cold Resistance {nameof( passed )} should be not be null if measurement was made." );

                    if ( resistanceValue < low )
                    {
                        Assert.IsFalse( passed.Value,
                             $"Cold Resistance {nameof( passed )} should be false if resistance {resistanceValue:#0.###} is lower than {low:#0.###}." );
                    }
                    else if ( resistanceValue > high )
                    {
                        Assert.IsFalse( passed.Value,
                             $"Cold Resistance {nameof( passed )} should be false if resistance {resistanceValue:#0.###} is higher than {high:#0.###}." );
                    }
                    else
                    {
                        Assert.IsTrue( passed.Value,
                             $"Cold Resistance {nameof( passed )} should be true if resistance {resistanceValue:#0.###} is within[{low:#0.###}, {high:#0.###}]." );
                    }
                }
                else if ( outcomeValue == ( int ) FirmwareOutcomes.BadStatus )
                {
                    Assert.IsNull( resistance.Resistance, $"{nameof( ColdResistance.Resistance )} should be null if measurement failed." );

                    bitValue = ( int ) cc.isr.VI.BufferElementStatusBits.Compliance;
                    Assert.AreEqual( bitValue & statusValue, bitValue,
                        $"status {statusValue} must report compliance if measurement failed with bad status." );
                }
            }
            else
            {
                Assert.IsNotNull( resistance.Resistance, $"{nameof( ColdResistance.Resistance )} should be not be null if measurement was made." );

                int? passFailOutcome = session.QueryNullableIntegerThrowIfError( $"print({resistance.EntityName}.passFailOutcome) ", "Cold Resistance pass fail outcome" );
                Assert.IsNotNull( passFailOutcome, $"{resistance.EntityName}.passFailOutcome should be not be null if measurement was made." );

                float resistanceValue = resistance.Resistance.Value;
                // if measurement was made
                float low = resistance.LowLimit;
                float high = resistance.HighLimit;
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

            }
        }

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
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

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }
}
