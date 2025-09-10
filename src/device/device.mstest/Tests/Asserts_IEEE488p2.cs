using System;
using cc.isr.Std.TimeSpanExtensions;
using cc.isr.Std.SplitExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Device.Tests;

public sealed partial class Asserts
{
    #region " session open, close "

    /// <summary>   Queries if a given assert visa session should open. </summary>
    /// <remarks>
    /// Unlike <see cref="AssertDeviceShouldOpenWithoutDeviceErrors(TestSite, VisaSessionBase, ResourceSettingsBase)"/>,
    /// this function uses the Session base to open the session thus not assume the existence of the
    /// status subsystem.
    /// </remarks>
    /// <param name="session">          The visa session. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    public static void AssertVisaSessionShouldOpen( VisaSessionBase? session, Pith.Settings.ResourceSettings? resourceSettings )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( session.Session );
        Assert.IsNotNull( resourceSettings, $"{nameof( Pith.Settings.ResourceSettings )} should not be null." );
        Assert.IsTrue( resourceSettings.Exists, $"{nameof( Pith.Settings.ResourceSettings )} should exist in the settings file." );
        Assert.IsTrue( session.Session.TimingSettings.Exists );
        Assert.IsTrue( session.Session.RegistersBitmasksSettings.Exists );
        Tests.Base.TestBase.AssertResourceNameShouldPing( session.Session, resourceSettings );

        Assert.AreEqual( cc.isr.VI.Syntax.Ieee488Syntax.LanguageTsp.Equals( resourceSettings.Language, StringComparison.OrdinalIgnoreCase ),
            session.Session.SplitCommonCommands, $"{resourceSettings.Language} instruments must split common commands." );
        Assert.AreEqual( ( int ) session.Session.ServiceRequestEnableEventsBitmask, session.Session.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask );

        session.SubsystemSupportMode = SubsystemSupportMode.Native;
        session.OpenSession( resourceSettings.ResourceName, resourceSettings.ResourceModel );
        Asserts.AssertMessageQueue();
    }

    /// <summary>   Closes a VISA session. </summary>
    /// <remarks>   2024-10-09. </remarks>
    /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
    ///                                         illegal values. </exception>
    /// <param name="session">  The visa session. </param>
    public static void AssertVisaSessionShouldClose( VisaSessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        if ( session.Session is null ) throw new ArgumentException( $"{nameof( session )}.{nameof( session.Session )} is null." );

        try
        {
            Asserts.AssertMessageQueue();
            session.Session.CloseSession();
        }
        catch
        {
            throw;
        }
        finally
        {
        }

        Assert.IsFalse( session.IsDeviceOpen, $"{session.ResourceNameCaption} failed closing session" );
        Asserts.AssertMessageQueue();
    }

    #endregion

    #region " service request events "

    /// <summary>   Assert that the session should wait for the bitmask to set. </summary>
    /// <remarks>   David, 2021-03-31. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="expectedBitmask">  The expected bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    public static void AssertBitmaskShouldBeSet( cc.isr.VI.Pith.SessionBase? session, byte expectedBitmask, TimeSpan timeout )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        (_, byte status) = (false, expectedBitmask);
        (bool timedOut, int statusByte, TimeSpan _) = session.AwaitStatusBitmask( expectedBitmask, timeout );
        Assert.IsFalse( timedOut, $"Awaiting 0x{( int ) expectedBitmask:X2} should not time out" );
        Assert.AreNotEqual( 0, statusByte & status,
            $"Status byte 0x{statusByte:X2} 0x{status:X2} bitmask should be set" );
        Base.TestBase.ConsoleOutputMemberMessage( $"{session.OpenResourceModel} status byte 0x{statusByte:X2} 0x{status:X2} bitmask was set" );
    }

    /// <summary>   Assert that the session should wait for the bitmask to set. </summary>
    /// <param name="session">         The session. </param>
    /// <param name="expectedBitmask"> The expected bitmask. </param>
    /// <param name="timeout">         The timeout. </param>
    public static void AssertBitmaskShouldBeSet( cc.isr.VI.Pith.SessionBase? session, cc.isr.VI.Pith.ServiceRequests expectedBitmask, TimeSpan timeout )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        (_, cc.isr.VI.Pith.ServiceRequests status) = (false, expectedBitmask);
        (bool timedOut, cc.isr.VI.Pith.ServiceRequests statusByte, TimeSpan _) = session.AwaitStatusBitmask( expectedBitmask, timeout );
        Assert.IsFalse( timedOut, $"Getting 0x{( int ) statusByte:X2} awaiting 0x{( int ) expectedBitmask:X2} should not time out" );
        Assert.AreNotEqual( 0, ( int ) statusByte & ( int ) status, $"Status byte 0x{( int ) statusByte:X2} 0x{( int ) status:X2} bitmask should be set" );
        Base.TestBase.ConsoleOutputMemberMessage( $"{session.OpenResourceModel} status byte 0x{( int ) statusByte:X2} 0x{( int ) status:X2} bitmask was set" );
    }

    /// <summary>   Assert that the session should wait for both bitmasks to be set. </summary>
    /// <remarks>   David, 2021-03-31. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="expectedBitmask">  The expected bitmask. </param>
    /// <param name="secondaryBitmask"> The secondary bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    public static void AssertBitmasksShouldBeSet( cc.isr.VI.Pith.SessionBase? session, cc.isr.VI.Pith.ServiceRequests expectedBitmask,
        cc.isr.VI.Pith.ServiceRequests secondaryBitmask, TimeSpan timeout )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        (_, cc.isr.VI.Pith.ServiceRequests status) = (false, expectedBitmask);
        (bool timedOut, cc.isr.VI.Pith.ServiceRequests statusByte, TimeSpan _) = session.AwaitStatusBitmask( expectedBitmask, timeout );
        Assert.IsFalse( timedOut, $"Awaiting 0x{( int ) expectedBitmask:X2} should not time out" );
        Assert.AreNotEqual( 0, ( int ) statusByte & ( int ) status,
            $"Status byte 0x{( int ) statusByte:X2} 0x{( int ) status:X2} bitmask should be set" );
        Assert.AreNotEqual( 0, ( int ) statusByte & ( int ) secondaryBitmask,
            $"Status byte 0x{( int ) statusByte:X2} 0x{( int ) secondaryBitmask:X2} bitmask should also be set" );
        Base.TestBase.ConsoleOutputMemberMessage( $"{session.OpenResourceModel} status byte 0x{( int ) statusByte:X2} 0x{( int ) status:X2} bitmask was set" );
    }

    /// <summary>   Assert that the session should wait for both bitmasks to be set. </summary>
    /// <remarks>   David, 2021-11-15. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="expectedBitmask">  The expected bitmask. </param>
    /// <param name="secondaryBitmask"> The secondary bitmask. </param>
    /// <param name="timeout">          The timeout. </param>
    public static void AssertBitmasksShouldBeSet( cc.isr.VI.Pith.SessionBase? session, int expectedBitmask, int secondaryBitmask, TimeSpan timeout )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        (_, int status) = (false, expectedBitmask);
        (bool timedOut, int statusByte, TimeSpan _) = session.AwaitStatusBitmask( expectedBitmask, timeout );
        Assert.IsFalse( timedOut, $"Awaiting 0x{expectedBitmask:X2} should not time out" );
        Assert.AreNotEqual( 0, statusByte & status,
            $"Status byte 0x{statusByte:X2} 0x{status:X2} bitmask should be set" );
        Assert.AreNotEqual( 0, statusByte & secondaryBitmask,
            $"Status byte 0x{statusByte:X2} 0x{secondaryBitmask:X2} bitmask should also be set" );
        Base.TestBase.ConsoleOutputMemberMessage( $"{session.OpenResourceModel} status byte 0x{statusByte:X2} 0x{status:X2} bitmask was set" );
    }

    /// <summary> Assert that the Visa Session should await for status bitmasks set and clear. </summary>
    /// <param name="session">      The session. </param>
    /// <param name="setBitmask">   The set bitmask. </param>
    /// <param name="clearBitmask"> The clear bitmask. </param>
    /// <param name="timeout">      The timeout. </param>
    public static void AssertSessionShouldWaitForBitmaskSetAndClear( cc.isr.VI.Pith.SessionBase? session, cc.isr.VI.Pith.ServiceRequests setBitmask,
        cc.isr.VI.Pith.ServiceRequests clearBitmask, TimeSpan timeout )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        (bool _, cc.isr.VI.Pith.ServiceRequests expectedStatus) = (false, setBitmask);
        (bool timedOut, cc.isr.VI.Pith.ServiceRequests status, TimeSpan _) = session.AwaitStatusBitmask( setBitmask, timeout );
        Assert.IsFalse( timedOut, $"Awaiting 0x{( int ) setBitmask:X2} should not time out" );
        Assert.AreNotEqual( 0, ( int ) status & ( int ) expectedStatus,
            $"Status byte 0x{( int ) status:X2} 0x{( int ) expectedStatus:X2} bitmask should be set" );
        Assert.AreEqual( 0, ( int ) status & ( int ) clearBitmask,
            $"Status byte 0x{( int ) status:X2} 0x{( int ) clearBitmask:X2} bitmask should also be clear" );
        Base.TestBase.ConsoleOutputMemberMessage( $"{session.OpenResourceModel} status byte 0x{( int ) status:X2} 0x{( int ) expectedStatus:X2} bitmask was set" );
    }

    /// <summary> Assert that the session should wait for a status bitmask. </summary>
    /// <param name="session">  The session. </param>
    public static void AssertSessionShouldWaitForStatusBitmask( cc.isr.VI.Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        if ( session.ServiceRequestEventEnabled )
        {
            Base.TestBase.ConsoleOutputMemberMessage( $"{session.OpenResourceModel} Disabling service request" );
            session.DisableServiceRequestEventHandler();
        }
        else
        {
            Base.TestBase.ConsoleOutputMemberMessage( $"{session.OpenResourceModel} service request handler was disabled" );
        }

        Assert.IsFalse( session.ServiceRequestEventEnabled, $"Service request event should be disabled" );
        string queryCommand = "*CLS; *WAI";
        _ = session.WriteLine( queryCommand );
        _ = session.StatusReadDelay.AsyncDelay();
        TimeSpan timeout = TimeSpan.FromMilliseconds( 200d );
        cc.isr.VI.Pith.ServiceRequests expectedBitmask = cc.isr.VI.Pith.ServiceRequests.StandardEventSummary;
        (bool TimedOut, cc.isr.VI.Pith.ServiceRequests StatusByte, TimeSpan Elapsed) actualOutcome = session.AwaitStatusBitmask( expectedBitmask, timeout );

        // session.MakeEmulatedReplyIfEmpty(expectedBitmask)
        System.Threading.Tasks.Task<(bool TimedOut,
            cc.isr.VI.Pith.ServiceRequests StatusByte,
            TimeSpan Elapsed)> task = cc.isr.VI.Pith.SessionBase.StartAwaitingBitmaskTask( expectedBitmask, timeout, session.ReadStatusByte );
        task.Wait();
        // actualOutcome = (Not .Wait(timeout), CType(.Result, cc.isr.VI.Pith.ServiceRequests))
        actualOutcome = task.Result;
        actualOutcome = session.AwaitStatusBitmask( expectedBitmask, timeout );
        Assert.IsTrue( actualOutcome.TimedOut, $"waiting for event summary event on *CLS is expected to time out" );
    }

    /// <summary>   Assert session should clear execution state. </summary>
    /// <remarks>   2024-07-31. </remarks>
    /// <param name="session">  The session. </param>
    public static void AssertSessionShouldClearExecutionState( cc.isr.VI.Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsTrue( session.IsDeviceOpen, "Attempted a call with closed session" );
        string propertyName = nameof( session.ClearExecutionState ).SplitWords();
        Base.TestBase.ConsoleOutputMemberMessage( $"Testing session {propertyName}" );
        session.ClearExecutionState();
        Assert.IsTrue( session.IsDeviceOpen, $"Device should remain open after {propertyName}" );
    }

    /// <summary>   Assert that executing a naked Clear Status Command might clear the register enable bitmasks. </summary>
    /// <remarks>   2024-07-31. </remarks>
    /// <param name="session">              The session. </param>
    public static void AssertNakedClsMightClearEnableBitmasks( cc.isr.VI.Pith.SessionBase? session, int standardEventEnableBitmask, int serviceRequestEnableBitmask )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        string propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        int actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{actualStandardEventEnableBitmask:X2}" );

        // program the standard event request register.

        int expectedStandardEventEnableBitmask = standardEventEnableBitmask;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableOperationCompleteBitmask ).SplitWords();
        Assert.IsGreaterThan( 0, expectedStandardEventEnableBitmask, $"{propertyName} 0x{expectedStandardEventEnableBitmask:X2} should be positive" );

        session.EnableServiceRequestOnOperationCompletion( standardEventEnableBitmask, serviceRequestEnableBitmask, true );
        actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableOperationCompleteBitmask ).SplitWords();
        Assert.AreEqual( expectedStandardEventEnableBitmask, actualStandardEventEnableBitmask, $"{propertyName} should match after enabling" );

        // clear execution state might clear the enable bitmasks.
        session.ClearExecutionStateQueryComplete();
        //_ = session.WriteLine( session.ClearExecutionStateCommand );
        //_ = SessionBase.AsyncDelay( session.ClearRefractoryPeriod );

        // this command now restore the enabled registers if the session is known to clear them.
        // session.ClearExecutionState();
        actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        if ( session.StatusClearDistractive )
            expectedStandardEventEnableBitmask = 0;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableOperationCompleteBitmask ).SplitWords();
        Assert.AreEqual( expectedStandardEventEnableBitmask, actualStandardEventEnableBitmask, $"{propertyName} should match after clearing execution state." );
        Base.TestBase.ConsoleOutputMemberMessage( $"Exists {propertyName} is 0x{actualStandardEventEnableBitmask:X2}" );
    }

    /// <summary>   Assert wait complete should enable. </summary>
    /// <remarks>   2024-07-31. </remarks>
    /// <param name="session">              The session. </param>
    /// <param name="clearClearsBitmasks">  True if *CLS clears the standard event enable bitmask. </param>
    public static void AssertWaitCompleteShouldEnable( cc.isr.VI.Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        string propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        cc.isr.VI.Pith.StandardEvents actualStandardEventEnableBitmask = ( cc.isr.VI.Pith.StandardEvents ) ( int ) session.QueryStandardEventEnableBitmask()!;
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{( int ) actualStandardEventEnableBitmask:X2}" );

        // program the standard event request register.

        cc.isr.VI.Pith.StandardEvents expectedStandardEventEnableBitmask = session.StandardEventEnableOperationCompleteBitmask;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableOperationCompleteBitmask ).SplitWords();
        Assert.IsGreaterThan( 0, ( int ) expectedStandardEventEnableBitmask, $"{propertyName} 0x{( int ) expectedStandardEventEnableBitmask:X2} should be positive" );
        session.EnableServiceRequestOnOperationCompletion( true );
        actualStandardEventEnableBitmask = ( cc.isr.VI.Pith.StandardEvents ) ( int ) session.QueryStandardEventEnableBitmask()!;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableOperationCompleteBitmask ).SplitWords();
        Assert.AreEqual( expectedStandardEventEnableBitmask, actualStandardEventEnableBitmask, $"{propertyName} should match after enabling" );
        Base.TestBase.ConsoleOutputMemberMessage( $"Exists {propertyName} is 0x{( int ) actualStandardEventEnableBitmask:X2}" );
    }

    /// <summary> Assert should enable wait complete service request. </summary>
    /// <param name="session">  The session. </param>
    public static void AssertWaitCompleteServiceRequestShouldEnable( cc.isr.VI.Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        string propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableBitmask ).SplitWords();
        cc.isr.VI.Pith.ServiceRequests actualServiceRequestEnableBitmask = ( cc.isr.VI.Pith.ServiceRequests ) session.QueryServiceRequestEnableBitmask();
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{( int ) actualServiceRequestEnableBitmask:X2}" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        cc.isr.VI.Pith.StandardEvents actualStandardEventEnableBitmask = ( cc.isr.VI.Pith.StandardEvents ) ( int ) session.QueryStandardEventEnableBitmask()!;
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{( int ) actualStandardEventEnableBitmask:X2}" );

        // program the service request register.
        cc.isr.VI.Pith.ServiceRequests expectedServiceRequestEnableBitmask = session.ServiceRequestEnableOperationCompleteBitmask;
        cc.isr.VI.Pith.StandardEvents expectedStandardEventEnableBitmask = session.StandardEventEnableOperationCompleteBitmask;
        session.EnableServiceRequestOnOperationCompletion();
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableBitmask ).SplitWords();
        Assert.AreEqual( expectedServiceRequestEnableBitmask, session.ServiceRequestEnableOperationCompleteBitmask, $"{propertyName} should match after enabling" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        Assert.AreEqual( expectedStandardEventEnableBitmask, session.StandardEventEnableOperationCompleteBitmask, $"{propertyName} should match after enabling" );
        session.ClearExecutionState();
        // this is already done as part of the above command
        // _ = session.QueryOperationCompleted();
        actualStandardEventEnableBitmask = ( cc.isr.VI.Pith.StandardEvents ) ( int ) session.QueryStandardEventEnableBitmask()!;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableOperationCompleteBitmask ).SplitWords();
        Assert.AreEqual( expectedStandardEventEnableBitmask, actualStandardEventEnableBitmask, $"{propertyName} should match after clearing execution state." );
        Base.TestBase.ConsoleOutputMemberMessage( $"Exists {propertyName} is 0x{( int ) actualStandardEventEnableBitmask:X2}" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableBitmask ).SplitWords();
        actualServiceRequestEnableBitmask = ( cc.isr.VI.Pith.ServiceRequests ) session.QueryServiceRequestEnableBitmask();
        Assert.AreEqual( expectedServiceRequestEnableBitmask, actualServiceRequestEnableBitmask, $"{propertyName} should match after clearing execution state." );
        Base.TestBase.ConsoleOutputMemberMessage( $"Exists {propertyName} is 0x{( int ) actualServiceRequestEnableBitmask:X2}" );
    }

    /// <summary>   Assert if the clear execution state command clears the event enable registers. </summary>
    /// <remarks>   2024-08-02. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="statusSettings">   The status settings. </param>
    public static void AssertClearExecutionStatePreservesEnableBitmasks( cc.isr.VI.Pith.SessionBase? session, int standardEventEnableBitmask, int serviceRequestEnableBitmask )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsTrue( session.TimingSettings.Exists );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        string propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableBitmask ).SplitWords();
        int actualServiceRequestEnableBitmask = session.QueryServiceRequestEnableBitmask();
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{actualServiceRequestEnableBitmask:X2}: {actualServiceRequestEnableBitmask}" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        int actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{actualStandardEventEnableBitmask:X2}: {actualStandardEventEnableBitmask}" );

        // program the service request register.
        int expectedServiceRequestEnableBitmask = serviceRequestEnableBitmask;
        int expectedStandardEventEnableBitmask = standardEventEnableBitmask;
        // session.ApplyStandardServiceRequestEnableBitmasks( expectedStandardEventEnableBitmask, expectedServiceRequestEnableBitmask );
        session.WriteStandardServiceRequestEnableBitmasks( expectedStandardEventEnableBitmask, expectedServiceRequestEnableBitmask );
        actualServiceRequestEnableBitmask = session.QueryServiceRequestEnableBitmask();
        actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableBitmask ).SplitWords();
        Assert.AreEqual( expectedServiceRequestEnableBitmask, actualServiceRequestEnableBitmask, $"{propertyName} should match after enabling" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        Assert.AreEqual( expectedStandardEventEnableBitmask, actualStandardEventEnableBitmask, $"{propertyName} should match after enabling" );
        // this now preserves the enable registers
        session.ClearExecutionState();

#if false
            this.SetLastAction( "storing the last standard event enable bitmask" );
            _ = this.QueryStandardEventEnableBitmask();
            this.SetLastAction( "storing the last service request event enable bitmask" );
            _ = this.QueryServiceRequestEnableBitmask();
#endif
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableBitmask ).SplitWords();
        actualServiceRequestEnableBitmask = session.QueryServiceRequestEnableBitmask();
        Assert.AreEqual( expectedServiceRequestEnableBitmask, actualServiceRequestEnableBitmask, $"{propertyName} should match after clearing execution state." );
        actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableOperationCompleteBitmask ).SplitWords();
        Assert.AreEqual( expectedStandardEventEnableBitmask, actualStandardEventEnableBitmask, $"{propertyName} should match after clearing execution state." );
    }

    /// <summary> Assert standard service request should enable. </summary>
    /// <param name="session">  The session. </param>
    public static void AssertStandardServiceRequestShouldEnable( cc.isr.VI.Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        string propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableBitmask ).SplitWords();
        int actualServiceRequestEnableBitmask = session.QueryServiceRequestEnableBitmask();
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{actualServiceRequestEnableBitmask:X2}: {actualServiceRequestEnableBitmask}" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        int actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{actualStandardEventEnableBitmask:X2}: {actualStandardEventEnableBitmask}" );

        // program the service request register.
        int expectedServiceRequestEnableBitmask = session.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask;
        int expectedStandardEventEnableBitmask = session.RegistersBitmasksSettings.StandardEventEnableEventsBitmask;
        session.ApplyStandardServiceRequestEnableBitmasks( expectedStandardEventEnableBitmask, expectedServiceRequestEnableBitmask );
        actualServiceRequestEnableBitmask = session.QueryServiceRequestEnableBitmask();
        actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableBitmask ).SplitWords();
        Assert.AreEqual( expectedServiceRequestEnableBitmask, actualServiceRequestEnableBitmask, $"{propertyName} should match after enabling" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        Assert.AreEqual( expectedStandardEventEnableBitmask, actualStandardEventEnableBitmask, $"{propertyName} should match after enabling" );
    }

    /// <summary> Assert standard service request should disable. </summary>
    /// <param name="session">  The session. </param>
    public static void AssertStandardServiceRequestShouldDisable( cc.isr.VI.Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        string propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableOperationCompleteBitmask ).SplitWords();
        int actualServiceRequestEnableBitmask = session.QueryServiceRequestEnableBitmask();
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{actualServiceRequestEnableBitmask:X2}" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        int actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        Base.TestBase.ConsoleOutputMemberMessage( $"Initial {propertyName} is 0x{actualStandardEventEnableBitmask:X2}" );

        // program the service request register.
        int expectedServiceRequestEnableBitmask = 0;
        int expectedStandardEventEnableBitmask = 0;
        session.ApplyStandardServiceRequestEnableBitmasks( expectedStandardEventEnableBitmask, expectedServiceRequestEnableBitmask );
        actualServiceRequestEnableBitmask = session.QueryServiceRequestEnableBitmask();
        actualStandardEventEnableBitmask = ( int ) session.QueryStandardEventEnableBitmask()!;
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.ServiceRequestEnableBitmask ).SplitWords();
        Assert.AreEqual( expectedServiceRequestEnableBitmask, actualServiceRequestEnableBitmask, $"{propertyName} should match after enabling" );
        propertyName = nameof( cc.isr.VI.Pith.SessionBase.StandardEventEnableBitmask ).SplitWords();
        Assert.AreEqual( expectedStandardEventEnableBitmask, actualStandardEventEnableBitmask, $"{propertyName} should match after enabling" );
    }

    /// <summary>   Assert that the session should wait for the message available bitmask. </summary>
    /// <remarks>   2024-08-01. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="resourceSettings"> The resource settings. </param>
    /// <param name="statusSettings">   The status settings. </param>
    public static void AssertSessionShouldWaitForMessageAvailable( cc.isr.VI.Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsTrue( session.IsDeviceOpen, "Attempted a call with closed session" );

        // set service request enable
        AssertStandardServiceRequestShouldEnable( session );

        // reading the status byte using the status byte query command.
        string queryCommand = session.StatusByteQueryCommand;
        int maskedStatus;

        session.SetLastAction( "sending the status byte query command" );
        _ = session.WriteLine( queryCommand );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );
        try
        {
            // after the query command, the message available bit should be set
            AssertBitmasksShouldBeSet( session, session.MessageAvailableBitmask, session.RequestingServiceBitmask, TimeSpan.FromMilliseconds( 200d ) );
        }
        catch
        {
            throw;
        }
        finally
        {
            // then we should be able to read the result
            session.SetLastAction( "reading the reply to the status byte query command" );
            string result = session.ReadLineTrimEnd();
            if ( int.TryParse( result, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture, out int value ) )
            {
                // result = $"{value:X2}";
                maskedStatus = value & ( int ) cc.isr.VI.Pith.ServiceRequests.MessageAvailable;
                Assert.AreEqual( 0, maskedStatus,
                    $"The message available bit {( int ) cc.isr.VI.Pith.ServiceRequests.MessageAvailable:X2} must be off in the status byte 0x{value:X2}." );
            }
            _ = SessionBase.AsyncDelay( session.StatusReadDelay );

            // must refresh the service request here to update the status register flags.
            ServiceRequests statusByte = session.ReadStatusByte();

            maskedStatus = ( int ) statusByte & ( int ) cc.isr.VI.Pith.ServiceRequests.MessageAvailable;
            Assert.AreEqual( 0, maskedStatus,
                $"The message available bit {( int ) cc.isr.VI.Pith.ServiceRequests.MessageAvailable:X2}  must be of after read status byte {( int ) statusByte:X2}." );
        }
    }

    /// <summary>   Assert query result should be detected. </summary>
    /// <remarks>   David, 2021-03-31. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="queryCommand">     The query command. </param>
    /// <param name="expectedBitmask">  The expected bitmask. </param>
    /// <param name="expectedReply">    The expected reply. </param>
    public static void AssertQueryResultShouldBeDetected( cc.isr.VI.Pith.SessionBase? session, string? queryCommand, byte expectedBitmask, string? expectedReply )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( queryCommand );
        Assert.IsNotNull( expectedReply );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );

        // write query command
        _ = session.WriteLine( queryCommand );
        try
        {
            AssertBitmaskShouldBeSet( session, expectedBitmask, TimeSpan.FromMilliseconds( 200d ) );
        }
        catch
        {
            throw;
        }
        finally
        {
        }
        string actualReply = session.ReadLineTrimEnd();
        Assert.IsTrue( expectedReply.StartsWith( actualReply, StringComparison.OrdinalIgnoreCase ), $"{queryCommand} returned {actualReply} should return {expectedReply}" );
        // must refresh the service request here to update the status register flags.
        _ = SessionBase.AsyncDelay( session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError( session.ReadStatusByte() );
    }

    /// <summary> Assert that the session should wait for operation completion. </summary>
    /// <param name="session">       The session. </param>
    /// <param name="actionCommand"> The action command, which, by added '?' or value can be used to
    /// read current value and set the same value thus not changing
    /// things. </param>
    public static void AssertSessionShouldWaitForOperationCompletion( cc.isr.VI.Pith.SessionBase? session, string? actionCommand )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( actionCommand );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        AssertWaitCompleteShouldEnable( session );
        _ = session.WriteLine( actionCommand );
        AssertBitmaskShouldBeSet( session, cc.isr.VI.Pith.ServiceRequests.StandardEventSummary, TimeSpan.FromMilliseconds( 200d ) );
    }

    /// <summary> Assert that the session should wait for service request operation completion. </summary>
    /// <param name="session">       The session. </param>
    /// <param name="actionCommand"> The action command, which, by added '?' or value can be used to
    /// read current value and set the same value thus not changing
    /// things. </param>
    public static void AssertSessionShouldWaitForServiceRequestOperationCompletion( cc.isr.VI.Pith.SessionBase? session, string? actionCommand )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );
        Assert.IsNotNull( actionCommand );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        AssertWaitCompleteServiceRequestShouldEnable( session );
        _ = session.WriteLine( actionCommand );
        AssertBitmasksShouldBeSet( session, cc.isr.VI.Pith.ServiceRequests.StandardEventSummary, cc.isr.VI.Pith.ServiceRequests.RequestingService, TimeSpan.FromMilliseconds( 200d ) );
    }

    /// <summary>   Assert service request handling should toggle. </summary>
    /// <remarks>   Leaves the system at it present start. </remarks>
    /// <param name="session">  The session. </param>
    public static void AssertServiceRequestHandlingShouldToggle( cc.isr.VI.Pith.SessionBase? session )
    {
        Assert.IsNotNull( session, $"{nameof( session )} should not be null." );

        Assert.IsTrue( session.IsDeviceOpen, $"Attempted a call with closed session" );
        for ( int i = 1; i <= 2; i++ )
        {
            if ( session.ServiceRequestEventEnabled )
            {
                Base.TestBase.ConsoleOutputMemberMessage( "Disabling service request handling" );
                session.DisableServiceRequestEventHandler();
                Assert.IsFalse( session.ServiceRequestEventEnabled, $"Service request event not disabled" );
            }
            else
            {
                Base.TestBase.ConsoleOutputMemberMessage( "Enabling service request handling" );
                session.EnableServiceRequestEventHandler();
                Assert.IsTrue( session.ServiceRequestEventEnabled, $"Service request event not enabled" );
            }
        }
    }

    #endregion
}
