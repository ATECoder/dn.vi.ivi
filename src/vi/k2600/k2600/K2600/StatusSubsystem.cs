// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600;

/// <summary> Status subsystem. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-14 </para>
/// </remarks>
public class StatusSubsystem : Tsp.StatusSubsystemBase
{
    #region " construction and cleanup "

    /// <summary> Initializes a new instance of the <see cref="StatusSubsystem" /> class. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="session"> The session. </param>
    public StatusSubsystem( Pith.SessionBase session ) : base( session ) => this.VersionInfoBase = new VersionInfo();

    /// <summary> Creates a new StatusSubsystem. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A StatusSubsystem. </returns>
    public static StatusSubsystem Create()
    {
        StatusSubsystem? subsystem;
        try
        {
            subsystem = new StatusSubsystem( SessionFactory.Instance.Factory.Session() );
        }
        catch
        {
            throw;
        }

        return subsystem;
    }

    #endregion

    #region " device error handling "

    /// <summary>   Handles device error from the <see cref="Pith.SessionBase"/>. </summary>
    /// <remarks>   2024-09-17. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Service request event information. </param>
    protected override void HandleDeviceErrorOccurred( object? sender, ServiceRequestEventArgs e )
    {
    }

    #endregion

    #region " commands: measurement register events "

    /// <summary> Gets or sets the measurement status query command. </summary>
    /// <value> The measurement status query command. </value>
    protected override string MeasurementStatusQueryCommand { get; set; } = VI.Syntax.Tsp.Status.MeasurementEventQueryCommand;

    /// <summary> Gets or sets the measurement event condition query command. </summary>
    /// <value> The measurement event condition query command. </value>
    protected override string MeasurementEventConditionQueryCommand { get; set; } = VI.Syntax.Tsp.Status.MeasurementEventConditionQueryCommand;

    #endregion

    #region " commands: operation register events "

    /// <summary> Gets or sets the operation event enable Query command. </summary>
    /// <value> The operation event enable Query command. </value>
    protected override string OperationEventEnableQueryCommand { get; set; } = VI.Syntax.Tsp.Status.OperationEventEnableQueryCommand;

    /// <summary> Gets or sets the operation event enable command format. </summary>
    /// <value> The operation event enable command format. </value>
    protected override string OperationEventEnableCommandFormat { get; set; } = VI.Syntax.Tsp.Status.OperationEventEnableCommandFormat;

    /// <summary> Gets or sets the operation event status query command. </summary>
    /// <value> The operation event status query command. </value>
    protected override string OperationEventStatusQueryCommand { get; set; } = VI.Syntax.Tsp.Status.OperationEventQueryCommand;

    #endregion

    #region "commands:  questionable register "

    /// <summary> Gets or sets the questionable status query command. </summary>
    /// <value> The questionable status query command. </value>
    protected override string QuestionableStatusQueryCommand { get; set; } = VI.Syntax.Tsp.Status.QuestionableEventQueryCommand;

    #endregion

    #region " commands: identitiy "

    /// <summary> Gets or sets the identity query command. </summary>
    /// <value> The identity query command. </value>
    protected override string IdentificationQueryCommand { get; set; } = Syntax.Ieee488Syntax.IdentificationQueryCommand + " " + Syntax.Ieee488Syntax.WaitCommand;

    #endregion

    #region " not supported "

    /// <summary>   Not supported. </summary>
    protected override string PresetCommand { get; set; } = string.Empty;

    #endregion

}
