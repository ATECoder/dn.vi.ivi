using System.Diagnostics;
using cc.isr.Std.NumericExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Defines measure base element. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2009-02-02, 2.1.3320.x. </para>
/// </remarks>
public abstract partial class MeasureBase : DeviceUnderTestElementBase
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected MeasureBase() : base() { }

    /// <summary> Clones an existing measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    protected MeasureBase( MeasureBase value ) : base( value )
    {
        if ( value is not null )
        {
            // measurement
            this._displayFormat = value.DisplayFormat;
            this._measurementOutcome = value.MeasurementOutcome;
            this._measuredValue = value.MeasuredValue;
            this._measuredValueCaption = value.MeasuredValueCaption;
            this.Reading = value.Reading;
            this.FirmwareReading = value.FirmwareReading;
            this.FirmwareOutcomeReading = value.FirmwareOutcomeReading;
            this.FirmwareOkayReading = value.FirmwareOkayReading;
            this.FirmwareStatusReading = value.FirmwareStatusReading;
            this._timestamp = value.Timestamp;

            // Configuration
            this._aperture = value.Aperture;
            this._highLimit = value.HighLimit;
            this._lowLimit = value.LowLimit;
        }
    }

    /// <summary> Copies the configuration described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public virtual void CopyConfiguration( MeasureBase value )
    {
        base.CopyConfiguration( value );
        if ( value is not null )
        {
            this._aperture = value.Aperture;
            this._highLimit = value.HighLimit;
            this._lowLimit = value.LowLimit;
        }
    }

    /// <summary> Copies the measurement described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public virtual void CopyMeasurement( MeasureBase value )
    {
        if ( value is not null )
        {
            this._displayFormat = value.DisplayFormat;
            this._measurementOutcome = value.MeasurementOutcome;
            this._measuredValue = value.MeasuredValue;
            this._measuredValueCaption = value.MeasuredValueCaption;
            this._reading = value.Reading;
            this._timestamp = value.Timestamp;
        }
    }

    #endregion

    #region " preset "

    /// <summary> Sets values to their known clear execution state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineClearExecutionState()
    {
        this.Reading = string.Empty;
        this.FirmwareOkayReading = string.Empty;
        this.FirmwareStatusReading = string.Empty;
        this.FirmwareOutcomeReading = string.Empty;
        this.MeasurementOutcome = MeasurementOutcomes.None;
        this.Timestamp = DateTimeOffset.MinValue;
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Use this method to customize the reset. </remarks>
    public override void InitKnownState()
    {
    }

    /// <summary> Sets the known preset state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void PresetKnownState()
    {
        this.DisplayFormat = "0.000";
        this.VoltageDisplayFormat = "0.0000";
    }

    /// <summary> Restores defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ResetKnownState()
    {
        base.ResetKnownState();
        /* moved to preset
            this.DisplayFormat = "0.000";
            this.VoltageDisplayFormat = "0.0000";
         */
    }

    #endregion

    #region " equals "

    /// <summary>
    /// Indicates whether the current <see cref="MeasureBase"></see> value is equal to a
    /// specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The measure to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="MeasureBase"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Equals( MeasureBase other )
    {
        return other is not null && this.Reading is not null && this.Reading.Equals( other.Reading ) && this.ConfigurationEquals( other ) && true;
    }

    /// <summary>
    /// Indicates whether the current <see cref="MeasureBase"></see> configuration values
    /// are equal to a specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The measure to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="MeasureBase"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public virtual bool ConfigurationEquals( MeasureBase other )
    {
        return other is not null && base.ConfigurationEquals( other )
            && this.Aperture.Approximates( other.Aperture, 0.00001d )
            && this.HighLimit.Approximates( other.HighLimit, 0.0001d )
            && this.LowLimit.Approximates( other.LowLimit, 0.0001d )
            && true;
    }

    /// <summary>   Returns a hash code for this instance. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <returns>   A hash code for this object. </returns>
    public override int GetHashCode()
    {
        return System.HashCode.Combine( this.Aperture.GetHashCode,
            this.HighLimit.GetHashCode,
            this.LowLimit.GetHashCode,
            base.GetHashCode() );
    }

    /// <summary> Check throw if unequal configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="other"> The measure to compare to this object. </param>
    public virtual void CheckThrowUnequalConfiguration( MeasureBase other )
    {
        if ( other is not null )
        {
            if ( !this.ConfigurationEquals( other ) )
            {
                string format = "Unequal configuring--instrument {0} value of {1} is not {2}";
                if ( !this.Aperture.Approximates( other.Aperture, 0.00001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Aperture", this.Aperture, other.Aperture ) );
                }
                else if ( !this.HighLimit.Approximates( other.HighLimit, 0.00001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "High Limit", this.HighLimit, other.HighLimit ) );
                }
                else if ( !this.LowLimit.Approximates( other.LowLimit, 0.00001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Low Limit", this.LowLimit, other.LowLimit ) );
                }
                else
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed logic" );
                }
            }
        }
    }

    #endregion

    #region " display properties "

    private string? _displayFormat;

    /// <summary> Gets or sets the display format for the primary measurement. </summary>
    /// <value> The display format. </value>
    public string? DisplayFormat
    {
        get => this._displayFormat;
        set => _ = this.SetProperty( ref this._displayFormat, value );
    }

    private string? _voltageDisplayFormat;

    /// <summary> Gets or sets the display format for Voltage. </summary>
    /// <value> The display format. </value>
    public string? VoltageDisplayFormat
    {
        get => this._voltageDisplayFormat;
        set => _ = this.SetProperty( ref this._voltageDisplayFormat, value );
    }

    #endregion

    #region " parser "

    private string? _firmwareReading;

    /// <summary> Gets or sets the firmware reading. </summary>
    /// <value> The firmware outcome reading. </value>
    public string? FirmwareReading
    {
        get => this._firmwareReading;
        set => _ = this.SetProperty( ref this._firmwareReading, value );
    }

    private string? _firmwareOutcomeReading;

    /// <summary> Gets or sets the firmware outcome reading. </summary>
    /// <value> The firmware outcome reading. </value>
    public string? FirmwareOutcomeReading
    {
        get => this._firmwareOutcomeReading;
        set => _ = this.SetProperty( ref this._firmwareOutcomeReading, value );
    }

    private string? _firmwareStatusReading;

    /// <summary> Gets or sets the firmware status reading. </summary>
    /// <value> The firmware status reading. </value>
    public string? FirmwareStatusReading
    {
        get => this._firmwareStatusReading;
        set => _ = this.SetProperty( ref this._firmwareStatusReading, value );
    }

    private string? _firmwareOkayReading;

    /// <summary> Gets or sets the firmware okay reading. </summary>
    /// <value> The firmware okay reading. </value>
    public string? FirmwareOkayReading
    {
        get => this._firmwareOkayReading;
        set => _ = this.SetProperty( ref this._firmwareOkayReading, value );
    }

    /// <summary> Sets the reading and outcome. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="reading"> Specifies the reading as received from the instrument. </param>
    /// <param name="outcome"> The <see cref="MeasurementOutcomes"/>. </param>
    public virtual void ParseReading( string reading, MeasurementOutcomes outcome )
    {
        double value = 0d;
        if ( string.IsNullOrWhiteSpace( reading ) )
        {
            this.Reading = string.Empty;
            this.MeasurementOutcome = outcome != MeasurementOutcomes.None ? MeasurementOutcomes.MeasurementFailed | outcome : outcome;
        }
        else
        {
            System.Globalization.NumberStyles numberFormat = System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent;
            this.Reading = reading;
            this.MeasurementOutcome = double.TryParse( this.Reading, numberFormat, System.Globalization.CultureInfo.InvariantCulture, out value )
                ? value >= this.LowLimit && value <= this.HighLimit
                    ? MeasurementOutcomes.PartPassed
                    : outcome | MeasurementOutcomes.PartFailed
                : MeasurementOutcomes.MeasurementFailed | MeasurementOutcomes.UnexpectedReadingFormat;
        }

        this.MeasuredValue = value;
        this.Timestamp = DateTimeOffset.Now;
        this.MeasurementAvailable = true;
    }

    /// <summary> Applies the contact check outcome described by outcome. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="outcome"> The <see cref="MeasurementOutcomes"/>. </param>
    public void ApplyContactCheckOutcome( MeasurementOutcomes outcome )
    {
        this.MeasurementOutcome |= outcome;
    }

    #endregion

    #region " measurement properties "

    private bool _measurementAvailable;

    /// <summary> Gets or sets (protected) the measurement available. </summary>
    /// <value> The measurement available. </value>
    public bool MeasurementAvailable
    {
        get => this._measurementAvailable;
        set => _ = this.SetProperty( ref this._measurementAvailable, value );
    }

    private Syntax.FirmwareOutcomes? _firmwareOutcome;

    /// <summary> Gets or sets the firmware outcome. </summary>
    /// <value> The outcome. </value>
    public Syntax.FirmwareOutcomes? FirmwareOutcome
    {
        get => this._firmwareOutcome;
        set => _ = this.SetProperty( ref this._firmwareOutcome, value );
    }

    private MeasurementOutcomes? _measurementOutcome;

    /// <summary> Gets or sets the measurement outcome. </summary>
    /// <value> The outcome. </value>
    public MeasurementOutcomes? MeasurementOutcome
    {
        get => this._measurementOutcome;
        set => _ = this.SetProperty( ref this._measurementOutcome, value );
    }

    private string? _reading;

    /// <summary>
    /// Gets or sets  or sets (protected) the reading.  When set, the value is converted to the <see cref="MeasuredValue"/>.
    /// </summary>
    /// <value> The reading. </value>
    public string? Reading
    {
        get => this._reading;
        set => _ = this.SetProperty( ref this._reading, value );
    }

    private double _measuredValue;

    /// <summary> Gets or sets the measured value. </summary>
    /// <value> The measured value. </value>
    public double MeasuredValue
    {
        get => this._measuredValue;
        set
        {
            if ( this.SetProperty( ref this._measuredValue, value ) )
            {
                this.MeasuredValueCaption = this.MeasurementOutcome == MeasurementOutcomes.None || (this.MeasurementOutcome & MeasurementOutcomes.MeasurementNotMade) != 0
                    ? string.Empty
                    : string.IsNullOrWhiteSpace( this.Reading ) || (this.MeasurementOutcome & MeasurementOutcomes.MeasurementFailed) != 0
                        ? "#null#"
                        : this._measuredValue.ToString( this.DisplayFormat, System.Globalization.CultureInfo.CurrentCulture );
            }
        }
    }

    private string? _measuredValueCaption;

    /// <summary> Gets or sets (protected) the measured value display caption. </summary>
    /// <value> The measured value caption. </value>
    public string? MeasuredValueCaption
    {
        get => this._measuredValueCaption;
        protected set => _ = this.SetProperty( ref this._measuredValueCaption, value );
    }

    private DateTimeOffset _timestamp;

    /// <summary>
    /// Gets or sets (protected) the measurement time stamp. Gets set when setting the reading or
    /// the measured value.
    /// </summary>
    /// <value> The timestamp. </value>
    public DateTimeOffset Timestamp
    {
        get => this._timestamp;
        protected set => _ = this.SetProperty( ref this._timestamp, value );
    }

    /// <summary> Emulates a reading. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void EmulateReading()
    {
        this.FirmwareReading = this.GenerateRandomReading( this.LowLimit, this.HighLimit ).ToString();
        this.FirmwareOutcomeReading = "0";
    }

    #endregion

    #region " configuration properties "

    private double _aperture;

    /// <summary>
    /// Gets or sets the integration period in number of power line cycles for taking the measurement.
    /// </summary>
    /// <value> The aperture. </value>
    public double Aperture
    {
        get => this._aperture;
        set => _ = this.SetProperty( ref this._aperture, value );
    }

    private double _highLimit;

    /// <summary>
    /// Gets or sets the high limit for determining measurement pass fail condition.
    /// </summary>
    /// <value> The high limit. </value>
    public double HighLimit
    {
        get => this._highLimit;
        set => _ = this.SetProperty( ref this._highLimit, value );
    }

    private double _lowLimit;

    /// <summary>
    /// Gets or sets the low limit for determining measurement pass fail condition.
    /// </summary>
    /// <value> The low limit. </value>
    public double LowLimit
    {
        get => this._lowLimit;
        set => _ = this.SetProperty( ref this._lowLimit, value );
    }

    #endregion
}
