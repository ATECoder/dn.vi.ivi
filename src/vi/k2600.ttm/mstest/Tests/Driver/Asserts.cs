using System;

namespace cc.isr.VI.Tsp.K2600.Ttm.Tests.Driver;

/// <summary>   An asserts. </summary>
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

    /// <summary>   Assert orphan messages or device errors. </summary>
    /// <remarks>   2024-11-11. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="memberName">   (Optional) Name of the member. </param>
    public static void AssertOrphanMessagesOrDeviceErrors( Pith.SessionBase? session, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "" )
    {
        Assert.IsNotNull( session, $"{nameof( session )} must not be null." );
        Assert.IsTrue( session.IsDeviceOpen, $"VISA session to '{nameof( session.ResourceNameCaption )}' must be open." );
        string orphanMessages = session.ReadLines( session.StatusReadDelay, TimeSpan.FromMilliseconds( 100 ), false );
        Assert.IsTrue( string.IsNullOrWhiteSpace( orphanMessages ), $"The following messages were left on the output buffer after {memberName}:/n{orphanMessages}" );
        session.ThrowDeviceExceptionIfError( failureMessage: $"{nameof( Pith.SessionBase.ReadLines )} after {memberName} failed" );
    }

}
