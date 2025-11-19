namespace cc.isr.VI.Syntax.Tsp;

/// <summary> Defines the TSP syntax. Modified for TSP2. </summary>
/// <remarks> (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para><para>
/// David, 2005-01-15, 1.0.1841.x. </para></remarks>
public static class Constants
{
    #region " constants "

    /// <summary> The  unknown values for values that go to the data log
    /// but not for creating the data file name. </summary>
    public const string UnknownValue = "N/A";

    /// <summary> The illegal file characters. </summary>
    public const string IllegalFileCharacters = @"/\\:*?""<>|";

    /// <summary> The  set of characters that should not be used in a
    /// script name. </summary>
    public const string IllegalScriptNameCharacters = @"./\\";

    /// <summary> The local node reference. </summary>
    public const string LocalNode = "_G.localnode";

    /// <summary> The  continuation prompt. </summary>
    public const string ContinuationPrompt = ">>>>";

    /// <summary> The  ready prompt. </summary>
    public const string ReadyPrompt = "TSP>";

    /// <summary> The  error prompt. </summary>
    public const string ErrorPrompt = "TSP?";

    #endregion

    #region " command builders "

    /// <summary> Builds a command. </summary>
    /// <param name="format"> Specifies a format string for the command. </param>
    /// <param name="args">   Specifies the arguments for the command. </param>
    /// <returns> The command. </returns>
    public static string Build( string format, params object[] args )
    {
        return string.Format( System.Globalization.CultureInfo.InvariantCulture, format, args );
    }

    #endregion
}
