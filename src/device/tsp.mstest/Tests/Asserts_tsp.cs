using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Device.Tsp.Tests;

public static partial class Asserts
{
    /// <summary>   Assert that the Lua function should be called. </summary>
    /// <remarks>   2024-08-19. </remarks>
    /// <param name="session">                  The Visa Session. </param>
    /// <param name="functionName">             Name of the function. </param>
    /// <param name="functionCode">             The function code. </param>
    /// <param name="functionArguments">        The function arguments. </param>
    /// <param name="expectedResult">           The expected result. </param>
    /// <param name="callFunctionResultDelay">  The call function result delay. </param>
    public static void AssertFunctionShouldBeCalled( Pith.SessionBase session, string functionName, string functionCode, string functionArguments,
        string expectedResult, TimeSpan callFunctionResultDelay )
    {
        Assert.IsNotNull( session, $"{nameof( session )} is null" );

        Assert.IsTrue( TimeSpan.Zero < callFunctionResultDelay, $"{nameof( callFunctionResultDelay )} must be greater than zero." );

        // assert that the function is null or should be null
        Asserts.AssertFunctionShouldNull( session, functionName );

        // load the function
        bool loaded = session.LoadFunction( functionName, functionCode );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

        // assert that the function name exists
        Assert.IsTrue( loaded, $"Function '{functionName}' should not be null." );

        // call the function with its arguments
        session.CallFunction( functionName, functionArguments );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // fetch the result from the instrument.
        string actualResult = session.ReadLine();

        // read the function call boolean outcome (true or false).
        string booleanOutcome = session.ReadLineTrimEnd();

        // _ = SessionBase.AsyncDelay( cc.isr.VI.Tsp.K2600.Ttmware.MSTest.AllSettings.ScriptSettings.CallFunctionResultDelay );

        string expectedBooleanOutcome = cc.isr.VI.Syntax.Tsp.Lua.TrueValue;
        Assert.AreEqual( expectedBooleanOutcome, booleanOutcome,
            $"the function call returned an incorrect boolean outcome '{booleanOutcome}' instead of '{expectedBooleanOutcome}'" );
        Assert.AreEqual( expectedResult, actualResult );
    }

    /// <summary>   Asserts that the function should be nulled. </summary>
    /// <remarks>   2024-08-20. </remarks>
    /// <param name="session">      The Visa Session. </param>
    /// <param name="functionName"> Name of the function. </param>
    public static void AssertFunctionShouldNull( Pith.SessionBase session, string functionName )
    {
        Assert.IsNotNull( session, $"{nameof( Pith.SessionBase )} is null" );

        // if the function exists, nil it.
        if ( !session.IsNil( functionName ) )
        {
            // nil the function
            session.NillObject( functionName );

            Asserts.AssertGarbageShouldBeCollected( session );

            // assert that the function is now nil.
            Assert.IsTrue( session.IsNil( functionName ), $"Function {functionName} should be {SessionBase.NilValue}" );
        }
    }

    /// <summary>   Assert that garbage should be collected. </summary>
    /// <remarks>   2024-08-20. </remarks>
    /// <param name="session">  The Visa Session. </param>
    public static void AssertGarbageShouldBeCollected( Pith.SessionBase session )
    {
        Assert.IsNotNull( session, $"{nameof( Pith.SessionBase )} is null" );

        Assert.IsTrue( session.IsDeviceOpen, $"The session {session.ResourceNameCaption} should be open." );

        if ( !session.CollectGarbageQueryComplete() )
            _ = session.TraceWarning( message: $"garbage collection incomplete (reply not '1')" );
    }
}
