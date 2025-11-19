namespace cc.isr.VI;

public partial class ChannelTraceSubsystemBase
{
    #region " trace parameter "

    /// <summary> Define trace parameter read writes. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>" )]
    private void DefineTraceParameterReadWrites()
    {
        this.TraceParameterReadWrites = new();
        foreach ( TraceParameters enumValue in Enum.GetValues( typeof( TraceParameters ) ) )
            this.TraceParameterReadWrites.Add( enumValue );
    }

    /// <summary> Gets or sets a dictionary of Trace Parameter parses. </summary>
    /// <value> A Dictionary of Trace Parameter parses. </value>
    public Pith.EnumReadWriteCollection TraceParameterReadWrites { get; private set; }

    /// <summary>
    /// Gets or sets the supported Trace Parameter. This is a subset of the functions supported by
    /// the instrument.
    /// </summary>
    /// <value> Options that control the supported. </value>
    public TraceParameters SupportedParameters
    {
        get;
        set
        {
            if ( !this.SupportedParameters.Equals( value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the cached source Parameter. </summary>
    /// <value>
    /// The <see cref="TraceParameters">Trace Parameter</see> or none if not set or unknown.
    /// </value>
    public TraceParameters? Parameter
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Parameter, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the Trace Parameter. </summary>
    /// <param name="value"> The  Trace Parameter. </param>
    /// <returns> The <see cref="TraceParameters">Trace Parameter</see> or none if unknown. </returns>
    public TraceParameters? ApplyParameter( TraceParameters value )
    {
        _ = this.WriteParameter( value );
        return this.QueryParameter();
    }

    /// <summary> Gets or sets the Trace Parameter query command. </summary>
    /// <value> The Trace Parameter query command, e.g., :CALC{0}:PAR{1}:DEF? </value>
    protected virtual string ParameterQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Trace Parameter. </summary>
    /// <returns> The parameter. </returns>
    public TraceParameters? QueryParameter()
    {
        this.Parameter = this.Session.Query( this.Parameter.GetValueOrDefault( TraceParameters.None ), this.TraceParameterReadWrites,
            string.Format( this.ParameterQueryCommand, this.ChannelNumber, this.TraceNumber ) );
        return this.Parameter;
    }

    /// <summary> Gets or sets the Trace Parameter command. </summary>
    /// <value> The Trace Parameter command, e.g., :CALC{0}:PAR{1}:DEF {{0}}. </value>
    protected virtual string ParameterCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Trace Parameter without reading back the value from the device. </summary>
    /// <param name="value"> The Trace Parameter. </param>
    /// <returns> The <see cref="TraceParameters">Trace Parameter</see> or none if unknown. </returns>
    public TraceParameters? WriteParameter( TraceParameters value )
    {
        this.Parameter = this.Session.Write( value, string.Format( this.ParameterCommandFormat, this.ChannelNumber, this.TraceNumber ), this.TraceParameterReadWrites );
        return this.Parameter;
    }

    #endregion
}
/// <summary> A bit-field of flags for specifying trace parameters. </summary>
[Flags]
public enum TraceParameters
{
    /// <summary> An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None = 0,

    /// <summary> An enum constant representing the absolute impedance option. </summary>
    [System.ComponentModel.Description( "Z: Absolute impedance value (Z)" )]
    AbsoluteImpedance = 1,

    /// <summary> An enum constant representing the absolute admittance option. </summary>
    [System.ComponentModel.Description( "Y: Absolute admittance (Y)" )]
    AbsoluteAdmittance = 2,

    /// <summary> An enum constant representing the series resistance option. </summary>
    [System.ComponentModel.Description( "R: Equivalent series resistance (R)" )]
    SeriesResistance = 4,

    /// <summary> An enum constant representing the series reactance option. </summary>
    [System.ComponentModel.Description( "X: Equivalent series reactance (X)" )]
    SeriesReactance = 8,

    /// <summary> An enum constant representing the parallel conductance option. </summary>
    [System.ComponentModel.Description( "G: Equivalent parallel conductance (G)" )]
    ParallelConductance = 0x10,

    /// <summary> An enum constant representing the parallel susceptance option. </summary>
    [System.ComponentModel.Description( "B: Equivalent parallel susceptance (B)" )]
    ParallelSusceptance = 0x20,

    /// <summary> An enum constant representing the series inductance option. </summary>
    [System.ComponentModel.Description( "LS: Equivalent series inductance (LS)" )]
    SeriesInductance = 0x40,

    /// <summary> An enum constant representing the parallel inductance option. </summary>
    [System.ComponentModel.Description( "LP: Equivalent parallel inductance (LP)" )]
    ParallelInductance = 0x80,

    /// <summary> An enum constant representing the series capacitance option. </summary>
    [System.ComponentModel.Description( "CS: Equivalent series capacitance (CS)" )]
    SeriesCapacitance = 0x100,

    /// <summary> An enum constant representing the parallel capacitance option. </summary>
    [System.ComponentModel.Description( "CP: Equivalent parallel capacitance (CP)" )]
    ParallelCapacitance = 0x200,

    /// <summary> An enum constant representing the series resistance 1 option. </summary>
    [System.ComponentModel.Description( "RS: Equivalent series resistance (RS)" )]
    SeriesResistance1 = 0x400,

    /// <summary> An enum constant representing the parallel resistance option. </summary>
    [System.ComponentModel.Description( "RP: Equivalent parallel resistance (RP)" )]
    ParallelResistance = 0x800,

    /// <summary> An enum constant representing the quality factor option. </summary>
    [System.ComponentModel.Description( "Q: Q value (Q)" )]
    QualityFactor = 0x1000,

    /// <summary> An enum constant representing the dissipation factor option. </summary>
    [System.ComponentModel.Description( "D: Dissipation factor (D)" )]
    DissipationFactor = 0x2000,

    /// <summary> An enum constant representing the impedance phase option. </summary>
    [System.ComponentModel.Description( "TZ: Impedance phase (TZ)" )]
    ImpedancePhase = 0x4000,

    /// <summary> An enum constant representing the absolute phase option. </summary>
    [System.ComponentModel.Description( "TY: Absolute phase (TY)" )]
    AbsolutePhase = 0x8000,

    /// <summary> An enum constant representing the oscillator voltage option. </summary>
    [System.ComponentModel.Description( "VAC: Oscillator Voltage (VAC)" )]
    OscillatorVoltage = 0x10000,

    /// <summary> An enum constant representing the oscillator current option. </summary>
    [System.ComponentModel.Description( "IAC: Oscillator Current (IAC)" )]
    OscillatorCurrent = 0x20000,

    /// <summary> An enum constant representing the bias voltage option. </summary>
    [System.ComponentModel.Description( "VDC: DC Bias Voltage (VDC)" )]
    BiasVoltage = 0x40000,

    /// <summary> An enum constant representing the bias current option. </summary>
    [System.ComponentModel.Description( "IDC: DC Bias Current (IDC)" )]
    BiasCurrent = 0x80000,

    /// <summary> An enum constant representing the complex impedance option. </summary>
    [System.ComponentModel.Description( "IMP: Complex Impedance (IMP)" )]
    ComplexImpedance = 0x100000,

    /// <summary> An enum constant representing the complex admittance option. </summary>
    [System.ComponentModel.Description( "ADM: Complex Admittance (ADM)" )]
    ComplexAdmittance = 0x200000
}
