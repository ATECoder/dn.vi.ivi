using System.Diagnostics;

namespace cc.isr.Visa.Tests;

/// <summary>   Implements specific assertion tests. </summary>
/// <remarks>   David, 2021-11-05. </remarks>
internal sealed class Asserts
{
    #region " console output "

    /// <summary>   Console output member message. </summary>
    /// <remarks>   2025-04-28. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="withPath">         (Optional) True to with path. </param>
    /// <param name="withFileName">     (Optional) True to with file name. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void ConsoleOutputMemberMessage( string message, bool withPath = false, bool withFileName = true,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string member = withPath
            ? $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}"
            : withFileName
              ? $"{System.IO.Path.GetFileNameWithoutExtension( sourcePath )}.{memberName}.Line#{sourceLineNumber}"
              : $"{memberName}.Line#{sourceLineNumber}";

        Console.Out.WriteLine( $"{member}:\r\n\t{message}" );
    }

    #endregion

    /// <summary>   Opens an <see cref="Ivi.Visa.IVisaSession?"/>. </summary>
    /// <remarks>   2024-08-10. </remarks>
    /// <param name="resourceName">         Name of the resource. </param>
    /// <param name="openTimeout">          The open openTimeout. </param>
    /// <param name="communicationTimeout"> The communication openTimeout. </param>
    /// <returns>   An <see cref="Ivi.Visa.IVisaSession?"/> </returns>
    public static Ivi.Visa.IVisaSession? OpenVisaSession( string resourceName, TimeSpan openTimeout, TimeSpan communicationTimeout )
    {
        // Initialization:
        _ = Ivi.Visa.GlobalResourceManager.Parse( resourceName ) ?? throw new InvalidOperationException( $"Failed parsing the resource name: {resourceName}." );

        Ivi.Visa.IVisaSession? visaSession = Ivi.Visa.GlobalResourceManager.Open( resourceName, Ivi.Visa.AccessModes.None, ( int ) openTimeout.TotalMilliseconds );
        Assert.IsNotNull( visaSession, $"Failed opening an {nameof( Ivi.Visa.IVisaSession )} to resource name {resourceName}" );

        // Timeout for VISA Read Operations
        visaSession.TimeoutMilliseconds = ( int ) communicationTimeout.TotalMilliseconds;

        // Use SynchronizeCallbacks to specify that the object marshals callbacks across threads appropriately.
        visaSession.SynchronizeCallbacks = true;

        return visaSession;
    }

    /// <summary>   Tries to open an <see cref="Ivi.Visa.IVisaSession?"/>. </summary>
    /// <remarks>   2024-08-10. </remarks>
    /// <param name="resourceName">         Name of the resource. </param>
    /// <param name="openTimeout">          The open openTimeout. </param>
    /// <param name="communicationTimeout"> The communication openTimeout. </param>
    /// <returns>   An <see cref="Ivi.Visa.IVisaSession?"/> </returns>
    public static Ivi.Visa.IVisaSession? TryOpenVisaSession( string resourceName, TimeSpan openTimeout, TimeSpan communicationTimeout )
    {
        Ivi.Visa.IVisaSession? visaSession = null;
        // Separate try-catch for the instrument initialization prevents accessing uninitialized object
        try
        {
            visaSession = OpenVisaSession( resourceName, openTimeout, communicationTimeout );
            Assert.IsNotNull( visaSession, $"Failed opening an {nameof( Ivi.Visa.IVisaSession )} to resource name {resourceName}" );
        }
        catch ( Ivi.Visa.NativeVisaException e )
        {
            Asserts.ConsoleOutputMemberMessage( $"\r\nError initializing an {nameof( Ivi.Visa.IVisaSession )} at '{resourceName}' :\r\n\t{e.Message}" );
        }
        finally
        {
        }
        return visaSession;
    }

    /// <summary>   Opens an <see cref="Ivi.Visa.IMessageBasedSession?"/>. </summary>
    /// <remarks>   2024-08-10. </remarks>
    /// <param name="resourceName">         Name of the resource. </param>
    /// <param name="openTimeout">          The open openTimeout. </param>
    /// <param name="communicationTimeout"> The communication openTimeout. </param>
    /// <returns>   An <see cref="Ivi.Visa.IMessageBasedSession?"/> </returns>
    public static Ivi.Visa.IMessageBasedSession? OpenMessageBasedSession( string resourceName, TimeSpan openTimeout, TimeSpan communicationTimeout )
    {
        Ivi.Visa.IMessageBasedSession? messageBasedSession = null;
        Ivi.Visa.IVisaSession? visaSession = TryOpenVisaSession( resourceName, openTimeout, communicationTimeout );
        Assert.IsNotNull( visaSession, $"Failed opening an {nameof( Ivi.Visa.IVisaSession )} to resource name {resourceName}" );

        messageBasedSession = ( Ivi.Visa.IMessageBasedSession ) visaSession;
        Assert.IsNotNull( messageBasedSession, $"Failed opening an {nameof( Ivi.Visa.IMessageBasedSession )} to resource name {resourceName}" );

        // clear the instrument (SDC)
        messageBasedSession.Clear();

        return messageBasedSession;
    }


    /// <summary>   Tries to open an <see cref="Ivi.Visa.IMessageBasedSession?"/>. </summary>
    /// <remarks>   2024-08-10. </remarks>
    /// <param name="resourceName">         Name of the resource. </param>
    /// <param name="openTimeout">          The open openTimeout. </param>
    /// <param name="communicationTimeout"> The communication openTimeout. </param>
    /// <returns>   An <see cref="Ivi.Visa.IMessageBasedSession?"/> </returns>
    public static Ivi.Visa.IMessageBasedSession? TryOpenMessageBasedSession( string resourceName, TimeSpan openTimeout, TimeSpan communicationTimeout )
    {
        Ivi.Visa.IMessageBasedSession? messageBasedSession = null;
        try
        {
            messageBasedSession = OpenMessageBasedSession( resourceName, openTimeout, communicationTimeout );
            Assert.IsNotNull( messageBasedSession, $"Failed opening an {nameof( Ivi.Visa.IMessageBasedSession )} to resource name {resourceName}" );
        }
        catch ( Ivi.Visa.NativeVisaException e )
        {
            Asserts.ConsoleOutputMemberMessage( $"\nError initializing an {nameof( Ivi.Visa.IMessageBasedSession )} at '{resourceName}' :\n{e.Message}" );
        }
        finally
        {
        }
        return messageBasedSession;

    }

    /// <summary>   Asserts opening a visa session. </summary>
    /// <remarks>   David, 2021-11-05. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="openTimeout">      The openTimeout. </param>
    /// <returns>   An <see cref="Ivi.Visa.IVisaSession?"/> </returns>
    public static Ivi.Visa.IVisaSession AssertVisaSessionShouldOpen( string resourceName, TimeSpan openTimeout, TimeSpan communicationTimeout )
    {
        Ivi.Visa.IVisaSession? session = OpenVisaSession( resourceName, openTimeout, communicationTimeout );
        Assert.IsNotNull( session, $"Failed opening an {nameof( Ivi.Visa.IVisaSession )} to resource name {resourceName}" );
        Assert.AreEqual( resourceName, session.ResourceName, "Session resource name must match" );
        session.SynchronizeCallbacks = true;
        return session;
    }

    /// <summary>   Asserts opening a message based visa session. </summary>
    /// <remarks>   2024-08-10. </remarks>
    /// <param name="resourceName">         Name of the resource. </param>
    /// <param name="openTimeout">          The open openTimeout. </param>
    /// <param name="communicationTimeout"> The communication openTimeout. </param>
    /// <returns>   An <see cref="Ivi.Visa.IMessageBasedSession?"/> </returns>
    public static Ivi.Visa.IMessageBasedSession AssertMessageBasedSessionShouldOpen( string resourceName, TimeSpan openTimeout, TimeSpan communicationTimeout )
    {
        Ivi.Visa.IMessageBasedSession? session = OpenMessageBasedSession( resourceName, openTimeout, communicationTimeout );
        Assert.IsNotNull( session, $"Failed opening an {nameof( Ivi.Visa.IMessageBasedSession )} to resource name {resourceName}" );
        Assert.AreEqual( resourceName, session.ResourceName, "Session resource name must match" );
        session.SynchronizeCallbacks = true;
        return session;
    }

    /// <summary>   Assert visa session should await service requests. </summary>
    /// <remarks>   David, 2021-11-05. </remarks>
    /// <param name="session">                          The session. </param>
    /// <param name="enableServiceRequestCommand">      The enable service request command. </param>
    /// <param name="enableOperationCompletionCommand"> The enable operation completion command. </param>
    /// <param name="operationCompleteQueryCommand">    The operation complete query command. </param>
    /// <param name="expectedServiceRequestMessage">    Message describing the expected service
    ///                                                 request. </param>
    /// <param name="repeatCount">                      Number of repeats. </param>
    public static void AssertSessionShouldWaitForServiceRequests( Ivi.Visa.IMessageBasedSession session,
                                                                  string enableServiceRequestCommand,
                                                                  string enableOperationCompletionCommand,
                                                                  string operationCompleteQueryCommand,
                                                                  string expectedServiceRequestMessage, int repeatCount )
    {
        session.ServiceRequest += OnServiceRequest;
        WriteToSession( session, enableServiceRequestCommand );
        WriteToSession( session, enableOperationCompletionCommand );

        for ( int i = 0; i < repeatCount; i++ )
        {
            // clear previous values.
            ClearServiceRequestValues();

            // query operation completion
            WriteToSession( session, operationCompleteQueryCommand );

            // wait for the service request message to come in.
            StartAwaitingServiceRequestReadingTask( TimeSpan.FromMilliseconds( 200d ) ).Wait();

            // assert receipt.
            Assert.IsTrue( string.IsNullOrWhiteSpace( ServiceRequestErrorMessage ),
                                                $"Service request trial #{i + 1} should not return an error: {ServiceRequestErrorMessage}" );
            Assert.AreNotEqual( 0, ( int? ) ServiceRequestFlags,
                                                 $"Service request trial #{i + 1} flags should not be {ServiceRequestFlags}" );
            Assert.AreEqual( expectedServiceRequestMessage, ServiceRequestMessage,
                                                $"Service request trial #{i + 1} should return the expected message" );
        }

    }

    #region " service request handling "

    /// <summary>   Gets or sets a message describing the service request. </summary>
    /// <value> A message describing the service request. </value>
    public static string? ServiceRequestMessage { get; set; }

    /// <summary>   Gets or sets a message describing the service request error. </summary>
    /// <value> A message describing the service request error. </value>
    public static string? ServiceRequestErrorMessage { get; set; }

    /// <summary>   Gets or sets the service request flags. </summary>
    /// <value> Options that control the service request. </value>
    public static Ivi.Visa.StatusByteFlags? ServiceRequestFlags { get; set; }

    /// <summary>   Clears the service request values. </summary>
    /// <remarks>   David, 2021-11-06. </remarks>
    public static void ClearServiceRequestValues()
    {
        ServiceRequestMessage = string.Empty;
        ServiceRequestErrorMessage = string.Empty;
        ServiceRequestFlags = 0;
    }

    /// <summary>   Starts awaiting service request reading task. </summary>
    /// <remarks>   David, 2021-11-06. </remarks>
    /// <param name="timeout">  The openTimeout. </param>
    /// <returns>   A Task. </returns>
    public static async Task StartAwaitingServiceRequestReadingTask( TimeSpan timeout )
    {
        await Task.Factory.StartNew( () =>
        {
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            while ( string.IsNullOrWhiteSpace( ServiceRequestMessage )
                        && string.IsNullOrWhiteSpace( ServiceRequestErrorMessage )
                        && sw.ElapsedTicks < ticks )
                Thread.SpinWait( 1 );
        } ).ConfigureAwait( false );
    }

    /// <summary>   Starts awaiting predicate task. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    /// <param name="timeout">      The openTimeout. </param>
    /// <param name="predicate">    The predicate. </param>
    /// <returns>   A Task. </returns>
    public static async Task StartAwaitingPredicateTask( TimeSpan timeout, Func<bool> predicate, Action abort )
    {
        await Task.Factory.StartNew( () =>
        {
            long ticks = timeout.Ticks;
            Stopwatch sw = Stopwatch.StartNew();
            bool timedOut = false;
            bool done = predicate.Invoke() || sw.ElapsedTicks > ticks;
            while ( !done )
            {
                Thread.SpinWait( 1 );
                timedOut = sw.ElapsedTicks > ticks;
                done = predicate.Invoke() || timedOut;
            }
            if ( timedOut ) abort?.Invoke();
        } ).ConfigureAwait( false );
    }

    /// <summary>   Handles the visa event. </summary>
    /// <remarks>   David, 2021-11-05. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information to send to registered event handlers. </param>
    private static void OnServiceRequest( object? sender, Ivi.Visa.VisaEventArgs e )
    {
        try
        {
            Ivi.Visa.IMessageBasedSession? mbs = sender as Ivi.Visa.IMessageBasedSession;
            ServiceRequestFlags = mbs?.ReadStatusByte();

            if ( (ServiceRequestFlags & Ivi.Visa.StatusByteFlags.MessageAvailable) != 0 )
            {
                string? textRead = mbs?.RawIO.ReadString();
                if ( textRead is not null )
                    ServiceRequestMessage = InsertCommonEscapeSequences( textRead );
                else
                    ServiceRequestErrorMessage = $"MAV in status byte {ServiceRequestFlags} is not set for event {e.EventType}, which means that message is not available. Is the command to enable SRQ correct? Is the instrument is 488.2 compatible?";
            }
        }
        catch ( Exception ex )
        {
            ServiceRequestErrorMessage = ex.ToString();
        }
    }

    #endregion

    #region " supporting functions "

    /// <summary>   Replace common escape sequences. </summary>
    /// <remarks>   David, 2021-11-05. </remarks>
    /// <param name="s">    The string. </param>
    /// <returns>   A <see cref="string" />. </returns>
    public static string ReplaceCommonEscapeSequences( string s )
    {
        return s.Replace( "\\n", "\n" ).Replace( "\\r", "\r" );
    }

    /// <summary>   Inserts a common escape sequences described by s. </summary>
    /// <remarks>   David, 2021-11-05. </remarks>
    /// <param name="s">    The string. </param>
    /// <returns>   A <see cref="string" />. </returns>
    public static string InsertCommonEscapeSequences( string s )
    {
        return s.Replace( "\n", "\\n" ).Replace( "\r", "\\r" );
    }

    /// <summary>   Writes to session. </summary>
    /// <remarks>   David, 2021-11-05. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="txtWrite"> The text write. </param>
    public static void WriteToSession( Ivi.Visa.IMessageBasedSession session, string txtWrite )
    {
        string textToWrite = ReplaceCommonEscapeSequences( txtWrite );
        session.RawIO.Write( textToWrite );
    }

    #endregion

    #region " sync wait "

    /// <summary>   Asynchronous wait. </summary>
    /// <remarks>   2024-08-10. </remarks>
    /// <param name="duration"> The duration. </param>
    /// <returns>   A TimeSpan. </returns>
    public static TimeSpan AsyncWait( TimeSpan duration )
    {
        return Task.Run( () => SyncWait( duration ) ).Result;
    }

    /// <summary>   Waits for the specified duration. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    /// <param name="duration"> The duration. </param>
    /// <returns>   The elapsed time. </returns>
    public static TimeSpan SyncWait( TimeSpan duration )
    {
        Stopwatch sw = Stopwatch.StartNew();
        while ( sw.Elapsed < duration )
            Thread.SpinWait( 1 );
        return sw.Elapsed;
    }

    /// <summary>   Waits until the stopwatch time elapses. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    /// <param name="stopwatch">    The stopwatch. </param>
    /// <param name="duration">     The duration. </param>
    /// <returns>   The elapsed time. </returns>
    public static TimeSpan SyncLetElapse( Stopwatch stopwatch, TimeSpan duration )
    {
        while ( stopwatch.Elapsed <= duration )
            Thread.SpinWait( 1 );
        return stopwatch.Elapsed;
    }

    /// <summary>   Waits until the stopwatch time elapses. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    /// <param name="stopwatch">        The stopwatch. </param>
    /// <param name="duration">         The duration. </param>
    /// <param name="loopDelay">        The loop delay. </param>
    /// <param name="doEventsAction">   The do events action. </param>
    /// <returns>   The elapsed time. </returns>
    public static TimeSpan SyncLetElapse( Stopwatch stopwatch, TimeSpan duration, TimeSpan loopDelay, Action doEventsAction )
    {
        while ( stopwatch.Elapsed <= duration )
        {
            if ( loopDelay > TimeSpan.Zero )
                _ = SyncWait( loopDelay );
            doEventsAction?.Invoke();
        }
        return stopwatch.Elapsed;
    }

    /// <summary>   Waits synchronously until the predicate action completed or openTimeout. </summary>
    /// <remarks>   David, 2021-04-01. </remarks>
    /// <param name="timeout">          The openTimeout. </param>
    /// <param name="onsetDelay">       The onset delay; before the first call to. </param>
    /// <param name="pollInterval">     Specifies time between serial polls. </param>
    /// <param name="predicate">        The completion predicate. </param>
    /// <param name="doEventsAction">   The do events action. </param>
    /// <returns> (true if completed; otherwise, timed out, elapsed time). </returns>
    public static (bool Completed, TimeSpan Elapsed) SyncAwaitUntil( TimeSpan timeout, TimeSpan onsetDelay,
                                                                     TimeSpan pollInterval, Func<bool> predicate, Action? doEventsAction )
    {
        long ticks = timeout.Ticks;
        Stopwatch sw = Stopwatch.StartNew();
        _ = SyncLetElapse( sw, onsetDelay );
        bool completed = predicate.Invoke();
        if ( timeout <= TimeSpan.Zero )
            return (false, TimeSpan.Zero);
        bool timedOut = sw.ElapsedTicks >= ticks;
        while ( !(completed || timedOut) )
        {
            Thread.SpinWait( 1 );
            doEventsAction?.Invoke();
            if ( pollInterval != TimeSpan.Zero )
                _ = SyncLetElapse( sw, sw.Elapsed.Add( pollInterval ) );
            completed = predicate.Invoke();
            timedOut = !completed && sw.ElapsedTicks >= ticks;
        }
        return (completed, sw.Elapsed);
    }

    #endregion

    #region " wait until "

    /// <summary>
    /// Starts a task waiting for a the predicate or openTimeout. The task complete after a openTimeout or if
    /// the action function value matches the bitmask value.
    /// </summary>
    /// <remarks>
    /// The task openTimeout is included in the task function. Otherwise, upon Wait(openTimeout), the task
    /// deadlocks attempting to get the task result. For more information see
    /// https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html. That document is short
    /// on examples for how to resolve this issue.
    /// </remarks>
    /// <param name="timeout">          The openTimeout. </param>
    /// <param name="onsetDelay">       The onset delay; before the first call to
    ///                                 <paramref name="predicate"/> </param>
    /// <param name="pollInterval">     Specifies time between serial polls. </param>
    /// <param name="predicate">        The predicate. </param>
    /// <param name="doEventsAction">   The do events action. </param>
    /// <returns>   A Threading.Tasks.Task(Of ( bool, TimeSpan ) ) (true if completed, elapsed time). </returns>
    public static Task<(bool, TimeSpan)> StartAwaitingUntilTask( TimeSpan timeout, TimeSpan onsetDelay, TimeSpan pollInterval,
                                                                 Func<bool> predicate, Action? doEventsAction )
    {
        return Task.Factory.StartNew<(bool, TimeSpan)>( () => SyncAwaitUntil( timeout, onsetDelay, pollInterval, predicate, doEventsAction ) );
    }

    /// <summary>   A TimeSpan extension method that await until. </summary>
    /// <remarks>   David, 2021-04-01. </remarks>
    /// <param name="timeout">          The openTimeout. </param>
    /// <param name="onsetDelay">       The time to wait before starting to query. </param>
    /// <param name="pollInterval">     Specifies time between serial polls. </param>
    /// <param name="predicate">        . </param>
    /// <param name="doEventsAction">   The do events action. </param>
    /// <returns>   (true if completed; otherwise timed out, elapsed time). </returns>
    public static (bool Completed, TimeSpan Elapsed) AsyncAwaitUntil( TimeSpan timeout, TimeSpan onsetDelay, TimeSpan pollInterval,
                                                                     Func<bool> predicate, Action? doEventsAction )
    {
        // emulate the reply for disconnected operations.
        CancellationTokenSource cts = new();
        Task<(bool, TimeSpan)> t = StartAwaitingUntilTask( timeout, onsetDelay, pollInterval, predicate, doEventsAction );
        t.Wait();
        return t.Result;
    }

    #endregion
}
