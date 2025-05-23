namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Firmware;

internal static partial class Asserts
{
    /// <summary>   Assert command should execute. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="command">      The query. </param>
    /// <param name="logEnabled">   (Optional) True to enable, false to disable the log. </param>
    public static void AssertCommandShouldExecute( Pith.SessionBase? session, string command, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        Assert.IsNotNull( command, $"{nameof( command )} must not be null." );
        Assert.IsFalse( string.IsNullOrWhiteSpace( command ), $"{nameof( command )} must not be null or empty or white space." );

        _ = session.WriteLine( command );
        _ = Pith.SessionBase.AsyncDelay( session.ReadAfterWriteDelay, session.StatusReadDelay );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        string expectedValue = "1";
        string query = cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand;
        _ = session.WriteLine( query );
        _ = Pith.SessionBase.AsyncDelay( session.ReadAfterWriteDelay, session.StatusReadDelay );
        string reply = session.ReadLineTrimEnd();
        Assert.AreEqual( expectedValue, reply, $"'{query}' should return the expected reply." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        if ( logEnabled ) Asserts.LogIT( $"{command} executed" );
    }

    /// <summary>   Assert query should execute. </summary>
    /// <remarks>   2024-11-04. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="query">        The query. </param>
    /// <param name="logEnabled">   (Optional) True to enable, false to disable the log. </param>
    /// <returns>   A string. </returns>
    public static string AssertQueryShouldExecute( Pith.SessionBase? session, string query, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        Assert.IsNotNull( query, $"{nameof( query )} must not be null." );
        Assert.IsFalse( string.IsNullOrWhiteSpace( query ), $"{nameof( query )} must not be null or empty or white space." );

        _ = session.WriteLine( query );
        _ = Pith.SessionBase.AsyncDelay( session.ReadAfterWriteDelay, session.StatusReadDelay );

        string reply = session.ReadLineTrimEnd();
        Assert.IsFalse( string.IsNullOrWhiteSpace( reply ), $"{nameof( reply )} must not be null or empty or white space." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        if ( logEnabled ) Asserts.LogIT( $"{query} returned {reply}" );

        return reply;
    }

    /// <summary>   Assert query reply should be valid. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="query">            The query. </param>
    /// <param name="expectedValue">    The expected value. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertQueryReplyShouldBeValid( Pith.SessionBase? session, string query, string expectedValue, bool logEnabled = false )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"{session.CandidateResourceName} should be open" );
        Assert.IsNotNull( query, $"{nameof( query )} must not be null." );
        Assert.IsFalse( string.IsNullOrWhiteSpace( query ), $"{nameof( query )} must not be null or empty or white space." );

        _ = session.WriteLine( query );
        _ = Pith.SessionBase.AsyncDelay( session.ReadAfterWriteDelay, session.StatusReadDelay );

        string reply = session.ReadLineTrimEnd();
        Assert.AreEqual( expectedValue, reply, $"'{query}' should return the expected reply." );

        VI.Device.Tests.Asserts.AssertOrphanMessages( session );
        VI.Device.Tests.Asserts.ThrowIfDeviceErrors( session, $"Device error occurred after 'VI.Device.Tests.Asserts.AssertOrphanMessages()'" );

        if ( logEnabled ) Asserts.LogIT( $"{query} returned {reply}" );
    }

    /// <summary>   Assert setter query reply should be valid. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="command">          The query. </param>
    /// <param name="query">            The query. </param>
    /// <param name="expectedValue">    The expected value. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertSetterQueryReplyShouldBeValid( Pith.SessionBase? session, string command, string query, string expectedValue, bool logEnabled = false )
    {
        Asserts.AssertCommandShouldExecute( session, command, logEnabled );
        string reply = Asserts.AssertQueryShouldExecute( session, query, logEnabled );
        Assert.AreEqual( expectedValue, reply, $"'{query}' should return the expected reply after {command}." );
    }

    /// <summary>   Assert query reply should be valid. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="query">            The query. </param>
    /// <param name="expectedValue">    The expected value. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertQueryReplyShouldBeValid( Pith.SessionBase? session, string query, bool expectedValue, bool logEnabled = false )
    {
        string reply = Asserts.AssertQueryShouldExecute( session, query, logEnabled );
        Assert.IsTrue( bool.TryParse( reply, out bool actualValue ), $"Failed parsing value '{reply}' to Boolean from command '{query}'" );
        Assert.AreEqual( expectedValue, actualValue, $"'{query}' should return the expected reply." );
    }

    /// <summary>   Assert setter query reply should be valid. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="command">          The query. </param>
    /// <param name="query">            The query. </param>
    /// <param name="expectedValue">    The expected value. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertSetterQueryReplyShouldBeValid( Pith.SessionBase? session, string command, string query, bool expectedValue, bool logEnabled = false )
    {
        Asserts.AssertCommandShouldExecute( session, command, logEnabled );
        string reply = Asserts.AssertQueryShouldExecute( session, query, logEnabled );
        Assert.IsTrue( bool.TryParse( reply, out bool actualValue ), $"Failed parsing value '{reply}' to Boolean from command '{query}'" );
        Assert.AreEqual( expectedValue, actualValue, $"'{query}' should return the expected reply after {command}." );
    }

    /// <summary>   Assert query reply should be valid. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="query">            The query. </param>
    /// <param name="expectedValue">    The expected value. </param>
    /// <param name="delta">            The delta. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertQueryReplyShouldBeValid( Pith.SessionBase? session, string query, double expectedValue, double delta, bool logEnabled = false )
    {
        string reply = Asserts.AssertQueryShouldExecute( session, query, logEnabled );
        Assert.IsTrue( double.TryParse( reply, out double actualValue ), $"Failed parsing value '{reply}' to double from command '{query}'" );
        Assert.AreEqual( expectedValue, actualValue, delta, $"'{query}' should return the expected reply." );
    }

    /// <summary>   Assert setter query reply should be valid. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="command">          The query. </param>
    /// <param name="query">            The query. </param>
    /// <param name="expectedValue">    The expected value. </param>
    /// <param name="delta">            The delta. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertSetterQueryReplyShouldBeValid( Pith.SessionBase? session, string command, string query, double expectedValue, double delta, bool logEnabled = false )
    {
        Asserts.AssertCommandShouldExecute( session, command, logEnabled );
        string reply = Asserts.AssertQueryShouldExecute( session, query, logEnabled );
        Assert.IsTrue( double.TryParse( reply, out double actualValue ), $"Failed parsing value '{reply}' to double from command '{query}'" );
        Assert.AreEqual( expectedValue, actualValue, delta, $"'{query}' should return the expected reply after {command}." );
    }

    /// <summary>   Assert query reply should be valid. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="query">            The query. </param>
    /// <param name="expectedValue">    The expected value. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertQueryReplyShouldBeValid( Pith.SessionBase? session, string query, int expectedValue, bool logEnabled = false )
    {
        string reply = Asserts.AssertQueryShouldExecute( session, query, logEnabled );
        Assert.IsTrue( double.TryParse( reply, out double actualValue ), $"Failed parsing value '{reply}' to double from command '{query}'" );
        Assert.AreEqual( expectedValue, ( int ) actualValue, $"'{query}' should return the expected reply." );
    }

    /// <summary>   Assert setter query reply should be valid. </summary>
    /// <remarks>   2024-11-03. </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="command">          The query. </param>
    /// <param name="query">            The query. </param>
    /// <param name="expectedValue">    The expected value. </param>
    /// <param name="logEnabled">       (Optional) True to enable, false to disable the log. </param>
    public static void AssertSetterQueryReplyShouldBeValid( Pith.SessionBase? session, string command, string query, int expectedValue, bool logEnabled = false )
    {
        Asserts.AssertCommandShouldExecute( session, command, logEnabled );
        string reply = Asserts.AssertQueryShouldExecute( session, query, logEnabled );
        Assert.IsTrue( double.TryParse( reply, out double actualValue ), $"Failed parsing value '{reply}' to double from command '{query}'" );
        Assert.AreEqual( expectedValue, ( int ) actualValue, $"'{query}' should return the expected reply after {command}." );
    }
}
