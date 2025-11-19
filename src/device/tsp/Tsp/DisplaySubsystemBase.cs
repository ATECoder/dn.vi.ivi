// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp;

/// <summary> Defines a System Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-10-07 </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">status
/// Subsystem</see>. </param>
public abstract class DisplaySubsystemBase( Tsp.StatusSubsystemBase statusSubsystem ) : VI.DisplaySubsystemBase( statusSubsystem )
{
    #region " exists "

    /// <summary>
    /// Reads the display existence indicator. Some TSP instruments (e.g., 3706) may have no display.
    /// </summary>
    /// <returns> <c>true</c> if the display exists; otherwise, <c>false</c>. </returns>
    public override bool? QueryExists()
    {
        // detect the display
        this.Session.MakeEmulatedReplyIfEmpty( this.Exists.GetValueOrDefault( true ) );
        this.Exists = !this.Session.IsNil( Syntax.Tsp.Display.SubsystemName );
        return this.Exists;
    }

    #endregion

    #region " clear "

    /// <summary> Gets or sets the clear command. </summary>
    /// <value> The clear command. </value>
    protected override string ClearCommand { get; set; } = Syntax.Tsp.Display.ClearCommand;

    /// <summary> Clears the display. </summary>
    /// <remarks> Sets the display to the user mode. </remarks>
    public override void ClearDisplay()
    {
        this.DisplayScreen = DisplayScreens.User;
        if ( this.QueryExists().GetValueOrDefault( false ) )
        {
            _ = this.Session.WriteLine( Syntax.Tsp.Display.ClearCommand );
        }
    }

    /// <summary> Clears the display if not in measurement mode and set measurement mode. </summary>
    /// <remarks> Sets the display to user. </remarks>
    public void TryClearDisplayMeasurement()
    {
        if ( this.Exists.GetValueOrDefault( false ) && ( int? ) (this.DisplayScreen & DisplayScreens.Measurement) == 0 )
        {
            _ = this.TryClearDisplay();
        }
    }

    /// <summary> Clears the display if not in measurement mode and set measurement mode. </summary>
    /// <remarks> Sets the display to the measurement. </remarks>
    public void ClearDisplayMeasurement()
    {
        if ( this.QueryExists().GetValueOrDefault( false ) && ( int? ) (this.DisplayScreen & DisplayScreens.Measurement) == 0 )
        {
            this.ClearDisplay();
        }

        this.DisplayScreen = DisplayScreens.Measurement;
    }

    #endregion

    #region " display character "

    /// <summary> Displays the character. </summary>
    /// <param name="lineNumber">      The line number. </param>
    /// <param name="position">        The position. </param>
    /// <param name="characterNumber"> The character number. </param>
    public void DisplayCharacter( int lineNumber, int position, int characterNumber )
    {
        this.DisplayScreen = DisplayScreens.User | DisplayScreens.Custom;
        if ( !this.QueryExists().GetValueOrDefault( false ) )
        {
            return;
        }
        // ignore empty character.
        if ( characterNumber is <= 0 or > Syntax.Tsp.Display.MaximumCharacterNumber )
        {
            return;
        }

        _ = this.Session.WriteLine( Syntax.Tsp.Display.SetCursorCommandFormat, lineNumber, position );
        _ = this.Session.WriteLine( Syntax.Tsp.Display.SetCharacterCommandFormat, characterNumber );
    }

    #endregion

    #region " display line "

    /// <summary> Displays a message on the display. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="value">      The value. </param>
    public override void DisplayLine( int lineNumber, string value )
    {
        this.DisplayScreen = DisplayScreens.User | DisplayScreens.Custom;
        if ( !this.QueryExists().GetValueOrDefault( false ) )
        {
            return;
        }

        // ignore empty strings.
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            return;
        }

        int length = Syntax.Tsp.Display.FirstLineLength;
        if ( lineNumber < 1 )
        {
            lineNumber = 1;
        }
        else if ( lineNumber > 2 )
        {
            lineNumber = 2;
        }

        if ( lineNumber == 2 )
        {
            length = Syntax.Tsp.Display.SecondLineLength;
        }

        _ = this.Session.WriteLine( Syntax.Tsp.Display.SetCursorLineCommandFormat, lineNumber );
        if ( value.Length < length )
            value = value.PadRight( length );
        _ = this.Session.WriteLine( Syntax.Tsp.Display.SetTextCommandFormat, value );
    }

    /// <summary> Displays the program title. </summary>
    /// <param name="title">    Top row data. </param>
    /// <param name="subtitle"> Bottom row data. </param>
    public void DisplayTitle( string title, string subtitle )
    {
        this.DisplayLine( 1, title );
        this.DisplayLine( 2, subtitle );
    }

    #endregion

    #region " restore "

    /// <summary> Gets or sets the restore display command query complete. </summary>
    /// <value> The restore display query complete command. </value>
    public string RestoreMainCompleteQueryCommand { get; set; } = Syntax.Tsp.Display.RestoreMainCompleteQueryCommand;

    /// <summary> Restores the instrument display getting the complete query message. </summary>
    public void RestoreDisplayCompleteQuery()
    {
        this.DisplayScreen = DisplayScreens.Default;
        if ( this.Exists.HasValue && this.Exists.Value && !string.IsNullOrWhiteSpace( this.RestoreMainCompleteQueryCommand ) )
        {
            // Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
            this.Session.SetLastAction( "restoring display" );
            _ = this.Session.WriteLine( $"{this.RestoreMainCompleteQueryCommand}" );
            _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
            _ = this.Session.ReadFiniteLine();
            this.Session.ThrowDeviceExceptionIfError();
        }
    }

    /// <summary> Gets or sets the restore display command. </summary>
    /// <value> The restore display command. </value>
    public string RestoreMainWaitCommand { get; set; } = Syntax.Tsp.Display.RestoreMainWaitCommand;

    /// <summary> Restores the instrument display. </summary>
    public void RestoreDisplay()
    {
        this.DisplayScreen = DisplayScreens.Default;
        if ( this.Exists.HasValue && this.Exists.Value && !string.IsNullOrWhiteSpace( this.RestoreMainWaitCommand ) )
        {
            // Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
            this.Session.SetLastAction( "restoring display" );
            _ = this.Session.WriteLine( this.RestoreMainWaitCommand );
            _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );

            this.Session.ThrowDeviceExceptionIfError();

            // output the operation completion result of '1'
            _ = this.Session.WriteLine( Syntax.Tsp.Lua.OperationCompletedQueryCommand );
            _ = SessionBase.AsyncDelay( this.Session.ReadAfterWriteDelay + this.Session.StatusReadDelay );
            this.Session.ReadAndThrowIfOperationIncomplete();
        }
    }

    #endregion
}
