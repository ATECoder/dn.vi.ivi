namespace cc.isr.VI.Tsp.K2600.Ttm;

public partial class ShuntResistance
{
    /// <summary> Validates the current level or range described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateCurrent( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmShuntSettings.CurrentMinimum && value <= ( double ) Properties.Settings.Instance.TtmShuntSettings.CurrentMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmShuntSettings.CurrentMinimum
                ? string.Format( System.Globalization.CultureInfo.CurrentCulture,
                "Shunt MeasuredValue Current value of {0} is lower than the minimum of {1}.", value, Properties.Settings.Instance.TtmShuntSettings.CurrentMinimum )
                : string.Format( System.Globalization.CultureInfo.CurrentCulture,
                "Shunt MeasuredValue Current value of {0} is high than the maximum of {1}.", value, Properties.Settings.Instance.TtmShuntSettings.CurrentMaximum );

        return affirmative;
    }

    /// <summary> Validates the limit described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateLimit( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmShuntSettings.Minimum && value <= ( double ) Properties.Settings.Instance.TtmShuntSettings.Maximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmShuntSettings.Minimum
                ? string.Format( System.Globalization.CultureInfo.CurrentCulture,
                "Shunt MeasuredValue Limit value of {0} is lower than the minimum of {1}.", value, Properties.Settings.Instance.TtmShuntSettings.Minimum )
                : string.Format( System.Globalization.CultureInfo.CurrentCulture,
                "Shunt MeasuredValue Limit value of {0} is high than the maximum of {1}.", value, Properties.Settings.Instance.TtmShuntSettings.Maximum );

        return affirmative;
    }

    /// <summary> Validates the voltage limit described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateVoltageLimit( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmShuntSettings.VoltageMinimum && value <= ( double ) Properties.Settings.Instance.TtmShuntSettings.VoltageMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmShuntSettings.VoltageMinimum
                ? string.Format( System.Globalization.CultureInfo.CurrentCulture,
                "Shunt MeasuredValue Voltage Limit value of {0} is lower than the minimum of {1}.", value, Properties.Settings.Instance.TtmShuntSettings.VoltageMinimum )
                : string.Format( System.Globalization.CultureInfo.CurrentCulture,
                "Shunt MeasuredValue Voltage Limit value of {0} is high than the maximum of {1}.", value, Properties.Settings.Instance.TtmShuntSettings.VoltageMaximum );

        return affirmative;
    }
}
