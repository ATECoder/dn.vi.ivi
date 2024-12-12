using System.ComponentModel;

namespace cc.isr.VI;
/// <summary>
/// A dictionary of measurement events bitmasks capable of detecting bits status.
/// </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-12-19 </para>
/// </remarks>
public class MeasurementEventsBitmaskDictionary : BitmasksDictionary
{
    /// <summary> Default constructor. </summary>
    public MeasurementEventsBitmaskDictionary() : base()
    {
    }

    /// <summary> Initializes a new instance of the class with serialized data. </summary>
    /// <param name="info">    The <see cref="System.Runtime.Serialization.SerializationInfo" />
    /// that holds the serialized object data about the exception being
    /// thrown. </param>
    /// <param name="context"> The <see cref="System.Runtime.Serialization.StreamingContext" />
    /// that contains contextual information about the source or destination.
    /// </param>
#if NET8_0_OR_GREATER
#pragma warning disable CA1041 // Provide ObsoleteAttribute message
[Obsolete( DiagnosticId = "SYSLIB0051" )] // add this attribute to the serialization ctor
#pragma warning restore CA1041 // Provide ObsoleteAttribute message
#endif
    protected MeasurementEventsBitmaskDictionary( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context ) : base( info, context )
    {
    }

    /// <summary> Adds key. </summary>
    /// <param name="key">     The bitmask key. </param>
    /// <param name="bitmask"> The bitmask. </param>
    public void Add( MeasurementEventBitmaskKey key, int bitmask )
    {
        this.Add( ( int ) key, bitmask );
    }

    /// <summary> Adds key. </summary>
    /// <param name="key">            The bitmask key. </param>
    /// <param name="bitmask">        The bitmask. </param>
    /// <param name="excludeFromAll"> True to exclude, false to include from all. </param>
    public void Add( MeasurementEventBitmaskKey key, int bitmask, bool excludeFromAll )
    {
        this.Add( ( int ) key, bitmask, excludeFromAll );
    }

    /// <summary> QueryEnum if 'status' bas any bit on. </summary>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> <c>true</c> if any bit on; otherwise <c>false</c> </returns>
    public bool IsAnyBitOn( int status, MeasurementEventBitmaskKey key )
    {
        return this.IsAnyBitOn( status, ( int ) key );
    }

    /// <summary> QueryEnum if all bitmask bits in 'status' are on. </summary>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> A Boolean. </returns>
    public bool AreAllBitsOn( int status, MeasurementEventBitmaskKey key )
    {
        return this.AreAllBitsOn( status, ( int ) key );
    }

    /// <summary> Return the masked status value. </summary>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> An Integer? </returns>
    public int MaskedValue( int status, MeasurementEventBitmaskKey key )
    {
        return this.MaskedValue( status, ( int ) key );
    }

    /// <summary> QueryEnum if 'status' bas any bit on. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> <c>true</c> if any bit on; otherwise <c>false</c> </returns>
    public bool IsAnyBitOn( MeasurementEventBitmaskKey key )
    {
        return this.IsAnyBitOn( this.Status, key );
    }

    /// <summary> QueryEnum if all bitmask bits in 'status' are on. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> A Boolean. </returns>
    public bool AreAllBitsOn( MeasurementEventBitmaskKey key )
    {
        return this.AreAllBitsOn( this.Status, key );
    }

    /// <summary> Return the masked status value. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> An Integer? </returns>
    public int MaskedValue( MeasurementEventBitmaskKey key )
    {
        return this.MaskedValue( this.Status, key );
    }
}
/// <summary> Values that represent measurement event bitmask key values. </summary>
public enum MeasurementEventBitmaskKey
{
    /// <summary> The measurement reading overflow bitmask key value. </summary>
    [Description( "Reading Overflow" )]
    ReadingOverflow,

    /// <summary> The measurement low limit 1 bitmask key value. </summary>
    [Description( "Low Limit 1" )]
    LowLimit1,

    /// <summary> The measurement high limit 1 bitmask key value. </summary>
    [Description( "High Limit 1" )]
    HighLimit1,

    /// <summary> The measurement low limit 2 bitmask key value. </summary>
    [Description( "Low Limit 2" )]
    LowLimit2,

    /// <summary> The measurement status high limit 2 bitmask key value. </summary>
    [Description( "High Limit 2" )]
    HighLimit2,

    /// <summary> The measurement status reading available bitmask key value. </summary>
    [Description( "Reading Available" )]
    ReadingAvailable,

    /// <summary> The measurement status buffer available bitmask key value. </summary>
    [Description( "Buffer Available" )]
    BufferAvailable,

    /// <summary> The measurement status buffer half full bitmask key value. </summary>
    [Description( "Buffer Half Full" )]
    BufferHalfFull,

    /// <summary> The measurement status buffer full bitmask key value. </summary>
    [Description( "Buffer Full" )]
    BufferFull,

    /// <summary> The measurement status buffer pre-triggered bitmask key value. </summary>
    [Description( "Buffer Pre-Triggered" )]
    BufferPreTriggered,

    /// <summary> The measurement questionable  bitmask key value;
    /// measurement status is questionable if bit value is on. </summary>
    [Description( "Questionable" )]
    Questionable,

    /// <summary> The measurement origin bitmask key value specifying
    /// A/D converter from which reading originated.
    /// For the 7510 this will be always 0 (main) or 2 (digitizer). </summary>
    [Description( "Origin" )]
    Origin,

    /// <summary> The measurement terminal bitmask key value; front is 1 rear is 0. </summary>
    [Description( "Measurement Terminal" )]
    MeasurementTerminal,

    /// <summary> The first reading in Group bitmask key value. </summary>
    [Description( "First Reading In Group" )]
    FirstReadingInGroup,

    /// <summary> Indicates a buffer reading with status only value. </summary>
    [Description( "Status only measurement" )]
    StatusOnly,

    /// <summary> Summary of all limit, overflow and other failure bits. </summary>
    [Description( "Failure Summary" )]
    FailuresSummary
}
