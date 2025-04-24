using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that clears the display described by session.
    /// </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="session">  The session. </param>
    public static void ClearDisplay( this Pith.SessionBase session )
    {
        _ = session.WriteLine( "_G.display.clear()" );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that sets display cursor. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="lineNumber">   The line number. </param>
    /// <param name="columnNumber"> The column number. </param>
    public static void SetDisplayCursor( this Pith.SessionBase session, int lineNumber, int columnNumber )
    {
        _ = session.WriteLine( Syntax.Tsp.Display.SetCursorCommandFormat, lineNumber, columnNumber );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that displays text on the instrument panel.
    /// </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="contents">     The contents. </param>
    /// <param name="lineNumber">   (Optional) The line number. </param>
    /// <param name="columnNumber"> (Optional) The column number. </param>
    public static void DisplayLine( this Pith.SessionBase session, string contents, int lineNumber = 1, int columnNumber = 1 )
    {
        // ignore empty strings.
        if ( string.IsNullOrWhiteSpace( contents ) )
            return;

        int length = Syntax.Tsp.Display.FirstLineLength;
        if ( lineNumber < 1 )
            lineNumber = 1;
        else if ( lineNumber > 2 )
            lineNumber = 2;

        if ( lineNumber == 2 )
            length = Syntax.Tsp.Display.SecondLineLength;

        _ = session.WriteLine( Syntax.Tsp.Display.SetCursorCommandFormat, lineNumber, columnNumber );

        int availableLength = length - columnNumber + 1;
        if ( contents.Length < availableLength )
            contents = contents.PadRight( availableLength );
        _ = session.WriteLine( Syntax.Tsp.Display.SetTextCommandFormat, contents );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that displays text. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="firstLine">    The first line. </param>
    /// <param name="secondLine">   The second line. </param>
    public static void Display( this Pith.SessionBase session, string firstLine, string secondLine )
    {
        session.ClearDisplay();
        session.DisplayLine( firstLine, 1, 1 );
        session.DisplayLine( secondLine, 2, 1 );
    }

    /// <summary> Gets or sets the restore display command query complete. </summary>
    /// <value> The restore display query complete command. </value>
    public static string RestoreMainCompleteQueryCommand { get; set; } = Syntax.Tsp.Display.RestoreMainCompleteQueryCommand;

    /// <summary> A <see cref="Pith.SessionBase"/> extension method that restores the instrument display getting the complete query message. </summary>
    public static void RestoreDisplayCompleteQuery( this SessionBase session )
    {
        // Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
        session.SetLastAction( "restoring display" );
        _ = session.WriteLine( $"{SessionBaseExtensionMethods.RestoreMainCompleteQueryCommand}" );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        _ = session.ReadFiniteLine();
        session.ThrowDeviceExceptionIfError();
    }

}
