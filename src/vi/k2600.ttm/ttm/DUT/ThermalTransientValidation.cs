// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600.Ttm;

public partial class ThermalTransient
{
    /// <summary> Validates the allowed voltage change described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateVoltageChange( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum && value <= ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum
                ? $"Thermal Transient Voltage Change value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum}."
                : $"Thermal Transient Voltage Change value of {value} is higher than the maximum of {Properties.Settings.Instance.TtmTraceSettings.VoltageMaximum}.";

        return affirmative;
    }

    /// <summary> Validates the aperture described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateAperture( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmTraceSettings.ApertureMinimum && value <= ( double ) Properties.Settings.Instance.TtmTraceSettings.ApertureMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmTraceSettings.ApertureMinimum
                ? $"Thermal Transient aperture value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmTraceSettings.ApertureMinimum}."
                : $"Thermal Transient aperture value of {value} is higher than the maximum of {Properties.Settings.Instance.TtmTraceSettings.ApertureMaximum}.";

        return affirmative;
    }

    /// <summary> Validates the current level described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateCurrentLevel( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmTraceSettings.CurrentMinimum && value <= ( double ) Properties.Settings.Instance.TtmTraceSettings.CurrentMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmTraceSettings.CurrentMinimum
                ? $"Thermal Transient CurrentLevel value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmTraceSettings.CurrentMinimum}."
                : $"Thermal Transient Current Level value of {value} is higher than the maximum of {Properties.Settings.Instance.TtmTraceSettings.CurrentMaximum}.";

        return affirmative;
    }

    /// <summary> Validates the limit described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateLimit( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageChangeMinimum && value <= ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageChangeMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum
                ? $"Thermal Transient Limit value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum}."
                : $"Thermal Transient Limit value of {value} is higher than the maximum of {Properties.Settings.Instance.TtmTraceSettings.VoltageMaximum}.";

        return affirmative;
    }

    /// <summary> Validates the median filter size described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateMedianFilterSize( int value, out string details )
    {
        bool affirmative = value >= Properties.Settings.Instance.TtmTraceSettings.MedianFilterLengthMinimum && value <= Properties.Settings.Instance.TtmTraceSettings.MedianFilterLengthMaximum;
        details = affirmative
            ? string.Empty
            : value < Properties.Settings.Instance.TtmTraceSettings.TracePointsMinimum
                ? $"Thermal Transient Median Filter Length of {value} is lower than the minimum of {Properties.Settings.Instance.TtmTraceSettings.MedianFilterLengthMinimum}."
                : $"Thermal Transient Median Filter Length value of {value} is higher than the maximum of {Properties.Settings.Instance.TtmTraceSettings.MedianFilterLengthMaximum}.";

        return affirmative;
    }

    /// <summary> Validates the Duration described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="samplingInterval"> The sampling interval. </param>
    /// <param name="tracePoints">      The trace points. </param>
    /// <param name="details">          [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidatePulseWidth( double samplingInterval, int tracePoints, out string details )
    {
        double pulseWidth = samplingInterval * tracePoints;
        bool affirmative = ValidateSamplingInterval( samplingInterval, out string details1 ) && ValidateTracePoints( tracePoints, out details1 )
            && pulseWidth >= ( double ) Properties.Settings.Instance.TtmTraceSettings.DurationMinimum && pulseWidth <= ( double ) Properties.Settings.Instance.TtmTraceSettings.DurationMaximum;
        if ( affirmative )
            details = string.Empty;
        else if ( !string.IsNullOrWhiteSpace( details1 ) )
            details = details1; // we have details. nothing to do.
        else
        {
            details = pulseWidth < ( double ) Properties.Settings.Instance.TtmTraceSettings.DurationMinimum
                ? $"Thermal Transient Pulse Width of {pulseWidth}s is lower than the minimum of {Properties.Settings.Instance.TtmTraceSettings.DurationMinimum}s."
                : $"Thermal Transient Pulse Width of {pulseWidth}s is higher than the maximum of {Properties.Settings.Instance.TtmTraceSettings.DurationMaximum}s.";
        }

        return affirmative;
    }

    /// <summary> Validates the sampling interval described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateSamplingInterval( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalMinimum
            && value <= ( double ) Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalMinimum
                ? $"Thermal Transient Sample Interval value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalMinimum}."
                : $"Thermal Transient Sample Interval value of {value} is higher than the maximum of {Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalMaximum}.";

        return affirmative;
    }

    /// <summary> Validates the trace points described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateTracePoints( int value, out string details )
    {
        bool affirmative = value >= Properties.Settings.Instance.TtmTraceSettings.TracePointsMinimum
            && value <= Properties.Settings.Instance.TtmTraceSettings.TracePointsMaximum;
        details = affirmative
            ? string.Empty
            : value < Properties.Settings.Instance.TtmTraceSettings.TracePointsMinimum
                ? $"Thermal Transient Trace Points value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmTraceSettings.TracePointsMinimum}."
                : $"Thermal Transient Trace Points value of {value} is higher than the maximum of {Properties.Settings.Instance.TtmTraceSettings.TracePointsMaximum}";

        return affirmative;
    }

    /// <summary> Validates the voltage limit described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateVoltageLimit( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum
            && value <= ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum
                ? $"Thermal Transient Voltage Limit value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmTraceSettings.VoltageMinimum}."
                : $"Thermal Transient Voltage Limit value of {value} is high than the maximum of {Properties.Settings.Instance.TtmTraceSettings.VoltageMaximum}.";

        return affirmative;
    }

    /// <summary> Validates the transient voltage limit described by details. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">                 The value. </param>
    /// <param name="maximumColdResistance"> The maximum cold resistance. </param>
    /// <param name="currentLevel">          The current level. </param>
    /// <param name="allowedVoltageChange">  The allowed voltage change. </param>
    /// <param name="details">               [out] The details. </param>
    /// <returns>
    /// <c>true</c> if the voltage limit is in rage; <c>false</c> is the limit is too low.
    /// </returns>
    public static bool ValidateVoltageLimit( double value, double maximumColdResistance, double currentLevel, double allowedVoltageChange, out string details )
    {
        double expectedMaximumVoltage = (maximumColdResistance * currentLevel) + allowedVoltageChange;
        bool affirmative = ValidateVoltageChange( allowedVoltageChange, out string details1 )
                                && ValidateCurrentLevel( currentLevel, out details1 )
                                && ValidateVoltageLimit( value, out details1 )
                                && value >= expectedMaximumVoltage;
        details = details1;
        if ( !affirmative && string.IsNullOrWhiteSpace( details ) )
            details = $"A Thermal transient voltage limit of {value} volts is too low;. Based on the cold resistance high limit and thermal transient current level and voltage change, the voltage might reach {expectedMaximumVoltage} volts.";
        // we have details. nothing to do.
#if false
        else if ( expectedMaximumVoltage < expectedMaximumVoltage )
        {
            details = $"A Thermal transient voltage limit of {value} volts is too low;. Based on the cold resistance high limit and thermal transient current level and voltage change, the voltage might reach {expectedMaximumVoltage} volts.";
        }
#endif
        return affirmative;
    }

    #region " estimator "

    /// <summary> Validates the ThermalCoefficient described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateThermalCoefficient( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficientMinimum && value <= ( double ) Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficientMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficientMinimum
                ? string.Format( System.Globalization.CultureInfo.CurrentCulture, "Thermal Coefficient value of {0} is lower than the minimum of {1}.", value, Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficientMinimum )
                : string.Format( System.Globalization.CultureInfo.CurrentCulture, "Thermal Coefficient value of {0} is higher than the maximum of {1}.", value, Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficientMaximum );

        return affirmative;
    }

    #endregion
}
