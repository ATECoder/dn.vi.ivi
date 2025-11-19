// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary>   A specific trace logger implementation. </summary>
/// <remarks>   2024-07-30. </remarks>
public class SessionLogger : cc.isr.Logging.TraceLog.TraceLogger<SessionLogger>
{
    #region " construction and cleanup "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-07-30. </remarks>
    public SessionLogger() { }

    #endregion

    #region " singleton "

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static SessionLogger Instance => _instance.Value;

    private static readonly Lazy<SessionLogger> _instance = new( () => new SessionLogger(), true );

    #endregion

    #region " orlogger "

    /// <summary>   Reset the Serilog Log Logger to default and dispose the original if possible.. </summary>
    /// <remarks>   David, 2021-03-13. </remarks>
    public static new void CloseAndFlush()
    {
        cc.isr.Logging.Orlog.Orlogger.CloseAndFlush();
    }

    #endregion
}
