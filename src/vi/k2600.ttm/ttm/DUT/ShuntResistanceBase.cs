// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;
using cc.isr.Std.NumericExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Defines a measured shunt resistance element. </summary>
/// <remarks>
/// David, 2012-11-10, 3.1.4697 <para>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. </para><para>
/// Licensed under The MIT License. </para>
/// </remarks>
public abstract class ShuntResistanceBase : MeasureBase, IEquatable<ShuntResistanceBase>
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected ShuntResistanceBase() : base()
    {
    }

    /// <summary> Clones an existing measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    protected ShuntResistanceBase( ShuntResistanceBase value ) : base( value )
    {
        if ( value is not null )
        {
            this._currentRange = value.CurrentRange;
            this._currentLevel = value.CurrentLevel;
            this._voltageLimit = value.VoltageLimit;
        }
    }

    #endregion

    #region " preset "

    /// <summary> Restores defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ResetKnownState()
    {
        base.ResetKnownState();
        /* moved to preset
            this.Aperture = ( double ) Properties.Settings.Instance.TtmShuntSettings.ApertureDefault;
            this.CurrentRange = ( double ) Properties.Settings.Instance.TtmShuntSettings.CurrentRangeDefault;
            this.CurrentLevel = ( double ) Properties.Settings.Instance.TtmShuntSettings.CurrentLevelDefault;
            this.LowLimit = ( double ) Properties.Settings.Instance.TtmShuntSettings.LowLimitDefault;
            this.HighLimit = ( double ) Properties.Settings.Instance.TtmShuntSettings.HighLimitDefault;
            this.VoltageLimit = ( double ) Properties.Settings.Instance.TtmShuntSettings.VoltageLimitDefault;
         */
    }

    /// <summary>   Sets the known preset state. </summary>
    /// <remarks>   2024-11-15. </remarks>
    public override void PresetKnownState()
    {
        base.PresetKnownState();
        this.Aperture = ( double ) Properties.Settings.Instance.TtmShuntSettings.ApertureDefault;
        this.CurrentRange = ( double ) Properties.Settings.Instance.TtmShuntSettings.CurrentRangeDefault;
        this.CurrentLevel = ( double ) Properties.Settings.Instance.TtmShuntSettings.CurrentLevelDefault;
        this.LowLimit = ( double ) Properties.Settings.Instance.TtmShuntSettings.LowLimitDefault;
        this.HighLimit = ( double ) Properties.Settings.Instance.TtmShuntSettings.HighLimitDefault;
        this.VoltageLimit = ( double ) Properties.Settings.Instance.TtmShuntSettings.VoltageLimitDefault;
    }

    #endregion

    #region " equals "

    /// <summary> Check throw if unequal configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="other"> The resistance to compare to this object. </param>
    public void CheckThrowUnequalConfiguration( ShuntResistanceBase other )
    {
        if ( other is not null )
        {
            base.CheckThrowUnequalConfiguration( other );
            if ( !this.ConfigurationEquals( other ) )
            {
                string format = "Unequal configuring--instrument {0}={1}.NE.{2}";
                if ( !this.CurrentRange.Approximates( other.CurrentRange, 0.000001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Current Range", this.CurrentRange, other.CurrentRange ) );
                }
                else if ( !this.CurrentLevel.Approximates( other.CurrentLevel, 0.000001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Current Level", this.CurrentLevel, other.CurrentLevel ) );
                }
                else if ( !this.VoltageLimit.Approximates( other.VoltageLimit, 0.000001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Voltage Limit", this.VoltageLimit, other.VoltageLimit ) );
                }
                else
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed logic" );
                }
            }
        }
    }

    /// <summary>
    /// Indicates whether the current <see cref="ShuntResistanceBase"></see> value is equal to a
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
        return obj is not null && this.Equals( ( ShuntResistanceBase ) obj );
    }

    /// <summary>
    /// Indicates whether the current <see cref="ShuntResistanceBase"></see> value is equal to a
    /// specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="ShuntResistanceBase"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Equals( ShuntResistanceBase other )
    {
        return other is not null && base.Equals( other )
            && this.CurrentLevel.Approximates( other.CurrentLevel, 0.000001d )
            && this.CurrentRange.Approximates( other.CurrentRange, 0.000001d )
            && this.VoltageLimit.Approximates( other.VoltageLimit, 0.000001d )
            && true;
    }

    /// <summary>
    /// Indicates whether the current <see cref="ShuntResistanceBase"></see> configuration values
    /// are equal to a specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="ShuntResistanceBase"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool ConfigurationEquals( ShuntResistanceBase other )
    {
        return other is not null && base.ConfigurationEquals( other )
            && this.CurrentLevel.Approximates( other.CurrentLevel, 0.000001d )
            && this.CurrentRange.Approximates( other.CurrentRange, 0.000001d )
            && this.VoltageLimit.Approximates( other.VoltageLimit, 0.000001d )
            && true;
    }

    /// <summary> Returns a hash code for this instance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A hash code for this object. </returns>
    public override int GetHashCode()
    {
        return System.HashCode.Combine( this.CurrentRange.GetHashCode,
            this.CurrentLevel.GetHashCode,
            this.VoltageLimit.GetHashCode,
            base.GetHashCode() );
    }

    /// <summary> Implements the operator =. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( ShuntResistanceBase left, ShuntResistanceBase right )
    {
        return ReferenceEquals( left, right ) || (left is not null && left.Equals( right ));
    }

    /// <summary> Implements the not equal operator. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( ShuntResistanceBase left, ShuntResistanceBase right )
    {
        return !ReferenceEquals( left, right ) && (left is null || !left.Equals( right ));
    }

    #endregion

    #region " configuration properties "

    private double _currentRange;

    /// <summary> Gets or sets the current Range. </summary>
    /// <value> The current Range. </value>
    public double CurrentRange
    {
        get => this._currentRange;
        set => _ = this.SetProperty( ref this._currentRange, value );
    }

    private double _currentLevel;

    /// <summary> Gets or sets the current level. </summary>
    /// <value> The current level. </value>
    public double CurrentLevel
    {
        get => this._currentLevel;
        set => _ = this.SetProperty( ref this._currentLevel, value );
    }

    private double _voltageLimit;

    /// <summary> Gets or sets the cached Source Voltage Limit. </summary>
    /// <value> The Source Voltage Limit. </value>
    public double VoltageLimit
    {
        get => this._voltageLimit;

        set => _ = this.SetProperty( ref this._voltageLimit, value );
    }

    #endregion
}
