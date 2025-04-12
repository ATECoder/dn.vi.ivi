using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

/// <summary>   A session base methods. </summary>
/// <remarks>   2025-04-10. </remarks>
[CLSCompliant( false )]
public static partial class Methods
{
    /// <summary>   Trace last action. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void TraceLastAction( string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        System.Diagnostics.Trace.WriteLine( message, $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}" );
    }

    /// <summary>   Trace last action. </summary>
    /// <remarks>   2025-04-11. </remarks>
    /// <param name="session">          The session to act on. </param>
    /// <param name="message">          The message. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void TraceLastAction( this SessionBase session, string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        System.Diagnostics.Trace.WriteLine( message, $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}" );
        session.SetLastAction( message );
    }

}
