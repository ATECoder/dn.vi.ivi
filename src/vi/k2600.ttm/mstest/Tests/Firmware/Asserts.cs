using System.Diagnostics;
using cc.isr.VI.Tsp.SessionBaseExtensions;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Firmware;
/// <summary>   An asserts. </summary>
/// <remarks>   2024-10-31. </remarks>
internal static partial class Asserts
{
    /// <summary>   Gets or sets the legacy driver. </summary>
    /// <voltageChange> The legacy driver. </voltageChange>
    public static int LegacyDriver { get; set; } = 1;

    /// <summary>   (Immutable) the Initial Resistance element. </summary>
    public const string IR = "ir";

    /// <summary>   (Immutable) the final Resistance element. </summary>
    public const string FR = "fr";

    /// <summary>   (Immutable) the trace element. </summary>
    public const string TR = "tr";

    public const string EST = "est";

    /// <summary>   Ttm element name. </summary>
    /// <remarks>   2025-02-06. </remarks>
    /// <param name="ttmElement">   (ir) Type of the cold resistance. </param>
    /// <returns>   A string. </returns>
    public static string TtmElementName( string ttmElement )
    {
        return string.Equals( Asserts.IR, ttmElement, StringComparison.Ordinal )
            ? "Initial Resistance"
            : string.Equals( Asserts.FR, ttmElement, StringComparison.Ordinal )
                ? "Final Resistance"
                : string.Equals( Asserts.TR, ttmElement, StringComparison.Ordinal )
                    ? "Trace"
                    : string.Equals( Asserts.EST, ttmElement, StringComparison.Ordinal )
                        ? "Estimator"
                        : string.Empty;
    }

    /// <summary>   Contact check option. </summary>
    /// <remarks>   2025-02-06. </remarks>
    /// <param name="ttmElement">   (ir) Type of the cold resistance. </param>
    /// <returns>   The ContactCheckOptions. </returns>
    public static ContactCheckOptions ContactCheckOption( string ttmElement )
    {
        return string.Equals( Asserts.IR, ttmElement, StringComparison.Ordinal )
          ? ContactCheckOptions.Initial
          : string.Equals( Asserts.FR, ttmElement, StringComparison.Ordinal )
            ? ContactCheckOptions.Final
            : string.Equals( Asserts.TR, ttmElement, StringComparison.Ordinal )
              ? ContactCheckOptions.Trace
              : ContactCheckOptions.None;
    }

    /// <summary> Logs an iterator. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The voltageChange. </param>
    public static void LogIT( string value )
    {
        Console.Out.WriteLine( value );
    }

    /// <summary>
    /// Primes the instrument to wait for a trigger. This clears the digital outputs and loops until
    /// trigger or <see cref="SessionBase.AssertTrigger"/> command or external *TRG command.
    /// </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="lockLocal">        (Optional) When true, displays the title and locks the local
    ///                                 key. </param>
    /// <param name="onCompleteReply">  (Optional) The operation completion reply. </param>
    public static void PrepareForTrigger( Pith.SessionBase? session, bool lockLocal = false, string onCompleteReply = "OPC" )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"VISA session to '{nameof( session.ResourceNameCaption )}' must be open." );

        session.MessageAvailableBitmask = Pith.ServiceRequests.MessageAvailable;

        session.SetLastAction( "clearing execution state." );
        session.ClearExecutionState();

        session.SetLastAction( "enabling service request on completion." );
        session.EnableServiceRequestOnOperationCompletion();

        session.SetLastAction( "waiting for trigger" );
        _ = session.WriteLine( "prepareForTrigger({0},'{1}') waitcomplete() ",
                lockLocal ? cc.isr.VI.Syntax.Tsp.Lua.TrueValue : cc.isr.VI.Syntax.Tsp.Lua.FalseValue, onCompleteReply );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
    }

    /// <summary>   Assert trigger cycle should start. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="session">                              The session. </param>
    /// <param name="timeout">                              The timeout. </param>
    /// <param name="validateTriggerCycleCompleteMessage">  True to validate trigger cycle complete
    ///                                                     message. </param>
    /// <param name="lockLocal">                            (Optional) When true, displays the title
    ///                                                     and locks the local key. </param>
    /// <param name="onCompleteReply">                      (Optional) The operation completion
    ///                                                     reply. </param>
    public static void AssertTriggerCycleShouldStart( Pith.SessionBase? session, TimeSpan timeout, bool validateTriggerCycleCompleteMessage,
        bool lockLocal = false, string onCompleteReply = "OPC" )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"VISA session to '{nameof( session.ResourceNameCaption )}' must be open." );

        // start the trigger cycle
        Asserts.PrepareForTrigger( session, lockLocal, onCompleteReply );

        //allow some time for the trigger loop to start..
        _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 100 ) );

        // send a trigger command.
        session.AssertTrigger();

        bool messageAvailable = false;
        Stopwatch sw = Stopwatch.StartNew();
        while ( !messageAvailable && sw.Elapsed < timeout )
        {
            _ = Pith.SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 10 ) );
            messageAvailable = session.IsMessageAvailableBitSet( session.ReadStatusByte() );
        }

        Assert.IsTrue( messageAvailable, "The trigger wait should not timeout." );

        if ( validateTriggerCycleCompleteMessage )
        {
            string reading = session.ReadLineTrimEnd();
            Assert.IsFalse( string.IsNullOrEmpty( reading ), $"The trigger cycle completion reply should not be null or empty." );
            Assert.AreEqual( onCompleteReply, reading, "The trigger cycle completion reply should match the expected value" );
        }

        sw.Stop();
        TimeSpan timeSpan = sw.Elapsed;
        Console.WriteLine( $"Trigger Cycle Time: {timeSpan:s\\.fff}s" );
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

        //allow some time for the trigger loop to start..
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
    /// <remarks>   2025-02-07. </remarks>
    /// <param name="session">  The session. </param>
    public static void AssertEstimatesShouldRead( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        string ttmElement = Asserts.EST;
        string ttmElementName = Asserts.TtmElementName( ttmElement );

        int? outcome = session.QueryNullableIntegerThrowIfError( $"print(ttm.{ttmElement}.outcome) ", $"{ttmElementName} outcome" );
        Assert.IsNotNull( outcome, $"{nameof( outcome )} should not be null." );
        double? initialVoltage = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.initialVoltage) ", $"{ttmElementName} Initial Voltage" );
        Assert.IsNotNull( initialVoltage, $"{nameof( initialVoltage )} should not be null." );
        double? finalVoltage = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.finalVoltage) ", $"{ttmElementName} Final Voltage" );
        Assert.IsNotNull( finalVoltage, $"{nameof( finalVoltage )} should not be null." );
        double? voltageChange = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.voltageChange) ", $"{ttmElementName} Voltage Change" );
        Assert.IsNotNull( voltageChange, $"{nameof( voltageChange )} should not be null." );

        if ( ( int ) FirmwareOutcomes.Unknown == (outcome.Value & ( int ) FirmwareOutcomes.Unknown) )
        {
            Assert.AreEqual( 0, initialVoltage, $"{nameof( initialVoltage )} {initialVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, finalVoltage, $"{nameof( finalVoltage )} {finalVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, voltageChange, $"{nameof( voltageChange )} {finalVoltage} should be zero if {nameof( outcome )} is not known." );
        }
        else if ( 0 == initialVoltage )
        {
            // !@# Kludge until we initialize outcome to unknown
            Assert.AreEqual( 0, initialVoltage, $"{nameof( initialVoltage )} {initialVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, finalVoltage, $"{nameof( finalVoltage )} {finalVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, voltageChange, $"{nameof( voltageChange )} {finalVoltage} should be zero if {nameof( outcome )} is not known." );
        }
        else
        {
            Assert.AreNotEqual( 0, initialVoltage, $"{nameof( initialVoltage )} {initialVoltage} should not be zero if {nameof( outcome )} is known." );
            Assert.AreNotEqual( 0, finalVoltage, $"{nameof( finalVoltage )} {finalVoltage} should be zero if {nameof( outcome )} is known." );
            Assert.AreNotEqual( 0, voltageChange, $"{nameof( voltageChange )} {finalVoltage} should be zero if {nameof( outcome )} is known." );
        }
    }

    /// <summary>   Assert estimates should estimate. </summary>
    /// <remarks>   2025-02-07. </remarks>
    /// <param name="session">  The session. </param>
    public static void AssertEstimatesShouldEstimate( Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        string ttmElement = Asserts.EST;
        string ttmElementName = Asserts.TtmElementName( ttmElement );

        int? outcome = session.QueryNullableIntegerThrowIfError( $"print(ttm.{ttmElement}.outcome) ", $"{ttmElementName} outcome" );
        Assert.IsNotNull( outcome, $"{nameof( outcome )} should not be null." );
        double? initialVoltage = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.initialVoltage) ", $"{ttmElementName} Initial Voltage" );
        Assert.IsNotNull( initialVoltage, $"{nameof( initialVoltage )} should not be null." );
        double? finalVoltage = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.finalVoltage) ", $"{ttmElementName} Final Voltage" );
        Assert.IsNotNull( finalVoltage, $"{nameof( finalVoltage )} should not be null." );
        double? voltageChange = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.voltageChange) ", $"{ttmElementName} Voltage Change" );
        Assert.IsNotNull( voltageChange, $"{nameof( voltageChange )} should not be null." );

        double? temperatureChange = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.temperatureChange) ", $"{ttmElementName} Temperature Change" );
        Assert.IsNotNull( temperatureChange, $"{nameof( temperatureChange )} should not be null." );

        // !@# Kludge until we initialize outcome to unknown
        if ( 0 != initialVoltage && 0 == temperatureChange.Value )
        {
            string ttmTrace = Asserts.TR;
            string traceElementName = Asserts.TtmElementName( Asserts.TR );
            int? traceOutcome = session.QueryNullableIntegerThrowIfError( $"print(ttm.{Asserts.TR}.outcome) ", $"{traceElementName} outcome" );
            // the legacy driver does not call the estimator estimate and, therefore, could be tested even if testing compliance with the legacy driver.
            if ( traceOutcome.HasValue && (( int ) FirmwareOutcomes.Unknown != (traceOutcome.Value & ( int ) FirmwareOutcomes.Unknown)) )
            {
                string command = $"ttm.{ttmElement}:estimate( ttm.{ttmTrace} ) ";
                Asserts.AssertCommandShouldExecute( session, command, true );
            }
        }

        temperatureChange = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.temperatureChange) ", $"{ttmElementName} Temperature Change" );
        Assert.IsNotNull( temperatureChange, $"{nameof( temperatureChange )} should not be null." );
        double? thermalConductance = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.thermalConductance) ", $"{ttmElementName} Thermal Conductance" );
        Assert.IsNotNull( thermalConductance, $"{nameof( thermalConductance )} should not be null." );
        double? thermalTimeConstant = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.thermalTimeConstant) ", $"{ttmElementName} Thermal Time Constant" );
        Assert.IsNotNull( thermalTimeConstant, $"{nameof( thermalTimeConstant )} should not be null." );
        double? thermalCapacitance = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.thermalCapacitance) ", $"{ttmElementName} Thermal Capacitance" );
        Assert.IsNotNull( thermalCapacitance, $"{nameof( thermalCapacitance )} should not be null." );
        if ( ( int ) FirmwareOutcomes.Unknown == (outcome.Value & ( int ) FirmwareOutcomes.Unknown) )
        {
            Assert.AreEqual( 0, initialVoltage, $"{nameof( initialVoltage )} {initialVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, finalVoltage, $"{nameof( finalVoltage )} {finalVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, voltageChange, $"{nameof( voltageChange )} {finalVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, temperatureChange, $"{nameof( temperatureChange )} {temperatureChange} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, thermalConductance, $"{nameof( thermalConductance )} {thermalConductance} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, thermalTimeConstant, $"{nameof( thermalTimeConstant )} {thermalTimeConstant} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, thermalCapacitance, $"{nameof( thermalCapacitance )} {thermalCapacitance} should be zero if {nameof( outcome )} is not known." );
        }
        else if ( 0 == initialVoltage )
        {
            // !@# Kludge until we initialize outcome to unknown
            Assert.AreEqual( 0, initialVoltage, $"{nameof( initialVoltage )} {initialVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, finalVoltage, $"{nameof( finalVoltage )} {finalVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, voltageChange, $"{nameof( voltageChange )} {finalVoltage} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, temperatureChange, $"{nameof( temperatureChange )} {temperatureChange} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, thermalConductance, $"{nameof( thermalConductance )} {thermalConductance} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, thermalTimeConstant, $"{nameof( thermalTimeConstant )} {thermalTimeConstant} should be zero if {nameof( outcome )} is not known." );
            Assert.AreEqual( 0, thermalCapacitance, $"{nameof( thermalCapacitance )} {thermalCapacitance} should be zero if {nameof( outcome )} is not known." );
        }
        else
        {
            Assert.AreNotEqual( 0, initialVoltage, $"{nameof( initialVoltage )} {initialVoltage} should not be zero if {nameof( outcome )} is known." );
            Assert.AreNotEqual( 0, finalVoltage, $"{nameof( finalVoltage )} {finalVoltage} should be zero if {nameof( outcome )} is known." );
            Assert.AreNotEqual( 0, voltageChange, $"{nameof( voltageChange )} {finalVoltage} should be zero if {nameof( outcome )} is known." );
            Assert.AreNotEqual( 0, temperatureChange, $"{nameof( temperatureChange )} {temperatureChange} should not be zero if {nameof( outcome )} is known." );
            Assert.AreNotEqual( 0, thermalConductance, $"{nameof( thermalConductance )} {thermalConductance} should not be zero if {nameof( outcome )} is known." );
            Assert.AreNotEqual( 0, thermalTimeConstant, $"{nameof( thermalTimeConstant )} {thermalTimeConstant} should not be zero if {nameof( outcome )} is known." );
            Assert.AreNotEqual( 0, thermalCapacitance, $"{nameof( thermalCapacitance )} {thermalCapacitance} should not be zero if {nameof( outcome )} is known." );
        }
    }


    /// <summary>   Parse pass fail outcome. </summary>
    /// <remarks>   2025-01-27. </remarks>
    /// <param name="passFailOutcome">  The pass fail outcome. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool? Low, bool? High, bool? Pass) ParsePassFailOutcome( int? passFailOutcome )
    {
        if ( !passFailOutcome.HasValue )
            return (new bool?(), new bool?(), new bool?());

        int bitmask = ( int ) PassFailBits.Low;
        bool? low = 0 == passFailOutcome
            ? new bool?()
            : bitmask == (passFailOutcome & bitmask);

        bitmask = ( int ) PassFailBits.High;
        bool? high = 0 == passFailOutcome
            ? new bool?()
            : bitmask == (passFailOutcome & bitmask);

        bitmask = ( int ) PassFailBits.Pass;
        bool? pass = 0 == passFailOutcome
            ? new bool?()
            : bitmask == (passFailOutcome & bitmask);

        return (low, high, pass);
    }


    /// <summary>   Assert TTM Element should read low, high and pass. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="ttmElement">   TTM Element abbreviation, e.g., ir, fr, tr or est. </param>
    /// <returns>   A Tuple. </returns>
    public static (bool? Low, bool? High, bool? Pass) AssertTtmElementShouldReadLowHighPass( Pith.SessionBase? session, string ttmElement )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        Assert.IsNotNull( ttmElement, $"{nameof( ttmElement )} must not be null." );
        Assert.AreNotEqual( string.Empty, ttmElement, $"{nameof( ttmElement )} must not be empty." );
        string ttmElementName = Asserts.TtmElementName( ttmElement );
        Assert.AreNotEqual( string.Empty, ttmElementName, $"Failed getting element name from {nameof( ttmElementName )}='{ttmElementName}'." );
        Assert.IsTrue( string.Equals( Asserts.IR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.FR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.TR, ttmElement, System.StringComparison.Ordinal ),
            $"{nameof( ttmElement )} must be either '{Asserts.IR}' or '{Asserts.FR}' or '{Asserts.TR}'." );

        // the legacy driver is agnostic to the low and high properties and, therefore, could be tested even if testing the legacy driver.
        int? passFailOutcome;
        if ( MeterSubsystem.LegacyFirmware )
        {
            bool? pass = session.QueryNullableBoolThrowIfError( $"print(ttm.{ttmElement}.pass) ", $"{ttmElementName} pass" );
            double lowLimit = session.QueryDoubleThrowIfError( $"print(ttm.{ttmElement}.lowLimit) ", $"{ttmElementName} Low Limit" );
            double highLimit = session.QueryDoubleThrowIfError( $"print(ttm.{ttmElement}.highLimit) ", $"{ttmElementName} High Limit" );
            double value = string.Equals( Asserts.TR, ttmElement, System.StringComparison.Ordinal )
                ? session.QueryDoubleThrowIfError( $"print(ttm.{ttmElement}.voltageChange) ", $"{ttmElementName}" )
                : session.QueryDoubleThrowIfError( $"print(ttm.{ttmElement}.resistance) ", $"{ttmElementName}" );
            passFailOutcome = pass.HasValue
                 ? ( int ) PassFailBits.Pass
                 : value < lowLimit
                   ? ( int ) PassFailBits.Low
                   : value > highLimit
                     ? ( int ) PassFailBits.High
                     : ( int ) PassFailBits.Unknown;
        }
        else
        {
            passFailOutcome = session.QueryNullableIntegerThrowIfError( $"print(ttm.{ttmElement}.passFailOutcome) ", $"{ttmElementName} pass fail outcome" );
            Assert.IsNotNull( passFailOutcome, $"{nameof( passFailOutcome )} should not be null." );
        }
        return Asserts.ParsePassFailOutcome( passFailOutcome );
    }

    /// <summary>   Assert TTM Element should read limits. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">  The session. </param>
    /// <returns>   A Tuple. </returns>
    public static (double LowLimit, double HighLimit) AssertTtmElementShouldReadLimits( Pith.SessionBase? session, string ttmElement )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        Assert.IsNotNull( ttmElement, $"{nameof( ttmElement )} must not be null." );
        Assert.AreNotEqual( string.Empty, ttmElement, $"{nameof( ttmElement )} must not be empty." );
        string ttmElementName = Asserts.TtmElementName( ttmElement );
        Assert.AreNotEqual( string.Empty, ttmElementName, $"Failed getting element name from {nameof( ttmElementName )}='{ttmElementName}'." );
        Assert.IsTrue( string.Equals( Asserts.IR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.FR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.TR, ttmElement, System.StringComparison.Ordinal ),
            $"{nameof( ttmElement )} must be either '{Asserts.IR}' or '{Asserts.FR}' or '{Asserts.TR}'." );

        double lowLimit = session.QueryDoubleThrowIfError( $"print(ttm.{ttmElement}.lowLimit) ", $"{ttmElementName} Low Limit" );
        double highLimit = session.QueryDoubleThrowIfError( $"print(ttm.{ttmElement}.highLimit) ", $"{ttmElementName} High Limit" );
        return (lowLimit, highLimit);
    }

    /// <summary>   Assert TTM Element Measurements should be fetched. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="ttmElement">   TTM Element abbreviation, e.g., ir, fr, tr or est. </param>
    /// <returns>   A Tuple. </returns>
    public static (int? Outcome, int? status, bool okayAndPass, double? Measurement) AssertTtmElementShouldBeFetched( Pith.SessionBase? session, string ttmElement )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        Assert.IsNotNull( ttmElement, $"{nameof( ttmElement )} must not be null." );
        Assert.AreNotEqual( string.Empty, ttmElement, $"{nameof( ttmElement )} must not be empty." );
        string ttmElementName = Asserts.TtmElementName( ttmElement );
        Assert.AreNotEqual( string.Empty, ttmElementName, $"Failed getting element name from {nameof( ttmElementName )}='{ttmElementName}'." );
        Assert.IsTrue( string.Equals( Asserts.IR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.FR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.TR, ttmElement, System.StringComparison.Ordinal ),
            $"{nameof( ttmElement )} must be either '{Asserts.IR}' or '{Asserts.FR}' or '{Asserts.TR}'." );

        bool okayAndPass;
        if ( MeterSubsystem.LegacyFirmware )
        {
            okayAndPass = session.QueryBoolThrowIfError( $"print(ttm.{ttmElement}:isOkay()) ", $"{ttmElementName} Is Okay and Pass" );
        }
        else
        {
            okayAndPass = session.QueryBoolThrowIfError( $"print(ttm.{ttmElement}:isOkayAndPass()) ", $"{ttmElementName} Is Okay and Pass" );
        }
        int? outcome = session.QueryNullableIntegerThrowIfError( $"print(ttm.{ttmElement}.outcome) ", $"{ttmElementName} Measurement Outcome" );
        int? status = session.QueryNullableIntegerThrowIfError( $"print(ttm.{ttmElement}.status) ", $"{ttmElementName} Buffer Status Reading" );
        double? measurement = string.Equals( Asserts.TR, ttmElement, StringComparison.Ordinal )
            ? session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.voltageChange) ", $"{ttmElementName}" )
            : session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.resistance) ", $"{ttmElementName}" );
        return (outcome, status, okayAndPass, measurement);
    }

    /// <summary>   Assert TTM Element Measurements should read contact check. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="ttmElement">   TTM Element abbreviation, e.g., ir, fr, tr or est. </param>
    /// <returns>   A Tuple. </returns>
    public static (int? openLeadsStatus, double? LowLeadsResistance, double? HighLeadsResistance, double? dutResistance) AssertTtmElementShouldReadContactCheck( Pith.SessionBase? session, string ttmElement )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        Assert.IsNotNull( ttmElement, $"{nameof( ttmElement )} must not be null." );
        Assert.AreNotEqual( string.Empty, ttmElement, $"{nameof( ttmElement )} must not be empty." );
        string ttmElementName = Asserts.TtmElementName( ttmElement );
        Assert.AreNotEqual( string.Empty, ttmElementName, $"Failed getting element name from {nameof( ttmElementName )}='{ttmElementName}'." );
        Assert.IsTrue( string.Equals( Asserts.IR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.FR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.TR, ttmElement, System.StringComparison.Ordinal ),
            $"{nameof( ttmElement )} must be either '{Asserts.IR}' or '{Asserts.FR}' or '{Asserts.TR}'." );

        int? openLeadsStatus = session.QueryNullableIntegerThrowIfError( $"print(ttm.{ttmElement}.openLeadsStatus) ", $"{ttmElementName}open leads status" );
        double? low = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.lowLeadsR) ", $"{ttmElementName}low leads contact resistance" );
        double? high = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.highLeadsR) ", $"{ttmElementName}high leads contact resistance" );
        double? dut = session.QueryNullableDoubleThrowIfError( $"print(ttm.{ttmElement}.dutR) ", $"{ttmElementName}DUT leads contact resistance" );
        return (openLeadsStatus, low, high, dut);
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

        return MeterSubsystem.LegacyFirmware
            ? new int?()
            : session.QueryIntegerThrowIfError( $"print(ttm.contactCheckOptionsGetter()) ", "Meter Values Contact Check Options" );
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

        return MeterSubsystem.LegacyFirmware
            ? new double?()
            : session.QueryDoubleThrowIfError( $"print(ttm.contactLimitGetter()) ", "Meter Values Contact Check Options" );
    }

    /// <summary>   Assert contact check should conform. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <param name="outcomes">     The outcomes. </param>
    /// <param name="options">      Options for controlling the operation. </param>
    /// <param name="option">       The option. </param>
    /// <param name="limit">        The limit. </param>
    /// <param name="leadsStatus">  True to okay. </param>
    /// <param name="lowR">         True to low. </param>
    /// <param name="highR">        True to high. </param>
    /// <param name="dutR">         The dut r. </param>
    public static void AssertContactCheckShouldConform( int? outcomes, int? options, ContactCheckOptions option, double? limit,
        int? leadsStatus, double? lowR, double? highR, double? dutR )
    {
        Assert.IsNotNull( options, $"Meter Contact Check {nameof( options )} should not be null." );
        Assert.IsNotNull( limit, $"Leads resistance {nameof( limit )} should not be null." );
        Assert.IsNotNull( outcomes, $"{nameof( outcomes )} must have a value." );
        Assert.IsNotNull( leadsStatus, $"{nameof( leadsStatus )} must have a value." );
        Assert.IsNotNull( lowR, $"{nameof( lowR )} must have a value." );
        Assert.IsNotNull( highR, $"{nameof( highR )} must have a value." );
        Assert.IsNotNull( dutR, $"{nameof( dutR )} must have a value." );

        if ( 0 == (options.Value & ( int ) option) )
        {
            // if the specified option is net enabled the contact check values should be unknown.
            Assert.AreEqual( ( int ) LeadsStatusBits.Unknown, leadsStatus.Value, $"{nameof( leadsStatus )} value {leadsStatus} must be unknow if the options {options} do not include {option}." );
            Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, lowR.Value, 0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber,
                $"{nameof( lowR )} contact resistance value should be unknown if the options {options} do not include {option}." );
            Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, highR.Value, 0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber,
                $"{nameof( highR )} contact resistance value should be unknown if the options {options} do not include {option}." );
            Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, dutR.Value, 0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber,
                $"{nameof( dutR )} contact resistance value should be unknown if the options {options} do not include {option}." );
        }
        else
        {
            if ( ( int ) FirmwareOutcomes.Unknown == (outcomes & ( int ) FirmwareOutcomes.Unknown) )
            {
                // if outcome is not known, measurements were not started.
                Assert.AreEqual( ( int ) LeadsStatusBits.Unknown, leadsStatus.Value, $"{nameof( leadsStatus )} value {leadsStatus} must be unknow if the options {options} do not include {option}." );
                Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, lowR.Value, 0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber,
                    $"{nameof( lowR )} contact resistance value should be unknown if the options {options} do not include {option}." );
                Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, highR.Value, 0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber,
                    $"{nameof( highR )} contact resistance value should be unknown if the options {options} do not include {option}." );
                Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, dutR.Value, 0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber,
                    $"{nameof( dutR )} contact resistance value should be unknown if the options {options} do not include {option}." );
            }
            else if ( ( int ) FirmwareOutcomes.OpenLeads == (outcomes & ( int ) FirmwareOutcomes.OpenLeads) )
            {
                // if leads are open okay should be false and low and high should be higher than the limit
                Assert.AreNotEqual( ( int ) LeadsStatusBits.Okay, leadsStatus, $"{nameof( leadsStatus )} value should not be {LeadsStatusBits.Okay} if contact check did not fail if outcome has no value." );
                Assert.IsNotNull( highR, $"{nameof( highR )} contact resistance should not be null if leads are open." );
                Assert.IsTrue( (lowR.Value > limit.Value) || (highR.Value > limit.Value),
                    $"{nameof( lowR )} ({lowR}) and/or {nameof( highR )} ({highR}) contact values should exceed the contact check threshold {limit}." );
            }
            else
            {
                // if leads are not open okay should be true and low and high should be zero
                Assert.AreEqual( ( int ) LeadsStatusBits.Okay, leadsStatus, $"{nameof( leadsStatus )} value should not be {leadsStatus} if contact check did not fail if outcome has no value." );
                Assert.IsTrue( lowR.Value < limit.Value, $"{nameof( lowR )} ({lowR}) contact value should be lower than the contact check threshold {limit}." );
                Assert.IsTrue( highR.Value < limit.Value, $"{nameof( highR )} ({highR}) contact value should be lower than the contact check threshold {limit}." );
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

                if ( !MeterSubsystem.LegacyFirmware )
                {
                    Assert.IsNotNull( low, $"{nameof( low )} should not be null if outcome {firmwareOutcome} is okay." );
                    Assert.IsNotNull( high, $"{nameof( high )} should not be null if outcome {firmwareOutcome} is okay." );
                    Assert.AreEqual( pass.Value, !(low.Value || high.Value), $"{nameof( pass )} {pass} should equal not (low {low} or high {high})" );

                    Assert.AreEqual( low, measurement < lowLimit,
                        $"{nameof( low )} {low} should equals {measurement < lowLimit} if {nameof( measurement )} {measurement} is less than {nameof( lowLimit )} {lowLimit}." );
                    Assert.AreEqual( high, measurement > highLimit,
                        $"{nameof( high )} {high} should equals {measurement > highLimit} if {nameof( measurement )} {measurement} is higher than {nameof( highLimit )} {highLimit}." );
                }
            }
            else
            {
                Asserts.AssertMeasurementShouldConformOnNotOkay( status, okayAndPass, measurement, low, high, pass );

                // legacy driver is oblivious to contact check but expects NaN if contact check failed, therefore, this needs to be tested.

                if ( !MeterSubsystem.LegacyFirmware )
                {
                    if ( 0 != (firmwareOutcome & cc.isr.VI.Tsp.K2600.Ttm.Syntax.FirmwareOutcomes.OpenLeads) )
                    {
                        // if contact check failed, the measurement should be NaN
                        Assert.IsNotNull( measurement, $"{nameof( measurement )} should have a value after contact check." );
                        Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, measurement.Value, 0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber,
                            $"{nameof( measurement )} should be set to {cc.isr.VI.Syntax.ScpiSyntax.NotANumber} if contact check failed." );
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
        // the TTM initializes pass and measurement to NaN
        Assert.IsTrue( measurement.HasValue, $"'{nameof( measurement )}' should have a value." );
        Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, measurement.Value, 0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber,
            $"'{nameof( measurement )}' should have a SCPI.NAN value." );

        Assert.IsFalse( okayAndPass, $"'{nameof( okayAndPass )}' must not be true if outcome is nil." );

        Assert.IsTrue( pass.HasValue, $"'{nameof( pass )}' should have a value event if measurements were not made." );
        Assert.IsFalse( pass.Value, $"'{nameof( pass )}' should not pass if measurements were not made." );

        Assert.IsTrue( low.HasValue, $"'{nameof( low )}' should have a value event if measurements were not made." );
        Assert.IsFalse( low.Value, $"'{nameof( low )}' should be false if measurements were not made." );

        Assert.IsTrue( high.HasValue, $"'{nameof( high )}' should have a value event if measurements were not made." );
        Assert.IsFalse( high.Value, $"'{nameof( high )}' should be false if measurements were not made." );

        Assert.IsTrue( status.HasValue, $"'{nameof( status )}' should have a value event if measurements were not made." );
        Assert.AreEqual( 1, status, $"'{nameof( status )}' should have a value of 1 if measurements were not made." );
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

    /// <summary>   Assert TTM Element has readings. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <param name="session">              The session. </param>
    /// <param name="ttmElement">   TTM Element abbreviation, e.g., ir, fr, tr or est. </param>
    public static void AssertTtmElementReadings( Pith.SessionBase? session, string ttmElement )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );

        Assert.IsNotNull( ttmElement, $"{nameof( ttmElement )} must not be null." );
        Assert.AreNotEqual( string.Empty, ttmElement, $"{nameof( ttmElement )} must not be empty." );
        string ttmElementName = Asserts.TtmElementName( ttmElement );
        Assert.AreNotEqual( string.Empty, ttmElementName, $"Failed getting element name from {nameof( ttmElementName )}='{ttmElementName}'." );
        Assert.IsTrue( string.Equals( Asserts.IR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.FR, ttmElement, System.StringComparison.Ordinal )
            | string.Equals( Asserts.TR, ttmElement, System.StringComparison.Ordinal ),
            $"{nameof( ttmElement )} must be either '{Asserts.IR}' or '{Asserts.FR}' or '{Asserts.TR}'." );

        (int? outcome, int? status, bool okayAndPass, double? measurement) = Asserts.AssertTtmElementShouldBeFetched( session, ttmElement );
        (bool? low, bool? high, bool? pass) = Asserts.AssertTtmElementShouldReadLowHighPass( session, ttmElement );
        (double lowLimit, double highLimit) = Asserts.AssertTtmElementShouldReadLimits( session, ttmElement );
        Assert.IsNotNull( measurement, $"{nameof( measurement )} must not be null." );

        // scale the NaN which is set as 'volts' because it is scaled up to 'millivolts' for display.
        if ( string.Equals( Asserts.TR, ttmElement, System.StringComparison.Ordinal ) )
        {
            if ( 0.00001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber > Math.Abs( (0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber) - measurement.Value ) )
            {
                measurement *= 1000;
            }
        }

        Asserts.AssertMeasurementShouldConform( outcome, status, okayAndPass, measurement, lowLimit, highLimit, low, high, pass );

        if ( !outcome.HasValue )
            // status is initialized to nil with cold resistance
            Assert.IsFalse( status.HasValue, $"{ttmElementName} '{nameof( status )}' should not have a value if measurements were not made." );

        // the legacy driver is oblivious to contact check but will see the NaN result if contact check failed.

        if ( !MeterSubsystem.LegacyFirmware )
        {
            int? contactOptions = Asserts.AssertMeterShouldReadContactCheckOptions( session );
            double? limit = Asserts.AssertMeterShouldReadContactCheckLimit( session );
            (int? leadsStatus, double? lowR, double? highR, double? dutR) = Asserts.AssertTtmElementShouldReadContactCheck( session, ttmElement );
            Asserts.AssertContactCheckShouldConform( outcome, contactOptions, ContactCheckOptions.Initial, limit, leadsStatus, lowR, highR, dutR );

            ContactCheckOptions contactCheckOption = Asserts.ContactCheckOption( ttmElement );

            if ( ( int ) contactCheckOption == (contactOptions & ( int ) contactCheckOption) )
            {
                if ( leadsStatus.HasValue && (( int ) LeadsStatusBits.Okay != leadsStatus.Value) )
                {
                    // if contact check failed, the measurement reading should be NaN
                    Assert.IsNotNull( measurement, $"{nameof( measurement )} should have a value after contact check." );
                    Assert.AreEqual( cc.isr.VI.Syntax.ScpiSyntax.NotANumber, measurement.Value, 0.001 * cc.isr.VI.Syntax.ScpiSyntax.NotANumber,
                        $"{nameof( measurement )} should be set to NaN if contact check failed." );
                }
            }
        }
    }
}
