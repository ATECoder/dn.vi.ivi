using System;
using System.ComponentModel;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> Thermal Transient Model header. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-02-25 </para>
/// </remarks>
public partial class ThermalTransientHeader : cc.isr.WinControls.ModelViewBase
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   David, 2021-09-04. </remarks>
    public ThermalTransientHeader() => this.InitializeComponent();

    #region " dut "

    private DeviceUnderTest? DeviceUnderTestInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.DeviceUnderTest_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.DeviceUnderTest_PropertyChanged;
        }
    }

    /// <summary> Gets or sets the device under test. </summary>
    /// <value> The device under test. </value>
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public DeviceUnderTest? DeviceUnderTest
    {
        get => this.DeviceUnderTestInternal;
        set
        {
            this.DeviceUnderTestInternal = value;
            this.ThermalTransientInternal = value?.ThermalTransient;
        }
    }

    /// <summary> Releases the resources. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void ReleaseResources()
    {
        this.ThermalTransientInternal = null;
        this.DeviceUnderTestInternal = null;
    }

    /// <summary> Executes the device under test property changed action. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( DeviceUnderTest sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( Ttm.DeviceUnderTest.Outcome ):
                {
                    if ( sender.Outcome == MeasurementOutcomes.None )
                    {
                        this.Clear();
                    }

                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _deviceUnderTest for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void DeviceUnderTest_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.DeviceUnderTest_PropertyChanged ), [sender, e] );
            else if ( sender is DeviceUnderTest s )
                this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception handling property", $"Exception handling '{e.PropertyName}' property change. {ex}." );
        }
    }

    #endregion

    #region " display value "

    /// <summary> Clears this object to its blank/initial state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public void Clear()
    {
        this.InfoProvider?.Clear();
        this._asymptoteTextBox.Text = string.Empty;
        this._estimatedVoltageTextBox.Text = string.Empty;
        this._iterationsCountTextBox.Text = string.Empty;
        this._correlationCoefficientTextBox.Text = string.Empty;
        this._standardErrorTextBox.Text = string.Empty;
        this._timeConstantTextBox.Text = string.Empty;
        this._outcomeTextBox.Text = string.Empty;
    }

    #endregion

    #region " part: thermal transient "


    /// <summary> The Part Thermal Transient. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>" )]
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

    /// <summary> Executes the device under test property changed action. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( ThermalTransient sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( ThermalTransient.TimeConstant ):
                {
                    this._timeConstantTextBox.Text = sender.TimeConstantCaption;
                    break;
                }

            case nameof( ThermalTransient.Asymptote ):
                {
                    this._asymptoteTextBox.Text = sender.AsymptoteCaption;
                    break;
                }

            case nameof( ThermalTransient.EstimatedVoltage ):
                {
                    this._estimatedVoltageTextBox.Text = sender.EstimatedVoltageCaption;
                    break;
                }

            case nameof( ThermalTransient.CorrelationCoefficient ):
                {
                    this._correlationCoefficientTextBox.Text = sender.CorrelationCoefficientCaption;
                    break;
                }

            case nameof( ThermalTransient.StandardError ):
                {
                    this._standardErrorTextBox.Text = sender.StandardErrorCaption;
                    break;
                }

            case nameof( ThermalTransient.Iterations ):
                {
                    this._iterationsCountTextBox.Text = sender.IterationsCaption;
                    break;
                }

            case nameof( ThermalTransient.OptimizationOutcome ):
                {
                    this._outcomeTextBox.Text = sender.OptimizationOutcomeCaption;
                    this.ToolTip?.SetToolTip( this._outcomeTextBox, sender.OptimizationOutcomeDescription );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _thermalTransient for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void ThermalTransient_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.ThermalTransient_PropertyChanged ), [sender, e] );
            else if ( sender is ThermalTransient s )
                this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception handling property", $"Exception handling '{e.PropertyName}' property change. {ex}." );
        }
    }

    #endregion
}
