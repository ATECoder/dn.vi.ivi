using System.Diagnostics;
using cc.isr.Std.TimeSpanExtensions;

namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a Measure Voltage Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class MeasureVoltageSubsystemBase : MeasureSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasureVoltageSubsystemBase" /> class.
    /// </summary>
    /// <remarks> David, 2020-08-12. </remarks>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    /// <param name="readingAmounts">  The reading amounts. </param>
    protected MeasureVoltageSubsystemBase( StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem, readingAmounts ) => this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Volt;

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.Level = 0;
    }

    #endregion

    #region " measure voltage "

    /// <summary> Waits for the voltage to exceed a minimum voltage level. </summary>
    /// <param name="limen">   The threshold level. </param>
    /// <param name="timeout"> The timeout. </param>
    /// <returns>
    /// <c>true</c> if level was reached before the <paramref name="timeout">timeout</paramref>
    /// expired; <c>false</c> otherwise.
    /// </returns>
    public bool AwaitMinimumLevel( double limen, TimeSpan timeout )
    {
        Stopwatch sw = Stopwatch.StartNew();
        do
        {
            _ = TimeSpan.FromMilliseconds( 1 ).AsyncWait();
            _ = this.MeasureReadingAmounts();
        }
        while ( limen > this.Level && sw.Elapsed <= timeout );
        return this.Level.HasValue && limen <= this.Level.Value;
    }

    /// <summary> Waits for the voltage to attain a level. </summary>
    /// <param name="targetLevel"> The target level. </param>
    /// <param name="delta">       The delta. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns>
    /// <c>true</c> if level was reached before the <paramref name="timeout">timeout</paramref>
    /// expired; <c>false</c> otherwise.
    /// </returns>
    public bool AwaitLevel( double targetLevel, double delta, TimeSpan timeout )
    {
        Stopwatch sw = Stopwatch.StartNew();
        bool hasValue;
        do
        {
            _ = TimeSpan.FromMilliseconds( 1 ).AsyncWait();
            _ = this.MeasureReadingAmounts();
            hasValue = this.Level.HasValue && Math.Abs( targetLevel - this.Level.Value ) <= delta;
        }
        while ( !hasValue && sw.Elapsed <= timeout );
        return hasValue;
    }

    /// <summary> Gets or sets the cached voltage level. </summary>
    /// <value> The voltage. </value>
    public double? Level
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Level, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion
}
