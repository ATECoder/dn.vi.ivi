using cc.isr.VI.Foundation;

namespace cc.isr.VI.Tsp.SessionBaseExtensions;

/// <summary>   A session base exception methods. </summary>
/// <remarks>   2025-05-10. </remarks>
public static partial class SessionBaseMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that nulls an object and returns control
    /// after awaiting operation completion reply.
    /// </summary>
    /// <remarks>   2025-05-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="objectName">   Name of the object. </param>
    public static void NillObject( this Pith.SessionBase session, string objectName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( Session ) );
        if ( objectName is null || string.IsNullOrWhiteSpace( objectName ) ) throw new ArgumentNullException( nameof( objectName ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"The {nameof( session )} {session.ResourceNameCaption} is not open." );

        // if the object exists, nil it.
        session.TraceLastAction( $"\r\n\tchecking if the '{objectName}' exists;. " );
        session.LastNodeNumber = default;
        if ( !session.IsNil( objectName ) )
        {
            // nil the Script
            session.TraceLastAction( $"\r\n\tNulling {objectName};. " );
            _ = session.WriteLine( $"{objectName} = nil {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();

            if ( session.IsNil( objectName ) )
                if ( !session.CollectGarbageQueryComplete() )
                    _ = session.TraceWarning( message: "garbage collection incomplete (e.g., reading .ne. '1');. " );
                else
                    throw new InvalidOperationException( $"The {objectName} was not nullified;. " );
        }
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that nulls an object on the specified node. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   Specifies the remote node number. </param>
    /// <param name="objectName">   Specifies the object name. </param>
    public static void NillObject( this Pith.SessionBase session, int nodeNumber, string objectName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( objectName is null || string.IsNullOrWhiteSpace( objectName ) ) throw new ArgumentNullException( nameof( objectName ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"The {nameof( session )} {session.ResourceNameCaption} is not open." );

        session.LastNodeNumber = nodeNumber;

        session.TraceLastAction( $"\r\n\tchecking if the '{objectName}' exists on node {nodeNumber};. " );
        if ( !session.IsNil( nodeNumber, objectName ) )
        {
            // TODO: Should we wait complete on the node as well as the controller.
            // possibly, waiting on the controller waits for all nodes.
            session.EnableServiceRequestOnOperationCompletion();

            session.TraceLastAction( $"\r\n\tnulling '{objectName}' on node {nodeNumber};. " );
            _ = session.ExecuteCommandQueryComplete( nodeNumber, "{0} = nil", objectName );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();

            if ( session.IsNil( nodeNumber, objectName ) )
                if ( !session.CollectGarbageQueryComplete( nodeNumber ) )
                    _ = session.TraceWarning( message: $"garbage collection incomplete (e.g., reading .ne. '1') on node {nodeNumber};. " );
                else
                    throw new InvalidOperationException( $"The {objectName} was not nullified on node {nodeNumber};. " );
        }
    }
}
