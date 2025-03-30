namespace cc.isr.VI.Tsp2;

/// <summary> Defines a DigitalInputOutput Subsystem for a TSP System. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2016-01-15 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class DigitalInputOutputSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplaySubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">status
    /// Subsystem</see>. </param>
    protected DigitalInputOutputSubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
    }

    #endregion

    #region " digital lines "

    /// <summary> The digital lines. </summary>

    /// <summary> Gets or sets the digital lines. </summary>
    /// <value> The digital lines. </value>
    public DigitalLineCollection DigitalLines { get; protected set; }

    #endregion

    #region " digital line mode  "

    /// <summary> Writes and reads back the Digital Input Output Digital Line Mode. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="value">      The Digital Input Output Digital Line Mode. </param>
    /// <returns>
    /// The <see cref="DigitalLineMode">DigitalInputOutput Digital Line Mode</see> or none if unknown.
    /// </returns>
    public DigitalLineMode? ApplyDigitalLineMode( int lineNumber, DigitalLineMode value )
    {
        return this.ApplyDigitalLineMode( this.DigitalLines[lineNumber], value );
    }

    /// <summary> Writes and reads back the Digital Input Output Digital Line Mode. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="digitalLine"> The digital line. </param>
    /// <param name="value">       The Digital Input Output Digital Line Mode. </param>
    /// <returns>
    /// The <see cref="DigitalLineMode">DigitalInputOutput Digital Line Mode</see> or none if unknown.
    /// </returns>
    public DigitalLineMode? ApplyDigitalLineMode( DigitalLine digitalLine, DigitalLineMode value )
    {
        if ( digitalLine is null )
            throw new ArgumentNullException( nameof( digitalLine ) );
        _ = this.WriteDigitalLineMode( digitalLine, value );
        _ = this.QueryDigitalLineMode( digitalLine );
        return digitalLine.DigitalLineMode;
    }

    /// <summary> Get the Digital Line Mode query command. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected abstract string DigitalLineModeQueryCommand( int lineNumber );

    /// <summary> Queries the Digital Line Mode. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <returns>
    /// The <see cref="DigitalLineMode">Digital Line Mode</see> or none if unknown.
    /// </returns>
    public DigitalLineMode? QueryDigitalLineMode( int lineNumber )
    {
        return this.QueryDigitalLineMode( this.DigitalLines[lineNumber] );
    }

    /// <summary> Queries the Digital Line Mode. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="digitalLine"> The digital line. </param>
    /// <returns>
    /// The <see cref="DigitalLineMode">Digital Line Mode</see> or none if unknown.
    /// </returns>
    public virtual DigitalLineMode? QueryDigitalLineMode( DigitalLine digitalLine )
    {
        return digitalLine is null
            ? throw new ArgumentNullException( nameof( digitalLine ) )
            : digitalLine.QueryDigitalLineMode( this.Session, this.DigitalLineModeQueryCommand( digitalLine.LineNumber ) );
    }

    /// <summary> Gets Digital Line Mode command format. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected abstract string DigitalLineModeCommandFormat( int lineNumber );

    /// <summary>
    /// Writes the Digital Line Mode without reading back the value from the device.
    /// </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="value">      The Digital Line Mode. </param>
    /// <returns>
    /// The <see cref="DigitalLineMode">Digital Line Mode</see> or none if unknown.
    /// </returns>
    public DigitalLineMode? WriteDigitalLineMode( int lineNumber, DigitalLineMode value )
    {
        return this.WriteDigitalLineMode( this.DigitalLines[lineNumber], value );
    }

    /// <summary>
    /// Writes the Digital Line Mode without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="digitalLine"> The digital line. </param>
    /// <param name="value">       The Digital Line Mode. </param>
    /// <returns>
    /// The <see cref="DigitalLineMode">Digital Line Mode</see> or none if unknown.
    /// </returns>
    public virtual DigitalLineMode? WriteDigitalLineMode( DigitalLine digitalLine, DigitalLineMode value )
    {
        return digitalLine is null
            ? throw new ArgumentNullException( nameof( digitalLine ) )
            : digitalLine.WriteDigitalLineMode( this.Session, this.DigitalLineModeCommandFormat( digitalLine.LineNumber ), value );
    }

    #endregion

    #region " digital line reset "

    /// <summary> Digital line reset command. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="lineNumber"> The line number. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected abstract string DigitalLineResetCommand( int lineNumber );

    /// <summary> Resets the digital line. </summary>
    /// <param name="lineNumber"> The line number. </param>
    public virtual void ResetDigitalLine( int lineNumber )
    {
        this.ResetDigitalLine( this.DigitalLines[lineNumber] );
    }

    /// <summary> Resets the digital line. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="digitalLine"> The digital line. </param>
    public virtual void ResetDigitalLine( DigitalLine digitalLine )
    {
        if ( digitalLine is null )
            throw new ArgumentNullException( nameof( digitalLine ) );
        digitalLine.ResetDigitalLine( this.Session, this.DigitalLineResetCommand( digitalLine.LineNumber ) );
    }

    #endregion

    #region " digital line state "

    /// <summary> True if digital line read write supported. </summary>
    private bool _digitalLineReadWriteSupported;

    /// <summary> Gets or sets the digital line read write Supported. </summary>
    /// <value>
    /// The digital line read write Supported. <c>true</c> if both read and write are supported and
    /// Supported; <c>false</c> otherwise.
    /// </value>
    public bool DigitalLineReadWriteSupported
    {
        get => this._digitalLineReadWriteSupported;
        protected set
        {
            if ( !Equals( this.DigitalLineReadWriteSupported, value ) )
            {
                this._digitalLineReadWriteSupported = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Digital Input Output Digital Line State. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="value">      The Digital Input Output Digital Line State. </param>
    /// <returns> An Integer? </returns>
    public int? ApplyDigitalLineState( int lineNumber, DigitalLineState value )
    {
        return this.ApplyDigitalLineState( this.DigitalLines[lineNumber], value );
    }

    /// <summary> Writes and reads back the Digital Input Output Digital Line State. </summary>
    /// <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    /// null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="digitalLine"> The digital line. </param>
    /// <param name="value">       The Digital Input Output Digital Line State. </param>
    /// <returns> An Integer? </returns>
    public int? ApplyDigitalLineState( DigitalLine digitalLine, DigitalLineState value )
    {
        if ( digitalLine is null )
            throw new ArgumentNullException( nameof( digitalLine ) );
        if ( digitalLine.DigitalLineReadWriteEnabled )
        {
            _ = this.WriteDigitalLineState( digitalLine, value );
            _ = this.QueryDigitalLineState( digitalLine );
        }
        else
        {
            throw new InvalidOperationException( $"Digital line #{digitalLine.LineNumber} read and write not enabled" );
        }

        return ( int? ) digitalLine.DigitalLineState;
    }

    /// <summary> Get the Digital Line State query command. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected abstract string DigitalLineStateQueryCommand( int lineNumber );

    /// <summary> Queries the Digital Line State. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <returns> The digital line state. </returns>
    public DigitalLineState? QueryDigitalLineState( int lineNumber )
    {
        return this.QueryDigitalLineState( this.DigitalLines[lineNumber] );
    }

    /// <summary> Queries the Digital Line State. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="digitalLine"> The digital line. </param>
    /// <returns> The digital line state. </returns>
    public virtual DigitalLineState? QueryDigitalLineState( DigitalLine digitalLine )
    {
        return digitalLine is null
            ? throw new ArgumentNullException( nameof( digitalLine ) )
            : digitalLine.QueryDigitalLineState( this.Session, this.DigitalLineStateQueryCommand( digitalLine.LineNumber ) );
    }

    /// <summary> Gets Digital Line State command format. </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected abstract string DigitalLineStateCommandFormat( int lineNumber );

    /// <summary>
    /// Writes the Digital Line State without reading back the value from the device.
    /// </summary>
    /// <param name="lineNumber"> The line number. </param>
    /// <param name="value">      The Digital Line State. </param>
    /// <returns> An Integer? </returns>
    public DigitalLineState? WriteDigitalLineState( int lineNumber, DigitalLineState value )
    {
        return this.WriteDigitalLineState( this.DigitalLines[lineNumber], value );
    }

    /// <summary>
    /// Writes the Digital Line State without reading back the value from the device.
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="digitalLine"> The digital line. </param>
    /// <param name="value">       The Digital Line State. </param>
    /// <returns> An Integer? </returns>
    public virtual DigitalLineState? WriteDigitalLineState( DigitalLine digitalLine, DigitalLineState value )
    {
        return digitalLine is null
            ? throw new ArgumentNullException( nameof( digitalLine ) )
            : digitalLine.WriteDigitalLineState( this.Session, this.DigitalLineStateCommandFormat( digitalLine.LineNumber ), value );
    }

    #endregion

    #region " read port (level) "

    /// <summary> The level. </summary>
    private int? _level;

    /// <summary> Gets or sets the level. </summary>
    /// <value> The level. </value>
    public int? Level
    {
        get => this._level;
        protected set
        {
            if ( !Nullable.Equals( this.Level, value ) )
            {
                this._level = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Applies the level described by value. </summary>
    /// <param name="value"> The Digital Input Output Digital Line State. </param>
    /// <returns> An Integer? </returns>
    public int? ApplyLevel( int value )
    {
        _ = this.WriteLevel( value );
        return this.QueryLevel();
    }

    /// <summary> Gets or sets the level query command. </summary>
    /// <value> The level query command. </value>
    protected virtual string LevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the level. </summary>
    /// <returns> The level. </returns>
    public int? QueryLevel()
    {
        this.Level = this.Session.Query( this.Level, this.LevelQueryCommand );
        return this.Level;
    }

    /// <summary> Reads the port. </summary>
    /// <returns> The port. </returns>
    public int? ReadPort()
    {
        return this.QueryLevel();
    }

    /// <summary> Gets or sets the level command format. </summary>
    /// <value> The level command format. </value>
    protected virtual string LevelCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Level without reading back the value from the device. </summary>
    /// <param name="value"> The Digital Input Output Digital Line State. </param>
    /// <returns> An Integer? </returns>
    public int? WriteLevel( int value )
    {
        this.Level = this.Session.WriteLine( value, this.LevelCommandFormat );
        return this.Level;
    }

    /// <summary> Writes a port. </summary>
    /// <param name="value"> The Digital Input Output Digital Line State. </param>
    /// <returns> An Integer? </returns>
    public int? WritePort( int value )
    {
        return this.WriteLevel( value );
    }

    #endregion

}
