using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class Methods
{
    /// <summary>   A <see cref="SessionBase"/> extension method that deletes the script. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Name of the script. </param>
    public static void DeleteScript( this SessionBase session, string scriptName )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) )
            throw new ArgumentNullException( nameof( scriptName ) );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            Methods.TraceLastAction( $"Script {scriptName} not found for deletion." );
        else
        {
            Methods.TraceLastAction( $"deleting the {scriptName} script;. " );
            _ = session.WriteLine( $"script.delete({scriptName})" );
            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

            session.SetLastAction( "checking if the script was deleted;. " );
            if ( session.IsNil( scriptName ) )
                Methods.TraceLastAction( "script was deleted;. " );
            else
                throw new InvalidOperationException( $"Deletion of {nameof( scriptName )} failed. The script object value is not nil.;. " );
        }
    }
}
