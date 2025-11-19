// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Enums;
using cc.isr.Std.Logging;
using cc.isr.Std.Logging.ILoggerExtensions;
using cc.isr.Std.Listeners;

namespace cc.isr.VI.Device.Tests.Base;

/// <summary> The abstract parse tests. </summary>
/// <remarks>
/// (c) 2017 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2017-10-11 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class ParseTests
{
    #region " construction and cleanup "

    /// <summary>   Initializes the test class before running the first test. </summary>
    /// <remarks>
    /// Use <see cref="InitializeTestClass(TestContext)"/> to run code before running the first test
    /// in the class.
    /// </remarks>
    /// <param name="testContext">  Gets or sets the test context which provides information about
    ///                             and functionality for the current test run. </param>
    public static void InitializeBaseTestClass( TestContext testContext )
    {
        string methodFullName = $"{testContext.FullyQualifiedTestClassName}.{System.Reflection.MethodBase.GetCurrentMethod()?.Name}";
        try
        {
            // initialize the logger.
            _ = Logger?.BeginScope( methodFullName );
        }
        catch ( Exception ex )
        {
            if ( Logger is null )
                Trace.WriteLine( $"Failed initializing the test class: {ex}", methodFullName );
            else
                Logger.LogExceptionMultiLineMessage( "Failed initializing the test class:", ex );

            // cleanup to meet strong guarantees

            try
            {
                CleanupBaseTestClass();
            }
            finally
            {
            }
        }
    }

    /// <summary> Cleans up the test class after all tests in the class have run. </summary>
    /// <remarks> Use <see cref="CleanupTestClass"/> to run code after all tests in the class have run. </remarks>
    public static void CleanupBaseTestClass()
    { }

    private IDisposable? _loggerScope;

    /// <summary>   Gets or sets the trace listener. </summary>
    /// <value> The trace listener. </value>
    public LoggerTraceListener<ParseTests>? TraceListener { get; set; }

    /// <summary> Initializes the trace listener and logger scope. </summary>
    /// <remarks> call this method from the subclass. </remarks>
    public void DefineTraceListener()
    {
        if ( ParseTests.Logger is not null && this.TraceListener is null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<ParseTests>( ParseTests.Logger );
            _ = Trace.Listeners.Add( this.TraceListener );
            Asserts.DefineTraceListener( this.TraceListener! );
        }
    }

    /// <summary> Initializes the test class instance before each test runs. </summary>
    public virtual void InitializeBeforeEachTest()
    {
        this.DefineTraceListener();

        // assert reading of test settings from the configuration file.
        Assert.IsNotNull( this.LocationSettings );
        Assert.IsTrue( this.LocationSettings.Exists );
        double expectedUpperLimit = 12d;
        Assert.IsLessThan( expectedUpperLimit,
            Math.Abs( this.LocationSettings.TimeZoneOffset() ), $"{nameof( this.LocationSettings.TimeZoneOffset )} should be lower than {expectedUpperLimit}" );

        if ( Logger is not null )
        {
            this._loggerScope = Logger.BeginScope( this.TestContext?.TestName ?? string.Empty );
            this.TraceListener = new LoggerTraceListener<ParseTests>( Logger );
            _ = Trace.Listeners.Add( this.TraceListener );
        }
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>
    public virtual void CleanupAfterEachTest()
    {
        Assert.IsNotNull( this.TraceListener );
        Assert.IsFalse( this.TraceListener.Any( TraceEventType.Error ),
            $"{nameof( this.TraceListener )} should have no {TraceEventType.Error} messages:" +
            $"{Environment.NewLine}{string.Join( Environment.NewLine, [.. this.TraceListener!.Messages[TraceEventType.Error].ToArray()] )}" );
        this._loggerScope?.Dispose();
        this.TraceListener?.Dispose();
        Trace.Listeners.Clear();
    }

    /// <summary>
    /// Gets or sets the test context which provides information about and functionality for the
    /// current test run.
    /// </summary>
    /// <value> The test context. </value>
    public TestContext? TestContext { get; set; }

    /// <summary>   Gets a logger instance for this category. </summary>
    /// <value> The logger. </value>
    public static ILogger<ParseTests>? Logger { get; } = LoggerProvider.CreateLogger<ParseTests>();

    #endregion

    #region " settings "

    /// <summary>   Gets or sets the location settings. </summary>
    /// <value> The location settings. </value>
    protected Json.AppSettings.Settings.LocationSettings? LocationSettings { get; set; }

    #endregion

    #region " register value format provider "

    /// <summary> (Unit Test Method) tests hexadecimal value format provider. </summary>
    [TestMethod()]
    public void HexValueFormatProviderTest()
    {
        Pith.HexFormatProvider provider = Pith.HexFormatProvider.FormatProvider( 2 );
        Pith.ServiceRequests expectedValue = Pith.ServiceRequests.All & ~Pith.ServiceRequests.RequestingService;
        string expectedCaption = $"0x{( int ) expectedValue:X2}";
        string actualCaption = provider.Format( expectedValue );
        Assert.AreEqual( expectedCaption, actualCaption, $"Should be the hexadecimal caption of {expectedValue}" );

        Pith.ServiceRequests actualValue = provider.ParseEnumHexValue<Pith.ServiceRequests>( actualCaption ).GetValueOrDefault( Pith.ServiceRequests.None );
        Assert.AreEqual( expectedValue, actualValue, $"Should be the hexadecimal value parsed value {expectedValue}" );

        expectedCaption = "0x..";
        actualCaption = provider.NullValueCaption;
        Assert.AreEqual( expectedCaption, actualCaption, $"Should be the null caption" );

        provider = Pith.HexFormatProvider.FormatProvider( 4 );
        expectedValue = Pith.ServiceRequests.All & ~Pith.ServiceRequests.RequestingService;
        expectedCaption = $"0x{( int ) expectedValue:X4}";
        actualCaption = provider.Format( expectedValue );
        Assert.AreEqual( expectedCaption, actualCaption, $"Should be the hexadecimal caption of {expectedValue}" );

        actualValue = provider.ParseEnumHexValue<Pith.ServiceRequests>( actualCaption ).GetValueOrDefault( Pith.ServiceRequests.None );
        Assert.AreEqual( expectedValue, actualValue, $"Should be the hexadecimal value parsed value {expectedValue}" );

        expectedCaption = "0x....";
        actualCaption = provider.NullValueCaption;
        Assert.AreEqual( expectedCaption, actualCaption, $"Should be the null caption" );
    }

    #endregion

    #region " parse: enum; boolean "

    /// <summary> (Unit Test Method) Assert that nullable value should correctly equate. </summary>
    /// <remarks> David, 2020-10-28. </remarks>
    [TestMethod( DisplayName = "01. Nullable values should equate" )]
    public void NullableValuesShouldEquate()
    {
        Assert.AreEqual( new bool?(), new bool?(), "? New Boolean? = New Boolean?" );
        Assert.AreNotEqual( new bool?( true ), new bool?(), "? New Boolean?(True) = New Boolean?" );
        // VB.Net: Assert.IsNull(new bool?() == new bool?(), "? Null = New Boolean? = New Boolean?");
        Assert.AreEqual( new bool?(), new bool?(), "? New Boolean? = New Boolean?" );
    }

    /// <summary> A test for ParseEnumValue. </summary>
    /// <param name="value">    The value. </param>
    /// <param name="expected"> The expected. </param>
    public static void ParseEnumValueTestHelper<T>( string value, T? expected ) where T : struct
    {
        T? actual;
        actual = Pith.SessionBase.ParseEnumValue<T>( value );
        Assert.AreEqual( expected, actual );
    }

    /// <summary> (Unit Test Method) assert that enum values should parse. </summary>
    [TestMethod( DisplayName = "02. Enum value should parse" )]
    public void EnumValueShouldParse()
    {
        ParseEnumValueTestHelper<TraceEventType>( "2", TraceEventType.Error );
    }

    /// <summary> (Unit Test Method) assert that boolean values should parse. </summary>
    [TestMethod( DisplayName = "03. Boolean values should parse" )]
    public void BooleanValuesShouldParse()
    {
        string reading = "0";
        bool expectedResult = false;
        bool successParsing = Pith.SessionBase.TryParse( reading, out bool actualResult );
        Assert.AreEqual( expectedResult, actualResult, $"Value set to {actualResult}" );
        Assert.IsTrue( successParsing, $"Success set to {actualResult}" );
        reading = "1";
        expectedResult = true;
        successParsing = Pith.SessionBase.TryParse( reading, out actualResult );
        Assert.AreEqual( expectedResult, actualResult, $"Value set to {actualResult}" );
        Assert.IsTrue( successParsing, $"Success set to {actualResult}" );
    }

    /// <summary> (Unit Test Method) assert that enum names function should return the expected values. </summary>
    /// <remarks> David, 2020-10-28. </remarks>
    [TestMethod( DisplayName = "04. Enum names should return expected value" )]
    public void EnumNamesShouldReturnExpectedValue()
    {
        TraceEventType traceEvent = TraceEventType.Verbose;

        string expectedValue = "Verbose";
        string actualValue = traceEvent.Names();
        Assert.AreEqual( expectedValue, actualValue, $"{nameof( EnumExtensions.Names )} of {nameof( TraceEventType )}.{nameof( TraceEventType.Verbose )} should match" );
        ArmSources armSource = ArmSources.Bus;

        expectedValue = "Bus";
        actualValue = armSource.Names();
        Assert.AreEqual( expectedValue, actualValue, $"{nameof( EnumExtensions.Names )} of {nameof( ArmSources )}.{nameof( ArmSources.Bus )} should match" );

        expectedValue = "Bus (BUS)";
        actualValue = armSource.Description();
        Assert.AreEqual( expectedValue, actualValue, $"{nameof( EnumExtensions.Description )} of {nameof( ArmSources )}.{nameof( ArmSources.Bus )} should match" );

        expectedValue = "Bus (BUS)";
        actualValue = armSource.Descriptions();
        Assert.AreEqual( expectedValue, actualValue, $"{nameof( EnumExtensions.Descriptions )} of {nameof( ArmSources )}.{nameof( ArmSources.Bus )} should match" );
        armSource = ArmSources.Bus | ArmSources.External;

        expectedValue = "Bus, External";
        actualValue = armSource.Names();
        Assert.AreEqual( expectedValue, actualValue, $"{nameof( EnumExtensions.Names )} of {nameof( ArmSources )}.({nameof( ArmSources.Bus )} or {nameof( ArmSources.External )}) should match" );

        expectedValue = "Bus (BUS), External (EXT)";
        actualValue = armSource.Descriptions();
        Assert.AreEqual( expectedValue, actualValue, $"{nameof( EnumExtensions.Descriptions )} of {nameof( ArmSources )}.({nameof( ArmSources.Bus )} or {nameof( ArmSources.External )}) should match" );
    }

    #endregion

    #region " enum read write "

    /// <summary> Tests unknown enum read write value. </summary>
    /// <param name="enumReadWrites">    The enum read writes. </param>
    /// <param name="expectedEnumValue"> The expected enum value. </param>
    private static void TestUnknownEnumReadWriteValue( Pith.EnumReadWriteCollection enumReadWrites, long expectedEnumValue )
    {
        Assert.IsFalse( enumReadWrites.Exists( expectedEnumValue ), $"{expectedEnumValue} should Not exist" );
        _ = Assert.ThrowsExactly<KeyNotFoundException>( () => enumReadWrites.SelectItem( expectedEnumValue ), $"{expectedEnumValue} enum value Is out of range" );
    }

    /// <summary> Tests unknown enum read write value. </summary>
    /// <param name="enumReadWrites">    The enum read writes. </param>
    /// <param name="expectedReadValue"> The expected read value. </param>
    private static void TestUnknownEnumReadWriteValue( Pith.EnumReadWriteCollection enumReadWrites, string expectedReadValue )
    {
        Assert.IsFalse( enumReadWrites.Exists( expectedReadValue ), $"{expectedReadValue} should Not exist" );
        _ = Assert.ThrowsExactly<KeyNotFoundException>( () => enumReadWrites.SelectItem( expectedReadValue ), $"{expectedReadValue} read value should Not exist" );
    }

    /// <summary> Tests enum read write value. </summary>
    /// <param name="enumReadWrites">    The enum read writes. </param>
    /// <param name="expectedEnumValue"> The expected enum value. </param>
    /// <param name="expectedReadValue"> The expected read value. </param>
    private static void TestEnumReadWriteValue( Pith.EnumReadWriteCollection enumReadWrites, long expectedEnumValue, string expectedReadValue )
    {
        Assert.IsTrue( enumReadWrites.Exists( expectedEnumValue ), $"{expectedEnumValue} exists" );
        Assert.IsTrue( enumReadWrites.Exists( expectedReadValue ), $"{expectedReadValue} exists" );
        Pith.EnumReadWrite enumReadWrite = enumReadWrites.SelectItem( expectedEnumValue );
        Assert.AreEqual( expectedEnumValue, enumReadWrite.EnumValue, "expected item equals item selected from the collection" );
        Assert.AreEqual( expectedReadValue, enumReadWrite.ReadValue, "expected read value equals read value selected from the collection" );
    }

    /// <summary> (Unit Test Method) Assert that enum read write values should be selected. </summary>
    [TestMethod( DisplayName = "05. Enum read write values should be selected" )]
    public void EnumReadWriteValuesShouldBeSelected()
    {
        Pith.EnumReadWriteCollection enumReadWrites = [];
        MultimeterSubsystemBase.DefineFunctionModeReadWrites( enumReadWrites );
        long expectedEnumValue = 1000L;
        string expectedReadValue = "xx";
        TestUnknownEnumReadWriteValue( enumReadWrites, expectedEnumValue );
        TestUnknownEnumReadWriteValue( enumReadWrites, expectedReadValue );
        expectedEnumValue = ( long ) MultimeterFunctionModes.VoltageDC;
        expectedReadValue = "dmm.FUNC_DC_VOLTAGE";
        TestEnumReadWriteValue( enumReadWrites, expectedEnumValue, expectedReadValue );
        long removedEnumValue = ( long ) MultimeterFunctionModes.ResistanceCommonSide;
        _ = enumReadWrites.RemoveAt( removedEnumValue );
        TestUnknownEnumReadWriteValue( enumReadWrites, removedEnumValue );

        // check the diminished collection
        TestEnumReadWriteValue( enumReadWrites, expectedEnumValue, expectedReadValue );
    }

    #endregion

    #region " buffer readings tests "

    /// <summary> (Unit Test Method) Assert that a buffer reading should parse to unit amount. </summary>
    [TestMethod( DisplayName = "06. Buffer reading should parse to unit amount" )]
    public void BufferReadingShouldToUnitAmount()
    {
        double expectedValue = 1.234d;
        UnitsAmounts.Unit expectedUnit = UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        string readingValue = $"{expectedValue}VDC";
        UnitsAmounts.Amount expectedAmount = new( expectedValue, expectedUnit );
        UnitsAmounts.Amount actualAmount = Syntax.ScpiSyntax.ToAmount( readingValue );
        Assert.AreEqual( expectedAmount, actualAmount );
    }

    #endregion

    #region " bitmaps tests "

    /// <summary> Define measurement events bitmasks. </summary>
    /// <param name="bitmaskDictionary"> The bitmask dictionary. </param>
    public static void DefineBitmasks( MeasurementEventsBitmaskDictionary bitmaskDictionary )
    {
        Assert.IsNotNull( bitmaskDictionary );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.ReadingOverflow, 1 << 0 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.LowLimit1, 1 << 1 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.HighLimit1, 1 << 2 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.LowLimit2, 1 << 3 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.HighLimit2, 1 << 4 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.ReadingAvailable, 1 << 5 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.BufferAvailable, 1 << 7 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.BufferHalfFull, 1 << 8 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.BufferFull, 1 << 9 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.BufferPreTriggered, 1 << 11 );
        bitmaskDictionary.Add( MeasurementEventBitmaskKey.Questionable, 1 << 31, true );
    }

    /// <summary> Define operation event bitmasks. </summary>
    /// <param name="bitmaskDictionary"> The bitmask dictionary. </param>
    public static void DefineBitmasks( OperationEventsBitmaskDictionary bitmaskDictionary )
    {
        Assert.IsNotNull( bitmaskDictionary );
        bitmaskDictionary.Add( OperationEventBitmaskKey.Arming, 1 << 6 );
        bitmaskDictionary.Add( OperationEventBitmaskKey.Calibrating, 1 << 0 );
        bitmaskDictionary.Add( OperationEventBitmaskKey.Idle, 1 << 10 );
        bitmaskDictionary.Add( OperationEventBitmaskKey.Measuring, 1 << 4 );
        bitmaskDictionary.Add( OperationEventBitmaskKey.Triggering, 1 << 5 );
    }

    /// <summary> Define questionable event bitmasks. </summary>
    /// <param name="bitmaskDictionary"> The bitmask dictionary. </param>
    public static void DefineBitmasks( QuestionableEventsBitmaskDictionary bitmaskDictionary )
    {
        Assert.IsNotNull( bitmaskDictionary );
        bitmaskDictionary.Add( QuestionableEventBitmaskKey.CalibrationSummary, 1 << 4 );
        bitmaskDictionary.Add( QuestionableEventBitmaskKey.CommandWarning, 1 << 8 );
        // bitmaskDictionary.Add(QuestionableEventBitmask.TemperatureSummary, 1 << 14)
    }

    /// <summary> (Unit Test Method) Assert that the bitmap dictionary should build. </summary>
    [TestMethod( DisplayName = "07. Bitmap dictionary should build" )]
    public void BitmapDictionaryShouldBuild()
    {
        MeasurementEventsBitmaskDictionary measurementBitmasks = [];
        DefineBitmasks( measurementBitmasks );

        // test existing key
        int queryKey = ( int ) MeasurementEventBitmaskKey.HighLimit2;
        int expectedBitmask = 1 << 4;
        int actualBitmask = measurementBitmasks[queryKey];
        Assert.AreEqual( expectedBitmask, actualBitmask, $"Bitmask of key 0x{queryKey:X} should match expected value" );

        // test non existing key
        queryKey = ( int ) MeasurementEventBitmaskKey.FirstReadingInGroup;
        _ = Assert.ThrowsExactly<ArgumentException>( () => measurementBitmasks.IsAnyBitOn( queryKey ), $"Bitmask of key 0x{queryKey:X} should throw an exception" );

        // test invalid bitmask
        queryKey = ( int ) MeasurementEventBitmaskKey.FirstReadingInGroup;
        int existingBitmask = 1 << 4;
        _ = Assert.ThrowsExactly<ArgumentException>( () => measurementBitmasks.Add( queryKey, existingBitmask ), $"Bitmask of key 0x{queryKey:X} should throw an exception because {existingBitmask} already exists" );
        OperationEventsBitmaskDictionary operationBitmasks = [];
        DefineBitmasks( operationBitmasks );

        // test existing key
        queryKey = ( int ) OperationEventBitmaskKey.Arming;
        expectedBitmask = 1 << 6;
        actualBitmask = operationBitmasks[queryKey];
        Assert.AreEqual( expectedBitmask, actualBitmask, $"Bitmask of key 0x{queryKey:X} should match expected value" );

        // test non existing key
        queryKey = ( int ) OperationEventBitmaskKey.Setting;
        _ = Assert.ThrowsExactly<ArgumentException>( () => operationBitmasks.IsAnyBitOn( queryKey ), $"Bitmask of key 0x{queryKey:X} should throw an exception" );

        // test invalid bitmask
        queryKey = ( int ) OperationEventBitmaskKey.Setting;
        existingBitmask = 1 << 6;
        _ = Assert.ThrowsExactly<ArgumentException>( () => operationBitmasks.Add( queryKey, existingBitmask ), $"Bitmask of key 0x{queryKey:X} should throw an exception because {existingBitmask} already exists" );
        QuestionableEventsBitmaskDictionary questionableBitmasks = [];
        DefineBitmasks( questionableBitmasks );

        // test existing key
        queryKey = ( int ) QuestionableEventBitmaskKey.CalibrationSummary;
        expectedBitmask = 1 << 4;
        actualBitmask = questionableBitmasks[queryKey];
        Assert.AreEqual( expectedBitmask, actualBitmask, $"Bitmask of key 0x{queryKey:X} should match expected value" );

        // test non existing key
        queryKey = ( int ) QuestionableEventBitmaskKey.TemperatureSummary;
        _ = Assert.ThrowsExactly<ArgumentException>( () => questionableBitmasks.IsAnyBitOn( queryKey ), $"Bitmask of key 0x{queryKey:X} should throw an exception" );

        // test invalid bitmask
        queryKey = ( int ) QuestionableEventBitmaskKey.TemperatureSummary;
        existingBitmask = 1 << 4;
        _ = Assert.ThrowsExactly<ArgumentException>( () => questionableBitmasks.Add( queryKey, existingBitmask ), $"Bitmask of key 0x{queryKey:X} should throw an exception because {existingBitmask} already exists" );
    }

    #endregion
}
