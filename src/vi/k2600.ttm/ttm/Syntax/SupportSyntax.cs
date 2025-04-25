namespace cc.isr.VI.Tsp.K2600.Ttm.Syntax;

/// <summary> A syntax of the Support LUA subsystem. </summary>
/// <remarks> (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para> </remarks>
public static class SupportSyntax
{
    /// <summary> The version query command. </summary>
    public const string VersionQueryCommand = "_G.print(_G.isr_support_getVersion())";

    /// <summary> The thermal transient script version query command. </summary>
    public const string ThermalTransientScriptVersionQueryCommand = "_G.print(_G.isr_ttm_getVersion())";

}
