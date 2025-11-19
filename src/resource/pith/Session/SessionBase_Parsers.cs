// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Enums;
using System.ComponentModel;
using System.Reflection;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " boolean parsers "

    /// <summary> Parses a value to a Boolean. </summary>
    /// <exception cref="FormatException"> Thrown when the format of the received message is
    /// incorrect. </exception>
    /// <param name="value"> The value. </param>
    /// <returns>
    /// <c>true</c> if the value equals '1' or <c>false</c> if '0'; otherwise an exception is thrown.
    /// </returns>
    public bool ParseBoolean( string value )
    {
        return TryParse( value, out bool result )
            ? result
            : throw new FormatException( $"{this.ResourceNameCaption} '{value}' is invalid Boolean format" );
    }

    /// <summary> Converts a value to an one zero. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> value as a <see cref="string" />. </returns>
    public static string ToOneZero( bool value )
    {
        return $"{value.GetHashCode():'1';'1';'0'}";
    }

    /// <summary> Converts a value to a true false. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> value as a <see cref="string" />. </returns>
    public static string ToTrueFalse( bool value )
    {
        return $"{value.GetHashCode():'true';'true';'false'}";
    }

    /// <summary> Tries to parse a value to a Boolean. </summary>
    /// <param name="value">  The value. </param>
    /// <param name="result"> [out] Value read from the instrument. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public static bool TryParse( string value, out bool result )
    {
        result = default;
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            return false;
        }
        else if ( int.TryParse( value, out int numericValue ) )
        {
            result = numericValue != 0;
            return true;
        }
        else
        {
            return bool.TryParse( value, out result );
        }
    }

    #endregion

    #region " decimal parsers "

    /// <summary> Parses a value to Decimal. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    /// illegal values. </exception>
    /// <exception cref="FormatException">       Thrown when the format of the received message is
    /// incorrect. </exception>
    /// <param name="value"> The value. </param>
    /// <returns> Value if the value is a valid number; otherwise, Default. </returns>
    public decimal ParseDecimal( string value )
    {
        return value is null
            ? throw new ArgumentNullException( nameof( value ), "Query not executed" )
            : string.IsNullOrWhiteSpace( value )
                ? throw new ArgumentException( "Query returned an empty string", nameof( value ) )
                : decimal.TryParse( value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent, System.Globalization.CultureInfo.InvariantCulture,
                                    out decimal parsedValue )
                                ? parsedValue
                                : throw new FormatException( $"{this.ResourceNameCaption} '{value}' is invalid Decimal format" );
    }

    /// <summary> Tries to parse a Decimal reading. </summary>
    /// <param name="value">  The value. </param>
    /// <param name="result"> [in,out] The result. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public static bool TryParse( string value, out decimal result )
    {
        result = default;
        return !string.IsNullOrWhiteSpace( value )
                && decimal.TryParse( value,
                                     System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                                     System.Globalization.CultureInfo.InvariantCulture, out result );
    }

    #endregion

    #region " double parsers "

    /// <summary>   Parses a value to Double. </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <exception cref="FormatException">          Thrown when the format of the received message is
    ///                                             incorrect. </exception>
    /// <param name="value">    The value. </param>
    /// <returns>   Value if the value is a valid number; otherwise, Default. </returns>
    public double ParseDouble( string value )
    {
        return value is null
            ? throw new ArgumentNullException( nameof( value ), "Query not executed" )
            : string.IsNullOrWhiteSpace( value )
                ? throw new ArgumentException( "Query returned an empty string", nameof( value ) )
                : double.TryParse( value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                                          System.Globalization.CultureInfo.InvariantCulture, out double parsedValue )
                                ? parsedValue
                                : throw new FormatException( $"{this.ResourceNameCaption} '{value}' is invalid Double format" );
    }

    /// <summary> Tries to parse a Double reading. </summary>
    /// <param name="value">  The value. </param>
    /// <param name="result"> [out] The result. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public static bool TryParse( string value, out double result )
    {
        result = default;
        return !string.IsNullOrWhiteSpace( value )
                && double.TryParse( value,
                                    System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                                    System.Globalization.CultureInfo.InvariantCulture, out result );
    }

    #endregion

    #region " integer parsers "

    /// <summary> Parses a value to Integer. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    /// illegal values. </exception>
    /// <exception cref="FormatException">       Thrown when the format of the received message is
    /// incorrect. </exception>
    /// <param name="value"> The value. </param>
    /// <returns> Value if the value is a valid number; otherwise, Default. </returns>
    public int ParseInteger( string value )
    {
        return value is null
            ? throw new ArgumentNullException( nameof( value ), "Query not executed" )
            : string.IsNullOrWhiteSpace( value )
                ? throw new ArgumentException( "Query returned an empty string", nameof( value ) )
                : int.TryParse( value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                                            System.Globalization.CultureInfo.InvariantCulture, out int parsedValue )
                                ? parsedValue
                                : throw new FormatException( $"{this.ResourceNameCaption} '{value}' is invalid Integer format" );
    }

    /// <summary> Tries to parse a value to Integer. </summary>
    /// <param name="value">  The value. </param>
    /// <param name="result"> [out] The result. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public static bool TryParse( string value, out int result )
    {
        result = default;
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            return false;
        }
        // check if we have an infinity.
        else if ( TryParse( value, out double inf ) && (inf > int.MaxValue || inf < int.MinValue) )
        {
            result = inf > int.MaxValue ? int.MaxValue : int.MinValue;
            return true;
        }
        else
        {
            return int.TryParse( value, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                                 System.Globalization.CultureInfo.InvariantCulture, out result );
        }
    }

    #endregion

    #region " boolean query and parse

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns> The presentValue. </returns>
    public bool? Query( bool? value, string queryCommand )
    {
        return string.IsNullOrWhiteSpace( queryCommand )
            ? value
            : this.Query( value.GetValueOrDefault( default ), queryCommand );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The parsed value or default. </returns>
    public bool Query( bool emulatingValue, string dataToWrite )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return string.IsNullOrWhiteSpace( dataToWrite )
            ? emulatingValue
            : this.ParseBoolean( this.QueryTrimEnd( dataToWrite ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The format of the data to write. </param>
    /// <param name="args">             The format arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public bool Query( bool emulatingValue, string format, params object[] args )
    {
        return this.Query( emulatingValue, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
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
            : this.QueryElapsed( value.GetValueOrDefault( default ), queryCommand );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   A Tuple:  (bool ParsedValue, <see cref="QueryParseBooleanInfo"/> )  </returns>
    public QueryParseBooleanInfo QueryElapsed( bool emulatingValue, string dataToWrite )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return new QueryParseBooleanInfo( emulatingValue, string.Empty, QueryInfo.Empty );
        else
        {
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters );
            bool parsedValue = this.ParseBoolean( parsedMessage );
            return new QueryParseBooleanInfo( parsedValue, parsedMessage, queryInfo );
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The format of the data to write. </param>
    /// <param name="args">             The format arguments. </param>
    /// <returns>   A Tuple:  (bool ParsedValue, <see cref="QueryParseBooleanInfo"/> )  </returns>
    public QueryParseBooleanInfo QueryElapsed( bool emulatingValue, string format, params object[] args )
    {
        return this.QueryElapsed( emulatingValue, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   The presentValue. </returns>
    public bool? Query( bool? value, int receivedMessageItemIndex, string queryCommand )
    {
        return string.IsNullOrWhiteSpace( queryCommand )
            ? value
            : this.Query( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">           The value to emulate if emulated reply is empty. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="dataToWrite">              The data to write. </param>
    /// <returns>   The second. </returns>
    public bool Query( bool emulatingValue, int receivedMessageItemIndex, string dataToWrite )
    {
        if ( string.IsNullOrEmpty( dataToWrite ) ) return emulatingValue;

        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        dataToWrite = this.QueryTrimEnd( dataToWrite );
        return this.ParseBoolean( dataToWrite.Split( ',' )[receivedMessageItemIndex] );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">           The value to emulate if emulated reply is empty. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="dataToWrite">              The data to write. </param>
    /// <returns>   A Tuple:  (bool ParsedValue, <see cref="QueryParseBooleanInfo"/> )  </returns>
    public QueryParseBooleanInfo QueryElapsed( bool emulatingValue, int receivedMessageItemIndex, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return new QueryParseBooleanInfo( emulatingValue, string.Empty, QueryInfo.Empty );
        else
        {
            this.MakeEmulatedReplyIfEmpty( emulatingValue );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters ).Split( ',' )[receivedMessageItemIndex];
            bool parsedValue = this.ParseBoolean( parsedMessage );
            return new QueryParseBooleanInfo( parsedValue, parsedMessage, queryInfo );
        }
    }

    /// <summary>   Queries and parses the indexed presentValue from the instrument. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   <see cref="QueryParseBooleanInfo"/> </returns>
    public QueryParseBooleanInfo QueryElapsed( bool? value, int receivedMessageItemIndex, string queryCommand )
    {
        return !string.IsNullOrWhiteSpace( queryCommand )
            ? this.QueryElapsed( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand )
            : QueryParseBooleanInfo.Empty;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">       [in,out] The value. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQuery( ref bool value, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return value;
        this.MakeEmulatedReplyIfEmpty( value );
        return SessionBase.TryParse( this.QueryTrimEnd( dataToWrite ), out value );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">  [in,out] The value. </param>
    /// <param name="format"> The format of the data to write. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQuery( ref bool value, string format, params object[] args )
    {
        return this.TryQuery( ref value, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">        [in,out] The value. </param>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   A Tuple:  (bool success, <see cref="QueryParseBooleanInfo"/> )  </returns>
    public (bool Success, QueryParseBooleanInfo QueryParseBooleanInfo) TryQueryElapsed( ref bool value, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return (false, new QueryParseBooleanInfo( value, string.Empty, QueryInfo.Empty ));
        else
        {
            this.MakeEmulatedReplyIfEmpty( value );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters );
            bool success = SessionBase.TryParse( parsedMessage, out bool parsedValue );
            return (success, new QueryParseBooleanInfo( parsedValue, parsedMessage, queryInfo ));
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">    [in,out] The value. </param>
    /// <param name="format">   The format of the data to write. </param>
    /// <param name="args">     The format arguments. </param>
    /// <returns>   A Tuple:  (bool success, <see cref="QueryParseBooleanInfo"/> )  </returns>
    public (bool Success, QueryParseBooleanInfo QueryParseBooleanInfo) TryQueryElapsed( ref bool value, string format, params object[] args )
    {
        return this.TryQueryElapsed( ref value, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    #endregion

    #region " decimal query and parse "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The parsed value or default. </returns>
    public decimal Query( decimal emulatingValue, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return emulatingValue;
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseDecimal( this.QueryTrimEnd( dataToWrite ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The format of the data to write. </param>
    /// <param name="args">             The format arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public decimal? Query( decimal emulatingValue, string format, params object[] args )
    {
        return this.Query( emulatingValue, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   A Tuple:  (decimal ParsedValue, <see cref="QueryParseInfo{T}"/> )  </returns>
    public QueryParseInfo<decimal> QueryElapsed( decimal emulatingValue, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return new QueryParseInfo<decimal>( emulatingValue, string.Empty, QueryInfo.Empty );
        else
        {
            this.MakeEmulatedReplyIfEmpty( emulatingValue );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters );
            decimal parsedValue = this.ParseDecimal( parsedMessage );
            return new QueryParseInfo<decimal>( parsedValue, parsedMessage, queryInfo );
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The format of the data to write. </param>
    /// <param name="args">             The format arguments. </param>
    /// <returns>   A Tuple:  (decimal ParsedValue, <see cref="QueryParseInfo{T}"/> ) </returns>
    public QueryParseInfo<decimal> QueryElapsed( decimal emulatingValue, string format, params object[] args )
    {
        return this.QueryElapsed( emulatingValue, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">           The value to emulate if emulated reply is empty. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="dataToWrite">              The data to write. </param>
    /// <returns>   The second. </returns>
    public decimal Query( decimal emulatingValue, int receivedMessageItemIndex, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return emulatingValue;

        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        dataToWrite = this.QueryTrimEnd( dataToWrite ) ?? $"{emulatingValue}";
        return this.ParseDecimal( dataToWrite.Split( ',' )[receivedMessageItemIndex] );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">           The value to emulate if emulated reply is empty. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="dataToWrite">              The data to write. </param>
    /// <returns>   A Tuple:  (decimal ParsedValue, <see cref="QueryParseInfo{T}"/> )  </returns>
    public QueryParseInfo<decimal> QueryElapsed( decimal emulatingValue, int receivedMessageItemIndex, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return new QueryParseInfo<decimal>( emulatingValue, string.Empty, QueryInfo.Empty );
        else
        {
            this.MakeEmulatedReplyIfEmpty( emulatingValue );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters ).Split( ',' )[receivedMessageItemIndex];
            decimal parsedValue = this.ParseDecimal( parsedMessage );
            return new QueryParseInfo<decimal>( parsedValue, parsedMessage, queryInfo );
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">       [in,out] The value. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQuery( ref decimal value, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return false;

        this.MakeEmulatedReplyIfEmpty( value );
        return SessionBase.TryParse( this.QueryTrimEnd( dataToWrite ), out value );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">  [in,out] The value. </param>
    /// <param name="format"> The format of the data to write. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQuery( ref decimal value, string format, params object[] args )
    {
        return this.TryQuery( ref value, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">        [in,out] The value. </param>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   A Tuple:  (bool Success, <see cref="QueryParseInfo{T}"/> )  </returns>
    public (bool Success, QueryParseInfo<decimal> QueryParseBooleanInfo) TryQueryElapsed( ref decimal value, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return (false, new QueryParseInfo<decimal>( value, string.Empty, QueryInfo.Empty ));
        else
        {
            this.MakeEmulatedReplyIfEmpty( value );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters );
            bool success = SessionBase.TryParse( parsedMessage, out decimal parsedValue );
            return (success, new QueryParseInfo<decimal>( parsedValue, parsedMessage, queryInfo ));
        }

    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">    [in,out] The value. </param>
    /// <param name="format">   The format of the data to write. </param>
    /// <param name="args">     The format arguments. </param>
    /// <returns>   A Tuple:  (bool Success, <see cref="QueryParseInfo{T}"/> )  </returns>
    public (bool Success, QueryParseInfo<decimal> QueryParseBooleanInfo) TryQueryElapsed( ref decimal value, string format, params object[] args )
    {
        return this.TryQueryElapsed( ref value, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    #endregion

    #region " double query and parse "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns> The presentValue. </returns>
    public double? Query( double? value, string queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
            value = this.Query( value.GetValueOrDefault( default ), queryCommand );
        return value;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The parsed value or default. </returns>
    public double Query( double emulatingValue, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return emulatingValue;
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseDouble( this.QueryTrimEnd( dataToWrite ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The format of the data to write. </param>
    /// <param name="args">             The format arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public double? Query( double emulatingValue, string format, params object[] args )
    {
        return this.Query( emulatingValue, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
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
            ? this.QueryElapsed( value.GetValueOrDefault( default ), queryCommand )
            : QueryParseInfo<double>.Empty;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   A Tuple:  (double ParsedValue, <see cref="QueryParseInfo{T}"/> )  </returns>
    public QueryParseInfo<double> QueryElapsed( double emulatingValue, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return new QueryParseInfo<double>( emulatingValue, string.Empty, QueryInfo.Empty );
        else
        {
            this.MakeEmulatedReplyIfEmpty( emulatingValue );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters );
            double parsedValue = this.ParseDouble( parsedMessage );
            return new QueryParseInfo<double>( parsedValue, parsedMessage, queryInfo );
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The format of the data to write. </param>
    /// <param name="args">             The format arguments. </param>
    /// <returns>   A Tuple:  (double ParsedValue, <see cref="QueryParseInfo{T}"/> ) </returns>
    public QueryParseInfo<double> QueryElapsed( double emulatingValue, string format, params object[] args )
    {
        return this.QueryElapsed( emulatingValue, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   The presentValue. </returns>
    public double? Query( double? value, int receivedMessageItemIndex, string queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
            value = this.Query( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand );
        return value;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">           The value to emulate if emulated reply is empty. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="dataToWrite">              The data to write. </param>
    /// <returns>   The second. </returns>
    public double Query( double emulatingValue, int receivedMessageItemIndex, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return emulatingValue;

        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        dataToWrite = this.QueryTrimEnd( dataToWrite ) ?? $"{emulatingValue}";
        return this.ParseDouble( dataToWrite.Split( ',' )[receivedMessageItemIndex] );
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
            ? this.QueryElapsed( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand )
            : QueryParseInfo<double>.Empty;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">           The value to emulate if emulated reply is empty. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="dataToWrite">              The data to write. </param>
    /// <returns>   A Tuple:  (double ParsedValue, <see cref="QueryParseInfo{T}"/> )  </returns>
    public QueryParseInfo<double> QueryElapsed( double emulatingValue, int receivedMessageItemIndex, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return new QueryParseInfo<double>( emulatingValue, string.Empty, QueryInfo.Empty );
        else
        {
            this.MakeEmulatedReplyIfEmpty( emulatingValue );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters ).Split( ',' )[receivedMessageItemIndex];
            double parsedValue = this.ParseDouble( parsedMessage );
            return new QueryParseInfo<double>( parsedValue, parsedMessage, queryInfo );
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">       [in,out] The value. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQuery( ref double value, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return false;

        this.MakeEmulatedReplyIfEmpty( value );
        return SessionBase.TryParse( this.QueryTrimEnd( dataToWrite ), out value );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">  [in,out] The value. </param>
    /// <param name="format"> The format of the data to write. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQuery( ref double value, string format, params object[] args )
    {
        return this.TryQuery( ref value, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">        [in,out] The value. </param>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   A Tuple:  (bool Success, <see cref="QueryParseInfo{T}"/> )  </returns>
    public (bool Success, QueryParseInfo<double> QueryParseBooleanInfo) TryQueryElapsed( ref double value, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return (false, new QueryParseInfo<double>( value, string.Empty, QueryInfo.Empty ));
        else
        {
            this.MakeEmulatedReplyIfEmpty( value );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters );
            bool success = SessionBase.TryParse( parsedMessage, out double parsedValue );
            return (success, new QueryParseInfo<double>( parsedValue, parsedMessage, queryInfo ));
        }

    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">    [in,out] The value. </param>
    /// <param name="format">   The format of the data to write. </param>
    /// <param name="args">     The format arguments. </param>
    /// <returns>   A Tuple:  (bool Success, <see cref="QueryParseInfo{T}"/> )  </returns>
    public (bool Success, QueryParseInfo<double> QueryParseBooleanInfo) TryQueryElapsed( ref double value, string format, params object[] args )
    {
        return this.TryQueryElapsed( ref value, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary> Write the present value without reading back the present value from the device. </summary>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <returns> The present value. </returns>
    public double WriteLine( double value, string commandFormat )
    {
        if ( !string.IsNullOrWhiteSpace( commandFormat ) )
        {
            if ( value >= VI.Syntax.ScpiSyntax.Infinity - 1d )
            {
                _ = this.WriteLine( commandFormat, "MAX" );
                value = VI.Syntax.ScpiSyntax.Infinity;
            }
            else if ( value <= VI.Syntax.ScpiSyntax.NegativeInfinity + 1d )
            {
                _ = this.WriteLine( commandFormat, "MIN" );
                value = VI.Syntax.ScpiSyntax.NegativeInfinity;
            }
            else
            {
                _ = this.WriteLine( commandFormat, value );
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
                sentMessage = this.WriteLine( commandFormat, "MAX" );
                value = VI.Syntax.ScpiSyntax.Infinity;
            }
            else if ( value <= VI.Syntax.ScpiSyntax.NegativeInfinity + 1d )
            {
                sentMessage = this.WriteLine( commandFormat, "MIN" );
                value = VI.Syntax.ScpiSyntax.NegativeInfinity;
            }
            else
            {
                sentMessage = this.WriteLine( commandFormat, value );
            }
        }
        return (value, new WriteInfo<double>( value, commandFormat, sentMessage, sw.Elapsed ));
    }

    #endregion

    #region " int query and parse "

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">        The present value to return if the command is null or empty or white space. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns> The presentValue. </returns>
    public int? Query( int? value, string queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
            value = this.Query( value.GetValueOrDefault( default ), queryCommand );
        return value;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The parsed value or default. </returns>
    public int Query( int emulatingValue, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return emulatingValue;
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseInteger( this.QueryTrimEnd( dataToWrite ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The format of the data to write. </param>
    /// <param name="args">             The format arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public int? Query( int emulatingValue, string format, params object[] args )
    {
        return this.Query( emulatingValue, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
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
            ? this.QueryElapsed( value.GetValueOrDefault( default ), queryCommand )
            : QueryParseInfo<int>.Empty;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   A Tuple:  (int ParsedValue, <see cref="QueryParseInfo{T}"/> )  </returns>
    public QueryParseInfo<int> QueryElapsed( int emulatingValue, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return new QueryParseInfo<int>( emulatingValue, string.Empty, QueryInfo.Empty );
        else
        {
            this.MakeEmulatedReplyIfEmpty( emulatingValue );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters );
            int parsedValue = this.ParseInteger( parsedMessage );
            return new QueryParseInfo<int>( parsedValue, parsedMessage, queryInfo );
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The format of the data to write. </param>
    /// <param name="args">             The format arguments. </param>
    /// <returns>   A Tuple:  (int ParsedValue, <see cref="QueryParseInfo{T}"/> ) </returns>
    public QueryParseInfo<int> QueryElapsed( int emulatingValue, string format, params object[] args )
    {
        return this.QueryElapsed( emulatingValue, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   The presentValue. </returns>
    public int? Query( int? value, int receivedMessageItemIndex, string queryCommand )
    {
        if ( !string.IsNullOrWhiteSpace( queryCommand ) )
            value = this.Query( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand );
        return value;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">           The value to emulate if emulated reply is empty. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="dataToWrite">              The data to write. </param>
    /// <returns>   The second. </returns>
    public int Query( int emulatingValue, int receivedMessageItemIndex, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return emulatingValue;

        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        dataToWrite = this.QueryTrimEnd( dataToWrite ) ?? $"{emulatingValue}";
        return this.ParseInteger( dataToWrite.Split( ',' )[receivedMessageItemIndex] );
    }

    /// <summary>   Queries and parses the indexed presentValue from the instrument. </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">                    The present value to return if the command is null or empty or white space. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="queryCommand">             The query dataToWrite. </param>
    /// <returns>   <see cref="QueryParseInfo{T}"/> </returns>
    public QueryParseInfo<int> QueryElapsed( int? value, int receivedMessageItemIndex, string queryCommand )
    {
        return !string.IsNullOrWhiteSpace( queryCommand )
            ? this.QueryElapsed( value.GetValueOrDefault( default ), receivedMessageItemIndex, queryCommand )
            : QueryParseInfo<int>.Empty;
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the item identified by the item index from a comma-separated received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="emulatingValue">           The value to emulate if emulated reply is empty. </param>
    /// <param name="receivedMessageItemIndex"> Zero-based index of the received message item. </param>
    /// <param name="dataToWrite">              The data to write. </param>
    /// <returns>   A Tuple:  (int ParsedValue, <see cref="QueryParseInfo{T}"/> )  </returns>
    public QueryParseInfo<int> QueryElapsed( int emulatingValue, int receivedMessageItemIndex, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return new QueryParseInfo<int>( emulatingValue, string.Empty, QueryInfo.Empty );
        else
        {
            this.MakeEmulatedReplyIfEmpty( emulatingValue );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters ).Split( ',' )[receivedMessageItemIndex];
            int parsedValue = this.ParseInteger( parsedMessage );
            return new QueryParseInfo<int>( parsedValue, parsedMessage, queryInfo );
        }
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">       [in,out] The value. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQuery( ref int value, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) return false;

        this.MakeEmulatedReplyIfEmpty( value );
        return SessionBase.TryParse( this.QueryTrimEnd( dataToWrite ), out value );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <param name="value">  [in,out] The value. </param>
    /// <param name="format"> The format of the data to write. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQuery( ref int value, string format, params object[] args )
    {
        return this.TryQuery( ref value, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">        [in,out] The value. </param>
    /// <param name="dataToWrite">  The data to write. </param>
    /// <returns>   A Tuple:  (bool Success, <see cref="QueryParseInfo{T}"/> )  </returns>
    public (bool Success, QueryParseInfo<int> QueryParseBooleanInfo) TryQueryElapsed( ref int value, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( dataToWrite ) )
            return (false, new QueryParseInfo<int>( value, string.Empty, QueryInfo.Empty ));
        else
        {
            this.MakeEmulatedReplyIfEmpty( value );
            QueryInfo queryInfo = this.QueryElapsed( dataToWrite );
            string parsedMessage = queryInfo.ReceivedMessage.TrimEnd( this._terminationCharacters );
            bool success = SessionBase.TryParse( parsedMessage, out int parsedValue );
            return (success, new QueryParseInfo<int>( parsedValue, parsedMessage, queryInfo ));
        }

    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read and
    /// parses the received message.
    /// </summary>
    /// <remarks>   David, 2021-04-10. </remarks>
    /// <param name="value">    [in,out] The value. </param>
    /// <param name="format">   The format of the data to write. </param>
    /// <param name="args">     The format arguments. </param>
    /// <returns>   A Tuple:  (bool Success, <see cref="QueryParseInfo{T}"/> )  </returns>
    public (bool Success, QueryParseInfo<int> QueryParseBooleanInfo) TryQueryElapsed( ref int value, string format, params object[] args )
    {
        return this.TryQueryElapsed( ref value, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    #endregion

    #region " time span query and parse "

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
            : this.Query( timespanFormat, dataToWrite );
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
            : this.Query( timespanFormat, dataToWrite );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// </summary>
    /// <remarks>   see also: https://msdn.Microsoft.com/en-us/library/ee372287.aspx#Other. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="timespanFormat">   The format for parsing the result. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The parsed value. </returns>
    public TimeSpan Query( string timespanFormat, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( timespanFormat ) ) throw new ArgumentNullException( nameof( timespanFormat ) );
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        this.MakeEmulatedReplyIfEmpty( TimeSpan.Zero );
        return TimeSpan.ParseExact( this.QueryTrimEnd( dataToWrite ), timespanFormat, System.Globalization.CultureInfo.InvariantCulture );
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// Parses the time span return value.
    /// </summary>
    /// <remarks>   2024-07-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="result">           [in,out] The result. </param>
    /// <param name="timespanFormat">   The format for parsing the result. For example, "s\.FFFFFFF",
    ///                                 convert the value to time span from seconds. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   <c>true</c> if the parsed value is valid. </returns>
    public bool TryQuery( ref TimeSpan result, string timespanFormat, string dataToWrite )
    {
        if ( string.IsNullOrWhiteSpace( timespanFormat ) ) throw new ArgumentNullException( nameof( timespanFormat ) );
        if ( string.IsNullOrWhiteSpace( dataToWrite ) ) throw new ArgumentNullException( nameof( dataToWrite ) );
        this.MakeEmulatedReplyIfEmpty( result );
        return TimeSpan.TryParseExact( this.QueryTrimEnd( dataToWrite ), timespanFormat, System.Globalization.CultureInfo.InvariantCulture, out result );
    }

    /// <summary>
    /// Writes the <see cref="TimeSpan">TimeSpan</see> value building the data to write using the
    /// given <paramref name="commandFormat"/> returning the provided value if the <paramref name="commandFormat"/>
    /// is empty.
    /// </summary>
    /// <param name="value">         The present value to return if the command is null or empty or white space. </param>
    /// <param name="commandFormat"> The command format. </param>
    /// <returns> The present value. </returns>
    public string WriteLine( TimeSpan value, string commandFormat )
    {
        return string.IsNullOrWhiteSpace( commandFormat )
            ? value.ToString()
            : this.WriteLine( commandFormat, value );
    }

    #endregion

    #region " enum read write parser "

    /// <summary>
    /// Queries first enum value returning the <paramref name="emulatingValue"/> if the
    /// <paramref name="dataToWrite"/> is empty.
    /// </summary>
    /// <remarks>   2024-07-05. </remarks>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="emulatingValue">   The emulating value. </param>
    /// <param name="enumReadWrites">   enumeration read and write values. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The first enum value. </returns>
    public T QueryFirst<T>( T emulatingValue, EnumReadWriteCollection enumReadWrites, string dataToWrite ) where T : struct, Enum
    {
        long v = this.QueryFirst( Convert.ToInt64( emulatingValue ), enumReadWrites, dataToWrite );
        // return v.ToNullableEnum<T>().GetValueOrDefault( value );
        return v.ToEnum<T>();
    }

    /// <summary> Queries first enum value returning the <paramref name="emulatingValue"/> if the
    ///           <paramref name="dataToWrite"/> is empty. </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="enumReadWrites">   enumeration read and write values. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The first enum value. </returns>
    public long QueryFirst( long emulatingValue, EnumReadWriteCollection enumReadWrites, string dataToWrite )
    {
        if ( enumReadWrites is null ) throw new ArgumentNullException( nameof( enumReadWrites ) );
        this.MakeEmulatedReplyIfEmpty( enumReadWrites.SelectItemOrDefault( emulatingValue ).ReadValue );
        return this.Parse( enumReadWrites, (this.QueryTrimEnd( dataToWrite ) ?? $"{emulatingValue}").Split( ',' )[0].Trim() );
    }

    /// <summary>
    /// Issues the query command and parses the returned values into An enum using the enum name.
    /// </summary>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="enumReadWrites"> enumeration read and write values. </param>
    /// <param name="dataToWrite">    The data to write. </param>
    /// <returns> The parsed value or none if unknown. </returns>
    public T Query<T>( T emulatingValue, EnumReadWriteCollection enumReadWrites, string dataToWrite ) where T : struct, Enum
    {
        long v = this.Query( Convert.ToInt64( emulatingValue ), enumReadWrites, dataToWrite );
        return v.ToEnum<T>();
    }

    /// <summary>
    /// Performs a synchronous write of ASCII-encoded string data, followed by a synchronous read.
    /// Parses the string return value to an integer using the 'parse dictionary'.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="enumReadWrites">   enumeration read and write values. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The parsed value or default. </returns>
    public long Query( long emulatingValue, EnumReadWriteCollection enumReadWrites, string dataToWrite )
    {
        if ( enumReadWrites is null ) throw new ArgumentNullException( nameof( enumReadWrites ) );
        this.MakeEmulatedReplyIfEmpty( enumReadWrites.SelectItemOrDefault( emulatingValue ).ReadValue );
        return this.Parse( enumReadWrites, this.QueryTrimEnd( dataToWrite ) ?? $"{emulatingValue}" );
    }

    /// <summary> Parses a value to Long using the 'parse dictionary'. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <exception cref="ArgumentException">     Thrown when one or more arguments have unsupported or
    /// illegal values. </exception>
    /// <exception cref="FormatException">       Thrown when the format of the ? is incorrect. </exception>
    /// <param name="enumReadWrites"> enumeration read and write values. </param>
    /// <param name="value">          The value. </param>
    /// <returns> Value if the value is a valid number; otherwise, Default. </returns>
    public long Parse( EnumReadWriteCollection enumReadWrites, string value )
    {
        long parsedValue = value is null
            ? throw new ArgumentNullException( nameof( value ), "Query not executed" )
            : enumReadWrites is null
                ? throw new ArgumentNullException( nameof( enumReadWrites ), "Parse dictionary not provided" )
                : string.IsNullOrWhiteSpace( value )
                                ? throw new ArgumentException( "Query returned an empty string", nameof( value ) )
                                : enumReadWrites.Exists( value )
                                                ? enumReadWrites.SelectItem( value ).EnumValue
                                                : throw new FormatException( $"{this.ResourceNameCaption} '{value}' not found in the parse dictionary" );
        return parsedValue;
    }

    /// <summary>   Tries to parse a value to Long using the 'parse dictionary'. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="enumReadWrites">   enumeration read and write values. </param>
    /// <param name="key">              The key. </param>
    /// <param name="value">            The value. </param>
    /// <returns>   <c>true</c> if the parsed value is valid. </returns>
    public static bool TryParse( EnumReadWriteCollection enumReadWrites, string key, out long value )
    {
        if ( !string.IsNullOrWhiteSpace( key ) && enumReadWrites is not null && enumReadWrites.Exists( key ) )
        {
            value = enumReadWrites.SelectItem( key ).EnumValue;
            return true;
        }
        else
        {
            value = 0;
            return false;
        }
    }

    /// <summary>
    /// Synchronously writes the Enum write value without reading back the value from the device.
    /// </summary>
    /// <param name="value">          The value. </param>
    /// <param name="commandFormat">  The command format for creating the data to write. </param>
    /// <param name="enumReadWrites"> enumeration read and write values. </param>
    /// <returns> The value or none if unknown. </returns>
    public T Write<T>( T value, string commandFormat, EnumReadWriteCollection enumReadWrites ) where T : struct, Enum
    {
        // long v = this.Session.Write( ( long ) (Enum.ToObject( type of( T ), value )), commandFormat, enumReadWrites );
        long v = this.Write( Convert.ToInt64( value ), commandFormat, enumReadWrites );
        return v.ToEnum<T>();
    }

    /// <summary>
    /// Synchronously writes the Enum write value without reading back the value from the device.
    /// </summary>
    /// <param name="value">          The value. </param>
    /// <param name="commandFormat">  The command format for creating the data to write. </param>
    /// <param name="enumCodeValues"> Dictionary of parses. </param>
    /// <returns> The value or none if unknown. </returns>
    public long Write( long value, string commandFormat, EnumReadWriteCollection enumCodeValues )
    {
        if ( !string.IsNullOrWhiteSpace( commandFormat ) && enumCodeValues is not null )
        {
            _ = this.WriteLine( commandFormat, enumCodeValues.SelectItem( value ).WriteValue );
        }

        return value;
    }

    #endregion

    #region " enum query and parse "

    /// <summary> Parse enum value. </summary>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    /// type. </exception>
    /// <param name="value"> The value. </param>
    /// <returns> The parsed enum value. </returns>
    public static T ParseEnumValue<T>( string value ) where T : struct
    {
        return Enum.TryParse( value, out T result )
                ? result
                : throw new InvalidCastException( $"Can't convert {value} to {typeof( T )}" );
    }

    /// <summary> Queries first enum value. </summary>
    /// <param name="value">       The value. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> The first enum value. </returns>
    public T QueryFirstEnumValue<T>( T value, string dataToWrite ) where T : struct, Enum
    {
        string currentValue = value.ToString();
        this.MakeEmulatedReplyIfEmpty( currentValue );
        if ( !string.IsNullOrWhiteSpace( dataToWrite ) )
        {
            _ = this.WriteLine( dataToWrite );
            string? s = this.ReadLineTrimEnd();
            if ( !string.IsNullOrWhiteSpace( s ) )
                currentValue = s!.Split( ',' )[0].Trim();
        }

        return ParseEnumValue<T>( currentValue );
    }

    /// <summary>
    /// Issues the query command and parses the returned values into An enum using the enum value.
    /// </summary>
    /// <param name="value">       The value. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> The parsed value or none if unknown. </returns>
    public T QueryEnumValue<T>( T value, string dataToWrite ) where T : struct
    {
        string? currentValue = value.ToString();
        this.MakeEmulatedReplyIfEmpty( currentValue );
        if ( !string.IsNullOrWhiteSpace( dataToWrite ) )
        {
            _ = this.WriteLine( dataToWrite );
            currentValue = this.ReadLineTrimEnd();
        }

        if ( currentValue is not null )
            return ParseEnumValue<T>( currentValue );
        else
            return default;
    }

    /// <summary>   Parse contained. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    ///                                         type. </exception>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="value">    The value. </param>
    /// <returns>   A T. </returns>
    public static T ParseContained<T>( string value )
    {
        Type enumType = typeof( T );
        foreach ( FieldInfo fi in enumType.GetFields() )
        {
            if ( string.Compare( fi.Name, value, true, System.Globalization.CultureInfo.CurrentCulture ) == 0 )
            {
                return ( T ) fi.GetValue( null );
            }
            else
            {
                object[] fieldAttributes = fi.GetCustomAttributes( typeof( DescriptionAttribute ), false );
                foreach ( DescriptionAttribute attr in fieldAttributes.Cast<DescriptionAttribute>() )
                {
                    if ( attr.Description.Contains( value ) )
                    {
                        return ( T ) fi.GetValue( null );
                    }
                }
            }
        }

        throw new InvalidCastException( $"Can't convert {value} to {enumType}" );
    }

    /// <summary>
    /// Issues the query command and parses the returned values into An enum using the enum name.
    /// </summary>
    /// <param name="value">       The value. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> The parsed value or none if unknown. </returns>
    public T QueryEnum<T>( T value, string dataToWrite ) where T : struct
    {
        string? currentValue = value.ToString();
        this.MakeEmulatedReplyIfEmpty( currentValue );
        if ( !string.IsNullOrWhiteSpace( dataToWrite ) )
        {
            _ = this.WriteLine( dataToWrite );
            currentValue = this.ReadLineTrimEnd();
        }

        return string.IsNullOrWhiteSpace( currentValue )
            ? value
            : Std.ParseExtensions.ParseExtensionMethods.IsNumber( currentValue! )
                ? ( T ) ( object ) int.Parse( currentValue )
                : SessionBase.ParseContained<T>( currentValue!.BuildDelimitedValue() );
    }

    /// <summary>
    /// Issues the query command and parses the returned values into An enum using the enum name.
    /// </summary>
    /// <param name="value">       The value. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> The parsed value or none if unknown. </returns>
    public T? QueryEnum<T>( T? value, string dataToWrite ) where T : struct, Enum
    {
        string? currentValue = value.ToString();
        this.MakeEmulatedReplyIfEmpty( currentValue );
        if ( !string.IsNullOrWhiteSpace( dataToWrite ) )
        {
            _ = this.WriteLine( dataToWrite );
            currentValue = this.ReadLineTrimEnd();
        }

        return string.IsNullOrWhiteSpace( currentValue )
            ? value
            : Std.ParseExtensions.ParseExtensionMethods.IsNumber( currentValue! )
                ? ( T ) ( object ) int.Parse( currentValue )
                : SessionBase.ParseContained<T>( currentValue!.BuildDelimitedValue() );
    }

    /// <summary>   Convert this object into a string representation. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="enumValue">    The enum value. </param>
    /// <returns>   EnumValue as a string. </returns>
    public static string ToString<T>( T enumValue )
    {
        if ( enumValue is null ) throw new ArgumentNullException( nameof( enumValue ) );
        Type enumType = typeof( T );
        FieldInfo fi = enumType.GetField( enumValue.ToString() );

        // Get the Description attribute that has been applied to this enum
        object[] fieldAttributes = fi.GetCustomAttributes( typeof( DescriptionAttribute ), false );
        if ( fieldAttributes.Length > 0 )
        {
            if ( fieldAttributes[0] is DescriptionAttribute descAttr )
            {
                return descAttr.Description;
            }
        }
        // Enum does not have Description attribute so we return default string representation.
        return enumValue.ToString();
    }

    /// <summary>
    /// Synchronously writes the Enum name without reading back the value from the device.
    /// </summary>
    /// <param name="value">         The value. </param>
    /// <param name="commandFormat"> The command format for creating the data to write. </param>
    /// <returns> The value or none if unknown. </returns>
    public T WriteEnum<T>( T value, string commandFormat ) where T : struct
    {
        if ( string.IsNullOrWhiteSpace( commandFormat ) ) throw new ArgumentNullException( nameof( commandFormat ) );
        _ = this.WriteLine( commandFormat, SessionBase.ToString<T>( value ).ExtractBetween() );
        return value;
    }

    /// <summary>
    /// Synchronously writes the Enum value without reading back the value from the device.
    /// </summary>
    /// <param name="value">         The value. </param>
    /// <param name="commandFormat"> The command format for creating the data to write. </param>
    /// <returns> The value or none if unknown. </returns>
    public T WriteEnumValue<T>( T value, string commandFormat ) where T : struct
    {
        if ( string.IsNullOrWhiteSpace( commandFormat ) ) throw new ArgumentNullException( nameof( commandFormat ) );

        int v = Convert.ToInt32( value );
        // int v = ( int ) (Enum.ToObject( type of( T ), value ));
        _ = this.WriteLine( commandFormat, v );

        return value;
    }

    #endregion

    #region " string "

    /// <summary>
    /// Queries a <see cref="string"/> returning The present value if the query dataToWrite is empty.
    /// </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="presentValue"> The present Value. </param>
    /// <param name="queryCommand"> The query dataToWrite. </param>
    /// <returns>   The present value. </returns>
    public string? QueryTrimEnd( string? presentValue, string queryCommand )
    {
        return string.IsNullOrWhiteSpace( queryCommand ) ? presentValue : this.QueryTrimEnd( queryCommand );
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
            : this.QueryElapsed( queryCommand );
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
            : this.QueryElapsed( string.Format( System.Globalization.CultureInfo.InvariantCulture, commandFormat, args ) );
    }

    #endregion

}

