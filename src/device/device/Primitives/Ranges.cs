using cc.isr.Std.Primitives;

namespace cc.isr.VI;

/// <summary> A ranges. </summary>
/// <remarks> (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para> </remarks>
public static class Ranges
{
    /// <summary>
    /// Gets a new instance of the default full range bound by the minimum decimal value and the
    /// maximum decimal value minus 1E+14.
    /// </summary>
    /// <value> The full range. </value>
    public static RangeR FullRange =>
            // round off of the decimal range cause an exception when converting back. 
            new( ( double ) decimal.MinValue, ( double ) decimal.MaxValue - 10000000000000.0d );

    /// <summary> Gets the non-negative range bound by maximum decimal value minus 1E+14. </summary>
    /// <value> The non-negative full range. </value>
    public static RangeR NonnegativeFullRange => new( 0d, ( double ) decimal.MaxValue - 10000000000000.0d );

    /// <summary> Gets the Standard aperture range. </summary>
    /// <value> The Standard aperture range. </value>
    public static RangeR StandardApertureRange => new( 0.0001d, 1d );

    /// <summary> Gets the Standard filter window range. </summary>
    /// <value> The Standard filter window range. </value>
    public static RangeR StandardFilterWindowRange => new( 0.001d, 0.1d );

    /// <summary> Gets the default filter count range. </summary>
    /// <value> The default filter count range. </value>
    public static RangeI StandardFilterCountRange => new( 0, 100 );

    /// <summary> Gets the Standard power line cycles range. </summary>
    /// <value> The Standard power line cycles range. </value>
    public static RangeR StandardPowerLineCyclesRange
    {
        get
        {
            const double lineFrequency = 50d;
            return new RangeR( lineFrequency * StandardApertureRange.Min, lineFrequency * StandardApertureRange.Max );
        }
    }

}
