namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a SCPI Sense Voltage Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific ReSenses, Inc.<para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class MeasureResistanceSubsystemBase : MeasureSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="SenseResistanceSubsystemBase" /> class.
    /// </summary>
    /// <remarks> David, 2020-07-28. </remarks>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    /// <param name="readingAmounts">  The reading amounts. </param>
    protected MeasureResistanceSubsystemBase( StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem, readingAmounts )
    {
        this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ohm;
        this.ResistanceRangeCurrents = [];
    }

    #endregion

    #region " resistance range "

    /// <summary> The current. </summary>
    private decimal _current;

    /// <summary> Gets or sets the current for the specific range. </summary>
    /// <value> The current. </value>
    public decimal Current
    {
        get => this._current;
        set
        {
            if ( value != this.Current )
            {
                this._current = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the resistance range currents for either two- or four-wire resistance
    /// measurements.
    /// </summary>
    /// <value> The resistance range currents. </value>
    public ResistanceRangeCurrentCollection ResistanceRangeCurrents { get; private set; }

    /// <summary> The resistance range current. </summary>
    private ResistanceRangeCurrent? _resistanceRangeCurrent;

    /// <summary> Gets or sets the resistance range current. </summary>
    /// <value> The resistance range current. </value>
    public ResistanceRangeCurrent? ResistanceRangeCurrent
    {
        get => this._resistanceRangeCurrent;
        set
        {
            if ( value is not null && (this.ResistanceRangeCurrent is null || (value.ResistanceRange != this.ResistanceRangeCurrent.ResistanceRange)) )
            {
                this._resistanceRangeCurrent = value;
                this.NotifyPropertyChanged();
                this.Current = value.RangeCurrent;
                _ = this.ApplyRange( ( double ) value.ResistanceRange );
            }
        }
    }

    #endregion
}
