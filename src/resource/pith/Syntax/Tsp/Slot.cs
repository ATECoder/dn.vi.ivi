namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP Slot syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class Slot
{
    /// <summary>   The subsystem name format. </summary>
    public const string SubsystemNameFormat = "_G.slot[{0}]";

    /// <summary> The interlock state format. </summary>
    public const string InterlockStateFormat = "_G.slot[{0}].interlock.state";

    /// <summary> The interlock state query (print) command format. </summary>
    public const string InterlockStateQueryCommandFormat = "_G.print(_G.string.format('%d',_G.slot[{0}].interlock.state))";
}
