namespace cc.isr.VI.Tsp.K2600.Ttm.Syntax;

/// <summary> A syntax of the Support LUA subsystem. </summary>
/// <remarks> (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para> </remarks>
public static class SupportSyntax
{
    /// <summary>   (Immutable) the legacy version getter. </summary>
    public const string LegacyVersionGetter = "_G.isr.version()";

    /// <summary>   (Immutable) the latest version getter. </summary>
    public const string LatestVersionGetter = "_G.isr_support_getVersion()";

    /// <summary>   (Immutable)  the version query command. </summary>
    public static string VersionQueryCommand => MeterSubsystem.LegacyFirmware
        ? $"_G.print({SupportSyntax.LegacyVersionGetter})"
        : $"_G.print({SupportSyntax.LatestVersionGetter})";

}
