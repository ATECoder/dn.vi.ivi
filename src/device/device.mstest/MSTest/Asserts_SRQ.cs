using System;
using cc.isr.Std.TimeSpanExtensions;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.Device.MSTest;

public sealed partial class Asserts
{
    #region " service request polling "

    /// <summary>   Assert device could be polled. </summary>
    /// <remarks>   2024-08-01. </remarks>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="device">           The device. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    /// <param name="statusSettings">   The status settings. </param>
    public static void AssertDeviceShouldBePolled( VisaSessionBase? device, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Assert.IsTrue( device.Session.TimingSettings.Exists );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );
        Assert.IsTrue( device.IsDeviceOpen, "session should be open" );

        if ( device.Session is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.Session )} is null." );
        if ( device.StatusSubsystemBase is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} is null." );

        // ensure service request handling is disabled.
        device.Session.DisableServiceRequestEventHandler();

        // must refresh the service request here to update the status register flags.
        _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
        device.Session.ThrowDeviceExceptionIfError( device.Session.ReadStatusByte() );

        Assert.IsFalse( device.Session.ServiceRequestEventEnabled, $"{nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEventEnabled ).SplitWords()} should be disabled" );
        device.Session.ApplyStatusByteEnableBitmask( device.Session.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );
        Assert.IsFalse( device.Session.ServiceRequestEventEnabled, "service request event should not be enabled" );
        try
        {
            device.PollTimespan = TimeSpan.FromMilliseconds( 100d );
            Assert.IsFalse( device.PollAutoRead, $"{nameof( VisaSessionBase.PollAutoRead ).SplitWords()} should be disabled" );
            device.PollAutoRead = true;
            Assert.IsTrue( device.PollAutoRead, $"{nameof( VisaSessionBase.PollAutoRead ).SplitWords()} should be enabled" );
            Assert.IsFalse( device.PollEnabled, $"{nameof( VisaSessionBase.PollEnabled ).SplitWords()} should be disabled" );
            device.PollEnabled = true;
            Assert.IsTrue( device.PollEnabled, $"{nameof( VisaSessionBase.PollEnabled ).SplitWords()} should be enabled" );
            Assert.IsTrue( string.IsNullOrWhiteSpace( device.PollReading ), $"{nameof( VisaSessionBase.PollAutoRead ).SplitWords()} should be empty" );

            // read operation completion
            string expectedPollMessage = device.Session.OperationCompletedReplyMessage;
            string queryCommand = device.Session.OperationCompletedQueryCommand;
            string pollMessageName = $"Message {queryCommand}";
            _ = device.Session.WriteLine( queryCommand );

            // wait for the message
            device.StartAwaitingPollReadingTask( TimeSpan.FromTicks( 100L * device.PollTimespan.Ticks ) ).Wait();
            Assert.IsFalse( string.IsNullOrWhiteSpace( device.PollReading ), $"Poll reading '{device.PollReading}' should not be empty" );
            Assert.AreEqual( expectedPollMessage, device.PollReading, $"Expected {pollMessageName} {queryCommand} message" );
            device.PollAutoRead = false;
            device.PollEnabled = false;
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
            cc.isr.VI.Pith.ServiceRequests statusByte = device.Session.ReadStatusByte();
            // wait for the poll timer to disabled
            device.StartAwaitingPollTimerDisabledTask( TimeSpan.FromTicks( 2L * device.PollTimespan.Ticks ) ).Wait();
            Assert.IsFalse( device.PollAutoRead, $"{nameof( VisaSessionBase.PollAutoRead ).SplitWords()} should be disabled" );
            Assert.IsFalse( device.PollEnabled, $"{nameof( VisaSessionBase.PollEnabled ).SplitWords()} should be disabled" );
            Assert.IsFalse( device.PollTimerEnabled, $"{nameof( VisaSessionBase.PollTimerEnabled ).SplitWords()} should be disabled" );
        }
        catch
        {
            throw;
        }
        finally
        {
            device.PollAutoRead = false;
            device.PollEnabled = false;
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
            _ = device.Session.ReadStatusByte();
        }
    }

    /// <summary>   Assert query result should be polled. </summary>
    /// <remarks>   David, 2021-03-31. </remarks>
    /// <param name="device">                               The device. </param>
    /// <param name="queryCommand">                         The query command. </param>
    /// <param name="expectedPollMessageAvailableBitmask">  The expected poll message available
    ///                                                     bitmask. </param>
    /// <param name="expectedReply">                        The expected reply. </param>
    public static void AssertQueryResultShouldBePolled( VisaSessionBase? device, string? queryCommand,
                                                        int expectedPollMessageAvailableBitmask, string? expectedReply )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( queryCommand );
        Assert.IsNotNull( expectedReply );
        Assert.IsTrue( device.IsDeviceOpen, "session should be open" );

        if ( device.Session is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.Session )} is null." );
        if ( device.StatusSubsystemBase is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} is null." );

        // ensure service request handling is disabled.
        device.Session.DisableServiceRequestEventHandler();

        // must refresh the service request here to update the status register flags.
        _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
        _ = device.Session.ReadStatusByte();
        Assert.IsFalse( device.Session.ServiceRequestEventEnabled, $"{nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEventEnabled ).SplitWords()} should be disabled" );
        device.PollMessageAvailableBitmask = expectedPollMessageAvailableBitmask;
        try
        {
            device.PollTimespan = TimeSpan.FromMilliseconds( 100d );
            Assert.IsFalse( device.PollAutoRead, $"{nameof( VisaSessionBase.PollAutoRead ).SplitWords()} should be disabled" );
            device.PollAutoRead = true;
            Assert.IsTrue( device.PollAutoRead, $"{nameof( VisaSessionBase.PollAutoRead ).SplitWords()} should be enabled" );
            Assert.IsFalse( device.PollEnabled, $"{nameof( VisaSessionBase.PollEnabled ).SplitWords()} should be disabled" );
            device.PollEnabled = true;
            Assert.IsTrue( device.PollEnabled, $"{nameof( VisaSessionBase.PollEnabled ).SplitWords()} should be enabled" );
            Assert.IsTrue( string.IsNullOrWhiteSpace( device.PollReading ), $"{nameof( VisaSessionBase.PollAutoRead ).SplitWords()} should be empty" );

            // read operation completion
            string expectedSPollMessage = expectedReply;
            string pollMessageName = $"Custom command: {queryCommand}";
            _ = device.Session.WriteLine( queryCommand );

            // wait for the message
            device.StartAwaitingPollReadingTask( TimeSpan.FromTicks( 100L * device.PollTimespan.Ticks ) ).Wait();
            Assert.IsFalse( string.IsNullOrWhiteSpace( device.PollReading ), $"Poll reading '{device.PollReading}' should not be empty" );
            Assert.AreEqual( expectedSPollMessage, device.PollReading, $"Expected {pollMessageName} {queryCommand} message" );
            device.PollAutoRead = false;
            device.PollEnabled = false;
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
            _ = device.Session.ReadStatusByte();
            // wait for the poll timer to disabled
            device.StartAwaitingPollTimerDisabledTask( TimeSpan.FromTicks( 2L * device.PollTimespan.Ticks ) ).Wait();
            Assert.IsFalse( device.PollAutoRead, $"{nameof( VisaSessionBase.PollAutoRead ).SplitWords()} should be disabled" );
            Assert.IsFalse( device.PollEnabled, $"{nameof( VisaSessionBase.PollEnabled ).SplitWords()} should be disabled" );
            Assert.IsFalse( device.PollTimerEnabled, $"{nameof( VisaSessionBase.PollTimerEnabled ).SplitWords()} should be disabled" );
        }
        catch
        {
            throw;
        }
        finally
        {
            device.PollAutoRead = false;
            device.PollEnabled = false;
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
            _ = device.Session.ReadStatusByte();
        }
    }

    #endregion

    #region " service request handling "

    /// <summary> Name of the service request message. </summary>
    private static string _serviceRequestMessageName = "Operation Completed";

    /// <summary> Message describing the expected service request. </summary>
    private static string _expectedServiceRequestMessage = string.Empty;
    private static readonly Std.Concurrent.ConcurrentToken<string> _serviceRequestReadingToken = new();

    /// <summary> Handles the service request. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private static void HandleServiceRequest( object? sender, EventArgs e )
    {
        Assert.IsNotNull( sender );
        try
        {
            if ( sender is cc.isr.VI.Pith.SessionBase mbs )
            {
                cc.isr.VI.Pith.ServiceRequests sb = mbs.ReadStatusByte();
                if ( (sb & cc.isr.VI.Pith.ServiceRequests.MessageAvailable) != 0 )
                    _serviceRequestReadingToken.Value = mbs.ReadLineTrimEnd();
                else
                    Assert.Fail( "MAV in status register is not set, which means that message is not available. Make sure the command to enable SRQ is correct, and the instrument is 488.2 compatible." );
            }
        }
        catch ( Exception ex )
        {
            Assert.Fail( ex.ToString() );
        }
    }

    /// <summary> Assert service request should be handled. </summary>
    /// <param name="session"> The session. </param>
    private static void AssertServiceRequestShouldBeHandled( cc.isr.VI.Pith.SessionBase? session, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );
        Assert.IsTrue( session.TimingSettings.Exists );
        Assert.IsFalse( session.ServiceRequestEventEnabled, "service request event should not be enabled" );
        Assert.IsNotNull( session.ServiceRequestEnableBitmask );
        string expectedServiceRequestEnableCommand = string.Format( System.Globalization.CultureInfo.CurrentCulture,
            session.ServiceRequestEnableCommandFormat, ( int ) session.ServiceRequestEnableBitmask );
        try
        {
            session.EnableServiceRequestEventHandler();
            Assert.IsTrue( session.ServiceRequestEventEnabled, $"Service request event not enabled" );
            session.ServiceRequested += HandleServiceRequest;
            // read operation completion
            _expectedServiceRequestMessage = cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue;
            _serviceRequestMessageName = "Operation Completed";
            string queryCommand = "*OPC?\n";
            _ = session.WriteLine( queryCommand );
            // wait for the message
            _ = TimeSpan.FromMilliseconds( 400 ).AsyncWaitUntil( TimeSpan.FromMilliseconds( 10 ), () => !string.IsNullOrWhiteSpace( _serviceRequestReadingToken.Value ) );
            Assert.AreEqual( _expectedServiceRequestMessage, _serviceRequestReadingToken.Value, $"Expected {_serviceRequestMessageName} {queryCommand} message" );
            session.ServiceRequested -= HandleServiceRequest;
            session.DisableServiceRequestEventHandler();
            // must refresh the service request here to update the status register flags.
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( session.StatusReadDelay );
            session.ThrowDeviceExceptionIfError( session.ReadStatusByte() );
            Assert.IsFalse( session.ServiceRequestEventEnabled, $"Service request event should be disabled" );
        }
        catch
        {
            throw;
        }
        finally
        {
            session.DisableServiceRequestEventHandler();
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( session.StatusReadDelay );
        }
        session.ThrowDeviceExceptionIfError( session.ReadStatusByte() );
    }

    /// <summary>   Assert requesting service be enabled by session. </summary>
    /// <remarks>   2024-08-01. </remarks>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="device">           The device. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    /// <param name="statusSettings">   The status settings. </param>
    public static void AssertRequestingServiceBeEnabledBySession( VisaSessionBase? device, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );
        Assert.IsTrue( device.Session.TimingSettings.Exists );
        Assert.IsTrue( device.IsDeviceOpen, "session should be open" );

        if ( device.Session is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.Session )} is null." );
        if ( device.StatusSubsystemBase is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} is null." );

        _ = device.Session.WriteLine( device.Session.ClearExecutionStateCommand );
        device.Session.ApplyStatusByteEnableBitmask( device.Session.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );
        device.Session.ApplyStandardEventEnableBitmask( device.Session.RegistersBitmasksSettings.StandardEventEnableEventsBitmask );
        _ = device.Session.ReadStatusByte();
        // read operation completion
        string queryCommand = "*OPC\n";
        _ = device.Session.WriteLine( queryCommand );
        int statusByte = ( int ) device.Session.ReadStatusByte();
        Assert.AreEqual( ( int ) cc.isr.VI.Pith.ServiceRequests.RequestingService, statusByte & ( int ) cc.isr.VI.Pith.ServiceRequests.RequestingService,
            $"Requesting service bit should be on." );
    }

    /// <summary>   Assert session service request should be handled. </summary>
    /// <remarks>   David, 2021-07-04. </remarks>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="device">           The device. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    /// <param name="statusSettings">   The status settings. </param>
    ///
    public static void AssertServiceRequestShouldBeHandledBySession( VisaSessionBase? device, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );
        Assert.IsTrue( device.Session.TimingSettings.Exists );
        Assert.IsTrue( device.IsDeviceOpen, "session should be open" );

        if ( device.Session is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.Session )} is null." );
        if ( device.StatusSubsystemBase is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} is null." );

        _ = device.Session.WriteLine( device.Session.ClearExecutionStateCommand );
        device.Session.ApplyStatusByteEnableBitmask( device.Session.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );
        device.Session.ApplyStandardEventEnableBitmask( device.Session.RegistersBitmasksSettings.StandardEventEnableEventsBitmask );
        _ = device.Session.ReadStatusByte();
        AssertServiceRequestShouldBeHandled( device.Session, resourceSettings );
    }

    /// <summary>   Assert device should handle service request. </summary>
    /// <remarks>   2024-08-01. </remarks>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="device">           The device. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    /// <param name="statusSettings">   The status settings. </param>
    public static void AssertServiceRequestShouldBeHandledByDevice( VisaSessionBase? device, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( device.Session );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );
        Assert.IsTrue( device.Session.TimingSettings.Exists );
        Assert.IsTrue( device.IsDeviceOpen, "session should be open" );

        if ( device.Session is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.Session )} is null." );
        if ( device.StatusSubsystemBase is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} is null." );

        device.Session.ApplyStatusByteEnableBitmask( device.Session.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );
        Assert.IsFalse( device.Session.ServiceRequestEventEnabled, $"{nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEventEnabled ).SplitWords()} should not be enabled" );
        int expectedPollMessageAvailableBitmask = ( int ) cc.isr.VI.Pith.ServiceRequests.MessageAvailable;
        Assert.AreEqual( expectedPollMessageAvailableBitmask, device.PollMessageAvailableBitmask, "Message available " );
        try
        {
            device.Session.EnableServiceRequestEventHandler();
            Assert.IsTrue( device.Session.ServiceRequestEventEnabled, $"{nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEventEnabled ).SplitWords()} should be enabled" );
            Assert.IsFalse( device.ServiceRequestAutoRead, $"{nameof( VisaSessionBase.ServiceRequestAutoRead ).SplitWords()} should be off" );
            device.ServiceRequestAutoRead = true;
            Assert.IsTrue( device.ServiceRequestAutoRead, $"{nameof( VisaSessionBase.ServiceRequestAutoRead ).SplitWords()} should be on" );
            Assert.IsFalse( device.ServiceRequestHandlerAssigned, $"{nameof( VisaSessionBase.ServiceRequestHandlerAssigned ).SplitWords()} should not be assigned" );
            device.AddServiceRequestEventHandler();
            Assert.IsTrue( device.ServiceRequestHandlerAssigned, $"{nameof( VisaSessionBase.ServiceRequestHandlerAssigned ).SplitWords()} should be assigned" );

            // read operation completion
            _expectedServiceRequestMessage = cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue;
            _serviceRequestMessageName = "Operation Completed";
            string queryCommand = "*OPC?";
            _ = device.Session.WriteLine( queryCommand );

            // wait for the message
            device.StartAwaitingServiceRequestReadingTask( TimeSpan.FromMilliseconds( 10000d ) ).Wait();
            Assert.IsFalse( string.IsNullOrWhiteSpace( device.ServiceRequestReading ), $"Service request reading '{device.ServiceRequestReading}' should not be empty" );
            Assert.AreEqual( _expectedServiceRequestMessage, device.ServiceRequestReading, $"Expected {_serviceRequestMessageName} {queryCommand} message" );
            device.Session.DisableServiceRequestEventHandler();
            // must refresh the service request here to update the status register flags.
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
            _ = device.Session.ReadStatusByte();
            Assert.IsFalse( device.Session.ServiceRequestEventEnabled, $"{nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEventEnabled ).SplitWords()} should be disabled" );
        }
        catch
        {
            throw;
        }
        finally
        {
            if ( device.ServiceRequestHandlerAssigned )
                device.RemoveServiceRequestEventHandler();
            device.Session.DisableServiceRequestEventHandler();
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
            _ = device.Session.ReadStatusByte();
        }
    }

    /// <summary>   Assert service request should be handled. </summary>
    /// <remarks>   David, 2021-03-31. </remarks>
    /// <param name="device">                               The device. </param>
    /// <param name="queryCommand">                         The query command. </param>
    /// <param name="expectedPollMessageAvailableBitmask">  The expected poll message available
    ///                                                     bitmask. </param>
    /// <param name="expectedReply">                        The expected reply. </param>
    public static void AssertServiceRequestShouldBeHandled( VisaSessionBase? device, string? queryCommand,
                                                           int expectedPollMessageAvailableBitmask, string? expectedReply )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( queryCommand );
        Assert.IsNotNull( expectedReply );
        Assert.IsTrue( device.IsDeviceOpen, "session should be open" );

        if ( device.Session is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.Session )} is null." );
        if ( device.StatusSubsystemBase is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} is null." );

        device.PollMessageAvailableBitmask = expectedPollMessageAvailableBitmask;
        try
        {
            device.Session.EnableServiceRequestEventHandler();
            Assert.IsTrue( device.Session.ServiceRequestEventEnabled, $"{nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEventEnabled ).SplitWords()} should be enabled" );
            Assert.IsFalse( device.ServiceRequestAutoRead, $"{nameof( VisaSessionBase.ServiceRequestAutoRead ).SplitWords()} should be off" );
            device.ServiceRequestAutoRead = true;
            Assert.IsTrue( device.ServiceRequestAutoRead, $"{nameof( VisaSessionBase.ServiceRequestAutoRead ).SplitWords()} should be on" );
            Assert.IsFalse( device.ServiceRequestHandlerAssigned, $"{nameof( VisaSessionBase.ServiceRequestHandlerAssigned ).SplitWords()} should not be assigned" );
            device.AddServiceRequestEventHandler();
            Assert.IsTrue( device.ServiceRequestHandlerAssigned, $"{nameof( VisaSessionBase.ServiceRequestHandlerAssigned ).SplitWords()} should be assigned" );

            // read operation completion
            _expectedServiceRequestMessage = expectedReply;
            string pollMessageName = $"Custom command: {queryCommand}";
            _ = device.Session.WriteLine( queryCommand );

            // wait for the message
            device.StartAwaitingServiceRequestReadingTask( TimeSpan.FromMilliseconds( 10000d ) ).Wait();
            Assert.IsFalse( string.IsNullOrWhiteSpace( device.ServiceRequestReading ), $"Service request reading '{device.ServiceRequestReading}' should not be empty" );
            Assert.AreEqual( _expectedServiceRequestMessage, device.ServiceRequestReading, $"Expected {_serviceRequestMessageName} {queryCommand} message" );
            device.Session.DisableServiceRequestEventHandler();
            // must refresh the service request here to update the status register flags.
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
            _ = device.Session.ReadStatusByte();
            Assert.IsFalse( device.Session.ServiceRequestEventEnabled, $"{nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEventEnabled ).SplitWords()} should be disabled" );
        }
        catch
        {
            throw;
        }
        finally
        {
            if ( device.ServiceRequestHandlerAssigned )
                device.RemoveServiceRequestEventHandler();
            device.Session.DisableServiceRequestEventHandler();
            _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
            _ = device.Session.ReadStatusByte();
        }
    }

    #endregion
}
