namespace cc.isr.VI.Tsp.K2600.Ttm;

public partial class MeterMain : DeviceUnderTestElementBase, IEquatable<MeterMain>
{

    /// <summary> Validates the Post Transient Delay described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value">   The value. </param>
    /// <param name="details"> [out] The details. </param>
    /// <returns> <c>true</c> if value is in range; otherwise, <c>false</c>. </returns>
    public static bool ValidatePostTransientDelay( double value, out string details )
    {
        bool affirmative = value >= ( double ) Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayMinimum
            && value <= ( double ) Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayMaximum;
        details = affirmative
            ? string.Empty
            : value < ( double ) Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayMinimum
                ? $"Post Transient Delay value of {value} is lower than the minimum of {Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayMinimum}."
                : $"Post Transient Delay value of {value} is higher than the maximum of {Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayMaximum}.";

        return affirmative;
    }

    /// <summary>   Validates the legacy driver. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool ValidateLegacyDriver( int value, out string details )
    {
        bool affirmative = value is >= 0 and <= 1;
        details = affirmative
            ? string.Empty
            : value < 0
                ? $"Legacy Driver value of {value} is lower than the minimum of 0."
                : $"Legacy Driver value of {value} is higher than the maximum of 1.";

        return affirmative;
    }

    /// <summary>   Validates the contact threshold. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <param name="value">    The value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool ValidateContactThreshold( int value, out string details )
    {
        bool affirmative = value is >= 10 and <= 999;
        details = affirmative
            ? string.Empty
            : value < 0
                ? $"Contact Check Threshold value of {value} is lower than the minimum of 10."
                : $"Contact Check Threshold value of {value} is higher than the maximum of 999.";

        return affirmative;
    }

    /// <summary>   Validates the contact check options. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <param name="contactOption">    The contact option value. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool ValidateContactCheckOptions( int contactOption, out string details )
    {
        bool affirmative = contactOption is 1 or 3 or 5 or 7;
        details = affirmative
            ? string.Empty
           : $"Contact Check Option value of {contactOption} must be either 1, 3, 5 or 7.";

        return affirmative;
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

}
