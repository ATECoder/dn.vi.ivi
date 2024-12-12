using cc.isr.VI.ExceptionExtensions;

namespace cc.isr.VI;

/// <summary> Defines a SCPI Display Subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2005-01-15, 1.0.1841.x. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class DisplaySubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " clear caution messages (impedance analyzer)"

    /// <summary> Gets or sets the clear caution messages command. </summary>
    /// <remarks> SCPI: ":DISP:CCL". </remarks>
    /// <value> The Abort command. </value>
    protected virtual string ClearCautionMessagesCommand { get; set; } = string.Empty;

    /// <summary> Clears the caution messages. </summary>
    public void ClearCautionMessages()
    {
        _ = this.Session.WriteLine( this.ClearCautionMessagesCommand );
    }

    #endregion

    #region " clear display "

    /// <summary> Gets or sets the clear command. </summary>
    /// <remarks> SCPI: ":DISP:CLE". </remarks>
    /// <value> The clear command. </value>
    protected virtual string ClearCommand { get; set; } = string.Empty;

    /// <summary> Clears the triggers. </summary>
    public virtual void ClearDisplay()
    {
        if ( this.QueryExists().GetValueOrDefault( false ) )
            _ = this.Session.WriteLine( this.ClearCommand );
    }

    /// <summary> Clears the display without raising exceptions. </summary>
    /// <returns> <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public bool TryClearDisplay()
    {
        Pith.ServiceRequests statusByte = Pith.ServiceRequests.None;
        string details;
        try
        {
            this.Session.SetLastAction( "clearing display" );
            this.ClearDisplay();
            _ = this.Session.TraceInformation();
            (statusByte, details) = this.Session.TraceDeviceExceptionIfError();
        }
        catch ( Pith.NativeException ex )
        {
            _ = this.Session.TraceException( ex );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            _ = this.Session.TraceException( ex );
        }

        return !this.Session.IsErrorBitSet( statusByte );
    }

    #endregion

    #region " display line "

    /// <summary> Gets or sets the Display Line command. </summary>
    /// <remarks> SCPI: ":DISP:USER{0}:TEXT ""{1}""". </remarks>
    /// <value> The DisplayText command. </value>
    protected virtual string DisplayLineCommandFormat { get; set; } = string.Empty;

    /// <summary> Displays a line of text. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="value">      if set to <c>true</c> if enabling; False if disabling. </param>
    public virtual void DisplayLine( int lineNumber, string value )
    {
        if ( !string.IsNullOrWhiteSpace( this.DisplayLineCommandFormat ) )
        {
            _ = this.Session.WriteLine( string.Format( this.DisplayLineCommandFormat, lineNumber, value ) );
        }
    }

    /// <summary> Displays a line of text. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="format">     Describes the format to use. </param>
    /// <param name="args">       A variable-length parameters list containing arguments. </param>
    public virtual void DisplayLine( int lineNumber, string format, params object[] args )
    {
        this.DisplayLine( lineNumber, string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
    }

    #endregion

    #region " enabled "

    /// <summary> Enabled. </summary>
    private bool? _enabled;

    /// <summary> Gets or sets the cached Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Enabled
    {
        get => this._enabled;

        protected set
        {
            if ( !Equals( this.Enabled, value ) )
            {
                this._enabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyEnabled( bool value )
    {
        _ = this.WriteEnabled( value );
        return string.IsNullOrWhiteSpace( this.DisplayEnabledQueryCommand ) ? this.Enabled : this.QueryEnabled();
    }

    /// <summary> Gets or sets the display enabled query command. </summary>
    /// <value> The display enabled query command. </value>
    protected virtual string DisplayEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Enabled sentinel. Also sets the
    /// <see cref="Enabled">enabled</see> sentinel.
    /// </summary>
    /// <returns>
    /// <c>null</c> display status is not known; <c>true</c> if enabled; otherwise, <c>false</c>.
    /// </returns>
    public bool? QueryEnabled()
    {
        this.Session.MakeEmulatedReplyIfEmpty( this.Enabled.GetValueOrDefault( true ) );
        this.Enabled = this.Session.Query( this.Enabled.GetValueOrDefault( true ), this.DisplayEnabledQueryCommand );
        return this.Enabled;
    }

    /// <summary> Gets or sets the display enable command format. </summary>
    /// <value> The display enable command format. </value>
    protected virtual string DisplayEnableCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Forced Digital Output Pattern  Enabled sentinel. Does not read back from the
    /// instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns>
    /// <c>null</c> if display status is not known; <c>true</c> if enabled; otherwise, <c>false</c>.
    /// </returns>
    public bool? WriteEnabled( bool value )
    {
        _ = this.Session.WriteLine( this.DisplayEnableCommandFormat, value.GetHashCode() );
        this.Enabled = value;
        return this.Enabled;
    }

    #endregion

    #region " exists "

    /// <summary> The exists. </summary>
    private bool? _exists;

    /// <summary> Gets or sets the cached Exists sentinel. </summary>
    /// <value>
    /// <c>null</c> if display exists is not known; <c>true</c> if display exists is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Exists
    {
        get => this._exists;

        protected set
        {
            if ( !Equals( this.Exists, value ) )
            {
                this._exists = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the display exists query command. </summary>
    /// <value> The display Installed query command. </value>
    protected virtual string DisplayExistsQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the display existence sentinel. Also sets the
    /// <see cref="Exists">installed</see> sentinel.
    /// </summary>
    /// <returns>
    /// <c>null</c> status is not known; <c>true</c> if display exists; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool? QueryExists()
    {
        this.Session.MakeEmulatedReplyIfEmpty( this.Exists.GetValueOrDefault( true ) );
        this.Exists = this.Session.Query( this.Exists.GetValueOrDefault( true ), this.DisplayExistsQueryCommand );
        return this.Exists;
    }

    #endregion

    #region " display screen  "

    /// <summary> The Display Screen. </summary>
    private DisplayScreens? _displayScreen;

    /// <summary> Gets or sets the cached Display Screen. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public DisplayScreens? DisplayScreen
    {
        get => this._displayScreen;

        protected set
        {
            if ( !Nullable.Equals( this.DisplayScreen, value ) )
            {
                this._displayScreen = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Display Screen. </summary>
    /// <param name="value"> The Display Screen. </param>
    /// <returns> The Display Screen. </returns>
    public DisplayScreens? ApplyDisplayScreen( DisplayScreens value )
    {
        _ = this.WriteDisplayScreens( value );
        return this.QueryDisplayScreens();
    }

    /// <summary> Gets or sets the Display Screen  query command. </summary>
    /// <value> The Display Screen  query command. </value>
    protected virtual string DisplayScreenQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Display Screen. </summary>
    /// <returns> The Display Screen  or none if unknown. </returns>
    public DisplayScreens? QueryDisplayScreens()
    {
        this.DisplayScreen = this.Session.QueryEnum( this.DisplayScreen, this.DisplayScreenQueryCommand );
        return this.DisplayScreen;
    }

    /// <summary> Gets or sets the Display Screen  command format. </summary>
    /// <value> The Display Screen  command format. </value>
    protected virtual string DisplayScreenCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Display Screen  without reading back the value from the device. </summary>
    /// <remarks> This command sets the Display Screen. </remarks>
    /// <param name="value"> The Display Screen. </param>
    /// <returns> The Display Screen. </returns>
    public DisplayScreens? WriteDisplayScreens( DisplayScreens value )
    {
        this.DisplayScreen = this.Session.WriteEnum( value, this.DisplayScreenCommandFormat );
        return this.DisplayScreen;
    }

    #endregion
}

/// <summary> A bit-field of flags for specifying display screens. </summary>
/// <remarks> Screens 0 to 1024 are used by 2600 TSP 1. </remarks>
[Flags]
public enum DisplayScreens
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not specified" )]
    None = 0,

    /// <summary> Special User Screens </summary>
    [System.ComponentModel.Description( "Default screen" )]
    Default = 1,

    /// <summary>Cleared user screen mode.</summary>
    [System.ComponentModel.Description( "User screen" )]
    User = 128,

    /// <summary>Last command displayed title.</summary>
    [System.ComponentModel.Description( "Title is displayed" )]
    Title = 256,

    /// <summary>Custom lines are displayed.</summary>
    [System.ComponentModel.Description( "Special display" )]
    Custom = 512,

    /// <summary>Measurement displayed.</summary>
    [System.ComponentModel.Description( "Measurement is displayed" )]
    Measurement = 1024,

    /// <summary> 2450+7500 SCREENS. </summary>
    [System.ComponentModel.Description( "Home (HOME)" )]
    Home = Measurement << 1,

    /// <summary> An enum constant representing the home large reading option. </summary>
    [System.ComponentModel.Description( "Home Large Reading (HOME_LARG)" )]
    HomeLargeReading = Home + 1,

    /// <summary> An enum constant representing the reading table option. </summary>
    [System.ComponentModel.Description( "Reading Table (READ)" )]
    ReadingTable = Home + 2,

    /// <summary> An enum constant representing the graph option. </summary>
    [System.ComponentModel.Description( "Graph (GRAP)" )]
    Graph = Home + 3,

    /// <summary> An enum constant representing the histogram option. </summary>
    [System.ComponentModel.Description( "Histogram (HIST)" )]
    Histogram = Home + 4,

    /// <summary> An enum constant representing the graph swipe option. </summary>
    [System.ComponentModel.Description( "Graph swipe (SWIPE_GRAP)" )]
    GraphSwipe = Home + 5,

    /// <summary> An enum constant representing the settings swipe option. </summary>
    [System.ComponentModel.Description( "Settings swipe (SWIPE_SETT)" )]
    SettingsSwipe = Home + 6,

    /// <summary> An enum constant representing the statistics swipe option. </summary>
    [System.ComponentModel.Description( "Statistics swipe (SWIPE_STAT)" )]
    StatisticsSwipe = Home + 7,

    /// <summary> An enum constant representing the user swipe option. </summary>
    [System.ComponentModel.Description( "User swipe (SWIPE_USER)" )]
    UserSwipe = Home + 8,

    /// <summary> An enum constant representing the source swipe option (2450 screens). </summary>
    [System.ComponentModel.Description( "Source swipe (SOUR)" )]
    SourceSwipe = Home + 9,

    /// <summary> An enum constant representing the functions swipe option (7510 screens). </summary>
    [System.ComponentModel.Description( "Functions swipe (SWIPE_FUNC)" )]
    FunctionsSwipe = Home + 10,

    /// <summary> An enum constant representing the secondary swipe option. </summary>
    [System.ComponentModel.Description( "Secondary swipe (SWIPE_SEC)" )]
    SecondarySwipe = Home + 11,

    /// <summary> An enum constant representing the processing option. </summary>
    [System.ComponentModel.Description( "Processing (PROC)" )]
    Processing = Home + 12
}
