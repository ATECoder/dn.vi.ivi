using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class Methods
{
    /// <summary>   Checks if the script is Binary. </summary>
    /// <remarks>   2024-09-24. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   <c>true</c> if the script is a binary script; otherwise, <c>false</c>. </returns>
    public static bool IsBinaryScript( this Pith.SessionBase session, string scriptName )
    {
        return !session.IsNil( $"_G.string.find( _G.string.sub( {scriptName}.source , 1 , 50 ), 'loadstring(table.concat(' , 1 , true )" );
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
