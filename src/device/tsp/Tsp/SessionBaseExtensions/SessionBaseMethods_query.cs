using cc.isr.VI.Tsp.ParseStringExtensions;

namespace cc.isr.VI.Tsp.SessionBaseExtensions;

public static partial class SessionBaseMethods
{
    #region " Data Queue Query and Clear "

    /// <summary>   Queries the data queue count. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="node">     . </param>
    /// <returns>   Count. </returns>
    public static int QueryDataQueueCount( this Pith.SessionBase? session, NodeEntityBase? node )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        return session.QueryPrint( 0, 1, $"node[{node.Number}].dataqueue.count" );
    }

    /// <summary>   clears the data queue for the specified node and issues a wait complete command. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="node">                 Specifies the node. </param>
    /// <param name="reportQueueNotEmpty">  (Optional) true to report queue not empty. </param>
    public static void ClearDataQueueWaitComplete( this Pith.SessionBase? session, NodeEntityBase? node, bool reportQueueNotEmpty = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        if ( reportQueueNotEmpty )
        {
            if ( session.QueryDataQueueCount( node ) > 0 )
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Data queue not empty on node {node.Number};. " );
        }
        _ = session.WriteLine( "node[{0}].dataqueue.clear() waitcomplete({0})", node.Number );
    }

    #endregion

    #region " Query numeric values "

    /// <summary>   Query a string value from the instrument. </summary>
    /// <remarks>   2024-11-04. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid or if messages were left on the output buffer.</exception>
    /// <param name="session">  The session. </param>
    /// <param name="query">    The query. </param>
    /// <param name="caption">  (Optional) ("") The caption describing the query. Replaced by the <paramref name="query"/> if empty. </param>
    /// <returns>   A string. </returns>
    public static string QueryStringThrowIfError( this Pith.SessionBase? session, string query, string caption = "" )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"The VISA session to '{session.CandidateResourceName}' should be open." );
        if ( query is null ) throw new ArgumentNullException( nameof( query ), $"{nameof( query )} must not be null." );
        if ( string.IsNullOrWhiteSpace( query ) ) throw new ArgumentNullException( nameof( query ), $"{nameof( query )} must not be empty or whitespace." );
        caption = (caption is null) ? query : caption;

        _ = session.WriteLine( query );
        _ = Pith.SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        string reading = session.ReadLineTrimEnd();

        if ( string.IsNullOrWhiteSpace( reading ) ) throw new InvalidOperationException( $"The fetched '{caption}' reading should not be null or empty." );

        if ( string.Equals( cc.isr.VI.Syntax.Tsp.Lua.NilValue, reading, StringComparison.OrdinalIgnoreCase ) )
            throw new InvalidOperationException( $"The fetched '{caption}' reading '{reading}' should not be '{cc.isr.VI.Syntax.Tsp.Lua.NilValue}'." );

        string orphanMessages = session.ReadLines( session.StatusReadDelay, TimeSpan.FromMilliseconds( 100 ), true );
        if ( !string.IsNullOrWhiteSpace( orphanMessages ) ) throw new InvalidOperationException( $"There should be no orphan messages after executing '{query}':\n{orphanMessages}\n" );
        session.ThrowDeviceExceptionIfError( failureMessage: $"The query '{query}' failed" );

        return reading;
    }

    /// <summary>   Query a string value from the instrument. </summary>
    /// <remarks>   2024-11-04. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid or if messages were left on the output
    ///                                                 buffer. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="query">    The query. </param>
    /// <param name="caption">  (Optional) The caption describing the query. Replaced by the <paramref name="query"/> if empty. </param>
    /// <returns>   A string. </returns>
    public static string QueryStringOrNilThrowIfError( this Pith.SessionBase? session, string query, string caption = "" )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"The VISA session to '{session.CandidateResourceName}' should be open." );
        if ( query is null ) throw new ArgumentNullException( nameof( query ), $"{nameof( query )} must not be null." );
        if ( string.IsNullOrWhiteSpace( query ) ) throw new ArgumentNullException( nameof( query ), $"{nameof( query )} must not be empty or whitespace." );
        caption = (caption is null) ? query : caption;

        _ = session.WriteLine( query );
        _ = Pith.SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        string reading = session.ReadLineTrimEnd();
        if ( string.IsNullOrWhiteSpace( reading ) ) throw new InvalidOperationException( $"The fetched {caption} reading should not be null or empty." );

        string orphanMessages = session.ReadLines( session.StatusReadDelay, TimeSpan.FromMilliseconds( 100 ), true );
        if ( !string.IsNullOrWhiteSpace( orphanMessages ) ) throw new InvalidOperationException( $"There should be no orphan messages after executing '{query}':\n{orphanMessages}\n" );
        session.ThrowDeviceExceptionIfError( failureMessage: $"The query '{query}' failed" );

        return reading;
    }

    /// <summary>   Query a Boolean value from the instrument. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid or if messages were left on the output
    ///                                                 buffer or if the fetched value failed to parse. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="query">    The query. </param>
    /// <param name="caption">  (Optional) ("") The caption describing the query. Replaced by the <paramref name="query"/>
    ///                         if empty. </param>
    /// <returns>   The returned boolean value. </returns>
    public static bool QueryBoolThrowIfError( this Pith.SessionBase? session, string query, string caption = "" )
    {
        string reading = QueryStringThrowIfError( session, query, caption ).Trim();
        if ( bool.TryParse( reading, out bool value ) )
            return value;
        else
            throw new InvalidOperationException( $"The reading '{reading}' for the {(caption is null ? query : caption)} query failed to parse." );
    }

    /// <summary>   Query a nullable Boolean value from the instrument. </summary>
    /// <remarks>   2024-10-31. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid or if messages were left on the output
    ///                                                 buffer or if the fetched value failed to parse. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="query">    The query. </param>
    /// <param name="caption">  (Optional) ("") The caption describing the query. Replaced by the <paramref name="query"/>
    ///                         if empty. </param>
    /// <returns>   A bool? </returns>
    public static bool? QueryNullableBoolThrowIfError( this Pith.SessionBase? session, string query, string caption = "" )
    {
        string reading = QueryStringOrNilThrowIfError( session, query, caption ).Trim();
        if ( reading.TryParseNullableBool( out bool? result ) )
            return result;
        else
            throw new InvalidOperationException( $"The reading '{reading}' for the {(caption is null ? query : caption)} query failed to parse." );
    }

    /// <summary>   Query an Integer value from the instrument. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid or if messages were left on the output
    ///                                                 buffer or if the fetched value failed to parse. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="query">    The query. </param>
    /// <param name="caption">  (Optional) ("") The caption describing the query. Replaced by the <paramref name="query"/>
    ///                         if empty. </param>
    /// <returns>   An int. </returns>
    public static int QueryIntegerThrowIfError( this Pith.SessionBase? session, string query, string caption = "" )
    {
        string reading = QueryStringThrowIfError( session, query, caption ).Trim();
        if ( int.TryParse( reading, System.Globalization.NumberStyles.AllowExponent | System.Globalization.NumberStyles.AllowDecimalPoint,
            System.Globalization.CultureInfo.CurrentCulture, out int value ) )
            return value;
        else
            throw new InvalidOperationException( $"The reading '{reading}' for the {(caption is null ? query : caption)} query failed to parse." );
    }

    /// <summary>   Query an nullable Integer value from the instrument. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid or if messages were left on the output
    ///                                                 buffer or if the fetched value failed to parse. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="query">    The query. </param>
    /// <param name="caption">  (Optional) ("") The caption describing the query. Replaced by the <paramref name="query"/>
    ///                         if empty. </param>
    /// <returns>   An int? </returns>
    public static int? QueryNullableIntegerThrowIfError( this Pith.SessionBase? session, string query, string caption = "" )
    {
        string reading = QueryStringOrNilThrowIfError( session, query, caption ).Trim();
        if ( reading.TryParseNullableInteger( out int? result ) )
            return result;
        else
            throw new InvalidOperationException( $"The reading '{reading}' for the {(caption is null ? query : caption)} query failed to parse." );
    }

    /// <summary>   Query an Double value from the instrument. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid or if messages were left on the output
    ///                                                 buffer or if the fetched value failed to parse. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="query">    The query. </param>
    /// <param name="caption">  (Optional) ("") The caption describing the query. Replaced by the <paramref name="query"/>
    ///                         if empty. </param>
    /// <returns>   A double. </returns>
    public static double QueryDoubleThrowIfError( this Pith.SessionBase? session, string query, string caption = "" )
    {
        string reading = QueryStringThrowIfError( session, query, caption ).Trim();
        if ( double.TryParse( reading, out double value ) )
            return value;
        else
            throw new InvalidOperationException( $"The reading '{reading}' for the {(caption is null ? query : caption)} query failed to parse." );
    }

    /// <summary>   Query an nullable Double value from the instrument. </summary>
    /// <remarks>   2024-10-26. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid or if messages were left on the output
    ///                                                 buffer or if the fetched value failed to parse. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="query">    The query. </param>
    /// <param name="caption">  (Optional) ("") The caption describing the query. Replaced by the <paramref name="query"/>
    ///                         if empty. </param>
    /// <returns>   A double? </returns>
    public static double? QueryNullableDoubleThrowIfError( this Pith.SessionBase? session, string query, string caption = "" )
    {
        string reading = QueryStringOrNilThrowIfError( session, query, caption ).Trim();
        if ( reading.TryParseNullableDouble( out double? result ) )
            return result;
        else
            throw new InvalidOperationException( $"The reading '{reading}' for the {(caption is null ? query : caption)} query failed to parse." );
    }

    #endregion
}
