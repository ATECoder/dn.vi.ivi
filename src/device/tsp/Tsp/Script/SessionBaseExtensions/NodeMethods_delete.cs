using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method makes a script nil. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="nodeNumber">           Specifies the remote node number. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    public static void NillScript( this Pith.SessionBase? session, int nodeNumber, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = nodeNumber;
        session.SetLastAction( $"enabling service request on operation completion" );

        session.SetLastAction( $"checking if the script exists on node {nodeNumber}" );
        if ( !session.IsNil( nodeNumber, scriptName ) )
        {
            // TODO: Should we wait complete on the node as well as the controller.
            // possibly, waiting on the controller waits for all nodes.
            session.EnableServiceRequestOnOperationCompletion();

            session.TraceLastAction( $"nulling script '{scriptName}' on node {nodeNumber}" );
            _ = session.ExecuteCommandQueryComplete( nodeNumber, "{0} = nil", scriptName );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();
        }

        if ( !session.IsNil( nodeNumber, scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} was not nullified on node {nodeNumber}." );
    }
}
