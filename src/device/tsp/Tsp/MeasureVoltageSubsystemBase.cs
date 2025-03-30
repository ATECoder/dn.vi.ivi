namespace cc.isr.VI.Tsp;
/// <summary>
/// Defines the contract that must be implemented by a Source Measure Unit Measure Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="StatusSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="VI.StatusSubsystemBase">TSP status
/// Subsystem</see>. </param>
public class MeasureVoltageSubsystemBase( Tsp.StatusSubsystemBase statusSubsystem ) : SourceMeasureUnitBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.AutoRangeVoltageEnabled = true;
    }

    #endregion

    #region " auto range voltage enabled "

    /// <summary> Auto Range Voltage enabled. </summary>
    private bool? _autoRangeVoltageEnabled;

    /// <summary> Gets or sets the cached Auto Range Voltage Enabled sentinel. </summary>
    /// <value>
    /// <c>null</c> if Auto Range Voltage Enabled is not known; <c>true</c> if output is on;
    /// otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? AutoRangeVoltageEnabled
    {
        get => this._autoRangeVoltageEnabled;
        protected set => _ = this.SetProperty( ref this._autoRangeVoltageEnabled, value );
    }

    /// <summary>
    /// Writes the enabled state of the current Auto Range Voltage and reads back the value from the
    /// device.
    /// </summary>
    /// <remarks>
    /// This command enables or disables the over-current Auto Range Voltage (OCP)
    /// function. The enabled state is On (1); the disabled state is Off (0). If the over-current
    /// AutoRangeVoltage function is enabled and the output goes into constant current operation, the
    /// output is disabled and OCP is set in the Questionable Condition status register. The system reset
    /// (G_reset()) value is Off.
    /// </remarks>
    /// <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    /// <returns>
    /// <c>true</c> <see cref="AutoRangeVoltageEnabled">enabled</see>;
    /// <c>false</c> otherwise.
    /// </returns>
    public bool? ApplyAutoRangeVoltageEnabled( bool value )
    {
        _ = this.WriteAutoRangeVoltageEnabled( value );
        return this.QueryAutoRangeVoltageEnabled();
    }

    /// <summary> Gets the automatic range voltage enabled query command. </summary>
    /// <value> The automatic range voltage enabled query command. </value>
    protected virtual string AutoRangeVoltageEnabledQueryCommand => string.Format( this.AutoRangeVoltageEnabledQueryCommandFormat, this.SourceMeasureUnitReference );

    /// <summary> Gets the automatic range voltage enabled query command format. </summary>
    /// <value> The automatic range voltage enabled query command format. </value>
    protected virtual string AutoRangeVoltageEnabledQueryCommandFormat { get; set; } = "_G.print({0}.measure.autorangev)";

    /// <summary> Queries the current AutoRangeVoltage state. </summary>
    /// <returns>
    /// <c>true</c> <see cref="AutoRangeVoltageEnabled">enabled</see>;
    /// <c>false</c> otherwise.
    /// </returns>
    public bool? QueryAutoRangeVoltageEnabled()
    {
        this.AutoRangeVoltageEnabled = this.Session.Query( this.AutoRangeVoltageEnabled.GetValueOrDefault( true ), this.AutoRangeVoltageEnabledQueryCommand );
        // Me.AutoRangeVoltageEnabled = Me.Session.Query(Me.AutoRangeVoltageEnabled.GetValueOrDefault(True), "print({0}.measure.autorangev)", Me.SourceMeasureUnitReference)
        return this.AutoRangeVoltageEnabled;
    }

    /// <summary> Gets the automatic range voltage enabled command format. </summary>
    /// <value> The automatic range voltage enabled command format. </value>
    protected virtual string AutoRangeVoltageEnabledCommandFormat { get; set; } = "{0}.measure.autorangev = {{0:'1';'1';'0'}} ";

    /// <summary> Gets the automatic range voltage enabled command. </summary>
    /// <value> The automatic range voltage enabled command. </value>
    protected virtual string AutoRangeVoltageEnabledCommand => string.Format( this.AutoRangeVoltageEnabledCommandFormat, this.SourceMeasureUnitReference );

    /// <summary>
    /// Writes the enabled state of the current Auto Range Voltage without reading back the value
    /// from the device.
    /// </summary>
    /// <remarks>
    /// This command enables or disables the over-current AutoRangeVoltage (OCP)
    /// function. The enabled state is On (1); the disabled state is Off (0). If the over-current
    /// AutoRangeVoltage function is enabled and the output goes into constant current operation, the
    /// output is disabled and OCP is set in the Questionable Condition status register. The system
    /// reset (G_reset()) value is Off.
    /// </remarks>
    /// <param name="value"> Enable if set to <c>true</c>; otherwise, disable. </param>
    /// <returns>
    /// <c>true</c> <see cref="AutoRangeVoltageEnabled">enabled</see>;
    /// <c>false</c> otherwise.
    /// </returns>
    public bool? WriteAutoRangeVoltageEnabled( bool value )
    {
        // this.Session.WriteLine ( string.Format(Globalization.CultureInfo.InvariantCulture,
        //    "{0}.measure.autorangev = {{0:'1';'1';'0'}} ", this.SourceMeasureUnitReference), CType(value, Integer))
        _ = this.Session.WriteLine( this.AutoRangeVoltageEnabledCommand, value.GetHashCode() );
        this.AutoRangeVoltageEnabled = value;
        return this.AutoRangeVoltageEnabled;
    }

    #endregion

    #region " reading "

    /// <summary> The reading. </summary>
    private string? _reading;

    /// <summary>
    /// Gets or sets  or sets (protected) the reading.  When set, the value is converted to
    /// resistance.
    /// </summary>
    /// <value> The reading. </value>
    public string? Reading
    {
        get => this._reading;
        protected set => _ = this.SetProperty( ref this._reading, value );
    }

    #endregion

    #region " voltage "

    /// <summary> The voltage. </summary>
    private double? _voltage;

    /// <summary> Gets or sets (protected) the measured resistance. </summary>
    /// <value> The resistance. </value>
    public double? Voltage
    {
        get => this._voltage;
        protected set => _ = this.SetProperty( ref this._voltage, value );
    }

    /// <summary> Turns on the source and measures. </summary>
    /// <exception cref="InvalidCastException"> Thrown when an object cannot be cast to a required
    /// type. </exception>
    public void Measure()
    {
        string printFormat = "%8.5f";
        _ = this.Session.WriteLine( "{0}.source.output = {0}.OUTPUT_ON waitcomplete() print(string.format('{1}',{0}.measure.r())) ", this.SourceMeasureUnitReference, printFormat );
        this.Reading = this.Session.ReadLine();
        if ( string.IsNullOrWhiteSpace( this.Reading ) )
        {
            this.Voltage = new double?();
        }
        else if ( double.TryParse( this.Reading, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                                   System.Globalization.CultureInfo.InvariantCulture, out double value ) )
        {
            this.Voltage = value;
        }
        else
        {
            this.Voltage = new double?();
            throw new InvalidCastException( string.Format( System.Globalization.CultureInfo.InvariantCulture, "Failed parsing {0} to number reading '{1}'", this.Reading, this.Session.LastMessageSent ) );
        }
    }

    #endregion
}
