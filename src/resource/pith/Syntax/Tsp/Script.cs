namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP Display syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class Script
{
    /// <summary>   (Immutable) the load script command. </summary>
    public const string LoadScriptCommand = "loadscript";

    /// <summary>   (Immutable) the load and run script command. </summary>
    public const string LoadAndRunScriptCommand = "loadandrunscript";

    /// <summary>   (Immutable) the end script command. </summary>
    public const string EndScriptCommand = "endscript";

    /// <summary>   (Immutable) the start of binary script. </summary>
    public const string StartOfBinaryScript = "\\27LuaP\\0\\4\\4\\4";

    /// <summary>   (Immutable) the end of binary script. </summary>
    public const string EndOfBinaryScript = "\\128\\27\"";
}
