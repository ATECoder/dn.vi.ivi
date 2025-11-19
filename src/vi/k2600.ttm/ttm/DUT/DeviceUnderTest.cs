// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace cc.isr.VI.Tsp.K2600.Ttm;
/// <summary>
/// Defines the device under test element including measurement and configuration.
/// </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2009-02-02, 2.1.3320.x. </para>
/// </remarks>
public class DeviceUnderTest : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, ICloneable
{
    #region " construction "

    /// <summary> Primary Constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public DeviceUnderTest() : base()
    {
        // initial resistance must be first.
        this.Elements = [];

        // TODO: Bind elements and display initial property values.

        this.MeterMain = new MeterMain();
        this.Elements.Add( this.MeterMain );

        this.InitialResistance = new ColdResistance();
        this.Elements.Add( this.InitialResistance );

        this.FinalResistance = new ColdResistance();
        this.Elements.Add( this.FinalResistance );

        this.ShuntResistance = new ShuntResistance();
        this.Elements.Add( this.ShuntResistance );

        this.ThermalTransient = new ThermalTransient();
        this.Elements.Add( this.ThermalTransient );

        this.Estimator = new Estimator();
        this.Elements.Add( this.Estimator );
    }

    /// <summary> Clones an existing part. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public DeviceUnderTest( DeviceUnderTest value ) : base()
    {
        if ( value is null ) throw new ArgumentException( nameof( value ) );
        if ( value.Estimator is null ) throw new ArgumentException( $"{nameof( value )}.{nameof( value.Estimator )}" );
        if ( value.MeterMain is null ) throw new ArgumentException( $"{nameof( value )}.{nameof( value.MeterMain )}" );
        if ( value.InitialResistance is null ) throw new ArgumentException( $"{nameof( value )}.{nameof( value.InitialResistance )}" );
        if ( value.FinalResistance is null ) throw new ArgumentException( $"{nameof( value )}.{nameof( value.FinalResistance )}" );
        if ( value.ShuntResistance is null ) throw new ArgumentException( $"{nameof( value )}.{nameof( value.ShuntResistance )}" );
        if ( value.ThermalTransient is null ) throw new ArgumentException( $"{nameof( value )}.{nameof( value.ThermalTransient )}" );
        this.Elements = [];
        if ( value is not null )
        {
            this.PartNumber = value.PartNumber;
            this.OperatorId = value.OperatorId;
            this.LotId = value.LotId;
            this.SampleNumber = value.SampleNumber;
            this.SerialNumber = value.SerialNumber;

            // todo: remove this.
            this.ContactCheckEnabled = value.ContactCheckEnabled;
            this.ContactCheckThreshold = value.ContactCheckThreshold;

            // initial resistance must be first.
            this.InitialResistance = new ColdResistance( value.InitialResistance );
            this.Elements.Add( this.InitialResistance );
            this.FinalResistance = new ColdResistance( value.FinalResistance );
            this.Elements.Add( this.FinalResistance );
            this.ShuntResistance = new ShuntResistance( value.ShuntResistance );
            this.Elements.Add( this.ShuntResistance );
            this.ThermalTransient = new ThermalTransient( value.ThermalTransient );
            this.Elements.Add( this.ThermalTransient );
            this.MeterMain = new MeterMain( value.MeterMain );
            this.Estimator = new Estimator( value.Estimator );
            this._outcome = value.Outcome;
        }
    }

    /// <summary> Clones the part. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A copy of this object. </returns>
    public object Clone()
    {
        return new DeviceUnderTest( this );
    }

    /// <summary> Creates a new Device. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A Device. </returns>
    public static DeviceUnderTest Create()
    {
        DeviceUnderTest device;
        try
        {
            device = new DeviceUnderTest();
        }
        catch
        {
            throw;
        }

        return device;
    }

    /// <summary> Copies the device under test information except the serial number. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The other device under test. </param>
    public virtual void CopyInfo( DeviceUnderTest value )
    {
        if ( value is not null )
        {
            this.OperatorId = value.OperatorId;
            this.LotId = value.LotId;
            this.PartNumber = value.PartNumber;
        }
    }

    /// <summary> Copies the configuration described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The other device under test. </param>
    public virtual void CopyConfiguration( DeviceUnderTest value )
    {
        if ( value is not null && value.InitialResistance is not null && value.FinalResistance is not null && value.ThermalTransient is not null )
        {
            this._contactCheckEnabled = value.ContactCheckEnabled;
            this._contactCheckThreshold = value.ContactCheckThreshold;
            this.InitialResistance?.CopyConfiguration( value.InitialResistance );
            this.FinalResistance?.CopyConfiguration( value.FinalResistance );
            this.ThermalTransient?.CopyConfiguration( value.FinalResistance );
        }
    }

    /// <summary> Copies the measurement described by value. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The other device under test. </param>
    public virtual void CopyMeasurement( DeviceUnderTest value )
    {
        if ( value is not null && value.InitialResistance is not null && value.FinalResistance is not null && value.ThermalTransient is not null )
        {
            this.InitialResistance?.CopyMeasurement( value.InitialResistance );
            this.FinalResistance?.CopyMeasurement( value.FinalResistance );
            this.ThermalTransient?.CopyMeasurement( value.FinalResistance );
        }
    }

    #endregion

    #region " i presetable "

    /// <summary> Initializes known state. </summary>
    /// <remarks> This erases the last reading. </remarks>
    public void ClearMeasurements()
    {
        this.Elements.DefineClearExecutionState();
        this.Outcome = MeasurementOutcomes.None;
    }

    /// <summary> Initializes known state. </summary>
    /// <remarks> This erases the last reading. </remarks>
    public void ClearPartInfo()
    {
        this.LotId = string.Empty;
        this.OperatorId = string.Empty;
        this.SampleNumber = 0;
        this.SerialNumber = 0;
        this.PartNumber = string.Empty;
    }

    /// <summary> Sets values to their known clear execution state. </summary>
    /// <remarks> This erases the last reading. </remarks>
    public virtual void DefineClearExecutionState()
    {
        this.ClearPartInfo();
        this.ClearMeasurements();
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Use this method to customize the reset. </remarks>
    public virtual void InitKnownState()
    {
        this.Elements.InitKnownState();
    }

    /// <summary> Sets the known preset state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void PresetKnownState()
    {
        this.ContactCheckEnabled = Properties.Settings.Instance.TtmMeterSettings.ContactCheckEnabledDefault;
        this.ContactCheckThreshold = Properties.Settings.Instance.TtmMeterSettings.ContactCheckThresholdDefault;
        this.Elements.PresetKnownState();
    }

    /// <summary> Sets the known reset (default) state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public virtual void ResetKnownState()
    {
        /* moved to preset
         */
        this.Elements.ResetKnownState();
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
                                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField ).GetValue( this );
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

    #region " collectible "

    /// <summary> Returns a unique key based on the part number and sample number. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="partNumber">   The part number. </param>
    /// <param name="sampleNumber"> The sample number. </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string BuildUniqueKey( string? partNumber, int sampleNumber )
    {
        if ( partNumber is null ) throw new ArgumentNullException( nameof( partNumber ) );
        return string.Format( System.Globalization.CultureInfo.CurrentCulture, "{0}.{1}", partNumber, sampleNumber );
    }

    /// <summary> Gets or sets the sample number. </summary>
    /// <value> The sample number. </value>
    public int SampleNumber
    {
        get;
        set
        {
            if ( this.SetProperty( ref field, value ) )
                this.UniqueKey = BuildUniqueKey( this.PartNumber, this.SampleNumber );
        }
    }

    /// <summary> Gets or sets (protected) the unique key. </summary>
    /// <value> The unique key. </value>
    public string? UniqueKey
    {
        get;
        protected set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #region " record "

    /// <summary> Gets the data header. </summary>
    /// <value> The data header. </value>
    public static string DataHeader => "\"Sample Number\",\"Time Stamp\",\"Serial Number\",\"Initial MeasuredValue\",\"Final MeasuredValue\",\"Thermal Transient Voltage\"";

    /// <summary>   The data record format. </summary>
    /// <value> The data record format. </value>
    public string DataRecordFormat { get; private set; } = "{0},#{1}#,{2},{3:G5},{4:G5},{5:G5}";

    /// <summary> Gets the data record to save to a comma separated file. </summary>
    /// <value> The data record. </value>
    public string DataRecord
    {
        get
        {
            string? timestamp = this.ThermalTransient?.Timestamp.ToString( "G" );
            return string.Format( System.Globalization.CultureInfo.CurrentCulture, this.DataRecordFormat,
                this.SampleNumber, timestamp, this.SerialNumber, this.InitialResistance?.MeasuredValue, this.FinalResistance?.MeasuredValue, this.ThermalTransient?.MeasuredValue );
        }
    }

    #endregion

    #region " device under test info "

    /// <summary> Information equals. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="other"> The other. </param>
    /// <returns> <c>true</c> if basic info is equal excluding serial number. </returns>
    public bool InfoEquals( DeviceUnderTest other )
    {
        return other is not null && string.Equals( this.PartNumber, other.PartNumber, StringComparison.Ordinal )
            && string.Equals( this.LotId, other.LotId, StringComparison.Ordinal )
            && string.Equals( this.OperatorId, other.OperatorId, StringComparison.Ordinal )
            && this.ContactCheckEnabled.Equals( other.ContactCheckEnabled )
            && this.ContactCheckThreshold.Equals( other.ContactCheckThreshold );
    }

    /// <summary> Gets or sets the part number. </summary>
    /// <value> The part number. </value>
    public string? PartNumber
    {
        get;
        set
        {
            if ( this.SetProperty( ref field, value ) )
                this.UniqueKey = BuildUniqueKey( this.PartNumber, this.SampleNumber );
        }
    }

    /// <summary> Gets or sets or Sets the Lot ID. </summary>
    /// <value> The identifier of the lot. </value>
    public string? LotId
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets or Sets the Operator ID. </summary>
    /// <value> The identifier of the operator. </value>
    public string? OperatorId
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the part serial number. </summary>
    /// <value> The serial number. </value>
    public int SerialNumber
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #region " contact check info "

    /// <summary> True to enable, false to disable the contact check. </summary>
    private bool _contactCheckEnabled;

    /// <summary> Gets or sets the contact check enabled. </summary>
    /// <value> The contact check enabled. </value>
    public bool ContactCheckEnabled
    {
        get => this._contactCheckEnabled;
        set
        {
            if ( !value.Equals( this.ContactCheckEnabled ) )
            {
                this._contactCheckEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    private int _contactCheckThreshold;

    /// <summary> Gets or sets the contact check threshold. </summary>
    /// <value> The serial number. </value>
    public int ContactCheckThreshold
    {
        get => this._contactCheckThreshold;
        set
        {
            if ( !value.Equals( this.ContactCheckThreshold ) )
            {
                this._contactCheckThreshold = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " measurement elements "

    /// <summary> Gets or sets the elements. </summary>
    /// <value> The elements. </value>
    private DeviceUnderTestElementCollection Elements { get; set; }

    /// <summary> Handles the resistance measure property changed event. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       Source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( MeasureBase sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( MeasureBase.MeasurementAvailable ):
                {
                    if ( sender.MeasurementAvailable )
                    {
                        this.MergeOutcomes();
                    }

                    break;
                }

            default:
                break;
        }
    }

    #endregion

    #region " Estimator "

    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void Estimator_PropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        this.NotifyPropertyChanged( nameof( this.Estimator ) );
    }

    private Estimator? EstimatorInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.Estimator_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.Estimator_PropertyChanged;
        }
    }

    /// <summary>
    /// Gets or sets reference to the <see cref="Estimator">meter cold Main</see>
    /// </summary>
    /// <value> The meter Main. </value>
    public Estimator? Estimator
    {
        get => this.EstimatorInternal;
        set => this.EstimatorInternal = value;
    }

    #endregion

    #region " meter main "

    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void MeterMain_PropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        this.NotifyPropertyChanged( nameof( this.MeterMain ) );
    }

    private MeterMain? MeterMainInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.MeterMain_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.MeterMain_PropertyChanged;
        }
    }

    /// <summary>
    /// Gets or sets reference to the <see cref="MeterMain">meter cold Main</see>
    /// </summary>
    /// <value> The meter Main. </value>
    public MeterMain? MeterMain
    {
        get => this.MeterMainInternal;
        set => this.MeterMainInternal = value;
    }

    #endregion

    #region " initial resistance "

    /// <summary> Event handler. Called by _initialResistance for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void InitialResistance_PropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        try
        {
            if ( sender is MeasureBase s ) this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception handling property", $"Exception handling '{e.PropertyName}' property change. {ex}." );
        }
        finally
        {
            this.NotifyPropertyChanged( nameof( this.InitialResistance ) );
        }
    }

    private ColdResistance? InitialResistanceInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.InitialResistance_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.InitialResistance_PropertyChanged;
        }
    }

    /// <summary>
    /// Gets or sets reference to the <see cref="ColdResistance">initial cold resistance</see>
    /// </summary>
    /// <value> The initial resistance. </value>
    public ColdResistance? InitialResistance
    {
        get => this.InitialResistanceInternal;
        set => this.InitialResistanceInternal = value;
    }

    #endregion

    #region " final resistance "

    /// <summary> Event handler. Called by _finalResistance for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void FinalResistance_PropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        try
        {
            if ( sender is MeasureBase s ) this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception handling property", $"Exception handling '{e.PropertyName}' property change. {ex}." );
        }
        finally
        {
            this.NotifyPropertyChanged( nameof( this.FinalResistance ) );
        }
    }

    private ColdResistance? FinalResistanceInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
            {
                field.PropertyChanged -= this.FinalResistance_PropertyChanged;
            }

            field = value;
            if ( field is not null )
            {
                field.PropertyChanged += this.FinalResistance_PropertyChanged;
            }
        }
    }

    /// <summary>
    /// Gets or sets reference to the <see cref="ColdResistance">Final cold resistance</see>
    /// </summary>
    /// <value> The Final resistance. </value>
    public ColdResistance? FinalResistance
    {
        get => this.FinalResistanceInternal;
        set => this.FinalResistanceInternal = value;
    }

    #endregion

    #region " shunt resistance "

    /// <summary> Event handler. Called by _shuntResistance for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void ShuntResistance_PropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        this.NotifyPropertyChanged( nameof( this.ShuntResistance ) );
    }

    private ShuntResistance? ShuntResistanceInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.ShuntResistance_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.ShuntResistance_PropertyChanged;
        }
    }

    /// <summary>
    /// Gets or sets reference to the <see cref="ShuntResistance">Shunt resistance</see>
    /// </summary>
    /// <value> The shunt resistance. </value>
    public ShuntResistance? ShuntResistance
    {
        get => this.ShuntResistanceInternal;
        set => this.ShuntResistanceInternal = value;
    }

    #endregion

    #region " thermal transient "

    /// <summary> Event handler. Called by _thermalTransient for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void ThermalTransient_PropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        try
        {
            if ( sender is MeasureBase s ) this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception handling property", $"Exception handling '{e.PropertyName}' property change. {ex}." );
        }
        finally
        {
            this.NotifyPropertyChanged( nameof( this.ThermalTransient ) );
        }
    }

    private ThermalTransient? ThermalTransientInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.ThermalTransient_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.ThermalTransient_PropertyChanged;
        }
    }

    /// <summary>
    /// Gets or sets reference to the <see cref="ThermalTransient">thermal transient</see>
    /// </summary>
    /// <value> The thermal transient. </value>
    public ThermalTransient? ThermalTransient
    {
        get => this.ThermalTransientInternal;
        set => this.ThermalTransientInternal = value;
    }

    /// <summary> Validates the transient voltage limit described by details. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="details"> [out] The details. </param>
    /// <returns>
    /// <c>true</c> if the voltage limit is in rage; <c>false</c> is the limit is too low.
    /// </returns>
    public bool ValidateTransientVoltageLimit( ref string details )
    {
        if ( this.InitialResistance is null )
            throw new InvalidOperationException( "Initial MeasuredValue object is null." );

        if ( this.ThermalTransient is null )
            throw new InvalidOperationException( "Thermal Transient object is null." );

        double value = (this.InitialResistance.HighLimit * this.ThermalTransient.CurrentLevel) + this.ThermalTransient.AllowedVoltageChange;
        if ( value > this.ThermalTransient.VoltageLimit )
        {
            details = string.Format( System.Globalization.CultureInfo.CurrentCulture,
                "A Thermal transient voltage limit of {0} volts is too low;. It must be at least {1} volts, which is determined by the cold resistance high limit and thermal transient current level.", this.ThermalTransient.VoltageLimit, value );
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion

    #region " configuration "

    /// <summary>
    /// Checks if the configuration is equal to the other <see cref="DeviceUnderTest">DUT</see>.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="other"> The other. </param>
    /// <returns> <c>true</c> if configurations are the same as the other device. </returns>
    public bool ConfigurationEquals( DeviceUnderTest other )
    {
        return other is null
            ? throw new ArgumentNullException( nameof( other ) )
            : other is not null && this.ContactCheckEnabled.Equals( other.ContactCheckEnabled )
                && this.ContactCheckThreshold.Equals( other.ContactCheckThreshold )
                && this.InitialResistance is not null && other.InitialResistance is not null
                && this.InitialResistance.ConfigurationEquals( other.InitialResistance )
                && this.FinalResistance is not null && other.FinalResistance is not null
                && this.FinalResistance.ConfigurationEquals( other.FinalResistance )
                && this.ThermalTransient is not null && other.ThermalTransient is not null
                && this.ThermalTransient.ConfigurationEquals( other.ThermalTransient );
    }

    /// <summary>
    /// Checks if the thermal transient configuration is equal to the other
    /// <see cref="DeviceUnderTest">DUT</see> changed.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="other"> The other. </param>
    /// <returns> <c>true</c> if configurations are the same as the other device. </returns>
    public bool ThermalTransientConfigurationEquals( DeviceUnderTest other )
    {
        return other is null
            ? throw new ArgumentNullException( nameof( other ) )
            : other is not null
                && this.InitialResistance is not null && other.InitialResistance is not null
                && this.InitialResistance.ConfigurationEquals( other.InitialResistance )
                && this.FinalResistance is not null && other.FinalResistance is not null
                && this.FinalResistance.ConfigurationEquals( other.FinalResistance )
                && this.ThermalTransient is not null && other.ThermalTransient is not null
                && this.ThermalTransient.ConfigurationEquals( other.ThermalTransient );
    }

    /// <summary>
    /// Checks if DUT Information and configuration are equal between the two devices.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="other"> The other. </param>
    /// <returns> <c>true</c> if info and configurations are the same as the other device. </returns>
    public bool InfoConfigurationEquals( DeviceUnderTest other )
    {
        return other is null
            ? throw new ArgumentNullException( nameof( other ) )
            : this.InfoEquals( other ) && this.ConfigurationEquals( other );
    }

    #endregion

    #region " equality "

    /// <summary>
    /// Checks if the configuration is equal to the other <see cref="DeviceUnderTest">DUT</see>
    /// changed.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <exception cref="ArgumentNullException">     Thrown when one or more required arguments are
    ///                                              null. </exception>
    /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
    /// <param name="other"> The other. </param>
    /// <returns> <c>true</c> if configurations are the same as the other device. </returns>
    public bool SourceMeasureUnitEquals( DeviceUnderTest other )
    {
        if ( other is null )
        {
            throw new ArgumentNullException( nameof( other ) );
        }
        else if ( other.InitialResistance is null || other.FinalResistance is null || other.ThermalTransient is null )
        {
            throw new InvalidOperationException( "DUT elements not instantiated." );
        }
        else if ( other.InitialResistance.SourceMeasureUnit is null || other.FinalResistance.SourceMeasureUnit is null || other.ThermalTransient.SourceMeasureUnit is null )
        {
            throw new InvalidOperationException( "DUT Source Measure Elements are null." );
        }
        else if ( string.IsNullOrWhiteSpace( other.InitialResistance.SourceMeasureUnit )
            || string.IsNullOrWhiteSpace( other.FinalResistance.SourceMeasureUnit ) || string.IsNullOrWhiteSpace( other.ThermalTransient.SourceMeasureUnit ) )
        {
            throw new InvalidOperationException( string.Format( System.Globalization.CultureInfo.CurrentCulture, "A DUT Source Measure Element is empty: Initial='{0}'; Final='{1}'; Transient='{2}'.",
                other.InitialResistance.SourceMeasureUnit, other.FinalResistance.SourceMeasureUnit, other.ThermalTransient.SourceMeasureUnit ) );
        }

        return other is not null && string.Equals( this.InitialResistance!.SourceMeasureUnit, other.InitialResistance.SourceMeasureUnit, StringComparison.Ordinal )
            && string.Equals( this.FinalResistance!.SourceMeasureUnit, other.FinalResistance.SourceMeasureUnit, StringComparison.Ordinal )
            && string.Equals( this.ThermalTransient!.SourceMeasureUnit, other.ThermalTransient.SourceMeasureUnit, StringComparison.Ordinal );
    }

    /// <summary> Gets the timestamp. </summary>
    /// <value> The timestamp. </value>
    public DateTimeOffset? Timestamp => this.ThermalTransient?.Timestamp;

    /// <summary> Gets the initial resistance caption. </summary>
    /// <value> The initial resistance caption. </value>
    public string? InitialResistanceCaption => this.InitialResistanceInternal?.MeasuredValueCaption;

    /// <summary> Gets the final resistance caption. </summary>
    /// <value> The final resistance caption. </value>
    public string? FinalResistanceCaption => this.FinalResistanceInternal?.MeasuredValueCaption;

    /// <summary> Gets the thermal transient voltage caption. </summary>
    /// <value> The thermal transient voltage caption. </value>
    public string? ThermalTransientVoltageCaption => this.ThermalTransientInternal?.MeasuredValueCaption;

    #endregion

    #region " outcome "

    /// <summary>
    /// Checks if any part measurement is in. This is determined by examining all outcomes as not
    /// equal to
    /// <see cref="MeasurementOutcomes.None">None</see>
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> <c>true</c> if one or more measurement outcomes were set. </returns>
    public bool AnyMeasurementMade()
    {
        return this.InitialResistance?.MeasurementOutcome != MeasurementOutcomes.None
            || this.ThermalTransient?.MeasurementOutcome != MeasurementOutcomes.None
            || this.FinalResistance?.MeasurementOutcome != MeasurementOutcomes.None;
    }

    /// <summary> Gets or sets the sentinel indicating if Any measurements is available. </summary>
    /// <value> The measurement available. </value>
    public bool AnyMeasurementsAvailable
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Checks if all part measurements are in. This is determined by examining all outcomes as not
    /// equal to
    /// <see cref="MeasurementOutcomes.None">None</see>
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> <c>true</c> if all measurement outcomes were set. </returns>
    public bool AllMeasurementsMade()
    {
        return this.InitialResistance?.MeasurementOutcome != MeasurementOutcomes.None
            && this.ThermalTransient?.MeasurementOutcome != MeasurementOutcomes.None
            && this.FinalResistance?.MeasurementOutcome != MeasurementOutcomes.None;
    }

    /// <summary> Gets or sets the sentinel indicating if all measurements are available. </summary>
    /// <value> The measurement available. </value>
    public bool AllMeasurementsAvailable
    {
        get;
        set
        {
            field = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Merges the measurements outcomes to a Double outcome. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void MergeOutcomes()
    {
        MeasurementOutcomes outcome = MeasurementOutcomes.None;
        outcome = MergeOutcomes( outcome, this.InitialResistance?.MeasurementOutcome );
        outcome = MergeOutcomes( outcome, this.FinalResistance?.MeasurementOutcome );
        outcome = MergeOutcomes( outcome, this.ThermalTransient?.MeasurementOutcome );
        this.Outcome = outcome;
    }

    /// <summary>
    /// Combine the existing outcome with new outcome. If failed, leave failed. If hit compliance set
    /// to hit compliance.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="existingOutcome"> The existing outcome. </param>
    /// <param name="newOutcome">      The new outcome. </param>
    /// <returns> The MeasurementOutcomes. </returns>
    private static MeasurementOutcomes MergeOutcomes( MeasurementOutcomes? existingOutcome, MeasurementOutcomes? newOutcome )
    {
        if ( existingOutcome is null ) throw new ArgumentNullException( nameof( existingOutcome ) );
        if ( newOutcome is null ) throw new ArgumentNullException( nameof( newOutcome ) );

        if ( (newOutcome & MeasurementOutcomes.PartPassed) != 0 )
        {
            return existingOutcome == MeasurementOutcomes.None ? MeasurementOutcomes.PartPassed : existingOutcome.Value;
        }
        else
        {
            existingOutcome &= ~MeasurementOutcomes.PartPassed;
            return existingOutcome.Value | newOutcome.Value;
        }
    }

    /// <summary> The outcome. </summary>
    private MeasurementOutcomes _outcome;

    /// <summary> Gets or sets the measurement outcome for the part. </summary>
    /// <value> The outcome. </value>
    public MeasurementOutcomes Outcome
    {
        get => this._outcome;

        protected set
        {
            // None is used to flag the measurements were cleared.
            if ( MeasurementOutcomes.None == value || !value.Equals( this.Outcome ) )
            {
                this._outcome = value;
                this.NotifyPropertyChanged();
            }

            this.AllMeasurementsAvailable = this.AllMeasurementsMade();
            this.AnyMeasurementsAvailable = this.AnyMeasurementMade();
        }
    }

    #endregion
}
/// <summary> Collection of device under tests. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-01-06 </para>
/// </remarks>
public class DeviceUnderTestCollection : System.Collections.ObjectModel.ObservableCollection<DeviceUnderTest>
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="System.Collections.ObjectModel.ObservableCollection{DeviceUnderTest}" /> class.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public DeviceUnderTestCollection() : base() => this.Devices = new DeviceCollection();

    /// <summary> Collection of devices. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private class DeviceCollection : System.Collections.ObjectModel.KeyedCollection<string, DeviceUnderTest>
    {
        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <remarks> David, 2020-10-12. </remarks>
        /// <param name="item"> The element from which to extract the key. </param>
        /// <returns> The key for the specified element. </returns>
        protected override string GetKeyForItem( DeviceUnderTest item )
        {
            if ( item is null ) throw new ArgumentNullException( nameof( item ) );
            if ( item.UniqueKey is null ) throw new ArgumentNullException( $"{nameof( item )}.{nameof( item.UniqueKey )}" );
            return item.UniqueKey;
        }
    }

    /// <summary> Gets or sets the devices. </summary>
    /// <value> The devices. </value>
    private DeviceCollection Devices { get; set; }

    /// <summary>
    /// Raises the <see cref="System.Collections.ObjectModel.ObservableCollection{DeviceUnderTest}.CollectionChanged" />
    /// event with the provided arguments.
    /// </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="e">    Arguments of the event being raised. </param>
    protected override void OnCollectionChanged( NotifyCollectionChangedEventArgs e )
    {
        this.Devices.Clear();
        foreach ( DeviceUnderTest d in this )
            this.Devices.Add( d );
        base.OnCollectionChanged( e );
    }
}
