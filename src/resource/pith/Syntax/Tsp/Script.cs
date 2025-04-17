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
    public const string FindSavedScriptCommandFormat = "local exists = false for name in script.user.catalog() do exists = (name=='{0}') if exists then break end end";

    /// <summary>   (Immutable) the delete saved script command format. </summary>
    public const string DeleteSavedScriptCommandFormat = "local exists = false for name in script.user.catalog() do exists = (name=='{0}') if exists then break end end if exists then _G.script.delete( name ) end";

    /// <summary>   (Immutable) the find saved script command format. </summary>
    public const string FindSavedScriptQueryFormat = "local exists = false for name in script.user.catalog() do exists = (name=='{0}') if exists then break end end print(exists)";

    /// <summary>   (Immutable) the delete a load menu item command format. </summary>
    public const string FindLoadMenuItemQueryFormat = "local exists = false for name in display.loadmenu.catalog() do exists = (name=='{0}') if exists then break end end _G.print(exists)";

    /// <summary>   (Immutable) the delete a load menu item command format. </summary>
    public const string DeleteLoadMenuItemCommandFormat = "local exists = false for name in display.loadmenu.catalog() do exists = (name=='{0}') if exists then break end end if exist then _G.display.loadmenu.delete( name ) end";

    /// <summary>   (Immutable) the delete an existing load menu item command format. </summary>
    public const string DeleteExistingLoadMenuItemCommandFormat = "_G.display.loadmenu.delete( '{0}' ) end";

}
