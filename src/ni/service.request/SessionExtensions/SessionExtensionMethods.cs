using Ivi.Visa;

namespace NI.ServiceRequest.SessionExtensions;

/// <summary>   A session extension methods. </summary>
/// <remarks>   2026-01-29. </remarks>
internal static class SessionExtensionMethods
{
    /// <summary>   Replace common escape sequences. </summary>
    /// <remarks>   2026-01-29. </remarks>
    /// <param name="s">    The string. </param>
    /// <returns>   A string. </returns>
    public static string ReplaceCommonEscapeSequences( this string s )
    {
        return s.Replace( "\\n", "\n" ).Replace( "\\r", "\r" );
    }

    /// <summary>   Inserts a common escape sequences. </summary>
    /// <remarks>   2026-01-29. </remarks>
    /// <param name="s">    The string. </param>
    /// <returns>   A string. </returns>
    public static string InsertCommonEscapeSequences( this string s )
    {
        return s.Replace( "\n", "\\n" ).Replace( "\r", "\\r" );
    }

    /// <summary>   Queries a hexadecimal. </summary>
    /// <remarks>   2026-01-29. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>   The hexadecimal. </returns>
    public static string QueryHex( this IMessageBasedSession? session, string queryCommand )
    {
        ArgumentNullException.ThrowIfNull( session );
        try
        {
            session.WriteLine( queryCommand );
            string? response = session.RawIO.ReadString();
            if ( int.TryParse( response, out int result ) )
                return $"0x{result:X2}";
            else
                throw new FormatException( $"Response '{response}' is not a valid integer." );
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
            return "0X..";
        }
    }

    /// <summary>   Queries an int. </summary>
    /// <remarks>   2026-01-29. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FormatException">          Thrown when the format of an input is incorrect. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="queryCommand"> The query command. </param>
    /// <returns>   The int. </returns>
    public static int QueryInt( this IMessageBasedSession? session, string queryCommand )
    {
        ArgumentNullException.ThrowIfNull( session );
        try
        {
            session.WriteLine( queryCommand );
            string? response = session.RawIO.ReadString();
            if ( int.TryParse( response, out int result ) )
                return result;
            else
                throw new FormatException( $"Response '{response}' is not a valid integer." );
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
            return 0;
        }
    }

    /// <summary>   Reads status byte. </summary>
    /// <remarks>   2026-01-29. </remarks>
    public static string ReadStatusByteHex( this IMessageBasedSession? session )
    {
        ArgumentNullException.ThrowIfNull( session );
        try
        {
            short? status = ( short? ) session.ReadStatusByte();
            if ( status is null )
                return "??";
            else
                return $"0x{( int ) status:X2}";
        }
        catch ( Exception exp )
        {
            _ = MessageBox.Show( exp.Message );
            return "E";
        }
    }

    /// <summary>
    /// Synchronously reads ASCII-encoded string data until an END indicator or
    /// <see cref="ReadTerminationCharacter">termination character</see>
    /// termination character is reached irrespective of the buffer size.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <exception cref="cc.isr.VI.Pith.NativeException">  Thrown when a Native error condition
    ///                                                 occurs. </exception>
    /// <returns>   The received message. </returns>
    ///
    public static string ReadLine( this IMessageBasedSession? session )
    {
        ArgumentNullException.ThrowIfNull( session );
        System.Text.StringBuilder builder = new();
        try
        {
            ReadStatus endReadStatus = ReadStatus.Unknown;
            int bufferSize = session is Ivi.Visa.INativeVisaSession s
                ? s.GetAttributeInt32( Ivi.Visa.NativeVisaAttribute.ReadBufferSize ) : 1024;
            bool hitEndRead = false;
            do
            {
                string msg = session.RawIO.ReadString( bufferSize, out endReadStatus );
                hitEndRead = endReadStatus is ReadStatus.EndReceived or ReadStatus.TerminationCharacterEncountered;
                _ = builder.Append( msg );
                Application.DoEvents();
            }
            while ( !hitEndRead );
        }
        catch ( Exception )
        {
            throw;
        }
        finally
        {
        }
        return builder.ToString();
    }

    /// <summary>
    /// Synchronously reads ASCII-encoded string data. Reads up to the
    /// <see cref="TerminationCharacters">termination characters</see>.
    /// </summary>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters">termination characters</see>.
    /// </returns>
    public static string ReadLineTrimEnd( this IMessageBasedSession? session )
    {
        ArgumentNullException.ThrowIfNull( session );
        return session.ReadLine().TrimEnd( '\n' );
    }

    /// <summary>   Asynchronous delay. </summary>
    /// <remarks>   2026-01-29. </remarks>
    /// <param name="millisecondsDelay">    The milliseconds delay. </param>
    /// <returns>   A Task. </returns>
    public static void AsyncDelay( int millisecondsDelay, CancellationToken cancellationToken )
    {
        Task.Delay( millisecondsDelay, cancellationToken ).Wait( cancellationToken );
    }

    /// <summary>   Writes a line. </summary>
    /// <remarks>   2026-01-29. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <param name="command">  The command. </param>
    public static void WriteLine( this IMessageBasedSession? session, string command )
    {
        ArgumentNullException.ThrowIfNull( session );
        if ( string.IsNullOrWhiteSpace( command ) ) throw new ArgumentNullException( nameof( command ) );
        if ( !command.EndsWith( "\n", StringComparison.OrdinalIgnoreCase ) )
            command += "\n";
        session.RawIO.Write( command );
    }

    /// <summary>
    /// An IMessageBasedSession? extension method that reads operation complete reply.
    /// </summary>
    /// <remarks>   2026-01-29. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="timeoutMilliseconds">  (Optional) The timeout in milliseconds. </param>
    /// <returns>   The operation complete reply. </returns>
    public static string ReadOperationCompleteReply( this IMessageBasedSession? session, int timeoutMilliseconds = 10000 )
    {
        ArgumentNullException.ThrowIfNull( session );
        string reply = string.Empty;
        int timeOut = session.TimeoutMilliseconds;
        try
        {
            session.TimeoutMilliseconds = timeoutMilliseconds;
            reply = session.ReadLineTrimEnd();
        }
        catch
        {
            throw;
        }
        finally
        {
            session.TimeoutMilliseconds = timeOut;
        }
        return reply;
    }

    /// <summary>
    /// An IMessageBasedSession? extension method that queries operation completed.
    /// </summary>
    /// <remarks>   2026-01-29. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="cancellationToken">    A token that allows processing to be cancelled. </param>
    /// <param name="timeoutMilliseconds">  (Optional) The timeout in milliseconds. </param>
    /// <returns>   The operation completed. </returns>
    public static bool? QueryOperationCompleted( this IMessageBasedSession? session, CancellationToken cancellationToken, int timeoutMilliseconds = 10000 )
    {
        ArgumentNullException.ThrowIfNull( session );
        session.WriteLine( "*OPC?" );
        SessionExtensionMethods.AsyncDelay( 10, cancellationToken );
        string reply = session.ReadOperationCompleteReply( timeoutMilliseconds );
        return string.Equals( "1", reply, StringComparison.OrdinalIgnoreCase );
    }

    /// <summary>
    /// An IMessageBasedSession? extension method that executes the command and wait for operation
    /// completion. operation.
    /// </summary>
    /// <remarks>
    /// 2024-10-08. Important: This method does not restore the service request and standard event
    /// enable registers.
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="command">              The command. </param>
    /// <param name="cancelleation">        The cancellation token. </param>
    /// <param name="queryCompleteTimeout"> (Optional) The query complete timeout. </param>
    /// <returns>   A bool? </returns>
    public static async Task<bool?> ExecuteCommandOperationCompleted( this IMessageBasedSession? session, string command,
        CancellationToken cancellationToken, int queryCompleteTimeout = 10000 )
    {
        ArgumentNullException.ThrowIfNull( session );
        session.WriteLine( command );
        SessionExtensionMethods.AsyncDelay( 10, cancellationToken );
        return session.QueryOperationCompleted( cancellationToken, queryCompleteTimeout );
    }

    /// <summary>
    /// An IMessageBasedSession? extension method that executes the commands and wait for operation
    /// completion. operation.
    /// </summary>
    /// <remarks>
    /// TSP instruments do not accept common command separate by semicolon. These commands need to be
    /// split and executed as separate commands. A delay is inserted after commands that have
    /// refractory periods defined such as reset or clear.
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="dataToWrite">          The data to write. </param>
    /// <param name="cancellationToken">    A token that allows processing to be cancelled. </param>
    /// <returns>   A bool? </returns>
    public static async Task<bool?> ExecuteCommandsOperationCompleted( this IMessageBasedSession? session, string dataToWrite,
        CancellationToken cancellationToken )
    {
        ArgumentNullException.ThrowIfNull( session );
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );

        const char compound_Commands_Separator = ';';

        if ( dataToWrite.Contains( compound_Commands_Separator ) )
        {
            Queue<string> q = new( dataToWrite.Split( compound_Commands_Separator ) );
            while ( q.Count != 0 )
            {
                string command = q.Dequeue().Trim();
                if ( !string.IsNullOrWhiteSpace( command ) )
                {
                    bool success = ( bool ) await session.ExecuteCommandOperationCompleted( command, cancellationToken, queryCompleteTimeout: 10000 );
                    Application.DoEvents();
                    if ( !success )
                        return false;
                }
            }
        }
        else
        {
            bool success = ( bool ) await session.ExecuteCommandOperationCompleted( dataToWrite, cancellationToken, queryCompleteTimeout: 10000 );
            Application.DoEvents();
            if ( !success )
                return false;
        }
        return true;
    }
}
