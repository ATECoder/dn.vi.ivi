using System;
using System.ComponentModel;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> Part header. </summary>
/// <remarks>
/// (c) 2014 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2014-04-12 </para>
/// </remarks>
public partial class PartHeader : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public PartHeader() : base()
    {
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this._partNumberTextBox.Text = string.Empty;
        this._partSerialNumberTextBox.Text = string.Empty;
    }

    /// <summary> Releases the resources. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    private void ReleaseResources()
    {
        this.DeviceUnderTestInternal = null;
    }

    /// <summary>   UserControl overrides dispose to clean up the component list. </summary>
    /// <remarks>   David, 2021-09-07. </remarks>
    /// <param name="disposing">    true to release both managed and unmanaged resources; false to
    ///                             release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( disposing )
            {
                this.ReleaseResources();
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " dut "

    /// <summary>   Gets or sets the device under test internal. </summary>
    /// <value> The device under test internal. </value>
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
        set => this.DeviceUnderTestInternal = value;
    }

    /// <summary> Executes the property changed action. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="sender">       The source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void OnPropertyChanged( DeviceUnderTest sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName )
        {
            case nameof( Ttm.DeviceUnderTest.PartNumber ):
                {
                    if ( string.IsNullOrWhiteSpace( sender.PartNumber ) )
                        this.Visible = false;
                    else
                    {
                        this.Visible = true;
                        this._partNumberTextBox.Text = sender.PartNumber;
                    }

                    break;
                }

            case nameof( Ttm.DeviceUnderTest.SerialNumber ):
                {
                    this._partSerialNumberTextBox.Text = $"{sender.SerialNumber}";
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
            else if ( sender is Ttm.DeviceUnderTest s )
                this.OnPropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            Debug.Assert( !Debugger.IsAttached, "Exception handling property", $"Exception handling '{e.PropertyName}' property change. {ex}." );
        }
    }

    #endregion
}
