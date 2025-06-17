using cc.isr.VI.Pith.ExceptionExtensions;
using cc.isr.Std.TimeSpanExtensions;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " query print "

    /// <summary> Gets or sets the print command commandFormat. </summary>
    /// <value> The print command commandFormat. </value>
    private string PrintCommandFormat { get; set; } = "_G.print({0})";

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read.
    /// </summary>
    /// <param name="dataToPrint"> The LUA command to print. </param>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters()">termination characters</see>.
    /// </returns>
    public string? QueryPrintTrimEnd( string dataToPrint )
    {
        return this.QueryTrimEnd( string.Format( this.PrintCommandFormat, dataToPrint ) );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read.
    /// </summary>
    /// <param name="format"> The commandFormat for building the LUA command to be printed. </param>
    /// <param name="args">   The commandFormat arguments. </param>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters()">termination characters</see>.
    /// </returns>
    public string? QueryPrintTrimEnd( string format, params object[] args )
    {
        return this.QueryTrimEnd( string.Format( this.PrintCommandFormat, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) ) );
    }

    #endregion

    #region " query print and parse "

    #region " boolean "

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Boolean return value.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The commandFormat for building the LUA command to be printed. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public bool QueryPrint( bool emulatingValue, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseBoolean( this.QueryPrintStringFormatTrimEnd( 1, format, args ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Boolean return value.
    /// </summary>
    /// <param name="value">  [in,out] Value read from the instrument. </param>
    /// <param name="format"> The commandFormat for building the LUA command to be printed. </param>
    /// <param name="args">   The commandFormat arguments. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref bool value, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintStringFormatTrimEnd( 1, format, args ) ?? $"{value}", out value );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Boolean return value.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToPrint">      The data to print. </param>
    /// <returns>   The parsed value or default. </returns>
    public bool QueryPrint( bool emulatingValue, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseBoolean( this.QueryPrintStringFormatTrimEnd( 1, dataToPrint ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Boolean return value.
    /// </summary>
    /// <param name="value">       [in,out] Value read from the instrument. </param>
    /// <param name="dataToPrint"> The data to print. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref bool value, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintStringFormatTrimEnd( 1, dataToPrint ) ?? $"{value}", out value );
    }

    #endregion

    #region " double "

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Double return value.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The commandFormat for building the LUA command to be printed. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public double QueryPrint( double emulatingValue, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseDouble( this.QueryPrintTrimEnd( format, args ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Double return value.
    /// </summary>
    /// <param name="value">  [in,out] Value read from the instrument. </param>
    /// <param name="format"> The commandFormat for building the LUA command to be printed. </param>
    /// <param name="args">   The commandFormat arguments. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref double value, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintTrimEnd( format, args ) ?? $"{value}", out value );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Double return value.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="dataToPrint">      The data to print. </param>
    /// <returns>   The parsed value or default. </returns>
    public double QueryPrint( double emulatingValue, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseDouble( this.QueryPrintTrimEnd( dataToPrint ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Double return value.
    /// </summary>
    /// <param name="value">       [in,out] Value read from the instrument. </param>
    /// <param name="dataToPrint"> The data to print. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref double value, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintTrimEnd( dataToPrint ) ?? $"{value}", out value );
    }

    #endregion

    #region " integer "

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Integer return value.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="format">           The commandFormat for building the LUA command to be printed. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public int QueryPrint( int emulatingValue, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseInteger( this.QueryPrintTrimEnd( format, args ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Integer return value.
    /// </summary>
    /// <param name="value">  [in,out] Value read from the instrument. </param>
    /// <param name="format"> The commandFormat of the data to write. </param>
    /// <param name="args">   The commandFormat arguments. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref int value, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintTrimEnd( format, args ) ?? $"{value}", out value );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Integer return value.
    /// </summary>
    /// <param name="dataToPrint"> The data to print. </param>
    /// <returns> The parsed value or default. </returns>
    public int QueryPrintInt( string dataToPrint )
    {
        string? s = this.QueryPrintTrimEnd( dataToPrint );
        return s is null
            ? throw new NativeException( $"Querying {dataToPrint} returned nothing." )
            : this.ParseInteger( s );
    }

    /// <summary>
    /// Performs a synchronous write of a LUA print of the data to print, followed by a synchronous
    /// read. Parses the Integer return value.
    /// </summary>
    /// <param name="value">       [in,out] Value read from the instrument. </param>
    /// <param name="dataToPrint"> The data to print. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref int value, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        string? s = this.QueryPrintTrimEnd( dataToPrint );
        return s is null
            ? throw new NativeException( $"Querying {dataToPrint} returned nothing." )
            : TryParse( s, out value );
    }

    #endregion

    #endregion

    #region " query print string "

    /// <summary> Gets or sets the query print string commandFormat command. </summary>
    /// <value> The query print string commandFormat command. </value>
    public string PrintCommandStringFormat { get; set; } = "_G.print(string.format('{0}',{1}))";

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. The commandFormat conforms to the 'C' query command and returns the Boolean outcome.
    /// </summary>
    /// <remarks>
    /// The commandFormat string follows the same rules as the print commandFormat family of standard C functions. The
    /// only differences are that the options or modifiers *, l, L, n, p, and h are not supported and
    /// that there is an extra option, q. The q option formats a string in a form suitable to be
    /// safely read back by the Lua interpreter: the string is written between double quotes, and all
    /// double quotes, newlines, embedded zeros, and backslashes in the string are correctly escaped
    /// when written. For instance, the call string.format('%q', 'a string with ''quotes'' and [BS]n
    /// new line') will produce the string: a string with [BS]''quotes[BS]'' and [BS]new line The
    /// options c, d, E, e, f, g, G, i, o, u, X, and x all expect a number as argument, whereas q and
    /// s expect a string. This function does not accept string values containing embedded zeros.
    /// </remarks>
    /// <param name="format"> The LUA number Format, e.g., %d or %9.6f, or %5.3e of the data top be printed. </param>
    /// <param name="args">   The commandFormat arguments. </param>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters()">termination characters</see>.
    /// </returns>
    public string? QueryPrintStringFormatTrimEnd( string format, params string[] args )
    {
        return this.QueryTrimEnd( string.Format( this.PrintCommandStringFormat, format, Parameterize( args ) ) );
    }

    /// <summary> Gets or sets the print command string number commandFormat. </summary>
    /// <value> The print command string number commandFormat. </value>
    public string PrintCommandStringNumberFormat { get; set; } = "_G.print(string.format('%{0}f',{1}))";

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. The commandFormat conforms to the 'C' query command and returns the Boolean outcome.
    /// </summary>
    /// <param name="numberFormat"> Number of formats. </param>
    /// <param name="dataToPrint">  The data to print. </param>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters()">termination characters</see>.
    /// </returns>
    public string? QueryPrintStringFormatTrimEnd( decimal numberFormat, string dataToPrint )
    {
        return this.QueryTrimEnd( string.Format( this.PrintCommandStringNumberFormat, numberFormat, dataToPrint ) );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. The commandFormat conforms to the 'C' query command and returns the Boolean outcome.
    /// </summary>
    /// <param name="numberFormat"> Number of formats. </param>
    /// <param name="format">       The LUA commandFormat of the data top be printed. </param>
    /// <param name="args">         The commandFormat arguments. </param>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters()">termination characters</see>.
    /// </returns>
    public string? QueryPrintStringFormatTrimEnd( decimal numberFormat, string format, params object[] args )
    {
        return this.QueryPrintStringFormatTrimEnd( numberFormat, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary> Gets or sets the query print string integer commandFormat command. </summary>
    /// <value> The query print string integer commandFormat command. </value>
    public string PrintCommandStringIntegerFormat { get; set; } = "_G.print(string.format('%d',{1}))";

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. The commandFormat conforms to the 'C' query command and returns the Boolean outcome.
    /// </summary>
    /// <param name="numberFormat"> The number commandFormat for integer type. </param>
    /// <param name="dataToPrint">  The data to print. </param>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters()">termination characters</see>.
    /// </returns>
    public string? QueryPrintStringFormatTrimEnd( int numberFormat, string dataToPrint )
    {
        return this.QueryTrimEnd( string.Format( this.PrintCommandStringIntegerFormat, numberFormat, dataToPrint ) );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. The commandFormat conforms to the 'C' query command and returns the Boolean outcome.
    /// </summary>
    /// <param name="numberFormat"> The number commandFormat for integer type. </param>
    /// <param name="format">       The LUA commandFormat of the data top be printed. </param>
    /// <param name="args">         The commandFormat arguments. </param>
    /// <returns>
    /// The received message without the <see cref="TerminationCharacters()">termination characters</see>.
    /// </returns>
    public string? QueryPrintStringFormatTrimEnd( int numberFormat, string format, params object[] args )
    {
        return this.QueryPrintStringFormatTrimEnd( numberFormat, string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args ) );
    }

    /// <summary>
    /// Returns a string from the parameter array of arguments for use when running the function.
    /// </summary>
    /// <param name="args"> Specifies a parameter array of arguments. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string Parameterize( params string[] args )
    {
        System.Text.StringBuilder arguments = new();
        int i;
        if ( args is not null && args.Length >= 0 )
        {
            int loopTo = args.Length - 1;
            for ( i = 0; i <= loopTo; i++ )
            {
                if ( i > 0 )
                {
                    _ = arguments.Append( "," );
                }

                _ = arguments.Append( args[i] );
            }
        }

        return arguments.ToString();
    }

    #endregion

    #region " query print string and parse "

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Decimal.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="numberFormat">     The number commandFormat for the numeric value to use in the print
    ///                                 string commandFormat statement. </param>
    /// <param name="dataToPrint">      The data to print. </param>
    /// <returns>   The parsed value or default. </returns>
    public decimal QueryPrint( decimal emulatingValue, decimal numberFormat, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        string? s = this.QueryPrintStringFormatTrimEnd( numberFormat, dataToPrint );
        return s is null ? emulatingValue : this.ParseDecimal( s );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Decimal.
    /// </summary>
    /// <param name="numberFormat"> The number commandFormat for the numeric value to use in the print
    /// string commandFormat statement. </param>
    /// <param name="value">        [in,out] Value read from the instrument. </param>
    /// <param name="dataToPrint">  The data to print. </param>
    /// <returns> True returned value is valid. </returns>
    public bool TryQueryPrint( decimal numberFormat, ref decimal value, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        string? s = this.QueryPrintStringFormatTrimEnd( numberFormat, dataToPrint );
        if ( s is null )
        {
            value = decimal.Zero;
            return false;
        }
        else
            return TryParse( s, out value );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Decimal.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="numberFormat">     The number commandFormat for the numeric value to use in the print
    ///                                 string commandFormat statement. </param>
    /// <param name="format">           The commandFormat for constructing the data to write. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public decimal QueryPrint( decimal emulatingValue, decimal numberFormat, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseDecimal( this.QueryPrintStringFormatTrimEnd( numberFormat, format, args ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Decimal.
    /// </summary>
    /// <param name="numberFormat"> The number commandFormat for the numeric value to use in the print
    /// string commandFormat statement. </param>
    /// <param name="value">        [in,out] Value read from the instrument. </param>
    /// <param name="format">       The commandFormat for constructing the data to write. </param>
    /// <param name="args">         The commandFormat arguments. </param>
    /// <returns> True returned value is valid. </returns>
    public bool TryQueryPrint( decimal numberFormat, ref decimal value, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintStringFormatTrimEnd( numberFormat, format, args ) ?? $"{value}", out value );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Double.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="numberFormat">     The number commandFormat for the numeric value to use in the print
    ///                                 string commandFormat statement. </param>
    /// <param name="dataToPrint">      The data to print. </param>
    /// <returns>   The parsed value or default. </returns>
    public double QueryPrint( double emulatingValue, decimal numberFormat, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseDouble( this.QueryPrintStringFormatTrimEnd( numberFormat, dataToPrint ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Double.
    /// </summary>
    /// <param name="numberFormat"> The number commandFormat for the numeric value to use in the print
    /// string commandFormat statement. </param>
    /// <param name="value">        [in,out] Value read from the instrument. </param>
    /// <param name="dataToPrint">  The data to print. </param>
    /// <returns> True returned value is valid. </returns>
    public bool TryQueryPrint( decimal numberFormat, ref double value, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintStringFormatTrimEnd( numberFormat, dataToPrint ) ?? $"{value}", out value );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Double.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="numberFormat">     The number commandFormat for the numeric value to use in the print
    ///                                 string commandFormat statement. </param>
    /// <param name="format">           The commandFormat for constructing the data to write. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public double QueryPrint( double emulatingValue, decimal numberFormat, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseDouble( this.QueryPrintStringFormatTrimEnd( numberFormat, format, args ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Double.
    /// </summary>
    /// <param name="numberFormat"> The number commandFormat for the numeric value to use in the print
    /// string commandFormat statement. </param>
    /// <param name="value">        [in,out] Value read from the instrument. </param>
    /// <param name="format">       The commandFormat for constructing the data to write. </param>
    /// <param name="args">         The commandFormat arguments. </param>
    /// <returns> True returned value is valid. </returns>
    public bool TryQueryPrint( decimal numberFormat, ref double value, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintStringFormatTrimEnd( numberFormat, format, args ) ?? $"{value}", out value );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Integer.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="numberFormat">     The number commandFormat for the numeric value to use in the print
    ///                                 string commandFormat statement. </param>
    /// <param name="dataToPrint">      The data to print. </param>
    /// <returns>   The parsed value or default. </returns>
    public int QueryPrint( int emulatingValue, int numberFormat, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseInteger( this.QueryPrintStringFormatTrimEnd( numberFormat, dataToPrint ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to integer.
    /// </summary>
    /// <param name="numberFormat"> The number commandFormat for the numeric value to use in the print
    /// string commandFormat statement. </param>
    /// <param name="value">        [in,out] Value read from the instrument. </param>
    /// <param name="dataToPrint">  The data to print. </param>
    /// <returns> True returned value is valid. </returns>
    public bool TryQueryPrint( int numberFormat, ref int value, string dataToPrint )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintStringFormatTrimEnd( numberFormat, dataToPrint ) ?? $"{value}", out value );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to Integer.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="numberFormat">     The number commandFormat for the numeric value to use in the print
    ///                                 string commandFormat statement. </param>
    /// <param name="format">           The commandFormat for constructing the data to write. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public int QueryPrint( int emulatingValue, int numberFormat, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        return this.ParseInteger( this.QueryPrintStringFormatTrimEnd( numberFormat, format, args ) ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Performs a synchronous write of a Lua print string commandFormat command, followed by a synchronous
    /// read. Parses the reading to integer.
    /// </summary>
    /// <param name="numberFormat"> The number commandFormat for the numeric value to use in the print
    /// string commandFormat statement. </param>
    /// <param name="value">        [in,out] Value read from the instrument. </param>
    /// <param name="format">       The commandFormat for constructing the data to write. </param>
    /// <param name="args">         The commandFormat arguments. </param>
    /// <returns> True returned value is valid. </returns>
    public bool TryQueryPrint( int numberFormat, ref int value, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( value );
        return TryParse( this.QueryPrintStringFormatTrimEnd( numberFormat, format, args ) ?? $"{value}", out value );
    }

    #endregion

    #region " true "

    /// <summary> Represents the LUA true value. </summary>
    public const string TrueValue = "true";

    /// <summary> Equals True. </summary>
    /// <param name="value"> Specifies the global or statement to evaluate. </param>
    /// <returns> <c>true</c> if equals True; otherwise <c>false</c> </returns>
    public static bool EqualsTrue( string? value )
    {
        return string.IsNullOrWhiteSpace( value ) || string.Equals( TrueValue, value, StringComparison.Ordinal );
    }

    /// <summary> Returns <c>true</c> if the specified global or statement is True. </summary>
    /// <param name="value"> Specifies the global or statement to evaluate. </param>
    /// <returns> <c>true</c> if the specified statement or global is true; otherwise <c>false</c> </returns>
    public bool IsTrue( string value )
    {
        return SessionBase.EqualsTrue( this.QueryPrintTrimEnd( value ) );
    }

    /// <summary>   Returns <c>true</c> if the specified global or statement is True. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <param name="format">   The commandFormat for building the LUA command to be printed. </param>
    /// <param name="args">     The commandFormat arguments. </param>
    /// <returns>
    /// <c>true</c> if the specified statement or global is true; otherwise <c>false</c>
    /// </returns>
    public bool IsTrue( string format, params object[] args )
    {
        return SessionBase.EqualsTrue( this.QueryPrintTrimEnd( format, args ) );
    }

    #endregion

    #region " false "

    /// <summary> Represents the LUA false value. </summary>
    public const string FalseValue = "false";

    /// <summary> Equals False. </summary>
    /// <param name="value"> Specifies the global or statement to evaluate. </param>
    /// <returns> <c>true</c> if equals False; otherwise <c>false</c> </returns>
    public static bool EqualsFalse( string? value )
    {
        return string.IsNullOrWhiteSpace( value ) || string.Equals( FalseValue, value, StringComparison.Ordinal );
    }

    /// <summary> Returns <c>false</c> if the specified global or statement is False. </summary>
    /// <param name="value"> Specifies the global or statement to evaluate. </param>
    /// <returns> <c>true</c> if the specified statement or global is false; otherwise <c>false</c> </returns>
    public bool IsFalse( string value )
    {
        return SessionBase.EqualsFalse( this.QueryPrintTrimEnd( value ) );
    }

    /// <summary>   Returns <c>false</c> if the specified global or statement is False. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <param name="format">   The commandFormat for building the LUA command to be printed. </param>
    /// <param name="args">     The commandFormat arguments. </param>
    /// <returns>
    /// <c>true</c> if the specified statement or global is false; otherwise <c>false</c>
    /// </returns>
    public bool IsFalse( string format, params object[] args )
    {
        return SessionBase.EqualsFalse( this.QueryPrintTrimEnd( format, args ) );
    }

    #endregion

    #region " nil "

    /// <summary> Represents the LUA nil value. </summary>
    public const string NilValue = "nil";

    /// <summary> Equals nil. </summary>
    /// <param name="value"> Specifies the global which to look for. </param>
    /// <returns> <c>true</c> if equals nil; otherwise <c>false</c> </returns>
    public static bool EqualsNil( string? value )
    {
        return string.IsNullOrWhiteSpace( value ) || string.Equals( NilValue, value, StringComparison.Ordinal );
    }

    /// <summary> Returns <c>true</c> if the specified global exists. </summary>
    /// <param name="value"> Specifies the global which to look for. </param>
    /// <returns> <c>true</c> if the specified global exists; otherwise <c>false</c> </returns>
    public bool IsGlobalExists( string value )
    {
        return !SessionBase.EqualsNil( this.QueryPrintTrimEnd( value ) );
    }

    /// <summary> Returns <c>true</c> if the specified global is Nil. </summary>
    /// <param name="value"> Specifies the global which to look for. </param>
    /// <returns> <c>true</c> if the specified global exists; otherwise <c>false</c> </returns>
    public bool IsNil( string value )
    {
        return SessionBase.EqualsNil( this.QueryPrintTrimEnd( value ) );
    }

    /// <summary> Returns <c>true</c> if the specified global is Nil. </summary>
    /// <param name="format"> The commandFormat for building the global. </param>
    /// <param name="args">   The commandFormat arguments. </param>
    /// <returns> <c>true</c> if the specified global exists; otherwise <c>false</c> </returns>
    public bool IsNil( string format, params object[] args )
    {
        return SessionBase.EqualsNil( this.QueryPrintTrimEnd( format, args ) );
    }

    /// <summary>
    /// Checks the series of values and return <c>true</c> if any one of them is nil.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="values"> Specifies a list of nil objects to check. </param>
    /// <returns> <c>True </c> if any value is nil; otherwise, <c>false</c> </returns>
    public bool IsNil( string[] values )
    {
        if ( values is null || values.Length == 0 )
        {
            throw new ArgumentNullException( nameof( values ) );
        }
        else
        {
            foreach ( string value in values )
            {
                if ( !string.IsNullOrWhiteSpace( value ) )
                {
                    if ( this.IsNil( value ) )
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    #endregion

    #region " tsp / lua syntax "

    /// <summary> Returns true if the statement built using the provided arguments is true. </summary>
    /// <remarks> The <paramref name="format"/> might be a function or a compound statement such as <c>a==b</c>. </remarks>
    /// <exception cref="FormatException">          Thrown when the output is neither true nor false. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="format"> The format for constructing the assertion. </param>
    /// <param name="args">   The format arguments. </param>
    /// <returns> True if statement true, false if not. </returns>
    public bool IsStatementTrue( string format, params object[] args )
    {
        bool value;
        try
        {
            string? result = this.QueryPrintTrimEnd( format, args );
            value = string.Equals( cc.isr.VI.Syntax.Tsp.Lua.TrueValue, result, StringComparison.OrdinalIgnoreCase )
                || (string.Equals( cc.isr.VI.Syntax.Tsp.Lua.FalseValue, result, StringComparison.OrdinalIgnoreCase )
                    ? false
                    : throw new FormatException(
                        $"Statement '{string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args )}' returned '{result}', which is not Boolean" ));
        }
        catch ( FormatException )
        {
            throw;
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new InvalidOperationException( $"Statement '{string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args )}' failed. Last message = '{this.LastMessageSent}'", ex );
        }

        return value;
    }

    /// <summary>   Query if the command <paramref name="command"/> command exists. </summary>
    /// <remarks>   2025-06-14. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="command">  The command. </param>
    /// <returns>   True if the command exists, false if not. </returns>
    public bool IsCommandExist( string command )
    {
        if ( string.IsNullOrWhiteSpace( command ) )
            throw new ArgumentNullException( nameof( command ) );

        string commandObject = command;
        if ( commandObject.Contains( '(' ) )
            commandObject = commandObject[..commandObject.IndexOf( '(' )];

        return !this.IsNil( commandObject );
    }

    /// <summary>
    /// Query if the command represented by <paramref name="commandFormat"/> exists and returns a <see cref="cc.isr.VI.Syntax.Tsp.Lua"/>.
    /// 
    /// <see cref="cc.isr.VI.Syntax.Tsp.Lua.TrueValue"/>.
    /// </summary>
    /// <remarks>   2025-06-14. </remarks>
    /// <param name="commandFormat">    The format for building the LUA command to be printed. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   True if command not nil and true, false if not. </returns>
    public bool IsCommandExistAndTrue( string commandFormat, params object[] args )
    {
        return this.IsCommandExist( commandFormat ) && this.IsStatementTrue( commandFormat, args );
    }

    /// <summary>   Is command true. </summary>
    /// <remarks>   2025-06-16. </remarks>
    /// <exception cref="FormatException">           Thrown when the output is neither true nor false. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when command failed to execute. </exception>
    /// <param name="commandFormat">    The format for building the LUA command to be printed. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   A bool?: null if command is not known. </returns>
    public bool? IsCommandTrue( string commandFormat, params object[] args )
    {
        return this.IsCommandExist( commandFormat )
            ? this.IsStatementTrue( commandFormat, args )
            : new bool?();
    }

    #endregion

    #region " node "

    #region " execute command - node "

    /// <summary> Gets the execute command succeeded with a wait command.  Requires node number and command arguments. </summary>
    public const string ExecuteNodeCommandWaitFormat = "_G.node[{0}].execute(\"{1}\") _G.waitcomplete()";

    /// <summary>   Executes a command on the remote node ending with wait completion. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="nodeNumber">       Specifies the node number. </param>
    /// <param name="commandFormat">    The command format for constructing the data to write. </param>
    /// <param name="args">             The command format arguments. </param>
    /// <returns>   The command message. </returns>
    public string ExecuteCommandWaitComplete( int nodeNumber, string commandFormat, params object[] args )
    {
        if ( string.IsNullOrWhiteSpace( commandFormat ) )
        {
            throw new ArgumentNullException( nameof( commandFormat ) );
        }
        else
        {
            string command = string.Format( System.Globalization.CultureInfo.CurrentCulture, commandFormat, args );
            _ = this.WriteLine( ExecuteNodeCommandWaitFormat, nodeNumber, command );
            return command;
        }
    }

    /// <summary> Gets the execute command succeeded with a wait complete query.  Requires node number and command arguments. </summary>
    public const string ExecuteNodeCommandQueryCompleteFormat = "_G.node[{0}].execute(\"{1}\") _G.waitcomplete() print('1') ";

    /// <summary>   Executes a command on the remote node ending with wait completion. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="nodeNumber">       Specifies the node number. </param>
    /// <param name="commandFormat">    The command format for constructing the data to write. </param>
    /// <param name="args">             The command format arguments. </param>
    /// <returns>   The command message. </returns>
    public string ExecuteCommandQueryComplete( int nodeNumber, string commandFormat, params object[] args )
    {
        if ( string.IsNullOrWhiteSpace( commandFormat ) )
        {
            throw new ArgumentNullException( nameof( commandFormat ) );
        }
        else
        {
            string command = string.Format( System.Globalization.CultureInfo.CurrentCulture, commandFormat, args );
            _ = this.WriteLine( ExecuteNodeCommandQueryCompleteFormat, nodeNumber, command );
            return command;
        }
    }

    #endregion

    #region " query - node "

    /// <summary> Gets the value returned by executing a command on the node.
    /// Requires node number and value to get arguments. </summary>
    public const string NodeValueGetterCommandFormat1 = "_G.node[{0}].execute('dataqueue.add({1})') _G.waitcomplete({0}) _G.waitcomplete() _G.print(_G.node[{0}].dataqueue.next())";

    /// <summary>
    /// Executes a query on the remote node and prints the result. This leaves an item in the input
    /// buffer that must be retried.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="nodeNumber">  Specifies the remote node number. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    public void ExecuteQuery( int nodeNumber, string dataToWrite )
    {
        _ = string.IsNullOrWhiteSpace( dataToWrite )
            ? throw new ArgumentNullException( nameof( dataToWrite ) )
            : this.WriteLine( NodeValueGetterCommandFormat1, nodeNumber, dataToWrite );
    }

    /// <summary>
    /// Executes a query on the remote node prints the result. This leaves an item in the input
    /// buffer that must be retried.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="nodeNumber"> Specifies the remote node number. </param>
    /// <param name="format">     The commandFormat for constructing the data to write. </param>
    /// <param name="args">       The commandFormat arguments. </param>
    public void ExecuteQuery( int nodeNumber, string format, params object[] args )
    {
        if ( string.IsNullOrWhiteSpace( format ) )
        {
            throw new ArgumentNullException( nameof( format ) );
        }
        else
        {
            string command = string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args );
            _ = this.WriteLine( NodeValueGetterCommandFormat1, nodeNumber, command );
        }
    }

    /// <summary>
    /// Executes a command on the remote node and prints the result, followed by a synchronous read.
    /// </summary>
    /// <param name="nodeNumber">  Specifies the remote node number. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> The string. </returns>
    public string? QueryPrintTrimEnd( int nodeNumber, string dataToWrite )
    {
        this.ExecuteQuery( nodeNumber, dataToWrite );
        return this.ReadLineTrimEnd();
    }

    /// <summary>
    /// Executes a command on the remote node and prints the result, followed by a synchronous read.
    /// </summary>
    /// <param name="nodeNumber"> Specifies the remote node number. </param>
    /// <param name="format">     The commandFormat for constructing the data to write. </param>
    /// <param name="args">       The commandFormat arguments. </param>
    /// <returns> The string. </returns>
    public string? QueryPrintTrimEnd( int nodeNumber, string format, params object[] args )
    {
        this.ExecuteQuery( nodeNumber, format, args );
        return this.ReadLineTrimEnd();
    }

    #endregion

    #region " query - node and parse "

    #region " boolean "

    /// <summary>
    /// Executes a command on the remote node and prints the result, followed by a synchronous read.
    /// Parses the results as a Boolean.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="nodeNumber">       Specifies the remote node number. </param>
    /// <param name="format">           The commandFormat for constructing the data to write. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public bool? QueryPrint( bool emulatingValue, int nodeNumber, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        this.ExecuteQuery( nodeNumber, format, args );
        return this.ParseBoolean( this.ReadLineTrimEnd() ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Executes a command on the remote node and prints the result, followed by a synchronous read.
    /// Parses the results as a Boolean.
    /// </summary>
    /// <param name="result">     [in,out] The result value. </param>
    /// <param name="nodeNumber"> Specifies the remote node number. </param>
    /// <param name="format">     The commandFormat for constructing the data to write. </param>
    /// <param name="args">       The commandFormat arguments. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref bool result, int nodeNumber, string format, params object[] args )
    {
        this.ExecuteQuery( nodeNumber, format, args );
        return TryParse( this.ReadLineTrimEnd() ?? $"{result}", out result );
    }

    /// <summary>
    /// Executes a command on the remote node and prints the result, followed by a synchronous read.
    /// Parses the results as a Boolean.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="nodeNumber">       Specifies the remote node number. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The parsed value or default. </returns>
    public bool QueryPrint( bool emulatingValue, int nodeNumber, string dataToWrite )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        this.ExecuteQuery( nodeNumber, dataToWrite );
        return this.ParseBoolean( this.ReadLineTrimEnd() ?? $"{emulatingValue}" );
    }

    /// <summary>
    /// Executes a command on the remote node and prints the result, followed by a synchronous read.
    /// Parses the results as a Boolean.
    /// </summary>
    /// <param name="result">      [in,out] The result value. </param>
    /// <param name="nodeNumber">  Specifies the remote node number. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref bool result, int nodeNumber, string dataToWrite )
    {
        this.ExecuteQuery( nodeNumber, dataToWrite );
        return TryParse( this.ReadLineTrimEnd() ?? $"{result}", out result );
    }

    #endregion

    #region " double "

    /// <summary>
    /// Executes a command on the remote node and prints the result, followed by a synchronous read.
    /// Parses the results as a Double.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="nodeNumber">       Specifies the remote node number. </param>
    /// <param name="format">           The commandFormat for constructing the data to write. </param>
    /// <param name="args">             The commandFormat arguments. </param>
    /// <returns>   The parsed value or default. </returns>
    public double? QueryPrint( double emulatingValue, int nodeNumber, string format, params object[] args )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        this.ExecuteQuery( nodeNumber, format, args );
        return this.ParseDouble( this.ReadLineTrimEnd() ?? $"{emulatingValue}" );
    }

    /// <summary> Executes a command on the remote node and retrieves a Double. </summary>
    /// <param name="result">     [in,out] The result value. </param>
    /// <param name="nodeNumber"> Specifies the remote node number. </param>
    /// <param name="format">     The commandFormat for constructing the data to write. </param>
    /// <param name="args">       The commandFormat arguments. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref double result, int nodeNumber, string format, params object[] args )
    {
        this.ExecuteQuery( nodeNumber, format, args );
        return TryParse( this.ReadLineTrimEnd() ?? $"{result}", out result );
    }

    /// <summary>
    /// Executes a command on the remote node and prints the result, followed by a synchronous read.
    /// Parses the results as a Double.
    /// </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="emulatingValue">   The value to emulate if emulated reply is empty. </param>
    /// <param name="nodeNumber">       Specifies the remote node number. </param>
    /// <param name="dataToWrite">      The data to write. </param>
    /// <returns>   The parsed value or default. </returns>
    public double QueryPrint( double emulatingValue, int nodeNumber, string dataToWrite )
    {
        this.MakeEmulatedReplyIfEmpty( emulatingValue );
        this.ExecuteQuery( nodeNumber, dataToWrite );
        return this.ParseDouble( this.ReadLineTrimEnd() ?? $"{emulatingValue}" );
    }

    /// <summary> Executes a command on the remote node and retrieves a Double. </summary>
    /// <param name="result">      [in,out] The result value. </param>
    /// <param name="nodeNumber">  Specifies the remote node number. </param>
    /// <param name="dataToWrite"> The data to write. </param>
    /// <returns> <c>true</c> if the parsed value is valid. </returns>
    public bool TryQueryPrint( ref double result, int nodeNumber, string dataToWrite )
    {
        this.ExecuteQuery( nodeNumber, dataToWrite );
        return TryParse( this.ReadLineTrimEnd() ?? $"{result}", out result );
    }

    #endregion

    #endregion

    #region " nil "

    /// <summary> Returns <c>true</c> if the specified node global is Nil. </summary>
    /// <param name="nodeNumber"> Specifies the remote node number to validate. </param>
    /// <param name="value">      Specifies the global which to look for. </param>
    /// <returns> <c>true</c> if the specified node global exists; otherwise <c>false</c> </returns>
    public bool IsNil( int nodeNumber, string value )
    {
        return string.Equals( NilValue, this.QueryPrintTrimEnd( nodeNumber, value ), StringComparison.Ordinal );
    }

    /// <summary>
    /// Checks the series of values and return <c>true</c> if any one of them is nil.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="nodeNumber"> Specifies the remote node number to validate. </param>
    /// <param name="values">     Specifies a list of nil objects to check. </param>
    /// <returns> <c>True </c> if any value is nil; otherwise, <c>false</c> </returns>
    public bool IsNil( int nodeNumber, params string[] values )
    {
        if ( values is null || values.Length == 0 ) throw new ArgumentNullException( nameof( values ) );
        bool affirmative = false;
        foreach ( string value in values )
        {
            if ( !string.IsNullOrWhiteSpace( value ) )
            {
                if ( this.IsNil( nodeNumber, value ) )
                {
                    affirmative = true;
                    break;
                }
            }
        }

        return affirmative;
    }

    /// <summary>
    /// Checks the series of values and return <c>true</c> if any one of them is nil.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="isControllerNode"> true if this object is controller node. </param>
    /// <param name="nodeNumber">       Specifies the remote node number to validate. </param>
    /// <param name="values">           Specifies a list of nil objects to check. </param>
    /// <returns> <c>True </c> if any value is nil; otherwise, <c>false</c> </returns>
    public bool IsNil( bool isControllerNode, int nodeNumber, params string[] values )
    {
        return values is null || values.Length == 0
            ? throw new ArgumentNullException( nameof( values ) )
            : isControllerNode ? this.IsNil( values ) : this.IsNil( nodeNumber, values );
    }

    /// <summary> Loops until the name is found or timeout. </summary>
    /// <remarks> Use to verify if the Test Script exists. </remarks>
    /// <param name="nodeNumber"> Specifies the node number. </param>
    /// <param name="name">       Specifies the script name. </param>
    /// <param name="timeout">    The timeout. </param>
    /// <returns> <c>True </c> if nil; otherwise, <c>false</c> </returns>
    public bool WaitNotNil( int nodeNumber, string name, TimeSpan timeout )
    {
        return timeout.AsyncWaitUntil( TimeSpan.FromMilliseconds( Math.Min( 10d, timeout.TotalMilliseconds ) ),
                                       () => !this.IsNil( nodeNumber, name ), SessionBase.DoEventsAction ).Completed;
    }

    /// <summary>
    /// Checks the series of values and return <c>true</c> if any one of them is nil.
    /// </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="isControllerNode"> true if this object is controller node. </param>
    /// <param name="nodeNumber">       Specifies the remote node number to validate. </param>
    /// <param name="value">            Specifies the global which to look for. </param>
    /// <returns>   <c>True </c> if any value is nil; otherwise, <c>false</c> </returns>
    public bool IsNil( bool isControllerNode, int nodeNumber, string value )
    {
        return isControllerNode ? this.IsNil( value ) : this.IsNil( nodeNumber, value );
    }

    #endregion

    #endregion
}
