namespace cc.isr.VI.Device.Tsp.Tests;

/// <summary>
/// Test manager for <see cref="VisaSessionBase"/> and <see cref="SubsystemBase"/> Tests.
/// </summary>
/// <remarks>
/// David, 2019-12-12 <para>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved. </para><para>
/// Licensed under The MIT License.</para>
/// </remarks>
public static partial class Asserts
{
    private static cc.isr.Std.Listeners.TraceMessageListener? TraceListener { get; set; }
    /// <summary>   Defines the trace listener. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="traceListener">    The trace listener. </param>
    public static void DefineTraceListener( cc.isr.Std.Listeners.TraceMessageListener traceListener )
    {
        Asserts.TraceListener = traceListener;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the assert if error messages were traced.
    /// </summary>
    /// <remarks>
    /// 20211111: vs 16.11.6: This addresses a bug in failure of the trace listener to filter
    /// messages.
    /// </remarks>
    /// <value> True if assert trace error message queue is enabled. Otherwise, false. </value>
    public static bool AssertIfTraceErrorMessages { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the assert if warning messages were traced.
    /// </summary>
    /// <remarks>
    /// 20211111: vs 16.11.6: This addresses a bug in failure of the trace listener to filter
    /// messages.
    /// </remarks>
    /// <value> True if assert trace message warning queue is enabled. Otherwise, false. </value>
    public static bool AssertIfTraceWarningMessages { get; set; } = true;

    /// <summary>   Assert non empty warning or error messages in the trace listener. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    public static void AssertMessageQueue()
    {
        Assert.IsNotNull( Asserts.TraceListener, nameof( Asserts.TraceListener ) );
        if ( Asserts.AssertIfTraceErrorMessages )
            Assert.IsFalse( Asserts.TraceListener.Any( TraceEventType.Error ),
                $"{nameof( Asserts.TraceListener )} should have no {TraceEventType.Error} messages:" +
                $"\n{string.Join( "\n", [.. Asserts.TraceListener!.Messages[TraceEventType.Error].ToArray()] )}" );
        if ( Asserts.AssertIfTraceWarningMessages )
            Assert.IsFalse( Asserts.TraceListener.Any( TraceEventType.Warning ),
            $"{nameof( Asserts.TraceListener )} should have no {TraceEventType.Warning} messages:" +
            $"\n{string.Join( "\n", [.. Asserts.TraceListener!.Messages[TraceEventType.Warning].ToArray()] )}" );
    }

    /// <summary>   Gets temporary path under the '~cc.isr` and this class namespace folder name. </summary>
    /// <remarks>   2025-06-03. </remarks>
    /// <param name="firstSubfolderName">   (Optional) [CallerMemberName] Name of the second
    ///                                     subfolder. </param>
    /// <param name="secondSubfolderName">  (Optional) Name of the second subfolder. </param>
    /// <returns>   The temporary path. </returns>
    public static string GetTempPath( string[] subfolders )
    {
        string tempPath = Path.Combine( Path.GetTempPath(), "~cc.isr", "VI", "Device", "Tsp" );

        foreach ( string subfolder in subfolders )
        {
            if ( !string.IsNullOrWhiteSpace( subfolder ) )
                tempPath = Path.Combine( tempPath, subfolder );
        }
        _ = System.IO.Directory.CreateDirectory( tempPath );
        return tempPath;
    }
}
