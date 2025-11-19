using System.ComponentModel;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Defines a measured cold resistance element. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2009-02-02, 2.1.3320.x. </para>
/// </remarks>
public abstract partial class DeviceUnderTestElementBase : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IEquatable<DeviceUnderTestElementBase>
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected DeviceUnderTestElementBase() : base() => this._randomNumberGenerator = new Random( DateTimeOffset.Now.Second );

    /// <summary> Clones an existing measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    protected DeviceUnderTestElementBase( DeviceUnderTestElementBase value ) : this()
    {
        if ( value is not null )
        {
            this._sourceMeasureUnit = value.SourceMeasureUnit;
        }
    }

    /// <summary> Copies the configuration described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public virtual void CopyConfiguration( DeviceUnderTestElementBase value )
    {
        if ( value is not null )
        {
            this.SourceMeasureUnit = value.SourceMeasureUnit;
        }
    }

    #endregion

    #region " preset "

    /// <summary> Sets values to their known clear execution state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void DefineClearExecutionState()
    {
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Use this method to customize the reset. </remarks>
    public virtual void InitKnownState()
    {
    }

    /// <summary> Sets the known preset state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void PresetKnownState()
    {
        this.SourceMeasureUnit = Properties.Settings.Instance.TtmMeterSettings.SourceMeasureUnitDefault;
    }

    /// <summary> Restores defaults. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void ResetKnownState()
    {
        /* moved top preset
            this.SourceMeasureUnit = Properties.Settings.Instance.TtmMeterSettings.SourceMeasureUnitDefault;
         */
    }

    #endregion

    #region " notify property change implementation "

    /// <summary>   Notifies a property changed. </summary>
    /// <remarks>   David, 2021-02-01. </remarks>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected void NotifyPropertyChanged( [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
    {
        this.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
    }

    /// <summary>   Removes the property changed event handlers. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    protected void RemovePropertyChangedEventHandlers()
    {
        MulticastDelegate? event_delegate = ( MulticastDelegate ) this.GetType().GetField( nameof( this.PropertyChanged ),
                                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                                        | System.Reflection.BindingFlags.GetField ).GetValue( this );
        Delegate[]? delegates = event_delegate.GetInvocationList();
        if ( delegates is not null )
        {
            foreach ( Delegate? item in delegates )
            {
                this.PropertyChanged -= ( PropertyChangedEventHandler ) item;
            }
        }
    }

    #endregion

    #region " configuration properties "

    private string _sourceMeasureUnit = Syntax.ThermalTransient.DefaultSourceMeterName;

    /// <summary> Gets or sets the cached Source Measure Unit. </summary>
    /// <value> The Source Measure Unit, e.g., 'smua' or 'smub'. </value>
    public virtual string SourceMeasureUnit
    {
        get => this._sourceMeasureUnit;
        set => _ = this.SetProperty( ref this._sourceMeasureUnit, value );
    }

    #endregion

    #region " random "

    /// <summary> The random number generator. </summary>
    private readonly Random _randomNumberGenerator;

    /// <summary> Generates a random reading. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> The random reading. </returns>
    public double GenerateRandomReading( double lowLimit, double highLimit, double precision = 0.001d )
    {
        if ( 0 >= precision ) throw new ArgumentException( $"{nameof( precision )} must be position." );
        double k = 1 / precision;
        return this._randomNumberGenerator.Next( ( int ) Math.Round( k * lowLimit ), ( int ) Math.Round( k * highLimit ) ) / k;
    }

    #endregion

    #region " equals "

    /// <summary>
    /// Indicates whether the current <see cref="DeviceUnderTestElementBase"></see> value is equal to a
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
        return this.Equals( ( DeviceUnderTestElementBase ) obj );
    }

    /// <summary>
    /// Indicates whether the current <see cref="DeviceUnderTestElementBase"></see> value is equal to a
    /// specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="other" /> parameter and this instance are the same type
    /// and represent the same value; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals( DeviceUnderTestElementBase other )
    {
        return other is not null && this.ConfigurationEquals( other );
    }

    /// <summary> Check throw unequal configuration. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    /// <param name="other"> The thermal transient configuration to compare to this object. </param>
    public void CheckThrowUnequalConfiguration( DeviceUnderTestElementBase other )
    {
        if ( other is not null )
        {
            if ( !this.ConfigurationEquals( other ) )
            {
                string format = "Unequal configuring--instrument {0}={1}.NE.{2}";
                if ( !this.SourceMeasureUnit.Equals( other.SourceMeasureUnit ) )
                {
                    throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, format, "Source measure unit", this.SourceMeasureUnit, other.SourceMeasureUnit ) );
                }
                else
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed logic" );
                }
            }
        }
    }

    /// <summary>
    /// Indicates whether the current <see cref="DeviceUnderTestElementBase"></see> configuration values
    /// are equal to a specified object.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The cold resistance to compare to this object. </param>
    /// <returns>
    /// <c>true</c> if the other parameter is equal to the current
    /// <see cref="DeviceUnderTestElementBase"></see> value;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool ConfigurationEquals( DeviceUnderTestElementBase other )
    {
        return other is not null
            && string.Equals( this.SourceMeasureUnit, other.SourceMeasureUnit, StringComparison.Ordinal )
            && true;
    }

    /// <summary> Returns a hash code for this instance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A hash code for this object. </returns>
    public override int GetHashCode()
    {
        return this.SourceMeasureUnit.GetHashCode();
    }

    /// <summary> Implements the operator =. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator ==( DeviceUnderTestElementBase left, DeviceUnderTestElementBase right )
    {
        return ReferenceEquals( left, right ) || (left is not null && left.Equals( right ));
    }

    /// <summary> Implements the not equal operator. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="left">  The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operation. </returns>
    public static bool operator !=( DeviceUnderTestElementBase left, DeviceUnderTestElementBase right )
    {
        return !ReferenceEquals( left, right ) && (left is null || !left.Equals( right ));
    }

    #endregion
}
