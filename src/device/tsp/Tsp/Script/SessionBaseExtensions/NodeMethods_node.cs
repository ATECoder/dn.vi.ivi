using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class NodeMethods
{
    /// <summary> Reads the Controller node number. </summary>
    /// <returns> An Integer or Null of failed. </returns>
    public static int QueryControllerNodeNumber( this SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        session.TraceLastAction( "fetching controller node number." );
        int controllerNodeNumber = session.QueryPrint( 0, 1, "_G.tsplink.node" );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        return controllerNodeNumber;
    }

}
