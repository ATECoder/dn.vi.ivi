using System;

namespace cc.isr.VI.Tsp2
{
    /// <summary> Defines a System Subsystem for a TSP System. </summary>
    /// <remarks>
    /// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
    /// Licensed under The MIT License. </para><para>
    /// David, 2013-10-07 </para>
    /// </remarks>
    public abstract class DisplaySubsystemBase : VI.DisplaySubsystemBase
    {
        #region " construction and cleanup "

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
        /// </summary>
        /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">status
        /// Subsystem</see>. </param>
        protected DisplaySubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
        {
            this.RestoreMainScreenWaitCompleteCommand = Syntax.Tsp.Display.RestoreMainWaitCompleteCommand;
        }

        #endregion

        #region " i presettable "

        /// <summary>
        /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
        /// default values.
        /// </summary>
        public override void DefineKnownResetState()
        {
            base.DefineKnownResetState();
            this.Enabled = true;
            this.Exists = true;
        }

        #endregion

        #region " command syntax "

        #region " display screen  "

        /// <summary> Gets or sets the display Screen command format. </summary>
        /// <value> The display Screen command format. </value>
        protected override string DisplayScreenCommandFormat { get; set; } = Syntax.Tsp.Display.DisplayScreenCommandFormat;

        /// <summary> Gets or sets the display Screen query command. </summary>
        /// <value> The display Screen query command. </value>
        protected override string DisplayScreenQueryCommand { get; set; } = string.Empty;

        #endregion

        #region " enabled "

        /// <summary> Gets or sets the display enable command format. </summary>
        /// <value> The display enable command format. </value>
        protected override string DisplayEnableCommandFormat { get; set; } = string.Empty;

        /// <summary> Gets or sets the display enabled query command. </summary>
        /// <value> The display enabled query command. </value>
        protected override string DisplayEnabledQueryCommand { get; set; } = string.Empty;

        #endregion

        #endregion

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

        /// <summary> Gets or sets the default display screen. </summary>
        /// <value> The default display. </value>
        public DisplayScreens DefaultScreen { get; set; } = DisplayScreens.User;

        /// <summary> Gets or sets the clear command. </summary>
        /// <value> The clear command. </value>
        protected override string ClearCommand { get; set; } = Syntax.Tsp.Display.ClearCommand;

        /// <summary> Clears the display. </summary>
        /// <remarks> Sets the display to the user mode. </remarks>
        public override void ClearDisplay()
        {
            this.DisplayScreen = this.DefaultScreen;
            base.ClearDisplay();
        }

        /// <summary> Clears the display if not in measurement mode and set measurement mode. </summary>
        /// <remarks> Sets the display to user. </remarks>
        public void TryClearDisplayMeasurement()
        {
            if ( (this.Exists.GetValueOrDefault( false ) && ( int? ) (this.DisplayScreen & DisplayScreens.Measurement) == 0) == true )
            {
                _ = this.TryClearDisplay();
            }
        }

        /// <summary> Gets or sets the measurement screen. </summary>
        /// <value> The measurement screen. </value>
        public DisplayScreens MeasurementScreen { get; set; } = DisplayScreens.Measurement;

        /// <summary> Clears the display if not in measurement mode and set measurement mode. </summary>
        /// <remarks> Sets the display to the measurement. </remarks>
        public void ClearDisplayMeasurement()
        {
            if ( (this.QueryExists().GetValueOrDefault( false ) && ( int? ) (this.DisplayScreen & DisplayScreens.Measurement) == 0) == true )
            {
                this.ClearDisplay();
            }

            this.DisplayScreen = this.MeasurementScreen;
        }

        #endregion

        #region " user screen text "

        /// <summary> Gets or sets the user screen. </summary>
        /// <value> The user screen. </value>
        public DisplayScreens UserScreen { get; set; } = DisplayScreens.UserSwipe;

        /// <summary> Displays a message on the display. </summary>
        /// <param name="lineNumber"> The line number. </param>
        /// <param name="value">      The value. </param>
        public override void DisplayLine( int lineNumber, string value )
        {
            if ( this.QueryExists().GetValueOrDefault( false ) && !string.IsNullOrWhiteSpace( value ) )
            {
                lineNumber = Math.Max( 1, Math.Min( 2, lineNumber ) );
                int length = lineNumber == 1 ? Syntax.Tsp.Display.FirstLineLength : Syntax.Tsp.Display.SecondLineLength;
                if ( value.Length < length )
                    value = value.PadRight( length );
                _ = this.Session.WriteLine( Syntax.Tsp.Display.SetTextLineCommandFormat, lineNumber, value );
                this.DisplayScreen = this.UserScreen;
            }
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

        /// <summary> Gets or sets the restore display command. </summary>
        /// <value> The restore display command. </value>
        public string RestoreMainScreenWaitCompleteCommand { get; set; }

        /// <summary> Restores the instrument display. </summary>
        /// <param name="timeout"> The timeout. </param>
        public void RestoreDisplay( TimeSpan timeout )
        {
            this.DisplayScreen = DisplayScreens.Default;
            if ( this.Exists.HasValue && this.Exists.Value && !string.IsNullOrWhiteSpace( this.RestoreMainScreenWaitCompleteCommand ) )
            {
                this.Session.EnableWaitComplete();
                // Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
                _ = this.Session.WriteLine( this.RestoreMainScreenWaitCompleteCommand );
                _ = this.Session.ApplyStatusByte( this.Session.AwaitOperationCompletion( timeout ).Status );
            }
        }

        /// <summary> Restores the instrument display. </summary>
        public void RestoreDisplay()
        {
            this.DisplayScreen = DisplayScreens.Default;
            if ( this.Exists.HasValue && this.Exists.Value && !string.IsNullOrWhiteSpace( this.RestoreMainScreenWaitCompleteCommand ) )
            {
                // Documentation error: Display Main equals 1, not 0. This code should work on other instruments.
                _ = this.Session.WriteLine( this.RestoreMainScreenWaitCompleteCommand );
                _ = SessionBase.AsyncDelay( this.Session.StatusReadDelay );
                _ = this.Session.QueryOperationCompleted();
            }
        }

        #endregion

    }
}
