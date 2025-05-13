using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.SessionBaseExtensions;

/// <summary>   A session base methods. </summary>
/// <remarks>   2025-04-10. </remarks>
public static partial class SessionBaseMethods
{
    #region " load menu "

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that queries if a load menu item exists.
    /// </summary>
    /// <remarks>   2025-04-21. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="itemName"> Name of the item. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool LoadMenuItemExists( this SessionBase session, string itemName )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( itemName ) ) throw new ArgumentNullException( nameof( itemName ) );

        string query = string.Format( Syntax.Tsp.Display.FindLoadMenuItemQueryFormat, itemName );
        return SessionBase.EqualsTrue( session.QueryTrimEnd( query ) );
    }

    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that deletes the load menu item. </summary>
    /// <remarks>   2025-04-16. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="itemName"> Name of the item. </param>
    public static void DeleteLoadMenuItem( this SessionBase session, string itemName )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( itemName ) ) throw new ArgumentNullException( nameof( itemName ) );

        if ( session.LoadMenuItemExists( itemName ) )
        {
            session.TraceLastAction( $"\r\n\tdeleting the '{itemName}' load menu item;. " );
            string command = string.Format( Syntax.Tsp.Display.DeleteExistingLoadMenuItemCommandFormat, itemName );
            command += " " + Syntax.Tsp.Lua.OperationCompletedQueryCommand;
            _ = session.WriteLine( command );

            // read query reply and throw if reply is not 1.
            session.ReadAndThrowIfOperationIncomplete();

            // throw if device errors
            session.ThrowDeviceExceptionIfError();

            if ( session.LoadMenuItemExists( itemName ) )
                throw new InvalidOperationException( $"The load menu item {itemName} was not deleted." );
        }
    }

    #endregion

    #region " display lines "

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

    #endregion

    #region " display script "

    /// <summary> Gets or sets the restore display command query complete. </summary>
    /// <value> The restore display query complete command. </value>
    public static string RestoreMainCompleteQueryCommand { get; set; } = Syntax.Tsp.Display.RestoreMainCompleteQueryCommand;

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that restores the main display and waits
    /// for the complete query to return '1'.
    /// </summary>
    /// <remarks>   2025-05-12. </remarks>
    /// <param name="session">  The session. </param>
    public static void RestoreMainDisplay( this SessionBase session )
    {
        // Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
        session.SetLastAction( "restoring display" );
        _ = session.WriteLine( $"{RestoreMainCompleteQueryCommand}" );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        _ = session.ReadFiniteLine();
        session.ThrowDeviceExceptionIfError();
    }

    #endregion

}
