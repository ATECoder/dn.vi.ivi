namespace cc.isr.VI;

/// <summary> Defines a SCPI Binning Subsystem. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2005-01-15, 1.0.1841.x. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class BinningSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="BinningSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected BinningSubsystemBase( StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.FeedSourceReadWrites = [];
        this.DefineFeedSourceReadWrites();
    }

    #endregion


    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.BinningStrobeDuration = TimeSpan.Zero;
        this.FeedSource = FeedSources.Voltage;
    }

    #endregion

    #region " commands "

    /// <summary> Gets or sets the Immediate command. </summary>
    /// <remarks> SCPI: ":CALC3:IMM". </remarks>
    /// <value> The Immediate command. </value>
    protected virtual string ImmediateCommand { get; set; } = string.Empty;

    /// <summary> Immediately calculate limits. </summary>
    public void Immediate()
    {
        if ( !string.IsNullOrWhiteSpace( this.ImmediateCommand ) )
            _ = this.Session.WriteLine( this.ImmediateCommand );
    }

    /// <summary> Gets or sets the limit 1 clear command. </summary>
    /// <value> The limit 1 clear command. </value>
    protected virtual string Limit1ClearCommand { get; set; } = string.Empty;

    /// <summary> Clear limit 1. </summary>
    public void Limit1Clear()
    {
        if ( !string.IsNullOrWhiteSpace( this.Limit1ClearCommand ) )
            _ = this.Session.WriteLine( this.Limit1ClearCommand );
    }

    /// <summary> Gets or sets the limit 2 clear command. </summary>
    /// <value> The limit 2 clear command. </value>
    protected virtual string Limit2ClearCommand { get; set; } = string.Empty;

    /// <summary> Clear limit 2. </summary>
    public void Limit2Clear()
    {
        if ( !string.IsNullOrWhiteSpace( this.Limit2ClearCommand ) )
            _ = this.Session.WriteLine( this.Limit2ClearCommand );
    }

    #endregion

    #region " digital i/o: forced digital output pattern enabled "

    /// <summary> The forced digital output pattern enabled. </summary>
    private bool? _forcedDigitalOutputPatternEnabled;

    /// <summary> Gets or sets the cached Forced Digital Output Pattern Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Forced Digital Output Pattern Enabled is not known; <c>true</c> if output is
    /// on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? ForcedDigitalOutputPatternEnabled
    {
        get => this._forcedDigitalOutputPatternEnabled;

        protected set
        {
            if ( !Equals( this.ForcedDigitalOutputPatternEnabled, value ) )
            {
                this._forcedDigitalOutputPatternEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Forced Digital Output Pattern Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyForcedDigitalOutputPatternEnabled( bool value )
    {
        _ = this.WriteForcedDigitalOutputPatternEnabled( value );
        return this.QueryForcedDigitalOutputPatternEnabled();
    }

    /// <summary> Gets or sets the automatic delay enabled query command. </summary>
    /// <remarks> SCPI: ":CALC3:FORC:STAT?". </remarks>
    /// <value> The automatic delay enabled query command. </value>
    protected virtual string ForcedDigitalOutputPatternEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Forced Digital Output Pattern Enabled sentinel. Also sets the
    /// <see cref="ForcedDigitalOutputPatternEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryForcedDigitalOutputPatternEnabled()
    {
        this.ForcedDigitalOutputPatternEnabled = this.Session.Query( this.ForcedDigitalOutputPatternEnabled, this.ForcedDigitalOutputPatternEnabledQueryCommand );
        return this.ForcedDigitalOutputPatternEnabled;
    }

    /// <summary> Gets or sets the automatic delay enabled command Format. </summary>
    /// <remarks> SCPI: ":CALC3:FORC:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic delay enabled query command. </value>
    protected virtual string ForcedDigitalOutputPatternEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Forced Digital Output Pattern Enabled sentinel. Does not read back from the
    /// instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteForcedDigitalOutputPatternEnabled( bool value )
    {
        this.ForcedDigitalOutputPatternEnabled = this.Session.WriteLine( value, this.ForcedDigitalOutputPatternEnabledCommandFormat );
        return this.ForcedDigitalOutputPatternEnabled;
    }

    #endregion

    #region " digital i/o: forced digital output pattern "

    /// <summary> A pattern specifying the forced digital output. </summary>
    private int? _forcedDigitalOutputPattern;

    /// <summary> Gets or sets the cached Forced Digital Output Pattern. </summary>
    /// <value> The Forced Digital Output Pattern or none if not set or unknown. </value>
    public int? ForcedDigitalOutputPattern
    {
        get => this._forcedDigitalOutputPattern;

        protected set
        {
            if ( !Nullable.Equals( this.ForcedDigitalOutputPattern, value ) )
            {
                this._forcedDigitalOutputPattern = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Forced Digital Output Pattern. </summary>
    /// <param name="value"> The current Forced Digital Output Pattern. </param>
    /// <returns> The Forced Digital Output Pattern or none if unknown. </returns>
    public int? ApplyForcedDigitalOutputPattern( int value )
    {
        _ = this.WriteForcedDigitalOutputPattern( value );
        return this.QueryForcedDigitalOutputPattern();
    }

    /// <summary> Gets or sets the forced digital output Pattern query command. </summary>
    /// <remarks> SCPI ":CALC3:FORC:PATT". </remarks>
    /// <value> The forced digital output Pattern query command. </value>
    protected virtual string ForcedDigitalOutputPatternQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Forced Digital Output Pattern. </summary>
    /// <returns> The Forced Digital Output Pattern or none if unknown. </returns>
    public int? QueryForcedDigitalOutputPattern()
    {
        this.ForcedDigitalOutputPattern = this.Session.Query( this.ForcedDigitalOutputPattern, this.ForcedDigitalOutputPatternQueryCommand );
        return this.ForcedDigitalOutputPattern;
    }

    /// <summary> Gets or sets the forced digital output Pattern command format. </summary>
    /// <remarks> SCPI ":CALC3:FORC:PATT". </remarks>
    /// <value> The forced digital output Pattern command format. </value>
    protected virtual string ForcedDigitalOutputPatternCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Sets back the Forced Digital Output Pattern without reading back the value from the device.
    /// </summary>
    /// <param name="value"> The current Forced Digital Output Pattern. </param>
    /// <returns> The Forced Digital Output Pattern or none if unknown. </returns>
    public int? WriteForcedDigitalOutputPattern( int value )
    {
        this.ForcedDigitalOutputPattern = this.Session.WriteLine( value, this.ForcedDigitalOutputPatternCommandFormat );
        return this.ForcedDigitalOutputPattern;
    }

    #endregion

    #region " binning strobe "

    /// <summary> Duration of the binning strobe. </summary>
    private TimeSpan _binningStrobeDuration;

    /// <summary> Gets or sets the duration of the binning strobe. </summary>
    /// <value> The binning strobe duration. </value>
    public TimeSpan BinningStrobeDuration
    {
        get => this._binningStrobeDuration;
        set
        {
            if ( value != this.BinningStrobeDuration )
            {
                this._binningStrobeDuration = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Binning Strobe Enabled status. </summary>
    private bool? _binningStrobeEnabled;

    /// <summary> Gets or sets the cached status of outputting a Binning strobe. </summary>
    /// <value>
    /// <c>null</c> if a Binning Strobe Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? BinningStrobeEnabled
    {
        get => this._binningStrobeEnabled;

        protected set
        {
            if ( !Equals( this.BinningStrobeEnabled, value ) )
            {
                this._binningStrobeEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Binning Strobe Enabled status. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyBinningStrobeEnabled( bool value )
    {
        _ = this.WriteBinningStrobeEnabled( value );
        return this.QueryBinningStrobeEnabled();
    }

    /// <summary> Gets or sets the Binning Strobe Enabled status query command. </summary>
    /// <remarks> SCPI: ":CALC3:BSTR:STAT?". </remarks>
    /// <value> The Binning enabled query command. </value>
    protected virtual string BinningStrobeEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Binning Strobe Enabled status. Also sets the
    /// <see cref="BinningStrobeEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryBinningStrobeEnabled()
    {
        this.BinningStrobeEnabled = this.Session.Query( this.BinningStrobeEnabled, this.BinningStrobeEnabledQueryCommand );
        return this.BinningStrobeEnabled;
    }

    /// <summary> Gets or sets the Binning Strobe Enabled status command Format. </summary>
    /// <remarks> SCPI: ":CALC3:BSTR:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Binning enabled query command. </value>
    protected virtual string BinningStrobeEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Binning Strobe Enabled status. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteBinningStrobeEnabled( bool value )
    {
        this.BinningStrobeEnabled = this.Session.WriteLine( value, this.BinningStrobeEnabledCommandFormat );
        return this.BinningStrobeEnabled;
    }

    #endregion

    #region " feed source "

    /// <summary> Define feed source read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineFeedSourceReadWrites()
    {
        this.FeedSourceReadWrites = new();
        foreach ( FeedSources enumValue in Enum.GetValues( typeof( FeedSources ) ) )
            this.FeedSourceReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of Feed source parses. </summary>
    /// <value> A Dictionary of Feed source parses. </value>
    public Pith.EnumReadWriteCollection FeedSourceReadWrites { get; private set; }

    /// <summary> The supported feed sources. </summary>
    private FeedSources _supportedFeedSources;

    /// <summary> Gets or sets the supported Feed sources. </summary>
    /// <value> The supported Feed sources. </value>
    public FeedSources SupportedFeedSources
    {
        get => this._supportedFeedSources;
        set
        {
            if ( !this.SupportedFeedSources.Equals( value ) )
            {
                this._supportedFeedSources = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The feed source. </summary>
    private FeedSources? _feedSource;

    /// <summary> Gets or sets the cached source FeedSource. </summary>
    /// <value>
    /// The <see cref="FeedSource">source Feed Source</see> or none if not set or unknown.
    /// </value>
    public FeedSources? FeedSource
    {
        get => this._feedSource;

        protected set
        {
            if ( !this.FeedSource.Equals( value ) )
            {
                this._feedSource = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the source Feed Source. </summary>
    /// <param name="value"> The  Source Feed Source. </param>
    /// <returns> The <see cref="FeedSource">source Feed Source</see> or none if unknown. </returns>
    public FeedSources? ApplyFeedSource( FeedSources value )
    {
        _ = this.WriteFeedSource( value );
        return this.QueryFeedSource();
    }

    /// <summary> Gets or sets the Feed source query command. </summary>
    /// <remarks> SCPI: "?". </remarks>
    /// <value> The Feed source query command. </value>
    protected virtual string FeedSourceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Feed source. </summary>
    /// <returns> The <see cref="FeedSource">Feed source</see> or none if unknown. </returns>
    public FeedSources? QueryFeedSource()
    {
        this.FeedSource = this.Session.Query( this.FeedSource.GetValueOrDefault( FeedSources.None ), this.FeedSourceReadWrites, this.FeedSourceQueryCommand );
        return this.FeedSource;
    }

    /// <summary> Gets or sets the Feed source command format. </summary>
    /// <remarks> SCPI: " {0}". </remarks>
    /// <value> The write Feed source command format. </value>
    protected virtual string FeedSourceCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Feed Source without reading back the value from the device. </summary>
    /// <param name="value"> The Feed Source. </param>
    /// <returns> The <see cref="FeedSource">Feed Source</see> or none if unknown. </returns>
    public FeedSources? WriteFeedSource( FeedSources value )
    {
        this.FeedSource = this.Session.Write( value, this.FeedSourceCommandFormat, this.FeedSourceReadWrites );
        return this.FeedSource;
    }

    #endregion

    #region " limits failed "

    /// <summary> Limits Failed. </summary>
    private bool? _limitsFailed;

    /// <summary> Gets or sets the cached Limits Failed sentinel. </summary>
    /// <value>
    /// <c>null</c> if Limits Failed is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? LimitsFailed
    {
        get => this._limitsFailed;

        protected set
        {
            if ( !Equals( this.LimitsFailed, value ) )
            {
                this._limitsFailed = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the Limits Failed query command. </summary>
    /// <remarks> SCPI: ":CALC3:LIM1:FAIL?". </remarks>
    /// <value> The Limits Failed query command. </value>
    protected virtual string LimitsFailedQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limits Failed sentinel. Also sets the
    /// <see cref="LimitsFailed">Failed</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if Failed; otherwise <c>false</c>. </returns>
    public bool? QueryLimitsFailed()
    {
        this.LimitsFailed = this.Session.Query( this.LimitsFailed, this.LimitsFailedQueryCommand );
        return this.LimitsFailed;
    }

    #endregion

    #region " pass source "

    /// <summary> The Pass Source. </summary>
    private int? _passSource;

    /// <summary> Gets or sets the cached Pass Source. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? PassSource
    {
        get => this._passSource;

        protected set
        {
            if ( !Nullable.Equals( this.PassSource, value ) )
            {
                this._passSource = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Pass Source. </summary>
    /// <param name="value"> The Pass Source. </param>
    /// <returns> The Pass Source. </returns>
    public int? ApplyPassSource( int value )
    {
        _ = this.WritePassSource( value );
        return this.QueryPassSource();
    }

    /// <summary> Gets or sets The Pass Source query command. </summary>
    /// <value> The Pass Source query command. </value>
    protected virtual string PassSourceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Pass Source. </summary>
    /// <returns> The Pass Source or none if unknown. </returns>
    public int? QueryPassSource()
    {
        this.PassSource = this.Session.Query( this.PassSource, this.PassSourceQueryCommand );
        return this.PassSource;
    }

    /// <summary> Gets or sets The Pass Source command format. </summary>
    /// <value> The Pass Source command format. </value>
    protected virtual string PassSourceCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes The Pass Source without reading back the value from the device. </summary>
    /// <remarks> This command sets The Pass Source. </remarks>
    /// <param name="value"> The Pass Source. </param>
    /// <returns> The Pass Source. </returns>
    public int? WritePassSource( int value )
    {
        this.PassSource = this.Session.WriteLine( value, this.PassSourceCommandFormat );
        return this.PassSource;
    }

    #endregion

    #region " limit 1 "

    #region " limit1 enabled "

    /// <summary> Limit1 enabled. </summary>
    private bool? _limit1Enabled;

    /// <summary> Gets or sets the cached Limit1 Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Limit1 Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit1Enabled
    {
        get => this._limit1Enabled;

        protected set
        {
            if ( !Equals( this.Limit1Enabled, value ) )
            {
                this._limit1Enabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyLimit1Enabled( bool value )
    {
        _ = this.WriteLimit1Enabled( value );
        return this.QueryLimit1Enabled();
    }

    /// <summary> Gets or sets the Limit1 enabled query command. </summary>
    /// <remarks> SCPI: ":CALC3:LIM1:STAT?". </remarks>
    /// <value> The Limit1 enabled query command. </value>
    protected virtual string Limit1EnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit1 Enabled sentinel. Also sets the
    /// <see cref="Limit1Enabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryLimit1Enabled()
    {
        this.Limit1Enabled = this.Session.Query( this.Limit1Enabled, this.Limit1EnabledQueryCommand );
        return this.Limit1Enabled;
    }

    /// <summary> Gets or sets the Limit1 enabled command Format. </summary>
    /// <remarks> SCPI: ":CALC3:LIM1:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Limit1 enabled query command. </value>
    protected virtual string Limit1EnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Limit1 Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteLimit1Enabled( bool value )
    {
        this.Limit1Enabled = this.Session.WriteLine( value, this.Limit1EnabledCommandFormat );
        return this.Limit1Enabled;
    }

    #endregion

    #region " limit1 failed "

    /// <summary> Limit1 Failed. </summary>
    private bool? _limit1Failed;

    /// <summary> Gets or sets the cached Limit1 Failed sentinel. </summary>
    /// <value>
    /// <c>null</c> if Limit1 Failed is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit1Failed
    {
        get => this._limit1Failed;

        protected set
        {
            if ( !Equals( this.Limit1Failed, value ) )
            {
                this._limit1Failed = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the Limit1 Failed query command. </summary>
    /// <remarks> SCPI: ":CALC3:LIM1:FAIL?". </remarks>
    /// <value> The Limit1 Failed query command. </value>
    protected virtual string Limit1FailedQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit1 Failed sentinel. Also sets the
    /// <see cref="Limit1Failed">Failed</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if Failed; otherwise <c>false</c>. </returns>
    public bool? QueryLimit1Failed()
    {
        this.Limit1Failed = this.Session.Query( this.Limit1Failed, this.Limit1FailedQueryCommand );
        return this.Limit1Failed;
    }

    #endregion

    #region " limit1 lower level "

    /// <summary> The Limit1 Lower Level. </summary>
    private double? _limit1LowerLevel;

    /// <summary>
    /// Gets or sets the cached Limit1 Lower Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Limit1LowerLevel
    {
        get => this._limit1LowerLevel;

        protected set
        {
            if ( !Nullable.Equals( this.Limit1LowerLevel, value ) )
            {
                this._limit1LowerLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Lower Level. </summary>
    /// <param name="value"> The Limit1 Lower Level. </param>
    /// <returns> The Limit1 Lower Level. </returns>
    public double? ApplyLimit1LowerLevel( double value )
    {
        _ = this.WriteLimit1LowerLevel( value );
        return this.QueryLimit1LowerLevel();
    }

    /// <summary> Gets or sets The Limit1 Lower Level query command. </summary>
    /// <value> The Limit1 Lower Level query command. </value>
    protected virtual string Limit1LowerLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit1 Lower Level. </summary>
    /// <returns> The Limit1 Lower Level or none if unknown. </returns>
    public double? QueryLimit1LowerLevel()
    {
        this.Limit1LowerLevel = this.Session.Query( this.Limit1LowerLevel, this.Limit1LowerLevelQueryCommand );
        return this.Limit1LowerLevel;
    }

    /// <summary> Gets or sets The Limit1 Lower Level command format. </summary>
    /// <value> The Limit1 Lower Level command format. </value>
    protected virtual string Limit1LowerLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit1 Lower Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit1 Lower Level. </remarks>
    /// <param name="value"> The Limit1 Lower Level. </param>
    /// <returns> The Limit1 Lower Level. </returns>
    public double? WriteLimit1LowerLevel( double value )
    {
        this.Limit1LowerLevel = this.Session.WriteLine( value, this.Limit1LowerLevelCommandFormat );
        return this.Limit1LowerLevel;
    }

    #endregion

    #region " limit1 upper level "

    /// <summary> The Limit1 Upper Level. </summary>
    private double? _limit1UpperLevel;

    /// <summary>
    /// Gets or sets the cached Limit1 Upper Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Limit1UpperLevel
    {
        get => this._limit1UpperLevel;

        protected set
        {
            if ( !Nullable.Equals( this.Limit1UpperLevel, value ) )
            {
                this._limit1UpperLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Upper Level. </summary>
    /// <param name="value"> The Limit1 Upper Level. </param>
    /// <returns> The Limit1 Upper Level. </returns>
    public double? ApplyLimit1UpperLevel( double value )
    {
        _ = this.WriteLimit1UpperLevel( value );
        return this.QueryLimit1UpperLevel();
    }

    /// <summary> Gets or sets The Limit1 Upper Level query command. </summary>
    /// <value> The Limit1 Upper Level query command. </value>
    protected virtual string Limit1UpperLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit1 Upper Level. </summary>
    /// <returns> The Limit1 Upper Level or none if unknown. </returns>
    public double? QueryLimit1UpperLevel()
    {
        this.Limit1UpperLevel = this.Session.Query( this.Limit1UpperLevel, this.Limit1UpperLevelQueryCommand );
        return this.Limit1UpperLevel;
    }

    /// <summary> Gets or sets The Limit1 Upper Level command format. </summary>
    /// <value> The Limit1 Upper Level command format. </value>
    protected virtual string Limit1UpperLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit1 Upper Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit1 Upper Level. </remarks>
    /// <param name="value"> The Limit1 Upper Level. </param>
    /// <returns> The Limit1 Upper Level. </returns>
    public double? WriteLimit1UpperLevel( double value )
    {
        this.Limit1UpperLevel = this.Session.WriteLine( value, this.Limit1UpperLevelCommandFormat );
        return this.Limit1UpperLevel;
    }

    #endregion

    #region " limit1 auto clear "

    /// <summary> Limit1 Auto Clear. </summary>
    private bool? _limit1AutoClear;

    /// <summary> Gets or sets the cached Limit1 Auto Clear sentinel. </summary>
    /// <value>
    /// <c>null</c> if Limit1 Auto Clear is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit1AutoClear
    {
        get => this._limit1AutoClear;

        protected set
        {
            if ( !Equals( this.Limit1AutoClear, value ) )
            {
                this._limit1AutoClear = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Auto Clear sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? ApplyLimit1AutoClear( bool value )
    {
        _ = this.WriteLimit1AutoClear( value );
        return this.QueryLimit1AutoClear();
    }

    /// <summary> Gets or sets the Limit1 Auto Clear query command. </summary>
    /// <remarks> SCPI: ":CALC3:LIM1:STAT?". </remarks>
    /// <value> The Limit1 Auto Clear query command. </value>
    protected virtual string Limit1AutoClearQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit1 Auto Clear sentinel. Also sets the
    /// <see cref="Limit1AutoClear">AutoClear</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? QueryLimit1AutoClear()
    {
        this.Limit1AutoClear = this.Session.Query( this.Limit1AutoClear, this.Limit1AutoClearQueryCommand );
        return this.Limit1AutoClear;
    }

    /// <summary> Gets or sets the Limit1 Auto Clear command Format. </summary>
    /// <remarks> SCPI: ":CALC3:LIM1:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Limit1 Auto Clear query command. </value>
    protected virtual string Limit1AutoClearCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Limit1 Auto Clear sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is Auto Clear. </param>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? WriteLimit1AutoClear( bool value )
    {
        this.Limit1AutoClear = this.Session.WriteLine( value, this.Limit1AutoClearCommandFormat );
        return this.Limit1AutoClear;
    }

    #endregion

    #region " limit1 lower source "

    /// <summary> The Limit1 Lower Source. </summary>
    private int? _limit1LowerSource;

    /// <summary> Gets or sets the cached Limit1 Lower Source. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? Limit1LowerSource
    {
        get => this._limit1LowerSource;

        protected set
        {
            if ( !Nullable.Equals( this.Limit1LowerSource, value ) )
            {
                this._limit1LowerSource = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Lower Source. </summary>
    /// <param name="value"> The Limit1 Lower Source. </param>
    /// <returns> The Limit1 Lower Source. </returns>
    public int? ApplyLimit1LowerSource( int value )
    {
        _ = this.WriteLimit1LowerSource( value );
        return this.QueryLimit1LowerSource();
    }

    /// <summary> Gets or sets The Limit1 Lower Source query command. </summary>
    /// <value> The Limit1 Lower Source query command. </value>
    protected virtual string Limit1LowerSourceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit1 Lower Source. </summary>
    /// <returns> The Limit1 Lower Source or none if unknown. </returns>
    public int? QueryLimit1LowerSource()
    {
        this.Limit1LowerSource = this.Session.Query( this.Limit1LowerSource, this.Limit1LowerSourceQueryCommand );
        return this.Limit1LowerSource;
    }

    /// <summary> Gets or sets The Limit1 Lower Source command format. </summary>
    /// <value> The Limit1 Lower Source command format. </value>
    protected virtual string Limit1LowerSourceCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit1 Lower Source without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit1 Lower Source. </remarks>
    /// <param name="value"> The Limit1 Lower Source. </param>
    /// <returns> The Limit1 Lower Source. </returns>
    public int? WriteLimit1LowerSource( int value )
    {
        this.Limit1LowerSource = this.Session.WriteLine( value, this.Limit1LowerSourceCommandFormat );
        return this.Limit1LowerSource;
    }

    #endregion

    #region " limit1 upper source "

    /// <summary> The Limit1 Upper Source. </summary>
    private int? _limit1UpperSource;

    /// <summary> Gets or sets the cached Limit1 Upper Source. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? Limit1UpperSource
    {
        get => this._limit1UpperSource;

        protected set
        {
            if ( !Nullable.Equals( this.Limit1UpperSource, value ) )
            {
                this._limit1UpperSource = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit1 Upper Source. </summary>
    /// <param name="value"> The Limit1 Upper Source. </param>
    /// <returns> The Limit1 Upper Source. </returns>
    public int? ApplyLimit1UpperSource( int value )
    {
        _ = this.WriteLimit1UpperSource( value );
        return this.QueryLimit1UpperSource();
    }

    /// <summary> Gets or sets The Limit1 Upper Source query command. </summary>
    /// <value> The Limit1 Upper Source query command. </value>
    protected virtual string Limit1UpperSourceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit1 Upper Source. </summary>
    /// <returns> The Limit1 Upper Source or none if unknown. </returns>
    public int? QueryLimit1UpperSource()
    {
        this.Limit1UpperSource = this.Session.Query( this.Limit1UpperSource, this.Limit1UpperSourceQueryCommand );
        return this.Limit1UpperSource;
    }

    /// <summary> Gets or sets The Limit1 Upper Source command format. </summary>
    /// <value> The Limit1 Upper Source command format. </value>
    protected virtual string Limit1UpperSourceCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit1 Upper Source without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit1 Upper Source. </remarks>
    /// <param name="value"> The Limit1 Upper Source. </param>
    /// <returns> The Limit1 Upper Source. </returns>
    public int? WriteLimit1UpperSource( int value )
    {
        this.Limit1UpperSource = this.Session.WriteLine( value, this.Limit1UpperSourceCommandFormat );
        return this.Limit1UpperSource;
    }

    #endregion

    #endregion

    #region " limit 2 "

    #region " limit2 enabled "

    /// <summary> Limit2 enabled. </summary>
    private bool? _limit2Enabled;

    /// <summary> Gets or sets the cached Limit2 Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Limit2 Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit2Enabled
    {
        get => this._limit2Enabled;

        protected set
        {
            if ( !Equals( this.Limit2Enabled, value ) )
            {
                this._limit2Enabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyLimit2Enabled( bool value )
    {
        _ = this.WriteLimit2Enabled( value );
        return this.QueryLimit2Enabled();
    }

    /// <summary> Gets or sets the Limit2 enabled query command. </summary>
    /// <remarks> SCPI: ":CALC3:LIM2:STAT?". </remarks>
    /// <value> The Limit2 enabled query command. </value>
    protected virtual string Limit2EnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit2 Enabled sentinel. Also sets the
    /// <see cref="Limit2Enabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryLimit2Enabled()
    {
        this.Limit2Enabled = this.Session.Query( this.Limit2Enabled, this.Limit2EnabledQueryCommand );
        return this.Limit2Enabled;
    }

    /// <summary> Gets or sets the Limit2 enabled command Format. </summary>
    /// <remarks> SCPI: ":CALC3:LIM2:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Limit2 enabled query command. </value>
    protected virtual string Limit2EnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Limit2 Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteLimit2Enabled( bool value )
    {
        this.Limit2Enabled = this.Session.WriteLine( value, this.Limit2EnabledCommandFormat );
        return this.Limit2Enabled;
    }

    #endregion

    #region " limit2 failed "

    /// <summary> Limit2 Failed. </summary>
    private bool? _limit2Failed;

    /// <summary> Gets or sets the cached Limit2 Failed sentinel. </summary>
    /// <value>
    /// <c>null</c> if Limit2 Failed is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit2Failed
    {
        get => this._limit2Failed;

        protected set
        {
            if ( !Equals( this.Limit2Failed, value ) )
            {
                this._limit2Failed = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the Limit2 Failed query command. </summary>
    /// <remarks> SCPI: ":CALC3:LIM2:FAIL?". </remarks>
    /// <value> The Limit2 Failed query command. </value>
    protected virtual string Limit2FailedQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit2 Failed sentinel. Also sets the
    /// <see cref="Limit2Failed">Failed</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if Failed; otherwise <c>false</c>. </returns>
    public bool? QueryLimit2Failed()
    {
        this.Limit2Failed = this.Session.Query( this.Limit2Failed, this.Limit2FailedQueryCommand );
        return this.Limit2Failed;
    }

    #endregion

    #region " limit2 lower level "

    /// <summary> The Limit2 Lower Level. </summary>
    private double? _limit2LowerLevel;

    /// <summary>
    /// Gets or sets the cached Limit2 Lower Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Limit2LowerLevel
    {
        get => this._limit2LowerLevel;

        protected set
        {
            if ( !Nullable.Equals( this.Limit2LowerLevel, value ) )
            {
                this._limit2LowerLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Lower Level. </summary>
    /// <param name="value"> The Limit2 Lower Level. </param>
    /// <returns> The Limit2 Lower Level. </returns>
    public double? ApplyLimit2LowerLevel( double value )
    {
        _ = this.WriteLimit2LowerLevel( value );
        return this.QueryLimit2LowerLevel();
    }

    /// <summary> Gets or sets The Limit2 Lower Level query command. </summary>
    /// <value> The Limit2 Lower Level query command. </value>
    protected virtual string Limit2LowerLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit2 Lower Level. </summary>
    /// <returns> The Limit2 Lower Level or none if unknown. </returns>
    public double? QueryLimit2LowerLevel()
    {
        this.Limit2LowerLevel = this.Session.Query( this.Limit2LowerLevel, this.Limit2LowerLevelQueryCommand );
        return this.Limit2LowerLevel;
    }

    /// <summary> Gets or sets The Limit2 Lower Level command format. </summary>
    /// <value> The Limit2 Lower Level command format. </value>
    protected virtual string Limit2LowerLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit2 Lower Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit2 Lower Level. </remarks>
    /// <param name="value"> The Limit2 Lower Level. </param>
    /// <returns> The Limit2 Lower Level. </returns>
    public double? WriteLimit2LowerLevel( double value )
    {
        this.Limit2LowerLevel = this.Session.WriteLine( value, this.Limit2LowerLevelCommandFormat );
        return this.Limit2LowerLevel;
    }

    #endregion

    #region " limit2 upper level "

    /// <summary> The Limit2 Upper Level. </summary>
    private double? _limit2UpperLevel;

    /// <summary>
    /// Gets or sets the cached Limit2 Upper Level. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Limit2UpperLevel
    {
        get => this._limit2UpperLevel;

        protected set
        {
            if ( !Nullable.Equals( this.Limit2UpperLevel, value ) )
            {
                this._limit2UpperLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Upper Level. </summary>
    /// <param name="value"> The Limit2 Upper Level. </param>
    /// <returns> The Limit2 Upper Level. </returns>
    public double? ApplyLimit2UpperLevel( double value )
    {
        _ = this.WriteLimit2UpperLevel( value );
        return this.QueryLimit2UpperLevel();
    }

    /// <summary> Gets or sets The Limit2 Upper Level query command. </summary>
    /// <value> The Limit2 Upper Level query command. </value>
    protected virtual string Limit2UpperLevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit2 Upper Level. </summary>
    /// <returns> The Limit2 Upper Level or none if unknown. </returns>
    public double? QueryLimit2UpperLevel()
    {
        this.Limit2UpperLevel = this.Session.Query( this.Limit2UpperLevel, this.Limit2UpperLevelQueryCommand );
        return this.Limit2UpperLevel;
    }

    /// <summary> Gets or sets The Limit2 Upper Level command format. </summary>
    /// <value> The Limit2 Upper Level command format. </value>
    protected virtual string Limit2UpperLevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit2 Upper Level without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit2 Upper Level. </remarks>
    /// <param name="value"> The Limit2 Upper Level. </param>
    /// <returns> The Limit2 Upper Level. </returns>
    public double? WriteLimit2UpperLevel( double value )
    {
        this.Limit2UpperLevel = this.Session.WriteLine( value, this.Limit2UpperLevelCommandFormat );
        return this.Limit2UpperLevel;
    }

    #endregion

    #region " limit2 auto clear "

    /// <summary> Limit2 Auto Clear. </summary>
    private bool? _limit2AutoClear;

    /// <summary> Gets or sets the cached Limit2 Auto Clear sentinel. </summary>
    /// <value>
    /// <c>null</c> if Limit2 Auto Clear is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? Limit2AutoClear
    {
        get => this._limit2AutoClear;

        protected set
        {
            if ( !Equals( this.Limit2AutoClear, value ) )
            {
                this._limit2AutoClear = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Auto Clear sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? ApplyLimit2AutoClear( bool value )
    {
        _ = this.WriteLimit2AutoClear( value );
        return this.QueryLimit2AutoClear();
    }

    /// <summary> Gets or sets the Limit2 Auto Clear query command. </summary>
    /// <remarks> SCPI: ":CALC3:LIM2:STAT?". </remarks>
    /// <value> The Limit2 Auto Clear query command. </value>
    protected virtual string Limit2AutoClearQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit2 Auto Clear sentinel. Also sets the
    /// <see cref="Limit2AutoClear">AutoClear</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? QueryLimit2AutoClear()
    {
        this.Limit2AutoClear = this.Session.Query( this.Limit2AutoClear, this.Limit2AutoClearQueryCommand );
        return this.Limit2AutoClear;
    }

    /// <summary> Gets or sets the Limit2 Auto Clear command Format. </summary>
    /// <remarks> SCPI: ":CALC3:LIM2:STAT {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Limit2 Auto Clear query command. </value>
    protected virtual string Limit2AutoClearCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Limit2 Auto Clear sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is Auto Clear. </param>
    /// <returns> <c>true</c> if AutoClear; otherwise <c>false</c>. </returns>
    public bool? WriteLimit2AutoClear( bool value )
    {
        this.Limit2AutoClear = this.Session.WriteLine( value, this.Limit2AutoClearCommandFormat );
        return this.Limit2AutoClear;
    }

    #endregion

    #region " limit2 lower source "

    /// <summary> The Limit2 Lower Source. </summary>
    private int? _limit2LowerSource;

    /// <summary> Gets or sets the cached Limit2 Lower Source. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? Limit2LowerSource
    {
        get => this._limit2LowerSource;

        protected set
        {
            if ( !Nullable.Equals( this.Limit2LowerSource, value ) )
            {
                this._limit2LowerSource = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Lower Source. </summary>
    /// <param name="value"> The Limit2 Lower Source. </param>
    /// <returns> The Limit2 Lower Source. </returns>
    public int? ApplyLimit2LowerSource( int value )
    {
        _ = this.WriteLimit2LowerSource( value );
        return this.QueryLimit2LowerSource();
    }

    /// <summary> Gets or sets The Limit2 Lower Source query command. </summary>
    /// <value> The Limit2 Lower Source query command. </value>
    protected virtual string Limit2LowerSourceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit2 Lower Source. </summary>
    /// <returns> The Limit2 Lower Source or none if unknown. </returns>
    public int? QueryLimit2LowerSource()
    {
        this.Limit2LowerSource = this.Session.Query( this.Limit2LowerSource, this.Limit2LowerSourceQueryCommand );
        return this.Limit2LowerSource;
    }

    /// <summary> Gets or sets The Limit2 Lower Source command format. </summary>
    /// <value> The Limit2 Lower Source command format. </value>
    protected virtual string Limit2LowerSourceCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit2 Lower Source without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit2 Lower Source. </remarks>
    /// <param name="value"> The Limit2 Lower Source. </param>
    /// <returns> The Limit2 Lower Source. </returns>
    public int? WriteLimit2LowerSource( int value )
    {
        this.Limit2LowerSource = this.Session.WriteLine( value, this.Limit2LowerSourceCommandFormat );
        return this.Limit2LowerSource;
    }

    #endregion

    #region " limit2 upper source "

    /// <summary> The Limit2 Upper Source. </summary>
    private int? _limit2UpperSource;

    /// <summary> Gets or sets the cached Limit2 Upper Source. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public int? Limit2UpperSource
    {
        get => this._limit2UpperSource;

        protected set
        {
            if ( !Nullable.Equals( this.Limit2UpperSource, value ) )
            {
                this._limit2UpperSource = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Limit2 Upper Source. </summary>
    /// <param name="value"> The Limit2 Upper Source. </param>
    /// <returns> The Limit2 Upper Source. </returns>
    public int? ApplyLimit2UpperSource( int value )
    {
        _ = this.WriteLimit2UpperSource( value );
        return this.QueryLimit2UpperSource();
    }

    /// <summary> Gets or sets The Limit2 Upper Source query command. </summary>
    /// <value> The Limit2 Upper Source query command. </value>
    protected virtual string Limit2UpperSourceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Limit2 Upper Source. </summary>
    /// <returns> The Limit2 Upper Source or none if unknown. </returns>
    public int? QueryLimit2UpperSource()
    {
        this.Limit2UpperSource = this.Session.Query( this.Limit2UpperSource, this.Limit2UpperSourceQueryCommand );
        return this.Limit2UpperSource;
    }

    /// <summary> Gets or sets The Limit2 Upper Source command format. </summary>
    /// <value> The Limit2 Upper Source command format. </value>
    protected virtual string Limit2UpperSourceCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes The Limit2 Upper Source without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets The Limit2 Upper Source. </remarks>
    /// <param name="value"> The Limit2 Upper Source. </param>
    /// <returns> The Limit2 Upper Source. </returns>
    public int? WriteLimit2UpperSource( int value )
    {
        this.Limit2UpperSource = this.Session.WriteLine( value, this.Limit2UpperSourceCommandFormat );
        return this.Limit2UpperSource;
    }

    #endregion

    #endregion
}
