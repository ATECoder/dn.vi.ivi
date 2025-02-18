using cc.isr.VI.Tsp.SessionBaseExtensions;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;

namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Firmware;
internal static partial class Asserts
{
    /// <summary>   Assert tsp syntax should not fail. </summary>
    /// <remarks>   2024-11-02. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertTspSyntaxShouldNotFail( Pith.SessionBase? session, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string command;
        string expectedValue;
        int expectedInt;

        // *SRE service request enable
        command = "_G.status.request_enable=0";
        Asserts.AssertCommandShouldExecute( session, command, logEnabled );

        expectedValue = session.ResourceSettings.ResourceModel;
        command = "_G.print(localnode.model)";
        Asserts.AssertQueryReplyShouldBeValid( session, command, expectedValue, logEnabled );

        expectedValue = "1";
        command = "*OPC?";
        Asserts.AssertQueryReplyShouldBeValid( session, command, expectedValue, logEnabled );

        // *RST
        command = "_G.reset()";
        Asserts.AssertCommandShouldExecute( session, command, logEnabled );

        expectedValue = "1";
        command = cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand;
        Asserts.AssertQueryReplyShouldBeValid( session, command, expectedValue, logEnabled );

        expectedInt = 60;
        command = "_G.print(localnode.linefreq)";
        Asserts.AssertQueryReplyShouldBeValid( session, command, expectedInt, logEnabled );

        // *CLS
        command = session.ClearExecutionStateCommand;
        Asserts.AssertCommandShouldExecute( session, command, logEnabled );

        // *OPC?
        expectedValue = "1";
        command = session.OperationCompletedQueryCommand;
        Asserts.AssertQueryReplyShouldBeValid( session, command, expectedValue, logEnabled );

        // *CLS *OPC?
        expectedValue = "1";
        command = session.ClearExecutionStateQueryCompleteCommand;
        Asserts.AssertQueryReplyShouldBeValid( session, command, expectedValue, logEnabled );

        expectedValue = "1";
        command = $"_G.errorqueue.clear() {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} ";
        Asserts.AssertQueryReplyShouldBeValid( session, command, expectedValue, logEnabled );

        // !@#: @to_be_fixed
#if false
        command = "_G.status.reset() _G.waitcomplete() _G.status.standard.enable=253 _G.status.request_enable=32 _G.opc()";
        Asserts.AssertCommandShouldExecute( session, command, logEnabled );

        // wait for operation completion bit.
        (bool timedOut, Pith.ServiceRequests status, TimeSpan elapsed) = session.AwaitOperationCompletion( TimeSpan.FromMilliseconds( 1000 ) );
        Assert.IsFalse( timedOut, $"wait for operation completion timed out with status {( int ) status:X2} after {elapsed.TotalMilliseconds:0} ms." );
#endif

        // this should work....
        command = "*IDN?; *WAI";
        _ = Asserts.AssertQueryShouldExecute( session, command, logEnabled );

        expectedValue = "1";
        command = session.OperationCompletedQueryCommand;
        Asserts.AssertQueryReplyShouldBeValid( session, command, expectedValue, logEnabled );

        command = "_G.print(display)";
        _ = Asserts.AssertQueryShouldExecute( session, command, logEnabled );
    }

    /// <summary>   Assert meter value should reset. </summary>
    /// <remarks>   2024-11-02. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="checkSmuSetter">   (Optional) True to check smu setter. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertMeterValueShouldReset( Pith.SessionBase? session, bool checkSmuSetter = false, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string query, command;
        bool expectedBoolean;
        double expectedDouble;
        int expectedInt;
        string expectedValue;

        cc.isr.VI.Tsp.K2600.Ttm.TtmMeterSettings meterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;

        Assert.IsTrue( meterSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings )} should exist." );

        expectedBoolean = true;
        query = "_G.print(_G.isr.access.loaded())";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );

        double expectedPostTransientDelayDefault; // = 0.5;
        if ( !MeterSubsystem.LegacyFirmware )
        {
            expectedPostTransientDelayDefault = meterSettings.PostTransientDelayDefault;
            query = "_G.print(string.format('%9.3f',_G.ttm.meterDefaults.postTransientDelay))";
            double postTransientDelayDefault = session.QueryDoubleThrowIfError( query, "post transient delay" );
            Assert.AreEqual( expectedPostTransientDelayDefault, postTransientDelayDefault, $"{nameof( postTransientDelayDefault )} should equal the settings value." );
            Asserts.LogIT( $"{query} returned {postTransientDelayDefault}" );
        }

        // get actual value
        query = "_G.print(string.format('%9.4f',_G.ttm.postTransientDelayGetter()))";
        double postTransientDelay = session.QueryDoubleThrowIfError( query, "actual post transient delay" );

        expectedDouble = postTransientDelay + 0.001;
        command = $"_G.ttm.postTransientDelaySetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        expectedDouble = postTransientDelay;
        command = $"_G.ttm.postTransientDelaySetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        // the legacy driver is agnostic to the meter settings other than the post transient delay and, therefore, could be tested even if testing the legacy driver.

        if ( !MeterSubsystem.LegacyFirmware )
        {
            int contactLimit; // 100;
            query = "_G.print(string.format('%d',_G.ttm.meterDefaults.contactLimit))";

            contactLimit = session.QueryIntegerThrowIfError( query, "contact limit" );
            Assert.AreEqual( meterSettings.ContactCheckThresholdDefault, contactLimit, $"{nameof( contactLimit )} should equal the settings value." );
            Asserts.LogIT( $"{query} returned {contactLimit}" );

            // get actual value
            query = "_G.print(string.format('%d',_G.ttm.contactLimitGetter()))";
            contactLimit = session.QueryIntegerThrowIfError( query, "actual contact limit" );

            // set a new value
            expectedInt = contactLimit + 1;
            command = $"_G.ttm.contactLimitSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            // restore actual value
            expectedInt = contactLimit;
            command = $"_G.ttm.contactLimitSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            int contactOptions; // = 7;
            query = "_G.print(string.format('%d',_G.ttm.meterDefaults.contactCheckOptions))";
            contactOptions = session.QueryIntegerThrowIfError( query, "default contact check options" );
            Assert.AreEqual( ( int ) meterSettings.ContactCheckOptionsDefault, contactOptions, $"{nameof( contactOptions )} should equal the settings value." );
            Asserts.LogIT( $"{query} returned {contactOptions}" );

            // get actual value
            query = "_G.print(string.format('%d',_G.ttm.contactCheckOptionsGetter()))";
            int initialContactOptions = session.QueryIntegerThrowIfError( query, "actual contact check options" );

            // change after updating the deployed version;
            expectedInt = initialContactOptions == 1 ? 7 : 1;
            query = "_G.print(string.format('%d',_G.ttm.contactCheckOptionsGetter()))";
            command = $"_G.ttm.contactCheckOptionsSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            // restore value
            expectedInt = initialContactOptions;
            command = $"_G.ttm.contactCheckOptionsSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            expectedBoolean = ( int ) ContactCheckOptions.Initial == (contactOptions & ( int ) ContactCheckOptions.Initial);
            query = "_G.print(_G.ttm.checkContactsBeforeInitialResistance())";
            Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );

            // change after updating the deployed version;
            expectedBoolean = ( int ) ContactCheckOptions.Trace == (contactOptions & ( int ) ContactCheckOptions.Trace);
            query = "_G.print(_G.ttm.checkContactsBeforeThermalTransient())";
            Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );

            // change after updating the deployed version;
            expectedBoolean = ( int ) ContactCheckOptions.Final == (contactOptions & ( int ) ContactCheckOptions.Final);
            query = "_G.print(_G.ttm.checkContactsBeforeFinalResistance())";
            Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );

            int legacyDriver; // = 1;
            query = "_G.print(string.format('%d',_G.ttm.meterDefaults.legacyDriver))";
            legacyDriver = session.QueryIntegerThrowIfError( query, "legacy driver" );
            Assert.AreEqual( meterSettings.LegacyDriverDefault, legacyDriver, $"{nameof( legacyDriver )} should equal the settings value." );
            Asserts.LogIT( $"{query} returned {legacyDriver}" );

            expectedInt = legacyDriver == 1 ? 0 : 1;
            query = "_G.print(string.format('%d',_G.ttm.legacyDriverGetter()))";
            command = $"_G.ttm.legacyDriverSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            // restore value
            expectedInt = legacyDriver;
            command = $"_G.ttm.legacyDriverSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            string model = session.ResourceSettings.ResourceModel;
            query = "_G.print(localnode.model)";
            Asserts.AssertQueryReplyShouldBeValid( session, query, model, logEnabled );

            string smuName; // "smua";
            query = "_G.print(_G.ttm.meterDefaults.smuName)";
            smuName = session.QueryStringThrowIfError( query, "smu name" );
            Assert.AreEqual( meterSettings.SourceMeasureUnitDefault, smuName, $"{nameof( smuName )} should equal the settings value." );

            expectedValue = smuName;
            expectedBoolean = true;
            query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.smuGetter())";
            Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );

            expectedValue = smuName;
            query = "_G.print(_G.ttm.smuNameGetter())";
            Asserts.AssertQueryReplyShouldBeValid( session, query, expectedValue, logEnabled );

            if ( checkSmuSetter )
            {
                if ( ("26" == model[..2]) && ("2" == model.Substring( 3, 1 )) )
                {
                    expectedValue = smuName == "smua" ? "smub" : "smua";
                    query = "_G.print(_G.ttm.smuNameGetter())";
                    command = $"_G.ttm.smuNameSetter('{expectedValue}')";
                    Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedValue, logEnabled );

                    expectedBoolean = true;
                    query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.smuGetter())";
                    Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
                }

                // restore the smu name to the default value
                expectedValue = smuName;
                query = "_G.print(_G.ttm.smuNameGetter())";
                command = $"_G.ttm.smuNameSetter(_G.ttm.meterDefaults.smuName)";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedValue, logEnabled );

                expectedBoolean = true;
                query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.smuGetter())";
                Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
            }
        }
    }

    /// <summary>   Assert cold resistance defaults should equal settings. </summary>
    /// <remarks>   2025-02-06. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="logEnabled">   (Optional) True to enable, false to disable the log. </param>
    public static void AssertColdResistanceDefaultsShouldEqualSettings( Pith.SessionBase? session, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string query;
        double expectedDouble;

        cc.isr.VI.Tsp.K2600.Ttm.TtmResistanceSettings resistanceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings;
        Assert.IsTrue( resistanceSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings )} should exist." );

        cc.isr.VI.Tsp.K2600.Ttm.TtmMeterSettings meterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
        Assert.IsTrue( meterSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings )} should exist." );

        double aperture; // 1.0000;
        query = $"_G.print(string.format('%7.4f',_G.ttm.coldResistance.Defaults.aperture))";
        aperture = session.QueryDoubleThrowIfError( query, "default aperture" );
        Assert.AreEqual( resistanceSettings.ApertureDefault, aperture, $"{nameof( aperture )} should match settings value." );

        expectedDouble = MeterSubsystem.LegacyFirmware ? 0.0001 : resistanceSettings.CurrentMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minCurrent))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.0001, logEnabled );

        expectedDouble = MeterSubsystem.LegacyFirmware ? 0.01 : resistanceSettings.CurrentMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxCurrent))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.0001, logEnabled );

        expectedDouble = resistanceSettings.Minimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minResistance))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.0001, logEnabled );

        expectedDouble = resistanceSettings.Maximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxResistance))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.0001, logEnabled );

        expectedDouble = resistanceSettings.VoltageMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minVoltage))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.0001, logEnabled );

        expectedDouble = MeterSubsystem.LegacyFirmware ? 10 : resistanceSettings.VoltageMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxVoltage))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.0001, logEnabled );

        double lowLimit; // 1.85;
        query = $"_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.lowLimit))";
        lowLimit = session.QueryDoubleThrowIfError( query, "default low limit" );
        Assert.AreEqual( resistanceSettings.LowLimitDefault, lowLimit, $"{nameof( lowLimit )} should match settings value." );

        double highLimit; // 2.156;
        query = $"_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.highLimit))";
        highLimit = session.QueryDoubleThrowIfError( query, "default high limit" );
        Assert.AreEqual( resistanceSettings.HighLimitDefault, highLimit, $"{nameof( highLimit )} should match settings value." );

        double level; // 0.01;
        query = $"_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.level))";
        level = session.QueryDoubleThrowIfError( query, "Cold resistance current source level or voltage source current limit" );
        Assert.AreEqual( resistanceSettings.CurrentLevel, level, $"{nameof( level )} should match {nameof( resistanceSettings.CurrentLevel )} default settings value." );

        double limit; // 0.1
        query = $"_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.limit))";
        limit = session.QueryDoubleThrowIfError( query, "Cold resistance current source voltage limit or voltage source voltage level" );
        Assert.AreEqual( resistanceSettings.VoltageLimit, limit, $"{nameof( limit )} should match the {nameof( resistanceSettings.VoltageLimit )} default settings value." );

        if ( MeterSubsystem.LegacyFirmware )
        {
            string smuName = "smua";
            query = $"_G.print('{smuName}' == _G.ttm.coldResistance.Defaults.smuI)";
            Asserts.AssertQueryReplyShouldBeValid( session, query, true, logEnabled );
        }
        else
        {
            query = $"_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.failStatus))";
            int failStatus = session.QueryIntegerThrowIfError( query, "default fail StatusReading" );
            Assert.AreEqual( resistanceSettings.FailStatus, failStatus, $"{nameof( highLimit )} should match settings value." );

            query = $"_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.levelI))";
            double levelI = session.QueryDoubleThrowIfError( query, "default current source current level" );
            Assert.AreEqual( resistanceSettings.CurrentLevel, levelI, $"{nameof( levelI )} should match settings value." );

            query = $"_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.levelV))";
            double levelV = session.QueryDoubleThrowIfError( query, "default voltage source voltage level" );
            Assert.AreEqual( resistanceSettings.VoltageLevel, levelV, $"{nameof( levelV )} should match settings value." );

            query = $"_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.limitI))";
            double limitI = session.QueryDoubleThrowIfError( query, "default voltage source current limit" );
            Assert.AreEqual( resistanceSettings.CurrentLimit, limitI, $"{nameof( limitI )} should match settings value." );

            query = $"_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.limitV))";
            double limitV = session.QueryDoubleThrowIfError( query, "default current source voltage limit" );
            Assert.AreEqual( resistanceSettings.VoltageLimit, limitV, $"{nameof( limitV )} should match settings value." );
        }
    }

    /// <summary>   Assert initial resistance commands should execute. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">              The session. </param>
    /// <param name="coldResistanceType">   (Optional) (ir) Type of the cold resistance. </param>
    /// <param name="checkSmuSetter">       (Optional) True to check smu setter. </param>
    /// <param name="logEnabled">           (Optional) True to enable, false to disable the log. </param>
    public static void AssertColdResistanceShouldReset( Pith.SessionBase? session, string coldResistanceType = "ir", bool checkSmuSetter = false, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        Assert.IsNotNull( coldResistanceType, $"{nameof( coldResistanceType )} must not be null." );
        Assert.AreNotEqual( string.Empty, coldResistanceType, $"{nameof( coldResistanceType )} must not be empty." );
        Assert.IsTrue( string.Equals( "ir", coldResistanceType, System.StringComparison.Ordinal ) | string.Equals( "fr", coldResistanceType, System.StringComparison.Ordinal ),
            $"{nameof( coldResistanceType )} must be either 'ir' or 'fr'." );
        string command, query;
        bool expectedBoolean;
        double expectedDouble;
        int expectedInt;
        string expectedValue;

        cc.isr.VI.Tsp.K2600.Ttm.TtmResistanceSettings resistanceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings;
        Assert.IsTrue( resistanceSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings )} should exist." );

        cc.isr.VI.Tsp.K2600.Ttm.TtmMeterSettings meterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
        Assert.IsTrue( meterSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings )} should exist." );

        string model = session.ResourceSettings.ResourceModel;
        command = "_G.print(localnode.model)";
        Asserts.AssertQueryReplyShouldBeValid( session, command, model, logEnabled );

        // legacy driver: these need only be tested for the legacy driver.

        if ( MeterSubsystem.LegacyFirmware )
        {
            if ( checkSmuSetter )
            {
                string smuName = "smua";
                query = $"_G.print(_G.localnode.{smuName} == _G.ttm.{coldResistanceType}.smuI)";
                if ( !session.QueryBoolThrowIfError( query, $"find {smuName}" ) )
                {
                    smuName = "smub";
                    query = $"_G.print(_G.localnode.{smuName} == _G.ttm.{coldResistanceType}.smuI)";
                    Assert.IsTrue( session.QueryBoolThrowIfError( query, $"find {smuName}" ), $"failed finding SMU for {coldResistanceType} cold resistance." );
                }

                if ( ("26" == model[..2]) && ("2" == model.Substring( 3, 1 )) )
                {
                    expectedValue = smuName == "smua" ? "smub" : "smua";
                    command = $"_G.ttm.{coldResistanceType}:currentSourceChannelSetter({expectedValue})";
                    Asserts.AssertCommandShouldExecute( session, command, logEnabled );

                    expectedBoolean = true;
                    query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.{coldResistanceType}.smuI)";
                    Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
                }

                expectedValue = smuName;
                command = $"_G.ttm.{coldResistanceType}:currentSourceChannelSetter({expectedValue})";
                Asserts.AssertCommandShouldExecute( session, command, logEnabled );

                expectedBoolean = true;
                query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.{coldResistanceType}.smuI)";
                Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
            }
        }

        // get actual value
        query = $"_G.print(string.format('%7.4f',_G.ttm.{coldResistanceType}.aperture))";
        double aperture = session.QueryDoubleThrowIfError( query, "actual aperture" );

        // get actual value
        // restore actual value
        expectedDouble = aperture + 1;
        command = $"_G.ttm.{coldResistanceType}:apertureSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        // restore actual value
        expectedDouble = aperture;
        command = $"_G.ttm.{coldResistanceType}:apertureSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        // get actual value
        query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.lowLimit))";
        double lowLimit = session.QueryDoubleThrowIfError( query, "actual low limit" );

        // set a new value
        expectedDouble = lowLimit + 0.01;
        command = $"_G.ttm.{coldResistanceType}:lowLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

        // restore actual value
        expectedDouble = lowLimit;
        command = $"_G.ttm.{coldResistanceType}:lowLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

        // get actual value
        query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.highLimit))";
        double highLimit = session.QueryDoubleThrowIfError( query, "actual high limit" );

        // set a new value
        expectedDouble = highLimit + 0.01;
        command = $"_G.ttm.{coldResistanceType}:highLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

        // restore actual value
        expectedDouble = highLimit;
        command = $"_G.ttm.{coldResistanceType}:highLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

        if ( !MeterSubsystem.LegacyFirmware )
        {
            // get actual value
            query = $"_G.print(string.format('%d',_G.ttm.{coldResistanceType}.failStatus))";
            int failStatus = session.QueryIntegerThrowIfError( query, "actual fail StatusReading" );

            // set a new value
            expectedInt = failStatus == 2 ? 66 : 2;
            command = $"_G.ttm.{coldResistanceType}:failStatusSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            // restore actual value
            expectedInt = failStatus;
            command = $"_G.ttm.{coldResistanceType}:failStatusSetter(_G.ttm.coldResistance.Defaults.failStatus)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );
        }

        bool isVoltageSource = !MeterSubsystem.LegacyFirmware && session.QueryBoolThrowIfError( $"_G.print(_G.ttm.{coldResistanceType}:sourceVoltage())", "source voltage method" );

        Asserts.AssertColdResistanceShouldSet( session, isVoltageSource, coldResistanceType, logEnabled );

        if ( !MeterSubsystem.LegacyFirmware )
        {
            // set a new value
            expectedBoolean = !isVoltageSource;
            query = $"_G.print(_G.ttm.{coldResistanceType}:sourceVoltage())";
            command = expectedBoolean
                ? $"_G.ttm.{coldResistanceType}:sourceOutputSetter(_G.ttm.SourceOutputs.voltage)"
                : $"_G.ttm.{coldResistanceType}:sourceOutputSetter(_G.ttm.SourceOutputs.current)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedBoolean, logEnabled );

            Asserts.AssertColdResistanceShouldSet( session, isVoltageSource, coldResistanceType, logEnabled );

            // restore actual value
            expectedBoolean = isVoltageSource;
            query = $"_G.print(_G.ttm.{coldResistanceType}:sourceVoltage())";
            command = expectedBoolean
                ? $"_G.ttm.{coldResistanceType}:sourceOutputSetter(_G.ttm.SourceOutputs.voltage)"
                : $"_G.ttm.{coldResistanceType}:sourceOutputSetter(_G.ttm.SourceOutputs.current)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedBoolean, logEnabled );
        }
    }

    /// <summary>
    /// Asserts that the cold resistance voltage source level and limits should be set.
    /// </summary>
    /// <remarks>   2025-02-03. </remarks>
    /// <param name="session">              The session. </param>
    /// <param name="isVoltageSource">      True if is voltage source, false if not. </param>
    /// <param name="coldResistanceType">   (Optional) (ir) Type of the cold resistance. </param>
    /// <param name="logEnabled">           (Optional) True to enable, false to disable the log. </param>
    public static void AssertColdResistanceShouldSet( Pith.SessionBase? session, bool isVoltageSource, string coldResistanceType = "ir", bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        Assert.IsNotNull( coldResistanceType, $"{nameof( coldResistanceType )} must not be null." );
        Assert.AreNotEqual( string.Empty, coldResistanceType, $"{nameof( coldResistanceType )} must not be empty." );
        Assert.IsTrue( string.Equals( "ir", coldResistanceType, System.StringComparison.Ordinal ) | string.Equals( "fr", coldResistanceType, System.StringComparison.Ordinal ),
            $"{nameof( coldResistanceType )} must be either 'ir' or 'fr'." );
        string command, query;
        double expectedDouble;

        if ( isVoltageSource )
        {
            // get actual value
            query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.level))";
            double limit = session.QueryDoubleThrowIfError( query, "actual voltage source current limit" );

            // set a new value
            expectedDouble = limit - 0.001;
            command = $"_G.ttm.{coldResistanceType}:levelSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            // restore actual value
            expectedDouble = limit;
            command = $"_G.ttm.{coldResistanceType}:levelSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            // get actual value
            query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.limit))";
            double level = session.QueryDoubleThrowIfError( query, "actual voltage source voltage level" );

            // set a new value
            expectedDouble = level - 0.001;
            command = $"_G.ttm.{coldResistanceType}:limitSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            // restore actual value
            expectedDouble = level;
            command = $"_G.ttm.{coldResistanceType}:limitSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );
        }
        else
        {
            // get actual value
            query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.level))";
            double level = session.QueryDoubleThrowIfError( query, "actual current source current level" );

            // set a new value
            expectedDouble = level - 0.001;
            command = $"_G.ttm.{coldResistanceType}:levelSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            // restore actual value
            expectedDouble = level;
            command = $"_G.ttm.{coldResistanceType}:levelSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            // get actual value
            query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.limit))";
            double limit = session.QueryDoubleThrowIfError( query, "actual current source voltage limit" );

            // set a new value
            expectedDouble = limit - 0.001;
            command = $"_G.ttm.{coldResistanceType}:limitSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            // restore actual value
            expectedDouble = limit;
            command = $"_G.ttm.{coldResistanceType}:limitSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );
        }

        if ( !MeterSubsystem.LegacyFirmware )
        {
            if ( isVoltageSource )
            {
                // get actual value
                query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.levelV))";
                double levelV = session.QueryDoubleThrowIfError( query, "actual voltage source voltage level" );

                // set a new value
                expectedDouble = levelV - 0.001;
                command = $"_G.ttm.{coldResistanceType}:levelSetter({expectedDouble},_G.ttm.{coldResistanceType}:sourceVoltage())";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

                // restore actual value
                expectedDouble = levelV;
                command = $"_G.ttm.{coldResistanceType}:levelSetter({expectedDouble},_G.ttm.{coldResistanceType}:sourceVoltage())";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

                // get actual value
                query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.limitI))";
                double limitI = session.QueryDoubleThrowIfError( query, "actual voltage source current limit" );

                // set a new value
                expectedDouble = limitI - 0.001;
                command = $"_G.ttm.{coldResistanceType}:limitSetter({expectedDouble},_G.ttm.{coldResistanceType}:sourceVoltage())";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

                // restore actual value
                expectedDouble = limitI;
                command = $"_G.ttm.{coldResistanceType}:limitSetter({expectedDouble},_G.ttm.{coldResistanceType}:sourceVoltage())";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );
            }
            else
            {
                // get actual value
                query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.levelI))";
                double levelI = session.QueryDoubleThrowIfError( query, "actual current source current level" );

                // set a new value
                expectedDouble = levelI - 0.001;
                command = $"_G.ttm.{coldResistanceType}:levelSetter({expectedDouble},_G.ttm.{coldResistanceType}:sourceVoltage())";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

                // restore actual value
                expectedDouble = levelI;
                command = $"_G.ttm.{coldResistanceType}:levelSetter({expectedDouble},_G.ttm.{coldResistanceType}:sourceVoltage())";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

                // get actual value
                query = $"_G.print(string.format('%9.6f',_G.ttm.{coldResistanceType}.limitV))";
                double limitV = session.QueryDoubleThrowIfError( query, "actual current source voltage limit" );

                // set a new value
                expectedDouble = limitV - 0.001;
                command = $"_G.ttm.{coldResistanceType}:limitSetter({expectedDouble},_G.ttm.{coldResistanceType}:sourceVoltage())";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

                // restore actual value
                expectedDouble = limitV;
                command = $"_G.ttm.{coldResistanceType}:limitSetter({expectedDouble},_G.ttm.{coldResistanceType}:sourceVoltage())";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );
            }
        }
    }

    /// <summary>   Assert estimator should reset. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="logEnabled">   (Optional) True to enable, false to disable the log. </param>
    public static void AssertEstimatorShouldReset( Pith.SessionBase? session, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string command, query;
        double expectedDouble;

        cc.isr.VI.Tsp.K2600.Ttm.TtmEstimatorSettings estimatorSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmEstimatorSettings;
        Assert.IsTrue( estimatorSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmEstimatorSettings )} should exist." );

        expectedDouble = estimatorSettings.ThermalCoefficientDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.estimator.Defaults.thermalCoefficient))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        // get actual value
        query = "_G.print(string.format('%9.6f',_G.ttm.est.thermalCoefficient))";
        double thermalCoefficient = session.QueryDoubleThrowIfError( query, "actual thermal coefficient" );

        // set a new value
        expectedDouble = thermalCoefficient + 0.00001;
        command = $"_G.ttm.est:thermalCoefficientSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // restore actual value
        expectedDouble = thermalCoefficient;
        command = $"_G.ttm.est:thermalCoefficientSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );
    }

    /// <summary>   Assert thermal transient defaults should equal settings. </summary>
    /// <remarks>   2025-02-18. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="checkSmuSetter">   (Optional) True to check smu setter. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertThermalTransientDefaultsShouldEqualSettings( Pith.SessionBase? session, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string command, query;
        double expectedDouble;
        int expectedInt;

        cc.isr.VI.Tsp.K2600.Ttm.TtmTraceSettings traceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings;
        cc.isr.VI.Tsp.K2600.Ttm.TtmMeterSettings meterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
        Assert.IsTrue( traceSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings )} should exist." );
        Assert.IsTrue( meterSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings )} should exist." );

        string model = session.ResourceSettings.ResourceModel;
        command = "_G.print(localnode.model)";
        Asserts.AssertQueryReplyShouldBeValid( session, command, model, logEnabled );

        // legacy driver: these need only be tested for the legacy driver.

        if ( MeterSubsystem.LegacyFirmware )
        {
            string smuName = "smua";
            query = $"_G.print('{smuName}' == _G.ttm.trace.Defaults.smuI)";
            Asserts.AssertQueryReplyShouldBeValid( session, query, true, logEnabled );
        }

        expectedDouble = traceSettings.ApertureDefault;
        query = "_G.print(string.format('%8.5f',_G.ttm.trace.Defaults.aperture))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.ApertureMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minAperture))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.ApertureMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxAperture))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.CurrentLevelDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.level))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.limit))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.CurrentMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minCurrent))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.CurrentMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxCurrent))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minVoltage))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxVoltage))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.LowLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.lowLimit))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.HighLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.highLimit))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageChangeMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxVoltageChange))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.001, logEnabled );

        expectedInt = traceSettings.MedianFilterLengthDefault;
        query = "_G.print(string.format('%d',_G.ttm.trace.Defaults.medianFilterSize))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedInt, logEnabled );

        expectedDouble = traceSettings.SamplingIntervalDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.period))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.SamplingIntervalMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minPeriod))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.SamplingIntervalMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxPeriod))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedInt = traceSettings.TracePointsDefault;
        query = "_G.print(string.format('%d',_G.ttm.trace.Defaults.points))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedInt, logEnabled );

        expectedInt = traceSettings.TracePointsMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minPoints))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedInt, logEnabled );

        expectedDouble = traceSettings.LatencyDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.latency))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.000001, logEnabled );
    }

    /// <summary>   Assert thermal transient should reset. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="checkSmuSetter">   (Optional) True to check smu setter. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertThermalTransientShouldReset( Pith.SessionBase? session, bool checkSmuSetter = false, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string command, query;
        bool expectedBoolean;
        double expectedDouble;
        int expectedInt;
        string expectedValue;

        cc.isr.VI.Tsp.K2600.Ttm.TtmTraceSettings traceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings;
        cc.isr.VI.Tsp.K2600.Ttm.TtmMeterSettings meterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;
        Assert.IsTrue( traceSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmTraceSettings )} should exist." );
        Assert.IsTrue( meterSettings.Exists, $"cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.{nameof( cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings )} should exist." );

        string model = session.ResourceSettings.ResourceModel;
        command = "_G.print(localnode.model)";
        Asserts.AssertQueryReplyShouldBeValid( session, command, model, logEnabled );

        // legacy driver: these need only be tested for the legacy driver.

        if ( MeterSubsystem.LegacyFirmware )
        {
            string smuName = "smua";
            query = $"_G.print(_G.localnode.{smuName} == _G.ttm.tr.smuI)";
            if ( !session.QueryBoolThrowIfError( query, $"find {smuName}" ) )
            {
                smuName = "smub";
                query = $"_G.print(_G.localnode.{smuName} == _G.ttm.ts.smuI)";
                Assert.IsTrue( session.QueryBoolThrowIfError( query, $"find {smuName}" ), $"failed finding SMU for thermal transient." );
            }

            if ( checkSmuSetter )
            {
                if ( ("26" == model[..2]) && ("2" == model.Substring( 3, 1 )) )
                {
                    expectedValue = smuName == "smua" ? "smub" : "smua";
                    command = $"_G.ttm.tr:currentSourceChannelSetter({expectedValue})";
                    Asserts.AssertCommandShouldExecute( session, command, logEnabled );

                    expectedBoolean = true;
                    query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.tr.smuI)";
                    Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
                }

                expectedValue = smuName;
                command = $"_G.ttm.tr:currentSourceChannelSetter({expectedValue})";
                Asserts.AssertCommandShouldExecute( session, command, logEnabled );

                expectedBoolean = true;
                query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.tr.smuI)";
                Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
            }
        }

        // get actual value
        query = "_G.print(string.format('%8.5f',_G.ttm.tr.aperture))";
        double aperture = session.QueryDoubleThrowIfError( query, "actual aperture" );

        // set a new value
        expectedDouble = aperture - 0.001;
        command = $"_G.ttm.tr:apertureSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        // restore actual value
        expectedDouble = aperture;
        command = $"_G.ttm.tr:apertureSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        // get actual value
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.level))";
        double level = session.QueryDoubleThrowIfError( query, "actual trace level" );

        // set a new value
        expectedDouble = level + 0.001;
        command = $"_G.ttm.tr:levelSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // restore actual value
        expectedDouble = level;
        command = $"_G.ttm.tr:levelSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // get actual value
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.limit))";
        double limit = session.QueryDoubleThrowIfError( query, "actual trace limit" );

        // set a new value
        expectedDouble = limit + 0.001;
        command = $"_G.ttm.tr:limitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // restore actual value
        expectedDouble = limit;
        command = $"_G.ttm.tr:limitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // get actual value
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.lowLimit))";
        double lowLimit = session.QueryDoubleThrowIfError( query, "actual trace low limit" );

        // set a new value
        expectedDouble = lowLimit + 0.0001;
        command = $"_G.ttm.tr:lowLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // restore actual value
        expectedDouble = lowLimit;
        command = $"_G.ttm.tr:lowLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // get actual value
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.highLimit))";
        double highLimit = session.QueryDoubleThrowIfError( query, "actual trace high limit" );

        // set a new value
        expectedDouble = highLimit - 0.001;
        command = $"_G.ttm.tr:highLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // restore actual value
        expectedDouble = highLimit;
        command = $"_G.ttm.tr:highLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // get actual value
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxVoltageChange))";
        double maxVoltageChange = session.QueryDoubleThrowIfError( query, "actual trace maximum voltage change" );

        // set a new value
        expectedDouble = maxVoltageChange - 0.0001;
        command = $"_G.ttm.tr:maxVoltageChangeSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // restore actual value
        expectedDouble = maxVoltageChange;
        command = $"_G.ttm.tr:maxVoltageChangeSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // get actual value
        query = "_G.print(string.format('%d',_G.ttm.tr.medianFilterSize))";
        int medianFilterSize = session.QueryIntegerThrowIfError( query, "actual trace median filter size" );

        // set a new value
        medianFilterSize = 0 == medianFilterSize ? 3 : medianFilterSize;
        expectedInt = medianFilterSize + 2;
        command = $"_G.ttm.tr:medianFilterSizeSetter({expectedInt})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        // restore actual value
        expectedInt = medianFilterSize;
        command = $"_G.ttm.tr:medianFilterSizeSetter({expectedInt})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        // get actual value
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.period))";
        double period = session.QueryDoubleThrowIfError( query, "actual trace period" );

        // set a new value
        expectedDouble = period + 0.00001;
        command = $"_G.ttm.tr:periodSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // restore actual value
        expectedDouble = period;
        command = $"_G.ttm.tr:periodSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // get actual value
        query = "_G.print(string.format('%d',_G.ttm.tr.points))";
        int points = session.QueryIntegerThrowIfError( query, "actual trace points" );

        // set a new value
        points = 99 >= points ? 100 : points;
        expectedInt = points + 1;
        command = $"_G.ttm.tr:pointsSetter({expectedInt})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        // restore actual value
        expectedInt = points;
        command = $"_G.ttm.tr:pointsSetter({expectedInt})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        // get the max read estimate 
        // set the max
        query = MeterSubsystem.LegacyFirmware
            ? "_G.print(string.format('%9.6f',_G.isr.devices.k2600.estimateMinPeriod(_G.ttm.tr.aperture)))"
            : "_G.print(string.format('%9.6f',_G.isr.smuDevice.estimateMinPeriod(_G.ttm.tr.aperture)))";
        expectedDouble = session.QueryDoubleThrowIfError( query, "estimated min period" );
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.period))";
        command = "_G.ttm.tr:maxRateSetter()";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        // get actual value
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.latency))";
        double latency = session.QueryDoubleThrowIfError( query, "actual trace latency" );

        // set a new value
        expectedDouble = latency + 0.00001;
        command = $"_G.ttm.tr:latencySetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.000001, logEnabled );

        // restore actual value
        expectedDouble = latency;
        command = $"_G.ttm.tr:latencySetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.000001, logEnabled );
    }
}
