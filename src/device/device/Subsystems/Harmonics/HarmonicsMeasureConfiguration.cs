namespace cc.isr.VI;

/// <summary>   The harmonics measure configuration. </summary>
/// <remarks>   David, 2021-03-30. </remarks>
public class HarmonicsMeasureConfiguration
{
    /// <summary>   Gets or sets the access rights mode. </summary>
    /// <value> The access rights mode. </value>
    public int AccessRightsMode { get; set; } = 2;

    /// <summary>
    /// Gets or sets a value indicating whether the bandwidth limiting is enabled.
    /// </summary>
    /// <value> True if bandwidth limiting enabled, false if not. </value>
    public bool BandwidthLimitingEnabled { get; set; } = false;

    /// <summary>   Gets or sets the nominal resistance. </summary>
    /// <value> The nominal resistance. </value>
    public double NominalResistance { get; set; }

    /// <summary>   Gets or sets the generator power level. </summary>
    /// <value> The generator power level. </value>
    public double GeneratorPowerLevel { get; set; }

    /// <summary>   Gets or sets the generator current maximum. </summary>
    /// <value> The generator current maximum. </value>
    public double GeneratorCurrentMaximum { get; set; }

    /// <summary>   Gets the generator current. </summary>
    /// <value> The generator current. </value>
    public double GeneratorCurrent => this.GeneratorOutputLevel / this.NominalResistance;

    /// <summary>   QueryEnum if this generator current is over limit. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <returns>   True if generator current high, false if not. </returns>
    public bool IsGeneratorCurrentExceedsLimit()
    {
        return this.GeneratorCurrentMaximum < this.GeneratorCurrent;
    }

    /// <summary>   Gets or sets the generator output level. </summary>
    /// <value> The generator output level. </value>
    public double GeneratorOutputLevel => HarmonicsMeasureConfiguration.ExpectedGeneratorOutputLevel( this.GeneratorPowerLevel, this.NominalResistance );

    /// <summary>   Gets or sets the generator timer time span. </summary>
    /// <value> The generator timer time span. </value>
    public TimeSpan GeneratorTimerTimeSpan { get; set; }

    /// <summary>   Gets or sets the generator timer range. </summary>
    /// <value> The generator timer range. </value>
    public Std.Primitives.RangeI GeneratorTimerRange { get; set; } = new Std.Primitives.RangeI( 0, 1 );

    /// <summary>   Gets or sets the impedance range mode. </summary>
    /// <value> The impedance range mode. </value>
    public int ImpedanceRangeMode { get; set; }

    /// <summary>   Gets or sets the measure mode. </summary>
    /// <value> The measure mode. </value>
    public VI.HarmonicsMeasureMode MeasureMode { get; set; } = VI.HarmonicsMeasureMode.Decibel;

    /// <summary>   Gets or sets the measurement start mode. </summary>
    /// <value> The measurement start mode. </value>
    public int MeasurementStartMode { get; set; } = 4;

    /// <summary>   Gets or sets the third harmonics index high limit in decibels. </summary>
    /// <value> The third harmonics index high limit in decibels. </value>
    public double ThirdHarmonicsIndexHighLimit { get; set; } = 150;

    /// <summary>   Gets or sets the third harmonics index low limit in decibels. </summary>
    /// <value> The third harmonics index low limit in decibels. </value>
    public double ThirdHarmonicsIndexLowLimit { get; set; } = 115;

    /// <summary>
    /// Gets or sets a value indicating whether the Voltmeter Output is enabled.
    /// </summary>
    /// <value> True if Voltmeter Output enabled, false if not. </value>
    public bool VoltmeterOutputEnabled { get; set; } = true;

    /// <summary>   Gets or sets the voltmeter range; null it auto range. </summary>
    /// <value> The voltmeter range. </value>
    public double? VoltmeterRange { get; set; }

    /// <summary>   Gets or sets the voltmeter range mode. </summary>
    /// <value> The voltmeter mode. </value>
    public int VoltmeterRangeMode { get; set; }

    /// <summary>   Gets or sets a value indicating whether the using automatic range. </summary>
    /// <value> True if using automatic range, false if not. </value>
    public bool UsingAutoRange { get; set; }

    /// <summary>   Gets the voltmeter low limit. </summary>
    /// <value> The voltmeter low limit. </value>
    public double VoltmeterLowLimit => HarmonicsMeasureConfiguration.ExpectedVoltmeterLimit( this.GeneratorOutputLevel, this.ThirdHarmonicsIndexHighLimit );

    /// <summary>   Gets the voltmeter high limit. </summary>
    /// <value> The voltmeter high limit. </value>
    public double VoltmeterHighLimit => HarmonicsMeasureConfiguration.ExpectedVoltmeterLimit( this.GeneratorOutputLevel, this.ThirdHarmonicsIndexLowLimit );

    /// <summary>   Expected generator output level. </summary>
    /// <remarks>   David, 2021-05-06. </remarks>
    /// <param name="powerLevel">           The power level. </param>
    /// <param name="nominalResistance">    The nominal resistance. </param>
    /// <returns>   A double. </returns>
    public static double ExpectedGeneratorOutputLevel( double powerLevel, double nominalResistance )
    {
        double value = Math.Sqrt( powerLevel * nominalResistance );
        int decimalPlaces = value > 100 ? 2 : 3;
        return Math.Round( Math.Sqrt( powerLevel * nominalResistance ), decimalPlaces );
    }

    /// <summary>   Expected voltmeter limit. </summary>
    /// <remarks>   David, 2021-05-06. </remarks>
    /// <param name="outputLevel">              The output level. </param>
    /// <param name="thirdHarmonicsIndexLimit"> The third harmonics index limit. </param>
    /// <returns>   A double. </returns>
    public static double ExpectedVoltmeterLimit( double outputLevel, double thirdHarmonicsIndexLimit )
    {
        return outputLevel * Math.Pow( 10, -thirdHarmonicsIndexLimit / 20 ); ;
    }

    /// <summary>   Select generator timer. </summary>
    /// <remarks>   David, 2021-04-09. </remarks>
    /// <param name="nominalResistance">    The nominal resistance. </param>
    /// <param name="generatorTimer">       The generator timer. </param>
    /// <returns>   A TimeSpan. </returns>
    public static TimeSpan SelectGeneratorTimer( double nominalResistance, TimeSpan generatorTimer )
    {
        return nominalResistance >= 100
            ? generatorTimer
            : nominalResistance >= 50
                ? TimeSpan.FromMilliseconds( 50 )
                    : nominalResistance >= 25
                        ? TimeSpan.FromMilliseconds( 75 )
                        : TimeSpan.FromMilliseconds( 100 );
    }


}
