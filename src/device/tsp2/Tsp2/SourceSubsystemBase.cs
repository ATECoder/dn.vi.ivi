using System.Diagnostics;
using cc.isr.Enums;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp2;

/// <summary>
/// Defines the contract that must be implemented by a Source Current Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class SourceSubsystemBase : VI.SourceSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a
    /// <see cref="isr.VI.StatusSubsystemBase">status
    /// subsystem</see>. </param>
    protected SourceSubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
        this.FunctionModeReadWrites.AddReplace( ( long ) SourceFunctionModes.CurrentDC, "smu.FUNC_DC_CURRENT", SourceFunctionModes.CurrentDC.DescriptionUntil() );
        this.FunctionModeReadWrites.AddReplace( ( long ) SourceFunctionModes.VoltageDC, "smu.FUNC_DC_VOLTAGE", SourceFunctionModes.VoltageDC.DescriptionUntil() );
        this.FunctionModeRanges[( int ) SourceFunctionModes.CurrentDC].SetRange( -1.05d, 1.05d );
        this.FunctionModeRanges[( int ) SourceFunctionModes.VoltageDC].SetRange( -210, 210d );
    }

    #endregion

    #region " auto range state "

    /// <summary> Gets or sets the cached Auto Range Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Range Enabled is not known; <c>true</c> if output is on; otherwise,
    /// <c>false</c>.
    /// </value>
    public override bool? AutoRangeEnabled
    {
        get => base.AutoRangeEnabled;
        protected set
        {
            if ( !Equals( this.AutoRangeEnabled, value ) )
            {
                base.AutoRangeEnabled = value;
                this.AutoRangeState = value.HasValue ? value.Value ? OnOffState.On : OnOffState.Off : new OnOffState?();
            }
        }
    }

    /// <summary> State of the automatic range. </summary>
    private OnOffState? _autoRangeState;

    /// <summary> Gets or sets the Auto Range. </summary>
    /// <value> The automatic range state. </value>
    public OnOffState? AutoRangeState
    {
        get => this._autoRangeState;
        protected set
        {
            if ( !Nullable.Equals( value, this.AutoRangeState ) )
            {
                this._autoRangeState = value;
                this.AutoRangeEnabled = value.HasValue ? value.Value == OnOffState.On : new bool?();
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the AutoRange state. </summary>
    /// <param name="value"> The Aperture. </param>
    /// <returns> An OnOffState? </returns>
    public OnOffState? ApplyAutoRangeState( OnOffState value )
    {
        _ = this.WriteAutoRangeState( value );
        return this.QueryAutoRangeState();
    }

    /// <summary> Gets the Auto Range state query command. </summary>
    /// <value> The Auto Range state query command. </value>
    protected virtual string AutoRangeStateQueryCommand { get; set; } = Syntax.Tsp.Lua.PrintCommand( "_G.smu.measure.autorange" );

    /// <summary> Queries automatic range state. </summary>
    /// <returns> The automatic range state. </returns>
    public OnOffState? QueryAutoRangeState()
    {
        this.Session.LastAction = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Reading {nameof( this.AutoRangeState )};. " );
        this.Session.LastNodeNumber = new int?();
        string mode = this.AutoRangeState.ToString();
        this.Session.MakeEmulatedReplyIfEmpty( mode );
        mode = this.Session.QueryTrimEnd( this.AutoRangeStateQueryCommand );
        if ( string.IsNullOrWhiteSpace( mode ) )
        {
            string message = $"Failed fetching {nameof( this.AutoRangeState )}";
            Debug.Assert( !Debugger.IsAttached, message );
            this.AutoRangeState = new OnOffState?();
        }
        else
        {
            this.AutoRangeState = SessionBase.ParseContained<OnOffState>( mode.BuildDelimitedValue() );
        }

        return this.AutoRangeState;
    }

    /// <summary> The Auto Range state command format. </summary>
    /// <value> The automatic range state command format. </value>
    protected virtual string AutoRangeStateCommandFormat { get; set; } = "_G.smu.measure.autorange={0}";

    /// <summary> Writes an automatic range state. </summary>
    /// <param name="value"> The Aperture. </param>
    /// <returns> An OnOffState. </returns>
    public OnOffState WriteAutoRangeState( OnOffState value )
    {
        this.Session.LastAction = cc.isr.VI.SessionLogger.Instance.LogInformation( $"Writing {nameof( this.AutoRangeState )}={value};. " );
        this.Session.LastNodeNumber = new int?();
        _ = this.Session.WriteLine( this.AutoRangeStateCommandFormat, value.ExtractBetween() );
        this.AutoRangeState = value;
        return value;
    }

    #endregion

    #region " function mode "

    /// <summary> Gets the function mode query command. </summary>
    /// <value> The function mode query command. </value>
    protected override string FunctionModeQueryCommand { get; set; } = Syntax.Tsp.Lua.PrintCommand( "_G.smu.Source.func" );

    /// <summary> Gets the function mode command format. </summary>
    /// <value> The function mode command format. </value>
    protected override string FunctionModeCommandFormat { get; set; } = "_G.smu.Source.func={0}";

    #endregion

    #region " syntax "

    #region " limit "

    /// <summary> The current limit function. </summary>
    private const string Current_Limit_Function = "i";

    /// <summary> The voltage limit function. </summary>
    private const string Voltage_Limit_Function = "v";

    /// <summary> Limit function mode. </summary>
    /// <returns> A <see cref="string" />. </returns>
    private string LimitFunctionMode()
    {
        return this.FunctionMode is null
            ? Voltage_Limit_Function
            : this.FunctionMode.Value == SourceFunctionModes.CurrentDC ? Voltage_Limit_Function : Current_Limit_Function;
    }

    /// <summary>   Gets or sets the limit query resolution. </summary>
    /// <value> The limit query resolution. </value>
    protected virtual double LimitQueryResolution { get; set; } = 9.6;

    /// <summary> Gets the limit query format. </summary>
    /// <value> The limit query format. </value>
    /// <remarks> _G.smu.source.{0}limit.level </remarks>
    protected virtual string LimitQueryFormat { get; set; } = string.Empty;

    /// <summary> Gets the limit query command format. </summary>
    /// <value> The limit query command format. </value>
    /// <remarks> _G.print(_G.smu.source.vlimit.level) </remarks>
    protected virtual string LimitQueryCommandFormat()
    {
        return string.Format( Syntax.Tsp.Lua.PrintCommandFormat, string.Format( this.LimitQueryFormat, this.LimitFunctionMode() ) );
    }

    /// <summary>   Limit query formatted command format. </summary>
    /// <remarks>   David, 2021-07-10. </remarks>
    /// <param name="resolution">   The resolution. </param>
    /// <returns>   A <see cref="string" />. </returns>
    protected virtual string LimitQueryCommandFormat( double resolution )
    {
        return string.Format( Syntax.Tsp.Lua.PrintCommandStringNumberFormat, resolution.ToString(), string.Format( this.LimitQueryFormat, this.LimitFunctionMode() ) );
    }

    /// <summary> Gets the limit query command. </summary>
    /// <value> The limit query command. </value>
    protected override string ModalityLimitQueryCommandFormat
    {
        get => this.LimitQueryCommandFormat( this.LimitQueryResolution );
        set { }
    }

    /// <summary> Gets the limit command format. </summary>
    /// <value> The limit command format. </value>
    protected virtual string LimitCommandFormat { get; set; } = string.Empty;

    /// <summary> Gets the modality limit command format. </summary>
    /// <value> The modality limit command format. </value>
    protected override string ModalityLimitCommandFormat
    {
        get => string.Format( this.LimitCommandFormat, this.LimitFunctionMode(), "{0}" );
        set { }
    }

    #endregion

    #region " limit tripped "

    /// <summary> Gets the limit tripped query command format. </summary>
    /// <value> The limit tripped query command format. </value>
    protected virtual string LimitTrippedQueryCommandFormat { get; set; } = string.Empty;

    /// <summary> Gets the limit tripped print command. </summary>
    /// <value> The limit tripped print command. </value>
    protected override string LimitTrippedQueryCommand
    {
        get => string.Format( this.LimitTrippedQueryCommandFormat, this.LimitFunctionMode() );
        set
        {
         }
    }

    #endregion

    #endregion

}
