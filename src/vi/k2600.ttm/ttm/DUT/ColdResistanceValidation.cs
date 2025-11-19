namespace cc.isr.VI.Tsp.K2600.Ttm;

public partial class ColdResistance
{
    /// <summary>   Validates the aperture described by value. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateAperture( double value, out string details )
    {
        bool affirmative = value >= Properties.Settings.Instance.TtmResistanceSettings.ApertureMinimum
            && value <= Properties.Settings.Instance.TtmResistanceSettings.ApertureMaximum;
        details = affirmative
            ? string.Empty
            : value < Properties.Settings.Instance.TtmResistanceSettings.ApertureMinimum
                ? $"Cold Resistance aperture value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmResistanceSettings.ApertureMinimum}."
                : $"Cold Resistance aperture value of {value} is higher than the maximum of {Properties.Settings.Instance.TtmResistanceSettings.ApertureMaximum}.";

        return affirmative;
    }

    /// <summary>   Validates the current described by value. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateCurrentRange( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmResistanceSettings.CurrentMinimum
            && value <= ( double ) Properties.Settings.Instance.TtmResistanceSettings.CurrentMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmResistanceSettings.CurrentMinimum
                ? $"Cold Resistance Current value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmResistanceSettings.CurrentMinimum}."
                : $"Cold Resistance Current value of {value} is high than the maximum of {Properties.Settings.Instance.TtmResistanceSettings.CurrentMaximum}.";

        return affirmative;
    }

    /// <summary>   Validates the limit described by value. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateLimit( double value, out string details )
    {
        bool affirmative = value >= Properties.Settings.Instance.TtmResistanceSettings.Minimum
            && value <= Properties.Settings.Instance.TtmResistanceSettings.Maximum;
        details = affirmative
            ? string.Empty
            : value < Properties.Settings.Instance.TtmResistanceSettings.Minimum
                ? $"Cold Resistance Limit value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmResistanceSettings.Minimum}."
                : $"Cold Resistance Limit value of {value} is high than the maximum of {Properties.Settings.Instance.TtmResistanceSettings.Maximum}.";

        return affirmative;
    }

    /// <summary>   Validates the voltage level described by value. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateVoltageRange( double value, out string details )
    {
        bool affirmative = value >= Properties.Settings.Instance.TtmResistanceSettings.VoltageMinimum
            && value <= Properties.Settings.Instance.TtmResistanceSettings.VoltageMaximum;
        details = affirmative
            ? string.Empty
            : value < Properties.Settings.Instance.TtmResistanceSettings.VoltageMinimum
                ? $"Cold Resistance Voltage Limit value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmResistanceSettings.VoltageMinimum}."
                : $"Cold Resistance Voltage Limit value of {value} is high than the maximum of {Properties.Settings.Instance.TtmResistanceSettings.VoltageMaximum}.";

        return affirmative;
    }
}
