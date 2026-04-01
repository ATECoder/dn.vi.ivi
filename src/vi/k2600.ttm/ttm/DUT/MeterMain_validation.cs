using cc.isr.Std.IComparableExtensions;
namespace cc.isr.VI.Tsp.K2600.Ttm;

public partial class MeterMain : DeviceUnderTestElementBase, IEquatable<MeterMain>
{
    /// <summary> Validates the Post Transient Delay described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="delay">   The delay. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns>   True if the <paramref name="delay"/> is within range; otherwise, false.. </returns>
    public static bool ValidatePostTransientDelay( double delay, out string details )
    {
        return delay.IsBetweenInclusive( ( double ) Properties.DriverSettings.Instance.MeterDefaults.PostTransientDelayMinimum,
            ( double ) Properties.DriverSettings.Instance.MeterDefaults.PostTransientDelayMaximum, "Post Transient Delay", out details );
    }

    /// <summary>   Validates the legacy driver. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <param name="driverOption">    The driver option. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the <paramref name="driverOption"/> is within range; otherwise, false.. </returns>
    public static bool ValidateLegacyDriver( int driverOption, out string details )
    {
        bool affirmative = driverOption is 0 or 1;
        details = affirmative
            ? string.Empty
            : $"Legacy Driver value of {driverOption} must be either 0 or 1.";

        return affirmative;
    }

    /// <summary>   Validates the contact threshold. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <param name="limit">    The limit value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the <paramref name="limit"/> is within range; otherwise, false.. </returns>
    public static bool ValidateContactThreshold( int limit, out string details )
    {
        return limit.IsBetweenInclusive( Properties.DriverSettings.Instance.MeterDefaults.ContactCheckThresholdMinimum,
            Properties.DriverSettings.Instance.MeterDefaults.ContactCheckThresholdMaximum, "Contact Check Limit", out details );
    }

    /// <summary>   Validates the contact check options. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <param name="contactCheckOptions">  The contact option value. </param>
    /// <param name="details">              [out] The details. </param>
    /// <returns>
    /// True if the <paramref name="contactCheckOptions"/> is within range; otherwise, false.
    /// </returns>
    public static bool ValidateContactCheckOptions( int contactCheckOptions, out string details )
    {
        return contactCheckOptions.IsBetweenInclusive( ( int ) Properties.DriverSettings.Instance.MeterDefaults.ContactCheckOptionsMinimum,
            ( int ) Properties.DriverSettings.Instance.MeterDefaults.ContactCheckOptionsMaximum, "Contact check options", out details );
    }

    /// <summary>   Validates the open limit. </summary>
    /// <remarks>   2026-03-24. </remarks>
    /// <param name="limit">    The limit value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the <paramref name="limit"/> is within range; otherwise, false.. </returns>
    public static bool ValidateOpenLimit( int limit, out string details )
    {
        return limit.IsBetweenInclusive( Properties.DriverSettings.Instance.MeterDefaults.OpenLimitMinimum,
                Properties.DriverSettings.Instance.MeterDefaults.OpenLimitMaximum, "Open limit", out details );
    }

    /// <summary>   Validates the a source measure unit with the specified name exists. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <param name="session">                  The session. </param>
    /// <param name="sourceMeasureUnitName">    The name of the source measure unit. </param>
    /// <param name="details">                  [out] The details. </param>
    /// <returns>
    /// True if a source measure unit with the specified name exists; otherwise, false.
    /// </returns>
    public static bool ValidateSourceMeasureUnitName( Pith.SessionBase session, string sourceMeasureUnitName, out string details )
    {
        details = Syntax.ThermalTransient.SourceMeasureUnitExists( session, sourceMeasureUnitName )
            ? string.Empty
           : $"Source meter name value of {sourceMeasureUnitName} is invalid {cc.isr.VI.Syntax.Tsp.Constants.LocalNode}.{sourceMeasureUnitName} is nil.";
        return string.IsNullOrWhiteSpace( details );
    }

    /// <summary>   Validates the source sense shunt. </summary>
    /// <remarks>   2026-03-24. </remarks>
    /// <param name="shunt">    The shunt. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool ValidateSourceSenseShunt( int shunt, out string details )
    {
        return shunt.IsBetweenInclusive( Properties.DriverSettings.Instance.MeterDefaults.SourceSenseShuntMinimum,
                Properties.DriverSettings.Instance.MeterDefaults.SourceSenseShuntMaximum, "Source-Sense shunt", out details );
    }
}
