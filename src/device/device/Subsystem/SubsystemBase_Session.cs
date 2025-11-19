using System.Diagnostics;

namespace cc.isr.VI;

public abstract partial class SubsystemBase
{

    #region " enumeration "

    /// <summary> Writes the Enum present value without reading back the present value from the device. </summary>
    /// <param name="commandFormat"> The command format. </param>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <returns> The present value or none if unknown. </returns>
    public T WriteValue<T>( string commandFormat, T value ) where T : struct, Enum
    {
        return this.Session.WriteEnumValue( value, commandFormat );
    }

    /// <summary>
    /// Issues the query dataToWrite and parses the returned enum present value name into an Enum.
    /// </summary>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <returns> The parsed present value or none if unknown. </returns>
    public T QueryEnum<T>( string queryCommand, T value ) where T : struct, Enum
    {
        return this.Session.QueryEnum( value, queryCommand );
    }

    /// <summary>
    /// Issues the query dataToWrite and parses the returned enum present value name into an Enum.
    /// </summary>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <returns> The parsed present value or none if unknown. </returns>
    public T? QueryEnum<T>( string queryCommand, T? value ) where T : struct, Enum
    {
        return this.Session.QueryEnum( value, queryCommand );
    }

    /// <summary>
    /// Issues the query dataToWrite and parses the returned enum present value name into an Enum.
    /// </summary>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="readWrites">   The read writes. </param>
    /// <returns> The parsed present value or none if unknown. </returns>
    public T QueryEnum<T>( string queryCommand, T value, Pith.EnumReadWriteCollection readWrites ) where T : struct, Enum
    {
        return this.Session.Query( value, readWrites, queryCommand );
    }

    /// <summary> Queries first present value returned from the instrument. </summary>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="readWrites">   The read writes. </param>
    /// <returns> The first present value. </returns>
    public T QueryFirstValue<T>( string queryCommand, T value, Pith.EnumReadWriteCollection readWrites ) where T : struct, Enum
    {
        return this.Session.QueryFirst( value, readWrites, queryCommand );
    }

    /// <summary> Writes the Enum present value name without reading back the present value from the device. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="commandFormat"> The command format. </param>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <param name="readWrites">    Dictionary of parses. </param>
    /// <returns> The present value or none if unknown. </returns>
    public T WriteEnum<T>( string commandFormat, T value, Pith.EnumReadWriteCollection readWrites ) where T : struct, Enum
    {
        return this.Session.Write( value, commandFormat, readWrites );
    }

    /// <summary> Writes the Enum present value name without reading back the present value from the device. </summary>
    /// <param name="commandFormat"> The command format. </param>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <returns> The present value or none if unknown. </returns>
    public T WriteEnum<T>( string commandFormat, T value ) where T : struct, Enum
    {
        return this.Session.WriteEnum( value, commandFormat );
    }

    #endregion

    #region " boolean "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns> The present value. </returns>
    public bool? Query( bool? value, string queryCommand )
    {
        return string.IsNullOrWhiteSpace( queryCommand )
            ? value
            : this.Session.Query( value.GetValueOrDefault( default ), queryCommand );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns>   <see cref="QueryParseBooleanInfo"/> </returns>
    public QueryParseBooleanInfo QueryElapsed( bool? value, string queryCommand )
    {
        return string.IsNullOrWhiteSpace( queryCommand )
            ? QueryParseBooleanInfo.Empty
            : this.Session.QueryElapsed( value.GetValueOrDefault( default ), queryCommand );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   The present value. </returns>
    public bool? Query( bool? value, int receivedMessageItemIndex, string queryCommand )
    {
        return string.IsNullOrWhiteSpace( queryCommand )
            ? value
            : this.Session.Query( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand );
    }

    /// <summary>   Queries and parses the indexed present value from the instrument. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   <see cref="QueryParseBooleanInfo"/> </returns>
    public QueryParseBooleanInfo QueryElapsed( bool? value, int receivedMessageItemIndex, string queryCommand )
    {
        return !string.IsNullOrWhiteSpace( queryCommand )
            ? this.Session.QueryElapsed( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand )
            : QueryParseBooleanInfo.Empty;
    }

    /// <summary> Writes the value without reading back the present value from the device. </summary>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <returns> The present value. </returns>
    public bool Write( bool value, string commandFormat )
    {
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
        {
            _ = this.Session.WriteLine( commandFormat, value.GetHashCode() );
        }
        return value;
    }

    /// <summary>
    /// Writes the present value without reading back the present value from the device and returns the elapsed time.
    /// Synchronously writes ASCII-encoded string data to the device or interface. Converts the
    /// specified string to an ASCII string and appends it to the formatted I/O write buffer. Appends
    /// a newline (0xA) to the formatted I/O write buffer, flushes the buffer, and sends an END with
    /// the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">            The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat">    The command format. </param>
    /// <returns>   A Tuple: ( bool vSentValue, ExecuteInfo ExecuteInfo ). </returns>
    public (bool SentValue, WriteBooleanInfo WriteBooleanInfo) WriteLineElapsed( bool value, string commandFormat )
    {
        return string.IsNullOrWhiteSpace( commandFormat )
            ? (value, WriteBooleanInfo.Empty)
            : (value, new WriteBooleanInfo( value, this.Session.WriteLineElapsed( commandFormat, value.GetHashCode() ) ));
    }

    #endregion

    #region " integer "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns> The present value. </returns>
    public int? Query( int? value, string queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
            value = this.Session.Query( value.GetValueOrDefault( default ), queryCommand );
        return value;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns>   <see cref="QueryParseInfo{T}"/> </returns>
    public QueryParseInfo<int> QueryElapsed( int? value, string queryCommand )
    {
        return !string.IsNullOrWhiteSpace( queryCommand )
            ? this.Session.QueryElapsed( value.GetValueOrDefault( default ), queryCommand )
            : QueryParseInfo<int>.Empty;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   The present value. </returns>
    public int? Query( int? value, int receivedMessageItemIndex, string queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
            value = this.Session.Query( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand );
        return value;
    }

    /// <summary>   Queries and parses the indexed present value from the instrument. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   <see cref="QueryParseInfo{T}"/> </returns>
    public QueryParseInfo<int> QueryElapsed( int? value, int receivedMessageItemIndex, string queryCommand )
    {
        return !string.IsNullOrWhiteSpace( queryCommand )
            ? this.Session.QueryElapsed( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand )
            : QueryParseInfo<int>.Empty;
    }

    /// <summary> Writes the present value without reading back the present value from the device. </summary>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <returns> The present value. </returns>
    public int Write( int value, string commandFormat )
    {
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
            _ = this.Session.WriteLine( commandFormat, value );
        return value;
    }

    /// <summary>
    /// Writes the present value without reading back the present value from the device and returns the elapsed time.
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">            The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat">    The command format. </param>
    /// <returns>   A Tuple: (int SentValue, <see cref="WriteInfo{T}"/> WriteInfo ) . </returns>
    public (int SentValue, WriteInfo<int> WriteInfo) WriteLineElapsed( int value, string commandFormat )
    {
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
        {
            ExecuteInfo executeInfo = this.Session.WriteLineElapsed( commandFormat, value );
            return (value, new WriteInfo<int>( value, commandFormat, executeInfo.SentMessage, executeInfo.GetElapsedTimes() ));
        }
        return (value, WriteInfo<int>.Empty);
    }

    #endregion

    #region " double "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns> The present value. </returns>
    public double? Query( double? value, string queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
            value = this.Session.Query( value.GetValueOrDefault( default ), queryCommand );
        return value;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns>   <see cref="QueryParseInfo{T}"/> </returns>
    public QueryParseInfo<double> QueryElapsed( double? value, string queryCommand )
    {
        return !string.IsNullOrWhiteSpace( queryCommand )
            ? this.Session.QueryElapsed( value.GetValueOrDefault( default ), queryCommand )
            : QueryParseInfo<double>.Empty;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   The present value. </returns>
    public double? Query( double? value, int receivedMessageItemIndex, string queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
            value = this.Session.Query( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand );
        return value;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   A Tuple: (double? ReceivedValue, <see cref="QueryParseInfo{T}"/> QueryParseInfo). </returns>
    public QueryParseInfo<double> QueryElapsed( double? value, int receivedMessageItemIndex, string queryCommand )
    {
        return !string.IsNullOrWhiteSpace( queryCommand )
            ? this.Session.QueryElapsed( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand )
            : QueryParseInfo<double>.Empty;
    }

    /// <summary> Write the present value without reading back the present value from the device. </summary>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <returns> The present value. </returns>
    public double Write( double value, string commandFormat )
    {
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
        {
            if ( value >= VI.Syntax.ScpiSyntax.Infinity - 1d )
            {
                _ = this.Session.WriteLine( commandFormat, "MAX" );
                value = VI.Syntax.ScpiSyntax.Infinity;
            }
            else if ( value <= VI.Syntax.ScpiSyntax.NegativeInfinity + 1d )
            {
                _ = this.Session.WriteLine( commandFormat, "MIN" );
                value = VI.Syntax.ScpiSyntax.NegativeInfinity;
            }
            else
            {
                _ = this.Session.WriteLine( commandFormat, value );
            }
        }
        return value;
    }

    /// <summary> WriteEnum the present value without reading back the present value from the device and return the elapsed time. </summary>
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">            The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat">    The command format. </param>
    /// <returns>   A Tuple: (int SentValue, <see cref="WriteInfo{T}"/> WriteInfo ) . </returns>
    public (double SentValue, WriteInfo<double> WriteInfo) WriteLineElapsed( double value, string commandFormat )
    {
        Stopwatch sw = Stopwatch.StartNew();
        string sentMessage = string.Empty;
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
        {
            if ( value >= VI.Syntax.ScpiSyntax.Infinity - 1d )
            {
                sentMessage = this.Session.WriteLine( commandFormat, "MAX" );
                value = VI.Syntax.ScpiSyntax.Infinity;
            }
            else if ( value <= VI.Syntax.ScpiSyntax.NegativeInfinity + 1d )
            {
                sentMessage = this.Session.WriteLine( commandFormat, "MIN" );
                value = VI.Syntax.ScpiSyntax.NegativeInfinity;
            }
            else
            {
                sentMessage = this.Session.WriteLine( commandFormat, value );
            }
        }
        return (value, new WriteInfo<double>( value, commandFormat, sentMessage, sw.Elapsed ));
    }

    #endregion

    #region " string "

    /// <summary>   Reads the elapsed. </summary>
    /// <remarks>   David, 2021-04-16. </remarks>
    /// <returns>   The elapsed. </returns>
    public ReadInfo ReadElapsed()
    {
        return this.Session.ReadElapsed();
    }

    /// <summary>   Queries a string returning the elapsed time. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns>   A Tuple: (string ReceivedMessage, <see cref="QueryInfo"/>) . </returns>
    public QueryInfo QueryElapsed( string queryCommand )
    {
        return this.Session.QueryElapsed( queryCommand );
    }

    /// <summary>
    /// Queries a <see cref="string"/> returning The present value if the query dataToWrite is empty.
    /// </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="presentValue"> The present Value. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns>   The present value. </returns>
    public string? QueryTrimEnd( string? presentValue, string queryCommand )
    {
        return string.IsNullOrWhiteSpace( queryCommand ) ? presentValue : this.Session.QueryTrimEnd( queryCommand );
    }

    /// <summary> Queries a <see cref="string"/> returning The present value if the query dataToWrite is empty. </summary>
    /// <param name="presentValue">         The present value. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <param name="args">          A variable-length parameters list containing arguments. </param>
    /// <returns> The present value. </returns>
    public string? QueryTrimEnd( string? presentValue, string commandFormat, params object[] args )
    {
        return this.QueryTrimEnd( presentValue, string.Format( commandFormat, args ) );
    }

    /// <summary>   Queries a string returning the elapsed time. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">        The present value. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns>   A Tuple: (string ReceivedMessage, <see cref="QueryInfo"/>) . </returns>
    public QueryInfo QueryElapsed( string value, string queryCommand )
    {
        return string.IsNullOrWhiteSpace( queryCommand )
            ? new QueryInfo( queryCommand, string.Empty, value, TimeSpan.Zero )
            : this.Session.QueryElapsed( queryCommand );
    }

    /// <summary>   Queries a string returning the elapsed time. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">            The present value. </param>
    /// <param name="commandFormat">    The command format. </param>
    /// <param name="args">             A variable-length parameters list containing arguments. </param>
    /// <returns>   A Tuple: (string receivedMessage, TimeSpan Elapsed ) . </returns>
    public QueryInfo QueryElapsed( string value, string commandFormat, params object[] args )
    {
        return string.IsNullOrWhiteSpace( commandFormat )
            ? new QueryInfo( commandFormat, string.Empty, value, TimeSpan.Zero )
            : this.Session.QueryElapsed( string.Format( System.Globalization.CultureInfo.InvariantCulture, commandFormat, args ) );
    }

    /// <summary>
    /// WriteEnum the constructed command without reading back the present value from the device. Synchronously writes ASCII-
    /// encoded string data to the device or interface. Converts the specified string to an ASCII
    /// string and appends it to the formatted I/O write buffer. Appends a newline (0xA) to the
    /// formatted I/O write buffer, flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <param name="commandFormat">    The command format. </param>
    /// <param name="args">             A variable-length parameters list containing arguments. </param>
    /// <returns>   The present value. </returns>
    public string WriteLine( string commandFormat, params object[] args )
    {
        return this.Session.WriteLine( string.Format( commandFormat, args ) );
    }

    /// <summary>
    /// WriteEnum the <paramref name="dataToWrite"/> without reading back the present value from the device. Synchronously writes ASCII-
    /// encoded string data to the device or interface. Converts the specified string to an ASCII
    /// string and appends it to the formatted I/O write buffer. Appends a newline (0xA) to the
    /// formatted I/O write buffer, flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-13. </remarks>
    /// <param name="dataToWrite">    The dataToWrite. </param>
    /// <returns>   The present value. </returns>
    public string WriteLine( string dataToWrite )
    {
        return this.Session.WriteLine( dataToWrite );
    }

    /// <summary>
    /// WriteEnum the <paramref name="dataToWrite"/> without reading back the present value from the device and return the elapsed time.
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   <see cref="ExecuteInfo"/>. </returns>
    public ExecuteInfo WriteLineElapsed( string dataToWrite )
    {
        return this.Session.WriteLineElapsed( dataToWrite );
    }

    /// <summary>
    /// WriteEnum the constructed command without reading back the present value from the device and return the elapsed time.
    /// Synchronously writes ASCII-encoded string data to the device or interface.
    /// Converts the specified string to an ASCII string and appends it to the formatted
    /// I/O write buffer. Appends a newline (0xA) to the formatted I/O write buffer,
    /// flushes the buffer, and sends an END with the buffer if required.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="commandFormat">    The command format. </param>
    /// <param name="args">             A variable-length parameters list containing arguments. </param>
    /// <returns>   A Tuple: ( double SentValue, TimeSpan Elapsed ). </returns>
    public ExecuteInfo WriteLineElapsed( string commandFormat, params object[] args )
    {
        return this.Session.WriteLineElapsed( string.Format( System.Globalization.CultureInfo.InvariantCulture, commandFormat, args ) );
    }

    #endregion

    #region " time span "

    /// <summary>
    /// Queries and parses a <see cref="TimeSpan">TimeSpan</see> value using the given <paramref name="timespanFormat"/>
    /// returning the provided value if the <paramref name="dataToWrite"/> or <paramref name="timespanFormat"/> are
    /// empty.
    /// </summary>
    /// <remarks>   2024-07-05. </remarks>
    /// <param name="value">            The present value to return if the command is null or empty or white space. </param>
    /// <param name="timespanFormat">   Describes the timespan Format to use for parsing the returned
    ///                                 data. </param>
    /// <param name="dataToWrite">      The query dataToWrite. </param>
    /// <returns>   The present value. </returns>
    public TimeSpan Query( TimeSpan value, string timespanFormat, string dataToWrite )
    {
        return (string.IsNullOrWhiteSpace( timespanFormat ) || string.IsNullOrWhiteSpace( dataToWrite ))
            ? value
            : this.Session.Query( timespanFormat, dataToWrite );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   2024-07-05. </remarks>
    /// <param name="value">            The present value to return if the command is null or empty or white space. </param>
    /// <param name="timespanFormat">   Describes the timespan Format to use for parsing the returned
    ///                                 data. </param>
    /// <param name="dataToWrite">      The query dataToWrite. </param>
    /// <returns>   The present value. </returns>
    public TimeSpan? Query( TimeSpan? value, string timespanFormat, string dataToWrite )
    {
        return (string.IsNullOrWhiteSpace( timespanFormat ) || string.IsNullOrWhiteSpace( dataToWrite ))
            ? value
            : this.Session.Query( timespanFormat, dataToWrite );
    }

    /// <summary>
    /// Writes the <see cref="TimeSpan">TimeSpan</see> value building the data to write using the
    /// given <paramref name="commandFormat"/> returning the provided value if the <paramref name="commandFormat"/>
    /// is empty.
    /// </summary>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <returns> The present value. </returns>
    public string Write( TimeSpan value, string commandFormat )
    {
        return string.IsNullOrWhiteSpace( commandFormat )
            ? value.ToString()
            : this.Session.WriteLine( commandFormat, value );
    }

    #endregion

    #region " payload "

    /// <summary> Issues the query dataToWrite and parses the returned payload. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="payload"> The payload. </param>
    /// <returns>
    /// <c>true</c> if <see cref="VI.Pith.PayloadStatus"/> is
    /// <see cref="VI.Pith.PayloadStatus.Okay"/>; otherwise <c>false</c>.
    /// </returns>
    public bool Query( Pith.PayloadBase payload )
    {
        if ( payload is null ) throw new ArgumentNullException( nameof( payload ) );
        bool result = true;
        if ( !string.IsNullOrWhiteSpace( payload.QueryCommand ) )
        {
            result = this.Session.Query( payload );
        }

        return result;
    }

    /// <summary>
    /// WriteEnum the payload. A <see cref="Query(VI.Pith.PayloadBase)"/> must be issued to get the present value
    /// from the device.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="payload"> The payload. </param>
    /// <returns>
    /// <c>true</c> if <see cref="VI.Pith.PayloadStatus"/> is
    /// <see cref="VI.Pith.PayloadStatus.Okay"/>; otherwise <c>false</c>.
    /// </returns>
    public bool Write( Pith.PayloadBase payload )
    {
        if ( payload is null ) throw new ArgumentNullException( nameof( payload ) );
        bool result = true;
        if ( !string.IsNullOrWhiteSpace( payload.CommandFormat ) )
        {
            result = this.Session.Write( payload );
        }

        return result;
    }

    #endregion

    #region " query with status read "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <remarks>   David, 2021-04-03. </remarks>
    /// <param name="readReadyDelay">   The read ready delay. </param>
    /// <param name="readDelay">        The read delay. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <param name="queryStatusError"> The query status error. </param>
    /// <returns>   <see cref="QueryInfo"/>. </returns>
    public QueryInfo QueryStatusRead( TimeSpan readReadyDelay, TimeSpan readDelay, string dataToWrite,
        Func<(bool, string, int)> queryStatusError )
    {
        return this.Session.QueryIfNoStatusError( readReadyDelay, readDelay, dataToWrite, queryStatusError );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <param name="statusReadDelay"> The read delay. </param>
    /// <param name="readDelay">       The read delay. </param>
    /// <param name="dataToWrite">     The data to write. </param>
    /// <returns>
    /// The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>.
    /// </returns>
    public string Query( TimeSpan statusReadDelay, TimeSpan readDelay, string dataToWrite )
    {
        return this.Session.QueryIfNoStatusError( statusReadDelay, readDelay, dataToWrite ).ReceivedMessage;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous status
    /// after the specified delay. A final read is performed after a delay provided no error occurred.
    /// </summary>
    /// <param name="statusReadDelay"> The read delay. </param>
    /// <param name="readDelay">       The read delay. </param>
    /// <param name="dataToWrite">     The data to write. </param>
    /// <returns> The trim end. </returns>
    public string? QueryTrimEnd( TimeSpan statusReadDelay, TimeSpan readDelay, string dataToWrite )
    {
        return this.Session.QueryTrimEnd( statusReadDelay, readDelay, dataToWrite );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read
    /// after the specified delay.
    /// </summary>
    /// <param name="readDelay">   The read delay. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns>
    /// The  <see cref="VI.Pith.SessionBase.LastMessageReceived">last received data</see>.
    /// </returns>
    public string Query( TimeSpan readDelay, string dataToWrite )
    {
        return this.Session.Query( readDelay, dataToWrite );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read
    /// after the specified delay.
    /// </summary>
    /// <param name="readDelay">   The read delay. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> The trim end. </returns>
    public string QueryTrimEnd( TimeSpan readDelay, string dataToWrite )
    {
        return this.Session.QueryTrimEnd( readDelay, dataToWrite );
    }

    #endregion
}
