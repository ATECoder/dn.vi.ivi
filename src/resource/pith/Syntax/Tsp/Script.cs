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

    /// <summary>   (Immutable) the copy script to node command format. </summary>
    /// <remarks>
    /// Examples <c>
    /// _ = session.WriteLine( "{1}=script.new( {0}.source , '{1}' ) waitcomplete()", sourceName, destinationName );
    /// </c></remarks>
    public const string CopyScriptCommandFormat = "{1}=script.new( {0}.source , '{1}' ) waitcomplete()";

    /// <summary>   (Immutable) the copy script to node command format. </summary>
    /// <remarks>
    /// Example: <c>
    /// string.Format( Syntax.Tsp.CopyScriptCommandFormat, nodeNumber, sourceName, destinationName );
    /// StringBuilder builder = new();
    /// _ = builder.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Lua.LoadStringCommand}(table.concat(" );
    /// _ = builder.Append(commands );
    /// _ = builder.AppendLine( "))()" );
    /// session.WriteLines(builder.ToString(), Environment.NewLine, TimeSpan.Zero );
    /// </c></remarks>
    public const string CopyScriptToNodeCommandFormat = "node[{0}].execute('waitcomplete() {2}=script.new({1}.source,[[{2}]])') waitcomplete({0}) waitcomplete()";

    /// <summary>
    /// Gets a command to retrieve a catalog from the local node.
    /// This command must be enclosed in a 'do end' construct.
    /// a print(names) or dataqueue.add(names) needs to be added to get the data through.
    /// </summary>
    public const string ScriptCatalogGetterCommand = "local names for name in _G.script.user.catalog() do if names then names = names .. ',' .. name else names = name end end";

    /// <summary>   (Immutable) the saved script find command format. </summary>
    public const string FindSavedScriptCommandFormat = "local exists=false for name in _G.script.user.catalog() do exists = (name=='{0}') if exists then break end end";

    /// <summary>   (Immutable) the delete saved script command format. </summary>
    public const string DeleteSavedScriptCommandFormat = "local exists=false for name in _G.script.user.catalog() do exists = (name=='{0}') if exists then break end end if exists then _G.script.delete( name ) end";

    /// <summary>   (Immutable) the find saved script command format. </summary>
    public const string FindSavedScriptQueryFormat = "local exists=false for name in _G.script.user.catalog() do exists = (name=='{0}') if exists then break end end print(exists)";
}
