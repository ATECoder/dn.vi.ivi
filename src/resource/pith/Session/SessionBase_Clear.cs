using System.ComponentModel;

namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " cls "

    private bool _statusClearDistractive;

    /// <summary>   Gets or sets a value indicating whether the status clear is distractive. </summary>
    /// <remarks>
    /// Per the SCPI standard, the clear execution state (*CLS) command must not clear the enable
    /// state of the event registers. The Keithley 2600 instruments does not adhere to this standard.
    /// This condition is, therefore, set to true so that the <see cref="ClearExecutionState(int)"/>
    /// method can store and then restore the enabled state of the event registers.
    /// </remarks>
    /// <value> True if status clear is distractive, false if not. </value>
    public bool StatusClearDistractive
    {
        get => this._statusClearDistractive;
        set => _ = this.SetProperty( ref this._statusClearDistractive, value );
    }

    private bool _clearsDeviceStructures = true;

    /// <summary>   True if *CLS clears the device structures [true]. </summary>
    /// <remarks>
    /// Per the SCPI standard, the clear execution state (*CLS) command must clear the device
    /// structures and the error queue thus clearing the status byte. The Keithley 2600 instruments does not adhere to this standard.
    /// This condition is, therefore, set to true so that the <see cref="ClearExecutionState(int)"/>
    /// method can clear the error queue and thus clear teh status byte.
    /// </remarks>
    /// <value> True if clears device structures, false if not. </value>
    [Description( "True if *CLS clears the device structures [true]" )]
    public bool ClearsDeviceStructures
    {
        get => this._clearsDeviceStructures;
        set => _ = this.SetProperty( ref this._clearsDeviceStructures, value );
    }

    /// <summary> The clear refractory period. </summary>
    private TimeSpan _clearRefractoryPeriod;

    /// <summary> Gets or sets the clear refractory period. </summary>
    /// <value> The clear refractory period. </value>
    public TimeSpan ClearRefractoryPeriod
    {
        get => this._clearRefractoryPeriod;
        set => _ = base.SetProperty( ref this._clearRefractoryPeriod, value );
    }

    /// <summary> Gets the clear execution state command. </summary>
    /// <remarks>
    /// SCPI: "*CLS".
    /// <see cref="VI.Syntax.Ieee488Syntax.ClearExecutionStateCommand"> </see>
    /// </remarks>
    /// <value> The clear execution state command. </value>
    public string ClearExecutionStateCommand { get; set; } = Syntax.Ieee488Syntax.ClearExecutionStateCommand;

    /// <summary>   Gets or sets the 'clear execution state operation complete' command. </summary>
    /// <value> The 'clear execution state wait complete' command. </value>
    public string ClearExecutionStateOperationCompleteCommand { get; set; } = Syntax.Ieee488Syntax.ClearExecutionStateOperationCompleteCommand;

    /// <summary>   Gets or sets the 'clear execution state wait' command. </summary>
    /// <value> The 'clear execution state wait' command. </value>
    public string ClearExecutionStateWaitCommand { get; set; } = Syntax.Ieee488Syntax.ClearExecutionStateWaitCommand;

    /// <summary>   Gets or sets the 'clear execution state query complete' command. </summary>
    /// <value> The 'clear execution state query complete' command. </value>
    public string ClearExecutionStateQueryCompleteCommand { get; set; } = Syntax.Ieee488Syntax.ClearExecutionStateQueryCompleteCommand;

    /// <summary> Returns <c>true</c> if the instrument supports clear execution state. </summary>
    /// <value> <c>true</c> if the instrument supports clear execution state. </value>
    public bool SupportsClearExecutionState => !string.IsNullOrWhiteSpace( this.ClearExecutionStateCommand );

    /// <summary>
    /// Sets the instrument to its the known clear state clearing status registers and the error
    /// queue.
    /// </summary>
    /// <remarks>
    /// 2024-10-08. Important: This method does not restore the service request and standard event
    /// enable registers.
    /// </remarks>
    /// <param name="queryCompleteTimeout">  (Optional) The query complete timeout. </param>
    public virtual void ClearExecutionStateQueryComplete( int queryCompleteTimeout = 10000 )
    {
        if ( this.SupportsOperationComplete )
        {
            this.SetLastAction( "clearing execution state and querying completion" );
            _ = this.WriteLine( this.ClearExecutionStateWaitCommand );
            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );

            this.SetLastAction( "reading the operation completion reply after clear execution state" );
            this.OperationCompleted = this.QueryOperationCompleted( queryCompleteTimeout );
        }
        else
        {
            this.SetLastAction( "clearing execution state" );
            _ = this.WriteLine( this.ClearExecutionStateCommand );
            // if not using OPC, wait the refractory period.
            _ = SessionBase.AsyncDelay( this.ClearRefractoryPeriod );
            this.OperationCompleted = true;
        }

        this.ClearErrorCache();
        this.ClearDiscardedData();
    }

    /// <summary>
    /// Sets the instrument to its the known clear state clearing status registers and the error
    /// queue.
    /// </summary>
    /// <remarks>
    /// Sends the '*CLS' message. <para>
    /// A query unterminated error (-420) will occur if the instrument output buffer has pending
    /// messages. </para><para>
    /// This method restores the service request and standard event enable registers if the  
    /// <see cref="StatusClearDistractive"/> is true. </para>
    /// </remarks>
    /// <param name="queryCompleteTimeout"> (Optional) (10000) The query complete timeout
    ///                                     in milliseconds. </param>
    public virtual void ClearExecutionState( int queryCompleteTimeout = 10000 )
    {
        // skip if the instrument does not support the CLS command.
        if ( !(this.IsSessionOpen && this.SupportsClearExecutionState) )
        {
            this.OperationCompleted = new bool?();
            return;
        }

        this.LastNodeNumber = default;

        int serviceRequests = 0;
        int standardEvents = 0;

        if ( this.StatusClearDistractive )
        {
            // store the standard and status event register enable state
            this.SetLastAction( "storing the last standard event enable bitmask" );
            standardEvents = ( int ) this.QueryStandardEventEnableBitmask().GetValueOrDefault( StandardEvents.None );
            this.SetLastAction( "storing the last service request event enable bitmask" );
            serviceRequests = this.QueryServiceRequestEnableBitmask();
        }

        this.ClearExecutionStateQueryComplete( queryCompleteTimeout );

        if ( this.StatusClearDistractive )
        {
            // restore the standard and status event register enable state
            this.SetLastAction( "restoring the last standard event enable bitmask" );
            this.WriteStandardEventEnableBitmask( standardEvents );
            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );
            this.SetLastAction( "restoring the last service request event enable bitmask" );
            this.WriteServiceRequestEnableBitmask( serviceRequests );
            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );
            // standardEvents = this.QueryStandardEventEnableBitmask().GetValueOrDefault( StandardEvents.None );
            // serviceRequests = this.QueryServiceRequestEnableBitmask();
            // int actualStandardEvents = ( int ) this.StandardEventEnableBitmask.GetValueOrDefault( 0 );
            // int actualServiceRequests = ( int ) this.ServiceRequestEnableBitmask.GetValueOrDefault( ServiceRequests.None );
            // Debug.Assert( standardEvents == actualStandardEvents );
            // Debug.Assert( serviceRequests == actualServiceRequests );
        }

        if ( !this.ClearsDeviceStructures )
        {
            // if this instrument does not clear the device structures, we need to clear the error queue.
            _ = this.QueryDeviceErrors( this.ReadStatusByte() );

            // clear the error queue just in case.
            this.ClearErrorQueue();
        }

    }

    #endregion

    #region " rst "

    /// <summary> The reset refractory period. </summary>
    private TimeSpan _resetRefractoryPeriod;

    /// <summary> Gets or sets the reset refractory period. </summary>
    /// <value> The reset refractory period. </value>
    public TimeSpan ResetRefractoryPeriod
    {
        get => this._resetRefractoryPeriod;
        set => _ = base.SetProperty( ref this._resetRefractoryPeriod, value );
    }

    /// <summary> Gets the reset known state command. </summary>
    /// <remarks>
    /// SCPI: "*RST".
    /// <see cref="VI.Syntax.Ieee488Syntax.ResetKnownStateCommand"> </see>
    /// </remarks>
    /// <value> The reset known state command. </value>
    public string ResetKnownStateCommand { get; set; } = Syntax.Ieee488Syntax.ResetKnownStateCommand;

    /// <summary>   Gets or sets the 'reset known state wait' command. </summary>
    /// <value> The 'reset known state wait' command. </value>
    public string ResetKnownStateWaitCommand { get; set; } = Syntax.Ieee488Syntax.ResetKnownStateWaitCommand;

    /// <summary> Returns <c>true</c> if the instrument supports reset known state. </summary>
    /// <value> <c>true</c> if the instrument supports reset known state. </value>
    public bool SupportsResetKnownState => !string.IsNullOrWhiteSpace( this.ResetKnownStateCommand );

    /// <summary> 
    /// Sends a <see cref="ResetKnownStateCommand">reset (*RST)</see> command to the instrument to set
    /// the known reset (default) state.
    /// </summary>
    /// <remarks>   2024-10-08. </remarks>
    /// <param name="queryCompleteTimeout">  (Optional) The query complete timeout. </param>
    public virtual void ResetKnownState( int queryCompleteTimeout = 10000 )
    {
        // the status subsystem sets the registers default bitmaps upon reset.
        this.ResetRegistersKnownState();

        this.LastNodeNumber = default;

        if ( !this.SupportsResetKnownState )
            this.OperationCompleted = new bool?();

        else if ( this.SupportsResetKnownState )
        {
            this.SetLastAction( "resetting known state and querying completion" );
            _ = this.WriteLine( this.ResetKnownStateWaitCommand );
            _ = SessionBase.AsyncDelay( this.ReadAfterWriteDelay );

            this.SetLastAction( "reading the operation completion reply after resetting known state" );
            this.OperationCompleted = this.QueryOperationCompleted( queryCompleteTimeout );
        }
        else
        {
            this.SetLastAction( "resetting known state" );
            _ = this.WriteLine( this.ResetKnownStateCommand );
            // if not using OPC, wait the refractory period.
            _ = SessionBase.AsyncDelay( this.ResetRefractoryPeriod );
            this.OperationCompleted = true;
        }
    }

    #endregion

    #region " status and read delays "

    private TimeSpan _readAfterWriteDelay;

    /// <summary>
    /// Gets or sets the to delay time to apply when reading immediately after a write.
    /// </summary>
    /// <value> The read delay. </value>
    public TimeSpan ReadAfterWriteDelay
    {
        get => this._readAfterWriteDelay;
        set => _ = base.SetProperty( ref this._readAfterWriteDelay, value );
    }

    private TimeSpan _statusReadDelay;

    /// <summary>
    /// Gets or sets the to delay time to apply when reading the status byte after a write.
    /// </summary>
    /// <value> The read delay. </value>
    public TimeSpan StatusReadDelay
    {
        get => this._statusReadDelay;
        set => _ = base.SetProperty( ref this._statusReadDelay, value );
    }

    #endregion
}
