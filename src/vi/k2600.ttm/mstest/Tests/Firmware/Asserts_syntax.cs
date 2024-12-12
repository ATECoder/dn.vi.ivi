using System;
using cc.isr.VI.Tsp.SessionBaseExtensions;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;

namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Firmware;
internal static partial class Asserts
{
    /// <summary>   Assert tsp syntax should not fail. </summary>
    /// <remarks>   2024-11-02. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="logEnabled">   (Optional) True to enable, false to disable the log. </param>
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

        expectedValue = Settings.AllSettings.ResourceSettings.ResourceModel;
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

        // this may not work....
        command = "_G.status.reset() _G.status.standard.enable=253 _G.status.request_enable=32 _G.opc()";
        Asserts.AssertCommandShouldExecute( session, command, logEnabled );

        // wait for operation completion bit.
        (bool timedOut, Pith.ServiceRequests status, TimeSpan elapsed) = session.AwaitOperationCompletion( TimeSpan.FromMilliseconds( 100 ) );
        Assert.IsFalse( timedOut, $"wait for operation completion timed out with status {( int ) status:X2} after {elapsed.TotalMilliseconds:0} ms." );

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
    /// <param name="session">      The session. </param>
    /// <param name="logEnabled">   (Optional) True to enable, false to disable the log. </param>
    public static void AssertMeterValueShouldReset( Pith.SessionBase? session, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string query, command;
        bool expectedBoolean;
        double expectedDouble;
        int expectedInt;
        string expectedValue;

        cc.isr.VI.Tsp.K2600.Ttm.TtmMeterSettings meterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;

        expectedBoolean = true;
        query = "_G.print(_G.ttm.loaded())";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );

        double postTransientDelayDefault; // = 0.5;
        query = "_G.print(string.format('%9.3f',_G.ttm.meterDefaults.postTransientDelay))";

        postTransientDelayDefault = session.QueryDoubleThrowIfError( query, "post transient delay" );
        Assert.AreEqual( meterSettings.PostTransientDelayDefault, postTransientDelayDefault, $"{nameof( postTransientDelayDefault )} should equal the settings value." );
        Asserts.LogIT( $"{query} returned {postTransientDelayDefault}" );

        expectedDouble = postTransientDelayDefault + 0.001;
        query = "_G.print(string.format('%9.3f',_G.ttm.postTransientDelayGetter()))";
        command = $"_G.ttm.postTransientDelaySetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        expectedDouble = postTransientDelayDefault;
        command = $"_G.ttm.postTransientDelaySetter(_G.ttm.meterDefaults.postTransientDelay)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        // the legacy driver is agnostic to the meter settings other than the post transient delay and, therefore, could be tested even if testing the legacy driver.

        if ( !Asserts.LegacyFirmware )
        {
            int contactLimit; // 100;
            query = "_G.print(string.format('%d',_G.ttm.meterDefaults.contactLimit))";

            contactLimit = session.QueryIntegerThrowIfError( query, "contact limit" );
            Assert.AreEqual( meterSettings.ContactCheckThresholdDefault, contactLimit, $"{nameof( contactLimit )} should equal the settings value." );
            Asserts.LogIT( $"{query} returned {contactLimit}" );

            expectedInt = contactLimit + 1;
            query = "_G.print(string.format('%d',_G.ttm.contactLimitGetter()))";
            command = $"_G.ttm.contactLimitSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            expectedInt = contactLimit;
            command = $"_G.ttm.contactLimitSetter(_G.ttm.meterDefaults.contactLimit)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            int contactOptions; // = 7;
            query = "_G.print(string.format('%d',_G.ttm.meterDefaults.contactCheckOptions))";
            contactOptions = session.QueryIntegerThrowIfError( query, "contact check options" );
            Assert.AreEqual( ( int ) meterSettings.ContactCheckOptionsDefault, contactOptions, $"{nameof( contactOptions )} should equal the settings value." );
            Asserts.LogIT( $"{query} returned {contactOptions}" );

            expectedInt = contactOptions == 3 ? 7 : 3;
            query = "_G.print(string.format('%d',_G.ttm.contactCheckOptionsGetter()))";
            command = $"_G.ttm.contactCheckOptionsSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            expectedInt = contactOptions;
            command = $"_G.ttm.contactCheckOptionsSetter(_G.ttm.meterDefaults.contactCheckOptions)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            expectedBoolean = ( int ) ContactCheckOptions.Initial == (contactOptions & ( int ) ContactCheckOptions.Initial);
            query = "_G.print(_G.ttm.checkContactsBeforeInitialResistance())";
            Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );

            expectedBoolean = ( int ) ContactCheckOptions.PreTrace == (contactOptions & ( int ) ContactCheckOptions.PreTrace);
            query = "_G.print(_G.ttm.checkContactsBeforeThermalTransient())";
            Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );

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

            expectedInt = legacyDriver;
            command = $"_G.ttm.legacyDriverSetter(_G.ttm.meterDefaults.legacyDriver)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            string model = Settings.AllSettings.ResourceSettings.ResourceModel;
            query = "_G.print(localnode.model)";
            Asserts.AssertQueryReplyShouldBeValid( session, query, model, logEnabled );

            string smuName; // "smua";
            query = "_G.print(_G.ttm.meterDefaults.smuName)";
            smuName = session.QueryStringThrowIfError( query, "smu name" );
            Assert.AreEqual( meterSettings.SourceMeasureUnitDefault, smuName, $"{nameof( smuName )} should equal the settings value." );

            if ( ("26" == model[..2]) && ("2" == model.Substring( 3, 1 )) )
            {
                expectedValue = smuName == "smua" ? "smub" : "smua";
                query = "_G.print(_G.ttm.smuName)";
                command = $"_G.ttm.smuNameSetter({expectedValue})";
                Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedValue, logEnabled );

                expectedBoolean = true;
                query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.smuGetter())";
                Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
            }

            expectedValue = smuName;
            query = "_G.print(_G.ttm.smuName)";
            command = $"_G.ttm.smuNameSetter(_G.ttm.meterDefaults.smuName)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedValue, logEnabled );

            expectedBoolean = true;
            query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.smuGetter())";
            Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
        }
    }

    /// <summary>   Assert cold resistance defaults should be fetched. </summary>
    /// <remarks>   2024-11-02. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="logEnabled">   (Optional) True to enable, false to disable the log. </param>
    public static void AssertColdResistanceDefaultsShouldBeFetched( Pith.SessionBase? session, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string query;
        double expectedDouble;

        cc.isr.VI.Tsp.K2600.Ttm.TtmResistanceSettings resistanceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings;

        expectedDouble = resistanceSettings.CurrentMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.minCurrent))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.0001, logEnabled );

        expectedDouble = resistanceSettings.CurrentMaximum;
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

        expectedDouble = resistanceSettings.VoltageMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.maxVoltage))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.0001, logEnabled );
    }

    /// <summary>   Assert initial resistance commands should execute. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="logEnabled">   (Optional) True to enable, false to disable the log. </param>
    public static void AssertInitialResistanceShouldReset( Pith.SessionBase? session, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        string command, query;
        bool expectedBoolean;
        double expectedDouble;
        int expectedInt;
        string expectedValue;

        cc.isr.VI.Tsp.K2600.Ttm.TtmResistanceSettings resistanceSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmResistanceSettings;
        cc.isr.VI.Tsp.K2600.Ttm.TtmMeterSettings meterSettings = cc.isr.VI.Tsp.K2600.Ttm.Properties.Settings.Instance.TtmMeterSettings;

        string model = Settings.AllSettings.ResourceSettings.ResourceModel;
        command = "_G.print(localnode.model)";
        Asserts.AssertQueryReplyShouldBeValid( session, command, model, logEnabled );

        // legacy driver: these need only be tested for the legacy driver.

        if ( 1 == Asserts.LegacyDriver )
        {
            string smuName; // "smua";
            query = "_G.print(_G.ttm.coldResistance.Defaults.smuI)";
            smuName = session.QueryStringThrowIfError( query, "smu name" );
            Assert.AreEqual( meterSettings.SourceMeasureUnitDefault, smuName, $"{nameof( smuName )} should match settings value." );

            if ( ("26" == model[..2]) && ("2" == model.Substring( 3, 1 )) )
            {
                expectedValue = smuName == "smua" ? "smub" : "smua";
                command = $"_G.ttm.ir:currentSourceChannelSetter({expectedValue})";
                Asserts.AssertCommandShouldExecute( session, command, logEnabled );

                expectedBoolean = true;
                query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.ir.smuI)";
                Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
            }

            expectedValue = smuName;
            command = $"_G.ttm.ir:currentSourceChannelSetter({expectedValue})";
            Asserts.AssertCommandShouldExecute( session, command, logEnabled );

            expectedBoolean = true;
            query = $"_G.print(_G.localnode.{expectedValue} == _G.ttm.ir.smuI)";
            Asserts.AssertQueryReplyShouldBeValid( session, query, expectedBoolean, logEnabled );
        }

        double aperture; // 1.0000;
        query = "_G.print(string.format('%7.4f',_G.ttm.coldResistance.Defaults.aperture))";
        aperture = session.QueryDoubleThrowIfError(query, "default aperture" );
        Assert.AreEqual( resistanceSettings.ApertureDefault, aperture, $"{nameof( aperture )} should match settings value." );

        expectedDouble = aperture + 1;
        query = "_G.print(string.format('%7.4f',_G.ttm.ir.aperture))";
        command = $"_G.ttm.ir:apertureSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        expectedDouble = aperture;
        query = "_G.print(string.format('%7.4f',_G.ttm.ir.aperture))";
        command = "_G.ttm.ir:apertureSetter(_G.ttm.coldResistance.Defaults.aperture)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

        // legacy driver: these need only be tested for the legacy driver.

        if ( 1 == Asserts.LegacyDriver )
        {
            double level; // 0.01;
            query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.level))";
            level = session.QueryDoubleThrowIfError(query, "default current source level" );
            Assert.AreEqual( resistanceSettings.CurrentLevel, level, $"{nameof( level )} should match settings value." );

            expectedDouble = level + 0.001;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.level))";
            command = $"_G.ttm.ir:levelSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

            expectedDouble = level;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.level))";
            command = "_G.ttm.ir:levelSetter(_G.ttm.coldResistance.Defaults.level)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.0001, logEnabled );

            double limit; // 0.1
            query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.limit))";
            limit = session.QueryDoubleThrowIfError(query, "default current source voltage limit" );
            Assert.AreEqual( resistanceSettings.VoltageLimit, limit, $"{nameof( limit )} should match settings value." );

            expectedDouble = limit + 0.01;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.limit))";
            command = $"_G.ttm.ir:limitSetter({expectedDouble})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            expectedDouble = limit;
            command = "_G.ttm.ir:limitSetter(_G.ttm.coldResistance.Defaults.limit)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );
        }

        double lowLimit; // 1.85;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.lowLimit))";
        lowLimit = session.QueryDoubleThrowIfError(query, "default low limit" );
        Assert.AreEqual( resistanceSettings.LowLimitDefault, lowLimit, $"{nameof( lowLimit )} should match settings value." );

        expectedDouble = lowLimit + 0.01;
        query = "_G.print(string.format('%9.6f',_G.ttm.ir.lowLimit))";
        command = $"_G.ttm.ir:lowLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

        expectedDouble = lowLimit;
        query = "_G.print(string.format('%9.6f',_G.ttm.ir.lowLimit))";
        command = "_G.ttm.ir:lowLimitSetter(_G.ttm.coldResistance.Defaults.lowLimit)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

        double highLimit; // 2.156;
        query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.highLimit))";
        highLimit = session.QueryDoubleThrowIfError(query, "default high limit" );
        Assert.AreEqual( resistanceSettings.HighLimitDefault, highLimit, $"{nameof( highLimit )} should match settings value." );

        expectedDouble = highLimit + 0.01;
        query = "_G.print(string.format('%9.6f',_G.ttm.ir.highLimit))";
        command = $"_G.ttm.ir:highLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

        expectedDouble = highLimit;
        query = "_G.print(string.format('%9.6f',_G.ttm.ir.highLimit))";
        command = "_G.ttm.ir:highLimitSetter(_G.ttm.coldResistance.Defaults.highLimit)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

        // legacy driver: agnostic; can test.

        if ( !Asserts.LegacyFirmware )
        {
            int failStatus; // 2;
            query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.failStatus))";
            failStatus = session.QueryIntegerThrowIfError( query, "default fail StatusReading" );
            Assert.AreEqual( resistanceSettings.FailStatus, failStatus, $"{nameof( highLimit )} should match settings value." );

            expectedInt = failStatus == 2 ? 66 : 2;
            query = "_G.print(string.format('%d',_G.ttm.ir.failStatus))";
            command = $"_G.ttm.ir:failStatusSetter({expectedInt})";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

            expectedInt = failStatus;
            command = "_G.ttm.ir:failStatusSetter(_G.ttm.coldResistance.Defaults.failStatus)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );
        }

        // legacy driver: agnostic; can test.

        if ( !Asserts.LegacyFirmware )
        {

            double levelI; // 0.01;
            query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.levelI))";
            levelI = session.QueryDoubleThrowIfError(query, "default current source current level" );
            Assert.AreEqual( resistanceSettings.CurrentLevel, levelI, $"{nameof( levelI )} should match settings value." );

            double levelV; // 0.02 ;
            query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.levelV))";
            levelV = session.QueryDoubleThrowIfError(query, "default voltage source voltage level" );
            Assert.AreEqual( resistanceSettings.VoltageLevel, levelV, $"{nameof( levelV )} should match settings value." );

            double limitI; // 0.04;
            query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.limitI))";
            limitI = session.QueryDoubleThrowIfError(query, "default voltage source current limit" );
            Assert.AreEqual( resistanceSettings.CurrentLimit, limitI, $"{nameof( limitI )} should match settings value." );

            double limitV; // 0.1;
            query = "_G.print(string.format('%9.6f',_G.ttm.coldResistance.Defaults.limitV))";
            limitV = session.QueryDoubleThrowIfError(query, "default current source voltage limit" );
            Assert.AreEqual( resistanceSettings.VoltageLimit, limitV, $"{nameof( limitV )} should match settings value." );

            bool sourceVoltage = session.QueryBoolThrowIfError("_G.ttm.ir:sourceVoltage()", "source voltage method" );
            Assert.AreEqual( resistanceSettings.SourceOutput, sourceVoltage ? SourceOutputOption.Voltage : SourceOutputOption.Current, $"{nameof( sourceVoltage )} should match settings value." );

            expectedDouble = levelV + 0.01;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.levelV))";
            command = $"_G.ttm.ir:levelSetter({expectedDouble},_G.ttm.ir:sourceVoltage())";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            expectedDouble = levelV;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.levelV))";
            command = "_G.ttm.ir:levelSetter(_G.ttm.coldResistance.Defaults.levelV,_G.ttm.ir:sourceVoltage())";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            expectedDouble = limitI + 0.01;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.limitI))";
            command = $"_G.ttm.ir:limitSetter({expectedDouble},_G.ttm.ir:sourceVoltage())";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            expectedDouble = limitI;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.limitI))";
            command = "_G.ttm.ir:limitSetter(_G.ttm.coldResistance.Defaults.limitI,_G.ttm.ir:sourceVoltage())";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            expectedBoolean = !sourceVoltage;
            query = "_G.print(_G.ttm.ir:sourceVoltage())";
            command = "_G.ttm.ir:sourceOutputSetter(_G.ttm.SourceOutputs.current)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedBoolean, logEnabled );

            expectedDouble = levelI + 0.01;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.levelI))";
            command = $"_G.ttm.ir:levelSetter({expectedDouble},_G.ttm.ir:sourceVoltage())";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            expectedDouble = levelI;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.levelI))";
            command = "_G.ttm.ir:levelSetter(_G.ttm.coldResistance.Defaults.levelI,_G.ttm.ir:sourceVoltage())";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            expectedDouble = limitV + 0.01;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.limitV))";
            command = $"_G.ttm.ir:limitSetter({expectedDouble},_G.ttm.ir:sourceVoltage())";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            expectedDouble = limitV;
            query = "_G.print(string.format('%9.6f',_G.ttm.ir.limitV))";
            command = "_G.ttm.ir:limitSetter(_G.ttm.coldResistance.Defaults.limitV,_G.ttm.ir:sourceVoltage())";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.001, logEnabled );

            expectedBoolean = sourceVoltage;
            query = "_G.print(_G.ttm.ir:sourceVoltage())";
            command = "_G.ttm.ir:sourceOutputSetter(_G.ttm.SourceOutputs.voltage)";
            Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedBoolean, logEnabled );
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

        expectedDouble = estimatorSettings.ThermalCoefficientDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.estimator.Defaults.thermalCoefficient))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = 0.00051;
        query = "_G.print(string.format('%9.6f',_G.ttm.est.thermalCoefficient))";
        command = $"_G.ttm.est:thermalCoefficientSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = estimatorSettings.ThermalCoefficientDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.est.thermalCoefficient))";
        command = "_G.ttm.est:thermalCoefficientSetter(_G.ttm.estimator.Defaults.thermalCoefficient)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );
    }

    /// <summary>   Assert thermal transient should reset. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="logEnabled">   (Optional) True to enable, false to disable the log. </param>
    public static void AssertThermalTransientShouldReset( Pith.SessionBase? session, bool logEnabled = false )
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

        string model = Settings.AllSettings.ResourceSettings.ResourceModel;
        command = "_G.print(localnode.model)";
        Asserts.AssertQueryReplyShouldBeValid( session, command, model, logEnabled );

        // legacy driver: these need only be tested for the legacy driver.

        if ( 1 == Asserts.LegacyDriver )
        {
            string smuName; // "smua";
            query = "_G.print(_G.ttm.coldResistance.Defaults.smuI)";
            smuName = session.QueryStringThrowIfError( query, "smu name" );
            Assert.AreEqual( meterSettings.SourceMeasureUnitDefault, smuName, $"{nameof( smuName )} should match settings value." );

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

        expectedDouble = traceSettings.ApertureDefault;
        query = "_G.print(string.format('%7.4f',_G.ttm.trace.Defaults.aperture))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.0001;
        query = "_G.print(string.format('%7.4f',_G.ttm.tr.aperture))";
        command = $"_G.ttm.tr:apertureSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.ApertureDefault;
        query = "_G.print(string.format('%7.4f',_G.ttm.tr.aperture))";
        command = "_G.ttm.tr:apertureSetter(_G.ttm.trace.Defaults.aperture)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.ApertureMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minAperture))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.0001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minAperture))";
        command = $"_G.ttm.tr:minApertureSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.ApertureMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minAperture))";
        command = "_G.ttm.tr:minApertureSetter(_G.ttm.trace.Defaults.minAperture)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.ApertureMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxAperture))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.0001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxAperture))";
        command = $"_G.ttm.tr:maxApertureSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.ApertureMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxAperture))";
        command = "_G.ttm.tr:maxApertureSetter(_G.ttm.trace.Defaults.maxAperture)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.CurrentLevelDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.level))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.level))";
        command = $"_G.ttm.tr:levelSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.CurrentLevelDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.level))";
        command = "_G.ttm.tr:levelSetter(_G.ttm.trace.Defaults.level)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.limit))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.limit))";
        command = $"_G.ttm.tr:limitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.limit))";
        command = "_G.ttm.tr:limitSetter(_G.ttm.trace.Defaults.limit)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.CurrentMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minCurrent))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minCurrent))";
        command = $"_G.ttm.tr:minCurrentSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.CurrentMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minCurrent))";
        command = "_G.ttm.tr:minCurrentSetter(_G.ttm.trace.Defaults.minCurrent)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.CurrentMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxCurrent))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.0001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxCurrent))";
        command = $"_G.ttm.tr:maxCurrentSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.CurrentMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxCurrent))";
        command = "_G.ttm.tr:maxCurrentSetter(_G.ttm.trace.Defaults.maxCurrent)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minVoltage))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minVoltage))";
        command = $"_G.ttm.tr:minVoltageSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minVoltage))";
        command = "_G.ttm.tr:minVoltageSetter(_G.ttm.trace.Defaults.minVoltage)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxVoltage))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxVoltage))";
        command = $"_G.ttm.tr:maxVoltageSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxVoltage))";
        command = "_G.ttm.tr:maxVoltageSetter(_G.ttm.trace.Defaults.maxVoltage)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.LowLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.lowLimit))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.0001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.lowLimit))";
        command = $"_G.ttm.tr:lowLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.LowLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.lowLimit))";
        command = "_G.ttm.tr:lowLimitSetter(_G.ttm.trace.Defaults.lowLimit)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.HighLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.highLimit))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.highLimit))";
        command = $"_G.ttm.tr:highLimitSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.HighLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.highLimit))";
        command = "_G.ttm.tr:highLimitSetter(_G.ttm.trace.Defaults.highLimit)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageChangeMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxVoltageChange))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.0001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxVoltageChange))";
        command = $"_G.ttm.tr:maxVoltageChangeSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.VoltageChangeMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxVoltageChange))";
        command = "_G.ttm.tr:maxVoltageChangeSetter(_G.ttm.trace.Defaults.maxVoltageChange)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedInt = traceSettings.MedianFilterLengthDefault;
        query = "_G.print(string.format('%d',_G.ttm.trace.Defaults.medianFilterSize))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedInt, logEnabled );

        expectedInt += 2;
        query = "_G.print(string.format('%d',_G.ttm.tr.medianFilterSize))";
        command = $"_G.ttm.tr:medianFilterSizeSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        expectedInt = traceSettings.MedianFilterLengthDefault;
        query = "_G.print(string.format('%d',_G.ttm.tr.medianFilterSize))";
        command = "_G.ttm.tr:medianFilterSizeSetter(_G.ttm.trace.Defaults.medianFilterSize)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        expectedDouble = traceSettings.SamplingIntervalDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.period))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.00001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.period))";
        command = $"_G.ttm.tr:periodSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.SamplingIntervalDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.period))";
        command = "_G.ttm.tr:periodSetter(_G.ttm.trace.Defaults.period)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.SamplingIntervalMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minPeriod))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.000001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minPeriod))";
        command = $"_G.ttm.tr:minPeriodSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.SamplingIntervalMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minPeriod))";
        command = "_G.ttm.tr:minPeriodSetter(_G.ttm.trace.Defaults.minPeriod)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.SamplingIntervalMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxPeriod))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble += 0.0001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxPeriod))";
        command = $"_G.ttm.tr:maxPeriodSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.SamplingIntervalMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxPeriod))";
        command = "_G.ttm.tr:maxPeriodSetter(_G.ttm.trace.Defaults.maxPeriod)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedInt = traceSettings.TracePointsDefault;
        query = "_G.print(string.format('%d',_G.ttm.trace.Defaults.points))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedInt, logEnabled );

        expectedInt += 1;
        query = "_G.print(string.format('%d',_G.ttm.tr.points))";
        command = $"_G.ttm.tr:pointsSetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        expectedInt = traceSettings.TracePointsDefault;
        query = "_G.print(string.format('%d',_G.ttm.tr.points))";
        command = "_G.ttm.tr:pointsSetter(_G.ttm.trace.Defaults.points)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        expectedInt = traceSettings.TracePointsMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.minPoints))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedInt, logEnabled );

        expectedInt += 1;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minPoints))";
        command = $"_G.ttm.tr:minPointsSetter({expectedInt})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        expectedInt = traceSettings.TracePointsMinimum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.minPoints))";
        command = "_G.ttm.tr:minPointsSetter(_G.ttm.trace.Defaults.minPoints)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        expectedInt = traceSettings.TracePointsMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.maxPoints))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedInt, logEnabled );

        expectedInt += 1;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxPoints))";
        command = $"_G.ttm.tr:maxPointsSetter({expectedInt})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        expectedInt = traceSettings.TracePointsMaximum;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.maxPoints))";
        command = "_G.ttm.tr:maxPointsSetter(_G.ttm.trace.Defaults.maxPoints)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedInt, logEnabled );

        // get the max read estimate 
        // set the max
        expectedDouble = session.QueryDoubleThrowIfError(
            "_G.print(string.format('%9.6f',_G.isr.smuDevice.estimateMinPeriod(_G.ttm.tr.aperture))", "estimated min period" );
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.period))";
        command = "_G.ttm.tr:maxRateSetter()";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.00001, logEnabled );

        expectedDouble = traceSettings.LowLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.trace.Defaults.latency))";
        Asserts.AssertQueryReplyShouldBeValid( session, query, expectedDouble, 0.000001, logEnabled );

        expectedDouble += 0.000001;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.latency))";
        command = $"_G.ttm.tr:latencySetter({expectedDouble})";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.000001, logEnabled );

        expectedDouble = traceSettings.LowLimitDefault;
        query = "_G.print(string.format('%9.6f',_G.ttm.tr.latency))";
        command = "_G.ttm.tr:latencySetter(_G.ttm.trace.Defaults.latency)";
        Asserts.AssertSetterQueryReplyShouldBeValid( session, command, query, expectedDouble, 0.000001, logEnabled );
    }
}
