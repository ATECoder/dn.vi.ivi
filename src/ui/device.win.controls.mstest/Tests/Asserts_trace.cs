using System;

namespace cc.isr.VI.DeviceWinControls.Tests;

public sealed partial class Asserts
{
    private static cc.isr.Std.Tests.TraceMessageListener? TraceListener { get; set; }
    /// <summary>   Defines the trace listener. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="traceListener">    The trace listener. </param>
    public static void DefineTraceListener( cc.isr.Std.Tests.TraceMessageListener traceListener )
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

    /// <summary>   Asserts that a trace message should be queued. </summary>
    /// <remarks>   David, 2021-07-07. </remarks>
    public static void AssertTraceMessageShouldBeQueued()
    {
        if ( Asserts.TraceListener is null ) throw new ArgumentException( nameof( Asserts.TraceListener ) );

        if ( !AssertIfTraceErrorMessages ) return;

        string payload = "Device message";
        _ = cc.isr.VI.SessionLogger.Instance.LogWarning( payload );

        // with the new talker, the device identifies the following libraries:
        // 0x0100 core agnostic; 0x01006 vi device and 0x01026 Keithley Meter
        // so these test looks for the first warning
        int fetchNumber = 0;
        if ( TraceListener.Messages.TryGetValue( TraceEventType.Warning, out System.Collections.Generic.List<string>? traceMessages ) )
            fetchNumber += 1;

        if ( traceMessages is null )
            Assert.Fail( $"Failed tracing the warning message." );

        string traceMessage = traceMessages[0];

        if ( string.IsNullOrWhiteSpace( traceMessage ) )
            Assert.Fail( $"{payload} failed to trace message {fetchNumber}" );

        Assert.HasCount( 1, traceMessages, $"{payload} expected on warning message." );
        Assert.Contains( payload, traceMessage, $"'{payload}' should be contained in the trace message {traceMessage}" );
    }

}
