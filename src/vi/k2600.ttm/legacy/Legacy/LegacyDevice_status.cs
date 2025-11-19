// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

public partial class LegacyDevice : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDevice
{
    /// <summary> Sets the device to issue an SRQ upon any of the SCPI events. Uses *ESE to select
    /// (mask) the events that will issue SRQ and  *SRE to select (mask) the event registers to be
    /// included in the bits that will issue an SRQ. </summary>
    /// <remarks> 3475. Add Or Visa.Ieee4882.ServiceRequests.OperationEvent. </remarks>
    public bool EnableWaitComplete()
    {
        if ( !this.IsConnected ) return false;
        Pith.SessionBase session = this.Meter!.TspDevice!.Session!;

        // _ = session.ReadStatusByte();

        session.SetLastAction( "clearing execution state." );
        session.ClearExecutionState();

        session.SetLastAction( "enabling service request." );
        session.EnableServiceRequestOnOperationCompletion();

        session.SetLastAction( "querying operation completion." );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        return true;
    }
}
