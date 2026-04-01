using cc.isr.Std.IComparableExtensions;
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
        return value.IsBetweenInclusive( Properties.DriverSettings.Instance.ColdResistanceDefaults.ApertureMinimum,
            Properties.DriverSettings.Instance.ColdResistanceDefaults.ApertureMaximum, "Cold Resistance aperture", out details );
    }

    /// <summary>   Validates the current described by value. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateCurrentRange( double value, out string details )
    {
        return value.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentMinimum,
            ( double ) Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentMaximum, "Cold Resistance current", out details );
    }

    /// <summary>   Validates the limit described by value. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateLimit( double value, out string details )
    {
        return value.IsBetweenInclusive( Properties.DriverSettings.Instance.ColdResistanceDefaults.Minimum,
            Properties.DriverSettings.Instance.ColdResistanceDefaults.Maximum, "Cold Resistance Limit", out details );
    }

    /// <summary>   Validates the voltage level described by value. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidateVoltageRange( double value, out string details )
    {
        return value.IsBetweenInclusive( Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageMinimum,
            Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageMaximum, "Cold resistance voltage limit", out details );
    }
}
