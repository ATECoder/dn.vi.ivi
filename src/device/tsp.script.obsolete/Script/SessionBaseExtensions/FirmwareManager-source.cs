using cc.isr.VI.Pith;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>
    /// Clears the <paramref scriptName="scriptName">specified</paramref> script source.
    /// </summary>
    /// <remarks>   2024-07-09. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    public static void ClearScriptSource( this Pith.SessionBase? session, string? scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        _ = session.WriteLine( $"{scriptName}.source = nil " );
    }

    /// <summary>
    /// Fetches the <paramref scriptName="scriptName">specified</paramref> script source.
    /// </summary>
    /// <remarks>
    /// Requires setting the correct bits for message available bits. No longer trimming spaces as
    /// this caused failure to load script to the 2602.
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="DeviceException">          Thrown when a Device error condition occurs. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <param name="fetchDelay">   The time to wait between sending the fetch command and reading
    ///                             the data. </param>
    /// <param name="readLineTimeout">  The read timeout. </param>
    /// <returns>   The script source. </returns>
    public static string FetchScriptSource( this Pith.SessionBase? session, string? scriptName, TimeSpan fetchDelay, TimeSpan readLineTimeout )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        string command = $"isr.script.list({scriptName})";
        session.SetLastAction( "fetching script source" );
        session.SetLastActionDetails( $"sending {command}" );
        _ = session.WriteLine( command );

        // todo: replace with waiting for message available.
        _ = SessionBase.AsyncDelay( fetchDelay );
        string fetchedSource = session.ReadLines( session.StatusReadDelay, readLineTimeout, true );

        _ = SessionBase.AsyncDelay( session.StatusReadDelay + session.StatusReadDelay );
        ServiceRequests statusByte = session.ReadStatusByte();
        session.ThrowDeviceExceptionIfError( statusByte );

        if ( session.IsMessageAvailableBitSet( statusByte ) )
            Debug.Assert( !Debugger.IsAttached, "Buffer Not empty" );

        return fetchedSource;
    }

    /// <summary>
    /// Fetches the <paramref scriptName="scriptName">specified</paramref> script source.
    /// </summary>
    /// <remarks>   Requires setting the proper message available bits for the session. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="DeviceException">          Thrown when a Device error condition occurs. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="nodeNumber">       Specifies the subsystem node. </param>
    /// <param name="scriptName">       Specifies the script name. </param>
    /// <param name="fetchDelay">       The time to wait between sending the fetch command and
    ///                                 reading the data. </param>
    /// <param name="readLineTimeout">  The read timeout. </param>
    /// <returns>   The script source. </returns>
    public static string FetchScriptSource( this Pith.SessionBase? session, int nodeNumber, string? scriptName, TimeSpan fetchDelay, TimeSpan readLineTimeout )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        // clear data queue and report if not empty.
        session.SetLastAction( "clearing data queue" );

        LinkSubsystemBase.ClearDataQueue( session, nodeNumber );

        session.ThrowDeviceExceptionIfError();

        string command = $"isr.script.list({scriptName},{nodeNumber})";
        session.SetLastAction( "listing script source" );
        session.SetLastActionDetails( $"sending {command}" );

        _ = session.WriteLine( command );

        // todo: replace with waiting for message available.
        _ = SessionBase.AsyncDelay( fetchDelay );

        string fetchedSource = session.ReadLines( session.StatusReadDelay, readLineTimeout, true );

        _ = SessionBase.AsyncDelay( session.StatusReadDelay + session.StatusReadDelay );
        ServiceRequests statusByte = session.ReadStatusByte();
        session.ThrowDeviceExceptionIfError( statusByte );

        if ( session.IsMessageAvailableBitSet( statusByte ) )
            Debug.Assert( !Debugger.IsAttached, "Buffer Not empty" );

        return fetchedSource;
    }

    /// <summary>   Fetches the script source line by line. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="script">           Specifies the script. </param>
    /// <returns>   The script source. </returns>
    public static string FetchScriptSource( this Pith.SessionBase? session, ScriptEntityBase script )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        if ( script.Node is null ) throw new ArgumentNullException( nameof( script.Node ) );
        if ( script.Node.IsController )
            return FetchScriptSource( session, script.Name, TimeSpan.FromMilliseconds( 500 ), TimeSpan.FromMilliseconds( 400 ) );
        else
            return FetchScriptSource( session, script.Node.Number, script.Name, TimeSpan.FromMilliseconds( 500 ), TimeSpan.FromMilliseconds( 400 ) );
    }

    /// <summary>   Reads the entire source of the script <paramref scriptName="scriptName"/>. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <param name="trimSpaces">   Specifies a directive to trim leading and trailing spaces from
    ///                             each line. </param>
    /// <param name="fetchDelay">   The time to wait between sending the fetch command and reading
    ///                             the data. </param>
    /// <returns>   The script source. </returns>
    public static string FetchScriptSource( this Pith.SessionBase? session, string scriptName, bool trimSpaces, TimeSpan fetchDelay )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        _ = session.WriteLine( "print({0}.source)", scriptName );

        // todo: replace with waiting for message available.
        _ = SessionBase.AsyncDelay( fetchDelay );

        return session.ReadLines( TimeSpan.FromMilliseconds( 10d ), TimeSpan.FromMilliseconds( 400d ), trimSpaces, !trimSpaces );
    }

    /// <summary>   Check if a user script was saved as a byte code script. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Gets or sets the script name. </param>
    /// <returns>   <c>true</c> if binary; otherwise, <c>false</c>. </returns>
    public static bool IsScriptSavedAsByteCode( this Pith.SessionBase? session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.MakeTrueFalseReplyIfEmpty( true );
        return session.IsStatementTrue( "string.sub({0}.source,1,24) == 'loadstring(table.concat('", scriptName );
    }
}
