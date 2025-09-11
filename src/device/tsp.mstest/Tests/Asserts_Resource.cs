using cc.isr.VI.Device.Tsp.Tests.Base;

namespace cc.isr.VI.Device.Tsp.Tests;

public sealed partial class Asserts
{
    #region " device trace message test "

    /// <summary>   Assert device trace message should be queued. </summary>
    /// <remarks>   David, 2021-07-07. </remarks>
    /// <param name="device">   The device. </param>
    public static void AssertDeviceTraceMessageShouldBeQueued( VisaSessionBase? device )
    {
        Assert.IsNotNull( device );

        if ( Asserts.TraceListener is null ) throw new ArgumentException( nameof( Asserts.TraceListener ) );

        if ( !AssertIfTraceErrorMessages ) return;

        Console.WriteLine( "NOTE: The following warning messages are by design and intended for testing message tracing." );

        string payload = "Device message";
        _ = cc.isr.VI.SessionLogger.Instance.LogWarning( payload );

        // with the new talker, the device identifies the following libraries:
        // 0x0100 core agnostic; 0x01006 vi device and 0x01026 Keithley Meter
        // so these test looks for the first warning
        int fetchNumber = 0;
        if ( TraceListener.Messages.TryGetValue( TraceEventType.Warning, out System.Collections.Generic.List<string>? traceMessages ) )
            fetchNumber += 1;

        if ( traceMessages is null )
            Assert.Fail( $"Failed tracing the warning message." );

        string traceMessage = traceMessages[0];

        if ( string.IsNullOrWhiteSpace( traceMessage ) )
            Assert.Fail( $"{payload} failed to trace message {fetchNumber}" );

        Assert.HasCount( 1, traceMessages, $"{payload} expected on warning message." );
        Assert.Contains( payload, traceMessage, $"'{payload}' should be contained in the trace message {traceMessage}" );
    }

    #endregion

    #region " device should open and close "

    /// <summary>   Assert session should open without device errors. </summary>
    /// <remarks>   David, 2021-03-31. </remarks>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="device">           The device. </param>
    public static void AssertDeviceShouldOpenWithoutDeviceErrors( VisaSessionBase? device, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Assert.IsNotNull( device.StatusSubsystemBase, $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} should be defined. Device not initialized." );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );

        TestBase.AssertResourceNameShouldPing( device.Session, resourceSettings );

        (bool success, string details) = device.TryOpenSession( resourceSettings.ResourceName, resourceSettings.ResourceModel );

        Assert.IsTrue( success, $"Failed to open session: {details}" );

        Assert.IsTrue( success, $"Failed to open session: {details}" );
        TestBase.ConsoleOutputMemberMessage( $"{resourceSettings.ResourceName} is open" );

        // the device must be initialized so as to define the controller node.
        // this also clears any existing errors from previous tests.
        device.ResetClearInit();
        device.PresetKnownState();

        Asserts.AssertMessageQueue();

        if ( device.IsSessionOpen )
            // report device errors if the error bit is set.
            _ = device.Session.TryQueryAndReportDeviceErrors( device.Session.ReadStatusByte() );

        Asserts.AssertMessageQueue();
    }

    /// <summary>   Assert session should open. </summary>
    /// <remarks>   David, 2021-03-31. </remarks>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="device">           The device. </param>
    public static void AssertDeviceShouldOpen( VisaSessionBase? device, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Assert.IsNotNull( device.StatusSubsystemBase, $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} should be defined. Device not initialized." );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );

        TestBase.AssertResourceNameShouldPing( device.Session, resourceSettings );

        (bool success, string details) = device.TryOpenSession( resourceSettings.ResourceName, resourceSettings.ResourceModel );

        Assert.IsTrue( success, $"Failed to open session: {details}" );
        TestBase.ConsoleOutputMemberMessage( $"{resourceSettings.ResourceName} is open" );

        // the device must be initialized so as to define the controller node.
        // this also clears any existing errors from previous tests.
        device.ResetClearInit();
        device.PresetKnownState();
    }

    /// <summary>   Assert device should Initialize known state. </summary>
    /// <remarks>   2024-11-15. </remarks>
    /// <param name="device">   The device. </param>
    public static void AssertDeviceShouldInitializeKnownState( VisaSessionBase? device )
    {
        Assert.IsNotNull( device, $"{nameof( device )} should not be null." );
        Assert.IsNotNull( device.Session, $"{nameof( device )}.{nameof( device.Session )} should not be null." );
        Assert.IsTrue( device.IsDeviceOpen, $"{device.ResourceNameCaption} session should be open." );
        device.InitKnownState();
        device.Session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   Assert device should preset known state. </summary>
    /// <remarks>   2024-11-15. </remarks>
    /// <param name="device">   The device. </param>
    public static void AssertDeviceShouldPresetKnownState( VisaSessionBase? device )
    {
        Assert.IsNotNull( device, $"{nameof( device )} should not be null." );
        Assert.IsNotNull( device.Session, $"{nameof( device )}.{nameof( device.Session )} should not be null." );
        Assert.IsTrue( device.IsDeviceOpen, $"{device.ResourceNameCaption} session should be open." );
        device.PresetKnownState();
        device.Session.ThrowDeviceExceptionIfError();
    }

    /// <summary> Asserts closing a session after checking for existing or new errors. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <param name="device">   The device. </param>
    public static void AssertDeviceShouldCloseWithoutErrors( VisaSessionBase? device )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Assert.IsNotNull( device.StatusSubsystemBase, $"{nameof( device )}.{nameof( device.StatusSubsystemBase )}" );

        try
        {
            Asserts.AssertMessageQueue();
            if ( device.IsSessionOpen )
                // report device errors if the error bit is set.
                _ = device.Session.TryQueryAndReportDeviceErrors( device.Session.ReadStatusByte() );
            Asserts.AssertMessageQueue();
        }
        catch
        {
            throw;
        }
        finally
        {
            device.CloseSession();
        }

        Assert.IsFalse( device.IsDeviceOpen, $"{device.ResourceNameCaption} failed closing session" );
        Asserts.AssertMessageQueue();
    }

    /// <summary> Asserts closing a session asserting queued messages. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <param name="device">   The device. </param>
    public static void AssertDeviceShouldCloseAssertingMessageQueue( VisaSessionBase? device )
    {
        Assert.IsNotNull( device );

        try
        {
            Asserts.AssertMessageQueue();
        }
        catch
        {
            throw;
        }
        finally
        {
            device.CloseSession();
        }

        Assert.IsFalse( device.IsDeviceOpen, $"{device.ResourceNameCaption} failed closing session" );
        Asserts.AssertMessageQueue();
    }

    /// <summary> Asserts closing a session. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <param name="device">   The device. </param>
    public static void AssertDeviceShouldClose( VisaSessionBase? device )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.StatusSubsystemBase );

        try
        {
            // pause a bit
            // _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 500 ) );
            device.CloseSession();
        }
        catch
        {
            throw;
        }
        finally
        {
        }
        Assert.IsFalse( device.IsDeviceOpen, $"{device.ResourceNameCaption} failed closing session" );
    }

    #endregion
}
