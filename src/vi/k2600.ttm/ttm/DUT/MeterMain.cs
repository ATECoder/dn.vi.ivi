// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;
using cc.isr.Std.NumericExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Defines a measured thermal transient resistance and voltage. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License.</para>
/// </remarks>
public partial class MeterMain : DeviceUnderTestElementBase, IEquatable<MeterMain>
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public MeterMain() : base() { }

    /// <summary> Clones an existing measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public MeterMain( MeterMain value ) : base( value )
    {
        if ( value is not null )
        {
            // Configuration
            this._contactCheckOptions = value.ContactCheckOptions;
            this._contactLimit = value.ContactLimit;
            this._legacyDriver = value.LegacyDriver;
            this._postTransientDelay = value.PostTransientDelay;
        }
    }

    /// <summary> Copies the configuration described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public void CopyConfiguration( MeterMain value )
    {
        base.CopyConfiguration( value );
        if ( value is not null )
        {
            this.ContactCheckOptions = value.ContactCheckOptions;
            this.ContactLimit = value.ContactLimit;
            this.LegacyDriver = value.LegacyDriver;
            this.PostTransientDelay = value.PostTransientDelay;
        }
    }

    #endregion

    #region " preset "

    /// <summary> Sets the known reset (default) state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ResetKnownState()
    {
        base.ResetKnownState();
        /* Moved to preset:
            this.LegacyDriver = Properties.Settings.Instance.TtmMeterSettings.LegacyDriver;
            this.ContactLimit = Properties.Settings.Instance.TtmMeterSettings.ContactCheckThresholdDefault;
            this.ContactCheckOptions = Properties.Settings.Instance.TtmMeterSettings.ContactCheckOptions;
            this.PostTransientDelay = ( double ) Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayDefault;
         */
    }

    /// <summary>   Sets the known preset state. Set to default values. </summary>
    /// <remarks>   2024-11-15. </remarks>
    public override void PresetKnownState()
    {
        base.PresetKnownState();
        this.LegacyDriver = Properties.Settings.Instance.TtmMeterSettings.LegacyDriver;
        this.ContactLimit = Properties.Settings.Instance.TtmMeterSettings.ContactCheckThresholdDefault;
        this.ContactCheckOptions = Properties.Settings.Instance.TtmMeterSettings.ContactCheckOptions;
        this.PostTransientDelay = ( double ) Properties.Settings.Instance.TtmMeterSettings.PostTransientDelayDefault;
    }

    #endregion

    #region " equals "

    /// <summary>
    /// Indicates whether the current <see cref="MeterMain"></see> value is equal to a
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
        return this.Equals( ( MeterMain ) obj );
    }

    /// <summary>
    /// Indicates whether the current <see cref="MeterMain"></see> value is equal to a
    /// specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="other" /> parameter and this instance are the same type
    /// and represent the same value; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals( MeterMain other )
    {
        return other is not null && this.ConfigurationEquals( other );
    }

    /// <summary> Check throw unequal configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="other"> The thermal transient configuration to compare to this object. </param>
    public void CheckThrowUnequalConfiguration( MeterMain other )
    {
        if ( other is not null )
        {
            if ( !this.ConfigurationEquals( other ) )
            {
                string format = "Unequal configuring--instrument {0}={1}.NE.{2}";
                if ( !this.ContactCheckOptions.Equals( other.ContactCheckOptions ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Contact check options", this.ContactCheckOptions, other.ContactCheckOptions ) );
                }
                else if ( !this.ContactLimit.Equals( other.ContactLimit ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Trace Points", this.ContactLimit, other.ContactLimit ) );
                }
                else if ( !this.LegacyDriver.Equals( other.LegacyDriver ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Median Filter Size", this.LegacyDriver, other.LegacyDriver ) );
                }
                else if ( !this.PostTransientDelay.Approximates( other.PostTransientDelay, 0.001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Post Transient Delay", this.PostTransientDelay, other.PostTransientDelay ) );
                }
                else
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed logic" );
                }
            }
        }
    }

    /// <summary>
    /// Indicates whether the current <see cref="MeterMain"></see> configuration values
    /// are equal to a specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="MeterMain"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool ConfigurationEquals( MeterMain other )
    {
        return other is not null
            && this.ContactCheckOptions.Equals( other.ContactCheckOptions )
            && this.ContactLimit.Equals( other.ContactLimit )
            && this.LegacyDriver.Equals( other.LegacyDriver )
            && this.PostTransientDelay.Approximates( other.PostTransientDelay, 0.001d )
            && base.ConfigurationEquals( other );
    }

    /// <summary> Returns a hash code for this instance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A hash code for this object. </returns>
    public override int GetHashCode()
    {
        return System.HashCode.Combine( this.ContactCheckOptions.GetHashCode,
            this.ContactLimit.GetHashCode,
            this.ContactLimit.GetHashCode,
            this.LegacyDriver.GetHashCode,
            this.PostTransientDelay.GetHashCode,
            base.GetHashCode() );
    }

    /// <summary> Implements the operator =. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( MeterMain left, MeterMain right )
    {
        return ReferenceEquals( left, right ) || (left is not null && left.Equals( right ));
    }

    /// <summary> Implements the not equal operator. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( MeterMain left, MeterMain right )
    {
        return !ReferenceEquals( left, right ) && (left is null || !left.Equals( right ));
    }

    #endregion

    #region " configuration properties "

    private int _legacyDriver;

    /// <summary> Gets or sets the cached Legacy Driver flag. </summary>
    /// <value> The Legacy driver flag.. </value>
    public int LegacyDriver
    {
        get => this._legacyDriver;
        set => _ = this.SetProperty( ref this._legacyDriver, value );
    }

    private int _contactLimit;

    /// <summary> Gets or sets the contact limit in ohms. </summary>
    /// <value> The contact limit in ohms. </value>
    public int ContactLimit
    {
        get => this._contactLimit;
        set => _ = this.SetProperty( ref this._contactLimit, value );
    }

    private cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions _contactCheckOptions;

    /// <summary> Gets or sets the contact check options. </summary>
    /// <value> The contact check options. </value>
    public cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions ContactCheckOptions
    {
        get => this._contactCheckOptions;
        set => _ = this.SetProperty( ref this._contactCheckOptions, value );
    }

    private double _postTransientDelay;

    /// <summary>
    /// Gets or sets the delay time in seconds between the end of the thermal transient and the start
    /// of the final cold resistance measurement.
    /// </summary>
    /// <value> The post transient delay. </value>
    public double PostTransientDelay
    {
        get => this._postTransientDelay;
        set => _ = this.SetProperty( ref this._postTransientDelay, value );
    }

    #endregion
}
