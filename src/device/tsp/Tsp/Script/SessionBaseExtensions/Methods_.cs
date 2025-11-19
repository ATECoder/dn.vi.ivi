// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

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
    public static string TraceLastAction( string message, bool withPath = false, bool withFileName = true,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string memberInfo = withPath
            ? $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}"
            : withFileName
              ? $"{System.IO.Path.GetFileNameWithoutExtension( sourcePath )}.{memberName}.Line#{sourceLineNumber}"
              : $"{memberName}.Line#{sourceLineNumber}";

        System.Diagnostics.Trace.WriteLine( message, memberInfo );
        return $"{memberInfo}:\r\n\t{message}";
    }

    /// <summary>   Console output member message. </summary>
    /// <remarks>   2025-04-28. </remarks>
    /// <param name="message">          The message. </param>
    /// <param name="withPath">         (Optional) True to with path. </param>
    /// <param name="withFileName">     (Optional) True to with file name. </param>
    /// <param name="memberName">       (Optional) Name of the member. </param>
    /// <param name="sourcePath">       (Optional) Full pathname of the source file. </param>
    /// <param name="sourceLineNumber"> (Optional) Source line number. </param>
    public static string ConsoleOutputMemberMessage( string message, bool withPath = false, bool withFileName = true,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0 )
    {
        string memberInfo = withPath
            ? $"[{sourcePath}].{memberName}.Line#{sourceLineNumber}"
            : withFileName
              ? $"{System.IO.Path.GetFileNameWithoutExtension( sourcePath )}.{memberName}.Line#{sourceLineNumber}"
              : $"{memberName}.Line#{sourceLineNumber}";

        message = $"{memberInfo}:\r\n\t{message}";
        Console.Out.WriteLine( message );
        return message;
    }
}
