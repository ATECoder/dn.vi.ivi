using System.Diagnostics;
using cc.isr.Std.NumericExtensions;
using cc.isr.VI.Tsp.K2600.Ttm.Syntax;
namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Defines a measured cold resistance element. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2009-02-02, 2.1.3320.x. </para>
/// </remarks>
public abstract class ColdResistanceBase : MeasureBase, IEquatable<ColdResistanceBase>
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected ColdResistanceBase() : base()
    {
    }

    /// <summary> Clones an existing measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    protected ColdResistanceBase( ColdResistanceBase value ) : base( value )
    {
        if ( value is not null )
        {
            this.CurrentLevel = value.CurrentLevel;
            this.CurrentLimit = value.CurrentLimit;
            this.VoltageLevel = value.VoltageLevel;
            this.VoltageLimit = value.VoltageLimit;
            this.SourceOutputOption = value.SourceOutputOption;
            this.FailStatus = value.FailStatus;
        }
    }

    /// <summary> Copies the configuration described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public void CopyConfiguration( ColdResistanceBase value )
    {
        base.CopyConfiguration( value );
        if ( value is not null )
        {
            this.CurrentLevel = value.CurrentLevel;
            this.CurrentLimit = value.CurrentLimit;
            this.VoltageLevel = value.VoltageLevel;
            this.VoltageLimit = value.VoltageLimit;
            this.SourceOutputOption = value.SourceOutputOption;
            this.FailStatus = value.FailStatus;
        }
    }

    #endregion

    #region " preset "

    /// <summary> Restores defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void ResetKnownState()
    {
        base.ResetKnownState();
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
        /* Moved to Preset
            this.Aperture = Properties.DriverSettings.Instance.ColdResistanceDefaults.Aperture;
            this.LowLimit = Properties.DriverSettings.Instance.ColdResistanceDefaults.LowLimit;
            this.HighLimit = Properties.DriverSettings.Instance.ColdResistanceDefaults.HighLimit;
            this.CurrentLevel = Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentLevel;
            this.CurrentLimit = Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentLimit;
            this.VoltageLimit = Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageLimit;
            this.VoltageLevel = Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageLevel;
            this.FailStatus = Properties.DriverSettings.Instance.ColdResistanceDefaults.FailStatus;
            this.SourceOutputOption = Properties.DriverSettings.Instance.ColdResistanceDefaults.SourceOutput;
         */
    }

    /// <summary>   Sets the known preset state. Sets configuration the their defaults. </summary>
    /// <remarks>   2024-11-15. </remarks>
    public override void PresetKnownState()
    {
        base.PresetKnownState();
        this.SourceOutputOption = Properties.DriverSettings.Instance.ColdResistanceDefaults.SourceOutput;
        this.Aperture = Properties.DriverSettings.Instance.ColdResistanceDefaults.Aperture;
        this.LowLimit = Properties.DriverSettings.Instance.ColdResistanceDefaults.LowLimit;
        this.HighLimit = Properties.DriverSettings.Instance.ColdResistanceDefaults.HighLimit;
        this.CurrentLevel = Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentLevel;
        this.CurrentLimit = Properties.DriverSettings.Instance.ColdResistanceDefaults.CurrentLimit;
        this.VoltageLimit = Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageLimit;
        this.VoltageLevel = Properties.DriverSettings.Instance.ColdResistanceDefaults.VoltageLevel;
        this.FailStatus = Properties.DriverSettings.Instance.ColdResistanceDefaults.FailStatus;
        cc.isr.VI.Pith.SessionBase.DoEventsAction?.Invoke();
    }

    #endregion

    #region " equals "

    /// <summary>
    /// Indicates whether the current <see cref="ColdResistanceBase"></see> value is equal to a
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
        return ReferenceEquals( this, obj ) || (obj is not null && this.Equals( ( ColdResistanceBase ) obj ));
    }

    /// <summary>
    /// Indicates whether the current <see cref="ColdResistanceBase"></see> value is equal to a
    /// specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="ColdResistanceBase"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool Equals( ColdResistanceBase other )
    {
        return other is not null && this.Reading is not null && this.Reading.Equals( other.Reading )
            && this.ConfigurationEquals( other ) && true;
    }

    /// <summary> Returns a hash code for this instance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A hash code for this object. </returns>
    public override int GetHashCode()
    {
        return System.HashCode.Combine( this.CurrentLevel.GetHashCode,
            this.CurrentLimit.GetHashCode,
            this.VoltageLevel.GetHashCode,
            this.VoltageLimit.GetHashCode,
            this.SourceOutputOption.GetHashCode,
            this.FailStatus.GetHashCode,
            base.GetHashCode() );
    }

    /// <summary>   Configuration equals. </summary>
    /// <remarks>   2024-11-05. </remarks>
    /// <param name="other">    The cold resistance to compare to this object. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool ConfigurationEquals( ColdResistanceBase other )
    {
        return other is not null && base.ConfigurationEquals( other )
            && this.CurrentLevel.Approximates( other.CurrentLevel, 0.0001d )
            && this.CurrentLimit.Approximates( other.CurrentLevel, 0.0001d )
            && this.FailStatus.Equals( other.FailStatus )
            && this.SourceOutputOption.Equals( other.SourceOutputOption )
            && this.VoltageLevel.Approximates( other.VoltageLevel, 0.0001d )
            && this.VoltageLimit.Approximates( other.VoltageLimit, 0.0001d )
            && true;
    }

    /// <summary> Implements the operator =. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( ColdResistanceBase left, ColdResistanceBase right )
    {
        return ReferenceEquals( left, right ) || (left is not null && left.Equals( right ));
    }

    /// <summary> Implements the not equal operator. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( ColdResistanceBase left, ColdResistanceBase right )
    {
        return !ReferenceEquals( left, right ) && (left is null || !left.Equals( right ));
    }

    /// <summary> Check throw unequal configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="other"> The thermal transient configuration to compare to this object. </param>
    public void CheckThrowUnequalConfiguration( ColdResistanceBase other )
    {
        if ( other is not null )
        {
            base.CheckThrowUnequalConfiguration( other );
            if ( !this.ConfigurationEquals( other ) )
            {
                string format = "Unequal configuring--instrument {0}={1}.NE.{2}";
                if ( !this.CurrentLevel.Approximates( other.CurrentLevel, 0.0001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Current Level", this.CurrentLevel, other.CurrentLevel ) );
                }
                else if ( !this.CurrentLimit.Approximates( other.CurrentLimit, 0.0001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Current Limit", this.CurrentLimit, other.CurrentLimit ) );
                }
                else if ( !this.VoltageLevel.Approximates( other.VoltageLevel, 0.0001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Voltage Level", this.VoltageLevel, other.VoltageLevel ) );
                }
                else if ( !this.VoltageLimit.Approximates( other.VoltageLimit, 0.0001d ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Voltage Limit", this.VoltageLimit, other.VoltageLimit ) );
                }
                else if ( !this.FailStatus.Equals( other.FailStatus ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Fail StatusReading", this.FailStatus, other.FailStatus ) );
                }
                else if ( !this.SourceOutputOption.Equals( other.SourceOutputOption ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Source output option", this.SourceOutputOption, other.SourceOutputOption ) );
                }
                else
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed logic" );
                }
            }
        }
    }

    #endregion

    #region " configuration "

    /// <summary> Gets or sets the current level. </summary>
    /// <value> The current level. </value>
    public double CurrentLevel
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the current Limit. </summary>
    /// <value> The current Limit. </value>
    public double CurrentLimit
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the cached Source Voltage Level. </summary>
    /// <value> The Source Voltage Level. </value>
    public double VoltageLevel
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the cached Source Voltage Limit. </summary>
    /// <value> The Source Voltage Limit. </value>
    public double VoltageLimit
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>   Gets or sets source output option. </summary>
    /// <value> The source output option. </value>
    public SourceOutputOption SourceOutputOption
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>   Gets or sets the fail status. </summary>
    /// <value> The fail status. </value>
    public int FailStatus
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    #endregion
}
