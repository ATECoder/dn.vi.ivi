using System.Diagnostics;
using cc.isr.Std.NumericExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Defines a measured thermal transient resistance and voltage. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para>
/// </remarks>
public class Estimator : DeviceUnderTestElementBase, IEquatable<Estimator>
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public Estimator() : base() { }

    /// <summary> Clones an existing measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public Estimator( Estimator value ) : base( value )
    {
        if ( value is not null )
        {
            // Configuration
            this._thermalCoefficient = value.ThermalCoefficient;
        }
    }

    /// <summary> Copies the configuration described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public void CopyConfiguration( Estimator value )
    {
        base.CopyConfiguration( value );
        if ( value is not null )
        {
            this._thermalCoefficient = value.ThermalCoefficient;
        }
    }

    #endregion

    #region " preset "

    /// <summary> Sets the known reset (default) state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ResetKnownState()
    {
        base.ResetKnownState();
        /* moved to preset
            this.ThermalCoefficient = ( double ) Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficientDefault;
         */
    }

    /// <summary>   Sets the known preset state. </summary>
    /// <remarks>   2024-11-15. </remarks>
    public override void PresetKnownState()
    {
        base.PresetKnownState();
        this.ThermalCoefficient = ( double ) Properties.Settings.Instance.TtmEstimatorSettings.ThermalCoefficientDefault;
    }

    #endregion

    #region " equals "

    /// <summary>
    /// Indicates whether the current <see cref="Estimator"></see> value is equal to a
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
        return this.Equals( ( Estimator ) obj );
    }

    /// <summary>
    /// Indicates whether the current <see cref="Estimator"></see> value is equal to a
    /// specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="other" /> parameter and this instance are the same type
    /// and represent the same value; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals( Estimator other )
    {
        return other is not null && this.ConfigurationEquals( other );
    }

    /// <summary> Check throw unequal configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="other"> The thermal transient configuration to compare to this object. </param>
    public void CheckThrowUnequalConfiguration( Estimator other )
    {
        if ( other is not null )
        {
            if ( !this.ConfigurationEquals( other ) )
            {
                string format = "Unequal configuring--instrument {0}={1}.NE.{2}";
                if ( !this.ThermalCoefficient.Approximates( other.ThermalCoefficient, 0.00001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Post Transient Delay", this.ThermalCoefficient, other.ThermalCoefficient ) );
                }
                else
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed logic" );
                }
            }
        }
    }

    /// <summary>
    /// Indicates whether the current <see cref="Estimator"></see> configuration values
    /// are equal to a specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="Estimator"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool ConfigurationEquals( Estimator other )
    {
        return other is not null
            && this.ThermalCoefficient.Approximates( other.ThermalCoefficient, 0.000001d )
            && base.ConfigurationEquals( other );
    }

    /// <summary> Returns a hash code for this instance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A hash code for this object. </returns>
    public override int GetHashCode()
    {
        return System.HashCode.Combine( this.ThermalCoefficient.GetHashCode,
            base.GetHashCode() );
    }

    /// <summary> Implements the operator =. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( Estimator left, Estimator right )
    {
        return ReferenceEquals( left, right ) || (left is not null && left.Equals( right ));
    }

    /// <summary> Implements the not equal operator. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( Estimator left, Estimator right )
    {
        return !ReferenceEquals( left, right ) && (left is null || !left.Equals( right ));
    }

    #endregion

    #region " configuration properties "

    private double? _thermalCoefficient;

    /// <summary> Gets or sets the cached Thermal Coefficient. </summary>
    /// <value> The Thermal Coefficient. </value>
    public double? ThermalCoefficient
    {
        get => this._thermalCoefficient;
        set => _ = this.SetProperty( ref this._thermalCoefficient, value );
    }

    #endregion
}
