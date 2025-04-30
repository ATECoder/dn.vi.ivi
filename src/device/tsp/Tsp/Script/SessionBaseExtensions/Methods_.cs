namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

/// <summary>   A session base extension methods. </summary>
/// <remarks>   2025-04-10. </remarks>
[CLSCompliant( false )]
public static partial class SessionBaseExtensionMethods
{
    /// <summary>   Trace last action. </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="withPath">         (Optional) True to with path. </param>
    /// <param name="withFileName">     (Optional) True to with file name. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void TraceLastAction( string message, bool withPath = false, bool withFileName = true,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string category = withPath
            ? $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}"
            : withFileName
              ? $"{System.IO.Path.GetFileNameWithoutExtension( sourcePath )}.{memberName}.Line#{sourceLineNumber}"
              : $"{memberName}.Line#{sourceLineNumber}";

        System.Diagnostics.Trace.WriteLine( message, category );
    }

    /// <summary>   Console output member message. </summary>
    /// <remarks>   2025-04-28. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="withPath">         (Optional) True to with path. </param>
    /// <param name="withFileName">     (Optional) True to with file name. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static void ConsoleOutputMemberMessage( string message, bool withPath = false, bool withFileName = true,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string member = withPath
            ? $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}"
            : withFileName
              ? $"{System.IO.Path.GetFileNameWithoutExtension( sourcePath )}.{memberName}.Line#{sourceLineNumber}"
              : $"{memberName}.Line#{sourceLineNumber}";

        Console.Out.WriteLine( $"{member}:\r\n\t{message}" );
    }
}
