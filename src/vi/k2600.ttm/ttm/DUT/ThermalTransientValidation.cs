using cc.isr.Std.IComparableExtensions;

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
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.TraceDefaults.VoltageMinimum,
            ( double ) Properties.DriverSettings.Instance.TraceDefaults.VoltageMaximum, "Thermal Transient Voltage Change ", out details );
    }

    /// <summary> Validates the aperture described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateAperture( double value, out string details )
    {
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.TraceDefaults.ApertureMinimum,
            ( double ) Properties.DriverSettings.Instance.TraceDefaults.ApertureMaximum, "Thermal Transient Aperture", out details );
    }

    /// <summary> Validates the current level described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateCurrentLevel( double value, out string details )
    {
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.TraceDefaults.CurrentMinimum,
            ( double ) Properties.DriverSettings.Instance.TraceDefaults.CurrentMaximum, "Thermal Transient Current Level", out details );
    }

    /// <summary> Validates the limit described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateLimit( double value, out string details )
    {
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.TraceDefaults.VoltageChangeMinimum,
            ( double ) Properties.DriverSettings.Instance.TraceDefaults.VoltageChangeMaximum, "Thermal Transient Limit", out details );
    }

    /// <summary> Validates the median filter size described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateMedianFilterSize( int value, out string details )
    {
        return value.IsBetweenInclusive( Properties.DriverSettings.Instance.TraceDefaults.MedianFilterLengthMinimum,
            Properties.DriverSettings.Instance.TraceDefaults.MedianFilterLengthMaximum, "Thermal Transient Median Filter Length", out details );
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
        return ValidateSamplingInterval( samplingInterval, out details ) &&
            ValidateTracePoints( tracePoints, out details ) &&
            pulseWidth.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.TraceDefaults.DurationMinimum,
            ( double ) Properties.DriverSettings.Instance.TraceDefaults.DurationMaximum, "Thermal Transient Pulse Width", out details );
    }

    /// <summary> Validates the sampling interval described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateSamplingInterval( double value, out string details )
    {
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.TraceDefaults.SamplingIntervalMinimum,
            ( double ) Properties.DriverSettings.Instance.TraceDefaults.SamplingIntervalMaximum, "Thermal Transient Sampling Interval", out details );
    }

    /// <summary> Validates the trace points described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateTracePoints( int value, out string details )
    {
        return value.IsBetweenInclusive( Properties.DriverSettings.Instance.TraceDefaults.TracePointsMinimum,
            Properties.DriverSettings.Instance.TraceDefaults.TracePointsMaximum, "Thermal Transient Trace Points", out details );
    }

    /// <summary> Validates the voltage limit described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateVoltageLimit( double value, out string details )
    {
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.TraceDefaults.VoltageMinimum,
            ( double ) Properties.DriverSettings.Instance.TraceDefaults.VoltageMaximum, "Thermal Transient Voltage Limit", out details );
    }

    /// <summary>   Validates the voltage limit described by value. </summary>
    /// <remarks>   2026-03-27. </remarks>
    /// <param name="value">                    The value. </param>
    /// <param name="expectedMaximumVoltage">   The expected maximum voltage. </param>
    /// <param name="details">                  [out] The details. </param>
    /// <returns>   <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateVoltageLimit( double value, double expectedMaximumVoltage, out string details )
    {
        bool affirmative = value >= expectedMaximumVoltage;
        details = affirmative
            ? string.Empty
            : $"A Thermal Transient Voltage Limit of {value} volts is too low; Based on the cold resistance high limit and thermal transient current level and voltage change, the voltage might reach {expectedMaximumVoltage} volts.";
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
        return ValidateVoltageChange( allowedVoltageChange, out details )
                                && ValidateCurrentLevel( currentLevel, out details )
                                && ValidateVoltageLimit( value, out details )
                                && ValidateVoltageLimit( value, (maximumColdResistance * currentLevel) + allowedVoltageChange, out details );
    }

    #region " estimator "

    /// <summary> Validates the ThermalCoefficient described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateThermalCoefficient( double value, out string details )
    {
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.EstimatorDefaults.ThermalCoefficientMinimum,
            ( double ) Properties.DriverSettings.Instance.EstimatorDefaults.ThermalCoefficientMaximum, "Thermal Coefficient", out details );
    }

    #endregion
}
