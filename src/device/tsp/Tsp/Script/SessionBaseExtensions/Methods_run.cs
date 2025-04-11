using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class Methods
{
    /// <summary>   A <see cref="SessionBase"/> extension method that executes the 'script' operation. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The reference to the K2600 Device. </param>
    /// <param name="scriptName">           (Optional) [empty] Name of the script. Empty if running
    ///                                     an anonymous script. </param>
    /// <param name="scriptElementName">    (Optional) [empty] Name of the script element. </param>
    public static void RunScript( this SessionBase session, string scriptName = "", string scriptElementName = "" )
    {
        if ( session == null )
            throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen )
            throw new InvalidOperationException( $"{nameof( session )} is not open." );

        if ( !string.IsNullOrWhiteSpace( scriptName ) )
        {
            session.SetLastAction( $"checking if the {scriptName} script exists;. " );
            if ( session.IsNil( scriptName ) )
                throw new InvalidOperationException( $"The script {scriptName} cannot be run because it was not found." );

            session.SetLastAction( $"running the {scriptName} script;. " );
            _ = session.WriteLine( $"{scriptName}.run()" );
        }
        else
        {
            session.SetLastAction( $"running the anonymous script;. " );
            _ = session.WriteLine( $"scrip.run()" );
        }
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );

        if ( !string.IsNullOrWhiteSpace( scriptElementName ) )
        {
            session.SetLastAction( $"looking for {scriptElementName};. " );
            if ( session.IsNil( scriptElementName ) )
                throw new InvalidOperationException( $"The script element {scriptElementName} was not found after running the {scriptName} script." );
        }
    }
}
