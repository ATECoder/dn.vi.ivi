using cc.isr.Std.EscapeSequencesExtensions;

namespace cc.isr.VI.Tsp;

/// <summary> Defines a Status Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-10-07 </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="StatusSubsystemBase" /> class.
/// </remarks>
/// <param name="session"> A reference to a <see cref="Pith.SessionBase">message based TSP session</see>. </param>
public abstract class StatusSubsystemBase( Pith.SessionBase session ) : VI.StatusSubsystemBase( Pith.SessionBase.Validated( session ) )
{
    #region " construction and cleanup "

    /// <summary> Validated the given status subsystem base. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="statusSubsystemBase"> The status subsystem base. </param>
    /// <returns> A StatusSubsystemBase. </returns>
    public static Tsp.StatusSubsystemBase Validated( Tsp.StatusSubsystemBase statusSubsystemBase )
    {
        return statusSubsystemBase is null ? throw new ArgumentNullException( nameof( statusSubsystemBase ) ) : statusSubsystemBase;
    }

    #endregion

    #region " i presettable "

    /// <summary>   Sets the known initial post reset state. </summary>
    /// <remarks>   2024-09-17. </remarks>
    public override void InitKnownState()
    {
        base.InitKnownState();
        this.SerialNumber = new long?();
        this.SerialNumberReading = string.Empty;
        string activity = string.Empty;
        base.InitKnownState();
        try
        {
            activity = $"{this.ResourceModelCaption} storing communication timeout";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Session.StoreCommunicationTimeout( this.InitializeTimeout );

            activity = $"{this.ResourceModelCaption} clearing error queue";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            // clear the error queue on the controller node only.
            this.Session.ClearErrorQueue();
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
        finally
        {
            activity = $"{this.ResourceModelCaption} restoring communication timeout";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Session.RestoreCommunicationTimeout();
        }

        try
        {
            this.Session.SetLastAction( "enabling service request upon operation completion" );
            this.Session.EnableStandardServiceRequestEvents();

            this.Session.SetLastAction( "discarding unread data #1" );
            activity = $"{this.ResourceModelCaption} discarding unread data #1";
            // flush the input buffer in case the instrument has some leftovers.
            _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );

            if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Done {activity};. Data: {this.Session.DiscardedData}" );
        }
        catch ( Pith.NativeException ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }

        try
        {
            this.Session.SetLastAction( "discarding unread data #2" );
            // flush write may cause the instrument to send off a new data.
            activity = $"{this.ResourceModelCaption} discarding unread data #2";
            _ = this.Session.DiscardUnreadData( TimeSpan.FromMilliseconds( 100 ) );
            if ( !string.IsNullOrWhiteSpace( this.Session.DiscardedData ) )
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Done {activity};. Data: {this.Session.DiscardedData}" );
        }
        catch ( Pith.NativeException ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }

        // wait for operations to complete
        this.Session.SetLastAction( "querying operation completion" );
        _ = this.Session.QueryOperationCompleted();
    }

    #endregion

    #region " preset "

    /// <summary> Gets or sets the preset command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:PRES".
    /// <see cref="F:isr.VI.Syntax.Ieee488Syntax.ScpiVI.Syntax.Tsp.StatusPresetCommand"></see>
    /// </remarks>
    /// <value> The preset command. </value>
    protected override string PresetCommand { get; set; } = string.Empty;

    #endregion

    #region " measurement events "

    /// <summary> Gets or sets the measurement status query command. </summary>
    /// <value> The measurement status query command. </value>
    protected override string MeasurementStatusQueryCommand { get; set; } = VI.Syntax.Tsp.Status.MeasurementEventQueryCommand;

    /// <summary> Gets or sets the measurement event condition query command. </summary>
    /// <value> The measurement event condition query command. </value>
    protected override string MeasurementEventConditionQueryCommand { get; set; } = VI.Syntax.Tsp.Status.MeasurementEventConditionQueryCommand;

    #endregion

    #region " operation register events "

    /// <summary> Gets or sets the operation event enable Query command. </summary>
    /// <value> The operation event enable Query command. </value>
    protected override string OperationEventEnableQueryCommand { get; set; } = VI.Syntax.Tsp.Status.OperationEventEnableQueryCommand;

    /// <summary> Gets or sets the operation event enable command format. </summary>
    /// <value> The operation event enable command format. </value>
    protected override string OperationEventEnableCommandFormat { get; set; } = VI.Syntax.Tsp.Status.OperationEventEnableCommandFormat;

    /// <summary> Gets or sets the operation event status query command. </summary>
    /// <value> The operation event status query command. </value>
    protected override string OperationEventStatusQueryCommand { get; set; } = VI.Syntax.Tsp.Status.OperationEventQueryCommand;

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
    protected override string QuestionableStatusQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.QuestionableEventQueryCommand;

    #endregion

    #region " line frequency "

    /// <summary> Gets or sets line frequency query command. </summary>
    /// <value> The line frequency query command. </value>
    protected override string LineFrequencyQueryCommand { get; set; } = VI.Syntax.Tsp.LocalNode.LineFrequencyQueryCommand;

    #endregion

    #region " identity "

    /// <summary> Gets or sets the identity query command. </summary>
    /// <value> The identity query command. </value>
    protected override string IdentificationQueryCommand { get; set; } = VI.Syntax.Tsp.LocalNode.IdentificationQueryCommand;

    /// <summary> Gets or sets the serial number query command. </summary>
    /// <value> The serial number query command. </value>
    protected override string SerialNumberQueryCommand { get; set; } = VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand;

    /// <summary> Queries the Identity. </summary>
    /// <remarks> Sends the <see cref="IdentificationQueryCommand">identity query</see>. </remarks>
    /// <returns>   A <see cref="string" />. </returns>
    public override string QueryIdentity()
    {
        if ( !string.IsNullOrWhiteSpace( this.IdentificationQueryCommand ) )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Requesting identity;. " );
            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
            this.WriteIdentificationQueryCommand();
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Trying to read identity;. " );
            cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
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
    public VersionInfo VersionInfo { get; private set; } = new();

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

    /// <summary>Instrument summary. </summary>
    [System.ComponentModel.Description( "Instrument summary" )]
    InstrumentSummary = 0x2000,

    /// <summary>Program running.</summary>
    [System.ComponentModel.Description( "Program Running" )]
    ProgramRunning = 0x4000,

    /// <summary>Unknown value. Sets bit 16 (zero based and beyond the register size).</summary>
    [System.ComponentModel.Description( "Unknown" )]
    Unknown = 0x10000
}
