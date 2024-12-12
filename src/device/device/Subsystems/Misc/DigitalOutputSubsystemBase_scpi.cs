namespace cc.isr.VI;

public partial class DigitalOutputSubsystemBase
{
    #region " output mode "

    /// <summary> Define output mode read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineOutputModeReadWrites()
    {
        this.OutputModeReadWrites = new();
        foreach ( OutputModes enumValue in Enum.GetValues( typeof( OutputModes ) ) )
            this.OutputModeReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of Output mode parses. </summary>
    /// <value> A Dictionary of Output mode parses. </value>
    public Pith.EnumReadWriteCollection OutputModeReadWrites { get; private set; }

    /// <summary> The supported output modes. </summary>
    private OutputModes _supportedOutputModes;

    /// <summary>
    /// Gets or sets the supported Function Mode. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> The supported Output modes. </value>
    public OutputModes SupportedOutputModes
    {
        get => this._supportedOutputModes;
        set
        {
            if ( !this.SupportedOutputModes.Equals( value ) )
            {
                this._supportedOutputModes = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The output mode. </summary>
    private OutputModes? _outputMode;

    /// <summary> Gets or sets the cached Output mode. </summary>
    /// <value> The <see cref="OutputMode">Output mode</see> or none if not set or unknown. </value>
    public OutputModes? OutputMode
    {
        get => this._outputMode;

        protected set
        {
            if ( !this.OutputMode.Equals( value ) )
            {
                this._outputMode = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Output mode. </summary>
    /// <param name="value"> The  Output mode. </param>
    /// <returns> The <see cref="OutputMode">source Output mode</see> or none if unknown. </returns>
    public OutputModes? ApplyOutputMode( OutputModes value )
    {
        _ = this.WriteOutputMode( value );
        return this.QueryOutputMode();
    }

    /// <summary> Gets or sets the Output mode query command. </summary>
    /// <value> The Output mode query command. </value>
    protected virtual string OutputModeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Output mode. </summary>
    /// <returns> The <see cref="OutputMode">Output mode</see> or none if unknown. </returns>
    public OutputModes? QueryOutputMode()
    {
        this.OutputMode = this.Session.Query( this.OutputMode.GetValueOrDefault( OutputModes.None ), this.OutputModeReadWrites, this.OutputModeQueryCommand );
        return this.OutputMode;
    }

    /// <summary> Gets or sets the Output mode command format. </summary>
    /// <value> The Output mode command format. </value>
    protected virtual string OutputModeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Output mode without reading back the value from the device. </summary>
    /// <param name="value"> The Output mode. </param>
    /// <returns> The <see cref="OutputMode">Output mode</see> or none if unknown. </returns>
    public OutputModes? WriteOutputMode( OutputModes value )
    {
        this.OutputMode = this.Session.Write( value, this.OutputModeCommandFormat, this.OutputModeReadWrites );
        return this.OutputMode;
    }

    #endregion

    #region " digital output active level "

    /// <summary> Define the active level read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineDigitalActiveLevelReadWrites()
    {
        this.DigitalActiveLevelReadWrites = new();
        foreach ( DigitalActiveLevels enumValue in Enum.GetValues( typeof( DigitalActiveLevels ) ) )
            this.DigitalActiveLevelReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of digital active level parses. </summary>
    /// <value> A Dictionary of digital active level parses. </value>
    public Pith.EnumReadWriteCollection DigitalActiveLevelReadWrites { get; private set; }

    /// <summary> The supported digital active levels. </summary>
    private DigitalActiveLevels _supportedDigitalActiveLevels;

    /// <summary>
    /// Gets or sets the supported digital active levels. This is a subset of the functions supported by the
    /// instrument.
    /// </summary>
    /// <value> The supported digital active levels. </value>
    public DigitalActiveLevels SupportedDigitalActiveLevels
    {
        get => this._supportedDigitalActiveLevels;
        set
        {
            if ( !this.SupportedDigitalActiveLevels.Equals( value ) )
            {
                this._supportedDigitalActiveLevels = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The current digital active level. </summary>
    private DigitalActiveLevels? _currentDigitalActiveLevel;

    /// <summary> Gets or sets the current digital active level common to all digital lines. </summary>
    /// <value>
    /// The <see cref="CurrentDigitalActiveLevel">digital active level</see> or none if not set or
    /// unknown.
    /// </value>
    public DigitalActiveLevels? CurrentDigitalActiveLevel
    {
        get => this._currentDigitalActiveLevel;

        protected set
        {
            if ( !this.CurrentDigitalActiveLevel.Equals( value ) )
            {
                this._currentDigitalActiveLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the current digital active level. </summary>
    /// <param name="value"> The digital active level. </param>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">source digital active level</see> or none if unknown.
    /// </returns>
    public DigitalActiveLevels? ApplyDigitalActiveLevel( DigitalActiveLevels value )
    {
        _ = this.WriteDigitalActiveLevel( value );
        return this.QueryDigitalActiveLevel();
    }

    /// <summary> Gets or sets the current digital active level query command. </summary>
    /// <value> The digital active level query command. </value>
    protected virtual string DigitalActiveLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current digital active level. </summary>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">digital active level</see> or none if unknown.
    /// </returns>
    public DigitalActiveLevels? QueryDigitalActiveLevel()
    {
        this.CurrentDigitalActiveLevel = this.Session.Query( this.CurrentDigitalActiveLevel.GetValueOrDefault( DigitalActiveLevels.None ),
            this.DigitalActiveLevelReadWrites, this.DigitalActiveLevelQueryCommand );
        return this.CurrentDigitalActiveLevel;
    }

    /// <summary> Gets or sets the current digital active level command format. </summary>
    /// <value> The digital active level command format. </value>
    protected virtual string DigitalActiveLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the current digital active level without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The digital active level. </param>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">digital active level</see> or none if unknown.
    /// </returns>
    public DigitalActiveLevels? WriteDigitalActiveLevel( DigitalActiveLevels value )
    {
        this.CurrentDigitalActiveLevel = this.Session.Write( value, this.DigitalActiveLevelCommandFormat, this.DigitalActiveLevelReadWrites );
        return this.CurrentDigitalActiveLevel;
    }

    /// <summary> Writes and reads back the current digital active level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="value">      The  digital active level. </param>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">source digital active level</see> or none if unknown.
    /// </returns>
    public DigitalActiveLevels? ApplyDigitalActiveLevel( int lineNumber, DigitalActiveLevels value )
    {
        _ = this.WriteDigitalActiveLevel( lineNumber, value );
        return this.QueryDigitalActiveLevel( lineNumber );
    }

    /// <summary> Queries the current digital active level. Also updates the <see cref="CommonDigitalActiveLevel"/>. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="lineNumber"> The line number. </param>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">digital active level</see> or none if unknown.
    /// </returns>
    public DigitalActiveLevels? QueryDigitalActiveLevel( int lineNumber )
    {
        this.CurrentDigitalActiveLevel = this.Session.Query( this.CurrentDigitalActiveLevel.GetValueOrDefault( DigitalActiveLevels.None ), this.DigitalActiveLevelReadWrites,
            string.Format( this.DigitalActiveLevelQueryCommand, lineNumber ) );
        if ( this.CurrentDigitalActiveLevel.HasValue )
        {
            this.DigitalOutputLines.Update( lineNumber, this.CurrentDigitalActiveLevel.Value );
            this.CommonDigitalActiveLevel = this.DigitalOutputLines.DigitalActiveLevel;
        }
        else
        {
            this.DigitalOutputLines.Update( lineNumber, DigitalActiveLevels.None );
        }

        this.NotifyPropertyChanged( nameof( this.DigitalOutputLines ) );
        return this.CurrentDigitalActiveLevel;
    }

    /// <summary>
    /// Writes the digital active level without reading back the value from the device. Also updates the <see cref="CommonDigitalActiveLevel"/> />
    /// </summary>
    /// <param name="lineNumber"> The output line number. </param>
    /// <param name="value">            The digital active level. </param>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">digital active level</see> or none if unknown.
    /// </returns>
    public DigitalActiveLevels? WriteDigitalActiveLevel( int lineNumber, DigitalActiveLevels value )
    {
        this.CurrentDigitalActiveLevel = this.Session.Write( value, string.Format( this.DigitalActiveLevelCommandFormat, lineNumber ), this.DigitalActiveLevelReadWrites );
        if ( this.CurrentDigitalActiveLevel.HasValue )
        {
            this.DigitalOutputLines.Update( lineNumber, this.CurrentDigitalActiveLevel.Value );
            this.CommonDigitalActiveLevel = this.DigitalOutputLines.DigitalActiveLevel;
        }
        else
        {
            this.DigitalOutputLines.Update( lineNumber, DigitalActiveLevels.None );
        }

        this.NotifyPropertyChanged( nameof( this.DigitalOutputLines ) );
        return this.CurrentDigitalActiveLevel;
    }

    /// <summary> The common digital active level. </summary>
    private DigitalActiveLevels? _commonDigitalActiveLevel;

    /// <summary> Gets or sets the cached digital active level of all lines. </summary>
    /// <value>
    /// The <see cref="CommonDigitalActiveLevel">digital active level</see> or none if not set or not
    /// the same for all lines.
    /// </value>
    public DigitalActiveLevels? CommonDigitalActiveLevel
    {
        get => this._commonDigitalActiveLevel;

        protected set
        {
            if ( !this.CommonDigitalActiveLevel.Equals( value ) )
            {
                this._commonDigitalActiveLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the digital output lines. </summary>
    /// <value> The digital output lines. </value>
    public DigitalOutputLineCollection DigitalOutputLines { get; private set; }

    /// <summary> Applies the output level. </summary>
    /// <param name="lineNumbers"> The output lines. </param>
    /// <param name="value">       The  digital active level. </param>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">source digital active level</see> or none if unknown.
    /// </returns>
    public DigitalActiveLevels? ApplyDigitalActiveLevel( IEnumerable<int> lineNumbers, DigitalActiveLevels value )
    {
        foreach ( int lineNumber in lineNumbers )
            _ = this.ApplyDigitalActiveLevel( lineNumber, value );
        return this.CommonDigitalActiveLevel;
    }

    /// <summary> Writes and reads back the output levels. </summary>
    /// <param name="outputLines"> The output lines. </param>
    /// <param name="values">      The values. </param>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">source digital active level</see> or none if unknown.
    /// </returns>
    public IEnumerable<DigitalActiveLevels?> ApplyDigitalActiveLevels( IEnumerable<int> outputLines, IEnumerable<DigitalActiveLevels> values )
    {
        List<DigitalActiveLevels?> result = [];
        for ( int i = 0, loopTo = outputLines.Count() - 1; i <= loopTo; i++ )
            result.Add( this.ApplyDigitalActiveLevel( outputLines.ElementAtOrDefault( i ), values.ElementAtOrDefault( i ) ) );
        return result;
    }

    /// <summary> Queries the digital active levels. </summary>
    /// <param name="outputLines"> The output lines. </param>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">digital active level</see> or none if unknown.
    /// </returns>
    public DigitalActiveLevels? QueryDigitalActiveLevels( IEnumerable<int> outputLines )
    {
        if ( !string.IsNullOrWhiteSpace( this.DigitalActiveLevelQueryCommand ) )
        {
            foreach ( int lineNumber in outputLines )
                _ = this.QueryDigitalActiveLevel( lineNumber );
        }

        return this.CommonDigitalActiveLevel;
    }

    /// <summary> Queries the polarities and levels of all signal lines. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    public void QueryDigitalOutputLines()
    {
        _ = this.QueryDigitalActiveLevels( this.DigitalOutputLines.DigitalOutputLineNumbers );
        foreach ( int lineNumber in this.DigitalOutputLines.DigitalOutputLineNumbers )
            _ = this.QueryLineLevel( lineNumber );
    }

    /// <summary>
    /// Writes the digital active level without reading back the value from the device.
    /// </summary>
    /// <param name="outputLines"> The output lines. </param>
    /// <param name="value">       The digital active level. </param>
    /// <returns>
    /// The <see cref="CurrentDigitalActiveLevel">digital active level</see> or none if unknown.
    /// </returns>
    public DigitalActiveLevels? WriteDigitalActiveLevel( IEnumerable<int> outputLines, DigitalActiveLevels value )
    {
        if ( !string.IsNullOrWhiteSpace( this.DigitalActiveLevelQueryCommand ) )
        {
            foreach ( int lineNumber in outputLines )
                _ = this.WriteDigitalActiveLevel( lineNumber, value );
        }

        return this.CommonDigitalActiveLevel;
    }

    #endregion
}
/// <summary> A digital output line. </summary>
public class DigitalOutputLine
{
    /// <summary> Gets the line number. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The line number. </value>
    public int LineNumber { get; set; }

    /// <summary> Gets the digital active level. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The digital active. </value>
    public DigitalActiveLevels ActiveLevel { get; set; }

    /// <summary> Gets the line level. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The line level. </value>
    public int? LineLevel { get; set; }
}
/// <summary> Collection of digital output lines. </summary>
public class DigitalOutputLineCollection : System.Collections.ObjectModel.KeyedCollection<int, DigitalOutputLine>
{
    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns> The key for the specified element. </returns>
    protected override int GetKeyForItem( DigitalOutputLine item )
    {
        return item is null ? throw new ArgumentNullException( nameof( DigitalOutputLine ) ) : item.LineNumber;
    }

    /// <summary> Updates the digital active level. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="activeLevel">   The active level. </param>
    public void Update( int lineNumber, DigitalActiveLevels activeLevel )
    {
        if ( !this.Contains( lineNumber ) )
        {
            this.Add( new DigitalOutputLine() { LineNumber = lineNumber } );
        }

        this[lineNumber].ActiveLevel = activeLevel;
    }

    /// <summary> Updates the line level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="lineLevel">  The line level. </param>
    public void Update( int lineNumber, int lineLevel )
    {
        if ( !this.Contains( lineNumber ) )
        {
            this.Add( new DigitalOutputLine() { LineNumber = lineNumber } );
        }

        this[lineNumber].LineLevel = lineLevel;
    }

    /// <summary> Updates the digital active level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="lineLevel">  The line level. </param>
    public void Update( int lineNumber, int? lineLevel )
    {
        if ( !this.Contains( lineNumber ) )
        {
            this.Add( new DigitalOutputLine() { LineNumber = lineNumber } );
        }

        this[lineNumber].LineLevel = lineLevel;
    }

    /// <summary> Updates the digital active level. </summary>
    /// <remarks> David, 2020-11-12. </remarks>
    /// <param name="lineNumber">  The line number. </param>
    /// <param name="activeLevel"> The active level. </param>
    /// <param name="lineLevel">   The line level. </param>
    public void Update( int lineNumber, DigitalActiveLevels activeLevel, int lineLevel )
    {
        if ( !this.Contains( lineNumber ) )
        {
            this.Add( new DigitalOutputLine() { LineNumber = lineNumber } );
        }

        this[lineNumber].LineLevel = lineLevel;
        this[lineNumber].ActiveLevel = activeLevel;
    }

    /// <summary> Gets the digital output line numbers. </summary>
    /// <value> The digital output line numbers. </value>
    public IEnumerable<int> DigitalOutputLineNumbers
    {
        get
        {
            List<int> l = [];
            foreach ( DigitalOutputLine dol in this )
                l.Add( dol.LineNumber );
            return l;
        }
    }

    /// <summary> Gets the active levels. </summary>
    /// <value> The active levels. </value>
    public IEnumerable<DigitalActiveLevels> ActiveLevels
    {
        get
        {
            List<DigitalActiveLevels> l = [];
            foreach ( DigitalOutputLine dol in this )
                l.Add( dol.ActiveLevel );
            return l;
        }
    }

    /// <summary> Gets the line levels. </summary>
    /// <value> The line levels. </value>
    public IEnumerable<int?> LineLevels
    {
        get
        {
            List<int?> l = [];
            foreach ( DigitalOutputLine dol in this )
                l.Add( dol.LineLevel );
            return l;
        }
    }

    /// <summary> Gets the polarity. Return nothing if values are not all the same. </summary>
    /// <value>
    /// The digital active level common to all line. Nothing if digital active level is not the same
    /// for all lines.
    /// </value>
    public DigitalActiveLevels? DigitalActiveLevel
    {
        get
        {
            DigitalActiveLevels? result = new global::cc.isr.VI.DigitalActiveLevels?();
            if ( this.Any() )
            {
                foreach ( DigitalOutputLine dol in this )
                {
                    if ( result.HasValue )
                    {
                        if ( result.Value != dol.ActiveLevel )
                        {
                            result = default;
                            break;
                        }
                    }
                    else
                    {
                        result = dol.ActiveLevel;
                    }
                }
            }

            return result;
        }
    }
}
/// <summary> Enumerates the digital output mode. Defined
/// with the <see cref="FlagsAttribute"/> for defining the
/// <see cref="VI.DigitalOutputSubsystemBase.SupportedOutputModes"/></summary>
[Flags]
public enum OutputModes
{
    /// <summary> the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 0,

    /// <summary> Using line 4 (2400) as end of test signal. </summary>
    [System.ComponentModel.Description( "End of test (EOT)" )]
    EndTest = 1,

    /// <summary> Using line 4 (2400) as busy signal. </summary>
    [System.ComponentModel.Description( "Busy (BUSY)" )]
    Busy = 2
}
/// <summary>
/// Enumerates the digital active level (high or low) of the end of test or busy signals. Defined
/// with the <see cref="FlagsAttribute"/> for defining the
/// <see cref="VI.DigitalOutputSubsystemBase.SupportedDigitalActiveLevels"/>
/// </summary>
/// <remarks> David, 2020-11-12. </remarks>
[Flags]
public enum DigitalActiveLevels
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "Not Defined ()" )]
    None = 0,

    /// <summary> An enum constant representing the active high option. </summary>
    [System.ComponentModel.Description( "High (HI)" )]
    High = 1,

    /// <summary> An enum constant representing the active low option. </summary>
    [System.ComponentModel.Description( "Low (LO)" )]
    Low = 2
}
