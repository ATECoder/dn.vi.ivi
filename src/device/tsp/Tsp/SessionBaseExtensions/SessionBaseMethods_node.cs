using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.SessionBaseExtensions;

public static partial class SessionBaseMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/>  extension method that queries controller node number. </summary>
    /// <remarks>   2025-04-26. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session to act on. </param>
    /// <returns>   An Integer or Null of failed. </returns>
    public static int QueryControllerNodeNumber( this Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        session.TraceLastAction( "fetching controller node number." );
        int controllerNodeNumber = session.QueryPrint( 0, 1, cc.isr.VI.Syntax.Tsp.Node.ControllerNodeNumber );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // throw if device error occurred
        session.ThrowDeviceExceptionIfError();

        return controllerNodeNumber;
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/>  extension method that query if 'session' is controller node.
    /// </summary>
    /// <remarks>   2025-04-26. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <returns>   True if controller node, false if not. </returns>
    public static bool IsControllerNode( this Pith.SessionBase session, int nodeNumber )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        return nodeNumber == 0 || nodeNumber == session.QueryControllerNodeNumber();
    }

}
