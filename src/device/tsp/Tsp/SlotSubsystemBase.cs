namespace cc.isr.VI.Tsp;

/// <summary> Defines a System Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-01-13 </para>
/// </remarks>
public abstract class SlotSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
    /// </summary>
    /// <param name="slotNumber">      The slot number. </param>
    /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">status
    /// Subsystem</see>. </param>
    protected SlotSubsystemBase( int slotNumber, Tsp.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
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
    private bool? _isSlotExists;

    /// <summary> Gets or sets (Protected) the Slot existence indicator. </summary>
    /// <value> The Slot existence indicator. </value>
    public bool? IsSlotExists
    {
        get => this._isSlotExists;
        protected set => _ = this.SetProperty( ref this._isSlotExists, value );
    }

    /// <summary> Queries slot exists. </summary>
    /// <returns> The slot exists. </returns>
    public bool? QuerySlotExists()
    {
        this.IsSlotExists = !this.Session.IsNil( string.Format( Syntax.Tsp.Slot.SubsystemNameFormat, this.SlotNumber ) );
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
    public InterlockCollection Interlocks { get; private set; } = [];

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
    private bool? _supportsInterlock;

    /// <summary> Gets or sets the supports interlock. </summary>
    /// <value> The supports interlock. </value>
    public bool? SupportsInterlock
    {
        get => this._supportsInterlock;
        protected set => _ = this.SetProperty( ref this._supportsInterlock, value );
    }

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
            this.SupportsInterlock = !this.Session.IsNil( Syntax.Tsp.Slot.InterlockStateFormat, this.SlotNumber );
        }

        return this.SupportsInterlock;
    }

    #endregion

    #region " interlocak state "

    /// <summary> State of the interlocks. </summary>
    private int? _interlocksState;

    /// <summary> Gets or sets the state of the interlocks. </summary>
    /// <value> The interlock state. </value>
    public int? InterlocksState
    {
        get => this._interlocksState;
        protected set => _ = this.SetProperty( ref this._interlocksState, value );
    }

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
            this.InterlocksState = this.Session.Query( this.InterlocksState, string.Format( Syntax.Tsp.Slot.InterlockStateQueryCommandFormat, this.SlotNumber ) );
            this.Interlocks.UpdateInterlockState( this.InterlocksState.GetValueOrDefault( 0 ) );
        }

        return this.InterlocksState;
    }

    /// <summary> Query if 'interlockNumber' is interlock engaged. </summary>
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
