namespace cc.isr.VI.Tsp;
/// <summary>
/// Defines the contract that must be implemented by a Source Current Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="CurrentSourceSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
/// Subsystem</see>. </param>
public class CurrentSourceSubsystemBase( Tsp.StatusSubsystemBase statusSubsystem ) : SourceMeasureUnitBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.Level = 0.105d;
        this.VoltageLimit = 0.000105d;
        this.Range = new double?();
        this.AutoRangeEnabled = true;
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
    public bool? AutoRangeEnabled
    {
        get => this._autoRangeEnabled;
        protected set => _ = this.SetProperty( ref this._autoRangeEnabled, value );
    }

    /// <summary>
    /// Writes the enabled state of the current Auto Range and reads back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command enables or disables the over-current Auto Range (OCP)
    /// function. The enabled state is On (1); the disabled state is Off (0). If the over-current
    /// AutoRange function is enabled and the output goes into constant current operation, the output
    /// is disabled and OCP is set in the Questionable Condition status register. After _G.reset(),
    /// the value is Off.
    /// </remarks>
    /// <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    /// <returns>
    /// <c>true</c> if <see cref="AutoRangeEnabled">Auto Range Enabled</see>;
    /// <c>false</c> otherwise.
    /// </returns>
    public bool? ApplyAutoRangeEnabled( bool value )
    {
        _ = this.WriteAutoRangeEnabled( value );
        return this.QueryAutoRangeEnabled();
    }

    /// <summary> Queries the current AutoRange state. </summary>
    /// <returns> True if the AutoRange is on; Otherwise, False. </returns>
    public bool? QueryAutoRangeEnabled()
    {
        this.AutoRangeEnabled = this.Session.QueryPrint( this.AutoRangeEnabled.GetValueOrDefault( true ),
                                                         "{0}.source.rangei", this.SourceMeasureUnitReference );
        return this.AutoRangeEnabled;
    }

    /// <summary>
    /// Writes the enabled state of the current Auto Range without reading back the value from the
    /// device.
    /// </summary>
    /// <remarks>
    /// This command enables or disables the over-current AutoRange (OCP)
    /// function. The enabled state is On (1); the disabled state is Off (0). If the over-current
    /// AutoRange function is enabled and the output goes into constant current operation, the output
    /// is disabled and OCP is set in the Questionable Condition status register. After _G.reset()
    /// the value is Off.
    /// </remarks>
    /// <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    /// <returns>
    /// <c>true</c> if <see cref="AutoRangeEnabled">Auto Range Enabled</see>;
    /// <c>false</c> otherwise.
    /// </returns>
    public bool? WriteAutoRangeEnabled( bool value )
    {
        _ = this.Session.WriteLine( string.Format( System.Globalization.CultureInfo.InvariantCulture,
                                                   "{0}.source.rangei = {{0:'1';'1';'0'}} ",
                                                   this.SourceMeasureUnitReference ), value.GetHashCode() );
        this.AutoRangeEnabled = value;
        return this.AutoRangeEnabled;
    }

    #endregion

    #region " level "

    /// <summary> The level. </summary>
    private double? _level;

    /// <summary> Gets or sets the cached Source Current Level. </summary>
    /// <value> The Source Current Level. Actual current depends on the power supply mode. </value>
    public double? Level
    {
        get => this._level;
        protected set => _ = this.SetProperty( ref this._level, value );
    }

    /// <summary> Writes and reads back the source current level. </summary>
    /// <remarks>
    /// This command set the immediate output current level. The value is in Amperes. The immediate
    /// level is the output current setting. After _G.reset(), the current values = 0.
    /// </remarks>
    /// <param name="value"> The current level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? ApplyLevel( double value )
    {
        _ = this.WriteLevel( value );
        return this.QueryLevel();
    }

    /// <summary> Queries the current level. </summary>
    /// <returns> The current level or none if unknown. </returns>
    public double? QueryLevel()
    {
        const decimal printFormat = 9.6m;
        this.Level = this.Session.QueryPrint( this.Level.GetValueOrDefault( 0d ), printFormat,
                                                                            "{0}.source.leveli", this.SourceMeasureUnitReference );
        return this.Level;
    }

    /// <summary>
    /// Writes the source current level without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the immediate output current level. The value is in Amperes. The immediate
    /// level is the output current setting. After _G.reset(), the current values = 0.
    /// </remarks>
    /// <param name="value"> The current level. </param>
    /// <returns> The Source Current Level. </returns>
    public double? WriteLevel( double value )
    {
        _ = this.Session.WriteLine( "{0}.source.leveli={1}", this.SourceMeasureUnitReference, value );
        this.Level = value;
        return this.Level;
    }

    #endregion

    #region " range "

    /// <summary> The Current Range. </summary>
    private double? _range;

    /// <summary>
    /// Gets or sets the cached Source current range. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? Range
    {
        get => this._range;
        protected set => _ = this.SetProperty( ref this._range, value );
    }

    /// <summary> Writes and reads back the Source current Range. </summary>
    /// <remarks>
    /// The value is in Amperes. After _G.reset(), the range is set to Auto and the specific range is unknown.
    /// </remarks>
    /// <param name="value"> The current Range. </param>
    /// <returns> The Current Range. </returns>
    public double? ApplyRange( double value )
    {
        _ = this.WriteRange( value );
        return this.QueryRange();
    }

    /// <summary> Queries the current Range. </summary>
    /// <returns> The current Range or none if unknown. </returns>
    public double? QueryRange()
    {
        const decimal printFormat = 9.6m;
        this.Range = this.Session.QueryPrint( this.Range.GetValueOrDefault( 0.99d ), printFormat,
                                                        "{0}.source.rangei", this.SourceMeasureUnitReference );
        return this.Range;
    }

    /// <summary>
    /// Writes the Source current Range without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command sets the current Range. The value is in Amperes. After _G.reset(), the range is auto and
    /// the value is not known.
    /// </remarks>
    /// <param name="value"> The Source current Range. </param>
    /// <returns> The Source Current Range. </returns>
    public double? WriteRange( double value )
    {
        if ( value >= VI.Syntax.ScpiSyntax.Infinity - 1d )
        {
            _ = this.Session.WriteLine( "{0}.source.rangei={0}.source.rangei.max", this.SourceMeasureUnitReference );
            value = VI.Syntax.ScpiSyntax.Infinity;
        }
        else if ( value <= VI.Syntax.ScpiSyntax.NegativeInfinity + 1d )
        {
            _ = this.Session.WriteLine( "{0}.source.rangei={0}.source.rangei.min", this.SourceMeasureUnitReference );
            value = VI.Syntax.ScpiSyntax.NegativeInfinity;
        }
        else
        {
            _ = this.Session.WriteLine( "{0}.source.rangei={1}", this.SourceMeasureUnitReference, value );
        }

        this.Range = value;
        return this.Range;
    }

    #endregion

    #region " voltage limit "

    /// <summary> The Voltage Limit. </summary>
    private double? _voltageLimit;

    /// <summary>
    /// Gets or sets the cached source Voltage Limit for a Current Source. Set to
    /// <see cref="VI.Syntax.ScpiSyntax.Infinity">infinity</see> to set to maximum or to
    /// <see cref="VI.Syntax.ScpiSyntax.NegativeInfinity">negative infinity</see> for minimum.
    /// </summary>
    /// <value> <c>null</c> if value is not known. </value>
    public double? VoltageLimit
    {
        get => this._voltageLimit;
        protected set => _ = this.SetProperty( ref this._voltageLimit, value );
    }

    /// <summary> Writes and reads back the source Voltage Limit. </summary>
    /// <remarks>
    /// This command set the immediate output Voltage Limit. The value is in Amperes. The immediate
    /// Limit is the output Voltage setting. After _G.reset() the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Voltage Limit. </param>
    /// <returns> The Source Voltage Limit. </returns>
    public double? ApplyVoltageLimit( double value )
    {
        _ = this.WriteVoltageLimit( value );
        return this.QueryVoltageLimit();
    }

    /// <summary> Queries the Voltage Limit. </summary>
    /// <returns> The Voltage Limit or none if unknown. </returns>
    public double? QueryVoltageLimit()
    {
        const decimal printFormat = 9.6m;
        this.VoltageLimit = this.Session.QueryPrint( this.VoltageLimit.GetValueOrDefault( 0.099d ), printFormat, "{0}.source.limitv", this.SourceMeasureUnitReference );
        return this.VoltageLimit;
    }

    /// <summary>
    /// Writes the source Voltage Limit without reading back the value from the device.
    /// </summary>
    /// <remarks>
    /// This command set the immediate output Voltage Limit. The value is in Amperes. The immediate
    /// Limit is the output Voltage setting. After _G.reset(), the Voltage values = 0.
    /// </remarks>
    /// <param name="value"> The Voltage Limit. </param>
    /// <returns> The Source Voltage Limit. </returns>
    public double? WriteVoltageLimit( double value )
    {
        if ( value >= VI.Syntax.ScpiSyntax.Infinity - 1d )
        {
            _ = this.Session.WriteLine( "{0}.source.limitv={0}.source.limitv.max", this.SourceMeasureUnitReference );
            value = VI.Syntax.ScpiSyntax.Infinity;
        }
        else if ( value <= VI.Syntax.ScpiSyntax.NegativeInfinity + 1d )
        {
            _ = this.Session.WriteLine( "{0}.source.limitv={0}.source.limitv.min", this.SourceMeasureUnitReference );
            value = VI.Syntax.ScpiSyntax.NegativeInfinity;
        }
        else
        {
            _ = this.Session.WriteLine( "{0}.source.limitv={1}", this.SourceMeasureUnitReference, value );
        }

        this.VoltageLimit = value;
        return this.VoltageLimit;
    }

    #endregion
}
