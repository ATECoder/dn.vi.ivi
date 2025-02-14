using System;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

/// <summary>   Asserts for testing the legacy driver. </summary>
/// <remarks>   2024-11-01. </remarks>
internal static partial class Asserts
{
    /// <summary>   Gets or sets the legacy driver. </summary>
    /// <value> The legacy driver. </value>
    public static int LegacyDriver { get; set; } = 1;

    /// <summary> Logs an iterator. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public static void LogIT( string value )
    {
        Console.Out.WriteLine( value );
    }
}
