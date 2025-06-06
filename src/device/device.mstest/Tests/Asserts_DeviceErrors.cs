using System;
using cc.isr.Std.TimeSpanExtensions;
using cc.isr.Std.SplitExtensions;
using cc.isr.VI.Pith;
using cc.isr.VI.Device.Tests.Settings;

namespace cc.isr.VI.Device.Tests;

public sealed partial class Asserts
{
    #region " orphan messages "

    /// <summary>   Assert orphan messages. </summary>
    /// <remarks>   2025-04-29. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="withPath">         (Optional) True to with path. </param>
    /// <param name="withFileName">     (Optional) True to with file name. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void AssertOrphanMessages( Pith.SessionBase? session, bool withPath = false, bool withFileName = true,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string member = withPath
            ? $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}"
            : withFileName
              ? $"{System.IO.Path.GetFileNameWithoutExtension( sourcePath )}.{memberName}.Line#{sourceLineNumber}"
              : $"{memberName}.Line#{sourceLineNumber}";

        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"VISA session to '{nameof( session.ResourceNameCaption )}' must be open." );
        string orphanMessages = session.ReadLines( session.StatusReadDelay, TimeSpan.FromMilliseconds( 100 ), false );
        Assert.IsTrue( string.IsNullOrWhiteSpace( orphanMessages ), $"{member}:\r\n\tThe following messages were left on the output buffer:\r\n\t{orphanMessages}" );
    }

    /// <summary>   Throw if device errors. </summary>
    /// <remarks>   2025-04-29. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="message">          The message. </param>
    /// <param name="withPath">         (Optional) True to with path. </param>
    /// <param name="withFileName">     (Optional) True to with file name. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void ThrowIfDeviceErrors( Pith.SessionBase? session, string message, bool withPath = false, bool withFileName = true,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string member = withPath
            ? $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}"
            : withFileName
              ? $"{System.IO.Path.GetFileNameWithoutExtension( sourcePath )}.{memberName}.Line#{sourceLineNumber}"
              : $"{memberName}.Line#{sourceLineNumber}";
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        session.ThrowDeviceExceptionIfError( failureMessage: $"{member}:\r\n\t{message}" );
    }

    #endregion

    #region " device errors "

    /// <summary>   Assert device errors should match. </summary>
    /// <remarks>   2025-01-16. </remarks>
    /// <param name="subsystem">            The subsystem. </param>
    /// <param name="deviceErrorSettings">  Information describing the device errors for testing. </param>
    public static void AssertDeviceErrorsShouldMatch( StatusSubsystemBase subsystem, DeviceErrorsSettings? deviceErrorSettings )
    {
        Assert.IsNotNull( subsystem, $"{nameof( subsystem )} should not be null." );
        Assert.IsNotNull( deviceErrorSettings, $"{nameof( deviceErrorSettings )} should not be null." );
        string propertyName = nameof( cc.isr.VI.Pith.ServiceRequests.ErrorAvailable ).SplitWords();
        Assert.IsFalse( subsystem.Session.IsErrorBitSet( subsystem.Session.ServiceRequestStatus ), $"{subsystem.ResourceNameCaption} {propertyName} bit {subsystem.Session.ServiceRequestStatus:X} should be off" );
        Assert.IsFalse( subsystem.Session.ErrorAvailable, $"{subsystem.ResourceNameCaption} error available bit {subsystem.Session.ServiceRequestStatus:X} is on; last device error: {subsystem.Session.LastErrorCompoundErrorMessage}" );
        _ = nameof( cc.isr.VI.Pith.ServiceRequests.ErrorAvailable ).SplitWords();
        string errors = subsystem.Session.DeviceErrorReport;
        Assert.IsTrue( string.IsNullOrWhiteSpace( errors ), $"{subsystem.ResourceNameCaption} device errors: {errors}" );
        Assert.IsFalse( subsystem.Session.HasDeviceError, $"{subsystem.ResourceNameCaption} last device error:\n{subsystem.Session.DeviceErrorPreamble}\n{subsystem.Session.LastErrorCompoundErrorMessage}" );
        Assert.IsFalse( subsystem.Session.HasErrorReport, $"{subsystem.ResourceNameCaption} device error report:\n{subsystem.Session.DeviceErrorPreamble}\n{subsystem.Session.DeviceErrorReport}" );
        Assert.AreEqual( 0, subsystem.Session.DeviceErrorReportCount, "Error count should be zero" );

        DeviceError deviceError = new();
        deviceError.Parse( deviceErrorSettings.ParseCompoundErrorMessage );
        propertyName = nameof( DeviceError.ErrorMessage ).SplitWords();
        Assert.AreEqual( deviceErrorSettings.ParseErrorMessage, deviceError.ErrorMessage, $"{subsystem.ResourceNameCaption} parsed {propertyName} should match" );

        propertyName = nameof( DeviceError.ErrorNumber ).SplitWords();
        Assert.AreEqual( deviceErrorSettings.ParseErrorNumber, deviceError.ErrorNumber, $"{subsystem.ResourceNameCaption} parsed {propertyName} should match" );

        propertyName = nameof( DeviceError.ErrorLevel ).SplitWords();
        Assert.AreEqual( deviceErrorSettings.ParseErrorLevel, deviceError.ErrorLevel, $"{subsystem.ResourceNameCaption} parsed {propertyName} should match" );
    }

    /// <summary> Assert session device errors should clear. </summary>
    /// <param name="device">         The device. </param>
    /// <param name="deviceErrorSettings"> Information describing the device errors for testing. </param>
    public static void AssertSessionDeviceErrorsShouldClear( VisaSessionBase? device, DeviceErrorsSettings? deviceErrorSettings )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( deviceErrorSettings, $"{nameof( deviceErrorSettings )} should not be null." );
        if ( device.Session is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.Session )} is null." );
        if ( device.StatusSubsystemBase is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} is null." );

        device.Session.ClearActiveState();
        AssertDeviceErrorsShouldMatch( device.StatusSubsystemBase, deviceErrorSettings );
    }

    /// <summary> Assert that the device should read exiting errors that are generated by an erroneous message. </summary>
    /// <param name="device">         The device. </param>
    /// <param name="deviceErrorSettings"> Information describing the device errors for testing. </param>
    public static void AssertDeviceErrorsShouldRead( VisaSessionBase? device, DeviceErrorsSettings? deviceErrorSettings )
    {
        Assert.IsNotNull( device );
        Assert.IsNotNull( deviceErrorSettings, $"{nameof( deviceErrorSettings )} should not be null." );
        if ( device.Session is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.Session )} is null." );
        if ( device.StatusSubsystemBase is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} is null." );

        Console.WriteLine( "NOTE: The following error log is expected as an outcome of the invoked device error." );

        // send an erroneous command
        string erroneousCommand = deviceErrorSettings.ErroneousCommand;
        _ = device.Session.WriteLine( erroneousCommand );
        TimeSpan appliedDelay = TimeSpan.FromMilliseconds( deviceErrorSettings.ErrorAvailableMillisecondsDelay ).AsyncWaitElapsed();

        // read the service request status; this should generate an error available
        _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
        cc.isr.VI.Pith.ServiceRequests statusByte = device.Session.ReadStatusByte();
        device.Session.ApplyStatusByte( statusByte );
        int value = ( int ) statusByte;

        // check the error bits
        cc.isr.VI.Pith.ServiceRequests actualServiceRequest = device.Session.ErrorAvailableBitmask;
        cc.isr.VI.Pith.ServiceRequests expectedServiceRequest = cc.isr.VI.Pith.ServiceRequests.ErrorAvailable;
        Assert.AreEqual( expectedServiceRequest, actualServiceRequest,
                        $"{device.ResourceNameCaption} error bits expected {expectedServiceRequest:X} <> actual {actualServiceRequest:X}" );

        // check the error available status
        Assert.AreEqual( ( int ) expectedServiceRequest, value & ( int ) expectedServiceRequest,
            $"{device.ResourceNameCaption} error bits {expectedServiceRequest:X} are expected in value: {value:X}; applied {appliedDelay:s\\.fff}s delay" );

        // check the error available status
        actualServiceRequest = device.Session.ServiceRequestStatus;
        Assert.AreEqual( expectedServiceRequest, actualServiceRequest & expectedServiceRequest,
            $"{device.ResourceNameCaption} error bits {expectedServiceRequest:X} are expected in {nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestStatus )}: {actualServiceRequest:X}" );
        bool actualErrorAvailable = device.Session.ErrorAvailable;
        Assert.IsTrue( actualErrorAvailable, $"{device.ResourceNameCaption} an error is expected" );

        string propertyName = nameof( DeviceError.ErrorMessage ).SplitWords();
        Assert.AreEqual( deviceErrorSettings.ExpectedErrorMessage, device.Session.DeviceErrorQueue.LastError.ErrorMessage, $"{device.ResourceNameCaption} {propertyName} should match" );

        propertyName = nameof( DeviceError.ErrorNumber ).SplitWords();
        Assert.AreEqual( deviceErrorSettings.ExpectedErrorNumber, device.Session.DeviceErrorQueue.LastError.ErrorNumber, $"{device.ResourceNameCaption} {propertyName} should match" );

        propertyName = nameof( DeviceError.ErrorLevel ).SplitWords();
        Assert.AreEqual( deviceErrorSettings.ExpectedErrorLevel, device.Session.DeviceErrorQueue.LastError.ErrorLevel, $"{device.ResourceNameCaption} {propertyName} should match" );
    }

    /// <summary>   Assert session should read device errors. </summary>
    /// <remarks>   2024-10-07. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="deviceErrorsSettings">   Information describing the device errors. </param>
    public static void AssertSessionShouldReadDeviceErrors( SessionBase? session, DeviceErrorsSettings? deviceErrorsSettings )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( deviceErrorsSettings );

        // send an erroneous command
        string erroneousCommand = deviceErrorsSettings.ErroneousCommand;
        _ = session.WriteLine( erroneousCommand );
        TimeSpan appliedDelay = TimeSpan.FromMilliseconds( deviceErrorsSettings.ErrorAvailableMillisecondsDelay ).AsyncWaitElapsed();

        // read the service request status; this should generate an error available
        _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( session.StatusReadDelay );
        cc.isr.VI.Pith.ServiceRequests statusByte = session.ReadStatusByte();
        Assert.IsTrue( session.IsErrorBitSet( statusByte ),
            $"Status byte '0x{( int ) statusByte:X2}' error bit '0x{( int ) cc.isr.VI.Pith.ServiceRequests.ErrorAvailable:X2}' should be set after sending an erroneous command." );

        // check the error bits
        int value = ( int ) statusByte;
        cc.isr.VI.Pith.ServiceRequests expectedServiceRequest = cc.isr.VI.Pith.ServiceRequests.ErrorAvailable;

        // check the error available status
        Assert.AreEqual( ( int ) expectedServiceRequest, value & ( int ) expectedServiceRequest,
            $"{session.ResourceNameCaption} error bits {expectedServiceRequest:X} are expected in value: {value:X}; applied {appliedDelay:s\\.fff}s delay" );

        _ = session.QueryDeviceErrors( statusByte );

        string propertyName = nameof( DeviceError.ErrorMessage ).SplitWords();
        Assert.AreEqual( deviceErrorsSettings.ExpectedErrorMessage, session.DeviceErrorQueue.LastError.ErrorMessage,
            $"{session.ResourceNameCaption} {propertyName} should match" );

        propertyName = nameof( DeviceError.ErrorNumber ).SplitWords();
        Assert.AreEqual( deviceErrorsSettings.ExpectedErrorNumber, session.DeviceErrorQueue.LastError.ErrorNumber,
            $"{session.ResourceNameCaption} {propertyName} should match" );

        propertyName = nameof( DeviceError.ErrorLevel ).SplitWords();
        Assert.AreEqual( deviceErrorsSettings.ExpectedErrorLevel, session.DeviceErrorQueue.LastError.ErrorLevel,
            $"{session.ResourceNameCaption} {propertyName} should match" );
    }

    /// <summary>   Assert session should clear device errors. </summary>
    /// <remarks>   2024-10-07. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="deviceErrorsSettings">   Information describing the device errors. </param>
    public static void AssertSessionShouldClearDeviceErrors( SessionBase? session, DeviceErrorsSettings? deviceErrorsSettings )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( deviceErrorsSettings );

        // send an erroneous command
        string erroneousCommand = deviceErrorsSettings.ErroneousCommand;
        _ = session.WriteLine( erroneousCommand );
        _ = TimeSpan.FromMilliseconds( deviceErrorsSettings.ErrorAvailableMillisecondsDelay ).AsyncWaitElapsed();

        // read the service request status; this should generate an error available
        _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( session.StatusReadDelay );
        cc.isr.VI.Pith.ServiceRequests statusByte = session.ReadStatusByte();
        Assert.IsTrue( session.IsErrorBitSet( statusByte ),
            $"Status byte '0x{( int ) statusByte:X2}' error bit '0x{( int ) cc.isr.VI.Pith.ServiceRequests.ErrorAvailable:X2}' should be set after sending an erroneous command." );

        // issue a reset.
        session.ResetKnownState();

        // get the status byte.
        statusByte = session.ReadStatusByte();

        // status byte should not be affected by the reset.
        Assert.IsTrue( session.IsErrorBitSet( statusByte ),
            $"Status byte '0x{( int ) statusByte:X2}' error bit '0x{( int ) cc.isr.VI.Pith.ServiceRequests.ErrorAvailable:X2}' should still be set after *RST." );

        // issue a clear without fixing the CLS 2600 issue.
        session.ClearExecutionStateQueryComplete();

        // get the status byte.
        statusByte = session.ReadStatusByte();

        if ( session.ClearsDeviceStructures )
        {
            // status byte should be cleared for these devices.
            Assert.IsFalse( session.IsErrorBitSet( statusByte ),
                $"Status byte '0x{( int ) statusByte:X2}' error bit '0x{( int ) cc.isr.VI.Pith.ServiceRequests.ErrorAvailable:X2}' should be cleared after *CLS for devices with {nameof( SessionBase.ClearsDeviceStructures )}={session.ClearsDeviceStructures}." );
        }
        else
        {
            // status byte should not be affected by the *CLS for these devices.
            Assert.IsTrue( session.IsErrorBitSet( statusByte ),
                $"Status byte '0x{( int ) statusByte:X2}' error bit '0x{( int ) cc.isr.VI.Pith.ServiceRequests.ErrorAvailable:X2}' should still be set after *CLS for devices with {nameof( SessionBase.ClearsDeviceStructures )}={session.ClearsDeviceStructures}." );
        }

        // issue a clear.
        session.ClearExecutionState();

        // get the status byte.  This should be clear if the error queue is cleared.
        statusByte = session.ReadStatusByte();

        DeviceError? de = session.EnqueueDeviceError();

        Assert.IsNotNull( de, $"The session should fetch a non-null device error even if no error exists." );
        Assert.IsFalse( de.IsError, $"The fetched device error {de.CompoundErrorMessage} should be 'no error' after clear execution state." );
        Assert.IsFalse( session.IsErrorBitSet( statusByte ),
            $"Status byte '0x{( int ) statusByte:X2}' error bit '0x{( int ) cc.isr.VI.Pith.ServiceRequests.ErrorAvailable:X2}' should not be set after clearing the execution state." );
    }

    /// <summary> Assert if device has errors. </summary>
    /// <param name="device">         The device. </param>
    /// <param name="subsystemsInfo"> Information describing the subsystems. </param>
    public static void AssertOnDeviceErrors( VisaSessionBase? device )
    {
        Assert.IsNotNull( device );
        if ( device.Session is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.Session )} is null." );
        if ( device.StatusSubsystemBase is null ) throw new ArgumentException( $"{nameof( device )}.{nameof( device.StatusSubsystemBase )} is null." );

        _ = device.Session.TimingSettings.ReadAfterWriteDelay.AsyncWaitElapsed();

        // read the service status byte; this should generate an error available
        // check the error available status
        _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( device.Session.StatusReadDelay );
        Assert.IsFalse( device.Session.IsErrorBitSet( device.Session.ReadStatusByte() ),
            $"{device.ResourceNameCaption} encountered an error {device.Session.DeviceErrorQueue.LastError.CompoundErrorMessage}." );
    }

    /// <summary>   Assert session last error. </summary>
    /// <remarks>   2024-09-17. </remarks>
    /// <param name="session">  The session. </param>
    public static void AssertSessionLastError( cc.isr.VI.Pith.SessionBase? session, ServiceRequests statusByte )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        _ = SessionBase.AsyncDelay( session.TimingSettings.ReadAfterWriteDelay + session.StatusReadDelay );

        Assert.IsFalse( session.IsErrorBitSet( statusByte ), $"{session.ResourceNameCaption} encountered an error {session.LastErrorCompoundErrorMessage}." );
    }

    #endregion
}
