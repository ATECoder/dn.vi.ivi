using System.Diagnostics;
using System.Text;
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
            this.DisplayFormat = value.DisplayFormat;
            this.MeasurementOutcome = value.MeasurementOutcome;
            this.MeasuredValue = value.MeasuredValue;
            this.MeasuredValueCaption = value.MeasuredValueCaption;
            this.Reading = value.Reading;
            this.FirmwareReading = value.FirmwareReading;
            this.FirmwareOutcomeReading = value.FirmwareOutcomeReading;
            this.FirmwareOkayReading = value.FirmwareOkayReading;
            this.FirmwareStatusReading = value.FirmwareStatusReading;
            this.Timestamp = value.Timestamp;

            // Configuration
            this.Aperture = value.Aperture;
            this.HighLimit = value.HighLimit;
            this.LowLimit = value.LowLimit;
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
            this.Aperture = value.Aperture;
            this.HighLimit = value.HighLimit;
            this.LowLimit = value.LowLimit;
        }
    }

    /// <summary> Copies the measurement described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public virtual void CopyMeasurement( MeasureBase value )
    {
        if ( value is not null )
        {
            this.DisplayFormat = value.DisplayFormat;
            this.MeasurementOutcome = value.MeasurementOutcome;
            this.MeasuredValue = value.MeasuredValue;
            this.MeasuredValueCaption = value.MeasuredValueCaption;
            this.Reading = value.Reading;
            this.Timestamp = value.Timestamp;
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

    /// <summary> Gets or sets the display format for the primary measurement. </summary>
    /// <value> The display format. </value>
    public string? DisplayFormat
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the display format for Voltage. </summary>
    /// <value> The display format. </value>
    public string? VoltageDisplayFormat
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }


    #endregion

    #region " parser "

    /// <summary> Gets or sets the firmware reading. </summary>
    /// <value> The firmware outcome reading. </value>
    public string? FirmwareReading
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the firmware outcome reading. </summary>
    /// <value> The firmware outcome reading. </value>
    public string? FirmwareOutcomeReading
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the firmware status reading. </summary>
    /// <value> The firmware status reading. </value>
    public string? FirmwareStatusReading
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the firmware okay reading. </summary>
    /// <value> The firmware okay reading. </value>
    public string? FirmwareOkayReading
    {
        get;
        set => _ = this.SetProperty( ref field, value );
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

    /// <summary> Gets or sets (protected) the measurement available. </summary>
    /// <value> The measurement available. </value>
    public bool MeasurementAvailable
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the firmware outcome. </summary>
    /// <value> The outcome. </value>
    public Syntax.FirmwareOutcomes? FirmwareOutcome
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the measurement outcome. </summary>
    /// <value> The outcome. </value>
    public MeasurementOutcomes? MeasurementOutcome
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>
    /// Gets or sets  or sets (protected) the reading.  When set, the value is converted to the <see cref="MeasuredValue"/>.
    /// </summary>
    /// <value> The reading. </value>
    public string? Reading
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the measured value. </summary>
    /// <value> The measured value. </value>
    public double MeasuredValue
    {
        get;
        set
        {
            if ( this.SetProperty( ref field, value ) )
            {
                this.MeasuredValueCaption = this.MeasurementOutcome == MeasurementOutcomes.None || (this.MeasurementOutcome & MeasurementOutcomes.MeasurementNotMade) != 0
                    ? string.Empty
                    : string.IsNullOrWhiteSpace( this.Reading ) || (this.MeasurementOutcome & MeasurementOutcomes.MeasurementFailed) != 0
                        ? "#null#"
                        : this.MeasuredValue.ToString( this.DisplayFormat, System.Globalization.CultureInfo.CurrentCulture );
            }
        }
    }

    /// <summary> Gets or sets (protected) the measured value display caption. </summary>
    /// <value> The measured value caption. </value>
    public string? MeasuredValueCaption
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>
    /// Gets or sets (protected) the measurement time stamp. Gets set when setting the reading or
    /// the measured value.
    /// </summary>
    /// <value> The timestamp. </value>
    public DateTimeOffset Timestamp
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
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

    /// <summary>
    /// Gets or sets the integration period in number of power line cycles for taking the measurement.
    /// </summary>
    /// <value> The aperture. </value>
    public double Aperture
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>
    /// Gets or sets the high limit for determining measurement pass fail condition.
    /// </summary>
    /// <value> The high limit. </value>
    public double HighLimit
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>
    /// Gets or sets the low limit for determining measurement pass fail condition.
    /// </summary>
    /// <value> The low limit. </value>
    public double LowLimit
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #region " report " 

    /// <summary>   Builds measured values. </summary>
    /// <remarks>   2026-03-30. </remarks>
    /// <param name="prefix">  (Optional) [CRLF] The prefix. </param>
    /// <returns>   A string. </returns>
    public string BuildMeasuredValues( string prefix = "\r\n" )
    {
        StringBuilder sb = new();
        _ = sb.Append( $"{prefix}Okay Outcome: {this.FirmwareOkayReading}" );
        _ = sb.Append( $"{prefix}      Status: {this.FirmwareStatusReading}" );
        _ = sb.Append( $"{prefix}     Outcome: {this.FirmwareOutcomeReading}" );
        _ = sb.Append( $"{prefix}     Reading: {this.FirmwareReading}" );
        _ = sb.Append( $"{prefix}      Amount: {this.MeasuredValue}" );
        return sb.ToString();
    }

    #endregion

}
