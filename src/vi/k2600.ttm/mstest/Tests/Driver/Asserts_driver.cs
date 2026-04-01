using System.Diagnostics;
using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;
using cc.isr.VI.Tsp.ParseStringExtensions;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Driver;

internal static partial class Asserts
{
    /// <summary>   Assert meter should initialize known state. </summary>
    /// <remarks>   2024-11-15. </remarks>
    /// <param name="meter"> The legacy device. </param>
    public static void AssertMeterShouldInitializeKnowState( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsTrue( meter.IsSessionOpen, "The meter should be connected if the session opened." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Pith.SessionBase session = meter.TspDevice.Session;

        // initialize meter state: this reads the meter model number.
        meter.InitKnownState();

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary>   Assert meter should preset. </summary>
    /// <remarks>   2024-11-15. </remarks>
    /// <param name="meter"> The legacy device. </param>
    public static void AssertMeterShouldPreset( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsTrue( meter.IsSessionOpen, "The meter should be connected if the session opened." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Pith.SessionBase session = meter.TspDevice.Session;

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
    /// <remarks>   2026-03-27. </remarks>
    /// <param name="coldResistanceMeasure">    The cold resistance measure. </param>
    /// <param name="sourceOutputOption">       The current source output option. </param>
    /// <returns>   A double. </returns>
    private static double? CurrentGetter( ColdResistanceMeasure? coldResistanceMeasure, SourceOutputOption sourceOutputOption )
    {
        Assert.IsNotNull( coldResistanceMeasure, $"{nameof( coldResistanceMeasure )} should not be null." );
        Assert.IsNotNull( coldResistanceMeasure.Session, $"{nameof( coldResistanceMeasure.Session )} should not be null." );
        Assert.IsTrue( coldResistanceMeasure.Session.IsDeviceOpen,
            $"The {nameof( coldResistanceMeasure.Session )} should be connected if the session opened." );
        string modality = (Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption)
            ? "Current source current level"
            : "Voltage source current Limit";
        double? value;
        Assert.IsNotNull( coldResistanceMeasure, $"The {coldResistanceMeasure} should not be null." );

        // The legacy driver level setter sets a current value.
        // This is the current level of the current source or the current limit of a voltage source.
        if ( Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption )
        {
            // With the legacy firmware or with the current source, the level setter sets the current level (ir.level and fr.level) of the source.
            // The legacy driver current level getter reads these levels (ir.level and fr.level) thus reading
            // the current level that was set by the level setter.
            value = coldResistanceMeasure.QueryCurrentLevel();
        }
        else
        {
            // With the new firmware and voltage source, the level setter sets the current limit (ir.limit and fr.limit) of the source.
            // The legacy driver voltage limit getter reads these limits (ir.limit and fr.limit) thus reading
            // current that was set by the level setter.
            value = coldResistanceMeasure.QueryVoltageLimit();
        }
        Assert.IsNotNull( value, $"The {modality} should not be null." );
        Assert.IsPositive<double>( value.Value, $"The {modality} must be positive." );
        return value;
    }

    /// <summary>   Current source current level or voltage source current limit getter. </summary>
    /// <remarks>   2025-02-13. </remarks>
    /// <param name="meter">         The TTM top level device driver. </param>
    /// <param name="sourceOutputOption">   The current source output option. </param>
    /// <returns>   A double. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private static double? CurrentGetter( Meter? meter, SourceOutputOption sourceOutputOption )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        double? value1 = CurrentGetter( meter.InitialResistance, sourceOutputOption );
        double? value2 = CurrentGetter( meter.FinalResistance, sourceOutputOption );
        string modality = (Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption)
            ? "Current source current level"
            : "Voltage source current Limit";
        Assert.AreEqual( value1.GetValueOrDefault( 0 ), value2.GetValueOrDefault( 0 ), 0.001 * value1.GetValueOrDefault( 0 ),
            $"The {modality} read from initial and final resistance meter elements should be approximately equal." );
        return value1;
    }

    /// <summary>   Current source voltage limit or voltage source voltage level getter. </summary>
    /// <remarks>   2025-02-13. </remarks>
    /// <param name="coldResistanceMeasure">    The cold resistance measure. </param>
    /// <param name="sourceOutputOption">       The current source output option. </param>
    /// <returns>   A double?. </returns>
    private static double? VoltageGetter( ColdResistanceMeasure? coldResistanceMeasure, SourceOutputOption sourceOutputOption )
    {
        Assert.IsNotNull( coldResistanceMeasure, $"{nameof( coldResistanceMeasure )} should not be null." );
        Assert.IsNotNull( coldResistanceMeasure.Session, $"{nameof( coldResistanceMeasure.Session )} should not be null." );
        Assert.IsTrue( coldResistanceMeasure.Session.IsDeviceOpen,
            $"The {nameof( coldResistanceMeasure.Session )} should be connected if the session opened." );

        // The legacy driver limit setter sets a voltage value.
        // This is the voltage limit of the current source or the voltage level of a voltage source.
        string modality = (Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption)
            ? "Current source voltage limit"
            : "Voltage source voltage Level";

        Assert.IsNotNull( coldResistanceMeasure, $"The {coldResistanceMeasure} should not be null." );
        double? value;

        if ( Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption )
        {
            // With the legacy firmware or with the current source, the limit setter sets the voltage limits (ir.limit and fr.limit) of the source.
            // The legacy driver voltage limit getter reads these limits (ir.limit and fr.limit) thus reading
            // the voltage limit that was set by the limit setter.
            value = coldResistanceMeasure.QueryVoltageLimit();
        }
        else
        {
            // With the new firmware and voltage source, the limit setter sets the voltage level (ir.level and fr.level) of the source.
            // The legacy driver current level getter reads these levels (ir.level and fr.level) thus reading
            // voltage that was set by the limit setter.
            value = coldResistanceMeasure.QueryCurrentLevel();
        }
        Assert.IsNotNull( value, $"The {modality} should not be null." );
        Assert.IsPositive<double>( value.Value, $"The {modality} must be positive." );
        return value;
    }

    /// <summary>   Current source voltage limit or voltage source voltage level getter. </summary>
    /// <remarks>   2026-03-30. </remarks>
    /// <param name="meter">                The TTM top level device driver. </param>
    /// <param name="sourceOutputOption">   The current source output option. </param>
    /// <returns>   A double?. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private static double? VoltageGetter( Meter? meter, SourceOutputOption sourceOutputOption )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        double? value1 = VoltageGetter( meter.InitialResistance, sourceOutputOption );
        double? value2 = VoltageGetter( meter.FinalResistance, sourceOutputOption );
        string modality = (Ttm.MeterSubsystem.LegacyFirmware || SourceOutputOption.Current == sourceOutputOption)
            ? "Current source current level"
            : "Voltage source current Limit";
        Assert.AreEqual( value1.GetValueOrDefault( 0 ), value2.GetValueOrDefault( 0 ), 0.001 * value1.GetValueOrDefault( 0 ),
            $"The {modality} read from initial and final resistance meter elements should be approximately equal." );
        return value1;
    }

    /// <summary>   Assert source should configure. </summary>
    /// <remarks>   2026-03-27. </remarks>
    /// <param name="coldResistanceMeasure">    The cold resistance measure. </param>
    /// <param name="coldResistance">           The cold resistance. </param>
    /// <param name="entityName">               Name of the entity. </param>
    public static void AssertSourceShouldConfigure( ColdResistanceMeasure? coldResistanceMeasure, ColdResistance? coldResistance )
    {
        Assert.IsNotNull( coldResistance, $"{nameof( coldResistance )} should not be null." );

        Assert.IsNotNull( coldResistanceMeasure, $"{nameof( coldResistanceMeasure )} should not be null." );
        Assert.IsNotNull( coldResistanceMeasure.Session, $"{nameof( coldResistanceMeasure.Session )} should not be null." );
        Assert.IsTrue( coldResistanceMeasure.Session.IsDeviceOpen,
            $"The {nameof( coldResistanceMeasure.Session )} should be connected if the session opened." );

        Pith.SessionBase session = coldResistanceMeasure.Session;

        double? expectedValue;
        double? actualValue;
        double? appliedValue;
        string sourceName;
        string modality;
        double maxValue;

        sourceName = "Current Source";
        modality = "Current Level";
        maxValue = Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentMaximum;

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentLevel;
        actualValue = coldResistance.CurrentLevel;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{modality} should equal the settings value." );

        // get actual current source current level
        actualValue = coldResistanceMeasure.QueryCurrentLevel();

        Assert.IsNotNull( actualValue, $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.IsPositive<double>( actualValue.Value, $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} must be positive." );
        Assert.IsLessThan<double>( actualValue.Value, maxValue,
            $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} should be less than the maximum {modality} settings value." );

        // alter the actual value
        expectedValue = 0.9 * actualValue.Value;
        _ = coldResistanceMeasure.WriteCurrentLevel( ( float ) expectedValue );
        appliedValue = coldResistanceMeasure.QueryCurrentLevel();
        Assert.IsNotNull( appliedValue, $"The applied {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value, $"{modality} should equal the changed value." );

        // restore the value
        expectedValue = actualValue.Value;
        _ = coldResistanceMeasure.WriteCurrentLevel( ( float ) expectedValue );
        appliedValue = coldResistanceMeasure.QueryCurrentLevel();
        Assert.IsNotNull( appliedValue, $"The applied {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value, $"{modality} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        sourceName = "Voltage Source";
        modality = "Voltage Level";
        maxValue = Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageMaximum;

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageLevel;
        actualValue = coldResistance.VoltageLevel;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{modality} should equal the settings value." );

        // get actual current source current level
        actualValue = coldResistanceMeasure.QueryVoltageLevel();

        Assert.IsNotNull( actualValue, $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.IsPositive<double>( actualValue.Value, $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} must be positive." );
        Assert.IsLessThan<double>( actualValue.Value, maxValue,
            $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} should be less than the maximum {modality} settings value." );

        // alter the actual value
        expectedValue = 0.9 * actualValue.Value;
        _ = coldResistanceMeasure.WriteVoltageLevel( ( float ) expectedValue );
        appliedValue = coldResistanceMeasure.QueryVoltageLevel();
        Assert.IsNotNull( appliedValue, $"The applied {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value, $"{modality} should equal the changed value." );

        // restore the value
        expectedValue = actualValue.Value;
        _ = coldResistanceMeasure.WriteVoltageLevel( ( float ) expectedValue );
        appliedValue = coldResistanceMeasure.QueryVoltageLevel();
        Assert.IsNotNull( appliedValue, $"The applied {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value, $"{modality} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        sourceName = "Current Source";
        modality = "Voltage Limit";
        maxValue = Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageMaximum;

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageLimit;
        actualValue = coldResistance.VoltageLimit;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{modality} should equal the settings value." );

        // get actual current source voltage limit
        actualValue = coldResistanceMeasure.QueryVoltageLimit();

        Assert.IsNotNull( actualValue, $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.IsPositive<double>( actualValue.Value, $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} must be positive." );
        Assert.IsLessThan<double>( actualValue.Value, maxValue,
            $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} should be less than the maximum {modality} settings value." );

        // alter the actual value
        expectedValue = 0.9 * actualValue.Value;
        _ = coldResistanceMeasure.WriteVoltageLimit( ( float ) expectedValue );
        appliedValue = coldResistanceMeasure.QueryVoltageLimit();
        Assert.IsNotNull( appliedValue, $"The applied {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value, $"{modality} should equal the changed value." );

        // restore the value
        expectedValue = actualValue.Value;
        _ = coldResistanceMeasure.WriteVoltageLimit( ( float ) expectedValue );
        appliedValue = coldResistanceMeasure.QueryVoltageLimit();
        Assert.IsNotNull( appliedValue, $"The applied {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value, $"{modality} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        sourceName = "Voltage Source";
        modality = "Current Limit";
        maxValue = Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentMaximum;

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentLimit;
        actualValue = coldResistance.CurrentLimit;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{modality} should equal the settings value." );

        // get actual current source voltage limit
        actualValue = coldResistanceMeasure.QueryCurrentLimit();

        Assert.IsNotNull( actualValue, $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.IsPositive<double>( actualValue.Value, $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} must be positive." );
        Assert.IsLessThan<double>( actualValue.Value, maxValue,
            $"The {coldResistanceMeasure.EntityName} {sourceName} {modality} should be less than the maximum {modality} settings value." );

        // alter the actual value
        expectedValue = 0.9 * actualValue.Value;
        _ = coldResistanceMeasure.WriteCurrentLimit( ( float ) expectedValue );
        appliedValue = coldResistanceMeasure.QueryCurrentLimit();
        Assert.IsNotNull( appliedValue, $"The applied {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value, $"{modality} should equal the changed value." );

        // restore the value
        expectedValue = actualValue.Value;
        _ = coldResistanceMeasure.WriteCurrentLimit( ( float ) expectedValue );
        appliedValue = coldResistanceMeasure.QueryCurrentLimit();
        Assert.IsNotNull( appliedValue, $"The applied {coldResistanceMeasure.EntityName} {sourceName} {modality} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value, $"{modality} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

    }

    /// <summary>   Assert source should configure. </summary>
    /// <remarks>   2024-11-27. </remarks>
    /// <param name="meter">         The TTM top level device driver. </param>
    public static void AssertSourceShouldConfigure( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsTrue( meter.IsSessionOpen, "The meter should be connected if the session opened." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );

        Asserts.AssertSourceShouldConfigure( meter.InitialResistance, meter.ConfigInfo.InitialResistance );
        Asserts.AssertSourceShouldConfigure( meter.FinalResistance, meter.ConfigInfo.FinalResistance );
    }

    public static void AssertMeasurementsShouldConfigure( MeterSubsystem? meterSubsystem, MeterMain? meterMain )
    {
        Assert.IsNotNull( meterMain, $"{nameof( meterMain )} should not be null." );
        Assert.IsNotNull( meterSubsystem, $"{nameof( meterSubsystem )} should not be null." );
        Assert.IsNotNull( meterSubsystem.Session, $"{nameof( meterSubsystem.Session )} should not be null." );
        Assert.IsTrue( meterSubsystem.Session.IsDeviceOpen,
            $"The {nameof( meterSubsystem.Session )} should be connected if the session opened." );

        Pith.SessionBase session = meterSubsystem.Session;

        double? expectedValue;
        double? actualValue;
        double? appliedValue;

        // Post transient delay

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.MeterDefaults.PostTransientDelay;
        actualValue = meterMain.PostTransientDelay;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{meterSubsystem.EntityName} {nameof( meterSubsystem.PostTransientDelay )} should equal the settings value." );

        // get actual value
        actualValue = meterSubsystem.QueryPostTransientDelay();
        Assert.IsNotNull( actualValue, $"The {meterSubsystem.EntityName} {nameof( MeterMain.PostTransientDelay )} should not be null." );
        Assert.IsGreaterThan( 0, actualValue.Value, $"The {meterSubsystem.EntityName} {nameof( MeterMain.PostTransientDelay )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualValue;
        appliedValue = meterSubsystem.ApplyPostTransientDelay( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {meterSubsystem.EntityName} applied {nameof( MeterMain.PostTransientDelay )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {meterSubsystem.EntityName} applied {nameof( MeterMain.PostTransientDelay )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualValue;
        appliedValue = meterSubsystem.ApplyPostTransientDelay( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {meterSubsystem.EntityName} restored {nameof( MeterMain.PostTransientDelay )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {meterSubsystem.EntityName} applied {nameof( MeterMain.PostTransientDelay )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

    }

    public static void AssertMeasurementsShouldConfigure( ThermalTransientMeasure? thermalTransientMeasure, ThermalTransient? thermalTransient )
    {
        Assert.IsNotNull( thermalTransient, $"{nameof( ThermalTransient )} should not be null." );

        Assert.IsNotNull( thermalTransientMeasure, $"{nameof( ThermalTransientMeasure )} should not be null." );
        Assert.IsNotNull( thermalTransientMeasure.Session, $"{nameof( ThermalTransientMeasure.Session )} should not be null." );
        Assert.IsTrue( thermalTransientMeasure.Session.IsDeviceOpen,
            $"The {nameof( ThermalTransientMeasure.Session )} should be connected if the session opened." );

        Pith.SessionBase session = thermalTransientMeasure.Session;

        double? expectedValue;
        double? actualValue;
        double? appliedValue;

        // High Limit

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.TraceDefaults.HighLimit;
        actualValue = thermalTransient.HighLimit;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{thermalTransientMeasure.EntityName} {nameof( ThermalTransient.HighLimit )} should equal the settings value." );

        // get actual value
        actualValue = thermalTransientMeasure.QueryHighLimit();
        Assert.IsNotNull( actualValue, $"The {thermalTransientMeasure.EntityName} {nameof( ThermalTransient.HighLimit )} should not be null." );
        Assert.IsGreaterThan( 0, actualValue.Value, $"The {thermalTransientMeasure.EntityName} {nameof( ThermalTransient.HighLimit )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualValue;
        appliedValue = thermalTransientMeasure.ApplyHighLimit( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.HighLimit )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.HighLimit )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualValue;
        appliedValue = thermalTransientMeasure.ApplyHighLimit( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {thermalTransientMeasure.EntityName} restored {nameof( ThermalTransient.HighLimit )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.HighLimit )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        // Low Limit

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.TraceDefaults.LowLimit;
        actualValue = thermalTransient.LowLimit;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{thermalTransientMeasure.EntityName} {nameof( ThermalTransient.LowLimit )} should equal the settings value." );

        // get actual value
        actualValue = thermalTransientMeasure.QueryLowLimit();
        Assert.IsNotNull( actualValue, $"The {thermalTransientMeasure.EntityName} {nameof( ThermalTransient.LowLimit )} should not be null." );
        Assert.IsGreaterThan( 0, actualValue.Value, $"The {thermalTransientMeasure.EntityName} {nameof( ThermalTransient.LowLimit )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualValue;
        appliedValue = thermalTransientMeasure.ApplyLowLimit( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.LowLimit )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.LowLimit )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualValue;
        appliedValue = thermalTransientMeasure.ApplyLowLimit( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {thermalTransientMeasure.EntityName} restored {nameof( ThermalTransient.LowLimit )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.LowLimit )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );


        // Current Level

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.TraceDefaults.CurrentLevel;
        actualValue = thermalTransient.CurrentLevel;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{thermalTransientMeasure.EntityName} {nameof( ThermalTransient.CurrentLevel )} should equal the settings value." );

        // get actual value
        actualValue = thermalTransientMeasure.QueryCurrentLevel();
        Assert.IsNotNull( actualValue, $"The {thermalTransientMeasure.EntityName} {nameof( ThermalTransient.CurrentLevel )} should not be null." );
        Assert.IsGreaterThan( 0, actualValue.Value, $"The {thermalTransientMeasure.EntityName} {nameof( ThermalTransient.CurrentLevel )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualValue;
        appliedValue = thermalTransientMeasure.ApplyCurrentLevel( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.CurrentLevel )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.CurrentLevel )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualValue;
        appliedValue = thermalTransientMeasure.ApplyCurrentLevel( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {thermalTransientMeasure.EntityName} restored {nameof( ThermalTransient.CurrentLevel )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.CurrentLevel )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        // Voltage Change

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.TraceDefaults.VoltageChange;
        actualValue = thermalTransient.AllowedVoltageChange;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{thermalTransientMeasure.EntityName} {nameof( ThermalTransient.AllowedVoltageChange )} should equal the settings value." );

        // get actual value
        actualValue = thermalTransientMeasure.QueryVoltageChange();
        Assert.IsNotNull( actualValue, $"The {thermalTransientMeasure.EntityName} {nameof( ThermalTransient.AllowedVoltageChange )} should not be null." );
        Assert.IsGreaterThan( 0, actualValue.Value, $"The {thermalTransientMeasure.EntityName} {nameof( ThermalTransient.AllowedVoltageChange )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualValue;
        appliedValue = thermalTransientMeasure.ApplyVoltageChange( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.AllowedVoltageChange )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.AllowedVoltageChange )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualValue;
        appliedValue = thermalTransientMeasure.ApplyVoltageChange( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {thermalTransientMeasure.EntityName} restored {nameof( ThermalTransient.AllowedVoltageChange )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {thermalTransientMeasure.EntityName} applied {nameof( ThermalTransient.AllowedVoltageChange )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    public static void AssertMeasurementsShouldConfigure( ColdResistanceMeasure? coldResistanceMeasure, ColdResistance? coldResistance )
    {
        Assert.IsNotNull( coldResistance, $"{nameof( coldResistance )} should not be null." );

        Assert.IsNotNull( coldResistanceMeasure, $"{nameof( coldResistanceMeasure )} should not be null." );
        Assert.IsNotNull( coldResistanceMeasure.Session, $"{nameof( coldResistanceMeasure.Session )} should not be null." );
        Assert.IsTrue( coldResistanceMeasure.Session.IsDeviceOpen,
            $"The {nameof( coldResistanceMeasure.Session )} should be connected if the session opened." );

        Pith.SessionBase session = coldResistanceMeasure.Session;

        double? expectedValue;
        double? actualValue;
        double? appliedValue;

        Asserts.AssertSourceShouldConfigure( coldResistanceMeasure, coldResistance );

        // High Limit

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.HighLimit;
        actualValue = coldResistance.HighLimit;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{coldResistanceMeasure.EntityName} {nameof( ColdResistance.HighLimit )} should equal the settings value." );

        // get actual value
        actualValue = coldResistanceMeasure.QueryHighLimit();
        Assert.IsNotNull( actualValue, $"The {coldResistanceMeasure.EntityName} {nameof( ColdResistance.HighLimit )} should not be null." );
        Assert.IsGreaterThan( 0, actualValue.Value, $"The {coldResistanceMeasure.EntityName} {nameof( ColdResistance.HighLimit )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualValue;
        appliedValue = coldResistanceMeasure.ApplyHighLimit( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {coldResistanceMeasure.EntityName} applied {nameof( ColdResistance.HighLimit )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {coldResistanceMeasure.EntityName} applied {nameof( ColdResistance.HighLimit )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualValue;
        appliedValue = coldResistanceMeasure.ApplyHighLimit( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {coldResistanceMeasure.EntityName} restored {nameof( ColdResistance.HighLimit )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {coldResistanceMeasure.EntityName} applied {nameof( ColdResistance.HighLimit )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        // Low Limit

        expectedValue = cc.isr.VI.Tsp.K2600.Ttm.Properties.DriverSettings.Instance.ColdResistanceDefaults.LowLimit;
        actualValue = coldResistance.LowLimit;
        Assert.AreEqual( expectedValue.Value, actualValue.Value, 0.001 * expectedValue.Value,
            $"{coldResistanceMeasure.EntityName} {nameof( ColdResistance.LowLimit )} should equal the settings value." );

        // get actual value
        actualValue = coldResistanceMeasure.QueryLowLimit();
        Assert.IsNotNull( actualValue, $"The {coldResistanceMeasure.EntityName} {nameof( ColdResistance.LowLimit )} should not be null." );
        Assert.IsGreaterThan( 0, actualValue.Value, $"The {coldResistanceMeasure.EntityName} {nameof( ColdResistance.LowLimit )} must be positive." );

        // alter the actual value
        expectedValue = 0.9 * actualValue;
        appliedValue = coldResistanceMeasure.ApplyLowLimit( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {coldResistanceMeasure.EntityName} applied {nameof( ColdResistance.LowLimit )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {coldResistanceMeasure.EntityName} applied {nameof( ColdResistance.LowLimit )} should equal the changed value." );

        // restore the actual value
        expectedValue = actualValue;
        appliedValue = coldResistanceMeasure.ApplyLowLimit( ( float ) expectedValue );
        Assert.IsNotNull( appliedValue, $"The {coldResistanceMeasure.EntityName} restored {nameof( ColdResistance.LowLimit )} should not be null." );
        Assert.AreEqual( expectedValue.Value, appliedValue.Value, 0.001 * expectedValue.Value,
            $"The {coldResistanceMeasure.EntityName} applied {nameof( ColdResistance.LowLimit )} should equal the restored value." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

    }

    /// <summary>   Assert measurements should configure. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="meter"> The legacy device. </param>
    public static void AssertMeasurementsShouldConfigure( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsTrue( meter.IsSessionOpen, "The meter should be connected if the session opened." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Pith.SessionBase session = meter.TspDevice.Session;

        SourceOutputOption? initialSourceOutputOption = meter.InitialResistance?.QuerySourceOutputOption();
        Assert.IsNotNull( initialSourceOutputOption, "Failed to query the initial source output option." );

        SourceOutputOption? alternateOption = initialSourceOutputOption == SourceOutputOption.Current
                ? SourceOutputOption.Voltage
                : SourceOutputOption.Current;

        Asserts.AssertSourceShouldConfigure( meter );

        if ( alternateOption != initialSourceOutputOption )
        {
            alternateOption = meter.InitialResistance?.ApplySourceOutputOption( initialSourceOutputOption.Value );

            // validate the alternate option settings.
            Asserts.AssertSourceShouldConfigure( meter );
        }

        Asserts.AssertMeasurementsShouldConfigure( meter.MeterSubsystem, meter.ConfigInfo.MeterMain );
        Asserts.AssertMeasurementsShouldConfigure( meter.InitialResistance, meter.ConfigInfo.InitialResistance );
        Asserts.AssertMeasurementsShouldConfigure( meter.FinalResistance, meter.ConfigInfo.FinalResistance );
        Asserts.AssertMeasurementsShouldConfigure( meter.ThermalTransient, meter.ConfigInfo.ThermalTransient );
    }

    /// <summary>   Assert trigger cycle should start. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="meter">                         The legacy device. </param>
    /// <param name="timeout">                              The timeout. </param>
    /// <param name="validateTriggerCycleCompleteMessage">  True to validate trigger cycle complete
    ///                                                     message. </param>
    public static void AssertTriggerCycleShouldStart( Meter? meter, TimeSpan timeout )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice!.Session, $"{nameof( Meter )} should not be null." );
        Pith.SessionBase session = meter.TspDevice.Session;

        // start the trigger cycle
        meter.PrepareForTrigger();

        Assert.IsTrue( meter.IsAwaitingTrigger, "awaiting trigger should be true." );

        //allow some time for the trigger loop to start..
        _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );

        session.AssertTrigger();

        bool messageAvailable = false;
        Stopwatch sw = Stopwatch.StartNew();
        while ( !messageAvailable && sw.Elapsed < timeout )
        {
            _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 10 ) );
            messageAvailable = meter.IsMeasurementCompleted();
        }

        Assert.IsTrue( messageAvailable, "The trigger wait should not timeout." );
        Assert.IsFalse( meter.IsAwaitingTrigger, "awaiting trigger should be false after trigger." );

        sw.Stop();
        TimeSpan timeSpan = sw.Elapsed;
        Console.WriteLine( $"Trigger Cycle Time: {timeSpan:s\\.fff}s" );
    }

    /// <summary>   Assert trigger cycle complete message should read. </summary>
    /// <remarks>   2026-03-30. </remarks>
    /// <param name="legacyDevice"> The legacy device. </param>
    /// <param name="timeout">      The timeout. </param>
    public static void AssertTriggerCycleCompleteMessageShouldRead( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice!.Session, $"{nameof( Meter )} should not be null." );
        Pith.SessionBase session = meter.TspDevice.Session;

        string reading = session.ReadLineTrimEnd();
        Assert.IsFalse( string.IsNullOrEmpty( reading ), $"Triggered output should not be null or empty." );
        Assert.AreEqual( meter.MeasurementCompletedReply, reading, "The trigger cycle completion reply should match the expected value" );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary>   Assert measurements should read. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="meter">                     The legacy device. </param>
    /// <param name="readTriggerCycleCompleteMessage">  True to read trigger cycle complete message. </param>
    public static void AssertMeasurementsShouldRead( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice!.Session, $"{nameof( Meter )} should not be null." );

        // the trigger cycle complete message was already read
        meter.ReadMeasurements();

        Console.WriteLine( meter?.InitialResistance?.BuildMeasuredValues() );
        Console.WriteLine( meter?.FinalResistance?.BuildMeasuredValues() );
        Console.WriteLine( meter?.ThermalTransient?.BuildMeasuredValues() );
    }

    /// <summary>   Assert cold resistance reading should validate. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="meter">        The TTM device. </param>
    /// <param name="subsystem">    The subsystem. </param>
    /// <param name="measureBase">  The measure base. </param>
    public static void AssertMeasuredReadingsShouldValidate( Meter? meter, MeasureSubsystemBase subsystem, MeasureBase measureBase )
    {
        Assert.IsNotNull( measureBase, $"{nameof( measureBase )} should not be null." );
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice!.Session, $"{nameof( Meter )} should not be null." );
        Pith.SessionBase session = meter.TspDevice.Session;

        if ( measureBase.FirmwareReading == SessionBase.NilValue )
        {
            Assert.IsFalse( measureBase.MeasurementAvailable, $"{nameof( MeasureBase.MeasurementAvailable )} should be false if measurement was not made." );
        }
        else
        {
            _ = subsystem.QueryHighLimit();
            _ = subsystem.QueryLowLimit();
            Assert.IsGreaterThan( 0, measureBase.LowLimit, $"{nameof( MeasureBase.LowLimit )} should be positive." );
            Assert.IsGreaterThan( 0, measureBase.HighLimit, $"{nameof( MeasureBase.HighLimit )} should be positive." );

            Assert.IsNotNull( measureBase.FirmwareOutcomeReading, $"{nameof( MeasureBase.FirmwareOutcomeReading )} should be not not be null if measurement was made." );
            Assert.IsNotEmpty( measureBase.FirmwareOutcomeReading, $"{nameof( MeasureBase.FirmwareOutcomeReading )} should be not not be empty if measurement was made." );
            Assert.IsTrue( measureBase.FirmwareOutcomeReading.TryParseNullableInteger( out int? outcomeValue ) );
            Assert.IsNotNull( outcomeValue, $"{nameof( MeasureBase.FirmwareOutcomeReading )} {measureBase.FirmwareOutcomeReading} should yield a value if measurement was made." );

            int? status = session.QueryNullableIntegerThrowIfError( $"print({subsystem.EntityName}.status) ", $"{subsystem.EntityName} status" );
            Assert.IsNotNull( status, $"{subsystem.EntityName}.status should be not be null if measurement was made." );

            Assert.IsNotNull( measureBase.FirmwareStatusReading, $"{nameof( MeasureBase.FirmwareStatusReading )} should be not not be null if measurement was made." );
            Assert.IsTrue( measureBase.FirmwareStatusReading.TryParseNullableInteger( out int? statusValue ) );
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
                    Assert.IsTrue( measureBase.MeasurementAvailable, $"{nameof( MeasureBase.MeasurementAvailable )} should be true if measurement was made." );

                    double measuredValue = measureBase.MeasuredValue;
                    double low = measureBase.LowLimit;
                    double high = measureBase.HighLimit;
                    bool? passed = session.QueryNullableBoolThrowIfError( $"print({subsystem.EntityName}.pass) ", $"{subsystem.EntityName} pass value" );
                    Assert.IsNotNull( passed, $"{subsystem.EntityName}.pass should be not be null if measurement was made." );

                    if ( measuredValue < low )
                    {
                        Assert.IsFalse( passed.Value,
                             $"{subsystem.EntityName}.pass should be false if measured value {measuredValue:#0.###} is lower than {low:#0.###}." );
                    }
                    else if ( measuredValue > high )
                    {
                        Assert.IsFalse( passed.Value,
                             $"{subsystem.EntityName}.pass should be false if measured value {measuredValue:#0.###} is higher than {high:#0.###}." );
                    }
                    else
                    {
                        Assert.IsTrue( passed.Value,
                             $"{subsystem.EntityName}.pass should be true if measured value {measuredValue:#0.###} is within[{low:#0.###}, {high:#0.###}]." );
                    }
                }
                else if ( outcomeValue == ( int ) FirmwareOutcomes.BadStatus )
                {
                    Assert.IsFalse( measureBase.MeasurementAvailable, $"{nameof( MeasureBase.MeasurementAvailable )} should be false if measurement failed or not made." );

                    bitValue = ( int ) cc.isr.VI.BufferElementStatusBits.Compliance;
                    Assert.AreEqual( bitValue & statusValue, bitValue,
                        $"status {statusValue} must report compliance if measurement failed with bad status." );
                }
            }
            else
            {
                Assert.IsTrue( measureBase.MeasurementAvailable, $"{nameof( MeasureBase.MeasurementAvailable )} should be true if measurement was made." );

                int? passFailOutcome = session.QueryNullableIntegerThrowIfError( $"print({subsystem.EntityName}.passFailOutcome) ",
                    $"{subsystem.EntityName} pass fail outcome" );
                Assert.IsNotNull( passFailOutcome, $"{subsystem.EntityName}.passFailOutcome should be not be null if measurement was made." );

                double resistanceValue = measureBase.MeasuredValue;
                double low = measureBase.LowLimit;
                double high = measureBase.HighLimit;
                if ( ( int ) PassFailBits.Unknown == passFailOutcome.Value )
                {
                    // failed contact check.
                    Assert.AreEqual( ( int ) FirmwareOutcomes.BadStatus, ( int ) FirmwareOutcomes.BadStatus & outcomeValue.Value,
                        $"Outcome value {outcomeValue.Value} bad status bit {FirmwareOutcomes.BadStatus} should be set if contact check failed." );
                }
                else if ( resistanceValue < low )
                {
                    Assert.AreEqual( ( int ) PassFailBits.Low, passFailOutcome.Value,
                        $"Pass fail value {passFailOutcome.Value} must be 'low' if measured value {resistanceValue} is lower than {low}." );
                }
                else if ( resistanceValue > high )
                {
                    Assert.AreEqual( ( int ) PassFailBits.High, passFailOutcome.Value,
                        $"Pass fail value {passFailOutcome.Value} must be 'high' if measured value {resistanceValue} is high than {high}." );
                }
                else
                {
                    Assert.AreEqual( ( int ) PassFailBits.Pass, passFailOutcome.Value,
                        $"Pass fail value {passFailOutcome.Value} must be 'pass' if measured value {resistanceValue} is within [{low},{high}]." );
                }

            }
        }

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }

    /// <summary>   Assert initial resistance reading should validate. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="meter"> The legacy device. </param>
    public static void AssertInitialResistanceReadingShouldValidate( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Assert.IsNotNull( meter.InitialResistance, $"{nameof( Meter )}{nameof( Meter.InitialResistance )} should not be null." );
        Asserts.AssertMeasuredReadingsShouldValidate( meter, meter.InitialResistance, meter.InitialResistance.ColdResistance );
    }

    /// <summary>   Assert Final resistance reading should validate. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="meter"> The legacy device. </param>
    public static void AssertFinalResistanceReadingShouldValidate( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Assert.IsNotNull( meter.FinalResistance, $"{nameof( Meter )}{nameof( Meter.FinalResistance )} should not be null." );
        Asserts.AssertMeasuredReadingsShouldValidate( meter, meter.FinalResistance, meter.FinalResistance.ColdResistance );
    }

    /// <summary>   Assert initial resistance reading should validate. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="meter"> The legacy device. </param>
    public static void AssertThermalTransientReadingShouldValidate( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice, $"{nameof( Meter )}.{nameof( Meter.TspDevice )} should not be null." );
        Assert.IsNotNull( meter.TspDevice.Session, $"{nameof( Meter )}.{nameof( Meter.TspDevice )}.{nameof( Meter.TspDevice.Session )} should not be null." );
        Assert.IsNotNull( meter.ThermalTransient, $"{nameof( Meter )}{nameof( Meter.ThermalTransient )} should not be null." );
        Asserts.AssertMeasuredReadingsShouldValidate( meter, meter.ThermalTransient, meter.ThermalTransient.ThermalTransient );
    }

    /// <summary>   Assert trigger cycle should abort. </summary>
    /// <remarks>   2024-11-08. </remarks>
    /// <param name="meter"> The legacy device. </param>
    public static void AssertTriggerCycleShouldAbort( Meter? meter )
    {
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter, $"{nameof( Meter )} should not be null." );
        Assert.IsNotNull( meter.TspDevice!.Session, $"{nameof( Meter )} should not be null." );
        Pith.SessionBase session = meter.TspDevice.Session;

        // start the trigger cycle
        meter.PrepareForTrigger();

        Assert.IsTrue( meter.IsAwaitingTrigger, "awaiting trigger should be true." );

        //allow some time for the trigger loop to start..
        _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );

        meter.AbortTriggerSequence();

        Assert.IsFalse( meter.IsAwaitingTrigger, "awaiting trigger should be false after trigger." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );
    }
}
