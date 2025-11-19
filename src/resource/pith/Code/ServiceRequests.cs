// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;

namespace cc.isr.VI.Pith;

/// <summary> Gets or sets the status byte bits of the service request register. </summary>
/// <remarks>
/// Enumerates the Status Byte Register Bits. Use *STB? or status.request_event to read this
/// register. Use *SRE or status.request_enable to enable these services. This attribute is used
/// to read the status byte, which is returned as a numeric value. The binary equivalent of the
/// returned value indicates which register bits are set. <para>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. </para><para>
/// Licensed under The MIT License. </para>
/// </remarks>
[Flags(), CLSCompliant( true )]
public enum ServiceRequests
{
    /// <summary> The None option. </summary>
    [Description( "None" )]
    None = 0,

    /// <summary>
    /// Bit B0, Measurement Summary Bit (MSB). Set summary bit indicates
    /// that an enabled measurement event has occurred.
    /// </summary>
    [Description( "Measurement Summary Bit (MSB)" )]
    MeasurementEvent = 0x1,

    /// <summary>
    /// Bit B1, System Summary Bit (SSB). Set summary bit indicates
    /// that an enabled system event has occurred.
    /// </summary>
    [Description( "System Summary Bit (SSB)" )]
    SystemEvent = 0x2,

    /// <summary>
    /// Bit B2, Error Available (EAV). Set summary bit indicates that
    /// an error or status message is present in the Error Queue.
    /// </summary>
    [Description( "Error Available (EAV)" )]
    ErrorAvailable = 0x4,

    /// <summary>
    /// Bit B3, Questionable Summary Bit (QSB). Set summary bit indicates
    /// that an enabled questionable event has occurred.
    /// </summary>
    [Description( "Questionable Summary Bit (QSB)" )]
    QuestionableEvent = 0x8,

    /// <summary>
    /// Bit B4 (16), Message Available (MAV). Set summary bit indicates that
    /// a response message is present in the Output Queue.
    /// </summary>
    [Description( "Message Available (MAV)" )]
    MessageAvailable = 0x10,

    /// <summary>Bit B5, Standard Event Summary Bit (ESB). Set summary bit indicates
    /// that an enabled standard event has occurred.
    /// </summary>
    [Description( "Event Summary Bit (ESB)" )]
    StandardEventSummary = 0x20, // (32) ESB

    /// <summary>
    /// Bit B6 (64), Request Service (RQS)/Main Summary Status (MSS).
    /// Set bit indicates that an enabled summary bit of the Status Byte Register
    /// is set. Depending on how it is used, Bit B6 of the Status Byte Register
    /// is either the Request for Service (RQS) bit or the Main Summary Status
    /// (MSS) bit: When using the GPIB serial poll sequence of the unit to obtain
    /// the status byte (serial poll byte), B6 is the RQS bit. When using
    /// status.condition or the *STB? common command to read the status byte,
    /// B6 is the MSS bit.
    /// </summary>
    [Description( "Request Service (RQS)/Main Summary Status (MSS)" )]
    RequestingService = 0x40,

    /// <summary>
    /// Bit B7 (128), Operation Summary (OSB). Set summary bit indicates that
    /// an enabled operation event has occurred.
    /// </summary>
    [Description( "Operation Summary Bit (OSB)" )]
    OperationEvent = 0x80,

    /// <summary>
    /// Includes all bits.
    /// </summary>
    [Description( "All" )]
    All = 0xFF, // 255

    /// <summary>
    /// Unknown value due to, for example, error trying to get value from the device.
    /// </summary>
    [Description( "Unknown" )]
    Unknown = 0x100
}
