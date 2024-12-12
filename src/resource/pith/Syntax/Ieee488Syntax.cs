using System.ComponentModel;

namespace cc.isr.VI.Syntax;

/// <summary> Defines the standard IEEE488 command set. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class Ieee488Syntax
{
    #region " ieee 488.2 standard commands "

    /// <summary> Gets the Clear Status (CLS) command. </summary>
    public const string ClearExecutionStateCommand = "*CLS";

    /// <summary>   (Immutable) the clear execution state operation complete command. </summary>
    public const string ClearExecutionStateOperationCompleteCommand = "*CLS; *OPC";

    /// <summary>   (Immutable) the clear execution state wait command. </summary>
    public const string ClearExecutionStateWaitCommand = "*CLS; *WAI";

    /// <summary>   (Immutable) the clear execution state query complete command. </summary>
    public const string ClearExecutionStateQueryCompleteCommand = "*CLS; *OPC?";

    /// <summary> Gets the identification query (*IDN?) command. </summary>
    public const string IdentificationQueryCommand = "*IDN?";

    /// <summary>
    /// (Immutable) Gets the operation complete (*OPC) command. This command sets the Operation
    /// Complete bit in the Standard Event Register after all pending commands have completed.
    /// </summary>
    public const string OperationCompleteCommand = "*OPC";

    /// <summary> Gets the Operation Complete Enable '*CLS; *ESE {0:D}; *OPC' command format. </summary>
    [Obsolete( "Replace with StandardEventEnableCommandFormat" )]
    public const string OperationCompleteEnableCommandFormat = "*ESE {0:D}";

    /// <summary> Gets the operation complete query (*OPC?) command. </summary>
    public const string OperationCompletedQueryCommand = "*OPC?";

    /// <summary> Gets the options query (*OPT?) command. </summary>
    public const string OptionsQueryCommand = "*OPT?";

    /// <summary> Gets the reset to know state (*RST) command. </summary>
    public const string ResetKnownStateCommand = "*RST";

    /// <summary>   (Immutable) the reset known state operation Complete Command. </summary>
    public const string ResetKnownStateOperationCompleteCommand = "*RST; *OPC";

    /// <summary>   (Immutable) the reset known state wait command. </summary>
    public const string ResetKnownStateWaitCommand = "*RST; *WAI";

    /// <summary> Gets the Standard Event Enable (*ESE) command. </summary>
    public const string StandardEventEnableCommandFormat = "*ESE {0:D}";

    /// <summary> Gets the Standard Event Enable query (*ESE?) command. </summary>
    public const string StandardEventEnableQueryCommand = "*ESE?";

    /// <summary> Gets the Standard Event Enable (*ESR?) command. </summary>
    public const string StandardEventStatusQueryCommand = "*ESR?";

    /// <summary> Gets the Service Request Enable (*SRE) command. </summary>
    public const string ServiceRequestEnableCommandFormat = "*SRE {0:D}";

    /// <summary> Gets the Standard Event and Service Request Enable command format. </summary>
    public const string StandardServiceEnableCommandFormat = "*ESE {0:D}; *SRE {1:D}";

    /// <summary>
    /// (Immutable) Gets the Standard Event and Service Request Enable command format preceded by
    /// clear execution state.
    /// </summary>
    public const string StandardServiceEnableOperationCompleteCommandFormat = "*CLS; *WAI; *ESE {0:D}; *SRE {1:D}; *OPC";

    /// <summary>   (Immutable) the standard service enable query complete command format. </summary>
    public const string StandardServiceEnableQueryCompleteCommandFormat = "*CLS; *WAI; *ESE {0:D}; *SRE {1:D}; *OPC?";

    /// <summary> Gets the Service Request Enable query (*SRE?) command. </summary>
    public const string ServiceRequestEnableQueryCommand = "*SRE?";

    /// <summary> Gets the Service Request Status Byte query (*STB?) command. </summary>
    public const string StatusByteQueryCommand = "*STB?";

    /// <summary>
    /// (Immutable) Gets the Wait (*WAI) command. This command is used to suspend the execution of
    /// subsequent commands until the device operations of all previous overlapped commands are
    /// finished. This command is not needed for sequential commands, which are allowed to finish
    /// before the next command is executed.
    /// </summary>
    public const string WaitCommand = "*WAI";

    #endregion

    #region " keithley ieee488 commands "

    /// <summary> Gets the Language query (*LANG?) command. </summary>
    public const string LanguageQueryCommand = "*LANG?";

    /// <summary> Gets the Language command format (*LANG). </summary>
    public const string LanguageCommandFormat = "*LANG {0}";

    /// <summary> The language scpi. </summary>
    public const string LanguageScpi = "SCPI";

    /// <summary> The language TSP. </summary>
    public const string LanguageTsp = "TSP";

    #endregion

    #region " builders "

    /// <summary> Builds the device clear (DCL) command. </summary>
    /// <returns>
    /// An enumerator that allows for-each to be used to process build device clear command in this
    /// collection.
    /// </returns>
    public static IEnumerable<byte> BuildDeviceClear()
    {
        // Thee DCL command to the interface.
        byte[] commands = [ Convert.ToByte( ( int ) CommandCode.Untalk ), Convert.ToByte( ( int ) CommandCode.Unlisten ),
                                          Convert.ToByte( ( int ) CommandCode.DeviceClear ),
                                          Convert.ToByte( ( int ) CommandCode.Untalk ), Convert.ToByte( ( int ) CommandCode.Unlisten ) ];
        return commands;
    }

    /// <summary> Builds selective device clear (SDC) in this collection. </summary>
    /// <param name="gpibAddress"> The gpib address. </param>
    /// <returns>
    /// An enumerator that allows for-each to be used to process build selective device clear in this
    /// collection.
    /// </returns>
    public static IEnumerable<byte> BuildSelectiveDeviceClear( byte gpibAddress )
    {
        byte[] commands = [ Convert.ToByte((int)CommandCode.Untalk), Convert.ToByte((int)CommandCode.Unlisten),
                                        Convert.ToByte( Convert.ToByte((int)CommandCode.ListenAddressGroup) | gpibAddress),
                                        Convert.ToByte((int)CommandCode.SelectiveDeviceClear),
                                        Convert.ToByte((int)CommandCode.Untalk), Convert.ToByte((int)CommandCode.Unlisten) ];
        return commands;
    }

    #endregion
}

/// <summary>   Values that represent command languages. </summary>
/// <remarks>   2024-09-16. </remarks>
public enum CommandLanguage
{
    /// <summary>   An enum constant representing the SCPI option. </summary>
    Scpi = 0,

    /// <summary>   An enum constant representing the tsp option. </summary>
    Tsp = 1,
}

/// <summary> Values that represent IEEE 488.2 Command Code. </summary>
public enum CommandCode
{
    /// <summary> An enum constant representing the none option. </summary>
    None = 0,

    /// <summary> An enum constant representing the go to local option. </summary>
    [Description( "GTL" )]
    GoToLocal = 0x1,

    /// <summary> An enum constant representing the selective device clear option. </summary>
    [Description( "SDC" )]
    SelectiveDeviceClear = 0x4,

    /// <summary> An enum constant representing the group execute trigger option. </summary>
    [Description( "GET" )]
    GroupExecuteTrigger = 0x8,

    /// <summary> An enum constant representing the local lockout option. </summary>
    [Description( "LLO" )]
    LocalLockout = 0x11,

    /// <summary> An enum constant representing the device clear option. </summary>
    [Description( "DCL" )]
    DeviceClear = 0x14,

    /// <summary> An enum constant representing the serial poll enable option. </summary>
    [Description( "SPE" )]
    SerialPollEnable = 0x18,

    /// <summary> An enum constant representing the serial poll disable option. </summary>
    [Description( "SPD" )]
    SerialPollDisable = 0x19,

    /// <summary> An enum constant representing the listen address group option. </summary>
    [Description( "LAG" )]
    ListenAddressGroup = 0x20,

    /// <summary> An enum constant representing the talk address group option. </summary>
    [Description( "TAG" )]
    TalkAddressGroup = 0x40,

    /// <summary> An enum constant representing the secondary command group option. </summary>
    [Description( "SCG" )]
    SecondaryCommandGroup = 0x60,

    /// <summary> An enum constant representing the unlisten option. </summary>
    [Description( "UNL" )]
    Unlisten = 0x3F,

    /// <summary> An enum constant representing the untalk option. </summary>
    [Description( "UNT" )]
    Untalk = 0x5F
}
