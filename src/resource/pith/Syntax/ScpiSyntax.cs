// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.Std.ParseExtensions;

namespace cc.isr.VI.Syntax;

/// <summary> includes the SCPI Commands. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class ScpiSyntax
{

    #region " constants "

    /// <summary>   (Immutable) the operation completed value. </summary>
    public const string OperationCompletedValue = "1";

    /// <summary>   (Immutable) the operation not completed value. </summary>
    public const string OperationNotCompletedValue = "0";

    #endregion

    #region " numerical constants "

    /// <summary> Gets the SCPI value for infinity. </summary>
    public const double Infinity = 9.9E+37d;

    /// <summary> Gets the SCPI caption for infinity. </summary>
    public const string InfinityCaption = "9.90000E+37";

    /// <summary> Gets the SCPI value for negative infinity. </summary>
    public const double NegativeInfinity = -9.91E+37d;

    /// <summary> Gets the SCPI caption for negative infinity. </summary>
    public const string NegativeInfinityCaption = "-9.91000E+37";

    /// <summary> Gets the SCPI value for 'non-a-number' (NAN). </summary>
    public const double NotANumber = 9.91E+37d;

    /// <summary> Gets the SCPI caption for 'not-a-number' (NAN). </summary>
    public const string NotANumberCaption = "9.91000E+37";

    #endregion

    #region " default error messages "

    /// <summary> Gets the error message representing no error. </summary>
    public const string NoErrorMessage = "No Error";

    /// <summary> Gets the compound error message representing no error. </summary>
    public const string NoErrorCompoundMessage = "0,No Error";

    #endregion

    #region " status "

    /// <summary> Gets the 'Next Error' query command. </summary>
    public const string NextErrorQueryCommand = ":STAT:QUE?";

    /// <summary> Gets the error queue clear command. </summary>
    public const string ClearErrorQueueCommand = ":STAT:QUE:CLEAR";

    /// <summary> Gets the preset status command. </summary>
    public const string StatusPresetCommand = ":STAT:PRES";

    /// <summary> Gets the measurement event condition command. </summary>
    public const string MeasurementEventConditionQueryCommand = ":STAT:MEAS:COND?";

    /// <summary> Gets the measurement event status query command. </summary>
    public const string MeasurementEventQueryCommand = ":STAT:MEAS:EVEN?";

    /// <summary> Gets the Measurement event enable Query command. </summary>
    public const string MeasurementEventEnableQueryCommand = ":STAT:MEAS:ENAB?";

    /// <summary> Gets the Measurement event enable command format. </summary>
    public const string MeasurementEventEnableCommandFormat = ":STAT:MEAS:ENAB {0:D}";

    /// <summary> Gets the Measurement event Positive Transition Query command. </summary>
    public const string MeasurementEventPositiveTransitionQueryCommand = ":STAT:MEAS:PTR?";

    /// <summary> Gets the Measurement event Positive Transition command format. </summary>
    public const string MeasurementEventPositiveTransitionCommandFormat = ":STAT:MEAS:PTR {0:D}";

    /// <summary> Gets the Measurement event Negative Transition Query command. </summary>
    public const string MeasurementEventNegativeTransitionQueryCommand = ":STAT:MEAS:NTR?";

    /// <summary> Gets the Measurement event Negative Transition command format. </summary>
    public const string MeasurementEventNegativeTransitionCommandFormat = ":STAT:MEAS:NTR {0:D}";

    /// <summary> Gets the measurement event condition command. </summary>
    public const string OperationEventConditionQueryCommand = ":STAT:OPER:COND?";

    /// <summary> Gets the operation event enable command format. </summary>
    public const string OperationEventEnableCommandFormat = ":STAT:OPER:ENAB {0:D}";

    /// <summary> Gets the operation event enable Query command. </summary>
    public const string OperationEventEnableQueryCommand = ":STAT:OPER:ENAB?";

    /// <summary> Gets the operation register event status query command. </summary>
    public const string OperationEventQueryCommand = ":STAT:OPER:EVEN?";

    /// <summary> Gets the operation event map command format. </summary>
    public const string OperationEventMapCommandFormat = ":STAT:OPER:MAP {0:D},{1:D},{2:D}";

    /// <summary> Gets the operation map query command format. </summary>
    public const string OperationEventMapQueryCommandFormat = ":STAT:OPER:MAP? {0:D}";

    /// <summary> Gets the measurement event condition command. </summary>
    public const string QuestionableEventConditionQueryCommand = ":STAT:QUES:COND?";

    /// <summary> Gets the Questionable event enable command format. </summary>
    public const string QuestionableEventEnableCommandFormat = ":STAT:QUES:ENAB {0:D}";

    /// <summary> Gets the Questionable event enable Query command. </summary>
    public const string QuestionableEventEnableQueryCommand = ":STAT:QUES:ENAB?";

    /// <summary> Gets the Questionable register event status query command. </summary>
    public const string QuestionableEventQueryCommand = ":STAT:QUES:EVEN?";

    /// <summary> Gets the Questionable event map command format. </summary>
    public const string QuestionableEventMapCommandFormat = ":STAT:QUES:MAP {0:D},{1:D},{2:D}";

    /// <summary> Gets the Questionable map query command format. </summary>
    public const string QuestionableEventMapQueryCommandFormat = ":STAT:QUES:MAP? {0:D}";

    #endregion

    #region " system "

    /// <summary> Gets the last system error queue query command. </summary>
    public const string LastSystemErrorQueryCommand = ":SYST:ERR?";

    /// <summary> Gets clear system error queue command. </summary>
    public const string ClearSystemErrorQueueCommand = ":SYST:CLE";

    /// <summary> The read line frequency command. </summary>
    public const string ReadLineFrequencyCommand = ":SYST:LFR?";

    /// <summary> The initialize memory command. </summary>
    public const string InitializeMemoryCommand = ":SYST:MEM:INIT";

    /// <summary> The preset command. </summary>
    public const string SystemPresetCommand = ":SYST:PRES";

    /// <summary> The language (SCPI) revision query command. </summary>
    public const string LanguageRevisionQueryCommand = ":SYST:VERS?";

    #endregion

    #region " units "

    /// <summary>   Adds a unit to the units dictionary. </summary>
    /// <remarks>   2024-07-04. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="suffix">   The suffix. </param>
    /// <param name="unit">     The unit. </param>
    public static void AddUnit( string suffix, cc.isr.UnitsAmounts.Unit unit )
    {
        if ( string.IsNullOrWhiteSpace( suffix ) ) throw new ArgumentNullException( nameof( suffix ) );
        if ( unit is null ) throw new ArgumentNullException( nameof( unit ) );
        if ( ScpiSyntax.UnitsDictionary.Keys.Contains( suffix ) )
        {
            if ( !unit.Equals( ScpiSyntax.UnitsDictionary[suffix] ) )
                throw new cc.isr.VI.Pith.NativeException(
                    $"Mismatch detected: Attempting to add {unit.Symbol} unit where existing {suffix} is {ScpiSyntax.UnitsDictionary[suffix].Symbol}" );
        }
        else
            ScpiSyntax.UnitsDictionary.Add( suffix, unit );
    }

    /// <summary>   (Immutable) the lazy units dix. </summary>
    private static readonly Lazy<Dictionary<string, cc.isr.UnitsAmounts.Unit>> _lazyUnitsDix = new( CreateUnitsDictionary, true );

    /// <summary>
    /// Creates units dictionary for translating SCPI unit names to <see cref="cc.isr.UnitsAmounts.StandardUnits">
    /// standard units</see>.
    /// </summary>
    /// <remarks>   2024-07-04. </remarks>
    /// <returns>   The new units dictionary. </returns>
    private static Dictionary<string, cc.isr.UnitsAmounts.Unit> CreateUnitsDictionary()
    {
        Dictionary<string, cc.isr.UnitsAmounts.Unit> dix = new()
            {
                { "AMP", cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere },
                { "ADC", cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere },
                { "AAC", cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere },
                { "C", cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.DegreeCelsius },
                { "CELS", cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.DegreeCelsius },
                { "DB", cc.isr.UnitsAmounts.StandardUnits.UnitlessUnits.Decibel },
                { "F", cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.DegreeFahrenheit },
                { "FAHR", cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.DegreeFahrenheit },
                { "HZ", cc.isr.UnitsAmounts.StandardUnits.FrequencyUnits.Hertz },
                { "K", cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.Kelvin },
                { "KELV", cc.isr.UnitsAmounts.StandardUnits.TemperatureUnits.Kelvin },
                { "OHM", cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm },
                { "OHM4W", cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm },
                { "RDNG#", cc.isr.UnitsAmounts.StandardUnits.UnitlessUnits.Count },
                { "SECS", cc.isr.UnitsAmounts.StandardUnits.TimeUnits.Second },
                { "VOLT", cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt },
                { "VDC", cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt },
                { "VAC", cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt }
            };
        return dix;
    }

    /// <summary>   Returns the Unit Parser hash. </summary>
    /// <value>
    /// A Dictionary for translating SCPI unit names to <see cref="cc.isr.UnitsAmounts.StandardUnits">
    /// standard units</see>.
    /// </value>
    public static Dictionary<string, cc.isr.UnitsAmounts.Unit> UnitsDictionary => _lazyUnitsDix.Value;

    /// <summary> Parse the reading to extract the unit. </summary>
    /// <param name="unitReading"> The reading. </param>
    /// <returns> A  Tuple of (<see cref="string">unit name</see>, <see cref="UnitsAmounts.Unit"/>). </returns>
    public static (string Value, cc.isr.UnitsAmounts.Unit Unit) ParseUnit( string unitReading )
    {
        KeyValuePair<string, UnitsAmounts.Unit> unit = new( "VOLT", cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt );

        if ( string.IsNullOrWhiteSpace( unitReading ) ) return (unit.Key, unit.Value);

        foreach ( KeyValuePair<string, UnitsAmounts.Unit> currentUnit in ScpiSyntax.UnitsDictionary )
        {
            unit = currentUnit;
            if ( string.Equals( unitReading, unit.Key, StringComparison.OrdinalIgnoreCase ) )
                break;
        }

        return (unit.Key, unit.Value);
    }

    /// <summary> Tries to parse a unit from the unit suffix of the reading. </summary>
    /// <param name="reading"> Specifies the reading text. </param>
    /// <param name="unit">    [in,out] The unit. </param>
    /// <returns> <c>true</c> if parsed. </returns>
    public static bool TryParseUnit( string reading, ref cc.isr.UnitsAmounts.Unit? unit )
    {
        string suffix = ParseUnitSuffix( reading );
        unit = string.IsNullOrWhiteSpace( suffix ) || !ScpiSyntax.UnitsDictionary.Keys.Contains( suffix )
            ? null
            : ScpiSyntax.UnitsDictionary[suffix];
        return unit is not null;
    }

    /// <summary> Extracts the unit suffix from the reading. </summary>
    /// <param name="reading"> The reading value that includes units as a suffix. </param>
    /// <returns> The unit suffix. </returns>
    public static string ParseUnitSuffix( string reading )
    {
        string suffix = string.Empty;
        if ( !string.IsNullOrWhiteSpace( reading ) && ScpiSyntax.UnitsDictionary.Keys.Any() )
        {
            foreach ( string currentSuffix in ScpiSyntax.UnitsDictionary.Keys )
            {
                suffix = currentSuffix;
                if ( reading.EndsWith( suffix, StringComparison.OrdinalIgnoreCase ) )
                {
                    break;
                }
            }
        }

        return suffix;
    }

    /// <summary> Trims unit suffixes. </summary>
    /// <param name="reading">     The raw reading. </param>
    /// <param name="unitsSuffix"> The units suffix. </param>
    /// <returns> A <see cref="string" /> with the units suffix removed. </returns>
    public static string TrimUnits( string reading, string unitsSuffix )
    {
        if ( !(string.IsNullOrWhiteSpace( reading ) || string.IsNullOrWhiteSpace( unitsSuffix )) )
        {
            reading = reading[..^unitsSuffix.Length];
        }

        return reading;
    }

    /// <summary> Trims unit suffixes. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> A <see cref="string" /> with the units suffix removed. </returns>
    public static string TrimUnits( string value )
    {
        return TrimUnits( value, ParseUnitSuffix( value ) );
    }

    /// <summary> Return an amount form a reading containing value and unit. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="reading"> The reading. </param>
    /// <returns> Reading as an cc.isr.UnitsAmounts.Amount. </returns>
    public static cc.isr.UnitsAmounts.Amount ToAmount( string reading )
    {
        if ( string.IsNullOrWhiteSpace( reading ) ) throw new ArgumentNullException( nameof( reading ) );

        // strip out the non-numeric part of the number
        string valueReading = reading.ExtractNumberPart();
        string unitReading = reading[valueReading.Length..];
        if ( !double.TryParse( valueReading, out double value ) )
        {
            value = double.NaN;
        }

        return BuildAmount( value, unitReading );
    }

    /// <summary> Builds an amount. </summary>
    /// <param name="value">       The value. </param>
    /// <param name="unitReading"> The reading. </param>
    /// <returns> An cc.isr.UnitsAmounts.Amount. </returns>
    public static cc.isr.UnitsAmounts.Amount BuildAmount( double value, string unitReading )
    {
        return new cc.isr.UnitsAmounts.Amount( value, ParseUnit( unitReading ).Unit );
    }

    #endregion
}
