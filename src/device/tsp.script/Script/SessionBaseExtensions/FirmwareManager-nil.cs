namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>   [local method] Nill script. </summary>
    /// <remarks>   2024-09-20. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="InvalidCastException">     Thrown when an object cannot be cast to a
    ///                                             required type. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    private static bool NillScriptThis( this Pith.SessionBase? session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;

        session.SetLastAction( $"nulling script '{scriptName}" );
        _ = session.WriteLine( $"{scriptName} = nil {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );

        // read query reply and throw if reply is not 1.
        session.ReadAndThrowIfOperationIncomplete();

        // throw if device errors
        session.ThrowDeviceExceptionIfError();

        return session.IsNil( scriptName );
    }

    /// <summary>   Makes a script nil. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <returns>   <c>true</c> if the script is nil; otherwise <c>false</c>. </returns>
    public static bool NillScript( this Pith.SessionBase? session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        return session.IsNil( scriptName ) || session.NillScriptThis( scriptName );
    }

    /// <summary>   [local method] Nill script. </summary>
    /// <remarks>   2024-09-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="nodeNumber">           Specifies the remote node number. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    private static bool NillScriptThis( this Pith.SessionBase? session, int nodeNumber, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = nodeNumber;
        session.SetLastAction( $"enabling service request on operation completion" );

        // TODO: Should we wait complete on the node as well as the controller. 
        // possibly, waiting on the controller waits for all nodes.
        session.EnableServiceRequestOnOperationCompletion();

        session.SetLastAction( $"nulling script '{scriptName}" );
        _ = session.ExecuteCommandQueryComplete( nodeNumber, "{0} = nil", scriptName );

        // read query reply and throw if reply is not 1.
        session.ReadAndThrowIfOperationIncomplete();

        // throw if device errors
        session.ThrowDeviceExceptionIfError();

        return session.IsNil( nodeNumber, scriptName );
    }

    /// <summary>   Makes a script nil and returns <c>true</c> if the script was nullified. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="nodeNumber">           Specifies the remote node number. </param>
    /// <param name="scriptName">           Specifies the script name. </param>
    /// <returns>   <c>true</c> if the script is nil; otherwise <c>false</c>. </returns>
    public static bool NillScript( this Pith.SessionBase? session, int nodeNumber, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        return !string.IsNullOrWhiteSpace( scriptName ) && (session.IsNil( nodeNumber, scriptName )
            || session.NillScriptThis( nodeNumber, scriptName ));
    }
}
