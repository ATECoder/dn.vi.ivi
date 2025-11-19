// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP Display syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class Display
{
    /// <summary> The name of the display subsystem. </summary>
    public const string SubsystemName = "_G.display";

    /// <summary> The clear display command. </summary>
    public const string ClearCommand = "_G.display.clear()";

    /// <summary> The display screen command format. </summary>
    public const string DisplayScreenCommandFormat = "_G.display.changescreen(_G.display.SCREEN_{0})";

    /// <summary> The set cursor command format. </summary>
    public const string SetCursorCommandFormat = "_G.display.setcursor({0}, {1})";

    /// <summary> The set cursor line command format. </summary>
    public const string SetCursorLineCommandFormat = "_G.display.setcursor({0}, 1)";

    /// <summary> The set Character format. </summary>
    public const string SetCharacterCommandFormat = "_G.display.settext(string.char({0}))";

    /// <summary> The set text format. </summary>
    public const string SetTextCommandFormat = "_G.display.settext('{0}')";

    /// <summary> The set text format. </summary>
    public const string SetTextLineCommandFormat = "_G.display.settext(_G.display.TEXT{0},'{1}')";

    /// <summary> The restore main screen and send a wait complete reply. </summary>
    public const string RestoreMainCompleteQueryCommand = "_G.display.screen = _G.display.MAIN or 0 _G.waitcomplete() _G.print('1') ";

    /// <summary> The restore main screen and set the operation complete status bit command. </summary>
    public const string RestoreMainOperationCompleteCommand = "_G.display.screen = _G.display.MAIN or 0 _G.opc()";

    /// <summary> The restore main screen and wait command. </summary>
    public const string RestoreMainWaitCommand = "_G.display.screen = _G.display.MAIN or 0 _G.waitcomplete()";

    /// <summary> The length of the first line. </summary>
    public const int FirstLineLength = 20;

    /// <summary> The length of the second line. </summary>
    public const int SecondLineLength = 32;

    /// <summary> The maximum character number. </summary>
    public const int MaximumCharacterNumber = short.MaxValue - 1;

    /// <summary>   (Immutable) the delete a load menu item command format. </summary>
    public const string FindLoadMenuItemQueryFormat = "local exists=false for name in _G.display.loadmenu.catalog() do exists = (name=='{0}') if exists then break end end _G.print(exists)";

    /// <summary>   (Immutable) the delete a load menu item command format. </summary>
    public const string DeleteLoadMenuItemCommandFormat = "local exists=false for name in _G.display.loadmenu.catalog() do exists = (name=='{0}') if exists then break end end if exists then _G.display.loadmenu.delete( name ) end";

    /// <summary>   (Immutable) the delete an existing load menu item command format. </summary>
    public const string DeleteExistingLoadMenuItemCommandFormat = "_G.display.loadmenu.delete( '{0}' )";

    /// <summary>   (Immutable) the add load menu item command format. </summary>
    public const string AddLoadMenuItemCommandFormat = "_G.display.loadmenu.add( '{0}', '{1}', 0 )";
}
