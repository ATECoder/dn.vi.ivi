using System.ComponentModel;

namespace cc.isr.VI;
/// <summary>
/// A dictionary of operation events bitmasks capable of detecting bits status.
/// </summary>
/// <remarks>
/// (c) 2019 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2019-12-19 </para>
/// </remarks>
public class OperationEventsBitmaskDictionary : BitmasksDictionary
{
    /// <summary> Default constructor. </summary>
    public OperationEventsBitmaskDictionary() : base()
    {
    }

    /// <summary> Initializes a new instance of the class with serialized data. </summary>
    /// <param name="info">    The <see cref="System.Runtime.Serialization.SerializationInfo" />
    /// that holds the serialized object data about the exception being
    /// thrown. </param>
    /// <param name="context"> The <see cref="System.Runtime.Serialization.StreamingContext" />
    /// that contains contextual information about the source or destination.
    /// </param>
#if NET5_0_OR_GREATER
#pragma warning disable CA1041 // Provide ObsoleteAttribute message
[Obsolete( DiagnosticId = "SYSLIB0051" )] // add this attribute to the serialization ctor
#pragma warning restore CA1041 // Provide ObsoleteAttribute message
#endif
    protected OperationEventsBitmaskDictionary( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context ) : base( info, context )
    {
    }

    /// <summary> Adds key. </summary>
    /// <param name="key">     The bitmask key. </param>
    /// <param name="bitmask"> The bitmask. </param>
    public void Add( OperationEventBitmaskKey key, int bitmask )
    {
        this.Add( ( int ) key, bitmask );
    }

    /// <summary> Adds key. </summary>
    /// <param name="key">            The bitmask key. </param>
    /// <param name="bitmask">        The bitmask. </param>
    /// <param name="excludeFromAll"> True to exclude, false to include from all. </param>
    public void Add( OperationEventBitmaskKey key, int bitmask, bool excludeFromAll )
    {
        this.Add( ( int ) key, bitmask, excludeFromAll );
    }

    /// <summary> QueryEnum if 'status' bas any bit on. </summary>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> <c>true</c> if any bit on; otherwise <c>false</c> </returns>
    public bool IsAnyBitOn( int status, OperationEventBitmaskKey key )
    {
        return this.IsAnyBitOn( status, ( int ) key );
    }

    /// <summary> QueryEnum if all bitmask bits in 'status' are on. </summary>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> A Boolean. </returns>
    public bool AreAllBitsOn( int status, OperationEventBitmaskKey key )
    {
        return this.AreAllBitsOn( status, ( int ) key );
    }

    /// <summary> Return the masked status value. </summary>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> An Integer? </returns>
    public int MaskedValue( int status, OperationEventBitmaskKey key )
    {
        return this.MaskedValue( status, ( int ) key );
    }

    /// <summary> QueryEnum if 'status' bas any bit on. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> <c>true</c> if any bit on; otherwise <c>false</c> </returns>
    public bool IsAnyBitOn( OperationEventBitmaskKey key )
    {
        return this.IsAnyBitOn( this.Status, key );
    }

    /// <summary> QueryEnum if all bitmask bits in 'status' are on. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> A Boolean. </returns>
    public bool AreAllBitsOn( OperationEventBitmaskKey key )
    {
        return this.AreAllBitsOn( this.Status, key );
    }

    /// <summary> Return the masked status value. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> An Integer? </returns>
    public int MaskedValue( OperationEventBitmaskKey key )
    {
        return this.MaskedValue( this.Status, key );
    }
}
/// <summary> Values that represent operation event bitmask keys. </summary>
public enum OperationEventBitmaskKey
{
    /// <summary> The operation Event Arming bitmask key value. </summary>
    [Description( "Arming" )]
    Arming,

    /// <summary> The operation Event idle bitmask key value. </summary>
    [Description( "Idle" )]
    Idle,

    /// <summary> The operation Event Triggering bitmask key value. </summary>
    [Description( "Triggering" )]
    Triggering,

    /// <summary> The operation Event Measuring bitmask key value. </summary>
    [Description( "Measuring" )]
    Measuring,

    /// <summary> The operation Event Setting bitmask key value. </summary>
    [Description( "Setting" )]
    Setting,

    /// <summary> The operation Event Calibrating bitmask key value. </summary>
    [Description( "Calibrating" )]
    Calibrating,

    /// <summary> The operation Event Prompts Enabled bitmask key value. </summary>
    [Description( "Prompts Enabled" )]
    PromptsEnabled,

    /// <summary> The operation Event User bitmask key value. </summary>
    [Description( "User" )]
    User,

    /// <summary> The operation Event Program Running bitmask key value. </summary>
    [Description( "Program Running" )]
    ProgramRunning
}
