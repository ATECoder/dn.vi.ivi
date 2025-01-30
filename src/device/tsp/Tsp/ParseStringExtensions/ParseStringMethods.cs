namespace cc.isr.VI.Tsp.ParseStringExtensions;

/// <summary>   A parse string methods. </summary>
/// <remarks>   2024-11-11. </remarks>
public static class ParseStringMethods
{

    /// <summary>   Attempts to parse nullable bool a bool from the given string. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="result">   [out] True to result. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryParseNullableBool( this string value, out bool? result )
    {
        if ( string.IsNullOrWhiteSpace( value ) || string.Equals( cc.isr.VI.Syntax.Tsp.Lua.NilValue, value, StringComparison.Ordinal ) )
        {
            result = new bool?();
            return true;
        }
        else
        {
            if ( bool.TryParse( value, out bool temp ) )
            {
                result = temp;
                return true;
            }
            else
            {
                result = temp;
                return false;
            }
        }
    }

    /// <summary>
    /// Attempts to parse a nullable integer from the given data, returning a default value rather
    /// than throwing an exception if it fails.
    /// </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="result">   [out] True to result. </param>
    /// <returns>   An Int. </returns>
    public static bool TryParseNullableInteger( this string value, out int? result )
    {
        if ( string.IsNullOrWhiteSpace( value ) || string.Equals( cc.isr.VI.Syntax.Tsp.Lua.NilValue, value, StringComparison.Ordinal ) )
        {
            result = new int?();
            return true;
        }
        else
        {
            if ( int.TryParse( value,
                System.Globalization.NumberStyles.AllowExponent | System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.CurrentCulture, out int temp ) )
            {
                result = temp;
                return true;
            }
            else
            {

                result = temp;
                return false;
            }
        }
    }

    /// <summary>   Attempts to parse nullable double a double from the given string. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="result">   [out] True to result. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryParseNullableDouble( this string value, out double? result )
    {
        if ( string.IsNullOrWhiteSpace( value ) || string.Equals( cc.isr.VI.Syntax.Tsp.Lua.NilValue, value, StringComparison.Ordinal ) )
        {
            result = new double?();
            return true;
        }
        else
        {
            if ( double.TryParse( value, out double temp ) )
            {
                result = temp;
                return true;
            }
            else
            {
                result = temp;
                return false;
            }
        }
    }
}
