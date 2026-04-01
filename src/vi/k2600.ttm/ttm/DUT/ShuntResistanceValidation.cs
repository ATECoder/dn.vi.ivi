using cc.isr.Std.IComparableExtensions;
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
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.ShuntDefaults.CurrentMinimum,
            ( double ) Properties.DriverSettings.Instance.ShuntDefaults.CurrentMaximum, "Shunt Current", out details );
    }

    /// <summary> Validates the limit described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateLimit( double value, out string details )
    {
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.ShuntDefaults.Minimum,
            ( double ) Properties.DriverSettings.Instance.ShuntDefaults.Maximum, "Shunt limit", out details );
    }

    /// <summary> Validates the voltage limit described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateVoltageLimit( double value, out string details )
    {
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.ShuntDefaults.VoltageMinimum,
            Properties.DriverSettings.Instance.ShuntDefaults.VoltageMaximum, "Shunt voltage limit", out details );
    }
}
