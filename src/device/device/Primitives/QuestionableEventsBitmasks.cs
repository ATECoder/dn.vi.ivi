// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

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
public class QuestionableEventsBitmaskDictionary : BitmasksDictionary
{
    /// <summary> Default constructor. </summary>
    public QuestionableEventsBitmaskDictionary() : base()
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
    protected QuestionableEventsBitmaskDictionary( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context ) : base( info, context )
    {
    }

    /// <summary> Adds key. </summary>
    /// <param name="key">     The bitmask key. </param>
    /// <param name="bitmask"> The bitmask. </param>
    public void Add( QuestionableEventBitmaskKey key, int bitmask )
    {
        this.Add( ( int ) key, bitmask );
    }

    /// <summary> Adds key. </summary>
    /// <param name="key">            The bitmask key. </param>
    /// <param name="bitmask">        The bitmask. </param>
    /// <param name="excludeFromAll"> True to exclude, false to include from all. </param>
    public void Add( QuestionableEventBitmaskKey key, int bitmask, bool excludeFromAll )
    {
        this.Add( ( int ) key, bitmask, excludeFromAll );
    }

    /// <summary> QueryEnum if 'status' bas any bit on. </summary>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> <c>true</c> if any bit on; otherwise <c>false</c> </returns>
    public bool IsAnyBitOn( int status, QuestionableEventBitmaskKey key )
    {
        return this.IsAnyBitOn( status, ( int ) key );
    }

    /// <summary> QueryEnum if all bitmask bits in 'status' are on. </summary>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> A Boolean. </returns>
    public bool AreAllBitsOn( int status, QuestionableEventBitmaskKey key )
    {
        return this.AreAllBitsOn( status, ( int ) key );
    }

    /// <summary> Return the masked status value. </summary>
    /// <param name="status"> The status. </param>
    /// <param name="key">    The bit mask key. </param>
    /// <returns> An Integer? </returns>
    public int MaskedValue( int status, QuestionableEventBitmaskKey key )
    {
        return this.MaskedValue( status, ( int ) key );
    }

    /// <summary> QueryEnum if 'status' bas any bit on. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> <c>true</c> if any bit on; otherwise <c>false</c> </returns>
    public bool IsAnyBitOn( QuestionableEventBitmaskKey key )
    {
        return this.IsAnyBitOn( this.Status, key );
    }

    /// <summary> QueryEnum if all bitmask bits in 'status' are on. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> A Boolean. </returns>
    public bool AreAllBitsOn( QuestionableEventBitmaskKey key )
    {
        return this.AreAllBitsOn( this.Status, key );
    }

    /// <summary> Return the masked status value. </summary>
    /// <param name="key"> The bit mask key. </param>
    /// <returns> An Integer? </returns>
    public int MaskedValue( QuestionableEventBitmaskKey key )
    {
        return this.MaskedValue( this.Status, key );
    }
}
/// <summary> Values that represent questionable event bitmask keys. </summary>
public enum QuestionableEventBitmaskKey
{
    /// <summary> The questionable event command warning bitmask key value. </summary>
    [Description( "Command Warning" )]
    CommandWarning,

    /// <summary> The questionable event calibration summary bitmask key value. </summary>
    [Description( "Calibration Summary" )]
    CalibrationSummary,

    /// <summary> The questionable event temperature summary bitmask key value. </summary>
    [Description( "Temperature Summary" )]
    TemperatureSummary
}
