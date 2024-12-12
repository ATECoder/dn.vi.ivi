namespace cc.isr.VI.Tsp.K2600;
/// <summary>   A 2600 syntax. </summary>
/// <remarks>   2024-11-14. </remarks>
public static class K2600Syntax
{
    /// <summary>   Gets or sets the default source meter name. </summary>
    /// <value> The default source meter name. </value>
    public static string DefaultSourceMeterName { get; set; } = "smua";

    /// <summary>   Gets or sets a list of names of the source measure units. </summary>
    /// <value> A list of names of the source measure units. </value>
    public static string SourceMeasureUnitNames { get; set; } = "smua,smub";
}
