using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> Measurements header. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-02-25 </para>
/// </remarks>
public partial class MeasurementsHeader : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public MeasurementsHeader() : base()
    {
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this._outcomeTextBox.Text = string.Empty;
    }

    /// <summary> Releases the resources. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void ReleaseResources()
    {
        this.InitialResistanceInternal = null;
        this.FinalResistanceInternal = null;
        this.ThermalTransientInternal = null;
        this.DeviceUnderTestInternal = null;
    }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks>   David, 2021-09-08. </remarks>
    /// <param name="disposing">    true to release both managed and unmanaged resources; false to
    ///                             release only unmanaged resources. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( disposing )
            {
                this.ReleaseResources();
                if ( disposing )
                {
                    this.components?.Dispose();
                }
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " dut "

#if NET9_0_OR_GREATER

#endif

    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
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
            if ( value is null )
            {
                this.InitialResistanceInternal = null;
                this.FinalResistanceInternal = null;
                this.ThermalTransientInternal = null;
            }
            else
            {
                this.InitialResistanceInternal = value.InitialResistance;
                this.FinalResistanceInternal = value.FinalResistance;
                this.ThermalTransientInternal = value.ThermalTransient;
            }
        }
    }

    /// <summary> Handles the device under test property changed event. </summary>
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
                    this.OutcomeSetter( sender.Outcome );
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

#if NET9_0_OR_GREATER

#endif

    /// <summary> Gets or sets a message describing the measurement. </summary>
    /// <value> A message describing the measurement. </value>
    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
    [Browsable( false )]
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public string? MeasurementMessage
    {
        get;
        set
        {
            if ( this.SetProperty( ref field, value ) )
                this._outcomeTextBox.Text = value;
        }
    }

    /// <summary> Gets or sets the test <see cref="MeasurementOutcomes">outcome</see>. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public void OutcomeSetter( MeasurementOutcomes value )
    {
        if ( this.InvokeRequired )
        {
            _ = this.Invoke( new Action<MeasurementOutcomes>( this.OutcomeSetter ), [value] );
            return;
        }

        if ( value == MeasurementOutcomes.None )
        {
            this.MeasurementMessage = string.Empty;
            this._outcomePictureBox.Visible = false;
        }
        else if ( (value & MeasurementOutcomes.MeasurementFailed) == 0 )
            this.MeasurementMessage = "OKAY";
        else if ( (value & MeasurementOutcomes.FailedContactCheck) != 0 )
            this.MeasurementMessage = "CONTACTS";
        else if ( (value & MeasurementOutcomes.HitCompliance) != 0 )
            this.MeasurementMessage = "COMPLIANCE";
        else if ( (value & MeasurementOutcomes.UnexpectedReadingFormat) != 0 )
            this.MeasurementMessage = "READING ?";
        else if ( (value & MeasurementOutcomes.UnexpectedOutcomeFormat) != 0 )
            this.MeasurementMessage = "OUTCOME ?";
        else if ( (value & MeasurementOutcomes.UnspecifiedFirmwareOutcome) != 0 )
            this.MeasurementMessage = "DEVICE";
        else if ( (value & MeasurementOutcomes.UnspecifiedProgramFailure) != 0 )
            this.MeasurementMessage = "PROGRAM";
        else if ( (value & MeasurementOutcomes.MeasurementNotMade) != 0 )
            this.MeasurementMessage = "PARTIAL";
        else
        {
            this.MeasurementMessage = "FAILED";
            if ( (value & MeasurementOutcomes.UnknownOutcome) != 0 )
                Debug.Assert( !Debugger.IsAttached, "Unknown outcome" );
        }

        if ( (value & MeasurementOutcomes.PartPassed) != 0 )
        {
            this._outcomePictureBox.Visible = true;
            this._outcomePictureBox.Image = GetResourceImage( nameof( Ttm.Controls.Properties.Resources.Good ), this._outcomePictureBox.Image );
        }
        else if ( (value & MeasurementOutcomes.PartFailed) != 0 )
        {
            this._outcomePictureBox.Visible = true;
            this._outcomePictureBox.Image = GetResourceImage( nameof( Ttm.Controls.Properties.Resources.Bad ), this._outcomePictureBox.Image );
        }
    }

    /// <summary>   Gets resource image. </summary>
    /// <remarks>   2024-11-13. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="defaultImage"> The default image. </param>
    /// <returns>   The resource image. </returns>
    public static Image? GetResourceImage( string resourceName, Image? defaultImage )
    {
        object? image = Ttm.Controls.Properties.Resources.ResourceManager.GetObject( resourceName, System.Globalization.CultureInfo.CurrentCulture );
        return image is null ? defaultImage : ( System.Drawing.Bitmap ) image;
    }

    #endregion

    #region " display alert "

    /// <summary>   Gets the alert annunciator. </summary>
    /// <value> The alert annunciator. </value>
    public System.Windows.Forms.Control AlertAnnunciator => this._alertsPictureBox;

    /// <summary> Shows the alerts. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="show">   true to show, false to hide. </param>
    /// <param name="isGood"> true if this object is good. </param>
    public void ShowAlerts( bool show, bool isGood )
    {
        if ( this.InvokeRequired )
        {
            _ = this.Invoke( new Action<bool, bool>( this.ShowAlerts ), [show, isGood] );
        }
        else
        {
            this._alertsPictureBox.Image = show
                ? isGood
                    ? GetResourceImage( nameof( Ttm.Controls.Properties.Resources.Good ), this._alertsPictureBox.Image )
                    : GetResourceImage( nameof( Ttm.Controls.Properties.Resources.Bad ), this._alertsPictureBox.Image )
                : null;
        }
    }

    /// <summary> Shows the outcome. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="show">   true to show, false to hide. </param>
    /// <param name="isGood"> true if this object is good. </param>
    public void ShowOutcome( bool show, bool isGood )
    {
        if ( this.InvokeRequired )
            _ = this.Invoke( new Action<bool, bool>( this.ShowOutcome ), [show, isGood] );
        else
        {
            this._outcomePictureBox.Image = show
                ? isGood
                    ? GetResourceImage( nameof( Ttm.Controls.Properties.Resources.Good ), this._outcomePictureBox.Image )
                    : GetResourceImage( nameof( Ttm.Controls.Properties.Resources.Bad ), this._outcomePictureBox.Image )
                : null;
        }
    }

    #endregion

    #region " display value "

    /// <summary> Clears this object to its blank/initial state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
#if NET5_0_OR_GREATER
#else
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0058:Remove unnecessary expression value", Justification = "incorrect.  Invoke(Action) is void." )]
#endif
    public void Clear()
    {
        if ( this.InvokeRequired )
            this.Invoke( new Action( this.Clear ) );
        else
        {
            this.InfoProvider?.Clear();
            this.ShowOutcome( false, false );
            this.ShowAlerts( false, false );
            this._initialResistanceTextBox.Text = string.Empty;
            this._finalResistanceTextBox.Text = string.Empty;
            this._thermalTransientVoltageTextBox.Text = string.Empty;
        }
    }

    /// <summary> Displays the thermal transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="textBox">    The text box control. </param>
    /// <param name="resistance"> The resistance. </param>
    private void SetErrorProvider( TextBox textBox, MeasureBase resistance )
    {
        if ( (resistance.MeasurementOutcome & MeasurementOutcomes.PartFailed) != 0 )
        {
            this.InfoProvider?.SetIconPadding( textBox, -this.InfoProvider.Icon.Width );
            this.InfoProvider?.SetError( textBox, "Value out of range" );
        }
        else if ( (resistance.MeasurementOutcome & MeasurementOutcomes.MeasurementFailed) != 0 )
        {
            this.InfoProvider?.SetIconPadding( textBox, -this.InfoProvider.Icon.Width );
            this.InfoProvider?.SetError( textBox, "Measurement failed" );
        }
        else if ( (resistance.MeasurementOutcome & MeasurementOutcomes.MeasurementNotMade) != 0 )
        {
            this.InfoProvider?.SetIconPadding( textBox, -this.InfoProvider.Icon.Width );
            this.InfoProvider?.SetError( textBox, "Measurement not made" );
        }
        else
        {
            this.InfoProvider?.SetError( textBox, "" );
        }
    }

    /// <summary> Displays the resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="textBox"> The text box control. </param>
    private void ClearResistance( TextBox textBox )
    {
        if ( textBox is not null )
        {
            if ( textBox.InvokeRequired )
            {
                _ = textBox.Invoke( new Action<TextBox>( this.ClearResistance ), textBox );
            }
            else
            {
                textBox.Text = string.Empty;
                this.InfoProvider?.SetError( textBox, "" );
            }
        }
    }

    /// <summary> Displays the resistance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="textBox">    The text box control. </param>
    /// <param name="resistance"> The resistance. </param>
    private void ShowResistance( TextBox textBox, MeasureBase resistance )
    {
        if ( textBox is not null )
        {
            if ( textBox.InvokeRequired )
            {
                _ = textBox.Invoke( new Action<TextBox, MeasureBase>( this.ShowResistance ), [textBox, resistance] );
            }
            else
            {
                textBox.Text = resistance.MeasuredValueCaption;
                this.SetErrorProvider( textBox, resistance );
            }
        }
    }

    /// <summary> Displays the thermal transient. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="textBox">    The text box control. </param>
    /// <param name="resistance"> The resistance. </param>
    private void ShowThermalTransient( TextBox textBox, MeasureBase resistance )
    {
        if ( textBox is not null )
        {
            if ( textBox.InvokeRequired )
            {
                _ = textBox.Invoke( new Action<TextBox, MeasureBase>( this.ShowThermalTransient ), [textBox, resistance] );
            }
            else
            {
                textBox.Text = resistance.MeasuredValueCaption;
                this.SetErrorProvider( textBox, resistance );
            }
        }
    }

    #endregion

    #region " part: initial resistance "

#if NET9_0_OR_GREATER

#endif

    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>" )]
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

    /// <summary> Executes the initial resistance property changed action. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnInitialResistancePropertyChanged( ColdResistance sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( ColdResistance.MeasurementAvailable ):
                {
                    if ( sender.MeasurementAvailable )
                        this.ShowResistance( this._initialResistanceTextBox, sender );
                    break;
                }

            case nameof( ColdResistance.Reading ):
                {
                    if ( string.IsNullOrWhiteSpace( sender.Reading ) )
                        this.ClearResistance( this._initialResistanceTextBox );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _initialResistance for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void InitialResistance_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.InitialResistance_PropertyChanged ), [sender, e] );
            else if ( sender is ColdResistance s )
                this.OnInitialResistancePropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception handling property", $"Exception handling '{e.PropertyName}' property change. {ex}." );
        }
    }

    #endregion

    #region " part: final resistance "

#if NET9_0_OR_GREATER

#endif

    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>" )]
    private ColdResistance? FinalResistanceInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( field is not null )
                field.PropertyChanged -= this.FinalResistance_PropertyChanged;

            field = value;
            if ( field is not null )
                field.PropertyChanged += this.FinalResistance_PropertyChanged;
        }
    }

    /// <summary> Executes the Final resistance property changed action. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnFinalResistancePropertyChanged( ColdResistance sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( ColdResistance.MeasurementAvailable ):
                {
                    if ( sender.MeasurementAvailable )
                        this.ShowResistance( this._finalResistanceTextBox, sender );
                    break;
                }

            case nameof( ColdResistance.Reading ):
                {
                    if ( string.IsNullOrWhiteSpace( sender.Reading ) )
                        this.ClearResistance( this._finalResistanceTextBox );
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Event handler. Called by _finalResistance for property changed events. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender"> The source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void FinalResistance_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null ) return;
        try
        {
            if ( this.InvokeRequired )
                _ = this.Invoke( new Action<object, PropertyChangedEventArgs>( this.FinalResistance_PropertyChanged ), [sender, e] );
            else if ( sender is ColdResistance s )
                this.OnFinalResistancePropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception handling property", $"Exception handling '{e.PropertyName}' property change. {ex}." );
        }
    }

    #endregion

    #region " part: thermal transient "

#if NET9_0_OR_GREATER

#endif

    /// <summary> The Part Thermal Transient. </summary>
    [field: System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0032:Use auto property", Justification = "preview; not preferred time." )]
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

    /// <summary> Executes the initial resistance property changed action. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( MeasureBase sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( MeasureBase.MeasurementAvailable ):
                {
                    if ( sender.MeasurementAvailable )
                        this.ShowThermalTransient( this._thermalTransientVoltageTextBox, sender );
                    break;
                }

            case nameof( MeasureBase.Reading ):
                {
                    if ( string.IsNullOrWhiteSpace( sender.Reading ) )
                        this.ClearResistance( this._thermalTransientVoltageTextBox );
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
            else
                if ( sender is MeasureBase s ) this.OnPropertyChanged( s, e.PropertyName! );
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception handling property", $"Exception handling '{e.PropertyName}' property change. {ex}." );
        }
    }

    #endregion
}
