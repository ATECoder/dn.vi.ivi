namespace cc.isr.VI.Device.Tests;
/// <summary>
/// Test manager for <see cref="VisaSessionBase"/> and <see cref="SubsystemBase"/> Tests.
/// </summary>
/// <remarks>
/// David, 2019-12-12 <para>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved. </para><para>
/// Licensed under The MIT License.</para>
/// </remarks>
public sealed partial class Asserts
{
    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    private Asserts()
    {
    }

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
        Assert.IsNotNull( Asserts.TraceListener, $"{nameof( Asserts.TraceListener )} is not defined." );
        if ( Asserts.AssertIfTraceErrorMessages )
            Assert.IsFalse( Asserts.TraceListener.Any( TraceEventType.Error ),
                $"{nameof( Asserts.TraceListener )} should have no {TraceEventType.Error} messages:" +
                $"\n{string.Join( "\n", [.. Asserts.TraceListener!.Messages[TraceEventType.Error].ToArray()] )}" );
        if ( Asserts.AssertIfTraceWarningMessages )
            Assert.IsFalse( Asserts.TraceListener.Any( TraceEventType.Warning ),
            $"{nameof( Asserts.TraceListener )} should have no {TraceEventType.Warning} messages:" +
            $"\n{string.Join( "\n", [.. Asserts.TraceListener!.Messages[TraceEventType.Warning].ToArray()] )}" );
    }

}
