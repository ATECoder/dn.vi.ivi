using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   Makes a script nil. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <returns>   <c>true</c> if the script is nil; otherwise <c>false</c>. </returns>
    public static void NillScript( this Pith.SessionBase? session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( !session.IsNil( scriptName ) )
        {
            session.TraceLastAction( $"nulling the '{scriptName}' script;. " );
            _ = session.WriteLine( $"{scriptName} = nil {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();
        }

        if ( !session.IsNil( $"{scriptName}" ) )
            throw new InvalidOperationException( $"The script {scriptName} was not nullified." );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that removes the script from the list of user scripts. </summary>
    /// <remarks>   2025-04-11. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static void RemoveUserScript( this Pith.SessionBase? session, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;
        session.SetLastAction( $"checking if the {scriptName} script is listed as user script;. " );
        if ( !session.IsNil( $"script.user.scripts.{scriptName}" ) )
        {
            session.TraceLastAction( $"removing '{scriptName} script from the user scripts." );
            _ = session.WriteLine( $"script.user.scripts.{scriptName}.name = '' {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();
        }

        if ( !session.IsNil( $"script.user.scripts.{scriptName}" ) )
            throw new InvalidOperationException( $"The script {scriptName} was not removed from the user scripts." );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that deletes the script. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptName">       Name of the script. </param>
    public static void DeleteScript( this SessionBase session, string scriptName )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        // removes the saved script from the catalog of saved scripts. 
        session.RemoveSavedScript( scriptName );

        // remove the script from the list of user scripts.
        session.RemoveUserScript( scriptName );

        // nill the script if is is not nil.
        session.NillScript( scriptName );
    }
}
