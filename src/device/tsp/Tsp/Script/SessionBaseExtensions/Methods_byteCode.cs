using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseMethods
{
    /// <summary>   Query if 'source' is byte code. </summary>
    /// <remarks>   2024-10-11. </remarks>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <returns>   True if byte code script, false if not. </returns>
    public static bool IsByteCodeScript( this string source )
    {
        return source.Contains( @"\27LuaP\0\4\4\4\", StringComparison.Ordinal );
    }

    /// <summary>   Checks if the script is byte code. </summary>
    /// <remarks>   2024-09-24. </remarks>
    /// <param name="session">              The session. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <param name="usingSupportScript">   (Optional) [false] True to using support script. </param>
    /// <returns>   <c>true</c> if the script is a byte code script; otherwise, <c>false</c>. </returns>
    public static bool IsByteCodeScript( this Pith.SessionBase session, string scriptName, bool usingSupportScript = false )
    {
        string commandObject = "_G.isr.script.isByteCode";
        usingSupportScript = usingSupportScript && !session.IsNil( commandObject );
        return usingSupportScript
            ? session.IsStatementTrue( "_G.isr.script.isByteCode({0})", scriptName )
            : !session.IsNil( $"_G.string.find( _G.string.sub( {scriptName}.source , 1 , 50 ), 'loadstring(table.concat(' , 1 , true )" );
    }

    /// <summary>   Checks if the script is byte code. </summary>
    /// <remarks>   2025-06-14. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="nodeNumber">           The node number. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <param name="usingSupportScript">   (Optional) [false] True to using support script. </param>
    /// <returns>   <c>true</c> if the script is a byte code script; otherwise, <c>false</c>. </returns>
    public static bool IsByteCodeScript( this Pith.SessionBase session, int nodeNumber, string scriptName, bool usingSupportScript = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        string commandObject = "_G.isr.script.isByteCode";
        usingSupportScript = usingSupportScript && !session.IsNil( commandObject );
        return session.IsControllerNode( nodeNumber )
                ? session.IsByteCodeScript( scriptName, usingSupportScript )
                : session.IsStatementTrue( "_G.isr.script.isByteCode({0},{1})", scriptName, nodeNumber );
    }

    /// <summary>   Checks if the script is byte code. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="nodeNumber">           The node number. </param>
    /// <param name="script">               The script. </param>
    /// <param name="usingSupportScript">   (Optional) [false] True to using support script. </param>
    /// <returns>   <c>true</c> if the script is a byte code script; otherwise, <c>false</c>. </returns>
    public static bool IsByteCodeScript( this Pith.SessionBase session, int nodeNumber, ScriptInfo script, bool usingSupportScript = false )
    {
        return SessionBaseMethods.IsByteCodeScript( session, nodeNumber, script.Title, usingSupportScript );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method to convert to byte code.
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session to act on. </param>
    /// <param name="scriptName">           Name of the script. </param>
    public static void ConvertToByteCode( this SessionBase session, string scriptName )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );

        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;

        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be converted to byte code because it was not found." );

        if ( session.IsByteCodeScript( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} is already a byte code script." );

        session.SetLastAction( "converting the script to byte code;. " );
        _ = session.WriteLine( $"{scriptName}.source=nil" );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();
    }


}
