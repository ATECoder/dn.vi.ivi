// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary> Manages TSP messages. </summary>
/// <remarks>
/// David, 2013-11-07          <para>
/// David, 2008-01-28, 2.0.2949.  Convert to .NET Framework. </para><para>
/// David, 2007-03-12, 1.15.2627 </para><para>
/// TSP Messages are designed to augment the service request messaging. This may be required
/// whenever the SRQ messaging is insufficient due to the fact that the SRQ messages are unable
/// to report on individual units or nodes. <p>
/// With TSP, only a single register is available for displaying the status of one or more nodes
/// with one or more units (SMU or switches). Therefore, the main device is unable to report status on each
/// node or unit. For example, the main device can raise only a single OPC flag. Thus there is no way
/// to tell which unit finished, only that an operation finished. In that case, the output
/// message must be much simpler more like the SRQ bit but for the entire operation rather than
/// the specific command. We need to know the node, unit, and status values.  That could be
/// followed by contents or data. </p><p>
/// Message Structure: </p><p>
/// Each TSP message consists of a preamble and potential follow up messages. </p><p>
/// Preamble:  </p><p>
/// The preamble is designed to by succinct providing minimal sufficient information to determine
/// status and what to do next. To accomplish this, without reinventing much, the messaging
/// system reports the following register information for the node or the unit: </p><p>
/// Service Request Register byte: SRQ, status byte, STB, or status.condition </p><p>
/// Status Register Byte:  ESR, or event status register, or status.standard.event </p><p>
/// Finally the information included a message available, data available, and buffer available
/// bits.  These help determine whether additional information is already in the output queue or
/// is in the messaging queue. </p><p>
/// Format:  All status information is in Hex as follows: </p><p>
/// NODE , UNIT , STB , ESR , INFO [, CONTENTS     </p><p>
/// 40 ,    b ,  BF ,  FF , FFFF [, FF, message] </p><p>
/// Contents:                   </p><p>
/// Message contents consists of a message type number following by a message contents. </p><p>
/// TYPE, MESSAGE </p><p>
/// FF, some text </p> </para><para>
/// (c) 2007 Integrated Scientific Resources, Inc. All rights reserved. </para><para>
/// Licensed under The MIT License.</para>
/// </remarks>
public class TspMessage
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public TspMessage() : base()
    {
    }

    /// <summary> Copy constructor. </summary>
    /// <param name="value"> Specifies message received from the instrument. </param>
    public TspMessage( TspMessage value ) : this()
    {
        if ( value is null )
        {
            this.Clear();
        }
        else
        {
            this.NodeNumber = value.NodeNumber;
            this.UnitNumber = value.UnitNumber;
            this.StandardEvents = value.StandardEvents;
            this.InfoStatus = value.InfoStatus;
            this.ServiceRequests = value.ServiceRequests;
            this.MessageType = value.MessageType;
            this.Contents = value.Contents;
            this.LastMessage = value.LastMessage;
        }
    }

    #endregion

    #region " fields "

    /// <summary> Gets the message contents. </summary>
    /// <value> The contents. </value>
    public string Contents { get; set; } = string.Empty;

    /// <summary> Gets the failure message parsing the message. </summary>
    /// <value> A message describing the failure. </value>
    public string FailureMessage { get; private set; } = string.Empty;

    /// <summary>
    /// Gets or sets the device information
    /// <see cref="TspMessageInfoBits">status bits</see>.
    /// </summary>
    /// <value> The information status. </value>
    public TspMessageInfoBits InfoStatus { get; set; }

    /// <summary> Gets or sets the last message. </summary>
    /// <value> A message describing the last. </value>
    public string? LastMessage { get; set; }

    /// <summary>
    /// Gets or sets the
    /// <see cref="TspMessageTypes">message type</see>.
    /// </summary>
    /// <value> The type of the message. </value>
    public TspMessageTypes MessageType { get; set; }

    /// <summary> Gets or sets the one-based node number. </summary>
    /// <value> The node number. </value>
    public int NodeNumber { get; set; }

    /// <summary>
    /// Gets or sets the service request
    /// <see cref="VI.Pith.ServiceRequests">status bits</see>.
    /// </summary>
    /// <value> The service requests. </value>
    public Pith.ServiceRequests ServiceRequests { get; set; }

    /// <summary>
    /// Gets or sets the standard event
    /// <see cref="VI.Pith.StandardEvents">status bits</see>.
    /// </summary>
    /// <value> The standard events. </value>
    public Pith.StandardEvents StandardEvents { get; set; }

    /// <summary> Gets or sets the unit number (a or b). </summary>
    /// <value> The unit number. </value>
    public string UnitNumber { get; set; } = string.Empty;

    #endregion

    #region " parse "

    /// <summary> Clears this object to its blank/initial state. </summary>
    public void Clear()
    {
        this.LastMessage = string.Empty;
        this.UnitNumber = string.Empty;
        this.NodeNumber = 0;
        this.MessageType = TspMessageTypes.None;
        this.InfoStatus = TspMessageInfoBits.None;
        this.StandardEvents = Pith.StandardEvents.None;
        this.ServiceRequests = Pith.ServiceRequests.None;
        this.Contents = string.Empty;
        this.FailureMessage = string.Empty;
    }

    /// <summary> Parses the message received from the instrument. </summary>
    /// <remarks>
    /// The TSP Message consists of the following elements: <p>
    /// NODE , UNIT , STB , ESR , INFO [, CONTENTS          </p><p>
    /// 40 ,    b ,  BF ,  FF , FFFF [, FF, message]      </p><p>
    /// node - node Number                                  </p><p>
    /// unit - unit number                                  </p><p>
    /// STB  - status register bits                         </p><p>
    /// ESR  - standard register bits                       </p><p>
    /// info - message information bits                     </p><p>
    /// contents - message data                             </p><p>
    /// </p>
    /// </remarks>
    /// <param name="value"> Specifies message received from the instrument. </param>
    public void Parse( string value )
    {
        this.Clear();
        this.UnitNumber = string.Empty;
        this.NodeNumber = 0;
        this.MessageType = TspMessageTypes.None;
        this.InfoStatus = TspMessageInfoBits.None;
        this.StandardEvents = Pith.StandardEvents.None;
        this.ServiceRequests = Pith.ServiceRequests.None;
        this.Contents = string.Empty;
        this.FailureMessage = string.Empty;
        if ( !string.IsNullOrWhiteSpace( value ) )
        {
            string[]? values = value.Split( ',' );
            if ( values is null || values.Length <= 0 )
            {
                return;
            }

            // trim all values.
            for ( int index = 0, loopTo = values.Length - 1; index <= loopTo; index++ )
            {
                values[index] = values[index].Trim();
                switch ( index )
                {
                    case 0:
                        {
                            this.NodeNumber = Convert.ToInt32( values[index], 16 );
                            break;
                        }

                    case 1:
                        {
                            this.UnitNumber = values[index];
                            break;
                        }

                    case 2:
                        {
                            this.ServiceRequests = ( Pith.ServiceRequests ) Convert.ToInt32( values[index], 16 );
                            break;
                        }

                    case 3:
                        {
                            this.StandardEvents = ( Pith.StandardEvents ) Convert.ToInt32( values[index], 16 );
                            break;
                        }

                    case 4:
                        {
                            this.InfoStatus = ( TspMessageInfoBits ) Convert.ToInt32( values[index], 16 );
                            break;
                        }

                    case 5:
                        {
                            this.MessageType = ( TspMessageTypes ) Convert.ToInt32( values[index], 16 );
                            break;
                        }

                    case 6:
                        {
                            this.Contents = values[index];
                            break;
                        }

                    default:
                        break;
                }
            }
        }
    }

    #endregion
}
/// <summary>
/// Enumerates the TSP Message Type.  This is set as system flags to allow combining types for
/// detecting a super structure.  For example, data available is the sum of binary and ASCII data
/// available bits.
/// </summary>
[Flags()]
public enum TspMessageTypes
{
    /// <summary>0x0000. not defined.</summary>
    [System.ComponentModel.Description( "Not Defined" )]
    None = 0,

    /// <summary>0x001. Sync Instrument message.</summary>
    [System.ComponentModel.Description( "Sync Status" )]
    SyncStatus = 1
}
/// <summary> Enumerates the TSP Instrument status. </summary>
[Flags()]
public enum TspMessageInfoBits
{
    /// <summary>0x0000. not defined.</summary>
    [System.ComponentModel.Description( "Not Defined" )]
    None = 0,

    /// <summary>0x0001. New message.  Not yet handled. </summary>
    [System.ComponentModel.Description( "New Message" )]
    NewMessage = 1,

    /// <summary>0x0002. Debug message.  Debug messages always have
    /// Contents following the information register. </summary>
    [System.ComponentModel.Description( "Debug Message" )]
    DebugMessage = 2,

    /// <summary> 0x0004. Non zero if unit or node is still active. </summary>
    [System.ComponentModel.Description( "Tsp Active" )]
    TspActive = 4,

    /// <summary> 0x0008. Non zero if unit or node had an error. </summary>
    [System.ComponentModel.Description( "Tsp Error" )]
    TspError = 8,

    /// <summary>0x0010. Source function in compliance.</summary>
    [System.ComponentModel.Description( "Compliance" )]
    Compliance = 16,

    /// <summary>0x0020. Non zero if unit has taken new “chunks” of data in its
    /// buffer.</summary>
    [System.ComponentModel.Description( "Data Available" )]
    DataAvailable = 32
}
