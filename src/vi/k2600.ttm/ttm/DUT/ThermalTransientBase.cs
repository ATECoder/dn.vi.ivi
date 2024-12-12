using System.Diagnostics;
using cc.isr.Std.NumericExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Defines a measured thermal transient resistance and voltage. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para>
/// </remarks>
public abstract class ThermalTransientBase : MeasureBase, IEquatable<ThermalTransientBase>
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected ThermalTransientBase() : base()
    {
    }

    /// <summary> Clones an existing measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    protected ThermalTransientBase( ThermalTransientBase value ) : base( value )
    {
        if ( value is not null )
        {
            // Configuration
            this._allowedVoltageChange = value.AllowedVoltageChange;
            this._currentLevel = value.CurrentLevel;
            this._medianFilterSize = value.MedianFilterSize;
            this._samplingInterval = value.SamplingInterval;
            this._tracePoints = value.TracePoints;
            this._voltageLimit = value.VoltageLimit;
        }
    }

    /// <summary> Copies the configuration described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public void CopyConfiguration( ThermalTransientBase value )
    {
        base.CopyConfiguration( value );
        if ( value is not null )
        {
            this._allowedVoltageChange = value.AllowedVoltageChange;
            this._currentLevel = value.CurrentLevel;
            this._medianFilterSize = value.MedianFilterSize;
            this._samplingInterval = value.SamplingInterval;
            this._tracePoints = value.TracePoints;
            this._voltageLimit = value.VoltageLimit;
        }
    }

    #endregion

    #region " preset "

    /// <summary> Sets the known reset (default) state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ResetKnownState()
    {
        base.ResetKnownState();
        /*  Moved to preset
            this.Aperture = ( double ) Properties.Settings.Instance.TtmTraceSettings.ApertureDefault;
            this.LowLimit = ( double ) Properties.Settings.Instance.TtmTraceSettings.LowLimitDefault;
            this.HighLimit = ( double ) Properties.Settings.Instance.TtmTraceSettings.HighLimitDefault;
            this.AllowedVoltageChange = ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageChangeDefault;
            this.CurrentLevel = ( double ) Properties.Settings.Instance.TtmTraceSettings.CurrentLevelDefault;
            this.MedianFilterSize = Properties.Settings.Instance.TtmTraceSettings.MedianFilterLengthDefault;
            this.SamplingInterval = ( double ) Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalDefault;
            this.TracePoints = Properties.Settings.Instance.TtmTraceSettings.TracePointsDefault;
            this.VoltageLimit = ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageChangeDefault;
         */
    }

    /// <summary>   Sets the known preset state. </summary>
    /// <remarks>   2024-11-15. </remarks>
    public override void PresetKnownState()
    {
        base.PresetKnownState();
        this.Aperture = ( double ) Properties.Settings.Instance.TtmTraceSettings.ApertureDefault;
        this.LowLimit = ( double ) Properties.Settings.Instance.TtmTraceSettings.LowLimitDefault;
        this.HighLimit = ( double ) Properties.Settings.Instance.TtmTraceSettings.HighLimitDefault;
        this.AllowedVoltageChange = ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageChangeDefault;
        this.CurrentLevel = ( double ) Properties.Settings.Instance.TtmTraceSettings.CurrentLevelDefault;
        this.MedianFilterSize = Properties.Settings.Instance.TtmTraceSettings.MedianFilterLengthDefault;
        this.SamplingInterval = ( double ) Properties.Settings.Instance.TtmTraceSettings.SamplingIntervalDefault;
        this.TracePoints = Properties.Settings.Instance.TtmTraceSettings.TracePointsDefault;
        this.VoltageLimit = ( double ) Properties.Settings.Instance.TtmTraceSettings.VoltageChangeDefault;
    }

    #endregion

    #region " equals "

    /// <summary>
    /// Indicates whether the current <see cref="ThermalTransientBase"></see> value is equal to a
    /// specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="obj"> An object. </param>
    /// <returns>
    /// <c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the
    /// same value; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals( object obj )
    {
        return this.Equals( ( ThermalTransientBase ) obj );
    }

    /// <summary>
    /// Indicates whether the current <see cref="ThermalTransientBase"></see> value is equal to a
    /// specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="other" /> parameter and this instance are the same type
    /// and represent the same value; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals( ThermalTransientBase other )
    {
        return other is not null && this.Reading is not null
            && other.Reading is not null
            && this.Reading.Equals( other.Reading )
            && this.ConfigurationEquals( other );
    }

    /// <summary>
    /// Indicates whether the current <see cref="ThermalTransientBase"></see> configuration values
    /// are equal to a specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="ThermalTransientBase"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool ConfigurationEquals( ThermalTransientBase other )
    {
        return other is not null && base.ConfigurationEquals( other )
            && this.AllowedVoltageChange.Approximates( other.AllowedVoltageChange, 0.001d )
            && this.CurrentLevel.Approximates( other.CurrentLevel, 0.0001d )
            && this.MedianFilterSize.Equals( other.MedianFilterSize )
            && this.SamplingInterval.Approximates( other.SamplingInterval, 0.000001d )
            && this.TracePoints.Equals( other.TracePoints )
            && this.VoltageLimit.Approximates( other.VoltageLimit, 0.0001d )
            && true;
    }

    /// <summary> Returns a hash code for this instance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A hash code for this object. </returns>
    public override int GetHashCode()
    {
        return System.HashCode.Combine( this.AllowedVoltageChange.GetHashCode,
            this.CurrentLevel.GetHashCode,
            this.MedianFilterSize.GetHashCode,
            this.SamplingInterval.GetHashCode,
            this.TracePoints.GetHashCode,
            this.VoltageLimit.GetHashCode,
            base.GetHashCode() );

    }

    /// <summary> Implements the operator =. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( ThermalTransientBase left, ThermalTransientBase right )
    {
        return ReferenceEquals( left, right ) || (left is not null && left.Equals( right ));
    }

    /// <summary> Implements the not equal operator. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( ThermalTransientBase left, ThermalTransientBase right )
    {
        return !ReferenceEquals( left, right ) && (left is null || !left.Equals( right ));
    }

    /// <summary> Check throw unequal configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="other"> The thermal transient configuration to compare to this object. </param>
    public void CheckThrowUnequalConfiguration( ThermalTransientBase other )
    {
        if ( other is not null )
        {
            base.CheckThrowUnequalConfiguration( other );
            if ( !this.ConfigurationEquals( other ) )
            {
                string format = "Unequal configuring--instrument {0}={1}.NE.{2}";
                if ( !this.AllowedVoltageChange.Approximates( other.AllowedVoltageChange, 0.0001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Allowed Voltage Change", this.AllowedVoltageChange, other.AllowedVoltageChange ) );
                }
                else if ( !this.CurrentLevel.Approximates( other.CurrentLevel, 0.0001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Current Level", this.CurrentLevel, other.CurrentLevel ) );
                }
                else if ( !this.MedianFilterSize.Equals( other.MedianFilterSize ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Median Filter Size", this.MedianFilterSize, other.MedianFilterSize ) );
                }
                else if ( !this.SamplingInterval.Approximates( other.SamplingInterval, 0.000001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Sampling Interval", this.SamplingInterval, other.SamplingInterval ) );
                }
                else if ( !this.TracePoints.Equals( other.TracePoints ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Trace Points", this.TracePoints, other.TracePoints ) );
                }
                else
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed logic" );
                }
            }
        }
    }

    #endregion

    #region " configuration properties "

    private double _allowedVoltageChange;

    /// <summary> Gets or sets the maximum expected transient voltage. </summary>
    /// <value> The allowed voltage change. </value>
    public double AllowedVoltageChange
    {
        get => this._allowedVoltageChange;
        set => _ = this.SetProperty( ref this._allowedVoltageChange, value );
    }

    private double _currentLevel;

    /// <summary> Gets or sets the current level. </summary>
    /// <value> The current level. </value>
    public double CurrentLevel
    {
        get => this._currentLevel;
        set => _ = this.SetProperty( ref this._currentLevel, value );
    }

    private int _medianFilterSize;

    /// <summary> Gets or sets the cached Median Filter Size. </summary>
    /// <value> The Median Filter Size. </value>
    public int MedianFilterSize
    {
        get => this._medianFilterSize;
        set => _ = this.SetProperty( ref this._medianFilterSize, value );
    }

    /// <summary> The sampling interval. </summary>
    private double _samplingInterval;

    /// <summary> Gets or sets the sampling interval. </summary>
    /// <value> The sampling interval. </value>
    public double SamplingInterval
    {
        get => this._samplingInterval;
        set => _ = this.SetProperty( ref this._samplingInterval, value );
    }

    private double _voltageLimit;

    /// <summary> Gets or sets the cached Source Voltage Limit. </summary>
    /// <value> The Source Voltage Limit. </value>
    public virtual double VoltageLimit
    {
        get => this._voltageLimit;

        set => _ = this.SetProperty( ref this._voltageLimit, value );
    }

    /// <summary> The trace points. </summary>
    private int _tracePoints;

    /// <summary> Gets or sets the number of trace points to measure. </summary>
    /// <value> The trace points. </value>
    public int TracePoints
    {
        get => this._tracePoints;
        set => _ = this.SetProperty( ref this._tracePoints, value );
    }

    #endregion

}
