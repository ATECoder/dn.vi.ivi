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

    /// <summary>
    /// Gets a command to retrieve a catalog from the local node.
    /// This command must be enclosed in a 'do end' construct.
    /// a print(names) or dataqueue.add(names) needs to be added to get the data through.
    /// </summary>
    public const string ScriptCatalogGetterCommand = "local names='' for name in script.user.catalog() do names = names .. name .. ',' end";

    /// <summary>   (Immutable) the saved script find command format. </summary>
    public const string SavedScriptFindCommandFormat = "local exists = false for name in script.user.catalog() do exists = (name=='{0}') end";

    /// <summary>   (Immutable) the find save script command format. </summary>
    public const string FindSaveScriptCommandFormat = "local exists = false for name in script.user.catalog() do exists = (name=='{0}') end print(exists)";
}
