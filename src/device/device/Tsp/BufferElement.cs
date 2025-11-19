namespace cc.isr.VI;

/// <summary> A buffer element. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-01-09 </para>
/// </remarks>
public class BufferElement
{
    /// <summary> Gets or sets the value. </summary>
    /// <value> The value. </value>
    public double Value { get; set; }

    /// <summary> Gets or sets the timestamp. </summary>
    /// <value> The timestamp. </value>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary> Gets or sets the relative time. </summary>
    /// <value> The relative time. </value>
    public TimeSpan RelativeTime { get; set; }

    /// <summary> Gets or sets the status. </summary>
    /// <value> The status. </value>
    public BufferElementStatusBits Status { get; set; }
}
/// <summary> Enumerates the status bits as defined in reading buffers. </summary>
[Flags()]
public enum BufferElementStatusBits
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not defined" )]
    None = 0,

    /// <summary>0x02 over temperature condition.</summary>
    [System.ComponentModel.Description( "Over Temp" )]
    OverTemp = 2,

    /// <summary>0x04 measure was auto ranged.</summary>
    [System.ComponentModel.Description( "Auto Range Measure" )]
    AutoRangeMeasure = 4,

    /// <summary>0x08 source was auto ranged.</summary>
    [System.ComponentModel.Description( "Auto Range Source" )]
    AutoRangeSource = 8,

    /// <summary> 0x10 4W (remote) sense mode. </summary>
    [System.ComponentModel.Description( "Four Wire" )]
    FourWire = 16,

    /// <summary> 0x20 relative applied to reading. </summary>
    [System.ComponentModel.Description( "Relative" )]
    Relative = 32,

    /// <summary>0x40 source function in compliance.</summary>
    [System.ComponentModel.Description( "Compliance" )]
    Compliance = 64,

    /// <summary>0x80 reading was filtered.</summary>
    [System.ComponentModel.Description( "Filtered" )]
    Filtered = 128
}
