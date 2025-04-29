using System;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy.Tests;

/// <summary>   Asserts for testing the legacy driver. </summary>
/// <remarks>   2024-11-01. </remarks>
internal static partial class Asserts
{
    /// <summary>   Gets or sets the legacy driver. </summary>
    /// <value> The legacy driver. </value>
    public static int LegacyDriver { get; set; } = 1;

    /// <summary>   Output full member. </summary>
    /// <remarks>   2025-04-28. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void OutputFullMember( string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        Console.WriteLine( $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}:\r\n\t{message}" );
    }

    /// <summary>   Output member. </summary>
    /// <remarks>   2025-04-28. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void OutputMember( string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        Console.WriteLine( $"{memberName}.Line#{sourceLineNumber}::\r\n\t{message}" );
    }
}
