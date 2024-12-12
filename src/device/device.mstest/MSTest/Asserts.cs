namespace cc.isr.VI.Device.MSTest;
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

    private static cc.isr.Std.Tests.TraceMessageListener? TraceListener { get; set; }
    /// <summary>   Defines the trace listener. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    /// <param name="traceListener">    The trace listener. </param>
    public static void DefineTraceListener( cc.isr.Std.Tests.TraceMessageListener traceListener )
    {
        Asserts.TraceListener = traceListener;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the asset if error messages were traced.
    /// </summary>
    /// <remarks>
    /// 20211111: vs 16.11.6: This addresses a bug in failure of the trace listener to filter
    /// messages.
    /// </remarks>
    /// <value> True if asset message queue enabled, false if not. </value>
    public static bool AssetIfTraceErrorMessages { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the asset if warning messages were traced.
    /// </summary>
    /// <remarks>
    /// 20211111: vs 16.11.6: This addresses a bug in failure of the trace listener to filter
    /// messages.
    /// </remarks>
    /// <value> True if asset message queue enabled, false if not. </value>
    public static bool AssetIfTraceWarningMessages { get; set; } = true;

    /// <summary>   Assert non empty warning or error messages in the trace listener. </summary>
    /// <remarks>   David, 2021-06-29. </remarks>
    public static void AssertMessageQueue()
    {
        Assert.IsNotNull( Asserts.TraceListener, nameof( Asserts.TraceListener ) );
        if ( Asserts.AssetIfTraceErrorMessages )
            Assert.IsFalse( Asserts.TraceListener.Any( TraceEventType.Error ),
                $"{nameof( Asserts.TraceListener )} should have no {TraceEventType.Error} messages:" +
                $"\n{string.Join( "\n", [.. Asserts.TraceListener!.Messages[TraceEventType.Error].ToArray()] )}" );
        if ( Asserts.AssetIfTraceWarningMessages )
            Assert.IsFalse( Asserts.TraceListener.Any( TraceEventType.Warning ),
            $"{nameof( Asserts.TraceListener )} should have no {TraceEventType.Warning} messages:" +
            $"\n{string.Join( "\n", [.. Asserts.TraceListener!.Messages[TraceEventType.Warning].ToArray()] )}" );
    }

}
