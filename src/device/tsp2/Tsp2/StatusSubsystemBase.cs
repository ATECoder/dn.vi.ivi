using cc.isr.Std.EscapeSequencesExtensions;

namespace cc.isr.VI.Tsp2;

/// <summary> Defines a Status Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-10-07 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class StatusSubsystemBase : VI.StatusSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusSubsystemBase" /> class.
    /// </summary>
    /// <param name="session"> A reference to a <see cref="Pith.SessionBase">message based TSP session</see>. </param>
    protected StatusSubsystemBase( Pith.SessionBase session ) : base( Pith.SessionBase.Validated( session ), Syntax.Tsp.EventLog.NoErrorCompoundMessage )
    {
        this.VersionInfo = new VersionInfo();
        InitializeSession( session );
    }

    #endregion

    #region " session "

    /// <summary> Initializes the session. </summary>
    /// <param name="session"> A reference to a <see cref="Pith.SessionBase">message based TSP session</see>. </param>
    private static void InitializeSession( Pith.SessionBase session )
    {
        session.DeviceClearDelayPeriod = TimeSpan.FromMilliseconds( 10d );
        session.ClearExecutionStateCommand = Syntax.Tsp.Status.ClearExecutionStateCommand;
        session.OperationCompleteCommand = Syntax.Tsp.Lua.OperationCompleteCommand;
        session.OperationCompletedQueryCommand = Syntax.Tsp.Lua.OperationCompletedQueryCommand;
        session.ResetKnownStateCommand = Syntax.Tsp.Lua.ResetKnownStateCommand;
        session.ServiceRequestEnableCommandFormat = Syntax.Tsp.Status.ServiceRequestEnableCommandFormat;

        // session.ServiceRequestEnableQueryCommand = Syntax.Tsp.Status.ServiceRequestEnableQueryCommand
        session.ServiceRequestEnableQueryCommand = Pith.Ieee488.Syntax.ServiceRequestEnableQueryCommand;

        // session.StandardEventStatusQueryCommand = Syntax.Tsp.Status.StandardEventStatusQueryCommand
        session.StandardEventStatusQueryCommand = Pith.Ieee488.Syntax.StandardEventStatusQueryCommand;

        // session.StandardEventEnableQueryCommand = Syntax.Tsp.Status.StandardEventEnableQueryCommand
        session.StandardEventEnableQueryCommand = Pith.Ieee488.Syntax.StandardEventEnableQueryCommand;
        session.StandardServiceEnableCommandFormat = Syntax.Tsp.Status.StandardServiceEnableCommandFormat;
        session.StandardServiceEnableCompleteCommandFormat = Syntax.Tsp.Status.StandardServiceEnableCompleteCommandFormat;
        session.WaitCommand = Syntax.Tsp.Lua.WaitCommand;
        // session.WaitCommand = VI.Syntax.Syntax.WaitCommand

        session.ErrorAvailableBitmask = Pith.ServiceRequests.ErrorAvailable;
        session.MeasurementEventBitmask = Pith.ServiceRequests.MeasurementEvent;
        session.MessageAvailableBitmask = Pith.ServiceRequests.MessageAvailable;
        session.StandardEventSummaryBitmask = Pith.ServiceRequests.StandardEvent;
    }

    #endregion

    #region " i presettable "

    /// <summary> Sets the known initial post reset state. </summary>
    public override void InitKnownState()
    {
        base.InitKnownState();
        this.SerialNumber = new long?();
        this.SerialNumberReading = string.Empty;
        base.InitKnownState();
        try
        {
            // flush the input buffer in case the instrument has some leftovers.
            _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
            if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Data discarded after turning prompts and errors off;. Data: {this.Session.DiscardedData}." );
            }
        }
        catch ( Pith.NativeException ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, $"Exception ignored clearing read buffer" );
        }

        try
        {
            // flush write may cause the instrument to send off a new data.
            _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
            if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Unread data discarded after discarding unset data;. Data: {this.Session.DiscardedData}." );
            }
        }
        catch ( Pith.NativeException ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, $"Exception ignored clearing read buffer" );
        }

        // wait for operations to complete
        _ = this.Session.QueryOperationCompleted();
    }

    #endregion

    #region " preset "

    /// <summary> Gets or sets the preset command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:PRES".
    /// <see cref="F:isr.VI.Syntax.Syntax.ScpiSyntax.StatusPresetCommand"></see>
    /// </remarks>
    /// <value> The preset command. </value>
    protected override string PresetCommand { get; set; } = string.Empty;

    #endregion

    #region " language "

    /// <summary> Gets or sets the Language query command. </summary>
    /// <value> The Language query command. </value>
    protected override string LanguageQueryCommand { get; set; } = Pith.Ieee488.Syntax.LanguageQueryCommand;

    /// <summary> Gets or sets the Language command format. </summary>
    /// <value> The Language command format. </value>
    protected override string LanguageCommandFormat { get; set; } = Pith.Ieee488.Syntax.LanguageCommandFormat;

    #endregion

    #region " measurement events "

    /// <summary> Gets or sets the measurement status query command. </summary>
    /// <value> The measurement status query command. </value>
    protected override string MeasurementStatusQueryCommand { get; set; } = string.Empty; // Not available with TSP2

    /// <summary> Gets or sets the measurement event condition query command. </summary>
    /// <value> The measurement event condition query command. </value>
    protected override string MeasurementEventConditionQueryCommand { get; set; } = string.Empty; // Not available with TSP2

    #endregion

    #region " operation register events "

    /// <summary> Gets or sets the operation event enable Query command. </summary>
    /// <value> The operation event enable Query command. </value>
    protected override string OperationEventEnableQueryCommand { get; set; } = Syntax.Tsp.Status.OperationEventEnableQueryCommand;

    /// <summary> Gets or sets the operation event enable command format. </summary>
    /// <value> The operation event enable command format. </value>
    protected override string OperationEventEnableCommandFormat { get; set; } = Syntax.Tsp.Status.OperationEventEnableCommandFormat;

    /// <summary> Gets or sets the operation event status query command. </summary>
    /// <value> The operation event status query command. </value>
    protected override string OperationEventStatusQueryCommand { get; set; } = Syntax.Tsp.Status.OperationEventQueryCommand;

    /// <summary> Programs the Operation register event enable bit mask. </summary>
    /// <param name="value"> The bitmask. </param>
    /// <returns> The mask to use for enabling the events; nothing if unknown. </returns>
    public override int? WriteOperationEventEnableBitmask( int value )
    {
        if ( (value & ( int ) OperationEventBits.UserRegister) != 0 )
        {
            // if enabling the user register, enable all events on the user register.
            value = 0x4FFF;
        }

        return this.WriteOperationEventEnableBitmask( value );
    }

    #endregion

    #region " questionable register "

    /// <summary> Gets or sets the questionable status query command. </summary>
    /// <value> The questionable status query command. </value>
    protected override string QuestionableStatusQueryCommand { get; set; } = Pith.Scpi.Syntax.QuestionableEventQueryCommand;

    #endregion

    #region " line frequency "

    /// <summary> Gets or sets line frequency query command. </summary>
    /// <value> The line frequency query command. </value>
    protected override string LineFrequencyQueryCommand { get; set; } = Syntax.Tsp.LocalNode.LineFrequencyQueryCommand;

    #endregion

    #region " identity "

    /// <summary> Gets or sets the identity query command. </summary>
    /// <value> The identity query command. </value>
    protected override string IdentificationQueryCommand { get; set; } = Syntax.Tsp.LocalNode.IdentificationQueryCommand;

    /// <summary> Gets or sets the serial number query command. </summary>
    /// <value> The serial number query command. </value>
    protected override string SerialNumberQueryCommand { get; set; } = Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand;

    /// <summary> Queries the Identity. </summary>
    /// <remarks> Sends the <see cref="IdentificationQueryCommand">identity query</see>. </remarks>
    /// <returns>   A <see cref="string" />. </returns>
    public override string QueryIdentity()
    {
        if ( !string.IsNullOrWhiteSpace( this.IdentificationQueryCommand ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Requesting identity;. " );
            VI.Pith.SessionBase.DoEventsAction?.Invoke();
            this.WriteIdentificationQueryCommand();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Trying to read identity;. " );
            VI.Pith.SessionBase.DoEventsAction?.Invoke();
            // wait for the delay time.
            // Stopwatch.StartNew. Wait(Me.ReadAfterWriteRefractoryPeriod)
            string value = this.Session.ReadLineTrimEnd();
            value = value.ReplaceCommonEscapeSequences().Trim();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting identity to {value};. " );
            this.VersionInfo.Parse( value );
            this.VersionInfoBase = this.VersionInfo;
            this.Identity = this.VersionInfo.Identity;
        }

        return this.Identity;
    }

    /// <summary> Gets or sets the information describing the version. </summary>
    /// <value> Information describing the version. </value>
    public VersionInfo VersionInfo { get; private set; }

    #endregion

    #region " device errors "

    /// <summary> Gets or sets the clear error queue command. </summary>
    /// <value> The clear error queue command. </value>
    protected override string ClearErrorQueueCommand { get; set; } = Syntax.Tsp.EventLog.ClearEventLogCommand;

    /// <summary> Gets or sets the last error query command. </summary>
    /// <value> The last error query command. </value>
    protected override string DeviceErrorQueryCommand { get; set; } = string.Empty; // VI.Syntax.Syntax.LastSystemErrorQueryCommand

    /// <summary> Gets or sets the 'Next Error' query command. </summary>
    /// <value> The error queue query command. </value>
    protected override string DequeueErrorQueryCommand { get; set; } = string.Empty; // VI.Syntax.Syntax.LastSystemErrorQueryCommand

    /// <summary> Gets or sets the 'Next Error' query command. </summary>
    /// <value> The error queue query command. </value>
    protected override string NextDeviceErrorQueryCommand { get; set; } = Syntax.Tsp.EventLog.NextErrorFormattedQueryCommand;

    /// <summary> Enqueue device error. </summary>
    /// <param name="compoundErrorMessage"> Message describing the compound error. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    protected override DeviceError EnqueueDeviceError( string compoundErrorMessage )
    {
        TspDeviceError de = new TspDeviceError();
        de.Parse( compoundErrorMessage );
        if ( de.IsError )
            this.DeviceErrorQueue.Enqueue( de );
        return de;
    }

    #endregion

    #region " collect garbage "

    /// <summary> Gets or sets the collect garbage wait complete command. </summary>
    /// <value> The collect garbage wait complete command. </value>
    protected override string CollectGarbageWaitCompleteCommand { get; set; } = Syntax.Tsp.Lua.CollectGarbageWaitCompleteCommand;

    #endregion

}

/// <summary> Enumerates the status bits for the operations register. </summary>
[Flags()]
public enum OperationEventBits
{
    /// <summary>Empty.</summary>
    [System.ComponentModel.Description( "Empty" )]
    None = 0,

    /// <summary>Calibrating.</summary>
    [System.ComponentModel.Description( "Calibrating" )]
    Calibrating = 0x1,

    /// <summary>Measuring.</summary>
    [System.ComponentModel.Description( "Measuring" )]
    Measuring = 0x10,

    /// <summary>Prompts enabled.</summary>
    [System.ComponentModel.Description( "Prompts Enabled" )]
    Prompts = 0x800,

    /// <summary>User Register.</summary>
    [System.ComponentModel.Description( "User Register" )]
    UserRegister = 0x1000,

    /// <summary>User Register.</summary>
    [System.ComponentModel.Description( "Instrument summary" )]
    InstrumentSummary = 0x2000,

    /// <summary>Program running.</summary>
    [System.ComponentModel.Description( "Program Running" )]
    ProgramRunning = 0x4000,

    /// <summary>Unknown value. Sets bit 16 (zero based and beyond the register size).</summary>
    [System.ComponentModel.Description( "Unknown" )]
    Unknown = 0x10000
}
