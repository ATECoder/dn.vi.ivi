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
        _ = session.WriteLine( $"_G.display.setcursor( {lineNumber}, {columnNumber} )" );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that displays text. </summary>
    /// <remarks>   2025-04-14. </remarks>
    public static void Display( this Pith.SessionBase session, int lineNumber, int columnNumber, string contents )
    {
        _ = session.WriteLine( "_G.display.clear()" );
        _ = session.WriteLine( $"_G.display.setcursor( {lineNumber}, {columnNumber} )" );
        _ = session.WriteLine( $"_G.display.settext( {contents} )" );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that displays text. </summary>
    /// <remarks>   2025-04-14. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="firstLine">    The first line. </param>
    /// <param name="secondLine">   The second line. </param>
    public static void Display( this Pith.SessionBase session, string firstLine, string secondLine )
    {
        session.ClearDisplay();
        session.Display( 1, 1, firstLine );
        session.Display( 2, 1, secondLine );
    }
}
