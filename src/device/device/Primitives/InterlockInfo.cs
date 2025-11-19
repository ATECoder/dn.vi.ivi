// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary> Information about the interlock. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-02-16, 4.0.5890. </para>
/// </remarks>
/// <remarks> Constructor. </remarks>
/// <param name="interlockNumber"> The interlock number. </param>
public class InterlockInfo( int interlockNumber ) : object()
{
    /// <summary> Gets the interlock number. </summary>
    /// <value> The number. </value>
    public int Number { get; private set; } = interlockNumber;

    /// <summary> Gets the state. </summary>
    /// <value> The state. </value>
    public InterlockState State { get; set; }

    /// <summary> Gets the is engaged. </summary>
    /// <value> The is engaged. </value>
    public bool IsEngaged => this.State == InterlockState.Engaged;
}
/// <summary> Collection of interlocks. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-02-16, 4.0.5890. </para>
/// </remarks>
public class InterlockCollection : System.Collections.ObjectModel.KeyedCollection<int, InterlockInfo>
{
    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns> The key for the specified element. </returns>
    protected override int GetKeyForItem( InterlockInfo item )
    {
        return item is null ? throw new ArgumentNullException( nameof( item ) ) : item.Number;
    }

    /// <summary> Adds interlockNumber. </summary>
    /// <param name="interlockNumber"> The interlock number to add. </param>
    /// <returns> An InterlockInfo. </returns>
    public InterlockInfo Add( int interlockNumber )
    {
        InterlockInfo value = new( interlockNumber );
        this.Add( value );
        return value;
    }

    /// <summary> Updates the interlock state described by state. </summary>
    /// <param name="state"> The state. </param>
    public void UpdateInterlockState( int state )
    {
        foreach ( InterlockInfo interlockInformation in this )
            interlockInformation.State = (interlockInformation.Number & state) == interlockInformation.Number ? InterlockState.Engaged : InterlockState.Open;
    }

    /// <summary> Gets the sentinel indicating if all interlocks are engaged. </summary>
    /// <value> The are all interlocks engaged. </value>
    public bool AreAllEngaged
    {
        get
        {
            bool affirmative = true;
            foreach ( InterlockInfo interlockInformation in this )
                affirmative = affirmative && interlockInformation.IsEngaged;
            return affirmative;
        }
    }

    /// <summary> Gets the open interlocks. </summary>
    /// <value> The open interlocks. </value>
    public IList<int> OpenInterlocks
    {
        get
        {
            List<int> l = [];
            foreach ( InterlockInfo interlockInformation in this )
            {
                if ( !interlockInformation.IsEngaged )
                {
                    l.Add( interlockInformation.Number );
                }
            }

            return l;
        }
    }
}
/// <summary> Values that represent interlock states. </summary>
public enum InterlockState
{
    /// <summary> An enum constant representing the open] option. </summary>
    [System.ComponentModel.Description( "Open" )]
    Open,

    /// <summary> An enum constant representing the engaged option. </summary>
    [System.ComponentModel.Description( "Engaged" )]
    Engaged
}
