// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary> Defines a System Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-01-13 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class SlotSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
    /// </summary>
    /// <param name="slotNumber">      The slot number. </param>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// Subsystem</see>. </param>
    protected SlotSubsystemBase( int slotNumber, StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.SlotNumber = slotNumber;
        this.EnumerateInterlocksThis( 2 );
    }

    #endregion

    #region " slot number "

    /// <summary> Gets or sets the slot number. </summary>
    /// <value> The slot number. </value>
    public int SlotNumber { get; private set; }

    #endregion

    #region " exists "

    /// <summary> The is slot exists. </summary>

    /// <summary> Gets or sets (Protected) the Slot existence indicator. </summary>
    /// <value> The Slot existence indicator. </value>
    public bool? IsSlotExists
    {
        get;

        protected set
        {
            if ( !Equals( value, this.IsSlotExists ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    // TspSyntax.Slot.SubsystemNameFormat

    /// <summary> Gets or sets the slot exists query command format. </summary>
    /// <value> The slot exists query command format. </value>
    protected virtual string SlotExistsQueryCommandFormat { get; set; } = string.Empty;

    /// <summary> Queries slot exists. </summary>
    /// <returns> The slot exists. </returns>
    public bool? QuerySlotExists()
    {
        this.IsSlotExists = !this.Session.IsNil( string.Format( this.SlotExistsQueryCommandFormat, this.SlotNumber ) );
        if ( !this.IsSlotExists.GetValueOrDefault( false ) )
        {
            this.SupportsInterlock = false;
            this.InterlocksState = 0;
        }

        return this.IsSlotExists;
    }

    #endregion

    #region " interlocks "

    /// <summary> Gets or sets the interlocks. </summary>
    /// <value> The interlocks. </value>
    public InterlockCollection? Interlocks { get; private set; }

    /// <summary> Enumerate interlocks. </summary>
    /// <param name="interlockCount"> Number of interlocks. </param>
    private void EnumerateInterlocksThis( int interlockCount )
    {
        this.Interlocks = [];
        for ( int i = 1, loopTo = interlockCount; i <= loopTo; i++ )
            _ = this.Interlocks.Add( i );
    }

    #region " supports interlock "

    /// <summary> The supports interlock. </summary>

    /// <summary> Gets or sets the supports interlock. </summary>
    /// <value> The supports interlock. </value>
    public bool? SupportsInterlock
    {
        get;

        protected set
        {
            if ( !Equals( value, this.SupportsInterlock ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    // TspSyntax.Slot.InterlockStateFormat

    /// <summary> Gets or sets the interlock state name query format. </summary>
    /// <value> The interlock state name query format. </value>
    protected virtual string InterlockStateNameQueryFormat { get; set; } = string.Empty;

    /// <summary> Queries supports interlock. </summary>
    /// <returns> The supports interlock. </returns>
    public bool? QuerySupportsInterlock()
    {
        if ( !this.IsSlotExists.HasValue )
        {
            _ = this.QuerySlotExists();
        }

        if ( this.IsSlotExists.GetValueOrDefault( false ) )
        {
            this.SupportsInterlock = !this.Session.IsNil( this.InterlockStateNameQueryFormat, this.SlotNumber );
        }

        return this.SupportsInterlock;
    }

    #endregion

    #region " interlocak state "

    /// <summary> State of the interlocks. </summary>

    /// <summary> Gets or sets the state of the interlocks. </summary>
    /// <value> The interlock state. </value>
    public int? InterlocksState
    {
        get;

        protected set
        {
            if ( !Equals( value, this.InterlocksState ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the interlock state query command format. </summary>
    /// <remarks> see TspSyntax.Slot.InterlockStateQueryCommandFormat. </remarks>
    /// <value> The interlock state query command format. </value>
    protected virtual string InterlockStateQueryCommandFormat { get; set; } = string.Empty;

    /// <summary> Queries interlocks state. </summary>
    /// <returns> The interlock state. </returns>
    public int? QueryInterlocksState()
    {
        if ( !this.IsSlotExists.HasValue )
            _ = this.QuerySlotExists();
        if ( !this.SupportsInterlock.HasValue )
            _ = this.QuerySupportsInterlock();
        if ( this.IsSlotExists.GetValueOrDefault( false ) && this.QuerySupportsInterlock().GetValueOrDefault( false ) )
        {
            this.InterlocksState = this.Session.Query( this.InterlocksState, string.Format( this.InterlockStateQueryCommandFormat, this.SlotNumber ) );
            this.Interlocks?.UpdateInterlockState( this.InterlocksState.GetValueOrDefault( 0 ) );
        }

        return this.InterlocksState;
    }

    /// <summary> QueryEnum if 'interlockNumber' is interlock engaged. </summary>
    /// <param name="interlockNumber"> The interlock number. </param>
    /// <returns> <c>true</c> if interlock engaged; otherwise <c>false</c> </returns>
    public bool IsInterlockEngaged( int interlockNumber )
    {
        if ( this.SupportsInterlock.GetValueOrDefault( false ) )
        {
            if ( !this.InterlocksState.HasValue )
            {
                _ = this.QueryInterlocksState();
            }

            return (this.InterlocksState.GetValueOrDefault( 0 ) & interlockNumber) == interlockNumber;
        }
        else
        {
            return true;
        }
    }

    #endregion
    #endregion
}
