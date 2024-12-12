using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.SessionBaseExtensions;

/// <summary>   A session base methods. </summary>
/// <remarks>   2024-09-11. </remarks>
[CLSCompliant( false )]
public static partial class SessionBaseMethods
{
    /// <summary>   Loads a function. </summary>
    /// <remarks>   2024-08-22. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="functionName"> Specifies the function name. </param>
    /// <param name="functionCode"> The function code. </param>
    /// <returns>   True if it the function was loaded and its name exists; otherwise, false. </returns>
    public static bool LoadFunction( this Pith.SessionBase session, string functionName, string functionCode )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( string.IsNullOrWhiteSpace( functionName ) ) throw new ArgumentNullException( nameof( functionName ) );
        if ( string.IsNullOrWhiteSpace( functionCode ) ) throw new ArgumentNullException( nameof( functionCode ) );

        // it seems that the 2602 does not handle the end of line characters:
        functionCode = functionCode.Replace( '\r', ' ' );
        functionCode = functionCode.Replace( '\n', ' ' );

        // load the function
        _ = session.WriteLine( functionCode );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

        // return true if function name exists.
        return !session.IsNil( functionName );
    }

    /// <summary> Calls the function with the given arguments in protected mode. </summary>
    /// <remarks>
    /// Protected mode means that any error inside the function is not propagated; instead, the call
    /// (Lua pcall) catches the error and returns a status code. Its first result is the status code
    /// (a Boolean), which is <c>true</c> if the call succeeds without errors. In such case, pcall
    /// also returns all results from the call, after this first result. In case of any error, pcall
    /// returns false plus the error message.
    /// </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="functionName"> Specifies the function name. </param>
    /// <param name="args">         Specifies the function arguments. </param>
    public static void CallFunction( this Pith.SessionBase session, string functionName, string args )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( string.IsNullOrWhiteSpace( functionName ) ) throw new ArgumentNullException( nameof( functionName ) );
        string callStatement = Syntax.Tsp.Lua.CallFunctionCommand( functionName, args );
        callStatement = Syntax.Tsp.Lua.PrintCommand( callStatement );
        _ = session.WriteLine( callStatement );
    }
}
