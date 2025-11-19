// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by a Source Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class SourceSubsystemBase : SubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected SourceSubsystemBase( StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;
        this.DefaultFunctionRange = Ranges.NonnegativeFullRange;
        this.DefaultFunctionModeDecimalPlaces = 3;
        this._amount = new cc.isr.UnitsAmounts.Amount( 0d, this.DefaultFunctionUnit );
        this.FunctionUnit = this.DefaultFunctionUnit;
        this._functionRange = this.DefaultFunctionRange;
        this._functionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;

        this.FunctionModeDecimalPlaces = [];
        this.DefineFunctionModeDecimalPlaces();
        this.FunctionModeReadWrites = [];
        this.DefineFunctionModeReadWrites();
        this.FunctionModeRanges = [];
        this.DefineFunctionModeRanges();
        this.FunctionModeUnits = [];
        this.DefineFunctionModeUnits();

        this._levelCaption = string.Empty;
        this._readBackCaption = string.Empty;
        this._readBackAmount = new();

    }

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the clear execution state (CLS) by setting system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    public override void DefineClearExecutionState()
    {
        base.DefineClearExecutionState();
        this.Level = new double?();
        this.DefineFunctionClearKnownState();
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.Amount = new cc.isr.UnitsAmounts.Amount( 0d, this.DefaultFunctionUnit );
        this.FunctionUnit = this.DefaultFunctionUnit;
        this._functionRange = this.DefaultFunctionRange;
        this.FunctionRangeDecimalPlaces = this.DefaultFunctionModeDecimalPlaces;
        this.AutoClearEnabled = false;
        this.AutoDelayEnabled = true;
        this.Delay = TimeSpan.Zero;
        this.SweepPoints = 2500;
    }

    #endregion

    #region " auto clear enabled "

    /// <summary> The automatic clear enabled. </summary>

    /// <summary> Gets or sets the cached Auto Clear Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Clear Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoClearEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.AutoClearEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Auto Clear Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoClearEnabled( bool value )
    {
        _ = this.WriteAutoClearEnabled( value );
        return this.QueryAutoClearEnabled();
    }

    /// <summary> Gets or sets the automatic Clear enabled query command. </summary>
    /// <remarks> SCPI: ":SOUR:CLE:AUTO?". </remarks>
    /// <value> The automatic Clear enabled query command. </value>
    protected virtual string AutoClearEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Clear Enabled sentinel. Also sets the
    /// <see cref="AutoClearEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoClearEnabled()
    {
        this.AutoClearEnabled = this.Session.Query( this.AutoClearEnabled, this.AutoClearEnabledQueryCommand );
        return this.AutoClearEnabled;
    }

    /// <summary> Gets or sets the automatic Clear enabled command Format. </summary>
    /// <remarks> SCPI: ":SOU:CLE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic Clear enabled query command. </value>
    protected virtual string AutoClearEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Clear Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoClearEnabled( bool value )
    {
        this.AutoClearEnabled = this.Session.WriteLine( value, this.AutoClearEnabledCommandFormat );
        return this.AutoClearEnabled;
    }

    #endregion

    #region " auto delay enabled "

    /// <summary> The automatic delay enabled. </summary>

    /// <summary> Gets or sets the cached Auto Delay Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Delay Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoDelayEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.AutoDelayEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Auto Delay Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoDelayEnabled( bool value )
    {
        _ = this.WriteAutoDelayEnabled( value );
        return this.QueryAutoDelayEnabled();
    }

    /// <summary> Gets or sets the automatic delay enabled query command. </summary>
    /// <remarks> SCPI: ":SOUR:DEL:AUTO?". </remarks>
    /// <value> The automatic delay enabled query command. </value>
    protected virtual string AutoDelayEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Delay Enabled sentinel. Also sets the
    /// <see cref="AutoDelayEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoDelayEnabled()
    {
        this.AutoDelayEnabled = this.Session.Query( this.AutoDelayEnabled, this.AutoDelayEnabledQueryCommand );
        return this.AutoDelayEnabled;
    }

    /// <summary> Gets or sets the automatic delay enabled command Format. </summary>
    /// <remarks> SCPI: ":SOUR:DEL:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic delay enabled query command. </value>
    protected virtual string AutoDelayEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Delay Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoDelayEnabled( bool value )
    {
        this.AutoDelayEnabled = this.Session.WriteLine( value, this.AutoDelayEnabledCommandFormat );
        return this.AutoDelayEnabled;
    }

    #endregion

    #region " auto range enabled "

    /// <summary> Auto Range enabled. </summary>
    private bool? _autoRangeEnabled;

    /// <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Range Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public virtual bool? AutoRangeEnabled
    {
        get => this._autoRangeEnabled;

        protected set
        {
            if ( !Equals( this.AutoRangeEnabled, value ) )
            {
                this._autoRangeEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Auto Range Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyAutoRangeEnabled( bool value )
    {
        _ = this.WriteAutoRangeEnabled( value );
        return this.QueryAutoRangeEnabled();
    }

    /// <summary> Gets or sets the automatic Range enabled query command. </summary>
    /// <remarks> SCPI: "SOUR:RANG:AUTO?". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string AutoRangeEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Auto Range Enabled sentinel. Also sets the
    /// <see cref="AutoRangeEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryAutoRangeEnabled()
    {
        this.AutoRangeEnabled = this.Session.Query( this.AutoRangeEnabled, this.AutoRangeEnabledQueryCommand );
        return this.AutoRangeEnabled;
    }

    /// <summary> Gets or sets the automatic Range enabled command Format. </summary>
    /// <remarks> SCPI: "SOUR:RANGE:AUTO {0:'ON';'ON';'OFF'}". </remarks>
    /// <value> The automatic Range enabled query command. </value>
    protected virtual string AutoRangeEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Auto Range Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteAutoRangeEnabled( bool value )
    {
        this.AutoRangeEnabled = this.Session.WriteLine( value, this.AutoRangeEnabledCommandFormat );
        return this.AutoRangeEnabled;
    }

    #endregion

    #region " delay "

    /// <summary> The delay. </summary>
    private TimeSpan? _delay;

    /// <summary> Gets or sets the cached Source Delay. </summary>
    /// <remarks>
    /// The delay is used to delay operation in the Source layer. After the programmed Source event
    /// occurs, the instrument waits until the delay period expires before performing the Device
    /// Action.
    /// </remarks>
    /// <value> The Source Delay or none if not set or unknown. </value>
    public TimeSpan? Delay
    {
        get => this._delay;

        protected set
        {
            if ( !Nullable.Equals( this.Delay, value ) )
            {
                this._delay = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Source Delay. </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The Source Delay or none if unknown. </returns>
    public TimeSpan? ApplyDelay( TimeSpan value )
    {
        _ = this.WriteDelay( value );
        _ = this.QueryDelay();
        return default;
    }

    /// <summary> Gets or sets the delay query command. </summary>
    /// <remarks> SCPI: ":SOUR:DEL?". </remarks>
    /// <value> The delay query command. </value>
    protected virtual string DelayQueryCommand { get; set; } = string.Empty;

    /// <summary> Gets or sets the Delay format for converting the query to time span. </summary>
    /// <remarks> For example: "s\.FFFFFFF" will convert the result from seconds. </remarks>
    /// <value> The Delay query command. </value>
    protected virtual string DelayFormat { get; set; } = string.Empty;

    /// <summary> Queries the Delay. </summary>
    /// <returns> The Delay or none if unknown. </returns>
    public TimeSpan? QueryDelay()
    {
        this.Delay = this.Session.Query( this.Delay, this.DelayFormat, this.DelayQueryCommand );
        return this.Delay;
    }

    /// <summary> Gets or sets the delay command format. </summary>
    /// <remarks> SCPI: ":SOUR:DEL {0:s\.FFFFFFF}". </remarks>
    /// <value> The delay command format. </value>
    protected virtual string DelayCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Source Delay without reading back the value from the device. </summary>
    /// <param name="value"> The current Delay. </param>
    /// <returns> The Source Delay or none if unknown. </returns>
    public TimeSpan? WriteDelay( TimeSpan value )
    {
        _ = this.Session.WriteLine( value, this.DelayCommandFormat );
        this.Delay = value;
        return this.Delay;
    }

    #endregion

    #region " level "

    /// <summary> The amount. </summary>
    private cc.isr.UnitsAmounts.Amount _amount;

    /// <summary> Gets or sets the amount. </summary>
    /// <value> The amount. </value>
    public cc.isr.UnitsAmounts.Amount Amount
    {
        get => this._amount;
        set
        {
            this._amount = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Creates a new amount. </summary>
    /// <param name="unit"> The unit. </param>
    private void NewAmount( cc.isr.UnitsAmounts.Unit unit )
    {
        if ( this.Level.HasValue )
        {
            this._amount = new cc.isr.UnitsAmounts.Amount( this.Level.Value, unit );
            this.LevelCaption = $"{this.Amount} {this.Amount.Unit}";
        }
        else
        {
            this._amount = new cc.isr.UnitsAmounts.Amount( 0d, unit );
            this.LevelCaption = $"-.---- {this.Amount.Unit}";
        }

        this.NotifyPropertyChanged( nameof( this.Amount ) );
        this.NotifyPropertyChanged( nameof( this.FunctionUnit ) );
    }

    /// <summary> The level caption. </summary>
    private string _levelCaption;

    /// <summary> Gets or sets the Level caption. </summary>
    /// <value> The Level caption. </value>
    public string LevelCaption
    {
        get => this._levelCaption;
        set
        {
            if ( !string.Equals( value, this.LevelCaption, StringComparison.OrdinalIgnoreCase ) )
            {
                this._levelCaption = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached Source Current Level. </summary>
    /// <value> The Source Current Level. Actual current depends on the power supply mode. </value>
    public double? Level
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Level, value ) )
            {
                field = value;
                this.NewAmount( this.FunctionUnit );
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the source current level. </summary>
    /// <remarks>
    /// This command set the immediate output current level. The value is in Amperes. The immediate
    /// level is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <param name="value"> The current level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? ApplyLevel( double value )
    {
        _ = this.WriteLevel( value );
        return this.QueryLevel();
    }

    /// <summary> Gets or sets The Level query command. </summary>
    /// <value> The Level query command. </value>
    protected virtual string LevelQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current level. </summary>
    /// <returns> The current level or none if unknown. </returns>
    public double? QueryLevel()
    {
        this.Level = this.Session.Query( this.Level.GetValueOrDefault( 0d ), this.LevelQueryCommand );
        return this.Level;
    }

    /// <summary> Gets or sets The Level command format. </summary>
    /// <value> The Level command format. </value>
    protected virtual string LevelCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the source current level without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output current level. The value is in Amperes. The immediate
    /// level is the output current setting. At *RST, the current values = 0.
    /// </remarks>
    /// <param name="value"> The current level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? WriteLevel( double value )
    {
        _ = this.Session.WriteLine( this.LevelCommandFormat, value );
        this.Level = value;
        return this.Level;
    }

    #endregion

    #region " limit "

    /// <summary> The Limit. </summary>

    /// <summary>
    /// Gets or sets the cached source Limit for a Current Source. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Limit
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Limit, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the source Limit. </summary>
    /// <remarks>
    /// This command set the immediate output Limit. The value is in Amperes. The immediate Limit is
    /// the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Limit. </param>
    /// <returns> The Source Limit. </returns>
    public double? ApplyLimit( double value )
    {
        _ = this.WriteLimit( value );
        return this.QueryLimit();
    }

    /// <summary> Gets or sets the limit query command. </summary>
    /// <value> The limit query command. </value>
    protected virtual string ModalityLimitQueryCommandFormat { get; set; } = string.Empty;

    /// <summary> Queries the Limit. </summary>
    /// <returns> The Limit or none if unknown. </returns>
    public virtual double? QueryLimit()
    {
        this.Limit = this.Session.Query( this.Limit, this.ModalityLimitQueryCommandFormat );
        return this.Limit;
    }

    /// <summary> Gets or sets the modality limit command format. </summary>
    /// <value> The modality limit command format. </value>
    protected virtual string ModalityLimitCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the source Limit without reading back the value from the device. </summary>
    /// <remarks>
    /// This command set the immediate output Limit. The value is in Amperes. The immediate Limit is
    /// the output Voltage setting. At *RST, the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Limit. </param>
    /// <returns> The Source Limit. </returns>
    public virtual double? WriteLimit( double value )
    {
        _ = this.Session.WriteLine( value, this.ModalityLimitCommandFormat );
        this.Limit = value;
        return this.Limit;
    }

    #endregion

    #region " limit tripped "

    /// <summary> Limit Tripped. </summary>

    /// <summary> Gets or sets the cached Limit Tripped sentinel. </summary>
    /// <value>
    /// <c>null</c> if Limit Tripped is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? LimitTripped
    {
        get;

        protected set
        {
            if ( !Equals( this.LimitTripped, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the limit tripped command format. </summary>
    /// <value> The limit tripped command format. </value>
    protected virtual string LimitTrippedQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Limit Tripped sentinel. Also sets the
    /// <see cref="LimitTripped">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryLimitTripped()
    {
        this.LimitTripped = this.Session.Query( this.LimitTripped, this.LimitTrippedQueryCommand );
        return this.LimitTripped;
    }

    #endregion

    #region " output enabled "

    /// <summary> Output enabled. </summary>

    /// <summary> Gets or sets the cached Output Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Output Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? OutputEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.OutputEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Output Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyOutputEnabled( bool value )
    {
        _ = this.WriteOutputEnabled( value );
        return this.QueryOutputEnabled();
    }

    /// <summary> Gets or sets the Output enabled query command. </summary>
    /// <value> The Output enabled query command. </value>
    protected virtual string OutputEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Output Enabled sentinel. Also sets the
    /// <see cref="OutputEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryOutputEnabled()
    {
        this.OutputEnabled = this.Session.Query( this.OutputEnabled, this.OutputEnabledQueryCommand );
        return this.OutputEnabled;
    }

    /// <summary> Gets or sets the Output enabled command Format. </summary>
    /// <value> The Output enabled query command. </value>
    protected virtual string OutputEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Output Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteOutputEnabled( bool value )
    {
        this.OutputEnabled = this.Session.WriteLine( value, this.OutputEnabledCommandFormat );
        return this.OutputEnabled;
    }

    #endregion

    #region " range "

    /// <summary> The Range. </summary>

    /// <summary>
    /// Gets or sets the cached sense Range. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Range
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Range, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the sense Range. </summary>
    /// <param name="value"> The Range. </param>
    /// <returns> The Range. </returns>
    public double? ApplyRange( double value )
    {
        _ = this.WriteRange( value );
        return this.QueryRange();
    }

    /// <summary> Gets or sets The Range query command. </summary>
    /// <value> The Range query command. </value>
    protected virtual string RangeQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries The Range. </summary>
    /// <returns> The Range or none if unknown. </returns>
    public double? QueryRange()
    {
        this.Range = this.Session.Query( this.Range, this.RangeQueryCommand );
        return this.Range;
    }

    /// <summary> Gets or sets The Range command format. </summary>
    /// <value> The Range command format. </value>
    protected virtual string RangeCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes The Range without reading back the value from the device. </summary>
    /// <remarks> This command sets The Range. </remarks>
    /// <param name="value"> The Range. </param>
    /// <returns> The Range. </returns>
    public double? WriteRange( double value )
    {
        this.Range = this.Session.WriteLine( value, this.RangeCommandFormat );
        return this.Range;
    }

    #endregion

    #region " read back enabled "

    /// <summary> The last read back. </summary>

    /// <summary> Gets or sets the last ReadBack. </summary>
    /// <value> The last ReadBack. </value>
    public string? LastReadBack
    {
        get;
        set
        {
            if ( !string.Equals( value, this.LastReadBack, StringComparison.OrdinalIgnoreCase ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The read back caption. </summary>
    private string _readBackCaption;

    /// <summary> Gets or sets the ReadBack caption. </summary>
    /// <value> The ReadBack caption. </value>
    public string ReadBackCaption
    {
        get => this._readBackCaption;
        set
        {
            if ( !string.Equals( value, this.ReadBackCaption, StringComparison.OrdinalIgnoreCase ) )
            {
                this._readBackCaption = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> The read back amount. </summary>
    private cc.isr.UnitsAmounts.Amount _readBackAmount;

    /// <summary> Gets or sets the read back amount. </summary>
    /// <value> The amount. </value>
    public cc.isr.UnitsAmounts.Amount ReadBackAmount
    {
        get => this._readBackAmount;
        set
        {
            this._readBackAmount = value;
            this.NotifyPropertyChanged();
            this.HasReadBackAmount = value is not null;
        }
    }

    /// <summary> Gets or sets the has read back amount. </summary>
    /// <value> The has read back amount. </value>
    public bool HasReadBackAmount
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Parse read back amount. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> A Double. </returns>
    public double ParseReadBackAmount( string value )
    {
        double result = 0d;
        string caption;
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            this.ReadBackAmount = new();
            caption = $"-.---- {this.Amount.Unit}";
        }
        else if ( double.TryParse( value, out result ) )
        {
            this.ReadBackAmount = new cc.isr.UnitsAmounts.Amount( result, this.Amount.Unit );
            caption = $"{this.ReadBackAmount} {this.ReadBackAmount.Unit}";
        }
        else
        {
            this.ReadBackAmount = new();
            caption = $"-NAN- {this.Amount.Unit}";
        }

        this.ReadBackCaption = caption;
        this.LastReadBack = value;
        return result;
    }

    /// <summary> Gets or sets the query source value command format. </summary>
    /// <value> The query source value command format. </value>
    protected virtual string QuerySourceValueCommandFormat { get; set; } = "_G.print(defbuffer1.sourcevalues[{0}])";

    /// <summary> Parse read back buffer amount. </summary>
    /// <param name="index"> Zero-based index of the. </param>
    /// <returns> A Double. </returns>
    public double ParseReadBackBufferAmount( int index )
    {
        string? value = "unknown";
        string? result = this.Session.QueryTrimEnd( value, this.QuerySourceValueCommandFormat, index );
        return (result == value)
            ? double.NaN
            : this.ParseReadBackAmount( value );
    }

    /// <summary> Parse read back buffer amount. </summary>
    /// <returns> A Double. </returns>
    public double ParseReadBackBufferAmount()
    {
        string? value = "unknown";
        string? result = this.Session.QueryTrimEnd( value, this.QuerySourceValueCommandFormat, "defbuffer1.n" );
        return (result == value)
            ? double.NaN
            : this.ParseReadBackAmount( value );
    }

    #endregion

    #region " read back enabled "

    /// <summary> The read back enabled. </summary>

    /// <summary> Gets or sets the cached Read Back Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Read Back Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? ReadBackEnabled
    {
        get;

        protected set
        {
            if ( !Equals( this.ReadBackEnabled, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Read Back Enabled sentinel. </summary>
    /// <param name="value"> if set to <c>true</c> if enabling; False if disabling. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? ApplyReadBackEnabled( bool value )
    {
        _ = this.WriteReadBackEnabled( value );
        return this.QueryReadBackEnabled();
    }

    /// <summary> Gets or sets the Read Back enabled query command. </summary>
    /// <remarks> SCPI: :SOUR:function:READ:BACK? TSP:  smu.source.readback. </remarks>
    /// <value> The Read Back enabled query command. </value>
    protected virtual string ReadBackEnabledQueryCommand { get; set; } = string.Empty;

    /// <summary>
    /// Queries the Read Back Enabled sentinel. Also sets the
    /// <see cref="ReadBackEnabled">Enabled</see> sentinel.
    /// </summary>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? QueryReadBackEnabled()
    {
        this.ReadBackEnabled = this.Session.Query( this.ReadBackEnabled, this.ReadBackEnabledQueryCommand );
        return this.ReadBackEnabled;
    }

    /// <summary> Gets or sets the Read Back enabled command Format. </summary>
    /// <remarks>
    /// SCPI: :SOU:function:READ:BACK {0:'ON';'ON';'OFF'}
    /// TSP: _G.smu.source.readback={0:'smu.ON';'smu.ON';'smu.OFF'}
    /// </remarks>
    /// <value> The Read Back enabled query command. </value>
    protected virtual string ReadBackEnabledCommandFormat { get; set; } = string.Empty;

    /// <summary>
    /// Writes the Read Back Enabled sentinel. Does not read back from the instrument.
    /// </summary>
    /// <param name="value"> if set to <c>true</c> is enabled. </param>
    /// <returns> <c>true</c> if enabled; otherwise <c>false</c>. </returns>
    public bool? WriteReadBackEnabled( bool value )
    {
        this.ReadBackEnabled = this.Session.WriteLine( value, this.ReadBackEnabledCommandFormat );
        return this.ReadBackEnabled;
    }

    #endregion

    #region " sweep points "

    /// <summary> The sweep point. </summary>

    /// <summary> Gets or sets the cached Sweep Points. </summary>
    /// <value> The Sweep Points or none if not set or unknown. </value>
    public int? SweepPoints
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.SweepPoints, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Sweep Points. </summary>
    /// <param name="value"> The current SweepPoints. </param>
    /// <returns> The SweepPoints or none if unknown. </returns>
    public int? ApplySweepPoints( int value )
    {
        _ = this.WriteSweepPoints( value );
        return this.QuerySweepPoints();
    }

    /// <summary> Gets or sets Sweep Points query command. </summary>
    /// <remarks> SCPI: ":SOUR:SWE:POIN?". </remarks>
    /// <value> The Sweep Points query command. </value>
    protected virtual string SweepPointsQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the current Sweep Points. </summary>
    /// <returns> The Sweep Points or none if unknown. </returns>
    public int? QuerySweepPoints()
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepPointsQueryCommand ) )
        {
            this.SweepPoints = this.Session.Query( 0, this.SweepPointsQueryCommand );
        }

        return this.SweepPoints;
    }

    /// <summary> Gets or sets Sweep Points command format. </summary>
    /// <remarks> SCPI: ":SOUR:SWE:POIN {0}". </remarks>
    /// <value> The Sweep Points command format. </value>
    protected virtual string SweepPointsCommandFormat { get; set; } = string.Empty;

    /// <summary> WriteEnum the Sweep Points without reading back the value from the device. </summary>
    /// <param name="value"> The current Sweep Points. </param>
    /// <returns> The Sweep Points or none if unknown. </returns>
    public int? WriteSweepPoints( int value )
    {
        if ( !string.IsNullOrWhiteSpace( this.SweepPointsCommandFormat ) )
        {
            _ = this.Session.WriteLine( this.SweepPointsCommandFormat, value );
        }

        this.SweepPoints = value;
        return this.SweepPoints;
    }

    #endregion
}
