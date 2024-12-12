using System.ComponentModel;
using cc.isr.VI.Pith;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by System Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SystemSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> The status subsystem. </param>
[CLSCompliant( false )]
public abstract class SystemSubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " auto zero enabled "

    /// <summary> The automatic zero enabled. </summary>
    private bool? _autoZeroEnabled;

    /// <summary> Gets or sets the cached Auto Zero enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Zero enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoZeroEnabled
    {
        get => this._autoZeroEnabled;

        protected set
        {
            if ( !Equals( this.AutoZeroEnabled, value ) )
            {
                this._autoZeroEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Auto Zero enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if Enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoZeroEnabled( bool value )
    {
        _ = this.WriteAutoZeroEnabled( value );
        return this.QueryAutoZeroEnabled();
    }

    /// <summary> Gets or sets the Auto Zero enabled query command. </summary>
    /// <remarks> SCPI: ":SYST:AZERO?". </remarks>
    /// <value> The Auto Zero enabled query command. </value>
    protected virtual string AutoZeroEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Zero Enabled sentinel. Also sets the
    /// <see cref="AutoZeroEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoZeroEnabled()
    {
        this.AutoZeroEnabled = this.Session.Query( this.AutoZeroEnabled, this.AutoZeroEnabledQueryCommand );
        return this.AutoZeroEnabled;
    }

    /// <summary> Gets or sets the Auto Zero enabled command Format. </summary>
    /// <remarks> SCPI: ":SYST:AZERO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Auto Zero enabled query command. </value>
    protected virtual string AutoZeroEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Zero Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoZeroEnabled( bool value )
    {
        this.AutoZeroEnabled = this.Session.WriteLine( value, this.AutoZeroEnabledCommandFormat );
        return this.AutoZeroEnabled;
    }

    #endregion

    #region " beeper enabled + immediate "

    /// <summary> The beeper enabled. </summary>
    private bool? _beeperEnabled;

    /// <summary> Gets or sets the cached Beeper enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Beeper enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? BeeperEnabled
    {
        get => this._beeperEnabled;

        protected set
        {
            if ( !Equals( this.BeeperEnabled, value ) )
            {
                this._beeperEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Beeper enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if Enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyBeeperEnabled( bool value )
    {
        _ = this.WriteBeeperEnabled( value );
        return this.QueryBeeperEnabled();
    }

    /// <summary> Gets or sets the Beeper enabled query command. </summary>
    /// <remarks> SCPI: ":SYST:BEEP:STAT?". </remarks>
    /// <value> The Beeper enabled query command. </value>
    protected virtual string BeeperEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Beeper Enabled sentinel. Also sets the
    /// <see cref="BeeperEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryBeeperEnabled()
    {
        this.BeeperEnabled = this.Session.Query( this.BeeperEnabled, this.BeeperEnabledQueryCommand );
        return this.BeeperEnabled;
    }

    /// <summary> Gets or sets the Beeper enabled command Format. </summary>
    /// <remarks> SCPI: ":SYST:BEEP:STAT {0:'1';'1';'0'}". </remarks>
    /// <value> The Beeper enabled query command. </value>
    protected virtual string BeeperEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Beeper Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteBeeperEnabled( bool value )
    {
        this.BeeperEnabled = this.Session.WriteLine( value, this.BeeperEnabledCommandFormat );
        return this.BeeperEnabled;
    }

    /// <summary> Gets or sets the beeper immediate command format. </summary>
    /// <remarks> SCPI: ":SYST:BEEP:IMM {0}, {1}". </remarks>
    /// <value> The beeper immediate command format. </value>
    protected virtual string BeeperImmediateCommandFormat { get; set; } = string.Empty;

    /// <summary> Commands the instrument to issue a Beep on the instrument. </summary>
    /// <param name="frequency"> Specifies the frequency of the beep. </param>
    /// <param name="duration">  Specifies the duration of the beep. </param>
    public void BeepImmediately( int frequency, float duration )
    {
        if ( !string.IsNullOrWhiteSpace( this.BeeperImmediateCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.BeeperEnabledCommandFormat, frequency, duration );
        }
    }

    #endregion

    #region " contact check enabled "

    /// <summary> The contact check enabled. </summary>
    private bool? _contactCheckEnabled;

    /// <summary> Gets or sets the cached Contact Check enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Contact Check enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? ContactCheckEnabled
    {
        get => this._contactCheckEnabled;

        protected set
        {
            if ( !Equals( this.ContactCheckEnabled, value ) )
            {
                this._contactCheckEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Contact Check enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if Enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyContactCheckEnabled( bool value )
    {
        _ = this.WriteContactCheckEnabled( value );
        return this.QueryContactCheckEnabled();
    }

    /// <summary> Gets or sets the Contact Check enabled query command. </summary>
    /// <remarks> SCPI: ":SYST:CCH?". </remarks>
    /// <value> The Contact Check enabled query command. </value>
    protected virtual string ContactCheckEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the contact check Enabled sentinel. Also sets the
    /// <see cref="ContactCheckEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryContactCheckEnabled()
    {
        this.ContactCheckEnabled = this.Session.Query( this.ContactCheckEnabled, this.ContactCheckEnabledQueryCommand );
        return this.ContactCheckEnabled;
    }

    /// <summary> Gets or sets the Contact Check enabled command Format. </summary>
    /// <remarks> SCPI: ":SYST:CCH {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Contact Check enabled query command. </value>
    protected virtual string ContactCheckEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the contact check Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteContactCheckEnabled( bool value )
    {
        this.ContactCheckEnabled = this.Session.WriteLine( value, this.ContactCheckEnabledCommandFormat );
        return this.ContactCheckEnabled;
    }

    #endregion

    #region " contact check resistance "

    /// <summary> The Contact Check Resistance. </summary>
    private double? _contactCheckResistance;

    /// <summary> Gets or sets the cached source Contact Check Resistance. </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? ContactCheckResistance
    {
        get => this._contactCheckResistance;

        protected set
        {
            if ( !Nullable.Equals( this.ContactCheckResistance, value ) )
            {
                this._contactCheckResistance = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the ContactCheck Resistance. </summary>
    /// <param name="value"> the ContactCheck Resistance. </param>
    /// <returns> the ContactCheck Resistance. </returns>
    public double? ApplyContactCheckResistance( double value )
    {
        _ = this.WriteContactCheckResistance( value );
        return this.QueryContactCheckResistance();
    }

    /// <summary> Gets or sets the ContactCheck Resistance query command. </summary>
    /// <remarks> scpi: "SYST:CCH:RES?". </remarks>
    /// <value> the ContactCheck Resistance query command. </value>
    protected virtual string ContactCheckResistanceQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the ContactCheck Resistance. </summary>
    /// <returns> the ContactCheck Resistance or none if unknown. </returns>
    public double? QueryContactCheckResistance()
    {
        this.ContactCheckResistance = this.Session.Query( this.ContactCheckResistance, this.ContactCheckResistanceQueryCommand );
        return this.ContactCheckResistance;
    }

    /// <summary> Gets or sets the ContactCheck Resistance command format. </summary>
    /// <remarks> scpi: "SYST:CCH:RES {0}". </remarks>
    /// <value> the ContactCheck Resistance command format. </value>
    protected virtual string ContactCheckResistanceCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the ContactCheck Resistance without reading back the value from the device.
    /// </summary>
    /// <remarks> This command sets the ContactCheck Resistance. </remarks>
    /// <param name="value"> the ContactCheck Resistance. </param>
    /// <returns> the ContactCheck Resistance. </returns>
    public double? WriteContactCheckResistance( double value )
    {
        this.ContactCheckResistance = this.Session.WriteLine( value, this.ContactCheckResistanceCommandFormat );
        return this.ContactCheckResistance;
    }

    #endregion

    #region " contact check supported "

    /// <summary> The contact check supported. </summary>
    private bool? _contactCheckSupported;

    /// <summary> Gets or sets the contact check supported. </summary>
    /// <value> The contact check supported. </value>
    public bool? ContactCheckSupported
    {
        get => this._contactCheckSupported;

        protected set
        {
            if ( !Equals( this.ContactCheckSupported, value ) )
            {
                this._contactCheckSupported = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the contact check option value. </summary>
    /// <value> The contact check option value. </value>
    protected virtual string ContactCheckOptionValue { get; set; } = "CONTACT-CHECK";

    /// <summary> Determines if contact check option is supported. </summary>
    /// <param name="options"> The options. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public virtual bool IsContactCheckSupported( string options )
    {
        return !string.IsNullOrWhiteSpace( options ) && options.Contains( this.ContactCheckOptionValue );
    }

    /// <summary> Queries support for contact check. </summary>
    /// <returns> <c>true</c> if supports contact check; otherwise <c>false</c>. </returns>
    public virtual bool? QueryContactCheckSupported()
    {
        _ = this.QueryOptions();
        return this.ContactCheckSupported;
    }

    #endregion

    #region " fan level "

    /// <summary> The Fan Level. </summary>
    private FanLevel? _fanLevel;

    /// <summary> Gets or sets the cached Fan Level. </summary>
    /// <value> The Fan Level or null if unknown. </value>
    public FanLevel? FanLevel
    {
        get => this._fanLevel;

        protected set
        {
            if ( !Nullable.Equals( this.FanLevel, value ) )
            {
                this._fanLevel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Converts the specified value to string. </summary>
    /// <param name="value"> The <see cref="FanLevel">Fan Level</see>. </param>
    /// <returns> A <see cref="string" />. </returns>
    protected virtual string FromFanLevel( FanLevel value )
    {
        return value == VI.FanLevel.Normal ? "NORM" : "QUIET";
    }

    /// <summary> Converts a value to a fan level. </summary>
    /// <param name="value"> The <see cref="FanLevel">Route Fan Level</see>. </param>
    /// <returns> Value as a FanLevel. </returns>
    private FanLevel ToFanLevel( string value )
    {
        return value.StartsWith( this.FromFanLevel( VI.FanLevel.Quiet ), StringComparison.OrdinalIgnoreCase ) ? VI.FanLevel.Quiet : VI.FanLevel.Normal;
    }

    /// <summary> Writes and reads back the Fan Level. </summary>
    /// <param name="value"> The <see cref="FanLevel">Fan Level</see>. </param>
    /// <returns> The Fan Level or null if unknown. </returns>
    public FanLevel? ApplyFanLevel( FanLevel value )
    {
        _ = this.WriteFanLevel( value );
        return this.QueryFanLevel();
    }

    /// <summary> Gets or sets the Fan Level query command. </summary>
    /// <value> The Fan Level command. </value>
    protected virtual string FanLevelQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Fan Level. Also sets the <see cref="FanLevel">output on</see> sentinel.
    /// </summary>
    /// <returns> The Fan Level or null if unknown. </returns>
    public FanLevel? QueryFanLevel()
    {
        string? result = this.Session.QueryTrimEnd( this.FromFanLevel( this.FanLevel.GetValueOrDefault( VI.FanLevel.Normal ) ), this.FanLevelQueryCommand );
        return result is null
            ? this.FanLevel
            : this.ToFanLevel( result! );
    }

    /// <summary> Gets or sets the Fan Level command format. </summary>
    /// <value> The Fan Level command format. </value>
    protected virtual string FanLevelCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Fan Level. Does not read back from the instrument. </summary>
    /// <param name="value"> The Fan Level. </param>
    /// <returns> The Fan Level or null if unknown. </returns>
    public FanLevel? WriteFanLevel( FanLevel value )
    {
        _ = this.Session.WriteLine( this.FanLevelCommandFormat, this.FromFanLevel( value ) );
        this.FanLevel = value;
        return this.FanLevel;
    }

    #endregion

    #region " four wire sense enabled "

    /// <summary> The four wire sense enabled. </summary>
    private bool? _fourWireSenseEnabled;

    /// <summary> Gets or sets the cached Four Wire Sense enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Four Wire Sense enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? FourWireSenseEnabled
    {
        get => this._fourWireSenseEnabled;

        protected set
        {
            if ( !Equals( this.FourWireSenseEnabled, value ) )
            {
                this._fourWireSenseEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Four Wire Sense enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if Enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyFourWireSenseEnabled( bool value )
    {
        _ = this.WriteFourWireSenseEnabled( value );
        return this.QueryFourWireSenseEnabled();
    }

    /// <summary> Gets the Four Wire Sense enabled query command. </summary>
    /// <remarks> SCPI: ":SYST:RSEN?". </remarks>
    /// <value> The Four Wire Sense enabled query command. </value>
    protected virtual string FourWireSenseEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Four Wire Sense Enabled sentinel. Also sets the
    /// <see cref="FourWireSenseEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryFourWireSenseEnabled()
    {
        this.FourWireSenseEnabled = this.Session.Query( this.FourWireSenseEnabled, this.FourWireSenseEnabledQueryCommand );
        return this.FourWireSenseEnabled;
    }

    /// <summary> Gets the Four Wire Sense enabled command Format. </summary>
    /// <remarks> SCPI: ":SYST:RSEN {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Four Wire Sense enabled query command. </value>
    protected virtual string FourWireSenseEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Four Wire Sense Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteFourWireSenseEnabled( bool value )
    {
        this.FourWireSenseEnabled = this.Session.WriteLine( value, this.FourWireSenseEnabledCommandFormat );
        return this.FourWireSenseEnabled;
    }

    #endregion

    #region " front terminals selected "

    /// <summary> Gets the front terminal label. </summary>
    /// <value> The front terminal label. </value>
    public string FrontTerminalLabel { get; set; } = "F";

    /// <summary> Gets the rear terminal label. </summary>
    /// <value> The rear terminal label. </value>
    public string RearTerminalLabel { get; set; } = "R";

    /// <summary> Gets the unknown terminal label. </summary>
    /// <value> The unknown terminal label. </value>
    public string UnknownTerminalLabel { get; set; } = string.Empty;

    /// <summary> Gets the terminals caption. </summary>
    /// <value> The terminals caption. </value>
    public string TerminalsCaption => this.FrontTerminalsSelected.HasValue ? this.FrontTerminalsSelected.Value ? this.FrontTerminalLabel : this.RearTerminalLabel : this.UnknownTerminalLabel;

    /// <summary> Gets true if the subsystem supports front terminals selection query. </summary>
    /// <value>
    /// The value indicating if the subsystem supports front terminals selection query.
    /// </value>
    public bool SupportsFrontTerminalsSelectionQuery => !string.IsNullOrWhiteSpace( this.FrontTerminalsSelectedQueryCommand );

    /// <summary> The front terminals selected. </summary>
    private bool? _frontTerminalsSelected;

    /// <summary> Gets or sets the cached Front Terminals Selected sentinel. </summary>
    /// <value>
    /// <c>null</c> if Front Terminals Selected is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? FrontTerminalsSelected
    {
        get => this._frontTerminalsSelected;

        protected set
        {
            if ( !Equals( this.FrontTerminalsSelected, value ) )
            {
                this._frontTerminalsSelected = value;
                this.NotifyPropertyChanged();
                this.NotifyPropertyChanged( nameof( MultimeterSubsystemBase.TerminalsCaption ) );
            }
        }
    }

    /// <summary> Writes and reads back the Front Terminals Selected sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if Selected; otherwise <c>false</c>. </returns>
    public bool? ApplyFrontTerminalsSelected( bool value )
    {
        _ = this.WriteFrontTerminalsSelected( value );
        return this.QueryFrontTerminalsSelected();
    }

    /// <summary> Gets or sets the Front Terminals Selected query command. </summary>
    /// <remarks> SCPI: ":SYST:FRSW?". </remarks>
    /// <value> The Front Terminals Selected query command. </value>
    protected virtual string FrontTerminalsSelectedQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Front Terminals Selected sentinel. Also sets the
    /// <see cref="FrontTerminalsSelected">Selected</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if Selected; otherwise <c>false</c>. </returns>
    public bool? QueryFrontTerminalsSelected()
    {
        this.FrontTerminalsSelected = this.Session.Query( this.FrontTerminalsSelected, this.FrontTerminalsSelectedQueryCommand );
        return this.FrontTerminalsSelected;
    }

    /// <summary> Gets or sets the Front Terminals Selected command Format. </summary>
    /// <remarks> SCPI: ":SYST:FRSW {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The Front Terminals Selected query command. </value>
    protected virtual string FrontTerminalsSelectedCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Front Terminals Selected sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is Selected. </param>
    /// <returns> <c>true</c> if Selected; otherwise <c>false</c>. </returns>
    public bool? WriteFrontTerminalsSelected( bool value )
    {
        this.FrontTerminalsSelected = this.Session.WriteLine( value, this.FrontTerminalsSelectedCommandFormat );
        return this.FrontTerminalsSelected;
    }

    #endregion

    #region " memory: initialize "

    /// <summary> Gets or sets the initialize memory command. </summary>
    /// <value> The initialize memory command. </value>
    protected virtual string InitializeMemoryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Initializes battery backed RAM. This initializes trace, source list, user-defined math,
    /// source-memory locations, standard save setups, and all call math expressions.
    /// </summary>
    /// <remarks> Sends the <see cref="InitializeMemoryCommand"/> message. </remarks>
    public void InitializeMemory()
    {
        if ( !string.IsNullOrWhiteSpace( this.InitializeMemoryCommand ) )
        {
            _ = this.Session.WriteLine( this.InitializeMemoryCommand );
        }
    }

    #endregion

    #region " options "

    /// <summary> Gets or sets the option query command. </summary>
    /// <value> The option query command. </value>
    protected virtual string OptionQueryCommand { get; set; } = string.Empty;

    /// <summary> Options for controlling the operation. </summary>
    private string _options = string.Empty;

    /// <summary> Gets or sets options for controlling the operation. </summary>
    /// <value> The options. </value>
    public string Options
    {
        get => this._options;
        set
        {
            if ( !string.Equals( value, this.Options, StringComparison.Ordinal ) )
            {
                this._options = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Queries the options. </summary>
    /// <returns> The options. </returns>
    public virtual string QueryOptions()
    {
        this.Options = string.IsNullOrWhiteSpace( this.OptionQueryCommand ) ? string.Empty : this.Session?.Query( this.OptionQueryCommand ) ?? string.Empty;
        this.ContactCheckSupported = this.IsContactCheckSupported( this.Options );
        _ = this.EnumerateInstalledScanCards( this.Options );
        _ = this.ParseMemoryOption( this.Options );
        return this.Options;
    }

    #endregion

    #region " memory option "

    /// <summary> The memory option. </summary>
    private int? _memoryOption;

    /// <summary> Gets or sets the memory Option. </summary>
    /// <value> The memory Option. </value>
    public int? MemoryOption
    {
        get => this._memoryOption;

        protected set
        {
            if ( !Equals( this.MemoryOption, value ) )
            {
                this._memoryOption = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the memory 1 option value. </summary>
    /// <value> The memory 1 option value. </value>
    protected virtual string Memory1OptionValue { get; set; } = "MEM1";

    /// <summary> Gets or sets the memory 2 option value. </summary>
    /// <value> The memory 2 option value. </value>
    protected virtual string Memory2OptionValue { get; set; } = "MEM2";

    /// <summary> Parses the memory option. </summary>
    /// <param name="options"> The options. </param>
    /// <returns> An Integer. </returns>
    public virtual int ParseMemoryOption( string options )
    {
        this.MemoryOption = string.IsNullOrWhiteSpace( options )
            ? 0
            : options.IndexOf( this.Memory1OptionValue, StringComparison.OrdinalIgnoreCase ) >= 0
                ? 1
                : options.IndexOf( this.Memory2OptionValue, StringComparison.OrdinalIgnoreCase ) >= 0 ? 2 : 0;

        return this.MemoryOption.Value;
    }

    #endregion

    #region " preset "

    /// <summary> Gets or sets the preset command. </summary>
    /// <remarks>
    /// SCPI: ":SYST:PRES".
    /// <see cref="VI.Syntax.ScpiSyntax.StatusPresetCommand"> </see>
    /// </remarks>
    /// <value> The preset command. </value>
    protected virtual string PresetCommand { get; set; } = VI.Syntax.ScpiSyntax.SystemPresetCommand;

    /// <summary> Sets the known preset state. </summary>
    public override void PresetKnownState()
    {
        if ( this.Session.IsDeviceOpen && !string.IsNullOrWhiteSpace( this.PresetCommand ) )
        {
            this.Session.SetLastAction( $"presetting {this.GetType().Name}" );
            _ = this.Session.WriteLineIfDefined( this.PresetCommand );
            _ = SessionBase.AsyncDelay( this.PresetRefractoryPeriod );
        }
    }

    private TimeSpan _presetRefractoryPeriod = TimeSpan.FromMilliseconds( 10d );

    /// <summary> Gets or sets the post-preset refractory period. </summary>
    /// <value> The post-preset refractory period. </value>
    public TimeSpan PresetRefractoryPeriod
    {
        get => this._presetRefractoryPeriod;
        set => _ = this.SetProperty( ref this._presetRefractoryPeriod, value );
    }

    #endregion

    #region " scan-card installed "

    private readonly List<string> _installedScanCards = [];

    /// <summary> Gets or sets the installed scan cards. </summary>
    /// <value> The installed scan cards. </value>
    public IList<string> InstalledScanCards
    {
        get => this._installedScanCards;
        set
        {
            if ( !Equals( value, this.InstalledScanCards ) )
            {
                this._installedScanCards.Clear();
                this._installedScanCards.AddRange( value );
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets a list of names of the scan cards. The list is empty of the instrument does not support
    /// scan cards.
    /// </summary>
    /// <value> A list of names of the scan cards. </value>
    protected virtual IList<string> ScanCardNames { get; set; } = Array.Empty<string>();

    /// <summary> Enumerates the installed scan cards. </summary>
    /// <param name="options"> The options. </param>
    /// <returns> <c>true</c> if any scan cards are installed; otherwise <c>false</c> </returns>
    public virtual bool EnumerateInstalledScanCards( string options )
    {
        List<string> l = [];
        if ( this.ScanCardNames.Any() )
        {
            foreach ( string cardName in this.ScanCardNames )
            {
                if ( options.Contains( cardName ) )
                    l.Add( cardName );
            }
        }

        this.InstalledScanCards = l;
        return this.InstalledScanCards.Any();
    }

    /// <summary> Queries the installed scan cards. </summary>
    /// <returns> <c>true</c> if any scan cards were installed; otherwise <c>false</c>. </returns>
    public virtual bool? QueryInstalledScanCards()
    {
        _ = this.QueryOptions();
        return this.InstalledScanCards.Any();
    }

    /// <summary> Returns true if the instrument supports scan card options. </summary>
    /// <value> <c>true</c> if the instrument supports scan card options. </value>
    public bool SupportsScanCardOption => this.ScanCardNames.Any();

    #endregion

    #region " version: firmware "

    /// <summary> The Firmware Version. </summary>
    private double? _firmwareVersion;

    /// <summary> Gets or sets the cached version the Firmware. </summary>
    /// <value> The Firmware Version. </value>
    public double? FirmwareVersion
    {
        get => this._firmwareVersion;

        protected set
        {
            this._firmwareVersion = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Gets or sets the Firmware Version query command. </summary>
    /// <value> The Firmware Version query command. </value>
    protected virtual string FirmwareVersionQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Firmware version. </summary>
    /// <returns> System.Nullable{System.Double}. </returns>
    public double? QueryFirmwareVersion()
    {
        if ( !string.IsNullOrWhiteSpace( this.FirmwareVersionQueryCommand ) )
        {
            this._firmwareVersion = this.Session.Query( 0.0d, this.FirmwareVersionQueryCommand );
        }

        return this.FirmwareVersion;
    }

    #endregion

    #region " version: language "

    /// <summary> The Language revision. </summary>
    private double? _languageRevision;

    /// <summary>
    /// Gets or sets the cached version level of the Language standard implemented by the device.
    /// </summary>
    /// <value> The Language revision. </value>
    public double? LanguageRevision
    {
        get => this._languageRevision;

        protected set
        {
            this._languageRevision = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Gets or sets the Language revision query command. </summary>
    /// <remarks> ':SYST:VERS?'. </remarks>
    /// <value> The Language revision query command. </value>
    protected virtual string LanguageRevisionQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the version level of the Language standard implemented by the device.
    /// </summary>
    /// <remarks> Sends the ':SYST:VERS?' query. </remarks>
    /// <returns> System.Nullable{System.Double}. </returns>
    public double? QueryLanguageRevision()
    {
        if ( !string.IsNullOrWhiteSpace( this.LanguageRevisionQueryCommand ) )
        {
            this._languageRevision = this.Session.Query( 0.0d, this.LanguageRevisionQueryCommand );
        }

        return this.LanguageRevision;
    }

    #endregion
}
/// <summary> Values that represent fan levels. </summary>
public enum FanLevel
{
    /// <summary> An enum constant representing the normal option. </summary>
    [Description( "Normal" )]
    Normal,

    /// <summary> An enum constant representing the quiet option. </summary>
    [Description( "Quiet" )]
    Quiet
}
