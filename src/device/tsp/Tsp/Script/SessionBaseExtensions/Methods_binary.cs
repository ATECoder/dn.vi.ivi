using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   Query if 'source' is binary source. </summary>
    /// <remarks>   2024-10-11. </remarks>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <returns>   True if binary source, false if not. </returns>
    public static bool IsBinarySource( this string source )
    {
        return source.Contains( @"\27LuaP\0\4\4\4\", StringComparison.Ordinal );
    }

    /// <summary>   Checks if the script is Binary. </summary>
    /// <remarks>   2024-09-24. </remarks>
    /// <param name="session">              The session. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <param name="usingSupportScript">   (Optional) [false] True to using support script. </param>
    /// <returns>   <c>true</c> if the script is a binary script; otherwise, <c>false</c>. </returns>
    public static bool IsBinaryScript( this Pith.SessionBase session, string scriptName, bool usingSupportScript = false )
    {
        return usingSupportScript
            ? session.IsStatementTrue( "_G.isr.script.isBinary({0})", scriptName )
            : !session.IsNil( $"_G.string.find( _G.string.sub( {scriptName}.source , 1 , 50 ), 'loadstring(table.concat(' , 1 , true )" );
    }

    /// <summary>   Checks if the script is Binary. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="nodeNumber">           The node number. </param>
    /// <param name="script">               The script. </param>
    /// <param name="usingSupportScript">   (Optional) [false] True to using support script. </param>
    /// <returns>   <c>true</c> if the script is a binary script; otherwise, <c>false</c>. </returns>
    public static bool IsBinaryScript( this Pith.SessionBase session, int nodeNumber, ScriptInfo script, bool usingSupportScript = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        return session.IsControllerNode( nodeNumber )
                ? session.IsBinaryScript( script.Title, usingSupportScript )
                : session.IsStatementTrue( "_G.isr.script.isBinary({0},{1})", script.Title, nodeNumber );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method to convert to binary.
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session to act on. </param>
    /// <param name="scriptName">           Name of the script. </param>
    public static void ConvertToBinary( this SessionBase session, string scriptName )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );

        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be converted to binary because it was not found." );

        session.SetLastAction( "converting the script to binary;. " );
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
