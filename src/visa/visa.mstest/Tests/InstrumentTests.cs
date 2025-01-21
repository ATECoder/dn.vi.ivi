using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace cc.isr.Visa.Tests;

/// <summary>   (Unit Test Class) instrument tests base class. </summary>
/// <remarks>   David, 2021-11-13. </remarks>
[TestClass]
public abstract partial class InstrumentTests
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
            Gac.GacLoader.LoadInstalledVisaAssemblies();
            Trace.WriteLine( "Initializing", methodFullName );
        }
        catch ( Exception ex )
        {
            Trace.WriteLine( $"Failed initializing the test class: {ex}", methodFullName );

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
    /// <remarks> Use <see cref="CleanupBaseTestClass"/> to run code after all tests in the class have run. </remarks>
    public static void CleanupBaseTestClass()
    { }

    /// <summary> Initializes the test class instance before each test runs. </summary>											   
    [TestInitialize()]
    public virtual void InitializeBeforeEachTest()
    {
        Console.WriteLine( $"{this.TestContext?.FullyQualifiedTestClassName}: {DateTime.Now} {TimeZoneInfo.Local}" );
        Console.WriteLine( $"Testing {typeof( Gac.Vendor ).Assembly.FullName}" );
    }

    /// <summary> Cleans up the test class instance after each test has run. </summary>	   
    [TestCleanup()]
    public virtual void CleanupAfterEachTest()
    {
        Trace.Listeners.Clear();
    }

    /// <summary>
    /// Gets or sets the test context which provides information about and functionality for the
    /// current test run.
    /// </summary>
    /// <value> The test context. </value>
    public TestContext? TestContext { get; set; }

    #endregion

    /// <summary>   Gets or sets the name of the resource. </summary>
    /// <value> The name of the resource. </value>
    protected string? ResourceName { get; set; }

    /// <summary>   Gets or sets the 'enabled service request' command. </summary>
    /// <value> The 'enabled service request' command. </value>
    protected string? EnabledServiceRequestCommand { get; set; }

    /// <summary>   Gets or sets the enable operation completion event. </summary>
    /// <value> The enable operation completion event. </value>
    protected string? EnableOperationCompletionEvent { get; set; }

    /// <summary>   Gets or sets the 'operation completion query' command. </summary>
    /// <value> The 'operation completion query' command. </value>
    protected string? OperationCompletionQueryCommand { get; set; }

    /// <summary>   Gets or sets a message describing the expected service request. </summary>
    /// <value> A message describing the expected service request. </value>
    protected string? ExpectedServiceRequestMessage { get; set; }

    /// <summary>   Gets or sets the number of service request test repeats. </summary>
    /// <value> The number of service request test repeats. </value>
    protected int ServiceRequestTestRepeatCount { get; set; }

    /// <summary>   Gets or sets the session open timeout. </summary>
    /// <value> The session open timeout. </value>
    protected TimeSpan SessionOpenTimeout { get; set; } = TimeSpan.FromMilliseconds( 3000 );

    /// <summary>   Gets or sets the communication timeout. </summary>
    /// <value> The communication timeout. </value>
    protected TimeSpan CommunicationTimeout { get; set; } = TimeSpan.FromMilliseconds( 3000 );

    /// <summary>   Gets or sets the read after write delay. </summary>
    /// <value> The read after write delay. </value>
    protected TimeSpan ReadAfterWriteDelay { get; set; } = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   (Unit Test Method) assert that resource should exist. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    [TestMethod( "01. Resource Should Exist" )]
    public void ResourceShouldExist()
    {
        if ( this.ResourceName == null ) return;
        string pattern = "(TCPIP|GPIB|USB)?*INSTR";
        IEnumerable<string> resources = Ivi.Visa.GlobalResourceManager.Find( pattern );
        Assert.IsTrue( resources.Any(), $"Resource manager  Failed to find resources matching {pattern}." );

        List<string> values = new( resources );
        Assert.IsTrue( values.Any( s => string.Equals( s, this.ResourceName, StringComparison.OrdinalIgnoreCase ) ),
            $"VISA Resource names should include the {this.ResourceName} resource name" );
    }

    /// <summary>   (Unit Test Method) queries if a given session should open. </summary>
    /// <remarks>   David, 2021-11-05. </remarks>
    [TestMethod( "02. Session Should Open" )]
    public void SessionShouldOpen()
    {
        if ( this.ResourceName == null ) return;
        Ivi.Visa.IMessageBasedSession? session = null;
        try
        {
            session = Asserts.AssertMessageBasedSessionShouldOpen( this.ResourceName, this.SessionOpenTimeout, this.CommunicationTimeout );
            session.Clear();
        }
        catch
        {
            throw;
        }
        finally
        {
            session?.Dispose();
        }
    }

    /// <summary>   (Unit Test Method) session should handle service requests. </summary>
    /// <remarks>   David, 2021-11-06. </remarks>
    [TestMethod( "03. Session Should Handle Service Requests" )]
    [Ignore( "Awaits fixing of service request on the Virtual Machine" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Design", "MSTEST0015:Test method should not be ignored", Justification = "<Pending>" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    public void SessionShouldHandleServiceRequests()
    {
        if ( this.ResourceName == null ) return;
        Ivi.Visa.IMessageBasedSession? session = null;
        try
        {
            session = Asserts.AssertMessageBasedSessionShouldOpen( this.ResourceName, this.SessionOpenTimeout, this.CommunicationTimeout );
            session.Clear();
            Asserts.AssertSessionShouldWaitForServiceRequests( session, this.EnabledServiceRequestCommand!,
                                                                                      this.EnableOperationCompletionEvent!,
                                                                                      this.OperationCompletionQueryCommand!,
                                                                                      this.ExpectedServiceRequestMessage!,
                                                                                      this.ServiceRequestTestRepeatCount );
        }
        catch
        {
            throw;
        }
        finally
        {
            session?.Dispose();
        }
    }

    /// <summary>   (Unit Test Method) Assert that the session should write and read asynchronously. </summary>
    /// <remarks>   2024-08-30. </remarks>
    [TestMethod( "04. Session Should Write Read Asynchronously" )]
    public void SessionShouldWriteReadAsync()
    {
        if ( this.ResourceName == null ) return;
        Ivi.Visa.IMessageBasedSession? session = null;
        try
        {
            session = Asserts.AssertMessageBasedSessionShouldOpen( this.ResourceName, this.SessionOpenTimeout, this.CommunicationTimeout );
            session.Clear();
            this.AssertShouldWriteAndReadAsync( session );
        }
        catch
        {
            throw;
        }
        finally
        {
            session?.Dispose();
        }
    }

    /// <summary>   Virtual method for implementing instrument specific syntax tests. </summary>
    /// <remarks>   2024-09-25. </remarks>
    /// <param name="session">  The session. </param>
    protected virtual void AssertSyntaxTestsShouldPass( Ivi.Visa.IMessageBasedSession? session )
    {
    }

    /// <summary>   (Unit Test Method) syntax tests should pass. </summary>
    /// <remarks>   2024-09-25. </remarks>
    [TestMethod( "05. Syntax Tests should pass" )]
    public void SyntaxTestsShouldPass()
    {
        if ( this.ResourceName == null ) return;
        Ivi.Visa.IMessageBasedSession? session = null;
        try
        {
            session = Asserts.AssertMessageBasedSessionShouldOpen( this.ResourceName, this.SessionOpenTimeout, this.CommunicationTimeout );
            session.Clear();
            this.AssertSyntaxTestsShouldPass( session );
        }
        catch
        {
            throw;
        }
        finally
        {
            session?.Dispose();
        }
    }

    #region " async read write support methods "

    /// <summary>   Gets or sets the message to write. </summary>
    /// <value> The message to write. </value>
    protected string? MessageToWrite { get; set; }

    /// <summary>   The visa asynchronous result. </summary>
    private Ivi.Visa.IVisaAsyncResult? _visaAsyncResult;

    /// <summary>   Length of the actual write message. </summary>
    private long _actualWriteMessageLength;

    /// <summary>   The received message. </summary>
    private string? _receivedMessage;

    /// <summary>   Length of the expected received message. </summary>
    private long _expectedReceivedMessageLength;

    /// <summary>   Length of the actual received message. </summary>
    private long _actualReceivedMessageLength;

    /// <summary>   Gets or sets the expected maximum write time. </summary>
    /// <value> The expected maximum write time. </value>
    protected TimeSpan ExpectedMaximumWriteTime { get; set; }

    /// <summary>   Gets or sets the expected maximum read time. </summary>
    /// <value> The expected maximum read time. </value>
    protected TimeSpan ExpectedMaximumReadTime { get; set; }

    /// <summary>   Clears the read state. </summary>
    /// <remarks>   David, 2021-11-17. </remarks>
    private void ClearReadState()
    {
        this._receivedMessage = string.Empty;
        this._actualReceivedMessageLength = 0;
        this._operationCompletionException = null;
    }

    /// <summary>   Clears the write state. </summary>
    /// <remarks>   David, 2021-11-17. </remarks>
    private void ClearWriteState()
    {
        this.ClearReadState();
        this._actualWriteMessageLength = 0;
    }

    private Ivi.Visa.IMessageBasedSession? _messageBasedSession;

    private Exception? _operationCompletionException;

    private void OnWriteComplete( Ivi.Visa.IVisaAsyncResult result )
    {
        try
        {
            this.ClearWriteState();
            _ = this._messageBasedSession?.RawIO.EndWrite( result );
        }
        catch ( Exception exp )
        {
            this._operationCompletionException = exp;
        }
        this._actualWriteMessageLength = result.Count;
    }

    /// <summary>   Query if this object is write completed. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    /// <returns>   True if write completed, false if not. </returns>
    private bool IsWriteCompleted()
    {
        return this._actualWriteMessageLength > 0;
    }

    private void AssertShouldWriteAndReadAsync( Ivi.Visa.IMessageBasedSession messageBasedSession )
    {
        this._messageBasedSession = messageBasedSession;
        this.ClearWriteState();
        string? textToWrite = string.Empty;
        if ( this.MessageToWrite is not null )
            textToWrite = Asserts.ReplaceCommonEscapeSequences( this.MessageToWrite! );

        if ( textToWrite is null ) return;

        int expectedWriteMessageLength = textToWrite.Length;

        // use sync query to get the read message.
        Console.Out.WriteLine( $"getting synchronous reply to {this.MessageToWrite}" );
        messageBasedSession.RawIO.Write( textToWrite );
        string receivedMessage = messageBasedSession.RawIO.ReadString();
        Assert.IsFalse( string.IsNullOrWhiteSpace( receivedMessage ), $"Synchronous query of {this.MessageToWrite}" );
        this._expectedReceivedMessageLength = receivedMessage.Length;
        Console.Out.WriteLine( $"writing {this.MessageToWrite}" );

        this._visaAsyncResult = messageBasedSession.RawIO.BeginWrite( textToWrite, new Ivi.Visa.VisaAsyncCallback( this.OnWriteComplete ),
               textToWrite.Length );

        (bool completed, TimeSpan elapsed) = Asserts.AsyncAwaitUntil( this.ExpectedMaximumWriteTime, TimeSpan.FromMilliseconds( 4 ),
                                     TimeSpan.FromMilliseconds( 1 ), this.IsWriteCompleted, null );
        if ( !completed )
            this.AbortAsyncOperation( messageBasedSession );

        if ( this._operationCompletionException is not null ) throw this._operationCompletionException;

        Assert.IsTrue( completed,
                $"Async write operation should return expected write count {expectedWriteMessageLength} before timing out after {elapsed}ms" );

        Assert.AreEqual( expectedWriteMessageLength, this._actualWriteMessageLength, $"Actual write count should equal expected value" );

        this.AssertBeginRead( messageBasedSession );
    }

    /// <summary>   Query if this object is read completed. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    /// <returns>   True if read completed, false if not. </returns>
    private bool IsReadCompleted()
    {
        return this._actualReceivedMessageLength > 0;
    }

    /// <summary>   Executes the 'read complete' action. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    /// <param name="result">   The result. </param>
    private void OnReadComplete( Ivi.Visa.IVisaAsyncResult result )
    {
        try
        {
            this.ClearReadState();
            string? responseString = this._messageBasedSession?.RawIO.EndReadString( result );
            if ( responseString is not null )
                this._receivedMessage = Asserts.InsertCommonEscapeSequences( responseString );
        }
        catch ( Exception exp )
        {
            this._operationCompletionException = exp;
        }
        this._actualReceivedMessageLength = result.Count;
    }

    /// <summary>   Assert begin read. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    /// <exception cref="_OperationCompletionException">  Thrown when an Operation Completion error
    ///                                                 condition occurs. </exception>
    /// <param name="messageBasedSession">  The message based session. </param>
    private void AssertBeginRead( Ivi.Visa.IMessageBasedSession messageBasedSession )
    {
        this._messageBasedSession = messageBasedSession;
        this._visaAsyncResult = messageBasedSession.RawIO.BeginRead(
            1024,
            new Ivi.Visa.VisaAsyncCallback( this.OnReadComplete ),
            null );

        (bool completed, TimeSpan elapsed) = Asserts.AsyncAwaitUntil( this.ExpectedMaximumWriteTime, TimeSpan.FromMilliseconds( 4 ),
                                     TimeSpan.FromMilliseconds( 1 ), this.IsReadCompleted, null );

        if ( completed ) this.AbortAsyncOperation( messageBasedSession );

        if ( this._operationCompletionException is not null ) throw this._operationCompletionException;

        Console.Out.WriteLine( $"{this._receivedMessage} was read" );

        Assert.IsTrue( completed,
                 $"Async read operation should return expected read count {this._expectedReceivedMessageLength} before timing out after {elapsed}ms" );

        Assert.AreEqual( this._expectedReceivedMessageLength, this._actualReceivedMessageLength, $"Actual read count should equal expected value" );
    }

    /// <summary>   Abort asynchronous operation. </summary>
    /// <remarks>   David, 2021-11-13. </remarks>
    /// <param name="messageBasedSession">  The message based session. </param>
    private void AbortAsyncOperation( Ivi.Visa.IMessageBasedSession messageBasedSession )
    {
        try
        {
            messageBasedSession.RawIO.AbortAsyncOperation( this._visaAsyncResult );
        }
        catch ( Exception ex )
        {
            this._operationCompletionException = ex;
        }
    }

    #endregion
}
