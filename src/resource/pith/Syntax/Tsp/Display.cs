namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP Display syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class Display
{
    /// <summary> The name of the display subsystem. </summary>
    public const string SubsystemName = "display";

    /// <summary> The clear display command. </summary>
    public const string ClearCommand = "display.clear()";

    /// <summary> The display screen command format. </summary>
    public const string DisplayScreenCommandFormat = "display.changescreen(_G.display.SCREEN_{0})";

    /// <summary> The set cursor command format. </summary>
    public const string SetCursorCommandFormat = "display.setcursor({0}, {1})";

    /// <summary> The set cursor line command format. </summary>
    public const string SetCursorLineCommandFormat = "display.setcursor({0}, 1)";

    /// <summary> The set Character format. </summary>
    public const string SetCharacterCommandFormat = "display.settext(string.char({0}))";

    /// <summary> The set text format. </summary>
    public const string SetTextCommandFormat = "display.settext('{0}')";

    /// <summary> The set text format. </summary>
    public const string SetTextLineCommandFormat = "display.settext(display.TEXT{0},'{1}')";

    /// <summary> The restore main screen and send a wait complete reply. </summary>
    public const string RestoreMainCompleteQueryCommand = "display.screen = display.MAIN or 0 _G.waitcomplete() print('1') ";

    /// <summary> The restore main screen and set the operation complete status bit command. </summary>
    public const string RestoreMainOperationCompleteCommand = "display.screen = display.MAIN or 0 _G.opc()";

    /// <summary> The restore main screen and wait command. </summary>
    public const string RestoreMainWaitCommand = "display.screen = display.MAIN or 0 _G.waitcomplete()";

    /// <summary> The length of the first line. </summary>
    public const int FirstLineLength = 20;

    /// <summary> The length of the second line. </summary>
    public const int SecondLineLength = 32;

    /// <summary> The maximum character number. </summary>
    public const int MaximumCharacterNumber = short.MaxValue - 1;

}
